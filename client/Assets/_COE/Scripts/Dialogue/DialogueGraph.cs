using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>O que a condicao olha. Tudo aqui e ESTADO CANONICO DO JOGO (prompt-mestre secao 9):
    /// periodo do dia, memoria do NPC, conhecimento do NPC, estado de missao, confianca e o nascimento
    /// (destino, origem e o que a crianca carrega). Valores novos entram no FIM: o numero nao muda.</summary>
    public enum CondicaoTipo
    {
        Sempre = 0,        // o fallback offline; toda conversa precisa de pelo menos um
        NoPeriodo,         // Numero = (int)TimeOfDay
        Lembra,            // Chave = eventId do historico de vida (T005)
        NaoLembra,         // Chave = eventId
        SabeTopico,        // Chave = topicoId (conhecimento limitado do NPC)
        MissaoEmEstado,    // Chave = questId, Numero = EstadoMissao.* (T006 responde)
        ConfiancaMinima,   // Numero = minimo (T010 responde; sem T010 vale 0)
        Todas,             // Partes = todas tem de valer (B14: "tem 8 anos E lembra da promessa cumprida")
        ObjetivoProximo,   // Chave = "questId/objetivoId": missao em andamento e este e o proximo objetivo pendente
        Destino,           // Chave = destinyId do DestinyCatalog (C8 das fichas G1); sem nascimento nunca passa
        Origem,            // Chave = originId do DestinyCatalog; sem nascimento nunca passa
        TemItem,           // Chave = itemId: passa com quantidade > 0 no inventario; sem leitor nunca passa
    }

    /// <summary>Uma condicao de dado, avaliada sem efeito colateral. Imutavel.</summary>
    public sealed class Condicao
    {
        public static readonly Condicao Sempre = new Condicao(CondicaoTipo.Sempre, null, 0);

        public readonly CondicaoTipo Tipo;
        public readonly string Chave;
        public readonly int Numero;
        public readonly Condicao[] Partes;   // so em Todas

        public Condicao(CondicaoTipo tipo, string chave, int numero, Condicao[] partes = null)
        {
            Tipo = tipo; Chave = chave; Numero = numero; Partes = partes ?? new Condicao[0];
        }

        public static Condicao Periodo(TimeOfDay p) { return new Condicao(CondicaoTipo.NoPeriodo, null, (int)p); }
        public static Condicao Lembra(string eventId) { return new Condicao(CondicaoTipo.Lembra, eventId, 0); }
        public static Condicao NaoLembra(string eventId) { return new Condicao(CondicaoTipo.NaoLembra, eventId, 0); }
        public static Condicao Sabe(string topicoId) { return new Condicao(CondicaoTipo.SabeTopico, topicoId, 0); }
        public static Condicao Missao(string questId, int estado) { return new Condicao(CondicaoTipo.MissaoEmEstado, questId, estado); }
        public static Condicao Confianca(int minimo) { return new Condicao(CondicaoTipo.ConfiancaMinima, null, minimo); }
        public static Condicao E(params Condicao[] partes) { return new Condicao(CondicaoTipo.Todas, null, 0, partes); }
        public static Condicao Objetivo(string questId, string objetivoId) { return new Condicao(CondicaoTipo.ObjetivoProximo, questId + "/" + objetivoId, 0); }
        public static Condicao Destino(string destinyId) { return new Condicao(CondicaoTipo.Destino, destinyId, 0); }
        public static Condicao Origem(string originId) { return new Condicao(CondicaoTipo.Origem, originId, 0); }
        public static Condicao TemItem(string itemId) { return new Condicao(CondicaoTipo.TemItem, itemId, 0); }
    }

    /// <summary>Uma resposta do jogador. ProximoNoId vazio/null encerra a conversa -- isso e uma saida
    /// valida, nao um beco. Pedido != null e o que a fala PEDE ao sistema de missao; quem aplica e o
    /// QuestIntentAdapter, nunca esta classe.</summary>
    public sealed class DialogueOption
    {
        public readonly string TextoKey;
        public readonly Condicao Condicao;
        public readonly string ProximoNoId;
        public readonly PedidoDeMissao Pedido;   // null = a opcao so conversa

        public DialogueOption(string textoKey, Condicao condicao, string proximoNoId, PedidoDeMissao pedido)
        {
            TextoKey = textoKey; Condicao = condicao ?? COE.Condicao.Sempre;
            ProximoNoId = proximoNoId; Pedido = pedido;
        }
    }

    /// <summary>Um no de fala. Opcoes vazias = no terminal (o NPC fala e a conversa acaba).</summary>
    public sealed class DialogueNode
    {
        public readonly string Id;
        public readonly string TextoKey;
        public readonly Condicao Condicao;         // quando este no pode ser a ENTRADA da conversa
        public readonly DialogueOption[] Opcoes;

        public DialogueNode(string id, string textoKey, Condicao condicao, DialogueOption[] opcoes)
        {
            Id = id; TextoKey = textoKey; Condicao = condicao ?? COE.Condicao.Sempre;
            Opcoes = opcoes ?? new DialogueOption[0];
        }
    }

    /// <summary>Grafo de dialogo de um NPC: dado puro, deterministico, sem IA. A entrada e escolhida
    /// percorrendo Nos NA ORDEM e ficando no primeiro cuja condicao passa; por isso o no mais especifico
    /// vem primeiro e o fallback vem por ultimo.
    ///
    /// FALLBACK OFFLINE OBRIGATORIO: Validar() recusa grafo sem no de entrada incondicional e recusa no
    /// que tenha opcoes mas nenhuma opcao incondicional -- se toda opcao depende de condicao, existe um
    /// estado de jogo em que o jogador fica preso sem nada para clicar. Beco sem saida vira erro de dado,
    /// nao bug de playtest.</summary>
    public sealed class DialogueGraph
    {
        public readonly string Id;
        public readonly string NpcId;
        public readonly DialogueNode[] Nos;

        public DialogueGraph(string id, string npcId, DialogueNode[] nos)
        {
            Id = id; NpcId = npcId; Nos = nos ?? new DialogueNode[0];
        }

        public DialogueNode No(string id)
        {
            for (int i = 0; i < Nos.Length; i++) if (Nos[i].Id == id) return Nos[i];
            return null;
        }

        /// <summary>Lista de problemas do dado. Array vazio = grafo utilizavel. Nunca lanca.</summary>
        public string[] Validar()
        {
            List<string> erros = new List<string>();
            string onde = "grafo " + Id + ": ";

            if (Nos.Length == 0) { erros.Add(onde + "sem nos"); return erros.ToArray(); }
            if (string.IsNullOrEmpty(NpcId) || NpcCatalog.Npc(NpcId) == null)
                erros.Add(onde + "npcId desconhecido: " + NpcId);

            HashSet<string> ids = new HashSet<string>();
            foreach (DialogueNode n in Nos)
            {
                if (string.IsNullOrEmpty(n.Id)) { erros.Add(onde + "no sem id"); continue; }
                if (!ids.Add(n.Id)) erros.Add(onde + "id de no repetido: " + n.Id);
            }

            bool temEntradaIncondicional = false;
            foreach (DialogueNode n in Nos)
                if (n.Condicao.Tipo == CondicaoTipo.Sempre) temEntradaIncondicional = true;
            if (!temEntradaIncondicional)
                erros.Add(onde + "nenhum no serve de entrada incondicional (sem fallback offline)");

            foreach (DialogueNode n in Nos)
            {
                bool temSaidaIncondicional = n.Opcoes.Length == 0;   // no terminal ja e saida
                foreach (DialogueOption o in n.Opcoes)
                {
                    if (o.Condicao.Tipo == CondicaoTipo.Sempre) temSaidaIncondicional = true;
                    if (!string.IsNullOrEmpty(o.ProximoNoId) && No(o.ProximoNoId) == null)
                        erros.Add(onde + "no " + n.Id + " aponta para no inexistente: " + o.ProximoNoId);
                    if (o.Pedido != null && string.IsNullOrEmpty(o.Pedido.QuestId))
                        erros.Add(onde + "no " + n.Id + " tem pedido sem missao");
                }
                if (!temSaidaIncondicional)
                    erros.Add(onde + "no " + n.Id + " e beco sem saida: tem opcoes, mas todas condicionais");
            }

            return erros.ToArray();
        }
    }
}
