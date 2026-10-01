using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;

namespace COE.Tests
{
    /// <summary>ADR-0005: o catalogo em C# (QuestCatalog) e a fonte de verdade em runtime; os JSON de
    /// content/quests/ sao a copia que quem escreve conteudo le. Duas copias da mesma verdade divergem com o
    /// tempo, e a divergencia normalmente aparece so quando alguem joga. Este teste faz a divergencia aparecer
    /// no portao: compara id, titulo, tipo, central/opcional, pre-missoes, pre-eventos (eventos_de_vida),
    /// objetivos_concluidos (implicados por pre-missao), ids de objetivo, ids de transacao de recompensa,
    /// evento de conclusao, desfechos, registra_no_historico e flags. Vermelho aqui = alguem mexeu num lado so.
    /// Fora da paridade, de proposito: idade_min/idade_max/fase (o QuestSystem nao le idade; ver reporte T006).
    /// ponytail: parser de JSON por regex, nao por biblioteca — sao 8 arquivos com formato fixo gerado por
    /// script, e JsonUtility nao le dicionario aninhado. Se o formato virar algo de verdade, troque por
    /// um leitor real em vez de endurecer a regex.</summary>
    public class QuestDataParityTests
    {
        static string PastaDeDados()
        {
            // Assets/ -> client/ -> raiz do repositorio
            string raiz = Path.GetFullPath(Path.Combine(Application.dataPath, "..", ".."));
            return Path.Combine(raiz, "content", "quests");
        }

        static string Campo(string json, string nome)
        {
            Match m = Regex.Match(json, "\"" + nome + "\"\\s*:\\s*\"([^\"]*)\"");
            return m.Success ? m.Groups[1].Value : null;
        }

        static bool Booleano(string json, string nome)
        {
            Match m = Regex.Match(json, "\"" + nome + "\"\\s*:\\s*(true|false)");
            return m.Success && m.Groups[1].Value == "true";
        }

        /// <summary>Ids de um bloco de lista: "&lt;bloco&gt;": [ { ... "id": "x" ... }, ... ].</summary>
        static List<string> IdsDoBloco(string json, string bloco, string chave = "id")
        {
            var ids = new List<string>();
            foreach (Match m in Regex.Matches(Bloco(json, bloco), "\"" + chave + "\"\\s*:\\s*\"([^\"]*)\""))
                ids.Add(m.Groups[1].Value);
            return ids;
        }

        /// <summary>Conteudo de uma lista de objetos, um por linha, que fecha com "]" em linha propria.</summary>
        static string Bloco(string json, string bloco)
        {
            Match b = Regex.Match(json, "\"" + bloco + "\"\\s*:\\s*\\[(.*?)\\n\\s*\\]", RegexOptions.Singleline);
            return b.Success ? b.Groups[1].Value : "";
        }

        static List<string> ListaDeTexto(string json, string bloco)
        {
            var itens = new List<string>();
            Match b = Regex.Match(json, "\"" + bloco + "\"\\s*:\\s*\\[(.*?)\\]", RegexOptions.Singleline);
            if (!b.Success) return itens;
            foreach (Match m in Regex.Matches(b.Groups[1].Value, "\"([^\"]+)\""))
                itens.Add(m.Groups[1].Value);
            return itens;
        }

        static Dictionary<string, string> LerJsons()
        {
            string dir = PastaDeDados();
            Assert.IsTrue(Directory.Exists(dir), "pasta de dados ausente: " + dir);
            var mapa = new Dictionary<string, string>();
            foreach (string arquivo in Directory.GetFiles(dir, "q*.json"))
            {
                string json = File.ReadAllText(arquivo);
                string id = Campo(json, "id");
                Assert.IsNotNull(id, "sem campo id: " + arquivo);
                mapa[id] = json;
            }
            return mapa;
        }

