# Mecânicas extraídas de *Hell Mode* — o que serve ao COE, em que forma, e o que fica de fora

> **Estado (2026-10-15): PROPOSTA.** Nada aqui está aprovado. Cada item leva **[PROPOSTA]**, **[HIPÓTESE A VALIDAR]**,
> **[FUTURO]** ou **[DESCARTADO]**. Nenhum vira código sem aprovação do idealizador (CLAUDE.md: "mudança de produto
> vira proposta em `docs/`").
>
> **Regra de originalidade aplicada a este documento.** *Hell Mode* é referência declarada do COE (GDD cap. 01;
> prompt-mestre §4; ADR-0002). O que se extrai daqui é **mecânica abstrata** — o porquê de uma regra funcionar. Não se
> copia nome, personagem, classe, interface, cena, número nem curva de progressão (prompt-mestre §4). Por isso a
> descrição da referência neste documento é deliberadamente sem números, e toda proposta vem com a forma **própria** do
> COE e as cinco perguntas do ADR-0002 (para sistema visível).
>
> **Fontes da referência:** wikis de fãs (hell-mode.fandom.com, isekai.fandom.com), Wikipedia EN/JA, sinopses da
> J-Novel Club, resenha da ANN; nenhuma página de capítulo. **Fontes do COE:** `docs/gdd/GDD_MESTRE_v1_2.md`,
> `docs/direcao/DOSSIE_CONTINUIDADE_v1_0.md`, `docs/direcao/PROMPT_MESTRE_AGENTE_v1_0.md`, ADR-0002/0004/0009/0010,
> `docs/design/SLICE_A_PRIMEIRA_EXISTENCIA.md`, `docs/qa/CONFORMIDADE_DO_SLICE.md`, `Scripts/Progression/Mastery.cs`.

---

## §0 — Resumo

| ID | Ideia no COE | Lição da referência | Estado | Onde mora | Custo |
|---|---|---|---|---|---|
| HM-01 | **Registro vivo**: uma tela com o que carrego, o que vivi e o que aprendi; progresso desenhado como o anel do Limiar | o jogador vê o próprio progresso e decide a partir dele | [PROPOSTA] slice | `UI`, lê `Inventory`, `LifeSystem`, `Progression` | M |
| HM-02 | **Segredos de ofício**: cada mestre de Auren tem um jeito de fazer que só se descobre fazendo diferente | experimentar paga; regras escondidas recompensam quem testa | [PROPOSTA] slice | `Progression`, `Dialogue`, `NPC` | M |
| HM-03 | **A leitura dos fios aos 8**: a vila lê o que a criança *viveu* e propõe uma vocação; dá para recusar | um rito de infância que rotula a criança e erra | [PROPOSTA] slice | `LifeSystem`, `Dialogue` | P–M |
| HM-04 | **Assistência mexe na execução, nunca no crescimento** (insumo para a decisão bloqueada das assistências) | o modo duro não fortalece o inimigo; só encarece crescer | [PROPOSTA] regra | `Combat`, `Progression` | P |
| HM-05 | **Tudo que se ganha tem uso**: erva cura, moeda compra acesso (nunca domínio) | todo recurso converte em outro, e o jogador calcula a rota | [PROPOSTA] slice | `Inventory`, `Dialogue` | P |
| HM-06 | **Prática dentro da rotina**: tarefas da casa viram atividade de domínio ligada à origem | o treino acontece escondido no trabalho diário | [PROPOSTA] (a) / [HIPÓTESE] (b) | `Progression`, `NPC` | P–M |
| HM-07 | Vínculo com criaturas do Bosque (aliado utilitário, uma por vez) | aliados com papéis fora do combate: batedor, mensageiro, cura, escudo | [FUTURO] | novo módulo | G |
| HM-08 | Híbridas por dois fios: técnica híbrida exige domínio nas duas trilhas-mãe | o novo nasce da combinação de dois anteriores, com custo somado | [FUTURO] | `Combat`, `Progression` | M |
| HM-09 | Domínio equilibrado como requisito de ascensão | a habilidade-mestra só sobe quando as sub-habilidades se igualam | [FUTURO] | `Ascension` | P |
| HM-10 | O que atravessa a ascensão: tabela explícita do que fica e do que reinicia | a promoção zera o nível e depois compensa | [FUTURO] | `Ascension`, `Save` | P |
| HM-11 | Gesto de uma vez por dia que recarrega com o descanso | habilidade "desperta" com recarga diária | [FUTURO] | `Combat`, `LifeSystem` | P |
| HM-12 | Renome por feito único (Guilda dos Exploradores) | a guilda sobe de patente por feito concluído | [FUTURO] | `Reputation` | P |
| HM-13 | O Bosque sobe de grau por marco da Trama | um evento do mundo sobe o perigo de todas as criaturas | [FUTURO] | `World` | M |
| HM-14 | Bestiário por observação (página do HM-01) | registro de criaturas e do que foi derrotado | [FUTURO] | `UI` | P |
| HM-15 | Grupo de papéis complementares nas expedições adultas | o grupo recruta o papel que falta | [FUTURO distante] | — | — |
| — | Modos de dificuldade, XP por abate, cartas, patentes em letra, gacha, progresso offline… | — | [DESCARTADO] (§4) | — | — |

P = pequeno (dias), M = médio (uma semana de bloco), G = grande (vários blocos + arte nos portões do ADR-0002).

**Ordem sugerida:** HM-01 → HM-05 → HM-04 (junto da decisão das assistências) → HM-02 → HM-06a → HM-03. HM-01 e HM-05
podem entrar **antes** do primeiro playtest (fecham lacunas que a conformidade já apontou: itens sem tela, moeda sem uso,
domínio invisível). HM-02, HM-03 e HM-06 dependem do que a rodada 1 disser sobre H1 (duração) e H5 (variação real).

---

## §1 — O que a referência faz bem (as lições, sem a forma)

1. **O progresso é legível e por isso otimizável.** O protagonista vê exatamente quanto falta, e o prazer da obra vem de
   ele planejar em cima disso. *No COE:* o domínio existe (`Mastery.cs`) e já anuncia "esta tarefa já te ensinou tudo por
   agora" — mas nenhuma tela mostra isso fora do painel de treino do B15.
2. **Testar é recompensado.** As maiores viradas da obra vêm de descobrir uma condição escondida ou um uso não óbvio de
   uma regra. *No COE:* a Q-05 ("chegar devagar acalma o bicho") já é isso, em ponto pequeno; falta virar padrão. (A
   Q-06, "o segredo do ferreiro", é outra coisa: um segredo de *confiança* que a criança guarda, não um que descobre.)
3. **Todo recurso tem para onde ir.** Dinheiro vira insumo, insumo vira capacidade, capacidade vira prática. *No COE:*
   moedas não têm onde ser gastas e itens não aparecem em tela (CONFORMIDADE).
4. **Aliados resolvem problemas fora da luta.** Os papéis mais lembrados são batedor, mensageiro, curandeiro e escudo,
   não só "quem bate". *No COE:* não há aliado nenhum (fora do escopo do slice).
5. **Reiniciar pode ser recompensa.** A promoção de classe da obra zera o nível e mesmo assim é desejada, porque o teto
   sobe. *No COE:* o Nível de Grau já reinicia na ascensão (GDD cap. 04; INCONSISTENCIAS A4) — a obra é evidência de que
   isso funciona se o jogador vê o que ganha.
6. **A dificuldade mora no custo de crescer, não no inimigo.** O modo mais duro da obra não fortalece os monstros; só
   encarece o crescimento do protagonista. *No COE:* destino não é dificuldade (ADR-0004), mas a lição serve às
   assistências de combate (HM-04).
7. **O mundo fecha exploits com regra de mundo.** A obra corta brechas do protagonista com regras que fazem sentido
   dentro da ficção, não com teto arbitrário. *No COE:* é a mesma filosofia do anti-farm do `Mastery` (retorno
   decrescente + teto por etapa + limiar de desafio), e vale como critério para toda proposta abaixo.

---

## §2 — Propostas para o vertical slice

### HM-01 — Registro vivo **[PROPOSTA]**

**Problema que resolve.** Três sistemas prontos e invisíveis: `Inventario` (itens sem tela — roadmap passo 6),
`LifeEventHistory` (marcos sem tela) e `Mastery` (domínio só aparece no painel de treino do B15).

**Forma no COE.** Uma tela, aberta por botão do HUD, com três páginas:
- **O que carrego** — itens do `Inventario`, cada um com uma linha de "para que serve" (HM-05 dá uso a eles). A manta de
  nascimento abre a página, no estado do tecido do destino (ficha `avatar` C4).
- **O que vivi** — marcos e eventos do `LifeEventHistory`, em ordem, agrupados por fase da vida. Só o que está no
  histórico daquele save.
- **O que aprendi** — atividades **descobertas**, agrupadas por trilha. Cada uma mostra o arco do anel do Limiar:
  o arco enche com o progresso da atividade na etapa e **para no vão** quando bate o teto. O texto troca de
  "aprendendo" para "já me ensinou tudo por agora". **Sem número na tela.**

**Diferença da referência.** Lá o jogador lê o número exato que falta. Aqui não: os números de `Mastery` são HIPÓTESE v0,
o público inclui crianças (ADR-0009) e o pilar é viver, não farmar. O arco mostra *direção e fim de etapa*, não conta.
Atividade não descoberta não aparece (nem como "???"), para não matar a HM-02.

**ADR-0002.**
1. *Necessidade do mundo:* Eryndor esquece a própria passagem (dossiê §I). Em Auren, quem ensina a criança a guardar o
   que viveu é Eira (educadora, história) — o registro nasce dela, num beat curto do B07.
2. *Regra própria:* o progresso é o anel que não se fecha. O vão é sempre o espaço da próxima etapa: domínio nunca
   aparece como círculo completo.
3. *Destino/Grau/Trama:* a primeira linha é a manta, no estado do destino; as páginas mudam só pelo que foi vivido.
   (FUTURO: na ascensão, o vão do anel muda de lugar.)
4. *Decisão que cria:* ver que uma atividade parou de render e escolher outra — o anti-farm passa a ser lido, não
   adivinhado.
5. *Pilar:* Viver e crescer.

**Escopo do protótipo.** uGUI montada em código (padrão do `Scripts/UI`), **só leitura** do `SaveData`: nenhum campo
novo, sem mudança de `SchemaVersion`. Um mapeamento puro `ArcoDoDominio(progressoDaAtividade, teto) → 0..1` em C#.
Localização por chave. Área segura e destro/canhoto como o resto do HUD.

**Testes.**
- EditMode: arco 0 sem prática; arco 1 no teto; atividade trivial usa o teto menor; nunca passa de 1.
- EditMode: atividade sem `Descoberta` não entra na lista; evento fora do histórico não entra.
- EditMode: save antigo (sem histórico de domínio) abre a tela vazia, sem exceção.
- Abuso: abrir e fechar a tela não grava nada (comparar o save byte a byte antes e depois).

**Riscos.** Espaço de tela no celular em paisagem; texto demais para criança. Mitigação: três páginas curtas, ícone por
item, e o arco carrega a maior parte da leitura.

---

### HM-02 — Segredos de ofício **[PROPOSTA]**

**Problema que resolve.** A descoberta (`MarcoDeDominio.Descoberta`) só acontece hoje como "primeira vez nesta etapa".
Não há recompensa por fazer *diferente*. E a reputação está inerte: só a Q-04 mexe nela e nenhuma fala a lê.

**Forma no COE.** Cada mestre de ofício de Auren tem **um** jeito de fazer que nunca diz em voz alta e que o jogo
reconhece quando o jogador faz:
- **Lysa** (ervas) — já tem a Q-05, "chegar devagar": é o primeiro caso e o modelo, já autorado.
- **Borin** (forja) — a Q-06 já põe a criança na forja e lhe confia um segredo de *confiança*
  (`confianca_de_borin`). O segredo de *ofício* seria outro, a autorar no mesmo lugar.
- **Tovin** (observação, treino do B15) — candidato: o bloqueio no último instante antes do golpe vira uma atividade
  própria, mais desafiadora. *Exemplo, a autorar.*

Ao descobrir:
1. grava `descoberta.<oficio>.<segredo>` no `LifeEventHistory` (idempotente; o id é a chave);
2. **abre uma atividade nova** (`AtividadeDef` com `Desafio` maior), que pelas regras do `Mastery` tem ledger e teto
   próprios. A recompensa por experimentar é *ter mais o que aprender*, não um número;
3. o mestre reage, se testemunhou (`NpcMemory`). Pode dar `confianca` com ele: é a primeira fala a **ler** a reputação.

**Diferença da referência.** Lá a descoberta rende poder. Aqui rende conhecimento, reconhecimento e uma prática nova,
coerente com "descobrir ≠ aprender ≠ dominar" (dossiê §D) e com "opcional nunca vira vantagem numérica" (B15).

**ADR-0002.**
1. *Necessidade do mundo:* em Auren, ofício se ensina pela mão. Borin "mede tudo duas vezes e desconfia de quem tem
   pressa" (o exemplo do próprio ADR-0002); um mestre que só fala o segredo para quem já o encontrou é o mesmo traço.
2. *Regra própria:* o mestre nunca diz o segredo; o jogo reconhece o gesto e o mestre só reage se viu.
3. *Destino/Origem:* a origem põe a criança mais perto de um mestre (artesãos → Borin; agricultores → Lysa;
   guardiões → Tovin), pelas afinidades favorecidas que o `DestinyCatalog` já declara. Ataca a lacuna "a origem quase
   não muda Mara e Daren" (CONFORMIDADE) sem dar vantagem: os três segredos existem para todos.
4. *Decisão que cria:* repetir o que rende ou tentar outro jeito.
5. *Pilar:* Viver e crescer.

**Escopo do protótipo.**
- **Lysa:** o caso que já existe (Q-05) passa a gravar `descoberta.*` e abrir uma atividade.
- **Borin:** conteúdo novo pequeno, na forja da Q-06.
- **Tovin:** só se a rodada 1 de playtest pedir mais conteúdo no B15. Sem campo novo no save: descoberta é evento
do histórico.

**Testes.**
- EditMode: descobrir duas vezes grava uma vez; a atividade nova só existe depois da descoberta.
- EditMode: a atividade nova obedece a teto e trivialidade como qualquer outra (não é brecha de farm).
- EditMode: o mestre só reage se o `NpcMemory` dele tem o evento.
- Abuso: recarregar o save no meio da descoberta não duplica a confiança (transação única por id de evento).

---

### HM-03 — A leitura dos fios aos 8 **[PROPOSTA]**

**A lição invertida.** Na referência, um rito de infância avalia um talento **inato**, decide o lugar social da criança e
erra sobre o protagonista. O COE não tem talento inato a avaliar — destino não dá poder e origem não dá vantagem
irreversível. Então o rito lê o que a criança **viveu**.

**Forma no COE.** No B14 ("três anos depois"), Maelis — administração regional, dona do livro onde o avatar assinou ou
riscou na Q-07 — faz o registro das crianças da vila que chegaram aos 8. Ela lê:
- as trilhas que mais renderam domínio dos 5 aos 7;
- os marcos;
- as opcionais (as mesmas marcas da manta).

Daí escreve no livro uma **vocação provável** e o mestre que receberia a criança. O jogador pode aceitar, ou **recusar
com o traço/risco** — o mesmo gesto da Q-07. Recusar é um marco que os NPCs lembram, e a vila não insiste. A leitura
nunca é sentença: é o vão do anel de novo, uma proposta da vila.

**ADR-0002.**
1. *Necessidade do mundo:* Auren divide aprendizes entre poucos mestres (Borin, Lysa, Tovin, Eira) e decide isso em
   comunidade — é o papel de Maelis no GDD cap. 06.
2. *Regra própria:* a vocação é função **pura do histórico**, nunca do destino. Duas crianças com a mesma infância e
   destinos diferentes recebem a mesma leitura — o ADR-0004 vira algo que o jogador *vê*.
3. *Destino/Grau/Trama:* o destino muda o tom de Maelis (o que ela diz da família), não o resultado.
4. *Decisão que cria:* aceitar o caminho que a vila vê ou recusar o rótulo.
5. *Pilar:* Escolher e transformar.

**Escopo do protótipo.**
- `VocacaoProvavel(SaveData) → trilhaId` em C# puro, com desempate fixo e documentado.
- Um nó de diálogo condicionado a `marco_idade_8`.
- Dois eventos: `evento.vocacao_aceita` e `evento.vocacao_recusada`.
- O mestre que "receberia" só muda fala no slice; aprendizado de fato é fase 8–11 (FUTURO).

**Testes.**
- EditMode: mesmo histórico com os 4 destinos → mesma vocação (invariante do ADR-0004).
- EditMode: histórico vazio → vocação "ainda aberta" (nunca nulo, nunca exceção).
- EditMode: empate → regra de desempate fixa.
- Abuso: aceitar e recarregar não grava os dois eventos; recusar não é revertível por diálogo.

**Risco.** Rotular criança é tema pesado. O tom "intimista, infância com peso" comporta, mas a fala precisa deixar
claro que é palpite da vila. Revisão de texto antes de ir para playtest com criança (ADR-0009).

---

### HM-04 — Assistência mexe na execução, nunca no crescimento **[PROPOSTA]** (regra)

**Insumo para** a decisão `BLOCKED_PRODUCT_DECISION` das assistências de combate (ADR-0004, invariante 3; CONFORMIDADE).

**Lição.** A obra separa com clareza *quão difícil é a luta* de *quão caro é crescer*: o modo duro só mexe no segundo.
O COE já separou destino de dificuldade; falta dizer o que a assistência pode tocar.

**Regra proposta.**
- A assistência pode mudar a **execução**: janela da esquiva, dano recebido, mira, tempo de reação do oponente, botão
  "Tocar" no lugar do traço.
- A assistência **nunca** muda `Mastery`, recompensa, marco ou fala.
- Uma criança com assistência aprende a mesma coisa que um adulto sem ela, pelo mesmo número de práticas.
- Ligar ou desligar a assistência a qualquer momento não deixa rastro no save de progresso.

**Teste de portão.** EditMode: a mesma sequência de práticas com assistência ligada e desligada produz o mesmo ledger de
`Mastery`, byte a byte.

---

### HM-05 — Tudo que se ganha tem uso **[PROPOSTA]**

**Problema que resolve.** Moedas sem onde gastar; ervas da Q-05 sem uso; cesto da Q-03 só como objeto (CONFORMIDADE).

**Forma mínima no COE.**
- **Ervas curam.** Usar uma erva fora do treino recupera Vida; Lysa diz como, no diálogo que já existe
  (`perguntar_ervas`).
- **Moeda compra acesso, nunca domínio.** Oren (comerciante) vende uma coisa que *abre uma prática*, não um número.
  Exemplo a autorar: linha e agulha que permitem cerzir a manta (uma atividade Artesanal nova). Pagar não pula o
  aprender: a atividade comprada tem teto e trivialidade como qualquer outra.

**Por que esta forma.** Respeita "dinheiro não substitui requisito/provação" (GDD cap. 07; prompt-mestre §8) e "opcional
nunca vira vantagem numérica" (B15), e ainda dá ao jogador o prazer da conversão que a referência tem.

**Risco a medir.** O dinheiro inicial muda por destino (40/25/8/18 no `DestinyCatalog`). Se o que se compra é
*variedade* de prática, o teto de trilha por etapa (`Mastery`) limita o efeito; mesmo assim, conferir em playtest que a
Vida Árdua não fica sem acesso ao que os outros destinos têm. Se ficar, Oren aceita troca por favor no lugar de moeda.

**Escopo do protótipo.** Consumir item no `Inventario` (ainda não existe a operação), uma opção de compra no diálogo de
Oren, e a compra como **transação única** (débito e crédito no mesmo commit do save).

**Testes.**
- EditMode: comprar sem moeda falha sem alterar nada.
- EditMode: comprar debita e credita no mesmo commit.
- EditMode: usar erva com 0 ervas falha.
- Abuso: interromper o save no meio da compra não deixa nem moeda perdida nem item duplicado (gravação atômica tmp +
  replace já existe; o teste prova).

---

### HM-06 — Prática dentro da rotina

**Lição.** Na obra, o protagonista treina escondido dentro do trabalho diário de criança, e o gargalo real é a recarga
de cada dia, não a vontade de repetir.

**(a) Tarefas da casa viram prática ligada à origem — [PROPOSTA].** As rotinas de Mara e Daren já existem
(`cuidar_da_casa`, `preparar_o_oficio`). Ajudar nelas vira `AtividadeDef`, com a trilha decidida pela origem:
- agricultores → Natural;
- artesãos → Artesanal;
- guardiões → Exploratória.

É conteúdo pequeno, e ataca a lacuna da conformidade (origem quase não muda Mara e Daren) com variação de *rotina*, não de
poder. Testes: a mesma tarefa rende trilhas diferentes por origem; o teto por etapa vale igual.

**(b) Cada período do dia comporta uma prática "de verdade" — [HIPÓTESE A VALIDAR].** Dar ao período (manhã, tarde,
noite; `TimeOfDayCycle`) o papel de recurso: praticar ocupa o período. Isso torna "o que treinar hoje" uma escolha. O
risco é parecer barra de energia de jogo mobile. **Não implementar antes** da rodada 1: só se H1 (duração) mostrar
farming de treino ou se os jogadores não sentirem o dia passar.

---

## §3 — Futuro (registrado para não se perder; nada disso entra no slice)

- **HM-07 — Vínculo com criaturas do Bosque [FUTURO].**
  - **Forma:** na vida adulta, uma criança que aprendeu a acalmar bichos (o padrão da Q-05) pode ser acompanhada por
    **uma** criatura do Bosque de cada vez. Cada criatura tem **uma** utilidade fora da luta: achar trilha, levar recado
    a um NPC, iluminar, guardar um objeto.
  - **Proibido nela:** carta, patente, fusão, bônus de atributo, invocar do nada, e lutar no lugar do jogador. O combate
    continua sendo ação em terceira pessoa.
  - **Risco:** é a mecânica central da referência, e por isso o maior risco de originalidade deste documento. Exige
    ficha G1 completa antes de qualquer arte, além de IA de acompanhante, animação e custo de desempenho no celular.
- **HM-08 — Híbridas por dois fios [FUTURO].** A técnica híbrida autorada (ex.: "Lâmina Incandescente", GDD cap. 05) só
  abre com Domínio nas duas trilhas-mãe na mesma etapa. No Registro, aparece como dois fios que se cruzam. Mantém
  "combinação livre proibida".
- **HM-09 — Domínio equilibrado [FUTURO].** O requisito "domínio" da ascensão (GDD cap. 04) pede **amplitude**: N
  atividades distintas em Domínio na trilha, e não uma só levada ao limite. É o anti-farm do `Mastery` aplicado à
  ascensão. Insumo para fechar o cap. 04.
- **HM-10 — O que atravessa a ascensão [FUTURO].**
  - **Tabela explícita:** atravessam Nível de Vida, marcos, descobertas, reputação e domínios; reinicia o Nível de Grau.
  - **Aviso:** o reinício é anunciado em 2 passos, como o salto 5→8.
  - Resolve INCONSISTENCIAS A4. Sem copiar a proporção de perda da referência — o COE ainda não tem atributo exibido.
- **HM-11 — Gesto de uma vez por dia [FUTURO].** Uma técnica por especialização que recarrega **com o descanso**
  (`TimeOfDayCycle`), nunca com relógio real. Sem cronômetro que puxe a criança de volta ao celular (ADR-0009).
- **HM-12 — Renome por feito [FUTURO].** A Guilda dos Exploradores (facção proposta, GDD cap. 08) reconhece feitos
  autorados e únicos, que alimentam a dimensão Renome. Sem patente em letra e sem feito repetível.
- **HM-13 — O Bosque sobe de grau [FUTURO].** Um marco da Trama muda o estado do Bosque (mais fundo, outras criaturas)
  pelo mecanismo que já existe (`PecaPorEvento`). Mudança autorada e por evento, nunca por relógio nem por nível do
  jogador.
- **HM-14 — Bestiário por observação [FUTURO].** Uma página do HM-01. A criatura entra no registro quando o jogador
  **observa** um comportamento (Tovin ensina a observar), e o que se sabe muda a abordagem, como na Q-05.
- **HM-15 — Papéis complementares [FUTURO distante].** Só se o co-op das expedições adultas sair do papel (CLAUDE.md:
  "não desenhar para ele agora"). Fica aqui só como nota.

---

## §4 — Descartado, e por quê

| Mecânica da referência | Por que não entra no COE |
|---|---|
| Modos de dificuldade escolhidos antes de nascer, multiplicando o custo de crescer | ADR-0004 (destino não é dificuldade) e prompt-mestre §4 (não copiar curvas). O COE já transformou essa inspiração em Destino de Nascimento. |
| Nível por abate / XP por monstro | GDD cap. 05 ("sem XP por golpe"); pilar "não farming"; o COE cresce por atividade significativa (`Mastery`). |
| Bônus de atributo pelo que se carrega (cartas no livro) | CLAUDE.md: sistema de status fora de escopo; números só depois de playtest (GDD cap. 13). A versão sem poder já existe: a manta guarda marcas e não dá nada. |
| Cartas, patentes em letra, fusão de cartas, classe "invocador" | prompt-mestre §4 (classes e designs da referência); o COE não tem classe rígida (GDD cap. 05). |
| Gacha, sorteio de tesouro, bilhete de sorte | ECA Digital art. 22 (ADR-0009): loot box proibida para criança e adolescente. Público infantil. |
| Aliados que acumulam progresso enquanto o jogador não joga | Anti-farm; cria pressão para voltar ao celular (ADR-0009). |
| Atributo liberado por idade em porcentagem | Sem sistema de status. A idade no COE abre *acesso* (treino aos 8, sem combate adulto na infância), não número. |
| Talento inato com estrelas que decide o lugar social | Contradiz "origem sem vantagem irreversível" e "o protagonista não precisa ser o único escolhido" (GDD cap. 01/02). A HM-03 é a versão invertida. |
| Deus-projetista que escreve as regras do jogo e manda mensagens | "Não é isekai" (GDD cap. 01). Aethron é guardião do Limiar, não designer de jogo. |
| XP dividida por tamanho do grupo, masmorras com andares e patentes | Sem multiplayer; sem masmorra no slice; o mundo é Auren + Bosque (CLAUDE.md). |
| Leilão mensal, economia de mercado | O protótipo tem só a economia cotidiana (GDD cap. 07). |

---

## §5 — O que precisa ser decidido

1. **HM-01 e HM-05 entram antes do playtest?** A recomendação é sim: são pequenas, só de leitura ou de transação, e
   fecham lacunas que a conformidade já marcou.
2. **HM-04 vale como regra das assistências?** Ela não decide *quais* assistências existem, só o limite delas.
3. **HM-02, HM-03 e HM-06a** esperam a rodada 1 do playtest? A recomendação é que sim, porque mexem em conteúdo e duração
   (H1, H5).
4. **HM-07** (criaturas) é o que mais aproxima o COE da referência. Se for desejado, abrir ficha G1 própria antes de
   qualquer outra coisa.

Aprovado um item, ele vira bloco com testes próprios. Se o item mudar o GDD, entra como adendo/delta (ADR-0003), nunca
como reescrita.
