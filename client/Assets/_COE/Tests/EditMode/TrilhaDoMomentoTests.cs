using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Qual musica toca: a entrada manda sobre tudo, combate sobre lugar, bosque sobre hora; o combate segura alguns
    /// segundos depois do ultimo golpe e entra mais rapido do que as outras trocas.</summary>
    public class TrilhaDoMomentoTests
    {
        [Test]
        public void Entrada_MandaSobreTudo()
        {
            Assert.AreEqual(TrilhaMusical.Limiar, TrilhaDoMomento.Escolher(TelaDeEntrada.Limiar, true, true, true));
            Assert.AreEqual(TrilhaMusical.Menu, TrilhaDoMomento.Escolher(TelaDeEntrada.Menu, true, true, true));
        }

        [Test]
        public void NoMundo_CombateBosqueENoite_NessaOrdem()
        {
            Assert.AreEqual(TrilhaMusical.Combate, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, true, true, true));
            Assert.AreEqual(TrilhaMusical.Bosque, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, false, true, true));
            Assert.AreEqual(TrilhaMusical.Bosque, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, false, true, false));
            Assert.AreEqual(TrilhaMusical.VilaNoite, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, false, false, true));
            Assert.AreEqual(TrilhaMusical.VilaDia, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, false, false, false));
        }

        [Test]
        public void Combate_SeguraDepoisDoGolpe_ESoltaSozinho()
        {
            Assert.IsFalse(TrilhaDoMomento.EmCombate(0f, -1f), "sem golpe nenhum nao ha combate");
            Assert.IsTrue(TrilhaDoMomento.EmCombate(10f, 10f));
            Assert.IsTrue(TrilhaDoMomento.EmCombate(10f + TrilhaDoMomento.SegundosDeCombate - 0.1f, 10f));
            Assert.IsFalse(TrilhaDoMomento.EmCombate(10f + TrilhaDoMomento.SegundosDeCombate, 10f));
        }

        [Test]
        public void Combate_EntraMaisRapidoQueAsOutrasTrocas()
        {
            Assert.Less(TrilhaDoMomento.Troca(TrilhaMusical.Combate), TrilhaDoMomento.Troca(TrilhaMusical.VilaDia));
            Assert.Greater(TrilhaDoMomento.Troca(TrilhaMusical.Combate), 0f);
        }
    }
}
