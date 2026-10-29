# Ficha G1 — `feixe_de_espinho`

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md)). Criatura do episódio 2 ([`EPISODIO2_DESPERTAR_DOS_TALENTOS.md`](../../design/EPISODIO2_DESPERTAR_DOS_TALENTOS.md), abreviado `EP2`), que é **PROPOSTA inteira**. Nada aqui vale antes de o idealizador aprovar o roteiro **e** o Art Director dar a nota.
> Fato com fonte é cânone ou está no `EP2` (marcado `EP2 §…`). **PROPOSTA** = ideia desta ficha.
> Criatura não fala nem tem destino próprio: C6, C7 e C8 são adaptados, e o pontuador decide (o ADR não define N/A).

## 1. Identificação

- **id:** `feixe_de_espinho` (no código, `criatura.feixe_de_espinho`, `EP2` §3.4). Passa no regex do `PIPELINE.md` §6 (16 caracteres) e não contém chave do `HumanoidMapping`.
- **categoria:** criatura. Para orçamento, Prop médio (§6).
- **onde aparece:** episódio 2. E2-B11 (`q13_o_fio_do_nilo`, encontro `enc.espinhal_feixes`, de dia); E2-B14 (`q15`, `enc.trilha_misto` e `enc.espinhal_feixes_noite`). Âncoras: `bosque_espinhal` (a casa deles, `EP2` §1.2) e `bosque_trilha`.
- **cânone de partida:**
  - A borda do Bosque dos Sussurros é espinheiro que "fecha de novo"; Tovin o corta com o podão que fica no cabide do posto; os canos de couro nas pernas dele são por causa desse espinheiro (ficha `tovin`, C2, aprovada no G1 por delegação). A ficha `mara` parte da mesma premissa ("o espinheiro fecha de novo e o podão dele reabre").
  - Nilo passou por um buraco no espinheiro aos 5; aos 11 ele o acha de novo, e no espinhal há um fio de luz turquesa e violeta preso nos espinhos (`EP2` B11).
  - `EP2` §3.4: fechado, um feixe vertical de galhos de 1,3 m, igual ao espinheiro em volta; aberto, tripé de pernas longas e finas de 1,8 m com um miolo pendurado no centro (o alvo); espinhos longos de ponta clara, eriçados no bote; casca cinza-terracota, pontas marfim; parado é mata; bote em linha de 2 m, aviso de 0,8 s (estalo seco e espinhos eriçando); bloqueio frontal anula; depois do bote, fica aberto 1,5 s (a janela); fio de luz e cravo prendem o tripé aberto; `peso` 2. Percepção dá o contorno a 3 m + 1 m por ponto (teto 8 m) e adianta o aviso em 0,1 s por ponto (teto 0,3 s) (`EP2` §3.3). Marcial ≥ 2: o contra derruba um feixe aberto.
- **autor da ficha / data:** Concept Art Lead (agente), 2026-10-29
- **estado:** aguardando nota (autoavaliação na §5; **pendente de nota do Art Director separado, ADR-0010 §6**)

## 2. Critérios C1–C10

**C1 — Gancho.** Um feixe de galhos igual a todo o espinheiro do bosque que, quando a criança passa perto demais, se abre em três pernas mais altas que um homem e dá o bote; é por isso que o espinheiro que Tovin corta de manhã está fechado de novo à tarde. *(`EP2` §3.4 com a ficha `tovin`, C2)*

**C2 — Necessidade do mundo.** O espinheiro da borda "fecha de novo" todo dia, e alguém em Auren tem um ofício por causa disso (ficha `tovin`, C2): o feixe é a parte do espinheiro que anda (`EP2` §3.4). **PROPOSTA:** ele anda para fechar buraco. O feixe disperso não foge para o fundo: volta ao vão mais próximo do espinheiro e fica ali, fechado. Cada buraco que Tovin abre, um feixe fecha, e cada buraco que uma criança atravessa, também; foi assim que o buraco do Nilo sumiu e voltou (`EP2` B11). Os espinhos seguram o que passa: lã, pano, e o fio de luz que Nilo acha. Sem ele, o espinheiro seria só cenário.

