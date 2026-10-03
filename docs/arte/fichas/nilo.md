# Ficha G1 — `nilo`

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md); a régua 0/1/2 está lá).
> Fato com fonte é cânone. **PROPOSTA** = ideia desta ficha, não está nos docs e não vale até ser aprovada. Par desta ficha: [`sera`](sera.md).

## 1. Identificação

- **id:** `nilo` (`NpcCatalog.cs`; GDD cap. 06, NPC-07)
- **categoria:** Npc, base criança (`PIPELINE.md` §3.1). O objeto de C4 é um Prop à parte, `forquilha_nilo` (**PROPOSTA**).
- **onde aparece no slice:**
  - B07: rotina de infância — `praca_centro` de manhã (aula com Eira), `entrada_bosque` à tarde (`atividade.brincar_perto_do_bosque`), `casa_nilo` à noite (`NpcCatalog.cs`); q03, objetivo `procurar_na_trilha` em `entrada_bosque` (`q03_o_cesto_perdido.json`).
  - B08: q04, `decidir` na praça e `sustentar_a_escolha` em `casa_nilo` (`q04_uma_promessa.json`).
  - B09: some. `evento.nilo_desapareceu` é gravado na conclusão da q04; enquanto valer, a rotina dele aponta para a âncora-sentinela `ausente`, e a q03 se encerra (ADR-0007 §3; `NpcCatalog.AncoraAusente`, que hoje só ele usa). A q07 é a busca (ADR-0005, decisão 2).
  - B14: volta aos 8 sem saber onde esteve, lembrando só "um círculo de luz"; manhã em `posto_guarda` (`atividade.olhar_o_treino`), tarde em `entrada_bosque` ("olhando o bosque de longe"), noite em casa. **Implementado como [PROPOSTA]** (`SLICE` B09, nota de 2026-10-01; `NpcCatalog.cs`, `B14 [PROPOSTA]`); pelo ADR-0007 §3, o retorno é da leva B.
- **cânone de partida:**
  - "amigo de infância impulsivo; aventuras e vínculo futuro" (dossiê §G); "amigo de infância com trajetória própria" (GDD cap. 06). `traco.impulsivo`; vínculo com `sera` (`relacao.amigo_de_infancia`); sabe só `topico.vila_auren` e `topico.bosque` (`NpcCatalog.cs`).
  - A regra do bosque é a mesma para adulto e criança: só com o Tovin. "Até a entrada, pode. Depois dela, só comigo" (`dialogo.tovin.sobre_bosque`); "Da entrada para dentro, nem eu vou sem o Tovin" (Lysa, adulta, `dialogo.lysa.sobre_bosque`); "a entrada do bosque é o seu limite" (`dialogo.mara.em_casa`).
  - Q-04: a confiança dele e a de Sera vão a +20 com a promessa cumprida, a −20 com a quebrada (ADR-0007 §4; `ReputationSystem.Consequencias`). Testemunha os dois desfechos, o próprio sumiço e o salto (`NpcMemory.Testemunhos`).
  - Pistas da q07 (`strings.pt-BR.json`): a cama amanheceu vazia (Maelis); passou cedo "com um embrulho debaixo do braço: pão" (Oren); na véspera perguntou "o que tem depois da clareira" (Eira); "pegada pequena na trilha, indo para dentro do bosque. Nenhuma voltando" e "O rastro acabou na clareira" (Tovin).
  - Falas já escritas, grafo `nilo_brincar` (`DialogueCatalog.cs`): `na_escola` ("Psiu! A Eira tá olhando…"), `chamando` (com as opções "Vamos!" e "Agora não dá."), `combinado` ("…Quem chegar por último é sapo!"), `promessa_cumprida`, `promessa_quebrada`, `aos_oito`, `aos_oito_cumprida` ("Quero ser guarda…"), `aos_oito_quebrada`. Esta ficha não muda nenhuma.
  - Hoje: protótipo a 1,10 m, escalado inteiro para 1,28 aos 8 (`Prototipos.cs`; `NpcActor.AjustarCorpo`: "HIPOTESE v0: Nilo e Sera têm a idade do jogador").
  - Acervo antigo (`ACERVO.csv`): COE-NPC-007 a 010 dão túnica azul, macacão marfim, cabelo ruivo-castanho, sardas e um lenço mostarda de dono provável; o "kit de exploração" (bolsa de lona, lupa, caderno) é COE-PRP-006, de dono incerto (Nilo ou Lysa). Não é cânone; é o default de §4.
- **autor da ficha / data:** Concept Art Lead (agente), 2026-10-03
- **estado:** aprovado G1 por delegação (ADR-0010, 2026-10-03); C2, C3, C4, C5, C7, C9 e §7 revistos no mesmo dia pelas condições do §5 e pela Arbitragem 2 do [`ELENCO.md`](ELENCO.md); folga da forquilha (conferência final) corrigida em 2026-10-03 (W3), reconferido sem pendência pelo Art Director (2026-10-03, leva C1, fim da §5)

## 2. Critérios C1–C10

**C1 — Gancho.** *(PROPOSTA)* O menino que imita o Tovin com o que acha — uma forquilha atravessada nas costas no lugar do chifre, a calça enrolada no lugar dos canos — passa as tardes com a ponta dos pés na linha do bosque, à vista de todos, e esconde um buraco no espinheiro que só ele conhece. Na manhã em que ele some, o que fica é a forquilha, cravada onde o rastro dele acaba.

**C2 — Necessidade do mundo.** Cânone: Auren encosta no Bosque dos Sussurros (dossiê §I) e vive dele; toda tarde Lysa colhe e Tovin caça na `entrada_bosque` (`NpcCatalog.cs`). A regra do bosque vale para todos: depois da entrada, só com o Tovin. A própria Lysa, adulta, não entra sem ele (§1). Ninguém explica o que há depois: Eira não sabe o que tem depois da clareira (`dialogo.eira.lugar_vazio`); do Limiar, "ninguém daqui sabe te contar" (`dialogo.lysa.sobre_limiar`); e o bosque "não avisa" (`dialogo.tovin.sobre_bosque`). Nilo passa a tarde exatamente ali, na mesma âncora e no mesmo período dos dois adultos, e aos 8 quer ser guarda (`dialogo.nilo.aos_oito_cumprida`; rotina no `posto_guarda`). **PROPOSTA (o resto):** um bosque com uma porta e uma chave só — o Tovin — produz a criança que quer ser a chave. Nilo copia o único que passa, com o que um menino de 5 anos acha: um galho em diagonal nas costas onde o Tovin leva o chifre, e uma calça velha de adulto enrolada grossa nas canelas onde o Tovin usa os canos de couro (C3). Aos 8, a cópia vira desejo dito em voz alta.

