# CHRONICLES OF EXISTENCE — Comparativo competitivo (2026-10-29)

**Status:** PROPOSTA. É uma pesquisa de mesa (web) com parecer de especialista. Nada aqui muda o GDD: o que for produto vira decisão do idealizador.
**Base:** `docs/gdd/GDD_MESTRE_v1_2.md` (cap. 01, 02, 03, 07, 11), `docs/design/SLICE_A_PRIMEIRA_EXISTENCIA.md`, `docs/design/PROPOSTA_MECANICAS_HELL_MODE.md`, `docs/PROJETO.md` §2 e os ADR-0004, 0006 e 0009.
**Convenções:**
- Número sem fonte conferida aparece como **não confirmado**.
- As estimativas de receita do AppGoblin são muito baixas para títulos conhecidos (ex.: Genshin com "US$200 mil/mês"), então **não são usadas**. Do AppGoblin só entram instalações e notas, que vêm da página da loja.
- Fato tem fonte, inferência é marcada com *(inferência)* e opinião com *(parecer)*.

**Escopo do COE neste documento:** RPG de ação narrativo 3D em terceira pessoa, fantasia medieval estilizada (toon/anime). Plataforma: Android primeiro, paisagem, toque. Single-player e offline. Público: crianças e adultos (ADR-0009). Fluxo: a alma escolhe 1 de 4 Destinos de Nascimento permanentes e 1 de 3 origens, nasce em Auren, é jogável aos 5 anos e salta para os 8 por marco narrativo. A reputação é por NPC, as missões têm consequência e o domínio cresce pela prática. O slice "A Primeira Existência" tem hipótese de 45–75 min.

---

## (a) Similares atuais

Escolhidos pelo que realmente fica perto do COE em pelo menos dois eixos: ação 3D no celular, crescer ou viver uma vida, e escolha com consequência. Ficaram de fora Avernum (RPG tático em turnos, longe do toque em tempo real) e jogos de "vida" 2D sem combate além do Stardew.

