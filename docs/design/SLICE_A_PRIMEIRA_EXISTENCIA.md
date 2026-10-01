# A Primeira Existência — roteiro jogável do vertical slice

> **Estado (2026-09-30): roteiro de projeto. T002–T011 aceitas e a fiação da T012 feita; falta conteúdo, e nada foi jogado.**
> O estado de cada sistema está em `docs/PROJETO.md` §6. Este documento é a especificação que a T012 consome.
> Decisões posteriores que mudam beats: `docs/adr/ADR-0005-fonte-unica-das-missoes-e-desaparecimento.md` e
> `docs/adr/ADR-0007-decisoes-da-leva-a.md` (anotadas em B02, B04, B06, B07 e B09).
> Os dados correspondentes vivem em `content/quests/*.json` e são conferidos por
> `content/quests/validate_quests.py` — validação de **dados**, não de jogo.
>
> **Fontes:** `docs/direcao/DOSSIE_CONTINUIDADE_v1_0.md` §D, §G, §H, §I, §L, §M ·
> `docs/gdd/GDD_MESTRE_v1_2.md` 06, 07, 08, 11, 12 · `docs/backlog/BACKLOG_v1_1.md` ·
> `docs/tech/CONTRATO_T003_T004.md` · `docs/adr/ADR-0004-destino-nao-e-dificuldade.md` · `CLAUDE.md`.
>
> **Convenção de marcação:** **[APROVADO]** = está no dossiê ou no GDD v1.2 e é reproduzido aqui.
> **[PROPOSTA]** = decisão de design deste documento, precisa de aprovação do coordenador antes de virar código.

---

## §1 — Âncoras que o roteiro exige da T008

Uma âncora é um `GameObject` vazio em `Ancoras/<id>` na cena Auren. É o contrato entre este roteiro
(T012), a missão (T006), o NPC (T007) e a cena (T008). **ID publicado não muda sem migração.**
A cópia que o validador confere está em `content/quests/_schema.json` → `ancoras_validas`.

### 1.1 Já existem em `client/Assets/_COE/Editor/AurenSceneBuilder.cs` (T008, aceita)

| Âncora | Uso neste roteiro |
|---|---|
| `spawn_player` | Onde o personagem de 5 anos entra no mundo (B06) e reentra depois do salto (B14). |
| `portao_sul` | Limite sul; nada do slice acontece além dele. Fecha o mapa sem parede invisível narrativa. |
| `casa_familia` | Mara e Daren. Beats B06, B07; missões q01, q02. |
| `casa_nilo` | Nilo. Beat B08; missão q04. |
| `casa_sera` | Sera. Beat B08; missão q04. |
| `praca_centro` | Centro social: q02, q03, q04 (decisão), q07 (notar a ausência). |
| `mural_avisos` | Mural de Maelis: q07 (perguntar na vila). |
| `ferraria` | Borin; missão q06; entrega da espada de madeira no pós-salto (B15). |
| `ervanaria` | Lysa; missão q05. |
| `posto_guarda` | Tovin; **treino supervisionado pós-salto (B15)**. |
| `entrada_bosque` | Limite navegável ao norte: q03, q05, q07 (seguir até o bosque), B10. |
| `bosque_clareira` | Símbolo do Limiar; missão q08; beats B11–B13. |
| `horta_familia` | Canteiro atrás de `casa_familia`; objetivo `procurar_na_horta` de q03. |

### 1.2 Pedido à T008 — atendido em 2026-09-29

A T008 aceitou a proposta: `horta_familia` existe na cena, com um canteiro atrás de `casa_familia` (tabela acima). O fallback para `casa_familia` não é mais necessário.

### 1.3 Fora de Auren

O prólogo (B01–B05) acontece na cena `TheLiminalRealm` e na tela de criação, que **não têm âncoras de Auren**
e por isso não entram na lista acima nem no validador. É deliberado: o validador só conhece o mundo de Auren.

---

## §2 — Roteiro beat a beat

