# Prompts de concept para o G2 (bíblia v1.1, só o slice)

- **Para quê:** gerar o concept de cada personagem com ficha G1 aprovada, no formato que o teste cego de silhueta (`client/tools/silhueta.py`) e o Tripo (imagem → 3D) precisam. Substitui, para o elenco do slice, as fichas de prompt da bíblia v1.0 (`arte/referencias/acervo/documentos/`), que descreviam só o cargo.
- **Quem gera:** o idealizador, no Tripo Studio (Imagem, GPT Image 2), na conta do plano Max. Um agente não gera: gastar créditos pede a mão dele.
- **Ordem:** só depois do G1 aprovado na ficha (`docs/arte/fichas/<id>.md`, campo "estado").

## Antes de gerar
1. No Tripo, marcar a geração como **privada**.
2. Colar o bloco **Estilo + formato** e, embaixo, o bloco do personagem.
3. Gerar 2 a 4 variações; ficar com a que mostrar as 3 formas da ficha (C3) mais claras.
   - **Formato: quadrado (1:1), nunca em pé.** A figura ocupa ~80% da altura da imagem. Em 2:3 em pé o aro do Borin cai abaixo do corte de 7 px do `silhueta.py` e some (reconferência do Art Director, 2026-10-03; mínimos: Borin ≥ 62% da altura, Maelis ≥ 57%, Daren e Nilo ≥ 50%).
4. Baixar o PNG para `arte/referencias/concepts_g2/<id>_frente_v01.png` (versões seguintes `v02`...; nunca sobrescrever).
5. Anotar a data e o id da tarefa do Tripo; o coordenador abre o bloco `### <id>` na `PROVENIENCIA.md`.

## Bloco "Estilo + formato" (igual para todos)

```text
Original character concept for a stylized anime fantasy game (cel-shaded / toon look: flat colors, soft two-tone shading, thin clean outline; medieval village, believable linen, wool, leather and iron). ONE character only, full body, FRONT view, orthographic, standing in a strict T-pose (arms straight out horizontally, palms down, legs slightly apart, feet pointing forward). Plain flat uniform light cream background, no floor, no cast shadow, no other objects, no text, no logo, no watermark. Even studio light. SQUARE 1:1 image. The whole body fits inside the frame with a small margin above the head and below the feet, and fills about 80% of the image height. Readable silhouette first: the three named shapes below must be clearly visible against the background. Do not copy any existing anime, game or artist.
```

## borin — G1 aprovado por delegação (ADR-0010, 2026-10-03)

Ficha: [`fichas/borin.md`](fichas/borin.md). Altura de referência 1,82 m.

```text
CHARACTER: Borin, the village blacksmith, about 50 years old. Tall and lean (1.82 m), narrow torso, long arms. Long neck that juts forward, shaved head, clean-shaven, small short leather visor on the forehead.
SHAPE 1 — asymmetric arms: his RIGHT arm is wrapped from shoulder to glove in a thick padded leather forge sleeve, about twice the volume of the left arm; his LEFT arm is thin and bare to the elbow, with a rolled-up linen sleeve.
SHAPE 2 — a CLOSED ring, 28 cm wide, made of a flat dark iron band 4 cm wide whose face points to the viewer, held by a rigid bent flat iron bracket about 15 cm long, 5 cm wide with its face to the viewer, coming out of the left side of the belt, so the ring sits 10 cm OUT from the left thigh, clearly outside the body outline, with background visible between ring and thigh. Small flat iron tags hang inside the ring, plus one flat notched iron plate (a measuring gauge, 18 x 6 cm, with 5 notches).
SHAPE 3 — the shaved skull on the long forward neck, giving a hooked, leaning-forward head line.
Clothes: undyed ivory linen shirt; leather apron ONLY from the waist down, split into two flaps, terracotta brown leather (#A86D52); dark brown trousers; plain work boots. Dark neutral iron for metal.
AVOID: beard, barrel chest, broad heroic build, full bib apron, hammer or anvil as accessory, any turquoise, gold or violet color, any open or broken circle shape.
```

