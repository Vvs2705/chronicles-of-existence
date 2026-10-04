"""Mata o COE.exe -roteiro em instantes aleatorios e confere o save do robo depois de cada morte (T014 R5: o processo
morto de verdade, nao so a regra). Build de desenvolvimento do PC; o save de quem joga nao e tocado (o robo usa
roteiro_save.json e apaga o dele ao abrir).

  python client/tools/save_caos.py [--rodadas 10] [--exe client/Builds/win/COE.exe] [--seed N]
  python client/tools/save_caos.py --autoteste

Rodada par: rota quebrada, morte na janela do salto (57-61 s do processo; a rota salta por volta de 58-60 s).
Rodada impar: rota completa, morte em qualquer ponto (10-170 s). Depois de cada morte o save e lido como o
LocalSave.Load le (o principal; ilegivel, o .bak) e tem de: ser JSON, ter saveVersion == SaveData.SchemaVersion e ter
o salto inteiro ou nada (marco_idade_8 no historico <=> ageYears 8). Principal ilegivel com .bak bom conta como AVISO
(o jogo recupera, mas a gravacao atomica nao deveria deixar isso). Sai 1 se alguma rodada deixar save ilegivel, de
versao errada ou com o salto pela metade.
"""
import argparse
import json
import os
import random
import subprocess
import sys
import tempfile

SCHEMA = 2                 # SaveData.SchemaVersion
MARCO = "marco_idade_8"    # AgeAdvanceCatalog.SaltoInfancia
SAVE = "roteiro_save.json"
PASTA = os.path.join(os.environ.get("USERPROFILE", ""), "AppData", "LocalLow", "V-STACK", "Chronicles of Existence")


def _ler(path):
    try:
        with open(path, encoding="utf-8") as f:
            return json.load(f)
    except (OSError, ValueError):
        return None


def conferir(pasta):
    """(nivel, texto): nivel 'ok', 'aviso' ou 'falha'."""
    principal = os.path.join(pasta, SAVE)
    bak = principal + ".bak"
    if not os.path.exists(principal) and not os.path.exists(bak):
        return "ok", "sem save (morreu antes da primeira gravacao)"
    d, fonte = _ler(principal), "principal"
    if d is None:
        d, fonte = _ler(bak), ".bak (principal ilegivel)"
    if d is None:
        return "falha", "principal e .bak ilegiveis"
    if d.get("saveVersion") != SCHEMA:
        return "falha", "%s com saveVersion %r" % (fonte, d.get("saveVersion"))
    eventos = {e.get("eventId") for e in ((d.get("lifeHistory") or {}).get("eventos") or [])}
    idade, marco = d.get("ageYears"), MARCO in eventos
    if marco != (idade == 8):
        return "falha", "%s com o salto pela metade (idade %r, %s %s)" % (fonte, idade, MARCO, "sim" if marco else "nao")
    return ("ok" if fonte == "principal" else "aviso"), "%s, idade %r, %d eventos" % (fonte, idade, len(eventos))


def rodar(exe, pasta, rodadas, rng):
    contagem = {"ok": 0, "aviso": 0, "falha": 0}
    for i in range(rodadas):
        salto = i % 2 == 0
        t = rng.uniform(57.0, 61.0) if salto else rng.uniform(10.0, 170.0)
        args = [exe, "-screen-width", "1200", "-screen-height", "540", "-screen-fullscreen", "0", "-toque", "-roteiro"]
        if salto:
            args.append("quebrada")
        p = subprocess.Popen(args)
        try:
            p.wait(timeout=t)
            como = "terminou sozinho antes"
        except subprocess.TimeoutExpired:
            p.kill()   # TerminateProcess: sem OnApplicationQuit, como o sistema matando o app
            p.wait()
            como = "morto"
        nivel, texto = conferir(pasta)
        contagem[nivel] += 1
        print("%-5s rodada %2d (%s, %s aos %5.1f s): %s" % (
            nivel.upper(), i + 1, "quebrada/salto" if salto else "completa", como, t, texto), flush=True)
    print("save_caos: %d ok, %d aviso, %d falha em %d rodadas" % (contagem["ok"], contagem["aviso"], contagem["falha"], rodadas))
    return 1 if contagem["falha"] else 0


def _autoteste():
    def caso(principal, bak):
        d = tempfile.mkdtemp()
        for nome, conteudo in ((SAVE, principal), (SAVE + ".bak", bak)):
            if conteudo is not None:
                with open(os.path.join(d, nome), "w", encoding="utf-8") as f:
                    f.write(conteudo if isinstance(conteudo, str) else json.dumps(conteudo))
        return conferir(d)[0]

    def save(idade, marco, versao=SCHEMA):
        ev = [{"eventId": "evento.q08_concluida"}] + ([{"eventId": MARCO}] if marco else [])
        return {"saveVersion": versao, "ageYears": idade, "lifeHistory": {"eventos": ev}}

    assert caso(None, None) == "ok", "sem save"
    assert caso(save(5, False), None) == "ok", "antes do salto"
    assert caso(save(8, True), save(5, False)) == "ok", "depois do salto"
    assert caso(save(8, False), None) == "falha", "idade sem marco"
    assert caso(save(5, True), None) == "falha", "marco sem idade"
    assert caso('{"saveVersion": 2, "ageY', save(5, False)) == "aviso", "principal truncado, .bak bom"
    assert caso('{"saveVersion": 2, "ageY', "lixo") == "falha", "os dois ilegiveis"
    assert caso(save(5, False, versao=1), None) == "falha", "versao velha gravada por esta build"
    print("autoteste ok")


def main(argv=None):
    p = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    p.add_argument("--rodadas", type=int, default=10)
    p.add_argument("--exe", default=os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "Builds", "win", "COE.exe"))
    p.add_argument("--seed", type=int, default=None)
    p.add_argument("--autoteste", action="store_true")
    a = p.parse_args(argv)
    if a.autoteste:
        _autoteste()
        return 0
    if not os.path.exists(a.exe):
        print("save_caos: sem %s (build de desenvolvimento: client/tools/build_windows.ps1)" % a.exe)
        return 2
    return rodar(os.path.abspath(a.exe), PASTA, a.rodadas, random.Random(a.seed))


if __name__ == "__main__":
    sys.exit(main())
