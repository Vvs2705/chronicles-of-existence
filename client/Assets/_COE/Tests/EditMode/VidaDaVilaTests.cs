using System;
using NUnit.Framework;
using UnityEngine;

namespace COE.Tests
{
    /// <summary>A vila viva, as regras puras: o passeio do morador (circuito com pausa, da passagem a quem esta na frente),
    /// o bicho solto (fica no circulo, foge de quem corre, o cachorro acompanha quem anda devagar), a revoada e o som do
    /// mundo (laco sem estalo, nada estoura, mesmo numero toda vez). Sem cena.</summary>
    public class VidaDaVilaTests
    {
        static Vector3 P(float x, float z) { return new Vector3(x, 0f, z); }

        // ---------------------------------------------------------------- passada dos bichos

        [Test]
        public void Passada_DiagonalJunta_LadosOpostos_EParadoNaoMexe()
        {
            const float t = 0.1f;
            foreach (Especie e in new[] { Especie.Cachorro, Especie.Gato, Especie.Porco })
            {
                Assert.AreEqual(Animal.Passada(0, t, true, e), Animal.Passada(3, t, true, e), 1e-4f, e + ": diagonal 0-3 junta");
                Assert.AreEqual(Animal.Passada(1, t, true, e), Animal.Passada(2, t, true, e), 1e-4f, e + ": diagonal 1-2 junta");
                Assert.AreEqual(-Animal.Passada(0, t, true, e), Animal.Passada(1, t, true, e), 1e-4f, e + ": uma diagonal contra a outra");
                Assert.AreNotEqual(0f, Animal.Passada(0, t, true, e), e + ": andando a perna mexe");
                Assert.AreEqual(0f, Animal.Passada(0, t, false, e), e + ": parado a perna fica reta");
            }
            Assert.AreEqual(-Animal.Passada(0, t, true, Especie.Galinha), Animal.Passada(1, t, true, Especie.Galinha), 1e-4f, "galinha alterna");
            Assert.Less(Animal.Velocidade(Especie.Porco), Animal.Velocidade(Especie.Galinha), "porco e o mais lento");
        }

        // ---------------------------------------------------------------- passo por chao e fumaca

        [Test]
        public void Chao_PedraEMadeiraTemPassoProprio_ORestoEhTerra()
        {
            Assert.AreEqual(Som.PassoPedra, Chao.PassoEm("COE_Auren_Pedra"));
            Assert.AreEqual(Som.PassoMadeira, Chao.PassoEm("COE_Auren_Madeira"));
            Assert.AreEqual(Som.Passo, Chao.PassoEm("COE_Auren_Grama"));
            Assert.AreEqual(Som.Passo, Chao.PassoEm(null));
            float[] terra = Sintese.Efeito(Som.Passo), pedra = Sintese.Efeito(Som.PassoPedra), madeira = Sintese.Efeito(Som.PassoMadeira);
            Assert.AreNotEqual(terra.Length, madeira.Length, "madeira soa mais longa que terra");
            CollectionAssert.AreNotEqual(terra, pedra, "pedra soa diferente de terra");
        }

        [Test]
        public void Fumaca_NasceESomeComTamanhoZero_SobeEOVentoLeva()
        {
            Vector3 d0, dMeio, dFim;
            float e0, eMeio, eFim;
            Fumaca.Baforada(0, 5, 0f, out d0, out e0);
            Fumaca.Baforada(0, 5, Fumaca.Duracao * 0.5f, out dMeio, out eMeio);
            Fumaca.Baforada(0, 5, Fumaca.Duracao * 0.999f, out dFim, out eFim);
            Assert.AreEqual(0f, e0, 1e-4f, "nasce sem tamanho (nao pisca)");
            Assert.Less(eFim, 0.05f, "some antes de reciclar");
            Assert.Greater(eMeio, 1f, "no meio da subida a baforada passa de um metro: aparece de longe");
            Assert.Greater(dMeio.y, d0.y);
            Assert.Greater(dFim.y, dMeio.y, "sobe");
            Assert.Greater(dFim.x, dMeio.x, "o vento leva");
            Vector3 dOutra;
            float eOutra;
            Fumaca.Baforada(1, 5, 0f, out dOutra, out eOutra);
            Assert.Greater(dOutra.y, d0.y, "as baforadas vem defasadas, nao em bloco");
        }

