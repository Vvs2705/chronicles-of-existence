using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Vigor e Mana: custo, saldo e regeneracao. Regra pura, sem cena (CLAUDE.md: EditMode e o padrao).</summary>
    public class CombatResourcesTests
    {
        [Test]
        public void Gastar_TiraDoSaldo_ESemSaldoNaoGasta()
        {
            var p = new ResourcePool(10f, 1f, 0f);
            Assert.IsTrue(p.TryGastar(6f));
            Assert.AreEqual(4f, p.Atual, 1e-4f);
            Assert.IsFalse(p.TryGastar(6f), "tudo ou nada: 6 nao sai de um saldo de 4");
            Assert.AreEqual(4f, p.Atual, 1e-4f, "gasto recusado nao pode tirar nada");
        }

        [Test]
        public void Regeneracao_SoComecaDepoisDoAtraso()
        {
            var p = new ResourcePool(10f, 5f, 1f); // 5/s depois de 1 s parado
            p.TryGastar(10f);
            p.Tick(0.9f);
            Assert.AreEqual(0f, p.Atual, 1e-4f, "dentro do atraso nao regenera");
            p.Tick(0.2f);                          // 0,1 s alem do atraso
            Assert.AreEqual(0.5f, p.Atual, 1e-3f, "a sobra do frame ja conta como regeneracao");
        }

        [Test]
        public void GastoNovo_ReiniciaOAtraso()
        {
            var p = new ResourcePool(10f, 5f, 1f);
            p.TryGastar(4f);
            p.Tick(0.9f);
            p.TryGastar(1f);                       // martelar o botao
            p.Tick(0.9f);
            Assert.AreEqual(5f, p.Atual, 1e-3f, "quem gasta de novo nunca chega a regenerar");
        }

        [Test]
        public void Regeneracao_NaoPassaDoMaximo()
        {
            var p = new ResourcePool(10f, 100f, 0f);
            p.TryGastar(5f);
            p.Tick(10f);
            Assert.AreEqual(10f, p.Atual, 1e-4f);
        }

        [Test]
        public void Drenar_TiraSoOQueTem()
        {
            var p = new ResourcePool(10f, 0f, 0f);
            p.TryGastar(7f);
            Assert.AreEqual(3f, p.Drenar(8f), 1e-4f, "drenar nao deixa saldo negativo");
            Assert.AreEqual(0f, p.Atual, 1e-4f);
        }

        [Test]
        public void CombatResources_NasceCheioComOsNumerosV0()
        {
            var r = new CombatResources();
            Assert.AreEqual(CombatMoves.VigorMaxV0, r.Vigor.Atual, 1e-4f);
            Assert.AreEqual(CombatMoves.ManaMaxV0, r.Mana.Atual, 1e-4f);
            Assert.AreEqual(1f, r.Vigor.Fracao, 1e-4f);
        }

        [Test]
        public void MagiaSemMana_NaoSai()
        {
            var r = new CombatResources();
            int lancadas = 0;
            for (int i = 0; i < 20; i++) if (r.Mana.TryGastar(CombatMoves.Magia.Mana)) lancadas++;
            Assert.AreEqual(3, lancadas, "30 de mana / 10 por magia = 3 e para; a quarta nao sai");
            Assert.IsFalse(r.Mana.Tem(CombatMoves.Magia.Mana));
        }
    }
}
