using System.Collections.Generic;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Fases da vida (T009). As faixas vem do GDD v1.2 cap. 03 "Etapas narrativas" -- se alguem
    /// mudar um numero aqui, esta mudando o GDD em silencio, e o teste quebra.</summary>
    public class LifePhasesTests
    {
        [Test]
        public void De_RespeitaAsFaixasDoGdd()
        {
            Assert.AreEqual(LifePhase.PrimeirasDescobertas, LifePhases.De(5));
            Assert.AreEqual(LifePhase.PrimeirasDescobertas, LifePhases.De(7));
            Assert.AreEqual(LifePhase.DespertarDosTalentos, LifePhases.De(8));
            Assert.AreEqual(LifePhase.DespertarDosTalentos, LifePhases.De(11));
            Assert.AreEqual(LifePhase.Formacao, LifePhases.De(12));
            Assert.AreEqual(LifePhase.Formacao, LifePhases.De(15));
            Assert.AreEqual(LifePhase.Independencia, LifePhases.De(16));
            Assert.AreEqual(LifePhase.Independencia, LifePhases.De(18));
            Assert.AreEqual(LifePhase.Legado, LifePhases.De(19));
            Assert.AreEqual(LifePhase.Legado, LifePhases.De(60));
        }

        [Test]
        public void De_AbaixoDeCinco_NaoInventaFase()
        {
            Assert.AreEqual(LifePhase.PrimeirasDescobertas, LifePhases.De(0));
            Assert.AreEqual(LifePhase.PrimeirasDescobertas, LifePhases.De(-3), "save adulterado nao lanca");
        }

        [Test]
        public void IdadeMinima_ReconstroiAFase()
        {
            foreach (LifePhase f in System.Enum.GetValues(typeof(LifePhase)))
                Assert.AreEqual(f, LifePhases.De(LifePhases.IdadeMinima(f)), "primeira idade de " + f);
        }

        [Test]
        public void Id_EUnicoEEmSnakeCaseAscii()
        {
            HashSet<string> vistos = new HashSet<string>();
            foreach (LifePhase f in System.Enum.GetValues(typeof(LifePhase)))
            {
                string id = LifePhases.Id(f);
                Assert.IsTrue(vistos.Add(id), "id de fase repetido: " + id + " (ele e chave no save)");
                foreach (char c in id)
                    Assert.IsTrue((c >= 'a' && c <= 'z') || c == '_', "id de fase fora do ASCII minusculo: " + id);
            }
        }
    }
}
