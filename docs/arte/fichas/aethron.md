# Ficha G1 — `aethron`

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md)). Par desta ficha: [`simbolo_limiar`](simbolo_limiar.md).
> Fato com fonte é cânone. **PROPOSTA** = ideia desta ficha, não está nos docs e não vale até ser aprovada.

## 1. Identificação

- **id:** `aethron` (chave `limiar.aethron` em `strings.pt-BR.json`, `LimiarRoteiro.FalanteKey`). Não está no `NpcCatalog` nem na regra de ids do `PIPELINE.md` §6 (ver §6 desta ficha).
- **categoria:** Npc, base adulta, fora de Auren **(PROPOSTA)**: personagem não jogável que só existe no Limiar.
- **onde aparece no slice:** B01, o Limiar (`SLICE` §2). Hoje é um palco desligado dentro da Bootstrap, com câmera própria por cima da do jogo, fundo Azul profundo e o símbolo, **sem modelo de Aethron** (`EntradaSceneSetup.PalcoDoLimiar`). No GDD é ele também quem apresenta destinos e origens (GDD cap. 02; dossiê §C: "a entidade mostra três opções"), mas as telas do B02 e do B03 desligam o palco (`EntryFlow`, `Tela.Destino`). Não aparece em Auren: "Aethron não reaparece; o Limiar apenas ecoa" (`SLICE` B11). Tela de título: o palco liga com o símbolo ao fundo (`EntryFlow.Titulo`); Aethron nela é **PROPOSTA** (C4).
- **cânone de partida:**
  - Guardião do Limiar; apresenta destinos e não controla necessariamente toda a Trama; associação narrativa: "mistério de suas informações" (GDD cap. 08).
  - Não assumir que seja onisciente, benigno ou antagonista (dossiê §I). As falas não podem prometer que o jogador é o escolhido (`SLICE` B01). O teste `StringsCoberturaTests.Limiar_FalasEscritas_SemEscolhidoNemDificuldade` reprova "escolhid", "predestinad", "fácil", "difícil" e "extrem" em toda fala do Limiar.
  - O Limiar é a passagem anterior ao nascimento, normalmente esquecida (GDD cap. 08); quase ninguém retém lembrança (dossiê §I). A escolha feita ali é imutável (GDD cap. 02, REGRA BASE; B05 é "o ponto sem volta").
  - Seis falas escritas (`limiar.fala.1` a `6`). O jogador só responde com o botão de seguir e pode reler a fala anterior com "Voltar" (`LimiarRoteiro.cs`; aceite do `SLICE` B01). A voz que já existe: define pela negação seguida de "só" ("não há chão nem céu: só a passagem"; "não é prêmio nem castigo: é só o lugar de onde você começa"), concede com "mas" ("Guardo a passagem, mas não a teci"), recusa explicar ("não cabe a mim dizer por que está") e é impessoal ("Não é por você").
  - Paleta (GDD cap. 09): Dourado #D6B36A é do Limiar e da ascensão; Turquesa #86C8C9, da Trama; Violeta #9777B8, das anomalias; Azul profundo #253850, de menus e elementos arcanos, é o fundo do palco (`EntradaSceneSetup.cs`).
  - Limiar mínimo: uma cena, a fala de Aethron e o símbolo (ADR-0007 §5). Som: identidade etérea do Limiar e da Trama (dossiê §J).
  - Nenhum NPC de Auren sabe `topico.limiar` (`DialogueCatalog.cs`, `NpcCatalog.cs`).
  - Acervo: `cenario:limiar` #160, "santuário celeste com figura distante (Aethron?)" (`ACERVO.csv`). É referência do default a evitar (§4), não cânone.
- **autor da ficha / data:** Concept Art Lead (agente), 2026-10-03
- **estado:** aprovado G1 por delegação (ADR-0010, 2026-10-03)

## 2. Critérios C1–C10

