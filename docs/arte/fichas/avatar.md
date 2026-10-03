# Ficha G1 — `avatar` (`avatar_crianca5` e `avatar_crianca8`)

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md)). Uma ficha para as duas idades do slice.
> Fato com fonte é cânone. **PROPOSTA** = ideia desta ficha, não está nos docs e não vale até ser aprovada.

## 1. Identificação

- **id:** `avatar_crianca5` (1,10 m, `BodyScale.Crianca5`) e `avatar_crianca8` (1,28 m, `BodyScale.Crianca8`) (`PIPELINE.md` §3 e §6)
- **categoria:** Avatar, base infantil: uma topologia e um rig com dois presets de proporção, por comprimento de osso e shape key, nunca escala (`PIPELINE.md` §3)
- **onde aparece no slice:** B01–B05 sem corpo (`TheLiminalRealm` e tela de criação); B06–B13 aos 5; B14–B16 aos 8 (`SLICE` §2). A câmera fica atrás e acima dele o jogo inteiro: pivô a 85% da altura, distância de 2,5 alturas, 15° na partida e 8° em conversa (`BodyByAge.cs`, `ThirdPersonCamera.cs`) — o jogador vê o avatar **de costas, de um pouco acima**.
- **cânone de partida:**
  - nasce em Eryndor, não é isekai, começa jogável aos 5 (GDD cap. 01); "não precisa ser único escolhido" (GDD cap. 08; dossiê §I)
  - almas passam pelo Limiar antes de nascer e quase ninguém retém lembrança; um símbolo visto no Limiar reaparece em Auren; o protagonista investiga lembranças fragmentadas (dossiê §I)
  - Aethron: "Fios entrelaçados num círculo que não se fecha… Se um dia o vir de novo, lembre-se de que o viu primeiro neste lugar" e "Todos os que passam escolhem por onde entrar na vida. Você não é o primeiro e não será o último" (`limiar.fala.4`, `limiar.fala.5`)
  - destino imutável depois de confirmado (GDD cap. 02, REGRA BASE); itens de nascimento por destino e origem (`DestinyCatalog.cs`), aplicados ao inventário por `Inventario.Nascer`
  - tocar o símbolo da clareira grava `marco.eco_do_limiar` e levanta `salto_temporal_liberado`; o salto só é oferecido no símbolo e em dois passos ("Deixar a infância para trás"); o salto não concede nada e "não apaga nada" (`SLICE` B11–B13, §4.2; `q08…json`; `SaltoGatilho.cs`; `salto.sobrevive`)
  - Nilo volta do bosque lembrando só de "um círculo de luz" (`dialogo.nilo.aos_oito*`)
  - personalização planejada: nome, rosto, cabelo, olhos, pele (dossiê §J); a aparência saiu do slice, o B04 fica só com o nome (ADR-0007 §5)
  - figuras centrais preservam reconhecimento entre etapas; não criar modelo por ano de idade (GDD cap. 09)
  - acervo: `COE-PRO-001`, que o `ACERVO.csv` descreve como "menino, cabelo castanho claro, olhos azuis", e cuja imagem (`arte/referencias/acervo/imagens/estudos_historicos/exec-d5e0bd86-1dd4-44e5-b447-a7da2a4240bc.png`) mostra cabelo castanho desgrenhado, colete verde, camisa de linho, calça balão e botas; `COE-PRO-002` (criança cacheada, colete verde, camisa de linho, calção balão verde, sapato com tira, `ACERVO.csv`); protótipo `protagonista`, "cabelo cacheado, colete verde" (`PROVENIENCIA.md` §6). Os três usam a mesma roupa e põem a identidade no cabelo — justamente o que a personalização vai trocar.
- **autor da ficha / data:** Concept Art Lead (agente), 2026-10-03; condições do parecer cumpridas em 2026-10-03 (fim da §5)
- **estado:** aprovado G1 por delegação (ADR-0010, 2026-10-03)

## 2. Critérios C1–C10

**C1 — Gancho.** Uma criança de cinco anos chega a Auren enrolada na manta do destino que ela mesma escolheu, com um desenho na ponta do dedo que ninguém lhe ensinou: um círculo que ela sabe de cor, não sabe de onde vem, e cujo vão o dedo nunca cruza. *(PROPOSTA)*

**C2 — Necessidade do mundo.** Cânone: em Eryndor, toda alma escolhe por onde entrar na vida e quase todas esquecem a passagem (`limiar.fala.5`; dossiê §I); o símbolo do Limiar reaparece em Auren e alguém investiga lembranças fragmentadas (dossiê §I). **PROPOSTA:** um mundo que esquece a própria passagem deixa, de vez em quando, uma sobra em alguém. O avatar é o caso comum com uma sobra: não lembra de Aethron em palavras, lembra a forma do sinal no dedo. Não é exceção — Nilo volta do bosque com outra sobra, a luz. E o que ele escolheu no Limiar não fica no Limiar: vira a manta com que ele nasce (C4).

