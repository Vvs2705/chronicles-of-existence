# Ficha G1 — `<id>`

> Modelo do portão G1 do [ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md). Copie para `docs/arte/fichas/<id>.md` e preencha em ~1 página.
> A régua 0/1/2 abaixo é cópia do ADR; se o ADR mudar, este modelo muda junto.
> Todo fato leva a fonte (`doc §seção`) ou a marca **PROPOSTA** (ideia desta ficha, ainda não aprovada). Se uma resposta serve para qualquer jogo do gênero, reescreva.

## 1. Identificação

- **id:** `<snake_case>` — NPC usa o id do `NpcCatalog`; avatar e estrutura, os de `PIPELINE.md` §6
- **categoria:** Avatar | Npc | Prop | Estrutura | Vfx | Simbolo
- **onde aparece no slice:** beats (`SLICE_A_PRIMEIRA_EXISTENCIA.md` §2), missões, âncoras
- **cânone de partida:** o que os docs já dizem, com fonte
- **autor da ficha / data:** quem escreveu (não pontua)
- **estado:** rascunho | aguardando nota | aprovado G1 | reprovado | refazer

## 2. Critérios C1–C10

Responda embaixo de cada pergunta. Nota 2 exige o que a coluna "2" descreve, escrito aqui, não prometido.

| # | Pergunta | 0 | 1 | 2 |
|---|---|---|---|---|
| C1 | **Gancho em uma frase**, com detalhe concreto e tensão | não existe | descreve função ou cargo | detalhe concreto + conflito |
| C2 | **Necessidade do mundo** que o originou: que condição de Eryndor/Valtheris/Eldoria/Auren o fez existir? | nenhuma | genérica, serviria em qualquer RPG | específica de Eryndor/Auren |
| C3 | **Silhueta própria**: reconhecível em preto a 30% de escala | nada | lista de peças de roupa | 3 formas dominantes nomeadas |
| C4 | **Objeto-assinatura com regra**: um objeto que só ele tem e que faz algo no jogo | nenhum | objeto decorativo | objeto + regra de jogo escrita |
| C5 | **Regra ou habilidade exclusiva**: algo que só ele faz e o jogador vê | nada | kit genérico do catálogo | técnica exclusiva especificada |
| C6 | **Voz**: 3 falas que só ele diria (ritmo, vocabulário, tique) | nenhuma | tom descrito em adjetivos | 3 falas escritas |
| C7 | **Contradição visível em ação recorrente** | nenhuma | abstrata na ficha | observável no comportamento |
| C8 | **Relação concreta com o avatar** que muda por destino/origem | nenhuma | uma linha genérica | concreta e ramificada pelos 4 destinos |
| C9 | **Mudança no tempo**: como muda no salto dos 5 para os 8 anos | nenhuma | "vai mudar" | 2 batidas marcadas, antes e depois |
| C10 | **Momento de cartaz**: a cena que o vende em 5 segundos | nenhuma | cena implícita | cena escrita e encenável |

**C1 —**
**C2 —**
**C3 —** (as 3 formas, com nome; dizer se sobrevivem à T-pose e à câmera do jogo)
**C4 —** (objeto, medida em metros, regra: o que o jogo lê e o que o jogador vê)
**C5 —**
**C6 —** 1. "…" 2. "…" 3. "…"
**C7 —**
**C8 —** serena: … · normal: … · dificil: … · ruptura: … (origem, se mudar algo: …)
**C9 —** antes (5–7): … · depois (8): … · pede variante de malha? sim/não
**C10 —**

Avatar: C6 e C7 não se aplicam; C4, C5 e C8 são obrigatórios em nota 2 (ADR-0002, nota do avatar). Outro N/A: justificar e deixar o pontuador decidir (o ADR não define).

## 3. Amarração (ADR-0002, perguntas 3 a 5 — sem nota, mas obrigatórias)

- **Destino, Grau ou Trama:** o que muda nesta peça entre Vida Serena e Vida da Ruptura, ou entre Grau I e II
- **Decisão de jogo que ele cria:** a escolha que o jogador não faria sem ele
- **Pilar (um só):** viver e crescer | escolher e transformar | superar a própria existência

## 4. O que NÃO é → o que é

Cada linha é um par: o default do gerador do qual se afastar **e** a regra positiva que entra no lugar (regra de encomenda do ADR). Descreva o default por traços, sem citar obra, personagem ou artista de terceiros.

| Não é | É |
|---|---|
| | |

## 5. Avaliação — preenchida por quem NÃO escreveu a ficha

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total /20 |
|---|---|---|---|---|---|---|---|---|---|---|
| | | | | | | | | | | |

- **Corte:** ≥ 14/20 e nenhum zero em C2, C3, C4, C5 (HIPÓTESE do ADR, a recalibrar)
- **Pontuador / data:**
- **Veredito:** aprovado | reprovado | refazer (critérios a reescrever: …)
- **Observações do pontuador:**

## 6. Encaminhamento

**Para o G2 (concept), só depois de aprovado:**
- vistas: frente, perfil, costas, 3/4 verdadeiros; T-pose; fundo neutro; linha de chão; altura-alvo em metros (`PIPELINE.md` §3)
- silhueta: as 3 formas de C3 em preto a 30%, embaralhada com ≥ 3 do elenco; e um render na câmera do jogo
- paleta: blocos de cor com os hex do GDD cap. 09, e os que ficam reservados
- objeto-assinatura (C4) desenhado à parte, com medida
- orçamento que o desenho precisa caber (`PIPELINE.md` §4)
- testes a registrar: silhueta (≥ 4/5), descrição, troco

**Para o G3 (`PROVENIENCIA.md`, bloco `### <id>`):** `g1:` data + nota + pontuador · `g2:` data + resultado dos testes · `entrada:` caminho do concept + SHA-256 · `ferramenta`, `plano`, `licenca`, `termos_url` · `referencias_terceiros` se houve · `g3:` data da conferência.
