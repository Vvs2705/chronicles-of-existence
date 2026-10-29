# Chronicles of Existence: monetização (2026-10-29)

**Modelo:** episódio 1 grátis e completo + **uma compra única** que libera o episódio 2 ([`CONTRATO_V1.md`](CONTRATO_V1.md) §2, ainda PROPOSTA até o idealizador fechar o tamanho da v1). É o modelo de Grimvalor e Oceanhorn ([`COMPETITIVO.md`](COMPETITIVO.md), "Modelo de negócio"). **Sem anúncio, sem moeda premium, sem energia e sem nada aleatório pago.** Molde de código e de documento: `docs/MONETIZACAO.md` do Dungeon Courier, sem a parte de anúncio.

**Estado:** a compra está no código e testada com a loja falsa (PR #41), mas a oferta está **desligada** (`Compras.OfertaLigada = false`) porque o episódio 2 ainda não existe. Para vender falta o que está no §5.

## 1. O produto

| Id na loja | Tipo | O que libera | Preço |
|---|---|---|---|
| `coe.episodio2` | não consumível | o episódio 2, "Despertar dos Talentos" (8 → 11 anos; [`design/EPISODIO2_DESPERTAR_DOS_TALENTOS.md`](design/EPISODIO2_DESPERTAR_DOS_TALENTOS.md)) | **o idealizador decide.** Referência: abaixo dos US$ 6,99 do Grimvalor, com preço regional no Brasil. Sugestão para a conversa: US$ 4,99 e R$ 14,90, validados pela H7 no Limited Release |

**Depois de cadastrado na Play Console, o id nunca muda** (`Compras.Episodio2`). Produto novo entra no fim de `Compras.Catalogo`. Não há segundo produto na v1: nem pacote, nem cosmético, nem "apoie o desenvolvedor".

## 2. Como está ligado

| Peça | Versão | Onde |
|---|---|---|
| Unity IAP (`com.unity.purchasing`) | 5.4.3 (a mesma do DC; a 5.4.4 puxa o `com.unity.services.deployment`, só de Editor) | `client/Packages/manifest.json`; traz o `com.unity.services.core` 1.18.0 e o Google Play Billing no Gradle |
| Regra | C# puro, sem SDK | `Scripts/Loja/Compras.cs` (`Conceder`, `Tem`, `MostrarOferta`), testada em `ComprasTests` |
| Ponte com a loja | Unity IAP 5 | `Scripts/Loja/LojaIap.cs`, testada em `LojaIapTests` (PlayMode, loja falsa) |
| Direito | save v3, bloco `compras` (`CompraData.direitos`) | a vida nova (`SaveState.NovaVida`) leva o bloco: a compra é da conta, não da vida. Migração v2 → v3 testada (`Migracao_V2_ParaV3_ComprasVazias_RestoIgual`) |
| Loja falsa | define `IAP_FAKE_STORE_DEFAULT` | `Editor/ProjectSetup.cs`; PC e Editor compram de mentira, sem dinheiro |

**Fluxo em 2 passos:** COMPRAR → `PurchaseProduct` → a loja cobra → `OnPurchasePending` → `GameSession.ConcederCompra` grava o direito no save → **só então** `ConfirmPurchase`. Se o jogo não gravou, a loja não confirma e o Google Play devolve o dinheiro sozinho em 3 dias. `Conceder` é idempotente: pendente repetido, restaurar e reinstalar não duplicam nada (regra de recompensa idempotente do CLAUDE.md).

- **Ao abrir:** `FetchPurchases`; quem reinstalou ou trocou de aparelho recebe o episódio 2 de volta.
- **Restaurar compras:** botão no painel da oferta do menu de pausa (`MenuDePausa`). No Google Play é o `RestoreTransactions`; na loja falsa, o `FetchPurchases`.
- **Compra adiada** (aprovação dos pais pelo Family Link, pagamento pendente): a janela fecha sem liberar e a compra chega depois como pendente, pelo mesmo caminho. Falta um aviso próprio ("esperando aprovação"); hoje aparece o aviso genérico de compra que não deu certo (`loja.falhou`).

## 3. Guardrails (Prompt Mestre do estúdio §7, ECA Digital e ADR-0009)

Estas regras valem para qualquer mudança futura. Mudar uma delas é decisão de produto: vira proposta em `docs/`, nunca commit direto.

1. **Nada aleatório pago.** Nenhum baú, sorteio, gacha ou "caixa" comprável, nem indireto (moeda que compra sorteio). Lei 15.211/2025 (ECA Digital): o conteúdo de qualquer compra é fixo e listado antes de pagar.
2. **Sem moeda premium e sem energia.** O jogo não vende tempo, vida, tentativa nem atalho de progresso. Comprar não muda dificuldade nem destino (ADR-0004: destino não é dificuldade).
3. **Sem anúncio na v1.** Público misto com crianças (ADR-0009): anúncio traz a carga de SDK certificado para Famílias, perfilamento e `AD_ID`. Se um dia entrar (só pelo fallback do `COMPETITIVO.md`, depois de testar premium), entra como no DC: não personalizado para todos, sem `AD_ID` no manifesto, interstitial atrás de flag **desligada** por padrão, nunca no meio de cena, e a política de privacidade muda **antes** do build que o traz.
4. **A oferta não interrompe a história.** Só no menu de pausa e depois do gancho do episódio 1 (B16). Nunca no meio de cena, de conversa ou de luta (`Compras.MostrarOferta`, testada).
5. **Sem pressão.** Sem contador de "oferta acaba em", sem desconto falso, sem lembrete repetido, sem botão de fechar escondido. Fechar a oferta é tão fácil quanto comprar.
6. **Restaurar sempre funciona** e nunca cobra de novo (queixa nº 1 do BitLife no `COMPETITIVO.md`).
7. **IA generativa nunca concede compra** (CLAUDE.md). O direito só nasce do recibo da loja passando por `Compras.Conceder`.
8. **Controles parentais do Google Play valem:** a compra passa pelo Family Link; o jogo não tem pagamento próprio.

## 4. Privacidade e manifesto

| Permissão | Esperado no AAB | De onde vem |
|---|---|---|
| `android.permission.INTERNET` | sim | Unity (telemetria de hardware, `tech/DIVIDA_TECNICA.md` P2) e o Billing |
| `com.android.vending.BILLING` | sim | Google Play Billing (Unity IAP) |
| `android.permission.ACCESS_NETWORK_STATE` | provável | telemetria do Billing 9 (`datatransport`), como no DC; permissão normal, sem dado do jogador |
| `com.google.android.gms.permission.AD_ID` | **não** | nenhum SDK de anúncio. Se aparecer, sai pelo mesmo caminho do `Setup.SemIdDePublicidade` do DC |

**A conferir no primeiro AAB:** `aapt dump permissions` e o relatório de merge do Gradle, como o DC fez (`docs/MONETIZACAO.md` §1.3 dele). O resultado entra nesta tabela.

**Seção de segurança de dados da Play (rascunho, conferir com o AAB):** o jogo não coleta dado pessoal nem compartilha dado com terceiro para publicidade. Histórico de compras: tratado pelo Google Play, não pelo jogo. Diagnóstico do app (estatística técnica anônima da Unity): declarar como coletado, não compartilhado, não ligado à identidade, sem finalidade de publicidade. Dados criptografados em trânsito: sim (HTTPS da Unity e do Google). Pedido de exclusão: não se aplica (nada guardado por nós). O texto público está em [`privacidade/POLITICA.md`](privacidade/POLITICA.md).

## 5. O que falta para vender

| Item | Estado |
|---|---|
| Episódio 2 jogável | em produção (q09–q16 em dados; `design/EPISODIO2_DESPERTAR_DOS_TALENTOS.md`) |
| Validação de recibo | falta (`ponytail:` no `LojaIap.cs`): obrigatória antes de `OfertaLigada = true` |
| Produto `coe.episodio2` na Play Console | espera a conta de organização (D-U-N-S, previsão 2026-11-25) |
| Preço | decisão do idealizador (§1) |
| Ligar a oferta | `Compras.OfertaLigada = true` só com os três itens acima prontos |
| Medir a H7 (≥ 2% de quem termina o ep. 1) | Limited Release |
