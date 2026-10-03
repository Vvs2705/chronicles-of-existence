# Ficha G1 — `sera`

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md); a régua 0/1/2 está lá).
> Fato com fonte é cânone. **PROPOSTA** = ideia desta ficha, não está nos docs e não vale até ser aprovada. Par desta ficha: [`nilo`](nilo.md).

## 1. Identificação

- **id:** `sera` (`NpcCatalog.cs`; GDD cap. 06, NPC-08)
- **categoria:** Npc, base criança (`PIPELINE.md` §3.1)
- **onde aparece no slice:**
  - B07: rotina de infância — `praca_centro` de manhã (aula com Eira) e à tarde (`atividade.ajudar_na_feira`), `casa_sera` à noite (`atividade.estudar_sozinha`) (`NpcCatalog.cs`).
  - B08: a q04 começa nela — âncora `casa_sera`, objetivo `ouvir_o_pedido`; `decidir` na praça, com Nilo (`q04_uma_promessa.json`).
  - B09: enquanto a q07 está em andamento, os nós de desfecho dela ganham a opção "Você sabe do Nilo?" (`DialogueCatalog.cs`, `nilo_sumiu_*`).
  - B14: aos 8, ervanaria de manhã (`atividade.aprender_com_lysa`), feira à tarde, casa à noite. **Implementado como [PROPOSTA]** (`NpcCatalog.cs`, `B14 [PROPOSTA]`); a fala de Lysa já confirma: "A Sera agora aprende comigo" (`dialogo.lysa.aos_oito`).
- **cânone de partida:**
  - "amiga/rival inteligente e competitiva; relações ramificadas" (dossiê §G); "amiga/rival com consequências de relacionamento" (GDD cap. 06). `traco.inteligente`, `traco.competitiva`; vínculos com `nilo` (amiga de infância) e `eira` (aluna); sabe `topico.vila_auren` e `topico.historia_de_eldoria`. É a única criança que sabe história; além dela, só a Eira (`NpcCatalog.cs`).
  - A história que ela sabe tem buracos que ninguém fecha. Eldoria "é mais velho que qualquer avó daqui. Tem ruína por aí que ninguém sabe quem levantou. Nem eu, ainda" (`dialogo.eira.sobre_historia`). A Primeira Fratura é o mistério da campanha, e quase nenhuma alma retém lembrança do Limiar (dossiê §I; GDD cap. 08).
  - Q-04: promessa cumprida leva a confiança dela e a de Nilo a +20; quebrada, a −20 (ADR-0007 §4). Testemunha os dois desfechos, o sumiço de Nilo e o salto (`NpcMemory.Testemunhos`). O ADR-0005 recusou fazê-la sumir: "lê como remoção de obstáculo, não como perda".
  - Falas já escritas, grafo `sera_promessa` (`DialogueCatalog.cs`, `strings.pt-BR.json`):
    - `o_pedido`: conta o segredo de Nilo e pede a promessa.
    - `na_praca`: "…leio melhor que o Nilo… Até o mural!"
    - `promessa_cumprida`: "Eu fiquei de olho pra ver se você ia contar…"
    - `promessa_quebrada`: "Eu lembro de tudo… Pra você eu não conto mais nada."
    - `nilo_sumiu_cumprida`: "…E eu odeio não saber."
    - `nilo_sumiu_quebrada`: "…Então não adiantou nada, né?"
    - `aos_oito` e `aos_oito_cumprida`, sem citação nesta ficha.
    - `aos_oito_quebrada`: "…as plantas, pelo menos, fazem o que dizem."
    - Esta ficha não muda nenhuma delas.
  - Em Auren, adulto que guarda o que foi dito escreve: Maelis anota as queixas e mantém o registro do desaparecimento aberto "no meu livro"; Oren diz "Favor eu não esqueço: eu anoto"; Eira lê os avisos do mural à noite (`dialogo.maelis.no_mural`, `dialogo.maelis.aos_oito_registro`, `dialogo.oren.cesto_achado`, `dialogo.eira.noite`).
  - Hoje: protótipo a 1,10 m, escalado inteiro para 1,28 aos 8 (`Prototipos.cs`, `NpcActor.AjustarCorpo`).
  - Acervo antigo (`ACERVO.csv`, COE-NPC-011 a 014): túnica ferrugem sem manga, faixa verde, calção balão, bota de cano dobrado, cabelo meio-preso, expressões "desafiadora" e "sorriso confiante"; aos 8, bolsa escolar com folha bordada. Não é cânone; é o default de §4.
