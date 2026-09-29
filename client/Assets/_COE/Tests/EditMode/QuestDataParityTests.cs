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
    /// no portao: comparou id, titulo, tipo, central/opcional, pre-missoes, ids de objetivo e ids de transacao
    /// de recompensa. Vermelho aqui = alguem mexeu num lado so.
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
            Match b = Regex.Match(json, "\"" + bloco + "\"\\s*:\\s*\\[(.*?)\\n\\s*\\]", RegexOptions.Singleline);
            if (!b.Success) return ids;
            foreach (Match m in Regex.Matches(b.Groups[1].Value, "\"" + chave + "\"\\s*:\\s*\"([^\"]*)\""))
                ids.Add(m.Groups[1].Value);
            return ids;
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
            }
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
