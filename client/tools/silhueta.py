"""Teste cego de silhueta do portao G2 (docs/adr/ADR-0002-riscos-de-originalidade.md).

Uso (da raiz do repo):
    python client/tools/silhueta.py --saida <pasta> [--seed N] [--px-por-metro 120] rotulo=caminho[@altura_m] ...
    python client/tools/silhueta.py --teste

Escreve <pasta>/folha_teste.png (silhuetas pretas lado a lado no mesmo chao, embaralhadas pela seed,
so um numero embaixo de cada) e <pasta>/gabarito.txt (seed + numero -> rotulo). A folha vai para
quem faz o teste; o gabarito fica com quem aplica. O terminal nunca mostra o gabarito.
Escala: altura na folha = altura_m x px_por_metro (120 px/m = 30% de 400 px/m); sem @altura_m, 1,75 m.
"""
import argparse
import os
import random
import statistics
import sys
import tempfile

from PIL import Image, ImageChops, ImageDraw, ImageFilter, ImageFont

# ponytail: mascara calculada a 512 px de altura; a 30% o adulto sai com ~210 px e sobra resolucao.
# Folha em escala bem maior (--px-por-metro > ~250) sai serrilhada: subir TRAB e recalibrar GRAD/BOLA.
TRAB = 512
# Calibrado nos concepts #5, #48, #52, #68, #69 e COE-NPC-015 (fundo creme de estudio), medidas a 512 px:
TOL = 40   # dif. max. por canal vs mediana da borda que ainda e fundo. O degrade do estudio chega a
           # ~28 (#48, chao claro). Cor sozinha nao separa: o macacao marfim fica a ~16 do fundo e
           # vazou com qualquer TOL que ainda cobrisse o degrade.
GRAD = 6   # variacao max. (max-min 3x3) que ainda e fundo liso. Fundo: <= 5; contorno mais fraco
           # medido (ombro iluminado do #68): so vaza com GRAD >= 8. O contorno e a barreira de verdade.
BOLA = 5   # o preenchimento so anda onde cabe um bloco BOLA x BOLA: nao vaza por fresta de 1-4 px
# Sombra suave nos pes: mesmo matiz (+-4) e saturacao (+-12) do fundo, de 10 a 70 mais escura. Nela a
# barreira so vale acima de GRAD_SOMBRA; sem isso o rastro da sombra ficava grudado no pe (#68). Os 10 minimos
# evitam o ombro do NPC-015: quase cor de fundo, contorno fraco (grad 7-8), vazava para o peito.
SOMBRA_MATIZ, SOMBRA_SAT, SOMBRA_MIN, SOMBRA_MAX = 4, 12, 10, 70
GRAD_SOMBRA = 14
# Buraco cercado pela figura (entre as pernas, entre braco e tronco): sai se cabe um bloco FURO_BOLA x
# FURO_BOLA perto da cor do fundo (dif <= FURO_TOL) e sem textura (grad <= FURO_GRAD). O tecido tem trama
# (grad > 3): com grad <= 6 o peito do #52, #69 e NPC-015 era furado; com FURO_TOL 40, o do #52.
# FURO_TOL 20 ainda limpa o vao das canelas do #48, onde o chao e ~20 mais claro que a mediana.
FURO_TOL, FURO_GRAD, FURO_BOLA = 20, 3, 9
LIMPA = 7  # abertura final: some com fio de sombra (< 7 px a 512, < 3 px na folha) e cisco
ALTURA_PADRAO = 1.75


def _max_canal(img):
    r, g, b = img.split()
    return ImageChops.lighter(ImageChops.lighter(r, g), b)


def _faixa(img, a, b):
    return img.point(lambda v: 255 if a <= v <= b else 0)


def _e(*mascaras):
    m = mascaras[0]
    for o in mascaras[1:]:
        m = ImageChops.darker(m, o)
    return m


def _abrir(m, k):  # tira o que nao comporta um bloco k x k
    return m.filter(ImageFilter.MinFilter(k)).filter(ImageFilter.MaxFilter(k))


