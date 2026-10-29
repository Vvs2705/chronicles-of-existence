# Ficha G1 — `enredado`

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md)). Encontro maior do episódio 2 ([`EPISODIO2_DESPERTAR_DOS_TALENTOS.md`](../../design/EPISODIO2_DESPERTAR_DOS_TALENTOS.md), abreviado `EP2`), que é **PROPOSTA inteira**. Nada aqui vale antes de o idealizador aprovar o roteiro **e** o Art Director dar a nota. Par desta ficha: [`lajeiro`](lajeiro.md) (o corpo é de lajeiro) e [`simbolo_limiar`](simbolo_limiar.md) (o anel é o mesmo desenho).
> Fato com fonte é cânone ou está no `EP2` (marcado `EP2 §…`). **PROPOSTA** = ideia desta ficha.
> Criatura não fala nem tem destino próprio: C6, C7 e C8 são adaptados, e o pontuador decide.
> Esta ficha cobre a criatura **e** o VFX do anel e dos três fios (`EP2` §7.1 pede os dois juntos). Se o Art Director preferir, o VFX ganha ficha própria depois; a forma não muda.

## 1. Identificação

- **id:** `enredado` (no código, `criatura.enredado`, `EP2` §3.4). Passa no regex do `PIPELINE.md` §6 e não contém chave do `HumanoidMapping` (conferido: nem `die`, nem `run`).
- **categoria:** criatura + Vfx. Para orçamento: corpo como Npc, anel como Prop médio, fios como Vfx (§6).
- **onde aparece:** episódio 2. E2-B15 (`q16_o_enredado`, objetivos `ver_o_enredado` → `cortar_os_tres_nos` → `seguir_os_fios`, encontro `enc.coracao_enredado`). Âncora: `bosque_coracao`, "uma depressão redonda com o chão sem som; fim da trilha: dali não se vai a lugar nenhum" (`EP2` §1.2). Gancho no E2-B16: os fios recolhem para a clareira e o símbolo ganha um segundo vão.
- **cânone de partida:**
  - `EP2` §3.4: a anomalia (Trama) prendeu o lajeiro mais velho do bosque; os fios fazem o desenho do símbolo do Limiar, com um vão. Três formas: massa baixa de lajes, 2 m de altura por 3,5 m; acima do dorso, um anel de fios de luz de 2,4 m, aberto por um vão; três fios esticados do anel até o chão, como cordas de tenda, cada um preso num nó. Cor: lajes cinza-ardósia; fios Violeta #9777B8 trançados com Turquesa #86C8C9. Regra: os fios o puxam como marionete (investidas mais longas, varredura com a viseira); os abafadores em volta tiram o som (HUD "silêncio"); cada nó cortado solta um fio e ele fica mais lento; três nós, o anel se desfaz. `peso` 4. Os três nós e os caminhos de cada um estão na tabela do `EP2` §3.4 (frente: caminho-base, Marcial, Força; flanco: base, Artesanal, Arcana + Vontade; de cima: base, Exploratória, Natural, Social).
  - Luta de **libertar**, não de matar; fecha pelo caminho-base com todas as afinidades em 0 (`EP2` B15, aceite). Nós cortados não ficam gravados no meio da luta (`EP2` §5.2, E-03). "O Enredado sem som" (`EP2` §7.1, áudio).
  - O símbolo do Limiar: anel de três fios trançados, vão de 40° na posição de uma hora, Turquesa, sem Violeta "para o slice não declarar o símbolo uma anomalia" (ficha `simbolo_limiar`, C3 e §4).
  - Nilo, aos 8: "um círculo de luz e um silêncio enorme" (`dialogo.nilo.aos_oito*`). Ruptura: "algo fora de lugar no bosque" (`destino.ruptura.descricao`).
- **autor da ficha / data:** Concept Art Lead (agente), 2026-10-29
- **estado:** aguardando nota (autoavaliação na §5; **pendente de nota do Art Director separado, ADR-0010 §6**)

