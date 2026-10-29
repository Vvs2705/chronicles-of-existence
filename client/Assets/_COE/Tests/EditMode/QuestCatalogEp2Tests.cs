using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;

namespace COE.Tests
{
    /// <summary>Episodio 2 (Quest/QuestCatalogEp2.cs): a q09 so abre com a compra E o gancho do ep. 1, nenhum id repete o
    /// do episodio 1, o catalogo bate com content/quests/episodio2/*.json e todo texto existe no pt-BR.
    /// ponytail: a paridade aqui cobre id, titulo, tipo, central, pre-condicoes, objetivos, recompensas, evento e flags,
    /// com a mesma regex de QuestDataParityTests. Quando o ep. 2 passar de 3 missoes, unificar os dois testes.</summary>
    public class QuestCatalogEp2Tests
    {
        static SaveData Save(bool comCompra, bool comGancho)
        {
            var s = new SaveData();
            if (comCompra) Compras.Conceder(s, Compras.Episodio2);
            if (comGancho) new LifeEventHistory(s).Registrar(GameSession.MarcoGancho, LifeEventCategoria.Marco, 8);
            return s;
        }

        [Test]
        public void Liberado_SoComACompraEOGancho()
        {
            Assert.IsFalse(QuestCatalogEp2.Liberado(null), "save null le como fechado");
            Assert.IsFalse(QuestCatalogEp2.Liberado(Save(false, false)));
            Assert.IsFalse(QuestCatalogEp2.Liberado(Save(false, true)), "sem a compra: segue em Auren aos 8, nada aparece");
            Assert.IsFalse(QuestCatalogEp2.Liberado(Save(true, false)), "comprou no menu antes do fim do ep. 1: espera o gancho");
            Assert.IsTrue(QuestCatalogEp2.Liberado(Save(true, true)));

            SaveData depois = Save(false, true);
            Compras.Conceder(depois, Compras.Episodio2);
            Assert.IsTrue(QuestCatalogEp2.Liberado(depois), "comprar depois abre sem reiniciar o save");
        }

        [Test]
        public void Episodio2_NaoEntraNoCatalogoDoEpisodio1()
        {
            foreach (QuestDef def in QuestCatalogEp2.Missoes)
                Assert.IsNull(QuestCatalog.Missao(def.Id), def.Id + ": o QuestSystem do ep. 1 nao pode ver o ep. 2");
            Assert.AreEqual(Compras.Episodio2, QuestCatalogEp2.Direito);
        }

        [Test]
        public void NenhumIdRepeteOEpisodio1()
        {
            var ep1 = new HashSet<string>();
            foreach (QuestDef d in QuestCatalog.Missoes) ep1.UnionWith(Ids(d));
            foreach (string[] f in QuestCatalog.Flags) ep1.Add("flag:" + f[0]);

            var ep2 = new HashSet<string>();
            foreach (QuestDef d in QuestCatalogEp2.Missoes)
                foreach (string id in Ids(d))
                {
                    Assert.IsFalse(ep1.Contains(id), "id do ep. 2 repete o do ep. 1: " + id);
                    Assert.IsTrue(ep2.Add(id), "id repetido dentro do ep. 2: " + id);
                }
            foreach (string[] f in QuestCatalogEp2.Flags)
                Assert.IsFalse(ep1.Contains("flag:" + f[0]), "flag do ep. 2 repete a do ep. 1: " + f[0]);
        }

        /// <summary>O que uma missao grava ou paga: o id dela, o de cada recompensa e o de cada evento.</summary>
        static List<string> Ids(QuestDef d)
        {
            var l = new List<string> { d.Id, d.EventoDeConclusao };
            foreach (RecompensaDef r in d.Recompensas) l.Add(r.Id);
            l.AddRange(d.EventosAoConcluir);
            l.AddRange(d.Desfechos);
            return l;
        }

        [Test]
        public void PreRequisitosApontamParaOQueExiste_ECentralNaoDependeDeOpcional()
        {
            foreach (QuestDef d in QuestCatalogEp2.Missoes)
                foreach (string pre in d.PreMissoes)
                {
                    QuestDef p = QuestCatalog.Missao(pre) ?? QuestCatalogEp2.Missao(pre);
                    Assert.IsNotNull(p, d.Id + ": pre-missao inexistente " + pre);
                    if (d.Central) Assert.IsTrue(p.Central, d.Id + " (central) depende da opcional " + pre);
                }
        }

