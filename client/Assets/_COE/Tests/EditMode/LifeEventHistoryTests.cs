using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>LifeEventHistory (T005): registro idempotente, consultas e persistencia como bloco novo do save.
    /// Nenhum teste depende de cena; os que tocam disco gravam numa pasta temporaria propria.</summary>
    public class LifeEventHistoryTests
    {
        string dir;
        string Path_ { get { return System.IO.Path.Combine(dir, LocalSave.FileName); } }

        [SetUp]
        public void SetUp()
        {
            dir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "coe_life_tests_" + System.Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
        }

        [TearDown]
        public void TearDown()
        {
            try { Directory.Delete(dir, true); } catch (System.Exception) { }
        }

        static LifeEventHistory Nova() { return new LifeEventHistory(new SaveData()); }

        [Test]
        public void Registrar_FatoNovo_EntraNoHistorico()
        {
            LifeEventHistory h = Nova();

            Assert.IsTrue(h.Registrar("nascimento", LifeEventCategoria.Nascimento, 5),
                          "primeira vez devolve true: e agora que o efeito e concedido");

            Assert.AreEqual(1, h.Total);
            Assert.IsTrue(h.Ja("nascimento"));
            Assert.IsFalse(h.Ja("marco_idade_8"), "o que nao aconteceu nao aconteceu");
            LifeEvent e = h.Todos()[0];
            Assert.AreEqual("nascimento", e.eventId);
            Assert.AreEqual(LifeEventCategoria.Nascimento, e.categoria);
            Assert.AreEqual(5, e.idade);
            Assert.Greater(e.emUtc, 0L, "o registro carimba o momento");
            Assert.IsFalse(e.resumido);
        }

        // NEGATIVO — o coracao da T005 (dossie §H/§M, GDD §07): repetir o gatilho nao duplica nem concede de novo.
        [Test]
        public void Registrar_MesmoId_NaoDuplicaNemConcedeDeNovo()
        {
            LifeEventHistory h = Nova();
            Assert.IsTrue(h.Registrar("quest_cesto_perdido_entregue", LifeEventCategoria.Escolha, 5, "quest_cesto_perdido", "devolveu"));

            bool segunda = h.Registrar("quest_cesto_perdido_entregue", LifeEventCategoria.Escolha, 6, "quest_cesto_perdido", "devolveu de novo");

            Assert.IsFalse(segunda, "segunda chamada NAO pode autorizar recompensa");
            Assert.AreEqual(1, h.Total, "e nao pode deixar um segundo evento no historico");
            Assert.AreEqual(1, h.Contar(LifeEventCategoria.Escolha));
            Assert.AreEqual(5, h.Todos()[0].idade, "o fato original nao foi sobrescrito pela repeticao");
            Assert.AreEqual("devolveu", h.Todos()[0].detalhe);
        }

        // NEGATIVO — a variante que o dossie §M exige: salvar/carregar NO MEIO e tentar de novo.
        [Test]
        public void Registrar_DepoisDeSalvarECarregar_ContinuaSendoUmSoFato()
        {
            SaveData save = new SaveData();
            LifeEventHistory antes = new LifeEventHistory(save);
            Assert.IsTrue(antes.Registrar("marco_idade_8", LifeEventCategoria.Marco, 8));

            LocalSave.Save(save, Path_);
            SaveData recarregado = LocalSave.Load(Path_);
            LifeEventHistory depois = new LifeEventHistory(recarregado);

            Assert.IsTrue(depois.Ja("marco_idade_8"), "o fato sobreviveu ao disco");
            Assert.IsFalse(depois.Registrar("marco_idade_8", LifeEventCategoria.Marco, 8),
                           "recarregar o save nao devolve o direito de envelhecer de novo");
            Assert.AreEqual(1, depois.Total);
            Assert.AreEqual(1, depois.Contar(LifeEventCategoria.Marco));
        }

        [Test]
        public void Ordem_DeRegistroEPreservada()
        {
            LifeEventHistory h = Nova();
            h.Registrar("nascimento", LifeEventCategoria.Nascimento, 5);
            h.Registrar("primeira_espada", LifeEventCategoria.PrimeiraVez, 8, "npc_tovin");
            h.Registrar("amizade_nilo", LifeEventCategoria.Relacao, 6, "npc_nilo");
            h.Registrar("marco_idade_8", LifeEventCategoria.Marco, 8);

            IList<LifeEvent> todos = h.Todos();
            Assert.AreEqual(4, todos.Count);
            Assert.AreEqual("nascimento", todos[0].eventId);
            Assert.AreEqual("primeira_espada", todos[1].eventId);
            Assert.AreEqual("amizade_nilo", todos[2].eventId);
            Assert.AreEqual("marco_idade_8", todos[3].eventId);
        }

        [Test]
        public void Consulta_PorCategoriaEPorEscopo()
        {
            LifeEventHistory h = Nova();
            h.Registrar("amizade_nilo", LifeEventCategoria.Relacao, 6, "npc_nilo", "primeiro amigo");
            h.Registrar("briga_sera", LifeEventCategoria.Relacao, 7, "npc_sera");
            h.Registrar("marco_idade_8", LifeEventCategoria.Marco, 8);
            h.Registrar("conversa_nilo_bosque", LifeEventCategoria.Escolha, 7, "npc_nilo", "aceitou");

            Assert.AreEqual(2, h.Contar(LifeEventCategoria.Relacao));
            Assert.AreEqual(1, h.Contar(LifeEventCategoria.Marco));
            Assert.AreEqual(0, h.Contar(LifeEventCategoria.Nascimento), "categoria sem evento conta zero, nao explode");

            Assert.AreEqual("briga_sera", h.Ultimo(LifeEventCategoria.Relacao).eventId, "o mais recente da categoria");
            Assert.IsNull(h.Ultimo(LifeEventCategoria.Nascimento));

            List<LifeEvent> doNilo = h.PorEscopo("npc_nilo");   // o que T007/T010 vao perguntar
            Assert.AreEqual(2, doNilo.Count);
            Assert.AreEqual("amizade_nilo", doNilo[0].eventId);
            Assert.AreEqual("conversa_nilo_bosque", doNilo[1].eventId);
            Assert.AreEqual(0, h.PorEscopo("npc_borin").Count);
            Assert.AreEqual(0, h.PorEscopo(null).Count, "escopo vazio nao devolve o historico inteiro");
        }

        [Test]
        public void RoundTrip_PeloSave_ComAcentoNoDetalhe()
        {
            SaveData save = new SaveData();
            LifeEventHistory h = new LifeEventHistory(save);
            h.Registrar("nascimento", LifeEventCategoria.Nascimento, 5, "npc_mara", "colo da mãe");
            h.Registrar("escolha_bosque", LifeEventCategoria.Escolha, 7, "quest_o_animal_ferido", "salvou a raposa ferida");
            long carimbo = h.Todos()[1].emUtc;

            LocalSave.Save(save, Path_);
            SaveData back = LocalSave.Load(Path_);

            Assert.AreEqual(2, back.lifeHistory.eventos.Count);
            LifeEvent e = back.lifeHistory.eventos[1];
            Assert.AreEqual("escolha_bosque", e.eventId);
            Assert.AreEqual(LifeEventCategoria.Escolha, e.categoria);
            Assert.AreEqual(7, e.idade);
            Assert.AreEqual(carimbo, e.emUtc, "o carimbo de tempo atravessa o disco sem perder precisao");
            Assert.AreEqual("quest_o_animal_ferido", e.escopo);
            Assert.AreEqual("salvou a raposa ferida", e.detalhe);
            Assert.AreEqual("colo da mãe", back.lifeHistory.eventos[0].detalhe, "acento sobrevive ao disco");
            Assert.AreEqual(SaveData.SchemaVersion, back.saveVersion, "bloco novo NAO sobe a versao do save");
        }

        [Test]
        public void RoundTrip_HistoricoVazio()
        {
            SaveData save = new SaveData();
            Assert.AreEqual(0, new LifeEventHistory(save).Total);

            LocalSave.Save(save, Path_);
            SaveData back = LocalSave.Load(Path_);

            Assert.IsNotNull(back.lifeHistory);
            Assert.IsNotNull(back.lifeHistory.eventos);
            Assert.AreEqual(0, back.lifeHistory.eventos.Count);
            Assert.AreEqual(0, new LifeEventHistory(back).Total);
            Assert.IsFalse(File.Exists(LocalSave.RejectedPath(Path_)), "lista vazia nao e save invalido");
        }

        // Regra de crescimento da T004: chave que falta vira o padrao neutro, sem migracao e sem subir versao.
        [Test]
        public void SaveV1_SemOBloco_CarregaComHistoricoVazio()
        {
            const string v1SemBloco = "{\"saveVersion\":1,\"characterId\":\"abc\",\"birth\":{\"destinyId\":\"serena\"," +
                                      "\"originId\":\"agricultores\",\"characterName\":\"Íris\",\"confirmedAtUtc\":42}," +
                                      "\"identity\":{\"appearanceId\":\"ap_01\"},\"ageYears\":6,\"attributes\":{}," +
                                      "\"affinities\":{},\"lifeLevel\":2}";
            File.WriteAllText(Path_, v1SemBloco);

            SaveData back = LocalSave.Load(Path_);

            Assert.IsFalse(File.Exists(LocalSave.RejectedPath(Path_)), "save v1 sem o bloco novo continua valido");
            Assert.AreEqual("serena", back.birth.destinyId, "o resto do save veio inteiro");
            Assert.AreEqual(6, back.ageYears);
            Assert.IsNotNull(back.lifeHistory, "bloco ausente nasce com o padrao neutro");
            LifeEventHistory h = new LifeEventHistory(back);
            Assert.AreEqual(0, h.Total);
            Assert.IsFalse(h.Ja("marco_idade_8"));
            Assert.IsTrue(h.Registrar("marco_idade_8", LifeEventCategoria.Marco, 8), "e o save velho segue jogavel");
        }

        [Test]
        public void BlocoNull_NoSave_ViraPadraoNeutro()
        {
            SaveData save = new SaveData();
            save.lifeHistory = null;   // arquivo adulterado, ou codigo velho que zerou o campo

            LifeEventHistory h = new LifeEventHistory(save);

            Assert.IsNotNull(save.lifeHistory, "o bloco e restaurado no proprio save, nao so no historico");
            Assert.IsTrue(h.Registrar("nascimento", LifeEventCategoria.Nascimento, 5));
            Assert.AreEqual(1, save.lifeHistory.eventos.Count);
        }

        // Save adulterado com id repetido nao pode virar dois fatos (nem duas recompensas).
        [Test]
        public void Abrir_SaveComIdRepetido_DeduplicaEIgnoraEventoInvalido()
        {
            LifeHistoryData d = new LifeHistoryData();
            d.eventos.Add(new LifeEvent { eventId = "marco_idade_8", categoria = LifeEventCategoria.Marco, idade = 8 });
            d.eventos.Add(new LifeEvent { eventId = "marco_idade_8", categoria = LifeEventCategoria.Marco, idade = 8 });
            d.eventos.Add(new LifeEvent { eventId = "", categoria = LifeEventCategoria.Marco, idade = 9 });
            d.eventos.Add(null);

            LifeEventHistory h = new LifeEventHistory(d);

            Assert.AreEqual(1, h.Total);
            Assert.AreEqual(1, h.Contar(LifeEventCategoria.Marco));
            Assert.IsFalse(h.Registrar("marco_idade_8", LifeEventCategoria.Marco, 8));
        }

        [Test]
        public void Registrar_SemIdOuCategoria_Lanca()
        {
            LifeEventHistory h = Nova();
            Assert.Throws<System.ArgumentException>(delegate { h.Registrar("", LifeEventCategoria.Marco, 8); });
            Assert.Throws<System.ArgumentException>(delegate { h.Registrar("evento_sem_categoria", "", 8); });
            Assert.Throws<System.ArgumentNullException>(delegate { h.Registrar(null); });
            Assert.AreEqual(0, h.Total, "nada entrou pela metade");
        }

        // Teto de crescimento: o detalhe e podado, o FATO nunca.
        [Test]
        public void Teto_ResumeOsMaisAntigos_SemPerderFatoNemAbrirRecompensa()
        {
            LifeEventHistory h = Nova();
            int n = LifeEventHistory.TetoPorCategoria + 3;
            for (int i = 0; i < n; i++)
                h.Registrar("relacao_" + i, LifeEventCategoria.Relacao, 6, "npc_nilo", "detalhe " + i);
            h.Registrar("marco_idade_8", LifeEventCategoria.Marco, 8, "quest_ecos", "confirmou");

            Assert.AreEqual(n + 1, h.Total, "o teto nao apaga evento");
            Assert.AreEqual(n, h.Contar(LifeEventCategoria.Relacao), "Contar continua dizendo a verdade");

            LifeEvent maisAntigo = h.Todos()[0];
            Assert.IsTrue(maisAntigo.resumido, "o mais antigo da categoria cheia perdeu o detalhe");
            Assert.AreEqual("relacao_0", maisAntigo.eventId, "mas mantem id");
            Assert.AreEqual(LifeEventCategoria.Relacao, maisAntigo.categoria, "e categoria");
            Assert.AreEqual("", maisAntigo.detalhe);
            Assert.AreEqual("", maisAntigo.escopo);
            Assert.AreEqual(0, maisAntigo.idade);

            // o que nao pode acontecer de jeito nenhum:
            Assert.IsTrue(h.Ja("relacao_0"), "evento resumido continua tendo acontecido");
            Assert.IsFalse(h.Registrar("relacao_0", LifeEventCategoria.Relacao, 6),
                           "resumir NAO devolve o direito de conceder a recompensa de novo");

            Assert.IsFalse(h.Todos()[3].resumido, "so os que passam do teto sao resumidos");
            Assert.IsFalse(h.Ultimo(LifeEventCategoria.Marco).resumido, "categoria abaixo do teto nao e tocada");
            Assert.AreEqual(LifeEventHistory.TetoPorCategoria, DetalhadosDe(h, LifeEventCategoria.Relacao));
        }

        [Test]
        public void Teto_SobreviveAoSave()
        {
            SaveData save = new SaveData();
            LifeEventHistory h = new LifeEventHistory(save);
            for (int i = 0; i < LifeEventHistory.TetoPorCategoria + 2; i++)
                h.Registrar("relacao_" + i, LifeEventCategoria.Relacao, 6, "npc_nilo", "detalhe " + i);

            LocalSave.Save(save, Path_);
            LifeEventHistory back = new LifeEventHistory(LocalSave.Load(Path_));

            Assert.AreEqual(LifeEventHistory.TetoPorCategoria + 2, back.Total);
            Assert.AreEqual(LifeEventHistory.TetoPorCategoria, DetalhadosDe(back, LifeEventCategoria.Relacao));
            Assert.IsTrue(back.Ja("relacao_0"));
            Assert.IsFalse(back.Registrar("relacao_0", LifeEventCategoria.Relacao, 6));
        }

        static int DetalhadosDe(LifeEventHistory h, string categoria)
        {
            int n = 0;
            foreach (LifeEvent e in h.Todos()) if (e.categoria == categoria && !e.resumido) n++;
            return n;
        }
    }
}