- **autor da ficha / data:** Concept Art Lead (agente), 2026-10-03
- **estado:** aprovado G1 por delegação (ADR-0010, 2026-10-03). C2, C3, C4, C8 e §7 foram revistos no mesmo dia, pela condição do §5 e pela Arbitragem 2 do [`ELENCO.md`](ELENCO.md)

## 2. Critérios C1–C10

**C1 — Gancho.** *(segredo contado: cânone, `dialogo.sera.o_pedido`; tabuinha: PROPOSTA)* A única criança de Auren que sabe a história de Eldoria anota numa tabuinha de cera toda promessa que ouve, para que nada vire ruína sem dono, e cobra cada uma. A primeira palavra que ela quebra é a dela, quando conta a você o segredo do Nilo.

**C2 — Necessidade do mundo.**

*Cânone:*
- Sera tem o que nenhuma outra criança tem: a história de Eldoria (§1).
- Essa história se rompeu. Há ruínas sem autor e uma Primeira Fratura que ninguém explica, num mundo em que quase toda alma esquece o Limiar por onde passou (§1).
- Em Auren, os adultos que guardam o que foi dito escrevem: Maelis no livro, Oren nas anotações (§1).

*PROPOSTA (o resto):*
- Numa terra que perdeu a própria história e esquece até o próprio nascimento, o que não se escreve se perde.
- A única criança que aprendeu isso inteiro foi a que aprendeu a história. Sera escreve tudo para que nada do que ela ouviu vire ruína sem dono, inclusive a palavra das crianças, que é a única de Auren que ninguém anota.
- A rivalidade sai daí: quem sabe mais da história sabe mais que os outros, e ela faz questão de saber.

A C2 não se apoia na aula de praça, que o `NpcCatalog.cs` diz ser provisória.

**C3 — Silhueta em 3 formas.** *(PROPOSTA; reescrita em 2026-10-03, condição do §5)*

Corpo de 1,10 m (empata com o jogador) e h = 1,19 m com o coque. Referências: peito de 0,21 m e linha dos braços a ~0,86 m. Medidas a 30% pela régua do `client/tools/silhueta.py` (120 px/m).

Linguagem do par: Sera é feita de volumes empilhados no eixo (pino, ovo, coluna); Nilo, de pontas e diagonais. Ela não é criança arrumada de adulta: sem joia, sem cintura marcada, sem salto, tudo amarrado para correr.

1. **Coque.** O cabelo enrolado num cilindro centrado no alto da cabeça, de 0,09 m de altura e Ø 0,07 m (11 × 8 px).
   - Preso com tira de couro, com fios escapando e o estilete da tabuinha atravessado (o estilete é detalhe).
   - Sai do contorno por cima.
   - Divide a zona centrada com a torre do Oren; a Arbitragem 1 aceita a vizinhança pela escala.
   - T-pose: sobrevive (rígido no `Head`).
   - Câmera: de cima, é um ponto no centro da cabeça; de frente, ela é a única criança com prolongamento vertical.
2. **Bolota.** Colete de linho cru acolchoado com lã batida, das axilas ao quadril (de 0,82 a 0,52 m do chão), em forma de ovo.
   - Mede 0,36 m na barriga, a ~0,60 m do chão (43 px), contra um peito de 0,21 m: sai 7,5 cm (9 px) para fora do tronco de cada lado e 6 cm (7 px) para fora da coluna, logo abaixo. Por isso as duas não se fundem num bloco, que era o defeito da caixa.
   - Nas axilas fecha para 0,27 m, com o ombro redondo e por dentro da ponta do ombro (não é o rolo da Mara).
   - Não é o bloco do Daren: lá o tronco é um retângulo de ombro reto; aqui é curva, mais larga no meio.
   - Fica exatamente onde o avatar afina: a trouxa dele é uma barra nas omoplatas (0,70–0,79 m), e a cintura dele é fina (ficha `avatar`, C3).
   - É o mesmo gesto do coque: o coque a deixa mais alta, o colete a deixa mais larga, porque ela se monta maior.
   - T-pose: sobrevive (rígido em `Spine`/`Chest`; o braço em T passa ~0,25 m acima do ponto mais largo).
   - Câmera: é o tronco inteiro, de frente, de costas e de 3/4.
   - É a forma nova e a menos testada. Se o G2 a confundir com o bloco do Daren, volta para o coordenador arbitrar: esta ficha não tem outra zona livre a oferecer (§7).
3. **Coluna.** Calça larga e reta, do quadril ao tornozelo, com as pernas encostadas. De frente viram um bloco só, de 0,24 m, sem vão.
   - É o contrário das pernas em parêntese do Oren, que abrem um vão oval, e dos canos do Tovin, que fecham o vão só no joelho.
   - Termina em sapato baixo, não em tamanco: o tamanco de sola alta é do avatar.
   - T-pose: sobrevive.
   - Câmera: é a metade de baixo dela na tela.

