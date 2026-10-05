"""Device lab do COE num Android fisico (POCO F4 de referencia): o robo interno (Roteiro.cs, -roteiro) e o robo de
toque (adb input) no aparelho, com logcat filtrado, telas, memoria, bateria/temperatura e CSV do PerfHud. Tudo bruto
em client/Builds/device_lab/<run_id>/ (ignorado pelo git); o resumo pequeno vai a mao para docs/medicoes/.

  python client/tools/device_lab.py --modo smoke|functional|touch|perf|chaos|soak|full
        [--serial S] [--rebuild] [--keep-installed] [--run-id ID] [--qualidade baixa|media|alta]
        [--minutos N] [--minimo]
  python client/tools/device_lab.py --autoteste

smoke      12 nascimentos (4 destinos x 3 origens), `-roteiro nascimento`: Auren aos 5, save, NPCs, o stick move.
functional 24 rotas (12 nascimentos x promessa cumprida/quebrada; --minimo = 8) do Limiar ao gancho, invariantes no save.
touch      robo de toque pela resolucao REAL (fracoes da tela): HUD, joystick, camera, pausa, voltar, combate, modais.
perf       Auren com -autowalk por --minutos (padrao 15) em cada faixa (ou so --qualidade), sem video; perf_report.py.
chaos      force-stop do COE em 5 pontos (nascimento, recompensa, durante/depois do salto, treino), relanca com
           -continuar e confere o save (save_caos.conferir) e a chegada ao gancho sem duplicar nada.
soak       rotas em laco por --minutos (padrao 45), amostrando PID, memoria, temperatura e bateria a cada 30 s.
full       smoke, functional, touch, chaos, perf e soak, nessa ordem.

Escopo do adb: SO o pacote do COE (install -r, start, force-stop; pm clear so no touch). O save.json do aparelho e
guardado em backup_save/ antes e devolvido no fim. O robo de toque so toca com o COE em primeiro plano (senao para).
Nada de root, de outro app, de dado pessoal. Sai 1 se alguma rodada falhou, 2 se o aparelho nao serve.
"""
import argparse
import datetime
import hashlib
import json
import math
import os
import queue
import random
import re
import subprocess
import sys
import threading
import time

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import save_caos  # noqa: E402  conferir(pasta): o mesmo juiz do save que o PC usa

PKG = "br.com.vstack.coe"                                  # = ProjectSetup.AppId
FILES = "/storage/emulated/0/Android/data/%s/files" % PKG  # Application.persistentDataPath no aparelho
TOOLS = os.path.dirname(os.path.abspath(__file__))
CLIENT = os.path.dirname(TOOLS)
APK = os.path.join(CLIENT, "Builds", "android", "COE.apk")
DESTINOS = ["serena", "normal", "dificil", "ruptura"]       # DestinyCatalog.Destinos
ORIGENS = ["agricultores", "artesaos", "guardioes"]         # DestinyCatalog.Origens (as 3 servem aos 4 destinos)
QUALIDADES = ["baixa", "media", "alta"]                     # Qualidade.cs (ADR-0009)
MARCO_SALTO = save_caos.MARCO
MARCO_GANCHO = "marco.fim_da_primeira_existencia"           # GameSession.MarcoGancho
CUMPRIDA, QUEBRADA = "evento.q04_promessa_cumprida", "evento.q04_promessa_quebrada"
CENTRAIS = ["q01_um_novo_amanhecer", "q02_uma_pequena_responsabilidade", "q04_uma_promessa",
            "q07_o_desaparecimento", "q08_ecos_do_limiar"]
CONCLUIDA = 3                                               # QuestStatus.Concluida

# Classificacao termica do protocolo do device lab (bateria, graus C). Status do thermalservice: 3 = SEVERE.
def classificar_termica(temp_c, status):
    if temp_c is None:
        return "DESCONHECIDA"
    if temp_c >= 47 or (status or 0) >= 3:
        return "ABORTAR"
    if temp_c >= 45:
        return "RESFRIAR"
    if temp_c >= 42:
        return "HOT"
    if temp_c >= 38:
        return "WARM"
    return "COLD"


# Falhas no logcat (filtrado ao COE). Ordem = gravidade na hora de reportar.
PADROES = [
    ("FATAL EXCEPTION", re.compile(r"FATAL EXCEPTION")),
    ("AndroidRuntime", re.compile(r"\sE AndroidRuntime\s*:")),
    ("ANR", re.compile(r"\bANR in " + re.escape(PKG))),
    ("SIGSEGV", re.compile(r"SIGSEGV")),
    ("SIGABRT", re.compile(r"SIGABRT")),
    ("CRASH", re.compile(r"\s[EF] CRASH\s*:")),
    ("OutOfMemory", re.compile(r"OutOfMemory")),
    ("UnityException", re.compile(r"UnityException")),
    ("NullReferenceException", re.compile(r"NullReferenceException")),
    ("Exception", re.compile(r"\s[EW] Unity\s*:.*Exception")),
    ("erro Unity", re.compile(r"\sE Unity\s*:")),
]
LINHA = re.compile(r"^\d\d-\d\d \d\d:\d\d:\d\d\.\d+\s+(\d+)\s+\d+\s+\w\s")   # logcat -v threadtime


def falhas_no_logcat(linhas):
    """[(padrao, linha)]: uma entrada por linha (o primeiro padrao que casa)."""
    r = []
    for l in linhas:
        for nome, rx in PADROES:
            if rx.search(l):
                r.append((nome, l.rstrip()))
                break
    return r


def filtrar_logcat(linhas, pids):
    """So o que e do COE: linhas dos processos dele ou que citam o pacote (ActivityManager, ANR, tombstone)."""
    pids = {str(p) for p in pids}
    for l in linhas:
        m = LINHA.match(l)
        if (m and m.group(1) in pids) or PKG in l:
            yield l


def pss_kb(meminfo):
    m = re.search(r"TOTAL PSS:\s+(\d+)", meminfo) or re.search(r"^\s*TOTAL\s+(\d+)", meminfo, re.M)
    return int(m.group(1)) if m else None


def resumo_save(d):
    """O que o relatorio e as invariantes leem de um roteiro_save.json."""
    if not d:
        return None
    eventos = [e.get("eventId") for e in (d.get("lifeHistory") or {}).get("eventos") or []]
    rep = {"%s.%s" % (l.get("alvo"), l.get("dimensao")): l.get("valor") for l in (d.get("reputation") or {}).get("leituras") or []}
    quests = {q.get("questId"): q.get("status") for q in (d.get("quests") or {}).get("missoes") or []}
    inv = d.get("inventario") or {}
    rec = inv.get("recompensasAplicadas") or []
    return {
        "versao": d.get("saveVersion"), "destino": (d.get("birth") or {}).get("destinyId"),
        "origem": (d.get("birth") or {}).get("originId"), "idade": d.get("ageYears"),
        "eventos": len(eventos), "salto": eventos.count(MARCO_SALTO), "gancho": eventos.count(MARCO_GANCHO),
        "promessa": [e for e in (CUMPRIDA, QUEBRADA) if e in eventos], "nilo_sumiu": "evento.nilo_desapareceu" in eventos,
        "concluidas": sorted(q for q, s in quests.items() if s == CONCLUIDA), "recompensas": len(rec),
        "recompensas_repetidas": sorted({r for r in rec if rec.count(r) > 1}),
        "eventos_repetidos": sorted({e for e in eventos if eventos.count(e) > 1}),
        "confianca_sera": rep.get("npc_sera.confianca"), "confianca_nilo": rep.get("npc_nilo.confianca"),
        "moedas": inv.get("moedas"), "pratica": {p.get("activityId"): p.get("vezes") for p in (d.get("life") or {}).get("pratica") or []},
        "cena": d.get("sceneId"), "ancora": d.get("anchorId"),
    }