Depois de gerar, conferir na imagem as 3 formas e os itens do "Para o G2" da ficha (§5): o aro fora do contorno, ligado ao cinto por um suporte de 5 cm de face (sem ele, o `silhueta.py` descarta o aro); a assimetria dos braços; aro fechado e sem as cores reservadas. A forma 3 (crânio sobre pescoço projetado) só se prova de perfil: pedir também a vista de perfil (outra geração). *(ajuste de 2026-10-03, W3)*

## mara — G1 aprovado por delegação (ADR-0010, 2026-10-03)

Ficha: [`fichas/mara.md`](fichas/mara.md). Altura de referência 1,80 m (nada passa da cabeça).

```text
CHARACTER: Mara, the adult woman who runs the child's household in the village. Tall (1.80 m), straight legs, wearing simple strap shoes. A plain flat cloth tight to the skull, no volume. NOTHING above the head.
SHAPE 1 — blanket collar: the house blanket, rolled into a thick roll 12 cm in diameter, lies over BOTH shoulders, crosses on the chest and its ends are tucked into the sash. The neck disappears completely; the shoulders become round and about 8 cm wider on each side.
SHAPE 2 — bell sleeves: short sleeves of the same wool, flaring like a bell from shoulder to elbow; the mouth at the elbow is 24 cm wide over a bare forearm only 8 cm wide. In T-pose each arm is thick down to the elbow and then thins abruptly: a clear step at the elbow, identical on both arms.
SHAPE 3 — torn diagonal hem: a skirt of the same cloth wrapped at the waist over trousers, its hem cut on a steep diagonal: down to mid-shin on HER RIGHT side (35 cm above the ground, viewer's left) and only to mid-thigh on her left side (70 cm above the ground, viewer's right). Front view: one slanted cut crossing both legs; her right leg hidden down to the shin, her left leg fully visible from mid-thigh down. The skirt is no wider than the hips.
Clothes: blanket roll, sleeves and skirt all in the same terracotta wool (#A86D52); plain tunic underneath; trousers and head cloth in neutral earth brown.
AVOID: long dress, triangle-dress silhouette, apron, braid or long hair, puffed sleeves, pastel colors, sage green, visible neck, anything carried on the head, barefoot, ivory jumpsuit, mystical glow, any turquoise, gold or violet color.
```

Conferir na imagem: nada passa do topo da cabeça e o rolo esconde o pescoço sem rasgar na axila em T-pose; os sinos leem no cotovelo, não no pulso; a barra lê como corte inclinado, e não como avental reto (se não ler, C3 volta).

## daren — G1 aprovado por delegação (ADR-0010, 2026-10-03)

Ficha: [`fichas/daren.md`](fichas/daren.md). Altura de referência 1,70 m (nada passa da cabeça; com a vara e a caixa a figura tem ~2,1 m de largura, e o enquadramento tem de caber a largura inteira).

```text
CHARACTER: Daren, the child's father, who carries the household's goods to the south gate. Ordinary build (1.70 m), clean-shaven, short hair, visible neck, no broad chest. Nothing above the head. The whole pole and both hanging loads fit inside the frame.
SHAPE 1 — carrying pole: a straight raw-wood shoulder pole 1.90 m long and 5 cm thick rests across his shoulders behind the neck. In T-pose it runs along the line of the outstretched arms and sticks out 10 cm beyond each hand. No uprights, no crossbar: it never rises above the head.
SHAPE 2 — trade case: from the pole end on HIS LEFT (viewer's right), a stiff waxed-leather strap 6 cm wide and 15 cm long, its flat face to the viewer (no thin rope), holds a tall narrow case of dark leather and wood, 16 x 16 x 60 cm, tied shut, hanging vertically beyond the left hand from about 1.30 m down to 0.70 m above the ground: a tall upright "I".
SHAPE 3 — cargo box: from the pole end on HIS RIGHT (viewer's left), the same kind of stiff leather strap, 6 cm wide and 25 cm long, face to the viewer, holds a wide low wooden box, 40 x 25 x 25 cm, full and with its lid tied, hanging beyond the right hand from about 1.20 m to 0.95 m above the ground: a lying-down block. Nothing sticks out more than 10 cm above its rim. Both loads hang at the same level, more than 50 cm away from the hips.
Clothes: dark waxed canvas vest over a plain shirt, neutral earth trousers, dark leather road boots; pole and box in raw neutral wood, case in dark leather.
AVOID: beard, barrel chest, broad heroic build, leather apron, tool belt or pouch, any tool visible on the body, backpack or back frame, anything rising above the head, blue clothing, high collar with frog fastenings, ivory jumpsuit, barefoot, any turquoise, gold or violet color.
```

