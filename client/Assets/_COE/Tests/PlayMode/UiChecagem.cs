using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace COE.PlayModeTests
{
    /// <summary>Conferencias de tela das HUDs em uGUI (Bloco D): alvo de toque >= 48 dp, dentro da area segura, sem um
    /// botao por cima do outro. Canvas overlay em pixel 1:1: canto do mundo = pixel da tela.</summary>
    public static class UiChecagem
    {
        public static Rect Retangulo(RectTransform rt)
        {
            var c = new Vector3[4];
            rt.GetWorldCorners(c);
            return new Rect(c[0].x, c[0].y, c[2].x - c[0].x, c[2].y - c[0].y);
        }

        public static List<Button> BotoesAtivos(Component raiz)
        {
            return raiz.GetComponentsInChildren<Button>(false).Where(b => b.isActiveAndEnabled).ToList();
        }

        public static Button Botao(Component raiz, string nome)
        {
            Button b = BotoesAtivos(raiz).FirstOrDefault(x => x.name == nome);
            Assert.IsNotNull(b, "botao '" + nome + "' nao esta na tela");
            return b;
        }

        /// <summary>Alvo >= 48 dp, inteiro na area segura, sem sobrepor outro botao ativo.</summary>
        public static void BotoesUsaveis(Component raiz, string caso)
        {
            List<Button> botoes = BotoesAtivos(raiz);
            Rect safe = Screen.safeArea;
            float minimo = ControlPreset.DpToPx(ControlPreset.MinTargetDp, Tela.Dpi) - 0.5f;
            for (int i = 0; i < botoes.Count; i++)
            {
                Rect r = Retangulo((RectTransform)botoes[i].transform);
                string nome = caso + "/" + botoes[i].name;
                Assert.GreaterOrEqual(r.height, minimo, nome + ": alvo de toque abaixo de 48 dp (" + r + ", tela " + Screen.width + "x" + Screen.height + ")");
                Assert.GreaterOrEqual(r.width, minimo, nome + ": alvo de toque abaixo de 48 dp");
                Assert.IsTrue(safe.xMin - 0.5f <= r.xMin && r.xMax <= safe.xMax + 0.5f && safe.yMin - 0.5f <= r.yMin && r.yMax <= safe.yMax + 0.5f,
                    nome + ": fora da area segura " + r + " / " + safe);
                for (int j = i + 1; j < botoes.Count; j++)
                {
                    Rect o = Retangulo((RectTransform)botoes[j].transform);
                    Assert.IsFalse(r.Overlaps(o), nome + " sobrepoe " + botoes[j].name);
                }
            }
        }
    }
}
