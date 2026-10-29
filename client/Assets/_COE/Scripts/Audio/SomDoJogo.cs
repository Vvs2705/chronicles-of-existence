using System.Threading.Tasks;
using UnityEngine;

namespace COE
{
    /// <summary>Toca o som do prototipo (Sintese): a trilha do momento em laco e os efeitos. Quem faz barulho recebe ESTE
    /// objeto por campo serializado (ligado pelo gerador de cena, SomSceneSetup) e chama Tocar; sem ele, fica mudo.
    /// A trilha sai da regra pura TrilhaDoMomento: a tela de entrada (o EntryFlow diz qual), o combate (quem bate ou apanha
    /// chama MarcarCombate), o bosque (jogador passou do z da entrada do bosque), o trecho do episodio 2 (festa, descida,
    /// Enredado, pelas missoes da sessao) e o periodo que pinta a LuzDoDia. Cada
    /// trilha tem a sua fonte; a da vez sobe e as outras descem e pausam (voltar a noite continua de onde parou).
    /// ponytail: as seis trilhas sao sintetizadas UMA vez por processo numa thread de fundo (C# puro, sem API do Unity) e
    /// viram clip no quadro em que ficam prontas; enquanto a da vez nao fica pronta, segue a anterior (no boot, ~1 s de
    /// silencio). Memoria: ~2 MB por trilha (nove). Volume do jogador e o liga/desliga do menu (AudioListener).</summary>
    public class SomDoJogo : MonoBehaviour
    {
        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo gerador (PartidaSetup); vazio = a do SaveState.")]
        [SerializeField] Partida partida;
        [SerializeField, Range(0f, 1f)] float volumeMusica = 0.3f;
        [SerializeField, Range(0f, 1f)] float volumeEfeitos = 0.8f;
        [Tooltip("De quem se mede o bosque. Ligado pelo gerador; vazio = nunca no bosque.")]
        [SerializeField] Transform jogador;
        [Tooltip("z a partir do qual o jogador esta no bosque (Auren: um pouco antes da ancora entrada_bosque). Ligado pelo gerador.")]
        [SerializeField] float zDoBosque = 1e6f;
        [Tooltip("Ancora bosque_coracao: o Enredado (q16) toca dentro do raio dela. Ligado pelo gerador; vazio = no bosque inteiro.")]
        [SerializeField] Transform coracao;
        [SerializeField] float raioDoCoracao = 18f;

        static AudioClip[] clips;
        static AudioClip[] trilhas;
        static Task<float[]>[] gerando;

        AudioSource efeitos;
        AudioSource[] fontes;
        TrilhaMusical? tocando;
        float ultimoGolpe = -1f;

        /// <summary>A tela da entrada aberta agora (EntryFlow escreve a cada troca de tela). Nenhuma = jogo no mundo.</summary>
        public TelaDeEntrada Entrada { get; set; }

        /// <summary>A trilha que esta subindo (ou ja tocando); null antes da primeira ficar pronta.</summary>
        public TrilhaMusical? Tocando { get { return tocando; } }

        void Awake()
        {
            if (clips == null || clips[0] == null)   // null do Unity tambem cobre clip destruido
            {
                clips = new AudioClip[System.Enum.GetValues(typeof(Som)).Length];
                for (int i = 0; i < clips.Length; i++) clips[i] = Clip(((Som)i).ToString(), Sintese.Efeito((Som)i));
            }
            int n = System.Enum.GetValues(typeof(TrilhaMusical)).Length;
            if (trilhas == null) { trilhas = new AudioClip[n]; gerando = new Task<float[]>[n]; }
            for (int i = 0; i < n; i++) Pronta((TrilhaMusical)i);   // dispara na thread de fundo o que ainda nao existe

            efeitos = gameObject.AddComponent<AudioSource>();
            efeitos.playOnAwake = false;
            efeitos.spatialBlend = 0f;
            fontes = new AudioSource[n];
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

        /// <summary>Houve golpe agora (dado, levado ou bloqueado): a musica de combate entra e segura por
        /// TrilhaDoMomento.SegundosDeCombate.</summary>
        public void MarcarCombate() { ultimoGolpe = Time.time; }

        /// <summary>Clip da trilha, se ja ficou pronto. Sem clip nem sintese em curso (primeira vez, ou clip destruido), dispara
        /// a sintese na thread de fundo; pronta, vira clip neste quadro e o float[] da thread e solto.</summary>
        static AudioClip Pronta(TrilhaMusical t)
        {
            int i = (int)t;
            if (trilhas[i] != null) return trilhas[i];   // null do Unity tambem cobre clip destruido
            Task<float[]> g = gerando[i];
            if (g == null) { gerando[i] = Task.Run(() => Sintese.Trilha(t)); return null; }
            if (!g.IsCompleted) return null;
            if (g.IsFaulted) { Debug.LogException(g.Exception); gerando[i] = null; return null; }   // erro de codigo: tenta de novo
            trilhas[i] = Clip("Trilha" + t, g.Result);
            gerando[i] = null;
            return trilhas[i];
        }

        void Update()
        {
            if (fontes == null) return;
            GameSession sessao = Partida.De(partida);
            SaveData save = sessao.Save;
            bool noite = TimeOfDayCycle.Atual(save == null ? null : save.life) == TimeOfDay.Noite;
            bool bosque = jogador != null && jogador.position.z >= zDoBosque;
            TrilhaMusical quer = TrilhaDoMomento.Escolher(Entrada, TrilhaDoMomento.EmCombate(Time.time, ultimoGolpe), bosque, noite,
                                                          TrilhaDoMomento.NoLugar(TrilhaDoMomento.DoEpisodio(sessao.Missoes), PertoDoCoracao()));
            if (Pronta(quer) != null) tocando = quer;   // senao segue a que ja toca ate a nova ficar pronta
            if (tocando == null) return;

            float passo = Time.unscaledDeltaTime / TrilhaDoMomento.Troca(tocando.Value) * volumeMusica;
            for (int i = 0; i < fontes.Length; i++)
            {
                bool daVez = i == (int)tocando.Value;
                AudioSource f = fontes[i];
                if (f == null)
                {
                    if (!daVez) continue;
                    fontes[i] = f = Fonte(trilhas[i]);
                }
                f.volume = Mathf.MoveTowards(f.volume, daVez ? volumeMusica : 0f, passo);
                if (daVez && !f.isPlaying) f.UnPause();
                else if (!daVez && f.volume <= 0f && f.isPlaying) f.Pause();
            }
        }

        bool PertoDoCoracao()
        {
            if (coracao == null || jogador == null) return true;
            return (jogador.position - coracao.position).sqrMagnitude <= raioDoCoracao * raioDoCoracao;
        }

        AudioSource Fonte(AudioClip clip)
        {
            AudioSource a = gameObject.AddComponent<AudioSource>();
            a.clip = clip;
            a.loop = true;
            a.spatialBlend = 0f;
            a.playOnAwake = false;
            a.volume = 0f;
            a.Play();
            return a;
        }
    }
}
