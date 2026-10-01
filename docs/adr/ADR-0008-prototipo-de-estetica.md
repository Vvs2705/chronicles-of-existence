# ADR-0008 — Protótipo de estética: exceção ao portão do ADR-0002

- **ID:** ADR-0008
- **Data:** 2026-09-30
- **Estado:** APROVADO pelo idealizador (Vinicius, 2026-09-30, escolhas na conversa: "exceção de protótipo", "anime estilizado", "imagens do acervo")
- **Depende de:** ADR-0002, ADR-0006
- **Documentos afetados:** `CLAUDE.md` (linha do portão), `docs/arte/PROVENIENCIA.md`, `docs/arte/PIPELINE.md`, `client/Assets/_COE/Art/`

## Contexto

O idealizador quer ver, no mesmo dia, uma cena jogável de Auren com a estética nova. O ADR-0002 exige ficha G1 aprovada, concept aprovado (G2) e licença registrada (G3) antes de qualquer malha; em 2026-09-30 existe só a ficha do Borin, sem aprovação final.

## Decisão

1. **Exceção temporária e marcada.** Malhas geradas no Tripo3D a partir de 2026-09-30 para a cena de protótipo podem entrar no projeto **sem** G1/G2/G3, desde que cada uma tenha bloco em `PROVENIENCIA.md` com `estado: PROTOTIPO` (ferramenta, plano, data, entrada usada) e fique em `client/Assets/_COE/Art/Prototipo/`.
2. **Nada marcado PROTOTIPO vai para build de loja.** Antes de qualquer envio, cada peça passa pelo portão normal (ficha G1 → concept G2 → G3) ou é substituída.
3. **Entrada permitida:** imagens do acervo de referência (ChatGPT, `imagens/` no checkout principal), apesar do PROVENIENCIA §3.2, só para o protótipo.
4. **Estética decidida (decisão 9 do PROJETO §6):** anime estilizado — cel-shading/toon no URP, cores chapadas, contorno leve, céu e névoa pintados.

## Consequências

- O risco de "personagem genérico" (ADR-0002) é aceito para o protótipo e volta a valer no portão.
- Plano gratuito do Tripo3D é de uso não comercial e pode tornar o modelo público no site: aceitável para protótipo, nunca para peça final.
- O validador de arte (V04) exige `g3` como data: as peças PROTOTIPO ficam fora de `Art/<Categoria>/<id>/` justamente para não fingir que passaram pelo portão.
- A V03 aceita `.jpg`/`.jpeg` só em `Art/Prototipo/` (o Tripo exporta a textura em JPEG; converter para PNG multiplicaria o peso no LFS de peça que vai ser trocada). Fora dali, e para master (`.blend`, `.psd`...), a regra segue igual (2026-10-01).
