# Despertar dos Talentos — roteiro jogável do episódio 2 (8 → 11 anos)

> **Estado (2026-10-29): PROPOSTA inteira.** Nada aqui foi aprovado, codificado nem jogado. O escopo vem do
> [`CONTRATO_V1.md`](../CONTRATO_V1.md) §4 (também PROPOSTA). Pelo `CLAUDE.md`, mudança de produto vira proposta em
> `docs/`: **todo beat, missão, regra e número deste arquivo é [PROPOSTA]** até o idealizador aprovar.
> Formato: o mesmo do [`SLICE_A_PRIMEIRA_EXISTENCIA.md`](SLICE_A_PRIMEIRA_EXISTENCIA.md) (âncoras, beats com estado,
> missões, transições, regressão, pendências), com duas seções a mais pedidas para este episódio: §0 (o que vem do
> episódio 1) e §7 (o que o roteiro pede de arte e de código).
>
> **Fontes:** `docs/CONTRATO_V1.md` §1, §4 · `docs/gdd/GDD_MESTRE_v1_2.md` cap. 03, 04, 05, 06, 07, 08 ·
> `docs/GDD.md` §0 (lacunas 02.5, 02.6, 03.5, 06.2, 06.5, 13.3, S.ASSIST) · `docs/COMPETITIVO.md` (C1–C5, H1–H9) ·
> `docs/arte/fichas/ELENCO.md` e as fichas dos 10 NPCs, do avatar, de Aethron e do símbolo · ADR-0002, 0004, 0005,
> 0007, 0010 · `content/quests/*.json` e `_schema.json` · `docs/design/PROPOSTA_MECANICAS_HELL_MODE.md` (HM-01 a HM-06,
> HM-14; citados pelo id, sem forma da obra de referência).
>
> **Convenção de marcação:** **[PROPOSTA]** = decisão deste documento. **[CÂNONE]** = já aprovado (GDD v1.2, ADR, ficha
> aprovada no G1 ou fala já escrita em `strings.pt-BR.json`) e só citado aqui. **HIPÓTESE v0** = número a balancear
> em playtest; o GDD não publica número.
>
> **Regra de redação herdada (persona do roteirista, `vstack-agentes/.../39_ai-narrative-writer.md`):** destino e
> origem mudam detalhe, reação e custo, **não** o roteiro. Não são doze campanhas. Toda variação cabe em 1–2 linhas
> por célula, ou a célula diz "igual".

---

## §0 — O que vem do episódio 1, e como volta

O episódio 2 começa no save que o episódio 1 deixou, no ponto do B16 ("Continuar em Auren",
`marco.fim_da_primeira_existencia`). Nada é zerado. Cada fio aberto do episódio 1 volta assim:

| Fio do ep. 1 | Onde foi gravado | Como ecoa no ep. 2 |
|---|---|---|
| **O desaparecimento do Nilo** (q07, ADR-0005, ADR-0007 §3) | `evento.nilo_desapareceu`, `evento.q07_concluida` | Nilo, aos 8, só lembra de "um círculo de luz e um silêncio enorme" ([CÂNONE] `dialogo.nilo.aos_oito*`). No ep. 2 o **silêncio** ganha corpo: é o que os abafadores fazem (§3.4). Na q13 ele mostra o buraco no espinheiro por onde passou; na q16 o jogador encontra o que ele viu. A folha de Maelis continua **de pé** até o fim ([CÂNONE] "não fecho"); o ep. 2 acrescenta uma linha a ela, não a deita. |
| **O símbolo do Limiar** (q08, B11–B13) | `marco.eco_do_limiar`, `marco_idade_8` | Na q10 a criança, que agora lê, acha o mesmo sinal no livro de Maelis (se assinou com o círculo na q07) ou só o risco (se riscou). O símbolo volta a ter nós turquesa: há algo a fazer nele de novo (regra da ficha `simbolo_limiar`, C5). No fim (B16 do ep. 2), o anel ganha um **segundo vão**: é o gancho. |
| **A assinatura da q07** (ADR-0010 adendo, item 11) | `evento.q07_assinou_com_o_circulo` **ou** `..._com_um_risco` | q10: Maelis abre o livro na página assinada. Com o círculo, Eira diz que já viu esse traço "num papel mais velho que Auren"; com o risco, Maelis diz que a página "não diz nada além do que você contou". A pista para a q13 chega nos dois casos, por caminhos diferentes (Eira lê a página velha; ou a criança lê a folha de Nilo). |
| **A promessa da q04 (±20 com Sera e Nilo)** (ADR-0007 §4) | `evento.q04_promessa_cumprida` **ou** `..._quebrada`; Confiança de `sera` e `nilo` em cordial (+20) ou frio (−20) | É a primeira vez que a reputação é **lida** por regra (lacuna 06.2 do `GDD.md` §0). Nilo cordial chama a criança para seguir o fio (q13); Nilo frio vai sozinho e a criança o rastreia. Sera cordial faz dupla com a criança na festa (q14) e desce ao bosque (q15); Sera fria compete na festa e fica na linha com Lysa. **Nenhum caminho fecha**: muda quem vai junto, a cena e a fala. E a q13 repete a pergunta da q04: Nilo pede segredo de novo. |
| **A espada comum ou marcada** (q06, `confianca_de_borin`, ADR-0010 §3–4) | `rec.b15.espada_do_treino`, flag `confianca_de_borin` | Aos 11 a espada de madeira fica curta. Entra a **espada de freixo** (`item.espada_de_freixo`), com a mesma regra: com `confianca_de_borin` **ou** com a q11 concluída, Borin passa a plaqueta de cobre para a nova (`item.espada_de_freixo_marcada`). **Quem pulou a q06 aos 5 tem uma segunda chance aos 8.** Estatística idêntica nos dois casos, de novo. |
| **4 destinos × 3 origens** (ADR-0004, T003) | `BirthChoice` | Cada destino ganha **um acontecimento próprio** no ato I (E2-B07), usando os ids de evento que hoje não têm conteúdo (`GDD.md` 02.6). Cada origem muda a casa, a tarefa de família e uma fala (§3.7, lacuna 02.5/S.B03). Destino nunca muda número de criatura, de dano nem de recompensa (ADR-0004; teste em §5). |
| **O que a criança fez pelos outros** (q03, q05, q06) | `evento.q03/q05/q06_concluida` | Só quem testemunhou lembra, como no ep. 1 (`NpcMemory`). Lysa: o bicho que "ainda aparece na porta" ([CÂNONE]) é um abafador manso, e é ele que ensina a criança a ver os outros (q12). Oren: o buraco na torre. Borin: a prova de cobre no aro. Regra negativa do ep. 1 mantida: ninguém cita o que não está no histórico. |
| **O treino (B15) e o teto do domínio** | ledger `life.pratica` | Continua. O salto 8→11 é **dentro da mesma etapa** (`talentos`, 8–11): ele **não** reabre os tetos do `Mastery` (§5.2, E-07). |

---

## §1 — Âncoras que o roteiro exige

Uma âncora é um `GameObject` vazio em `Ancoras/<id>` na cena Auren (contrato do SLICE §1). Lugar: **só Auren e o
Bosque dos Sussurros** (`CLAUDE.md`). O episódio aprofunda o bosque que já existe atrás do vão; não cria região.

### 1.1 Já existem (T008) e o ep. 2 usa

| Âncora | Uso neste roteiro |
|---|---|
| `spawn_player` | Reentrada aos 11 depois do salto (E2-B10). |
| `casa_familia` | E2-B01 e E2-B10: casa por origem (§3.7); tarefa de família (HM-06a); Descansar. |
| `casa_nilo` | q13 (Nilo aos 11). |
| `casa_sera` | q12 (tabuinha da Sera). |
| `praca_centro` | E2-B02 (leitura dos fios), q10 (aula), q14 (a festa inteira). |
| `mural_avisos` | q10 (Eira lê o mural à noite; o livro de Maelis). |
| `ferraria` | q11; entrega da espada de freixo marcada (E2-B10). |
| `ervanaria` | q12 (unguento). |
| `posto_guarda` | q09; entrega da espada de freixo comum (E2-B10); q13 (Nilo aprendiz de guarda). |
| `entrada_bosque` | q09 (primeira luta, na borda), q12 (colher), q13, q15 (reunir na linha). |
| `bosque_clareira` | q10 (o sinal e o símbolo; libera o salto), E2-B09 (salto), E2-B16 (segundo vão). |
| `horta_familia` | Tarefa de família dos agricultores (§3.7). |
| `portao_sul` | Guardiões guardam o portão durante a festa (q14, §3.7). Continua sendo o limite sul. |

### 1.2 Pedido novo à cena (dentro do Bosque dos Sussurros) **[PROPOSTA]**

Quatro âncoras, todas ao norte da clareira (z > 78 no `AurenSceneBuilder`), numa trilha só, sem bifurcação que
pareça mundo aberto. Entram em `content/quests/_schema.json` → `ancoras_validas.pedido_t012` (a lista hoje vazia) até
a cena as ter.

| Âncora | O que é | Missões |
|---|---|---|
| `bosque_trilha` | Trilha estreita entre o espinheiro, logo depois da clareira. Tem um tronco caído (atalho por Força) e uma volta pelo lado (atalho por Exploratória). | q09 (fim), q13, q15 |
| `bosque_espinhal` | Trecho fechado de espinheiro, onde o fio do Nilo ficou preso. Casa dos feixes-de-espinho. | q13, q15 |
| `bosque_lajedo` | Clareira de chão de pedra nua, sem folha: é onde os lajeiros raspam e largam laje. | q15 |
| `bosque_coracao` | Fundo do bosque, uma depressão redonda com o chão sem som. Encontro maior. Fim da trilha: dali não se vai a lugar nenhum. | q15 (fim), q16 |

**Custo de desempenho:** é o maior risco técnico do episódio no Android de entrada (ADR-0009, H8). A trilha é um
corredor com oclusão natural (espinheiro), uma área carregada por vez; o orçamento do `CONTRATO_V1.md` §5
(≤ 150 000 tris, ≤ 200 draw calls) vale por área, não somado.

---

## §2 — Roteiro beat a beat

Colunas como no SLICE: **Jogador · Onde · Sistema · Avanço · Se ignorar/atrasar · Aceite observável**, mais
**Destino/origem** (o que muda, ou "igual") e **Reputação lida** quando houver.