## 2. Critérios C1–C10

**C1 — Gancho.** O lajeiro mais velho do bosque, duas vezes maior que os outros, investe puxado por três fios de luz violeta que formam sobre ele o mesmo anel aberto do símbolo do Limiar, e ninguém ouve nada: a criança de 11 anos não veio matá-lo, veio cortar os três nós que o prendem. *(`EP2` §3.4 e B15)*

**C2 — Necessidade do mundo.** A anomalia que a Ruptura vê "fora de lugar no bosque" (`destino.ruptura.descricao`) e que o episódio 2 abre para todos (`EP2` §6.1, decisão 1, recomendada) precisa de um corpo que o jogo possa enfrentar sem explicar a Primeira Fratura. O Enredado é esse corpo: os fios desenham o símbolo que a criança viu antes de nascer, e o que eles prendem é um bicho comum do bosque, não um monstro. **PROPOSTA:** ele junta as duas metades da memória do Nilo. O "círculo de luz" é o anel; o "silêncio enorme" são os bandos de abafadores em volta (ficha `abafador`, C2). A criança acha, aos 11, o que o Nilo viu aos 5, e o jogo não diz se ele esteve ali.

**C3 — Silhueta em 3 formas.** *(medidas PROPOSTA, a partir do `EP2` §3.4)* Vista canônica: **de frente e um pouco de lado (3/4)**, a da câmera de terceira pessoa quando ele investe.
1. **Massa de lajes:** o corpo do lajeiro (ficha `lajeiro`, C3) com 2,00 m no alto do dorso × 3,50 m de comprimento × 1,70 m de largura. As cinco lajes do dorso são grossas (0,15 m), rachadas e com musgo de palmo nas juntas; a viseira chega a 0,20 m do chão. Em preto, o mesmo retângulo deitado do lajeiro, com o dobro do tamanho: lê "lajeiro, e grande".
2. **Anel:** de pé sobre o dorso, um anel de 2,40 m de diâmetro externo, de **três fios trançados** (trança de 0,18 m), com vão de 40° na posição de uma hora vista de frente, como o do símbolo (ficha `simbolo_limiar`, C3). Centro a 3,40 m do chão; base a 2,20 m (0,20 m acima do dorso, sem tocar); topo a 4,60 m. A caixa da figura vai a 4,60 m: é a coisa mais alta do episódio.
3. **Três fios:** cada fio da trança sai do anel e vai a um nó. **Da frente:** desce do anel até a viseira (nó a ~0,60 m do chão). **Do flanco:** desce em diagonal até o chão, 3 m à esquerda do corpo, esticado como corda de tenda (nó no chão, ~0,30 m de altura). **De cima:** o nó fica no topo do anel, onde o terceiro fio sai da trança e desce por trás até a cauda. Em preto: duas linhas tensas saindo do anel para baixo, uma para a frente e uma para o lado, e o anel com o vão.
- *T-pose:* não se aplica; o anel é peça separada, presa ao osso `peito` por uma restrição, não ao esqueleto. *Câmera:* com a criança de ~1,42 m (HIPÓTESE, `EP2` §4.2) a ~3,5 m atrás dela e o Enredado a ~6 m à frente, o topo do anel fica fora do quadro de perto; ele é enquadrado inteiro na entrada do coração (`ver_o_enredado`), a ~15 m. **Fraqueza declarada:** de lado, como no símbolo, o anel vira barra e o vão some. Como o corpo vira toda hora, o vão é lido na entrada, de frente, e na derrubada (C10).

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **A trança de três fios, que perde um fio a cada nó.** Cada um dos três fios da trança do anel é o fio de um nó.
- **O jogo lê:** `nos_cortados` (0 a 3), estado de cena (não vai para o save; `EP2` §5.2, E-03). Cada nó aceita o caminho-base e os de afinidade da tabela do `EP2` §3.4. Com 0 nós cortados, investida de 6 m (o dobro da do lajeiro, HIPÓTESE v0) e varredura; cada nó cortado tira 1/3 da velocidade de investida (HIPÓTESE v0; "fica mais lento", `EP2` §3.4).
- **O jogador vê:** o nó cortado passa de Violeta a Turquesa por um instante e o fio dele se solta da trança e recolhe pelo chão, na direção da trilha (para a clareira, `EP2` B15). O anel fica com **dois** fios, depois **um**: a trança afina na frente do jogador, sem barra de vida. Com o terceiro, o último fio se desfaz e o anel some.
- **Cor e valor:** cada fio é Violeta #9777B8 (anomalia) com uma alma fina de Turquesa #86C8C9 (Trama) torcida por dentro, as duas cores reservadas (GDD cap. 09; ART_BIBLE §3). Ao contrário de turquesa e dourado (ficha `simbolo_limiar`, C4), violeta e turquesa têm valor bem diferente (luminância relativa ~0,23 contra ~0,51): a trança lê em duas faixas também em escala de cinza. Mesmo assim, o contador é a **quantidade de fios**, não a cor.
- Nada é concedido no encontro; a recompensa é o marco da missão, uma vez (`rec.q16_o_enredado.marco_enredado_livre`).

