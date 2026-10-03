# Prompts de concept para o G2 (bíblia v1.1, só o slice)

- **Para quê:** gerar o concept de cada personagem com ficha G1 aprovada, no formato que o teste cego de silhueta (`client/tools/silhueta.py`) e o Tripo (imagem → 3D) precisam. Substitui, para o elenco do slice, as fichas de prompt da bíblia v1.0 (`arte/referencias/acervo/documentos/`), que descreviam só o cargo.
- **Quem gera:** o idealizador, no Tripo Studio (Imagem, GPT Image 2), na conta do plano Max. Um agente não gera: gastar créditos pede a mão dele.
- **Ordem:** só depois do G1 aprovado na ficha (`docs/arte/fichas/<id>.md`, campo "estado").

## Antes de gerar
1. No Tripo, marcar a geração como **privada**.
2. Colar o bloco **Estilo + formato** e, embaixo, o bloco do personagem.
3. Gerar 2 a 4 variações; ficar com a que mostrar as 3 formas da ficha (C3) mais claras.
4. Baixar o PNG para `arte/referencias/concepts_g2/<id>_frente_v01.png` (versões seguintes `v02`...; nunca sobrescrever).
5. Anotar a data e o id da tarefa do Tripo; o coordenador abre o bloco `### <id>` na `PROVENIENCIA.md`.

## Bloco "Estilo + formato" (igual para todos)

```text
Original character concept for a stylized anime fantasy game (cel-shaded / toon look: flat colors, soft two-tone shading, thin clean outline; medieval village, believable linen, wool, leather and iron). ONE character only, full body, FRONT view, orthographic, standing in a strict T-pose (arms straight out horizontally, palms down, legs slightly apart, feet pointing forward). Plain flat uniform light cream background, no floor, no cast shadow, no other objects, no text, no logo, no watermark. Even studio light. The whole body fits inside the frame with margin above the head and below the feet. Readable silhouette first: the three named shapes below must be clearly visible against the background. Do not copy any existing anime, game or artist.
```

## borin — G1 aprovado por delegação (ADR-0010, 2026-10-03)

Ficha: [`fichas/borin.md`](fichas/borin.md). Altura de referência 1,82 m.

```text
CHARACTER: Borin, the village blacksmith, about 50 years old. Tall and lean (1.82 m), narrow torso, long arms. Long neck that juts forward, shaved head, clean-shaven, small short leather visor on the forehead.
SHAPE 1 — asymmetric arms: his RIGHT arm is wrapped from shoulder to glove in a thick padded leather forge sleeve, about twice the volume of the left arm; his LEFT arm is thin and bare to the elbow, with a rolled-up linen sleeve.
SHAPE 2 — a CLOSED iron ring, 28 cm wide, thick dark iron bar, held by a rigid bracket on the left side of the belt so it sits 10 cm OUT from the left thigh, clearly outside the body outline. Small flat iron tags hang inside the ring, plus one flat notched iron plate (a measuring gauge, 18 x 6 cm, with 5 notches).
SHAPE 3 — the shaved skull on the long forward neck, giving a hooked, leaning-forward head line.
Clothes: undyed ivory linen shirt; leather apron ONLY from the waist down, split into two flaps, terracotta brown leather (#A86D52); dark brown trousers; plain work boots. Dark neutral iron for metal.
AVOID: beard, barrel chest, broad heroic build, full bib apron, hammer or anvil as accessory, any turquoise, gold or violet color, any open or broken circle shape.
```

Depois de gerar, conferir na imagem as 3 formas e os itens do "Para o G2" da ficha (§5): o aro fora do contorno, a assimetria dos braços, aro fechado e sem as cores reservadas.

Os blocos dos outros personagens entram aqui quando a ficha de cada um passar no G1.
