# CHRONICLES OF EXISTENCE — memória do projeto

Única memória viva do projeto. Vive no repositório e é atualizada ao fim de cada fase.

**Última atualização:** 2026-09-29 (T012: fiação em runtime; antes, T002–T011 e virada para mobile).
**Estado de maturidade:** nível 2 — sistemas do slice implementados e ligados em runtime (T001–T012 técnico). Dá para nascer e andar por Auren no celular; **ainda não é o slice jogável:** faltam conteúdo (textos, falas), a passagem do dia e a arte (T013).

Ordem de leitura para quem chega: prompt-mestre → dossiê → GDD → backlog → este documento.

---

## 1. O que é

Action RPG narrativo em terceira pessoa, **mobile — Android primeiro, paisagem, toque** ([ADR-0006](adr/ADR-0006-plataforma-mobile.md)), single-player. Fantasia medieval estilizada com influência de anime. Cooperativo é possibilidade arquitetural futura (expedições da vida adulta), não compromisso de produção; MMORPG está explicitamente fora.

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
| **T002–T011 aceitas** (2026-09-29, Unity 6000.3.23f1 em batch mode) | 0 erro e 0 aviso de compilação; EditMode **367 testes, 366 passam, 0 falham, 1 ignorado de propósito (rotina condicional de NPC, aguarda conteúdo da T012)**; PlayMode **20/20**; os 8 testes obrigatórios do backlog têm teste nomeado `Obrigatorio<n>_*` e passam (§6) |
| Pipeline de arte técnico pronto | `docs/arte/PIPELINE.md` (orçamento de celular), `docs/arte/PROVENIENCIA.md`, validador `COE / Validar arte` (25 de 27 regras), placeholder infantil de 1,10 m com avatar Humanoid de 19 ossos |
| **Rodou num Android real** (2026-09-29) | POCO F4 (Android 14, Adreno 650, 1080x2400): APK instalado, Auren aberta em paisagem com o toque, logcat da Unity sem erro; CSV do `PerfHud`: 30,3 FPS estável (travado na meta de 30), 33 ms por quadro, 95 MB alocados, sensor de 48–52 °C sem subir. É um aparelho acima do alvo de faixa média |
| Build Android por script | `client/tools/build_android.ps1` → `client/Builds/android/COE.apk` (BuildSummary Succeeded, APK de desenvolvimento com 41,2 MB, 2026-09-29) |
| **T001 concluída** (2026-09-29, Unity 6000.3.23f1 em batch mode) | 0 erro e 0 aviso de compilação; EditMode **275/275** (264 em `COE.Tests`, 11 em `COE.EditorTests`); PlayMode **15/15**; `build_windows.ps1` → `BuildSummary result=Succeeded`, `COE.exe` com 155,8 MB; `run_windows.ps1` abre a janela na Bootstrap e o `Player.log` sai sem erro |

### 2.2 O que NÃO é fato

- **Ainda não é o slice jogável.** A fiação existe (T012: nascimento → Auren com NPCs, missões, inventário e salto), mas faltam textos, falas, a passagem do dia e a arte (§6).
- **Só um aparelho, e acima do alvo.** O POCO F4 segura 30 FPS no graybox; aparelho de faixa média, arte real e sessão longa (aquecimento) ainda não foram medidos.
- **Nenhuma arte do COE foi produzida.** O humanoide em `Art/Humanoid/` é placeholder gerado por script (`client/tools/placeholder_humanoid.py`). As ~200 referências de concept vão para `arte/referencias/`.
- **Nada foi jogado.** A hipótese de 45–75 minutos do slice não foi medida e não pode ser, porque não há slice.

### 2.3 Estado das decisões

Classificação do prompt-mestre: **APROVADO / PROPOSTA / HIPÓTESE A VALIDAR / FUTURO / DESCARTADO**.

