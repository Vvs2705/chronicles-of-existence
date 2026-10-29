"""logo_blender.py - cena do logo e do icone do Chronicles of Existence no Blender 5.2 (Cycles), a partir do emblema do Tripo.

O emblema (anel, Arvore da Vida, cristal e losango) e o GLB gerado no Tripo a partir da referencia aprovada pelo idealizador
(docs/arte/PROVENIENCIA.md sec. 4b). O resto e montado aqui: luz de estudio (rim azul de um lado, quente dourado do outro),
letreiro em 3D com a Cinzel Bold (o X aceso em azul), ceu noturno e a paisagem da key art. O brilho (bloom) entra depois,
no icone_app.py, para valer tambem nos PNGs transparentes.

Uso (da raiz do repositorio):
  blender -b -P client/tools/logo_blender.py -- <tomada> <saida.png> [--amostras N] [--escala 0.25] [--blend arq.blend]
Tomadas: frente (2048, emblema, transparente), fundo (2048, ceu sem objeto), play (2048, emblema grande sobre o ceu),
         logo (4096x2048, emblema + letreiro, transparente), keyart (3840x2160, logo sobre a paisagem).
"""

import math, os, random, sys
import bpy, bmesh
from mathutils import Vector, noise

RAIZ = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
GLB = os.path.join(RAIZ, "docs", "arte", "loja", "fonte", "emblema_tripo.glb")
FONTE = os.path.join(
    RAIZ, "client", "Assets", "_COE", "Resources", "Fontes", "Cinzel-Bold.ttf"
)
TOMADAS = {
    "frente": (2048, 2048),
    "fundo": (2048, 2048),
    "play": (2048, 2048),
    "logo": (4096, 2048),
    "keyart": (3840, 2160),
}


def hexrgb(h, k=1.0):
    h = h.lstrip("#")
    srgb = [int(h[i : i + 2], 16) / 255 for i in (0, 2, 4)]
    lin = [c / 12.92 if c <= 0.04045 else ((c + 0.055) / 1.055) ** 2.4 for c in srgb]
    return (lin[0] * k, lin[1] * k, lin[2] * k, 1.0)


KEY_DIST = 15.0 * 50 / 36  # camera da key art: ve 15 m de largura no plano do logo (lente 50)
GANHO = 0.3  # calibragem geral das luzes de estudio (a 1.0 o ouro estourava)
DOURADO, AZUL, NOITE, VERDE, LARANJA = (
    "#D4AF37",
    "#00A8FF",
    "#0B1F3B",
    "#3FBF7F",
    "#FF8A3D",
)


# ---------------------------------------------------------------- materiais


def no(nt, tipo, **inp):
    n = nt.nodes.new(tipo)
    for k, v in inp.items():
        n.inputs[k].default_value = v
    return n


def bsdf_de(mat):
    return next(n for n in mat.node_tree.nodes if n.type == "BSDF_PRINCIPLED")


def mat_ouro(nome="Ouro"):
    """Ouro polido com gradiente vertical (claro em cima, queimado embaixo) e um granulado fino no relevo."""
    m = bpy.data.materials.new(nome)
    nt = m.node_tree
    b = bsdf_de(m)
    b.inputs["Metallic"].default_value = 1.0
    b.inputs["Roughness"].default_value = 0.2
    coord = nt.nodes.new("ShaderNodeTexCoord")
    sep = nt.nodes.new("ShaderNodeSeparateXYZ")
    nt.links.new(coord.outputs["Generated"], sep.inputs[0])
    ramp = nt.nodes.new("ShaderNodeValToRGB")
    ramp.color_ramp.elements[0].color = hexrgb("#8A5414")
    ramp.color_ramp.elements[1].color = hexrgb("#FFE6A0")
    mid = ramp.color_ramp.elements.new(0.55)
    mid.color = hexrgb(DOURADO)
    nt.links.new(sep.outputs["Z"], ramp.inputs[0])
    nt.links.new(ramp.outputs[0], b.inputs["Base Color"])
    ruido = no(nt, "ShaderNodeTexNoise", Scale=180.0, Detail=4.0)
    bump = no(nt, "ShaderNodeBump", Strength=0.04)
    nt.links.new(ruido.outputs["Fac"], bump.inputs["Height"])
    nt.links.new(bump.outputs["Normal"], b.inputs["Normal"])
    return m


def mat_brilho(nome, cor, forca):
    m = bpy.data.materials.new(nome)
    b = bsdf_de(m)
    b.inputs["Base Color"].default_value = cor
    b.inputs["Emission Color"].default_value = cor
    b.inputs["Emission Strength"].default_value = forca
    b.inputs["Roughness"].default_value = 0.1
    return m


