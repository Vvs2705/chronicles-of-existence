using System;

namespace COE
{
    /// <summary>ESTE E O UNICO ARQUIVO DA T007 QUE CONHECE A T006. QuestSystem, QuestIntent, QuestAcao,
    /// QuestStatus e QuestResultado aparecem aqui e em lugar nenhum mais do modulo Dialogue/NPC: se a T006
    /// mudar a assinatura, o conserto e neste arquivo.
    ///
    /// A REGRA QUE ELE EXISTE PARA GARANTIR: uma fala nao muda missao, inventario nem reputacao. A opcao
    /// de dialogo carrega um PedidoDeMissao; quem valida e aplica e o QuestSystem; sem QuestSystem ligado,
    /// NADA muda e o retorno diz isso em vez de fingir sucesso
    /// (DialogueRunnerTests.SemValidador_PedidoNaoMudaNada).</summary>
    public static class QuestIntentAdapter
    {
        /// <summary>Despacha o pedido que a fala emitiu. Devolve o resultado do QuestSystem, incluindo as
        /// recompensas que ELE aprovou -- o dialogo nunca inventa recompensa.
        ///
        /// missoes == null (sistema de missao ainda nao ligado, cena de teste, prologo) devolve Ok == false
        /// sem tocar em nada. Excecao do validador vira Ok == false tambem: adaptador quebrado nao derruba
        /// a conversa nem concede por acidente.</summary>
        public static QuestResultado Despachar(QuestSystem missoes, PedidoDeMissao pedido)
        {
            if (pedido == null || string.IsNullOrEmpty(pedido.QuestId)) return Recusa(QuestErro.MissaoDesconhecida);
            if (missoes == null) return Recusa(QuestErro.IntencaoInvalida);

            try { return missoes.TentarAvancar(pedido.QuestId, pedido.Intencao); }
            catch (Exception) { return Recusa(QuestErro.IntencaoInvalida); }
        }

        /// <summary>Liga a leitura de estado de missao ao DialogueContext. E so LEITURA: o delegate
        /// devolve int e nao tem como escrever no QuestLog.</summary>
        public static Func<string, int> Leitor(QuestSystem missoes)
        {
            if (missoes == null) return null;
            return delegate (string questId) { return (int)missoes.Estado(questId); };
        }

        static QuestResultado Recusa(QuestErro erro)
        {
            QuestResultado r = default(QuestResultado);
            r.Ok = false;
            r.Erro = erro;
            r.Status = QuestStatus.Indisponivel;
            r.Recompensas = QuestResultado.Nada;
            return r;
        }
    }

    /// <summary>Os estados de missao como numero, para o dado de dialogo comparar sem que o resto do
    /// modulo precise conhecer a T006. Derivados do enum: nao ha como divergirem.</summary>
    public static class EstadoMissao
    {
        public const int Indisponivel = (int)QuestStatus.Indisponivel;
        public const int Disponivel = (int)QuestStatus.Disponivel;
        public const int EmAndamento = (int)QuestStatus.EmAndamento;
        public const int Concluida = (int)QuestStatus.Concluida;
        public const int Falhada = (int)QuestStatus.Falhada;
    }

    /// <summary>O que uma fala PEDE ao sistema de missao: um id de missao e uma acao da allowlist fechada
    /// da T006 (iniciar, cumprir objetivo, concluir, falhar -- nao existe "conceder recompensa").
    ///
    /// Imutavel e sem metodo de aplicar: um pedido nao tem como mudar nada sozinho. E dado.</summary>
    public sealed class PedidoDeMissao
    {
        public readonly string QuestId;
        public readonly QuestIntent Intencao;

        public PedidoDeMissao(string questId, QuestIntent intencao) { QuestId = questId; Intencao = intencao; }

        public static PedidoDeMissao Iniciar(string questId)
        {
            return new PedidoDeMissao(questId, new QuestIntent(QuestAcao.Iniciar, null));
        }

        public static PedidoDeMissao Objetivo(string questId, string objetivoId)
        {
            return new PedidoDeMissao(questId, new QuestIntent(QuestAcao.CumprirObjetivo, objetivoId));
        }

        public static PedidoDeMissao Concluir(string questId)
        {
            return new PedidoDeMissao(questId, new QuestIntent(QuestAcao.Concluir, null));
        }
    }
}
