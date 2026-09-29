using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Progressao por atividade significativa e teto anti-farm (T009). Puro.
    /// O teste obrigatorio n. 7 do backlog -- "acao trivial repetida nao gera dominio ilimitado" -- sao os
    /// Obrigatorio7_*: a mesma atividade satura, 500 repeticoes nao viram dominio, a acao trivial rende uma vez
    /// so, e nem 40 atividades distintas furam o teto da trilha na etapa. O teto sobreviver a recarregar esta
    /// em ProgressionSaveRoundTripTests.</summary>
    public class ProgressionMasteryTests
    {
        // Desafio 5 mantem a atividade acima do personagem durante todos os testes: nada fica trivial por acaso.
        static AtividadeDef Desafiadora(string id, string trilha) { return new AtividadeDef(id, trilha, 5); }

        static int Repetir(SaveData s, AtividadeDef a, int vezes)
        {
            int ultimoGanho = 0;
            for (int i = 0; i < vezes; i++) ultimoGanho = Mastery.Praticar(s, a).ProgressoGanho;
            return ultimoGanho;
        }

        // --- contrato de ids ---

        [Test]
        public void Os12IdsDoContratoResolvem()
        {
            SaveData s = new SaveData();
            foreach (string id in Mastery.Atributos)
            {
                Assert.IsTrue(Mastery.TrilhaExiste(id), "atributo do contrato 1 sem campo no save: " + id);
                Assert.AreEqual(1, Mastery.Valor(s, id), "atributo comeca em 1: " + id);
            }
            foreach (string id in Mastery.Afinidades)
            {
                Assert.IsTrue(Mastery.TrilhaExiste(id), "afinidade do contrato 1 sem campo no save: " + id);
                Assert.AreEqual(0, Mastery.Valor(s, id), "afinidade comeca em 0: " + id);
            }
        }

        [Test]
        public void TrilhaDesconhecida_ERecusada_SemAlterarNada()
        {
            SaveData s = new SaveData();
            GanhoResultado r = Mastery.Praticar(s, new AtividadeDef("treino", "carisma", 3));

            Assert.IsFalse(r.Aceito, "id fora do contrato nao inventa trilha nova");
            Assert.AreEqual(0, s.life.pratica.Count);
        }

        // --- descobrir != aprender != dominar ---

        [Test]
        public void PrimeiraVez_EDescoberta()
        {
            SaveData s = new SaveData();
            GanhoResultado r = Mastery.Praticar(s, Desafiadora("colher_ervas", "natural"));

            Assert.IsTrue(r.Aceito);
            Assert.AreEqual(MarcoDeDominio.Descoberta, r.Marco);
            Assert.AreEqual(Mastery.GanhoBase, r.ProgressoGanho);
            Assert.IsFalse(r.Trivial);
        }

        [Test]
        public void SegundaVez_EAprendizado_ERendeMenos()
        {
            SaveData s = new SaveData();
            AtividadeDef a = Desafiadora("colher_ervas", "natural");

            int primeiro = Mastery.Praticar(s, a).ProgressoGanho;
            GanhoResultado segundo = Mastery.Praticar(s, a);

            Assert.AreEqual(MarcoDeDominio.Aprendizado, segundo.Marco);
            Assert.Less(segundo.ProgressoGanho, primeiro, "retorno decrescente: a segunda vez ensina menos");
            Assert.Greater(segundo.ProgressoGanho, 0);
        }

        [Test]
        public void Dominio_SaiUmaVezSO_AoBaterOTetoDaEtapa()
        {
            SaveData s = new SaveData();
            AtividadeDef a = Desafiadora("colher_ervas", "natural");

            int dominios = 0;
            for (int i = 0; i < 40; i++)
                if (Mastery.Praticar(s, a).Marco == MarcoDeDominio.Dominio) dominios++;

            Assert.AreEqual(1, dominios,
                "Dominio e transicao, nao estado: repetir depois de saturado nao pode reemitir o marco");
            Assert.AreEqual(Mastery.TetoAtividadePorFase, Mastery.ProgressoTotal(s, "natural"));
        }

        // --- TESTE OBRIGATORIO 7 do backlog ---

        [Test]
        public void Obrigatorio7_RepetirAMesmaAtividade_Satura_MasAtividadeNovaAindaRende()
        {
            SaveData s = new SaveData();
            AtividadeDef bater = Desafiadora("bater_no_poste", "marcial");

            Assert.AreEqual(0, Repetir(s, bater, 20), "a mesma atividade para de render dentro da etapa");
            Assert.AreEqual(Mastery.TetoAtividadePorFase, Mastery.ProgressoTotal(s, "marcial"));

            GanhoResultado nova = Mastery.Praticar(s, Desafiadora("duelo_com_sera", "marcial"));
            Assert.AreEqual(Mastery.GanhoBase, nova.ProgressoGanho,
                "atividade NOVA na mesma trilha continua rendendo cheio: o teto pune repetir, nao jogar");
            Assert.AreEqual(MarcoDeDominio.Descoberta, nova.Marco);
        }

        [Test]
        public void Obrigatorio7_QuinhentasRepeticoes_NaoGeramDominioIlimitado()
        {
            SaveData s = new SaveData();
            Repetir(s, Desafiadora("bater_no_poste", "marcial"), 500);

            Assert.AreEqual(Mastery.TetoAtividadePorFase, Mastery.ProgressoTotal(s, "marcial"));
            Assert.AreEqual(0, Mastery.Valor(s, "marcial"),
                "30 de progresso nao chega nos 60 de um ponto: 500 repeticoes = zero afinidade");
            Assert.AreEqual(1, s.life.pratica.Count, "uma linha de ledger por (atividade, fase), nao 500");
        }

        [Test]
        public void Obrigatorio7_AcaoTrivialRepetida_RendeUmaVezSo_ENuncaViraDominio()
        {
            SaveData s = new SaveData();
            s.attributes.forca = 4;
            AtividadeDef cesto = new AtividadeDef("carregar_cesto", "forca", 1);   // desafio 1 < forca 4

            GanhoResultado primeira = Mastery.Praticar(s, cesto);
            Assert.IsTrue(primeira.Trivial);
            Assert.AreEqual(Mastery.TetoTrivialPorFase, primeira.ProgressoGanho);
            Assert.IsTrue(primeira.Saturada, "trivial satura na primeira execucao");

            for (int i = 0; i < 100; i++)
            {
                GanhoResultado r = Mastery.Praticar(s, cesto);
                Assert.AreEqual(0, r.ProgressoGanho);
                Assert.AreNotEqual(MarcoDeDominio.Dominio, r.Marco, "repeticao trivial nunca e dominio");
            }
            Assert.AreEqual(4, s.attributes.forca);
        }

        [Test]
        public void MesmaTarefa_ViraTrivialQuandoOPersonagemCresce()
        {
            SaveData s = new SaveData();
            AtividadeDef tarefa = new AtividadeDef("varrer_a_forja", "forca", 2);

            Assert.IsFalse(Mastery.Praticar(s, tarefa).Trivial, "desafio 2 com forca 1 ainda ensina");

            s.attributes.forca = 3;   // GDD cap. 03: a evolucao exige tarefas progressivamente mais desafiadoras
            Assert.IsTrue(Mastery.Praticar(s, tarefa).Trivial);
        }

        // --- teto por etapa, e a etapa seguinte reabrindo ---

        [Test]
        public void Obrigatorio7_TetoDaTrilhaPorEtapa_SeguraAteComMuitasAtividadesDistintas()
        {
            SaveData s = new SaveData();
            for (int i = 0; i < 40; i++) Repetir(s, Desafiadora("treino_" + i, "forca"), 10);

            Assert.AreEqual(Mastery.TetoTrilhaPorFase, Mastery.ProgressoTotal(s, "forca"),
                "40 atividades distintas nao furam o teto da etapa");
            Assert.AreEqual(1 + Mastery.TetoTrilhaPorFase / Mastery.ProgressoPorPonto, s.attributes.forca);
        }

        [Test]
        public void MudarDeEtapa_ReabreOTeto()
        {
            SaveData s = new SaveData();
            AtividadeDef a = Desafiadora("treino_de_espada", "marcial");
            Assert.AreEqual(0, Repetir(s, a, 20));

            s.ageYears = 8;   // a etapa mudou (GDD cap. 03: 8-11 Despertar dos Talentos)
            GanhoResultado r = Mastery.Praticar(s, a);

            Assert.AreEqual(Mastery.GanhoBase, r.ProgressoGanho, "crescer devolve espaco de aprendizado");
            Assert.AreEqual(2, s.life.pratica.Count, "linha nova por etapa; a da infancia continua no historico");
            Assert.AreEqual(Mastery.TetoAtividadePorFase + Mastery.GanhoBase, Mastery.ProgressoTotal(s, "marcial"));
        }

        [Test]
        public void PontoSoNasceAoCruzarOLimiar_EORestoNaoSome()
        {
            SaveData s = new SaveData();
            Repetir(s, Desafiadora("treino_a", "vigor"), 10);   // 30 de progresso
            Assert.AreEqual(1, s.attributes.vigor, "30 ainda nao e um ponto");

            Repetir(s, Desafiadora("treino_b", "vigor"), 10);   // +30 = 60
            Assert.AreEqual(2, s.attributes.vigor, "o resto da primeira atividade contou para o ponto");
        }

        [Test]
        public void CadaUmaDas12Trilhas_SobeSoOProprioCampo()
        {
            SaveData zero = new SaveData();
            foreach (string trilha in Trilhas())
            {
                SaveData s = new SaveData();
                Repetir(s, Desafiadora("treino_a", trilha), 10);   // 30
                Repetir(s, Desafiadora("treino_b", trilha), 10);   // +30 = 60 = um ponto

                // A escrita e por reflexao (Mastery.CampoDe): um id trocado subiria o campo errado em silencio.
                foreach (string outra in Trilhas())
                    Assert.AreEqual(Mastery.Valor(zero, outra) + (outra == trilha ? 1 : 0), Mastery.Valor(s, outra),
                        "praticar '" + trilha + "' deixou '" + outra + "' com o valor errado");
            }
        }

        static IEnumerable<string> Trilhas()
        {
            foreach (string id in Mastery.Atributos) yield return id;
            foreach (string id in Mastery.Afinidades) yield return id;
        }

        // --- Nivel de Vida nao multiplica poder ---

        [Test]
        public void NivelDeVidaAlto_NaoRendeUmPingoAMais()
        {
            SaveData baixo = new SaveData();
            SaveData alto = new SaveData();
            alto.lifeLevel = 99;

            AtividadeDef a = Desafiadora("treino_de_espada", "marcial");
            for (int i = 0; i < 10; i++)
            {
                GanhoResultado rb = Mastery.Praticar(baixo, a);
                GanhoResultado ra = Mastery.Praticar(alto, a);
                Assert.AreEqual(rb.ProgressoGanho, ra.ProgressoGanho,
                    "Nivel de Vida e marcador historico, nao multiplicador (dossie D)");
            }
            Assert.AreEqual(Mastery.ProgressoTotal(baixo, "marcial"), Mastery.ProgressoTotal(alto, "marcial"));
        }

        [Test]
        public void LifeLevel_NaoExpoeFatorNenhum()
        {
            foreach (MethodInfo m in typeof(LifeLevel).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                Assert.AreNotEqual(typeof(float), m.ReturnType, "LifeLevel." + m.Name + " devolve fator: viraria multiplicador");
                Assert.AreNotEqual(typeof(double), m.ReturnType, "LifeLevel." + m.Name + " devolve fator: viraria multiplicador");
            }
        }

        [Test]
        public void AposMarco_SomaUm_Sempre()
        {
            Assert.AreEqual(2, LifeLevel.AposMarco(1));
            Assert.AreEqual(3, LifeLevel.AposMarco(2));
            Assert.AreEqual(100, LifeLevel.AposMarco(99), "linear ate no fim: nada de curva por nivel");
            Assert.AreEqual(2, LifeLevel.AposMarco(0), "save adulterado com nivel 0 volta para o minimo");
        }

        // --- fronteiras ---

        [Test]
        public void Praticar_NaoEnvelhece_ENaoMexeNoNascimento()
        {
            SaveData s = new SaveData();
            s.birth.destinyId = "serena";
            s.birth.originId = "agricultores";
            s.birth.confirmedAtUtc = 123L;

            Repetir(s, Desafiadora("treino_de_espada", "marcial"), 50);

            Assert.AreEqual(5, s.ageYears);
            Assert.AreEqual(1, s.lifeLevel);
            Assert.AreEqual("serena", s.birth.destinyId, "progressao nao toca em destino (ADR-0004)");
            Assert.AreEqual(123L, s.birth.confirmedAtUtc);
        }

        [Test]
        public void Praticar_ComEntradaInvalida_NaoLanca()
        {
            Assert.IsFalse(Mastery.Praticar(null, Desafiadora("x", "forca")).Aceito);
            Assert.IsFalse(Mastery.Praticar(new SaveData(), null).Aceito);
            Assert.IsFalse(Mastery.Praticar(new SaveData(), new AtividadeDef("", "forca", 1)).Aceito);
        }
    }
}