        [Test]
        public void Balanco_FicaDentroDoLimite_MexeEVoltaPeloRepouso()
        {
            float maxX = 0f, maxZ = 0f, minX = 0f;
            for (int i = 0; i < 400; i++)
            {
                Vector3 e = Balanco.Inclinacao(i * 0.05f, 2f, 4f).eulerAngles;
                float x = Mathf.DeltaAngle(0f, e.x), z = Mathf.DeltaAngle(0f, e.z);
                Assert.LessOrEqual(Mathf.Abs(x), 2.01f, "passou do limite em X");
                Assert.LessOrEqual(Mathf.Abs(z), 2.01f, "passou do limite em Z");
                maxX = Mathf.Max(maxX, x); minX = Mathf.Min(minX, x); maxZ = Mathf.Max(maxZ, Mathf.Abs(z));
            }
            Assert.Greater(maxX, 1f, "o vento mexe");
            Assert.Less(minX, -1f, "e volta para o outro lado");
            Assert.Greater(maxZ, 0.2f, "nao vai e volta num trilho so");
            Assert.AreNotEqual(Balanco.Inclinacao(1f, 2f, 4f), Balanco.Inclinacao(1f + 0.7f, 2f, 4f), "a fase separa as pecas");
        }

        // ---------------------------------------------------------------- Ruas

        [Test]
        public void Ruas_Entre_VoltaAteOUltimoPontoComum_ESaiPeloOutroRamo()
        {
            Vector3[] ferraria = { P(-19f, -36f), P(0f, -40f), P(0f, -4f), P(14f, 10f) };
            Vector3[] ervanaria = { P(-19f, -36f), P(0f, -40f), P(0f, -4f), P(-15f, 12f) };
            CollectionAssert.AreEqual(new[] { P(14f, 10f), P(0f, -4f), P(-15f, 12f) }, Ruas.Entre(ferraria, ervanaria),
                                      "ramos irmaos: so ate a praca e de volta");

            Vector3[] casa = { P(-19f, -36f), P(-22f, -43f) };
            Vector3[] horta = { P(-19f, -36f), P(-15.5f, -41f), P(-15.5f, -55f), P(-22f, -55f) };
            CollectionAssert.AreEqual(new[] { P(-22f, -43f), P(-19f, -36f), P(-15.5f, -41f), P(-15.5f, -55f), P(-22f, -55f) },
                                      Ruas.Entre(casa, horta), "o ponto comum (spawn) entra uma vez so");
            CollectionAssert.AreEqual(new[] { P(-22f, -55f), P(-15.5f, -55f), P(-15.5f, -41f), P(-19f, -36f), P(-22f, -43f) },
                                      Ruas.Entre(horta, casa), "a volta e o mesmo caminho ao contrario");
        }

        [Test]
        public void Ruas_Entre_MesmaAncora_UmPontoSo_EUmPercursoDentroDoOutro()
        {
            Vector3[] posto = { P(-19f, -36f), P(0f, -40f), P(0f, 14f), P(0f, 34f), P(7f, 34f) };
            Assert.AreEqual(1, Ruas.Entre(posto, posto).Length, "mesma ancora: nao anda");
            Vector3[] spawn = { P(-19f, -36f) };
            CollectionAssert.AreEqual(posto, Ruas.Entre(spawn, posto), "do spawn: o percurso inteiro");
            Assert.IsEmpty(Ruas.Entre(null, posto));
        }

        // ---------------------------------------------------------------- Passeio

        [Test]
        public void Passeio_VaiEVolta_ComPausaEmCadaPonta_EDaAVoltaInteira()
        {
            Passeio p = Passeio.De(new[] { P(0f, 0f), P(10f, 0f) }, 2f, 1f);
            Assert.AreEqual(2f * (1f + 5f), p.Duracao, 1e-4f, "duas pausas de 1 s e 2 x 10 m a 2 m/s");

            PoseDePasseio parado = p.Em(0.5f);
            Assert.IsFalse(parado.Andando, "comeca pausando no primeiro ponto");
            Assert.AreEqual(0f, parado.Posicao.x, 1e-4f);
            Assert.AreEqual(1f, parado.Direcao.x, 1e-4f, "na pausa ja olha para onde vai");

            PoseDePasseio meio = p.Em(1f + 2.5f);
            Assert.IsTrue(meio.Andando);
            Assert.AreEqual(5f, meio.Posicao.x, 1e-3f, "na metade do trecho");

            PoseDePasseio volta = p.Em(6f + 1f + 1f);
            Assert.AreEqual(8f, volta.Posicao.x, 1e-3f, "na volta, saindo do segundo ponto");
            Assert.AreEqual(-1f, volta.Direcao.x, 1e-4f);

            Assert.AreEqual(p.Em(2f).Posicao.x, p.Em(2f + p.Duracao * 3f).Posicao.x, 1e-3f, "volta inteira repete");
        }

