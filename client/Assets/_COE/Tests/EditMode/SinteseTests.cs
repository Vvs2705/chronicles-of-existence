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
        public void Musica_DuraOCompassoCerto_NaoEstoura_EEmendaSemClique()
        {
            float[] m = Sintese.Musica();
            Assert.AreEqual((int)(Sintese.SegundosDaMusica * Sintese.Taxa), m.Length);
            Assert.That(Pico(m), Is.InRange(0.5f, 0.81f), "musica muda ou estourando");
            Assert.Less(Math.Abs(m[0] - m[m.Length - 1]), 0.1f, "o laco estala na emenda");
            CollectionAssert.AreEqual(m, Sintese.Musica(), "a musica muda entre chamadas");
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
