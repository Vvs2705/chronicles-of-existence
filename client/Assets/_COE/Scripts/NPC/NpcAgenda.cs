namespace COE
{
    /// <summary>Algo que tira o NPC da rotina: o jogador puxou conversa, ou um evento da vila exige ele em
    /// outro lugar (GDD cap. 06: "prioridades para interrupcao por eventos (ex.: incendio)"). Dado imutavel.</summary>
    public sealed class Interrupcao
    {
        public const int PrioridadeConversa = 1;
        public const int PrioridadeEvento = 2;   // evento da vila passa na frente de conversa

        public readonly string Id;            // quem abriu fecha pelo mesmo id (NpcAgenda.Retomar)
        public readonly int Prioridade;       // maior vence; empate nao troca -- quem chegou primeiro fica
        public readonly string AncoraId;      // null = fica onde esta (conversa)
        public readonly string AtividadeKey;

        public Interrupcao(string id, int prioridade, string ancoraId, string atividadeKey)
        {
            Id = id; Prioridade = prioridade; AncoraId = ancoraId; AtividadeKey = atividadeKey;
        }

        /// <summary>A interrupcao do dialogo: o NPC para onde esta. O orquestrador da conversa chama
        /// Interromper(Conversa) ao abrir e Retomar(Conversa.Id) ao fechar.</summary>
        public static readonly Interrupcao Conversa =
            new Interrupcao("conversa", PrioridadeConversa, null, "atividade.conversar");
    }

    /// <summary>O que UM NPC esta fazendo agora: a interrupcao em curso, se houver; senao a rotina do periodo
    /// (NpcDef.Onde, ja condicional por memoria). C# PURO, sem UnityEngine: o MonoBehaviour do NPC so
    /// pergunta Agora() e anda ate a ancora.
    ///
    /// ESTADO TRANSITORIO, fora do save de proposito: conversa e evento da vila acabam antes de alguem
    /// salvar, e carregar devolve todo mundo a rotina, que e o padrao neutro certo.
    ///
    /// RETOMAR NAO RESTAURA FOTO: acabada a interrupcao, o NPC volta a rotina do periodo ATUAL. Se a conversa
    /// atravessou manha -> tarde, ele vai para o lugar da tarde, nao para onde estava.
    ///
    /// ponytail: uma interrupcao por vez, sem pilha. A mais forte substitui a mais fraca, que se perde (a
    /// conversa cortada por um incendio nao volta sozinha). Pilha quando dois eventos simultaneos precisarem
    /// sobreviver um ao outro.</summary>
    public sealed class NpcAgenda
    {
        public readonly NpcDef Npc;
        Interrupcao atual;

        /// <summary>npc null e aceito: Agora() devolve null, como NpcCatalog.Onde para id desconhecido.</summary>
        public NpcAgenda(NpcDef npc) { Npc = npc; }

        /// <summary>A interrupcao em curso; null = seguindo a rotina.</summary>
        public Interrupcao Atual { get { return atual; } }

        /// <summary>true = interrompeu. false = ja existe interrupcao de prioridade igual ou maior (o NPC esta
        /// ocupado: a UI mostra isso em vez de abrir a conversa) ou entrada invalida. Nada muda no false.</summary>
        public bool Interromper(Interrupcao i)
        {
            if (i == null || string.IsNullOrEmpty(i.Id)) return false;
            if (atual != null && atual.Prioridade >= i.Prioridade) return false;
            atual = i;
            return true;
        }

        /// <summary>Encerra a interrupcao SO se ela ainda for a de id informado: a conversa que fecha depois
        /// de um incendio a ter substituido nao cancela o incendio. true = voltou a rotina.</summary>
        public bool Retomar(string interrupcaoId)
        {
            if (atual == null || atual.Id != interrupcaoId) return false;
            atual = null;
            return true;
        }

        /// <summary>Onde o NPC deve estar e o que faz agora. Com interrupcao, AncoraId null = parado onde esta.</summary>
        public RotinaEntrada Agora(TimeOfDay periodo, NpcBook memoria)
        {
            if (atual != null) return new RotinaEntrada(periodo, atual.AncoraId, atual.AtividadeKey);
            return Npc == null ? null : Npc.Onde(periodo, memoria);
        }
    }
}
