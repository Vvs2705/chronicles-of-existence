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

        [Test]
        public void PopulateSozinho_NaoPoeARota()
        {
            BootstrapSceneBuilder.Populate();
            Assert.AreEqual(0, Object.FindObjectsByType<EntryFlow>(FindObjectsSortMode.None).Length,
                "Populate e o chassi de Auren: com a rota, Auren reabriria o nascimento ou recarregaria a cena");
        }
    }
}
