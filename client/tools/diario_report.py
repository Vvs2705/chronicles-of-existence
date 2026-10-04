"""Resumo de um diario de sessao do playtest (client/Assets/_COE/Scripts/Core/DiarioDeSessao.cs).

Uso:
  python client/tools/diario_report.py [sessao_*.txt | pasta] [--md saida.md]
  python client/tools/diario_report.py --autoteste

Sem argumento le a pasta do diario deste PC (%USERPROFILE%\\AppData\\LocalLow\\V-STACK\\Chronicles of Existence\\diario).
Pasta = o sessao_*.txt mais novo dela (pelo nome, como a rotacao do jogo).

Contrato (docs/qa/PLAYTEST.md §6): "mm:ss<TAB>tipo<TAB>detalhe", tempo de relogio desde o inicio; mm passa de 59 (75:03).
Le: duracao total e ativa (sem pausa->volta), entrada em Auren (1a linha da q01), tempo ativo por missao (em_andamento ->
concluida/falhada), desfechos da q04 e da q07, salto (marco_idade_8), fim do slice e lacunas de mais de 1 min sem
progresso (objetivo, missao, evento, idade), fora de pausa; acima de 3 min em destaque.
Tolerante: tipo desconhecido e contado e ignorado; sem "fim" (app morto) termina na ultima linha e sai marcado.
Sai com codigo 2 se o arquivo nao e um diario. So biblioteca padrao.
"""
import argparse
import os
import sys

TIPOS = {"inicio", "objetivo", "missao", "evento", "periodo", "idade", "pausa", "volta", "fim"}
PROGRESSO = {"objetivo", "missao", "evento", "idade"}
LACUNA_S = 60
TRAVOU_S = 180   # [PROPOSTA] do PLAYTEST.md §6
SALTO = "marco_idade_8"
FIM_DO_SLICE = "marco.fim_da_primeira_existencia"
PASTA_DO_PC = os.path.join(os.path.expanduser("~"), "AppData", "LocalLow", "V-STACK", "Chronicles of Existence", "diario")


class DiarioInvalido(Exception):
    pass


def segundos(mmss):
    try:
        m, s = mmss.strip().split(":")
        return int(m) * 60 + int(s)
    except ValueError:
        raise DiarioInvalido("tempo fora de mm:ss: %r" % mmss)


def mmss(s):
    return "%02d:%02d" % divmod(int(s), 60)


def ler(texto):
    linhas = []
    for n, l in enumerate(texto.splitlines(), start=1):
        if not l.strip():
            continue
        p = l.split("\t", 2)
        if len(p) < 2:
            raise DiarioInvalido("linha %d sem TAB: %r" % (n, l))
        linhas.append((segundos(p[0]), p[1].strip(), p[2].strip() if len(p) > 2 else ""))
    if not linhas or linhas[0][1] != "inicio":
        raise DiarioInvalido("a primeira linha nao e 'inicio'")
    return linhas


