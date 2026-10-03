# Ficha G1 — `simbolo_limiar`

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md)). Par desta ficha: [`aethron`](aethron.md).
> Fato com fonte é cânone. **PROPOSTA** = ideia desta ficha, não está nos docs e não vale até ser aprovada.
> Objeto: C6, C7 e C8 foram escritos pensando em personagem. Onde não se aplicam, o critério diz por quê e o pontuador decide (o ADR não define N/A).

## 1. Identificação

- **id:** `simbolo_limiar` (`Prototipos.cs`; `PIPELINE.md` §13). Na cena de Auren é o objeto `SimboloDoLimiar` (`AurenSceneBuilder.NomeSimbolo`), que carrega o `SaltoGatilho`; no palco do Limiar, o objeto `Simbolo` (`EntradaSceneSetup.NomeSimbolo`). Rótulo na tela: "Símbolo do Limiar" (`salto.simbolo`).
- **categoria:** Simbolo
- **onde aparece no slice:** ao fundo da tela de título (`EntryFlow.Titulo`); no B01, em plano fechado durante as seis falas (`SLICE` B01; `EntradaSceneSetup.PalcoDoLimiar`); no B11, na clareira, missão `q08_ecos_do_limiar`, objetivos `achar_o_simbolo` → `tocar_o_simbolo` na âncora `bosque_clareira` (`MissaoMundo.cs`); no B12–B13, como o lugar onde o salto é oferecido (`SaltoGatilho.cs`, `SaltoHud.cs`). É citado no B14, por Nilo ("um círculo de luz", `dialogo.nilo.aos_oito_*`), e no B16 (`gancho.texto`: "o círculo que não se fecha").
- **cânone de partida:**
  - Um símbolo visto no Limiar reaparece em Auren (dossiê §I). Ordem do slice: desaparecimento perto do bosque → símbolo do Limiar → salto temporal (dossiê §L).
  - Signos da Trama: fios entrelaçados, círculos incompletos, pontos conectados; "não são logo registrado" (dossiê §J; GDD cap. 09). Turquesa #86C8C9 é da Trama e dos sinais mágicos; Dourado #D6B36A, do Limiar e da ascensão; Violeta #9777B8, das anomalias (GDD cap. 09).
  - Aethron o descreve como fios entrelaçados num círculo que não se fecha, que "não deveria estar aqui" (`limiar.fala.4`).
  - B11: em tela, "visualmente o mesmo de B01"; tocar grava `marco.eco_do_limiar` e levanta `salto_temporal_liberado`; concluir a missão não salta; o símbolo não some; Aethron não reaparece (`SLICE` B11). As flags `eco_do_limiar_tocado` e `salto_temporal_liberado` são o evento `evento.q08_concluida` (`QuestCatalog.Flags`).
  - Em Auren o salto é oferecido só no símbolo, não em qualquer ponto; o `SaltoGatilho` nasce desligado e o `SaltoHud` o liga só com o salto liberado (Q-08 concluída e ainda 5 anos) (`SaltoHud.cs`; `AurenSceneBuilder.SimboloDoLimiar`). Tocar é descoberta, saltar é decisão (`SLICE` §4.1). Depois do salto, voltar à clareira não oferece saltar de novo (`SLICE` R5).
  - Posição: 2 m ao norte da âncora `bosque_clareira` (0, 0, 70), fora do gatilho da Q-08, que fica na âncora. O jogador chega pelo sul, vindo de `entrada_bosque` (0, 0, 60). A clareira é fechada dos lados e no fundo, e o vão de 6 m da entrada não tem tranca: a criança pode chegar lá antes da Q-08 (`AurenSceneBuilder.Bosque`).
  - Altura-alvo de 1,5 m e o mesmo objeto nos dois lugares (`Prototipos.cs`; `SLICE` B01: "o mesmo protótipo da clareira"). Câmera do palco: FOV 45° a 3,4 m; o símbolo ocupa de 42% a 95% da altura da tela, acima do painel de fala, e o `EntradaSceneTests` exige que fique no centro (`EntradaSceneSetup.cs`).
  - Hoje: o greybox do palco é um disco **dourado** de 1,2 m (`EntradaSceneSetup.cs`, material `COE_Limiar_Simbolo`). O protótipo do Tripo é "um círculo incompleto genérico, sem validação de cânone" (`PROVENIENCIA.md` §6); a textura dele (`Art/Prototipo/Pecas/simbolo_limiar/simbolo_limiar_basecolor.JPEG`, conferida para esta ficha) é pedra cinza com musgo, arcos e glifos amarelos luminosos. No acervo, o emblema da Trama #21 (azul-marinho chapado) e #29 (metal dourado) (`ACERVO.csv`). Os três são referência do default a evitar (§4), não cânone.
  - Vida da Ruptura: "um sonho que se repete e algo fora de lugar no bosque" (`destino.ruptura.descricao`). O `evento.anomalia_no_bosque` existe no `DestinyCatalog.cs` e nada o lê.
