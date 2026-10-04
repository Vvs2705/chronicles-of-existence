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

        // P2 (Prompt Mestre §23): o leitor por regex nos casos que costumam quebrar.
        [Test]
        public void ParDepoisDoObjetoStrings_NaoViraTexto()
        {
            Strings.Load("{ \"strings\": { \"a\": \"{0} chaves } dentro do texto\" }, \"autor\": \"fora\" }");
            Assert.AreEqual("{0} chaves } dentro do texto", Strings.Get("a"), "chave dentro do texto nao fecha o objeto");
            Assert.AreEqual("[autor]", Strings.Get("autor"), "par de topo depois de strings nao entra na tabela");
            Assert.AreEqual(1, Strings.Count);
        }

        [Test]
        public void Escapes_DoJson_Acentos_Unicode_BarraTabEAspas()
        {
            Assert.IsTrue(Strings.Load("{ \"strings\": { "
                + "\"acento\": \"Íris já está na praça\", "
                + "\"unicode\": \"S\\u00e3o Jo\\u00e3o \\u2192 fim\", "
                + "\"barra\": \"C:\\\\pasta\\/arquivo\", "
                + "\"tab\": \"a\\tb\", "
                + "\"aspas\": \"ela disse \\\"oi\\\"\", "
                + "\"quebra\": \"linha 1\\nlinha 2\" } }"));
            Assert.AreEqual("Íris já está na praça", Strings.Get("acento"));
            Assert.AreEqual("São João \u2192 fim", Strings.Get("unicode"));
            Assert.AreEqual("C:\\pasta/arquivo", Strings.Get("barra"));
            Assert.AreEqual("a\tb", Strings.Get("tab"));
            Assert.AreEqual("ela disse \"oi\"", Strings.Get("aspas"));
            Assert.AreEqual("linha 1\nlinha 2", Strings.Get("quebra"));
        }

        [Test]
        public void ValorVazio_EFallback_CaemNaChaveDeReserva()
        {
            Strings.Load("{ \"strings\": { \"vazio\": \"\", \"reserva\": \"Texto de reserva\" } }");
            Assert.AreEqual("[vazio]", Strings.Get("vazio"), "texto vazio conta como nao escrito");
            Assert.AreEqual("Texto de reserva", Strings.GetOu("vazio", "reserva"));
            Assert.AreEqual("Texto de reserva", Strings.GetOu("nao_existe", "reserva"));
            Assert.AreEqual("[nem_esta]", Strings.GetOu("nao_existe", "nem_esta"), "sem reserva escrita: a chave da reserva aparece");
        }

        [Test]
        public void ObjetoStringsSemFechar_EJsonTruncado()
        {
            Assert.IsFalse(Strings.Load("{ \"strings\": { \"a\": \"b\" "), "arquivo cortado no meio nao carrega pela metade");
            Assert.AreEqual(0, Strings.Count);
        }

        [Test]
        public void ArquivoDoJogo_CarregaInteiro()
        {
            string p = System.IO.Path.Combine(UnityEngine.Application.dataPath, "_COE", "Resources", "strings.pt-BR.json");
            Assert.IsTrue(Strings.Load(System.IO.File.ReadAllText(p)));
            Assert.Greater(Strings.Count, 200, "o arquivo de textos do slice tem centenas de chaves");
            Assert.AreEqual("[versao]", Strings.Get("versao"), "metadado do topo nao vira texto");
        }
    }
}
