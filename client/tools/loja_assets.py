"""loja_assets.py - artes da pagina da Google Play do Chronicles of Existence (Python 3 + Pillow + numpy, fora do Unity).

Molde: client/tools/loja_assets.py do Dungeon Courier. Nada de arte nova: o icone do icone_app.py, as fontes OFL do jogo
(Cinzel e Nunito, Resources/Fontes) e as fotos reais do build (as do robo -roteiro, docs/LOJA.md sec. 5).

Uso (da raiz do repositorio):
  python -I client/tools/loja_assets.py [--fotos client/Builds/loja_fotos] [--saida client/Builds/loja] [--lang pt,en]
  python -I client/tools/loja_assets.py --autoteste      (fotos sinteticas numa pasta temporaria; confere tamanhos e formatos)

Fotos: <fotos>/<lang>/NN_<etapa>.png, a pasta roteiro/ de uma rodada `COE.exe -roteiro -lang <lang>` (client/tools/loja_fotos.ps1
copia). Sem a pasta do idioma, usa a do pt-BR (o HUD fica em portugues; o ideal e fotografar em cada idioma).

Saida em client/Builds/loja/ (fora do git):
  icone_512.png                  o icone do app (Assets/_COE/Icon/icone.png): a loja e o aparelho mostram o mesmo
  feature_1024x500_<lang>.png    nome do jogo em Cinzel + a frase do idioma sobre uma foto de Auren escurecida
                                 (sem foto: o fundo azul do icone com a luz do Limiar)
  capturas/<lang>/NN_<nome>.png  as capturas de docs/LOJA.md sec. 5 em 1920x1080 RGB (paisagem, como o jogo), com a legenda
                                 do idioma numa faixa no topo. Foto que falta e avisada (FALTA) e o resto sai.
"""
import argparse, glob, os, shutil, sys, tempfile
from PIL import Image, ImageDraw, ImageFilter, ImageFont

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
from icone_app import fundo as fundo_limiar, frente as simbolo_limiar, AZUL_FUNDO, DOURADO, MARFIM  # noqa: E402

CLIENT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
FONTES = os.path.join(CLIENT, "Assets", "_COE", "Resources", "Fontes")
CINZEL, NUNITO = os.path.join(FONTES, "Cinzel-Bold.ttf"), os.path.join(FONTES, "Nunito-Regular.ttf")
ICONE_APP = os.path.join(CLIENT, "Assets", "_COE", "Icon", "icone.png")
FOTOS = os.path.join(CLIENT, "Builds", "loja_fotos")
SAIDA = os.path.join(CLIENT, "Builds", "loja")
LANGS = ("pt-BR", "en", "es", "id")
CAP_W, CAP_H, FAIXA = 1920, 1080, 150
TITULO = "Chronicles of Existence"

# docs/LOJA.md sec. 5, na ordem da loja: (nome da captura, etapas do -roteiro em ordem de preferencia, legenda por idioma).
# A etapa e o nome da foto sem o NN_ (Roteiro.Foto). Legenda so promete o que o build tem (regra do LOJA.md).
CAPTURAS = [
    ("01_escolha_como_nascer", ("nascimento_destino",),
     {"pt-BR": "Escolha como nascer", "en": "Choose how you're born", "es": "Elige cómo nacer", "id": "Pilih cara kamu lahir"}),
    ("02_tres_familias", ("nascimento_origem",),
     {"pt-BR": "Três famílias possíveis em cada destino", "en": "Three possible families for each destiny",
      "es": "Tres familias posibles en cada destino", "id": "Tiga keluarga yang mungkin di tiap takdir"}),
    ("03_acorde_em_auren", ("auren_inicio",),
     {"pt-BR": "Acorde aos cinco anos na vila de Auren", "en": "Wake up at five in the village of Auren",
      "es": "Despierta a los cinco años en la aldea de Auren", "id": "Bangun di usia lima tahun di desa Auren"}),
    ("04_a_vila_lembra", ("fala_*",),
     {"pt-BR": "A vila lembra o que você fez", "en": "The village remembers what you did",
      "es": "La aldea recuerda lo que hiciste", "id": "Desa mengingat apa yang kamu lakukan"}),
    ("05_tres_anos_depois", ("tres_anos_depois", "salto_certeza"),
     {"pt-BR": "Cresça de verdade: três anos depois", "en": "Really grow up: three years later",
      "es": "Crece de verdad: tres años después", "id": "Benar-benar tumbuh: tiga tahun kemudian"}),
    ("06_primeiro_treino", ("treino_painel", "aos8_*"),
     {"pt-BR": "Primeiro treino de espada e de magia", "en": "Your first sword and magic training",
      "es": "Tu primer entrenamiento de espada y magia", "id": "Latihan pedang dan sihir pertamamu"}),
    ("07_o_limiar", ("nascimento_limiar",),
     {"pt-BR": "Antes de nascer, o Guardião do Limiar", "en": "Before birth, the Guardian of the Threshold",
      "es": "Antes de nacer, el Guardián del Umbral", "id": "Sebelum lahir, Penjaga Ambang"}),
    ("08_o_bosque_chama", ("gancho",),
     {"pt-BR": "O Bosque dos Sussurros chama", "en": "The Whispering Wood is calling",
      "es": "El Bosque de los Susurros te llama", "id": "Hutan Bisikan memanggil"}),
]
# frase da feature graphic (a descricao curta encurtada; LOJA.md sec. 3)
FRASE = {"pt-BR": "Escolha como nascer. Viva a infância. Cresça.", "en": "Choose how you're born. Live your childhood. Grow up.",
         "es": "Elige cómo nacer. Vive tu infancia. Crece.", "id": "Pilih cara lahir. Jalani masa kecil. Tumbuh."}
