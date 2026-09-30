using UnityEngine;

namespace COE
{
    /// <summary>Carrega Assets/_COE/Resources/strings.&lt;idioma&gt;.json em Strings uma vez por sessao.
    /// Resources.Load e o mesmo caminho no editor, no PC e dentro do APK (no Android o StreamingAssets fica dentro do
    /// .apk, onde File.ReadAllText nao chega). O arquivo e versionado como esta: sem etapa de sincronizacao.
    /// ponytail: um arquivo por idioma, inteiro na memoria. Addressables ou o pacote Localization so se os textos
    /// crescerem (varios idiomas, download separado ou centenas de KB).</summary>
    public static class StringsLoader
    {
        public const string DefaultLanguage = "pt-BR"; // idioma-fonte
        static bool loaded;

        public static void EnsureLoaded() { if (!loaded) Load(DefaultLanguage); }

        public static bool Load(string language)
        {
            loaded = true;
            string nome = "strings." + language;   // Resources.Load quer o caminho sem a extensao .json
            TextAsset arquivo = Resources.Load<TextAsset>(nome);
            bool ok = arquivo != null && Strings.Load(arquivo.text);
            if (!ok) Debug.LogWarning("StringsLoader: Resources/" + nome + ".json ausente ou invalido; a tela mostra [chave].");
            return ok;
        }
    }
}
