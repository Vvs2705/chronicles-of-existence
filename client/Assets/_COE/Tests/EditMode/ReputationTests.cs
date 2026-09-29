using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace COE.Tests
{
    /// <summary>Reputacao contextual (T010): aplicacao idempotente por evento canonico, contexto por alvo,
    /// faixa qualitativa, teto anti-farm e persistencia como bloco novo do save.
    /// C# puro — nenhum teste monta cena; o unico toque em UnityEngine e o JsonUtility do round-trip.</summary>
    public class ReputationTests
    {
        SaveData save;
        LifeEventHistory historia;
        ReputationData bloco;

        [SetUp]
        public void SetUp()
        {
            save = new SaveData();
            historia = new LifeEventHistory(save);
            bloco = save.reputation;  // o bloco de verdade do save: e ele que o LocalSave grava
        }

        ReputationSystem Novo()
        {
            return new ReputationSystem(bloco, new ReputationLedger(historia, save));
        }

        /// <summary>Recarregar o save: bloco e historico continuam os mesmos, os sistemas sao reconstruidos.</summary>
        ReputationSystem Recarregar()
        {
            historia = new LifeEventHistory(save);
            return new ReputationSystem(bloco, new ReputationLedger(historia, save));
        }

        // ---------- idempotencia ----------

        [Test]
        public void Aplicar_EventoNovo_MudaOValorEDevolveAMudanca()
        {
            ReputationSystem rep = Novo();

            ReputationResultado r = rep.Aplicar("quest_animal_ferido_curado", "npc_lysa", ReputationDimensao.Confianca, 20);

            Assert.IsTrue(r.Aplicado, "primeira vez aplica");
            Assert.AreEqual(ReputationMotivo.Ok, r.Motivo);
            Assert.AreEqual(1, r.Mudancas.Count);
            Assert.AreEqual(0, r.Mudancas[0].De);
            Assert.AreEqual(20, r.Mudancas[0].Para);
            Assert.AreEqual(ReputationFaixa.Neutro, r.Mudancas[0].FaixaDe);
            Assert.AreEqual(ReputationFaixa.Cordial, r.Mudancas[0].FaixaPara);
            Assert.IsTrue(r.Mudancas[0].MudouDeFaixa, "so vale avisar o jogador quando a faixa muda");
            Assert.AreEqual(20, rep.Valor("npc_lysa", ReputationDimensao.Confianca));
        }

        // NEGATIVO OBRIGATORIO — o coracao da T010 (dossie §M, GDD §07 "Duplicar recompensa em save/load"):
        // o mesmo evento canonico aplicado duas vezes so conta uma.
        [Test]
        public void Aplicar_MesmoEventoDuasVezes_SoContaUma()
        {
            ReputationSystem rep = Novo();
            Assert.IsTrue(rep.Aplicar("quest_animal_ferido_curado", "npc_lysa", ReputationDimensao.Confianca, 20).Aplicado);

            ReputationResultado segunda = rep.Aplicar("quest_animal_ferido_curado", "npc_lysa", ReputationDimensao.Confianca, 20);

            Assert.IsFalse(segunda.Aplicado, "segunda chamada NAO pode conceder de novo");
            Assert.AreEqual(ReputationMotivo.JaAplicado, segunda.Motivo);
            Assert.AreEqual(0, segunda.Mudancas.Count);
            Assert.AreEqual(20, rep.Valor("npc_lysa", ReputationDimensao.Confianca), "o valor nao pode ter dobrado");
            Assert.AreEqual(1, bloco.leituras.Count, "e nao pode ter nascido uma segunda leitura do mesmo par");
        }

        // O caso que o exploit de verdade usa: salvar, recarregar e repetir o gatilho.
        [Test]
        public void Aplicar_DepoisDeRecarregar_NaoDuplica()
        {
            Assert.IsTrue(Novo().Aplicar("quest_cesto_perdido_entregue", "com_auren", ReputationDimensao.Renome, 15).Aplicado);

            ReputationSystem depoisDoLoad = Recarregar();
            ReputationResultado r = depoisDoLoad.Aplicar("quest_cesto_perdido_entregue", "com_auren", ReputationDimensao.Renome, 15);

            Assert.IsFalse(r.Aplicado, "recarregar o save nao pode reabrir a concessao");
            Assert.AreEqual(ReputationMotivo.JaAplicado, r.Motivo);
            Assert.AreEqual(15, depoisDoLoad.Valor("com_auren", ReputationDimensao.Renome));
        }

        // A reputacao marca o SEU proprio fato (prefixo rep_): o id canonico continua livre para o dono dele.
        [Test]
        public void Aplicar_NaoQueimaOIdCanonicoDaMissao()
        {
            Assert.IsTrue(Novo().Aplicar("quest_promessa_cumprida", "npc_nilo", ReputationDimensao.Confianca, 10).Aplicado);

            Assert.IsTrue(historia.Registrar("quest_promessa_cumprida", LifeEventCategoria.Escolha, 5),
                          "a missao ainda precisa conseguir registrar o proprio evento canonico");
            Assert.IsTrue(historia.Ja(ReputationLedger.Prefixo + "quest_promessa_cumprida"));
        }

        // ---------- contexto: o mesmo ato em duas comunidades ----------

        // Dossie §G: heroi numa aldeia, temido em outra. E dado (dois efeitos no mesmo ato), nao `if`.
        [Test]
        public void Aplicar_MesmoAto_SobeNumaComunidadeEDesceNaOutra()
        {
            ReputationSystem rep = Novo();
            ReputationAto denunciar = new ReputationAto(
                "denunciou_o_cacador",
                new ReputationEfeito("com_auren", ReputationDimensao.Renome, 12),
                new ReputationEfeito("com_bosque_dos_sussurros", ReputationDimensao.Renome, -22),
                new ReputationEfeito("npc_tovin", ReputationDimensao.Confianca, -20));

            ReputationResultado r = rep.Aplicar("quest_desaparecimento_denuncia", denunciar);

            Assert.IsTrue(r.Aplicado);
            Assert.AreEqual(3, r.Mudancas.Count);
            Assert.AreEqual(12, rep.Valor("com_auren", ReputationDimensao.Renome));
            Assert.AreEqual(-22, rep.Valor("com_bosque_dos_sussurros", ReputationDimensao.Renome));
            Assert.AreEqual(ReputationFaixa.Neutro, rep.Faixa("com_auren", ReputationDimensao.Renome));
            Assert.AreEqual(ReputationFaixa.Frio, rep.Faixa("com_bosque_dos_sussurros", ReputationDimensao.Renome));
            Assert.AreEqual(ReputationFaixa.Frio, rep.Faixa("npc_tovin", ReputationDimensao.Confianca),
                            "nao existe medidor unico: o mesmo ato produz leituras opostas por alvo");
        }

        [Test]
        public void Aplicar_AtoComVariosAlvos_ContinuaIdempotentePeloEvento()
        {
            ReputationSystem rep = Novo();
            ReputationAto ato = new ReputationAto(
                "denunciou_o_cacador",
                new ReputationEfeito("com_auren", ReputationDimensao.Renome, 12),
                new ReputationEfeito("npc_tovin", ReputationDimensao.Confianca, -20));
            Assert.IsTrue(rep.Aplicar("quest_desaparecimento_denuncia", ato).Aplicado);

            Assert.IsFalse(rep.Aplicar("quest_desaparecimento_denuncia", ato).Aplicado);
            Assert.AreEqual(12, rep.Valor("com_auren", ReputationDimensao.Renome));
            Assert.AreEqual(-20, rep.Valor("npc_tovin", ReputationDimensao.Confianca));
        }

        // ---------- faixa qualitativa nas bordas ----------

        [Test]
        public void Faixa_NasBordas()
        {
            Assert.AreEqual(ReputationFaixa.Hostil, ReputationFaixa.De(-100));
            Assert.AreEqual(ReputationFaixa.Hostil, ReputationFaixa.De(-60), "-60 ainda e hostil");
            Assert.AreEqual(ReputationFaixa.Frio, ReputationFaixa.De(-59), "um ponto acima ja e frio");
            Assert.AreEqual(ReputationFaixa.Frio, ReputationFaixa.De(-20), "-20 ainda e frio");
            Assert.AreEqual(ReputationFaixa.Neutro, ReputationFaixa.De(-19));
            Assert.AreEqual(ReputationFaixa.Neutro, ReputationFaixa.De(0));
            Assert.AreEqual(ReputationFaixa.Neutro, ReputationFaixa.De(19));
            Assert.AreEqual(ReputationFaixa.Cordial, ReputationFaixa.De(20), "20 ja e cordial");
            Assert.AreEqual(ReputationFaixa.Cordial, ReputationFaixa.De(59));
            Assert.AreEqual(ReputationFaixa.Leal, ReputationFaixa.De(60), "60 ja e leal");
            Assert.AreEqual(ReputationFaixa.Leal, ReputationFaixa.De(100));
        }

        [Test]
        public void Aplicar_NaoPassaDoLimiteDaEscala()
        {
            ReputationSystem rep = Novo();
            for (int i = 0; i < 10; i++)
                rep.Aplicar("marco_confianca_" + i, "npc_mara", ReputationDimensao.Confianca, ReputationSystem.DeltaMaximo);

            Assert.AreEqual(ReputationSystem.Maximo, rep.Valor("npc_mara", ReputationDimensao.Confianca));
            Assert.AreEqual(ReputationFaixa.Leal, rep.Faixa("npc_mara", ReputationDimensao.Confianca));
        }

        // ---------- alvo desconhecido ----------

        [Test]
        public void Consulta_AlvoDesconhecido_DevolveNeutroSemLancar()
        {
            ReputationSystem rep = Novo();

            Assert.AreEqual(0, rep.Valor("npc_maelis", ReputationDimensao.Confianca));
            Assert.AreEqual(ReputationFaixa.Neutro, rep.Faixa("npc_maelis", ReputationDimensao.Confianca));
            Assert.AreEqual(0, rep.Valor("com_karvorn", ReputationDimensao.Renome), "comunidade nunca visitada tambem e neutra");
            Assert.AreEqual(0, rep.Valor(null, ReputationDimensao.Confianca), "alvo nulo nao pode lancar numa consulta de UI");
            Assert.AreEqual(ReputationFaixa.Neutro, rep.Faixa("npc_maelis", "temor"),
                            "dimensao ainda nao implementada responde neutro, nao excecao");
            Assert.AreEqual(0, bloco.leituras.Count, "consultar nao pode criar leitura no save");
        }

        // ---------- validacao: dialogo emite intencao, quem aplica valida ----------

        [Test]
        public void Aplicar_IntencaoInvalida_NaoMudaNadaENaoQueimaOEvento()
        {
            ReputationSystem rep = Novo();

            Assert.AreEqual(ReputationMotivo.DeltaForaDoLimite,
                            rep.Aplicar("dialogo_borin_elogio", "npc_borin", ReputationDimensao.Confianca, 100).Motivo,
                            "uma fala nao pode pedir +100 de uma vez");
            Assert.AreEqual(ReputationMotivo.DimensaoDesconhecida,
                            rep.Aplicar("dialogo_borin_elogio", "npc_borin", "bondade", 5).Motivo);
            Assert.AreEqual(ReputationMotivo.AlvoInvalido,
                            rep.Aplicar("dialogo_borin_elogio", "Aldeia Auren", ReputationDimensao.Renome, 5).Motivo,
                            "alvo fora do snake_case e recusado, nao vira leitura fantasma");
            Assert.AreEqual(ReputationMotivo.AtoVazio, rep.Aplicar("dialogo_borin_elogio", (ReputationAto)null).Motivo);
            Assert.AreEqual(0, bloco.leituras.Count, "nenhum pedido invalido tocou o save");

            // e o id continua utilizavel para o pedido correto
            Assert.IsTrue(rep.Aplicar("dialogo_borin_elogio", "npc_borin", ReputationDimensao.Confianca, 5).Aplicado,
                          "pedido invalido nao pode queimar o id do evento");
        }

        // ---------- anti-farm ----------

        // Dossie §D/§F: repeticao trivial tem limite. Cumprimentar mil vezes chega a cordial, nunca a leal.
        [Test]
        public void AplicarTrivial_Repetido_Satura()
        {
            ReputationSystem rep = Novo();

            int aplicados = 0;
            for (int i = 0; i < 200; i++)
                if (rep.AplicarTrivial("npc_borin", ReputationDimensao.Confianca, ReputationSystem.DeltaTrivialMaximo).Aplicado)
                    aplicados++;

            Assert.AreEqual(ReputationSystem.TetoTrivial, rep.Valor("npc_borin", ReputationDimensao.Confianca),
                            "acao trivial repetida para no teto");
            Assert.AreEqual(ReputationFaixa.Cordial, rep.Faixa("npc_borin", ReputationDimensao.Confianca),
                            "trivial chega a cordial; leal exige acontecimento");
            Assert.Less(aplicados, 200, "as repeticoes depois do teto sao recusadas");
            Assert.AreEqual(ReputationMotivo.Saturado,
                            rep.AplicarTrivial("npc_borin", ReputationDimensao.Confianca, 1).Motivo);
        }

        [Test]
        public void AplicarTrivial_AcimaDoTeto_NaoAumentaMasAindaPodeCair()
        {
            ReputationSystem rep = Novo();
            for (int i = 0; i < 4; i++)
                rep.Aplicar("marco_leal_" + i, "npc_nilo", ReputationDimensao.Confianca, ReputationSystem.DeltaMaximo);
            Assert.AreEqual(ReputationFaixa.Leal, rep.Faixa("npc_nilo", ReputationDimensao.Confianca));

            Assert.IsFalse(rep.AplicarTrivial("npc_nilo", ReputationDimensao.Confianca, 5).Aplicado,
                           "quem ja e leal por acontecimentos nao fica mais leal por repeticao");
            Assert.AreEqual(100, rep.Valor("npc_nilo", ReputationDimensao.Confianca));

            Assert.IsTrue(rep.AplicarTrivial("npc_nilo", ReputationDimensao.Confianca, -5).Aplicado,
                          "mas ainda pode perder pontos por um deslize trivial");
            Assert.AreEqual(95, rep.Valor("npc_nilo", ReputationDimensao.Confianca));
        }

        [Test]
        public void AplicarTrivial_NaoPoluiOHistoricoDeVida()
        {
            ReputationSystem rep = Novo();
            for (int i = 0; i < 20; i++) rep.AplicarTrivial("npc_oren", ReputationDimensao.Confianca, 1);

            Assert.AreEqual(0, historia.Total, "gentileza cotidiana nao e fato canonico");
        }

        // ---------- o que mudou desde o ultimo marco ----------

        [Test]
        public void MudancasDesdeMarco_ListaOQueMudouEZeraAoFixar()
        {
            ReputationSystem rep = Novo();
            rep.Aplicar("quest_novo_amanhecer", "com_auren", ReputationDimensao.Renome, 10);
            rep.FixarMarco();
            Assert.AreEqual(0, rep.MudancasDesdeMarco().Count, "logo depois do marco nada mudou");

            rep.Aplicar("quest_segredo_do_ferreiro", "com_auren", ReputationDimensao.Renome, 15);
            rep.Aplicar("quest_segredo_do_ferreiro_borin", "npc_borin", ReputationDimensao.Confianca, 25);

            System.Collections.Generic.List<ReputationMudanca> m = rep.MudancasDesdeMarco();
            Assert.AreEqual(2, m.Count);
            Assert.AreEqual("com_auren", m[0].Alvo);
            Assert.AreEqual(10, m[0].De);
            Assert.AreEqual(25, m[0].Para);
            Assert.IsTrue(m[0].MudouDeFaixa, "neutro -> cordial e o que o resumo do salto temporal conta");

            rep.FixarMarco();
            Assert.AreEqual(0, rep.MudancasDesdeMarco().Count);
        }

        // ---------- costura com a T007 (dialogo) ----------

        // T007 le confianca por um delegate Func<string,int> (DialogueContext.Confianca), com o id NU do NPC.
        // Este teste prova que ConfiancaNo serve de delegate direto — a ligacao inteira e `ctx.Confianca = rep.ConfiancaNo;`
        // sem que Scripts/Reputation compile contra Scripts/Dialogue.
        [Test]
        public void ConfiancaNo_ServeDeDelegateDeLeituraDoDialogo()
        {
            ReputationSystem rep = Novo();
            rep.Aplicar("quest_segredo_do_ferreiro", ReputationAlvo.Npc("borin"), ReputationDimensao.Confianca, 25);

            System.Func<string, int> leitorDoDialogo = rep.ConfiancaNo;

            Assert.AreEqual(25, leitorDoDialogo("borin"), "id nu do NpcCatalog vira alvo npc_borin");
            Assert.AreEqual(0, leitorDoDialogo("lysa"), "NPC sem historico responde neutro, nao excecao");
            Assert.AreEqual(25, rep.Valor("npc_borin", ReputationDimensao.Confianca));
        }

        // O lado de ESCRITA do dialogo: a fala nao chama Aplicar, ela entrega um ReputationAto (o pedido) e
        // quem orquestra passa o id do evento canonico daquele momento. O sistema valida e pode recusar.
        [Test]
        public void AtoVindoDeUmaFala_ERecusadoQuandoAbusivo_EIdempotenteQuandoValido()
        {
            ReputationSystem rep = Novo();
            ReputationAto abuso = new ReputationAto("elogio",
                new ReputationEfeito(ReputationAlvo.Npc("borin"), ReputationDimensao.Confianca, 100));

            Assert.AreEqual(ReputationMotivo.DeltaForaDoLimite, rep.Aplicar("dialogo_borin_elogio", abuso).Motivo,
                            "uma fala — inclusive uma escrita por IA — nao concede +100");
            Assert.AreEqual(0, rep.ConfiancaNo("borin"));

            ReputationAto valido = new ReputationAto("promessa",
                new ReputationEfeito(ReputationAlvo.Npc("borin"), ReputationDimensao.Confianca, 10));
            Assert.IsTrue(rep.Aplicar("dialogo_borin_promessa", valido).Aplicado);
            Assert.IsFalse(rep.Aplicar("dialogo_borin_promessa", valido).Aplicado, "reabrir a conversa nao paga de novo");
            Assert.AreEqual(10, rep.ConfiancaNo("borin"));
        }

        // ---------- persistencia ----------

        // Round-trip do bloco ISOLADO: e assim que ele viaja dentro do SaveData (JsonUtility, T004).
        [Test]
        public void RoundTrip_DoBlocoIsolado()
        {
            ReputationSystem rep = Novo();
            rep.Aplicar("quest_animal_ferido_curado", "npc_lysa", ReputationDimensao.Confianca, 20);
            rep.FixarMarco();
            rep.Aplicar("quest_desaparecimento_denuncia", "com_bosque_dos_sussurros", ReputationDimensao.Renome, -22);

            string json = JsonUtility.ToJson(bloco);
            ReputationData lido = JsonUtility.FromJson<ReputationData>(json);
            ReputationSystem depois = new ReputationSystem(lido, new ReputationLedger(historia, save));

            Assert.AreEqual(20, depois.Valor("npc_lysa", ReputationDimensao.Confianca));
            Assert.AreEqual(-22, depois.Valor("com_bosque_dos_sussurros", ReputationDimensao.Renome));
            Assert.AreEqual(ReputationFaixa.Frio, depois.Faixa("com_bosque_dos_sussurros", ReputationDimensao.Renome));
            Assert.AreEqual(1, depois.MudancasDesdeMarco().Count, "o marco fixado antes da gravacao sobrevive");
            Assert.AreEqual("com_bosque_dos_sussurros", depois.MudancasDesdeMarco()[0].Alvo);
        }

        [Test]
        public void RoundTrip_BlocoVazio()
        {
            string json = JsonUtility.ToJson(new ReputationData());
            ReputationData lido = JsonUtility.FromJson<ReputationData>(json);

            ReputationSystem rep = new ReputationSystem(lido, new ReputationLedger(historia, save));
            Assert.AreEqual(0, rep.Valor("npc_borin", ReputationDimensao.Confianca), "save sem reputacao carrega neutro");
            Assert.AreEqual(0, rep.MudancasDesdeMarco().Count);
        }

        // Save editado a mao / bloco corrompido: abrir nao pode lancar nem deixar leitura mentirosa.
        [Test]
        public void Abrir_BlocoAdulterado_LimpaSemLancar()
        {
            ReputationData sujo = new ReputationData();
            sujo.leituras.Add(Entrada("npc_borin", ReputationDimensao.Confianca, 40));
            sujo.leituras.Add(Entrada("npc_borin", ReputationDimensao.Confianca, 90));  // par repetido
            sujo.leituras.Add(Entrada("", ReputationDimensao.Confianca, 50));           // sem alvo
            sujo.leituras.Add(Entrada("npc_lysa", "bondade", 50));                      // dimensao inexistente
            sujo.leituras.Add(Entrada("npc_sera", ReputationDimensao.Confianca, 9999)); // fora da escala
            sujo.leituras.Add(null);

            ReputationSystem rep = new ReputationSystem(sujo, new ReputationLedger(historia, save));

            Assert.AreEqual(40, rep.Valor("npc_borin", ReputationDimensao.Confianca), "vale a primeira linha, nao a duplicada");
            Assert.AreEqual(0, rep.Valor("npc_lysa", "bondade"));
            Assert.AreEqual(ReputationSystem.Maximo, rep.Valor("npc_sera", ReputationDimensao.Confianca), "valor fora da escala e limitado");
            Assert.AreEqual(2, sujo.leituras.Count, "as linhas invalidas saem do bloco");
        }

        // ---------- T010: fonte unica no historico, desfecho da Q-04, persistencia pelo LocalSave ----------

        const string PromessaCumprida = "evento.q04_promessa_cumprida";
        const string PromessaQuebrada = "evento.q04_promessa_quebrada";

        /// <summary>Grava o fato pelo MESMO caminho da T006 (HistoricoDeVidaLedger, que abre a propria instancia
        /// do historico sobre este save).</summary>
        void MissaoGravou(string eventoId)
        {
            Assert.IsTrue(new HistoricoDeVidaLedger(save).RegistrarSePrimeiro(eventoId, QuestSystem.Escopo("q04_uma_promessa")));
        }

        // Os ids que a T006 publica tem `.` de dono. Sem isto a reputacao recusava "evento.q05_concluida" com
        // fonte_invalida e nenhum fato de missao conseguia mover reputacao.
        [Test]
        public void T010_FonteComNamespaceDaMissao_EAceita_EAlvoContinuaEstrito()
        {
            ReputationSystem rep = Novo();

            Assert.IsTrue(rep.Aplicar("evento.q05_concluida", ReputationAlvo.Npc("lysa"), ReputationDimensao.Confianca, 10).Aplicado);
            Assert.AreEqual(ReputationMotivo.FonteInvalida,
                            rep.Aplicar("Evento.Q05", ReputationAlvo.Npc("lysa"), ReputationDimensao.Confianca, 10).Motivo);
            Assert.AreEqual(ReputationMotivo.FonteInvalida,
                            rep.Aplicar("evento q05", ReputationAlvo.Npc("lysa"), ReputationDimensao.Confianca, 10).Motivo);
            Assert.AreEqual(ReputationMotivo.AlvoInvalido,
                            rep.Aplicar("evento.q06_concluida", "npc.borin", ReputationDimensao.Confianca, 10).Motivo,
                            "alvo e chave de leitura npc_/com_, nao namespace de evento");
        }

        // Fonte unica: toda variacao por evento deixa UM fato rep_ no historico de vida (categoria relacao, escopo =
        // alvo). O bloco guarda o valor; o "por que" mora no historico, onde a T007 o acha pelo escopo do NPC.
        [Test]
        public void T010_VariacaoNasceDeFatoNoHistorico_UmFatoRepPorEvento()
        {
            ReputationSystem rep = Novo();
            rep.Aplicar("evento.q05_concluida", ReputationAlvo.Npc("lysa"), ReputationDimensao.Confianca, 10);
            rep.Aplicar("evento.q05_concluida", ReputationAlvo.Npc("lysa"), ReputationDimensao.Confianca, 10);

            Assert.AreEqual(1, historia.Total, "um evento, um fato: repetir nao cria segundo");
            LifeEvent fato = historia.Todos()[0];
            Assert.AreEqual(ReputationLedger.Prefixo + "evento.q05_concluida", fato.eventId);
            Assert.AreEqual(LifeEventCategoria.Relacao, fato.categoria);
            Assert.AreEqual(1, historia.PorEscopo(ReputationAlvo.Npc("lysa")).Count);
            Assert.AreEqual(10, rep.ConfiancaNo("lysa"));
        }

        // T005 tirou o indice por instancia do LifeEventHistory; a reputacao tinha o mesmo defeito no proprio
        // indice. Duas instancias sobre o MESMO save enxergam uma a outra e nao criam linha repetida. Ledger com
        // historico de OUTRO save e recusado: a marca rep_ iria para um arquivo e o valor para outro.
        [Test]
        public void T010_DuasInstanciasNoMesmoSave_NaoDuplicamNemDivergem_EOutroSaveERecusado()
        {
            ReputationSystem a = new ReputationSystem(bloco, new ReputationLedger(new LifeEventHistory(save), save));
            ReputationSystem b = new ReputationSystem(bloco, new ReputationLedger(new LifeEventHistory(save), save));

            Assert.IsTrue(a.Aplicar("evento.q06_concluida", ReputationAlvo.Npc("borin"), ReputationDimensao.Confianca, 15).Aplicado);
            Assert.AreEqual(ReputationMotivo.JaAplicado,
                            b.Aplicar("evento.q06_concluida", ReputationAlvo.Npc("borin"), ReputationDimensao.Confianca, 15).Motivo);
            Assert.AreEqual(15, b.ConfiancaNo("borin"), "b ve a leitura que a criou depois de b abrir");

            Assert.IsTrue(b.Aplicar("dialogo_borin_promessa", ReputationAlvo.Npc("borin"), ReputationDimensao.Confianca, 5).Aplicado);
            Assert.AreEqual(1, bloco.leituras.Count, "o mesmo par nao ganha segunda linha (o load descartaria uma)");
            Assert.AreEqual(20, a.ConfiancaNo("borin"));

            Assert.Throws<System.ArgumentException>(() => new ReputationLedger(new LifeEventHistory(new SaveData()), save));
        }

        // Slice B08 + GDD §07 ("Q-04: escolha social e confianca"): o desfecho que a T006 grava vira confianca de
        // Sera e Nilo, so depois que o fato existe e uma vez so.
        [Test]
        public void T010_PromessaCumprida_SobeConfiancaDeSeraENilo_UmaVezSo()
        {
            ReputationSystem rep = Novo();
            Assert.AreEqual(0, rep.Sincronizar(), "sem fato no historico, nada muda");
            MissaoGravou("evento.q04_concluida");
            Assert.AreEqual(0, rep.Sincronizar(), "concluir sem desfecho nao mexe em confianca");
            Assert.AreEqual(0, bloco.leituras.Count);

            MissaoGravou(PromessaCumprida);
            Assert.AreEqual(1, rep.Sincronizar());
            int sera = rep.ConfiancaNo("sera");
            Assert.AreEqual(0, rep.Sincronizar(), "sincronizar de novo nao paga de novo");
            Assert.AreEqual(0, Recarregar().Sincronizar(), "nem depois de recarregar");

            ReputationSystem depois = Recarregar();
            Assert.AreEqual(sera, depois.ConfiancaNo("sera"));
            Assert.AreEqual(ReputationFaixa.Cordial, depois.Faixa(ReputationAlvo.Npc("sera"), ReputationDimensao.Confianca));
            Assert.AreEqual(ReputationFaixa.Cordial, depois.Faixa(ReputationAlvo.Npc("nilo"), ReputationDimensao.Confianca));
        }

        [Test]
        public void T010_PromessaQuebrada_DerrubaConfianca_ESoODesfechoGravadoConta()
        {
            ReputationSystem rep = Novo();
            MissaoGravou(PromessaQuebrada);

            Assert.AreEqual(1, rep.Sincronizar());
            Assert.AreEqual(ReputationFaixa.Frio, rep.Faixa(ReputationAlvo.Npc("sera"), ReputationDimensao.Confianca));
            Assert.AreEqual(ReputationFaixa.Frio, rep.Faixa(ReputationAlvo.Npc("nilo"), ReputationDimensao.Confianca));
            Assert.IsFalse(historia.Ja(ReputationLedger.Prefixo + PromessaCumprida), "o outro desfecho nao contou");
            Assert.AreEqual(2, bloco.leituras.Count, "so Sera e Nilo; ninguem mais foi tocado");
        }

        // IDs estaveis: consequencia com id que nenhuma missao grava, NPC inexistente ou delta acima do teto nunca
        // aplicaria — em silencio. Este teste fica vermelho antes.
        [Test]
        public void T010_Consequencias_ApontamParaEventoPublicadoENpcExistente_ESaoAtosValidos()
        {
            HashSet<string> publicados = new HashSet<string>();
            foreach (QuestDef d in QuestCatalog.Missoes)
            {
                publicados.Add(d.EventoDeConclusao);
                publicados.UnionWith(d.EventosAoConcluir);
                publicados.UnionWith(d.Desfechos);
            }

            foreach (ReputationConsequencia c in ReputationSystem.Consequencias)
            {
                Assert.IsTrue(publicados.Contains(c.EventoId), "nenhuma missao grava " + c.EventoId);
                foreach (ReputationEfeito ef in c.Ato.Efeitos)
                    if (ef.Alvo.StartsWith(ReputationAlvo.PrefixoNpc))
                        Assert.IsNotNull(NpcCatalog.Npc(ef.Alvo.Substring(ReputationAlvo.PrefixoNpc.Length)), c.EventoId + ": NPC inexistente " + ef.Alvo);

                SaveData s = new SaveData();
                ReputationResultado r = new ReputationSystem(s.reputation, new ReputationLedger(new LifeEventHistory(s), s)).Aplicar(c.EventoId, c.Ato);
                Assert.IsTrue(r.Aplicado, c.EventoId + " recusado: " + r.Motivo);
            }
        }

        // Persistencia de verdade: o SaveData inteiro pelo JSON do LocalSave (o mesmo ToJson/FromJson que vai para
        // disco). Confianca por NPC, renome por comunidade e a marca rep_ voltam juntos; o que ja contou continua contado.
        [Test]
        public void T010_RoundTrip_PeloLocalSave_ConfiancaRenomeEIdempotenciaSobrevivem()
        {
            ReputationSystem rep = Novo();
            rep.Aplicar("evento.q05_concluida", ReputationAlvo.Npc("lysa"), ReputationDimensao.Confianca, 20);
            rep.Aplicar("evento.q03_concluida", ReputationAlvo.Comunidade("auren"), ReputationDimensao.Renome, -20);
            rep.AplicarTrivial(ReputationAlvo.Npc("borin"), ReputationDimensao.Confianca, 5);
            MissaoGravou(PromessaCumprida);
            Assert.AreEqual(1, rep.Sincronizar());

            SaveData back = LocalSave.FromJson(LocalSave.ToJson(save));
            Assert.IsNotNull(back);
            ReputationSystem depois = new ReputationSystem(back.reputation, new ReputationLedger(new LifeEventHistory(back), back));

            Assert.AreEqual(20, depois.ConfiancaNo("lysa"));
            Assert.AreEqual(-20, depois.RenomeEm("auren"), "reputacao por comunidade persiste");
            Assert.AreEqual(ReputationFaixa.Frio, depois.Faixa(ReputationAlvo.Comunidade("auren"), ReputationDimensao.Renome));
            Assert.AreEqual(5, depois.ConfiancaNo("borin"), "ato trivial persiste no bloco, sem fato no historico");
            Assert.AreEqual(rep.ConfiancaNo("sera"), depois.ConfiancaNo("sera"));

            Assert.AreEqual(ReputationMotivo.JaAplicado,
                            depois.Aplicar("evento.q05_concluida", ReputationAlvo.Npc("lysa"), ReputationDimensao.Confianca, 20).Motivo,
                            "a marca rep_ voltou do disco: repetir depois do load nao paga");
            Assert.AreEqual(0, depois.Sincronizar(), "a promessa nao conta de novo depois do load");
            Assert.AreEqual(20, depois.ConfiancaNo("lysa"));
        }

        static ReputationEntry Entrada(string alvo, string dimensao, int valor)
        {
            ReputationEntry e = new ReputationEntry();
            e.alvo = alvo;
            e.dimensao = dimensao;
            e.valor = valor;
            e.valorMarco = valor;
            return e;
        }
    }
}
