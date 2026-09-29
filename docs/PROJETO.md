# CHRONICLES OF EXISTENCE — memória do projeto

Única memória viva do projeto. Vive no repositório e é atualizada ao fim de cada fase.

**Última atualização:** 2026-09-29 (fechamento da T001).
**Estado de maturidade:** nível 1 — Planejado, com baseline técnico verificado. O projeto compila, os testes passam e a build abre. **Não há slice jogável.** Nada foi jogado ou medido como COE.

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
| A camada técnica está em `_COE` | `client/Assets/_COE/`, namespace `COE`: 56 `.cs` de runtime, 6 de editor, 31 de teste |
| Cenas Bootstrap e Auren, geradas por script, nessa ordem no Build Settings | `Editor/BootstrapSceneBuilder.cs`, `Editor/AurenSceneBuilder.cs`, `ProjectSettings/EditorBuildSettings.asset` |
| **T001 concluída** (2026-09-29, Unity 6000.3.23f1 em batch mode) | 0 erro e 0 aviso de compilação; EditMode **275/275** (264 em `COE.Tests`, 11 em `COE.EditorTests`); PlayMode **15/15**; `build_windows.ps1` → `BuildSummary result=Succeeded`, `COE.exe` com 155,8 MB; `run_windows.ps1` abre a janela na Bootstrap e o `Player.log` sai sem erro |

### 2.2 O que NÃO é fato

- **Só a T001 está concluída.** Há código em `_COE` que antecipa partes de T002–T011 (destino, save, histórico de vida, missão, diálogo, reputação, Auren, treino de combate). Nenhuma dessas tarefas foi aceita; cada uma confere o próprio aceite quando chegar a vez.
- **Os 8 testes obrigatórios do backlog não foram conferidos item a item** contra os 290 testes atuais (destino imutável, 12 combinações, idempotência de recompensa, save/load, salto temporal, missão opcional, domínio por repetição trivial, diálogo generativo).
- **Nenhuma arte do COE foi produzida.** O humanoide em `Art/Humanoid/` é placeholder gerado por script (`client/tools/placeholder_humanoid.py`).
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
- **Caminho longo em worktree.** Dentro de `.claude\worktrees\<nome>\client`, os shadergraphs de exemplo do URP no `Library\PackageCache` passam de 260 caracteres. O import deles falha (`DirectoryNotFoundException`), e o URP conta 6 erros no `BuildSummary`, mesmo com `LongPathsEnabled=1` no Windows. A build sai assim mesmo. No checkout principal o caminho é 49 caracteres mais curto e o erro não aparece. Para medir "sem erro de import", rode no checkout principal.
- A build é Development, e a primeira execução de cada `COE.exe` novo abre o alerta do Firewall do Windows (porta do profiler). Pode cancelar: o jogo não usa rede.
- Depois de uma build, o `git status` acusa `Bootstrap.unity` e vários `.asset` de `ProjectSettings/` como modificados. A cena é regerada com `fileID` novos e os assets são regravados com LF: nada muda no conteúdo. Desfaça com `git checkout --` antes de commitar, a não ser que você tenha mexido no gerador.

**Blender**
- Instalado em `C:\Program Files\Blender Foundation\Blender 5.2`. Roda headless: `blender.exe --background --python <script> -- <args>`.
- O gerador de placeholder (`client/tools/placeholder_humanoid.py`) já usa esse modo e produz exatamente o que `HumanoidSetup.cs` espera.

**Repositório**
- Remoto privado no GitHub: `chronicles-of-existence` (`origin`, branch `main`).
- O trabalho acontece em git worktrees sob `.claude/worktrees/`. A pilha de `git stash` é compartilhada entre todos eles: nunca usar `git stash` sem nome.
- Binário de arte e áudio vai para o Git LFS pelo `.gitattributes` da raiz. Os FBX entraram no LFS na T001 sem reescrever o histórico: o commit inicial ainda guarda os binários direto no git.

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

### T001 — concluída em 2026-09-29

Critérios do GDD v1.2 (Apêndice B.4), com a evidência de cada um:

