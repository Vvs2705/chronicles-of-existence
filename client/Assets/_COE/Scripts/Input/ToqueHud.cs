using UnityEngine;
using UnityEngine.UI;

namespace COE
{
    /// <summary>Desenho dos controles de toque (joystick e os 6 botoes) em uGUI (Bloco D, 2026-10-04). So MOSTRA: quem le o
    /// dedo e decide a acao e o PlayerInputReader (TouchControls). A geometria e a mesma do hit-test (ControlPreset, mesma
    /// area segura e Dpi), entao o disco desenhado e o alvo do toque. Some com modal aberto (UiFundo.HaModal) e no PC sem
    /// toque. Rotulos por Strings (TouchControls.ChaveDoRotulo).</summary>
    public class ToqueHud : MonoBehaviour
    {
        [SerializeField] PlayerInputReader leitor;   // ligado pelo gerador de cena

        public PlayerInputReader Leitor { get { return leitor; } set { leitor = value; } }

        static readonly Color CorBase = new Color(1f, 1f, 1f, 0.12f);
        static readonly Color CorAlca = new Color(1f, 1f, 1f, 0.45f);
        static readonly Color CorBotao = new Color(1f, 1f, 1f, 0.22f);
        static readonly Color CorSegurando = new Color(1f, 0.8f, 0.25f, 0.7f);

        Canvas canvas;
        Image baseJoystick, alca;
        Image[] botoes = new Image[0];
        Text[] rotulos = new Text[0];
        ControlPreset presetMontado;
        Rect safeMontado;
        float dpiMontado = -1f, raioJoystick;
        int alturaMontada;
        HandPreset maoMontada;

        /// <summary>Disco do botao i (teste confere contra o ControlPreset).</summary>
        public RectTransform Botao(int i) { return botoes[i].rectTransform; }
        public bool Visivel { get { return canvas != null && canvas.enabled; } }

        void Awake()
        {
            StringsLoader.EnsureLoaded();
            canvas = Tela.NovoCanvas(transform, "ToqueCanvas", Tela.CamadaToque);
            baseJoystick = Tela.Imagem(canvas.transform, "Joystick", Tela.SpriteDisco, CorBase);
            alca = Tela.Imagem(canvas.transform, "Alca", Tela.SpriteDisco, CorAlca);
            canvas.enabled = false;
        }

        void LateUpdate()
        {
            ControlPreset p = leitor != null ? leitor.Preset : null;
            bool mostrar = p != null && leitor.isActiveAndEnabled && leitor.MostraToque && !UiFundo.HaModal;
            if (canvas.enabled != mostrar) canvas.enabled = mostrar;
            if (!mostrar) return;

            Rect safe = Screen.safeArea;
            float dpi = Tela.Dpi;
            // A mao entra na conta: o menu troca preset.hand no MESMO objeto (sem isso os botoes ficavam do lado antigo
            // enquanto o toque ja valia no lado novo; revisao do Bloco D).
            if (p != presetMontado || p.hand != maoMontada || safe != safeMontado || dpi != dpiMontado || Screen.height != alturaMontada
                || p.buttons.Length != botoes.Length)
                Montar(p, safe, dpi);

            Vector2 c = leitor.JoystickAtivo ? leitor.JoystickAncora : p.JoystickRestPx(safe, dpi);
            Tela.Colocar(baseJoystick.rectTransform, Quadrado(c, raioJoystick));
            Tela.Colocar(alca.rectTransform, Quadrado(c + leitor.JoystickMove * raioJoystick, raioJoystick * 0.45f));
            baseJoystick.color = leitor.JoystickAtivo ? CorBotao : CorBase;
            for (int i = 0; i < botoes.Length; i++)
            {
                // Aceso enquanto o dedo segura: e o estado "segurando" da Defesa (BlockHeld).
                Color cor = leitor.Segurando(p.buttons[i].action) ? CorSegurando : CorBotao;
                if (botoes[i].color != cor) botoes[i].color = cor;
            }
        }

        /// <summary>So quando muda tela, area segura, mao ou densidade: o resto do quadro so move o joystick.</summary>
        void Montar(ControlPreset p, Rect safe, float dpi)
        {
            if (botoes.Length != p.buttons.Length)
            {
                foreach (Image b in botoes) if (b != null) Destroy(b.gameObject);
                botoes = new Image[p.buttons.Length];
                rotulos = new Text[p.buttons.Length];
                for (int i = 0; i < botoes.Length; i++)
                {
                    botoes[i] = Tela.Imagem(canvas.transform, "Botao_" + p.buttons[i].action, Tela.SpriteDisco, CorBotao);
                    rotulos[i] = Tela.Texto(botoes[i].transform, "Rotulo", 14, TextAnchor.MiddleCenter, Color.white);
                    rotulos[i].fontStyle = FontStyle.Bold;
                    rotulos[i].horizontalOverflow = HorizontalWrapMode.Overflow;
                    Tela.Esticar(rotulos[i].rectTransform, 0f);
                }
            }
            int fonte = Mathf.RoundToInt(ControlPreset.DpToPx(14f, dpi));
            for (int i = 0; i < botoes.Length; i++)
            {
                Tela.Colocar(botoes[i].rectTransform, Quadrado(p.ButtonCenterPx(i, safe, dpi), p.ButtonRadiusPx(i, dpi)));
                rotulos[i].fontSize = fonte;
                rotulos[i].text = Strings.Get(TouchControls.ChaveDoRotulo(p.buttons[i].action));
            }
            raioJoystick = ControlPreset.DpToPx(p.joystickRadiusDp, dpi);
            presetMontado = p;
            maoMontada = p.hand;
            safeMontado = safe;
            dpiMontado = dpi;
            alturaMontada = Screen.height;
        }

        static Rect Quadrado(Vector2 centro, float raio) { return new Rect(centro.x - raio, centro.y - raio, 2f * raio, 2f * raio); }
    }
}
