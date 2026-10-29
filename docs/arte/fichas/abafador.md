# Ficha G1 — `abafador`

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md)). Criatura do episódio 2 ([`EPISODIO2_DESPERTAR_DOS_TALENTOS.md`](../../design/EPISODIO2_DESPERTAR_DOS_TALENTOS.md), abreviado `EP2`), que é **PROPOSTA inteira** (o roteiro não foi aprovado). Esta ficha herda esse estado: nada aqui vale antes de o idealizador aprovar o roteiro **e** o Art Director dar a nota.
> Fato com fonte é cânone ou está no `EP2` (marcado `EP2 §…`). **PROPOSTA** = ideia desta ficha, que não está em doc nenhum.
> Criatura não fala nem tem destino próprio: C6, C7 e C8 foram escritos para personagem. Onde a ficha adapta, diz como, e o pontuador decide (o ADR não define N/A).

## 1. Identificação

- **id:** `abafador` (no código, `criatura.abafador`, `EP2` §3.4). O id passa no regex do `PIPELINE.md` §6 e não contém nenhuma chave do `HumanoidMapping`.
- **categoria:** criatura. O `_MODELO_G1.md` não tem a categoria; para orçamento, ver §6 (Prop pequeno).
- **onde aparece:** episódio 2, não o slice. E2-B03 (`q09_a_linha_do_posto`, encontro `enc.trilha_abafadores_borda`, a primeira luta do jogo); E2-B06 (`q12`, o abafador manso na porta da `ervanaria`, só com `evento.q05_concluida`); E2-B14 (`q15`, `enc.trilha_misto`, com feixes); E2-B15 (`q16`, o bando em volta do Enredado faz o silêncio do `bosque_coracao`). Âncoras: `bosque_trilha`, `bosque_coracao`, porta da `ervanaria`.
- **cânone de partida:**
  - O Bosque dos Sussurros abafa o som: grito não atravessa as árvores, o chifre atravessa (ficha `tovin`, C2, aprovada no G1 por delegação).
  - Nilo, aos 8, lembra "um círculo de luz e um silêncio enorme" (`dialogo.nilo.aos_oito*`, `strings.pt-BR.json`).
  - Lysa, aos 8, com a q05: "Aquele bichinho que cuidamos juntos ainda aparece aqui na porta…" (fala `aos_oito_animal`). Na q05 o bicho fica sob o chapéu emborcado dela, porque "escuro acalma", e só se acalma com a criança parada a até 1,5 m por ~3 s (ADR-0010, adendo, item 10; `BichoNoChapeu.cs`, regra `ChegarDevagar`). A ficha `lysa` (C9) diz que pôr o bicho em cena "pede uma criatura com G1 própria": é esta.
  - `EP2` §3.4: bola achatada de folha seca e musgo de 0,45 m; leque de folhas que abre até 0,70 m quando alarmado; sem pernas, anda em pulos rasos; vem em 3 a 5; perto do bando, o áudio cai e o HUD diz "silêncio"; um por vez abre o leque (0,6 s, o aviso) e salta; quando sobra um, ele foge; `peso` 1; Natural ≥ 1 dispersa o bando com 3 s parado. "Derrotar = dispersar": não morre em cena, não deixa item, moeda nem XP (ADR-0009; GDD cap. 05).
- **autor da ficha / data:** Concept Art Lead (agente), 2026-10-29
- **estado:** aguardando nota (autoavaliação na §5; **pendente de nota do Art Director separado, ADR-0010 §6**)

## 2. Critérios C1–C10

**C1 — Gancho.** Um monte de folha seca do tamanho de uma abóbora que, quando se assusta, levanta atrás de si um leque maior que ele, e o bosque inteiro fica mudo: a primeira coisa que a criança enfrenta com uma espada é o mesmo bicho que dorme na porta da ervanaria. *(o bicho manso é `EP2` §0; o resto é `EP2` §3.4 com o desenho desta ficha)*