Duração-alvo do episódio: **60–90 min** (HIPÓTESE, H1). Estimativa por beat ao lado do título; só centrais ≈ 70 min,
tudo ≈ 88 min.

---

### Ato I — aos 8 anos (fase `talentos`)

### E2-B00 — A porta do episódio **[PROPOSTA]** (CONTRATO_V1 §2)

- **Jogador:** termina o B16 do ep. 1 e vê, uma vez, o cartão do episódio 2 (conteúdo listado, preço, "compra única").
  Pode comprar ou seguir em Auren aos 8 sem comprar.
- **Onde:** tela do gancho do ep. 1 e menu. **Nunca** no meio de cena.
- **Sistema:** IAP não consumível `coe.episodio2` (dois passos: concede, depois confirma; restauração). O direito de
  acesso **não** vai no `SaveData`: é da loja. O save do ep. 1 continua o mesmo arquivo.
  **Feito diferente (PR #41, 2026-10-29):** o direito também é gravado no save v3 (bloco `compras`), para o jogo abrir
  o episódio sem rede; a loja continua devolvendo o direito ao restaurar. O portão é `QuestCatalogEp2.Liberado(save)`:
  direito presente **e** `marco.fim_da_primeira_existencia` no histórico.
- **Avanço:** direito de acesso presente → a q09 fica disponível no próximo amanhecer.
- **Se ignorar:** o jogador segue em Auren aos 8, com as falas de pós-salto do ep. 1. Nada some, nada trava, nada
  insiste. O cartão volta só no menu.
- **Aceite observável:** sem a compra, nenhuma missão `q09`+ aparece; com a compra, a q09 aparece sem reiniciar o save.
  Restaurar a compra num aparelho novo com o save copiado abre o mesmo estado.

### E2-B01 — Manhã de aprendiz **[PROPOSTA]** (~3 min)

- **Jogador:** acorda aos 8 em `casa_familia`. Ouve o chifre de Tovin chamar Auren para o dia ([CÂNONE] da ficha
  `tovin`, C4: o chifre voltou a tocar depois do B15). Mara ou Daren dão a tarefa da casa da origem.
- **Onde:** `casa_familia` → rua.
- **Sistema:** HM-06a (tarefa da casa vira atividade de domínio da afinidade da origem, com teto), `NpcAgenda`.
- **Avanço:** sair de casa. A tarefa é opcional e repetível até o teto.
- **Se ignorar/atrasar:** nada. A tarefa fica no lugar.
- **Destino/origem:** a casa muda por origem (§3.7); a tarefa muda por origem (Natural, Artesanal ou Exploratória);
  destino muda só a fala de Mara (o estado da manta, [CÂNONE] da ficha `mara`, C8).
- **Aceite observável:** em três saves, um por origem, a casa tem objetos diferentes e a tarefa rende trilhas
  diferentes no painel de progresso (HM-01). A mesma tarefa repetida para no teto, com o texto de "já te ensinou".

### E2-B02 — A leitura dos fios **[PROPOSTA]** (HM-03) (~3 min)

- **Jogador:** Maelis faz o registro das crianças que chegaram aos 8. Lê na prancha o que a criança **viveu** e
  escreve uma "vocação provável" e o mestre que a receberia (Tovin, Eira, Borin ou Lysa). O jogador aceita ou recusa
  com o mesmo gesto da q07 (traço ou risco).
- **Onde:** `praca_centro`.
- **Sistema:** `VocacaoProvavel(SaveData) → trilhaId`, função **pura do histórico** (trilhas com mais domínio dos 5 aos 7,
  marcos, opcionais). Grava `evento.vocacao_aceita` **ou** `evento.vocacao_recusada`, exatamente um.
- **Avanço:** responder.
- **Se ignorar/atrasar:** a leitura espera; a q09 não depende dela.
- **Destino/origem:** destino muda **o tom** de Maelis sobre a família, nunca o resultado (ADR-0004 visível ao
  jogador). Origem: igual.
- **Reputação lida:** nenhuma.
- **Aceite observável:** dois saves com a mesma infância e destinos diferentes recebem a **mesma** vocação. Histórico
  vazio dá "ainda aberta", nunca erro. Aceitar ou recusar não abre nem fecha nenhuma missão: o mestre "proposto" só
  muda uma fala de boas-vindas dele.
- **Guarda-corpo:** é palpite da vila, dito como palpite. Revisão de texto antes de playtest com criança (ADR-0009;
  risco do HM-03).

### E2-B03 — A linha do posto · `q09_a_linha_do_posto` (central) **[PROPOSTA]** (~12 min)

- **Jogador:** vira aprendiz de ronda de Tovin. Aprende a ler rastro, faz a primeira ronda até o vão e, pela primeira
  vez, entra no bosque além da clareira, atrás de Tovin. Na borda da trilha enfrenta um bando de abafadores: a
  primeira luta de verdade. Na volta, é a criança que guia ("agora pisa você, eu vou atrás").
- **Onde:** `posto_guarda` → `entrada_bosque` → `bosque_clareira` → `bosque_trilha` (borda) → `posto_guarda`.
- **Sistema:** Quest, Combat (encontro `enc.trilha_abafadores_borda`), Mastery (ronda, leitura de rastro, luta),
  HM-02 (segredo de ofício do Tovin: o bloqueio no último instante abre a técnica "contra", §3.2).
- **Avanço:** objetivos em ordem: `ouvir_o_toque` → `ronda_ate_o_vao` → `ler_o_rastro` → `primeira_luta_na_borda` →
  `guiar_a_volta`.
- **Se ignorar/atrasar:** central. Sem ela, nada do ato I anda (a q10 pede a q09). Sem timer.
- **Destino/origem:** origem `guardioes`: a "ronda com Tovin" ([CÂNONE] `oportunidade.ronda_com_tovin`, hoje sem
  conteúdo) é esta missão começando um passo à frente (Tovin já espera a criança no posto; mesma missão, mesmos
  objetivos). Destino: a fala de recepção da ficha `tovin`, C8 (quatro linhas). Fora isso, igual.
- **Reputação lida:** Confiança de `tovin` sobe +10 ao concluir (HIPÓTESE v0); a faixa cordial é lida na q15.
- **Aceite observável:** a luta da borda pode ser vencida com afinidades e atributos no valor inicial. Os abafadores
  fogem quando sobra um (não há último golpe para farmar). Perder devolve a criança à linha, puxada por Tovin, com a
  Vida cheia e nada perdido.

### E2-B04 — A lousa e o livro · `q10_a_lousa_e_o_livro` (central) **[PROPOSTA]** (~10 min)

- **Jogador:** estuda com Eira na praça (a lousa, [CÂNONE] da ficha `eira`, C4) e, à noite, aprende a ler o mural com
  ela. Maelis abre o livro na página da q07. A criança compara o sinal (ou o risco) com o que lembra, vai à clareira e,
  com a segunda manifestação que Eira ensinou de lado ("o fio de luz", §3.2), toca o símbolo de novo. Os nós ficam
  dourados: o salto para os 11 está liberado.
- **Onde:** `praca_centro` → `mural_avisos` (noite) → `bosque_clareira`.
- **Sistema:** Quest, Dialogue (lousa e livro), Mastery (Intelecto, Arcana), Combat (`SpellCast`, segunda manifestação),
  `AgeAdvance` (novo marco `marco_idade_11`, liberado por `evento.q10_concluida`).
- **Avanço:** `aula_na_praca` → `responder_a_lousa` → `ler_o_mural_a_noite` → `a_pagina_assinada` → `o_sinal_na_clareira`.
- **Se ignorar/atrasar:** central. O salto não abre sem ela. A noite do mural exige o período Noite; chegar lá é
  concluir uma missão ou Descansar (ADR-0007 §1).
- **Destino/origem:** destino, a fala de Eira sobre a criança ([CÂNONE] ficha `eira`, C8: aluna de dia na Serena e na
  Normal, de lanterna na Árdua, alvo da curiosidade dela na Ruptura). Origem: igual.
- **Assinatura da q07:** com o círculo, Eira reconhece o traço "num papel mais velho que Auren" e não diz o nome do
  que se rompeu (B11/B16 do ep. 1: a Fratura só se insinua). Com o risco, Maelis lê a folha de Nilo em voz alta: "entrou
  um, voltou um, e a conta não fecha". Os dois caminhos levam à clareira. Save sem desfecho (anterior à regra): fala
  neutra de Maelis.
- **Aceite observável:** concluir a q10 grava `marco.fio_de_luz` uma vez; a técnica aparece no botão de magia; os nós do
  símbolo passam de turquesa a dourado e o prompt "Seguir adiante" aparece na clareira.

### E2-B05 — O aro de provas · `q11_o_aro_de_provas` (opcional) **[PROPOSTA]** (~8 min)

- **Jogador:** volta à ferraria. Com `confianca_de_borin`, Borin pede que a criança "seja os olhos dele" na
  conferência (o segredo da q06 continua, [CÂNONE] ADR-0010 §2). Sem a flag, a criança descobre o segredo agora, vendo
  Borin conferir com o polegar. Forja, com ele, os cravos de ferro que Tovin usa para marcar caminho no bosque, e decide
  o que fazer com o segredo: guardar ou contar a Maelis.
- **Onde:** `ferraria` → `posto_guarda`.
- **Sistema:** Quest, Mastery (Artesanal, Força), HM-02 (segredo de ofício do Borin: bater no ponto que o gabarito
  aponta, não no que o olho vê, abre "forja fina"), Reputação.
- **Avanço:** `voltar_a_ferraria` → `ser_os_olhos` → `forjar_os_cravos` → `decidir_o_segredo` → `entregar_a_tovin`.
- **Desfechos:** `evento.q11_segredo_guardado` **ou** `evento.q11_segredo_contado`, um só.
  - **Guardado:** Confiança de `borin` +20. Aos 11, uma ferramenta que ele errou falha na festa (o podão de Tovin
    quebra na banca dele, q14); Borin sabe e a criança sabe.
  - **Contado:** Confiança de `borin` −20, Renome `com_auren` +10. Aos 11, Borin usa lentes que Oren trouxe de fora e a
    forja voltou a acertar; ele diz que não perdoou e que enxerga.
  Nenhum dos dois é "o certo"; os dois mudam fala e objeto aos 11, nenhum muda número de combate.
- **Se ignorar/atrasar:** opcional. Expira no salto (idade máxima 10) e é nomeada no aviso. A espada de freixo comum
  continua chegando no posto.
- **Destino/origem:** origem `artesaos`: a "oficina de Borin" ([CÂNONE] `oportunidade.oficina_de_borin`, hoje sem
  conteúdo) é esta missão, oferecida pelo próprio Borin ao passar na rua. Destino: fala de Borin (ficha `borin`, C8).
- **Aceite observável:** concluir a q11 sem a q06 também faz a espada de freixo vir marcada aos 11. As duas espadas têm
  números idênticos (teste existente do B15 estendido).

### E2-B06 — As ervas que curam · `q12_as_ervas_que_curam` (opcional) **[PROPOSTA]** (~8 min)

- **Jogador:** colhe com Lysa na beira, de costas para o bosque ([CÂNONE] ficha `lysa`, C1). Se fez a q05, o bicho que
  "ainda aparece na porta" é um abafador manso, e ele mostra como os outros se acalmam: ficar parado perto deles.
  Sera, que agora aprende com Lysa ([CÂNONE] `dialogo.sera.aos_oito*`), disputa com a criança quem nomeia mais ervas
  na tabuinha. Preparam o unguento juntas e a criança o usa numa ferida de verdade (a sua, depois do treino, ou a do
  joelho de Sera).
- **Onde:** `entrada_bosque` → `casa_sera` → `ervanaria`.
- **Sistema:** Quest, Mastery (Natural, Intelecto), HM-05 (erva cura: primeira operação de **consumir** item),
  Reputação (Sera).
- **Avanço:** `colher_com_lysa` → `a_tabuinha_de_sera` → `preparar_o_unguento` → `usar_numa_ferida`.
- **Se ignorar/atrasar:** opcional, expira no salto. **Opcional não vira vantagem numérica** (regra do B15): os dois
  unguentos que levam ao bosque chegam a **todos** na q15, das mãos de Lysa. Quem fez a q12 os preparou: objeto com a
  própria marca e outra fala.
- **Destino/origem:** origem `agricultores`: a "horta da família" ([CÂNONE] `oportunidade.horta_da_familia`) é a
  colheita desta missão começando na própria horta. Destino: o que Lysa ensina (ficha `lysa`, C8).
- **Reputação lida:** Sera cordial ensina ("Assim, olha"); Sera fria compete ("Escreve a sua, que eu escrevo a minha").
  Mesmos objetivos.
- **Aceite observável:** usar unguento com 0 unguentos falha sem mudar nada. O bicho manso só aparece com
  `evento.q05_concluida`.

### E2-B07 — O acontecimento do destino **[PROPOSTA]** (~2 min cada; um por save)

Os quatro destinos listam acontecimentos que hoje **nenhum sistema lê** (`GDD.md` 02.6). O ep. 2 dá conteúdo a um por
destino, gravando o id que já está publicado no `DestinyCatalog` (ID publicado não muda). Dispara no primeiro
Descansar depois da q09 ou, no mais tardar, na conclusão da q10 (então todo save o vê antes do salto). Nenhum dá item,
moeda, domínio ou número de combate: é fala, objeto em cena e memória.

| Destino | Id gravado | O que acontece |
|---|---|---|
| `serena` | `evento.festa_da_colheita` | A família é sorteada pela vila para abrir a festa da colheita "quando a criança tiver idade de carregar a primeira cesta" — aos 11. Prepara a q14: na festa, essa família abre a mesa. |
| `normal` | `evento.visita_do_comerciante` | O fornecedor de Oren chega pela estrada sul; a criança, que vive de recado ([CÂNONE] `oportunidade.recado_da_vila`), leva o recado dele à ferraria. Aos 11, é ele quem traz as lentes de Borin (se o segredo foi contado) ou o sino da festa. |
| `dificil` (Vida Árdua) | `evento.ano_de_escassez` | A colheita da família falha; Daren vende uma ferramenta. A criança ajuda um vizinho ([CÂNONE] `oportunidade.favor_do_vizinho`), que aos 11 paga o favor na festa, emprestando a banca. |
| `ruptura` | `evento.anomalia_no_bosque` + `evento.sonho_recorrente` | À noite o amuleto rachado ([CÂNONE] `item.amuleto_rachado`) racha mais um fio e a criança vê, da janela, uma luz violeta na clareira que ninguém mais vê. Tovin não acredita e não desmente ("não sei o que tem no bosque e não finjo saber", ficha `tovin`, C8). Aos 11, a criança reconhece a luz antes dos outros: informação, não poder. |

- **Aceite observável:** cada save grava **só** o id do próprio destino. As missões que vêm depois são as mesmas nos
  quatro.
- **Guarda-corpo (ADR-0004, inv. 4):** a Ruptura vê mais cedo, não vê mais. Nenhum destino é rota ao "final verdadeiro".

### E2-B08 — A prática fora das missões **[PROPOSTA]**

Entre missões o ato I tem quatro práticas repetíveis até o teto, uma por mestre, sempre no mesmo lugar:
alvo de palha no `posto_guarda` (Marcial), lousa da manhã na `praca_centro` (Intelecto, Arcana), bancada da porta da
`ferraria` (Artesanal, Força), canteiro da `ervanaria` (Natural). Mais a tarefa da casa da origem (E2-B01). É aqui que
o jogador escolhe o que treinar; o teto (`Mastery`) diz quando parar. Sem relógio real; o período só anda por missão
e Descansar.

### E2-B09 — O salto para os 11 **[PROPOSTA]** (~3 min)

- **Jogador:** na clareira, escolhe "Seguir adiante". Recebe o aviso do que se encerra e confirma em ação separada.
- **Onde:** `bosque_clareira`.
- **Sistema:** `AgeAdvance` com o segundo `MarcoDeIdade` (`marco_idade_11`, de 8 para 11, liberado por
  `evento.q10_concluida`), `SaltoHud` reaproveitado.
- **Avanço:** confirmação explícita. Ver §4.1.
- **Se ignorar/atrasar:** fechar o aviso devolve a Auren aos 8. As opcionais e a prática continuam.
- **Aceite observável:** igual ao B13 do ep. 1 (interromper durante a transição deixa o save antes ou depois, nunca no
  meio), e **mais um**: o salto 8→11 não reabre teto de prática (mesma etapa).

---

### Ato II — aos 11 anos (fase `talentos`)

### E2-B10 — Onze anos **[PROPOSTA]** (~4 min)

- **Jogador:** acorda aos 11. Recebe a espada de freixo (comum no posto, ou marcada na ferraria). Vê o que mudou e
  quem lembra.
- **Onde:** `spawn_player` → `posto_guarda` ou `ferraria`.
- **Sistema:** `BodyByAge` (corpo de 11 dentro da base infantil, ainda sem a base adolescente), `Inventario` (entrega
  da espada pela idade, como o B15: `rec.e2_b10.espada_de_freixo`), `NpcMemory`, `PecaPorEvento`.
- **Avanço:** pegar a espada.
- **Aceite observável:** ver §4.3.

### E2-B11 — O fio do Nilo · `q13_o_fio_do_nilo` (central) **[PROPOSTA]** (~10 min)

- **Jogador:** Nilo, aos 11, é aprendiz de guarda ([CÂNONE] "quero ser guarda", `dialogo.nilo.aos_oito_cumprida`). Ele
  achou de novo o buraco no espinheiro por onde passou aos 5 e quer saber o que tem do outro lado. A criança segue com
  ele (ou atrás dele) até o espinhal, enfrenta os feixes-de-espinho e acha, preso nos espinhos, um fio de luz
  turquesa e violeta. Nilo encosta no fio e lembra mais: "O círculo tinha um vão. E do outro lado alguém contava. Como o
  Tovin conta quem entra." Ele pede segredo.
- **Onde:** `posto_guarda` → `entrada_bosque` → `bosque_trilha` → `bosque_espinhal`.
- **Sistema:** Quest, Combat (`enc.espinhal_feixes`), Reputação (Nilo), Percepção (rastro), Mastery.
- **Avanço:** `nilo_no_posto` → `o_buraco_no_espinheiro` → `seguir_o_fio` → `o_que_nilo_lembra` → `decidir_o_segredo_de_nilo`.
- **Desfechos:** `evento.q13_contou_a_maelis` **ou** `evento.q13_guardou_com_nilo`. É a pergunta da q04 de novo, com
  três anos a mais:
  - **Guardou:** Confiança de `nilo` +20. Na q15 ele confia na criança para ir na frente.
  - **Contou:** Renome `com_auren` +10 e Maelis escreve uma segunda linha na folha que continua de pé ("o fio existe").
    Nilo: −10 se a promessa da q04 foi cumprida ("de você eu não esperava"), 0 se foi quebrada ("de novo, né").
- **Reputação lida (q04):** Nilo cordial: chama a criança no posto, de manhã ("Vem!", o mesmo da ficha `nilo`) e
  mostra o buraco. Nilo frio: não chama; à tarde ele não está no posto e a forquilha está cravada outra vez na linha, como
  aos 5 (eco direto da ficha `nilo`, C4). A criança acha o buraco pelo rastro: com Percepção ≥ 2 os rastros brilham a
  8 m; abaixo disso, a 3 m (§3.3). Os dois caminhos chegam ao mesmo espinhal.
- **Se ignorar/atrasar:** central. A q14 pede a q13.
- **Destino/origem:** destino, como Nilo vê a criança (ficha `nilo`, C8). Origem: igual.
- **Aceite observável:** os dois desfechos, salvos e recarregados, mudam a fala de Nilo e de Maelis; exatamente um no
  histórico.

### E2-B12 — A festa · `q14_a_festa_da_colheita` (central) **[PROPOSTA]** (~12 min)

- **Jogador:** Auren faz a festa da colheita na praça. A criança escolhe **três de seis bancas**, uma por afinidade:
  o alvo de Tovin (Marcial), a prova de Borin (Artesanal), as ervas de Lysa (Natural), as perguntas de Eira (Arcana e
  Intelecto), a troca de Oren (Social) e a corrida até o vão (Exploratória). Depois, a dança da corda com Sera e Nilo.
  Na noite, a anomalia acorda: o bosque cala, o chifre de Tovin toca **o alarme** e a vila se junta na praça.
- **Onde:** `praca_centro` (bancas), `entrada_bosque` (corrida), `portao_sul` (guardiões).
- **Sistema:** Quest (objetivos paralelos nas bancas), Mastery (cada banca é uma atividade, uma vez por festa),
  Reputação (Sera, Renome), `NpcAgenda.Interrupcao` com `PrioridadeEvento` **pela primeira vez por um evento da vila**
  (lacuna 06.5).
- **Avanço:** `abrir_a_festa` → `tres_bancas` (paralelo, quaisquer três) → `a_danca_da_corda` → `o_chifre_na_noite`.
- **Regra de período [PROPOSTA, pede nota ao ADR-0007 §1]:** a festa é um dia autorado. Durante a q14 o período fica
  em Tarde; vira Noite quando `tres_bancas` se cumpre. É a única exceção à regra "o período só anda por missão
  concluída ou Descansar", e está presa a um objetivo, não a relógio.
- **Se ignorar/atrasar:** central. As bancas não expiram até a terceira; depois da noite, a festa acabou (as bancas
  que sobraram viram fala: "ano que vem").
- **Destino/origem:**
  - Serena (`evento.festa_da_colheita` gravado no E2-B07): a família abre a mesa (`abrir_a_festa` com fala de Mara/Daren).
    Os outros três destinos: Maelis abre. Mesmo objetivo.
  - Normal: o comerciante da visita toca o sino de abertura.
  - Árdua: o vizinho do favor empresta a banca à família; sem isso a família trabalharia a festa inteira (a fala diz).
  - Ruptura: o amuleto racha no fim da dança, um instante antes do bosque calar.
  - Origem: a banca da família muda (§3.7).
- **Reputação lida:** Sera cordial faz dupla nas perguntas de Eira; fria, disputa contra. Na dança, Nilo e Sera só dão
  a mão à criança se a Confiança com cada um não for fria; frio, dançam com outro par e a fala diz por quê. Renome
  `com_auren` +10 ao concluir (HIPÓTESE v0); a faixa é lida por Maelis no fim do episódio.
- **Aceite observável:** cada banca rende **uma** prática por festa (fazer a mesma banca de novo não paga). Três
  bancas diferentes cobrem três afinidades diferentes. No alarme, os 10 NPCs deixam a rotina e vão para a praça
  (Nilo e Tovin para a linha).

### E2-B13 — O chifre na noite **[PROPOSTA]** (fim da q14)

- **Jogador:** ouve o alarme e vê a vila reagir: Mara amarra uma tira nova no vão ([CÂNONE] da ficha `mara`, a tira
  como limite), Maelis conta quem está na praça, Lysa separa unguentos.
- **Sistema:** a terceira regra do chifre (ficha `tovin`, C4): toque de alarme, por evento, não por período.
  O HUD diz "alarme" por texto (nada depende só de som, GDD cap. 09).
- **Aceite observável:** a q15 abre na mesma gravação em que a q14 conclui. Recarregar não toca o alarme de novo.

### E2-B14 — A descida · `q15_a_anomalia_no_bosque` (central) **[PROPOSTA]** (~12 min)

- **Jogador:** Tovin decide quem desce com ele: a criança e Nilo (sempre); Sera, se a Confiança dela for cordial
  (ela marca o caminho na tabuinha). Na linha, Lysa dá dois unguentos a cada um. A trilha agora está cheia: abafadores e
  feixes juntos, depois o lajeiro no lajedo. No fim, o chão do coração do bosque não tem som.
- **Onde:** `entrada_bosque` → `bosque_trilha` → `bosque_espinhal` → `bosque_lajedo` → `bosque_coracao`.
- **Sistema:** Combat (três encontros: `enc.trilha_misto`, `enc.espinhal_feixes_noite`, `enc.lajedo_lajeiro`),
  Inventário (unguento), Reputação (Tovin, Sera), Mastery.
- **Avanço:** `reunir_na_linha` → `marcar_o_caminho` → `atravessar_a_trilha` → `o_lajedo` → `chegar_ao_coracao`.
- **`marcar_o_caminho`:** com cravos (q11) ou estacas (q09), a criança marca a volta; com Exploratória ≥ 2, acha a volta
  pelo lado e marca menos. Sem nada, Tovin marca e diz "da próxima, você". Os três cumprem.
- **Reputação lida:** Tovin cordial (da q09) deixa a criança ir na frente na trilha; neutro, vai atrás. Mesmo encontro.
- **Se ignorar/atrasar:** central. Voltar à vila no meio é permitido (o bosque não tranca, regra do B10); os encontros
  já vencidos ficam vencidos (§3.4).
- **Aceite observável:** os três encontros se vencem com afinidades e atributos iniciais (caminho-base). Lysa entrega os
  unguentos uma vez (`rec.q15_a_anomalia_no_bosque.unguentos`), não importa quantas vezes a criança volte à linha.

### E2-B15 — O Enredado · `q16_o_enredado` (central, encontro maior) **[PROPOSTA]** (~8 min)

- **Jogador:** no coração do bosque, um lajeiro velho, duas vezes maior que os outros, está preso por fios de luz que
  formam sobre ele um anel aberto, o mesmo desenho do símbolo, e o movem como marionete. Os abafadores se juntam em
  volta dele: é deles o silêncio. A criança não precisa matar nada: precisa **cortar os três nós** que prendem os fios.
  Cada nó aceita mais de um jeito (§3.4). Quando o terceiro solta, o anel se desfaz, os fios recolhem pela trilha em
  direção à clareira e o lajeiro velho vai embora devagar, para o fundo.
- **Onde:** `bosque_coracao`.
- **Sistema:** Combat (`enc.coracao_enredado`), afinidades como caminhos (§3.2), companheiros por reputação (§3.6).
- **Avanço:** `ver_o_enredado` → `cortar_os_tres_nos` → `seguir_os_fios`.
- **Se ignorar/atrasar:** pode sair e voltar. Nós cortados **não** ficam gravados no meio da luta (§5.2, E-03).
- **Aceite observável:** o encontro fecha pelo caminho-base (ataque leve no nó, na janela depois de o Enredado bater no
  tronco do canto) com **todas** as afinidades em 0, e também fecha por pelo menos dois caminhos de afinidade por nó.

### E2-B16 — O segundo vão (gancho) **[PROPOSTA]** (~3 min)

- **Jogador:** segue os fios até a clareira. O símbolo do Limiar agora tem **dois vãos**. Volta a Auren de madrugada.
  Maelis escreve no livro e não deita a folha. Nilo diz que alguém, do outro lado, ainda conta. A tela "Fim do Despertar
  dos Talentos" abre.
- **Onde:** `bosque_clareira` → `praca_centro`.
- **Sistema:** `PecaPorEvento` (o símbolo com dois vãos, ligado a `evento.q16_concluida`); tela final grava
  `marco.fim_do_despertar` uma vez, como o B16 do ep. 1.
- **Aceite observável:** a tela não nomeia a Primeira Fratura, não diz o que é o segundo vão e **não promete data, lugar
  nem episódio** ("continua em…" é proibido, como no ep. 1). O salto para os 12 (`formacao`) **não** é oferecido neste
  episódio: os nós do símbolo ficam turquesa e o prompt não aparece.
- **Gancho do ep. 3, sem promessa:** o anel que era um vão virou dois; o que conta do outro lado; a folha de pé.

---

## §3 — As missões do episódio 2

Formato de dados = `content/quests/_schema.json` (versão 1). Fase de todas: `talentos`. Ids novos, `snake_case`,
estáveis. **Seis centrais e duas opcionais.** Nenhuma central depende de opcional (o validador cobra).

### 3.1 Tabela

| Id | Título | Tipo | Central? | Idade | Pré-condição | Âncora | NPCs |
|---|---|---|---|---|---|---|---|
| `q09_a_linha_do_posto` | A Linha do Posto | cotidiana | **central** | 8–10 | `marco.fim_da_primeira_existencia` + direito de acesso do ep. 2 (fora do save) | `posto_guarda` | tovin |
| `q10_a_lousa_e_o_livro` | A Lousa e o Livro | narrativa | **central** | 8–10 | q09 concluída | `praca_centro` | eira, maelis |
| `q11_o_aro_de_provas` | O Aro de Provas | cotidiana | opcional | 8–10 | q09 concluída | `ferraria` | borin, tovin |
| `q12_as_ervas_que_curam` | As Ervas que Curam | social | opcional | 8–10 | q09 concluída | `ervanaria` | lysa, sera |
| `q13_o_fio_do_nilo` | O Fio do Nilo | narrativa | **central** | 11 | q10 concluída + `marco_idade_11` | `posto_guarda` | nilo, tovin, maelis |
| `q14_a_festa_da_colheita` | A Festa da Colheita | social | **central** | 11 | q13 concluída | `praca_centro` | os 10 |
| `q15_a_anomalia_no_bosque` | A Anomalia no Bosque | exploratoria | **central** | 11 | q14 concluída | `entrada_bosque` | tovin, nilo, sera, lysa |
| `q16_o_enredado` | O Enredado | narrativa | **central** | 11 | q15 concluída | `bosque_coracao` | tovin, nilo |

`idade_max` 10 nas missões do ato I: expiram no salto e entram no aviso (§4.1). `idade_max` 11 nas do ato II (o salto
para 12 não existe neste episódio). A q14 lista os dez NPCs porque o alarme interrompe a rotina de todos.

### 3.2 Objetivos, recompensas e histórico

Recompensa = `rec.<missao>.<item>`, idempotente pelo id (`LifeEventLedger`). Quantidades: HIPÓTESE v0.

**`q09_a_linha_do_posto`** (dados em `content/quests/episodio2/`, espelho em `QuestCatalogEp2`; ainda fora do
`QuestSystem` e da cena: faltam a âncora `bosque_trilha` e o encontro)
- Objetivos (em ordem): `ouvir_o_toque` (`posto_guarda`, tovin) · `ronda_ate_o_vao` (`entrada_bosque`, tovin) ·
  `ler_o_rastro` (`bosque_clareira`, tovin) · `primeira_luta_na_borda` (`bosque_trilha`, encontro `enc.trilha_abafadores_borda`) ·
  `guiar_a_volta` (`posto_guarda`, tovin).
- Recompensas: `rec.q09_a_linha_do_posto.marco_primeira_ronda` (marco `marco.primeira_ronda`) ·
  `rec.q09_a_linha_do_posto.estacas` (item `item.estaca_de_freixo`, 3, hipótese).
- Histórico: `evento.q09_concluida` (tovin; flag `primeira_ronda_feita`). Fora da missão, se acontecer:
  `descoberta.marcial.bloqueio_no_ultimo_instante` (HM-02; tovin testemunha se estava no pátio).
- Se ignorada: central; trava o ato I.

**`q10_a_lousa_e_o_livro`**
- Objetivos (em ordem): `aula_na_praca` (`praca_centro`, eira) · `responder_a_lousa` (`praca_centro`, eira) ·
  `ler_o_mural_a_noite` (`mural_avisos`, eira) · `a_pagina_assinada` (`mural_avisos`, maelis) ·
  `o_sinal_na_clareira` (`bosque_clareira`, sem NPC: gatilho na âncora).
- Recompensas: `rec.q10_a_lousa_e_o_livro.marco_primeira_leitura` (`marco.primeira_leitura`) ·
  `rec.q10_a_lousa_e_o_livro.marco_fio_de_luz` (`marco.fio_de_luz`: a técnica).
- Histórico: `evento.q10_concluida` (eira, maelis; flags `sabe_ler`, `fio_de_luz_aprendido`, `salto_onze_liberado`).
- `responder_a_lousa`: Intelecto ≥ 2 cumpre em uma aula; abaixo, em duas (a segunda no dia seguinte). Mesmo resultado.
- Se ignorada: central; sem ela não há salto para os 11.

**`q11_o_aro_de_provas`**
- Objetivos (em ordem): `voltar_a_ferraria` (`ferraria`, borin) · `ser_os_olhos` (`ferraria`, borin) ·
  `forjar_os_cravos` (`ferraria`, borin) · `decidir_o_segredo` (`ferraria`, borin; objetivo de desfecho) ·
  `entregar_a_tovin` (`posto_guarda`, tovin).
- Desfechos: `evento.q11_segredo_guardado` / `evento.q11_segredo_contado` (`desfecho_no_objetivo`: `decidir_o_segredo`).
- Recompensas: `rec.q11_o_aro_de_provas.cravos` (item `item.cravo_de_marcar`, 4, hipótese) ·
  `rec.q11_o_aro_de_provas.moedas` (6, hipótese).
- Histórico: `evento.q11_concluida` (borin, tovin; flag `aprendiz_de_borin`); os dois desfechos (borin; o contado também
  maelis).
- Se ignorada: opcional; expira no salto; a espada de freixo vem comum, salvo `confianca_de_borin`.

**`q12_as_ervas_que_curam`**
- Objetivos (em ordem): `colher_com_lysa` (`entrada_bosque`, lysa) · `a_tabuinha_de_sera` (`casa_sera`, sera) ·
  `preparar_o_unguento` (`ervanaria`, lysa, sera) · `usar_numa_ferida` (`ervanaria`, sem NPC obrigatório).
- Recompensa: `rec.q12_as_ervas_que_curam.marco_unguento` (`marco.unguento_de_lysa`). **Nenhum item a mais:** os
  unguentos da q15 são de todos.
- Histórico: `evento.q12_concluida` (lysa, sera; flag `preparou_unguento`).
- Se ignorada: opcional; expira no salto; os unguentos chegam do mesmo jeito.

**`q13_o_fio_do_nilo`**
- Objetivos (em ordem): `nilo_no_posto` (`posto_guarda`, nilo) · `o_buraco_no_espinheiro` (`entrada_bosque`, nilo) ·
  `seguir_o_fio` (`bosque_espinhal`, encontro `enc.espinhal_feixes`) · `o_que_nilo_lembra` (`bosque_espinhal`, nilo) ·
  `decidir_o_segredo_de_nilo` (`bosque_espinhal`, nilo; objetivo de desfecho).
- Desfechos: `evento.q13_contou_a_maelis` / `evento.q13_guardou_com_nilo`.
- Recompensa: `rec.q13_o_fio_do_nilo.marco_fio_do_nilo` (`marco.fio_do_nilo`).
- Histórico: `evento.q13_concluida` (nilo, tovin; flag `fio_achado`); desfechos (nilo; o contado também maelis).
- Se ignorada: central; trava a festa.

**`q14_a_festa_da_colheita`**
- Objetivos: `abrir_a_festa` (`praca_centro`, maelis ou família) → `tres_bancas` (paralelo, `praca_centro`/`entrada_bosque`)
  → `a_danca_da_corda` (`praca_centro`, sera, nilo) → `o_chifre_na_noite` (`praca_centro`, tovin). `objetivos_em_ordem`
  true; `tres_bancas` é um objetivo de contagem (3 de 6 atividades `festa.banca_*`, cada uma com id próprio).
- Recompensas: `rec.q14_a_festa_da_colheita.fita` (item `item.fita_da_colheita`, 1: a marca da festa na manta, como as
  marcas do ep. 1) · `rec.q14_a_festa_da_colheita.moedas` (5, hipótese).
- Histórico: `evento.q14_concluida` (os 10; flag `festa_vivida`) · `evento.anomalia_despertou` (os 10; flag
  `alarme_tocado`). **Id novo** e universal, diferente do `evento.anomalia_no_bosque` (que continua sendo o
  acontecimento só da Ruptura, E2-B07).
- Se ignorada: central; trava a q15.

**`q15_a_anomalia_no_bosque`**
- Objetivos (em ordem): `reunir_na_linha` (`entrada_bosque`, tovin, lysa) · `marcar_o_caminho` (`bosque_trilha`) ·
  `atravessar_a_trilha` (`bosque_trilha`, `enc.trilha_misto`) · `o_lajedo` (`bosque_lajedo`, `enc.lajedo_lajeiro`) ·
  `chegar_ao_coracao` (`bosque_coracao`, tovin, nilo).
- Recompensas: `rec.q15_a_anomalia_no_bosque.unguentos` (item `item.unguento`, 2, hipótese) ·
  `rec.q15_a_anomalia_no_bosque.marco_noite_no_bosque` (`marco.noite_no_bosque`).
- Histórico: `evento.q15_concluida` (tovin, nilo, e sera se desceu; flag `desceu_ao_coracao`).
- Se ignorada: central; trava a q16.

**`q16_o_enredado`**
- Objetivos (em ordem): `ver_o_enredado` · `cortar_os_tres_nos` (`enc.coracao_enredado`) · `seguir_os_fios`
  (`bosque_clareira`, gatilho).
- Recompensa: `rec.q16_o_enredado.marco_enredado_livre` (`marco.enredado_livre`).
- Histórico: `evento.q16_concluida` (tovin, nilo; flags `enredado_libertado`, `segundo_vao_visto`).
- Se ignorada: central e última. A tela final grava `marco.fim_do_despertar` uma vez.

Entregas pela idade (fora de missão, padrão do `Inventario.EntregarEspada`): `rec.e2_b10.espada_de_freixo` entrega
**ou** `item.espada_de_freixo` **ou** `item.espada_de_freixo_marcada` (marcada se `confianca_de_borin` ou
`evento.q11_concluida`), uma vez, aos 11.

### 3.3 Afinidades e atributos: o que cada um faz **[PROPOSTA]**

Princípio (GDD cap. 03 e 04): afinidade e atributo **abrem caminho e mudam a execução**, nunca multiplicam tudo nem
trancam caminho completo. Toda trava de afinidade tem um caminho-base que não pede nada. Limiares: **1** e **2**
(HIPÓTESE v0; o `Mastery` permite até 4 pontos por trilha nesta etapa). Regra de alcance: **cada limiar é alcançável só
com as atividades das missões centrais e da prática livre, em qualquer origem** (teste em §5.1).

**Afinidades (seis, todas com efeito)**

| Afinidade | Efeito no ep. 2 | Onde se vê | Caminho-base de quem não tem |
|---|---|---|---|
| **Marcial** | ≥ 1 e a descoberta do HM-02 (bloquear no último instante, na q09): abre a técnica **contra** (bloqueio no último instante devolve um golpe). ≥ 2: o contra derruba um feixe aberto. | combate em toda a trilha; nó da frente do Enredado | bloqueio comum + ataque depois do bote |
| **Arcana** | ≥ 1: o **fio de luz** (aprendido na q10) prende um feixe aberto ou um fio do Enredado por 2 s. ≥ 2: alcance de 6 m em vez de 3 m. | q13, q15, q16 | o fio de luz existe para todos a partir da q10; Arcana só muda alcance e uso |
| **Natural** | ≥ 1: ficar parado 3 s perto de um bando de abafadores o dispersa sem luta (o encontro conta como vencido; "chegar devagar" da q05 crescido). ≥ 2: o mesmo com um lajeiro que não investiu ainda. | trilha, lajedo, nó de trás do Enredado | lutar |
| **Artesanal** | ≥ 1: na bancada (casa ou ferraria) faz cravos com 2 estacas velhas. ≥ 2: um cravo fincado prende uma perna de feixe ou um fio do Enredado. | q15 `marcar_o_caminho`; nó do flanco | Tovin marca; cortar o fio na janela |
| **Social** | ≥ 1: na troca de Oren, troca favor por mercadoria (HM-05, quando não há moeda). ≥ 2: no Enredado, **chamar** um companheiro presente (Nilo, Sera ou Tovin) para segurar a atenção dele 4 s. | q14, q16 | esperar a janela |
| **Exploratória** | ≥ 1: os rastros e os desvios aparecem no indicador de objetivo. ≥ 2: a volta pelo lado na `bosque_trilha` (evita o tronco) e o caminho pelo lajedo alto até o nó de cima do Enredado. | q13, q15, q16 | o caminho do meio (mais longo, com luta) |

**Atributos (seis, cada um lido por pelo menos uma regra; lacuna 03.5)**

| Atributo | Regra (HIPÓTESE v0) | Onde |
|---|---|---|
| **Força** | Ataque forte **quebra a postura** de criatura com `peso` ≤ Força + 1 (abafador peso 1, feixe 2, lajeiro 3, Enredado 4). Força ≥ 3 empurra o tronco do atalho da `bosque_trilha`. Na q11, Força ≥ 2 forja o cravo em duas batidas em vez de três (mesmo cravo). | combate; q11; q15 |
| **Agilidade** | Janela para encadear leve → forte: 0,30 s + 0,04 s por ponto, teto 0,50 s. | combate |
| **Vigor** | Barra de Vigor máxima: base + 5 por ponto, teto +25. | combate |
| **Intelecto** | Aulas da q10 em uma vez com Intelecto ≥ 2 (senão duas). Ler a página velha (pista extra da q10 com o círculo) pede Intelecto ≥ 1; sem ele, Eira lê em voz alta (mesma pista, outra fala). | q10 |
| **Percepção** | Distância em que rastros e feixes disfarçados ganham contorno: 3 m + 1 m por ponto, teto 8 m. O aviso visual do bote do feixe começa 0,1 s antes por ponto, teto 0,3 s. | q13, trilha, espinhal |
| **Vontade** | A canalização do fio de luz aguenta golpes sem quebrar: 0 golpes + 1 a cada 2 pontos, teto 2. Segurar o fio do Enredado pede canalizar 2 s. | q16; combate |

Nenhum atributo é **requisito** de missão central: todos mudam execução ou abrem atalho. O teste
`EncontrosFecham_ComAtributosEAfinidadesIniciais` (§5) prova isso para cada encontro.

### 3.4 Combate: as criaturas do Bosque dos Sussurros **[PROPOSTA]**

Todas originais e sujeitas ao portão do ADR-0002 (ficha G1 → concept G2 → licença G3) **antes de qualquer malha**.
Descrição no padrão das fichas (necessidade do mundo, três formas, cor, regra). **Derrotar = dispersar**: as
criaturas fogem, não morrem em cena (público misto com crianças, ADR-0009) e não deixam item, moeda nem XP
(GDD cap. 05, "sem XP por golpe"). O que se ganha lutando é prática no `Mastery`, com teto.

**Abafador** (`criatura.abafador`) — papel: bando leve; ensina esquiva e a ler o grupo.
- *Necessidade do mundo:* o Bosque dos Sussurros "abafa o som" (ficha `tovin`, C2, PROPOSTA aprovada no G1): o
  abafador é o porquê. O leque dele come o som. É também o "silêncio enorme" de que Nilo lembra.
- *Três formas:* (1) bola achatada de folha seca e musgo, 0,45 m; (2) leque de folhas largas atrás do corpo, que abre
  até 0,70 m quando alarmado, maior que a bola; (3) sem pernas visíveis: anda em pulos rasos.
- *Cor:* corpo verde-musgo escuro (Verde #648B67 sombreado), leque oliva-palha; dois pontos claros de olho.
- *Regra:* vem em 3 a 5. Perto do bando, o áudio do ambiente cai (aviso diegético) e o HUD mostra "silêncio" em texto
  (não depende só de som). Um de cada vez abre o leque (0,6 s, o aviso) e salta. Quando sobra um, ele foge.
- *Natural ≥ 1:* ficar parado 3 s perto dispersa o bando.

**Feixe-de-espinho** (`criatura.feixe_de_espinho`) — papel: emboscador; ensina percepção e bloqueio.
- *Necessidade do mundo:* o espinheiro do vão "fecha de novo" todo dia e Tovin o corta com o podão (ficha `tovin`,
  C2). O feixe é a parte do espinheiro que anda: é por isso que fecha.
- *Três formas:* (1) fechado, um feixe vertical de galhos de 1,3 m, igual ao espinheiro em volta; (2) aberto, três
  pernas longas e finas em tripé e 1,8 m de altura, com um miolo pendurado no centro (o alvo); (3) espinhos longos de
  ponta clara, eriçados no bote.
- *Cor:* casca cinza-terracota (Terracota #A86D52 dessaturado), pontas marfim (#E9DEC6).
- *Regra:* parado, é mata. O bote é em linha, 2 m; o aviso é um estalo seco e os espinhos eriçando (0,8 s). Bloqueio
  frontal anula o bote. Depois do bote, fica aberto 1,5 s: a janela. Fio de luz e cravo prendem o tripé aberto.

**Lajeiro** (`criatura.lajeiro`) — papel: bruto lento e blindado; ensina flanco, ataque forte e paciência.
- *Necessidade do mundo:* o bosque não tem pedreira, mas tem lajedos de pedra nua: são os lajeiros que raspam o chão e
  largam laje. As lajes da praça de Auren vieram do bosque, pela mão de Oren (PROPOSTA de mundo, a conferir com a ficha
  `oren`).
- *Três formas:* (1) quadrúpede baixo e largo, 1,0 m de altura por 1,8 m; (2) dorso em escada de lajes chatas, da
  cabeça à cauda, a da frente servindo de viseira; (3) patas curtas e largas como pilões.
- *Cor:* cinza-ardósia de pedra molhada, musgo verde nas juntas das lajes.
- *Regra:* raspa a pata 1 s (som e poeira: o aviso) e investe em linha reta. De frente, o bloqueio não segura (empurra).
  De lado, as juntas das lajes abrem: é ali que se bate. Ataque forte com Força suficiente derruba uma laje e o
  atordoa 1,5 s. Se investir contra um tronco, fica preso 2 s.

**Encontro maior — O Enredado** (`criatura.enredado`) — papel: o fim do episódio; luta de **libertar**, não de matar.
- *Necessidade do mundo:* a anomalia (Trama) prendeu o lajeiro mais velho do bosque. Os fios dela fazem o desenho do
  símbolo do Limiar, com um vão. Liga o bosque ao mistério principal sem explicá-lo.
- *Três formas:* (1) massa baixa de lajes, 2 m de altura por 3,5 m; (2) acima do dorso, um anel de fios de luz de 2,4 m,
  aberto por um vão; (3) três fios esticados do anel até o chão, como cordas de tenda, cada um preso num nó.
- *Cor:* lajes cinza-ardósia; fios Violeta #9777B8 (anomalia) trançados com Turquesa #86C8C9 (Trama), as duas cores
  que a paleta reserva para isso (GDD cap. 09).
- *Regra:* os fios puxam o lajeiro como marionete: investidas mais longas que as de um lajeiro e uma varredura com a
  laje da frente. Os abafadores em volta tiram o som do lugar (o HUD diz "silêncio"). Cada nó cortado solta um fio:
  ele fica mais lento. Três nós: o anel se desfaz e os fios recolhem para a clareira.
- *Os três nós e os jeitos de cortar (cada nó aceita o caminho-base e pelo menos dois de afinidade):*

| Nó | Caminho-base (nada exigido) | Caminhos de afinidade |
|---|---|---|
| **Da frente** (na laje-viseira) | ataque leve no nó enquanto ele está preso no tronco depois de investir | Marcial (contra no fim da varredura) · Força (ataque forte que quebra a postura) |
| **Do flanco** (fio esticado à esquerda) | ataque leve no fio na mesma janela | Artesanal (cravo prende o fio e Nilo corta) · Arcana + Vontade (fio de luz canalizado 2 s desfaz o nó) |
| **De cima** (no topo do anel) | o Enredado baixa a cabeça depois da terceira investida; o nó desce ao alcance por 2 s | Exploratória (lajedo alto, alcança sem esperar) · Natural (chegar devagar enquanto ele respira entre investidas) · Social (chamar um companheiro para segurar a atenção dele) |

### 3.5 Assistências de combate **[PROPOSTA]** (resposta à decisão S.ASSIST; regra do HM-04; ADR-0004 inv. 3)

**Regra:** a assistência mexe na **execução**, nunca no **crescimento**. Não muda `Mastery`, recompensa, marco, fala,
destino, origem nem histórico. Fica em `Configuracoes` (preferência do aparelho), **não** no `SaveData`. Liga e desliga a
qualquer hora, inclusive no meio da luta, sem rastro no save de progresso. Destino não liga nem sugere assistência.

| Id | Assistência | O que muda |
|---|---|---|
| `assist.esquiva_larga` | Esquiva larga | i-frames da esquiva de 0,35 s para 0,60 s |
| `assist.dano_reduzido` | Golpes mais leves | dano recebido × 0,5 |
| `assist.aviso_longo` | Aviso longo | o aviso de toda criatura dura 50% mais |
| `assist.mira` | Mira assistida | o golpe vira para a criatura mais próxima à frente |
| `assist.toque_no_traco` | Tocar em vez de traçar | botão "Tocar" no lugar do gesto do fio de luz e do traço |

**Sem assistência, ninguém é punido por perder:** derrota = Tovin puxa a criança para trás, Vida cheia, o encontro
recomeça do início, nada perdido. Isso vale para todos e não é assistência.

### 3.6 Reputação lida por falas **[PROPOSTA]** (lacuna 06.2 e 13.3)

Faixas já no código (`ReputationFaixa`, HIPÓTESE v0): hostil ≤ −60 · frio ≤ −20 · neutro · cordial ≥ 20 · leal ≥ 60.
O diálogo ramifica **por faixa**, nunca por número cru. Fontes novas de reputação neste episódio: q09 (Tovin +10),
q11 (Borin ±20, Renome +10), q13 (Nilo +20 / −10 / 0, Renome +10), q14 (Renome +10).

| NPC | Fonte | O que a faixa muda | Onde |
|---|---|---|---|
| Nilo | q04, q13 | cordial: chama para seguir o fio, dança, vai na frente no Enredado; frio: vai sozinho, dança com outro | q13, q14, q16 |
| Sera | q04 | cordial: dupla na festa, desce ao bosque; frio: compete, fica com Lysa | q12, q14, q15 |
| Tovin | q09 | cordial: a criança lidera a trilha; neutro: vai atrás | q15 |
| Borin | q11 | cordial: "meus olhos"; frio: "enxergo, e não esqueço" | E2-B10, q14 |
| Maelis | Renome `com_auren` | cordial: no fim, lê o nome da criança no livro em voz alta; neutro: só escreve | E2-B16 |
| Mara, Daren, Lysa, Eira, Oren | Confiança deles (ainda sem fonte no ep. 1) | uma fala de faixa cada, que só aparece fora do neutro; no ep. 2 elas sobem por presença nas bancas da festa (+5 por banca do mestre, HIPÓTESE v0; Mara e Daren pela tarefa da casa até o teto) | q14, E2-B16 |

Regra do ep. 1 mantida: **fala lê estado, não muda estado.** Quem muda reputação é o desfecho ou a conclusão de missão.

### 3.7 Variantes por origem: casa, família e uma fala **[PROPOSTA]** (H5; lacuna 02.5 e S.B03)

| Origem | Casa (aos 8 → aos 11) | Tarefa da família (HM-06a) | Banca na festa (q14) | Uma fala (Daren, aos 11) |
|---|---|---|---|---|
| `agricultores` | cestos de colheita na porta, a horta (`horta_familia`) cresce entre 8 e 11 | colher na horta com Mara (Natural) | a mesa da colheita: a família traz a primeira cesta | "Terra não tem pressa. Você aprendeu isso antes de mim." |
| `artesaos` | bancada na frente da casa, ferramentas penduradas; aos 11 a bancada tem duas alturas (a sua) | consertar ferramenta com Daren (Artesanal) | a banca de peças: a família vende o que fez no ano | "Peça boa é a que volta para conserto, não a que quebra." |
| `guardioes` | cabide de lanças de ronda e uma lanterna na porta; aos 11, uma segunda lanterna | ronda em volta da casa à noite com Daren (Exploratória) | a família guarda o `portao_sul` durante a festa e chega na praça no alarme | "Na festa, alguém tem de olhar para fora. Hoje fomos nós." |

Destino muda o **estado** dos objetos (o pano inteiro, liso ou remendado, como a manta: [CÂNONE] `DestinyCatalog`),
nunca a lista. Prova de que é variação e não campanha: as três origens fazem as mesmas oito missões.

---

## §4 — As transições

### 4.1 O salto 8 → 11: aviso, confirmação **[PROPOSTA]** (mesmas regras do SLICE §4.1–4.2)

O aviso nomeia:
1. **Opcionais não concluídas** pelo título canônico: "O Aro de Provas", "As Ervas que Curam". (Some se não há.)
2. **Vínculos que mudam:** Nilo vira aprendiz de guarda; Sera passa a dividir a ervanaria com Lysa; Borin muda de
   mesa ou de lentes (conforme a q11).
3. **O que se encerra:** a espada de madeira fica curta; as práticas dos 8 continuam, **mas o que já deram nesta etapa
   não volta** (dito assim; é a regra do `Mastery`, mesma etapa).
4. **O que sobrevive:** nome, destino, origem, histórico, promessas, confiança, moedas, itens, domínio.

Confirmação em ação separada, com rótulo que diz o que acontece ("Deixar os oito anos para trás"). Transação idempotente
(`marco_idade_11`). O salto não concede atributo, item (a espada de freixo é entrega pela idade, do mesmo jeito que a do
B15, com id próprio), grau nem ascensão.

### 4.2 O depois: o que muda e o que tem de ser lembrado **[PROPOSTA]**

| Muda visivelmente | Como |
|---|---|
| Corpo | 1,28 m → ~1,42 m (HIPÓTESE), ainda base infantil; a manta vira capa curta (a manta de nascimento é a mesma, dobrada). |
| Nilo e Sera | 11 anos, outra rotina: Nilo no posto de manhã e na linha à tarde; Sera na ervanaria. |
| Auren | as peças da festa sendo montadas na praça; a casa da origem com o objeto novo (§3.7). |
| Bosque | o espinheiro mais alto; o símbolo com nós turquesa (há algo a fazer de novo). |

| Tem de ser lembrado | Origem | Quem lembra |
|---|---|---|
| Vocação aceita ou recusada | `evento.vocacao_aceita` / `..._recusada` | Maelis e o mestre proposto |
| O segredo de Borin | `evento.q11_segredo_guardado` / `..._contado` | Borin (e Maelis, se contado) |
| A primeira ronda | `evento.q09_concluida` | Tovin |
| O unguento | `evento.q12_concluida` | Lysa e Sera |
| O acontecimento do destino | o id do E2-B07 | só os NPCs que o testemunham pela tabela |

Regra negativa igual à do ep. 1: nenhum NPC cita evento ausente do histórico daquele save.

### 4.3 O fim do episódio **[PROPOSTA]**

Ver E2-B16. Ponto natural de fim: a criança volta a Auren de madrugada, com o bosque calmo e uma pergunta nova. A tela
final não oferece compra (não há ep. 3 à venda) e não diz "continua". Depois dela, o jogador continua em Auren aos 11,
com falas de pós-episódio, e a prática segue até o teto.

---

## §5 — Checklist de regressão do episódio 2

**Nada aqui foi executado.** Cada linha é passo jogável ou teste EditMode proposto (nome em `código`).

### 5.1 Passos de tela e testes de regra

| # | Passo | Resultado esperado |
|---|---|---|
| R-E2-01 | Sem o direito de acesso, terminar o ep. 1 e rodar Auren | Nenhuma `q09`+ aparece; o cartão aparece uma vez no fim do ep. 1 e depois só no menu. |
| R-E2-02 | Rodar o episódio inteiro com **zero** opcionais (q11, q12) | Chega ao E2-B16. O aviso do salto nomeou as duas. Os unguentos chegaram na q15. |
| R-E2-03 | Para cada um dos 4 destinos × 3 origens, jogar até o fim (`-roteiro`) | As 12 fecham; cada destino grava só o próprio id do E2-B07; números de criatura idênticos (`Criaturas_MesmosNumeros_NosQuatroDestinos`). |
| R-E2-04 | Cada encontro com todas as afinidades e atributos no valor inicial e sem assistência | Fecha pelo caminho-base (`EncontrosFecham_ComAtributosEAfinidadesIniciais`). |
| R-E2-05 | Para cada origem, só centrais + prática livre | Todo limiar de afinidade da §3.3 é alcançável (`LimiaresDeAfinidade_AlcancaveisSoComCentrais_EmCadaOrigem`). |
| R-E2-06 | Cada um dos 6 atributos | Tem pelo menos uma regra que o lê (`TodoAtributo_TemLeitor`; quebra se alguém tirar a leitura). |
| R-E2-07 | q04 cumprida × quebrada → q13, q14, q15 | Falas e companheiros diferentes, mesmos objetivos; salvar e recarregar mantém. |
| R-E2-08 | q11 sem q06 | A espada de freixo vem marcada; números iguais aos da comum (`EspadaDeFreixo_ComumOuMarcada_MesmosNumeros`). |
| R-E2-09 | Mesmo histórico, 4 destinos, E2-B02 | Mesma vocação (`Vocacao_IgualNosQuatroDestinos`); histórico vazio → "ainda aberta". |
| R-E2-10 | Alarme da q14 | Os 10 NPCs interrompem a rotina por `PrioridadeEvento`; recarregar depois não toca de novo. |
| R-E2-11 | Ligar e desligar cada assistência no meio de um encontro | `SaveData` byte a byte igual nos campos de progresso; mesma sequência de práticas com e sem assistência dá o mesmo ledger (`Assistencia_NaoMudaLedgerNemSave`, HM-04). |
| R-E2-12 | E2-B16 | A tela não nomeia a Fratura, não promete data nem lugar; o salto para 12 não é oferecido. `marco.fim_do_despertar` uma vez. |
| R-E2-13 | Conversar com todos os NPCs aos 8 e aos 11 | Nenhuma fala altera inventário, moedas, missão ou reputação (regra R8 do ep. 1 estendida). |
| R-E2-14 | Nenhum NPC cita evento ausente | Partida A faz q11 e q12, B não; nas falas aos 11, só A ouve Borin e Lysa sobre isso. |

### 5.2 Catálogo de exploits do episódio 2

| # | Exploit | Contramedida | Teste que cobre |
|---|---|---|---|
| E-01 | **Farm de afinidade**: repetir a mesma banca, a mesma tarefa da casa ou o mesmo alvo para subir afinidade e abrir os limiares cedo | `Mastery` já limita: retorno decrescente, teto por atividade (30), teto trivial (6) e **teto por trilha por etapa (240)**. Banca da festa = uma prática por festa (id `festa.banca_*` no ledger da festa). | `Obrigatorio7_*` existentes + `Festa_MesmaBancaDuasVezes_PagaUmaVez` + `TetoDeTrilha_ValeParaAsAtividadesNovasDoEp2` |
| E-02 | **Repetir encontro**: voltar à trilha depois de Descansar e bater nos abafadores de novo | Encontro vencido grava `encontro.<id>` uma vez (idempotente). Depois de Descansar, a trilha pode repovoar abafadores (prática), mas a atividade `luta.<criatura>` vira **trivial** quando o desafio da criatura fica abaixo do domínio já alcançado, e criatura não deixa item, moeda nem XP. Vencer de novo não regrava marco. | `Encontro_VencidoDuasVezes_GravaUmaVez` · `LutaRepetida_FicaTrivial_ESatura` · `Criatura_NaoConcedeItemNemMoeda` |
| E-03 | **Fechar o app no meio da luta** para desfazer um erro ou duplicar um nó cortado | Nada do encontro é gravado no meio: nós cortados, Vida, unguento gasto **na luta** e posição das criaturas são estado de cena. O save é feito antes do encontro (ao entrar na âncora) e depois (vitória, uma transação: evento + prática + objetivo). Matar o processo devolve ao antes, sem perda e sem ganho. **Exceção explícita:** unguento usado conta só na vitória; se o app fechar, o unguento volta (não há punição por queda do Android). | `Encontro_InterrompidoAntesDoCommit_VoltaAoAntes_SemGanhoNemPerda` · `Encontro_VitoriaGravaNumaSoTransacao` · `save_caos.py` com um cenário novo (matar no meio do Enredado) |
| E-04 | **Relógio do aparelho**: adiantar a data para "virar o dia", abrir a festa de novo ou recarregar algo diário | Não existe relógio real no domínio (ADR-0007 §1): o período só anda por missão concluída, Descansar e a regra autorada da festa. Nada é diário por data. A festa é uma missão de instância única, não um evento de calendário. Datas do save são só carimbo (`createdAtUtc`, `updatedAtUtc`). | `DominioNaoLeRelogio` (cerca: nenhum `DateTime.Now`/`UtcNow` fora de carimbo em `Scripts/` de domínio) · `MudarRelogio_NaoMudaPeriodoNemMissao` |
| E-05 | **Recompensa da festa ou da q15 em save/load** (unguentos, fita, moedas) | Id de transação no histórico; concessão é no-op se o id já está. | `Obrigatorio3_*` estendidos às 8 missões novas (paridade JSON × `QuestCatalog`) |
| E-06 | **Assistência como atalho de progresso** (ligar tudo, farmar, desligar) | Assistência não toca `Mastery` nem recompensa (HM-04); o ganho é o mesmo com e sem. | `Assistencia_NaoMudaLedgerNemSave` |
| E-07 | **Salto como reset de teto**: saltar 8→11 para reabrir todas as práticas | O salto é dentro da etapa `talentos`; teto é por etapa, não por idade. O aviso diz isso. | `Salto8Para11_NaoReabreTeto` |
| E-08 | **Desfecho duplo** (contar e guardar o segredo; q11, q13) | `QuestDef.Desfechos`: exatamente um; o segundo botão some. | `Q11_Q13_UmDesfechoSo_EIdempotente` |
| E-09 | **Compra que não volta ou que vem duas vezes** (IAP do ep. 2) | Direito de acesso é da loja, não do save; dois passos (concede, confirma); restauração. Comprar de novo não duplica nada no save (não há item no save). | `Episodio2_DireitoNaoMoraNoSave` · teste com a loja falsa do PC |
| E-10 | **Comprar com Oren** (HM-05) e fechar no meio | Débito e crédito no mesmo commit (tmp + replace). | `Compra_DebitoECreditoNoMesmoCommit` |

---

## §6 — Pendências que este documento abre

### 6.1 As três maiores decisões do idealizador

1. **Festa e anomalia para todos, ou só para um destino?** Hoje `evento.festa_da_colheita` é acontecimento só da Vida
   Serena e `evento.anomalia_no_bosque` só da Ruptura (`DestinyCatalog`). **Recomendado:** a festa (q14) e a anomalia
   (q15, com o id novo `evento.anomalia_despertou`) são de todos; os ids antigos ficam como o acontecimento próprio
   daquele destino (E2-B07). Alternativa: deixar exclusivos, e então dois destinos ficam sem o clímax do episódio.
2. **Um salto (8 → 11) no meio, ou outra estrutura de idade?** **Recomendado:** um salto, como no ep. 1 (vive-se muito
   numa idade e pouco na outra). Alternativas: dois saltos (8→10→11, mais aviso e mais custo de corpo) ou o episódio
   todo aos 8 com salto no fim (perde-se a criança de 11 lutando no bosque).
3. **As assistências de combate (S.ASSIST, hoje `BLOCKED_PRODUCT_DECISION`).** **Recomendado:** a regra HM-04 com as
   cinco assistências da §3.5, mais "derrota sem perda" para todos. Alternativa: só a derrota sem perda, sem
   assistências (mais simples, menos acessível).

### 6.2 Outras pendências

4. **HM-03 (leitura dos fios)** rotula a criança; precisa de revisão de texto antes de playtest com criança. Se não
   aprovado, o E2-B02 sai e nada mais depende dele.
5. **A exceção de período da festa** (§E2-B12) precisa de nota no ADR-0007 §1.
6. **Bosque com quatro âncoras novas** (§1.2): aceitar como aprofundamento do Bosque dos Sussurros (não é região nova)
   e medir no Android de entrada antes de fechar a trilha (H8).
7. **Lajes da praça vindas do bosque** (lajeiro, §3.4): conferir com a ficha `oren` antes do G1 das criaturas.
8. **Quantidades** (estacas 3, cravos 4, unguentos 2, moedas 5 e 6, reputação ±10/±20/+5): HIPÓTESE v0.
9. **Duração (60–90 min)**: estimativa de mesa; só o playtest mede (H1).
10. **HM-14 (bestiário por observação)** está como FUTURO na proposta HM; este roteiro **não** depende dele. Se for
    antecipado, a página entra no HM-01 e Tovin ensina a observar na q09.
11. **HM-07 (criatura acompanhante)** fica fora: o abafador manso da q12 é só cena e fala, não aliado.

---

## §7 — O que este roteiro pede de arte e de código

Lista para `docs/ASSETS.md` (arte) e para o `GDD.md` §0 (requisitos novos a acompanhar). Nada disto entra em produção
antes de o idealizador aprovar o roteiro.

### 7.1 Arte (ADR-0002: ficha G1 → G2 → G3 antes de qualquer malha)

| Peça | Tipo | Observação |
|---|---|---|
| Abafador, feixe-de-espinho, lajeiro | 3 criaturas, fichas G1 novas | rig quadrúpede compartilhado entre lajeiro e Enredado; abafador sem esqueleto (pulo por animação de malha) |
| O Enredado | criatura + VFX (anel e três fios) | violeta + turquesa, as cores reservadas; anel com vão, coerente com a ficha `simbolo_limiar` |
| Bosque: trilha, espinhal, lajedo, coração | cenário (4 áreas, greybox primeiro) | oclusão por espinheiro; orçamento por área (CONTRATO §5) |
| Símbolo com dois vãos | variante do símbolo | por evento (`PecaPorEvento`) |
| Avatar aos 11 | proporção na base infantil | sem modelo por ano (GDD cap. 09); a manta vira capa curta |
| Nilo e Sera aos 11 | proporção | Nilo com algo do posto; Sera com a tabuinha e a ervanaria |
| Casa por origem (3 conjuntos × 2 idades) | props | cestos/horta; bancada; cabide/lanternas |
| Festa | props | bandeirolas, mesas, seis bancas, a corda da dança, sino |
| Itens | props + ícone | espada de freixo (comum, marcada com plaqueta de cobre), estaca de freixo, cravo de marcar, unguento, fita da colheita |
| VFX | magia | fio de luz (segunda manifestação), quebra de postura, dispersar |
| Animação | clips | contra (bloqueio no último instante), canalizar, empurrar tronco; criaturas: pulo, bote, raspar, investir |
| Áudio | sons e música | ambiente do bosque **com abafamento** (filtro por proximidade do bando); festa (música da vila); chifre de alarme; motivo do Limiar no E2-B16 (lacuna 09.10); vozes das criaturas (o Enredado sem som) |

### 7.2 Código (domínio em C# puro com teste; MonoBehaviour só orquestra)

| Item | Módulo | Teste mínimo |
|---|---|---|
| 8 missões `q09`–`q16` no `QuestCatalog` + `content/quests/*.json` + listas do `_schema.json` (âncoras novas) | Quest | `QuestDataParityTests`, `validate_quests.py` |
| Missão com objetivo de contagem (`tres_bancas`: 3 de 6) | Quest | `Festa_TresBancasDiferentes_Cumprem_AMesmaDuasVezesNao` |
| Segundo marco de idade `marco_idade_11` (8→11, liberado por `evento.q10_concluida`) | LifeSystem (`AgeAdvance`) | `Salto8Para11_*` (idempotente, interrompido, não reabre teto) |
| Entrega da espada de freixo pela idade (`rec.e2_b10.espada_de_freixo`) | Inventory | `EspadaDeFreixo_ComumOuMarcada_*` |
| Consumir item (unguento) e compra em Oren (HM-05) | Inventory | `Consumir_SemItem_Falha`, `Compra_DebitoECreditoNoMesmoCommit` |
| `CriaturaDef` (peso, aviso, janela, desafio) e `EncontroDef` (id, criaturas, âncora) em dados | Combat | `Criaturas_MesmosNumeros_NosQuatroDestinos` |
| Ledger de encontro idempotente; commit só na vitória | Combat + Save | `Encontro_*` (§5.2 E-02, E-03) |
| Regras de atributo (Força, Agilidade, Vigor, Intelecto, Percepção, Vontade) em funções puras | Progression | `TodoAtributo_TemLeitor` + um teste por regra |
| Regras de afinidade (limiar → caminho aberto) e grafo "todo obstáculo tem caminho-base" | Progression | `TodoObstaculo_TemCaminhoBase`, `LimiaresDeAfinidade_Alcancaveis*` |
| Técnicas novas: contra, fio de luz (2ª manifestação), canalização | Combat | fases e custo como `SpellCast`; contra só com a descoberta |
| Assistências em `Configuracoes` (fora do save) | Combat + UI | `Assistencia_NaoMudaLedgerNemSave` (ADR-0004 inv. 3) |
| Diálogo condicionado a faixa de reputação (`Condicao.Confianca` existe e ninguém usa) | Dialogue | uma fala por NPC central muda com a faixa |
| Interrupção de rotina por evento da vila (`PrioridadeEvento`) | NPC | `Alarme_InterrompeOsDez_ERetoma` |
| Exceção de período da festa | LifeSystem | `Festa_TardeViraNoite_SoNasTresBancas` |
| HM-03 `VocacaoProvavel` (se aprovado) | LifeSystem | §5.1 R-E2-09 |
| HM-06a tarefa da casa por origem | Progression + NPC | mesma tarefa rende trilha diferente por origem |
| HM-01 Registro vivo (para o jogador **ver** afinidade e limiar) | UI | foto em 20:9 e 16:9 |
| Acontecimentos de destino (E2-B07) usando os ids já publicados | Destiny + LifeSystem | cada destino grava só o próprio |
| IAP `coe.episodio2` (fora do save) e porta da q09 | novo, fora do domínio | loja falsa do PC; `Episodio2_DireitoNaoMoraNoSave` |
| Âncoras novas no `AurenSceneBuilder` (regerar por script, `CenaEstavel`) | Editor | `Populate_TodoAncoraTemPercursoLivre_*` estendido |
| Cerca anti-relógio | Arquitetura | `DominioNaoLeRelogio` |
| `SaveData.SchemaVersion` | Save | **sem mudança prevista**: tudo é evento no histórico, item no inventário ou preferência fora do save. Se algum campo novo surgir, sobe a versão com migração e teste de save do ep. 1 (CLAUDE.md). |

### 7.3 Linhas novas para o `GDD.md` §0 (a acompanhar quando aprovado)

E2.q09–E2.q16 (oito missões) · E2.afinidades (seis com efeito) · E2.atributos (seis lidos) · E2.criaturas (três +
Enredado) · E2.assist (fecha S.ASSIST) · E2.reputacao (fecha 06.2 e parte de 13.3) · E2.origem (fecha 02.5 e S.B03) ·
E2.destino (fecha 02.6) · E2.alarme (fecha 06.5) · E2.salto11 · E2.fim (gancho sem promessa) · E2.exploits (E-01 a E-10).