**C1 — Gancho.** O porteiro de rosto comum que é ele mesmo a folha fechada de uma porta, com o batente dourado erguido dos ombros, que toda alma de Eryndor encontra e quase nenhuma lembra, e que nunca vira a cabeça para o único sinal do próprio Limiar que ele não guarda. *(PROPOSTA; o encontro e o esquecimento são cânone: `limiar.fala.2`, dossiê §I)*

**C2 — Necessidade do mundo.** Cânone: toda vida de Eryndor atravessa o Limiar antes de começar e quase ninguém se lembra dele depois (`limiar.fala.2`; GDD cap. 08); ali cada um escolhe "por onde entrar na vida" (`limiar.fala.5`), e essa escolha não se desfaz (GDD cap. 02, REGRA BASE). **PROPOSTA:** uma escolha sem volta pede uma porta que fecha atrás de quem passa, e uma porta pede quem a guarde. Aethron existe por isso e só por isso: guarda a passagem e não a teceu (`limiar.fala.3`). Ele é **marcenaria** (verga, ombreira, soleira, folha: linha reta e ângulo reto) num mundo cuja estrutura é **tecelagem** (a Trama: fios, círculos incompletos, pontos conectados, GDD cap. 09). Daí a única coisa que ele não sabe ler: o círculo de fios que apareceu dentro da passagem dele (`limiar.fala.4`).

**C3 — Silhueta em 3 formas.** *(PROPOSTA)* De frente, em T-pose, as três formas são as três partes de uma porta. Nenhuma tem curva desenhada (a curva é do símbolo, o par desta ficha) e a figura é estritamente simétrica.
1. **Umbral (verga e ombreiras):** uma canga de 0,70 m sobre os ombros, de cujas pontas sobem duas ombreiras de 0,10 × 0,10 m até 0,38 m acima da cabeça, fechadas por uma verga reta de 0,70 × 0,12 m, rente, sem beiral. A cabeça fica dentro, com vão de 0,14 m de cada lado e de 0,38 m em cima. Sai do contorno do corpo. É peça rígida à parte, presa ao osso `UpperChest` (ver §6): sobrevive à T-pose e ao idle. Na folha do teste (120 px/m): verga de 84 × 14 px, vãos de 17 px.
2. **Folha:** sobreveste rígida, retângulo de 0,48 × 1,05 m do peito até um palmo abaixo do joelho, sem cintura e sem abertura, com uma fresta dourada vertical pintada no meio. Cobre a linha da cintura e o vão das pernas: de frente, o tronco é uma porta fechada com canelas por baixo. Presa à coluna, sobrevive à T-pose.
3. **Soleira:** laje de pedra de 0,90 × 0,40 × 0,15 m sob os pés descalços, gasta no meio. Sai do contorno (três vezes a largura dos pés). É objeto de cena, não do esqueleto: não depende de pose. É também o objeto de C4.

Apoio, não forma: postura a prumo (pescoço reto, ombros nivelados), cabelo curto cortado reto, mãos vazias. Zonas do Borin não usadas: não há assimetria de braço, nada pendurado no quadril, nem crânio raspado com pescoço projetado.

**Câmera do jogo:** a dele é só a do palco do B01 (FOV 45°, a 3,4 m do símbolo, olhando reto a 0,25 m; o painel de fala, com a margem, começa a ~43,6% da altura da tela; `EntradaSceneSetup.cs`, `EntryFlow.OnGUI`). Nunca a de terceira pessoa. **PROPOSTA de posição:** a 7 m da câmera (3,6 m atrás do símbolo), 2,4 m à direita, topo da soleira a 0,30 m. A figura ocupa de 48% a 92% da altura da tela, toda acima do painel, e de x 0,65 a 0,78 conforme a proporção da tela, sem encostar no símbolo, que o `EntradaSceneTests` exige centralizado (x 0,3–0,7). Num celular 1080p em paisagem: verga de ~130 × 22 px de frente (~105 px na pose real, três quartos virado, C7), vãos da cabeça de ~26 px, soleira de ~170 px de largura. Umbral e folha sobrevivem; a soleira fica só ~5 pontos acima da borda do painel e é a primeira a sumir se ele crescer.

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **A soleira.** Laje de 0,90 × 0,40 × 0,15 m, com uma cava retangular de 0,40 × 0,25 × 0,03 m gasta no meio e pintada em Dourado #D6B36A: o desgaste de toda vida que passou.
Regra: no título, o jogo lê a propriedade `Nasceu` do `EntryFlow` (destino gravado no save), a mesma que hoje troca "Começar" por "Continuar". Falso: Aethron está de pé na soleira, no título e no B01. Verdadeiro: o título mostra o símbolo e a **soleira vazia**, só a cava dourada no escuro. "Nova vida" apaga o save (`EntryFlow.RecomecarVida`) e, no B01, ele está de volta. O jogador vê, a cada abertura do jogo, que quem já nasceu não o vê mais: o esquecimento do dossiê §I desenhado. Nada é concedido; é exibição de estado, como o aro de provas do Borin.

