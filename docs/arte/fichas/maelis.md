# Ficha G1 — `maelis`

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md)).
> Fato com fonte é cânone. **PROPOSTA** = ideia desta ficha, não está nos docs e não vale até ser aprovada.

## 1. Identificação

- **id:** `maelis` (`NpcCatalog.cs`; GDD cap. 06, NPC-10)
- **categoria:** Npc, base adulta (`PIPELINE.md` §3.1, faixa 1,55–1,95 m)
- **onde aparece no slice:** B07 (cotidiano: manhã no `mural_avisos`, "ouvindo as queixas"; tarde na `praca_centro`, "percorrendo a vila"; noite no `mural_avisos`, "registrando as decisões do dia" — `NpcCatalog.cs`, `strings.pt-BR.json`); B09, `q07_o_desaparecimento`: `notar_a_ausencia` (`praca_centro`) e `perguntar_na_vila` (`mural_avisos`, com Eira e Oren) (`content/quests/q07_o_desaparecimento.json`); B14, lembra a q07 (`SLICE` §4.3). O mural é "o mural de Maelis" (`SLICE` §1.1).
- **cânone de partida:**
  - "administração regional e decisões comunitárias" (GDD cap. 06); "administradora cautelosa; conflitos comunitários" (dossiê §G); traço `traco.cautelosa`; papel "Administradora da vila" (`strings`)
  - vínculos (`NpcCatalog.cs`): em `maelis`, `Vinculo("tovin", "relacao.superiora")` e `Vinculo("eira", "relacao.aliada")`; em `tovin`, `relacao.subordinado`; em `oren`, `Vinculo("maelis", "relacao.contribuinte")`. A chave descreve o papel do dono do vínculo: Maelis manda no posto e recebe a contribuição de Oren.
  - sabe `topico.administracao`, `topico.seguranca_da_vila`, `topico.vila_auren`; ninguém em Auren sabe `topico.limiar` (`NpcCatalog.cs`, `DialogueCatalog.cs`)
  - testemunha `evento.nilo_desapareceu` (é quem dá a notícia), `evento.q07_concluida` e `marco_idade_8` (`NpcMemory.cs`)
  - falas já escritas (`strings`): `ausencia` ("Não quero alarde antes de ter certeza"), `depois_da_busca` ("Esse registro eu não fecho"), `no_mural` ("fale devagar, que eu anoto"), `noite`, `sobre_vila` ("O meu trabalho é lembrar disso quem esquece"), `aos_oito_registro` ("o registro do desaparecimento continua aberto no meu livro"), `aos_oito`. Esta ficha não muda nenhuma.
  - acervo: `COE-NPC-016` é só corpo de referência em macacão marfim (`ACERVO.csv`): mulher madura, cabelo grisalho cacheado, preso. O protótipo do Tripo saiu de "túnica rosa-antigo com debrum dourado, faixa ameixa" (`PROVENIENCIA.md` §6) — dourado é reservado ao Limiar (GDD cap. 09).
- **autor da ficha / data:** Concept Art Lead (agente), 2026-10-03; condições do parecer cumpridas em 2026-10-03 (fim da §5)
- **estado:** aprovado G1 por delegação (ADR-0010, 2026-10-03)

## 2. Critérios C1–C10

**C1 — Gancho.** A administradora que não dá alarme sem certeza e não fecha registro sem resposta carrega Auren escrita numa prancha pendurada no pescoço — e nela, há três anos, a folha do sumiço de Nilo continua de pé, sem deitar no livro. *(cautela e registro aberto: cânone, falas `ausencia` e `aos_oito_registro`; prancha e folha: PROPOSTA)*

**C2 — Necessidade do mundo.** Cânone: Auren é a vila de Eldoria na borda do Bosque dos Sussurros (GDD cap. 08, dossiê §I); tudo chega pela estrada do sul (`dialogo.oren.sobre_comercio`) e ao norte o limite é a entrada do bosque (`SLICE` §1.1) — Auren é fim de estrada; o bosque "não avisa" (`dialogo.tovin.sobre_bosque`); o Limiar é esquecido por quase todos (dossiê §I) e Nilo volta sem saber onde esteve (`dialogo.nilo.aos_oito*`); as estruturas públicas do slice são ferraria, ervanaria e posto (`PIPELINE.md` §3.2) — nenhuma é da administração. O GDD põe o centro político no reino de Eldoria (cap. 08); que ele não fique em Auren é leitura desta ficha, apoiada no Castelo de Eldoria que o dossiê §I cita como lore. **PROPOSTA:** uma vila de fim de estrada, na borda de um bosque que não avisa, num mundo que esquece a própria passagem, erra de dois jeitos: alarme por boato (gente que larga a colheita ou corre para dentro do bosque) e esquecimento do que ninguém soube explicar. Maelis existe contra os dois: escreve antes de decidir e mantém aberto o que não tem resposta. Daí a cautela e daí a pendência. E como Auren não tem casa de administração, o escritório é o que ela carrega: a mesa no pescoço e a vara da vila presa na mesa (C3).

