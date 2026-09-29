# DELTA — GDD Mestre v1.1 → v1.2

Fontes comparadas (extração 1:1 dos `.docx` originais entregues pelo idealizador):

- `docs/gdd/historico/GDD_MESTRE_v1_1.md` — 164 parágrafos de corpo, 20 tabelas, 147 linhas de tabela.
- `docs/gdd/GDD_MESTRE_v1_2.md` — 200 parágrafos de corpo, 21 tabelas, 155 linhas de tabela.

Método: diff textual integral dos dois markdowns gerados pelo mesmo conversor. O resultado abaixo é exaustivo — não há nenhuma outra diferença de texto entre as duas versões.

## Resumo

O v1.2 é o v1.1 **sem nenhuma alteração** nos capítulos 01 a 13 nem no Apêndice A. Todo o delta é **aditivo**: um capítulo novo no fim do documento, sua entrada no sumário e a troca do número da versão. Nenhuma regra, número, nome de NPC, missão, atributo, tabela de módulos ou contrato de dados foi alterado ou removido.

## 1. Metadados

| Onde | v1.1 | v1.2 |
|---|---|---|
| Linha de versão (capa) | `Versão 1.1 \| 28 de setembro de 2026` | `Versão 1.2 \| 28 de setembro de 2026` |
| Data | 28 de setembro de 2026 | 28 de setembro de 2026 (inalterada) |

A data é idêntica nas duas versões: ambas foram geradas no mesmo dia.

## 2. Seções novas no v1.2

### 2.1 Sumário ("Mapa de capítulos")

Linha nova: `Apêndice B — Fundação técnica T001`, inserida **entre** `12 — Backlog e testes de aceitação` e `13 — Riscos, pendências e próximo marco`.

### 2.2 "Registro da versão"

Entrada nova:

> v1.2 — Adiciona o Apêndice B com a fundação técnica T001, configurações de ambiente, contrato de trabalho do Claude Code e testes de aceite. Nenhum gameplay foi implementado.

A entrada do v1.1 permanece idêntica.

### 2.3 Capítulo novo: "Apêndice B — Fundação técnica T001"

Bloco inteiramente novo (78 linhas de markdown, 1 tabela nova de 8 linhas), no fim do documento, **depois** do Apêndice A. Subseções:

| Subseção | Conteúdo novo |
|---|---|
| Callout de abertura | Declara-se "especificação operacional complementar ao GDD Mestre v1.1". |
| B.1. Baseline aprovado | Tabela nova (7 itens): motor/pipeline, plataforma, repositório, editor, pacotes, cena inicial, fonte de verdade. |
| B.2. Fronteira de escopo | 4 bullets: incluído / excluído / não criar serializados e GUIDs à mão / não usar ScriptableObject como estado. |
| B.3. Execução sequencial | 7 bullets de passo a passo de setup no Hub e no Editor. |
| B.4. Critérios de aceite | 6 bullets verificáveis (Play Mode limpo, versões documentadas, Git, cena Bootstrap, cópia limpa, nada de T002–T014). |
| B.5. Contrato para Claude Code | Parágrafo de regras de trabalho do agente. |
| B.6. Entregáveis e transição | Lista dos 7 arquivos do pacote T001 e gatilho de liberação de T002. |
| Referências técnicas verificadas | 4 URLs (Unity 6.3 LTS, URP, controle de versão/meta, CLAUDE.md). |

## 3. Números que aparecem pela primeira vez no v1.2

Nenhum número preexistente foi alterado. Os valores abaixo são **novos** — só existem no Apêndice B:

| Item | Valor introduzido no v1.2 | Observação |
|---|---|---|
| Versão da Unity | `Unity 6.3 LTS, versão 6000.3.x fixada no início` | No v1.1 (e no capítulo 01 do próprio v1.2) o texto continua sendo apenas "Unity 6 LTS (revisão exata fixada ao iniciar repositório)". Ver `INCONSISTENCIAS.md`, item 1. |
| Template do projeto | `Universal 3D / URP` | Novo. |
| Cenas técnicas | `Bootstrap` + `T001_SmokeTest` | `T001_SmokeTest` não existia no v1.1. |
| Pacote T001 | 7 arquivos: README.md, CLAUDE.md, .gitignore, .gitattributes, PROJECT_SETUP.md, T001_CRITERIOS.md, PROMPT_T001.md | Novo. |

## 4. Decisões que mudaram de estado

| Decisão | Estado no v1.1 | Estado no v1.2 |
|---|---|---|
| Baseline de repositório/Editor (T001) | Existia apenas como linha de backlog `T001 / P0 — Repositório e Unity baseline`, sem especificação | Especificação operacional completa, rotulada "Baseline aprovado" no Apêndice B.1 |
| Versão exata da Unity | PENDENTE (capítulo 13) | **Continua PENDENTE no capítulo 13** e simultaneamente aparece como aprovada (6.3 LTS / 6000.3.x) no Apêndice B.1 — contradição interna, ver `INCONSISTENCIAS.md` |
| Contrato de trabalho do Claude Code | Apenas o Apêndice A (brief e Definition of Done) | Apêndice A inalterado + Apêndice B.5 com contrato operacional |

Nenhuma decisão foi rebaixada, revogada ou movida para DESCARTADO entre as duas versões.

## 5. Nada foi removido

O diff não contém **nenhuma** linha removida além da própria linha de versão da capa. Todos os 12 capítulos, o Apêndice A, o glossário e as 20 tabelas do v1.1 estão integralmente presentes no v1.2, palavra por palavra.

## 6. Diferenças que são só de formatação

- A entrada `v1.2 —` do "Registro da versão" não está em negrito, enquanto a entrada `**v1.1 —**` está. Puramente estilístico no `.docx` de origem.
- A tabela do Apêndice B.1 usa cabeçalho sem negrito (`Item` / `Definição`), diferente de todas as outras 20 tabelas do documento, que usam cabeçalho em negrito. Puramente estilístico.