- **autor da ficha / data:** Concept Art Lead (agente), 2026-10-03
- **estado:** aprovado G1 por delegação (ADR-0010, 2026-10-03)

## 2. Critérios C1–C10

**C1 — Gancho.** Um anel de três fios de luz turquesa, trançados, que não fecha por um vão de 40 cm e deixa um fio solto cair até o chão: no Limiar, o guardião diz que ele não deveria estar ali; numa clareira de Auren, é nele que a criança, se quiser, deixa a infância para trás. *(o desenho é PROPOSTA; o resto é cânone: `limiar.fala.4`, `SLICE` §4.1)*

**C2 — Necessidade do mundo.** Cânone: a Trama liga possibilidades e consequências (GDD cap. 08) e a memória do Limiar se perde (dossiê §I). O slice precisa que algo visto antes de nascer seja reconhecido em Auren (dossiê §I, §L) e que o salto de três anos tenha um lugar no mundo, não um botão: sem o símbolo, o `SaltoHud` mostra um botão no alto da tela, como na área de treino (`SaltoHud.cs`). É a única peça que atravessa do Limiar para a vida: a criança não lembra de Aethron, mas reconhece a forma. **PROPOSTA:** a incompletude é o mistério. Um círculo de fios que não fecha e um fio que se soltou insinuam que algo na Trama se rompeu, como o gancho diz ("algo, há muito tempo, se rompeu", `gancho.texto`), sem nomear a Primeira Fratura, que o slice só pode insinuar (`SLICE` B11, B16).

**C3 — Silhueta em 3 formas.** *(PROPOSTA)* Vista de frente: a face do anel virada para o sul, para quem chega de `entrada_bosque`, e para a câmera do palco. A T-pose não se aplica a objeto.
1. **Anel trançado:** três fios trançados formando um anel de pé, com 1,20 m de diâmetro externo, trança de 0,10 m e centro vazio (vê-se a clareira através dele). Na folha do teste (120 px/m): 144 px de diâmetro, trança de 12 px.
2. **Vão:** abertura de 40° na posição de uma hora (no alto, à direita de quem olha), com ~0,40 m de corda. Em cada borda, a trança termina num nó do tamanho dos outros (Ø 0,06 m) até o eco (C4), de onde os três fios se abrem e apontam para o outro lado sem chegar. São 49 px na folha. É a forma que dá nome ao símbolo e a primeira que precisa ser lida.
3. **Fio solto:** um dos três fios sai da trança na posição de sete horas e desce ~0,45 m, com duas ondas, até o chão; tem 0,05 m de espessura (6 px na folha). É a única parte fora do contorno do anel e o único contato com o mundo: na clareira, toca a grama; no palco do B01, aparece quase inteiro acima do painel, e só os últimos ~10 cm entram atrás dele; no título, acaba no escuro. Com ele, a altura fecha em 1,50 m, a do `Prototipos.cs`.

Pontos conectados: um nó de luz em cada cruzamento da trança, a cada ~0,28 m (11 ao longo do anel; número sem significado declarado, e nunca 4 ou simetria de 4). É detalhe, não forma.