**C3 — Silhueta em 3 formas.** *(PROPOSTA; px pela régua de 30% do `client/tools/silhueta.py`, 120 px/m; a criança de 5 tem 132 px)* Todas abaixo do queixo, para a silhueta não depender de cabelo nem de rosto, e todas fora da faixa do braço pendente: com ombros a 0,83 m e envergadura de 1,06 m (`PIPELINE.md` §3), o braço caído ocupa de ±0,13 a ±0,18 m do centro, de 0,40 a 0,83 m do chão. Cada forma passa por fora dessa faixa ou abaixo dela, e as três ficam separadas por vazios (a cintura fina e as canelas):
1. **Trouxa nas costas:** a borda de cima da manta, enrolada uma volta (Ø 0,09 m, 11 px), atravessa as omoplatas na horizontal, de 0,70 a 0,79 m do chão, com 0,60 m de ponta a ponta (72 px). Dois cordões passam por cima dos ombros e se cruzam no peito (detalhe). As pontas da trouxa são os dois cantos de cima da manta — as "orelhas do nó" de antes, tiradas do vão do braço. Em T-pose, de frente: as pontas aparecem sob a linha dos braços, 0,19 m (23 px) para fora do tronco de cada lado. De braço caído, de frente e de costas: passam 0,12 m (14 px) por fora do braço; de costas ficam mais perto da câmera que ele e nunca são cobertas. Rígida no `Chest`. Não é o rolo da Mara, que passa por cima dos ombros e esconde o pescoço: esta fica abaixo da linha dos ombros, atrás, e o pescoço fica à vista. Também não é a caixa da Sera, que fica na frente do tronco.
2. **Sino:** abaixo da trouxa, a manta passa por dentro do cinto nas costas — a cintura afina — e se abre em sino até uma barra reta, de 0,48 m de largura (58 px), a 0,10 m das panturrilhas; aos 5, a barra fica na dobra do joelho (0,30 m do chão). Sem bicos: o sino é uma massa só, separada da trouxa pela cintura fina. De frente, em T-pose e de braço caído: a manta aparece dos dois lados das pernas abaixo das mãos, até 0,14 m (17 px) para fora de cada perna. De costas (a câmera do jogo): a maior massa da tela. Rígido (`Spine` em cima, `Hips` embaixo).
3. **Tamancos:** tamancos de madeira de sola alta, base de 0,18 × 0,10 m e 0,08 m de altura; de frente, dois blocos de 12 × 10 px sobre um tornozelo de 6 px. Leem em qualquer pose e de costas (o salto do tamanco aparece embaixo da barra). Zona livre no elenco: ninguém usa os pés (a laje do Aethron é chão, não calçado). É a forma mais fraca; se não ler a 30% no G2, a sola sobe para 0,10 m antes de qualquer outra mudança.
Leitura em preto, de frente e de costas: barra larga no alto das costas, cintura fina, sino curto, dois blocos nos pés. Nada depende de cabelo ou rosto.

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **A manta de nascimento.** Manta de lã terracota, 0,60 × 0,90 m: a borda de cima enrolada na trouxa, o resto passado pelo cinto e aberto em sino. É o destino escolhido no Limiar feito objeto — o único item com que **todo** destino nasce (`DestinyCatalog.cs`: `item.manta_boa` na serena; `item.manta_simples` na normal e na ruptura; `item.manta_remendada` na dificil). Mesmo pano da manta da casa que a ficha `mara` propõe.
Regra — o jogo lê, o jogador vê; a manta não dá nada:
- **Destino:** o estado do tecido sai do id da manta no inventário — inteira com barra tecida, lisa ou remendada; a malha é a mesma nos quatro. O contorno nunca diz o destino; o tecido diz. Base: destino não é dificuldade (ADR-0004) e a Vida da Ruptura vem "sem poder automaticamente superior" (GDD cap. 02, tabela de destinos).
- **Vida:** cada missão opcional concluída deixa uma marca no sino, lida do histórico: `evento.q03_concluida` → carrapichos na barra (a trilha do bosque); `evento.q05_concluida` → um rasgo cerzido num canto (o animal); `evento.q06_concluida` → um furo de fagulha chamuscado (a forja). Cada marca ≥ 0,08 m, para ler na câmera. Nenhuma sai, porque o salto não apaga nada (`salto.sobrevive`). As opcionais que o aviso do B12 nomeia são as marcas que a manta ainda não tem.
- **Tempo:** a manta não cresce. Aos 8 a mesma barra fica acima do joelho (0,42 m do chão) e a trouxa passa menos por fora do braço (C9): o salto se vê de costas, na câmera, sem número (`SLICE` §4.3, "a silhueta muda em tela").

