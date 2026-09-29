"""Gera o humanoide-placeholder do COE (crianca de 5 anos, 1,10 m, corpo + 8 clips).

Uso (headless, sem abrir o Blender; da raiz do repo):
    "C:\\Program Files\\Blender Foundation\\Blender 5.2\\blender.exe" --background --factory-startup
        --python client/tools/placeholder_humanoid.py -- client/Assets/_COE/Art/Humanoid
Contrato de escala, eixos e ossos: docs/arte/PIPELINE.md.

Escreve Model.fbx (T-pose) + Idle/Run/Attack1/Attack2/Attack3/Dodge/Hit/Death .fbx,
que e exatamente o que "COE -> Montar humanoide" (Editor/HumanoidSetup.cs) espera.
Nomes de osso = nomes do HumanBodyBones do Unity (Left/Right + UpperArm/LowerArm/UpperLeg/LowerLeg),
que o auto-mapeamento Humanoid reconhece; e o mesmo padrao que o modelo final segue (PIPELINE.md, rig).

ponytail: skin rigido (1 osso por parte, sem pesos suaves) e formas primitivas. Nao e arte:
e um boneco legivel em silhueta para medir jogabilidade e FPS antes da arte existir.
"""
import math
import os
import sys

import bpy
from mathutils import Vector

FPS = 30
H = 1.10      # altura total, pes em z=0 = BodyScale.Crianca5 (Scripts/Character/BodyScale.cs)
S = H / 1.75  # as translacoes dos clips foram afinadas no boneco adulto de 1,75 m; escalam com a altura
# nome do osso -> (inicio, fim) em metros. Crianca de 5 anos olhando para -Y no Blender (= +Z no Unity
# com axis_forward=-Z/axis_up=Y): Left* em +X, dedos do pe em -Y. Proporcao infantil (docs/arte/PIPELINE.md):
# cabeca 0,20 m = 5,5 cabecas de altura (adulto ~7,5); quadril 0,52 m = 47% da altura (adulto ~53%).
# ponytail: numeros so para 5 anos; a base de 8 anos (1,28 m) e outra tabela, nao este corpo escalonado.
BONES = {
    "Hips":          ((0.00, 0.00, 0.52), (0.00, 0.00, 0.60)),
    "Spine":         ((0.00, 0.00, 0.60), (0.00, 0.00, 0.70)),
    "Chest":         ((0.00, 0.00, 0.70), (0.00, 0.00, 0.86)),
    "Neck":          ((0.00, 0.00, 0.86), (0.00, 0.00, 0.90)),
    "Head":          ((0.00, 0.00, 0.90), (0.00, 0.00, 1.10)),
    "LeftShoulder":  ((0.03, 0.00, 0.83), (0.11, 0.00, 0.83)),
    "LeftUpperArm":  ((0.11, 0.00, 0.83), (0.29, 0.00, 0.83)),
    "LeftLowerArm":  ((0.29, 0.00, 0.83), (0.44, 0.00, 0.83)),
    "LeftHand":      ((0.44, 0.00, 0.83), (0.53, 0.00, 0.83)),
    "RightShoulder": ((-0.03, 0.00, 0.83), (-0.11, 0.00, 0.83)),
    "RightUpperArm": ((-0.11, 0.00, 0.83), (-0.29, 0.00, 0.83)),
    "RightLowerArm": ((-0.29, 0.00, 0.83), (-0.44, 0.00, 0.83)),
    "RightHand":     ((-0.44, 0.00, 0.83), (-0.53, 0.00, 0.83)),
    "LeftUpperLeg":  ((0.07, 0.00, 0.52), (0.07, 0.00, 0.29)),
    "LeftLowerLeg":  ((0.07, 0.00, 0.29), (0.07, 0.00, 0.06)),
    "LeftFoot":      ((0.07, 0.00, 0.06), (0.07, -0.10, 0.015)),
    "RightUpperLeg": ((-0.07, 0.00, 0.52), (-0.07, 0.00, 0.29)),
    "RightLowerLeg": ((-0.07, 0.00, 0.29), (-0.07, 0.00, 0.06)),
    "RightFoot":     ((-0.07, 0.00, 0.06), (-0.07, -0.10, 0.015)),
}
PARENTS = {
    "Spine": "Hips", "Chest": "Spine", "Neck": "Chest", "Head": "Neck",
    "LeftShoulder": "Chest", "LeftUpperArm": "LeftShoulder", "LeftLowerArm": "LeftUpperArm", "LeftHand": "LeftLowerArm",
    "RightShoulder": "Chest", "RightUpperArm": "RightShoulder", "RightLowerArm": "RightUpperArm", "RightHand": "RightLowerArm",
    "LeftUpperLeg": "Hips", "LeftLowerLeg": "LeftUpperLeg", "LeftFoot": "LeftLowerLeg",
    "RightUpperLeg": "Hips", "RightLowerLeg": "RightUpperLeg", "RightFoot": "RightLowerLeg",
}
# parte do corpo -> (osso, tamanho x/y/z da caixa centrada no meio do osso). Cabeca vai de 0,90 a 1,10
# (topo = H) e o pe de 0,00 a 0,075 (sola no chao): a caixa envolvente da malha e exatamente 0..H.
PARTS = {
    "Hips": (0.20, 0.13, 0.08), "Spine": (0.19, 0.13, 0.10), "Chest": (0.22, 0.14, 0.16),
    "Neck": (0.06, 0.06, 0.04), "Head": (0.17, 0.18, 0.20),
    "LeftUpperArm": (0.18, 0.07, 0.07), "LeftLowerArm": (0.15, 0.06, 0.06), "LeftHand": (0.09, 0.07, 0.035),
    "RightUpperArm": (0.18, 0.07, 0.07), "RightLowerArm": (0.15, 0.06, 0.06), "RightHand": (0.09, 0.07, 0.035),
    "LeftUpperLeg": (0.10, 0.10, 0.23), "LeftLowerLeg": (0.08, 0.08, 0.23), "LeftFoot": (0.07, 0.16, 0.075),
    "RightUpperLeg": (0.10, 0.10, 0.23), "RightLowerLeg": (0.08, 0.08, 0.23), "RightFoot": (0.07, 0.16, 0.075),
}