Conferir na imagem: em T-pose a vara lê além das mãos, e o estojo e a caixa se separam como I e bloco; as tiras de 6 cm aparecem de face e ligam os dois pesos à vara (corda fina reprova: o `silhueta.py` solta os pesos da figura; ajuste de 2026-10-03, W3); a vara não sobe em Π (não lê como o umbral do Aethron); sem barba e sem peito largo, para não cair no ferreiro default ao lado do Borin.

## lysa — G1 aprovado por delegação (ADR-0010, 2026-10-03)

Ficha: [`fichas/lysa.md`](fichas/lysa.md). Altura de referência 1,58 m. O chapéu passa da cabeça, e a ficha não dá a caixa (a copa tem 0,10 m): medir no concept e usar essa altura no `silhueta.py`.

```text
CHARACTER: Lysa, the village herbalist, an adult woman, the shortest adult of the cast (1.58 m), low hips, short hair hidden under the hat. Nothing in her hands but the gloves.
SHAPE 1 — drying-rack hat: a low-crowned hat of darkened reed with a flat brim 72 cm wide (about four times the head), its edge rolled into a braided reed rim 5 cm thick and sloping about 8 degrees down from crown to edge. Seen from the front it is a wide band about 8 cm tall, clearly thicker than a line. Along the edge, 8 small herb bundles 12 cm long hang upside down from wooden hooks, a fringe that ends about 5 cm above the line of the arms.
SHAPE 2 — balloon base: undyed ivory linen trousers (#E9DEC6), very full at the thighs (each leg about 30 cm wide) and gathered tight at the shin, so the gap between the shins stays open: it never reads as a skirt.
SHAPE 3 — funnel cuffs: thick leather gloves with wide gauntlet cuffs, 16 cm across at the cuff over a 6 cm wrist, the same on both hands: each arm ends in a visible flare at the wrist.
Clothes: clay-brown vest, ivory linen trousers, dark leather gloves, darkened reed hat. The only green on her is the herb bundles (#648B67).
AVOID: pointed witch hat, dark cloak, cauldron, staff, crow, druid or elf look, all-green outfit, leaves sewn on the clothes, glowing healing magic, dress, apron, braid, long hair, herbalist pouches or sample kit as accessories, teal or sage green, pale straw close to gold, any turquoise, gold or violet color.
```

Conferir na imagem: a aba de frente lê como faixa grossa, não como linha (borda de 5 cm); os punhos leem no pulso, separados das mangas em sino da Mara (cotovelo); a base-balão mantém o vão das canelas e não lê como saia.

## tovin — G1 aprovado por delegação (ADR-0010, 2026-10-03)

Ficha: [`fichas/tovin.md`](fichas/tovin.md). Altura de referência 1,76 m. Caixa no `silhueta.py`: ~1,91 m (a boca do chifre sobe 0,15 m acima da cabeça).

```text
CHARACTER: Tovin, the village guard and hunter, an adult man (1.76 m) with a short torso and long legs. Dark brown skin, short tightly curled hair, beard, bare head, clean shoulders. He carries no weapon.
SHAPE 1 — horn mouth: a curved ox horn, 90 cm along its outer curve, slung diagonally across his back from the left hip to the right shoulder. Its open mouth (18 cm wide) rises 15 cm above the top of his head, beside the head over HIS RIGHT shoulder (viewer's left), tilted outward: a curved cone, off-center. Bone-ivory horn, wooden mouthpiece, terracotta lashings.
SHAPE 2 — stake case on the left shin: a hard leather case strapped to the OUTSIDE of his LEFT shin only (viewer's right), from 8 to 32 cm above the ground and 10 cm wide, holding five ash stakes points down; their flat heads stick out 8 cm above the case and fan outward. Short, no feathers: not a quiver. Nothing on the other leg.
SHAPE 3 — funnel leggings: hard leather leggings from the ankle to a hand above the knee, flaring to 22 cm at the knee and narrowing to 13 cm at the ankle, over tight trousers at the thigh. Front view: the gap between the legs closes at the knees and opens again below, like an hourglass.
Clothes: straight charcoal wool tunic to the hip, dark leather, ivory horn (#E9DEC6), terracotta lashings (#A86D52), pale ash stakes.
AVOID: bow, quiver, arrows, bracers, chest badge, hood, helmet, spear, shield, sword, billhook or any blade, anything hanging at the hip or thigh, shoulder pads, gambeson, V-shaped heroic body, broad chest, wide-brimmed hat, forest-green clothing, any turquoise, gold or violet color.
```

