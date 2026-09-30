using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;

namespace COE.Tests
{
    /// <summary>O arquivo de textos (Resources/strings.pt-BR.json) cobre o que o codigo pede.
    /// Chave de SISTEMA ausente quebra aqui. Conteudo que depende de redacao (ADR-0002: fala, descricao e
    /// personalidade so depois do portao) fica numa lista EXPLICITA de pendentes: continua "[chave]" na tela, mas a
    /// lista e visivel e estreita, para nao esconder erro de digitacao.</summary>
    public class StringsCoberturaTests
    {
        // Chaves literais em Strings.Get/Format sao de sistema. Sem excecao: toda chave de sistema tem de estar no arquivo.
        static readonly string[] SistemaPendente = { };

        // Chaves que vem dos catalogos e ainda sao texto a escrever. "*" = qualquer trecho.
        static readonly string[] ConteudoPendente =
        {
            "destino.*.descricao", "destino.*.familia", "destino.*.contexto_social",   // cartoes do nascimento (B02)
            "origem.*.descricao", "origem.*.oficio",                                    // cartoes do nascimento (B03)
            "missao.*.obj.*",                                                           // "[a escrever]" em content/quests
            "dialogo.opcao.*",                                                          // falas do jogador e desfechos
            "dialogo.borin.*", "dialogo.lysa.*", "dialogo.nilo.*",                      // falas de NPC (DialogueCatalog)
        };

        static readonly Regex Literal = new Regex("Strings\\.(?:Get|Format)\\(\\s*\"([^\"]+)\"\\s*[,)]");

        [SetUp] public void Carregar() { Assert.IsTrue(StringsLoader.Load(StringsLoader.DefaultLanguage), "arquivo de textos"); }
        [TearDown] public void Limpar() { Strings.Load(null); }

        [Test]
        public void Resources_AchaOArquivo_EOJsonTemParesComAcento()
        {
            Assert.IsNotNull(Resources.Load<TextAsset>("strings." + StringsLoader.DefaultLanguage), "Assets/_COE/Resources");
            Assert.Greater(Strings.Count, 0);
            Assert.AreEqual("Vida Difícil", Strings.Get("destino.dificil.rotulo"), "UTF-8 e acento intactos");
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

        [Test]
        public void ChavesDosCatalogos_EstaoNoArquivo_OuSaoConteudoPendente()
        {
            var chaves = new List<string>();
            foreach (DestinyDef d in DestinyCatalog.Destinos)
                chaves.AddRange(new[] { d.RotuloKey, d.DescricaoKey, d.FamiliaKey, d.ContextoSocialKey });
            foreach (OriginDef o in DestinyCatalog.Origens) chaves.AddRange(new[] { o.RotuloKey, o.DescricaoKey, o.OficioKey });
            foreach (NpcDef n in NpcCatalog.Npcs) chaves.Add(n.NomeKey);
            foreach (QuestDef q in QuestCatalog.Missoes)
            {
                chaves.Add(q.TituloKey);
                foreach (ObjetivoDef o in q.Objetivos) chaves.Add(o.TextoKey);
                foreach (string ev in q.Desfechos) chaves.Add(MissaoNaConversa.ChaveDoDesfecho(ev));
            }
            foreach (DialogueGraph g in DialogueCatalog.Grafos)
                foreach (DialogueNode no in g.Nos)
                {
                    chaves.Add(no.TextoKey);
                    foreach (DialogueOption op in no.Opcoes) chaves.Add(op.TextoKey);
                }

            Faltando(chaves, ConteudoPendente);
        }

        static void Faltando(List<string> chaves, string[] pendentes)
        {
            var faltam = new List<string>();
            foreach (string k in chaves)
                if (Strings.Get(k) == "[" + k + "]" && !Pendente(k, pendentes) && !faltam.Contains(k)) faltam.Add(k);
            Assert.IsEmpty(faltam, "fora de Resources/strings.pt-BR.json: " + string.Join(", ", faltam));
        }

        static bool Pendente(string chave, string[] pendentes)
        {
            foreach (string p in pendentes)
                if (Regex.IsMatch(chave, "^" + Regex.Escape(p).Replace("\\*", ".+") + "$")) return true;
            return false;
        }
    }
}
