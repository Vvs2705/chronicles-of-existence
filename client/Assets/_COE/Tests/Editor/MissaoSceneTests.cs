using System.Collections.Generic;
using COE.EditorTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace COE.EditorTests
{
    /// <summary>T012 (raia de missoes): AurenSceneBuilder.Populate monta, via MissaoSceneSetup, um QuestTrigger por
    /// objetivo sem NPC (MissaoMundo.Gatilhos) na ancora certa, desligado ate o MissaoHud ligar, sem collider, e o HUD
    /// com a lista de gatilhos ligada por campo (sem Find em runtime). Cena nova em memoria por teste.</summary>
    public class MissaoSceneTests
    {
        [SetUp]
        public void Abrir()
        {
            Scene s = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SceneManager.SetActiveScene(s);
            AurenSceneBuilder.Populate();
        }

        [TearDown]
        public void Fechar() { LogAssert.NoUnexpectedReceived(); }

        static QuestTrigger[] TodosOsGatilhos()
        {
            return AurenSceneBuilder.Achar(MissaoSceneSetup.Raiz).GetComponentsInChildren<QuestTrigger>(true);
        }

        [Test]
        public void UmGatilhoPorObjetivoSemNpc_NaAncoraCerta_DesligadoESemCollider()
        {
            QuestTrigger[] cena = TodosOsGatilhos();
            Assert.AreEqual(MissaoMundo.Gatilhos.Length, cena.Length, "gatilho a mais ou a menos na cena");

            foreach (string[] linha in MissaoMundo.Gatilhos)
            {
                QuestTrigger t = System.Array.Find(cena, x => x.QuestId == linha[0] && x.ObjetivoId == linha[1]);
                Assert.IsNotNull(t, "falta o gatilho " + linha[0] + "." + linha[1]);
                Vector3 d = t.transform.position - AurenSceneBuilder.PosicaoDaAncora(linha[2]);
                d.y = 0f;
                Assert.Less(d.magnitude, 0.01f, t.name + " fora da ancora " + linha[2]);
                Assert.IsFalse(t.gameObject.activeSelf, t.name + ": nasce desligado; o MissaoHud liga so o pendente");
                Assert.IsEmpty(t.GetComponentsInChildren<Collider>(true), t.name + ": collider barraria o percurso");
                Assert.IsNotNull(t.GetComponentInChildren<Renderer>(true), t.name + ": sem marcador visivel");
            }
        }

        [Test]
        public void Hud_TemTodosOsGatilhosLigadosPorCampo()
        {
            MissaoHud hud = AurenSceneBuilder.Achar(MissaoSceneSetup.Raiz).GetComponent<MissaoHud>();
            Assert.IsNotNull(hud, "raiz Missoes sem MissaoHud");

            SerializedProperty lista = new SerializedObject(hud).FindProperty("gatilhos");
            var ligados = new List<Object>();
            for (int i = 0; i < lista.arraySize; i++) ligados.Add(lista.GetArrayElementAtIndex(i).objectReferenceValue);

            CollectionAssert.AreEquivalent(TodosOsGatilhos(), ligados, "o HUD liga/desliga exatamente os gatilhos da cena");
            CollectionAssert.AllItemsAreNotNull(ligados);
        }
    }
}
