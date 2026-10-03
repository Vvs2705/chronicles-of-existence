# Ficha G1 — `oren`

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md)).
> Fato com fonte é cânone. **PROPOSTA** = ideia desta ficha, não está nos docs e não vale até ser aprovada.

## 1. Identificação

- **id:** `oren` (`NpcCatalog.cs`; GDD cap. 06, NPC-09)
- **categoria:** Npc, base adulta (`PIPELINE.md` §3.1, faixa 1,55–1,95 m)
- **onde aparece no slice:** B07: q02, objetivo `cumprir_tarefa` ("Levar o recado de Daren a Oren"), e q03 O Cesto Perdido, objetivo `procurar_na_praca` (`q02_uma_pequena_responsabilidade.json`, `q03_o_cesto_perdido.json`); B09: q07, objetivo `perguntar_na_vila`, com pista parcial (`q07_o_desaparecimento.json`; `SLICE` B09); B14, fala de depois do salto. Âncoras: `portao_sul` de manhã, `praca_centro` à tarde e à noite (`NpcCatalog.cs`)
- **cânone de partida:**
  - comerciante pragmático; negociação e economia (dossiê §G); "comerciante e economia" (GDD cap. 06); traço `pragmatico`; só sabe de `topico.comercio`, `topico.vila_auren` e `topico.estradas` (`NpcCatalog.cs`)
  - rotina: recebe a carga no `portao_sul` de manhã, negocia na praça à tarde, fecha as contas na praça à noite
  - vínculos: Oren é fornecedor de Borin e Borin é cliente de Oren (`relacao.fornecedor` em `oren`, `relacao.cliente` em `borin`; o rótulo descreve o dono da entrada, corrigido no código em 2026-10-03, `ELENCO.md`); Maelis (`relacao.contribuinte`); Mara é freguesa dele (`relacao.freguesa` em `mara`). **Ferro:** o ferro de Auren chega de fora na carga de Oren (ficha do Borin, aprovada no ADR-0010).
  - lembra de `evento.q02_concluida` e de `evento.q03_concluida` (`NpcMemory.cs`). A q03 grava a flag `ajudou_oren` e dá ao jogador `item.cesto_de_vime` ×1 (`rec.q03_o_cesto_perdido.item_cesto`, que não é hipótese). É opcional, expira no salto e **se encerra** se ainda estiver aberta quando a q04 conclui e Nilo some (ADR-0007 §3; `q03…json`)
  - falas já escritas (`strings.pt-BR.json`): `na_banca` ("Olhar é de graça. Pegar, não. Precisa de alguma coisa ou veio só espiar?"), `sobre_comercio` ("Tudo que chega a Auren vem pela estrada do sul e passa pela minha mão. Compro barato, vendo justo e durmo tranquilo."), `noite` ("…Moeda contada de noite não some de dia."), `cesto_achado` ("…Favor eu não esqueço: eu anoto."), `recado_feito`, `viu_nilo` ("…com um embrulho debaixo do braço: pão, pelo cheiro. Quem leva pão não pretende voltar para o almoço."), `aos_oito` ("Três anos de estrada… Você cresceu, criança."), `aos_oito_cesto` ("Ainda uso aquele cesto que você achou. Três anos de estrada, e ele não rasgou.")
  - **tensão no cânone:** o jogador ganha `item.cesto_de_vime` e, aos 8, Oren "ainda usa" o cesto achado. O item e o cesto achado não podem ser o mesmo objeto (a C4 resolve isso, como PROPOSTA)
  - economia do protótipo: só a cotidiana, de moedas e mercadorias (GDD cap. 07); moedas iniciais entre 8 (Vida Árdua) e 40 (Serena) (`DestinyCatalog.cs`, HIPÓTESE v0)
  - mapa: o `portao_sul` é o limite do mapa e do slice (`SLICE` §1.1); a rua sobe dele até a praça (`AurenSceneBuilder.cs`)
  - concept antigo: só o corpo-base de macacão (`COE-NPC-015`, `ACERVO.csv`). Pelo `ELENCO.md`, no teste de silhueta com os concepts antigos ele saiu igual ao corpo do Borin (o mesmo homem largo); o teste não está registrado em `PROJETO.md` nem em `PROVENIENCIA.md`. A bíblia v1.0 (`arte/referencias/acervo/documentos/Chronicles_of_Existence_Biblia_Prompts_Concept_Art_v1_0.md`, COE-NPC-015, não canônica) pedia "caderno de contas, bolsos e capa curta" e "evitar caricatura de trapaceiro". Protótipo Tripo: casaco malva curto, bolsa de moedas e caderno no cinto (`PROVENIENCIA.md` §6)
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

**C1 — Gancho.** Oren, que não dá nada de graça e sobe a rua toda manhã debaixo de uma torre de cestos com todo o ferro que Auren recebe, paga o favor de uma criança de cinco anos com o cesto do topo e carrega o buraco na torre pelo resto da vida. *(PROPOSTA)*

