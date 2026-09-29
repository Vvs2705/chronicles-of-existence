using System.Collections.Generic;
using COE.EditorTools;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace COE.EditorTests
{
    /// <summary>T008: prova que AurenSceneBuilder.Populate monta a vila em uma cena nova em memoria - ancoras com
    /// id estavel, tres casas acessiveis com interior e porta livre, tres estruturas publicas solidas e a borda do
    /// Bosque dos Sussurros barrando o jogador com um vao aberto na entrada, horta atras da casa da familia e todo
    /// ancora alcancavel a partir do spawn pela capsula do Player (crianca). Cada teste monta e fecha a propria
    /// cena (aditiva), nenhum depende do arquivo Auren.unity.</summary>
    public class AurenSceneTests
    {
        readonly List<Scene> abertas = new List<Scene>();

        [SetUp]
        public void Abrir() { NovaCena(); }

        [TearDown]
        public void Fechar()
        {
            // Com NewSceneMode.Single cada teste ja substitui a cena do anterior, entao nao ha o que fechar —
            // e fechar a ULTIMA cena carregada nao e suportado pelo Unity (vira warning e reprova o NoUnexpectedReceived).
            // So fecha o que sobrou aberto alem da cena ativa.
            for (int i = abertas.Count - 1; i >= 0; i--)
                if (abertas[i].IsValid() && abertas[i].isLoaded && SceneManager.loadedSceneCount > 1)
                    EditorSceneManager.CloseScene(abertas[i], true);
            abertas.Clear();
            LogAssert.NoUnexpectedReceived();
        }

        Scene NovaCena()
        {
            // Em batch mode a cena inicial e "untitled" e nao salva, e NewScene ADITIVA lanca InvalidOperationException.
            // Single descarta a cena atual (o runner nao tem trabalho a perder) e funciona no editor e no batch.
            Scene s = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SceneManager.SetActiveScene(s);
            abertas.Add(s);
            return s;
        }

        [Test]
        public void Populate_CriaTodosOsAncorasComIdEstavel()
        {
            AurenSceneBuilder.Populate();

            Transform raiz = AurenSceneBuilder.Achar(AurenSceneBuilder.RaizAncoras).transform;
            Assert.AreEqual(AurenSceneBuilder.Ancoras.Count, raiz.childCount, "ancora a mais ou a menos na cena");
            foreach (string id in AurenSceneBuilder.Ancoras)
            {
                Transform a = raiz.Find(id);
                Assert.IsNotNull(a, "falta o ancora '" + id + "' (contrato com T006/T007/T012)");
                Assert.Less(Vector3.Distance(a.position, AurenSceneBuilder.PosicaoDaAncora(id)), 0.01f,
                            "ancora '" + id + "' fora da posicao de projeto");
            }
        }

        [Test]
        public void Populate_TresCasasAcessiveis_TemInteriorEVaoDePortaLivre()
        {
            AurenSceneBuilder.Populate();

            Assert.AreEqual(3, AurenSceneBuilder.CasasAcessiveis.Count, "o dossie pede tres casas acessiveis");
            foreach (string id in AurenSceneBuilder.CasasAcessiveis)
            {
                Transform casa = Construcao(id);
                Assert.IsNotNull(casa.Find("piso"), id + ": sem piso");
                Assert.IsNotNull(casa.Find("verga"), id + ": sem verga sobre a porta");

                int paredes = 0;
                foreach (Transform t in casa) if (t.name.StartsWith("parede")) paredes++;
                Assert.AreEqual(5, paredes, id + ": fundo + dois lados + dois trechos de fachada");
                Assert.GreaterOrEqual(casa.GetComponentsInChildren<Collider>().Length, 6, id + ": interior sem colisao");

                SimpleInteractable porta = casa.GetComponentInChildren<SimpleInteractable>();
                Assert.IsNotNull(porta, id + ": porta sem SimpleInteractable");
                Assert.IsNotEmpty(porta.Prompt, id + ": porta sem prompt de texto");

                // O vao (centro da fachada, na altura do peito) precisa estar vazio: o jogador entra andando.
                Transform frente = casa.Find("parede_frente_esq");
                Vector3 vao = casa.TransformPoint(new Vector3(0f, 1.2f, frente.localPosition.z));
                Assert.IsFalse(AlgumColisorContem(vao), id + ": o vao da porta esta tapado");

                Assert.IsNotNull(Ancora(id), id + ": sem marcador de entrada em Ancoras/");
            }
        }

        [Test]
        public void Populate_TresEstruturasPublicas_SaoBlocoSolidoComPortaMarcada()
        {
            AurenSceneBuilder.Populate();

            Assert.AreEqual(3, AurenSceneBuilder.EstruturasPublicas.Count, "o dossie pede tres estruturas publicas");
            foreach (string id in AurenSceneBuilder.EstruturasPublicas)
            {
                Transform e = Construcao(id);
                Assert.IsNotNull(e.Find("corpo"), id + ": sem corpo");
                Assert.IsNotNull(e.Find("telhado"), id + ": sem telhado");
                Assert.IsTrue(AlgumColisorContem(e.position + Vector3.up), id + ": o corpo nao barra o jogador");
                Assert.IsNotNull(Ancora(id), id + ": sem ancora de porta");
            }
        }

        [Test]
        public void Populate_BordaDoBosque_BarraOJogadorEDeixaAEntradaAberta()
        {
            AurenSceneBuilder.Populate();

            Assert.IsNotNull(Ancora("entrada_bosque"), "sem ancora da entrada do bosque");
            Vector3 entrada = AurenSceneBuilder.PosicaoDaAncora("entrada_bosque") + Vector3.up;
            Assert.IsFalse(AlgumColisorContem(entrada + Vector3.forward * 2f), "o vao da entrada do bosque esta tapado");
            Assert.IsTrue(AlgumColisorContem(new Vector3(20f, 1.5f, 62f)), "a borda leste do bosque nao barra");
            Assert.IsTrue(AlgumColisorContem(new Vector3(-20f, 1.5f, 62f)), "a borda oeste do bosque nao barra");
            Assert.IsTrue(AlgumColisorContem(new Vector3(0f, 1.5f, 78f)), "a clareira do bosque nao tem fundo");
            Assert.IsTrue(AlgumColisorContem(new Vector3(0f, 1.5f, 89.5f)), "o limite norte do mundo nao barra");
        }

        [Test]
        public void Populate_LimpaOsObjetosDeProvaDoT002_EPoeOPlayerNoSpawn()
        {
            AurenSceneBuilder.Populate();

            foreach (string nome in new[] { "Ground", "Poste", "Caixa" })
                Assert.IsFalse(TemRaiz(nome), "sobrou '" + nome + "' do BootstrapSceneBuilder na cena de Auren");

            Transform player = AurenSceneBuilder.Achar("Player").transform;
            Assert.IsNotNull(player.GetComponent<CharacterController>(), "Player sem CharacterController");
            Assert.Less(Vector3.Distance(player.position, AurenSceneBuilder.PosicaoDaAncora("spawn_player")), 0.01f,
                        "Player nao nasceu no ancora spawn_player");
        }

        [Test]
        public void Populate_CincoInteragiveisDeProva_TodosComPrompt()
        {
            AurenSceneBuilder.Populate();

            var achados = new List<SimpleInteractable>();
            foreach (GameObject raiz in SceneManager.GetActiveScene().GetRootGameObjects())
                achados.AddRange(raiz.GetComponentsInChildren<SimpleInteractable>());

            Assert.AreEqual(5, achados.Count, "tres portas de casa + poco + mural de avisos");
            foreach (SimpleInteractable it in achados) Assert.IsNotEmpty(it.Prompt, it.name + ": prompt vazio");
        }

        [Test]
        public void Populate_TodoAncoraTemPercursoLivre_ParaACapsulaDoPlayer()
        {
            AurenSceneBuilder.Populate();
            CharacterController cc = AurenSceneBuilder.Achar("Player").GetComponent<CharacterController>();
            Assert.IsNotNull(cc, "Player sem CharacterController");
            Physics.SyncTransforms();
            string capsula = " (capsula r=" + (cc.radius + cc.skinWidth) + " h=" + cc.height + " degrau=" + cc.stepOffset + ")";

            // Controle: a varredura enxerga parede. Sem isto, fisica fora do ar no EditMode faria o resto passar vazio.
            Vector3 fora = AurenSceneBuilder.PosicaoDaAncora("casa_familia") + new Vector3(-3f, 0f, 0f);
            Assert.IsNotNull(Bloqueio(cc, fora, fora + new Vector3(0f, 0f, -5f)),
                             "a varredura atravessou a fachada de casa_familia fora do vao" + capsula);

            foreach (string id in AurenSceneBuilder.Ancoras)
            {
                if (id == "spawn_player") continue;
                Vector3[] p = AurenSceneBuilder.Percurso(id);
                for (int i = 1; i < p.Length; i++)
                {
                    string b = Bloqueio(cc, p[i - 1], p[i]);
                    Assert.IsNull(b, "spawn_player -> " + id + ": trecho " + p[i - 1] + " -> " + p[i] + " barrado por " + b + capsula);
                }
            }

            // Da porta ao interior: passa no vao, sob a verga.
            foreach (string id in AurenSceneBuilder.CasasAcessiveis)
            {
                string b = Bloqueio(cc, AurenSceneBuilder.PosicaoDaAncora(id), Construcao(id).position);
                Assert.IsNull(b, id + ": da porta ao interior barrado por " + b + capsula);
            }
        }

        [Test]
        public void Populate_HortaDaFamilia_TemCanteiroAtrasDaCasa()
        {
            AurenSceneBuilder.Populate();

            Transform horta = AurenSceneBuilder.Achar(AurenSceneBuilder.RaizMundo).transform.Find("Cenario/horta_familia");
            Assert.IsNotNull(horta, "falta a horta da familia (slice secao 1.2, objetivo procurar_na_horta de q03)");
            Assert.IsNotNull(horta.Find("canteiro"), "horta sem canteiro");
            // A porta de casa_familia abre para o norte (rua); "atras" e o sul.
            float zCasa = Construcao("casa_familia").position.z;
            Assert.Less(horta.position.z, zCasa, "a horta tem de ficar atras de casa_familia, nao na rua");
            Assert.Less(AurenSceneBuilder.PosicaoDaAncora("horta_familia").z, zCasa, "ancora horta_familia na frente da casa");
        }

        [Test]
        public void T011_ParceiroDeTreino_FicaNoPostoDaGuarda()
        {
            AurenSceneBuilder.Populate();

            // B15: treino supervisionado no posto_guarda. Que ele nao barra nenhum percurso, o
            // Populate_TodoAncoraTemPercursoLivre ja varre (o colisor do parceiro esta na cena).
            Transform parceiro = AurenSceneBuilder.Achar("ParceiroDeTreino").transform;
            Assert.IsNotNull(parceiro.GetComponent<TrainingDummy>(), "ParceiroDeTreino sem TrainingDummy");
            Vector3 d = parceiro.position - AurenSceneBuilder.PosicaoDaAncora("posto_guarda");
            d.y = 0f;
            Assert.Less(d.magnitude, 5f, "o parceiro de treino ficou longe do posto_guarda (B15)");
            Assert.Greater(parceiro.position.y, 0f, "pivo no centro da capsula: a altura do Bootstrap se mantem");
        }

        [Test]
        public void Populate_DuasVezes_EmCenasNovas_NaoLancaENaoMudaOContrato()
        {
            AurenSceneBuilder.Populate();
            int ancoras = AurenSceneBuilder.Achar(AurenSceneBuilder.RaizAncoras).transform.childCount;

            NovaCena(); // Build() sempre parte de uma cena nova; a segunda passada repete esse caminho
            Assert.DoesNotThrow(() => AurenSceneBuilder.Populate());
            Assert.AreEqual(ancoras, AurenSceneBuilder.Achar(AurenSceneBuilder.RaizAncoras).transform.childCount);
        }

        // ---------------------------------------------------------------- utilidades

        static Transform Construcao(string id)
        {
            Transform t = AurenSceneBuilder.Achar(AurenSceneBuilder.RaizMundo).transform.Find("Construcoes/" + id);
            Assert.IsNotNull(t, "falta a construcao '" + id + "'");
            return t;
        }

        static Transform Ancora(string id)
        {
            return AurenSceneBuilder.Achar(AurenSceneBuilder.RaizAncoras).transform.Find(id);
        }

        static bool TemRaiz(string nome)
        {
            foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
                if (go.name == nome) return true;
            return false;
        }

        /// <summary>Varre a capsula do CharacterController do Player de 'de' ate 'para' (pes em y=0), com a base erguida
        /// ate o stepOffset: o que fica abaixo do degrau o CharacterController sobe. Devolve "pai/nome" do primeiro
        /// colisor que barra, ou null se o trecho esta livre. Ignora o proprio Player e triggers.
        /// ponytail: linha reta entre pontos autorados, nao pathfinding. NavMesh so quando NPC navegar (T007).</summary>
        static string Bloqueio(CharacterController cc, Vector3 de, Vector3 para)
        {
            float r = cc.radius + cc.skinWidth;
            Vector3 baixo = Vector3.up * (cc.stepOffset + r);
            Vector3 cima = Vector3.up * (cc.center.y + cc.height * 0.5f - r);
            Transform player = cc.transform;

            foreach (Collider c in Physics.OverlapCapsule(de + baixo, de + cima, r, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                if (!c.transform.IsChildOf(player)) return Nome(c);

            Vector3 d = para - de;
            if (d.sqrMagnitude < 1e-6f) return null;
            foreach (RaycastHit h in Physics.CapsuleCastAll(de + baixo, de + cima, r, d.normalized, d.magnitude,
                                                            Physics.AllLayers, QueryTriggerInteraction.Ignore))
                if (!h.collider.transform.IsChildOf(player)) return Nome(h.collider);
            return null;
        }

        static string Nome(Collider c)
        {
            return c.transform.parent != null ? c.transform.parent.name + "/" + c.name : c.name;
        }

        /// <summary>Colisao por AABB de colisor, sem depender de Physics.OverlapX fora do play mode.</summary>
        static bool AlgumColisorContem(Vector3 ponto)
        {
            Physics.SyncTransforms();
            foreach (GameObject raiz in SceneManager.GetActiveScene().GetRootGameObjects())
                foreach (Collider c in raiz.GetComponentsInChildren<Collider>())
                    if (c.bounds.Contains(ponto)) return true;
            return false;
        }
    }
}