**C3 — Silhueta em 3 formas.** *(PROPOSTA; reescrita em 2026-10-03, condição (a) do §5)* Tem 1,04 m, a menor criança do elenco (o avatar tem 1,10), dentro de 1,00–1,20 (`PIPELINE.md` §3.1). Referência de corpo: ombros de 0,24 m (pontas em ±0,12 m), linha dos ombros a 0,81 m, cabeça de 0,16 m (±0,08 m) com o topo a 1,04 m. Linguagem do par: Nilo é feito de pontas e de diagonais; Sera, de volumes empilhados no eixo. Medidas a 30% pela régua do `client/tools/silhueta.py` (120 px/m).
1. **Forquilha.** Galho de 1,00 m — cabo de 0,80 m e dois braços de 0,20 m abertos em ~60° —, com Ø 0,035 m (4 px), atravessado nas costas a ~23° da vertical, preso por uma tira de couro (detalhe). A ponta de baixo fica atrás da coxa direita, a 0,35 m do chão (x = +0,10 m), escondida pela perna quando vista de frente. O cabo passa por cima do ombro esquerdo, perto da ponta dele (x = −0,10 m na linha dos ombros; a borda de fora do cabo fica rente à ponta do ombro, ~0,1 cm por dentro, em −0,119 m), e sobe ao lado da cabeça com 4,5 cm de folga no ponto mais perto (5 px na folha). A forquilha abre a 1,08 m (x = −0,22 m), com as pontas a 1,28 m (x = −0,20 m) e a 1,20 m (x = −0,38 m). De frente, só o terço de cima sai do contorno: um Y acima e ao lado da cabeça, por cima do ombro esquerdo, 0,24 m (29 px) acima do topo da cabeça, e nada sai embaixo, por isso não lê como lança.
   - **Conta da folga** (correção de 2026-10-03, W3; a versão anterior, com o cabo em x = −0,07 m a ~20°, passava a 1,3–2 cm da cabeça e fundia com ela na folha): cabeça como elipse de 0,16 × 0,19 m, do queixo a 0,85 m ao topo a 1,04 m (centro em 0,945 m); eixo do cabo de (+0,10; 0,35) a (−0,10; 0,81), seguindo até a forquilha. A menor distância entre a borda da cabeça e o eixo do cabo é de 6,3 cm; tirando o raio do cabo (1,75 cm), sobram **4,5 cm** de vão (5,5 px na folha; ~14 px na máscara de 512 px do `silhueta.py`, acima do bloco de 5 px que o preenchimento do fundo precisa para entrar no vão). Com a cabeça como círculo de 0,16 m, 5,4 cm. Os braços do Y ficam acima de 1,08 m e a mais de 0,15 m da cabeça. É o espelho do chifre do Tovin, que sai acima do ombro **direito** (C2; ficha `tovin`, C3). T-pose: sobrevive (rígida no `Chest`, sem osso próprio; o braço em T não a toca). Câmera: de costas e de 3/4, é a primeira coisa que se vê. **Riscos:** diagonal nas costas lê como cabo de espada, e o que separa é o Y aberto e sem guarda; na corrida, o braço direito pode bater na ponta de baixo (G2).
2. **Redemoinho.** Cabelo curto e rente (não conta), menos um tufo duro que nasce no lado direito do cocuruto e sai para cima e para fora, a ~50° da vertical, em cunha: 0,14 m de comprimento, 0,07 m na base, 0,03 m na ponta (17 × 8 px, afinando até 4). A ponta fica 0,08 m além do lado direito da cabeça e 0,07 m acima do topo. É o único contorno de cabeça do elenco que sai para um lado só, e em ponta: a nuvem da Maelis é redonda e ocupa os dois lados; o coque da Sera é centrado; o chifre do Tovin sai de cima do ombro, não da cabeça. Fica do lado oposto da forquilha, com a cabeça inteira entre os dois (≥ 0,20 m, 24 px), e por isso as duas formas não se fundem em preto. T-pose: sobrevive (rígido no `Head`). Câmera: de costas e de 3/4, a cunha quebra o contorno redondo da nuca, e é ela que reconhece o Nilo aos 8 (C9). **Risco:** ler como chifre. A cunha tem ponta romba e duas ou três mechas separadas; se o teste de descrição disser "chifre", ela deita para 65°.
3. **Rolos.** Calça velha de adulto enrolada em quatro voltas em cada canela: anéis de Ø 0,17 m sobre canelas de 0,07 m (+5 cm de cada lado, 6 px), de 0,08 a 0,22 m do chão, sobre uma botina baixa do tamanho dele. É como ele imita os canos de couro do Tovin, que abrem no joelho e afinam no tornozelo (ficha `tovin`, C3): não sabe fazer funil, faz rolo, e a forma sai ao contrário. É também o contrário do avatar, que nessa altura mostra a canela fina entre a barra do sino (0,30 m) e o tamanco (ficha `avatar`, C3). T-pose: sobrevive (rígido no `LowerLeg`). Câmera: são os pés de quem anda na frente da criança. Está no limite (6 px por lado): se o teste não ler, entra uma quinta volta (Ø 0,19 m). Na corrida, os rolos podem bater um no outro (G2).