FUNDO_FEATURE = ("auren_inicio", "tres_anos_depois", "gancho")   # a primeira foto que existir vira o fundo da feature graphic


def fonte(caminho, tam):
    try:
        return ImageFont.truetype(caminho, tam)
    except OSError:
        print("AVISO fonte ausente (%s): usando a padrao do Pillow" % os.path.basename(caminho))
        return ImageFont.load_default(tam)


def achar(pasta_fotos, lang, etapas):
    """A primeira foto que existir para as etapas, no idioma; sem ela, a do pt-BR."""
    for lg in (lang, "pt-BR"):
        for e in etapas:
            achadas = sorted(glob.glob(os.path.join(pasta_fotos, lg, "[0-9][0-9]_" + e + ".png")))
            if achadas:
                return achadas[0]
    return None


def cobrir(img, w, h):
    """Redimensiona e recorta no centro para preencher w x h (a foto 20:9 do aparelho perde as pontas, nao ganha faixa)."""
    s = max(w / img.width, h / img.height)
    img = img.resize((round(img.width * s), round(img.height * s)), Image.LANCZOS)
    x, y = (img.width - w) // 2, (img.height - h) // 2
    return img.crop((x, y, x + w, y + h))


def texto_cabe(d, txt, caminho, tam, larg_max, minimo=24):
    f = fonte(caminho, tam)
    while tam > minimo and d.textlength(txt, font=f) > larg_max:
        tam -= 2
        f = fonte(caminho, tam)
    return f


