# Proveniência e licença de arte

- **Por quê:** o portão **G3** do ADR-0002 exige "licença e proveniência registradas" antes de rig, animação e import no Unity. O dossiê §J manda "não presumir que todo resultado gerado por IA possui licença comercial irrestrita". O GDD cap. 09 manda registrar prompt, origem, versão e autorização comercial.
- **Quem lê:** pessoas, e o validador de import (regra **V04** de `docs/arte/PIPELINE.md` §11), que procura o bloco `### <id>` deste arquivo.
- **Estado:** 2026-09-29; conferido em 2026-09-30. Nenhum asset **do jogo** foi gerado em serviço externo. Existe um acervo de referência gerado no ChatGPT, fora do git (§5). Nenhuma conta foi criada e nenhum login foi feito para escrever este documento.

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

**Concepts do acervo** (`arte/referencias/acervo/imagens/`, 235 PNG, mapa em `docs/arte/ACERVO.csv`): gerados com **"GPT astra 6"**, informado pelo idealizador em 2026-09-30, a partir da bíblia de prompts em `documentos/`. Termos de uso **não consultados**, e o produto ainda não foi identificado com URL oficial. Antes de um concept virar `entrada` de gerador 3D (regra 2 abaixo), registrar aqui o `termos_url` e o que ele diz sobre a propriedade da imagem gerada. Imagem gerada pelo próprio Tripo segue a tabela acima: no plano gratuito, o Tripo retém os direitos também sobre ela.

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

## 5. Acervo de referência (não é asset do jogo)

Fatos conferidos em 2026-09-30. Esta seção não é um bloco de registro da §4 e o validador não a lê.

- **O que é:** 235 PNGs gerados no ChatGPT (modelo de imagem da OpenAI; os arquivos trazem manifesto C2PA), em 2026-09-28 e 2026-09-29. Junto vêm um catálogo de 341 fichas de **prompt** (`catalogo_341_fichas.json`; não são fichas G1 do ADR-0002) e uma bíblia de prompts.
- **Onde está:** `arte/referencias/acervo/` (`imagens/`, `documentos/`, `INVENTARIO.csv`, `LEIA-ME.md`, `INDEX.html`), versionado pelo Git LFS desde 2026-10-03 (ADR-0010): copiado da raiz do checkout principal com os 235 SHA-256 conferidos contra o `ACERVO.csv`. O zip de 481 MB era cópia idêntica e foi apagado em 2026-10-03; os originais da raiz ficam até o idealizador decidir apagá-los.
- **Portões:** nenhuma imagem passou por G1 nem por G2.
- **Plano, conta e termos da ferramenta na data da geração:** A REGISTRAR pelo idealizador.
- **Uso permitido:** referência de direção.
- **Uso vetado até o registro acima e o G2:** entrada de geração de malha (Tripo3D ou outro gerador).

**Tripo Bridge** (`client/Packages/com.tripo3d.unitybridge`, v1.0.14): o pacote não traz arquivo LICENSE, e o repositório citado no `package.json` não é público (HTTP 404 em 2026-10-03). Uso coberto pelos termos do Tripo (§2); só Editor, não entra no player (ADR-0010).

## 6. Protótipo de estética (ADR-0008) — estado: PROTOTIPO

Exceção aprovada pelo idealizador em 2026-09-30. **Nenhuma peça abaixo vai para build de loja**: antes, cada uma passa pelo portão do ADR-0002 (G1 → G2 → G3) ou é substituída. Ficam em `client/Assets/_COE/Art/Prototipo/`, fora de `Art/<Categoria>/<id>/`, e o validador (V04) não as lê.

Campos comuns a todas as peças da tabela:
- **Ferramenta:** Tripo Studio (studio.tripo3d.ai), na conta do idealizador, pelo Chrome dele. O plano não é de assinante (privacidade e textura 8K aparecem bloqueadas); pelos termos da §2, o uso é **não comercial** e o modelo pode ficar público no site.
- **Entrada:** imagem gerada **dentro do Tripo** (GPT Image 2) a partir de prompt de texto escrito pelo coordenador com base no GDD, no SLICE e na ficha do Borin. Nenhum arquivo do acervo da §5 foi enviado.
- **Parâmetros:** modelo 3D H3.1 (Modelo HD), 8000 polígonos, triângulos, textura 2K com "remover iluminação" ligado. Os personagens receberam Auto Rig humanoide com esqueleto Mixamo.
- **Exportação:** FBX (predefinição Blender). No projeto entram só o FBX, `*_basecolor` e `*_normal`.
- **Data:** 2026-09-30 (os seis adultos da vila, em 2026-10-01). Prompts dos adultos pedem roupa justa nas pernas, sem saia longa, pelo aviso de rig da Mara.
- **g1 / g2 / g3:** exceção ADR-0008 (não passaram).

