using System.IO;
using UnityEngine;
using UnityEngine.Profiling;

namespace COE
{
    /// <summary>Mostra FPS, frame time, memoria alocada, bateria e temperatura; SO EM BUILD DE DESENVOLVIMENTO
    /// (Debug.isDebugBuild; o editor conta) grava CSV a cada 1 s em persistentDataPath (Android:
    /// /storage/emulated/0/Android/data/&lt;pacote&gt;/files). Release nao escreve arquivo no aparelho do jogador.
    /// Colunas (ordem fixa; tools/perf_report.py confere as 8 primeiras e infere o periodo pela mediana dos deltas):
    /// t_s,fps,frame_ms,alloc_mb,battery,temp_c,device,gpu,fps_min_1s,qualidade
    /// - qualidade: nome do nivel do QualitySettings (Baixa, Media, Alta; ADR-0009), para cada medicao dizer a faixa;
    /// - fps / frame_ms: suavizados (lerp 0,1) no instante da amostra;
    /// - fps_min_1s: menor FPS instantaneo (1/unscaledDeltaTime, sem suavizacao) dentro do ultimo periodo de amostra,
    ///   para pegar engasgos de 1 frame que a suavizacao esconde.
    /// CUSTO: o medidor nao pode pesar no que mede. Tudo o que e caro (arquivo do kernel, bateria, montagem de
    /// string, CSV) roda 1x por amostra no Update; o OnGUI (2x+ por quadro) so desenha as strings prontas.
    /// ponytail: ainda sem contadores de gameplay no CSV; o controller do COE (T002) acrescenta
    /// contadores proprios DEPOIS de fps_min_1s (perf_report.py le por nome e tolera extras).</summary>
    public class PerfHud : MonoBehaviour
    {
        [SerializeField] float sampleEverySeconds = 1f;
        [SerializeField] int fontSize = 28;
        // FPS alvo (Application.targetFrameRate) NAO e daqui: e configuracao do jogador (Configuracoes, MenuDePausa).
        // Este componente so mede.
        [Tooltip("Diagnostico de input/movimento na tela. Ligados pelo gerador de cena; vazios = linha sem o dado.")]
        [SerializeField] PlayerInputReader input;
        [SerializeField] CharacterMotor motor;

        float smoothedDt;
        float minFpsWindow = float.MaxValue; // minimo instantaneo desde a ultima amostra
        float nextSample;
        string csvPath;   // null = nao grava (release)
        string hudText = string.Empty;   // montadas na amostra, so lidas no OnGUI
        string diagText = string.Empty;
        GUIStyle style;
        bool simulandoToque;
        float xTexto = Margem;

        /// <summary>Desenha o texto na tela? Nao mexe no CSV (que so existe em build de desenvolvimento). Quem
        /// liga/desliga: o menu de configuracoes.</summary>
        public bool Mostrar { get; set; } = true;

        void Start()
        {
            useGUILayout = false; // sem GUILayout aqui: pula o passe de Layout do OnGUI
            StringsLoader.EnsureLoaded();
            smoothedDt = Time.unscaledDeltaTime;
            simulandoToque = DevSceneArg.Tem("-toque");   // uma vez: no Android le a intent por JNI
            csvPath = CaminhoDoCsv(Debug.isDebugBuild, Application.persistentDataPath, SystemInfo.deviceModel, System.DateTime.Now);
            if (csvPath == null) return;
            File.WriteAllText(csvPath, "t_s,fps,frame_ms,alloc_mb,battery,temp_c,device,gpu,fps_min_1s,qualidade\n");
            Debug.Log("PerfHud: CSV em " + csvPath);
        }

        /// <summary>Arquivo de CSV desta sessao, ou null quando nao e build de desenvolvimento: ai o PerfHud so mede
        /// e desenha, sem um File.AppendAllText por segundo no aparelho de quem joga.</summary>
        public static string CaminhoDoCsv(bool buildDeDesenvolvimento, string pasta, string aparelho, System.DateTime agora)
        {
            if (!buildDeDesenvolvimento) return null;
            return Path.Combine(pasta, "perf_" + aparelho.Replace(' ', '_') + "_" + agora.ToString("yyyyMMdd_HHmmss") + ".csv");
        }

        /// <summary>Caixa que o texto ocupa (as linhas de medida e, se houver, a de diagnostico), em coordenadas do GUI
        /// (origem em cima). O OnGUI desenha nestas mesmas medidas.</summary>
        public static Rect AreaDoTexto(float x, int fonte, bool comDiagnostico)
        {
            return new Rect(x, Margem, comDiagnostico ? 900f : 700f, fonte * (comDiagnostico ? 7f : 5f));
        }

