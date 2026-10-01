using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace COE.EditorTools
{
    /// <summary>Mede a velocidade natural de um clip de locomocao "no lugar" num modelo Humanoid numa altura dada: o pe
    /// apoiado (o mais baixo) anda para tras na velocidade da passada. Se o corpo anda em outra velocidade, o pe desliza.
    /// C# puro na conta (<see cref="VelocidadeDoApoio"/>), Unity so para amostrar os ossos.</summary>
    public static class PassadaMedida
    {
        /// <summary>Amostras (tempo; pe esquerdo x, y, z; pe direito x, y, z), em metros no espaco da raiz: velocidade horizontal
        /// do pe apoiado (m/s), mediana. Horizontal e nao "para tras": o eixo de frente do modelo do Tripo varia. 0 se nao
        /// ha apoio medivel.</summary>
        public static float VelocidadeDoApoio(IList<(float T, float XE, float YE, float ZE, float XD, float YD, float ZD)> a)
        {
            var v = new List<float>();
            for (int i = 1; i < a.Count; i++)
            {
                float dt = a[i].T - a[i - 1].T;
                if (dt <= 0f) continue;
                bool esqApoia = a[i].YE <= a[i].YD;   // o pe mais baixo e o que esta no chao
                if (esqApoia != (a[i - 1].YE <= a[i - 1].YD)) continue;   // troca de apoio neste passo: nao mede
                float dx = esqApoia ? a[i].XE - a[i - 1].XE : a[i].XD - a[i - 1].XD;
                float dz = esqApoia ? a[i].ZE - a[i - 1].ZE : a[i].ZD - a[i - 1].ZD;
                float vel = (float)System.Math.Sqrt(dx * dx + dz * dz) / dt;
                if (vel > 0f) v.Add(vel);
            }
            if (v.Count == 0) return 0f;
            v.Sort();
            return v[v.Count / 2];
        }

        /// <summary>Toca o clip no modelo em altura-alvo e mede. Editor, sem Play Mode (PlayableGraph avaliado a mao).</summary>
        public static float Medir(GameObject modeloFonte, AnimationClip clip, float altura)
        {
            GameObject go = Object.Instantiate(modeloFonte);
            try
            {
                Bounds b = new Bounds(go.transform.position, Vector3.zero);
                foreach (Renderer r in go.GetComponentsInChildren<Renderer>()) b.Encapsulate(r.bounds);
                if (b.size.y > 0f) go.transform.localScale *= altura / b.size.y;

                Animator an = go.GetComponentInChildren<Animator>();
                an.applyRootMotion = false;
                Transform raiz = go.transform;
                Transform esq = an.GetBoneTransform(HumanBodyBones.LeftFoot), dir = an.GetBoneTransform(HumanBodyBones.RightFoot);

                // AnimationMode posiciona o esqueleto Humanoid em modo de edicao (o PlayableGraph avaliado a mao dispara os
                // eventos mas nao escreve os ossos fora do Play Mode).
                bool jaEstava = AnimationMode.InAnimationMode();
                if (!jaEstava) AnimationMode.StartAnimationMode();
                var a = new List<(float, float, float, float, float, float, float)>();
                try
                {
                    for (float t = 0f; t <= clip.length; t += 1f / 60f)
                    {
                        AnimationMode.BeginSampling();
                        AnimationMode.SampleAnimationClip(an.gameObject, clip, t);
                        AnimationMode.EndSampling();
                        Vector3 e = raiz.InverseTransformPoint(esq.position) * raiz.localScale.y;
                        Vector3 d = raiz.InverseTransformPoint(dir.position) * raiz.localScale.y;
                        a.Add((t, e.x, e.y, e.z, d.x, d.y, d.z));
                    }
                }
                finally { if (!jaEstava) AnimationMode.StopAnimationMode(); }
                float ve = VelocidadeDoApoio(a);
                if (ve <= 0.01f)
                    Debug.LogWarning("PassadaMedida: " + clip.name + " sem movimento de pe medivel (" + a.Count + " amostras; pe esq. z "
                        + a.Min(x => x.Item4).ToString("F3") + ".." + a.Max(x => x.Item4).ToString("F3") + ")");
                return ve;
            }
            finally { Object.DestroyImmediate(go); }
        }

        /// <summary>Batch: -executeMethod COE.EditorTools.PassadaMedida.Relatorio. Loga a velocidade natural de Walk/Run
        /// na protagonista aos 5 anos (1,10 m).</summary>
        public static void Relatorio()
        {
            GameObject modelo = Prototipos.CarregarDoDisco("protagonista");
            foreach (string arq in new[] { "Walking", "Running" })
            {
                AnimationClip c = AssetDatabase.LoadAllAssetsAtPath(PrototipoAnimacoes.Pasta + "/" + arq + ".fbx")
                    .OfType<AnimationClip>().FirstOrDefault(x => !x.name.StartsWith("__preview__", System.StringComparison.Ordinal));
                if (modelo == null || c == null) { Debug.Log("PASSADA " + arq + ": sem modelo ou clip"); continue; }
                Debug.Log("PASSADA " + arq + " (clip " + c.name + ", " + c.length.ToString("F2") + " s): "
                    + Medir(modelo, c, BodyScale.Crianca5).ToString("F2") + " m/s a 1,10 m");
            }
        }
    }
}
