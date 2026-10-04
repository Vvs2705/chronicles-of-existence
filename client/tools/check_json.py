#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""Confere os JSON de conteudo do COE: sintaxe valida e nenhuma chave repetida no mesmo objeto.

Por que: o jogo le strings.pt-BR.json por regex (Scripts/Loc/Strings.cs), que aceita JSON quebrado e, com chave repetida,
fica calado com a ULTIMA. Aqui isso reprova. So biblioteca padrao.
Uso: python client/tools/check_json.py            (confere os arquivos do repositorio)
     python client/tools/check_json.py --autoteste
Saida 0 = ok; 1 = algum arquivo invalido.
"""

import glob
import json
import os
import sys

RAIZ = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
PADROES = [
    "client/Assets/_COE/Resources/*.json",
    "content/quests/*.json",
]


class ChaveRepetida(ValueError):
    pass


def _sem_repetida(pares):
    vistos = {}
    for k, v in pares:
        if k in vistos:
            raise ChaveRepetida("chave repetida: %r" % k)
        vistos[k] = v
    return vistos


def conferir_texto(texto):
    """None se ok, senao o motivo."""
    try:
        json.loads(texto, object_pairs_hook=_sem_repetida)
        return None
    except (ValueError, ChaveRepetida) as e:
        return str(e)


def main():
    arquivos = sorted(f for p in PADROES for f in glob.glob(os.path.join(RAIZ, p)))
    if not arquivos:
        print("nenhum JSON encontrado (raiz %s)" % RAIZ)
        return 1
    ruins = 0
    for f in arquivos:
        with open(f, encoding="utf-8-sig") as h:
            motivo = conferir_texto(h.read())
        rel = os.path.relpath(f, RAIZ)
        if motivo:
            ruins += 1
            print("FALHOU %s: %s" % (rel, motivo))
    print("%s: %d arquivos JSON, %d com problema" % ("OK" if ruins == 0 else "FALHOU", len(arquivos), ruins))
    return 1 if ruins else 0


def autoteste():
    assert conferir_texto('{"a": 1, "b": {"c": 2}}') is None
    assert "repetida" in conferir_texto('{"a": 1, "a": 2}')
    assert "repetida" in conferir_texto('{"x": {"k": 1, "k": 1}}')
    assert conferir_texto('{"a": 1,}') is not None, "virgula sobrando reprova"
    assert conferir_texto('{"t": "acentuação e \\"aspas\\"\\n"}') is None
    print("autoteste ok")
    return 0


if __name__ == "__main__":
    sys.exit(autoteste() if sys.argv[1:] == ["--autoteste"] else main())