**C3 — Silhueta em 3 formas.** *(medidas PROPOSTA, a partir do `EP2` §3.4)* Vista canônica: **de frente**, porque o bote é em linha, na direção da criança.
1. **Feixe fechado:** fuso vertical de galhos de 1,30 m × Ø 0,40 m no meio, mais fino em cima (Ø 0,15 m) e embaixo (Ø 0,25 m), amarrado por um nó de raiz a 0,85 m. É, de propósito, igual ao espinheiro em volta: a forma 1 é o disfarce, e o teste é que ela **não** se separe do cenário.
2. **Tripé:** as três pernas são os galhos de baixo do feixe, que se abrem até 1,80 m de altura, em base triangular de 1,40 m de lado, pernas de Ø 0,05 m. No centro pende o **miolo**, uma bola de raiz de Ø 0,30 m, de 0,75 a 1,05 m do chão. Em preto: um Λ alto de três pernas, com um ponto no meio. É a forma que lê o perigo.
3. **Coroa eriçada:** no aviso e no bote, espinhos de 0,25 m saem radiais do terço de cima, em ~20 pontas, e o contorno do alto passa de Ø 0,15 m para Ø 0,90 m. Pontas claras (Marfim) de 0,06 m, que leem à noite (`enc.espinhal_feixes_noite`).
- *T-pose:* não se aplica; a pose de repouso do rig é a forma 1. *Câmera:* com a criança de ~1,42 m (HIPÓTESE, `EP2` §4.2) e a câmera a ~3,5 m atrás dela, o tripé aberto é 1,27 vez a altura dela e o miolo fica na altura do peito: o alvo está no golpe de espada, sem pular. **Fraqueza declarada:** de lado, o tripé ainda é Λ, mas duas pernas se sobrepõem; a leitura de três pernas é de frente e de 3/4.

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **O miolo.** Bola de raiz enrolada, Ø 0,30 m, a única parte do feixe que não é galho e a única que leva dano.
- **O jogo lê:** o estado do feixe (`fechado`, `aviso`, `bote`, `aberto`, `preso`, `voltando`). Fechado, o miolo fica dentro do feixe, a 0,85 m, coberto pelos galhos: golpe no feixe fechado não dá dano e faz um "toc" de madeira (o jogo diz ao jogador que ali é bicho, sem dar dano). Aberto (1,5 s depois do bote), o miolo fica exposto entre as pernas e é o único alvo. Preso (fio de luz ou cravo numa perna, `EP2` §3.3), a janela dura o tempo da prisão (2 s pelo fio, com Arcana ≥ 1; até o cravo ser arrancado, com Artesanal ≥ 2).
- **O jogador vê:** o miolo é Marfim desbotado, mais claro que a casca, e balança no bote como um prumo; quando o feixe fecha, ele some.
- **Segundo efeito do mesmo objeto (PROPOSTA de cena):** no `bosque_espinhal`, o fio de luz do Nilo está preso nos espinhos de um feixe fechado. É esse feixe que abre quando a criança puxa o fio (`o_buraco_no_espinheiro` → `seguir_o_fio`, `EP2` B11).

**C5 — Regra exclusiva.** É a única criatura que fica invisível parada: com Percepção baixa, ela só ganha contorno a 3 m; com Percepção 5, a 8 m (`EP2` §3.3). O aviso começa mais cedo com Percepção (até 0,3 s). O bote é em linha reta de 2 m e o bloqueio frontal o anula por inteiro; é a criatura que ensina o bloqueio, como o abafador ensina a esquiva. Marcial ≥ 2: o contra no fim do bote o derruba aberto. **PROPOSTA:** disperso, ele anda até o vão mais próximo e fecha (C2); na área, o caminho que ele fechou fica fechado até a criança sair (estado de cena, não vai para o save).

**C6 — Voz.** *(adaptação: três sons escritos; o pontuador decide se conta.)* PROPOSTA:
1. Parado: nada além da brisa que mexe todo o espinheiro. Ele balança no mesmo tempo dos outros arbustos, nem mais, nem menos.
2. Aviso: dois estalos secos de galho quebrando, "tec… tec", espaçados de 0,4 s, e o chiado dos espinhos eriçando por baixo. Os 0,8 s de aviso são o intervalo entre o primeiro estalo e o bote.
3. Voltando: galho arrastado na terra, que termina num rangido curto quando ele fecha no vão.

**C7 — Contradição visível.** *(PROPOSTA)* Anda para ficar parado. Toda vez que é disperso, em toda luta, o jogador o vê caminhar de volta, devagar, sobre as três pernas, até um buraco do espinheiro, e fechar ali. A criatura que ataca quem passa é a mesma que tapa o buraco por onde se passaria. E o que ela segura nos espinhos (o fio do Nilo) é a pista que a criança veio buscar.

**C8 — Relação com o avatar por destino e origem.** *(PROPOSTA; ADR-0004: nenhum número muda por destino.)*
- **serena, normal, dificil:** igual.
- **ruptura:** igual em número e forma. O fio preso nos espinhos tem violeta, a cor da luz que só a Ruptura viu da janela aos 8 (E2-B07): o jogador da Ruptura reconhece a cor antes de Nilo falar. É informação que já está na tela para todos.
- **origem `guardioes`:** a criança cresceu vendo o pai ou a mãe voltar do vão com o couro das pernas riscado (PROPOSTA de fala na tarefa de família, `EP2` §3.7); as outras origens: igual.
- Sincero: não ramifica pelos quatro destinos. Pela régua, nota 1.

