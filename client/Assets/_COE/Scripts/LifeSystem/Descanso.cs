using UnityEngine;

namespace COE
{
    /// <summary>O "Descansar" de casa (ADR-0007 §1): interagir faz o dia andar UM periodo. A regra e a gravacao sao da
    /// GameSession.Descansar; aqui so o rotulo e o toque. Quem reage e quem ja le o periodo do save: os NPCs
    /// (NpcActor, a cada quadro) e o HUD (MissaoHud, 5x/s).
    /// ponytail: sem tela, som nem animacao de dormir; o retorno ao jogador e o periodo no HUD e a vila mudando de
    /// lugar. Transicao visual e da T013.</summary>
    public class Descanso : Interactable
    {
        /// <summary>A ancora da casa: e onde o save reabre depois de descansar.</summary>
        public const string AncoraId = "casa_familia";

        string prompt;

        public override string Prompt
        {
            get { return prompt ?? (prompt = Strings.Get("casa.descansar")); }   // uma vez: o OnGUI pergunta todo quadro
        }

        protected override void OnInteract(GameObject quem)
        {
            GameSession s = SaveState.Sessao;
            s.Posicao(gameObject.scene.name, AncoraId);   // vai na MESMA gravacao do descanso
            s.Descansar();
        }
    }
}
