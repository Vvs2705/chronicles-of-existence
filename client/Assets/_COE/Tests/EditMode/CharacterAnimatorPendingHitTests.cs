using NUnit.Framework;

namespace COE.Tests
{
    public class CharacterAnimatorPendingHitTests
    {
        [Test]
        public void DisparaNoEvento_UmaVezSo()
        {
            var p = new PendingHit();
            int hits = 0;
            p.Schedule(delegate { hits++; });
            Assert.AreEqual(0, hits, "espera o OnHitFrame");
            Assert.IsTrue(p.Pending);
            p.Fire();
            Assert.AreEqual(1, hits);
            p.Fire();          // segundo OnHitFrame do mesmo clip
            p.Tick(10f);       // timeout depois do evento
            Assert.AreEqual(1, hits, "nao dispara duas vezes");
            Assert.IsFalse(p.Pending);
        }

        [Test]
        public void SemEvento_DisparaNoTimeout_UmaVezSo()
        {
            var p = new PendingHit();
            int hits = 0;
            p.Schedule(delegate { hits++; }, 0.6f);
            p.Tick(0.5f);
            Assert.AreEqual(0, hits, "ainda dentro do fallback");
            p.Tick(0.2f);
            Assert.AreEqual(1, hits, "clip sem evento / interrompido: o dano sai no timeout");
            p.Tick(1f);
            p.Fire();          // evento atrasado
            Assert.AreEqual(1, hits);
        }

        [Test]
        public void ScheduleDuplo_DisparaOAntigoAntes_ENaoPerdeNenhum()
        {
            var p = new PendingHit();
            int first = 0, second = 0;
            p.Schedule(delegate { first++; });
            p.Schedule(delegate { second++; });
            Assert.AreEqual(1, first, "o pendente antigo sai na hora");
            Assert.AreEqual(0, second);
            p.Fire();
            Assert.AreEqual(1, first);
            Assert.AreEqual(1, second);
        }

        [Test]
        public void NovoSchedule_ReiniciaOTimeout()
        {
            var p = new PendingHit();
            int hits = 0;
            p.Schedule(delegate { }, 0.6f);
            p.Tick(0.5f);
            p.Schedule(delegate { hits++; }, 0.6f);
            p.Tick(0.5f);
            Assert.AreEqual(0, hits, "timeout conta do novo golpe");
            p.Tick(0.2f);
            Assert.AreEqual(1, hits);
        }

        [Test]
        public void Clear_DescartaSemEntregar()
        {
            var p = new PendingHit();
            int hits = 0;
            p.Schedule(delegate { hits++; });
            p.Clear();
            p.Fire();
            p.Tick(10f);
            Assert.AreEqual(0, hits);
        }
    }
}
