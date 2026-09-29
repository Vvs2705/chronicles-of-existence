# ADR-0005 — Fonte única das missões e quem desaparece na Q-07

- **ID:** ADR-0005
- **Data:** 2026-09-28
- **Estado:** APROVADO (decisão delegada pelo Vinicius ao coordenador; ambas reversíveis)
- **Depende de:** ADR-0003
- **Documentos afetados:** `content/quests/**`, `client/Assets/_COE/Scripts/Quest/QuestCatalog.cs`, `docs/design/SLICE_A_PRIMEIRA_EXISTENCIA.md`

## Contexto

As entregas paralelas T006 (framework de missões) e T012 (roteiro e dados) produziram **duas descrições das mesmas 8 missões**: a tabela em C# de `QuestCatalog.cs` e os arquivos `content/quests/*.json`. Hoje elas concordam (a T012 alinhou ids, objetivos, tipos e recompensas ao que a T006 publicou). Duas cópias da mesma verdade divergem com o tempo, e a divergência aparece tarde — normalmente quando alguém joga.

Além disso, o dossiê §L manda a Q-07 ("O Desaparecimento") acontecer perto do bosque, mas **não diz quem desaparece**. Sem essa definição, a missão narrativa central e o beat emocional do slice não fecham.

## Decisão 1 — o catálogo em C# é a fonte de verdade; o JSON é a cópia de design, guardada por teste

`QuestCatalog.cs` continua sendo o que o jogo lê em runtime, pela mesma razão do `DestinyCatalog`: o código precisa conhecer os ids para validar, não há modo de falha "arquivo ausente", e o catálogo fica testável fora do Unity. Os `content/quests/*.json` continuam existindo porque são legíveis por quem escreve conteúdo e já têm validador próprio.

A divergência deixa de ser possível em silêncio: entra um **teste EditMode de paridade** que lê os JSON e compara com o catálogo (id, título, tipo, central/opcional, pré-condições, ids de objetivo e ids de transação de recompensa). Diferiu, fica vermelho, com a mensagem dizendo qual campo de qual missão.

**Rejeitado:** fazer o `QuestCatalog` carregar os JSON em runtime. Reintroduz o modo de falha de arquivo ausente/corrompido no caminho crítico da campanha, e o ganho (editar sem recompilar) não existe hoje, porque quem edita conteúdo ainda é o próprio time de código.

**Consequência:** ao mudar uma missão, mude nos dois lugares — o teste é o lembrete. Quando alguém que não programa assumir o conteúdo, a decisão se inverte e o catálogo passa a carregar o JSON (o caminho de upgrade já está comentado no código).

## Decisão 2 — quem desaparece na Q-07 é **Nilo**

Nilo é o amigo de infância impulsivo (dossiê §G) e é com ele que a Q-04 ("Uma Promessa") se compromete. Fazer desaparecer justamente a pessoa da promessa transforma a missão social opcional-em-aparência numa dívida concreta: a escolha do jogador na Q-04 volta como custo na Q-07, que é o que o dossiê §L pede do beat ("decisão com consequência" antes do desaparecimento).

**Rejeitados:**
- **Sera** — é a rival competitiva; o desaparecimento dela lê como remoção de obstáculo, não como perda.
- **Um NPC anônimo** — barato de escrever e sem peso; o slice existe para provar que crescer e ser lembrado importa.
- **Deixar em aberto até o playtest** — trava a Q-07 e a Q-08, que são centrais.

**Reversível:** é dado. Trocar quem desaparece é editar o JSON da Q-07, o catálogo e o roteiro; nenhuma regra depende do nome.

## Pendência que fica aberta — resolvida em 2026-09-29

A T008 acrescentou `horta_familia` à cena, e o objetivo `procurar_na_horta` aponta para ela. O texto abaixo fica como registro.

A âncora `horta_familia`, pedida pela T012 para o objetivo `procurar_na_horta` da Q-03, **não existe** na cena de Auren. Enquanto a T008 não a acrescentar, o objetivo aponta para `casa_familia` (fallback já previsto no roteiro). Item pequeno, fica na fila da próxima passada na cena.
