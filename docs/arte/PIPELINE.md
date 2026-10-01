# Pipeline de arte 3D — referência → (Tripo3D) → Blender → Unity

- **Estado:** v0, 2026-09-29; conferido em 2026-09-30. Nenhum asset do jogo foi produzido e não há registro de envio ao Tripo3D. Existe um acervo de concept gerado em serviço externo, só como referência (PROVENIENCIA.md §5). Este documento prepara o técnico para quando a arte entrar.
- **Fontes:** dossiê §J (`docs/direcao/DOSSIE_CONTINUIDADE_v1_0.md`), GDD cap. 09, ADR-0002 (portões G1/G2/G3), `Scripts/Character/BodyScale.cs`, `Editor/HumanoidMapping.cs`, `Editor/HumanoidSetup.cs`, `Scripts/NPC/NpcCatalog.cs`, `Editor/AurenSceneBuilder.cs`.
- **Convenção:** número sem fonte e marcado **HIPÓTESE v0** é ponto de partida, a calibrar no primeiro asset real. Licença e termos do Tripo3D: `docs/arte/PROVENIENCIA.md`.
- **Ferramentas fixadas:** Blender **5.2.1 LTS** (hash `9e2066aef7ef`, `C:\Program Files\Blender Foundation\Blender 5.2`), Unity 6000.3.23f1 URP. Versão nova de Blender ou do exportador FBX só entra depois de reexportar o placeholder e passar no validador (§11).

## 1. Fluxo e portões

| # | Etapa | Entra | Sai | Portão (ADR-0002) |
|---|---|---|---|---|
| 1 | Ficha do personagem/peça | brief | ficha 10 critérios | **G1**: ≥ 14/20, sem zero em C2–C5 |
| 2 | Concept autoral | ficha G1 | imagens em `arte/referencias/<assunto>/` | **G2**: teste cego de silhueta (≥ 4/5) e de descrição |
| 3 | Malha base | concept G2 **próprio** | GLB do Tripo3D ou malha manual em `arte/fonte/<Categoria>/<id>/` (imutável) | registro em PROVENIENCIA antes de gerar |
| 4 | Blender | malha base | `.blend` master em `arte/fonte/<Categoria>/<id>/<id>.blend` | — |
| 5 | Retopologia, UV, rig, bake, LOD, colisão | `.blend` | FBX + PNG em `client/Assets/_COE/Art/<Categoria>/<id>/` | **G3**: G2 + licença/proveniência registradas, **antes** de rig, animação e import |
| 6 | Import Unity + validador | FBX | prefab | §11 sem FAIL |
| 7 | QA no jogo | prefab | aceite | aceite artístico do idealizador |

- Referência de terceiros (Pinterest, capturas de outros jogos) **nunca** é entrada de gerador: só concept próprio aprovado em G2. Motivo: PROVENIENCIA.md §2 (termos do Tripo3D, cláusula 3.1).
- Master (`.blend`, GLB/OBJ de origem, PSD) fica **fora** de `client/Assets`: o Unity tentaria importar `.blend` via Blender instalado, lento e frágil. `client/Assets` recebe só derivados (FBX, PNG).
- ponytail: `arte/fonte/` ainda não existe; nasce com o primeiro asset. Pasta vazia não vai para o git.
- **Tripo Bridge** (`client/Packages/com.tripo3d.unitybridge`, v1.0.14, baixado de `tripo-public.tripo3d.ai` em 2026-09-30): no Unity, `Tools > Tripo3D Bridge > Start Server` abre um WebSocket em `127.0.0.1:60610` e o Tripo Studio manda o modelo para `Assets/TripoModels/<nome>/` (materiais, texturas e uma instância na cena aberta). Essa pasta é **caixa de entrada** e está no `.gitignore`: o que chega ali ainda não é asset do COE. Vale a mesma ordem da tabela (G1 → G2 → registro na PROVENIENCIA antes de gerar), o master vai para `arte/fonte/` e o personagem passa pelo Blender (retopologia, rig com os ossos de §7.1, T-pose) antes de entrar em `Art/<Categoria>/<id>/`. Ligar o servidor só na hora de receber (ele não confere a origem da conexão) e com uma cena descartável aberta, nunca `Bootstrap` ou `Auren`, que são geradas por script. O plugin também apaga, uma vez por dia ao abrir o editor, subpastas com mais de 7 dias em `Assets/ImportedModels/`: não usar esse nome de pasta.

## 2. Unidades, eixos, origem, transformações

| Item | Blender (autoria) | Unity (runtime) |
|---|---|---|
| Unidade | Metric, Unit Scale `1.0`, Length `Meters`: 1 BU = 1 m | 1 unidade = 1 m |
| Cima | +Z | +Y |
| Frente do personagem/objeto | **−Y** (vista Front, numpad 1, olha para ele) | **+Z** |
| Esquerda do personagem | +X | −X |
| Origem | chão, centro entre os pés (personagem); centro da base (prop); centro do footprint no chão (estrutura) | root do FBX em (0,0,0) |

- Conversão (derivação): exportador FBX com `axis_forward="-Z"`, `axis_up="Y"` leva Blender (x, y, z) → FBX (x, z, −y); o Unity espelha X no import → (−x, z, −y). Blender −Y vira Unity +Z. Verificado por medição no placeholder (§7.3); a checagem final é a regra V11 no Unity.
- Import de GLB do Tripo3D no Blender: o importador glTF converte +Y-up/+Z-frente do glTF para Z-up/−Y-frente. Conferir a frente por asset: geração por imagem nem sempre sai de frente.
- Antes do export, no objeto raiz e na malha: `Apply > All Transforms` (location 0, rotation 0, scale 1). Armature também com transform aplicado. Nenhum objeto com escala negativa.
- Estrutura: porta/fachada principal para −Y no Blender (= +Z local no Unity), como o greybox (`AurenSceneBuilder`: "porta (+Z local)").

