using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>GDD cap. 12, criterio da T007: "rotina interrompivel e condicional por memoria". Puro: sem cena,
    /// sem relogio real, sem save em disco.</summary>
    public class NpcAgendaTests
    {
        static readonly Interrupcao Incendio =
            new Interrupcao("evento_teste", Interrupcao.PrioridadeEvento, "praca_centro", "atividade.teste");

        static NpcAgenda Agenda(string npcId) { return new NpcAgenda(NpcCatalog.Npc(npcId)); }

        // --- interrompivel ---

        [Test]
        public void SemInterrupcao_SegueARotinaDoPeriodo()
        {
            NpcAgenda borin = Agenda("borin");
            Assert.IsNull(borin.Atual);
            Assert.AreEqual("ferraria", borin.Agora(TimeOfDay.Manha, null).AncoraId);
            Assert.AreEqual(NpcCatalog.Onde("borin", TimeOfDay.Noite).AncoraId, borin.Agora(TimeOfDay.Noite, null).AncoraId);
        }

        [Test]
        public void Conversa_Interrompe_ERetomaNaRotinaDoPeriodoAtual()
        {
            NpcAgenda lysa = Agenda("lysa");
            Assert.AreEqual("ervanaria", lysa.Agora(TimeOfDay.Manha, null).AncoraId, "montagem do teste");

            Assert.IsTrue(lysa.Interromper(Interrupcao.Conversa));
            RotinaEntrada conversando = lysa.Agora(TimeOfDay.Manha, null);
            Assert.IsNull(conversando.AncoraId, "na conversa ela para onde esta");
            Assert.AreEqual(Interrupcao.Conversa.AtividadeKey, conversando.AtividadeKey);

            // A conversa atravessou manha -> tarde: continua conversando, e ao acabar vai para a rotina DA TARDE.
            Assert.AreEqual(Interrupcao.Conversa.AtividadeKey, lysa.Agora(TimeOfDay.Tarde, null).AtividadeKey);
            Assert.IsTrue(lysa.Retomar(Interrupcao.Conversa.Id));
            Assert.IsNull(lysa.Atual);
            Assert.AreEqual("entrada_bosque", lysa.Agora(TimeOfDay.Tarde, null).AncoraId,
                "retomar e voltar a rotina do periodo atual, nao a foto de antes");
        }

        [Test]
        public void EventoDaVila_PassaNaFrenteDaConversa_EFecharAConversaNaoCancelaOEvento()
        {
            NpcAgenda tovin = Agenda("tovin");
            Assert.IsTrue(tovin.Interromper(Interrupcao.Conversa));
            Assert.IsTrue(tovin.Interromper(Incendio), "prioridade maior toma o lugar da conversa");
            Assert.AreEqual("praca_centro", tovin.Agora(TimeOfDay.Manha, null).AncoraId);

            Assert.IsFalse(tovin.Retomar(Interrupcao.Conversa.Id), "a UI fechando a conversa nao apaga o incendio");
            Assert.AreSame(Incendio, tovin.Atual);

            Assert.IsTrue(tovin.Retomar(Incendio.Id));
            Assert.AreEqual(NpcCatalog.Onde("tovin", TimeOfDay.Manha).AncoraId, tovin.Agora(TimeOfDay.Manha, null).AncoraId);
        }

        [Test]
        public void Conversa_NaoFuraEvento_NemOutraConversa()
        {
            NpcAgenda maelis = Agenda("maelis");
            Assert.IsTrue(maelis.Interromper(Incendio));
            Assert.IsFalse(maelis.Interromper(Interrupcao.Conversa), "ocupada: a conversa nao abre");
            Assert.AreSame(Incendio, maelis.Atual, "o false nao mudou nada");

            NpcAgenda eira = Agenda("eira");
            Assert.IsTrue(eira.Interromper(Interrupcao.Conversa));
            Assert.IsFalse(eira.Interromper(Interrupcao.Conversa), "empate nao troca");
        }

        [Test]
        public void EntradaInvalida_NaoInterrompeENaoLanca()
        {
            NpcAgenda oren = Agenda("oren");
            Assert.IsFalse(oren.Interromper(null));
            Assert.IsFalse(oren.Interromper(new Interrupcao("", Interrupcao.PrioridadeEvento, null, "atividade.teste")));
            Assert.IsFalse(oren.Retomar("nada_em_curso"));
            Assert.IsNull(oren.Atual);
            Assert.IsNull(new NpcAgenda(null).Agora(TimeOfDay.Manha, null), "NPC desconhecido nao tem rotina");
        }

        // --- condicional por memoria ---

        /// <summary>NPC de teste: o catalogo de Auren ainda nao publica rotina condicional (conteudo da T012).
        /// A entrada condicional vem DEPOIS da incondicional de proposito: a memoria vence em qualquer ordem.</summary>
        static NpcDef NpcComRotinaCondicional()
        {
            return new NpcDef("teste", "npc.teste.nome", "npc.teste.papel", new string[0], new[]
            {
                new RotinaEntrada(TimeOfDay.Noite, "posto_guarda", "atividade.teste_de_sempre"),
                new RotinaEntrada(TimeOfDay.Noite, "entrada_bosque", "atividade.teste_depois", "evento.teste"),
                new RotinaEntrada(TimeOfDay.Manha, "praca_centro", "atividade.teste_manha"),
            }, new Vinculo[0], new string[0]);
        }

        [Test]
        public void Rotina_MudaComOQueONpcLembra()
        {
            NpcDef n = NpcComRotinaCondicional();
            NpcBook book = new NpcBook();
            Assert.AreEqual("posto_guarda", n.Onde(TimeOfDay.Noite, book).AncoraId, "sem a lembranca, a rotina de sempre");
            Assert.AreEqual("posto_guarda", n.Onde(TimeOfDay.Noite, null).AncoraId);

            NpcMemory.Registrar(book, "teste", "evento.teste", Importancia.Notavel, 1);
            Assert.AreEqual("entrada_bosque", n.Onde(TimeOfDay.Noite, book).AncoraId, "lembrou, mudou o comportamento");
            Assert.AreEqual("praca_centro", n.Onde(TimeOfDay.Manha, book).AncoraId, "so o periodo condicionado muda");
        }

        [Test]
        public void Rotina_EAMemoriaDESTENpc_NaoADeOutro()
        {
            NpcBook book = new NpcBook();
            NpcMemory.Registrar(book, "borin", "evento.teste", Importancia.Notavel, 1);
            Assert.AreEqual("posto_guarda", NpcComRotinaCondicional().Onde(TimeOfDay.Noite, book).AncoraId,
                "o que Borin lembra nao muda a rotina de outro NPC");
        }

        [Test]
        public void Retomada_RespeitaAMemoria()
        {
            NpcBook book = new NpcBook();
            NpcMemory.Registrar(book, "teste", "evento.teste", Importancia.Notavel, 1);
            NpcAgenda a = new NpcAgenda(NpcComRotinaCondicional());

            Assert.IsTrue(a.Interromper(Interrupcao.Conversa));
            Assert.IsTrue(a.Retomar(Interrupcao.Conversa.Id));
            Assert.AreEqual("entrada_bosque", a.Agora(TimeOfDay.Noite, book).AncoraId,
                "depois da interrupcao, a rotina volta ja condicionada pelo que ele lembra");
        }
    }
}