def realcar_tripo(mat):
    """Faz brilhar o que e azul puro na textura do Tripo (cristal, losango, raiz azul) e aquece um pouco a copa laranja."""
    nt = mat.node_tree
    b = bsdf_de(mat)
    base = b.inputs["Base Color"].links[0].from_socket
    # tronco e galhos sairam cinza-prata na textura: o que tem pouca saturacao vira ouro (mesmo valor) e metal
    hsv = nt.nodes.new("ShaderNodeSeparateColor")
    hsv.mode = "HSV"
    nt.links.new(base, hsv.inputs[0])
    sat = nt.nodes.new("ShaderNodeMapRange")
    sat.inputs["From Min"].default_value, sat.inputs["From Max"].default_value = 0.32, 0.12
    nt.links.new(hsv.outputs[1], sat.inputs["Value"])
    ouro = nt.nodes.new("ShaderNodeCombineColor")
    ouro.mode = "HSV"
    ouro.inputs[0].default_value, ouro.inputs[1].default_value = 0.115, 0.72
    vmul = nt.nodes.new("ShaderNodeMath")
    vmul.operation = "MULTIPLY"
    vmul.inputs[1].default_value = 0.95
    nt.links.new(hsv.outputs[2], vmul.inputs[0])
    nt.links.new(vmul.outputs[0], ouro.inputs[2])
    mix = nt.nodes.new("ShaderNodeMix")
    mix.data_type = "RGBA"
    nt.links.new(sat.outputs[0], mix.inputs["Factor"])
    nt.links.new(base, mix.inputs["A"])
    nt.links.new(ouro.outputs[0], mix.inputs["B"])
    viva = nt.nodes.new("ShaderNodeHueSaturation")
    viva.inputs["Saturation"].default_value = 1.6
    nt.links.new(mix.outputs["Result"], viva.inputs["Color"])
    nt.links.new(viva.outputs["Color"], b.inputs["Base Color"])
    met = b.inputs["Metallic"].links[0].from_socket if b.inputs["Metallic"].links else None
    mmax = nt.nodes.new("ShaderNodeMath")
    mmax.operation = "MAXIMUM"
    nt.links.new(sat.outputs[0], mmax.inputs[0])
    if met:
        nt.links.new(met, mmax.inputs[1])
    nt.links.new(mmax.outputs[0], b.inputs["Metallic"])
    sep = nt.nodes.new("ShaderNodeSeparateColor")
    nt.links.new(base, sep.inputs[0])
    mx = nt.nodes.new("ShaderNodeMath")
    mx.operation = "MAXIMUM"
    nt.links.new(sep.outputs[0], mx.inputs[0])
    nt.links.new(sep.outputs[1], mx.inputs[1])
    azul = nt.nodes.new("ShaderNodeMath")
    azul.operation = "SUBTRACT"
    nt.links.new(sep.outputs[2], azul.inputs[0])
    nt.links.new(sep.outputs[0], azul.inputs[1])  # B - R: pega o cristal ciano claro (B - max(R,G) nao pegava)
    faixa_a = nt.nodes.new("ShaderNodeMapRange")
    (
        faixa_a.inputs["From Min"].default_value,
        faixa_a.inputs["From Max"].default_value,
    ) = 0.12, 0.45
    faixa_a.inputs["To Max"].default_value = 6.0
    nt.links.new(azul.outputs[0], faixa_a.inputs["Value"])
    quente = nt.nodes.new("ShaderNodeMath")
    quente.operation = "SUBTRACT"
    nt.links.new(sep.outputs[0], quente.inputs[0])
    nt.links.new(sep.outputs[2], quente.inputs[1])
    faixa_q = nt.nodes.new("ShaderNodeMapRange")
    (
        faixa_q.inputs["From Min"].default_value,
        faixa_q.inputs["From Max"].default_value,
    ) = 0.45, 0.85
    faixa_q.inputs["To Max"].default_value = 0.8
    nt.links.new(quente.outputs[0], faixa_q.inputs["Value"])
    soma = nt.nodes.new("ShaderNodeMath")
    soma.operation = "ADD"
    nt.links.new(faixa_a.outputs[0], soma.inputs[0])
    nt.links.new(faixa_q.outputs[0], soma.inputs[1])
    nt.links.new(base, b.inputs["Emission Color"])
    nt.links.new(soma.outputs[0], b.inputs["Emission Strength"])


