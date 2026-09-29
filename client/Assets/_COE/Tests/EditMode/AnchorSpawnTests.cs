using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Escolha da ancora de entrada (T004 grava SaveData.anchorId, T009 consome). Puro: so a regra, sem cena.
    /// A ligacao na cena e o teleporte estao em COE.EditorTests.AnchorSpawnSceneTests.</summary>
    public class AnchorSpawnTests
    {
        static readonly string[] Cena = { "spawn_player", "praca_centro", "ferraria" };

        [Test]
        public void AncoraConhecida_EElaMesma()
        {
            Assert.AreEqual("ferraria", AnchorSpawn.AncoraEfetiva("ferraria", Cena));
        }

        [Test]
        public void AncoraVaziaNulaOuDesconhecida_CaiEmSpawnPlayer()
        {
            Assert.AreEqual("spawn_player", AnchorSpawn.AncoraEfetiva("", Cena), "\"\" e o padrao do SaveData (B06, B14)");
            Assert.AreEqual("spawn_player", AnchorSpawn.AncoraEfetiva(null, Cena));
            Assert.AreEqual("spawn_player", AnchorSpawn.AncoraEfetiva("bosque_antigo", Cena),
                "id antigo, de outra cena ou editado a mao nao trava a entrada");
            Assert.AreEqual("spawn_player", AnchorSpawn.AncoraEfetiva("Ferraria", Cena), "id e exato: snake_case minusculo");
            Assert.AreEqual("spawn_player", AnchorSpawn.AncoraEfetiva("ferraria", null), "cena sem lista nao lanca");
        }
    }
}
