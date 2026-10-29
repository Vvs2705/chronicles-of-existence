using UnityEngine;

namespace COE
{
    /// <summary>Um bando voando em roda (Revoada): passaros sobre os telhados, borboletas na horta. Cada filho direto e um
    /// membro; as asas sao os filhos "asa_e"/"asa_d" dele (montados pelo VidaSceneSetup). So decora: sem colisor, sem
    /// regra. Some nos periodos fora de <see cref="periodos"/> (passarinho dorme a noite).
    /// ponytail: um Update para o bando todo e transform por membro; dezenas de bichos pedem GPU instancing.</summary>
    public class Bando : MonoBehaviour
    {
        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo gerador (PartidaSetup); vazio = a do SaveState.")]
        [SerializeField] Partida partida;
        [SerializeField] float raio = 8f;
        [SerializeField] float altura = 2f;
        [SerializeField] float volta = 14f;
        [SerializeField] float batidas = 6f;
        [Tooltip("Periodos em que o bando aparece: bit 0 manha, 1 tarde, 2 noite.")]
        [SerializeField] int periodos = 3;

        Transform[] membros;
        Transform[] asaE, asaD;

        void Awake()
        {
            int n = transform.childCount;
            membros = new Transform[n];
            asaE = new Transform[n];
            asaD = new Transform[n];
            for (int i = 0; i < n; i++)
            {
                membros[i] = transform.GetChild(i);
                asaE[i] = membros[i].Find("asa_e");
                asaD[i] = membros[i].Find("asa_d");
            }
        }

        void Update()
        {
            SaveData save = Partida.De(partida).Save;
            bool visivel = Morador.NoPeriodo(periodos, TimeOfDayCycle.Atual(save == null ? null : save.life));
            float t = Time.time;
            for (int i = 0; i < membros.Length; i++)
            {
                if (membros[i].gameObject.activeSelf != visivel) membros[i].gameObject.SetActive(visivel);
                if (!visivel) continue;
                int n = membros.Length;
                membros[i].localPosition = Revoada.Posicao(i, n, t, raio, altura, volta);
                membros[i].localRotation = Quaternion.LookRotation(Revoada.Direcao(i, n, t, raio, altura, volta));
                float a = Revoada.Asa(i, t, batidas);
                if (asaE[i] != null) asaE[i].localRotation = Quaternion.Euler(0f, 0f, a);
                if (asaD[i] != null) asaD[i].localRotation = Quaternion.Euler(0f, 0f, -a);
            }
        }
    }
}
