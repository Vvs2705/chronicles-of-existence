using System;

namespace COE
{
    /// <summary>Estado de uma partida do COE, gravado como JSON por LocalSave. C# puro: nada de UnityEngine aqui.
    ///
    /// COMO ESTE FORMATO CRESCE SEM QUEBRAR:
    /// - Campo ou bloco NOVO nasce com padrao neutro e NAO sobe saveVersion. O leitor ignora chave que nao conhece
    ///   e mantem o padrao da chave que faltar, entao save velho carrega no jogo novo e vice-versa.
    /// - Inventario (T006+), missoes, reputacao e historico de vida (T005) entram como blocos novos, do mesmo jeito.
    /// - Subir SchemaVersion so quando um campo EXISTENTE for renomeado, removido ou mudar de significado — e ai o
    ///   tratamento da versao velha vai em LocalSave (hoje: descarte com copia, ver o cabecalho de LocalSave).
    /// - Ascensao/Grau de Existencia ficam de fora de proposito (dossie §E: fora do primeiro slice).</summary>
    [Serializable]
    public class SaveData
    {
        public const int SchemaVersion = 1;

        public int saveVersion = SchemaVersion;
        public string characterId = "";                // Guid gerado UMA vez, na primeira gravacao; nunca muda depois
        public BirthChoice birth = new BirthChoice();  // DTO da T003 (contrato §2): gravado e lido sem interpretacao
        public Identity identity = new Identity();
        public int ageYears = 5;                       // dossie §D: a vida jogavel comeca aos 5
        public Attributes attributes = new Attributes();
        public Affinities affinities = new Affinities();
        public int lifeLevel = 1;
        public LifeHistoryData lifeHistory = new LifeHistoryData();  // T005: bloco novo, padrao neutro (lista vazia); nao sobe saveVersion
        public QuestLog quests = new QuestLog();       // T006: idem. Lista vazia = nenhuma missao tocada ainda
        public LifeState life = new LifeState();       // T009: idem. Dia/periodo do cotidiano e ledger anti-farm
        public NpcBook npcs = new NpcBook();           // bloco da T007: memoria dos NPCs por id de evento.
                                                       // Nasce vazio (padrao neutro) e NAO sobe saveVersion.
                                                       // O historico canonico dos eventos e da T005; aqui so
                                                       // fica quem lembra de que.
        public ReputationData reputation = new ReputationData();  // T010: idem. Confianca por NPC e renome por comunidade
        public string createdAtUtc = "";               // ISO 8601 UTC, carimbado por LocalSave.Save
        public string updatedAtUtc = "";
    }

    /// <summary>O que sobra da identidade depois do BirthChoice (o nome mora la). ponytail: so aparencia hoje;
    /// a criacao de personagem (tarefa posterior) diz o que mais entra aqui.</summary>
    [Serializable]
    public class Identity
    {
        public string appearanceId = "";
    }

    /// <summary>Os 6 atributos pelos ids do contrato §1 — o nome do campo E o id estavel no JSON.
    /// Base 1 = crianca de 5 anos sem treino; valores reais e curvas sao T009.</summary>
    [Serializable]
    public class Attributes
    {
        public int forca = 1;
        public int agilidade = 1;
        public int vigor = 1;
        public int intelecto = 1;
        public int percepcao = 1;
        public int vontade = 1;
    }

    /// <summary>As 6 afinidades pelos ids do contrato §1. Comecam em 0: afinidade e aprendizado acumulado,
    /// ninguem nasce com pratica marcial ou arcana.</summary>
    [Serializable]
    public class Affinities
    {
        public int marcial;
        public int arcana;
        public int natural;
        public int artesanal;
        public int social;
        public int exploratoria;
    }
}
