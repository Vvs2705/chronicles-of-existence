using COE.EditorTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace COE.EditorTests
{
    /// <summary>T002: BootstrapSceneBuilder.Populate monta o jogador na escala da crianca de 5 anos (BodyScale.Crianca5),
    /// a camera com enquadramento de crianca e todas as dependencias ligadas por campo serializado (nada de busca
    /// global). Cada teste monta a propria cena nova em memoria; nenhum depende de Bootstrap.unity.</summary>
    public class BootstrapSceneTests
    {
        [SetUp]
        public void Abrir()
        {
            // Single: em batch mode a cena inicial e "untitled" e NewScene aditiva lanca (ver AurenSceneTests).
            Scene s = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SceneManager.SetActiveScene(s);
        }

        [TearDown]
        public void Fechar() { LogAssert.NoUnexpectedReceived(); }

        [Test]
        public void Populate_PlayerNaEscalaDaCriancaDeCincoAnos()
        {
            BootstrapSceneBuilder.Populate();
            GameObject player = Raiz("Player");

            CharacterController cc = player.GetComponent<CharacterController>();
            Assert.IsNotNull(cc, "Player sem CharacterController");
            Assert.AreEqual(BodyScale.Crianca5, cc.height, 1e-4f, "capsula fora da altura da crianca de 5 anos");
            Assert.AreEqual(0f, cc.center.y - cc.height * 0.5f, 1e-4f, "base da capsula fora dos pes (y=0)");
            Assert.Greater(cc.stepOffset, 0f);
            Assert.Less(cc.stepOffset, cc.height * 0.25f, "degrau de adulto: a crianca subiria em caixote");
            Assert.AreEqual(Vector3.one, player.transform.localScale, "a raiz do Player nao pode ser escalada (o humanoide ja vem na altura)");

            Transform body = player.transform.Find("Body");
            Assert.IsNotNull(body, "sem capsula visual");
            // Capsula primitiva: meia altura 1 x escala. Topo = centro + meia altura.
            Assert.AreEqual(BodyScale.Crianca5, body.localPosition.y + body.localScale.y, 1e-4f, "capsula visual fora da altura");
        }

        [Test]
        public void Populate_CameraEnquadraACrianca_EDependenciasLigadasPeloGerador()
        {
            BootstrapSceneBuilder.Populate();
            GameObject player = Raiz("Player");
            PlayerInputReader input = Raiz("Input").GetComponent<PlayerInputReader>();
            ThirdPersonCamera cam = Raiz("Main Camera").GetComponent<ThirdPersonCamera>();
            Assert.IsNotNull(input, "sem PlayerInputReader");
            Assert.IsNotNull(cam, "sem ThirdPersonCamera");

            var so = new SerializedObject(cam);
            float pivo = so.FindProperty("pivotOffset").vector3Value.y;
            float distancia = so.FindProperty("distance").floatValue;
            Assert.Less(pivo, BodyScale.Crianca5, "pivo da camera acima da cabeca da crianca (enquadramento de adulto)");
            Assert.Greater(pivo, BodyScale.Crianca5 * 0.5f, "pivo abaixo da cintura");
            Assert.LessOrEqual(distancia, BodyScale.Crianca5 * 3f, "camera longe demais: a crianca some na tela");

            // Sem referencia global: cada dependencia chega por campo serializado ligado pelo gerador.
            // AreEqual (nao AreSame): UnityEngine.Object compara pelo objeto nativo, nao pelo wrapper C#.
            Assert.AreEqual(player.transform, Ref(cam, "target"));
            Assert.AreEqual(input, Ref(cam, "input"));
            Assert.AreEqual(input, Ref(player.GetComponent<CharacterMotor>(), "input"));
            Assert.AreEqual(cam, Ref(player.GetComponent<CharacterMotor>(), "cam"));
            Assert.AreEqual(input, Ref(player.GetComponent<PlayerInteractor>(), "input"));
            Assert.AreEqual(input, Ref(player.GetComponent<PlayerCombat>(), "input"));
            PerfHud perf = Raiz("Perf").GetComponent<PerfHud>();
            Assert.AreEqual(input, Ref(perf, "input"), "PerfHud sem input: voltaria a procurar por quadro");
            Assert.AreEqual(player.GetComponent<CharacterMotor>(), Ref(perf, "motor"));
        }

        [Test]
        public void T011_Populate_CombateNaEscalaDaCrianca_ELigadoPeloGerador()
        {
            BootstrapSceneBuilder.Populate();
            GameObject player = Raiz("Player");
            GameObject parceiro = Raiz("ParceiroDeTreino");
            Camera cam = Raiz("Main Camera").GetComponent<Camera>();
            DamagePopup numeros = Raiz("DamagePopup").GetComponent<DamagePopup>();
            Assert.IsNotNull(numeros, "sem DamagePopup na cena");

            // Golpe da crianca de 8 anos no meio do tronco, nao no ombro de adulto (0,9 m fixo).
            float golpe = player.GetComponent<Hitbox>().altura;
            Assert.AreEqual(BootstrapSceneBuilder.AlturaDoGolpe, golpe, 1e-4f);
            Assert.Greater(golpe, BodyScale.Crianca8 * 0.4f, "golpe abaixo da cintura da crianca");
            Assert.Less(golpe, BodyScale.Crianca8 * 0.7f, "golpe acima do peito da crianca");

            // Instrutor ADULTO (capsula primitiva: meia altura 1 x escala, pivo no centro) com o bastao no tronco da crianca.
            Transform t = parceiro.transform;
            Assert.AreEqual(BodyScale.Adulto, t.position.y + t.localScale.y, 1e-4f, "parceiro fora da altura de adulto");
            Assert.AreEqual(0f, t.position.y - t.localScale.y, 1e-4f, "pes do parceiro fora do chao");
            Assert.AreEqual(BootstrapSceneBuilder.AlturaDoGolpe, t.position.y + parceiro.GetComponent<Hitbox>().altura, 1e-4f,
                            "o bastao do instrutor tem de mirar o tronco da crianca");

            // Sem busca global em runtime (FindAnyObjectByType, Camera.main): tudo por campo serializado.
            Assert.AreEqual(player.transform, Ref(parceiro.GetComponent<TrainingDummy>(), "alvo"));
            Assert.AreEqual(cam, Ref(numeros, "cam"));
            Assert.AreEqual(numeros, Ref(player.GetComponent<HitFlash>(), "numeros"));
            Assert.AreEqual(numeros, Ref(parceiro.GetComponent<HitFlash>(), "numeros"));
        }

        [Test]
        public void ADR0006_Populate_LigaOPresetDeToqueNoInput()
        {
            BootstrapSceneBuilder.Populate();
            PlayerInputReader input = Raiz("Input").GetComponent<PlayerInputReader>();

            // O asset versionado tem de carregar como ControlPreset: se o GUID do script (ControlPreset.cs.meta) ou o
            // YAML quebrarem, isto vem nulo e o gerador cairia calado no padrao em memoria.
            var asset = AssetDatabase.LoadAssetAtPath<ControlPreset>(BootstrapSceneBuilder.PresetPath);
            Assert.IsNotNull(asset, BootstrapSceneBuilder.PresetPath + " nao carrega como ControlPreset");
            Assert.AreEqual(asset, Ref(input, "preset"), "o gerador liga o asset por campo serializado");
            Assert.AreEqual(HandPreset.Destro, asset.hand);
            foreach (TouchAction a in System.Enum.GetValues(typeof(TouchAction)))
                Assert.IsTrue(System.Array.Exists(asset.buttons, b => b.action == a), "asset sem botao de " + a);
        }

        static Object Ref(Object alvo, string campo)
        {
            Assert.IsNotNull(alvo, "componente ausente para ler '" + campo + "'");
            SerializedProperty p = new SerializedObject(alvo).FindProperty(campo);
            Assert.IsNotNull(p, alvo.GetType().Name + " nao tem o campo '" + campo + "'");
            return p.objectReferenceValue;
        }

        static GameObject Raiz(string nome)
        {
            foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
                if (go.name == nome) return go;
            Assert.Fail("a cena nao tem o objeto de raiz '" + nome + "'");
            return null;
        }
    }
}
