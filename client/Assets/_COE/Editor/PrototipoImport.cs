using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace COE.EditorTools
{
    /// <summary>ADR-0008 — import dos FBX do Tripo3D em Assets/_COE/Art/Prototipo/** (fora do validador de arte: PROTOTIPO
    /// nao finge ter passado pelo portao do ADR-0002).
    /// Personagens/: Humanoid com avatar do proprio modelo; se o auto-mapeamento falhar, loga e cai para Generic (marca
    /// no userData do importer; <see cref="Reimportar"/> limpa a marca e tenta Humanoid de novo).
    /// Pecas/: sem rig, eixo assado no import (raiz com rotacao identidade).
    /// Os dois: sem camera/luz/colisor importados, malha nao legivel e comprimida, normais do arquivo, e todo material
    /// embutido vira COE/Toon com a textura/cor base do FBX (material toon gerado a partir da textura base).
    /// Batch: Unity -executeMethod COE.EditorTools.PrototipoImport.Reimportar</summary>
    public class PrototipoImport : AssetPostprocessor
    {
        public const string Raiz = Prototipos.Raiz + "/";
        const string MarcaGeneric = "coe:generic";

        // Depois do preprocessador de material do URP (-980), que monta o URP/Lit com _BaseMap/_BaseColor do FBX.
        public override int GetPostprocessOrder() { return 100; }
        public override uint GetVersion() { return 1; }

        static bool EhPrototipo(string path)
        {
            return path.StartsWith(Raiz, StringComparison.Ordinal) && path.EndsWith(".fbx", StringComparison.OrdinalIgnoreCase);
        }

        static bool EhPersonagem(string path) { return path.StartsWith(Raiz + "Personagens/", StringComparison.Ordinal); }

        void OnPreprocessModel()
        {
            if (!EhPrototipo(assetPath)) return;
            var mi = (ModelImporter)assetImporter;
            mi.importCameras = false;
            mi.importLights = false;
            mi.addCollider = false;             // colisao e do greybox/gerador, nunca da malha do Tripo
            mi.isReadable = false;              // sem copia na RAM do celular
            mi.meshCompression = ModelImporterMeshCompression.Medium;
            mi.importNormals = ModelImporterNormals.Import;
            mi.importBlendShapes = false;
            mi.importAnimation = false;         // o personagem anima com os clips do placeholder (Player.controller)
            mi.materialImportMode = ModelImporterMaterialImportMode.ImportViaMaterialDescription;
            mi.materialLocation = ModelImporterMaterialLocation.InPrefab;

            if (EhPersonagem(assetPath))
            {
                bool generic = mi.userData == MarcaGeneric;
                mi.animationType = generic ? ModelImporterAnimationType.Generic : ModelImporterAnimationType.Human;
                mi.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
                mi.optimizeGameObjects = false;
                mi.skinWeights = ModelImporterSkinWeights.Standard;   // 4 ossos por vertice (PIPELINE §4, V16)
            }
            else
            {
                mi.animationType = ModelImporterAnimationType.None;
                mi.bakeAxisConversion = true;   // PIPELINE §9: estatico sai com a raiz em rotacao identidade
            }
        }

        void OnPreprocessMaterialDescription(MaterialDescription descricao, Material material, AnimationClip[] clips)
        {
            if (!EhPrototipo(assetPath)) return;
            Shader toon = Shader.Find(LookSetup.ShaderToon);
            if (toon == null) return;   // shader ainda nao importado: fica o URP/Lit; "COE / Reimportar prototipos" corrige

            Texture textura = null;
            TexturePropertyDescription t;
            if (descricao.TryGetProperty("DiffuseColor", out t) && t.texture != null) textura = t.texture;
            else if (material.HasProperty("_BaseMap")) textura = material.GetTexture("_BaseMap");
            if (textura == null) textura = TexturaBaseAoLado();
            Color cor = material.HasProperty("_BaseColor") ? material.GetColor("_BaseColor") : Color.white;
            cor.a = 1f;   // toon e opaco: transparencia do Tripo (quase sempre engano do export) nao vale aqui

            // O Tripo manda normal/metalico/rugosidade: o toon nao usa, e slot velho no material ainda puxa a textura
            // para o APK. Esvazia todos antes de trocar o shader.
            foreach (string slot in material.GetTexturePropertyNames()) material.SetTexture(slot, null);
            material.shader = toon;
            foreach (string k in material.shaderKeywords) material.DisableKeyword(k);
            material.renderQueue = -1;
            material.SetTexture("_BaseMap", textura);
            material.SetColor("_BaseColor", cor);
        }

        /// <summary>O FBX do Tripo aponta a textura para "tripo_convert_*.fbm/&lt;id&gt;_basecolor.JPEG", pasta que nao vem
        /// junto; o arquivo chega ao lado do FBX. Acha o "*_basecolor.*" da pasta e registra a dependencia (textura trocada
        /// reimporta o modelo).</summary>
        Texture TexturaBaseAoLado()
        {
            string pasta = Path.GetDirectoryName(assetPath).Replace('\\', '/');
            foreach (string arquivo in Directory.GetFiles(pasta))
            {
                string nome = Path.GetFileName(arquivo);
                if (nome.EndsWith(".meta", StringComparison.OrdinalIgnoreCase)) continue;
                if (nome.IndexOf("_basecolor", StringComparison.OrdinalIgnoreCase) < 0) continue;
                string path = pasta + "/" + nome;
                context.DependsOnSourceAsset(path);
                Texture t = AssetDatabase.LoadAssetAtPath<Texture>(path);
                if (t != null) return t;
            }
            return null;
        }

        /// <summary>Textura que vem com o FBX (o Tripo exporta 2048 px): teto de 1024 (PIPELINE §4, Avatar/Npc) e ASTC 6x6
        /// no Android (§4.1). ponytail: um teto para tudo; por categoria/mapa quando a peca passar pelo portao.</summary>
        void OnPreprocessTexture()
        {
            if (!assetPath.StartsWith(Raiz, StringComparison.Ordinal)) return;
            var ti = (TextureImporter)assetImporter;
            ti.maxTextureSize = 1024;
            ti.mipmapEnabled = true;
            TextureImporterPlatformSettings android = ti.GetPlatformTextureSettings("Android");
            android.overridden = true;
            android.maxTextureSize = 1024;
            android.format = TextureImporterFormat.ASTC_6x6;
            ti.SetPlatformTextureSettings(android);
        }

        // Import interativo (arrastar o FBX): confere o avatar depois do import. Em batch o Reimportar confere na hora.
        static void OnPostprocessAllAssets(string[] importados, string[] apagados, string[] movidos, string[] origens)
        {
            foreach (string p in importados)
            {
                if (!EhPrototipo(p) || !EhPersonagem(p)) continue;
                string path = p;
                EditorApplication.delayCall += () => ConferirAvatar(path);
            }
        }

        static void ConferirAvatar(string path)
        {
            var mi = AssetImporter.GetAtPath(path) as ModelImporter;
            if (mi == null || mi.userData == MarcaGeneric) return;
            Avatar avatar = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Avatar>().FirstOrDefault();
            if (avatar != null && avatar.isValid && avatar.isHuman) return;

            Debug.LogWarning("PrototipoImport: " + path + " nao mapeou como Humanoid (rig do Tripo fora do padrao?); caindo para"
                + " Generic. O modelo entra parado, sem os clips do Player.controller. Conferir em Rig > Configure e rodar"
                + " COE / Reimportar prototipos.");
            mi.userData = MarcaGeneric;
            mi.SaveAndReimport();
        }

        /// <summary>Reimporta todo FBX de Art/Prototipo com as regras acima (tenta Humanoid de novo) e confere o avatar.
        /// Rodar depois de trocar um FBX ou este arquivo, e antes de regerar as cenas.</summary>
        [MenuItem("COE/Reimportar prototipos")]
        public static void Reimportar()
        {
            AssetDatabase.Refresh();
            string raiz = Prototipos.Raiz;
            if (!Directory.Exists(raiz)) { Debug.Log("PrototipoImport: sem " + raiz + "; nada a reimportar."); return; }

            string[] fbx = Directory.GetFiles(raiz, "*.fbx", SearchOption.AllDirectories)
                .Select(f => f.Replace('\\', '/')).OrderBy(f => f, StringComparer.Ordinal).ToArray();
            foreach (string path in fbx)
            {
                var mi = AssetImporter.GetAtPath(path) as ModelImporter;
                if (mi == null) continue;
                if (mi.userData == MarcaGeneric) mi.userData = "";
                mi.SaveAndReimport();
                if (EhPersonagem(path)) ConferirAvatar(path);
            }
            AssetDatabase.SaveAssets();
            Debug.Log("PrototipoImport: " + fbx.Length + " FBX reimportados em " + raiz + ": " + string.Join(", ", fbx.Select(Path.GetFileNameWithoutExtension)));
        }
    }
}
