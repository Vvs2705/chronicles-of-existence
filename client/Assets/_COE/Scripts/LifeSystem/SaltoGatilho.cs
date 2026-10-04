using UnityEngine;

namespace COE
{
    /// <summary>O simbolo do Limiar na clareira (slice B11-B12, §4.1): e ali que o salto e oferecido, nao em qualquer
    /// ponto de Auren. Interagir abre o aviso do SaltoHud. Quem liga e desliga este gatilho e o proprio SaltoHud:
    /// ele so existe para o jogador com o salto liberado (Q-08 concluida, ainda 5 anos).</summary>
    public class SaltoGatilho : Interactable
    {
        [SerializeField] SaltoHud hud;
        string prompt;

        public override string Prompt
        {
            get { return prompt ?? (prompt = Strings.Get("salto.simbolo")); }   // uma vez: o prompt pergunta quando o alvo muda
        }

        protected override void OnInteract(GameObject quem)
        {
            if (hud != null) hud.Abrir();
        }
    }
}
