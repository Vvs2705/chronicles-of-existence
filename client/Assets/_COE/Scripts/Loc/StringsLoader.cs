using UnityEngine;

namespace COE
{
    /// <summary>Carrega Assets/_COE/Resources/strings.&lt;idioma&gt;.json em Strings uma vez por sessao.
    /// Resources.Load e o mesmo caminho no editor, no PC e dentro do APK (no Android o StreamingAssets fica dentro do
    /// .apk, onde File.ReadAllText nao chega). O arquivo e versionado como esta: sem etapa de sincronizacao.
    ///
    /// IDIOMAS (Prompt Mestre §1: pt-BR, EN, ES, ID): `-lang en|es|id|pt-BR` na linha de comando (ou no extra da intent no
    /// Android, DevSceneArg) vence; sem ela, o idioma do aparelho; idioma fora dos quatro abre em ingles. O pt-BR carrega
    /// sempre primeiro e o escolhido se mescla por cima (Strings.Mesclar): chave sem traducao aparece em pt-BR.
    /// ponytail: um arquivo por idioma, inteiro na memoria, sem troca de idioma dentro do jogo (vem do aparelho).
    /// Opcao no menu, Addressables ou o pacote Localization so se o playtest ou o tamanho pedirem.</summary>
    public static class StringsLoader
    {
        public const string DefaultLanguage = "pt-BR"; // idioma-fonte
        public static readonly string[] Idiomas = { DefaultLanguage, "en", "es", "id" };
        static bool loaded;

        /// <summary>O idioma que esta na tela (o pedido, ou o pt-BR se o arquivo dele faltou).</summary>
        public static string Atual { get; private set; }

        public static void EnsureLoaded() { if (!loaded) Load(Escolher(DevSceneArg.Valor("-lang"), Application.systemLanguage)); }

        /// <summary>Qual idioma abrir: o pedido (`-lang`), se for um dos quatro; senao o do aparelho; senao ingles.</summary>
        public static string Escolher(string pedido, SystemLanguage aparelho)
        {
            if (!string.IsNullOrEmpty(pedido))
            {
                if (pedido.Equals("pt", System.StringComparison.OrdinalIgnoreCase)) return DefaultLanguage;
                foreach (string i in Idiomas) if (i.Equals(pedido, System.StringComparison.OrdinalIgnoreCase)) return i;
            }
            switch (aparelho)
            {
                case SystemLanguage.Portuguese: return DefaultLanguage;
                case SystemLanguage.Spanish: return "es";
                case SystemLanguage.Indonesian: return "id";
                default: return "en";
            }
        }

        /// <summary>pt-BR inteiro e, se `language` for outro, ele por cima. true = o idioma pedido esta na tela.</summary>
        public static bool Load(string language)
        {
            loaded = true;
            Atual = DefaultLanguage;
            bool ok = Ler(DefaultLanguage, true);
            if (string.IsNullOrEmpty(language) || language == DefaultLanguage) return ok;
            if (!Ler(language, false)) return false;
            Atual = language;
            return ok;
        }

        static bool Ler(string language, bool substituir)
        {
            string nome = "strings." + language;   // Resources.Load quer o caminho sem a extensao .json
            TextAsset arquivo = Resources.Load<TextAsset>(nome);
            bool ok = arquivo != null && (substituir ? Strings.Load(arquivo.text) : Strings.Mesclar(arquivo.text));
            if (!ok) Debug.LogWarning("StringsLoader: Resources/" + nome + ".json ausente ou invalido" + (substituir ? "; a tela mostra [chave]." : "; fica o pt-BR."));
            return ok;
        }
    }
}
