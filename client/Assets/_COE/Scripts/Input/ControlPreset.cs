using UnityEngine;

namespace COE
{
    public enum HandPreset { Destro, Canhoto, Tablet }

    /// <summary>Zonas de toque e sensibilidades. Destro = mover na esquerda, agir na direita.
    /// Canhoto espelha. Tablet usa zonas menores nos cantos inferiores (alcance dos polegares).</summary>
    [CreateAssetMenu(menuName = "COE/Control Preset")]
    public class ControlPreset : ScriptableObject
    {
        public HandPreset hand = HandPreset.Destro;

        [Header("Joystick flutuante")]
        [Tooltip("Raio em dp (px @160dpi). ~60dp = 1 cm.")]
        public float joystickRadiusDp = 60f;
        public float deadZone = 0.12f;

        [Header("Camera (graus por dp arrastado)")]
        public float lookSensitivityX = 0.25f;
        public float lookSensitivityY = 0.18f;

        [Header("Gestos")]
        public float tapMaxSeconds = 0.22f;
        public float tapMaxDp = 12f;
        public float flickMinDp = 50f;
        public float flickMaxSeconds = 0.25f;

        public const float TabletMinInches = 6.5f;
        static bool loggedPhone;

        /// <summary>Zonas de canto do Tablet so fazem sentido em tela >= 6,5" (largura/dpi); em celular caem nas metades do
        /// Destro, mantendo so o layout de botoes do Tablet. ponytail: heuristica; dpi desconhecido (0) = confia no preset.</summary>
        public static bool TabletZonesFit(float widthPx, float dpi) { return dpi <= 0f || widthPx / dpi >= TabletMinInches; }

        // Mao usada para as ZONAS (nao para o layout da HUD). Fora do Play (testes EditMode) e sempre o preset.
        HandPreset ZoneHand
        {
            get
            {
                if (hand != HandPreset.Tablet || !Application.isPlaying || TabletZonesFit(Screen.width, Screen.dpi)) return hand;
                if (!loggedPhone) { loggedPhone = true; Debug.Log("ControlPreset: Tablet em tela de " + (Screen.width / Screen.dpi).ToString("0.0") + "\" (< " + TabletMinInches + "\"): zonas de toque do Destro."); }
                return HandPreset.Destro;
            }
        }

        // Rects em fracao de tela (0..1), origem no canto inferior esquerdo.
        public Rect MoveZone()
        {
            HandPreset z = ZoneHand;
            Rect r = z == HandPreset.Tablet ? new Rect(0f, 0f, 0.35f, 0.6f) : new Rect(0f, 0f, 0.5f, 1f);
            return z == HandPreset.Canhoto ? Mirror(r) : r;
        }

        public Rect ActionZone()
        {
            HandPreset z = ZoneHand;
            Rect r = z == HandPreset.Tablet ? new Rect(0.65f, 0f, 0.35f, 0.6f) : new Rect(0.5f, 0f, 0.5f, 1f);
            return z == HandPreset.Canhoto ? Mirror(r) : r;
        }

        static Rect Mirror(Rect r) { return new Rect(1f - r.x - r.width, r.y, r.width, r.height); }

        public static float DpToPx(float dp)
        {
            float dpi = Screen.dpi > 0f ? Screen.dpi : 160f;
            return dp * dpi / 160f;
        }

        public static ControlPreset Default(HandPreset hand)
        {
            var p = CreateInstance<ControlPreset>();
            p.hand = hand;
            return p;
        }
    }
}
