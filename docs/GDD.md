# GDD — §0 ESTADO REAL (placar)

Este arquivo **não é o GDD**. É o placar que diz, requisito por requisito, o que o jogo de fato entrega hoje. O desenho mora em [`gdd/GDD_MESTRE_v1_2.md`](gdd/GDD_MESTRE_v1_2.md) (13 capítulos + apêndices) e em [`design/SLICE_A_PRIMEIRA_EXISTENCIA.md`](design/SLICE_A_PRIMEIRA_EXISTENCIA.md) (beats B01–B16, §4, §5). Quando um ADR em [`adr/`](adr/) contradiz o GDD, **vale o ADR** (ex.: ADR-0006 troca "PC/Windows" por mobile, Android primeiro; ADR-0007 §2 troca o rótulo "Vida Difícil" por "Vida Árdua"; ADR-0007 §5 tira a aparência do slice).

- **Data:** 2026-10-29. Auditoria só de leitura do código em `client/Assets/_COE/` (`main` em `498214c`). Nenhum código mudou desde o snapshot de 2026-10-04 do [`PROJETO.md`](PROJETO.md) §0; o único commit depois dele é a proposta [`design/PROPOSTA_MECANICAS_HELL_MODE.md`](design/PROPOSTA_MECANICAS_HELL_MODE.md) (só doc).
- **Contagem de testes** citada (EditMode 610/610, PlayMode 38/38, `-roteiro` nas duas rotas, `save_caos.py` 10/10) é a do `PROJETO.md` §0, de 2026-10-04: **o Unity não foi rodado nesta auditoria**. Rodados aqui: `content/quests/validate_quests.py` (11/11 OK) e `client/tools/check_json.py` (10 arquivos, 0 problema).
- Todo nome de classe, arquivo e teste abaixo foi conferido por Grep no código. Caminhos relativos a `client/Assets/_COE/` quando não dizem outra coisa.

**Legenda de estado**

| Estado | Quando |
|---|---|
| **feito** | regra ou conteúdo no código **e** com teste automático ou verificação citada |
| **parcial** | existe no código, mas falta parte do que o desenho pede (ou existe sem quem o use) |
| **falta** | só existe em documento; nada no código |
| **pendente** | depende de algo externo: aparelho, gente (playtest), conta, credencial, revisão jurídica |
| **futuro** | fora do escopo por decisão registrada (GDD "FUTURO", "Excluir nesta fase", ADR) |
| **decisão** | espera o idealizador (`BLOCKED_PRODUCT_DECISION` ou proposta não aprovada) |

## Placar

| feito | parcial | falta | pendente | futuro | decisão | total | **% feito** |
|---|---|---|---|---|---|---|---|
| 106 | 33 | 6 | 7 | 13 | 6 | 171 | **67,1%** (106 / 158) |

`% feito = feito / (total − futuro)`. Parcial não conta como meio ponto. A seção "Para chegar à loja" (no fim) **não entra no placar**: o GDD foi escrito para o slice no PC e não cobre loja.

**Leitura em uma frase:** as regras do slice (destino permanente, idempotência, salto, anti-farm, memória dos NPCs, save versionado) estão feitas e cobertas por teste; o que falta é o que se vê e se ouve (espada como item, itens em tela, áudio de ambiente e do Limiar, arte final), o que pede gente ou aparelho (playtest, medição no Android) e quatro decisões de produto.

## §0 Matriz

### Cap. 01 — Visão do produto e pilares

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| 01.1 | Action RPG narrativo em 3ª pessoa, single-player | **feito** | `Character/CharacterMotor.cs`, `Camera/ThirdPersonCamera.cs`, `Character/PlayerCombat.cs`; `-roteiro` joga do Limiar ao gancho (`Core/Roteiro.cs`, PROJETO §0) | — |
| 01.2 | Lançamento-alvo | **parcial** | Android: `Editor/ProjectSetup.cs`, `Editor/BuildAndroid.cs` (`BuildRelease`), AAB IL2CPP ARM64 validado com chave de debug, `client/tools/check_16kb.py` | GDD diz PC/Windows; **ADR-0006 prevalece** (mobile). Release assinado: `BLOCKED_CREDENTIAL` |
| 01.3 | Unity 6 LTS com revisão fixada | **feito** | `client/ProjectSettings/ProjectVersion.txt` = 6000.3.23f1; URP 17.3.0 em `client/Packages/manifest.json` | — |
| 01.4 | Fantasia estilizada própria, influência anime | **parcial** | estética de protótipo toon (ADR-0008): `Editor/LookSetup.cs`, `Tests/Editor/EsteticaTests.cs` | arte final presa no G3 (ADR-0002), fora da `main` |
| 01.5 | Expedições cooperativas futuras, não MMORPG | **futuro** | nenhum módulo de rede na manifest (Bloco G) | — |
| 01.6 | Identidade própria; revisar nomes/obra com profissional antes do lançamento | **parcial** | portões G1→G2→G3 (ADR-0002), `docs/arte/PROVENIENCIA.md`, `docs/arte/fichas/`; G1 13/13, G2 4 aprovados | revisão jurídica de nome e marca não feita; nome "Chronicles of Existence" é provisório (PROJETO §1) |

