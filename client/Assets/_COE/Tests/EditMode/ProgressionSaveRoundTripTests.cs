using System.IO;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Round-trip do bloco da T009 pelo save. UNICO arquivo de teste da T009 que depende do Unity
    /// (LocalSave usa JsonUtility), por isso mora sozinho: os outros quatro rodam fora do Editor.
    ///
    /// O que isto prova, alem de gravar e ler: o teto anti-farm SOBREVIVE a fechar e reabrir o jogo. Se o
    /// ledger de pratica nao persistisse, bastaria salvar e recarregar para zerar o teto e farmar de novo --
    /// o mesmo exploit por outra porta.</summary>
    public class ProgressionSaveRoundTripTests
    {
        string dir;
        string Caminho { get { return Path.Combine(dir, LocalSave.FileName); } }

        [SetUp]
        public void SetUp()
        {
            dir = Path.Combine(Path.GetTempPath(), "coe_t009_tests_" + System.Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
        }

        [TearDown]
        public void TearDown()
        {
            try { Directory.Delete(dir, true); } catch (System.Exception) { }
        }

        [Test]
        public void BlocoNovo_NasceNeutro_ENaoSobeSaveVersion()
        {
            SaveData d = new SaveData();
            Assert.AreEqual(1, SaveData.SchemaVersion, "bloco NOVO nao sobe schema (regra da T004)");
            Assert.IsNotNull(d.life);
            Assert.AreEqual(1, d.life.day);
            Assert.AreEqual(TimeOfDayCycle.IdManha, d.life.timeOfDay);
            Assert.AreEqual(0, d.life.pratica.Count);
        }

        [Test]
        public void RoundTrip_DiaPeriodoLedgerIdadeENivelDeVida()
        {
            SaveData d = new SaveData();
            LifeEventHistory h = new LifeEventHistory(d);

            Mastery.Praticar(d, new AtividadeDef("treino_de_espada", "marcial", 5));
            Mastery.Praticar(d, new AtividadeDef("carregar_cesto", "forca", 3));
            TimeOfDayCycle.Avancar(d.life);
            AgeAdvance.ConfirmarSalto(d, AgeAdvance.PrepararSalto(d, AgeAdvanceCatalog.SaltoInfancia, h, null), h);

            LocalSave.Save(d, Caminho);
            SaveData lido = LocalSave.Load(Caminho);

            Assert.AreEqual(8, lido.ageYears);
            Assert.AreEqual(2, lido.lifeLevel);
            Assert.AreEqual(d.life.day, lido.life.day);
            Assert.AreEqual(TimeOfDayCycle.IdManha, lido.life.timeOfDay);
            Assert.AreEqual(2, lido.life.pratica.Count);
            Assert.AreEqual(Mastery.GanhoBase, Mastery.ProgressoTotal(lido, "marcial"));
            Assert.AreEqual(Mastery.GanhoBase, Mastery.ProgressoTotal(lido, "forca"));
        }

        [Test]
        public void DepoisDeRecarregar_OTetoAntiFarmContinuaValendo()
        {
            SaveData d = new SaveData();
            AtividadeDef a = new AtividadeDef("bater_no_poste", "marcial", 5);
            for (int i = 0; i < 20; i++) Mastery.Praticar(d, a);

            LocalSave.Save(d, Caminho);
            SaveData lido = LocalSave.Load(Caminho);

            Assert.AreEqual(0, Mastery.Praticar(lido, a).ProgressoGanho,
                "salvar e recarregar nao pode reabrir o teto da etapa");
            Assert.AreEqual(Mastery.TetoAtividadePorFase, Mastery.ProgressoTotal(lido, "marcial"));
        }

        [Test]
        public void DepoisDeRecarregar_OSaltoContinuaNaoDuplicando()
        {
            SaveData d = new SaveData();
            LifeEventHistory h = new LifeEventHistory(d);
            AgeAdvance.ConfirmarSalto(d, AgeAdvance.PrepararSalto(d, AgeAdvanceCatalog.SaltoInfancia, h, null), h);

            LocalSave.Save(d, Caminho);
            SaveData lido = LocalSave.Load(Caminho);
            LifeEventHistory hLido = new LifeEventHistory(lido);

            Assert.IsFalse(AgeAdvance.PodeAvancarIdade(lido, AgeAdvanceCatalog.SaltoInfancia, hLido));
            SaltoPreparado p = AgeAdvance.PrepararSalto(lido, AgeAdvanceCatalog.SaltoInfancia, hLido, null);
            Assert.AreEqual(SaltoBloqueio.JaAplicado, p.Motivo);
            Assert.IsFalse(AgeAdvance.ConfirmarSalto(lido, p, hLido).Aplicado);
            Assert.AreEqual(8, lido.ageYears, "exploit n. 5: idade parada em 8 mesmo depois de recarregar");
        }

        [Test]
        public void SaveSemOBlocoDaT009_CarregaComPadraoNeutro()
        {
            File.WriteAllText(Caminho, "{\"saveVersion\":1,\"ageYears\":5,\"lifeLevel\":1}");
            SaveData lido = LocalSave.Load(Caminho);

            Assert.IsNotNull(lido.life, "save v1 sem a chave 'life' nao pode virar null");
            Assert.AreEqual(TimeOfDayCycle.IdManha, lido.life.timeOfDay);
            Assert.AreEqual(0, Mastery.ProgressoTotal(lido, "marcial"));
        }
    }
}
