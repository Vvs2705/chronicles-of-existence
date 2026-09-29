using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>T012: idade -> dimensoes do corpo (Corpo, fonte unica do gerador, da camera e do BodyByAge) e quando a
    /// tela do salto aparece e o que o aviso B12 diz (SaltoHud.Disponivel / SaltoHud.Aviso). Regra pura, sem cena.</summary>
    public class IdadeTests
    {
        [Test]
        public void Corpo_CincoAnos_E_OitoAnos_PelaBodyScale()
        {
            Assert.AreEqual(BodyScale.Crianca5, Corpo.DaIdade(5).Altura, 1e-5f);
            Assert.AreEqual(BodyScale.Crianca8, Corpo.DaIdade(8).Altura, 1e-5f);
            Assert.AreEqual(BodyScale.Crianca8, Corpo.DaIdade(9).Altura, 1e-5f, "depois do salto nao volta a 1,10 m");
        }

        [Test]
        public void Corpo_ProporcoesDeCrianca_EmQualquerIdade()
        {
            foreach (int idade in new[] { 5, 8 })
            {
                Corpo c = Corpo.DaIdade(idade);
                Assert.AreEqual(c.Altura * 0.5f, c.CentroY, 1e-5f, idade + ": base da capsula fora dos pes");
                Assert.Less(c.Degrau, c.Altura * 0.25f, idade + ": degrau de adulto, a crianca subiria em caixote");
                Assert.Less(c.PivoCamera, c.Altura, idade + ": pivo da camera acima da cabeca");
                Assert.Greater(c.PivoCamera, c.Altura * 0.5f, idade + ": pivo abaixo da cintura");
                Assert.LessOrEqual(c.DistanciaCamera, c.Altura * 3f, idade + ": camera longe demais, a crianca some");
                Assert.Greater(c.AlturaDoGolpe, c.Altura * 0.4f, idade + ": golpe abaixo da cintura");
                Assert.Less(c.AlturaDoGolpe, c.Altura * 0.7f, idade + ": golpe acima do peito");
            }
            Assert.Greater(Corpo.DaIdade(8).DistanciaCamera, Corpo.DaIdade(5).DistanciaCamera, "aos 8 a camera recua junto");
        }

        [Test]
        public void TelaDoSalto_SoAparece_ComAQ08_EAntesDoSalto()
        {
            SaveData s = new SaveData();
            GameSession g = new GameSession(s, null);
            Assert.IsFalse(SaltoHud.Disponivel(g), "save novo: sem Q-08 nao ha salto a oferecer");
            Assert.IsFalse(SaltoHud.Disponivel(null));

            g.Historia.Registrar(AgeAdvanceCatalog.LiberadoPor, LifeEventCategoria.Marco, 5);
            Assert.IsTrue(SaltoHud.Disponivel(g), "Q-08 concluida aos 5 anos: o botao aparece");

            Assert.IsTrue(g.ConfirmarSalto(g.PrepararSalto()).Aplicado);
            Assert.IsFalse(SaltoHud.Disponivel(g), "salto aplicado: nunca oferece de novo (R5)");
        }

        [Test]
        public void Aviso_NomeiaAsOpcionaisPeloTitulo_EDizOQueSobrevive()
        {
            SaveData s = new SaveData();
            GameSession g = new GameSession(s, null);
            g.Historia.Registrar(AgeAdvanceCatalog.LiberadoPor, LifeEventCategoria.Marco, 5);
            SaltoPreparado p = g.PrepararSalto();
            Assert.IsNotEmpty(p.OportunidadesEncerradas, "o slice tem opcionais abertas num save novo");

            string aviso = SaltoHud.Aviso(p);

            StringAssert.Contains(Strings.Get("salto.encerra"), aviso);
            foreach (string id in p.OportunidadesEncerradas)
            {
                StringAssert.Contains(Strings.Get(QuestCatalog.Missao(id).TituloKey), aviso, id + " fora do aviso");
                StringAssert.DoesNotContain(id, aviso, "B12: pelo titulo canonico, nunca pelo id");
            }
            // Chave ausente = "[salto.fase]" sem os argumentos (Strings.Format): compara com o mesmo Format.
            StringAssert.Contains(Strings.Format("salto.idade", 5, 8), aviso);
            StringAssert.Contains(Strings.Format("salto.fase", Strings.Get("fase.descobertas"), Strings.Get("fase.talentos")),
                                  aviso, "a fase muda: descobertas -> talentos");
            StringAssert.Contains(Strings.Get("salto.sobrevive"), aviso, "§4.1: sem isto o jogador teme perder o save");
        }

        [Test]
        public void Aviso_SemOpcionaisAbertas_NaoMostraASecao()
        {
            var p = new SaltoPreparado { Possivel = true, IdadeAtual = 5, IdadeDepois = 8, MudaDeFase = true,
                                         FaseAtual = LifePhase.PrimeirasDescobertas, FaseDepois = LifePhase.DespertarDosTalentos };

            string aviso = SaltoHud.Aviso(p);

            StringAssert.DoesNotContain(Strings.Get("salto.encerra"), aviso, "B12: nada de lista vazia com moldura");
            StringAssert.Contains(Strings.Get("salto.vinculos"), aviso);
        }
    }
}