**C2 — Necessidade do mundo.** O bosque abafa o som (ficha `tovin`, C2): o abafador é o porquê, e por isso Auren tem um chifre, e não um sino, na borda. **PROPOSTA:** o leque come o som agudo e deixa passar o grave; por isso grito não atravessa e o chifre do Tovin, que é grave, atravessa. Ele também dá corpo ao "silêncio enorme" do Nilo (cânone), sem explicar o sumiço: no coração do bosque, o silêncio era o bando juntado em volta do Enredado (`EP2` B15). E é o bicho ferido da q05, que aos 5 só podia ser tratado chegando devagar, porque o abafador assustado vira leque e salta (ADR-0010, item 10). Não é bicho de qualquer bosque: existe porque este bosque se chama dos Sussurros.

**C3 — Silhueta em 3 formas.** *(medidas PROPOSTA, a partir do `EP2` §3.4)* Vista canônica: **de lado**, que é como a câmera de terceira pessoa o vê quando o bando cerca a criança.
1. **Monte:** bola achatada de folha seca, 0,45 m de largura × 0,30 m de altura, base chata no chão, sem pescoço nem cabeça destacada. Parado, lê como um monte de folhas, de propósito (o bando parado é um canteiro de montes).
2. **Leque:** fechado, deitado sobre o dorso como uma crista de 0,30 m, que sai 0,08 m atrás do monte. Aberto: semicírculo **cheio**, de pé atrás do corpo, 0,70 m de largura × 0,50 m de altura, com 7 nervuras radiais que passam da borda em pontas de 3 cm. A caixa aberta tem 0,80 m de altura: o bicho fica quase três vezes mais alto. É a forma que avisa o ataque.
3. **Pulo raso:** sem pernas; no salto o monte inteiro descola do chão até 0,25 m, achatado em gota (0,50 × 0,22 m), com o leque aberto atrás como vela. Em preto: um vão de luz entre o bicho e o chão. É a única criatura do elenco que tira o corpo todo do chão.
- *T-pose:* não se aplica (sem esqueleto, §6). *Câmera:* a criança de 11 tem ~1,42 m (HIPÓTESE, `EP2` §4.2) e a câmera fica ~3,5 m atrás dela (2,5 × a altura, `ThirdPersonCamera`): o monte fechado ocupa ~1/3 da altura da criança e lê pouco; o leque aberto passa da cintura dela e lê bem. **Fraqueza declarada:** fechado e de frente, é só um monte. É assumido: o perigo é o leque.

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **O leque, com os dois olhos nele.** O monte não tem rosto: os "dois pontos claros de olho" do `EP2` §3.4 passam do corpo para a base do leque, um de cada lado do eixo, em marfim #E9DEC6, Ø 0,06 m, cobertos quando o leque está fechado. O rosto só aparece no aviso.
- **O jogo lê:** o estado do leque de cada abafador (`fechado`, `aberto`, que dura 0,6 s antes do salto, ou `fugindo`) e a distância da criança ao abafador mais próximo do bando. A até 8 m de qualquer abafador do bando (HIPÓTESE v0), o grupo de áudio de ambiente e de efeitos passa por um passa-baixa (HIPÓTESE: corte em ~800 Hz), e o HUD escreve "silêncio" em texto (`EP2` §3.4; nada depende só de som, GDD cap. 09).
- **O jogador vê:** um leque de cada vez se abre, e a criatura que abriu é a que salta. O olho marfim aparecendo é o aviso. Com o `assist.aviso_longo`, os 0,6 s passam a 0,9 s (`EP2` §3.5). Quando o bando dispersa, o som volta de uma vez, sem rampa.
- Medidas: leque de 0,70 × 0,50 m, 7 nervuras; olhos de Ø 0,06 m (11 px a 30% numa folha de 512 px de altura com o bicho aberto em 80%; acima do corte de 7 px do `silhueta.py`).