        [Test]
        public void Passeio_RotaVazia_EEmCasa_UmPontoSoFicaParado()
        {
            Assert.IsNull(Passeio.De(new Vector3[0], 1f, 1f));
            Assert.IsNull(Passeio.De(null, 1f, 1f));
            Passeio um = Passeio.De(new[] { P(3f, 4f) }, 1.5f, 2f);
            for (float t = 0f; t < 20f; t += 0.7f)
            {
                PoseDePasseio pose = um.Em(t);
                Assert.IsFalse(pose.Andando);
                Assert.AreEqual(3f, pose.Posicao.x, 1e-4f);
                Assert.Greater(pose.Direcao.sqrMagnitude, 0.5f, "direcao nunca zero (LookRotation)");
            }
        }

        [Test]
        public void Passeio_DaPassagem_SoAQuemEstaPertoENaFrente()
        {
            Vector3 eu = P(0f, 0f), frente = Vector3.forward;
            Assert.IsTrue(Passeio.DaPassagem(eu, frente, P(0f, 1f)), "na frente, perto: para");
            Assert.IsFalse(Passeio.DaPassagem(eu, frente, P(0f, -1f)), "atras: segue");
            Assert.IsFalse(Passeio.DaPassagem(eu, frente, P(0f, Passeio.RaioDePassagem + 0.1f)), "longe: segue");
            Assert.IsTrue(Passeio.DaPassagem(eu, frente, new Vector3(0.3f, 5f, 1f)), "altura nao conta");
        }

        // ---------------------------------------------------------------- Ciscar

        static float NoPlano(Vector3 a, Vector3 b) { a.y = b.y = 0f; return Vector3.Distance(a, b); }

        [Test]
        public void Ciscar_MilPassosLongeDoJogador_FicaNoCirculo_EAlternaOQueFaz()
        {
            Vector3 centro = P(5f, -3f);
            var c = new Ciscar(centro, 3f, 0.6f, 42);
            var vistos = new bool[Enum.GetValues(typeof(EstadoDoBicho)).Length];
            float andou = 0f;
            Vector3 antes = c.Posicao;
            for (int i = 0; i < 1000; i++)
            {
                c.Passo(0.1f, P(100f, 100f), 0f);
                Assert.LessOrEqual(NoPlano(c.Posicao, centro), 3f + 1e-3f, "saiu do circulo no passo " + i);
                vistos[(int)c.Estado] = true;
                andou += NoPlano(c.Posicao, antes);
                antes = c.Posicao;
            }
            Assert.IsTrue(vistos[(int)EstadoDoBicho.Andando] && vistos[(int)EstadoDoBicho.Bicando] && vistos[(int)EstadoDoBicho.Parado],
                          "galinha que nunca anda, nunca bica ou nunca para");
            Assert.IsFalse(vistos[(int)EstadoDoBicho.Fugindo], "fugiu sem ninguem perto");
            Assert.Greater(andou, 5f, "quase nao saiu do lugar em 100 s");
        }

        [Test]
        public void Ciscar_MesmaSemente_MesmoCaminho()
        {
            var a = new Ciscar(P(0f, 0f), 3f, 0.6f, 7);
            var b = new Ciscar(P(0f, 0f), 3f, 0.6f, 7);
            for (int i = 0; i < 300; i++)
            {
                a.Passo(0.1f, P(50f, 0f), 0f);
                b.Passo(0.1f, P(50f, 0f), 0f);
            }
            Assert.AreEqual(a.Posicao, b.Posicao);
        }

