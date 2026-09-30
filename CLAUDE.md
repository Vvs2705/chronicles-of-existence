# Chronicles of Existence (COE) — convenções técnicas

Action RPG em terceira pessoa, single-player, **mobile (Android primeiro, paisagem, toque)** — ADR-0006. Fonte de verdade de **produto** (GDD, destinos,
missões, elenco, slice): `docs/`. Este arquivo é só a convenção de **código**.

## Stack fixada
- Unity 6000.3.23f1 (`client/ProjectSettings/ProjectVersion.txt`) · URP · C#
- Input System (`PlayerInputReader`), Animator/Humanoid, ScriptableObjects, Unity Test Framework
- Git + Git LFS para binário. `.meta` de todo asset vai no commit.
- Nada entra na `Packages/manifest.json` sem estar em uso no mesmo commit.

## Pastas (`client/Assets/_COE/`)
- `Scripts/<Modulo>/` — um README de 2 linhas por módulo diz o que entra ali. Módulos:
  `Core`, `Character`, `Combat`, `Destiny`, `Ascension`, `LifeSystem`, `Progression`, `NPC`,
  `Dialogue`, `Quest`, `Reputation`, `Inventory`, `World`, `UI`, `Save`, mais `Anim`, `Camera`, `Input`, `Loc`, `Perf`.
- `Editor/` — ferramentas de editor (build, setup, geradores de cena e de humanoide).
- `Tests/EditMode|Editor|PlayMode/`, `Scenes/`, `Materials/`, `Settings/`.

## Assemblies
`COE` (runtime) · `COE.Editor` · `COE.Tests` (EditMode) · `COE.EditorTests` · `COE.PlayModeTests`.
Namespace `COE` no runtime, `COE.EditorTools` no editor. Módulo novo entra no asmdef existente;
asmdef novo só quando houver motivo de tempo de compilação.

## Regras de arquitetura
- **ScriptableObject é definição, não estado.** Asset compartilhado nunca guarda progresso de save.
- **IDs estáveis** (string, minúsculo, `snake_case`; `.` separa namespace, ex.: `evento.q04_concluida`,
  `rec.<missao>.<item>`) para NPC, item, missão, destino, origem, evento de vida e recompensa.
  ID publicado não muda; se mudar, precisa de migração.
- **Save versionado**: `SaveData.SchemaVersion` sobe junto com mudança de schema, com migração e
  teste de save antigo. Gravação atômica (tmp + replace). Campo novo nasce com padrão neutro.
- **Recompensa é idempotente**: recarregar, repetir ou interromper não duplica item, XP nem marco.
- Domínio (regras, cálculo, validação) em C# puro e testável; MonoBehaviour só orquestra.
- Dependência explícita (campo serializado, parâmetro). Sem event bus global, sem God Manager.
- Simplificação deliberada leva comentário `// ponytail:` dizendo o teto e o caminho de upgrade.

## Testes
- EditMode (`COE.Tests`) para regra pura — o padrão. PlayMode (`COE.PlayModeTests`) só para o que
  exige cena/frames (AnimationEvent, física, ordem de execução).
- Cada teste monta e destrói a própria mini-cena; nenhum depende de asset de cena.
- Lógica não trivial nova chega com pelo menos um teste que quebra se ela quebrar.

## Build e cena
- `Editor/BootstrapSceneBuilder.cs` → menu `COE / Gerar cena Bootstrap` recria
  `Assets/_COE/Scenes/Bootstrap.unity` do zero. **Não edite a cena à mão; edite o script.**
- `COE / Aplicar settings do projeto` (`ProjectSetup.Apply`) fixa URP, Quality e Player.
- Windows (só ferramenta de desenvolvimento, não é alvo de lançamento): `client/tools/build_windows.ps1` (`-executeMethod COE.EditorTools.BuildWindows.Build`)
  → `client/Builds/win/COE.exe`. Rodar: `client/tools/run_windows.ps1`.

## O que NÃO fazer
- Multiplayer/rede (co-op é expedição adulta, muito depois — não desenhar para ele agora).
- IA generativa em runtime com autoridade sobre estado: ela nunca concede item, missão ou save.
- Mapa continental / mundo aberto: o slice é Auren + Bosque dos Sussurros e mais nada.
- Lançar para PC/Windows agora (ADR-0006) e sistema de status: fora de escopo.
- Alterar decisão de GDD em silêncio: mudança de produto vira proposta em `docs/`, não commit.
- Gerar malha (Tripo3D, Blender) de personagem, criatura, local, item-assinatura ou VFX sem os portões do
  ADR-0002: ficha G1 aprovada, concept aprovado no G2, licença registrada no G3 (`docs/arte/PROVENIENCIA.md`).
