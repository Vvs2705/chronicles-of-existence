using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace COE.EditorTools
{
    /// <summary>ADR-0008 — look do prototipo de estetica (anime estilizado): materiais toon (Art/Look/COE_Toon.shader),
    /// ceu pintado, fog quente de fim de tarde, sol quente, ambiente trilight e um pos leve. Chamado pelos geradores de
    /// cena; nada aqui e editado a mao na cena.
    /// persistir = true (Build) grava materiais e perfil de volume como asset; false (teste) deixa tudo em memoria.
    /// Orcamento de celular (docs/arte/PIPELINE.md §4.1): sem SSAO, sem bloom, sombra dura da luz principal, um passe de
    /// pos (ajuste de cor + vinheta) e far plane no fim da fog.</summary>
    public static class LookSetup
    {
        public const string ShaderToon = "COE/Toon";
        public const string ShaderCeu = "COE/Ceu";
        /// <summary>LightMode do passe de contorno (casco invertido) do COE/Toon.</summary>
        public const string PasseContorno = "SRPDefaultUnlit";
        public const string MatDir = "Assets/_COE/Materials";
        public const string CeuPath = MatDir + "/COE_Ceu.mat";
        public const string VolumePath = "Assets/_COE/Settings/COE_Auren_Volume.asset";
        /// <summary>Raiz do Volume global de pos em Auren.</summary>
        public const string NomeVolume = "Look";

        /// <summary>Materiais sem contorno: chao, rua e paredes grandes. O casco invertido de uma caixa rasa vira risco
        /// preto na borda da rua e custa um draw a mais por peca, sem ganho de leitura.</summary>
        static readonly string[] semContorno = { "COE_Floor", "COE_Auren_Grama", "COE_Auren_Terra", "COE_Auren_Pedra" };

        // Cores de sol, ambiente, fog e ceu: LuzDoDia.Paleta (runtime), uma por periodo. A cena nasce na TARDE.
        public const float FogInicio = 30f;
        public const float FogFim = 150f;   // o fundo da vila (bosque, z=90) fica a ~180 m do portao: some na fog

        /// <summary>Material toon novo em memoria (testes e cena sem asset).</summary>
        public static Material NovoMaterial(string nome, Color cor)
        {
            var m = new Material(Toon()) { name = nome };
            Ajustar(m, cor);
            return m;
        }

        /// <summary>Material toon como asset em <see cref="MatDir"/>. Idempotente: o asset que ja existe (inclusive os
        /// URP/Lit do greybox) troca de shader e recebe a cor da paleta do codigo a cada geracao.</summary>
        public static Material MaterialAsset(string nome, Color cor)
        {
            string path = MatDir + "/" + nome + ".mat";
            Material m = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (m == null)
            {
                m = NovoMaterial(nome, cor);
                AssetDatabase.CreateAsset(m, path);
                return m;
            }
            Ajustar(m, cor);
            EditorUtility.SetDirty(m);
            return m;
        }

        static void Ajustar(Material m, Color cor)
        {
            Shader toon = Toon();
            if (m.shader != toon) m.shader = toon;
            m.SetColor("_BaseColor", cor);
            m.SetShaderPassEnabled(PasseContorno, Array.IndexOf(semContorno, m.name) < 0);
        }

        static Shader Toon()
        {
            Shader s = Shader.Find(ShaderToon);
            if (s == null) throw new Exception("Shader " + ShaderToon + " nao encontrado (Assets/_COE/Art/Look/COE_Toon.shader).");
            return s;
        }

        /// <summary>Ceu, fog, sol, ambiente, camera e pos da cena de Auren (cena ATIVA, depois do chassi do Bootstrap).</summary>
        public static void AplicarAuren(bool persistir)
        {
            Light sol = AurenSceneBuilder.Achar("Directional Light").GetComponent<Light>();
            sol.shadows = LightShadows.Hard;   // o URP_Base ja nao tem sombra suave; aqui fica explicito
            RenderSettings.sun = sol;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogStartDistance = FogInicio;
            RenderSettings.fogEndDistance = FogFim;

            // Tarde (intensidade 1,0: 1,15 com sol laranja estourava a terra batida, captura de 2026-09-30; sol baixo por
            // tras da camera no spawn, rosto aceso). Em jogo, LuzDoDia troca pela paleta do periodo do save.
            Material ceu = Ceu(persistir);
            RenderSettings.skybox = ceu;
            LuzDoDia.Aplicar(LuzDoDia.Paleta(TimeOfDay.Tarde), sol, ceu);
            if (persistir) EditorUtility.SetDirty(ceu);
            var luz = new SerializedObject(sol.gameObject.AddComponent<LuzDoDia>());
            luz.FindProperty("sol").objectReferenceValue = sol;
            luz.FindProperty("ceu").objectReferenceValue = ceu;
            luz.ApplyModifiedPropertiesWithoutUndo();

            Camera cam = AurenSceneBuilder.Achar("Main Camera").GetComponent<Camera>();
            cam.clearFlags = CameraClearFlags.Skybox;
            cam.farClipPlane = FogFim + 10f;   // alem da fog tudo ja e cor de ceu: nao desenha
            UniversalAdditionalCameraData dados = cam.GetComponent<UniversalAdditionalCameraData>();
            if (dados != null) dados.renderPostProcessing = true;   // URP nasce com pos desligado na camera

            var volume = new GameObject(NomeVolume).AddComponent<Volume>();
            volume.isGlobal = true;
            volume.sharedProfile = Perfil(persistir);

            // ADR-0009: a faixa Baixa desliga o pos; o menu de configuracoes (que aplica a faixa) recebe o volume.
            MenuDePausa menu = UnityEngine.Object.FindFirstObjectByType<MenuDePausa>();
            if (menu != null)
            {
                var so = new SerializedObject(menu);
                so.FindProperty("posProcessamento").objectReferenceValue = volume;
                so.ApplyModifiedPropertiesWithoutUndo();
            }
        }

        static Material Ceu(bool persistir)
        {
            Material m = persistir ? AssetDatabase.LoadAssetAtPath<Material>(CeuPath) : null;
            bool novo = m == null;
            if (novo)
            {
                Shader s = Shader.Find(ShaderCeu);
                if (s == null) throw new Exception("Shader " + ShaderCeu + " nao encontrado (Assets/_COE/Art/Look/COE_Ceu.shader).");
                m = new Material(s) { name = "COE_Ceu" };
            }
            if (persistir && novo) AssetDatabase.CreateAsset(m, CeuPath);
            return m;
        }

        /// <summary>Pos leve: um passe (ajuste de cor + vinheta), sem tonemapping (a paleta chapada fica como foi pintada)
        /// e sem bloom (custo de celular). ponytail: valores de olho no PC; calibrar no aparelho com o PerfHud.</summary>
        static VolumeProfile Perfil(bool persistir)
        {
            VolumeProfile p = persistir ? AssetDatabase.LoadAssetAtPath<VolumeProfile>(VolumePath) : null;
            if (p == null)
            {
                p = ScriptableObject.CreateInstance<VolumeProfile>();
                if (persistir) AssetDatabase.CreateAsset(p, VolumePath);
            }

            Componente<Tonemapping>(p, persistir).mode.Override(TonemappingMode.None);
            ColorAdjustments cor = Componente<ColorAdjustments>(p, persistir);
            cor.saturation.Override(0f);
            cor.contrast.Override(4f);
            Vignette vinheta = Componente<Vignette>(p, persistir);
            vinheta.intensity.Override(0.22f);
            vinheta.smoothness.Override(0.4f);
            vinheta.color.Override(new Color(0.22f, 0.13f, 0.1f));

            if (persistir) EditorUtility.SetDirty(p);
            return p;
        }

        static T Componente<T>(VolumeProfile p, bool persistir) where T : VolumeComponent
        {
            T c;
            if (!p.TryGet(out c))
            {
                c = p.Add<T>();
                if (persistir) AssetDatabase.AddObjectToAsset(c, p);
            }
            c.active = true;
            if (persistir) EditorUtility.SetDirty(c);
            return c;
        }

    }
}