## 3. Escala e bases corporais

Altura = topo da cabeça (sem cabelo volumoso) até a sola, em T-pose, pés em y = 0. Escala e proporção não dependem de plataforma: as faixas valem para Android (ADR-0006) e para a build Windows de desenvolvimento.

| Base (id do avatar) | Alvo (m) | Fonte | Faixa aceita (V09) | r_perna (V13) | r_cabeca (V13) |
|---|---|---|---|---|---|
| `avatar_crianca5` | **1,10** | `BodyScale.Crianca5` | 1,067 – 1,155 | 0,40 – 0,49 | 0,19 – 0,28 |
| `avatar_crianca8` | **1,28** | `BodyScale.Crianca8` | 1,242 – 1,344 | 0,43 – 0,51 | 0,17 – 0,25 |
| `avatar_adolescente` | 1,60 — **HIPÓTESE v0**, não existe em `BodyScale`; fora do slice | — | 1,552 – 1,680 | 0,46 – 0,54 | 0,14 – 0,21 |
| `avatar_adulto` | **1,75** | `BodyScale.Adulto` | 1,698 – 1,838 | 0,50 – 0,58 | 0,12 – 0,18 |

- Faixa de altura = alvo × [0,97; 1,05]: −3% tolera pose, +5% tolera cabelo. **HIPÓTESE v0.**
- `r_perna = (y(LeftUpperLeg) − minY) / h` (altura do quadril); `r_cabeca = (maxY − y(Neck)) / h` (cabeça + pescoço); `minY`, `maxY`, `h` como em §11. Faixas **HIPÓTESE v0**, derivadas do placeholder infantil (0,473 / 0,218) e do adulto antigo (0,543 / 0,175).
- **Não escalonar** (dossiê §J): as faixas de `crianca5` e `adulto` não se sobrepõem em nenhuma das duas razões. Criança escalonada para 1,75 m reprova em V13; adulto encolhido para 1,10 m também.
- Base infantil = **uma** topologia e **um** rig com dois presets de proporção (5 e 8 anos), por comprimento de osso + shape keys, nunca escala uniforme. GDD cap. 09: "Não criar modelos por ano de idade." Adolescente e adulto são bases próprias.
- Referência de proporção do placeholder de 5 anos (`client/tools/placeholder_humanoid.py`): cabeça 0,20 m (5,5 cabeças de altura), quadril a 0,52 m, ombros a 0,83 m, envergadura 1,06 m, pé 0,16 m.

### 3.1 NPCs de Auren (ids do `NpcCatalog`)

| id | Base | Faixa de altura (V09) |
|---|---|---|
| `mara`, `daren`, `borin`, `lysa`, `tovin`, `eira`, `oren`, `maelis` | adulto | 1,55 – 1,95 m |
| `nilo`, `sera` | criança (amigos de infância, dossiê §G) | 1,00 – 1,20 m |

- Faixas **HIPÓTESE v0** (adulto: 1,75 ± ~11%; criança: faixa ampla em torno de 1,10). Altura individual é direção de arte, decidida na ficha G1.
- NPC criança muda no salto temporal (dossiê §G); a versão de 8 anos usa faixa 1,15 – 1,40 m (**HIPÓTESE v0**) e só é modelada quando a ficha pedir.

### 3.2 Estruturas (ids publicados pelo `AurenSceneBuilder`)

| id | Footprint do greybox L × P (m) | Altura greybox (m) |
|---|---|---|
| `casa_familia` | 10 × 9 | parede 3,0 |
| `casa_nilo`, `casa_sera` | 9 × 9 | parede 3,0 |
| `ferraria` | 11 × 9 | 5,0 |
| `ervanaria` | 9 × 8 | 4,0 |
| `posto_guarda` | 8 × 7 | 4,5 |

- Estado do greybox em 2026-09-29. A arte não pode passar do footprint: os percursos de navegação têm folga mínima de ~0,87 m (`AurenSceneBuilder`, nota de ESCALA). O mundo é adulto de propósito: porta 1,8 × 2,2 m, parede 3 m.
- Peças de kit modular: id com prefixo `modulo_` (ex.: `modulo_parede_taipa_3m`).

## 4. Orçamento por categoria — **HIPÓTESE v0**

Alvo (ADR-0006): **Android de faixa média, paisagem, 30 FPS (33,3 ms) de base**. O aparelho mínimo é pendência do idealizador (ADR-0006); até lá tudo aqui é **HIPÓTESE v0**, com classe suposta de GPU Adreno 610–619 / Mali-G57, 4 GB de RAM, tela 1080 × 2400. Substitui a hipótese de PC (GTX 1060, 60 FPS) da v0 do mesmo dia. Conformidade só existe medindo com `Scripts/Perf/PerfHud` (CSV com `fps_min_1s` e `temp_c`) na cena de Auren, **no aparelho**; número medido na build Windows não vale. Triângulo isolado não define desempenho.

| Categoria | Tris LOD0 (máx) | LOD exigido | Materiais (máx) | Textura, lado (máx, px) | Ossos deform (máx) |
|---|---|---|---|---|---|
| Avatar | 15 000 | não (câmera sempre perto) | 2 | 1024 | 75 |
| Npc | 8 000 | LOD1 ≤ 50%, LOD2 ≤ 25% | 2 | 1024 | 55 |
| Prop pequeno (maior dimensão < 1 m) | 500 | não | 1 | 256 | — |
| Prop médio (1 – 4 m) | 2 000 | LOD1 ≤ 50% | 1 | 512 | — |
| Estrutura (edifício inteiro) | 10 000 | LOD1 ≤ 50%, LOD2 ≤ 25% | 3 | 1024 | — |
| Estrutura `modulo_*` | 1 000 | não | 1 | 1024 (compartilhada em `Art/Shared/`) | — |
| Vfx (malha) | 300 | não | 1 | 512 (atlas de flipbook) | — |

