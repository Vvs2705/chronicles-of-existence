using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace COE.EditorTools
{
    /// <summary>Build Windows: aplica os settings, regera as cenas Bootstrap e Auren e gera client/Builds/win/COE.exe.
    /// Uso: tools/build_windows.ps1 (Unity -executeMethod COE.EditorTools.BuildWindows.Build).</summary>
    public static class BuildWindows
    {
        /// <summary>Cenas habilitadas no Build Settings, com a Bootstrap primeiro. Sem isso a build so leva a Bootstrap
        /// e Auren fica de fora do jogo (achado da T008).</summary>
        static string[] CenasHabilitadas()
        {
            var paths = new System.Collections.Generic.List<string>();
            foreach (EditorBuildSettingsScene s in EditorBuildSettings.scenes)
                if (s.enabled && !string.IsNullOrEmpty(s.path)) paths.Add(s.path);
            if (paths.Count == 0) paths.Add(BootstrapSceneBuilder.ScenePath);
            int i = paths.IndexOf(BootstrapSceneBuilder.ScenePath);
            if (i > 0) { paths.RemoveAt(i); paths.Insert(0, BootstrapSceneBuilder.ScenePath); }
            return paths.ToArray();
        }

        [MenuItem("COE/Build Windows")]
        public static void Build()
        {
            string client = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            string exe = Path.Combine(client, "Builds", "win", "COE.exe");
            Directory.CreateDirectory(Path.GetDirectoryName(exe));

            ProjectSetup.Apply();
            BootstrapSceneBuilder.Build();
            AurenSceneBuilder.Build(); // sem isto a Auren ia para a build com a cena velha do disco

            BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = CenasHabilitadas(),   // todas as cenas do Build Settings (Bootstrap primeiro, Auren junto)
                locationPathName = exe,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.Development,  // Development: profiler e log detalhado no PC
            });
            BuildSummary s = report.summary;
            Debug.Log(string.Format("BuildSummary(win): result={0} size={1:F1} MB time={2} errors={3} output={4}",
                s.result, s.totalSize / 1048576f, s.totalTime, s.totalErrors, s.outputPath));
            if (s.result == BuildResult.Succeeded) return;

            foreach (BuildStep step in report.steps)
                foreach (BuildStepMessage msg in step.messages)
                    if (msg.type == LogType.Error || msg.type == LogType.Exception)
                        Debug.LogError("[" + step.name + "] " + msg.content);
            if (Application.isBatchMode) EditorApplication.Exit(1);
        }
    }
}
