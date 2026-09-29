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
    /// ponytail: ainda sem contadores de gameplay no CSV; o controller do COE (T002) acrescenta
    /// contadores proprios DEPOIS de fps_min_1s (perf_report.py le por nome e tolera extras).</summary>
    public class PerfHud : MonoBehaviour
    {
        [SerializeField] float sampleEverySeconds = 1f;
        [SerializeField] int fontSize = 28;
        [SerializeField] int targetFrameRate = 60; // COE e PC; VSync off nas Quality

        float smoothedDt;
        float minFpsWindow = float.MaxValue; // minimo instantaneo desde a ultima amostra
        float nextSample;
        string csvPath;
        GUIStyle style;

        void Start()
        {
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
            File.AppendAllText(csvPath, string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "{0:F1},{1:F1},{2:F2},{3},{4:F2},{5},{6},{7},{8:F1}\n",
                Time.unscaledTime, 1f / smoothedDt, smoothedDt * 1000f, AllocMb(), SystemInfo.batteryLevel,
                TempC(), SystemInfo.deviceModel, SystemInfo.graphicsDeviceName, minFpsWindow));
            minFpsWindow = float.MaxValue;
        }

        void OnGUI()
        {
            if (style == null) style = new GUIStyle(GUI.skin.label) { fontSize = fontSize, normal = { textColor = Color.yellow } };
            string temp = TempC();
            GUI.Label(new Rect(10, 10, 700, fontSize * 5),
                Strings.Format("perf.hud", 1f / smoothedDt, smoothedDt * 1000f, AllocMb(), SystemInfo.batteryLevel, SystemInfo.batteryStatus,
                    temp == NotAvailable ? Strings.Get("perf.nd") : temp, minFpsWindow == float.MaxValue ? 0f : minFpsWindow),
                style);

            // Diagnostico de input/movimento na propria tela: sem isso, "o boneco nao anda" nao diz SE a tecla chegou.
            // ponytail: sai quando existir HUD de verdade (T013).
            PlayerInputReader inp = FindAnyObjectByType<PlayerInputReader>();
            CharacterMotor mot = FindAnyObjectByType<CharacterMotor>();
            if (inp != null || mot != null)
            {
                string txt = "input " + (inp != null ? inp.Move.ToString("F2") : "-")
                           + "  vel " + (mot != null ? mot.Ultimo.Velocidade.ToString("F2") : "-")
                           + "  pos " + (mot != null ? mot.transform.position.ToString("F1") : "-");
                GUI.Label(new Rect(10, 10 + fontSize * 5, 900, fontSize * 2), txt, style);
            }
        }

        static long AllocMb() { return Profiler.GetTotalAllocatedMemoryLong() / (1024 * 1024); }

        const string NotAvailable = "n/d"; // token do CSV (perf_report.py trata nao-numero como ausente); a tela traduz via perf.nd

        // Unity nao expoe temperatura em SystemInfo. Android: le a zona termica 0 do kernel (milligraus),
        // sem permissao especial na maioria dos aparelhos. iOS/desktop: "n/d".
        // ponytail: trocar por Adaptive Performance (ThermalStatus) quando o pacote entrar.
        static string TempC()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                string raw = File.ReadAllText("/sys/class/thermal/thermal_zone0/temp").Trim();
                float milli;
                if (float.TryParse(raw, out milli)) return (milli > 1000f ? milli / 1000f : milli).ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch { }
#endif
            return NotAvailable;
        }
    }
}
