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

        /// <summary>Codigo de saida do batch quando falta a chave de upload: o build_android_release.ps1 traduz em BLOCKED_CREDENTIAL.</summary>
        public const int SaidaSemChave = 3;

        /// <summary>Target da loja (Play: API 36 a partir de 01/11/2026, docs/PROJETO.md §6). Fixo no release; o dev segue "Auto".</summary>
        public const int TargetApiRelease = 36;

        /// <summary>RELEASE (Bloco F): AAB, IL2CPP, ARM64, nao-Development, versionName/versionCode da linha de comando
        /// (-coeVersao, -coeCodigo), API minima 26 e alvo TargetApiRelease. Chave de upload SO por variavel de ambiente
        /// (COE_KEYSTORE, COE_KEYSTORE_PASS, COE_KEY_ALIAS, COE_KEY_PASS); nada disso vai para o ProjectSettings nem para o git.
        /// Sem chave: sai com SaidaSemChave, sem abrir build. -coeDebugSign valida o caminho com a chave de debug da Unity
        /// (AAB nao publicavel, nome *_debugsign). Nao publica nada. Uso: tools/build_android_release.ps1.</summary>
        public static void BuildRelease()
        {
            string versao = Arg("-coeVersao") ?? "0.1.0";
            int codigo;
            if (!int.TryParse(Arg("-coeCodigo"), out codigo) || codigo < 1) codigo = 1;
            bool debugSign = System.Array.IndexOf(System.Environment.GetCommandLineArgs(), "-coeDebugSign") >= 0;
            string ks = Env("COE_KEYSTORE"), ksPass = Env("COE_KEYSTORE_PASS"), alias = Env("COE_KEY_ALIAS"), aliasPass = Env("COE_KEY_PASS");
            bool temChave = ks != null && File.Exists(ks) && ksPass != null && alias != null && aliasPass != null;
            if (!temChave && !debugSign)
            {
                Debug.LogError("BLOCKED_CREDENTIAL: chave de upload ausente. Defina COE_KEYSTORE (arquivo existente), COE_KEYSTORE_PASS, "
                    + "COE_KEY_ALIAS e COE_KEY_PASS no ambiente, ou rode com -coeDebugSign para validar o caminho (AAB nao publicavel).");
                if (Application.isBatchMode) EditorApplication.Exit(SaidaSemChave);
                return;
            }

            string client = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            string aab = Path.Combine(client, "Builds", "android", "COE_" + versao + "_" + codigo + (temChave ? "" : "_debugsign") + ".aab");
            Directory.CreateDirectory(Path.GetDirectoryName(aab));
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);

            ProjectSetup.Apply();   // IL2CPP, ARM64, API minima 26, paisagem, Vulkan + GLES3
            BootstrapSceneBuilder.Build();
            AurenSceneBuilder.Build();

            // Valores so desta build: restaurados no fim para o ProjectSettings.asset nao mudar no git (nem levar caminho local).
            string versaoAntes = PlayerSettings.bundleVersion;
            int codigoAntes = PlayerSettings.Android.bundleVersionCode;
            AndroidSdkVersions targetAntes = PlayerSettings.Android.targetSdkVersion;
            bool aabAntes = EditorUserBuildSettings.buildAppBundle;
            bool ok = false;
            try
            {
                PlayerSettings.bundleVersion = versao;
                PlayerSettings.Android.bundleVersionCode = codigo;
                PlayerSettings.Android.targetSdkVersion = (AndroidSdkVersions)TargetApiRelease;
                EditorUserBuildSettings.buildAppBundle = true;
                PlayerSettings.Android.useCustomKeystore = temChave;
                if (temChave)
                {
                    PlayerSettings.Android.keystoreName = ks;
                    PlayerSettings.Android.keystorePass = ksPass;
                    PlayerSettings.Android.keyaliasName = alias;
                    PlayerSettings.Android.keyaliasPass = aliasPass;
                }

                BuildReport report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
                {
                    scenes = BuildWindows.CenasHabilitadas(),
                    locationPathName = aab,
                    target = BuildTarget.Android,
                    targetGroup = BuildTargetGroup.Android,
                    options = BuildOptions.None,   // release: sem Development, sem profiler, sem CSV no aparelho
                });
                BuildSummary s = report.summary;
                Debug.Log(string.Format("BuildSummary(android-release): result={0} size={1:F1} MB time={2} errors={3} output={4} versao={5} codigo={6} assinatura={7}",
                    s.result, s.totalSize / 1048576f, s.totalTime, s.totalErrors, s.outputPath, versao, codigo, temChave ? "upload" : "debug"));
                ok = s.result == BuildResult.Succeeded;
            }
            finally
            {
                PlayerSettings.bundleVersion = versaoAntes;
                PlayerSettings.Android.bundleVersionCode = codigoAntes;
                PlayerSettings.Android.targetSdkVersion = targetAntes;
                EditorUserBuildSettings.buildAppBundle = aabAntes;
                PlayerSettings.Android.useCustomKeystore = false;
                PlayerSettings.Android.keystoreName = "";
                PlayerSettings.Android.keystorePass = "";
                PlayerSettings.Android.keyaliasName = "";
                PlayerSettings.Android.keyaliasPass = "";
            }
            if (!ok && Application.isBatchMode) EditorApplication.Exit(1);   // depois do finally: nada de chave no ProjectSettings
        }

        static string Arg(string nome)
        {
            string[] a = System.Environment.GetCommandLineArgs();
            int i = System.Array.IndexOf(a, nome);
            return i >= 0 && i + 1 < a.Length ? a[i + 1] : null;
        }

        static string Env(string nome)
        {
            string v = System.Environment.GetEnvironmentVariable(nome);
            return string.IsNullOrEmpty(v) ? null : v;
        }

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

            // APK para teste interno, assinado com a chave de debug da Unity. Release (AAB, chave de upload): BuildRelease.
            // Nao criar keystore aqui.
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
