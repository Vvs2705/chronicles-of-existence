# Proveniência e licença de arte

- **Por quê:** o portão **G3** do ADR-0002 exige "licença e proveniência registradas" antes de rig, animação e import no Unity. O dossiê §J manda "não presumir que todo resultado gerado por IA possui licença comercial irrestrita". O GDD cap. 09 manda registrar prompt, origem, versão e autorização comercial.
- **Quem lê:** pessoas, e o validador de import (regra **V04** de `docs/arte/PIPELINE.md` §11), que procura o bloco `### <id>` deste arquivo.
- **Estado:** 2026-09-29. Nenhum asset foi gerado em serviço externo. Nenhuma conta foi criada e nenhum login foi feito para escrever este documento.

## 1. Campos por asset

Um bloco por asset, na seção 4. Formato fixo para o validador: título `### <id>` e uma linha `- chave: valor` por campo. Data sempre `AAAA-MM-DD`.

| Chave | Obrigatório | Conteúdo |
|---|---|---|
| `categoria` | sim | `Avatar`, `Npc`, `Prop`, `Estrutura`, `Vfx` ou `placeholder` |
| `ferramenta` | sim | nome e versão de cada ferramenta da cadeia (ex.: `Tripo3D Studio <modelo mostrado na UI>; Blender 5.2.1 LTS`) |
| `plano` | sim | plano **ativo na data da geração**: `free`, `pro`, `max`, `team`, `api` ou `n/a` |
| `comprovante_plano` | se `plano` pago | nº do pedido ou da fatura. Nunca dado de cartão |
| `data` | sim | data da geração (ou da modelagem, se manual) |
| `entrada` | sim | caminho do concept usado (`arte/referencias/...`) + SHA-256 do arquivo; `n/a` se modelado do zero |
| `prompt` | se houve | texto exato enviado; `n/a` se não houve |
| `parametros` | se houve | seed, limite de faces, textura, estilo, qualquer opção marcada |
| `task_id` | se houve | id da tarefa no fornecedor |
| `saida_fonte` | se houve | `arte/fonte/<Categoria>/<id>/<arquivo>` + SHA-256, baixado no dia da geração |
| `visibilidade` | se Tripo3D | `privada` ou `publica` no serviço |
| `autor` | sim | quem operou o gerador e quem modelou/retopologizou/riggou no Blender |
| `licenca` | sim | `propria`, `tripo_pago`, `cc0`, `cc_by_4_0`, `outra`, `tripo_free` ou `desconhecida` (os dois últimos reprovam V04) |
| `atribuicao` | se `cc_by_4_0` | texto de crédito exigido |
| `parecer` | se `outra` | referência do parecer que autorizou |
| `termos_url` | sim | URL dos termos que valiam na data |
| `termos_versao` | se houver | o "Last updated" dos termos na data |
| `termos_consultados_em` | sim | data em que alguém leu os termos |
| `referencias_terceiros` | se houve | referência de terceiro usada **só como inspiração** (nunca enviada a gerador) |
| `g1` | sim | data + nota da ficha (ex.: `2026-10-02, 16/20, pontuado por X`) ou `n/a` |
| `g2` | sim | data + resultado dos testes cegos (ex.: `2026-10-05, silhueta 5/5, descricao ok`) ou `n/a` |
| `g3` | sim | data em que este registro foi conferido completo, ou `n/a` |
| `obs` | não | qualquer coisa que um auditor precise saber |

## 2. Tripo3D: o que os termos dizem

Consultado em **2026-09-29**, só leitura, em navegador (a busca automática recebe HTTP 403 nas páginas oficiais):

- Termos: <https://www.tripo3d.ai/terms>, "Terms of User Agreement", **Last updated: July 11, 2025**. Contratante: Holymolly Ltd (Hong Kong). Lei de Hong Kong, arbitragem na HKIAC (§11).
- Preços: <https://www.tripo3d.ai/pricing>.

| Tema | Plano gratuito | Plano pago (Pro, Max, Team) | Fonte |
|---|---|---|---|
| Direitos sobre o output | **Tripo retém todos os direitos** sobre Inputs e Outputs de "Free Users", inclusive licenciar e obter receita | usuário pago "generally ha[s] all rights" sobre Inputs e Outputs, condicionado ao cumprimento do acordo | termos §5.2.1 e §5.2.2 |
| Licença de volta ao Tripo | — (Tripo já retém tudo) | licença royalty-free, perpétua, irrevogável, mundial e não exclusiva para a empresa usar e exibir "as may be necessary for Service provision" | §5.2.2 |
| Uso comercial (página de preço) | "Public Models · Non-Commercial Use" | "Private Models · Commercial Use" | pricing |
| Treino de IA com seus dados | sem promessa | a empresa não usa Inputs e Outputs como dado de treino | §5.2.2 |
| Exclusividade | nenhuma | nenhuma: outros usuários podem receber outputs "similar or identical" | §3.2 |
| Garantia de não infração | nenhuma | nenhuma | §3.2 e §4 |
| Responsabilidade pela entrada | o usuário é "solely responsible"; proibido input que infrinja direito de terceiro; o usuário indeniza a empresa | idem | §3.1 e §9 |
| Visibilidade padrão | se o usuário não escolher, "the system may default to its most permissive setting"; output em área pública pode ser retido "in perpetuity" | idem | §4 e §10.5 |
| Créditos/preço na data | 200 créditos/mês, US$ 0 | Pro: 3 000 créditos/mês, US$ 20/mês na cobrança anual (US$ 240/ano); Max: US$ 90/mês anual; Team: US$ 55/mês/assento anual | pricing |

