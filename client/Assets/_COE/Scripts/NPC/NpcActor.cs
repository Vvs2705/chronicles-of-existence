using UnityEngine;

namespace COE
{
    /// <summary>Um NPC de Auren em cena (T012): corpo placeholder na escala certa, parado na ancora do que a agenda diz
    /// que ele faz agora, e Interactable -- interagir abre a conversa no DialogueHud. Regra nenhuma mora aqui: rotina e
    /// NpcCatalog/NpcAgenda, conversa e DialogueRunner, missao e QuestSystem.
    ///
    /// DEPENDENCIA EXPLICITA: raiz "Ancoras", corpo e DialogueHud chegam por campo serializado, ligados pelo
    /// NpcSceneSetup. A ancora e filha direta da raiz ligada (como no AnchorSpawn); nada de Find global.
    /// Quando a rotina muda com o jogo rodando (periodo, memoria, fim de conversa) ele CAMINHA ate a ancora nova pelas
    /// Ruas (os percursos do T008, chao que o teste de navegacao ja garante livre), para e olha para o jogador que cruza na
    /// frente dele (Passeio.DaPassagem) e para de vez se alguem puxar conversa no meio do caminho.
    /// ponytail: carregar o save, ficar ausente ou voltar de ausente, e a rotina mudar de novo no meio da caminhada
    /// teleportam (sem rua conhecida a partir de um ponto qualquer da rota). Desvio de quem esta parado na rua pede NavMesh.
    /// Solido (colisor no corpo): nenhuma vaga fica num percurso do Player.</summary>
    public class NpcActor : Interactable
    {
        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo gerador (PartidaSetup); vazio = a do SaveState.")]
        [SerializeField] Partida partida;
        /// <summary>Cada NPC fica numa vaga propria a esta distancia da ancora (angulo pelo indice no catalogo): varios
        /// NPCs na mesma ancora (praca de manha) nao se sobrepoem. 1,5 m afasta ate duas capsulas de adulto vizinhas.</summary>
        public const float RaioDaVaga = 1.5f;
        /// <summary>Passo de quem vai trabalhar, sem pressa (o Player anda a MotionSolver.VelocidadeCaminhadaPadrao).</summary>
        public const float Velocidade = 1.5f;

        [SerializeField] string npcId = "";
        [Tooltip("Indice no NpcCatalog: angulo da vaga em volta da ancora.")]
        [SerializeField] int vaga;
        [Tooltip("Base corporal de crianca (docs/arte/PIPELINE.md §3.1): cresce com a idade do jogador.")]
        [SerializeField] bool crianca;
        [SerializeField] Transform ancoras;   // raiz "Ancoras" da cena
        [SerializeField] Transform corpo;     // capsula placeholder, pivo no centro
        [SerializeField] DialogueHud dialogo;
        [Tooltip("As ruas de Auren (raiz Ancoras). Vazio = teleporta para a ancora nova.")]
        [SerializeField] Ruas ruas;
        [Tooltip("O Player: o NPC que caminha para e da passagem a ele.")]
        [SerializeField] Transform jogador;

        NpcAgenda agenda;
        string prompt;
        Animator anim;
        Vector3[] rota;
        int proximo;

        /// <summary>A caminhada em curso (ultimo ponto = a vaga da ancora nova); null = parado na vaga.</summary>
        public Vector3[] Rota { get { return rota; } }
        public bool Caminhando { get { return rota != null; } }

        public string NpcId { get { return npcId; } }

        /// <summary>Estado transitorio (conversa, evento da vila), fora do save de proposito (NpcAgenda).</summary>
        public NpcAgenda Agenda
        {
            get
            {
                if (agenda == null) agenda = new NpcAgenda(NpcCatalog.Npc(npcId));
                return agenda;
            }
        }

        /// <summary>A entrada de rotina em que o NPC esta posto agora; null antes do primeiro Posicionar.</summary>
        public RotinaEntrada Rotina { get; private set; }

