using UnityEngine;

namespace COE
{
    /// <summary>Visual dos paineis IMGUI do prototipo no tom do jogo (anime estilizado, ADR-0008): cantos arredondados,
    /// azul-noite com filete dourado, botao que acende ao passar/tocar. Texturas geradas em codigo (9 fatias pelo
    /// GUIStyle.border): nada de arquivo, uma textura por estilo, criada uma vez.
    /// ponytail: estilos para conversa (e quem quiser reusar). A UI de verdade (Canvas, fonte propria) e a T013.</summary>
    public static class UiEstilo
    {
        public static readonly Color Noite = new Color(0.09f, 0.10f, 0.17f, 0.96f);
        public static readonly Color Ouro = new Color(1f, 0.82f, 0.36f);
        public static readonly Color Tinta = new Color(0.96f, 0.94f, 0.88f);   // texto: creme, menos duro que branco
        static readonly Color BotaoFundo = new Color(0.17f, 0.19f, 0.30f, 0.98f);
        static readonly Color BotaoBorda = new Color(0.36f, 0.40f, 0.58f);
        static readonly Color CorBotaoAceso = new Color(0.24f, 0.27f, 0.42f, 1f);

        const int Lado = 48, Raio = 14;
        static Texture2D painel, botao, botaoAceso, botaoApertado, etiqueta;

        public static Texture2D Painel { get { return painel ?? (painel = Arredondado(Noite, Ouro, 2)); } }
        public static Texture2D Botao { get { return botao ?? (botao = Arredondado(BotaoFundo, BotaoBorda, 2)); } }
        public static Texture2D BotaoAceso { get { return botaoAceso ?? (botaoAceso = Arredondado(CorBotaoAceso, Ouro, 2)); } }
        public static Texture2D BotaoApertado { get { return botaoApertado ?? (botaoApertado = Arredondado(Ouro, Ouro, 0)); } }
        public static Texture2D Etiqueta { get { return etiqueta ?? (etiqueta = Arredondado(Ouro, Ouro, 0)); } }

        public static RectOffset Borda { get { return new RectOffset(Raio + 2, Raio + 2, Raio + 2, Raio + 2); } }

        public static GUIStyle EstiloPainel()
        {
            return new GUIStyle { normal = { background = Painel }, border = Borda };
        }

        public static GUIStyle EstiloBotao(int fonte)
        {
            var s = new GUIStyle(GUI.skin.button)
            {
                fontSize = fonte, wordWrap = true, alignment = TextAnchor.MiddleLeft, border = Borda,
                padding = new RectOffset(fonte, fonte, fonte / 3, fonte / 3),
            };
            s.normal.background = Botao; s.normal.textColor = Tinta;
            s.hover.background = BotaoAceso; s.hover.textColor = Ouro;
            s.focused.background = BotaoAceso; s.focused.textColor = Ouro;
            s.active.background = BotaoApertado; s.active.textColor = Noite;
            return s;
        }

        public static GUIStyle EstiloEtiqueta(int fonte)
        {
            return new GUIStyle
            {
                fontSize = fonte, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter, border = Borda,
                padding = new RectOffset(fonte, fonte, 0, 0),
                normal = { background = Etiqueta, textColor = new Color(0.12f, 0.10f, 0.16f) },
            };
        }

        /// <summary>Retangulo arredondado com borda, antialias de 1 px na curva. espessura 0 = sem borda.</summary>
        static Texture2D Arredondado(Color fundo, Color borda, int espessura)
        {
            var t = new Texture2D(Lado, Lado, TextureFormat.RGBA32, false) { hideFlags = HideFlags.HideAndDontSave, filterMode = FilterMode.Bilinear };
            var px = new Color[Lado * Lado];
            for (int y = 0; y < Lado; y++)
                for (int x = 0; x < Lado; x++)
                {
                    // distancia ao retangulo interno (raio): <0 dentro, >0 fora
                    float cx = Mathf.Clamp(x + 0.5f, Raio, Lado - Raio), cy = Mathf.Clamp(y + 0.5f, Raio, Lado - Raio);
                    float d = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(cx, cy)) - Raio;
                    float dentro = Mathf.Clamp01(0.5f - d);
                    Color c = d > -espessura ? borda : fundo;
                    c.a *= dentro;
                    px[y * Lado + x] = c;
                }
            t.SetPixels(px);
            t.Apply();
            return t;
        }
    }
}
