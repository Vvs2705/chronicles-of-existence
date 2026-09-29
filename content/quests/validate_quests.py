#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Validador dos dados de missao do COE (content/quests/*.json).

Roda sem dependencia externa:  PYTHONUTF8=1 python content/quests/validate_quests.py

O que ele cobra (fonte: docs/backlog/BACKLOG_v1_1.md "Testes obrigatorios",
docs/direcao/DOSSIE_CONTINUIDADE_v1_0.md secao M, docs/design/SLICE_A_PRIMEIRA_EXISTENCIA.md):

  1. id de missao unico, em snake_case, igual ao nome do arquivo
  2. id de objetivo unico dentro da missao, em snake_case
  3. toda pre-condicao aponta para missao/objetivo que existe
  4. nenhum ciclo de dependencia entre missoes
  5. toda missao central e alcancavel a partir do inicio (sem beco sem saida)
  6. missao opcional NUNCA e pre-condicao de missao central (exploit 6 do backlog)
  7. todo id de transacao de recompensa e unico em todo o conteudo (idempotencia)
  8. todo NPC citado esta na lista dos 10 de Auren (dossie secao G)
  9. toda ancora citada esta na lista declarada em _schema.json (contrato com a T008)
 10. desfechos (se houver) sao 2+ eventos de registra_no_historico, distintos do de conclusao
 11. nome de flag em snake_case e unico em todo o conteudo (flag e nome de UM evento)