Conferir na imagem: a boca do chifre lê como chifre (cone curvo, boca para fora), não como aljava nem como punho de espada; o estojo fica na canela **esquerda** (a direita é da barra da Mara, `ELENCO.md`, Arbitragem 3) e não lê como aljava (curto, cabeças chatas, sem pena); os canos em funil leem no joelho e não como bota de cano largo.

## eira — G1 aprovado por delegação (ADR-0010, 2026-10-03)

Ficha: [`fichas/eira.md`](fichas/eira.md). Altura de referência 1,92 m (nada passa da cabeça).

```text
CHARACTER: Eira, the village teacher, a woman of about 35, the tallest person of the cast (1.92 m). Visible neck, short dark hair with no volume, no glasses, nothing in her hands.
SHAPE 1 — slate on the right flank: a portrait-oriented blackened wooden slate, 45 x 60 x 2 cm, with a light wooden frame and chalk marks, its FACE TURNED TO THE VIEWER in the same plane as her chest. It hangs against the RIGHT side of her body (viewer's left), from 0.85 to 1.45 m above the ground, about 12 cm below the line of the arms, and sticks out 20 cm beyond her flank. A thin leather strap runs from her left shoulder to her right hip.
SHAPE 2 — belted waist: a stiff leather belt 14 cm tall cinches the tunic to 26 cm wide at the waist; above it the tunic gathers to 38 cm at the ribs, below it opens to 40 cm at the hips and ends there. Front view: a clear X-shaped notch in the torso.
SHAPE 3 — lantern under the left arm: a small unlit box oil lantern of dark iron, 14 x 14 x 20 cm, hangs from a short hook against her LEFT ribs (viewer's right), from 1.28 to 1.48 m above the ground, sticking out 12 cm from the body: a small box facing the big slate.
Clothes: straight ivory linen sleeves (#E9DEC6) rolled to the elbow and dusted with chalk; hip-length tunic in a dark, muted terracotta (#A86D52, darker); trousers and work boots; charcoal and chalk on the slate.
AVOID: mage robe, hood, staff, wand, pointer stick, glowing book, books under the arm, book bag, bell or pocket sleeves, dress, skirt, high bun, braid, apron, grey bun with glasses, lilac, honey or mustard tones, deep blue, slate seen edge-on, any turquoise, gold or violet color (also not in the lantern glass).
```

Conferir na imagem: a lousa de face não lê como escudo nem como bolsa (retrato, moldura clara, giz à vista); a cintura lê a 30% do lado esquerdo; a lanterna não se confunde com o estojo e a caixa do Daren nem com a vara da Maelis.

## nilo — G1 aprovado por delegação (ADR-0010, 2026-10-03)

Ficha: [`fichas/nilo.md`](fichas/nilo.md). Prompt da idade de 5 anos. Altura de referência 1,04 m (1,11 m com o tufo). Caixa no `silhueta.py`: 1,28 m, a ponta mais alta da forquilha (ficha, C3, geometria corrigida em 2026-10-03, W3).

