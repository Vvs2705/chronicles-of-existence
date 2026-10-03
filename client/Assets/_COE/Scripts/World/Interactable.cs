using System;
using System.Collections.Generic;
using UnityEngine;

namespace COE
{
    /// <summary>Base de tudo que o jogador pode acionar no mundo (T002): expoe um texto de prompt e dispara
    /// Interacted quando o PlayerInteractor aceita a interacao. O QUE acontece depois (falar, abrir, pegar)
    /// e da subclasse - dialogo, NPC e missao sao T006/T007 e nao existem aqui.
    ///
    /// CONTRATO: Interact() nao valida raio nem campo de visao; quem chama ja escolheu o alvo.
    /// ponytail: registro estatico + varredura linear no PlayerInteractor, sem collider, layer nem physics query.
    /// Uma vila com dezenas de interagiveis nao sente; trocar por Physics.OverlapSphere em layer propria se
    /// passar de algumas centenas por cena.</summary>
    public abstract class Interactable : MonoBehaviour
    {
        static readonly List<Interactable> ativos = new List<Interactable>();

        /// <summary>Interagiveis habilitados na cena. So leitura: OnEnable/OnDisable mantem a lista.</summary>
        public static IList<Interactable> Ativos { get { return ativos; } }

        /// <summary>Texto curto mostrado ao jogador, ex.: "Falar com Borin".</summary>
        public abstract string Prompt { get; }

        /// <summary>false = existe mas nao pode ser alvo agora (NPC ausente, ADR-0007 §3): o PlayerInteractor pula,
        /// sem prompt e sem USAR. Diferente de desligar o componente: ele continua rodando e volta sozinho.</summary>
        public virtual bool Acionavel { get { return true; } }

        /// <summary>Disparado uma vez por interacao aceita. Argumento: quem interagiu.</summary>
        public event Action<GameObject> Interacted;

        public void Interact(GameObject quem)
        {
            OnInteract(quem);
            if (Interacted != null) Interacted(quem);
        }

        protected virtual void OnInteract(GameObject quem) { }

        protected virtual void OnEnable() { ativos.Add(this); }
        protected virtual void OnDisable() { ativos.Remove(this); }
    }
}
