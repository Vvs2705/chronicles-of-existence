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

        /// <summary>ADR-0009: as tres faixas, na ordem de FaixaQualidade (o indice e o nivel do QualitySettings). A Alta e o
        /// URP_Base; Baixa e Media sao copias dele (mesmo renderer) com render menor e sombra curta ou nenhuma.
        /// Numeros [PROPOSTA], a medir no aparelho simples.
        /// MipTextura 1 = texturas pela metade (metade da memoria de textura: o que mais pesa em aparelho de 2-3 GB).
        /// LodBias: as transicoes da PIPELINE §4 supoem 1; a Baixa troca de LOD mais cedo, a Alta mais tarde.</summary>
        public static readonly (string Nome, string Asset, float Escala, float Sombra, int MapaSombra, bool Hdr, SkinWeights Pele, int MipTextura, float LodBias)[] Faixas =
        {
            ("Baixa", SettingsDir + "/URP_Baixa.asset", 0.7f,  0f,  512,  false, SkinWeights.TwoBones,  1, 0.7f),
            ("Media", SettingsDir + "/URP_Media.asset", 0.85f, 30f, 1024, true,  SkinWeights.FourBones, 0, 1f),
            ("Alta",  UrpPath,                          1f,    50f, 2048, true,  SkinWeights.FourBones, 0, 1.5f),
        };

        static void AplicarFaixas(UniversalRenderPipelineAsset alta)
        {
            var qs = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/QualitySettings.asset")[0]);
            SerializedProperty niveis = qs.FindProperty("m_QualitySettings");
            niveis.arraySize = Faixas.Length;
            for (int i = 0; i < Faixas.Length; i++)
            {
                var f = Faixas[i];
                if (AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(f.Asset) == null) AssetDatabase.CopyAsset(UrpPath, f.Asset);
                var urp = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(f.Asset);
                var u = new SerializedObject(urp);
                u.FindProperty("m_RenderScale").floatValue = f.Escala;
                u.FindProperty("m_MainLightShadowsSupported").boolValue = f.Sombra > 0f;
                u.FindProperty("m_ShadowDistance").floatValue = f.Sombra;
                u.FindProperty("m_MainLightShadowmapResolution").intValue = f.MapaSombra;
                u.FindProperty("m_SupportsHDR").boolValue = f.Hdr;
                if (u.ApplyModifiedPropertiesWithoutUndo()) EditorUtility.SetDirty(urp);

                SerializedProperty n = niveis.GetArrayElementAtIndex(i);
                n.FindPropertyRelative("name").stringValue = f.Nome;
                n.FindPropertyRelative("customRenderPipeline").objectReferenceValue = urp;
                n.FindPropertyRelative("vSyncCount").intValue = 0;   // teto de FPS: Application.targetFrameRate (MenuDePausa)
                // O URP reescreve o antiAliasing a cada render (UniversalRenderPipeline.Render): MSAA 1x = desligado = 0.
                n.FindPropertyRelative("antiAliasing").intValue = urp.msaaSampleCount > 1 ? urp.msaaSampleCount : 0;
                n.FindPropertyRelative("skinWeights").intValue = (int)f.Pele;
                n.FindPropertyRelative("globalTextureMipmapLimit").intValue = f.MipTextura;
                n.FindPropertyRelative("lodBias").floatValue = f.LodBias;
            }
            // Antes da deteccao (MenuDePausa.Start) vale a Media; no editor a Alta (o editor nao troca de nivel).
            qs.FindProperty("m_CurrentQuality").intValue = (int)FaixaQualidade.Alta;
            SerializedProperty porPlataforma = qs.FindProperty("m_PerPlatformDefaultQuality");
            for (int i = 0; i < porPlataforma.arraySize; i++)
                porPlataforma.GetArrayElementAtIndex(i).FindPropertyRelative("second").intValue = (int)FaixaQualidade.Media;
            qs.ApplyModifiedPropertiesWithoutUndo();
        }

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
            // O URP reescreve estes dois a cada render (UniversalRenderPipeline.Render); salvos diferentes, toda rodada com
            // graficos deixava ProjectSettings modificado no git. MSAA e do asset do URP: 1x = desligado = 0 no Quality.
            GraphicsSettings.lightsUseColorTemperature = true;
            AplicarFaixas(urp);

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

            // Voltar: mirando API 36 o Android 16 nao despacha mais KEYCODE_BACK nem chama onBackPressed. Ligado, o Unity
            // registra o OnBackInvokedCallback (enableOnBackInvokedCallback no manifest) e entrega o voltar ao script
            // como Esc (VoltarHud). Ja vinha ligado (padrao de projeto novo no Unity 6.2+); aqui fica fixo.
            PlayerSettings.Android.predictiveBackSupport = true;

            // Vulkan primeiro; OpenGLES3 de reserva para GPU com driver Vulkan ruim ou ausente.
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
            PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { GraphicsDeviceType.Vulkan, GraphicsDeviceType.OpenGLES3 });
        }
    }
}