De frente, a silhueta fica: pino, ovo, coluna. No validador, V09 mede h = 1,19 (faixa 1,00–1,20; o coque não pode passar de 0,10 m) e V13 dá r_cabeca ≈ 0,28, no teto (para Npc, só avisa).

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **A tabuinha.**

O objeto:
- Duas folhas de madeira clara de 0,14 × 0,10 × 0,012 m, unidas por uma dobradiça de couro, com cera escura por dentro. Aberta, mede 0,28 × 0,10 m.
- Fica presa por uma presilha no peito do colete. Não pende do pescoço, porque quem leva o registro pendurado é a Maelis (§7).
- Estilete de osso de 0,13 m, com ponta romba de um lado e espátula do outro. Fica no coque em todos os estados: é detalhe, não sinal.
- Veio de presente da Eira, para quem lia melhor (a disputa da fala `na_praca`).

A regra: o jogo lê o desfecho da q04, que Sera testemunha (`NpcMemory.Testemunhos`), e o salto. Cada partida tem um desfecho só (`QuestCatalog.cs`, B08). Cada estado muda a forma ou a cor de uma área de 0,14 m ou mais, nunca só o estilete:
- **promessa ainda em aberto:** fechada, madeira clara lisa;
- **`evento.q04_promessa_cumprida`:** aberta, com a cera escura virada para fora. O retângulo dobra de largura e escurece: ela passa a mostrar o que escreve ("De você eu não duvido mais", `dialogo.sera.promessa_cumprida`);
- **`evento.q04_promessa_quebrada`:** fechada e amarrada em X por uma tira de couro escuro de 3 cm sobre a tampa clara ("Pra você eu não conto mais nada", `dialogo.sera.promessa_quebrada`). O contraste é de valor (escuro sobre claro), não só de cor, e por isso lê na faixa Baixa e não depende de cor (GDD cap. 09);
- **`marco_idade_8`:** folhas prensadas por dentro, com as pontas verdes para fora das bordas, nos dois estados (C9).

Na tela, a ~2 m, num celular 1080p em paisagem (FOV vertical de ~60°, HIPÓTESE), a tampa fechada ocupa ~66 × 48 px e cada braço do X ~14 px. Aberta, são ~132 × 48 px escuros. Na conversa e de passagem, o jogador vê se a promessa ainda está em aberto, se ela abriu a tabuinha para ele ou se a amarrou.

Nada é concedido: a confiança é o número do `ReputationSystem` (±20), e a tabuinha só a mostra.

Custo: três variantes de uma peça pequena (fechada, aberta, amarrada) e as folhas, tudo dentro dos 8 000 tris do Npc, mais o componente de "peça por evento" do `ELENCO.md`.

**C5 — Regra exclusiva.** *(PROPOSTA)* **De olho.** Base no cânone: "Eu fiquei de olho pra ver se você ia contar".
- Enquanto a q04 está em andamento e nenhum desfecho foi gravado, a cabeça de Sera acompanha o jogador sempre que ele está a até 10 m, em qualquer âncora e período.
- Com `promessa_cumprida`, ela para de vigiar.
- Com `promessa_quebrada`, vira o rosto para o lado oposto quando ele chega a menos de 3 m, e isso dura até o salto (aos 8 ela diz "Já não estou brava", `dialogo.sera.aos_oito_quebrada`). Na conversa, o rosto virado vira cabeça baixa sobre a tabuinha amarrada, sem sair do enquadramento da câmera.
- Nenhum outro NPC de Auren acompanha a criança com a cabeça (regra de elenco, §7).

O código já lê as condições (`Condicao.Missao`, `Condicao.Lembra`). A técnica é o look-at do Humanoid (`Animator.SetLookAtPosition` em `OnAnimatorIK`, recurso nativo do Unity), sem clip novo. Custo: um script pequeno nela e o IK Pass ligado na camada do controller dos NPCs, com peso 0 para os outros.

**C6 — Voz.** Compara e põe número ("melhor que", "mais que", "três a um"), cita quem disse ("A Eira disse") e fecha cobrando palavra ("Promete?", "Tá escrito"). Bate com as falas já escritas, e nenhuma delas muda.
1. "A Eira disse que eu leio melhor que o Nilo, e aposto que corro mais que você. Quer ver? Até o mural!" (`dialogo.sera.na_praca`)
2. "Escuta, e não espalha: o Nilo anda indo sozinho pra beira do bosque, e me fez jurar segredo. Agora você também sabe. Promete que não conta pra ninguém?" (`dialogo.sera.o_pedido`)
3. "Três a um pra mim. Tá escrito. O que tá escrito ninguém desdiz." *(PROPOSTA: fecho de conversa, junto do gesto de C7)*

