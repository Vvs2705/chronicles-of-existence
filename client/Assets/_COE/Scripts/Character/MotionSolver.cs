namespace COE
{
    /// <summary>Resultado de um passo de movimento. Direcao planar ja no espaco do MUNDO.
    /// TemDirecao == false: DirX/DirZ/YawAlvoGraus nao valem nada e quem aplica nao deve girar o personagem.</summary>
    public struct MotionStep
    {
        public float DirX;               // direcao planar desejada, unitaria (x)
        public float DirZ;               // direcao planar desejada, unitaria (z)
        public bool TemDirecao;          // false quando o input esta na zona morta
        public float Velocidade;         // m/s no plano
        public float Velocidade01;       // 0..1 para CharacterAnimator.SetSpeed
        public float YawAlvoGraus;       // rotacao alvo do personagem (graus, eixo Y)
        public float VelocidadeVertical; // m/s ja integrada neste passo (negativa caindo)
    }

    /// <summary>Regra de movimento do personagem em terceira pessoa (T002). C# PURO: sem UnityEngine,
    /// testavel sem cena, como Damage. O MonoBehaviour (CharacterMotor) so aplica o resultado.
    ///
    /// CONTRATO. Entrada: input bruto (x = lateral, y = frente, -1..1), yaw da camera em graus,
    /// se esta correndo, se esta no chao, a velocidade vertical atual e o dt. Saida: MotionStep.
    /// Eixos iguais aos do Unity: yaw 0 = frente em +Z; yaw 90 = frente em +X.</summary>
    public static class MotionSolver
    {
        public const float VelocidadeCaminhadaPadrao = 1.6f;  // crianca de 1,10 m: passada natural medida 0,71 m/s andando e
        public const float VelocidadeCorridaPadrao = 3.8f;    // 1,78 correndo (PassadaMedida, 2026-09-30); era 2,2/4,8 de adulto e o pe
        public const float GravidadePadrao = -20f;            // deslizava. Clip a ~2,2x de cadencia (PrototipoAnimacoes).
        /// <summary>Empurrao para baixo que mantem CharacterController.isGrounded verdadeiro.</summary>
        public const float ColaNoChao = -2f;
        public const float ZonaMorta = 0.05f;

        const float Rad2Deg = 57.29577951308232f;

        public static MotionStep Solve(
            float inputX, float inputY, float yawCameraGraus, bool correndo, bool noChao,
            float velocidadeVertical, float dt,
            float velCaminhada = VelocidadeCaminhadaPadrao,
            float velCorrida = VelocidadeCorridaPadrao,
            float gravidade = GravidadePadrao)
        {
            MotionStep r = default(MotionStep);

            // No chao a velocidade vertical nao acumula: sem isso isGrounded oscila e o personagem "flutua" no degrau.
            r.VelocidadeVertical = noChao && velocidadeVertical <= 0f
                ? ColaNoChao
                : velocidadeVertical + gravidade * dt;

            double mag = System.Math.Sqrt((double)inputX * inputX + (double)inputY * inputY);
            if (mag <= ZonaMorta) return r; // parado: sem direcao, sem velocidade, sem rotacao alvo

            // Stick analogico dosa a velocidade ate 1; diagonal do teclado (1,1) NAO vira 1,41.
            float escala = (float)(mag > 1.0 ? 1.0 : mag);
            float lx = (float)(inputX / mag);
            float ly = (float)(inputY / mag);

            double yawRad = yawCameraGraus / Rad2Deg;
            float cos = (float)System.Math.Cos(yawRad);
            float sin = (float)System.Math.Sin(yawRad);

            // Base da camera no plano: frente = (sin, cos), direita = (cos, -sin) - mesma convencao do Unity.
            r.DirX = lx * cos + ly * sin;
            r.DirZ = -lx * sin + ly * cos;
            r.TemDirecao = true;
            r.YawAlvoGraus = (float)(System.Math.Atan2(r.DirX, r.DirZ) * Rad2Deg);

            r.Velocidade = escala * (correndo ? velCorrida : velCaminhada);
            r.Velocidade01 = velCorrida > 0f ? r.Velocidade / velCorrida : 0f;
            if (r.Velocidade01 > 1f) r.Velocidade01 = 1f;
            return r;
        }
    }
}