**C9 — Mudança no tempo.** O salto do episódio é dos 8 para os 11.
- **Antes (8):** não se luta com ele. Na q09, na borda da trilha, Tovin aponta um feixe fechado entre o espinheiro e diz que "esse aí não é mato"; a criança só vê um feixe igual aos outros (PROPOSTA de fala, sem encontro novo). O espinheiro que fecha de um dia para o outro é o feixe trabalhando.
- **Depois (11):** a criança de ~1,42 m enfrenta o feixe no espinhal de dia (q13) e de noite (q15). O miolo, que aos 8 ficaria na altura da cabeça dela (0,75–1,05 m contra 1,28 m de altura), aos 11 fica na altura do peito: dá para bater de frente.
- **Variante de malha:** não.

**C10 — Momento de cartaz.** *(PROPOSTA)* Espinhal fechado, de dia, Nilo uns passos à frente. A criança passa entre feixes de galho iguais. Dois estalos secos. Um dos feixes se abre em três pernas mais altas que ela, eriça uma coroa de espinhos claros e vem em linha. Ela ergue a espada e bloqueia; o feixe fica aberto, com uma bola de raiz balançando no meio, e ela bate nela. O feixe recolhe as pernas, anda até um buraco do espinheiro e fecha. No espinho dele, um fio de luz turquesa e violeta.

## 3. Amarração

- **Destino, Grau ou Trama:** nenhum o altera. A Trama encosta nele por fora: o fio de luz preso nos espinhos (`EP2` B11).
- **Decisão de jogo que ele cria:** parar e olhar ou seguir rápido pelo espinhal. Quem investiu em Percepção vê o contorno de longe e escolhe por onde passar; quem não investiu aprende a bloquear. Arcana e Artesanal dão um terceiro jeito: prender e bater.
- **Pilar:** viver e crescer (o mesmo espinheiro que a criança via o Tovin cortar aos 5 vira o que ela aprende a ler aos 11).

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| árvore-gente: tronco com rosto, braços de galho e pés de raiz, que fala devagar | sem rosto, sem braços e sem fala; um feixe de galhos que vira tripé |
| homenzinho de graveto com garras, que anda curvado | três pernas, nenhuma cabeça; o alvo é uma bola de raiz pendurada |
| planta carnívora com boca | nada abre em boca; o perigo é a coroa de espinhos e o bote em linha |
| bola de espinhos que rola | parado de pé, anda devagar, ataca só em linha reta de 2 m |
| máquina de três pernas, metal, raio | galho seco, casca cinza-castanha e ponta clara; nenhum brilho |
| espinheiro escuro e venenoso, roxo ou preto | a cor do espinheiro de verdade; Violeta e Turquesa só no fio preso, que não é dele |

## 5. Avaliação

**Autoavaliação do autor (não vale como nota; ADR-0002 e ADR-0010 §6):**

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 2 | 2 | 2 | 1 | 2 | 1 | 1 | 2 | **17/20** |

- C9 = 1: a batida dos 8 é uma fala e um feixe parado, não uma mudança dele; C6 = 1 (som, não fala); C8 = 1 (não ramifica pelos quatro destinos).
- **Pendente de nota do Art Director separado (ADR-0010).** Corte: ≥ 14/20 e nenhum zero em C2–C5 (HIPÓTESE do ADR).
- **Pontuador / data:**
- **Veredito:**
- **Observações do pontuador:**

## 6. Encaminhamento