**C7 — Contradição visível.**
- Cânone: na mesma fala em que exige a promessa, ela quebra a dela e conta o segredo que jurou ao Nilo (fala 2 de C6).
- *(PROPOSTA)* Diz "Eu lembro de tudo", mas não confia na memória. No fim de toda conversa com o jogador, abre a tabuinha, risca uma linha com o estilete e fecha.
- Se a promessa foi cumprida, a tabuinha já está aberta, e ela só risca. Se foi quebrada, está amarrada, e ela não risca mais nada sobre ele.
- Na tabuinha aberta há placares e promessas de todo mundo, e nenhum risco sobre o juramento ao Nilo (textura).
- Custo: um clip próprio de ~1,5 s (riscar), com a troca fechada → aberta → fechada feita por evento de animação.

**C8 — Relação com o avatar por destino e origem.** *(PROPOSTA; só fala, nunca item nem poder — dossiê §H; `SLICE` R8. Oportunidades e eventos conferem em `DestinyCatalog.cs`)*
- **serena** (`oportunidade.aulas_com_eira`, `oportunidade.tarde_livre`): colega de aula, rival de leitura, com placar na tabuinha. A tarde livre do jogador ela não perdoa: "Você tem a tarde inteira. Eu tenho a feira."
- **normal** (`aulas_com_eira`, `feira_de_auren`, `recado_da_vila`): rival de igual para igual, na aula, na feira à tarde (onde ela já está) e em cada recado, que ela transforma em corrida ("Até o mural!").
- **dificil** (Vida Árdua: sem `aulas_com_eira`; com `trabalho_cedo`): não há leitura para disputar. Ela escreve a promessa da q04 no lugar da criança, que não vai à aula: "A palavra é sua. A letra é minha." A promessa pesa mais porque está na mão dela.
- **ruptura** (`evento.anomalia_no_bosque`; também sem `aulas_com_eira`): a criança viveu uma coisa que não está em nenhuma história de Eldoria que a Sera conhece. Ela não diz "não sei o que é". Pede para ouvir de novo, devagar, e escreve tudo: "Isso não está em história nenhuma. Então vai estar na minha." É a única criança de quem ela é a cronista. Não tem `topico.limiar` e não dá nome ao que ouviu.
- **origem:** não muda nada; a tabuinha é a mesma para as três.

Custo: 4 variantes de fala.

**C9 — Mudança dos 5 para os 8.** *(PROPOSTA)*
- **Antes (5–7):** corpo de 1,10 m, 1,19 m com o coque. Fica na praça o dia inteiro (aula e feira). A tabuinha é placar e livro de promessas.
- **Depois (8):** corpo de 1,28 m, que empata de novo com o jogador (`BodyScale.Crianca8`), e 1,37 m com o coque, dentro da faixa de 8 anos (1,15–1,40, `PIPELINE.md` §3.1).
  - O **coque** fica: empate não conta.
  - A **bolota** é o mesmo colete, agora curto: termina nas costelas e, num tronco mais comprido, fica menos redonda.
  - A **coluna** é a mesma calça, que agora bate no meio da canela. Ela passou da roupa, o contrário do Nilo, que cresceu para dentro da dele.
  - A tabuinha virou prensa de folhas.
  - De manhã está na ervanaria, com Lysa (rotina já implementada), e o rosto virado de C5 acaba.
- **Duas batidas:** das promessas de gente para as folhas que curam (a mesma tabuinha: as plantas "fazem o que dizem", `aos_oito_quebrada`); da praça o dia inteiro para a ervanaria de manhã, com a roupa curta.
- **Pede malha nova? Não.** O mesmo FBX de 5 anos, com o preset de proporção de 8 (comprimento de osso e blend shape, `PIPELINE.md` §3) e dois blend shapes a mais: barra do colete e barra da calça. A única peça nova são as folhas da tabuinha (menos de 100 tris).
- Pendência de código: hoje o `NpcActor` escala o corpo inteiro (1,10 → 1,28).

**C10 — Momento de cartaz.** *(PROPOSTA)* Praça, manhã. Sera chega primeiro ao mural, bate a mão na madeira e se vira. Tira o estilete do coque, abre a tabuinha e risca sem olhar para ela, com os olhos no jogador, que ainda vem chegando: "Três a um. Tá escrito."

## 3. Amarração

