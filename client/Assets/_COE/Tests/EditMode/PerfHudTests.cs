using System;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>O CSV de desempenho so existe em build de desenvolvimento: em release o PerfHud mede e desenha, mas nao
    /// escreve arquivo por segundo no aparelho do jogador.</summary>
    public class PerfHudTests
    {
        static readonly DateTime Agora = new DateTime(2026, 9, 30, 14, 5, 9);

        [Test]
        public void Release_NaoTemCsv()
        {
            Assert.IsNull(PerfHud.CaminhoDoCsv(false, "pasta", "POCO F4", Agora), "sem caminho = o Update nao grava");
        }

        [Test]
        public void Desenvolvimento_GravaNaPastaDada_ComAparelhoEHora()
        {
            string caminho = PerfHud.CaminhoDoCsv(true, "pasta", "POCO F4", Agora);

            StringAssert.StartsWith("pasta", caminho);
            StringAssert.EndsWith("perf_POCO_F4_20260930_140509.csv", caminho);
        }
    }
}
