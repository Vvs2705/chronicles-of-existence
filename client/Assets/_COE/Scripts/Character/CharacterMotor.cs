using UnityEngine;

namespace COE
{
    /// <summary>Movimento do jogador em terceira pessoa (T002). Le o input, pergunta a regra ao MotionSolver
    /// (C# puro) e APLICA: CharacterController.Move, rotacao suave para a direcao andada e Speed 0..1 no
    /// CharacterAnimator. Sem Animator (modelo ainda nao existe) o SetSpeed e no-op: o movimento continua valendo.
    /// Dependencias explicitas em campos serializados; a cena e montada por BootstrapSceneBuilder.</summary>
    [RequireComponent(typeof(CharacterController))]
    [DefaultExecutionOrder(-50)] // depois do PlayerInputReader (-100), antes do LateUpdate da camera
    public class CharacterMotor : MonoBehaviour
    {
        [SerializeField] PlayerInputReader input;
        [SerializeField] ThirdPersonCamera cam;
        [SerializeField] CharacterAnimator anim; // opcional; nulo = sem ponte de animacao
        [SerializeField] SomDoJogo som;          // opcional: passos; nulo = mudo
        [Tooltip("Metros andados entre dois sons de passo (crianca de 5 anos; aos 8 a passada cresce pouco).")]
        [SerializeField] float passada = 0.55f;
        float andado;

        [Header("Calibracao (m/s)")]
        [SerializeField] float velocidadeCaminhada = MotionSolver.VelocidadeCaminhadaPadrao;
        [SerializeField] float velocidadeCorrida = MotionSolver.VelocidadeCorridaPadrao;
        [SerializeField] float gravidade = MotionSolver.GravidadePadrao;
        [Tooltip("Quanto o personagem gira por segundo para alinhar com a direcao andada.")]
        [SerializeField] float giroGrausPorSegundo = 720f;

        CharacterController cc;
        float velocidadeVertical;

        /// <summary>Ultimo passo resolvido; util para depurar no Inspector/HUD.</summary>
        public MotionStep Ultimo { get; private set; }

        // Demo automatizada: `COE.exe -autowalk` faz o personagem andar em quadrado sozinho, pelo MESMO caminho
        // de movimento (MotionSolver -> CharacterController). Serve para fotografar/gravar sem depender de teclado
        // injetado pelo Windows, que o Input System ignora quando a janela nao tem foco de verdade.
        // ponytail: input sintetico so para captura; nao substitui playtest com pessoa.
        bool autoWalk;
        float autoTempo;

        /// <summary>Velocidades do corpo da idade (BodyByAge). Idempotente.</summary>
        public void DefinirVelocidades(float caminhada, float corrida)
        {
            velocidadeCaminhada = caminhada;
            velocidadeCorrida = corrida;
        }

        public float VelocidadeCaminhada { get { return velocidadeCaminhada; } }
        public float VelocidadeCorrida { get { return velocidadeCorrida; } }

        void Awake()
        {
            cc = GetComponent<CharacterController>();
            if (anim == null) anim = GetComponent<CharacterAnimator>();
            // So build de desenvolvimento (a activity e exportada: release nao obedece extra de terceiro). DevSceneArg: no
            // Android a flag chega pelo extra "unity" da intent.
            autoWalk = Debug.isDebugBuild && DevSceneArg.Tem("-autowalk");
        }

        /// <summary>Input do jogador, ou o do -autowalk: 4 s para cada lado do quadrado.</summary>
        Vector2 LerMove()
        {
            if (!autoWalk) return input != null ? input.Move : Vector2.zero;
            autoTempo += Time.deltaTime;
            switch (((int)(autoTempo / 4f)) % 4)
            {
                case 0: return new Vector2(0f, 1f);
                case 1: return new Vector2(1f, 0f);
                case 2: return new Vector2(0f, -1f);
                default: return new Vector2(-1f, 0f);
            }
        }

        void Update()
        {
            Vector2 move = LerMove();
            bool correndo = input != null && input.RunHeld;
            // Sem camera o movimento fica relativo a propria frente do personagem (nunca trava o jogo).
            float yawCam = cam != null ? cam.Yaw : transform.eulerAngles.y;

            MotionStep step = MotionSolver.Solve(
                move.x, move.y, yawCam, correndo, cc.isGrounded,
                velocidadeVertical, Time.deltaTime,
                velocidadeCaminhada, velocidadeCorrida, gravidade);
            Ultimo = step;
            velocidadeVertical = step.VelocidadeVertical;

            Vector3 v = new Vector3(step.DirX, 0f, step.DirZ) * step.Velocidade;
            v.y = velocidadeVertical;
            cc.Move(v * Time.deltaTime);
            // Passo pela distancia, nao por AnimationEvent: so o clip de correr tem OnFootstep, e o andar ficava mudo.
            if (som != null && cc.isGrounded && step.Velocidade > 0.1f)
            {
                andado += step.Velocidade * Time.deltaTime;
                if (andado >= passada) { andado -= passada; som.Tocar(Som.Passo, 0.6f); }
            }

            if (step.TemDirecao)
            {
                float yaw = Mathf.MoveTowardsAngle(transform.eulerAngles.y, step.YawAlvoGraus,
                                                   giroGrausPorSegundo * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0f, yaw, 0f);
            }

            if (anim != null) anim.SetSpeed(step.Velocidade01);
        }
    }
}
