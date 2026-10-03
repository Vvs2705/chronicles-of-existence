using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>T014 / SLICE §5 R6: o slice inteiro, do nascimento ao gancho (B16), concluindo ZERO missoes opcionais, pela
    /// MESMA fachada que o jogo usa (GameSession), em cada destino e em cada desfecho da promessa. Depois, salvar e
    /// carregar: o que tem de ser lembrado esta no historico (§4.3), o que nao aconteceu nao esta, e nada se repete
    /// (salto, gancho). Os passos de tela de R1-R18 ficam em docs/qa/T014_REGRESSAO.md.</summary>
    public class SliceInteiroTests
    {
        [Test]
        public void R6_DoNascimentoAoGancho_SemNenhumaOpcional_EmCadaDestinoEDesfecho()
        {
            QuestDef q04 = QuestCatalog.Missao("q04_uma_promessa");
            foreach (DestinyDef destino in DestinyCatalog.Destinos)
                for (int desfecho = 0; desfecho < q04.Desfechos.Length; desfecho++)
                    Jogar(destino.Id, desfecho, destino.Id + "/" + q04.Desfechos[desfecho]);
        }

        /// <summary>R9 (ADR-0004 reescrito pelo ADR-0007 §7): save editado a mao com destino inexistente. O jogo nao promete
        /// anti-cheat local; exige-se que DETECTE e REGISTRE (aviso no log) e que carregue sem quebrar em silencio.</summary>
        [Test]
        public void R9_SaveEditadoComDestinoInexistente_CarregaERegistraAInconsistencia()
        {
            string dir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "coe_t014_" + System.Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(dir);
            try
            {
                var s = new SaveData();
                OriginDef origem = DestinySystem.OrigensDisponiveis(DestinyCatalog.Destinos[0].Id)[0];
                s.birth = DestinySystem.Confirmar(s.birth, DestinyCatalog.Destinos[0].Id, origem.Id, "Íris").Escolha;
                string path = System.IO.Path.Combine(dir, LocalSave.FileName);
                System.IO.File.WriteAllText(path, LocalSave.ToJson(s).Replace("\"" + s.birth.destinyId + "\"", "\"vida_de_rei\""));

                UnityEngine.TestTools.LogAssert.Expect(UnityEngine.LogType.Warning,
                    new System.Text.RegularExpressions.Regex("nascimento invalido.*vida_de_rei"));
                SaveData lido = LocalSave.Load(path);
                Assert.IsNotNull(lido, "save editado nao pode derrubar o carregamento");
                Assert.AreEqual("vida_de_rei", lido.birth.destinyId, "detecta e registra; nao reescreve o arquivo do jogador");
            }
            finally { try { System.IO.Directory.Delete(dir, true); } catch (System.Exception) { } }
        }

        static void Jogar(string destinoId, int desfecho, string caso)
        {
            // B02-B05: nascer (o que a tela de nascimento faz, sem a tela)
            var s = new SaveData();
            OriginDef origem = DestinySystem.OrigensDisponiveis(destinoId)[0];
            BirthResult nascimento = DestinySystem.Confirmar(s.birth, destinoId, origem.Id, "Íris");
            Assert.IsTrue(nascimento.Ok, caso + ": nascimento");
            s.birth = nascimento.Escolha;
            Inventario.Nascer(s.inventario, DestinySystem.CircunstanciaDe(nascimento.Escolha));

            int gravacoes = 0;
            var g = new GameSession(s, () => gravacoes++);

            // B06-B11: so as centrais, na ordem em que ficam disponiveis
            var centrais = QuestCatalog.Missoes.Where(d => d.Central).Select(d => d.Id).ToList();
            for (int volta = 0; volta < centrais.Count && centrais.Any(id => g.Missoes.Estado(id) != QuestStatus.Concluida); volta++)
                foreach (string id in centrais)
                    if (g.Missoes.Estado(id) == QuestStatus.Disponivel) Concluir(g, id, desfecho, caso);
            foreach (string id in centrais)
                Assert.AreEqual(QuestStatus.Concluida, g.Missoes.Estado(id), caso + ": central travada " + id);
            foreach (QuestDef d in QuestCatalog.Missoes.Where(d => !d.Central))
                Assert.AreNotEqual(QuestStatus.Concluida, g.Missoes.Estado(d.Id), caso + ": opcional tocada " + d.Id);

            // B12-B13: o aviso anuncia exatamente as opcionais ainda abertas; o salto se aplica uma vez
            string[] abertas = g.OpcionaisAbertas();
            SaltoPreparado aviso = g.PrepararSalto();
            Assert.IsTrue(aviso.Possivel, caso + ": salto nao liberou");
            CollectionAssert.AreEquivalent(abertas, aviso.OportunidadesEncerradas, caso + ": o aviso nao nomeou as opcionais abertas");
            Assert.IsTrue(g.ConfirmarSalto(aviso).Aplicado, caso + ": salto");
            Assert.AreEqual(8, s.ageYears);
            Assert.IsFalse(g.ConfirmarSalto(aviso).Aplicado, caso + ": salto aplicado duas vezes");
            Assert.IsEmpty(g.OpcionaisAbertas(), caso + ": opcional sobreviveu ao salto");

            // B15-B16: os quatro verbos do treino (o que o TrainingProgress.Sink faz) e o gancho, uma vez
            Assert.IsFalse(g.GanchoPendente(), caso + ": gancho antes do treino");
            foreach (AtividadeDef a in new[] { TrainingProgress.AtividadeLeve, TrainingProgress.AtividadeForte,
                                               TrainingProgress.AtividadeEsquiva, TrainingProgress.AtividadeMagia })
                Mastery.Praticar(s, a);
            Assert.IsTrue(g.GanchoPendente(), caso + ": o slice nao chegou ao fim");
            Assert.IsTrue(g.VerGancho());
            Assert.Greater(gravacoes, 0);

            // Salvar e carregar: §4.3 lembrado, regra negativa, nada se repete
            SaveData lido = LocalSave.FromJson(LocalSave.ToJson(s));
            var depois = new GameSession(lido, null);
            Assert.AreEqual(nascimento.Escolha.destinyId, lido.birth.destinyId, caso + ": destino mudou no round-trip");
            Assert.AreEqual("Íris", lido.birth.characterName, caso + ": nome com acento");
            Assert.AreEqual(8, lido.ageYears);
            Assert.IsFalse(AgeAdvance.PodeAvancarIdade(lido, AgeAdvanceCatalog.SaltoInfancia, depois.Historia), caso + ": salto oferecido de novo");
            Assert.IsFalse(depois.GanchoPendente(), caso + ": gancho de novo depois de carregar");
            foreach (string lembrado in new[] { "marco.primeiro_dia", "evento.q07_concluida", "marco.eco_do_limiar", GameSession.MarcoGancho })
                Assert.IsTrue(depois.Historia.Ja(lembrado), caso + ": esqueceu " + lembrado);
            Assert.IsTrue(depois.Historia.Ja("evento.q04_promessa_cumprida") ^ depois.Historia.Ja("evento.q04_promessa_quebrada"),
                caso + ": a promessa tem exatamente um desfecho no historico");
            foreach (string nunca in new[] { "evento.q03_concluida", "evento.q05_concluida", "evento.q06_concluida" })
                Assert.IsFalse(depois.Historia.Ja(nunca), caso + ": lembra opcional nunca feita " + nunca);
        }

        static void Concluir(GameSession g, string id, int desfecho, string caso)
        {
            QuestDef d = QuestCatalog.Missao(id);
            Assert.IsTrue(g.Missao(m => m.Iniciar(id)).Ok || g.Missoes.Estado(id) == QuestStatus.EmAndamento, caso + ": iniciar " + id);
            foreach (ObjetivoDef o in d.Objetivos)
                if (!g.Missoes.ObjetivosFeitos(id).Contains(o.Id))
                    Assert.IsTrue(g.Missao(m => m.CumprirObjetivo(id, o.Id)).Ok, caso + ": " + id + "/" + o.Id);
            if (d.Desfechos.Length > 0)
                Assert.IsTrue(g.Missao(m => m.EscolherDesfecho(id, d.Desfechos[desfecho])).Ok, caso + ": desfecho " + id);
            QuestResultado r = g.Missao(m => m.Concluir(id));
            Assert.IsTrue(r.Ok || g.Missoes.Estado(id) == QuestStatus.Concluida, caso + ": concluir " + id + " " + r.Erro);
        }
    }
}
