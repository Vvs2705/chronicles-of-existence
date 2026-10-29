using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>Quantos de um item o jogador tem. itemId e o id estavel da recompensa (ex.: "item.cesto_de_vime").</summary>
    [Serializable]
    public class ItemPilha
    {
        public string itemId = "";
        public int quantidade;
    }

    /// <summary>BLOCO NOVO do save (T012): moedas e itens por id. Padrao neutro (zero, listas vazias) = exatamente o
    /// que um save anterior a esta tarefa significa: save v1 sem a chave carrega com inventario vazio (cabecalho de SaveData.cs).
    ///
    /// recompensasAplicadas: ids rec.* que JA entraram aqui. E a segunda linha de defesa da idempotencia (a primeira e
    /// o QuestSystem, que so concede uma vez por id no historico): aplicar o mesmo id de novo nao soma nada, venha de
    /// onde vier (gatilho, dialogo, recarregar). Mora no MESMO arquivo que o historico e grava atomicamente com ele,
    /// entao nao discorda do historico num crash; no maximo fica atras, e Inventario.Sincronizar alcanca.</summary>
    [Serializable]
    public class InventarioData
    {
        public int moedas;
        public List<ItemPilha> itens = new List<ItemPilha>();
        public List<string> recompensasAplicadas = new List<string>();
    }

    /// <summary>Aplica recompensa de missao no inventario. C# PURO. So moedas e item: marco nao e posse, o
    /// QuestSystem ja o gravou no historico de vida (QuestCatalog, RecompensaDef).
    /// ponytail: item e so id + quantidade, sem definicao (ScriptableObject de item, README do modulo). Entra quando
    /// item precisar de nome, icone ou uso; o id ja e o estavel.</summary>
    public static class Inventario
    {
        /// <summary>Soma a recompensa UMA vez por id. false = marco, nula, sem id ou ja aplicada (nada muda).</summary>
        public static bool Aplicar(InventarioData inv, RecompensaDef r)
        {
            if (inv == null || r == null || string.IsNullOrEmpty(r.Id)) return false;
            if (r.Tipo != QuestCatalog.TipoMoedas && r.Tipo != QuestCatalog.TipoItem) return false;
            if (inv.recompensasAplicadas == null) inv.recompensasAplicadas = new List<string>();
            if (inv.recompensasAplicadas.Contains(r.Id)) return false;

            if (r.Tipo == QuestCatalog.TipoMoedas) inv.moedas += r.Quantidade;
            else Pilha(inv, r.Alvo).quantidade += r.Quantidade;
            inv.recompensasAplicadas.Add(r.Id);
            return true;
        }

        /// <summary>Moedas e itens iniciais do nascimento (Circunstancia: destino + origem), com ids de transacao
        /// estaveis "rec.nascimento.*": idempotente como qualquer recompensa. Devolve quantas entraram.</summary>
        public static int Nascer(InventarioData inv, Circunstancia c)
        {
            int n = 0;
            if (c.MoedasIniciais > 0 && Aplicar(inv, new RecompensaDef("rec.nascimento.moedas", QuestCatalog.TipoMoedas, "", c.MoedasIniciais))) n++;
            if (c.ItensIniciais != null)
                foreach (string item in c.ItensIniciais)
                    if (Aplicar(inv, new RecompensaDef("rec.nascimento." + item, QuestCatalog.TipoItem, item, 1))) n++;
            return n;
        }

        /// <summary>O inventario alcanca o historico: toda recompensa do catalogo ja CONCEDIDA (rec.* no historico) e
        /// ainda nao aplicada entra agora. Idempotente; devolve quantas entraram. E por aqui que a recompensa chega,
        /// quem quer que tenha concluido a missao (gatilho, dialogo, save gravado entre as duas coisas).</summary>
        public static int Sincronizar(InventarioData inv, LifeEventHistory historia)
        {
            if (inv == null || historia == null) return 0;
            int n = 0;
            foreach (QuestDef d in QuestCatalog.Missoes)
                foreach (RecompensaDef r in d.Recompensas)
                    if (historia.Ja(r.Id) && Aplicar(inv, r)) n++;
            return n;
        }

        /// <summary>B15: a espada do treino (nao e a espada de brinquedo da origem guardioes, item.espada_de_madeira, que
        /// o Borin acha "fora de medida"). Comum e marcada tem a mesma estatistica (ADR-0010 §3).</summary>
        public const string EspadaComum = "item.espada_de_treino";
        public const string EspadaMarcada = "item.espada_de_treino_marcada";

        /// <summary>Id de transacao da entrega: UM para as duas, entao so uma espada entra na vida inteira.</summary>
        public const string RecEspada = "rec.b15.espada_do_treino";

        /// <summary>ADR-0010 §3: com confianca_de_borin (q06) a espada e a marcada, feita por ele; sem a flag, a comum do posto.</summary>
        public static string EspadaDevida(LifeEventHistory historia)
        {
            return historia != null && historia.Ja(QuestCatalog.EventoDaFlag("confianca_de_borin")) ? EspadaMarcada : EspadaComum;
        }

        /// <summary>Entrega a espada do treino quando a idade ja permite treinar (TrainingProgress.PodeTreinar). Idempotente
        /// pelo RecEspada. true = entrou agora.
        /// ponytail: a entrega e pela idade, no passo do MissaoMundo, e nao na conversa da ferraria (marcada) ou do posto
        /// (comum) como o ADR-0010 §3 descreve: as falas aos_oito do Borin e do Tovin ja contam quem entrega. Vira acao de
        /// dialogo validada pelo dominio (QuestIntentAdapter) se o playtest pedir a entrega na mao do NPC.</summary>
        public static bool EntregarEspada(InventarioData inv, LifeEventHistory historia, int idadeAnos)
        {
            if (inv == null || !TrainingProgress.PodeTreinar(idadeAnos)) return false;
            return Aplicar(inv, new RecompensaDef(RecEspada, QuestCatalog.TipoItem, EspadaDevida(historia), 1));
        }

        /// <summary>A espada do treino que o jogador tem (a marcada vence), ou null.</summary>
        public static string EspadaNaMao(InventarioData inv)
        {
            if (Quantidade(inv, EspadaMarcada) > 0) return EspadaMarcada;
            return Quantidade(inv, EspadaComum) > 0 ? EspadaComum : null;
        }

        /// <summary>Itens com quantidade &gt; 0, na ordem em que entraram (S.ITENS: a tela de itens so le isto).</summary>
        public static List<ItemPilha> Visiveis(InventarioData inv)
        {
            var r = new List<ItemPilha>();
            if (inv == null || inv.itens == null) return r;
            foreach (ItemPilha p in inv.itens) if (p != null && p.quantidade > 0 && !string.IsNullOrEmpty(p.itemId)) r.Add(p);
            return r;
        }

        public static int Quantidade(InventarioData inv, string itemId)
        {
            if (inv == null || inv.itens == null) return 0;
            foreach (ItemPilha p in inv.itens) if (p != null && p.itemId == itemId) return p.quantidade;
            return 0;
        }

        static ItemPilha Pilha(InventarioData inv, string itemId)
        {
            if (inv.itens == null) inv.itens = new List<ItemPilha>();
            foreach (ItemPilha p in inv.itens) if (p != null && p.itemId == itemId) return p;
            var nova = new ItemPilha { itemId = itemId ?? "" };
            inv.itens.Add(nova);
            return nova;
        }
    }
}
