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

        [Test]
        public void Obrigatorio1_ZerarCarimboNoSave_NaoReabreNascimento()
        {
            BirthChoice nascida = DestinySystem.Confirmar(null, "serena", "agricultores", "Iris").Escolha;
            nascida.confirmedAtUtc = 0; // save editado a mao

            Assert.IsTrue(DestinySystem.EstaConfirmada(nascida));
            BirthResult troca = DestinySystem.Confirmar(nascida, "ruptura", "guardioes", "Iris");
            Assert.AreEqual(BirthError.JaConfirmado, troca.Erro);
            Assert.AreEqual("serena", nascida.destinyId);
        }

        // --- BACKLOG obrigatorio 1 / ADR-0004 invariante 1: destino de nascimento e PERMANENTE (teste negativo) ---

        [Test]
        public void Obrigatorio1_DestinoImutavelAposConfirmacao()
        {
            BirthChoice nascida = Nascida("serena", "agricultores", "Mira");
            long horaOriginal = nascida.confirmedAtUtc;

            // Tenta ir para cada uma das 12 combinacoes, inclusive a propria: nenhuma passa.
            foreach (DestinyDef d in DestinyCatalog.Destinos)
                foreach (OriginDef o in DestinyCatalog.Origens)
                {
                    string onde = "serena/agricultores -> " + d.Id + "/" + o.Id;
                    BirthResult troca = DestinySystem.Confirmar(nascida, d.Id, o.Id, "Outra");
                    Assert.IsFalse(troca.Ok, onde + ": trocar destino depois do nascimento e o exploit que o dossie secao C descartou");
                    Assert.AreEqual(BirthError.JaConfirmado, troca.Erro, onde + ": a falha tem de ser explicita e nomeada");
                    Assert.IsNull(troca.Escolha, onde);
                }

            Assert.AreEqual("serena", nascida.destinyId, "a escolha original nao pode ser alterada");
            Assert.AreEqual("agricultores", nascida.originId);
            Assert.AreEqual("Mira", nascida.characterName);
            Assert.AreEqual(horaOriginal, nascida.confirmedAtUtc);
        }

        [Test]
        public void Obrigatorio1_NenhumMetodoPublicoAlteraEscolhaConfirmada()
        {
            // Pega o setter "obvio" que alguem acrescente depois (Trocar(escolha, id), Resetar(escolha)...):
            // todo metodo publico de Destiny que recebe BirthChoice e chamado com uma escolha confirmada e um
            // id valido DIFERENTE nos parametros de texto; a escolha tem de sair intacta, lance o metodo ou nao.
            // ponytail: so parametro BirthChoice por valor; ref/out e outros modulos ficam fora da varredura.
            int chamados = 0;
            foreach (Type t in new[] { typeof(DestinySystem), typeof(DestinyCatalog) })
                foreach (MethodInfo m in t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                {
                    ParameterInfo[] ps = m.GetParameters();
                    BirthChoice nascida = Nascida("serena", "agricultores", "Mira");
                    long hora = nascida.confirmedAtUtc;
                    object[] args = new object[ps.Length];
                    bool recebeEscolha = false;
                    for (int i = 0; i < ps.Length; i++)
                    {
                        Type pt = ps[i].ParameterType;
                        if (pt == typeof(BirthChoice)) { args[i] = nascida; recebeEscolha = true; }
                        else if (pt == typeof(string)) args[i] = "ruptura";
                        else if (pt.IsValueType) args[i] = Activator.CreateInstance(pt);
                    }
                    if (!recebeEscolha) continue;

                    try { m.Invoke(null, args); }
                    catch (TargetInvocationException) { } // recusar lancando e aceitavel; alterar nao
                    chamados++;

                    string onde = t.Name + "." + m.Name + " alterou a escolha confirmada";
                    Assert.AreEqual("serena", nascida.destinyId, onde);
                    Assert.AreEqual("agricultores", nascida.originId, onde);
                    Assert.AreEqual("Mira", nascida.characterName, onde);
                    Assert.AreEqual(hora, nascida.confirmedAtUtc, onde);
                }
            Assert.Greater(chamados, 0, "a varredura nao achou metodo nenhum: o teste ficou cego");
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

        // --- BACKLOG obrigatorio 2: as 12 combinacoes destino/origem carregam sem falha ---

        [Test]
        public void Obrigatorio2_DozeCombinacoesCarregam()
        {
            // "Carregar" sem depender do codigo de T004: o BirthChoice passa pelo mesmo JsonUtility que o
            // LocalSave usa, volta, e tem de validar, continuar confirmado e compor a circunstancia.
            int ok = 0;
            foreach (DestinyDef d in DestinyCatalog.Destinos)
                foreach (OriginDef o in DestinySystem.OrigensDisponiveis(d.Id))
                {
                    string onde = d.Id + "/" + o.Id;
                    BirthResult r = DestinySystem.Confirmar(null, d.Id, o.Id, "Íris");
                    Assert.IsTrue(r.Ok, onde + " falhou: " + r.Erro);

                    BirthChoice lida = UnityEngine.JsonUtility.FromJson<BirthChoice>(
                        UnityEngine.JsonUtility.ToJson(r.Escolha));
                    Assert.AreEqual(BirthError.Nenhum, DestinySystem.Validar(lida), onde);
                    Assert.IsTrue(DestinySystem.EstaConfirmada(lida), onde + ": confirmacao se perdeu no caminho");
                    Assert.AreEqual("Íris", lida.characterName, onde);

                    Circunstancia c = DestinySystem.CircunstanciaDe(lida);
                    Assert.AreEqual(d.Id, c.DestinyId, onde);
                    Assert.AreEqual(o.Id, c.OriginId, onde);
                    ok++;
                }
            Assert.AreEqual(12, ok, "4 destinos x 3 origens = 12 configuracoes (dossie secao C)");
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
