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

        /// <summary>"Nova vida" da tela de titulo: save em branco gravado por cima (o anterior fica no .bak do LocalSave).
        /// Unico lugar fora do Load que troca o Current; a sessao se reabre sozinha sobre o novo (regra 1).</summary>
        public static void NovaVida() { Current = new SaveData(); Commit(); }

        /// <summary>O save ja foi lido do disco neste processo. Trocar de cena NAO rele: o que esta em memoria e a
        /// partida (com save de versao mais nova o Commit e recusado, e reler apagaria o que a sessao fez).</summary>
        public static bool Carregado { get; private set; }

        // Play no Editor sem domain reload: a trava volta a zero a cada Play, como num processo novo.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void NovoProcesso() { Carregado = false; Diario = null; }

        /// <summary>Grava o save. Fora do Play (testes de editor, geradores de cena) nao toca o disco: um teste que
        /// avancava missao pela sessao gravava por cima do save.json de quem joga no mesmo PC (visto em 2026-10-02).
        /// Toda transicao que deu certo passa por aqui (GameSession.gravar), entao e aqui que o diario olha o save.</summary>
        public static void Commit()
        {
            if (!Application.isPlaying) return;
            LocalSave.Save(Current);
            if (Diario != null) Diario.Observar(Current);
        }

        /// <summary>Diario de playtest desta sessao (DiarioDeSessao). null fora do Play: teste de editor nao escreve em
        /// persistentDataPath, pela mesma regra do Commit.</summary>
        public static DiarioDeSessao Diario { get; private set; }

        /// <summary>Uma vez por processo, logo depois do Load. -roteiro escreve noutra pasta (como o save dele): o robo
        /// nunca empurra para fora os diarios de quem joga.</summary>
        public static void AbrirDiario(string cena)
        {
            if (!Application.isPlaying) return;
            string pasta = System.IO.Path.Combine(Application.persistentDataPath, Roteiro.Ligado ? "roteiro_diario" : "diario");
            Diario = new DiarioDeSessao(pasta, () => System.DateTime.UtcNow, Current, Application.version, cena);
            Debug.Log("Diario de sessao: " + (Diario.Arquivo ?? "sem arquivo (pasta nao abriu)"));
        }

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
    /// Tambem e o ouvido do diario de sessao para segundo plano e saida (pausa/volta/fim).
    /// ponytail: um slot so. Varios slots = escolher o path antes do Load; nada mais muda.</summary>
    [DefaultExecutionOrder(-200)]
    public class SaveBootstrap : MonoBehaviour
    {
        void Awake()
        {
            if (SaveState.Carregado) return;   // uma vez por processo, nao por cena
            SaveState.Load();
            SaveState.AbrirDiario(gameObject.scene.name);
        }

        void OnApplicationPause(bool paused) { if (paused) SaveState.Commit(); SegundoPlano(paused); }

        void OnApplicationFocus(bool foco) { SegundoPlano(!foco); }

        // ponytail: no Android matar o app pela lista de recentes pode nao chamar OnApplicationQuit; ai o diario termina
        // na ultima "pausa". Sem conserto do lado do jogo.
        void OnApplicationQuit()
        {
            SaveState.Commit();
            if (SaveState.Diario != null) SaveState.Diario.Fim();
        }

        static void SegundoPlano(bool sim) { if (SaveState.Diario != null) SaveState.Diario.SegundoPlano(sim); }
    }
}
