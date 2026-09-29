# Chronicles of Existence — Backlog inicial para Claude Code

> **Nota do coordenador, 2026-09-28 (ADR-0003):** cópia fiel do backlog v1.1 entregue pelo Vinicius. A linha abaixo diz "GDD Mestre v1.1"; a fonte de verdade vigente é o **GDD Mestre v1.2** (`docs/gdd/GDD_MESTRE_v1_2.md`). O v1.2 é aditivo (só acrescenta o Apêndice B), então nenhuma tarefa T001–T014 muda de conteúdo — mas quem implementar lê o v1.2.

Versão 1.1. Fonte de verdade: GDD Mestre v1.1.

## Contrato de trabalho
- Implementar uma tarefa por vez.
- Informar arquivos criados/alterados, passos de setup no Unity e testes.
- Não implementar multiplayer, IA generativa em runtime nem mapa continental no primeiro slice.
- Separar ScriptableObject definitions de estado mutável persistente.
- Usar IDs estáveis, eventos idempotentes e save versionado.
- Não modificar decisões do GDD silenciosamente.

## Ordem
| ID | Entrega | Prioridade | Dependência |
|---|---|---|---|
| T001 | Projeto e repositório Unity baseline | P0 | — |
| T002 | Controller e câmera | P0 | T001 |
| T003 | Destiny + Origin | P0 | T001 |
| T004 | Save v1 | P0 | T003 |
| T005 | LifeEventHistory | P0 | T004 |
| T006 | Quest framework | P0 | T005 |
| T007 | NPC e diálogo | P0 | T005 |
| T008 | Auren graybox | P0 | T002 |
| T009 | Life System | P1 | T004, T005 |
| T010 | Reputação mínima | P1 | T005, T007 |
| T011 | Treino de combate | P1 | T002, T009 |
| T012 | Missões e transição narrativa | P1 | T006–T011 |
| T013 | Passagem de arte e performance | P1 | T008, T011 |
| T014 | Regressão do slice | P0 | T003–T013 |

## Testes obrigatórios
1. Destino imutável depois do nascimento.
2. 12 combinações destino/origem carregam sem falha.
3. Conclusão e recompensa de missão são idempotentes.
4. Save/load restaura histórico e NPCs.
5. Salto temporal exige confirmação e não duplica.
6. Missão opcional não bloqueia campanha.
7. Ação trivial repetida não gera domínio ilimitado.
8. Diálogo generativo eventual não altera inventário/missões diretamente.

## Primeiro prompt
Inspecione este projeto Unity e prepare T001 somente. Antes de editar, descreva os arquivos,
pacotes e assemblies envolvidos. Não implemente outras tarefas. Ao terminar, forneça os
passos de verificação no Editor e os resultados dos testes disponíveis.
