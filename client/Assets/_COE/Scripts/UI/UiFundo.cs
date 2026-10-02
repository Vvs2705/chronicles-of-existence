using UnityEngine;

namespace COE
{
    /// <summary>Fundo solido para os modais IMGUI (menu, salto, conversa). O GUI.Box do skin padrao e translucido: a HUD
    /// de toque e o diagnostico apareciam atraves do painel. ponytail: um pixel branco tingido por GUI.color; a UI de
    /// verdade (Canvas) e a T013.</summary>
    public static class UiFundo
    {
        static Texture2D pixel;
        static int modalAte = -1;

        /// <summary>Algum modal foi desenhado neste quadro ou no anterior. A HUD de toque e a de desempenho nao se desenham
        /// enquanto isso: o GUI.depth nao ordena HUDs com useGUILayout = false, e elas apareciam por cima do painel.</summary>
        public static bool HaModal { get { return Time.frameCount <= modalAte; } }

        public static void MarcarModal() { modalAte = Time.frameCount + 1; }

        public static readonly Color Escurecer = new Color(0f, 0f, 0f, 0.6f);
        public static readonly Color Painel = new Color(0.10f, 0.11f, 0.14f, 0.97f);
        public static readonly Color Destaque = new Color(1f, 0.82f, 0.3f);   // opcao escolhida

        public static void Pintar(Rect area, Color cor)
        {
            if (pixel == null)
            {
                pixel = new Texture2D(1, 1) { hideFlags = HideFlags.HideAndDontSave };
                pixel.SetPixel(0, 0, Color.white);
                pixel.Apply();
            }
            Color antes = GUI.color;
            GUI.color = cor;
            GUI.DrawTexture(area, pixel);
            GUI.color = antes;
        }

        /// <summary>Modal: escurece a tela inteira e pinta o painel por cima.</summary>
        public static void Modal(Rect painel)
        {
            MarcarModal();
            Pintar(new Rect(0f, 0f, Screen.width, Screen.height), Escurecer);
            GUI.Box(painel, GUIContent.none, UiEstilo.PainelCache);   // moldura arredondada com filete dourado
        }
    }
}
