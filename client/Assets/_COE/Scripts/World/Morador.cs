using UnityEngine;

namespace COE
{
    /// <summary>Morador de Auren que da vida a rua: anda o passeio do periodo (Passeio), para e olha para o jogador que
    /// cruza na frente dele e some em casa nos periodos sem rota (a noite quase todo mundo recolhe; o vigia sai com a
    /// lanterna). Nao e NPC de historia: nao conversa, nao lembra, nao entra no save; quem tem nome e falas continua sendo o
    /// NpcActor. Regra nenhuma mora aqui: o passeio e o Passeio, o periodo e o do save.
    ///
    /// SEM COLISOR de proposito: um corpo andando pela rua barraria os percursos do T008 a cada volta. Em vez de esbarrar,
    /// ele da passagem (Passeio.DaPassagem).
    /// ponytail: o corpo e variante de um prototipo do elenco (outro tom pelo Tingir, outra altura): ASSETS.md §1 ja aceita variante de
    /// cor para figurante. Morador com cara propria entra pelos portoes do ADR-0002 como qualquer personagem.</summary>
    public class Morador : MonoBehaviour
    {
        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo gerador (PartidaSetup); vazio = a do SaveState.")]
        [SerializeField] Partida partida;
        [SerializeField] Transform jogador;
        [Tooltip("O corpo (modelo ou capsula). Desligado nos periodos sem rota.")]
        [SerializeField] Transform corpo;
        [Tooltip("Rota vazia = em casa nesse periodo.")]
        [SerializeField] Vector3[] rotaManha = new Vector3[0];
        [SerializeField] Vector3[] rotaTarde = new Vector3[0];
        [SerializeField] Vector3[] rotaNoite = new Vector3[0];
        [SerializeField] float velocidade = 1.4f;
        [SerializeField] float pausa = 2f;
        [Tooltip("Segundos de avanco no passeio: dois moradores na mesma rota nao andam juntos.")]
        [SerializeField] float fase;
        [Tooltip("Velocidade do Animator andando: o clip de andar foi medido na crianca; o adulto anda mais devagar com ele.")]
        [SerializeField] float ritmoDoClip = 1f;

        /// <summary>Valor do parametro Speed do Animator andando: o ponto Walk do blend (PrototipoAnimacoes).</summary>
        public static float SpeedAndando { get { return MotionSolver.VelocidadeCaminhadaPadrao / MotionSolver.VelocidadeCorridaPadrao; } }

        Animator anim;
        Passeio passeio;
        TimeOfDay? periodo;
        float t;

        /// <summary>O passeio em curso; null = em casa (ou antes do primeiro quadro).</summary>
        public Passeio Atual { get { return passeio; } }

        /// <summary>Mascara de periodos (bit 0 manha, 1 tarde, 2 noite), a mesma do Bando e da FonteDeSom.</summary>
        public static bool NoPeriodo(int mascara, TimeOfDay p) { return (mascara & (1 << (int)p)) != 0; }

        public Vector3[] Rota(TimeOfDay p)
        {
            return p == TimeOfDay.Manha ? rotaManha : p == TimeOfDay.Tarde ? rotaTarde : rotaNoite;
        }

        void Awake() { anim = GetComponentInChildren<Animator>(); }

        void Update()
        {
            SaveData save = Partida.De(partida).Save;
            Atualizar(TimeOfDayCycle.Atual(save == null ? null : save.life), Time.deltaTime);
        }

        /// <summary>Um passo do morador. Publico porque Update nao roda em teste de Editor.</summary>
        public void Atualizar(TimeOfDay agora, float dt)
        {
            if (periodo != agora)
            {
                periodo = agora;
                passeio = Passeio.De(Rota(agora), velocidade, pausa);
                t = fase;
                if (corpo != null) corpo.gameObject.SetActive(passeio != null);
            }
            if (passeio == null) return;

            PoseDePasseio pose = passeio.Em(t);
            bool espera = jogador != null && pose.Andando && Passeio.DaPassagem(transform.position, pose.Direcao, jogador.position);
            if (!espera) t += dt;
            pose = passeio.Em(t);

            Vector3 olhar = pose.Direcao;
            if (espera)
            {
                olhar = jogador.position - transform.position;
                olhar.y = 0f;
                if (olhar.sqrMagnitude < 1e-4f) olhar = pose.Direcao;
            }
            transform.position = pose.Posicao;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(olhar.normalized), 360f * dt);

            if (anim == null || anim.runtimeAnimatorController == null) return;
            bool andando = pose.Andando && !espera;
            anim.SetFloat(AnimParams.Speed, andando ? SpeedAndando : 0f, 0.15f, dt);
            anim.speed = andando ? ritmoDoClip : 1f;
        }
    }
}