Detalhe, não forma: camisa de adulto enfiada no cinto e estufada de achados; carrapichos e espinhos presos nos rolos (C7). Validador: V09 mede h = 1,11 m com o tufo (faixa 1,00–1,20); V13, r_cabeca ≈ 0,27 (faixa 0,19–0,28). A forquilha é Prop à parte e não entra na conta. No `silhueta.py`, a caixa da figura vai até a ponta da forquilha: `@1,28` aos 5.

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **A forquilha.** Prop `forquilha_nilo`: o galho de C3, de 1,00 m, Ø 0,035 m, braços de 0,20 m. Cravado, enterra 0,15 m e fica com 0,85 m de fora, a altura do ombro dele aos 5.
Regra: o jogo lê dois eventos que já existem e já movem a rotina dele, `evento.nilo_desapareceu` e `marco_idade_8` (`NpcCatalog.cs`; `QuestCatalog.EventoNiloDesapareceu`; `AgeAdvanceCatalog.SaltoInfancia`). É o gatilho único dos marcos do sumiço (`ELENCO.md`, Arbitragem 2, item 1).
- Sem `evento.nilo_desapareceu`: nas costas dele (forma 1).
- Com o evento e sem `marco_idade_8`: sai das costas e fica cravada no ponto onde a escolta do Tovin termina, 4 m depois da linha das árvores, já dentro da clareira e antes da âncora dela (linha em z = 62; ponto em z ≈ 66, x ≈ +1,5; âncora da clareira em z = 70; símbolo em z = 72, a ~6 m; clareira de z 62 a 78; `AurenSceneBuilder.cs`; fichas `tovin`, C5, e `simbolo_limiar`). É onde o rastro dele acaba. Fica nos três períodos, sem colisor, e não bloqueia o caminho.
- Com `marco_idade_8`: volta às costas dele (C9). O ponto fica vazio.

O que o jogador vê: na q07, seguindo o Tovin pela trilha, a escolta termina na vara, e a fala "O rastro acabou na clareira" (`dialogo.tovin.depois_da_busca`) ganha um objeto no lugar. Quem passa pelo vão antes já a encontra lá. O jogador viu essa vara nas costas do Nilo todo dia, e por isso a pista se lê sem texto. Depois do salto, ela está de novo nas costas dele, e o lugar está vazio. Nada é concedido: não é item, não vai para o inventário, não muda número.
Custo: um Prop médio (`PIPELINE.md` §4: até 2 000 tris, LOD1, 1 material, 512 px) e o componente de "peça por evento" que o `ELENCO.md` já lista.

**C5 — Regra exclusiva.** Cânone: é o único NPC que pode não estar em Auren (`AncoraAusente`: "Hoje só Nilo desaparecido aponta para cá"). *(PROPOSTA, depende do dono do conteúdo da q03)* **Pescar além da linha.** O vão de 6 m é aberto (B10; `AurenSceneBuilder.Bosque`) e qualquer um passa por ele. O que ninguém alcança está dentro do espinheiro, nas barreiras de 2 m de espessura dos dois lados do vão, onde a mão não chega nem do lado de cá nem do de lá. No objetivo `procurar_na_trilha` (q03, `entrada_bosque`, NPC `nilo`), o que se procura (o cesto, ou a pista dele, conforme a variante da q03) está preso no espinheiro, a um palmo do chão, ao lado do vão. O jogador pergunta com a fala de missão que já existe ("Nilo, você viu um cesto lá na trilha?"). Nilo põe a ponta dos pés na linha, enfia a forquilha por baixo das ramas, engancha pela alça e puxa; o objetivo fecha. O alcance por cima da linha é só dele (`ELENCO.md`, Arbitragem 2, item 2). É também a razão, dentro da ficção, de a q03 se encerrar quando ele some (ADR-0007 §3): sem o Nilo, ninguém tira nada do espinheiro. Custo: um clip próprio de ~3 s (enganchar e puxar), com a forquilha passando das costas para a mão.

**C6 — Voz.** Convida e desafia em frases de duas a quatro palavras ("Vem!", "hein?", "tá?"), mede a coragem pela vara e repete a última palavra quando fala sério. Bate com as falas já escritas, e nenhuma delas muda.
1. "Vem! Eu sei um lugar na beira do bosque que ninguém conhece. Vamos?" (`dialogo.nilo.chamando`, já escrita)
2. "Pisar na linha é coragem. Passar dela é coisa do Tovin. Eu ainda não sou o Tovin. Ainda." *(PROPOSTA: nó novo de tarde, em `entrada_bosque`)*
3. "Fica atrás de mim e não pisa, hein? Até a vara é nosso. Depois da vara, só a vara vai." *(PROPOSTA: a cena de C5)*

**C7 — Contradição visível.** *(PROPOSTA)* Na frente de todos, ninguém respeita tanto a linha; a roupa dele diz o contrário. Toda tarde em `entrada_bosque` (rotina, `NpcCatalog.cs`), à vista de quem chega, ele está com os pés exatamente na linha, o corpo inclinado e a forquilha cutucando o mato. Quando o jogador se aproxima a ~2 m, dá um passo atrás, para o lado da vila. Até a linha pode ("Até a entrada, pode"), e é só isso que ele mostra. Mas os rolos da calça vivem cheios de carrapicho e espinho, e a camisa, de coisas que só nascem do outro lado: pinha, pena, folha escura. Quem só fica na linha não volta assim. De onde isso vem é o segredo da q04 (abaixo). Custo: um idle próprio de tarde (inclinado, cutucando; o passo atrás é opcional, por gatilho de distância) e a textura dos rolos. Saída mais barata: forquilha nas costas e idle de mão em pala, olhando o mato.

**O segredo da q04 (leitura desta ficha; PROPOSTA).** A tarde na entrada é permitida e pública, e não é segredo. O que a Sera conta ("o Nilo anda indo sozinho pra beira do bosque", `dialogo.sera.o_pedido`) é o resto da tarde. Uns vinte passos a oeste do vão, depois da curva da barreira e fora da vista da entrada, há um buraco baixo no espinheiro, do tamanho de uma criança de gatinhas: "um lugar na beira do bosque que ninguém conhece" (`dialogo.nilo.chamando`). Lá não há adulto, e lá ele não fica na linha: entra de gatinhas até a cintura, empurrando a forquilha na frente. A rotina não muda, porque a âncora é a `entrada_bosque` e o buraco fica a poucos passos dela. A cena nunca mostra o buraco; a pista é a roupa (C7). Com a promessa quebrada, um adulto sabe, o buraco é tapado e a vila passa a vigiar o Nilo ("Agora todo mundo fica me olhando", `dialogo.nilo.promessa_quebrada`). Com a promessa cumprida, o buraco continua dele, com uma promessa a mais ("Um dia eu te levo lá", `dialogo.nilo.promessa_cumprida`). Nos dois casos, na manhã em que some, ele não sai pelo buraco: as pegadas que o Tovin acha estão na trilha que passa pelo vão (`dialogo.tovin.rastro_no_bosque`).

