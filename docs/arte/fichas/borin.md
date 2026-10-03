# Ficha G1 — Borin (`borin`, COE-NPC-003)

- **Estado:** RASCUNHO para o portão G1 (ADR-0002), 2026-09-30. Ficha-piloto: serve também para descobrir se a rubrica funciona.
- **Quem pontua:** o idealizador, em sessão separada (ADR-0002: a nota não é dada por quem escreveu). Tabela no fim.
- **Canon usado:** dossiê §G ("ferreiro exigente e leal; fabricação/metais"), GDD cap. 06 (NPC-03) e cap. 08 (Karvorn: mineração e metalurgia), missão `q06_o_segredo_do_ferreiro` (objetivos `entrar_na_ferraria`, `ajudar_borin`, `guardar_o_segredo`; flag `confianca_de_borin`), slice B07 e B15 (espada de treino de Borin: objeto e fala diferentes, estatística idêntica), `NpcCatalog` (forja de manhã e de tarde, praça à noite; vínculos com Oren e Daren), `DialogueCatalog` (ele lembra de quem ajudou na forja).
- **Concept existente:** `imagens/estudos_historicos/exec-b83da087-c6b4-470e-8dd0-536a320b6e64.png` (prancha de três vistas, rosto, luvas e avental). É o ferreiro-padrão do gênero: forte, ereto, barba, avental. O brief no fim diz o que fica e o que muda.

## O que esta ficha inventa (precisa da sua aprovação)

Nada disto está no GDD nem no dossiê. É proposta de produto, não decisão:

1. A névoa do Bosque dos Sussurros enferruja ferro comum em dias, e piorou nos últimos anos.
2. Borin veio de Karvorn e ficou em Auren quando os outros ferreiros foram embora.
3. **O segredo da q06:** Borin tempera o aço com água de uma nascente dentro do Bosque, lugar que a vila evita. O aço temperado assim não enferruja e, perto da Trama, mostra fios turquesa (o signo dos "fios entrelaçados" do GDD cap. 09).
4. Uma rotina de madrugada (ir à entrada do Bosque e voltar com o balde) e uma interação de fole na q06.

Nenhum item mexe em número de combate, em economia ou na Primeira Fratura: os fios aparecem, mas nada explica o porquê.

## Os 10 critérios

**C1 — Gancho.** O ferreiro que proíbe toda criança de chegar perto do Bosque é o único adulto que vai lá toda madrugada, e volta com um balde coberto que não deixa ninguém espiar.

**C2 — Necessidade do mundo.** Auren vive ao lado do Bosque dos Sussurros, e a névoa que desce de lá come ferro: dobradiça, enxada, lâmina de guarda. Uma vila de agricultores, artesãos e guardas sem ferro que dure para de trabalhar. Borin é quem mantém as ferramentas de Auren vivas. Aprendeu ligas em Karvorn, mas foi a água do próprio Bosque que resolveu o problema, e é exatamente o lugar de onde a vila manda as crianças se afastarem. Por isso ele mede tudo duas vezes: um erro de têmpera é uma enxada podre na próxima colheita.

**C3 — Silhueta (três formas dominantes, lidas em preto a 30%).**
1. **Braço de pinça:** o braço esquerdo, que segura a peça no fogo, vai inteiro numa manga grossa de couro acolchoado, dos dedos ao ombro, com uma ombreira alta. O braço direito, o do martelo, fica nu até o ombro. A assimetria de massa aparece de longe.
2. **Corpo alto e curvado:** alto (1,88 m), braços longos, pescoço projetado para a frente, de quem passou trinta anos olhando para dentro da forja. Nada do ferreiro baixo e largo, que lê como anão.
3. **Coque alto amarrado em pano:** o cabelo sobe num nó no topo da cabeça, enrolado num pano (longe da brasa), e cria uma saliência que nenhum outro adulto de Auren tem.

**C4 — Objeto-assinatura com regra: o balde da têmpera.** Balde de madeira com aros de cobre, tampado com pano encerado e amarrado com um nó.
- **Regra 1 (rotina):** de manhã cedo Borin carrega o balde da `entrada_bosque` até a `ferraria`. Com o balde na mão ele não conversa ("Agora não. Não.").
- **Regra 2 (produto):** o aço temperado nessa água é o **aço-de-fio**. Se `confianca_de_borin` estiver no histórico, a espada de madeira do treino (B15) vem com um anel desse aço no cabo. Perto do Símbolo do Limiar e de anomalias da Trama, o anel mostra fios turquesa. É só uma pista visual: dano, alcance e velocidade são os mesmos da espada comum.

**C5 — Técnica exclusiva: escutar o ferro.** Depois de cada sequência de golpes, Borin para, encosta o ouvido perto da peça e só então dá o último golpe. O jogador vê isso na rotina da forja. Na q06 (`ajudar_borin`) a criança cuida do fole: segura USAR para soprar e solta quando Borin levanta a pinça, em três rodadas. Errar não reprova: ele diz "De novo. De novo." e a rodada recomeça. Essa interação só existe com ele.

