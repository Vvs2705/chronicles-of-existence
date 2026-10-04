using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>T011 (GDD v1.2: "Ataque leve/forte, defesa e magia inicial"): o que a auditoria fechou em regra pura.
    /// Fases e recarga da primeira magia (SpellCast) e a ligacao do treino com o dominio da T009 (Mastery).
    /// Leve/forte/bloqueio/esquiva/custo da magia ja estavam cobertos em CombatMovesTests, CombatResourcesTests e
    /// CombatTrainingTests.</summary>
    public class CombatT011Tests
    {
        // ---------- magia inicial: tres fases + recarga ----------

        [Test]
        public void T011_Magia_TresFasesEmOrdem_EfeitoSoNaManifestacao()
        {
            var m = new SpellCast(0.5f, 0.2f, 1f, 3f);
            Assert.IsTrue(m.Pronta);
            Assert.IsTrue(m.Iniciar());
            Assert.AreEqual(SpellFase.Preparacao, m.Fase);
            Assert.IsFalse(m.Tick(0.4f), "preparando: o efeito ainda nao existe");
            Assert.AreEqual(SpellFase.Preparacao, m.Fase);
            Assert.IsTrue(m.Tick(0.2f), "fim da preparacao: manifesta");
            Assert.AreEqual(SpellFase.Manifestacao, m.Fase);
            Assert.IsFalse(m.Tick(0.25f), "manifesta uma vez so");
            Assert.AreEqual(SpellFase.Consequencia, m.Fase);
            Assert.IsFalse(m.Tick(1f));
            Assert.AreEqual(SpellFase.Pronta, m.Fase, "a consequencia acabou");
        }

        [Test]
        public void T011_Magia_RecargaImpedeRelancar_AteZerar()
        {
            var m = new SpellCast(0.5f, 0.2f, 1f, 3f);
            m.Iniciar();
            Assert.IsFalse(m.Iniciar(), "em curso nao relanca");
            m.Tick(1.8f);                                  // as tres fases (1,7 s) acabaram
            Assert.AreEqual(SpellFase.Pronta, m.Fase);
            Assert.IsFalse(m.Pronta, "fases acabaram, recarga nao");
            Assert.IsFalse(m.Iniciar());
            m.Tick(1.3f);                                  // 3,1 s desde o inicio
            Assert.IsTrue(m.Pronta);
            Assert.IsTrue(m.Iniciar());
        }

        [Test]
        public void T011_Magia_FrameLongoNaoPulaManifestacao()
        {
            var m = new SpellCast();
            m.Iniciar();
            Assert.IsTrue(m.Tick(10f), "um frame travado nao some com o efeito");
            Assert.AreEqual(SpellFase.Pronta, m.Fase);
            Assert.IsTrue(m.Pronta, "10 s passam da recarga v0");
        }

        [Test]
        public void T011_Magia_NumerosV0_PreparacaoExiste_ERecargaMaiorQueAsFases()
        {
            float fases = CombatMoves.MagiaPreparacaoV0 + CombatMoves.MagiaManifestacaoV0 + CombatMoves.Magia.Recuperacao;
            Assert.Greater(CombatMoves.MagiaPreparacaoV0, 0f, "sem preparacao a magia vira golpe leve pago em mana");
            Assert.Greater(CombatMoves.MagiaRecargaV0, fases, "recarga menor que as fases seria enfeite");
        }

        // ---------- treino -> dominio (T009) ----------

        [Test]
        public void T011_Atividades_IdsEstaveis_UmaTrilhaPorId()
        {
            var vistos = new HashSet<string>();
            foreach (AtividadeDef a in TrainingProgress.Atividades)
            {
                Assert.IsTrue(Regex.IsMatch(a.Id, "^[a-z0-9]+(_[a-z0-9]+)*$"), "id fora de snake_case: " + a.Id);
                Assert.IsTrue(vistos.Add(a.Id), "activityId repetido (a mesma linha de ledger em duas trilhas): " + a.Id);
                Assert.IsTrue(Mastery.TrilhaExiste(a.TrilhaId), a.Id + ": trilha fora do contrato: " + a.TrilhaId);
            }
            Assert.AreEqual(5, vistos.Count, "leve, forte, bloqueio, esquiva e magia");
            Assert.AreEqual(CombatMoves.AfinidadeArcana, TrainingProgress.AtividadeMagia.TrilhaId, "a fagulha rende arcana");
            Assert.AreEqual(CombatMoves.AfinidadeMarcial, TrainingProgress.AtividadeBloqueio.TrilhaId, "defesa rende marcial");
        }

        [Test]
        public void T011_TreinoRendeDominioPeloMastery_ESaturaPorVerbo()
        {
            {
                var s = new SaveData { ageYears = 8 };
                var g = new GameSession(s, null);

                for (int i = 0; i < 50; i++) TrainingProgress.Registrar(g, TrainingProgress.AtividadeLeve, 1f);
                Assert.AreEqual(Mastery.TetoAtividadePorFase, Mastery.ProgressoTotal(s, CombatMoves.AfinidadeMarcial),
                                "50 golpes leves param no teto da atividade na etapa (B15: o ganho estaciona)");
                Assert.AreEqual(0, Mastery.Valor(s, CombatMoves.AfinidadeMarcial), "um verbo so nao compra um ponto");

                TrainingProgress.Registrar(g, TrainingProgress.AtividadeForte, 1f);
                Assert.AreEqual(Mastery.TetoAtividadePorFase + Mastery.GanhoBase,
                                Mastery.ProgressoTotal(s, CombatMoves.AfinidadeMarcial), "outro verbo ainda ensina");

                TrainingProgress.Registrar(g, TrainingProgress.AtividadeMagia, 1f);
                Assert.AreEqual(Mastery.GanhoBase, Mastery.ProgressoTotal(s, CombatMoves.AfinidadeArcana),
                                "a fagulha rende arcana, nao marcial");
                Assert.AreEqual(8, s.ageYears, "treinar nao envelhece");
            }
        }

        // Bloco E: a espada de madeira no corpo da crianca. A esfera do golpe vai da frente do corpo ate braco + lamina;
        // o forte estica mais; nada perto dos 2,3 m do golpe de adulto de antes; a magia (alcance da fagulha) nao muda.
        [Test]
        public void EspadaDeMadeira_AlcanceDaCriancaDe8_DoCorpoAteBracoMaisLamina()
        {
            Corpo c = Corpo.DaIdade(8);
            MoveSpec leve = CombatMoves.NoCorpo(CombatMoves.Leve, c, CombatMoves.LaminaEspadaDeMadeira);
            MoveSpec forte = CombatMoves.NoCorpo(CombatMoves.Forte, c, CombatMoves.LaminaEspadaDeMadeira);
            Assert.AreEqual(c.Raio, leve.Alcance - leve.Raio, 1e-4f, "o golpe comeca na frente do corpo");
            Assert.AreEqual(c.Braco + CombatMoves.LaminaEspadaDeMadeira, leve.Alcance + leve.Raio, 1e-4f, "e vai ate a ponta da espada");
            Assert.Greater(forte.Alcance + forte.Raio, leve.Alcance + leve.Raio, "o forte estica o braco");
            Assert.Less(forte.Alcance + forte.Raio, 1.5f, "golpe de crianca com espada de madeira, nao de adulto");
            MoveSpec magia = CombatMoves.NoCorpo(CombatMoves.Magia, c, CombatMoves.LaminaEspadaDeMadeira);
            Assert.AreEqual(CombatMoves.Magia.Alcance, magia.Alcance);
            Assert.AreEqual(CombatMoves.Magia.Raio, magia.Raio);
        }

        // Bloco E: a velocidade cresce com a altura (o modelo dos 8 e o dos 5 escalado: a passada cresce junto).
        [Test]
        public void Velocidade_DaIdade_CresceComAAltura()
        {
            Assert.AreEqual(MotionSolver.VelocidadeCaminhadaPadrao, Corpo.DaIdade(5).VelocidadeCaminhada, 1e-4f, "5 anos: a passada medida");
            Assert.AreEqual(MotionSolver.VelocidadeCorridaPadrao, Corpo.DaIdade(5).VelocidadeCorrida, 1e-4f);
            float k = BodyScale.Crianca8 / BodyScale.Crianca5;
            Assert.AreEqual(MotionSolver.VelocidadeCaminhadaPadrao * k, Corpo.DaIdade(8).VelocidadeCaminhada, 1e-4f);
            Assert.AreEqual(MotionSolver.VelocidadeCorridaPadrao * k, Corpo.DaIdade(8).VelocidadeCorrida, 1e-4f);
            Assert.Less(Corpo.DaIdade(8).VelocidadeCorrida, 5f, "crianca de 8 nao corre como adulto (4,8 m/s era o de adulto)");
        }

        [Test]
        public void T011_FarmEmAlvoIndefeso_NaoChegaNoSave()
        {
            var s = new SaveData { ageYears = 8 };
            var g = new GameSession(s, null);
            var ledger = new TrainingLedger();
            for (int i = 0; i < 500; i++)
                TrainingProgress.Registrar(g, TrainingProgress.AtividadeLeve, ledger.RegistrarGolpe(false, i * 0.4f));
            Assert.AreEqual(0, Mastery.ProgressoTotal(s, CombatMoves.AfinidadeMarcial), "alvo indefeso nao rende dominio");
            Assert.AreEqual(0, s.life.pratica.Count, "nem linha de ledger nasce");
        }
    }
}
