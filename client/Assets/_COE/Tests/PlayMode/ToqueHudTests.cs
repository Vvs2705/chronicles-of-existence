using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace COE.PlayModeTests
{
    /// <summary>Bloco D: os controles de toque em uGUI. O disco desenhado de cada botao e o alvo do toque (mesma geometria
    /// do ControlPreset, mesma area segura e densidade), a mao canhota espelha o desenho como espelha o hit-test, e com modal
    /// aberto nada fica por cima do painel.</summary>
    public class ToqueHudTests
    {
        GameObject leitorGo, hudGo;

        [TearDown]
        public void TearDown()
        {
            if (hudGo != null) Object.Destroy(hudGo);
            if (leitorGo != null) Object.Destroy(leitorGo);
        }

        IEnumerator Montar(HandPreset mao)
        {
            leitorGo = new GameObject("Input");
            leitorGo.SetActive(false);   // o modo celular de desenvolvimento entra antes do OnEnable
            PlayerInputReader leitor = leitorGo.AddComponent<PlayerInputReader>();
            typeof(PlayerInputReader).GetField("touchHudDev", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(leitor, true);
            leitor.Preset = ControlPreset.Default(mao);
            leitorGo.SetActive(true);
            hudGo = new GameObject("ToqueHud");
            hudGo.AddComponent<ToqueHud>().Leitor = leitor;
            yield return null;
            yield return null;
        }

        [UnityTest]
        public IEnumerator DiscoDesenhado_EOAlvoDoToque_NasDuasMaos()
        {
            foreach (HandPreset mao in new[] { HandPreset.Destro, HandPreset.Canhoto })
            {
                yield return Montar(mao);
                ToqueHud hud = hudGo.GetComponent<ToqueHud>();
                ControlPreset p = leitorGo.GetComponent<PlayerInputReader>().Preset;
                Assert.IsTrue(hud.Visivel, mao + ": modo celular mostra os controles");
                for (int i = 0; i < p.buttons.Length; i++)
                {
                    Rect r = Retangulo(hud.Botao(i));
                    Vector2 centro = p.ButtonCenterPx(i, Screen.safeArea, Tela.Dpi);
                    float raio = p.ButtonRadiusPx(i, Tela.Dpi);
                    Assert.AreEqual(centro.x, r.center.x, 0.5f, mao + " " + p.buttons[i].action + ": x do disco != x do alvo");
                    Assert.AreEqual(centro.y, r.center.y, 0.5f, mao + " " + p.buttons[i].action + ": y do disco != y do alvo");
                    Assert.AreEqual(2f * raio, r.width, 0.5f, mao + " " + p.buttons[i].action + ": tamanho do disco != alvo");
                    Assert.AreEqual(i, p.ButtonAt(centro, Screen.safeArea, Tela.Dpi), "tocar no centro desenhado acerta o botao");
                }
                TearDown();
                yield return null;
            }
        }

        [UnityTest]
        public IEnumerator ModalAberto_EscondeOsControles()
        {
            yield return Montar(HandPreset.Destro);
            ToqueHud hud = hudGo.GetComponent<ToqueHud>();
            Assert.IsTrue(hud.Visivel);
            UiFundo.MarcarModal();
            yield return null;
            Assert.IsFalse(hud.Visivel, "botao de toque por cima do painel do modal");
            yield return null;
            yield return null;
            Assert.IsTrue(hud.Visivel, "fechou o modal, os controles voltam");
        }

        // Retangulo do RectTransform em px de tela (Canvas overlay 1:1: mundo = tela).
        static Rect Retangulo(RectTransform rt)
        {
            var c = new Vector3[4];
            rt.GetWorldCorners(c);
            return new Rect(c[0].x, c[0].y, c[2].x - c[0].x, c[2].y - c[0].y);
        }
    }
}