```text
CHARACTER: Nilo, a 5-year-old village boy, the smallest child of the cast (1.04 m), with real small-child proportions (big head, short limbs) and an eager, mischievous face. He imitates the village guard with things he finds. Child-sized clothes, nothing adult about the body.
SHAPE 1 — forked stick on the back: a bare forked branch 1.00 m long (80 cm handle, two 20 cm prongs opened about 60 degrees), 3.5 cm thick, strapped diagonally across his back about 23 degrees from vertical. The lower end hides behind his right thigh; the handle crosses HIS LEFT shoulder near its outer end (viewer's right), not close to the neck, and the open Y rises above and beside his head with a clear gap of at least 4.5 cm between the stick and the head along the whole head; the prong tips are 24 cm above the top of the head. Nothing sticks out below. A plain stick, not a weapon: no blade, no guard, no slingshot band.
SHAPE 2 — cowlick: short close-cropped hair except one stiff tuft growing from the RIGHT side of the crown (viewer's left), pointing up and out at about 50 degrees: a blunt wedge 14 cm long, 7 cm wide at the base and 3 cm at the tip, made of two or three separate locks, on the opposite side of the head from the fork.
SHAPE 3 — rolled cuffs: old oversized trousers rolled four times around each shin into thick rings 17 cm wide over 7 cm shins, from 8 to 22 cm above the ground, above small low boots.
Clothes: faded terracotta trousers (#A86D52, washed out, the lighter reverse showing in the rolls) with burrs and thorns caught in them; an oversized undyed linen shirt tucked into the belt and stuffed with found things (a pine cone, a feather, dark leaves); grey-brown branch.
AVOID: sword, spear, slingshot, staff or any weapon, round cloud of hair wider than the head, spiky hero hair, blue tunic, neckerchief, canvas bag, magnifying glass, notebook, green vest, mustard, adult proportions, glowing marks or circles, any turquoise, gold or violet color.
```

Aos 8 (concept depois): 1,24 m, as mesmas três formas; a forquilha fica nas costas, a ~22°, com as pontas 5 e 12 cm acima da cabeça e 5,8 cm de folga; três voltas de rolo em vez de quatro (Ø 15 cm).

Conferir na imagem: o cabo cruza o ombro perto da ponta, com pelo menos 4,5 cm entre o cabo e a cabeça em toda a altura dela (colado na cabeça, funde em preto), e a forquilha não lê como espada nem lança; o redemoinho não lê como chifre nem como a nuvem redonda da Maelis; os rolos leem nas canelas (+5 cm de cada lado).

## sera — G1 aprovado por delegação (ADR-0010, 2026-10-03)

Ficha: [`fichas/sera.md`](fichas/sera.md). Prompt da idade de 5 anos. Altura de referência 1,10 m. Caixa no `silhueta.py`: 1,19 m com o coque.

```text
CHARACTER: Sera, a 5-year-old village girl, clever and competitive, with real small-child proportions (1.10 m). No jewelry, no marked waist, no heels: everything tied down for running. A small closed wax tablet (two light wooden leaves 14 x 10 cm with a leather hinge) clipped to the chest of her vest.
SHAPE 1 — high bun: hair wound into a small cylinder centered on top of the head, 9 cm tall and 7 cm wide, tied with a leather strip, loose strands escaping, a short blunt bone stylus pushed through it. A child's knot for running, not a lady's bun.
SHAPE 2 — egg vest: a padded vest of undyed linen stuffed with wool, from the armpits to the hips, shaped like an egg: 36 cm wide at the belly against a 21 cm chest, bulging about 7.5 cm beyond the torso on each side and clearly wider than the legs below; it narrows to 27 cm at the armpits, the shoulders round and inside the shoulder tips.
SHAPE 3 — column legs: wide straight trousers from hip to ankle with the legs touching, reading from the front as one single block 24 cm wide with no gap between the legs, ending in low flat shoes.
Clothes: ivory linen vest (#E9DEC6); trousers in dark green (a darker shade of #648B67); light wood and dark wax on the tablet.
AVOID: glasses, book under the arm, crossed arms, scowl, long loose hair, ribbon or bow, twirly dress, rust sleeveless tunic, green sash, balloon shorts, folded boots, school bag, clogs, platform soles, blanket or cape on the back, square boxy vest, board hanging from the neck, smooth phone-like slab, adult look or makeup, any turquoise, gold or violet color.
```

Aos 8 (concept depois): 1,28 m (1,37 m com o coque); o mesmo coque; o colete fica curto, termina nas costelas e é menos redondo; a calça bate no meio da canela; a tabuinha vira prensa de folhas, com pontas verdes para fora.

