using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace COE.EditorTools
{
    /// <summary>ADR-0008 — clips do Mixamo (FBX "sem skin") em Art/Prototipo/Animacoes/ trocam os do placeholder nos
    /// modelos do Tripo. Os clips do placeholder foram feitos com os bracos em T, e o retarget Humanoid copia isso para
    /// qualquer avatar; os do Mixamo tem pose de gente. Um AnimatorOverrideController sobre o Player.controller troca so
    /// os clips: estados, parametros e transicoes continuam os do HumanoidSetup.
    /// Tabela: slot do Player.controller -> arquivo do Mixamo. Os eventos (OnHitFrame, OnFootstep...) vem do
    /// HumanoidMapping, entao o dano do golpe continua saindo no quadro de impacto.
    /// ponytail: Dodge, Hit e Death seguem os do placeholder (bracos em T num lance curto); trocar quando baixar os clips.</summary>
    public static class PrototipoAnimacoes
    {
        public const string Pasta = Prototipos.Raiz + "/Animacoes";
        public const string OverridePath = Pasta + "/Prototipo.overrideController";

        static readonly (HumanoidClip Slot, string Arquivo)[] tabela =
        {
            (HumanoidClip.Idle, "Breathing Idle"),
            (HumanoidClip.Run, "Running"),
            (HumanoidClip.Attack1, "Punching"),
            (HumanoidClip.Attack2, "Punching"),
            (HumanoidClip.Attack3, "Punching"),
        };

        /// <summary>Configura os FBX (Humanoid, um clip por slot) e grava o override. Null se faltar arquivo ou controller:
        /// quem chama segue com o Player.controller puro.</summary>
        public static RuntimeAnimatorController Montar()
        {
            var baseCtrl = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(HumanoidSetup.ControllerPath);
            if (baseCtrl == null || !Directory.Exists(Pasta)) return null;

            var novos = new Dictionary<HumanoidClip, AnimationClip>();
            foreach (var grupo in tabela.GroupBy(t => t.Arquivo))
            {
                string path = Pasta + "/" + grupo.Key + ".fbx";
                var mi = AssetImporter.GetAtPath(path) as ModelImporter;
                if (mi == null) { Debug.LogWarning("PrototipoAnimacoes: falta " + path); continue; }
                ConfigurarClips(mi, grupo.Select(g => g.Slot).ToArray());
                foreach (AnimationClip c in AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>())
                    foreach (HumanoidClip slot in grupo.Select(g => g.Slot))
                        if (c.name == slot.ToString()) novos[slot] = c;
            }
            if (novos.Count == 0) return null;

            var ov = AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(OverridePath);
            if (ov == null) { ov = new AnimatorOverrideController(baseCtrl); AssetDatabase.CreateAsset(ov, OverridePath); }
            ov.runtimeAnimatorController = baseCtrl;
            var pares = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            foreach (AnimationClip original in baseCtrl.animationClips.Distinct())
            {
                AnimationClip novo = null;
                foreach (var kv in novos) if (original.name == kv.Key.ToString()) novo = kv.Value;
                pares.Add(new KeyValuePair<AnimationClip, AnimationClip>(original, novo));
            }
            ov.ApplyOverrides(pares);
            EditorUtility.SetDirty(ov);
            AssetDatabase.SaveAssets();
            Debug.Log("PrototipoAnimacoes: override com " + novos.Count + " clips do Mixamo: " + string.Join(", ", novos.Keys));
            return ov;
        }

        /// <summary>Um take do Mixamo vira um clip por slot, com loop e eventos do HumanoidMapping. Mesmo ajuste de raiz do
        /// HumanoidSetup.ImportClip: altura assada, XZ e giro viram root motion que o Animator descarta.</summary>
        static void ConfigurarClips(ModelImporter mi, HumanoidClip[] slots)
        {
            mi.animationType = ModelImporterAnimationType.Human;
            mi.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
            mi.importAnimation = true;
            mi.clipAnimations = new ModelImporterClipAnimation[0];
            mi.SaveAndReimport();

            ModelImporterClipAnimation[] takes = mi.defaultClipAnimations;
            if (takes.Length == 0) { Debug.LogWarning("PrototipoAnimacoes: " + mi.assetPath + " sem animacao."); return; }
            mi.clipAnimations = slots.Select(slot =>
            {
                ModelImporterClipAnimation c = takes[0];
                var clip = new ModelImporterClipAnimation
                {
                    name = slot.ToString(),
                    takeName = c.takeName,
                    firstFrame = c.firstFrame,
                    lastFrame = c.lastFrame,
                    loopTime = HumanoidMapping.Loops(slot),
                    lockRootHeightY = true,
                    keepOriginalPositionY = true,
                    lockRootRotation = true,
                    lockRootPositionXZ = true,
                    events = HumanoidMapping.Events(slot).Select(e => new AnimationEvent { functionName = e.Method, time = e.Time }).ToArray(),
                };
                return clip;
            }).ToArray();
            mi.SaveAndReimport();
        }

        /// <summary>O controller que os modelos do Tripo usam: o override se ja foi montado, senao o Player.controller.</summary>
        public static RuntimeAnimatorController Controller()
        {
            var ov = AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>(OverridePath);
            if (ov != null) return ov;
            return AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(HumanoidSetup.ControllerPath);
        }
    }
}