def invariantes(r, destino, origem, rota):
    """Falhas de regra numa rota que chegou ao gancho. rota: 'completa' ou 'quebrada'. [] = tudo certo."""
    if r is None:
        return ["sem save"]
    f = []
    if r["versao"] != save_caos.SCHEMA:
        f.append("saveVersion %r" % r["versao"])
    if (r["destino"], r["origem"]) != (destino, origem):
        f.append("nascimento mudou: %s/%s" % (r["destino"], r["origem"]))
    if r["idade"] != 8 or r["salto"] != 1:
        f.append("salto: idade %r, marco %dx" % (r["idade"], r["salto"]))
    if r["gancho"] != 1:
        f.append("gancho %dx" % r["gancho"])
    esperado = CUMPRIDA if rota == "completa" else QUEBRADA
    if r["promessa"] != [esperado]:
        f.append("promessa %r (esperado %s)" % (r["promessa"], esperado))
    sinal = 1 if rota == "completa" else -1
    for npc in ("sera", "nilo"):
        v = r["confianca_" + npc]
        if v is None or v * sinal <= 0:
            f.append("confianca de %s %r na rota %s" % (npc, v, rota))
    if not r["nilo_sumiu"]:
        f.append("evento.nilo_desapareceu ausente")
    faltam = [q for q in CENTRAIS if q not in r["concluidas"]]
    if faltam:
        f.append("centrais abertas: " + ",".join(faltam))
    if r["recompensas_repetidas"]:
        f.append("recompensa repetida: " + ",".join(r["recompensas_repetidas"]))
    return f


GEOMETRIA = re.compile(r"ToqueHud: tela=(\d+)x(\d+) safe=\(x:([\d.,-]+), y:([\d.,-]+), width:([\d.,-]+), height:([\d.,-]+)\) dpi=([\d.,]+)")
# Botoes de toque do preset destro (ControlPreset.cs, dp a partir do canto inferior DIREITO da area segura).
BOTOES_DP = {"ataque": (90, 90), "forte": (200, 70), "magia": (180, 180), "esquiva": (70, 200), "defesa": (280, 64), "usar": (64, 280)}
REF_W = 2400.0   # telas uGUI medidas na largura do POCO F4 em paisagem; escalam com a largura da area segura


def geometria(linha):
    """(W, H, safe_x, safe_y, safe_w, safe_h, dpi) da linha 'ToqueHud:' do logcat, ou None. Aceita virgula decimal."""
    m = GEOMETRIA.search(linha)
    return tuple(float(g.replace(",", ".")) for g in m.groups()) if m else None


class Toque:
    """Coordenadas do adb (origem em cima, a esquerda) a partir da geometria REAL que o jogo anunciou."""

    def __init__(self, g):
        self.W, self.H, self.sx, self.sy, self.sw, self.sh, dpi = g
        self.k = (dpi or 160.0) / 160.0

    def botao(self, nome):
        cx, cy = BOTOES_DP[nome]
        return int(self.sx + self.sw - cx * self.k), int(self.H - (self.sy + cy * self.k))

    def ui(self, x_ref, y):
        """Ponto de tela uGUI medido em 2400 px de largura sem recorte: escala com a area segura (EntryFlow, menus)."""
        return int(self.sx + x_ref * self.sw / REF_W), int(y)

    def joystick(self):
        return int(self.sx + self.sw * 0.25), int(self.H * 0.70)

    def camera(self):
        return int(self.sx + self.sw * 0.55), int(self.H * 0.30)


def diferenca(png_a, png_b):
    """Diferenca media (0-255) entre duas fotos, ou None sem Pillow: o juiz de 'o mundo mexeu' no robo de toque."""
    try:
        from PIL import Image, ImageChops, ImageStat
    except ImportError:
        return None
    a, b = Image.open(png_a).convert("L"), Image.open(png_b).convert("L")
    if a.size != b.size:
        return 255.0
    return round(sum(ImageStat.Stat(ImageChops.difference(a, b)).mean), 2)


TEMPO_ROTEIRO = re.compile(r"^(\d+[.,]\d+)s (.*)$")


def amostras_perf(pasta):
    """[(t_s, pior quadro em ms)] de todos os CSV do PerfHud da rodada (um por cena; t_s corre o processo inteiro)."""
    r = []
    for f in sorted(os.listdir(pasta)) if os.path.isdir(pasta) else []:
        if f.startswith("perf_") and f.endswith(".csv"):
            for l in open(os.path.join(pasta, f), encoding="utf-8").read().splitlines()[1:]:
                c = l.split(",")
                try:
                    fmin = float(c[8])
                    r.append((float(c[0]), 1000.0 / fmin if fmin > 0 else 9999.0))
                except (IndexError, ValueError):
                    pass
    return sorted(r)


def primeira_acao(roteiro_txt, amostras, janela=2.0):
    """{acao: [pior ms na 1a vez, na 2a, ...]} casando o instante de cada acao do Roteiro (realtimeSinceStartup) com o
    pior quadro das amostras do PerfHud que cobrem [t, t+janela] (a amostra em t_s cobre o segundo anterior a t_s).
    Acoes: cada verbo do treino, cada interacao (a 1a abre a primeira conversa) e o salto (recarga de cena: carga)."""
    def pior(t):
        v = [ms for ts, ms in amostras if t <= ts <= t + janela]
        return round(max(v), 1) if v else None
    r = {}
    for linha in roteiro_txt:
        m = TEMPO_ROTEIRO.match(linha.strip())
        if not m:
            continue
        t, texto = float(m.group(1).replace(",", ".")), m.group(2).strip()
        if texto.startswith("verbo "):
            chave = texto.split(" ", 1)[1]
        elif " -> " in texto and not texto.startswith(("descansar", "salto")):
            chave = "interacao"
        elif texto.startswith("salto -> "):
            chave = "salto"
        else:
            continue
        r.setdefault(chave, []).append(pior(t))
    return r


def run_id_de(agora, commit, aparelho):
    return "COE_%s_%s_%s" % (agora.strftime("%Y%m%d-%H%M"), commit[:7], re.sub(r"[^A-Za-z0-9]", "", aparelho) or "android")


# ------------------------------------------------------------------------------------------------------------- adb

class Adb:
    def __init__(self, serial):
        self.serial = serial
        self.exe = os.path.join(os.environ.get("LOCALAPPDATA", ""), "Android", "Sdk", "platform-tools", "adb.exe")
        if not os.path.exists(self.exe):
            self.exe = "adb"

    def cmd(self, *args):
        return [self.exe] + (["-s", self.serial] if self.serial else []) + list(args)

    def run(self, *args, timeout=120):
        p = subprocess.run(self.cmd(*args), capture_output=True, timeout=timeout)
        return p.stdout.decode("utf-8", "replace") + p.stderr.decode("utf-8", "replace")

    def sh(self, linha, timeout=60):
        return self.run("shell", linha, timeout=timeout)

    def png(self, destino):
        p = subprocess.run(self.cmd("exec-out", "screencap", "-p"), capture_output=True, timeout=30)
        with open(destino, "wb") as f:
            f.write(p.stdout)
        return len(p.stdout) > 0

    def pid(self):
        out = self.sh("pidof " + PKG)
        if "error:" in out or "no devices" in out:
            raise RuntimeError("adb: " + out.strip())   # aparelho caiu: nao e o jogo que morreu
        m = re.match(r"\s*(\d+)", out)
        return m.group(1) if m else None

    def na_frente(self):
        """O COE e a janela com foco? O robo de toque so injeta toque nele (input vai para quem estiver na tela)."""
        return PKG in self.sh("dumpsys window | grep mCurrentFocus")   # foco da JANELA: dialogo do sistema por cima = nao


