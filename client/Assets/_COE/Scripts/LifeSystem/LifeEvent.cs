using System;

namespace COE
{
    /// <summary>Um acontecimento CANONICO da vida do personagem — um fato com significado, nao um log.
    /// Contrato do GDD v1.2 §10 ("LifeEvent | eventId, scopeId, age, eventType, consequences, processed").
    ///
    /// O QUE ENTRA: nascimento, marco de idade, escolha com consequencia, primeira vez em algo, relacao.
    /// O QUE NAO ENTRA (dossie §K e GDD §10, "Event History: nao armazenar cada frame/dialogo trivial"):
    /// transcricao de conversa, passo de caminhada, abrir inventario, cada dia que passou.
    ///
    /// O `eventId` e a chave de idempotencia do jogo inteiro: ele identifica O FATO, nao a vez em que aconteceu.
    /// `marco_idade_8` e um id; `conversa_borin_2026_09_28_18_03` NAO e — id por instancia faz o historico
    /// crescer sem fim e destroi a protecao contra recompensa repetida (dossie §H/§M).
    ///
    /// Dois campos do contrato do GDD nao viraram campo aqui, de proposito:
    /// - `consequences`: quem concede recompensa e o sistema dono dela (T006 missao, T010 reputacao). O historico
    ///   diz "aconteceu"; nao guarda o premio, senao vira uma segunda fonte de verdade que pode discordar.
    /// - `processed`: estar registrado JA significa processado. Duas verdades ("existe" e "foi processado") podem
    ///   divergir num crash; uma so nao pode. Ver LifeEventHistory.Registrar.</summary>
    [Serializable]
    public class LifeEvent
    {
        public string eventId = "";    // id estavel, snake_case ASCII, sem acento (CLAUDE.md / contrato §1)
        public string categoria = "";  // um dos LifeEventCategoria abaixo (`eventType` no contrato do GDD)
        public int idade;              // `age` no contrato do GDD. Idade do personagem quando ocorreu; 0 = desconhecida (evento resumido)
        public long emUtc;             // DateTime.UtcNow.Ticks no registro; 0 = desconhecido (evento resumido)
        public string escopo = "";     // `scopeId` no contrato do GDD. Chave do contexto: "npc_borin", "quest_cesto_perdido", "" quando nao ha
        public string detalhe = "";    // valor do contexto, curto: "recusou", "lysa", "bosque". Nunca uma frase de dialogo
        public bool resumido;          // true = o detalhe foi podado pelo teto; o FATO continua valendo (ver Podar)
    }

    /// <summary>Categorias de acontecimento. Sao ids estaveis gravados no save: nao renomear sem migracao.</summary>
    public static class LifeEventCategoria
    {
        public const string Nascimento = "nascimento";     // uma vez por personagem
        public const string Marco = "marco";               // marco narrativo/de idade (dossie §D: idade avanca por marco)
        public const string Escolha = "escolha";           // escolha com consequencia duradoura
        public const string PrimeiraVez = "primeira_vez";  // primeira vez em algo (primeira espada, primeira magia)
        public const string Relacao = "relacao";           // vinculo criado/rompido com um NPC
    }
}
