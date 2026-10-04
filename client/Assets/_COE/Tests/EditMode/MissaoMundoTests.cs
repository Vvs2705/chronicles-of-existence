using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>T012 (raia de missoes no mundo): o que comeca sozinho, qual gatilho de ancora vale, o que o HUD mostra e
    /// a conclusao automatica — tudo pela GameSession, contando gravacoes (uma por transicao que mudou algo, zero na
    /// recusa). C# puro: nada de cena.</summary>
    public class MissaoMundoTests
    {
        int gravacoes;
        GameSession Abrir(SaveData s) { return new GameSession(s, () => gravacoes++); }

        [SetUp] public void Zerar() { gravacoes = 0; }

        [Test]
        public void Tabelas_ApontamParaMissaoEObjetivoDoCatalogo_EAutomaticasSaoCentrais()
        {
            foreach (string id in MissaoMundo.Automaticas)
            {
                QuestDef d = QuestCatalog.Missao(id);
                Assert.IsNotNull(d, id);
                Assert.IsTrue(d.Central, id + ": so central comeca sozinha");
            }
            foreach (string[] g in MissaoMundo.Gatilhos)
            {
                Assert.AreEqual(3, g.Length);
                QuestDef d = QuestCatalog.Missao(g[0]);
                Assert.IsTrue(d != null && d.Objetivo(g[1]) != null, g[0] + "." + g[1] + " nao existe no catalogo");
            }
            foreach (string[] a in MissaoMundo.ObjetivosAutomaticos)
            {
                Assert.AreEqual(2, a.Length);
                QuestDef d = QuestCatalog.Missao(a[0]);
                Assert.IsTrue(d != null && d.Objetivo(a[1]) != null, a[0] + "." + a[1] + " nao existe no catalogo");
                Assert.AreEqual("", MissaoMundo.AncoraDo(a[0], a[1]), a[0] + "." + a[1] + ": automatico nao tem gatilho de ancora");
            }
        }

        [Test]
        public void Q01_AcordarSeCumpreSozinho_NaGravacaoDoInicio()
        {
            GameSession g = Abrir(new SaveData());

            Assert.IsTrue(MissaoMundo.Avancar(g).Ok);

            CollectionAssert.AreEqual(new[] { "acordar" }, g.Missoes.ObjetivosFeitos(MissaoTeste.Q01), "ADR-0007 §6");
            Assert.AreEqual("falar_com_familia", MissaoMundo.Atual(g.Missoes, MissaoTeste.Q01).Id, "o primeiro ato e falar com a familia");
            Assert.AreEqual(1, gravacoes, "iniciar + acordar = uma gravacao");
            Assert.AreEqual(QuestStatus.EmAndamento, g.Missoes.Estado(MissaoTeste.Q01), "acordar nao conclui nada");
            Assert.AreEqual(TimeOfDay.Manha, TimeOfDayCycle.Atual(g.Save.life), "e nao faz o dia andar");
        }

        [Test]
        public void SaveAntigo_Q01ComAcordarPendente_SeResolveNoProximoAvancar_UmaVez()
        {
            SaveData antigo = new SaveData();
            Assert.IsTrue(new QuestSystem(antigo.quests, new HistoricoDeVidaLedger(antigo)).Iniciar(MissaoTeste.Q01).Ok);
            GameSession g = Abrir(LocalSave.FromJson(LocalSave.ToJson(antigo)));   // gravado antes do ADR-0007
            Assert.IsEmpty(g.Missoes.ObjetivosFeitos(MissaoTeste.Q01), "abrir o save nao mexe em missao");

            Assert.IsTrue(MissaoMundo.Avancar(g).Ok);
            CollectionAssert.Contains(g.Missoes.ObjetivosFeitos(MissaoTeste.Q01), "acordar");
            Assert.AreEqual(1, gravacoes);

            Assert.IsFalse(MissaoMundo.Avancar(g).Ok, "ja cumprido: nada a fazer");
            Assert.AreEqual(1, gravacoes, "o HUD chama 5x por segundo: objetivo automatico ja feito nao pode gravar de novo");
        }

        [Test]
        public void AbrirACena_IniciaSoAQ01_EGravaUmaVez()
        {
            GameSession g = Abrir(new SaveData());

            Assert.IsTrue(MissaoMundo.Avancar(g).Ok);

            Assert.AreEqual(QuestStatus.EmAndamento, g.Missoes.Estado(MissaoTeste.Q01), "B06: o despertar abre a q01");
            foreach (QuestDef d in QuestCatalog.Missoes)
                if (d.Id != MissaoTeste.Q01) Assert.AreNotEqual(QuestStatus.EmAndamento, g.Missoes.Estado(d.Id), d.Id);
            Assert.AreEqual(1, gravacoes);

            Assert.IsFalse(MissaoMundo.Avancar(g).Ok, "nada mais a fazer");
            Assert.AreEqual(1, gravacoes, "Avancar sem mudanca nao grava");
        }

        [Test]
        public void Q02_NaoComecaSozinha_EDoDialogo()
        {
            GameSession g = Abrir(new SaveData());
            MissaoTeste.Concluir(g.Missoes, MissaoTeste.Q01);

            MissaoMundo.Avancar(g);

            Assert.AreEqual(QuestStatus.Disponivel, g.Missoes.Estado(MissaoTeste.Q02), "B06: disponivel com Daren, nao iniciada");
        }

        [Test]
        public void Q07ComTudoFeito_ConcluiEAQ08ComecaSozinha_NaMesmaGravacao()
        {
            GameSession g = Abrir(new SaveData());
            QuestSystem m = g.Missoes;
            MissaoTeste.Concluir(m, MissaoTeste.Q01);
            MissaoTeste.Concluir(m, MissaoTeste.Q02);
            MissaoTeste.Concluir(m, MissaoTeste.Q04);
            MissaoTeste.Cumprir(m, MissaoTeste.Q07);   // o dialogo cumpriu o ultimo objetivo; ninguem concluiu

            Assert.IsTrue(MissaoMundo.Avancar(g).Ok);

            Assert.AreEqual(QuestStatus.Concluida, m.Estado(MissaoTeste.Q07));
            Assert.AreEqual(QuestStatus.EmAndamento, m.Estado(MissaoTeste.Q08), "a q08 nao tem NPC: comeca sozinha");
            Assert.IsTrue(g.Historia.Ja("marco.desaparecimento"));
            Assert.AreEqual(1, gravacoes);
        }

        [Test]
        public void Q04SemDesfecho_NaoConclui_EConcluiDepoisDoDesfecho()
        {
            GameSession g = Abrir(new SaveData());
            QuestSystem m = g.Missoes;
            MissaoTeste.Concluir(m, MissaoTeste.Q01);
            MissaoTeste.Concluir(m, MissaoTeste.Q02);
            MissaoMundo.Avancar(g);   // inventario alcanca a q02 aqui, fora da conta abaixo
            QuestDef q04 = QuestCatalog.Missao(MissaoTeste.Q04);
            Assert.IsTrue(m.Iniciar(q04.Id).Ok);
            foreach (ObjetivoDef o in q04.Objetivos) Assert.IsTrue(m.CumprirObjetivo(q04.Id, o.Id).Ok);
            gravacoes = 0;

            Assert.IsFalse(MissaoMundo.Avancar(g).Ok, "sem desfecho nao conclui (DesfechoPendente), nao forca");
            Assert.AreEqual(QuestStatus.EmAndamento, m.Estado(q04.Id));
            Assert.AreEqual(0, gravacoes);

            Assert.IsTrue(g.Missao(x => x.EscolherDesfecho(q04.Id, q04.Desfechos[0])).Ok);   // o dialogo decide
            Assert.IsTrue(MissaoMundo.Avancar(g).Ok);
            Assert.AreEqual(QuestStatus.Concluida, m.Estado(q04.Id));
            Assert.AreEqual(2, gravacoes);
        }

        [Test]
        public void GatilhoOrdenado_SoOProximoPendente_ESomeDepoisDeCumprido()
        {
            GameSession g = Abrir(new SaveData());
            QuestSystem m = g.Missoes;
            Assert.IsFalse(MissaoMundo.Ativo(m, MissaoTeste.Q01, "sair_de_casa"), "missao nao iniciada: gatilho escondido");
            MissaoMundo.Avancar(g);   // inicia a q01 e cumpre "acordar" (automatico)

            Assert.IsTrue(MissaoMundo.Ativo(m, MissaoTeste.Q01, "falar_com_familia"));
            Assert.IsFalse(MissaoMundo.Ativo(m, MissaoTeste.Q01, "sair_de_casa"), "fora de ordem: falta falar com a familia (NPC)");

            Assert.IsTrue(g.Missao(x => x.CumprirObjetivo(MissaoTeste.Q01, "falar_com_familia")).Ok);   // o dialogo
            Assert.IsTrue(MissaoMundo.Ativo(m, MissaoTeste.Q01, "sair_de_casa"));

            Assert.IsTrue(MissaoMundo.Cumprir(g, MissaoTeste.Q01, "sair_de_casa").Ok);
            Assert.IsFalse(MissaoMundo.Ativo(m, MissaoTeste.Q01, "sair_de_casa"), "cumprido some");
        }

        [Test]
        public void GatilhoParalelo_ValeEmQualquerOrdem()
        {
            GameSession g = Abrir(new SaveData());
            QuestSystem m = g.Missoes;
            MissaoTeste.Concluir(m, MissaoTeste.Q01);
            Assert.IsTrue(m.Iniciar(MissaoTeste.Q03).Ok);

            Assert.IsTrue(MissaoMundo.Ativo(m, MissaoTeste.Q03, "procurar_na_horta"), "q03 e paralela: a horta vale ja");
            Assert.IsFalse(MissaoMundo.Ativo(m, MissaoTeste.Q05, "encontrar_o_animal"), "q05 nao iniciada");
        }

        [Test]
        public void GatilhoDoUltimoObjetivo_ConcluiNaMesmaGravacao()
        {
            GameSession g = Abrir(new SaveData());
            MissaoMundo.Avancar(g);
            g.Missao(x => x.CumprirObjetivo(MissaoTeste.Q01, "falar_com_familia"));
            int antes = gravacoes;

            Assert.IsTrue(MissaoMundo.Cumprir(g, MissaoTeste.Q01, "sair_de_casa").Ok);

            Assert.AreEqual(antes + 1, gravacoes, "objetivo + conclusao = uma gravacao");
            Assert.AreEqual(QuestStatus.Concluida, g.Missoes.Estado(MissaoTeste.Q01));
            Assert.IsTrue(g.Historia.Ja("marco.primeiro_dia"), "B06: concluir grava o marco");
            Assert.AreEqual(QuestStatus.Disponivel, g.Missoes.Estado(MissaoTeste.Q02));
        }

        [Test]
        public void GatilhoRecusado_NaoGrava()
        {
            GameSession g = Abrir(new SaveData());

            Assert.IsFalse(MissaoMundo.Cumprir(g, MissaoTeste.Q01, "sair_de_casa").Ok, "missao nao iniciada");
            MissaoMundo.Avancar(g);
            QuestResultado r = MissaoMundo.Cumprir(g, MissaoTeste.Q01, "sair_de_casa");

            Assert.IsFalse(r.Ok);
            Assert.AreEqual(QuestErro.ObjetivoForaDeOrdem, r.Erro);
            Assert.AreEqual(1, gravacoes, "so o inicio da q01 gravou");
        }

        [Test]
        public void Hud_CentralAntesDaOpcional_ComOObjetivoAtual()
        {
            GameSession g = Abrir(new SaveData());
            QuestSystem m = g.Missoes;
            MissaoTeste.Concluir(m, MissaoTeste.Q01);
            Assert.IsTrue(m.Iniciar(MissaoTeste.Q03).Ok);
            Assert.IsTrue(m.Iniciar(MissaoTeste.Q02).Ok);

            var lista = MissaoMundo.EmAndamento(m);

            Assert.AreEqual(2, lista.Count);
            Assert.AreEqual(MissaoTeste.Q02, lista[0].Id, "central primeiro, mesmo iniciada depois");
            Assert.AreEqual(MissaoTeste.Q03, lista[1].Id);
            Assert.AreEqual("receber_tarefa", MissaoMundo.Atual(m, MissaoTeste.Q02).Id);
            m.CumprirObjetivo(MissaoTeste.Q02, "receber_tarefa");
            Assert.AreEqual("cumprir_tarefa", MissaoMundo.Atual(m, MissaoTeste.Q02).Id);
            Assert.AreEqual("missao.q02.obj.cumprir_tarefa", MissaoMundo.Atual(m, MissaoTeste.Q02).TextoKey);
        }
    }

    /// <summary>Atalhos de montagem para os testes da T012. Mexem direto no QuestSystem (sem sessao, sem gravar), como
    /// o dialogo faria: e o estado de partida que o teste quer, nao o que ele prova.</summary>
    internal static class MissaoTeste
    {
        public const string Q01 = "q01_um_novo_amanhecer";
        public const string Q02 = "q02_uma_pequena_responsabilidade";
        public const string Q03 = "q03_o_cesto_perdido";
        public const string Q04 = "q04_uma_promessa";
        public const string Q05 = "q05_o_animal_ferido";
        public const string Q07 = "q07_o_desaparecimento";
        public const string Q08 = "q08_ecos_do_limiar";

        /// <summary>Inicia (se preciso), grava o primeiro desfecho se houver e cumpre todos os objetivos. Nao conclui.
        /// Desfecho antes, como a conversa: na Q-07 ele e cobrado ao cumprir perguntar_na_vila (QuestDef.ObjetivoDoDesfecho).</summary>
        public static void Cumprir(QuestSystem m, string questId)
        {
            QuestDef d = QuestCatalog.Missao(questId);
            if (m.Estado(questId) != QuestStatus.EmAndamento) Assert.IsTrue(m.Iniciar(questId).Ok, questId + " nao iniciou");
            if (d.Desfechos.Length > 0) Assert.IsTrue(m.EscolherDesfecho(questId, d.Desfechos[0]).Ok);
            foreach (ObjetivoDef o in d.Objetivos) Assert.IsTrue(m.CumprirObjetivo(questId, o.Id).Ok, questId + "." + o.Id);
        }

        public static void Concluir(QuestSystem m, string questId)
        {
            Cumprir(m, questId);
            Assert.IsTrue(m.Concluir(questId).Ok, questId + " nao concluiu");
        }
    }
}