**APROVADO** (decidido pelo idealizador, registrado no dossiê)
- Action RPG narrativo, terceira pessoa, single-player primeiro (seção B). **Plataforma: mobile, Android primeiro** ([ADR-0006](adr/ADR-0006-plataforma-mobile.md), 2026-09-29), no lugar do PC do dossiê.
- [ADR-0002](adr/ADR-0002-riscos-de-originalidade.md): teste positivo de originalidade como portão obrigatório (G1 ficha → G2 concept → G3 licença) antes de qualquer malha. Linha no `CLAUDE.md`.
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
- Consequência da Q-04 na reputação: promessa cumprida leva a confiança de Sera e Nilo a +20; quebrada, a −20 (`ReputationSystem.Consequencias`, T010).
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
- Paisagem, layout de toque (botões ≥ 48 dp no canto do polegar), 30 FPS em Android de faixa média, orçamentos da `PIPELINE.md` §4 e API mínima 26: tudo a validar no primeiro aparelho.
- Escala do corpo: 1,10 m aos 5 anos e 1,28 m aos 8 (`BodyScale`, mediana OMS).

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
- **Android:** módulo, SDK (API 34–36), NDK e OpenJDK instalados junto do Unity; `adb` em `%LOCALAPPDATA%\Android\Sdk\platform-tools`. A primeira build Android reimporta tudo para a plataforma, e alternar entre Android e Windows reimporta de novo. `run_android.ps1` precisa de aparelho com depuração USB (sai com 2 sem aparelho).
- **Simular o celular no PC:** `client/tools/run_windows.ps1 -Celular -Scene Auren -KeepOpen` abre uma janela 20:9 (metade do POCO F4) com o toque feito pelo mouse, no tamanho relativo do aparelho. Um gamepad pareado no PC (um "Wireless Controller" Bluetooth estava pareado em 2026-09-29) também move o personagem: se ele andar sozinho, é o gamepad.
- Mudou um FBX em `Art/Humanoid/`? Rode `COE / Montar humanoide` (`HumanoidSetup.Run`) e depois `COE / Validar arte`. O avatar é mapeado por nome quando o esqueleto usa os nomes humanos do Unity.
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
| Cometer o erro de originalidade por subtração e produzir personagens genéricos | **alta** | ADR-0002 aprovado; nenhuma ficha G1 escrita ainda |
| Escopo: o GDD descreve um continente, cinco graus e cooperativo; o slice é uma vila | alta | mitigado pelo dossiê (seção L), depende de disciplina |
| Combinatória de doze configurações × oito missões virar quatro campanhas | alta | mitigado no papel ("não escrever quatro campanhas"), não testado |
| Exploits de progressão: farming trivial, duplicação de recompensa, ascensão por menu | alta | testes obrigatórios 1–8 nomeados e verdes; a revisão cruzada achou e fechou o salto sem a Q-08 e dois índices por instância |
| Desempenho em celular | alta | nada medido em aparelho; `PerfHud` grava CSV com FPS, memória, bateria e temperatura |
| GDD e dossiê ainda dizem "PC" | média | ADR-0006 prevalece; atualizar o GDD na próxima revisão de produto |
| Licença e proveniência dos assets gerados por IA | alta | nenhum asset do COE gerado ainda; registrar desde o primeiro |
| Nome comercial não pesquisado | média | aberto |
| Versão do Unity a fixar em definitivo | baixa | 6000.3.23f1, a confirmar |

## 6. CONTINUAR DAQUI

### Estado das tarefas (2026-09-29)

"Aceita" = regra implementada e testes verdes no Unity. Não quer dizer jogável: a fiação é a T012.