**C3 — Silhueta em 3 formas.** *(PROPOSTA; 1,68 m, dentro da faixa do `PIPELINE.md` §3.1; px pela régua de 30% do `client/tools/silhueta.py`, 120 px/m)* As três saem do contorno e são rígidas, para sobreviver à T-pose e ao idle compartilhado. A câmera do jogo fica atrás e acima da criança — 2,75 m atrás do pivô de 0,94 m, a ~1,65 m de altura com 15° e ~1,32 m em conversa, com 8° (`BodyByAge.cs`, `ThirdPersonCamera.cs`) — e vê Maelis de frente, de um pouco acima da cintura dela:
1. **Nuvem grisalha:** cabelo grisalho cacheado, solto num volume redondo de 0,32 m de largura (38 px, o dobro da cabeça), preso só na nuca. Nada em cima da cabeça. Entre os adultos, é a única cabeça redonda e mais larga que o rosto (a Eira também vai de cabeça descoberta, sem volume; o Nilo deixa a moita, e o volume redondo fica aqui — `ELENCO.md`). Em T-pose, de frente: um círculo maior que a cabeça, sem aba. Massa sólida (toon) rígida na `Head`. Câmera do jogo: de frente, contra o fundo, a qualquer distância.
2. **Vara de ofício:** vara de pau pintado de escuro, 0,90 m × Ø 0,04 m (108 × 5 px), ponteira de ferro, enfiada em duas alças na ponta esquerda da prancha; fica de pé ao lado do corpo, de 0,40 m a 1,30 m do chão — para abaixo do ombro, para não virar muleta em T-pose. Em T-pose, de frente: uma reta vertical fora do contorno, à esquerda do quadril e do tronco, cruzando a ponta da prancha. Rígida no `Chest` junto com a prancha; no idle e no andar, o braço esquerdo passa 0,13 m por dentro dela. Câmera do jogo: uma reta ao lado dela, do joelho ao peito. É vizinha do aro do Borin (lado de fora da coxa): lá é um O, aqui um I mais alto e preso à prancha; o G2 confere (`ELENCO.md`, Arbitragem 2, item 3). É a vara de juiz de vila, de pau, não um cetro: diz que ela fala pela vila e não mede nada (medir é do Borin).
3. **Prancha na cintura:** a prancha-registro (C4), 0,62 m de largura (74 px), com o tampo a 1,00 m do chão, passa 0,11 m (13 px) de cada lado do quadril: em T-pose é uma barra horizontal abaixo dos braços. Rígida no `Chest` pelas alças. Câmera do jogo: vê o tampo de cima nas duas idades da criança (rasante aos 5); o que muda é onde a prancha bate no corpo dela — no rosto aos 5, no peito aos 8 (C9). De frente, a barra é fina; se não ler a 30% no G2, a prancha alarga para 0,70 m ou ganha borda mais grossa antes de trocar a forma.
Leitura em preto: círculo claro em cima, cruz à esquerda (vara × prancha), barra na cintura. Apoio, não conta como forma: tabardo reto de feltro até o meio da coxa, sem cintura marcada. A vara é a forma mais frágil: se não ler, engrossa para Ø 0,05 m antes de qualquer outra mudança.

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **A prancha-registro.** Tábua de 0,62 × 0,34 × 0,02 m com borda levantada de 0,02 m, pendurada do pescoço por duas alças de couro. Sobre ela, presos por uma tira, o **livro de Auren** (0,30 × 0,22 × 0,05 m; o "meu livro" de `aos_oito_registro`) e um tinteiro de chifre no canto direito; no canto de trás, um prendedor de madeira. Registro fechado deita no livro; **registro aberto fica de pé**: a folha dele (0,15 × 0,20 m, marfim) vai presa em pé no prendedor e passa 0,15 m acima do tampo.
Regra — o jogo lê, o jogador vê; nada é concedido e o livro não é item do jogador:
- **Folha do sumiço:** de pé se e somente se `evento.nilo_desapareceu` está no histórico (Maelis já o testemunha, `NpcMemory.cs`). Não deita no slice, nem aos 8: o registro segue aberto (`aos_oito_registro`). Antes da Q-04, o prendedor está vazio. É o **único marco do sumiço que continua depois do salto**: a tira da Mara, o chifre do Tovin e a forquilha do Nilo vão do mesmo evento até o `marco_idade_8`; a folha passa dele (`ELENCO.md`, Arbitragem 2, item 1).
- **Página assinada:** existe se e somente se o objetivo `perguntar_na_vila` da q07 está concluído — é o passo 4 da C5; a q07 só conclui depois, em `seguir_ate_o_bosque`. O sinal na página é o círculo com `evento.q07_assinou_com_o_circulo` e um risco sem ele, sempre em tinta escura (o turquesa do traço é só da tela). Aparece quando ela gira a prancha (C5 e C9).
- **Folha da anomalia (só Vida da Ruptura):** uma segunda folha, amarelada e de cantos enrolados, de pé desde o primeiro dia se `BirthChoice.destinyId == "ruptura"` — lido do save, porque os `Acontecimentos` do destino hoje não vão para o histórico. É o registro do que a Lysa viu e não sabe nomear (o nono feixe, ficha `lysa`): a Lysa viu, a Maelis registrou, uma história só.
Custo: cada folha é um prop rígido preso ao `Chest`, ligado e desligado por script (a "peça ligada por evento" do `ELENCO.md`); NPC não tem osso secundário (`PIPELINE.md` §4).

