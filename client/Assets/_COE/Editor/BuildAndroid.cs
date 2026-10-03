using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace COE.EditorTools
{
    /// <summary>Build Android de desenvolvimento (ADR-0006): aplica os settings, regera as cenas Bootstrap e Auren e gera
    /// client/Builds/android/COE.apk assinado com a chave de debug.
    /// Uso: tools/build_android.ps1 (Unity -buildTarget Android -executeMethod COE.EditorTools.BuildAndroid.Build).</summary>
    public static class BuildAndroid
    {
        [MenuItem("COE/Build Android")]
        public static void Build() { Gerar("COE.apk", BuildOptions.Development); }   // Development: profiler e stack trace no logcat

        /// <summary>APK para amigos testarem: sem modo de desenvolvimento (sem a marca "Development Build", HUD de
        /// desempenho desligado por padrao e nenhum CSV gravado no aparelho), ainda com a chave de debug: instala por
        /// arquivo, sem loja. Uso: tools/build_android.ps1 -Testadores.</summary>
        [MenuItem("COE/Build Android para testadores")]
        public static void BuildTestadores() { Gerar("COE_teste.apk", BuildOptions.None); }

        static void Gerar(string nomeApk, BuildOptions opcoes)
        {
            string client = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            string apk = Path.Combine(client, "Builds", "android", nomeApk);
            Directory.CreateDirectory(Path.GetDirectoryName(apk));

            // Pelo menu, com o editor em Windows: troca a plataforma antes (reimporta assets). Em batch o
            // build_android.ps1 ja abre com -buildTarget Android e isto nao roda.
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

            ProjectSetup.Apply();
            BootstrapSceneBuilder.Build();
            AurenSceneBuilder.Build(); // sem isto a Auren ia para a build com a cena velha do disco

            // APK para teste interno, assinado com a chave de debug da Unity. AAB e keystore de release so quando
            // houver loja (pendencia do ADR-0006); nao criar keystore aqui.
            EditorUserBuildSettings.buildAppBundle = false;
            PlayerSettings.Android.useCustomKeystore = false;

            BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = BuildWindows.CenasHabilitadas(),   // todas as cenas do Build Settings (Bootstrap primeiro, Auren junto)
                locationPathName = apk,
                target = BuildTarget.Android,
                targetGroup = BuildTargetGroup.Android,
                options = opcoes,
            });
            BuildSummary s = report.summary;
            Debug.Log(string.Format("BuildSummary(android): result={0} size={1:F1} MB time={2} errors={3} output={4}",
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
