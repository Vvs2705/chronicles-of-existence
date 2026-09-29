using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace COE.PlayModeTests
{
    /// <summary>Integracao com mini-cena montada em codigo: o que os EditMode de regra pura nao veem
    /// (relay de AnimationEvent no modelo filho e fallback do golpe pendente).</summary>
    public class CombatIntegrationTests
    {
        readonly List<GameObject> spawned = new List<GameObject>();

        GameObject Spawn(string name, Vector3 pos)
        {
            var go = new GameObject(name);
            go.transform.position = pos;
            spawned.Add(go);
            return go;
        }

        [TearDown]
        public void TearDown()
        {
            foreach (GameObject go in spawned) if (go != null) Object.Destroy(go);
            spawned.Clear();
        }

        /// <summary>Como o prefab importado: CharacterAnimator na raiz, Animator no modelo filho (sem controller).</summary>
        CharacterAnimator Rig()
        {
            GameObject root = Spawn("Rig", Vector3.zero);
            var model = new GameObject("Model");
            model.transform.SetParent(root.transform, false);
            model.AddComponent<Animator>();
            var ca = root.AddComponent<CharacterAnimator>(); // Awake roda aqui: o filho ja existe
            ca.immediate = false; // espera o OnHitFrame do clip
            return ca;
        }

        [UnityTest]
        public IEnumerator OnHitFrameNoModeloFilho_EntregaOGolpeUmaVez()
        {
            CharacterAnimator ca = Rig();
            GameObject model = ca.transform.GetChild(0).gameObject;
            Assert.IsNotNull(model.GetComponent<AnimEventRelay>(), "Awake deve por o relay no GameObject do Animator");

            int hits = 0;
            ca.Attack(0, delegate { hits++; });
            Assert.AreEqual(0, hits, "immediate=false: espera o evento");
            model.SendMessage(AnimParams.EventHitFrame); // o que o Unity faz com o AnimationEvent do clip; sem receptor = erro no log
            Assert.AreEqual(1, hits, "evento no filho chega ao CharacterAnimator da raiz");

            yield return new WaitForSeconds(0.8f); // passa do fallback (0,6 s)
            Assert.AreEqual(1, hits, "fallback nao reentrega golpe ja entregue");
        }

        [UnityTest]
        public IEnumerator SemOnHitFrame_GolpeSaiNoFallback()
        {
            CharacterAnimator ca = Rig();
            int hits = 0;
            ca.Attack(0, delegate { hits++; });

            yield return new WaitForSeconds(0.2f); // folga para 1 frame de ate 0,33 s (maximumDeltaTime) sem passar de 0,6
            Assert.AreEqual(0, hits, "antes do fallback nao sai");
            yield return new WaitForSeconds(0.8f);
            Assert.AreEqual(1, hits, "clip sem evento: o golpe sai no fallback, uma vez");
        }
    }
}