- **Destino, Grau ou Trama:** a Trama não a toca diretamente. Ela é a criança que guarda por escrito a história de uma terra que se rompeu, e o que acontece à criança da Ruptura é a primeira coisa que não está em história nenhuma (C8). O destino muda o campo da rivalidade: leitura na Serena e na Normal, a letra da promessa na Árdua, a crônica na Ruptura. Grau: fora do slice (dossiê §E).
- **Decisão de jogo:** a q04 inteira nasce dela, porque sem o pedido não há promessa a cumprir nem a quebrar. A decisão continua à vista depois, na tabuinha aberta ou amarrada (C4) e no olhar que para ou se desvia (C5).
- **Pilar:** escolher e transformar.

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| menina gênio de óculos e livro debaixo do braço | menina de tabuinha de cera e estilete no cabelo, o que Auren teria |
| rival de braços cruzados e cara fechada o tempo todo | rival que aposta corrida e anota o placar; o rosto virado é só para quem quebrou a palavra (C5) |
| cabelo longo solto, laço, vestido rodado | coque preso com tira e estilete, colete acolchoado em ovo, calça larga e reta |
| túnica ferrugem sem manga, faixa verde, calção balão (acervo antigo, `ACERVO.csv` COE-NPC-011) | coluna verde-escura, variação do #648B67 (contrasta com a terracota da praça, onde ela passa o dia, e combina com a ervanaria aos 8); colete de linho cru em marfim #E9DEC6 (liberado em roupa de NPC pelo ADR-0010 §5; o acolchoado é extensão pequena dessa decisão, a registrar); tabuinha de madeira clara com cera escura |
| sabe tudo e explica o mundo | sabe a história que a Eira ensinou e o que ela mesma anotou (`NpcCatalog.cs`); o que não está escrito, ela escreve |
| livro de registro ou prancha pendurada no pescoço (é da Maelis, ficha `maelis`) | tabuinha de cera de criança, presa no colete; a cera se apaga, e ela não apaga |
| retângulo liso que lê como aparelho moderno | madeira grossa, dobradiça de couro, cantos gastos, cera à mostra: objeto de escola de Auren |

## 5. Avaliação — parecer do Art Director: 18/20, aprovado (ADR-0010)

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total /20 |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 1 | 1 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | **18** |

- **Corte:** ≥ 14/20 e nenhum zero em C2, C3, C4, C5 — **atingido** (HIPÓTESE do ADR, a recalibrar)
- **Pontuador / data:** Art Director (agente) / 2026-10-03. Não escreveu a ficha.
- **Veredito:** **aprovado por delegação (ADR-0010), com uma condição antes de encomendar o concept:** C3 ganha uma terceira forma de verdade (ver C3). Subir C2 é recomendado, não é condição.

**Nota por critério (1 linha cada):**
- **C1 = 2.** Detalhe concreto (coque, tabuinha, cobrar cada promessa) e o conflito canônico (quebra a palavra dada a Nilo para pedir a sua). Duas frases.
- **C2 = 1.** O cânone citado confere (Maelis anota, Oren anota, Eira lê o mural), mas o costume de anotar não é condição de Eryndor nem de Auren: não toca bosque, Limiar, Trama, Fratura, escassez nem ofício, e "aula de praça aberta" é provisório do código (`NpcCatalog.cs`: até haver âncora de escola). Trocando os nomes, serve a qualquer vila com mural. O cânone dá a ela algo que nenhuma outra criança tem, `topico.historia_de_eldoria`, e a C2 não usa: numa terra em que a história se rompeu (Primeira Fratura) e há ruína "que ninguém sabe quem levantou" (fala da Eira), o que não se escreve se perde. É um caminho para 2; a escolha é de quem escreve.
- **C3 = 1.** Coque (8 × 11 px) e coluna (pernas sem vão, zona livre no elenco) são formas. A caixa não é: pelas medidas da própria ficha, o colete tem 0,30 m sobre uma coluna de 0,26 m, um degrau de 0,02 m por lado (2,4 px na folha), e caixa e coluna leem como um bloco só, do sovaco ao tornozelo. Tronco reto sem cintura já é o tronco padrão de uma criança de 5 anos, e é também a forma 2 do Daren (colete duro do ombro ao quadril, sem cintura), em outra escala. O substituto oferecido, "estatura", é o coque contado de novo (o que a separa do avatar é o cabelo). Para subir a 2: uma terceira forma que saia do contorno, fora das zonas do `ELENCO.md`.
- **C4 = 2.** Objeto com medida e regra escrita sobre os dois desfechos da q04 e o salto, que já existem e que ela testemunha. Defeito de leitura: "sem desfecho" e "quebrada" são as duas fechadas, e o que as separa é um estilete de 0,13 m (G2, item 3).
- **C5 = 2.** "De olho" tem base canônica (`promessa_cumprida`), limiares escritos (10 m, 3 m, até o salto) e técnica nativa (IK de olhar do Humanoid). É exclusivo: nenhuma outra ficha de Auren põe NPC acompanhando a criança com a cabeça (Aethron usa a técnica para a lente, fora de Auren). Ressalva: virar o rosto a menos de 3 m não pode quebrar o enquadramento da conversa.
- **C6 = 2.** Duas falas canônicas e uma nova, as três com o tique (número, "a Eira disse", cobrar palavra).
- **C7 = 2.** A contradição é canônica (exige a promessa quebrando a dela) e o gesto de riscar no fim de cada conversa a torna recorrente e visível.
- **C8 = 2.** Quatro destinos com oportunidades e item que conferem; na Árdua, "A palavra é sua. A letra é minha." ramifica a q04 sem mudar número.
- **C9 = 2.** Duas batidas (promessas → folhas; praça → ervanaria, com a roupa curta); "passou da roupa", o contrário do Nilo, é uma regra de par clara e barata (blend shape).
- **C10 = 2.** Cena curta e encenável, com o gesto que a define.

