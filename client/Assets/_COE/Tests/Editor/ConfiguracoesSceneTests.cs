using COE.EditorTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace COE.EditorTests
{
    /// <summary>ConfiguracoesSceneSetup (chamado pelo BootstrapSceneBuilder.Populate) liga o menu de pausa por campo
    /// serializado: input, PerfHud e o que trava (motor, combate, interacao, camera). Aplicar "canhoto" espelha o
    /// layout numa COPIA do preset: o preset da cena (asset de definicao) nao muda. Iniciar chamado direto (Start nao
    /// roda em teste de Editor), com armazenamento em memoria. A pausa (Time.timeScale) e testada no PlayMode.</summary>
    public class ConfiguracoesSceneTests
    {
        int fpsAntes;

        [SetUp]
        public void Abrir()
        {
            // Single: em batch mode a cena inicial e "untitled" e NewScene aditiva lanca (ver AurenSceneTests).
            Scene s = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SceneManager.SetActiveScene(s);
            fpsAntes = Application.targetFrameRate;
        }

        [TearDown]
        public void Fechar() { Application.targetFrameRate = fpsAntes; }

        [Test]
        public void Populate_LigaMenuNoInputENoDesempenho_ETravaMotorCombateInteracaoECamera()
        {
            BootstrapSceneBuilder.Populate();
            MenuDePausa menu = Raiz(ConfiguracoesSceneSetup.NomeMenu).GetComponent<MenuDePausa>();
            Assert.IsNotNull(menu, "sem MenuDePausa");
            Assert.AreEqual(Raiz("Input").GetComponent<PlayerInputReader>(), Ref(menu, "input"));
            Assert.AreEqual(Raiz("Perf").GetComponent<PerfHud>(), Ref(menu, "desempenho"));

            GameObject player = Raiz("Player");
            SerializedProperty travar = new SerializedObject(menu).FindProperty("travar");
            var ligados = new Object[travar.arraySize];
            for (int i = 0; i < ligados.Length; i++) ligados[i] = travar.GetArrayElementAtIndex(i).objectReferenceValue;
            CollectionAssert.AllItemsAreNotNull(ligados);
            CollectionAssert.AreEquivalent(new Object[]
            {
                player.GetComponent<CharacterMotor>(), player.GetComponent<PlayerCombat>(), player.GetComponent<PlayerInteractor>(),
                Raiz("Main Camera").GetComponent<ThirdPersonCamera>(),
            }, ligados);
        }

        [Test]
        public void Iniciar_AplicaOSalvo_CanhotoNumaCopia_SensibilidadeFpsEDesempenho()
        {
            BootstrapSceneBuilder.Populate();
            PlayerInputReader input = Raiz("Input").GetComponent<PlayerInputReader>();
            ControlPreset daCena = input.Preset;
            var mem = new ConfigEmMemoria();
            mem.Gravar(Configuracoes.ChaveMao, "canhoto");
            mem.Gravar(Configuracoes.ChaveSensibilidade, "1.5");
            mem.Gravar(Configuracoes.ChaveFps, "60");
            mem.Gravar(Configuracoes.ChaveDesempenho, "0");

            Raiz(ConfiguracoesSceneSetup.NomeMenu).GetComponent<MenuDePausa>().Iniciar(mem);
            try
            {
                Assert.AreNotSame(daCena, input.Preset, "o menu troca o preset por uma copia de runtime");
                Assert.AreEqual(HandPreset.Destro, daCena.hand, "o preset da cena e definicao: nao vira canhoto");
                Assert.AreEqual(HandPreset.Canhoto, input.Preset.hand);
                var tela = new Rect(0f, 0f, 1280f, 720f);
                Assert.IsTrue(input.Preset.InMoveZone(new Vector2(1200f, 200f), tela), "canhoto: joystick na direita");
                Assert.Less(input.Preset.ButtonCenterPx(0, tela, 160f).x, 640f, "canhoto: botoes na esquerda");
                Assert.AreEqual(1.5f, input.LookMultiplier, 1e-5f);
                Assert.AreEqual(60, Application.targetFrameRate);
                Assert.IsFalse(Raiz("Perf").GetComponent<PerfHud>().Mostrar, "desempenho desligado so esconde o texto");
            }
            finally
            {
                if (input.Preset != daCena) Object.DestroyImmediate(input.Preset);   // copia em memoria; nunca o asset
            }
        }

        static Object Ref(Object alvo, string campo)
        {
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