**C2 — Necessidade do mundo.** Cânone: o portão sul é a única saída de Auren para o mundo (o norte acaba no bosque); tudo o que chega passa pela mão de Oren; o ferro chega de fora na carga dele e Borin reforja (ficha do Borin, ADR-0010). **PROPOSTA:** a moeda é pouca (os números de moedas são HIPÓTESE v0 em `DestinyCatalog.cs`; nenhum doc diz que é pouca). A carroça de fora não entra na vila. O carroceiro não é de Auren: descarrega de manhã no portão, **espera ali até o fim da tarde**, carrega o que Auren manda (a produção que Daren leva ao portão à tarde, `NpcCatalog.cs`) e vira, para voltar antes do escuro. É uma por dia (o mesmo desenho da ficha `daren`, C2). O que chega para Auren sobe a rua nas costas de Oren, numa torre de cestos; o que sai, cada casa leva ao portão. Por isso ele sabe o que cada casa recebe, e por isso deduz as coisas pelo que a pessoa carrega (o pão de Nilo, `viu_nilo`). Todo o ferro que chega cabe num caixote na base da torre: a escassez de que Borin fala tem o tamanho das costas de um homem pequeno. Com pouca moeda, favor vira moeda (daí o "eu anoto"). E numa vila de uma estrada só, quem segura a carga não pode ficar devendo a ninguém: daí o pragmatismo. "Estrada", nas falas dos 8 anos, é a rua do portão até a praça, que ele sobe todo dia; não é viagem (leitura PROPOSTA).

**C3 — Silhueta em 3 formas.** *(PROPOSTA)* O corpo é seco, de 1,60 m: ombros estreitos (0,36 m) e caídos pelas alças, braços finos, sem barriga. É a resposta direta ao teste antigo: Oren não é o homem largo, e a largura dele está nas costas. A estatura é só apoio, não forma: em preto, a cabeça fica dentro do retângulo da torre, a figura é a mais alta da folha e a Lysa (1,58 m) é mais baixa que ele (Arbitragem 2, item 6, do `ELENCO.md`). Ocupa só zonas que o `ELENCO.md` deixou com ele ou livres.
1. **Torre de cestos:** armação de madeira de 0,50 × 1,10 m nas costas, da cintura até 0,35 m acima da cabeça, coroada pelo **cestinho** redondo (Ø 0,28 × 0,22 m), com o topo a 2,17 m. É a única massa centrada acima da cabeça no elenco (arbitragem 1 do `ELENCO.md`). Sai do contorno por cima e 7 cm de cada lado do tronco. *T-pose:* rígida no `UpperChest`, sobrevive. *Câmera:* numa conversa a ~2 m, o topo continua dentro do quadro (pivô a 0,94 m, 2,75 m atrás, 15°, com o campo de visão padrão; `BodyByAge.cs`, `ThirdPersonCamera.cs`). Do outro lado da praça, é a coisa mais alta em cima de uma pessoa.
2. **Pernas em parêntese:** as pernas de quem carrega peso há trinta anos, finas e arqueadas, em calção até o joelho e canelas enfaixadas. Os joelhos ficam 0,10 m mais afastados do que os tornozelos, e o vão entre as pernas vira um oval alto. Em preto, a forma é esse vão. É o contrário do Tovin, cujos canos fecham o vão no joelho, e da Sera, que não tem vão. *T-pose:* é malha, sobrevive. *Câmera:* é o que fica na altura dos olhos da criança.
3. **Bandeirola da carga:** um triângulo de pano encerado terracota, de 0,30 × 0,20 m, numa haste de madeira de 0,04 m presa à quina de cima, à esquerda, da armação, armado duro para fora. O pano passa 0,25 m além da borda da torre, entre 1,92 e 2,10 m do chão. Em preto, é um triângulo apontado para a esquerda no alto da torre, **fora** do retângulo dela (36 × 24 px a 30%). É o sinal de comerciante: de longe, Auren vê a bandeirola subir a rua do portão (~66 m) antes de ver o Oren inteiro, e sabe que a carga chegou. A zona está livre: nenhum adulto passa de 1,90 m fora do centro à esquerda. O chifre do Tovin fica à direita e vai a ~1,91 m; o Y do Nilo, à esquerda, é de criança e não passa de ~1,30 m. *T-pose:* rígida, parte do prop da torre, sobrevive. *Câmera:* numa conversa a 2 m, fica no alto do quadro, à esquerda; do outro lado da praça, é o primeiro sinal dele. **Risco:** ser contada como parte da torre; o que separa é o lugar (para o lado e fora do retângulo, não em cima) e a forma (triângulo, não bloco).
Apoio, não conta como forma: a estatura (1,60 m; aos 5 anos, a cabeça da criança chega ao peito dele, e aos 8, ao ombro, C9), a alça de couro na testa (ele carrega parte do peso com a cabeça) e o caixote do ferro na base da armação. Na faixa Baixa (ADR-0009: sem contorno toon), a torre é bloco, o vão das pernas é espaço vazio grande e a bandeirola é um triângulo cheio: os três leem.

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **A torre de cestos.** Armação de 0,50 × 1,10 m; caixote do ferro na base (0,50 × 0,25 × 0,30 m); três cestos de vime escuro empilhados; no topo, o **cestinho**, o único de vime cru, claro; ganchos nos montantes, onde ele pendura a mercadoria à tarde, na praça.
Regra: lê o que o código já sabe.
- **Enquanto o cesto está perdido** (do começo do jogo até `evento.q03_concluida`), a prateleira do meio da armação fica vazia. De frente, o corpo cobre o vão; de costas e de 3/4, que é o que a câmera do jogo mais mostra, a torre fica vazada no meio. É a q03 à vista antes de ser oferecida. Se a q03 nunca for feita (ignorada, ou encerrada quando Nilo some, ADR-0007 §3), o vão fica os três anos; aos 8, Oren pôs ali um cesto novo, claro (C9).
- **O cestinho é o `item.cesto_de_vime`** que a q03 já concede. Com `evento.q03_concluida` no histórico, ele sai do topo da torre e não volta: a torre fica 0,22 m mais baixa pelo resto do jogo, inclusive aos 8 anos. O cesto achado volta para a prateleira do meio e fecha o vão ("Ainda uso aquele cesto que você achou"); o cestinho é o pagamento, e o buraco no topo é a anotação ("Favor eu não esqueço: eu anoto"). O favor muda o buraco de lugar: do meio da torre para o topo. Sem a q03, o cestinho fica lá. É o mesmo mecanismo da prova no aro do Borin (adereço ligado ou desligado por evento do histórico), com o sentido trocado: Borin acrescenta, Oren desfalca.
- **O caixote do ferro:** de manhã, no `portao_sul`, aparecem nele as pontas das barras de Borin; à tarde e à noite, está fechado e vazio (lê o período). É a regra do ferro que chega de fora, na única forma que o jogador consegue ver.
Nada concede poder; o item já existe e a concessão já é idempotente.