**C5 — Regra exclusiva.** *(PROPOSTA; símbolo com as medidas da ficha [`simbolo_limiar`](simbolo_limiar.md): anel de 1,20 m com centro a 0,90 m do chão, vão de 40° na posição de uma hora, bordas a 1,25 m e 1,44 m)* **O traço: o círculo que não se fecha.** É o gesto que só existe porque este personagem escolheu nascer no Limiar: a sobra da passagem é a forma do sinal, na ponta do dedo (`limiar.fala.4`).
- **Na tela:** o jogador arrasta o dedo pelo anel; a linha acompanha, acende em turquesa (cor da Trama, GDD cap. 09) e **para no vão** — o dedo passa, a linha não. No B01 e na assinatura, o traço é o arco inteiro, de uma borda do vão até a outra. No B11, o traço é o mesmo trecho do gesto no mundo: das três horas (altura dos olhos da criança) até o nó da borda de baixo do vão. O fio solto do símbolo não entra no traço. Não há traço errado (cobrir a maior parte do trecho basta) e há sempre o botão "Tocar" para quem não arrasta (crianças, acessibilidade; ADR-0009).
- **No mundo (B11):** a criança fica **de pé** diante do anel, põe o dedo na trança na altura dos olhos, do lado direito, e sobe com ele até o vão: o braço estica inteiro, ela fica na ponta dos pés, e o dedo para no nó da borda de baixo do vão (1,25 m), logo acima da cabeça dela. Ali `tocar_o_simbolo` fecha, grava o que já grava (`marco.eco_do_limiar`, `salto_temporal_liberado`) e os dois nós da borda passam a dourado (ficha `simbolo_limiar`, C4). O dedo não pula o vão; ninguém passa por ele.
- **Onde — três vezes, todas em beat que já existe:**
  1. **B01**, sem corpo: depois de `limiar.fala.4`, antes do destino, o primeiro traço é só na tela. O jogador decora a forma com a própria mão.
  2. **q07**, `perguntar_na_vila`: é a assinatura no livro de Maelis, e o jogador pode recusar e fazer um risco (ficha [`maelis`](maelis.md), C5). No livro, o sinal fica em tinta escura.
  3. **B11**, `tocar_o_simbolo`: como acima. Depois, o B12 e o B13 seguem como hoje (aviso "o que se encerra", confirmação em dois passos, recarga de Auren aos 8); o traço não muda nada neles.
- **O que não faz:** não dá dano, item, atributo nem grau; não abre nada que o slice já não abra; não fecha o vão; não explica nem resolve o mistério (`SLICE` B11, B16). Não faz da criança a escolhida: Aethron diz que todos escolhem e que ela não é a primeira (`limiar.fala.5`), e Nilo traz outra sobra.
- Se o dono do `SLICE` recusar a etapa no B01 (que hoje "só escuta e responde"), o traço nasce no B11 e a fala 4 continua plantando a forma.

**C6 — Voz.** Não se aplica ao avatar (ADR-0002, nota do avatar).

**C7 — Contradição visível.** Não se aplica ao avatar (ADR-0002, nota do avatar). Em troca, C4, C5 e C8 são obrigatórios em nota 2.

**C8 — O que o destino muda no avatar.** *(PROPOSTA; só tecido, objeto e fala — dossiê §H. Ids conferem em `DestinyCatalog.cs`; destino lido de `BirthChoice.destinyId`)*
- **serena** (`item.manta_boa`, `item.brinquedo_entalhado`, `oportunidade.aulas_com_eira`): manta inteira, com barra tecida; o brinquedo entalhado vai preso no cruzamento dos cordões. Maelis recebe a assinatura como de "casa que assina em dia". Aos 8, com as aulas de Eira, assina o livro com o nome, ao lado do sinal dos 5.
- **normal** (`item.manta_simples`, `item.cantil`, `oportunidade.recado_da_vila`, `oportunidade.aulas_com_eira`): manta lisa, a de quase toda criança de Auren; cantil a tiracolo. Maelis a trata como quem já leva recado da vila. Aos 8 também assina com o nome.
- **dificil** — Vida Árdua (`item.manta_remendada`, `item.corda_puida`, `evento.ano_de_escassez`): manta com três remendos de pano cru; a corda puída faz as vezes dos cordões da trouxa. Na assinatura, Maelis mostra a página da família no ano magro, como favor lembrado e não dívida. Sem aula com Eira no destino, aos 8 assina de novo com o sinal que usou aos 5: circunstância, não castigo.
- **ruptura** (`item.manta_simples`, `item.amuleto_rachado`, `evento.sonho_recorrente`, `oportunidade.marca_do_limiar`): o amuleto rachado pende de um cordão no pescoço, sobre o cruzamento dos cordões (como a ficha `mara` o põe); nenhuma fala desta ficha reage a ele. `oportunidade.marca_do_limiar`, hoje id sem conteúdo, vira isto: desde o primeiro dia o círculo está riscado a carvão na parede ao lado da cama, em `casa_familia` — a "pergunta sem resposta" da família (`destino.ruptura.familia`) — e o anel já está parado na clareira desde o primeiro dia (ficha `simbolo_limiar`, C8). Na assinatura com o círculo, Maelis para de escrever e pergunta onde a criança aprendeu o sinal; a prancha dela já tem uma segunda folha de pé. Aos 8, assina de novo com o sinal.
- **origem:** não muda manta, trouxa nem traço. PROPOSTA desta ficha: o que vem do Limiar é igual nas três famílias.