Legenda de colunas: **Sistema** = a raia que o beat exercita. **Avanço** = o que faz o jogo passar ao próximo beat.
**Se ignorar/atrasar** = o que acontece quando o jogador não coopera. **Aceite observável** = o que precisa
**aparecer na tela** para o beat contar como funcionando (é isto que vira passo de teste na T014).

---

### B01 — O Limiar **[APROVADO]** (dossiê §I, §L)

- **Jogador:** desperta sem corpo num espaço que não é lugar; encontra Aethron, Guardião do Limiar. Só escuta e responde; não anda, não luta.
- **Onde:** cena `TheLiminalRealm`.
- **Sistema:** diálogo linear (pré-T007, pode ser fala fixa). O **símbolo da Trama** (fios entrelaçados, círculo incompleto — GDD 09) é plantado aqui em plano fechado.
- **Avanço:** diálogo chega ao fim.
- **Se ignorar/atrasar:** não há como; o beat é a única interação disponível. Sem timer.
- **Aceite observável:** o símbolo aparece em tela ao menos uma vez antes da escolha de destino, e o jogador pode reler a última fala (sem avanço automático que roube a leitura).
- **Guarda-corpo:** Aethron **não** é onisciente nem benigno por decreto (dossiê §I). As falas não podem prometer que o jogador é o escolhido.

---

### B02 — Os quatro destinos **[APROVADO]** (dossiê §C, ADR-0004)

- **Jogador:** vê as quatro condições de nascimento — Vida Serena (`serena`), Vida Normal (`normal`), Vida Árdua (`dificil`; rótulo do ADR-0007 §2, no lugar de "Vida Difícil"; o id não muda), Vida da Ruptura (`ruptura`) — e escolhe uma.
- **Onde:** `TheLiminalRealm`.
- **Sistema:** T003 (`BirthChoice.destinyId`).
- **Avanço:** destino selecionado (ainda **não** confirmado: `confirmedAtUtc == 0`).
- **Se ignorar/atrasar:** pode voltar e trocar à vontade **antes** de B05. Nenhum estado gravado ainda.
- **Aceite observável:** a tela descreve os quatro destinos em termos de **circunstância de vida** (família, oportunidades, acontecimentos) e **em nenhum lugar** usa as palavras fácil/difícil/extremo como nível de desafio. Um texto visível diz que a escolha será permanente.
- **Exploit coberto:** ADR-0004 e dossiê §C — destino não é dificuldade; rotular assim reabre o exploit de começar no fácil e migrar.

---

### B03 — As três origens compatíveis **[APROVADO]** (dossiê §C)

- **Jogador:** recebe três origens condicionadas ao destino — `agricultores`, `artesaos`, `guardioes` — e escolhe uma.
- **Onde:** `TheLiminalRealm`.
- **Sistema:** T003 (`BirthChoice.originId`).
- **Avanço:** origem selecionada.
- **Se ignorar/atrasar:** pode voltar a B02 e trocar destino; a origem selecionada é descartada junto.
- **Aceite observável:** as três origens mudam o **texto** de família, casa e ocupação de Mara e Daren; as 12 combinações (4×3) produzem texto sem buraco e sem placeholder cru na tela.

---

### B04 — Personalização **[APROVADO]** (dossiê §C)

- **Jogador:** define nome e aparência dentro da base corporal infantil (GDD 09: três bases; aqui só a de criança).
- **Onde:** tela de criação.
- **Sistema:** T003 (`BirthChoice.characterName`) + Character.
- **Avanço:** nome válido informado.
- **Se ignorar/atrasar:** nome vazio ou só espaços é rejeitado com mensagem; nunca aceita em silêncio.
- **ADR-0007 §5:** a aparência saiu do slice (FUTURO). O B04 fica só com o nome.
- **Aceite observável:** nome com acento (ex.: "Íris") sobrevive intacto ao salvar e reaparece igual na tela depois de carregar (contrato T003×T004, invariante 3).

---

### B05 — A confirmação do nascimento **[APROVADO]** (dossiê §C; contrato T003×T004)

