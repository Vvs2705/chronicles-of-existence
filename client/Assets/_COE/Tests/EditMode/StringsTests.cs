using NUnit.Framework;

namespace COE.Tests
{
    public class StringsTests
    {
        const string Json = "{ \"versao\": \"0.1.0\", \"idioma\": \"pt-BR\", \"strings\": { \"hud.vida\": \"Vida\", \"hud.dica\": \"toque: ataque\\narraste: \\\"câmera\\\"\", \"hud.segundos\": \"{0}s\" } }";

        [TearDown] public void Clear() { Strings.Load(null); }

        [Test]
        public void ChaveExistente_DevolveTexto()
        {
            Assert.IsTrue(Strings.Load(Json));
            Assert.AreEqual(3, Strings.Count);
            Assert.AreEqual("Vida", Strings.Get("hud.vida"));
            Assert.AreEqual("toque: ataque\narraste: \"câmera\"", Strings.Get("hud.dica"), "escapes JSON (\\n, \\\") resolvidos");
            Assert.AreEqual("2.5s", Strings.Format("hud.segundos", 2.5f), "Format invariante (ponto decimal)");
        }

        [Test]
        public void ChaveAusente_DevolveChaveEntreColchetes_NuncaVazio()
        {
            Strings.Load(Json);
            Assert.AreEqual("[hud.nao_existe]", Strings.Get("hud.nao_existe"));
            Assert.AreEqual("[hud.nao_existe]", Strings.Format("hud.nao_existe", 1));
            Assert.AreEqual("[]", Strings.Get(null));
        }

        [Test]
        public void JsonInvalido_NaoLanca_ETabelaFicaVazia()
        {
            Assert.DoesNotThrow(() => Assert.IsFalse(Strings.Load("{ isto nao e json")));
            Assert.DoesNotThrow(() => Assert.IsFalse(Strings.Load("")));
            Assert.DoesNotThrow(() => Assert.IsFalse(Strings.Load(null)));
            Assert.DoesNotThrow(() => Assert.IsFalse(Strings.Load("{ \"versao\": \"1\" }")), "sem objeto strings");
            Assert.AreEqual(0, Strings.Count);
            Assert.AreEqual("[hud.vida]", Strings.Get("hud.vida"));
        }

        [Test]
        public void ParesForaDoObjetoStrings_SaoIgnorados()
        {
            Strings.Load("{ \"versao\": \"0.1.0\", \"idioma\": \"pt-BR\", \"strings\": { \"a\": \"b\" } }");
            Assert.AreEqual("[versao]", Strings.Get("versao"));
            Assert.AreEqual("b", Strings.Get("a"));
        }
    }
}