**C5 — Regra exclusiva.** É a única luta do jogo sem vida para zerar: vence-se cortando três nós, cada um por pelo menos três caminhos, e o caminho-base fecha com tudo em 0 (`EP2` B15). O Enredado não se dispersa, como as outras criaturas: é solto. **PROPOSTA de geometria do nó de cima:** depois da terceira investida, ele baixa a cabeça 2 s; o anel, preso ao peito, tomba para a frente e o nó do topo desce até ~1,30 m do chão, à frente do focinho, ao alcance da criança (HIPÓTESE v0; o G2 confere a conta com o comprimento do pescoço). Com Exploratória ≥ 2, a laje alta da borda do coração (~3 m, pedido de cena) põe a mão da criança (~4,75 m, com o braço erguido) no topo do anel sem esperar.

**C6 — Voz.** *(adaptação: o pontuador decide se conta.)* "O Enredado sem som" (`EP2` §7.1) vira a voz:
1. Na luta, nada. Os pilões batem no chão e não se ouve; só o HUD diz "silêncio" (os abafadores em volta, ficha `abafador`, C4). É a única luta muda do jogo.
2. A cada nó cortado, o som do lugar volta um terço: um passa-baixa que abre em três degraus (PROPOSTA de áudio, ligada a `nos_cortados`). O jogador **ouve** o progresso, e o HUD o escreve.
3. Quando o anel se desfaz, o primeiro som que ele faz é o ronco grave de pedra moendo do lajeiro comum (ficha `lajeiro`, C6). A voz dele é a de um bicho do bosque.

**C7 — Contradição visível.** *(PROPOSTA)* O maior bicho do bosque ataca a criança e o tempo todo tenta ir embora. Entre as investidas, ele vira o corpo para o fundo do coração e dá dois passos; os fios esticam, o anel inclina, e ele é puxado de volta para o centro. Observável em toda pausa da luta, sem fala. No fim, solto, ele faz o que tentava: vai embora devagar para o fundo (`EP2` B15). É a contradição do lajeiro comum, que só quer voltar a raspar (ficha `lajeiro`, C7), sem a liberdade de fazê-lo.