**C5 — Regra exclusiva.** É a única criatura que muda o que o jogador ouve, e a única que se vence sem lutar. Natural ≥ 1: ficar parado 3 s perto do bando o dispersa, e o encontro conta como vencido (`EP2` §3.3). É o "chegar devagar" da q05 (ficha `lysa`, C5) crescido: a regra que aos 5 acalmava um bicho, aos 8 resolve um encontro. Correr a menos de 4 m faz um leque se abrir na hora (PROPOSTA: a mesma distância do `ChegarDevagar`, para que a regra seja uma só). Quando sobra um, ele foge: não há último golpe para farmar (`EP2` B03).

**C6 — Voz.** *(adaptação: criatura não fala; o pontuador decide se três sons escritos contam.)* PROPOSTA, para o áudio:
1. Fechado: um farfalhar seco a cada ~2 s, como alguém pisando em folha, mas sem ninguém pisando: é o único som que sobra dentro do silêncio dele.
2. Leque abrindo: um sopro curto e abafado, "fff", de 0,6 s, que soa **antes** do passa-baixa, a única coisa nítida no silêncio. É o aviso sonoro, e repete o aviso visual (o olho).
3. Dispersar: nenhum som do bicho. O que se ouve é o bosque voltando de uma vez: pássaro, vento, os passos da criança. A voz dele é o som que ele devolve.

**C7 — Contradição visível.** *(PROPOSTA)* O bicho que tira o som do bosque é o que mais se assusta com barulho. É observável em toda luta: quem chega correndo faz o leque abrir e leva o salto; quem para, vê o bando se recolher. E a criança que aprendeu a lutar com ele na trilha o encontra, na mesma semana, dormindo na soleira da `ervanaria` (só com a q05; `EP2` B06), com o leque fechado e sem silêncio nenhum em volta: o manso não abafa.

**C8 — Relação com o avatar por destino e origem.** *(PROPOSTA; ADR-0004: destino nunca muda número de criatura nem de recompensa, e aqui não muda.)*
- **serena, normal, dificil:** o abafador é igual. O que muda é se ele já é conhecido: com `evento.q05_concluida` (qualquer destino), a criança de 8 chega à q09 sabendo que o bicho da trilha é o da porta, e Lysa diz isso na q12. Sem a q05, a primeira vez é na luta.
- **ruptura:** igual em número e em forma. A criança, que viu a luz violeta na clareira (E2-B07), ouve o silêncio da trilha como o da janela daquela noite; é fala, não regra.
- **origem:** igual. Sincero: ramifica pela q05, não pelos quatro destinos. Pela régua, isso é nota 1.

**C9 — Mudança no tempo.** O episódio salta dos 8 para os 11 (`EP2` B09), não dos 5 para os 8; a ficha marca as duas batidas desse salto.
- **Antes (8):** na borda da trilha, de dia, um bando de 3 (`enc.trilha_abafadores_borda`), com Tovin atrás. O leque é mais alto que a cintura da criança de 1,28 m. O manso dorme na porta da `ervanaria` (só com a q05).
- **Depois (11):** à noite, na trilha cheia, bando de 5 misturado com feixes (`enc.trilha_misto`). Com a criança de ~1,42 m, o leque aberto fica na altura do quadril dela. No coração, os bandos ficam em anel em volta do Enredado e não atacam: só calam o lugar (`EP2` B15). O manso continua na porta, um pouco maior de leque (PROPOSTA: escala de leque 1,1, sem malha nova).
- **Variante de malha:** não. Muda número, hora e o que ele faz.

**C10 — Momento de cartaz.** *(PROPOSTA)* Trilha estreita entre o espinheiro, de dia. A criança de 8, com a espada de madeira, passa entre quatro montes de folha. O som do bosque cai; no alto da tela, "silêncio". Um dos montes ergue atrás de si um leque maior que ele, com dois olhos claros, e salta raso. A criança esquiva para o lado. Ela para, respira, e os montes vão embora em pulinhos. O passarinho volta a cantar.

