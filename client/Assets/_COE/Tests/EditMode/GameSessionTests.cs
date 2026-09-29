using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>T012: a sessao aplica as duas regras de fiacao do SaveState. Abrir sincroniza; transicao que deu
    /// certo grava UMA vez; recusada nao grava; trocar o save reabre; o salto encerra as opcionais e grava uma vez.</summary>
    public class GameSessionTests
    {
        int gravacoes;
        GameSession Abrir(SaveData s) { return new GameSession(s, () => gravacoes++); }

        [SetUp] public void Zerar() { gravacoes = 0; }

        [Test]
        public void Abrir_SincronizaMemoriaEReputacaoComOHistorico_SemGravar()
        {
            SaveData s = new SaveData();
            new LifeEventHistory(s).Registrar("evento.q04_promessa_cumprida", LifeEventCategoria.Marco, 5);

            GameSession g = Abrir(s);

            Assert.IsTrue(NpcMemory.Lembra(s.npcs, "sera", "evento.q04_promessa_cumprida"), "Sera testemunhou a promessa");
            Assert.Greater(g.Reputacao.ConfiancaNo("sera"), 0, "a promessa cumprida ja pesa ao abrir o save");
            Assert.AreEqual(0, gravacoes, "abrir nao grava; so transicao grava");
        }

        [Test]
        public void TransicaoOk_GravaUmaVez_ERecusada_NaoGrava()
        {
            GameSession g = Abrir(new SaveData());
            string q01 = QuestCatalog.Missoes[0].Id;

            Assert.IsTrue(g.Missao(m => m.Iniciar(q01)).Ok);
            Assert.AreEqual(1, gravacoes);

            Assert.IsFalse(g.Missao(m => m.Iniciar(q01)).Ok, "iniciar de novo e recusado");
            Assert.IsFalse(g.Missao(m => m.Concluir("missao_que_nao_existe")).Ok);
            Assert.AreEqual(1, gravacoes, "recusa nao grava nada");
        }

        [Test]
        public void SaveStateSessao_ReabreQuandoOSaveTroca()
        {
            SaveData antes = SaveState.Current;
            try
            {
                SaveState.Current = new SaveData();
                GameSession a = SaveState.Sessao;
                Assert.AreSame(a, SaveState.Sessao, "mesmo save = mesma sessao");

                SaveState.Current = new SaveData();   // o que o Load faz
                Assert.AreNotSame(a, SaveState.Sessao);
                Assert.AreSame(SaveState.Current, SaveState.Sessao.Save, "a sessao nova e sobre o save novo");
            }
            finally { SaveState.Current = antes; }
        }

        [Test]
        public void Salto_SemAQ08_NaoAconteceNemGrava()
        {
            SaveData s = new SaveData();
            GameSession g = Abrir(s);

            SaltoResultado r = g.ConfirmarSalto(g.PrepararSalto());

            Assert.IsFalse(r.Aplicado);
            Assert.AreEqual(5, s.ageYears);
            Assert.AreEqual(0, gravacoes);
        }

        [Test]
        public void Salto_EncerraAsOpcionaisAbertas_EGravaUmaVez()
        {
            SaveData s = new SaveData();
            new LifeEventHistory(s).Registrar(AgeAdvanceCatalog.LiberadoPor, LifeEventCategoria.Marco, 5);
            GameSession g = Abrir(s);
            string[] abertas = g.OpcionaisAbertas();
            Assert.IsNotEmpty(abertas, "o slice tem opcionais");

            SaltoPreparado p = g.PrepararSalto();
            CollectionAssert.AreEquivalent(abertas, p.OportunidadesEncerradas, "o aviso lista o que vai se encerrar");
            SaltoResultado r = g.ConfirmarSalto(p);

            Assert.IsTrue(r.Aplicado);
            Assert.AreEqual(8, s.ageYears);
            foreach (string q in abertas) Assert.AreEqual(QuestStatus.Falhada, g.Missoes.Estado(q), q + " encerrou no salto");
            Assert.IsEmpty(g.OpcionaisAbertas());
            Assert.AreEqual(1, gravacoes, "um salto = uma gravacao");

            Assert.IsFalse(g.ConfirmarSalto(p).Aplicado, "confirmar de novo nao salta");
            Assert.AreEqual(1, gravacoes);
        }

        [Test]
        public void Encerrar_NaoMexeEmCentralNemEmConcluida()
        {
            GameSession g = Abrir(new SaveData());
            QuestDef central = System.Array.Find(QuestCatalog.Missoes, d => d.Central);

            Assert.AreEqual(QuestErro.CentralNaoFalha, g.Missoes.Encerrar(central.Id).Erro);
            Assert.AreEqual(QuestErro.MissaoDesconhecida, g.Missoes.Encerrar("nao_existe").Erro);
        }
    }
}
