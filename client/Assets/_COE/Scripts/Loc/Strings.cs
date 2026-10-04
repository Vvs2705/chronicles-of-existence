using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace COE
{
    /// <summary>Tabela de strings por chave semantica (GDD 06_TECNOLOGIA/03: texto exibido usa chave; pt-BR e a
    /// fonte). C# puro, sem Unity: StringsTests roda sem cena. Get nunca devolve vazio: chave
    /// ausente vira "[chave]" para aparecer na tela e ser corrigida.
    /// ponytail: JsonUtility nao le dicionario, entao um regex extrai os pares "k": "v" de DENTRO do objeto "strings"
    /// (mapa plano por schema; o objeto e recortado ate a chave que o fecha, ciente de aspas e escapes). Escapes do JSON
    /// (\n, \", \\, \t, \/, \uXXXX) por Regex.Unescape. Chave repetida fica com a ultima: o check_json.py reprova no CI.
    /// Segundo idioma ou aninhamento = decidir o sistema de localizacao (gate do Prompt Mestre §23).</summary>
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
                int fim = FimDoObjeto(json, i);
                if (fim < 0) return false;   // objeto "strings" sem fechar: JSON truncado
                foreach (Match m in Pair.Matches(json.Substring(i, fim - i + 1)))
                    map[Regex.Unescape(m.Groups[1].Value)] = Regex.Unescape(m.Groups[2].Value);
            }
            catch (Exception) { map.Clear(); return false; }
            return map.Count > 0;
        }

        /// <summary>Indice da '}' que fecha o objeto aberto em `inicio`, ou -1. Chave dentro de texto ("{0}") nao conta.</summary>
        static int FimDoObjeto(string json, int inicio)
        {
            int nivel = 0;
            bool emTexto = false;
            for (int k = inicio; k < json.Length; k++)
            {
                char c = json[k];
                if (emTexto)
                {
                    if (c == '\\') k++;            // pula o caractere escapado (inclusive \")
                    else if (c == '"') emTexto = false;
                }
                else if (c == '"') emTexto = true;
                else if (c == '{') nivel++;
                else if (c == '}' && --nivel == 0) return k;
            }
            return -1;
        }

        public static string Get(string key)
        {
            string v;
            return key != null && map.TryGetValue(key, out v) && !string.IsNullOrEmpty(v) ? v : "[" + key + "]";
        }

        /// <summary>O texto de 'key' se estiver escrito; senao o de 'fallback' (que segue a regra do Get).</summary>
        public static string GetOu(string key, string fallback)
        {
            string v;
            return key != null && map.TryGetValue(key, out v) && !string.IsNullOrEmpty(v) ? v : Get(fallback);
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
