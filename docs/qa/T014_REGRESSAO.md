# T014 — Regressão do slice: matriz R1–R18

Base: `docs/design/SLICE_A_PRIMEIRA_EXISTENCIA.md` §5. Para cada passo, o que já é verificado por **teste automático** (roda em batch, falha sozinho se o comportamento quebrar) e o que só se confere **em tela**, numa build.

Estado em **2026-10-01**: EditMode 498/498, PlayMode 27/27. "Tela" = passo manual, ainda não executado como roteiro completo.

## Os oito obrigatórios do backlog

| # | Automático (nome do teste) | Só em tela |
|---|---|---|
| R1 destino permanente | `Obrigatorio1_DestinoImutavelAposConfirmacao`, `Obrigatorio1_NenhumMetodoPublicoAlteraEscolhaConfirmada`, `Obrigatorio1_ZerarCarimboNoSave_NaoReabreNascimento`, `Confirmar_DepoisDeConfirmado_*` | percorrer pausa e configurações e não achar troca de destino/origem |
| R2 12 combinações | `Obrigatorio2_DozeCombinacoesCarregam`, `Obrigatorio2_DozeCombinacoesTemDadosCompletos`, `Destinos_EOrigens_TemTextoEscrito`, `OrigensDisponiveis_DaAsTresOrigens_NosQuatroDestinos` | entrar em Auren com as 12 e ler família e textos (coerência, sem placeholder) |
| R3 recompensa não duplica | `Obrigatorio3_ConcluirDuasVezes_NaoConcedeNemRegistraDuasVezes`, `Obrigatorio3_SalvarECarregarNoMeio_PreservaProgressoENaoDuplica`, `Obrigatorio3_StatusRevertidoAMao_AindaAssimNaoPagaDeNovo` | moedas no HUD antes/depois de reentregar a q02 |
| R4 promessa lembrada | `Obrigatorio3_DesfechoDaPromessa_ExatamenteUmEIdempotente`, `B08_SeraENilo_FalamDiferente_ConformeAPromessa`, `Lembra_SoCitaEventoQueONpcTestemunha` | falar com Sera depois de fechar e reabrir o jogo |
| R5 salto idempotente | `Obrigatorio5_*` (7 testes, inclusive `InterromperAntesDoCommit_RecarregaAntesDoSalto_ESaltaUmaVezSo`), `ConfirmarSalto_*`; gravação atômica no `LocalSave` | **matar o processo** durante a transição e reabrir |
| R6 slice sem opcionais | **`SliceInteiroTests.R6_DoNascimentoAoGancho_SemNenhumaOpcional_EmCadaDestinoEDesfecho`** (novo: 4 destinos × 2 desfechos, do nascimento ao gancho do B16, salvar/carregar), `Obrigatorio6_*` | jogar do início ao B16 sem opcionais |
| R7 treino satura | `Obrigatorio7_*` (5 testes), `AplicarTrivial_Repetido_Satura`, `Dominio_SaiUmaVezSO_AoBaterOTetoDaEtapa` | **lacuna:** não existe painel de progresso que mostre o ganho parando e diga por quê (aceite do B15) |
| R8 fala não muda estado | `Obrigatorio8_DialogoGerativoNaoAlteraInventarioNemMissao`, `AtoVindoDeUmaFala_ERecusadoQuandoAbusivo_EIdempotenteQuandoValido` | conversar com todos os NPCs e comparar inventário/moedas/missões |

## Exploits do dossiê §M

| # | Automático | Só em tela |
|---|---|---|
| R9 save editado | **`SliceInteiroTests.R9_SaveEditadoComDestinoInexistente_CarregaERegistraAInconsistencia`** (novo), `DestinySystem.Validar` (`DestinySystemTests`) | — (o registro é o aviso no log) |
| R10 sem ascensão por menu | por construção: `Scripts/Ascension` só tem o README, nenhum código muda Grau de Existência | percorrer menus e inventário |
| R11 recompensa no reload | `Obrigatorio3_SalvarECarregarNoMeio_*`, `Aplicar_DepoisDeRecarregar_NaoDuplica`, `RoundTrip_NaoDuplicaAoRegistrarDeNovoDepoisDeCarregar` | salvar no instante da entrega de q03/q05 |
| R12 = R5 | — | — |
| R13 missão não reabre | `Obrigatorio3_ConcluirDuasVezes_*`, `Obrigatorio3_StatusRevertidoAMao_*`, `MissaoConcluidaDeVerdade_ChegaAMemoria_EAFala` | voltar à ferraria e ouvir a fala de pós-missão do Borin |
| R14 = R7 | — | — |
| R15 NPC lembra o certo | `Lembra_SoCitaEventoQueONpcTestemunha`, `NpcNuncaLembraDeEventoQueNaoEstaNoHistorico`, `Memoria_EPorNpc_CadaUmLembraDoQueViu`, `EventoNoHistorico_ViraLembrancaSoDeQuemTestemunhou`; regra negativa também no R6 novo | Lysa em duas partidas, com e sem a q05 |
| R16 = R8 | — | — |
| R17 12 combinações com save/load | `Obrigatorio2_*` (round-trip do `BirthChoice` nas 12), `Nome_ComAcentoCombinante_ESalvoNormalizado`, `Obrigatorio4_SaveLoadRestauraHistoricoENpcs`; o R6 novo faz o round-trip do save inteiro com "Íris" | — |
| R18 transição não bloqueia | `TelaDoSalto_Abrir_TravaOPersonagem_SoComOSaltoLiberado_EFecharDestrava`, `Obrigatorio5_SoAnunciar_NaoEnvelhece`, `Obrigatorio6_OpcionalAbandonadaEmAndamento_NaoTrancaACampanha` | fechar o aviso do B12, concluir opcionais, voltar e saltar |

## Lacunas abertas

1. **Painel de progresso do treino (R7/B15):** a regra satura, mas o jogador não vê o teto nem o porquê.
2. **Roteiro de tela inteiro (R1–R18):** os passos "só em tela" ainda não foram executados como uma rodada completa numa build; os automáticos acima cobrem a regra de cada um.
3. **Matar o processo na transição do salto (R5):** coberto na regra (gravação atômica, commit único); falta a prova com o processo morto de verdade.

## Dados

`PYTHONUTF8=1 python content/quests/validate_quests.py` continua sendo a verificação dos dados de missão (§5.3); não substitui nenhum passo acima.
