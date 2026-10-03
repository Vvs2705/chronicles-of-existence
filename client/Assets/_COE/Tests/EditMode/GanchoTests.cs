using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>B16 — o gancho: aparece quando o treino do B15 termina (cada um dos quatro verbos ao menos uma vez:
    /// ataque leve, ataque forte, defesa OU esquiva, magia) e uma vez so — vira marco no historico, sobrevive a recarregar.</summary>
    public class GanchoTests
    {
        static SaveData OitoAnos() { return new SaveData { ageYears = 8 }; }

        static void Treinar(SaveData s, params AtividadeDef[] atividades)
        {
            foreach (AtividadeDef a in atividades) Mastery.Praticar(s, a);
        }

        [Test]
        public void Pendente_SoDepoisDosQuatroVerbosDoTreino()
        {
            SaveData s = OitoAnos();
            var g = new GameSession(s, null);
            Assert.IsFalse(g.GanchoPendente(), "sem treino nao ha gancho");

            Treinar(s, TrainingProgress.AtividadeLeve, TrainingProgress.AtividadeForte, TrainingProgress.AtividadeMagia);
            Assert.IsFalse(g.GanchoPendente(), "faltou defesa/esquiva");

            Treinar(s, TrainingProgress.AtividadeBloqueio);
            Assert.IsTrue(g.GanchoPendente(), "os quatro verbos: o slice chegou ao fim");
        }

        [Test]
        public void EsquivaValeComoOTerceiroVerbo_ERepetirUmSoNaoBasta()
        {
            SaveData s = OitoAnos();
            var g = new GameSession(s, null);
            for (int i = 0; i < 20; i++) Treinar(s, TrainingProgress.AtividadeLeve);
            Assert.IsFalse(g.GanchoPendente(), "repetir o mesmo golpe nao completa o treino");

            Treinar(s, TrainingProgress.AtividadeForte, TrainingProgress.AtividadeEsquiva, TrainingProgress.AtividadeMagia);
            Assert.IsTrue(g.GanchoPendente());
        }

        [Test]
        public void VerGancho_GravaUmaVez_ENaoVoltaAoRecarregar()
        {
            SaveData s = OitoAnos();
            int gravacoes = 0;
            var g = new GameSession(s, () => gravacoes++);
            Treinar(s, TrainingProgress.AtividadeLeve, TrainingProgress.AtividadeForte, TrainingProgress.AtividadeBloqueio, TrainingProgress.AtividadeMagia);

            Assert.IsTrue(g.VerGancho());
            Assert.AreEqual(1, gravacoes, "o marco sai numa gravacao");
            Assert.IsFalse(g.VerGancho(), "segunda vez nao registra de novo");
            Assert.AreEqual(1, gravacoes, "e nao grava de novo");
            Assert.IsFalse(g.GanchoPendente());
            Assert.IsTrue(g.Historia.Ja(GameSession.MarcoGancho));

            var recarregada = new GameSession(LocalSave.FromJson(LocalSave.ToJson(s)), null);
            Assert.IsFalse(recarregada.GanchoPendente(), "recarregar o jogo nao mostra o fim de novo");
        }
    }
}
