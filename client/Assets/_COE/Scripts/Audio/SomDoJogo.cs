using UnityEngine;

namespace COE
{
    /// <summary>Toca o som do prototipo (Sintese): a musica em laco e os efeitos. Quem faz barulho recebe ESTE objeto por
    /// campo serializado (ligado pelo gerador de cena) e chama Tocar; sem ele, fica mudo (nada quebra).
    /// A noite a caixinha fica mais lenta, grave e baixa (pitch 0,8): o mesmo periodo que pinta a LuzDoDia.
    /// ponytail: clips gerados no Awake (~20 s de musica a 22 kHz, mono). Volume do jogador e a T013 (menu).</summary>
    public class SomDoJogo : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] float volumeMusica = 0.3f;
        [SerializeField, Range(0f, 1f)] float volumeEfeitos = 0.8f;

        AudioSource musica, efeitos;
        AudioClip[] clips;

        void Awake()
        {
            clips = new AudioClip[System.Enum.GetValues(typeof(Som)).Length];
            for (int i = 0; i < clips.Length; i++) clips[i] = Clip(((Som)i).ToString(), Sintese.Efeito((Som)i));

            efeitos = gameObject.AddComponent<AudioSource>();
            efeitos.playOnAwake = false;
            efeitos.spatialBlend = 0f;
            musica = gameObject.AddComponent<AudioSource>();
            musica.clip = Clip("Musica", Sintese.Musica());
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
            bool noite = TimeOfDayCycle.Atual(SaveState.Current.life) == TimeOfDay.Noite;
            float k = Time.unscaledDeltaTime / LuzDoDia.SegundosDeTroca;
            musica.pitch = Mathf.MoveTowards(musica.pitch, noite ? 0.8f : 1f, k);
            musica.volume = Mathf.MoveTowards(musica.volume, volumeMusica * (noite ? 0.7f : 1f), k * volumeMusica);
        }
    }
}
