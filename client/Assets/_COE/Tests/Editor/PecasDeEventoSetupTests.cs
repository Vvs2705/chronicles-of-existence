using System;
using System.Collections.Generic;
using COE.EditorTools;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace COE.EditorTests
{
    /// <summary>A vila reage: PecasDeEventoSetup.Montar poe as pecas ligadas por evento perto das ancoras, com os eventos
    /// das fichas (ELENCO.md Arbitragem 2.1; borin C9), sem colisor e com nomes unicos. Mini-cena por teste: so a raiz
    /// "Ancoras" nas posicoes de projeto, sem depender do AurenSceneBuilder chamar o setup.</summary>
    public class PecasDeEventoSetupTests
    {
        static readonly string[] Antes = { "tira_crianca_vao", "bancada_borin_fundo" };

        Transform raiz;

        [SetUp]
        public void Abrir()
        {
            Scene s = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SceneManager.SetActiveScene(s);
            Transform ancoras = new GameObject(AurenSceneBuilder.RaizAncoras).transform;
            foreach (string id in AurenSceneBuilder.Ancoras)
            {
                Transform a = new GameObject(id).transform;
                a.SetParent(ancoras, false);
                a.position = AurenSceneBuilder.PosicaoDaAncora(id);
            }
            PecasDeEventoSetup.Montar(ancoras);
            raiz = AurenSceneBuilder.Achar(PecasDeEventoSetup.Raiz).transform;
        }

        [TearDown]
        public void Fechar() { LogAssert.NoUnexpectedReceived(); }

        [Test]
        public void CenaGerada_NoEstadoDeProjeto_SemColisor_ENomesUnicos()
        {
            foreach (PecaPorEvento p in raiz.GetComponentsInChildren<PecaPorEvento>(true))
                Assert.AreEqual(Array.IndexOf(Antes, p.name) >= 0, p.Ligada, "cena salva aos 5 anos, nada aconteceu: " + p.name);

            Assert.IsEmpty(raiz.GetComponentsInChildren<Collider>(true), "peca com colisor barraria percurso (T008)");

            var nomes = new HashSet<string>();
            foreach (Transform t in raiz) Assert.IsTrue(nomes.Add(t.name), "nome repetido entre irmaos (CenaEstavel): " + t.name);
        }

        /// <summary>A MESMA instancia de historico cresce, como no jogo: transicao de missao nao recarrega a cena, entao a
        /// peca tem de perceber o Total mudar.</summary>
        [Test]
        public void Pecas_SeguemOHistorico_DoComecoAoSalto()
        {
            var h = new LifeEventHistory(new SaveData());
            Confere(h, "antes do sumico", Antes);

            h.Registrar(QuestCatalog.EventoNiloDesapareceu, LifeEventCategoria.Marco, 5);
            Confere(h, "do sumico ao salto", "forquilha_nilo", "tira_crianca_vao", "tira_nilo_vao", "folha_maelis", "bancada_borin_fundo");

            h.Registrar(AgeAdvanceCatalog.SaltoInfancia, LifeEventCategoria.Marco, 8);
            Confere(h, "depois do salto", "tira_crianca_prateleira", "tira_nilo_prateleira", "folha_maelis", "bancada_borin_porta");
        }

        void Confere(LifeEventHistory h, string quando, params string[] ligadas)
        {
            PecaPorEvento[] pecas = raiz.GetComponentsInChildren<PecaPorEvento>(true);
            Assert.AreEqual(8, pecas.Length, "peca a mais ou a menos");
            foreach (PecaPorEvento p in pecas)
            {
                p.Atualizar(h);
                bool espera = Array.IndexOf(ligadas, p.name) >= 0;
                Assert.AreEqual(espera, p.Ligada, quando + ": " + p.name);
                Assert.IsTrue(p.gameObject.activeSelf, "a peca fica ligada para acompanhar o historico: " + p.name);
                Assert.Greater(p.transform.childCount, 0, p.name + " sem visual");
                foreach (Transform filho in p.transform)
                    Assert.AreEqual(espera, filho.gameObject.activeSelf, quando + ": " + p.name + "/" + filho.name);
            }
        }
    }
}