**C5 — Regra exclusiva.** *(PROPOSTA; depende do dono do conteúdo da q07)* **"Quem conta, assina."** Maelis é a única pessoa de Auren que transforma o que a criança conta em registro assinado, a tinta, que não se apaga. No fim de `perguntar_na_vila`:
1. a criança volta ao mural depois de ouvir Eira e Oren; Maelis escreve ditando em voz alta as três pistas que já são fala — "antes do sol, cama vazia" (`ausencia`), "perguntou o que tem depois da clareira" (`dialogo.eira.lugar_vazio`), "levava pão" (`dialogo.oren.viu_nilo`) — sem acrescentar nada (B09: parciais e não contraditórias);
2. curva o tronco até a altura da criança, gira a prancha e estende a pena;
3. o jogador assina: traça com o dedo o círculo que não se fecha (gesto do avatar, ficha [`avatar`](avatar.md) C5) **ou** escolhe "fazer um risco". Qualquer traço vale: sem erro, sem repetição, sem recompensa extra. Na tela o traço acende em turquesa; no livro, fica em tinta escura;
4. ela sopra a tinta, endireita e o objetivo fecha.
Assinar com o círculo grava `evento.q07_assinou_com_o_circulo` (id novo, PROPOSTA; Maelis testemunha). Acontece uma vez na vida do save. Só ela pede assinatura; só no livro dela fica uma marca do jogador.

**C6 — Voz.** *(PROPOSTA; texto final é do redator)* Dita o que escreve, no formato fato, testemunha, "anotado"; frases curtas em pares paralelos. Bate com as falas escritas (`no_mural`, `ausencia`):
1. "Antes do sol, cama vazia. Testemunha: a mãe. Anotado. Agora me diz o que você viu, não o que você acha."
2. "Alarme eu não dou sem certeza. Registro eu não fecho sem resposta. Uma coisa segura a outra."
3. "Quem conta, assina. Não sabe escrever? Faz um sinal seu. Sinal que é seu, eu reconheço daqui a dez anos."

**C7 — Contradição visível.** *(PROPOSTA)* Pede calma a todos e não larga nada. Observável nos lugares dela, o mural e a praça (`NpcCatalog.cs`); ela não vai à entrada do bosque, que é de Lysa, Tovin, Nilo e da tira da Mara (`ELENCO.md`, Arbitragem 2, item 2):
1. **A tarde parada:** depois de `evento.nilo_desapareceu`, a tarde dela deixa de ser "percorrendo a vila". Ela fica na `praca_centro`, parada, de prancha fechada, voltada para o norte, por cima dos telhados, onde fica o bosque. É uma entrada condicional na **mesma âncora**, `RotinaEntrada(TimeOfDay.Tarde, "praca_centro", "atividade.olhar_o_bosque", "evento.nilo_desapareceu")`, que reaproveita a atividade existente ("olhando o bosque de longe"). Como a âncora não muda, `notar_a_ausencia` (praça, com ela) segue alcançável em qualquer período. A direção do olhar é encenação: se o `NpcActor` não orientar o NPC, fica a prancha fechada.
2. **Ali ela não escreve:** no mural, de manhã e à noite, está no clipe `anotar`; na praça, depois do sumiço, a prancha fica fechada (idle compartilhado). A mulher que escreve tudo para de escrever justamente no lugar onde andava anotando a vila.
3. **A folha de pé**, que ela nunca deita, nem três anos depois.
A mulher que proíbe alarme passa as tardes olhando de longe para onde o menino sumiu.