def mat_ceu(nome, estrelas=1.0, horizonte=0.0, clarao=1.0, halo=0.22, centro=(0.5, 0.5), nucleo=("#7FD0FF", 0.1)):
    """Ceu noturno so de emissao: azul profundo, nevoa (nebulosa), estrelas e o clarao atras do emblema (centro)."""
    m = bpy.data.materials.new(nome)
    nt = m.node_tree
    for n in list(nt.nodes):
        if n.type != "OUTPUT_MATERIAL":
            nt.nodes.remove(n)
    out = next(n for n in nt.nodes if n.type == "OUTPUT_MATERIAL")
    tc = nt.nodes.new("ShaderNodeTexCoord")
    uv = tc.outputs["UV"]
    # distancia do centro (clarao) e altura (gradiente)
    sep = nt.nodes.new("ShaderNodeSeparateXYZ")
    nt.links.new(uv, sep.inputs[0])
    cen = no(nt, "ShaderNodeVectorMath")
    cen.operation = "DISTANCE"
    cen.inputs[1].default_value = (centro[0], centro[1], 0.0)
    nt.links.new(uv, cen.inputs[0])
    grad = nt.nodes.new("ShaderNodeValToRGB")
    grad.color_ramp.elements[0].color = hexrgb("#163A6E", 1.0)
    grad.color_ramp.elements[1].color = hexrgb("#03070F", 1.0)
    e = grad.color_ramp.elements.new(0.32)
    e.color = hexrgb(NOITE, 1.0)
    nt.links.new(cen.outputs["Value"], grad.inputs[0])
    # nebulosa: ruido grande em azul e um toque de violeta frio
    neb = no(nt, "ShaderNodeTexNoise", Scale=2.2, Detail=8.0, Roughness=0.62)
    nt.links.new(uv, neb.inputs["Vector"])
    nebr = nt.nodes.new("ShaderNodeValToRGB")
    nebr.color_ramp.elements[0].position = 0.45
    nebr.color_ramp.elements[0].color = (0, 0, 0, 1)
    nebr.color_ramp.elements[1].color = hexrgb("#1E6FD0", 0.55)
    nt.links.new(neb.outputs["Fac"], nebr.inputs[0])
    c1 = nt.nodes.new("ShaderNodeMix")
    c1.data_type = "RGBA"
    c1.blend_type = "ADD"
    c1.inputs["Factor"].default_value = 1.0
    nt.links.new(grad.outputs[0], c1.inputs["A"])
    nt.links.new(nebr.outputs[0], c1.inputs["B"])
    # estrelas: voronoi fino, so os pontos mais proximos do centro da celula
    vor = no(nt, "ShaderNodeTexVoronoi", Scale=260.0, Randomness=1.0)
    vor.feature = "DISTANCE_TO_EDGE" if False else "F1"
    nt.links.new(uv, vor.inputs["Vector"])
    st = nt.nodes.new("ShaderNodeMapRange")
    st.inputs["From Min"].default_value, st.inputs["From Max"].default_value = 0.06, 0.0
    st.inputs["To Max"].default_value = 8.0 * estrelas
    nt.links.new(vor.outputs["Distance"], st.inputs["Value"])
    var = no(nt, "ShaderNodeMath")
    var.operation = "MULTIPLY"
    nt.links.new(st.outputs[0], var.inputs[0])
    nt.links.new(vor.outputs["Color"], var.inputs[1])
    pot = no(nt, "ShaderNodeMath")
    pot.operation = "POWER"
    pot.inputs[1].default_value = 3.0
    nt.links.new(var.outputs[0], pot.inputs[0])
    c2 = nt.nodes.new("ShaderNodeMix")
    c2.data_type = "RGBA"
    c2.blend_type = "ADD"
    nt.links.new(pot.outputs[0], c2.inputs["Factor"])
    c2.inputs["B"].default_value = (1.0, 0.95, 0.85, 1)
    nt.links.new(c1.outputs["Result"], c2.inputs["A"])
    # clarao: azul largo e um nucleo dourado (o emblema fica na frente dele)
    hal = nt.nodes.new("ShaderNodeMapRange")
    hal.inputs["From Min"].default_value, hal.inputs["From Max"].default_value = (
        0.0,
        0.42,
    )
    hal.inputs["To Min"].default_value, hal.inputs["To Max"].default_value = 1.0, 0.0
    hal.interpolation_type = "SMOOTHERSTEP"
    nt.links.new(cen.outputs["Value"], hal.inputs["Value"])
    c3 = nt.nodes.new("ShaderNodeMix")
    c3.data_type = "RGBA"
    c3.blend_type = "ADD"
    nt.links.new(hal.outputs[0], c3.inputs["Factor"])
    c3.inputs["B"].default_value = hexrgb("#2E8CFF", halo * clarao)
    nt.links.new(c2.outputs["Result"], c3.inputs["A"])
    nuc = nt.nodes.new("ShaderNodeMapRange")
    nuc.inputs["From Min"].default_value, nuc.inputs["From Max"].default_value = (
        0.0,
        0.2,
    )
    nuc.inputs["To Min"].default_value, nuc.inputs["To Max"].default_value = 1.0, 0.0
    nuc.interpolation_type = "SMOOTHERSTEP"
    nt.links.new(cen.outputs["Value"], nuc.inputs["Value"])
    c4 = nt.nodes.new("ShaderNodeMix")
    c4.data_type = "RGBA"
    c4.blend_type = "ADD"
    nt.links.new(nuc.outputs[0], c4.inputs["Factor"])
    c4.inputs["B"].default_value = hexrgb(nucleo[0], nucleo[1] * clarao)
    nt.links.new(c3.outputs["Result"], c4.inputs["A"])
    ultimo = c4
    if horizonte:
        # por do sol dourado na linha do horizonte (key art)
        hz = nt.nodes.new("ShaderNodeMapRange")
        hz.inputs["From Min"].default_value, hz.inputs["From Max"].default_value = (
            0.0,
            0.45,
        )
        hz.inputs["To Min"].default_value, hz.inputs["To Max"].default_value = (
            horizonte,
            0.0,
        )
        hz.interpolation_type = "SMOOTHERSTEP"
        nt.links.new(sep.outputs["Y"], hz.inputs["Value"])
        c5 = nt.nodes.new("ShaderNodeMix")
        c5.data_type = "RGBA"
        c5.blend_type = "ADD"
        nt.links.new(hz.outputs[0], c5.inputs["Factor"])
        c5.inputs["B"].default_value = hexrgb(LARANJA, 1.0)
        nt.links.new(c4.outputs["Result"], c5.inputs["A"])
        ultimo = c5
    em = no(nt, "ShaderNodeEmission", Strength=1.0)
    nt.links.new(ultimo.outputs["Result"], em.inputs["Color"])
    nt.links.new(em.outputs[0], out.inputs["Surface"])
    return m


def mat_silhueta(nome, cor, nevoa):
    """Morro pintado: cor chapada que clareia para o azul da nevoa embaixo (perspectiva aerea falsa), sem luz."""
    m = bpy.data.materials.new(nome)
    nt = m.node_tree
    for n in list(nt.nodes):
        if n.type != "OUTPUT_MATERIAL":
            nt.nodes.remove(n)
    out = next(n for n in nt.nodes if n.type == "OUTPUT_MATERIAL")
    tc = nt.nodes.new("ShaderNodeTexCoord")
    sep = nt.nodes.new("ShaderNodeSeparateXYZ")
    nt.links.new(tc.outputs["Generated"], sep.inputs[0])
    r = nt.nodes.new("ShaderNodeValToRGB")
    r.color_ramp.elements[0].color = nevoa
    r.color_ramp.elements[1].color = cor
    r.color_ramp.elements[0].position = 0.55
    nt.links.new(sep.outputs["Z"], r.inputs[0])
    em = no(nt, "ShaderNodeEmission", Strength=1.0)
    nt.links.new(r.outputs[0], em.inputs["Color"])
    nt.links.new(em.outputs[0], out.inputs["Surface"])
    return m


# ---------------------------------------------------------------- objetos


