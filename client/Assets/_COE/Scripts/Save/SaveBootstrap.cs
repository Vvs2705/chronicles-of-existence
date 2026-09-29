using UnityEngine;

namespace COE
{
    /// <summary>A unica instancia viva do save na partida e o UNICO ponto de escrita em disco.
    /// API para o resto do jogo, inteira: SaveState.Current (ler/mudar), SaveState.Load(), SaveState.Commit().
    /// Nenhum sistema serializa nada por conta propria nem chama LocalSave direto — quem quiser persistir algo
    /// muda um campo de Current e pede Commit.</summary>
    public static class SaveState
    {
        public static SaveData Current = new SaveData();

        public static void Load() { Current = LocalSave.Load(); }

        public static void Commit() { LocalSave.Save(Current); }
    }

    /// <summary>Carrega o save antes de tudo na cena (-200: antes do PlayerInputReader -100) e grava ao pausar/sair.
    /// ponytail: um slot so. Varios slots = escolher o path antes do Load; nada mais muda.</summary>
    [DefaultExecutionOrder(-200)]
    public class SaveBootstrap : MonoBehaviour
    {
        void Awake() { SaveState.Load(); }

        void OnApplicationPause(bool paused) { if (paused) SaveState.Commit(); }

        void OnApplicationQuit() { SaveState.Commit(); }
    }
}
