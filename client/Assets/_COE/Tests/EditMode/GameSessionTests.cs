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

        // Bloco C (2026-10-04): o nascimento sai da tela e vira transicao da sessao.
        [Test]
        public void Nascer_Ok_GravaNascimentoEInventarioInicialNumaGravacao_EDepoisRecusa()
        {
            SaveData s = new SaveData();
            GameSession g = Abrir(s);
            OriginDef origem = DestinySystem.OrigensDisponiveis("normal")[0];

            BirthResult r = g.Nascer("normal", origem.Id, "Íris");

            Assert.IsTrue(r.Ok, r.Erro.ToString());
            Assert.AreEqual("normal", s.birth.destinyId);
            Assert.AreEqual(origem.Id, s.birth.originId);
            Assert.Greater(s.inventario.recompensasAplicadas.Count, 0, "o inventario inicial entra na mesma transicao");
            Assert.AreEqual(1, gravacoes, "nascimento + inventario = UMA gravacao");

            int moedas = s.inventario.moedas;
            BirthResult deNovo = g.Nascer("ruptura", origem.Id, "Outro");
            Assert.IsFalse(deNovo.Ok, "destino e permanente (ADR-0004)");
            Assert.AreEqual(BirthError.JaConfirmado, deNovo.Erro);
            Assert.AreEqual("normal", s.birth.destinyId);
            Assert.AreEqual(moedas, s.inventario.moedas, "nascer de novo nao paga o inventario de novo");
            Assert.AreEqual(1, gravacoes, "recusa nao grava");
        }

        [Test]
        public void Nascer_Invalido_NaoMudaNemGrava()
        {
            SaveData s = new SaveData();
            BirthResult r = Abrir(s).Nascer("vida_de_rei", "agricultores", "Íris");
            Assert.IsFalse(r.Ok);
            Assert.IsTrue(string.IsNullOrEmpty(s.birth.destinyId));
            Assert.AreEqual(0, s.inventario.recompensasAplicadas.Count);
            Assert.AreEqual(0, gravacoes);
        }

        // Bloco C: o treino escreve pela sessao e grava quando rende ou quando fecha o treino; saturado, nao regrava.
        [Test]
        public void Praticar_GravaQuandoRende_NaoQuandoSatura_EGravaAoFecharOTreino()
        {
            SaveData s = new SaveData { ageYears = 8 };
            GameSession g = Abrir(s);

            Assert.IsTrue(g.Praticar(TrainingProgress.AtividadeLeve).ProgressoGanho > 0);
            Assert.AreEqual(1, gravacoes, "pratica que rendeu grava");
            for (int i = 0; i < 60; i++) g.Praticar(TrainingProgress.AtividadeLeve);
            int saturado = gravacoes;
            GanhoResultado r = g.Praticar(TrainingProgress.AtividadeLeve);
            Assert.IsTrue(r.Aceito && r.Saturada && r.ProgressoGanho == 0, "o golpe repetido ja bateu o teto da etapa");
            Assert.AreEqual(saturado, gravacoes, "pratica saturada nao reescreve o save");

            g.Praticar(TrainingProgress.AtividadeForte);
            g.Praticar(TrainingProgress.AtividadeEsquiva);
            Assert.IsFalse(g.GanchoPendente());
            int antes = gravacoes;
            g.Praticar(TrainingProgress.AtividadeMagia);
            Assert.IsTrue(g.GanchoPendente(), "os quatro verbos fecham o treino");
            Assert.AreEqual(antes + 1, gravacoes, "a pratica que fecha o treino esta gravada");
            Assert.AreEqual(8, s.ageYears, "treinar nao envelhece");
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

        /// <summary>Prompt Mestre §28 ("opcionais encerradas"): o salto encerra so a opcional ABERTA; a concluida antes fica
        /// concluida e nem aparece no aviso (dois filtros: OpcionaisAbertas e o guard do QuestSystem.Encerrar).</summary>
        [Test]
        public void Salto_NaoEncerraOpcionalJaConcluida()
        {
            SaveData s = new SaveData();
            new LifeEventHistory(s).Registrar(AgeAdvanceCatalog.LiberadoPor, LifeEventCategoria.Marco, 5);
            GameSession g = Abrir(s);
            // As opcionais pedem pre-requisito: a campanha anda pelas centrais ate a primeira opcional abrir.
            QuestDef opcional = null;
            for (int volta = 0; volta < QuestCatalog.Missoes.Length; volta++)
            {
                opcional = System.Array.Find(QuestCatalog.Missoes, d => !d.Central && g.Missoes.Estado(d.Id) == QuestStatus.Disponivel);
                if (opcional != null) break;
                QuestDef central = System.Array.Find(QuestCatalog.Missoes, d => d.Central
                    && (g.Missoes.Estado(d.Id) == QuestStatus.Disponivel || g.Missoes.Estado(d.Id) == QuestStatus.EmAndamento));
                Assert.IsNotNull(central, "a campanha parou antes de abrir uma opcional");
                MissaoTeste.Concluir(g.Missoes, central.Id);
            }
            Assert.IsNotNull(opcional, "nenhuma opcional abriu");
            string id = opcional.Id;
            MissaoTeste.Concluir(g.Missoes, id);
            Assert.AreEqual(QuestStatus.Concluida, g.Missoes.Estado(id), id + " concluida antes do salto");

            SaltoPreparado p = g.PrepararSalto();
            CollectionAssert.DoesNotContain(p.OportunidadesEncerradas, id, "concluida nao entra no aviso do salto");
            Assert.IsTrue(g.ConfirmarSalto(p).Aplicado);
            Assert.AreEqual(QuestStatus.Concluida, g.Missoes.Estado(id), "e continua concluida depois dele");
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