| id | categoria | observação |
|---|---|---|
| protagonista | Personagens | criança de 5 anos, cabelo cacheado, colete verde |
| nilo | Personagens | colete musgo, faixa ferrugem |
| sera | Personagens | túnica tijolo, faixa oliva |
| borin | Personagens | avental de couro, aro de provas na cintura (proposta da ficha G1) |
| mara | Personagens | vestido sálvia, avental creme; o Tripo avisou que o vestido longo dificulta o rig |
| daren | Personagens | 2026-10-01; túnica azul-ardósia, cinto com bolsa de ferramentas |
| lysa | Personagens | 2026-10-01; trança ruiva, colete musgo, avental curto, bolsa de ervas |
| tovin | Personagens | 2026-10-01; gibão cinza, ombreira e braçadeiras de couro, sem arma |
| eira | Personagens | 2026-10-01; coque grisalho, óculos, blusa mel e saia mostarda até o joelho |
| oren | Personagens | 2026-10-01; casaco malva curto, bolsa de moedas e caderno no cinto |
| maelis | Personagens | 2026-10-01; túnica rosa-antigo com debrum dourado, faixa ameixa |
| parceiro_treino | Personagens | 2026-10-01; instrutor do treino do B15 (aprendiz da guarda, vinte e poucos anos), gibão acolchoado azul-petróleo, ombreiras e braçadeiras de couro, mãos vazias; sem ficha G1 (ver `fichas/README.md`) |
| casa_familia, ferraria, poco, arvore, barril, caixote, cesto, lanterna, arbusto, simbolo_limiar, bigorna, banco | Pecas | o símbolo usa um "círculo incompleto" genérico, sem validação de cânone |
| ervanaria, posto_guarda, casa_nilo, casa_sera, mural_avisos | Pecas | 2026-10-01; mesmos parâmetros (H3.1, 8000 triângulos, 2K); trocam o greybox em Auren, colisão e interagíveis continuam do greybox |

