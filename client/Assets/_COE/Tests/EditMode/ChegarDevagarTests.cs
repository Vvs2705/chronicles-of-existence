using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>"Chegar devagar" (ADR-0010 adendo 10; ficha lysa C5), a regra pura do bicho calmo da q05: parado perto
    /// acalma, longe ou andando nao, sair do raio zera, correr perto assusta. Estado de cena: regra nova (cena recarregada,
    /// app reaberto) comeca assustada. Sem cena.</summary>
    public class ChegarDevagarTests
    {
        const float Perto = 1f;
        const float Longe = 2f;
        const float Andando = MotionSolver.VelocidadeCaminhadaPadrao;
        const float Correndo = MotionSolver.VelocidadeCorridaPadrao;
        const float Dt = 0.25f;   // exato em binario: 12 passos somam 3 s sem erro de arredondamento

        static void Ficar(ChegarDevagar r, float segundos, float distancia, float velocidade)
        {
            for (int i = 0; i < (int)(segundos / Dt + 0.5f); i++) r.Passo(distancia, velocidade, Dt);
        }

        [Test]
        public void ParadoPerto_TresSegundos_Acalma()
        {
            var r = new ChegarDevagar();
            Ficar(r, ChegarDevagar.SegundosParado - Dt, Perto, 0f);
            Assert.IsFalse(r.Calmo, "quase tres respiracoes ainda nao");
            r.Passo(Perto, 0f, Dt);
            Assert.IsTrue(r.Calmo);

            var naBorda = new ChegarDevagar();
            Ficar(naBorda, ChegarDevagar.SegundosParado, ChegarDevagar.RaioDeEspera, 0f);
            Assert.IsTrue(naBorda.Calmo, "a ate 1,5 m conta");
        }

        [Test]
        public void Longe_OuAndando_NaoAcalma()
        {
            var longe = new ChegarDevagar();
            Ficar(longe, 10f, Longe, 0f);
            Assert.IsFalse(longe.Calmo, "parado longe do chapeu");
            Assert.AreEqual(0f, longe.Espera);

            var andando = new ChegarDevagar();
            Ficar(andando, 10f, Perto, Andando);
            Assert.IsFalse(andando.Calmo, "perto, mas andando: ficar parado e o gesto");
            Assert.IsFalse(andando.Assustou, "andar nao assusta");
        }

        [Test]
        public void SairDoRaio_ZeraAEspera()
        {
            var r = new ChegarDevagar();
            Ficar(r, 2.5f, Perto, 0f);
            Assert.AreEqual(2.5f, r.Espera);
            r.Passo(Longe, 0f, Dt);
            Assert.AreEqual(0f, r.Espera, "saiu: comeca de novo");
            Ficar(r, 2.5f, Perto, 0f);
            Assert.IsFalse(r.Calmo, "a espera nao soma em pedacos");
            Ficar(r, 0.5f, Perto, 0f);
            Assert.IsTrue(r.Calmo);
        }

        [Test]
        public void CorrerPerto_Assusta_EZera()
        {
            var r = new ChegarDevagar();
            Ficar(r, 2.5f, Perto, 0f);
            r.Passo(ChegarDevagar.RaioDeSusto - 0.1f, Correndo, Dt);
            Assert.IsTrue(r.Assustou, "correu a menos de 4 m: o chapeu treme");
            Assert.AreEqual(0f, r.Espera);
            r.Passo(ChegarDevagar.RaioDeSusto + 0.1f, Correndo, Dt);
            Assert.IsFalse(r.Assustou, "longe o bastante, correr nao assusta");
        }

        /// <summary>Calmo vale ate a cena sair: o jogador ainda vai procurar Lysa ou Tovin, as vezes longe e correndo.</summary>
        [Test]
        public void Calmo_FicaCalmo_ERegraNovaComecaAssustada()
        {
            var r = new ChegarDevagar();
            Ficar(r, ChegarDevagar.SegundosParado, Perto, 0f);
            Assert.IsTrue(r.Calmo);
            r.Passo(Perto, Correndo, Dt);
            r.Passo(50f, Correndo, Dt);
            Assert.IsTrue(r.Calmo);
            Assert.IsFalse(r.Assustou, "calmo nao treme");

            Assert.IsFalse(new ChegarDevagar().Calmo, "fora do save: reabrir o app pede repetir a espera");
        }

        /// <summary>HIPOTESE do ELENCO.md: o limite fica entre andar e correr do MotionSolver. Se a velocidade da crianca
        /// mudar, este teste avisa antes de o andar comecar a assustar (ou o correr deixar de assustar).</summary>
        [Test]
        public void LimiteDeSusto_EntreAndarECorrer()
        {
            Assert.Greater(ChegarDevagar.VelocidadeDeSusto, Andando);
            Assert.Less(ChegarDevagar.VelocidadeDeSusto, Correndo);
            Assert.Less(ChegarDevagar.VelocidadeParado, Andando * 0.5f, "parado nao pode aceitar passo lento");
        }
    }
}