        [Test]
        public void CatalogoEJsonDescrevemAsMesmasMissoes()
        {
            Dictionary<string, string> jsons = LerJsons();
            Assert.AreEqual(QuestCatalog.Missoes.Length, jsons.Count,
                "quantidade de missoes diferente entre QuestCatalog e content/quests");

            foreach (QuestDef def in QuestCatalog.Missoes)
            {
                Assert.IsTrue(jsons.ContainsKey(def.Id), "missao do catalogo sem JSON: " + def.Id);
                string json = jsons[def.Id];

                Assert.AreEqual(def.TituloKey, Campo(json, "titulo_key"), def.Id + ": titulo_key divergente");
                Assert.AreEqual(def.Tipo.ToString().ToLowerInvariant(), Campo(json, "tipo"), def.Id + ": tipo divergente");
                Assert.AreEqual(def.Central, Booleano(json, "central"), def.Id + ": central/opcional divergente");

                CollectionAssert.AreEquivalent(def.PreMissoes, ListaDeTexto(json, "missoes_concluidas"),
                    def.Id + ": pre-missoes divergentes");

                CollectionAssert.AreEqual(IdsDeObjetivo(def), IdsDoBloco(json, "objetivos"),
                    def.Id + ": ids (ou ordem) de objetivo divergentes");

                CollectionAssert.AreEquivalent(IdsDeRecompensa(def), IdsDoBloco(json, "recompensas", "id_transacao"),
                    def.Id + ": ids de transacao de recompensa divergentes");

                // O alvo de marco vira evento no historico (roteiro e NPC citam): tipo e alvo tambem tem de bater.
                var recNoCodigo = new List<string>();
                foreach (RecompensaDef r in def.Recompensas) recNoCodigo.Add(r.Id + "|" + r.Tipo + "|" + r.Alvo);
                var recNoJson = new List<string>();
                foreach (Match m in Regex.Matches(Bloco(json, "recompensas"),
                    "\"id_transacao\"\\s*:\\s*\"([^\"]*)\"\\s*,\\s*\"tipo\"\\s*:\\s*\"([^\"]*)\"\\s*,\\s*\"alvo\"\\s*:\\s*\"([^\"]*)\""))
                    recNoJson.Add(m.Groups[1].Value + "|" + m.Groups[2].Value + "|" + m.Groups[3].Value);
                CollectionAssert.AreEquivalent(recNoCodigo, recNoJson, def.Id + ": tipo/alvo de recompensa divergentes");

                CollectionAssert.AreEquivalent(def.PreEventos, ListaDeTexto(json, "eventos_de_vida"),
                    def.Id + ": pre-eventos (eventos_de_vida) divergentes");
                Assert.AreEqual(def.EventoDeConclusao, Campo(json, "evento_de_conclusao"),
                    def.Id + ": evento_de_conclusao divergente");
                CollectionAssert.AreEquivalent(def.Desfechos, ListaDeTexto(json, "desfechos"),
                    def.Id + ": desfechos divergentes");

                // O JSON diz o que a missao grava; o codigo e quem grava. Mesma lista, senao NPC lembra (ou
                // esquece) um fato que o roteiro nao previu.
                var gravados = new List<string> { def.EventoDeConclusao };
                gravados.AddRange(def.EventosAoConcluir);
                gravados.AddRange(def.Desfechos);
                CollectionAssert.AreEquivalent(gravados, IdsDoBloco(json, "registra_no_historico", "evento"),
                    def.Id + ": registra_no_historico divergente do que o QuestSystem grava");

                // "<missao>.<objetivo>": sem campo no codigo porque Concluida ja implica todos os objetivos.
                foreach (string req in ListaDeTexto(json, "objetivos_concluidos"))
                {
                    string[] partes = req.Split('.');
                    Assert.AreEqual(2, partes.Length, def.Id + ": objetivos_concluidos fora do formato: " + req);
                    Assert.Contains(partes[0], def.PreMissoes,
                        def.Id + ": exige objetivo de " + partes[0] + " sem exigir a missao concluida");
                    QuestDef pre = QuestCatalog.Missao(partes[0]);
                    Assert.IsTrue(pre != null && pre.Objetivo(partes[1]) != null, def.Id + ": objetivo inexistente " + req);
                }
            }
        }

