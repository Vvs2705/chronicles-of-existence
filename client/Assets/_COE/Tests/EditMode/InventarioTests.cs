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

        /// <summary>B15 / ADR-0010 §3: na idade do treino entra UMA espada de treino: a marcada com confianca_de_borin (q06),
        /// a comum sem ela. Antes da idade, nada; repetir nao duplica; a flag levantada depois nao troca a espada.</summary>
        [Test]
        public void EspadaDoTreino_ComumOuMarcada_SoNaIdadeDoTreino_UmaVezSo()
        {
            int crianca = 5, aprendiz = 8;
            Assert.IsFalse(TrainingProgress.PodeTreinar(crianca));
            Assert.IsTrue(TrainingProgress.PodeTreinar(aprendiz));

            var inv = new InventarioData();
            var semFlag = new LifeEventHistory(new SaveData());
            Assert.IsFalse(Inventario.EntregarEspada(inv, semFlag, crianca), "aos 5 nao ha espada");
            Assert.IsNull(Inventario.EspadaNaMao(inv));
            Assert.IsTrue(Inventario.EntregarEspada(inv, semFlag, aprendiz), "aos 8 entra a comum");
            Assert.AreEqual(Inventario.EspadaComum, Inventario.EspadaNaMao(inv));
            Assert.IsFalse(Inventario.EntregarEspada(inv, semFlag, aprendiz), "repetir nao duplica");
            Assert.AreEqual(1, Inventario.Quantidade(inv, Inventario.EspadaComum));

            SaveData comQ06 = new SaveData();
            var historia = new LifeEventHistory(comQ06);
            historia.Registrar(QuestCatalog.EventoDaFlag("confianca_de_borin"), LifeEventCategoria.Marco, aprendiz);
            var invB = new InventarioData();
            Assert.IsTrue(Inventario.EntregarEspada(invB, historia, aprendiz));
            Assert.AreEqual(Inventario.EspadaMarcada, Inventario.EspadaNaMao(invB), "com a q06, a do Borin");
            Assert.AreEqual(0, Inventario.Quantidade(invB, Inventario.EspadaComum), "uma espada so");

            Assert.IsFalse(Inventario.EntregarEspada(inv, historia, aprendiz), "a flag depois nao troca a espada");
            Assert.AreEqual(Inventario.EspadaComum, Inventario.EspadaNaMao(inv));
            Assert.AreEqual(0, Inventario.Quantidade(new InventarioData(), Inventario.EspadaMarcada));
        }

        /// <summary>A espada chega pelo passo do mundo (MissaoMundo.Avancar), gravando uma vez, e so na idade do treino.</summary>
        [Test]
        public void EspadaDoTreino_ChegaPeloPassoDoMundo_NaIdadeDoTreino()
        {
            SaveData s = new SaveData();
            GameSession g = Abrir(s);
            MissaoMundo.Avancar(g);
            Assert.IsNull(Inventario.EspadaNaMao(s.inventario), "aos " + s.ageYears + " ainda nao");

            s.ageYears = 8;
            int antes = gravacoes;
            MissaoMundo.Avancar(g);
            Assert.AreEqual(Inventario.EspadaComum, Inventario.EspadaNaMao(s.inventario));
            Assert.Greater(gravacoes, antes, "a entrega grava");
            antes = gravacoes;
            MissaoMundo.Avancar(g);
            Assert.AreEqual(antes, gravacoes, "o passo seguinte nao regrava nada");
        }

        /// <summary>S.ITENS: a tela de itens lista o que o jogador tem (nome, quantidade acima de 1), na ordem em que entrou,
        /// sem item zerado, e as moedas no fim; sem nada, a frase de vazio. Nascimento real: o inventario sai do catalogo.</summary>
        [Test]
        public void TelaDeItens_ListaOQueTem_ComQuantidadeEMoedas_SemZerado()
        {
            Assert.IsTrue(StringsLoader.Load(StringsLoader.DefaultLanguage), "arquivo de textos");
            try
            {
                var inv = new InventarioData();
                Assert.AreEqual(Strings.Get("itens.vazio"), MenuDePausa.TextoDosItens(inv));
                Assert.AreEqual(Strings.Get("itens.vazio"), MenuDePausa.TextoDosItens(null));

                Inventario.Nascer(inv, DestinyCatalog.Compor("dificil", "guardioes"));
                Inventario.Aplicar(inv, new RecompensaDef("rec.teste.ervas", QuestCatalog.TipoItem, "item.ervas_de_lysa", 2));
                inv.itens.Add(new ItemPilha { itemId = "item.cantil", quantidade = 0 });
                string t = MenuDePausa.TextoDosItens(inv);

                string[] linhas = t.Split('\n');
                StringAssert.Contains(Strings.Get("item.espada_de_madeira"), t, "item da origem");
                StringAssert.Contains(Strings.Get("item.faca_gasta"), t, "item do destino");
                StringAssert.Contains(Strings.Get("item.ervas_de_lysa") + "  x2", t, "quantidade acima de 1");
                StringAssert.DoesNotContain(Strings.Get("item.cantil"), t, "item zerado nao aparece");
                StringAssert.DoesNotContain("[", t, "todo item com nome escrito");
                Assert.AreEqual(Inventario.Visiveis(inv).Count + (inv.moedas > 0 ? 2 : 0), linhas.Length, "um item por linha, linha em branco e moedas");
                if (inv.moedas > 0) StringAssert.StartsWith(Strings.Get("hud.moedas"), linhas[linhas.Length - 1]);
            }
            finally { Strings.Load(null); }
        }

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
