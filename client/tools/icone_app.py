"""icone_app.py - icone do app do Chronicles of Existence (Python 3 + Pillow + numpy, fora do Unity).

Desenho por codigo, sem arte de terceiros e sem malha (nao passa pelos portoes do ADR-0002). A cena e a promessa do jogo:
uma crianca de costas, pequena, diante do Limiar aceso a noite. O portal de pedra com filete dourado, a luz quente saindo de
dentro em raios, os fios da Trama em turquesa girando em volta e o vale de Auren escuro embaixo (ART_BIBLE sec. 2, 3 e 6:
Auren quente, Limiar e Trama frios e dourados; violeta so em anomalia, entao nao aparece). Paleta: GDD cap. 09.
ponytail: icone de lancamento gerado por codigo. Se o idealizador quiser um pintado (a partir do concept do Aethron ou do
avatar, G2), troca so os 3 PNGs; o ProjectSetup.AplicarIcone e a loja seguem iguais.

Uso (da raiz do repositorio):
  python -I client/tools/icone_app.py               grava os PNGs do icone
  python -I client/tools/icone_app.py --autoteste   desenha numa pasta temporaria e confere tamanho, zona segura e contraste

Saidas em client/Assets/_COE/Icon/ (fora de Resources: so o ProjectSetup.AplicarIcone usa, nao entra no jogo):
  icone_fundo.png   432x432  camada de tras do adaptativo do Android: ceu, estrelas, vale e o clarao do portal
  icone_frente.png  432x432  camada da frente, transparente: portal, luz, Trama e a crianca, dentro da zona segura
  icone.png         512x512  as duas camadas juntas, sem transparencia: Windows e o icone da Play (loja_assets.py copia)
"""
import argparse, math, os, random, sys, tempfile
import numpy as np
from PIL import Image, ImageChops, ImageDraw, ImageFilter

CLIENT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
SAIDA = os.path.join(CLIENT, "Assets", "_COE", "Icon")
AZUL, AZUL_FUNDO, DOURADO, TURQUESA, MARFIM = (0x25, 0x38, 0x50), (0x0F, 0x18, 0x26), (0xD6, 0xB3, 0x6A), (0x86, 0xC8, 0xC9), (0xE9, 0xDE, 0xC6)
VERDE, TERRACOTA = (0x64, 0x8B, 0x67), (0xA8, 0x6D, 0x52)
CEU_TOPO, NOITE, PEDRA, PEDRA_LUZ, SOMBRA = (0x07, 0x0D, 0x1A), (0x14, 0x24, 0x3C), (0x2A, 0x31, 0x3D), (0x5A, 0x55, 0x52), (0x06, 0x09, 0x11)
LUZ_CENTRO = (0xFF, 0xF4, 0xD6)
SS = 4                  # supersample: desenha a 4x e reduz (borda lisa sem antialias do ImageDraw)
SEGURA = 0.66           # zona segura do adaptativo: o circulo de 66% do lado sempre aparece
# O portal fica no mesmo lugar nas duas camadas: centro da tela, um pouco abaixo (o vale e o horizonte ficam no fundo).
PORTAL_Y = 0.04         # deslocamento do centro do desenho, em fracao do lado


def _grade(w):
    y, x = np.mgrid[0:w, 0:w] / (w - 1) - 0.5
    return x, y


def _rgb(arr):
    return Image.fromarray(np.clip(arr, 0, 255).astype(np.uint8), "RGB")


