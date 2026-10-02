using UnityEngine;

namespace COE
{
    /// <summary>A unica instancia viva do save na partida e o UNICO ponto de escrita em disco.
    /// API para o resto do jogo, inteira: SaveState.Current (ler/mudar), SaveState.Load(), SaveState.Commit().
    /// Nenhum sistema serializa nada por conta propria nem chama LocalSave direto — quem quiser persistir algo
    /// muda um campo de Current e pede Commit.
    ///
    /// DUAS REGRAS DE FIACAO (contrato T005):
    /// 1. Load TROCA Current por um SaveData NOVO. Quem abriu LifeEventHistory, ledger ou QuestSystem sobre o Current
    ///    anterior tem de reabrir sobre o novo — senao grava num objeto que ninguem mais salva.
    /// 2. Commit = UMA gravacao atomica do SaveData inteiro. Fato do historico, quests, reputation, npcs e o efeito da
    ///    recompensa mudam antes do MESMO Commit: saem todos juntos ou nenhum sai (idempotencia da recompensa).
    ///
    /// TELA DE NASCIMENTO: abre quando Current.birth.destinyId esta vazio (sem save, save novo, v0 migrado) — NUNCA
    /// por confirmedAtUtc == 0, que um save editado a mao zera (achado da T003).</summary>
    public static class SaveState
    {
        public static SaveData Current = new SaveData();

        public static void Load() { Current = LocalSave.Load(); Carregado = true; }

        /// <summary>O save ja foi lido do disco neste processo. Trocar de cena NAO rele: o que esta em memoria e a
        /// partida (com save de versao mais nova o Commit e recusado, e reler apagaria o que a sessao fez).</summary>
        public static bool Carregado { get; private set; }

        // Play no Editor sem domain reload: a trava volta a zero a cada Play, como num processo novo.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void NovoProcesso() { Carregado = false; }

        /// <summary>Grava o save. Fora do Play (testes de editor, geradores de cena) nao toca o disco: um teste que
        /// avancava missao pela sessao gravava por cima do save.json de quem joga no mesmo PC (visto em 2026-10-02).</summary>
        public static void Commit() { if (Application.isPlaying) LocalSave.Save(Current); }

        static GameSession sessao;

        /// <summary>A partida aberta sobre o Current (T012). Reaberta sozinha quando Current troca (Load, ou teste que
        /// atribui Current): e a regra 1 acima aplicada por construcao. Quem tem cena usa isto para as transicoes.</summary>
        public static GameSession Sessao
        {
            get
            {
                if (sessao == null || sessao.Save != Current) sessao = new GameSession(Current, Commit);
                return sessao;
            }
        }
    }

    /// <summary>Carrega o save antes de tudo na primeira cena (-200: antes do PlayerInputReader -100) e grava ao pausar/sair.
    /// ponytail: um slot so. Varios slots = escolher o path antes do Load; nada mais muda.</summary>
    [DefaultExecutionOrder(-200)]
    public class SaveBootstrap : MonoBehaviour
    {
        void Awake() { if (!SaveState.Carregado) SaveState.Load(); }   // uma vez por processo, nao por cena

        void OnApplicationPause(bool paused) { if (paused) SaveState.Commit(); }

        void OnApplicationQuit() { SaveState.Commit(); }
    }
}