**C9 — Mudança dos 5 para os 8.** *(PROPOSTA)*
- **Antes (5–7, 1,10 m):** barra na dobra do joelho (0,30 m); trouxa 0,12 m por fora do braço caído; tamancos pequenos; diante do anel, alcançar o vão pede o braço inteiro e a ponta dos pés (C5); a prancha de Maelis (tampo a 1,00 m) fica na altura do rosto, e ela se curva para a assinatura; no cruzamento dos cordões, o objeto do destino; nenhuma arma.
- **Depois (8, 1,28 m):** a mesma manta, com as mesmas marcas: a barra sobe para acima do joelho (0,42 m) e a trouxa, que não cresce, passa só 0,09 m (11 px) por fora do braço — a criança ficou maior que a manta; tamancos maiores, mesma forma; de volta à clareira, o vão está na altura da cabeça e não há mais nada a fazer ali (ficha `simbolo_limiar`, C5 e C9); a prancha de Maelis fica na altura do peito, e ela mostra a página sem se curvar. A câmera do jogo (atrás e acima: ~1,65 m de altura aos 5 com 15°, mais alta aos 8) vê o tampo da prancha nas duas idades; o que muda é onde ele bate no corpo da criança.
- **Variante de malha:** nenhuma além do previsto: o preset de 8 anos da mesma topologia (`PIPELINE.md` §3). A manta é a mesma peça e não ganha shape key; como os presets mudam comprimento de osso e não escala, ela mantém o tamanho.

**C10 — Momento de cartaz.** *(PROPOSTA)* A clareira. Câmera atrás e acima de uma criança de 1,10 m: uma trouxa de lã terracota atravessada nas costas, mais larga que os braços; o resto da manta em sino até a dobra do joelho; dois tamancos embaixo. À frente, um anel de luz turquesa da altura de um adulto baixo. Ela põe o dedo na trança e sobe com ele; o braço estica, ela fica na ponta dos pés, e o dedo para no nó da borda do vão, logo acima da cabeça. Os dois nós viram dourado. A linha não passa do vão, e ela não tenta.

## 3. Amarração

- **Destino, Grau ou Trama:** o destino é o estado da manta e o que fica preso nos cordões (C8); a forma é a mesma da Serena à Ruptura, o tecido muda. Grau não se aplica no slice (dossiê §E). A Trama aparece num gesto só, o traço, que não concede nada (`SLICE` §4.2).
- **Decisão de jogo:** (1) pôr ou não o sinal do Limiar no livro da vila, na q07; (2) deixar a infância agora, no símbolo (B12–B13), ou voltar a Auren e terminar as opcionais que o aviso do B12 nomeia — que são as marcas que ainda faltam na manta e que, depois do salto, nunca mais entram. *(PROPOSTA)*
- **Pilar:** viver e crescer.

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| criança de camisa de linho, colete verde, calça balão e botas, com a identidade inteira no cabelo — os dois candidatos do acervo (`COE-PRO-001`, `COE-PRO-002`) e o protótipo `protagonista` | identidade abaixo do queixo: trouxa, sino e tamancos; o cabelo pode ser qualquer um. Dos candidatos fica só a proporção infantil (cabeça grande, ~5,5 cabeças, `PIPELINE.md` §3) e o linho por baixo |
| capa curta de herói, de cor forte, presa por broche, esvoaçando | manta de dormir grossa, de criança: enrolada no alto para não arrastar, passada pelo cinto, pende em sino e não esvoaça |
| saco de dormir de aventureiro amarrado em cima da mochila | não há mochila: a trouxa é a borda da própria manta, que continua pendendo abaixo dela |
| marca no corpo, olho que brilha, sinal tatuado: o escolhido | nenhum sinal no corpo; o que veio do Limiar é um gesto de dedo, e Nilo também volta com uma sobra |
| arma aos 5 anos | aos 5 não há arma; a espada de madeira chega no B15; o traço é sempre do dedo |
| criança que atravessa o portal e "nasce de novo" | não há portal: o vão não se atravessa e não se fecha; o salto é o aviso e a confirmação que já existem |
| tocar o símbolo desperta poder | tocar grava um marco e abre uma escolha, o salto, que não dá nada |
| silhueta que muda com o destino (rico e pobre na forma) | mesma forma nos quatro destinos; muda o tecido |
| turquesa, dourado ou violeta na roupa (Trama, Limiar, anomalias, GDD cap. 09); o colete verde do protótipo, que a ficha `mara` pede para conferir | manta terracota #A86D52 ("Auren", GDD cap. 09), o mesmo pano da manta de Mara; remendos em pano cru; tamancos de madeira natural; turquesa só na linha do traço, que é sinal da Trama |

