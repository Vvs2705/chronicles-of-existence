using System.IO;
using UnityEngine;

namespace COE
{
    /// <summary>Carrega StreamingAssets/content/strings.&lt;idioma&gt;.json em Strings uma vez por sessao.
    /// ponytail: File.ReadAllText direto (COE e PC; nao ha o caminho de APK do Android). Trocar por Addressables
    /// se o conteudo crescer alem de alguns KB.</summary>
    public static class StringsLoader
    {
        public const string DefaultLanguage = "pt-BR"; // idioma-fonte
        static bool loaded;

        public static void EnsureLoaded() { if (!loaded) Load(DefaultLanguage); }

        public static bool Load(string language)
        {
            loaded = true;
            string path = Path.Combine(Application.streamingAssetsPath, "content", "strings." + language + ".json");
            bool ok = File.Exists(path) && Strings.Load(File.ReadAllText(path));
            if (!ok) Debug.LogWarning("StringsLoader: " + path + " ausente ou invalido; a tela mostra [chave].");
            return ok;
        }
    }
}
