using System.Collections.Generic;
using System.Linq;
using COE.EditorTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace COE.EditorTests
{
    /// <summary>ADR-0008: o shader toon e o ceu compilam (editor, GLES3 e Vulkan); a normalizacao de escala dos prototipos
    /// do Tripo; e os geradores com e sem prototipo - sem FBX fica o greybox, com FBX o visual troca e colisao, gatilhos e
    /// contratos de cena ficam. O "FBX" dos testes e um objeto de cena falso (Prototipos.Carregar): nenhum teste depende
    /// de haver ou nao arquivo em Art/Prototipo.</summary>
    public class EsteticaTests
    {
        GameObject falso;

        [SetUp]
        public void Abrir()
        {
            // Single: em batch mode a cena inicial e "untitled" e NewScene aditiva lanca (ver AurenSceneTests).
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

        // ---------------------------------------------------------------- shaders

        [Test]
        public void Toon_ExisteCompilaNoEditor_ETemOsQuatroPasses()
        {
            Shader s = Shader.Find(LookSetup.ShaderToon);
            Assert.IsNotNull(s, "falta Assets/_COE/Art/Look/COE_Toon.shader");
            Assert.IsFalse(ShaderUtil.ShaderHasError(s), Mensagens(s));

            // Le o subshader SERIALIZADO: em -nographics o subshader do URP nao fica ativo e o Material so ve o de erro.
            ShaderData.Subshader sub = ShaderUtil.GetShaderData(s).GetSerializedSubshader(0);
            var passes = new HashSet<string>();
            for (int i = 0; i < sub.PassCount; i++) passes.Add(sub.GetPass(i).Name.ToUpperInvariant());
            foreach (string p in new[] { "ToonForward", "Contorno", "ShadowCaster", "DepthOnly" })
                Assert.IsTrue(passes.Contains(p.ToUpperInvariant()), "sem o passe " + p + " (tem: " + string.Join(", ", passes) + ")");
        }

        [Test]
        public void AnimacoesDoMixamo_TrocamOsClipsETrazemOImpactoDoGolpe()
        {
            if (!System.IO.File.Exists(PrototipoAnimacoes.Pasta + "/Punching.fbx"))
                Assert.Inconclusive("sem clips do Mixamo em " + PrototipoAnimacoes.Pasta + " (fallback: Player.controller)");

            var ac = PrototipoAnimacoes.Montar() as UnityEditor.Animations.AnimatorController;
            Assert.IsNotNull(ac, "Prototipo.controller nao montado");
            var estados = ac.layers[0].stateMachine.states.Select(s => s.state).ToArray();

            // Andando (2,2 de 4,8 m/s) o blend tem de cair no clip de ANDAR, nao em meio parado + meio correndo.
            var tree = (UnityEditor.Animations.BlendTree)estados.First(s => s.name == "Locomotion").motion;
            var filhos = tree.children;
            Assert.AreEqual(3, filhos.Length, "Idle, Walk e Run");
            float andando = MotionSolver.VelocidadeCaminhadaPadrao / MotionSolver.VelocidadeCorridaPadrao;
            var walk = filhos.First(f => Mathf.Approximately(f.threshold, andando));
            Assert.AreEqual("Walk", walk.motion.name, "no ponto de andar tem de estar o clip de andar");
            foreach (var f in filhos)
                Assert.AreNotEqual(HumanoidSetup.ControllerPath.Replace("Player.controller", ""),
                    System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(f.motion)).Replace('\\', '/') + "/", f.motion.name + " ainda e do placeholder (bracos em T)");

            AnimationClip golpe = (AnimationClip)estados.First(s => s.name == "Attack1").motion;
            Assert.IsTrue(golpe.events.Any(e => e.functionName == AnimParams.EventHitFrame),
                "sem OnHitFrame o golpe nao causa dano quando o Animator anima (CharacterAnimator.immediate = false)");
        }

        [Test]
        public void Passada_PeApoiadoParaTras_EACadenciaQueCasaComOCorpo()
        {
            // Pe esquerdo apoiado (mais baixo) andando 0,1 m a cada 0,1 s = 1 m/s em qualquer eixo; o direito no ar nao conta.
            var a = new List<(float, float, float, float, float, float, float)>();
            for (int i = 0; i <= 5; i++) a.Add((i * 0.1f, 0.06f * i, 0f, -0.08f * i, 0.5f * i, 0.2f, 0.3f * i));
            Assert.AreEqual(1f, PassadaMedida.VelocidadeDoApoio(a), 1e-4f);
            Assert.AreEqual(0f, PassadaMedida.VelocidadeDoApoio(new List<(float, float, float, float, float, float, float)>()));

            Assert.AreEqual(1.5f, PrototipoAnimacoes.Cadencia(1.5f, 1f), 1e-4f, "corpo a 1,5 m/s, passada natural 1 m/s");
            Assert.AreEqual(PrototipoAnimacoes.CadenciaMaxima, PrototipoAnimacoes.Cadencia(10f, 1f), 1e-4f, "teto: crianca nao pedala");
            Assert.AreEqual(1f, PrototipoAnimacoes.Cadencia(2f, 0f), "medicao falhou = sem ajuste");
        }

        [Test]
        public void Ceu_ExisteECompilaNoEditor()
        {
            Shader s = Shader.Find(LookSetup.ShaderCeu);
            Assert.IsNotNull(s, "falta Assets/_COE/Art/Look/COE_Ceu.shader");
            Assert.IsFalse(ShaderUtil.ShaderHasError(s), Mensagens(s));
        }

        /// <summary>O alvo e Android com Vulkan e GLES3 de reserva (ProjectSetup.AplicarAndroid): cada passe, vertice e
        /// fragmento, sem keyword e com sombra em cascata + fog linear.</summary>
        [Test]
        public void Toon_ECeu_CompilamParaGles3EVulkan()
        {
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Android, BuildTarget.Android))
                Assert.Ignore("modulo Android nao instalado neste editor");

            string[][] keywords = { new string[0], new[] { "_MAIN_LIGHT_SHADOWS_CASCADE", "FOG_LINEAR" } };
            foreach (string nome in new[] { LookSetup.ShaderToon, LookSetup.ShaderCeu })
            {
                ShaderData.Subshader sub = ShaderUtil.GetShaderData(Shader.Find(nome)).GetSerializedSubshader(0);
                for (int i = 0; i < sub.PassCount; i++)
                {
                    ShaderData.Pass passe = sub.GetPass(i);
                    foreach (ShaderCompilerPlatform plataforma in new[] { ShaderCompilerPlatform.GLES3x, ShaderCompilerPlatform.Vulkan })
                    foreach (ShaderType estagio in new[] { ShaderType.Vertex, ShaderType.Fragment })
                    foreach (string[] kw in keywords)
                    {
                        ShaderData.VariantCompileInfo r = passe.CompileVariant(estagio, kw, plataforma, BuildTarget.Android);
                        Assert.IsTrue(r.Success, nome + " / " + passe.Name + " / " + plataforma + " / " + estagio + " ["
                            + string.Join(" ", kw) + "]: " + string.Join("; ", r.Messages.Select(mm => mm.message)));
                    }
                }
            }
        }

        // ---------------------------------------------------------------- normalizacao (C# puro)

        [Test]
        public void Escala_LevaAAlturaAlvo_EApoiaABaseCentradaNaOrigem()
        {
            var medido = new Bounds(new Vector3(0.3f, 1.2f, -0.1f), new Vector3(0.6f, 2f, 0.4f)); // base em y=0,2
            float s = Prototipos.Escala(medido.size, BodyScale.Crianca5, Vector2.zero);
            Assert.AreEqual(0.55f, s, 1e-5f);

            Vector3 baseCentral = new Vector3(medido.center.x, medido.min.y, medido.center.z);
            Vector3 depois = baseCentral * s + Prototipos.ApoioNaBase(medido, s);
            Assert.Less(depois.magnitude, 1e-5f, "o centro da base tem de cair no pivo (chao, centrado em XZ): " + depois);
        }

        [Test]
        public void Escala_FootprintLimita_EDegeneradaFicaEmUm()
        {
            // Casa de 20 x 3 x 10 m para 6 m de altura (x2) numa planta de 10 x 9: a largura manda (x0,5).
            Assert.AreEqual(0.5f, Prototipos.Escala(new Vector3(20f, 3f, 10f), 6f, new Vector2(10f, 9f)), 1e-5f);
            Assert.AreEqual(2f, Prototipos.Escala(new Vector3(20f, 3f, 10f), 6f, Vector2.zero), 1e-5f, "sem planta: so a altura");
            Assert.AreEqual(1f, Prototipos.Escala(new Vector3(1f, 0f, 1f), 6f, Vector2.zero), "malha sem altura nao explode");
            Assert.AreEqual(1f, Prototipos.Escala(new Vector3(1f, 2f, 1f), 0f, Vector2.zero), "alvo invalido nao zera");
        }

        [Test]
        public void Arvores_OrcamentoDeTriangulosLimitaAQuantidade()
        {
            Assert.AreEqual(10, Prototipos.QuantasArvores(6000));
            Assert.AreEqual(4, Prototipos.QuantasArvores(50000), "piso de 4 arvores, mesmo pesadas");
            Assert.AreEqual(int.MaxValue, Prototipos.QuantasArvores(0));
        }

        [Test]
        public void Tabela_TodoIdDoContratoTemAlturaECaminho()
        {
            string[] personagens = { "protagonista", "nilo", "sera", "borin", "mara" };
            string[] pecas = { "casa_familia", "ferraria", "poco", "arvore", "barril", "caixote", "cesto", "lanterna",
                               "arbusto", "simbolo_limiar", "bigorna", "banco" };
            CollectionAssert.AreEquivalent(personagens.Concat(pecas), Prototipos.Ids());
            foreach (string id in personagens)
                Assert.AreEqual(Prototipos.Raiz + "/Personagens/" + id + "/" + id + ".fbx", Prototipos.Caminho(id));
            foreach (string id in pecas)
                Assert.AreEqual(Prototipos.Raiz + "/Pecas/" + id + "/" + id + ".fbx", Prototipos.Caminho(id));

            Assert.AreEqual(BodyScale.Crianca5, Prototipos.Altura("protagonista"));
            Assert.AreEqual(BodyScale.Crianca5, Prototipos.Altura("nilo"));
            Assert.AreEqual(BodyScale.Crianca5, Prototipos.Altura("sera"));
            Assert.AreEqual(BodyScale.Adulto, Prototipos.Altura("mara"));
            Assert.AreEqual(1.82f, Prototipos.Altura("borin"));
            foreach (string id in pecas) Assert.Greater(Prototipos.Altura(id), 0f, id);
        }

        /// <summary>Peca solta nunca cai num percurso do T008 (spawn -> ancora, porta -> interior): o teste de navegacao
        /// do AurenSceneTests so enxerga a peca quando o FBX existe; este confere a tabela sempre.</summary>
        [Test]
        public void PecasSoltas_LongeDeTodoPercursoDoT008()
        {
            Prototipos.Carregar = _ => null;
            AurenSceneBuilder.Populate();   // so para ler o centro das casas (fim do trecho porta -> interior)

            var trechos = new List<(Vector3, Vector3)>();
            foreach (string id in AurenSceneBuilder.Ancoras)
            {
                if (id == "spawn_player") continue;
                Vector3[] p = AurenSceneBuilder.Percurso(id);
                for (int i = 1; i < p.Length; i++) trechos.Add((p[i - 1], p[i]));
            }
            Transform construcoes = AurenSceneBuilder.Achar(AurenSceneBuilder.RaizMundo).transform.Find("Construcoes");
            foreach (string id in AurenSceneBuilder.CasasAcessiveis)
                trechos.Add((AurenSceneBuilder.PosicaoDaAncora(id), construcoes.Find(id).position));

            foreach (Prototipos.Pose pose in Prototipos.Soltas)
                foreach (var (a, b) in trechos)
                    Assert.GreaterOrEqual(DistanciaXZ(pose.Pos, a, b), 1.5f,
                        pose.Id + " em " + pose.Pos + " perto demais do trecho " + a + " -> " + b);
        }

        // ---------------------------------------------------------------- geradores

        [Test]
        public void Auren_SemPrototipo_FicaOGreybox_NoLookToon()
        {
            Prototipos.Carregar = _ => null;
            AurenSceneBuilder.Populate();
            Transform mundo = AurenSceneBuilder.Achar(AurenSceneBuilder.RaizMundo).transform;

            foreach (GameObject raiz in SceneManager.GetActiveScene().GetRootGameObjects())
                foreach (Transform t in raiz.GetComponentsInChildren<Transform>(true))
                    Assert.IsFalse(t.name.StartsWith(Prototipos.Prefixo), "prototipo sem FBX: " + t.name);

            foreach (Renderer r in mundo.Find("Construcoes/casa_familia").GetComponentsInChildren<Renderer>())
                Assert.IsTrue(r.enabled, "greybox da casa escondido sem prototipo: " + r.name);
            Assert.IsTrue(mundo.Find("Cenario/poco").GetComponent<Renderer>().enabled);
            Assert.AreEqual(LookSetup.ShaderToon, mundo.Find("Terreno/terreno").GetComponent<Renderer>().sharedMaterial.shader.name);

            // NPC sem prototipo: capsula a mostra, toon, uma cor chapada por NPC.
            var cores = new HashSet<Color>();
            foreach (Transform npc in AurenSceneBuilder.Achar(NpcSceneSetup.RaizNpcs).transform)
            {
                Renderer r = npc.Find("Corpo").GetComponent<Renderer>();
                Assert.IsTrue(r.enabled, npc.name + ": capsula escondida sem prototipo");
                Assert.AreEqual(LookSetup.ShaderToon, r.sharedMaterial.shader.name, npc.name);
                cores.Add(r.sharedMaterial.GetColor("_BaseColor"));
            }
            Assert.AreEqual(NpcCatalog.Npcs.Length, cores.Count, "dois NPCs com a mesma cor: nao se distinguem de longe");

            // Ceu pintado, fog, ambiente em tres cores, pos ligado na camera.
            Assert.IsTrue(RenderSettings.fog);
            Assert.AreEqual(FogMode.Linear, RenderSettings.fogMode);
            Assert.AreEqual(AmbientMode.Trilight, RenderSettings.ambientMode);
            Assert.AreEqual(LookSetup.ShaderCeu, RenderSettings.skybox.shader.name);
            Camera cam = AurenSceneBuilder.Achar("Main Camera").GetComponent<Camera>();
            Assert.AreEqual(CameraClearFlags.Skybox, cam.clearFlags);
            Assert.IsNotNull(AurenSceneBuilder.Achar(LookSetup.NomeVolume).GetComponent("Volume"), "sem Volume global de pos");
        }

        [Test]
        public void Auren_ComPrototipo_TrocaOVisual_EMantemColisaoGatilhosEContratos()
        {
            falso = Falso();
            Prototipos.Carregar = _ => falso;
            AurenSceneBuilder.Populate();
            Transform mundo = AurenSceneBuilder.Achar(AurenSceneBuilder.RaizMundo).transform;

            // Casa: modelo no lugar da caixa, na altura-alvo, base no chao, dentro da planta; greybox invisivel e solido.
            Transform casa = mundo.Find("Construcoes/casa_familia");
            Transform proto = mundo.Find("Construcoes/" + Prototipos.Prefixo + "casa_familia");
            Assert.IsNotNull(proto, "sem prototipo da casa");
            Bounds b = Caixa(proto);
            Assert.AreEqual(0f, b.min.y, 0.01f, "casa fora do chao");
            Assert.LessOrEqual(b.size.y, Prototipos.Altura("casa_familia") + 0.01f);
            Assert.LessOrEqual(b.size.x, 10.01f, "casa passa da planta do greybox");
            Assert.LessOrEqual(b.size.z, 9.01f, "casa passa da planta do greybox");
            Assert.AreEqual(casa.position.x, b.center.x, 0.01f, "casa fora do centro da planta");
            foreach (Renderer r in casa.GetComponentsInChildren<Renderer>()) Assert.IsFalse(r.enabled, "greybox a mostra: " + r.name);
            Assert.GreaterOrEqual(casa.GetComponentsInChildren<Collider>().Length, 6, "a casa perdeu a colisao do percurso");

            // Poco: o interagivel e o colisor do greybox continuam.
            Transform poco = mundo.Find("Cenario/poco");
            Assert.IsNotNull(poco.GetComponent<SimpleInteractable>());
            Assert.IsNotNull(poco.GetComponent<Collider>());
            Assert.IsFalse(poco.GetComponent<Renderer>().enabled);

            // Simbolo do Limiar: modelo ao lado, gatilho do salto intacto (um so).
            Assert.IsNotNull(mundo.Find(Prototipos.Prefixo + "simbolo_limiar"));
            Assert.AreEqual(1, Object.FindObjectsByType<SaltoGatilho>(FindObjectsInactive.Include, FindObjectsSortMode.None).Length);

            // Pecas soltas: com caixa quando pede, sem quando nao.
            Transform cenario = mundo.Find("Cenario");
            Assert.IsNotNull(cenario.Find(Prototipos.Prefixo + "bigorna").GetComponent<BoxCollider>(), "bigorna sem colisor");
            Assert.IsNull(cenario.Find(Prototipos.Prefixo + "lanterna").GetComponent<Collider>(), "lanterna barrando");

            // Contratos do T008/T012 que o visual nao pode mudar.
            int interagiveis = 0;
            foreach (GameObject raiz in SceneManager.GetActiveScene().GetRootGameObjects())
                interagiveis += raiz.GetComponentsInChildren<SimpleInteractable>(true).Length;
            Assert.AreEqual(5, interagiveis, "tres portas + poco + mural");

            // NPC com prototipo: capsula desligada, modelo na altura, pes no chao, sem colisor; crianca cresce aos 8.
            Transform npcs = AurenSceneBuilder.Achar(NpcSceneSetup.RaizNpcs).transform;
            NpcActor mara = npcs.Find("mara").GetComponent<NpcActor>();
            Transform corpoMara = mara.transform.Find("Corpo");
            Assert.IsFalse(corpoMara.GetComponent<Renderer>().enabled);
            Bounds bm = Caixa(corpoMara.Find(Prototipos.Prefixo + "mara"));
            Assert.AreEqual(BodyScale.Adulto, bm.size.y, 0.01f, "mara fora da altura");
            Assert.AreEqual(mara.transform.position.y, bm.min.y, 0.01f, "mara fora do chao");
            Assert.IsEmpty(mara.GetComponentsInChildren<Collider>(), "colisor de NPC barraria os percursos");
            Assert.AreEqual(1.82f, Caixa(npcs.Find("borin/Corpo/" + Prototipos.Prefixo + "borin")).size.y, 0.01f);

            NpcActor nilo = npcs.Find("nilo").GetComponent<NpcActor>();
            nilo.AjustarCorpo(8);
            Assert.AreEqual(BodyScale.Crianca8, Caixa(nilo.transform.Find("Corpo/" + Prototipos.Prefixo + "nilo")).size.y, 0.01f,
                            "o amigo de infancia nao cresceu com o salto");
            Assert.IsTrue(npcs.Find("daren/Corpo").GetComponent<Renderer>().enabled, "daren nao esta no contrato: segue capsula");

            // Player: modelo do protagonista na altura de 5 anos, capsula escondida, e o BodyByAge o escala aos 8.
            GameObject player = AurenSceneBuilder.Achar("Player");
            Transform modelo = player.transform.Find(Prototipos.Prefixo + "protagonista");
            Assert.IsNotNull(modelo, "sem prototipo do protagonista no Player");
            Assert.AreEqual(BodyScale.Crianca5, Caixa(modelo).size.y, 0.01f);
            Assert.AreEqual(player.transform.position.y, Caixa(modelo).min.y, 0.01f, "protagonista fora do chao");
            Assert.IsFalse(player.transform.Find("Body").gameObject.activeSelf, "capsula a mostra junto do modelo");
            player.GetComponent<BodyByAge>().Aplicar(8);
            Assert.AreEqual(BodyScale.Crianca8, Caixa(modelo).size.y, 0.01f, "o protagonista nao cresceu aos 8");
        }

        // ---------------------------------------------------------------- utilidades

        /// <summary>"FBX" falso: cubo de 1 m com o centro fora do pivo (x 0,3, base em y 1,5), como um export do Tripo.
        /// Sem colisor (o import do prototipo tambem nao traz).</summary>
        static GameObject Falso()
        {
            var raiz = new GameObject("falso");
            GameObject cubo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Object.DestroyImmediate(cubo.GetComponent<Collider>());
            cubo.transform.SetParent(raiz.transform, false);
            cubo.transform.localPosition = new Vector3(0.3f, 2f, -0.2f);
            return raiz;
        }

        static Bounds Caixa(Transform t)
        {
            Assert.IsNotNull(t, "objeto de prototipo ausente");
            Renderer[] rs = t.GetComponentsInChildren<Renderer>();
            Assert.IsNotEmpty(rs, t.name + " sem Renderer");
            Bounds b = rs[0].bounds;
            foreach (Renderer r in rs) b.Encapsulate(r.bounds);
            return b;
        }

        static float DistanciaXZ(Vector3 p, Vector3 a, Vector3 b)
        {
            var p2 = new Vector2(p.x, p.z);
            var a2 = new Vector2(a.x, a.z);
            Vector2 ab = new Vector2(b.x, b.z) - a2;
            float t = ab.sqrMagnitude < 1e-6f ? 0f : Mathf.Clamp01(Vector2.Dot(p2 - a2, ab) / ab.sqrMagnitude);
            return Vector2.Distance(p2, a2 + ab * t);
        }

        static string Mensagens(Shader s)
        {
            return string.Join("; ", ShaderUtil.GetShaderMessages(s).Select(m => m.message + " (" + m.file + ":" + m.line + ")"));
        }
    }
}