- Espelho em código: `Editor/ArtImportValidatorRules.cs` (`orcAvatar` … `orcVfx`). Mudou aqui, muda lá no mesmo commit.
- Conta que fecha a tabela (quadro típico de Auren, pior caso sem culling): 1 avatar (15 000) + 4 NPCs em LOD0 (32 000) + 2 estruturas em LOD0 e 4 em LOD1 (40 000) + 20 props médios (40 000) + 40 props pequenos (20 000) + Vfx (≈ 2 000) ≈ 149 000 ≤ **150 000 tris visíveis por quadro**. Referência de ordem de grandeza: a demo Armies do guia Android roda ~210 000 tris por quadro a ~30 FPS [A1].
- LOD: cada nível corta 50% [A1]; por isso LOD2 = 25% também na estrutura. Transições (`LODGroup`, altura relativa na tela): LOD0→1 em 0,30; LOD1→2 em 0,12; culling em 0,03, mais cedo que no PC porque a resolução menor cria micro-triângulo antes (alvo: triângulo com ≥ 10 px de área [A1]). **HIPÓTESE v0.**
- Ossos: Npc 55 = o conjunto completo de §7.1 (19 obrigatórios + `UpperChest`, 2 `Toes`, 2 `Eye`, `Jaw`, 30 dedos), sem osso secundário; Avatar 75 deixa 20 para cabelo e roupa. Rig Humanoid custa 30–50% mais CPU que Generic [U4], e são vários NPCs animados ao mesmo tempo.
- Influências por vértice: ≤ 4 no importador (`maxBonesPerVertex: 4`, V16). Custo sobe acima de 4 [U3]; `QualitySettings.skinWeights = TwoBones` é alavanca de runtime do tier baixo, não regra de asset.
- Densidade de texel (orientação, sem regra de máquina): personagem ≈ 512 px/m; ambiente ≈ 256 px/m, com trim sheet repetido em `Art/Shared/`. **HIPÓTESE v0.**
- Placeholder atual: 204 tris, 19 ossos, 1 material.

### 4.1 O que o celular cobra e o PC não — **HIPÓTESE v0**

| Item | Meta | Base |
|---|---|---|
| FPS | **30 FPS (33,3 ms) sustentados** — HIPÓTESE v0; `fps_min_1s` ≥ 25 e sem queda por aquecimento em 20 min de sessão. 60 FPS é opção futura, não meta. | Unity renderiza celular a 30 FPS fixos por padrão, para poupar bateria e calor [U2] |
| Compressão de textura | ASTC (Player Settings › Android › Texture Compression Formats), mipmaps ligados (+33% de memória, menos banda [A2]). Bloco por mapa: `_basecolor` 5×5 (5,12 bpp) em Avatar e Npc, 6×6 (3,56 bpp) no resto, 8×8 (2 bpp) em prop pequeno; `_normal` 6×6; `_metallicsmoothness`, `_occlusion` e `_emission` 8×8; atlas de Vfx 6×6. ETC2 só se o aparelho mínimo não tiver ASTC. Vale no lugar da linha de compressão de §5 (que é de PC) para Android. | ASTC de 4×4 (8 bpp) a 12×12 (0,89 bpp); Adreno 4xx+ e Mali-T624+ suportam; sem suporte, o Unity descomprime para RGBA32 [U1]. 6×6 é o bloco mais usado em celular e já serve para normal map [R1]. Metálico/rugosidade podem ter metade da resolução da cor [A2] |
| Texturas por material | ≤ 4 amostradores: `_occlusion` e `_emission` só quando o asset pede | O guia Android cita ≤ 5 como teto e avisa que 5 já pode pesar [A3] |
| Draw calls por quadro | ≤ 200 (`Batches` no Stats) e ≤ 60 `SetPass` | ~500 por quadro é o teto da maioria dos celulares, segundo a Arm [R2]; a folga fica para a animação Humanoid e para o calor. SRP Batcher ligado no `URP_Base` agrupa material de mesmo shader e variante; por isso o teto de materiais da tabela |
| Overdraw e transparência (Vfx) | Opaco por padrão. Transparente só em Vfx (`Particles/Unlit`) e recorte só em cabelo e folhagem. ≤ 3 camadas transparentes no mesmo pixel no pico de um golpe (Rendering Debugger › Overdraw); partícula não cobre mais de ¼ da tela | Transparência custa mais em GPU de celular que em PC; evitar camadas sobrepostas; recorte × mistura se decide medindo [A3] |
| Sombras | O `URP_Base` já está afinado: só luz principal, 1 cascata, mapa 2048, 50 m, sem sombra suave nem de luz adicional. Regra de asset: projetam sombra Avatar, Npc, Estrutura e Prop médio; Prop pequeno e Vfx com `Cast Shadows = Off` | Sombra redesenha quem projeta e soma draw call [U4] |