| Jogo | Estúdio | Plataforma | Modelo · preço | Números (fonte, data) | O que faz bem | O que os reviews reclamam |
|---|---|---|---|---|---|---|
| **Genshin Impact** | HoYoverse (miHoYo) | Android, iOS, PC, PS | F2P com gacha (personagens e armas), energia ("Resina") para recompensas | Mais de 218 milhões de instalações e mais de US$4,4 bi de IAP líquido no celular (AppMagic). A Niko Partners projeta mais de US$10 bi de gasto total em 2025 ([WN Hub, 11/10/2024](https://wnhub.io/news/finance/item-45753)). Google Play: 145,5 mi de instalações, nota 4,03 com 5,2 mi de avaliações ([AppGoblin, lido em 29/10/2026](https://appgoblin.info/apps/com.miHoYo.GenshinImpact)) | O padrão de "RPG de ação 3D anime no celular": mundo bonito, combate com troca de personagem, controles de toque maduros | "Jogo que suga dinheiro", "pode gastar e não ganhar o que quer", banner de arma "é golpe", espera longa por reprise de personagem, missões longas demais e pedido de pular cenas ([OpenCritic, reviews de 09–14/09/2026](https://opencritic.com/game/10217/genshin-impact/user-reviews?page=2)). Instalação de **46 GB** no celular (v4.6) ([GGRecon, 07/05/2024](https://www.ggrecon.com/guides/genshin-impact-storage-pc-mobile-playstation/)) |
| **Wuthering Waves** | Kuro Games | Android, iOS, PC, PS5 | F2P com gacha | US$317,3 mi no celular no 1º ano (AppMagic) ([PocketGamer.biz, 09/06/2025](https://www.pocketgamer.biz/hot-five-wuthering-waves-makes-3173m-in-first-year-on-mobile-scopely-closes-35bn-niantic-deal-and-epic-games-store-hits-40m-mobile-installs)) | Mostra que um 2º gacha 3D de ação ainda cabe no mercado, com combate mais rápido | Reclamações específicas **não coletadas** nesta pesquisa. A Niko liga a queda do Genshin à "concorrência crescente no gênero RPG" ([WN Hub, 2024](https://wnhub.io/news/finance/item-45753)) |
| **Grimvalor** | Direlight | Android, iOS | **Grátis + desbloqueio único**: 1º ato (de 5) grátis com anúncio, depois US$6,99 ([Android Central, 14/06/2019](https://www.androidcentral.com/grimvalor-beautifully-polished-platformer-you-can-try-out-free-game-week)). No iOS era US$5,99 pago, sem IAP nem anúncio ([TouchArcade, 11/10/2018](https://toucharcade.com/2018/10/11/grimvalor-review/)) | Google Play: 6,5 mi+ de instalações, nota 4,71 com 133 mil avaliações; atualizado em 18/08/2026 ([AppGoblin, lido em 29/10/2026](https://appgoblin.info/apps/com.direlight.grimvalor)). Receita: **não confirmado** | Ação de toque polida; o 1º ato grátis leva 1–2 h e vende o resto | História direta "abaixo do resto do jogo", colisão "grudenta" em paredes, trechos muito punitivos ([TouchArcade](https://toucharcade.com/2018/10/11/grimvalor-review/); [Android Central](https://www.androidcentral.com/grimvalor-beautifully-polished-platformer-you-can-try-out-free-game-week)) |
| **Oceanhorn** | Cornfox & Bros. | iOS (2013), Android (15/12/2016), PC, consoles | **Grátis + desbloqueio**: 1º capítulo grátis, desbloqueio a US$5,49 no Android ([Android Police, 15/12/2016](https://www.androidpolice.com/2016/12/15/oceanhorn-a-zelda-inspired-premium-action-adventure-game-sets-sail-for-the-play-store/)) | Mais de 1 milhão de cópias em todas as plataformas até out/2016; Metacritic iOS 85 ([Wikipedia](https://en.wikipedia.org/wiki/Oceanhorn:_Monster_of_Uncharted_Seas)). Instalações no Android: **não confirmado** | O "Zelda de celular": aventura 3D com espada e escudo, premium, que funciona no toque | Críticas aos quebra-cabeças e à história; a nota cai fora do celular (PS4/Switch 66) ([Wikipedia](https://en.wikipedia.org/wiki/Oceanhorn:_Monster_of_Uncharted_Seas)) |
| **Dragon Quest V** (port mobile) | Square Enix (original Chunsoft/ArtePiazza) | Android, iOS (22/01/2015) | **Premium**: US$14,99 sem IAP ([Android Police, 22/01/2015](https://www.androidpolice.com/2015/01/22/dragon-quest-v-raids-play-store-party-full-monsters-demanding-14-99/)); £14,99 no iOS UK ([App Store](https://apps.apple.com/gb/app/dragon-quest-v/id933113994)) | Vendas do mobile: **não confirmado** (o remake de PS2 vendeu 1,64 mi até 2018, [dragon-quest.org](https://dragon-quest.org/wiki/Dragon_Quest_V)) | **O precedente de "crescer no jogo"**: o herói vai da infância à vida adulta e à paternidade, com saltos de tempo ([dragon-quest.org](https://dragon-quest.org/wiki/Dragon_Quest_V)). Nota 4,5/5 ([Pocket Gamer, 27/01/2015](https://www.pocketgamer.com/dragon-quest-v/review/)) | Save antiquado: sair do app no meio da masmorra perde progresso; muito grind de dinheiro e XP ([Pocket Gamer](https://www.pocketgamer.com/dragon-quest-v/review/)). Combate em turnos, sem ação |
| **Eternium** | Making Fun | Android, iOS | F2P **sem energia** ("no limiting stamina or energy"); offline depois do download, salvo "algumas funções online"; gemas e IAP de £3,09 a £20,49; a classificação etária lista "Loot Boxes" ([App Store UK, lido em 29/10/2026](https://apps.apple.com/gb/app/eternium/id579931356)) | Google Play: 26,3 mi+ de instalações, nota 4,79 com 2,7 mi de avaliações ([AppGoblin](https://appgoblin.info/apps/com.makingfun.mageandminions)) | Prova que "ARPG de celular sem energia e jogável offline" vira lealdade alta (nota 4,8–4,9) | Ato 2 com mapas bugados, missões secundárias movidas para depois da história, "os devs seguiram em frente" (review de 21/11/2023); anúncio repetitivo por gemas (2018) ([App Store UK](https://apps.apple.com/gb/app/eternium/id579931356)) |
| **BitLife** | Candywriter | Android, iOS | F2P com anúncio + IAP (Bitizenship, God Mode) | Google Play: 61,4 mi de instalações, nota 4,36 com 1,3 mi de avaliações; atualizado em 21/10/2026 ([AppGoblin](https://appgoblin.info/apps/com.candywriter.bitlife)) | **A meta de "viver uma vida"** em massa: nascer, crescer, decidir, morrer; cada vida é uma história para contar | Anúncio mesmo depois de pagar o "sem anúncio", recurso (ex.: gangue) trancado atrás da Bitizenship, restauração de compra falhando, carreiras e eventos que se repetem ([ComplaintsBoard](https://www.complaintsboard.com/bitlife-life-simulator-b150146), uma reclamação datada de 29/11/2023). É só texto: não há mundo nem corpo |
| **Stardew Valley** (mobile) | ConcernedApe (port Chucklefish) | Android, iOS, PC, consoles | **Premium** sem IAP nem anúncio; ₱299 no iOS Filipinas ([App Store](https://apps.apple.com/ph/app/stardew-valley/id1406710800)) | Mais de 50 mi de unidades em todas as plataformas (fev/2026) ([VGChartz, 27/02/2026](https://www.vgchartz.com/article/467162/stardew-valley-sales-top-50-million-units/)). Google Play: 7,2 mi+ de instalações **pagas**, nota 4,64 com 203 mil avaliações ([AppGoblin](https://appgoblin.info/apps/com.chucklefish.stardewvalley)) | Rotina de vila, NPCs com relação e calendário: o "life sim" que o público já sabe jogar no celular | Deslizar para andar lento e sem resposta desde ~2022, com pedido de joystick configurável ([fórum oficial, 14/08/2026](https://forums.stardewvalley.net/threads/touch-control-delay-responsiveness-issue-on-android-v1-6-15-3.54450/)); toque que atravessa botões e ataque automático instável ([App Store PH](https://apps.apple.com/ph/app/stardew-valley/id1406710800)) |
| **Fable** (2027, referência de PC/console) | Playground Games / Xbox Game Studios | PC, Xbox Series, PS5 (não é mobile) | Premium (preço **não confirmado**) | Lançamento adiado de "outono de 2026" para **23/02/2027**, anúncio de 29/05/2026 ([Wikipedia](https://en.wikipedia.org/wiki/Fable_(2026_video_game)); [Pure Xbox, 28/08/2026](https://www.purexbox.com/news/2026/08/fable-director-reveals-original-october-2026-release-plans-but-theyre-glad-for-the-delay)) | **O concorrente de conceito mais próximo**: começa com o herói criança na vila, salto de tempo para a vida adulta, reputação e moral vistas pelos outros ([Wikipedia](https://en.wikipedia.org/wiki/Fable_(2026_video_game))) | Não lançado; sem reviews. A série é a referência de sensação do próprio GDD (cap. 01) |

### Contexto de mercado (fatos que pesam na decisão)
- Em 2025 o mercado de jogos mobile somou US$82 bi (+1%) e 50 bi de instalações (−7%). **99% dos downloads no Google Play não custam nada na entrada** ([Sensor Tower via Outlook Respawn, 28/02/2026](https://respawn.outlookindia.com/gaming/gaming-news/mobile-games-82-billion-revenue-2025-sensor-tower-report)).
- Brasil: 88,8% dizem que jogo digital é uma das principais formas de entretenimento, e o smartphone é a plataforma principal de **40,8%**, 8 p.p. a menos que na edição anterior ([PGB 2025 via Propmark, 26/03/2025](https://propmark.com.br/digital/pgb-2025-mulheres-superam-os-homens-e-sao-as-que-mais-consomem-jogos-digitais/)).
- O ECA Digital (Lei 15.211/2025, sancionada em 17/09/2025, em vigor desde março/2026) **proíbe loot box** em jogo destinado a menores ou de acesso provável por eles, e proíbe publicidade direcionada por perfilamento a crianças e adolescentes ([Demarest](https://www.demarest.com.br/protecao-de-criancas-e-adolescentes-no-ambiente-digital/); resumo de escritório, não é parecer jurídico).
- Google Play, Famílias: um app com crianças no público e anúncios só pode servir anúncio a criança por SDK autocertificado. Um público misto pede tela neutra de idade ([Play Console Help](https://support.google.com/googleplay/android-developer/answer/9283445), lido em 29/10/2026).

---

## (b) Matriz de recursos

Legenda: **sim** · **não** · **parcial** · **?** (não confirmado nesta pesquisa). Na coluna do COE vale o que o GDD e o slice definem (o slice ainda não foi jogado por ninguém de fora).

| Recurso | COE | Genshin | WuWa | Grimvalor | Oceanhorn | DQ V | Eternium | BitLife | Stardew | Fable (2027) |
|---|---|---|---|---|---|---|---|---|---|---|
| **Controle**: ação em tempo real no toque, paisagem | sim (joystick + botões, ADR-0006) | sim | sim | sim (2D lateral) | sim | não (turnos) | parcial (gestos, isométrico) | não (menus) | parcial (toque com queixas) | não (gamepad) |
| **Loop**: missão → vínculo → treino → marco | sim | parcial (explorar/coletar/gacha) | parcial | não (fase → chefe) | parcial (ilha → masmorra) | parcial (masmorra → grind) | não (fase → loot) | sim (ano a ano em texto) | sim (dia a dia) | sim |
| **Meta/progressão** sem grind por abate (domínio pela prática, retorno decrescente) | sim (`Mastery`, GDD 03) | não (nível + gacha) | não | não (XP e equipamento) | parcial | não (grind de XP) | não (loot) | parcial (atributos por ação) | sim (habilidade por uso) | ? |
| **Monetização** sem item aleatório pago e sem energia | sim (guardrail) | não (gacha + Resina) | não | sim | sim | sim | parcial (sem energia, mas tem loot box) | parcial (sem sorteio pago, mas paywall e anúncio) | sim | sim |
| **Offline** completo | sim | não | não | sim | sim | sim | parcial | parcial | sim | ? |
| **Onboarding**: história começa no 1º minuto, sem tutorial de menu | sim (Limiar, B01) | parcial | parcial | sim | sim | parcial | parcial | sim | parcial | ? |
| **Arte**: 3D estilizado/anime | sim (toon URP, ADR-0008) | sim | sim | parcial (2,5D sombrio) | sim (cartunesco) | parcial (2D/3D retrô) | parcial | não | não (pixel 2D) | sim (realista estilizado) |
| **Narrativa com consequência** lembrada por NPC | sim (Q-04 ±20, memória social) | não (história fixa) | não | não | não | parcial (escolha de noiva) | não | parcial (textual) | parcial (amizade e casamento) | sim |
| **Crescer/envelhecer** com corpo e mundo mudando | sim (5 → 8, `BodyScale`) | não | não | não | não | sim (infância → adulto → pai) | não | sim (só texto) | não | sim (criança → adulto) |
| **Escolher as condições de nascimento** (permanente) | sim (4 destinos × 3 origens) | não | não | não | não | não | não | parcial (aleatório ou customizado) | não | ? |

---

## (c) O que a tabela diz

**Onde o mercado está lotado**
1. **RPG de ação 3D anime no celular = gacha.** Genshin e Wuthering Waves ocupam esse lugar com orçamento de centenas de milhões e instalação de dezenas de GB. Se o COE for lido como "Genshin menor", perde em tudo: escala, mundo, personagem *(parecer)*.
2. **ARPG de loot e fase** (Eternium, Grimvalor): mercado maduro, fiel e de nota alta, mas o loop é bater e pegar item. O COE não compete nisso.
3. **Simulador de vida em texto** (BitLife): 61 mi de instalações provam o apetite por "viver uma vida". O formato é texto com anúncio, e a reclamação é anúncio e paywall.

**Onde o mercado está vazio**
1. **Crescer em 3D, com corpo, num jogo de ação no celular.** Só DQ V (turnos, de 1992) e Fable (console, fev/2027) fazem a criança virar adulto em cena. No celular de toque, em tempo real, não achei nenhum *(inferência: ausência nesta pesquisa não prova ausência no mercado)*.
2. **Escolher como nascer.** Nenhum similar faz da condição de nascimento uma escolha deliberada, permanente e sem rótulo de dificuldade. O BitLife sorteia; o Fable escolhe moral depois.
3. **"Vila que lembra" no celular.** Consequência guardada por NPC aparece no Fable (console). No celular, a história dos gachas é fixa por design, porque todos os jogadores precisam viver o mesmo evento.
4. **RPG narrativo 3D leve, offline e sem gacha**, que caiba num Android simples (ADR-0009). O Genshin pede 46 GB; o público brasileiro de celular de entrada fica sem RPG 3D de história *(inferência)*.

**O que faria o COE perder**
- **A infância ser chata.** É o risco nº 1 (PROJETO §2, "que a infância seja divertida"). O anúncio vende "crescer"; se os 10 primeiros minutos forem buscar cesto, a retenção do D1 cai antes do salto. O próprio Grimvalor só vende o ato 2 porque o ato 1 é bom.
- **A escolha não ser percebida.** Se as 12 combinações parecerem reskins (H5), o diferencial C1 vira só texto de loja.
- **Comparação visual com o Genshin.** O mesmo estilo anime com menos orçamento gera "parece um Genshin pobre" nos comentários. A arte precisa de silhueta própria (portões do ADR-0002) e de escala de vila, não de mundo aberto.
- **Rodar mal no celular simples.** O controle de toque é a principal queixa até no Stardew. Toque ruim mais FPS baixo na faixa de entrada mata o jogo na avaliação da loja.
- **Pouco conteúdo no slice para cobrar.** Com 45–75 min, um desbloqueio pago precisa de mais episódio do que existe hoje (ver (f)).
- **O Fable lançar em fev/2027 e "dono" da ideia.** Ele valida o conceito, mas os criativos do COE vão ser comparados com ele. A defesa é "no seu celular, offline" *(parecer)*.

---

## (d) Diferenciais C1..C5

Nenhum similar junta os cinco. Para cada um: a reclamação ou lacuna que resolve, como aparece na tela nos **primeiros 3 minutos** e como vira criativo (verbo legível em 3 s). Onde o slice de hoje não entrega no minuto 3, o texto diz e propõe o ajuste **[PROPOSTA]**.

### C1. Escolha como você nasce (e é para sempre)
- **Resolve:** a lacuna "ninguém deixa escolher a condição de nascer" (BitLife sorteia; o Genshin dá um protagonista fixo). Também resolve o exploit "começa no fácil e troca", porque o destino não é dificuldade (ADR-0004).
- **Primeiros 3 min:** o Limiar (B01), com seis falas de Aethron e o símbolo da Trama sempre visível, e depois as 4 cartas de destino e as 3 origens (B02–B03). Um texto visível diz "esta escolha é permanente". **Já está no slice.**
- **Criativo:** "**Escolha como nascer.**" Um dedo toca 1 de 4 cartas (Serena, Normal, Árdua, Ruptura) e a carta vira a casa onde a criança acorda. Variante A/B: as 4 casas lado a lado.

### C2. Você joga a infância, e o corpo cresce
- **Resolve:** nenhum RPG de ação de celular deixa jogar criança e virar outra idade em cena. Só DQ V (turnos) e Fable (console) fazem isso. O BitLife cresce, mas em texto.
- **Primeiros 3 min:** acordar aos 5 anos em Auren (B06) com câmera baixa, porta alta e Mara e Daren olhando de cima (corpo de 1,10 m, `BodyScale`). **[PROPOSTA]** um "eco do futuro" de 2 s no Limiar, mostrando a silhueta de 8 anos treinando, promete o salto antes de jogar.
- **Criativo:** "**Cresça.**" No mesmo enquadramento da praça, a criança de 5 anos corre e, no corte, a de 8 segura a espada de madeira (1,10 m → 1,28 m). É um antes/depois legível sem som.

### C3. A vila lembra o que você prometeu
- **Resolve:** "a história não muda" dos gachas (OpenCritic do Genshin pede para pular cena, sinal de cena sem peso) e a consequência só textual do BitLife. A Q-04 já deixa Sera e Nilo com ±20 de confiança e uma fala diferente depois do salto.
- **Primeiros 3 min:** hoje a Q-04 vem depois. **[PROPOSTA]** uma fala de NPC no despertar que reconheça a origem escolhida ("Filho do ferreiro, já de pé?") prova que a escolha foi lida, e um aviso do tipo "Sera vai lembrar disso" na primeira decisão.
- **Criativo:** "**Cumpra ou quebre.**" Tela dividida: a promessa cumprida faz Sera acenar aos 8 anos; a quebrada faz Sera virar o rosto. Texto: "Ela vai lembrar."

### C4. Aprender fazendo, sem grind
- **Resolve:** o grind de XP e dinheiro (reclamação do DQ V) e o "XP por abate" de Eternium e Grimvalor. No COE, o domínio sobe por tarefa com retorno decrescente e teto (`Mastery`, GDD 03), e a referência Hell Mode inspira o HM-01 "Registro vivo" e o HM-06 "prática dentro da rotina" **[PROPOSTA]**.
- **Primeiros 3 min:** a primeira tarefa da casa (q01/q02) mostra "+ Agilidade" ou o domínio subindo na hora. **[PROPOSTA]** o anel do HM-01 desenhado no HUD na primeira tarefa.
- **Criativo:** "**Treine na vida.**" Carregar o cesto vira braço mais forte, que vira o primeiro golpe com espada de madeira no posto da guarda: três cortes de 1 s cada.

### C5. RPG 3D de história que roda no seu celular, offline, sem gacha e sem energia
- **Resolve:** o Genshin com 46 GB e gacha ("suga dinheiro"), o paywall e o anúncio do BitLife, e a lacuna de RPG 3D para Android de entrada (ADR-0009: Android 8, 2–3 GB, 30 FPS nas três faixas).
- **Primeiros 3 min:** abre sem login nem download extra, sem loja na primeira tela e sem moeda premium no HUD. **[PROPOSTA]** a frase "Sem sorteio pago. Sem energia. Sem internet." na página da loja e na tela de carregamento.
- **Criativo:** "**Jogue no modo avião.**" O dedo liga o modo avião e o jogo continua na vila. É o criativo de confiança para pais (público misto, ADR-0009).

**Os 3 que eu poria primeiro no teste de criativo:** C1 (escolha como nascer), C2 (cresça) e C3 (cumpra ou quebre). C4 e C5 sustentam a nota da loja e a conversão, mas não param o dedo no feed *(parecer)*.

**Posicionamento (rascunho):** para quem gosta de RPG e quer uma história que seja sua, Chronicles of Existence é o RPG de ação no celular em que **você escolhe como nascer, joga a infância e cresce**, e a vila lembra o que você fez. Sem gacha e sem internet.

---

## (e) Hipóteses e critérios que matam a ideia

As H1–H5 são as "HIPÓTESE A VALIDAR" do `PROJETO.md` §2, ganhando aqui um critério de morte. As H6–H9 são de mercado.

| ID | Hipótese | Como medir | Critério que mata (ou obriga a redesenhar) |
|---|---|---|---|
| H1 | O slice dura 45–75 min | Playtest com 5–8 pessoas, tempo até o salto (B14) | Mediana abaixo de 30 min (não sustenta cobrança, ver (f)) ou acima de 100 min com abandono antes do salto |
| H2 | A infância é divertida | Escala de diversão 1–5 por bloco e abandono por bloco | Mais de 40% dos jogadores param antes de B10 sem ser obrigados, ou nota média abaixo de 3 nos blocos de 5 anos |
| H3 | As escolhas são perceptíveis | Pergunta pós-teste: "o que mudou por causa de algo que você fez?" | Menos de 50% citam espontaneamente a promessa (Q-04) ou a reação de um NPC |
| H4 | O salto temporal é significativo | Reação no momento do salto e pergunta "o que você sentiu?" | Mais da metade descreve como "tela de carregamento" ou não percebe a mudança de corpo |
| H5 | 12 configurações produzem variação real | Dois jogadores com destinos diferentes comparam as sessões | Os jogadores não conseguem citar 3 diferenças concretas entre as duas partidas |
| H6 | "Escolha como nascer" para o dedo no feed | Lote de 6–9 vídeos (C1, C2, C3 × 2 variações + controle "ação anime genérica"); CPI e retenção do vídeo aos 3 s | Os criativos C1–C3 não batem o controle em CPI nem em retenção aos 3 s. Aí o gancho de produto não é o de marketing |
| H7 | Quem termina o episódio 1 paga o desbloqueio | Taxa de compra entre quem chega ao fim do slice (teste fechado ou Limited Release) | Conversão abaixo de 2% dos que terminam o ep. 1 (inferência a partir da meta de 1–3% citada pela Flippfly, [Game Developer, 13/11/2012](https://gamedeveloper.com/business/players-won-t-pay-just-because-they-like-your-game), dado antigo e de confiança baixa) |
| H8 | Roda a 30 FPS no Android de entrada | `PerfHud` no aparelho de referência do ADR-0009 | Abaixo de 25 FPS sustentados na faixa Baixa ou aquecimento que force corte; nesse caso o C5 não pode ser prometido |
| H9 | O toque não vira a principal queixa | Teste de controle com 5 pessoas e taxa de "não consegui fazer o que queria" | Mais de 30% de falhas de entrada no combate de treino (B15) |

---

## (f) Modelo de negócio da v1 de loja

**Guardrails:** nada aleatório pago (ECA Digital), sem energia, anúncio só não personalizado e rewarded opcional. Pesa também o público misto com crianças (ADR-0009), que traz SDK certificado de Famílias, tela neutra de idade e proibição de perfilamento.

| Modelo | A favor (com dado) | Contra (com dado) |
|---|---|---|
| **Premium pago** (ex.: R$ 19,90–29,90) | Stardew tem 7,2 mi+ de instalações **pagas** no Play; DQ V se sustenta a US$14,99. Sem anúncio nem compra, o público de crianças fica simples | 99% dos downloads do Play são grátis na entrada (Sensor Tower 2025). Um estúdio novo e sem marca não tem a vitrine que Stardew e DQ trazem de fora do celular. Sem teste grátis, o risco de "parece um Genshin pobre" vira reembolso |
| **Grátis + desbloqueio único** (episódio 1 grátis) | É exatamente o modelo de Grimvalor (ato 1 grátis, US$6,99, 6,5 mi+ instalações, nota 4,71) e de Oceanhorn (cap. 1 grátis, US$5,49; mais de 1 mi de cópias somando plataformas). Entra pelo funil dos 99% grátis. O slice "A Primeira Existência" **já é** um episódio com gancho (desaparecimento + símbolo da Trama + salto). É uma compra só, de conteúdo fixo e visível, compatível com o ECA Digital | Converte pouco: a meta citada de 1–3% é antiga. O desbloqueio exige conteúdo pago que ainda não existe (o slice acaba aos 8 anos). Precisa de restauração de compra impecável (queixa do BitLife) |
| **F2P com anúncio** (rewarded opcional) | Maior topo de funil. BitLife chega a 61 mi de instalações com anúncio + IAP | O anúncio no meio de uma história quebra a cena (o jogo é narrativo, não de sessão curta). Com crianças, o público exige SDK de Famílias e proíbe perfilamento, então o eCPM tende a ser menor *(inferência)*. "Anúncio mesmo depois de pagar" é a principal queixa do BitLife. Sem loop repetível, o rewarded não tem onde morar sem virar atalho de progresso, o que o GDD proíbe ("dinheiro não substitui requisito pessoal") |

**Recomendação: grátis + desbloqueio único, com o episódio 1 ("A Primeira Existência", 5 → 8 anos) grátis e completo, e sem anúncio na v1.**

Por quê:
1. É o modelo dos dois similares de ação premium que deram certo no Android **sem marca prévia** (Grimvalor e Oceanhorn), e encaixa no funil em que 99% dos downloads são grátis.
2. O slice já é desenhado como episódio fechado com gancho (B11–B14). O "fim do ep. 1" vira o ponto natural de compra, sem paywall no meio da história e sem tirar algo que o jogador começou.
3. Cumpre os guardrails com folga: uma compra de conteúdo fixo, nada aleatório, sem energia. Sem anúncio na v1, some a carga da política de Famílias para SDK de anúncio e o risco do ECA Digital com perfilamento. O rewarded pode entrar depois, se houver um loop que não atalhe progresso.
4. Mantém a promessa do C5 ("sem gacha, sem energia, sem internet") limpa na loja.

**Condições para a recomendação valer:**
- **Conteúdo pago:** só publicar com pelo menos o episódio 2 pronto (8 → 11 anos, "Despertar dos Talentos", GDD 03). Desbloqueio sem conteúdo atrás é reembolso e nota 1.
- **Preço:** preço regional (Brasil) de referência abaixo do Grimvalor em dólar, a validar no teste H7. Valor final **não confirmado**: depende do tamanho do ep. 2.
- **Fallback:** se a H7 ficar abaixo de 2% no Limited Release, testar premium pago com preço baixo antes de considerar anúncio. O anúncio fica por último porque briga com a narrativa e com o público infantil.

---

## Lacunas desta revisão
- Receita de Grimvalor, Oceanhorn, DQ V mobile, Eternium e BitLife: **não confirmado** (as estimativas do AppGoblin não são confiáveis e não foram usadas).
- Reclamações do Wuthering Waves e do Genshin no Play Store BR não foram coletadas; as do Genshin vêm de uma página do OpenCritic (amostra pequena).
- Preço regional no Brasil dos similares premium não foi conferido.
- Nenhum dado de conversão de "free-to-try" recente e público foi achado; o número de 1–3% é de 2012.
- Leitura jurídica do ECA Digital veio de resumo de escritório; precisa de parecer antes de declarar o público na loja (ADR-0009 §3).
