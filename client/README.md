# Chronicles of Existence — cliente Unity

Action RPG em terceira pessoa, single-player, **mobile (Android primeiro, paisagem, toque)**, Unity/URP.
Decisão de plataforma: `docs/adr/ADR-0006-plataforma-mobile.md`. A build de Windows é só ferramenta de desenvolvimento.
Convenções de código: `CLAUDE.md` na raiz do repositório. Fonte de verdade de produto: `docs/`.

**Estado:** base técnica do COE
(animação, câmera, input, dano/vida/hitbox, save, strings, ferramentas de editor).
**Não compilado desde a poda** — o Unity é aberto pelo coordenador.

## Abrir

Unity Hub → Add → selecione `client/`. O Hub lê `ProjectSettings/ProjectVersion.txt`
(`6000.3.23f1`). Na primeira abertura o Package Manager resolve `Packages/manifest.json`.

Depois, pelo menu do editor:

- **`COE → Aplicar settings do projeto`** (`ProjectSetup.Apply`): URP em Graphics/Quality,
  Player de Android (paisagem, IL2CPP, ARM64, API mínima 26, Vulkan + OpenGLES3) e de Windows 64 (dev),
  Active Input Handling = Both. O Input Handling só vale na próxima
  abertura do editor.
- **`COE → Gerar cena Bootstrap`** (`BootstrapSceneBuilder.Build`): recria
  `Assets/_COE/Scenes/Bootstrap.unity` do zero (chão, luz direcional, Input, Save, Player com
  CharacterController/Health/Hitbox/Faction/CharacterAnimator, câmera em terceira pessoa, Perf)
  e a registra no Build Settings. Idempotente — **não edite a cena à mão; edite o script.**
- **`COE → Montar humanoide`** (`HumanoidSetup.Run`): importa os FBX de
  `Assets/_COE/Art/Humanoid/` como Humanoid, põe os AnimationEvents nos clips e gera
  `Player.controller` + `PlayerModel.prefab`. Pasta ausente = loga e sai sem erro.

## Build e execução

Android é o alvo do jogo. Windows fica só como ferramenta de desenvolvimento.

```
powershell -ExecutionPolicy Bypass -File client/tools/build_android.ps1   # -> client/Builds/android/COE.apk (log: client/Builds/build_android.log)
powershell -ExecutionPolicy Bypass -File client/tools/run_android.ps1 -Seconds 25 -Shots 5
powershell -ExecutionPolicy Bypass -File client/tools/build_windows.ps1   # -> client/Builds/win/COE.exe (dev)
powershell -ExecutionPolicy Bypass -File client/tools/run_windows.ps1
```

Os dois builds rodam `ProjectSetup.Apply`, `BootstrapSceneBuilder.Build` e `AurenSceneBuilder.Build` antes do
`BuildPlayer`. Também dá para buildar pelo menu `COE → Build Android` / `COE → Build Windows`; o de Android troca a
plataforma do editor se precisar, o que reimporta os assets. O APK é de desenvolvimento, assinado com a chave de
debug da Unity. AAB e keystore de release ficam para quando houver loja.

`run_android.ps1` precisa de um celular por USB com Depuração USB ativada e autorizada. Ele instala o APK
(`adb install -r`), abre o jogo, salva as capturas em `client/Builds/android/shots/` e mostra do `logcat` só os
erros da Unity. Sem aparelho, sai com código 2.

## Testes

`Window → General → Test Runner`:

- **EditMode** (`COE.Tests`, `Tests/EditMode/`): `DamageFormulaTests`, `SaveDataTests`,
  `StringsTests`, `ComboCounterTests`, `CharacterAnimatorPendingHitTests`.
- **EditMode / Editor** (`COE.EditorTests`, `Tests/Editor/`): `HumanoidMappingTests`.
- **PlayMode** (`COE.PlayModeTests`, `Tests/PlayMode/`): `CombatIntegrationTests` — relay de
  AnimationEvent no modelo filho e fallback do golpe pendente. Cada teste monta e destrói a
  própria mini-cena; nenhum depende de cena salva ou de `StreamingAssets`.

## Estrutura

```
Assets/_COE/Scripts/       asmdef "COE", namespace COE
  Anim/       ponte com o Animator (AnimParams, CharacterAnimator, AnimEventRelay, PendingHit, ComboCounter)
  Camera/     ThirdPersonCamera (órbita, colisão, mira suave)
  Combat/     Damage, Health, Hitbox, Faction, HitFlash, DamagePopup
  Input/      PlayerInputReader: teclado/mouse e gamepad
  Loc/        Strings + StringsLoader (texto por chave semântica)
  Perf/       PerfHud (FPS na tela + CSV)
  Save/       LocalSave, SaveData, SaveBootstrap
  <módulos vazios do backlog: Core, Character, Destiny, Ascension, LifeSystem,
   Progression, NPC, Dialogue, Quest, Inventory, World, UI — cada um com README>
Assets/_COE/Editor/        asmdef "COE.Editor", namespace COE.EditorTools
  BuildAndroid, BuildWindows, BootstrapSceneBuilder, ProjectSetup, HumanoidSetup, HumanoidMapping
Assets/_COE/Tests/         EditMode / Editor / PlayMode
```
