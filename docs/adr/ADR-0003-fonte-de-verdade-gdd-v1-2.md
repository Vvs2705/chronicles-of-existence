# ADR-0003 — Fonte de verdade do produto é o GDD Mestre v1.2

- **ID:** ADR-0003
- **Data:** 2026-09-28
- **Estado:** APROVADO (decisão delegada pelo Vinicius ao coordenador)
- **Documentos afetados:** `docs/backlog/BACKLOG_v1_1.md`, `docs/gdd/`, `CLAUDE.md`, `docs/PROJETO.md`

## Contexto

Três documentos entregues pelo Vinicius discordam sobre qual é a fonte de verdade:

- `COE_Dossie_Continuidade_v1_0.md` §A manda ler o **GDD Mestre v1.2**;
- o **Apêndice A e o Apêndice B do próprio GDD v1.2** dizem "fonte de verdade: GDD v1.1";
- o `BACKLOG_v1_1.md` repete "Versão 1.1. Fonte de verdade: GDD Mestre v1.1".

Sem resolver isso, quem implementar uma tarefa T001–T014 pode codar contra um documento superado, e o próprio prompt-mestre (§2) proíbe promover hipótese a decisão sem registro.

## Decisão

**O GDD Mestre v1.2 (`docs/gdd/GDD_MESTRE_v1_2.md`) é a fonte de verdade de produto.** A regra de precedência, herdada do dossiê §A, fica explícita: decisão expressa mais recente do Vinicius → registro de decisões (estes ADRs) → versão mais nova do GDD → demais documentos.

Fato que torna a decisão barata: a extração dos dois `.docx` mostrou que **o v1.2 é puramente aditivo** em relação ao v1.1 — acrescenta o Apêndice B (fundação técnica T001), a linha no sumário e o número da versão, e **não altera nenhum número nem remove nenhum capítulo** (ver `docs/gdd/DELTA_v1_1_para_v1_2.md`). Portanto nenhuma tarefa do backlog muda de conteúdo; muda só qual arquivo se lê.

## Alternativas rejeitadas

1. **Editar a cópia do backlog para dizer "v1.2".** Rejeitada: as cópias em `docs/direcao/` e `docs/backlog/` são fontes entregues pelo Vinicius e o dossiê §O.3 proíbe reescrever capítulo prévio em silêncio. Em vez disso, foi acrescentada uma **nota do coordenador** no topo, visivelmente marcada como adendo.
2. **Esperar o Vinicius decidir.** Rejeitada: ele delegou, e a divergência bloqueia a T002 em diante.
3. **Congelar no v1.1 por ser o que o backlog cita.** Rejeitada: descartaria o Apêndice B, que é justamente a fundação técnica.

## Consequências

- `docs/backlog/BACKLOG_v1_1.md` ganhou nota de errata no topo; o corpo permanece intacto.
- Quando o GDD for revisado, a nova versão entra em `docs/gdd/` e o histórico vai para `docs/gdd/historico/`, sem apagar o anterior.
- Continua **aberta** a inconsistência A1 levantada na extração: o Apêndice B rotula `Unity 6.3 LTS / 6000.3.x` como "baseline aprovado" enquanto o capítulo 01, o capítulo 13 e o dossiê §N tratam a versão como pendência. Resolvida na prática pelo uso: o projeto compila, testa e builda em **6000.3.23f1**, que é o que está instalado. Registrar como decisão própria quando o Vinicius confirmar.
