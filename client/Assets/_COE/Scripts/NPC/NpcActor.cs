using UnityEngine;

namespace COE
{
    /// <summary>Um NPC de Auren em cena (T012): corpo placeholder na escala certa, parado na ancora do que a agenda diz
    /// que ele faz agora, e Interactable -- interagir abre a conversa no DialogueHud. Regra nenhuma mora aqui: rotina e
    /// NpcCatalog/NpcAgenda, conversa e DialogueRunner, missao e QuestSystem.
    ///
    /// DEPENDENCIA EXPLICITA: raiz "Ancoras", corpo e DialogueHud chegam por campo serializado, ligados pelo
    /// NpcSceneSetup. A ancora e filha direta da raiz ligada (como no AnchorSpawn); nada de Find global.
    /// ponytail: teleporta quando a rotina muda (periodo, memoria, fim de conversa). Andar ate la pede NavMesh; entra
    /// quando alguem precisar ver o NPC caminhando. Sem colisor: nao barra percurso nem o Player.</summary>
    public class NpcActor : Interactable
    {
        /// <summary>Cada NPC fica numa vaga propria a esta distancia da ancora (angulo pelo indice no catalogo): varios
        /// NPCs na mesma ancora (praca de manha) nao se sobrepoem. 1,5 m afasta ate duas capsulas de adulto vizinhas.</summary>
        public const float RaioDaVaga = 1.5f;

        [SerializeField] string npcId = "";
        [Tooltip("Indice no NpcCatalog: angulo da vaga em volta da ancora.")]
        [SerializeField] int vaga;
        [Tooltip("Base corporal de crianca (docs/arte/PIPELINE.md §3.1): cresce com a idade do jogador.")]
        [SerializeField] bool crianca;
        [SerializeField] Transform ancoras;   // raiz "Ancoras" da cena
        [SerializeField] Transform corpo;     // capsula placeholder, pivo no centro
        [SerializeField] DialogueHud dialogo;

        NpcAgenda agenda;
        string prompt;

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

        /// <summary>O nome do NPC (chave de Strings). Cacheado: o PlayerInteractor le isto em todo OnGUI.</summary>
        public override string Prompt
        {
            get
            {
                if (prompt == null) prompt = Agenda.Npc != null ? Strings.Get(Agenda.Npc.NomeKey) : npcId;
                return prompt;
            }
        }

        // Awake: a cena carrega depois do save (SaveBootstrap -200 na Bootstrap), e o salto recarrega Auren.
        void Awake() { AjustarCorpo(SaveState.Current == null ? 5 : SaveState.Current.ageYears); }

        void Update() { Posicionar(SaveState.Current); }

        protected override void OnInteract(GameObject quem)
        {
            if (dialogo != null) dialogo.Abrir(this);
        }

        /// <summary>Poe o NPC na vaga da ancora da rotina do periodo do save, se a rotina mudou. Em conversa (ou
        /// qualquer interrupcao) fica onde esta. Publico porque Update nao roda em teste de Editor.
        /// ponytail: evento da vila com ancora propria (Interrupcao.AncoraId) ainda nao existe; quando existir, ir
        /// para ela aqui.</summary>
        public void Posicionar(SaveData save)
        {
            if (Agenda.Atual != null) return;
            RotinaEntrada e = Agenda.Agora(TimeOfDayCycle.Atual(save == null ? null : save.life), save == null ? null : save.npcs);
            if (e == Rotina) return;   // sem alocacao: sem interrupcao, Agora devolve a entrada do catalogo
            Rotina = e;
            Transform a = e == null || ancoras == null ? null : ancoras.Find(e.AncoraId);
            if (a != null) transform.position = a.position + Quaternion.Euler(0f, vaga * 36f, 0f) * (Vector3.forward * RaioDaVaga);
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
