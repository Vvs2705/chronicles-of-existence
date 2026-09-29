using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace COE
{
    /// <summary>Tabela de strings por chave semantica (GDD 06_TECNOLOGIA/03: texto exibido usa chave; pt-BR e a
    /// fonte). C# puro, sem Unity: StringsTests roda sem cena. Get nunca devolve vazio: chave
    /// ausente vira "[chave]" para aparecer na tela e ser corrigida.
    /// ponytail: JsonUtility nao le dicionario, entao um regex extrai os pares "k": "v" do objeto "strings"
    /// (mapa plano por schema). Trocar por parser real (Newtonsoft) se o formato ganhar aninhamento.</summary>
    public static class Strings
    {
        static readonly Dictionary<string, string> map = new Dictionary<string, string>();
        static readonly Regex Pair = new Regex("\"((?:[^\"\\\\]|\\\\.)*)\"\\s*:\\s*\"((?:[^\"\\\\]|\\\\.)*)\"", RegexOptions.Compiled);

        public static int Count { get { return map.Count; } }

        /// <summary>Substitui a tabela. false se o JSON nao tem objeto "strings" com pelo menos um par; nunca lanca.</summary>
        public static bool Load(string json)
        {
            map.Clear();
            if (string.IsNullOrEmpty(json)) return false;
            try
            {
                int i = json.IndexOf("\"strings\"", StringComparison.Ordinal);
                if (i < 0) return false;
                i = json.IndexOf('{', i);
                if (i < 0) return false;
                foreach (Match m in Pair.Matches(json, i))
                    map[Regex.Unescape(m.Groups[1].Value)] = Regex.Unescape(m.Groups[2].Value);
            }
            catch (Exception) { map.Clear(); return false; }
            return map.Count > 0;
        }

        public static string Get(string key)
        {
            string v;
            return key != null && map.TryGetValue(key, out v) && !string.IsNullOrEmpty(v) ? v : "[" + key + "]";
        }

        /// <summary>Get + string.Format invariante; formato invalido devolve o texto cru em vez de lancar.</summary>
        public static string Format(string key, params object[] args)
        {
            string s = Get(key);
            try { return string.Format(System.Globalization.CultureInfo.InvariantCulture, s, args); }
            catch (FormatException) { return s; }
        }
    }
}
