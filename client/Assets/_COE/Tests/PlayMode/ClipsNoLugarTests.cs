using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace COE.PlayModeTests
{
    /// <summary>ADR-0008: o jogo nao usa root motion (esquiva e golpe sao no lugar). Clip do Mixamo com o recuo assado na
    /// pose levava o corpo ~1 m para longe do Player na esquiva (captura de 2026-10-01): camera, colisor e alvo ficavam
    /// para tras. Precisa de quadros: o Animator so escreve os ossos em Play Mode.</summary>
    public class ClipsNoLugarTests
    {
        const string Modelo = "Assets/_COE/Art/Prototipo/Personagens/protagonista/protagonista.fbx";
        const string Controller = "Assets/_COE/Art/Prototipo/Animacoes/Prototipo.controller";
        GameObject go;

        [TearDown] public void Limpar() { if (go != null) Object.Destroy(go); }

        [UnityTest]
        public IEnumerator EsquivaGolpeEQueda_OCorpoNaoSaiDeCimaDoPlayer()
        {
#if UNITY_EDITOR
            var fonte = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(Modelo);
            var ctrl = UnityEditor.AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(Controller);
#else
            GameObject fonte = null; RuntimeAnimatorController ctrl = null;
#endif
            if (fonte == null || ctrl == null) Assert.Inconclusive("sem prototipo da protagonista ou Prototipo.controller");

            go = Object.Instantiate(fonte);
            Bounds b = new Bounds(go.transform.position, Vector3.zero);
            foreach (Renderer r in go.GetComponentsInChildren<Renderer>()) b.Encapsulate(r.bounds);
            go.transform.localScale *= BodyScale.Crianca5 / b.size.y;
            Animator an = go.GetComponentInChildren<Animator>();
            an.applyRootMotion = false;   // como no jogo: quem move e o CharacterController
            an.cullingMode = AnimatorCullingMode.AlwaysAnimate;   // teste sem camera: com culling os ossos nao se mexem
            an.runtimeAnimatorController = ctrl;
            CharacterAnimator ca = go.AddComponent<CharacterAnimator>();   // recebe OnFootstep/OnHitFrame, como no Player
            Transform quadril = an.GetBoneTransform(HumanBodyBones.Hips);
            yield return null;

            var lances = new (string Nome, System.Action Disparar)[]
                { ("esquiva", ca.Dodge), ("golpe recebido", ca.Hit), ("queda", () => ca.Dead(true)) };
            foreach (var (gatilho, disparar) in lances)
            {
                disparar();
                float horizontal = 0f, yMin = float.MaxValue, yMax = float.MinValue;
                for (float fim = Time.time + 1.4f; Time.time < fim; )
                {
                    yield return null;
                    Vector3 q = go.transform.InverseTransformPoint(quadril.position) * go.transform.localScale.y;
                    horizontal = Mathf.Max(horizontal, new Vector2(q.x, q.z).magnitude);
                    yMin = Mathf.Min(yMin, q.y);
                    yMax = Mathf.Max(yMax, q.y);
                }
                Assert.Greater(yMax - yMin, 0.02f, gatilho + ": o quadril nem se mexeu, o clip nao tocou (medida vazia)");
                Assert.Less(horizontal, 0.3f, gatilho + " leva o quadril a " + horizontal.ToString("F2") + " m do Player");
            }
        }
    }
}
