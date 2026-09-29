using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace COE.Tests
{
    /// <summary>ADR-0006: regra do toque em PAISAGEM (TouchControls + ControlPreset), sem cena e sem dispositivo.
    /// dpi 160 = 1 dp por px, entao as contas abaixo estao em dp. Tela 1280x720 (area segura = tela, salvo no teste
    /// da safe area). Preset destro padrao: joystick na metade esquerda, camera na direita, botoes no canto inferior
    /// direito.</summary>
    public class TouchControlsTests
    {
        const float Dpi = 160f;
        Rect tela;
        ControlPreset preset;
        TouchControls toque;

        [SetUp]
        public void Montar()
        {
            tela = new Rect(0f, 0f, 1280f, 720f);
            preset = ControlPreset.Default(HandPreset.Destro);
            toque = new TouchControls();
        }

        [TearDown]
        public void Destruir() { Object.DestroyImmediate(preset); }

        // ---------- joystick ----------

        [Test]
        public void Joystick_DentroDaDeadZone_NaoMove()
        {
            Frame(Down(1, 200f, 200f));
            Assert.IsTrue(toque.JoystickActive, "toque na metade esquerda vira joystick");
            Assert.AreEqual(Vector2.zero, toque.Move, "o ponto do toque e o centro do joystick flutuante");
            Frame(Hold(1, 205f, 200f)); // 5 de 60 dp = 0,08 < dead zone 0,12
            Assert.AreEqual(Vector2.zero, toque.Move);
        }

        [Test]
        public void Joystick_MeioRaioDaMeiaVelocidade_ForaDoRaioDirecaoNormalizada()
        {
            Frame(Down(1, 200f, 200f));
            Frame(Hold(1, 230f, 200f)); // 30 de 60 dp
            AssertVec(new Vector2(0.5f, 0f), toque.Move, "analogico: meio raio = meia deflexao");
            Frame(Hold(1, 500f, 600f)); // (300, 400) dp: muito alem do raio
            AssertVec(new Vector2(0.6f, 0.8f), toque.Move, "direcao preservada");
            Assert.AreEqual(1f, toque.Move.magnitude, 1e-4f, "alem do raio satura em 1");
        }

        [Test]
        public void Joystick_RaioEmDp_AcompanhaODpi()
        {
            toque.Update(new[] { Down(1, 200f, 200f) }, preset, tela, 320f);
            toque.Update(new[] { Hold(1, 260f, 200f) }, preset, tela, 320f); // 60 px a 320 dpi = 30 dp
            AssertVec(new Vector2(0.5f, 0f), toque.Move, "o raio de 60 dp vira 120 px em tela de 320 dpi");
        }

        [Test]
        public void Joystick_Soltar_ParaNaHora()
        {
            Frame(Down(1, 200f, 200f));
            Frame(Hold(1, 260f, 200f));
            Frame(Up(1, 260f, 200f));
            Assert.IsFalse(toque.JoystickActive);
            Assert.AreEqual(Vector2.zero, toque.Move, "soltou = parado, sem Move residual");
        }

        // ---------- botoes ----------

        [Test]
        public void CadaBotao_NaZonaCerta_ViraAAcaoCerta_SoNoFrameDoToque()
        {
            foreach (TouchAction a in System.Enum.GetValues(typeof(TouchAction)))
            {
                var t = new TouchControls();
                Vector2 c = Centro(a);
                t.Update(new[] { Down(1, c.x, c.y) }, preset, tela, Dpi);
                foreach (TouchAction outra in System.Enum.GetValues(typeof(TouchAction)))
                    Assert.AreEqual(outra == a, t.Pressed(outra), "toque no botao " + a + " acionou " + outra);
                Assert.IsFalse(t.JoystickActive, a + ": botao nao vira joystick");
                Assert.AreEqual(Vector2.zero, t.Look, a + ": botao nao gira camera");

                t.Update(new[] { Hold(1, c.x, c.y) }, preset, tela, Dpi);
                Assert.IsFalse(t.Pressed(a), a + ": Pressed vale so no frame do toque");
            }
        }

        [Test]
        public void SegurarDefesa_MantemHeld_MesmoEscorregando_AteSoltar()
        {
            Vector2 d = Centro(TouchAction.Defesa);
            Frame(Down(1, d.x, d.y));
            Assert.IsTrue(toque.Held(TouchAction.Defesa), "defende ja no frame do toque");
            for (int k = 1; k <= 3; k++)
            {
                Frame(Hold(1, d.x - 80f, d.y + 10f * k)); // o dedo sai do botao (raio 34 dp)
                Assert.IsTrue(toque.Held(TouchAction.Defesa), "frame " + k + ": o dedo e dono da defesa ate soltar");
                Assert.AreEqual(Vector2.zero, toque.Look, "o dedo da defesa nao vira camera");
            }
            Frame(Up(1, d.x - 80f, d.y + 30f));
            Assert.IsFalse(toque.Held(TouchAction.Defesa));
            Frame();
            Assert.IsFalse(toque.Held(TouchAction.Defesa));
        }

        // ---------- multitoque ----------

        [Test]
        public void Multitoque_JoystickCameraEBotao_Independentes()
        {
            Vector2 atq = Centro(TouchAction.Ataque);
            Frame(Down(1, 200f, 200f), Down(2, 900f, 500f), Down(3, atq.x, atq.y));
            Assert.IsTrue(toque.JoystickActive);
            Assert.IsTrue(toque.Pressed(TouchAction.Ataque));

            Frame(Hold(1, 260f, 200f), Hold(2, 1000f, 500f), Hold(3, atq.x, atq.y));
            AssertVec(new Vector2(1f, 0f), toque.Move, "joystick");
            AssertVec(new Vector2(100f * preset.lookSensitivityX, 0f), toque.Look, "camera: 100 dp arrastados");
            Assert.IsFalse(toque.Pressed(TouchAction.Ataque), "segurar o ataque nao repete o golpe");
            Assert.IsTrue(toque.Held(TouchAction.Ataque));

            // Polegar do joystick invade o lado da camera: continua joystick. Dedo extra no lado do joystick: ignorado.
            Frame(Hold(1, 700f, 200f), Hold(2, 1000f, 520f), Up(3, atq.x, atq.y), Down(4, 100f, 100f));
            AssertVec(new Vector2(1f, 0f), toque.Move, "o dedo do joystick nao troca de papel");
            AssertVec(new Vector2(200f, 200f), toque.JoystickAnchor, "segundo dedo nao reposiciona o joystick");
            AssertVec(new Vector2(0f, 20f * preset.lookSensitivityY), toque.Look, "camera segue so com o dedo dela");
            Assert.IsFalse(toque.Held(TouchAction.Ataque));
        }

        [Test]
        public void DedoSumiuSemEnded_SoltaJoystickEDefesa()
        {
            Vector2 d = Centro(TouchAction.Defesa);
            Frame(Down(1, 200f, 200f), Down(2, d.x, d.y));
            Frame(); // app pausado: os dedos somem sem fase Ended
            Assert.IsFalse(toque.JoystickActive, "joystick preso");
            Assert.IsFalse(toque.Held(TouchAction.Defesa), "defesa presa");
        }

        // ---------- mao e area segura ----------

        [Test]
        public void Canhoto_EspelhaZonasEBotoes()
        {
            var destro = new List<Vector2>();
            for (int i = 0; i < preset.buttons.Length; i++) destro.Add(preset.ButtonCenterPx(i, tela, Dpi));

            preset.hand = HandPreset.Canhoto;
            for (int i = 0; i < preset.buttons.Length; i++)
                AssertVec(new Vector2(tela.width - destro[i].x, destro[i].y), preset.ButtonCenterPx(i, tela, Dpi),
                          preset.buttons[i].action + " espelhado");

            Frame(Down(1, 400f, 500f)); // acima dos botoes, que agora estao na esquerda
            Assert.IsFalse(toque.JoystickActive, "canhoto: a esquerda e da camera");
            Frame(Hold(1, 440f, 500f));
            Assert.Greater(toque.Look.x, 0f);
            Frame(Up(1, 440f, 500f), Down(2, 1080f, 400f));
            Assert.IsTrue(toque.JoystickActive, "canhoto: o joystick e na direita");
            Vector2 atq = Centro(TouchAction.Ataque);
            Assert.Less(atq.x, tela.width * 0.5f, "canhoto: botoes no canto inferior esquerdo");
            Frame(Hold(2, 1080f, 400f), Down(3, atq.x, atq.y));
            Assert.IsTrue(toque.Pressed(TouchAction.Ataque));
        }

        [Test]
        public void SafeArea_DeslocaBotoesEDivisa()
        {
            tela = new Rect(0f, 60f, 1192f, 660f); // notch a direita (88 px) e barra de gestos embaixo (60 px)
            Vector2 atq = Centro(TouchAction.Ataque);
            AssertVec(new Vector2(1192f - 90f, 60f + 90f), atq, "o botao conta do canto da area segura");

            Frame(Down(1, 1280f - 90f, 90f)); // onde o ataque ficaria na tela cheia (embaixo do notch/barra)
            Assert.IsFalse(toque.Pressed(TouchAction.Ataque));
            Frame(Up(1, 1280f - 90f, 90f), Down(2, atq.x, atq.y));
            Assert.IsTrue(toque.Pressed(TouchAction.Ataque));

            Frame(Hold(2, atq.x, atq.y), Down(3, 620f, 300f)); // divisa = meio da area segura (596), nao da tela (640)
            Assert.IsFalse(toque.JoystickActive, "x=620 ja e o lado da camera nesta area segura");
        }

        [Test]
        public void LayoutPadrao_CabeEm640x360dp_SemSobrepor_AlvosDe48dp()
        {
            var referencia = new Rect(0f, 0f, 640f, 360f); // celular pequeno em paisagem
            var acoes = new HashSet<TouchAction>();
            for (int i = 0; i < preset.buttons.Length; i++)
            {
                TouchAction a = preset.buttons[i].action;
                Assert.IsTrue(acoes.Add(a), a + " repetido");
                float r = preset.ButtonRadiusPx(i, Dpi);
                Vector2 c = preset.ButtonCenterPx(i, referencia, Dpi);
                Assert.GreaterOrEqual(2f * r, ControlPreset.MinTargetDp, a + ": alvo menor que 48 dp");
                Assert.GreaterOrEqual(c.x - r, 320f, a + ": invade o lado do joystick");
                Assert.LessOrEqual(c.x + r, 640f, a + ": sai da tela");
                Assert.GreaterOrEqual(c.y - r, 0f, a + ": sai por baixo");
                Assert.LessOrEqual(c.y + r, 360f, a + ": sai por cima");
                for (int j = 0; j < i; j++)
                    Assert.Greater(Vector2.Distance(c, preset.ButtonCenterPx(j, referencia, Dpi)), r + preset.ButtonRadiusPx(j, Dpi),
                                   a + " sobrepoe " + preset.buttons[j].action);
            }
            Assert.AreEqual(System.Enum.GetValues(typeof(TouchAction)).Length, acoes.Count, "falta botao para alguma acao");

            preset.buttons[0].radiusDp = 10f; // asset mal calibrado
            Assert.AreEqual(ControlPreset.MinTargetDp * 0.5f, preset.ButtonRadiusPx(0, Dpi), 1e-4f, "alvo e forcado a 48 dp");
        }

        // ---------- apoio ----------

        void Frame(params TouchPoint[] toques) { toque.Update(toques, preset, tela, Dpi); }

        static TouchPoint Down(int id, float x, float y) { return new TouchPoint(id, new Vector2(x, y), true, false); }
        static TouchPoint Hold(int id, float x, float y) { return new TouchPoint(id, new Vector2(x, y), false, false); }
        static TouchPoint Up(int id, float x, float y) { return new TouchPoint(id, new Vector2(x, y), false, true); }

        Vector2 Centro(TouchAction a)
        {
            for (int i = 0; i < preset.buttons.Length; i++)
                if (preset.buttons[i].action == a) return preset.ButtonCenterPx(i, tela, Dpi);
            Assert.Fail("preset sem botao de " + a);
            return Vector2.zero;
        }

        static void AssertVec(Vector2 esperado, Vector2 real, string msg)
        {
            Assert.AreEqual(esperado.x, real.x, 1e-3f, msg + " (x)");
            Assert.AreEqual(esperado.y, real.y, 1e-3f, msg + " (y)");
        }
    }
}