def clean():
    bpy.ops.wm.read_factory_settings(use_empty=True)
    bpy.context.scene.render.fps = FPS


def build_armature():
    arm_data = bpy.data.armatures.new("PlaceholderRig")
    arm = bpy.data.objects.new("PlaceholderRig", arm_data)
    bpy.context.collection.objects.link(arm)
    bpy.context.view_layer.objects.active = arm
    bpy.ops.object.mode_set(mode="EDIT")
    for name, (head, tail) in BONES.items():
        b = arm_data.edit_bones.new(name)
        b.head, b.tail = Vector(head), Vector(tail)
        b.use_connect = False
    for child, parent in PARENTS.items():
        arm_data.edit_bones[child].parent = arm_data.edit_bones[parent]
    bpy.ops.object.mode_set(mode="OBJECT")
    return arm


def build_body(arm):
    """Uma caixa por parte, cada uma 100% presa a um osso (skin rigido)."""
    meshes = []
    for bone_name, (sx, sy, sz) in PARTS.items():
        head, tail = (Vector(v) for v in BONES[bone_name])
        center = (head + tail) / 2.0
        bpy.ops.mesh.primitive_cube_add(size=1.0, location=center)
        ob = bpy.context.active_object
        ob.name = "part_" + bone_name
        ob.scale = (sx, sy, sz)
        bpy.ops.object.transform_apply(location=False, rotation=False, scale=True)
        ob["bone"] = bone_name
        meshes.append(ob)

    # grupo de vertices por parte ANTES de juntar: o join preserva os grupos
    for ob in meshes:
        vg = ob.vertex_groups.new(name=ob["bone"])
        vg.add([v.index for v in ob.data.vertices], 1.0, "REPLACE")

    bpy.ops.object.select_all(action="DESELECT")
    for ob in meshes:
        ob.select_set(True)
    body = meshes[0]
    bpy.context.view_layer.objects.active = body
    bpy.ops.object.join()
    body.name = "PlaceholderBody"
    body.data.name = "PlaceholderBody"

    mat = bpy.data.materials.new("PlaceholderSkin")
    mat.use_nodes = True
    bsdf = next(n for n in mat.node_tree.nodes if n.type == "BSDF_PRINCIPLED")
    bsdf.inputs["Base Color"].default_value = (0.62, 0.55, 0.45, 1.0)
    body.data.materials.append(mat)

    body.parent = arm
    mod = body.modifiers.new("Armature", "ARMATURE")
    mod.object = arm
    return body


