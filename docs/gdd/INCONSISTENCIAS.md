# INCONSISTÊNCIAS — GDD Mestre v1.2 × Dossiê de Continuidade v1.0 × Prompt-Mestre v1.0

Data da checagem: 2026-09-28.

## Regra de precedência aplicada

O próprio material define a hierarquia. Prompt-Mestre §2: *"Em caso de conflito, priorize a decisão expressa mais recente do idealizador, depois o registro de decisões, depois a versão mais nova do GDD."* Dossiê §A: *"Caso haja divergência, aplicar o controle de mudanças e confirmar a decisão mais recente; não apagar silenciosamente versões anteriores."*

Na prática, para as divergências abaixo:

1. **Decisão expressa mais recente do idealizador** — não há nenhuma registrada após 28/09/2026 sobre estes pontos específicos.
2. **Registro de decisões** — Dossiê §N (`Confirmado pelo idealizador` / `Decisões consolidadas` / `Ainda requer testes/decisão`).
3. **GDD mais novo** — GDD Mestre v1.2.

Documentos comparados:

- `docs/gdd/GDD_MESTRE_v1_2.md` (extração do `.docx` canônico)
- `C:\Users\VINICIUS\Downloads\COE_Dossie_Continuidade_v1_0.md`
- `C:\Users\VINICIUS\Downloads\COE_Prompt_Mestre_Agente_v1_0.txt`
- Referência cruzada: `C:\Users\VINICIUS\Downloads\Chronicles_of_Existence_Backlog_Claude_Code_v1_1.md`

---

## A. Contradições diretas

