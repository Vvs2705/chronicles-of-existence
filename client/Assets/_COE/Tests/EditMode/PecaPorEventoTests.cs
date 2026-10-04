using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>PecaPorEvento.Visivel (ELENCO.md, "peca ligada por evento"): visivel se e so se todos de 'exige' estao no
    /// historico e nenhum de 'some' esta. Regra pura, sem cena.</summary>
    public class PecaPorEventoTests
    {
        const string Sumiu = QuestCatalog.EventoNiloDesapareceu;
        const string Salto = AgeAdvanceCatalog.SaltoInfancia;
        static readonly string[] Nada = new string[0];

        static LifeEventHistory Com(params string[] ids)
        {
            var h = new LifeEventHistory(new SaveData());
            foreach (string id in ids) h.Registrar(id, LifeEventCategoria.Marco, 5);
            return h;
        }

        [Test]
        public void SemCondicao_SempreVisivel()
        {
            Assert.IsTrue(PecaPorEvento.Visivel(Nada, Nada, Com()));
            Assert.IsTrue(PecaPorEvento.Visivel(Nada, Nada, Com(Sumiu, Salto)));
            Assert.IsTrue(PecaPorEvento.Visivel(null, null, Com(Sumiu)), "lista nula vale como vazia");
        }

        [Test]
        public void Exige_TodosTemDeEstar()
        {
            string[] exige = { Sumiu, Salto };
            Assert.IsFalse(PecaPorEvento.Visivel(exige, Nada, Com()));
            Assert.IsFalse(PecaPorEvento.Visivel(exige, Nada, Com(Sumiu)), "um so nao basta");
            Assert.IsTrue(PecaPorEvento.Visivel(exige, Nada, Com(Salto, Sumiu)), "ordem do registro nao importa");
        }

        [Test]
        public void Some_NenhumPodeEstar()
        {
            string[] some = { Salto };
            Assert.IsTrue(PecaPorEvento.Visivel(Nada, some, Com(Sumiu)));
            Assert.IsFalse(PecaPorEvento.Visivel(Nada, some, Com(Salto)));
        }

        [Test]
        public void HistoricoNulo_ValeComoVazio()
        {
            Assert.IsTrue(PecaPorEvento.Visivel(Nada, new[] { Salto }, null), "nada aconteceu: o que so some fica");
            Assert.IsFalse(PecaPorEvento.Visivel(new[] { Sumiu }, Nada, null), "nada aconteceu: o que exige nao aparece");
        }

        /// <summary>ADR-0010 adendo 10: o chapeu da Lysa fica no chao com a q05 em andamento e buscar_ajuda cumprido, e volta
        /// quando ela sai de "em andamento" (concluida, ou encerrada pelo salto). Le so o QuestLog.</summary>
        [Test]
        public void NaMissao_ChapeuDaQ05_SoEmAndamentoComBuscarAjuda()
        {
            const string Q05 = "q05_o_animal_ferido";
            Assert.IsTrue(PecaPorEvento.NaMissao("", "", null), "peca sem missao: so o historico decide");
            Assert.IsFalse(PecaPorEvento.NaMissao(Q05, "buscar_ajuda", null), "sem sistema de missao: nada em andamento");

            foreach (bool concluir in new[] { true, false })
            {
                var s = new SaveData();
                var m = new QuestSystem(s.quests, new HistoricoDeVidaLedger(s));
                Assert.IsFalse(PecaPorEvento.NaMissao(Q05, "buscar_ajuda", m), "q05 nunca tocada");
                var linha = new QuestState { questId = Q05, status = (int)QuestStatus.EmAndamento };
                s.quests.missoes.Add(linha);
                Assert.IsFalse(PecaPorEvento.NaMissao(Q05, "buscar_ajuda", m), "em andamento, Lysa ainda nao veio");
                linha.objetivosFeitos.Add("encontrar_o_animal");
                Assert.IsFalse(PecaPorEvento.NaMissao(Q05, "buscar_ajuda", m));
                linha.objetivosFeitos.Add("buscar_ajuda");
                Assert.IsTrue(PecaPorEvento.NaMissao(Q05, "buscar_ajuda", m), "o chapeu vai para o chao");
                Assert.IsTrue(PecaPorEvento.NaMissao(Q05, "", m), "sem objetivo: basta em andamento");

                if (concluir)
                {
                    Assert.IsTrue(m.CumprirObjetivo(Q05, "tratar_o_animal").Ok);
                    Assert.IsTrue(m.Concluir(Q05).Ok);
                }
                else Assert.IsTrue(m.Encerrar(Q05).Ok);   // o salto
                Assert.IsFalse(PecaPorEvento.NaMissao(Q05, "buscar_ajuda", m), (concluir ? "concluida" : "encerrada") + ": o chapeu volta");
            }
        }

        /// <summary>Arbitragem 2.1: os marcos do sumico vao de evento.nilo_desapareceu ate marco_idade_8.</summary>
        [Test]
        public void JanelaDoSumico_AbreNoSumicoEFechaNoSalto()
        {
            string[] exige = { Sumiu }, some = { Salto };
            Assert.IsFalse(PecaPorEvento.Visivel(exige, some, Com()), "antes da Q-04");
            Assert.IsTrue(PecaPorEvento.Visivel(exige, some, Com(Sumiu)), "do sumico ao salto");
            Assert.IsFalse(PecaPorEvento.Visivel(exige, some, Com(Sumiu, Salto)), "depois do salto");
        }
    }
}
