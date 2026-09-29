using System.Linq;
using COE.EditorTools;
using NUnit.Framework;

namespace COE.EditorTests
{
    public class HumanoidMappingTests
    {
        static readonly string[] Mixamo =
        {
            "Model", "Breathing Idle", "Running", "Sword And Shield Slash", "Sword And Shield Slash (1)",
            "Sword And Shield Attack", "Stand To Roll", "Sword And Shield Impact", "Dying",
        };

        [Test]
        public void NomesPadraoDoMixamo_Mapeiam()
        {
            HumanoidFiles f = HumanoidMapping.Classify(Mixamo);
            Assert.AreEqual("Model", f.Model);
            Assert.AreEqual("Breathing Idle", f.Clips[HumanoidClip.Idle]);
            Assert.AreEqual("Running", f.Clips[HumanoidClip.Run]);
            Assert.AreEqual("Stand To Roll", f.Clips[HumanoidClip.Dodge]);
            Assert.AreEqual("Sword And Shield Impact", f.Clips[HumanoidClip.Hit]);
            Assert.AreEqual("Dying", f.Clips[HumanoidClip.Death]);
            Assert.IsEmpty(f.Missing.ToArray());
            Assert.IsEmpty(f.Unmapped);
        }

        [Test]
        public void NomeDesconhecido_FicaDeFora_EFaltaApareceEmMissing()
        {
            HumanoidFiles f = HumanoidMapping.Classify(new[] { "Model", "Idle", "Samba Dancing", "Idle (2)" });
            CollectionAssert.AreEquivalent(new[] { "Samba Dancing", "Idle (2)" }, f.Unmapped);
            Assert.AreEqual("Idle", f.Clips[HumanoidClip.Idle]);
            CollectionAssert.Contains(f.Missing.ToArray(), HumanoidClip.Run);
            Assert.IsNull(HumanoidMapping.Match("Samba Dancing"));
        }

        [Test]
        public void TresAtaquesOrdenados_ViramAttackIndex012_EQuartoSobra()
        {
            HumanoidFiles f = HumanoidMapping.Classify(new[] { "Model", "Attack3", "Attack1", "Attack4", "Attack2" });
            Assert.AreEqual("Attack1", f.Clips[HumanoidClip.Attack1]);
            Assert.AreEqual("Attack2", f.Clips[HumanoidClip.Attack2]);
            Assert.AreEqual("Attack3", f.Clips[HumanoidClip.Attack3]);
            CollectionAssert.AreEqual(new[] { "Attack4" }, f.Unmapped);
            Assert.AreEqual(0, HumanoidMapping.AttackIndex(HumanoidClip.Attack1));
            Assert.AreEqual(1, HumanoidMapping.AttackIndex(HumanoidClip.Attack2));
            Assert.AreEqual(2, HumanoidMapping.AttackIndex(HumanoidClip.Attack3));

            HumanoidFiles m = HumanoidMapping.Classify(Mixamo); // ordem alfabetica
            Assert.AreEqual("Sword And Shield Attack", m.Clips[HumanoidClip.Attack1]);
            Assert.AreEqual("Sword And Shield Slash", m.Clips[HumanoidClip.Attack2]);
            Assert.AreEqual("Sword And Shield Slash (1)", m.Clips[HumanoidClip.Attack3]);
        }

        [Test]
        public void Eventos_NosTemposDoPipeline()
        {
            Assert.AreEqual((AnimParams.EventHitFrame, 0.40f), HumanoidMapping.Events(HumanoidClip.Attack1).Single());
            Assert.AreEqual((AnimParams.EventHitFrame, 0.40f), HumanoidMapping.Events(HumanoidClip.Attack2).Single());
            Assert.AreEqual((AnimParams.EventHitFrame, 0.45f), HumanoidMapping.Events(HumanoidClip.Attack3).Single());
            var dodge = HumanoidMapping.Events(HumanoidClip.Dodge);
            CollectionAssert.Contains(dodge.Select(e => e.Method).ToArray(), AnimParams.EventDodgeEnd);
            CollectionAssert.Contains(dodge.Select(e => e.Method).ToArray(), AnimParams.EventFootstep);
            Assert.AreEqual(2, HumanoidMapping.Events(HumanoidClip.Run).Count(e => e.Method == AnimParams.EventFootstep));
            Assert.IsEmpty(HumanoidMapping.Events(HumanoidClip.Idle));
            Assert.IsTrue(HumanoidMapping.Loops(HumanoidClip.Idle) && HumanoidMapping.Loops(HumanoidClip.Run));
            Assert.IsFalse(HumanoidMapping.Loops(HumanoidClip.Death));
        }
    }
}