| # | Tema | Onde diz A | Onde diz B | Deve prevalecer | Severidade |
|---|---|---|---|---|---|
| A1 | Versão da Unity | GDD v1.2, Apêndice **B.1 "Baseline aprovado"**: `Unity 6.3 LTS, versão 6000.3.x fixada no início` | GDD v1.2, cap. 01: `Unity 6 LTS (revisão exata fixada ao iniciar repositório)`; GDD v1.2, cap. 13: `Versão exata da Unity — Fixar após verificar compatibilidade de pacotes`; Dossiê §K: *"foi sugerida Unity 6.3 LTS, mas fixar build e revalidar compatibilidade de pacotes ao iniciar implementação"*; Dossiê §N lista `versão exata Unity fixada no repositório` em **"Ainda requer testes/decisão"** | **B — PENDENTE.** O registro de decisões (Dossiê §N) vence o GDD, e o próprio GDD v1.2 se contradiz em dois capítulos. O rótulo "Baseline aprovado" do Apêndice B.1 não tem lastro; deve virar PROPOSTA até alguém abrir o Package Manager | ALTA |
| A2 | Fonte de verdade do Apêndice B e do backlog | GDD v1.2, callout do Apêndice B: *"Especificação operacional complementar ao GDD Mestre **v1.1**"*; GDD v1.2, Apêndice A: *"respeitando GDD **v1.1**"*; `Backlog_Claude_Code_v1_1.md` linha 2: `Fonte de verdade: GDD Mestre v1.1` | Dossiê §A, ordem de leitura: *"(3) GDD Mestre **v1.2** e revisões"* | **B — v1.2.** O v1.2 é a versão mais nova e o dossiê já aponta para ela. As três referências a v1.1 precisam ser atualizadas, senão o Claude Code implementa contra um documento superado | ALTA |
| A3 | Destino de nascimento = dificuldade? | Prompt-Mestre §5.4: `Vida Serena (fácil)` … `Vida da Ruptura (extrema)` — equipara destino a nível de dificuldade | GDD v1.2, cap. 02, callout REGRA BASE: *"Ascensão e assistências de jogabilidade são sistemas separados"*, e a tabela de destinos descreve experiência sem rótulo de dificuldade; Dossiê §C: *"Separar três sistemas: destino inicial permanente, Grau de Existência evolutivo, assistências/dificuldade momentânea"* e *"não somente HP dos inimigos"* | **B — separação.** Registro de decisões + GDD mais novo. Os rótulos "(fácil)"/"(extrema)" do Prompt-Mestre devem ser removidos: eles reintroduzem exatamente o exploit que o Dossiê §C descartou | ALTA |
| A4 | Nível de Grau após ascensão | GDD v1.2, cap. 03: *"Nível de Grau … **é reiniciado** após uma ascensão"*; Prompt-Mestre §5.7: *"Nível de Grau **reinicia** na ascensão"* | Dossiê §E: *"o Nível de Grau **pode** ir para 1 no novo grau"* | **A — reinício obrigatório.** Dois documentos afirmam, um permite. Ambiguidade de implementação em T009/Ascension; fixar como regra determinística | MÉDIA |
| A5 | Nome dos responsáveis familiares | GDD v1.2, cap. 06, tabela de NPCs: `NPC-01 Mara` e `NPC-02 Daren` como nomes fixos com ID estável | GDD v1.2, cap. 02: *"dois papéis principais adaptáveis (Mara e Daren, **nomes provisórios**)"*; Dossiê §C: *"A definição estética e os **nomes dos responsáveis podem variar** segundo a opção"* | **B — nome é dado de instância, ID é estável.** `NPC-01`/`NPC-02` permanecem estáveis; `displayName` é resolvido por destino/origem. Afeta T003 e T007 diretamente | MÉDIA |
| A6 | Bosque de Auren | Dossiê §I: *"bosque adjacente **Bosque dos Sussurros**"* (também na persona do agente) | GDD v1.2 **nunca nomeia o bosque** — usa `bosque adjacente` (cap. 08, 11) e a cena `Auren_Forest` (cap. 10) | **A — Bosque dos Sussurros** (nome provisório do registro de decisões). O GDD v1.2 precisa registrar o nome, senão narrativa e cena divergem | BAIXA |
| A7 | Grau II — quando prototipar | GDD v1.2, cap. 04, tabela: Grau `II — Desperta` com status `Estrutura/tutoria narrativa`; texto: *"Primeira ascensão funcional fica **fora do vertical slice**"* | Dossiê §E: *"dois graus podem ser prototipados isoladamente **posteriormente**"* (a persona do agente lê isso como *"os dois primeiros graus são suficientes para a prova inicial de ascensão"*) | **A — fora do slice.** "Posteriormente" não é "no slice". Risco de o agente puxar ascensão para dentro do escopo | MÉDIA |
| A8 | Sumário × títulos reais (interno ao GDD v1.2) | Mapa de capítulos: `07 — Missões, economia e exploits`, `09 — Arte, áudio e pipeline 3D`, `10 — Arquitetura técnica e dados`, `11 — Vertical slice de Auren`, `12 — Backlog e testes de aceitação`, `13 — Riscos, pendências e próximo marco` | Cabeçalhos reais: `07 — Missões, economia e prevenção de exploits`, `09 — Arte, áudio e produção 3D`, `10 — Arquitetura técnica e contratos de dados`, `11 — Vertical slice: A Primeira Existência`, `12 — Backlog técnico inicial e testes`, `13 — Riscos, pendências e próximos passos` | **B — cabeçalhos reais.** Divergência puramente editorial do `.docx`, mas quebra busca por título | BAIXA |
| A9 | Posição dos apêndices no sumário | Mapa de capítulos lista `Apêndice B — Fundação técnica T001` **entre** os capítulos 12 e 13, e **não lista o Apêndice A** | No corpo, a ordem real é: 12 → 13 → Apêndice A → Apêndice B | **B — ordem real do corpo.** Corrigir o sumário do próximo `.docx` | BAIXA |
| A10 | URL de referência da URP | GDD v1.2, "Referências técnicas verificadas": `docs.unity3d.com/**6000.0**/Documentation/Manual/urp/...` | GDD v1.2, Apêndice B.1: baseline `6000.3.x` | **B — 6000.3.x** (quando e se a versão for fixada, ver A1). A URL aponta para o ramo 6000.0 da documentação; "verificada" é afirmação forte para um link de outra versão | BAIXA |
| A11 | Nomes das classes de validação | Prompt-Mestre §7: `DestinySystem` registra origem; `AscensionValidator` autoriza ascensão | GDD v1.2, cap. 10, tabela de módulos: `Destiny` e `Ascension` (sem sufixo) | **B — nomes do GDD** como nome de **módulo**; `DestinySystem`/`AscensionValidator` podem ser os nomes das classes dentro deles. Registrar a convenção antes de T003 para não gerar dois namespaces | BAIXA |

---

## B. Lacunas (o dossiê exige, o GDD v1.2 não cobre)

Não são contradições de valor, são conteúdo presente em um documento e ausente no outro. Pela regra de precedência, o registro de decisões (Dossiê) vence o silêncio do GDD.

