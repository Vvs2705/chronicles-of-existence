# Dívida técnica da camada genérica

O que ainda precisa mudar ou sair do código atual de `client/`. Conferido contra o código em 2026-09-29; confira o estado real antes de agir sobre qualquer linha. Tarefas referenciadas: [`docs/backlog/BACKLOG_v1_1.md`](../backlog/BACKLOG_v1_1.md).

| Onde | Problema | Tarefa | O que fazer |
|---|---|---|---|
| `Scripts/Loc/StringsLoader.cs`, `Strings.cs` | Lê `StreamingAssets/content/strings.<idioma>.json`, que não existe; sem o arquivo, o HUD mostra `[chave]`. | **T007**, **T012** | Criar os textos do COE e decidir entre este carregador simples e o pacote Localization. |
| `Scripts/Input/PlayerInputReader.cs`, `Scripts/Input/ControlPreset.cs`, `Settings/ControlPreset_Destro.asset` | Caminho de toque (EnhancedTouch + zonas do `ControlPreset`) ainda compila e é injetado por `BootstrapSceneBuilder` e `AurenSceneBuilder`. HUD de toque está fora de escopo. | **T002** | Apagar o caminho de toque, o `ControlPreset`, o asset e as referências nos geradores de cena. Revalidar a ausência de Input Actions asset para rebind e acessibilidade. |
| `BootstrapSceneBuilder.cs` (cápsula de 2 m), `Scripts/Camera/ThirdPersonCamera.cs` (`pivotOffset` 1,5 m, `distance` 5 m), `Hitbox` a 0,9 m | Escala de adulto. O COE começa com um personagem de cinco anos. | **T002**, **T011** | Ajustar cápsula, enquadramento de câmera e alcance de golpe para escala de criança. A mira suave da câmera (gira para o inimigo mais próximo ao atacar) assume combate; rever para exploração de vila. |
| `client/tools/placeholder_humanoid.py` | Gera humanoide de 1,75 m (adulto). | **T002**, **T011** | Proporções infantis na tabela de ossos do topo do arquivo; é mudar números, não reescrever. |
| `Editor/HumanoidMapping.cs` (`HumanoidClip`), `Editor/HumanoidSetup.cs` | Clipes esperados: Idle, Run, Attack1-3, Dodge, Hit, Death, para uma única base corporal. Não há clipe para a primeira magia (`Skill`) nem conjunto etário. | **T011**, **T013** | Definir o conjunto de clipes do treino (espada de madeira + primeira magia), por idade (5 e 8 anos), e suportar as **três bases corporais** do dossiê (seção J). |
| `Scripts/Perf/PerfHud.cs` | Colunas de bateria e temperatura, leitura de zona térmica sob `#if UNITY_ANDROID`, limiares de celular. | **T013** | Tirar os ramos de celular e calibrar os limiares para PC. |
| `Assets/_COE/Settings/URP_Base.asset` | MSAA desligado, uma cascata de sombra, sem sombra de luz adicional: afinado para GPU de celular. | **T013** | Revisar sombras, antialiasing e pós-processamento para PC. |
| `PlayerInteractor.cs`, `DamagePopup.cs`, `PerfHud.cs` | Feedback desenhado com `OnGUI`/IMGUI, que é de protótipo. | **T013** | Substituir por UI de verdade na passagem de arte; o dossiê (seção J) pede VFX legível em três fases. |
