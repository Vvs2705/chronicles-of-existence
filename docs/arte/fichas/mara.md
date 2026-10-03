# Ficha G1 — `mara`

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md)). Par com [`daren`](daren.md).
> Fato com fonte é cânone. **PROPOSTA** = ideia desta ficha, não está nos docs e não vale até ser aprovada. As PROPOSTAS do [`borin`](borin.md) citadas aqui valem como canon de trabalho (aprovadas por delegação em 2026-10-03).

## 1. Identificação

- **id:** `mara` (`NpcCatalog.cs`; GDD cap. 06, NPC-01). O nome é provisório (GDD cap. 02); o id é estável, e o nome pode virar dado por origem (`INCONSISTENCIAS.md` A5, recomendação ainda não decidida).
- **categoria:** Npc, base adulta (`PIPELINE.md` §3.1)
- **onde aparece no slice:**
  - B03: o texto da família muda com a origem (`SLICE` B03).
  - B06/q01, objetivo `falar_com_familia` (`q01…json`). O jogador nasce de frente para a porta de `casa_familia`, com Mara e Daren "à frente, a ~6 m" (`AurenSceneBuilder.cs`, `RotacaoDoSpawn`). Os dois dividem a âncora; o `NpcActor` põe cada NPC a 1,5 m dela (`RaioDaVaga`).
  - B09: fala `nilo_sumiu` enquanto a q07 está em andamento. B14: fala `aos_oito` (`DialogueCatalog.cs`).
  - Âncoras: `casa_familia` de manhã e à noite, `praca_centro` à tarde (`NpcCatalog.cs`).
