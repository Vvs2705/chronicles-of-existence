using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace COE.Tests
{
    /// <summary>O voltar do Android / Esc (DIVIDA_TECNICA): uma acao por voltar, da tela de cima para a de baixo, e na
    /// raiz a confirmacao de saida. Regra pura (VoltarHud.Decidir), sem cena.</summary>
    public class VoltarHudTests
    {
        static AcaoDoVoltar D(VoltarHud.Estado e) { return VoltarHud.Decidir(e); }

        /// <summary>Prompt Mestre §28 (pause/back): a confirmacao de saida pausa e trava so o que estava ligado; abrir de novo
        /// nao perde a escala de antes; fechar devolve tudo e nao religa o que outra tela travou. Sem quadro: so os metodos.</summary>
        [Test]
        public void Saida_PausaETravaSoOQueEstavaLigado_EFecharDevolve()
        {
            var ligado = new GameObject("ligado").AddComponent<Light>();
            var jaTravado = new GameObject("jaTravado").AddComponent<Light>();
            jaTravado.enabled = false;
            var go = new GameObject("Voltar");
            VoltarHud v = go.AddComponent<VoltarHud>();
            typeof(VoltarHud).GetField("travar", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(v, new Behaviour[] { ligado, jaTravado });
            float antes = Time.timeScale;
            try
            {
                v.AbrirSaida();
                Assert.IsTrue(v.SaidaAberta);
                Assert.AreEqual(0f, Time.timeScale, "a confirmacao pausa");
                Assert.IsFalse(ligado.enabled, "e trava quem estava ligado");
                v.AbrirSaida();   // segundo pedido com ela aberta: nao pode guardar a escala 0 como "a de antes"
                v.FecharSaida();
                Assert.IsFalse(v.SaidaAberta);
                Assert.AreEqual(antes, Time.timeScale, "continuar jogando despausa");
                Assert.IsTrue(ligado.enabled);
                Assert.IsFalse(jaTravado.enabled, "o que outra tela travou continua travado");
                v.FecharSaida();
                Assert.AreEqual(antes, Time.timeScale, "fechar de novo nao muda nada");
            }
            finally
            {
                Time.timeScale = antes;
                Object.DestroyImmediate(go);
                Object.DestroyImmediate(ligado.gameObject);
                Object.DestroyImmediate(jaTravado.gameObject);
            }
        }

        [Test]
        public void EmJogo_NadaAberto_PedeConfirmacaoParaSair()
        {
            Assert.AreEqual(AcaoDoVoltar.PedirSaida, D(new VoltarHud.Estado()));
        }

        [Test]
        public void CadaTelaSozinha_FechaElaMesma()
        {
            Assert.AreEqual(AcaoDoVoltar.FecharSaida, D(new VoltarHud.Estado { saida = true }), "segundo voltar = continuar jogando");
            Assert.AreEqual(AcaoDoVoltar.FecharConversa, D(new VoltarHud.Estado { conversa = true }));
            Assert.AreEqual(AcaoDoVoltar.FecharMenu, D(new VoltarHud.Estado { menu = true }));
            Assert.AreEqual(AcaoDoVoltar.FecharSalto, D(new VoltarHud.Estado { salto = true }), "\"ainda nao\": nada grava");
            Assert.AreEqual(AcaoDoVoltar.Nada, D(new VoltarHud.Estado { gancho = true }), "B16 e de leitura: so o botao dele fecha");
        }

        [Test]
        public void Entrada_VoltaUmaTela_ENaRaizPedeSaida()
        {
            Assert.AreEqual(AcaoDoVoltar.VoltarEntrada, D(new VoltarHud.Estado { entrada = true, entradaPodeVoltar = true }),
                "tela com Voltar/Cancelar: o voltar faz o mesmo");
            Assert.AreEqual(AcaoDoVoltar.PedirSaida, D(new VoltarHud.Estado { entrada = true }), "titulo, aviso, destino: raiz");
            Assert.AreEqual(AcaoDoVoltar.PedirSaida, D(new VoltarHud.Estado { entradaPodeVoltar = true }),
                "entrada fechada nao conta, mesmo com o PodeVoltar velho");
        }

        [Test]
        public void DuasAbertas_FechaADeCima()
        {
            Assert.AreEqual(AcaoDoVoltar.FecharSaida, D(new VoltarHud.Estado { saida = true, entrada = true, entradaPodeVoltar = true }));
            Assert.AreEqual(AcaoDoVoltar.FecharSaida, D(new VoltarHud.Estado { saida = true, conversa = true, menu = true }));
            Assert.AreEqual(AcaoDoVoltar.VoltarEntrada, D(new VoltarHud.Estado { entrada = true, entradaPodeVoltar = true, menu = true }),
                "a entrada cobre a tela inteira");
            Assert.AreEqual(AcaoDoVoltar.Nada, D(new VoltarHud.Estado { gancho = true, conversa = true }),
                "o gancho fica por cima: a conversa de baixo nao fecha escondida");
            Assert.AreEqual(AcaoDoVoltar.FecharConversa, D(new VoltarHud.Estado { conversa = true, menu = true }));
            Assert.AreEqual(AcaoDoVoltar.FecharConversa, D(new VoltarHud.Estado { conversa = true, salto = true }));
            Assert.AreEqual(AcaoDoVoltar.FecharMenu, D(new VoltarHud.Estado { menu = true, salto = true }));
        }

        /// <summary>As 128 combinacoes: nunca fecha o que esta fechado, e confirmacao aberta sempre fecha primeiro.</summary>
        [Test]
        public void TodasAsCombinacoes_SoFechaOQueEstaAberto()
        {
            for (int bits = 0; bits < 128; bits++)
            {
                var e = new VoltarHud.Estado
                {
                    saida = (bits & 1) != 0, entrada = (bits & 2) != 0, entradaPodeVoltar = (bits & 4) != 0,
                    gancho = (bits & 8) != 0, conversa = (bits & 16) != 0, menu = (bits & 32) != 0, salto = (bits & 64) != 0,
                };
                AcaoDoVoltar a = D(e);
                string caso = "combinacao " + bits;
                if (e.saida) Assert.AreEqual(AcaoDoVoltar.FecharSaida, a, caso);
                if (a == AcaoDoVoltar.FecharSaida) Assert.IsTrue(e.saida, caso);
                if (a == AcaoDoVoltar.VoltarEntrada) Assert.IsTrue(e.entrada && e.entradaPodeVoltar, caso);
                if (a == AcaoDoVoltar.FecharConversa) Assert.IsTrue(e.conversa, caso);
                if (a == AcaoDoVoltar.FecharMenu) Assert.IsTrue(e.menu, caso);
                if (a == AcaoDoVoltar.FecharSalto) Assert.IsTrue(e.salto, caso);
                if (a == AcaoDoVoltar.Nada) Assert.IsTrue(e.gancho, caso);
                if (a == AcaoDoVoltar.PedirSaida) Assert.IsFalse(e.saida, caso);
                // Fora da entrada, so pede saida com tudo fechado (na entrada, o que esta por baixo dela nao conta).
                if (a == AcaoDoVoltar.PedirSaida && !e.entrada) Assert.IsFalse(e.gancho || e.conversa || e.menu || e.salto, caso);
            }
        }
    }

    /// <summary>docs/PROJETO.md §6, Tecnico 3: no canhoto o texto do HUD de desempenho passava por cima do botao USAR.
    /// O texto (fonte 28 px, com a linha de diagnostico) nao pode cruzar nenhum botao de toque, nas duas maos.
    /// Telas: o modo celular do PC (1200x540, dpi simulado do PlayerInputReader), o POCO F4 (2400x1080, 2,75x) e uma
    /// tela baixa e densa (1600x720, 2x). ponytail: mora aqui por regra da leva (um arquivo de teste novo); o lugar
    /// natural e PerfHudTests.</summary>
    public class PerfHudCanhotoTests
    {
        const int Fonte = 28;   // PerfHud.fontSize padrao

        static readonly (float W, float H, float Dpi)[] Telas =
        {
            (1200f, 540f, 540f * 160f / 393f),
            (2400f, 1080f, 440f),
            (1600f, 720f, 320f),
        };

        ControlPreset preset;

        [TearDown]
        public void Destruir() { if (preset != null) Object.DestroyImmediate(preset); }

        static bool Cruza(ControlPreset p, Rect tela, float dpi, Rect texto)
        {
            for (int i = 0; i < p.buttons.Length; i++)
            {
                Vector2 c = p.ButtonCenterPx(i, tela, dpi);   // origem embaixo
                float r = p.ButtonRadiusPx(i, dpi);
                if (new Rect(c.x - r, tela.height - c.y - r, 2f * r, 2f * r).Overlaps(texto)) return true;   // GUI: origem em cima
            }
            return false;
        }

        [Test]
        public void DuasMaos_TextoNaoCruzaBotao()
        {
            foreach (HandPreset mao in new[] { HandPreset.Destro, HandPreset.Canhoto })
            {
                preset = ControlPreset.Default(mao);
                foreach (var t in Telas)
                {
                    var tela = new Rect(0f, 0f, t.W, t.H);
                    float x = PerfHud.XDoTexto(preset, tela, t.Dpi, t.H, Fonte, true);
                    Assert.IsFalse(Cruza(preset, tela, t.Dpi, PerfHud.AreaDoTexto(x, Fonte, true)), mao + " " + t.W + "x" + t.H);
                    if (mao == HandPreset.Destro) Assert.AreEqual(10f, x, "destro: botoes a direita, o texto fica no canto");
                }
                Object.DestroyImmediate(preset);
                preset = null;
            }
        }

        [Test]
        public void Canhoto_NoModoCelularDoPc_CruzavaOUsar_EAgoraAndaParaADireita()
        {
            preset = ControlPreset.Default(HandPreset.Canhoto);
            var tela = new Rect(0f, 0f, 1200f, 540f);
            float dpi = Telas[0].Dpi;

            Assert.IsTrue(Cruza(preset, tela, dpi, PerfHud.AreaDoTexto(10f, Fonte, true)), "no canto, o texto cobria o USAR (o defeito)");
            float x = PerfHud.XDoTexto(preset, tela, dpi, tela.height, Fonte, true);
            Assert.Greater(x, 10f);
            Assert.Less(x, tela.width * 0.25f, "anda so o necessario: passa a coluna da esquerda, nao vai para o meio");
        }
    }
}
