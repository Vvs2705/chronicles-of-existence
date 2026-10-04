using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>O bloco de missoes atravessando o JSON de verdade (T004). Separado de QuestTests.cs de
    /// proposito: estes tres testes usam LocalSave, que usa UnityEngine.JsonUtility, entao SO rodam no
    /// editor — QuestTests.cs continua puro e roda fora dele.
    ///
    /// O que se prova aqui: o bloco nasce com padrao neutro e save v1 antigo (sem a chave) carrega limpo. A regra de
    /// versao esta no cabecalho de SaveData.cs.</summary>
    public class QuestSaveTests
    {
        // Save v1 legitimo, gravado ANTES da T006: tem saveVersion 1 e nenhuma chave "quests".
        const string SaveSemBlocoDeMissao =
            "{\"saveVersion\":1,\"characterId\":\"abc\"," +
            "\"birth\":{\"destinyId\":\"serena\",\"originId\":\"agricultores\",\"characterName\":\"Mira\",\"confirmedAtUtc\":1}," +
            "\"identity\":{\"appearanceId\":\"\"},\"ageYears\":5," +
            "\"attributes\":{\"forca\":1,\"agilidade\":1,\"vigor\":1,\"intelecto\":1,\"percepcao\":1,\"vontade\":1}," +
            "\"affinities\":{\"marcial\":0,\"arcana\":0,\"natural\":0,\"artesanal\":0,\"social\":0,\"exploratoria\":0}," +
            "\"lifeLevel\":1,\"createdAtUtc\":\"2026-09-28T00:00:00.0000000Z\",\"updatedAtUtc\":\"2026-09-28T00:00:00.0000000Z\"}";

        [Test]
        public void SaveAntigoSemOBloco_CarregaComCampanhaDoZero()
        {
            SaveData lido = LocalSave.FromJson(SaveSemBlocoDeMissao);

            Assert.IsNotNull(lido, "save v1 anterior a T006 continua valido");
            Assert.IsNotNull(lido.quests, "chave ausente vira padrao neutro, nao null");
            Assert.AreEqual(0, lido.quests.missoes.Count);

            QuestSystem q = new QuestSystem(lido.quests, new HistoricoDeVidaLedger(lido));
            Assert.AreEqual(QuestStatus.Disponivel, q.Estado("q01_um_novo_amanhecer"));
        }

        /// <summary>ADR-0010 adendo 11: save gravado antes da assinatura, com a Q-07 em andamento e perguntar_na_vila ja
        /// cumprido (Eira ou Oren fechavam). Atravessa o JSON e joga o resto pela sessao, como o jogo: Maelis nao pede
        /// assinatura (o objetivo ja esta feito), Tovin leva ao bosque e a Q-07 conclui sem desfecho. Nenhum campo mudou de
        /// significado: o passo v1 -> v2 e identidade.</summary>
        [Test]
        public void SaveAntigo_Q07ComPerguntarFeitoSemAssinatura_ConcluiPeloJogo()
        {
            const string q07 = "q07_o_desaparecimento";
            SaveData antigo = new SaveData();
            var linha = new QuestState { questId = q07, status = (int)QuestStatus.EmAndamento };
            linha.objetivosFeitos.AddRange(new[] { "notar_a_ausencia", "perguntar_na_vila" });
            antigo.quests.missoes.Add(linha);
            var g = new GameSession(LocalSave.FromJson(LocalSave.ToJson(antigo)), null);

            Assert.IsEmpty(MissaoNaConversa.Opcoes("maelis", g.Missoes), "objetivo ja feito: Maelis nao oferece a assinatura");
            OpcaoDeMissao seguir = System.Array.Find(MissaoNaConversa.Opcoes("tovin", g.Missoes),
                o => o.TextoKey == "missao.q07.obj.seguir_ate_o_bosque");
            Assert.IsNotNull(seguir, "Tovin leva ao bosque");
            Assert.IsTrue(g.Missao(m => MissaoNaConversa.Aplicar(m, seguir.Pedidos)).Ok);
            MissaoMundo.Avancar(g);   // o que o MissaoHud faz 5x/s

            Assert.AreEqual(QuestStatus.Concluida, g.Missoes.Estado(q07), "central nao trava em DesfechoPendente");
            Assert.IsTrue(g.Historia.Ja("marco.desaparecimento"));
            Assert.IsFalse(g.Historia.Ja(QuestCatalog.EventoAssinouComOCirculo) || g.Historia.Ja(QuestCatalog.EventoAssinouComUmRisco),
                "padrao neutro: sem pagina assinada");
            Assert.AreEqual(QuestStatus.EmAndamento, g.Missoes.Estado("q08_ecos_do_limiar"), "a campanha segue");
        }

        [Test]
        public void RoundTrip_PreservaStatusObjetivosERecompensaConcedida()
        {
            SaveData antes = new SaveData();
            QuestSystem q = new QuestSystem(antes.quests, new HistoricoDeVidaLedger(antes));
            QuestDef d = QuestCatalog.Missao("q01_um_novo_amanhecer");
            q.Iniciar(d.Id);
            foreach (ObjetivoDef o in d.Objetivos) q.CumprirObjetivo(d.Id, o.Id);
            Assert.IsTrue(q.Concluir(d.Id).Ok);

            SaveData depois = LocalSave.FromJson(LocalSave.ToJson(antes));

            Assert.IsNotNull(depois);
            QuestSystem q2 = new QuestSystem(depois.quests, new HistoricoDeVidaLedger(depois));
            Assert.AreEqual(QuestStatus.Concluida, q2.Estado(d.Id));
            Assert.AreEqual(d.Objetivos.Length, q2.ObjetivosFeitos(d.Id).Length);
            Assert.AreEqual(0, q2.Concluir(d.Id).Recompensas.Length,
                "recompensa ja concedida nao volta depois do round-trip (obrigatorio nº 3)");
            Assert.IsTrue(new LifeEventHistory(depois).Ja("marco.primeiro_dia"),
                "o marco aplicado atravessa o JSON (slice B06/§4.3: Mara e Daren lembram o primeiro dia)");
        }
    }
}
