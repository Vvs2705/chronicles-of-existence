using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Salto temporal por marco narrativo (T009). Puro: nao monta cena, nao toca disco.
    /// O centro deste arquivo e o EXPLOIT N. 5 do backlog -- "salto temporal exige confirmacao e nao
    /// duplica" -- nos testes Obrigatorio5_*: sem confirmacao nao salta (so anunciar, confirmar sem preparo
    /// ou com preparo forjado), e confirmado de novo nao duplica (clique duplo, preparo velho depois de
    /// recarregar). Recarregar do disco e interromper antes do Commit estao em ProgressionSaveRoundTripTests.</summary>
    public class AgeAdvanceTests
    {
        const string Marco = AgeAdvanceCatalog.SaltoInfancia;

        static SaveData Crianca()
        {
            SaveData s = new SaveData();
            Assert.AreEqual(5, s.ageYears, "montagem do teste: a vida comeca aos 5");
            new LifeEventHistory(s).Registrar(AgeAdvanceCatalog.LiberadoPor, LifeEventCategoria.Marco, 5); // Q-08 feita
            return s;
        }

        [Test]
        public void SaltoSemAQ08Concluida_NaoLibera()
        {
            SaveData s = new SaveData();   // save novo: nenhuma missao feita
            LifeEventHistory h = new LifeEventHistory(s);

            Assert.IsFalse(AgeAdvance.PodeAvancarIdade(s, Marco, h));
            SaltoPreparado p = AgeAdvance.PrepararSalto(s, Marco, h, Abertas());
            Assert.AreEqual(SaltoBloqueio.NaoLiberado, p.Motivo);
            Assert.IsFalse(AgeAdvance.ConfirmarSalto(s, p, h).Aplicado);
            Assert.AreEqual(5, s.ageYears, "o salto nao pula a campanha");
        }

        [Test]
        public void RequisitoDoSalto_EOEventoDaFlagDaQ08()
        {
            Assert.AreEqual(QuestCatalog.EventoDaFlag("salto_temporal_liberado"), AgeAdvanceCatalog.LiberadoPor);
        }

        static string[] Abertas() { return new[] { "q03_cesto_perdido", "q05_animal_ferido" }; }

        // --- caminho feliz ---

        [Test]
        public void PodeAvancarIdade_AosCinco_PeloMarcoDoSlice()
        {
            SaveData s = Crianca();
            Assert.IsTrue(AgeAdvance.PodeAvancarIdade(s, Marco, new LifeEventHistory(s)));
        }

        [Test]
        public void PrepararSalto_AnunciaIdadeFaseEOQueSeraEncerrado()
        {
            SaveData s = Crianca();
            SaltoPreparado p = AgeAdvance.PrepararSalto(s, Marco, new LifeEventHistory(s), Abertas());

            Assert.IsTrue(p.Possivel);
            Assert.AreEqual(SaltoBloqueio.Nenhum, p.Motivo);
            Assert.AreEqual(5, p.IdadeAtual);
            Assert.AreEqual(8, p.IdadeDepois, "dossie L: salto de 5 para cerca de 8");
            Assert.AreEqual(LifePhase.PrimeirasDescobertas, p.FaseAtual);
            Assert.AreEqual(LifePhase.DespertarDosTalentos, p.FaseDepois);
            Assert.IsTrue(p.MudaDeFase);
            CollectionAssert.AreEqual(Abertas(), p.OportunidadesEncerradas,
                "a interface precisa listar o que o salto encerra ANTES de perguntar");
        }

        [Test]
        public void Obrigatorio5_SoAnunciar_NaoEnvelhece()
        {
            SaveData s = Crianca();
            AgeAdvance.PrepararSalto(s, Marco, new LifeEventHistory(s), Abertas());

            Assert.AreEqual(5, s.ageYears, "anunciar nao e confirmar");
            Assert.AreEqual(1, s.lifeLevel);
            Assert.AreEqual(1, new LifeEventHistory(s).Total, "so a liberacao da Q-08; anunciar nao registra nada");
        }

        [Test]
        public void ConfirmarSalto_Envelhece_ESobeUmNivelDeVida()
        {
            SaveData s = Crianca();
            LifeEventHistory h = new LifeEventHistory(s);
            SaltoResultado r = AgeAdvance.ConfirmarSalto(s, AgeAdvance.PrepararSalto(s, Marco, h, Abertas()), h);

            Assert.IsTrue(r.Aplicado);
            Assert.AreEqual(5, r.IdadeAntes);
            Assert.AreEqual(8, r.IdadeDepois);
            Assert.AreEqual(8, s.ageYears);
            Assert.AreEqual(2, s.lifeLevel, "um marco atravessado = um Nivel de Vida, somado e nunca multiplicado");
            Assert.AreEqual(TimeOfDayCycle.IdManha, s.life.timeOfDay, "depois do salto, amanhece");
            CollectionAssert.AreEqual(Abertas(), r.OportunidadesEncerradas,
                "quem fecha missao e a T006; a T009 so devolve a lista");
        }

        [Test]
        public void ConfirmarSalto_ZeraAAncora_SoQuandoAplica()
        {
            SaveData s = Crianca();
            s.sceneId = "auren";
            s.anchorId = "bosque_clareira";   // B13: a confirmacao acontece na clareira
            LifeEventHistory h = new LifeEventHistory(s);
            SaltoPreparado p = AgeAdvance.PrepararSalto(s, Marco, h, null);
            Assert.AreEqual("bosque_clareira", s.anchorId, "anunciar nao move ninguem");

            Assert.IsTrue(AgeAdvance.ConfirmarSalto(s, p, h).Aplicado);
            Assert.AreEqual("", s.anchorId, "B14: tres anos depois acorda em spawn_player, na mesma operacao do salto");
            Assert.AreEqual("auren", s.sceneId, "o salto nao troca de cena");

            s.anchorId = "ferraria";
            Assert.IsFalse(AgeAdvance.ConfirmarSalto(s, p, h).Aplicado);
            Assert.AreEqual("ferraria", s.anchorId, "salto recusado nao altera nada, nem a ancora");
        }

        [Test]
        public void Salto_NaoConcedeAtributoNemAfinidade()
        {
            SaveData s = Crianca();
            LifeEventHistory h = new LifeEventHistory(s);
            AgeAdvance.ConfirmarSalto(s, AgeAdvance.PrepararSalto(s, Marco, h, null), h);

            // slice secao 4.2: "o salto nao concede atributo, item, grau nem ascensao" -- salto que da poder e farmavel.
            foreach (string id in Mastery.Atributos) Assert.AreEqual(1, Mastery.Valor(s, id), "salto mexeu em " + id);
            foreach (string id in Mastery.Afinidades) Assert.AreEqual(0, Mastery.Valor(s, id), "salto mexeu em " + id);
        }

        // --- TESTE NEGATIVO 1: clique duplo / gatilho repetido ---

        [Test]
        public void Obrigatorio5_ConfirmarDuasVezes_SoContaUma()
        {
            SaveData s = Crianca();
            LifeEventHistory h = new LifeEventHistory(s);
            SaltoPreparado p = AgeAdvance.PrepararSalto(s, Marco, h, Abertas());

            SaltoResultado primeiro = AgeAdvance.ConfirmarSalto(s, p, h);
            SaltoResultado segundo = AgeAdvance.ConfirmarSalto(s, p, h);   // o mesmo preparo, de novo

            Assert.IsTrue(primeiro.Aplicado);
            Assert.IsFalse(segundo.Aplicado, "exploit n. 5: confirmar duas vezes nao pode envelhecer duas vezes");
            Assert.AreEqual(SaltoBloqueio.JaAplicado, segundo.Motivo);
            Assert.AreEqual(8, s.ageYears, "a idade parou em 8, nao foi para 11");
            Assert.AreEqual(2, s.lifeLevel, "o Nivel de Vida tambem nao dobrou");
            Assert.AreEqual(2, h.Contar(LifeEventCategoria.Marco), "liberacao da Q-08 + o marco do salto registrado UMA vez");
        }

        // --- TESTE NEGATIVO 2: preparo velho sobrevivendo a um reload ---

        [Test]
        public void Obrigatorio5_PreparoVelhoDepoisDeRecarregar_NaoDuplica()
        {
            SaveData s = Crianca();
            LifeEventHistory h = new LifeEventHistory(s);
            SaltoPreparado p = AgeAdvance.PrepararSalto(s, Marco, h, Abertas());
            AgeAdvance.ConfirmarSalto(s, p, h);

            // Recarregar = reabrir o historico em cima do MESMO SaveData, como o LocalSave faz no Load.
            LifeEventHistory depoisDoLoad = new LifeEventHistory(s);

            Assert.IsFalse(AgeAdvance.PodeAvancarIdade(s, Marco, depoisDoLoad));
            SaltoResultado r = AgeAdvance.ConfirmarSalto(s, p, depoisDoLoad);

            Assert.IsFalse(r.Aplicado);
            Assert.AreEqual(SaltoBloqueio.JaAplicado, r.Motivo);
            Assert.AreEqual(8, s.ageYears);
            Assert.AreEqual(2, s.lifeLevel);
        }

        [Test]
        public void PrepararSalto_DepoisDeAplicado_NaoAnunciaPerdaNenhuma()
        {
            SaveData s = Crianca();
            LifeEventHistory h = new LifeEventHistory(s);
            AgeAdvance.ConfirmarSalto(s, AgeAdvance.PrepararSalto(s, Marco, h, Abertas()), h);

            SaltoPreparado p2 = AgeAdvance.PrepararSalto(s, Marco, h, Abertas());
            Assert.IsFalse(p2.Possivel);
            Assert.AreEqual(SaltoBloqueio.JaAplicado, p2.Motivo);
            Assert.AreEqual(0, p2.OportunidadesEncerradas.Length,
                "nao assustar o jogador com uma perda que nao vai acontecer");
        }

        // --- TESTE NEGATIVO 3: nada alem do marco envelhece ---

        [Test]
        public void IdadeNaoSobe_ComDiaEPraticaRepetidos()
        {
            SaveData s = Crianca();
            AtividadeDef cesto = new AtividadeDef("carregar_cesto", "forca", 1);

            for (int i = 0; i < 500; i++)
            {
                Mastery.Praticar(s, cesto);
                TimeOfDayCycle.Avancar(s.life);
            }

            Assert.AreEqual(5, s.ageYears,
                "dossie D: a idade nao acelera ao caminhar nem ao repetir tarefa");
            Assert.AreEqual(1, s.lifeLevel);
            Assert.AreEqual(1, new LifeEventHistory(s).Contar(LifeEventCategoria.Marco), "so a liberacao da Q-08; o salto nao aconteceu");
        }

        // --- recusas ---

        [Test]
        public void Obrigatorio5_ConfirmarSemPreparo_NaoEnvelhece()
        {
            SaveData s = Crianca();
            SaltoResultado r = AgeAdvance.ConfirmarSalto(s, null, new LifeEventHistory(s));

            Assert.IsFalse(r.Aplicado);
            Assert.AreEqual(SaltoBloqueio.PreparoInvalido, r.Motivo);
            Assert.AreEqual(5, s.ageYears, "sem anuncio nao ha confirmacao, e sem confirmacao nao ha salto");
        }

        [Test]
        public void Obrigatorio5_ConfirmarComPreparoForjado_NaoEnvelhece()
        {
            SaveData s = Crianca();
            SaltoPreparado forjado = new SaltoPreparado();
            forjado.MarcoId = Marco;
            forjado.Possivel = false;

            Assert.IsFalse(AgeAdvance.ConfirmarSalto(s, forjado, new LifeEventHistory(s)).Aplicado);
            Assert.AreEqual(5, s.ageYears);
        }

        [Test]
        public void MarcoDesconhecido_RecusaSemLancar()
        {
            SaveData s = Crianca();
            LifeEventHistory h = new LifeEventHistory(s);

            Assert.IsFalse(AgeAdvance.PodeAvancarIdade(s, "marco_que_nao_existe", h));
            Assert.IsFalse(AgeAdvance.PodeAvancarIdade(s, null, h));
            Assert.AreEqual(SaltoBloqueio.MarcoDesconhecido,
                AgeAdvance.PrepararSalto(s, "marco_que_nao_existe", h, Abertas()).Motivo);
        }

        [Test]
        public void MarcoQueNaoAvanca_ERecusado()
        {
            SaveData s = Crianca();
            s.ageYears = 12;   // ja passou da idade alvo do salto da infancia
            Assert.AreEqual(SaltoBloqueio.NaoAvanca,
                AgeAdvance.PrepararSalto(s, Marco, new LifeEventHistory(s), null).Motivo);
        }

        [Test]
        public void IdadeAbaixoDoMinimo_ERecusada()
        {
            SaveData s = Crianca();
            s.ageYears = 4;
            Assert.AreEqual(SaltoBloqueio.IdadeInsuficiente,
                AgeAdvance.PrepararSalto(s, Marco, new LifeEventHistory(s), null).Motivo);
        }

        [Test]
        public void PrepararSalto_SemListaAberta_NaoLanca()
        {
            SaveData s = Crianca();
            SaltoPreparado p = AgeAdvance.PrepararSalto(s, Marco, new LifeEventHistory(s), null);
            Assert.IsTrue(p.Possivel);
            Assert.IsNotNull(p.OportunidadesEncerradas);
            Assert.AreEqual(0, p.OportunidadesEncerradas.Length);
        }

        [Test]
        public void PrepararSalto_CopiaAListaDoChamador()
        {
            SaveData s = Crianca();
            List<string> abertas = new List<string> { "q03_cesto_perdido" };
            SaltoPreparado p = AgeAdvance.PrepararSalto(s, Marco, new LifeEventHistory(s), abertas);

            abertas.Clear();
            Assert.AreEqual(1, p.OportunidadesEncerradas.Length, "o aviso nao pode mudar por baixo da interface");
        }

        // --- a forma da API e parte da regra ---

        [Test]
        public void ApiSoTemOsTresMetodos()
        {
            List<string> nomes = new List<string>();
            foreach (MethodInfo m in typeof(AgeAdvance).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
                nomes.Add(m.Name);
            nomes.Sort();

            CollectionAssert.AreEqual(new[] { "ConfirmarSalto", "PodeAvancarIdade", "PrepararSalto" }, nomes,
                "nenhum atalho de uma chamada so: envelhecer exige anunciar e depois confirmar (dossie D)");
        }
    }
}
