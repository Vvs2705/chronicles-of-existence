using System.Text.RegularExpressions;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>T012: o inventario minimo. Recompensa de moedas/item chega UMA vez por id — pelo gatilho, pelo dialogo
    /// ou depois de recarregar — e o bloco atravessa o JSON (LocalSave, entao so roda no editor); save v1 sem o
    /// bloco carrega com inventario vazio. Marco nao e posse: fica no historico.</summary>
    public class InventarioTests
    {
        const string Cesto = "item.cesto_de_vime";

        int gravacoes;
        GameSession Abrir(SaveData s) { return new GameSession(s, () => gravacoes++); }

        [SetUp] public void Zerar() { gravacoes = 0; }

        static SaveData Recarregar(SaveData s)
        {
            SaveData lido = LocalSave.FromJson(LocalSave.ToJson(s));
            Assert.IsNotNull(lido);
            return lido;
        }

        /// <summary>q03 com praca e trilha feitas (pelo dialogo): so falta a horta, que e gatilho de ancora.</summary>
        GameSession Q03FaltandoAHorta()
        {
            GameSession g = Abrir(new SaveData());
            MissaoTeste.Concluir(g.Missoes, MissaoTeste.Q01);
            Assert.IsTrue(g.Missoes.Iniciar(MissaoTeste.Q03).Ok);
            Assert.IsTrue(g.Missoes.CumprirObjetivo(MissaoTeste.Q03, "procurar_na_praca").Ok);
            Assert.IsTrue(g.Missoes.CumprirObjetivo(MissaoTeste.Q03, "procurar_na_trilha").Ok);
            return g;
        }

        [Test]
        public void GatilhoQueFechaAMissao_PoeOItemNaMesmaGravacao()
        {
            GameSession g = Q03FaltandoAHorta();

            Assert.IsTrue(MissaoMundo.Cumprir(g, MissaoTeste.Q03, "procurar_na_horta").Ok);

            Assert.AreEqual(QuestStatus.Concluida, g.Missoes.Estado(MissaoTeste.Q03));
            Assert.AreEqual(1, Inventario.Quantidade(g.Save.inventario, Cesto));
            Assert.AreEqual(1, gravacoes, "objetivo + conclusao + item = uma gravacao");
        }

        [Test]
        public void RepetirERecarregar_NaoDuplica()
        {
            GameSession g = Q03FaltandoAHorta();
            MissaoMundo.Cumprir(g, MissaoTeste.Q03, "procurar_na_horta");

            Assert.IsFalse(MissaoMundo.Cumprir(g, MissaoTeste.Q03, "procurar_na_horta").Ok, "missao concluida nao reabre");
            Assert.IsFalse(MissaoMundo.Avancar(g).Ok);
            Assert.IsFalse(g.Missao(m => m.Concluir(MissaoTeste.Q03)).Ok);

            GameSession depois = Abrir(Recarregar(g.Save));
            MissaoMundo.Avancar(depois);

            Assert.AreEqual(1, Inventario.Quantidade(depois.Save.inventario, Cesto), "recarregar nao paga de novo");
            Assert.AreEqual(1, gravacoes, "nada depois da primeira conclusao gravou");
        }

        [Test]
        public void UltimoObjetivoPeloDialogo_AvancarConcluiEPaga()
        {
            GameSession g = Abrir(new SaveData());
            MissaoTeste.Concluir(g.Missoes, MissaoTeste.Q01);
            MissaoTeste.Cumprir(g.Missoes, MissaoTeste.Q02);   // prestar_contas com Daren: dialogo

            Assert.IsTrue(MissaoMundo.Avancar(g).Ok);

            Assert.AreEqual(QuestStatus.Concluida, g.Missoes.Estado(MissaoTeste.Q02));
            Assert.AreEqual(5, g.Save.inventario.moedas, "B07: q02 paga 5 moedas (HIPOTESE v0 do catalogo)");
            Assert.AreEqual(1, gravacoes);
        }

        [Test]
        public void ConclusaoPeloDialogo_SemAplicar_ChegaNaMesmaGravacao_UmaVez()
        {
            GameSession g = Abrir(new SaveData());
            MissaoTeste.Concluir(g.Missoes, MissaoTeste.Q01);
            MissaoTeste.Cumprir(g.Missoes, MissaoTeste.Q02);

            // Quem concluiu nao aplicou nada: a sessao alcanca o inventario antes de gravar (GameSession.Sincronizar).
            Assert.IsTrue(g.Missao(m => m.Concluir(MissaoTeste.Q02)).Ok);
            Assert.AreEqual(5, g.Save.inventario.moedas, "a recompensa sai na mesma gravacao da conclusao");

            Assert.IsFalse(MissaoMundo.Avancar(g).Ok, "nada mais a fazer");
            Assert.AreEqual(5, g.Save.inventario.moedas, "uma vez so");
            Assert.AreEqual(1, gravacoes, "a conclusao grava uma vez, ja com a recompensa");
        }

        [Test]
        public void MesmaRecompensaDuasVezes_SomaUmaVez_EMarcoNaoEntra()
        {
            var inv = new InventarioData();
            RecompensaDef ervas = QuestCatalog.Missao(MissaoTeste.Q05).Recompensas[0];
            RecompensaDef marco = QuestCatalog.Missao(MissaoTeste.Q01).Recompensas[0];

            Assert.IsTrue(Inventario.Aplicar(inv, ervas));
            Assert.IsFalse(Inventario.Aplicar(inv, ervas));
            Assert.IsFalse(Inventario.Aplicar(inv, marco), "marco mora no historico, nao no inventario");

            Assert.AreEqual(2, Inventario.Quantidade(inv, "item.ervas_de_lysa"));
            Assert.AreEqual(1, inv.itens.Count);
            Assert.AreEqual(0, inv.moedas);
        }

        [Test]
        public void RoundTrip_PreservaMoedasItensEAplicadas()
        {
            GameSession g = Q03FaltandoAHorta();
            MissaoMundo.Cumprir(g, MissaoTeste.Q03, "procurar_na_horta");
            g.Save.inventario.moedas = 7;

            SaveData lido = Recarregar(g.Save);

            Assert.AreEqual(7, lido.inventario.moedas);
            Assert.AreEqual(1, Inventario.Quantidade(lido.inventario, Cesto));
            CollectionAssert.AreEqual(g.Save.inventario.recompensasAplicadas, lido.inventario.recompensasAplicadas);
        }

        [Test]
        public void SaveSemOBloco_CarregaVazio_ERecebeUmaVezOQueJaTinhaGanho()
        {
            SaveData velho = new SaveData();
            QuestSystem q = new GameSession(velho, null).Missoes;
            MissaoTeste.Concluir(q, MissaoTeste.Q01);
            MissaoTeste.Concluir(q, MissaoTeste.Q02);   // rec.q02 no historico, inventario intocado
            string json = Regex.Replace(LocalSave.ToJson(velho), "\"inventario\"\\s*:\\s*\\{[^{}]*\\}\\s*,", "");
            json = Regex.Replace(json, "\"saveVersion\"\\s*:\\s*\\d+", "\"saveVersion\": 1");   // save v1 de antes da T012
            StringAssert.DoesNotContain("inventario", json, "o JSON de teste tem de ser de antes da T012");

            SaveData lido = LocalSave.FromJson(json);

            Assert.IsNotNull(lido, "save sem o bloco continua valido");
            Assert.IsNotNull(lido.inventario, "chave ausente vira padrao neutro, nao null");
            Assert.AreEqual(0, lido.inventario.moedas);
            Assert.IsNotNull(lido.inventario.itens);
            Assert.IsNotNull(lido.inventario.recompensasAplicadas);

            GameSession g = Abrir(lido);   // abrir sincroniza: o que o historico diz que foi concedido chega, uma vez
            Assert.AreEqual(5, lido.inventario.moedas);
            Assert.IsFalse(MissaoMundo.Avancar(g).Ok);
            Assert.AreEqual(5, lido.inventario.moedas, "e nao chega de novo");
        }
    }
}
