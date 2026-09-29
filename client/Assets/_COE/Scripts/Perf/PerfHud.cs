using System.IO;
using UnityEngine;
using UnityEngine.Profiling;

namespace COE
{
    /// <summary>Mostra FPS, frame time, memoria alocada, bateria e temperatura; grava CSV a cada 1 s em
    /// persistentDataPath (Android: /storage/emulated/0/Android/data/&lt;pacote&gt;/files).
    /// Colunas (ordem fixa; tools/perf_report.py confere as 8 primeiras e infere o periodo pela mediana dos deltas):
    /// t_s,fps,frame_ms,alloc_mb,battery,temp_c,device,gpu,fps_min_1s
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
        // ponytail: 30 FPS e a meta de celular do docs/arte/PIPELINE.md §4.1 (HIPOTESE v0; aparelho minimo pendente
        // no ADR-0006). VSync off nas Quality. Teto: um numero so para todo aparelho; 60 vira opcao de menu quando o
        // aparelho minimo sustentar (fps_min_1s medido, sem aquecer).
        [SerializeField] int targetFrameRate = 30;
        [Tooltip("Diagnostico de input/movimento na tela. Ligados pelo gerador de cena; vazios = linha sem o dado.")]
        [SerializeField] PlayerInputReader input;
        [SerializeField] CharacterMotor motor;

        float smoothedDt;
        float minFpsWindow = float.MaxValue; // minimo instantaneo desde a ultima amostra
        float nextSample;
        string csvPath;
        string hudText = string.Empty;   // montadas na amostra, so lidas no OnGUI
        string diagText = string.Empty;
        GUIStyle style;

        void Start()
        {
            useGUILayout = false; // sem GUILayout aqui: pula o passe de Layout do OnGUI
            Application.targetFrameRate = targetFrameRate;
            StringsLoader.EnsureLoaded();
            smoothedDt = Time.unscaledDeltaTime;
            csvPath = Path.Combine(Application.persistentDataPath,
                "perf_" + SystemInfo.deviceModel.Replace(' ', '_') + "_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv");
            File.WriteAllText(csvPath, "t_s,fps,frame_ms,alloc_mb,battery,temp_c,device,gpu,fps_min_1s\n");
            Debug.Log("PerfHud: CSV em " + csvPath);
        }

        void Update()
        {
            float dt = Time.unscaledDeltaTime;
            smoothedDt = Mathf.Lerp(smoothedDt, dt, 0.1f);
            if (dt > 0f) minFpsWindow = Mathf.Min(minFpsWindow, 1f / dt);
            if (Time.unscaledTime < nextSample) return;
            nextSample = Time.unscaledTime + sampleEverySeconds;

            string temp = TempC();
            float bateria = SystemInfo.batteryLevel;
            long alloc = AllocMb();
            float minFps = minFpsWindow == float.MaxValue ? 0f : minFpsWindow;
            File.AppendAllText(csvPath, string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "{0:F1},{1:F1},{2:F2},{3},{4:F2},{5},{6},{7},{8:F1}\n",
                Time.unscaledTime, 1f / smoothedDt, smoothedDt * 1000f, alloc, bateria,
                temp, SystemInfo.deviceModel, SystemInfo.graphicsDeviceName, minFps));
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
            if (style == null) style = new GUIStyle(GUI.skin.label) { fontSize = fontSize, normal = { textColor = Color.yellow } };
            GUI.Label(new Rect(10, 10, 700, fontSize * 5), hudText, style);
            if (diagText.Length > 0) GUI.Label(new Rect(10, 10 + fontSize * 5, 900, fontSize * 2), diagText, style);
        }

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
