using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Validacao do DADO de dialogo (T007): o catalogo nao pode ter beco sem saida, referencia
    /// quebrada nem conversa sem fallback offline. Puro.</summary>
    public class DialogueGraphTests
    {
        [Test]
        public void Catalogo_ETodoValido()
        {
            string[] erros = DialogueCatalog.ValidarTodos();
            Assert.AreEqual(0, erros.Length, string.Join(" | ", erros));
        }

        [Test]
        public void NenhumNo_FicaSemContinuacao()
        {
            // Beco sem saida = no com opcoes em que TODAS dependem de condicao: existe estado de jogo em
            // que o jogador abre a conversa e nao tem nada para clicar.
            foreach (DialogueGraph g in DialogueCatalog.Grafos)
                foreach (DialogueNode n in g.Nos)
                {
                    if (n.Opcoes.Length == 0) continue;   // terminal e saida valida
                    bool temSaida = false;
                    foreach (DialogueOption o in n.Opcoes)
                        if (o.Condicao.Tipo == CondicaoTipo.Sempre) temSaida = true;
                    Assert.IsTrue(temSaida, g.Id + "/" + n.Id + " nao tem opcao incondicional");
                }
        }

        [Test]
        public void TodoGrafo_TemEntradaIncondicional()
        {
            foreach (DialogueGraph g in DialogueCatalog.Grafos)
            {
                bool tem = false;
                foreach (DialogueNode n in g.Nos) if (n.Condicao.Tipo == CondicaoTipo.Sempre) tem = true;
                Assert.IsTrue(tem, g.Id + " nao abre sem condicao: sem fallback offline");
            }
        }

        [Test]
        public void TodoGrafo_EDeUmNpcQueExiste()
        {
            foreach (DialogueGraph g in DialogueCatalog.Grafos)
                Assert.IsNotNull(NpcCatalog.Npc(g.NpcId), g.Id + " aponta para NPC inexistente: " + g.NpcId);
        }

        [Test]
        public void Lembra_SoCitaEventoQueONpcTestemunha()
        {
            // Lembra(x) de evento que o NPC nunca presencia nunca passa (no morto); NaoLembra(x) sempre passa.
            // Os dois sao erro de dado: a memoria so nasce do historico via NpcMemory.Testemunhos.
            foreach (DialogueGraph g in DialogueCatalog.Grafos)
                foreach (DialogueNode n in g.Nos)
                {
                    ConferirMemoria(g, n.Id, n.Condicao);
                    foreach (DialogueOption o in n.Opcoes) ConferirMemoria(g, n.Id, o.Condicao);
                }
        }

        static void ConferirMemoria(DialogueGraph g, string noId, Condicao c)
        {
            if (c.Tipo != CondicaoTipo.Lembra && c.Tipo != CondicaoTipo.NaoLembra) return;
            Assert.IsTrue(NpcMemory.Testemunha(g.NpcId, c.Chave),
                g.Id + "/" + noId + " depende de " + c.Chave + ", que " + g.NpcId + " nunca presencia");
        }

        // --- conteudo da leva A (ADR-0007): os dez NPCs falam, e a fala muda com o estado ---

        const string Q07 = "q07_o_desaparecimento";

        static DialogueContext Ctx(string npcId, NpcBook memoria, string missaoEmAndamento)
        {
            DialogueContext c = new DialogueContext();
            c.NpcId = npcId;
            c.Periodo = TimeOfDay.Tarde;
            c.Memoria = memoria;
            c.EstadoDaMissao = id => id == missaoEmAndamento ? EstadoMissao.EmAndamento : EstadoMissao.Indisponivel;
            return c;
        }

        static string Entrada(string npcId, NpcBook memoria, string missaoEmAndamento)
        {
            return DialogueRunner.Entrada(DialogueCatalog.Do(npcId), Ctx(npcId, memoria, missaoEmAndamento)).Id;
        }

        [Test]
        public void TodoNpc_TemConversa_ComVariacaoPorEstado()
        {
            foreach (NpcDef n in NpcCatalog.Npcs)
            {
                DialogueGraph g = DialogueCatalog.Do(n.Id);
                Assert.IsNotNull(g, n.Id + " nao tem conversa escrita");
                Assert.GreaterOrEqual(g.Nos.Length, 3, g.Id + ": conversa curta demais");

                bool varia = false;   // pelo menos uma entrada que depende de periodo, memoria, missao ou conhecimento
                foreach (DialogueNode no in g.Nos) if (no.Condicao.Tipo != CondicaoTipo.Sempre) varia = true;
                Assert.IsTrue(varia, g.Id + " diz a mesma coisa em qualquer estado de jogo");
            }
            Assert.AreEqual(NpcCatalog.Npcs.Length, DialogueCatalog.Grafos.Length, "um grafo por NPC, sem sobra");
        }

        [Test]
        public void B08_SeraENilo_FalamDiferente_ConformeAPromessa()
        {
            // O criterio central do slice: a escolha da Q-04 tem de ser visivel na fala de quem a viveu.
            foreach (string npc in new[] { "sera", "nilo" })
            {
                string semPromessa = Entrada(npc, new NpcBook(), null);

                NpcBook cumpriu = new NpcBook();
                NpcMemory.Registrar(cumpriu, npc, "evento.q04_promessa_cumprida", Importancia.Marcante, 1);
                NpcBook quebrou = new NpcBook();
                NpcMemory.Registrar(quebrou, npc, "evento.q04_promessa_quebrada", Importancia.Marcante, 1);

                string c = Entrada(npc, cumpriu, null);
                string q = Entrada(npc, quebrou, null);
                Assert.AreNotEqual(c, q, npc + " fala igual nos dois desfechos");
                Assert.AreNotEqual(semPromessa, c, npc + " nao reage a promessa cumprida");
                Assert.AreNotEqual(semPromessa, q, npc + " nao reage a promessa quebrada");

                DialogueGraph g = DialogueCatalog.Do(npc);
                Assert.AreNotEqual(g.No(c).TextoKey, g.No(q).TextoKey, npc + ": nos diferentes, mesma fala");
            }
        }

        [Test]
        public void B08_Sera_RespondeDiferenteSobreNilo_ConformeAPromessa()
        {
            // Quando Nilo some, a mesma escolha volta como custo: a resposta de Sera depende do desfecho.
            DialogueGraph g = DialogueCatalog.Do("sera");
            var destinos = new System.Collections.Generic.List<string>();
            foreach (string ev in new[] { "evento.q04_promessa_cumprida", "evento.q04_promessa_quebrada" })
            {
                NpcBook book = new NpcBook();
                NpcMemory.Registrar(book, "sera", ev, Importancia.Marcante, 1);

                DialogueContext semBusca = Ctx("sera", book, null);
                DialogueNode no = DialogueRunner.Entrada(g, semBusca);
                foreach (DialogueOption o in DialogueRunner.Opcoes(g, no, semBusca))
                    Assert.IsTrue(string.IsNullOrEmpty(o.ProximoNoId), "sem a q07, ninguem pergunta por Nilo: " + o.TextoKey);

                DialogueContext naBusca = Ctx("sera", book, Q07);
                DialogueStep passo = DialogueRunner.Escolher(g, no, 0, naBusca);
                Assert.IsTrue(passo.Ok && passo.Proximo != null, ev + ": com a q07 em andamento, a primeira opcao pergunta por Nilo");
                destinos.Add(passo.Proximo.Id);
            }
            Assert.AreNotEqual(destinos[0], destinos[1]);
        }

        [Test]
        public void B09_QuatroNpcs_FalamDoSumico_SoEnquantoAQ07Anda_ECadaUmDizUmaCoisa()
        {
            // Maelis, Eira e Oren dao pista parcial; Tovin aponta o bosque. Sem a missao, ninguem fala de sumico.
            var falas = new System.Collections.Generic.List<string>();
            foreach (string npc in new[] { "maelis", "eira", "oren", "tovin" })
            {
                string normal = Entrada(npc, new NpcBook(), null);
                string naBusca = Entrada(npc, new NpcBook(), Q07);
                Assert.AreNotEqual(normal, naBusca, npc + " nao muda de fala com a q07 em andamento");

                string chave = DialogueCatalog.Do(npc).No(naBusca).TextoKey;
                Assert.IsFalse(falas.Contains(chave), npc + " repete a pista de outro NPC");
                falas.Add(chave);
            }
        }

        [Test]
        public void Validar_AcusaBecoSemSaida()
        {
            DialogueGraph ruim = new DialogueGraph("teste_beco", "borin", new[]
            {
                new DialogueNode("entrada", "t", Condicao.Sempre, new[]
                {
                    new DialogueOption("so_se_lembrar", Condicao.Lembra("evento.nunca"), null, null),
                }),
            });
            string[] erros = ruim.Validar();
            Assert.AreEqual(1, erros.Length, string.Join(" | ", erros));
            StringAssert.Contains("beco sem saida", erros[0]);
        }

        [Test]
        public void Validar_AcusaReferenciaQuebrada()
        {
            DialogueGraph ruim = new DialogueGraph("teste_ref", "borin", new[]
            {
                new DialogueNode("entrada", "t", Condicao.Sempre, new[]
                {
                    new DialogueOption("ir", Condicao.Sempre, "no_que_nao_existe", null),
                }),
            });
            string[] erros = ruim.Validar();
            Assert.AreEqual(1, erros.Length, string.Join(" | ", erros));
            StringAssert.Contains("no inexistente", erros[0]);
        }

        [Test]
        public void Validar_AcusaGrafoSemEntradaIncondicional()
        {
            DialogueGraph ruim = new DialogueGraph("teste_sem_fallback", "borin", new[]
            {
                new DialogueNode("so_de_noite", "t", Condicao.Periodo(TimeOfDay.Noite), new DialogueOption[0]),
            });
            string[] erros = ruim.Validar();
            Assert.AreEqual(1, erros.Length, string.Join(" | ", erros));
            StringAssert.Contains("fallback offline", erros[0]);
        }
    }
}