**C8 — Relação com o avatar por destino e origem.** *(PROPOSTA; só fala, nunca item nem poder — dossiê §H; `SLICE` R8. Itens e oportunidades conferem em `DestinyCatalog.cs`)*
- **serena** (`oportunidade.tarde_livre`): a criança tem a tarde livre, e a tarde de Nilo é a linha. Ele a trata como sócia, e o `chamando` vira "Hoje também?".
- **normal** (`oportunidade.recado_da_vila`): Nilo é a tentação no meio do recado — "O recado espera. O bosque, não." —, contra o "Tarefa dada é tarefa cumprida" de Daren (`dialogo.daren.tarefa_pendente`).
- **dificil** (Vida Árdua; `oportunidade.trabalho_cedo`): a criança já trabalha, e para Nilo isso é estar mais perto do Tovin do que ele. "Você já trabalha? Então você já pode passar da linha." É uma admiração perigosa: ele quer que ela atravesse junto.
- **ruptura** (`evento.sonho_recorrente`): pergunta o que ela sonha e acha que ela sabe o que há do outro lado. Aos 8, com o "círculo de luz" na lembrança, pergunta se o sonho dela também tem um. Não sabe a palavra Limiar (não tem `topico.limiar`).
- **origem `guardioes`** (`oportunidade.ronda_com_tovin`): a criança anda com o Tovin, que é quem ele copia. Nilo esconde a forquilha atrás das costas quando ela vem com ele, como quem foi pego imitando. Aos 8, quer ser guarda (fala já escrita) e pergunta como é a ronda. As outras origens não mudam nada.
Custo: 5 variantes de fala.

**C9 — Mudança dos 5 para os 8.** *(PROPOSTA)*
- **Antes (5–7):** 1,04 m, cabeça grande. Forquilha nas costas e, à tarde, na mão, cutucando além da linha; redemoinho; quatro voltas de rolo.
- **Depois (8):** 1,24 m, dentro da faixa de 8 anos (1,15–1,40, `PIPELINE.md` §3.1) e abaixo do jogador (1,28). As três formas continuam, e é isso que o faz ser reconhecido (GDD cap. 09; §5, G2 item 3). A **forquilha** voltou às costas no salto (C4) e não sai mais de lá: ele não cutuca mais nada, e ela fica onde o Tovin leva o chifre, só que do outro lado. Quem a trouxe de volta é cena da leva B. Na geometria de 8 (PROPOSTA: ponta de baixo em (+0,11; 0,42), cabo cruzando a linha dos ombros, a 1,00 m, em x = −0,12 m, ~22° da vertical; ombros de ±0,14 m; cabeça de 0,17 × 0,20 m com o topo a 1,24 m), as pontas ficam 0,05 e 0,12 m acima da cabeça, e a folga entre o cabo e a cabeça é de 5,8 cm. O **redemoinho** é o mesmo, porque a cabeça cresce pouco. Os **rolos** perderam uma volta: a perna mais comprida comeu a sobra da calça, e ficam três (Ø 0,15 m). À tarde ele fica do lado de cá da linha, olhando "de longe" (atividade que já existe), e de manhã olha o treino no posto.
- **Duas batidas:** a vara na mão, além da linha → a vara nas costas, parada; os pés na linha → do lado de cá, olhando de longe, e o posto de manhã.
- **Pede malha nova? Não.** O mesmo FBX de 5 anos com o preset de proporção de 8 (comprimento de osso e blend shape, nunca escala uniforme: `PIPELINE.md` §3, dossiê §J) e um blend shape a mais (os rolos, de quatro para três voltas). A forquilha é Prop à parte. Pendências de código: hoje o `NpcActor` escala o corpo inteiro (1,10 → 1,28), e o preset entra quando a malha real entrar; a vaga de tarde em `entrada_bosque` precisa ser autorada (na linha aos 5, alguns metros atrás aos 8), porque hoje é um ângulo fixo a 1,5 m da âncora (`NpcActor.RaioDaVaga`).

**C10 — Momento de cartaz.** *(PROPOSTA)* Tarde, entrada do bosque, câmera baixa atrás do jogador. Na linha das árvores está um menino menor que você, com um tufo de cabelo espetado para um lado e a calça enrolada em rolos grossos nas canelas. Ele tem a ponta dos pés exatamente na linha e a forquilha enfiada nas samambaias do outro lado. Vira só a cabeça: "Vem! Mas não pisa." O contracartaz, já na q07, é a trilha vazia com a vara cravada onde o Tovin para.

## 3. Amarração

- **Destino, Grau ou Trama:** é o único do elenco infantil que a Trama toca: atravessou e voltou com "um círculo de luz" na lembrança (`dialogo.nilo.aos_oito*`). Isso fica na fala, nunca no corpo: nem aos 8 ele leva signo da Trama ou cor reservada. O destino muda como ele vê a criança (C8): sócia de tarde na Serena, alguém mais perto do Tovin do que ele na Árdua, alguém que talvez saiba na Ruptura. Grau: fora do slice (dossiê §E).
- **Decisão de jogo:** seguir ou não o "Vem!" (opções que já existem) e, depois, guardar ou contar o segredo dele (q04). Ele some de qualquer jeito (ADR-0007 §3). A escolha não o salva: decide se o buraco continua dele e como ele volta (falas `promessa_*` e `aos_oito_*`). E a q03 só pode ser feita enquanto ele está lá (C5).
- **Pilar:** viver e crescer.

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| menino explorador de kit: túnica azul, lenço no pescoço, bolsa de lona, lupa, caderno (acervo antigo: COE-NPC-007; o kit é COE-PRP-006, de dono incerto) | menino que copia o único adulto que passa da linha, com o que acha: um galho no lugar do chifre, a calça enrolada no lugar dos canos |
| espada de madeira, estilingue ou cajado | forquilha de galho, sem elástico, nas costas e em diagonal: serve para alcançar, não para acertar |
| cabelo espetado de protagonista, ou cabelo em bola maior que a cabeça (é a forma da Maelis) | cabelo curto com um único redemoinho duro, em cunha, para um lado só |
| criança destemida que corre para dentro do perigo | criança que, na frente dos outros, não passa da linha, e tem um buraco no espinheiro que ninguém conhece |
| herói marcado pela Trama ao voltar: brilho, círculo, turquesa, dourado ou violeta (GDD cap. 09) | a marca é uma falta: até o salto, a vara fica onde o rastro acaba; o círculo existe só na fala |
| colete verde (o protótipo já tem três verdes, ficha `tovin` §4); azul e mostarda do acervo | calça terracota #A86D52 desbotada, com o avesso mais claro à mostra nos rolos (contrasta com o verde do bosque, onde ele passa a tarde); camisa de linho cru; galho castanho-acinzentado |