def resumir(linhas):
    fim = [d for t, tp, d in linhas if tp == "fim"]
    total = segundos(fim[-1]) if fim else linhas[-1][0]

    pausas, desde = [], None
    for t, tp, _ in linhas:
        if tp == "pausa" and desde is None:
            desde = t
        elif tp == "volta" and desde is not None:
            pausas.append((desde, t))
            desde = None
    if desde is not None:
        pausas.append((desde, total))   # pausa sem volta: parado ate o fim

    def ativo(a, b):
        return (b - a) - sum(max(0, min(b, fe) - max(a, ps)) for ps, fe in pausas)

    def primeiro(tp, detalhe):
        return next((t for t, x, d in linhas if x == tp and d == detalhe), None)

    missoes = {}   # questId -> [inicio, fim, ultimo status], na ordem em que aparecem
    for t, tp, d in linhas:
        if tp == "objetivo":
            m = missoes.setdefault(d.split("/")[0], [None, None, "?"])
            if m[0] is None:
                m[0] = t   # concluiu objetivo sem em_andamento antes
        elif tp == "missao":
            q, _, st = d.partition(" ")
            m = missoes.setdefault(q, [None, None, st])
            m[2] = st
            if st == "em_andamento" and m[0] is None:
                m[0] = t
            if st in ("concluida", "falhada") and m[1] is None:
                m[1] = t
    for m in missoes.values():
        m.append(ativo(m[0], m[1]) if m[0] is not None and m[1] is not None else None)

    def em_andamento(t):
        return [q for q, m in missoes.items() if m[0] is not None and m[0] <= t and (m[1] is None or m[1] > t)]

    fim_do_slice = primeiro("evento", FIM_DO_SLICE)
    prog = [(t, tp, d) for t, tp, d in linhas[1:] if tp in PROGRESSO]
    pares = list(zip(prog, prog[1:]))
    if prog and fim_do_slice is None:
        pares.append((prog[-1], (total, "fim", "fim da sessao")))   # parou sem chegar ao gancho
    lacunas = []
    for (a, ta, da), (b, tb, db) in pares:
        dur = ativo(a, b)
        if dur > LACUNA_S:
            lacunas.append({"de": a, "ate": b, "ativo": dur, "antes": "%s %s" % (ta, da), "depois": "%s %s" % (tb, db),
                            "pendente": em_andamento(a), "travou": dur > TRAVOU_S})

    entrada = next((t for t, tp, d in linhas if tp in ("objetivo", "missao") and d.startswith("q01_")), None)
    avisos = []
    if "save=continuado" in linhas[0][2]:
        avisos.append("inicio com save=continuado: o save anterior nao foi apagado; a sessao nao serve para a H1")
    if not fim:
        avisos.append("sem 'fim' (jogo fechado a forca ou caiu): o total vai ate a ultima linha")
    nova_vida = [t for t, tp, d in linhas[1:] if tp == "idade" and d == "5"]
    if nova_vida:
        avisos.append("Nova vida em %s: dai em diante e outra partida; separe as contas" % mmss(nova_vida[0]))
    desconhecidos = sorted({tp for _, tp, _ in linhas if tp not in TIPOS})
    if desconhecidos:
        n = sum(1 for _, tp, _ in linhas if tp not in TIPOS)
        avisos.append("%d linha(s) de tipo desconhecido ignorada(s): %s" % (n, ", ".join(desconhecidos)))

    return {
        "inicio": linhas[0][2], "total": total, "sem_fim": not fim, "pausas": pausas, "ativo": ativo(0, total),
        "entrada_auren": entrada, "missoes": missoes,
        "desfechos": [(t, d) for t, tp, d in linhas if tp == "evento" and d.startswith(("evento.q04_", "evento.q07_"))
                      and not d.endswith("_concluida")],
        "salto": primeiro("evento", SALTO), "fim_do_slice": fim_do_slice,
        "slice_ativo": ativo(0, fim_do_slice) if fim_do_slice is not None else None,
        "lacunas": lacunas, "avisos": avisos,
    }


