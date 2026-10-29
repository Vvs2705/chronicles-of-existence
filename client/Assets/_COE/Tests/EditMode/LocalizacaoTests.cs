using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;

namespace COE.Tests
{
    /// <summary>Idiomas (Prompt Mestre §1: pt-BR, EN, ES, ID) e fontes OFL (ART_BIBLE §5). Cada idioma traduz toda chave do
    /// pt-BR com os mesmos marcadores {n}; o escolhido se mescla por cima do pt-BR; `-lang` vence o idioma do aparelho.</summary>
    public class LocalizacaoTests
    {
        static readonly Regex Marcador = new Regex(@"\{\d+\}");

        [TearDown] public void Limpar() { Strings.Load(null); }

        /// <summary>A tabela de UM arquivo (sem o pt-BR por baixo).</summary>
        static Dictionary<string, string> Tabela(string idioma)
        {
            TextAsset a = Resources.Load<TextAsset>("strings." + idioma);
            Assert.IsNotNull(a, "falta Resources/strings." + idioma + ".json");
            Assert.IsTrue(Strings.Load(a.text), idioma + ": JSON sem objeto strings");
            var t = new Dictionary<string, string>();
            foreach (string k in Strings.Chaves) t[k] = Strings.Get(k);
            return t;
        }

        static string Marcadores(string s)
        {
            var l = new List<string>();
            foreach (Match m in Marcador.Matches(s)) l.Add(m.Value);
            l.Sort();
            return string.Join(",", l);
        }

        [Test]
        public void CadaIdioma_TraduzTodaChaveDoPtBR_ComOsMesmosMarcadores()
        {
            Dictionary<string, string> pt = Tabela(StringsLoader.DefaultLanguage);
            foreach (string idioma in StringsLoader.Idiomas)
            {
                if (idioma == StringsLoader.DefaultLanguage) continue;
                Dictionary<string, string> t = Tabela(idioma);
                CollectionAssert.AreEquivalent(pt.Keys, t.Keys, idioma + ": as mesmas chaves do pt-BR");
                int iguais = 0;
                foreach (KeyValuePair<string, string> p in pt)
                {
                    Assert.AreEqual(Marcadores(p.Value), Marcadores(t[p.Key]), idioma + "/" + p.Key + ": marcadores {n}");
                    if (t[p.Key] == p.Value) iguais++;
                }
                Assert.Less(iguais, pt.Count / 5, idioma + ": texto demais igual ao pt-BR (sem traducao?)");   // nomes e cognatos (es: ~11%)
            }
        }

        /// <summary>ADR-0004 em todo idioma: destino e circunstancia de nascimento, nunca nivel de dificuldade.</summary>
        [Test]
        public void Destinos_NaoViramDificuldade_EmNenhumIdioma()
        {
            var palavras = new Dictionary<string, string>
            {
                { "en", @"\b(easy|hard|difficult\w*|extreme)\b" },
                { "es", @"\b(f[aá]cil|dif[ií]cil|dificultad|extrem\w*)\b" },
                { "id", @"\b(mudah|sulit|kesulitan|ekstrem\w*)\b" },
            };
            foreach (KeyValuePair<string, string> p in palavras)
            {
                Tabela(p.Key);
                foreach (DestinyDef d in DestinyCatalog.Destinos)
                    foreach (string k in new[] { d.RotuloKey, d.DescricaoKey, d.FamiliaKey, d.ContextoSocialKey })
                        Assert.IsFalse(Regex.IsMatch(Strings.Get(k), p.Value, RegexOptions.IgnoreCase), p.Key + "/" + k + ": " + Strings.Get(k));
            }
        }

        [Test]
        public void Idioma_LangVence_SenaoOAparelho_SenaoIngles()
        {
            Assert.AreEqual("es", StringsLoader.Escolher("es", SystemLanguage.Portuguese), "-lang vence o aparelho");
            Assert.AreEqual("id", StringsLoader.Escolher("ID", SystemLanguage.English), "sem diferenca de caixa");
            Assert.AreEqual("pt-BR", StringsLoader.Escolher("pt", SystemLanguage.English));
            Assert.AreEqual("pt-BR", StringsLoader.Escolher(null, SystemLanguage.Portuguese));
            Assert.AreEqual("es", StringsLoader.Escolher("", SystemLanguage.Spanish));
            Assert.AreEqual("id", StringsLoader.Escolher(null, SystemLanguage.Indonesian));
            Assert.AreEqual("en", StringsLoader.Escolher("klingon", SystemLanguage.Japanese), "fora dos quatro: ingles");
        }

        [Test]
        public void ChaveSemTraducao_MostraOPtBR_ENaoAChave()
        {
            Assert.IsTrue(Strings.Load("{\"strings\": {\"a\": \"um\", \"b\": \"dois\"}}"));
            Assert.IsTrue(Strings.Mesclar("{\"strings\": {\"a\": \"one\"}}"));
            Assert.AreEqual("one", Strings.Get("a"));
            Assert.AreEqual("dois", Strings.Get("b"), "sem traducao, fica o pt-BR");
            Assert.IsFalse(Strings.Mesclar("sem json"), "lixo nao mexe na tabela");
            Assert.AreEqual("one", Strings.Get("a"));
        }

        [Test]
        public void Load_DeOutroIdioma_FicaPorCimaDoPtBR()
        {
            Assert.IsTrue(StringsLoader.Load(StringsLoader.DefaultLanguage));
            string moedasPt = Strings.Get("hud.moedas");
            foreach (string idioma in new[] { "en", "es", "id" })
            {
                Assert.IsTrue(StringsLoader.Load(idioma), idioma);
                Assert.AreEqual(idioma, StringsLoader.Atual);
                Assert.AreNotEqual(moedasPt, Strings.Get("hud.moedas"), idioma + ": a HUD traduzida");
            }
            Assert.IsFalse(StringsLoader.Load("xx"), "idioma sem arquivo");
            Assert.AreEqual(StringsLoader.DefaultLanguage, StringsLoader.Atual, "fica o pt-BR");
            Assert.AreEqual(moedasPt, Strings.Get("hud.moedas"));
        }

        /// <summary>ART_BIBLE §5: Nunito no texto, Cinzel nos titulos, com a licenca OFL ao lado do arquivo.</summary>
        [Test]
        public void Fontes_OFL_NoResources_ComALicencaAoLado()
        {
            foreach (string c in new[] { Tela.CaminhoTexto, Tela.CaminhoTitulo }) Assert.IsNotNull(Resources.Load<Font>(c), c);
            Assert.AreEqual("Nunito-Regular", Tela.FontePadrao.name);
            Assert.AreEqual("Cinzel-Bold", Tela.FonteTitulo.name);
            foreach (string l in new[] { "Fontes/OFL-Nunito", "Fontes/OFL-Cinzel" })
            {
                TextAsset t = Resources.Load<TextAsset>(l);
                Assert.IsNotNull(t, l);
                StringAssert.Contains("SIL Open Font License", t.text, l);
            }
        }
    }
}