**C8 — Relação com o avatar por destino e origem.** *(PROPOSTA; ADR-0004, inv. 4: a Ruptura vê mais cedo, não vê mais; nenhum número muda.)*
- **ruptura:** a criança viu, aos 8, uma luz violeta na clareira que ninguém mais viu (E2-B07). Aos 11, no `bosque_lajedo`, o topo do anel aparece violeta por cima das árvores do coração **para todos** (é o mesmo objeto); só na Ruptura o amuleto rachado (`item.amuleto_rachado`) racha mais um fio quando o terceiro nó solta (peça por evento, sem efeito de jogo).
- **serena, normal, dificil:** iguais. O anel é visto pela primeira vez na entrada do coração.
- **reputação, não destino:** quem vai junto e quem o Social chama (Nilo, Sera ou Tovin, `EP2` §3.6) muda a cena e a fala, não o encontro.
- Sincero: dois ramos (Ruptura e os outros), como o símbolo. Nota 1.

**C9 — Mudança no tempo.** O salto do episódio é dos 8 para os 11.
- **Antes (8):** ele não aparece. Existe por fora: o silêncio na memória do Nilo (para todos) e a luz violeta da janela (só na Ruptura, E2-B07). No símbolo da clareira, os nós voltam a ficar turquesa na q10: há algo a fazer nele de novo (`EP2` §0).
- **Depois (11):** a luta (q16). E o que fica: o coração do bosque passa a ter som; os três fios recolhidos abrem o **segundo vão** do símbolo (`PecaPorEvento`, `evento.q16_concluida`, `EP2` B16). PROPOSTA: uma laje rachada do dorso dele fica no chão do coração, ligada ao mesmo evento, para quem voltar.
- **Variante de malha:** não do corpo. O anel não existe depois do evento.

**C10 — Momento de cartaz.** *(PROPOSTA)* A criança de 11 desce para o coração do bosque. O som some de uma vez; no alto da tela, "silêncio". No fundo da depressão, um lajeiro do tamanho de uma carroça investe contra nada, puxado por três fios violeta que saem de um anel aberto acima dele, o anel do símbolo. Ele bate num tronco e fica preso. A criança corre até a viseira e corta o primeiro nó: o fio solta da trança, risca o chão em direção à trilha, e o anel fica mais fino. Por um instante, ouve-se o vento.

## 3. Amarração

- **Destino, Grau ou Trama:** é a Trama tocando o mundo da criança pela primeira vez como algo que se enfrenta. A Ruptura reconhece a cor antes (C8). O segundo vão do símbolo (B16) é o gancho do episódio 3, sem promessa.
- **Decisão de jogo que ele cria:** qual caminho em cada nó. O jogador olha o que treinou nos três anos (Marcial, Força, Artesanal, Arcana, Vontade, Exploratória, Natural, Social) e escolhe como soltar cada fio; quem não treinou nada espera a janela. É o lugar em que a infância vivida vira jeito de resolver.
- **Pilar:** superar a própria existência (a criança desfaz, com o que aprendeu, um pedaço do que a Trama prendeu).

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| fera corrompida com tentáculos ou vermes saindo do corpo | o corpo está inteiro, de pedra e musgo; tudo o que o prende está **fora** dele: três fios esticados até o chão |
| marionete com cruzeta de madeira e fios descendo do céu | um anel de pé sobre o dorso e fios que **descem** do anel para o chão e para a viseira, como estais de tenda |
| chefe de barra de vida, com fase de fúria e ponto fraco brilhante | sem barra: a trança perde um fio por nó; o nó se corta por mais de um caminho |
| criatura grande que se escala para golpear o ponto fraco no alto | não se escala: o nó de cima desce quando ele baixa a cabeça, ou se alcança de uma laje alta |
| círculo mágico de runas, girando, com escrita | o anel do símbolo: três fios trançados, um vão às 13 h, sem escrita e sem giro |
| monstro que morre e explode em luz | solto, ele ronca, vira e vai embora a passo |
| aura escura, preto e vermelho | Violeta com alma Turquesa nos fios; o corpo continua cinza-ardósia e verde |

## 5. Avaliação

**Autoavaliação do autor (não vale como nota; ADR-0002 e ADR-0010 §6):**

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 2 | 2 | 2 | 1 | 2 | 1 | 2 | 2 | **18/20** |