def rot(arm, bone, frame, x=0.0, y=0.0, z=0.0):
    pb = arm.pose.bones[bone]
    pb.rotation_mode = "XYZ"
    pb.rotation_euler = (math.radians(x), math.radians(y), math.radians(z))
    pb.keyframe_insert("rotation_euler", frame=frame)


def loc(arm, bone, frame, x=0.0, y=0.0, z=0.0):
    """Translacao em eixos do MUNDO (z = para cima), em metros de adulto (escala por S).
    Osso vertical com roll 0: local = (X, Z_mundo, -Y_mundo). Antes gravava z no Z local, que e
    horizontal: Death nunca caia e Dodge nunca subia (medido no FBX importado)."""
    pb = arm.pose.bones[bone]
    pb.location = (x * S, z * S, -y * S)
    pb.keyframe_insert("location", frame=frame)


def clip_idle(arm, n):
    for f, a in ((1, 0), (n // 2, 3), (n, 0)):
        rot(arm, "Spine", f, x=a)
        rot(arm, "Chest", f, x=a * 0.5)
        rot(arm, "Head", f, x=-a * 0.5)
        loc(arm, "Hips", f, z=-a * 0.004)
        rot(arm, "LeftUpperArm", f, z=-4 - a)
        rot(arm, "RightUpperArm", f, z=4 + a)


def clip_run(arm, n):
    h = n // 2
    for f, s in ((1, 1), (h, -1), (n, 1)):
        rot(arm, "LeftUpperLeg", f, x=38 * s)
        rot(arm, "RightUpperLeg", f, x=-38 * s)
        # joelho: x positivo leva a canela para +Y (tras) = flexao certa de quem olha para -Y
        rot(arm, "LeftLowerLeg", f, x=45 if s > 0 else 12)
        rot(arm, "RightLowerLeg", f, x=12 if s > 0 else 45)
        rot(arm, "LeftUpperArm", f, x=-30 * s, z=-10)
        rot(arm, "RightUpperArm", f, x=30 * s, z=10)
        rot(arm, "LeftLowerArm", f, x=-40)
        rot(arm, "RightLowerArm", f, x=-40)
        rot(arm, "Spine", f, x=8)
        loc(arm, "Hips", f, z=0.03 if s > 0 else 0.0)
    for f in (n // 4, 3 * n // 4):  # passada: peso no chao
        loc(arm, "Hips", f, z=-0.02)


def _swing(arm, n, side, wind, hit, follow):
    other = "Right" if side == "Left" else "Left"
    k = max(2, int(n * 0.40))  # impacto em ~40% do clipe (contrato do pipeline)
    rot(arm, side + "UpperArm", 1, x=0, z=0)
    rot(arm, side + "UpperArm", max(2, k // 2), x=wind, z=-20)
    rot(arm, side + "UpperArm", k, x=hit, z=10)
    rot(arm, side + "UpperArm", n, x=follow, z=0)
    rot(arm, side + "LowerArm", 1, x=-10)
    rot(arm, side + "LowerArm", k, x=-55)
    rot(arm, side + "LowerArm", n, x=-15)
    rot(arm, "Spine", 1, z=0)
    rot(arm, "Spine", max(2, k // 2), z=-14 if side == "Left" else 14)
    rot(arm, "Spine", k, z=16 if side == "Left" else -16)
    rot(arm, "Spine", n, z=0)
    rot(arm, other + "UpperArm", k, x=20, z=10 if other == "Right" else -10)


def clip_attack1(arm, n):
    _swing(arm, n, "Right", wind=-40, hit=55, follow=25)


def clip_attack2(arm, n):
    _swing(arm, n, "Left", wind=-30, hit=60, follow=20)


def clip_attack3(arm, n):
    _swing(arm, n, "Right", wind=-70, hit=80, follow=40)
    k = int(n * 0.45)
    rot(arm, "LeftUpperLeg", 1, x=0)
    rot(arm, "LeftUpperLeg", k, x=25)
    rot(arm, "LeftUpperLeg", n, x=0)


def clip_dodge(arm, n):
    mid = max(2, n // 3)
    for f in (1, mid, n):
        up = f == mid
        rot(arm, "Spine", f, x=-22 if up else 0)
        loc(arm, "Hips", f, z=0.10 if up else 0.0)
        rot(arm, "LeftUpperLeg", f, x=-20 if up else 0)
        rot(arm, "RightUpperLeg", f, x=20 if up else 0)
        rot(arm, "LeftUpperArm", f, z=-30 if up else -4)
        rot(arm, "RightUpperArm", f, z=30 if up else 4)


def clip_hit(arm, n):
    for f, a in ((1, 0), (max(2, n // 3), -18), (n, 0)):
        rot(arm, "Spine", f, x=a)
        rot(arm, "Chest", f, x=a * 0.6)
        rot(arm, "Head", f, x=a * 0.8)
        rot(arm, "LeftUpperArm", f, x=a, z=-12)
        rot(arm, "RightUpperArm", f, x=a, z=12)


def clip_death(arm, n):
    # cai de costas: tronco para +Y, quadril ate ~8 cm do chao, pernas para a frente (-Y) sem furar o chao
    drop = -(BONES["Hips"][0][2] - 0.08) / S
    for f, sp, hz, leg, arm_x in ((1, 0, 0.0, 0, 0), (int(n * 0.35), -25, -0.15, -20, -20), (n, -88, drop, -85, -60)):
        rot(arm, "Spine", f, x=sp)
        rot(arm, "Chest", f, x=sp * 0.4)
        rot(arm, "Head", f, x=-sp * 0.3)
        loc(arm, "Hips", f, z=hz)
        rot(arm, "LeftUpperLeg", f, x=leg)
        rot(arm, "RightUpperLeg", f, x=leg * 0.9)
        rot(arm, "LeftUpperArm", f, x=arm_x, z=-20)
        rot(arm, "RightUpperArm", f, x=arm_x, z=20)


# nome do clipe -> (frames, funcao). Duracoes a 30 FPS.
CLIPS = [
    ("Idle", 75, clip_idle),
    ("Run", 21, clip_run),
    ("Attack1", 14, clip_attack1),
    ("Attack2", 14, clip_attack2),
    ("Attack3", 18, clip_attack3),
    ("Dodge", 12, clip_dodge),
    ("Hit", 9, clip_hit),
    ("Death", 42, clip_death),
]


def rest_pose(arm):
    for pb in arm.pose.bones:
        pb.rotation_mode = "XYZ"
        pb.rotation_euler = (0.0, 0.0, 0.0)
        pb.location = (0.0, 0.0, 0.0)


def export(path, arm, body, animated):
    bpy.ops.object.select_all(action="DESELECT")
    arm.select_set(True)
    body.select_set(True)
    bpy.context.view_layer.objects.active = arm
    bpy.ops.export_scene.fbx(
        filepath=path,
        use_selection=True,
        object_types={"ARMATURE", "MESH"},
        add_leaf_bones=False,
        bake_anim=animated,
        bake_anim_use_all_actions=False,
        bake_anim_use_nla_strips=False,
        bake_anim_simplify_factor=0.0,
        armature_nodetype="NULL",
        apply_unit_scale=True,
        global_scale=1.0,
        path_mode="COPY",
        axis_forward="-Z",
        axis_up="Y",
    )
    return os.path.getsize(path)


def preview(path, arm, body):
    """PNG de conferencia: 3 poses lado a lado no Workbench (sem luz nem material)."""
    sc = bpy.context.scene
    sc.render.engine = "BLENDER_WORKBENCH"
    sc.render.resolution_x, sc.render.resolution_y = 1100, 700
    sc.render.film_transparent = False
    cam_data = bpy.data.cameras.new("PreviewCam")
    cam_data.type = "ORTHO"
    cam_data.ortho_scale = 3.3 * H
    cam = bpy.data.objects.new("PreviewCam", cam_data)
    bpy.context.collection.objects.link(cam)
    cam.location = (0.0, -8.0, 0.55 * H)
    cam.rotation_euler = (math.radians(90), 0.0, 0.0)
    sc.camera = cam

    poses = [("T-pose", None, 0), ("Corrida", clip_run, 21), ("Ataque 3", clip_attack3, 18)]
    copies = []
    for i, (label, fn, frames) in enumerate(poses):
        rest_pose(arm)
        if fn is not None:
            act = bpy.data.actions.new("preview_" + label)
            if not arm.animation_data:
                arm.animation_data_create()
            arm.animation_data.action = act
            fn(arm, frames)
            sc.frame_set(max(1, int(frames * 0.4)))
        dup = body.copy()
        dup.data = body.data.copy()
        bpy.context.collection.objects.link(dup)
        dup.modifiers.clear()
        dup.parent = None
        # aplica a pose atual no vertice (o duplicado nao tem armature)
        depsgraph = bpy.context.evaluated_depsgraph_get()
        dup.data = bpy.data.meshes.new_from_object(body.evaluated_get(depsgraph))
        dup.location.x = (i - 1) * 1.1 * H  # so x: z guarda a origem da malha (centro do quadril)
        copies.append(dup)
        if arm.animation_data and arm.animation_data.action:
            a = arm.animation_data.action
            arm.animation_data.action = None
            bpy.data.actions.remove(a)
    body.hide_render = True
    arm.hide_render = True
    sc.render.filepath = path
    bpy.ops.render.render(write_still=True)
    for d in copies:
        bpy.data.objects.remove(d)
    body.hide_render = False
    return path


def main():
    out = sys.argv[sys.argv.index("--") + 1] if "--" in sys.argv else "."
    out = os.path.abspath(out)
    os.makedirs(out, exist_ok=True)

    clean()
    arm = build_armature()
    body = build_body(arm)
    bpy.context.view_layer.objects.active = arm
    # ponytail: keyframe_insert em pose bone funciona em modo objeto; o exportador FBX exige modo objeto

    written = []
    rest_pose(arm)  # corpo em T-pose, sem animacao
    if arm.animation_data:
        arm.animation_data.action = None
    written.append(("Model.fbx", export(os.path.join(out, "Model.fbx"), arm, body, False)))

    for name, frames, fn in CLIPS:
        rest_pose(arm)
        act = bpy.data.actions.new(name)
        if not arm.animation_data:
            arm.animation_data_create()
        arm.animation_data.action = act
        fn(arm, frames)
        bpy.context.scene.frame_start = 1
        bpy.context.scene.frame_end = frames
        p = os.path.join(out, name + ".fbx")
        written.append((name + ".fbx", export(p, arm, body, True)))
        arm.animation_data.action = None
        bpy.data.actions.remove(act)

    png = os.environ.get("COE_PREVIEW_PNG")
    if png:
        preview(png, arm, body)
        print("PREVIEW " + png)

    print("PLACEHOLDER_OK " + out)
    for n, size in written:
        print("  %-12s %7.1f KB" % (n, size / 1024.0))
    print("  ossos=%d partes=%d" % (len(BONES), len(PARTS)))


main()
