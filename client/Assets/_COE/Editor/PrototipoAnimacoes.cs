using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace COE.EditorTools
{
    /// <summary>ADR-0008 — clips do Mixamo (FBX "sem skin", locomocao "In Place") em Art/Prototipo/Animacoes/ nos modelos do
    /// Tripo. Os clips do placeholder tem bracos em T e o retarget Humanoid copia a pose; os do Mixamo tem pose de gente.
    ///
    /// Monta Prototipo.controller: copia do Player.controller (estados, parametros e transicoes do HumanoidSetup) com
    /// (1) Locomotion em tres pontos, Idle 0 / Walk = caminhada/corrida / Run 1: andando toca o clip de andar, nao meio
    /// "parado" + meio "correndo" (era isso que lia como deslizar); (2) cadencia de cada clip MEDIDA no modelo da
    /// protagonista a 1,10 m (PassadaMedida): timeScale = velocidade do corpo / velocidade do pe apoiado, e o pe para de
    /// escorregar; (3) Attack1..3 e Skill com o soco, Dodge, Hit e Dead com esquiva, reacao e queda do Mixamo, eventos do
    /// HumanoidMapping (OnHitFrame continua dando o dano; OnDodgeEnd fecha a esquiva). Nenhum estado fica com clip do
    /// placeholder. ponytail: cadencia medida aos 5 anos (aos 8 o corpo cresce 16% e a passada natural junto: sobra ~16%
    /// de escorregao; medir por idade se incomodar).</summary>
    public static class PrototipoAnimacoes
    {
        public const string Pasta = Prototipos.Raiz + "/Animacoes";
        public const string ControllerPath = Pasta + "/Prototipo.controller";
        const string OverrideAntigo = Pasta + "/Prototipo.overrideController";

        /// <summary>Arquivo do Mixamo -> clips que saem dele (nome, loop, eventos do slot do HumanoidMapping).</summary>
        static readonly (string Arquivo, string Clip, HumanoidClip EventosDe, bool Loop)[] clips =
        {
            ("Breathing Idle", "Idle", HumanoidClip.Idle, true),
            ("Walking", "Walk", HumanoidClip.Run, true),
            ("Running", "Run", HumanoidClip.Run, true),
            ("Punching", "Attack1", HumanoidClip.Attack1, false),
            ("Punching", "Attack2", HumanoidClip.Attack2, false),
            ("Punching", "Attack3", HumanoidClip.Attack3, false),
            ("Standing Dodge Backward", "Dodge", HumanoidClip.Dodge, false),   // a esquiva do jogo e no lugar (i-frames)
            ("Hit Reaction", "Hit", HumanoidClip.Hit, false),
            ("Dying", "Death", HumanoidClip.Death, false),
        };

        /// <summary>Cadencia maxima: acima disso a crianca "pedala". O corpo continua na velocidade do jogo e o pe volta a
        /// escorregar um pouco; o log avisa para recalibrar MotionSolver.</summary>
        public const float CadenciaMaxima = 2.3f;

        /// <summary>Velocidade de reproducao que faz o pe apoiado andar na velocidade do corpo. Natural 0 (medicao falhou)
        /// = 1, sem ajuste; limitada a [0,5; CadenciaMaxima].</summary>
        public static float Cadencia(float velocidadeDoCorpo, float velocidadeNatural)
        {
            if (velocidadeNatural <= 0.01f) return 1f;
            return Mathf.Clamp(velocidadeDoCorpo / velocidadeNatural, 0.5f, CadenciaMaxima);
        }

        public static RuntimeAnimatorController Montar()
        {
            if (!Directory.Exists(Pasta) || AssetDatabase.LoadAssetAtPath<AnimatorController>(HumanoidSetup.ControllerPath) == null) return null;

            var porNome = new Dictionary<string, AnimationClip>();
            foreach (var grupo in clips.GroupBy(c => c.Arquivo))
            {
                string path = Pasta + "/" + grupo.Key + ".fbx";
                var mi = AssetImporter.GetAtPath(path) as ModelImporter;
                if (mi == null) { Debug.LogWarning("PrototipoAnimacoes: falta " + path); continue; }
                Configurar(mi, grupo.ToArray());
                foreach (AnimationClip c in AssetDatabase.LoadAllAssetsAtPath(path).OfType<AnimationClip>())
                    if (grupo.Any(g => g.Clip == c.name)) porNome[c.name] = c;
            }
            if (!porNome.ContainsKey("Idle") || !porNome.ContainsKey("Run")) return null;

            AssetDatabase.DeleteAsset(OverrideAntigo);
            AssetDatabase.DeleteAsset(ControllerPath);
            AssetDatabase.CopyAsset(HumanoidSetup.ControllerPath, ControllerPath);
            var ac = AssetDatabase.LoadAssetAtPath<AnimatorController>(ControllerPath);

            float caminhada = MotionSolver.VelocidadeCaminhadaPadrao, corrida = MotionSolver.VelocidadeCorridaPadrao;
            GameObject modelo = Prototipos.CarregarDoDisco("protagonista");
            float cadWalk = 1f, cadRun = 1f;
            AnimationClip walk;
            porNome.TryGetValue("Walk", out walk);
            if (modelo != null)
            {
                float natWalk = walk != null ? PassadaMedida.Medir(modelo, walk, BodyScale.Crianca5) : 0f;
                float natRun = PassadaMedida.Medir(modelo, porNome["Run"], BodyScale.Crianca5);
                cadWalk = Cadencia(caminhada, natWalk);
                cadRun = Cadencia(corrida, natRun);
                Debug.Log("PrototipoAnimacoes: passada natural a 1,10 m: andar " + natWalk.ToString("F2") + " m/s, correr "
                    + natRun.ToString("F2") + " m/s; corpo " + caminhada + " e " + corrida + " m/s; cadencia "
                    + cadWalk.ToString("F2") + "x e " + cadRun.ToString("F2") + "x"
                    + (cadWalk >= CadenciaMaxima || cadRun >= CadenciaMaxima ? " (NO TETO: o pe ainda escorrega; recalibrar MotionSolver)" : ""));
            }

            foreach (ChildAnimatorState s in ac.layers[0].stateMachine.states)
            {
                AnimatorState st = s.state;
                if (st.name == "Locomotion")
                {
                    var tree = new BlendTree { name = "Locomotion", blendType = BlendTreeType.Simple1D, blendParameter = AnimParams.Speed, useAutomaticThresholds = false };
                    AssetDatabase.AddObjectToAsset(tree, ac);
                    tree.AddChild(porNome["Idle"], 0f);
                    if (walk != null) tree.AddChild(walk, caminhada / corrida);
                    tree.AddChild(porNome["Run"], 1f);
                    ChildMotion[] filhos = tree.children;
                    for (int i = 0; i < filhos.Length; i++)
                        filhos[i].timeScale = filhos[i].motion == walk ? cadWalk : filhos[i].motion == porNome["Run"] ? cadRun : 1f;
                    tree.children = filhos;
                    st.motion = tree;
                }
                else if (st.name == "Skill" && porNome.ContainsKey("Attack3")) st.motion = porNome["Attack3"];
                else if (st.name == "Dead" && porNome.ContainsKey("Death")) st.motion = porNome["Death"];   // estado Dead, clip Death
                else if (porNome.ContainsKey(st.name)) st.motion = porNome[st.name];                        // Attack1..3, Dodge, Hit
            }
            EditorUtility.SetDirty(ac);
            AssetDatabase.SaveAssets();
            Debug.Log("PrototipoAnimacoes: " + ControllerPath + " com " + porNome.Count + " clips do Mixamo: " + string.Join(", ", porNome.Keys));
            return ac;
        }

        /// <summary>Um take vira um clip por entrada, com loop e eventos. Mesmo ajuste de raiz do HumanoidSetup.ImportClip.</summary>
        static void Configurar(ModelImporter mi, (string Arquivo, string Clip, HumanoidClip EventosDe, bool Loop)[] saidas)
        {
            mi.animationType = ModelImporterAnimationType.Human;
            mi.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
            mi.importAnimation = true;
            mi.clipAnimations = new ModelImporterClipAnimation[0];
            mi.SaveAndReimport();

            ModelImporterClipAnimation[] takes = mi.defaultClipAnimations;
            if (takes.Length == 0) { Debug.LogWarning("PrototipoAnimacoes: " + mi.assetPath + " sem animacao."); return; }
            ModelImporterClipAnimation t = takes[0];
            mi.clipAnimations = saidas.Select(s => new ModelImporterClipAnimation
            {
                name = s.Clip,
                takeName = t.takeName,
                firstFrame = t.firstFrame,
                lastFrame = t.lastFrame,
                loopTime = s.Loop,
                lockRootHeightY = true,
                keepOriginalPositionY = true,
                lockRootRotation = true,
                lockRootPositionXZ = true,
                events = HumanoidMapping.Events(s.EventosDe).Select(e => new AnimationEvent { functionName = e.Method, time = e.Time }).ToArray(),
            }).ToArray();
            mi.SaveAndReimport();
        }

        /// <summary>O controller dos modelos do Tripo: Prototipo.controller se ja foi montado, senao o Player.controller.</summary>
        public static RuntimeAnimatorController Controller()
        {
            var ac = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(ControllerPath);
            return ac != null ? ac : AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(HumanoidSetup.ControllerPath);
        }
    }
}
