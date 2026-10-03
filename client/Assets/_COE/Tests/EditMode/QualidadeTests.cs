using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>ADR-0009: a faixa grafica sai da RAM do aparelho na primeira abertura e o jogador pode fixar outra em
    /// Configuracoes (persistida em coe.cfg.v1.qualidade, fora do save). Valor estranho no PlayerPrefs = automatica.</summary>
    public class QualidadeTests
    {
        [TestCase(1800, FaixaQualidade.Baixa)]   // 2 GB
        [TestCase(2800, FaixaQualidade.Baixa)]   // 3 GB
        [TestCase(3700, FaixaQualidade.Media)]   // 4 GB
        [TestCase(5400, FaixaQualidade.Media)]
        [TestCase(5600, FaixaQualidade.Alta)]    // 6 GB
        [TestCase(7600, FaixaQualidade.Alta)]    // 8 GB (POCO F4)
        [TestCase(0, FaixaQualidade.Media)]      // leitura invalida
        public void Detectar_PelaRam(int ramMb, FaixaQualidade esperada)
        {
            Assert.AreEqual(esperada, Qualidade.Detectar(ramMb));
        }

        [Test]
        public void Configuracao_AutomaticaPorPadrao_EscolhaPersiste_ELixoVoltaAoAuto()
        {
            var mem = new ConfigEmMemoria();
            var c = new Configuracoes(mem, false);
            Assert.IsNull(c.QualidadeEscolhida, "sem escolha = automatica");
            Assert.AreEqual(FaixaQualidade.Baixa, c.QualidadeEfetiva(2800));

            c.DefinirQualidade(FaixaQualidade.Alta);
            Assert.AreEqual("alta", mem.Valores[Configuracoes.ChaveQualidade]);
            var relida = new Configuracoes(mem, false);
            Assert.AreEqual(FaixaQualidade.Alta, relida.QualidadeEfetiva(2800), "escolha do jogador vale acima da deteccao");

            relida.DefinirQualidade(null);
            Assert.AreEqual("auto", mem.Valores[Configuracoes.ChaveQualidade]);
            Assert.IsNull(new Configuracoes(mem, false).QualidadeEscolhida);

            mem.Valores[Configuracoes.ChaveQualidade] = "ultra";
            Assert.IsNull(new Configuracoes(mem, false).QualidadeEscolhida, "PlayerPrefs editado a mao nao quebra: volta ao auto");
        }
    }
}
