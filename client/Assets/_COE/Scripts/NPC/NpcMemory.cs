using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>Quanto o fato pesa para o NPC. Decide o que decai e o que fica para sempre.</summary>
    public enum Importancia
    {
        Trivial = 1,   // favor pequeno, recado, conversa util -- vira numero quando o espaco acaba
        Notavel = 2,   // mudou o dia do NPC
        Marcante = 3,  // nunca esquece: morte, traicao, salvamento, promessa quebrada
    }

    /// <summary>Um fato com significado que UM NPC guarda. NAO e transcricao: nao existe campo de texto
    /// de fala aqui, de proposito (dossie secao G, prompt-mestre secao 9).
    ///
    /// eventId APONTA para o evento canonico do LifeEventHistory (T005), que e dona do historico e da
    /// idempotencia por id. Aqui fica so "quem lembra de que, e quanto isso pesou" -- o historico inteiro
    /// nao e copiado para dentro do NPC.</summary>
    [Serializable]
    public class NpcMemoryFact
    {
        public string npcId = "";
        public string eventId = "";      // id do LifeEvent (T005); a chave de idempotencia e (npcId, eventId)
        public int importancia = (int)Importancia.Trivial;
        public long registradoEmUtc;     // ticks UTC; ordem de chegada, nao relogio de jogo
    }

    /// <summary>O que sobrou depois que os fatos triviais de um NPC decairam. Guarda a SENSACAO sem a
    /// lista: "voce ja me ajudou N vezes". Uma linha por NPC.</summary>
    [Serializable]
    public class NpcMemorySummary
    {
        public string npcId = "";
        public int esquecidos;   // quantos fatos triviais foram sumarizados
    }

    /// <summary>Bloco de save da T007 (contrato NPCState do GDD cap. 10: npcId + memoryEventIds).
    /// Nasce vazio = NPC nao lembra de nada, que e o padrao neutro correto para save antigo.</summary>
    [Serializable]
    public class NpcBook
    {
        public List<NpcMemoryFact> fatos = new List<NpcMemoryFact>();
        public List<NpcMemorySummary> resumos = new List<NpcMemorySummary>();
    }

    /// <summary>Quem presenciou um evento canonico do historico de vida e, por isso, passa a lembrar dele.
    /// DEFINICAO imutavel, como NpcDef.</summary>
    public sealed class Testemunho
    {
        public readonly string EventoId;
        public readonly Importancia Importancia;
        public readonly string[] Npcs;

        public Testemunho(string eventoId, Importancia importancia, params string[] npcs)
        {
            EventoId = eventoId; Importancia = importancia; Npcs = npcs ?? new string[0];
        }
    }

    /// <summary>Memoria dos NPCs: registrar, consultar, decair. C# PURO, sem UnityEngine.
    ///
    /// REGRAS QUE VIRAM TESTE:
    /// 1. Idempotencia por (npcId, eventId): registrar duas vezes o mesmo evento nao cria dois fatos.
    ///    Recarregar o save e repetir a conversa nao inflam a memoria. Limite conhecido: um fato TRIVIAL que
    ///    ja decaiu perdeu o id, entao registra-lo de novo entra como fato novo. Por isso Testemunhos nunca e
    ///    Trivial -- Sincronizar roda a cada carga e nao pode inflar o resumo.
    /// 2. Marcante nunca decai. Trivial decai por CAPACIDADE, nao por relogio -- assim o resultado e
    ///    deterministico e testavel sem simular tempo.
    /// 3. Nada de texto de fala. O que o NPC "lembra" e um id de evento; a fala e reconstruida pelo
    ///    dialogo a partir do id.</summary>
    public static class NpcMemory
    {
        /// <summary>Quantos fatos TRIVIAIS um NPC carrega antes do mais antigo virar numero no resumo.
        /// ponytail: limite fixo e por NPC. Se um dia precisar variar por personalidade (Eira lembra mais
        /// que Nilo), o lugar e um campo em NpcDef -- nao um sistema de esquecimento.</summary>
        public const int LimiteTrivialPorNpc = 5;

        /// <summary>QUEM LEMBRA DE QUE. Copia em C# de content/quests/*.json -> registra_no_historico[].npcs,
        /// guardada por NpcDataParityTests -- o mesmo regime do ADR-0005 para o QuestCatalog: o C# e o que o
        /// jogo le, o JSON e o que o redator le, e o teste fica vermelho se um lado mudar sozinho.
        /// evento.q08_concluida nao entra: ninguem de Auren esta na clareira (npcs vazio no JSON).
        ///
        /// HIPOTESE v0: a importancia. Promessa cumprida/quebrada e Marcante pela propria definicao do enum;
        /// o resto e Notavel. As duas nao decaem hoje, entao a diferenca so pesa quando alguem ler importancia.
        /// ponytail: tabela em codigo pelo mesmo motivo do NpcCatalog; vai para JSON junto com ele.</summary>
        public static readonly Testemunho[] Testemunhos =
        {
            new Testemunho("evento.q01_concluida", Importancia.Notavel, "mara", "daren"),
            new Testemunho("evento.q01_familia_apresentada", Importancia.Notavel, "mara", "daren"),
            new Testemunho("evento.q02_concluida", Importancia.Notavel, "daren", "oren"),
            new Testemunho("evento.q03_concluida", Importancia.Notavel, "oren"),
            new Testemunho("evento.q04_concluida", Importancia.Notavel, "sera", "nilo"),
            new Testemunho("evento.q04_promessa_cumprida", Importancia.Marcante, "sera", "nilo"),
            new Testemunho("evento.q04_promessa_quebrada", Importancia.Marcante, "sera", "nilo"),
            // ADR-0007 §3. Nilo: e a lembranca que liga a rotina "ausente" dele. Sera: estava com ele na promessa.
            // Maelis: e quem da a noticia na Q-07 (notar_a_ausencia), entao tem de saber antes do jogador.
            new Testemunho(QuestCatalog.EventoNiloDesapareceu, Importancia.Marcante, "nilo", "sera", "maelis"),
            new Testemunho("evento.q05_concluida", Importancia.Notavel, "lysa"),
            new Testemunho("evento.q06_concluida", Importancia.Notavel, "borin"),
            new Testemunho("evento.q07_concluida", Importancia.Notavel, "maelis", "tovin"),
        };

        /// <summary>O testemunho daquele evento, ou null se nenhum NPC o presenciou.</summary>
        public static Testemunho TestemunhoDe(string eventoId)
        {
            for (int i = 0; i < Testemunhos.Length; i++) if (Testemunhos[i].EventoId == eventoId) return Testemunhos[i];
            return null;
        }

        /// <summary>true se este NPC passa a lembrar do evento quando ele entra no historico. E o que um dado
        /// de dialogo (Condicao.Lembra) ou de rotina (RotinaEntrada.SeLembra) pode citar.</summary>
        public static bool Testemunha(string npcId, string eventoId)
        {
            Testemunho t = TestemunhoDe(eventoId);
            return t != null && npcId != null && Array.IndexOf(t.Npcs, npcId) >= 0;
        }

        /// <summary>Traz para a memoria dos NPCs o que o historico de vida (T005, fonte unica) diz que eles
        /// testemunharam. E assim que missao concluida vira lembranca: QuestSystem.Concluir grava
        /// evento.qNN_concluida no historico, e isto le. Um NPC nunca "lembra" de evento ausente do historico
        /// daquele save (SLICE_A secao 4.3, regra negativa).
        ///
        /// QUEM CHAMA: o orquestrador, ao carregar o save e depois de cada operacao que grave no historico
        /// (missao concluida, desfecho da q04). Chamada explicita, sem event bus.
        /// IDEMPOTENTE: chave (npcId, eventId) e nenhum testemunho e Trivial, entao chamar de novo -- ou
        /// recarregar e chamar -- nao duplica fato nem mexe no resumo. Devolve quantos fatos entraram agora.
        ///
        /// Le Todos() e nao PorEscopo(): evento podado pelo teto perde o escopo mas mantem o id, e o NPC tem
        /// de continuar lembrando dele.</summary>
        public static int Sincronizar(NpcBook book, LifeEventHistory historia)
        {
            if (book == null || historia == null) return 0;
            int novos = 0;
            IList<LifeEvent> eventos = historia.Todos();
            for (int i = 0; i < eventos.Count; i++)
            {
                Testemunho t = TestemunhoDe(eventos[i].eventId);
                if (t == null) continue;
                for (int j = 0; j < t.Npcs.Length; j++)
                    if (Registrar(book, t.Npcs[j], t.EventoId, t.Importancia, eventos[i].emUtc)) novos++;
            }
            return novos;
        }

        /// <summary>Registra um fato. false quando nao registrou: entrada invalida OU ja lembrado
        /// (idempotencia). Nunca lanca, nunca duplica.</summary>
        public static bool Registrar(NpcBook book, string npcId, string eventId, Importancia importancia, long utcTicks)
        {
            if (book == null || string.IsNullOrEmpty(npcId) || string.IsNullOrEmpty(eventId)) return false;
            if (Lembra(book, npcId, eventId)) return false;

            NpcMemoryFact f = new NpcMemoryFact();
            f.npcId = npcId;
            f.eventId = eventId;
            f.importancia = (int)importancia;
            f.registradoEmUtc = utcTicks;
            book.fatos.Add(f);

            Decair(book, npcId);
            return true;
        }

        public static bool Lembra(NpcBook book, string npcId, string eventId)
        {
            if (book == null || npcId == null || eventId == null) return false;
            for (int i = 0; i < book.fatos.Count; i++)
            {
                NpcMemoryFact f = book.fatos[i];
                if (f != null && f.npcId == npcId && f.eventId == eventId) return true;
            }
            return false;
        }

        /// <summary>Os fatos que este NPC ainda guarda, na ordem em que chegaram. Array vazio, nunca null.</summary>
        public static NpcMemoryFact[] Fatos(NpcBook book, string npcId)
        {
            List<NpcMemoryFact> r = new List<NpcMemoryFact>();
            if (book != null && npcId != null)
                for (int i = 0; i < book.fatos.Count; i++)
                    if (book.fatos[i] != null && book.fatos[i].npcId == npcId) r.Add(book.fatos[i]);
            return r.ToArray();
        }

        /// <summary>Quantos fatos triviais deste NPC ja viraram resumo ("voce ja me ajudou varias vezes").</summary>
        public static int Esquecidos(NpcBook book, string npcId)
        {
            NpcMemorySummary s = Resumo(book, npcId);
            return s == null ? 0 : s.esquecidos;
        }

        static NpcMemorySummary Resumo(NpcBook book, string npcId)
        {
            if (book == null || npcId == null) return null;
            for (int i = 0; i < book.resumos.Count; i++)
                if (book.resumos[i] != null && book.resumos[i].npcId == npcId) return book.resumos[i];
            return null;
        }

        /// <summary>Sumariza o trivial mais antigo enquanto este NPC passar do limite. Notavel e Marcante
        /// ficam: a memoria encolhe pelo que NAO tinha significado.</summary>
        static void Decair(NpcBook book, string npcId)
        {
            while (true)
            {
                int triviais = 0;
                int alvo = -1;
                for (int i = 0; i < book.fatos.Count; i++)
                {
                    NpcMemoryFact f = book.fatos[i];
                    if (f == null || f.npcId != npcId || f.importancia != (int)Importancia.Trivial) continue;
                    triviais++;
                    if (alvo < 0) alvo = i;   // o primeiro da lista e o mais antigo
                }
                if (triviais <= LimiteTrivialPorNpc || alvo < 0) return;

                book.fatos.RemoveAt(alvo);
                NpcMemorySummary s = Resumo(book, npcId);
                if (s == null)
                {
                    s = new NpcMemorySummary();
                    s.npcId = npcId;
                    book.resumos.Add(s);
                }
                s.esquecidos++;
            }
        }
    }
}
