using COE.EditorTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace COE.EditorTests
{
    /// <summary>T012: IdadeSceneSetup (chamado pelo BootstrapSceneBuilder.Populate) liga o BodyByAge e a tela do salto
    /// por campo serializado, e o corpo de 8 anos sai inteiro da idade: capsula, visual, camera e golpe.
    /// Aplicar/Abrir chamados direto porque Awake/Update nao rodam em teste de Editor. Cada teste monta a propria cena.</summary>
    public class IdadeSceneTests
    {
        SaveData saveAntes;

        [SetUp]
        public void Abrir()
        {
            // Single: em batch mode a cena inicial e "untitled" e NewScene aditiva lanca (ver AurenSceneTests).
            Scene s = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SceneManager.SetActiveScene(s);
            saveAntes = SaveState.Current;
        }

        [TearDown]
        public void Fechar()
        {
            SaveState.Current = saveAntes;
            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void Populate_LigaBodyByAge_NaCameraENosVisuais()
        {
            BootstrapSceneBuilder.Populate();
            GameObject player = Raiz("Player");
            BodyByAge corpo = player.GetComponent<BodyByAge>();
            Assert.IsNotNull(corpo, "Player sem BodyByAge");
            Assert.AreEqual(Raiz("Main Camera").GetComponent<ThirdPersonCamera>(), Ref(corpo, "cam"));

            SerializedProperty visuais = new SerializedObject(corpo).FindProperty("visuais");
            Assert.AreEqual(player.transform.childCount, visuais.arraySize, "todo visual do Player (capsula e modelo) cresce");
            for (int i = 0; i < visuais.arraySize; i++)
                Assert.AreEqual(player.transform.GetChild(i), visuais.GetArrayElementAtIndex(i).objectReferenceValue);
            Assert.IsNotNull(player.transform.Find("Body"));
            Assert.AreEqual(player.GetComponent<PlayerCombat>(), Ref(Raiz(IdadeSceneSetup.NomeTreino).GetComponent<TreinoHud>(), "combate"),
                "o painel do treino le a pratica do PlayerCombat do Player (Bloco C: sem estado estatico)");
        }

        [Test]
        public void Populate_TelaDoSalto_TravaMotorCombateEInteracao()
        {
            BootstrapSceneBuilder.Populate();
            GameObject player = Raiz("Player");
            SaltoHud hud = Raiz(IdadeSceneSetup.NomeHud).GetComponent<SaltoHud>();
            Assert.IsNotNull(hud, "sem SaltoHud");

            SerializedProperty travar = new SerializedObject(hud).FindProperty("travar");
            var ligados = new Object[travar.arraySize];
            for (int i = 0; i < ligados.Length; i++) ligados[i] = travar.GetArrayElementAtIndex(i).objectReferenceValue;
            CollectionAssert.AreEquivalent(new Object[]
            {
                player.GetComponent<CharacterMotor>(), player.GetComponent<PlayerCombat>(), player.GetComponent<PlayerInteractor>(),
            }, ligados);
        }

        [Test]
        public void TelaDoSalto_Abrir_TravaOPersonagem_SoComOSaltoLiberado_EFecharDestrava()
        {
            BootstrapSceneBuilder.Populate();
            CharacterMotor motor = Raiz("Player").GetComponent<CharacterMotor>();
            PlayerCombat combate = Raiz("Player").GetComponent<PlayerCombat>();
            SaltoHud hud = Raiz(IdadeSceneSetup.NomeHud).GetComponent<SaltoHud>();

            SaveState.Current = new SaveData();   // sem Q-08
            hud.Abrir();
            Assert.IsFalse(hud.Aberto, "sem Q-08 o aviso nao abre");
            Assert.IsTrue(motor.enabled);

            new LifeEventHistory(SaveState.Current).Registrar(AgeAdvanceCatalog.LiberadoPor, LifeEventCategoria.Marco, 5);
            hud.Abrir();
            Assert.IsTrue(hud.Aberto);
            Assert.IsFalse(motor.enabled, "aviso aberto: o personagem nao anda");
            Assert.IsFalse(combate.enabled, "aviso aberto: o personagem nao ataca");

            hud.Fechar();   // "ainda nao"
            Assert.IsFalse(hud.Aberto);
            Assert.IsTrue(motor.enabled && combate.enabled);
            Assert.AreEqual(5, SaveState.Current.ageYears, "abrir e fechar o aviso nao envelhece");
        }

        [Test]
        public void BodyByAge_OitoAnos_CapsulaVisualCameraEGolpe_EVoltaAosCinco()
        {
            BootstrapSceneBuilder.Populate();
            GameObject player = Raiz("Player");
            BodyByAge corpo = player.GetComponent<BodyByAge>();
            ThirdPersonCamera cam = Raiz("Main Camera").GetComponent<ThirdPersonCamera>();

            corpo.Aplicar(8);
            corpo.Aplicar(8);   // idempotente: nao cresce duas vezes
            ConfereCorpo(player, cam, Corpo.DaIdade(8));
            Assert.AreEqual(BootstrapSceneBuilder.AlturaDoGolpe, player.GetComponent<Hitbox>().altura, 1e-4f,
                            "o golpe aos 8 e o mesmo que o instrutor mira");

            corpo.Aplicar(5);
            ConfereCorpo(player, cam, Corpo.DaIdade(5));
        }

        static void ConfereCorpo(GameObject player, ThirdPersonCamera cam, Corpo c)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            Assert.AreEqual(c.Altura, cc.height, 1e-4f, "capsula fora da altura da idade");
            Assert.AreEqual(0f, cc.center.y - cc.height * 0.5f, 1e-4f, "base da capsula fora dos pes");
            Assert.AreEqual(c.Raio, cc.radius, 1e-4f);
            Assert.AreEqual(c.Degrau, cc.stepOffset, 1e-4f);
            Assert.AreEqual(c.AlturaDoGolpe, player.GetComponent<Hitbox>().altura, 1e-4f);

            Transform body = player.transform.Find("Body");   // capsula primitiva: topo = centro + escala
            Assert.AreEqual(c.Altura, body.localPosition.y + body.localScale.y, 1e-4f, "capsula visual fora da altura");

            var so = new SerializedObject(cam);
            Assert.AreEqual(c.PivoCamera, so.FindProperty("pivotOffset").vector3Value.y, 1e-4f);
            Assert.AreEqual(c.DistanciaCamera, so.FindProperty("distance").floatValue, 1e-4f);
        }

        static Object Ref(Object alvo, string campo)
        {
            SerializedProperty p = new SerializedObject(alvo).FindProperty(campo);
            Assert.IsNotNull(p, alvo.GetType().Name + " nao tem o campo '" + campo + "'");
            return p.objectReferenceValue;
        }

        /// <summary>B16: a tela do gancho esta em Auren (travando motor, combate e interacao) e nao na Bootstrap, que e a
        /// area de treino de desenvolvimento: treinar la nao pode encerrar o slice.</summary>
        [Test]
        public void Gancho_SoEmAuren_TravaMotorCombateEInteracao()
        {
            BootstrapSceneBuilder.Populate();
            Assert.IsNull(Object.FindFirstObjectByType<GanchoHud>(), "Bootstrap nao encerra o slice");

            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AurenSceneBuilder.Populate();
            GanchoHud[] ganchos = Object.FindObjectsByType<GanchoHud>(FindObjectsSortMode.None);
            Assert.AreEqual(1, ganchos.Length);
            GameObject player = Raiz("Player");
            SerializedProperty travar = new SerializedObject(ganchos[0]).FindProperty("travar");
            var travados = new System.Collections.Generic.List<Object>();
            for (int i = 0; i < travar.arraySize; i++) travados.Add(travar.GetArrayElementAtIndex(i).objectReferenceValue);
            CollectionAssert.AreEquivalent(new Object[]
                { player.GetComponent<CharacterMotor>(), player.GetComponent<PlayerCombat>(), player.GetComponent<PlayerInteractor>() }, travados);
        }

        static GameObject Raiz(string nome)
        {
            foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
                if (go.name == nome) return go;
            Assert.Fail("a cena nao tem o objeto de raiz '" + nome + "'");
            return null;
        }
    }
}
