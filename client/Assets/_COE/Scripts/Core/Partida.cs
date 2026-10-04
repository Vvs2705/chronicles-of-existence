using UnityEngine;

namespace COE
{
    /// <summary>A partida da cena: a sessao que telas, NPCs, gatilhos e o corpo usam, por campo serializado `partida` (dependencia
    /// explicita; o gerador liga em toda a cena, PartidaSetup). Mora no objeto "Save", ao lado do SaveBootstrap.
    /// No jogo a sessao e a do SaveState (bootstrap do processo: le o save uma vez e grava); teste atribui a sua (Usar) e
    /// nada vai para o save.json do PC. Este e o UNICO ponto de runtime, alem da entrada e do robo, que fala com o SaveState
    /// (cerca no ArquiteturaTests).</summary>
    public class Partida : MonoBehaviour
    {
        GameSession propria;

        /// <summary>A sessao desta cena: a atribuida (teste) ou a do SaveState, reaberta sozinha quando o save troca.</summary>
        public GameSession Sessao { get { return propria ?? SaveState.Sessao; } }

        /// <summary>Teste: a cena passa a usar esta sessao (com o "gravar" que o teste quiser). null volta ao SaveState.</summary>
        public void Usar(GameSession sessao) { propria = sessao; }

        /// <summary>A sessao de quem tem o campo `partida`. Sem partida ligada (componente montado a mao, teste de editor), a do
        /// SaveState: o comportamento de antes, num lugar so.</summary>
        public static GameSession De(Partida partida) { return partida != null ? partida.Sessao : SaveState.Sessao; }
    }
}
