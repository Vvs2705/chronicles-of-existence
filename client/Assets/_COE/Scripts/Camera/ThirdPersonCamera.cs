using UnityEngine;

namespace COE
{
    /// <summary>Camera em terceira pessoa: orbita por arrasto, colisao por SphereCast, suavizacao
    /// e mira suave (gira parcialmente para o inimigo HOSTIL mais proximo do centro quando o jogador ataca).</summary>
    public class ThirdPersonCamera : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] PlayerInputReader input;
        // ponytail: enquadramento da crianca de 5 anos derivado so da altura (hipotese v0, calibrar no playtest).
        // Pivo a 85% da altura (~0,94 m, linha dos olhos): a camera fica abaixo do adulto e a vila "cresce" em volta.
        // Distancia = 2,5x a altura (~2,75 m), a mesma proporcao do enquadramento adulto antigo (1,5 m / 5 m sobre
        // 2 m): com o FOV padrao a crianca ocupa o mesmo ~1/3 da altura da tela. Salto para 8 anos = trocar
        // Crianca5 por Crianca8 aqui; se o salto virar runtime, expor um setter chamado pelo LifeSystem.
        [SerializeField] Vector3 pivotOffset = new Vector3(0f, BodyScale.Crianca5 * 0.85f, 0f);
        [SerializeField] float distance = BodyScale.Crianca5 * 2.5f;
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

        /// <summary>Yaw suavizado; o controller do jogador (T002) usa para movimento relativo a camera.</summary>
        public float Yaw { get { return smoothYaw; } }

        void Start()
        {
            if (target != null) yaw = target.eulerAngles.y;
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
                if (input.AttackPressed && TrainingProgress.PodeTreinar()) StartSoftAim();
                if (input.Look.sqrMagnitude > 0f) aiming = false; // jogador assumiu o controle
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