def mascara(img):
    """Mascara L (255 = figura) na escala de trabalho. Fundo = o que o preenchimento a partir da borda alcanca."""
    img = img.convert("RGB")
    img.thumbnail((10 * TRAB, TRAB))
    w, h = img.size
    px = img.load()
    borda = [px[x, y] for x in range(w) for y in (0, h - 1)] + [px[x, y] for y in range(h) for x in (0, w - 1)]
    ref = tuple(int(statistics.median(p[c] for p in borda)) for c in range(3))
    dif = _max_canal(ImageChops.difference(img, Image.new("RGB", img.size, ref)))
    grad = _max_canal(ImageChops.subtract(img.filter(ImageFilter.MaxFilter(3)), img.filter(ImageFilter.MinFilter(3))))
    rh, rs, rv = Image.new("RGB", (1, 1), ref).convert("HSV").getpixel((0, 0))
    hh, ss, vv = img.convert("HSV").split()
    sombra = _e(_faixa(hh, rh - SOMBRA_MATIZ, rh + SOMBRA_MATIZ), _faixa(ss, rs - SOMBRA_SAT, rs + SOMBRA_SAT),
                _faixa(vv, rv - SOMBRA_MAX, rv - SOMBRA_MIN))
    liso = ImageChops.lighter(_e(_faixa(dif, 0, TOL), _faixa(grad, 0, GRAD)), _e(sombra, _faixa(grad, 0, GRAD_SOMBRA)))
    pad = Image.new("L", (w + 2, h + 2), 255)  # moldura de fundo: uma semente so cobre a borda inteira
    pad.paste(liso.filter(ImageFilter.MinFilter(BOLA)), (1, 1))
    ImageDraw.floodfill(pad, (0, 0), 128)
    fundo = pad.crop((1, 1, w + 1, h + 1)).point(lambda v: 255 if v == 128 else 0)
    fundo = ImageChops.darker(fundo.filter(ImageFilter.MaxFilter(BOLA)), liso)  # devolve a beira comida pela bola
    fig = ImageChops.invert(fundo)
    # ponytail: cor de fundo = mediana unica da borda. Teto: buraco cercado pela figura so sai se for
    # largo, liso e perto dessa cor (vao estreito ou sombreado entre braco e tronco fica preto), e a
    # sombra de contato dura entre os pes (borda com grad > 14) fica como ponte preta ligando os pes.
    # Upgrade: modelo de fundo local (cor interpolada do fundo ao redor) no lugar da mediana unica.
    fig = ImageChops.subtract(fig, _abrir(_e(_faixa(dif, 0, FURO_TOL), _faixa(grad, 0, FURO_GRAD), fig), FURO_BOLA))
    fig = _abrir(fig, LIMPA)
    if not fig.getbbox():
        return fig
    # so o pedaco ligado ao corpo: cisco solto de sombra nao entra na caixa nem muda a escala
    cob = fig.resize((w, 1), Image.Resampling.BOX)
    x0 = max(range(w), key=lambda x: cob.getpixel((x, 0)))  # coluna mais cheia = corpo
    y0 = next(y for y in range(h) if fig.getpixel((x0, y)))
    ImageDraw.floodfill(fig, (x0, y0), 128)
    return fig.point(lambda v: 255 if v == 128 else 0)


def silhueta(caminho, altura_m, px_por_metro):
    """Alfa L recortado na figura, com altura exata de altura_m x px_por_metro."""
    fig = mascara(Image.open(caminho))
    caixa = fig.getbbox()
    if not caixa:
        raise SystemExit(f"erro: nenhuma figura encontrada em {caminho}")
    fig = fig.crop(caixa)
    alt = round(altura_m * px_por_metro)
    return fig.resize((max(1, round(fig.width * alt / fig.height)), alt), Image.Resampling.BOX)