## 3. Amarração

- **Destino, Grau ou Trama:** nenhum o altera (C8). A ligação com a Trama é indireta e só aparece no fim: o bando é o silêncio que guarda o Enredado (`EP2` B15), e é o "silêncio enorme" do Nilo. Não explica o sumiço.
- **Decisão de jogo que ele cria:** lutar ou ficar parado. Quem treinou Natural pode parar e vencer sem golpe; quem não treinou luta. As duas fecham o encontro (`EP2` §3.3).
- **Pilar:** viver e crescer (a mesma regra de chegar devagar dos 5 volta maior aos 8).

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| bolinha fofa de rosto grande e olhos de brilho, que pede abraço | monte sem rosto: os olhos só aparecem no leque, e só no aviso |
| espírito da floresta pálido, translúcido, que balança a cabeça | folha seca e musgo opacos, cor de chão do bosque; nada brilha nele |
| bolinha preta de fuligem em enxame | corpo verde-musgo escuro e palha, e o bando é de 3 a 5, nunca enxame |
| bicho de folha com cara de máscara de folha e graveto na mão | sem membros, sem máscara, sem objeto; a única forma além do corpo é o leque |
| cauda de pavão, leque de penas coloridas com ocelos | 7 nervuras de folha larga em semicírculo cheio, só dois olhos na base, oliva e palha |
| criatura que ruge ou guincha para avisar | o aviso é visual (o olho no leque) e um sopro abafado; em volta dele o som cai |
| bicho que morre e larga moeda | se dispersa em pulos rasos e devolve o som ao bosque |

## 5. Avaliação

**Autoavaliação do autor (não vale como nota; ADR-0002 e ADR-0010 §6 pedem quem não escreveu):**

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 2 | 2 | 2 | 1 | 2 | 1 | 2 | 2 | **18/20** (16/20 se C6 for N/A e o corte for proporcional) |

- C6 = 1 por ser som, não fala; C8 = 1 porque ramifica pela q05, não pelos quatro destinos (o ADR-0004 proíbe criatura diferente por destino).
- **Pendente de nota do Art Director separado (ADR-0010).** Corte: ≥ 14/20 e nenhum zero em C2–C5 (HIPÓTESE do ADR).
- **Pontuador / data:**
- **Veredito:**
- **Observações do pontuador:**

## 6. Encaminhamento

**Para o G2 (concept), só depois de aprovado:**
- vistas: lado (canônica), frente, 3/4, de cima; três estados (fechado, leque aberto, pulo); ao lado, a criança de 1,28 m e a de ~1,42 m; linha de chão.
- silhueta: o ADR não tem teste para criatura. **Adaptação proposta:** os três estados em preto a 30%, embaralhados com o feixe-de-espinho, o lajeiro e um monte de folha comum; quem leu as fichas acerta ≥ 4 de 5, e o monte fechado **pode** ser confundido com o monte comum (é o disfarce), o aberto não.
- paleta: corpo Verde #648B67 sombreado para escuro (um tom abaixo, sem hex novo fora da família); leque oliva-palha (dessaturado do Verde em direção ao Marfim); olhos Marfim #E9DEC6. **Fora:** Turquesa e Violeta (não é magia nem anomalia), Dourado e Terracota.
- orçamento (`PIPELINE.md` §4, HIPÓTESE v0): categoria mais próxima **Prop pequeno** (maior dimensão < 1 m): 500 tris, 1 material, 256 px. Exceção pedida: projeta sombra (a regra do Prop pequeno manda `Cast Shadows = Off`), ou ganha uma sombra em decalque, porque o pulo raso só se lê com a sombra no chão; o G2 escolhe medindo. Bando de 5 = 2 500 tris.
- **rig:** sem esqueleto (`EP2` §7.1). Dois blend shapes (`leque`, 0 → 1; `achatar`, para o pulo e o golpe sofrido) e a animação do transform da raiz para o salto. Clips: `respirar` (laço, 2 s), `alarme` (0,6 s, leque abre, olhos aparecem), `pulo` (0,5 s), `golpeado` (0,2 s, achata), `fugir` (pulos em sequência, laço), `dormir` (o manso, leque fechado, laço). Sem clip de morte. Nomes sem chave do `HumanoidMapping` (o `idle` vira `respirar`). Pasta: o §6 do PIPELINE não tem categoria de criatura; proposta `Art/Criatura/abafador/`.
- código que a ficha pede (dependência, não feito aqui): passa-baixa por proximidade do bando no mixer; texto "silêncio" no HUD; olhos no leque como parte do blend shape.
- testes a registrar: silhueta adaptada, descrição, troco (trocar pelo feixe na q09: a luta deixa de ensinar a ler o grupo e o silêncio some, então não passa sem reescrita).

