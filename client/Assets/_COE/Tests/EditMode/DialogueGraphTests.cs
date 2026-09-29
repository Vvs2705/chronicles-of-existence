using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Validacao do DADO de dialogo (T007): o catalogo nao pode ter beco sem saida, referencia
    /// quebrada nem conversa sem fallback offline. Puro.</summary>
    public class DialogueGraphTests
    {
        [Test]
        public void Catalogo_ETodoValido()
        {
            string[] erros = DialogueCatalog.ValidarTodos();
            Assert.AreEqual(0, erros.Length, string.Join(" | ", erros));
        }

        [Test]
        public void NenhumNo_FicaSemContinuacao()
        {
            // Beco sem saida = no com opcoes em que TODAS dependem de condicao: existe estado de jogo em
            // que o jogador abre a conversa e nao tem nada para clicar.
            foreach (DialogueGraph g in DialogueCatalog.Grafos)
                foreach (DialogueNode n in g.Nos)
                {
                    if (n.Opcoes.Length == 0) continue;   // terminal e saida valida
                    bool temSaida = false;
                    foreach (DialogueOption o in n.Opcoes)
                        if (o.Condicao.Tipo == CondicaoTipo.Sempre) temSaida = true;
                    Assert.IsTrue(temSaida, g.Id + "/" + n.Id + " nao tem opcao incondicional");
                }
        }

        [Test]
        public void TodoGrafo_TemEntradaIncondicional()
        {
            foreach (DialogueGraph g in DialogueCatalog.Grafos)
            {
                bool tem = false;
                foreach (DialogueNode n in g.Nos) if (n.Condicao.Tipo == CondicaoTipo.Sempre) tem = true;
                Assert.IsTrue(tem, g.Id + " nao abre sem condicao: sem fallback offline");
            }
        }

        [Test]
        public void TodoGrafo_EDeUmNpcQueExiste()
        {
            foreach (DialogueGraph g in DialogueCatalog.Grafos)
                Assert.IsNotNull(NpcCatalog.Npc(g.NpcId), g.Id + " aponta para NPC inexistente: " + g.NpcId);
        }

        [Test]
        public void Validar_AcusaBecoSemSaida()
        {
            DialogueGraph ruim = new DialogueGraph("teste_beco", "borin", new[]
            {
                new DialogueNode("entrada", "t", Condicao.Sempre, new[]
                {
                    new DialogueOption("so_se_lembrar", Condicao.Lembra("evento.nunca"), null, null),
                }),
            });
            string[] erros = ruim.Validar();
            Assert.AreEqual(1, erros.Length, string.Join(" | ", erros));
            StringAssert.Contains("beco sem saida", erros[0]);
        }

        [Test]
        public void Validar_AcusaReferenciaQuebrada()
        {
            DialogueGraph ruim = new DialogueGraph("teste_ref", "borin", new[]
            {
                new DialogueNode("entrada", "t", Condicao.Sempre, new[]
                {
                    new DialogueOption("ir", Condicao.Sempre, "no_que_nao_existe", null),
                }),
            });
            string[] erros = ruim.Validar();
            Assert.AreEqual(1, erros.Length, string.Join(" | ", erros));
            StringAssert.Contains("no inexistente", erros[0]);
        }

        [Test]
        public void Validar_AcusaGrafoSemEntradaIncondicional()
        {
            DialogueGraph ruim = new DialogueGraph("teste_sem_fallback", "borin", new[]
            {
                new DialogueNode("so_de_noite", "t", Condicao.Periodo(TimeOfDay.Noite), new DialogueOption[0]),
            });
            string[] erros = ruim.Validar();
            Assert.AreEqual(1, erros.Length, string.Join(" | ", erros));
            StringAssert.Contains("fallback offline", erros[0]);
        }
    }
}