def montar(itens, saida, seed, px_por_metro):
    """itens: [(rotulo, caminho, altura_m)]. Escreve folha_teste.png e gabarito.txt; devolve os caminhos."""
    itens = list(itens)
    random.Random(seed).shuffle(itens)
    sils = [silhueta(c, a, px_por_metro) for _, c, a in itens]
    margem, vao = 40, max(30, round(0.3 * px_por_metro))
    fonte = ImageFont.load_default(size=28)
    chao = margem + max(s.height for s in sils)
    folha = Image.new("L", (2 * margem + sum(s.width for s in sils) + vao * (len(sils) - 1), chao + 60), 255)
    d = ImageDraw.Draw(folha)
    d.line((margem // 2, chao, folha.width - margem // 2, chao), fill=200)
    x = margem
    for n, s in enumerate(sils, 1):
        folha.paste(0, (x, chao - s.height), s)
        d.text((x + s.width / 2, chao + 12), str(n), fill=0, font=fonte, anchor="mt")
        x += s.width + vao
    os.makedirs(saida, exist_ok=True)
    folha_p, gab_p = os.path.join(saida, "folha_teste.png"), os.path.join(saida, "gabarito.txt")
    folha.save(folha_p)
    with open(gab_p, "w", encoding="utf-8") as f:
        f.write(f"seed {seed}\n")
        for n, (r, c, a) in enumerate(itens, 1):
            f.write(f"{n}\t{r}\t{a:.2f} m\t{c}\n")
    return folha_p, gab_p


def _item(texto):
    rotulo, igual, resto = texto.partition("=")
    if not igual or not rotulo or not resto:
        raise argparse.ArgumentTypeError(f"esperado rotulo=caminho[@altura_m], veio {texto!r}")
    caminho, arroba, alt = resto.rpartition("@")
    if not arroba:
        return rotulo, resto, ALTURA_PADRAO
    try:
        altura = float(alt.replace(",", "."))
    except ValueError:
        raise argparse.ArgumentTypeError(f"altura invalida em {texto!r}")
    if altura <= 0:
        raise argparse.ArgumentTypeError(f"altura precisa ser > 0 em {texto!r}")
    return rotulo, caminho, altura


def _teste():
    """Autoverificacao com imagem sintetica: macacao claro cercado de contorno escuro + sombra suave."""
    w, h = 300, 400
    img = Image.new("RGB", (w, h))
    d = ImageDraw.Draw(img)
    for y in range(h):  # degrade de estudio: 10 niveis de cima para baixo
        v = y * 10 // h
        d.line((0, y, w, y), fill=(240 - v, 226 - v, 204 - v))
    sombra = Image.new("L", (w, h), 0)
    ImageDraw.Draw(sombra).ellipse((90, 330, 250, 356), fill=255)
    img.paste((212, 198, 176), (0, 0), sombra.filter(ImageFilter.GaussianBlur(6)))  # ~25 mais escura, borda macia
    d.ellipse((125, 40, 175, 95), fill=(60, 40, 30))                                 # cabeca escura
    marfim = (252, 238, 216)  # ~14 acima do fundo, dentro da TOL: so a barreira de gradiente segura
    d.rectangle((110, 95, 190, 340), fill=marfim, outline=(90, 80, 70), width=2)
    d.line((130, 95, 170, 95), fill=marfim, width=4)  # "ombro" sem contorno escuro, como no concept #68
    for x in range(113, 188, 3):  # trama do tecido: e o que separa o marfim de um vao liso cor de fundo
        d.line((x, 98, x, 337), fill=(244, 230, 208))
    d.rectangle((140, 250, 160, 325), fill=(233, 219, 197), outline=(90, 80, 70), width=2)  # vao entre as pernas
    d.ellipse((20, 300, 34, 314), fill=(60, 40, 30))  # cisco escuro solto no fundo
    fig = mascara(img)
    assert fig.getpixel((150, 200)) == 255, "regiao marfim interna deveria ser figura"
    assert fig.getpixel((150, 60)) == 255, "cabeca deveria ser figura"
    assert fig.getpixel((5, 5)) == 0 and fig.getpixel((60, 200)) == 0, "fundo deveria ficar de fora"
    assert fig.getpixel((225, 346)) == 0 and fig.getpixel((100, 345)) == 0, "sombra deveria ficar de fora"
    assert fig.getpixel((150, 290)) == 0, "vao cor de fundo cercado pela figura deveria ficar de fora"
    assert fig.getpixel((27, 307)) == 0, "cisco solto nao deveria entrar na figura"
    assert fig.getbbox()[3] in (341, 342), f"pe deveria acabar em y=341, caixa {fig.getbbox()}"
    with tempfile.TemporaryDirectory() as tmp:
        p = os.path.join(tmp, "sint.png")
        img.save(p)
        for alt_m, ppm in ((1.10, 120), (1.75, 120), (1.75, 400)):
            s = silhueta(p, alt_m, ppm)
            medida = s.getbbox()[3] - s.getbbox()[1]
            assert abs(medida - alt_m * ppm) <= 1, f"altura {medida} px, esperado {alt_m * ppm:.0f}"
        folha_p, gab_p = montar([("a", p, 1.10), ("b", p, 1.75), ("c", p, 1.3), ("d", p, 1.5)], tmp, 7, 120)
        assert Image.open(folha_p).getbbox() and open(gab_p, encoding="utf-8").readline() == "seed 7\n"
    print("ok: silhueta.py --teste passou")


def main():
    ap = argparse.ArgumentParser(description="Folha do teste cego de silhueta (G2, ADR-0002).")
    ap.add_argument("itens", nargs="*", type=_item, metavar="rotulo=caminho[@altura_m]")
    ap.add_argument("--saida", help="pasta de saida (folha_teste.png + gabarito.txt)")
    ap.add_argument("--seed", type=int, help="seed do embaralhamento (padrao: aleatoria, impressa no terminal)")
    ap.add_argument("--px-por-metro", type=float, default=120.0, help="padrao 120 = 30%% de 400 px/m")
    ap.add_argument("--teste", action="store_true", help="autoverificacao sem arquivos externos")
    a = ap.parse_args()
    if a.teste:
        _teste()
        return
    if not a.saida or not a.itens:
        ap.error("informe --saida e pelo menos um rotulo=caminho[@altura_m]")
    if len({r for r, _, _ in a.itens}) != len(a.itens):
        ap.error("rotulos repetidos deixariam o gabarito ambiguo")
    if len(a.itens) < 4:
        print("aviso: o ADR-0002 pede o alvo + pelo menos 3 do mesmo elenco (>= 4 silhuetas)", file=sys.stderr)
    seed = a.seed if a.seed is not None else random.randrange(1_000_000)
    folha_p, gab_p = montar(a.itens, a.saida, seed, a.px_por_metro)
    print(f"folha:    {folha_p}\ngabarito: {gab_p}\nseed:     {seed}")


if __name__ == "__main__":
    main()