**Animações (Mixamo)** em `client/Assets/_COE/Art/Prototipo/Animacoes/`: Breathing Idle, Walking, Running (as duas "In Place"), Talking, Waving, Punching, Standing Dodge Backward, Hit Reaction e Dying.
- **Origem:** baixadas do mixamo.com em 2026-09-30, na conta Adobe do idealizador, em FBX "Without Skin", a 30 fps, sem redução de keyframes. Só a animação vem; o personagem padrão do Mixamo não entra no projeto.
- **Termos:** o FAQ oficial (https://helpx.adobe.com/creative-cloud/faq/mixamo-faq.html, consultado em 2026-09-30) diz que personagens e animações podem ser usados "royalty free for personal, commercial, and non-profit projects", incluindo jogos.
- **A conferir** antes da loja: se os termos de uso da Adobe proíbem redistribuir os arquivos soltos. Dentro do jogo compilado, eles não ficam expostos.
- **Uso:** `PrototipoAnimacoes` monta `Prototipo.controller` (cópia do `Player.controller`) com locomoção em três pontos e cadência medida, soco, esquiva, reação e queda. Talking e Waving estão baixadas e ainda sem uso.

## 7. Concepts do G2 (referência, não é asset do jogo)

Lote 1, primeira folha do teste de silhueta (`docs/arte/fichas/README.md`): `borin`, `maelis`, `tovin`, `mara`, `daren`. Gerado pelo coordenador com autorização do idealizador (2026-10-03). Campos comuns:
- ferramenta: Tripo Studio (studio.tripo3d.ai), Imagem, modelo GPT Image 2, na conta do idealizador, pelo Chrome dele
- plano: max (selo "Max" no menu da conta em 2026-10-03; nº da fatura com o idealizador, a preencher)
- data: 2026-10-03
- entrada: n/a (texto só; nenhuma imagem enviada)
- prompt: bloco "Estilo + formato" + bloco do personagem em `docs/arte/PROMPTS_G2.md`, como estavam no commit deste registro (travessões trocados por hífen ao digitar)
- parametros: proporção 1:1, resolução padrão (4K desligado)
- visibilidade: o gerador de imagem não oferece opção de visibilidade (só o de modelo 3D); nada foi publicado
- licenca: tripo_pago
- termos_url: https://www.tripo3d.ai/terms
- termos_versao: Last updated: July 11, 2025 (relido em 2026-10-03, sem mudança)
- termos_consultados_em: 2026-10-03
- g1: 2026-10-03, aprovado por delegação e reconferido (fichas)
- g2: 2026-10-03, `docs/arte/g2/folha1.md`: borin, maelis, tovin e daren aprovados (silhueta 5/5 duas vezes, descrição específica, troco passa); mara reprovada na descrição (v01 e v02)
- g3: n/a (concept não vai para o jogo; vira `entrada` do modelo 3D só depois do G2)

Por id (`task_id` e arquivo preenchidos depois de gerar):
### concept_g2_borin
- categoria: concept (referência G2)
- tentativas: 22:30 e 22:36 (horário do histórico do Tripo; 2 imagens por tentativa)
- task_id: o histórico de imagem do Tripo não mostra id; identificação pelo horário escolhido: 22:36, imagem 2 de 2
- prompt: bloco do `PROMPTS_G2.md` com a linha de reforço "apron only from the belt down" (2ª tentativa), registrada na seção "Reforço do lote 1"
- saida_fonte: `arte/referencias/concepts_g2/borin_frente_v01.png` (1024 x 1024, sha256 196e3ccafdf4f8f204bf92a9db7cb812a70d04b6f26ee1ebafb5cb7f97ffd4dc)

### concept_g2_maelis
- categoria: concept (referência G2)
- tentativas: 22:31, 22:35 e 22:41 (horário do histórico do Tripo; 2 imagens por tentativa)
- task_id: o histórico de imagem do Tripo não mostra id; identificação pelo horário escolhido: 22:41, imagem 1 de 2
- prompt: bloco do `PROMPTS_G2.md` com a linha de reforço com as três formas e o lado da imagem (3ª tentativa), registrada na seção "Reforço do lote 1"
- saida_fonte: `arte/referencias/concepts_g2/maelis_frente_v01.png` (1024 x 1024, sha256 3efb84d393fcfec33d5558449f1bc517de30e315cb06f19989797e0a330b03a9)

### concept_g2_tovin
- categoria: concept (referência G2)
- tentativas: 22:31, 22:35, 22:40 e 22:41 (horário do histórico do Tripo; 2 imagens por tentativa)
- task_id: o histórico de imagem do Tripo não mostra id; identificação pelo horário escolhido: 22:41, imagem 1 de 2
- prompt: bloco do `PROMPTS_G2.md` com a linha de reforço com as três formas e o lado da imagem (4ª tentativa), registrada na seção "Reforço do lote 1"
- saida_fonte: `arte/referencias/concepts_g2/tovin_frente_v01.png` (1024 x 1024, sha256 f47a3b3a1d23b49d5489ea1378f37d9653e4b624aab75e4a6fd7c0fddfb15532)

### concept_g2_mara
- categoria: concept (referência G2)
- tentativas: 22:32 e 22:34 (horário do histórico do Tripo; 2 imagens por tentativa)
- task_id: o histórico de imagem do Tripo não mostra id; identificação pelo horário escolhido: 22:34, imagem 2 de 2
- prompt: bloco do `PROMPTS_G2.md` com a linha de reforço "not a dress, no apron" (2ª tentativa), registrada na seção "Reforço do lote 1"
- saida_fonte: `arte/referencias/concepts_g2/mara_frente_v01.png` (1024 x 1024, sha256 637211faee59ca01901a6421eab4ef404c4e4d97aefba499d7b1b7b5e6fef2d7)
- derivado: `arte/referencias/concepts_g2/mara_frente_v01_espelhado.png` (espelhado na horizontal: a barra comprida saiu do lado esquerdo dela; a Arbitragem 3 pede o direito) (sha256 4e86a7528e8587328aa555f23ba39b8b5e1fdb3296bde88765da0a68db1ab1bb)

### concept_g2_daren
- categoria: concept (referência G2)
- tentativas: 22:32 e 22:35 (horário do histórico do Tripo; 2 imagens por tentativa)
- task_id: o histórico de imagem do Tripo não mostra id; identificação pelo horário escolhido: 22:35, imagem 2 de 2
- prompt: bloco do `PROMPTS_G2.md` com a linha de reforço com os lados da caixa e do estojo (2ª tentativa), registrada na seção "Reforço do lote 1"
- saida_fonte: `arte/referencias/concepts_g2/daren_frente_v01.png` (1024 x 1024, sha256 a26a7ca62ac1e1740bee46dd9479c8a03ef960963caed5a96b3d20a0944e8bf1)
### concept_g2_mara_v02
- categoria: concept (referência G2), refeito porque o v01 reprovou no teste de descrição ("genérico": o rolo da manta sem uso visível)
- tentativas: 22:50 (2 imagens)
- task_id: sem id no histórico de imagem; identificação: 22:50, imagem 2 de 2
- prompt: bloco da Mara no `PROMPTS_G2.md` com a linha de reforço v02 (barra rasgada em tiras e uma tira arrancada pendurada da mão), registrada na seção "Reforço do lote 1"
- saida_fonte: `arte/referencias/concepts_g2/mara_frente_v02.png` (1024 x 1024, sha256 7c2e26335a91e1ab32d6ab52b1a5054902f5bd50e3c948498c0f56eb5e64d16c); não precisou espelhar
- obs: o coque aparece no lugar do pano justo na cabeça (C3 da ficha); corrigir na malha
