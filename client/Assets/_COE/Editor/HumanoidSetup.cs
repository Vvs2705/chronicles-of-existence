using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace COE.EditorTools
{
    /// <summary>Monta o humanoide a partir dos FBX do Meshy+Mixamo em Assets/_COE/Art/Humanoid/:
    /// importa como Humanoid, poe os AnimationEvents nos clips, gera Player.controller e PlayerModel.prefab.
    /// Batch: Unity -executeMethod COE.EditorTools.HumanoidSetup.Run. Pasta vazia/ausente = log e sai sem erro.
    /// ponytail: idempotencia = controller recriado do zero e prefab sobrescrito a cada execucao (GUID do controller muda;
    /// o prefab e a cena greybox sao regerados depois, entao ninguem fica apontando para o antigo).</summary>
    public static class HumanoidSetup
    {
        public const string Dir = "Assets/_COE/Art/Humanoid";
        public const string ControllerPath = Dir + "/Player.controller";
        public const string PrefabPath = Dir + "/PlayerModel.prefab";

        [MenuItem("COE/Montar humanoide")]
        public static void Run()
        {
            AssetDatabase.Refresh();
            string[] files = Directory.Exists(Dir) ? Directory.GetFiles(Dir, "*.fbx") : new string[0];
            if (files.Length == 0) { Debug.Log("HumanoidSetup: nenhum .fbx em " + Dir + "; nada a montar."); return; }

            var paths = files.ToDictionary(f => Path.GetFileNameWithoutExtension(f), f => Dir + "/" + Path.GetFileName(f));
            HumanoidFiles found = HumanoidMapping.Classify(paths.Keys);
            if (found.Model == null) { Debug.LogWarning("HumanoidSetup: falta o modelo (arquivo com 'model' no nome, ex.: Model.fbx) em " + Dir + "."); return; }

            Avatar avatar = ImportModel(paths[found.Model]);
            if (avatar == null || !avatar.isHuman) { Debug.LogWarning("HumanoidSetup: Avatar Humanoid invalido em " + found.Model + "; conferir Rig -> Configure no Inspector."); return; }

            var clips = new Dictionary<HumanoidClip, AnimationClip>();
            foreach (KeyValuePair<HumanoidClip, string> kv in found.Clips)
            {
                AnimationClip clip = ImportClip(paths[kv.Value], kv.Key, avatar);
                if (clip != null) clips[kv.Key] = clip;
            }

            AnimatorController ac = BuildController(clips);
            BuildPrefab(paths[found.Model], avatar, ac);

            Debug.Log("HumanoidSetup: modelo=" + found.Model + "; clips=" + string.Join(", ", clips.Select(kv => kv.Key + "<-" + found.Clips[kv.Key]))
                + "; SEM clip=[" + string.Join(", ", found.Missing) + "]; ignorados=[" + string.Join(", ", found.Unmapped) + "] -> " + PrefabPath);
        }

        /// <summary>Instancia PlayerModel.prefab como filho do player (antes do Awake: o CharacterAnimator acha o Animator nos filhos),
        /// esconde a capsula greybox e liga o dano por OnHitFrame. Retorna o modelo, ou null se o prefab ainda nao existe.</summary>
        public static GameObject AttachTo(GameObject player)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
            if (prefab == null) return null; // sem arte ainda: capsula (estado normal; o aviso fica no menu Montar humanoide)

            Transform old = player.transform.Find(prefab.name);
            if (old != null) Object.DestroyImmediate(old.gameObject);

            // Capsula: GameObject inativo em vez de so o MeshRenderer desligado, porque HitFlash pega o
            // "primeiro Renderer nos filhos" e isso ignora GameObject inativo, mas nao Renderer desligado.
            foreach (MeshFilter mf in player.GetComponentsInChildren<MeshFilter>())
            {
                if (mf.sharedMesh == null || mf.sharedMesh.name != "Capsule") continue;
                if (mf.gameObject == player) mf.GetComponent<MeshRenderer>().enabled = false; else mf.gameObject.SetActive(false);
            }

            var model = (GameObject)PrefabUtility.InstantiatePrefab(prefab, player.transform);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;

            CharacterAnimator ca = player.GetComponent<CharacterAnimator>();
            if (ca == null) ca = player.AddComponent<CharacterAnimator>(); // o controller do jogador (T002) reusa o existente
            ca.immediate = false; // campo publico + SetDirty (SerializedObject perde referencia/valor em batch mode)
            EditorUtility.SetDirty(ca);
            return model;
        }

        static Avatar ImportModel(string path)
        {
            var mi = (ModelImporter)AssetImporter.GetAtPath(path);
            mi.animationType = ModelImporterAnimationType.Human;
            mi.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
            mi.importAnimation = false; // o modelo so da malha + Avatar; animacao vem dos clips
            mi.SaveAndReimport();

            // O auto-mapeamento do Unity deixa Chest e ombros de fora na proporcao infantil (validador, regra V15).
            // Esqueleto que ja usa os nomes humanos do Unity (contrato do docs/arte/PIPELINE.md §7.1) e mapeado por
            // nome, osso a osso; qualquer outro rig (ex.: Mixamo) fica com o auto-mapeamento.
            HumanDescription hd = mi.humanDescription;
            var noEsqueleto = new HashSet<string>(hd.skeleton.Select(s => s.name));
            bool nomesDoUnity = Enumerable.Range(0, HumanTrait.BoneCount)
                .Where(HumanTrait.RequiredBone).All(i => noEsqueleto.Contains(HumanTrait.BoneName[i]));
            if (nomesDoUnity)
            {
                hd.human = HumanTrait.BoneName.Where(noEsqueleto.Contains)
                    .Select(n => new HumanBone { boneName = n, humanName = n, limit = new HumanLimit { useDefaultValues = true } })
                    .ToArray();
                mi.humanDescription = hd;
                mi.SaveAndReimport();
            }
            return AssetDatabase.LoadAllAssetsAtPath(path).OfType<Avatar>().FirstOrDefault();
        }

        static AnimationClip ImportClip(string path, HumanoidClip which, Avatar avatar)
        {
            var mi = (ModelImporter)AssetImporter.GetAtPath(path);
            mi.animationType = ModelImporterAnimationType.Human;
            mi.avatarSetup = ModelImporterAvatarSetup.CopyFromOther;
            mi.sourceAvatar = avatar;
            mi.importAnimation = true;
            mi.clipAnimations = new ModelImporterClipAnimation[0]; // volta ao take original antes de ler os defaults
            mi.SaveAndReimport();

            ModelImporterClipAnimation[] takes = mi.defaultClipAnimations;
            if (takes.Length == 0) { Debug.LogWarning("HumanoidSetup: " + path + " nao tem animacao."); return null; }
            ModelImporterClipAnimation c = takes[0]; // Mixamo = 1 take ("mixamo.com") por arquivo
            c.name = which.ToString();
            c.loopTime = HumanoidMapping.Loops(which);
            // Altura assada na pose (Death cai de verdade); XZ e giro NAO: viram root motion e o Animator descarta
            // (applyRootMotion = false), entao o corpo fica no lugar e quem move e o codigo.
            c.lockRootHeightY = true;
            c.keepOriginalPositionY = true;
            c.events = HumanoidMapping.Events(which).Select(e => new AnimationEvent { functionName = e.Method, time = e.Time }).ToArray();
            mi.clipAnimations = new[] { c };
            mi.SaveAndReimport();

            return AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>().FirstOrDefault(a => !a.name.StartsWith("__preview__"));
        }

        // Maquina de estados do pipeline secao 4 (uma camada, Any State so para Hit e Dead).
        static AnimatorController BuildController(Dictionary<HumanoidClip, AnimationClip> clips)
        {
            AssetDatabase.DeleteAsset(ControllerPath);
            AnimatorController ac = AnimatorController.CreateAnimatorControllerAtPath(ControllerPath);
            ac.AddParameter(AnimParams.Speed, AnimatorControllerParameterType.Float);
            ac.AddParameter(AnimParams.Attack, AnimatorControllerParameterType.Trigger);
            ac.AddParameter(AnimParams.AttackIndex, AnimatorControllerParameterType.Int);
            ac.AddParameter(AnimParams.Skill, AnimatorControllerParameterType.Trigger);
            ac.AddParameter(AnimParams.SkillIndex, AnimatorControllerParameterType.Int);
            ac.AddParameter(AnimParams.Dodge, AnimatorControllerParameterType.Trigger);
            ac.AddParameter(AnimParams.Hit, AnimatorControllerParameterType.Trigger);
            ac.AddParameter(AnimParams.Dead, AnimatorControllerParameterType.Bool);
            ac.AddParameter(AnimParams.Stagger, AnimatorControllerParameterType.Bool);
            ac.AddParameter(AnimParams.Telegraph, AnimatorControllerParameterType.Trigger);
            // ponytail: sem estado de Telegraph (so inimigo usa; este controller e do jogador). Adicionar quando inimigo humanoide usar.

            AnimatorStateMachine sm = ac.layers[0].stateMachine;
            BlendTree tree;
            AnimatorState loco = ac.CreateBlendTreeInController("Locomotion", out tree, 0);
            tree.blendType = BlendTreeType.Simple1D;
            tree.blendParameter = AnimParams.Speed;
            tree.useAutomaticThresholds = false;
            if (Get(clips, HumanoidClip.Idle) != null) tree.AddChild(clips[HumanoidClip.Idle], 0f);
            if (Get(clips, HumanoidClip.Run) != null) tree.AddChild(clips[HumanoidClip.Run], 1f);
            sm.defaultState = loco;

            var atk = new AnimatorState[3];
            for (int i = 0; i < 3; i++) atk[i] = State(sm, "Attack" + (i + 1), Get(clips, HumanoidClip.Attack1 + i));
            AnimatorState skill = State(sm, "Skill", Get(clips, HumanoidClip.Attack3)); // PoC: skill reusa Attack3
            AnimatorState dodge = State(sm, "Dodge", Get(clips, HumanoidClip.Dodge));
            AnimatorState hit = State(sm, "Hit", Get(clips, HumanoidClip.Hit));
            AnimatorState dead = State(sm, "Dead", Get(clips, HumanoidClip.Death));

            for (int i = 0; i < 3; i++)
            {
                int index = HumanoidMapping.AttackIndex(HumanoidClip.Attack1 + i);
                AttackCond(Go(loco, atk[i], 0.05f, 0f), index);
                if (i < 2) AttackCond(Go(atk[i], atk[i + 1], 0.08f, 0.55f), index + 1); // encadeamento antes da volta a Locomotion
                Go(atk[i], loco, 0.15f, 0.90f);
            }

            foreach (AnimatorState from in atk.Prepend(loco)) Go(from, skill, 0.05f, 0f).AddCondition(AnimatorConditionMode.If, 0f, AnimParams.Skill);
            Go(skill, loco, 0.15f, 0.90f);

            foreach (AnimatorState from in atk.Concat(new[] { loco, skill, hit })) // "qualquer exceto Dead", sem Any State
                Go(from, dodge, 0.03f, 0f).AddCondition(AnimatorConditionMode.If, 0f, AnimParams.Dodge);
            Go(dodge, loco, 0.10f, 0.95f);

            AnimatorStateTransition toHit = Any(sm, hit, 0.05f);
            toHit.AddCondition(AnimatorConditionMode.If, 0f, AnimParams.Hit);
            toHit.AddCondition(AnimatorConditionMode.IfNot, 0f, AnimParams.Dead);
            Go(hit, loco, 0.10f, 0.85f).AddCondition(AnimatorConditionMode.IfNot, 0f, AnimParams.Stagger);
            Go(hit, hit, 0.10f, 0.85f).AddCondition(AnimatorConditionMode.If, 0f, AnimParams.Stagger); // Stagger = Hit em loop

            Any(sm, dead, 0.10f).AddCondition(AnimatorConditionMode.If, 0f, AnimParams.Dead);

            EditorUtility.SetDirty(ac);
            AssetDatabase.SaveAssets();
            return ac;
        }

        static void BuildPrefab(string modelPath, Avatar avatar, AnimatorController ac)
        {
            var go = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(modelPath));
            go.name = "PlayerModel";
            Animator anim = go.GetComponent<Animator>();
            if (anim == null) anim = go.AddComponent<Animator>();
            anim.runtimeAnimatorController = ac;
            anim.avatar = avatar;
            anim.applyRootMotion = false; // o codigo move (CharacterController)
            PrefabUtility.SaveAsPrefabAsset(go, PrefabPath); // instancia de modelo -> Prefab Variant do FBX (reimport propaga)
            Object.DestroyImmediate(go);
            AssetDatabase.SaveAssets();
        }

        static AnimationClip Get(Dictionary<HumanoidClip, AnimationClip> clips, HumanoidClip c)
        {
            AnimationClip a;
            return clips.TryGetValue(c, out a) ? a : null; // sem clip = estado vazio (log lista o que falta)
        }

        static AnimatorState State(AnimatorStateMachine sm, string name, Motion motion)
        {
            AnimatorState s = sm.AddState(name);
            s.motion = motion;
            return s;
        }

        static AnimatorStateTransition Go(AnimatorState from, AnimatorState to, float duration, float exitTime)
        {
            AnimatorStateTransition t = from.AddTransition(to);
            t.duration = duration;
            t.hasExitTime = exitTime > 0f;
            t.exitTime = exitTime;
            return t;
        }

        static AnimatorStateTransition Any(AnimatorStateMachine sm, AnimatorState to, float duration)
        {
            AnimatorStateTransition t = sm.AddAnyStateTransition(to);
            t.duration = duration;
            t.hasExitTime = false;
            t.canTransitionToSelf = false;
            return t;
        }

        static void AttackCond(AnimatorStateTransition t, int index)
        {
            t.AddCondition(AnimatorConditionMode.If, 0f, AnimParams.Attack);
            t.AddCondition(AnimatorConditionMode.Equals, index, AnimParams.AttackIndex);
        }
    }
}
