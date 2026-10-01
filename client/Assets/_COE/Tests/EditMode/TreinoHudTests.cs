using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>B15 / SLICE §5 R7: o painel de progresso do treino mostra o ganho ESTACIONANDO depois de N repeticoes do
    /// mesmo golpe e diz ao jogador por que. Texto montado em C# puro (TreinoHud.Texto) a partir do Mastery.</summary>
    public class TreinoHudTests
    {
        [SetUp] public void Carregar() { Assert.IsTrue(StringsLoader.Load(StringsLoader.DefaultLanguage)); }
        [TearDown] public void Limpar() { Strings.Load(null); }

        [Test]
        public void R7_CinquentaVezesOMesmoGolpe_OProgressoParaNoTeto_EOTextoDizPorQue()
        {
            var s = new SaveData { ageYears = 8 };
            AtividadeDef leve = TrainingProgress.AtividadeLeve;
            string primeiro = TreinoHud.Texto(leve, Mastery.Praticar(s, leve));
            StringAssert.Contains("12/" + Mastery.TetoAtividadePorFase, primeiro, "a primeira pratica mostra quanto rendeu contra o teto");

            string ultimo = null;
            GanhoResultado r = default(GanhoResultado);
            for (int i = 1; i < 50; i++) { r = Mastery.Praticar(s, leve); ultimo = TreinoHud.Texto(leve, r); }
            Assert.AreEqual(0, r.ProgressoGanho, "depois do teto, repetir nao rende");
            Assert.LessOrEqual(r.ProgressoDaAtividade, r.Teto, "o progresso nunca passa do teto");
            Assert.AreEqual(Strings.Format("treino.saturado", Strings.Get("treino.atividade." + leve.Id)), ultimo,
                "parado no teto, o texto diz por que e o que fazer");
            StringAssert.DoesNotContain("[", ultimo, "chave sem texto no arquivo");

            string outro = TreinoHud.Texto(TrainingProgress.AtividadeForte, Mastery.Praticar(s, TrainingProgress.AtividadeForte));
            StringAssert.Contains("12/", outro, "golpe novo ainda rende: o teto e por atividade");
        }

        [Test]
        public void TodoVerboDoTreino_TemNomeEscrito_ETextoInvalidoNaoAparece()
        {
            foreach (AtividadeDef a in TrainingProgress.Atividades)
                Assert.AreNotEqual("[treino.atividade." + a.Id + "]", Strings.Get("treino.atividade." + a.Id), a.Id);
            Assert.IsNull(TreinoHud.Texto(TrainingProgress.AtividadeLeve, default(GanhoResultado)), "pratica recusada nao vira texto");
        }
    }
}