def relatorio(r, nome, md=False):
    c = (lambda x: "`%s`" % x) if md else (lambda x: x)
    t = lambda s: "n/d" if s is None else mmss(s)
    out = ["# Diario: %s" % nome if md else "Diario: %s" % nome, ""]
    sec = lambda titulo: out.extend(["", ("## " if md else "== ") + titulo, ""])
    out += [
        "- inicio: %s" % c(r["inicio"]),
        "- duracao total: %s%s" % (t(r["total"]), " (SEM FIM: ultima linha)" if r["sem_fim"] else ""),
        "- duracao ativa (sem pausas): %s (%d pausa(s), %s parado)" % (
            t(r["ativo"]), len(r["pausas"]), t(r["total"] - r["ativo"])),
        "- entrada em Auren (1a linha da q01; antes disso, o prologo B01-B05): %s" % t(r["entrada_auren"]),
        "- salto (%s): %s" % (c(SALTO), t(r["salto"])),
        "- fim do slice (%s): %s · duracao do slice sem pausas (H1): %s" % (
            c(FIM_DO_SLICE), t(r["fim_do_slice"]), t(r["slice_ativo"])),
    ]
    for a in r["avisos"]:
        out.append(("- **AVISO:** %s" if md else "- AVISO: %s") % a)
    sec("Desfechos (q04, q07)")
    out += ["- %s %s" % (t(s), c(d)) for s, d in r["desfechos"]] or ["- nenhum"]
    sec("Missoes (tempo ativo, de em_andamento a concluida/falhada)")
    for q, (ini, fim, st, dur) in r["missoes"].items():
        out.append("- %s: %s (%s -> %s) · status final %s" % (
            c(q), t(dur) if dur is not None else "nao terminou", t(ini), t(fim), st))
    sec("Lacunas sem progresso > %d min, fora de pausa (acima de %d min: candidata a travou)" % (LACUNA_S // 60, TRAVOU_S // 60))
    for g in r["lacunas"]:
        marca = ("**TRAVOU?** " if md else ">> TRAVOU? ") if g["travou"] else ""
        out.append("- %s%s -> %s (%s ativos): depois de %s, ate %s · em andamento: %s" % (
            marca, t(g["de"]), t(g["ate"]), t(g["ativo"]), c(g["antes"]), c(g["depois"]),
            ", ".join(c(q) for q in g["pendente"]) or "nenhuma"))
    if not r["lacunas"]:
        out.append("- nenhuma")
    return "\n".join(out) + "\n"


EXEMPLO = (
    "00:00\tinicio\tversao=0.1 cena=Bootstrap save=novo idade=5 periodo=manha\n"
    "03:10\tmissao\tq01_um_novo_amanhecer em_andamento\n"
    "05:00\tobjetivo\tq01_um_novo_amanhecer/falar_com_familia\n"
    "06:30\tobjetivo\tq01_um_novo_amanhecer/sair_de_casa\n"
    "06:30\tmissao\tq01_um_novo_amanhecer concluida\n"
    "06:30\tmissao\tq02_uma_pequena_responsabilidade em_andamento\n"
    "06:30\tmissao\tq05_o_animal_ferido disponivel\n"
    "06:30\tevento\tevento.q01_concluida\n"
    "08:00\tobjetivo\tq02_uma_pequena_responsabilidade/receber_tarefa\n"
    "12:10\tobjetivo\tq02_uma_pequena_responsabilidade/cumprir_tarefa\n"   # lacuna de 4:10: travou?
    "12:40\tperiodo\ttarde\n"
    "19:30\tmissao\tq06_o_segredo_do_ferreiro em_andamento\n"
    "20:00\tpausa\t\n"
    "30:00\tvolta\t\n"
    "31:00\tmissao\tq02_uma_pequena_responsabilidade concluida\n"          # 11:30 de relogio, 1:30 ativos
    "33:00\tgravacao_futura\tqualquer coisa\n"                              # tipo desconhecido
    "33:20\tmissao\tq04_uma_promessa em_andamento\n"
    "40:00\tevento\tevento.q04_promessa_quebrada\n"
    "40:00\tevento\tevento.q04_concluida\n"
    "40:00\tmissao\tq04_uma_promessa concluida\n"
    "55:00\tevento\tevento.q07_assinou_com_um_risco\n"
    "68:00\tmissao\tq06_o_segredo_do_ferreiro falhada\n"
    "68:00\tevento\tmarco_idade_8\n"
    "68:00\tidade\t8\n"
    "74:40\tevento\tmarco.fim_da_primeira_existencia\n"
    "75:03\tpausa\t\n"                                                      # app morto: sem "fim"
)


def _autoteste():
    assert segundos("75:03") == 4503 and mmss(4503) == "75:03", "minutos passam de 59"
    r = resumir(ler(EXEMPLO))
    assert r["sem_fim"] and r["total"] == 4503, r["total"]
    assert r["pausas"] == [(1200, 1800), (4503, 4503)], r["pausas"]
    assert r["ativo"] == 4503 - 600, "a pausa de 10 min sai da duracao ativa"
    assert r["entrada_auren"] == 190
    assert r["missoes"]["q01_um_novo_amanhecer"] == [190, 390, "concluida", 200]
    assert r["missoes"]["q02_uma_pequena_responsabilidade"][3] == 1860 - 390 - 600, "pausa fora do tempo da missao"
    assert r["missoes"]["q05_o_animal_ferido"] == [None, None, "disponivel", None]
    assert r["missoes"]["q06_o_segredo_do_ferreiro"][2] == "falhada"
    assert [d for _, d in r["desfechos"]] == ["evento.q04_promessa_quebrada", "evento.q07_assinou_com_um_risco"], r["desfechos"]
    assert r["salto"] == 4080 and r["fim_do_slice"] == 4480 and r["slice_ativo"] == 4480 - 600
    g = [(x["de"], x["ate"], x["ativo"], x["travou"]) for x in r["lacunas"]]
    assert (480, 730, 250, True) in g, "lacuna de 4:10 em destaque"
    assert (1170, 1860, 90, False) in g, "lacuna que atravessa a pausa conta so o tempo ativo"
    assert (390, 480, 90, False) in g and len(g) == 11 and sum(x[3] for x in g) == 6, g
    q02 = [x for x in r["lacunas"] if x["de"] == 480][0]
    assert q02["pendente"] == ["q02_uma_pequena_responsabilidade"] and "cumprir_tarefa" in q02["depois"], q02
    assert any("sem 'fim'" in a for a in r["avisos"]) and any("gravacao_futura" in a for a in r["avisos"]), r["avisos"]
    for md in (False, True):
        txt = relatorio(r, "sessao_x.txt", md)
        assert "75:03" in txt and "SEM FIM" in txt and "TRAVOU?" in txt, txt
    assert relatorio(r, "x", True).startswith("# Diario")
    r2 = resumir(ler("00:00\tinicio\tsave=continuado\n01:00\tperiodo\tmanha\n01:00\tidade\t5\n02:00\tfim\t02:00\n"))
    assert not r2["sem_fim"] and r2["total"] == 120 and r2["lacunas"] == [], r2
    assert any("save=continuado" in a for a in r2["avisos"]) and any("Nova vida em 01:00" in a for a in r2["avisos"])
    for ruim in ("", "01:00\tmissao\tq01 em_andamento\n", "00:00\tinicio\tx\nxx:10\tfim\t\n", "00:00 inicio\n"):
        try:
            resumir(ler(ruim))
            raise AssertionError("aceitou diario fora do contrato: %r" % ruim)
        except DiarioInvalido:
            pass
    print(relatorio(r, "EXEMPLO embutido"))
    print("autoteste ok")


def main(argv=None):
    if hasattr(sys.stdout, "reconfigure"):
        sys.stdout.reconfigure(encoding="utf-8")
    p = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    p.add_argument("diario", nargs="?", default=PASTA_DO_PC, help="sessao_*.txt ou pasta (pega o mais novo)")
    p.add_argument("--md", help="grava o relatorio em Markdown neste arquivo")
    p.add_argument("--autoteste", action="store_true")
    a = p.parse_args(argv)
    if a.autoteste:
        _autoteste()
        return 0
    caminho = a.diario
    if os.path.isdir(caminho):
        sessoes = sorted(f for f in os.listdir(caminho) if f.startswith("sessao_") and f.endswith(".txt"))
        if not sessoes:
            print("nenhum sessao_*.txt em %s" % caminho, file=sys.stderr)
            return 2
        caminho = os.path.join(caminho, sessoes[-1])
    try:
        with open(caminho, encoding="utf-8-sig") as f:
            r = resumir(ler(f.read()))
    except (OSError, DiarioInvalido) as e:
        print("diario invalido: %s" % e, file=sys.stderr)
        return 2
    nome = os.path.basename(caminho)
    if a.md:
        with open(a.md, "w", encoding="utf-8") as f:
            f.write(relatorio(r, nome, md=True))
    sys.stdout.write(relatorio(r, nome))
    return 0


if __name__ == "__main__":
    sys.exit(main())
