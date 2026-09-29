using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace COE
{
    /// <summary>Le o dispositivo via Input System e expoe o estado do frame (alvo do COE: PC).
    /// Teclado/mouse: WASD move, Shift esq. corre, E interage, botao direito do mouse gira a camera, esquerdo ataca,
    /// Espaco esquiva.
    /// Gamepad (Gamepad.current, sem Input Actions asset): stick esq. move, stick dir. camera, L3 corre, Oeste interage,
    /// Sul ataque, Leste esquiva, LB/RB/LT/RT = slots 0-3 (SkillPressed/SkillHeld).
    /// ponytail: o caminho de toque (EnhancedTouch + zonas do ControlPreset) continua compilando, mas nao
    /// e alvo do COE; apagar junto com ControlPreset quando o T002 definir o esquema de input definitivo.</summary>
    [DefaultExecutionOrder(-100)] // roda antes de Player e Camera no mesmo frame
    public class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] ControlPreset preset;

        public ControlPreset Preset { get { return preset; } set { preset = value; } }

        /// <summary>Troca so a mao (Destro/Canhoto/Tablet) mantendo as sensibilidades do preset atual.
        /// Clona o asset antes de mudar para nao gravar no .asset em Play no Editor.</summary>
        public void SetHand(HandPreset hand)
        {
            if (preset == null) preset = ControlPreset.Default(hand);
            if (preset.hand == hand) return;
            preset = Instantiate(preset);
            preset.hand = hand;
        }
        public Vector2 Move { get; private set; }            // -1..1, ja com dead zone
        public Vector2 Look { get; private set; }            // graus neste frame (x = yaw, y = pitch)
        public bool AttackPressed { get; private set; }      // valido so neste frame
        public bool DodgePressed { get; private set; }
        public bool InteractPressed { get; private set; }    // valido so neste frame
        public bool RunHeld { get; private set; }            // estado continuo: segurar para correr
        public Vector2 DodgeDir { get; private set; }        // direcao do flick em tela, normalizada
        public Vector2 JoystickAnchor { get; private set; }  // px, para desenhar o HUD
        public bool JoystickActive { get; private set; }
        /// <summary>Slot 0-3 pressionado neste frame (so gamepad; a HUD chama TryUseSkill direto).</summary>
        public bool SkillPressed(int i) { return i >= 0 && i < 4 && skillPressed[i]; }
        /// <summary>Slot 0-3 segurado (ex.: bloqueio direcional).</summary>
        public bool SkillHeld(int i) { return i >= 0 && i < 4 && skillHeld[i]; }

        const float StickLookDegPerSec = 180f; // ponytail: sensibilidade fixa do stick direito; expor no ControlPreset se pedirem
        readonly bool[] skillPressed = new bool[4], skillHeld = new bool[4];
        int moveTouchId = -1;
        int lookTouchId = -1;
        Vector2 moveAnchor;

        void Awake()
        {
            if (preset == null) preset = ControlPreset.Default(HandPreset.Destro);
        }

        void OnEnable() { EnhancedTouchSupport.Enable(); }
        void OnDisable() { EnhancedTouchSupport.Disable(); }

        void Update()
        {
            Look = Vector2.zero;
            AttackPressed = false;
            DodgePressed = false;
            InteractPressed = false;
            RunHeld = false;
            for (int i = 0; i < 4; i++) { skillPressed[i] = false; skillHeld[i] = false; }

            ReadTouches();
            ReadGamepad();
#if UNITY_EDITOR || UNITY_STANDALONE
            ReadDesktopFallback();
#endif
        }

        void ReadGamepad()
        {
            Gamepad gp = Gamepad.current;
            if (gp == null) return;
            if (moveTouchId < 0)
            {
                Vector2 m = gp.leftStick.ReadValue();
                if (m.magnitude > 1f) m.Normalize();
                if (m.magnitude >= preset.deadZone) Move = m;
            }
            Vector2 look = gp.rightStick.ReadValue();
            if (look.magnitude >= preset.deadZone) Look += look * StickLookDegPerSec * Time.deltaTime;
            if (gp.buttonSouth.wasPressedThisFrame) AttackPressed = true;
            if (gp.buttonEast.wasPressedThisFrame) { DodgePressed = true; DodgeDir = Move; } // zero = sem direcao (esquiva contextual decide)
            if (gp.buttonWest.wasPressedThisFrame) InteractPressed = true;
            if (gp.leftStickButton.isPressed) RunHeld = true;
            ButtonControl[] slots = { gp.leftShoulder, gp.rightShoulder, gp.leftTrigger, gp.rightTrigger };
            for (int i = 0; i < 4; i++)
            {
                skillPressed[i] |= slots[i].wasPressedThisFrame;
                skillHeld[i] |= slots[i].isPressed;
            }
        }

        void ReadTouches()
        {
            float radiusPx = ControlPreset.DpToPx(preset.joystickRadiusDp);
            float pxPerDp = ControlPreset.DpToPx(1f);
            bool sawMove = false;

            foreach (Touch t in Touch.activeTouches)
            {
                if (t.phase == TouchPhase.Began)
                {
                    if (moveTouchId < 0 && Inside(preset.MoveZone(), t.screenPosition))
                    {
                        moveTouchId = t.touchId;
                        moveAnchor = t.screenPosition;
                        JoystickAnchor = moveAnchor;
                        JoystickActive = true;
                    }
                    else if (lookTouchId < 0 && Inside(preset.ActionZone(), t.screenPosition))
                    {
                        lookTouchId = t.touchId;
                    }
                }

                if (t.touchId == moveTouchId)
                {
                    sawMove = true;
                    Vector2 v = (t.screenPosition - moveAnchor) / radiusPx;
                    if (v.magnitude > 1f) v.Normalize();
                    Move = v.magnitude < preset.deadZone ? Vector2.zero : v;
                    if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) ReleaseMove();
                }
                else if (t.touchId == lookTouchId)
                {
                    // ponytail: o delta de um flick tambem gira a camera por 1-2 frames; aceitavel no greybox.
                    Look = new Vector2(t.delta.x * preset.lookSensitivityX, t.delta.y * preset.lookSensitivityY) / pxPerDp;

                    if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                    {
                        float dur = (float)(t.time - t.startTime);
                        Vector2 travel = t.screenPosition - t.startScreenPosition;
                        float distDp = travel.magnitude / pxPerDp;
                        if (dur <= preset.tapMaxSeconds && distDp <= preset.tapMaxDp)
                        {
                            AttackPressed = true;
                        }
                        else if (dur <= preset.flickMaxSeconds && distDp >= preset.flickMinDp)
                        {
                            DodgePressed = true;
                            DodgeDir = travel.normalized;
                        }
                        lookTouchId = -1;
                    }
                }
            }

            if (!sawMove && moveTouchId >= 0) ReleaseMove(); // dedo sumiu sem fase Ended (raro)
        }

        void ReleaseMove()
        {
            moveTouchId = -1;
            Move = Vector2.zero;
            JoystickActive = false;
        }

        static bool Inside(Rect zone01, Vector2 px)
        {
            return zone01.Contains(new Vector2(px.x / Screen.width, px.y / Screen.height));
        }

