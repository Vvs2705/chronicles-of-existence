# Ficha G1 — `borin`

> Ficha-piloto do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md)).
> Fato com fonte é cânone. **PROPOSTA** = ideia desta ficha, não está nos docs e não vale até ser aprovada.

## 1. Identificação

- **id:** `borin` (`NpcCatalog.cs`; GDD cap. 06, NPC-03)
- **categoria:** Npc, base adulta (`PIPELINE.md` §3.1)
- **onde aparece no slice:** B07 (cotidiano) e missão opcional `q06_o_segredo_do_ferreiro`, objetivos `entrar_na_ferraria` → `ajudar_borin` → `guardar_o_segredo` (`content/quests/q06_o_segredo_do_ferreiro.json`); B15, espada de madeira do treino depois do salto (`SLICE` §1.1 cita a entrega na `ferraria`; quem entrega e em que condição é **PROPOSTA**, ver C9); âncoras `ferraria` (manhã e tarde) e `praca_centro` (noite) (`NpcCatalog.cs`)
- **cânone de partida:**
  - ferreiro exigente e leal; fabricação/metais (dossiê §G); "ferreiro e aprendizado artesanal" (GDD cap. 06)
  - Q-06 tem foco em "profissão e domínio" (GDD cap. 07); existe um segredo e o jogador o guarda (id do objetivo). **O que é o segredo não está escrito em lugar nenhum.**
  - concluir a q06 grava `evento.q06_concluida` e a flag `confianca_de_borin`; Borin lembra (`NpcMemory.cs`). A espada diferente para quem tem a flag (objeto e fala, nunca poder) **não é cânone**: é [PROPOSTA] no `SLICE` B15, repetida no `q06…json`
  - vínculos: Oren é o fornecedor de Borin, e Borin é cliente de Oren (`NpcCatalog.cs`: em `borin`, `Vinculo("oren", "relacao.fornecedor")`; em `oren`, `Vinculo("borin", "relacao.cliente")`); Daren é conhecido de ofício
  - só sabe de `topico.metais`, `topico.ferraria`, `topico.vila_auren`; não sabe do Limiar (`NpcCatalog.cs`)
  - origem `artesaos` traz `oportunidade.oficina_de_borin` e `item.martelo_leve` (`DestinyCatalog.cs`)
  - Karvorn é a região de "montanhas, mineração e metalurgia" (GDD cap. 08; o GDD não diz se Eldoria tem ou não mina); o Bosque dos Sussurros é adjacente a Auren (dossiê §I)
- **autor da ficha / data:** raia D (concept), 2026-09-30; corpo corrigido em 2026-09-30 conforme o parecer (§5, "fatos que não conferiram")
- **estado:** parecer do Art Director em §5; aguardando o idealizador

## 2. Critérios C1–C10

**C1 — Gancho.** O ferreiro que devolve todo trabalho com um "de novo" confere cada peça duas vezes com o polegar porque já não confia nos próprios olhos, e a única pessoa de Auren que sabe disso tem cinco anos. *(PROPOSTA)*

**C2 — Necessidade do mundo.** Cânone: Karvorn é a região de mineração e metalurgia (GDD cap. 08); Oren recebe a carga de manhã no `portao_sul` e é o fornecedor de Borin (`NpcCatalog.cs`). **PROPOSTA (todo o resto deste critério):** em Auren ferro não nasce, chega — Eldoria não tem mina perto e o metal vem de fora, na carga de Oren. Por isso Borin quase nunca forja peça nova; ele **reforja** — a enxada de hoje é a foice de ontem. A umidade do Bosque come o que ele não cuida (ideia tirada do exemplo ilustrativo do ADR-0002, pergunta 1; não está no GDD nem no dossiê). Numa vila sem sobra de metal, peça torta é peça perdida: daí a exigência. E como cada ferramenta já foi de alguém, ele sabe de quem é cada pedaço de ferro da vila: daí a lealdade.

