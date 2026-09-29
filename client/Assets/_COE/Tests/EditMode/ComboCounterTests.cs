using NUnit.Framework;

namespace COE.Tests
{
    public class ComboCounterTests
    {
        [Test]
        public void Avanca_EDaAVolta()
        {
            var c = new ComboCounter();
            Assert.AreEqual(0, c.Next(999f), "1o golpe");
            Assert.AreEqual(1, c.Next(0.5f));
            Assert.AreEqual(2, c.Next(0.5f));
            Assert.AreEqual(0, c.Next(0.5f), "depois do finalizador volta ao Attack1");
            Assert.AreEqual(1, c.Next(0.5f));
        }

        [Test]
        public void PassouDaJanela_VoltaAoZero()
        {
            var c = new ComboCounter(1.2f);
            c.Next(0f);
            Assert.AreEqual(1, c.Next(1.2f), "na borda ainda encadeia");
            Assert.AreEqual(0, c.Next(1.3f), "passou da janela: recomeca");
            Assert.AreEqual(1, c.Next(0.5f));
        }
    }
}
