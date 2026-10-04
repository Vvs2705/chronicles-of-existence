using System;

namespace COE
{
    /// <summary>Estado de uma partida do COE, gravado como JSON por LocalSave. C# puro: nada de UnityEngine aqui.
    ///
    /// POLITICA DE VERSAO (unica, desde o v2 — 2026-10-04; CLAUDE.md "Save versionado"):
    /// - QUALQUER mudanca no formato gravado que uma build anterior nao preserve sobe SchemaVersion: campo ou bloco
    ///   novo, campo renomeado, removido ou com significado novo. Motivo: o JsonUtility ignora chave que nao conhece;
    ///   uma build velha lendo um save "da mesma versao" perderia o que nao conhece na gravacao seguinte. Com a versao
    ///   maior, a build velha recusa gravar por cima (LocalSave: versao futura fica intacta).
    /// - Junto com a subida: o passo v -> v+1 em LocalSave.Migracoes (DTO congelado da versao de origem) e teste que
    ///   carrega um JSON da versao velha. Campo novo nasce com padrao neutro, que e o que o save velho significa.
    /// - SaveDataTests.FormatoGravado_Congelado quebra quando o formato muda sem passar por aqui.
    /// - Historico: o v1 (T004, 2026-09-28) cresceu sem subir a versao (historico, missoes, vida, NPCs, reputacao,
    ///   inventario entraram como blocos novos de padrao neutro). O v2 e esse v1 inteiro, com a politica acima.
    /// - Ascensao/Grau de Existencia ficam de fora de proposito (dossie §E: fora do primeiro slice).
    ///
    /// FLAGS DE HISTORIA: nao existe bloco de flags, de proposito. Fato de historia ("prometeu a Nilo", "salto dos
    /// 8 feito", "recompensa X paga") e um LifeEvent em lifeHistory, consultado por LifeEventHistory.Ja(id) — a
    /// fonte unica que quests, NPCs, reputacao e o salto ja usam. Um bloco de flags paralelo seria segunda verdade
    /// que pode discordar num crash. Progresso de missao mora em quests; o resto do que o jogo lembra, idem por bloco.</summary>
    [Serializable]
    public class SaveData
    {
        public const int SchemaVersion = 2;

        public int saveVersion = SchemaVersion;
        public string characterId = "";                // Guid gerado UMA vez, na primeira gravacao; nunca muda depois
        public BirthChoice birth = new BirthChoice();  // DTO da T003 (contrato §2): gravado e lido sem interpretacao
        public Identity identity = new Identity();
        public int ageYears = 5;                       // dossie §D: a vida jogavel comeca aos 5
        public Attributes attributes = new Attributes();
        public Affinities affinities = new Affinities();
        public int lifeLevel = 1;
        public LifeHistoryData lifeHistory = new LifeHistoryData();  // T005: padrao neutro = lista vazia
        public QuestLog quests = new QuestLog();       // T006: idem. Lista vazia = nenhuma missao tocada ainda
        public LifeState life = new LifeState();       // T009: idem. Dia/periodo do cotidiano e ledger anti-farm
        public NpcBook npcs = new NpcBook();           // bloco da T007: memoria dos NPCs por id de evento.
                                                       // O historico canonico dos eventos e da T005; aqui so
                                                       // fica quem lembra de que.
        public ReputationData reputation = new ReputationData();  // T010: idem. Confianca por NPC e renome por comunidade
        public InventarioData inventario = new InventarioData();  // T012: idem. Moedas e itens de recompensa de missao
        // ONDE o jogador esta (T004, GDD "cena"). Padrao neutro "" nos dois = entrada padrao.
        // Ancora e o contrato estavel (slice §1, AurenSceneBuilder.Ancoras); posicao livre fica de fora de proposito:
        // a cena e regerada do zero pelo builder e um Vector3 velho pode cair dentro de parede.
        public string sceneId = "";                    // id snake_case da cena (ex.: "auren"); "" = cena inicial do jogo
        public string anchorId = "";                   // id da ancora de entrada nessa cena; "" = spawn_player (B06, B14)
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
