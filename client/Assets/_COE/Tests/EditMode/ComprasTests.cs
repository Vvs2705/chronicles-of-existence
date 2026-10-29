using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Desbloqueio do episodio 2 (Loja/Compras.cs; CONTRATO_V1 §2; Prompt Mestre §7): o direito entra uma vez,
    /// grava uma vez, sobrevive a vida nova, e a oferta so aparece com a flag, no menu ou depois do gancho.</summary>
    public class ComprasTests
    {
        [Test]
        public void Conceder_UmaVez_SoProdutoDoCatalogo()
        {
            var s = new SaveData();
            Assert.IsFalse(Compras.Tem(s, Compras.Episodio2));
            Assert.IsTrue(Compras.Conceder(s, Compras.Episodio2));
            Assert.IsTrue(Compras.Conceder(s, Compras.Episodio2), "pendente repetido / restaurar: continua tendo");
            Assert.AreEqual(1, s.compras.direitos.Count, "nao duplica");
            Assert.IsFalse(Compras.Conceder(s, "coe.moedas_x100"), "fora do catalogo: a loja nao confirma");
            Assert.IsFalse(Compras.Conceder(s, null));
            Assert.AreEqual(1, s.compras.direitos.Count);

            var semBloco = new SaveData { compras = null };   // bloco gravado como null
            Assert.IsTrue(Compras.Conceder(semBloco, Compras.Episodio2));
            Assert.IsTrue(Compras.Tem(semBloco, Compras.Episodio2));
        }

        [Test]
        public void ConcederCompra_GravaUmaVez_EOPedidoDeOutroProdutoNaoGrava()
        {
            int gravacoes = 0;
            var g = new GameSession(new SaveData(), () => gravacoes++);
            Assert.IsTrue(g.ConcederCompra(Compras.Episodio2));
            Assert.AreEqual(1, gravacoes, "o direito vai para o disco ANTES de a loja confirmar");
            Assert.IsTrue(g.ConcederCompra(Compras.Episodio2));
            Assert.AreEqual(1, gravacoes, "ja tinha: nao regrava");
            Assert.IsFalse(g.ConcederCompra("coe.outra_coisa"));
            Assert.AreEqual(1, gravacoes);
        }

        [Test]
        public void NovaVida_LevaAsCompras()
        {
            SaveData antes = SaveState.Current;
            try
            {
                SaveState.Current = new SaveData { ageYears = 8 };
                Compras.Conceder(SaveState.Current, Compras.Episodio2);
                SaveState.NovaVida();
                Assert.AreEqual(5, SaveState.Current.ageYears, "vida nova do zero");
                Assert.IsTrue(Compras.Tem(SaveState.Current, Compras.Episodio2), "a compra e da conta, nao da vida");
            }
            finally { SaveState.Current = antes; }
        }

        /// <summary>Prompt Mestre §7: nada no meio de cena. Desligada por padrao enquanto o episodio 2 nao existe.</summary>
        [Test]
        public void Oferta_DesligadaPorPadrao_ESoNoMenuOuDepoisDoGancho()
        {
            Assert.IsFalse(Compras.OfertaLigada, "o episodio 2 ainda nao existe: a oferta fica desligada (ponytail)");
            var s = new SaveData();
            var h = new LifeEventHistory(s);
            Assert.IsFalse(Compras.MostrarOferta(false, true, h), "desligada nao aparece nem no menu");
            Assert.IsTrue(Compras.MostrarOferta(true, true, h), "ligada: no menu, sempre");
            Assert.IsFalse(Compras.MostrarOferta(true, false, h), "fora do menu, antes do gancho: nunca");
            h.Registrar(GameSession.MarcoGancho, LifeEventCategoria.Marco, 8);
            Assert.IsTrue(Compras.MostrarOferta(true, false, h), "fim do episodio 1 (B16)");
            Assert.IsFalse(Compras.MostrarOferta(true, false, null));
        }

        [Test]
        public void TextoDaOferta_PeloEstadoDaCompraEDaLoja()
        {
            Assert.IsTrue(StringsLoader.Load(StringsLoader.DefaultLanguage));
            try
            {
                StringAssert.StartsWith(Strings.Get("loja.ep2.ja_tem"), MenuDePausa.TextoDaOferta(true, LojaIap.Situacao.Pronta, "R$ 9,90", false, ""));
                StringAssert.DoesNotContain("R$", MenuDePausa.TextoDaOferta(true, LojaIap.Situacao.Pronta, "R$ 9,90", false, ""), "ja tem: sem preco");
                StringAssert.Contains(Strings.Get("loja.conectando"), MenuDePausa.TextoDaOferta(false, LojaIap.Situacao.Conectando, null, false, ""));
                StringAssert.Contains(Strings.Get("loja.indisponivel"), MenuDePausa.TextoDaOferta(false, LojaIap.Situacao.SemLoja, null, false, ""));
                string venda = MenuDePausa.TextoDaOferta(false, LojaIap.Situacao.Pronta, "R$ 9,90", true, "x");
                StringAssert.Contains(Strings.Format("loja.preco", "R$ 9,90"), venda);
                StringAssert.Contains(Strings.Get("loja.teste"), venda, "loja falsa avisa que e teste");
                StringAssert.EndsWith("\nx", venda, "o aviso da ultima acao");
                StringAssert.DoesNotContain("[", venda, "todo texto escrito");
            }
            finally { Strings.Load(null); }
        }
    }
}