### Cap. 02 — Destino, nascimento e origem

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| 02.1 | Fluxo: Limiar → Aethron → destino → origem → personalização → Auren aos 5 | **feito** | `Core/EntryFlow.cs` (passos `Limiar, Destino, Origem, Nome, Certeza`), `Core/LimiarRoteiro.cs`; `EntradaTelaTests` (PlayMode), `Montar_PoeOPalcoDoLimiar_ComCameraEOSimboloEnquadrado` | o Limiar é um palco dentro da `Bootstrap.unity`, não a cena `TheLiminalRealm` (PROPOSTA do SLICE B01) |
| 02.2 | Quatro destinos como circunstância de vida | **feito** | `Destiny/DestinyCatalog.cs` (`serena`, `normal`, `dificil`, `ruptura`); `Destinos_EOrigens_TemTextoEscrito`, `Destinos_NaoUsamPalavraDeDificuldade`, `ChavesDeTexto_NaoRotulamDestinoComoDificuldade` | rótulo "Vida Árdua" no lugar de "Vida Difícil" (ADR-0007 §2) |
| 02.3 | Destino de Nascimento imutável | **feito** | `Destiny/DestinySystem.cs`; `Obrigatorio1_DestinoImutavelAposConfirmacao`, `Obrigatorio1_NenhumMetodoPublicoAlteraEscolhaConfirmada`, `Obrigatorio1_ZerarCarimboNoSave_NaoReabreNascimento` | ver S.B05b ("Nova vida") |
| 02.4 | Três origens (agricultores, artesãos, guardiões) | **feito** | `DestinyCatalog.Origens`; `TresOrigens_ComOsIdsDoContrato`, `OrigensDisponiveis_DaAsTresOrigens_NosQuatroDestinos` | "compatíveis" sem filtro: as 3 servem aos 4 destinos |
| 02.5 | Mara e Daren mudam profissão, recursos, diálogos e objetos da casa por origem/destino | **parcial** | `Resources/strings.pt-BR.json`: `dialogo.mara.destino_*` e `dialogo.daren.destino_*` (4 cada); por origem `dialogo.daren.origem_*` e, no despertar (q01), `dialogo.mara.primeiro_dia_*` (C3; `Despertar_MaraReconheceAOrigem_UmaFalaPorOrigem`) | Mara só muda pela origem no despertar; objetos da casa não mudam (CONFORMIDADE, "Fica aberto") |
| 02.6 | Destino muda oportunidades e acontecimentos | **parcial** | `DestinyDef` lista oportunidades e eventos (`evento.festa_da_colheita`, `evento.anomalia_no_bosque`, `oportunidade.aulas_com_eira`…) | só `oportunidade.ronda_com_tovin` tem conteúdo (`Dialogue/DialogueCatalog.cs`); os demais ids não são lidos por nenhum sistema |
| 02.7 | Persistir IDs estáveis de destino e origem | **feito** | `Destiny/BirthChoice.cs` dentro de `SaveData.birth`; `Ids_SaoSnakeCaseAsciiSemAcento`, `RoundTrip_ComAcentoNoNome` | — |
| 02.8 | Bloquear mudança retroativa após confirmar | **feito** | idem 02.3; `Nascer_Ok_GravaNascimentoEInventarioInicialNumaGravacao_EDepoisRecusa` | — |
| 02.9 | Nenhuma origem/destino com vantagem numérica irreversível | **feito** | `CadaDestino_EOMaiorEmAlgumEixo`, `NenhumDestino_EOMaiorEmTodosOsEixos`, `CatalogoNaoTemCampoDeDificuldade`, `ApiDeNascimento_NaoAceitaDificuldadeNemAssistencia` | moedas/itens iniciais são HIPÓTESE v0, sem playtest A/B (13.1) |
| 02.10 | Testar as 12 combinações na criação **e nas missões essenciais** | **parcial** | criação: `Obrigatorio2_DozeCombinacoesCarregam`, `Obrigatorio2_DozeCombinacoesTemDadosCompletos` | `SliceInteiroTests.R6_*` percorre as missões com 4 destinos × **a primeira origem** de cada, não as 12 |

### Cap. 03 — Vida, envelhecimento e progressão

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| 03.1 | Cinco etapas (5–7, 8–11, 12–15, 16–18, 19+) | **feito** | `LifeSystem/LifePhases.cs`; `De_RespeitaAsFaixasDoGdd` | — |
| 03.2 | Tempo híbrido: rotina cotidiana + idade por marco confirmado | **feito** | `LifeSystem/TimeOfDayCycle.cs`, `Descanso.cs`, `AgeAdvance.cs`; `ConcluirMissao_AvancaUmPeriodo_NaMesmaGravacao`, `Descansar_AvancaUmPeriodo_GravaUmaVez_ENoiteViraAManhaSeguinte` | sem relógio de tempo real (ADR-0007 §1) |
| 03.3 | Antes do salto, mostrar o que expira | **feito** | `AgeAdvance.PrepararSalto`, `LifeSystem/SaltoHud.cs`; `PrepararSalto_AnunciaIdadeFaseEOQueSeraEncerrado`, `Aviso_NomeiaAsOpcionaisPeloTitulo_EDizOQueSobrevive` | — |
| 03.4 | Opcional não bloqueia o envelhecimento | **feito** | `Obrigatorio6_OpcionalAbandonadaEmAndamento_NaoTrancaACampanha`, `Obrigatorio6_OpcionalFalhada_NaoTrancaACampanha`, `Salto_EncerraAsOpcionaisAbertas_EGravaUmaVez` | — |
| 03.5 | Seis atributos com aplicação (equipamento, precisão, resistência…) | **parcial** | `SaveData.Attributes` e trilhas em `Progression/Mastery.cs` crescem pelo treino | **nenhuma regra lê atributo** (dano, requisito, investigação): só `Mastery` escreve |
| 03.6 | Seis afinidades; origem favorece, não bloqueia | **feito** | `SaveData.Affinities`, afinidades da `OriginDef`; `AfinidadesDeOrigem_EstaoNaListaDoContrato` | — |
| 03.7 | Rendimento limitado por domínio; tarefas mais desafiadoras | **feito** | `Mastery` (retorno decrescente, teto por etapa, limiar de desafio); `Obrigatorio7_*` (5), `MudarDeEtapa_ReabreOTeto` | números HIPÓTESE v0 (13.4) |
| 03.8 | Nível de Vida marca marcos e não multiplica | **feito** | `Progression/LifeLevel.cs`; `LifeLevel_NaoExpoeFatorNenhum`, `ConfirmarSalto_Envelhece_ESobeUmNivelDeVida` | — |
| 03.9 | Nível de Grau reinicia na ascensão | **futuro** | `SaveData` sem grau, de propósito (comentário do cabeçalho) | GDD cap. 04: ascensão fora do slice |
| 03.10 | Slice: 5 anos → ~8, sem corpo por ano | **feito** | `Character/BodyByAge.cs`, `BodyScale.cs`; `Corpo_CincoAnos_E_OitoAnos_PelaBodyScale`, `Corpo_ProporcoesDeCrianca_EmQualquerIdade` | o corpo dos 8 é o dos 5 escalado (malha final no G3) |

### Cap. 04 — Ascensão da Existência

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| 04.1 | Grau independente do destino | **feito** | ADR-0004: `DestinySystem` não toca grau; `ApiDeNascimento_NaoAceitaDificuldadeNemAssistencia` | separação por construção (não há grau ainda) |
| 04.2 | Quatro caminhos (Divino, Arcano, Superação, Ruptura) | **futuro** | `Scripts/Ascension/` só tem o README | — |
| 04.3 | Requisitos: domínio, marco, provação, estabilização, histórico | **futuro** | idem | — |
| 04.4 | Graus I–V (II como estrutura narrativa) | **futuro** | idem | — |
| 04.5 | Arquitetura e prenúncio narrativo de ascensão no slice ("Incluir", cap. 11) | **decisão** | nenhum beat nem texto | `BLOCKED_PRODUCT_DECISION` (CONFORMIDADE) |