| Critério | Evidência |
|---|---|
| Abre e roda sem erro de compilação nem exceção na cena técnica | 0 `error CS`; PlayMode 15/15; `COE.exe` abre na Bootstrap e o `Player.log` sai sem erro. A Bootstrap faz o papel de cena técnica; `T001_SmokeTest` não foi criada |
| Versão do Editor, dos pacotes e commit baseline documentados | Editor e pacotes no `README.md`; o baseline é o commit que fecha a T001, com a tag `t001-baseline` |
| Assets, Packages e ProjectSettings no git com `.meta`; Library e UserSettings fora | `git ls-files client/Library client/UserSettings` vazio; Force Text (`m_SerializationMode: 2`) e Visible Meta Files ligados |
| Bootstrap salva e primeira no perfil de build | `EditorBuildSettings.asset`: Bootstrap, depois Auren |
| Cópia limpa abre na mesma versão sem perder referência | Import do zero (`Rebuilding Library because the asset database could not be found`) no worktree e no checkout principal: EditMode verde nos dois, 0 referência perdida, 0 erro de import no checkout principal |
| Nada de T002–T014 antecipado; impedimento registrado | **Não atendido, e registrado:** o código de `_COE` já antecipa partes de T002–T011 (seção 2.2). Cada tarefa confere o próprio aceite quando chegar a vez |

Na T001 também entraram: `.gitattributes` com Git LFS para binário; Addressables e Localization saíram do manifest (sem uso, regra do `CLAUDE.md`; o "276" citado antes contava um teste de exemplo do próprio Addressables); `TrainingDummy` passou a usar o ritmo configurado no Inspector (antes os campos eram ignorados, e o compilador acusava `CS0414`).

### Próxima tarefa: T002 — controller e câmera

**Prioridade:** P0. **Dependência:** T001 (concluída). GDD: "Movimenta/interage sem referências globais rígidas."

O controller, a câmera e a interação já existem em `_COE` (`CharacterMotor`, `ThirdPersonCamera`, `PlayerInteractor`, `PlayerInputReader`). A T002 é aceitar e ajustar esse código, não escrever do zero. O que está pendente para ela na [dívida técnica](tech/DIVIDA_TECNICA.md):
1. Apagar o caminho de toque (`ControlPreset`, EnhancedTouch, `ControlPreset_Destro.asset`) e as referências a ele nos geradores de cena.
2. Passar cápsula, câmera e placeholder para escala de criança de cinco anos.
3. Rever a mira suave da câmera, que assume combate, para exploração de vila.

Depois, na ordem do backlog: **T003** (Destiny + Origin) → **T004** (Save v1) → **T005** (LifeEventHistory) → **T006** (Quest) e **T007** (NPC e diálogo) → **T008** (Auren graybox).

### Decisões que o idealizador precisa tomar

1. **Aprovar ou não o ADR-0002** (teste positivo de originalidade como portão obrigatório). Precisa acontecer **antes** de qualquer encomenda de arte, não depois.
2. Confirmar a versão do Unity a fixar em definitivo.
3. Decidir entre o carregador de strings atual (`StringsLoader`) e o pacote Localization. Localization saiu do manifest na T001 por falta de uso; volta se for o escolhido. Enquanto isso, o HUD mostra a chave crua (`[perf.hud]`).

---

## Handoff (modelo da seção P do dossiê)

**Versão / data:** 2026-09-29, fechamento da T001.
**Última decisão aprovada:** ver `docs/adr/`.
**Artefatos produzidos hoje:** `.gitattributes` (LFS); FBX no LFS; manifest sem Addressables e Localization; correção do `TrainingDummy`; este documento, `README.md` e `docs/tech/DIVIDA_TECNICA.md` atualizados.
**Sistemas existentes e testados:** baseline técnico verificado: compila, EditMode 275/275, PlayMode 15/15, build Windows gerada e aberta. Os sistemas de T002–T011 que já existem em código não foram aceitos.
**Propostas ainda abertas:** ADR-0002 (portão de originalidade); reputação em cinco dimensões; cinco graus de existência; seleção das oito missões.
**Riscos e bloqueios:** risco de cometer o erro de originalidade por subtração; código antecipado de T002–T011 ainda por aceitar tarefa a tarefa.
**Próxima tarefa recomendada:** T002 (seção 6).
**Mudanças necessárias no GDD / CLAUDE.md / backlog:** registrar o portão do ADR-0002 no capítulo de arte do GDD e uma linha em `CLAUDE.md` proibindo encomenda de arte sem ficha aprovada; conferir se o backlog, cuja fonte declarada é o GDD v1.1, continua coerente com o GDD v1.2.
