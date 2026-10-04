using COE.EditorTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace COE.EditorTests
{
    /// <summary>Passo 6 (2026-10-04): em Auren gerada, todo componente com o campo `partida` aponta para a Partida do objeto
    /// Save. Sem isso ele cairia no SaveState (funciona, mas some a dependencia explicita e teste nao injeta sessao).</summary>
    public class PartidaSceneTests
    {
        [SetUp]
        public void Abrir()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AurenSceneBuilder.Populate();
        }

        [TearDown]
        public void Fechar() { EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single); }

        [Test]
        public void TodoCampoPartida_EstaLigadoNaPartidaDoSave()
        {
            Partida partida = AurenSceneBuilder.Achar("Save").GetComponent<Partida>();
            Assert.IsNotNull(partida, "objeto Save sem Partida");
            int ligados = 0;
            foreach (MonoBehaviour m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
            {
                SerializedProperty p = new SerializedObject(m).FindProperty(PartidaSetup.Campo);
                if (p == null || p.propertyType != SerializedPropertyType.ObjectReference) continue;
                Assert.AreEqual(partida, p.objectReferenceValue, m.GetType().Name + " em " + m.name + " sem a Partida da cena");
                ligados++;
            }
            Assert.Greater(ligados, 20, "NPCs, gatilhos, HUDs, corpo, luz e som recebem a Partida");
        }
    }
}
