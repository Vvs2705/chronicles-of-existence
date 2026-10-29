"""icone_app.py - icone do app do Chronicles of Existence (Python 3 + Pillow + numpy, fora do Unity).

Desenho por codigo, sem arte de terceiros e sem malha (nao passa pelos portoes do ADR-0002): o Limiar. Um arco de porta em
dourado sobre o azul profundo, com o simbolo da Trama dentro (circulo incompleto e pontos ligados, em turquesa), como manda o
ART_BIBLE sec. 2, 3 e 6. Paleta: GDD cap. 09 (azul #253850, dourado #D6B36A, turquesa #86C8C9, marfim #E9DEC6).
ponytail: icone de lancamento gerado por codigo. Se o idealizador quiser um pintado (a partir do concept do Aethron ou do
avatar, G2), troca so os 3 PNGs; o ProjectSetup.AplicarIcone e a loja seguem iguais.

Uso (da raiz do repositorio):
  python -I client/tools/icone_app.py               grava os PNGs do icone
  python -I client/tools/icone_app.py --autoteste   desenha numa pasta temporaria e confere tamanho, zona segura e contraste

Saidas em client/Assets/_COE/Icon/ (fora de Resources: so o ProjectSetup.AplicarIcone usa, nao entra no jogo):
  icone_fundo.png   432x432  camada de tras do icone adaptativo do Android
  icone_frente.png  432x432  camada da frente, transparente, dentro da zona segura (o Android corta ate 1/3 da borda)
  icone.png         512x512  as duas camadas juntas, sem transparencia: Windows e o icone da Play (loja_assets.py copia)
"""
import argparse, math, os, sys, tempfile
import numpy as np
from PIL import Image, ImageDraw, ImageFilter

CLIENT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
SAIDA = os.path.join(CLIENT, "Assets", "_COE", "Icon")
AZUL, AZUL_FUNDO, DOURADO, TURQUESA, MARFIM = (0x25, 0x38, 0x50), (0x0F, 0x18, 0x26), (0xD6, 0xB3, 0x6A), (0x86, 0xC8, 0xC9), (0xE9, 0xDE, 0xC6)
SS = 4                  # supersample: desenha a 4x e reduz (borda lisa sem antialias do ImageDraw)
SEGURA = 0.66           # zona segura do adaptativo: o circulo de 66% do lado sempre aparece


def fundo(w):
    """Azul profundo com luz dourada saindo do meio (de dentro do Limiar)."""
    y, x = np.mgrid[0:w, 0:w] / (w - 1) - 0.5
    r = np.clip(np.hypot(x, y) / 0.7071, 0, 1)[..., None]
    rgb = np.array(AZUL) * (1 - r) + np.array(AZUL_FUNDO) * r
    luz = np.exp(-(x * x * 1.6 + (y - 0.02) ** 2) / 0.035)[..., None] * 0.30
    rgb = rgb * (1 - luz) + np.array(DOURADO) * luz
    return Image.fromarray(rgb.astype(np.uint8), "RGB").convert("RGBA")


