#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Resumo dos diarios de sessao do playtest (docs/qa/PLAYTEST.md §6). So biblioteca padrao.

Entrada: um ou mais sessao_*.txt, ou a pasta de uma rodada. Formato (Scripts/Core/DiarioDeSessao.cs):
    mm:ss<TAB>tipo<TAB>detalhe        (tempo de relogio desde o inicio; minutos podem passar de 59)
Saida por sessao: duracao total e ativa (menos pausas), duracao do slice (inicio -> marco.fim_da_primeira_existencia,
menos pausas), prologo (inicio -> primeira linha da q01), tempo por missao, desfechos da q04 e da q07, opcionais
comecadas e concluidas, descansos, e lacunas sem progresso acima de 1 min (marcadas acima de 3 min).
Saida por rodada: mediana e faixa (min-max) de cada numero, como pede a ficha 46 (rodada pequena: nunca media sozinha).

Uso: python client/tools/diario_report.py <sessao_*.txt | pasta> [...]
     python client/tools/diario_report.py --autoteste
Privacidade: o diario so tem ids do jogo, numeros e tempos; este script nao le nem escreve mais nada.
"""

import glob
import os
import statistics
import sys

PROGRESSO = {"objetivo", "missao", "evento", "idade"}
CONHECIDOS = PROGRESSO | {"inicio", "periodo", "pausa", "volta", "fim"}
MARCO_FIM = "marco.fim_da_primeira_existencia"
Q01 = "q01_um_novo_amanhecer"
# Opcionais do slice (content/quests: "central": false). Lidas dos JSON quando o script roda dentro do repositorio.
OPCIONAIS_PADRAO = {"q03_o_cesto_perdido", "q05_o_animal_ferido", "q06_o_segredo_do_ferreiro"}


def opcionais():
    pasta = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "..", "content", "quests")
    achadas = set()
    try:
        import json
        for f in glob.glob(os.path.join(pasta, "q*.json")):
            with open(f, encoding="utf-8") as h:
                d = json.load(h)
            if d.get("central") is False:
                achadas.add(d["id"])
    except (OSError, ValueError, KeyError):
        pass
    return achadas or OPCIONAIS_PADRAO


def segundos(mmss):
    m, s = mmss.split(":")
    return int(m) * 60 + int(s)


def mmss(seg):
    seg = int(round(seg))
    return "%02d:%02d" % (seg // 60, seg % 60)


def ler(linhas):
    """[(t, tipo, detalhe)] e quantas linhas foram ignoradas (tipo desconhecido ou linha quebrada)."""
    eventos, ignoradas = [], 0
    for bruta in linhas:
        partes = bruta.rstrip("\r\n").split("\t")
        if len(partes) < 2:
            if bruta.strip():
                ignoradas += 1
            continue
        try:
            t = segundos(partes[0])
        except ValueError:
            ignoradas += 1
            continue
        tipo, detalhe = partes[1], partes[2] if len(partes) > 2 else ""
        if tipo not in CONHECIDOS:
            ignoradas += 1
            continue
        eventos.append((t, tipo, detalhe))
    return eventos, ignoradas


def pausado(pausas, a, b):
    """Segundos de pausa dentro de [a, b]."""
    return sum(max(0, min(fim, b) - max(ini, a)) for ini, fim in pausas)


def resumir(eventos, opc):
    r = {"avisos": []}
    if not eventos or eventos[0][1] != "inicio":
        r["avisos"].append("sem linha de inicio")
    elif "save=continuado" in eventos[0][2]:
        r["avisos"].append("save continuado (nao comecou do nascimento)")
    ultimo = eventos[-1][0] if eventos else 0
    fim = next((e for e in eventos if e[1] == "fim"), None)
    if fim is None:
        r["avisos"].append("sem fim (processo morto ou app fechado): termina na ultima linha")
    total = segundos(fim[2]) if fim and fim[2] else ultimo

    pausas, aberta = [], None
    for t, tipo, _ in eventos:
        if tipo == "pausa" and aberta is None:
            aberta = t
        elif tipo == "volta" and aberta is not None:
            pausas.append((aberta, t))
            aberta = None
    if aberta is not None:
        pausas.append((aberta, total))
    r["total"] = total
    r["ativa"] = total - pausado(pausas, 0, total)

    # "Nova vida" no meio da sessao: periodo manha seguido de idade 5 depois do inicio = outra partida
    for i in range(1, len(eventos) - 1):
        if eventos[i][1] == "periodo" and eventos[i][2] == "manha" and eventos[i + 1][1] == "idade" and eventos[i + 1][2] == "5":
            r["avisos"].append("nova vida em %s: o que vem depois e outra partida (separe as contas)" % mmss(eventos[i][0]))
            break

    gancho = next((t for t, tipo, d in eventos if tipo == "evento" and d == MARCO_FIM), None)
    r["slice"] = None if gancho is None else gancho - pausado(pausas, 0, gancho)
    q01 = next((t for t, tipo, d in eventos if tipo in ("objetivo", "missao") and d.split("/")[0].split(" ")[0] == Q01), None)
    r["prologo"] = q01

    # Comecou = objetivo cumprido ou "em_andamento". Uma "falhada" sozinha (q03 no sumico do Nilo, opcional encerrada pelo
    # salto) e regra do jogo, nao missao que o jogador pegou (PLAYTEST.md §6).
    inicio_missao, concluida = {}, {}
    for t, tipo, d in eventos:
        q = d.split("/")[0].split(" ")[0]
        if tipo == "objetivo" or (tipo == "missao" and d.endswith(" em_andamento")):
            inicio_missao.setdefault(q, t)
        if tipo == "missao" and d.endswith(" concluida"):
            inicio_missao.setdefault(q, t)
            concluida.setdefault(q, t)
    r["missoes"] = {q: concluida[q] - inicio_missao[q] - pausado(pausas, inicio_missao[q], concluida[q])
                    for q in sorted(concluida)}
    r["opcionais_comecadas"] = sorted(q for q in inicio_missao if q in opc)
    r["opcionais_concluidas"] = sorted(q for q in concluida if q in opc)
    r["desfechos"] = sorted(d for t, tipo, d in eventos if tipo == "evento"
                            and (d.startswith("evento.q04_promessa_") or d.startswith("evento.q07_assinou_")))

    concluiu_em = {t for t, tipo, d in eventos if tipo == "missao" and d.endswith(" concluida")}
    r["descansos"] = sum(1 for t, tipo, _ in eventos if tipo == "periodo" and t not in concluiu_em and t > 0)

    progresso = [e for e in eventos if e[1] in PROGRESSO]
    lacunas, anterior = [], (eventos[0] if eventos else None)
    for e in progresso:
        if anterior is not None:
            dur = e[0] - anterior[0] - pausado(pausas, anterior[0], e[0])
            if dur > 60:
                lacunas.append((dur, anterior, e))
        anterior = e
    r["lacunas"] = lacunas
    return r


def imprimir(nome, r, ignoradas):
    print("== %s" % nome)
    for a in r["avisos"]:
        print("  aviso: " + a)
    if ignoradas:
        print("  linhas ignoradas (tipo desconhecido ou quebradas): %d" % ignoradas)
    print("  duracao total %s | ativa %s | slice %s | prologo %s" % (
        mmss(r["total"]), mmss(r["ativa"]), mmss(r["slice"]) if r["slice"] is not None else "nao chegou ao gancho",
        mmss(r["prologo"]) if r["prologo"] is not None else "-"))
    for q, s in r["missoes"].items():
        print("  missao %-36s %s" % (q, mmss(s)))
    print("  desfechos: %s" % (", ".join(r["desfechos"]) or "-"))
    print("  opcionais: comecou %d (%s), concluiu %d" % (len(r["opcionais_comecadas"]), ", ".join(r["opcionais_comecadas"]) or "-",
                                                       len(r["opcionais_concluidas"])))
    print("  descansos: %d" % r["descansos"])
    for dur, a, b in r["lacunas"]:
        print("  lacuna %s%s entre %s %s e %s %s" % (mmss(dur), " [>3 min: candidata a travamento]" if dur > 180 else "",
                                                     a[1], a[2], b[1], b[2]))


def rodada(resumos):
    print("== rodada: %d sessoes (mediana e faixa)" % len(resumos))
    for chave, nome in (("total", "duracao total"), ("ativa", "duracao ativa"), ("slice", "slice"), ("prologo", "prologo")):
        v = [r[chave] for r in resumos if r[chave] is not None]
        if v:
            print("  %-14s mediana %s, faixa %s-%s (%d de %d)" % (nome, mmss(statistics.median(v)), mmss(min(v)), mmss(max(v)),
                                                                len(v), len(resumos)))
    chegaram = sum(1 for r in resumos if r["slice"] is not None)
    print("  chegaram ao gancho: %d de %d" % (chegaram, len(resumos)))
    print("  fizeram ao menos uma opcional: %d de %d" % (sum(1 for r in resumos if r["opcionais_comecadas"]), len(resumos)))


def arquivos(args):
    r = []
    for a in args:
        r.extend(sorted(glob.glob(os.path.join(a, "sessao_*.txt"))) if os.path.isdir(a) else [a])
    return r


def main(args):
    fs = arquivos(args)
    if not fs:
        print(__doc__)
        return 2
    opc, resumos = opcionais(), []
    for f in fs:
        with open(f, encoding="utf-8") as h:
            eventos, ignoradas = ler(h)
        r = resumir(eventos, opc)
        imprimir(os.path.basename(f), r, ignoradas)
        resumos.append(r)
    if len(resumos) > 1:
        rodada(resumos)
    return 0


EXEMPLO = """00:00\tinicio\tversao=1.0 cena=Bootstrap save=novo idade=5 periodo=manha
01:00\tmissao\tq05_o_animal_ferido falhada
02:00\tobjetivo\tq01_um_novo_amanhecer/acordar
02:00\tmissao\tq01_um_novo_amanhecer em_andamento
03:30\tmissao\tq01_um_novo_amanhecer concluida
03:30\tperiodo\ttarde
04:00\tpausa\t
09:00\tvolta\t
09:10\tmissao\tq03_o_cesto_perdido em_andamento
13:40\tobjetivo\tq03_o_cesto_perdido/procurar_na_praca
13:50\tperiodo\tnoite
14:00\tevento\tevento.q04_promessa_cumprida
14:05\tdesconhecido\tqualquer coisa
20:00\tevento\tmarco.fim_da_primeira_existencia
20:30\tfim\t20:30
"""


def autoteste():
    eventos, ignoradas = ler(EXEMPLO.splitlines(True))
    assert ignoradas == 1, "tipo desconhecido e contado e ignorado"
    r = resumir(eventos, OPCIONAIS_PADRAO)
    assert r["total"] == segundos("20:30")
    assert r["ativa"] == segundos("20:30") - 300, "a pausa de 5 min sai da duracao ativa"
    assert r["slice"] == segundos("20:00") - 300
    assert r["prologo"] == segundos("02:00")
    assert "q05_o_animal_ferido" not in r["opcionais_comecadas"], "falhada sozinha nao e opcional comecada"
    assert r["missoes"] == {"q01_um_novo_amanhecer": 90}
    assert r["opcionais_comecadas"] == ["q03_o_cesto_perdido"] and r["opcionais_concluidas"] == []
    assert r["desfechos"] == ["evento.q04_promessa_cumprida"]
    assert r["descansos"] == 1, "periodo sem missao concluida junto e descanso"
    duracoes = [l[0] for l in r["lacunas"]]
    assert 270 in duracoes, "q03 em andamento -> objetivo: 4:30 sem progresso, pausa nao conta"
    assert 360 in duracoes, "evento -> gancho: 6 min sem progresso (acima de 3 min)"
    assert not any(l[0] == 330 for l in r["lacunas"]), "a pausa nao vira lacuna"
    sem_fim, _ = ler(EXEMPLO.splitlines(True)[:-1])
    assert any("sem fim" in a for a in resumir(sem_fim, OPCIONAIS_PADRAO)["avisos"])
    print("autoteste ok")
    return 0


if __name__ == "__main__":
    sys.exit(autoteste() if sys.argv[1:] == ["--autoteste"] else main(sys.argv[1:]))
