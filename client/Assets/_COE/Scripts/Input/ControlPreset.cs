using System;
using UnityEngine;

namespace COE
{
    public enum HandPreset { Destro, Canhoto }

    /// <summary>Botao da HUD de toque. Cada um alimenta a acao de mesmo papel no PlayerInputReader (contrato T011):
    /// Ataque = AttackPressed, Forte = HeavyPressed, Defesa = BlockHeld (segurado), Magia = CastPressed,
    /// Esquiva = DodgePressed + DodgeDir, Interagir = InteractPressed.</summary>
    public enum TouchAction { Ataque, Forte, Defesa, Magia, Esquiva, Interagir }

    [Serializable]
    public struct TouchButton
    {
        public TouchAction action;
        [Tooltip("Centro em dp a partir do canto INFERIOR da area segura do lado dos botoes: x anda para dentro da tela, y para cima.")]
        public Vector2 centerDp;
        [Tooltip("Raio do alvo em dp. Abaixo de MinTargetDp/2 (diametro 48 dp) e forcado para cima.")]
        public float radiusDp;

        public TouchButton(TouchAction action, float xDp, float yDp, float radiusDp)
        {
            this.action = action;
            centerDp = new Vector2(xDp, yDp);
            this.radiusDp = radiusDp;
        }
    }

    /// <summary>DEFINICAO do layout de toque em PAISAGEM (ADR-0006); nada de estado de jogo aqui.
    /// Destro: joystick flutuante na parte esquerda, arrastar no resto da tela gira a camera, botoes no canto inferior
    /// direito, ao alcance do polegar. Canhoto espelha tudo. Medidas em dp (px a 160 dpi) contadas a partir de
    /// Screen.safeArea, entao notch e barra de gestos nao comem botao. Quem decide qual dedo vira qual acao e
    /// TouchControls (C# puro); o PlayerInputReader so repassa.
    /// ponytail: layout padrao e hipotese de mesa (tela de referencia 640x360 dp); calibrar no primeiro teste em
    /// aparelho. Botao contextual (Interagir so com alvo) e reposicionar pelo jogador ficam para a UI da T013.</summary>
    [CreateAssetMenu(menuName = "COE/Control Preset")]
    public class ControlPreset : ScriptableObject
    {
        /// <summary>Menor alvo de toque aceito, em dp de diametro (recomendacao Android/Material).</summary>
        public const float MinTargetDp = 48f;
        const float DpiPadrao = 160f; // Screen.dpi = 0 quando o aparelho nao informa

        public HandPreset hand = HandPreset.Destro;

        [Header("Joystick flutuante")]
        [Tooltip("Fracao da largura da area segura, a partir da borda do lado do joystick, onde um toque vira joystick.")]
        [Range(0.2f, 0.8f)] public float moveZoneWidth = 0.5f;
        [Tooltip("Raio em dp (px @160dpi). 60 dp ~ 1 cm.")]
        public float joystickRadiusDp = 60f;
        public float deadZone = 0.12f;

        [Header("Camera (graus por dp arrastado)")]
        public float lookSensitivityX = 0.25f;
        public float lookSensitivityY = 0.18f;

        [Header("Botoes (canto inferior do lado oposto ao joystick)")]
        public TouchButton[] buttons = DefaultButtons();

        /// <summary>Layout padrao: Ataque grande no canto; Forte, Magia e Esquiva num arco em volta dele; Defesa e
        /// Interagir nas pontas. Cabe na metade de uma tela de 640x360 dp sem sobrepor (TouchControlsTests).</summary>
        public static TouchButton[] DefaultButtons()
        {
            return new[]
            {
                new TouchButton(TouchAction.Ataque, 90f, 90f, 45f),
                new TouchButton(TouchAction.Forte, 200f, 70f, 34f),
                new TouchButton(TouchAction.Magia, 180f, 180f, 34f),
                new TouchButton(TouchAction.Esquiva, 70f, 200f, 34f),
                new TouchButton(TouchAction.Defesa, 280f, 64f, 34f),
                new TouchButton(TouchAction.Interagir, 64f, 280f, 34f),
            };
        }

        public static float DpToPx(float dp, float dpi) { return dp * (dpi > 0f ? dpi : DpiPadrao) / DpiPadrao; }

        /// <summary>Um toque que COMECA aqui vira joystick. A divisa acompanha a area segura; a faixa fora dela (lado
        /// do notch) conta para o lado da borda.</summary>
        public bool InMoveZone(Vector2 px, Rect safe)
        {
            float w = safe.width * moveZoneWidth;
            return hand == HandPreset.Canhoto ? px.x >= safe.xMax - w : px.x < safe.x + w;
        }

        public Vector2 ButtonCenterPx(int i, Rect safe, float dpi) { return FromCorner(buttons[i].centerDp, true, safe, dpi); }

        public float ButtonRadiusPx(int i, float dpi) { return DpToPx(Mathf.Max(buttons[i].radiusDp, MinTargetDp * 0.5f), dpi); }

        /// <summary>Indice do botao sob o ponto (o de centro mais proximo, se dois alvos se tocarem) ou -1.</summary>
        public int ButtonAt(Vector2 px, Rect safe, float dpi)
        {
            int best = -1;
            float bestSq = float.MaxValue;
            for (int i = 0; i < buttons.Length; i++)
            {
                float sq = (px - ButtonCenterPx(i, safe, dpi)).sqrMagnitude;
                float r = ButtonRadiusPx(i, dpi);
                if (sq <= r * r && sq < bestSq) { best = i; bestSq = sq; }
            }
            return best;
        }

        /// <summary>Onde a HUD desenha o joystick em repouso (dica de onde tocar).</summary>
        public Vector2 JoystickRestPx(Rect safe, float dpi)
        {
            float d = joystickRadiusDp + 40f;
            return FromCorner(new Vector2(d, d), false, safe, dpi);
        }

        // Canto inferior da area segura do lado dos botoes (ou do joystick); x conta para dentro da tela.
        Vector2 FromCorner(Vector2 dp, bool buttonSide, Rect safe, float dpi)
        {
            Vector2 px = dp * DpToPx(1f, dpi);
            bool direita = buttonSide == (hand == HandPreset.Destro);
            return new Vector2(direita ? safe.xMax - px.x : safe.x + px.x, safe.y + px.y);
        }

        public static ControlPreset Default(HandPreset hand)
        {
            var p = CreateInstance<ControlPreset>();
            p.hand = hand;
            return p;
        }
    }
}