## 5. Avaliação — parecer do Art Director: 15/16 (C6 e C7 N/A), aprovado (ADR-0010)

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 1 | 2 | 2 | N/A | N/A | 2 | 2 | 2 | **15/16** |

- **Corte:** C6 e C7 não se aplicam ao avatar (ADR-0002, nota do avatar). Decisão do pontuador: o total é sobre os 8 critérios avaliados (16 pontos), e o corte é a proporção do ADR (14/20 = 70%) arredondada para cima, **≥ 12/16**. Continuam valendo nenhum zero em C2–C5 e, no avatar, **C4, C5 e C8 em 2**. **Atingido** (HIPÓTESE do ADR, a recalibrar).
- **Pontuador / data:** Art Director (agente) / 2026-10-03. Não escreveu a ficha.
- **Veredito:** **aprovado G1 por delegação (ADR-0010).** C4, C5 e C8 estão em 2, como o ADR exige. C3 fica em 1 e é a primeira coisa que o G2 precisa resolver (item 1 abaixo). Duas dependências não são desta ficha: a etapa do traço no B01 (dono: `SLICE`) e a assinatura da q07 (ficha `maelis`; dono: narrativa/quest). Se as duas caírem, C5 continua de pé pelo B11. Se cair o traço do B11, C5 perde a base e a ficha volta para nota.

**Nota por critério (1 linha cada):**
- **C1 = 2.** Tem detalhe concreto (a manta do destino, o desenho na ponta do dedo) e conflito (o círculo que não fecha). O C10 contradiz o gancho; ver "coerência interna".
- **C2 = 2.** A "sobra" da passagem sai de cânone que conferiu (`limiar.fala.5`; dossiê §I: quase ninguém lembra, o símbolo reaparece) e não faz do avatar uma exceção, porque Nilo traz outra sobra. É específico do Limiar de Eryndor. O resto é PROPOSTA.
- **C3 = 1.** De frente, em T-pose, a 30%, as três formas leem (12, 58 e 14 px fora do contorno) e nenhuma cai em zona arbitrada de outro personagem. Dois problemas: (a) as orelhas do nó só existem de braço aberto. Elas vão de 0,105 a 0,205 m do centro, um palmo abaixo da axila, que é onde pende o braço de uma criança de 1,10 m (ombros a 0,83 m e envergadura de 1,06 m, `PIPELINE.md` §3). No idle e na câmera de costas, que é a do jogo inteiro, elas atravessam o braço e sobram ~4 px. (b) Os bicos são os cantos do sino, então na câmera do jogo o avatar lê como uma forma só. Para subir a 2, falta uma primeira forma que sobreviva ao braço caído e à câmera de costas.
- **C4 = 2.** A manta tem medida e três camadas de regra escritas com ids que conferem: a manta por destino, as marcas por `evento.q03/q05/q06_concluida` e a barra que sobe com o corpo. Nada é concedido, e o inventário não tem como descartar a manta (`Inventario.cs`), então a regra não quebra. É exibição de estado, aceita pela mesma leitura do aro do Borin. A camada do destino repete a manta da Mara (mesmo pano e mesmo estado); o que é só do avatar são as marcas e a barra.
- **C5 = 2.** O traço está especificado na tela e no mundo, com o que ele não faz e com uma alternativa sem gesto. A geometria fecha: com ombros a 0,83 m e braço de ~0,41 m, o braço erguido chega a ~1,24 m, e na ponta dos pés alcança o nó a 1,25 m, no limite, como a ficha quer. Responde à pergunta do ADR para o avatar: é o gesto que só existe porque a criança passou pelo Limiar. Ajuste: a tela pede "a maior parte do arco", mas o clipe do mundo só sobe das 3 h até o vão. Falta dizer qual dos dois é o traço do B11.
- **C6, C7 — N/A** (ADR-0002, nota do avatar).
- **C8 = 2.** Cada um dos quatro destinos tem estado de tecido, objeto no nó e reação na assinatura, com ids que conferem em `DestinyCatalog.cs`. A origem fica igual por decisão escrita. É concreto, ramificado e só usa fala e objeto.
- **C9 = 2.** Tem duas batidas marcadas: a barra passa da dobra do joelho para cima dele; o vão passa do limite do braço para a altura da cabeça; os olhos passam de baixo para cima da prancha de Maelis. Não pede variante de malha além do preset de 8 anos. Há um deslize de conta, listado abaixo.
- **C10 = 2.** A cena está escrita e é encenável na clareira que já existe, de costas, que é como o jogador vê o avatar. Cabe em ~5 s.

