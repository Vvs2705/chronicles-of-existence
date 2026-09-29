# CHRONICLES OF EXISTENCE — memória do projeto

Única memória viva do projeto. Vive no repositório e é atualizada ao fim de cada fase.

**Última atualização:** 2026-09-28, 16:17.
**Estado de maturidade:** nível 1 — Planejado. Há documentação versionada e projeto configurado. **Não há protótipo.** Nada foi jogado, medido ou testado como COE.

Ordem de leitura para quem chega: prompt-mestre → dossiê → GDD → backlog → este documento.

---

## 1. O que é

Action RPG narrativo em terceira pessoa, PC/Windows, single-player. Fantasia medieval estilizada com influência de anime. Cooperativo é possibilidade arquitetural futura (expedições da vida adulta), não compromisso de produção; MMORPG está explicitamente fora.

Não é isekai. A alma ainda não nasceu, encontra o Guardião Aethron no Limiar da Existência, escolhe um **Destino de Nascimento permanente** entre quatro (Vida Serena, Vida Normal, Vida Difícil, Vida da Ruptura) e depois uma entre **três origens familiares compatíveis** (agricultores, artesãos/comerciantes, guardiões regionais). Quatro destinos × três origens = até doze configurações iniciais, **não doze campanhas**. A vida começa jogável aos cinco anos, na vila de Auren, reino de Eldoria, continente Valtheris, mundo Eryndor.

**Pilares:** viver e crescer; escolher e transformar; superar a própria existência.

**Vertical slice ("A Primeira Existência"):** prólogo no Limiar → escolha de destino e origem → personalização → despertar aos cinco anos em Auren → atividades e vínculos → decisão com consequência → desaparecimento perto do bosque → símbolo do Limiar → salto temporal confirmado para ~8 anos → treino supervisionado de movimento, ataque, defesa e primeira magia → gancho narrativo. Uma vila e um bosque, dez NPCs relevantes, oito missões planejadas (cinco centrais, três opcionais).

O nome "Chronicles of Existence" é **provisório**. Disponibilidade de marca e domínio não foi pesquisada.

## 2. Onde estamos

### 2.1 O que é fato verificado

| Fato | Evidência |
|---|---|
| Os quatro documentos-fonte estão versionados no repositório | `docs/direcao/`, `docs/backlog/`, `docs/gdd/` |
| O projeto Unity existe, com editor 6000.3.23f1 fixado | `client/ProjectSettings/ProjectVersion.txt` |
| A camada técnica genérica está em `_COE` | `client/Assets/_COE/`, namespace `COE`, 31 arquivos `.cs` |
| Existe uma cena Bootstrap, gerada por script | `Editor/BootstrapSceneBuilder.cs`: chão, luz, input, save, cápsula do jogador e câmera |

### 2.2 O que NÃO é fato

- **Nenhuma tarefa do backlog está concluída.** T001 está em andamento pela raia do cliente Unity.
- **Nenhum teste do COE foi executado.** Ninguém abriu o Editor nem rodou o Test Runner. **Não há evidência de que o projeto compile.**
- **Nenhum dos 8 testes obrigatórios do backlog tem alvo implementado** — destino imutável, 12 combinações, idempotência de recompensa, save/load, salto temporal, missão opcional, domínio por repetição trivial, diálogo generativo.
- **Nenhuma arte do COE foi produzida.** Nenhum concept, nenhuma malha, nenhuma animação.
- **Nada foi jogado.** A hipótese de 45–75 minutos do slice não foi medida e não pode ser, porque não há slice.

### 2.3 Estado das decisões

Classificação do prompt-mestre: **APROVADO / PROPOSTA / HIPÓTESE A VALIDAR / FUTURO / DESCARTADO**.

**APROVADO** (decidido pelo idealizador, registrado no dossiê)
- Action RPG narrativo, terceira pessoa, PC, single-player primeiro (seção B).
- Quatro Destinos de Nascimento; destino **permanente**, sem troca (seção C).
- Três origens familiares compatíveis por destino, de arquétipos modulares (seção C).
- Separação em três sistemas distintos: destino permanente, Grau de Existência evolutivo, assistências de jogabilidade (seção C).
- Vida jogável começa aos cinco anos; envelhecimento por marcos narrativos anunciados e confirmados (seção D).
- Seis atributos: Força, Agilidade, Vigor, Intelecto, Percepção, Vontade (seção D).
- Nível de Vida persiste; Nível de Grau reinicia na ascensão (seção D).
- Quatro caminhos de ascensão: Divina, Arcana, Superação, Ruptura; ascensão pela Ruptura não é exclusiva da Vida da Ruptura (seção E).
- Reputação contextual por NPC e comunidade, não medidor binário universal (seção G).
- Vertical slice limitado a Auren e bosque (seção L).

