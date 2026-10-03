# Contrato T003 (Destiny + Origin) × T004 (Save v1)

Escrito pelo coordenador em 2026-09-28, **antes** de despachar as duas tarefas, porque elas se encontram no mesmo arquivo de save. Fonte: `docs/gdd/GDD_MESTRE_v1_2.md` (ADR-0003), `docs/direcao/DOSSIE_CONTINUIDADE_v1_0.md` §C, §D e §K, `docs/adr/ADR-0004-destino-nao-e-dificuldade.md`, `docs/backlog/BACKLOG_v1_1.md`.

Regra de convivência: **T003 é dona do domínio** (destino, origem, validação); **T004 é dona da persistência** (formato, versão, escrita, migração). Nenhuma das duas edita arquivo da outra; o encontro é só por estes tipos.

## 1. Identificadores estáveis (nunca mudam depois de gravados em save)

Destinos (`DestinyId`) — nomes de **circunstância de vida**, nunca de dificuldade (ADR-0004):

| Id | Rótulo provisório de interface |
|---|---|
| `serena` | Vida Serena |
| `normal` | Vida Normal |
| `dificil` | Vida Difícil |
| `ruptura` | Vida da Ruptura |

Origens (`OriginId`) — três arquétipos, reaproveitados pelos quatro destinos (dossiê §C: 12 configurações, **não** 12 campanhas):

| Id | Arquétipo |
|---|---|
| `agricultores` | família de agricultores |
| `artesaos` | artesãos / comerciantes |
| `guardioes` | guardiões regionais |

Atributos (`AttributeId`): `forca`, `agilidade`, `vigor`, `intelecto`, `percepcao`, `vontade`.
Afinidades (`AffinityId`): `marcial`, `arcana`, `natural`, `artesanal`, `social`, `exploratoria`.

Regra: id em `snake_case` ASCII, sem acento. O rótulo acentuado vive na camada de texto, nunca no save.

## 2. O que T003 entrega e o que T004 consome

T003 expõe um DTO puro e serializável (sem UnityEngine), que é **exatamente** o que T004 grava:

```csharp
[Serializable] public class BirthChoice
{
    public string destinyId;      // um dos 4 acima
    public string originId;       // um dos 3 acima
    public string characterName;  // nome do avatar, validado por T003
    public long   confirmedAtUtc; // ticks UTC do momento da confirmação; 0 = ainda não confirmado
}
```

- `confirmedAtUtc > 0` significa **escolha confirmada e permanente**.
- T004 grava e lê esse bloco sem interpretá-lo. T004 **não** valida regra de destino; T003 **não** decide formato de arquivo.

## 3. Invariantes (viram teste, não comentário)

Vindos do ADR-0004 e do backlog §"Testes obrigatórios":

1. **Destino permanente.** Depois de `confirmedAtUtc > 0`, nenhuma API troca `destinyId` nem `originId`. A tentativa falha de forma explícita (retorno/exceção), nunca em silêncio.
2. **As 12 combinações carregam.** Os 4 destinos × 3 origens produzem configuração válida; nenhuma combinação lança nem fica sem dados.
3. **Round-trip.** Salvar e carregar devolve exatamente o mesmo `BirthChoice`, inclusive acento no nome e `confirmedAtUtc`.
4. **Versão e migração.** O save carrega `saveVersion`; save de versão anterior ou desconhecida **não derruba o jogo**: migra ou é descartado com log claro.
5. **Save corrompido/ausente** devolve estado padrão sem exceção não tratada (já coberto hoje por `SaveDataTests`; manter).
6. **Escrita atômica.** Interrupção no meio da gravação não deixa save meio escrito (o `LocalSave` já grava em `.tmp` + move; manter e testar).
7. **Assistência de jogabilidade não toca em destino/origem.** Se existir campo de dificuldade/assistência no save, mudá-lo não altera nenhum campo de `BirthChoice`.

## 4. Fronteiras de arquivo

| Tarefa | Pode criar/editar | Não toca |
|---|---|---|
| **T003** | `Scripts/Destiny/**`, `Tests/EditMode/Destiny*Tests.cs`, definições em `Scripts/Destiny/Data/` | `Scripts/Save/**`, `Editor/**`, `docs/**` |
| **T004** | `Scripts/Save/**`, `Tests/EditMode/Save*Tests.cs` | `Scripts/Destiny/**`, `Editor/**`, `docs/**` |

`Editor/BootstrapSceneBuilder.cs` e qualquer fiação em cena são do **coordenador**, depois das duas entregas.

## 5. Decisões já tomadas (não reabrir)

- **Definição ≠ estado.** Catálogo de destinos/origens pode ser ScriptableObject ou JSON, mas **nunca** guarda escolha do jogador (CLAUDE.md e dossiê §K).
- **Sem UI nesta leva.** T003 e T004 entregam domínio e persistência; tela de criação de personagem é tarefa posterior.
- **Sem Ascensão.** Grau de Existência fica fora (dossiê §E: fora do primeiro slice).
- **Idade inicial 5 anos** entra como dado do save (`ageYears`), mas a progressão de fases é T009.

## 6. Desvios conferidos em 2026-09-30

O código aceito diverge deste contrato nos pontos abaixo. Vale o código; o texto acima fica como registro.

- **§2 e §3.1, confirmação do nascimento.** Vale `destinyId` não vazio, e não `confirmedAtUtc > 0`: um save editado à mão zera o carimbo e reabriria o nascimento (`DestinySystem`, `Scripts/Save/SaveBootstrap.cs`). O carimbo continua gravado.
- **§3.4, save de versão futura.** Não migra nem é descartado: o arquivo (e o `.bak`) fica intacto, o jogo segue com save padrão e a gravação por cima é bloqueada, com log (`Scripts/Save/LocalSave.cs`).
- **§2, "T004 não valida regra de destino".** `LocalSave.Auditar` chama `DestinySystem.Validar` ao carregar, só para registrar nascimento inválido no log; não corrige nem recusa o save.
- **§4, `Scripts/Destiny/Data/`.** Não existe: o catálogo é código (`Scripts/Destiny/DestinyCatalog.cs`).
- **§1, rótulo de `dificil`.** Passou a "Vida Árdua" ([ADR-0007](../adr/ADR-0007-decisoes-da-leva-a.md) §2); o id não muda.