**Incerto — não presumir:**

1. **Conflito interno nos termos.** §3.2 diz, sem distinguir plano, que o usuário "may use Outputs for lawful commercial or non-commercial purposes"; §5.2.1 diz que o Tripo retém todos os direitos do plano gratuito, e a página de preço diz "Non-Commercial Use". Leitura do COE: **plano gratuito não serve para asset do jogo**.
2. **Cancelamento.** Os termos não dizem se o direito sobre um output gerado com plano pago sobrevive ao fim da assinatura. Mitigação: `plano` e `comprovante_plano` da data da geração; saída baixada no mesmo dia.
3. **"Generally have all rights … subject to".** Não é cessão exclusiva, não há garantia de não infração e não há exclusividade. Passar no G1–G3 não é parecer de propriedade intelectual (ADR-0002, Consequências). Revisão jurídica antes de release comercial.
4. **API.** `platform.tripo3d.ai` pode ter termos suplementares ("Supplemental Terms", §1). Não consultados: a página não expôs texto. Consultar antes de usar a API.
5. **Fontes de terceiros divergem.** Sites de terceiros (ex.: videosdk.live, 3daistudio.com, busca de 2026-09-29) dizem que o plano gratuito sai em **CC BY 4.0**. Os termos oficiais consultados não dizem isso. Vale o termo oficial.
6. **Proteção autoral de output de IA** varia por país. Fora do escopo técnico; os campos `autor` e `ferramenta` existem para registrar a contribuição humana (retopologia, UV, rig, textura).

**Blender:** "What you create with Blender is your sole property." (<https://www.blender.org/about/license/>, consultado em 2026-09-29). A GPL cobre o programa, não o que se cria com ele.

**Mixamo** (citado no `HumanoidSetup` como fonte de clip): termos não consultados nesta rodada. Consultar e registrar antes do primeiro clip Mixamo entrar.

## 3. Regras do COE para gerador externo

1. Só plano **pago e ativo**, com o modelo marcado **privado antes de gerar**. `tripo_free` reprova V04.
2. Entrada só concept **próprio**, aprovado em G2. Nunca imagem de terceiro, foto de pessoa real, marca ou captura de outro jogo (§3.1 dos termos põe a responsabilidade no usuário).
3. Bloco deste arquivo aberto **antes** de gerar (id, plano, comprovante, entrada, prompt) e completado depois (`task_id`, `saida_fonte` com SHA-256).
4. Saída baixada no dia para `arte/fonte/<Categoria>/<id>/`, que é imutável. Correção vira arquivo novo.
5. Prompt sem dado pessoal, sem segredo, sem nome de obra de referência (ADR-0002).
6. Output de gerador nunca vai direto para o Unity: passa por Blender (retopologia, UV, rig, bake) e pelo validador.
7. Termos mudam. A cada lote, reler `termos_url`; se o "Last updated" mudou, registrar em `termos_versao` e reavaliar esta seção.

## 4. Registro

### placeholder_humanoid
- categoria: placeholder
- ferramenta: Blender 5.2.1 LTS (hash 9e2066aef7ef), headless, script `client/tools/placeholder_humanoid.py`
- plano: n/a
- data: 2026-09-29
- entrada: n/a (geometria primitiva gerada por código; tabela de ossos no próprio script)
- prompt: n/a
- saida_fonte: `client/Assets/_COE/Art/Humanoid/` (Model, Idle, Run, Attack1, Attack2, Attack3, Dodge, Hit, Death .fbx); o gerador versionado no git é a fonte
- autor: script do repositório (raia L3 da missão de 2026-09-29, sob orientação do Vinicius)
- licenca: propria
- termos_url: https://www.blender.org/about/license/
- termos_consultados_em: 2026-09-29
- g1: n/a
- g2: n/a
- g3: n/a
- obs: boneco técnico de 1,10 m (criança de 5 anos) para jogabilidade e medição. Não é arte do COE e não passa pelos portões do ADR-0002. O validador roda nele só as regras [P] (PIPELINE.md §11), sem V04.
