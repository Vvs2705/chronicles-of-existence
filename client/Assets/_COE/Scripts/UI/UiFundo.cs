using UnityEngine;

namespace COE
{
    /// <summary>Estado de "ha modal na tela" (menu, salto, conversa, entrada, gancho, saida). Quem abre marca a cada quadro;
    /// a HUD de toque, o cartao de missao, o indicador, o prompt, o dano e o diagnostico se escondem enquanto isso.
    /// ponytail: um contador de quadro estatico (estado da tela, nao da partida). Pilha de telas se um dia houver modal
    /// sobre modal com regras diferentes.</summary>
    public static class UiFundo
    {
        static int modalAte = -1;

        /// <summary>Algum modal se marcou neste quadro ou no anterior.</summary>
        public static bool HaModal { get { return Time.frameCount <= modalAte; } }

        public static void MarcarModal() { modalAte = Time.frameCount + 1; }

        /// <summary>Cor do fundo que escurece o mundo atras do painel (Tela.FundoModal).</summary>
        public static readonly Color Escurecer = new Color(0f, 0f, 0f, 0.6f);
    }
}