- **Jogador:** confirma. É o ponto sem volta do prólogo.
- **Onde:** tela de criação.
- **Sistema:** T003 grava `confirmedAtUtc > 0`; T004 cria o save v1 com `ageYears = 5`.
- **Avanço:** confirmação explícita, com um segundo passo de "tem certeza" que enuncia a permanência.
- **Se ignorar/atrasar:** cancelar volta para B02 sem gravar nada.
- **Aceite observável:** depois da confirmação, **nenhuma tela do jogo** oferece trocar destino ou origem. Existe um arquivo de save em disco antes do primeiro carregamento de Auren.
- **Exploit coberto:** dossiê §M, "destino permanente"; backlog teste 1.

---

### B06 — Despertar aos cinco anos · missão `q01_um_novo_amanhecer` **[APROVADO]** (dossiê §D, §L)

- **Jogador:** acorda em `casa_familia`, fala com Mara e Daren, sai para a rua.
- **Onde:** `casa_familia` → `spawn_player`.
- **Sistema:** T002 (andar/interagir), T006 (missão), T007 (NPC), T008 (cena), T005 (evento de vida).
- **Avanço:** três objetivos em ordem: `acordar` → `falar_com_familia` → `sair_de_casa`.
- **ADR-0007 §6:** `acordar` se cumpre sozinho quando a q01 começa; o primeiro ato do jogador é falar com a família.
- **Se ignorar/atrasar:** se o jogador ficar no quarto, a missão fica Ativa e nenhuma outra abre. Não é softlock: o objetivo pendente está sempre no mesmo cômodo e a família repete o gancho. Sem timer.
- **Aceite observável:** o HUD mostra o objetivo pendente; concluir grava `marco.primeiro_dia` no histórico e a q02 passa a estar disponível com Daren.

---

### B07 — O cotidiano de Auren · `q02` central + `q03`, `q05`, `q06` opcionais **[APROVADO]** (dossiê §D, §H, §L)

- **Jogador:** cumpre a tarefa de Daren (q02) e, se quiser, ajuda Oren a achar o cesto (q03), cuida do animal com Lysa (q05) e trabalha com Borin (q06). Conhece a vila e as rotinas.
- **Onde:** `praca_centro`, `horta_familia`, `entrada_bosque`, `ervanaria`, `ferraria`.
- **Sistema:** T006, T007 (rotina + memória), T009 (tempo do dia, afinidade), T010 (confiança por NPC).
- **Avanço:** `q02` concluída. As três opcionais **não** são condição de nada.
- **Como o dia passa (ADR-0007 §1):** o período (manhã → tarde → noite) avança um passo ao concluir uma missão e ao escolher Descansar em `casa_familia`, e só nesses casos. Sem relógio de tempo real.
- **Se ignorar/atrasar:** as três opcionais permanecem disponíveis até o salto temporal e entram na lista "o que se encerra" de B12. Ignorá-las muda fala e objeto depois do salto, **nunca** poder de combate (dossiê §H: dinheiro/favor não substitui domínio).
- **Aceite observável:** é possível chegar a `q08` tendo concluído **zero** missões opcionais — verificado como caminho jogável, não como opinião. Repetir a entrega da q02 não paga moedas de novo.
- **Exploit coberto:** backlog testes 6 (opcional não bloqueia) e 3 (recompensa idempotente); dossiê §M.
- **Limite de farming [PROPOSTA]:** as opcionais são **de instância única** — concluídas, não reabrem. O ganho repetível de domínio é assunto da T009/T011, não da missão; nenhuma missão deste slice concede atributo diretamente.

---

### B08 — A decisão com consequência · `q04_uma_promessa` **[APROVADO]** (dossiê §L; GDD 11)

