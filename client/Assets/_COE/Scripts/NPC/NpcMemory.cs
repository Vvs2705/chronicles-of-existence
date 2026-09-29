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

    /// <summary>Memoria dos NPCs: registrar, consultar, decair. C# PURO, sem UnityEngine.
    ///
    /// REGRAS QUE VIRAM TESTE:
    /// 1. Idempotencia por (npcId, eventId): registrar duas vezes o mesmo evento nao cria dois fatos.
    ///    Recarregar o save e repetir a conversa nao inflam a memoria.
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
