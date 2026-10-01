using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>Tudo que o dialogo pode LER do jogo. So leitura: nenhum campo aqui permite escrever.
    /// Os delegates existem para o dialogo nao compilar contra Quest nem Reputacao -- quem monta o
    /// contexto liga as pontas (QuestIntentAdapter.Leitor para T006; T010 preenche Confianca). Ausente =
    /// neutro: missao sem estado e Indisponivel, confianca e 0.</summary>
    public sealed class DialogueContext
    {
        public string NpcId = "";
        public TimeOfDay Periodo = TimeOfDay.Manha;   // periodo canonico da T009
        public NpcBook Memoria;                       // null = NPC nao lembra de nada
        public Func<string, int> EstadoDaMissao;      // questId -> EstadoMissao.*
        public Func<string, int> Confianca;           // npcId -> -100..100 (T010: ReputationSystem.ConfiancaNo)

        public int Missao(string questId)
        {
            if (EstadoDaMissao == null || string.IsNullOrEmpty(questId)) return EstadoMissao.Indisponivel;
            return EstadoDaMissao(questId);
        }

        public int ConfiancaCom(string npcId)
        {
            return Confianca == null || string.IsNullOrEmpty(npcId) ? 0 : Confianca(npcId);
        }
    }

    /// <summary>O que uma escolha do jogador produziu. Proximo == null com Encerrou == true e fim normal
    /// da conversa. Pedido != null e um PEDIDO ao sistema de missao: este struct nao aplicou nada.</summary>
    public struct DialogueStep
    {
        public DialogueNode Proximo;
        public PedidoDeMissao Pedido;
        public bool Encerrou;
        public bool Ok;          // false = escolha invalida (indice fora, opcao nao disponivel); nada mudou
    }

    /// <summary>PONTO DE EXTENSAO para IA generativa. Um adaptador que implemente isto pode reescrever a
    /// FALA, e so isso: a assinatura devolve string, entao nao ha como conceder item, missao ou
    /// reputacao por aqui (prompt-mestre secao 9; CLAUDE.md: "IA generativa em runtime com autoridade
    /// sobre estado" e proibida). Devolver null/vazio cai no texto deterministico.
    ///
    /// Nao ha implementacao nesta tarefa, de proposito: conteudo essencial funciona offline. Quando uma
    /// implementacao existir, o texto que ela devolve e CONTEUDO NAO CONFIAVEL -- nunca instrucao.
    /// Pedido estruturado gerado por IA, se um dia existir, entra pelo mesmo PedidoDeMissao +
    /// QuestIntentAdapter que o dado autoral usa (a allowlist QuestIntent.TryParse da T006 existe
    /// exatamente para isso); nao ganha caminho proprio.</summary>
    public interface IFalaGenerativa
    {
        string Reescrever(string npcId, string noId, string textoDeterministico);
    }

    /// <summary>Roda um grafo de dialogo. C# PURO e SEM ESTADO: toda funcao recebe grafo + contexto e
    /// devolve resposta. Nao grava save, nao toca missao, nao toca inventario, nao toca memoria --
    /// registrar memoria e chamada explicita de quem orquestra, depois que o sistema dono validou.</summary>
    public static class DialogueRunner
    {
        /// <summary>O no de entrada: o primeiro cuja condicao passa, na ordem de declaracao. Para um grafo
        /// que passou em Validar(), nunca devolve null (existe no incondicional).</summary>
        public static DialogueNode Entrada(DialogueGraph grafo, DialogueContext ctx)
        {
            if (grafo == null) return null;
            for (int i = 0; i < grafo.Nos.Length; i++)
                if (Satisfaz(grafo.Nos[i].Condicao, grafo.NpcId, ctx)) return grafo.Nos[i];
            return null;
        }

        /// <summary>As opcoes visiveis agora, na ordem de declaracao. Array vazio, nunca null.</summary>
        public static DialogueOption[] Opcoes(DialogueGraph grafo, DialogueNode no, DialogueContext ctx)
        {
            List<DialogueOption> r = new List<DialogueOption>();
            if (no != null)
                for (int i = 0; i < no.Opcoes.Length; i++)
                    if (Satisfaz(no.Opcoes[i].Condicao, grafo == null ? null : grafo.NpcId, ctx)) r.Add(no.Opcoes[i]);
            return r.ToArray();
        }

        /// <summary>O jogador escolheu a opcao visivel de indice <paramref name="indiceVisivel"/>.
        ///
        /// NAO MUDA ESTADO. Devolve para onde a conversa vai e, se houver, o PedidoDeMissao que a fala
        /// emitiu. Iniciar, avancar e concluir missao acontecem quando o QuestSystem valida o pedido
        /// (QuestIntentAdapter.Despachar) -- nunca aqui.</summary>
        public static DialogueStep Escolher(DialogueGraph grafo, DialogueNode no, int indiceVisivel, DialogueContext ctx)
        {
            DialogueStep s = default(DialogueStep);
            DialogueOption[] visiveis = Opcoes(grafo, no, ctx);
            if (grafo == null || no == null || indiceVisivel < 0 || indiceVisivel >= visiveis.Length) return s;

            DialogueOption o = visiveis[indiceVisivel];
            s.Ok = true;
            s.Pedido = o.Pedido;
            s.Proximo = string.IsNullOrEmpty(o.ProximoNoId) ? null : grafo.No(o.ProximoNoId);
            s.Encerrou = s.Proximo == null;
            return s;
        }

        /// <summary>A fala do no. Sempre deterministica; <paramref name="estilo"/> so pode trocar o texto,
        /// e se falhar ou devolver vazio o texto deterministico continua valendo (fallback offline).</summary>
        public static string Fala(DialogueNode no, DialogueContext ctx, IFalaGenerativa estilo)
        {
            if (no == null) return "";
            string texto = Strings.Get(no.TextoKey);
            if (estilo == null) return texto;
            try
            {
                string gerado = estilo.Reescrever(ctx == null ? null : ctx.NpcId, no.Id, texto);
                return string.IsNullOrEmpty(gerado) ? texto : gerado;
            }
            catch (Exception) { return texto; }   // adaptador quebrado nunca trava a conversa
        }

        /// <summary>Avalia uma condicao. Sem efeito colateral: chamar duas vezes da o mesmo resultado
        /// e nao escreve em lugar nenhum.</summary>
        public static bool Satisfaz(Condicao c, string npcId, DialogueContext ctx)
        {
            if (c == null) return true;
            string alvo = npcId;
            if (string.IsNullOrEmpty(alvo) && ctx != null) alvo = ctx.NpcId;

            switch (c.Tipo)
            {
                case CondicaoTipo.Sempre: return true;
                case CondicaoTipo.NoPeriodo: return ctx != null && (int)ctx.Periodo == c.Numero;
                case CondicaoTipo.Lembra: return ctx != null && NpcMemory.Lembra(ctx.Memoria, alvo, c.Chave);
                case CondicaoTipo.NaoLembra: return ctx == null || !NpcMemory.Lembra(ctx.Memoria, alvo, c.Chave);
                case CondicaoTipo.SabeTopico: return NpcCatalog.Sabe(alvo, c.Chave);
                case CondicaoTipo.MissaoEmEstado: return ctx != null && ctx.Missao(c.Chave) == c.Numero;
                case CondicaoTipo.ConfiancaMinima: return ctx != null && ctx.ConfiancaCom(alvo) >= c.Numero;
                case CondicaoTipo.Todas:
                    foreach (Condicao parte in c.Partes) if (!Satisfaz(parte, npcId, ctx)) return false;
                    return c.Partes.Length > 0;
                default: return false;   // tipo novo sem tratamento esconde a opcao em vez de liberar
            }
        }
    }
}