        [Test]
        public void FlagsDoJsonSaoAsDoCatalogo()
        {
            var doJson = new List<string>();
            foreach (string json in LerJsons().Values)
                foreach (Match e in Regex.Matches(Bloco(json, "registra_no_historico"), "\\{[^{}]*\\}"))
                    foreach (string flag in ListaDeTexto(e.Value, "flags"))
                        doJson.Add(flag + " = " + Campo(e.Value, "evento"));

            var doCodigo = new List<string>();
            foreach (string[] par in QuestCatalog.Flags) doCodigo.Add(par[0] + " = " + par[1]);

            CollectionAssert.AreEquivalent(doCodigo, doJson, "flag = evento divergente entre QuestCatalog.Flags e content/quests");
        }

        /// <summary>T012: o que o mundo aciona sozinho. inicio_automatico = MissaoMundo.Automaticas; objetivo com
        /// "automatico": true = MissaoMundo.ObjetivosAutomaticos (ADR-0007 §6, sem gatilho); todo outro objetivo com
        /// npcs vazio = um gatilho em MissaoMundo.Gatilhos na MESMA ancora. Objetivo com NPC e do dialogo.</summary>
        [Test]
        public void InicioAutomaticoEGatilhosDeAncora_BatemComOJson()
        {
            var automaticas = new List<string>();
            var gatilhos = new List<string>();
            var objetivosAutomaticos = new List<string>();
            foreach (KeyValuePair<string, string> par in LerJsons())
            {
                if (Booleano(par.Value, "inicio_automatico")) automaticas.Add(par.Key);
                foreach (Match o in Regex.Matches(Bloco(par.Value, "objetivos"), "\\{[^{}]*\\}"))
                {
                    bool semNpc = ListaDeTexto(o.Value, "npcs").Count == 0;
                    if (Booleano(o.Value, "automatico"))
                    {
                        Assert.IsTrue(semNpc, par.Key + "." + Campo(o.Value, "id") + ": objetivo automatico nao pode pedir NPC");
                        objetivosAutomaticos.Add(par.Key + "|" + Campo(o.Value, "id"));
                    }
                    else if (semNpc)
                        gatilhos.Add(par.Key + "|" + Campo(o.Value, "id") + "|" + Campo(o.Value, "ancora"));
                }
            }

            var doCodigo = new List<string>();
            foreach (string[] g in MissaoMundo.Gatilhos) doCodigo.Add(g[0] + "|" + g[1] + "|" + g[2]);
            var autoNoCodigo = new List<string>();
            foreach (string[] a in MissaoMundo.ObjetivosAutomaticos) autoNoCodigo.Add(a[0] + "|" + a[1]);
            CollectionAssert.AreEquivalent(autoNoCodigo, objetivosAutomaticos, "\"automatico\": true divergente de MissaoMundo.ObjetivosAutomaticos");

            CollectionAssert.AreEquivalent(MissaoMundo.Automaticas, automaticas, "inicio_automatico divergente de MissaoMundo.Automaticas");
            CollectionAssert.AreEquivalent(doCodigo, gatilhos, "objetivo sem NPC divergente de MissaoMundo.Gatilhos");
        }

        [Test]
        public void TodoJsonTemMissaoNoCatalogo()
        {
            foreach (KeyValuePair<string, string> par in LerJsons())
                Assert.IsNotNull(QuestCatalog.Missao(par.Key), "JSON sem missao no catalogo: " + par.Key);
        }

        static List<string> IdsDeObjetivo(QuestDef def)
        {
            var ids = new List<string>();
            foreach (ObjetivoDef o in def.Objetivos) ids.Add(o.Id);
            return ids;
        }

        static List<string> IdsDeRecompensa(QuestDef def)
        {
            var ids = new List<string>();
            foreach (RecompensaDef r in def.Recompensas) ids.Add(r.Id);
            return ids;
        }
    }
}
