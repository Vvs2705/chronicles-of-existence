using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;

namespace COE.Tests
{
    /// <summary>O arquivo de textos (Resources/strings.pt-BR.json) cobre o que o codigo pede.
    /// Chave de SISTEMA (literal em Strings.Get/Format) e chave de CONTEUDO (a que um catalogo expoe a tela) tem o
    /// mesmo rigor desde a leva A: existe no arquivo, com texto de verdade. As listas de chave saem dos catalogos,
    /// nunca de uma copia fixa -- missao, NPC, destino ou no de dialogo novo sem texto quebra aqui, com o nome.</summary>
    public class StringsCoberturaTests
    {
        // Chaves literais em Strings.Get/Format sao de sistema. Sem excecao: toda chave de sistema tem de estar no arquivo.
        static readonly string[] SistemaPendente = { };

        // O que content/quests usa para "texto ainda nao escrito": nao pode chegar ao arquivo de textos.
        const string AEscrever = "[a escrever]";

        static readonly Regex Literal = new Regex("Strings\\.(?:Get|Format)\\(\\s*\"([^\"]+)\"\\s*[,)]");

        // ADR-0004 / B02: destino e circunstancia de nascimento, nunca nivel de desafio.
        static readonly Regex PalavraDeDificuldade = new Regex("f[aá]cil|dif[ií]cil|extrem", RegexOptions.IgnoreCase);

        [SetUp] public void Carregar() { Assert.IsTrue(StringsLoader.Load(StringsLoader.DefaultLanguage), "arquivo de textos"); }
        [TearDown] public void Limpar() { Strings.Load(null); }

        [Test]
        public void Resources_AchaOArquivo_EOJsonTemParesComAcento()
        {
            Assert.IsNotNull(Resources.Load<TextAsset>("strings." + StringsLoader.DefaultLanguage), "Assets/_COE/Resources");
            Assert.Greater(Strings.Count, 0);
            // ADR-0007 decisao 2: o id "dificil" nao muda, o rotulo exibido sim.
            Assert.AreEqual("Vida Árdua", Strings.Get("destino.dificil.rotulo"), "UTF-8 e acento intactos");
            // Formato invalido faz Strings.Format devolver o texto cru: aqui os placeholders precisam casar com o codigo.
            Assert.AreNotEqual(Strings.Get("perf.hud"), Strings.Format("perf.hud", 30f, 33.3f, 120L, 0.5f, "Charging", "n/d", 28f));
            Assert.AreNotEqual(Strings.Get("salto.idade"), Strings.Format("salto.idade", 5, 8));
            Assert.AreNotEqual(Strings.Get("salto.fase"), Strings.Format("salto.fase", "a", "b"));
        }

        [Test]
        public void ChavesDeSistema_EstaoNoArquivo()
        {
            var chaves = new List<string>();
            string scripts = Path.Combine(Application.dataPath, "_COE", "Scripts");
            foreach (string cs in Directory.GetFiles(scripts, "*.cs", SearchOption.AllDirectories))
                foreach (Match m in Literal.Matches(File.ReadAllText(cs)))
                    chaves.Add(m.Groups[1].Value);
            Assert.Greater(chaves.Count, 10, "a varredura achou as chaves literais do codigo");

            foreach (LifePhase f in Enum.GetValues(typeof(LifePhase))) chaves.Add("fase." + LifePhases.Id(f));   // SaltoHud

            Faltando(chaves, SistemaPendente);
        }

        // --- conteudo: gerado a partir dos catalogos ---

        /// <summary>Cartoes do nascimento (EntryFlow, B02/B03): rotulo + cada paragrafo.</summary>
        [Test]
        public void Destinos_EOrigens_TemTextoEscrito()
        {
            var chaves = new List<string>();
            foreach (DestinyDef d in DestinyCatalog.Destinos)
                chaves.AddRange(new[] { d.RotuloKey, d.DescricaoKey, d.FamiliaKey, d.ContextoSocialKey });
            foreach (OriginDef o in DestinyCatalog.Origens) chaves.AddRange(new[] { o.RotuloKey, o.DescricaoKey, o.OficioKey });
            Assert.AreEqual(4 * 4 + 3 * 3, chaves.Count, "4 destinos x 4 textos + 3 origens x 3 textos");
            Escrito(chaves);
        }

        /// <summary>ADR-0004 e ADR-0007 decisao 2: nenhum texto de destino (nem de origem) fala em facil/dificil/extremo.
        /// O id "dificil" continua existindo; o que o jogador le e a circunstancia.</summary>
        [Test]
        public void Destinos_NaoUsamPalavraDeDificuldade()
        {
            var chaves = new List<string>();
            foreach (DestinyDef d in DestinyCatalog.Destinos)
                chaves.AddRange(new[] { d.RotuloKey, d.DescricaoKey, d.FamiliaKey, d.ContextoSocialKey });
            foreach (OriginDef o in DestinyCatalog.Origens) chaves.AddRange(new[] { o.RotuloKey, o.DescricaoKey, o.OficioKey });

            foreach (string k in chaves)
                Assert.IsFalse(PalavraDeDificuldade.IsMatch(Strings.Get(k)), k + " rotula destino como dificuldade: " + Strings.Get(k));
        }

