using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace COE.EditorTools
{
    /// <summary>Settings do projeto por script (URP, Quality, Player). Idempotente. Alvo do COE: Windows/PC.
    /// Active Input Handling nao tem API publica em PlayerSettings: escrevemos direto no ProjectSettings.asset
    /// e o valor so vale na proxima abertura do editor.</summary>
    public static class ProjectSetup
    {
        public const string SettingsDir = "Assets/_COE/Settings";
        const string UrpPath = SettingsDir + "/URP_Base.asset";
        const string RendererPath = SettingsDir + "/URP_Base_Renderer.asset";

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
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Standalone, "br.com.vstack.coe");
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
            Debug.Log("ProjectSetup: URP=" + UrpPath + ", alvo Windows 64, Input System (Both).");
        }
    }
}