**C5 — Regra exclusiva.** *(PROPOSTA)* **"De onde veio".** É o único que avalia o que a criança carrega: diz de onde a coisa veio e quanto vale (Arbitragem 2, item 9, do `ELENCO.md`; Borin também reage a item, mas não avalia). São duas opções na mesma conversa, sem estado guardado:
- **"Quanto vale isso?"** aparece se a criança carrega o item do destino que tem fala: `item.brinquedo_entalhado` (serena), `item.cantil` (normal), `item.faca_gasta` (Vida Árdua) ou `item.amuleto_rachado` (ruptura).
- **"E isto aqui?"** aparece se ela carrega o item da origem: `item.martelo_leve`, `item.foice_pequena` ou `item.espada_de_madeira`.
As mantas e a `item.corda_puida` não têm fala e nunca são escolhidas; por isso as falas de origem da C8 sempre aparecem, ao lado da do destino. Cada opção leva ao nó daquele item. Oren não compra: "De criança eu não compro. Amanhã a tua casa vem pedir de volta." O `item.amuleto_rachado` é o único que ele não consegue rastrear (C6, fala 3). Nada muda de estado (R8, `SLICE` §5.1). Custo: a `Condicao` ainda não lê o inventário (`DialogueGraph.cs`); pede uma condição nova ("tem o item"), o teste dela e 7 nós de fala.

**C6 — Voz.** *(PROPOSTA; o texto final é do redator)* Máxima de balcão em duas metades, como as já escritas ("Olhar é de graça. Pegar, não."); deduz pelo que a pessoa carrega; chama o jogador de "criança".
1. "Favor se paga no ato. Leva o menor da torre. O buraco que ficar é a minha anotação." (q03, ao entregar o cestinho)
2. "Essa lâmina entrou por aquele portão antes de você nascer. Faca gasta é faca que trabalhou: vale mais que nova, e ninguém paga por isso." (C5, `item.faca_gasta`)
3. "Cobre? Não. Pedra? Também não. … Isso não passou pela minha mão, criança. E nada entra em Auren sem passar." (C5, `item.amuleto_rachado`)

**C7 — Contradição visível.** *(PROPOSTA)* Diz que compra barato, vende justo "e durmo tranquilo" (`sobre_comercio`), e é o único adulto que o jogador nunca vê sem a carga. De manhã já está no portão com a torre nas costas; à tarde, vende de pé, com a mercadoria pendurada na própria armação; à noite, na praça, refaz as amarras da torre, nó por nó, em vez de ir para casa. Nenhum período o mostra sentado: a tranquilidade dele pesa uns 30 kg. Dá para ver em qualquer período sem animação nova, porque a torre está sempre lá; "refazer as amarras" à noite é um idle próprio opcional.

