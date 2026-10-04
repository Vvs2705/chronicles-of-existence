using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Diario de sessao (playtest): comparacao do save entre gravacoes, formato mm:ss, rotacao dos 10 arquivos e
    /// privacidade. Tudo numa pasta temporaria; nada em persistentDataPath.</summary>
    public class DiarioDeSessaoTests
    {
        const string Nome = "Zebulom Quaresma";
        string dir;

        [SetUp]
        public void SetUp()
        {
            dir = Path.Combine(Path.GetTempPath(), "coe_diario_tests_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
        }

        [TearDown]
        public void TearDown()
        {
            try { Directory.Delete(dir, true); } catch (Exception) { }
        }

        static SaveData SaveComNome()
        {
            var s = new SaveData();
            s.characterId = "abc123def456";
            s.birth.destinyId = "ruptura";
            s.birth.originId = "guardioes";
            s.birth.characterName = Nome;
            return s;
        }

        static void Evento(SaveData s, string id)
        {
            new LifeEventHistory(s).Registrar(id, LifeEventCategoria.Marco, s.ageYears);
        }

        static QuestState Missao(SaveData s, string id, QuestStatus status)
        {
            var q = new QuestState { questId = id, status = (int)status };
            s.quests.missoes.Add(q);
            return q;
        }

        static List<string> Base(SaveData s, out DiarioDeSessao.Retrato r)
        {
            r = new DiarioDeSessao.Retrato();
            return DiarioDeSessao.Mudancas(r, s);
        }

        [Test]
        public void Mudancas_NadaMudou_NenhumaLinha()
        {
            SaveData s = SaveComNome();
            Missao(s, "q01", QuestStatus.EmAndamento).objetivosFeitos.Add("falar");
            Evento(s, "marco.primeiro_dia");
            DiarioDeSessao.Retrato r;
            Assert.IsNotEmpty(Base(s, out r), "retrato vazio ve tudo");
            CollectionAssert.IsEmpty(DiarioDeSessao.Mudancas(r, s), "gravar de novo sem mudar nada nao escreve");
        }

        [Test]
        public void Mudancas_ObjetivoNovo()
        {
            SaveData s = SaveComNome();
            QuestState q = Missao(s, "q01", QuestStatus.EmAndamento);
            DiarioDeSessao.Retrato r;
            Base(s, out r);
            q.objetivosFeitos.Add("falar_com_borin");
            CollectionAssert.AreEqual(new[] { "objetivo\tq01/falar_com_borin" }, DiarioDeSessao.Mudancas(r, s));
        }

        [Test]
        public void Mudancas_MissaoQueConcluiu_EMissaoNova()
        {
            SaveData s = SaveComNome();
            QuestState q = Missao(s, "q01", QuestStatus.EmAndamento);
            DiarioDeSessao.Retrato r;
            Base(s, out r);
            q.status = (int)QuestStatus.Concluida;
            Missao(s, "q02", QuestStatus.EmAndamento);
            CollectionAssert.AreEqual(new[] { "missao\tq01 concluida", "missao\tq02 em_andamento" }, DiarioDeSessao.Mudancas(r, s));
        }

        [Test]
        public void Mudancas_EventoNovo_SoONovo()
        {
            SaveData s = SaveComNome();
            Evento(s, "evento.q03_concluida");
            DiarioDeSessao.Retrato r;
            Base(s, out r);
            Evento(s, "evento.q04_promessa_quebrada");
            CollectionAssert.AreEqual(new[] { "evento\tevento.q04_promessa_quebrada" }, DiarioDeSessao.Mudancas(r, s));
        }

        [Test]
        public void Mudancas_PeriodoEIdade()
        {
            SaveData s = SaveComNome();
            DiarioDeSessao.Retrato r;
            CollectionAssert.AreEqual(new[] { "periodo\tmanha", "idade\t5" }, Base(s, out r));
            TimeOfDayCycle.Avancar(s.life);
            CollectionAssert.AreEqual(new[] { "periodo\ttarde" }, DiarioDeSessao.Mudancas(r, s));
            s.ageYears = 8;
            CollectionAssert.AreEqual(new[] { "idade\t8" }, DiarioDeSessao.Mudancas(r, s));
        }

        [Test]
        public void Mudancas_BlocoNull_NaoLanca()
        {
            var s = new SaveData { quests = null, lifeHistory = null, life = null };
            DiarioDeSessao.Retrato r;
            CollectionAssert.AreEqual(new[] { "periodo\tmanha", "idade\t5" }, Base(s, out r));
        }

        [Test]
        public void Tempo_MinutosPassamDe59()
        {
            Assert.AreEqual("00:00", DiarioDeSessao.Tempo(0));
            Assert.AreEqual("00:59", DiarioDeSessao.Tempo(59.9));
            Assert.AreEqual("01:01", DiarioDeSessao.Tempo(61));
            Assert.AreEqual("75:03", DiarioDeSessao.Tempo(75 * 60 + 3));
            Assert.AreEqual("00:00", DiarioDeSessao.Tempo(-5), "relogio para tras nao vira tempo negativo");
        }

        /// <summary>Uma sessao inteira no disco: formato das linhas, Nova vida no meio, pausa em par e o nome do
        /// personagem (nem o characterId) em lugar nenhum.</summary>
        [Test]
        public void Arquivo_FormatoESemNomeDoPersonagem()
        {
            SaveData s = SaveComNome();
            DateTime t = new DateTime(2026, 10, 3, 14, 0, 0, DateTimeKind.Local);
            var d = new DiarioDeSessao(Path.Combine(dir, "diario"), () => t, s, "1.0", "Bootstrap");
            Assert.AreEqual(Path.Combine(dir, "diario", "sessao_20261003_140000.txt"), d.Arquivo);

            t = t.AddSeconds(95);
            Missao(s, "q01", QuestStatus.EmAndamento).objetivosFeitos.Add("falar");
            d.Observar(s);
            d.Observar(s);   // pausa grava de novo sem mudar nada
            t = t.AddMinutes(10);
            d.SegundoPlano(true);
            d.SegundoPlano(true);   // foco e pausa chegam em par
            t = t.AddMinutes(1);
            d.SegundoPlano(false);
            SaveData novaVida = new SaveData();
            d.Observar(novaVida);
            t = t.AddMinutes(64);
            d.Fim();

            string[] linhas = File.ReadAllLines(d.Arquivo);
            CollectionAssert.AreEqual(new[]
            {
                "00:00\tinicio\tversao=1.0 cena=Bootstrap save=continuado idade=5 periodo=manha",
                "01:35\tobjetivo\tq01/falar",
                "01:35\tmissao\tq01 em_andamento",
                "11:35\tpausa\t",
                "12:35\tvolta\t",
                "12:35\tperiodo\tmanha",
                "12:35\tidade\t5",
                "76:35\tfim\t76:35",
            }, linhas);

            string tudo = File.ReadAllText(d.Arquivo);
            StringAssert.DoesNotContain("Zebulom", tudo);
            StringAssert.DoesNotContain(s.characterId, tudo);
        }

        [Test]
        public void SaveSemNascimento_ENovo()
        {
            var d = new DiarioDeSessao(dir, () => new DateTime(2026, 10, 3, 9, 0, 0, DateTimeKind.Local), new SaveData(), "1.0", "Bootstrap");
            StringAssert.Contains("save=novo", File.ReadAllText(d.Arquivo));
        }

        /// <summary>Abrir a 13a sessao deixa as 10 mais novas (a nova inclusa) e nao toca nada que nao seja sessao_*.txt,
        /// nem fora de diario/.</summary>
        [Test]
        public void Rotacao_Mantem10_SoSessoesEmDiario()
        {
            string pasta = Path.Combine(dir, "diario");
            Directory.CreateDirectory(pasta);
            for (int i = 0; i < 12; i++) File.WriteAllText(Path.Combine(pasta, DiarioDeSessao.NomeDoArquivo(new DateTime(2026, 1, 1, 10, i, 0))), "x");
            File.WriteAllText(Path.Combine(pasta, "notas.txt"), "x");
            File.WriteAllText(Path.Combine(dir, "sessao_20200101_000000.txt"), "x");   // fora de diario/
            File.WriteAllText(Path.Combine(dir, "save.json"), "{}");

            var d = new DiarioDeSessao(pasta, () => new DateTime(2026, 10, 3, 14, 0, 0, DateTimeKind.Local), SaveComNome(), "1.0", "Bootstrap");

            string[] sessoes = Directory.GetFiles(pasta, "sessao_*.txt");
            Assert.AreEqual(DiarioDeSessao.ArquivosGuardados, sessoes.Length);
            Assert.IsTrue(File.Exists(d.Arquivo), "a sessao nova fica");
            for (int i = 0; i < 3; i++)   // 12 velhas + a nova = 13: saem as 3 mais velhas
                Assert.IsFalse(File.Exists(Path.Combine(pasta, DiarioDeSessao.NomeDoArquivo(new DateTime(2026, 1, 1, 10, i, 0)))), "velha " + i);
            Assert.IsTrue(File.Exists(Path.Combine(pasta, DiarioDeSessao.NomeDoArquivo(new DateTime(2026, 1, 1, 10, 3, 0)))));
            Assert.IsTrue(File.Exists(Path.Combine(pasta, "notas.txt")), "outro arquivo da pasta fica");
            Assert.IsTrue(File.Exists(Path.Combine(dir, "sessao_20200101_000000.txt")), "fora de diario/ nao e tocado");
            Assert.IsTrue(File.Exists(Path.Combine(dir, "save.json")));
        }

        /// <summary>persistentDataPath que nao abre (aqui: um ARQUIVO no lugar da pasta) nao derruba a partida.</summary>
        [Test]
        public void PastaQueNaoAbre_NaoLanca()
        {
            string bloqueio = Path.Combine(dir, "diario");
            File.WriteAllText(bloqueio, "x");
            var d = new DiarioDeSessao(bloqueio, () => DateTime.UtcNow, SaveComNome(), "1.0", "Bootstrap");
            Assert.IsNull(d.Arquivo);
            d.Observar(SaveComNome());
            d.SegundoPlano(true);
            d.Fim();
        }

        /// <summary>Fora do Play (teste de editor) o diario nao abre nem escreve em persistentDataPath, pela regra do Commit.</summary>
        [Test]
        public void ForaDoPlay_NaoAbreDiario()
        {
            DiarioDeSessao antes = SaveState.Diario;
            SaveState.AbrirDiario("Teste");
            Assert.AreSame(antes, SaveState.Diario);
        }
    }
}
