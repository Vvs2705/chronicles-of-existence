using System.Collections.Generic;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Qual musica toca: a entrada manda sobre tudo, combate sobre lugar, bosque sobre hora; o combate segura alguns
    /// segundos depois do ultimo golpe e entra mais rapido do que as outras trocas. Episodio 2: festa na vila, descida e
    /// Enredado no bosque, lidos das missoes.</summary>
    public class TrilhaDoMomentoTests
    {
        [Test]
        public void Entrada_MandaSobreTudo()
        {
            Assert.AreEqual(TrilhaMusical.Limiar, TrilhaDoMomento.Escolher(TelaDeEntrada.Limiar, true, true, true));
            Assert.AreEqual(TrilhaMusical.Menu, TrilhaDoMomento.Escolher(TelaDeEntrada.Menu, true, true, true));
        }

        [Test]
        public void NoMundo_CombateBosqueENoite_NessaOrdem()
        {
            Assert.AreEqual(TrilhaMusical.Combate, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, true, true, true));
            Assert.AreEqual(TrilhaMusical.Bosque, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, false, true, true));
            Assert.AreEqual(TrilhaMusical.Bosque, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, false, true, false));
            Assert.AreEqual(TrilhaMusical.VilaNoite, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, false, false, true));
            Assert.AreEqual(TrilhaMusical.VilaDia, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, false, false, false));
        }

        [Test]
        public void Combate_SeguraDepoisDoGolpe_ESoltaSozinho()
        {
            Assert.IsFalse(TrilhaDoMomento.EmCombate(0f, -1f), "sem golpe nenhum nao ha combate");
            Assert.IsTrue(TrilhaDoMomento.EmCombate(10f, 10f));
            Assert.IsTrue(TrilhaDoMomento.EmCombate(10f + TrilhaDoMomento.SegundosDeCombate - 0.1f, 10f));
            Assert.IsFalse(TrilhaDoMomento.EmCombate(10f + TrilhaDoMomento.SegundosDeCombate, 10f));
        }

        [Test]
        public void Combate_EntraMaisRapidoQueAsOutrasTrocas()
        {
            Assert.Less(TrilhaDoMomento.Troca(TrilhaMusical.Combate), TrilhaDoMomento.Troca(TrilhaMusical.VilaDia));
            Assert.Greater(TrilhaDoMomento.Troca(TrilhaMusical.Combate), 0f);
        }

        [Test]
        public void Episodio2_FestaNaVila_DescidaEEnredadoNoBosque()
        {
            Assert.AreEqual(TrilhaMusical.Festa, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, false, false, false, MomentoDoEpisodio.Festa));
            Assert.AreEqual(TrilhaMusical.Bosque, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, false, true, false, MomentoDoEpisodio.Festa),
                "a festa e na praca: quem corre ate o vao ouve o bosque");
            Assert.AreEqual(TrilhaMusical.Descida, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, false, true, true, MomentoDoEpisodio.Descida));
            Assert.AreEqual(TrilhaMusical.Combate, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, true, true, true, MomentoDoEpisodio.Descida));
            Assert.AreEqual(TrilhaMusical.Enredado, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, true, true, true, MomentoDoEpisodio.Enredado),
                "a luta do Enredado tem a musica dele, nao a do combate comum");
            Assert.AreEqual(TrilhaMusical.VilaNoite, TrilhaDoMomento.Escolher(TelaDeEntrada.Nenhuma, false, false, true, MomentoDoEpisodio.Enredado));
            Assert.AreEqual(TrilhaMusical.Menu, TrilhaDoMomento.Escolher(TelaDeEntrada.Menu, true, true, true, MomentoDoEpisodio.Enredado));
        }

        static QuestSystem Com(params QuestState[] linhas)
        {
            var s = new SaveData();
            s.quests.missoes.AddRange(linhas);
            return new QuestSystem(s.quests, new HistoricoDeVidaLedger(s), QuestCatalogEp2.Todas);
        }

        static QuestState Linha(string id, QuestStatus st, params string[] feitos)
        {
            return new QuestState { questId = id, status = (int)st, objetivosFeitos = new List<string>(feitos) };
        }

        [Test]
        public void DoEpisodio_LeAsMissoes()
        {
            Assert.AreEqual(MomentoDoEpisodio.Nenhum, TrilhaDoMomento.DoEpisodio(null));
            Assert.AreEqual(MomentoDoEpisodio.Nenhum, TrilhaDoMomento.DoEpisodio(Com()), "sem ep. 2 nada muda");
            Assert.AreEqual(MomentoDoEpisodio.Festa, TrilhaDoMomento.DoEpisodio(Com(Linha(TrilhaDoMomento.MissaoDaFesta, QuestStatus.EmAndamento, "abrir_a_festa"))));
            Assert.AreEqual(MomentoDoEpisodio.Nenhum, TrilhaDoMomento.DoEpisodio(Com(Linha(TrilhaDoMomento.MissaoDaFesta, QuestStatus.EmAndamento, "abrir_a_festa", "tres_bancas", TrilhaDoMomento.FimDaFesta))),
                "depois da danca vem a noite do alarme");
            Assert.AreEqual(MomentoDoEpisodio.Descida, TrilhaDoMomento.DoEpisodio(Com(Linha(TrilhaDoMomento.MissaoDaFesta, QuestStatus.Concluida),
                                                                                       Linha(TrilhaDoMomento.MissaoDaDescida, QuestStatus.EmAndamento))));
            Assert.AreEqual(MomentoDoEpisodio.Enredado, TrilhaDoMomento.DoEpisodio(Com(Linha(TrilhaDoMomento.MissaoDaDescida, QuestStatus.Concluida),
                                                                                        Linha(TrilhaDoMomento.MissaoDoEnredado, QuestStatus.EmAndamento))));
            Assert.AreEqual(MomentoDoEpisodio.Nenhum, TrilhaDoMomento.DoEpisodio(Com(Linha(TrilhaDoMomento.MissaoDoEnredado, QuestStatus.Concluida))));
        }

        /// <summary>Os ids que a trilha le sao os do catalogo: renomear missao ou objetivo la quebra aqui, nao em silencio.</summary>
        [Test]
        public void IdsLidos_ExistemNoCatalogoDoEpisodio2()
        {
            Assert.IsNotNull(QuestCatalogEp2.Missao(TrilhaDoMomento.MissaoDaFesta));
            Assert.IsNotNull(QuestCatalogEp2.Missao(TrilhaDoMomento.MissaoDaFesta).Objetivo(TrilhaDoMomento.FimDaFesta));
            Assert.IsNotNull(QuestCatalogEp2.Missao(TrilhaDoMomento.MissaoDaDescida));
            Assert.IsNotNull(QuestCatalogEp2.Missao(TrilhaDoMomento.MissaoDoEnredado));
        }
    }
}