def mat_cristal(nome="Cristal"):
    """Cristal azul aceso: emissao que vai do azul fundo nas bordas ao ciano claro nas facetas de frente."""
    m = bpy.data.materials.new(nome)
    nt = m.node_tree
    b = bsdf_de(m)
    b.inputs["Base Color"].default_value = hexrgb("#021638")
    b.inputs["Roughness"].default_value = 0.08
    b.inputs["Specular IOR Level"].default_value = 0.35
    lw = no(nt, "ShaderNodeLayerWeight", Blend=0.35)
    r = nt.nodes.new("ShaderNodeValToRGB")
    r.color_ramp.elements[0].color = hexrgb(AZUL)
    r.color_ramp.elements[1].color = hexrgb("#0030B0")
    nt.links.new(lw.outputs["Facing"], r.inputs[0])
    nt.links.new(r.outputs[0], b.inputs["Emission Color"])
    b.inputs["Emission Strength"].default_value = 1.4
    return m


def anel(nome, cx, zc, r_in, r_out, prof, a0, a1, ponta0, ponta1, mat, n=180):
    """Arco de ouro varrido de a0 a a1 (graus), perfil chanfrado com crista na frente (-Y).
    ponta0/ponta1: a ponta afina ate virar lamina (True) ou sai em corte reto inclinado (False)."""
    perfil = ((0.0, 0.25), (0.12, -0.35), (0.5, -0.5), (0.88, -0.35), (1.0, 0.25), (0.85, 0.5), (0.15, 0.5))
    me = bpy.data.meshes.new(nome)
    bm = bmesh.new()
    aneis = []
    for i in range(n + 1):
        t = i / n
        a = math.radians(a0 + (a1 - a0) * t)
        # afinamento nas pontas: os 12% finais encolhem a largura e a espessura
        f = 1.0
        if ponta0 and t < 0.12:
            f = (t / 0.12) ** 0.7
        if ponta1 and t > 0.88:
            f = ((1 - t) / 0.12) ** 0.7
        f = max(f, 0.02)
        meio, larg = (r_in + r_out) / 2, (r_out - r_in) * f
        loop = []
        for u, v in perfil:
            r = meio + (u - 0.5) * larg
            loop.append(bm.verts.new((cx + r * math.cos(a), v * prof * (0.4 + 0.6 * f), zc + r * math.sin(a))))
        aneis.append(loop)
    k = len(perfil)
    for i in range(n):
        for j in range(k):
            bm.faces.new((aneis[i][j], aneis[i][(j + 1) % k], aneis[i + 1][(j + 1) % k], aneis[i + 1][j]))
    bm.faces.new(list(reversed(aneis[0])))
    bm.faces.new(aneis[-1])
    bm.normal_update()
    bm.to_mesh(me)
    for p in me.polygons:
        p.use_smooth = True
    ob = bpy.data.objects.new(nome, me)
    bpy.context.scene.collection.objects.link(ob)
    me.materials.append(mat)
    ob.modifiers.new("Quinas", "EDGE_SPLIT").split_angle = math.radians(35)
    return ob


def gema(nome, cx, z_meio, larg, alto_cima, alto_baixo, prof, lados, mat):
    """Bipiramide facetada (cristal da ponta e losango de baixo)."""
    me = bpy.data.meshes.new(nome)
    bm = bmesh.new()
    cima = bm.verts.new((cx, 0, z_meio + alto_cima))
    baixo = bm.verts.new((cx, 0, z_meio - alto_baixo))
    roda = [bm.verts.new((cx + larg * math.cos(2 * math.pi * i / lados), prof * math.sin(2 * math.pi * i / lados), z_meio)) for i in range(lados)]
    for i in range(lados):
        a, b_ = roda[i], roda[(i + 1) % lados]
        bm.faces.new((a, b_, cima))
        bm.faces.new((b_, a, baixo))
    bm.normal_update()
    bm.to_mesh(me)
    ob = bpy.data.objects.new(nome, me)
    bpy.context.scene.collection.objects.link(ob)
    me.materials.append(mat)
    return ob


def importar_emblema(glb, ouro):
    """Arvore do Tripo + anel, cristal e losango modelados aqui (o anel do Tripo saia escuro e com falhas)."""
    antes = set(bpy.data.objects)
    bpy.ops.import_scene.gltf(filepath=glb)
    novos = [o for o in bpy.data.objects if o not in antes]
    malha = next(o for o in novos if o.type == "MESH")
    bpy.context.view_layer.update()
    mw = malha.matrix_world
    ws = [mw @ v.co for v in malha.data.vertices]
    mnx, mxx = min(v.x for v in ws), max(v.x for v in ws)
    cx, R = (mnx + mxx) / 2, (mxx - mnx) / 2
    esq = [v for v in ws if v.x < mnx + 0.01 * R * 2]
    zc = sum(v.z for v in esq) / len(esq)
    prof = max(v.y for v in ws) - min(v.y for v in ws)
    # apaga a banda do anel do Tripo (fora de 0.80 R), menos embaixo, onde as raizes saem do anel
    bm = bmesh.new()
    bm.from_mesh(malha.data)
    inv = mw.inverted()
    fora = []
    for v in bm.verts:
        w = mw @ v.co
        d = math.hypot(w.x - cx, w.z - zc) / R
        ang = math.degrees(math.atan2(w.z - zc, w.x - cx))
        if d > 0.80 and not (-150 < ang < -40):
            fora.append(v)
        elif abs(w.x - cx) < 0.06 * R and w.z > zc + 0.62 * R:
            fora.append(v)  # o cristal do Tripo (o novo fica no lugar)
    bmesh.ops.delete(bm, geom=fora, context="VERTS")
    bm.to_mesh(malha.data)
    bm.free()
    del inv
    pecas = [
        anel("AnelEsq", cx, zc, 0.87 * R, 1.0 * R, prof * 0.75, 96, 218, False, True, ouro),
        anel("AnelDir", cx, zc, 0.87 * R, 1.0 * R, prof * 0.75, 84, -38, False, True, ouro),
        anel("FileteEsq", cx, zc, 0.825 * R, 0.85 * R, prof * 0.4, 100, 214, True, True, ouro),
        anel("FileteDir", cx, zc, 0.825 * R, 0.85 * R, prof * 0.4, 80, -34, True, True, ouro),
    ]
    cristal = mat_cristal()
    pecas.append(gema("Cristal", cx, zc + 0.86 * R, 0.1 * R, 0.42 * R, 0.30 * R, prof * 0.35, 6, cristal))
    base_z = min(v.z for v in ws)
    pecas.append(gema("Losango", cx, base_z + 0.10 * R, 0.08 * R, 0.10 * R, 0.13 * R, prof * 0.3, 4, cristal))
    luz("BrilhoCristal", "POINT", (cx, -prof, zc + 0.9 * R), "#38B6FF", 25, raio=0.02)
    # normaliza: altura 2, centro na origem, frente para -Y
    todos = [malha] + pecas
    cs = [o.matrix_world @ Vector(c) for o in todos for c in o.bound_box]
    mn = Vector((min(c.x for c in cs), min(c.y for c in cs), min(c.z for c in cs)))
    mxv = Vector((max(c.x for c in cs), max(c.y for c in cs), max(c.z for c in cs)))
    k = 2.0 / (mxv.z - mn.z)
    centro = (mn + mxv) / 2
    piv = bpy.data.objects.new("Emblema", None)
    bpy.context.scene.collection.objects.link(piv)
    raiz = malha
    while raiz.parent:
        raiz = raiz.parent
    for o in [raiz] + pecas + [bpy.data.objects["BrilhoCristal"]]:
        o.parent = piv
        o.location = o.location - centro
    piv.scale = (k, k, k)
    for s in malha.material_slots:
        if s.material:
            realcar_tripo(s.material)
    malha.name = "EmblemaMalha"
    # centro e raio do anel ja normalizados: o icone centra no anel (a mascara do Android e um circulo)
    return piv, (zc - centro.z) * k, R * k


