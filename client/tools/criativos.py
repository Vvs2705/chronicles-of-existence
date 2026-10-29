"""criativos.py - monta os criativos de divulgacao do Chronicles of Existence a partir de gravacoes do jogo real.

Molde: client/tools/criativos.py do Dungeon Courier. So gameplay real: cada cena e um trecho de uma gravacao de tela do build
(o robo -roteiro ou uma pessoa jogando). Por cima vai so a legenda do idioma e, no fim, o cartao de 2 s (icone + nome em Cinzel
+ "Gratis no Google Play"). Som: o da gravacao; sem ele, silencio. Nenhuma musica de terceiros. Nada de promessa que o build nao
cumpre (docs/LOJA.md, regra dos textos).

Uso (da raiz do repositorio; ffmpeg e ffprobe no PATH):
  python -I client/tools/criativos.py --comandos                imprime como gravar cada cena (PC e Android)
  python -I client/tools/criativos.py --listar                  roteiros, duracao e quais gravacoes faltam
  python -I client/tools/criativos.py --roteiro all --lang pt-BR [--rec client/Builds/rec] [--saida client/Builds/criativos]
        monta coe_<roteiro>_<lang>_paisagem.mp4 (1920x1080) e _vertical.mp4 (1080x1920: o jogo no meio, a mesma cena
        desfocada atras), H.264 30 fps e AAC 48 kHz
  python -I client/tools/criativos.py --autoteste               gravacao sintetica de 4 s, monta os dois formatos e confere

Gravacoes: <rec>/<gravacao>_<lang>.mp4. Sem a do idioma, usa a do pt-BR (o HUD fica em portugues; o ideal e gravar no idioma).
ponytail: os tempos de cada cena (inicio/duracao) sao HIPOTESE ate a primeira gravacao: o -roteiro nao grava video, entao
quem grava e o OBS no PC ou o `adb shell screenrecord` no aparelho, e o inicio de cada beat muda com a maquina. Medir na
primeira gravacao e corrigir a tabela G (o --listar mostra o que falta).
"""
import argparse, json, os, shutil, subprocess, sys, tempfile
from PIL import Image, ImageDraw

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
from icone_app import fundo as fundo_limiar, frente as simbolo_limiar, AZUL_FUNDO, DOURADO, MARFIM  # noqa: E402
from loja_assets import CINZEL, NUNITO, LANGS, TITULO, texto_cabe  # noqa: E402

CLIENT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
REC = os.path.join(CLIENT, "Builds", "rec")
SAIDA = os.path.join(CLIENT, "Builds", "criativos")
FPS, CTA = 30, 2.0
FORMATOS = {"paisagem": (1920, 1080), "vertical": (1080, 1920)}

# Gravacoes: nome -> (como gravar, nota). Tempos das cenas ficam nos roteiros (R), em segundos da gravacao.
G = {
    "nascimento": ("COE.exe -roteiro -lang <lang> (OBS gravando a janela do inicio ate Auren)", "Limiar, Destino, Origem, Certeza"),
    "auren": ("o mesmo -roteiro, de Auren aos 5 anos ate o salto", "despertar, primeira fala, promessa da Q-04"),
    "salto": ("o mesmo -roteiro, do aviso do salto ao treino", "salto de 3 anos, corpo maior, espada de madeira"),
    "treino": ("o mesmo -roteiro, o treino com o Tovin", "ataque, defesa e primeira magia"),
}
LEGENDAS = {
    "limiar": {"pt-BR": "Antes de nascer, você escolhe.", "en": "Before you're born, you choose.",
               "es": "Antes de nacer, tú eliges.", "id": "Sebelum lahir, kamu memilih."},
    "destino": {"pt-BR": "Quatro destinos. Para sempre.", "en": "Four destinies. Forever.",
                "es": "Cuatro destinos. Para siempre.", "id": "Empat takdir. Selamanya."},
    "acorda": {"pt-BR": "Você acorda com cinco anos.", "en": "You wake up at five.",
               "es": "Despiertas a los cinco años.", "id": "Kamu bangun di usia lima tahun."},
    "lembra": {"pt-BR": "A vila lembra o que você fez.", "en": "The village remembers.",
               "es": "La aldea recuerda lo que hiciste.", "id": "Desa mengingat apa yang kamu lakukan."},
    "cresce": {"pt-BR": "Três anos depois...", "en": "Three years later...",
               "es": "Tres años después...", "id": "Tiga tahun kemudian..."},
    "treina": {"pt-BR": "Espada de madeira. Primeira magia.", "en": "A wooden sword. Your first spell.",
               "es": "Espada de madera. Tu primera magia.", "id": "Pedang kayu. Sihir pertamamu."},
}
CTA_TXT = {"pt-BR": "Grátis no Google Play", "en": "Free on Google Play", "es": "Gratis en Google Play", "id": "Gratis di Google Play"}
# Roteiros: nome -> cenas (gravacao, inicio s, duracao s, legenda). HIPOTESE de tempo: ver o ponytail do cabecalho.
R = {
    "nascer": [("nascimento", 0.0, 4.0, "limiar"), ("nascimento", 6.0, 4.0, "destino"), ("auren", 1.0, 5.0, "acorda")],
    "vila": [("auren", 1.0, 3.0, "acorda"), ("auren", 20.0, 6.0, "lembra")],
    "crescer": [("salto", 0.0, 4.0, "cresce"), ("treino", 2.0, 6.0, "treina")],
}