- **Jogador:** Sera pede algo; o jogador decide; a decisão é sustentada (ou não) diante de Nilo.
- **Onde:** `casa_sera` → `praca_centro` → `casa_nilo`.
- **Sistema:** T006, T007, T010, T005.
- **Avanço:** `sustentar_a_escolha` concluído. Exatamente **um** entre `evento.q04_promessa_cumprida` e `evento.q04_promessa_quebrada` é gravado.
- **Se ignorar/atrasar:** a missão fica Ativa indefinidamente; Sera e Nilo mantêm o gancho. `q07` não abre enquanto isso.
- **Aceite observável:** depois de salvar e carregar, Sera e Nilo dizem falas **diferentes** conforme o evento gravado, e o histórico de vida mostra um único registro da promessa.
- **Por que este beat existe:** é a hipótese de playtest "escolhas visíveis" do dossiê §L. Se esta diferença não for perceptível ao jogador, o slice falhou no seu próprio critério.

---

### B09 — O desaparecimento · `q07_o_desaparecimento` **[APROVADO]** (dossiê §L; quem desaparece: ADR-0005)

- **Jogador:** nota que alguém não está onde devia, pergunta na vila e recebe de Tovin a direção do bosque.
- **Onde:** `praca_centro` → `mural_avisos` → `entrada_bosque`.
- **Sistema:** T006, T007 (memória: quem viu o quê), T010.
- **Avanço:** `seguir_ate_o_bosque` concluído.
- **Se ignorar/atrasar:** a pessoa continua desaparecida; nada expira. `q08` não abre.
- **Aceite observável:** três NPCs diferentes (Maelis, Eira, Oren) dão informações **parciais e não contraditórias**; o mural mostra o aviso.
- **Quem desaparece: Nilo** (ADR-0005, decisão 2): é o alvo da promessa em `q04`, e a decisão de B08 volta como custo.
- **ADR-0007 §3:** o evento de vida `evento.nilo_desapareceu` é gravado junto da conclusão da Q-04. Enquanto ele valer, Nilo não aparece em Auren (rotina na âncora-sentinela `ausente`), e a Q-03, que pede Nilo na trilha, é encerrada se ainda estiver aberta. O retorno de Nilo depois do salto (B14) é conteúdo da leva B.

---

### B10 — A borda do bosque **[APROVADO]** (dossiê §L)

- **Jogador:** atravessa `entrada_bosque` pela primeira vez com permissão narrativa.
- **Onde:** `entrada_bosque` → `bosque_clareira`.
- **Sistema:** T008 (limite navegável), T002.
- **Avanço:** chegar à clareira.
- **Se ignorar/atrasar:** pode voltar à vila quando quiser; o bosque não tranca atrás dele. **Sem porta de mão única neste slice.**
- **Aceite observável:** o bosque é intransponível fora do vão da entrada, e o jogador consegue voltar a `praca_centro` a pé a qualquer momento.

---

### B11 — O símbolo do Limiar · `q08_ecos_do_limiar` **[APROVADO]** (dossiê §I, §L)

- **Jogador:** encontra na clareira o mesmo símbolo visto em B01 e o toca.
- **Onde:** `bosque_clareira`.
- **Sistema:** T006, T005.
- **Avanço:** `achar_o_simbolo` → `tocar_o_simbolo`.
- **Se ignorar/atrasar:** pode sair da clareira e voltar depois; o símbolo não some.
- **Aceite observável:** o símbolo em tela é **visualmente o mesmo** de B01. Tocar grava `marco.eco_do_limiar` e levanta `salto_temporal_liberado`. **Concluir a missão NÃO executa o salto.**
- **Guarda-corpo:** Aethron não reaparece; o Limiar apenas **ecoa** (dossiê §I: a memória do Limiar normalmente se perde). Não resolver a Primeira Fratura aqui; o slice só a insinua.

---

### B12 — O aviso do salto: "o que se encerra" **[APROVADO]** (dossiê §D, §M; GDD 07 "pular fase sem aviso")

