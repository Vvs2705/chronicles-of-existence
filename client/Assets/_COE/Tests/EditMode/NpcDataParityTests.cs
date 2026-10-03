using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;

namespace COE.Tests
{
    /// <summary>Paridade entre o que a T007 carrega em C# e o que content/quests/*.json diz (mesmo regime do
    /// ADR-0005 e de QuestDataParityTests: o C# e o runtime, o JSON e a copia do redator, e divergir fica
    /// vermelho aqui em vez de aparecer no playtest). Depende de UnityEngine (Application.dataPath).
    /// ponytail: regex, pelo mesmo motivo de QuestDataParityTests -- 8 arquivos de formato fixo.</summary>
    public class NpcDataParityTests
    {
        static string[] Jsons()
        {
            string raiz = Path.GetFullPath(Path.Combine(Application.dataPath, "..", ".."));
            string dir = Path.Combine(raiz, "content", "quests");
            Assert.IsTrue(Directory.Exists(dir), "pasta de dados ausente: " + dir);
            return Directory.GetFiles(dir, "q*.json");
        }

        static List<string> Textos(string lista)
        {
            List<string> r = new List<string>();
            foreach (Match m in Regex.Matches(lista, "\"([^\"]+)\"")) r.Add(m.Groups[1].Value);
            return r;
        }

        [Test]
        public void Testemunhos_BatemComRegistraNoHistorico()
        {
            List<string> doJson = new List<string>();
            foreach (string arquivo in Jsons())
            {
                string json = File.ReadAllText(arquivo);
                Match bloco = Regex.Match(json, "\"registra_no_historico\"\\s*:\\s*\\[(.*?)\\n\\s*\\]", RegexOptions.Singleline);
                Assert.IsTrue(bloco.Success, "sem registra_no_historico: " + arquivo);
                foreach (Match item in Regex.Matches(bloco.Groups[1].Value, "\\{[^{}]*\\}"))
                {
                    string evento = Regex.Match(item.Value, "\"evento\"\\s*:\\s*\"([^\"]*)\"").Groups[1].Value;
                    foreach (string npc in Textos(Regex.Match(item.Value, "\"npcs\"\\s*:\\s*\\[([^\\]]*)\\]").Groups[1].Value))
                        doJson.Add(evento + " <- " + npc);
                }
            }
            Assert.Greater(doJson.Count, 0, "o parser nao achou nada: formato do JSON mudou?");

            List<string> doCodigo = new List<string>();
            foreach (Testemunho t in NpcMemory.Testemunhos)
            {
                if (t.EventoId == AgeAdvanceCatalog.SaltoInfancia) continue;   // B14: o salto nao e evento de missao (vem do AgeAdvance)
                foreach (string npc in t.Npcs) doCodigo.Add(t.EventoId + " <- " + npc);
            }

            CollectionAssert.AreEquivalent(doJson, doCodigo,
                "NpcMemory.Testemunhos e registra_no_historico[].npcs divergiram: mude os dois lados");
        }

        [Test]
        public void PedidoDeMissao_SoSaiDeNpcDaMissao()
        {
            // O achado desta auditoria: Lysa pedia a q03, que e de Oren e Nilo (SLICE_A B07).
            Dictionary<string, List<string>> npcsDaMissao = new Dictionary<string, List<string>>();
            foreach (string arquivo in Jsons())
            {
                string json = File.ReadAllText(arquivo);
                string id = Regex.Match(json, "\"id\"\\s*:\\s*\"([^\"]*)\"").Groups[1].Value;   // o primeiro "id" e o da missao
                // "npcs" no topo e o unico no comeco de linha; o dos objetivos vem depois de "{ ...".
                Match npcs = Regex.Match(json, "^[ \\t]*\"npcs\"\\s*:\\s*\\[([^\\]]*)\\]", RegexOptions.Multiline);
                Assert.IsTrue(npcs.Success, "sem npcs no topo: " + arquivo);
                npcsDaMissao[id] = Textos(npcs.Groups[1].Value);
            }

            foreach (DialogueGraph g in DialogueCatalog.Grafos)
                foreach (DialogueNode n in g.Nos)
                    foreach (DialogueOption o in n.Opcoes)
                    {
                        if (o.Pedido == null) continue;
                        string onde = g.Id + "/" + n.Id + ": ";
                        Assert.IsTrue(npcsDaMissao.ContainsKey(o.Pedido.QuestId), onde + "missao sem JSON: " + o.Pedido.QuestId);
                        CollectionAssert.Contains(npcsDaMissao[o.Pedido.QuestId], g.NpcId,
                            onde + g.NpcId + " pede " + o.Pedido.QuestId + ", mas a missao nao e dele");
                    }
        }
    }
}
