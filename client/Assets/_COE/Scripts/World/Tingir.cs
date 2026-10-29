using System.Collections.Generic;
using UnityEngine;

namespace COE
{
    /// <summary>Variante por cor (ASSETS.md §1 e §2: "variantes por troca de material, sem gerar de novo"): poe
    /// <see cref="cor"/> na cor base de todo renderer filho ao acordar, sem sujar o asset. Casa de fundo e morador usam o
    /// mesmo prototipo do elenco com outro tom.
    /// Um material por (material de origem, tom), compartilhado por quem pede o mesmo par: MaterialPropertyBlock tirava cada
    /// renderer do SRP Batcher (dezesseis casas e nove moradores = dezenas de draw calls fora do lote).
    /// ponytail: a copia nasce em runtime e vive no cache estatico ate o Unity descarregar (UnloadUnusedAssets a destroi;
    /// o cache refaz). Muitas variantes publicadas pedem material de tom como asset, gerado no Editor.</summary>
    public class Tingir : MonoBehaviour
    {
        [SerializeField] Color cor = Color.white;

        public Color Cor { get { return cor; } }

        static readonly Dictionary<(Material, Color), Material> tons = new Dictionary<(Material, Color), Material>();

        void Awake() { Aplicar(); }

        public void Aplicar()
        {
            if (cor == Color.white) return;
            foreach (Renderer r in GetComponentsInChildren<Renderer>(true))
            {
                Material[] ms = r.sharedMaterials;
                for (int i = 0; i < ms.Length; i++) ms[i] = Tom(ms[i], cor);
                r.sharedMaterials = ms;
            }
        }

        /// <summary>A copia de 'origem' com a cor base 'cor', a mesma para o mesmo par. Publico para o teste.</summary>
        public static Material Tom(Material origem, Color cor)
        {
            if (origem == null || !origem.HasProperty("_BaseColor")) return origem;
            Material m;
            if (tons.TryGetValue((origem, cor), out m) && m != null) return m;
            m = new Material(origem) { name = origem.name + "_tom" };
            m.SetColor("_BaseColor", cor);
            tons[(origem, cor)] = m;
            return m;
        }
    }
}