Fontes (consultadas em 2026-09-29):
- [U1] Unity 6.3 Manual, *Choose a GPU texture format by platform*: https://docs.unity3d.com/6000.3/Documentation/Manual/texture-choose-format-by-platform.html
- [U2] Unity 6.3 Scripting API, `Application.targetFrameRate`: https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Application-targetFrameRate.html
- [U3] Unity 6.3 Scripting API, `QualitySettings.skinWeights`: https://docs.unity3d.com/6000.3/Documentation/ScriptReference/QualitySettings-skinWeights.html
- [U4] Unity, *Mobile game optimization tips, part 2*: https://unity.com/how-to/mobile-game-optimization-tips-part-2
- [A1] Android Developers, *Optimize geometry* (atualizado em 2026-02-26): https://developer.android.com/games/optimize/geometry
- [A2] Android Developers, *Textures* (2026-02-26): https://developer.android.com/games/optimize/textures
- [A3] Android Developers, *Materials and shaders* (2026-02-26): https://developer.android.com/games/optimize/materials
- [R1] Arm, *Yet more ASTC compression* (2023-04-24): https://developer.arm.com/community/arm-community-blogs/b/mobile-graphics-and-gaming-blog/posts/yet-more-astc-compression
- [R2] Arm Community, resposta de Peter Harris (Arm) em *Overhead of the driver*: https://community.arm.com/support-forums/f/graphics-gaming-and-vr-forum/48848/overhead-of-the-driver

As fontes dão direção (ASTC, 50% por LOD, teto de ~500 draw calls, 30 FPS). Os números de cada categoria são derivação deste documento e só viram regra depois de medir no aparelho mínimo.

## 5. Materiais URP Lit e texturas

- Shader: `Universal Render Pipeline/Lit`, workflow **Metallic**. Vfx também pode usar `Universal Render Pipeline/Particles/Unlit` ou `.../Particles/Lit`. Visual anime/toon é decisão futura de direção de arte; até lá, Lit.
- Material é `.mat` na pasta do asset (ou em `Art/Shared/`), nome `<id>_<parte>` (ex.: `borin_corpo`, `borin_cabelo`). Material embutido no FBX não vale para arte final.

| Mapa (sufixo do arquivo) | Slot URP Lit | Canais | Espaço de cor |
|---|---|---|---|
| `_basecolor` | Base Map | RGB cor; A opacidade (só se o material for recortado) | sRGB |
| `_normal` | Normal Map | tangente, convenção OpenGL (Y+), a do Unity | linear, tipo Normal map |
| `_metallicsmoothness` | Metallic Map | R metálico; A smoothness = 1 − roughness (Smoothness Source: Metallic Alpha) | linear |
| `_occlusion` | Occlusion Map | G | linear |
| `_emission` | Emission Map (opcional) | RGB | sRGB |

- Arquivo: `<id>_<parte>_<mapa>.png` (ex.: `borin_corpo_normal.png`). PNG 8 bits; lado potência de 2, quadrado ou 2:1.
- Tripo3D e Blender produzem roughness; o bake para `_metallicsmoothness` inverte o canal e grava em A.
- Compressão (importador Unity, PC): padrão do Unity (BC7/DXT5 para cor, BC5 para normal). Mipmaps ligados.

## 6. Nomes, IDs e pastas

- ID: ASCII, minúsculo, `snake_case`, regex `^[a-z][a-z0-9]*(_[a-z0-9]+)*$`, até 40 caracteres. ID publicado não muda (CLAUDE.md).
- NPC usa **exatamente** o id do `NpcCatalog`. Estrutura usa o id do `AurenSceneBuilder`. Avatar: `avatar_crianca5`, `avatar_crianca8`, `avatar_adolescente`, `avatar_adulto`.

```
client/Assets/_COE/Art/
  Humanoid/                  placeholder atual (HumanoidSetup.Dir) — legado, não mover
  Avatar/<id>/               <id>_model.fbx, texturas, .mat
  Npc/<id>/                  idem
  Anim/<base>/               clips compartilhados por base: <base>_idle.fbx, <base>_run.fbx ...
  Prop/<id>/
  Estrutura/<id>/
  Vfx/<id>/
  Shared/                    trim sheets e texturas de kit
arte/referencias/<assunto>/  concepts e referências (fora do Unity)
arte/fonte/<Categoria>/<id>/ masters: .blend, GLB do Tripo3D, PSD (fora do Unity, Git LFS)
```

- Todo arquivo da pasta começa com `<id>_` (ou é `<id>.<ext>`). Malha: `<id>_LOD0`, `<id>_LOD1`, ... (o importador do Unity cria o `LODGroup` sozinho a partir do sufixo `_LODn`). Colisão: `<id>_col`, `<id>_col_1`, ...
- Clip de humanoide: `<id ou base>_<clip>.fbx`, clip ∈ `idle, run, attack1, attack2, attack3, dodge, hit, death`. O `HumanoidMapping.Classify` casa por substring e trata o 1º arquivo com `model` no nome como modelo; por isso o id **não pode conter** `model` nem nenhuma chave de `HumanoidMapping.Keys` (`death, dying, die, dodge, roll, evade, dive, hit, impact, reaction, attack, slash, punch, kick, strike, combo, run, sprint, jog, idle`). Os 10 ids do `NpcCatalog` passam.

## 7. Rig Humanoid e clips

### 7.1 Ossos

Nomes = enum `HumanBodyBones` do Unity, que o auto-mapeamento Humanoid reconhece sem configuração (`HumanoidSetup` usa `avatarSetup = CreateFromThisModel`). Obrigatórios, os mesmos 19 do placeholder:

`Hips, Spine, Chest, Neck, Head, LeftShoulder, LeftUpperArm, LeftLowerArm, LeftHand, RightShoulder, RightUpperArm, RightLowerArm, RightHand, LeftUpperLeg, LeftLowerLeg, LeftFoot, RightUpperLeg, RightLowerLeg, RightFoot`