Conferir na imagem: a bolota lê como ovo, separada da coluna, e não como tronco reto em bloco (a folha do Aethron); o coque lê como nó de criança e não como coque de adulta; a coluna é um bloco só, sem vão, até o tornozelo.

## oren — G1 aprovado por delegação (ADR-0010, 2026-10-03)

Ficha: [`fichas/oren.md`](fichas/oren.md). Corpo de 1,60 m. Caixa no `silhueta.py`: @2,17 com o cestinho (@2,10 sem ele).

```text
CHARACTER: Oren, the village merchant and porter, a small dry middle-aged man (1.60 m): narrow sloping shoulders, thin arms, no belly, no jewelry. A leather tumpline across his forehead takes part of the load.
SHAPE 1 — basket tower: a wooden frame 50 cm wide and 1.10 m tall on his back, from the waist to 35 cm above his head, stacked with three dark wicker baskets over a wooden crate at the base (50 x 25 x 30 cm) with the ends of iron bars sticking out, and crowned by one small round pale wicker basket (28 cm wide, 22 cm tall), the top at 2.17 m. From the front the frame sticks out 7 cm beyond each side of his torso and rises well above his head: a solid centered column. Hooks on the uprights.
SHAPE 2 — bowed legs: thin legs bowed by a lifetime of loads, in knee-length breeches and wrapped shins; the knees 10 cm farther apart than the ankles, so the gap between the legs is a tall oval.
SHAPE 3 — cargo pennant: a stiff triangle of waxed terracotta cloth (#A86D52), 30 x 20 cm, on a thin wooden staff fixed to the top corner of the frame on HIS LEFT (viewer's right), held rigid and pointing outward. It sticks out 25 cm beyond the edge of the tower, between 1.92 and 2.10 m above the ground, clearly OUTSIDE the tower's rectangle.
Clothes: ivory linen (#E9DEC6), brown wool, terracotta straps and tumpline, dark wicker, the small top basket in pale ivory raw wicker, dark neutral iron.
AVOID: fat jolly merchant, fancy vest, rings, hat, thin mustache, swindler look, broad heavy body, awning or wide-brimmed hat, account book or coin purse as accessory, traveler's cape, mauve coat, a golden-looking top basket, any turquoise, gold or violet color.
```

Conferir na imagem: a bandeirola fica fora do retângulo da torre e lê como triângulo; o vão oval das pernas lê em T-pose e não se confunde com os canos do Tovin; a torre lê cheia (bloco), o que a separa do Π vazado do Aethron.

## maelis — G1 aprovado por delegação (ADR-0010, 2026-10-03)

Ficha: [`fichas/maelis.md`](fichas/maelis.md). Altura de referência 1,68 m (nada passa da cabeça).

```text
CHARACTER: Maelis, the village administrator, a mature woman (1.68 m), bare head, calm and attentive face. A straight felt tabard to mid-thigh with no marked waist and an ink stain.
SHAPE 1 — grey cloud: curly grey hair worn loose in a round volume 32 cm wide (twice the width of the head), tied only at the nape. Nothing on top of the head. Front view: a circle bigger than the head.
SHAPE 2 — office staff: a dark-painted wooden staff, 90 cm long and 4 cm thick, with an iron ferrule, slid through two loops at the end of the writing board on HER LEFT (viewer's right). It stands upright beside her body, outside the body outline, from 0.40 to 1.30 m above the ground: it stops below the shoulder and never touches the ground.
SHAPE 3 — writing board at the waist: a wooden board 62 x 34 x 2 cm with a raised edge, hung from her neck by two leather straps, its top 1.00 m above the ground, held horizontal and sticking out 11 cm beyond each side of the hips: in T-pose a horizontal bar below the arms, crossed at its left end by the staff. On it: a book (30 x 22 x 5 cm), a horn inkwell in the right corner, and one ivory sheet (15 x 20 cm) standing upright in a wooden clip, rising 15 cm above the board.
Clothes: ink-grey felt tabard, grey hair, dark staff with iron tip, terracotta straps (#A86D52), ivory pages (#E9DEC6).
AVOID: noble robe, rich long tunic, gold trim, sash, regional insignia, crown, tiara, scepter, jewelry, stiff high collar, scroll or quill as pose props, old-rose tunic, plum sash, deep blue, staff used as a crutch or touching the ground, any yellow metal, any turquoise, gold or violet color.
```

