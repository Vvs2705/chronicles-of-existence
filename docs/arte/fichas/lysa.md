# Ficha G1 — `lysa`

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md)).
> Fato com fonte é cânone. **PROPOSTA** = ideia desta ficha, não está nos docs e não vale até ser aprovada.
> Não contradiz o cânone de trabalho do Borin (ADR-0010).

## 1. Identificação

- **id:** `lysa` (`NpcCatalog.cs`; GDD cap. 06, NPC-04)
- **categoria:** Npc, base adulta (`PIPELINE.md` §3.1: 1,55–1,95 m)
- **onde aparece no slice:** B07 (cotidiano) e a opcional `q05_o_animal_ferido`: `encontrar_o_animal` (`entrada_bosque`, sem NPC) → `buscar_ajuda` (`ervanaria`, Lysa) → `tratar_o_animal` (`entrada_bosque`, Lysa e Tovin) (`content/quests/q05_o_animal_ferido.json`); B12, no aviso "o que se encerra" se a q05 ficou aberta; B14, falas `aos_oito` e `aos_oito_animal` (`DialogueCatalog.cs`); depois do salto, Sera aprende com ela de manhã na `ervanaria` (rotina `aprender_com_lysa`, `NpcCatalog.cs`, marcada [PROPOSTA] no código). Âncoras: `ervanaria` (manhã `preparar_ervas`, noite `secar_ervas`) e `entrada_bosque` (tarde `colher_no_bosque`); os horários são HIPÓTESE v0 do próprio código (cabeçalho do `NpcCatalog.cs`).
- **cânone de partida:**
  - herbalista paciente; natureza e pistas do bosque (dossiê §G); "Herbalista, natureza e cuidado" (GDD cap. 06); a Q-05 é "Conhecimento e compaixão" (GDD cap. 07)
  - `traco.paciente`; vínculo com Tovin, `parceira_de_bosque`, e o recíproco dele; sabe `topico.ervas`, `topico.bosque`, `topico.vila_auren`, e **não** `topico.limiar` nem `topico.primeira_fratura` (`NpcCatalog.cs`). O nó `sobre_limiar` do grafo dela existe e é inalcançável de propósito (`DialogueCatalog.cs`)
  - falas já escritas (`strings.pt-BR.json`): "Pisa devagar aqui. As plantas da beira do bosque se assustam mais que bicho." · "Entra, mas não sopra as folhas…" · "…E nenhuma vai à boca sem eu mandar." · "O bosque dá muito a quem pede pouco. Da entrada para dentro, nem eu vou sem o Tovin." · depois da q05: "…Você teve mão leve com ele, e isso ninguém ensina." · aos 8: "A Sera agora aprende comigo…" e, com a q05, "Aquele bichinho que cuidamos juntos ainda aparece aqui na porta…"
  - a entrada do bosque é o limite da criança (`dialogo.mara.em_casa`) e de qualquer um sem Tovin (`dialogo.tovin.sobre_bosque`); o Bosque dos Sussurros é adjacente a Auren (dossiê §I); Tovin caça na mesma âncora, no mesmo período (`NpcCatalog.cs`)
  - q05: recompensa `item.ervas_de_lysa` ×2 (quantidade HIPÓTESE no json); grava `evento.q05_concluida` e `cuidou_do_animal`; só Lysa testemunha (`NpcMemory.cs`). Confiança em Lysa pela q05 só aparece como exemplo em comentário (`ReputationSystem.cs`) e em teste: a tabela `Consequencias` hoje só tem a Q-04
  - Vida da Ruptura: "algo fora de lugar no bosque" (`destino.ruptura.descricao`); `evento.anomalia_no_bosque` está declarado em `DestinyCatalog.cs` e **nenhum código o grava ainda**
- **autor da ficha / data:** Concept Art Lead (agente), 2026-10-03
- **estado:** aprovado G1 por delegação (ADR-0010, 2026-10-03); condição (b) decidida em 2026-10-03 pelo dono do conteúdo de missão, por delegação (ADR-0010, adendo, item 10), reconferido sem pendência pelo Art Director (2026-10-03, leva C1, fim da §5)

## 2. Critérios C1–C10

**C1 — Gancho.** *(PROPOSTA)* A pessoa que mais sabe do Bosque dos Sussurros colhe toda tarde de costas para ele, ajoelhada a dois passos do lado de fora da linha e virada para a vila (da linha para dentro, nem ela vai sem o Tovin), e o bicho ferido que a criança lhe traz saiu de lá de dentro.

