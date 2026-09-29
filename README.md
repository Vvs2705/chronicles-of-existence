# Chronicles of Existence (COE)

Action RPG narrativo em terceira pessoa, PC/Windows, single-player. Fantasia medieval estilizada com influência de anime.
A premissa não é isekai: a alma ainda não nasceu, encontra o Guardião do Limiar, escolhe um Destino de Nascimento permanente e uma origem familiar, e a vida começa jogável aos cinco anos.
Os pilares são viver e crescer, escolher e transformar, superar a própria existência.
O primeiro alvo é um vertical slice na vila de Auren — não o continente, não MMORPG, não cooperativo.
Nome de trabalho provisório: disponibilidade de marca e domínio não foi pesquisada.

**Comece por [docs/PROJETO.md](docs/PROJETO.md)** — estado real, armadilhas do ambiente e a seção CONTINUAR DAQUI.

## Estado real (2026-09-28)

Seja honesto ao ler esta tabela: **não existe nenhuma cena do COE jogável**. O que existe é documentação nova e uma camada técnica genérica em `_COE`, ainda não integrada em nada.

| Área | Estado |
|---|---|
| Documentação de direção (dossiê, prompt-mestre, backlog) | versionada neste repositório, em `docs/` |
| GDD Mestre v1.2 | extraído para `docs/gdd/` |
| Projeto Unity (`client/`) | existe; scripts em `Assets/_COE/`, namespace `COE` |
| Dívida técnica da camada genérica | listada em [docs/tech/DIVIDA_TECNICA.md](docs/tech/DIVIDA_TECNICA.md) |
| Backlog T001–T014 | **nenhuma tarefa concluída**; T001 em andamento |
| Cena do COE | só a `Bootstrap.unity`, gerada por script no build (chão, luz, input, save, cápsula do jogador e câmera). Sem mundo, NPC ou missão |
| Compilação | **não verificada**. Ninguém abriu o Editor nem rodou o Test Runner |
| Build do COE | **não existe** (ver abaixo) |

Não há protótipo do COE. Não há playtest do COE. Nada neste repositório foi testado como Chronicles of Existence.

## Como abrir o projeto Unity

O projeto Unity é a pasta `client/` (não a raiz do repositório).

1. Unity instalado: `6000.3.23f1`, em `C:\Program Files\Unity\Hub\Editor\6000.3.23f1`. A versão está fixada em `client/ProjectSettings/ProjectVersion.txt`; abrir com outra versão migra o projeto.
2. No Unity Hub: `Add` → `Add project from disk` → selecione a pasta `client`.
3. Um projeto Unity aceita **uma instância do Editor por vez**. Se outra sessão (ou um build em batch mode) já estiver com o projeto aberto, o Hub recusa ou o batch falha por lock.
4. Pacotes em `client/Packages/manifest.json`: URP 17.3.0, Input System 1.14.0, Addressables 2.6.0, Localization 1.5.4, Test Framework 1.5.1.

## Como buildar e rodar no Windows

Os scripts ficam em `client/tools/`:

```powershell
powershell -ExecutionPolicy Bypass -File client\tools\build_windows.ps1
powershell -ExecutionPolicy Bypass -File client\tools\run_windows.ps1 -Seconds 25 -Shots 5
```

O build roda o Unity em batch mode (`-batchmode -nographics`), chama `COE.EditorTools.BuildWindows.Build`, que aplica os settings e gera a cena `Bootstrap.unity` por script, produz `client/Builds/win/COE.exe` e escreve `client/Builds/build_win.log`. O `run_windows.ps1` abre a janela, tira capturas e fecha.

Verificado em 2026-09-28 16:19: o projeto compila com 0 erro, 23/23 EditMode e 2/2 PlayMode passam, `COE / Aplicar settings do projeto` e `COE / Gerar cena Bootstrap` rodam, o build gera `client/Builds/win/COE.exe` e a cena abre (chão, luz, câmera, Player). **O Player ainda não anda: movimento é a T002.**

**Nada disso foi executado nem verificado.** Não há build do COE, não há executável, ninguém rodou este comando. T001 está em curso pela raia do cliente Unity; o estado atualizado fica em `docs/PROJETO.md`.

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

Remoto privado: `github.com/Vvs2705/chronicles-of-existence` (`origin`, branch `main`).