**Fatos dados como cânone que não conferiram:**
1. A ficha descreve `COE-PRO-001` como "cabelo castanho desgrenhado, colete verde, camisa de linho, calça balão, botas" e cita o `ACERVO.csv`. O CSV diz só "menino, cabelo castanho claro, olhos azuis, vestida". A descrição confere com a imagem do acervo (`arte/referencias/acervo/imagens/estudos_historicos/exec-d5e0bd86-….png`), não com o CSV. Citar a imagem.
2. No C4, "GDD cap. 02: nenhum arquétipo melhor" sustenta que o contorno nunca diz o destino. No GDD cap. 02, essa frase fala das **origens** (arquétipos familiares). Para destino, a base é o ADR-0004 e o "sem poder automaticamente superior" da Vida da Ruptura (GDD cap. 02, tabela).
3. No C8, a frase sobre a origem, "o que vem do Limiar é igual nas três famílias (GDD cap. 02)", não está no GDD. É PROPOSTA desta ficha (boa); a fonte deve sair.

**Contas e coerência interna:**
4. O C3 diz, sobre as orelhas: "Câmera do jogo: aparecem dos dois lados do tronco, vistas de costas". Isso não fecha com a proporção do placeholder: de braço caído, elas atravessam o braço (ver C3).
5. O C9 põe os bicos "nas coxas" aos 8. Com a barra a 0,42 m e os bicos 0,12 m abaixo, eles ficam a ~0,30 m do chão, na altura do joelho de uma criança de 1,28 m.
6. O C1 diz que ela "tenta fechar e não consegue"; o C10 diz que "a linha não passa do vão, e ela não tenta". Pelo C5, quem tenta é o dedo do jogador no B01; no mundo, a criança para no vão. O gancho deve seguir o gesto.

Conferiram: ids, alturas e regra de presets (`PIPELINE.md` §3, §6; `BodyScale`); pivô a 85% e distância de 2,5× (`BodyByAge.cs`); itens, oportunidades e eventos por destino, e a `oportunidade.marca_do_limiar` sem conteúdo (`DestinyCatalog.cs`); `Inventario.Nascer`; q03, q05 e q06 como as três opcionais (`SLICE` B07, B12); `marco.eco_do_limiar` e `salto_temporal_liberado` (`q08…json`, `QuestCatalog.Flags`); `salto.sobrevive`; falas 4 e 5; "um círculo de luz" de Nilo (`aos_oito_cumprida` e `aos_oito_quebrada`); `destino.ruptura.familia`; dossiê §I e §J; ADR-0007 §5; GDD cap. 01, 08 e 09; `COE-PRO-002` e o protótipo `protagonista` (`PROVENIENCIA.md` §6); tampo da prancha de Maelis a 1,00 m (ficha `maelis`); vão a 1,25–1,44 m pela geometria da ficha `simbolo_limiar`; orçamento do Avatar (`PIPELINE.md` §4).

**Para o G2 — o que o concept precisa provar:**
1. **Braço caído.** Render de frente e de costas no idle, e na câmera do jogo (pivô a 0,94 m, 2,75 m atrás): as orelhas do nó leem sem atravessar o braço. Se não lerem, a forma 1 muda de lugar antes do concept final. O lugar novo fica fora da zona do braço pendente, do rolo da Mara (ombros) e da caixa da Sera, que já usa a mesma axila (ficha `sera`, §7). C3 só sobe a 2 com isso.
2. **De costas, não é capa nem saia.** Silhueta de costas em preto, com teste de descrição: o sino com dois bicos tem de ler como manta de dormir amarrada, não como a capa curta que a §4 recusa.
3. **Corrida.** O clipe `Running` com a manta rígida, a barra a 0,10 m da panturrilha e as orelhas ao lado do braço que balança. Mostrar onde atravessa antes de gastar os 4 ossos de mola da §6.
4. **O gesto cabe no corpo.** A criança de 1,10 m com o dedo no nó da borda a 1,25 m, de braço inteiro e na ponta dos pés (o render extra da §6), e o mesmo quadro com 1,28 m, sem esticar.
5. **Marcas e estados na tela.** As três marcas de vida (≥ 0,08 m) e os três estados do tecido se distinguem na câmera do jogo, num celular em paisagem. A manta terracota se separa da terra, das paredes de Auren e da manta da Mara quando as duas estão na mesma cena.
6. **O cabelo não muda a silhueta** (prova de cabelo da §6): com duas cabeças, as três formas continuam as mesmas.