### Cap. 05 — Combate, magia e habilidades

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| 05.1 | Vida, Vigor, Mana com barras | **feito** | `Combat/CombatResources.cs`, `Combat/BarrasHud.cs`; `CombatResourcesTests`, `HudLayoutTests` | — |
| 05.2 | Concentração só quando justificar | **futuro** | — | — |
| 05.3 | Sem classe rígida; especializações com compromisso | **futuro** | o treino rende `marcial`/`arcana` (`CombatMoves.AfinidadeMarcial/Arcana`); não há árvore | — |
| 05.4 | Armas, escolas de magia, híbridas, raras | **futuro** | — | "Excluir nesta fase" (cap. 11) |
| 05.5 | Infância sem combate adulto | **feito** | `PrimeiraInfancia_NaoTemAtaqueNemMagia_MasTemEsquiva`, `GolpesDesligados_ToqueVaiParaACamera_EsquivaContinua`, `Treino_SoDepoisDoSaltoParaOitoAnos` | — |
| 05.6 | Espada de madeira | **parcial** | alcance do corpo + lâmina: `CombatMoves.NoCorpo`, `EspadaDeMadeira_AlcanceDaCriancaDe8_DoCorpoAteBracoMaisLamina` | **sem item entregue no treino nem objeto na mão**; Borin só fala dela (CONFORMIDADE; ADR-0010 §3) |
| 05.7 | Ataque leve e forte | **feito** | `CombatMoves.Leve/Forte`; `CombatT011Tests`, `CombatIntegrationTests` (PlayMode) | — |
| 05.8 | Bloqueio e esquiva | **feito** | `Bloqueio_ReduzDanoFrontal`, `Bloqueio_NaoProtegeDasCostas`, `Esquiva_DaIFramesQueEngolemOGolpe`, `SemVigor_EsquivaNaoSai` | esquiva sem deslocamento (0,35 s de i-frames), decidir no playtest (dívida P2) |
| 05.9 | Uma manifestação mágica | **feito** | `Combat/SpellCast.cs`; `T011_Magia_TresFasesEmOrdem_EfeitoSoNaManifestacao`, `Magia_GastaManaENaoVigor`, `MagiaSemMana_NaoSai` | sem clip de magia: toca o `Attack3` (dívida P2) |
| 05.10 | Um adversário de treino | **feito** | `Combat/TrainingDummy.cs`, `TrainingDummyBrain.cs` (telegráfico); `T011_ParceiroDeTreino_FicaNoPostoDaGuarda` | — |
| 05.11 | Sem XP por contagem bruta de golpes | **feito** | `Combat/TrainingPractice.cs`; `Esquiva_SoContaQuandoEngoleUmGolpe`, `T011_TreinoRendeDominioPeloMastery_ESaturaPorVerbo`, `R7_CinquentaVezesOMesmoGolpe_OProgressoParaNoTeto_EOTextoDizPorQue` | — |
| 05.12 | Híbridas como combinações aprovadas em dados | **futuro** | — | — |
| 05.13 | Separar input, regra de dano e apresentação | **feito** | `CombatMoves`/`Damage` puros; `Input/PlayerInputReader.cs` só lê; `DamageFormulaTests`, `Imgui_NaoGanhaTelaNova_EOLeitorDeInputNaoDesenha` | — |

### Cap. 06 — Reputação, relações e NPCs

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| 06.1 | Subconjunto pequeno: Confiança por NPC + reputação local, persistentes | **feito** | `Reputation/ReputationSystem.cs` (`Confianca`, `Renome`); `T010_RoundTrip_PeloLocalSave_ConfiancaRenomeEIdempotenciaSobrevivem` | Honra/Compaixão/Temor ficam de fora (escopo) |
| 06.2 | Reputação consulta acontecimentos; mesma escolha afeta grupos diferentes | **parcial** | `ReputationLedger` aplica por evento: `T010_PromessaQuebrada_DerrubaConfianca_ESoODesfechoGravadoConta` | **inerte**: só a Q-04 muda reputação e nenhuma fala lê (`DialogueCatalog` não usa `Condicao.Confianca`) |
| 06.3 | Dez NPCs com IDs e funções | **feito** | `NPC/NpcCatalog.cs`; `Npcs_TemNomePapelTracosAtividadesERelacoesEscritos`, `Populate_DezNpcs_NaAncoraDaRotinaDoPeriodoDoSave` | — |
| 06.4 | Rotinas diárias determinísticas | **feito** | `NPC/NpcAgenda.cs`; `Rotina_EDeterministica`, `Rotina_CobreOsTresPeriodos_ParaTodoMundo` | NPC teleporta entre vagas, sem andar (dívida P1) |
| 06.5 | Interrupção por eventos com prioridade (ex.: incêndio) | **parcial** | `NpcAgenda.Interrupcao` com `PrioridadeEvento`; `Conversa_Interrompe_ERetomaNaRotinaDoPeriodoAtual` | só a conversa interrompe; nenhum evento da vila usa `PrioridadeEvento` |
| 06.6 | Memória estruturada e diálogos condicionais | **feito** | `NPC/NpcMemory.cs`, `Dialogue/DialogueGraph.cs`; `Lembra_SoCitaEventoQueONpcTestemunha`, `Rotina_MudaComOQueONpcLembra` | — |
| 06.7 | 4 NPCs com memória mais profunda (cap. 11) | **parcial** | os 10 lembram por testemunho (`NpcCatalog`, `Testemunhos_BatemComRegistraNoHistorico`) | não existe o conceito de "profundidade" que distinga 4 deles |
| 06.8 | IA generativa só expressa; não escreve inventário, missão, dinheiro, reputação | **feito** | `Dialogue/QuestIntentAdapter.cs`, `DialogueRunner.cs`; `Obrigatorio8_DialogoGerativoNaoAlteraInventarioNemMissao`, `AtoVindoDeUmaFala_ERecusadoQuandoAbusivo_EIdempotenteQuandoValido` | não há IA generativa no jogo; a guarda é testada com fala simulada |
| 06.9 | Missões principais funcionam offline | **feito** | nenhum código de rede; `-roteiro` joga sem rede | o AAB pede `INTERNET` mesmo assim (dívida P2, decisão 12) |
| 06.10 | Evento persistido: EventID, NPCID, idade, tipo, flags, status | **feito** | `LifeSystem/LifeEvent.cs` (`eventId`, `categoria`, `idade`, `escopo`, `detalhe`; registrado = processado); `NpcMemory` aponta o `eventId` | — |
| 06.11 | Persistir só eventos relevantes | **feito** | `Testemunhos_NuncaTriviais_SemRepeticao_ENpcsQueExistem`, `Teto_ResumeOsMaisAntigos_SemPerderFatoNemAbrirRecompensa` | — |