- **Jogador:** recebe, antes de qualquer confirmação, a lista explícita do que a infância deixa para trás.
- **Onde:** `bosque_clareira`.
- **Sistema:** T009 (dono do salto), lendo T006/T005.
- **Avanço:** o jogador fecha o aviso — o que o devolve ao mundo, **não** o faz saltar.
- **Se ignorar/atrasar:** fechar o aviso devolve o controle em Auren com 5–7 anos; ele pode terminar as opcionais e reabrir a confirmação quando quiser, em `bosque_clareira`.
- **Aceite observável:** o aviso **nomeia** cada missão opcional ainda não concluída (pelo título canônico, não pelo id) e cada vínculo que muda. Uma partida que já concluiu as três opcionais vê o aviso **sem** lista de pendências — e não uma lista vazia com moldura.
- Ver §4 para o conteúdo do aviso.

---

### B13 — A confirmação do salto **[APROVADO]** (dossiê §D, §M; backlog teste 5)

- **Jogador:** confirma explicitamente a passagem para ~8 anos.
- **Onde:** `bosque_clareira`.
- **Sistema:** T009, T004 (o salto é uma transação idempotente no save).
- **Avanço:** confirmação em ação afirmativa separada do aviso (dois passos, nunca um botão só).
- **Se ignorar/atrasar:** cancelar é sempre possível e não deixa estado pela metade.
- **Aceite observável:** interromper o jogo **durante** a transição e recarregar deixa o save num dos dois estados — antes do salto ou depois dele — **nunca** com idade 8 e a flag de salto ausente, nem com o salto aplicado duas vezes.
- **Exploit coberto:** dossiê §M "salto temporal idempotente"; backlog teste 5.

---

### B14 — Três anos depois **[APROVADO]** (dossiê §D, §G, §L)

- **Jogador:** acorda com ~8 anos em Auren. Reconhece a vila e é reconhecido.
- **Onde:** `spawn_player` → `praca_centro`.
- **Sistema:** T009 (idade/fase `talentos`), T007 (memória), T010 (reputação), T008.
- **Avanço:** falar com pelo menos um NPC que cite um evento gravado antes do salto.
- **Se ignorar/atrasar:** sem pressa; o treino em B15 espera.
- **Aceite observável:** ver §4.3 — o que **tem** de ser lembrado. Em uma frase: pelo menos um NPC nomeia corretamente a decisão de B08, e nenhum NPC cita um evento que não está no histórico daquele save.
- **Exploit coberto:** dossiê §M "NPC recupera evento correto"; backlog teste 4.

---

### B15 — Treino supervisionado **[APROVADO]** (dossiê §F, §L)

- **Jogador:** aprende ataque leve, ataque forte, defesa/esquiva e a primeira manifestação mágica, com espada de madeira, contra um oponente supervisionado.
- **Onde:** `posto_guarda` (Tovin). **[PROPOSTA]** reaproveitar o posto em vez de pedir uma âncora nova; o GDD 10 prevê uma cena `Auren_Training` para quando o treino crescer — caminho de upgrade registrado, não pago agora.
- **Sistema:** T011 (combate), T009 (domínio/afinidade).
- **Avanço:** executar cada um dos quatro verbos uma vez.
- **Se ignorar/atrasar:** o treino não expira. Repetir é permitido e **saturar é obrigatório**: a partir de um teto por etapa, repetição trivial para de render domínio (dossiê §D, §F).
- **Aceite observável:** o painel de progresso mostra o ganho **estacionando** após N repetições do mesmo golpe no boneco, e o texto diz ao jogador por quê. Nenhum atributo cresce sem limite.
- **Exploit coberto:** dossiê §M "treino trivial satura"; backlog teste 7.
- **[PROPOSTA]** se `confianca_de_borin` estiver no histórico (q06 concluída), a espada de treino é a que Borin entregou: **objeto e fala diferentes, estatística idêntica.** Opcional nunca vira vantagem numérica.

---

### B16 — O gancho **[APROVADO]** (dossiê §L, §I)

