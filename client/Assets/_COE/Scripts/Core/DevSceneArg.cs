using UnityEngine;
using UnityEngine.SceneManagement;

namespace COE
{
    /// <summary>`COE.exe -scene Auren` abre a build direto naquela cena, em vez da primeira do Build Settings.
    /// Existe para conferencia: sem isso, so da para ver a cena inicial, e verificar Auren exigiria mexer na
    /// ordem do Build Settings (que e a ordem de verdade do jogo) so para tirar uma foto.
    /// ponytail: sem menu nem selecao em jogo; quando existir MainMenu (backlog), isto some.</summary>
    public static class DevSceneArg
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Aplicar()
        {
            string[] args = System.Environment.GetCommandLineArgs();
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
    }
}