**Condições cumpridas em 2026-10-03:** *(pelo autor da ficha; o parecer acima não foi alterado)*
- **C3 = 1, problema (a), as orelhas atravessam o braço:** os dois cantos de cima da manta saíram do vão do braço e viraram as pontas da **trouxa**, enrolada na horizontal nas omoplatas (0,70–0,79 m), com 0,60 m de ponta a ponta. De braço caído ficam 0,12 m (14 px) por fora do braço, de frente e de costas; de costas, mais perto da câmera que ele. Fica fora do rolo da Mara (abaixo da linha dos ombros, pescoço à vista) e da caixa da Sera (atrás, não na frente).
- **C3 = 1, problema (b), sino e bicos leem como uma forma só:** os bicos saíram. O sino tem barra reta e fica separado da trouxa pela cintura fina (a manta passa pelo cinto). A terceira forma é nova e separada: **tamancos** de sola alta, nos pés, zona livre no elenco. São três massas separadas por vazios; nenhuma depende de cabelo ou rosto.
- **C5, ajuste do traço do B11:** C5 diz que, no B11, o traço da tela é o mesmo trecho do clipe (das três horas até o nó da borda de baixo do vão); o arco inteiro fica para o B01 e para a assinatura.
- **Fato 1 (`COE-PRO-001`):** §1 cita o que o `ACERVO.csv` diz e a imagem do acervo de onde vem a descrição da roupa.
- **Fato 2 (C4, "nenhum arquétipo melhor"):** a base passou a ser o ADR-0004 e o "sem poder automaticamente superior" da Vida da Ruptura (GDD cap. 02, tabela de destinos).
- **Fato 3 (C8, origem):** "igual nas três famílias" ficou como PROPOSTA desta ficha, sem fonte.
- **Conta 4 (orelhas vistas de costas):** resolvida com (a).
- **Conta 5 (bicos aos 8):** os bicos saíram; aos 8 a barra fica a 0,42 m, acima do joelho de 1,28 m (C4, C9).
- **Conta 6 (C1 × C10):** C1 agora diz que o dedo nunca cruza o vão; C10 mostra o dedo parando no vão e a criança sem tentar passar. Os dois seguem o gesto de C5.
- **Câmera e prancha de Maelis (pedido do coordenador):** C9 usa a câmera real (atrás e acima), que vê o tampo nas duas idades; a batida é onde a prancha bate na criança (rosto aos 5, peito aos 8) e se Maelis precisa se curvar.
- **Arbitragem 2 do `ELENCO.md`:** a manta não lê nem reage ao amuleto (item 8: só Borin e Oren reagem a ele); nenhuma fala desta ficha lê o que a criança carrega (item 9).
- **G2, item 1 do parecer (braço caído):** a §6 pede silhueta de braço caído, de frente e de costas, além da T-pose, e o render na câmera real. As zonas da §7 foram atualizadas.

**Conferência final (Art Director, 2026-10-03):** condições atendidas. Estão no corpo os problemas (a) e (b) de C3, o ajuste de C5, os fatos 1–3, as contas 4–6 e a Arbitragem 2, itens 8 e 9. C4, C5 e C8 continuam em 2. Ressalvas, sem pendência: em T-pose a trouxa (topo a 0,79 m) fica rente ao braço (ombro a 0,83 m) e funde com ele em preto, então a forma 1 só lê de braço caído e de costas, como a §6 já pede. O C3 e o §7 ainda falam da "caixa da Sera" (hoje é a bolota), e o C8 põe o amuleto no pescoço "como a ficha `mara` o põe", o que a ficha da Mara já não diz.

## 6. Encaminhamento

**Para o G2 (concept), só depois de aprovado:**
- vistas: frente, perfil, costas e 3/4 em T-pose, fundo neutro, linha de chão; **as duas idades lado a lado** (1,10 m e 1,28 m) com a mesma manta e a barra marcada (0,30 m e 0,42 m do chão)
- **prova de cabelo:** a mesma vista com duas cabeças diferentes (cabelo curto liso; cachos volumosos) — a silhueta das três formas não pode mudar
- silhueta: trouxa, sino e tamancos em preto a 30% (`client/tools/silhueta.py`), em T-pose **e** de braço caído, de frente **e** de costas; e um render na câmera real do jogo (pivô a 0,94 m, 2,75 m atrás, 15°), que é como o jogador vê o avatar; conferir que a manta terracota não some contra a terra e as paredes de Auren nem se confunde com a manta da Mara na mesma cena
- de costas, teste de descrição: a trouxa com o sino tem de ler como manta de dormir enrolada, não como capa curta nem como saco de dormir de mochila (§4)
- corrida: o clipe `Running` com a manta rígida, a barra a 0,10 m da panturrilha e a trouxa por fora do braço que balança; mostrar onde atravessa antes de gastar ossos de mola
- paleta: os três estados da manta (barra tecida, lisa, remendada) em terracota #A86D52; tamancos de madeira natural; roupa de baixo neutra; turquesa só no VFX do traço; proibidos na roupa: turquesa #86C8C9, dourado #D6B36A, violeta #9777B8
- objeto à parte: a manta aberta (0,60 × 0,90 m) e vestida; os cordões; os tamancos (0,18 × 0,10 × 0,08 m aos 5); as três marcas de vida, cada uma ≥ 0,08 m; os quatro objetos de destino (brinquedo, cantil, corda, amuleto no cordão); o círculo a carvão da parede (Ruptura)
- orçamento (`PIPELINE.md` §4, HIPÓTESE v0): 15 000 tris, 2 materiais (corpo; manta), textura 1024, 75 ossos → trouxa rígida no `Chest`, sino rígido (`Spine`/`Hips`); se o `Running` atravessar a barra, até 4 ossos de barra com mola simples por script (cabem nos 20 de roupa); estados e marcas são máscara de textura ligada por propriedade de material, sem malha extra
- custo declarado: **1 clipe próprio** (traçar de pé, ~2 s: dedo na trança à altura dos olhos, subida até o vão, braço inteiro e ponta dos pés, com alvo de mão no nó da borda a 1,25 m); **1 componente de traço** na tela, usado no B01, no livro de Maelis e na clareira; 1 decal (parede da Ruptura)
- render extra: a criança de 1,10 m de pé diante do anel de 1,20 m (centro a 0,90 m), com a mão no nó da borda do vão, e o mesmo quadro com 1,28 m, sem esticar — prova de que o gesto cabe no corpo
- testes: silhueta com Maelis, Mara, Borin, Nilo e Sera (≥ 4/5); descrição; troco com Nilo na clareira — tem de quebrar (ele lembra de luz, não da forma, e não traça)
- dependências: etapa do traço no B01 (dono: `SLICE`); assinatura na q07 (ficha `maelis`; dono: narrativa/quest); medidas do anel e troca de cor dos nós (ficha `simbolo_limiar`)

