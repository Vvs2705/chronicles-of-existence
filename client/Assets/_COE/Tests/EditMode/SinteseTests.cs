using System;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>O som sintetizado: nunca estoura (pico abaixo de 1), a musica emenda sem clique no laco, cada efeito
    /// tem som de verdade, e a mesma chamada da os mesmos numeros (sem aleatorio solto).</summary>
    public class SinteseTests
    {
        static float Pico(float[] b) { float m = 0f; foreach (float x in b) m = Math.Max(m, Math.Abs(x)); return m; }

        [Test]
        public void TodaTrilha_DuraAVolta_NaoEstoura_EmendaSemClique_EEDeterministica()
        {
            foreach (TrilhaMusical t in Enum.GetValues(typeof(TrilhaMusical)))
            {
                float[] m = Sintese.Trilha(t);
                Assert.AreEqual((int)(Sintese.SegundosDaTrilha(t) * Sintese.Taxa), m.Length, t + " com duracao errada");
                Assert.That(Sintese.SegundosDaTrilha(t), Is.InRange(12f, 30f), t + ": laco curto cansa, longo pesa na memoria");
                Assert.That(Pico(m), Is.InRange(0.5f, 0.81f), t + " muda ou estourando");
                Assert.Less(Math.Abs(m[0] - m[m.Length - 1]), 0.05f, t + ": o laco estala na emenda");
                CollectionAssert.AreEqual(m, Sintese.Trilha(t), t + " muda entre chamadas");
            }
        }

        /// <summary>Cada momento soa diferente: nenhuma trilha e a mesma de outra (nem a noite e o dia mais lento).</summary>
        [Test]
        public void Trilhas_SaoDiferentesEntreSi()
        {
            var vistas = new System.Collections.Generic.List<float[]>();
            foreach (TrilhaMusical t in Enum.GetValues(typeof(TrilhaMusical)))
            {
                float[] m = Sintese.Trilha(t);
                foreach (float[] outra in vistas)
                {
                    int n = Math.Min(m.Length, outra.Length);
                    double dif = 0;
                    for (int i = 0; i < n; i++) dif += Math.Abs(m[i] - outra[i]);
                    Assert.Greater(dif / n, 0.02, t + " igual a outra trilha");
                }
                vistas.Add(m);
            }
        }

        [Test]
        public void TodoEfeito_TemSom_NaoEstoura_EEDeterministico()
        {
            foreach (Som s in Enum.GetValues(typeof(Som)))
            {
                float[] e = Sintese.Efeito(s);
                Assert.Greater(e.Length, Sintese.Taxa / 50, s + " curto demais");
                Assert.That(Pico(e), Is.InRange(0.3f, 1f), s + " mudo ou estourando");
                CollectionAssert.AreEqual(e, Sintese.Efeito(s), s + " muda entre chamadas");
            }
        }
    }
}