**C5 — Regra exclusiva.** Cânone de código: é o único falante que sabe do Limiar (nenhum NPC tem `topico.limiar`), o único cuja fala se relê para trás (`LimiarRoteiro.Voltar`; nenhuma conversa de Auren tem "Voltar") e o único com quem não se escolhe o que dizer: a resposta é o próprio botão de seguir (`LimiarRoteiro`, "não há ramo"). **PROPOSTA — olhar para a lente:** é o único personagem do jogo que olha para a câmera. A alma não tem corpo (`SLICE` B01), então a câmera é ela: um look-at de cabeça do Humanoid mira a lente nas seis falas, enquanto em Auren toda conversa é entre NPC e criança, em terceira pessoa. Somado a C4, o jogador vê a regra inteira: com ele, a conversa volta e a escolha não. Relê-se qualquer fala até o fim do B01, troca-se o destino até o B05; depois da confirmação, nem a fala nem ele voltam, a não ser por "Nova vida", que apaga a vida atual.

**C6 — Voz.** *(PROPOSTA; texto final é do redator)* Mantém os tiques das seis falas (negação seguida de "só", concessão com "mas", impessoal, nunca diz o nome do jogador: "Não é por você") e acrescenta vocabulário de porta. As três passam no teste de texto do B01.
1. "Não é castigo. É só uma porta mais estreita: passa-se de lado." (B02, Vida Árdua selecionada)
2. "Essa porta range de um jeito que eu não conheço. Que abre, eu garanto. Para onde, não." (B02, Vida da Ruptura selecionada)
3. "Atravessou. Daqui em diante, sou só a porta de que quase ninguém se lembra." (logo depois da confirmação do B05, antes de Auren)

**C7 — Contradição visível.** *(PROPOSTA)* Guarda tudo o que passa pelo Limiar e não olha para a única coisa ali que não guarda. Nas seis falas, a cabeça mira a lente (C5) e o corpo fica virado três quartos para longe do símbolo, que está no centro do quadro. Na fala 4, quando nomeia o sinal, ele estende a mão aberta na direção do símbolo **sem virar a cabeça**. No título, enquanto ninguém nasceu, a mesma pose. Observável em toda fala e em toda abertura do jogo antes do nascimento. É o "não cabe a mim dizer" (`limiar.fala.4`) virado gesto: não se sabe se ele não sabe ou se não quer ver.

