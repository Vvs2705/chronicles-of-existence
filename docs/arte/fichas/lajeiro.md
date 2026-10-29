# Ficha G1 — `lajeiro`

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md)). Criatura do episódio 2 ([`EPISODIO2_DESPERTAR_DOS_TALENTOS.md`](../../design/EPISODIO2_DESPERTAR_DOS_TALENTOS.md), abreviado `EP2`), que é **PROPOSTA inteira**. Nada aqui vale antes de o idealizador aprovar o roteiro **e** o Art Director dar a nota. Par desta ficha: [`enredado`](enredado.md) (o lajeiro mais velho, preso pela anomalia).
> Fato com fonte é cânone ou está no `EP2` (marcado `EP2 §…`). **PROPOSTA** = ideia desta ficha.
> Criatura não fala nem tem destino próprio: C6, C7 e C8 são adaptados, e o pontuador decide.

## 1. Identificação

- **id:** `lajeiro` (no código, `criatura.lajeiro`, `EP2` §3.4). Passa no regex do `PIPELINE.md` §6 e não contém chave do `HumanoidMapping`.
- **categoria:** criatura. Para orçamento, Npc (§6).
- **onde aparece:** episódio 2. E2-B14 (`q15_a_anomalia_no_bosque`, objetivo `o_lajedo`, encontro `enc.lajedo_lajeiro`, à noite). Âncora: `bosque_lajedo`, "clareira de chão de pedra nua, sem folha: é onde os lajeiros raspam e largam laje" (`EP2` §1.2). O esqueleto e a malha base servem também ao Enredado (`EP2` §7.1).
- **cânone de partida:**
  - `EP2` §3.4: quadrúpede baixo e largo, 1,0 m de altura por 1,8 m; dorso em escada de lajes chatas, da cabeça à cauda, a da frente servindo de viseira; patas curtas e largas como pilões; cinza-ardósia de pedra molhada, musgo verde nas juntas. Raspa a pata 1 s (som e poeira, o aviso) e investe em linha reta; de frente, o bloqueio não segura (empurra); de lado, as juntas das lajes abrem e é ali que se bate; ataque forte com Força suficiente derruba uma laje e o atordoa 1,5 s; contra um tronco, fica preso 2 s; `peso` 3 (ataque forte quebra a postura com Força ≥ 2). Natural ≥ 2 o dispersa sem luta se ele ainda não investiu (`EP2` §3.3).
  - A praça de Auren é de pedra (`AurenSceneBuilder.cs`, objeto `praca`, material `COE_Auren_Pedra`).
  - Pendência aberta: "Lajes da praça vindas do bosque, pela mão de Oren: conferir com a ficha `oren` antes do G1 das criaturas" (`EP2` §6.2, item 7). Resolvida aqui (C2).
- **autor da ficha / data:** Concept Art Lead (agente), 2026-10-29
- **estado:** aguardando nota (autoavaliação na §5; **pendente de nota do Art Director separado, ADR-0010 §6**)

## 2. Critérios C1–C10

**C1 — Gancho.** Um bicho de pedra da altura de uma mesa, com o dorso em escada de lajes, que passa a noite raspando o chão do bosque até deixá-lo em pedra nua e investe em linha reta contra quem fica na frente: de frente, não há bloqueio que segure; de lado, as lajes abrem. *(`EP2` §3.4)*

**C2 — Necessidade do mundo.** O Bosque dos Sussurros tem clareiras de pedra nua sem pedreira nenhuma (`EP2` §1.2): o lajeiro raspa o chão até a pedra e solta lajes do próprio dorso, como bicho que troca casca. **Conferência com a ficha `oren` (pendência 7):** a ligação "pela mão de Oren" **não** fecha. Pela ficha dele, tudo o que chega a Auren entra pelo portão sul, do lado oposto ao bosque, e "nada entra em Auren sem passar" por ele (`oren`, C2 e C6); e a Arbitragem 2 (item 8) diz que Oren não rastreia. **PROPOSTA:** as lajes da praça são laje de lajeiro, assentadas antes de qualquer adulto vivo, do tempo em que o vão do espinheiro era mais largo; ninguém de Auren as traz hoje, e a ficha não liga isso a nenhum NPC. A praça vira a única prova em Auren de que o bosque tem esses bichos, e o lajedo é de onde ela veio. Se o idealizador recusar, o lajeiro perde só a frase da praça; o lajedo continua precisando dele.