def ff(*args):
    for exe in ("ffmpeg", "ffprobe"):
        if not shutil.which(exe):
            sys.exit("%s nao achado no PATH" % exe)
    r = subprocess.run(["ffmpeg", "-y", "-hide_banner", "-loglevel", "error"] + list(args), capture_output=True, text=True)
    if r.returncode:
        raise RuntimeError("ffmpeg falhou: " + r.stderr[-800:])


def tem_audio(arq):
    r = subprocess.run(["ffprobe", "-v", "error", "-select_streams", "a", "-show_entries", "stream=index", "-of", "csv=p=0", arq],
                       capture_output=True, text=True)
    return bool(r.stdout.strip())


def gravacao(rec, nome, lang):
    for lg in (lang, "pt-BR"):
        p = os.path.join(rec, "%s_%s.mp4" % (nome, lg))
        if os.path.isfile(p):
            return p
    return None


def legenda_png(txt, w, h, caminho):
    """Faixa da legenda (azul profundo com filete dourado, texto em Cinzel), transparente fora dela."""
    img = Image.new("RGBA", (w, h), (0, 0, 0, 0))
    d = ImageDraw.Draw(img)
    alt = int(h * (0.13 if w > h else 0.08))
    y0 = int(h * (0.06 if w > h else 0.18))
    d.rectangle((0, y0, w, y0 + alt), fill=AZUL_FUNDO + (215,))
    d.line((0, y0 + alt, w, y0 + alt), fill=DOURADO + (220,), width=3)
    f = texto_cabe(d, txt, CINZEL, int(alt * 0.5), w - 120, 28)
    d.text((w // 2, y0 + alt // 2), txt, font=f, fill=MARFIM, anchor="mm")
    img.save(caminho)


def cta_png(lang, w, h, caminho):
    bg = fundo_limiar(max(w, h)).crop(((max(w, h) - w) // 2, (max(w, h) - h) // 2, (max(w, h) - w) // 2 + w, (max(w, h) - h) // 2 + h))
    lado = int(min(w, h) * 0.55)
    bg.alpha_composite(simbolo_limiar(lado, 0.95), ((w - lado) // 2, int(h * 0.42) - lado // 2))
    d = ImageDraw.Draw(bg)
    ft = texto_cabe(d, TITULO, CINZEL, int(min(w, h) * 0.08), w - 120)
    d.text((w // 2, int(h * 0.72)), TITULO, font=ft, fill=MARFIM, anchor="mm")
    fs = texto_cabe(d, CTA_TXT[lang], NUNITO, int(min(w, h) * 0.05), w - 160, 20)
    d.text((w // 2, int(h * 0.80)), CTA_TXT[lang], font=fs, fill=DOURADO, anchor="mm")
    bg.convert("RGB").save(caminho)


def cena(src, ini, dur, leg_png, formato, saida):
    """Um trecho da gravacao no formato, com a legenda por cima e audio AAC (silencio se a gravacao nao tiver)."""
    w, h = FORMATOS[formato]
    if formato == "paisagem":
        filtro = "[0:v]scale=%d:%d:force_original_aspect_ratio=increase,crop=%d:%d,setsar=1,fps=%d[v];" % (w, h, w, h, FPS)
    else:   # o jogo e paisagem: no vertical vai inteiro no meio, com a propria cena desfocada atras
        filtro = ("[0:v]split[a][b];[a]scale=%d:%d:force_original_aspect_ratio=increase,crop=%d:%d,boxblur=24:2,eq=brightness=-0.12[f];"
                  "[b]scale=%d:-2[m];[f][m]overlay=(W-w)/2:(H-h)/2,setsar=1,fps=%d[v];" % (w, h, w, h, w, FPS))
    filtro += "[v][1:v]overlay=0:0[out]"
    entradas = ["-ss", str(ini), "-t", str(dur), "-i", src, "-i", leg_png]
    if tem_audio(src):
        audio = ["-map", "0:a:0"]
    else:
        entradas += ["-f", "lavfi", "-t", str(dur), "-i", "anullsrc=r=48000:cl=stereo"]
        audio = ["-map", "2:a:0"]
    ff(*entradas, "-filter_complex", filtro, "-map", "[out]", *audio, "-t", str(dur),
       "-c:v", "libx264", "-pix_fmt", "yuv420p", "-preset", "veryfast", "-crf", "20", "-c:a", "aac", "-ar", "48000", "-ac", "2", saida)


def montar(roteiro, lang, rec, saida, tmp):
    feitos = []
    for formato, (w, h) in FORMATOS.items():
        partes = []
        for i, (grav, ini, dur, leg) in enumerate(R[roteiro]):
            src = gravacao(rec, grav, lang)
            if not src:
                print("FALTA %s/%s: gravacao %s_%s.mp4 (nem a do pt-BR)" % (roteiro, lang, grav, lang))
                return feitos
            lp = os.path.join(tmp, "leg_%s_%d.png" % (formato, i))
            legenda_png(LEGENDAS[leg][lang], w, h, lp)
            p = os.path.join(tmp, "%s_%s_%02d.mp4" % (roteiro, formato, i))
            cena(src, ini, dur, lp, formato, p)
            partes.append(p)
        cp = os.path.join(tmp, "cta_%s.png" % formato)
        cta_png(lang, w, h, cp)
        cta = os.path.join(tmp, "%s_%s_cta.mp4" % (roteiro, formato))
        ff("-loop", "1", "-t", str(CTA), "-i", cp, "-f", "lavfi", "-t", str(CTA), "-i", "anullsrc=r=48000:cl=stereo",
           "-r", str(FPS), "-c:v", "libx264", "-pix_fmt", "yuv420p", "-preset", "veryfast", "-c:a", "aac", "-ar", "48000", "-ac", "2", cta)
        partes.append(cta)
        lista = os.path.join(tmp, "lista_%s_%s.txt" % (roteiro, formato))
        with open(lista, "w") as f:
            f.writelines("file '%s'\n" % p.replace("\\", "/") for p in partes)
        os.makedirs(saida, exist_ok=True)
        out = os.path.join(saida, "coe_%s_%s_%s.mp4" % (roteiro, lang, formato))
        ff("-f", "concat", "-safe", "0", "-i", lista, "-c", "copy", "-movflags", "+faststart", out)
        print("OK", os.path.relpath(out))
        feitos.append(out)
    return feitos


def sonda(arq):
    r = subprocess.run(["ffprobe", "-v", "error", "-show_entries", "stream=codec_type,width,height:format=duration", "-of", "json", arq],
                       capture_output=True, text=True)
    j = json.loads(r.stdout)
    v = next(s for s in j["streams"] if s["codec_type"] == "video")
    return v["width"], v["height"], float(j["format"]["duration"]), any(s["codec_type"] == "audio" for s in j["streams"])


def autoteste():
    erros = []
    for nome, cenas in R.items():
        for grav, ini, dur, leg in cenas:
            if grav not in G:
                erros.append("%s usa gravacao desconhecida %s" % (nome, grav))
            if leg not in LEGENDAS:
                erros.append("%s usa legenda desconhecida %s" % (nome, leg))
    for chave, txt in list(LEGENDAS.items()) + [("cta", CTA_TXT)]:
        for lang in LANGS:
            if not txt.get(lang):
                erros.append("legenda %s sem %s" % (chave, lang))
    if not (shutil.which("ffmpeg") and shutil.which("ffprobe")):
        erros.append("ffmpeg/ffprobe fora do PATH: o autoteste precisa deles")
    else:
        with tempfile.TemporaryDirectory() as t:
            rec, saida, tmp = os.path.join(t, "rec"), os.path.join(t, "out"), os.path.join(t, "tmp")
            os.makedirs(rec)
            os.makedirs(tmp)
            # uma gravacao 20:9 de 4 s sem som (como o screenrecord) serve para o roteiro de teste
            ff("-f", "lavfi", "-i", "testsrc2=size=1200x540:rate=30", "-t", "4", "-c:v", "libx264", "-pix_fmt", "yuv420p",
               os.path.join(rec, "auren_pt-BR.mp4"))
            R["_teste"] = [("auren", 0.0, 1.5, "acorda"), ("auren", 2.0, 1.0, "lembra")]
            try:
                feitos = montar("_teste", "es", rec, saida, tmp)   # es sem gravacao propria: cai na do pt-BR
            finally:
                del R["_teste"]
            if len(feitos) != 2:
                erros.append("montou %d de 2 formatos" % len(feitos))
            for arq in feitos:
                w, h, dur, audio = sonda(arq)
                formato = "paisagem" if "paisagem" in arq else "vertical"
                if (w, h) != FORMATOS[formato]:
                    erros.append("%s: %dx%d" % (os.path.basename(arq), w, h))
                if abs(dur - (1.5 + 1.0 + CTA)) > 0.25:
                    erros.append("%s: %.2f s (esperado %.1f)" % (os.path.basename(arq), dur, 1.5 + 1.0 + CTA))
                if not audio:
                    erros.append("%s sem trilha de audio" % os.path.basename(arq))
    for e in erros:
        print("FALHOU", e)
    print("criativos --autoteste:", "FALHOU" if erros else "OK")
    return 1 if erros else 0


def listar(rec):
    for nome, cenas in R.items():
        dur = sum(c[2] for c in cenas) + CTA
        faltam = sorted({"%s_<lang>.mp4" % c[0] for c in cenas if not gravacao(rec, c[0], "pt-BR")})
        print("%-8s %4.1f s  %s" % (nome, dur, "pronto para montar" if not faltam else "falta: " + ", ".join(faltam)))


def comandos():
    print("Gravar em %s\\<gravacao>_<lang>.mp4 (1 gravacao por idioma; sem a do idioma, usa a do pt-BR):" % os.path.relpath(REC))
    for nome, (como, nota) in G.items():
        print("  %-11s %s  [%s]" % (nome, como, nota))
    print("Android (aparelho em paisagem): adb shell screenrecord --size 1920x1080 /sdcard/<gravacao>_<lang>.mp4 ; "
          "adb pull /sdcard/<gravacao>_<lang>.mp4 client/Builds/rec/")
    print("Depois: confira os tempos da tabela R (--listar) e rode --roteiro all --lang <lang>.")


if __name__ == "__main__":
    ap = argparse.ArgumentParser()
    ap.add_argument("--roteiro", help="nome do roteiro ou all")
    ap.add_argument("--lang", default="pt-BR")
    ap.add_argument("--rec", default=REC)
    ap.add_argument("--saida", default=SAIDA)
    ap.add_argument("--listar", action="store_true")
    ap.add_argument("--comandos", action="store_true")
    ap.add_argument("--autoteste", action="store_true")
    a = ap.parse_args()
    if a.autoteste:
        sys.exit(autoteste())
    if a.listar:
        listar(a.rec)
        sys.exit(0)
    if a.comandos or not a.roteiro:
        comandos()
        sys.exit(0)
    if a.lang not in LANGS:
        sys.exit("idioma desconhecido: %s (use %s)" % (a.lang, ", ".join(LANGS)))
    nomes = list(R) if a.roteiro == "all" else [a.roteiro]
    with tempfile.TemporaryDirectory() as tmp:
        total = sum(len(montar(n, a.lang, a.rec, a.saida, tmp)) for n in nomes)
    sys.exit(0 if total == 2 * len(nomes) else 1)