**Para o G3:** bloco `### abafador` em `PROVENIENCIA.md`, como no modelo.

## 7. Teste de originalidade honesto

Obras que ele pode lembrar, e o que o afasta (lista de revisão; **não** vai para o briefing do gerador, que usa só a §4):

| Pode lembrar | Por quê | O que afasta |
|---|---|---|
| Kodama (*Princesa Mononoke*) | espírito pequeno da floresta, em grupo | o abafador não é pálido nem translúcido, não tem cabeça nem chocalha; é folha seca opaca, e o que ele faz é tirar o som |
| Fuligens / *susuwatari* (Ghibli) | bolinha sem membros, dois olhos, em grupo | sem olho no corpo, verde e palha, bando de 3 a 5 que salta em ataque; o leque é a forma |
| Koroks (*Zelda: Breath of the Wild*) e Deku Scrubs | bicho de folha do bosque | sem máscara, sem membros e sem graveto; não se esconde para ser achado, ataca |
| Pokémon de folha redonda (Seedot, Oddish, Shroomish, Bounsweet) | corpo redondo com folhas | nenhum tem leque vertical que abre como aviso nem regra de som; e o abafador não tem rosto no corpo |
| Musgo-bichos de *Hollow Knight* (Mosscreep) | monte de musgo que é bicho | a forma de leitura é o leque, não o monte; o encontro se vence ficando parado |
| Chibi-Totoro | criatura pequena e redonda do bosque | sem orelhas, sem pelo, sem rosto; nenhuma cena de fofura, exceto o manso dormindo |

**Maior risco:** cair no "bichinho redondo com dois olhinhos" (Kodama, fuligem, Pokémon). A defesa é tirar o rosto do corpo e pô-lo no leque, que só se abre no aviso. Se o concept do G2 vier com olhos no monte, reprova.

## 8. Ligações com outras fichas e com o elenco

- **`lysa`:** é o bicho da q05 e o da porta aos 8 (fala `aos_oito_animal`). Conta: a copa do chapéu tem 0,10 m (ficha `lysa`, C4) e o monte tem 0,30 m, então o chapéu emborcado (ADR-0010, item 10) não o engole: fica pousado por cima, como tampa, e a aba de 0,72 m passa 0,13 m de cada lado do monte de 0,45 m e faz a sombra. "Escuro acalma" porque o leque não abre com peso em cima e no escuro (PROPOSTA). Aos 5 o bicho é o mesmo (sem variante de filhote): o monte é pequeno para a criança de 1,10 m também.
- **`tovin`:** o chifre atravessa porque é grave (C2). Não muda a ficha dele.
- **`nilo`:** o "silêncio enorme" é o bando em volta do Enredado. Não diz que Nilo esteve lá; o `EP2` B15 é que encena.
- **Zonas de silhueta:** semicírculo cheio e vertical (leque), atrás de um corpo sem pernas, abaixo de 0,80 m. Ninguém do elenco humano ocupa essa altura com essa forma; o anel do `simbolo_limiar` é vazado e de 1,20 m, o leque é cheio e de 0,70 m.
