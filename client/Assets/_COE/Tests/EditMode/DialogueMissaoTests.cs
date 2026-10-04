using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;

namespace COE.Tests
{
    /// <summary>T012: objetivo de missao com NPC se cumpre conversando (MissaoNaConversa). Paridade com o JSON, quem
    /// ve qual opcao em que estado, ordem dos objetivos e desfecho da Q-04. Concluir e do MissaoMundo (L19): aqui so
    /// se prova que a conversa deixa a missao sem pendencia. Tudo em C# puro sobre um SaveData novo; nada grava em disco.</summary>
    public class DialogueMissaoTests
    {
        const string Q01 = "q01_um_novo_amanhecer";
        const string Q02 = "q02_uma_pequena_responsabilidade";
        const string Q04 = "q04_uma_promessa";
        const string Q05 = "q05_o_animal_ferido";
        const string Q06 = "q06_o_segredo_do_ferreiro";
        const string Q07 = "q07_o_desaparecimento";

        static QuestSystem Missoes(SaveData s) { return new QuestSystem(s.quests, new HistoricoDeVidaLedger(s, new LifeEventHistory(s))); }

        static List<string> Chaves(OpcaoDeMissao[] ops)
        {
            var r = new List<string>();
            foreach (OpcaoDeMissao o in ops) r.Add(o.TextoKey);
            return r;
        }

        static OpcaoDeMissao Opcao(OpcaoDeMissao[] ops, string textoKey)
        {
            foreach (OpcaoDeMissao o in ops) if (o.TextoKey == textoKey) return o;
            Assert.Fail("sem a opcao " + textoKey + " em [" + string.Join(", ", Chaves(ops)) + "]");
            return null;
        }

        static void Linha(SaveData s, string questId, QuestStatus status, params string[] feitos)
        {
            var q = new QuestState { questId = questId, status = (int)status };
            q.objetivosFeitos.AddRange(feitos);
            s.quests.missoes.Add(q);
        }