**Fatos dados como cânone que não conferiram:**
1. §1, "`nilo_sumiu_*` ("E eu odeio não saber.")", e C8 Ruptura, "já é fala dela": a frase só existe em `nilo_sumiu_cumprida`; no ramo da promessa quebrada ela nunca a disse.
2. §1, "`aos_oito*` (as plantas 'fazem o que dizem')": só em `aos_oito_quebrada`.
3. §4, marfim como "PROPOSTA de extensão… decide o style lock": já decidido no ADR-0010 §5, para linho cru em roupa de NPC. Colete de feltro duro em marfim é uma extensão pequena dessa decisão, a registrar.
4. C3 e §7, "o contrário das perneiras de Tovin, que deixam o vão aberto": os canos do Tovin fecham o vão no joelho (ficha `tovin`, C3); quem abre o vão é o Oren.
5. C2, "a aula acontece na praça, à vista da vila inteira": é provisório do código (`NpcCatalog.cs`), e a ficha cita a ressalva, mas a C2 se apoia nela, assim como a ficha `eira` ("manter Auren sem âncora de escola"). Uma âncora de escola derruba as duas.

Conferiram: id, traços, vínculos, tópicos, rotina e o B14 [PROPOSTA] (`NpcCatalog.cs`); q04 (âncora, objetivos, desfechos, um só por partida); testemunhos (`NpcMemory.cs`); confiança ±20 (ADR-0007 §4; `ReputationSystem.Consequencias`); falas `o_pedido`, `na_praca`, `promessa_*`, `aos_oito_quebrada` e a de Lysa (`strings.pt-BR.json`); opção "Você sabe do Nilo?" com a q07 em andamento (`DialogueCatalog.cs`); ADR-0005 sobre Sera; `marco_idade_8`; `BodyScale.Crianca8` = 1,28; `Condicao.Missao` e `Condicao.Lembra`; faixas, V09 e V13 (`PIPELINE.md` §3, §3.1, §11); acervo COE-NPC-011 e 012; oportunidades por destino (`DestinyCatalog.cs`).

**Régua de colisão (`ELENCO.md`):** concordo com a arbitragem 4 (a tabuinha não conta como forma) e aceito o coque na zona do Oren pela escala, como a arbitragem 1 aceitou o Y do Nilo. Discordo de uma omissão: "tronco em caixa" (Daren × Sera) não entrou nas zonas disputadas. Não precisa de árbitro, porque a caixa da Sera não se sustenta nem sozinha (C3).