- C6 = 1 (silêncio e som, não fala; o pontuador pode dar 2 se aceitar a adaptação); C8 = 1 (dois ramos).
- **Pendente de nota do Art Director separado (ADR-0010).** Corte: ≥ 14/20 e nenhum zero em C2–C5 (HIPÓTESE do ADR).
- **Pontuador / data:**
- **Veredito:**
- **Observações do pontuador:**

## 6. Encaminhamento

**Para o G2 (concept), só depois de aprovado:**
- vistas: frente (o vão às 13 h), 3/4 (a canônica), lado, de cima; estados: 3, 2, 1 e 0 fios; cabeça baixa com o anel tombado (o nó de cima a ~1,30 m); ao lado, a criança de ~1,42 m e um lajeiro comum; linha de chão.
- silhueta adaptada: em preto a 30%, embaralhado com o lajeiro, o `simbolo_limiar` e o Aethron; tem de ler "o símbolo **sobre** um lajeiro", e o anel tem de ser identificável como o do símbolo (mesmo vão, mesma posição) por quem leu as fichas.
- paleta: corpo como o do lajeiro (ardósia escurecida, Verde #648B67 no musgo, mais musgo e rachaduras); fios Violeta #9777B8 com alma Turquesa #86C8C9, cor chapada, sem contorno, lendo como luz (a mesma regra do símbolo); nós Violeta, que passam a Turquesa ao serem cortados. **Fora:** Dourado (é do eco do símbolo e do Limiar).
- orçamento (`PIPELINE.md` §4, HIPÓTESE v0):
  - **corpo:** categoria mais próxima **Npc**: até 8 000 tris no LOD0, LOD1 ≤ 50%, LOD2 ≤ 25%, 2 materiais, 1024 px, ≤ 55 ossos.
  - **anel:** **Prop médio** (o mesmo que a ficha `simbolo_limiar` usou): 2 000 tris, 1 material, 512 px; três tubos de 6 lados no LOD0. Peça filha, um objeto por fio, para cada um poder sumir sozinho (C4).
  - **fios até os nós:** **Vfx**, sem malha (`LineRenderer`) ou ≤ 300 tris; ≤ 3 camadas transparentes no mesmo pixel (`PIPELINE.md` §4.1), e de preferência opacos e sem luz, como o anel.
  - **quadro do encontro (pior caso):** avatar 15 000 + Enredado 8 000 + anel 2 000 + 3 companheiros 24 000 + 8 abafadores 4 000 + cenário do coração (área carregada sozinha, `EP2` §1.2) ≈ 53 000 + cenário, abaixo de 150 000.
- **rig:** o esqueleto genérico quadrúpede do lajeiro (ficha `lajeiro`, §6), com os mesmos 25 nomes de osso e os comprimentos da malha velha; os clips do lajeiro são reaproveitados porque só têm curva de rotação (a raiz o código move). *ponytail:* se a reutilização entre comprimentos diferentes deformar no G3, o Enredado ganha clips próprios a partir dos do lajeiro. Clips a mais: `puxado` (laço de pausa: dois passos para o fundo e o tranco de volta), `investida_longa`, `varredura` (a viseira de lado, ~0,6 s), `baixar_cabeca` (2 s), `solto` (estremece, ronca) e `ir_embora` (o do lajeiro, mais lento). Sem clip de morte. Pasta proposta: `Art/Criatura/enredado/` (corpo) e `Art/Vfx/enredado_anel/`.
- código que a ficha pede (dependência, não feito aqui): `nos_cortados` como estado de cena; o fio de cada nó some e recolhe; passa-baixa em três degraus; anel preso ao `peito` que tomba no `baixar_cabeca`; a laje alta do coração (cena); a laje rachada por evento (C9).
- testes a registrar: silhueta adaptada, descrição ("um bicho de pedra enorme preso por três fios de luz que fazem sobre ele um anel aberto; não se mata, cortam-se os nós": se a resposta nomear outro jogo, reprova), troco (trocar por um lajeiro grande sem anel: o símbolo, a memória do Nilo e o segundo vão somem; não passa).

**Para o G3:** blocos `### enredado` e `### enredado_anel` em `PROVENIENCIA.md`, como no modelo.

## 7. Teste de originalidade honesto

Lista de revisão; **não** vai para o briefing do gerador.

| Pode lembrar | Por quê | O que afasta |
|---|---|---|
| Nobres enfurecidos de *Pokémon Legends: Arceus* | criatura grande enlouquecida por uma fenda misteriosa no céu, que se acalma em vez de se vencer | o mais perto **no conceito**. Lá se acalma atirando bálsamos até zerar uma barra; aqui não há barra nem item: cortam-se três nós, cada um por caminhos que dependem do que a criança treinou, e a causa (o anel) está desenhada sobre o bicho, com o símbolo do próprio jogo |
| *Tatarigami* de *Princesa Mononoke* (Nago, Okkoto) | fera da floresta tomada por uma força que a cobre e a move | lá a corrupção nasce do corpo, escura e viscosa, e a fera morre; aqui o corpo está intacto, os fios são de luz e ficam de fora, esticados até o chão, e o bicho vai embora vivo |
| *Shadow of the Colossus* | colosso de pedra e musgo, ponto fraco no alto | não se escala; o nó de cima desce até a criança; é um bicho do bosque, não um colosso |
| *Kena: Bridge of Spirits*, *Ōkami* | purificar a corrupção em vez de matar | não há purificação nem luz que limpa; há fios que se cortam e que voltam para a clareira, para o símbolo |
| Chefes-marionete (genérico) | fios que movem o inimigo | os fios não descem de um mestre invisível; saem do anel do símbolo e descem ao chão como estais |
| Golens e colossos de pedra em geral | massa grande de pedra | o corpo é um lajeiro, com a mesma forma do comum, só que velho e grande |

**Maior risco:** o par conceitual "criatura grande enlouquecida por uma anomalia, que se liberta em vez de matar" (nobres de *Legends: Arceus*; tatarigami). A defesa é a mecânica (três nós, cada um por vários caminhos de afinidade, sem barra e sem item), a forma (a causa é o símbolo do jogo, por fora do corpo) e o fim (ele vai embora; os fios abrem o segundo vão).

## 8. Ligações com outras fichas e com o elenco

- **`simbolo_limiar`:** o anel do Enredado é o desenho do símbolo, com Violeta. A ficha do símbolo tirou o Violeta do símbolo "para o slice não declarar o símbolo uma anomalia"; esta ficha **não** muda isso: o símbolo da clareira continua Turquesa, e só o anel sobre o Enredado leva Violeta. A diferença de escala (2,40 contra 1,20 m) e os dois fios esticados até o chão (contra um fio solto e ondulado) separam os dois na mesma folha. O segundo vão do B16 é do símbolo (variante por evento), não desta ficha.
- **`lajeiro`:** mesmo esqueleto e mesma forma de corpo, com o dobro do tamanho (§6).
- **`abafador`:** os bandos fazem o silêncio do coração e não atacam (ficha `abafador`, C9).
- **`nilo`, `tovin`, `sera`:** companheiros por reputação (`EP2` §3.6); o Social chama um deles para segurar a atenção dele 4 s. Nenhuma ficha deles muda.
- **`avatar`:** o amuleto rachado só ganha uma rachadura na Ruptura (C8); o pedido é de peça por evento, sem efeito de jogo.
- **Zonas de silhueta:** anel vertical vazado **acima de 2,2 m**, sobre um corpo deitado. Nenhum NPC passa de 2,17 m (a torre do Oren); o anel do símbolo fica entre 0 e 1,5 m, no chão.