**C3 — Silhueta em 3 formas.** *(medidas PROPOSTA, a partir do `EP2` §3.4)* Vista canônica: **de lado**, porque a regra é o flanco.
1. **Mesa baixa:** corpo de 1,80 m de comprimento × 1,00 m de altura no alto do dorso × 0,90 m de largura, barriga reta a 0,35 m do chão. Em preto, um retângulo deitado sobre quatro pés, mais largo que alto: nenhum outro do elenco tem essa proporção.
2. **Escada de lajes:** cinco lajes chatas sobrepostas como telhas, descendo da nuca para a cauda, cada uma com 0,40–0,50 m de comprimento e 0,08 m de espessura, a de cima por cima da de trás, com o degrau de 0,06 m entre elas lendo como serrilha de cinco dentes no perfil. A primeira, a **viseira**, avança 0,25 m além do focinho, inclinada 30° para baixo, com a borda a 0,15 m do chão: de lado, uma pá; de frente, uma parede que esconde a cabeça.
3. **Pilões:** quatro patas retas, Ø 0,25 m, sem joelho aparente, pé chato. O vão entre a barriga e o chão (0,35 m) é a única luz na silhueta, e ele fecha quando o bicho se abaixa para raspar.
- *T-pose:* não se aplica (quadrúpede); a pose de repouso é de pé, patas retas. *Câmera:* com a criança de ~1,42 m (HIPÓTESE, `EP2` §4.2) a ~3,5 m, o dorso fica na altura do peito dela. De frente, a viseira esconde a cabeça e o bicho lê como um bloco: é a leitura de "não bata aqui". **Fraqueza declarada:** de cima, as lajes viram mancha cinza; a câmera nunca o vê de cima.

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **As lajes do dorso**, a viseira e as quatro de trás.
- **O jogo lê:** o ângulo entre a frente do lajeiro e o golpe. Golpe a menos de 60° do focinho bate na viseira: sem dano, a criança é empurrada (o bloqueio dela também não segura, `EP2` §3.4). Golpe fora disso bate numa junta, e só dá dano quando a junta está aberta: ela abre durante o raspar (1 s) e depois de cada investida, quando o corpo vira. Ataque forte com Força ≥ 2 numa junta aberta derruba a laje daquela junta: atordoa 1,5 s.
- **O jogador vê:** a junta abre como um degrau que se levanta, com musgo por dentro. A laje derrubada cai no chão do lajedo e fica ali, como peça de cena, até a área descarregar; o dorso fica com um dente a menos na serrilha. Com duas lajes no chão, ele se dispersa (HIPÓTESE v0; a vida dele é o encontro, não um número na tela).
- Nada é concedido: a laje no chão não é item, não se pega e não vai para o save (`EP2` §3.4, "sem item"; §5.2, E-03).

**C5 — Regra exclusiva.** É a única criatura que não se bloqueia nem se ataca de frente: a luta é de flanco. Raspa a pata três vezes em 1 s (som, poeira, junta aberta) e investe em linha reta. Se a criança sai da linha, ele segue até parar; se ela se põe na frente de um tronco, ele bate e fica preso 2 s. Natural ≥ 2: chegar devagar (parado 3 s) enquanto ele ainda raspa o dispersa sem luta (`EP2` §3.3). **PROPOSTA:** ele não persegue. Sem ninguém na linha, volta a raspar (C7).

**C6 — Voz.** *(adaptação: três sons escritos; o pontuador decide se conta.)* PROPOSTA:
1. Parado: um ronco grave de pedra moendo, quase abaixo do que se ouve, a cada ~3 s. É o mesmo som que o Enredado faz no fim, quando é solto (ficha `enredado`, C6).
2. Raspar (aviso): três raspadas de pedra em pedra, "xrrk, xrrk, xrrk", no 1 s do aviso. O ritmo é o aviso para quem não olha.
3. Investir: os pilões batendo no lajedo, "tum-tum-tum", e o estalo seco de laje contra tronco quando ele fica preso.

**C7 — Contradição visível.** *(PROPOSTA)* O bicho mais forte do bosque passa a luta inteira querendo voltar ao trabalho. Depois de cada investida, se a criança não está na linha dele, ele vira e volta a raspar o chão, como se ela não existisse. Observável em toda luta: ele só ataca quem fica na frente. É a mesma coisa que o Enredado não pode fazer (a anomalia não o deixa voltar ao fundo, ficha `enredado`, C7).

**C8 — Relação com o avatar por destino e origem.** *(PROPOSTA; ADR-0004: nenhum número muda por destino.)*
- **serena, normal, dificil, ruptura:** igual em número, forma e regra.
- **origem `artesaos`:** a criança que cresceu na bancada de Borin reconhece na laje caída a pedra da praça (fala na volta, PROPOSTA); as outras origens: igual.
- **reputação:** com Tovin cordial (q09), a criança vai na frente no lajedo; neutro, Tovin vai na frente (`EP2` B14). Mesmo encontro.
- Sincero: não ramifica pelos destinos. Nota 1.