        [Test]
        public void Ciscar_CorrerPerto_Assusta_EOBichoSeAfasta_AndarPertoNao()
        {
            var c = new Ciscar(P(0f, 0f), 4f, 0.6f, 3);
            Vector3 jogador = P(-1.5f, 0f);
            c.Passo(0.1f, jogador, MotionSolver.VelocidadeCorridaPadrao);
            Assert.AreEqual(EstadoDoBicho.Fugindo, c.Estado, "correu a 1,5 m");
            float d0 = NoPlano(c.Posicao, jogador);
            for (int i = 0; i < 10; i++) c.Passo(0.1f, jogador, MotionSolver.VelocidadeCorridaPadrao);
            Assert.Greater(NoPlano(c.Posicao, jogador), d0 + 0.5f, "fugiu sem se afastar");

            var calma = new Ciscar(P(0f, 0f), 4f, 0.6f, 3);
            calma.Passo(0.1f, P(-1.5f, 0f), MotionSolver.VelocidadeCaminhadaPadrao);
            Assert.AreNotEqual(EstadoDoBicho.Fugindo, calma.Estado, "andar devagar a 1,5 m nao assusta");
            calma.Passo(0.1f, calma.Posicao + Vector3.right * 0.3f, 0f);
            Assert.AreEqual(EstadoDoBicho.Fugindo, calma.Estado, "encostar assusta mesmo parado");
        }

        [Test]
        public void Ciscar_Encurralado_NaBorda_FogeDeLado_NaoFicaTremendo()
        {
            var c = new Ciscar(P(0f, 0f), 3f, 0.6f, 5);
            c.Por(P(2.9f, 0f));
            Vector3 jogador = P(1.4f, 0f);   // empurra para fora do circulo
            c.Passo(0.1f, jogador, MotionSolver.VelocidadeCorridaPadrao);
            Vector3 antes = c.Posicao;
            for (int i = 0; i < 8; i++) c.Passo(0.1f, jogador, MotionSolver.VelocidadeCorridaPadrao);
            Assert.Greater(NoPlano(c.Posicao, antes), 0.5f, "preso na borda");
            Assert.LessOrEqual(NoPlano(c.Posicao, P(0f, 0f)), 3f + 1e-3f);
        }

        [Test]
        public void Ciscar_CachorroAcompanha_QuemAndaDevagar_SoAteOLimite()
        {
            Vector3 centro = P(0f, 0f);
            var cao = new Ciscar(centro, 5f, 1.3f, 300, true, 7f);
            Vector3 jogador = P(3f, 0f);
            for (int i = 0; i < 60; i++) cao.Passo(0.1f, jogador, 0f);
            Assert.AreEqual(EstadoDoBicho.Seguindo, cao.Estado);
            Assert.AreEqual(Ciscar.DistanciaDeCompanhia, NoPlano(cao.Posicao, jogador), 0.1f, "fica do lado, sem encostar");
            Assert.Greater(Vector3.Dot(cao.Direcao, (jogador - cao.Posicao).normalized), 0.9f, "olha para o jogador");

            for (int i = 0; i < 200; i++) { jogador += Vector3.right * 0.1f; cao.Passo(0.1f, jogador, 1f); }
            Assert.LessOrEqual(NoPlano(cao.Posicao, centro), 7f + 1e-3f, "seguiu alem do limite");
            Assert.AreNotEqual(EstadoDoBicho.Seguindo, cao.Estado, "o jogador foi embora (23 m)");

            var galinha = new Ciscar(centro, 5f, 0.6f, 300);
            for (int i = 0; i < 60; i++) galinha.Passo(0.1f, P(3f, 0f), 0f);
            Assert.AreNotEqual(EstadoDoBicho.Seguindo, galinha.Estado, "galinha nao segue ninguem");
        }

        // ---------------------------------------------------------------- Revoada

