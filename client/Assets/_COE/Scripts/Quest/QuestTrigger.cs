using UnityEngine;

namespace COE
{
    /// <summary>Gatilho de objetivo SEM NPC na ancora (T012; tabela em MissaoMundo.Gatilhos). E um Interactable: o
    /// PlayerInteractor o escolhe e o botao USAR o aciona. Acionar = MissaoMundo.Cumprir pela sessao, que grava uma vez.
    ///
    /// Visivel (GameObject ativo) SO enquanto o objetivo e o proximo pendente: quem liga e desliga e o MissaoHud, que
    /// ja reavalia as missoes. Desligado, sai de Interactable.Ativos e nao rouba o alvo de NPC nenhum.
    /// Sem collider: nao barra percurso (AurenSceneTests varre a capsula do Player ate cada ancora).</summary>
    public class QuestTrigger : Interactable
    {
        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo gerador (PartidaSetup); vazio = a do SaveState.")]
        [SerializeField] Partida partida;
        [SerializeField] string questId = "";
        [SerializeField] string objetivoId = "";

        string prompt = "";

        public string QuestId { get { return questId; } }
        public string ObjetivoId { get { return objetivoId; } }

        /// <summary>O texto do objetivo (Strings; hoje "[chave]" ate o texto sair de "[a escrever]").</summary>
        public override string Prompt { get { return prompt; } }

        protected override void OnEnable()
        {
            base.OnEnable();
            // Montado aqui e nao no getter: o PlayerInteractor le Prompt a cada alvo novo, e chave ausente concatena.
            QuestDef d = QuestCatalog.Missao(questId);
            ObjetivoDef o = d == null ? null : d.Objetivo(objetivoId);
            prompt = o == null ? objetivoId : Strings.Get(o.TextoKey);
        }

        protected override void OnInteract(GameObject quem)
        {
            // Onde o jogador esta vai na MESMA gravacao do objetivo (T004: o save guarda a ancora; o AnchorSpawn le).
            Partida.De(partida).Posicao(CenaCatalogo.Id(gameObject.scene.name), MissaoMundo.AncoraDo(questId, objetivoId));
            if (MissaoMundo.Cumprir(Partida.De(partida), questId, objetivoId).Ok) gameObject.SetActive(false);
        }
    }
}
