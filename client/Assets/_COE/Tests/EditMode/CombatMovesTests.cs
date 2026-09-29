using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Catalogo v0 das acoes e regra do bloqueio. Numeros sao hipotese; o que estes testes travam sao as
    /// RELACOES entre eles (forte custa mais e bate mais, magia gasta mana e nao vigor).</summary>
    public class CombatMovesTests
    {
        [Test]
        public void Forte_CustaMaisEBateMaisQueLeve()
        {
            Assert.Greater(CombatMoves.Forte.Vigor, CombatMoves.Leve.Vigor, "ataque forte tem de custar mais Vigor");
            Assert.Greater(CombatMoves.Forte.Dano, CombatMoves.Leve.Dano, "ataque forte tem de bater mais");
            Assert.Greater(CombatMoves.Forte.Recuperacao, CombatMoves.Leve.Recuperacao, "forte fica mais tempo exposto");
        }

        [Test]
        public void Magia_GastaManaENaoVigor()
        {
            Assert.Greater(CombatMoves.Magia.Mana, 0f);
            Assert.AreEqual(0f, CombatMoves.Magia.Vigor, 1e-4f);
            Assert.AreEqual(CombatMoves.AfinidadeArcana, CombatMoves.Magia.Afinidade);
            Assert.AreEqual(CombatMoves.AfinidadeMarcial, CombatMoves.Leve.Afinidade);
        }

        [Test]
        public void Esquiva_CustaVigorEDaIFrames()
        {
            Assert.Greater(CombatMoves.Esquiva.Vigor, 0f, "esquiva de graca vira invulnerabilidade infinita");
            Assert.Greater(CombatMoves.IFramesEsquiva, 0f);
        }

        [Test]
        public void Bloqueio_ReduzDanoFrontal()
        {
            float passou = BlockRule.Reduzir(100f, 0f, true);
            Assert.AreEqual(100f * (1f - CombatMoves.ReducaoBloqueioFrontal), passou, 1e-3f);
            Assert.Less(passou, 100f);
        }

        [Test]
        public void Bloqueio_NaoProtegeDasCostas()
        {
            Assert.AreEqual(100f, BlockRule.Reduzir(100f, 180f, true), 1e-3f, "golpe pelas costas passa inteiro");
            Assert.AreEqual(100f, BlockRule.Reduzir(100f, CombatMoves.ConeBloqueioGraus + 1f, true), 1e-3f,
                            "logo fora do cone tambem passa inteiro");
            Assert.Less(BlockRule.Reduzir(100f, CombatMoves.ConeBloqueioGraus - 1f, true), 100f,
                        "dentro do cone, protege");
        }

        [Test]
        public void Bloqueio_SemVigor_NaoProtege()
        {
            Assert.AreEqual(100f, BlockRule.Reduzir(100f, 0f, false), 1e-3f, "guarda quebrada nao absorve nada");
        }
    }
}