**C8 — Relação com o avatar por destino.** *(PROPOSTA; só fala e objeto, nunca poder — dossiê §H. Ids conferem em `DestinyCatalog.cs`; destino lido de `BirthChoice.destinyId`)*
- **serena** (`oportunidade.aulas_com_eira`): na assinatura, "Na sua casa todo mundo assina em dia." Aos 8 pede o nome em letras, ao lado do sinal dos 5.
- **normal** (`oportunidade.recado_da_vila`): em `notar_a_ausencia`, acrescenta "Você já leva recado da vila. Este é o mais importante que vai levar." Aos 8 também pede o nome (`aulas_com_eira`).
- **dificil** — Vida Árdua (`evento.ano_de_escassez`, `oportunidade.favor_do_vizinho`): na assinatura, vira para a página da família no ano magro: "Sua casa está aqui também. Favor anotado não é dívida: é a vila lembrando de si." ("todo favor é lembrado", `destino.dificil.contexto_social`). Sem aula com Eira no destino, aos 8 a criança assina de novo com o sinal.
- **ruptura** (`evento.anomalia_no_bosque`): a segunda folha (C4) está de pé desde o ano do nascimento. Se a criança assina com o círculo, Maelis para de escrever pela única vez: "Esse sinal… onde você aprendeu?" Anota "não sabe dizer" e segue. Não sabe o que é e não finge saber. Da vila que "repara em você: uns com cuidado, outros com receio" (`destino.ruptura.contexto_social`), ela é o cuidado.
- **origem:** não muda nada (economia de variantes).

**C9 — Mudança dos 5 para os 8.** *(PROPOSTA)*
- **Antes (5–7):** prendedor vazio até a Q-04, depois uma folha nova, marfim limpo; tardes percorrendo a vila até o sumiço, depois paradas na praça. A prancha (tampo a 1,00 m) fica na altura do rosto da criança de 1,10 m: para a assinatura, Maelis se curva até ela (C5).
- **Depois (8):** a mesma folha, amarelada e de cantos enrolados; o livro mais grosso; o cabelo mais branco (troca de textura); as tardes paradas na praça, três anos depois. A prancha fica na altura do peito da criança de 1,28 m: na fala `aos_oito_registro` ela não se curva, só gira a prancha (o mesmo clipe da C5, cortado no giro), e a página aparece no enquadramento da conversa — o próprio sinal de três anos atrás. Com `evento.q07_assinou_com_o_circulo`, a fala ganha um fecho: "…e o seu sinal ainda está aqui. Não sei ler. Não risquei." Sem ele, vale como está.
- **Variante de malha:** não. Adulta; muda textura e prop (dossiê §G).

**C10 — Momento de cartaz.** *(PROPOSTA)* O mural, durante a q07, com a câmera do jogo atrás e acima de uma criança de 5 anos. Atrás de Maelis, no mural, o aviso de Nilo. Ela termina de ditar, se curva até a criança, gira a prancha pendurada no pescoço e estende a pena; no canto da prancha, uma folha clara está de pé, e a vara, de pé ao lado. "Quem conta, assina." Acontece em todo save (a q07 é central), no mural e na âncora que já existem; o novo é o clipe `curvar_e_oferecer`.

## 3. Amarração

- **Destino, Grau ou Trama:** o destino muda a página que ela mostra, a fala que ela usa e quantas folhas estão de pé (C8). Grau não se aplica no slice (dossiê §E). A Trama não a toca: ela não sabe do Limiar, e o único documento oficial de Auren que guarda o sinal do Limiar é o livro dela, que ela não sabe ler.
- **Decisão de jogo:** assinar o registro com o sinal do Limiar ou com um risco qualquer — pôr no livro da vila a única lembrança que o jogador trouxe da passagem, ou guardá-la. Volta aos 8, na página e na fala. *(PROPOSTA)*
- **Pilar:** escolher e transformar ("decisões registradas e reconhecidas por pessoas, comunidades e mundo", GDD cap. 01).

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| administradora-nobre: túnica longa de cor rica com debrum dourado, faixa, insígnia do reino, postura de corte (a bíblia de prompts pede "roupas de autoridade regional e insígnia de Eldoria" para `COE-NPC-016`, em `documentos/Chronicles_of_Existence_Biblia_Prompts_Concept_Art_v1_0.md` no checkout principal — acervo não canônico; o protótipo saiu de túnica rosa-antigo com debrum dourado) | funcionária de vila sem paço: tabardo reto de feltro com mancha de tinta, nenhuma insígnia, nenhum metal amarelo |
| coroa, tiara, cetro, joia, gola alta e rígida | cabeça descoberta, cabelo grisalho solto; a autoridade é uma vara de pau pintado, presa na mesa de trabalho |
| pergaminho enrolado na mão ou pena como adereço de pose | prancha de escrever presa ao pescoço, sempre a 1,00 m |
| autoridade que ordena e resolve | cautela: não dá alarme sem certeza e não fecha sem resposta (`ausencia`, `depois_da_busca`) |
| a heroína que entra no bosque atrás do menino | não vai nem à beira: olha de longe, da praça, de prancha fechada (C7) |
| sábia que explica o mundo | sabe de administração, segurança e Auren (`NpcCatalog.cs`); do Limiar, anota o sinal sem explicar |
| turquesa, dourado, violeta (Trama, Limiar, anomalias, GDD cap. 09) ou azul profundo #253850 (menus e arcano) | marfim #E9DEC6 nas páginas e nas folhas de pé — o uso que o GDD já dá ao marfim, "textos e pergaminhos"; terracota #A86D52 nas alças ("Auren e materiais urbanos"); feltro cinza-tinta e vara escura, neutros, PROPOSTA a decidir no style lock |