def octaedro(nome, comp, alt, prof, mat):
    """Lamina dourada pontuda (enfeite do 'of' e losango de baixo)."""
    me = bpy.data.meshes.new(nome)
    bm = bmesh.new()
    vs = [
        bm.verts.new(p)
        for p in (
            (-comp / 2, 0, 0),
            (comp / 2, 0, 0),
            (0, 0, alt / 2),
            (0, 0, -alt / 2),
            (0, -prof / 2, 0),
            (0, prof / 2, 0),
        )
    ]
    for a, b_, c in (
        (0, 2, 4),
        (2, 1, 4),
        (1, 3, 4),
        (3, 0, 4),
        (2, 0, 5),
        (1, 2, 5),
        (3, 1, 5),
        (0, 3, 5),
    ):
        bm.faces.new((vs[a], vs[b_], vs[c]))
    bm.normal_update()
    bm.to_mesh(me)
    ob = bpy.data.objects.new(nome, me)
    bpy.context.scene.collection.objects.link(ob)
    me.materials.append(mat)
    return ob


def texto(corpo, tam, z, mats, fonte):
    cu = bpy.data.curves.new("T_" + corpo, "FONT")
    cu.body = corpo
    cu.font = fonte
    cu.size = tam
    cu.align_x, cu.align_y = "CENTER", "CENTER"
    cu.extrude = tam * 0.07
    cu.bevel_depth = tam * 0.022
    cu.bevel_resolution = 4
    cu.space_character = 1.04
    for m in mats:
        cu.materials.append(m)
    ob = bpy.data.objects.new("T_" + corpo, cu)
    ob.rotation_euler = (math.radians(90), 0, 0)
    ob.location = (0, 0, z)
    bpy.context.scene.collection.objects.link(ob)
    return ob


def letreiro(z0, ouro, azul):
    """CHRONICLES / of / EXISTENCE: a Cinzel desenha minuscula como versalete, entao a capitular sai maior sozinha."""
    fonte = bpy.data.fonts.load(FONTE)
    objs = [texto("Chronicles", 0.95, z0, [ouro], fonte)]
    objs.append(texto("of", 0.42, z0 - 0.66, [ouro], fonte))
    ex = texto("EXistence", 0.95, z0 - 1.30, [ouro, azul], fonte)
    ex.data.body_format[1].material_index = 1
    objs.append(ex)
    for s in (-1, 1):
        lam = octaedro("Lamina", 1.55, 0.05, 0.035, ouro)
        lam.location = (s * 1.25, 0, z0 - 0.64)
        objs.append(lam)
    los = octaedro("Losango", 0.16, 0.26, 0.08, ouro)
    los.location = (0, 0, z0 - 1.98)
    objs.append(los)
    for s in (-1, 1):
        lam = octaedro("LaminaBaixo", 1.5, 0.035, 0.03, ouro)
        lam.location = (s * 0.9, 0, z0 - 1.98)
        objs.append(lam)
    bpy.context.view_layer.update()
    # o X aceso ilumina as letras vizinhas
    x_pos = ex.matrix_world @ Vector((-1.05, 0.0, 0.0))
    luz("BrilhoX", "POINT", (x_pos.x, -0.6, z0 - 1.30), AZUL, 60, raio=0.15)
    return objs


def luz(nome, tipo, pos, cor, watts, raio=None, tam=None, alvo=(0, 0, 0)):
    ld = bpy.data.lights.new(nome, tipo)
    ld.color = hexrgb(cor)[:3] if isinstance(cor, str) else cor
    ld.energy = watts * GANHO
    if raio is not None:
        ld.shadow_soft_size = raio
    if tam is not None:
        ld.size = tam
    ob = bpy.data.objects.new(nome, ld)
    ob.location = pos
    bpy.context.scene.collection.objects.link(ob)
    d = Vector(alvo) - Vector(pos)
    ob.rotation_euler = d.to_track_quat("-Z", "Y").to_euler()
    return ob


