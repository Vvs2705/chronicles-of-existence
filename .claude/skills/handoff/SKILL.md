---
name: handoff
description: Fecha uma sessao de trabalho no COE deixando o estado verdadeiro no repositorio (snapshot do docs/PROJETO.md, divida, testes e builds reais, bloqueios, proximo passo). Use ao terminar um bloco, antes de parar, ou quando pedirem "atualiza o estado"/"handoff".
---

# Handoff do COE

O estado vive em `docs/PROJETO.md` (§0 e o fim do arquivo); a divida em `docs/tech/DIVIDA_TECNICA.md`. Nao crie outra fonte.

1. Numeros reais desta sessao: rode `client/tools/verify.ps1` (skill `verify`) se ainda nao rodou depois da ultima mudanca. Anote EditMode, PlayMode, roteiros, build. So o que rodou.
2. `docs/PROJETO.md` §0: atualize a tabela do snapshot (data, commit/branch, contagens, builds, aparelho), a tabela de blocos e os proximos passos. O que virou passado vai para o §7 HISTORICO, marcado com data; nada de "falta X" que ja foi feito.
3. `docs/tech/DIVIDA_TECNICA.md`: linhas pagas para "Paga", novas com prioridade e bloco (skill `techdebt`).
4. Handoff no fim do `PROJETO.md` (modelo da secao P do dossie): versao/data, ultima decisao aprovada, sistemas testados com numeros, propostas abertas, riscos e bloqueios (com etiqueta BLOCKED_*), proxima tarefa recomendada.
5. Trabalho nao commitado de outra sessao (outro worktree) nao se mexe; cite onde esta.
6. Commit `docs(...)` com as contagens na mensagem; push/PR/merge conforme o `CLAUDE.md`.