| # | Tema | Dossiê / Prompt-Mestre | GDD v1.2 | Ação |
|---|---|---|---|---|
| B1 | Módulos `Inventory` e `UI` | Dossiê §K lista ambos entre os módulos separados; Prompt-Mestre §15 e a persona do agente citam Inventory | A tabela "Módulos iniciais" (cap. 10) tem 9 módulos e **não** inclui Inventory nem UI — embora o contrato do módulo `Quest` diga *"Alterar UI ou inventário sem validação"*, referenciando módulos que o GDD não define | Adicionar os dois ao cap. 10 ou declarar explicitamente que estão fora do slice. Q-03 "O Cesto Perdido" e a economia cotidiana pressupõem inventário |
| B2 | Contrato de dados de inventário | Dossiê §K manda persistir `inventário` no save | "Contratos essenciais" (cap. 10) tem 7 contratos — `CharacterIdentity`, `LifeState`, `ProgressionState`, `LifeEvent`, `QuestState`, `NPCState`, `SaveMetadata` — e **nenhum** cobre inventário | Falta `InventoryState` (ou equivalente) antes de T004/T006 |
| B3 | Facções | Dossiê §I: Guilda dos Exploradores, Ordem dos Guardiões, Círculo Arcano, Companhia Mercantil | GDD v1.2 **não menciona facções** em nenhum capítulo | Registrar como FUTURO explícito no GDD, para não reaparecerem como escopo implícito |
| B4 | Locais de lore | Dossiê §I: Valecross, Academia de Lythar, Castelo de Eldoria, Ruínas de Elnor | GDD v1.2, cap. 08, cita só as 5 regiões e "academia" genérica dentro de Eldoria | Mover para a Bíblia do Universo; o GDD v1.2 (cap. 08: *"Todo o restante é lore ou expansão, ainda não mundo 3D comprometido"*) já é o limite de escopo correto |
| B5 | Marcos M1–M7 × backlog T001–T014 | Dossiê §L: `M1 movimentação; M2 nascimento/save; M3 Auren/NPC/quests; M4 progressão temporal; M5 combate; M6 apresentação artística; M7 integração e demonstração comercial` | GDD v1.2, cap. 12, usa **apenas** T001–T014 e não mapeia M→T | Sem o mapeamento, a regra do Dossiê §O.7 (*"passar ao marco pendente mais próximo"*) é ambígua. Publicar a tabela M↔T |
| B6 | Assistências / acessibilidade no slice | GDD v1.2, cap. 02: *"assistências de jogabilidade são sistemas separados"*; Prompt-Mestre §8: assistência *"não deve alterar retroativamente o destino narrativo"* | A tabela "Incluir / Excluir" do cap. 11 **não menciona** assistências nem acessibilidade em nenhuma das duas colunas; cap. 13 lista `Idiomas e acessibilidade` como pendência aberta | Decidir explicitamente se o slice tem assistências. Hoje o sistema existe no discurso e não existe no escopo |

---

## C. Documentação × repositório real

| # | Afirmação | Evidência no repositório | Conclusão |
|---|---|---|---|
| C1 | Dossiê §K: *"O histórico mostra que foram produzidos GDD Mestre v1.1, v1.2 e **pacote de preparação T001**"*; GDD v1.2 §B.6 lista os 7 arquivos do pacote (`README.md`, `CLAUDE.md`, `.gitignore`, `.gitattributes`, `PROJECT_SETUP.md`, `T001_CRITERIOS.md`, `PROMPT_T001.md`) | Nenhum desses 7 arquivos existe neste worktree. Não existe projeto Unity `ChroniclesOfExistence`. O único `ProjectSettings/` presente é `client/ProjectSettings/` | **T001 está NÃO INICIADO no repositório.** O pacote pode existir fora do repo (ex.: Downloads), mas o GDD não deve ser lido como "T001 entregue". O próprio Dossiê §K já alerta: *"Não confundir documentos entregues com código realmente implementado."* |
| C2 | A1 (versão da Unity) | `client/ProjectSettings/ProjectVersion.txt` → `m_EditorVersion: 6000.3.23f1` | Há um Editor 6000.3.23f1 efetivamente em uso na máquina — é **dado observado**, não aprovação. Serve como candidato concreto para fechar a pendência A1, depois de verificar os pacotes no Package Manager |