#if UNITY_EDITOR || UNITY_STANDALONE
        void ReadDesktopFallback()
        {
            Keyboard kb = Keyboard.current;
            Mouse mouse = Mouse.current;
            if (kb == null || mouse == null) return;

            if (moveTouchId < 0)
            {
                Vector2 m = Vector2.zero;
                if (kb.wKey.isPressed) m.y += 1f;
                if (kb.sKey.isPressed) m.y -= 1f;
                if (kb.dKey.isPressed) m.x += 1f;
                if (kb.aKey.isPressed) m.x -= 1f;
                if (m != Vector2.zero) Move = m.normalized;
            }
            if (mouse.rightButton.isPressed)
            {
                Vector2 d = mouse.delta.ReadValue();
                Look += new Vector2(d.x * preset.lookSensitivityX, d.y * preset.lookSensitivityY);
            }
            if (kb.eKey.wasPressedThisFrame) InteractPressed = true;
            if (kb.leftShiftKey.isPressed) RunHeld = true;
            if (mouse.leftButton.wasPressedThisFrame) AttackPressed = true; // ponytail: sem HUD ainda, nenhum clique e "sobre botao"
            if (kb.spaceKey.wasPressedThisFrame)
            {
                DodgePressed = true;
                DodgeDir = Move == Vector2.zero ? Vector2.down : Move;
            }
        }
#endif
    }
}
