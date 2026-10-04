using UnityEngine;

namespace COE
{
    /// <summary>Camera em terceira pessoa: orbita por arrasto, colisao por SphereCast, suavizacao
    /// e mira suave (gira parcialmente para o inimigo HOSTIL mais proximo do centro quando o jogador ataca).</summary>
    public class ThirdPersonCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        PlayerCombat combate;   // do target: a mira suave so existe com o treino liberado
        [SerializeField] PlayerInputReader input;
        // ponytail: enquadramento derivado so da altura (Corpo: pivo a 85%, distancia 2,5x; hipotese v0, calibrar no
        // playtest). Nasce na crianca de 5 anos (~0,94 m / ~2,75 m, mesma proporcao do enquadramento adulto antigo:
        // a crianca ocupa ~1/3 da altura da tela). Depois do salto o BodyByAge chama Enquadrar com o corpo de 8.
        [SerializeField] Vector3 pivotOffset = new Vector3(0f, Corpo.DaIdade(5).PivoCamera, 0f);
        [SerializeField] float distance = Corpo.DaIdade(5).DistanciaCamera;
        [SerializeField] float minPitch = -20f;
        [SerializeField] float maxPitch = 60f;
        [SerializeField] float startPitch = 15f;
        [SerializeField] float smoothTime = 0.08f;
        [SerializeField] float collisionRadius = 0.25f;
        [SerializeField] LayerMask collisionMask = ~0;

        [Header("Mira suave")]
        [SerializeField] float aimRange = 8f;
        [SerializeField] float aimMaxAngle = 60f;    // so ajuda se o inimigo ja esta razoavelmente central
        [SerializeField] float aimStrength = 0.35f;  // 0 = nada, 1 = centraliza totalmente
        [SerializeField] float aimSpeed = 6f;

        float yaw, pitch;
        float smoothYaw, smoothPitch, yawVel, pitchVel;
        float aimYawTarget;
        bool aiming;
        readonly Collider[] buffer = new Collider[16];

        Transform foco;

        /// <summary>Conversa: a camera gira para enquadrar 'alvo' por cima do ombro do jogador (null = solta). Ao soltar,
        /// a camera fica onde esta.</summary>
        public void Focar(Transform alvo) { foco = alvo; }
        public Transform Foco { get { return foco; } }

        /// <summary>Yaw suavizado; o controller do jogador (T002) usa para movimento relativo a camera.</summary>
        public float Yaw { get { return smoothYaw; } }

        /// <summary>Pivo e distancia pela altura do corpo (BodyByAge, no Awake do Player).</summary>
        public void Enquadrar(Corpo corpo)
        {
            pivotOffset = new Vector3(0f, corpo.PivoCamera, 0f);
            distance = corpo.DistanciaCamera;
        }

        void Start()
        {
            if (target != null) { yaw = target.eulerAngles.y; combate = target.GetComponent<PlayerCombat>(); }
            pitch = startPitch;
            smoothYaw = yaw;
            smoothPitch = pitch;
        }

        void LateUpdate()
        {
            if (target == null) return;

            if (input != null)
            {
                yaw += input.Look.x;
                pitch = Mathf.Clamp(pitch - input.Look.y, minPitch, maxPitch);
                // Mesmo portao de idade do PlayerCombat: aos 5 anos o ataque e recusado e a camera nao gira para o instrutor.
                if (input.AttackPressed && combate != null && combate.PodeTreinar) StartSoftAim();
                if (input.Look.sqrMagnitude > 0f) aiming = false; // jogador assumiu o controle
            }

            if (foco != null)
            {
                Vector3 paraFoco = foco.position - target.position;
                paraFoco.y = 0f;
                if (paraFoco.sqrMagnitude > 0.01f)
                {
                    // +20 graus: o NPC aparece ao lado da crianca, nao escondido atras dela.
                    float alvoYaw = Mathf.Atan2(paraFoco.x, paraFoco.z) * Mathf.Rad2Deg + 20f;
                    yaw = Mathf.MoveTowardsAngle(yaw, alvoYaw, 240f * Time.deltaTime);
                    pitch = Mathf.MoveTowards(pitch, 8f, 60f * Time.deltaTime);
                }
            }

            if (aiming)
            {
                yaw = Mathf.LerpAngle(yaw, aimYawTarget, aimSpeed * Time.deltaTime);
                if (Mathf.Abs(Mathf.DeltaAngle(yaw, aimYawTarget)) < 0.5f) aiming = false;
            }

            smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawVel, smoothTime);
            smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchVel, smoothTime);

            Quaternion rot = Quaternion.Euler(smoothPitch, smoothYaw, 0f);
            Vector3 pivot = target.position + pivotOffset;
            Vector3 back = rot * Vector3.back;

            float d = distance;
            RaycastHit hit;
            if (Physics.SphereCast(pivot, collisionRadius, back, out hit, distance, collisionMask, QueryTriggerInteraction.Ignore))
                d = Mathf.Max(hit.distance, collisionRadius * 2f);

            transform.position = pivot + back * d;
            transform.rotation = rot;
        }

        // Escolhe o inimigo com menor angulo em relacao a frente da camera e gira o yaw
        // apenas aimStrength do caminho. Nao atravessa paredes (Linecast do pivot ate o alvo).
        // So Side.Hostile: na vila, clicar perto de NPC, animal ou morador (Neutral/sem Faction) nao puxa a camera.
        void StartSoftAim()
        {
            Vector3 pivot = target.position + pivotOffset;
            Vector3 fwd = Quaternion.Euler(0f, yaw, 0f) * Vector3.forward;
            int n = Physics.OverlapSphereNonAlloc(target.position, aimRange, buffer, ~0, QueryTriggerInteraction.Ignore);
            float bestAngle = aimMaxAngle;
            Vector3 bestDir = Vector3.zero;

            for (int i = 0; i < n; i++)
            {
                Health h = buffer[i].GetComponentInParent<Health>();
                if (h == null || h.transform == target || h.Dead) continue;
                if (Faction.Of(h.gameObject) != Side.Hostile) continue;
                Vector3 to = h.transform.position - target.position;
                to.y = 0f;
                if (to.sqrMagnitude < 0.01f) continue;
                float ang = Vector3.Angle(fwd, to);
                if (ang >= bestAngle) continue;
                // O fim da linha fica na superficie do proprio alvo (topo da capsula): so conta como parede o que NAO e ele.
                RaycastHit oc;
                if (Physics.Linecast(pivot, h.transform.position + Vector3.up, out oc, collisionMask, QueryTriggerInteraction.Ignore)
                    && oc.collider.GetComponentInParent<Health>() != h) continue;
                bestAngle = ang;
                bestDir = to;
            }

            if (bestDir == Vector3.zero) return;
            float targetYaw = Mathf.Atan2(bestDir.x, bestDir.z) * Mathf.Rad2Deg;
            aimYawTarget = yaw + Mathf.DeltaAngle(yaw, targetYaw) * aimStrength;
            aiming = true;
        }
    }
}