- 15 deles são os obrigatórios do próprio Unity; `Chest`, `Neck` e os dois `Shoulder` o COE exige a mais (deformação de ombro e pescoço).
- Opcionais aceitos: `UpperChest`, `LeftToes`/`RightToes`, `LeftEye`/`RightEye`, `Jaw` e os 30 dedos (`LeftThumbProximal`...). Nomes Mixamo (`mixamorig:Hips`, `mixamorig:LeftUpLeg`...) também passam no auto-mapeamento; o critério é a regra V15, não a grafia.
- Rig do Tripo3D (Auto Rig): nomenclatura não verificada. Entra se passar em V14–V17.
- Pose de repouso: **T-pose**, braços a ≤ 10° da horizontal, palmas para baixo, pés paralelos apontando para a frente.
- Hierarquia: `Hips` é a raiz do esqueleto, filha direta do objeto armature. Osso de controle/IK não vai para o FBX: marcar `Deform` desligado e exportar com `use_armature_deform_only=True` (§9).

### 7.2 Clips (contrato de `HumanoidMapping` / `HumanoidSetup`)

| Clip | Loop | Duração de referência a 30 FPS | Eventos (tempo normalizado) |
|---|---|---|---|
| Idle | sim | 75 quadros (2,5 s) | — |
| Run | sim | 21 (0,7 s) | `OnFootstep` 0,25 e 0,75 |
| Attack1 | não | 14 (0,47 s) | `OnHitFrame` 0,40 |
| Attack2 | não | 14 (0,47 s) | `OnHitFrame` 0,40 |
| Attack3 | não | 18 (0,6 s) | `OnHitFrame` 0,45 |
| Dodge | não | 12 (0,4 s) | `OnFootstep` 0,75; `OnDodgeEnd` 0,95 |
| Hit | não | 9 (0,3 s) | — |
| Death | não | 42 (1,4 s) | — |

- Um take por arquivo (`HumanoidSetup` lê só `takes[0]`). Os eventos são postos pelo `HumanoidSetup`; o FBX não precisa trazê-los.
- Deslocamento XZ do quadril é descartado (`applyRootMotion = false`; o código move). Altura do quadril é mantida na pose (`lockRootHeightY`): Death **cai** de verdade.
- Quem ataca é o **golpe da animação no tempo do evento**: mudar a duração muda o ritmo do combate. Duração fora de ±30% da referência gera WARN (V20).
- Conjunto por idade e clip de magia (`Skill`) ainda não existem: `docs/tech/DIVIDA_TECNICA.md` (T011/T013).

### 7.3 Placeholder infantil (estado medido em 2026-09-29)

Gerado por `client/tools/placeholder_humanoid.py` (Blender 5.2.1 headless), reimportado e medido no Blender:

- caixa da malha y (Unity) 0,000 – 1,100 m; x ±0,53 m; 204 tris; 19 ossos; 8 clips a 30 FPS;
- esquerda em +X no Blender (−X no Unity); ponta do pé para −Y no Blender (+Z no Unity);
- joelho da corrida flexiona para a frente; Death leva o quadril de 0,52 m a 0,08 m.

O placeholder é a **fixture positiva** do validador (deve passar como `avatar_crianca5` nas regras de escala, eixo, rig e clip). O placeholder adulto anterior (commit `889f2fd`, 1,74 m, pé virado para trás) é a **fixture negativa**: reprova V09, V11 e V13.

## 8. LOD e colisão

- LOD: malhas `<id>_LOD0..n` no mesmo FBX; razões e transições em §4. Avatar sem LOD. Personagem com LOD: mesmo esqueleto em todos os níveis.
- Colisão de personagem: **nenhuma malha**. A cápsula vem do código (`CharacterController`, altura = `BodyScale`); NPC idem.
- Prop e estrutura: preferir primitivas (Box/Capsule) criadas no prefab. Quando precisar de malha: `<id>_col`, convexa, ≤ 255 triângulos (limite de `MeshCollider` convexo), sem Renderer no prefab. Estrutura estática pode usar `MeshCollider` não convexo com malha simplificada ≤ 2 000 tris (**HIPÓTESE v0**).

## 9. Export Blender → FBX e import no Unity

Parâmetros de `bpy.ops.export_scene.fbx`: os do placeholder (que já passa pelo `HumanoidSetup`) mais `use_armature_deform_only=True`, que o placeholder dispensa porque todos os 19 ossos dele deformam:

```python
use_selection=True, object_types={"ARMATURE", "MESH"},
apply_unit_scale=True, global_scale=1.0, axis_forward="-Z", axis_up="Y",
add_leaf_bones=False, armature_nodetype="NULL", use_armature_deform_only=True,
bake_anim=<True só nos clips>, bake_anim_use_all_actions=False,
bake_anim_use_nla_strips=False, bake_anim_simplify_factor=0.0,
embed_textures=False, path_mode="COPY"
```

- Modelo (`_model.fbx`): malha + armature, sem animação, em T-pose. Clip: armature + malha, uma action.
- Import no Unity: `globalScale = 1`, `useFileScale = true` (nenhuma correção de escala no Unity; erro de escala se corrige no Blender). Humanoide: `animationType = Human`, `maxBonesPerVertex = 4`.
- Estático (Prop/Estrutura/Vfx): `bakeAxisConversion = true` no importador, para os nós saírem com rotação identidade. **HIPÓTESE v0**: confirmar no primeiro prop; o humanoide atual usa `false` e funciona.
- Gerar e medir o placeholder, da raiz do repo:
  `"C:\Program Files\Blender Foundation\Blender 5.2\blender.exe" --background --factory-startup --python client/tools/placeholder_humanoid.py -- client/Assets/_COE/Art/Humanoid`
  Com a variável de ambiente `COE_PREVIEW_PNG=<caminho.png>`, grava também um PNG de conferência (T-pose, corrida, ataque).
- Depois de regenerar os FBX: no Unity, `COE / Montar humanoide` (recria `Player.controller` e `PlayerModel.prefab`) e `COE / Gerar cena Bootstrap`.
- O FBX não é byte-idêntico entre exports (o Blender grava data de criação). Reprodutibilidade = mesma medição (§7.3), não mesmo hash.

