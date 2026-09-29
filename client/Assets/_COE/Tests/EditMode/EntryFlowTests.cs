using System.IO;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>T012: a porta de entrada (EntryFlow), regra pura. -scene manda (desenvolvimento); destino vazio abre o
    /// nascimento, e so o destino decide (nunca o carimbo); destino preenchido vai para a cena salva ou cai em Auren.
    /// A validacao antes do "tem certeza" e a mesma de DestinySystem. O aviso de save olha o principal E o .bak.</summary>
    public class EntryFlowTests
    {
        // Build Settings como o EntryFlow os ve: sem a propria entrada (Bootstrap).
        static readonly string[] Build = { "Auren", "Treino" };

        static BirthChoice Nascida() { return DestinySystem.Confirmar(null, "normal", "artesaos", "Iris").Escolha; }

        static string Cena(BirthChoice b, string sceneId)
        {
            string cena;
            Assert.AreEqual(EntryFlow.Rota.Cena, EntryFlow.Decidir(false, b, sceneId, Build, out cena));
            return cena;
        }

        [Test]
        public void SceneNaLinhaDeComando_NaoFazNada_ComOuSemDestino()
        {
            string cena;
            Assert.AreEqual(EntryFlow.Rota.Nenhuma, EntryFlow.Decidir(true, new BirthChoice(), "", Build, out cena),
                "-scene Bootstrap e area de treino: nem o nascimento abre");
            Assert.AreEqual(EntryFlow.Rota.Nenhuma, EntryFlow.Decidir(true, Nascida(), "auren", Build, out cena));
            Assert.IsNull(cena);
        }

        [Test]
        public void DestinoVazio_AbreNascimento_MesmoComCarimbo()
        {
            string cena;
            Assert.AreEqual(EntryFlow.Rota.Nascimento, EntryFlow.Decidir(false, null, "", Build, out cena));
            Assert.AreEqual(EntryFlow.Rota.Nascimento, EntryFlow.Decidir(false, new BirthChoice(), "auren", Build, out cena),
                "save novo / v0 migrado");
            var carimbada = new BirthChoice { confirmedAtUtc = 1234 };
            Assert.AreEqual(EntryFlow.Rota.Nascimento, EntryFlow.Decidir(false, carimbada, "", Build, out cena),
                "carimbo sem destino nao pula o nascimento");
        }

        [Test]
        public void DestinoPreenchido_VaiParaACenaSalva_MesmoComCarimboZerado()
        {
            BirthChoice b = Nascida();
            b.confirmedAtUtc = 0;   // save editado a mao: nao reabre o nascimento (achado da T003)
            Assert.AreEqual("Auren", Cena(b, ""), "sem cena salva = Auren");
            Assert.AreEqual("Auren", Cena(b, "auren"), "sceneId e snake_case; a cena e Auren");
            Assert.AreEqual("Treino", Cena(b, "treino"));
        }

        [Test]
        public void CenaSalvaForaDoBuild_CaiEmAuren()
        {
            Assert.AreEqual(EntryFlow.CenaInicial, Cena(Nascida(), "cena_que_nao_existe"));
            Assert.AreEqual(EntryFlow.CenaInicial, Cena(Nascida(), "bootstrap"), "a propria entrada nao entra na lista: sem laco");
            Assert.AreEqual(EntryFlow.CenaInicial, Cena(Nascida(), null));
        }

        [Test]
        public void ValidarEscolha_EARegraDoNascimento_ENomePadraoPassa()
        {
            Assert.AreEqual(BirthError.Nenhum, EntryFlow.ValidarEscolha("serena", "agricultores", EntryFlow.NomePadrao),
                "o nome ja preenchido na tela tem de ser aceito");
            Assert.AreEqual(BirthError.DestinoDesconhecido, EntryFlow.ValidarEscolha(null, "agricultores", "Iris"));
            Assert.AreEqual(BirthError.DestinoDesconhecido, EntryFlow.ValidarEscolha("modo_facil", "agricultores", "Iris"));
            Assert.AreEqual(BirthError.OrigemDesconhecida, EntryFlow.ValidarEscolha("ruptura", null, "Iris"));
            Assert.AreEqual(BirthError.NomeCurto, EntryFlow.ValidarEscolha("ruptura", "guardioes", "   "));
            Assert.AreEqual(BirthError.NomeCaractereInvalido, EntryFlow.ValidarEscolha("ruptura", "guardioes", "R2D2"));
        }

        [Test]
        public void SaveMaisNovo_PeloPrincipalOuPeloBak()
        {
            string dir = Path.Combine(Path.GetTempPath(), "coe_entrada_" + System.Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            try
            {
                string path = Path.Combine(dir, LocalSave.FileName);
                string futuro = "{\"saveVersion\":" + (SaveData.SchemaVersion + 1) + "}";
                Assert.IsFalse(EntryFlow.SaveMaisNovo(path), "sem arquivo nao ha aviso");

                File.WriteAllText(path, LocalSave.ToJson(new SaveData()));
                Assert.IsFalse(EntryFlow.SaveMaisNovo(path), "versao atual grava normal");

                File.WriteAllText(LocalSave.BackupPath(path), futuro);
                Assert.IsTrue(EntryFlow.SaveMaisNovo(path), "o .bak mais novo tambem trava a gravacao (LocalSave.Save)");

                File.Delete(LocalSave.BackupPath(path));
                File.WriteAllText(path, futuro);
                Assert.IsTrue(EntryFlow.SaveMaisNovo(path));

                File.WriteAllText(path, "lixo");
                Assert.IsFalse(EntryFlow.SaveMaisNovo(path), "ilegivel nao e mais novo: o LocalSave o poe de lado");
            }
            finally { Directory.Delete(dir, true); }
        }
    }
}