### Cap. 07 — Missões, economia e exploits

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| 07.1 | Oito missões Q-01..Q-08 | **feito** | `Quest/QuestCatalog.cs` + `content/quests/*.json`; `QuestDataParityTests`; `validate_quests.py` 11/11 (rodado em 2026-10-29) | — |
| 07.2 | Cinco centrais e três opcionais | **feito** | `Obrigatorio6_NenhumaCentralDependeDeOpcional`; validador: "centrais alcançáveis 5/5" | — |
| 07.3 | Q-03 com variantes modulares | **decisão** | Q-03 existe sem variantes (`Q03Aberta_EncerraNaMesmaGravacao_ENadaSeRepete`) | `BLOCKED_PRODUCT_DECISION`: o que varia, e por quê |
| 07.4 | Ramificação só em marcos importantes | **feito** | `QuestDef.Desfechos`; `Q04_Decidir_OfereceUmBotaoPorDesfecho_EGravaSoUm`, `Q07_PerguntarNaVila_SoMaelisAssina_DoisBotoes_EUmSoFica` | — |
| 07.5 | Recompensa narrativa única idempotente | **feito** | `Quest/LifeEventLedger.cs`; `Obrigatorio3_ConcluirDuasVezes_NaoConcedeNemRegistraDuasVezes`, `Obrigatorio3_StatusRevertidoAMao_AindaAssimNaoPagaDeNovo` | — |
| 07.6 | Economia cotidiana (moedas e mercadorias) | **parcial** | `Inventory/Inventario.cs`; moedas no `MissaoHud`; `RoundTrip_PreservaMoedasItensEAplicadas` | moedas sem onde gastar (Oren não vende); itens e moedas na tela de itens (S.ITENS) |
| 07.7 | Recursos especializados e conquistas existenciais | **futuro** | — | "protótipo somente da camada cotidiana" |
| 07.A1 | Abuso: começar no fácil e trocar → destino permanente | **feito** | 02.3; `R9_SaveEditadoComDestinoInexistente_CarregaERegistraAInconsistencia` | — |
| 07.A2 | Abuso: farming de cesta/alvo indefeso → rendimento limitado | **feito** | `Obrigatorio7_QuinhentasRepeticoes_NaoGeramDominioIlimitado`, `AplicarTrivial_Repetido_Satura`; opcionais de instância única (`Q03JaConcluida_FicaComoEsta`) | — |
| 07.A3 | Abuso: duplicar recompensa em save/load → idempotência + gravação consistente | **feito** | `Obrigatorio3_SalvarECarregarNoMeio_PreservaProgressoENaoDuplica`, `Migracao_V1_RecompensaNaoPagaDeNovo_NemComStatusRevertido`, `Save_Atomico_NaoDeixaTmp`; `client/tools/save_caos.py` 10/10 (PC) | — |
| 07.A4 | Abuso: ascender por equipamento caro → requisito pessoal | **feito** | por construção: nenhum código muda Grau (`Scripts/Ascension` só README; T014 R10) | o validador de requisitos pessoais é futuro (04.3) |
| 07.A5 | Abuso: pular fase sem aviso → confirmação + aviso de expiração | **feito** | `SaltoHud` em dois passos (aviso + "Tem certeza?"); `AvisoDoSalto_AbrePeloBotao_AindaNaoFechaSemMudarNada`, `TelaDoSalto_SoAparece_ComAQ08_EAntesDoSalto` | — |
| 07.A6 | Abuso: IA inventar recompensa → ações validadas pelo domínio | **feito** | 06.8 | — |

### Cap. 08 — Mundo, mitologia e campanha

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| 08.1 | Auren, bosque adjacente, três casas, três estruturas públicas, dez NPCs | **feito** | `Editor/AurenSceneBuilder.cs`; `Populate_TresCasasAcessiveis_TemInteriorEVaoDePortaLivre`, `Populate_TresEstruturasPublicas_SaoBlocoSolidoComPortaMarcada`, `Populate_BordaDoBosque_BarraOJogadorEDeixaAEntradaAberta` | estruturas públicas sem interior; o bosque é borda + clareira na cena de Auren |
| 08.2 | Nomes do mundo (Eryndor, Valtheris, Eldoria, Trama, Aethron) | **parcial** | `strings.pt-BR.json` cita Aethron, Eldoria e a Trama | Eryndor, Valtheris e "Bosque dos Sussurros" (dossiê) não aparecem em nenhum texto |
| 08.3 | Panteão (Elyra, Vaelor, Nythera), regiões de Valtheris, campanha em 6 atos | **futuro** | — | "Todo o restante é lore ou expansão" |
| 08.4 | Primeira Fratura só insinuada; protagonista não é o escolhido | **feito** | `Gancho_Escrito_SemRespostaNemPromessaDeContinuacao`, `Limiar_FalasEscritas_SemEscolhidoNemDificuldade` | — |