## 5. Avaliação — parecer do Art Director: 20/20, aprovado (ADR-0010)

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total /20 |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | **20** |

- **Corte:** ≥ 14/20 e nenhum zero em C2, C3, C4, C5 — **atingido** (HIPÓTESE do ADR, a recalibrar)
- **Pontuador / data:** Art Director (agente) / 2026-10-03. Não escreveu a ficha.
- **Veredito:** **aprovado por delegação (ADR-0010)**, com duas condições antes do concept:
  - **(a) C7 (item 1) e C10 saem da `entrada_bosque`.** A tarde ali já é palco da Lysa (ajoelhada, rotina canônica), do Tovin (2 m depois da linha), do Nilo (pés na linha; aos 8, olhando o bosque) e da tira da Mara. Maelis seria a quarta pessoa no lugar e a segunda mulher parada na linha olhando para dentro. E "a única adulta que vai todo dia olhar o bosque" não confere. Pelo cânone ela fica no mural e na praça. A contradição pode ficar na folha de pé e no "ali ela não escreve", encenados lá. Sem isso, o troco Lysa × Maelis na linha só quebra pelo adereço, não pelo gesto.
  - **(b) Aprovações de fora da ficha.** O dono da q07 aprova a assinatura em `perguntar_na_vila` (o B09 é beat aprovado) e o id novo `evento.q07_assinou_com_o_circulo`, que entra no contrato de save (testemunha no `NpcMemory.cs`, paridade no `q07…json`). E a ficha `avatar` mantém o traço. Se a assinatura cair, C5 cai inteiro, C4, C9 e a decisão de jogo perdem a página, e a ficha volta para nota.
- A nota mede a ficha sozinha. Fora de C3, a régua não tem coluna para colisão entre fichas (lacuna já apontada no parecer do Borin): o palco dividido entra como condição, não como ponto.

**Nota por critério (1 linha cada):**
- **C1 = 2.** Tem detalhe (a prancha no pescoço, a folha de pé que não deita) e tensão entre as duas falas canônicas ("sem alarde" × "não fecho").
- **C2 = 2.** A melhor C2 das três. Fim de estrada, bosque que não avisa e um mundo que esquece o Limiar dão dois erros possíveis (o alarme e o esquecimento), e ela existe contra os dois. Só serve a Eryndor. "O centro político fora da vila" é leitura (ver "fatos").
- **C3 = 2.** As três formas saem do contorno e são rígidas. A nuvem é única entre os adultos, e a vara e a prancha fazem uma cruz à esquerda que nenhuma outra ficha tem. Ressalvas: de frente, a prancha é uma barra de ~5 px que passa só 13 px de cada lado do quadril (forte de perfil e de 3/4, fraca de frente). A vara (um I) fica do mesmo lado e na mesma faixa de altura do aro do Borin (um O). A nuvem é a moita do Nilo em escala de adulto. Nenhuma é colisão clara.
- **C4 = 2.** Objeto com medidas e três regras com ids. A folha de pé é a fala canônica `aos_oito_registro` virada objeto. A regra da página assinada lê o evento errado (ver "fatos").
- **C5 = 2.** A melhor C5 das três: técnica exclusiva, passo a passo, sem erro possível, com uma escolha real (o sinal do Limiar ou um risco) que volta aos 8. Usa as três pistas canônicas sem acrescentar nada e cumpre o "me conte o que ouvir" da fala `ausencia`.
- **C6 = 2.** Ditado em pares paralelos, coerente com as falas `no_mural` e `ausencia`.
- **C7 = 2.** Observável (a rotina, o clipe que não toca, a folha de pé), com o custo declarado. O palco é a condição (a).
- **C8 = 2.** Quatro destinos com ids que conferem. Na Ruptura, ela para de escrever pela única vez: é a melhor reação ao sinal entre as três fichas.
- **C9 = 2.** Folha nova que amarela, livro mais grosso, cabelo mais branco, o fecho da fala: batidas marcadas, sem malha nova. A batida "aos 5 não vê a página, aos 8 vê" não acontece na câmera do jogo (ver "fatos").
- **C10 = 2.** Plano escrito e encenável. O palco é a condição (a). A continuação "depois dos 5 s" não acontece no jogo: a assinatura é no mural, com a q07 em andamento, e ela só vai à entrada do bosque depois de `evento.q07_concluida`.

