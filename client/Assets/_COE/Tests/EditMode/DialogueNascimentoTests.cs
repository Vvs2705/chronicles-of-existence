using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>O dialogo le o nascimento (C8 das fichas G1; ELENCO.md, "Condicao de dialogo por destino e por item"):
    /// as tres condicoes novas, o conteudo do DialogueCatalog que as usa e a ordem que impede a fala de nascimento de
    /// esconder missao ou lembranca. Puro: o contexto e montado como o DialogueHud monta, com o inventario saido de
    /// Inventario.Nascer sobre o DestinyCatalog de verdade.</summary>
    public class DialogueNascimentoTests
    {
        const string Q01 = "q01_um_novo_amanhecer";
        const string Q02 = "q02_uma_pequena_responsabilidade";
        const string Q06 = "q06_o_segredo_do_ferreiro";
        const string Q07 = "q07_o_desaparecimento";

        /// <summary>Contexto de quem nasceu em <paramref name="destino"/>/<paramref name="origem"/>: os itens sao os que
        /// o nascimento poe no inventario. Sem missao, sem memoria.</summary>
        static DialogueContext Nascido(string npcId, string destino, string origem, TimeOfDay periodo)
        {
            InventarioData inv = new InventarioData();
            Inventario.Nascer(inv, DestinyCatalog.Compor(destino, origem));
            DialogueContext c = new DialogueContext();
            c.NpcId = npcId;
            c.Periodo = periodo;
            c.Memoria = new NpcBook();
            c.Destino = destino;
            c.Origem = origem;
            c.QuantidadeDoItem = item => Inventario.Quantidade(inv, item);
            return c;
        }

        static string Entrada(string npcId, DialogueContext ctx)
        {
            return DialogueRunner.Entrada(DialogueCatalog.Do(npcId), ctx).Id;
        }

        static bool LeNascimento(Condicao c)
        {
            if (c.Tipo == CondicaoTipo.Destino || c.Tipo == CondicaoTipo.Origem || c.Tipo == CondicaoTipo.TemItem) return true;
            foreach (Condicao parte in c.Partes) if (LeNascimento(parte)) return true;
            return false;
        }

        static bool DeNascimento(DialogueNode n)
        {
            return LeNascimento(n.Condicao) || n.Id.StartsWith("destino_") || n.Id.StartsWith("origem_");
        }

        // --- as tres condicoes ---

        [Test]
        public void Destino_Origem_TemItem_PassamSoNoNascimentoCerto()
        {
            DialogueContext c = Nascido("borin", "dificil", "guardioes", TimeOfDay.Manha);

            Assert.IsTrue(DialogueRunner.Satisfaz(Condicao.Destino("dificil"), "borin", c));
            Assert.IsFalse(DialogueRunner.Satisfaz(Condicao.Destino("serena"), "borin", c));
            Assert.IsTrue(DialogueRunner.Satisfaz(Condicao.Origem("guardioes"), "borin", c));
            Assert.IsFalse(DialogueRunner.Satisfaz(Condicao.Origem("artesaos"), "borin", c));
            Assert.IsTrue(DialogueRunner.Satisfaz(Condicao.TemItem("item.faca_gasta"), "borin", c), "item do destino");
            Assert.IsTrue(DialogueRunner.Satisfaz(Condicao.TemItem("item.espada_de_madeira"), "borin", c), "item da origem");
            Assert.IsFalse(DialogueRunner.Satisfaz(Condicao.TemItem("item.amuleto_rachado"), "borin", c), "item de outro destino");
        }

        [Test]
        public void SemNascimento_NenhumaPassa()
        {
            // Teste, -scene ou save sem BirthChoice: Destino e Origem "", sem leitor de inventario.
            DialogueContext vazio = new DialogueContext { NpcId = "borin" };
            Condicao[] todas =
            {
                Condicao.Destino("serena"), Condicao.Origem("agricultores"), Condicao.TemItem("item.cantil"),
                Condicao.Destino(""), Condicao.Origem(""), Condicao.TemItem(""), Condicao.Destino(null),
            };
            foreach (Condicao c in todas)
            {
                Assert.IsFalse(DialogueRunner.Satisfaz(c, "borin", vazio), c.Tipo + " '" + c.Chave + "' passou sem nascimento");
                Assert.IsFalse(DialogueRunner.Satisfaz(c, "borin", null), c.Tipo + " '" + c.Chave + "' passou sem contexto");
            }

            vazio.QuantidadeDoItem = item => 0;
            Assert.IsFalse(DialogueRunner.Satisfaz(Condicao.TemItem("item.cantil"), "borin", vazio), "quantidade 0 nao e ter");
        }

        // --- o conteudo ---

        [Test]
        public void Catalogo_SoCitaIdsDoDestinyCatalog()
        {
            var destinos = new HashSet<string>();
            var origens = new HashSet<string>();
            var itens = new HashSet<string>();
            foreach (DestinyDef d in DestinyCatalog.Destinos) { destinos.Add(d.Id); itens.UnionWith(d.ItensIniciais); }
            foreach (OriginDef o in DestinyCatalog.Origens) { origens.Add(o.Id); itens.UnionWith(o.ItensIniciais); }

            int citadas = 0;
            foreach (DialogueGraph g in DialogueCatalog.Grafos)
                foreach (DialogueNode n in g.Nos)
                {
                    citadas += Conferir(g.Id + "/" + n.Id, n.Condicao, destinos, origens, itens);
                    foreach (DialogueOption o in n.Opcoes) citadas += Conferir(g.Id + "/" + n.Id, o.Condicao, destinos, origens, itens);
                }
            Assert.Greater(citadas, 30, "o catalogo quase nao le o nascimento");
        }

        static int Conferir(string onde, Condicao c, HashSet<string> destinos, HashSet<string> origens, HashSet<string> itens)
        {
            int n = 0;
            foreach (Condicao parte in c.Partes) n += Conferir(onde, parte, destinos, origens, itens);
            if (c.Tipo == CondicaoTipo.Destino) Assert.IsTrue(destinos.Contains(c.Chave), onde + ": destino inventado " + c.Chave);
            else if (c.Tipo == CondicaoTipo.Origem) Assert.IsTrue(origens.Contains(c.Chave), onde + ": origem inventada " + c.Chave);
            else if (c.Tipo == CondicaoTipo.TemItem) Assert.IsTrue(itens.Contains(c.Chave), onde + ": item que o nascimento nao da " + c.Chave);
            else return n;
            return n + 1;
        }

        /// <summary>Todo NPC adulto tem uma fala por destino, e existe um estado em que as quatro aparecem: a entrada
        /// muda com o destino. Nenhum no de nascimento fica morto atras de outro.</summary>
        [Test]
        public void CadaAdulto_TemUmaEntradaPorDestino_ENenhumNoDeNascimentoFicaMorto()
        {
            var adultos = new List<string> { "mara", "daren", "borin", "lysa", "tovin", "eira", "oren", "maelis" };
            foreach (DialogueGraph g in DialogueCatalog.Grafos)
            {
                var alcancados = new HashSet<string>();
                bool quatroDiferentes = false;
                foreach (TimeOfDay p in Enum.GetValues(typeof(TimeOfDay)))
                    foreach (OriginDef o in DestinyCatalog.Origens)
                    {
                        var entradas = new HashSet<string>();
                        foreach (DestinyDef d in DestinyCatalog.Destinos)
                        {
                            string id = Entrada(g.NpcId, Nascido(g.NpcId, d.Id, o.Id, p));
                            entradas.Add(id);
                            alcancados.Add(id);
                        }
                        quatroDiferentes |= entradas.Count == DestinyCatalog.Destinos.Length;
                    }

                foreach (DialogueNode n in g.Nos)
                    if (LeNascimento(n.Condicao))
                        Assert.IsTrue(alcancados.Contains(n.Id), g.Id + "/" + n.Id + " nunca e entrada: um no anterior sempre vence");

                if (g.No("destino_serena") == null) { Assert.IsFalse(adultos.Contains(g.NpcId), g.NpcId + " sem fala de destino"); continue; }
                foreach (DestinyDef d in DestinyCatalog.Destinos)
                    Assert.IsTrue(alcancados.Contains("destino_" + d.Id), g.Id + ": o destino " + d.Id + " nao tem fala propria");
                Assert.IsTrue(quatroDiferentes, g.Id + ": nenhum estado em que os quatro destinos falam diferente");
            }
        }

        /// <summary>Daren, Borin e Oren: a ferramenta da origem entra por opcao, uma so visivel, e leva a resposta
        /// daquela origem. Sem nascimento, a opcao nao aparece.</summary>
        [Test]
        public void FerramentaDaOrigem_UmaOpcao_QueLevaAOrigemCerta()
        {
            foreach (string npc in new[] { "daren", "borin", "oren" })
            {
                DialogueGraph g = DialogueCatalog.Do(npc);
                foreach (DestinyDef d in DestinyCatalog.Destinos)
                    foreach (OriginDef o in DestinyCatalog.Origens)
                    {
                        DialogueContext ctx = Nascido(npc, d.Id, o.Id, TimeOfDay.Manha);
                        DialogueNode no = DialogueRunner.Entrada(g, ctx);
                        DialogueOption[] vis = DialogueRunner.Opcoes(g, no, ctx);
                        int achadas = 0;
                        for (int i = 0; i < vis.Length; i++)
                        {
                            if (vis[i].ProximoNoId == null || !vis[i].ProximoNoId.StartsWith("origem_")) continue;
                            achadas++;
                            DialogueStep passo = DialogueRunner.Escolher(g, no, i, ctx);
                            Assert.IsTrue(passo.Ok);
                            Assert.AreEqual("origem_" + o.Id, passo.Proximo.Id, npc + "/" + d.Id + "/" + o.Id);
                            Assert.IsNull(passo.Pedido, "mostrar a ferramenta nao pede nada");
                        }
                        Assert.AreEqual(1, achadas, npc + "/" + d.Id + "/" + o.Id + " (" + no.Id + "): uma opcao de ferramenta");
                    }

                DialogueContext semNascimento = new DialogueContext { NpcId = npc, Periodo = TimeOfDay.Manha, Memoria = new NpcBook() };
                DialogueNode fallback = DialogueRunner.Entrada(g, semNascimento);
                foreach (DialogueOption op in DialogueRunner.Opcoes(g, fallback, semNascimento))
                    Assert.IsFalse(op.ProximoNoId != null && op.ProximoNoId.StartsWith("origem_"), npc + ": ferramenta sem nascimento");
            }
        }

        /// <summary>A ordem do grafo: missao, memoria e salto vencem a fala de nascimento em todos os destinos. E quando
        /// a fala de nascimento abre a conversa, o pedido de missao do fallback (q06 de Borin) continua visivel.</summary>
        [Test]
        public void FalaDeNascimento_NaoEscondeMissaoNemMemoria()
        {
            foreach (DestinyDef d in DestinyCatalog.Destinos)
            {
                string onde = " (" + d.Id + ")";

                DialogueContext c = Nascido("daren", d.Id, "agricultores", TimeOfDay.Manha);
                c.EstadoDaMissao = q => q == Q02 ? EstadoMissao.EmAndamento : EstadoMissao.Indisponivel;
                Assert.AreEqual("tarefa_pendente", Entrada("daren", c), "q02 em andamento" + onde);

                c = Nascido("mara", d.Id, "agricultores", TimeOfDay.Manha);
                c.EstadoDaMissao = q => q == Q01 ? EstadoMissao.EmAndamento : EstadoMissao.Indisponivel;
                Assert.AreEqual("primeiro_dia", Entrada("mara", c), "q01 em andamento" + onde);

                c = Nascido("tovin", d.Id, "guardioes", TimeOfDay.Manha);
                c.EstadoDaMissao = q => q == Q07 ? EstadoMissao.EmAndamento : EstadoMissao.Indisponivel;
                Assert.AreEqual("rastro_no_bosque", Entrada("tovin", c), "q07 em andamento" + onde);

                c = Nascido("borin", d.Id, "artesaos", TimeOfDay.Manha);
                c.ObjetivoProximo = (q, o) => q == Q06 && o == "ajudar_borin";
                Assert.AreEqual("na_forja", Entrada("borin", c), "ajudar_borin pendente" + onde);

                c = Nascido("borin", d.Id, "artesaos", TimeOfDay.Manha);
                NpcMemory.Registrar(c.Memoria, "borin", "evento.q06_concluida", Importancia.Notavel, 1);
                Assert.AreEqual("reencontro", Entrada("borin", c), "Borin lembra de quem ajudou" + onde);

                c = Nascido("sera", d.Id, "artesaos", TimeOfDay.Tarde);
                NpcMemory.Registrar(c.Memoria, "sera", "evento.q04_promessa_cumprida", Importancia.Marcante, 1);
                Assert.AreEqual("promessa_cumprida", Entrada("sera", c), "Sera lembra da promessa" + onde);

                c = Nascido("lysa", d.Id, "agricultores", TimeOfDay.Manha);
                NpcMemory.Registrar(c.Memoria, "lysa", AgeAdvanceCatalog.SaltoInfancia, Importancia.Marcante, 1);
                Assert.AreEqual("aos_oito", Entrada("lysa", c), "depois do salto" + onde);

                // q06 disponivel: a fala do destino abre, e o pedido de iniciar a q06 continua entre as opcoes.
                DialogueGraph g = DialogueCatalog.Do("borin");
                c = Nascido("borin", d.Id, "artesaos", TimeOfDay.Manha);
                c.EstadoDaMissao = q => q == Q06 ? EstadoMissao.Disponivel : EstadoMissao.Indisponivel;
                DialogueNode no = DialogueRunner.Entrada(g, c);
                StringAssert.StartsWith("destino_", no.Id, "Borin abre pelo destino" + onde);
                bool pedeQ06 = false;
                foreach (DialogueOption op in DialogueRunner.Opcoes(g, no, c))
                    pedeQ06 |= op.Pedido != null && op.Pedido.QuestId == Q06;
                Assert.IsTrue(pedeQ06, "a fala de destino escondeu o pedido da q06" + onde);
            }
        }

        /// <summary>So fala (dossie secao H; Obrigatorio8): opcao que le o nascimento nao pede nada, e no de nascimento
        /// so carrega pedido que o fallback do mesmo NPC ja carrega.</summary>
        [Test]
        public void FalaDeNascimento_NaoPedeMissaoItemNemPoder()
        {
            foreach (DialogueGraph g in DialogueCatalog.Grafos)
            {
                DialogueNode fallback = null;
                foreach (DialogueNode n in g.Nos) if (fallback == null && n.Condicao.Tipo == CondicaoTipo.Sempre) fallback = n;

                foreach (DialogueNode n in g.Nos)
                    foreach (DialogueOption o in n.Opcoes)
                    {
                        if (LeNascimento(o.Condicao)) Assert.IsNull(o.Pedido, g.Id + "/" + n.Id + ": " + o.TextoKey + " pede missao");
                        if (DeNascimento(n) && o.Pedido != null)
                            CollectionAssert.Contains(fallback.Opcoes, o, g.Id + "/" + n.Id + ": pedido que o fallback nao tem");
                    }
            }
        }

        /// <summary>ELENCO.md, Arbitragem 2, item 8: so Borin e Oren reagem ao amuleto da Ruptura.</summary>
        [Test]
        public void Amuleto_SoBorinEOrenReagem()
        {
            foreach (DialogueGraph g in DialogueCatalog.Grafos)
                foreach (DialogueNode n in g.Nos)
                    if (n.Condicao.Tipo == CondicaoTipo.TemItem && n.Condicao.Chave == "item.amuleto_rachado")
                        CollectionAssert.Contains(new[] { "borin", "oren" }, g.NpcId, g.Id + "/" + n.Id + " reage ao amuleto");
        }

        /// <summary>Toda fala e opcao nova esta escrita (nao cai em "[chave]"), sem rotulo de dificuldade para destino
        /// (ADR-0004) e sem explicar o Limiar (ADR-0007).</summary>
        [Test]
        public void FalasDeNascimento_Escritas_SemDificuldadeNemLimiar()
        {
            Assert.IsTrue(StringsLoader.Load(StringsLoader.DefaultLanguage), "arquivo de textos");
            try
            {
                var chaves = new List<string>();
                foreach (DialogueGraph g in DialogueCatalog.Grafos)
                    foreach (DialogueNode n in g.Nos)
                    {
                        if (DeNascimento(n)) chaves.Add(n.TextoKey);
                        foreach (DialogueOption o in n.Opcoes) if (LeNascimento(o.Condicao)) chaves.Add(o.TextoKey);
                    }
                Assert.Greater(chaves.Count, 40, "a varredura achou as falas de nascimento");

                foreach (string k in chaves)
                {
                    string v = Strings.Get(k);
                    Assert.IsFalse(v.StartsWith("["), k + " sem texto em Resources/strings.pt-BR.json");
                    Assert.IsNotEmpty(v.Trim(), k);
                    Assert.IsFalse(Regex.IsMatch(v, "f[aá]cil|dif[ií]cil|extrem", RegexOptions.IgnoreCase), k + ": destino nao e dificuldade: " + v);
                    Assert.IsFalse(Regex.IsMatch(v, "limiar", RegexOptions.IgnoreCase), k + ": ninguem em Auren nomeia o Limiar: " + v);
                }
            }
            finally { Strings.Load(null); }
        }
    }
}