**Câmeras do jogo.** No palco do B01, a 3,4 m, num celular 1080p em paisagem: anel de ~460 px de altura, vão de ~157 px, nós da borda de ~23 px antes do eco e de ~46 px depois. Na clareira, a 2 m da âncora, o centro do anel fica a 0,90 m, na altura dos olhos da criança de 1,10 m. A câmera de terceira pessoa fica 2,75 m atrás dela (`ThirdPersonCamera`, 2,5 × a altura) e vê o anel a ~4,75 m, de frente. Fraqueza declarada: visto de lado (leste ou oeste), o anel vira uma barra de 1,20 × 0,10 m e o vão some. A clareira não protege: tem 42 × 16 m (z de 62 a 78, `MeiaLarguraClareira = 21`, `AurenSceneBuilder.cs`), e o jogador pode dar a volta no anel. A defesa é o caminho e o toque: quem chega pelo sul o vê de frente, e o objetivo da Q-08 fica na âncora, 2 m ao sul, de frente para ele. A 45°, a corda do vão ainda projeta ~0,30 m; a 90°, some. Fica aceito, e o G2 confere (se a 45° não ler, a trança engrossa ou o anel gira um pouco para o caminho).

Diferenças: do par (Aethron), curva contra reta, assimetria contra simetria, turquesa contra dourado. Do aro do Borin: o dele é **fechado**, de ferro, preso ao quadril (ficha `borin`, §4); este é aberto, de luz, de pé no chão.

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* O objeto é o próprio símbolo: anel de 1,20 m, vão de 0,40 m e os **dois nós da borda do vão**.
Regra: o jogo lê `LifeEventHistory.Ja(QuestCatalog.EventoDaFlag("eco_do_limiar_tocado"))`, que é `evento.q08_concluida`. Falso: os dois nós da borda são iguais a todos os outros, turquesa e com Ø 0,06 m. É o estado do B01 e da clareira antes do toque, e por isso o B11 continua "visualmente o mesmo". Verdadeiro: os dois nós ficam em Dourado #D6B36A, dobram de diâmetro (Ø 0,12 m) e ganham um miolo claro de Ø 0,05 m em marfim #E9DEC6, para sempre naquele save, na clareira e no símbolo da tela de título. Sem cor também se lê: turquesa e dourado têm brilho quase igual (~0,51 e ~0,48), então quem marca a troca em escala de cinza é a forma e o valor, não o matiz. O ponto pequeno e médio vira ponto duplo com centro claro (~0,74). O dourado é do Limiar (GDD cap. 09): é o eco. E é o único Dourado pintado em Auren; a estrada já foi tirada do laranja por colar no Dourado do Limiar (`PROJETO.md` §6). O vão nunca fecha: nenhuma ponte de luz cruza a abertura, nem dourada, porque aos 8 anos o gancho ainda diz "o círculo que não se fecha". Nada é concedido; o salto continua sendo a confirmação do B13.

**C5 — Regra exclusiva.** Cânone de código: é o único objeto do jogo que muda a idade. O salto só é oferecido nele (`SaltoHud`; `SLICE` §4.1), em dois atos: tocar (Q-08, descoberta) e, depois, "Seguir adiante" (aviso do B12, confirmação do B13). Só vale enquanto `AgeAdvance.PodeAvancarIdade` for verdadeiro; aplicado o salto, voltar não oferece de novo (`SLICE` R5). **PROPOSTA:** o estado visível acompanha a regra. Nós turquesa: não há o que fazer nele (antes da Q-08) ou o que fazer é tocá-lo (Q-08). Nós dourados aos 5 anos: o prompt "Símbolo do Limiar" está lá. Nós dourados aos 8: nada mais a fazer; o eco ficou.

**C6 — Voz.** N/A: objeto não fala. Para o pontuador: o que se diz dele já é coerente em três textos escritos, o de Aethron ("fios entrelaçados num círculo que não se fecha", `limiar.fala.4`), o de Nilo aos 8 ("um círculo de luz") e o do gancho ("o círculo que não se fecha"). O desenho desta ficha não contradiz nenhum: fios de luz, sem pedra, sem metal. Se ele tiver voz, é som (dossiê §J, identidade etérea da Trama), fora desta ficha.

**C7 — Contradição visível.** N/A em "ação recorrente": objeto não age. Para o pontuador decidir, uma contradição estática: chama-se "do Limiar" (`salto.simbolo`) e é da Trama. É a única coisa turquesa no Limiar e, depois do eco, a única dourada em Auren. Visível em toda aparição (título, B01, clareira).