**Fatos dados como cânone que não conferiram:**
1. C3, "a única adulta do elenco de cabeça descoberta (… rodilha de Mara, lousa de Eira …)": depois da arbitragem do `ELENCO.md`, a rodilha saiu e a lousa desceu, e a Eira está de cabeça descoberta. A forma (volume grisalho redondo) continua só dela entre os adultos. Já a ficha `nilo` chama os lados da cabeça de "a única zona livre".
2. C3, C9 e §6, "a criança de 5 (olhos a 0,94 m) vê a prancha por baixo… a de 8 (1,09 m) vê a página": 0,94 e 1,09 m são o **pivô** da câmera (`BodyByAge.PivoCamera`), não a câmera. A câmera fica 2,5 alturas atrás e acima: ~1,65 m aos 5 com 15°, ~1,32 m em conversa (pitch 8°, `ThirdPersonCamera.cs`) e ~1,5 a 1,9 m aos 8. Nas duas idades ela vê o tampo de cima (rasante aos 5). A ficha `avatar` faz a mesma conta.
3. C7, "a única adulta que vai todo dia olhar o bosque": Lysa e Tovin estão na `entrada_bosque` toda tarde (`NpcCatalog.cs`), e Tovin "volta lá todo dia" (`depois_da_busca`).
4. C4, "Página assinada: existe se e somente se `evento.q07_concluida`": a assinatura acontece no fim de `perguntar_na_vila`, e a q07 só conclui em `seguir_ate_o_bosque`. Entre os dois momentos, a página assinada não existiria. A regra deve ler o evento novo (ou o objetivo concluído).
5. C2, "o centro político de Eldoria fica fora da vila (GDD cap. 08)": o GDD lista Eldoria como "Auren, estradas, academia e centro político". Que o centro não seja Auren é leitura (o dossiê §I cita um Castelo de Eldoria, como lore).
6. §6, "o `Talking` compartilhado não serve": não há clipe Talking no projeto (`PIPELINE.md` §7.2; `PrototipoAnimacoes.cs`: Idle, Walk, Run, ataques, Dodge, Hit, Death). O custo dos dois clipes continua de pé.
7. §7, nota (1) ao coordenador sobre `fornecedor`/`cliente`: já corrigido no código em 2026-10-03 (`NpcCatalog.cs`: borin → oren `relacao.cliente`; `ELENCO.md`).

Conferiram: id, traço, papel, rotina, vínculos (`superiora` e `aliada`, com os recíprocos `subordinado` e `contribuinte`) e tópicos (`NpcCatalog.cs`); testemunhos de `nilo_desapareceu`, `q07_concluida` e `marco_idade_8` (`NpcMemory.cs`, `q04…json`, `q07…json`); falas e nós (`strings.pt-BR.json`, `DialogueCatalog.cs`); as três pistas do ditado (`ausencia`, `lugar_vazio`, `viu_nilo`); `atividade.olhar_o_bosque` existe; `RaioDaVaga` de 1,5 m (`NpcActor.cs`); q08 sem NPC; `SLICE` §1.1, B09 e §4.3; GDD cap. 01, 06, 08 e 09; dossiê §G e §I; protótipo (`PROVENIENCIA.md` §6) e `COE-NPC-016` (`ACERVO.csv`; o grisalho cacheado aparece na imagem, preso, não solto); `PIPELINE.md` §3.1 e §4.

**Sobre o sumiço de Nilo em cinco objetos (pergunta do `ELENCO.md`):** é a vila reagindo enquanto cada um tem verbo e palco próprios. Maelis registra (prancha, mural), Eira dá a pista (lousa, praça), Tovin se cala (chifre, rua norte), Nilo deixa a forquilha (clareira). Vira o mesmo truque em dois pontos:
1. "O marco que fica para sempre" aparece três vezes: a folha que não deita, a pergunta que a Eira nunca apaga e a tira da Mara. Recomendo que fique só com a Maelis, que tem o cânone ("Esse registro eu não fecho").
2. A linha da entrada do bosque junta marco e gente demais: tira da Mara, pedras da Lysa, Lysa, Tovin, Nilo e Maelis. Saem a Maelis (condição (a)) e um dos dois marcos na linha. Recomendo cortar as pedras da Lysa, porque a tira é o C4 da Mara.

Na Ruptura, a folha da anomalia e o nono feixe da Lysa marcam o mesmo evento canônico. Ligados ("a Lysa viu, a Maelis registrou"), contam uma história só, em vez de dois marcos soltos.

