using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Round-trip do bloco de memoria de NPC (T007) pelo save da T004. Separado de
    /// NpcMemoryTests porque ESTE arquivo depende de UnityEngine (JsonUtility, via LocalSave) e so roda
    /// dentro do Unity; o resto da T007 e C# puro e roda fora.</summary>
    public class NpcSaveRoundTripTests
    {
        [Test]
        public void SaveNovo_NasceComMemoriaVazia_ESemSubirVersao()
        {
            SaveData d = new SaveData();
            Assert.AreEqual(1, SaveData.SchemaVersion, "bloco novo nao sobe saveVersion");
            Assert.IsNotNull(d.npcs);
            Assert.AreEqual(0, d.npcs.fatos.Count);
            Assert.AreEqual(0, d.npcs.resumos.Count);
        }

        [Test]
        public void RoundTrip_PreservaFatosEResumos()
        {
            SaveData d = new SaveData();
            NpcMemory.Registrar(d.npcs, "borin", "evento.ajudou_na_forja", Importancia.Marcante, 638000000000000000L);
            NpcMemory.Registrar(d.npcs, "lysa", "evento.cesto_devolvido", Importancia.Notavel, 638000000000000001L);
            for (int i = 0; i < NpcMemory.LimiteTrivialPorNpc + 2; i++)
                NpcMemory.Registrar(d.npcs, "oren", "evento.recado_" + i, Importancia.Trivial, i);

            SaveData lido = LocalSave.FromJson(LocalSave.ToJson(d));

            Assert.IsTrue(NpcMemory.Lembra(lido.npcs, "borin", "evento.ajudou_na_forja"));
            Assert.IsTrue(NpcMemory.Lembra(lido.npcs, "lysa", "evento.cesto_devolvido"));
            Assert.AreEqual((int)Importancia.Marcante, NpcMemory.Fatos(lido.npcs, "borin")[0].importancia);
            Assert.AreEqual(638000000000000000L, NpcMemory.Fatos(lido.npcs, "borin")[0].registradoEmUtc);
            Assert.AreEqual(NpcMemory.LimiteTrivialPorNpc, NpcMemory.Fatos(lido.npcs, "oren").Length);
            Assert.AreEqual(2, NpcMemory.Esquecidos(lido.npcs, "oren"), "o resumo tambem sobrevive ao save");
        }

        [Test]
        public void RoundTrip_NaoDuplicaAoRegistrarDeNovoDepoisDeCarregar()
        {
            SaveData d = new SaveData();
            NpcMemory.Registrar(d.npcs, "borin", "evento.ajudou_na_forja", Importancia.Notavel, 10);

            SaveData lido = LocalSave.FromJson(LocalSave.ToJson(d));
            Assert.IsFalse(NpcMemory.Registrar(lido.npcs, "borin", "evento.ajudou_na_forja", Importancia.Notavel, 20),
                "recarregar e reviver a cena nao pode dobrar a memoria");
            Assert.AreEqual(1, NpcMemory.Fatos(lido.npcs, "borin").Length);
        }

        [Test]
        public void SaveAntigo_SemOBlocoDeNpc_CarregaComMemoriaVazia()
        {
            // Save gravado antes desta tarefa: a chave "npcs" simplesmente nao existe no JSON.
            string json = "{\"saveVersion\":1,\"characterId\":\"abc\",\"ageYears\":5,\"lifeLevel\":1}";
            SaveData lido = LocalSave.FromJson(json);
            Assert.IsNotNull(lido.npcs, "campo ausente tem de nascer com padrao neutro, nao null");
            Assert.AreEqual(0, lido.npcs.fatos.Count);
        }
    }
}