## 10. Git LFS

- `.gitattributes` da raiz manda para o LFS: `*.fbx *.blend *.glb *.obj *.png *.jpg *.jpeg *.webp *.psd *.tga *.exr *.wav *.ogg *.ttf` (`*.glb` e `*.obj` já estão lá, conferido em 2026-09-30).
- `.meta` de todo asset vai no commit (CLAUDE.md). Arquivo de arte fora do LFS reprova V27.

## 11. Regras do validador de import

Escopo: cada pasta `Assets/_COE/Art/<Categoria>/<id>/`, Categoria ∈ {`Avatar`, `Npc`, `Prop`, `Estrutura`, `Vfx`}. `Anim/<base>/` roda só V18–V20. `Art/Humanoid/` (placeholder) roda como `avatar_crianca5` só as regras marcadas **[P]**.

Resultado por regra: `PASS`, `WARN`, `FAIL` ou `UNKNOWN` (sem evidência para medir). `UNKNOWN` nunca vira `PASS`. Severidade padrão: FAIL; exceções indicadas.

**Medidas comuns.** "Vértices" = vértices de todos os Renderers do LOD0 (sem `_col`), em espaço do GameObject raiz do modelo instanciado na origem, em pose de repouso: `MeshFilter` → `sharedMesh.vertices` × matriz local-para-raiz; `SkinnedMeshRenderer` → `BakeMesh` com o Animator desligado (pose de bind). `h = maxY − minY` desses vértices. `y(osso)` = `Animator.GetBoneTransform(HumanBodyBones.osso).position.y` com o modelo na origem, em repouso. Tris = soma de `GetIndexCount(submesh) / 3` dos submeshes de topologia Triangles.

### Identidade e arquivos
1. **V01** — Caminho da pasta casa `^Assets/_COE/Art/(Avatar|Npc|Prop|Estrutura|Vfx)/([a-z][a-z0-9]*(_[a-z0-9]+)*)/$` e o id tem ≤ 40 caracteres.
2. **V02** — Id conhecido: `Avatar` ∈ {`avatar_crianca5`, `avatar_crianca8`, `avatar_adolescente`, `avatar_adulto`}; `Npc` ∈ `NpcCatalog.Npcs[].Id`; `Estrutura` ∈ `AurenSceneBuilder.CasasAcessiveis` ∪ `AurenSceneBuilder.EstruturasPublicas` ou começa com `modulo_`; `Prop` e `Vfx`: qualquer id que passe V01 (não há catálogo).
3. **V03** — Todo arquivo da pasta, fora `.meta`, tem nome `<id>` ou `<id>_[a-z0-9_]+` e extensão ∈ {`.fbx`, `.png`, `.tga`, `.mat`, `.prefab`, `.controller`, `.anim`, `.asset`}. Nenhum `.blend`, `.glb`, `.gltf`, `.obj`, `.psd`, `.jpg`, `.jpeg`, `.webp` em `Assets/_COE/Art/**`. Exceção única: `.jpg`/`.jpeg` dentro de `Assets/_COE/Art/Prototipo/` (textura exportada pelo Tripo nos protótipos do ADR-0008, que não vão para a loja); master continua proibido ali.
4. **V04** — `docs/arte/PROVENIENCIA.md` tem o bloco `### <id>` com `g3:` = data `AAAA-MM-DD` e `licenca:` ∈ {`propria`, `tripo_pago`, `cc0`, `cc_by_4_0`, `outra`}. `tripo_free`, `desconhecida` ou vazio = FAIL. `cc_by_4_0` exige `atribuicao:` não vazio; `outra` exige `parecer:` não vazio.
5. **V05** — (`Avatar`, `Npc`, `Anim`) O id não contém `model` nem nenhuma chave de `HumanoidMapping.Keys` (substring, sem diferenciar maiúscula).

### Escala e transformação
6. **V06** [P] — Todo `ModelImporter` da pasta: `globalScale == 1` e `useFileScale == true`.
7. **V07** [P] — GameObject raiz do modelo importado: `localPosition == (0,0,0)` (±0,001 m), `localRotation` identidade (±0,01°), `localScale == (1,1,1)` (±0,0001).
8. **V08** [P] — Pés no chão: `minY` dos vértices ∈ [−0,02; +0,02] m.
9. **V09** [P] — `h` dentro da faixa: Avatar = tabela §3 pelo id; Npc = tabela §3.1 pelo id; Prop ∈ (0,02; 4,0] m; Estrutura ∈ [2,5; 15] m; Vfx sem regra.
10. **V10** [P] — Centro da caixa dos vértices: Avatar/Npc `|centroX| ≤ 0,10 m` e `|centroZ| ≤ 0,10 m`; Prop/Estrutura `|centroX|` e `|centroZ|` ≤ 5% da maior dimensão XZ.
11. **V11** [P] — Frente (Avatar/Npc): `position.x(LeftUpperLeg) < position.x(RightUpperLeg)`; e, entre os vértices com peso ≥ 0,5 em `LeftFoot` ou `LeftToes`, o mais distante do joint `LeftFoot` tem `z > z(LeftFoot)`.
12. **V12** — Estrutura com id do greybox: largura X ≤ largura do greybox e ≥ 90% dela; idem profundidade Z. `UNKNOWN` enquanto o `AurenSceneBuilder` não publicar os footprints como tabela (hoje são argumentos de `CasaAcessivel`/`EstruturaPublica`).

### Proporção (não escalonar)
13. **V13** [P] — `r_perna = (y(LeftUpperLeg) − minY) / h` e `r_cabeca = (maxY − y(Neck)) / h` dentro das faixas da base (§3). Avatar: FAIL. Npc: WARN (base pela tabela §3.1; adulto usa faixas de `avatar_adulto`, criança as de `avatar_crianca5`).

