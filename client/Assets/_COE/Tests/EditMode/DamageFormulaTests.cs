using NUnit.Framework;

namespace COE.Tests
{
    public class DamageFormulaTests
    {
        [Test]
        public void Compute_SegueFormulaDoDesign()
        {
            // base 14 em defesa 20 = 14*100/120
            Assert.AreEqual(11.67f, Damage.Compute(14f, 20f), 0.01f);
            // base 160 em defesa 8 ~ 148
            Assert.AreEqual(148.15f, Damage.Compute(160f, 8f), 0.01f);
        }

        [Test]
        public void Compute_NuncaAbaixoDeUm()
        {
            Assert.AreEqual(1f, Damage.Compute(10f, 1000f));
        }
    }
}