def aparelhos(adb):
    out = subprocess.run([adb.exe, "devices", "-l"], capture_output=True).stdout.decode("utf-8", "replace")
    return [l.split()[0] for l in out.splitlines()[1:] if re.search(r"\sdevice\s", l + " ")]


def info_aparelho(adb):
    props = {k: adb.sh("getprop " + k).strip() for k in (
        "ro.product.manufacturer", "ro.product.model", "ro.product.device", "ro.build.version.release",
        "ro.build.version.sdk", "ro.kernel.qemu", "ro.soc.model")}
    props["wm_size"] = adb.sh("wm size").strip().splitlines()[-1].split(":")[-1].strip()
    props["wm_density"] = adb.sh("wm density").strip().splitlines()[-1].split(":")[-1].strip()
    return props


def termica(adb):
    bat = adb.sh("dumpsys battery")
    t = re.search(r"temperature:\s*(\d+)", bat)
    n = re.search(r"level:\s*(\d+)", bat)
    carregando = bool(re.search(r"(AC|USB|Wireless) powered: true", bat))
    st = re.search(r"Thermal Status:\s*(\d+)", adb.sh("dumpsys thermalservice"))
    temp = int(t.group(1)) / 10.0 if t else None
    status = int(st.group(1)) if st else None
    return {"temp_c": temp, "bateria": int(n.group(1)) if n else None, "carregando": carregando,
            "status": status, "classe": classificar_termica(temp, status)}


class ForaDoCoe(Exception):
    pass


class Logcat:
    """logcat -v threadtime em fluxo desde agora; guarda em memoria e entrega as linhas novas numa fila (marcadores).
    Em disco so vai o filtrado ao COE (filtrar_logcat)."""

    def __init__(self, adb):
        inicio = adb.sh("date '+%m-%d %H:%M:%S.000'").strip()
        self.p = subprocess.Popen(adb.cmd("logcat", "-v", "threadtime", "-T", inicio), stdout=subprocess.PIPE,
                                  stderr=subprocess.DEVNULL)
        self.linhas, self.fila = [], queue.Queue()
        threading.Thread(target=self._ler, daemon=True).start()

    def _ler(self):
        for b in self.p.stdout:
            l = b.decode("utf-8", "replace")
            self.linhas.append(l)
            self.fila.put(l)

    def parar(self):
        self.p.kill()
        self.p.wait()


