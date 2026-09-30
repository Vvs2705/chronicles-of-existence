using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>T012: cumprir objetivo pelo gatilho grava a ancora de onde o jogador esta, junto da transicao.</summary>
    public class AncoraNaTransicaoTests
    {
        [Test]
        public void AncoraDoGatilho_VemDaTabela_EObjetivoSemGatilhoDaVazio()
        {
            Assert.AreEqual("horta_familia", MissaoMundo.AncoraDo("q03_o_cesto_perdido", "procurar_na_horta"));
            Assert.AreEqual("", MissaoMundo.AncoraDo("q03_o_cesto_perdido", "objetivo_que_nao_existe"));
        }

        [Test]
        public void Posicao_VaiJuntoDaGravacaoDaTransicao()
        {
            SaveData s = new SaveData();
            int gravacoes = 0;
            string ancoraGravada = null;
            GameSession g = new GameSession(s, () => { gravacoes++; ancoraGravada = s.anchorId; });

            g.Posicao("Auren", "casa_familia");
            Assert.AreEqual(0, gravacoes, "posicao sozinha nao grava");
            Assert.IsTrue(g.Missao(m => m.Iniciar(QuestCatalog.Missoes[0].Id)).Ok);

            Assert.AreEqual("casa_familia", ancoraGravada, "a ancora saiu na mesma gravacao da transicao");
            Assert.AreEqual("Auren", s.sceneId);
        }
    }
}
