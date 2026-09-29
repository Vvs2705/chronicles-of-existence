using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Memoria do NPC (T007): fatos com significado, por id, idempotentes, com decaimento do
    /// trivial. Puro: nao grava arquivo (o round-trip pelo save esta em NpcSaveRoundTripTests).</summary>
    public class NpcMemoryTests
    {
        NpcBook book;

        [SetUp]
        public void SetUp() { book = new NpcBook(); }

        [Test]
        public void Novo_NaoLembraDeNada()
        {
            Assert.AreEqual(0, book.fatos.Count);
            Assert.AreEqual(0, NpcMemory.Fatos(book, "borin").Length);
            Assert.IsFalse(NpcMemory.Lembra(book, "borin", "evento.ajudou_na_forja"));
        }

        [Test]
        public void Registrar_GuardaOFato()
        {
            Assert.IsTrue(NpcMemory.Registrar(book, "borin", "evento.ajudou_na_forja", Importancia.Notavel, 10));
            Assert.IsTrue(NpcMemory.Lembra(book, "borin", "evento.ajudou_na_forja"));
            Assert.AreEqual(1, NpcMemory.Fatos(book, "borin").Length);
            Assert.AreEqual((int)Importancia.Notavel, NpcMemory.Fatos(book, "borin")[0].importancia);
        }

        [Test]
        public void Registrar_NaoDuplica_MesmoIdDeEvento()
        {
            Assert.IsTrue(NpcMemory.Registrar(book, "borin", "evento.ajudou_na_forja", Importancia.Notavel, 10));
            Assert.IsFalse(NpcMemory.Registrar(book, "borin", "evento.ajudou_na_forja", Importancia.Marcante, 99),
                "o mesmo evento registrado de novo nao entra uma segunda vez");
            Assert.AreEqual(1, NpcMemory.Fatos(book, "borin").Length);
            Assert.AreEqual((int)Importancia.Notavel, NpcMemory.Fatos(book, "borin")[0].importancia,
                "a segunda chamada nao pode reescrever o fato que ja existia");
        }

        [Test]
        public void Memoria_EPorNpc_CadaUmLembraDoQueViu()
        {
            NpcMemory.Registrar(book, "borin", "evento.ajudou_na_forja", Importancia.Notavel, 10);
            Assert.IsTrue(NpcMemory.Lembra(book, "borin", "evento.ajudou_na_forja"));
            Assert.IsFalse(NpcMemory.Lembra(book, "lysa", "evento.ajudou_na_forja"),
                "conhecimento e limitado: Lysa nao viu o que aconteceu na ferraria");
        }

        [Test]
        public void EntradaInvalida_NaoRegistraENaoLanca()
        {
            Assert.IsFalse(NpcMemory.Registrar(book, null, "evento.x", Importancia.Trivial, 1));
            Assert.IsFalse(NpcMemory.Registrar(book, "borin", "", Importancia.Trivial, 1));
            Assert.IsFalse(NpcMemory.Registrar(null, "borin", "evento.x", Importancia.Trivial, 1));
            Assert.AreEqual(0, book.fatos.Count);
        }

        [Test]
        public void Trivial_DecaiParaResumo_QuandoPassaDoLimite()
        {
            int n = NpcMemory.LimiteTrivialPorNpc + 3;
            for (int i = 0; i < n; i++)
                NpcMemory.Registrar(book, "oren", "evento.recado_" + i, Importancia.Trivial, i);

            Assert.AreEqual(NpcMemory.LimiteTrivialPorNpc, NpcMemory.Fatos(book, "oren").Length,
                "a lista para de crescer: memoria nao e transcricao");
            Assert.AreEqual(3, NpcMemory.Esquecidos(book, "oren"), "o que saiu vira numero, nao some sem deixar sinal");
            Assert.IsFalse(NpcMemory.Lembra(book, "oren", "evento.recado_0"), "o mais antigo e o primeiro a sair");
            Assert.IsTrue(NpcMemory.Lembra(book, "oren", "evento.recado_" + (n - 1)), "o mais recente fica");
        }

        [Test]
        public void Marcante_NuncaDecai()
        {
            NpcMemory.Registrar(book, "tovin", "evento.salvou_a_vida", Importancia.Marcante, 0);
            for (int i = 0; i < NpcMemory.LimiteTrivialPorNpc + 10; i++)
                NpcMemory.Registrar(book, "tovin", "evento.conversa_" + i, Importancia.Trivial, i + 1);

            Assert.IsTrue(NpcMemory.Lembra(book, "tovin", "evento.salvou_a_vida"),
                "o que teve significado fica para sempre");
        }

        [Test]
        public void Decaimento_NaoVazaEntreNpcs()
        {
            for (int i = 0; i < NpcMemory.LimiteTrivialPorNpc + 2; i++)
                NpcMemory.Registrar(book, "oren", "evento.recado_" + i, Importancia.Trivial, i);
            NpcMemory.Registrar(book, "eira", "evento.aula_boa", Importancia.Trivial, 100);

            Assert.IsTrue(NpcMemory.Lembra(book, "eira", "evento.aula_boa"));
            Assert.AreEqual(0, NpcMemory.Esquecidos(book, "eira"));
        }

        // --- historico de vida -> memoria: quem lembra de que (achado da auditoria da T005) ---

        static LifeEventHistory Historico(params string[] eventos)
        {
            LifeEventHistory h = new LifeEventHistory(new LifeHistoryData());
            foreach (string e in eventos) h.Registrar(e, LifeEventCategoria.Marco, 5, "quest_teste");
            return h;
        }

        [Test]
        public void EventoNoHistorico_ViraLembrancaSoDeQuemTestemunhou()
        {
            Assert.AreEqual(1, NpcMemory.Sincronizar(book, Historico("evento.q06_concluida")));
            Assert.IsTrue(NpcMemory.Lembra(book, "borin", "evento.q06_concluida"));
            Assert.IsFalse(NpcMemory.Lembra(book, "lysa", "evento.q06_concluida"), "Lysa nao estava na ferraria");
        }

        [Test]
        public void Sincronizar_EIdempotente_RecarregarNaoDuplica()
        {
            LifeEventHistory h = Historico("evento.q04_concluida", "evento.q04_promessa_quebrada");
            Assert.AreEqual(4, NpcMemory.Sincronizar(book, h), "sera e nilo, dois eventos cada");
            Assert.AreEqual(0, NpcMemory.Sincronizar(book, h), "segunda passada (recarregar o save) nao acrescenta nada");
            Assert.AreEqual(2, NpcMemory.Fatos(book, "sera").Length);
            Assert.AreEqual(0, NpcMemory.Esquecidos(book, "sera"), "e nem mexe no resumo");
            Assert.AreEqual((int)Importancia.Marcante,
                NpcMemory.Fatos(book, "nilo")[1].importancia, "promessa quebrada e Marcante");
        }

        [Test]
        public void NpcNuncaLembraDeEventoQueNaoEstaNoHistorico()
        {
            // SLICE_A secao 4.3, regra negativa: lembrar de missao nunca feita e bug tao grave quanto esquecer.
            NpcMemory.Sincronizar(book, Historico("evento.q04_promessa_cumprida"));
            Assert.IsTrue(NpcMemory.Lembra(book, "sera", "evento.q04_promessa_cumprida"));
            Assert.IsFalse(NpcMemory.Lembra(book, "sera", "evento.q04_promessa_quebrada"), "so o desfecho que aconteceu");
            Assert.IsFalse(NpcMemory.Lembra(book, "lysa", "evento.q05_concluida"), "opcional nunca feita");
        }

        [Test]
        public void EventoSemTestemunha_NaoViraMemoria()
        {
            Assert.AreEqual(0, NpcMemory.Sincronizar(book, Historico("evento.q08_concluida", "nascimento")));
            Assert.AreEqual(0, book.fatos.Count);
            Assert.AreEqual(0, NpcMemory.Sincronizar(null, Historico("evento.q06_concluida")));
            Assert.AreEqual(0, NpcMemory.Sincronizar(book, null));
        }

        [Test]
        public void Testemunhos_NuncaTriviais_SemRepeticao_ENpcsQueExistem()
        {
            System.Collections.Generic.HashSet<string> vistos = new System.Collections.Generic.HashSet<string>();
            foreach (Testemunho t in NpcMemory.Testemunhos)
            {
                Assert.IsTrue(vistos.Add(t.EventoId), "evento repetido na tabela: " + t.EventoId);
                // Trivial decai e perde o id; Sincronizar roda a cada carga e o traria de volta inflando o resumo.
                Assert.AreNotEqual(Importancia.Trivial, t.Importancia, t.EventoId + " nao pode ser Trivial");
                Assert.Greater(t.Npcs.Length, 0, t.EventoId + " sem testemunha nao entra na tabela");
                foreach (string npc in t.Npcs)
                    Assert.IsNotNull(NpcCatalog.Npc(npc), t.EventoId + " cita NPC inexistente: " + npc);
            }
        }

        [Test]
        public void MissaoConcluidaDeVerdade_ChegaAMemoria_EAFala()
        {
            // Cadeia inteira, sem atalho: QuestSystem.Concluir -> historico (T005) -> Sincronizar -> dialogo.
            // A missao e a Q-06 do catalogo real, so sem pre-requisito, para o id do evento ser o publicado.
            QuestDef real = QuestCatalog.Missao("q06_o_segredo_do_ferreiro");
            QuestDef q06 = new QuestDef(real.Id, real.TituloKey, real.Tipo, real.Central, null, null,
                real.ObjetivosEmOrdem, real.Objetivos, real.Recompensas, real.EventoDeConclusao);
            SaveData save = new SaveData();
            LifeEventHistory historia = new LifeEventHistory(save);
            QuestSystem missoes = new QuestSystem(save.quests, new HistoricoDeVidaLedger(save, historia), new[] { q06 });

            Assert.IsTrue(missoes.Iniciar(q06.Id).Ok);
            foreach (ObjetivoDef o in q06.Objetivos) Assert.IsTrue(missoes.CumprirObjetivo(q06.Id, o.Id).Ok, o.Id);
            Assert.IsTrue(missoes.Concluir(q06.Id).Ok);
            Assert.AreEqual(0, save.npcs.fatos.Count, "a missao nao escreve memoria: quem liga e o orquestrador");

            Assert.AreEqual(1, NpcMemory.Sincronizar(save.npcs, historia));
            Assert.IsTrue(NpcMemory.Lembra(save.npcs, "borin", real.EventoDeConclusao));

            DialogueContext ctx = new DialogueContext();
            ctx.NpcId = "borin";
            ctx.Periodo = TimeOfDay.Manha;
            ctx.Memoria = save.npcs;
            Assert.AreEqual("reencontro", DialogueRunner.Entrada(DialogueCatalog.Do("borin"), ctx).Id,
                "Borin recebe quem ajudou na forja como conhecido (SLICE_A R13)");
        }

        [Test]
        public void Fato_NaoGuardaTranscricao()
        {
            // Prompt-mestre secao 9 / dossie secao G: memoria guarda fato com significado, nao conversa.
            foreach (System.Reflection.FieldInfo f in typeof(NpcMemoryFact).GetFields())
            {
                string nome = f.Name.ToLowerInvariant();
                Assert.IsFalse(nome.Contains("texto") || nome.Contains("fala") || nome.Contains("transcricao")
                    || nome.Contains("linha") || nome.Contains("dialogo"),
                    "NpcMemoryFact." + f.Name + " parece transcricao de conversa");
            }
        }
    }
}