**C9 — Mudança no tempo.** O salto do episódio é dos 8 para os 11.
- **Antes (8):** ele não aparece. Na ronda da q09, até o vão, Tovin mostra uma laje solta na beira da trilha e diz que é rastro de lajeiro e que rastro de lajeiro não se segue (PROPOSTA de fala). A criança já pisa as lajes dele na praça todo dia.
- **Depois (11):** o encontro do lajedo (q15), à noite. A criança de ~1,42 m vê o dorso na altura do próprio peito e a viseira na altura dos joelhos. E, no coração, encontra o mais velho de todos, preso (ficha `enredado`).
- **Variante de malha:** não para o lajeiro; o Enredado é malha própria sobre o mesmo esqueleto (§6).

**C10 — Momento de cartaz.** *(PROPOSTA)* Lajedo ao luar, pedra nua e nenhuma folha. Um bloco de pedra baixo raspa o chão três vezes; sobe poeira. Ele investe em linha. A criança sai para o lado no último instante; ele passa, bate num tronco e fica preso. Ela corre para o flanco, onde uma junta entre as lajes se abriu, e bate com força. Uma laje do dorso cai no chão com um estalo seco. Quando ele se solta, vira e volta a raspar.

## 3. Amarração

- **Destino, Grau ou Trama:** nenhum o altera. A Trama só toca o lajeiro mais velho, o Enredado.
- **Decisão de jogo que ele cria:** onde ficar. Na frente de um tronco (esperar que ele se prenda), no flanco (bater na junta aberta, com Força para derrubar laje) ou parado (Natural ≥ 2, antes de ele investir). Quem tem pressa e bate de frente não ganha nada.
- **Pilar:** viver e crescer (paciência e lugar, não força bruta).

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| gigante de pedra em pé, de braços enormes e punhos | quadrúpede da altura de uma mesa, sem braços; a força é a investida em linha |
| rocha redonda com olhos e braços | retângulo deitado com serrilha de cinco lajes no dorso |
| réptil de placas verticais pontudas nas costas | lajes chatas deitadas, sobrepostas como telhas, da nuca à cauda |
| tartaruga de casco em cúpula | dorso em degraus, sem cúpula; patas retas, sem casco que recolhe |
| bicho com escudo-rosto em pé, que se protege de frente e ataca de frente | a viseira é uma pá inclinada que raspa o chão; ele não se protege, investe, e só o flanco abre |
| golem de cristal, lava ou runa brilhante | ardósia molhada e musgo; nenhum brilho, nenhuma runa |

## 5. Avaliação

**Autoavaliação do autor (não vale como nota; ADR-0002 e ADR-0010 §6):**

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 2 | 2 | 2 | 1 | 2 | 1 | 1 | 2 | **17/20** |

- C2 = 2 só se a PROPOSTA da praça for aceita ou se o lajedo bastar; sem a praça, o pontuador pode ler o lajedo como necessidade de cenário e dar 1. C9 = 1: a batida dos 8 é rastro e fala, não o bicho.
- **Pendente de nota do Art Director separado (ADR-0010).** Corte: ≥ 14/20 e nenhum zero em C2–C5 (HIPÓTESE do ADR).
- **Pontuador / data:**
- **Veredito:**
- **Observações do pontuador:**

## 6. Encaminhamento

