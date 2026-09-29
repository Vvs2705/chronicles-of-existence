using System;

namespace COE
{
    /// <summary>ESTE E O UNICO PONTO DE ACOPLAMENTO ENTRE T006 (missoes) E T005 (LifeEventHistory).
    /// Se a T005 mudar de assinatura, so este arquivo muda: QuestSystem nao cita LifeEventHistory,
    /// LifeEvent nem SaveData em lugar nenhum.
    ///
    /// POR QUE ASSIM: a idempotencia por id de evento e da T005, nao da T006 (ordem do coordenador). O
    /// QuestSystem nao tem contador, flag de "ja paguei" nem lista propria de recompensas concedidas —
    /// ele PERGUNTA ao historico de vida.
    ///
    /// A REGRA DE OURO desta fronteira: registrar e perguntar sao a MESMA chamada. Registrar devolve
    /// true so na primeira vez, entao nao existe janela entre "consultei" e "concedi" em que um segundo
    /// caminho conceda de novo. Quem usa Ja() para decidir concessao esta usando a API errada; Ja() aqui
    /// serve so para pre-condicao (a missao ja pode comecar?).</summary>
    public interface ILifeEventLedger
    {
        /// <summary>Aquele fato ja aconteceu nesta vida? Id vazio ou desconhecido responde false, nunca lanca.
        /// Use para PRE-CONDICAO. Para conceder, use RegistrarSePrimeiro.</summary>
        bool Ja(string eventoId);

        /// <summary>Registra o fato e devolve true SO NA PRIMEIRA VEZ. false = ja estava registrado e
        /// nada mudou — e o chamador nao deve conceder nada.</summary>
        bool RegistrarSePrimeiro(string eventoId, string escopo);
    }

    /// <summary>A ponte real com a T005. Toda conclusao de missao e toda recompensa viram um fato
    /// canonico do historico de vida, na categoria Marco e com escopo "quest_&lt;questId&gt;" — que e
    /// exatamente a chave que LifeEventHistory.PorEscopo devolve para T007 (memoria de NPC) e T010
    /// (reputacao) mais tarde.
    ///
    /// A idade e lida do SaveData NO MOMENTO do registro (nao no construtor): o historico tem de dizer
    /// com que idade o fato aconteceu, e a idade muda durante a partida (T009).</summary>
    public sealed class HistoricoDeVidaLedger : ILifeEventLedger
    {
        readonly LifeEventHistory historia;
        readonly SaveData save;

        /// <summary>Caminho do jogo: abre o historico do proprio save.</summary>
        public HistoricoDeVidaLedger(SaveData save) : this(save, null) { }

        /// <summary>Caminho de quem ja abriu o historico (a fiacao do coordenador, e os testes). Reaproveitar
        /// o historico ja aberto e o preferido; abrir outro sobre o mesmo save tambem e seguro, porque
        /// LifeEventHistory nao guarda indice proprio (T005_DuasInstanciasNoMesmoSave_NaoDuplicam).</summary>
        public HistoricoDeVidaLedger(SaveData save, LifeEventHistory historia)
        {
            if (save == null) throw new ArgumentNullException("save");
            if (historia != null && historia.Dados != save.lifeHistory)   // mesma trava do ReputationLedger
                throw new ArgumentException("historia de outro save: o fato iria para um save e a missao para outro.", "historia");
            this.save = save;
            this.historia = historia ?? new LifeEventHistory(save);
        }

        public LifeEventHistory Historia { get { return historia; } }

        public bool Ja(string eventoId) { return historia.Ja(eventoId); }

        public bool RegistrarSePrimeiro(string eventoId, string escopo)
        {
            if (string.IsNullOrEmpty(eventoId)) return false;
            return historia.Registrar(eventoId, LifeEventCategoria.Marco, save.ageYears, escopo);
        }
    }
}
