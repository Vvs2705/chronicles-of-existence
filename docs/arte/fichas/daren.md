# Ficha G1 — `daren`

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md)). Par com [`mara`](mara.md).
> Fato com fonte é cânone. **PROPOSTA** = ideia desta ficha, não está nos docs e não vale até ser aprovada. As PROPOSTAS do [`borin`](borin.md) citadas aqui valem como canon de trabalho (aprovadas por delegação em 2026-10-03).

## 1. Identificação

- **id:** `daren` (`NpcCatalog.cs`; GDD cap. 06, NPC-02). O nome é provisório (GDD cap. 02); o id é estável (`INCONSISTENCIAS.md` A5).
- **categoria:** Npc, base adulta (`PIPELINE.md` §3.1)
- **onde aparece no slice:**
  - B03 e B06/q01 (`falar_com_familia`), ao lado de Mara diante da porta, a ~6 m do spawn (`AurenSceneBuilder.cs`). Os dois dividem a âncora, cada um a 1,5 m dela (`NpcActor`, `RaioDaVaga`).
  - B07/q02, da qual é dono: `receber_tarefa` → `cumprir_tarefa` ("Levar o recado de Daren a Oren") → `prestar_contas` (`q02…json`; `DialogueCatalog.cs`).
  - B14, fala `aos_oito`, que abre o treino com Tovin.
  - Âncoras: `casa_familia` de manhã e à noite, `portao_sul` à tarde (`NpcCatalog.cs`).
  - **Tensão a alinhar (dono da q02):** o jogo começa de manhã (`TimeOfDay.Manha = 0`) e concluir a q01 já vira a tarde (ADR-0007 §1), então pela rotina a q02 abre com Daren no `portao_sul`. Mas o `q02…json` ancora `receber_tarefa` e `prestar_contas` em `casa_familia`. Esta ficha encena a q02 no portão (C5, C10). Se o dono da q02 mantiver a casa, só muda o lugar: o cordão e a regra continuam iguais.