class Lab:
    def __init__(self, a):
        self.a = a
        serial = a.serial
        self.adb = Adb(serial)
        prontos = aparelhos(self.adb)
        if not serial:
            if len(prontos) != 1:
                raise SystemExit("device_lab: %d aparelhos prontos (%s); escolha com --serial" % (len(prontos), prontos))
            serial = prontos[0]
        elif serial not in prontos:
            raise SystemExit("device_lab: %s nao esta pronto (%s)" % (serial, prontos))
        self.adb.serial = serial
        self.info = info_aparelho(self.adb)
        if serial.startswith("emulator") or self.info["ro.kernel.qemu"] == "1":
            raise SystemExit("device_lab: %s e emulador; o lab so roda em aparelho fisico" % serial)
        commit = subprocess.run(["git", "rev-parse", "HEAD"], capture_output=True, cwd=TOOLS).stdout.decode().strip()
        self.commit = commit
        self.run_id = a.run_id or run_id_de(datetime.datetime.now(), commit, self.info["ro.product.device"])
        self.dir = os.path.join(CLIENT, "Builds", "device_lab", self.run_id)
        for sub in ("logcat", "screens", "video", "perf", "memory", "thermal", "routes", "failures"):
            os.makedirs(os.path.join(self.dir, sub), exist_ok=True)
        self.resultados = []
        self.comp = None
        self.registrar("aparelho", dict(self.info, serial="..." + serial[-4:], commit=commit, termica=termica(self.adb)))

    def registrar(self, tipo, dado):
        dado = dict(dado, tipo=tipo, quando=datetime.datetime.now().isoformat(timespec="seconds"))
        self.resultados.append(dado)
        with open(os.path.join(self.dir, "resultados.jsonl"), "a", encoding="utf-8") as f:
            f.write(json.dumps(dado, ensure_ascii=False) + "\n")
        print(json.dumps(dado, ensure_ascii=False)[:400], flush=True)

    # ---------------------------------------------------------------------------------------------- build e app

    def preparar(self):
        if self.a.rebuild:
            r = subprocess.run(["powershell", "-ExecutionPolicy", "Bypass", "-File", os.path.join(TOOLS, "build_android.ps1")])
            if r.returncode != 0:
                raise SystemExit("device_lab: build_android.ps1 falhou (%d)" % r.returncode)
        if not os.path.exists(APK):
            raise SystemExit("device_lab: sem %s (rode com --rebuild ou client/tools/build_android.ps1)" % APK)
        sha = hashlib.sha256(open(APK, "rb").read()).hexdigest()
        if not self.a.keep_installed:
            out = self.adb.run("install", "-r", APK, timeout=600)
            if "Success" not in out:
                raise SystemExit("device_lab: install falhou: " + out.strip()[-300:])
        self.registrar("apk", {"sha256": sha, "bytes": os.path.getsize(APK), "instalado": not self.a.keep_installed})
        res = [l.strip() for l in self.adb.sh("cmd package resolve-activity --brief -c android.intent.category.LAUNCHER " + PKG).splitlines() if "/" in l]
        self.comp = res[-1] if res else PKG + "/com.unity3d.player.UnityPlayerGameActivity"
        # O touch faz pm clear e grava save de teste; o perf abre Auren sem -roteiro (save.json normal). O que havia antes
        # da rodada vai para backup_save/ e volta no fim (devolver_save).
        self.backup = os.path.join(self.dir, "backup_save")
        os.makedirs(self.backup, exist_ok=True)
        existentes = self.adb.sh("ls " + FILES).split()
        self.salvos = [f for f in ("save.json", "save.json.bak") if f in existentes]
        for f in self.salvos:
            if not os.path.exists(os.path.join(self.backup, f)):
                self.adb.run("pull", FILES + "/" + f, os.path.join(self.backup, f))
        self.registrar("backup_save", {"arquivos": self.salvos})

    def devolver_save(self):
        """Devolve o save.json de antes da rodada; sem save antes, apaga o que os testes gravaram (so do COE)."""
        self.parar()
        for f in ("save.json", "save.json.bak"):
            if f in self.salvos:
                self.adb.run("push", os.path.join(self.backup, f), FILES + "/" + f)
            else:
                self.adb.sh("rm -f %s/%s" % (FILES, f))

    def abrir(self, extra):
        if not re.match(r"^[A-Za-z0-9_ \-]*$", extra):
            raise ValueError("extra com caractere fora do permitido: %r" % extra)
        self.adb.sh("input keyevent KEYCODE_WAKEUP")
        linha = "am start -S -n %s" % self.comp + (" -e unity '%s'" % extra if extra else "")
        out = self.adb.sh(linha)
        if "Error" in out:
            raise RuntimeError("am start: " + out)

    def parar(self):
        self.adb.sh("am force-stop " + PKG)

    def foto(self, nome):
        caminho = os.path.join(self.dir, "screens", nome + ".png")
        return caminho if self.adb.png(caminho) else None

    # ---------------------------------------------------------------------------------------------- robo interno

    def roteiro(self, nome, extra, limite=420, matar=None):
        """Uma rodada do Roteiro no aparelho. matar=(regex do marcador ROTEIRO, atraso s): force-stop do COE depois dele.
        Devolve o registro (ok, duracao, ultima linha, falhas do logcat, resumo do save) e deixa o bruto em routes/<nome>."""
        pasta = os.path.join(self.dir, "routes", nome)
        os.makedirs(pasta, exist_ok=True)
        csvs_antes = set(self.adb.sh("ls " + FILES).split())
        self.adb.sh("rm -f %s/roteiro/roteiro.txt" % FILES)   # so o save do robo fica (o -continuar precisa dele)
        log = Logcat(self.adb)
        t0 = time.time()
        self.abrir(extra)
        pids, morto, visto, marcadores, olhou = set(), None, False, [], 0.0
        rx = re.compile(matar[0]) if matar else None
        while time.time() - t0 < limite:
            try:
                l = log.fila.get(timeout=1.0)
            except queue.Empty:
                l = None
            if l and " Unity " in l and "ROTEIRO " in l:
                marcadores.append(l.split("ROTEIRO ", 1)[1].strip())
                if rx and morto is None and rx.search(l):
                    time.sleep(matar[1])
                    self.parar()
                    morto = "%.1f s depois de '%s'" % (matar[1], marcadores[-1])
            if time.time() - olhou >= 2.0:
                olhou = time.time()
                p = self.adb.pid()
                if p:
                    pids.add(p)
                    visto = True
                elif visto or time.time() - t0 > 30:
                    break
        else:
            self.parar()
            marcadores.append("TIMEOUT %d s" % limite)
        dur = time.time() - t0
        time.sleep(1.5)
        log.parar()
        for m in re.finditer(r"Start proc (\d+):" + re.escape(PKG), "".join(log.linhas)):
            pids.add(m.group(1))
        filtrado = list(filtrar_logcat(log.linhas, pids))
        with open(os.path.join(self.dir, "logcat", nome + ".txt"), "w", encoding="utf-8") as f:
            f.writelines(filtrado)
        for arq in ("roteiro_save.json", "roteiro_save.json.bak"):
            self.adb.run("pull", FILES + "/" + arq, os.path.join(pasta, arq))
        self.adb.run("pull", FILES + "/roteiro/roteiro.txt", os.path.join(pasta, "roteiro.txt"))
        for f in self.adb.sh("ls " + FILES).split():   # CSV do PerfHud desta rodada (primeira acao a frio, carga do salto)
            if f.startswith("perf_") and f not in csvs_antes:
                self.adb.run("pull", FILES + "/" + f, os.path.join(pasta, f))
        falhas = falhas_no_logcat(filtrado)
        try:
            save = json.load(open(os.path.join(pasta, "roteiro_save.json"), encoding="utf-8"))
        except (OSError, ValueError):
            save = None
        # O roteiro.txt que o jogo gravou e a fonte da verdade; o logcat em fluxo pode perder o comeco (1a rodada, 2026-10-05).
        try:
            txt = [l.strip() for l in open(os.path.join(pasta, "roteiro.txt"), encoding="utf-8") if l.strip()]
        except OSError:
            txt = []
        ultima = txt[-1] if txt else (marcadores[-1] if marcadores else "")
        reg = {"nome": nome, "extra": extra, "ok": ultima.endswith("ROTEIRO OK") and not morto,
               "linhas_logcat": len(filtrado),
               "duracao_s": round(dur, 1), "ultima": ultima, "morto": morto, "pids": sorted(pids),
               "falhas_logcat": [f[0] for f in falhas], "save": resumo_save(save),
               "termica": termica(self.adb)}
        if falhas or (not reg["ok"] and not morto):
            with open(os.path.join(self.dir, "failures", nome + ".txt"), "w", encoding="utf-8") as f:
                f.write("\n".join(marcadores) + "\n\n" + "\n".join("%s: %s" % x for x in falhas))
            self.foto("falha_" + nome)
        return reg

    def puxar_fotos(self, nome):
        self.adb.run("pull", FILES + "/roteiro", os.path.join(self.dir, "routes", nome, "fotos"), timeout=300)

    # ---------------------------------------------------------------------------------------------- modos

    def smoke(self):
        for d in DESTINOS:
            for o in ORIGENS:
                nome = "smoke_%s_%s" % (d, o)
                r = self.roteiro(nome, "-roteiro nascimento -destino %s -origem %s" % (d, o), limite=150)
                s = r["save"] or {}
                r["falhas"] = [x for x in [
                    None if r["ok"] else "roteiro: " + r["ultima"],
                    None if (s.get("destino"), s.get("origem")) == (d, o) else "nascimento gravado %r/%r" % (s.get("destino"), s.get("origem")),
                    None if s.get("idade") == 5 else "idade %r" % s.get("idade"),
                    None if s.get("versao") == save_caos.SCHEMA else "saveVersion %r" % s.get("versao"),
                    None if not [f for f in r["falhas_logcat"] if f != "erro Unity"] else "logcat: " + ",".join(r["falhas_logcat"]),
                ] if x]
                r["pass"] = not r["falhas"]
                if d == DESTINOS[0] and o == ORIGENS[0]:
                    self.puxar_fotos(nome)
                self.registrar("smoke", r)

    def matriz(self, minimo):
        if not minimo:
            return [(d, o, rota) for d in DESTINOS for o in ORIGENS for rota in ("completa", "quebrada")]
        # 8 rotas: os 4 destinos x 2 desfechos, as 3 origens em rodizio
        return [(d, ORIGENS[(2 * i + j) % 3], rota) for i, d in enumerate(DESTINOS) for j, rota in enumerate(("completa", "quebrada"))]

    def rota(self, nome, d, o, rota, foto=False, extra_mais=""):
        extra = "-roteiro%s -destino %s -origem %s%s%s" % (" quebrada" if rota == "quebrada" else "", d, o,
                                                         "" if foto else " -semfoto", extra_mais)
        r = self.roteiro(nome, extra)
        r["falhas"] = ([] if r["ok"] else ["roteiro: " + r["ultima"]]) + invariantes(r["save"], d, o, rota) + \
            (["logcat: " + ",".join(r["falhas_logcat"])] if [f for f in r["falhas_logcat"] if f != "erro Unity"] else [])
        r["pass"] = not r["falhas"]
        r.update(destino=d, origem=o, rota=rota)
        return r

    def functional(self):
        for i, (d, o, rota) in enumerate(self.matriz(self.a.minimo)):
            nome = "e2e_%02d_%s_%s_%s" % (i + 1, d, o, rota)
            r = self.rota(nome, d, o, rota, foto=i < 2)
            if i < 2:
                self.puxar_fotos(nome)
            self.registrar("functional", r)

    PONTOS = [   # (nome, rota, marcador do Roteiro, atraso min, max): onde o force-stop cai
        ("apos_nascimento", "quebrada", r"nasceu: ", 0.5, 2.0),
        ("recompensa_q02", "completa", r"q02_uma_pequena_responsabilidade/prestar_contas -> ", 1.5, 3.5),
        ("durante_salto", "quebrada", r"salto -> SaltoGatilho", 0.3, 1.2),
        ("apos_confirmar_salto", "quebrada", r"salto -> SaltoGatilho", 1.6, 3.0),
        ("treino", "quebrada", r"verbo ", 0.2, 1.5),
    ]

    def chaos(self, repeticoes=2):
        rng = random.Random(20261005)
        for rep in range(repeticoes):
            for i, (ponto, rota, marcador, a, b) in enumerate(self.PONTOS):
                d, o = DESTINOS[(i + rep) % 4], ORIGENS[(i + rep) % 3]
                nome = "chaos_%s_%d" % (ponto, rep + 1)
                atraso = round(rng.uniform(a, b), 2)
                base = "-roteiro%s -destino %s -origem %s" % (" quebrada" if rota == "quebrada" else "", d, o)
                morte = self.roteiro(nome + "_a_morte", base, matar=(marcador, atraso))
                nivel, texto = save_caos.conferir(os.path.join(self.dir, "routes", nome + "_a_morte"))
                volta = self.rota(nome + "_b_volta", d, o, rota, foto=False, extra_mais=" -continuar")
                r = {"nome": nome, "ponto": ponto, "rota": rota, "destino": d, "origem": o, "atraso_s": atraso,
                     "morto": morte["morto"], "save_na_morte": morte["save"], "conferir": [nivel, texto],
                     "volta_ok": volta["ok"], "volta_falhas": volta["falhas"], "save_final": volta["save"],
                     "falhas_logcat": morte["falhas_logcat"] + volta["falhas_logcat"]}
                r["falhas"] = ([] if morte["morto"] else ["nao morreu no ponto (%s)" % morte["ultima"]]) + \
                    ([] if nivel != "falha" else ["save na morte: " + texto]) + volta["falhas"]
                r["pass"] = not r["falhas"]
                self.registrar("chaos", r)

    def esfriar(self, ate=40.0, maximo_s=900):
        t = termica(self.adb)
        t0 = time.time()
        while t["temp_c"] is not None and t["temp_c"] >= ate and time.time() - t0 < maximo_s:
            print("esfriando: %.1f C (%s)" % (t["temp_c"], t["classe"]), flush=True)
            time.sleep(60)
            t = termica(self.adb)
        return t

    def amostrar(self, arquivo, t0):
        """Uma linha por chamada: t, pid, PSS, temperatura, bateria, status termico. Devolve a termica."""
        t = termica(self.adb)
        p = self.adb.pid()
        pss = pss_kb(self.adb.sh("dumpsys meminfo " + PKG)) if p else None
        novo = not os.path.exists(arquivo)
        with open(arquivo, "a", encoding="utf-8") as f:
            if novo:
                f.write("t_s,pid,pss_kb,temp_c,bateria,carregando,status_termico,classe\n")
            f.write("%d,%s,%s,%s,%s,%s,%s,%s\n" % (time.time() - t0, p or "", pss or "", t["temp_c"], t["bateria"],
                                                 t["carregando"], t["status"], t["classe"]))
        return t, p, pss

    def perf(self):
        faixas = [self.a.qualidade] if self.a.qualidade else QUALIDADES
        minutos = self.a.minutos or 15
        for q in faixas:
            antes = self.esfriar()
            if antes["classe"] == "ABORTAR":
                self.registrar("perf", {"qualidade": q, "pass": False, "falhas": ["termica ABORTAR antes de comecar"], "termica": antes})
                continue
            existentes = set(self.adb.sh("ls " + FILES).split())
            log = Logcat(self.adb)
            self.abrir("-scene Auren -autowalk -qualidade %s" % q)
            t0, pids, pior, abortou = time.time(), set(), antes, None
            amostras = os.path.join(self.dir, "perf", "%s_amostras.csv" % q)
            while time.time() - t0 < minutos * 60:
                time.sleep(30)
                t, p, _ = self.amostrar(amostras, t0)
                if p:
                    pids.add(p)
                else:
                    abortou = "o jogo morreu aos %d s" % (time.time() - t0)
                    break
                if (t["temp_c"] or 0) > (pior["temp_c"] or 0):
                    pior = t
                if t["classe"] == "ABORTAR":
                    abortou = "termica %s (%.1f C, status %s)" % (t["classe"], t["temp_c"], t["status"])
                    break
            open(os.path.join(self.dir, "perf", "%s_gfxinfo.txt" % q), "w", encoding="utf-8").write(self.adb.sh("dumpsys gfxinfo " + PKG))
            open(os.path.join(self.dir, "memory", "%s_meminfo.txt" % q), "w", encoding="utf-8").write(self.adb.sh("dumpsys meminfo " + PKG))
            self.parar()
            time.sleep(1)
            log.parar()
            filtrado = list(filtrar_logcat(log.linhas, pids))
            open(os.path.join(self.dir, "logcat", "perf_%s.txt" % q), "w", encoding="utf-8").writelines(filtrado)
            novos = [f for f in self.adb.sh("ls " + FILES).split() if f.startswith("perf_") and f not in existentes]
            csv_local, rel = None, None
            if novos:
                csv_local = os.path.join(self.dir, "perf", "%s_%s" % (q, novos[-1]))
                self.adb.run("pull", FILES + "/" + novos[-1], csv_local)
                md = csv_local[:-4] + ".md"
                rel = subprocess.run([sys.executable, os.path.join(TOOLS, "perf_report.py"), csv_local, "--md", md],
                                     capture_output=True).stdout.decode("utf-8", "replace")
            falhas = falhas_no_logcat(filtrado)
            r = {"qualidade": q, "minutos": round((time.time() - t0) / 60, 1), "termica_inicio": antes, "termica_pico": pior,
                 "termica_fim": termica(self.adb), "csv": csv_local, "relatorio": rel, "abortou": abortou,
                 "falhas_logcat": [f[0] for f in falhas]}
            r["falhas"] = ([abortou] if abortou else []) + ([] if csv_local else ["sem CSV do PerfHud"]) + \
                (["logcat: " + ",".join(r["falhas_logcat"])] if [f for f in r["falhas_logcat"] if f != "erro Unity"] else [])
            r["pass"] = not r["falhas"]
            self.registrar("perf", r)

    # ---------------------------------------------------------------------------------------------- robo de toque

    def frente(self):
        for _ in range(4):   # troca de cena deixa o foco vazio por um instante
            if self.adb.na_frente():
                return
            time.sleep(0.5)
        raise ForaDoCoe("o COE nao esta em primeiro plano: o robo de toque para (nada vai para outro app)")

    def tap(self, xy, espera=0.8):
        self.frente()
        self.adb.sh("input tap %d %d" % xy)
        time.sleep(espera)

    def arrastar(self, de, para, segura_s=1.0, espera=0.4):
        """Dedo que pousa, vai e fica parado (motionevent): o `input swipe` sobe o stick de zero ao maximo no caminho."""
        self.frente()
        self.adb.sh("input motionevent DOWN %d %d" % de)
        self.adb.sh("input motionevent MOVE %d %d" % para)
        time.sleep(segura_s)
        self.adb.sh("input motionevent UP %d %d" % para)
        time.sleep(espera)

    def voltar(self, espera=1.2):
        self.frente()
        self.adb.sh("input keyevent KEYCODE_BACK")
        time.sleep(espera)

    def esperar_linha(self, log, padrao, limite=25):
        rx, t0 = re.compile(padrao), time.time()
        while time.time() - t0 < limite:
            try:
                l = log.fila.get(timeout=1.0)
            except queue.Empty:
                continue
            if rx.search(l):
                return l
        return None

    def touch(self):
        try:
            self._touch()
        except ForaDoCoe as e:
            self.parar()
            self.foto("touch_fora_do_coe")
            self.registrar("touch", {"pass": False, "falhas": [str(e)]})

    def _touch(self):
        """Robo de toque: as telas do nascimento, a HUD de Auren e os modais pelo adb, com foto a cada passo e o logcat do
        voltar. Calibra pela linha 'ToqueHud:' (tela, area segura, dpi reais). Comeca com pm clear do COE (so dele).
        Parte aos 8 anos: usa o save que o proprio robo interno gravou numa rota inteira (functional), sem editar nada."""
        self.parar()
        self.adb.sh("pm clear " + PKG)
        log = Logcat(self.adb)
        self.abrir("-scene Auren")
        linha = self.esperar_linha(log, r"ToqueHud: tela=")
        g = geometria(linha or "")
        if not g:
            log.parar()
            self.registrar("touch", {"pass": False, "falhas": ["o jogo nao anunciou a geometria (ToqueHud:)"]})
            return
        t = Toque(g)
        self.parar()
        self.adb.sh("pm clear " + PKG)
        passos = []

        def passo(nome, ok=True, **extra):
            caminho = self.foto("touch_%02d_%s" % (len(passos) + 1, nome))
            vivo = bool(self.adb.pid())
            passos.append(dict(nome=nome, ok=bool(ok) and vivo, vivo=vivo, foto=os.path.basename(caminho or ""), **extra))
            print("touch %-28s ok=%s %s" % (nome, passos[-1]["ok"], extra or ""), flush=True)
            return caminho

        # 1. titulo e nascimento pelas telas do EntryFlow (coordenadas escalam com a area segura)
        self.abrir("")
        time.sleep(8)
        passo("titulo")
        self.voltar()
        passo("titulo_voltar_pede_sair")
        self.tap(t.ui(907, 587))
        passo("sair_cancelado")
        self.tap(t.ui(1200, 949), 1.5)
        passo("limiar")
        for _ in range(6):
            self.tap(t.ui(2013, 965), 1.0)
        passo("destino")
        self.tap(t.ui(906, 598), 1.2)
        passo("origem")
        self.voltar()
        passo("origem_voltar_volta_ao_destino")
        self.tap(t.ui(906, 598), 1.2)
        self.tap(t.ui(415, 598), 1.2)
        passo("nome")
        self.tap(t.ui(2013, 965), 1.2)
        passo("certeza")
        self.tap(t.ui(2013, 965), 10.0)
        anterior = passo("auren_aos_5")

        # 2. joystick nas 8 direcoes (mundo tem de mexer), camera, arrasto que comeca num botao
        jx, jy = t.joystick()
        r = int(60 * t.k)
        for ang in range(0, 360, 45):
            a = ang * 3.14159265 / 180.0
            self.arrastar((jx, jy), (jx + int(r * math.cos(a)), jy - int(r * math.sin(a))), 1.2)
            atual = passo("joystick_%03d" % ang)
            d = diferenca(anterior, atual)
            passos[-1].update(diferenca=d, ok=passos[-1]["ok"] and (d is None or d > 2.0))
            anterior = atual
        c = t.camera()
        self.arrastar(c, (c[0] + 500, c[1]), 0.3)
        atual = passo("camera_arrasto")
        d_cam = diferenca(anterior, atual)
        passos[-1].update(diferenca=d_cam)
        anterior = atual
        e = t.botao("esquiva")
        self.arrastar(e, (e[0] - 500, e[1]), 0.3)
        atual = passo("arrasto_sobre_botao")
        d_bot = diferenca(anterior, atual)
        passos[-1].update(diferenca=d_bot, ok=passos[-1]["ok"] and (d_bot is None or d_cam is None or d_bot < d_cam / 2))
        anterior = atual

        # 3. pausa: abre, o mundo nao anda por baixo, voltar fecha; voltar no jogo pede sair
        self.tap(t.ui(1821, 99), 1.2)
        anterior = passo("pausa_aberta")
        self.arrastar((jx, jy), (jx, jy - r), 1.5)
        atual = passo("pausa_joystick_por_baixo")
        d = diferenca(anterior, atual)
        passos[-1].update(diferenca=d, ok=passos[-1]["ok"] and (d is None or d < 2.0))
        self.voltar()
        passo("pausa_voltar_fecha")
        self.voltar()
        passo("jogo_voltar_pede_sair")
        self.tap(t.ui(907, 587))
        passo("sair_cancelado_no_jogo")

        # 4. usar perto de quem estiver la (a familia na casa) e voltar fecha a conversa
        self.tap(t.botao("usar"), 1.5)
        passo("usar")
        self.tap(t.ui(1200, 949), 0.6)
        passo("conversa_toque")
        self.voltar()
        passo("conversa_voltar")

        # 5. aos 8: combate pelo toque, com o save de uma rota inteira do robo
        save8 = None
        for raiz, _, arqs in os.walk(os.path.join(self.dir, "routes")):
            if "roteiro_save.json" in arqs:
                s = resumo_save(json.load(open(os.path.join(raiz, "roteiro_save.json"), encoding="utf-8")))
                if s and s["idade"] == 8:
                    save8 = os.path.join(raiz, "roteiro_save.json")
        if save8:
            self.parar()
            self.adb.run("push", save8, FILES + "/save.json")
            self.abrir("")
            time.sleep(8)
            passo("titulo_continuar")
            self.tap(t.ui(1200, 764), 10.0)
            anterior = passo("auren_aos_8")
            for b in ("ataque", "forte", "magia", "esquiva"):
                self.tap(t.botao(b), 1.2)
                atual = passo("toque_" + b)
                passos[-1].update(diferenca=diferenca(anterior, atual))
                anterior = atual
            d = t.botao("defesa")
            self.frente()
            self.adb.sh("input swipe %d %d %d %d 1500" % (d + d))
            passo("defesa_segurada")
        else:
            passos.append({"nome": "aos_8", "ok": False, "vivo": None, "foto": "", "motivo": "sem save de rota inteira (rode functional antes)"})
        time.sleep(1)
        voltares = [l.split("VoltarHud: ", 1)[1].strip() for l in log.linhas if "VoltarHud: voltar" in l]
        self.parar()
        log.parar()
        open(os.path.join(self.dir, "logcat", "touch.txt"), "w", encoding="utf-8").writelines(
            filtrar_logcat(log.linhas, set(re.findall(r"Start proc (\d+):" + re.escape(PKG), "".join(log.linhas)))))
        falhas = [p["nome"] for p in passos if not p["ok"]]
        self.registrar("touch", {"pass": not falhas, "falhas": falhas, "geometria": g, "passos": passos,
                                 "voltar_no_logcat": voltares})

    def soak(self):
        minutos = self.a.minutos or 45
        t = termica(self.adb)
        if (t["bateria"] or 0) < 20:
            self.registrar("soak", {"pass": False, "falhas": ["bateria %s%% < 20%%" % t["bateria"]], "termica": t})
            return
        t0, n, rodadas = time.time(), 0, []
        amostras = os.path.join(self.dir, "memory", "soak_amostras.csv")
        parar = threading.Event()

        def amostrador():
            while not parar.wait(30):
                self.amostrar(amostras, t0)
        threading.Thread(target=amostrador, daemon=True).start()
        matriz = self.matriz(False)
        try:
            while time.time() - t0 < minutos * 60:
                d, o, rota = matriz[n % len(matriz)]
                n += 1
                r = self.rota("soak_%02d_%s_%s_%s" % (n, d, o, rota), d, o, rota)
                rodadas.append({k: r[k] for k in ("nome", "pass", "duracao_s", "falhas")})
                if r["termica"]["classe"] == "ABORTAR":
                    break
                if r["termica"]["classe"] == "RESFRIAR":
                    self.esfriar()
        finally:
            parar.set()
        ok = sum(1 for r in rodadas if r["pass"])
        falhas = ["%s: %s" % (r["nome"], "; ".join(r["falhas"])) for r in rodadas if not r["pass"]]
        self.registrar("soak", {"minutos": round((time.time() - t0) / 60, 1), "rodadas": len(rodadas), "ok": ok,
                                "falhas": falhas, "pass": not falhas and ok > 0, "termica_fim": termica(self.adb),
                                "amostras": amostras})


