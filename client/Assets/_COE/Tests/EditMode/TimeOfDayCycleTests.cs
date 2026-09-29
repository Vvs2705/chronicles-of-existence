using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Ciclo do dia cotidiano (T009). Puro: nao monta cena, nao le save de disco, nao usa relogio.</summary>
    public class TimeOfDayCycleTests
    {
        [Test]
        public void Avancar_ManhaTardeNoite_EDaAVolta()
        {
            LifeState life = new LifeState();
            Assert.AreEqual(TimeOfDay.Manha, TimeOfDayCycle.Atual(life), "o dia comeca de manha");

            TimeOfDayCycle.Avancar(life);
            Assert.AreEqual(TimeOfDay.Tarde, TimeOfDayCycle.Atual(life));

            TimeOfDayCycle.Avancar(life);
            Assert.AreEqual(TimeOfDay.Noite, TimeOfDayCycle.Atual(life));

            TimeOfDayCycle.Avancar(life);
            Assert.AreEqual(TimeOfDay.Manha, TimeOfDayCycle.Atual(life), "depois da noite vem a manha de novo");
        }

        [Test]
        public void Avancar_ViraODiaUmaVezPorCiclo()
        {
            LifeState life = new LifeState();
            Assert.AreEqual(1, life.day);

            Assert.IsFalse(TimeOfDayCycle.Avancar(life), "manha->tarde nao vira o dia");
            Assert.IsFalse(TimeOfDayCycle.Avancar(life), "tarde->noite nao vira o dia");
            Assert.IsTrue(TimeOfDayCycle.Avancar(life), "noite->manha vira o dia");
            Assert.AreEqual(2, life.day);

            for (int i = 0; i < 30; i++) TimeOfDayCycle.Avancar(life);
            Assert.AreEqual(12, life.day, "30 periodos = 10 dias a mais");
            Assert.AreEqual(TimeOfDay.Manha, TimeOfDayCycle.Atual(life));
        }

        [Test]
        public void Avancar_NaoEnvelhece()
        {
            SaveData s = new SaveData();
            for (int i = 0; i < 300; i++) TimeOfDayCycle.Avancar(s.life);

            Assert.AreEqual(5, s.ageYears, "passar o dia NAO envelhece: idade so sobe em marco (dossie D)");
            Assert.AreEqual(1, s.lifeLevel);
        }

        [Test]
        public void Id_EDe_SaoOMesmoIdEmIdaEVolta()
        {
            foreach (TimeOfDay p in new[] { TimeOfDay.Manha, TimeOfDay.Tarde, TimeOfDay.Noite })
                Assert.AreEqual(p, TimeOfDayCycle.De(TimeOfDayCycle.Id(p)), "id estavel de " + p);
        }

        [Test]
        public void De_IdDesconhecidoOuNulo_CaiEmManha_SemLancar()
        {
            Assert.AreEqual(TimeOfDay.Manha, TimeOfDayCycle.De(null));
            Assert.AreEqual(TimeOfDay.Manha, TimeOfDayCycle.De(""));
            Assert.AreEqual(TimeOfDay.Manha, TimeOfDayCycle.De("madrugada"), "save adulterado nao trava o jogo");
        }

        [Test]
        public void Avancar_ComLifeNulo_NaoLanca()
        {
            Assert.IsFalse(TimeOfDayCycle.Avancar(null));
            Assert.AreEqual(TimeOfDay.Manha, TimeOfDayCycle.Atual(null));
        }
    }
}