### Rig e animação
14. **V14** [P] — (Avatar/Npc) `animationType == Human`; Avatar gerado não nulo, `isValid` e `isHuman`.
15. **V15** [P] — `Animator.GetBoneTransform` não nulo para os 19 ossos de §7.1.
16. **V16** [P] — Ossos distintos somados dos `SkinnedMeshRenderer.bones` do LOD0 ≤ 75; `ModelImporter.maxBonesPerVertex ≤ 4`.
17. **V17** [P] — T-pose: ângulo entre o vetor `LeftUpperArm → LeftLowerArm` e o plano XZ ≤ 10°; idem direito. Severidade WARN.
18. **V18** [P] — Pasta com clips: `HumanoidMapping.Classify(nomes sem extensão)` devolve `Missing` vazio e `Unmapped` vazio.
19. **V19** [P] — Cada clip: `frameRate == 30` (WARN se diferente). Idle e Run: `loopTime == true` e, amostrando o clip em t = 0 e t = `length` sobre o modelo, a rotação local de cada um dos 19 ossos difere ≤ 2° (FAIL).
20. **V20** [P] — Duração de cada clip dentro de ±30% da referência de §7.2. Severidade WARN.

### Geometria, material e textura
21. **V21** [P] — Tris do LOD0 ≤ teto da categoria (§4). Prop: pequeno se a maior dimensão da caixa < 1,0 m, senão médio.
22. **V22** — Categoria com LOD exigido (§4): `LODGroup` na raiz com o número de níveis da tabela, e `tris(LODn) ≤ razão × tris(LOD0)`.
23. **V23** — Malha `<id>_col*`: sem Renderer habilitado no prefab; ≤ 255 tris se o `MeshCollider` for convexo, ≤ 2 000 se não. Avatar/Npc: nenhuma malha `_col`.
24. **V24** — Todo material de todo Renderer é `.mat` cujo caminho começa com a pasta do asset ou com `Assets/_COE/Art/Shared/`; shader `Universal Render Pipeline/Lit` (Vfx também `.../Particles/Unlit` e `.../Particles/Lit`); número de materiais distintos ≤ teto (§4).
25. **V25** — Toda textura referenciada pelos materiais: largura e altura da fonte (`TextureImporter.GetSourceTextureWidthAndHeight`) potências de 2 e ≤ teto da categoria; `maxTextureSize` ≤ teto; `mipmapEnabled == true`.
26. **V26** — Nome de textura termina em `_basecolor`, `_normal`, `_metallicsmoothness`, `_occlusion` ou `_emission`; `_basecolor` e `_emission` com `sRGBTexture == true`; `_normal` com `textureType == NormalMap`; `_metallicsmoothness` e `_occlusion` com `sRGBTexture == false`.

### Repositório
27. **V27** — (fora do Unity: script de pre-commit ou CI) `git check-attr filter -- <arquivo>` devolve `lfs` para todo `.fbx`, `.png` e `.tga` da pasta.

**Prova do validador.** Checagem que não fica vermelha não é checagem. `Tests/Editor/ArtImportValidatorTests.cs` roda a fixture positiva (placeholder infantil atual, §7.3, validado como `avatar_crianca5`) esperando PASS em V06–V11 e V13–V21 e nenhuma regra fora das [P]; e a negativa com o **mesmo** placeholder: validado como `avatar_adulto`, FAIL em V09 (h = 1,10 m fora de 1,698–1,838) e em V13 (r_perna 0,473 e r_cabeca 0,218 fora das faixas adultas); para V11, as medidas do placeholder giradas 180° em Y (de costas) e espelhadas em Z (pé para trás) entram na regra pura e dão FAIL nas duas, com a medida original como controle (PASS). O placeholder adulto de `889f2fd` deixou de ser a negativa: exigiria `git show`/LFS e reimport dentro do teste para provar as mesmas três regras. Código: `Editor/ArtImportValidator.cs` (medição no Unity) e `Editor/ArtImportValidatorRules.cs` (tabela única de números e regras puras). Menu `COE / Validar arte`; batch, de `client/`: `Unity -batchmode -projectPath . -executeMethod COE.EditorTools.ArtImportValidator.RunBatch -logFile -` (sai com 1 se houver FAIL; UNKNOWN e WARN não reprovam).

## 12. Pendências