        /// <summary>x do texto: na margem, ou logo a direita dos botoes de toque que ficariam embaixo dele. Canhoto: os
        /// botoes vao para o canto inferior ESQUERDO e a coluna Esquiva/Usar sobe ate perto do topo (no modo celular do PC,
        /// 1200x540, o Usar comeca 109 px abaixo do topo e o texto ia ate 206). Destro: botoes a direita, nada cruza.
        /// safe e dpi sao os da HUD de toque (origem embaixo, como Screen.safeArea); o botao e o do ControlPreset.</summary>
        public static float XDoTexto(ControlPreset preset, Rect safe, float dpi, float alturaTela, int fonte, bool comDiagnostico)
        {
            Rect t = AreaDoTexto(Margem, fonte, comDiagnostico);
            if (preset == null) return t.x;
            for (bool mudou = true; mudou;)   // termina: x so cresce e cada botao empurra no maximo uma vez
            {
                mudou = false;
                for (int i = 0; i < preset.buttons.Length; i++)
                {
                    Vector2 c = preset.ButtonCenterPx(i, safe, dpi);
                    float r = preset.ButtonRadiusPx(i, dpi);
                    Rect botao = new Rect(c.x - r, alturaTela - c.y - r, 2f * r, 2f * r);
                    if (!botao.Overlaps(t)) continue;
                    t.x = botao.xMax + Margem;
                    mudou = true;
                }
            }
            return t.x;
        }

        // ponytail: a mesma conta de PlayerInputReader.Dpi() (privada la; outra raia): no modo celular do PC (-toque) a
        // janela vale 393 dp de altura. O touchHudDev do inspetor nao entra. Caminho: expor o dpi do leitor e ler daqui.
        float DpiDoToque() { return simulandoToque && Screen.dpi < 200f ? Screen.height * 160f / 393f : Screen.dpi; }

        void Update()
        {
            // Todo quadro (barato, sem alocar): trocar a mao no menu muda o lado na volta, sem esperar a amostra.
            xTexto = XDoTexto(input != null ? input.Preset : null, Screen.safeArea, DpiDoToque(), Screen.height, fontSize, diagText.Length > 0);

            float dt = Time.unscaledDeltaTime;
            smoothedDt = Mathf.Lerp(smoothedDt, dt, 0.1f);
            if (dt > 0f) minFpsWindow = Mathf.Min(minFpsWindow, 1f / dt);
            if (Time.unscaledTime < nextSample) return;
            nextSample = Time.unscaledTime + sampleEverySeconds;

            string temp = TempC();
            float bateria = SystemInfo.batteryLevel;
            long alloc = AllocMb();
            float minFps = minFpsWindow == float.MaxValue ? 0f : minFpsWindow;
            if (csvPath != null)
                File.AppendAllText(csvPath, string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    "{0:F1},{1:F1},{2:F2},{3},{4:F2},{5},{6},{7},{8:F1},{9}\n",
                    Time.unscaledTime, 1f / smoothedDt, smoothedDt * 1000f, alloc, bateria,
                    temp, SystemInfo.deviceModel, SystemInfo.graphicsDeviceName, minFps,
                    QualitySettings.names[QualitySettings.GetQualityLevel()]));
            hudText = Strings.Format("perf.hud", 1f / smoothedDt, smoothedDt * 1000f, alloc, bateria, SystemInfo.batteryStatus,
                temp == NotAvailable ? Strings.Get("perf.nd") : temp, minFps);

            // Diagnostico de input/movimento na propria tela: sem isso, "o boneco nao anda" nao diz SE a tecla chegou.
            // ponytail: 1 leitura por amostra (input segurado aparece; toque de 1 quadro nao). Sai quando existir HUD
            // de verdade (T013).
            diagText = input == null && motor == null ? string.Empty
                : "input " + (input != null ? input.Move.ToString("F2") : "-")
                + "  vel " + (motor != null ? motor.Ultimo.Velocidade.ToString("F2") : "-")
                + "  pos " + (motor != null ? motor.transform.position.ToString("F1") : "-");
            minFpsWindow = float.MaxValue;
        }

        void OnGUI()
        {
            if (!Mostrar || UiFundo.HaModal) return;   // a medicao (e o CSV de desenvolvimento) segue; so o desenho some
            if (style == null) style = new GUIStyle(GUI.skin.label) { fontSize = fontSize, normal = { textColor = Color.yellow } };
            GUI.Label(new Rect(xTexto, Margem, 700, fontSize * 5), hudText, style);
            if (diagText.Length > 0) GUI.Label(new Rect(xTexto, Margem + fontSize * 5, 900, fontSize * 2), diagText, style);
        }

        const float Margem = 10f;

        static long AllocMb() { return Profiler.GetTotalAllocatedMemoryLong() / (1024 * 1024); }

        const string NotAvailable = "n/d"; // token do CSV (perf_report.py trata nao-numero como ausente); a tela traduz via perf.nd

#if UNITY_ANDROID && !UNITY_EDITOR
        static bool tempNegada; // SELinux que nega a zona termica nega sempre: depois da 1a falha nem tenta de novo
#endif

        // Unity nao expoe temperatura em SystemInfo. Android: le a zona termica 0 do kernel (milligraus),
        // sem permissao especial na maioria dos aparelhos. iOS/desktop: "n/d".
        // ponytail: trocar por Adaptive Performance (ThermalStatus) quando o pacote entrar.
        static string TempC()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (!tempNegada)
            {
                try
                {
                    string raw = File.ReadAllText("/sys/class/thermal/thermal_zone0/temp").Trim();
                    float milli;
                    if (float.TryParse(raw, out milli)) return (milli > 1000f ? milli / 1000f : milli).ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch { tempNegada = true; }
            }
#endif
            return NotAvailable;
        }
    }
}