**C2 — Necessidade do mundo.** Cânone: Auren vive colada a um bosque onde ninguém entra sozinho (dossiê §I; falas de Mara, Tovin e Lysa acima), e a colheita diária de Lysa é na beira do bosque (chave `atividade.colher_no_bosque` no `NpcCatalog.cs`; o texto "Colhendo na beira do bosque" está em `strings.pt-BR.json`). **PROPOSTA:** a farmácia de Auren é a faixa de uns passos na boca do bosque. O que cura é o que o bosque deixa chegar até a beira; o que cresce dentro, Lysa só alcança no dia em que Tovin tem tempo. Ela existe porque a vila precisa de remédio e não pode entrar onde ele cresce. Daí colher pouco, ajoelhada e devagar ("dá muito a quem pede pouco" é ofício, não filosofia). E, como colhe a mesma faixa do lado de cá toda tarde, é a primeira a ver quando o que nasce ali muda: as "pistas do bosque" do dossiê §G, aqui, são ela notar a beira mudar sem saber dizer por quê.

**C3 — Silhueta em 3 formas.** *(PROPOSTA)* Lysa é a adulta mais baixa do elenco: 1,58 m, quadril baixo (`r_perna` ≈ 0,50, piso da base adulta, `PIPELINE.md` §3). Tudo preso à malha, sem osso secundário. Medidas a 30% pela régua do `client/tools/silhueta.py` (120 px/m):
1. **Aba-varal:** chapéu de junco de copa baixa e aba de 0,72 m (86 px, ~4× a cabeça), com a borda enrolada num rolo de junco trançado de 5 cm (6 px) e caimento de ~8° da copa até a borda. Na borda pendem 8 feixes de 0,12 m, de cabeça para baixo (franja de 14 px). **De frente, em T-pose,** é uma faixa larga de 0,72 m com ~0,08 m de altura (rolo e caimento) e a franja embaixo; o disco só aparece visto de cima. A borda de 5 cm existe para passar na limpeza do `silhueta.py`, que apaga traço abaixo de ~25 mm (`ELENCO.md`, arbitragem 2, item 7): aba chata de 1 cm, de frente, vira linha e some. **T-pose:** sobrevive; a faixa não depende dos braços, e a franja termina ~5 cm acima da linha deles. **Câmera do jogo** (pivô a 0,94 m, 2,75 m atrás da criança, 15° na partida: `BodyByAge.cs` e `ThirdPersonCamera.cs`; olho a ~1,65 m; a criança ocupa ~1/3 da tela em paisagem): de pé, a aba fica um palmo abaixo do olho e lê como elipse larga e achatada com franja; à tarde, ajoelhada na beira, desce para ~1,0 m e vira disco visto de cima, a leitura mais forte dela.
2. **Base-balão:** calça de linho cheia nas coxas e presa na canela, para ajoelhar no chão da beira: cada perna com o dobro da largura de uma calça justa (0,30 m, 36 px). Em preto, base pesada e baixa, volume simétrico (não é objeto ao lado da coxa, como o aro do Borin). **T-pose** e **câmera:** sobrevive; é o que a câmera mais mostra quando ela se ajoelha.
3. **Punhos em funil:** luvas de couro grosso de cano largo, punho de 0,16 m (19 px) sobre pulso de 0,06 m (7 px), para urtiga e espinho. Em T-pose são as pontas dos braços alargadas, iguais nos dois lados (não é a assimetria de braço do Borin). **Câmera:** na altura do peito da criança quando ela trabalha. É a forma mais frágil. **Reserva:** se os punhos não se separarem das mangas da Mara (que alargam no cotovelo) a 30%, o cano sobe e vira manopla até o meio do antebraço, com boca de 0,22 m (26 px). A ponta do braço é a zona que a arbitragem 2 (item 4) deu a ela; com a manopla, a diferença com a Mara passa a ser de lugar e de tamanho. A cintura marcada, a única zona de contorno ainda livre no elenco, ficou com a Eira (ficha par).

