---
name: techdebt
description: Confere a divida tecnica do COE contra o codigo real (docs/tech/DIVIDA_TECNICA.md, comentarios ponytail:, testes) e trata so a divida que ainda existe. Use quando pedirem para revisar, atualizar ou pagar divida tecnica, ou antes de planejar um bloco de hardening.
---

# Divida tecnica do COE

1. Leia `docs/tech/DIVIDA_TECNICA.md` (tabelas "Aberta" e "Paga") e o §0 do `docs/PROJETO.md`.
2. Para CADA linha aberta, confirme no codigo (grep e leitura do arquivo citado) se o problema ainda existe como descrito. Relatorio antigo e hipotese; grep e teste sao fato.
3. Colete os simplificacoes deliberadas: `grep -rn "ponytail:" client/Assets/_COE`. Cada uma diz o teto e o caminho de upgrade; so vira divida se o teto ja foi atingido (evidencia: bug, medicao, teste, playtest).
4. Cruze com as cercas automaticas em `client/Assets/_COE/Tests/EditMode/ArquiteturaTests.cs` (consumidores do `SaveState`, telas em IMGUI): lista que diminuiu = linha de divida que andou.
5. Atualize a tabela: linha paga vai para "Paga" com data e o que mudou; linha que mudou de natureza e reescrita; divida nova entra com prioridade (P0 perda de dado ou crash, P1 slice/mobile, P2 resto), bloco e acao minima. Bloqueio por ambiente leva a etiqueta (BLOCKED_HARDWARE, BLOCKED_CREDENTIAL, BLOCKED_LICENSE, BLOCKED_PRODUCT_DECISION, BLOCKED_EXTERNAL_SERVICE).
6. Pagar divida: mudanca pequena + teste que quebra se ela voltar + `verify` + commit `fix(...)`/`refactor(...)`. Mudanca de produto nao e divida: vira proposta em `docs/`.