### Cap. 09 — Arte, áudio e produção 3D

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| 09.1 | Personagens e cenários estilizados finais | **parcial** | protótipo (ADR-0008); G3 lote 1 fora da `main` (`c22b204`) | portão ADR-0002; regra de objetos separados reabriu o G3 |
| 09.2 | Símbolo próprio da Trama | **feito** | `Editor/Prototipos.cs`; `SimboloDoLimiarTests`, `Auren_SimboloNaClareira_NasceDesligado_ELigadoAoSaltoHud` | — |
| 09.3 | Paleta conceitual (#253850, #D6B36A, #648B67, #A86D52, #86C8C9, #9777B8, #E9DEC6) | **parcial** | terracota `#A86D52` no telhado de Auren (`AurenSceneBuilder.cs:147`) | a UI usa `UiEstilo.Noite/Ouro/Tinta`, que não são os hex do GDD |
| 09.4 | Base infantil com variação 5–8, sem modelo por ano | **feito** | 03.10; `client/tools/placeholder_humanoid.py` | — |
| 09.5 | NPCs compartilham rig e animações | **feito** | `Editor/HumanoidSetup.cs`, `HumanoidMapping.cs`, `PrototipoAnimacoes.cs`; `HumanoidMappingTests`, `ClipsNoLugarTests` | — |
| 09.6 | Registrar prompt, origem, versão e licença de cada asset | **parcial** | `docs/arte/PROVENIENCIA.md`, `ACERVO.csv`, `Editor/ArtImportValidator.cs` (`ArtImportValidatorTests`) | Tripo Bridge `BLOCKED_LICENSE` (repo 404, sem LICENSE) |
| 09.7 | Áudio: ambiente de Auren | **falta** | — | CONFORMIDADE, "Fica aberto" |
| 09.8 | Áudio: passos e atividades | **parcial** | `Audio/Sintese.cs` (`Som { Clique, Passo, Golpe, Magia, Missao }`), `Audio/SomDoJogo.cs`; `SinteseTests` | sons sintetizados de protótipo |
| 09.9 | Identidade musical intimista | **parcial** | `Sintese`: caixinha de música de 8 compassos | `// ponytail:` trilha composta é da T013 |
| 09.10 | Motivo do Limiar retomado em momentos-chave (B11, B16) | **falta** | — | CONFORMIDADE |
| 09.11 | HUD simples, legível, escalável, sem depender só da cor | **feito** | `UI/HudLayout.cs`, `UI/Tela.cs` (`Tela.Marcar`); `HudDeJogo_NaAreaSegura_SemCobrirOToque_EmTodaProporcao_NasDuasMaos`, `TextoDaHud_TemPeloMenosOMinimoEmDp_EmTodaTela` | visual de protótipo, sem fonte própria (ver loja) |

### Cap. 10 — Arquitetura técnica e dados

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| 10.1 | Camadas; domínio em C# puro | **feito** | `SaveData`, `Mastery`, `QuestSystem`, `DestinySystem`, `CombatMoves` sem `UnityEngine`; `ArquiteturaTests` | — |
| 10.2 | Sem gerenciador global monolítico | **parcial** | sessão por campo (`Core/Partida.cs`, `Editor/PartidaSetup.cs`); cerca `SaveState_NaoGanhaConsumidorNovo`, `SoOSaveStateTrocaOCurrent` | `SaveState` segue localizador estático com 2 consumidores (dívida P2) |
| 10.3 | Módulos com o que "não devem fazer" | **feito** | `Scripts/<Modulo>/README.md`; `GatilhoQueFechaAMissao_PoeOItemNaMesmaGravacao`; Combat não lê input (05.13) | — |
| 10.4 | CharacterIdentity | **feito** | `SaveData.characterId`, `birth` (nome, destino, origem), `identity.appearanceId` | `appearanceId` vazio: aparência é FUTURO (ADR-0007 §5) |
| 10.5 | LifeState | **feito** | `ageYears`, fase por `LifePhases` (derivada, não gravada), marcos no `lifeHistory`, `lifeLevel`; `RoundTrip_DiaPeriodoLedgerIdadeENivelDeVida` | — |
| 10.6 | ProgressionState | **parcial** | `attributes`, `affinities`, ledger `life.pratica` | sem `gradeId`/`gradeLevel` (ascensão fora do slice) |
| 10.7 | LifeEvent | **feito** | 06.10; `T005_IdsPublicadosNoHistorico_UnicosEEmFormatoEstavel`, `T005_InterromperAntesDeGravar_NaoPerdeNemDuplica` | — |
| 10.8 | QuestState | **feito** | `Quest/QuestState.cs` (`questId`, `status`, `objetivosFeitos`); `QuestSaveTests` | `rewardClaimed` é o id `rec.<missao>.<item>` no histórico, não campo |
| 10.9 | NPCState | **parcial** | `NpcBook` com `memoryEventIds`; `NpcSaveRoundTripTests` | `scheduleState` não gravado (rotina derivada do período e da memória); `relationshipFlags` = reputação |
| 10.10 | SaveMetadata | **parcial** | `saveVersion` (`SchemaVersion = 2`), `createdAtUtc`, `updatedAtUtc` | sem `slotId` (um slot) nem `contentVersion` |
| 10.11 | Definição compartilhada separada de estado por DTO versionado | **feito** | catálogos C# imutáveis (não ScriptableObject); estado só em `SaveData` | — |
| 10.12 | Gravação com temporário, verificação e backup válido | **feito** | `Save/LocalSave.cs` (`.tmp` → troca, `.bak`, `.vN` antes de migrar); `Save_Atomico_NaoDeixaTmp`, `Load_PrincipalCorrompido_RecuperaDoBackup`, `Load_V2Corrompido_ComBakV1_RecuperaMigrando` | — |
| 10.13 | Save antigo trata `schemaVersion` | **feito** | `LocalSave.Migracoes`; `Migracao_V1Completo_ParaV2_SemPerda`, `Migracao_TodaVersaoAnteriorTemPasso`, `FormatoGravado_Congelado`, `VersaoFutura_FicaIntacta_ENadaGravaPorCima` | — |
| 10.14 | Cenas: Bootstrap, MainMenu, CharacterCreation, TheLiminalRealm, Auren_Village/Interiors/Forest/Training | **parcial** | só `Scenes/Bootstrap.unity` e `Scenes/Auren.unity`, geradas por `BootstrapSceneBuilder`/`AurenSceneBuilder`; `Core/CenaCatalogo.cs` | deliberado (PROPOSTA do SLICE B01 e B15) |
| 10.15 | Cooperativo com autoridade de sessão | **futuro** | — | "não implementar ainda" |

### Cap. 11 — Vertical slice: critérios de aceitação

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| 11.1 | Sequência Limiar → … → treino, integrada | **feito** | `SliceInteiroTests.R6_DoNascimentoAoGancho_SemNenhumaOpcional_EmCadaDestinoEDesfecho`; `-roteiro` e `-roteiro quebrada` OK | — |
| 11.2 | Duração de 45–75 min | **pendente** | nenhum playtest com gente (`docs/qa/PLAYTEST.md`, kit pronto) | o robô fecha em 130–180 s |
| 11.C1 | 12 combinações entram no mundo sem erro nem história duplicada | **parcial** | dados e round-trip: `Obrigatorio2_*` | entrar em Auren com as 12 e ler a família é passo de tela (T014 R2) não executado; ver 02.10 |
| 11.C2 | Uma decisão altera diálogo futuro e persiste após salvar/carregar | **feito** | `B08_SeraENilo_FalamDiferente_ConformeAPromessa`, `Obrigatorio4_SaveLoadRestauraHistoricoENpcs`, `Q07_AosOito_MaelisLembraQualSinalFicou` | — |
| 11.C3 | Salto avisado, confirmado e aplicado uma vez | **feito** | `Obrigatorio5_*` (7, inclusive `InterromperAntesDoCommit_RecarregaAntesDoSalto_ESaltaUmaVezSo`); `save_caos.py` 10/10 | no Android quem mata é o sistema: não medido (`BLOCKED_HARDWARE`) |
| 11.C4 | NPC lembra evento relevante sem inventar inventário ou missão | **feito** | `Lembra_SoCitaEventoQueONpcTestemunha`, `EventoSemTestemunha_NaoViraMemoria`, `FalasNovas_NaoPedemMissaoItemNemPoder` | — |
| 11.C5 | Ataque, defesa e magia sem progressão ilimitada | **feito** | 05.7–05.11 | — |
| 11.C6 | Compreensível sem conhecer Hell Mode; arte e narrativa independentes | **pendente** | só se prova com gente (PLAYTEST H1–H5) | a proposta HM (S.HM) ainda não foi julgada |

### Cap. 12 — Backlog e testes obrigatórios

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| T001 | Projeto abre na versão fixada; Bootstrap carrega | **feito** | `BuildWindows.CenasHabilitadas` põe Bootstrap primeiro; `BootstrapSceneTests` | sem `T001_SmokeTest` nem `PROJECT_SETUP.md` (ver B.6) |
| T002 | Controller + câmera sem referências globais rígidas | **feito** | `CharacterMotor`, `ThirdPersonCamera`, `PlayerInteractor`; `InteractionTests`, `MotionSolverTests` | — |
| T003 | 4 destinos, 3 origens, destino imutável | **feito** | 02.2–02.4 | — |
| T004 | Save: identidade, cena e flags; migração | **feito** | `SaveData.sceneId/anchorId`, 10.12–10.13; `CenaCatalogo_IdEstavel_NomeAntigo_EIdInvalido` | flags são eventos no histórico, não bloco próprio (de propósito) |
| T005 | LifeEventHistory idempotente, consulta por NPC/quest | **feito** | `LifeSystem/LifeEventHistory.cs`; `LifeEventHistoryTests` | — |
| T006 | Quest: estados, objetivos, recompensa única, persistência | **feito** | `Quest/QuestSystem.cs`; `QuestTests`, `QuestSaveTests` | — |
| T007 | NPC e diálogo: rotina interrompível e condicional por memória | **feito** | `Conversa_Interrompe_ERetomaNaRotinaDoPeriodoAtual`, `Rotina_MudaComOQueONpcLembra`, `DialogueGraphTests` | interrupção só por conversa (06.5) |
| T008 | Auren graybox navegável | **feito** | `Populate_TodoAncoraTemPercursoLivre_ParaACapsulaDoPlayer`, `Populate_NenhumaVagaDeNpc_EntraEmParedeNemTapaPassagem_EmNenhumPeriodo` | — |
| T009 | Atributos, afinidade e salto confirmado | **feito** | `Mastery`, `AgeAdvance`; `ProgressionMasteryTests`, `AgeAdvanceTests` | — |
| T010 | Confiança e reputação local persistentes | **feito** | 06.1 | — |
| T011 | Treino: leve/forte, defesa, magia | **feito** | 05.7–05.9; `CombatTrainingTests` (EditMode e PlayMode) | — |
| T012 | 8 missões e transição narrativa | **feito** | 07.1, 11.1 | — |
| T013 | Arte, HUD, animação, áudio, profiling | **parcial** | UI em uGUI (Bloco D), `Perf/PerfHud.cs` + `perf_report.py` | arte de protótipo, áudio de protótipo, sem fonte própria |
| T014 | Regressão: 12 casos, saves, duplicação, consequências | **parcial** | `docs/qa/T014_REGRESSAO.md` liga R1–R18 a testes | passos de tela com gente não executados (S.R-TELA) |
| 12.O1 | Não alterar `birthDestinyId` depois do nascimento | **feito** | `Obrigatorio1_*` | — |
| 12.O2 | Evento não paga duas vezes após salvar/carregar | **feito** | `Obrigatorio3_SalvarECarregarNoMeio_*`, `T005_InterromperAntesDeGravar_NaoPerdeNemDuplica` | — |
| 12.O3 | Salto não ocorre sem confirmação nem duplica ao recuperar | **feito** | `Obrigatorio5_ConfirmarSemPreparo_NaoEnvelhece`, `Obrigatorio5_DepoisDeRecarregar_OSaltoNaoDuplica` | — |
| 12.O4 | Opcional não impede progressão principal | **feito** | `Obrigatorio6_CampanhaFechaIgnorandoAsTresOpcionais_EmQualquerDesfecho` | — |
| 12.O5 | Atributo não cresce sem limite com ação trivial | **feito** | `Obrigatorio7_AcaoTrivialRepetida_RendeUmaVezSo_ENuncaViraDominio`, `Obrigatorio7_DepoisDeRecarregar_OTetoAntiFarmContinuaValendo` | — |
| 12.O6 | Variação de origem muda contexto sem quebrar a missão base | **parcial** | `FerramentaDaOrigem_UmaOpcao_QueLevaAOrigemCerta`, `Destino_Origem_TemItem_PassamSoNoNascimentoCerto` | nenhum teste roda a campanha com cada origem (R6 usa uma por destino) |
| 12.O7 | NPC não concede recurso sem operação de domínio | **feito** | `Obrigatorio8_*`, `AtoVindoDeUmaFala_*` | — |
| 12.O8 | Save antigo trata `schemaVersion` | **feito** | 10.13 | — |

### Cap. 13 — Riscos e pendências

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| 13.1 | Equilíbrio dos destinos (playtest A/B) | **pendente** | kit `client/tools/playtest.ps1`, `Core/DiarioDeSessao.cs`, `diario_report.py`; nenhuma sessão | — |
| 13.2 | Sensação de infância divertida, não burocrática | **pendente** | idem | — |
| 13.3 | Limiares e grupos de reputação | **decisão** | ±20 da Q-04 aprovado (ADR-0007 §4); faixas em `ReputationSystem` | grupos e limiares não definidos; reputação inerte (06.2) |
| 13.4 | Curvas/XP só depois de instrumentação e testes | **parcial** | números marcados HIPÓTESE v0 em `Mastery` e `CombatMoves`; diário de sessão instrumenta | sem dado de gente |
| 13.5 | Viabilidade do coop | **futuro** | — | — |
| 13.6 | Licenças/PI: nomes, arte, termos das ferramentas | **pendente** | `PROVENIENCIA.md`; Tripo Bridge `BLOCKED_LICENSE` | revisão profissional e pesquisa de marca não feitas |
| 13.7 | Mercado e orçamento antes de pitch | **falta** | nenhum documento de mercado, orçamento ou equipe | — |
| 13.8 | Versão exata da Unity | **feito** | 01.3 | — |
| 13.9 | Escopo de idiomas e acessibilidade, com testes | **falta** | só `Resources/strings.pt-BR.json`; `Configuracoes` tem mão, sensibilidade, FPS, desempenho, qualidade e som | escopo nunca definido (ver loja) |
| 13.10 | Portão: não expandir antes de testar a infância em Auren | **feito** | só `Bootstrap.unity` e `Auren.unity` existem | — |

### Apêndice B — Fundação T001

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| B.1 | Unity 6000.3.x, URP, Git + LFS, `.meta`, Force Text | **feito** | `ProjectVersion.txt`, `.gitattributes` (LFS para fbx/blend/glb…), `manifest.json` | plataforma Windows do B.1 superada pelo ADR-0006 |
| B.4 | Compila e entra/sai do Play Mode sem erro; cópia limpa abre | **feito** | `verify.ps1 -Modo completo`: 0 erro, 0 aviso (PROJETO §0); worktrees limpas usadas no hardening | — |
| B.6 | Kit T001 (`PROJECT_SETUP.md`, `T001_CRITERIOS.md`, cena `T001_SmokeTest`) | **parcial** | papel coberto por `CLAUDE.md`, `client/README.md` e PROJETO §0 | os arquivos do kit não existem (INCONSISTENCIAS C1) |

### SLICE — beats, regressão e conformidade

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| S.B01 | Limiar: símbolo antes da escolha; reler a última fala | **feito** | `LimiarRoteiro` (`limiar.fala.*`, "Voltar" relê); `Montar_PoeOPalcoDoLimiar_ComCameraEOSimboloEnquadrado` | palco na Bootstrap (02.1) |
| S.B02 | Destinos sem palavra de dificuldade; texto de permanência | **feito** | 02.2 | — |
| S.B03 | Origem muda texto de família, casa e ocupação | **parcial** | `origem.*.oficio`, `dialogo.daren.origem_*`, `dialogo.mara.primeiro_dia_*` (C3) | a casa não muda (02.5) |
| S.B04 | Nome válido; acento sobrevive ao save | **feito** | `Nome_Invalido_NaoConfirma`, `Nome_ComAcentoCombinante_ESalvoNormalizado`, `RoundTrip_ComAcentoNoNome` | aparência FUTURO (ADR-0007 §5) |
| S.B05 | Confirmação em dois passos; save em disco antes de Auren | **feito** | passo `Certeza` do `EntryFlow`; `Nascer_Ok_GravaNascimentoEInventarioInicialNumaGravacao_EDepoisRecusa` | — |
| S.B05b | "Nenhuma tela oferece trocar destino" (B05/R1) × "Nova vida" no título | **decisão** | `SaveState.NovaVida` apaga o save e refaz o nascimento | não reabre o exploit (é outra vida), mas contraria a letra; registrar decisão (CONFORMIDADE) |
| S.B06 | Acorda em `casa_familia`; HUD mostra o objetivo | **parcial** | q01 com `acordar` automático (ADR-0007 §6); `Hud_CentralAntesDaOpcional_ComOObjetivoAtual` | a criança acorda na rua, não dentro de casa (T013) |
| S.B07 | Cotidiano: q02 central, q03/q05/q06 opcionais; dia por missão e descanso | **feito** | `Q05_TratarOAnimal_SoComOBichoCalmo_ComLysaOuTovin`, `Q06_LerORisco_SoPelaFala_ErrarRepete_AcertarCumpre`, `ChapeuDaLysa_SegueAQ05_NaEntradaDoBosque`, `Descanso_UmSo_DentroDeCasa_SemCollider_ELongeDaConversaComAFamilia` | — |
| S.B08 | Promessa: um desfecho, falas diferentes após save/load | **feito** | `Obrigatorio3_DesfechoDaPromessa_ExatamenteUmEIdempotente`, `B08_Sera_RespondeDiferenteSobreNilo_ConformeAPromessa` | — |
| S.B09 | Desaparecimento de Nilo; pistas parciais; mural | **feito** | `ConcluirAQ04_GravaOSumico_ENiloSaiDeAurenNosTresPeriodos`, `B09_QuatroNpcs_FalamDoSumico_SoEnquantoAQ07Anda_ECadaUmDizUmaCoisa`, `Q07_PerguntarNaVila_SoFechaComAssinatura` | — |
| S.B10 | Bosque intransponível fora do vão; volta a pé | **feito** | `Populate_BordaDoBosque_BarraOJogadorEDeixaAEntradaAberta` | — |
| S.B11 | Mesmo símbolo do B01; tocar não salta | **feito** | `SimboloDoLimiarTests`; `RequisitoDoSalto_EOEventoDaFlagDaQ08`, `SaltoSemAQ08Concluida_NaoLibera` | — |
| S.B12 | Aviso nomeia opcionais pelo título; sem moldura vazia | **feito** | `Aviso_NomeiaAsOpcionaisPeloTitulo_EDizOQueSobrevive`, `PrepararSalto_DepoisDeAplicado_NaoAnunciaPerdaNenhuma` | — |
| S.B13 | Confirmação separada; interromper deixa antes ou depois | **feito** | 11.C3 | — |
| S.B14 | Três anos depois: NPCs lembram certo; Auren muda visivelmente | **feito** | `B14_DepoisDoSalto_TodosFalamDiferente_ENiloVolta`, `Pecas_SeguemOHistorico_DoComecoAoSalto` (bancada), `Salto_NaoConcedeAtributoNemAfinidade` | — |
| S.B15 | Treino: quatro verbos, saturação visível, espada comum/marcada | **parcial** | `EsquivaValeComoOTerceiroVerbo_ERepetirUmSoNaoBasta`, `TreinoHud` (`R7_*`); espada do treino: `Inventario.EntregarEspada` (marcada com `confianca_de_borin`, comum sem; uma vez só), `EspadaNaMao` na mão direita; `EspadaDoTreino_ComumOuMarcada_SoNaIdadeDoTreino_UmaVezSo`, `EspadaDoTreino_ChegaPeloPassoDoMundo_NaIdadeDoTreino` | a entrega é pela idade, não na conversa da ferraria/posto (`ponytail:`); espada de primitivas até a arte de objetos |
| S.B16 | Gancho sem resposta nem promessa; grava uma vez | **feito** | `Gancho_AbreQuandoOTreinoTermina_ContinuarGravaOMarcoUmaVez`, `VerGancho_GravaUmaVez_ENaoVoltaAoRecarregar` | — |
| S.R9 | Save editado com destino inválido: registra, não quebra calado | **feito** | `LocalSave.Auditar`; `R9_SaveEditadoComDestinoInexistente_CarregaERegistraAInconsistencia` | — |
| S.R13 | Missão concluída não reabre e o NPC tem fala de pós-missão | **feito** | `MissaoConcluidaDeVerdade_ChegaAMemoria_EAFala`, `Obrigatorio3_ConcluirDuasVezes_*` | — |
| S.R18 | Fechar o aviso, concluir opcionais e saltar depois | **feito** | `TelaDoSalto_Abrir_TravaOPersonagem_SoComOSaltoLiberado_EFecharDestrava`, `Salto_NaoEncerraOpcionalJaConcluida` | — |
| S.R-TELA | Passos "só em tela" de R1–R18 com gente (R5 com processo morto no Android, R7 tentando farmar) | **pendente** | `T014_REGRESSAO.md`, lacunas 2 e 3 | `BLOCKED_HARDWARE` e playtest |
| S.ITENS | Itens ganhos visíveis ("o que sobrevive: moedas e itens", §4.1) | **feito** | botão Itens na HUD abre o painel de itens do `MenuDePausa` (`TextoDosItens`, `Inventario.Visiveis`, `HudLayout.BotaoItens`); `TelaDeItens_ListaOQueTem_ComQuantidadeEMoedas_SemZerado`, `Itens_TodoItemQuePodeEntrarNoInventario_TemNomeEscrito`, foto `itens` do `-roteiro` | lista de texto, sem ícone nem uso de item |
| S.CHIFRE | Chifre do Tovin (recolher, silêncio após o sumiço, fim do treino) | **falta** | — | ficha `tovin.md`; som e prop são arte |
| S.ASSIST | Assistências de combate ajustáveis, com teste de que não mudam destino/save (ADR-0004 inv. 3) | **decisão** | não existem; `ApiDeNascimento_NaoAceitaDificuldadeNemAssistencia` só garante a separação | `BLOCKED_PRODUCT_DECISION`: fora do slice ou um mínimo |
| S.HM | Proposta HM-01..HM-15 (`PROPOSTA_MECANICAS_HELL_MODE.md`) | **decisão** | nada em código | PROPOSTA de 2026-10-15, não aprovada |

### ADRs que acrescentam requisito verificável

| § | desenho | estado | como e onde | divergência |
|---|---|---|---|---|
| ADR-0002 | Portões G1 → G2 → G3 antes de qualquer malha | **parcial** | G1 13/13, G2 com 4 aprovados (`docs/arte/g2/`); nenhum asset da `main` passou o G3 | — |
| ADR-0005 | `QuestCatalog` é a fonte; JSON é cópia guardada por paridade | **feito** | `QuestDataParityTests`, `NpcDataParityTests` | — |
| ADR-0006 | Paisagem, toque (joystick + botões) | **feito** | `Input/ToqueHud.cs`, `TouchControls.cs`; `TouchControlsTests`, `ToqueHudTests`, `HudLayoutTests` (16:9, 19.5:9, 20:9, 360 dp, notch, duas mãos) | — |
| ADR-0009a | Três faixas gráficas com detecção automática | **feito** | `Perf/Qualidade.cs`; `QualidadeTests`, `Faixas_TresNiveisComOProprioUrp_EOMenuRecebeOPosDeAuren` | — |
| ADR-0009b | 30 FPS medidos no aparelho de referência (Android 8, 2–3 GB) | **pendente** | `PerfHud` + `perf_report.py` prontos; só PC em modo celular (`docs/medicoes/`) | `BLOCKED_HARDWARE` |

## Para chegar à loja

O GDD foi escrito para o slice no PC e não pede nada disto. Fora do placar.

| Tema | Estado | Evidência |
|---|---|---|
| **Monetização** | **falta** | nenhum IAP nem anúncio (`manifest.json` sem `com.unity.purchasing`/LevelPlay; Grep sem `Purchasing`). GDD cap. 01 diz só "premium como hipótese comercial". Com público infantil (ADR-0009), loot box é proibida (ECA Digital art. 22). Modelo de negócio é decisão do idealizador. |
| **Loja (Play Console, ficha, assinatura)** | **parcial** | caminho de release pronto (`client/tools/build_android_release.ps1`, `BuildAndroid.BuildRelease`, AAB 50,5 MB, 7 libs a 16 KB). Falta: chave de upload (`BLOCKED_CREDENTIAL`), conta do Play, regra dos 12 testadores por 14 dias, ficha da loja, capturas, classificação IARC. |
| **Ícone** | **falta** | `client/ProjectSettings/ProjectSettings.asset`: os ícones adaptativos do Android têm `m_Textures: []`; nenhum asset de ícone no projeto. |
| **Privacidade** | **falta** | nenhuma política de privacidade nem declaração de dados do Play. Pendente a decisão 12 (público misto, política de Famílias, LGPD art. 14, ECA Digital) e a permissão `INTERNET` que a Unity põe no AAB sem o jogo usar rede (dívida P2). Ajuda: sem conta, chat, anúncio nem rede; save local. |
| **Localização (pt-BR, EN, ES, ID)** | **falta** | só `Resources/strings.pt-BR.json` (443 linhas de texto). A base está pronta: texto por chave (`Loc/Strings.cs`, `StringsLoader.DefaultLanguage = "pt-BR"`), `StringsCoberturaTests`; não há seletor de idioma nem os arquivos EN/ES/ID. Teto registrado: `// ponytail:` um arquivo por idioma em memória. |
| **Áudio** | **parcial** | sons e música sintetizados em código (`Audio/Sintese.cs`, `SomDoJogo.cs`; opção de som em `Configuracoes`). Faltam ambiente de Auren, motivo do Limiar, chifre do Tovin e trilha composta (09.7–09.10). |
| **Tutorial** | **parcial** | ensino pelo jogo: indicador de objetivo (`Quest/IndicadorDeObjetivo.cs`, `RumoDaMissao.cs`), objetivo no `MissaoHud`, prompt de interação, linha do `TreinoHud`. Não há tutorial dos controles de toque nem texto de ajuda (nenhuma chave `dica.*`/`tutorial.*` no arquivo de textos). |
| **Interface final** | **parcial** | todas as telas em uGUI com layout testado por proporção e piso de 12 dp (Bloco D). Visual de protótipo, sem fonte própria nem arte de UI: `BLOCKED_PRODUCT_DECISION` (identidade visual da T013, licença de fonte). |
| **Desempenho no aparelho** | **pendente** | ver ADR-0009b; engasgo frio da primeira esquiva e o voltar do Android 16 (`UI/VoltarHud.cs`, fallback duplo) também esperam aparelho. |
| **iOS** | **futuro** | ADR-0006: exige Mac. |