**Contra Eira e Mara em preto:** Lysa é faixa larga em cima e base larga embaixo, e a mais baixa das três. Eira é a mais alta, com uma placa no flanco direito, uma lanterna sob o braço esquerdo e a cintura apertada. Mara tem rolo nos ombros e mangas em sino no cotovelo. Lysa não tem trança, avental nem vestido em triângulo, que são o default compartilhado com Mara (§4).

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **O chapéu de secar.** Aba de 0,72 m, copa de 0,10 m, 8 ganchos de madeira na borda, feixes de 0,12 × 0,04 m. A aba faz sombra e o passo faz vento: as ervas da tarde secam na cabeça dela na volta, e é por isso que ela anda devagar. Três regras, todas sobre o que o código já lê:
- **Período (rotina):** à tarde, em `entrada_bosque`, a aba leva os 8 feixes; de manhã e à noite, na `ervanaria`, a aba está vazia e os feixes pendem das vigas (`secar_ervas`). O jogador sabe pelo chapéu de onde ela vem.
- **Missão (estado da q05; aprovada, ADR-0010, adendo, item 10):** com a q05 em andamento e `buscar_ajuda` cumprido, o chapéu fica no chão em `entrada_bosque`, ao lado da vaga da Lysa, emborcado sobre o bicho ("escuro acalma"), sem colisor, e Lysa anda de cabeça descoberta em qualquer âncora. Quando a q05 sai de "em andamento" (concluída, ou encerrada pelo salto), o chapéu volta à cabeça dela. A regra lê só o estado da missão, que o save já guarda. O chapéu no chão é o marcador do objetivo no mundo, a mais do HUD, que continua dizendo o objetivo por texto; Lysa sem chapéu avisa a vila inteira que tem um bicho esperando.
- **Destino (`BirthChoice.destinyId == "ruptura"`):** um nono feixe pende separado, na frente da aba: uma planta da beira que ela não sabe nomear. Quando o histórico passar a gravar os acontecimentos do destino, a regra lê `evento.anomalia_no_bosque` no lugar do id. Cor desbotada e neutra: esta ficha **não** decide como a anomalia se vê (sem violeta, sem fio, círculo ou ponto da Trama).

Nada é concedido, nenhum número muda.

**C5 — Regra exclusiva.** *(aprovada pelo dono do conteúdo de missão, por delegação, com dois ajustes: ADR-0010, adendo, item 10)* **Chegar devagar.** No objetivo `tratar_o_animal`:
1. O jogador vai até o chapéu emborcado. Correndo (borda do joystick, ADR-0007 §9: 3,8 m/s contra 1,6 m/s andando, `MotionSolver.cs`) a menos de 4 m, o chapéu treme, o bicho encolhe e a contagem zera.
2. Parado a até 1,5 m do chapéu, o jogador espera três respirações (~3 s). Não precisa segurar botão: ficar parado é o gesto (ajuste 1; no toque, um gesto a menos).
3. O bicho fica calmo e a opção do objetivo, "Vamos cuidar dele juntos?", aparece na conversa com Lysa ou Tovin (os dois NPCs do objetivo). À tarde, Lysa está ajoelhada ali e levanta a aba; em outro período, a criança fala com ela onde ela estiver (o objetivo segue alcançável em qualquer período, ADR-0007, consequências). "Bicho calmo" é estado de cena e não vai para o save (ajuste 2): vale até o objetivo fechar, e fechar o app só pede repetir os 3 s.

Falhar só repete o passo 1: sem punição, sem recompensa extra, sem beco sem saída, e a q05 continua opcional. É a única regra do slice que mede a velocidade do jogador. A ideia de não correr já aparece na q07, na fala de Tovin (`dialogo.tovin.depois_da_busca`): a velocidade fica com ela, o rastro com ele. Custo: um componente que lê velocidade e distância do avatar, e uma condição que segura a opção da missão até o passo 2 (`ELENCO.md`, pendências de código).

**C6 — Voz.** *(PROPOSTA; texto final é do redator)* Ordem dada baixinho, conta em respirações, fala de planta como de bicho e de bicho como de planta, como nas falas já escritas:
1. "Para. Conta três respirações. Ele está contando as suas." (C5, passo 2)
2. "Correu, ele encolheu. Não é birra, é medo. Volta três passos e vem de novo." (C5, falha)
3. "Essa nasceu na beira e não é de beira. Não sei o nome. Deixo secando separada, pra ver o que vira." (C4, Ruptura)

**C7 — Contradição visível.** *(PROPOSTA)* É quem mais sabe do bosque e colhe de costas para ele. Observável toda tarde em `entrada_bosque`: ajoelha a dois passos do lado de fora da linha, virada para a vila, e colhe só o que nasce do lado de cá. Não se vira para o escuro nem quando Tovin, que caça ali na mesma hora, passa por ela e entra. Não estende a mão para o outro lado: o alcance por cima da linha é do Nilo (`ELENCO.md`, arbitragem 2, item 2). Custo: uma animação própria (ajoelhar e colher no chão, em loop); com idle compartilhado, sobra ela ajoelhada de costas para o bosque.