        [Test]
        public void Encadeamento_AQ09AbreOAtoI_EOAtoIIEUmaFila()
        {
            var pre = new Dictionary<string, string>
            {
                { "q09_a_linha_do_posto", "q08_ecos_do_limiar" },
                { "q10_a_lousa_e_o_livro", "q09_a_linha_do_posto" },
                { "q11_o_aro_de_provas", "q09_a_linha_do_posto" },
                { "q12_as_ervas_que_curam", "q09_a_linha_do_posto" },
                { "q13_o_fio_do_nilo", "q10_a_lousa_e_o_livro" },
                { "q14_a_festa_da_colheita", "q13_o_fio_do_nilo" },
                { "q15_a_anomalia_no_bosque", "q14_a_festa_da_colheita" },
                { "q16_o_enredado", "q15_a_anomalia_no_bosque" },
            };
            Assert.AreEqual(pre.Count, QuestCatalogEp2.Missoes.Length, "episodio 2: q09 a q16");
            foreach (QuestDef d in QuestCatalogEp2.Missoes)
                CollectionAssert.AreEqual(new[] { pre[d.Id] }, d.PreMissoes, d.Id + ": pre-missao");

            int opcionais = 0;
            foreach (QuestDef d in QuestCatalogEp2.Missoes) if (!d.Central) opcionais++;
            Assert.AreEqual(2, opcionais, "seis centrais e duas opcionais (secao 3)");

            foreach (string id in new[] { "q11_o_aro_de_provas", "q13_o_fio_do_nilo" })
            {
                QuestDef q = QuestCatalogEp2.Missao(id);
                Assert.IsNotNull(q.Objetivo(q.ObjetivoDoDesfecho), id + ": o objetivo do desfecho existe");
                Assert.AreEqual(2, q.Desfechos.Length, id + ": dois desfechos");
            }

            string salto = null;
            foreach (string[] f in QuestCatalogEp2.Flags) if (f[0] == "salto_onze_liberado") salto = f[1];
            Assert.AreEqual(QuestCatalogEp2.Missao("q10_a_lousa_e_o_livro").EventoDeConclusao, salto);
            CollectionAssert.Contains(QuestCatalogEp2.Missao("q14_a_festa_da_colheita").EventosAoConcluir,
                QuestCatalogEp2.EventoAnomaliaDespertou, "o alarme toca para todo destino");
        }

        [Test]
        public void TodoTextoDoEpisodio2_ExisteNoPtBR()
        {
            TextAsset a = Resources.Load<TextAsset>("strings." + StringsLoader.DefaultLanguage);
            Assert.IsNotNull(a);
            try
            {
                Assert.IsTrue(Strings.Load(a.text));
                var chaves = new HashSet<string>(Strings.Chaves);
                foreach (QuestDef d in QuestCatalogEp2.Missoes)
                {
                    Assert.IsTrue(chaves.Contains(d.TituloKey), "falta " + d.TituloKey);
                    foreach (ObjetivoDef o in d.Objetivos) Assert.IsTrue(chaves.Contains(o.TextoKey), "falta " + o.TextoKey);
                }
            }
            finally { Strings.Load(null); }
        }

        // ---- paridade com content/quests/episodio2/*.json ----

        static string Campo(string json, string nome)
        {
            Match m = Regex.Match(json, "\"" + nome + "\"\\s*:\\s*\"([^\"]*)\"");
            return m.Success ? m.Groups[1].Value : null;
        }

        static string Bloco(string json, string bloco)
        {
            Match b = Regex.Match(json, "\"" + bloco + "\"\\s*:\\s*\\[(.*?)\\n\\s*\\]", RegexOptions.Singleline);
            return b.Success ? b.Groups[1].Value : "";
        }

        static List<string> IdsDoBloco(string json, string bloco, string chave = "id")
        {
            var ids = new List<string>();
            foreach (Match m in Regex.Matches(Bloco(json, bloco), "\"" + chave + "\"\\s*:\\s*\"([^\"]*)\""))
                ids.Add(m.Groups[1].Value);
            return ids;
        }

        static List<string> ListaDeTexto(string json, string bloco)
        {
            var itens = new List<string>();
            Match b = Regex.Match(json, "\"" + bloco + "\"\\s*:\\s*\\[(.*?)\\]", RegexOptions.Singleline);
            if (!b.Success) return itens;
            foreach (Match m in Regex.Matches(b.Groups[1].Value, "\"([^\"]+)\"")) itens.Add(m.Groups[1].Value);
            return itens;
        }

