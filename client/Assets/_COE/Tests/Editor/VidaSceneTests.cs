using System.Collections.Generic;
using COE.EditorTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace COE.EditorTests
{
    /// <summary>A vila viva (VidaSceneSetup) dentro do contrato do T008: casa de fundo, arvore e adorno solido longe de todo
    /// percurso; rota de morador sem parede no caminho; circulo de bicho livre; morador e bicho sem colisor, ligados ao
    /// jogador, e somem nos periodos certos; o som esta montado; com prototipo a casa de fundo vira modelo espelhado e
    /// tingido sem perder a colisao. Cada teste monta a propria cena em memoria.</summary>
    public class VidaSceneTests
    {
        GameObject falso;

        [SetUp]
        public void Abrir()
        {
            Scene s = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SceneManager.SetActiveScene(s);
        }

        [TearDown]
        public void Fechar()
        {
            Prototipos.Carregar = Prototipos.CarregarDoDisco;
            if (falso != null) Object.DestroyImmediate(falso);
            LogAssert.NoUnexpectedReceived();
        }

        static Transform Vila() { return AurenSceneBuilder.Achar(AurenSceneBuilder.RaizMundo).transform.Find(VidaSceneSetup.RaizVila); }

        static List<(Vector3, Vector3)> Trechos()
        {
            var trechos = new List<(Vector3, Vector3)>();
            foreach (string id in AurenSceneBuilder.Ancoras)
            {
                if (id == "spawn_player") continue;
                Vector3[] p = AurenSceneBuilder.Percurso(id);
                for (int i = 1; i < p.Length; i++) trechos.Add((p[i - 1], p[i]));
            }
            return trechos;
        }

        /// <summary>Menor distancia no plano entre o trecho e a caixa do colisor (amostra o trecho a cada 10 cm).</summary>
        static float Distancia(Bounds b, Vector3 a, Vector3 c)
        {
            float m = float.MaxValue;
            int n = Mathf.Max(1, Mathf.CeilToInt(Vector3.Distance(a, c) / 0.1f));
            for (int i = 0; i <= n; i++)
            {
                Vector3 p = Vector3.Lerp(a, c, i / (float)n);
                p.y = b.center.y;
                Vector3 q = b.ClosestPoint(p);
                m = Mathf.Min(m, new Vector2(p.x - q.x, p.z - q.z).magnitude);
            }
            return m;
        }

        static bool Ignorar(Collider c)
        {
            return c.isTrigger || c.GetComponentInParent<NpcActor>() != null || c is CharacterController
                   || c.transform.root.name == "Player";
        }

        [Test]
        public void Cenario_NadaSolidoDaVila_EncostaEmPercurso()
        {
            Prototipos.Carregar = _ => null;
            AurenSceneBuilder.Populate();
            Physics.SyncTransforms();
            List<(Vector3, Vector3)> trechos = Trechos();

            Collider[] solidos = Vila().GetComponentsInChildren<Collider>();
            Assert.Greater(solidos.Length, VidaSceneSetup.Casas.Length + VidaSceneSetup.Arvores.Length, "vila sem colisao");
            var erros = new List<string>();
            foreach (Collider c in solidos)
                foreach (var (a, b) in trechos)
                    if (Distancia(c.bounds, a, b) < VidaSceneSetup.FolgaDoPercurso)
                        erros.Add(c.transform.parent.name + "/" + c.name + " a " + Distancia(c.bounds, a, b).ToString("F2") + " m de " + a + " -> " + b);
            CollectionAssert.IsEmpty(erros);
        }

        [Test]
        public void Cenario_DezesseisCasasNovas_SolidasComTelhado_ENaoSeSobrepoem()
        {
            Prototipos.Carregar = _ => null;
            AurenSceneBuilder.Populate();
            Physics.SyncTransforms();
            Transform casas = Vila().Find("Casas");
            Assert.AreEqual(16, casas.childCount);

            var caixas = new List<(string, Bounds)>();
            foreach (Transform t in AurenSceneBuilder.Achar(AurenSceneBuilder.RaizMundo).transform.Find("Construcoes"))
                foreach (Collider c in t.GetComponentsInChildren<Collider>()) caixas.Add((t.name, c.bounds));
            foreach (VidaSceneSetup.Casa casa in VidaSceneSetup.Casas)
            {
                Transform t = casas.Find(casa.Id);
                Assert.IsNotNull(t, casa.Id);
                Assert.IsNotNull(t.Find("telhado_frente"), casa.Id + " sem telhado");
                Assert.AreEqual(1, t.GetComponentsInChildren<Collider>().Length, casa.Id + ": colisao so no corpo");
                Bounds b = t.Find("corpo").GetComponent<Collider>().bounds;
                foreach (var (quem, outra) in caixas)
                    Assert.IsFalse(b.Intersects(outra), casa.Id + " em cima de " + quem);
                caixas.Add((casa.Id, b));
            }
        }

        [Test]
        public void Moradores_TodaRotaLivreDeParede_ESemColisor()
        {
            AurenSceneBuilder.Populate();
            Physics.SyncTransforms();
            Morador[] moradores = Object.FindObjectsByType<Morador>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
            Assert.AreEqual(VidaSceneSetup.Moradores.Length, moradores.Length);

            const float r = 0.3f;
            var erros = new List<string>();
            foreach (Morador m in moradores)
            {
                Assert.IsEmpty(m.GetComponentsInChildren<Collider>(true), m.name + " com colisor: barraria percurso");
                foreach (TimeOfDay p in new[] { TimeOfDay.Manha, TimeOfDay.Tarde, TimeOfDay.Noite })
                {
                    Vector3[] rota = m.Rota(p);
                    for (int i = 0; i < rota.Length && rota.Length > 1; i++)
                    {
                        Vector3 a = rota[i], b = rota[(i + 1) % rota.Length];
                        int n = Mathf.CeilToInt(Vector3.Distance(a, b) / 0.25f);
                        for (int k = 0; k <= n; k++)
                        {
                            Vector3 q = Vector3.Lerp(a, b, k / (float)n);
                            foreach (Collider c in Physics.OverlapCapsule(q + Vector3.up * (r + 0.2f), q + Vector3.up * (BodyScale.Adulto - r), r,
                                                                          Physics.AllLayers, QueryTriggerInteraction.Ignore))
                                if (!Ignorar(c)) erros.Add(m.name + " (" + p + ") atravessa " + c.transform.parent?.name + "/" + c.name + " em " + q);
                        }
                    }
                }
            }
            CollectionAssert.IsEmpty(new HashSet<string>(erros));
        }

        [Test]
        public void Bichos_CirculoLivre_SemColisor_ELigadosAoJogador()
        {
            AurenSceneBuilder.Populate();
            Physics.SyncTransforms();
            Transform player = AurenSceneBuilder.Achar("Player").transform;
            Animal[] bichos = Object.FindObjectsByType<Animal>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
            Assert.GreaterOrEqual(bichos.Length, 12, "oito galinhas, dois porcos, o cachorro e o gato");

            var erros = new List<string>();
            foreach (Animal a in bichos)
            {
                Assert.IsEmpty(a.GetComponentsInChildren<Collider>(true), a.name + " com colisor");
                Assert.AreSame(player, new SerializedObject(a).FindProperty("jogador").objectReferenceValue, a.name + " sem jogador");
                Assert.LessOrEqual(Vector3.Distance(Plano(a.transform.position), Plano(a.Centro)), a.Raio + 1e-3f, a.name + " nasce fora do circulo");
                for (int i = 0; i <= 16; i++)
                {
                    Vector3 p = i == 16 ? a.Centro : a.Centro + Quaternion.Euler(0f, i * 22.5f, 0f) * Vector3.forward * a.Raio;
                    foreach (Collider c in Physics.OverlapSphere(Plano(p) + Vector3.up * 0.3f, 0.2f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                        if (!Ignorar(c)) erros.Add(a.name + " anda dentro de " + c.transform.parent?.name + "/" + c.name + " em " + p);
                }
            }
            CollectionAssert.IsEmpty(new HashSet<string>(erros));
        }

        [Test]
        public void Vida_ANoiteQuaseTodoMundoRecolhe_EDeDiaVolta()
        {
            AurenSceneBuilder.Populate();
            int naRuaANoite = 0, naRuaDeManha = 0;
            foreach (Morador m in Object.FindObjectsByType<Morador>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
            {
                Transform corpo = m.transform.Find("Corpo");
                m.Atualizar(TimeOfDay.Noite, 0.1f);
                Assert.AreEqual(m.Rota(TimeOfDay.Noite).Length > 0, corpo.gameObject.activeSelf, m.name + " a noite");
                if (corpo.gameObject.activeSelf) naRuaANoite++;
                m.Atualizar(TimeOfDay.Manha, 0.1f);
                Assert.AreEqual(m.Rota(TimeOfDay.Manha).Length > 0, corpo.gameObject.activeSelf, m.name + " de manha");
                if (!corpo.gameObject.activeSelf) continue;
                naRuaDeManha++;
                Vector3[] rota = m.Rota(TimeOfDay.Manha);
                float perto = float.MaxValue;
                foreach (Vector3 q in rota) perto = Mathf.Min(perto, Vector3.Distance(q, m.transform.position));
                Assert.IsNotNull(m.Atual, m.name);
                Assert.Less(perto, m.Atual.Duracao * VidaSceneSetup.VelocidadeDoMorador, m.name + " longe da propria rota");
            }
            Assert.Greater(naRuaDeManha, 6, "de manha a vila enche");
            Assert.That(naRuaANoite, Is.InRange(1, 3), "a noite fica o vigia e pouca gente");

            foreach (Animal a in Object.FindObjectsByType<Animal>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
            {
                if (a.Especie != Especie.Galinha) continue;
                a.Atualizar(TimeOfDay.Noite, 0.1f);
                Assert.IsFalse(a.transform.Find("Corpo").gameObject.activeSelf, a.name + ": galinha recolhe a noite");
                a.Atualizar(TimeOfDay.Tarde, 0.1f);
                Assert.IsTrue(a.transform.Find("Corpo").gameObject.activeSelf, a.name);
            }
        }

        [Test]
        public void Sons_AmbienteBosqueBigornaPocoEBichos_Montados_EPortaBate()
        {
            AurenSceneBuilder.Populate();
            Assert.AreEqual(1, Object.FindObjectsByType<SomAmbiente>(FindObjectsSortMode.None).Length, "um fundo por cena");
            var sons = new HashSet<SomDoMundo>();
            foreach (FonteDeSom f in Object.FindObjectsByType<FonteDeSom>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
                sons.Add(f.Som);
            foreach (SomDoMundo s in System.Enum.GetValues(typeof(SomDoMundo)))
                Assert.IsTrue(sons.Contains(s), "ninguem toca " + s);

            int portas = 0;
            foreach (SimpleInteractable s in Object.FindObjectsByType<SimpleInteractable>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
            {
                Assert.IsNotNull(new SerializedObject(s).FindProperty("som").objectReferenceValue, s.name + " mudo");
                if (s.somAoUsar == Som.Porta) portas++;
            }
            Assert.AreEqual(AurenSceneBuilder.CasasAcessiveis.Count, portas, "as tres portas batem; poco e mural dao o clique");
        }

        [Test]
        public void ComPrototipo_CasaDeFundoViraModelo_EspelhadaETingida_SemPerderAColisao()
        {
            falso = new GameObject("falso");
            GameObject cubo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Object.DestroyImmediate(cubo.GetComponent<Collider>());
            cubo.transform.SetParent(falso.transform, false);
            Prototipos.Carregar = _ => falso;
            AurenSceneBuilder.Populate();

            Transform casas = Vila().Find("Casas");
            foreach (VidaSceneSetup.Casa c in VidaSceneSetup.Casas)
            {
                Transform proto = casas.Find(Prototipos.Prefixo + c.Id);
                Assert.IsNotNull(proto, c.Id + " sem prototipo");
                Assert.AreEqual(c.Espelhar ? -1f : 1f, proto.localScale.x, 1e-4f, c.Id + " espelhamento");
                Assert.AreEqual(c.Tinta != Color.white, proto.GetComponent<Tingir>() != null, c.Id + " tinta");
                Transform greybox = casas.Find(c.Id);
                foreach (Renderer r in greybox.GetComponentsInChildren<Renderer>()) Assert.IsFalse(r.enabled, "greybox a mostra: " + c.Id);
                Assert.IsNotNull(greybox.Find("corpo").GetComponent<Collider>(), c.Id + " perdeu a colisao");
            }
            Assert.IsNotNull(Vila().Find("Adornos/" + Prototipos.Prefixo + "barril"), "adorno nao virou peca");
            Assert.IsNotNull(Vila().Find("Arvoredo/" + Prototipos.Prefixo + "arvore"), "arvore da vila nao virou prototipo");
            foreach (Morador m in Object.FindObjectsByType<Morador>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
                Assert.IsNotNull(m.transform.Find("Corpo").GetComponentInChildren<Renderer>(), m.name + " sem corpo");
        }

        /// <summary>Variedade: cada morador com carga leva a peca dele (chapeu, cesta, saco, enxada, lenha, lanterna), sem
        /// colisor e dentro do Corpo (some junto a noite); os bichos de quatro patas andam com as pernas.</summary>
        [Test]
        public void Moradores_LevamACargaDeles_EBichosTemPernas()
        {
            AurenSceneBuilder.Populate();
            Transform raiz = AurenSceneBuilder.Achar(VidaSceneSetup.RaizVida).transform.Find("Moradores");
            var cargas = new HashSet<VidaSceneSetup.Carga>();
            foreach (VidaSceneSetup.Figura f in VidaSceneSetup.Moradores)
            {
                Transform corpo = raiz.Find(f.Id + "/Corpo");
                Assert.IsNotNull(corpo, f.Id);
                int pecas = 0;
                foreach (Renderer r in corpo.GetComponentsInChildren<Renderer>(true))
                    if (r.name.StartsWith("carga_", System.StringComparison.Ordinal)) pecas++;
                Assert.AreEqual(f.Leva != VidaSceneSetup.Carga.Nada, pecas > 0, f.Id + " leva " + f.Leva);
                cargas.Add(f.Leva);
            }
            Assert.GreaterOrEqual(cargas.Count, 6, "silhuetas variadas");

            foreach (Animal a in Object.FindObjectsByType<Animal>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
            {
                SerializedProperty pernas = new SerializedObject(a).FindProperty("pernas");
                Assert.AreEqual(a.Especie == Especie.Galinha ? 2 : 4, pernas.arraySize, a.name + " pernas");
                for (int i = 0; i < pernas.arraySize; i++) Assert.IsNotNull(pernas.GetArrayElementAtIndex(i).objectReferenceValue, a.name);
            }
        }

        /// <summary>Toda casa de fundo tem chamine em cima do telhado, sem colisor; duas de cada tres soltam fumaca, que some
        /// nos periodos sem fogo.</summary>
        [Test]
        public void Chamines_EmTodaCasaDeFundo_FumacaPorPeriodo()
        {
            AurenSceneBuilder.Populate();
            Transform raiz = Vila().Find(VidaSceneSetup.RaizChamines);
            Assert.AreEqual(VidaSceneSetup.Casas.Length, raiz.childCount, "uma chamine por casa de fundo");
            Assert.IsEmpty(raiz.GetComponentsInChildren<Collider>(true), "chamine e fumaca nao tem colisor");
            foreach (VidaSceneSetup.Casa c in VidaSceneSetup.Casas)
            {
                Transform ch = raiz.Find(c.Id);
                Assert.Greater(ch.position.y, VidaSceneSetup.AlturaDaCasa, c.Id + ": chamine abaixo do telhado");
            }

            Fumaca[] fumacas = raiz.GetComponentsInChildren<Fumaca>(true);
            Assert.GreaterOrEqual(fumacas.Length, 10);
            foreach (Fumaca f in fumacas)
            {
                f.Atualizar(TimeOfDay.Noite, 0.5f);
                Assert.IsTrue(f.transform.GetChild(0).gameObject.activeSelf, f.transform.parent.name + ": a noite a lareira fuma");
                f.Atualizar(TimeOfDay.Tarde, 0.5f);
                Assert.IsFalse(f.transform.GetChild(0).gameObject.activeSelf, f.transform.parent.name + ": a tarde sem fogo");
                Assert.IsFalse(GameObjectUtility.AreStaticEditorFlagsSet(f.transform.GetChild(0).gameObject, StaticEditorFlags.BatchingStatic),
                               "fumaca se mexe: fora do lote");
            }
        }

        /// <summary>A noite a vila acende: janelas de tres de cada quatro casas e a lanterna de cada porta trocam para o
        /// material aceso, e de dia voltam ao apagado. Nada disso tem colisor.</summary>
        [Test]
        public void ANoite_JanelasELanternasAcendem_EDeDiaApagam()
        {
            AurenSceneBuilder.Populate();
            Transform janelas = Vila().Find(VidaSceneSetup.RaizJanelas);
            Assert.AreEqual(VidaSceneSetup.Casas.Length, janelas.childCount, "janelas em toda casa de fundo");
            Assert.IsEmpty(janelas.GetComponentsInChildren<Collider>(true));

            AcendeANoite[] luzes = Object.FindObjectsByType<AcendeANoite>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
            int casas = 0;
            foreach (AcendeANoite l in luzes) if (l.transform.parent == janelas) casas++;
            Assert.AreEqual(12, casas, "tres de cada quatro casas acendem");
            Assert.GreaterOrEqual(luzes.Length - casas, VidaSceneSetup.Casas.Length, "uma lanterna por porta");

            foreach (AcendeANoite l in luzes)
            {
                Renderer r = l.GetComponentInChildren<Renderer>(true);
                l.Atualizar(TimeOfDay.Manha);
                Material dia = r.sharedMaterial;
                Assert.IsFalse(l.Acesa, l.name);
                l.Atualizar(TimeOfDay.Noite);
                Assert.IsTrue(l.Acesa, l.name);
                Assert.AreNotSame(dia, r.sharedMaterial, l.name + ": a noite nao acendeu");
                l.Atualizar(TimeOfDay.Tarde);
                Assert.AreSame(dia, r.sharedMaterial, l.name + ": de dia nao apagou");
            }
        }

        /// <summary>Custo de desenho: casa e arvore viram lote estatico; nada que anda (morador, bicho, NPC, Player) vira,
        /// senao fica congelado no lugar do build; adorno pequeno nao projeta sombra.</summary>
        [Test]
        public void Lote_ParadoViraLote_QuemAndaNao_AdornoSemSombra()
        {
            AurenSceneBuilder.Populate();
            int lote = 0;
            foreach (GameObject raiz in SceneManager.GetActiveScene().GetRootGameObjects())
                foreach (MeshRenderer r in raiz.GetComponentsInChildren<MeshRenderer>(true))
                {
                    bool estatico = GameObjectUtility.AreStaticEditorFlagsSet(r.gameObject, StaticEditorFlags.BatchingStatic);
                    bool anda = r.GetComponentInParent<Morador>(true) != null || r.GetComponentInParent<Animal>(true) != null
                                || r.GetComponentInParent<Bando>(true) != null || r.GetComponentInParent<NpcActor>(true) != null
                                || r.GetComponentInParent<CharacterMotor>(true) != null || r.GetComponentInParent<TrainingDummy>(true) != null;
                    if (anda) Assert.IsFalse(estatico, r.name + " anda e virou lote estatico (congela no build)");
                    if (estatico) lote++;
                }
            Transform casa = Vila().Find("Casas").GetChild(0);
            foreach (MeshRenderer r in casa.GetComponentsInChildren<MeshRenderer>())
                Assert.IsTrue(GameObjectUtility.AreStaticEditorFlagsSet(r.gameObject, StaticEditorFlags.BatchingStatic), r.name + " da casa fora do lote");
            Assert.Greater(lote, 200, "a vila parada inteira vira lote");

            int adornos = 0;
            foreach (Renderer r in Vila().GetComponentsInChildren<Renderer>(true))
                if (r.transform.parent != null && (r.name == "arbusto" || r.name == "barril" || r.name == "caixote"))
                {
                    adornos++;
                    Assert.AreEqual(UnityEngine.Rendering.ShadowCastingMode.Off, r.shadowCastingMode, r.name + " projeta sombra");
                }
            Assert.Greater(adornos, 16, "pressuposto: adornos nas portas");
        }

        /// <summary>Feira de dia na praca: cada barraca com bancada solida (o unico colisor), toldo e mercadoria; o feirante
        /// fica parado atras da bancada olhando para ela de manha e de tarde e recolhe a noite.</summary>
        [Test]
        public void Feira_BarracaComBancadaSolida_FeiranteAtrasOlhandoParaAPraca_ENoiteRecolhe()
        {
            Prototipos.Carregar = _ => null;
            AurenSceneBuilder.Populate();
            Transform feira = Vila().Find(VidaSceneSetup.RaizFeira);
            Assert.AreEqual(VidaSceneSetup.Feira.Length, feira.childCount);
            Transform moradores = AurenSceneBuilder.Achar(VidaSceneSetup.RaizVida).transform.Find("Moradores");
            for (int i = 0; i < VidaSceneSetup.Feira.Length; i++)
            {
                VidaSceneSetup.Barraca b = VidaSceneSetup.Feira[i];
                Transform t = feira.Find(b.Id);
                Assert.IsNotNull(t.Find("toldo"), b.Id + " sem toldo");
                Assert.IsNull(t.Find("toldo").GetComponentInChildren<Collider>(), b.Id + ": toldo nao barra ninguem");
                Assert.IsNotNull(t.Find("bancada").GetComponent<Collider>(), b.Id + ": bancada solida");

                Morador m = moradores.Find("feirante_" + (i + 1)).GetComponent<Morador>();
                m.Atualizar(TimeOfDay.Manha, 0.1f);
                Assert.Less(Vector3.Distance(Plano(m.transform.position), Plano(b.Vaga)), 0.01f, m.name + " fora da vaga");
                Assert.IsFalse(m.Atual.Em(1f).Andando, m.name + " anda");
                Vector3 olha = m.transform.Find("Corpo").forward;
                Vector3 paraABancada = Plano(b.Pos - b.Vaga).normalized;
                Assert.Greater(Vector3.Dot(olha, paraABancada), 0.99f, m.name + " de costas para a bancada");
                m.Atualizar(TimeOfDay.Noite, 0.1f);
                Assert.IsFalse(m.transform.Find("Corpo").gameObject.activeSelf, m.name + " a noite");
            }
        }

        /// <summary>Vento: toda copa da vila e todo pano de varal balancam, sem colisor no que se mexe (o tronco solido fica
        /// parado) e fora do lote estatico.</summary>
        [Test]
        public void Vento_CopasEPanosBalancam_SemColisor_EForaDoLote()
        {
            Prototipos.Carregar = _ => null;
            AurenSceneBuilder.Populate();
            Balanco[] todos = Vila().GetComponentsInChildren<Balanco>(true);
            Assert.AreEqual(VidaSceneSetup.Arvores.Length + 4 * VidaSceneSetup.Varais.Length, todos.Length, "copas + quatro panos por varal");
            foreach (Balanco b in todos)
            {
                Assert.IsEmpty(b.GetComponentsInChildren<Collider>(true), b.transform.parent.name + "/" + b.name + " balanca com colisor");
                foreach (MeshRenderer r in b.GetComponentsInChildren<MeshRenderer>(true))
                    Assert.IsFalse(GameObjectUtility.AreStaticEditorFlagsSet(r.gameObject, StaticEditorFlags.BatchingStatic), r.name + " no lote");
            }
            foreach (Transform arvore in Vila().Find("Arvoredo"))
                if (arvore.name == "arvore") Assert.IsNotNull(arvore.Find("tronco").GetComponent<Collider>(), "tronco perdeu a colisao");
            Assert.AreEqual(VidaSceneSetup.Varais.Length, Vila().Find(VidaSceneSetup.RaizVarais).childCount);
        }

        static Vector3 Plano(Vector3 v) { v.y = 0f; return v; }
    }
}