## 5. Avaliação — parecer do Art Director: 19/20, aprovado (ADR-0010)

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total /20 |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 1 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | **19** |

- **Corte:** ≥ 14/20 e nenhum zero em C2, C3, C4, C5 — **atingido** (HIPÓTESE do ADR, a recalibrar)
- **Pontuador / data:** Art Director (agente) / 2026-10-03. Não escreveu a ficha.
- **Veredito:** **aprovado por delegação (ADR-0010), com três condições antes de encomendar o concept:** (a) C3 reescrito — geometria da forquilha que feche e uma moita que não repita a forma 1 da Maelis (ver C3); (b) a ficha diz o que é o segredo da q04 na leitura dela (fato 2); (c) a frase da "regra de idade" em C2 é corrigida (fato 1).

**Nota por critério (1 linha cada):**
- **C1 = 2.** Detalhe concreto (calça de adulto em rolos, ponta dos pés na linha, a vara do outro lado) e conflito (o mais corajoso não passa, até o dia em que passa e só a vara fica). Duas frases.
- **C2 = 2.** A linha do bosque é condição de Auren, com três falas canônicas que a repetem e duas que confessam não saber o que há depois (Eira, Lysa). Ressalvas: a "regra de idade" está errada (fato 1), e, trocando os nomes, "vila à beira de floresta proibida" é o default do gênero; o que segura o 2 é a linha concreta e quem a repete.
- **C3 = 1.** Três formas nomeadas, mas: (1) a forquilha, como está escrita, não existe (fato 3) — conforme o ângulo que o concept escolher, ou as pontas saem bem para fora do contorno e a vara vira lança, ou o Y sobe colado à moita e as duas formas fundem em preto; (2) a moita é o mesmo recurso da forma 1 da Maelis (volume redondo mais largo que a cabeça), e a ficha afirma o contrário (fato 4). Na folha a escala separa, mas aos 8 a moita é a única forma forte que sobra (C9), e a ficha `maelis` põe os dois lado a lado na `entrada_bosque` toda tarde; (3) os rolos (+5 a 6 px por lado na folha) passam no limite. Para subir a 2: forquilha com ângulo e pontas coerentes e folga de ≥ 3 px entre o Y e a moita na folha; e uma moita que não seja um círculo (ou outra forma de cabeça).
- **C4 = 2.** Objeto com medida e regra escrita sobre um evento que já existe e já move a rotina dele; a vara cravada dá à q07 a pista física que ela não tinha. A melhor C4 das três que pontuei.
- **C5 = 2.** O cânone já o faz único (o único NPC que pode não estar em Auren), e "pescar além da linha" está especificado passo a passo. Condicional ao dono da q03 e à posição do cesto (fato 6).
- **C6 = 2.** Uma fala canônica e duas novas com o tique (repetir a última palavra: "Ainda."); combinam com o grafo `nilo_brincar`.
- **C7 = 2.** Observável toda tarde (pés na linha, vara do outro lado, passo atrás quando o jogador chega). Abre um buraco na q04 (fato 2).
- **C8 = 2.** Quatro destinos e a origem `guardioes`, com item, oportunidades e evento que conferem; a da Ruptura usa o "círculo de luz" canônico sem dar a ele a palavra Limiar.
- **C9 = 2.** Duas batidas (pés na linha → do lado de cá; roupa grande → quase do tamanho dele), com preset de 8 por osso e blend shape, não por escala. O risco está em C3: aos 8 sobra uma forma forte.
- **C10 = 2.** Cena escrita, um plano, uma fala curta. O contracartaz (a clareira vazia com a vara) é mais forte que o cartaz.

**Fatos dados como cânone que não conferiram:**
1. C2, "a regra é de idade (adulto passa, criança não)": Lysa, adulta, diz "Da entrada para dentro, nem eu vou sem o Tovin"; Tovin, "Depois dela, só comigo". A regra é "só com o Tovin", para todos. A cadeia "regra de idade → roupa de adulto" perde a base.
2. C7 contra a q04: o segredo canônico é "o Nilo anda indo sozinho pra beira do bosque" (`dialogo.sera.o_pedido`). A rotina já o põe na `entrada_bosque` toda tarde, com Lysa e Tovin na mesma âncora (`NpcCatalog.cs`), e o C7 acrescenta "até ali ele pode". Se é permitido e à vista de dois adultos, a promessa da q04 não guarda nada. A ficha precisa dizer o que é o segredo na leitura dela (a vara que passa da linha? um ponto da barreira longe do vão? uma hora sem adulto por perto?).
3. C3, forquilha: 1,10 m a 35° da vertical cobre 0,63 m na horizontal, mais que o dobro dos ombros de uma criança de 1,04 m. Não vai "por cima do ombro esquerdo" até "atrás da coxa direita"; para isso, o ângulo fica perto de 15°.
4. C3 e §7, "a única forma de cabeça no elenco que cresce para os lados": a nuvem grisalha da Maelis (0,32 m, o dobro da cabeça, forma 1 dela) também cresce para os lados. A "rodilha" citada em C3 já saiu da ficha da Mara.
5. §7, "lanterna de Eira (ombro direito, adulta)": a ficha `eira` a tirou de lá.
6. C5, "sem Nilo, ninguém alcança o outro lado": o vão de 6 m é aberto (B10; `AurenSceneBuilder.cs`), e o jogador passa por ele. Só vale se o cesto, ou a pista, estiver atrás da barreira sólida, fora do vão. Escrever isso.
7. §1, "kit de exploração (bolsa de lona, lupa, caderno)" do COE-NPC-007: no `ACERVO.csv` é COE-PRP-006, com dono incerto (Nilo ou Lysa).