| Tarefa | Estado | Evidência |
|---|---|---|
| T001 baseline | concluída | tag `t001-baseline` |
| T002 controller e câmera | aceita | escala de criança (`BodyScale`), toque de volta (ADR-0006), mira só em hostil, sem `Find` global; `BootstrapSceneTests`, `TouchControlsTests` |
| T003 destino e origem | aceita | `Obrigatorio1_*`, `Obrigatorio2_*`; confirmação por `destinyId` (carimbo zerado no save não reabre) |
| T004 save v1 | aceita | `sceneId`/`anchorId`, migração v0→v1 com cópia, versão futura intacta (principal e `.bak`), `Obrigatorio4_*` |
| T005 histórico de vida | aceita | `T005_*`; idempotente entre instâncias |
| T006 missões | aceita | `Obrigatorio3_*`, `Obrigatorio6_*`; desfecho único da Q-04; `validate_quests.py` 11/11 |
| T007 NPC e diálogo | aceita | rotina interrompível e por memória (`NpcAgendaTests`); `Obrigatorio8_*` |
| T008 Auren graybox | aceita | percursos varridos com a cápsula da criança; `horta_familia` |
| T009 life system | aceita | `Obrigatorio5_*`, `Obrigatorio7_*`; o salto exige a Q-08 concluída; spawner de âncora |
| T010 reputação | aceita | `T010_*`; ids com `.` aceitos; Sera e Nilo pela Q-04 (proposta) |
| T011 treino de combate | aceita | `T011_*`; magia em três fases; o parceiro não mata a criança |
| T012 integração | fiação feita | sessão, entrada/nascimento, NPC e diálogo, missões no mundo, inventário, idade e salto (abaixo); falta conteúdo |
| T013 arte | bloqueada pelo ADR-0002 | falta ficha G1; pipeline e validador prontos |
| T014 regressão | a fazer | depende de T012 e T013 |

### T012 — fiação em runtime: feita (2026-09-29); conteúdo pendente

O que liga os sistemas em jogo (contrato em `Scripts/Core/GameSession.cs`, acessado por `SaveState.Sessao`):
- **Sessão sem God Manager:** abre histórico, missões e reputação sobre o save; toda transição Ok sincroniza memória de NPC, reputação e inventário e grava UMA vez; recusa não grava. O save é lido do disco uma vez por processo (trocar de cena não relê).
- **Entrada (ícone → jogo):** a Bootstrap decide: sem `birth.destinyId` → tela de nascimento (destino, origem, nome, "tem certeza?"; moedas e itens iniciais entram no inventário na mesma gravação); com destino → cena salva (Auren). `-scene` pula a entrada (desenvolvimento). Save de versão mais nova → aviso.
- **Auren:** 10 NPCs na âncora da rotina do período, conversa de toque que pausa a agenda e trava o jogador, objetivos com NPC cumpridos conversando (o diálogo pede, a missão decide), escolha da promessa da Q-04; q01 e q08 começam sozinhas; 6 gatilhos de objetivo nas âncoras; HUD de missão e moedas; inventário mínimo no save.
- **Idade:** corpo, câmera e golpe por idade (`Corpo.DaIdade`); o salto é oferecido no símbolo do Limiar da clareira (§4.1), com o aviso "o que se encerra", encerra as opcionais e recarrega Auren aos 8 anos em `spawn_player`.
- **Textos:** `client/Assets/_COE/Resources/strings.pt-BR.json` (lido por `Resources.Load`, funciona no APK) com rótulos de sistema e nomes já aprovados nos docs (destinos, origens, NPCs, títulos de missão, fases); `StringsCoberturaTests` quebra se uma tela usar chave de sistema ausente. Descrições, objetivos e falas continuam `[chave]` (conteúdo).
- **Configurações** (menu de pausa, botão "Menu" no topo): mão destra/canhota (espelha o toque), sensibilidade da câmera 0,5–2,0, 30 ou 60 FPS, HUD de desempenho; em `PlayerPrefs` (`coe.cfg.v1.*`), fora do save de progresso.
- **Onde o jogador está:** gatilho de objetivo e conversa gravam a âncora (`GameSession.Posicao`) na mesma gravação da transição.
- **Verificado:** EditMode 437 (436 ok, 1 ignorado), PlayMode 24/24; no PC em modo celular: nascimento → Auren com a q01 iniciada e 40 moedas, textos reais no HUD, joystick de toque move a criança, menu abre, pausa, troca para canhoto e volta.