**C8 — Relação com o avatar por destino e origem.** *(PROPOSTA; só fala, nunca poder, dossiê §H e ADR-0004)* Cada destino é uma porta, e ele descreve a porta sem dizer que uma é melhor. Hoje tocar num cartão do B02 já leva ao B03 (`EntryFlow`, `Tela.Destino` → `Tela.Origem`), e o B03 abre com o destino no título ("Vida Árdua — em que família?") e um "Voltar" para o B02. A linha dele entra ali, no cabeçalho do B03, logo abaixo desse título: é a primeira coisa que o jogador lê depois de escolher a porta, e o "Voltar" já deixa trocar antes da confirmação, sem passo novo de seleção. As linhas:
- **serena:** "Uma porta que range menos. Não é prêmio: é só uma casa firme." (ecoa `destino.serena.descricao`)
- **normal:** "A porta da maioria. Não é pouco: é só a mais usada." (ecoa `destino.normal.descricao`)
- **dificil** (rótulo "Vida Árdua", ADR-0007 §2): fala 1 de C6. Nega o castigo, como a fala 6 já nega.
- **ruptura:** fala 2 de C6, a única porta que ele admite não conhecer. Casa com o "sinal estranho" e o "algo fora de lugar no bosque" que o jogo já promete (`destino.ruptura.descricao`) e com ele não ser onisciente (dossiê §I). Não diz que a porta é especial nem que quem passa por ela foi escolhido.
- **origem:** não ramifica, de propósito. Como o cabeçalho do B03 já é da linha do destino, a da origem vai para a tela de certeza do B05, acima de "Confirmar nascimento", igual para as três: "A casa você conhece do lado de lá. Daqui eu só vejo a porta." Ele guarda a passagem, não a família.

**C9 — Mudança dos 5 para os 8.** N/A, justificado: ele não está em Auren nem antes nem depois do salto (`SLICE` B11) e existe "antes do primeiro fôlego" (`limiar.fala.1`), fora da idade. O que o tempo muda é o que o jogo mostra dele: antes do nascimento, presente na soleira; depois, a soleira vazia no título, igual aos 5 e aos 8 (C4). O dourado que a criança vê na clareira é o eco do Limiar no símbolo, não ele (ficha [`simbolo_limiar`](simbolo_limiar.md), C4). Variante de malha: não. O pontuador decide se o N/A vale.

**C10 — Momento de cartaz.** *(PROPOSTA)* A primeira tela do jogo. Fundo chapado de Azul profundo. No meio, perto, o anel turquesa que não fecha. À direita, mais longe, um homem comum de pé numa laje de pedra gasta, com o batente dourado de uma porta erguido dos ombros, o corpo virado para longe do anel e os olhos na lente. Legenda: "Guardo a passagem, mas não a teci." Encenável no palco que já existe, com uma figura, uma laje e um idle.

## 3. Amarração

