# Chronicles of Existence (COE)

Action RPG narrativo em terceira pessoa, mobile (Android primeiro, paisagem, toque; [ADR-0006](docs/adr/ADR-0006-plataforma-mobile.md)), single-player. Fantasia medieval estilizada com influência de anime.
A premissa não é isekai: a alma ainda não nasceu, encontra o Guardião do Limiar, escolhe um Destino de Nascimento permanente e uma origem familiar, e a vida começa jogável aos cinco anos.
Os pilares são viver e crescer, escolher e transformar, superar a própria existência.
O primeiro alvo é um vertical slice na vila de Auren — não o continente, não MMORPG, não cooperativo.
Nome de trabalho provisório: disponibilidade de marca e domínio não foi pesquisada.

**Comece por [docs/PROJETO.md](docs/PROJETO.md)** — estado real, armadilhas do ambiente e a seção CONTINUAR DAQUI.

## Estado real (2026-09-29)

| Área | Estado |
|---|---|
| Documentação de direção (dossiê, prompt-mestre, backlog) | versionada neste repositório, em `docs/` |
| GDD Mestre v1.2 | extraído para `docs/gdd/` |
| Projeto Unity (`client/`) | scripts em `Assets/_COE/`, namespace `COE` |
| Backlog T001–T014 | **T001–T011 aceitas**; **T012 com a fiação feita** (entrada/nascimento → Auren com NPCs, missões, inventário e salto); falta conteúdo (textos, falas) e a arte |
| Compilação e testes | 0 erro de compilação; EditMode 367 testes, 366 passam, 0 falham, 1 ignorado de propósito (rotina condicional de NPC, aguarda conteúdo da T012); PlayMode 20/20 (2026-09-29) |
| Cenas | `Bootstrap.unity` e `Auren.unity`, geradas por script, nessa ordem no Build Settings |
| Build Android | `client/tools/build_android.ps1` → `COE.apk` (BuildSummary Succeeded, APK de desenvolvimento com 41,2 MB, 2026-09-29); rodou no POCO F4 (Android 14): Auren a 30 FPS estável, sem erro no logcat |
| Build Windows | só ferramenta de desenvolvimento; `COE.exe` gerado pelo script e aberto sem erro no log |
| Arte | pipeline e validador prontos (`docs/arte/`); nenhuma arte do COE; concepts vão para `arte/referencias/` |
| Dívida técnica | listada em [docs/tech/DIVIDA_TECNICA.md](docs/tech/DIVIDA_TECNICA.md) |

Pelo ícone o jogo já abre na tela de nascimento e entra em Auren; os textos ainda aparecem como `[chave]` e a arte é placeholder. Não há playtest.

## Como abrir o projeto Unity

O projeto Unity é a pasta `client/` (não a raiz do repositório).

1. Unity instalado: `6000.3.23f1`, em `C:\Program Files\Unity\Hub\Editor\6000.3.23f1`. A versão está fixada em `client/ProjectSettings/ProjectVersion.txt`; abrir com outra versão migra o projeto.
2. No Unity Hub: `Add` → `Add project from disk` → selecione a pasta `client`.
3. Um projeto Unity aceita **uma instância do Editor por vez**. Se outra sessão (ou um build em batch mode) já estiver com o projeto aberto, o Hub recusa ou o batch falha por lock.
4. Pacotes em `client/Packages/manifest.json`: URP 17.3.0, Input System 1.14.0, uGUI 2.0.0, Test Framework 1.5.1.
5. Binários (FBX, texturas, áudio) vêm pelo Git LFS: `git lfs install` uma vez na máquina antes de clonar.

## Como buildar e rodar

Os scripts ficam em `client/tools/`. O jogo é para Android (ADR-0006). A build de Windows é só ferramenta de desenvolvimento (teste rápido, captura de tela) e não decide nada de produto.

### Android (alvo do jogo)

Pré-requisitos: módulo Android do Unity 6000.3.23f1 (Android Build Support com SDK, NDK e OpenJDK) instalado pelo Hub. Para rodar, um celular ligado por USB com Opções do desenvolvedor → Depuração USB ativada, o aviso "Permitir depuração USB" aceito e a tela desbloqueada.