- **Jogador:** recebe a pergunta que a campanha herda — o símbolo, o desaparecimento e a Primeira Fratura ficam **abertos**.
- **Onde:** `posto_guarda` ou `praca_centro`.
- **Sistema:** narrativa; nenhuma mecânica nova.
- **Avanço:** fim do slice.
- **Se ignorar/atrasar:** n/a.
- **Aceite observável:** a tela final **não** afirma nenhuma resposta sobre a Primeira Fratura e **não** promete conteúdo que o slice não tem (sem "continua em Karvorn").

---

## §3 — As oito missões de Auren

Dados completos em `content/quests/*.json`. Os ids abaixo são os mesmos já publicados pela T006 em
`client/Assets/_COE/Scripts/Quest/QuestCatalog.cs`; **divergir deles exigiria migração de save.**

| Id | Título (GDD 07) | Tipo | Central? | Pré-condição | Âncora | NPCs |
|---|---|---|---|---|---|---|
| `q01_um_novo_amanhecer` | Um Novo Amanhecer | cotidiana | **central** | — (idade 5, fase descobertas) | `casa_familia` | mara, daren |
| `q02_uma_pequena_responsabilidade` | Uma Pequena Responsabilidade | cotidiana | **central** | q01 concluída | `praca_centro` | daren, oren |
| `q03_o_cesto_perdido` | O Cesto Perdido | exploratoria | opcional | q01 concluída | `praca_centro` | oren, nilo |
| `q04_uma_promessa` | Uma Promessa | social | **central** | q02 concluída | `casa_sera` | sera, nilo |
| `q05_o_animal_ferido` | O Animal Ferido | social | opcional | q01 concluída | `ervanaria` | lysa, tovin |
| `q06_o_segredo_do_ferreiro` | O Segredo do Ferreiro | cotidiana | opcional | q02 concluída | `ferraria` | borin |
| `q07_o_desaparecimento` | O Desaparecimento | narrativa | **central** | q04 concluída + objetivo `sustentar_a_escolha` | `praca_centro` | maelis, eira, oren, tovin |
| `q08_ecos_do_limiar` | Ecos do Limiar | narrativa | **central** | q07 concluída | `bosque_clareira` | — |

Cinco centrais, três opcionais, como manda o dossiê §L. **Nenhuma central depende de opcional** — o validador
falha se alguém quebrar isso. Todas ocorrem entre 5 e 7 anos (fase `descobertas`) e **expiram no salto temporal**;
por isso B12 precisa nomeá-las.

Sem texto de diálogo: todo `texto` de objetivo é `"[a escrever]"`. Escrever fala é tarefa de redação, não desta.

---

## §4 — A transição narrativa do salto temporal

É o beat que o dossiê §L chama de prova emocional do slice. Se a passagem de tempo não doer um pouco,
o slice não provou sua tese. Três partes, nesta ordem, sem atalho: **aviso → confirmação → depois**.

### 4.1 O aviso: "o que se encerra" **[APROVADO]** (dossiê §D: "avisar oportunidades que serão encerradas")

O aviso **não** é um "tem certeza?". É um inventário do que fica para trás. Ele lista, nomeando:

1. **Missões opcionais não concluídas**, pelo título canônico. (Vazio quando não há — e aí a seção some.)
2. **Vínculos que mudam**: Nilo e Sera terão ~8 anos, outras aspirações e outra rotina (dossiê §G: "não congelar amigos em uma rotina eterna").
3. **A criança que o jogador foi**: a fase `descobertas` se fecha; as atividades de criança de 5 anos não voltam.
4. **O que sobrevive** — e isto precisa estar escrito, senão o jogador teme perder o save: nome, destino, origem, histórico de vida, promessas, confiança dos NPCs, moedas e itens. O salto **não** zera nada.

Fechar o aviso devolve o jogador a Auren com 5–7 anos. A confirmação fica onde estava, em `bosque_clareira`.

**[PROPOSTA]** o aviso nunca aparece sozinho ao tocar o símbolo: só depois de o jogador escolher, em `bosque_clareira`,
seguir adiante. Tocar o símbolo (B11) é descoberta; saltar é decisão. São dois atos.