        /// <summary>A rotina aponta para a ancora-sentinela (Nilo desaparecido, ADR-0007 §3): o NPC nao esta em Auren.
        /// Sem corpo e sem conversa; o componente segue ligado para voltar sozinho quando a rotina mudar.</summary>
        public bool Ausente { get { return Rotina != null && Rotina.AncoraId == NpcCatalog.AncoraAusente; } }

        public override bool Acionavel { get { return !Ausente; } }

        /// <summary>O nome do NPC (chave de Strings). Cacheado: o PlayerInteractor le isto a cada alvo novo.</summary>
        public override string Prompt
        {
            get
            {
                if (prompt == null) prompt = Agenda.Npc != null ? Strings.Get(Agenda.Npc.NomeKey) : npcId;
                return prompt;
            }
        }

        // Awake: a cena carrega depois do save (SaveBootstrap -200 na Bootstrap), e o salto recarrega Auren.
        void Awake()
        {
            AjustarCorpo(Partida.De(partida).Save.ageYears);
            anim = GetComponentInChildren<Animator>(true);
        }

        void Update()
        {
            Posicionar(Partida.De(partida).Save, true);
            Andar(Time.deltaTime);
        }

        protected override void OnInteract(GameObject quem)
        {
            if (dialogo != null && !Ausente) dialogo.Abrir(this);
        }

        /// <summary>Poe o NPC na vaga da ancora da rotina do periodo do save, se a rotina mudou (o periodo anda ao
        /// concluir missao e ao descansar: ADR-0007 §1). Em conversa (ou qualquer interrupcao) fica onde esta — Nilo
        /// so some quando a conversa que fechou a Q-04 termina. Publico porque Update nao roda em teste de Editor.
        /// ponytail: evento da vila com ancora propria (Interrupcao.AncoraId) ainda nao existe; quando existir, ir
        /// para ela aqui.</summary>
        public void Posicionar(SaveData save) { Posicionar(save, false); }

        /// <summary>Com 'caminhando' (o Update), a rotina nova com rua dos dois lados vira caminhada em vez de teleporte.</summary>
        public void Posicionar(SaveData save, bool caminhando)
        {
            if (Agenda.Atual != null) return;
            RotinaEntrada e = Agenda.Agora(TimeOfDayCycle.Atual(save == null ? null : save.life), save == null ? null : save.npcs);
            if (e == Rotina) return;   // sem alocacao: sem interrupcao, Agora devolve a entrada do catalogo
            RotinaEntrada antes = Rotina;
            bool meioDoCaminho = rota != null;
            Rotina = e;
            rota = null;
            if (corpo != null) corpo.gameObject.SetActive(!Ausente);   // ausente: fica onde estava, invisivel
            Transform a = e == null || ancoras == null ? null : ancoras.Find(e.AncoraId);
            if (a == null) return;
            Vector3 destino = PosicaoNaVaga(a.position, vaga);

            Vector3[] rua = caminhando && !meioDoCaminho && antes != null && !Ausente && ruas != null
                            && antes.AncoraId != NpcCatalog.AncoraAusente
                ? ruas.Caminho(antes.AncoraId, e.AncoraId) : null;
            if (rua == null || rua.Length < 2) { transform.position = destino; return; }

            // Sai da vaga, vai a ancora velha, segue as ruas ate a ancora nova e entra na vaga dela.
            rota = new Vector3[rua.Length + 1];
            for (int i = 0; i < rua.Length; i++) rota[i] = new Vector3(rua[i].x, transform.position.y, rua[i].z);
            rota[rua.Length] = destino;
            proximo = 0;
        }

        /// <summary>Um passo da caminhada. Publico porque Update nao roda em teste de Editor. Em conversa fica parado.</summary>
        public void Andar(float dt)
        {
            bool andando = false;
            if (rota != null && Agenda.Atual == null)
            {
                Vector3 falta = rota[proximo] - transform.position;
                falta.y = 0f;
                Vector3 direcao = falta.sqrMagnitude > 1e-6f ? falta.normalized : transform.forward;
                Vector3 olhar = direcao;
                bool espera = jogador != null && Passeio.DaPassagem(transform.position, direcao, jogador.position);
                if (espera)
                {
                    olhar = jogador.position - transform.position;
                    olhar.y = 0f;
                    if (olhar.sqrMagnitude < 1e-4f) olhar = direcao;
                }
                else
                {
                    andando = true;
                    float passo = Velocidade * dt;
                    if (falta.magnitude <= passo)
                    {
                        transform.position = rota[proximo];
                        if (++proximo >= rota.Length) { rota = null; andando = false; }
                    }
                    else transform.position += direcao * passo;
                }
                if (olhar.sqrMagnitude > 1e-6f)
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(olhar.normalized), 360f * dt);
            }

