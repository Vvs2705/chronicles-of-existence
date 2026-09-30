using System;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>ADR-0007 §1: o periodo anda UM passo quando uma missao e concluida e quando o jogador descansa em casa,
    /// e so nesses dois casos; sempre pela GameSession, uma gravacao por transicao, recusa nao grava.
    /// E a consequencia do ADR: com o dia girando, todo NPC de quem uma missao aberta depende continua alcancavel
    /// em qualquer periodo (anti-softlock). C# puro: nada de cena.</summary>
    public class PassagemDoDiaTests
    {
        int gravacoes;
        GameSession Abrir(SaveData s) { return new GameSession(s, () => gravacoes++); }

        [SetUp] public void Zerar() { gravacoes = 0; }

        static TimeOfDay Periodo(GameSession g) { return TimeOfDayCycle.Atual(g.Save.life); }

        // ---------------------------------------------------------------- missao concluida

        [Test]
        public void ConcluirMissao_AvancaUmPeriodo_NaMesmaGravacao()
        {
            GameSession g = Abrir(new SaveData());
            MissaoMundo.Avancar(g);
            g.Missao(m => m.CumprirObjetivo(MissaoTeste.Q01, "falar_com_familia"));
            Assert.AreEqual(TimeOfDay.Manha, Periodo(g), "objetivo cumprido nao e missao concluida: o dia nao anda");
            int antes = gravacoes;

            Assert.IsTrue(MissaoMundo.Cumprir(g, MissaoTeste.Q01, "sair_de_casa").Ok);   // ultimo objetivo: conclui

            Assert.AreEqual(QuestStatus.Concluida, g.Missoes.Estado(MissaoTeste.Q01));
            Assert.AreEqual(TimeOfDay.Tarde, Periodo(g), "missao concluida = um periodo");
            Assert.AreEqual(antes + 1, gravacoes, "objetivo + conclusao + periodo = uma gravacao");

            Assert.IsFalse(MissaoMundo.Avancar(g).Ok);
            Assert.AreEqual(TimeOfDay.Tarde, Periodo(g), "reavaliar sem concluir nada nao anda o dia");
            Assert.AreEqual(antes + 1, gravacoes);
        }

        [Test]
        public void ConclusaoPedidaPeloDialogo_TambemAvanca()
        {
            GameSession g = Abrir(new SaveData());
            MissaoTeste.Concluir(g.Missoes, MissaoTeste.Q01);   // direto no QuestSystem: montagem, fora da sessao
            MissaoTeste.Cumprir(g.Missoes, MissaoTeste.Q02);
            Assert.AreEqual(TimeOfDay.Manha, Periodo(g));

            Assert.IsTrue(g.Missao(m => m.Concluir(MissaoTeste.Q02)).Ok);

            Assert.AreEqual(TimeOfDay.Tarde, Periodo(g), "qualquer transicao da sessao que conclua anda o dia");
            Assert.AreEqual(1, gravacoes);
        }

        [Test]
        public void DuasConclusoesNoMesmoPasso_AvancamUmPeriodoSo()
        {
            GameSession g = Abrir(new SaveData());
            MissaoTeste.Concluir(g.Missoes, MissaoTeste.Q01);
            MissaoTeste.Cumprir(g.Missoes, MissaoTeste.Q02);
            MissaoTeste.Cumprir(g.Missoes, MissaoTeste.Q03);

            Assert.IsTrue(MissaoMundo.Avancar(g).Ok);

            Assert.AreEqual(QuestStatus.Concluida, g.Missoes.Estado(MissaoTeste.Q02));
            Assert.AreEqual(QuestStatus.Concluida, g.Missoes.Estado(MissaoTeste.Q03));
            Assert.AreEqual(TimeOfDay.Tarde, Periodo(g), "um passo de conclusao = um periodo, nao um por missao");
            Assert.AreEqual(1, gravacoes);
        }

        [Test]
        public void Q07ConcluiEQ08Inicia_NoMesmoPasso_UmPeriodo()
        {
            GameSession g = Abrir(new SaveData());
            MissaoTeste.Concluir(g.Missoes, MissaoTeste.Q01);
            MissaoTeste.Concluir(g.Missoes, MissaoTeste.Q02);
            MissaoTeste.Concluir(g.Missoes, MissaoTeste.Q04);
            MissaoTeste.Cumprir(g.Missoes, MissaoTeste.Q07);

            Assert.IsTrue(MissaoMundo.Avancar(g).Ok);

            Assert.AreEqual(QuestStatus.EmAndamento, g.Missoes.Estado(MissaoTeste.Q08));
            Assert.AreEqual(TimeOfDay.Tarde, Periodo(g));
            Assert.AreEqual(1, gravacoes);
        }

        [Test]
        public void TransicaoRecusada_NaoAndaODiaNemGrava()
        {
            GameSession g = Abrir(new SaveData());

            Assert.IsFalse(g.Missao(m => m.Concluir(MissaoTeste.Q01)).Ok, "nao iniciada");
            Assert.IsFalse(g.Missao(m => m.Concluir("missao_que_nao_existe")).Ok);

            Assert.AreEqual(TimeOfDay.Manha, Periodo(g));
            Assert.AreEqual(0, gravacoes);
        }

        // ---------------------------------------------------------------- descansar

        [Test]
        public void Descansar_AvancaUmPeriodo_GravaUmaVez_ENoiteViraAManhaSeguinte()
        {
            GameSession g = Abrir(new SaveData());

            Assert.IsTrue(g.Descansar());
            Assert.AreEqual(TimeOfDay.Tarde, Periodo(g));
            Assert.AreEqual(1, gravacoes, "um descanso = uma gravacao");

            Assert.IsTrue(g.Descansar());
            Assert.AreEqual(TimeOfDay.Noite, Periodo(g));
            Assert.AreEqual(1, g.Save.life.day);

            Assert.IsTrue(g.Descansar());
            Assert.AreEqual(TimeOfDay.Manha, Periodo(g));
            Assert.AreEqual(2, g.Save.life.day, "dormiu, acordou: outro dia");
            Assert.AreEqual(3, gravacoes);
            Assert.AreEqual(5, g.Save.ageYears, "descansar nao envelhece: idade so no salto");
        }

        [Test]
        public void Descansar_SemOBlocoDeVida_RecusaENaoGrava()
        {
            SaveData s = new SaveData();
            s.life = null;   // bloco gravado como null
            GameSession g = Abrir(s);

            Assert.IsFalse(g.Descansar());
            Assert.AreEqual(0, gravacoes);
        }

        [Test]
        public void OPeriodoNovo_MudaARotinaQueONpcLe()
        {
            // O NpcActor pergunta NpcCatalog.Onde(TimeOfDayCycle.Atual(save.life)) a cada quadro: andar o dia e o que
            // tira o ferreiro da forja (NpcSceneTests prova o teleporte em cena).
            GameSession g = Abrir(new SaveData());
            Assert.AreEqual("ferraria", NpcCatalog.Onde("borin", Periodo(g), g.Save.npcs).AncoraId);

            g.Descansar();
            g.Descansar();

            Assert.AreEqual(TimeOfDay.Noite, Periodo(g));
            Assert.AreEqual("praca_centro", NpcCatalog.Onde("borin", Periodo(g), g.Save.npcs).AncoraId);
        }

        // ---------------------------------------------------------------- anti-softlock

        /// <summary>Para toda missao ABERTA (Disponivel ou EmAndamento): quem a oferece e todo NPC de objetivo ainda
        /// pendente tem ancora de verdade (de Auren, nunca a sentinela "ausente") nos TRES periodos, com a memoria
        /// deste save. E o que garante que girar o dia nunca tira do mapa alguem de quem o jogador precisa.</summary>
        static void TodoNpcExigidoEstaEmAuren(GameSession g, string quando)
        {
            foreach (QuestDef d in QuestCatalog.Missoes)
            {
                QuestStatus st = g.Missoes.Estado(d.Id);
                if (st != QuestStatus.Disponivel && st != QuestStatus.EmAndamento) continue;
                string[] feitos = g.Missoes.ObjetivosFeitos(d.Id);

                foreach (var p in MissaoNaConversa.Participantes)
                {
                    bool daMissao = p.Chave == d.Id;
                    bool deObjetivoPendente = p.Chave.StartsWith(d.Id + "/", StringComparison.Ordinal)
                        && Array.IndexOf(feitos, p.Chave.Substring(d.Id.Length + 1)) < 0;
                    if (!daMissao && !deObjetivoPendente) continue;

                    foreach (string npc in p.Npcs)
                        foreach (TimeOfDay periodo in Enum.GetValues(typeof(TimeOfDay)))
                        {
                            RotinaEntrada e = NpcCatalog.Onde(npc, periodo, g.Save.npcs);
                            string onde = quando + ": " + p.Chave + " precisa de " + npc + " de " + periodo;
                            Assert.IsNotNull(e, onde + ", e ele nao tem rotina");
                            CollectionAssert.Contains(NpcCatalogTests.AncorasDaT008, e.AncoraId, onde + ", e ele nao esta em Auren");
                        }
                }
            }
        }

        /// <summary>Joga uma missao pela sessao, como o jogo: inicia (se o NPC ainda nao iniciou), escolhe o primeiro
        /// desfecho se houver, cumpre objetivo por objetivo e deixa o MissaoMundo.Avancar concluir.</summary>
        static void Jogar(GameSession g, string questId)
        {
            QuestDef d = QuestCatalog.Missao(questId);
            if (g.Missoes.Estado(questId) == QuestStatus.Disponivel) Assert.IsTrue(g.Missao(m => m.Iniciar(questId)).Ok, questId);
            TodoNpcExigidoEstaEmAuren(g, questId + " iniciada");
            if (d.Desfechos.Length > 0) Assert.IsTrue(g.Missao(m => m.EscolherDesfecho(questId, d.Desfechos[0])).Ok);

            foreach (ObjetivoDef o in d.Objetivos)
            {
                string objetivoId = o.Id;
                Assert.IsTrue(g.Missao(m => m.CumprirObjetivo(questId, objetivoId)).Ok, questId + "." + objetivoId);
                MissaoMundo.Avancar(g);   // o que o MissaoHud faz 5x/s: conclui, inicia as automaticas
                TodoNpcExigidoEstaEmAuren(g, "depois de " + questId + "." + objetivoId);
            }
            Assert.AreEqual(QuestStatus.Concluida, g.Missoes.Estado(questId), questId + " nao concluiu");
        }

        [Test]
        public void CampanhaInteira_ComODiaGirando_TodoNpcExigidoContinuaAlcancavel()
        {
            GameSession g = Abrir(new SaveData());
            MissaoMundo.Avancar(g);
            TodoNpcExigidoEstaEmAuren(g, "save novo");

            Jogar(g, MissaoTeste.Q01);
            // O pior caso do ADR-0007 §3: a q03 aberta, com a trilha (Nilo) pendente, quando a q04 conclui.
            Assert.IsTrue(g.Missao(m => m.Iniciar(MissaoTeste.Q03)).Ok);
            Assert.IsTrue(g.Missao(m => m.CumprirObjetivo(MissaoTeste.Q03, "procurar_na_praca")).Ok);
            Jogar(g, MissaoTeste.Q02);
            Jogar(g, MissaoTeste.Q04);

            Assert.IsTrue(g.Historia.Ja(QuestCatalog.EventoNiloDesapareceu));
            Assert.AreEqual(QuestStatus.Falhada, g.Missoes.Estado(MissaoTeste.Q03), "aberta e pedindo Nilo: encerrada junto");

            Jogar(g, MissaoTeste.Q07);
            Jogar(g, MissaoTeste.Q08);   // comecou sozinha quando a q07 concluiu

            // Cinco conclusoes = cinco periodos: manha do dia 1 -> noite do dia 2.
            Assert.AreEqual(TimeOfDay.Noite, Periodo(g));
            Assert.AreEqual(2, g.Save.life.day);
            Assert.IsTrue(SaltoHud.Disponivel(g), "a campanha fecha com o dia girando: o salto esta liberado");
        }
    }
}