**C8 — Relação com o avatar por destino e origem.** *(PROPOSTA; nenhuma vantagem, ADR-0004)* Só a Ruptura ramifica, porque é o único destino cujo texto já promete "algo fora de lugar no bosque" (`destino.ruptura.descricao`):
- **ruptura:** o símbolo está na clareira desde o primeiro dia, inerte (sem prompt, nós turquesa). A criança pode achá-lo antes de qualquer missão e não pode fazer nada com ele até a Q-08. É o `evento.anomalia_no_bosque` do `DestinyCatalog` ganhando forma.
- **serena, normal, dificil:** a clareira fica vazia até `evento.q07_concluida`, e o símbolo aparece quando a Q-08 começa sozinha (`MissaoMundo.Automaticas`). Hoje ele está lá desde o início para todos, inerte e sem explicação; esta ficha troca isso por "aparece quando a investigação leva ao bosque".
- As três iguais são de propósito: a Trama não distingue conforto ("Não é por você", `limiar.fala.5`). A origem não muda nada.

**C9 — Mudança dos 5 para os 8.** *(PROPOSTA)*
- **Antes (5–7):** o centro do anel fica na altura dos olhos da criança, e o vão (de 1,25 a 1,44 m do chão) fica acima da cabeça dela: para tocá-lo, ela ergue o braço. Nós da borda turquesa até o toque, dourados depois; com os nós dourados, o prompt do salto.
- **Depois (8):** o anel é o mesmo e a criança não: com 1,28 m (`PIPELINE.md` §3), ela vê o centro um palmo abaixo dos olhos e o vão na altura da cabeça. Nós dourados, sem prompt (salto aplicado). E o fio solto está mais comprido: mais 0,40 m deitados na grama, como se outro trecho da trança tivesse se desfeito em três anos. Insinua, não explica (`SLICE` B16); depende do dono da narrativa.
- **Variante de malha:** só do fio solto, uma segunda peça de ~150 tris trocada pelo marco do salto (`AgeAdvanceCatalog.SaltoInfancia`, `marco_idade_8`). O anel não muda: o B11 exige igualdade com o B01 antes do toque.

**C10 — Momento de cartaz.** *(PROPOSTA)* Clareira verde, câmera atrás de uma criança de 1,10 m. À frente, de pé, um anel de luz turquesa trançada, da altura de um adulto baixo, que não fecha no alto à direita; um fio solto desce até a grama. A criança chega, ergue o braço e traça o anel; a linha para no vão. No toque (`tocar_o_simbolo`), os dois nós da borda crescem e passam de turquesa a dourado, com miolo claro; aparece "Símbolo do Limiar". Cinco segundos, com peças que já existem (âncora, objetivo `tocar_o_simbolo`, `SaltoGatilho`); só a troca dos nós é nova.

## 3. Amarração