- **cânone de partida:**
  - "responsável familiar, protetora; profissão varia com origem" (dossiê §G). Papel adaptável: profissão, recursos, diálogos e objetos da casa mudam com origem e destino, sem supor que um arquétipo seja rico ou melhor (GDD cap. 02). "A definição estética e os nomes dos responsáveis podem variar segundo a opção" (dossiê §C).
  - **Malha única para as 3 origens é PROPOSTA desta ficha.** O dossiê §C e o GDD cap. 02 deixam estética, nome e profissão variarem com a origem. A escolha aqui é de custo (`PIPELINE.md` §4): a origem entra por objeto da casa e fala, não por corpo.
  - traço `protetora`; rotina: cuida da casa (manhã), compra na praça o que falta para o dia (tarde), em casa (noite); vínculos: companheira de Daren, freguesa de Oren; sabe `topico.familia`, `topico.vila_auren` e `topico.oficio_da_familia`, mas **não** `topico.limiar` (`NpcCatalog.cs`). Papel exibido: "Responsável pela família e pela casa" (`strings.pt-BR.json`).
  - falas já escritas (`dialogo.mara.*`): "Acordou, meu bem? Vem cá, deixa eu ver esse rosto. Hoje você já pode ir até a rua, mas fala com o Daren antes de sair."; "a entrada do bosque é o seu limite. Combinado?"; "fica sempre perto de gente grande. Promete?"; "De noite a gente fica perto de casa."; "O Daren cuida do ofício, eu cuido do resto"; aos 8: "você já alcança a prateleira de cima". As falas são neutras quanto à origem (`DialogueCatalog.cs`, comentário de `Mara()`).
  - testemunha só `evento.q01_concluida`, `evento.q01_familia_apresentada` e `marco_idade_8` (`NpcMemory.cs`).
  - todo destino começa com uma manta: `item.manta_boa` (serena), `item.manta_simples` (normal e ruptura), `item.manta_remendada` (dificil) (`DestinyCatalog.cs`).
  - a entrada do bosque é um vão de 6 m na linha de árvores, a ~105 m da porta de casa; a clareira do símbolo fica 10 m depois (`AurenSceneBuilder.cs`: `casa_familia` (−22, −43), `entrada_bosque` (0, 60), `bosque_clareira` (0, 70)). As três casas acessíveis são montadas com a folha da porta aberta, encostada na fachada (`CasaAcessivel`). A porta mede 1,8 × 2,2 m (`VaoPorta`, `AlturaPorta`).
  - já existe um protótipo do Tripo, marcado PROTOTIPO (ADR-0008): "vestido sálvia, avental creme; o Tripo avisou que o vestido longo dificulta o rig" (`PROVENIENCIA.md`). Existe também concept antigo: macacão marfim de perna larga, descalça, trança longa (`ACERVO.csv` #106, #112). Os dois são o default (§4).
- **autor da ficha / data:** Concept Art Lead (agente), 2026-10-03. Reescrita no mesmo dia pelas condições do parecer (§5) e pela Arbitragem 2 do [`ELENCO.md`](ELENCO.md).
- **estado:** aprovado G1 por delegação (ADR-0010, 2026-10-03), condições cumpridas no corpo (lista no fim da §5)

## 2. Critérios C1–C10

**C1 — Gancho.** Mara amarra no vão do bosque uma tira do pano da casa para marcar até onde a criança pode ir, e a casa dela é a única de Auren de porta aberta à noite; quando o Nilo some do lado de lá, ela amarra uma segunda tira, para um menino que não é dela. *(PROPOSTA)*

**C2 — Necessidade do mundo.** Cânone:
- a casa fica a ~105 m de um vão de 6 m, e nada na cena o fecha (`AurenSceneBuilder.cs`). A ficha do Tovin diz que é o podão dele que mantém o vão aberto contra o espinheiro (PROPOSTA de lá).
- a vila inteira repete a mesma regra: "a entrada do bosque é o seu limite" (Mara); "Até a entrada, pode. Depois dela, só comigo." (Tovin); "Da entrada para dentro, nem eu vou sem o Tovin." (Lysa) (`strings.pt-BR.json`).
- no slice, uma criança some ali de verdade (ADR-0005; ADR-0007 §3).
- dez metros depois do vão está o símbolo que a criança viu no Limiar antes de nascer (`SLICE` B11; dossiê §I), e Mara não sabe disso.

**PROPOSTA:** o Tovin avisa a vila, com o chifre (ficha `tovin`). O limite de uma criança, ninguém marca, a não ser a casa dela. "Protetora" (dossiê §G) é como responde quem cria uma criança de cinco anos a 105 m de um vão que não fecha: Mara marca no caminho, com pano, o ponto que a voz dela já não alcança. O que ela faz com a própria porta fica sem explicação (C7).

**C3 — Silhueta em 3 formas.** *(PROPOSTA; reescrita em 2026-10-03 pelas duas arbitragens do [`ELENCO.md`](ELENCO.md): a rodilha saiu, porque a faixa sobre a cabeça é da Lysa; a estatura saiu, porque a mais alta do elenco é a Eira, com 1,92 m)*
Nada em cima da cabeça. As três formas são do mesmo pano terracota: nos ombros, nos cotovelos e na barra. A câmera fica atrás da criança (pivô 0,94 m, distância 2,75 m, inclinação inicial de 15°: altura ≈ 1,65 m, conta desta ficha sobre `ThirdPersonCamera.cs` e `BodyByAge.cs`). As medidas em px usam a régua de 30% do `client/tools/silhueta.py` (120 px/m). Altura: 1,80 m, que é apoio e não forma (Arbitragem 2.6).
1. **Colar de manta:** a manta da casa (C4), dobrada num rolo de 0,12 m de diâmetro, vai sobre os dois ombros, cruza no peito e tem as pontas presas na faixa. O pescoço some, e os ombros ficam redondos e ~0,08 m mais largos de cada lado (+10 px). É o oposto do pescoço longo e projetado do Borin e do pescoço à vista da Eira. A zona é dela pela Arbitragem 1.3.
   - Sai do contorno: sim.
   - T-pose: sim, se o rolo pesar na clavícula e no `UpperChest` e não no braço, para não rasgar na axila. Sem osso secundário (`PIPELINE.md` §4), é malha rígida.
   - Câmera: o ombro dela (~1,47 m) fica quase na linha da câmera, e o rolo lê de frente, de costas e de perfil.
2. **Mangas de manta:** mangas curtas da mesma lã, em sino do ombro ao cotovelo. A boca, no cotovelo, tem 0,24 m (29 px), sobre um antebraço nu de 0,08 m (10 px). Em T-pose, de frente, cada braço engrossa até o meio e afina de repente: um degrau no cotovelo, igual nos dois lados.
   - A zona é dela pela Arbitragem 2.4. O braço inteiro e grosso é do Borin, e as luvas de cano largo no pulso são da Lysa.
   - Rígidas no `UpperArm`, com a borda pesando pouco no `LowerArm`. T-pose: sim.
   - Câmera: de braços caídos, os sinos ficam ao lado da cintura (~1,10 m), na altura dos olhos da criança.
3. **Barra arrancada:** saia do mesmo pano, enrolada na cintura por cima da calça, com a barra em diagonal: vai até o meio da canela do lado direito (~0,35 m do chão) e só até o meio da coxa do lado esquerdo (~0,70 m).
   - Em preto, de frente, é um corte inclinado de 0,35 m (42 px) que atravessa as duas pernas. A perna direita some na saia até a canela; a esquerda aparece inteira da coxa para baixo.
   - É a única barra inclinada do elenco. O avental do Borin e o tabardo da Maelis fecham o vão das pernas em linha reta, e são apoio, não forma. A saia não é mais larga que o quadril, então não ocupa o lado de fora da coxa (Arbitragem 2.3). Também não é o trapézio do sino do avatar, que abre além das pernas.
   - É do C4: as tiras saem desta barra, sempre do lado esquerdo. A diagonal é o pano que já virou tira.
   - Rígida no `Hips`, com o lado longo pesado na perna direita. T-pose: sim. No andar, o lado esquerdo curto libera o passo, e o G2 olha o clipe.
   - Câmera: a barra fica abaixo dos olhos da criança e é vista de cima. A 6 m, lê como um traço inclinado na metade de baixo da figura.

Validador: sem nada acima da cabeça, h = 1,80 m, dentro de V09 e V13 (`PIPELINE.md` §11). Apoio, que não conta como forma: pano liso na cabeça, rente ao crânio e sem volume (a cabeça descoberta é marca da Maelis); o nó do rolo no peito; o sapato de tira (`ACERVO.csv` #122, vale manter).

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **A manta da casa e as tiras.** A manta de Mara mede 2,0 × 1,4 m aberta; vestida, é o colar de C3. É o pano de onde saiu a manta inicial da criança: mesmo pano e mesmo estado do destino (inteira com barra tecida, lisa ou remendada, `DestinyCatalog.cs`). Do mesmo pano é a saia de C3, e é da barra dela que Mara arranca as **tiras**, de 0,60 × 0,08 m. Ela as amarra a 1,0 m do chão (a altura dos olhos de uma criança de 1,10 m) no tronco do lado leste do vão de `entrada_bosque`.
Regra:
1. A **tira da criança** fica no tronco do começo do jogo até `marco_idade_8` entrar no histórico.
2. A **tira do Nilo**, do mesmo pano, aparece com `evento.nilo_desapareceu` e fica até `marco_idade_8`. É o gatilho único dos marcos do sumiço (Arbitragem 2.1). Não depende do estado da q07, e por isso não some quando a q07 conclui.
3. No salto, as duas saem do vão e aparecem dobradas na prateleira de cima de `casa_familia`, a mesma da fala `aos_oito`.
4. A tira não barra nada, não grava evento e não muda nada se for cruzada. É limite de palavra, não de parede.
O jogo lê `evento.nilo_desapareceu` e `marco_idade_8` no histórico e o destino do `BirthChoice`, tudo já no save. Nada é concedido. Custo: o componente de "peça ligada por evento" (pendência do `ELENCO.md`) e as texturas de estado (§6).

**C5 — Regra exclusiva.** *(PROPOSTA)* **A palavra no pano.** Mara é a única de Auren que fala com a criança sem estar lá. Hoje toda conversa de Auren sai de um NPC em cena (cada grafo do `DialogueCatalog` tem um NPC dono, e o painel abre na conversa com ele). A tira é o único lugar onde a fala dela aparece longe dela, e só no limite. Como funciona:
1. A tira (C4) é um interagível no tronco do vão, do mesmo tipo do `Descanso` (`Interactable`).
2. Tocar nela abre o painel de diálogo com o nome de Mara e sem Mara em cena. A fala é lembrança, não chamada.
3. A fala segue o estado do C4:
   - aos 5–7: "Até aqui, meu bem. Daqui eu não te vejo, então vale a palavra. Combinado?", com duas respostas: "Combinado." e "Hoje não.";
   - durante o sumiço, a tira do Nilo responde "Por aqui ele volta." (C6, fala 2);
   - aos 8, o vão não tem tira; na prateleira de casa, o mesmo toque devolve a fala `aos_oito`, que é canônica.
4. Nenhuma resposta grava evento nem muda estado (`SLICE` R8). Cruzar o vão depois do "Combinado." não tem punição nem testemunha. A palavra fica só com a criança, como a tira, que não barra.

Não lê inventário (a leitura do que a criança carrega é do Oren, Arbitragem 2.9) e não toca em `em_casa`. Custo: o interagível; um grafo de 3 nós que lê o mesmo estado do C4; abrir o painel sem `NpcActor` (código pequeno); 2 falas novas.

**C6 — Voz.** *(PROPOSTA; o texto final é do redator)* Ela diz "meu bem". Quando o limite é **longe** e ela não vai ver, pede acordo ("Combinado?" em `em_casa`, sobre o bosque; "Promete?" em `nilo_sumiu`, sobre a busca). Quando é **perto** e ela vê, só diz a regra ("De noite a gente fica perto de casa.", `noite`; "fala com o Daren antes de sair", `primeiro_dia`). Longe ela pede palavra; perto ela manda. Essa é a leitura que esta ficha faz das falas canônicas. O Tovin confere com "Entendeu?"; ela pede acordo.
1. "Aquela tira na entrada do bosque é nossa. Não é cerca, é lembrete. Até ela, meu bem. Combinado?" (cabe em `dialogo.mara.em_casa`: limite longe, pede acordo)
2. "Amarrei outra tira lá. A sua diz até onde você vai. A dele diz por onde ele volta." (segunda fala de `nilo_sumiu`; não é regra para a criança, então não pede nada)
3. "Até aqui, meu bem. Daqui eu não te vejo, então vale a palavra. Combinado?" (a tira, C5)

**C7 — Contradição visível.** *(PROPOSTA)* **Cerca o bosque e não fecha a porta.** Ela põe o limite mais longe de casa de toda Auren (uma tira a 105 m), e a casa dela é a única de Auren de porta aberta à noite. Dá para ver em três ações recorrentes:
1. A tira está no vão todo dia (C4).
2. **PROPOSTA de cena:** à noite, a folha da porta de `casa_nilo` e de `casa_sera` fecha, e a de `casa_familia` fica aberta. Hoje as três ficam abertas o tempo todo (`CasaAcessivel`). O custo é o componente de peça por estado (o mesmo do C4) na folha da porta, lendo o período.
3. Ela não explica: nenhuma fala dela diz por que a porta fica aberta, e o C2 não diz também.
Não pede posição nova, vaga autorada nem clipe: a cena mostra a porta, e Mara fica onde o `NpcActor` já a põe. É o inverso do Daren (C7 dele): ela prende os pés no vão e deixa a casa aberta.

**C8 — Relação com o avatar por destino e origem.** *(PROPOSTA; fala e objeto, nunca poder, dossiê §H)* A fala é escolhida pelo **destino**, não pelo item. O diálogo hoje não lê destino, e isso é a pendência "condição de diálogo por destino" do `ELENCO.md`. Itens, oportunidades e eventos conferem em `DestinyCatalog.cs`.
- **serena:** a manta é inteira, com barra tecida. Com `oportunidade.tarde_livre`: "Tarde livre é pra voltar com uma história. Uma só, que eu escuto inteira. Combinado?" Nesta vida, ela é quem escuta.
- **normal:** a manta é lisa. Com `oportunidade.feira_de_auren`, à tarde, na praça, ela é freguesa de Oren e a criança é a ajuda: "Me ajuda a levar? Você leva o leve, eu levo o resto. Combinado?"
- **dificil** (rótulo "Vida Árdua", ADR-0007): a manta é remendada, e a tira do bosque é um remendo, porque já não sobra pano inteiro para arrancar. Com `evento.ano_de_escassez`: "O pano é remendo, mas o combinado é inteiro. Até a tira, meu bem. Combinado?"
- **ruptura:** a manta é lisa, e a tira leva nó duplo. Num destino em que "a vila repara em você" (`destino.ruptura.contexto_social`): "Tem gente que olha muito pra você, meu bem. Deixa olhar. Você olha pra tira. Combinado?" A tira é a única coisa de Auren que diz à criança onde parar, e não o que ela é. Mara não reage ao amuleto (Arbitragem 2.8).
- **origem:** não muda malha, silhueta nem fala. Muda o objeto da casa que fica na mesa de `casa_familia` (GDD cap. 02: "objetos da casa mudam"): `agricultores`, saco de sal e sementes; `artesaos`, rolo de pano e fio; `guardioes`, jarra de óleo de lanterna.
  - **Para o produto, não para a arte:** se Mara tem ofício próprio, esta ficha não decide. O dossiê §G diz que a profissão dela varia com a origem, e a fala canônica `sobre_familia` dá o ofício ao Daren. Precisa de proposta em `docs/`. Para a arte, nada muda: o ofício dela, se houver, entra como objeto da casa e fala.

**C9 — Mudança dos 5 para os 8.** *(PROPOSTA)*
- **Antes (5–7):** a tira da criança está no vão, e a do Nilo do sumiço ao salto. À noite, a porta de `casa_familia` é a única aberta de Auren.
- **Depois (8):** o vão está sem tiras, e as duas estão dobradas na prateleira de cima. A fala `aos_oito` ("você já alcança a prateleira de cima") passa a apontar para elas sem mudar uma palavra. À noite, pela primeira vez, a porta de `casa_familia` fecha como as outras, e o resto da fala canônica ("Às vezes ainda acordo achando que vou te encontrar na porta") passa a ser dito com a porta fechada. O limite sai do bosque e passa a ser o do Tovin ("Depois dela, só comigo").
- **Variante de malha:** não. Mudam prop (as tiras) e o estado da porta (dossiê §G).

**C10 — Momento de cartaz.** *(PROPOSTA)* De manhã (o vão está vazio: Lysa, Tovin e Nilo só chegam à tarde, `NpcCatalog.cs`), com a câmera do jogo atrás da criança, que chega ao vão de 6 m. No tronco do lado leste, na altura da lente (a tira a 1,0 m e o pivô da câmera a 0,94 m), há uma tira terracota contra o verde escuro do bosque. A criança toca a tira, e o painel abre com o nome de Mara, que está a 105 m: "Até aqui, meu bem. Daqui eu não te vejo, então vale a palavra. Combinado?" Dura 5 s, sem clipe novo nem vaga autorada. É o quadro do C1, o que o parecer pediu.

## 3. Amarração

- **Destino, Grau ou Trama:** o destino muda o pano (estado da manta, da saia e da tira) e a fala (C8). A Trama passa a 10 m da tira: o limite que ela amarra fica antes do símbolo do Limiar, sem que ela saiba. O Grau não se aplica no slice (dossiê §E).
- **Decisão de jogo:** cruzar ou não o vão aos 5 anos, depois de responder à tira "Combinado." ou "Hoje não." (C5). Nada impede, nada pune, nada grava: a palavra é só da criança. *(PROPOSTA)*
- **Pilar:** viver e crescer.

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| a "mãe da vila" de vestido longo, avental e trança. É o protótipo do Tripo (vestido sálvia, avental creme, com aviso de rig, `PROVENIENCIA.md`) e o concept antigo (trança longa, `ACERVO.csv` #112) | túnica, calça e uma saia enrolada e torta por cima, mais curta de um lado porque é dali que sai a tira; o volume fica nos ombros e nos cotovelos (rolo e mangas de manta); nenhuma saia longa, nenhum vestido em triângulo |
| o corpo de referência de macacão marfim de perna larga, descalça, que serve a mais de um NPC (`ACERVO.csv` #106; o mesmo corpo de Maelis, COE-NPC-016) | corpo próprio: sem pescoço à vista, com a barra em diagonal sobre pernas retas, calçada |
| proteção como muro: porta fechada, criança dentro | a única casa de Auren de porta aberta à noite; o limite fica longe, no vão do bosque, e é de pano |
| mãe só doce, que acolhe e não negocia | "meu bem" e acordo para o limite longe; regra seca para o perto |
| intuição mística de mãe sobre o destino da criança | não sabe do Limiar (`NpcCatalog.cs`) e não fala do amuleto |
| mangas bufantes de vestido, de "mãe de conto", em cor pastel | manga curta de lã da manta, em sino até o cotovelo, com o antebraço nu de quem trabalha; terracota #A86D52 ("Auren", GDD cap. 09), nenhuma cor reservada (turquesa, dourado, violeta) |

## 5. Avaliação — parecer do Art Director: 16/20, aprovado (ADR-0010)

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total /20 |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 1 | 2 | 1 | 2 | 1 | 2 | 2 | 1 | **16** |

- **Corte:** ≥ 14/20 e nenhum zero em C2, C3, C4, C5 — **atingido** (HIPÓTESE do ADR, a recalibrar)
- **Pontuador / data:** Art Director (agente) / 2026-10-03. Não escreveu a ficha.
- **Veredito:** **aprovado por delegação (ADR-0010), com três condições antes de encomendar o concept:** (a) C3 ganha uma terceira forma que não seja estatura; (b) C5 deixa de disputar com o Oren a leitura do que a criança carrega e deixa de apagar o `em_casa`; (c) os fatos abaixo são corrigidos no corpo. C7 e C10 ficam em 1 sem bloquear; reescrever os dois é recomendado.

**Nota por critério (1 linha cada):**
- **C1 = 2.** Objeto concreto (tira cortada da manta, no tronco do vão) e conflito duplo (porta aberta × limite longe; a segunda tira para um menino que não é dela). Uma frase.
- **C2 = 2.** Ancorada em fatos conferidos (vão de 6 m a ~105 m da porta, a regra repetida por três NPCs, Nilo some ali, símbolo 10–12 m depois), e o traço do catálogo sai dela. Ressalvas: "vila ao lado de floresta perigosa" é a parte que serve a outro jogo; e "o aviso que o bosque não dá" é a premissa do Tovin, cujo C2 também nasce do "não avisa" e faz do chifre o aviso. Separar: Tovin avisa a vila, Mara marca o limite de uma criança.
- **C3 = 1.** Rolo nos ombros (o pescoço some) e sino no cotovelo leem a 30% em T-pose. A coluna não: Eira tem 1,92 m e é "a mais alta do elenco" (ficha `eira`, e a linha dela no `ELENCO.md`); Borin, com 1,82 m, fica a 2 px na folha (218 × 216); o funil é a soma das duas primeiras formas, não uma terceira. Colisão clara com a estatura da Eira, então no máximo 1.
- **C4 = 2.** Objeto com medida e regra escrita com ids (q07 → tira do Nilo; `marco_idade_8` → tiras na prateleira; destino → estado do pano), tudo já no save. Como no Borin, é exibição de estado, aceita pela régua. A regra 2 precisa dizer "q07 iniciada ou concluída, sem `marco_idade_8`", senão a tira some quando a q07 conclui.
- **C5 = 1.** Especificada, mas não exclusiva. A ficha `oren` reivindica a mesma coisa ("o único NPC cujo diálogo lê o inventário"), e o Borin já reage ao item que a criança traz (C8 dele). No troco, Oren diz "Moeda nova… de onde veio?" sem reescrita; Mara não diz a fala de preço dele. Pela regra de desempate do `ELENCO.md` (fica quem tem a C2 mais ligada), a leitura do que a criança carrega fica com o Oren: deduzir pelo que se carrega já é fala canônica dele (`viu_nilo`). E, como está escrita, a regra se sabota: `recompensasAplicadas` guarda também os itens de nascimento (`rec.nascimento.*`, `Inventario.Nascer`), e quem joga só a linha central fica com as moedas da q02 como "última recompensa" até os 8 anos. Aí o `em_casa` some depois da q02, e com ele o "Combinado?" do limite, que é a fala 1 desta ficha. Para subir: uma técnica que só ela tem e que nasça do C2/C4 (o vão, a tira).
- **C6 = 2.** Três falas escritas, com o tique canônico ("meu bem" + pedido de acordo). Vizinho: o "Entendeu?" do Tovin, também canônico e também sobre o bosque. O redator mantém Mara pedindo acordo e Tovin conferindo se a criança entendeu.
- **C7 = 1.** As três ações estão escritas, mas a metade visível da contradição não é dela. O `AurenSceneBuilder` monta as três casas acessíveis com a folha da porta aberta (`CasaAcessivel`), então a porta da Mara é igual à do Nilo e à da Sera. E o próprio C2 desfaz a tensão ("por isso a porta fica aberta"): fica coerência, não contradição. Para 2: as outras portas fecham à noite (uma linha no gerador), e a ficha deixa a tensão sem explicação; ou outra contradição observável.
- **C8 = 2.** Quatro destinos com pano, objeto e fala, e a origem nos objetos da casa (GDD cap. 02); os ids conferem. Custo não declarado: serena, normal e dificil escolhem a fala por oportunidade e por evento, não por item, então pedem a condição por destino (pendência do `ELENCO.md`).
- **C9 = 2.** Duas batidas marcadas (tiras no vão → dobradas na prateleira; soleira → dentro de casa), e a fala `aos_oito` canônica passa a apontar para as tiras sem mudar uma palavra. Sem variante de malha.
- **C10 = 1.** Escrita, mas não encenável como está. A porta tem 1,8 m de largura (`VaoPorta`) e Mara, com o rolo, ~0,6 m, então não "enche o vão". O `NpcActor` põe o NPC a 1,5 m da âncora (`RaioDaVaga`), na rua, não na soleira. E no B06 Mara e Daren dividem a âncora e o quadro, a ~9 m da câmera, com metade da altura da criança na tela (conta da própria ficha). O quadro que vende Mara é o do C1: a tira no tronco do vão. Reescrever com ele, ou com vaga autorada e o Daren fora do quadro.

**Fatos dados como cânone que não conferiram:**
1. "Eira tem 1,72" e "pernas de compasso, quadril alto, `r_perna` ≈ 0,57" (C3): a ficha `eira` refeita tem 1,92 m, é a mais alta do elenco e tirou as pernas longas justamente porque repetiam a coluna da Mara.
2. "(dossiê §G: 'não modelar antecipadamente todas as versões')" (§1): a frase do dossiê fala de versões **etárias**. O dossiê §C e o GDD cap. 02 deixam estética, nome e profissão de Mara variarem com a origem. Malha única para as três origens é PROPOSTA desta ficha. Mara sem ofício (C8: "o resto é a casa e o limite") contraria o dossiê §G ("profissão varia com origem"): é decisão de produto, não de arte, e vira proposta em `docs/`.
3. "Nunca dá ordem sem pergunta" (C6) e "a única fala dela sem pergunta no fim" (C6, fala 2): `dialogo.mara.noite` ("De noite a gente fica perto de casa.") e `primeiro_dia` ("fala com o Daren antes de sair.") são ordem sem pergunta; `sobre_familia` e `aos_oito` também não terminam em pergunta.
4. "A folha da porta de `casa_familia` fica aberta… (já é assim na cena)" (C7): confere, mas vale para as três casas acessíveis.
5. "o rolo de manta enchendo a largura do vão" de "a porta de 2,2 m" (C10): a porta tem 1,8 × 2,2 m.
6. "Custo: só posição na âncora, que já é a porta" (C7), e Mara "na soleira" (C7, C9, C10): a âncora fica a ~1 m da fachada e o NPC a 1,5 m dela. Pôr Mara na soleira pede vaga autorada (código).
7. "a fala é escolhida pelo item, com a mesma `Condicao` de C5" (C8): só a ruptura escolhe por item; as outras três escolhem por oportunidade ou evento.

Conferiram: id, traço, rotina, âncoras, vínculos, tópicos e papel (`NpcCatalog.cs`, `strings.pt-BR.json`); falas canônicas e a precedência do `aos_oito` (`DialogueCatalog.cs`); testemunhos (`NpcMemory.cs`); mantas, oportunidades e eventos por destino (`DestinyCatalog.cs`); vão de 6 m, distâncias, porta aberta e spawn a ~6 m (`AurenSceneBuilder.cs`); a conta da câmera (`BodyByAge.cs`, `ThirdPersonCamera.cs`); `InventarioData.recompensasAplicadas` e nenhuma `Condicao` de inventário (`Inventario.cs`, `DialogueGraph.cs`); `PROVENIENCIA.md` e `ACERVO.csv` #106, #112, #122; GDD cap. 02 e cap. 09; dossiê §C; `INCONSISTENCIAS.md` A5; `PIPELINE.md` §3.1 e §4.

**Discordância com o `ELENCO.md`:** a linha da Mara ("coluna: a adulta mais alta de Auren depois do Borin") contradiz a linha da Eira na mesma tabela (1,92, a mais alta do elenco). A arbitragem 5 só tratou dos homens baixos. O extremo de cima ficou sem árbitro, e a Eira ficou com ele quando foi reescrita. A terceira forma da Mara tem de sair da estatura.

**Outras colisões com o elenco:**
- **Amuleto:** a fala da ruptura diante do `item.amuleto_rachado` ("ninguém sabe de onde, nem eu") é o mesmo lance de Borin, Oren, Daren, Eira, Sera, Lysa e Nilo. Recomendo que fiquem só o do Borin (aprovado) e o do Oren (quebra a regra do C2 dele). A ruptura da Mara já se sustenta sem o amuleto: o nó duplo e a tira que diz "onde parar, e não o que ela é".
- **Braços das mulheres adultas:** Mara (sino no cotovelo), Eira (gota sob o antebraço) e Lysa (funil no pulso) leem as três pelo braço. A ficha lista Lysa e Maelis como vizinhas; falta a Eira.
- **O vão:** Lysa e Tovin (tarde, cânone), Nilo (tarde, cânone), Maelis (PROPOSTA) e as tiras da Mara usam a mesma âncora. Para o coordenador. A premissa "o vão não fecha" desta ficha também precisa casar com a do Tovin (o espinheiro fecha de novo e o podão dele reabre).

**O sumiço de Nilo em cinco objetos (pedido do `ELENCO.md`):** é a vila reagindo, e isso é bom enquanto cada reação usar um canal diferente. O chifre calado do Tovin é som; a forquilha é o único marco no lugar onde o rastro acaba; a lousa da Eira é pista, que o jogador lê antes de falar com ela. A tira da Mara fica: é a única no caminho da q07 (`seguir_ate_o_bosque` passa pelo vão) e a única de espera ("por onde ele volta"), não de registro. A repetição está entre a folha de pé da Maelis e a lousa da Eira: duas tábuas carregadas por uma mulher, as duas com o registro escrito do mesmo sumiço. Cortar ou fundir uma delas fica com o coordenador.

**Para o G2 — o que o concept precisa provar:**
1. A terceira forma nova (a da reescrita de C3) lê em preto a 30% na mesma folha que Eira (1,92 m) e Borin (1,82 m). Estatura não conta.
2. O rolo esconde o pescoço de frente, de costas e de perfil, e não rasga na axila em T-pose dentro de 8 000 tris, rígido no `UpperChest`.
3. Mara, Eira e Lysa na mesma folha: sino no cotovelo, gota no antebraço e funil no pulso se separam. Se não se separarem, as três mulheres adultas são o mesmo boneco pelo braço.
4. Render no vão, no celular em paisagem: a tira terracota lê a 6 m contra o verde #648B67, e a tira do Nilo se distingue da tira da criança. Se não se distinguir, a regra 2 de C4 não se vê.
5. Render do B06 com a câmera real e as vagas reais do `NpcActor` (Mara e Daren a 1,5 m da âncora). É esse o primeiro quadro de Auren, não o da soleira.

**Condições cumpridas em 2026-10-03:**
- **(a) Terceira forma de C3, que não seja estatura:** a coluna saiu e entrou a **barra arrancada**, uma saia enrolada com a barra em diagonal de 0,35 m (42 px), do meio da canela à direita ao meio da coxa à esquerda. A altura (1,80 m) virou apoio (Arbitragem 2.6). As medidas antigas da Eira saíram: ela tem 1,92 m e é a mais alta do elenco, e Mara já não disputa estatura nem pernas com ela.
- **(b) C5:** "Perguntar pelo que vê" saiu inteira. A leitura do inventário fica com o Oren (Arbitragem 2.9), e o `em_casa` não é mais tocado. Entrou **a palavra no pano**: a tira do vão é interagível, e a fala de Mara aparece ali, longe dela. É a única conversa de Auren sem o NPC em cena, e nasce do C2 e do C4, como o parecer pediu. A dependência da `Condicao` de inventário saiu da ficha toda.
- **(c) Fatos corrigidos:**
  1. Eira com 1,92 m (C3 e §7).
  2. Malha única é PROPOSTA de custo, sem a citação errada do dossiê §G (§1). Mara ter ou não ofício próprio ficou marcado como decisão de produto, a levar a `docs/` (C8, origem).
  3. O tique do C6 foi reescrito para bater com as falas canônicas que dão ordem sem pergunta (acordo para o limite longe, regra para o perto), e saiu a alegação de "única fala sem pergunta".
  4. O C7 deixou de atribuir a Mara uma porta que é igual nas três casas.
  5. A porta tem 1,8 × 2,2 m.
  6. Saíram "a soleira" e "posição na âncora": nada nesta ficha pede vaga autorada.
  7. O C8 escolhe a fala por destino e declara o custo.
- **C4, regra 2:** a tira do Nilo segue o gatilho único, de `evento.nilo_desapareceu` até `marco_idade_8`, e vai para a prateleira no salto (Arbitragem 2.1).
- **C7 (recomendação):** a tensão ficou sem explicação (o C2 já não diz "por isso a porta fica aberta"). A metade visível é dela: PROPOSTA de as portas de `casa_nilo` e `casa_sera` fecharem à noite.
- **C10 (recomendação):** reescrito com o quadro do C1, a tira no vão, encenável com a câmera e as rotinas de hoje.
- **C2 (ressalva):** separado do Tovin. Ele avisa a vila; ela marca o limite de uma criança. A premissa do vão casa com o espinheiro e o podão da ficha do Tovin.
- **Amuleto:** a reação da ruptura saiu (Arbitragem 2.8). O ramo se sustenta pelo nó duplo e pela fala do "olhar".
- **Braços:** a vizinhança com a Eira caiu junto com as mangas-bolso dela (Arbitragem 2.4). As mangas em sino no cotovelo são de Mara.

**Conferência final (Art Director, 2026-10-03):** condições atendidas. (a), (b), (c), as recomendações de C2, C7 e C10 e a Arbitragem 2, itens 1, 4, 5, 6, 8 e 9, estão no corpo. Ressalva para o G2, sem pendência: a barra arrancada cobre a perna direita até 0,35 m do chão, e o estojo de estacas do Tovin, reescrito na mesma rodada, ocupa a canela direita de 0,08 a 0,40 m. As duas fichas dão a faixa como livre; as duas vão para a mesma folha.

## 6. Encaminhamento

**Para o G2, só depois de aprovado:**
- **vistas:** frente, perfil, costas e 3/4 verdadeiros em T-pose; fundo neutro; linha de chão; 1,80 m marcado; a barra desenhada com as duas alturas (0,35 m e 0,70 m do chão).
- **silhueta:** colar de manta, mangas de manta e barra arrancada, em preto a 30%, embaralhada com Daren, Borin, Eira, Lysa e Maelis (`@1,80` para a Mara). Também um render na câmera do jogo no vão, de manhã (C10), e um no B06, com as vagas reais do `NpcActor`.
- **o concept precisa provar:**
  - nada passa do topo da cabeça;
  - o colar não rasga na axila em T-pose dentro de 8 000 tris;
  - os sinos das mangas leem no cotovelo e se separam dos punhos da Lysa;
  - a barra em diagonal lê a 30% como um corte inclinado e não como o avental reto do Borin nem como o tabardo da Maelis (se não ler, C3 volta);
  - no clipe de andar, o lado longo da saia não estica entre as pernas;
  - a tira terracota lê a 6 m contra o verde #648B67 do bosque, na tela do celular em paisagem, e a tira do Nilo se distingue da da criança (o nó duplo da ruptura não conta para isso).
- **paleta:** manta, mangas, saia e tiras em terracota #A86D52 ("Auren", GDD cap. 09), complementar ao verde do bosque, onde a tira precisa aparecer; calça e pano de cabeça em terra neutra. Proibidos: turquesa #86C8C9, dourado #D6B36A, violeta #9777B8.
- **objeto à parte:** a manta aberta (2,0 × 1,4 m), a saia com a barra arrancada e a tira (0,60 × 0,08 m), cada uma nos três estados (barra tecida, lisa, remendada); o nó duplo da ruptura; as tiras dobradas na prateleira (aos 8).
- **orçamento** (`PIPELINE.md` §4, HIPÓTESE v0):
  - Npc com 8 000 tris, 2 materiais, textura 1024 e 55 ossos sem secundário, então colar, mangas e saia são rígidos;
  - os estados do pano são 3 variantes de `_basecolor` ou uma máscara de remendo: custo de textura, não de malha;
  - as tiras são Prop pequeno (≤ 500 tris, 256 px), em 2 instâncias, mais as 2 dobradas;
  - os objetos de origem ficam na mesa da casa: 3 Props pequenos.
- **custo fora da arte:**
  - o componente de peça ligada por evento ou estado (tiras e portas; pendência do `ELENCO.md`);
  - o interagível da tira, com o painel aberto sem NPC em cena (C5);
  - a condição de diálogo por destino (C8; pendência do `ELENCO.md`);
  - 4 falas de destino e 2 novas de C5/C6;
  - nenhum clipe novo.
- **testes:**
  - troco com Daren no B06 e no vão (a tira é dela; a cena tem de quebrar);
  - troco com Tovin no vão (ele avisa com o chifre e leva pela mão; ela deixa a palavra no pano);
  - descrição;
  - o protótipo PROTOTIPO atual de Mara não serve de entrada (ADR-0008).

**Para o G3:** bloco `### mara` em `PROVENIENCIA.md` com `g1:` (data, nota, pontuador), `g2:` (data, silhueta n/5, descrição, troco), `entrada:` (concept em `arte/referencias/mara/` + SHA-256), `ferramenta`, `plano` pago, `licenca`, `termos_url`, `g3:`. `referencias_terceiros`: nenhuma usada nesta ficha.

## 7. Ligações com outras fichas

**PROPOSTAS que envolvem outros personagens:**
- **Daren:** Mara guarda a ponta norte do eixo de Auren (o vão do bosque) e Daren a ponta sul (o portão). As contradições se espelham: ela prende os pés e deixa a casa aberta, ele prende as mãos e solta os pés. Mara tem 1,80 m e Daren 1,70 m.
- **Nilo:** a segunda tira, "por onde ele volta", fica no vão de `evento.nilo_desapareceu` até `marco_idade_8`, o mesmo gatilho da forquilha, do chifre calado e da folha da Maelis (Arbitragem 2.1). Aos 8 está dobrada na prateleira. A tira é de Mara, não da mãe dele, que só aparece na fala canônica e não é NPC. A forquilha fica na clareira, não no vão (ficha do Nilo).
- **Nilo e Sera (cena):** a PROPOSTA do C7 fecha à noite as portas de `casa_nilo` e `casa_sera`. Se a ficha do Nilo quiser a porta dele aberta durante o sumiço, a contradição de Mara deixa de ser exclusiva nesse intervalo. O coordenador decide.
- **Avatar** (`avatar_crianca5`/`avatar_crianca8`): a manta inicial da criança saiu da manta de Mara, com o mesmo pano, o mesmo estado e a cor terracota. A ficha do avatar já adota isso e não divide forma com ela: rolo nos ombros e barra em diagonal × nó no peito e sino atrás das pernas.
- **Tovin:** ele avisa a vila (o chifre); ela marca o limite de uma criança (a tira). O vão aberto é dele (podão contra o espinheiro, ficha `tovin`); o pano no vão é só de Mara. Aos 8, o limite passa a ser o dele. A ficha do Tovin não deve marcar o vão com pano.
- **Oren:** os objetos de origem na mesa de casa vêm da praça dele, à tarde. Que ela é freguesa dele já é cânone, e nada aqui diz que a casa não compra dele (ficha do Oren, §7). A leitura do que a criança carrega é dele (Arbitragem 2.9).
- **Eira:** a mais alta do elenco é ela (1,92 m). Mara não usa estatura nem pernas longas como forma.
- **Elenco:** o C8 depende da "condição de diálogo por destino" do `ELENCO.md`, a mesma de várias fichas.

**Zonas de silhueta ocupadas** (depois das duas arbitragens do `ELENCO.md`):
- rolo de pano nos dois ombros, com o pescoço escondido. É a zona "ombros sem pescoço", dela pela Arbitragem 1.3;
- mangas em sino que alargam no **cotovelo**, iguais nos dois lados. É dela pela Arbitragem 2.4;
- barra inclinada atravessando as duas pernas, do meio da coxa (esquerda) ao meio da canela (direita), sem passar da largura do quadril. É livre no elenco;
- nada acima da cabeça; altura (1,80 m) só como apoio;
- prop de mundo: tira no tronco leste do vão do bosque, interagível.