**C6 — Voz.** Frases curtas, sem adjetivo, e o tique de repetir a última palavra, como quem confere uma medida:
- "Não encosta. Quente. Quente."
- "Pressa é ferrugem que ainda não chegou. Não chegou."
- "O que o balde traz, o balde leva. Leva."

**C7 — Contradição visível.** De dia, na praça, ele manda as crianças ficarem longe do Bosque ("A névoa come quem é pequeno. Pequeno."). Toda madrugada, quem estiver acordado o vê voltando de lá com o balde. Ele é exigente com regra e quebra a regra mais séria da vila todo dia, porque é leal a Auren, não ao costume.

**C8 — Relação com o avatar, pelos quatro destinos.**
- **Vida Serena:** a família manda uma moeda para ele deixar a criança olhar. Ele devolve: "Pagar não é saber. Saber." A entrada na forja é pela q06, como para todo mundo.
- **Vida Normal:** Daren manda a criança levar uma dobradiça quebrada. Ele deixa ficar, se ficar quieta. É a q06 padrão.
- **Vida Difícil:** a enxada da família apodreceu e não há como pagar. Borin conserta fiado e não conta para ninguém (leal). A q06 vira a criança "pagando" com ajuda. Ele fala mais duro e age mais gentil. A recompensa é a mesma dos outros destinos.
- **Vida da Ruptura:** em `guardar_o_segredo`, a criança vê os fios no vapor da têmpera antes que ele diga qualquer coisa. Borin percebe e fica sério: "Tu viu os fios? Ninguém vê. Ninguém." O segredo passa a ser dos dois. É gancho para a Q-08, sem resposta.
- **Origem artesãos:** a oportunidade `oportunidade.oficina_de_borin` já existe no código. Borin conhece a criança pelo ofício da família e fala dela como futura aprendiz.

**C9 — Mudança no salto (5 → 8 anos).**
- **Antes (5):** não deixa a criança tocar em nada; o balde é segredo; barba escura.
- **Depois (8), com `confianca_de_borin`:** entrega a espada de treino com o anel de aço-de-fio e deixa a criança segurar a pinça ("Esquerda. Sempre a esquerda."). A manga acolchoada ganhou um remendo novo e a barba, fios brancos.
- **Depois (8), sem a flag:** cumprimenta e só. As madrugadas continuam, e a criança nunca soube.

**C10 — Momento de cartaz.** Ferraria escura. Borin mergulha o aço em brasa no balde; o vapor sobe e, por um segundo, se entrelaça em fios turquesa antes de sumir. Ele olha para a criança na porta e diz: "Tu não viu. Não viu."

## Brief de arte (para refazer o concept antes do G2)

- **Manter do concept atual:** rosto e barba, camisa azul profundo (#253850), avental de couro com alças cruzadas nas costas e botas altas.
- **Mudar (as três formas de C3):**
  1. Manga acolchoada com ombreira alta no braço esquerdo; braço direito nu. Sai o par de luvas: ele usa só a manga, com a mão direita nua.
  2. Mais alto, mais magro nos quadris e curvado para a frente, com os braços longos. Pose de referência em T, mas o corpo guarda a curvatura.
  3. Coque alto enrolado em pano no lugar do rabo baixo; barba um pouco chamuscada nas pontas.
- **Signo da Trama, discreto:** a argola de trás, onde as alças se cruzam, vira um **círculo incompleto** (argola aberta). É o único signo nele; o turquesa só aparece no efeito do aço-de-fio.
- **Prancha à parte:** o balde da têmpera (madeira, aros de cobre, pano encerado, nó de corda) e o anel de aço-de-fio num cabo de espada de madeira, com e sem os fios.
- **Paleta:** terracota (#A86D52) no couro, azul profundo na camisa, turquesa (#86C8C9) só no efeito.
- Altura dentro da faixa de NPC adulto do PIPELINE §3.1 (1,55–1,95 m).

## Riscos

- **Nome:** "Borin" aparece na linhagem dos anões de Tolkien, nos apêndices de *O Senhor dos Anéis*. Um ferreiro barbudo e forte com esse nome é o arquétipo do anão, e é mais uma razão para a silhueta alta e curvada. Os nomes do COE são provisórios (dossiê §I); vale a revisão de nomes que o GDD cap. 01 já pede.
- **Custo:** a interação do fole (C5) e o anel que reage à Trama (C4) são código novo, pequeno, mas não feito. A rotina da madrugada precisa de um período antes da manhã, ou de a manhã começar na `entrada_bosque`.

## Pontuação G1 (preencher: 0, 1 ou 2)

| # | Critério | Nota |
|---|---|---|
| C1 | Gancho | |
| C2 | Necessidade do mundo | |
| C3 | Silhueta própria | |
| C4 | Objeto-assinatura com regra | |
| C5 | Técnica exclusiva | |
| C6 | Voz | |
| C7 | Contradição visível | |
| C8 | Relação com o avatar pelos 4 destinos | |
| C9 | Mudança no salto | |
| C10 | Momento de cartaz | |
| | **Total** (passa com ≥ 14/20 e nenhum zero em C2–C5) | |

Aprovada em G1, a ficha vira o brief do novo concept, e o concept vai para o teste cego de silhueta (G2) com `client/tools/silhueta.py`.
