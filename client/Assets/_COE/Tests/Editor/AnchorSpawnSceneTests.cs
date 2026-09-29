using COE.EditorTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace COE.EditorTests
{
    /// <summary>T009: o AnchorSpawn nasce no Player pelo BootstrapSceneBuilder, fica inerte sem ancoras e, com a raiz
    /// "Ancoras" ligada por campo serializado, poe o Player na ancora salva ou em spawn_player.
    /// Aplicar e chamado direto porque Awake nao roda em teste de Editor. Cada teste monta a propria cena em memoria.</summary>
    public class AnchorSpawnSceneTests
    {
        [SetUp]
        public void Abrir()
        {
            // Single: em batch mode a cena inicial e "untitled" e NewScene aditiva lanca (ver AurenSceneTests).
            Scene s = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SceneManager.SetActiveScene(s);
        }

        [TearDown]
        public void Fechar() { LogAssert.NoUnexpectedReceived(); }

        [Test]
        public void Bootstrap_PlayerTemSpawner_InerteSemAncoras()
        {
            BootstrapSceneBuilder.Populate();
            GameObject player = AurenSceneBuilder.Achar("Player");
            AnchorSpawn spawn = player.GetComponent<AnchorSpawn>();
            Assert.IsNotNull(spawn, "Player sem AnchorSpawn");
            Assert.IsNull(Ancoras(spawn), "Bootstrap nao tem ancoras: o campo fica vazio");

            Vector3 antes = player.transform.position;
            spawn.Aplicar("ferraria");
            Assert.AreEqual(antes, player.transform.position, "sem ancoras o spawner nao move ninguem");
        }

        [Test]
        public void Auren_ComAncorasLigadas_EntraNaAncoraSalva_OuNoSpawn()
        {
            AurenSceneBuilder.Populate();
            GameObject player = AurenSceneBuilder.Achar("Player");
            BootstrapSceneBuilder.LigarAncoras(player, AurenSceneBuilder.Achar(AurenSceneBuilder.RaizAncoras).transform);
            AnchorSpawn spawn = player.GetComponent<AnchorSpawn>();

            spawn.Aplicar("ferraria");
            Assert.Less(Vector3.Distance(player.transform.position, AurenSceneBuilder.PosicaoDaAncora("ferraria")), 0.01f,
                "ancora salva que a cena tem: entra nela");

            spawn.Aplicar("ancora_que_nao_existe");
            Assert.Less(Vector3.Distance(player.transform.position, AurenSceneBuilder.PosicaoDaAncora("spawn_player")), 0.01f,
                "ancora desconhecida: spawn_player");

            spawn.Aplicar("");   // B14: o salto zera anchorId
            Assert.Less(Vector3.Distance(player.transform.position, AurenSceneBuilder.PosicaoDaAncora("spawn_player")), 0.01f);
        }

        /// <summary>A ligacao em Auren e do GERADOR de Auren, nao do teste. Fica vermelho ate
        /// AurenSceneBuilder.Populate chamar BootstrapSceneBuilder.LigarAncoras logo depois de criar "Ancoras".</summary>
        [Test]
        public void Auren_GeradorLigaAsAncorasNoSpawner()
        {
            AurenSceneBuilder.Populate();
            AnchorSpawn spawn = AurenSceneBuilder.Achar("Player").GetComponent<AnchorSpawn>();

            Assert.AreEqual(AurenSceneBuilder.Achar(AurenSceneBuilder.RaizAncoras).transform, Ancoras(spawn),
                "falta em AurenSceneBuilder.Populate: BootstrapSceneBuilder.LigarAncoras(Achar(\"Player\"), raizAncoras);");
        }

        static Object Ancoras(AnchorSpawn spawn)
        {
            SerializedProperty p = new SerializedObject(spawn).FindProperty("ancoras");
            Assert.IsNotNull(p, "AnchorSpawn nao tem o campo serializado 'ancoras'");
            return p.objectReferenceValue;
        }
    }
}
