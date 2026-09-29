using System.IO;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Save v1 (T004): schema, versao/migracao, escrita atomica com backup e recuperacao.
    /// Nenhum teste depende de cena nem de Application.persistentDataPath: todos gravam numa pasta temporaria.</summary>
    public class SaveDataTests
    {
        string dir;
        string Path_ { get { return System.IO.Path.Combine(dir, LocalSave.FileName); } }

        [SetUp]
        public void SetUp()
        {
            dir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "coe_save_tests_" + System.Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
        }

        [TearDown]
        public void TearDown()
        {
            try { Directory.Delete(dir, true); } catch (System.Exception) { }
        }

        // Save v0: tem "versao", nao tem "saveVersion".
        const string SaveV0 = "{\"versao\":1,\"volume\":0.4}";

        [Test]
        public void Novo_Padroes()
        {
            SaveData d = new SaveData();
            Assert.AreEqual(1, SaveData.SchemaVersion);
            Assert.AreEqual(SaveData.SchemaVersion, d.saveVersion);
            Assert.AreEqual(5, d.ageYears, "a vida jogavel comeca aos 5");
            Assert.AreEqual(1, d.lifeLevel);
            Assert.AreEqual("", d.characterId, "id so e carimbado na primeira gravacao");
            Assert.IsNotNull(d.birth);
            Assert.AreEqual(0L, d.birth.confirmedAtUtc, "nascimento ainda nao confirmado");
            Assert.AreEqual(1, d.attributes.forca);
            Assert.AreEqual(1, d.attributes.vontade);
            Assert.AreEqual(0, d.affinities.marcial);
            Assert.AreEqual(0, d.affinities.exploratoria);
        }

        [Test]
        public void RoundTrip_ComAcentoNoNome()
        {
            SaveData d = new SaveData();
            d.birth.destinyId = "ruptura";
            d.birth.originId = "guardioes";
            d.birth.characterName = "Aurélio Çãézinho";
            d.birth.confirmedAtUtc = 638000000000000000L;
            d.identity.appearanceId = "ap_02";
            d.ageYears = 8;
            d.lifeLevel = 3;
            d.attributes.percepcao = 7;
            d.affinities.arcana = 2;

            LocalSave.Save(d, Path_);
            SaveData back = LocalSave.Load(Path_);

            Assert.AreEqual("ruptura", back.birth.destinyId);
            Assert.AreEqual("guardioes", back.birth.originId);
            Assert.AreEqual("Aurélio Çãézinho", back.birth.characterName, "acento sobrevive ao disco");
            Assert.AreEqual(638000000000000000L, back.birth.confirmedAtUtc);
            Assert.AreEqual("ap_02", back.identity.appearanceId);
            Assert.AreEqual(8, back.ageYears);
            Assert.AreEqual(3, back.lifeLevel);
            Assert.AreEqual(7, back.attributes.percepcao);
            Assert.AreEqual(1, back.attributes.forca, "campo nao mexido mantem o padrao");
            Assert.AreEqual(2, back.affinities.arcana);
            Assert.AreEqual(SaveData.SchemaVersion, back.saveVersion);
        }

        [Test]
        public void Load_ArquivoAusente_Padrao()
        {
            SaveData d = LocalSave.Load(Path_);
            Assert.AreEqual(SaveData.SchemaVersion, d.saveVersion);
            Assert.AreEqual(5, d.ageYears);
            Assert.IsTrue(string.IsNullOrEmpty(d.birth.destinyId));
            Assert.IsFalse(File.Exists(LocalSave.RejectedPath(Path_)), "nao existir nao e motivo de descarte");
        }

        [Test]
        public void Load_PrincipalCorrompido_RecuperaDoBackup()
        {
            SaveData d = new SaveData();
            d.birth.destinyId = "serena";
            d.birth.characterName = "Íris";
            LocalSave.Save(d, Path_);          // 1a gravacao: so o principal
            d.ageYears = 6;
            LocalSave.Save(d, Path_);          // 2a gravacao: o anterior vira .bak
            Assert.IsTrue(File.Exists(LocalSave.BackupPath(Path_)), "Save gera backup do save anterior");

            File.WriteAllText(Path_, "{ isso nao e json");
            SaveData back = LocalSave.Load(Path_);

            Assert.AreEqual("serena", back.birth.destinyId, "carregou do backup, nao do padrao");
            Assert.AreEqual("Íris", back.birth.characterName);
            Assert.IsTrue(File.Exists(LocalSave.RejectedPath(Path_)), "o arquivo corrompido foi guardado, nao apagado");
        }

        [Test]
        public void Load_CorrompidoSemBackup_Padrao()
        {
            File.WriteAllText(Path_, "{ isso nao e json");
            SaveData back = LocalSave.Load(Path_);
            Assert.AreEqual(SaveData.SchemaVersion, back.saveVersion);
            Assert.IsTrue(string.IsNullOrEmpty(back.birth.destinyId));
            Assert.AreEqual(5, back.ageYears);
            Assert.IsTrue(File.Exists(LocalSave.RejectedPath(Path_)));
        }

        [Test]
        public void Load_SaveV0SemSaveVersion_NaoQuebra_EGuardaCopia()
        {
            File.WriteAllText(Path_, SaveV0);
            Assert.AreEqual(0, LocalSave.VersionOf(SaveV0), "save sem saveVersion nao e save do COE");
            Assert.IsNull(LocalSave.FromJson(SaveV0));

            SaveData back = LocalSave.Load(Path_);

            Assert.AreEqual(SaveData.SchemaVersion, back.saveVersion, "descartado: vira save v1 padrao");
            Assert.IsTrue(string.IsNullOrEmpty(back.birth.destinyId), "nada do v0 vira destino");
            Assert.AreEqual(5, back.ageYears);
            Assert.IsFalse(File.Exists(Path_), "o v0 saiu do lugar do save corrente");
            Assert.IsTrue(File.Exists(LocalSave.RejectedPath(Path_)), "copia do v0 preservada");
            Assert.AreEqual(SaveV0, File.ReadAllText(LocalSave.RejectedPath(Path_)), "copia intacta, byte a byte");
        }

        [Test]
        public void Load_VersaoFutura_Tratada()
        {
            string futuro = "{\"saveVersion\":999,\"ageYears\":40,\"lifeLevel\":77,\"campoQueAindaNaoExiste\":1}";
            File.WriteAllText(Path_, futuro);
            Assert.AreEqual(999, LocalSave.VersionOf(futuro));

            SaveData back = LocalSave.Load(Path_);

            Assert.AreEqual(SaveData.SchemaVersion, back.saveVersion);
            Assert.AreEqual(5, back.ageYears, "save de versao desconhecida nao entra pela metade");
            Assert.AreEqual(1, back.lifeLevel);
            Assert.IsTrue(File.Exists(LocalSave.RejectedPath(Path_)), "save do futuro e guardado, nunca apagado");
        }

        [Test]
        public void Load_BlocoNulo_NaoDerruba()
        {
            File.WriteAllText(Path_, "{\"saveVersion\":1,\"birth\":null,\"attributes\":null}");
            SaveData back = LocalSave.Load(Path_);
            Assert.IsNotNull(back.birth, "quem le o save nunca recebe bloco null");
            Assert.IsNotNull(back.attributes);
            Assert.AreEqual(1, back.attributes.forca);
            Assert.IsNotNull(back.reputation, "bloco acrescentado depois tambem nasce com padrao neutro");
            Assert.IsNotNull(back.quests);
            Assert.IsNotNull(back.npcs);
            // Integracao 2026-09-28: o guard de bloco null de LocalSave.FromJson NAO dispara aqui, e isso esta certo.
            // O JsonUtility do Unity IGNORA "campo": null em objeto aninhado e mantem a instancia padrao — diferente
            // do shim de System.Text.Json usado fora do editor, onde null virava null e o save ia para .rejeitado.
            // A invariante que vale e a de cima (ninguem recebe bloco null); a quarentena continua coberta pelos
            // testes de arquivo corrompido e de versao diferente.
        }

        [Test]
        public void Save_Atomico_NaoDeixaTmp()
        {
            SaveData d = new SaveData();
            LocalSave.Save(d, Path_);
            Assert.IsTrue(File.Exists(Path_));
            Assert.IsFalse(File.Exists(Path_ + ".tmp"), "tmp renomeado por cima do destino");

            LocalSave.Save(d, Path_);
            Assert.IsFalse(File.Exists(Path_ + ".tmp"), "segunda gravacao tambem nao deixa tmp");
            Assert.IsTrue(File.Exists(LocalSave.BackupPath(Path_)));
        }

        [Test]
        public void CharacterId_ECreatedAt_GeradosUmaVezSo()
        {
            SaveData d = new SaveData();
            LocalSave.Save(d, Path_);
            string id = d.characterId;
            string criado = d.createdAtUtc;
            Assert.IsFalse(string.IsNullOrEmpty(id));
            Assert.IsFalse(string.IsNullOrEmpty(criado));

            d.ageYears = 9;
            LocalSave.Save(d, Path_);
            Assert.AreEqual(id, d.characterId, "regravar nao troca a identidade do personagem");
            Assert.AreEqual(criado, d.createdAtUtc);

            SaveData back = LocalSave.Load(Path_);
            Assert.AreEqual(id, back.characterId, "recarregar tambem nao troca");
            Assert.AreEqual(criado, back.createdAtUtc);
            Assert.IsFalse(string.IsNullOrEmpty(back.updatedAtUtc));
        }

        // NEGATIVO: a permanencia do nascimento e regra da T003, mas a persistencia nao pode furar por baixo.
        // Carregar nao normaliza, nao valida e nao "conserta" BirthChoice — devolve exatamente o que esta no arquivo.
        [Test]
        public void Load_NaoConsertaBirth()
        {
            SaveData d = new SaveData();
            d.birth.destinyId = "destino_que_a_t003_nao_conhece";
            d.birth.originId = "origem_estranha";
            d.birth.characterName = "Nôa";
            d.birth.confirmedAtUtc = 42L;
            LocalSave.Save(d, Path_);

            SaveData back = LocalSave.Load(Path_);

            Assert.AreEqual(42L, back.birth.confirmedAtUtc, "confirmedAtUtc nunca volta a 0 no carregamento");
            Assert.AreEqual("destino_que_a_t003_nao_conhece", back.birth.destinyId, "T004 nao troca destino por 'padrao'");
            Assert.AreEqual("origem_estranha", back.birth.originId);
            Assert.AreEqual("Nôa", back.birth.characterName);

            // e nem depois de regravar o que foi carregado
            LocalSave.Save(back, Path_);
            SaveData again = LocalSave.Load(Path_);
            Assert.AreEqual(42L, again.birth.confirmedAtUtc);
            Assert.AreEqual("destino_que_a_t003_nao_conhece", again.birth.destinyId);
        }
    }
}
