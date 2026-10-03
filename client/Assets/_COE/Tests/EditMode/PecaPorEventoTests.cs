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
