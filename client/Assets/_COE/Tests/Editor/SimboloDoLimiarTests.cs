using COE.EditorTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace COE.EditorTests
{
    /// <summary>T012 (slice §4.1): em Auren o salto e oferecido so no simbolo do Limiar da clareira, ligado ao SaltoHud
    /// nos dois sentidos; o botao do topo so existe em cena sem simbolo (Bootstrap).</summary>
    public class SimboloDoLimiarTests
    {
        [SetUp]
        public void CenaNova() { EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single); }

        [Test]
        public void Auren_SimboloNaClareira_NasceDesligado_ELigadoAoSaltoHud()
        {
            AurenSceneBuilder.Populate();

            SaltoGatilho[] gatilhos = Object.FindObjectsByType<SaltoGatilho>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            Assert.AreEqual(1, gatilhos.Length, "um simbolo so");
            SaltoGatilho g = gatilhos[0];
            Assert.IsFalse(g.enabled, "nasce desligado: o SaltoHud liga quando o salto e liberado");
            Assert.Less(Vector3.Distance(g.transform.position, AurenSceneBuilder.PosicaoDaAncora("bosque_clareira")), 3f);

            SaltoHud hud = Object.FindAnyObjectByType<SaltoHud>();
            Assert.AreSame(g, new SerializedObject(hud).FindProperty("gatilho").objectReferenceValue);
            Assert.AreSame(hud, new SerializedObject(g).FindProperty("hud").objectReferenceValue);
        }

        [Test]
        public void Bootstrap_SemSimbolo_OSaltoHudFicaComOBotaoDoTopo()
        {
            BootstrapSceneBuilder.Populate();

            Assert.AreEqual(0, Object.FindObjectsByType<SaltoGatilho>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length);
            SaltoHud hud = Object.FindAnyObjectByType<SaltoHud>();
            Assert.IsNull(new SerializedObject(hud).FindProperty("gatilho").objectReferenceValue);
        }
    }
}
