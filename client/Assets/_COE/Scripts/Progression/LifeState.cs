using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>Bloco de save da T009 (vida cotidiana + progressao). C# PURO, sem UnityEngine.
    ///
    /// COMO ENTRA NO SAVE: SaveData.life. Bloco NOVO => nasce com padrao neutro e NAO sobe saveVersion
    /// (regra escrita no cabecalho de SaveData.cs pela T004). Save v1 antigo, sem "life", carrega e
    /// ganha este bloco zerado: dia 1, manha, sem marcos, sem pratica.
    ///
    /// O QUE NAO ESTA AQUI: marco de idade ja aplicado. Isso e evento de vida e mora no LifeEventHistory da
    /// T005 (save.lifeHistory) -- duas listas dizendo "o salto ja aconteceu" seria duas verdades que podem
    /// discordar num crash.
    ///
    /// POR QUE O LEDGER DE PRATICA MORA NO SAVE E NAO EM MEMORIA: o teto anti-farm e POR ETAPA DA VIDA
    /// (dossie §D). Se ele nao persistir, fechar e reabrir o jogo zera o teto e o farm volta — seria o
    /// mesmo exploit por outra porta.</summary>
    [Serializable]
    public class LifeState
    {
        public int day = 1;                  // dias JOGADOS, nao calendario do mundo; so cresce
        public string timeOfDay = "manha";   // id estavel: manha | tarde | noite (TimeOfDayCycle)

        /// <summary>Uma linha por (atividade, fase). E o que impede dominio infinito por repeticao.</summary>
        public List<PracticeEntry> pratica = new List<PracticeEntry>();
    }

    /// <summary>Quanto uma atividade ja rendeu numa fase da vida. Chave composta: activityId + phaseId.
    /// Fase nova = linha nova, entao o teto reabre a cada etapa — crescer devolve espaco de aprendizado,
    /// repetir na mesma etapa nao.</summary>
    [Serializable]
    public class PracticeEntry
    {
        public string activityId = "";
        public string phaseId = "";     // LifePhases.Id(...)
        public string trackId = "";     // um dos 12 ids do CONTRATO_T003_T004 §1 (6 atributos + 6 afinidades)
        public int vezes;               // execucoes desta atividade NESTA fase
        public int progresso;           // progresso ja concedido por esta atividade NESTA fase
    }
}
