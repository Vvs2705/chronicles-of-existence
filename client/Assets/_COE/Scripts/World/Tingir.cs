using UnityEngine;

namespace COE
{
    /// <summary>Variante por cor (ASSETS.md §1 e §2: "variantes por troca de material, sem gerar de novo"): multiplica a cor
    /// base de todo renderer filho por <see cref="cor"/> ao acordar, por MaterialPropertyBlock, sem copiar material nem
    /// sujar o asset. Casa de fundo e morador usam o mesmo prototipo do elenco com outro tom.
    /// ponytail: o bloco tira o renderer do SRP Batcher; com dezenas de variantes vale mais um material por tom.</summary>
    public class Tingir : MonoBehaviour
    {
        [SerializeField] Color cor = Color.white;

        public Color Cor { get { return cor; } }

        void Awake() { Aplicar(); }

        public void Aplicar()
        {
            if (cor == Color.white) return;
            var bloco = new MaterialPropertyBlock();
            foreach (Renderer r in GetComponentsInChildren<Renderer>(true))
            {
                r.GetPropertyBlock(bloco);
                bloco.SetColor("_BaseColor", cor);
                r.SetPropertyBlock(bloco);
            }
        }
    }
}