**C8 — Relação com o avatar por destino e origem.** *(PROPOSTA; só fala e objeto, nunca poder, dossiê §H; itens e oportunidades conferem em `DestinyCatalog.cs`)*
- **serena:** a criança tem `oportunidade.tarde_livre`, e a tarde é a hora de Lysa na beira: é a única vida em que dá para passar todas as tardes ajoelhada ao lado dela. Lysa a trata como ajudante de beira e lhe ensina a contar respirações antes de pegar em qualquer coisa.
- **normal:** na q05 ela lava a ferida com a água do `item.cantil` da criança ("ferida se lava antes de tudo"); o item não é gasto. Trata-a como quem já sabe trazer e levar.
- **dificil** (rótulo "Vida Árdua", ADR-0007 §2): ano de escassez (`evento.ano_de_escassez`). Lysa não ensina remédio, ensina o que da beira se come ("essa enche barriga, aquela não"), e não cobra: em Auren "todo favor é lembrado" (`destino.dificil.contexto_social`).
- **ruptura:** ela é dos que olham a criança "com cuidado" (`destino.ruptura.contexto_social`): mostra o nono feixe e pergunta se a criança já viu aquela planta. Não liga a planta a nada da criança nem ao Limiar (não sabe `topico.limiar`); diz que não sabe o nome. Não reage ao amuleto (arbitragem 2, item 8).
- **origem:** `agricultores` (`oportunidade.horta_da_familia`): "Da horta você sabe. Da beira, ninguém planta." · `guardioes` (`oportunidade.ronda_com_tovin`): "Com o Tovin você aprendeu a pisar. Comigo, a parar." · `artesaos`: sem variante.
- Custo: 4 falas de destino e 2 de origem; o `DialogueGraph` ainda não tem condição de destino nem de origem (hoje: período, memória, tópico, missão, confiança). A mesma dependência já existe no C8 do Borin.

**C9 — Mudança dos 5 para os 8.**
- **Antes (5–7):** sozinha na ervanaria; aba cheia à tarde, vazia de manhã e à noite.
- **Depois (8):** Sera trabalha com ela de manhã (rotina pós-salto já implementada, marcada [PROPOSTA] no `NpcCatalog.cs` e no `SLICE` B09). Com `evento.q05_concluida`, o bicho aparece de manhã na porta da `ervanaria`: a fala `aos_oito_animal` já diz isso; pôr o bicho em cena é **PROPOSTA** e pede uma criatura com G1 própria. E pela primeira vez ela se oferece para ensinar a criança ("Se você tiver paciência com as plantas, também pode", fala `aos_oito` já escrita).
- **Pede variante de malha?** Não. Muda adereço (feixes, nono feixe), cena e fala.

**C10 — Momento de cartaz.** *(PROPOSTA)* Fim de tarde na boca do bosque, câmera do jogo atrás da criança. No chão, um chapéu de aba larga emborcado treme. Ao lado dele, ajoelhada de costas para as árvores e de frente para a criança, uma mulher baixa de cabelo curto e luvas grossas levanta um dedo: "Pisa devagar aqui." A criança dá um passo curto, e o chapéu para de tremer.

## 3. Amarração