        /// <summary>MissaoHud, QuestTrigger, SaltoHud e os botoes de MissaoNaConversa: titulo, cada objetivo e cada
        /// desfecho de toda missao.</summary>
        [Test]
        public void Missoes_TemTituloObjetivosEDesfechosEscritos()
        {
            var chaves = new List<string>();
            foreach (QuestDef q in QuestCatalog.Missoes)
            {
                chaves.Add(q.TituloKey);
                foreach (ObjetivoDef o in q.Objetivos) chaves.Add(o.TextoKey);
                foreach (string ev in q.Desfechos) chaves.Add(MissaoNaConversa.ChaveDoDesfecho(ev));
            }
            Assert.Greater(chaves.Count, QuestCatalog.Missoes.Length, "toda missao tem pelo menos um objetivo");
            Escrito(chaves);
        }

        /// <summary>Toda fala de NPC e toda resposta do jogador do DialogueCatalog.</summary>
        [Test]
        public void Dialogos_TemTodaFalaETodaOpcaoEscritas()
        {
            var chaves = new List<string>();
            foreach (DialogueGraph g in DialogueCatalog.Grafos)
                foreach (DialogueNode no in g.Nos)
                {
                    chaves.Add(no.TextoKey);
                    foreach (DialogueOption op in no.Opcoes) chaves.Add(op.TextoKey);
                }
            chaves.Add("dialogo.opcao.despedir");   // a saida que o DialogueHud acrescenta em no terminal
            Escrito(chaves);
        }

        /// <summary>Dois nos do mesmo NPC com a mesma fala = variacao que o jogador nao ve. E o que sustenta o B08:
        /// promessa cumprida e promessa quebrada precisam ser LIDAS como diferentes, nao so ter ids diferentes.</summary>
        [Test]
        public void Dialogos_NosDoMesmoNpc_TemFalasDiferentes()
        {
            foreach (DialogueGraph g in DialogueCatalog.Grafos)
            {
                var vistas = new Dictionary<string, string>();
                foreach (DialogueNode no in g.Nos)
                {
                    string fala = Strings.Get(no.TextoKey);
                    string outro;
                    Assert.IsFalse(vistas.TryGetValue(fala, out outro), g.Id + ": " + no.Id + " repete a fala de " + outro);
                    vistas[fala] = no.Id;
                }
            }
        }

        /// <summary>Nome aparece no prompt do NpcActor. Papel, tracos, atividades e relacoes ainda nao tem tela, mas
        /// ja tem texto: a primeira tela que os exibir nao pode mostrar "[chave]".</summary>
        [Test]
        public void Npcs_TemNomePapelTracosAtividadesERelacoesEscritos()
        {
            var chaves = new List<string>();
            foreach (NpcDef n in NpcCatalog.Npcs)
            {
                chaves.Add(n.NomeKey);
                chaves.Add(n.PapelKey);
                chaves.AddRange(n.TracosKeys);
                foreach (RotinaEntrada e in n.Rotina) chaves.Add(e.AtividadeKey);
                foreach (Vinculo v in n.Vinculos) chaves.Add(v.RelacaoKey);
            }
            chaves.Add(Interrupcao.Conversa.AtividadeKey);   // o que o NPC "faz" enquanto conversa (NpcAgenda)
            Escrito(chaves);
        }

        // --- ajuda ---

        static void Faltando(List<string> chaves, string[] pendentes)
        {
            var faltam = new List<string>();
            foreach (string k in chaves)
                if (Strings.Get(k) == "[" + k + "]" && !Pendente(k, pendentes) && !faltam.Contains(k)) faltam.Add(k);
            Assert.IsEmpty(faltam, "fora de Resources/strings.pt-BR.json: " + string.Join(", ", faltam));
        }

        /// <summary>Toda chave existe no arquivo com texto de verdade: nem ausente ("[chave]"), nem em branco, nem o
        /// marcador "[a escrever]".</summary>
        static void Escrito(List<string> chaves)
        {
            var ruins = new List<string>();
            foreach (string k in chaves)
            {
                string v = Strings.Get(k);
                bool ruim = string.IsNullOrEmpty(k) || v == "[" + k + "]" || v.Trim().Length == 0 || v.Trim() == AEscrever;
                if (ruim && !ruins.Contains(k)) ruins.Add(k);
            }
            Assert.IsEmpty(ruins, "sem texto escrito em Resources/strings.pt-BR.json: " + string.Join(", ", ruins));
        }

        static bool Pendente(string chave, string[] pendentes)
        {
            foreach (string p in pendentes)
                if (Regex.IsMatch(chave, "^" + Regex.Escape(p).Replace("\\*", ".+") + "$")) return true;
            return false;
        }
    }
}