**Para o G2 (concept), só depois de aprovado:**
- vistas: frente (canônica), 3/4, lado, de cima (a base em triângulo); três estados (fechado no meio de espinheiro comum, tripé, coroa no bote); ao lado, a criança de ~1,42 m; linha de chão.
- silhueta adaptada: o fechado embaralhado com 3 arbustos do espinheiro **tem** de se confundir (é o disfarce; se se destaca, reprova pelo outro lado); o tripé e a coroa embaralhados com o abafador, o lajeiro e o Nilo aos 11 (o Y da forquilha), ≥ 4 de 5 acertos.
- paleta: **PROPOSTA de correção do `EP2`:** "cinza-terracota (Terracota #A86D52 dessaturado)" fica perto demais da Terracota de Auren e da plaqueta de cobre, que o ADR-0010 (item 4) deixou como marca única do jogador. A casca vai para um cinza-castanho escuro, fora da família da Terracota (ordem de grandeza #6B625A; o hex final é do G2), com o verde do bosque nas sombras; pontas e miolo em Marfim #E9DEC6 desbotado. **Fora:** Turquesa e Violeta no corpo (só no fio preso, que é peça à parte).
- orçamento (`PIPELINE.md` §4, HIPÓTESE v0): categoria mais próxima **Prop médio** (1–4 m): 2 000 tris no LOD0, LOD1 ≤ 50%, 1 material, 512 px; projeta sombra. Os espinhos como cartões com recorte (`alpha clip`, permitido só em cabelo e folhagem, e isto é folhagem). Ossos: a categoria não tem teto; esta ficha declara ≤ 16. Encontro de 4 feixes = 8 000 tris.
- **rig:** esqueleto genérico (criatura sem forma humana; `Generic`, não Humanoid). Ossos: `raiz`, `feixe_base`, `feixe_meio`, `feixe_topo`, `miolo` e três pernas de três ossos (`perna_a_1..3`, `perna_b_1..3`, `perna_c_1..3`) = 14. A coroa é um blend shape (`ericar`, 0 → 1). Pose de repouso: fechado. Clips: `balancar` (laço, no tempo da brisa do espinheiro), `abrir` (0,4 s), `aviso` (0,8 s, coroa sobe), `bote` (0,3 s, 2 m em linha; o código move), `aberto` (1,5 s, miolo balança), `preso` (laço), `golpeado` (0,2 s), `voltar` (andar de 3 pernas, laço) e `fechar` (0,5 s). Sem clip de morte. Nomes sem chave do `HumanoidMapping`. Pasta proposta: `Art/Criatura/feixe_de_espinho/`.
- código que a ficha pede (dependência, não feito aqui): estado `fechado` sem dano com som de madeira; contorno por Percepção; o feixe disperso escolhe o vão livre mais próximo e fecha (estado de cena).
- testes a registrar: silhueta adaptada, descrição, troco (trocar pelo lajeiro no espinhal: a pista do fio e o "fecha de novo" somem; não passa sem reescrita).

**Para o G3:** bloco `### feixe_de_espinho` em `PROVENIENCIA.md`, como no modelo.

## 7. Teste de originalidade honesto

Lista de revisão; **não** vai para o briefing do gerador.

| Pode lembrar | Por quê | O que afasta |
|---|---|---|
| Ents (*O Senhor dos Anéis*), Treants (D&D), Groot | árvore que anda | não é humanoide: sem rosto, sem braços, sem fala, sem tronco de árvore; um feixe de galhos que vira tripé |
| *Twig blight* (D&D) | arbusto seco que se finge de planta e embosca | este é o mais perto no **conceito** (emboscada disfarçada de arbusto). O twig blight é humanoide pequeno com garras; o feixe não tem membros superiores, ataca em linha com a coroa e, disperso, volta a ser espinheiro. A regra (miolo, fecha o buraco, segura o fio) é só daqui |
| Ferrothorn (Pokémon) | três apêndices e espinhos | lá é uma bola de metal pendurada em três vinhas; aqui são três pernas de galho em pé e a bola é o alvo pendurado, de raiz, sem espinho |
| Tripés de *A Guerra dos Mundos* | tripé alto com corpo pendurado | o risco é da forma 2 lida em preto. Afasta: galho torto, coroa de espinhos no alto, nenhum corpo no topo (o miolo fica no meio, baixo) |
| Totens de graveto de *A Bruxa de Blair* | feixe de galhos amarrados com ar sinistro | nenhum símbolo, nenhuma figura humana; é planta, com cor de espinheiro comum |
| Deku Baba (*Zelda*), plantas carnívoras | planta que ataca quem passa | não tem boca nem caule; o bote é o corpo inteiro inclinando |

**Maior risco:** o conceito de "arbusto disfarçado que embosca" (twig blight). A defesa é a forma (tripé sem braço, alvo pendurado) e a regra que só o COE tem: ele fecha o buraco por onde se passa e guarda nos espinhos o que passou.

## 8. Ligações com outras fichas e com o elenco

- **`tovin`:** o espinheiro "fecha de novo" (C2 dele) ganha o porquê. A fala "esse aí não é mato" (C9) é PROPOSTA para o redator; não muda a ficha dele. O estojo de estacas na canela esquerda do Tovin (Arbitragem 3) também é um feixe de varas, mas pequeno (0,08–0,40 m), reto, de freixo claro e preso a uma pessoa: não disputa forma com uma criatura de 1,30 m sozinha, e as cores ficam separadas (madeira clara contra casca escura).
- **`nilo`:** o buraco e o fio (`EP2` B11). A forquilha em Y do Nilo e o tripé em Λ do feixe podem rimar em preto; o Y é de criança, nas costas, com a cabeça ao lado, e o Λ é a figura inteira. O G2 confere os dois na mesma folha (§6).
- **`maelis`:** a vara de ofício (um I de 4 cm) e as pernas do tripé (Ø 0,05 m) têm a mesma espessura, mas a vara é uma só e reta, ao lado de uma pessoa. Sem conflito.
- **Paleta:** a correção da casca (§6) protege a regra do ADR-0010, item 4 (cobre só na marca do jogador).