- **Destino, Grau ou Trama:** o destino muda a linha que ele diz sobre cada porta (C8): na Vida Serena ele nega o prêmio; na Vida da Ruptura, admite não conhecer a porta. Grau não se aplica no slice (ascensão fora de escopo, dossiê §E); esta ficha **não** o propõe como a entidade da ascensão Divina (GDD cap. 04), que é assunto da campanha. Trama: ele não a controla (GDD cap. 08) nem a lê inteira (`limiar.fala.3`); a forma dele (marcenaria) é o avesso da forma dela (tecelagem).
- **Decisão de jogo que ele cria:** escolher o destino sabendo que é sem volta. Ele dá à permanência do B05 (dossiê §C) um rosto e uma porta que fecha. Depois, no título, voltar a vê-lo custa a vida atual ("Nova vida"). *(PROPOSTA)*
- **Pilar:** escolher e transformar.

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| figura alada, de branco e ouro, com aro de luz atrás da cabeça | sem asas e sem nenhum círculo; o que emoldura a cabeça é um batente reto de madeira pintada, preso aos ombros, com cavilhas quadradas aparentes nos cantos |
| ancião de barba longa e manto, alto como estátua | adulto barbeado, de rosto comum e cabelo curto, com 1,90 m: escala de gente; grande é a porta, não ele |
| vulto de capuz e manto até o chão, com cajado, lanterna, chave, livro ou orbe, e mãos que brilham | cabeça descoberta dentro do umbral, sobreveste rígida que acaba abaixo do joelho, mãos vazias e sem luz |
| ser de luz, de pele dourada ou corpo de estrelas, com cabelo longo prateado ao vento e rosto belo e andrógino (o default do gerador para divindade em anime) | pele e cabelo de gente; o dourado está na madeira e na pedra, não no corpo; nada esvoaça (sem osso secundário, `PIPELINE.md` §4) |
| olho que tudo vê, olhos que brilham | olhos comuns, que miram a lente e nunca o símbolo (C5, C7); o dossiê §I proíbe tratá-lo como onisciente |
| vulto minúsculo num palácio celeste de mármore branco, ouro e violeta, entre nuvens e cascatas (acervo #160) | campo chapado de Azul profundo #253850, sem arquitetura; a única coisa sólida do Limiar é a soleira; violeta fora, porque é das anomalias |
| portão de santuário com dois travessões, o de cima saliente, de pontas curvadas para cima e pintado de vermelho | um travessão só, reto, rente às ombreiras, em Dourado chapado |
| curva, fio, anel, ponto: a linguagem da Trama | só linha reta e ângulo reto: ele é marcenaria, a Trama é tecelagem (`limiar.fala.3`) |
| sorriso de guia bondoso ou cenho de vilão | expressão neutra e atenta: nem promessa nem ameaça (dossiê §I) |

**Paleta (PROPOSTA):** Dourado #D6B36A no umbral, na fresta da folha e na cava da soleira. Folha, mangas e calças num neutro quente de valor médio (entre 50% e 65% na escala de cinza), para recortar contra o Azul profundo, que é escuro; nem branco nem marfim, para não somar "branco e ouro" ao batente em volta da cabeça. Pedra neutra na soleira. Proibidos nele: Turquesa #86C8C9 e Violeta #9777B8.

## 5. Avaliação — parecer do Art Director: 18/18 (C9 N/A), aprovado (ADR-0010)

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | N/A | 2 | **18/18** |

- **Corte:** o N/A de C9 vale. Aethron não está em Auren nem antes nem depois do salto ("Aethron não reaparece", `SLICE` B11) e existe "antes do primeiro fôlego" (`limiar.fala.1`). O que o tempo muda nele, a soleira vazia no título, já está pontuado em C4 e não conta duas vezes. Decisão do pontuador: o total é sobre os 9 critérios avaliados (18 pontos), e o corte é a proporção do ADR (70%) arredondada para cima, **≥ 13/18**, sem zero em C2–C5. **Atingido** (HIPÓTESE do ADR, a recalibrar).
- **Pontuador / data:** Art Director (agente) / 2026-10-03. Não escreveu a ficha.
- **Veredito:** **aprovado G1 por delegação (ADR-0010).** Nota cheia não quer dizer ficha sem risco. A régua satura em 2 (lacuna já registrada no parecer do Borin), e os riscos de Aethron estão no G2 e no código que a ficha pede e que hoje não existe: modelo no palco, look-at, clipe de gesto, linha sob o cartão do B02 e a regra do título. Se o dono do código recusar Aethron no palco atual, C3, C4, C7 e C10 perdem o lugar onde acontecem, e a ficha volta para nota.

**Nota por critério (1 linha cada):**
- **C1 = 2.** Tem detalhe concreto (batente dourado nos ombros, rosto comum) e conflito (guarda tudo e não olha para o único sinal que não guarda). Ressalva de redação: o gancho fala de "porta sem folha", e a forma 2 se chama "Folha". Falta escolher uma das duas imagens.
- **C2 = 2.** Sai de cânone que conferiu: a escolha sem volta (GDD cap. 02, REGRA BASE), a passagem de que quase ninguém se lembra (`limiar.fala.2`) e "guardo a passagem, mas não a teci" (`limiar.fala.3`). Ainda dá uma regra de forma própria, marcenaria contra tecelagem, que separa o guardião do signo.
- **C3 = 2.** De frente, em T-pose, a 30%, as três formas leem: o umbral (verga de 84 × 14 px, vãos de 17 e 46 px), a folha (58 × 126 px, sem cintura nem vão de perna) e a soleira (108 × 18 px). O umbral é peça à parte no `UpperChest` e não depende de pose. Não há colisão clara. A canga do Daren desce em carga e tem quase o dobro da largura (a ficha dele já registra a diferença). A aba da Lysa é disco sem montantes e sem vão. A torre do Oren é massa cheia. O que separa o Π dessas duas é o vão em volta da cabeça. A soleira é cenário, não corpo: é a forma mais fraca como silhueta do personagem, mas só ele a tem.
- **C4 = 2.** O objeto tem medida, e a regra está escrita sobre uma propriedade que já existe (`EntryFlow.Nasceu`, conferida no código). Nada é concedido. Ressalva: a soleira não muda, o que muda é quem está em cima dela. Aceito pela mesma leitura do aro do Borin, em que exibir estado conta.
- **C5 = 2.** O código já tem três exclusividades, e as três conferem: é o único que sabe do Limiar, o único com "Voltar" e o único com quem não se escolhe a fala (em Auren o diálogo tem opções, `DialogueCatalog.cs`). Soma-se uma técnica especificada: a cabeça mira a lente nas seis falas. O look-at ainda não existe em `Scripts/`; a ficha `sera` usa a mesma técnica com outro alvo, e o código pode ser um só.
- **C6 = 2.** São três falas escritas, com os tiques das seis que já existem e vocabulário de porta. Passam na regex do teste do Limiar (nada de "escolhid", "predestinad", "fácil", "difícil" ou "extrem") e nenhuma usa adjetivo de dificuldade para destino (ADR-0004).
- **C7 = 2.** A contradição aparece em ação recorrente e observável: o corpo fica três quartos longe do símbolo, a cabeça fica na lente, e na fala 4 a mão se estende para o símbolo sem a cabeça virar. Vale em toda fala e em todo título antes do nascimento.
- **C8 = 2.** São quatro linhas diferentes, uma por destino, e nenhuma diz que uma porta é melhor. A origem não ramifica, com a razão escrita. Implementação: hoje o B02 não tem cartão "selecionado", porque tocar num cartão já leva ao B03 (`EntryFlow`, `Tela.Destino` → `Tela.Origem`). A linha precisa de um passo de seleção ou vai para o cabeçalho do B03.
- **C9 — N/A** (ver o corte).
- **C10 = 2.** A tela de título se monta com uma figura, uma laje e um idle, no palco que já existe. É um quadro só, abaixo de 5 s.

**Fatos dados como cânone que não conferiram:** nenhum.

**Contas e coerência interna:**
1. O C3 diz que a soleira fica "8 pontos acima da borda do painel". O painel desenhado começa a ~43,6% da altura, ou mais, e não a 40%: a margem é `m = alvo × 0,25`, com `alvo ≥ 12%` da tela (`EntryFlow.OnGUI` e `Estilos`). A folga real é de ~5 pontos, e a soleira continua acima do painel.
2. A verga de "~130 × 22 px" é a medida de frente. Em jogo ele está três quartos virado (C7), e a verga projetada cai para ~0,57 m (~105 px a 1080p). Ainda lê; o G2 confere na pose real.
3. O C1 fala de "porta sem folha" e o C3 chama a forma 2 de "Folha" (ver C1).

Conferiram: `limiar.aethron` e `LimiarRoteiro.FalanteKey`; a ausência no `NpcCatalog`; o palco desligado com câmera própria (FOV 45°, 3,4 m, olhar a 0,25 m), o fundo Azul profundo e a nota `ponytail` sem modelo de Aethron (`EntradaSceneSetup.cs`); o palco desligado no B02 e no B03 e ligado no título, `Nasceu`, "Começar"/"Continuar", "Nova vida" e `RecomecarVida` (`EntryFlow.cs`); o "Voltar" só no Limiar e as opções de fala em Auren (`DialogueCatalog.cs`); o teste `Limiar_FalasEscritas_SemEscolhidoNemDificuldade` e a regex dele; as seis falas e os tiques citados; GDD cap. 02, 04, 08 e 09; dossiê §C, §E, §I e §J; ADR-0007 §5; o centro de x 0,3–0,7 no `EntradaSceneTests`; o ACERVO #160; `PROVENIENCIA.md` §6 (Breathing Idle, Talking sem uso, debrum dourado da Maelis); `PIPELINE.md` §3.1, §4, §6 e §11. As contas de enquadramento foram refeitas e conferem (48–92% da altura; x de 0,65 a 0,78 entre 16:9 e 20:9; verga de ~130 × 22 px a 1080p).

**Para o G2 — o que o concept precisa provar:**
1. **Contra a torre e a aba.** A folha de silhueta tem Oren, Lysa e Daren entre os ≥ 3 do elenco. O Π tem de ler como batente pelo vão em volta da cabeça (17 px dos lados, 46 px em cima). Se os vãos fecharem a 30%, ele colide com a torre do Oren, e C3 cai.
2. **Na pose real.** Render no palco do B01 com o corpo três quartos, a cabeça na lente e o painel por cima, e não só a vista de frente. Visto a 3/4, o umbral continua batente, e o dourado em volta da cabeça não lê como auréola (§4).
3. **Título.** Render da tela de título com o nome do jogo: a verga (de 8% a 10% do alto da tela) tem de ficar fora do título. Se não couber, vale a correção da §6 (recuar para 8 m ou descer 0,3 m) antes de mexer na forma.
4. **Gesto da fala 4.** O ombro sobe sob a ponta rígida da canga. Mostrar que não atravessa, já que não há osso secundário (`PIPELINE.md` §4).
5. **Olhar.** No celular, o olhar na lente se distingue do olhar das NPCs de Auren para a criança (Sera, C5): ele olha para quem joga, elas olham para baixo.
6. **Troco.** Eira no B01 (§6): a cena tem de quebrar.

**Condições cumpridas em 2026-10-03:**
- C1 e C3 usam a mesma imagem: o batente dourado nos ombros e o corpo como a folha fechada da porta (o "porta sem folha" saiu do gancho e do teste de descrição da §6).
- C3: a folga entre soleira e painel foi corrigida para ~5 pontos (o painel com margem começa a ~43,6%, `EntryFlow.OnGUI`), e a verga ficou com as duas medidas, de frente (~130 px) e na pose real três quartos (~105 px).
- C8: a linha do destino vai para o cabeçalho do B03, logo abaixo do título "<destino> — em que família?". Ali o "Voltar" para o B02 já permite trocar, sem novo passo de seleção. A linha da origem foi para a tela de certeza do B05. A §6 registra o custo novo.

**Conferência final (Art Director, 2026-10-03):** condições atendidas: C1 e C3 com a mesma imagem, as contas 1 e 2 e a linha do destino no cabeçalho do B03. Fica para o G2, sem pendência: a ficha `lysa` (G2, item 2) diz que, se a verga e a aba lerem como a mesma moldura em volta da cabeça, quem muda é o Aethron. Esta ficha não mudou, e a folha dela já tem a Lysa (G2, item 1).

## 6. Encaminhamento

**Para o G2 (concept), só depois de aprovado:**
- vistas: frente, perfil, costas e 3/4 verdadeiros, em T-pose, fundo neutro, linha de chão no topo da soleira; corpo de 1,90 m (faixa adulta 1,55–1,95 do `PIPELINE.md` §3.1), umbral até 2,40 m, soleira de 0,15 m.
- silhueta: umbral, folha e soleira em preto a 30% (`client/tools/silhueta.py`, 120 px/m), embaralhada com ≥ 3 do elenco. E dois renders no palco do B01: um com o painel de fala por cima; outro na tela de título, conferindo que a verga não fica atrás do nome do jogo, que ocupa o alto e o centro da tela (`EntryFlow`, `Tela.Titulo`). Se ficar, ele recua para 8 m ou desce 0,3 m.
- teste de descrição (2 frases): "Um porteiro de rosto comum leva nos ombros o batente dourado de uma porta, tem o corpo fechado como a folha dela e fica de pé numa soleira gasta, no escuro de antes do nascimento. Mostra a cada alma por onde entrar na vida e nunca olha para o círculo de fios que apareceu na passagem dele."
- teste do troco: pôr Eira (a outra figura que explica coisas) no B01. Tem de quebrar: ela não sabe do Limiar (`NpcCatalog.cs`).
- paleta: ver §4.
- objetos à parte: o umbral com a canga e as cavilhas; a soleira com a cava.
- orçamento (`PIPELINE.md` §4, HIPÓTESE v0): corpo como Npc (8 000 tris, 2 materiais, textura 1024, 55 ossos sem osso secundário), folha rígida pesada na coluna. Umbral e soleira como **peças à parte**, cada uma com menos de 1 m na maior dimensão, logo prop pequeno (500 tris, 1 material, 256 px); o umbral vai preso ao `UpperChest` por código. Dentro da malha, ele levaria a altura a 2,40 m e reprovaria a faixa de altura (V09) e provavelmente a razão de cabeça (V13) (`PIPELINE.md` §3 e §11).
- **custos que esta ficha pede e que hoje não existem:** o modelo no palco (a nota `ponytail` do `EntradaSceneSetup` diz que Aethron em cena levaria o Limiar a cena própria; esta ficha cabe no palco atual, e a decisão é do dono do código); look-at de cabeça na lente (código novo: hoje nenhum script faz look-at); um clipe novo de gesto (mão aberta, fala 4), sem ciclo de andar, porque ele nunca anda; Breathing Idle e Talking já estão no projeto, e Talking ainda sem uso (`PROVENIENCIA.md` §6); 6 strings novas (4 destinos, 1 origem, 1 depois do B05) e a linha do falante no cabeçalho do B03 e na tela de certeza do B05, que hoje não existe; a regra do título por `Nasceu` (C4).
- pendência de pipeline: o `PIPELINE.md` §6 só prevê id de NPC do `NpcCatalog`; falta a regra de pasta para personagem fora do catálogo (Aethron e `parceiro_treino`).

**Para o G3:** bloco `### aethron` em `PROVENIENCIA.md` com `g1:` (data, nota, pontuador), `g2:` (data, silhueta n/5, descrição, troco), `entrada:` (concept em `arte/referencias/aethron/` + SHA-256), `ferramenta`, `plano`, `licenca`, `termos_url`, `g3:`. `referencias_terceiros`: nenhuma usada nesta ficha.

## 7. Ligações com outras fichas

**PROPOSTAS que envolvem outra ficha:**
- **`simbolo_limiar` (par):** Aethron nunca olha para o símbolo (C7) e fica à direita dele, mais ao fundo (C3). Dourado é dele, turquesa é do símbolo; nenhum círculo em Aethron, nenhuma forma reta dominante no símbolo. O dourado que aparece no símbolo depois do toque é o eco do Limiar, não Aethron reaparecendo.
- **avatar (`avatar_crianca5`):** as 4 linhas por destino no cabeçalho do B03, a linha única da origem na tela de certeza do B05 e a linha depois da confirmação (C6, C8) entram no C8 do avatar. A alma é a lente: ele olha para a câmera (C5). Se o traço que a ficha `avatar` propõe para depois da fala 4 entrar no B01, Aethron não reage nem olha para o símbolo enquanto o jogador traça (C7).
- **Eira:** usada só no teste do troco (§6); nada muda nela.
- **Maelis:** o protótipo dela tem "debrum dourado" (`PROVENIENCIA.md` §6), e Dourado #D6B36A é do Limiar (GDD cap. 09). A ficha `maelis` já tira o debrum (§6); só falta trocar o protótipo.
- **todo NPC de Auren:** nenhum sabe do Limiar (cânone); esta ficha não muda isso.

**Zonas de silhueta que esta ficha ocupa:**
1. acima da cabeça: moldura reta em Π presa aos ombros, com a cabeça dentro;
2. tronco em bloco retangular sem cintura, do peito à canela;
3. laje horizontal sob os pés;
4. simetria estrita e nenhuma curva desenhada.