def estudio(escala=1.0):
    """Rim azul na esquerda, quente dourado na direita, chave suave de frente e um contraluz em cima."""
    e = escala
    luz(
        "RimAzul", "AREA", (-4.5 * e, 3.0 * e, 1.5 * e), "#8FCBFF", 1500 * e * e, tam=2.5 * e
    )
    luz(
        "RimQuente",
        "AREA",
        (4.5 * e, 3.0 * e, 0.8 * e),
        LARANJA,
        1800 * e * e,
        tam=2.5 * e,
    )
    luz(
        "ChaveQuente",
        "AREA",
        (3.5 * e, -5.0 * e, 3.0 * e),
        "#FFD9A0",
        900 * e * e,
        tam=3.0 * e,
    )
    luz(
        "ChaveFria",
        "AREA",
        (-4.0 * e, -4.5 * e, 1.0 * e),
        "#7FC8FF",
        350 * e * e,
        tam=3.0 * e,
    )
    luz("Topo", "AREA", (0, 1.0 * e, 6.0 * e), "#FFFFFF", 500 * e * e, tam=4.0 * e)


def mundo(forca=0.35):
    w = bpy.data.worlds.new("Mundo")
    bpy.context.scene.world = w
    nt = w.node_tree
    bg = next(n for n in nt.nodes if n.type == "BACKGROUND")
    bg.inputs["Strength"].default_value = 1.0
    # estudio so para os reflexos do metal: azul a esquerda, dourado a direita, softbox branca em cima.
    # A camera ve so o fundo escuro (`forca`), entao a key art e os PNGs transparentes nao mudam.
    tc = nt.nodes.new("ShaderNodeTexCoord")
    sep = nt.nodes.new("ShaderNodeSeparateXYZ")
    nt.links.new(tc.outputs["Generated"], sep.inputs[0])
    lado = nt.nodes.new("ShaderNodeValToRGB")
    lado.color_ramp.elements[0].color = hexrgb("#EEF4FF", 1.3)
    lado.color_ramp.elements[1].color = hexrgb("#FFD08A", 2.4)
    # softboxes recortadas nas laterais e chao escuro entre elas: o metal ganha faixas claras nitidas
    for pos in (0.16, 0.84):
        lado.color_ramp.elements.new(pos).color = hexrgb("#0A0E18", 0.15)
    lado.color_ramp.elements.new(0.07).color = hexrgb("#EEF4FF", 1.3)
    lado.color_ramp.elements.new(0.93).color = hexrgb("#FFD08A", 2.4)
    lado.color_ramp.interpolation = "EASE"
    mr = nt.nodes.new("ShaderNodeMapRange")
    mr.inputs["From Min"].default_value, mr.inputs["From Max"].default_value = -1.0, 1.0
    nt.links.new(sep.outputs["X"], mr.inputs["Value"])
    nt.links.new(mr.outputs[0], lado.inputs[0])
    topo = nt.nodes.new("ShaderNodeMapRange")
    topo.inputs["From Min"].default_value, topo.inputs["From Max"].default_value = 0.8, 0.86
    topo.inputs["To Max"].default_value = 3.0
    nt.links.new(sep.outputs["Z"], topo.inputs["Value"])
    soma = nt.nodes.new("ShaderNodeMix")
    soma.data_type = "RGBA"
    soma.blend_type = "ADD"
    nt.links.new(topo.outputs[0], soma.inputs["Factor"])
    soma.inputs["B"].default_value = (1.0, 0.95, 0.88, 1.0)
    nt.links.new(lado.outputs[0], soma.inputs["A"])
    # softbox de frente (atras da camera, -Y): a face chapada das letras reflete ouro claro em vez do vazio
    frente = nt.nodes.new("ShaderNodeMapRange")
    frente.inputs["From Min"].default_value, frente.inputs["From Max"].default_value = -0.6, -0.95
    frente.inputs["To Max"].default_value = 1.6
    nt.links.new(sep.outputs["Y"], frente.inputs["Value"])
    soma2 = nt.nodes.new("ShaderNodeMix")
    soma2.data_type = "RGBA"
    soma2.blend_type = "ADD"
    nt.links.new(frente.outputs[0], soma2.inputs["Factor"])
    soma2.inputs["B"].default_value = (1.0, 0.9, 0.75, 1.0)
    nt.links.new(soma.outputs["Result"], soma2.inputs["A"])
    soma = soma2
    cam = nt.nodes.new("ShaderNodeLightPath")
    fim = nt.nodes.new("ShaderNodeMix")
    fim.data_type = "RGBA"
    nt.links.new(cam.outputs["Is Camera Ray"], fim.inputs["Factor"])
    nt.links.new(soma.outputs["Result"], fim.inputs["A"])
    fim.inputs["B"].default_value = hexrgb("#10294D", forca)
    nt.links.new(fim.outputs["Result"], bg.inputs["Color"])


def plano_ceu(nome, larg, alt, y, z, mat):
    bpy.ops.mesh.primitive_plane_add(
        size=1, location=(0, y, z), rotation=(math.radians(90), 0, 0)
    )
    p = bpy.context.active_object
    p.name = nome
    p.scale = (larg, alt, 1)
    p.data.materials.append(mat)
    p.visible_shadow = False
    return p