```powershell
powershell -ExecutionPolicy Bypass -File client\tools\build_android.ps1
powershell -ExecutionPolicy Bypass -File client\tools\run_android.ps1 -Seconds 25 -Shots 5
```

O build abre o Unity em batch mode com `-buildTarget Android` e chama `COE.EditorTools.BuildAndroid.Build`. Esse método aplica os settings (paisagem, IL2CPP, ARM64, API mínima 26, Vulkan com OpenGLES3 de reserva) e regera as cenas Bootstrap e Auren. Sai um APK de desenvolvimento em `client/Builds/android/COE.apk`, assinado com a chave de debug, e o log vai para `client/Builds/build_android.log`. A primeira rodada reimporta o projeto inteiro para Android e demora mais.

O `run_android.ps1` usa o `adb` do SDK do Android (`%LOCALAPPDATA%\Android\Sdk\platform-tools`) ou, se ele não existir, o do SDK da Unity. Ele instala o APK (`adb install -r`), abre o jogo, tira as capturas em `client/Builds/android/shots/` e mostra do `logcat` só os erros da Unity. Sem aparelho conectado, avisa e sai com código 2. Com mais de um aparelho, escolha com `-Serial <id do adb devices>`.

### Windows (ferramenta de desenvolvimento)

```powershell
powershell -ExecutionPolicy Bypass -File client\tools\build_windows.ps1
powershell -ExecutionPolicy Bypass -File client\tools\run_windows.ps1 -Seconds 25 -Shots 5
```

O build roda o Unity em batch mode (`-batchmode -nographics`), chama `COE.EditorTools.BuildWindows.Build`, que aplica os settings e regera as cenas Bootstrap e Auren por script, produz `client/Builds/win/COE.exe` e escreve `client/Builds/build_win.log`. O `run_windows.ps1` abre a janela, tira capturas e fecha.

**Modo celular no PC** (até haver arte e teste frequente em aparelho): simula o POCO F4 numa janela 1200x540 (20:9, paisagem). O toque é feito com o mouse, com joystick e botões na tela no mesmo tamanho relativo do aparelho, e o jogo roda travado em 30 FPS.

```powershell
powershell -ExecutionPolicy Bypass -File client\tools\run_windows.ps1 -Celular -Scene Auren -KeepOpen
```

Com um gamepad pareado no PC (por exemplo, um controle de PlayStation por Bluetooth), o analógico dele também move o personagem.

Verificado em 2026-09-29 (fechamento da T001): `BuildSummary result=Succeeded`, `COE.exe` com 155,8 MB, a janela abre na Bootstrap e o `Player.log` sai sem erro. Rodando dentro de um worktree, o `BuildSummary` conta 6 erros de import do URP causados pelo caminho longo; ver as armadilhas em `docs/PROJETO.md`.

## Onde ficam os documentos

| Caminho | O que é |
|---|---|
| `docs/PROJETO.md` | memória viva: estado real, armadilhas, CONTINUAR DAQUI |
| `docs/gdd/` | GDD Mestre v1.2 e histórico (v1.1) |
| `docs/direcao/DOSSIE_CONTINUIDADE_v1_0.md` | dossiê de continuidade, decisões A–Q (fonte, não editar) |
| `docs/direcao/PROMPT_MESTRE_AGENTE_v1_0.md` | prompt-mestre do agente diretor (fonte, não editar) |
| `docs/backlog/BACKLOG_v1_1.md` | backlog T001–T014 (fonte, não editar) |
| `docs/adr/` | registros de decisão de arquitetura |
| `docs/tech/DIVIDA_TECNICA.md` | o que ainda precisa mudar ou sair do código atual |

Ordem de leitura recomendada pelo dossiê: prompt-mestre → dossiê → GDD → backlog e repositório.

## Repositório

Remoto privado no GitHub: `chronicles-of-existence` (`origin`, branch `main`).
