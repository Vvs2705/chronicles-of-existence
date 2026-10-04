using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace COE
{
    /// <summary>Base da UI de jogador em uGUI (Bloco D, 2026-10-04; ADR-0006). Cada tela monta a propria arvore em codigo,
    /// sob um Canvas proprio (ScreenSpaceOverlay, ordem = camada): o gerador de cena so poe o componente, sem prefab nem
    /// UI serializada na cena.
    ///
    /// ESCALA: CanvasScaler em pixel 1:1 (ConstantPixelSize). Os retangulos vem de layout puro em px de tela, com origem
    /// embaixo a esquerda (ControlPreset, TouchControls): o botao desenhado e o alvo do toque saem da MESMA geometria, que
    /// os testes de EditMode conferem em 16:9, 19.5:9 e 20:9. Tamanho em dp pelo Dpi (alvo >= 48 dp).
    /// Fonte embutida do Unity (LegacyRuntime.ttf): fonte propria e licenca sao a T013.
    /// ponytail: um Canvas por tela (camada pela ordem; rebuild isolado). Atlas e Canvas compartilhado se o profiler pedir.</summary>
    public static class Tela
    {
        /// <summary>Camadas (Canvas.sortingOrder): HUD sobre o mundo &lt; controles de toque &lt; modais &lt; sistema (sair). Cada modal
        /// tem a sua, na ordem do GUI.depth de antes (entrada na frente, depois gancho, menu, conversa, salto): empate entre
        /// Canvas overlay sai da ordem da hierarquia, e mexer no gerador trocaria quem recebe o toque.</summary>
        public const int CamadaHud = 10, CamadaToque = 20, CamadaSalto = 31, CamadaConversa = 32, CamadaMenu = 33,
                         CamadaGancho = 34, CamadaEntrada = 35, CamadaSistema = 40;

        /// <summary>No PC em modo celular (-toque), a janela faz o papel da tela do POCO F4: a altura dela vale 393 dp
        /// (1080 px / 2,75). Ligado pelo PlayerInputReader. ponytail: um aparelho de referencia fixo.</summary>
        public static bool SimulandoToque;

        /// <summary>Densidade do layout: a da tela, ou a simulada no modo celular do PC (dpi ~96).</summary>
        public static float Dpi
        {
            get { return SimulandoToque && Screen.dpi < 200f ? Screen.height * 160f / 393f : Screen.dpi; }
        }

        /// <summary>Alvo de toque: 48 dp no minimo e ~1/10 da altura em tela grande.</summary>
        public static float Alvo { get { return HudLayout.Alvo(Screen.height, Dpi); } }

        /// <summary>Corpo de texto: `dp` no minimo e uma fracao da altura (1/40 = texto corrido, 1/30 = destaque).</summary>
        public static int Fonte(float dp, float fracaoDaAltura)
        {
            return HudLayout.Fonte(dp, fracaoDaAltura, Screen.height, Dpi);
        }

        static Font fonte;
        public static Font FontePadrao { get { return fonte != null ? fonte : (fonte = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf")); } }

        /// <summary>Canvas proprio, filho de `dono`, em pixel 1:1. Devolve a raiz (tela inteira).</summary>
        public static Canvas NovoCanvas(Transform dono, string nome, int camada)
        {
            var go = new GameObject(nome, typeof(RectTransform));
            go.transform.SetParent(dono, false);
            var c = go.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            c.sortingOrder = camada;
            var s = go.AddComponent<CanvasScaler>();
            s.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            s.scaleFactor = 1f;
            go.AddComponent<GraphicRaycaster>();
            GarantirEventSystem();
            return c;
        }

        /// <summary>Botao de uGUI precisa de EventSystem. Um por cena, criado pela primeira tela que precisar.
        /// No modo celular do PC (-toque) o mouse e o toque simulado sao o mesmo dedo: ponteiro unico, um clique nao vira dois.
        /// No aparelho, multitoque: com o polegar no joystick, o outro ainda aperta botao de tela.</summary>
        public static void GarantirEventSystem()
        {
            if (EventSystem.current != null) return;
            var go = new GameObject("EventSystem", typeof(EventSystem));
            var m = go.AddComponent<InputSystemUIInputModule>();
            m.AssignDefaultActions();
            bool simulado = SimulandoToque || DevSceneArg.Tem("-toque");
            m.pointerBehavior = simulado ? UIPointerBehavior.SingleUnifiedPointer : UIPointerBehavior.SingleMouseOrPenButMultiTouchAndTrack;
        }

        /// <summary>Fundo escuro de modal na tela inteira, que segura o toque (nada atras recebe). O painel vai como filho.</summary>
        public static Image FundoModal(Transform canvas)
        {
            Image f = Imagem(canvas, "Fundo", null, UiFundo.Escurecer);
            f.raycastTarget = true;
            Esticar(f.rectTransform, 0f);
            return f;
        }

        /// <summary>Marca da opcao escolhida no texto: a escolha nao depende so da cor (Prompt Mestre §9, acessibilidade).</summary>
        public const string MarcaEscolhida = "\u2022 ";

        /// <summary>Opcao de alternancia: a escolhida fica em ouro cheio, texto escuro e com a marca na frente.</summary>
        public static void Marcar(Button b, bool escolhida)
        {
            ((Image)b.targetGraphic).sprite = escolhida ? SpriteBotaoApertado : SpriteBotao;
            Text r = Rotulo(b);
            r.color = escolhida ? UiEstilo.Noite : UiEstilo.Tinta;
            string t = r.text.StartsWith(MarcaEscolhida) ? r.text.Substring(MarcaEscolhida.Length) : r.text;
            r.text = escolhida ? MarcaEscolhida + t : t;
        }

        /// <summary>Retangulo em px de tela (origem embaixo a esquerda) aplicado num filho do Canvas 1:1.</summary>
        public static void Colocar(RectTransform rt, Rect px)
        {
            rt.anchorMin = rt.anchorMax = Vector2.zero;
            rt.pivot = Vector2.zero;
            rt.anchoredPosition = px.position;
            rt.sizeDelta = px.size;
        }

        /// <summary>Esticado sobre o pai inteiro (fundo escuro de modal, texto que ocupa o painel), com margem em px.</summary>
        public static void Esticar(RectTransform rt, float margem)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.offsetMin = new Vector2(margem, margem);
            rt.offsetMax = new Vector2(-margem, -margem);
        }

        public static RectTransform Filho(Transform pai, string nome)
        {
            var go = new GameObject(nome, typeof(RectTransform));
            go.transform.SetParent(pai, false);
            return (RectTransform)go.transform;
        }

        public static Image Imagem(Transform pai, string nome, Sprite sprite, Color cor)
        {
            var img = Filho(pai, nome).gameObject.AddComponent<Image>();
            img.sprite = sprite;
            img.color = cor;
            img.type = sprite != null && sprite.border != Vector4.zero ? Image.Type.Sliced : Image.Type.Simple;
            img.raycastTarget = false;
            return img;
        }

        public static Text Texto(Transform pai, string nome, int tamanho, TextAnchor alinhamento, Color cor)
        {
            var t = Filho(pai, nome).gameObject.AddComponent<Text>();
            t.font = FontePadrao;
            t.fontSize = tamanho;
            t.alignment = alinhamento;
            t.color = cor;
            t.horizontalOverflow = HorizontalWrapMode.Wrap;
            t.verticalOverflow = VerticalWrapMode.Truncate;
            t.raycastTarget = false;
            return t;
        }

        /// <summary>Botao com rotulo, no visual do jogo (UiEstilo): acende ao tocar, ouro cheio apertado.</summary>
        public static Button Botao(Transform pai, string nome, int fonte, UnityAction aoTocar)
        {
            Image fundo = Imagem(pai, nome, SpriteBotao, Color.white);
            fundo.raycastTarget = true;
            var b = fundo.gameObject.AddComponent<Button>();
            b.targetGraphic = fundo;
            b.transition = Selectable.Transition.SpriteSwap;
            b.spriteState = new SpriteState { highlightedSprite = SpriteBotaoAceso, selectedSprite = SpriteBotao, pressedSprite = SpriteBotaoApertado };
            Navigation n = b.navigation;
            n.mode = Navigation.Mode.None;   // conversa por gamepad fora do slice (ADR-0007 §8); sem foco preso
            b.navigation = n;
            if (aoTocar != null) b.onClick.AddListener(aoTocar);
            Text r = Texto(fundo.transform, "Rotulo", fonte, TextAnchor.MiddleCenter, UiEstilo.Tinta);
            r.fontStyle = FontStyle.Bold;
            Esticar(r.rectTransform, fonte * 0.4f);
            return b;
        }

        public static Text Rotulo(Button b) { return b.GetComponentInChildren<Text>(true); }

        // Sprites dos paineis: as texturas arredondadas do UiEstilo, em 9 fatias. Criadas uma vez.
        static Sprite painel, cartao, botao, botaoAceso, botaoApertado, disco;
        public static Sprite SpritePainel { get { return painel != null ? painel : (painel = Fatiado(UiEstilo.Painel)); } }
        public static Sprite SpriteCartao { get { return cartao != null ? cartao : (cartao = Fatiado(UiEstilo.Cartao)); } }
        public static Sprite SpriteBotao { get { return botao != null ? botao : (botao = Fatiado(UiEstilo.Botao)); } }
        public static Sprite SpriteBotaoAceso { get { return botaoAceso != null ? botaoAceso : (botaoAceso = Fatiado(UiEstilo.BotaoAceso)); } }
        public static Sprite SpriteBotaoApertado { get { return botaoApertado != null ? botaoApertado : (botaoApertado = Fatiado(UiEstilo.BotaoApertado)); } }

        /// <summary>Disco branco de borda suavizada (controles de toque), tingido pela cor da Image.</summary>
        public static Sprite SpriteDisco
        {
            get
            {
                if (disco != null) return disco;
                const int n = 128;
                var t = new Texture2D(n, n, TextureFormat.RGBA32, false) { hideFlags = HideFlags.HideAndDontSave };
                var px = new Color[n * n];
                float r = n * 0.5f;
                for (int y = 0; y < n; y++)
                    for (int x = 0; x < n; x++)
                    {
                        float d = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(r, r));
                        px[y * n + x] = new Color(1f, 1f, 1f, Mathf.Clamp01(r - d));   // borda de 1 px suavizada
                    }
                t.SetPixels(px);
                t.Apply();
                return disco = Sprite.Create(t, new Rect(0, 0, n, n), new Vector2(0.5f, 0.5f), 100f);
            }
        }

        static Sprite Fatiado(Texture2D t)
        {
            float b = UiEstilo.Borda.left;
            return Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f), 100f, 0,
                SpriteMeshType.FullRect, new Vector4(b, b, b, b));
        }
    }
}