        static Dictionary<string, string> LerJsons()
        {
            // Assets/ -> client/ -> raiz do repositorio
            string raiz = Path.GetFullPath(Path.Combine(Application.dataPath, "..", ".."));
            string dir = Path.Combine(raiz, "content", "quests", "episodio2");
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
        public void CatalogoEJsonDoEpisodio2DescrevemAsMesmasMissoes()
        {
            Dictionary<string, string> jsons = LerJsons();
            Assert.AreEqual(QuestCatalogEp2.Missoes.Length, jsons.Count, "quantidade de missoes do ep. 2 diferente");

            foreach (QuestDef def in QuestCatalogEp2.Missoes)
            {
                Assert.IsTrue(jsons.ContainsKey(def.Id), "missao do catalogo sem JSON: " + def.Id);
                string json = jsons[def.Id];

                Assert.AreEqual(QuestCatalogEp2.Direito, Campo(json, "direito_de_loja"), def.Id + ": direito_de_loja");
                Assert.AreEqual(def.TituloKey, Campo(json, "titulo_key"), def.Id + ": titulo_key");
                Assert.AreEqual(def.Tipo.ToString().ToLowerInvariant(), Campo(json, "tipo"), def.Id + ": tipo");
                Assert.AreEqual(def.Central, Regex.IsMatch(json, "\"central\"\\s*:\\s*true"), def.Id + ": central");
                CollectionAssert.AreEquivalent(def.PreMissoes, ListaDeTexto(json, "missoes_concluidas"), def.Id + ": pre-missoes");
                CollectionAssert.AreEquivalent(def.PreEventos, ListaDeTexto(json, "eventos_de_vida"), def.Id + ": eventos_de_vida");

                var objetivos = new List<string>();
                foreach (ObjetivoDef o in def.Objetivos) objetivos.Add(o.Id);
                CollectionAssert.AreEqual(objetivos, IdsDoBloco(json, "objetivos"), def.Id + ": ids (ou ordem) de objetivo");

                var rec = new List<string>();
                foreach (RecompensaDef r in def.Recompensas) rec.Add(r.Id + "|" + r.Tipo + "|" + r.Alvo);
                var recJson = new List<string>();
                foreach (Match m in Regex.Matches(Bloco(json, "recompensas"),
                    "\"id_transacao\"\\s*:\\s*\"([^\"]*)\"\\s*,\\s*\"tipo\"\\s*:\\s*\"([^\"]*)\"\\s*,\\s*\"alvo\"\\s*:\\s*\"([^\"]*)\""))
                    recJson.Add(m.Groups[1].Value + "|" + m.Groups[2].Value + "|" + m.Groups[3].Value);
                CollectionAssert.AreEquivalent(rec, recJson, def.Id + ": recompensas");

                Assert.AreEqual(def.EventoDeConclusao, Campo(json, "evento_de_conclusao"), def.Id + ": evento_de_conclusao");
                CollectionAssert.AreEquivalent(def.Desfechos, ListaDeTexto(json, "desfechos"), def.Id + ": desfechos");
                Assert.AreEqual(def.ObjetivoDoDesfecho, Campo(json, "desfecho_no_objetivo"), def.Id + ": desfecho_no_objetivo");
                var gravados = new List<string> { def.EventoDeConclusao };
                gravados.AddRange(def.EventosAoConcluir);
                gravados.AddRange(def.Desfechos);
                CollectionAssert.AreEquivalent(gravados, IdsDoBloco(json, "registra_no_historico", "evento"),
                    def.Id + ": registra_no_historico");
            }

            var flagsJson = new List<string>();
            foreach (string json in jsons.Values)
                foreach (Match e in Regex.Matches(Bloco(json, "registra_no_historico"), "\\{[^{}]*\\}"))
                    foreach (string flag in ListaDeTexto(e.Value, "flags"))
                        flagsJson.Add(flag + " = " + Campo(e.Value, "evento"));
            var flagsCodigo = new List<string>();
            foreach (string[] par in QuestCatalogEp2.Flags) flagsCodigo.Add(par[0] + " = " + par[1]);
            CollectionAssert.AreEquivalent(flagsCodigo, flagsJson, "flag = evento divergente no ep. 2");
        }
    }
}
