using System;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>ADR-0007 §3: concluir a Q-04 grava evento.nilo_desapareceu; enquanto ele valer Nilo esta na
    /// ancora-sentinela (fora de Auren) e a q03, que pede Nilo na trilha, e encerrada na MESMA gravacao se ainda
    /// estiver aberta. Idempotente: reavaliar e recarregar nao mudam nem gravam nada. C# puro.</summary>
    public class AusenciaDeNiloTests
    {
        int gravacoes;
        GameSession Abrir(SaveData s) { return new GameSession(s, () => gravacoes++); }

        [SetUp] public void Zerar() { gravacoes = 0; }

        static SaveData Recarregar(SaveData s)
        {
            SaveData lido = LocalSave.FromJson(LocalSave.ToJson(s));
            Assert.IsNotNull(lido);
            return lido;
        }

        /// <summary>q01 e q02 concluidas e a q04 com tudo feito (desfecho escolhido): falta so concluir.</summary>
        GameSession Q04ProntaParaConcluir()
        {
            GameSession g = Abrir(new SaveData());
            MissaoTeste.Concluir(g.Missoes, MissaoTeste.Q01);
            MissaoTeste.Concluir(g.Missoes, MissaoTeste.Q02);
            MissaoTeste.Cumprir(g.Missoes, MissaoTeste.Q04);
            return g;
        }

        static void NiloEsta(GameSession g, bool ausente, string quando)
        {
            foreach (TimeOfDay p in Enum.GetValues(typeof(TimeOfDay)))
                Assert.AreEqual(ausente, NpcCatalog.Onde("nilo", p, g.Save.npcs).AncoraId == NpcCatalog.AncoraAusente,
                    quando + ": Nilo de " + p);
        }

        [Test]
        public void ConcluirAQ04_GravaOSumico_ENiloSaiDeAurenNosTresPeriodos()
        {
            GameSession g = Q04ProntaParaConcluir();
            Assert.IsFalse(g.Historia.Ja(QuestCatalog.EventoNiloDesapareceu), "antes de concluir ninguem sumiu");
            NiloEsta(g, false, "q04 em andamento");   // o ultimo objetivo da q04 e com ele

            Assert.IsTrue(MissaoMundo.Avancar(g).Ok);

            Assert.AreEqual(QuestStatus.Concluida, g.Missoes.Estado(MissaoTeste.Q04));
            Assert.IsTrue(g.Historia.Ja(QuestCatalog.EventoNiloDesapareceu), "gravado junto da conclusao");
            foreach (string npc in new[] { "nilo", "sera", "maelis" })
                Assert.IsTrue(NpcMemory.Lembra(g.Save.npcs, npc, QuestCatalog.EventoNiloDesapareceu), npc + " sabe, na mesma gravacao");
            NiloEsta(g, true, "q04 concluida");
            Assert.AreEqual(1, gravacoes, "conclusao + evento + memoria = uma gravacao");
        }

        [Test]
        public void Q03Aberta_EncerraNaMesmaGravacao_ENadaSeRepete()
        {
            GameSession g = Q04ProntaParaConcluir();
            Assert.IsTrue(g.Missoes.Iniciar(MissaoTeste.Q03).Ok);
            Assert.IsTrue(g.Missoes.CumprirObjetivo(MissaoTeste.Q03, "procurar_na_horta").Ok);

            Assert.IsTrue(MissaoMundo.Avancar(g).Ok);

            Assert.AreEqual(QuestStatus.Falhada, g.Missoes.Estado(MissaoTeste.Q03), "pedia Nilo na trilha: encerrada");
            Assert.AreEqual(1, gravacoes, "a q03 encerra na gravacao da conclusao da q04, nao numa segunda");
            Assert.AreEqual(0, Inventario.Quantidade(g.Save.inventario, "item.cesto_de_vime"), "encerrar nao paga");
            CollectionAssert.DoesNotContain(g.OpcionaisAbertas(), MissaoTeste.Q03, "o aviso do salto nao a lista de novo");

            int eventos = g.Historia.Todos().Count;
            Assert.IsFalse(MissaoMundo.Avancar(g).Ok, "segunda leitura: nada a fazer");
            Assert.AreEqual(0, g.Sincronizar(), "sincronizar de novo nao muda nada");

            GameSession depois = Abrir(Recarregar(g.Save));
            Assert.IsFalse(MissaoMundo.Avancar(depois).Ok, "recarregar nao reencerra nem reconcede");
            Assert.AreEqual(QuestStatus.Falhada, depois.Missoes.Estado(MissaoTeste.Q03));
            Assert.AreEqual(eventos, depois.Historia.Todos().Count, "nenhum evento duplicado");
            Assert.AreEqual(1, gravacoes);
        }

        [Test]
        public void Q03NuncaIniciada_TambemEncerra_ComoNoSalto()
        {
            GameSession g = Q04ProntaParaConcluir();
            Assert.AreEqual(QuestStatus.Disponivel, g.Missoes.Estado(MissaoTeste.Q03));

            MissaoMundo.Avancar(g);

            Assert.AreEqual(QuestStatus.Falhada, g.Missoes.Estado(MissaoTeste.Q03), "sem Nilo ela nao tem como fechar: nao pode ser oferecida");
            Assert.IsEmpty(MissaoNaConversa.Opcoes("nilo", g.Missoes));
            foreach (OpcaoDeMissao o in MissaoNaConversa.Opcoes("oren", g.Missoes))   // Oren ainda oferece a q07
                Assert.AreNotEqual("missao.q03.titulo", o.TextoKey, "Oren nao oferece mais o cesto");
        }

        [Test]
        public void Q03JaConcluida_FicaComoEsta()
        {
            GameSession g = Q04ProntaParaConcluir();
            MissaoTeste.Concluir(g.Missoes, MissaoTeste.Q03);

            MissaoMundo.Avancar(g);

            Assert.AreEqual(QuestStatus.Concluida, g.Missoes.Estado(MissaoTeste.Q03));
            Assert.AreEqual(1, Inventario.Quantidade(g.Save.inventario, "item.cesto_de_vime"), "o cesto continua com o jogador");
        }

        [Test]
        public void SaveGravadoNoMeio_AbrirEncerraAQ03_SemGravar()
        {
            // O historico ja tem o sumico e a q03 ainda consta aberta: abrir a sessao alcanca, como faz com a memoria.
            SaveData s = new SaveData();
            var m = new QuestSystem(s.quests, new HistoricoDeVidaLedger(s));
            MissaoTeste.Concluir(m, MissaoTeste.Q01);
            Assert.IsTrue(m.Iniciar(MissaoTeste.Q03).Ok);
            new LifeEventHistory(s).Registrar(QuestCatalog.EventoNiloDesapareceu, LifeEventCategoria.Marco, 5);

            GameSession g = Abrir(s);

            Assert.AreEqual(QuestStatus.Falhada, g.Missoes.Estado(MissaoTeste.Q03));
            NiloEsta(g, true, "save aberto");
            Assert.AreEqual(0, gravacoes, "abrir nao grava; vai junto na proxima transicao");
        }

        [Test]
        public void SemOSumico_AQ03SegueAberta()
        {
            GameSession g = Abrir(new SaveData());
            MissaoTeste.Concluir(g.Missoes, MissaoTeste.Q01);
            Assert.IsTrue(g.Missao(m => m.Iniciar(MissaoTeste.Q03)).Ok);

            MissaoMundo.Avancar(g);

            Assert.AreEqual(QuestStatus.EmAndamento, g.Missoes.Estado(MissaoTeste.Q03));
            NiloEsta(g, false, "antes da q04");
        }
    }
}
