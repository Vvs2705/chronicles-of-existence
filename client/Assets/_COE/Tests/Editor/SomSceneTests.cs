using COE.EditorTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace COE.EditorTests
{
    /// <summary>O som da cena (SomSceneSetup): os sons novos chegam a quem faz o evento (menu, salto, descanso, treino,
    /// parceiro), o jogador apanha com baque e o boneco com madeira, e a trilha sabe de quem medir o bosque e de onde.
    /// Cena nova em memoria por teste.</summary>
    public class SomSceneTests
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

        static T Um<T>() where T : Object { return Object.FindAnyObjectByType<T>(FindObjectsInactive.Include); }

        [Test]
        public void SonsDeEvento_LigadosNoSomDaCena()
        {
            SomDoJogo som = Um<SomDoJogo>();
            Assert.IsNotNull(som);
            var ligados = new Object[] { Um<MenuDePausa>(), Um<SaltoHud>(), Um<Descanso>(), Um<TreinoHud>(), Um<TrainingDummy>(), Um<GanchoHud>(), Um<QuestTrigger>() };
            foreach (Object o in ligados)
            {
                Assert.IsNotNull(o, "a cena perdeu um componente que soa");
                Assert.AreSame(som, new SerializedObject(o).FindProperty(SomSceneSetup.Campo).objectReferenceValue, o.GetType().Name + " mudo");
            }
        }

        [Test]
        public void Jogador_ApanhaComBaque_EOParceiroComMadeira()
        {
            HitFlash jogador = AurenSceneBuilder.Achar("Player").GetComponent<HitFlash>();
            HitFlash parceiro = AurenSceneBuilder.Achar("ParceiroDeTreino").GetComponent<HitFlash>();
            Assert.AreEqual((int)Som.DanoRecebido, new SerializedObject(jogador).FindProperty("somDoAcerto").intValue);
            Assert.AreEqual((int)Som.Golpe, new SerializedObject(parceiro).FindProperty("somDoAcerto").intValue);
        }

        /// <summary>GDD 09.10: o motivo do Limiar volta ao tocar o simbolo da clareira (B11); os outros gatilhos dao o clique.</summary>
        [Test]
        public void TocarOSimbolo_SoaOMotivoDoLimiar()
        {
            int simbolo = 0;
            foreach (QuestTrigger q in Object.FindObjectsByType<QuestTrigger>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                var so = new SerializedObject(q);
                Assert.IsNotNull(so.FindProperty(SomSceneSetup.Campo).objectReferenceValue, q.name + " mudo");
                bool eOSimbolo = q.ObjetivoId == SomSceneSetup.ObjetivoDoSimbolo;
                if (eOSimbolo) simbolo++;
                Assert.AreEqual((int)(eOSimbolo ? Som.EcoDoLimiar : Som.Clique), so.FindProperty("somAoCumprir").intValue, q.name);
            }
            Assert.AreEqual(1, simbolo, "um gatilho de tocar o simbolo (q08)");
        }

        [Test]
        public void Trilha_MedeOBosqueDoJogador_AntesDaEntrada()
        {
            var so = new SerializedObject(Um<SomDoJogo>());
            Assert.AreSame(AurenSceneBuilder.Achar("Player").transform, so.FindProperty("jogador").objectReferenceValue);
            float z = so.FindProperty("zDoBosque").floatValue;
            float entrada = AurenSceneBuilder.PosicaoDaAncora("entrada_bosque").z;
            Assert.That(z, Is.InRange(entrada - 8f, entrada), "o bosque comeca a soar perto do vao das arvores");
            Assert.Greater(z, AurenSceneBuilder.PosicaoDaAncora("praca_centro").z + 20f, "a praca nao e bosque");
            Transform coracao = AurenSceneBuilder.Achar(AurenSceneBuilder.RaizAncoras).transform.Find(SomSceneSetup.AncoraDoCoracao);
            Assert.AreSame(coracao, so.FindProperty("coracao").objectReferenceValue, "o Enredado mede a distancia ao coracao do bosque");
        }
    }
}
