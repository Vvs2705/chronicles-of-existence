using COE.EditorTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace COE.EditorTests
{
    /// <summary>GuiaSetup: o tutorial guiado nasce com o menu de pausa (Bootstrap e Auren) e, em Auren, acende o cartao de
    /// missao e a seta de objetivo. Tudo por campo serializado (nada de Find em runtime).</summary>
    public class GuiaSceneTests
    {
        [SetUp]
        public void Abrir()
        {
            Scene s = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SceneManager.SetActiveScene(s);
        }

        [Test]
        public void Auren_GuiaLigadoNoLeitorMenuMissaoESeta()
        {
            AurenSceneBuilder.Populate();
            GuiaHud guia = Object.FindFirstObjectByType<GuiaHud>();
            Assert.IsNotNull(guia, "sem GuiaHud na cena");
            Assert.AreEqual(GuiaSetup.Nome, guia.gameObject.name);
            var so = new SerializedObject(guia);
            foreach (string campo in new[] { "leitor", "menu", "interacao", "combate", "missao", "indicador", "partida" })
                Assert.IsNotNull(so.FindProperty(campo).objectReferenceValue, "campo '" + campo + "' vazio");
            Assert.Greater(so.FindProperty("controle").arraySize, 0, "sem a lista do que precisa estar ligado");
        }
    }
}
