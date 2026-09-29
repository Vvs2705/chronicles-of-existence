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
    /// que um save anterior a esta tarefa significa, entao NAO sobe SaveData.SchemaVersion (cabecalho de SaveData.cs).
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