def fundo(w):
    """Ceu noturno com estrelas, o clarao dourado do portal no meio e o vale de Auren escuro embaixo (camada de tras)."""
    x, y = _grade(w)
    t = np.clip(y + 0.5, 0, 1)[..., None]
    ceu = np.array(CEU_TOPO) * (1 - t) + np.array(NOITE) * t
    # clarao do Limiar atras do portal (o centro do desenho), largo e quente
    d2 = x * x + (y - PORTAL_Y) ** 2
    ceu = ceu + np.array((235, 190, 110)) * (np.exp(-d2 / 0.05) * 0.55)[..., None]
    ceu = ceu + np.array((120, 180, 190)) * (np.exp(-d2 / 0.20) * 0.10)[..., None]
    img = _rgb(ceu).convert("RGBA")
    W = w * SS
    big = Image.new("RGBA", (W, W), (0, 0, 0, 0))
    d = ImageDraw.Draw(big)
    rnd = random.Random(7)   # estrelas fixas: rodar de novo da o mesmo icone
    for _ in range(90):
        sx, sy = rnd.random(), rnd.random() * 0.55
        if (sx - 0.5) ** 2 + (sy - 0.5 - PORTAL_Y) ** 2 < 0.11:
            continue   # nada de estrela em cima do clarao
        r = W * (0.0015 + rnd.random() ** 4 * 0.004)
        a = int(70 + rnd.random() * 130)
        d.ellipse((sx * W - r, sy * W - r, sx * W + r, sy * W + r), fill=MARFIM + (a,))
    # duas camadas de morro: a de tras azulada, a da frente quase preta com o capim recortado
    def morro(base, amp, fase, cor, dentes=0):
        pts = [(0, W)]
        for i in range(0, 241):
            u = i / 240
            yy = base + amp * math.sin(u * 5.1 + fase) * 0.6 + amp * math.sin(u * 11.3 + fase * 2) * 0.25
            if dentes and i % 2:
                yy -= dentes * (0.5 + 0.5 * math.sin(i * 1.7))
            pts.append((u * W, (yy + 0.5) * W))
        pts.append((W, W))
        d.polygon(pts, fill=cor)
    morro(0.24, 0.035, 0.8, (0x16, 0x26, 0x34, 255))
    morro(0.33, 0.025, 2.1, (0x0A, 0x12, 0x1B, 255), dentes=0.006)
    img.alpha_composite(big.resize((w, w), Image.LANCZOS))
    # luz do portal derramada no chao da frente (poça dourada)
    poca = np.exp(-((x / 0.30) ** 2 + ((y - 0.36) / 0.06) ** 2))[..., None] * 0.45
    arr = np.asarray(img.convert("RGB")).astype(float)
    arr = arr * (1 - poca) + np.array(DOURADO) * poca
    # vinheta
    v = np.clip(1 - (np.hypot(x, y) / 0.72) ** 3 * 0.55, 0.35, 1)[..., None]
    return _rgb(arr * v).convert("RGBA")


def _bezier(p0, p1, p2, p3, n=64):
    out = []
    for i in range(n + 1):
        t = i / n
        a, b, c, e = (1 - t) ** 3, 3 * (1 - t) ** 2 * t, 3 * (1 - t) * t * t, t ** 3
        out.append((a * p0[0] + b * p1[0] + c * p2[0] + e * p3[0], a * p0[1] + b * p1[1] + c * p2[1] + e * p3[1]))
    return out


def _brilho(camada, raio, forca=1.0):
    """Halo da propria camada (o alfa borrado, na cor de cada pixel), por baixo dela."""
    halo = camada.filter(ImageFilter.GaussianBlur(raio))
    if forca != 1.0:
        halo.putalpha(halo.getchannel("A").point(lambda v: min(255, int(v * forca))))
    halo.alpha_composite(camada)
    return halo


