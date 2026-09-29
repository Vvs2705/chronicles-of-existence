using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Anti-farm do treino (exploit nº 7 do backlog) e determinismo do oponente.
    /// O teste negativo obrigatorio esta em Farm_EmAlvoIndefeso_NaoGeraPraticaNenhuma.</summary>
    public class CombatTrainingTests
    {
        // ---------- anti-farm ----------

        [Test]
        public void Farm_EmAlvoIndefeso_NaoGeraPraticaNenhuma()
        {
            var l = new TrainingLedger();
            float total = 0f;
            for (int i = 0; i < 500; i++) total += l.RegistrarGolpe(false, i * 0.4f); // 200 s batendo num alvo parado
            Assert.AreEqual(0f, total, 1e-4f, "bater em alvo indefeso nao pode render NADA, nem pouco");
            Assert.AreEqual(0, l.Creditadas);
            Assert.AreEqual(500, l.GolpesIgnorados);
        }

        [Test]
        public void GolpeEmAlvoDefendido_Rende_MasTemRecarga()
        {
            var l = new TrainingLedger(1.5f);
            Assert.AreEqual(1f, l.RegistrarGolpe(true, 10f), 1e-4f);
            Assert.AreEqual(0f, l.RegistrarGolpe(true, 10.5f), 1e-4f, "spam dentro da recarga nao rende de novo");
            Assert.AreEqual(1f, l.RegistrarGolpe(true, 11.6f), 1e-4f, "passada a recarga, volta a render");
            Assert.AreEqual(2, l.Creditadas);
        }

        [Test]
        public void Turtle_GuardaErguidaDesdeSempre_NaoRende()
        {
            var l = new TrainingLedger();
            Assert.AreEqual(0f, l.RegistrarBloqueio(30f, 5f), 1e-4f, "segurar bloqueio nao e renda passiva");
            Assert.AreEqual(0, l.Creditadas);
        }

        [Test]
        public void Bloqueio_ValePelaReacao()
        {
            var l = new TrainingLedger();
            Assert.AreEqual(1f, l.RegistrarBloqueio(0.3f, 0f), 1e-4f, "levantou a guarda vendo o telegrafico");
            var l2 = new TrainingLedger();
            Assert.AreEqual(0.6f, l2.RegistrarBloqueio(1f, 0f), 1e-4f, "tardio vale menos");
        }

        [Test]
        public void Esquiva_SoContaQuandoEngoleUmGolpe()
        {
            var l = new TrainingLedger();
            // Rolar sozinho nunca chama RegistrarEsquiva (quem chama e Health.Evaded): nada a creditar.
            Assert.AreEqual(0, l.Creditadas);
            Assert.AreEqual(1f, l.RegistrarEsquiva(4f), 1e-4f);
            Assert.AreEqual(0f, l.RegistrarEsquiva(4.2f), 1e-4f, "recarga tambem vale para esquiva");
        }

        [Test]
        public void TiposDiferentes_NaoCompartilhamRecarga()
        {
            var l = new TrainingLedger();
            Assert.AreEqual(1f, l.RegistrarGolpe(true, 0f), 1e-4f);
            Assert.AreEqual(1f, l.RegistrarEsquiva(0.1f), 1e-4f, "esquivar depois de acertar nao pode ser bloqueado pela recarga do golpe");
        }

        // ---------- oponente deterministico ----------

        [Test]
        public void Boneco_TelegrafaAntesDeBater()
        {
            var b = new TrainingDummyBrain();
            Assert.AreEqual(DummyFase.Ocioso, b.Fase);
            Assert.IsFalse(b.Guardando, "parado e alvo indefeso");
            Assert.IsFalse(b.Tick(TrainingDummyBrain.OciosoPadrao + 0.01f), "sai do ocioso sem bater");
            Assert.AreEqual(DummyFase.Telegrafico, b.Fase);
            Assert.IsTrue(b.Guardando, "telegrafando, a guarda esta ativa");
            Assert.IsTrue(b.Tick(TrainingDummyBrain.TelegraficoPadrao + 0.01f), "o golpe so sai depois do telegrafico");
        }

        [Test]
        public void Boneco_EDeterministicoEAlternaOsGolpes()
        {
            var a = new TrainingDummyBrain();
            var b = new TrainingDummyBrain();
            int golpesA = 0, golpesB = 0;
            for (int i = 0; i < 400; i++)
            {
                if (a.Tick(0.05f)) golpesA++;
                if (b.Tick(0.05f)) golpesB++;
            }
            Assert.AreEqual(golpesA, golpesB, "mesmo dt, mesma sequencia: nada de sorteio");
            Assert.AreEqual(a.Ciclo, b.Ciclo);
            Assert.Greater(golpesA, 0);

            var c = new TrainingDummyBrain();
            c.Tick(TrainingDummyBrain.OciosoPadrao + TrainingDummyBrain.TelegraficoPadrao + 0.01f);
            int primeira = c.Variante;
            c.Tick(TrainingDummyBrain.GolpePadrao + TrainingDummyBrain.RecuperacaoPadrao +
                   TrainingDummyBrain.OciosoPadrao + TrainingDummyBrain.TelegraficoPadrao + 0.02f);
            Assert.AreNotEqual(primeira, c.Variante, "os dois golpes se alternam, e o aluno aprende o padrao");
        }

        [Test]
        public void Boneco_FrameLongoNaoPulaGolpe()
        {
            var b = new TrainingDummyBrain();
            int golpes = 0;
            for (int i = 0; i < 10; i++) if (b.Tick(4f)) golpes++; // dt maior que um ciclo inteiro (3,6 s)
            Assert.AreEqual(10, golpes, "um frame travado nao pode fazer o golpe desaparecer");
            Assert.AreEqual(11, b.Ciclo, "40 s / 3,6 s = 11 ciclos completos, mesmo em passos de 4 s");
        }

        // ---------- idade manda no acesso ----------

        [Test]
        public void Treino_SoDepoisDoSaltoParaOitoAnos()
        {
            System.Func<int> antes = TrainingProgress.IdadeAnos;
            try
            {
                int idade = 5;
                TrainingProgress.IdadeAnos = delegate { return idade; };
                Assert.IsFalse(TrainingProgress.PodeTreinar(), "aos 5 anos nao ha treino de combate (dossie §F)");
                idade = 8;
                Assert.IsTrue(TrainingProgress.PodeTreinar(), "depois do salto para ~8, o treino supervisionado existe");
            }
            finally { TrainingProgress.IdadeAnos = antes; }
        }

        [Test]
        public void Registrar_ComQualidadeZero_NaoChegaNaT009()
        {
            System.Action<AtividadeDef, float> antes = TrainingProgress.Sink;
            try
            {
                int chamadas = 0;
                TrainingProgress.Sink = delegate { chamadas++; };
                TrainingProgress.Registrar(TrainingProgress.AtividadeLeve, 0f);
                Assert.AreEqual(0, chamadas, "qualidade 0 nao vira pratica");
                TrainingProgress.Registrar(TrainingProgress.AtividadeLeve, 1f);
                Assert.AreEqual(1, chamadas);
            }
            finally { TrainingProgress.Sink = antes; }
        }
    }
}