- **Destino, Grau ou Trama:** é o signo da Trama. A Ruptura o vê desde o primeiro dia (C8); os outros destinos, quando a investigação chega ao bosque. Grau não se aplica no slice (dossiê §E); a ficha não o liga às ascensões Arcana ou da Ruptura (GDD cap. 04), embora as duas toquem a Trama: isso é campanha.
- **Decisão de jogo que ele cria:** quando deixar a infância. É o único lugar onde o salto é oferecido, depois do aviso que nomeia o que se encerra (B12); dá para virar as costas, terminar as opcionais e voltar (R18). Sem ele, a decisão vira um botão de menu.
- **Pilar:** viver e crescer.

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| emblema de simetria de quatro eixos: nó entrelaçado no centro, anel partido em quatro arcos iguais e quatro pontos nos pontos cardeais (acervo #21 e #29) | um vão só, assimétrico, na posição de uma hora; centro vazio, sem nó; a trança é o próprio anel |
| pedra com musgo e glifos luminosos amarelos gravados (textura do protótipo atual) | sem pedra e sem gravação: fios de luz em cor chapada; só o fio solto toca o chão |
| círculo mágico de anime: anéis concêntricos com runas, estrela de muitas pontas, escrita, deitado no chão e girando | um anel só, de pé, sem escrita e sem polígono; não gira e não pulsa |
| aro com teia tecida por dentro e penas ou contas penduradas | fios só na circunferência, nada por dentro, um único fio pendente e sem enfeite |
| aro aberto de metal, de joalheria, com pontas ornamentadas | pontas que acabam em nós de luz simples; 1,5 m de altura, nada de metal |
| laço de corda com nó | luz, não fibra: sem textura de corda, sem nó corrediço; o anel não pende de nada |
| portal: redemoinho ou superfície brilhante dentro do anel | dentro do anel não há nada; vê-se o fundo através |
| dourado (o greybox atual e o #29) | Turquesa #86C8C9, a cor da Trama; Dourado só nos dois nós da borda e só depois do eco (C4); Violeta fora, para o slice não declarar o símbolo uma anomalia |
| formas que já são de alguém: círculo com vão no alto cortado por um traço vertical; vão em cunha na lateral, como boca; ferradura de pé sobre as duas pontas; círculo com cruz embaixo; círculo com cabo oblíquo; uma pincelada de tinta que não fecha; três arcos entrelaçados em triângulo | vão estreito na posição de uma hora, sem traço; fio ondulado saindo das sete horas, nunca reto, nunca a 45° e nunca do ponto mais baixo; três fios trançados, não uma pincelada |

## 5. Avaliação — parecer do Art Director: 15/16 (C6 e C7 N/A), aprovado (ADR-0010)

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 2 | 2 | 2 | N/A | N/A | 1 | 2 | 2 | **15/16** |

- **Corte:** para objeto, o ADR não define N/A, então a decisão é do pontuador. **C6 é N/A** porque objeto não fala. **C7 é N/A** porque a régua pede contradição em ação recorrente, e objeto não age. A contradição estática que a ficha oferece ("do Limiar" e da Trama; única turquesa no Limiar, único dourado em Auren) é um bom contraste de design, mas não é o que C7 mede. **C8 e C9 se aplicam** e foram pontuados: o símbolo tem relação com o avatar e muda no salto. O total é sobre 16 pontos, e o corte é a proporção do ADR (70%) arredondada para cima, **≥ 12/16**, sem zero em C2–C5. **Atingido** (HIPÓTESE do ADR, a recalibrar). O teste de silhueta do G2 também não foi escrito para objeto; vale a adaptação da §6, contra círculos-armadilha.
- **Pontuador / data:** Art Director (agente) / 2026-10-03. Não escreveu a ficha.
- **Veredito:** **aprovado G1 por delegação (ADR-0010).** O código que a ficha pede é do dono do código e da narrativa: a cor do greybox, a face voltada para o sul, os nós lidos da flag, a visibilidade por destino e o fio longo aos 8. Se o C8 for recusado (o símbolo continua lá desde o início para todos), a nota não muda. Se for recusada a troca de cor dos nós, C4 perde a regra e a ficha volta para nota.

**Nota por critério (1 linha cada):**
- **C1 = 2.** Tem detalhe concreto (três fios, vão de 40 cm, fio solto) e tensão (o que "não deveria estar ali" é onde se deixa a infância). O desenho é PROPOSTA; o resto confere.
- **C2 = 2.** O que é de mundo é específico de Eryndor: a memória do Limiar se perde (dossiê §I) e algo na Trama se rompeu (`gancho.texto`, com a Primeira Fratura só insinuada). "O slice precisa de um lugar para o salto" é necessidade de produto e não conta; o critério passa sem ela.
- **C3 = 2.** De frente, a 30%, as três formas leem: o anel com 144 px, o vão com 49 px de corda e o fio com 6 px, acima do limiar de limpeza do `silhueta.py` (< 3 px na folha). O fio é a única saída do contorno. T-pose não se aplica. A fraqueza declarada é real (de lado, o vão some), e a defesa escrita não segura (ver abaixo). Vai para o G2.
- **C4 = 2.** O objeto e a regra estão escritos sobre um evento que existe (`evento.q08_concluida`, via `QuestCatalog.EventoDaFlag`), e o B11 continua "visualmente o mesmo" antes do toque. Nada é concedido, e o vão nunca fecha. Ressalva: a troca é só de matiz, porque turquesa #86C8C9 e dourado #D6B36A têm luminância quase igual (~0,51 e ~0,48).
- **C5 = 2.** A exclusividade de código confere: é o único lugar de Auren em que o salto é oferecido (`SaltoHud`, `SaltoGatilho`), em dois atos, e só enquanto `PodeAvancarIdade` vale. A PROPOSTA só torna o estado visível.
- **C6, C7 — N/A** (ver o corte).
- **C8 = 1.** É concreta e tem a razão escrita no cânone ("Não é por você", `limiar.fala.5`), mas ramifica em dois (a Ruptura contra os outros três), e a coluna 2 pede os quatro. Não peço reescrita: quatro variantes de um objeto seriam enfeite, e o ponto que falta é da régua, não da ficha. Depende de código novo (visibilidade por destino e por `evento.q07_concluida`).
- **C9 = 2.** Tem duas batidas: a altura relativa (o vão acima da cabeça aos 5 e na altura dela aos 8), os nós sem prompt depois do salto e o fio mais comprido (PROPOSTA de narrativa). O fio longo é uma peça trocável de ~150 tris, lida de `marco_idade_8`.
- **C10 = 2.** A cena dura 5 s e usa peças que já existem; só a troca de cor é nova. A troca acontece no toque, não na chegada (ver abaixo).

**Fatos dados como cânone que não conferiram:**
1. O C3 diz que, "no palco do B01, [o fio solto] some atrás do painel de fala". O palco enquadra o símbolo de 0 a 1,5 m entre ~41% e ~94% da altura da tela (`EntradaSceneSetup.cs`, que a própria §1 cita), e o painel desenhado cobre ~43,6% de baixo. Só os últimos ~7–10 cm do fio ficam atrás dele. O fio aparece no B01, o que é bom para C3.
2. O C3 diz: "Defesa: a clareira é fechada dos lados e o caminho chega pelo sul, de frente". A clareira é fechada, mas a 21 m do símbolo: tem 42 × 16 m, de z 62 a 78 (`MeiaLarguraClareira = 21`, `AurenSceneBuilder.cs`). Nada impede o jogador de dar a volta no anel e vê-lo de lado.

**Contas e coerência interna:**
3. O parágrafo "Conflito com a ficha `avatar`" da §7 ficou velho. A ficha `avatar` atual já não propõe ajoelhar nem passar pelo vão, e o `ELENCO.md` decidiu que a criança traça de pé, sem atravessar. A recomendação deste parágrafo foi a que valeu.
4. O C10 diz: "A criança chega, e … os nós passam de turquesa a dourado". Pelo C4 desta ficha e pelo C5 do avatar, a troca acontece no toque (`tocar_o_simbolo`, o traço), não na chegada.

Conferiram: ids e nomes de objeto (`Prototipos.cs`, `AurenSceneBuilder.NomeSimbolo`, `EntradaSceneSetup.NomeSimbolo`, `salto.simbolo`); objetivos e âncora da q08 e início automático depois da q07 (`q08…json`, `MissaoMundo.Automaticas`); flags que levam a `evento.q08_concluida` (`QuestCatalog.Flags`); o `SaltoGatilho` desligado e ligado pelo `SaltoHud` com a Q-08 concluída e ainda aos 5 anos, `PodeAvancarIdade` e `marco_idade_8`; posição a 2 m da âncora (0, 0, 70) e entrada em (0, 0, 60), com vão de 6 m sem tranca; altura de 1,5 m; greybox dourado de 1,2 m (`COE_Limiar_Simbolo`); textura do protótipo (pedra com musgo e glifos amarelos, conferida na imagem); `PROVENIENCIA.md` §6; ACERVO #21 e #29; `gancho.texto`; fala 4 de Aethron e falas de Nilo aos 8; `destino.ruptura.descricao`; `evento.anomalia_no_bosque` sem leitor; `PROJETO.md` §6 (a estrada saiu do laranja); `SLICE` B01, B11, §4.1, R5 e R18; dossiê §I, §J e §L; GDD cap. 08 e 09. As contas também conferem: o vão de 40° às 13 h dá bordas a 1,25 m e 1,44 m, e no B01 a 1080p o anel tem ~460 px e o vão ~157 px.

**Para o G2 — o que o concept precisa provar:**
1. **De lado.** A silhueta adaptada da §6 e, além da vista de frente, o anel a 45° e a 90° na clareira, onde o jogador dá a volta nele. A 45°, o vão tem de continuar visível. Se não continuar, o anel ganha espessura de trança, ou um leve giro na direção do caminho do sul, antes de qualquer outra mudança.
2. **O eco sem cor.** A troca de turquesa para dourado em escala de cinza: os dois hex têm valor quase igual, e o eco precisa ler também sem cor (nó mais claro ou maior), na câmera de terceira pessoa a ~4,75 m, no celular.
3. **Palco do B01.** Com o painel por cima, o anel turquesa contra o Azul profundo e o fio solto visível acima do painel.
4. **Luz, não corda.** Cor chapada, sem contorno e sem pulso, lendo como luz, e não como corda nem plástico (§4).
5. **Trança no orçamento.** Os três fios trançados em 2 000 tris (tubo de 6 lados) leem a 4,75 m. Se não lerem, a trança vira um tubo único com textura antes de se mexer no vão ou no fio.
6. **Descrição.** O teste de descrição da §6, com quem conhece o gênero: se a resposta citar emblema, marca ou jogo, reprovou.

**Condições cumpridas em 2026-10-03:**
- Fio solto (fato 1): no B01 ele aparece quase inteiro acima do painel; só os últimos ~10 cm entram atrás dele.
- Defesa de lado (fato 2): a clareira tem 42 × 16 m e não protege. A defesa passou a ser o caminho que chega pelo sul e o objetivo da Q-08, de frente para o anel. Ficaram escritos a projeção do vão a 45° (~0,30 m) e o que muda se não ler.
- §7: o parágrafo do conflito com o avatar foi trocado pela decisão do `ELENCO.md` (traça de pé, sem atravessar).
- C10: a troca acontece no toque (`tocar_o_simbolo`), depois do traço, e não na chegada.
- Eco sem cor (ressalva de C4 e item 2 do G2): os nós da borda começam do tamanho dos outros (Ø 0,06 m). No eco, dobram para Ø 0,12 m e ganham miolo marfim (~0,74). Em escala de cinza, a troca se lê por tamanho e valor, não por matiz. Medidas acertadas em C3, C4, C10 e §6.

**Conferência final (Art Director, 2026-10-03):** condições atendidas. Os fatos 1 e 2, as contas 3 e 4 e o eco sem cor (nós de Ø 0,06 m para 0,12 m, com miolo marfim) estão no corpo. As bordas do vão (1,25 m e 1,44 m) conferem com a trança de 0,10 m e com o alcance do avatar na ponta dos pés.

## 6. Encaminhamento

**Para o G2 (concept), só depois de aprovado:**
- vistas: frente (a canônica), 3/4, perfil (a fraqueza) e de cima; ao lado, a criança de 1,10 m e a de 1,28 m para escala; linha de chão.
- silhueta: o ADR não define teste de silhueta para Símbolo. **Adaptação proposta:** o símbolo em preto a 30%, embaralhado com pelo menos 4 círculos-armadilha da §4 (emblema de quatro eixos, círculo com traço vertical, pincelada, aro com teia, ferradura); quem leu as fichas acerta ≥ 4 de 5. E dois renders: no palco do B01, com o painel de fala e o anel turquesa contra o Azul profundo; na clareira, da câmera de terceira pessoa, com os nós turquesa e dourados lado a lado (a troca tem de ser visível no celular).
- teste de descrição (2 frases): "Um anel de três fios de luz trançados que não fecha no alto e deixa um fio solto até o chão. Visto antes de nascer, ele reaparece numa clareira, e é nele que a criança decide deixar a infância para trás." Se a resposta nomear jogo, marca ou emblema existente, reprovou.
- paleta: Turquesa #86C8C9 nos fios e nos nós; Dourado #D6B36A só nos dois nós da borda, depois do eco, com miolo em marfim #E9DEC6; cor chapada sem sombreamento, para ler como luz; sem o contorno do toon, que engrossaria os fios (o contorno já é desligável por material, `PIPELINE.md` §13); sem Violeta.
- orçamento (`PIPELINE.md` §4, HIPÓTESE v0): Prop médio (1–4 m), 2 000 tris no LOD0, LOD1 ≤ 50%, 1 material, 512 px. É apertado para três fios: tubo de 6 lados, ~48 segmentos por fio; no LOD1, 4 lados. Os dois nós da borda como peça filha à parte, em duas versões trocáveis (Ø 0,06 m turquesa; Ø 0,12 m dourado com miolo marfim), sem mexer no material do anel; o fio longo dos 8 anos como peça trocável (C9).
- código que esta ficha pede (dependências, não feitas aqui): o greybox do palco passa de Dourado a Turquesa (`EntradaSceneSetup.cs`); a face do anel virada para o sul nos dois lugares; a leitura da flag para os nós, na clareira e no palco do título (C4); a visibilidade por destino e por `evento.q07_concluida` (C8); o fio longo aos 8 (C9). Pasta: o `PIPELINE.md` §6 não tem categoria Simbolo; proposta: `Art/Prop/simbolo_limiar/`.

**Para o G3:** bloco `### simbolo_limiar` em `PROVENIENCIA.md` com `g1:` (data, nota, pontuador), `g2:` (data, resultado da silhueta adaptada, descrição), `entrada:` (concept em `arte/referencias/simbolo_limiar/` + SHA-256), `ferramenta`, `plano`, `licenca`, `termos_url`, `g3:`. `referencias_terceiros`: nenhuma usada nesta ficha. A peça `PROTOTIPO` atual (`Art/Prototipo/Pecas/simbolo_limiar/`) é substituída, não aproveitada.

## 7. Ligações com outras fichas

**PROPOSTAS que envolvem outra ficha:**
- **`aethron` (par):** Aethron nunca olha para o símbolo e fica à direita dele, mais ao fundo, no palco. O símbolo é turquesa e curvo; Aethron é dourado e reto. O dourado dos nós depois do toque é o eco do Limiar, não Aethron reaparecendo (`SLICE` B11).
- **avatar (`avatar_crianca5`, `avatar_crianca8`):** na Vida da Ruptura, a criança vê o símbolo desde o primeiro dia (C8), o que entra no C8 do avatar da Ruptura. O centro do anel na altura dos olhos aos 5 e um palmo abaixo aos 8, e o vão acima da cabeça aos 5 (C9), dependem das alturas do `PIPELINE.md` §3. O traço do avatar (ficha `avatar`, C5), a assinatura no livro da Maelis (ficha `maelis`, C5) e o círculo a carvão da Ruptura (ficha `avatar`, C8) desenham esta forma vista de frente: o anel e o vão na posição de uma hora; o fio solto não entra no traço. A linha turquesa do traço que "para no vão" casa com C4: o vão nunca fecha.
- **Avatar, decidido no `ELENCO.md` ("Decisões de enredo que cruzam fichas"):** a criança traça **de pé**, com o braço erguido até o vão, e **não atravessa** o vão. O símbolo fica com 1,50 m e o salto do B13 segue como hoje. É o que C3, C9 e C10 desta ficha já descrevem.
- **Lysa:** a ficha dela também lê a Ruptura (`evento.anomalia_no_bosque`) com uma planta da beira que ela não sabe nomear. Não conflita com C8: as duas podem ser a mesma anomalia vista de dois lados. Quem amarra é o coordenador.
- **Nilo:** o desenho (fios de luz) não contradiz a fala dele aos 8, "um círculo de luz". Esta ficha **não** diz que Nilo esteve no símbolo: o desaparecimento segue sem solução (`SLICE` B09).
- **Tovin, Maelis, Eira:** a Q-07 leva ao bosque; com C8, é o fim dela que faz o símbolo aparecer para os destinos que não são Ruptura. Nada muda nas falas deles, e nenhum NPC comenta o símbolo (ninguém sabe `topico.limiar`).
- **Borin:** o aro dele é fechado, de ferro, no quadril; este é aberto e de luz. A ficha dele já previa isso (§4).
- **Maelis:** o protótipo dela tem "debrum dourado" (`PROVENIENCIA.md` §6), o que furaria a regra de C4 (o único Dourado pintado em Auren são os nós depois do eco). A ficha `maelis` já tira o debrum (§6); só falta trocar o protótipo.

**Zonas de silhueta que esta ficha ocupa:**
1. anel vertical de 1,20 m com vão assimétrico na posição de uma hora;
2. fio pendente e ondulado que sai do contorno do anel até o chão;
3. curva e assimetria: o oposto de Aethron.
