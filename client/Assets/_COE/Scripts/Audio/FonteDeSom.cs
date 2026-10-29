using UnityEngine;

namespace COE
{
    /// <summary>Clips do som de lugar (SomDoMundo), gerados uma vez por processo e guardados (troca de cena nao sintetiza de
    /// novo), como os do SomDoJogo.</summary>
    public static class ClipsDoMundo
    {
        static AudioClip[] clips;

        public static AudioClip Clip(SomDoMundo som)
        {
            if (clips == null) clips = new AudioClip[System.Enum.GetValues(typeof(SomDoMundo)).Length];
            int i = (int)som;
            if (clips[i] == null) clips[i] = Criar(som.ToString(), Sintese.DoMundo(som));   // null do Unity cobre destruido
            return clips[i];
        }

        public static AudioClip Criar(string nome, float[] amostras)
        {
            AudioClip c = AudioClip.Create(nome, amostras.Length, 1, Sintese.Taxa, false);
            c.SetData(amostras, 0);
            return c;
        }
    }

    /// <summary>Um lugar que soa (3D): a bigorna bate de dia na porta da ferraria, o balde cai no poco de vez em quando, o
    /// bosque sussurra sem parar na entrada, a galinha cacareja, o cachorro late. Laco = toca continuo; senao, um disparo a
    /// cada intervalo sorteado entre min e max, com o tom variando um pouco para nao soar carimbo. Fora dos periodos da
    /// mascara, cala (o laco desce o volume devagar).
    /// ponytail: Random do Unity (decoracao, nao regra: nao precisa repetir igual). Distancia linear de alcance curto: a vila
    /// e pequena e o celular toca em alto-falante.</summary>
    public class FonteDeSom : MonoBehaviour
    {
        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo gerador (PartidaSetup); vazio = a do SaveState.")]
        [SerializeField] Partida partida;
        [SerializeField] SomDoMundo som;
        [SerializeField] bool laco;
        [SerializeField] float intervaloMin = 4f;
        [SerializeField] float intervaloMax = 10f;
        [SerializeField, Range(0f, 1f)] float volume = 0.7f;
        [SerializeField] float alcance = 18f;
        [Tooltip("Periodos em que soa: bit 0 manha, 1 tarde, 2 noite.")]
        [SerializeField] int periodos = 7;

        AudioSource fonte;
        float espera;

        public SomDoMundo Som { get { return som; } }

        void Awake()
        {
            fonte = gameObject.AddComponent<AudioSource>();
            fonte.spatialBlend = 1f;
            fonte.rolloffMode = AudioRolloffMode.Linear;
            fonte.minDistance = 1.5f;
            fonte.maxDistance = alcance;
            fonte.dopplerLevel = 0f;
            fonte.playOnAwake = false;
            fonte.volume = laco ? 0f : volume;
            if (laco)
            {
                fonte.clip = ClipsDoMundo.Clip(som);
                fonte.loop = true;
                fonte.time = Random.Range(0f, fonte.clip.length * 0.9f);   // duas fontes iguais nao sussurram em coro
                fonte.Play();
            }
            espera = Random.Range(0f, intervaloMax);
        }

        void Update()
        {
            SaveData save = Partida.De(partida).Save;
            bool ativo = Morador.NoPeriodo(periodos, TimeOfDayCycle.Atual(save == null ? null : save.life));
            if (laco)
            {
                fonte.volume = Mathf.MoveTowards(fonte.volume, ativo ? volume : 0f, Time.deltaTime / LuzDoDia.SegundosDeTroca);
                return;
            }
            if (!ativo) return;
            espera -= Time.deltaTime;
            if (espera > 0f) return;
            espera = Random.Range(intervaloMin, Mathf.Max(intervaloMin, intervaloMax));
            fonte.pitch = Random.Range(0.92f, 1.08f);
            fonte.PlayOneShot(ClipsDoMundo.Clip(som), volume);
        }
    }
}
