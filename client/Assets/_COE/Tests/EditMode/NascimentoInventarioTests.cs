using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>T012: as moedas e os itens iniciais do nascimento (destino + origem) entram no inventario uma vez so.</summary>
    public class NascimentoInventarioTests
    {
        [Test]
        public void Nascer_CreditaMoedasEItensIniciais_UmaVezSo()
        {
            BirthChoice b = DestinySystem.Confirmar(null, "serena", "agricultores", "Iris").Escolha;
            Circunstancia c = DestinySystem.CircunstanciaDe(b);
            var inv = new InventarioData();

            Inventario.Nascer(inv, c);

            Assert.AreEqual(c.MoedasIniciais, inv.moedas);
            if (c.ItensIniciais != null)
                foreach (string item in c.ItensIniciais) Assert.GreaterOrEqual(Inventario.Quantidade(inv, item), 1, item);

            Assert.AreEqual(0, Inventario.Nascer(inv, c), "nascer de novo (reload, toque duplo) nao credita");
            Assert.AreEqual(c.MoedasIniciais, inv.moedas);
        }
    }
}