- **Destino, Grau ou Trama:** o destino muda o que ela ensina (C8). Grau não se aplica no slice (ascensão fora de escopo, dossiê §E). A Trama só a toca na Vida da Ruptura, e só como coisa notada: o nono feixe, que ela não sabe nomear e não explica.
- **Decisão de jogo que ele cria:** gastar ou não tardes da infância na beira com ela (a q05 expira aos 7) e aceitar ir devagar num jogo que deixa correr. O que fica aos 8: o bicho na porta e a oferta de ensinar.
- **Pilar:** viver e crescer.

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| bruxa da floresta: chapéu pontudo, manto escuro, caldeirão, cajado torto, corvo no ombro | chapéu de copa baixa e aba chata com ervas secando; nada na mão além da luva; nenhum cajado |
| druida ou elfa: roupa toda verde, folha costurada, cura que brilha | colete cor de barro e calça de linho cru (marfim #E9DEC6, liberado em roupa pelo ADR-0010 §5); o único verde nela é o que ela colheu (#648B67, "áreas naturais", GDD cap. 09), e de manhã e à noite ela não tem verde nenhum; cura é água, tala e erva, sem luz |
| curandeira-mãe de vestido verde, avental e trança: o default que hoje cola nela e em Mara (vestido verde de coleta e coque trançado no acervo, `ACERVO.csv` #49, #52, #57; trança ruiva e avental curto no protótipo, `PROVENIENCIA.md` §6; Mara com trança longa e túnica verde no acervo e avental no protótipo) | calça-balão de ajoelhar, sem avental, cabelo curto escondido sob o chapéu: trança, avental e vestido ficam livres para a ficha de Mara |
| kit miúdo de herbalista como assinatura (estojo de amostras, pinça, saquinho, bolsa de lona: acervo) | o chapéu; o kit miúdo vira textura, não forma |
| sábia que explica o bosque e a magia | sabe de ervas, bosque e Auren (`NpcCatalog.cs`); nota que a beira mudou e diz que não sabe o nome |
| verde-azulado ou sálvia (a cor de placeholder dela, #81B29A em `Prototipos.cs`, é vizinha do turquesa #86C8C9 da Trama); palha clara, que cola no dourado #D6B36A do Limiar | nenhum azul-esverdeado na roupa; junco escurecido no chapéu, longe do dourado; turquesa, dourado e violeta proibidos nela |

## 5. Avaliação — parecer do Art Director: 20/20, aprovado (ADR-0010)

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total /20 |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | **20** |

- **Corte:** ≥ 14/20 e nenhum zero em C2, C3, C4, C5 — **atingido** (HIPÓTESE do ADR, a recalibrar)
- **Pontuador / data:** Art Director (agente) / 2026-10-03. Não escreveu a ficha.
- **Veredito:** **aprovado por delegação (ADR-0010)**, com duas condições antes do concept. (a) **C1 e C7 saem da pose do Nilo.** A imagem do gancho (de joelhos na linha, tronco para dentro, pés para fora) e a ação de C7 (ajoelhada na linha, alcança o escuro com a luva, não move os joelhos) são a contradição e a pose da ficha `nilo` (C1, C7, C10: pés na linha, corpo inclinado, forquilha do outro lado, "nunca pisou além da linha"), na mesma âncora e no mesmo período. Com Nilo no lugar, a cena de C7 continua funcionando: o teste do troco do G2 reprovaria o par. Pela regra do `ELENCO.md` (fica com a zona quem a usa no C4), o alcance por cima da linha é do Nilo, cuja forquilha é C4 e C5. Se o coordenador decidir o contrário, quem reescreve é o Nilo. (b) O dono da q05 aprova o chapéu emborcado e o "chegar devagar". Se não aprovar, C4 perde a regra de missão, C5 cai inteiro e a ficha volta para nota.
- A nota mede a ficha sozinha. Fora de C3, a régua não tem coluna para colisão entre fichas (lacuna já apontada no parecer do Borin), e por isso a colisão com o Nilo entra como condição e não como ponto.

**Nota por critério (1 linha cada):**
- **C1 = 2.** Tem detalhe concreto (de joelhos na linha, pés para fora) e tensão (quem mais sabe do bosque não entra, e o bicho veio de dentro). É a mesma imagem do gancho do Nilo: condição (a).
- **C2 = 2.** "A farmácia é a faixa da beira" sai do cânone (colheita na beira, a regra da linha em três falas) e faz dela quem vê a beira mudar, que é a leitura das "pistas do bosque". Ressalva: sem os nomes, "herbalista na orla de uma floresta perigosa" é do gênero. O que segura o 2 é a regra da linha, que é de Auren.
- **C3 = 2.** As três formas saem do contorno, e a aba é única no elenco, com a zona dada pela arbitragem 2. Ressalvas: de frente, a aba chata é uma linha, não um disco (o disco só aparece com a câmera alta). Os punhos ficam no braço, zona que o `ELENCO.md` não arbitrou e onde já estão as mangas da Mara (cotovelo) e as da Eira (antebraço). A reserva declarada, "proporção baixa", não existe (ver "fatos"). Fica uma vizinhança não arbitrada com a verga do Aethron (ver G2). Nenhuma delas é colisão clara.
- **C4 = 2.** Objeto com medidas e três regras com ids. A regra de missão é a melhor do elenco até aqui: o chapéu no chão marca o objetivo no mundo, sem seta. Custo: enquanto a q05 está aberta, ela fica sem a forma 1, e num save a q05 pode ficar aberta a infância inteira.
- **C5 = 2.** Regra especificada com números que conferem (3,8 contra 1,6 m/s, 4 m, ~3 s), e errar só repete o passo. Vizinha do Tovin: o "não correu" da fala canônica `depois_da_busca` é dele, na q07. A velocidade fica com ela, o rastro com ele.
- **C6 = 2.** Três falas no tique declarado (conta respirações, fala de planta como de bicho). A fala 3 preserva a diferença entre o "não sei" dela e o da Eira.
- **C7 = 2.** Observável toda tarde, com o custo declarado. É a mesma ação do Nilo: condição (a).
- **C8 = 2.** Quatro destinos e duas origens, com itens e oportunidades que conferem. A da normal (cantil usado, não gasto) é a mais limpa. Custo: 6 falas e uma condição de destino e origem que o diálogo ainda não tem.
- **C9 = 2.** Antes e depois marcados. A mudança acontece em volta dela (Sera, bicho, pedras), o que vale para adulto, como no Borin. Recomendação: cortar as pedras. Seriam o quarto marco na mesma linha (tira da Mara, Nilo aos 8, Maelis), e ela não sabe por que as pôs. C9 fica de pé com a Sera, o bicho e a oferta de ensinar.
- **C10 = 2.** Um plano, 5 s, fala canônica. Quebra com Nilo ou com Mara no lugar, porque depende do chapéu.

**Fatos dados como cânone que não conferiram:**
1. "a colheita diária de Lysa é 'na beira do bosque' (`NpcCatalog.cs`)": o código só tem a chave `atividade.colher_no_bosque`. O texto "Colhendo na beira do bosque" está em `strings.pt-BR.json`.
2. C3, "a terceira passa a ser a proporção (baixa, quadril baixo)": não serve de reserva. Oren (1,60 m, `ELENCO.md`) é só 2 cm mais alto, e o `silhueta.py` escala cada figura pela caixa inteira: com o chapéu, a caixa dela passa do 1,60 m do Oren.
3. C5, "a única missão do slice que pede ao jogador que vá mais devagar": vale como mecânica. Como ideia, a q07 já premia não correr (`dialogo.tovin.depois_da_busca`).
4. C3, "disco largo com franja": de frente, em T-pose, a aba chata é uma linha de ~86 px com franja.

Conferiram: id, traço, rotina com os horários HIPÓTESE, vínculo, tópicos e o nó `sobre_limiar` inalcançável (`NpcCatalog.cs`, `DialogueCatalog.cs`); todas as falas citadas (`strings.pt-BR.json`); objetivos, âncoras, recompensa HIPÓTESE, evento, flag e testemunha única da q05 (`q05…json`, `NpcMemory.cs`); `Consequencias` só com a Q-04 (`ReputationSystem.cs`); `evento.anomalia_no_bosque` sem ninguém que o grave; itens e oportunidades (`DestinyCatalog.cs`); velocidades (`MotionSolver.cs`); câmera (`BodyByAge.cs`, `ThirdPersonCamera.cs`); `PIPELINE.md` §3, §3.2 e §4; acervo #49, #52 e #57, protótipo (`PROVENIENCIA.md` §6) e #81B29A (`Prototipos.cs`); hex do GDD cap. 09; ADR-0007 §1, §2 e §9; ADR-0010 §5.

**Para o G2 — o que o concept precisa provar:**
1. **A aba de frente:** vista de frente, a aba chata é uma linha. A borda precisa de ≥ 0,03 m de espessura (ou de caimento) para passar na abertura do `silhueta.py` (`LIMPA` = 7 px a 512 px, ~2,5 cm nessa altura). Se não passar, a aba some, e os feixes, soltos do corpo, são descartados pelo preenchimento. Engrossar a aba antes de qualquer outra mudança.
2. **Lysa e Aethron na mesma folha:** a verga de 0,70 m com ombreiras e a aba de 0,72 m, com a franja terminando ~5 cm acima da linha dos braços, podem ler as duas como "cabeça dentro de uma moldura". A arbitragem 2 do `ELENCO.md` não listou o Aethron. Pela regra (C4), a zona é dela, e quem muda é ele.
3. **Os braços:** punhos (pulso) contra as mangas-bolso da Eira (antebraço) e as mangas em sino da Mara (cotovelo). Se os punhos não separarem a 30%, a terceira forma precisa vir de fora do braço e de fora da estatura.
4. **Lysa sem chapéu** (enquanto a q05 está aberta): continua identificável na câmera do jogo pela base-balão, que precisa manter o vão das canelas para não ler como saia.
5. **O chapéu emborcado no chão,** a 6–10 m, lê como "o chapéu da Lysa" na tela do celular em paisagem.
6. **Teste do troco** também com o Nilo, além de Mara e Tovin, depois da condição (a).

**Condições cumpridas em 2026-10-03** (pelo autor da ficha; a nota acima não mudou):
- **(a) Pose do Nilo:** C1, C7 e C10 trocados. Lysa colhe ajoelhada a dois passos do lado de fora da linha, de costas para o bosque e virada para a vila, e não estende a mão para o outro lado: o alcance é do Nilo (arbitragem 2, item 2). Na mesma tarde e na mesma âncora, ele olha para dentro e ela para fora.
- **(b) Dono da q05:** continua pendente; não depende desta ficha.
- **C9:** pedras aos 8 cortadas (recomendação do parecer e arbitragem 2, item 2). Ficam Sera, o bicho e a oferta de ensinar.
- **C3, aba de frente** (G2 item 1, fato 4, arbitragem 2 item 7): borda enrolada de 5 cm e caimento de ~8°. De frente, C3 descreve uma faixa de 0,72 × ~0,08 m com franja, não um disco.
- **C3, reserva** (fato 2): "proporção baixa" saiu. A reserva agora é a manopla até o meio do antebraço, na zona dos braços dada a ela (arbitragem 2, item 4). A estatura ficou só como dado (arbitragem 2, item 6).
- **Fato 1:** a citação da beira passou a apontar a chave no `NpcCatalog.cs` e o texto no `strings.pt-BR.json`.
- **Fato 3:** C5 diz que é a única regra que mede velocidade e cita a fala de Tovin na q07.
- **Amuleto** (arbitragem 2, item 8): C8 Ruptura não reage ao amuleto.
- **§6:** troco com Nilo; folha de silhueta com Nilo e Aethron; custo sem as pedras; animação trocada para "ajoelhar e colher no chão".

**Conferência final (Art Director, 2026-10-03):** pendente: condição (b). O dono da q05 ainda não aprovou o chapéu emborcado (a regra de missão do C4) nem o "chegar devagar" (C5). Até aprovar, C4 e C5 continuam condicionais, como diz o veredito. A condição (a), os fatos 1–4, as pedras cortadas e a borda de 5 cm (Arbitragem 2, itens 2, 4, 6, 7 e 8) estão atendidos no corpo. Continua aberta para o G2 a vizinhança entre a aba e a verga do Aethron, cuja ficha não mudou.

**Correções de 2026-10-03 (W3), para o conferente:** condição (b) decidida pelo dono do conteúdo de missão, por delegação (ADR-0010, adendo, item 10). O chapéu emborcado (C4) e o "chegar devagar" (C5) estão aprovados, com dois ajustes no C5: a espera é ficar parado a até 1,5 m do chapéu por ~3 s, sem segurar botão; e "bicho calmo" é estado de cena, fora do save, válido para a conversa com Lysa ou Tovin. A regra do chapéu lê o estado da q05 (em andamento e `buscar_ajuda` cumprido) e devolve o chapéu quando a q05 sai de andamento, inclusive pelo salto. O código está no `ELENCO.md`. C4, C5 e §6 (dependências) foram atualizados; formas e medidas não mudaram.

**Reconferência (Art Director, 2026-10-03, leva C1):** condição (b) atendida. O G1 fica aprovado por delegação, sem pendência. O C4, o C5 e a §6 seguem o ADR-0010, adendo, item 10:
- o chapéu emborcado lê só o estado da q05 (em andamento, com `buscar_ajuda` cumprido) e volta quando a q05 sai de andamento, inclusive pelo salto;
- a espera é ficar parado a até 1,5 m do chapéu por ~3 s, sem botão;
- "bicho calmo" fica fora do save;
- a opção aparece na conversa com Lysa ou com Tovin.

A decisão não muda forma. As medidas do C3 são as mesmas; conferi pelo diff. A janela sem chapéu é a que o parecer já contava no C4, com uma diferença: agora ela fecha também no salto. Pela regra antiga ("até `tratar_o_animal` concluído"), uma q05 vencida deixava a Lysa sem a forma 1 depois dos 8. Conta da aba: a ficha não declara a caixa; estimo ~1,66 m com a copa. Com a figura em 80% da altura, a borda de 5 cm dá 12 px contra o corte de 7 px. Os px da folha (86, 6, 14, 36, 19, 7 e 26) conferem. A linha "(b) continua pendente" da lista de condições cumpridas é histórica. Continuam para o G2, como já estavam: a aba contra a verga do Aethron, e a Lysa sem chapéu, que precisa ser identificável pela base-balão.

## 6. Encaminhamento

**Para o G2 (concept), só depois de aprovado:**
- vistas: frente, perfil, costas e 3/4 verdadeiros em T-pose, fundo neutro, linha de chão, 1,58 m marcados; a aba com a borda de 5 cm e o caimento desenhados de frente, em dois estados: com os 8 feixes (o da tarde, que vai ao teste de silhueta) e vazia
- silhueta: aba-varal, base-balão e punhos em funil em preto a 30% (`client/tools/silhueta.py`, 120 px/m), embaralhada com pelo menos Eira, Mara, Borin, Nilo e Aethron; render na câmera do jogo com ela de pé e ajoelhada de costas para o bosque
- paleta: verde #648B67 só nos feixes; barro no colete, linho cru marfim na calça, couro escuro nas luvas, junco escurecido no chapéu; reservados e proibidos nela: turquesa #86C8C9, dourado #D6B36A, violeta #9777B8
- objeto à parte: o chapéu com medidas, um gancho, um feixe; o chapéu emborcado no chão (estado da q05); o nono feixe
- orçamento (`PIPELINE.md` §4, HIPÓTESE v0): 8 000 tris, 2 materiais, textura 1024, 55 ossos sem osso secundário → chapéu e feixes rígidos no osso da cabeça, sem balanço; feixes como submalha liga/desliga por período, sem material extra
- a aba fica fora da cápsula do `NpcActor`: conferir que ela passa nas portas (vão de 1,8 m, `PIPELINE.md` §3.2) e não atravessa parede de perto
- **custo que esta ficha pede:** 1 animação própria (ajoelhar e colher no chão, C7); 1 componente de velocidade e distância e 1 condição na opção da missão (C5); 9 falas novas (C6 e C8) mais a condição de destino e origem no diálogo; a criatura da q05 com G1 própria (C4, C9)
- dependências: nenhuma de conteúdo; o chapéu emborcado e "chegar devagar" foram aprovados (ADR-0010, adendo, item 10). O código que pedem está no `ELENCO.md`, "Pendências de código"
- testes a registrar: silhueta (≥ 4/5), descrição, troco (com Mara, Tovin e Nilo: a cena da q05 e a da tarde na beira têm de quebrar com qualquer um deles no lugar dela)

**Para o G3:** bloco `### lysa` em `PROVENIENCIA.md` com `g1:` (data, nota, pontuador), `g2:` (data, silhueta n/5, descrição, troco), `entrada:` (concept em `arte/referencias/lysa/` + SHA-256), `ferramenta`, `plano`, `licenca`, `termos_url`, `g3:`. `referencias_terceiros`: nenhuma usada nesta ficha.

## 7. Ligações com outras fichas

**PROPOSTAS desta ficha que envolvem outro personagem:**
- **Tovin:** entra no bosque à tarde pela mesma âncora onde Lysa colhe de costas e não se vira para vê-lo entrar (C7). Na q05 ele está no objetivo `tratar_o_animal` (`q05_o_animal_ferido.json`); esta ficha não define o papel dele. Pedido: Tovin sem chapéu de aba larga.
- **Nilo:** o alcance por cima da linha é dele (`ELENCO.md`, arbitragem 2, item 2). Lysa colhe do lado de fora, de costas para o bosque, sem estender a mão para o outro lado: na mesma tarde e na mesma âncora, ele olha para dentro e ela para fora. As pedras aos 8 foram cortadas.
- **Sera:** aprende com Lysa aos 8 (rotina pós-salto implementada, [PROPOSTA] no `SLICE` B09). Esta ficha não lhe dá chapéu nem feixes: a aba é forma exclusiva de Lysa.
- **Mara:** Lysa abre mão de trança, avental e vestido para sair do default que as duas compartilham hoje; a ficha de Mara decide o dela.
- **Criatura da q05:** o chapéu emborcado a cobre (C4) e ela aparece na porta da ervanaria aos 8 (C9); precisa de ficha G1 de criatura.
- **Padrão do elenco, a conferir:** "o objeto do NPC muda com o estado do jogo" aparece no aro do Borin, no chapéu de Lysa e na lousa de Eira. Que não vire a mesma regra três vezes: no Borin é a marca do jogador; em Lysa, período, missão e destino; em Eira, o ciclo do dia (lousa limpa de manhã, cheia à tarde, apagada à noite).
- **Par desta ficha (Eira):** as duas dizem "não sei". Lysa só diante da coisa nova (das ervas ela tem certeza); em Eira é tique. O redator precisa guardar essa diferença. A cintura marcada, que seria a reserva da Lysa, ficou com a Eira.
- **Aethron:** a verga dele (0,70 m, com ombreiras) e a aba de Lysa (0,72 m) podem ler como "cabeça dentro de moldura" na mesma folha. Pela regra do C4, a zona é dela (parecer, G2 item 2).

**Zonas de silhueta que esta ficha ocupa:**
- faixa larga sobre a cabeça (aba de 0,72 m com borda de 5 cm), com franja pendente;
- base larga e simétrica das coxas à canela, com o vão das canelas aberto;
- ponta dos braços alargada, igual nos dois lados (punhos em funil), com reserva de manopla até o meio do antebraço.

A estatura (1,58 m) não é forma (arbitragem 2, item 6).