def serra(nome, x0, x1, base, alt, y, semente, mat, rugas=1.0, n=320):
    """Cadeia de montanhas como silhueta vertical: perfil de ruido em x, preenchida ate bem abaixo do horizonte."""
    me = bpy.data.meshes.new(nome)
    bm = bmesh.new()
    topo = []
    for i in range(n + 1):
        u = i / n
        x = x0 + (x1 - x0) * u
        h = noise.fractal(Vector((u * 3.0 * rugas, semente, 0)), 0.55, 2.1, 6)
        pico = abs(noise.noise(Vector((u * 1.3 * rugas, semente * 2.0, 1.0))))
        # V: baixo no meio (atras do logo), alto nas bordas da vista
        borda = min(1.0, abs(x) / ((y + KEY_DIST) * 0.36)) ** 1.5
        z = base + alt * (0.55 + 0.45 * h + 0.9 * pico) * (0.45 + 1.2 * borda)
        topo.append(bm.verts.new((x, y, z)))
    fundo_ = [
        bm.verts.new((x0 + (x1 - x0) * i / n, y, base - 30)) for i in range(n + 1)
    ]
    for i in range(n):
        bm.faces.new((fundo_[i], fundo_[i + 1], topo[i + 1], topo[i]))
    bm.to_mesh(me)
    ob = bpy.data.objects.new(nome, me)
    bpy.context.scene.collection.objects.link(ob)
    me.materials.append(mat)
    ob.visible_shadow = False
    return ob


def paisagem():
    """Key art: ceu noturno com o por do sol atras do vale, serras em V emoldurando o logo, cidadela a direita, ilhas."""
    rnd = random.Random(11)
    ceu = mat_ceu("CeuKeyart", estrelas=1.6, horizonte=0.5, clarao=1.0, halo=0.12, centro=(0.62, 0.4), nucleo=("#FFB347", 1.5))
    plano_ceu("Ceu", 92, 54, 90, 1, ceu)
    camadas = [  # y, base, altura, cor, nevoa, rugas
        (80, -4, 9, "#1B3A63", "#3B6FA8", 0.8),
        (60, -6, 7, "#122948", "#2A5486", 1.2),
        (42, -7, 5.5, "#0B1B33", "#1D3D66", 1.6),
        (28, -8, 4.2, "#050D1B", "#0E2240", 2.2),
    ]
    for i, (y, base, alt, cor, nev, rug) in enumerate(camadas):
        serra("Serra%d" % i, -y * 1.2, y * 1.2, base, alt, y, 3.1 + i * 1.7, mat_silhueta("Serra%d" % i, hexrgb(cor), hexrgb(nev)), rug)
    # cidadela: torres finas na direita, atras da 2a serra, com janelas acesas
    pedra = mat_silhueta("Torre", hexrgb("#0B1D3A"), hexrgb("#2A5C9A"))
    janela = mat_brilho("Janela", hexrgb("#FFB347"), 25.0)
    for k in range(11):
        x = 13 + k * 1.15 + rnd.uniform(-0.3, 0.3)
        h = rnd.uniform(4, 10) * (1.5 if k == 4 else 1.0)
        r = rnd.uniform(0.25, 0.55)
        bpy.ops.mesh.primitive_cone_add(vertices=8, radius1=r * 1.15, radius2=0.0, depth=h * 0.4, location=(x, 62, -2 + h + h * 0.2))
        bpy.context.active_object.data.materials.append(pedra)
        bpy.ops.mesh.primitive_cylinder_add(vertices=8, radius=r, depth=h, location=(x, 62, -2 + h / 2))
        bpy.context.active_object.data.materials.append(pedra)
        for _ in range(rnd.randint(2, 5)):
            bpy.ops.mesh.primitive_plane_add(size=0.1, location=(x + rnd.uniform(-0.15, 0.15), 62 - r * 1.01, -1 + rnd.uniform(0.3, 0.95) * h),
                                             rotation=(math.radians(90), 0, 0))
            bpy.context.active_object.data.materials.append(janela)
    # ilhas flutuantes nas bordas (o meio e do logo): rocha com topo chato e raiz pontuda embaixo
    ilha = mat_silhueta("Ilha", hexrgb("#0C1F3A"), hexrgb("#2C5C94"))
    for n_, (x, z, r) in enumerate(((-28, 12, 2.2), (-31, 4, 1.4), (-21, 17, 1.1), (28, 15, 1.7))):
        bpy.ops.mesh.primitive_ico_sphere_add(subdivisions=3, radius=r, location=(x, 70, z))
        ob = bpy.context.active_object
        for v in ob.data.vertices:
            ru = noise.noise(v.co * 1.7 / r + Vector((n_ * 3.0, 0, 0)))
            if v.co.z > 0:
                v.co.z *= 0.18
            else:
                v.co.z *= 2.3 + 0.8 * ru
            v.co.x *= 1.0 + 0.25 * ru
        ob.data.materials.append(ilha)
    # nevoa: faixas claras e transparentes no pe de cada serra (profundidade)
    for y, z, forca in ((70, -3, 0.5), (52, -5, 0.45), (36, -7, 0.4)):
        nevoa("Nevoa%d" % y, y, z, (y + KEY_DIST) * 0.8, 9, forca)


def nevoa(nome, y, z, larg, alt, forca):
    m = bpy.data.materials.new(nome)
    nt = m.node_tree
    for n in list(nt.nodes):
        if n.type != "OUTPUT_MATERIAL":
            nt.nodes.remove(n)
    out = next(n for n in nt.nodes if n.type == "OUTPUT_MATERIAL")
    tc = nt.nodes.new("ShaderNodeTexCoord")
    sep = nt.nodes.new("ShaderNodeSeparateXYZ")
    nt.links.new(tc.outputs["UV"], sep.inputs[0])
    sobe = nt.nodes.new("ShaderNodeMapRange")
    sobe.inputs["From Min"].default_value, sobe.inputs["From Max"].default_value = 0.0, 1.0
    sobe.inputs["To Min"].default_value, sobe.inputs["To Max"].default_value = forca, 0.0
    nt.links.new(sep.outputs["Y"], sobe.inputs["Value"])
    ru = no(nt, "ShaderNodeTexNoise", Scale=3.0, Detail=6.0)
    nt.links.new(tc.outputs["UV"], ru.inputs["Vector"])
    mul = nt.nodes.new("ShaderNodeMath")
    mul.operation = "MULTIPLY"
    nt.links.new(sobe.outputs[0], mul.inputs[0])
    nt.links.new(ru.outputs["Fac"], mul.inputs[1])
    tr = nt.nodes.new("ShaderNodeBsdfTransparent")
    em = no(nt, "ShaderNodeEmission", Strength=1.0)
    em.inputs["Color"].default_value = hexrgb("#5D8FCC")
    mix = nt.nodes.new("ShaderNodeMixShader")
    nt.links.new(mul.outputs[0], mix.inputs["Fac"])
    nt.links.new(tr.outputs[0], mix.inputs[1])
    nt.links.new(em.outputs[0], mix.inputs[2])
    nt.links.new(mix.outputs[0], out.inputs["Surface"])
    p = plano_ceu(nome, larg, alt, y, z, m)
    return p


