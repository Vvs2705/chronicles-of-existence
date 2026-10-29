using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace COE.Tests
{
    /// <summary>Tutorial guiado do episodio 1 (GuiaRegras) e o juice que o acompanha: ordem e condicao dos passos, cada um
    /// uma vez, Pular e Rever pelas Configuracoes (chave publicada coe.cfg.v1.guia), texto em todo passo, e as regras puras
    /// de vibracao, parada do acerto, tranco da camera, pulo do cartao e o botao de qualidade do menu.</summary>
    public class GuiaTests
    {
        static GuiaEstado Vila()
        {
            return new GuiaEstado { NoControle = true, Toque = true, Idade = 5 };
        }

        static string Proximo(GuiaEstado e, HashSet<string> vistos)
        {
            GuiaPasso p = GuiaRegras.Proximo(e, vistos.Contains);
            return p != null ? p.Id : null;
        }

        [Test]
        public void PrimeirosMinutos_DespertarAndarOlharMissaoRumoUsar_NaOrdem()
        {
            var vistos = new HashSet<string>();
            GuiaEstado e = Vila();
            e.CartaoMissao = true;
            e.Seta = true;
            e.AlvoDeUso = true;
            var ordem = new List<string>();
            for (string id = Proximo(e, vistos); id != null; id = Proximo(e, vistos)) { ordem.Add(id); vistos.Add(id); }
            CollectionAssert.AreEqual(new[] { GuiaRegras.Despertar, GuiaRegras.Mover, GuiaRegras.Olhar, GuiaRegras.Missao,
                                              GuiaRegras.Rumo, GuiaRegras.Usar }, ordem,
                "cartao de contexto (C1/C2) antes de tudo; o menu e os itens esperam a primeira missao concluida");
        }

        [Test]
        public void SemControle_NadaAparece()
        {
            GuiaEstado e = Vila();
            e.NoControle = false;
            Assert.IsNull(Proximo(e, new HashSet<string>()), "conversa, menu ou salto abertos: o guia espera");
        }

        [Test]
        public void PassoSemCondicao_Espera_ENaoTrancaOsOutros()
        {
            var vistos = new HashSet<string> { GuiaRegras.Despertar };
            GuiaEstado e = Vila();
            e.Toque = false;          // PC sem toque: andar, olhar e usar nao aparecem
            e.CartaoMissao = true;
            Assert.AreEqual(GuiaRegras.Missao, Proximo(e, vistos));
            vistos.Add(GuiaRegras.Missao);
            e.Concluidas = 1;
            e.TemItem = true;
            Assert.AreEqual(GuiaRegras.Menu, Proximo(e, vistos));
            vistos.Add(GuiaRegras.Menu);
            Assert.AreEqual(GuiaRegras.Itens, Proximo(e, vistos));
        }

        [Test]
        public void Diferenciais_PromessaAVilaLembra_EGolpesSoAosOito()
        {
            var vistos = new HashSet<string>();
            foreach (GuiaPasso p in GuiaRegras.Passos) vistos.Add(p.Id);
            vistos.Remove(GuiaRegras.Promessa);
            vistos.Remove(GuiaRegras.Golpes);
            GuiaEstado e = Vila();
            Assert.IsNull(Proximo(e, vistos), "sem a q04 e aos 5: nem promessa nem golpes");
            e.Promessa = true;
            Assert.AreEqual(GuiaRegras.Promessa, Proximo(e, vistos), "C3: a q04 comeca e o cartao diz que a vila lembra");
            vistos.Add(GuiaRegras.Promessa);
            e.Idade = 8;
            e.Golpes = true;
            Assert.AreEqual(GuiaRegras.Golpes, Proximo(e, vistos), "C4: no treino, aprender fazendo");
            vistos.Clear();
            Assert.AreNotEqual(GuiaRegras.Despertar, Proximo(e, vistos), "o cartao do despertar e so da primeira infancia");
        }

        [Test]
        public void Configuracoes_CadaPassoUmaVez_PularERever()
        {
            var mem = new ConfigEmMemoria();
            var c = new Configuracoes(mem, false);
            Assert.IsFalse(c.GuiaVisto(GuiaRegras.Mover));
            c.MarcarGuia(GuiaRegras.Mover);
            c.MarcarGuia(GuiaRegras.Despertar);
            c.MarcarGuia(GuiaRegras.Mover);
            Assert.AreEqual("despertar,mover", mem.Valores[Configuracoes.ChaveGuia], "ordem do catalogo, sem repetir");
            Assert.AreEqual("coe.cfg.v1.guia", Configuracoes.ChaveGuia, "chave publicada");
            Assert.IsTrue(new Configuracoes(mem, false).GuiaVisto(GuiaRegras.Mover), "volta do armazenamento");

            c.PularGuia();
            var lida = new Configuracoes(mem, false);
            foreach (GuiaPasso p in GuiaRegras.Passos) Assert.IsTrue(lida.GuiaVisto(p.Id), "Pular: " + p.Id);
            Assert.IsNull(GuiaRegras.Proximo(Vila(), lida.GuiaVisto));

            lida.ReverGuia();
            Assert.AreEqual("", mem.Valores[Configuracoes.ChaveGuia]);
            Assert.AreEqual(GuiaRegras.Despertar, GuiaRegras.Proximo(Vila(), new Configuracoes(mem, false).GuiaVisto).Id, "Rever: do comeco");
        }

        [Test]
        public void ListaGravada_IdDesconhecidoFica_ELixoNaoQuebra()
        {
            HashSet<string> l = GuiaRegras.Ler(" mover, ,passo_futuro,,despertar ");
            CollectionAssert.AreEquivalent(new[] { "mover", "passo_futuro", "despertar" }, l);
            Assert.AreEqual("despertar,mover,passo_futuro", GuiaRegras.Gravar(l));
            Assert.AreEqual(0, GuiaRegras.Ler(null).Count);
        }

        [Test]
        public void Vibracao_LigadaPorPadrao_ComChavePublicada()
        {
            var mem = new ConfigEmMemoria();
            var c = new Configuracoes(mem, false);
            Assert.IsTrue(c.Vibrar);
            c.DefinirVibrar(false);
            Assert.AreEqual("0", mem.Valores["coe.cfg.v1.vibracao"]);
            Assert.IsFalse(new Configuracoes(mem, false).Vibrar);
        }

        [Test]
        public void TodoPasso_TemTexto_EmPtBR()
        {
            Assert.IsTrue(StringsLoader.Load(StringsLoader.DefaultLanguage), "arquivo de textos");
            try
            {
                var ids = new HashSet<string>();
                foreach (GuiaPasso p in GuiaRegras.Passos)
                {
                    Assert.IsTrue(ids.Add(p.Id), "id repetido: " + p.Id);
                    StringAssert.IsMatch("^[a-z0-9_]+$", p.Id, "id estavel em snake_case");
                    foreach (string k in p.Chaves) Assert.AreNotEqual(k, Strings.Get(k), "sem texto: " + k);
                }
                string despertar = Strings.Format(GuiaRegras.Passo(GuiaRegras.Despertar).TextoKey, "Íris", "O ofício da casa é plantar.");
                StringAssert.Contains("Íris", despertar);
                StringAssert.Contains("O ofício da casa", despertar, "C1: a origem escolhida volta no primeiro cartao");
                foreach (string k in new[] { "guia.pular", "guia.entendi", "guia.agora", "guia.rotulo.oque", "guia.rotulo.quando",
                                             "guia.rotulo.porque", "titulo.promessa" })
                    Assert.AreNotEqual(k, Strings.Get(k), "sem texto: " + k);
            }
            finally { Strings.Load(null); }
        }

        [Test]
        public void Tremor_PulsoColadoViraUm_EDesligadoNaoVibra()
        {
            Assert.IsTrue(Tremor.Pode(true, 10f, float.NegativeInfinity, Tremor.AcertoMs));
            Assert.IsFalse(Tremor.Pode(true, 10.02f, 10f, Tremor.AcertoMs), "combo rapido: um pulso so");
            Assert.IsTrue(Tremor.Pode(true, 10f + Tremor.IntervaloMinimo, 10f, Tremor.AcertoMs));
            Assert.IsFalse(Tremor.Pode(false, 10f, float.NegativeInfinity, Tremor.AcertoMs), "menu: vibracao desligada");
            Assert.IsFalse(Tremor.Pode(true, 10f, float.NegativeInfinity, 0));
            Assert.Greater(Tremor.DanoMs, Tremor.AcertoMs, "levar golpe pesa mais que dar");
        }

        [Test]
        public void Tranco_CaiAteZero_EParadaNaoMexeNoTempoDoEditor()
        {
            Assert.AreEqual(Tranco.Dano, Tranco.Amplitude(Tranco.Dano, 0f, Tranco.DuracaoTranco), 1e-6f);
            float meio = Tranco.Amplitude(Tranco.Dano, Tranco.DuracaoTranco * 0.5f, Tranco.DuracaoTranco);
            Assert.That(meio, Is.GreaterThan(0f).And.LessThan(Tranco.Dano * 0.5f), "cai ao quadrado");
            Assert.AreEqual(0f, Tranco.Amplitude(Tranco.Dano, Tranco.DuracaoTranco, Tranco.DuracaoTranco));
            Assert.AreEqual(0f, Tranco.Amplitude(Tranco.Dano, -1f, Tranco.DuracaoTranco));

            float antes = Time.timeScale;
            var parada = new ParadaDeImpacto();
            Assert.IsFalse(parada.Parar(0f), "fora do Play o tempo do editor nao muda");
            Assert.AreEqual(antes, Time.timeScale);
        }

        [Test]
        public void PuloDoCartao_SobeEVoltaA1()
        {
            Assert.AreEqual(1f, MissaoHud.Pulo(0f), 1e-6f);
            Assert.Greater(MissaoHud.Pulo(0.1f), 1f);
            Assert.LessOrEqual(MissaoHud.Pulo(0.1f), 1.12f);
            Assert.AreEqual(1f, MissaoHud.Pulo(0.3f));
            Assert.AreEqual(1f, MissaoHud.Pulo(99f), "cena aberta sem missao concluida: tamanho normal");
        }

        [Test]
        public void BotaoDeQualidade_AndaPelasFaixasEVoltaAoAutomatico()
        {
            var vistos = new List<string>();
            FaixaQualidade? q = null;
            for (int i = 0; i < 4; i++) { q = MenuDePausa.ProximaQualidade(q); vistos.Add(q.HasValue ? q.Value.ToString() : "auto"); }
            CollectionAssert.AreEqual(new[] { "Baixa", "Media", "Alta", "auto" }, vistos);
        }
    }
}