def captura(foto, legenda):
    img = cobrir(Image.open(foto).convert("RGB"), CAP_W, CAP_H).convert("RGBA")
    faixa = Image.new("RGBA", (CAP_W, FAIXA + 40), (0, 0, 0, 0))
    fd = ImageDraw.Draw(faixa)
    for y in range(FAIXA + 40):   # azul profundo que some para baixo (nao corta a cena)
        a = 225 if y < FAIXA else int(225 * (1 - (y - FAIXA) / 40))
        fd.line((0, y, CAP_W, y), fill=AZUL_FUNDO + (a,))
    fd.line((0, FAIXA - 3, CAP_W, FAIXA - 3), fill=DOURADO + (200,), width=3)
    img.alpha_composite(faixa)
    d = ImageDraw.Draw(img)
    f = texto_cabe(d, legenda, CINZEL, 76, CAP_W - 160)
    d.text((CAP_W // 2, FAIXA // 2 - 2), legenda, font=f, fill=MARFIM, anchor="mm")
    return img.convert("RGB")


def feature(lang, foto):
    W, H = 1024, 500
    if foto:
        bg = cobrir(Image.open(foto).convert("RGB"), W, H).filter(ImageFilter.GaussianBlur(3)).convert("RGBA")
        veu = Image.new("RGBA", (W, H))
        vd = ImageDraw.Draw(veu)
        for x in range(W):   # escurece da esquerda (onde vai o texto) para a direita
            vd.line((x, 0, x, H), fill=AZUL_FUNDO + (int(235 - 150 * x / W),))
        bg.alpha_composite(veu)
    else:
        bg = cobrir(fundo_limiar(W).convert("RGB"), W, H).convert("RGBA")
    simb = simbolo_limiar(420, 0.9)
    bg.alpha_composite(simb, (W - 400, (H - 420) // 2))
    d = ImageDraw.Draw(bg)
    ft = texto_cabe(d, TITULO, CINZEL, 64, 640)
    d.text((56, H // 2 - 30), TITULO, font=ft, fill=MARFIM, anchor="ls")
    d.line((58, H // 2 - 6, 58 + min(560, d.textlength(TITULO, font=ft)), H // 2 - 6), fill=DOURADO, width=3)
    fs = texto_cabe(d, FRASE[lang], NUNITO, 34, 600, 20)
    d.text((58, H // 2 + 40), FRASE[lang], font=fs, fill=MARFIM, anchor="ls")
    return bg.convert("RGB")


def gerar(pasta_fotos, saida, langs, icone=ICONE_APP):
    os.makedirs(saida, exist_ok=True)
    faltas = 0
    if os.path.isfile(icone):
        im = Image.open(icone).convert("RGB")
        if im.size != (512, 512):
            im = im.resize((512, 512), Image.LANCZOS)
        im.save(os.path.join(saida, "icone_512.png"))
        print("OK icone_512.png")
    else:
        print("FALTA icone (rode client/tools/icone_app.py)")
        faltas += 1
    for lang in langs:
        fundo_foto = next((p for p in (achar(pasta_fotos, lang, (e,)) for e in FUNDO_FEATURE) if p), None)
        feature(lang, fundo_foto).save(os.path.join(saida, "feature_1024x500_%s.png" % lang))
        print("OK feature_1024x500_%s.png%s" % (lang, "" if fundo_foto else " (sem foto: fundo do Limiar)"))
        pasta = os.path.join(saida, "capturas", lang)
        os.makedirs(pasta, exist_ok=True)
        for nome, etapas, legenda in CAPTURAS:
            foto = achar(pasta_fotos, lang, etapas)
            if not foto:
                print("FALTA %s/%s (nenhuma foto de %s)" % (lang, nome, ", ".join(etapas)))
                faltas += 1
                continue
            captura(foto, legenda[lang]).save(os.path.join(pasta, nome + ".png"))
            print("OK capturas/%s/%s.png <- %s" % (lang, nome, os.path.relpath(foto, pasta_fotos)))
    return faltas


def autoteste():
    erros = []
    for nome, _, leg in CAPTURAS:
        for lang in LANGS:
            if not leg.get(lang):
                erros.append("legenda sem %s em %s" % (lang, nome))
    for lang in LANGS:
        if not FRASE.get(lang):
            erros.append("frase da feature sem " + lang)
    for f in (CINZEL, NUNITO):
        if not os.path.isfile(f):
            erros.append("fonte ausente: " + f)
    with tempfile.TemporaryDirectory() as t:
        fotos, saida = os.path.join(t, "fotos"), os.path.join(t, "loja")
        # pt-BR com todas as etapas (foto 20:9 como a do aparelho); en com so uma (o resto cai no pt-BR)
        os.makedirs(os.path.join(fotos, "pt-BR"))
        os.makedirs(os.path.join(fotos, "en"))
        etapas = ["nascimento_limiar", "nascimento_destino", "nascimento_origem", "auren_inicio", "fala_mara", "tres_anos_depois",
                  "treino_painel", "gancho"]
        for i, e in enumerate(etapas):
            Image.new("RGB", (2400, 1080), (40 + i * 20, 90, 120)).save(os.path.join(fotos, "pt-BR", "%02d_%s.png" % (i + 1, e)))
        Image.new("RGB", (1920, 1080), (200, 160, 90)).save(os.path.join(fotos, "en", "04_auren_inicio.png"))
        icone = os.path.join(t, "icone.png")
        Image.new("RGB", (512, 512), (37, 56, 80)).save(icone)
        faltas = gerar(fotos, saida, ("pt-BR", "en"), icone)
        if faltas:
            erros.append("%d FALTA com todas as fotos presentes" % faltas)
        esperado = {"icone_512.png": (512, 512), "feature_1024x500_pt-BR.png": (1024, 500), "feature_1024x500_en.png": (1024, 500)}
        for lang in ("pt-BR", "en"):
            for nome, _, _ in CAPTURAS:
                esperado[os.path.join("capturas", lang, nome + ".png")] = (CAP_W, CAP_H)
        for rel, tam in esperado.items():
            p = os.path.join(saida, rel)
            if not os.path.isfile(p):
                erros.append("nao saiu " + rel)
                continue
            im = Image.open(p)
            if im.size != tam or im.mode != "RGB":
                erros.append("%s: %s %s (esperado %s RGB)" % (rel, im.size, im.mode, tam))
        # o en usou a propria foto de Auren (cor laranja no centro) e nao a do pt-BR
        c = Image.open(os.path.join(saida, "capturas", "en", "03_acorde_em_auren.png")).getpixel((CAP_W // 2, CAP_H - 50))
        if c[0] < 150:
            erros.append("captura en de Auren nao usou a foto en (%s)" % (c,))
        # sem foto nenhuma: avisa FALTA e nao quebra
        if gerar(os.path.join(t, "vazio"), os.path.join(t, "loja2"), ("es",), icone) != len(CAPTURAS):
            erros.append("pasta sem fotos deveria dar FALTA em todas as capturas")
    for e in erros:
        print("FALHOU", e)
    print("loja_assets --autoteste:", "FALHOU" if erros else "OK")
    return 1 if erros else 0


if __name__ == "__main__":
    ap = argparse.ArgumentParser()
    ap.add_argument("--fotos", default=FOTOS)
    ap.add_argument("--saida", default=SAIDA)
    ap.add_argument("--lang", default=",".join(LANGS), help="idiomas separados por virgula (pt-BR,en,es,id)")
    ap.add_argument("--autoteste", action="store_true")
    a = ap.parse_args()
    if a.autoteste:
        sys.exit(autoteste())
    langs = [l for l in a.lang.split(",") if l]
    for l in langs:
        if l not in LANGS:
            sys.exit("idioma desconhecido: %s (use %s)" % (l, ", ".join(LANGS)))
    if os.path.isdir(a.saida):
        shutil.rmtree(a.saida)
    n = gerar(a.fotos, a.saida, langs)
    print("pronto em %s%s" % (a.saida, " (%d FALTA)" % n if n else ""))