**PROPOSTA** (escrito, aguardando decisão)
- [ADR-0002](adr/ADR-0002-riscos-de-originalidade.md): teste positivo de originalidade como portão obrigatório antes de qualquer arte. **Este é o item mais urgente da lista.**
- Cinco dimensões de reputação (Honra, Compaixão, Renome, Temor, Confiança) — o dossiê manda introduzir gradualmente, não construir os cinco (seção G).
- Cinco graus de existência (I Comum a V Primordial) como escala narrativa, sem promessa de implementação (seção E).
- Seleção de cinco missões centrais e três opcionais entre as oito concebidas (seção H).

**HIPÓTESE A VALIDAR** (só se resolve jogando ou medindo)
- Duração de 45–75 minutos do slice.
- Que a infância seja divertida de jogar.
- Que as escolhas sejam perceptíveis para o jogador.
- Que o salto temporal seja emocionalmente significativo.
- Que doze configurações de destino × origem produzam variação percebida e não doze reskins.
- Que a versão 6000.3.23f1 seja a versão certa para fixar (o dossiê fala em Unity 6 LTS e menciona 6.3 LTS).

**FUTURO** (fora do escopo agora, arquitetura preservada)
- Cooperativo, como expedições da vida adulta.
- Ascensão completa e seu balanceamento.
- Regiões além de Auren: Karvorn, Sylvara, Ashkar, Nharos.
- IA generativa para fala de NPC, sempre com fallback determinístico e sem autoridade sobre estado.

**DESCARTADO**
- Troca de dificuldade após o nascimento — identificado como exploit: começar no fácil, acumular poder e mudar para colher a recompensa da extrema (seção C).
- Herança genética complexa, múltiplas raças, nascimento por todo o continente e descendência na primeira versão (seção C).
- MMORPG.

## 3. Dívida técnica da camada genérica

O que ainda precisa mudar ou sair do código atual, arquivo por arquivo e com a tarefa do backlog correspondente, está em [docs/tech/DIVIDA_TECNICA.md](tech/DIVIDA_TECNICA.md).

## 4. Armadilhas conhecidas do ambiente

**Unity**
- Instalado em `C:\Program Files\Unity\Hub\Editor\6000.3.23f1`. A versão está fixada em `client/ProjectSettings/ProjectVersion.txt`; abrir com outra versão migra o projeto sem perguntar de forma reversível.
- **Um projeto Unity = uma instância por vez.** Se o Editor estiver aberto com `client/`, um build em batch mode falha por lock — e vice-versa. Fechar o Editor antes de rodar `build_windows.ps1`.
- Batch mode: `-batchmode -nographics -projectPath ... -executeMethod ... -logFile ... -quit`. O código de saída não conta a história toda: ler o log, que sai em `client/Builds/build_win.log`.
- `Active Input Handling` não tem API pública em `PlayerSettings`. Os scripts escrevem direto no `ProjectSettings.asset`, e **o valor só vale na próxima abertura do editor**.
- Um script de Editor que gera cena por código é versionável em diff; uma cena `.unity` editada à mão não é. O COE usa o primeiro padrão.

**Blender**
- Instalado em `C:\Program Files\Blender Foundation\Blender 5.2`. Roda headless: `blender.exe --background --python <script> -- <args>`.
- O gerador de placeholder (`client/tools/placeholder_humanoid.py`) já usa esse modo e produz exatamente o que `HumanoidSetup.cs` espera.

**Repositório**
- Remoto privado no GitHub: `chronicles-of-existence` (`origin`, branch `main`).
- O trabalho acontece em git worktrees sob `.claude/worktrees/`. A pilha de `git stash` é compartilhada entre todos eles: nunca usar `git stash` sem nome.

**Windows**
- Caminhos com espaço e acento (`MEUS PROJETOS`) quebram script que não cita o caminho. Citar sempre.
- PowerShell 5.1: sem `&&`, sem `??`, sem ternário. Encadear com `;` e `if ($?)`.
- Arquivos do repositório são UTF-8 sem BOM, com CRLF.
- `client/Library/`, `Temp/`, `Logs/`, `UserSettings/` e `Assets/StreamingAssets/content/` estão no `.gitignore`.

## 5. Riscos em aberto

| Risco | Gravidade | Estado |
|---|---|---|
| Repositório sem remoto: perda do disco apaga o COE | alta | mitigado: remoto privado no GitHub |
| Cometer o erro de originalidade por subtração e produzir personagens genéricos | **alta** | ADR-0002 escrito, aguarda aprovação |
| Escopo: o GDD descreve um continente, cinco graus e cooperativo; o slice é uma vila | alta | mitigado pelo dossiê (seção L), depende de disciplina |
| Combinatória de doze configurações × oito missões virar quatro campanhas | alta | mitigado no papel ("não escrever quatro campanhas"), não testado |
| Exploits de progressão: farming trivial, duplicação de recompensa, ascensão por menu | alta | listados na seção M do dossiê, nenhum teste existe |
| Licença e proveniência dos assets gerados por IA | alta | nenhum asset do COE gerado ainda; registrar desde o primeiro |
| Nome comercial não pesquisado | média | aberto |
| Versão do Unity a fixar em definitivo | baixa | 6000.3.23f1, a confirmar |