**Para o G2 (concept), só depois de aprovado:**
- vistas: lado (canônica), frente (a viseira como parede), 3/4 de trás (as juntas), de cima; estados: parado, raspando (abaixado, juntas abertas), sem uma laje; ao lado, a criança de ~1,42 m e uma laje da praça de Auren para comparar; linha de chão.
- silhueta adaptada: lado e frente em preto a 30%, embaralhados com o feixe, o abafador, o Enredado e o Aethron (a soleira dele é uma laje de 0,90 × 0,40 × 0,15 m, ver §8); ≥ 4 de 5 acertos.
- paleta: cinza-ardósia de pedra molhada (a mesma família do `COE_Auren_Pedra`, #AEAA9E, escurecida e esfriada, para casar com a praça), Verde #648B67 nas juntas. **Fora:** Turquesa, Violeta (só no Enredado), Dourado (é do Limiar e do Aethron) e Terracota.
- orçamento (`PIPELINE.md` §4, HIPÓTESE v0): categoria mais próxima **Npc** (corpo com rig e animação): até 8 000 tris no LOD0 (meta desta ficha: ~5 000, porque o bicho é de blocos), LOD1 ≤ 50%, LOD2 ≤ 25%, 2 materiais (pedra e musgo), 1024 px, ≤ 55 ossos. Um lajeiro por encontro.
- **rig (compartilhado com o Enredado, `EP2` §7.1):** esqueleto genérico quadrúpede (`Generic`). Ossos: `raiz`, `quadril`, `coluna`, `peito`, `pescoco`, `cabeca`, `viseira`; quatro patas de três ossos (`pata_de_1..3`, `pata_dd_1..3`, `pata_te_1..3`, `pata_td_1..3`); `cauda_1`, `cauda_2`; `laje_1..laje_4` (as de trás, cada uma num osso, para abrir a junta e para ser escondida quando cai) = 25 ossos. Clips: `respirar` (laço, 3 s), `raspar` (1 s, juntas abertas), `investir` (laço; o código move), `frear` (0,4 s), `preso` (2 s), `atordoado` (1,5 s), `virar` (0,5 s), `golpeado` (0,2 s), `ir_embora` (andar para o fundo, laço). Sem clip de morte. Nomes sem chave do `HumanoidMapping` (`respirar`, não `idle`). Pasta proposta: `Art/Criatura/lajeiro/`.
- código que a ficha pede (dependência, não feito aqui): leitura do ângulo frente × golpe; junta aberta por estado; laje derrubada como peça de cena (esconde o osso, solta um prop).
- testes a registrar: silhueta adaptada, descrição, troco (trocar pelo feixe no lajedo: a luta de flanco e o tronco somem; não passa sem reescrita).

**Para o G3:** bloco `### lajeiro` em `PROVENIENCIA.md`, como no modelo.

## 7. Teste de originalidade honesto

Lista de revisão; **não** vai para o briefing do gerador.

| Pode lembrar | Por quê | O que afasta |
|---|---|---|
| Golens de pedra (genérico de fantasia; Stone Talus de *Zelda*) | criatura de pedra que bate forte | não é humanoide nem tem braços; é quadrúpede baixo, e a regra é o flanco e o tronco |
| Bastiodon (Pokémon) | quadrúpede com escudo grande no rosto, forte de frente | o mais perto **na forma** da viseira. Lá o escudo é vertical, decorado e é o rosto; aqui é uma laje inclinada para o chão, uma pá que raspa, e a cabeça fica escondida atrás dela. E aqui ele não se protege: investe |
| Rhyhorn, Onix, Graveler (Pokémon) | bicho de pedra | Rhyhorn é o mais perto como "quadrúpede de pedra que investe"; tem chifre e corpo de rocha bruta. O lajeiro não tem chifre e o corpo é feito de lajes chatas em escada, que caem |
| Estegossauro, anquilossauro | dorso blindado em placas | placas verticais ou cravos; aqui as lajes são deitadas e sobrepostas como telhas |
| Pangolim, tatu | placas que se sobrepõem | escamas pequenas e muitas; aqui são cinco lajes grandes, e ele não enrola |
| Colossos de *Shadow of the Colossus* | criatura grande de pedra e musgo | o risco é do Enredado, não deste (ver lá) |
| Basarios (*Monster Hunter*) | criatura que se disfarça de rocha | o lajeiro não se disfarça; é o que deixa o chão em pedra |

**Maior risco:** a viseira lida como "escudo no rosto" (Bastiodon). A defesa é a inclinação (30° para baixo, borda a 0,15 m do chão), o uso (raspar o chão, não defender) e a regra (o flanco abre; a frente só empurra).

## 8. Ligações com outras fichas e com o elenco

- **`oren`:** pendência 7 do `EP2` resolvida contra a ligação: Oren não traz laje do bosque (C2). A ficha `oren` não muda.
- **`aethron`:** a forma 3 e o C4 dele são a **soleira**, uma laje sob os pés. Pela regra de desempate do `ELENCO.md` (fica quem usa a zona no C4), "laje sob os pés de uma figura de pé" é do Aethron. O lajeiro não tem laje sob os pés: as lajes são do dorso, deitadas em escada, irregulares, sem a cava dourada da soleira (Dourado é do Limiar, e nenhuma laje do lajeiro o leva). O G2 põe os dois na mesma folha.
- **`borin`:** a fala da origem `artesaos` (C8) é PROPOSTA para o redator; o Borin não ganha regra nova.
- **`enredado`:** mesmo esqueleto, malha própria e clips a mais (ficha `enredado`, §6).
- **Zonas de silhueta:** retângulo deitado de 1,8 × 1,0 m com serrilha no dorso e pá na frente. Nenhum NPC humano tem forma deitada; a torre de cestos do Oren é uma pilha **vertical**.
