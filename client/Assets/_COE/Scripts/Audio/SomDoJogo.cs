using UnityEngine;

namespace COE
{
    /// <summary>Toca o som do prototipo (Sintese): a musica em laco e os efeitos. Quem faz barulho recebe ESTE objeto por
    /// campo serializado (ligado pelo gerador de cena) e chama Tocar; sem ele, fica mudo (nada quebra).
    /// A noite a caixinha fica mais lenta, grave e baixa (pitch 0,8): o mesmo periodo que pinta a LuzDoDia.
    /// ponytail: clips gerados uma vez por processo (~20 s de musica a 22 kHz, mono) e guardados como os sprites do Tela:
    /// a troca de cena (salto, entrada) nao sintetiza de novo. Volume do jogador e a T013 (menu).</summary>
    public class SomDoJogo : MonoBehaviour
    {
        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo gerador (PartidaSetup); vazio = a do SaveState.")]
        [SerializeField] Partida partida;
        [SerializeField, Range(0f, 1f)] float volumeMusica = 0.3f;
        [SerializeField, Range(0f, 1f)] float volumeEfeitos = 0.8f;

        AudioSource musica, efeitos;
        static AudioClip[] clips;
        static AudioClip clipMusica;

        void Awake()
        {
            if (clipMusica == null)   // null do Unity tambem cobre clip destruido
            {
                clips = new AudioClip[System.Enum.GetValues(typeof(Som)).Length];
                for (int i = 0; i < clips.Length; i++) clips[i] = Clip(((Som)i).ToString(), Sintese.Efeito((Som)i));
                clipMusica = Clip("Musica", Sintese.Musica());
            }

            efeitos = gameObject.AddComponent<AudioSource>();
            efeitos.playOnAwake = false;
            efeitos.spatialBlend = 0f;
            musica = gameObject.AddComponent<AudioSource>();
            musica.clip = clipMusica;
            musica.loop = true;
            musica.spatialBlend = 0f;
            musica.volume = volumeMusica;
            musica.Play();
        }

        static AudioClip Clip(string nome, float[] amostras)
        {
            AudioClip c = AudioClip.Create(nome, amostras.Length, 1, Sintese.Taxa, false);
            c.SetData(amostras, 0);
            return c;
        }

        public void Tocar(Som som, float volume = 1f)
        {
            if (efeitos != null) efeitos.PlayOneShot(clips[(int)som], volume * volumeEfeitos);
        }

        void Update()
        {
            if (musica == null) return;
            bool noite = TimeOfDayCycle.Atual(Partida.De(partida).Save.life) == TimeOfDay.Noite;
            float k = Time.unscaledDeltaTime / LuzDoDia.SegundosDeTroca;
            musica.pitch = Mathf.MoveTowards(musica.pitch, noite ? 0.8f : 1f, k);
            musica.volume = Mathf.MoveTowards(musica.volume, volumeMusica * (noite ? 0.7f : 1f), k * volumeMusica);
        }
    }
}