**Para o G3:** blocos `### avatar_crianca5` e `### avatar_crianca8` em `PROVENIENCIA.md` com `g1:` (data, nota, pontuador), `g2:` (data, silhueta n/5, descrição, troco), `entrada:` (concept em `arte/referencias/avatar/` + SHA-256), `ferramenta`, `plano`, `licenca`, `termos_url`, `g3:`. `referencias_terceiros`: nenhuma usada nesta ficha. O protótipo `protagonista` (`estado: PROTOTIPO`) não serve de entrada.

## 7. Ligações com outras fichas

PROPOSTAS desta ficha que envolvem outro personagem:
- **Mara:** a manta da criança é do mesmo pano terracota e no mesmo estado da manta da casa (ficha `mara`, C4), e o amuleto da Ruptura vai no pescoço, como lá. Mara usa a dela em rolo por cima dos ombros, escondendo o pescoço; a criança enrola a dela no alto das costas, abaixo da linha dos ombros, com o pescoço à vista — as duas não dividem forma. Nova aqui: o círculo a carvão na parede ao lado da cama, em `casa_familia`, desde o primeiro dia (Ruptura).
- **Maelis:** a assinatura da q07 usa o traço (no livro, em tinta escura); a prancha bate no rosto da criança aos 5 e no peito aos 8; Maelis reage ao sinal por destino e tem segunda folha na Ruptura (ficha `maelis`, C4, C5, C8).
- **Símbolo do Limiar:** a criança traça de pé e o dedo para no nó da borda de baixo do vão (1,25 m); é esse toque que fecha `tocar_o_simbolo` e acende os nós em dourado (ficha `simbolo_limiar`, C4). O fio solto não entra no traço; ninguém atravessa o vão. Na Ruptura, o anel já está parado na clareira desde o primeiro dia (ficha `simbolo_limiar`, C8), o mesmo círculo que a criança risca a carvão na parede.
- **Nilo:** o "círculo de luz" dele é a outra sobra do Limiar, a prova de que o avatar não é único; nenhuma fala nova.
- **Nilo e Sera:** não podem usar manta em sino, trouxa horizontal nas costas nem tamancos (registrar nas fichas deles). A caixa da Sera fica na frente do tronco; a trouxa, atrás.
- **Borin:** a fagulha da forja na q06 marca a manta; nenhuma fala nova (combina com a prova no aro, que lê o mesmo evento). Ele não reage ao amuleto por esta ficha (`ELENCO.md`, Arbitragem 2, item 8, é dele e de Oren).
- **Lysa:** o rasgo cerzido da q05 vem do animal; quem cerze não está definido nesta ficha.
- **Oren:** os carrapichos da q03 vêm da trilha; nenhuma fala nova.
- **Eira:** aos 8, serena e normal assinam com o nome porque o destino oferece as aulas dela (`oportunidade.aulas_com_eira`); nenhuma fala nova.
- **Aethron:** o traço nasce da fala 4 dele e não pede nada novo dele.

Zonas de silhueta que esta ficha ocupa: **barra horizontal atrás, na altura das omoplatas (0,70–0,79 m aos 5), mais larga que o braço caído; sino atrás das pernas, da cintura fina à dobra do joelho aos 5 (acima do joelho aos 8), barra reta; os pés, em blocos (tamancos).**
