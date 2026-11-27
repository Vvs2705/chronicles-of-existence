# ADR-0011 — Geração antecipada pelo prazo do Tripo3D

- **Estado:** aceito (2026-11-27), pedido do idealizador no projeto TIME DE JOGOS: "Preciso usar esses créditos o quanto antes… Produzir o máximo possível do jogo e depois testamos."
- **Contexto:** as duas contas pagas do Tripo3D (A e B) vencem em 2026-11-29. O ADR-0002 e o CLAUDE.md proíbem gerar malha antes de G1 aprovado, G2 aprovado e registro na PROVENIENCIA. Hoje só 4 personagens (Borin, Daren, Tovin, Maelis) passaram no G2; criaturas do ep. 2 estão sem nota de G1; cenário e props nunca tiveram concept. Seguir os portões na ordem deixaria a maior parte do crédito vencer sem uso.
- **Decisão:**
  1. Até 2026-11-29 vale **gerar antes do portão**: concept e malha de todo id do `ASSETS.md` e do `LOTE_PRAZO_29.md`, com G1 aprovado ou não.
  2. Tudo vai para `arte/fonte/_antecipado/<id>/` no disco do PC, fora do git (repositório público, LFS de 1 GB). Proveniência mínima: uma linha por geração em `docs/arte/LOTE_PRAZO_29.csv`, gerada como **privada** e baixada no mesmo dia, com a conta paga ativa.
  3. **O portão de integração não muda:** nada desta pasta entra em `client/Assets/` sem G2 aprovado, G3 completo (PROVENIENCIA com o bloco do asset) e objetos separados do corpo. Uma malha antecipada que reprovar no G2 volta a ser só referência.
- **Consequências:** gasta crédito em malha que pode reprovar (aceito: o crédito venceria). O teste cego do G2 passa a usar concepts que já viraram malha; o juiz não pode saber disso. A dívida de proveniência fica no G3 de cada asset. Revisão jurídica antes de release comercial continua valendo (PROVENIENCIA §2).
