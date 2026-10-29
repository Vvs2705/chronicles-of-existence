using UnityEngine;

namespace COE
{
    /// <summary>Fumaca de chamine: baforadas (os filhos) que nascem pequenas no topo, sobem, engordam, o vento leva para o
    /// lado e somem. Aparece so nos periodos de <see cref="periodos"/> (fogao de manha, lareira a noite). Sem colisor, fora
    /// do save, deterministica pelo tempo e pela fase de cada casa.
    /// ponytail: esferas opacas que crescem e encolhem, no estilo greybox; fumaca de particula com transparencia entra com o
    /// VFX do ADR-0002 (e o custo de overdraw medido no aparelho).</summary>
    public class Fumaca : MonoBehaviour
    {
        public const float Subida = 4.5f;
        public const float Duracao = 7f;
        /// <summary>Baforadas por chamine: com cinco a coluna ficava em tufos soltos e mal aparecia de longe.</summary>
        public const int Baforadas = 8;
        const float Vento = 1.6f;

        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo gerador (PartidaSetup); vazio = a do SaveState.")]
        [SerializeField] Partida partida;
        [Tooltip("Periodos com fogo aceso: bit 0 manha, 1 tarde, 2 noite.")]
        [SerializeField] int periodos = 5;
        [Tooltip("Segundos de avanco: duas chamines nao soltam a baforada juntas.")]
        [SerializeField] float fase;

        float t;

        /// <summary>REGRA PURA: onde esta (relativo ao topo da chamine) e de que tamanho e a baforada i de n no tempo 'tempo'.
        /// Cada uma faz o ciclo de <see cref="Duracao"/> s, defasada das outras; nasce e some com tamanho zero.</summary>
        public static void Baforada(int i, int n, float tempo, out Vector3 desvio, out float escala)
        {
            float u = Mathf.Repeat(tempo / Duracao + i / (float)Mathf.Max(1, n), 1f);
            desvio = new Vector3(Vento * u * u, Subida * u, 0.25f * Mathf.Sin(u * 6f + i));
            escala = (0.5f + 1.6f * u) * Mathf.Min(1f, u * 8f) * (1f - u * u * u * u);
        }

        void Update()
        {
            SaveData save = Partida.De(partida).Save;
            Atualizar(TimeOfDayCycle.Atual(save == null ? null : save.life), Time.deltaTime);
        }

        /// <summary>Um passo da fumaca. Publico porque Update nao roda em teste de Editor.</summary>
        public void Atualizar(TimeOfDay agora, float dt)
        {
            bool acesa = Morador.NoPeriodo(periodos, agora);
            int n = transform.childCount;
            t += dt;
            for (int i = 0; i < n; i++)
            {
                Transform b = transform.GetChild(i);
                if (b.gameObject.activeSelf != acesa) b.gameObject.SetActive(acesa);
                if (!acesa) continue;
                Vector3 desvio;
                float escala;
                Baforada(i, n, t + fase, out desvio, out escala);
                b.localPosition = desvio;
                b.localScale = Vector3.one * Mathf.Max(0.001f, escala);
            }
        }
    }
}