def frente(w, segura=SEGURA):
    """Arco do Limiar + Trama, centrado e dentro do circulo de `segura` * w."""
    W = w * SS
    img = Image.new("RGBA", (W, W), (0, 0, 0, 0))
    d = ImageDraw.Draw(img)
    c = W / 2
    lado = W * segura * 0.74                     # o desenho todo cabe num quadrado dentro do circulo seguro
    larg, alt = lado * 0.62, lado * 0.92
    x0, x1 = c - larg / 2, c + larg / 2
    topo, base = c - alt / 2, c + alt / 2
    esp = W * 0.028                               # espessura do arco
    # arco: semicirculo em cima + pilares retos ate a base
    raio = larg / 2
    d.arc((x0, topo, x1, topo + 2 * raio), 180, 360, fill=DOURADO + (255,), width=int(esp))
    for x in (x0 + esp / 2, x1 - esp / 2):
        d.line((x, topo + raio, x, base), fill=DOURADO + (255,), width=int(esp))
    # degrau do limiar
    d.rounded_rectangle((x0 - esp * 1.6, base - esp * 0.2, x1 + esp * 1.6, base + esp * 1.1), radius=esp * 0.4, fill=DOURADO + (255,))
    # luz de dentro do Limiar: o vao do arco acende de baixo para cima
    vao = Image.new("L", (W, W), 0)
    dv = ImageDraw.Draw(vao)
    dv.pieslice((x0 + esp, topo + esp, x1 - esp, topo + 2 * raio - esp), 180, 360, fill=255)
    dv.rectangle((x0 + esp, topo + raio, x1 - esp, base - esp * 0.2), fill=255)
    grad = Image.linear_gradient("L").resize((W, W)).point(lambda v: int(v * 0.24))
    luz = Image.new("RGBA", (W, W), DOURADO + (0,))
    luz.putalpha(Image.composite(grad, Image.new("L", (W, W), 0), vao))
    img.alpha_composite(luz)
    d = ImageDraw.Draw(img)
    # Trama: circulo incompleto (aberto em cima a direita), um fio sai da abertura e liga dois pontos (ART_BIBLE sec. 6)
    tc, tr, te = (c - larg * 0.04, topo + raio + alt * 0.17), larg * 0.27, W * 0.018
    d.arc((tc[0] - tr, tc[1] - tr, tc[0] + tr, tc[1] + tr), 345, 285, fill=TURQUESA + (255,), width=int(te))
    pts = [(tc[0] + tr * math.cos(math.radians(a)), tc[1] + tr * math.sin(math.radians(a))) for a in (345, 285)]
    fora = (tc[0] + tr * 0.85, tc[1] - tr * 1.25)
    d.line([pts[1], fora], fill=TURQUESA + (240,), width=int(te * 0.7))
    for p, rp in ((pts[0], te * 1.3), (pts[1], te * 1.3), (fora, te * 1.5), (tc, te * 1.1)):
        d.ellipse((p[0] - rp, p[1] - rp, p[0] + rp, p[1] + rp), fill=MARFIM + (255,))
    img = img.resize((w, w), Image.LANCZOS)
    # brilho suave em volta do que e desenhado (o Limiar "acende"), por baixo
    halo = Image.new("RGBA", (w, w), DOURADO + (0,))
    halo.putalpha(img.getchannel("A").filter(ImageFilter.GaussianBlur(w / 40)).point(lambda v: v * 0.55))
    halo.alpha_composite(img)
    return halo


def gerar(saida):
    os.makedirs(saida, exist_ok=True)
    f, fr = fundo(432), frente(432)
    f.convert("RGB").save(os.path.join(saida, "icone_fundo.png"))
    fr.save(os.path.join(saida, "icone_frente.png"))
    junto = fundo(512)
    junto.alpha_composite(frente(512, 0.84))   # a Play arredonda so os cantos: o desenho pode crescer
    junto.convert("RGB").save(os.path.join(saida, "icone.png"))
    return [os.path.join(saida, n) for n in ("icone_fundo.png", "icone_frente.png", "icone.png")]


def autoteste():
    with tempfile.TemporaryDirectory() as t:
        fundo_p, frente_p, icone_p = gerar(t)
        erros = []
        tam = {fundo_p: (432, "RGB"), frente_p: (432, "RGBA"), icone_p: (512, "RGB")}
        for p, (lado, modo) in tam.items():
            im = Image.open(p)
            if im.size != (lado, lado) or im.mode != modo:
                erros.append("%s: %s %s (esperado %dx%d %s)" % (os.path.basename(p), im.size, im.mode, lado, lado, modo))
        # nada da frente fora do circulo seguro (o Android corta)
        a = np.asarray(Image.open(frente_p).getchannel("A")).astype(float)
        y, x = np.mgrid[0:432, 0:432] - 215.5
        fora = a[np.hypot(x, y) > 432 * SEGURA / 2].max()
        if fora > 64:   # o halo pode passar um pouco (o corte so apaga brilho); o desenho, nao
            erros.append("frente vaza da zona segura (alfa %d fora do circulo de %d%%)" % (fora, SEGURA * 100))
        if a.max() < 250:
            erros.append("frente vazia")
        # o arco precisa ler contra o fundo (diferenca de luminancia media)
        ic = np.asarray(Image.open(icone_p).convert("L")).astype(float)
        if ic[ic > 150].size < 2000:
            erros.append("pouco dourado claro no icone (%d px)" % ic[ic > 150].size)
        for e in erros:
            print("FALHOU", e)
        print("icone_app --autoteste:", "FALHOU" if erros else "OK")
        return 1 if erros else 0


if __name__ == "__main__":
    ap = argparse.ArgumentParser()
    ap.add_argument("--autoteste", action="store_true")
    ap.add_argument("--saida", default=SAIDA)
    a = ap.parse_args()
    if a.autoteste:
        sys.exit(autoteste())
    for p in gerar(a.saida):
        print("OK", os.path.relpath(p))