            if (anim == null || anim.runtimeAnimatorController == null) return;
            anim.SetFloat(AnimParams.Speed, andando ? Morador.SpeedAndando : 0f, 0.15f, dt);
            anim.speed = andando ? RitmoDoClip(corpo != null ? corpo.localScale.y * 2f : BodyScale.Adulto) : 1f;
        }

        /// <summary>O clip de andar foi medido na crianca de 5 anos: quem e mais alto toca mais devagar para o pe nao
        /// escorregar (a mesma conta do Morador, para a Velocidade do NPC).</summary>
        public static float RitmoDoClip(float altura)
        {
            return Velocidade * BodyScale.Crianca5 / (MotionSolver.VelocidadeCaminhadaPadrao * Mathf.Max(0.5f, altura));
        }

        /// <summary>Onde o NPC da vaga 'vaga' fica em volta da ancora. Ancora de porta: o angulo do indice pode cair dentro
        /// do predio, e o NPC e solido; gira (meia volta, depois quartos) ate achar chao livre de colisor que nao seja de
        /// NPC nem do Player. Publico para o teste de vagas (AurenSceneTests).
        /// ponytail: sem chao livre em nenhum giro fica no angulo do indice; ancora com predio dos dois lados pediria
        /// vaga autorada por ancora.</summary>
        public static Vector3 PosicaoNaVaga(Vector3 ancora, int vaga)
        {
            Physics.SyncTransforms();   // a cena acabou de ser montada (Editor) ou o NPC acabou de se mover
            foreach (float giro in Giros)
            {
                Vector3 p = ancora + Quaternion.Euler(0f, vaga * 36f + giro, 0f) * (Vector3.forward * RaioDaVaga);
                if (Livre(p)) return p;
            }
            return ancora + Quaternion.Euler(0f, vaga * 36f, 0f) * (Vector3.forward * RaioDaVaga);
        }

        static readonly float[] Giros = { 0f, 180f, 90f, -90f, 45f, -45f, 135f, -135f };
        static readonly Collider[] ocupado = new Collider[16];

        static bool Livre(Vector3 p)
        {
            const float r = 0.3f;   // ombro de adulto com folga (o colisor do corpo tem ~0,26)
            int n = Physics.OverlapCapsuleNonAlloc(p + Vector3.up * (r + 0.2f), p + Vector3.up * (BodyScale.Adulto - r), r,
                                                   ocupado, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            for (int i = 0; i < n; i++)
                if (ocupado[i].GetComponentInParent<NpcActor>() == null && !(ocupado[i] is CharacterController)) return false;
            return true;
        }

        /// <summary>Altura do corpo: adulto fixo; crianca acompanha a fase de vida do jogador (5 -> 8 anos no salto,
        /// dossie §G "nao congelar amigos"). HIPOTESE v0: Nilo e Sera tem a idade do jogador.</summary>
        public static float Altura(bool crianca, int idadeAnos)
        {
            if (!crianca) return BodyScale.Adulto;
            return LifePhases.De(idadeAnos) >= LifePhase.DespertarDosTalentos ? BodyScale.Crianca8 : BodyScale.Crianca5;
        }

        /// <summary>Escala a capsula (primitiva de 2 m, pivo no centro) para a altura, pes no chao do NPC.</summary>
        public void AjustarCorpo(int idadeAnos)
        {
            if (corpo == null) return;
            float h = Altura(crianca, idadeAnos);
            corpo.localScale = Vector3.one * (h * 0.5f);
            corpo.localPosition = Vector3.up * (h * 0.5f);
        }
    }
}