Conferir na imagem: a prancha de frente lê como barra (as pontas de 11 cm passam do quadril); a vara de 4 cm não some e não vira muleta; a nuvem lê como círculo maior que a cabeça, sem nada em cima.

## avatar — G1 aprovado por delegação (ADR-0010, 2026-10-03)

Ficha: [`fichas/avatar.md`](fichas/avatar.md). Prompt de `avatar_crianca5`. Altura de referência 1,10 m (nada passa da cabeça).

```text
CHARACTER: The player child, 5 years old (1.10 m), real small-child proportions (big head, about 5.5 heads tall). Plain neutral face and plain short neutral-brown hair, nothing distinctive: appearance will be customized later, so all identity is below the chin. Plain undyed linen shirt and neutral trousers under the blanket. Empty hands, no weapon.
SHAPE 1 — bedroll across the shoulder blades: the top edge of a terracotta wool blanket, rolled once into a roll 9 cm thick, runs HORIZONTALLY across the upper back from 70 to 79 cm above the ground, 60 cm from end to end; in T-pose its two ends show below the arm line, about 19 cm beyond the torso on each side. Two cords go over the shoulders and cross on the chest. The roll stays below the shoulder line and the neck stays visible.
SHAPE 2 — bell: below the roll, the rest of the blanket passes inside the belt at the back (the waist narrows) and opens like a bell down to a straight hem 48 cm wide at the back of the knees (30 cm above the ground). From the front it shows on both sides of the legs, up to 14 cm beyond each leg. Straight hem, no points.
SHAPE 3 — clogs: natural wooden clogs with high soles, base 18 x 10 cm and 8 cm tall: two chunky blocks under thin ankles.
Clothes: plain terracotta wool blanket (#A86D52), no patches, no woven border; neutral linen underneath; natural wood clogs.
AVOID: distinctive hairstyle (curls, spikes, bright color), striking eyes or face, green vest, balloon trousers, boots, hero cape, brooch, fabric flowing in the wind, sleeping bag on a backpack, backpack, glowing marks, tattoo or sigil on the body, wooden sword or any weapon, any turquoise, gold or violet color.
```

Aos 8 (concept depois): 1,28 m e a mesma manta, que não cresce: a barra sobe para acima do joelho (0,42 m do chão) e as pontas da trouxa passam só 9 cm por fora do braço caído; tamancos maiores, mesma forma.

Conferir na imagem: as três formas não dependem do cabelo (gerar de novo com outro cabelo e comparar); a trouxa com o sino lê como manta de dormir, não como capa curta nem saia; as pontas da trouxa ficam por fora de onde o braço cai (a vista de braço caído é outra geração).

## aethron — G1 aprovado por delegação (ADR-0010, 2026-10-03)

Ficha: [`fichas/aethron.md`](fichas/aethron.md). Corpo de 1,90 m. Caixa no `silhueta.py`: 2,55 m (soleira de 0,15 m + umbral até 2,40 m acima dela; conta desta página). Não é personagem de vila: o bloco comum vale, e a laje sob os pés é parte dele, não chão.