Conferiram: id, traço, vínculo, tópicos, rotina com `Ausente` e o B14 [PROPOSTA] (`NpcCatalog.cs`); `AncoraAusente`; q03, q04 e q07 (`content/quests`); `EventoNiloDesapareceu` e o encerramento da q03 (`QuestCatalog.cs`, `GameSession.cs`, ADR-0007 §3); testemunhos (`NpcMemory.cs`); as pistas da q07 e as falas da linha (`strings.pt-BR.json`); grafo `nilo_brincar` com "Vamos!" e "Agora não dá." (`DialogueCatalog.cs`); destinos e origem (`DestinyCatalog.cs`); faixas 1,00–1,20 e 1,15–1,40, `r_cabeca` e Prop médio (`PIPELINE.md` §3, §3.1, §4); `NpcActor.RaioDaVaga` e `AjustarCorpo`; GDD cap. 09 (reconhecimento entre etapas); linha das árvores em z = 62 e clareira em z = 70.

**O sumiço de Nilo em cinco objetos — parecer de elenco (pedido no `ELENCO.md`).** São cinco, e mais: a segunda tira da Mara no vão, a folha de pé da Maelis (mais a rotina dela na `entrada_bosque` e a página assinada), a pergunta na lousa da Eira, o chifre calado do Tovin e esta forquilha. O gancho (C1) de cinco fichas do elenco depende do mesmo evento.
- **O que é a vila reagindo:** cada objeto é do dono e diz o jeito dele — cuidado, registro, pergunta, sinal, rastro. É o que o ADR-0002 pede de um C4.
- **O que é o mesmo truque:** o verbo é um só, "um objeto passa a marcar a ausência e fica até o salto", pelo mesmo componente de peça por evento. Na segunda vez o jogador entende o mecanismo e para de ler as pessoas. Três se amontoam em 10 m (a tira no vão, Tovin parando a 4 m da linha, a forquilha a 4 m do símbolo — e a Maelis na entrada à tarde, depois da q07). Dois dizem a mesma coisa, "pergunta escrita que não se fecha" (a folha da Maelis e o "?" da Eira), e os dois atravessam o salto (amarelada, desbotada). E leem três gatilhos diferentes (`evento.nilo_desapareceu`, a q07 em andamento, a abertura da q07): a vila não reage junta.
- **Veredito:** como está, é o mesmo truque cinco vezes, com duas variações reais: o chifre que não toca (outro sentido, ouvido de qualquer ponto de Auren) e a forquilha onde o rastro acaba (a pista, no lugar). Um terceiro se sustenta: a folha da Maelis, a única com fala canônica atrás (`aos_oito_registro`) e o registro que atravessa o salto.
- **Cortes recomendados (decide o coordenador):**
  1. **Sai a segunda tira da Mara**, como objeto. A reação dela fica na fala canônica `nilo_sumiu`, e o vão já tem a forquilha a 4 m, Tovin e Lysa. Custo: o C1 da Mara perde a segunda metade e precisa ser reescrito; o C4 dela sobrevive com a tira da criança.
  2. **Sai o estado "q07" da lousa da Eira.** A pista já está na fala `lugar_vazio`, e a pergunta escrita que não se fecha é da Maelis. O C1 da Eira sobrevive (é sobre o dia anterior ao sumiço) e o C4 fica com dois estados. Se o coordenador preferir manter a Eira, a troca é a inversa só aos 8: fica o "?" desbotado e sai a folha "amarelada" — um registro aberto por idade, não dois.
  3. **Os três que ficam leem o mesmo fato:** `evento.nilo_desapareceu` no histórico até `marco_idade_8` (a forquilha, para sempre). A vila reage na mesma manhã, e o chifre calado deixa de depender de o jogador descansar no meio da q07 (parecer da ficha `tovin`, fato 5).
  4. **Um lugar, não dois:** os "4 m depois da linha" em que Tovin para (ficha `tovin`, C5) e os "uns 4 m antes do símbolo" desta forquilha são o mesmo ponto. A escolta termina na vara.

**Para o G2 — o que o concept precisa provar:**
1. Folha a 30% com a forquilha na geometria corrigida: o Y separado da moita, e o teste de descrição sem "espada" nem "lança".
2. Nilo de 8 anos ao lado da Maelis, na `entrada_bosque` à tarde, de costas, na câmera do jogo: se os dois lerem como a mesma cabeça redonda, a moita muda.
3. Folha das crianças nas duas idades (Nilo, Sera, avatar): aos 8, sem forquilha e com uma volta de rolo, Nilo ainda é identificado.
4. Os rolos aguentam o clip Run sem osso secundário.
5. A forquilha cravada lê na tela do celular a partir do ponto em que Tovin para, e não lê como marco da Trama (nem círculo, nem turquesa, dourado ou violeta).
- **Condições cumpridas em 2026-10-03:**
  - **(a) C3 reescrito.**
    - **Forquilha:** refeita. Tem 1,00 m e vai a ~20° da vertical, da parte de trás da coxa direita (0,35 m) até o ombro esquerdo, junto ao pescoço. O Y fica ao lado da cabeça com pelo menos 4 cm (5 px) de folga, e nada sai embaixo (fato 3).
    - **Moita:** saiu (fato 4). No lugar entrou o **redemoinho**, um tufo em cunha para um lado só, do lado oposto ao da forquilha, com a cabeça inteira entre os dois.
    - **Rolos:** ficam, com a regra de ganhar uma volta a mais se não lerem.
    - "Rodilha" e "lanterna" saíram do texto (fatos 4 e 5).
  - **(b) O segredo da q04:** é um buraco no espinheiro, fora da vista da entrada, por onde ele entra sozinho. A tarde na linha continua permitida e pública. A pista que se vê é a roupa (C7 e o parágrafo "O segredo da q04").
  - **(c) C2:** a regra é "só com o Tovin", para todos (fato 1). A cadeia nova: a vila tem um único adulto que passa da linha, e o Nilo o copia. A roupa de adulto deixou de ser explicada por idade. A fala 2 de C6 trocou "coisa de grande" por "coisa do Tovin".
  - **Arbitragem 2 do `ELENCO.md`:**
    - **Gatilho:** a forquilha lê o gatilho único (`evento.nilo_desapareceu` até `marco_idade_8`) e volta às costas dele no salto.
    - **Lugar:** fica cravada onde a escolta do Tovin termina. É um lugar só, não dois.
    - **Alcance:** o alcance por cima da linha é só dele (C5).
    - **Citações velhas:** o que é da Maelis é a folha, e a "fita" saiu. Também saíram do texto a pergunta na lousa da Eira, a Maelis na entrada e as pedras da Lysa.
  - **Outros fatos:**
    - **C5:** o que se pesca está dentro do espinheiro, fora do vão, que é aberto (fato 6).
    - **Acervo:** o "kit de exploração" é COE-PRP-006, de dono incerto (fato 7).
    - **Amuleto:** a reação a ele saiu; o C8 da Ruptura fica só com o sonho (Arbitragem 2, item 8).
  - **Zonas:** atualizadas (§7). Ficam fora do Nilo as formas novas do avatar: trouxa nas costas, sino e tamancos. A porta de `casa_nilo` fecha à noite também durante o sumiço (§7).

