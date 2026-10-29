using UnityEngine;

namespace COE
{
    /// <summary>O fundo de Auren: vento e passarinho de dia, grilo e coruja a noite (Sintese.Ambiente), em laco, 2D, por baixo
    /// da caixinha. Quando o periodo vira, um cruza com o outro no mesmo tempo da luz (LuzDoDia.SegundosDeTroca). Mudo com o
    /// som desligado no menu, como tudo (AudioListener.volume).
    /// ponytail: clips gerados uma vez por processo e guardados, como os do SomDoJogo.</summary>
    public class SomAmbiente : MonoBehaviour
    {
        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo gerador (PartidaSetup); vazio = a do SaveState.")]
        [SerializeField] Partida partida;
        [SerializeField, Range(0f, 1f)] float volume = 0.35f;

        static AudioClip clipDia, clipNoite;
        AudioSource dia, noite;
        bool primeiro = true;

        void Awake()
        {
            if (clipDia == null) clipDia = ClipsDoMundo.Criar("AmbienteDia", Sintese.Ambiente(false));
            if (clipNoite == null) clipNoite = ClipsDoMundo.Criar("AmbienteNoite", Sintese.Ambiente(true));
            dia = Fonte(clipDia);
            noite = Fonte(clipNoite);
        }

        AudioSource Fonte(AudioClip clip)
        {
            AudioSource a = gameObject.AddComponent<AudioSource>();
            a.clip = clip;
            a.loop = true;
            a.spatialBlend = 0f;
            a.volume = 0f;
            a.playOnAwake = false;
            a.Play();
            return a;
        }

        void Update()
        {
            SaveData save = Partida.De(partida).Save;
            bool eNoite = TimeOfDayCycle.Atual(save == null ? null : save.life) == TimeOfDay.Noite;
            float k = primeiro ? 1f : Time.unscaledDeltaTime / LuzDoDia.SegundosDeTroca * volume;
            primeiro = false;
            dia.volume = Mathf.MoveTowards(dia.volume, eNoite ? 0f : volume, k);
            noite.volume = Mathf.MoveTowards(noite.volume, eNoite ? volume : 0f, k);
        }
    }
}