- **cânone de partida:**
  - "responsável familiar, disciplinado; introduz atividade profissional" (dossiê §G). Papel adaptável por origem e destino (GDD cap. 02); estética e nome podem variar (dossiê §C).
  - **Malha única para as 3 origens é PROPOSTA desta ficha.** O dossiê §C e o GDD cap. 02 deixam estética e profissão variarem com a origem. A escolha aqui é de custo (`PIPELINE.md` §4): o ofício entra como carga e ferramenta trocáveis.
  - traço `disciplinado`; rotina: prepara o trabalho do dia (manhã, casa), leva o que a família produziu (tarde, `portao_sul`), em casa (noite); vínculos: companheiro de Mara e "conhecido de ofício" de Borin, nos dois sentidos (`NpcCatalog.cs`); sabe `topico.familia`, `topico.oficio_da_familia` e `topico.vila_auren`. Papel exibido: "Responsável pela família e pelo ofício".
  - falas já escritas (`dialogo.daren.*`): "Tarefa dada é tarefa cumprida."; "Você foi, entregou e voltou para contar. Foi uma tarefa pequena, mas foi sua. Eu guardo isso."; "Quem descansa direito trabalha direito."; "Pode olhar. Mexer, só quando eu mostrar como."; "Ofício é o que a gente faz todo dia, com chuva ou com sol."; aos 8: "O posto da guarda agora aceita aprendizes da sua idade. Tovin disse que você pode treinar lá: espada de madeira, e com juízo."
  - testemunha `evento.q01_concluida`, `evento.q01_familia_apresentada`, `evento.q02_concluida` (junto com Oren) e `marco_idade_8` (`NpcMemory.cs`).
  - o portão sul é o "fim da estrada: chegada/partida de Auren" (`AurenSceneBuilder.cs`) e o limite sul do slice (`SLICE` §1.1); "Tudo que chega a Auren vem pela estrada do sul e passa pela minha mão" (Oren); Oren recebe a carga ali de manhã (`NpcCatalog.cs`). Canon de trabalho (Borin): o ferro de Auren chega nessa carga. A porta das casas tem 1,8 m de largura (`VaoPorta`).
  - a origem dá à criança uma ferramenta pequena do ofício da casa: `item.foice_pequena`, `item.martelo_leve` ou `item.espada_de_madeira` (`DestinyCatalog.cs`). Quem a fez não está escrito.
  - já existe protótipo PROTOTIPO: "túnica azul-ardósia, cinto com bolsa de ferramentas" (`PROVENIENCIA.md`). Concept antigo: macacão marfim, descalço, cabelo escuro com têmporas grisalhas, túnica azul de gola alta com alamares (`ACERVO.csv` #108, #111). Os dois são o default (§4).
- **autor da ficha / data:** Concept Art Lead (agente), 2026-10-03. Reescrita no mesmo dia: primeiro pela Arbitragem 1 do [`ELENCO.md`](ELENCO.md) (o cavalete em Π saiu, a altura passou a 1,70 m); depois pelas condições do parecer (§5) e pela Arbitragem 2.
- **estado:** aprovado G1 por delegação (ADR-0010, 2026-10-03), condições cumpridas no corpo (lista no fim da §5); pendência da conferência final (tiras e vara) corrigida em 2026-10-03 (W3), reconferido sem pendência pelo Art Director (2026-10-03, leva C1, fim da §5)

## 2. Critérios C1–C10

**C1 — Gancho.** Daren manda uma criança de cinco anos atravessar Auren sozinha com um recado de nó, mas não a deixa encostar numa ferramenta. Quando ela volta para contar, ele amarra o recado na ponta da vara de carga com que leva tudo o que a casa faz até o portão sul. *(PROPOSTA)*

**C2 — Necessidade do mundo.** Cânone: Auren tem uma saída, a estrada do sul (`AurenSceneBuilder.cs`; Oren). A carga chega de manhã ao portão (Oren, `NpcCatalog.cs`), à tarde Daren leva a produção da casa ao mesmo portão (`NpcCatalog.cs`), e o ferro da vila chega nessa carga (Borin).

**PROPOSTA:**
- A carroça de fora descarrega de manhã no portão e não entra na vila (ficha do Oren). Ela **espera ali até o fim da tarde** e volta pela estrada com o que Auren manda. É uma por dia, e o que não estiver no portão quando ela parte fica para amanhã.
- Numa vila de uma estrada só, o horário da carroça é o relógio da casa, e a disciplina nasce daí. "Tarefa dada é tarefa cumprida" e "com chuva ou com sol" são o horário da carroça virado ditado.
- A q02 também nasce daí: Oren negocia na praça à tarde (`NpcCatalog.cs`), então alguém tem de avisá-lo de que a carga da casa está no portão antes de a carroça partir. Essa é a primeira tarefa da criança. O `q02…json` só diz "Levar o recado de Daren a Oren"; o conteúdo do recado é PROPOSTA e depende do dono da q02.

**C3 — Silhueta em 3 formas.** *(PROPOSTA; reescrita em 2026-10-03 pelas condições do parecer: bloco e botas não liam a 30%, e "bloco" é zona da Sera e do Aethron)*
Daren tem 1,70 m (Arbitragem 1.5), com o pescoço à vista e nada acima da cabeça. Cada homem do elenco se lê pela parte do corpo que o ofício gasta: Borin pelo braço, Oren pelas costas, Tovin pelas pernas (fichas `tovin` e `oren`). Daren se lê **pelos lados**: carrega numa vara de ombro, com um peso em cada ponta, como uma balança.
As três formas são as três partes dessa vara de carga, como os três cantos da manta do avatar e as três partes da porta do Aethron, mas cada uma tem um contorno diferente e fica num lugar diferente da folha. Medidas em px pela régua de 30% do `client/tools/silhueta.py` (120 px/m). A câmera é a da ficha da Mara (altura ≈ 1,65 m).
1. **Vara:** a canga, uma vara de madeira de 1,90 m e **Ø 0,05 m**, atravessada nos ombros, por trás do pescoço, a ~1,45 m do chão. É mais comprida que os braços abertos dele (~1,70 m).
   - Em T-pose, de frente, a vara continua a linha dos braços e passa 0,10 m (12 px de comprimento, 6 px de espessura) de cada mão; com a caixa, a figura dele chega a ~2,1 m de largura (~250 px), o T mais largo do elenco. De braços caídos, é uma barra 5× mais larga que os ombros, logo acima deles.
   - Não passa da cabeça. Não tem montantes nem verga, ao contrário do Π do Aethron, que sobe. Não é aba sobre a cabeça (Lysa) nem rolo sem pescoço (Mara).
   - Câmera: a 6 m e da praça ao portão (~66 m), é a coisa mais larga que alguém carrega em Auren.
2. **Estojo do ofício:** na ponta esquerda, pendurado por uma **tira de couro encerado de 0,06 m de largura e 0,15 m de comprimento, de face para a frente** (dura, rígida no rig), um estojo de couro e madeira, alto e estreito (0,16 × 0,16 × 0,60 m), sempre amarrado. Dentro vai a ferramenta grande da casa (C7). Em T-pose, de frente, é um I vertical além da mão esquerda, de ~1,30 m a ~0,70 m do chão (19 × 72 px), ligado à vara pela tira (7 px).
3. **Caixa da carga:** na ponta direita, pendurada por uma **tira igual, de 0,06 m de largura e 0,25 m de comprimento**, uma caixa larga e baixa (0,40 × 0,25 × 0,25 m), aberta ou cheia conforme o período (C4). Em T-pose, de frente, é um bloco deitado além da mão direita, de ~1,20 m a ~0,95 m do chão (48 × 30 px), ligado à vara pela tira (7 px).

Lidas juntas, de frente, as três formam uma linha comprida com um I de um lado e um bloco do outro, no mesmo nível. Pesam igual e têm formas opostas: "Carga torta chega torta" é equilibrar pelo peso, não pelo desenho.
- **Conta do teste oficial** (`client/tools/silhueta.py`; correção de 2026-10-03, W3): a máscara é feita a 512 px de altura, a abertura final (`LIMPA` = 7 px) apaga o que tem menos de 7 px, e depois só fica o pedaço ligado ao corpo. Daren entra com caixa de 1,70 m; com a figura ocupando ~80% da altura da imagem, 1 px ≈ 4,2 mm, e o corte fica em ~2,9 cm (~2,3 cm se a figura ocupar a altura toda). Com corda, os pesos se soltavam da figura e eram descartados. Agora, a 512 px: vara de 5 cm = 12 px; tiras de 6 cm = 14 px; estojo de 16 cm = 38 px; caixa de 25 cm de altura = 60 px. Tudo passa do corte com folga, e o caminho corpo → vara → tira → peso fica inteiro. Na folha (120 px/m): vara de 6 px, tiras de 7 px, estojo de 19 × 72 px, caixa de 48 × 30 px. O cordão do recado (C4) é fino, some na limpeza e é apoio.
- **Vizinhanças:** a assimetria é das pontas da vara, longe do corpo, e não do braço (zona do Borin) nem do flanco (lousa da Eira, colada ao corpo). Os pesos pendem a mais de 0,5 m do quadril, e não ao lado da coxa (Arbitragem 2.3: as caixas do Daren ficam e passam pela conferência do G2).
- **Rig:** tudo é prop próprio (`Prop` médio, preso por soquete ao `UpperChest`), com tiras e pesos rígidos, sem balanço. Dentro da malha, a vara mudaria o h e o r_cabeca do validador (V09/V13). No clipe de andar, o braço não pode atravessar as tiras, e o G2 confere.
- **À noite** (desde os 5, como pede o parecer): a vara de 1,90 m não passa pela porta de 1,8 m e fica encostada na parede de `casa_familia`, do lado de fora. Daren, sem ela, tem o contorno de qualquer homem; a forma dele fica encostada ao lado. Quem nunca larga a carga é o Oren (C7 dele).
- **Risco:** se o pontuador contar a vara como uma forma só, C3 fica em 1. O §7 lista por que não sobrou zona de adulto livre que leia a 30% e não seja dele.
Apoio, que não conta como forma: o colete de lona encerada, as botas de estrada e o cordão do recado na ponta direita (C4).

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **A vara de carga e o cordão do recado.** A vara, o estojo e a caixa são os de C3. O **cordão de recado** é uma corda de 0,40 m com uma conta de madeira e o nó da casa; quem recebe o recado dá o segundo nó.
Regra:
1. **O estojo da esquerda é o do ofício:** está sempre amarrado, em todo período e em toda idade. É lá que fica a ferramenta (C7).
2. **A caixa da direita é a da carga**, e segue a rotina canônica: de manhã, em `casa_familia`, aberta e vazia; à tarde, no portão, cheia e de tampa amarrada; à noite, a vara está encostada na parede, com a caixa vazia (a carroça levou a carga).
3. **O conteúdo da caixa é a origem.** Só aparece até 0,10 m acima da borda, e por isso a silhueta é a mesma nas 3 origens:
   - `agricultores`: espigas e feixe;
   - `artesaos`: peças embrulhadas;
   - `guardioes`: estacas de marco de estrada e pavio de lanterna. A casa de guardiões não vende: leva ao portão o que a estrada gasta.
   **A quantidade é o destino:** cheia em serena, normal e ruptura. Em dificil, a caixa vai pela metade, e ele põe uma pedra dentro para equilibrar o estojo ("Carga torta chega torta"). A condição material vem do destino, nunca da origem (GDD cap. 02; comentário de `DestinyCatalog.Origens`).
4. **O recado:** na q02, Daren dá o cordão com o nó da casa, Oren dá o segundo nó (`cumprir_tarefa`) e a criança o traz de volta (`prestar_contas`). A partir de `evento.q02_concluida` (Daren testemunha, `NpcMemory.cs`), o cordão de dois nós fica amarrado na ponta direita da vara, acima da caixa, para sempre. É o "Eu guardo isso" da fala canônica, desenhado.
5. **Aos 8:** a partir de `marco_idade_8`, o cordão ganha um terceiro nó, torto, que é o da criança, e à tarde um volume pequeno amarrado com o mesmo nó torto vai em cima da caixa (C9).
O jogo lê o período (rotina), a origem e o destino (`BirthChoice`), `evento.q02_concluida` e `marco_idade_8` (histórico). Nada é concedido: a q02 paga as mesmas 5 moedas (HIPÓTESE v0, `QuestCatalog.cs`). Custo: soquete e o prop da vara; os 3 conteúdos, a pedra, o volume pequeno e o cordão em 3 estados como Props pequenos; e o componente de "peça ligada por evento" do `ELENCO.md`, o mesmo das fichas do Borin, da Mara e do Nilo.

**C5 — Regra exclusiva.** **Voltar para contar.** Cânone: a q02 é a única das oito missões que termina voltando a quem deu a tarefa (`prestar_contas`, `QuestCatalog.cs`). Para Daren, e só para ele, a tarefa termina quando a criança volta e conta. Como funciona (o cordão é PROPOSTA; o lugar depende do dono da q02, §1):
1. `receber_tarefa`: Daren tira do colete um cordão com um nó e o entrega.
2. `cumprir_tarefa`: a criança acha Oren onde a rotina o puser (à tarde, na praça), e ele dá o segundo nó.
3. Enquanto a criança não volta, Daren repete "Tarefa dada é tarefa cumprida…" (`tarefa_pendente`), e a ponta direita da vara fica sem cordão.
4. `prestar_contas`: a criança volta, o cordão vai para a ponta da vara, a missão conclui e o dia vira noite (ADR-0007 §1).
A q01 já virou o dia sozinha. A q02 é a **primeira vez que o jogador escolhe quando** o dia vira, porque a conclusão espera pela volta. Demorar não tem punição: nada expira (`q02…json`, `se_ignorada`).

**C6 — Voz.** *(PROPOSTA; o texto final é do redator)* Ele fala em ditado espelhado: duas metades com a mesma palavra ou com verbos em par, como nas falas canônicas ("Tarefa dada é tarefa cumprida"; "Quem descansa direito trabalha direito"). Não fala de lugar nem de linha (isso é do Tovin) nem de preço (isso é do Oren). Nunca termina em pergunta, ao contrário da Mara.
1. "Recado dado é recado voltado. Só acaba quando você me conta." (cabe em `tarefa_pendente`)
2. "O olho faz, a mão confere." (em casa, quando a criança pergunta pela ferramenta pequena que ele fez)
   - É o lema de quem faz a olho, e Daren leva ao Borin o que fez (C8, origem). Sem saber, ele descreve o jeito do Borin, que confere com o polegar. A fala não revela nem toca o segredo da q06: quem não fez a q06 ouve só um ditado.
3. "Carga torta chega torta." Aos 8, a mesma frase dobrada: "Torto também chega. Amarra você." (C9)

**C7 — Contradição visível.** **Prende as mãos e solta os pés.** Ele não deixa a criança tocar numa ferramenta, mas a manda sozinha atravessar a vila. Os dois lados já estão no cânone. Dá para ver em três ações recorrentes:
1. Toda manhã em casa, a fala `em_casa`: "Pode olhar. Mexer, só quando eu mostrar como."
2. **PROPOSTA:** o ofício viaja fechado. A ferramenta grande da casa vai no estojo da esquerda, sempre amarrado (C4, regra 1): a criança vê o estojo todo dia e nunca a ferramenta.
3. Na q02, a criança faz sozinha ~66 m de rua, do portão à praça e de volta (`AurenSceneBuilder.cs`). Aos 8, vai sozinha ao posto da guarda (`aos_oito`).
Confia os pés, não as mãos: o inverso da Mara, que prende os pés no vão e deixa a casa aberta. Custo: nenhum clipe novo.

**C8 — Relação com o avatar por destino e origem.** *(PROPOSTA; fala, carga e objeto, nunca poder; os itens conferem em `DestinyCatalog.cs`; a fala é escolhida pelo destino, a pendência "condição de diálogo por destino" do `ELENCO.md`)*
- **serena:** caixa cheia, sem pressa. Foi Daren quem entalhou o `item.brinquedo_entalhado`, a olho; é nele que Borin aponta o entalhe torto (ficha do Borin, C8). A q02 vira passeio: "Recado dado é recado voltado. E na volta, olha a vila: ela também é tarefa."
- **normal:** caixa cheia. O `item.cantil` foi ele quem comprou do Oren, com couro e costura de fora, comprado num dia de feira (ficha do Oren, C8), e passou para a criança. Com `oportunidade.recado_da_vila`, este é o primeiro de muitos recados: "Hoje é o Oren. Amanhã é a vila inteira."
- **dificil** (Vida Árdua): caixa pela metade, com a pedra. A `item.corda_puida` da criança é a corda velha da vara, que ele trocou pelas tiras de couro e deu a ela. Com `oportunidade.trabalho_cedo`, o recado não é lição, é comida: "Se o Oren não souber até o fim da tarde, a carroça sai sem a gente. Corre. Mas volta pra contar." É só fala: a q02 continua sem prazo.
- **ruptura:** caixa cheia. No portão, a gente da carroça repara na criança (`destino.ruptura.contexto_social`), e Daren se põe entre ela e a estrada: "Olharem é com eles. Andar do meu lado é com você." Ele não reage ao amuleto (Arbitragem 2.8).
- **origem:** muda o conteúdo da caixa, a ferramenta fechada no estojo (C4, C7) e a ferramenta pequena que a criança já tem. Todas foram feitas por Daren, a olho, como cópia da dele, e cada uma passou pelo Borin:
  - `foice_pequena`: a prova da família no aro do Borin é de quando o Borin a acertou;
  - `martelo_leve`: Daren é o conhecido de ofício que abriu a `oportunidade.oficina_de_borin`;
  - `espada_de_madeira`: é a que o Borin acha "fora de medida".
  "O olho faz, a mão confere" (C6) é esse vínculo dito em voz alta.
  - **Risco (parecer):** a casa de `guardioes`, cujo ofício é "guardar, patrulhar e proteger", carregando produção na vara pode falhar no teste de descrição. As estacas e o pavio são da estrada, não de venda, e isso resolve só em parte.

**C9 — Mudança dos 5 para os 8.** *(PROPOSTA)*
- **Antes (5–7):** de manhã e à tarde, a vara nos ombros; à noite, encostada na parede de fora, como o ditado manda ("Quem descansa direito trabalha direito"). Desde a q02, o cordão de dois nós fica na ponta direita. A regra é "Mexer, só quando eu mostrar como".
- **Depois (8):** à tarde, um volume pequeno amarrado com nó torto, a parte da criança, vai em cima da caixa da carga. O cordão ganha o terceiro nó, torto. Uma segunda fala no nó `aos_oito` dobra o ditado: "Torto também chega. Amarra você." É a primeira vez que ele deixa a mão da criança tocar a carga. A fala canônica `aos_oito` manda a criança ao posto da guarda, e a primeira ferramenta que ela segura de verdade não é a dele. Custo: dois props pequenos ligados por evento, sem animação.
- **Variante de malha:** não. Mudam prop, carga e fala (dossiê §G).

**C10 — Momento de cartaz.** *(PROPOSTA)* À tarde, no `portao_sul`, na vaga que o `NpcActor` dá a ele (1,5 m da âncora); a linha do portão é arte da T013 e não existe no greybox. Contra a luz da estrada do sul, um homem de altura comum tem nos ombros uma vara mais larga que os próprios braços abertos. De um lado pende um estojo alto e fechado; do outro, uma caixa larga e cheia, as duas no mesmo nível. Ele tira do colete um cordão com um nó e o estende para baixo, para a criança: "Leva ao Oren. E volta pra contar." Dura 5 s, sem clipe novo: a entrega acontece na conversa.

## 3. Amarração

- **Destino, Grau ou Trama:** o destino muda o quanto pesa a carga (e a pedra da Vida Árdua), um objeto que veio dele (brinquedo, cantil, corda) e uma fala (C8). O Grau não se aplica no slice (dossiê §E). A Trama não o toca: ele é o lado de Auren que olha para a estrada, não para o bosque.
- **Decisão de jogo:** quando voltar para contar. Voltar fecha a q02 e vira o dia para a noite (ADR-0007 §1). Quem adia mantém a tarde (Oren na praça, Lysa e Tovin na entrada do bosque); quem volta logo vê o cordão ir para a ponta da vara. É a primeira vez que o jogador escolhe quando o dia vira, porque a q01 vira sozinha. *(PROPOSTA)*
- **Pilar:** escolher e transformar. O primeiro ato da criança é reconhecido por uma pessoa e fica à vista, pendurado na vara dele.

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| o pai largo, barbudo, de peito de barril e avental de couro, que é o mesmo default que a ficha do Borin evita no ferreiro | altura comum, sem barba, sem peito largo; o volume dele está nas pontas da vara, longe do corpo |
| o ofício como figurino (foice para camponês, martelo para artesão, espada para guarda), um boneco por origem | um corpo só; o ofício viaja fechado no estojo, e a carga na caixa, sem passar da borda |
| cinto com bolsa de ferramentas e túnica azul-ardósia (protótipo, `PROVENIENCIA.md`); túnica de gola alta com alamares (`ACERVO.csv` #108) | nenhuma ferramenta no corpo; colete de lona encerada escura; nada de azul, porque o Azul profundo #253850 é de menus e do arcano (GDD cap. 09) |
| mochila de aventureiro ou armação de costas, a carga que sobe (a torre é do Oren) | vara de ombro de 1,90 m: estojo alto de um lado e caixa larga do outro; ele carrega para os lados |
| "bloco": tronco sem cintura, que em preto é qualquer homem | o contorno dele não está no tronco: está nas pontas da vara |
| pai severo que dá ordem | ditado espelhado, sem pergunta; manda sozinho e espera a volta |
| macacão marfim, descalço (`ACERVO.csv` #111) | botas de estrada (a botina amarrada do `ACERVO.csv` #123 serve de ponto de partida), como apoio e não como forma |

## 5. Avaliação — parecer do Art Director: 18/20, aprovado (ADR-0010)

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total /20 |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 1 | 2 | 2 | 1 | 2 | 2 | 2 | 2 | **18** |

- **Corte:** ≥ 14/20 e nenhum zero em C2, C3, C4, C5 — **atingido** (HIPÓTESE do ADR, a recalibrar)
- **Pontuador / data:** Art Director (agente) / 2026-10-03. Não escreveu a ficha.
- **Veredito:** **aprovado por delegação (ADR-0010), com quatro condições antes de encomendar o concept:** (a) C3 traz uma segunda e uma terceira forma que leiam a 30% (bloco e botas não leem); (b) a fala 2 de C6 é trocada; (c) C4 e C7 decidem onde está a ferramenta fechada; (d) a canga sai dos ombros à noite já dos 5 aos 7 (ver C9), e os fatos abaixo são corrigidos.

**Nota por critério (1 linha cada):**
- **C1 = 2.** Recado de nó, canga e linha do portão, com conflito (solta a criança na vila e prende as mãos dela). São duas frases, e "linha que ele mesmo nunca cruza" fica sem explicação no resto da ficha.
- **C2 = 2.** Uma estrada só e uma carga por dia que não espera: o traço `disciplinado`, as duas falas-ditado e o motivo da q02 saem da mesma condição de Auren, ancorada na rotina e nas falas canônicas. Precisa casar com o Oren: aqui a carga parte no fim da tarde; lá, "a carroça descarrega no portão e volta pela estrada". As duas fichas têm de dizer que ela espera no portão até o fim da tarde.
- **C3 = 1.** A balança é forma de verdade. De braços caídos, é uma barra 3× mais larga que os ombros com dois pesos; em T-pose, a canga some atrás dos braços e sobram duas caixas soltas debaixo deles, o que ainda lê. O bloco não lê: tronco sem cintura e sem peito largo é o contorno de qualquer homem em preto, e no idle ele se funde à barra da canga. As botas (17 × 6 px a 30%) são a bota do default; a própria ficha diz que é a forma mais fraca. E "tronco em bloco" já está na Sera (colete em caixa) e no Aethron (sobreveste em bloco). Sobra uma forma garantida, como o Borin na primeira nota.
- **C4 = 2.** Objeto com medida e regra escrita por período, origem, destino, q02 e marco, tudo lido do save; nada é concedido. Há uma contradição interna a resolver: a regra 1 deixa as caixas "abertas e vazias" de manhã e à noite, e o C7 põe a ferramenta grande "dentro da caixa da esquerda, de tampa amarrada", todo dia.
- **C5 = 2.** É exclusiva pelo dado: a q02 é a única que termina voltando a quem deu a tarefa (a q06 termina com o Borin, mas sem sair da ferraria). E se vê pelo cordão de nós. É a estrutura de missão mais comum que existe (leva e volta) com um adereço por cima, a mais fraca das três C5 que pontuei, mas especificada passo a passo e sem punição.
- **C6 = 1.** O tique é claro e canônico (ditado espelhado), e as falas 1 e 3 são dele. A fala 2 ("Daqui pra lá é estrada. Daqui pra cá é casa. Eu fico na linha.") é o tique do Tovin quase palavra por palavra ("Do vão pra cá é Auren. Do vão pra lá é dele.", ficha `tovin`, C6; "Fica na linha", C8 dele). No troco, um diz a fala do outro sem reescrita. Trocar a fala 2 por uma no espelho dele sobe para 2.
- **C7 = 2.** Contradição real, com as duas metades no cânone (`em_casa`: "Mexer, só quando eu mostrar como"; q02: a criança atravessa a vila sozinha), observável todo dia e sem clipe novo.
- **C8 = 2.** Quatro destinos com carga, um objeto que veio dele e uma fala; três origens; os ids conferem e tudo fecha com o C8 do Borin sem mudar nada lá. Na ruptura, tirar a fala do amuleto (ver abaixo): pôr-se entre a criança e a estrada já sustenta o ramo.
- **C9 = 2.** Duas batidas marcadas (nivelada e sem cordão → volume de nó torto, terceiro nó, ditado dobrado). Mas "a canga nunca sai dos ombros, nem à noite" (5–7) é o C7 do Oren ("o único adulto que o jogador nunca vê sem a carga"). Pela regra de desempate do `ELENCO.md`, fica com o Oren: a carga é o C4 dele e o C2 dele nasce dela. Daren tira a canga à noite já aos 5, e a batida dos 8 fica no volume, no nó e na fala. Os 3° de inclinação dão ~4 px na ponta da canga a 30%: não contar com eles.
- **C10 = 2.** Um plano, 5 s, sem clipe novo, com a imagem da balança. Dependências: o portão não tem geometria no greybox (a "linha" é arte da T013), e o NPC fica a 1,5 m da âncora, não "exatamente na linha" (pede vaga autorada).

**Fatos dados como cânone que não conferiram:**
1. "É ali que o jogador aprende, pela primeira vez, que fechar uma missão gasta um período do dia" (C5) e "É a primeira troca de tempo do jogo" (§3): concluir a q01 já vira a manhã em tarde (ADR-0007 §1; o §1 desta ficha diz isso). Só vale como "a primeira vez que o jogador escolhe quando".
2. "na ordem natural a q02 começa com Daren no `portao_sul`" (§1, C10): pela rotina, sim. Mas o `q02…json` ancora `receber_tarefa` e `prestar_contas` em `casa_familia`. Quem alinha é o dono da q02.
3. "Esta ficha fixa uma malha só para as 3 origens, pelo mesmo motivo da Mara" (§1): herda o erro de citação da Mara. O dossiê §G fala de versões etárias; o dossiê §C e o GDD cap. 02 deixam estética e profissão variarem com a origem. Malha única é PROPOSTA.

Conferiram: id, traço, rotina, âncoras, vínculos nos dois sentidos, tópicos e papel (`NpcCatalog.cs`, `strings.pt-BR.json`); falas canônicas (`DialogueCatalog.cs`); testemunhos (`NpcMemory.cs`); objetivos e 5 moedas HIPÓTESE da q02 (`QuestCatalog.cs`, `q02…json`); `TimeOfDay.Manha = 0` e ADR-0007 §1; itens e oportunidades por destino e origem (`DestinyCatalog.cs`); portão como "fim da estrada" e 66 m do portão à praça (`AurenSceneBuilder.cs`); `PROVENIENCIA.md`; `ACERVO.csv` #108, #111, #123; Azul profundo #253850 (GDD cap. 09); a canga de 0,70 m do Aethron; `PIPELINE.md` §3.1 e §4.

**Discordância com o `ELENCO.md`:** a tabela aceita "tronco em bloco" e "botas de sola grossa" como as formas 2 e 3 do Daren. Como zona, não colidem. Como silhueta, não leem, e "bloco" aparece três vezes na tabela (Daren, Sera, Aethron). A arbitragem resolveu onde cada um fica, não se cada forma se vê.

**Outras colisões com o elenco:**
- **Amuleto:** "O que não tem explicação também pesa" é o mesmo lance de mais seis fichas. Ficam o do Borin e o do Oren.
- **Voz × Oren:** os dois homens de carga falam em máxima de duas metades. O redator mantém Daren no espelho da mesma palavra ("Tarefa dada é tarefa cumprida") e Oren em permissão e preço ("Olhar é de graça. Pegar, não.").
- **Cantil:** "o `item.cantil` era dele, da estrada" (C8) × a fala do Oren sobre o cantil, comprado num dia de feira (ficha `oren`, C8). Casam se o cantil foi comprado de fora. "Da estrada", para quem nunca cruza a linha, pede uma linha de explicação.
- **`guardioes`:** a casa cujo ofício é "guardar, patrulhar e proteger" (`origem.guardioes.oficio`) leva produção à carga numa canga. Risco para o teste de descrição; as estacas e o pavio só resolvem em parte.

**Para o G2 — o que o concept precisa provar:**
1. A balança lê nas duas poses: em T-pose (caixas soltas sob os braços, com a canga escondida atrás deles) e de braços caídos (barra acima dos ombros). No clipe de andar, o braço não atravessa as cordas.
2. As formas 2 e 3 da reescrita leem a 30%. Se o concept mantiver bloco e botas e eles não lerem, C3 continua com uma forma só.
3. Na mesma folha e na câmera do portão, ao lado do Oren: "carga para cima" × "carga para os lados" se separam. Se o Aethron entrar na folha, a balança não se confunde com o umbral em Π.
4. O conteúdo de origem fica a até 0,10 m da borda (a mesma silhueta nas 3 origens), e a carga pela metade, com a pedra, se lê como "pela metade" na câmera do jogo.
5. Sem barba e sem peito largo: ao lado do Borin, não cai no ferreiro default.

**Condições cumpridas em 2026-10-03:**
- **(a) C3 com segunda e terceira formas que leiam a 30%:** bloco e botas saíram de forma e viraram apoio. As três formas são agora as três partes da vara de carga, cada uma com contorno e lugar próprios na folha:
  - a **vara** de 1,90 m, que passa 12 px de cada mão em T-pose;
  - o **estojo do ofício**, um I de 19 × 72 px além da mão esquerda;
  - a **caixa da carga**, um bloco de 48 × 30 px além da mão direita.
  "Bloco" saiu também da lista de zonas (é da Sera e do Aethron). A ficha avisa que, se a vara for contada como uma forma só, C3 fica em 1.
- **(b) Fala 2 de C6:** "Daqui pra lá é estrada. Daqui pra cá é casa. Eu fico na linha." (o tique do Tovin) saiu. Entrou "O olho faz, a mão confere.", no espelho de verbos dele, sem lugar nem preço.
- **(c) Onde fica a ferramenta (C4 × C7):** fica no **estojo da esquerda**, sempre amarrado. A carga e o estado por período ficam na **caixa da direita**. As duas regras não se contradizem mais.
- **(d) A vara sai dos ombros à noite desde os 5:** ela fica encostada na parede de fora de `casa_familia` (C3, C4 regra 2, C9). O "nunca larga a carga" é do Oren. Saiu também a inclinação de 3°, que não lia.
- **Fatos corrigidos:**
  1. A q01 já vira o período. A q02 passou a ser "a primeira vez que o jogador **escolhe quando**" (C5, §3).
  2. O `q02…json` ancora `receber_tarefa` e `prestar_contas` em `casa_familia`. A tensão com a rotina da tarde está declarada no §1, para o dono da q02.
  3. Malha única é PROPOSTA de custo, sem a citação errada do dossiê §G (§1).
- **Amuleto:** a fala da ruptura saiu (Arbitragem 2.8). O ramo se sustenta por ele se pôr entre a criança e a estrada, e pela fala do "olharem".
- **C2 × Oren:** a ficha diz agora que a carroça descarrega de manhã e **espera no portão até o fim da tarde**. A ficha do Oren precisa dizer o mesmo (§7).
- **C1:** saiu a "linha que ele nunca cruza", que ficava sem explicação.
- **C10:** NPC na vaga do `NpcActor`, e a linha do portão declarada como arte da T013.
- **Cantil:** casado com o Oren (comprado de fora, num dia de feira), sem "da estrada".
- **`guardioes`:** o risco no teste de descrição está declarado (C4, C8).

**Conferência final (Art Director, 2026-10-03):** pendente. A condição (a) não fecha no teste oficial: o estojo e a caixa pendem de cordas sem largura declarada. O `silhueta.py` apaga traço com menos de 7 px a 512 px (~3 cm neste corpo) e depois guarda só o pedaço ligado ao corpo. As cordas somem, os dois pesos são descartados como cisco solto, e na folha sobra um homem em T-pose com 12 px de vara além de cada mão. Falta: (1) ligar o estojo e a caixa à vara por uma peça de ≥ 4 cm de largura vista de frente (tira de couro larga ou haste rígida) ou encostá-los na vara; (2) declarar a espessura da vara (≥ 4 cm), de que dependem os 12 px. As condições (b), (c) e (d), os fatos 1–3 e o casamento com o Oren (carroça, cantil, C7) estão atendidos. Para o G2: o estojo (um I vertical à esquerda, até 1,30 m) é vizinho da vara da Maelis (um I à esquerda, de 0,40 a 1,30 m), que não está na folha desta ficha.

**Correções de 2026-10-03 (W3), para o conferente:** (1) as cordas viraram tiras de couro encerado de 0,06 m de face, rígidas, de 0,15 m (estojo) e 0,25 m (caixa): a 512 px dão 14 px contra o corte de 7 px, e o caminho corpo → vara → tira → peso fica inteiro na máscara; (2) a vara tem Ø 0,05 m (12 px a 512; 6 px de espessura na folha), e os 12 px além de cada mão continuam; (3) a conta de px está no C3, e as tiras foram para a §6 (objeto à parte, o que o concept precisa provar) e para o prompt (`PROMPTS_G2.md`); (4) a vizinhança com a vara da Maelis foi para a folha da §6.

**Reconferência (Art Director, 2026-10-03, leva C1):** condições atendidas. O G1 fica aprovado por delegação, sem pendência. As duas faltas estão no corpo: (1) as tiras de couro de 0,06 m de face, rígidas, aparecem no C3, na §6 e no `PROMPTS_G2.md`, e não há mais corda; (2) a vara tem Ø 0,05 m no C3 e na §6. A Maelis está na folha da §6. Contas refeitas (caixa de 1,70 m): com a figura ocupando 80% da altura, 1 px = 4,15 mm e o corte fica em 29 mm. Vara: 12,0 px. Tiras: 14,5 px. Estojo: 38 px. Caixa: 60 px. Na folha: vara de 6 px, tiras de 7 px, estojo de 19 × 72 px, caixa de 48 × 30 px e 12 px de vara além de cada mão. Tudo confere. Premissa: com 2,1 m de largura em T-pose, a figura não chega a 80% da altura numa imagem 1:1 e fica em ~73%. Mesmo assim a vara dá 11,0 px e as tiras 13,2 px, e o caminho corpo → vara → tira → peso fica inteiro. Ressalva para o G2, sem pendência: numa imagem 2:3 em pé (~49%), a vara cai para 7,3 px, no limite do corte. O concept deve sair em 1:1 ou deitado.

## 6. Encaminhamento

**Para o G2, só depois de aprovado:**
- **vistas:** frente, perfil, costas e 3/4 verdadeiros em T-pose; fundo neutro; linha de chão; 1,70 m marcado. A vara de carga também é desenhada à parte, com o soquete no `UpperChest`, em três situações: em T-pose, de braços caídos e encostada na parede (noite).
- **silhueta:** vara, estojo e caixa, em preto a 30%, embaralhada com Mara, Borin, Oren, Tovin e Maelis (o estojo é um I à esquerda, como a vara dela). No `silhueta.py`, `@altura_m` é a caixa inteira da figura (Arbitragem 2.6): Daren entra em `@1,70`, com a largura de 1,90 m da vara. Também um render na câmera do jogo no portão sul, a ~6 m e da praça (~66 m), e um à noite, com a vara na parede.
- **o concept precisa provar:**
  - em T-pose, a vara lê além das mãos e o estojo e a caixa se separam como I e bloco;
  - as tiras de 6 cm aparecem de face, de frente, e ligam o estojo e a caixa à vara (se o concept trouxer corda fina, o `silhueta.py` descarta os pesos: refazer);
  - de braços caídos, a barra acima dos ombros lê;
  - a vara não lê como o umbral do Aethron, que sobe em Π;
  - ao lado do Oren, no portão, carregar para cima e carregar para os lados se distinguem;
  - nenhum conteúdo de origem passa 0,10 m da borda da caixa, e a caixa pela metade com a pedra lê como "pela metade" na câmera do jogo;
  - no clipe de andar, o braço não atravessa as tiras, e a vara de 1,90 m não entra nas paredes nos percursos do `AurenSceneBuilder`;
  - sem barba e sem peito largo, para não cair no ferreiro default ao lado do Borin.
- **paleta:** vara e caixa em madeira crua neutra; estojo em couro escuro; colete de lona encerada escura ("com chuva ou com sol"); cordão em terracota #A86D52, o pano da casa (o mesmo da manta da Mara); calça em terra neutra; botas em couro escuro. Proibidos: turquesa #86C8C9, dourado #D6B36A, violeta #9777B8.
- **objeto à parte:** a vara (1,90 m × Ø 0,05 m), o estojo (0,16 × 0,16 × 0,60 m, tira de 0,06 × 0,15 m) e a caixa (0,40 × 0,25 × 0,25 m, tira de 0,06 × 0,25 m), cada um nos estados de C4; o cordão (0,40 m, conta, com 1, 2 e 3 nós); os 3 conteúdos de origem; a pedra da Vida Árdua; o volume pequeno de nó torto (aos 8).
- **orçamento** (`PIPELINE.md` §4, HIPÓTESE v0):
  - Npc: 8 000 tris, 2 materiais, textura 1024, 55 ossos sem secundário;
  - vara com estojo e caixa: Prop médio, ≤ 2 000 tris, 1 material, 512 px, LOD1 ≤ 50%;
  - conteúdos, pedra, volume e cordão: Props pequenos, ≤ 500 tris e 256 px cada;
  - tudo rígido, sem balanço.
- **custo fora da arte:**
  - prender prop a osso e soltá-lo na âncora à noite (conferir se o `NpcActor` já faz; se não, é código novo pequeno);
  - o componente de peça ligada por evento (pendência do `ELENCO.md`);
  - a condição de diálogo por destino (pendência do `ELENCO.md`);
  - 4 falas de destino e 2 novas de C6;
  - nenhum clipe novo.
- **testes:**
  - troco com Mara no B06;
  - troco com Oren no portão sul (os dois usam a âncora): não podem ler como dois carregadores do mesmo molde;
  - troco com Borin na q06, como pede o parecer do Borin: Daren faz a olho e não "lê o risco", então a cena tem de quebrar;
  - descrição, com atenção à origem `guardioes`;
  - o protótipo PROTOTIPO atual de Daren não serve de entrada (ADR-0008).

**Para o G3:** bloco `### daren` em `PROVENIENCIA.md` com `g1:`, `g2:`, `entrada:` (concept em `arte/referencias/daren/` + SHA-256), `ferramenta`, `plano` pago, `licenca`, `termos_url`, `g3:`. A vara, os pesos, os conteúdos e o cordão entram como `Prop` com bloco próprio. `referencias_terceiros`: nenhuma usada nesta ficha.

## 7. Ligações com outras fichas

**PROPOSTAS que envolvem outros personagens:**
- **Mara:** é o par. Daren guarda a ponta sul do eixo de Auren (o portão), Mara a norte (o vão do bosque). As contradições se espelham: ele prende as mãos e solta os pés; ela prende os pés e deixa a casa aberta. O cordão é terracota, o pano da casa.
- **Oren:**
  - Oren dá o segundo nó no cordão do recado (`cumprir_tarefa`).
  - **A ficha do Oren precisa dizer** que a carroça que descarrega de manhã espera no portão até o fim da tarde, como diz o C2 daqui (pedido do parecer). Hoje ela diz só que a carroça "descarrega no portão e volta pela estrada".
  - O cantil da Vida Normal foi comprado dele (ficha do Oren, C8).
  - Os dois carregam e dividem o portão: Oren carrega para cima (torre nas costas) e nunca larga a carga; Daren carrega para os lados (vara de ombro) e a deixa na parede à noite. Nenhum usa a forma do outro.
- **Borin:** Daren faz a olho, Borin confere. "O olho faz, a mão confere" (C6) não revela o segredo da q06.
  - O `brinquedo_entalhado` (serena) foi entalhado por Daren, e é nele que o Borin aponta o entalhe torto.
  - A `espada_de_madeira` (guardioes) foi feita por Daren, e é a que o Borin acha "fora de medida".
  - A `foice_pequena` (agricultores) também foi feita por Daren, e a prova da família no aro é de quando o Borin a acertou.
  - Em `artesaos`, foi Daren quem abriu a `oportunidade.oficina_de_borin`.
  - Tudo isso fecha com o C8 do Borin, sem mudar nada nele.
- **Tovin:** que Daren falou com o Tovin sobre a vaga de aprendiz já é cânone (`aos_oito`), e a ficha do Tovin usa essa fala no C9. O tique de lugar e linha é dele; Daren não fala de linha. PROPOSTA: a primeira ferramenta que a criança segura de verdade é a do treino, não a do Daren.
- **Nilo:** a variante normal do C8 do Nilo ("O recado espera. O bosque, não.") se opõe ao ditado do Daren. Nada muda aqui.
- **Aethron:** os dois têm madeira nos ombros. A do Aethron sobe em Π e emoldura a cabeça; a do Daren é uma vara reta, mais larga que os braços, com pesos que descem. O coordenador confere.
- **Avatar:** as ferramentas pequenas de origem são cópias a olho das de Daren. O `cantil` (normal) foi comprado por ele, e a `corda_puida` (dificil) era da vara.

**Zonas de silhueta ocupadas** (depois das duas arbitragens do `ELENCO.md`):
- linha horizontal na altura dos ombros, mais comprida que os braços abertos (1,90 m), sem passar da cabeça;
- além das mãos, um peso diferente em cada ponta, no mesmo nível: um I alto à esquerda e um bloco deitado à direita, os dois a mais de 0,5 m do quadril. Pela Arbitragem 2.3, ficam e passam pela conferência do G2 com o aro do Borin;
- 1,70 m, só como apoio;
- livres de propósito: cabeça, pescoço (à vista), tronco (sem "bloco"), quadril e pernas;
- à noite, nenhuma: a vara fica encostada na parede de `casa_familia`.

**Por que a vara tem as três formas (para o coordenador):** depois das duas arbitragens, o elenco adulto já ocupa:
- cabeça: Lysa, Oren, Maelis e Borin;
- ombros: Mara;
- braços: Borin, Mara e Lysa;
- flanco: Eira;
- cintura: Maelis;
- quadril e coxa por fora: Borin;
- coxas: Lysa;
- joelhos e canelas: Tovin;
- vão das pernas: Oren;
- costas: Oren e Tovin.
Os pés não leem a 30% (parecer). O que sobra para Daren é o espaço além das mãos, e a vara o ocupa com três contornos.