**Para o G2 — o que o concept precisa provar:**
1. **A prancha de frente:** as pontas de 0,11 m × ~0,04 m precisam ler como barra. Se não lerem, alargar a prancha (≥ 0,70 m) ou engrossar a borda antes de trocar a forma.
2. **Na mesma folha que o Borin e o Nilo:** a vara (I, de 0,40 a 1,30 m) e o aro (O, de ~0,80 a 1,08 m) ficam do mesmo lado; a nuvem e a moita têm a mesma forma em escalas diferentes.
3. **A vara** de Ø 0,04 m passa na abertura do `silhueta.py` (≥ 7 px a 512 px) e não vira muleta no idle. Rígida no `Chest`, acompanha o `curvar_e_oferecer` sem bater no chão.
4. **Render na câmera real** (pivô a 0,94 m, 2,75 m atrás; 8° em conversa), não na altura do olho da criança, mostrando o que aparece da prancha aos 5 e aos 8. Se a página aparecer aos 5, a batida de C9 sai ou ganha enquadramento próprio (o giro da prancha na C5 já é um).
5. **A página assinada** leva o sinal em tinta escura. O turquesa do traço é da tela (ficha `avatar`, C5), não do livro: turquesa é proibido nela.
6. **Troco com a Eira,** válido depois da recomendação da ficha `eira` (a lousa sem a pergunta de Nilo aos 8).

**Condições cumpridas em 2026-10-03:** *(pelo autor da ficha; o parecer acima não foi alterado)*
- **(a) C7 e C10 saem da `entrada_bosque`:** feito. C7 encena a contradição na praça e no mural: a tarde parada é uma entrada condicional na **mesma âncora** `praca_centro` ("olhando o bosque de longe"), a prancha fica fechada só ali e a folha continua de pé. O cartaz (C10) virou a assinatura no mural, que acontece em todo save. C9 e §4 acompanham. Saiu "a única adulta que vai todo dia olhar o bosque".
- **(b) Aprovações de fora da ficha:** continuam pendentes com o dono da q07 (o passo da assinatura em `perguntar_na_vila` e o id `evento.q07_assinou_com_o_circulo`). A ficha `avatar` mantém o traço (C5 revisado em 2026-10-03, de pé, até o vão).
- **Fato 1 (cabeça descoberta):** C3 agora diz que a Eira também vai de cabeça descoberta e que a forma exclusiva é o volume redondo. Saíram da §2 a "rodilha da Mara" e a "lousa da Eira sobre a cabeça". O volume redondo fica com Maelis; o Nilo deixa a moita (`ELENCO.md`).
- **Fato 2 (câmera):** C3, C9 e §6 usam a câmera real, atrás e acima (~1,65 m com 15°, ~1,32 m em conversa), que vê o tampo nas duas idades. A batida de C9 passou a ser onde a prancha bate na criança (rosto aos 5, peito aos 8) e se Maelis precisa se curvar; a página aparece pelo giro da prancha, no enquadramento da conversa.
- **Fato 3:** saiu junto com (a).
- **Fato 4 (página assinada):** C4 lê o objetivo `perguntar_na_vila` concluído, não `evento.q07_concluida`; o sinal desenhado lê `evento.q07_assinou_com_o_circulo`.
- **Fato 5 (centro político):** C2 marca como leitura desta ficha, com o Castelo de Eldoria do dossiê §I.
- **Fato 6 (`Talking`):** saiu da §6. O custo dos dois clipes fica, com o motivo certo: o conjunto atual não tem gesto de escrever.
- **Fato 7 (`fornecedor`/`cliente`):** a nota saiu da §7; já está corrigido no código.
- **Arbitragem 2 do `ELENCO.md`:** item 1 — a folha é o único marco do sumiço que passa do salto, com gatilho `evento.nilo_desapareceu` (C4); item 2 — Maelis fora da `entrada_bosque` (C7, C10); item 3 — a vara (I) fica como vizinha do aro do Borin e vai ao G2 na mesma folha (C3, §6).
- **G2, itens 1 e 5 do parecer:** a alternativa para a prancha fraca de frente (≥ 0,70 m) entrou em C3; o sinal em tinta escura no livro entrou em C4 e C5.
- **Ruptura:** a folha da anomalia passa a ser o registro do nono feixe da Lysa (C4, §7), uma história só, como o parecer recomenda.

**Conferência final (Art Director, 2026-10-03):** pendente: condição (b). O dono da q07 ainda não aprovou a assinatura em `perguntar_na_vila` nem o id `evento.q07_assinou_com_o_circulo` (testemunha no `NpcMemory.cs`, paridade no `q07…json`). Até aprovar, o C5 continua condicional, e com ele a página de C4 e de C9. A condição (a), os fatos 1–7 e a Arbitragem 2, itens 1, 2 e 3, estão atendidos no corpo. Ressalva, sem pendência: a tarde parada na praça (C7, C9) lê `evento.nilo_desapareceu` e continua depois do salto. Não é objeto, mas é um segundo sinal do sumiço que passa do salto, além da folha.

## 6. Encaminhamento

