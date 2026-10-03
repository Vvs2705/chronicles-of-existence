# Fichas G1

- Aqui ficam as fichas do portão **G1** do [ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md): uma por personagem, local, item-assinatura, VFX ou símbolo, em `<id>.md`, a partir de [`_MODELO_G1.md`](_MODELO_G1.md).
- Ordem, sem atalho: **G1** ficha aprovada → **G2** concept com testes cegos → **G3** licença e proveniência em [`../PROVENIENCIA.md`](../PROVENIENCIA.md). Sem G1 aprovado não se encomenda concept; sem G2 não se gera malha.
- Quem escreve a ficha (Concept Art Lead ou quem propõe a peça) **não dá a nota**. A nota é do Art Director, em outra sessão. Corte: ≥ 14/20 e nenhum zero em C2–C5. Enquanto o idealizador delegar, a aprovação vale por delegação ([ADR-0010](../../adr/ADR-0010-arte-por-delegacao.md)).
- [`ELENCO.md`](ELENCO.md): mapa de silhuetas do elenco e quem ficou com cada zona disputada.
- Fato canônico leva a fonte; o que a ficha inventa leva a marca **PROPOSTA** e não muda o GDD sozinho.
- Escritas: [`borin`](borin.md) (piloto), [`mara`](mara.md), [`daren`](daren.md), [`lysa`](lysa.md), [`tovin`](tovin.md), [`eira`](eira.md), [`nilo`](nilo.md), [`sera`](sera.md), [`oren`](oren.md), [`maelis`](maelis.md), [`avatar`](avatar.md) (bases de 5 e 8 anos), [`aethron`](aethron.md), [`simbolo_limiar`](simbolo_limiar.md). O estado de cada uma está na própria ficha (§1); o resumo abaixo é de 2026-10-03.

| ficha | G1 | conferência final (Art Director, 2026-10-03) | depois da W3 (2026-10-03) | reconferência (Art Director, leva C1, 2026-10-03) |
|---|---|---|---|---|
| `borin` | aprovado por delegação (ADR-0010 §1) | pendente: 5 itens | corrigida | atendida, sem pendência |
| `daren` | aprovado por delegação | pendente: (a), tiras e vara | corrigida | atendida, sem pendência |
| `nilo` | aprovado por delegação | pendente: (a), folga da forquilha | corrigida | atendida, sem pendência (folga de 4,5 cm refeita) |
| `lysa` | aprovado por delegação | pendente: (b), dono da q05 | (b) decidida (ADR-0010, adendo, item 10) | atendida, sem pendência; forma não muda |
| `maelis` | aprovado por delegação | pendente: (b), dono da q07 | (b) decidida (ADR-0010, adendo, item 11) | atendida, sem pendência; forma não muda |
| `tovin` | aprovado por delegação | atendida, com ressalvas de texto | estojo na canela esquerda (`ELENCO.md`, Arbitragem 3); forquilha a ~6 m do símbolo | Arbitragem 3 coerente, sem colisão nova |
| `mara` | aprovado por delegação | atendida, com ressalva de G2 (canela) | canela direita dela (Arbitragem 3) | Arbitragem 3 coerente |
| `avatar`, `eira`, `oren`, `sera` | aprovado por delegação | atendida, com ressalvas de texto | texto corrigido | resolvidas, sem mudar forma nem medida |
| `aethron`, `simbolo_limiar` | aprovado por delegação | atendida | sem mudança | não reconferidas (sem mudança) |

Cada ficha traz, no fim da §5, a linha "Correções de 2026-10-03 (W3)" e, depois dela, a "Reconferência (Art Director, 2026-10-03, leva C1)". **As 13 fichas estão aprovadas no G1 por delegação, sem pendência de G1.** O campo "estado" do §1 de `borin`, `daren`, `nilo`, `lysa` e `maelis` ainda diz "a reconferir". Até o autor atualizar esse campo, vale a reconferência.
- **Antes de gerar os concepts do G2:** o `silhueta.py` apaga o que tem menos de 7 px numa máscara de 512 px de altura, e o bloco "Estilo + formato" do [`PROMPTS_G2.md`](../PROMPTS_G2.md) não fixa a proporção da imagem. Gerar em 1:1, ou com a imagem deitada, e conferir a fração da altura da imagem que a figura ocupa. O mínimo é ≥ 62% para o Borin (chapa de 40 mm), ≥ 57% para a Maelis (vara de 4 cm) e ≥ 50% para o Daren e o Nilo. Numa imagem 2:3 em pé, em T-pose, o Borin fica com ~58% e o aro some.
- **Primeira folha do G2 recomendada:** `borin`, `maelis`, `tovin`, `mara`, `daren`. São as vizinhanças que as arbitragens mandaram para o G2 (Arbitragem 2, item 3; Arbitragem 3), e o teste do ADR-0002 pede ≥ 4 acertos em 5.
- Falta escrever, no elenco do slice: o parceiro de treino do B15 e a criatura da q05.
- O ADR também lista como alvo as três casas, as três estruturas públicas e o VFX das magias iniciais: entram depois do elenco.
- O teste de silhueta do G2 pede pelo menos 4 silhuetas do mesmo elenco: nenhuma ficha chega ao G2 sozinha.