```text
CHARACTER: Aethron, the guardian of the Threshold that every soul crosses before birth. Not a villager, not a god: an ordinary-looking man (1.90 m), clean-shaven, short straight-cut hair, neutral attentive expression, ordinary skin and eyes with no glow, upright posture, barefoot, empty hands. He IS a door: only straight lines and right angles, strictly symmetric, no curves anywhere.
SHAPE 1 — door frame: a wooden yoke 70 cm wide rests on his shoulders; from its two ends rise two square posts (10 x 10 cm) to 38 cm above his head, closed by a straight lintel 70 x 12 cm, flush with the posts, no overhang, no upturned ends. His head sits inside the frame with an open gap of 14 cm on each side and 38 cm above it. Flat painted gold (#D6B36A), square pegs at the corners.
SHAPE 2 — door leaf: a rigid overgarment, a plain rectangle 48 cm wide and 1.05 m tall from the chest to a hand below the knee, no waist, no opening, with one thin vertical gold seam painted down the middle. It hides the waistline and the gap between the legs: the torso is a closed door with shins below.
SHAPE 3 — threshold slab: he stands on a worn stone slab 90 x 40 x 15 cm, three times as wide as his feet, with a shallow rectangular hollow (40 x 25 cm) worn into its middle and painted flat gold. The slab belongs to the character and is fully visible.
Clothes: leaf, sleeves and trousers in a warm neutral mid-value (50–65% grey), neither white nor ivory; neutral grey stone; gold only on the frame, the seam and the hollow.
AVOID: wings, halo or any ring or circle of light, any curve, white-and-gold robes, long beard, old sage, hood, floor-length cloak, staff, lantern, key, book, orb, glowing hands or eyes, golden skin, starry body, long silver hair blowing, beautiful androgynous divine face, shrine gate with two crossbars or upturned ends, red paint, smile or frown, marble palace, clouds, any turquoise or violet color.
```

Conferir na imagem: o Π lê como batente pelos vãos em volta da cabeça, e não como a torre cheia do Oren; o dourado em volta da cabeça não lê como auréola; em T-pose os ombros não atravessam a canga rígida.

## simbolo_limiar — G1 aprovado por delegação (ADR-0010, 2026-10-03)

Ficha: [`fichas/simbolo_limiar.md`](fichas/simbolo_limiar.md). Altura de referência 1,50 m (anel de 1,20 m e o fio solto até o chão). Aqui **não** se cola o bloco comum: ele pede personagem em T-pose, contorno e sombreado toon, e o símbolo é luz em cor chapada. O bloco abaixo substitui os dois. Esta geração mostra o estado antes do toque (todos os nós turquesa); a versão do eco, com os dois nós da borda do vão em dourado (Ø 12 cm, miolo marfim #E9DEC6), é uma segunda geração.

```text
OBJECT: Original prop concept for a stylized anime fantasy game: the Threshold Symbol, a sign made of light. ONE object only, not a character, standing upright, FRONT view, orthographic. Flat unshaded color that reads as light: no outline, no shading, no glow halo, no particles. Plain flat uniform light cream background, no floor, no cast shadow, no other objects, no people, no text, no logo, no watermark. The whole object fits inside the frame with margin. Do not copy any existing emblem, logo, game or artist.
SHAPE 1 — braided ring: three strands of turquoise light (#86C8C9) braided into one upright ring, 1.20 m outer diameter, braid 10 cm thick; the center is completely empty and the background shows through it. A small node of light at each crossing of the braid, about every 28 cm (11 nodes, never 4, no four-fold symmetry).
SHAPE 2 — the gap: the ring does not close. A 40-degree opening at the one o'clock position (top, to the viewer's right), about 40 cm across. At each edge the braid ends in a small node (6 cm), from which the three strands spread and point toward the other side without reaching it. Nothing crosses the gap.
SHAPE 3 — loose strand: one strand leaves the braid at the seven o'clock position (lower left as the viewer sees it, not the lowest point) and falls about 45 cm in two gentle waves to the bottom of the object, where the ground would be; 5 cm thick. With it the whole object is 1.50 m tall.
AVOID: four-fold symmetric emblem, central knot, four equal arcs or four dots, stone, moss, carved or glowing glyphs, metal, jewelry, rope or fiber texture, slip knot, magic circle with runes, star polygon, writing, lying flat on the ground, web, feathers or beads inside the ring, portal swirl or glowing surface inside the ring, horseshoe, gap cut by a vertical line, side wedge like a mouth, cross below, diagonal handle, single brush stroke, three interlocked arcs, straight or 45-degree strand, gold, violet.
```

Conferir na imagem: lê como luz, e não como corda nem plástico (cor chapada, sem contorno, sem pulso); o teste de descrição não cita emblema, marca nem jogo; numa vista a 45° (outra geração) o vão continua visível.

Os blocos dos outros personagens entram aqui quando a ficha de cada um passar no G1.