**C3 — Silhueta em 3 formas.** *(PROPOSTA)* Todas presas à malha, para sobreviverem à T-pose e a um idle compartilhado:
1. **Braço-clava:** manga de forja de couro grosso no braço direito, do ombro à luva, com o dobro do volume do esquerdo, que é fino e de manga arregaçada. Em T-pose a assimetria é a primeira coisa que se lê.
2. **Aro vazado:** aro de ferro fechado de 0,28 m pendurado no quadril esquerdo, com o vazio legível em preto e as plaquinhas pendentes. Fica na altura dos olhos de uma criança de 1,10 m: é o que a câmera do jogo mais vê.
3. **Pala sobre pescoço longo:** cabeça raspada, sem barba, com uma pala curta de couro na testa (corta o clarão da forja) em cima de um pescoço comprido e projetado para a frente; tronco estreito e alto, 1,82 m *(PROPOSTA, dentro de 1,55–1,95 do `PIPELINE.md` §3.1)*.
Apoio, não conta como forma: avental só da cintura para baixo, fendido em duas abas.

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **O aro de provas.** Aro fechado com rebite aparente (0,28 m de diâmetro, barra de 12 mm). Nele pendem o **gabarito** (chapa de 0,18 × 0,06 m com 5 entalhes de espessura) e as **provas**: plaquinhas de 0,04 × 0,02 m, uma por pessoa de Auren cuja ferramenta ele reforjou, cada uma com a marca dessa pessoa.
Regra: o aro é a confiança de Borin desenhada no mundo. A prova do jogador aparece no aro **se e somente se** `evento.q06_concluida` está no histórico; a espada de madeira do B15 traz a mesma marca rebitada no punho **se e somente se** `confianca_de_borin` existe. O jogo só lê o histórico; nada é concedido, e a estatística da espada é idêntica à da comum (`SLICE` B15, lá também [PROPOSTA]). Sem a q06, o aro está lá e a prova do jogador não.

**C5 — Regra exclusiva.** *(PROPOSTA, depende do dono do conteúdo da q06)* **Ler o risco.** Borin é o único NPC que nunca aceita na primeira entrega e o único para quem o jogador lê. No objetivo `ajudar_borin`: (1) o jogador entrega a peça; Borin passa o polegar duas vezes, devolve — "De novo." — e o objetivo não fecha; (2) na segunda entrega ele estende o gabarito e o jogador escolhe qual dos entalhes a peça alcança; ele aceita. Leitura errada só repete o passo: sem punição, sem recompensa extra, sem beco sem saída. É essa cena que revela o segredo que o objetivo `guardar_o_segredo` pede para guardar.

**C6 — Voz.** *(PROPOSTA; texto final é do redator de diálogo)* Frases curtas, conta em voz alta, mede em dedos, manda repetir:
1. "De novo. Não tá errado. Eu é que ainda não conferi duas vezes." (cabe em `dialogo.borin.aceitou`)
2. "Ferro aqui não nasce, chega. E chega pouco. Então nada meu sai torto." (cabe em `dialogo.borin.sobre_metais`)
3. "Lê esse risco pra mim. …Foi o que eu disse. Só queria ouvir de outra boca."

**C7 — Contradição visível.** *(PROPOSTA)* Cobra precisão de todo mundo e esconde a própria imprecisão. Observável em três ações que se repetem: devolve a peça do jogador sem olhar para ela (confere com o polegar, não com os olhos); leva qualquer coisa miúda até a porta, para a luz do dia, e finge que foi buscar ar; à noite, na praça (`NpcCatalog.cs`), cumprimenta o jogador pelo passo, antes de virar o rosto.

**C8 — Relação com o avatar por destino e origem.** *(PROPOSTA; só fala e objeto, nunca poder — dossiê §H. Itens e oportunidades citados existem em `DestinyCatalog.cs`)*
- **serena:** a criança chega de tarde livre, com o `item.brinquedo_entalhado`. Borin passa o polegar no brinquedo e aponta onde o entalhe saiu torto: trata-a como visita, e visita não pega em ferro na primeira vez.
- **normal:** a criança traz um recado (`oportunidade.recado_da_vila`). Ele a faz repetir o recado duas vezes, palavra por palavra: o tique dele aplicado a ela.
- **dificil** (rótulo "Vida Árdua", ADR-0007): ele reconhece pelo tato a `item.faca_gasta`, refaz o fio sem cobrar e resmunga que "isso não é favor, é ferro da vila". Trata-a como quem já trabalha (`oportunidade.trabalho_cedo`).
- **ruptura:** o `item.amuleto_rachado` é o único metal que ele se recusa a medir: "Isso não é ferro que eu conheça." Ele não sabe o que é (não tem `topico.limiar`) e não finge saber.
- **origem:** `artesaos` — já conhece a oficina e chega com o `item.martelo_leve`; ele pula a apresentação e cobra mais. `agricultores` — a prova da família já está no aro (a `item.foice_pequena` passou por ele). `guardioes` — ele confere a `item.espada_de_madeira` que a criança já tem e diz que está "fora de medida"; é o que a espada marcada corrige no B15 (só com `confianca_de_borin`), em forma, não em número.