### 4.2 A confirmação **[APROVADO]** (dossiê §M; backlog teste 5)

- Ação afirmativa separada, com rótulo que **diz o que vai acontecer** ("Deixar a infância para trás") e não um "OK".
- Cancelar é sempre possível até o último clique.
- O salto é **uma transação idempotente**: identificador próprio no histórico de vida, mesma regra das recompensas.
  Aplicar duas vezes é um no-op, e recarregar durante a transição nunca produz idade 9, nem idade 8 sem a flag.
- Regra de ouro: o salto **não** concede atributo, item, grau nem ascensão. Ele muda idade, fase, aparência e mundo.
  Um salto que dá poder é um salto farmável.

### 4.3 O depois: o que muda e o que tem de ser lembrado **[APROVADO]** (dossiê §D, §G, §L)

**Muda visivelmente** (se nada disso mudar, o jogador não sente o salto):

| O quê | Como |
|---|---|
| Aparência | Base corporal de criança maior/adolescente inicial; a silhueta muda em tela, não só um número. |
| Nilo e Sera | Mais velhos, com outra rotina e outra aspiração; a fala de abertura deles é outra. |
| Auren | Ao menos uma mudança visível e comentada por um NPC — **[PROPOSTA]** uma construção concluída ou um espaço reaproveitado perto de `praca_centro`. |
| Rotina | O jogador tem acesso ao treino (`posto_guarda`) que aos 5 anos não tinha. |
| Consequência de B09 | O desaparecimento continua sem solução e é citado por quem o investigou. |

**Tem de ser lembrado** (é isto que a T014 verifica; cada item é um registro do histórico de vida, não uma impressão):

| Fato | Origem | Quem lembra |
|---|---|---|
| A promessa e como ela terminou | `evento.q04_promessa_cumprida` **ou** `evento.q04_promessa_quebrada` | Sera e Nilo, com falas diferentes entre os dois desfechos |
| O desaparecimento investigado | `evento.q07_concluida` | Maelis e Tovin |
| O que o jogador fez pelos outros | `evento.q03_concluida`, `evento.q05_concluida`, `evento.q06_concluida` | Oren, Lysa, Borin — cada um só pelo que lhe diz respeito |
| O primeiro dia | `marco.primeiro_dia` | Mara e Daren |

**Regra negativa, igualmente obrigatória:** nenhum NPC pode citar um evento que não está no histórico daquele save.
Um NPC que "lembra" de uma missão opcional nunca feita é bug de mesma gravidade que um NPC que esqueceu.
E nenhuma fala — nem de diálogo generativo, se um dia existir — concede item, moeda, missão ou reputação:
fala expressa estado, quem muda estado é a operação de domínio (dossiê §G; backlog teste 8).

---

## §5 — Checklist de regressão do slice (base da T014)

Passos jogáveis, para executar numa build. **Nada aqui foi executado.** Cada linha é: o que fazer → o que
precisa aparecer. Um passo que não puder ser observado em tela não é um teste, é uma opinião.

### 5.1 Os oito testes obrigatórios do backlog

| # | Passo no jogo | Resultado esperado |
|---|---|---|
| R1 | Nascer com `dificil`/`artesaos`; depois percorrer **todos** os menus (pausa, opções, save, assistências) | Em nenhum lugar existe trocar destino ou origem. Alterar assistência/dificuldade não muda um byte de `BirthChoice`. |
| R2 | Criar uma partida para cada uma das **12** combinações destino×origem e entrar em Auren | As 12 carregam sem exceção, com família e textos coerentes, sem placeholder cru e sem história duplicada entre elas. |
| R3 | Concluir `q02`; anotar as moedas; salvar; recarregar; tentar reentregar a tarefa | O saldo não sobe de novo. `rec.q02_uma_pequena_responsabilidade.moedas` aparece uma única vez no histórico. |
| R4 | Concluir `q04` (quebrando a promessa); salvar; fechar o jogo; reabrir; falar com Sera | Sera usa a fala do desfecho quebrado. O histórico tem **um** registro. Mara e Daren não citam a promessa. |
| R5 | Chegar a B13; confirmar o salto; **matar o processo durante a transição**; reabrir | O save está antes do salto **ou** depois dele, completo. Nunca idade 8 sem a flag, nunca salto aplicado duas vezes. Voltar a `bosque_clareira` não oferece saltar de novo. |
| R6 | Jogar do início a B16 concluindo **zero** missões opcionais | O slice termina. Nenhum beat central ficou inacessível. O aviso de B12 nomeou as três opcionais pendentes. |
| R7 | No treino (B15), repetir o mesmo ataque no boneco 50 vezes | O ganho de domínio estaciona num teto visível e o jogo diz por quê. Nenhum atributo cresce sem limite. |
| R8 | Conversar com todos os NPCs alcançáveis | Nenhuma fala altera inventário, moedas, missão ou reputação. Toda mudança de estado veio de um objetivo concluído. |