**Conferência final (Art Director, 2026-10-03):** pendente, só na condição (a). O ângulo (20°) e as pontas da forquilha fecham, e o Y fica longe da cabeça, mas a folga do C3 ("pelo menos 4 cm (5 px) em toda a altura dela") não sai das coordenadas da própria ficha. Com o cabo de (+0,10; 0,35) a (−0,07; 0,81), Ø 0,035 m, e a cabeça de 0,16 m com o topo a 1,04 m, o cabo passa a ~1,5–2 cm da cabeça (~2 px a 30%) e funde com o lado dela na folha. Falta afastar o cabo (por exemplo, x ≈ −0,10 m na linha dos ombros, ~23° da vertical) ou corrigir o número no C3. (b), (c), os fatos 1–7 e a Arbitragem 2, itens 1, 2 e 8, estão atendidos.

**Correções de 2026-10-03 (W3), para o conferente:** (1) o cabo saiu de x = −0,07 m para x = −0,10 m na linha dos ombros (~23° da vertical, a sugestão da conferência), com a ponta de baixo no mesmo lugar; (2) a conta da folga está no C3 com as coordenadas da própria ficha: 4,5 cm no ponto mais perto (cabeça como elipse de 0,16 × 0,19 m; 5,4 cm como círculo), 5,5 px na folha; (3) mudaram o ponto da forquilha (1,08 m), as pontas (1,28 m e 1,20 m) e a altura acima da cabeça (0,24 m, 29 px); a caixa do `silhueta.py` passou de 1,30 m para 1,28 m (C3, §6, `ELENCO.md`, `PROMPTS_G2.md`); (4) aos 8, a geometria foi declarada e conferida (folga de 5,8 cm; pontas 0,05 e 0,12 m acima da cabeça); (5) C4: a forquilha em z ≈ 66 fica dentro da clareira (z 62–78), antes da âncora dela, a ~6 m do símbolo, como a ficha `tovin` passou a dizer.

**Reconferência (Art Director, 2026-10-03, leva C1):** condição (a) atendida. O G1 fica aprovado por delegação, sem pendência. Refiz a geometria com as coordenadas do C3. O cabo vai de (+0,10; 0,35) a (−0,10; 0,81), a 23,5° da vertical. A forquilha abre em (−0,22; 1,08), e as pontas ficam em (−0,20; 1,28) e (−0,38; 1,20). Com a cabeça como elipse de 0,16 × 0,19 m e centro em 0,945 m, a menor distância do eixo à cabeça é de 6,3 cm. A folga é de **4,5 cm** (5,4 px na folha), ou 5,4 cm com a cabeça como círculo, e os braços do Y ficam a 15,7 cm da cabeça. Aos 8: 21,6°, folga de 5,8 cm, pontas 0,05 e 0,12 m acima da cabeça. Na máscara (caixa de 1,28 m, figura com 80% da altura), a folga dá 14 px, e o preenchimento do fundo precisa de 5 px para entrar. O cabo (Ø 3,5 cm) dá 11 px contra o corte de 7 px. Ressalvas de texto, sem pendência:
- a borda de fora do cabo fica a ~0,1 cm por dentro da ponta do ombro (x = −0,119 m), e não a 1 cm. A folga não muda;
- aos 8, a caixa vai até a ponta do Y, a 1,36 m. Na folha das duas idades, o Nilo de 8 entra com `@1,36`, e não com 1,24 (Arbitragem 2, item 6).

## 6. Encaminhamento

**Para o G2, só depois de aprovado:**
- vistas: frente, perfil, costas e 3/4 em T-pose, fundo neutro, linha de chão, 1,04 m marcado (h = 1,11 com o tufo; caixa de 1,28 m com a forquilha); a mesma prancha no preset de 8 anos (1,24 m; caixa de 1,36 m com a forquilha, `@1,36` no teste), com a forquilha nas costas
- silhueta: forquilha, redemoinho e rolos em preto a 30% (`silhueta.py`, 120 px/m), embaralhados com Sera, o avatar, Tovin e Maelis (os vizinhos de zona); a folha das crianças nas duas idades (§5, G2 item 3); um render na câmera do jogo, de costas, e um na `entrada_bosque` à tarde, com Lysa e Tovin
- o concept tem de provar (§5, "Para o G2"):
  - a forquilha na geometria de C3 (cabo em x = −0,10 m na linha dos ombros, ~23°), com ≥ 4,5 cm (5 px) entre o cabo e a cabeça em toda a altura dela, e o teste de descrição sem "espada" nem "lança";
  - o redemoinho não lê como chifre nem como a nuvem da Maelis;
  - aos 8, Nilo é identificado pelas três formas;
  - os rolos aguentam o clip Run sem osso secundário e não se confundem com os tamancos do avatar, que ficam embaixo, no pé;
  - a forquilha cravada lê na tela do celular a partir de onde o Tovin para e da boca do vão (~4–6 m), e não lê como marco da Trama