**C8 — Relação com o avatar por destino e origem.** *(PROPOSTA; só fala, nunca poder, dossiê §H; são as mesmas falas da C5; itens conferem em `DestinyCatalog.cs`)*
- **serena** (`item.brinquedo_entalhado`): "Esse não passou pela minha mão: alguém daqui fez pra você. O que não tem preço eu não vendo." Trata a criança como freguesa futura, de casa que compra sem perguntar o preço.
- **normal** (`item.cantil`; `oportunidade.recado_da_vila`, `oportunidade.feira_de_auren`): "Couro de fora, costura de fora. Saiu da minha mão pra tua casa num dia de feira." A fala serve às três origens. É a criança dos recados; a q02 é o jeito de a família falar com ele.
- **dificil** ("Vida Árdua"; `item.faca_gasta`): a fala 2 da C6. Diz o preço sem desconto: é a única criança que ele trata como freguesa de verdade, porque a casa dela sabe o peso de cada moeda.
- **ruptura** (`item.amuleto_rachado`): a fala 3 da C6. A regra dele ("tudo passa pela minha mão") quebra num único objeto; ele não sabe o que é e não inventa.
- **origem:** `artesaos` (`item.martelo_leve`; "gente de oficina e de balcão"): "Cabeça de ferro de fora, cabo daqui. Família de balcão já sabe a regra." Com essa criança, pula o "Olhar é de graça". `agricultores` (`item.foice_pequena`): "Ferro velho, foice nova. O ferro eu trouxe; o Borin fez o resto." `guardioes` (`item.espada_de_madeira`): "Madeira de Auren. Disso eu não ganhei nada."
- Custo: 7 variantes de fala (4 destinos + 3 origens), que servem à C5 e à C8 ao mesmo tempo.

**C9 — Mudança dos 5 para os 8.** *(PROPOSTA)*
- **Antes (5–7):** a cabeça da criança (1,10 m, `PIPELINE.md` §3) chega ao peito dele, e a mercadoria pendurada nos ganchos da armação fica na altura dos olhos dela ("Olhar é de graça"). A torre tem o vão do cesto perdido no meio, ou, se a q03 foi feita, o vão fechado e o buraco no topo.
- **Depois (8):** a criança (1,28 m) chega ao ombro dele (~1,30 m). Com a q03, o buraco do topo tem três anos e continua lá (falas `aos_oito_cesto` e "três anos de estrada", que são cânone). Sem a q03, a fala é a neutra (`aos_oito`) e a torre mudou: ele desistiu de esperar o cesto perdido e pôs no vão do meio um cesto novo, claro. É uma mudança dele, visível depois do salto (lê `marco_idade_8` e a falta de `evento.q03_concluida`, com o mesmo componente de peça por evento).
- **Variante de malha:** não. Mudam adereços do prop da torre. (Opcional: escurecer o cesto achado aos 8, com uma troca de material.)

**C10 — Momento de cartaz.** *(PROPOSTA)* Manhã, câmera na altura da criança, junto ao portão sul. Um homem pequeno passa debaixo de uma torre de cestos mais alta que ele; do caixote da base saem as pontas do ferro; no topo, o cestinho claro e a bandeirola contra o céu, e a torre inteira balança no passo (rígida, sem osso extra). Ele para em cima das pernas arqueadas, a sombra da torre cai na criança e ele diz, sem olhar para baixo: "Olhar é de graça. Pegar, não." Cinco segundos, um plano.

## 3. Amarração

