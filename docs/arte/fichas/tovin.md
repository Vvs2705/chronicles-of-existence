# Ficha G1 — `tovin`

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md)).
> Fato com fonte é cânone. **PROPOSTA** = ideia desta ficha, não está nos docs e não vale até ser aprovada.

## 1. Identificação

- **id:** `tovin` (`NpcCatalog.cs`; GDD cap. 06, NPC-05)
- **categoria:** Npc, base adulta (`PIPELINE.md` §3.1, faixa 1,55–1,95 m)
- **onde aparece no slice:** B07 (cotidiano); q05, objetivo `tratar_o_animal`, com Lysa (`q05_o_animal_ferido.json`); B09, q07, objetivo `seguir_ate_o_bosque` ("Seguir com Tovin até o bosque", `q07_o_desaparecimento.json`); B14 (fala de depois do salto); B15, treino supervisionado no `posto_guarda` (`SLICE` B15). Âncoras: `posto_guarda` de manhã e de noite, `entrada_bosque` à tarde (`NpcCatalog.cs`)
- **cânone de partida:**
  - guarda e caçador; segurança e sobrevivência (dossiê §G); "guarda, caça e observação" (GDD cap. 06); traço `vigilante`; só sabe de `topico.caca`, `topico.seguranca_da_vila` e `topico.bosque`, não sabe do Limiar (`NpcCatalog.cs`)
  - vínculos: parceiro de bosque de Lysa; subordinado de Maelis (em `tovin`, `Vinculo("maelis", "relacao.subordinado")`; em `maelis`, `"relacao.superiora"`)
  - lembra de `evento.q07_concluida` (junto com Maelis) e do salto. **Não** testemunha a q05, embora esteja no objetivo (`NpcMemory.cs`: a q05 é só de Lysa), então nenhuma fala dele pode citar o animal. A q07 é central: todo save que chega aos 8 tem `evento.q07_concluida` (`SLICE` §3)
  - B15: sem `confianca_de_borin`, a espada de madeira comum vem do próprio treino, no `posto_guarda`; com a flag, Borin entrega a marcada (plaqueta de cobre no punho), com estatística idêntica (ADR-0010 §3–4)
  - falas já escritas (`strings.pt-BR.json`): `no_posto` ("Daqui eu vejo o portão e a trilha: quem entra e quem sai."), `sobre_bosque` ("Até a entrada, pode. Depois dela, só comigo. O bosque não é mau. Ele só não avisa."), `noite` ("…Criança na rua a esta hora eu levo pela mão até a porta de casa."), `rastro_no_bosque` ("…Nenhuma voltando. Se vier comigo, pisa onde eu pisar."), `depois_da_busca` ("O rastro acabou na clareira, e eu volto lá todo dia. Você foi até a beira comigo e não correu…"), `aos_oito` ("…E ninguém volta ao bosque sozinho, entendeu?"). Lysa: "Da entrada para dentro, nem eu vou sem o Tovin." Maelis: "Não quero alarde antes de ter certeza."
  - mapa: Auren é uma rua só, do `portao_sul` (z = −70) à entrada do bosque (z = 60); o posto fica nessa rua, "na estrada norte" (z = 34); a borda do bosque é barreira na linha z = 62, com um vão de 6 m (`AurenSceneBuilder.cs`; `SLICE` B10)
  - a origem `guardioes` traz `item.espada_de_madeira` e `oportunidade.ronda_com_tovin`, que é só um id, sem conteúdo (`DestinyCatalog.cs`)
  - concept antigo (#124–#158 do `ACERVO.csv`, não é cânone): túnica verde de caça, arco, aljava, braçadeiras, distintivo; pele retinta, cabelo crespo curto, barba. Protótipo Tripo (`PROVENIENCIA.md` §6): gibão cinza, ombreira e braçadeiras, que é o mesmo kit do `parceiro_treino`
- **autor da ficha / data:** Concept Art Lead (agente), 2026-10-03
- **estado:** aprovado G1 por delegação (ADR-0010, 2026-10-03)

## 2. Critérios C1–C10

| # | Pergunta | 0 | 1 | 2 |
|---|---|---|---|---|
| C1 | **Gancho em uma frase**, com detalhe concreto e tensão | não existe | descreve função ou cargo | detalhe concreto + conflito |
| C2 | **Necessidade do mundo** que o originou: que condição de Eryndor/Valtheris/Eldoria/Auren o fez existir? | nenhuma | genérica, serviria em qualquer RPG | específica de Eryndor/Auren |
| C3 | **Silhueta própria**: reconhecível em preto a 30% de escala | nada | lista de peças de roupa | 3 formas dominantes nomeadas |
| C4 | **Objeto-assinatura com regra**: um objeto que só ele tem e que faz algo no jogo | nenhum | objeto decorativo | objeto + regra de jogo escrita |
| C5 | **Regra ou habilidade exclusiva**: algo que só ele faz e o jogador vê | nada | kit genérico do catálogo | técnica exclusiva especificada |
| C6 | **Voz**: 3 falas que só ele diria (ritmo, vocabulário, tique) | nenhuma | tom descrito em adjetivos | 3 falas escritas |
| C7 | **Contradição visível em ação recorrente** | nenhuma | abstrata na ficha | observável no comportamento |
| C8 | **Relação concreta com o avatar** que muda por destino/origem | nenhuma | uma linha genérica | concreta e ramificada pelos 4 destinos |
| C9 | **Mudança no tempo**: como muda no salto dos 5 para os 8 anos | nenhuma | "vai mudar" | 2 batidas marcadas, antes e depois |
| C10 | **Momento de cartaz**: a cena que o vende em 5 segundos | nenhuma | cena implícita | cena escrita e encenável |

**C1 — Gancho.** Todo anoitecer Tovin toca o chifre para chamar Auren para dentro e conta quem voltou do bosque. Nas noites em que Nilo não voltou, ele leva o chifre à boca e baixa sem tocar, porque Maelis pediu silêncio. *(PROPOSTA; o pedido de silêncio vem da fala `dialogo.maelis.ausencia`)*

**C2 — Necessidade do mundo.** Cânone: Auren é uma rua só, entre o portão sul e um bosque que "não avisa"; o posto fica nela, em z = 34, a 28 m da linha das árvores (z = 62, `AurenSceneBuilder.cs`), e dali Tovin vê o portão e a trilha; a borda do bosque só se atravessa por um vão de 6 m; é no bosque que ficam a anomalia da Ruptura (`evento.anomalia_no_bosque`, `DestinyCatalog.cs`) e o sumiço de Nilo (ADR-0005; ADR-0007 §3). **PROPOSTA:** a borda do Bosque dos Sussurros é espinheiro fechado, e o vão é a única saída de Auren para o norte. A caça sai por ali e o perigo entra por ali, por isso guarda e caçador são um ofício só: quem caça no vão é quem vigia o vão. E o bosque faz jus ao nome: abafa o som. Grito não atravessa as árvores; chifre atravessa. É a única voz alta de Auren, e é dele. O vão só existe porque alguém corta o espinheiro que fecha de novo: é Tovin, com o podão que fica no cabide do posto (ferramenta, não forma). Os canos de couro nas pernas são por causa desse mesmo espinheiro. E quem segue rastro mata adentro marca o caminho para voltar: daí as estacas na canela.

**C3 — Silhueta em 3 formas.** *(PROPOSTA)* Altura de 1,76 m, tronco curto e pernas compridas (`r_perna` ≈ 0,56, dentro de 0,50–0,58, `PIPELINE.md` §3), ombros e pescoço sem nada (essa zona é da Mara, `ELENCO.md`). Não usa aba larga (Lysa), massa centrada acima da cabeça (Oren), prancha na cintura (Maelis), braço inteiro grosso ou o lado de fora do quadril e da coxa, de nenhum dos dois lados (Borin; Arbitragem 2, itens 3 e 4, do `ELENCO.md`).
1. **Boca do chifre:** um chifre curvo vai atravessado nas costas, em diagonal, do quadril esquerdo ao ombro direito. A boca (Ø 0,18 m) sobe 0,15 m acima da cabeça, ao lado dela, inclinada para fora: um cone curvo fora do centro. Não é a massa centrada do Oren nem o Y da forquilha do Nilo, que fica sobre o ombro esquerdo e em escala de criança. Sai do contorno. *T-pose:* rígido no `UpperChest`, sobrevive. *Câmera:* de costas, que é como a criança o vê ao segui-lo na q07, o chifre ocupa a figura inteira; de frente, aparece como um cone curvo acima do ombro direito.
2. **Estojo de estacas na canela direita** (o plano B da versão anterior, agora fora da faixa do Borin): um estojo de couro duro preso por fora do cano direito, de 0,08 a 0,32 m do chão, com cinco estacas de freixo de seção 0,04 × 0,02 m. As pontas ficam para baixo e as cabeças, chatas, saem 0,08 m por cima, abertas em leque para fora, até 0,40 m. É com elas que Tovin marca o caminho de volta quando segue rastro mata adentro: agachado para ler a pegada, a mão cai na canela, onde elas estão. Em preto, de frente, é um bloco de 0,10 m (12 px) colado por fora da canela direita, abaixo do funil do joelho, com as cabeças em leque, e nada do outro lado. A altura e a forma estão livres no elenco: o aro do Borin vai de 0,81 a 1,09 m, a vara da Maelis de 0,40 a 1,30 m, as botas do Daren ficam no pé, e os rolos do Nilo, nas canelas, são de criança e iguais dos dois lados. *T-pose:* peso todo no `LowerLeg` direito, rígido, sobrevive. *Câmera:* de costas, na q07, fica na altura do pé da criança, ao lado do rastro que ela segue. No slice ele não crava nenhuma: o rastro termina na forquilha do Nilo, que já marca o lugar (Arbitragem 2, item 2). **Riscos:** (1) ler como aljava presa na perna; o que separa é o tamanho (0,32 m, metade de uma aljava) e as cabeças chatas, sem pena; (2) ficar na mesma perna da forma 3; o que separa é a altura (canela contra joelho), a forma (bloco reto contra funil) e o lado (só a direita).
3. **Canos em funil:** perneiras de couro duro do tornozelo até um palmo acima do joelho, abrindo em funil no joelho (0,22 m) e afinando no tornozelo (0,13 m), sobre uma coxa de calça justa. Em preto, o vão entre as pernas fecha no joelho e abre de novo embaixo, como uma ampulheta. É o contrário da calça-balão da Lysa (cheia na coxa), das pernas em parêntese do Oren (vão aberto no joelho) e da calça reta com botas do Daren. *T-pose:* é malha, sobrevive. *Câmera:* é o que fica na altura dos olhos da criança, e o que ela vê pisando na frente dela na q07.
Apoio, não conta como forma: do concept antigo, vale manter a pele retinta, o cabelo crespo curto e a barba; a túnica de lã é reta, até o quadril. Na faixa Baixa (ADR-0009: sem contorno toon), as três leem porque são massa e vazio, não linha fina.

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **O chifre de recolher.** Chifre de boi com bocal de madeira, 0,90 m pela curva de fora, boca de 0,18 m, amarras e correia de couro. Não é arma: isso é regra desta ficha (PROPOSTA), não do dossiê nem do ADR-0009.
Regra: lê só o que o código já sabe (período, histórico de vida, progresso do treino).
- **Toque de recolher:** quando o período passa de Tarde para Noite (ao concluir missão ou ao Descansar, ADR-0007 §1), Tovin toca uma vez, do `posto_guarda`, e o som cobre Auren. O HUD continua dizendo o período por texto (GDD cap. 09: nada depende só de cor, e aqui também não depende só de som).
- **Silêncio da busca:** enquanto o histórico tem `evento.nilo_desapareceu` e ainda não tem `marco_idade_8`, ele não toca: leva o chifre à boca e baixa. É o gatilho único dos marcos do sumiço (Arbitragem 2, item 1, do `ELENCO.md`). A regra lê o histórico de vida, não a memória do Tovin, que não testemunha esse evento (`NpcMemory.cs`). À noite, nessa janela, o clipe fica em laço no posto, para o jogador que passar por lá. Todo save vê pelo menos um anoitecer calado: o que a própria conclusão da q07 provoca, porque ela termina à tarde, na beira (ADR-0007 §1). É o único silêncio do jogo que quer dizer alguma coisa.
- **Toque do treino:** quando o B15 se cumpre (`TrainingProgress.TreinoSupervisionadoFeito`), ele toca uma vez, de dia, antes da tela "Fim da Primeira Existência" (B16). Na prática, é o primeiro toque depois do sumiço: aos 8 não há missão aberta, e o período só anda se o jogador descansar (ADR-0007 §1). O chifre volta a tocar por causa da criança, e não da noite.
Nada é concedido e nenhum número muda.

**C5 — Regra exclusiva.** *(PROPOSTA)* **"Pisa onde eu pisar".** É o único NPC que anda com o jogador. No objetivo `seguir_ate_o_bosque` (q07), depois da fala `rastro_no_bosque`, Tovin anda da vaga dele na `entrada_bosque` (é tarde: o objetivo pede Tovin ali) até a **forquilha do Nilo**, cravada na beira da clareira, uns 4 m antes do símbolo, onde "o rastro acabou" (ficha `nilo`, C4). São uns 6 m, e ele vai deixando pegadas no chão. Lá ele para: o símbolo da q08 continua sem testemunha (`NpcMemory.cs`). Se a criança se afasta mais de 3 m do rastro, ele para, vira meio corpo, diz "Pisa onde eu pisar." e espera. Sem tempo, sem falha, sem recompensa extra; na forquilha, o objetivo fecha como hoje. Com 6 m, a regra dos 3 m quase não tem onde disparar: o comprimento do caminho (uma curva da trilha entre a linha e a clareira) é decisão de level design. A fala `depois_da_busca` ("Você foi até a beira comigo e não correu") passa a descrever o que o jogador fez de fato.

**C6 — Voz.** *(PROPOSTA; o texto final é do redator)* Fala em limites de chão (do vão pra cá, do vão pra lá), conta quem entra e quem sai e fecha com "Entendeu?", como já faz em `dialogo.tovin.aos_oito`.
1. "Do vão pra cá é Auren. Do vão pra lá é dele. Eu conto quem entra e quem sai. Entendeu?" (à tarde, na beira)
2. "A Maelis pediu sem alarde. Então o chifre fica quieto. Mas a minha conta não fecha: entrou um, não saiu nenhum." (à noite, entre o sumiço e o salto: a mesma janela da C4. No diálogo, isso pede que Tovin passe a testemunhar `evento.nilo_desapareceu`, com uma linha em `NpcMemory.Testemunhos`, e a condição `E(Lembra(evento.nilo_desapareceu), NaoLembra(marco_idade_8), Periodo(Noite))`, que o `DialogueGraph.cs` já sabe montar)
3. "Esse golpe já te deu o que tinha nesta fase. Daqui pra frente é braço cansado, não é treino. Troca de golpe, ou espera crescer. Entendeu?" (B15: o aviso `treino.saturado`, que já existe, dito por ele, com o mesmo teto por etapa de `TrainingProgress.cs`)

**C7 — Contradição visível.** *(PROPOSTA)* Proíbe o bosque a todo mundo ("Depois dela, só comigo"; "ninguém volta ao bosque sozinho") e vai sozinho todo dia ("eu volto lá todo dia", `depois_da_busca`). Observável em toda tarde (rotina `entrada_bosque`): ele fica 2 m depois da linha das árvores, do lado de lá do próprio limite, e é de lá que manda a criança voltar. Lysa, a parceira, fica do lado de cá (a fala dela já diz isso). Custo: é código. A âncora `entrada_bosque` não muda, porque é dividida com Lysa, Nilo e com objetivos da q03, q05 e q07. Hoje a vaga de cada NPC é um ângulo fixo a 1,5 m da âncora (`NpcActor.RaioDaVaga`); isto pede um deslocamento próprio de vaga para o Tovin, como a ficha do Nilo também registra.

**C8 — Relação com o avatar por destino e origem.** *(PROPOSTA; só fala, nunca poder, dossiê §H; os itens e as oportunidades conferem em `DestinyCatalog.cs`)*
- **serena** (`oportunidade.tarde_livre`): é a criança que tem a tarde livre, que é justamente quando ele está no vão. Ele a põe de "olho de cá": "Fica na linha e me grita se alguém passar."
- **normal** (`oportunidade.recado_da_vila`): a criança anda a vila inteira com recados, e ele a usa como contagem: "Viu alguém indo pro norte hoje?"
- **dificil** ("Vida Árdua", ADR-0007 §2; `item.corda_puida`): ensina o nó que deixa o trecho puído fora da carga, sem trocar a corda: "Corda puída não se joga fora. Dá o nó antes do fio fraco."
- **ruptura** (`evento.anomalia_no_bosque`): não sabe o que tem no bosque e não finge saber; com essa criança, troca o "até a entrada, pode" por "pra casa": "Você olha pro bosque do jeito que ele olha pra cá. Não gosto."
- **origem:** `guardioes` é família de ofício vizinho ("o portão, a estrada e o sossego da região"). A "ronda com Tovin" passa a ser ir com ele da praça ao posto logo depois do toque. No B15 sem a flag, a espada de casa (`item.espada_de_madeira`) fica no cabide: "Aqui todo mundo começa com a do posto." Com `agricultores` e `artesaos`, nada muda.

**C9 — Mudança dos 5 para os 8.** *(PROPOSTA)*
- **Antes (5–7):** o posto é fechado para a criança (o acesso ao treino é justamente o que muda depois do salto, `SLICE` §4.3). Ele fala de lado, com um olho na trilha; à tarde, está do lado de lá do vão; o chifre toca toda noite, menos entre o sumiço de Nilo e o salto.
- **Depois (8):** o posto abre para aprendizes (`dialogo.daren.aos_oito`). Tovin conduz o B15 no pátio e, pela primeira vez, fica de costas para a trilha. Sem `confianca_de_borin`, tira a espada comum do cabide do posto e entrega; com a flag, olha a plaqueta de cobre (ADR-0010 §4) e diz só: "Do Borin. Então já vem conferida." No fim dos quatro verbos, vem o toque do treino (C4).
- **Variante de malha:** não. Mudam a posição, a fala e um gatilho de som.

**C10 — Momento de cartaz.** *(PROPOSTA)* Anoitece durante a busca. Câmera baixa na rua norte, com o posto recortado contra o céu de fim de tarde. Tovin, de perfil, leva o chifre à boca, olha a trilha escura, segura um instante e baixa o chifre sem tocar. Cinco segundos, um plano, sem fala. No jogo, é o anoitecer que a conclusão da q07 provoca, ou qualquer noite até o salto (janela da C4).

## 3. Amarração

- **Destino, Grau ou Trama:** o destino muda o modo como ele recebe a criança na linha (C8). O Grau não se aplica no slice (dossiê §E). A Trama mora no bosque que ele guarda e não entende: a anomalia e o sumiço de Nilo são a conta que não fecha. Ele não sabe do Limiar (`NpcCatalog.cs`) e não finge saber.
- **Decisão de jogo:** atravessar ou não o vão antes da q07. O jogo não tranca (B10: "sem porta de mão única"), e quem está do lado de lá é o próprio Tovin, mandando voltar. No B15, quando largar o golpe que já saturou.
- **Pilar:** viver e crescer.

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| caçador de capuz, com arco, aljava, braçadeiras e distintivo no peito (o default do gerador; é o concept antigo #124–#158) | cabeça descoberta e nenhuma arma: a peça dele é um chifre, e o que leva no corpo são estacas de marcar caminho |
| guarda de elmo, lança e escudo | guarda com a voz (o chifre) e com os pés (canos de couro, rastro) |
| gibão com ombreiras e braçadeiras (o protótipo atual, que repete o `parceiro_treino`) | ombros limpos; chifre em diagonal nas costas, estojo de estacas na canela direita, canos em funil no joelho |
| corpo de herói em V, de peito largo | 1,76 m, tronco curto e pernas compridas; a massa está no chifre e nos joelhos, não no peito |
| batedor calado que sabe o segredo do bosque | só sabe de caça, segurança e bosque (`NpcCatalog.cs`); o que ele faz é contar quem entra e quem sai |
| verde de floresta (o protótipo já tem três coletes verdes: protagonista, Nilo e Lysa); turquesa, dourado ou violeta (GDD cap. 09) | lã cor de carvão, couro escuro e chifre cor de osso, marfim #E9DEC6, com amarras em terracota #A86D52: o objeto é a parte mais clara dele |

## 5. Avaliação — parecer do Art Director: 19/20, aprovado (ADR-0010)

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total /20 |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 1 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | **19** |

- **Corte:** ≥ 14/20 e nenhum zero em C2, C3, C4, C5 — **atingido** (HIPÓTESE do ADR, a recalibrar)
- **Pontuador / data:** Art Director (agente) / 2026-10-03. Não escreveu a ficha.
- **Veredito:** **aprovado por delegação (ADR-0010), com duas condições antes de encomendar o concept:** (a) a forma 2 de C3 sai da faixa do lado de fora da coxa (ver C3); (b) o silêncio do chifre passa a ler uma janela do histórico, e não o estado da q07 (ver C4 e fato 5). Nenhuma das duas muda o total; as duas mudam o que o G2 testa.

**Nota por critério (1 linha cada):**
- **C1 = 2.** Detalhe concreto (o chifre levado à boca e baixado sem tocar) e conflito (o ofício dele é tocar; a superiora pediu silêncio). São duas frases; cabe numa.
- **C2 = 2.** A C2 mais forte das três que pontuei: parte de geometria canônica (uma rua, um vão de 6 m, "ele só não avisa") e cada forma sai de uma condição do bosque (som abafado → chifre; espinheiro → podão e canos). Ressalva: espinheiro e som abafado são PROPOSTA e tocam arte de ambiente e áudio, que não são desta ficha.
- **C3 = 1.** Chifre e canos ficam. O podão colide com o aro do Borin: mesma altura (olho da criança), mesmo afastamento (0,10 m fora da coxa), lado espelhado — e espelho não separa silhueta em preto. Pela regra de desempate do `ELENCO.md`, a faixa é do Borin, que a usa no C4; a própria ficha levanta o problema (§7). Além disso, as medidas do podão não cabem no lugar descrito (fato 6) e, no idle, a mão direita cai em cima dele. Para subir a 2: trocar a forma 2 por uma fora da faixa quadril–coxa, com medidas que fechem. O feixe de estacas previsto em C3 "no mesmo lugar" não resolve: é a mesma faixa.
- **C4 = 2.** Objeto com medida e três regras escritas sobre o que o código já sabe (período, estado de missão, `TreinoSupervisionadoFeito`). Defeito: a regra do silêncio quase nunca dispara (fato 5). Condição: ler "o histórico tem `evento.nilo_desapareceu` e não tem `marco_idade_8`". Com isso, todo save tem ao menos um anoitecer calado — o da conclusão da q07, que é à tarde, com o jogador na beira — e o chifre volta a tocar só no B15, de dia, pela criança.
- **C5 = 2.** "Pisa onde eu pisar" está especificado (3 m, parar, virar meio corpo, esperar, sem falha) e é exclusivo: nenhuma outra ficha põe NPC andando com o jogador. Ressalva de escala: com a geometria atual o trajeto tem ~6 m (âncora em z = 60, beira em z = 66) e a regra quase não tem onde disparar; o comprimento é decisão de level design.
- **C6 = 2.** Três falas escritas com o tique ("Entendeu?", a conta de quem entra e de quem sai). A 3 contradiz a string que diz reaproveitar (fato 3).
- **C7 = 2.** Observável toda tarde, com a parceira do lado de cá como contraste. O custo está subestimado (fato 4).
- **C8 = 2.** Quatro destinos e a origem `guardioes`, com item, oportunidade e evento que conferem no `DestinyCatalog.cs`; dá conteúdo a `oportunidade.ronda_com_tovin`, que hoje é só um id.
- **C9 = 2.** Duas batidas (posto fechado → aberto; de costas para a trilha pela primeira vez) e o primeiro toque pela criança. Sem variante de malha.
- **C10 = 2.** Um plano, 5 s, sem fala. Depende da condição (b): com a regra atual, o quadro pode não acontecer no jogo.

**Fatos dados como cânone que não conferiram:**
1. "o posto fica no meio dela" (C2): `posto_guarda` está em z = 34, numa rua de z = −70 a z = 62 (`AurenSceneBuilder.cs`), a 28 m da linha das árvores. O meio da rua é a praça (z = −4).
2. "Não é arma (o público inclui crianças, ADR-0009; dossiê §H)" e "não entra no combate (dossiê §H; ADR-0009)": o dossiê §H trata de missões e economia e não fala de arma; o ADR-0009 diz que o público inclui crianças, não proíbe arma (o slice tem espada de treino, B15). A regra é PROPOSTA desta ficha, não cânone.
3. C6, fala 3, "o aviso `treino.saturado`… dito por ele": a string diz que o golpe saturou "nesta fase" e manda "esperar crescer" (teto por etapa, `TrainingProgress.cs`). "Já te deu o que tinha hoje" troca etapa por dia.
4. C7, "custo: um deslocamento da âncora, só dado de cena": a `entrada_bosque` é dividida com Lysa, Nilo (e, pela ficha `maelis`, Maelis depois da q07) e com objetivos da q03, q05 e q07; mover a âncora move todos. É uma vaga por NPC, e hoje a vaga é um ângulo fixo a 1,5 m da âncora (`NpcActor.RaioDaVaga`): é código, como a ficha do Nilo já registra.
5. C4, silêncio "enquanto `q07_o_desaparecimento` está EmAndamento": o período só anda ao concluir missão ou descansar (ADR-0007 §1). A q04 termina à noite (`sustentar_a_escolha` pede Nilo em `casa_nilo`, onde ele só está à noite) e vira manhã; `seguir_ate_o_bosque` pede Tovin na `entrada_bosque`, ou seja, à tarde; concluir a q07 vira noite com a missão já Concluída. Quem faz a q07 sem descansar no meio nunca vê um anoitecer com a q07 EmAndamento. O mesmo vale para a fala 2 de C6.
6. Medidas do podão: 0,30 m de lâmina + 0,25 m de cabo não cabem "entre o quadril e o meio da coxa" (~0,25 m num corpo de 1,76 m com `r_perna` 0,56). Ou o cabo sobe ao flanco (zona da lousa da Eira), ou a lâmina chega ao joelho e encosta no funil.
7. C5, "4 m depois da linha das árvores e antes da clareira": é o mesmo ponto em que a ficha `nilo` crava a forquilha ("uns 4 m antes do símbolo", clareira em z = 70). As fichas dão dois nomes ao mesmo lugar (beira × clareira). Alinhar: a escolta termina na vara.
8. §7, a lanterna da Eira sobre o ombro direito: resolvido; a ficha `eira` a tirou de lá.

Conferiram: id, papel, traço, rotina, âncoras, vínculos e tópicos (`NpcCatalog.cs`); testemunhos da q05 e da q07 (`NpcMemory.cs`); todas as falas citadas (`strings.pt-BR.json`); objetivos da q05 e da q07; vão de 6 m e barreira em z = 62; origem `guardioes`, itens, oportunidades e eventos dos destinos (`DestinyCatalog.cs`); B15 e plaqueta de cobre (ADR-0010 §3–4); `Condicao.Missao` e `TreinoSupervisionadoFeito`; acervo #124–#158 e protótipo (`PROVENIENCIA.md` §6); faixa, `r_perna`, V13 e orçamento de Prop pequeno (`PIPELINE.md` §3, §3.1, §4, §11); câmera (`ThirdPersonCamera.cs`, `BodyByAge.cs`).

**Régua de colisão (`ELENCO.md`) — onde discordo:** a arbitragem tratou esquerda × direita e flanco × coxa como zonas diferentes e deixou sem árbitro a faixa do lado de fora do quadril e da coxa, que hoje tem cinco formas: aro do Borin (O, coxa esquerda, C4), vara da Maelis (I, à esquerda, de 0,40 a 1,30 m), lousa da Eira (placa, flanco direito, de 0,85 a 1,45 m), caixas do Daren (dos dois lados, bem mais afastadas) e o podão (J, coxa direita). Em preto, espelho não separa; separam forma e altura. O podão é o único que repete a altura e o afastamento do aro. A vara da Maelis fica do mesmo lado do aro e merece a mesma conferência.

**Para o G2 — o que o concept precisa provar:**
1. A boca do chifre lê como chifre (cone curvo, boca virada para fora), não como aljava nem como punho de espada nas costas: a aljava é o default do acervo antigo (#141).
2. A forma 2 nova, fora da faixa quadril–coxa e com medidas que fechem, numa folha a 30% com Borin, Oren, Daren e Nilo (o Y nas costas do Nilo é o espelho do chifre, em escala de criança).
3. Os canos em funil leem a 30% (degrau de ~5 px no joelho) e o teste de descrição não responde "bota de cano largo": o acervo antigo já tinha joelheira de couro e bota de cano alto (#138, #156).
4. Render de costas na câmera do jogo, na `entrada_bosque` à tarde, com Lysa e Nilo na mesma âncora: as três figuras se separam.
5. O chifre como Prop separado (`tovin_chifre`), preso ao `UpperChest`, com V09 e V13 medidos no corpo sem ele.

**Condições cumpridas em 2026-10-03:**
- **(a) Forma 2 fora da faixa quadril–coxa:** o podão saiu do corpo (fica no cabide do posto, C2). Entrou o estojo de estacas na canela direita, de 0,08 a 0,40 m do chão: abaixo do aro do Borin (0,81–1,09 m) e da vara da Maelis (a partir de 0,40 m), na única faixa de contorno ainda livre em altura. As medidas fecham numa canela de ~0,44 m (joelho a ~0,52 m com `r_perna` 0,56). Na folha do G2 entram também Daren e Nilo (§6).
- **(b) Silêncio do chifre:** passou a ler a janela do histórico, `evento.nilo_desapareceu` e ainda não `marco_idade_8`, o gatilho único da Arbitragem 2 (C4). A fala 2 da C6 e a C10 leem a mesma janela.
- **Fatos:**
  1. a C2 diz que o posto fica em z = 34, a 28 m da linha das árvores, e não no meio da rua;
  2. "não é arma" ficou como regra desta ficha, sem citar o dossiê §H nem o ADR-0009 (C4; o podão, que tinha a mesma citação, saiu);
  3. a fala 3 da C6 diz "nesta fase" e "espera crescer", como a `treino.saturado`;
  4. o custo da C7 virou código (`NpcActor.RaioDaVaga`), sem mexer na âncora;
  5. a janela do chifre, como em (b);
  6. as medidas do podão deixaram de valer: ele saiu do corpo;
  7. a escolta termina na forquilha do Nilo, com o mesmo nome do lugar da ficha `nilo`, numa caminhada de ~6 m (C5);
  8. a nota da lanterna da Eira saiu do §7.

**Conferência final (Art Director, 2026-10-03):** condições atendidas. (a), (b), os fatos 1–8 e a Arbitragem 2, itens 1, 3 e 8, estão no corpo. Ressalvas de texto, sem pendência: o C5 põe a forquilha "na beira da clareira, uns 4 m antes do símbolo", mas pelas coordenadas da ficha `nilo` (z ≈ 66) e da `simbolo_limiar` (símbolo em z = 72, clareira de z 62 a 78) ela fica a ~6 m do símbolo, já dentro da clareira. E a canela direita (0,08–0,40 m) não está livre: a barra arrancada da Mara, da mesma rodada, cobre a perna direita até 0,35 m. As duas vão para a mesma folha do G2.

## 6. Encaminhamento

**Para o G2 (concept), só depois de aprovado:**
- vistas: frente, perfil, costas e 3/4 em T-pose, fundo neutro, linha de chão, altura de 1,76 m marcada. As costas valem tanto quanto a frente, porque é assim que a criança o vê na q07
- silhueta: boca do chifre, estojo de estacas na canela direita e canos em funil, em preto a 30%, numa folha com Borin, Oren, Daren e Nilo; e um render na câmera do jogo (pivô a 0,94 m, 2,75 m atrás, 15°; `BodyByAge.cs`, `ThirdPersonCamera.cs`) com Tovin de costas a 3 m, na `entrada_bosque` à tarde, com Lysa e Nilo na mesma âncora
- paleta: túnica de lã carvão, couro escuro, estacas de freixo claro, chifre em marfim #E9DEC6, amarras em terracota #A86D52; reservados e proibidos nele: turquesa #86C8C9, dourado #D6B36A, violeta #9777B8
- objetos à parte: o chifre, com 0,90 m pela curva, boca de 0,18 m e a correia; o estojo, com 0,24 × 0,10 m e cinco estacas de 0,32 m
- orçamento (`PIPELINE.md` §4, HIPÓTESE v0): corpo Npc com 8 000 tris, 2 materiais, textura 1024 e 55 ossos, sem osso secundário; o estojo entra na malha do corpo, com peso todo no `LowerLeg` direito. O chifre é um **Prop pequeno separado** (`tovin_chifre`: 500 tris, 1 material, 256), preso ao `UpperChest` no prefab. Dentro do FBX do corpo, ele subiria o topo para 1,90 m e jogaria o `r_cabeca` para fora da faixa (V13 mede todos os renderizadores, `PIPELINE.md` §11)
- custos que esta ficha pede: 1 clipe próprio (levar o chifre à boca e baixar, ~3 s, em três usos, em laço nas noites da janela); 1 som de chifre; o script de seguir com pegadas (C5); a vaga deslocada na `entrada_bosque` (C7, código); Tovin como testemunha de `evento.nilo_desapareceu` (C6, uma linha de dado); ~9 falas novas (C6, C8, B15)
- testes a registrar: silhueta (≥ 4/5) com Borin, Oren, Daren, Nilo e o parceiro de treino na folha; descrição (se a resposta disser "aljava", o estojo encurta e as cabeças ficam mais largas); troco com o parceiro de treino (a cena do chifre calado tem de quebrar sem Tovin)

**Para o G3:** blocos `### tovin` e `### tovin_chifre` em `PROVENIENCIA.md`, com `g1:` (data, nota, pontuador), `g2:` (data, silhueta n/5, descrição, troco), `entrada:` (concept + SHA-256), `ferramenta`, `plano`, `licenca`, `termos_url` e `g3:`. `referencias_terceiros`: nenhuma usada nesta ficha.

## 7. Ligações com outras fichas

PROPOSTAS desta ficha que envolvem outro personagem:
- **Maelis:** o "sem alarde" dela cala o chifre do sumiço até o salto (C1, C4, C6, C10). Sai da fala dela (`dialogo.maelis.ausencia`) e do vínculo `subordinado`/`superiora`; a ficha de Maelis precisa sustentar que foi ela quem pediu e que Tovin obedece.
- **Lysa:** à tarde, ela fica do lado de cá do vão e ele do lado de lá (C7). A borda do bosque é espinheiro e quem mantém o vão limpo é Tovin (C2): isso toca as ervas da beira, que são de Lysa, e a arte de ambiente da borda (barreira de z = 62, `AurenSceneBuilder.cs`). O "bosque que abafa o som" toca o áudio de ambiente.
- **Borin:** no B15, Tovin reage à espada marcada com "Do Borin. Então já vem conferida." Ele não sabe do segredo da vista (só a criança sabe, ficha do Borin).
- **Mara:** o podão continua existindo, no cabide do posto, e Tovin segue cortando o espinheiro (C2). A premissa da ficha `mara` ("o espinheiro fecha de novo e o podão dele reabre") continua valendo.
- **Parceiro de treino (sem ficha):** não pode usar chifre nem estojo de estacas. Se usar canos de couro, é o aprendiz de Tovin e precisa de outra forma dominante.
- **Avatar, origem `guardioes`:** conteúdo proposto para `oportunidade.ronda_com_tovin` (C8).
- **Nilo:** a escolta da C5 termina na forquilha cravada dele (ficha `nilo`, C4), e Tovin não crava estaca nenhuma ali. O chifre calado é pela ausência dele, com o mesmo gatilho da forquilha (Arbitragem 2, item 1).
- **Elenco masculino adulto:** cada homem se lê pela parte do corpo que o ofício gasta: Borin pelo braço, Oren pelas costas, Tovin pelas pernas. As pernas se separam no vão e na canela: Tovin fecha o vão no joelho (canos) e tem o estojo só na canela direita; Oren abre o vão no joelho (parêntese); Daren tem calça reta e botas.
- **Para o coordenador conferir:** o estojo é a única forma de adulto na faixa da canela (0,08–0,40 m). Os rolos do Nilo ficam na mesma altura relativa, mas são de criança e iguais dos dois lados.

Zonas de silhueta que esta ficha ocupa:
- acima do ombro direito, fora do centro (a boca do chifre, até 0,15 m acima da cabeça);
- lado de fora da canela direita, de 0,08 a 0,40 m (o estojo de estacas);
- joelhos (canos em funil abrindo no joelho, coxa justa: o vão entre as pernas fecha no joelho);
- costas em diagonal (vista de trás).
