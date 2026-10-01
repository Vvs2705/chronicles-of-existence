using UnityEngine;
using UnityEngine.SceneManagement;

namespace COE
{
    /// <summary>`COE.exe -scene Auren` abre a build direto naquela cena, em vez da primeira do Build Settings.
    /// Existe para conferencia: sem isso, so da para ver a cena inicial, e verificar Auren exigiria mexer na
    /// ordem do Build Settings (que e a ordem de verdade do jogo) so para tirar uma foto.
    /// ANDROID: a linha de comando chega pelo extra de intent "unity" (tools/run_android.ps1 -Scene Auren faz
    /// `am start -n br.com.vstack.coe/com.unity3d.player.UnityPlayerGameActivity -e unity "-scene Auren"`). A
    /// UnityPlayerGameActivity le esse extra (Source/.../UnityPlayerGameActivity.java do Unity 6000.3), mas nada
    /// garante que ele apareca em Environment.GetCommandLineArgs; por isso o extra e lido direto aqui.
    /// ponytail: sem menu nem selecao em jogo; quando existir MainMenu (backlog), isto some.</summary>
    public static class DevSceneArg
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Aplicar()
        {
            string[] args = Args();
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i] != "-scene") continue;
                string nome = args[i + 1];
                if (SceneManager.GetActiveScene().name == nome) return;
                if (Application.CanStreamedLevelBeLoaded(nome)) SceneManager.LoadScene(nome);
                else Debug.LogWarning("DevSceneArg: cena '" + nome + "' nao esta no Build Settings.");
                return;
            }
        }

        /// <summary>A flag veio na linha de comando (ou no extra "unity" do Android)? Ex.: `COE.exe -toque`.</summary>
        public static bool Tem(string flag) { return System.Array.IndexOf(Args(), flag) >= 0; }

        /// <summary>O valor depois da flag (`COE.exe -qualidade baixa` -> "baixa"), ou null.</summary>
        public static string Valor(string flag)
        {
            string[] args = Args();
            int i = System.Array.IndexOf(args, flag);
            return i >= 0 && i + 1 < args.Length ? args[i + 1] : null;
        }

        static string[] Args()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            // ponytail: A VALIDAR NO APARELHO. Le so o extra "unity" da intent que abriu o jogo; sem ele (abertura
            // pelo icone) cai na linha de comando normal.
            try
            {
                using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (var activity = player.GetStatic<AndroidJavaObject>("currentActivity"))
                using (var intent = activity.Call<AndroidJavaObject>("getIntent"))
                {
                    string extra = intent.Call<string>("getStringExtra", "unity");
                    if (!string.IsNullOrEmpty(extra))
                        return extra.Split((char[])null, System.StringSplitOptions.RemoveEmptyEntries);
                }
            }
            catch (System.Exception e) { Debug.LogWarning("DevSceneArg: extra 'unity' ilegivel: " + e.Message); }
#endif
            return System.Environment.GetCommandLineArgs();
        }
    }
}
