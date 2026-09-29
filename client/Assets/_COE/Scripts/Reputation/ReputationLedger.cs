using System;

namespace COE
{
    /// <summary>ADAPTADOR — o UNICO arquivo de Scripts/Reputation que conhece a T005 (LifeEventHistory) e o
    /// SaveData. ReputationSystem depende desta classe e de mais nada externo; se a T005 mudar de API, muda aqui.
    ///
    /// POR QUE O PREFIXO `rep_`: o id de idempotencia da reputacao NAO pode ser o id do evento canonico cru.
    /// A missao (T006/T012) tambem registra o seu proprio evento com aquele id — se a reputacao usasse o mesmo,
    /// quem registrasse primeiro faria o outro receber false e o efeito nunca seria concedido. Entao o fato
    /// canonico "quest_cesto_perdido_entregue" e da missao, e o fato "rep_quest_cesto_perdido_entregue" e a
    /// marca de que a reputacao daquele evento ja contou. Sao dois fatos distintos no mesmo historico, cada um
    /// com um dono, e os dois saem na mesma gravacao atomica do SaveData (T004).
    ///
    /// ORDEM DE USO: marcar primeiro, aplicar depois (e o que ReputationSystem.Aplicar faz). Como marca e efeito
    /// moram no MESMO SaveData e sao gravados juntos, nao existe arquivo em que a reputacao tenha mudado e a
    /// marca nao esteja la — ou os dois entraram, ou nenhum entrou e o jogador refaz a acao.
    ///
    /// ponytail: classe concreta, sem interface. Ela tem um consumidor e uma implementacao, e os testes usam o
    /// LifeEventHistory de verdade porque ele e C# puro. Interface so quando existir a segunda implementacao.</summary>
    public class ReputationLedger
    {
        /// <summary>Prefixo do id de evento da reputacao no historico de vida. Id gravado no save: mudar isto
        /// exige migracao (um save antigo passaria a achar que nenhuma reputacao foi aplicada e reaplicaria tudo).</summary>
        public const string Prefixo = "rep_";

        readonly LifeEventHistory historia;
        readonly SaveData save;

        /// <summary>`historia` e `save` tem de ser do MESMO save: a marca rep_ vai para `historia`, a reputacao vai
        /// para `save.reputation`, e as duas precisam sair na mesma gravacao atomica. Com historico de outro save a
        /// marca se perderia e o load pagaria de novo — por isso lanca.
        /// Qualquer instancia de LifeEventHistory aberta sobre esse save serve: desde a T005 ela nao guarda indice
        /// proprio (Ja/Registrar varrem a lista do save), entao duas instancias no mesmo save — a da missao e a da
        /// reputacao, p.ex. — enxergam os registros uma da outra e nao duplicam.</summary>
        public ReputationLedger(LifeEventHistory historia, SaveData save)
        {
            if (historia == null) throw new ArgumentNullException("historia");
            if (save == null) throw new ArgumentNullException("save");
            if (historia.Dados != save.lifeHistory)
                throw new ArgumentException("historia de outro save: a marca rep_ nao seria gravada junto com a reputacao.", "historia");
            this.historia = historia;
            this.save = save;
        }

        /// <summary>A reputacao daquele evento canonico ja foi aplicada nesta vida? Consulta pura, nao marca nada.</summary>
        public bool Ja(string fonteEventoId)
        {
            return historia.Ja(Prefixo + fonteEventoId);
        }

        /// <summary>O fato canonico em si (o da missao, sem prefixo) esta no historico? E o que
        /// ReputationSystem.Sincronizar pergunta antes de aplicar uma consequencia.</summary>
        public bool Aconteceu(string eventoId)
        {
            return historia.Ja(eventoId);
        }

        /// <summary>Marca o evento e devolve true SO na primeira vez — a idempotencia da T010 inteira sai daqui.
        /// A segunda chamada devolve false e nada e alterado.
        ///
        /// O fato entra como categoria `relacao` (LifeEventCategoria), com a idade atual do save. `escopo` recebe
        /// o alvo quando o ato tem um so — e assim que LifeEventHistory.PorEscopo("npc_borin") devolve tambem as
        /// mudancas de reputacao daquele NPC, que e o que a memoria do NPC (T007) consulta. Ato com varios alvos
        /// fica sem escopo: um evento so nao pode pertencer a dois escopos, e inventar um escopo sintetico
        /// poluiria a consulta por NPC.</summary>
        public bool PrimeiraVez(string fonteEventoId, ReputationAto ato)
        {
            string escopo = "";
            if (ato != null && ato.Efeitos != null && ato.Efeitos.Count == 1) escopo = ato.Efeitos[0].Alvo;
            string detalhe = ato == null ? "" : ato.AtoId;

            return historia.Registrar(Prefixo + fonteEventoId, LifeEventCategoria.Relacao, save.ageYears, escopo, detalhe);
        }
    }
}
