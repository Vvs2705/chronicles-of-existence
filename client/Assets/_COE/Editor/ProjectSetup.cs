using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace COE.EditorTools
{
    /// <summary>Settings do projeto por script (URP, Quality, Player). Idempotente. Alvo do COE: Android (ADR-0006);
    /// o Player de Windows continua configurado so como ferramenta de desenvolvimento.
    /// Active Input Handling nao tem API publica em PlayerSettings: escrevemos direto no ProjectSettings.asset
    /// e o valor so vale na proxima abertura do editor.</summary>
    public static class ProjectSetup
    {
        public const string SettingsDir = "Assets/_COE/Settings";
        const string UrpPath = SettingsDir + "/URP_Base.asset";
        const string RendererPath = SettingsDir + "/URP_Base_Renderer.asset";

        // ponytail: identificador provisorio, decisao pendente do idealizador. Depois de publicado na loja nao muda nunca
        // (outro identificador = outro app); ate la trocar aqui basta. Vale para Android e Windows, para os dois baterem.
        public const string AppId = "br.com.vstack.coe"; // o valor que ja estava no ProjectSettings

        [MenuItem("COE/Aplicar settings do projeto")]
        public static void Apply()
        {
            Directory.CreateDirectory(SettingsDir);

            UniversalRenderPipelineAsset urp = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(UrpPath);
            if (urp == null)
            {
                urp = UniversalRenderPipelineAsset.Create();
                AssetDatabase.CreateAsset(urp, UrpPath);
                // ponytail: LoadBuiltinRendererData e a unica API publica que cria o renderer data com PostProcessData;
                // ela grava em Assets/UniversalRenderer.asset, entao movemos para Settings/.
                urp.LoadBuiltinRendererData();
                AssetDatabase.SaveAssets();
                AssetDatabase.MoveAsset("Assets/UniversalRenderer.asset", RendererPath);
            }
            GraphicsSettings.defaultRenderPipeline = urp;
            int current = QualitySettings.GetQualityLevel();
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                QualitySettings.SetQualityLevel(i, false);
                QualitySettings.renderPipeline = urp;
                QualitySettings.vSyncCount = 0; // teto de FPS vem de Application.targetFrameRate no PerfHud
            }
            QualitySettings.SetQualityLevel(current, false);

            PlayerSettings.companyName = "V-STACK";
            PlayerSettings.productName = "Chronicles of Existence";
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Standalone, AppId);
            AplicarAndroid();
            PlayerSettings.runInBackground = true; // sem isso a build pausa ao perder o foco e a captura automatizada fotografa um jogo congelado
            // O Input System desabilita teclado/mouse quando o app perde o foco, mesmo com runInBackground:
            // sem IgnoreFocus, a rodada automatizada (client/tools/run_windows.ps1 -Drive) le input zerado.
            var inputSettings = UnityEngine.InputSystem.InputSystem.settings;
            if (inputSettings != null)
            {
                inputSettings.backgroundBehavior = UnityEngine.InputSystem.InputSettings.BackgroundBehavior.IgnoreFocus;
                EditorUtility.SetDirty(inputSettings);
                AssetDatabase.SaveAssets();
            } // sem isso a build pausa ao perder o foco e a captura automatizada fotografa um jogo congelado

            // 2 = Both (Input System + legado). So vale na proxima abertura do editor.
            var ps = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);
            SerializedProperty handler = ps.FindProperty("activeInputHandler");
            if (handler != null && handler.intValue != 2) { handler.intValue = 2; ps.ApplyModifiedPropertiesWithoutUndo(); }

            AssetDatabase.SaveAssets();
            Debug.Log("ProjectSetup: URP=" + UrpPath + ", alvo Android (" + AppId + ", ARM64, IL2CPP, API 26+), Windows 64 de dev, Input System (Both).");
        }

        /// <summary>Player de Android (ADR-0006). URP e Quality ficam como estao: URP_Base ja e afinado para celular.</summary>
        static void AplicarAndroid()
        {
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, AppId);

            // Paisagem sempre. A Unity nao tem "padrao LandscapeLeft + autorotacao": orientacao fixa desliga a rotacao.
            // AutoRotation so com as duas paisagens abre deitado (LandscapeLeft e a paisagem natural do Android)
            // e vira 180 graus quando o jogador gira o aparelho. Retrato nunca. HIPOTESE do ADR-0006, validar no aparelho.
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;

            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64; // a Play Store exige 64 bits; ARMv7 so dobraria o build
            // ponytail: API minima 26 (Android 8.0) e HIPOTESE ate existir aparelho minimo de referencia (pendencia do ADR-0006);
            // quando o idealizador fixar o aparelho, ajustar aqui (o enum da Unity 6000.3 comeca em 25).
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel26;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto; // maior API instalada no SDK

            // Vulkan primeiro; OpenGLES3 de reserva para GPU com driver Vulkan ruim ou ausente.
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
            PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { GraphicsDeviceType.Vulkan, GraphicsDeviceType.OpenGLES3 });
        }
    }
}
