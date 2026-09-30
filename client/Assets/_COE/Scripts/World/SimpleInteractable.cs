using UnityEngine;

namespace COE
{
    /// <summary>Interagivel de PROVA (T002): conta as interacoes e loga. Existe para a cena Bootstrap demonstrar
    /// que o alcance, o campo de visao e a tecla funcionam. Nao e NPC, dialogo nem missao (isso e T006/T007).
    /// Sem prompt proprio, mostra o generico de Strings (interacao.examinar).
    /// ponytail: o prompt proprio ainda e texto cru posto pelo gerador de cena (porta, poco, mural); vira chave de
    /// Strings quando esses objetos ganharem funcao de verdade.</summary>
    public class SimpleInteractable : Interactable
    {
        public string prompt = "";

        /// <summary>Quantas vezes este objeto foi acionado nesta sessao.</summary>
        public int Contagem { get; private set; }

        public override string Prompt
        {
            get { return string.IsNullOrEmpty(prompt) ? Strings.Get("interacao.examinar") : prompt; }
        }

        protected override void OnInteract(GameObject quem)
        {
            Contagem++;
            Debug.Log("SimpleInteractable: " + name + " acionado (" + Contagem + ")");
        }
    }
}