**C9 — Mudança dos 5 para os 8.** *(PROPOSTA)*
- **Antes (5–7):** trabalha no fundo da ferraria, de costas para a porta, à luz do fogo; entalhes do gabarito rasos; 6 provas no aro; fala com a criança de cima, sem se abaixar.
- **Depois (8):** a bancada está na porta, à luz do dia (pode ser a "mudança visível de Auren" que `SLICE` §4.3 pede); os entalhes do gabarito foram refeitos fundos, para o dedo; 9 provas no aro, 10 com a do jogador. Com `confianca_de_borin`, diz em voz alta por que a bancada mudou; sem ela, a bancada mudou e ele não explica (nenhum NPC cita evento ausente, `SLICE` §4.3).
- **Espada do B15 — versão única, PROPOSTA:** Borin só entrega espada a quem tem `confianca_de_borin`: a marcada, na `ferraria`. Sem a flag, Borin não entrega nada e a espada comum vem do próprio treino, no `posto_guarda`. Isso segue o `q06…json` e o `SLICE` B15; o `SLICE` §1.1 lê como entrega na ferraria sem condição e precisa ser alinhado pelo dono do SLICE.
- **Variante de malha:** não. É adulto; muda adereço (provas ligadas/desligadas), posição na cena e fala (dossiê §G: não modelar toda versão etária).

**C10 — Momento de cartaz.** *(PROPOSTA)* Câmera na altura da criança, dentro da ferraria. Contra o clarão do fogo, um homem comprido e curvo devolve uma peça sem olhar: "De novo." O aro tilinta na altura dos olhos da câmera. Ele anda até a porta, estende o gabarito para baixo com o braço grosso — os olhos dele apontam um palmo ao lado da chapa — e pede: "Lê pra mim."

## 3. Amarração

- **Destino, Grau ou Trama:** o destino muda o que a criança traz na mão e como ele a recebe (C8). Grau não se aplica no slice (ascensão fora de escopo, dossiê §E). A Trama não o toca: ele é o adulto de Auren que só acredita no que mede, e o amuleto da Ruptura é a única coisa que ele não mede.
- **Decisão de jogo:** gastar ou não a infância na ferraria antes do salto (a q06 expira aos 7) e aceitar ser devolvido. O que fica é visível aos 8: a prova no aro e a marca na espada. *(PROPOSTA)*
- **Pilar:** viver e crescer.

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| o ferreiro largo, barbudo, de peito de barril e avental inteiro de couro (o default do gerador; é também como o descreve o prompt COE-NPC-003 da bíblia de prompts, `documentos/Chronicles_of_Existence_Biblia_Prompts_Concept_Art_v1_0.md` no checkout principal — acervo **não rastreado no git e não canônico**, citado só como exemplo do default) | comprido, estreito, sem barba, com um braço grosso e um fino |
| martelo ou bigorna como assinatura (todo ferreiro tem) | ferramenta de **medir**: aro, gabarito e provas |
| força como traço central | precisão, tato e memória de quem é dono de cada ferro |
| aro aberto, turquesa, dourado ou violeta (círculo incompleto e essas cores são da Trama, do Limiar e das anomalias, GDD cap. 09) | aro **fechado** com rebite; terracota #A86D52 no couro ("Auren e materiais urbanos", GDD cap. 09), ferro escuro neutro; linho claro em marfim #E9DEC6 é **PROPOSTA** de extensão da paleta (no GDD cap. 09 o marfim é de "textos e pergaminhos"), a decidir no style lock |
| mestre sábio que explica o mundo | sabe de metal, de ferraria e de Auren, e mais nada (`NpcCatalog.cs`) |
| equipamento moderno de oficina | pala, manga e luva de couro, do jeito que Auren faria |

## 5. Avaliação — **parecer do Art Director: 19/20, aprovado com condições; aguardando o idealizador**

