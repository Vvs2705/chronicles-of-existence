using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>Estados de uma missao. Os NUMEROS estao congelados: eles vao para o save como int, entao
    /// reordenar ou inserir no meio quebraria save gravado (CLAUDE.md: id estavel; so acrescentar no fim).
    ///
    /// Indisponivel -> Disponivel -> EmAndamento -> Concluida
    ///                                   \-> Falhada        (so missao OPCIONAL; ver QuestSystem.Falhar)
    /// Concluida e Falhada sao TERMINAIS: nao ha transicao de saida. Quest concluida nao reabre
    /// (dossie §M / GDD cap. 07).</summary>
    public enum QuestStatus
    {
        Indisponivel = 0,
        Disponivel = 1,
        EmAndamento = 2,
        Concluida = 3,
        Falhada = 4,
    }

    /// <summary>Progresso de UMA missao. Isto e ESTADO de partida: vive no save, nunca no catalogo.
    /// Campos publicos e mutaveis porque JsonUtility exige; quem muda e QuestSystem, nao o chamador.
    ///
    /// Nao existe "recompensaConcedida" aqui de proposito: a idempotencia de recompensa e por ID DE
    /// RECOMPENSA no historico de vida (T005), nao por flag de missao — flag por missao nao protege
    /// duas missoes que compartilham a mesma recompensa unica.</summary>
    [Serializable]
    public class QuestState
    {
        public string questId = "";
        public int status = (int)QuestStatus.Disponivel;
        public List<string> objetivosFeitos = new List<string>();
    }

    /// <summary>O BLOCO NOVO do save (T006). Nasce vazio = nenhuma missao tocada, que e exatamente o que
    /// um save gravado antes desta tarefa significa (regra de versao no cabecalho de SaveData.cs).
    ///
    /// So guarda missao TOCADA. Missao que ninguem iniciou nao ocupa linha: seu estado e derivado do
    /// catalogo a cada consulta, entao acrescentar missao nova ao catalogo nao exige migracao de save.</summary>
    [Serializable]
    public class QuestLog
    {
        public List<QuestState> missoes = new List<QuestState>();
    }
}
