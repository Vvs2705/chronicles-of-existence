using COE.EditorTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace COE.EditorTests
{
    /// <summary>T012: so a Bootstrap e porta de entrada. EntradaSceneSetup.Montar poe UMA rota ligada ao input do Populate;
    /// o Populate sozinho (chassi de Auren e dos testes) nao poe nenhuma, senao Auren rotearia de novo ao abrir.</summary>
    public class EntradaSceneTests
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
        public void Montar_PoeARotaNaCena_LigadaAoInputDoPopulate()
        {
            BootstrapSceneBuilder.Populate();
            EntradaSceneSetup.Montar();

            EntryFlow[] rotas = Object.FindObjectsByType<EntryFlow>(FindObjectsSortMode.None);
            Assert.AreEqual(1, rotas.Length, "a Bootstrap precisa de exatamente uma rota de entrada");
            Object input = new SerializedObject(rotas[0]).FindProperty("input").objectReferenceValue;
            Assert.IsNotNull(input, "rota sem o input: a tela de nascimento nao conseguiria travar o toque do jogo");
            Assert.AreSame(Object.FindFirstObjectByType<PlayerInputReader>(), input);
        }

        /// <summary>B01: o Limiar e um palco na propria Bootstrap, longe da area de treino e desligado ate a tela abrir:
        /// camera propria por cima da do jogo, fundo Azul profundo (GDD, arcano) e o simbolo da Trama enquadrado.</summary>
        [Test]
        public void Montar_PoeOPalcoDoLimiar_ComCameraEOSimboloEnquadrado()
        {
            BootstrapSceneBuilder.Populate();
            EntradaSceneSetup.Montar();

            EntryFlow rota = Object.FindFirstObjectByType<EntryFlow>();
            var palco = new SerializedObject(rota).FindProperty("limiar").objectReferenceValue as GameObject;
            Assert.IsNotNull(palco, "rota sem o palco do Limiar");
            Assert.IsFalse(palco.activeSelf, "palco ligado roubaria a tela de quem ja nasceu");

            Camera cam = palco.GetComponentInChildren<Camera>(true);
            Assert.IsNotNull(cam);
            Assert.Greater(cam.depth, Camera.main.depth, "a camera do Limiar desenha por cima da do jogo");
            Assert.AreEqual(CameraClearFlags.SolidColor, cam.clearFlags);
            Assert.IsNull(palco.GetComponentInChildren<AudioListener>(true), "segundo AudioListener: aviso no console");

            Transform simbolo = palco.transform.Find(EntradaSceneSetup.NomeSimbolo);
            Assert.IsNotNull(simbolo, "sem o simbolo da Trama no palco");
            Vector3 v = cam.WorldToViewportPoint(simbolo.position + Vector3.up * 0.75f);
            Assert.Greater(v.z, 0f, "simbolo atras da camera");
            Assert.That(v.x, Is.InRange(0.3f, 0.7f), "simbolo fora do centro");
            Assert.That(v.y, Is.InRange(0.35f, 0.9f), "simbolo escondido pelo painel de fala");
            Assert.Less(palco.transform.position.y, -100f, "palco a vista da area de treino");
        }

        [Test]
        public void PopulateSozinho_NaoPoeARota()
        {
            BootstrapSceneBuilder.Populate();
            Assert.AreEqual(0, Object.FindObjectsByType<EntryFlow>(FindObjectsSortMode.None).Length,
                "Populate e o chassi de Auren: com a rota, Auren reabriria o nascimento ou recarregaria a cena");
        }
    }
}