> Esta nota é o **parecer do Art Director**. A aprovação final de uma ficha-piloto é do idealizador (Vinicius); até ele decidir, o G1 de Borin **não está aprovado** e nenhum concept é encomendado.

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total /20 |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 1 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | **19** |

- **Corte:** ≥ 14/20 e nenhum zero em C2, C3, C4, C5 — **atingido** (HIPÓTESE do ADR, a recalibrar)
- **Pontuador / data:** Art Director (ficha 06, agente) / 2026-09-30. Não escreveu a ficha.
- **Veredito:** **aprovado (parecer), com duas condições:** (a) narrativa/quest aprovar o segredo e o passo "ler o risco" da q06; (b) aprovação final do idealizador. Se (a) cair, C1, C5, C7 e C10 perdem a base e a ficha volta para nota.

**Nota por critério (1 linha cada):**
- **C1 = 2.** Detalhe concreto (polegar, duas vezes, "de novo") e conflito (esconde a vista; só uma criança de 5 sabe). Todo ele é PROPOSTA.
- **C2 = 2.** Ferro que chega e não nasce, ancorado em três fatos conferidos (Karvorn, carga de Oren no `portao_sul`, vínculo de fornecedor), e os dois traços do catálogo saem dessa condição. Ressalva: a escassez em si é inferência, não cânone (ver abaixo); trocando os nomes, "vila longe da mina" serve a outro jogo. Passa pela régua do ADR, cujo próprio exemplo é do mesmo grau.
- **C3 = 1.** Só o braço-clava é forma de silhueta garantida. O aro fica na frente do quadril e some na massa do corpo em preto (barra de 12 mm a 30% não se lê); a pala "só se prova no perfil", dito pela própria ficha. Para subir a 2: tirar o aro para fora do contorno (ou engrossar a barra) e trocar "pala" por "pescoço projetado + crânio raspado", que é a forma real.
- **C4 = 2.** Objeto com medida e regra escrita com ids (`evento.q06_concluida` → prova no aro e marca no punho). É regra de exibição de estado, não de mecânica; aceito porque a régua pede "regra escrita". As duas condições são uma só (a flag deriva do evento, `QuestCatalog.cs`).
- **C5 = 2.** "Ler o risco" está especificado passo a passo, com falha sem punição. Condicional: é conteúdo da q06 sem dono aprovado, e "único NPC que nunca aceita na primeira" não se verifica com 9 fichas por escrever.
- **C6 = 2.** Três falas escritas, com ritmo próprio. O tique descrito (contar em voz alta, medir em dedos) não aparece em nenhuma das três.
- **C7 = 2.** Três ações recorrentes observáveis. Todas pedem animação própria que não existe; com idle compartilhado, só a da cena da q06 sobrevive.
- **C8 = 2.** Quatro destinos e três origens, com item e oportunidade que conferem no `DestinyCatalog.cs`. Custo: 7 variantes de fala para um NPC de missão opcional.
- **C9 = 2.** Duas batidas marcadas (fundo/fogo → porta/luz; entalhes rasos → fundos; 6 → 9 provas), sem variante de malha.
- **C10 = 2.** Cena escrita e encenável, mas tem ~12 s e dois tempos, não 5 s. O quadro de cartaz é um só: o gabarito estendido para baixo e os olhos um palmo ao lado.

**Fatos dados como cânone que não conferiram:**
1. "a descrição que a bíblia de prompts dá a ele" (§4): não há descrição física de Borin em nenhum documento do repositório. Retirar ou citar o arquivo.
2. "metal se minera em Karvorn, **não em Eldoria** (GDD cap. 08)": o GDD só diz que Karvorn tem mineração e metalurgia. A escassez de ferro em Auren e o ferro na carga de Oren são PROPOSTA de mundo.
3. "sem a flag, o treino usa a espada comum (`SLICE` B15)": no SLICE isso está marcado **[PROPOSTA]**; só o `q06…json` afirma. E há tensão: `SLICE` §1.1 dá a entrega na ferraria sem condição, o json diz "em vez da que Borin entrega", e C9 faz Borin entregar sempre. Falta decidir quem entrega a espada comum.
4. "Oren é fornecedor dele **e cliente dele**": o catálogo diz Borin→Oren `fornecedor` e Oren→Borin `cliente`. A leitura natural é Oren fornece e Borin é o cliente, não o inverso.
5. "a umidade do Bosque" vem do exemplo do ADR-0002, não de GDD nem dossiê.
6. Marfim #E9DEC6 no GDD cap. 09 é "textos e pergaminhos"; usá-lo na camisa é extensão da paleta, a decidir no style lock.

