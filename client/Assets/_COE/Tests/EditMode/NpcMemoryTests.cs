using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Memoria do NPC (T007): fatos com significado, por id, idempotentes, com decaimento do
    /// trivial. Puro: nao grava arquivo (o round-trip pelo save esta em NpcSaveRoundTripTests).</summary>
    public class NpcMemoryTests
    {
        NpcBook book;

        [SetUp]
        public void SetUp() { book = new NpcBook(); }

        [Test]
        public void Novo_NaoLembraDeNada()
        {
            Assert.AreEqual(0, book.fatos.Count);
            Assert.AreEqual(0, NpcMemory.Fatos(book, "borin").Length);
            Assert.IsFalse(NpcMemory.Lembra(book, "borin", "evento.ajudou_na_forja"));
        }

        [Test]
        public void Registrar_GuardaOFato()
        {
            Assert.IsTrue(NpcMemory.Registrar(book, "borin", "evento.ajudou_na_forja", Importancia.Notavel, 10));
            Assert.IsTrue(NpcMemory.Lembra(book, "borin", "evento.ajudou_na_forja"));
            Assert.AreEqual(1, NpcMemory.Fatos(book, "borin").Length);
            Assert.AreEqual((int)Importancia.Notavel, NpcMemory.Fatos(book, "borin")[0].importancia);
        }

        [Test]
        public void Registrar_NaoDuplica_MesmoIdDeEvento()
        {
            Assert.IsTrue(NpcMemory.Registrar(book, "borin", "evento.ajudou_na_forja", Importancia.Notavel, 10));
            Assert.IsFalse(NpcMemory.Registrar(book, "borin", "evento.ajudou_na_forja", Importancia.Marcante, 99),
                "o mesmo evento registrado de novo nao entra uma segunda vez");
            Assert.AreEqual(1, NpcMemory.Fatos(book, "borin").Length);
            Assert.AreEqual((int)Importancia.Notavel, NpcMemory.Fatos(book, "borin")[0].importancia,
                "a segunda chamada nao pode reescrever o fato que ja existia");
        }

        [Test]
        public void Memoria_EPorNpc_CadaUmLembraDoQueViu()
        {
            NpcMemory.Registrar(book, "borin", "evento.ajudou_na_forja", Importancia.Notavel, 10);
            Assert.IsTrue(NpcMemory.Lembra(book, "borin", "evento.ajudou_na_forja"));
            Assert.IsFalse(NpcMemory.Lembra(book, "lysa", "evento.ajudou_na_forja"),
                "conhecimento e limitado: Lysa nao viu o que aconteceu na ferraria");
        }

        [Test]
        public void EntradaInvalida_NaoRegistraENaoLanca()
        {
            Assert.IsFalse(NpcMemory.Registrar(book, null, "evento.x", Importancia.Trivial, 1));
            Assert.IsFalse(NpcMemory.Registrar(book, "borin", "", Importancia.Trivial, 1));
            Assert.IsFalse(NpcMemory.Registrar(null, "borin", "evento.x", Importancia.Trivial, 1));
            Assert.AreEqual(0, book.fatos.Count);
        }

        [Test]
        public void Trivial_DecaiParaResumo_QuandoPassaDoLimite()
        {
            int n = NpcMemory.LimiteTrivialPorNpc + 3;
            for (int i = 0; i < n; i++)
                NpcMemory.Registrar(book, "oren", "evento.recado_" + i, Importancia.Trivial, i);

            Assert.AreEqual(NpcMemory.LimiteTrivialPorNpc, NpcMemory.Fatos(book, "oren").Length,
                "a lista para de crescer: memoria nao e transcricao");
            Assert.AreEqual(3, NpcMemory.Esquecidos(book, "oren"), "o que saiu vira numero, nao some sem deixar sinal");
            Assert.IsFalse(NpcMemory.Lembra(book, "oren", "evento.recado_0"), "o mais antigo e o primeiro a sair");
            Assert.IsTrue(NpcMemory.Lembra(book, "oren", "evento.recado_" + (n - 1)), "o mais recente fica");
        }

        [Test]
        public void Marcante_NuncaDecai()
        {
            NpcMemory.Registrar(book, "tovin", "evento.salvou_a_vida", Importancia.Marcante, 0);
            for (int i = 0; i < NpcMemory.LimiteTrivialPorNpc + 10; i++)
                NpcMemory.Registrar(book, "tovin", "evento.conversa_" + i, Importancia.Trivial, i + 1);

            Assert.IsTrue(NpcMemory.Lembra(book, "tovin", "evento.salvou_a_vida"),
                "o que teve significado fica para sempre");
        }

        [Test]
        public void Decaimento_NaoVazaEntreNpcs()
        {
            for (int i = 0; i < NpcMemory.LimiteTrivialPorNpc + 2; i++)
                NpcMemory.Registrar(book, "oren", "evento.recado_" + i, Importancia.Trivial, i);
            NpcMemory.Registrar(book, "eira", "evento.aula_boa", Importancia.Trivial, 100);

            Assert.IsTrue(NpcMemory.Lembra(book, "eira", "evento.aula_boa"));
            Assert.AreEqual(0, NpcMemory.Esquecidos(book, "eira"));
        }

        [Test]
        public void Fato_NaoGuardaTranscricao()
        {
            // Prompt-mestre secao 9 / dossie secao G: memoria guarda fato com significado, nao conversa.
            foreach (System.Reflection.FieldInfo f in typeof(NpcMemoryFact).GetFields())
            {
                string nome = f.Name.ToLowerInvariant();
                Assert.IsFalse(nome.Contains("texto") || nome.Contains("fala") || nome.Contains("transcricao")
                    || nome.Contains("linha") || nome.Contains("dialogo"),
                    "NpcMemoryFact." + f.Name + " parece transcricao de conversa");
            }
        }
    }
}