1. Aparelho mínimo de referência (pendência do ADR-0006, decisão do idealizador). Sem ele, §4, §4.1 e a meta de 30 FPS são hipótese. Definido o aparelho: medir Auren com `PerfHud` e recalibrar tris por quadro, draw calls e blocos ASTC.
2. ~~`*.glb` e `*.obj` no `.gitattributes`~~: feito (conferido em 2026-09-30).
3. `HumanoidSetup` monta um único humanoide, de `Art/Humanoid/`. Para `Avatar/<id>/`, `Npc/<id>/` e `Anim/<base>/`, precisa aceitar pasta por parâmetro (C#, T011/T013).
4. `AurenSceneBuilder` expor os footprints das estruturas como tabela pública (destrava V12).
5. Altura da base adolescente em `BodyScale` quando ela entrar no escopo (hoje 1,60 m é hipótese).
6. Calibrar as faixas de V09 e V13 no primeiro modelo real; registrar aqui a mudança.
7. Validador (§11): compilado e rodado no EditMode em 2026-09-29 pelo coordenador; a única falha é a fixture positiva em V15 (`GetBoneTransform` nulo para `Chest`, `LeftShoulder`, `RightShoulder`). É defeito do avatar, não da regra: o auto-mapeamento de `HumanoidSetup.ImportModel` (`CreateFromThisModel` sem `humanDescription`) deixa esses três ossos de fora na proporção infantil; correção = mapeamento explícito por nome e remontar o avatar. Enquanto V15 falhar, V19 do placeholder dá UNKNOWN (o loop é medido nos 19 ossos). Se V19 seguir UNKNOWN depois disso, `AnimationClip.SampleAnimation` não amostra humanoide fora do Play Mode: trocar por `AnimationMode.SampleAnimationClip`.
8. V27 não roda no Unity (fica UNKNOWN no relatório): falta o hook de pre-commit ou CI com `git check-attr filter`.
9. §7.3 ainda cita o placeholder adulto de `889f2fd` como fixture negativa; a prova vigente é a de §11.
10. Leitura adotada pelo validador onde §11 é ambígua: `Anim/<base>/` roda V05 além de V18–V20 (V05 cita `Anim`); V19 mede o loop em `Art/Avatar/<base>/`; V23 lê o `.prefab` da pasta (sem prefab, o FBX); V24–V26 leem os materiais dos Renderers do FBX, fora `_col`; o modelo é o `.fbx` com `model` no nome (Prop/Estrutura/Vfx aceitam também o único `.fbx` da pasta).
11. §5 ("Compressão (importador Unity, PC)") ainda descreve PC; para Android vale §4.1 (ASTC por tipo de mapa). Alinhar §5 e decidir se o validador confere o override de Android do `TextureImporter`.
12. `ProjectSetup.AplicarAndroid` não fixa o formato de compressão de textura: fixar ASTC por script, para não depender do padrão da versão do Unity.
13. `URP_Base`: distância de sombra 50 m e mapa 2048 são mais do que a vila pede com a câmera a ~2,75 m da criança. Rever (HIPÓTESE: ~30 m, 1024) só depois de medir no aparelho mínimo.

## 13. Protótipo de estética (ADR-0008)

Exceção marcada ao portão do ADR-0002: malha do Tripo3D entra **sem** G1/G2/G3, com bloco `estado: PROTOTIPO` em `PROVENIENCIA.md`, fora de `Art/<Categoria>/` (o validador de §11 não a vê) e nunca em build de loja.

- **Onde o FBX entra:** `client/Assets/_COE/Art/Prototipo/Personagens/<id>/<id>.fbx` (`protagonista`, `nilo`, `sera`, `mara`, `borin`) e `.../Pecas/<id>/<id>.fbx` (`casa_familia`, `ferraria`, `poco`, `arvore`, `barril`, `caixote`, `cesto`, `lanterna`, `arbusto`, `simbolo_limiar`, `bigorna`, `banco`). Textura embutida no FBX ou `.png` ao lado. Qualquer um pode faltar: sem o arquivo, a cena gera o greybox de sempre.
- **Import** (`Editor/PrototipoImport.cs`, automático): personagem Humanoid com avatar do próprio modelo; se o rig não mapear, aviso no Console e cai para Generic (modelo parado). Peça sem rig, eixo assado. Sem câmera, luz nem colisor; malha não legível e comprimida; todo material vira `COE/Toon` com a textura/cor base do FBX.
- **Escala** (`Editor/Prototipos.cs`): mede a caixa da malha, escala para a altura-alvo e apoia a base no chão (pivô na base, centrado). Casa, ferraria, poço e barril também não passam da planta do greybox. Colisão é sempre a do greybox (renderer desligado) ou uma caixa nas peças soltas.

| id | Altura-alvo (m) | Onde entra |
|---|---|---|
| `protagonista` | 1,10 (`BodyScale.Crianca5`; o `BodyByAge` escala aos 8) | filho do Player, com `Player.controller` |
| `nilo`, `sera` | 1,10 (crescem com a cápsula do `NpcActor`) | sob `NPCs/<id>/Corpo` |
| `mara` / `borin` | 1,75 / 1,82 | idem; demais NPCs ficam cápsula com cor chapada própria |
| `casa_familia` / `ferraria` | 6 (planta 10 × 9 / 11 × 9) | no lugar da caixa do greybox |
| `poco` | 2,6 (planta 3,2) | no lugar do cilindro da praça |
| `arvore` | 7 (± 15%, giro variado) | árvores mais perto da entrada do bosque, até 60 000 tris somados |
| `barril` 1 · `caixote` 0,6 · `banco` 0,5 · `bigorna` 0,7 · `lanterna` 0,5 · `cesto` 0,35 · `arbusto` 1 | — | barris no lugar dos cilindros; o resto pela tabela `Prototipos.Soltas`, a ≥ 1,5 m de todo percurso do T008 |
| `simbolo_limiar` | 1,5 | ao lado do gatilho do salto (o `SaltoGatilho` fica no objeto do greybox) |

- **Look** (`Editor/LookSetup.cs`, `Art/Look/COE_Toon.shader`, `Art/Look/COE_Ceu.shader`): toon com 2–3 faixas, sombra tingida, rim leve, contorno por casco invertido (passe `SRPDefaultUnlit`, desligado no chão, rua e pedra), céu em gradiente, fog linear 30–150 m na cor do horizonte, sol quente, ambiente trilight, pós de um passe (saturação, contraste, vinheta; sem bloom nem tonemapping). Todo material dos geradores vira toon, inclusive os `.mat` URP/Lit que já existiam. Custo no aparelho: **não medido** (HIPÓTESE, §4.1).
- **Regerar** (Editor fechado, de `client/`, nesta ordem): `-executeMethod COE.EditorTools.PrototipoImport.Reimportar`, depois `COE.EditorTools.BootstrapSceneBuilder.Build`, depois `COE.EditorTools.AurenSceneBuilder.Build`. No Editor: menus `COE / Reimportar prototipos`, `COE / Gerar cena Bootstrap`, `COE / Gerar cena Auren`.