Conferiram: id, traços, âncoras, tópicos e vínculos (`NpcCatalog.cs`); objetivos, expiração aos 7, evento e flag da q06; memória (`NpcMemory.cs`); itens e oportunidades (`DestinyCatalog.cs`); hex e signos da Trama (GDD cap. 09); dossiê §E, §G, §I; `PIPELINE.md` §3.1 e §4; ADR-0007 §2.

**Para o G2 — o que o concept precisa provar:**
1. Silhueta frontal em preto a 30%: o aro lê **fora** do contorno do corpo; se não ler, ele deixa de ser forma e C3 precisa de outra terceira.
2. A assimetria dos braços sobrevive à T-pose e a 8 000 tris sem parecer erro de malha.
3. Render a 1,10 m de câmera, na tela do celular em paisagem: a prova do jogador (0,04 × 0,02 m, textura 1024) é distinguível das outras. Se não for, a regra de C4 não se vê e a prova precisa de cor ou forma própria.
4. Definir qual é a "marca" do jogador na prova e no punho (hoje não está escrita).
5. Aro fechado e sem turquesa/dourado/violeta: não pode ser lido como signo da Trama.
6. Teste do troco com Daren (o outro adulto de ofício): a cena da q06 tem de quebrar.
7. Antes do concept: decisão 9 de `PROJETO.md` (estilo) e ≥ 3 fichas do elenco para o teste de silhueta.

**Lacunas da rubrica (para a recalibração que o ADR pede):** a régua premia o que está escrito e satura em 2 (nove de dez aqui), sem separar ficha boa de ficha completa; não tem nota intermediária em C3 nem diz em que vista a silhueta vale ou se adereço conta como forma; C5 fala em "kit do catálogo" (combate) e foi lido como interação exclusiva; C9 foi escrito para quem cresce e, no adulto, foi lido como mudança de cena, adereço e fala; C4 não diz se exibir estado conta como "fazer algo no jogo"; nada mede custo de produção (animação própria, 7 variantes de fala) nem dependência de conteúdo ainda não aprovado.

## 6. Encaminhamento

**Para o G2, só depois de aprovado:**
- vistas: frente, perfil, costas e 3/4 verdadeiros em T-pose, fundo neutro, linha de chão, altura 1,82 m marcada; a pala só se prova no perfil
- silhueta: braço-clava, aro vazado, pala sobre pescoço longo, em preto a 30%; **e** um render a 1,10 m de altura de câmera, onde o aro é o primeiro plano
- paleta: terracota #A86D52 (manga, avental, pala), ferro escuro neutro; camisa em marfim #E9DEC6 só se o style lock aceitar a extensão (GDD cap. 09: marfim é de "textos e pergaminhos"); reservados e proibidos nele: turquesa #86C8C9, dourado #D6B36A, violeta #9777B8
- objeto à parte: aro 0,28 m, gabarito 0,18 × 0,06 m, prova 0,04 × 0,02 m, e a marca rebitada no punho da espada de madeira
- orçamento (`PIPELINE.md` §4, HIPÓTESE v0): 8 000 tris, 2 materiais, textura 1024, 55 ossos sem osso secundário → aro rígido preso ao quadril, plaquinhas sem balanço, entalhes em forma e não em pintura fina
- o teste de silhueta pede ≥ 3 outros do elenco: o G2 de Borin **não roda sozinho**
- dependências fora desta ficha: o segredo e o passo "ler o risco" são conteúdo da q06 (dono: narrativa/quest); idle próprio (polegar duas vezes) é animação que hoje não existe; o estilo (anime × render 3D estilizado) está pendente em `PROJETO.md`, decisão 9

**Para o G3:** bloco `### borin` em `PROVENIENCIA.md` com `g1:` (data, nota, pontuador), `g2:` (data, silhueta n/5, descrição, troco), `entrada:` (concept em `arte/referencias/borin/` + SHA-256), `ferramenta`, `plano` pago, `licenca`, `termos_url`, `g3:`. `referencias_terceiros`: nenhuma usada nesta ficha.