**Para o G2 (concept), só depois de aprovado:**
- vistas: frente, perfil, costas e 3/4 em T-pose, fundo neutro, linha de chão, altura 1,68 m marcada e tampo da prancha a 1,00 m
- silhueta: nuvem grisalha, vara de ofício e prancha em preto a 30% (`client/tools/silhueta.py`, 120 px/m), na mesma folha que Borin (vara × aro) e Nilo; **e** um render na câmera real do jogo — pivô a 0,94 m, 2,75 m atrás, 15° andando e 8° em conversa — com a criança de 5 e a de 8 a 1,5 m dela: a folha de pé tem de ler nos dois, e o render mostra o que aparece do tampo em cada idade
- paleta: feltro cinza-tinta (tabardo), cabelo grisalho, vara escura com ponteira de ferro, terracota #A86D52 (alças), marfim #E9DEC6 (páginas, folhas), tinta escura no sinal da página; sai o debrum dourado do protótipo; proibidos: turquesa #86C8C9, dourado #D6B36A, violeta #9777B8, e azul profundo #253850 na roupa
- objeto à parte: prancha 0,62 × 0,34 × 0,02 m (e a alternativa de 0,70 m), livro 0,30 × 0,22 × 0,05 m, tinteiro, prendedor, folha de pé 0,15 × 0,20 m (nova e amarelada), vara 0,90 m, e a página com os dois sinais possíveis (círculo aberto; risco)
- orçamento (`PIPELINE.md` §4, HIPÓTESE v0): 8 000 tris, 2 materiais, textura 1024, 55 ossos sem secundário → cabelo, vara e prancha rígidos; folhas como props presos ao osso; a vara acompanha o `curvar_e_oferecer` sem bater no chão
- custo declarado: **2 clipes próprios** — `anotar` (loop com as mãos na prancha) e `curvar_e_oferecer` (C5), mais o corte dele só no giro, aos 8; o conjunto atual (Idle, Walk, Run, ataques, Dodge, Hit, Death; `PIPELINE.md` §7.2) não tem gesto de escrever. **~9 strings** (3 de voz, 4 de destino, o ditado e o fecho dos 8); **1 id novo** (`evento.q07_assinou_com_o_circulo`, com testemunha em `NpcMemory.cs` e paridade no `q07…json`); **1 entrada de rotina** (praça, mesma âncora)
- testes: silhueta com Borin, Nilo, avatar, Eira e Lysa (≥ 4/5); descrição; troco com Eira (a outra adulta letrada e aliada): a cena da assinatura tem de quebrar com ela no lugar — Eira lê, Maelis escreve a tinta e não apaga
- dependências: o passo da assinatura é conteúdo da q07 (dono: narrativa/quest); o sinal é o traço do avatar (ficha [`avatar`](avatar.md))

**Para o G3:** bloco `### maelis` em `PROVENIENCIA.md` com `g1:` (data, nota, pontuador), `g2:` (data, silhueta n/5, descrição, troco), `entrada:` (concept em `arte/referencias/maelis/` + SHA-256), `ferramenta`, `plano`, `licenca`, `termos_url`, `g3:`. `referencias_terceiros`: nenhuma usada nesta ficha. O protótipo atual (`estado: PROTOTIPO`) não serve de entrada.

## 7. Ligações com outras fichas

PROPOSTAS desta ficha que envolvem outro personagem:
- **avatar:** o sinal da assinatura é o traço do avatar (ficha `avatar`, C5), em tinta escura no livro; a prancha bate no rosto da criança aos 5 e no peito aos 8.
- **Eira:** Maelis lê de volta, no ditado, uma pista que já é fala dela (`dialogo.eira.lugar_vazio`). "Maelis escreve, Eira lê" (ficha `eira`) continua; a lousa da Eira deixou de marcar o sumiço (`ELENCO.md`, Arbitragem 2, item 1), então só a folha de Maelis guarda a pergunta. Maelis não leva lousa, armação nem carga nas costas. De noite as duas estão no mural (rotinas já publicadas).
- **Oren:** Maelis lê de volta `dialogo.oren.viu_nilo`; nada novo é atribuído a ele.
- **Nilo:** deixa a moita; o volume redondo de cabelo fica com Maelis (`ELENCO.md`). Os dois não dividem âncora.
- **Mara, Tovin, Nilo:** a tira, o chifre calado e a forquilha marcam o sumiço até o salto; a folha de Maelis é o único marco que passa dele (C4).
- **Lysa:** na Ruptura, a folha da anomalia é o registro do nono feixe da Lysa — a Lysa viu, a Maelis registrou. A tarde de Maelis fica na praça, longe da linha do bosque.
- **Borin:** a vara (I) e o aro (O) ficam do mesmo lado; o G2 confere na mesma folha.
- **família do avatar (Mara/Daren), dificil:** o nome da família está numa página do ano de escassez, como favor recebido, nunca como dívida.

Zonas de silhueta que esta ficha ocupa: **volume redondo de cabelo, mais largo que a cabeça, sem nada em cima (único entre os adultos); reta vertical fora do contorno, à esquerda, do joelho ao peito (vara; vizinha do aro do Borin, `ELENCO.md` Arbitragem 2, item 3); barra horizontal na frente da cintura, mais larga que o quadril, a 1,00 m (prancha; `ELENCO.md`, arbitragem 4).**