**Para o G2 — o que o concept precisa provar:**
1. A terceira forma nova lê a 30% numa folha com Nilo, o avatar, Daren e Oren.
2. O coque lê como nó de criança preso para correr, não como coque de adulta, e não se confunde com a torre do Oren.
3. Os estados da tabuinha na tela do celular, a ~2 m: aberta × fechada lê; "travada" precisa de um sinal maior que o estilete, ou o jogador não distingue "ainda não decidiu" de "me trancou".
4. A coluna aguenta o clip Run sem as pernas se atravessarem.
5. O look-at no controller compartilhado (IK Pass, peso 0 nos outros) e o rosto virado sem quebrar a câmera da conversa.
- **Condições cumpridas em 2026-10-03:**
  - **Condição (C3, terceira forma):**
    - A caixa saiu. No lugar entrou a **bolota**, um colete acolchoado em forma de ovo, com 0,36 m na barriga.
    - Ela fica 9 px para fora do tronco e 7 px para fora da coluna, então não forma um bloco só com ela.
    - É curva e não retângulo, então não é o bloco do Daren. E fica onde o avatar afina.
    - A estatura deixou de contar como forma (era o coque contado de novo).
  - **C2 (recomendação):** reescrita sobre o que só ela tem, `topico.historia_de_eldoria`. Numa terra de ruína sem autor, de Fratura e de Limiar esquecido, o que não se escreve se perde. A C2 não se apoia mais na aula de praça, que é provisória (fato 5).
  - **C4 (G2, item 3):**
    - "Promessa em aberto" (tampa clara lisa) e "quebrada" (tampa amarrada em X por uma tira de couro escuro de 3 cm) se distinguem por área e por valor: a 2 m, cada braço do X tem ~14 px.
    - O estilete fica no coque em todos os estados e deixou de ser sinal.
  - **Fatos:**
    1. "E eu odeio não saber" é só de `nilo_sumiu_cumprida`.
    2. "Fazem o que dizem" é só de `aos_oito_quebrada`.
    3. O marfim agora cita o ADR-0010 §5 (linho cru liberado; o acolchoado é extensão pequena, a registrar).
    4. Quanto ao vão entre as pernas: os canos do Tovin o fecham no joelho, e quem abre o vão oval é o Oren.
    5. A C2 não depende da âncora de escola.
  - **Arbitragem 2, item 8:** saiu do C8 da Ruptura a reação ao amuleto. Agora a Sera escreve o que aconteceu à criança, sem dizer "não sei o que é".
  - **C5 (ressalva):** na conversa, o rosto virado vira cabeça baixa sobre a tabuinha, sem sair do enquadramento.
  - **Zonas:** atualizadas (§7). As formas novas do avatar (trouxa nas costas, sino até o joelho, tamancos) ficam fora da Sera; a coluna termina em sapato baixo.

## 6. Encaminhamento

**Para o G2, só depois de aprovado:**
- **Vistas:** frente, perfil, costas e 3/4 em T-pose, com fundo neutro, linha de chão e 1,10 m marcado (h = 1,19 com o coque). A mesma prancha no preset de 8 anos (1,28 m / 1,37 m).
- **Silhueta:** coque, bolota e coluna em preto a 30% (`silhueta.py`, 120 px/m), embaralhados com Nilo, o avatar, Daren e Oren. Também a folha das crianças nas duas idades.
- **O concept tem de provar:**
  1. a bolota lê como ovo, separada da coluna por pelo menos 6 px, e não se confunde com o bloco do Daren nem com a trouxa do avatar;
  2. o coque lê como nó de criança preso para correr, não como coque de adulta, e não se confunde com a torre do Oren;
  3. os três estados da tabuinha se distinguem a ~2 m na tela de celular em paisagem, inclusive amarrada × fechada;
  4. a coluna aguenta o clip Run sem as pernas se atravessarem;
  5. o look-at funciona no controller compartilhado (IK Pass, peso 0 nos outros), e o rosto virado não quebra a câmera da conversa.
- **Paleta:** verde-escuro na coluna, linho cru em marfim no colete, madeira clara e cera escura na tabuinha. Reservados e proibidos: turquesa #86C8C9, dourado #D6B36A, violeta #9777B8.
- **Objeto à parte:** tabuinha de 0,14 × 0,10 × 0,012 m (0,28 × 0,10 aberta), estilete de 0,13 m, os três estados e as folhas.
- **Orçamento** (`PIPELINE.md` §4, HIPÓTESE v0): Npc com 8 000 tris, 2 materiais, textura 1024 e 55 ossos sem osso secundário, portanto coque, colete e tabuinha rígidos. As variantes entram como Renderers no FBX, e todas contam no V21.
- **Validador:** V09 mede h = 1,19 (o coque não passa de 0,10 m); V13 fica no teto. O preset de 8 anos não tem regra hoje; esta ficha pede preset, não modelo novo.
- **Testes a registrar:** silhueta (≥ 4/5), descrição e troco com o Nilo na q04. Com a Sera pedindo segredo sobre ela mesma, a cena tem de quebrar.
- **Custo total:**
  - 1 clip próprio (riscar);
  - o look-at;
  - 5 falas novas (1 de C6, 4 de C8);
  - as variantes da tabuinha e as folhas;
  - 2 blend shapes;
  - o componente de peça por evento;
  - o preset de proporção de 8 no `NpcActor`.
- **Dependência:** nenhuma de conteúdo de missão. O look-at pede o IK Pass no controller compartilhado.

