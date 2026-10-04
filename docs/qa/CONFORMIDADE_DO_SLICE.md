# Conformidade do slice: o que a documentação promete e o que o jogo entrega

**Conferido em 2026-10-04**, por auditoria independente só de leitura. Fontes: GDD v1.2 (Apêndice B e caps. 05, 07 e 09), `docs/design/SLICE_A_PRIMEIRA_EXISTENCIA.md` (B01–B16, §4, §5), backlog v1.1 e ADRs. Cada item foi conferido no código e no gerador de cena, ou seja, no que de fato aparece ou age para quem joga. Ficaram de fora o que já está fora do slice, o que é arte do G3 e o que a dívida já registrava.

**Resultado:** 46 itens (16 beats e 30 de UI/sistema), 34 entregues e 12 lacunas. Antes desta auditoria também faltavam as barras de Vida/Vigor/Mana (GDD cap. 05), entregues no mesmo dia (PR #25).

## Corrigido em 2026-10-04

| Lacuna | Fonte | O que mudou |
|---|---|---|
| Tovin nunca citava a busca depois do salto: o nó dos 8 vinha antes e sempre vencia | SLICE §4.3, "tem de ser lembrado" | Nó `aos_oito_depois_da_busca`: a lembrança da busca e o convite ao treino na mesma fala (a Q-07 é central, então aos 8 ele sempre lembra) |
| Daren não lembrava o primeiro dia | SLICE §4.3 | Nó `aos_oito_primeiro_dia`, por `evento.q01_concluida`, que ele testemunhou |
| A mudança visível de Auren (a bancada na porta) não era comentada por ninguém | SLICE §4.3 | O Borin comenta a bancada nas duas falas dos 8; o segredo da Q-06 só é insinuado para quem o conhece |
| A confirmação do salto ficava no próprio aviso (um botão só) | SLICE B13: "dois passos, nunca um botão só" | Segundo passo "Tem certeza?" (Voltar / Sim, crescer); o robô e o teste passam pelos dois |
| ATQ, FORTE, DEF e MAGIA apareciam aos 5 e não faziam nada | GDD cap. 05 (infância sem combate) | Somem aos 5; o toque no lugar deles gira a câmera; ESQ e USAR ficam |
| Portas, poço e mural: texto fora do arquivo de textos, sem acento, e USAR sem efeito | SLICE B09: "o mural mostra o aviso" | Texto pelo arquivo de textos; cada um diz uma linha na faixa de aviso; o mural mostra o aviso da Maelis depois do sumiço de Nilo |

## Fica aberto

| Lacuna | Fonte | Por que não foi feito agora |
|---|---|---|
| Os itens ganhos não aparecem em lugar nenhum (só as moedas, no cartão) | SLICE R10/R11, §4.1 | Precisa de desenho de tela: o painel do menu não comporta outra linha em 360 dp com notch, e o cartão é do objetivo. Item da T013 |
| A espada existe só na fala do Borin: não há item, nem entrega da espada comum no treino, nem objeto na mão | SLICE B15, ADR-0010 §3, GDD cap. 05 | A entrega como item é técnica; o objeto na mão é arte (prop preso a osso, regra dos objetos separados, G3). Melhor fazer junto |
| Áudio: falta o ambiente de Auren e o motivo do Limiar nos momentos-chave (B11, B16) | GDD cap. 09 | Desenho de som (protótipo pelo `Sintese` ou trilha da T013) |
| O chifre do Tovin (recolher, silêncio depois do sumiço, toque no fim do treino) | ficha `tovin.md`, `ELENCO.md` | Som e prop são arte; a regra (pelo histórico) é técnica e espera o som |
| A origem quase não muda Mara e Daren (texto e casa) | SLICE B03, GDD | Redação de 3 variantes por personagem (conteúdo) e objetos da casa (arte) |
| A criança acorda na rua, não dentro de casa | SLICE B06 | A câmera dentro da casa ficou para a T013 (`AurenSceneBuilder`) |
| Assistências de combate (o ADR-0004 pede teste que as muda no meio da sessão) | ADR-0004 invariante 3, SLICE R1 | `BLOCKED_PRODUCT_DECISION`: declarar fora do slice ou criar um mínimo |
| Prenúncio narrativo de ascensão ("Incluir" no slice) | GDD Apêndice | `BLOCKED_PRODUCT_DECISION`: não há beat nem texto previstos |
| Q-03 sem as "variantes modulares" | GDD | `BLOCKED_PRODUCT_DECISION`: o que varia, e por quê |

## Observações (não são lacunas, mas pedem decisão)

- **"Nova vida"** no título apaga o save e refaz o nascimento. Não reabre o exploit (é outra vida), mas contraria a letra do B05/R1 ("nenhuma tela oferece trocar"). Registrar a decisão.
- **Moedas** não têm onde ser gastas.
- **Reputação** é inerte: só a Q-04 a muda, e nenhuma fala a lê. Nada promete mostrá-la.