# ---------------------------------------------------------------- cena e render


def camera(pos, alvo, lente):
    cd = bpy.data.cameras.new("Cam")
    cd.lens = lente
    cd.sensor_width = 36
    cam = bpy.data.objects.new("Cam", cd)
    bpy.context.scene.collection.objects.link(cam)
    cam.location = pos
    cam.rotation_euler = (
        (Vector(alvo) - Vector(pos)).to_track_quat("-Z", "Y").to_euler()
    )
    bpy.context.scene.camera = cam
    return cam


def dist_para(largura_vista, lente, aspecto=1.0):
    """Distancia em que a camera ve `largura_vista` metros na horizontal (sensor 36 mm, encaixe horizontal)."""
    return largura_vista * lente / 36.0


def configurar(res, amostras, escala, transparente):
    sc = bpy.context.scene
    try:
        sc.render.engine = "CYCLES"
    except TypeError:
        pass
    sc.cycles.samples = amostras
    sc.cycles.use_denoising = True
    prefs = bpy.context.preferences.addons["cycles"].preferences
    for t in ("OPTIX", "CUDA"):
        try:
            prefs.compute_device_type = t
            prefs.get_devices()
            for d in prefs.devices:
                d.use = d.type != "CPU"
            sc.cycles.device = "GPU"
            break
        except Exception:
            continue
    sc.render.resolution_x, sc.render.resolution_y = res
    sc.render.resolution_percentage = int(escala * 100)
    sc.render.film_transparent = transparente
    sc.render.image_settings.file_format = "PNG"
    sc.render.image_settings.color_mode = "RGBA" if transparente else "RGB"
    sc.render.image_settings.color_depth = "16"
    # a lista do enum vem estatica no modo -b; tenta os nomes direto
    sc.view_settings.view_transform = "Khronos PBR Neutral"
    for alvo in (
        "AgX - Medium High Contrast",
        "Medium High Contrast",
        "AgX - Punchy",
        "Punchy",
    ):
        try:
            sc.view_settings.look = alvo
            break
        except TypeError:
            continue
    sc.cycles.max_bounces = 8
    sc.cycles.caustics_reflective = False


def montar(tomada, glb):
    bpy.ops.wm.read_factory_settings(use_empty=True)
    ouro = mat_ouro()
    azulx = mat_brilho("XAceso", hexrgb(AZUL), 2.2)
    emblema = zc = raio = None
    if tomada != "fundo":
        emblema, zc, raio = importar_emblema(glb, ouro)
    if tomada == "frente":
        mundo(0.25)
        estudio()
        # anel com 48% do lado; a ponta do cristal (1.28 R) fica dentro do circulo seguro de 66%
        camera((0, -dist_para(4.2 * raio, 100), zc), (0, 0, zc), 100)
    elif tomada == "fundo":
        plano_ceu("Ceu", 7, 7, 3, 0, mat_ceu("Ceu"))
        camera((0, -dist_para(4.0, 100) + 3, 0), (0, 3, 0), 100)
    elif tomada == "play":
        mundo(0.25)
        estudio()
        plano_ceu("Ceu", 7.5, 7.5, 1.5, 0, mat_ceu("Ceu"))
        d = dist_para(2.9 * raio, 100)
        camera((0, -d, zc), (0, 0, zc), 100)
        # o ceu cobre a vista toda na distancia do plano
        bpy.data.objects["Ceu"].scale = ((d + 1.5) * 36 / 100 * 1.05,) * 2 + (1,)
        bpy.data.objects["Ceu"].location.z = zc
    elif tomada == "logo":
        mundo(0.25)
        estudio(1.4)
        emblema.location = (0, 0.15, 1.78)
        emblema.scale = emblema.scale * 1.45
        letreiro(-0.05, ouro, azulx)
        camera((0, -dist_para(12.6, 100), 0.6), (0, 0, 0.6), 100)
    elif tomada == "keyart":
        mundo(0.15)
        estudio(1.4)
        emblema.location = (0, 0.15, 1.78)
        emblema.scale = emblema.scale * 1.45
        letreiro(-0.05, ouro, azulx)
        paisagem()
        camera((0, -KEY_DIST, 0.3), (0, 0, 0.7), 50)
    return emblema


def main():
    a = sys.argv[sys.argv.index("--") + 1 :] if "--" in sys.argv else []
    tomada, saida = a[0], a[1]
    amostras = int(a[a.index("--amostras") + 1]) if "--amostras" in a else 128
    escala = float(a[a.index("--escala") + 1]) if "--escala" in a else 1.0
    glb = a[a.index("--glb") + 1] if "--glb" in a else GLB
    montar(tomada, glb)
    configurar(TOMADAS[tomada], amostras, escala, tomada in ("frente", "logo"))
    if "--blend" in a:
        bpy.ops.wm.save_as_mainfile(filepath=a[a.index("--blend") + 1], compress=True)
    bpy.context.scene.render.filepath = saida
    bpy.ops.render.render(write_still=True)
    print("OK", tomada, saida)


main()