        [Test]
        public void Revoada_FicaPertoDoRaio_ESeMexe_EAsaBate()
        {
            for (int i = 0; i < 6; i++)
                for (float t = 0f; t < 30f; t += 0.37f)
                {
                    Vector3 p = Revoada.Posicao(i, 6, t, 10f, 2f, 16f);
                    float r = new Vector2(p.x, p.z).magnitude;
                    Assert.That(r, Is.InRange(10f * 0.8f, 10f * 1.2f), "membro " + i + " fora da roda");
                    Assert.LessOrEqual(Mathf.Abs(p.y), 2f * 0.12f + 1e-4f);
                    Assert.AreEqual(1f, Revoada.Direcao(i, 6, t, 10f, 2f, 16f).magnitude, 1e-3f);
                }
            Assert.Greater(Vector3.Distance(Revoada.Posicao(0, 6, 0f, 10f, 2f, 16f), Revoada.Posicao(0, 6, 2f, 10f, 2f, 16f)), 2f);
            Assert.AreNotEqual(Revoada.Asa(0, 0f, 6f), Revoada.Asa(0, 0.04f, 6f), "asa parada");
            Assert.AreNotEqual(Revoada.Posicao(0, 6, 1f, 10f, 2f, 16f), Revoada.Posicao(1, 6, 1f, 10f, 2f, 16f), "dois passaros no mesmo ponto");
        }

        [Test]
        public void NoPeriodo_LeAMascaraPorBit()
        {
            Assert.IsTrue(Morador.NoPeriodo(3, TimeOfDay.Manha));
            Assert.IsTrue(Morador.NoPeriodo(3, TimeOfDay.Tarde));
            Assert.IsFalse(Morador.NoPeriodo(3, TimeOfDay.Noite));
            Assert.IsTrue(Morador.NoPeriodo(4, TimeOfDay.Noite));
            Assert.IsFalse(Morador.NoPeriodo(0, TimeOfDay.Manha));
        }

        // ---------------------------------------------------------------- som do mundo

        static float Pico(float[] b) { float m = 0f; foreach (float x in b) m = Math.Max(m, Math.Abs(x)); return m; }

        /// <summary>O maior salto entre duas amostras vizinhas DENTRO do buffer. Ruido em banda alta salta por natureza; a
        /// emenda do laco nao pode saltar mais do que o proprio som salta (isso seria o estalo).</summary>
        static float MaiorSalto(float[] b) { float m = 0f; for (int i = 1; i < b.Length; i++) m = Math.Max(m, Math.Abs(b[i] - b[i - 1])); return m; }

        [Test]
        public void Ambiente_DiaENoite_DuramAVolta_EmendamSemEstalo_SaoDiferentes()
        {
            foreach (bool noite in new[] { false, true })
            {
                float[] a = Sintese.Ambiente(noite);
                Assert.AreEqual((int)(Sintese.SegundosDoAmbiente * Sintese.Taxa), a.Length);
                Assert.That(Pico(a), Is.InRange(0.3f, 0.71f), (noite ? "noite" : "dia") + " mudo ou estourando");
                Assert.LessOrEqual(Math.Abs(a[0] - a[a.Length - 1]), MaiorSalto(a), "o laco estala na emenda");
                Assert.Less(Math.Abs(a[0] - a[a.Length - 1]), 0.1f, "na emenda so ha vento: tem de ser suave");
                CollectionAssert.AreEqual(a, Sintese.Ambiente(noite), "muda entre chamadas");
            }
            CollectionAssert.AreNotEqual(Sintese.Ambiente(false), Sintese.Ambiente(true), "dia e noite soam igual");
        }

        [Test]
        public void TodoSomDoMundo_TemSom_NaoEstoura_EEDeterministico_OSussurroEmenda()
        {
            foreach (SomDoMundo s in Enum.GetValues(typeof(SomDoMundo)))
            {
                float[] e = Sintese.DoMundo(s);
                Assert.Greater(e.Length, Sintese.Taxa / 10, s + " curto demais");
                Assert.That(Pico(e), Is.InRange(0.3f, 1f), s + " mudo ou estourando");
                foreach (float x in e) Assert.IsFalse(float.IsNaN(x) || float.IsInfinity(x), s + " com NaN");
                CollectionAssert.AreEqual(e, Sintese.DoMundo(s), s + " muda entre chamadas");
            }
            float[] sus = Sintese.DoMundo(SomDoMundo.Sussurro);
            Assert.AreEqual((int)(Sintese.SegundosDoSussurro * Sintese.Taxa), sus.Length);
            Assert.LessOrEqual(Math.Abs(sus[0] - sus[sus.Length - 1]), MaiorSalto(sus), "o sussurro estala na emenda do laco");
        }
    }
}