        [Test]
        public void Participantes_BatemComOJson()
        {
            string dir = Path.Combine(Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..")), "content", "quests");
            var doJson = new List<string>();
            foreach (string arquivo in Directory.GetFiles(dir, "q*.json"))
            {
                string json = File.ReadAllText(arquivo);
                string id = Regex.Match(json, "\"id\"\\s*:\\s*\"([^\"]*)\"").Groups[1].Value;
                Match topo = Regex.Match(json, "^[ \\t]*\"npcs\"\\s*:\\s*\\[([^\\]]*)\\]", RegexOptions.Multiline);
                Assert.IsTrue(topo.Success, "sem npcs no topo: " + arquivo);
                foreach (Match n in Regex.Matches(topo.Groups[1].Value, "\"([^\"]+)\"")) doJson.Add(id + " <- " + n.Groups[1].Value);
                // Objetivo: objeto de uma linha com texto_key (recompensa e historico nao tem).
                foreach (Match o in Regex.Matches(json, "\\{\\s*\"id\"\\s*:\\s*\"([^\"]+)\"\\s*,\\s*\"texto_key\"[^{}\\n]*\"npcs\"\\s*:\\s*\\[([^\\]]*)\\]"))
                    foreach (Match n in Regex.Matches(o.Groups[2].Value, "\"([^\"]+)\""))
                        doJson.Add(id + "/" + o.Groups[1].Value + " <- " + n.Groups[1].Value);
            }
            Assert.Greater(doJson.Count, 20, "o parser nao achou os objetivos: formato do JSON mudou?");

            var doCodigo = new List<string>();
            foreach (var p in MissaoNaConversa.Participantes)
                foreach (string npc in p.Npcs) doCodigo.Add(p.Chave + " <- " + npc);

            CollectionAssert.AreEquivalent(doJson, doCodigo, "MissaoNaConversa.Participantes e o JSON divergiram: mude os dois lados");
        }

        [Test]
        public void SaveNovo_SoAFamiliaOfereceAQ01()
        {
            QuestSystem m = Missoes(new SaveData());

            Assert.AreEqual(Q01 + ":Iniciar", QuestPedido(Opcao(MissaoNaConversa.Opcoes("mara", m), "missao.q01.titulo")));
            Assert.IsNotEmpty(MissaoNaConversa.Opcoes("daren", m));
            Assert.IsEmpty(MissaoNaConversa.Opcoes("borin", m), "q06 ainda nao esta disponivel e a q01 nao e de Borin");
            Assert.IsEmpty(MissaoNaConversa.Opcoes("nilo", m));
        }

        [Test]
        public void MissaoOrdenada_SoOProximoObjetivoConta_ESoComQuemParticipa()
        {
            SaveData s = new SaveData();
            QuestSystem m = Missoes(s);
            Assert.IsTrue(m.Iniciar(Q01).Ok);

            Assert.IsEmpty(MissaoNaConversa.Opcoes("mara", m), "o proximo e 'acordar', que nao tem NPC (gatilho da L19)");

            Assert.IsTrue(m.CumprirObjetivo(Q01, "acordar").Ok);
            OpcaoDeMissao falar = Opcao(MissaoNaConversa.Opcoes("mara", m), "missao.q01.obj.falar_com_familia");
            Assert.IsNotEmpty(MissaoNaConversa.Opcoes("daren", m));
            Assert.IsEmpty(MissaoNaConversa.Opcoes("oren", m));

            QuestResultado r = MissaoNaConversa.Aplicar(m, falar.Pedidos);
            Assert.IsTrue(r.Ok);
            Assert.AreEqual(QuestStatus.EmAndamento, r.Status);
            CollectionAssert.Contains(m.ObjetivosFeitos(Q01), "falar_com_familia");
            Assert.IsEmpty(MissaoNaConversa.Opcoes("daren", m), "objetivo cumprido some das duas conversas");
        }

        [Test]
        public void Q02_InteiraPelaConversa_CadaObjetivoComQuemParticipa()
        {
            SaveData s = new SaveData();
            QuestSystem m = Missoes(s);
            m.Iniciar(Q01);
            foreach (ObjetivoDef o in m.Def(Q01).Objetivos) m.CumprirObjetivo(Q01, o.Id);
            Assert.IsTrue(m.Concluir(Q01).Ok);

            Assert.IsTrue(MissaoNaConversa.Aplicar(m, Opcao(MissaoNaConversa.Opcoes("daren", m), "missao.q02.titulo").Pedidos).Ok);
            MissaoNaConversa.Aplicar(m, Opcao(MissaoNaConversa.Opcoes("daren", m), "missao.q02.obj.receber_tarefa").Pedidos);
            MissaoNaConversa.Aplicar(m, Opcao(MissaoNaConversa.Opcoes("oren", m), "missao.q02.obj.cumprir_tarefa").Pedidos);
            OpcaoDeMissao contas = Opcao(MissaoNaConversa.Opcoes("daren", m), "missao.q02.obj.prestar_contas");

            Assert.IsTrue(MissaoNaConversa.Aplicar(m, contas.Pedidos).Ok);
            Assert.IsEmpty(MissaoNaConversa.Opcoes("daren", m), "nada pendente com Daren");
            Assert.AreEqual(QuestStatus.EmAndamento, m.Estado(Q02), "a conversa nao conclui: e o MissaoMundo.Avancar");
            Assert.IsTrue(m.Concluir(Q02).Ok, "sem pendencia: o proximo Avancar da L19 conclui e paga");

            Assert.IsFalse(MissaoNaConversa.Aplicar(m, contas.Pedidos).Ok, "opcao velha depois de concluida e recusada");
        }

        [Test]
        public void Q04_Decidir_OfereceUmBotaoPorDesfecho_EGravaSoUm()
        {
            SaveData s = new SaveData();
            Linha(s, Q04, QuestStatus.EmAndamento, "ouvir_o_pedido");
            QuestSystem m = Missoes(s);

            OpcaoDeMissao[] ops = MissaoNaConversa.Opcoes("nilo", m);
            Assert.AreEqual(2, ops.Length, "cumprida ou quebrada, nunca 'cumprir decidir' sem escolher");
            OpcaoDeMissao cumprida = Opcao(ops, MissaoNaConversa.ChaveDoDesfecho("evento.q04_promessa_cumprida"));
            Opcao(ops, MissaoNaConversa.ChaveDoDesfecho("evento.q04_promessa_quebrada"));

            Assert.IsTrue(MissaoNaConversa.Aplicar(m, cumprida.Pedidos).Ok);
            CollectionAssert.Contains(m.ObjetivosFeitos(Q04), "decidir");
            Assert.IsEmpty(MissaoNaConversa.Opcoes("sera", m), "Sera nao participa de sustentar_a_escolha");

            Assert.IsTrue(MissaoNaConversa.Aplicar(m, Opcao(MissaoNaConversa.Opcoes("nilo", m), "missao.q04.obj.sustentar_a_escolha").Pedidos).Ok);
            Assert.IsTrue(m.Concluir(Q04).Ok, "desfecho gravado + objetivos feitos: o MissaoMundo consegue concluir");
            Assert.IsTrue(new LifeEventHistory(s).Ja("evento.q04_promessa_cumprida"));
            Assert.IsFalse(new LifeEventHistory(s).Ja("evento.q04_promessa_quebrada"));
        }

        /// <summary>ADR-0010 adendo 11: em perguntar_na_vila so Maelis fecha o objetivo, com dois botoes de desfecho; Eira e
        /// Oren ficam com a pista na fala. Pedir o outro depois de assinar e recusado; a Q-07 conclui depois.</summary>
        [Test]
        public void Q07_PerguntarNaVila_SoMaelisAssina_DoisBotoes_EUmSoFica()
        {
            SaveData s = new SaveData();
            Linha(s, Q07, QuestStatus.EmAndamento, "notar_a_ausencia");
            QuestSystem m = Missoes(s);

            foreach (string npc in new[] { "eira", "oren", "tovin" })
                Assert.IsEmpty(MissaoNaConversa.Opcoes(npc, m), npc + " nao fecha perguntar_na_vila");

            OpcaoDeMissao[] ops = MissaoNaConversa.Opcoes("maelis", m);
            Assert.AreEqual(2, ops.Length, "tracar o sinal ou fazer um risco; nunca 'cumprir' sem assinar");
            OpcaoDeMissao circulo = Opcao(ops, MissaoNaConversa.ChaveDoDesfecho(QuestCatalog.EventoAssinouComOCirculo));
            OpcaoDeMissao risco = Opcao(ops, MissaoNaConversa.ChaveDoDesfecho(QuestCatalog.EventoAssinouComUmRisco));

            Assert.IsTrue(MissaoNaConversa.Aplicar(m, risco.Pedidos).Ok);
            CollectionAssert.Contains(m.ObjetivosFeitos(Q07), "perguntar_na_vila");
            Assert.IsEmpty(MissaoNaConversa.Opcoes("maelis", m), "assinou: os botoes somem");
            QuestResultado outro = MissaoNaConversa.Aplicar(m, circulo.Pedidos);   // botao velho na tela
            Assert.IsFalse(outro.Ok);
            Assert.AreEqual(QuestErro.DesfechoJaDecidido, outro.Erro, "exatamente um, nunca os dois");

            Assert.IsTrue(MissaoNaConversa.Aplicar(m, Opcao(MissaoNaConversa.Opcoes("tovin", m), "missao.q07.obj.seguir_ate_o_bosque").Pedidos).Ok);
            Assert.IsTrue(m.Concluir(Q07).Ok, "o MissaoMundo consegue concluir");
            var h = new LifeEventHistory(s);
            Assert.IsTrue(h.Ja(QuestCatalog.EventoAssinouComUmRisco));
            Assert.IsFalse(h.Ja(QuestCatalog.EventoAssinouComOCirculo));

            NpcMemory.Sincronizar(s.npcs, h);
            Assert.IsTrue(NpcMemory.Lembra(s.npcs, "maelis", QuestCatalog.EventoAssinouComUmRisco), "Maelis testemunha a assinatura");
            Assert.IsFalse(NpcMemory.Lembra(s.npcs, "tovin", QuestCatalog.EventoAssinouComUmRisco), "so ela");
        }

        /// <summary>Ficha maelis C9: aos 8 a fala lembra qual sinal ficou no livro. Save sem assinatura (anterior a regra) fica
        /// na fala de sempre. So fala.</summary>
        [Test]
        public void Q07_AosOito_MaelisLembraQualSinalFicou()
        {
            DialogueGraph g = DialogueCatalog.Do("maelis");
            var entradas = new List<string>();
            foreach (string assinatura in new[] { QuestCatalog.EventoAssinouComOCirculo, QuestCatalog.EventoAssinouComUmRisco, null })
            {
                SaveData s = new SaveData();
                var h = new LifeEventHistory(s);
                h.Registrar("evento.q07_concluida", LifeEventCategoria.Marco, 5);
                if (assinatura != null) h.Registrar(assinatura, LifeEventCategoria.Marco, 5);
                h.Registrar(AgeAdvanceCatalog.SaltoInfancia, LifeEventCategoria.Marco, 8);
                NpcMemory.Sincronizar(s.npcs, h);
                entradas.Add(DialogueRunner.Entrada(g, new DialogueContext { NpcId = "maelis", Memoria = s.npcs }).Id);
            }
            CollectionAssert.AreEqual(new[] { "aos_oito_registro_circulo", "aos_oito_registro_risco", "aos_oito_registro" }, entradas);
        }

        /// <summary>ADR-0010 adendo 10: tratar_o_animal so aparece com o bicho calmo, com Lysa ou Tovin. Lysa ensina a
        /// chegar devagar em qualquer periodo. A q05 continua opcional e paga o mesmo.</summary>
        [Test]
        public void Q05_TratarOAnimal_SoComOBichoCalmo_ComLysaOuTovin()
        {
            SaveData s = new SaveData();
            Linha(s, Q05, QuestStatus.EmAndamento, "encontrar_o_animal", "buscar_ajuda");
            QuestSystem m = Missoes(s);

            foreach (string npc in new[] { "lysa", "tovin" })
            {
                Assert.IsEmpty(MissaoNaConversa.Opcoes(npc, m), npc + ": sem o bicho calmo a opcao nao aparece");
                Opcao(MissaoNaConversa.Opcoes(npc, m, null, true), "missao.q05.obj.tratar_o_animal");
            }
            Assert.IsEmpty(MissaoNaConversa.Opcoes("borin", m, null, true), "so Lysa e Tovin");

            DialogueGraph lysa = DialogueCatalog.Do("lysa");
            foreach (TimeOfDay p in System.Enum.GetValues(typeof(TimeOfDay)))
            {
                var ctx = new DialogueContext
                {
                    NpcId = "lysa", Periodo = p, Memoria = s.npcs,
                    EstadoDaMissao = QuestIntentAdapter.Leitor(m),
                    ObjetivoProximo = (q, o) => MissaoNaConversa.EhOProximo(m, q, o),
                };
                Assert.AreEqual("chegar_devagar", DialogueRunner.Entrada(lysa, ctx).Id, p + ": Lysa diz como chegar");
            }

            OpcaoDeMissao tratar = Opcao(MissaoNaConversa.Opcoes("tovin", m, null, true), "missao.q05.obj.tratar_o_animal");
            Assert.IsTrue(MissaoNaConversa.Aplicar(m, tratar.Pedidos).Ok);
            QuestResultado fim = m.Concluir(Q05);
            Assert.IsTrue(fim.Ok);
            Assert.AreEqual(1, fim.Recompensas.Length, "nenhuma recompensa nova");
        }

        /// <summary>O catalogo amarra o desfecho a um objetivo (QuestDef.ObjetivoDoDesfecho); a conversa precisa oferecer os
        /// botoes nesse objetivo (Decisoes), senao ele nunca fecharia.</summary>
        [Test]
        public void ObjetivoDoDesfecho_EstaNasDecisoesDaConversa()
        {
            int achados = 0;
            foreach (QuestDef d in QuestCatalog.Missoes)
            {
                if (d.ObjetivoDoDesfecho == null) continue;
                achados++;
                Assert.IsNotNull(d.Objetivo(d.ObjetivoDoDesfecho), d.Id + ": objetivo do desfecho inexistente");
                Assert.Greater(d.Desfechos.Length, 1, d.Id + ": objetivo de desfecho sem desfechos");
                CollectionAssert.Contains(MissaoNaConversa.Decisoes, (d.Id, d.ObjetivoDoDesfecho), d.Id + ": sem botoes na conversa");
            }
            Assert.Greater(achados, 0);
        }

        /// <summary>Obrigatorio 8: as falas novas da q05 e da q07 so falam; quem pede e o dado de missao.</summary>
        [Test]
        public void FalasNovas_NaoPedemMissaoItemNemPoder()
        {
            foreach (string[] no in new[]
            {
                new[] { "maelis", "assinar" }, new[] { "maelis", "aos_oito_registro_circulo" },
                new[] { "maelis", "aos_oito_registro_risco" }, new[] { "lysa", "chegar_devagar" },
            })
            {
                DialogueNode n = DialogueCatalog.Do(no[0]).No(no[1]);
                Assert.IsNotNull(n, no[0] + "/" + no[1]);
                foreach (DialogueOption o in n.Opcoes) Assert.IsNull(o.Pedido, no[0] + "/" + no[1] + ": " + o.TextoKey);
            }
        }

        [Test]
        public void OpcaoAutoralVisivel_NaoDuplica()
        {
            SaveData s = new SaveData();
            Linha(s, Q06, QuestStatus.Disponivel);
            QuestSystem m = Missoes(s);
            Assert.AreEqual(1, MissaoNaConversa.Opcoes("borin", m).Length, "sem fala: a opcao de missao inicia a q06");

            DialogueGraph g = DialogueCatalog.Do("borin");
            var ctx = new DialogueContext { NpcId = "borin", EstadoDaMissao = QuestIntentAdapter.Leitor(m) };
            DialogueOption[] autorais = DialogueRunner.Opcoes(g, g.No("primeira_vez"), ctx);
            Assert.IsEmpty(MissaoNaConversa.Opcoes("borin", m, autorais), "a fala de Borin ja pede a q06");
        }

        [Test]
        public void Aplicar_SemPedido_OuSemMissoes_NaoMudaNada()
        {
            SaveData s = new SaveData();
            QuestSystem m = Missoes(s);
            Assert.IsFalse(MissaoNaConversa.Aplicar(m, new PedidoDeMissao[0]).Ok);
            Assert.IsFalse(MissaoNaConversa.Aplicar(null, new[] { PedidoDeMissao.Iniciar(Q01) }).Ok);
            Assert.AreEqual(0, s.quests.missoes.Count);
            Assert.IsEmpty(MissaoNaConversa.Opcoes("mara", null));
        }

        [Test]
        public void Q06_DeNoite_ComAjudarBorinPendente_ElePedeParaVoltarDeManha()
        {
            SaveData s = new SaveData();
            Linha(s, Q06, QuestStatus.EmAndamento, "entrar_na_ferraria");
            QuestSystem m = Missoes(s);
            var ctx = new DialogueContext
            {
                NpcId = "borin",
                Periodo = TimeOfDay.Noite,
                EstadoDaMissao = QuestIntentAdapter.Leitor(m),
                ObjetivoProximo = (q, o) => MissaoNaConversa.EhOProximo(m, q, o),
            };
            DialogueNode no = DialogueRunner.Entrada(DialogueCatalog.Do("borin"), ctx);
            Assert.AreEqual("noite_forja", no.Id, "de noite ele nao esta na forja e diz quando voltar");
            Assert.IsTrue(StringsLoader.Load(StringsLoader.DefaultLanguage), "arquivo de textos");
            StringAssert.Contains("manhã", Strings.Get(no.TextoKey));

            ctx.Periodo = TimeOfDay.Manha;
            Assert.AreEqual("na_forja", DialogueRunner.Entrada(DialogueCatalog.Do("borin"), ctx).Id, "de manha a forja volta");
        }

        [Test]
        public void Q06_LerORisco_SoPelaFala_ErrarRepete_AcertarCumpre()
        {
            SaveData s = new SaveData();
            Linha(s, Q06, QuestStatus.EmAndamento, "entrar_na_ferraria");
            QuestSystem m = Missoes(s);
            var ctx = new DialogueContext
            {
                NpcId = "borin",
                EstadoDaMissao = QuestIntentAdapter.Leitor(m),
                ObjetivoProximo = (q, o) => MissaoNaConversa.EhOProximo(m, q, o),
            };
            DialogueGraph g = DialogueCatalog.Do("borin");
            DialogueNode no = DialogueRunner.Entrada(g, ctx);
            Assert.AreEqual("na_forja", no.Id, "ajudar_borin pendente: a conversa abre na forja");
            CollectionAssert.DoesNotContain(Chaves(MissaoNaConversa.Opcoes("borin", m, DialogueRunner.Opcoes(g, no, ctx))),
                "missao.q06.obj.ajudar_borin", "sem atalho: ajudar_borin so se cumpre lendo o risco");

            no = Ir(g, no, ctx, "dialogo.opcao.entregar_peca", null);
            Assert.AreEqual("de_novo", no.Id, "a primeira entrega volta");
            no = Ir(g, no, ctx, "dialogo.opcao.entregar_de_novo", null);
            Assert.AreEqual("gabarito", no.Id);
            no = Ir(g, no, ctx, "dialogo.opcao.entalhe_quarto", null);
            Assert.AreEqual("errou", no.Id, "leitura errada so repete");
            no = Ir(g, no, ctx, "dialogo.opcao.ler_de_novo", null);
            Assert.AreEqual("gabarito", no.Id);

            DialogueStep certo = Passo(g, no, ctx, "dialogo.opcao.entalhe_terceiro");
            Assert.AreEqual("leu_certo", certo.Proximo.Id);
            Assert.IsTrue(MissaoNaConversa.Aplicar(m, new[] { certo.Pedido }).Ok, "acertar cumpre o objetivo");
            CollectionAssert.Contains(m.ObjetivosFeitos(Q06), "ajudar_borin");
            Assert.IsFalse(MissaoNaConversa.EhOProximo(m, Q06, "ajudar_borin"));
            CollectionAssert.Contains(Chaves(MissaoNaConversa.Opcoes("borin", m)), "missao.q06.obj.guardar_o_segredo",
                "o segredo foi revelado: guardar e o proximo");
        }

        [Test]
        public void EhOProximo_RespeitaAOrdemDosObjetivos()
        {
            SaveData s = new SaveData();
            Linha(s, Q06, QuestStatus.EmAndamento);
            QuestSystem m = Missoes(s);
            Assert.IsTrue(MissaoNaConversa.EhOProximo(m, Q06, "entrar_na_ferraria"));
            Assert.IsFalse(MissaoNaConversa.EhOProximo(m, Q06, "ajudar_borin"), "q06 e em ordem: falta entrar");
            Assert.IsFalse(MissaoNaConversa.EhOProximo(m, Q02, "receber_tarefa"), "missao fora de andamento");
            Assert.IsFalse(MissaoNaConversa.EhOProximo(null, Q06, "entrar_na_ferraria"));
        }

        static DialogueStep Passo(DialogueGraph g, DialogueNode no, DialogueContext ctx, string textoKey)
        {
            DialogueOption[] vis = DialogueRunner.Opcoes(g, no, ctx);
            for (int i = 0; i < vis.Length; i++)
                if (vis[i].TextoKey == textoKey) return DialogueRunner.Escolher(g, no, i, ctx);
            Assert.Fail("no " + no.Id + " sem a opcao " + textoKey);
            return default(DialogueStep);
        }

        static DialogueNode Ir(DialogueGraph g, DialogueNode no, DialogueContext ctx, string textoKey, PedidoDeMissao esperado)
        {
            DialogueStep p = Passo(g, no, ctx, textoKey);
            Assert.IsTrue(p.Ok);
            Assert.AreEqual(esperado, p.Pedido);
            return p.Proximo;
        }

        static string QuestPedido(OpcaoDeMissao o)
        {
            Assert.AreEqual(1, o.Pedidos.Length);
            return o.Pedidos[0].QuestId + ":" + o.Pedidos[0].Intencao.Acao;
        }
    }
}