**Para o G3:** bloco `### sera` em `PROVENIENCIA.md` com `g1:` (data, nota, pontuador), `g2:` (data, silhueta n/5, descrição, troco), `entrada:` (concept e SHA-256), `ferramenta`, `plano`, `licenca`, `termos_url` e `g3:`. `referencias_terceiros`: nenhuma usada nesta ficha.

## 7. Ligações com outras fichas

PROPOSTAS desta ficha que envolvem outro personagem:
- **Eira:**
  - A tabuinha foi presente dela para quem lia melhor (a disputa já está na fala `na_praca`). A C2 se apoia na fala dela sobre as ruínas (`sobre_historia`) e no tópico que só as duas têm.
  - A ficha da Eira tem uma lousa em que as crianças escrevem o que ela promete responder. É a mesma ideia de promessa escrita, vista pelos dois lados: Sera aprendeu o costume na lousa e o levou para a tabuinha.
- **Lysa:** aos 8, foi com a Lysa que a tabuinha virou prensa de folhas (a fala dela já diz que Sera aprende com ela). A ficha da Lysa não dá à Sera chapéu nem feixes, e esta ficha não usa nenhum dos dois.
- **Maelis:**
  - Leva o registro da vila numa prancha pendurada no pescoço e escreve em cena (clip `anotar`). Sera é a versão criança: a tinta dela não se apaga; a cera da Sera se apaga.
  - O lugar também muda: prancha na cintura × tabuinha presa no colete; 0,62 m × 0,14 m.
  - Os dois gestos de escrever existem. Se o elenco tiver de ficar com um só, o coordenador decide.
- **Oren:** diz "eu anoto" na fala, mas não escreve em cena. A torre dele e o coque da Sera dividem a zona centrada acima da cabeça, e a Arbitragem 1 aceitou a vizinhança pela escala.
- **Daren:**
  - O tronco dele é um bloco retangular de ombro reto (ficha dele). A bolota da Sera é curva e mais larga no meio, numa criança.
  - Se o G2 confundir as duas, a bolota volta para arbitragem. Esta ficha não acha outra zona livre para criança: o avatar ocupa as costas, o sino e os pés; a Maelis, os lados da cabeça; a Mara, os ombros; e Borin, Mara e Lysa, os braços.
- **Mara e as portas:** a ficha da Mara fecha à noite a porta de `casa_sera`. Combina com a rotina canônica "Estudando sozinha": ela estuda de porta fechada. Esta ficha não pede nada a mais.
- **Nilo:** a tabuinha aberta não tem risco sobre o juramento a ele. A tabuinha, a forquilha dele, a prova no aro do Borin, o cestinho do Oren e a manta do avatar usam o mesmo componente de "peça por evento do histórico".
- **Elenco inteiro e Aethron:** acompanhar a criança com a cabeça é exclusivo da Sera (C5); nenhuma outra ficha de Auren deve propor isso. A ficha do Aethron também usa look-at, mas olhando para a lente da câmera e fora de Auren. A técnica é a mesma com outro alvo, e o código pode ser um só.
- **Avatar:**
  - Na Vida Árdua, a Sera escreve a promessa por ele (C8).
  - Esta ficha não usa nenhuma forma do avatar: a trouxa de manta horizontal nas costas, na altura das omoplatas, com as pontas fora dos braços; a manta em sino até a dobra do joelho; os tamancos de sola alta.
  - A ficha do avatar já reserva para a Sera a frente do tronco. É ali que fica a bolota, na altura em que o avatar tem a cintura fina.
  - Em troca, a aparência do avatar, quando voltar ao jogo (ADR-0007 §5), não deve usar coque alto centrado, colete acolchoado em ovo nem calça larga de pernas encostadas.

Zonas de silhueta que esta ficha ocupa:
- **Acima da cabeça, centrado e pequeno:** o coque de 0,09 m. Divide a zona com a torre do Oren, aceito pela escala (Arbitragem 1).
- **Tronco em ovo, mais largo na barriga** (0,60 m do chão aos 5): fica onde o avatar afina, entre a trouxa e o sino dele. É vizinho do bloco do Daren (retângulo, adulto).
- **Pernas num bloco único, sem vão, até o tornozelo,** com sapato baixo.
- **Peça pequena no peito (tabuinha):** não conta como forma.
- **Não usa:**
  - trouxa nas costas, sino até o joelho nem tamancos (avatar);
  - rolo nos ombros (Mara);
  - cabelo em volume redondo (Maelis);
  - caixa reta (Daren);
  - nada pendurado fora do quadril ou da coxa (Borin).
