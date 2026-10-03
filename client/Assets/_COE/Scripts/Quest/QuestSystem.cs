using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>Por que a transicao falhou. Nenhum = sucesso.</summary>
    public enum QuestErro
    {
        Nenhum = 0,
        MissaoDesconhecida,
        PreRequisitoFaltando,   // missao pre-requisito nao concluida, ou evento de vida ausente
        NaoEstaDisponivel,      // Iniciar em missao que ja saiu de Disponivel
        NaoEstaEmAndamento,     // Objetivo/Concluir/Falhar em missao que nao esta EmAndamento
        ObjetivoDesconhecido,
        ObjetivoForaDeOrdem,
        ObjetivosPendentes,     // Concluir antes de cumprir tudo
        CentralNaoFalha,        // anti-softlock: missao central nao pode ir para Falhada
        IntencaoInvalida,       // acao fora da allowlist (fronteira com T007/dialogo)
        DesfechoInvalido,       // evento que nao e desfecho desta missao
        DesfechoJaDecidido,     // pedir o OUTRO desfecho depois de um gravado: exatamente um, nunca dois
        DesfechoPendente,       // Concluir missao com desfecho sem ter gravado nenhum
    }

    /// <summary>Acoes que o diálogo pode PEDIR. Allowlist fechada: dialogo (inclusive gerado por IA) nao
    /// alcanca nada fora desta lista, e nao existe acao "conceder recompensa" — recompensa e consequencia
    /// de Concluir, nunca pedido de terceiro (GDD cap. 07 "IA inventar recompensa" / dossie §M).</summary>
    public enum QuestAcao
    {
        Iniciar = 0,
        CumprirObjetivo = 1,
        Concluir = 2,
        Falhar = 3,
        EscolherDesfecho = 4,   // Q-04: a escolha do jogador na conversa; nao concede nada
    }

    /// <summary>A intencao estruturada que T007 entrega. Ela e um PEDIDO: quem valida e aplica e o
    /// QuestSystem. Dialogo nunca muda estado direto.</summary>
    public struct QuestIntent
    {
        public QuestAcao Acao;
        // CumprirObjetivo: id do objetivo. EscolherDesfecho: id do evento de desfecho.
        // ponytail: um campo para os dois porque nenhuma acao usa ambos; renomear quebraria a T007.
        public string ObjetivoId;

        public QuestIntent(QuestAcao acao, string objetivoId = null)
        {
            Acao = acao; ObjetivoId = objetivoId;
        }

        /// <summary>Converte texto de fora (linha de dialogo, JSON, saida de modelo) em intencao.
        /// false = texto desconhecido, e ai NADA acontece. Esta e a fronteira de confianca: string livre
        /// vira acao so por esta allowlist, nunca por Enum.Parse.</summary>
        public static bool TryParse(string acao, string objetivoId, out QuestIntent intent)
        {
            intent = default(QuestIntent);
            switch (acao)
            {
                case "iniciar": intent = new QuestIntent(QuestAcao.Iniciar); return true;
                case "objetivo": intent = new QuestIntent(QuestAcao.CumprirObjetivo, objetivoId); return true;
                case "concluir": intent = new QuestIntent(QuestAcao.Concluir); return true;
                case "falhar": intent = new QuestIntent(QuestAcao.Falhar); return true;
                case "desfecho": intent = new QuestIntent(QuestAcao.EscolherDesfecho, objetivoId); return true;
                default: return false;
            }
        }
    }

    /// <summary>Resultado de uma transicao. Ok == false sempre traz Erro e Recompensas vazio.</summary>
    public struct QuestResultado
    {
        public bool Ok;
        public QuestErro Erro;
        public QuestStatus Status;            // estado da missao DEPOIS da chamada
        public RecompensaDef[] Recompensas;   // so o que foi concedido AGORA; vazio na repeticao

        public static readonly RecompensaDef[] Nada = new RecompensaDef[0];
    }

    /// <summary>Maquina de estados de missao (T006). C# PURO, sem UnityEngine — testavel sem cena, como
    /// DestinySystem e MotionSolver. Nao le nem grava arquivo: o formato do save e de T004.
    ///
    /// CONTRATO. Recebe o QuestLog (bloco do save, ESTADO) e um ILifeEventLedger (historico de vida,
    /// T005). Muda o QuestLog no lugar; quem grava e quando e do dono do save.
    ///
    /// IDEMPOTENCIA — duas linhas de defesa independentes, porque uma so nao cobre os dois acidentes:
    ///   1. STATUS TERMINAL no QuestLog: Concluida/Falhada nao tem transicao de saida, entao concluir de
    ///      novo devolve NaoEstaEmAndamento e recompensa nenhuma.
    ///   2. ID DE RECOMPENSA no historico de vida: antes de conceder, pergunta Ja(rec.Id). Isso cobre o
    ///      caso que a linha 1 nao cobre — save gravado com o historico ja atualizado e o status ainda
    ///      atrasado (ou save editado a mao de volta para EmAndamento). Este e o teste obrigatorio nº 3 do
    ///      backlog e QuestTests.Obrigatorio3_* prova os dois caminhos.
    /// Consistencia entre as duas: QuestLog e historico moram no MESMO arquivo de save, gravado
    /// atomicamente por LocalSave (tmp + replace). Ou as duas mudancas entram, ou nenhuma entra.
    ///
    /// ANTI-SOFTLOCK: missao central nunca pode ir para Falhada, e nenhuma central depende de opcional
    /// (invariante do catalogo). Ignorar as tres opcionais fecha a campanha.
    ///
    /// O QUE NAO ENTRA AQUI: aplicar moedas e item. Conceder DEVOLVE as RecompensaDef aprovadas e T012/
    /// Inventory as aplica — GDD cap. 10: Quest "nao deve alterar UI ou inventario sem validacao".
    /// Marco e aplicado AQUI (vira evento de vida com o id do Alvo); na lista devolvida ele e so exibicao.
    ///
    /// TODO FATO QUE ESTE SISTEMA GRAVA vai com escopo Escopo(questId): recompensa (rec.*), marco
    /// (marco.*), EventoDeConclusao, EventosAoConcluir e o desfecho escolhido. E por ai que T007/T010 leem.</summary>
    public sealed class QuestSystem
    {
        readonly QuestLog log;
        readonly ILifeEventLedger historico;
        readonly QuestDef[] catalogo;

        public QuestSystem(QuestLog log, ILifeEventLedger historico)
            : this(log, historico, QuestCatalog.Missoes) { }

        /// <summary>O catalogo e parametro para os testes montarem cadeias minimas sem depender do
        /// conteudo das oito missoes de Auren. O jogo usa o construtor de dois argumentos.</summary>
        public QuestSystem(QuestLog log, ILifeEventLedger historico, QuestDef[] catalogo)
        {
            // log null = save antigo sem o bloco, ou bloco gravado como null: nasce vazio, nao lanca.
            this.log = log ?? new QuestLog();
            if (this.log.missoes == null) this.log.missoes = new List<QuestState>();
            if (historico == null) throw new ArgumentNullException("historico");
            if (catalogo == null) throw new ArgumentNullException("catalogo");
            this.historico = historico;
            this.catalogo = catalogo;
        }

        public QuestDef Def(string questId)
        {
            for (int i = 0; i < catalogo.Length; i++) if (catalogo[i].Id == questId) return catalogo[i];
            return null;
        }

        // --- consulta ---

        /// <summary>Estado atual. Missao que ninguem tocou nao tem linha no save: o estado vem das
        /// pre-condicoes, entao acrescentar missao ao catalogo nao exige migracao de save.
        /// Missao desconhecida responde Indisponivel (id de save velho ou de dialogo nao derruba nada).</summary>
        public QuestStatus Estado(string questId)
        {
            QuestState s = Linha(questId);
            if (s != null) return (QuestStatus)s.status;
            if (Def(questId) == null) return QuestStatus.Indisponivel;
            return Precondicao(questId) == QuestErro.Nenhum ? QuestStatus.Disponivel : QuestStatus.Indisponivel;
        }

        /// <summary>QuestErro.Nenhum = da para iniciar agora (ignorando o estado atual da propria missao).</summary>
        public QuestErro Precondicao(string questId)
        {
            QuestDef d = Def(questId);
            if (d == null) return QuestErro.MissaoDesconhecida;

            for (int i = 0; i < d.PreMissoes.Length; i++)
                if (Estado(d.PreMissoes[i]) != QuestStatus.Concluida) return QuestErro.PreRequisitoFaltando;

            for (int i = 0; i < d.PreEventos.Length; i++)
                if (!historico.Ja(d.PreEventos[i])) return QuestErro.PreRequisitoFaltando;

            return QuestErro.Nenhum;
        }

        /// <summary>Objetivos ja cumpridos. Copia: ninguem edita o save por fora.</summary>
        public string[] ObjetivosFeitos(string questId)
        {
            QuestState s = Linha(questId);
            return s == null || s.objetivosFeitos == null ? new string[0] : s.objetivosFeitos.ToArray();
        }

        /// <summary>Quantas missoes estao Concluida. E por aqui que a sessao percebe que uma transicao concluiu
        /// missao (ADR-0007 §1: o dia anda), venha ela do gatilho, do dialogo ou do MissaoMundo.Avancar.</summary>
        public int Concluidas()
        {
            int n = 0;
            for (int i = 0; i < log.missoes.Count; i++)
                if (log.missoes[i] != null && log.missoes[i].status == (int)QuestStatus.Concluida) n++;
            return n;
        }

        // --- transicoes ---

        /// <summary>Indisponivel/Disponivel -> EmAndamento. Pre-condicao nao atendida nao inicia, e o
        /// motivo volta explicito (nunca falha em silencio).</summary>
        public QuestResultado Iniciar(string questId)
        {
            QuestDef d = Def(questId);
            if (d == null) return Falha(QuestErro.MissaoDesconhecida, QuestStatus.Indisponivel);

            QuestStatus atual = Estado(questId);
            if (atual != QuestStatus.Indisponivel && atual != QuestStatus.Disponivel)
                return Falha(QuestErro.NaoEstaDisponivel, atual);

            QuestErro pre = Precondicao(questId);
            if (pre != QuestErro.Nenhum) return Falha(pre, QuestStatus.Indisponivel);

            QuestState s = LinhaOuCria(questId);
            s.status = (int)QuestStatus.EmAndamento;
            return Sucesso(QuestStatus.EmAndamento, QuestResultado.Nada);
        }

        /// <summary>Marca um objetivo. IDEMPOTENTE: objetivo ja cumprido devolve Ok e NAO entra de novo
        /// na lista — em missao ordenada, repetir o objetivo 1 nunca "empurra" o 2 (o teste
        /// ObjetivoRepetido_NaoAvancaDuasVezes cobra isso).</summary>
        public QuestResultado CumprirObjetivo(string questId, string objetivoId)
        {
            QuestDef d = Def(questId);
            if (d == null) return Falha(QuestErro.MissaoDesconhecida, QuestStatus.Indisponivel);

            QuestStatus atual = Estado(questId);
            if (atual != QuestStatus.EmAndamento) return Falha(QuestErro.NaoEstaEmAndamento, atual);
            if (d.Objetivo(objetivoId) == null) return Falha(QuestErro.ObjetivoDesconhecido, atual);

            QuestState s = LinhaOuCria(questId);
            if (s.objetivosFeitos.Contains(objetivoId)) return Sucesso(atual, QuestResultado.Nada);

            if (d.ObjetivosEmOrdem && ProximoPendente(d, s) != objetivoId)
                return Falha(QuestErro.ObjetivoForaDeOrdem, atual);

            s.objetivosFeitos.Add(objetivoId);
            return Sucesso(atual, QuestResultado.Nada);
        }

        /// <summary>EmAndamento -> Concluida, concedendo as recompensas que ainda nao foram concedidas.
        /// Este e o unico caminho que concede recompensa de missao.</summary>
        public QuestResultado Concluir(string questId)
        {
            QuestDef d = Def(questId);
            if (d == null) return Falha(QuestErro.MissaoDesconhecida, QuestStatus.Indisponivel);

            QuestStatus atual = Estado(questId);
            if (atual != QuestStatus.EmAndamento) return Falha(QuestErro.NaoEstaEmAndamento, atual);

            QuestState s = LinhaOuCria(questId);
            for (int i = 0; i < d.Objetivos.Length; i++)
                if (!s.objetivosFeitos.Contains(d.Objetivos[i].Id))
                    return Falha(QuestErro.ObjetivosPendentes, atual);
            if (d.Desfechos.Length > 0 && DesfechoGravado(d) == null)
                return Falha(QuestErro.DesfechoPendente, atual);

            RecompensaDef[] concedidas = Conceder(d);
            s.status = (int)QuestStatus.Concluida;
            string escopo = Escopo(d.Id);
            if (!string.IsNullOrEmpty(d.EventoDeConclusao))
                historico.RegistrarSePrimeiro(d.EventoDeConclusao, escopo);
            for (int i = 0; i < d.EventosAoConcluir.Length; i++)
                historico.RegistrarSePrimeiro(d.EventosAoConcluir[i], escopo);
            return Sucesso(QuestStatus.Concluida, concedidas);
        }

        /// <summary>Grava o desfecho de missao com desfechos mutuamente exclusivos (Q-04: promessa cumprida OU
        /// quebrada — slice B08). EXATAMENTE UM: o primeiro gravado vale para sempre; pedir o mesmo de novo e
        /// Ok sem efeito (idempotente), pedir o outro e DesfechoJaDecidido. So em EmAndamento: depois de
        /// Concluida a escolha esta selada. Nao concede nada — o desfecho e fato do historico que T007/T010
        /// leem pelo escopo da missao.</summary>
        public QuestResultado EscolherDesfecho(string questId, string eventoId)
        {
            QuestDef d = Def(questId);
            if (d == null) return Falha(QuestErro.MissaoDesconhecida, QuestStatus.Indisponivel);

            QuestStatus atual = Estado(questId);
            if (atual != QuestStatus.EmAndamento) return Falha(QuestErro.NaoEstaEmAndamento, atual);
            if (string.IsNullOrEmpty(eventoId) || Array.IndexOf(d.Desfechos, eventoId) < 0)
                return Falha(QuestErro.DesfechoInvalido, atual);

            string gravado = DesfechoGravado(d);
            if (gravado != null)
                return gravado == eventoId ? Sucesso(atual, QuestResultado.Nada) : Falha(QuestErro.DesfechoJaDecidido, atual);

            historico.RegistrarSePrimeiro(eventoId, Escopo(d.Id));
            return Sucesso(atual, QuestResultado.Nada);
        }

        /// <summary>EmAndamento -> Falhada. SO missao opcional: uma central em Falhada travaria a
        /// campanha para sempre, que e o softlock que o dossie §M manda impedir. Falhada nao concede
        /// recompensa e nao reabre.</summary>
        public QuestResultado Falhar(string questId)
        {
            QuestDef d = Def(questId);
            if (d == null) return Falha(QuestErro.MissaoDesconhecida, QuestStatus.Indisponivel);

            QuestStatus atual = Estado(questId);
            if (atual != QuestStatus.EmAndamento) return Falha(QuestErro.NaoEstaEmAndamento, atual);
            if (d.Central) return Falha(QuestErro.CentralNaoFalha, atual);

            LinhaOuCria(questId).status = (int)QuestStatus.Falhada;
            return Sucesso(QuestStatus.Falhada, QuestResultado.Nada);
        }

        /// <summary>Salto temporal (slice B12/B13, T012) e sumico de Nilo (ADR-0007 §3, q03): a oportunidade opcional
        /// se encerra em qualquer estado nao terminal, inclusive nunca iniciada (Falhar exige EmAndamento). Concluida fica como esta. Central nunca.
        /// Idempotente e sem efeito colateral: nao concede, nao registra no historico.</summary>
        public QuestResultado Encerrar(string questId)
        {
            QuestDef d = Def(questId);
            if (d == null) return Falha(QuestErro.MissaoDesconhecida, QuestStatus.Indisponivel);

            QuestStatus atual = Estado(questId);
            if (d.Central) return Falha(QuestErro.CentralNaoFalha, atual);
            if (atual == QuestStatus.Concluida || atual == QuestStatus.Falhada) return Sucesso(atual, QuestResultado.Nada);

            LinhaOuCria(questId).status = (int)QuestStatus.Falhada;
            return Sucesso(QuestStatus.Falhada, QuestResultado.Nada);
        }

        /// <summary>ENTRADA DA T007. Diálogo pede, missao decide. Intencao fora da allowlist nao chega
        /// aqui (QuestIntent.TryParse ja recusa), mas um enum corrompido cai em IntencaoInvalida.</summary>
        public QuestResultado TentarAvancar(string questId, QuestIntent intencao)
        {
            switch (intencao.Acao)
            {
                case QuestAcao.Iniciar: return Iniciar(questId);
                case QuestAcao.CumprirObjetivo: return CumprirObjetivo(questId, intencao.ObjetivoId);
                case QuestAcao.Concluir: return Concluir(questId);
                case QuestAcao.Falhar: return Falhar(questId);
                case QuestAcao.EscolherDesfecho: return EscolherDesfecho(questId, intencao.ObjetivoId);
                default: return Falha(QuestErro.IntencaoInvalida, Estado(questId));
            }
        }

        // --- interno ---

        /// <summary>Concede o que o historico ainda nao tem. Registrar E perguntar sao a mesma chamada
        /// (RegistrarSePrimeiro): nao existe janela entre consultar e conceder. Recompensa ja registrada
        /// e PULADA em silencio — nao e erro, recarregar o jogo e normal —, entao o array devolvido e
        /// exatamente o que o chamador deve aplicar; vazio significa "nao aplique nada".</summary>
        RecompensaDef[] Conceder(QuestDef d)
        {
            List<RecompensaDef> novas = new List<RecompensaDef>();
            string escopo = Escopo(d.Id);
            for (int i = 0; i < d.Recompensas.Length; i++)
            {
                RecompensaDef r = d.Recompensas[i];
                if (historico.RegistrarSePrimeiro(r.Id, escopo)) novas.Add(r);
                // Aplicar marco = gravar o id do marco (o que o roteiro cita: Ja("marco.primeiro_dia")).
                // Fora do if de proposito: idempotente, e repoe o marco se o rec.* ja estava gravado sem ele.
                if (r.Tipo == QuestCatalog.TipoMarco && !string.IsNullOrEmpty(r.Alvo))
                    historico.RegistrarSePrimeiro(r.Alvo, escopo);
            }
            return novas.Count == 0 ? QuestResultado.Nada : novas.ToArray();
        }

        /// <summary>O desfecho ja gravado desta missao, ou null.</summary>
        string DesfechoGravado(QuestDef d)
        {
            for (int i = 0; i < d.Desfechos.Length; i++)
                if (historico.Ja(d.Desfechos[i])) return d.Desfechos[i];
            return null;
        }

        /// <summary>Chave de contexto no historico de vida. E por ela que T007/T010 perguntam depois
        /// "o que aconteceu nesta missao" (LifeEventHistory.PorEscopo).</summary>
        public static string Escopo(string questId) { return "quest_" + questId; }

        static string ProximoPendente(QuestDef d, QuestState s)
        {
            for (int i = 0; i < d.Objetivos.Length; i++)
                if (!s.objetivosFeitos.Contains(d.Objetivos[i].Id)) return d.Objetivos[i].Id;
            return null;
        }

        QuestState Linha(string questId)
        {
            for (int i = 0; i < log.missoes.Count; i++)
                if (log.missoes[i] != null && log.missoes[i].questId == questId) return log.missoes[i];
            return null;
        }

        QuestState LinhaOuCria(string questId)
        {
            QuestState s = Linha(questId);
            if (s != null)
            {
                if (s.objetivosFeitos == null) s.objetivosFeitos = new List<string>();
                return s;
            }
            s = new QuestState();
            s.questId = questId;
            s.status = (int)QuestStatus.Disponivel;
            log.missoes.Add(s);
            return s;
        }

        static QuestResultado Sucesso(QuestStatus status, RecompensaDef[] recompensas)
        {
            QuestResultado r = default(QuestResultado);
            r.Ok = true; r.Erro = QuestErro.Nenhum; r.Status = status; r.Recompensas = recompensas;
            return r;
        }

        static QuestResultado Falha(QuestErro erro, QuestStatus status)
        {
            QuestResultado r = default(QuestResultado);
            r.Ok = false; r.Erro = erro; r.Status = status; r.Recompensas = QuestResultado.Nada;
            return r;
        }
    }
}
