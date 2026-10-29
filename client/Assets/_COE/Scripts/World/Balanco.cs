using UnityEngine;

namespace COE
{
    /// <summary>Vento: o transform (copa de arvore, roupa no varal) balanca em volta do proprio pivo, numa onda lenta com uma
    /// rajada por cima. Sem colisor por baixo (quem tem colisor nao balanca: mover colisor estatico custa na fisica), fora do
    /// save, deterministico pelo tempo e pela fase.
    /// ponytail: um Update por peca (as 28 copas da vila e os panos dos varais). As arvores do Bosque continuam paradas e em
    /// lote estatico; vento para tudo de uma vez e no vertex shader do COE/Toon, com o custo medido no aparelho.</summary>
    public class Balanco : MonoBehaviour
    {
        [Tooltip("Inclinacao maxima, em graus.")]
        [SerializeField] float graus = 1.5f;
        [Tooltip("Segundos de uma ida e volta da onda principal.")]
        [SerializeField] float periodo = 4f;
        [Tooltip("Segundos de avanco: duas pecas vizinhas nao balancam juntas.")]
        [SerializeField] float fase;

        Quaternion repouso;

        void Awake() { repouso = transform.localRotation; }

        void Update() { transform.localRotation = repouso * Inclinacao(Time.time + fase, graus, periodo); }

        /// <summary>REGRA PURA: a inclinacao no tempo 'tempo' (graus no X e no Z). Onda principal no X (o vento vem de um lado
        /// so), rajada mais curta somada, e um pouco de Z para a peca nao ir e voltar num trilho. Cada eixo fica dentro de 'graus'.</summary>
        public static Quaternion Inclinacao(float tempo, float graus, float periodo)
        {
            float w = 2f * Mathf.PI / Mathf.Max(0.1f, periodo);
            float onda = 0.7f * Mathf.Sin(w * tempo) + 0.3f * Mathf.Sin(w * 2.7f * tempo + 1.3f);
            float lado = 0.35f * Mathf.Sin(w * 0.61f * tempo + 0.4f);
            return Quaternion.Euler(graus * onda, 0f, graus * lado);
        }
    }
}