## 6. CONTINUAR DAQUI

### Próxima tarefa: T001 — Projeto e repositório Unity baseline

**Prioridade:** P0. **Dependência:** nenhuma. **Estado:** em andamento.

**Pré-requisitos**
1. Unity 6000.3.23f1 instalado (já está).
2. Editor fechado antes de qualquer build em batch mode.
3. Ter lido o contrato de trabalho do backlog: uma tarefa por vez, informar arquivos tocados e testes, não implementar multiplayer nem IA generativa em runtime, separar ScriptableObject de estado mutável, IDs estáveis e save versionado.

**Bloqueios reais que T001 tem que resolver primeiro**
1. **Provar que compila.** O projeto nunca foi aberto no Editor. Abrir `client/`, ler o console e rodar o Test Runner em Edit Mode é o primeiro passo verificável — e é o passo que ninguém deu.
2. Os wrappers `build_windows.ps1` e `run_windows.ps1` já usam `COE.exe`, `COE.EditorTools.BuildWindows.Build` e o log em `V-STACK\Chronicles of Existence`; o build por script foi executado e gerou o `.exe`.
3. Não há `StreamingAssets/content/` no disco. Sem arquivo de strings, o HUD mostra a chave crua (aparece `[perf.hud]` na cena Bootstrap) — o COE precisa dos seus próprios textos.
4. As demais cenas previstas pelo dossiê (MainMenu, CharacterCreation, TheLiminalRealm, Auren_Village, Auren_Interiors, Auren_Forest, Auren_Training) não existem — mas são das tarefas seguintes, não de T001.

**Critérios de aceite propostos para T001**
- O projeto abre no Editor sem erro de compilação e sem erro de import, **com o console mostrado como evidência**.
- A cena Bootstrap é gerada e é a primeira nas Build Settings.
- `powershell -File client\tools\build_windows.ps1` termina com sucesso e produz `client/Builds/win/COE.exe`.
- O executável abre uma janela sem erro no log.
- O Test Runner roda em Edit Mode e o resultado é informado como número, não como afirmação.
- `CLAUDE.md` descreve as convenções do COE (dono: raia do Unity).

**Fora de escopo de T001:** movimento de personagem, câmera, destino, origem, save, NPC, missão, qualquer arte.

### Depois de T001

Ordem do backlog, com dependências: **T002** (controller e câmera, depende de T001) → **T003** (Destiny + Origin, depende de T001) → **T004** (Save v1, depende de T003) → **T005** (LifeEventHistory) → **T006** (Quest) e **T007** (NPC e diálogo) → **T008** (Auren graybox, depende de T002).

### Decisões que o idealizador precisa tomar

1. **Aprovar ou não o ADR-0002** (teste positivo de originalidade como portão obrigatório). Precisa acontecer **antes** de qualquer encomenda de arte, não depois.
2. Confirmar a versão do Unity a fixar em definitivo.
3. Decidir sobre o remoto privado do repositório.
4. Decidir entre o carregador de strings atual (`StringsLoader`) e o pacote Localization, que está instalado e sem uso.

---

## Handoff (modelo da seção P do dossiê)

**Versão / data:** 2026-09-28, 16:17.
**Última decisão aprovada:** ver `docs/adr/`.
**Artefatos produzidos hoje:** `README.md`; `docs/PROJETO.md`; `docs/adr/ADR-0002-riscos-de-originalidade.md`; cópias-fonte em `docs/direcao/DOSSIE_CONTINUIDADE_v1_0.md`, `docs/direcao/PROMPT_MESTRE_AGENTE_v1_0.md` e `docs/backlog/BACKLOG_v1_1.md`; GDD extraído em `docs/gdd/` (outra raia).
**Sistemas existentes e testados:** **nenhum**. A camada genérica existe no repositório; ninguém abriu o Editor, então nem a compilação está verificada. **Testes não executados.**
**Propostas ainda abertas:** ADR-0002 (portão de originalidade); reputação em cinco dimensões; cinco graus de existência; seleção das oito missões.
**Riscos e bloqueios:** compilação nunca verificada; risco de cometer o erro de originalidade por subtração.
**Próxima tarefa recomendada:** T001, com os quatro bloqueios listados na seção 6 resolvidos primeiro.
**Mudanças necessárias no GDD / CLAUDE.md / backlog:** registrar o portão do ADR-0002 no capítulo de arte do GDD e uma linha em `CLAUDE.md` proibindo encomenda de arte sem ficha aprovada; conferir se o backlog, cuja fonte declarada é o GDD v1.1, continua coerente com o GDD v1.2.