def frente(w, segura=SEGURA):
    """Portal, luz, Trama e a crianca, dentro do circulo de `segura` * w (camada da frente, transparente)."""
    W = w * SS
    R = W * segura / 2                       # raio da zona segura, em px da tela grande
    cx, cy = W / 2, W / 2 + PORTAL_Y * W

    def P(u, v):                             # espaco do desenho: o circulo seguro e o disco unitario
        return (cx + u * R, cy + v * R)

    def caixa(u0, v0, u1, v1):
        a, b = P(u0, v0), P(u1, v1)
        return (a[0], a[1], b[0], b[1])

    # dimensoes do portal no espaco unitario
    meia, topo_arco, base = 0.40, -0.80, 0.56     # semi-largura do vao, topo do arco, soleira
    esp = 0.10                                     # espessura da moldura de pedra
    ca = topo_arco + meia                          # centro do semicirculo do arco

    def vao(dr, desenho, cor):
        """O vao do portal (arco + retangulo) aumentado de dr."""
        r = meia + dr
        desenho.pieslice(caixa(-r, ca - r, r, ca + r), 180, 360, fill=cor)
        desenho.rectangle(caixa(-r, ca, r, base), fill=cor)

    # 1. luz de dentro: branca no meio, dourada na borda, com raios
    luz = Image.new("RGBA", (W, W), (0, 0, 0, 0))
    mascara = Image.new("L", (W, W), 0)
    vao(0, ImageDraw.Draw(mascara), 255)
    yy, xx = np.mgrid[0:W, 0:W].astype(np.float32)
    lx, ly = P(0, 0.05)
    dist = np.hypot((xx - lx) / (R * 0.55), (yy - ly) / (R * 0.85))
    t = np.clip(dist, 0, 1)[..., None]
    cor = np.array(LUZ_CENTRO) * (1 - t) + np.array((0xE8, 0xA8, 0x4E)) * t
    ang = np.arctan2(yy - ly, xx - lx)
    raios = (0.5 + 0.5 * np.cos(ang * 9)) ** 4 * np.clip(dist, 0, 1.4) * 0.16
    cor = cor + np.array((255, 236, 190)) * raios[..., None]
    luz = Image.fromarray(np.clip(cor, 0, 255).astype(np.uint8), "RGB").convert("RGBA")
    luz.putalpha(mascara)
    del xx, yy, dist, t, cor, ang, raios

    # 2. moldura de pedra (bloco escuro com filete dourado por dentro e luz na aresta)
    moldura = Image.new("RGBA", (W, W), (0, 0, 0, 0))
    dm = ImageDraw.Draw(moldura)
    vao(esp, dm, PEDRA + (255,))
    # pedras: juntas da moldura (aduelas do arco e blocos dos pilares)
    for k in range(1, 9):
        a = math.radians(180 + k * 20)
        p0 = P(meia * math.cos(a), ca + meia * math.sin(a))
        p1 = P((meia + esp) * math.cos(a), ca + (meia + esp) * math.sin(a))
        dm.line((p0, p1), fill=SOMBRA + (255,), width=int(W * 0.004))
    for v in (ca + 0.16, ca + 0.38, ca + 0.60, ca + 0.82):
        if v < base:
            for s in (-1, 1):
                dm.line((P(s * meia, v), P(s * (meia + esp), v)), fill=SOMBRA + (255,), width=int(W * 0.004))
    dm.arc(caixa(-(meia + esp), ca - meia - esp, meia + esp, ca + meia + esp), 200, 340, fill=PEDRA_LUZ + (255,), width=int(W * 0.006))
    vao(0.018, dm, DOURADO + (255,))           # filete dourado em volta do vao
    moldura.alpha_composite(luz)
    # soleira: degrau largo de pedra com luz na borda de cima
    dm = ImageDraw.Draw(moldura)
    dm.rounded_rectangle(caixa(-0.54, base - 0.005, 0.54, base + 0.07), radius=W * 0.006, fill=PEDRA + (255,))
    dm.rounded_rectangle(caixa(-0.50, base + 0.055, 0.50, base + 0.10), radius=W * 0.006, fill=(0x1E, 0x23, 0x2C, 255))
    dm.line((P(-0.52, base), P(0.52, base)), fill=DOURADO + (255,), width=int(W * 0.005))
    # chave do arco: pedra de cima com o simbolo da Trama gravado em dourado
    kx, ky = P(0, topo_arco - esp * 0.5)
    kr = R * 0.085
    dm.polygon([(kx - kr, ky - kr * 1.15), (kx + kr, ky - kr * 1.15), (kx + kr * 0.7, ky + kr * 0.9), (kx - kr * 0.7, ky + kr * 0.9)],
               fill=(0x3A, 0x40, 0x4C, 255), outline=DOURADO + (255,), width=int(W * 0.004))
    dm.arc((kx - kr * 0.45, ky - kr * 0.55, kx + kr * 0.45, ky + kr * 0.35), 120, 60, fill=TURQUESA + (255,), width=int(W * 0.005))

    # 3. a crianca de costas, no vao, contra a luz (silhueta escura com a borda acesa)
    crianca = Image.new("RGBA", (W, W), (0, 0, 0, 0))
    dc = ImageDraw.Draw(crianca)
    px, chao = 0.02, base
    alt = 0.50                                     # do pe ao topo da cabeca: pequena diante do portal
    def C(u, v):                                   # espaco da crianca: v=0 no chao, v=1 no topo da cabeca
        return P(px + u * alt, chao - v * alt)
    sil = SOMBRA + (255,)
    cab_r = 0.12
    hx, hy = C(0, 1 - cab_r)
    rr = cab_r * alt * R
    dc.ellipse((hx - rr, hy - rr, hx + rr, hy + rr), fill=sil)
    # cabelo baguncado: bolinhas em volta do alto da cabeca (nada de ponta, que lia como orelha)
    for ang, dr, k in ((200, 0.95, 0.42), (235, 1.0, 0.45), (270, 1.02, 0.48), (305, 1.0, 0.45), (340, 0.95, 0.42), (15, 0.85, 0.35)):
        bx, by = hx + rr * dr * math.cos(math.radians(ang)), hy + rr * dr * math.sin(math.radians(ang))
        dc.ellipse((bx - rr * k, by - rr * k, bx + rr * k, by + rr * k), fill=sil)
    # pescoco, ombros arredondados e tunica que abre um pouco embaixo
    dc.polygon([C(-0.045, 0.80), C(0.045, 0.80), C(0.06, 0.74)] + [C(0.06 + 0.10 * math.sin(math.radians(t)), 0.74 - 0.10 * (1 - math.cos(math.radians(t)))) for t in range(0, 91, 15)]
               + [C(0.19, 0.34), C(0.21, 0.28), C(-0.21, 0.28), C(-0.19, 0.34)]
               + [C(-0.06 - 0.10 * math.sin(math.radians(t)), 0.74 - 0.10 * (1 - math.cos(math.radians(t)))) for t in range(90, -1, -15)]
               + [C(-0.06, 0.74)], fill=sil)
    # cachecol: a ponta voa para tras (para a esquerda), contra a luz
    dc.polygon([C(-0.06, 0.76), C(-0.20, 0.78), C(-0.33, 0.70), C(-0.24, 0.69), C(-0.12, 0.71)], fill=sil)
    # pernas finas e botas
    for sx in (-1, 1):
        dc.polygon([C(sx * 0.035, 0.29), C(sx * 0.11, 0.29), C(sx * 0.10, 0.03), C(sx * 0.05, 0.03)], fill=sil)
        dc.ellipse(caixa(px + (sx * 0.08 - 0.05) * alt, chao - 0.05 * alt, px + (sx * 0.08 + 0.05) * alt, chao + 0.005 * alt), fill=sil)
    # braco esquerdo caido; braco direito estendido para a luz, mao aberta
    lb = int(alt * R * 0.07)
    dc.line([C(-0.15, 0.66), C(-0.21, 0.50), C(-0.22, 0.38)], fill=sil, width=lb, joint="curve")
    dc.ellipse(caixa(px - 0.255 * alt, chao - 0.40 * alt, px - 0.185 * alt, chao - 0.33 * alt), fill=sil)
    dc.line([C(0.14, 0.66), C(0.27, 0.70), C(0.38, 0.80)], fill=sil, width=lb, joint="curve")
    mx, my = C(0.40, 0.82)
    mr = alt * R * 0.045
    dc.ellipse((mx - mr, my - mr, mx + mr, my + mr), fill=sil)
    # borda acesa: a silhueta inflada em dourado claro, por baixo da silhueta
    a = crianca.getchannel("A")
    borda = Image.new("RGBA", (W, W), (0xFF, 0xE2, 0xA0, 0))
    borda.putalpha(a.filter(ImageFilter.MaxFilter(7)).filter(ImageFilter.GaussianBlur(2)))
    borda.alpha_composite(crianca)
    crianca = borda

    # 4. Trama: fios turquesa que saem do portal e giram em volta, com nos acesos
    trama = Image.new("RGBA", (W, W), (0, 0, 0, 0))
    dt = ImageDraw.Draw(trama)
    fios = [
        ((0.18, -0.20), (0.72, -0.42), (0.86, 0.10), (0.60, 0.42)),
        ((-0.20, -0.42), (-0.70, -0.62), (-0.88, -0.05), (-0.62, 0.30)),
        ((0.05, -0.62), (0.30, -0.90), (0.62, -0.78), (0.66, -0.52)),
    ]
    nos = []
    for i, f in enumerate(fios):
        pts = [P(u, v) for u, v in _bezier(*f)]
        dt.line(pts, fill=TURQUESA + (235,), width=int(W * (0.0045 - i * 0.0008)), joint="curve")
        nos += [pts[-1], pts[len(pts) // 2]]
    for k, (nx, ny) in enumerate(nos):
        r = W * (0.008 if k % 2 == 0 else 0.005)
        dt.ellipse((nx - r, ny - r, nx + r, ny + r), fill=MARFIM + (255,))
    trama = _brilho(trama, W * 0.008, 1.6)

    img = Image.new("RGBA", (W, W), (0, 0, 0, 0))
    img.alpha_composite(_brilho(moldura, W * 0.02, 0.9))
    img.alpha_composite(trama)
    img.alpha_composite(crianca)
    return img.resize((w, w), Image.LANCZOS)


def gerar(saida):
    os.makedirs(saida, exist_ok=True)
    fundo(432).convert("RGB").save(os.path.join(saida, "icone_fundo.png"))
    frente(432).save(os.path.join(saida, "icone_frente.png"))
    junto = fundo(512)
    junto.alpha_composite(frente(512, 0.86))   # a Play arredonda so os cantos: o desenho pode crescer
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
        # nada solido da frente fora do circulo seguro (o Android corta)
        a = np.asarray(Image.open(frente_p).getchannel("A")).astype(float)
        y, x = np.mgrid[0:432, 0:432] - 215.5
        fora = a[np.hypot(x, y) > 432 * SEGURA / 2].max()
        if fora > 64:   # o halo pode passar um pouco (o corte so apaga brilho); o desenho, nao
            erros.append("frente vaza da zona segura (alfa %d fora do circulo de %d%%)" % (fora, SEGURA * 100))
        if a.max() < 250:
            erros.append("frente vazia")
        ic = np.asarray(Image.open(icone_p).convert("L")).astype(float)
        if ic[ic > 200].size < 2000:
            erros.append("pouca luz no portal (%d px claros)" % ic[ic > 200].size)
        if ic[ic < 30].size < 2000:
            erros.append("pouca sombra (silhueta e noite) (%d px escuros)" % ic[ic < 30].size)
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
