using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace COE.PlayModeTests
{
    /// <summary>Menu de pausa em mini-cena montada em codigo: canhoto salvo espelha o joystick (e o Start nao troca o
    /// armazenamento do teste pelo PlayerPrefs); abrir pausa e trava, fechar despausa e religa so o que o menu desligou;
    /// destruir aberto (troca de cena) nao deixa o jogo congelado. PlayMode porque Start, timeScale entre quadros e
    /// OnDisable precisam do ciclo de vida real. Armazenamento em memoria: o PlayerPrefs da maquina nao muda.</summary>
    public class MenuDePausaTests
    {
        readonly List<GameObject> spawned = new List<GameObject>();
        int fpsAntes;

        [SetUp]
        public void Guardar() { fpsAntes = Application.targetFrameRate; }

        [TearDown]
        public void TearDown()
        {
            foreach (GameObject go in spawned) if (go != null) Object.Destroy(go);
            spawned.Clear();
            Time.timeScale = 1f;
            Application.targetFrameRate = fpsAntes;
        }

        [UnityTest]
        public IEnumerator CanhotoSalvo_EspelhaOJoystick()
        {
            PlayerInputReader input = Novo<PlayerInputReader>("Input");   // Awake: preset destro padrao em memoria
            MenuDePausa menu = Menu(input);
            var mem = new ConfigEmMemoria();
            mem.Gravar(Configuracoes.ChaveMao, "canhoto");
            menu.Iniciar(mem);

            yield return null;   // Start roda aqui e nao pode recarregar do PlayerPrefs por cima

            Rect safe = Screen.safeArea;
            Assert.AreEqual(HandPreset.Canhoto, input.Preset.hand);
            Assert.IsTrue(input.Preset.InMoveZone(new Vector2(safe.xMax - 1f, safe.center.y), safe), "canhoto: joystick na direita");
            Assert.IsFalse(input.Preset.InMoveZone(new Vector2(safe.x + 1f, safe.center.y), safe), "e nao mais na esquerda");
            Assert.Greater(input.Preset.JoystickRestPx(safe, 160f).x, safe.center.x, "dica do joystick desenhada na direita");
        }

        [UnityTest]
        public IEnumerator Abrir_PausaETrava_Fechar_DespausaEReligaSoOQueDesligou()
        {
            PlayerInteractor livre = Novo<PlayerInteractor>("Player");
            PlayerInteractor jaTravado = Novo<PlayerInteractor>("TravadoPelaConversa");
            jaTravado.enabled = false;
            MenuDePausa menu = Menu(Novo<PlayerInputReader>("Input"), livre, jaTravado);
            menu.Iniciar(new ConfigEmMemoria());
            yield return null;

            menu.Abrir();
            Assert.IsTrue(menu.Aberto);
            Assert.AreEqual(0f, Time.timeScale, "menu aberto pausa o jogo");
            Assert.IsFalse(livre.enabled, "menu aberto: o personagem nao age");
            yield return null;
            Assert.AreEqual(0f, Time.timeScale, "segue pausado entre quadros");
            Assert.IsNotNull(menu.Vista, "menu aberto sem tela");
            UiChecagem.BotoesUsaveis(menu.Vista, "menu de pausa");   // Bloco D: >= 48 dp, na area segura, sem sobreposicao
            Assert.AreEqual(1 + 2 + 2 + 2 + 2 + 2 + 4, UiChecagem.BotoesAtivos(menu.Vista).Count,
                "voltar, mao, som, -/+, fps, desempenho e quatro de qualidade; a engrenagem some com o menu aberto");

            menu.Fechar();
            Assert.IsFalse(menu.Aberto);
            Assert.AreEqual(1f, Time.timeScale, "voltar ao jogo despausa");
            Assert.IsTrue(livre.enabled);
            Assert.IsFalse(jaTravado.enabled, "o que outra tela travou continua travado");
        }

        [UnityTest]
        public IEnumerator DestruirAberto_DevolveOTempoEOControle()
        {
            PlayerInteractor livre = Novo<PlayerInteractor>("Player");
            MenuDePausa menu = Menu(null, livre);
            menu.Iniciar(new ConfigEmMemoria());
            menu.Abrir();

            Object.Destroy(menu.gameObject);
            yield return null;

            Assert.AreEqual(1f, Time.timeScale, "troca de cena com o menu aberto nao congela o jogo");
            Assert.IsTrue(livre.enabled);
        }

        T Novo<T>(string nome) where T : Component
        {
            var go = new GameObject(nome);
            spawned.Add(go);
            return go.AddComponent<T>();
        }

        // Campos serializados privados (quem liga na cena e o gerador, via SerializedObject; aqui nao ha UnityEditor).
        MenuDePausa Menu(PlayerInputReader input, params Behaviour[] travar)
        {
            MenuDePausa menu = Novo<MenuDePausa>("MenuDePausa");
            Set(menu, "input", input);
            Set(menu, "travar", travar);
            return menu;
        }

        static void Set(object alvo, string campo, object valor)
        {
            FieldInfo f = alvo.GetType().GetField(campo, BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(f, alvo.GetType().Name + " nao tem o campo '" + campo + "'");
            f.SetValue(alvo, valor);
        }
    }
}
