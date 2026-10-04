using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Utilities;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace COE
{
    /// <summary>Le os dispositivos via Input System e expoe ACOES por frame, independentes de dispositivo (ADR-0006:
    /// alvo mobile, toque; gamepad opcional; teclado so para desenvolvimento). Quem consome (PlayerCombat, motor,
    /// camera) so le as propriedades, nunca o dispositivo.
    /// Acoes de combate (T011): AttackPressed (ataque leve), HeavyPressed (forte), BlockHeld (defesa, segurada),
    /// CastPressed (magia), DodgePressed + DodgeDir (esquiva).
    /// CONTRATO DE FONTE: cada dispositivo e um Read*() chamado no Update DEPOIS do reset e so LIGA flags (nunca
    /// desliga), entao as fontes somam. Fonte nova (toque: joystick virtual + botoes) entra como mais um Read*().
    /// Toque (ADR-0006, paisagem; layout no ControlPreset, regra em TouchControls): joystick flutuante no lado do
    /// joystick, arrastar no resto da tela gira a camera, botoes no canto do polegar: Ataque, Forte, Defesa (segurado),
    /// Magia, Esquiva (direcao = joystick; zero = contextual) e Interagir. Multitoque: cada dedo tem um papel.
    /// Teclado/mouse: WASD move, Shift esq. corre, E interage, botao direito do mouse gira a camera, esquerdo ataca,
    /// Espaco esquiva; combate (T011): Q ataque forte, C segurado bloqueia, R magia.
    /// Gamepad (Gamepad.current, sem Input Actions asset): stick esq. move, stick dir. camera, L3 corre, Oeste interage,
    /// Sul ataque, Leste esquiva, RT ataque forte, LT segurado bloqueia, LB magia; LB/RB/LT/RT tambem sao os
    /// slots 0-3 (SkillPressed/SkillHeld).
    /// ponytail: teclas e sensibilidades fixas no codigo (o jogador so escala o olhar: LookMultiplier). Rebind e
    /// opcoes de acessibilidade pedem um Input Actions asset (+ tela de opcoes); quando existir, este leitor passa a
    /// ler as actions e mantem as mesmas propriedades.</summary>
    [DefaultExecutionOrder(-100)] // roda antes de Player e Camera no mesmo frame
    public class PlayerInputReader : MonoBehaviour
    {
        [Tooltip("Layout de toque (ADR-0006). O gerador liga o asset; nulo = padrao destro em memoria.")]
        [SerializeField] ControlPreset preset;
        [Tooltip("Desenvolvimento: mostra a HUD de toque sem touchscreen e simula o dedo com o mouse (TouchSimulation).")]
        [SerializeField] bool touchHudDev;

        public ControlPreset Preset { get { return preset; } set { preset = value; } }
        /// <summary>Sensibilidade do jogador (menu de configuracoes) sobre o Look de todo dispositivo; 1 = calibragem
        /// do preset/codigo.</summary>
        public float LookMultiplier { get; set; } = 1f;

        public Vector2 Move { get; private set; }            // -1..1, ja com dead zone
        public Vector2 Look { get; private set; }            // graus neste frame (x = yaw, y = pitch)
        public bool AttackPressed { get; private set; }      // valido so neste frame
        public bool DodgePressed { get; private set; }
        public bool InteractPressed { get; private set; }    // valido so neste frame
        public bool RunHeld { get; private set; }            // estado continuo: segurar para correr
        public Vector2 DodgeDir { get; private set; }        // direcao da esquiva no espaco do input, normalizada
        public bool HeavyPressed { get; private set; }       // ataque forte (T011), valido so neste frame
        public bool BlockHeld { get; private set; }          // bloqueio (T011), estado continuo: segurar para defender
        public bool CastPressed { get; private set; }        // magia (T011), valido so neste frame
        /// <summary>Slot 0-3 pressionado neste frame (so gamepad; a HUD chama TryUseSkill direto).</summary>
        public bool SkillPressed(int i) { return i >= 0 && i < 4 && skillPressed[i]; }
        /// <summary>Slot 0-3 segurado (ex.: bloqueio direcional).</summary>
        public bool SkillHeld(int i) { return i >= 0 && i < 4 && skillHeld[i]; }

        const float DeadZone = 0.12f;
        const float StickLookDegPerSec = 180f; // stick direito
        const float MouseDegPorPixelX = 0.25f;
        const float MouseDegPorPixelY = 0.18f;
        readonly bool[] skillPressed = new bool[4], skillHeld = new bool[4];
        readonly TouchControls toque = new TouchControls();
        readonly List<TouchPoint> toques = new List<TouchPoint>(10);
        bool simulandoToque;

        void Awake()
        {
            if (preset == null) preset = ControlPreset.Default(HandPreset.Destro);
        }

        void OnEnable()
        {
            EnhancedTouchSupport.Enable(); // contado por referencia: so desliga de fato quando o ultimo chamar Disable
            simulandoToque = touchHudDev || DevSceneArg.Tem("-toque");   // tools/run_windows.ps1 -Celular
            Tela.SimulandoToque = simulandoToque;
            if (simulandoToque) TouchSimulation.Enable();
        }

        /// <summary>Densidade do layout de toque: Tela.Dpi (a da tela, ou a simulada no modo celular do PC).</summary>
        static float Dpi() { return Tela.Dpi; }

        void OnDisable()
        {
            if (simulandoToque) TouchSimulation.Disable();
            EnhancedTouchSupport.Disable();
        }

        void Update()
        {
            Move = Vector2.zero; // soltou a tecla/stick = parado; sem isso o ultimo Move ficava valendo
            Look = Vector2.zero;
            AttackPressed = false;
            DodgePressed = false;
            InteractPressed = false;
            RunHeld = false;
            HeavyPressed = false;
            BlockHeld = false;
            CastPressed = false;
            for (int i = 0; i < 4; i++) { skillPressed[i] = false; skillHeld[i] = false; }

            ReadTouch();
            ReadGamepad();
            ReadKeyboardMouse();
            Look *= LookMultiplier;
        }

        void ReadTouch()
        {
            toques.Clear();
            ReadOnlyArray<Touch> ativos = Touch.activeTouches;
            for (int i = 0; i < ativos.Count; i++)
            {
                Touch t = ativos[i];
                toques.Add(new TouchPoint(t.touchId, t.screenPosition, t.phase == TouchPhase.Began,
                                          t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled));
            }
            toque.Update(toques, preset, Screen.safeArea, Dpi());

            if (toque.Move != Vector2.zero) Move = toque.Move;
            Look += toque.Look;
            if (toque.Pressed(TouchAction.Ataque)) AttackPressed = true;
            if (toque.Pressed(TouchAction.Forte)) HeavyPressed = true;
            if (toque.Held(TouchAction.Defesa)) BlockHeld = true;
            if (toque.Pressed(TouchAction.Magia)) CastPressed = true;
            if (toque.Pressed(TouchAction.Interagir)) InteractPressed = true;
            if (toque.Pressed(TouchAction.Esquiva)) { DodgePressed = true; DodgeDir = toque.Move; } // zero = esquiva contextual (como no gamepad)
        }

        void ReadGamepad()
        {
            Gamepad gp = Gamepad.current;
            if (gp == null) return;
            Vector2 m = gp.leftStick.ReadValue();
            if (m.magnitude > 1f) m.Normalize();
            if (m.magnitude >= DeadZone) Move = m;
            Vector2 look = gp.rightStick.ReadValue();
            if (look.magnitude >= DeadZone) Look += look * StickLookDegPerSec * Time.deltaTime;
            if (gp.buttonSouth.wasPressedThisFrame) AttackPressed = true;
            if (gp.buttonEast.wasPressedThisFrame) { DodgePressed = true; DodgeDir = Move; } // zero = sem direcao (esquiva contextual decide)
            if (gp.buttonWest.wasPressedThisFrame) InteractPressed = true;
            if (gp.leftStickButton.isPressed) RunHeld = true;
            if (gp.rightTrigger.wasPressedThisFrame) HeavyPressed = true;
            if (gp.leftTrigger.isPressed) BlockHeld = true;
            if (gp.leftShoulder.wasPressedThisFrame) CastPressed = true;
            Slot(0, gp.leftShoulder); Slot(1, gp.rightShoulder); Slot(2, gp.leftTrigger); Slot(3, gp.rightTrigger);   // sem array por quadro
        }

        void Slot(int i, ButtonControl b) { skillPressed[i] |= b.wasPressedThisFrame; skillHeld[i] |= b.isPressed; }

        void ReadKeyboardMouse()
        {
            Keyboard kb = Keyboard.current;
            Mouse mouse = Mouse.current;
            if (kb == null || mouse == null) return;

            Vector2 m = Vector2.zero;
            if (kb.wKey.isPressed) m.y += 1f;
            if (kb.sKey.isPressed) m.y -= 1f;
            if (kb.dKey.isPressed) m.x += 1f;
            if (kb.aKey.isPressed) m.x -= 1f;
            if (m != Vector2.zero) Move = m.normalized;
            if (mouse.rightButton.isPressed)
            {
                Vector2 d = mouse.delta.ReadValue();
                Look += new Vector2(d.x * MouseDegPorPixelX, d.y * MouseDegPorPixelY);
            }
            if (kb.eKey.wasPressedThisFrame) InteractPressed = true;
            if (kb.leftShiftKey.isPressed) RunHeld = true;
            if (kb.qKey.wasPressedThisFrame) HeavyPressed = true;
            if (kb.cKey.isPressed) BlockHeld = true;
            if (kb.rKey.wasPressedThisFrame) CastPressed = true;
            // ponytail: o clique nao sabe se caiu "sobre botao". Com a HUD de toque simulada o clique ja e dedo: nao ataca.
            if (mouse.leftButton.wasPressedThisFrame && !simulandoToque) AttackPressed = true;
            if (kb.spaceKey.wasPressedThisFrame)
            {
                DodgePressed = true;
                DodgeDir = Move == Vector2.zero ? Vector2.down : Move;
            }
        }

        // ---- estado do toque para quem desenha (ToqueHud): so leitura ----
        /// <summary>Ha toque para mostrar: touchscreen de verdade ou o modo celular do PC (-toque).</summary>
        public bool MostraToque { get { return simulandoToque || Touchscreen.current != null; } }
        public bool JoystickAtivo { get { return toque.JoystickActive; } }
        public Vector2 JoystickAncora { get { return toque.JoystickAnchor; } }
        /// <summary>Deslocamento do joystick de toque (-1..1), so do dedo (o Move soma todas as fontes).</summary>
        public Vector2 JoystickMove { get { return toque.Move; } }
        public bool Segurando(TouchAction a) { return toque.Held(a); }
    }
}
