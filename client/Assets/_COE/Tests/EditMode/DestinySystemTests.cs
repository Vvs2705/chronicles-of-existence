using System;
using System.Reflection;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Regra de nascimento (T003). Puro: nao monta cena, nao le save.
    /// Os quatro invariantes do ADR-0004 que sao de COMPORTAMENTO moram aqui, com o teste negativo da
    /// troca de destino no centro.</summary>
    public class DestinySystemTests
    {
        static BirthChoice Nascida(string destino = "serena", string origem = "agricultores", string nome = "Mira")
        {
            BirthResult r = DestinySystem.Confirmar(null, destino, origem, nome);
            Assert.IsTrue(r.Ok, "montagem do teste falhou: " + r.Erro);
            return r.Escolha;
        }

        // --- origens compativeis ---

        [Test]
        public void OrigensDisponiveis_DaAsTresOrigens_NosQuatroDestinos()
        {
            foreach (DestinyDef d in DestinyCatalog.Destinos)
            {
                OriginDef[] origens = DestinySystem.OrigensDisponiveis(d.Id);
                Assert.AreEqual(3, origens.Length, d.Id + " tem de oferecer tres origens (dossie secao C)");
            }
        }

        [Test]
        public void OrigensDisponiveis_DestinoDesconhecido_DevolveVazioSemLancar()
        {
            Assert.AreEqual(0, DestinySystem.OrigensDisponiveis("facil").Length);
            Assert.AreEqual(0, DestinySystem.OrigensDisponiveis(null).Length);
        }

        [Test]
        public void OrigensDisponiveis_DevolveCopia()
        {
            OriginDef[] a = DestinySystem.OrigensDisponiveis("serena");
            a[0] = null;
            Assert.IsNotNull(DestinySystem.OrigensDisponiveis("serena")[0]);
        }

        // --- confirmacao ---

        [Test]
        public void Confirmar_PrimeiraVez_GravaEscolhaEMarcaHorario()
        {
            long antes = DateTime.UtcNow.Ticks;
            BirthResult r = DestinySystem.Confirmar(null, "ruptura", "guardioes", "Sera");

            Assert.IsTrue(r.Ok);
            Assert.AreEqual(BirthError.Nenhum, r.Erro);
            Assert.AreEqual("ruptura", r.Escolha.destinyId);
            Assert.AreEqual("guardioes", r.Escolha.originId);
            Assert.AreEqual("Sera", r.Escolha.characterName);
            Assert.GreaterOrEqual(r.Escolha.confirmedAtUtc, antes, "confirmedAtUtc tem de vir preenchido");
            Assert.IsTrue(DestinySystem.EstaConfirmada(r.Escolha));
        }

        [Test]
        public void EscolhaNova_NaoNasceConfirmada()
        {
            Assert.IsFalse(DestinySystem.EstaConfirmada(new BirthChoice()));
            Assert.IsFalse(DestinySystem.EstaConfirmada(null));
        }

        // --- ADR-0004 invariante 1: destino de nascimento e PERMANENTE (teste negativo) ---

        [Test]
        public void Confirmar_DepoisDeConfirmado_NaoTrocaDestinoNemOrigem()
        {
            BirthChoice nascida = Nascida("serena", "agricultores", "Mira");
            long horaOriginal = nascida.confirmedAtUtc;

            BirthResult troca = DestinySystem.Confirmar(nascida, "ruptura", "guardioes", "Mira");

            Assert.IsFalse(troca.Ok, "trocar destino depois do nascimento e o exploit que o dossie secao C descartou");
            Assert.AreEqual(BirthError.JaConfirmado, troca.Erro, "a falha tem de ser explicita e nomeada");
            Assert.IsNull(troca.Escolha);
            Assert.AreEqual("serena", nascida.destinyId, "a escolha original nao pode ser alterada");
            Assert.AreEqual("agricultores", nascida.originId);
            Assert.AreEqual(horaOriginal, nascida.confirmedAtUtc);
        }

        [Test]
        public void Confirmar_DepoisDeConfirmado_FalhaAteComOsMesmosIds()
        {
            BirthChoice nascida = Nascida();
            BirthResult r = DestinySystem.Confirmar(nascida, nascida.destinyId, nascida.originId, nascida.characterName);
            Assert.IsFalse(r.Ok, "nao existe reconfirmacao: qualquer segunda passagem falha");
            Assert.AreEqual(BirthError.JaConfirmado, r.Erro);
        }

        [Test]
        public void Confirmar_DepoisDeConfirmado_ChecaAPermanenciaAntesDeQualquerOutraCoisa()
        {
            // Id invalido + nome invalido: mesmo assim o erro tem de ser JaConfirmado, senao daria para
            // sondar a ordem das validacoes para achar um caminho que escreva por cima.
            BirthChoice nascida = Nascida();
            BirthResult r = DestinySystem.Confirmar(nascida, "inexistente", "inexistente", "");
            Assert.AreEqual(BirthError.JaConfirmado, r.Erro);
        }

        [Test]
        public void Confirmar_IdDesconhecido_FalhaTipado()
        {
            Assert.AreEqual(BirthError.DestinoDesconhecido,
                DestinySystem.Confirmar(null, "facil", "agricultores", "Mira").Erro);
            Assert.AreEqual(BirthError.OrigemDesconhecida,
                DestinySystem.Confirmar(null, "serena", "nobres", "Mira").Erro);
            Assert.IsNull(DestinySystem.Confirmar(null, "facil", "agricultores", "Mira").Escolha);
        }

        [Test]
        public void DozeCombinacoes_ConfirmamEDaoCircunstancia()
        {
            int ok = 0;
            foreach (DestinyDef d in DestinyCatalog.Destinos)
                foreach (OriginDef o in DestinySystem.OrigensDisponiveis(d.Id))
                {
                    BirthResult r = DestinySystem.Confirmar(null, d.Id, o.Id, "Mira");
                    Assert.IsTrue(r.Ok, d.Id + "/" + o.Id + " falhou: " + r.Erro);
                    Circunstancia c = DestinySystem.CircunstanciaDe(r.Escolha);
                    Assert.AreEqual(d.Id, c.DestinyId);
                    Assert.AreEqual(o.Id, c.OriginId);
                    ok++;
                }
            Assert.AreEqual(12, ok);
        }

        // --- nome do avatar ---

        [TestCase("Mira")]
        [TestCase("Ana Clara")]
        [TestCase("D'Auren")]
        [TestCase("Mara-Lys")]
        [TestCase("Jos\u00e9")]          // acento composto: passa
        [TestCase("Jose\u0301")]         // acento combinante: NFC junta e passa
        public void Nome_Valido_Confirma(string nome)
        {
            BirthResult r = DestinySystem.Confirmar(null, "normal", "artesaos", nome);
            Assert.IsTrue(r.Ok, nome + " devia ser aceito, veio " + r.Erro);
        }

        [Test]
        public void Nome_ComAcentoCombinante_ESalvoNormalizado()
        {
            BirthResult r = DestinySystem.Confirmar(null, "normal", "artesaos", "Jose\u0301");
            Assert.AreEqual("Jos\u00e9", r.Escolha.characterName, "o save guarda NFC, uma letra so");
        }

        [Test]
        public void Nome_ESalvoSemEspacoNasPontas()
        {
            BirthResult r = DestinySystem.Confirmar(null, "normal", "artesaos", "   Mira  ");
            Assert.AreEqual("Mira", r.Escolha.characterName);
        }

        [TestCase(null, BirthError.NomeCurto)]
        [TestCase("", BirthError.NomeCurto)]
        [TestCase("   ", BirthError.NomeCurto)]
        [TestCase("M", BirthError.NomeCurto)]
        [TestCase("Mirabelarandaluzitanadeauren", BirthError.NomeLongo)]
        [TestCase("Ana  Clara", BirthError.NomeCaractereInvalido)]   // espaco duplo
        [TestCase("Ana\tClara", BirthError.NomeCaractereInvalido)]   // controle
        [TestCase("Ana\u0007", BirthError.NomeCaractereInvalido)]    // bell
        [TestCase("Ana\uD83D\uDE00", BirthError.NomeCaractereInvalido)] // emoji
        [TestCase("Ana123", BirthError.NomeCaractereInvalido)]
        [TestCase("-Ana", BirthError.NomeCaractereInvalido)]
        [TestCase("Ana-", BirthError.NomeCaractereInvalido)]
        [TestCase("Ana<b>", BirthError.NomeCaractereInvalido)]
        public void Nome_Invalido_NaoConfirma(string nome, BirthError esperado)
        {
            BirthResult r = DestinySystem.Confirmar(null, "normal", "artesaos", nome);
            Assert.IsFalse(r.Ok);
            Assert.AreEqual(esperado, r.Erro);
            Assert.IsNull(r.Escolha, "nome invalido nao pode gerar escolha confirmada");
        }

        // --- save vindo de fora ---

        [Test]
        public void Validar_AceitaEscolhaCoerenteERecusaSaveEditado()
        {
            BirthChoice nascida = Nascida();
            Assert.AreEqual(BirthError.Nenhum, DestinySystem.Validar(nascida));

            nascida.destinyId = "modo_facil"; // simula save.json editado a mao
            Assert.AreEqual(BirthError.DestinoDesconhecido, DestinySystem.Validar(nascida));
            Assert.AreEqual(BirthError.DestinoDesconhecido, DestinySystem.Validar(null));
        }

        // --- CONTRATO secao 2: o DTO e exatamente este ---

        [Test]
        public void BirthChoice_TemExatamenteOsQuatroCamposDoContrato()
        {
            FieldInfo[] campos = typeof(BirthChoice).GetFields(BindingFlags.Public | BindingFlags.Instance);
            Assert.AreEqual(4, campos.Length, "campo a mais/a menos quebra o save de T004 (contrato secao 2)");
            Assert.AreEqual(typeof(string), typeof(BirthChoice).GetField("destinyId").FieldType);
            Assert.AreEqual(typeof(string), typeof(BirthChoice).GetField("originId").FieldType);
            Assert.AreEqual(typeof(string), typeof(BirthChoice).GetField("characterName").FieldType);
            Assert.AreEqual(typeof(long), typeof(BirthChoice).GetField("confirmedAtUtc").FieldType);
            Assert.AreEqual(0, typeof(BirthChoice).GetProperties(BindingFlags.Public | BindingFlags.Instance).Length,
                "propriedade nao serializa em JsonUtility: o contrato secao 2 so tem campos");
        }

        // --- ADR-0004: assistencia/dificuldade nao existe neste sistema ---

        [Test]
        public void ApiDeNascimento_NaoAceitaDificuldadeNemAssistencia()
        {
            string[] proibidos = { "dificuldade", "difficulty", "assist", "multiplicador", "multiplier" };
            foreach (MethodInfo m in typeof(DestinySystem).GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                Proibido(m.Name, "DestinySystem." + m.Name, proibidos);
                foreach (ParameterInfo p in m.GetParameters())
                    Proibido(p.Name, "DestinySystem." + m.Name + "(" + p.Name + ")", proibidos);
            }
        }

        static void Proibido(string nome, string onde, string[] proibidos)
        {
            string s = nome.ToLowerInvariant();
            foreach (string p in proibidos)
                Assert.IsFalse(s.Contains(p),
                    onde + ": assistencia de combate e outro sistema (ADR-0004); se chegou aqui, o desenho errou");
        }
    }
}
