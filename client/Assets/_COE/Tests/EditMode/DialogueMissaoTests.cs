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
        const string Q06 = "q06_o_segredo_do_ferreiro";

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

        static string QuestPedido(OpcaoDeMissao o)
        {
            Assert.AreEqual(1, o.Pedidos.Length);
            return o.Pedidos[0].QuestId + ":" + o.Pedidos[0].Intencao.Acao;
        }
    }
}
