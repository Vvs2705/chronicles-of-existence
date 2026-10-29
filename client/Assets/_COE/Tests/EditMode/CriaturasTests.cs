using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Criaturas do bosque e encontros do ep. 2 (Combat/Criaturas.cs): ritmo deterministico, regras de cada
    /// criatura (EPISODIO2 §3.4), um de cada vez, derrota recomeca do zero e o encontro aponta para um gatilho que existe.</summary>
    public class CriaturasTests
    {
        static EncontroState Encontro(string id) { return new EncontroState(Criaturas.Encontro(id)); }

        /// <summary>Anda em passos pequenos ate a criatura chegar na fase (ou estoura em 30 s de jogo).</summary>
        static void Ate(EncontroState e, CriaturaBrain b, CriaturaFase fase)
        {
            for (int i = 0; i < 3000 && b.Fase != fase; i++) e.Tick(0.01f);
            Assert.AreEqual(fase, b.Fase, b.Def.Id + " nunca chegou em " + fase);
        }

        [Test]
        public void TodoEncontro_CumpreUmGatilhoDeAncoraQueExiste_ComCriaturasConhecidas()
        {
            foreach (EncontroDef e in Criaturas.Encontros)
            {
                Assert.AreEqual(e.Ancora, MissaoMundo.AncoraDo(e.QuestId, e.ObjetivoId),
                                e.Id + ": o objetivo precisa ser gatilho de ancora na mesma ancora");
                Assert.Greater(e.Criaturas.Length, 0, e.Id);
                foreach (string c in e.Criaturas) Assert.IsNotNull(Criaturas.Def(c), e.Id + ": " + c);
                Assert.AreSame(e, Criaturas.DoObjetivo(e.QuestId, e.ObjetivoId));
            }
        }

        [Test]
        public void Ritmo_EDeterministico_EUmDeCadaVez()
        {
            EncontroState a = Encontro("enc.trilha_abafadores_borda"), b = Encontro("enc.trilha_abafadores_borda");
            int botesA = 0;
            for (int i = 0; i < 1000; i++)
            {
                botesA += a.Tick(0.01f).Length;
                b.Tick(0.01f);
                int atacando = 0;
                for (int k = 0; k < a.Bando.Length; k++)
                {
                    Assert.AreEqual(a.Bando[k].Fase, b.Bando[k].Fase, "mesmo dt, mesma luta");
                    if (a.Bando[k].Fase == CriaturaFase.Aviso || a.Bando[k].Fase == CriaturaFase.Bote) atacando++;
                }
                Assert.LessOrEqual(atacando, 1, "o bando ataca um de cada vez");
            }
            Assert.Greater(botesA, 1, "em 10 s o bando ja atacou mais de uma vez");
        }

        [Test]
        public void Abafadores_UltimoFogeSozinho_EOEncontroEVencido()
        {
            EncontroState e = Encontro("enc.trilha_abafadores_borda");
            for (int i = 0; i < e.Bando.Length - 1; i++)
                Assert.Greater(e.Golpear(e.Bando[i], 999f), 0f, "abafador apanha a qualquer hora");
            Assert.IsTrue(e.Bando[e.Bando.Length - 1].Fora, "quando sobra um, ele foge");
            Assert.IsTrue(e.Vencido);
            Assert.AreEqual(0, e.Tick(1f).Length, "vencido nao ataca mais");
        }

        [Test]
        public void Feixe_FechadoEMata_SoApanhaNaJanela()
        {
            EncontroState e = Encontro("enc.espinhal_feixes");
            CriaturaBrain f = e.Bando[0];
            Assert.AreEqual(0f, e.Golpear(f, 5f), "parado, e mata");
            Ate(e, f, CriaturaFase.Janela);
            Assert.AreEqual(5f, e.Golpear(f, 5f), "depois do bote, aberto");
        }

        [Test]
        public void Lajeiro_DeFrenteSegura_PeloFlancoEntra_EForteAtordoaComForca()
        {
            EncontroState e = Encontro("enc.lajedo_lajeiro");
            CriaturaBrain l = e.Bando[0];
            Assert.AreEqual(0f, e.Golpear(l, 5f, pelaFrente: true), "as lajes da frente seguram");
            Assert.AreEqual(5f, e.Golpear(l, 5f, pelaFrente: false));
            e.Golpear(l, 1f, forte: true, forca: 1);
            Assert.AreNotEqual(CriaturaFase.Atordoada, l.Fase, "peso 3 pede Forca 2");
            e.Golpear(l, 1f, forte: true, forca: 2);
            Assert.AreEqual(CriaturaFase.Atordoada, l.Fase);
            Assert.AreEqual(3f, e.Golpear(l, 3f, pelaFrente: true), "atordoado, a frente tambem abre");
            e.Tick(1.6f);
            Assert.AreNotEqual(CriaturaFase.Atordoada, l.Fase, "o atordoamento passa");
        }

        [Test]
        public void Enredado_NaoPerdeVida_TresNosNaJanelaLibertam()
        {
            EncontroState e = Encontro("enc.coracao_enredado");
            CriaturaBrain enredado = e.Enredado;
            Assert.IsNotNull(enredado);
            Assert.AreEqual(0f, e.Golpear(enredado, 999f), "golpe nao fere o Enredado");
            Assert.IsFalse(e.Cortar(NoDoEnredado.Frente), "fechado, o no nao se corta");

            Ate(e, enredado, CriaturaFase.Janela);
            Assert.IsTrue(e.Cortar(NoDoEnredado.Frente));
            Assert.IsFalse(e.Cortar(NoDoEnredado.Frente), "no cortado nao conta duas vezes");
            Assert.IsTrue(e.Cortar(NoDoEnredado.Flanco));
            Assert.IsFalse(e.Vencido);
            Assert.IsTrue(e.Cortar(NoDoEnredado.Cima));

            Assert.IsTrue(e.Vencido);
            foreach (CriaturaBrain b in e.Bando) Assert.IsTrue(b.Fora, b.Def.Id + ": libertado, o bando dispersa");
        }

        [Test]
        public void Derrota_RecomecaDoInicio_SemPerderNada()
        {
            EncontroState e = Encontro("enc.coracao_enredado");
            Ate(e, e.Enredado, CriaturaFase.Janela);
            e.Cortar(NoDoEnredado.Frente);
            e.Golpear(e.Bando[1], 5f);

            e.Derrota();

            Assert.AreEqual(1, e.Derrotas);
            Assert.AreEqual(0, e.NosCortados);
            foreach (CriaturaBrain b in e.Bando)
            {
                Assert.AreEqual(CriaturaFase.Espreita, b.Fase);
                Assert.AreEqual(b.Def.Vida, b.Vida);
            }
        }

        [Test]
        public void AvisoLongo_SoEstica_NuncaEncurta()
        {
            var normal = new CriaturaBrain(Criaturas.Def(Criaturas.Feixe));
            var longo = new CriaturaBrain(Criaturas.Def(Criaturas.Feixe));
            normal.Atacar(); longo.Atacar();
            float aviso = Criaturas.Def(Criaturas.Feixe).Aviso;
            Assert.IsTrue(normal.Tick(aviso + 0.01f), "o bote sai no fim do aviso");
            Assert.IsFalse(longo.Tick(aviso + 0.01f, 1.5f), "assist.aviso_longo: 50% a mais");
            Assert.IsFalse(new CriaturaBrain(Criaturas.Def(Criaturas.Feixe)).Tick(aviso + 0.01f, 0.1f), "sem Atacar fica na espreita");
        }
    }
}
