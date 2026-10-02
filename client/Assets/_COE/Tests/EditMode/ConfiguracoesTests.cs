using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Configuracoes do jogador (menu de pausa): padroes, ida e volta pelo armazenamento (chaves publicadas
    /// fixas), e valor invalido ou fora da faixa caindo no padrao. Armazenamento em memoria: nada toca o PlayerPrefs.</summary>
    public class ConfiguracoesTests
    {
        [Test]
        public void Padroes_Destro_Sensibilidade1_30Fps_DesempenhoSoEmDesenvolvimento()
        {
            var dev = new Configuracoes(new ConfigEmMemoria(), true);
            Assert.AreEqual(HandPreset.Destro, dev.Mao);
            Assert.AreEqual(1f, dev.Sensibilidade);
            Assert.AreEqual(30, dev.Fps, "meta de celular do PIPELINE §4.1");
            Assert.IsTrue(dev.MostrarDesempenho, "build de desenvolvimento mostra o HUD de desempenho");
            Assert.IsFalse(new Configuracoes(new ConfigEmMemoria(), false).MostrarDesempenho, "fora dela, escondido");
        }

        [Test]
        public void IdaEVolta_PeloArmazenamento_ComChavesPublicadas()
        {
            var mem = new ConfigEmMemoria();
            var c = new Configuracoes(mem, true);
            c.DefinirMao(HandPreset.Canhoto);
            c.MudarSensibilidade(3);
            c.DefinirFps(60);
            c.DefinirMostrarDesempenho(false);

            // Chave publicada nao muda (renomear quebra a configuracao de quem ja jogou): o teste fixa nome e formato.
            Assert.AreEqual("canhoto", mem.Valores["coe.cfg.v1.mao"]);
            Assert.AreEqual("1.3", mem.Valores["coe.cfg.v1.sensibilidade"]);
            Assert.AreEqual("60", mem.Valores["coe.cfg.v1.fps"]);
            Assert.AreEqual("0", mem.Valores["coe.cfg.v1.desempenho"]);

            var lida = new Configuracoes(mem, true);   // dev=true: o "0" gravado vence o padrao
            Assert.AreEqual(HandPreset.Canhoto, lida.Mao);
            Assert.AreEqual(1.3f, lida.Sensibilidade, 1e-5f);
            Assert.AreEqual(60, lida.Fps);
            Assert.IsFalse(lida.MostrarDesempenho);
        }

        /// <summary>Som: ligado de fabrica, e quem desliga continua sem som ao reabrir o jogo.</summary>
        [Test]
        public void Som_LigadoPorPadrao_DesligadoSobreviveAReabrir()
        {
            var mem = new ConfigEmMemoria();
            var c = new Configuracoes(mem, false);
            Assert.IsTrue(c.Som, "jogo novo sem som");
            c.DefinirSom(false);
            Assert.AreEqual("0", mem.Valores[Configuracoes.ChaveSom]);
            Assert.IsFalse(new Configuracoes(mem, false).Som, "reabrir religou o som");
            mem.Valores[Configuracoes.ChaveSom] = "lixo";
            Assert.IsTrue(new Configuracoes(mem, false).Som, "valor ilegivel desliga o som");
        }

        [Test]
        public void ValorInvalidoOuForaDaFaixa_CaiNoPadrao()
        {
            foreach (string lixo in new[] { "", "abc", "NaN", "Infinity", "1,5", "0.4", "2.1", "-1" })
            {
                var mem = new ConfigEmMemoria();
                mem.Gravar(Configuracoes.ChaveMao, lixo == "" ? "esquerda" : lixo);
                mem.Gravar(Configuracoes.ChaveSensibilidade, lixo);
                mem.Gravar(Configuracoes.ChaveFps, lixo == "" ? "45" : lixo);
                mem.Gravar(Configuracoes.ChaveDesempenho, lixo == "" ? "talvez" : lixo);

                var c = new Configuracoes(mem, false);
                Assert.AreEqual(HandPreset.Destro, c.Mao, lixo);
                Assert.AreEqual(Configuracoes.SensibilidadePadrao, c.Sensibilidade, lixo);
                Assert.AreEqual(30, c.Fps, lixo);
                Assert.IsFalse(c.MostrarDesempenho, lixo);
            }
        }

        [Test]
        public void Sensibilidade_AndaNoPasso_PresaNaFaixa_SemDerivaDeFloat()
        {
            var mem = new ConfigEmMemoria();
            var c = new Configuracoes(mem, false);
            for (int i = 0; i < 30; i++) c.MudarSensibilidade(1);   // 30 somas de 0,1: sem prender, derivaria
            Assert.AreEqual(Configuracoes.SensibilidadeMax, c.Sensibilidade, 1e-5f);
            Assert.AreEqual(2f, new Configuracoes(mem, false).Sensibilidade, 1e-5f, "o maximo gravado relê dentro da faixa");

            c.MudarSensibilidade(-100);
            Assert.AreEqual(Configuracoes.SensibilidadeMin, c.Sensibilidade, 1e-5f);
            Assert.AreEqual("0.5", mem.Valores[Configuracoes.ChaveSensibilidade]);

            mem.Gravar(Configuracoes.ChaveSensibilidade, "1.26");   // dentro da faixa, fora do passo: prende no passo
            Assert.AreEqual(1.3f, new Configuracoes(mem, false).Sensibilidade, 1e-5f);
        }

        [Test]
        public void Fps_So30Ou60()
        {
            var mem = new ConfigEmMemoria();
            var c = new Configuracoes(mem, false);
            c.DefinirFps(60);
            Assert.AreEqual(60, c.Fps);
            foreach (int invalido in new[] { 45, 120, 0, -1 })
            {
                c.DefinirFps(invalido);
                Assert.AreEqual(30, c.Fps, invalido.ToString());
                Assert.AreEqual("30", mem.Valores[Configuracoes.ChaveFps]);
            }
        }
    }
}
