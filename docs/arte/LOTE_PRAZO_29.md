# Lote do prazo — Tripo3D até 2026-11-29

As duas contas do Tripo3D (A e B) vencem em **2026-11-29**. Este lote gasta os créditos agora e gera o máximo de malha do COE, para corrigir lógica depois em vez de parar por falta de arte. A regra que libera isso está no [ADR-0011](../adr/ADR-0011-geracao-antecipada-pelo-prazo-do-tripo.md): **gerar agora, aprovar depois.** Nada daqui entra em `client/Assets/` antes do G2/G3.

## 0. Regras que não mudam (leia uma vez)

1. **Privado sempre.** Toda geração marcada como Privada (no plano gratuito o output é do Tripo; depois do dia 29 não se sabe se o direito sobrevive: PROVENIENCIA §2, itens 1 e 2).
2. **Baixar no mesmo dia, tudo.** GLB (com textura) de cada malha e o PNG de cada concept. Depois do dia 29 a conta pode perder acesso ao histórico. Último download até **29/11 às 20h**.
3. **Onde salvar (no PC, fora do git):** `chronicles-of-existence\arte\fonte\_antecipado\<id>\` com o nome `<id>_<tipo>_v01.glb` (`v02`… nunca sobrescreve). Essa pasta está no `.gitignore`: o repositório é **público** e o LFS gratuito tem 1 GB, então bruto do Tripo não vai para o GitHub.
4. **Uma linha por geração** em [`LOTE_PRAZO_29.csv`](LOTE_PRAZO_29.csv): `data,conta,id,tipo,task_id,creditos,arquivo,obs`. Isso é a proveniência mínima; o bloco completo da PROVENIENCIA se escreve no G3. Ledger do time: `python .claude/scripts/tj_tripo_ledger.py add --conta <A|B> --creditos <n> --task <id> --asset <id>` (no repo `time-jogos`) quando der; o CSV basta.
5. **Conta A até zerar, depois B** é a regra do time; aqui as duas rodam **em paralelo** porque o tempo acaba antes do crédito. Raia A = agente no Chrome do PC HOUSE; raia B = você.
6. **Encher a fila.** O Tripo aceita várias tarefas ao mesmo tempo: mande 5 a 10 de uma vez, baixe as prontas, mande mais. Esperar uma por uma é o que mais atrasa.
7. **Créditos:** HD ≈ 45, Auto Rig ≈ 20, imagem de concept ≈ 0 (estimativas do `ASSETS.md`; anote o valor real que a tela mostrar). Saldo A em 19/11: 25.145. Saldo B: anotar no CSV na primeira linha.

## 1. Configuração por tipo

| Tipo | Onde no Tripo | Opções |
|---|---|---|
| `concept` | Imagem (GPT Image 2) | quadrado 1:1, 2 a 4 variações, fica com a que mostra as 3 formas da ficha |
| `edit` | Imagem → editar a partir de um PNG | mesmo enquadramento; usado para tirar o objeto do corpo |
| `malha` | Imagem para 3D (ou multivista, se houver frente/perfil/costas) | HD, textura ligada, PBR ligado, 8K desligado, privado |
| `texto3d` | Texto para 3D | HD, textura ligada, privado; 2 variações em peça-chave |
| `rig` | Auto Rig, humanoide | só nos corpos sem objeto (objeto é peça própria presa a osso) |

## 2. Blocos de estilo

**Personagem:** o bloco "Estilo + formato" do [`PROMPTS_G2.md`](PROMPTS_G2.md) (linha 18), sempre, com o bloco do personagem embaixo e a linha `MOST IMPORTANT, ALL THREE MUST BE VISIBLE…` da seção "Reforço do lote 1".

**Cenário e prop (`ESTILO_CENARIO`), cole antes de cada prompt da raia B:**

```text
Original game asset for a stylized anime fantasy mobile game: cel-shaded toon look, flat hand-painted colors with soft two-tone shading, chunky readable shapes, low surface noise, no tiny details. Medieval village of Auren: terracotta (#A86D52) roof tiles and plaster, warm ivory (#E9DEC6) lime-washed walls, weathered brown timber, wattle-and-daub, grey fieldstone, dark iron fittings, warm green (#648B67) for plants. ONE object only, centered, upright, no ground plane, no other objects, no people, no text, no logo. Real-world scale given in meters. Do not copy any existing game, film or artist.
```

**Bosque (`ESTILO_BOSQUE`):** o mesmo bloco, trocando a segunda frase por: `The Whispering Woods: muted deep greens, mossy slate-grey stone, dry brown leaves, pale thorny branches, soft cool shadows; nothing glows.`

## 3. Raia A — agente, conta A (personagens e criaturas)

Ordem é prioridade. Personagem: `concept` (ou `edit`) → `malha` → `rig`. Objeto: `malha` a partir de um `edit` que deixa só o objeto, ou `texto3d`.

| # | id | passos | entrada / prompt | créd. |
|---|---|---|---|---|
| A01 | `borin` | edit → malha → rig | `arte/referencias/concepts_g2/borin_frente_v01.png` + `Same character, same T-pose, same framing and colors. Remove the iron ring and its bracket from the belt completely; keep the belt plain. Right arm in the thick padded leather sleeve must be TWICE the volume of the left arm.` | 65 |
| A02 | `borin.aro_provas` | texto3d ×2 | `A CLOSED ring 0.28 m wide made of a flat dark iron band 4 cm wide, held by a rigid bent flat iron bracket 15 cm long; small flat iron tags hang inside the ring and one flat notched iron plate 18 x 6 cm with 5 notches. Blacksmith's test ring, hand-forged, stylized toon.` | 90 |
| A03 | `daren` | edit → malha → rig | `daren_frente_v01.png` + `Same character, same T-pose. Remove the carrying pole, the crate, the tall case and all straps; empty hands, plain shoulders.` | 65 |
| A04 | `daren.vara_carga` | texto3d | `Carrying pole 1.90 m long, 5 cm thick, smooth worn wood; from the LEFT end hangs a flat wide low wooden crate (horizontal box) on a short wide flat leather strap 6 cm wide; from the RIGHT end hangs a tall narrow dark leather case standing upright. Stylized toon.` | 45 |
| A05 | `tovin` | edit → malha → rig | `tovin_frente_v01.png` + `Same character, same T-pose. Remove the horn from his back and the stake case from his shin; keep the funnel leather leggings.` | 65 |
| A06 | `tovin.chifre` | texto3d ×2 | `A big ivory ox horn used as a signal horn: strongly CURVED cone, wide open mouth flaring outward, wrapped with three terracotta (#A86D52) leather bindings and a shoulder strap. Not a straight tube, not a viking horn. Stylized toon.` | 90 |
| A07 | `tovin.estojo_estacas` | texto3d | `Short hard dark leather case 35 cm tall worn on the shin, holding five pale wooden stakes with flat heads fanning out above it.` | 45 |
| A08 | `maelis` | edit → malha → rig | `maelis_frente_v01.png` + `Same character, same T-pose. Remove the writing board and the staff; keep the big round cloud of curly grey hair and the ink-grey felt tabard.` | 65 |
| A09 | `maelis.prancha` | texto3d | `Flat wooden writing board 0.60 x 0.35 m worn like a tray from two neck straps, with a closed book and one upright ivory sheet clipped on it.` | 45 |
| A10 | `maelis.vara_oficio` | texto3d | `Short dark wooden office staff 1.0 m, plain, iron cap at the top, not a walking stick.` | 45 |
| A11 | `avatar_crianca5` | concept → malha → rig | bloco `avatar` do PROMPTS_G2 (linha 175) **sem** a manta (corpo) e de novo **com** a manta (referência) | 65 |
| A12 | `avatar.manta` | edit do concept com manta → malha | `Only the birth blanket: the horizontal bed-roll across the shoulder blades and the bell-shaped blanket to the knee, no child, same colors.` | 45 |
| A13 | `avatar.espada_madeira` | texto3d ×2 | `Child's wooden practice sword, total 0.62 m, wide flat blade with rounded tip, simple cross-guard, grip wrapped in linen, carved by hand, light wood.` | 90 |
| A14 | `nilo_crianca5` | concept → malha → rig | bloco `nilo` (linha 111); a forquilha sai como peça própria (A15) | 65 |
| A15 | `nilo.forquilha` | texto3d | `Forked Y-shaped hazel branch 0.75 m, bark partly peeled, used as a reaching stick.` | 45 |
| A16 | `sera_crianca5` | concept → malha → rig | bloco `sera` (linha 128) | 65 |
| A17 | `lysa` | concept → malha → rig | bloco `lysa` (linha 66); chapéu-varal separado em A18 | 65 |
| A18 | `lysa.chapeu_secar` | edit → malha | `Only the hat: brim 0.72 m wide with a 5 cm thick edge, bundles of drying herbs hanging from the brim, no person.` | 45 |
| A19 | `eira` | concept → malha → rig | bloco `eira` (linha 96); lousa e lanterna separadas | 65 |
| A20 | `eira.lousa` + `eira.lanterna` | texto3d | `Slate writing board 0.45 x 0.60 m in a wooden frame with a leather carry strap` · `Small iron hand lantern with horn panes, warm candle` | 90 |
| A21 | `oren` | concept → malha → rig | bloco `oren` (linha 145); torre de cestos separada | 65 |
| A22 | `oren.torre_cestos` | edit → malha | `Only the tower of stacked woven baskets 0.60 m tall on a back frame, with the small pennant on the left, no person.` | 45 |
| A23 | `aethron` | concept → malha → rig | bloco `aethron` (linha 192); umbral e soleira separados | 65 |
| A24 | `aethron.umbral` + `aethron.soleira` | edit → malha | só a canga com o batente em Π · só a laje de pedra com o vão dourado | 90 |
| A25 | `simbolo_limiar` | concept ×2 (antes e eco) → malha | bloco `simbolo_limiar` (linha 207). A malha é referência: o jogo usa shader | 45 |
| A26 | `abafador` (fechado e aberto) | concept ×2 → malha ×2 | ficha `abafador` C3/C4 + `ESTILO_BOSQUE`: `Creature: a flattened mound of dry brown leaves 0.45 x 0.30 m, no legs, no head; a fan of leaves folded on its back. Second image: the fan OPEN as a full semicircle 0.70 x 0.50 m standing behind the mound, 7 radial ribs with 3 cm points, two ivory eye-dots at the base of the fan. Side view.` | 90 |
| A27 | `feixe_de_espinho` (fechado e tripé) | concept ×2 → malha ×2 | `Creature disguised as a thorn bush: vertical spindle of pale thorny branches 1.30 m, tied by a root knot at 0.85 m. Second image: the lower branches open into a TRIPOD 1.80 m tall with a ball of roots 0.30 m hanging in the middle, crown of pale thorns raised. Front view.` | 90 |
| A28 | `lajeiro` | concept → malha → rig quadrúpede (manual) | `Low wide stone quadruped like a table: body 1.80 m long, 1.00 m tall, belly flat at 0.35 m; back is a staircase of five flat slate slabs from head to tail, the front slab is a visor hiding the head; short wide pillar legs; wet slate grey with green moss in the joints. Side view.` | 45 |
| A29 | `enredado` | edit do concept do lajeiro → malha | `The same creature twice as large (2.0 m tall, 3.5 m long), slabs thicker and cracked, moss in hand-wide joints; no ring, no light (the ring is a shader).` | 45 |
| A30 | `mara` v03 | concept → malha | bloco `mara` + reforço v02. **Risco:** G2 reprovado duas vezes; gera para não perder o crédito, a ficha segue aberta | 65 |
| A31 | `feirante_1..3`, `parceiro_treino` | concept → malha → rig | adultos neutros de Auren (linho, lã, avental de feira), **sem** objeto-assinatura e sem as zonas do `ELENCO.md`; servem de figurante | 195 |

Subtotal A ≈ 2.200 créditos. Sobrando tempo: versão aos 8 de `avatar`, `nilo` e `sera` (mesma ficha, 1,28 m) e uma segunda variação de cada corpo.

## 4. Raia B — você, conta B (cenário, props e bosque)

Tudo `texto3d`: cole `ESTILO_CENARIO` (ou `ESTILO_BOSQUE`) e a linha da tabela embaixo. Gere 2 variações das estruturas.

| # | id | prompt (depois do bloco de estilo) | créd. |
|---|---|---|---|
| B01 | `casa_familia` | `Two-room family cottage, footprint 10 x 9 m, walls 3.0 m, steep terracotta tile roof, ivory lime-washed wattle-and-daub walls on a fieldstone base, timber frame, one wooden door 2.2 m, two shuttered windows, stone chimney.` | 90 |
| B02 | `casa_nilo` | `Small village house 9 x 9 m, walls 3.0 m, thatched roof, plaster walls stained by weather, lean-to woodshed on one side.` | 90 |
| B03 | `casa_sera` | `Neat village house 9 x 9 m, walls 3.0 m, terracotta roof, flower boxes under the windows, small porch with a bench.` | 90 |
| B04 | `ferraria` | `Village smithy 11 x 9 m, 5.0 m tall, open front under a heavy timber roof, big stone forge with a wide chimney, anvil area, tool rack on the wall.` | 90 |
| B05 | `ervanaria` | `Herbalist's house 9 x 8 m, 4.0 m tall, bundles of herbs drying under the eaves, round window, small walled herb garden at the door.` | 90 |
| B06 | `posto_guarda` | `Watch post at the edge of the woods, 8 x 7 m, 4.5 m tall, stone ground floor with a timber lookout on top, a horn hook by the door, no flags.` | 90 |
| B07 | `casa_fundo_a..d` | quatro casas de fundo diferentes, 7 x 6 m, paredes 2,8 m, mais simples (cobrem as 16 `casa_fundo_*` por rotação e cor) | 180 |
| B08 | `modulo_parede_taipa` | `Modular wall segment 4 m wide, 3 m tall, wattle-and-daub with timber frame, tileable on both sides.` | 45 |
| B09 | `modulo_telhado` | `Modular terracotta tile roof segment 4 m wide, 45-degree slope.` | 45 |
| B10 | `modulo_cerca` | `Modular wooden village fence segment 3 m long, 1.1 m tall, split rails and posts.` | 45 |
| B11 | `modulo_muro_pedra` | `Modular dry-stone wall segment 3 m long, 1.0 m tall.` | 45 |
| B12 | `portao_sul` | `Village gate 5 m wide, 4 m tall, two timber posts with a roofed lintel, open double wooden doors.` | 45 |
| B13 | `poco` | `Village stone well 1.6 m wide with a wooden roof, crank and bucket.` | 45 |
| B14 | `bigorna` | `Blacksmith anvil on a tree-stump base, 0.9 m tall.` | 45 |
| B15 | `barril` + `caixote` | `Wooden barrel 0.9 m with iron hoops` · `Wooden crate 0.6 m cube, nailed planks` | 90 |
| B16 | `cesto` + `banco` | `Woven wicker basket 0.5 m with handles` · `Rustic wooden bench 1.6 m` | 90 |
| B17 | `lanterna` | `Village street lantern on a wooden post 2.4 m, iron cage with horn panes.` | 45 |
| B18 | `mural_avisos` | `Village notice board 2 m wide, small shingled roof, blank pinned papers (no text).` | 45 |
| B19 | `barraca_feira` ×3 | `Market stall 3 x 2 m with striped cloth awning`: frutas · tecidos · cerâmica | 135 |
| B20 | `carroca` | `Two-wheeled wooden hand cart 2.2 m with a few sacks.` | 45 |
| B21 | `galinheiro` + `varal` | `Small wooden chicken coop 2 x 1.5 m on stilts` · `Clothesline between two posts 4 m with linen hanging` | 90 |
| B22 | `treino_boneco` | `Straw training dummy on a post, 1.4 m, wrapped in burlap and rope.` | 45 |
| B23 | `arvore_a..c` | `Broadleaf village tree 7 m` · `Old oak 9 m with a wide crown` · `Young birch 5 m` | 135 |
| B24 | `arbusto_a..b` | `Round bush 1.2 m` · `Flowering hedge 2 m long` | 90 |
| B25 | `bosque_tronco_caido` | (`ESTILO_BOSQUE`) `Fallen mossy tree trunk 5 m long.` | 45 |
| B26 | `bosque_rocha` ×2 | `Mossy slate boulder 1.5 m` · `Flat slate slab outcrop 3 m` | 90 |
| B27 | `bosque_raiz` + `bosque_espinheiro` | `Big twisted root arch 2.5 m` · `Dense pale thorn hedge segment 3 m long, 1.8 m tall` | 90 |
| B28 | `pedra_limiar` | `Waist-high standing stone 1.1 m, worn, with a shallow ring-shaped groove (no glyphs, no glow).` | 45 |
| B29 | `festa_mesa` + `festa_fogueira` | `Long trestle feast table 3 m with bowls and bread` · `Village bonfire pit 1.5 m with stacked logs` (q14) | 90 |
| B30 | `aro_provas_suporte` | `Wooden rack 1.5 m holding iron test rings and tongs, for the smithy wall.` | 45 |

Subtotal B ≈ 2.500 créditos.

## 5. O que sobra de crédito

COE inteiro (A + B, com variações) fica perto de 5.000. Se as contas tiverem mais que isso, o resto vai para os outros jogos do estúdio (Dungeon Courier, Forge Street, ARKANA), cada um com a fila própria. Crédito parado no dia 29 é crédito perdido.

## 6. Depois do dia 29 (sem pressa)

1. `tj_process_asset.py` (Blender batch, LOD0–2, textura 1024) por pasta de `_antecipado`.
2. G2 dos personagens sem concept aprovado (folha 2: Oren, Lysa, Eira, Aethron; folha 3: crianças) com as imagens deste lote.
3. G3 com objetos presos a osso, PROVENIENCIA completa e só então `client/Assets/_COE/Art/`.