Isto valida DADOS, nao jogo: nada aqui prova que a missao funciona na Unity.
"""

import json
import os
import re
import sys

AQUI = os.path.dirname(os.path.abspath(__file__))
ID_SIMPLES = re.compile(r"^[a-z][a-z0-9_]*$")


def id_pontuado_ok(valor):
    """'rec.q01_x.slug' / 'evento.q01_concluida' / 'item.cesto_de_vime': cada segmento em snake_case."""
    partes = valor.split(".")
    return len(partes) >= 2 and all(ID_SIMPLES.match(p) for p in partes)


def carregar(erros):
    schema_path = os.path.join(AQUI, "_schema.json")
    with open(schema_path, encoding="utf-8") as f:
        schema = json.load(f)

    ancoras = set(schema["ancoras_validas"]["existentes_t008"]) | set(
        schema["ancoras_validas"]["pedido_t012"]
    )
    missoes = {}
    for nome in sorted(os.listdir(AQUI)):
        if not nome.endswith(".json") or nome.startswith("_"):
            continue
        caminho = os.path.join(AQUI, nome)
        try:
            with open(caminho, encoding="utf-8") as f:
                dado = json.load(f)
        except ValueError as e:
            erros.append("%s: JSON invalido: %s" % (nome, e))
            continue
        qid = dado.get("id", "")
        if nome[:-5] != qid:
            erros.append("%s: campo id ('%s') difere do nome do arquivo." % (nome, qid))
        if qid in missoes:
            erros.append("%s: id de missao duplicado '%s'." % (nome, qid))
        missoes[qid] = dado
    return schema, ancoras, missoes


def checar_missao(q, ancoras, npcs_validos, schema, erros, transacoes, flags):
    qid = q.get("id", "?")

    if not ID_SIMPLES.match(qid):
        erros.append("%s: id de missao nao esta em snake_case." % qid)
    if q.get("tipo") not in schema["tipos_validos"]:
        erros.append("%s: tipo '%s' fora de tipos_validos." % (qid, q.get("tipo")))
    if not isinstance(q.get("central"), bool):
        erros.append("%s: campo 'central' precisa ser booleano." % qid)

    pre = q.get("precondicoes", {})
    if pre.get("fase") not in schema["fases_validas"]:
        erros.append("%s: fase '%s' fora de fases_validas." % (qid, pre.get("fase")))

    # ancora da missao e dos objetivos
    for ancora, onde in [(q.get("ancora"), "missao")] + [
        (o.get("ancora"), "objetivo " + str(o.get("id"))) for o in q.get("objetivos", [])
    ]:
        if ancora not in ancoras:
            erros.append("%s: ancora desconhecida '%s' (%s)." % (qid, ancora, onde))

    # NPCs
    npcs_missao = set(q.get("npcs", []))
    for n in npcs_missao:
        if n not in npcs_validos:
            erros.append("%s: NPC '%s' nao esta na lista dos 10 de Auren." % (qid, n))

    # objetivos
    vistos = set()
    if not q.get("objetivos"):
        erros.append("%s: missao sem objetivos." % qid)
    for o in q.get("objetivos", []):
        oid = o.get("id", "")
        if not ID_SIMPLES.match(oid):
            erros.append("%s: id de objetivo '%s' nao esta em snake_case." % (qid, oid))
        if oid in vistos:
            erros.append("%s: id de objetivo duplicado '%s'." % (qid, oid))
        vistos.add(oid)
        for n in o.get("npcs", []):
            if n not in npcs_validos:
                erros.append("%s/%s: NPC '%s' nao esta na lista dos 10." % (qid, oid, n))
            elif n not in npcs_missao:
                erros.append(
                    "%s/%s: NPC '%s' aparece no objetivo mas nao na lista da missao."
                    % (qid, oid, n)
                )

    # recompensas: id de transacao unico em TODO o conteudo
    for r in q.get("recompensas", []):
        tid = r.get("id_transacao", "")
        if not id_pontuado_ok(tid):
            erros.append("%s: id_transacao '%s' fora do formato snake_case pontuado." % (qid, tid))
        if not tid.startswith("rec." + qid + "."):
            erros.append("%s: id_transacao '%s' nao usa o prefixo 'rec.<quest_id>.'." % (qid, tid))
        if tid in transacoes:
            erros.append(
                "id_transacao duplicado '%s' (em %s e %s): recompensa deixa de ser idempotente."
                % (tid, transacoes[tid], qid)
            )
        else:
            transacoes[tid] = qid
        if r.get("tipo") not in schema["tipos_de_recompensa"]:
            erros.append("%s: tipo de recompensa '%s' invalido." % (qid, r.get("tipo")))
        if r.get("tipo") == "moedas" and r.get("alvo") != "":
            erros.append("%s: recompensa de moedas nao deve ter 'alvo'." % qid)
        if r.get("tipo") in ("item", "marco") and not id_pontuado_ok(r.get("alvo", "")):
            erros.append("%s: alvo de recompensa '%s' invalido." % (qid, r.get("alvo")))

    # evento de conclusao e historico
    if not id_pontuado_ok(q.get("evento_de_conclusao", "")):
        erros.append("%s: evento_de_conclusao invalido." % qid)
    eventos_gravados = {h.get("evento") for h in q.get("registra_no_historico", [])}
    if q.get("evento_de_conclusao") not in eventos_gravados:
        erros.append(
            "%s: evento_de_conclusao '%s' nao aparece em registra_no_historico."
            % (qid, q.get("evento_de_conclusao"))
        )
    for h in q.get("registra_no_historico", []):
        if not id_pontuado_ok(h.get("evento", "")):
            erros.append("%s: evento de historico '%s' invalido." % (qid, h.get("evento")))
        for n in h.get("npcs", []):
            if n not in npcs_validos:
                erros.append("%s: historico cita NPC '%s' fora da lista dos 10." % (qid, n))
        for fl in h.get("flags", []):
            if not ID_SIMPLES.match(fl):
                erros.append("%s: flag '%s' nao esta em snake_case." % (qid, fl))
            elif fl in flags:
                erros.append("flag '%s' repetida (%s e %s): flag nomeia UM evento." % (fl, flags[fl], qid))
            else:
                flags[fl] = qid

    # desfechos: exatamente um sera gravado em jogo, entao precisam ser alternativas reais
    desfechos = q.get("desfechos", [])
    if desfechos:
        if len(desfechos) < 2 or len(set(desfechos)) != len(desfechos):
            erros.append("%s: 'desfechos' precisa de 2+ eventos distintos." % qid)
        for ev in desfechos:
            if ev not in eventos_gravados or ev == q.get("evento_de_conclusao"):
                erros.append(
                    "%s: desfecho '%s' tem de estar em registra_no_historico e nao ser o de conclusao."
                    % (qid, ev)
                )

    if not q.get("se_ignorada"):
        erros.append("%s: falta 'se_ignorada' (o que acontece se o jogador ignorar)." % qid)
    elif not q.get("central") and "OPCIONAL" not in q["se_ignorada"]:
        erros.append(
            "%s: missao opcional precisa dizer em 'se_ignorada' que a campanha segue sem ela." % qid
        )


def checar_grafo(missoes, erros):
    # 3. pre-condicoes apontam para o que existe
    eventos_existentes = set()
    for q in missoes.values():
        for h in q.get("registra_no_historico", []):
            eventos_existentes.add(h.get("evento"))

    for qid, q in missoes.items():
        pre = q.get("precondicoes", {})
        for dep in pre.get("missoes_concluidas", []):
            if dep not in missoes:
                erros.append("%s: pre-condicao aponta para missao inexistente '%s'." % (qid, dep))
            elif dep == qid:
                erros.append("%s: missao depende de si mesma." % qid)
        for ref in pre.get("objetivos_concluidos", []):
            if "." not in ref:
                erros.append("%s: pre-condicao de objetivo '%s' sem formato <quest>.<obj>." % (qid, ref))
                continue
            dq, _, do = ref.partition(".")
            if dq not in missoes:
                erros.append("%s: pre-condicao cita missao inexistente '%s'." % (qid, dq))
            elif do not in {o.get("id") for o in missoes[dq].get("objetivos", [])}:
                erros.append("%s: pre-condicao cita objetivo inexistente '%s'." % (qid, ref))
            elif dq not in pre.get("missoes_concluidas", []):
                erros.append(
                    "%s: exige o objetivo '%s' mas nao exige a missao '%s' concluida." % (qid, ref, dq)
                )
        for ev in pre.get("eventos_de_vida", []):
            if ev not in eventos_existentes:
                erros.append("%s: pre-condicao exige evento '%s' que nenhuma missao grava." % (qid, ev))

    # 4. ciclos (DFS com pilha)
    estado = {}

    def visita(n, caminho):
        estado[n] = 1
        for dep in missoes[n].get("precondicoes", {}).get("missoes_concluidas", []):
            if dep not in missoes:
                continue
            if estado.get(dep) == 1:
                erros.append("ciclo de dependencia: %s -> %s" % (" -> ".join(caminho + [n]), dep))
            elif estado.get(dep, 0) == 0:
                visita(dep, caminho + [n])
        estado[n] = 2

    for qid in sorted(missoes):
        if estado.get(qid, 0) == 0:
            visita(qid, [])

    # 5. alcancabilidade: ponto fixo a partir das missoes sem pre-requisito
    alcancaveis = set()
    mudou = True
    while mudou:
        mudou = False
        for qid, q in missoes.items():
            if qid in alcancaveis:
                continue
            deps = q.get("precondicoes", {}).get("missoes_concluidas", [])
            if all(d in alcancaveis for d in deps):
                alcancaveis.add(qid)
                mudou = True
    for qid, q in missoes.items():
        if q.get("central") and qid not in alcancaveis:
            erros.append("%s: missao CENTRAL inalcancavel a partir do inicio (beco sem saida)." % qid)

    # 6. opcional nunca e pre-condicao de central
    for qid, q in missoes.items():
        if not q.get("central"):
            continue
        for dep in q.get("precondicoes", {}).get("missoes_concluidas", []):
            if dep in missoes and not missoes[dep].get("central"):
                erros.append(
                    "%s (central) depende da missao OPCIONAL '%s' — exploit 6 do backlog." % (qid, dep)
                )

    return alcancaveis


def main():
    erros = []
    schema, ancoras, missoes = carregar(erros)
    npcs_validos = set(schema["npcs_validos"])
    transacoes = {}
    flags = {}

    for qid in sorted(missoes):
        checar_missao(missoes[qid], ancoras, npcs_validos, schema, erros, transacoes, flags)
    alcancaveis = checar_grafo(missoes, erros)

    centrais = sorted(q for q in missoes if missoes[q].get("central"))
    opcionais = sorted(q for q in missoes if not missoes[q].get("central"))

    print("COE — validador de missoes (content/quests)")
    print("  missoes lidas .......... %d (%d centrais, %d opcionais)"
          % (len(missoes), len(centrais), len(opcionais)))
    print("  objetivos .............. %d" % sum(len(q.get("objetivos", [])) for q in missoes.values()))
    print("  ids de transacao ....... %d (todos unicos)" % len(transacoes))
    print("  flags .................. %d (todas unicas)" % len(flags))
    print("  ancoras declaradas ..... %d (%d ja na cena T008, %d pedidas a T008)"
          % (len(ancoras), len(schema["ancoras_validas"]["existentes_t008"]),
             len(schema["ancoras_validas"]["pedido_t012"])))
    print("  centrais alcancaveis ... %d/%d"
          % (sum(1 for c in centrais if c in alcancaveis), len(centrais)))

    if len(centrais) != 5 or len(opcionais) != 3:
        print("  AVISO: o slice pede 5 centrais e 3 opcionais (dossie secao L).")

    if erros:
        print("\nFALHOU — %d problema(s):" % len(erros))
        for e in erros:
            print("  - " + e)
        return 1
    print("\nOK — 11 verificacoes passaram. (Valida DADOS; nao prova nada na Unity.)")
    return 0


if __name__ == "__main__":
    sys.exit(main())