Pendente (técnico):
1. O horário do dia não passa sozinho (ninguém chama `TimeOfDayCycle.Avancar`): os NPCs ficam na rotina da manhã. **Decisão de produto:** como o dia passa (descansar em casa? cada missão?).
2. NPCs sem colisor e teleportados entre vagas (sem NavMesh); dois deles nascem ao lado da câmera no spawn.
3. Conversa não navegável por gamepad.
4. No modo canhoto, o texto do HUD de desempenho (só em build de desenvolvimento) passa por cima do botão USAR.
5. Rótulos dos botões de toque (ATQ, FORTE, DEF, MAGIA, ESQ, USAR) e o "Examinar" do `SimpleInteractable` estão fixos no código, fora do arquivo de textos.

Conteúdo (criação, não técnico): textos `"[a escrever]"` das missões, `strings.pt-BR.json`, falas de 7 dos 10 NPCs, rotinas condicionais, evento de início do desaparecimento de Nilo.

### Arte (T013): pronto e pendente

- **Pronto:** `docs/arte/PIPELINE.md` (orçamento de celular, 27 regras), `docs/arte/PROVENIENCIA.md`, `COE / Validar arte`, placeholder infantil, `arte/referencias/` para os concepts.
- **Pendente:** fichas G1 (ADR-0002) → concept G2 → malha. O plano gratuito do Tripo3D é **uso não comercial**; o pago permite uso comercial (termos consultados em 2026-09-29). `HumanoidSetup` monta só `Art/Humanoid/` (precisa aceitar pasta por parâmetro), clip `Skill` da magia, footprints das estruturas para a regra V12.

### Decisões que o idealizador precisa tomar

1. **Aparelho mínimo de referência** (modelo ou faixa de GPU/RAM): define orçamento de arte e meta de FPS.
2. **Identificador do app:** `br.com.vstack.coe` (o valor que já estava no projeto). Não muda depois de publicado.
3. API mínima do Android: 26 (hipótese).
4. Regra da Q-04 na reputação (±20 para Sera e Nilo): aprovar ou ajustar.
5. [ADR-0004](adr/ADR-0004-destino-nao-e-dificuldade.md) promete que save editado não troca o destino, o que save local não garante. Recomendação: reescrever como "detecta id inválido, não promete anti-cheat local".
6. Carregador de strings atual (`StringsLoader`) ou pacote Localization. Enquanto isso, o HUD mostra a chave crua.
7. Rótulo "Vida Difícil": o B02 proíbe "difícil" como nível de desafio.
8. Correr no toque: joystick na borda ou botão próprio.
9. Plano do Tripo3D (o gratuito é não comercial).
10. Versão do Unity a fixar em definitivo.
11. **Como o dia passa** (manhã → tarde → noite): hoje não passa, e as rotinas dos NPCs ficam paradas na manhã.
12. "Acordar" (q01) exige andar até a porta de casa; se deve ser automático, é decisão de produto.

---

## Handoff (modelo da seção P do dossiê)

**Versão / data:** 2026-09-29, missão técnica pré-arte.
**Última decisão aprovada:** [ADR-0006](adr/ADR-0006-plataforma-mobile.md) — mobile, Android primeiro.
**Artefatos produzidos hoje:** T002–T011 aceitas; toque religado; build Android por script; `docs/arte/PIPELINE.md`, `docs/arte/PROVENIENCIA.md` e validador de arte; placeholder infantil; ADR-0006; `CLAUDE.md`, README e `docs/tech/DIVIDA_TECNICA.md` atualizados; revisão cruzada com os achados altos corrigidos.
**Sistemas existentes e testados:** EditMode 367 testes, 366 passam, 0 falham, 1 ignorado de propósito (rotina condicional de NPC, aguarda conteúdo da T012), PlayMode 20/20; 0 erro de compilação.
**Propostas ainda abertas:** regra da Q-04 na reputação; reputação em cinco dimensões; cinco graus de existência; seleção das oito missões.
**Riscos e bloqueios:** só um aparelho medido (POCO F4, acima do alvo); arte bloqueada até a ficha G1; sistemas sem fiação em runtime.
**Próxima tarefa recomendada:** T012, fiação em runtime (§6), e em paralelo as fichas G1 para destravar a arte.
**Mudanças necessárias no GDD / backlog:** trocar "PC" por mobile no GDD e no dossiê §B (ADR-0006); registrar o portão do ADR-0002 no capítulo de arte do GDD.