def relatorio(pasta):
    """resumo.md da rodada a partir do resultados.jsonl (o bruto fica ao lado). Sem aparelho."""
    regs = [json.loads(l) for l in open(os.path.join(pasta, "resultados.jsonl"), encoding="utf-8")]
    por = {}
    for r in regs:
        por.setdefault(r["tipo"], []).append(r)
    L = ["# Device lab %s" % os.path.basename(pasta), ""]
    for a in por.get("aparelho", [])[-1:]:
        t = a["termica"]
        L += ["%s %s (%s), Android %s / API %s, %s, %s dpi, SoC %s. Commit `%s`. Inicio: %.1f C, bateria %s%%, carregando=%s, status %s." % (
            a["ro.product.manufacturer"], a["ro.product.model"], a["ro.product.device"], a["ro.build.version.release"],
            a["ro.build.version.sdk"], a["wm_size"], a["wm_density"], a["ro.soc.model"], a["commit"][:7],
            t["temp_c"], t["bateria"], t["carregando"], t["status"]), ""]
    for a in por.get("apk", [])[-1:]:
        L += ["APK `%s...` (%.1f MB)." % (a["sha256"][:16], a["bytes"] / 1e6), ""]

    def ultimos(tipo, chave="nome"):   # rodada repetida (rerun) vale a ultima
        d = {}
        for r in por.get(tipo, []):
            d[r.get(chave) or r.get("qualidade")] = r
        return list(d.values())

    sm = ultimos("smoke")
    if sm:
        L += ["## Nascimentos (smoke): %d/%d" % (sum(r["pass"] for r in sm), len(sm)), "",
              "| Destino | Origem | Resultado | s | Falhas |", "|---|---|---|---|---|"]
        for r in sm:
            _, d, o = r["nome"].split("_", 2)
            L.append("| %s | %s | %s | %s | %s |" % (d, o, "PASS" if r["pass"] else "FAIL", r["duracao_s"], "; ".join(r["falhas"])))
        L.append("")
    fu = ultimos("functional")
    if fu:
        L += ["## Rotas do Limiar ao gancho: %d/%d" % (sum(r["pass"] for r in fu), len(fu)), "",
              "| # | Destino | Origem | Rota | Resultado | s | Concluidas | Promessa | Confianca Sera/Nilo | Moedas | Recompensas | Ancora | Falhas |",
              "|---|---|---|---|---|---|---|---|---|---|---|---|---|"]
        for i, r in enumerate(fu, 1):
            s = r["save"] or {}
            L.append("| %d | %s | %s | %s | %s | %s | %s | %s | %s/%s | %s | %s | %s | %s |" % (
                i, r["destino"], r["origem"], r["rota"], "PASS" if r["pass"] else "FAIL", r["duracao_s"],
                len(s.get("concluidas", [])), ",".join(p.split("_")[-1] for p in s.get("promessa", [])),
                s.get("confianca_sera"), s.get("confianca_nilo"), s.get("moedas"), s.get("recompensas"), s.get("ancora"),
                "; ".join(r["falhas"])))
        L.append("")
        acoes = {}
        for r in fu:
            if "-semfoto" not in r["extra"]:
                continue   # foto no aparelho custa quadros: so as rodadas sem foto entram na medida
            pasta_r = os.path.join(pasta, "routes", r["nome"])
            try:
                txt = open(os.path.join(pasta_r, "roteiro.txt"), encoding="utf-8").readlines()
            except OSError:
                continue
            for k, v in primeira_acao(txt, amostras_perf(pasta_r)).items():
                acoes.setdefault(k, []).append(v)
        if acoes:
            L += ["### Primeira acao a frio (pior quadro em ms na janela de 2 s; mediana entre rodadas sem foto)", "",
                  "| Acao | 1a vez | 2a vez | demais | rodadas |", "|---|---|---|---|---|"]
            for k, listas in sorted(acoes.items()):
                def med(i):
                    v = sorted(x[i] for x in listas if len(x) > i and x[i] is not None)
                    return v[len(v) // 2] if v else None
                demais = sorted(y for x in listas for y in x[2:] if y is not None)
                L.append("| %s | %s | %s | %s | %d |" % (k, med(0), med(1), demais[len(demais) // 2] if demais else None, len(listas)))
            L.append("")
    for r in por.get("touch", [])[-1:]:
        L += ["## Toque: %s" % ("PASS" if r["pass"] else "FAIL (%s)" % ", ".join(r["falhas"])), "",
              "Geometria anunciada pelo jogo: %s. Voltar no logcat: %s." % (r.get("geometria"), r.get("voltar_no_logcat")), "",
              "| Passo | ok | diferenca | foto |", "|---|---|---|---|"]
        for p in r.get("passos", []):
            L.append("| %s | %s | %s | %s |" % (p["nome"], p["ok"], p.get("diferenca", ""), p.get("foto", "")))
        L.append("")
    ch = ultimos("chaos")
    if ch:
        L += ["## Save chaos (force-stop do COE): %d/%d" % (sum(r["pass"] for r in ch), len(ch)), "",
              "| Ponto | Rota | Morte | Save na morte | Juiz | Volta | Falhas |", "|---|---|---|---|---|---|---|"]
        for r in ch:
            s = r["save_na_morte"] or {}
            L.append("| %s | %s | %s | idade %s, salto %s, %s eventos | %s | %s | %s |" % (
                r["ponto"], r["rota"], r["morto"], s.get("idade"), s.get("salto"), s.get("eventos"), r["conferir"][0],
                "ROTEIRO OK" if r["volta_ok"] else "falhou", "; ".join(r["falhas"])))
        L.append("")
    pe = ultimos("perf", "qualidade")
    if pe:
        L += ["## Desempenho por faixa", ""]
        for r in pe:
            if "termica_pico" not in r:
                L += ["### %s: FAIL (%s)" % (r["qualidade"], "; ".join(r["falhas"])), ""]
                continue
            L += ["### %s: %s (%.1f min; %.1f -> pico %.1f -> %.1f C; bateria %s -> %s%%)" % (
                r["qualidade"], "PASS" if r["pass"] else "FAIL (%s)" % "; ".join(r["falhas"]), r["minutos"],
                r["termica_inicio"]["temp_c"], r["termica_pico"]["temp_c"], r["termica_fim"]["temp_c"],
                r["termica_inicio"]["bateria"], r["termica_fim"]["bateria"]), "", "```", (r.get("relatorio") or "").strip(), "```", ""]
    for r in por.get("soak", [])[-1:]:
        L += ["## Soak: %s" % ("PASS" if r["pass"] else "FAIL"), "",
              "%s min, %s rodadas, %s ok. Falhas: %s. Termica no fim: %s." % (
                  r.get("minutos"), r.get("rodadas"), r.get("ok"), r.get("falhas"), r.get("termica_fim")), ""]
    with open(os.path.join(pasta, "resumo.md"), "w", encoding="utf-8") as f:
        f.write("\n".join(L) + "\n")
    return os.path.join(pasta, "resumo.md")


def main(argv=None):
    p = argparse.ArgumentParser(description=__doc__.splitlines()[0])
    p.add_argument("--modo", choices=["smoke", "functional", "touch", "perf", "chaos", "soak", "full", "relatorio"])
    p.add_argument("--serial", default="")
    p.add_argument("--rebuild", action="store_true")
    p.add_argument("--keep-installed", action="store_true")
    p.add_argument("--run-id", default="")
    p.add_argument("--qualidade", choices=QUALIDADES)
    p.add_argument("--minutos", type=float)
    p.add_argument("--minimo", action="store_true", help="functional com 8 rotas em vez de 24")
    p.add_argument("--autoteste", action="store_true")
    a = p.parse_args(argv)
    if a.autoteste:
        _autoteste()
        return 0
    if not a.modo:
        p.error("--modo e obrigatorio (ou --autoteste)")
    if a.modo == "relatorio":
        if not a.run_id:
            p.error("--modo relatorio pede --run-id")
        print(relatorio(os.path.join(CLIENT, "Builds", "device_lab", a.run_id)))
        return 0
    lab = Lab(a)
    lab.preparar()
    modos = ["smoke", "functional", "touch", "chaos", "perf", "soak"] if a.modo == "full" else [a.modo]
    try:
        for m in modos:
            getattr(lab, m)()
    finally:
        lab.devolver_save()
    relatorio(lab.dir)
    falhas = [r for r in lab.resultados if r.get("pass") is False]
    print("device_lab %s: %d registros, %d falhas -> %s" % (lab.run_id, len(lab.resultados), len(falhas), lab.dir))
    return 1 if falhas else 0


def _autoteste():
    assert classificar_termica(30.0, 0) == "COLD"
    assert classificar_termica(38.5, 0) == "WARM"
    assert classificar_termica(43.0, 1) == "HOT"
    assert classificar_termica(45.0, 0) == "RESFRIAR"
    assert classificar_termica(47.0, 0) == "ABORTAR"
    assert classificar_termica(36.0, 3) == "ABORTAR", "status SEVERE aborta mesmo frio"
    assert pss_kb("  TOTAL PSS:   412345   TOTAL RSS:  500000 ") == 412345
    assert pss_kb("       TOTAL   398765   1000\n") == 398765
    assert pss_kb("nada") is None

    l_coe = "10-05 14:00:00.123  4321  4350 I Unity   : ROTEIRO 012.3s nasceu: serena / agricultores\n"
    l_outro = "10-05 14:00:00.124  1111  1111 I Outro   : mensagem de outro app\n"
    l_am = "10-05 14:00:00.100  1500  1600 I ActivityManager: Start proc 4321:br.com.vstack.coe/u0a427\n"
    l_crash = "10-05 14:00:01.000  4321  4400 E CRASH   : *** signal 11 (SIGSEGV)\n"
    l_npe = "10-05 14:00:01.000  4321  4350 E Unity   : NullReferenceException: Object reference not set\n"
    l_err = "10-05 14:00:01.000  4321  4350 E Unity   : algo deu errado\n"
    assert list(filtrar_logcat([l_coe, l_outro, l_am], {4321})) == [l_coe, l_am], "so o COE vai para o disco"
    assert [f[0] for f in falhas_no_logcat([l_coe, l_crash, l_npe, l_err])] == ["SIGSEGV", "NullReferenceException", "erro Unity"]
    assert falhas_no_logcat([l_coe, l_am]) == []

    def save(rota, idade=8, salto=1, gancho=1, rep=False):
        ev = [{"eventId": e} for e in ["evento.q04_concluida", CUMPRIDA if rota == "completa" else QUEBRADA,
                                       "evento.nilo_desapareceu"] + [MARCO_SALTO] * salto + [MARCO_GANCHO] * gancho]
        s = 20 if rota == "completa" else -20
        return {"saveVersion": 2, "birth": {"destinyId": "serena", "originId": "artesaos"}, "ageYears": idade,
                "lifeHistory": {"eventos": ev}, "quests": {"missoes": [{"questId": q, "status": 3} for q in CENTRAIS]},
                "reputation": {"leituras": [{"alvo": "npc_sera", "dimensao": "confianca", "valor": s},
                                            {"alvo": "npc_nilo", "dimensao": "confianca", "valor": s}]},
                "inventario": {"moedas": 10, "recompensasAplicadas": ["rec.a", "rec.a"] if rep else ["rec.a", "rec.b"]}}

    assert invariantes(resumo_save(save("completa")), "serena", "artesaos", "completa") == []
    assert invariantes(resumo_save(save("quebrada")), "serena", "artesaos", "quebrada") == []
    assert invariantes(resumo_save(save("completa")), "serena", "artesaos", "quebrada"), "desfecho trocado"
    assert invariantes(resumo_save(save("completa")), "normal", "artesaos", "completa"), "destino mudou"
    assert invariantes(resumo_save(save("completa", salto=2)), "serena", "artesaos", "completa"), "salto duas vezes"
    assert invariantes(resumo_save(save("completa", gancho=0)), "serena", "artesaos", "completa"), "sem gancho"
    assert invariantes(resumo_save(save("completa", rep=True)), "serena", "artesaos", "completa"), "recompensa repetida"
    assert invariantes(None, "serena", "artesaos", "completa") == ["sem save"]
    g = geometria("10-05 I Unity : ToqueHud: tela=2400x1080 safe=(x:90.00, y:0.00, width:2310.00, height:1080.00) dpi=440")
    assert g == (2400.0, 1080.0, 90.0, 0.0, 2310.0, 1080.0, 440.0), g
    assert geometria("ToqueHud: tela=2400x1080 safe=(x:0,00, y:0,00, width:2400,00, height:1080,00) dpi=440") is not None, "virgula"
    t = Toque((2400.0, 1080.0, 0.0, 0.0, 2400.0, 1080.0, 440.0))
    assert t.botao("ataque") == (2152, 832) and t.botao("usar") == (2224, 310), (t.botao("ataque"), t.botao("usar"))
    assert Toque(g).ui(387, 965) == (462, 965) and Toque(g).ui(2013, 965) == (2027, 965), "uGUI escala com a area segura"
    txt = ["010,0s nasceu: serena / agricultores", "012,0s q01/falar -> mara", "020,0s q02/x -> daren",
           "100,0s   verbo South", "104,0s   verbo South", "200,0s salto -> SaltoGatilho"]
    amostras = [(10.5, 30.0), (12.5, 250.0), (13.5, 40.0), (20.5, 35.0), (100.5, 400.0), (101.5, 33.0), (104.5, 34.0), (201.0, 900.0)]
    pa = primeira_acao(txt, amostras)
    assert pa == {"interacao": [250.0, 35.0], "South": [400.0, 34.0], "salto": [900.0]}, pa
    assert run_id_de(datetime.datetime(2026, 10, 5, 14, 30), "eb8eab3828bef", "munch") == "COE_20261005-1430_eb8eab3_munch"
    print("autoteste ok")


if __name__ == "__main__":
    sys.exit(main())
