using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Regra de movimento (T002). Puro: nao monta cena, nao precisa de Animator nem de camera.</summary>
    public class MotionSolverTests
    {
        const float Dt = 1f / 60f;

        static MotionStep NoChao(float x, float y, float yawCamera, bool correndo = false)
        {
            return MotionSolver.Solve(x, y, yawCamera, correndo, true, 0f, Dt);
        }

        [Test]
        public void MesmoInput_ComYawDiferente_DaDirecaoDiferente()
        {
            // Yaw 0: a frente da camera e +Z.
            MotionStep a = NoChao(0f, 1f, 0f);
            Assert.IsTrue(a.TemDirecao);
            Assert.AreEqual(0f, a.DirX, 0.001f);
            Assert.AreEqual(1f, a.DirZ, 0.001f);
            Assert.AreEqual(0f, a.YawAlvoGraus, 0.01f);

            // Yaw 90: a MESMA tecla (frente) tem de empurrar para +X.
            MotionStep b = NoChao(0f, 1f, 90f);
            Assert.AreEqual(1f, b.DirX, 0.001f);
            Assert.AreEqual(0f, b.DirZ, 0.001f);
            Assert.AreEqual(90f, b.YawAlvoGraus, 0.01f);
        }

        [Test]
        public void Lateral_SegueADireitaDaCamera()
        {
            MotionStep a = NoChao(1f, 0f, 0f);   // direita com camera olhando +Z = +X
            Assert.AreEqual(1f, a.DirX, 0.001f);
            Assert.AreEqual(0f, a.DirZ, 0.001f);

            MotionStep b = NoChao(1f, 0f, 90f);  // camera olhando +X: direita = -Z
            Assert.AreEqual(0f, b.DirX, 0.001f);
            Assert.AreEqual(-1f, b.DirZ, 0.001f);
            Assert.AreEqual(180f, System.Math.Abs(b.YawAlvoGraus), 0.01f);
        }

        [Test]
        public void InputZero_VelocidadeZeroESemRotacaoAlvo()
        {
            MotionStep r = NoChao(0f, 0f, 45f);
            Assert.IsFalse(r.TemDirecao, "sem input nao ha direcao: o personagem nao pode girar sozinho");
            Assert.AreEqual(0f, r.Velocidade);
            Assert.AreEqual(0f, r.Velocidade01);
        }

        [Test]
        public void Correr_EMaisRapidoQueAndar()
        {
            MotionStep andando = NoChao(0f, 1f, 0f);
            MotionStep correndo = NoChao(0f, 1f, 0f, true);
            Assert.Greater(correndo.Velocidade, andando.Velocidade);
            Assert.AreEqual(1f, correndo.Velocidade01, 0.001f, "correndo a fundo = Speed 1 no Animator");
            Assert.Less(andando.Velocidade01, 1f);
        }

        [Test]
        public void Diagonal_NaoPassaDeUm()
        {
            MotionStep r = NoChao(1f, 1f, 0f, true); // teclado: W+D dao (1,1), modulo 1,41
            Assert.AreEqual(1f, r.Velocidade01, 0.001f, "diagonal nao pode correr mais que a reta");
            Assert.AreEqual(MotionSolver.VelocidadeCorridaPadrao, r.Velocidade, 0.001f);
            float mod = (float)System.Math.Sqrt(r.DirX * r.DirX + r.DirZ * r.DirZ);
            Assert.AreEqual(1f, mod, 0.001f, "a direcao devolvida e sempre unitaria");
        }

        [Test]
        public void StickParcial_DosaAVelocidade()
        {
            MotionStep meio = NoChao(0f, 0.5f, 0f, true);
            Assert.AreEqual(MotionSolver.VelocidadeCorridaPadrao * 0.5f, meio.Velocidade, 0.001f);
            float mod = (float)System.Math.Sqrt(meio.DirX * meio.DirX + meio.DirZ * meio.DirZ);
            Assert.AreEqual(1f, mod, 0.001f);
        }

        [Test]
        public void Gravidade_ColaNoChaoEAcumulaNoAr()
        {
            MotionStep chao = MotionSolver.Solve(0f, 0f, 0f, false, true, -50f, Dt);
            Assert.AreEqual(MotionSolver.ColaNoChao, chao.VelocidadeVertical, 0.001f);

            MotionStep ar = MotionSolver.Solve(0f, 0f, 0f, false, false, 0f, Dt);
            Assert.Less(ar.VelocidadeVertical, 0f);
            MotionStep ar2 = MotionSolver.Solve(0f, 0f, 0f, false, false, ar.VelocidadeVertical, Dt);
            Assert.Less(ar2.VelocidadeVertical, ar.VelocidadeVertical, "cai cada vez mais rapido");
        }
    }
}