### 5.2 Os exploits do dossiê §M

| # | Passo no jogo | Resultado esperado |
|---|---|---|
| R9 | (destino permanente) Editar o save em disco trocando `destinyId`; recarregar | O jogo rejeita ou registra explicitamente a inconsistência. **Não se promete resistência a adulteração local em jogo solo** (dossiê §M); o que se exige é que não quebre em silêncio. |
| R10 | (sem ascensão por menu) Percorrer menus e inventário procurando qualquer via de mudar Grau de Existência | Não existe. O slice não implementa ascensão; nenhum item ou tela a concede. |
| R11 | (recompensa não duplica no reload) Salvar **no instante** da entrega de `q03` e `q05`; recarregar | O item entra uma vez. Id de transação já no histórico ⇒ concessão é no-op. |
| R12 | (salto idempotente) Ver R5 | — |
| R13 | (quest concluída não reabre) Concluir `q06`; voltar à `ferraria` várias vezes | Borin tem fala de pós-missão; a missão não reabre e não paga de novo. |
| R14 | (treino trivial satura) Ver R7 | — |
| R15 | (NPC lembra o evento certo) Partida A conclui `q05`, partida B não; falar com Lysa nas duas depois do salto | Em A, Lysa cita o animal. Em B, **não cita**. Nenhuma das duas inventa um evento ausente. |
| R16 | (fala não muda estado) Ver R8 | — |
| R17 | (12 combinações válidas) Ver R2, agora com save/load em cada uma | As 12 salvam e carregam com round-trip exato, inclusive nome com acento. |
| R18 | (transição não bloqueia campanha) Em B12, fechar o aviso e voltar a Auren; concluir as opcionais; voltar e saltar | O retorno funciona, as opcionais concluem, o salto continua disponível. Nenhum beco sem saída. |

### 5.3 Verificação de dados (esta sim, executada)

`PYTHONUTF8=1 python content/quests/validate_quests.py` — confere ids, pré-condições, ciclos,
alcançabilidade, opcional×central, unicidade de transação, NPCs e âncoras. Saída real da execução
desta entrega está no reporte da T012. **Isso valida dados; não substitui nenhum passo de R1 a R18.**

---

## §6 — Pendências que este documento abre

1. **Quem desaparece em `q07`** (B09): resolvido, é Nilo (ADR-0005).
2. **Âncora `horta_familia`**: resolvido em 2026-09-29, a âncora existe na cena (§1.2).
3. **Treino em `posto_guarda`** — PROPOSTA; o GDD 10 prevê `Auren_Training` quando o treino crescer.
4. **A mudança visível de Auren no pós-salto** (§4.3) — PROPOSTA; custa arte e depende da T013.
5. **Quantidades de recompensa** (5 e 8 moedas, 2 ervas) — HIPÓTESE v0 marcada nos JSON; o GDD v1.2 não publica número.
6. **Divergência de fonte:** resolvido pelo ADR-0005. O `QuestCatalog.cs` é a fonte de verdade; os `content/quests/*.json` são a cópia de design, guardada por teste de paridade.
