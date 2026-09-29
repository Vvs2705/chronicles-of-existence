using UnityEngine;

namespace COE
{
    /// <summary>Interagivel de PROVA (T002): conta as interacoes e loga. Existe para a cena Bootstrap demonstrar
    /// que o alcance, o campo de visao e a tecla funcionam. Nao e NPC, dialogo nem missao (isso e T006/T007).
    /// ponytail: prompt e texto cru no componente; vira chave de Strings quando existir HUD de interacao.</summary>
    public class SimpleInteractable : Interactable
    {
        public string prompt = "Examinar";

        /// <summary>Quantas vezes este objeto foi acionado nesta sessao.</summary>
        public int Contagem { get; private set; }

        public override string Prompt { get { return prompt; } }

        protected override void OnInteract(GameObject quem)
        {
            Contagem++;
            Debug.Log("SimpleInteractable: " + name + " acionado (" + Contagem + ")");
        }
    }
}
