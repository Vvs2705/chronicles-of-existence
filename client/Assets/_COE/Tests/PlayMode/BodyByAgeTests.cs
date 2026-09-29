using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace COE.PlayModeTests
{
    /// <summary>T012: o Awake do BodyByAge (o que roda no load da cena, depois do SaveBootstrap) le a idade do save.
    /// Mini-cena em codigo: Player com CharacterController e Hitbox; SaveState.Current trocado e restaurado.</summary>
    public class BodyByAgeTests
    {
        GameObject player;
        SaveData saveAntes;

        [SetUp] public void Guardar() { saveAntes = SaveState.Current; }

        [TearDown]
        public void Restaurar()
        {
            SaveState.Current = saveAntes;
            if (player != null) Object.Destroy(player);
        }

        [UnityTest]
        public IEnumerator SaveDeOitoAnos_PlayerNasceComACapsulaEOGolpeDaIdade()
        {
            SaveState.Current = new SaveData { ageYears = 8 };   // o que o SaveBootstrap (-200) carrega depois do salto
            player = new GameObject("Player");
            player.SetActive(false);
            CharacterController cc = player.AddComponent<CharacterController>();
            Hitbox golpe = player.AddComponent<Hitbox>();
            player.AddComponent<BodyByAge>();
            player.SetActive(true);   // Awake aqui, como no load da cena
            yield return null;

            Assert.AreEqual(BodyScale.Crianca8, cc.height, 1e-4f, "capsula de 1,28 m aos 8 anos");
            Assert.AreEqual(BodyScale.Crianca8 * 0.5f, cc.center.y, 1e-4f, "pes no chao");
            Assert.AreEqual(Corpo.DaIdade(8).AlturaDoGolpe, golpe.altura, 1e-4f, "golpe no tronco da crianca de 8");
        }
    }
}
