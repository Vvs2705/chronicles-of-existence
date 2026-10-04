using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>O bloco do historico de vida dentro do SaveData. Nasce com padrao neutro (lista vazia): save v1
    /// antigo, sem esta chave, carrega com historico vazio. Regra de versao no cabecalho de SaveData.cs.</summary>
    [Serializable]
    public class LifeHistoryData
    {
        public List<LifeEvent> eventos = new List<LifeEvent>();
    }

    /// <summary>Historico de vida: registra, consulta e persiste os fatos com significado da vida do personagem.
    /// C# puro (nada de UnityEngine): quem grava em disco e a T004 (SaveState.Commit).
    ///
    /// IDEMPOTENCIA — o coracao da T005 (dossie §H/§M, GDD §07 "Duplicar recompensa em save/load"):
    /// `Registrar` devolve **true so na primeira vez** que aquele id aparece. Chamar de novo — por recarregar o
    /// save, por repetir o gatilho, por salvar e carregar no meio da concessao — devolve false e nao muda nada.
    /// O padrao de uso de QUALQUER recompensa unica e:
    ///
    ///     if (historia.Registrar("quest_cesto_perdido_entregue", LifeEventCategoria.Escolha, save.ageYears))
    ///     {
    ///         // conceder aqui: item, afinidade, reputacao, marco
    ///         SaveState.Commit();
    ///     }
    ///
    /// Por que isso basta: o fato e a recompensa moram no MESMO SaveData e saem numa unica gravacao atomica
    /// (T004, tmp + replace). Nao existe arquivo em que a recompensa esteja e o evento nao — ou os dois entraram,
    /// ou nenhum entrou e o jogador refaz a acao. O que NAO e coberto: alterar o save.json a mao (jogo solo,
    /// dossie §M nao promete invulnerabilidade a adulteracao local).
    ///
    /// FONTE UNICA: a verdade e SO a lista do save (Dados.eventos). Esta classe nao guarda indice proprio,
    /// entao duas instancias abertas sobre o mesmo SaveData — o ledger da T006, o salto da T009, a reputacao
    /// da T010 — enxergam os registros uma da outra e nenhuma concede de novo o que a outra ja registrou.
    ///
    /// SALTO TEMPORAL (T009, backlog "salto temporal exige confirmacao e nao duplica"): o salto e um evento como
    /// outro qualquer — `Registrar("marco_idade_8", LifeEventCategoria.Marco, 8)` vira o portao. Confirmacao do
    /// jogador primeiro, registro depois, e envelhecer/atualizar mundo so dentro do `if`. Carregar um save feito
    /// no meio da transicao reexecuta o gatilho, recebe false e nao envelhece duas vezes.
    ///
    /// LIMITE DE CRESCIMENTO: ver TetoPorCategoria.</summary>
    public class LifeEventHistory
    {
        /// <summary>Teto de eventos DETALHADOS por categoria. Passou do teto, o mais antigo daquela categoria vira
        /// "resumido": perde idade, carimbo de tempo, escopo e detalhe — e **mantem id e categoria**.
        ///
        /// Por que podar o detalhe e nunca o id: apagar um id faria `Ja(id)` mentir e a recompensa daquele evento
        /// poderia ser concedida de novo — exatamente o exploit de §H/§M que esta classe existe para impedir.
        /// Contar(categoria) tambem continua certo, porque o resumido continua na lista.
        ///
        /// Por que 100: o slice tem ~8 missoes e 10 NPCs (dossie §L), entao uma vida inteira de fatos canonicos
        /// nao chega perto disso. O teto e rede de seguranca contra um chamador que gere id por instancia
        /// ("conversa_borin_<data>"), nao o caminho normal.
        /// ponytail: o teto limita o tamanho de cada registro, nao a contagem de ids. Se um dia o historico crescer
        /// mesmo assim, o defeito esta em quem registra; a correcao e o id canonico, nao um teto menor.</summary>
        public const int TetoPorCategoria = 100;

        readonly LifeHistoryData dados;

        /// <summary>Abre o historico do save. Se o bloco faltar (save v1 antigo) ou vier null (arquivo adulterado),
        /// o padrao neutro e criado no proprio SaveData — a regra da T004: chave ausente vira padrao, nao excecao.</summary>
        public LifeEventHistory(SaveData save) : this(Bloco(save)) { }

        static LifeHistoryData Bloco(SaveData save)
        {
            if (save == null) throw new ArgumentNullException("save");
            if (save.lifeHistory == null) save.lifeHistory = new LifeHistoryData();
            return save.lifeHistory;
        }

        public LifeEventHistory(LifeHistoryData dados)
        {
            this.dados = dados ?? new LifeHistoryData();
            if (this.dados.eventos == null) this.dados.eventos = new List<LifeEvent>();

            // Save adulterado a mao pode trazer id repetido ou evento sem id. Limpar na abertura, uma vez, e o que
            // garante que Contar e Ja digam a verdade depois — e que um id duplicado no arquivo nao valha dois eventos.
            List<LifeEvent> limpos = new List<LifeEvent>(this.dados.eventos.Count);
            HashSet<string> vistos = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < this.dados.eventos.Count; i++)
            {
                LifeEvent e = this.dados.eventos[i];
                if (e == null || string.IsNullOrEmpty(e.eventId) || string.IsNullOrEmpty(e.categoria)) continue;
                if (!vistos.Add(e.eventId)) continue;
                limpos.Add(e);
            }
            this.dados.eventos = limpos;
        }

        /// <summary>O bloco persistido, para quem for gravar (T004) ou inspecionar.</summary>
        public LifeHistoryData Dados { get { return dados; } }

        public int Total { get { return dados.eventos.Count; } }

        /// <summary>Registra o fato com carimbo de agora. true = e a primeira vez (pode conceder o efeito);
        /// false = ja aconteceu antes, nada mudou.</summary>
        public bool Registrar(string id, string categoria, int idade, string escopo = null, string detalhe = null)
        {
            LifeEvent e = new LifeEvent();
            e.eventId = id;
            e.categoria = categoria;
            e.idade = idade;
            e.emUtc = DateTime.UtcNow.Ticks;
            e.escopo = escopo;
            e.detalhe = detalhe;
            return Registrar(e);
        }

        /// <summary>Mesma coisa com o carimbo vindo de fora (teste, conteudo pre-datado, migracao).
        /// Lanca se o evento nao tem id ou categoria: registro silencioso pela metade seria pior que o erro.</summary>
        public bool Registrar(LifeEvent evento)
        {
            if (evento == null) throw new ArgumentNullException("evento");
            if (string.IsNullOrEmpty(evento.eventId)) throw new ArgumentException("LifeEvent sem id: o id e a chave de idempotencia.", "evento");
            if (string.IsNullOrEmpty(evento.categoria)) throw new ArgumentException("LifeEvent '" + evento.eventId + "' sem categoria.", "evento");

            if (Ja(evento.eventId)) return false;  // JA ACONTECEU: nao duplica e nao concede de novo

            evento.escopo = evento.escopo ?? "";
            evento.detalhe = evento.detalhe ?? "";
            evento.resumido = false;
            dados.eventos.Add(evento);
            Podar(evento.categoria);
            return true;
        }

        /// <summary>Aquele fato ja aconteceu nesta vida? A pergunta que T006/T007/T010 fazem antes de conceder.
        /// ponytail: varre a lista do save a cada consulta (O(n), n = fatos canonicos, dezenas). Indice por
        /// instancia foi removido de proposito: duas instancias no mesmo save discordavam e pagavam duas vezes.
        /// Se um dia doer, o indice vai [NonSerialized] dentro de LifeHistoryData (um por bloco), nunca por instancia.</summary>
        public bool Ja(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            for (int i = 0; i < dados.eventos.Count; i++)
                if (dados.eventos[i].eventId == id) return true;
            return false;
        }

        /// <summary>Quantos fatos daquela categoria, resumidos inclusive.</summary>
        public int Contar(string categoria)
        {
            int n = 0;
            for (int i = 0; i < dados.eventos.Count; i++) if (dados.eventos[i].categoria == categoria) n++;
            return n;
        }

        /// <summary>O mais recente daquela categoria, ou null. Ex.: Ultimo(Relacao). Cuidado com Marco: missao e
        /// recompensa (HistoricoDeVidaLedger) tambem gravam ali; para "o salto ja houve?" use Ja(id do marco).</summary>
        public LifeEvent Ultimo(string categoria)
        {
            for (int i = dados.eventos.Count - 1; i >= 0; i--)
                if (dados.eventos[i].categoria == categoria) return dados.eventos[i];
            return null;
        }

        /// <summary>Tudo, em ordem cronologica de registro (a lista nunca e reordenada). Somente leitura:
        /// mexer no historico e sempre por Registrar, senao o indice de ids e a lista discordam.</summary>
        public IList<LifeEvent> Todos()
        {
            return dados.eventos.AsReadOnly();
        }

        /// <summary>Os fatos ligados a um NPC ou missao, em ordem — a "consulta por NPC/quest" que o GDD §12 pede
        /// de T005 e que T007 (memoria de NPC) e T010 (reputacao) consomem. Evento resumido perde o escopo e por
        /// isso some daqui; ele continua contando em Ja e Contar.
        /// Chaves canonicas (nao concatene prefixo na mao): por NPC, ReputationAlvo.Npc("borin") = "npc_borin";
        /// por missao, QuestSystem.Escopo(questId) = "quest_q03_o_cesto_perdido". Um evento tem UM escopo so.</summary>
        public List<LifeEvent> PorEscopo(string escopo)
        {
            List<LifeEvent> r = new List<LifeEvent>();
            if (string.IsNullOrEmpty(escopo)) return r;
            for (int i = 0; i < dados.eventos.Count; i++)
                if (dados.eventos[i].escopo == escopo) r.Add(dados.eventos[i]);
            return r;
        }

        /// <summary>Mantem no maximo TetoPorCategoria eventos detalhados daquela categoria, resumindo os mais
        /// antigos. ponytail: varredura da lista inteira a cada registro; a lista e curta por construcao
        /// (fatos canonicos). Se um dia doer, o proximo passo e um indice por categoria, nao um teto menor.</summary>
        void Podar(string categoria)
        {
            int detalhados = 0;
            for (int i = dados.eventos.Count - 1; i >= 0; i--)
            {
                LifeEvent e = dados.eventos[i];
                if (e.resumido || e.categoria != categoria) continue;
                detalhados++;
                if (detalhados <= TetoPorCategoria) continue;
                e.resumido = true;
                e.idade = 0;
                e.emUtc = 0;
                e.escopo = "";
                e.detalhe = "";
            }
        }
    }
}
