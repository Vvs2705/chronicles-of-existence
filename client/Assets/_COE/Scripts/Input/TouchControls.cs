using System.Collections.Generic;
using UnityEngine;

namespace COE
{
    /// <summary>Um dedo na tela neste frame, ja sem tipo de dispositivo (o PlayerInputReader converte do EnhancedTouch).
    /// Posicao em px com origem no canto inferior esquerdo, a mesma de Screen.safeArea.</summary>
    public struct TouchPoint
    {
        public int id;
        public Vector2 position;
        public bool began;   // TouchPhase.Began
        public bool ended;   // TouchPhase.Ended ou Canceled

        public TouchPoint(int id, Vector2 position, bool began, bool ended)
        {
            this.id = id;
            this.position = position;
            this.began = began;
            this.ended = ended;
        }
    }

    /// <summary>Regra do toque em C# puro (testavel sem cena): qual dedo vira joystick, camera ou botao, e o que cada
    /// um produz no frame. O papel e decidido no toque inicial e fica com o dedo ate ele soltar: o polegar do joystick
    /// que escorrega para o lado da camera continua joystick, e o dedo da Defesa segue defendendo se sair do botao.
    /// Ordem no toque inicial: botao > joystick (so no lado dele, um dedo) > camera (resto da tela, um dedo); dedo
    /// excedente e ignorado. Chame Update uma vez por frame com TODOS os toques ativos.</summary>
    public class TouchControls
    {
        /// <summary>-1..1 com dead zone; zero sem dedo no joystick.</summary>
        public Vector2 Move { get; private set; }
        /// <summary>Graus neste frame (x = yaw, y = pitch).</summary>
        public Vector2 Look { get; private set; }
        public bool JoystickActive { get { return moveId >= 0; } }
        /// <summary>Onde o dedo do joystick encostou (px); base do joystick flutuante.</summary>
        public Vector2 JoystickAnchor { get; private set; }

        /// <summary>Chave de Strings do rotulo do botao na HUD de toque (toque.atq, toque.forte...).</summary>
        public static string ChaveDoRotulo(TouchAction a)
        {
            switch (a)
            {
                case TouchAction.Ataque: return "toque.atq";
                case TouchAction.Forte: return "toque.forte";
                case TouchAction.Defesa: return "toque.def";
                case TouchAction.Magia: return "toque.magia";
                case TouchAction.Esquiva: return "toque.esq";
                default: return "toque.usar";
            }
        }

        /// <summary>Botao tocado NESTE frame.</summary>
        public bool Pressed(TouchAction a) { return (pressed & Bit(a)) != 0; }
        /// <summary>Botao com dedo em cima (estado continuo; Defesa vira BlockHeld).</summary>
        public bool Held(TouchAction a) { return (held & Bit(a)) != 0; }

        int moveId = -1, lookId = -1;
        Vector2 lookLast;
        int[] owner = new int[0]; // dedo dono de cada botao do preset (-1 = livre)
        int pressed, held;

        public void Update(IList<TouchPoint> touches, ControlPreset preset, Rect safe, float dpi)
        {
            Move = Vector2.zero;
            Look = Vector2.zero;
            pressed = 0;
            held = 0;
            if (owner.Length != preset.buttons.Length)
            {
                owner = new int[preset.buttons.Length];
                for (int b = 0; b < owner.Length; b++) owner[b] = -1;
            }

            // Dedo que sumiu sem Ended/Canceled (app pausado, EnhancedTouch religado) nao pode deixar papel preso.
            if (moveId >= 0 && !Has(touches, moveId)) moveId = -1;
            if (lookId >= 0 && !Has(touches, lookId)) lookId = -1;
            for (int b = 0; b < owner.Length; b++)
                if (owner[b] >= 0 && !Has(touches, owner[b])) owner[b] = -1;

            // 1) Dedo novo ganha papel. Antes do passo 2 para a Esquiva deste frame ja ver o Move final.
            for (int i = 0; i < touches.Count; i++)
            {
                TouchPoint t = touches[i];
                if (!t.began) continue;
                Release(t.id); // id reciclado (Android reusa ponteiros) nao herda papel velho
                int b = preset.ButtonAt(t.position, safe, dpi);
                if (b >= 0)
                {
                    pressed |= Bit(preset.buttons[b].action);
                    if (owner[b] < 0) owner[b] = t.id;
                }
                else if (preset.InMoveZone(t.position, safe))
                {
                    if (moveId < 0) { moveId = t.id; JoystickAnchor = t.position; }
                }
                else if (lookId < 0)
                {
                    lookId = t.id;
                    lookLast = t.position;
                }
            }

            // 2) Cada dono le o seu dedo. Soltou = nada neste frame (parar de mover e imediato).
            float pxPorDp = ControlPreset.DpToPx(1f, dpi);
            for (int i = 0; i < touches.Count; i++)
            {
                TouchPoint t = touches[i];
                if (t.ended) { Release(t.id); continue; }
                if (t.id == moveId)
                {
                    Vector2 v = (t.position - JoystickAnchor) / ControlPreset.DpToPx(preset.joystickRadiusDp, dpi);
                    if (v.sqrMagnitude > 1f) v.Normalize();
                    if (v.magnitude >= preset.deadZone) Move = v;
                }
                else if (t.id == lookId)
                {
                    Vector2 d = (t.position - lookLast) / pxPorDp;
                    Look += new Vector2(d.x * preset.lookSensitivityX, d.y * preset.lookSensitivityY);
                    lookLast = t.position;
                }
                else
                {
                    for (int b = 0; b < owner.Length; b++)
                        if (owner[b] == t.id) held |= Bit(preset.buttons[b].action);
                }
            }
        }

        void Release(int id)
        {
            if (moveId == id) moveId = -1;
            if (lookId == id) lookId = -1;
            for (int b = 0; b < owner.Length; b++)
                if (owner[b] == id) owner[b] = -1;
        }

        static bool Has(IList<TouchPoint> touches, int id)
        {
            for (int i = 0; i < touches.Count; i++)
                if (touches[i].id == id) return true;
            return false;
        }

        static int Bit(TouchAction a) { return 1 << (int)a; }
    }
}