- **Destino, Grau ou Trama:** o destino muda o que a criança carrega e, com isso, o que Oren lê nela (C5, C8). O Grau não se aplica no slice. A Trama aparece para ele como um furo na conta: o amuleto da Ruptura é a única coisa de Auren que não passou pela mão dele. Ele não sabe do Limiar (`NpcCatalog.cs`).
- **Decisão de jogo:** fazer a q03 antes de concluir a q04, porque depois Nilo some e a q03 se encerra (ADR-0007 §3), sabendo que a ajuda deixa uma marca permanente na torre dele.
- **Pilar:** escolher e transformar.

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| mercador gordo e risonho, de colete, anéis, chapéu e bigode fino (o trapaceiro que a própria bíblia v1.0 manda evitar) | pequeno, seco, de ombros caídos, sem barriga e sem joia: corpo de carregador |
| o homem largo que saiu igual ao corpo do Borin no teste de silhueta antigo (`ELENCO.md`) | seco, de 1,60 m e pernas arqueadas, com a largura nas costas e não no peito |
| toldo ou chapéu de aba larga de mascate (a faixa larga sobre a cabeça é da Lysa, `ELENCO.md`) | nada sobre a cabeça além da torre; a banca é a própria armação, com a mercadoria nos ganchos |
| caderno de contas e bolsa de moedas como assinatura (todo comerciante tem; é o protótipo atual) | a torre de cestos: a carga é a assinatura, e o favor se paga com um pedaço dela |
| capa curta de viajante que vem de longe | morador que sobe a mesma rua todo dia; a estrada de fora termina no portão |
| casaco malva (o do protótipo, que puxa para o violeta #9777B8 reservado às anomalias); dourado ou turquesa | linho marfim #E9DEC6 (ADR-0010 §5), lã marrom, alças, alça de testa e bandeirola em terracota #A86D52, vime escuro; o cestinho em vime cru cor de marfim, nunca puxando para o dourado #D6B36A |
| negociante esperto que sabe de tudo | só sabe de comércio, de Auren e de estradas (`NpcCatalog.cs`); deduz pelo que a pessoa carrega, e o amuleto é o que ele não sabe |

## 5. Avaliação — parecer do Art Director: 18/20, aprovado (ADR-0010)

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total /20 |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 1 | 2 | 2 | 2 | 2 | 2 | 1 | 2 | **18** |

- **Corte:** ≥ 14/20 e nenhum zero em C2, C3, C4, C5 — **atingido** (HIPÓTESE do ADR, a recalibrar)
- **Pontuador / data:** Art Director (agente) / 2026-10-03. Não escreveu a ficha.
- **Veredito:** **aprovado por delegação (ADR-0010), com três condições antes de encomendar o concept:** (a) C3 ganha uma terceira forma fora do contorno frontal da torre; (b) C5 diz qual item de nascimento entra (hoje cai sempre na manta) e como as falas de origem são alcançadas; (c) os fatos abaixo são corrigidos. C9 fica em 1 sem bloquear.

**Nota por critério (1 linha cada):**
- **C1 = 2.** Torre, ferro, o cestinho do topo e o buraco para sempre, com conflito (não dá nada de graça × paga com um pedaço da própria carga). Uma frase.
- **C2 = 2.** A cadeia é de Auren: uma estrada só, o ferro de fora na carga dele, "tudo passa pela minha mão", a dedução pelo que se carrega (`viu_nilo`) e o "eu anoto". Explica o pragmatismo e o favor como moeda. Ressalva: a premissa que segura tudo, "a carroça de fora não entra na vila", não tem razão de mundo (a rua é plana e tem 7 m). Uma linha dizendo por quê fecha o buraco.
- **C3 = 1.** A torre é a forma mais forte do elenco e é dele por arbitragem; as pernas em parêntese leem como vão oval. A estatura não lê. De frente, cabeça e tronco ficam dentro do retângulo da torre (0,50 m contra ombros de 0,36 m). Com o cestinho, a silhueta tem 2,17 m, a mais **alta** da folha; sem ele, 1,95 m. E "o homem mais baixo" é recorte de gênero: a Lysa (1,58 m) é mais baixa, e silhueta não tem gênero. Duas formas, e uma some no contorno.
- **C4 = 2.** Objeto com medida e duas regras escritas com ids (`evento.q03_concluida` tira o cestinho para sempre; o período mostra ou esconde o ferro). Ainda resolve a tensão de cânone entre o item da q03 e "Ainda uso aquele cesto". Falta dizer o que a torre mostra enquanto o cesto está perdido (antes da q03 e, se a q03 nunca for feita, por três anos).
- **C5 = 2.** Técnica especificada e dele. A ficha `mara` reivindicava a mesma leitura; pela regra de desempate do `ELENCO.md`, ela fica com o Oren, porque a avaliação "de onde veio e quanto vale" nasce do C2 e de uma fala canônica (`viu_nilo`). Duas correções. (1) "o único NPC cujo diálogo lê o inventário" vira "o único que avalia o que a criança carrega", porque Borin e Mara também reagem a item. (2) "Um item por conversa, primeiro o do destino" pega sempre a manta, que é o primeiro item de todo destino no `DestinyCatalog`, e a manta não tem fala; sem estado guardado, as falas de origem do C8 nunca aparecem.
- **C6 = 2.** Três falas escritas, máxima de balcão e "criança". A fala 1 casa o "favor se paga no ato" com o canônico "eu anoto" pelo buraco na torre. Vizinho: o Daren também fala em ditado de duas metades sobre carga; manter o Oren em permissão e preço.
- **C7 = 2.** Contradição observável em todo período sem clipe novo (a torre está sempre lá), com uma ação noturna opcional. "O único adulto que o jogador nunca vê sem a carga" hoje é falso, porque o Daren, dos 5 aos 7, não tira a canga nem à noite. Pela regra de desempate, fica com o Oren, e a correção é na ficha `daren`.
- **C8 = 2.** Quatro destinos pelo item de nascimento e três origens; os ids conferem. "A tua casa pagou em feijão" (normal) supõe `agricultores`, mas a Vida Normal tem as três origens.
- **C9 = 1.** As duas batidas estão marcadas, mas quem muda é a criança (chega ao peito dele, depois ao ombro) e uma fala que já é cânone. O Oren em si (corpo, torre, rotina, adereço) é o mesmo aos 5 e aos 8, e o buraco de três anos é o mesmo buraco. Para 2: uma mudança dele, visível depois do salto.
- **C10 = 2.** Um plano, 5 s, encenável: torre, ferro, cestinho contra o céu e a fala canônica. "O cestinho claro balança" vai contra o orçamento (rígido, sem osso extra): o que balança é a torre inteira, no passo.

**Fatos dados como cânone que não conferiram:**
1. "a moeda é pouca" (C2, na parte "Cânone"): os números de moedas são HIPÓTESE v0 (`DestinyCatalog.cs`), e nenhum doc diz que a moeda é pouca. É PROPOSTA.
2. "Oren é o primeiro adulto que ela alcança" (C9): a Lysa tem 1,58 m; aos 8, a criança alcança o ombro dela também.
3. "diferentes das pernas em pera do Daren" (§7): a ficha `daren` tirou as pernas em pera na reescrita (eram a base-balão da Lysa).
4. "Na folha do teste, todos ficam no mesmo chão e na mesma escala (`client/tools/silhueta.py`), e é aí que a estatura lê" (C3): o `silhueta.py` escala a figura inteira para a altura informada (`altura_m × px_por_metro`). Com a torre, a entrada é `@2,17` (ou `@1,95` sem o cestinho), e a altura do corpo não aparece.
5. A bíblia v1.0 é citada sem caminho: está em `arte/referencias/acervo/documentos/Chronicles_of_Existence_Biblia_Prompts_Concept_Art_v1_0.md` (COE-NPC-015), e o texto citado confere.
6. "no teste de silhueta com os concepts antigos saiu igual ao corpo do Borin" (§1, §4): a única fonte é o `ELENCO.md`; o teste não está registrado em `PROJETO.md` nem em `PROVENIENCIA.md`. Citar o `ELENCO.md`.

Conferiram: id, traço, rotina, âncoras, vínculos (com a correção do rótulo no `NpcCatalog.cs` da árvore de trabalho), tópicos e papel; falas canônicas (`strings.pt-BR.json`, `DialogueCatalog.cs`); testemunhos (`NpcMemory.cs`); recompensa, flag e encerramento da q03, e os objetivos da q02 e da q07 (`QuestCatalog.cs`, `content/quests/`); itens de nascimento (`DestinyCatalog.cs`); `ACERVO.csv` COE-NPC-015; `PROVENIENCIA.md`; o topo de 2,17 m dentro do quadro numa conversa a 2 m (borda de cima a ~2,9 m, com o FOV padrão); a porta de 2,2 m e `PIPELINE.md` §4 e §11 (V09 e V10 medem todos os renderizadores); ADR-0009 (faixa Baixa sem contorno).

**Discordância com o `ELENCO.md`:** as arbitragens 2 e 5 deram ao Oren "estatura: o homem mais baixo" como terceira forma. Em preto, ela não existe: a cabeça fica dentro da torre, a silhueta é a mais alta da folha e a Lysa está abaixo dele. A arbitragem resolveu um empate de números, não uma leitura.

**Outras colisões com o elenco:**
- **Torre × umbral do Aethron:** os dois são estrutura centrada acima da cabeça (Π de 0,70 m, 0,38 m acima, vãos de 17 px; torre de 0,50 m, 0,35 m + cestinho). O que separa é o vão (Π vazado, torre cheia). Só importa se os dois forem para a mesma folha; nunca dividem tela.
- **Amuleto:** a fala 3 do C6 é uma das duas que devem ficar (com a do Borin), porque quebra a regra do C2 dele; as outras seis fichas devem largar o amuleto.

**Para o G2 — o que o concept precisa provar:**
1. Folha de silhueta com o Oren entrado em `@2,17` e em `@1,95`, as duas leituras que o jogo mostra, no mesmo chão de Borin, Daren, Tovin e Lysa.
2. A terceira forma nova fica fora do contorno frontal da torre e lê a 30% em T-pose. Pela régua, nada que fique dentro do retângulo de 0,50 m conta.
3. As pernas em parêntese: o vão oval lê a 30% em T-pose e não se confunde com os canos do Tovin.
4. O buraco do cestinho (0,22 m) se vê numa conversa a 2 m, na câmera do jogo e no celular em paisagem. É a regra de C4.
5. Torre × umbral do Aethron, se embaralhados: cheio × vazado se separam.

**Condições cumpridas em 2026-10-03:**
- **(a) Terceira forma fora do contorno frontal da torre:** a estatura virou apoio. Entrou a bandeirola da carga, um triângulo terracota que sai 0,25 m para a esquerda além da borda da torre, entre 1,92 e 2,10 m, numa zona sem dono no elenco (C3). As pernas arqueadas ficam como forma 2. A caixa da figura no teste continua `@2,17` com o cestinho; sem ele, a bandeirola vira o topo, `@2,10` (§6).
- **(b) C5:** são duas opções na mesma conversa, sem estado guardado: o item do destino ("Quanto vale isso?") e o da origem ("E isto aqui?"). As mantas e a corda puída não têm fala e nunca são escolhidas, então as falas de origem sempre aparecem. Passou a dizer "o único que avalia o que a criança carrega" (Arbitragem 2, item 9). A reação ao amuleto ("não passou pela minha mão") fica com ele (Arbitragem 2, item 8).
- **(c) Fatos:**
  1. "a moeda é pouca" passou para PROPOSTA, com a nota de HIPÓTESE v0 dos números (C2);
  2. a C9 não diz mais "o primeiro adulto que ela alcança": a Lysa é mais baixa;
  3. a menção às pernas em pera do Daren saiu do §7;
  4. a C3 não diz mais que a estatura lê na folha: o `silhueta.py` escala pela caixa, e a entrada do Oren é `@2,17`;
  5. a bíblia v1.0 é citada com o caminho;
  6. o teste antigo de silhueta é citado pelo `ELENCO.md`.
- **Outras correções do parecer:**
  - a C2 diz por que a carroça não entra: o carroceiro é de fora, descarrega de manhã, espera no portão até o fim da tarde e volta antes do escuro, como na ficha `daren`;
  - a C4 diz como a torre fica com o cesto perdido (prateleira do meio vazia, vazada de costas; sem a q03, por três anos);
  - "pagou em feijão" virou "num dia de feira", que serve às três origens da Vida Normal;
  - a C10 deixou de fazer o cestinho balançar: balança a torre inteira, rígida;
  - a C9 ganhou uma mudança do próprio Oren aos 8: sem a q03, ele pôs um cesto novo no vão do meio.

**Conferência final (Art Director, 2026-10-03):** condições atendidas. Estão no corpo: (a), a bandeirola fora do retângulo da torre; (b), as duas opções do C5; (c), os fatos 1–6; as correções de C2, C4, C8, C9 e C10; e a Arbitragem 2, itens 6, 8 e 9. Ressalva de texto, sem pendência: o §7 ainda diz que a ficha `daren` traz o cantil "pago em feijão" e a carga que não sai dos 5 aos 7. As duas coisas já foram corrigidas lá.

**Correções de 2026-10-03 (W3), para o conferente:** o §7 deixou de dizer que a ficha `daren` traz o cantil "pago em feijão" e a carga que não sai dos 5 aos 7. Agora diz o que as duas fichas dizem: o cantil foi comprado do Oren num dia de feira, e a vara do Daren fica na parede à noite desde os 5. Nada mais mudou.

**Reconferência (Art Director, 2026-10-03, leva C1):** ressalva de texto resolvida, sem mudar forma nem medida. O §7 diz o que as duas fichas dizem: o cantil foi comprado num dia de feira, e a vara do Daren fica na parede desde os 5. Nenhum número mudou (diff).

## 6. Encaminhamento

**Para o G2 (concept), só depois de aprovado:**
- vistas: frente, perfil, costas e 3/4 em T-pose, fundo neutro, linha de chão, com as alturas de 1,60 m (corpo) e 2,17 m (torre) marcadas. O topo passa sob a verga de 2,2 m das portas de Auren (`PIPELINE.md` §3.2)
- silhueta: torre de cestos, pernas em parêntese e bandeirola, em preto a 30%. A caixa da figura entra em `@2,17` com o cestinho e em `@2,10` sem ele (aí a bandeirola é o topo); são as duas leituras que o jogo mostra. Na mesma folha: Borin, Daren, Tovin, Lysa e, se houver, Aethron (cheio × vazado). Mais um render de costas na câmera do jogo, numa conversa a 2 m, para mostrar o vão do cesto perdido
- paleta: linho marfim #E9DEC6, lã marrom, alças e bandeirola em terracota #A86D52, vime escuro, cestinho em vime cru cor de marfim, ferro escuro neutro; reservados e proibidos nele: turquesa #86C8C9, dourado #D6B36A, violeta #9777B8
- objeto à parte: armação de 0,50 × 1,10 m com os ganchos e a prateleira do meio, cestinho de Ø 0,28 × 0,22 m, caixote de 0,50 × 0,25 × 0,30 m com as pontas das barras, bandeirola de 0,30 × 0,20 m na haste de 0,04 m
- orçamento (`PIPELINE.md` §4, HIPÓTESE v0): corpo Npc com 8 000 tris, 2 materiais, textura 1024 e 55 ossos; as pernas arqueadas vêm na pose de repouso da malha, sem osso extra. **A torre não pode estar no FBX do corpo:** o V09 mede todos os renderizadores, e Oren passaria a medir 2,17 m, fora de 1,55–1,95 (`PIPELINE.md` §11; `ArtImportValidatorRules.cs`); o V10 também sairia do centro. A torre, com a bandeirola, é um **Prop médio** (`oren_torre`: 2 000 tris, 1 material, 512, LOD1), presa ao `UpperChest` no prefab. O cestinho, as barras e os dois cestos do meio (o achado e o novo dos 8) são Props pequenos separados (500 tris cada), porque ligam e desligam. São quatro renderizadores a mais: as draw calls precisam ser medidas (teto de 200, §4.1)
- custos que esta ficha pede: nenhuma animação obrigatória (idle de carga e "refazer as amarras" são opcionais); a condição de inventário no diálogo (C5); 8 falas novas; adereços que ligam e desligam por evento (cesto do meio, cestinho, cesto novo dos 8) e por período (barras), todos pelo componente único de peça por evento do `ELENCO.md`
- testes a registrar: silhueta (≥ 4/5) com Borin, Daren, Tovin e Lysa na folha; descrição; troco com Borin (a cena do cestinho tem de quebrar)

**Para o G3:** blocos `### oren`, `### oren_torre`, `### oren_cestinho`, `### oren_cesto` e `### oren_ferro` em `PROVENIENCIA.md`, com `g1:` (data, nota, pontuador), `g2:` (data, silhueta n/5, descrição, troco), `entrada:` (concept + SHA-256), `ferramenta`, `plano`, `licenca`, `termos_url` e `g3:`. `referencias_terceiros`: nenhuma usada nesta ficha.

## 7. Ligações com outras fichas

PROPOSTAS desta ficha que envolvem outro personagem:
- **Borin:** Oren traz o ferro de Borin, e as barras aparecem de manhã no caixote da torre (C2, C4); as falas da C8 dizem "o ferro eu trouxe; o Borin fez o resto". Bate com o vínculo corrigido no código em 2026-10-03 (Oren `fornecedor`, Borin `cliente`, `ELENCO.md`).
- **Borin e o amuleto:** Borin se recusa a medir o amuleto; Oren não consegue rastreá-lo (C5, C8). São duas reações diferentes ao mesmo objeto, e a ficha do avatar precisa saber das duas.
- **Avatar:** o cestinho (`item.cesto_de_vime`) pode aparecer nas costas da criança. A decisão é da ficha do avatar, com custo de encaixe nas bases de 5 e de 8 anos; esta ficha não pede.
- **Mara e Daren:** as falas da C8 dizem que "a tua casa" compra de Oren num dia de feira. Isso combina com Mara ser freguesa dele (cânone); as fichas deles não podem dizer que a família não compra dele.
- **Daren:**
  - **Carroça:** as duas fichas dizem a mesma coisa. Ela descarrega de manhã no `portao_sul`, espera ali até o fim da tarde e volta com o que Auren manda (C2 daqui; C2 da `daren`). Oren recebe de manhã e passa a tarde na praça; por isso alguém tem de avisá-lo de que a carga da casa está no portão antes de a carroça partir, que é a leitura da `daren` para o recado da q02 (o conteúdo do recado continua com o dono da q02).
  - **Cordão do recado:** Oren dar o segundo nó no cordão de Daren em `cumprir_tarefa` (ficha `daren`) cabe nele: é o "eu anoto" feito com a mão. Nada muda aqui.
  - **C7:** a frase "o único adulto que o jogador nunca vê sem a carga" fica com o Oren, pelo parecer. A ficha `daren` já tira a vara dos ombros à noite desde os 5: ela fica encostada na parede de fora de `casa_familia` (C3, C4 e C9 dela).
  - **Cantil:** as duas fichas dizem o mesmo: o cantil da Vida Normal foi comprado do Oren num dia de feira (C8 daqui; C8 da `daren`), o que serve às três origens.
- **Nilo:** a pista da q07 (o pão) é dedução pelo que ele carregava, coerente com a C5. Se a q03 se encerra porque Nilo some (ADR-0007 §3), o vão do cesto fica na torre até os 8 (C4).
- **Elenco masculino adulto:** Daren tem 1,70 m, Tovin 1,76 m e Borin 1,82 m; Oren, com 1,60 m, é o mais baixo dos homens, mas isso é apoio, não forma. As pernas em parêntese de Oren (vão oval, largo no joelho) são o oposto dos canos do Tovin (vão fechado no joelho).

Zonas de silhueta que esta ficha ocupa:
- acima da cabeça, centrada (torre coroada pelo cestinho, até 2,17 m; arbitragem 1 do `ELENCO.md`);
- alto da torre, para fora à esquerda, de 1,92 a 2,10 m (a bandeirola);
- vão entre as pernas, em oval alto (pernas em parêntese);
- costas inteiras, com o vão do cesto perdido no meio enquanto a q03 não fecha (vista de trás).