- paleta: terracota #A86D52 desbotada na calça (valor mais claro que o couro de Borin), linho cru, galho neutro; o cabelo fica com o concept. Reservados e proibidos: turquesa #86C8C9, dourado #D6B36A, violeta #9777B8
- objeto à parte: forquilha de 1,00 m (cabo de 0,80, braços de 0,20 m), Ø 0,035 m; nas costas e cravada (0,85 m de fora)
- orçamento (`PIPELINE.md` §4, HIPÓTESE v0): Npc com 8 000 tris, 2 materiais, textura 1024 e 55 ossos sem osso secundário, portanto tufo rígido na cabeça e rolos rígidos nas canelas; `forquilha_nilo` como Prop médio (2 000 tris, LOD1, 1 material, 512 px)
- validador: V09 mede h = 1,11 (faixa 1,00–1,20); V13 dá r_cabeca ≈ 0,27. O preset de 8 anos não tem regra hoje (o §3.1 só fala em "modelar quando a ficha pedir"); esta ficha pede preset, não modelo novo
- testes a registrar: silhueta (≥ 4/5), descrição, troco com Sera na q04 (trocados, quem tem o segredo vira quem o conta: a cena tem de quebrar)
- custo total: 2 clips próprios (idle de tarde na linha; enganchar e puxar), textura dos rolos com espinho, 7 falas novas (2 de C6, 5 de C8), 1 Prop, 1 blend shape, o componente de peça por evento, a vaga autorada em `entrada_bosque` e o preset de proporção de 8 no `NpcActor`
- dependência: o conteúdo da q03 (o que fica preso no espinheiro em `procurar_na_trilha`) e o do segredo da q04 (o buraco no espinheiro) são do dono de narrativa/quest

**Para o G3:** blocos `### nilo` e `### forquilha_nilo` em `PROVENIENCIA.md` com `g1:` (data, nota, pontuador), `g2:` (data, silhueta n/5, descrição, troco), `entrada:` (concept e SHA-256), `ferramenta`, `plano`, `licenca`, `termos_url`, `g3:`. `referencias_terceiros`: nenhuma usada nesta ficha.

## 7. Ligações com outras fichas

PROPOSTAS desta ficha que envolvem outro personagem:
- **Tovin:**
  - Nilo o copia no espelho. A forquilha nas costas, saindo acima do ombro esquerdo, é o chifre dele, que sai acima do direito. Os rolos na canela são os canos dele, ao contrário. Aos 8, a cópia vira o "quero ser guarda" que já está escrito.
  - A forquilha fica cravada onde a escolta dele termina (ficha `tovin`, C5): a escolta termina na vara. Uma fala do Tovin apontando a vara é opcional, para o redator.
- **Tovin e o segredo da q04:** com a promessa quebrada, quem tapa o buraco no espinheiro é um adulto. Pela ficha do Tovin (o vão e o espinheiro são dele), o natural é que seja ele. A cena e a fala ficam com o dono da q04.
- **Oren e a q03:** em `procurar_na_trilha`, o que está preso no espinheiro volta pela forquilha (C5). Esta ficha não fixa onde está o cesto em cada variante.
- **Lysa:** colhe ajoelhada a dois passos do lado de fora da linha, de costas para o bosque, e não alcança o outro lado (ficha `lysa`, C7; Arbitragem 2, item 2). O Nilo cutuca a linha atrás dela, sem ser visto. As pedras dela aos 8 foram cortadas e esta ficha não as cita.
- **Mara:** a tira dela, no tronco leste do vão, marca a linha em que o Nilo pisa à tarde. A tira do Nilo (do sumiço ao salto) e a forquilha leem o mesmo gatilho e ficam em lugares diferentes: a tira no vão, a vara onde o rastro acaba.
- **Mara e as portas:** a ficha dela fecha à noite as portas de `casa_nilo` e `casa_sera`. Esta ficha **não** pede a porta do Nilo aberta durante o sumiço. Os marcos do sumiço são os quatro da Arbitragem 2: uma porta aberta seria o quinto e ainda tiraria da Mara a única porta aberta de Auren. O "a cama dele amanheceu vazia" (Maelis) não pede porta aberta.
- **Maelis:** a folha de pé na prancha dela é o único marco que passa do salto, enquanto a forquilha volta às costas do Nilo no salto. A Maelis saiu da `entrada_bosque` (Arbitragem 2, item 2) e esta ficha não a põe lá.
- **Daren:** a variante de C8 normal o opõe ao "tarefa dada é tarefa cumprida" (fala já escrita). Nada muda no Daren.
- **Sera, Borin, Oren e avatar:** o componente de "peça por evento do histórico" é o mesmo da forquilha, da tabuinha da Sera, da prova no aro do Borin, do cestinho do Oren e da manta do avatar.
- **Avatar:** esta ficha não usa nenhuma forma do avatar (trouxa de manta horizontal nas costas, na altura das omoplatas, com as pontas fora dos braços; manta em sino até a dobra do joelho; tamancos de sola alta). A diagonal da forquilha cruza as costas por dentro do contorno e só sai acima do ombro. Os rolos ficam na canela, entre a barra do sino e o tamanco do avatar, justamente onde ele é fino. Em troca, a aparência do avatar, quando voltar ao jogo (ADR-0007 §5), não deve usar tufo em cunha para um lado, calça enrolada em rolos nas canelas nem objeto longo em diagonal nas costas.

Zonas de silhueta que esta ficha ocupa:
- **haste em Y na diagonal das costas, saindo por cima do ombro esquerdo**, em escala de criança. É o espelho do chifre do Tovin (ombro direito); separam a escala e a forma (Y × cone).
- **um lado do topo da cabeça:** tufo em cunha para cima e para a direita. A nuvem redonda da Maelis ocupa os dois lados, e o coque da Sera, o centro.
- **canelas:** rolos que deixam a perna mais larga embaixo, entre a barra do sino do avatar e os tamancos dele, que ficam no pé.
- **estatura:** a menor criança do elenco (1,04 m aos 5, 1,24 m aos 8), só como apoio (Arbitragem 2, item 6).
- **não usa:** trouxa horizontal nas costas, sino até o joelho nem tamancos (avatar); cabelo em volume redondo (Maelis); rolo nos ombros (Mara); nada pendurado fora do quadril ou da coxa (Borin).
