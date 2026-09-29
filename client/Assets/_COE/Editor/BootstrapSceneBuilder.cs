using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace COE.EditorTools
{
    /// <summary>Gera Assets/_COE/Scenes/Bootstrap.unity: chao, luz, input, save, Player (capsula + CharacterController +
    /// Health + Hitbox + Faction + CharacterAnimator + CharacterMotor + PlayerInteractor), camera em terceira pessoa
    /// e dois cubos com SimpleInteractable (poste e caixa) para provar a interacao do T002.
    /// Nada de mundo, NPC ou missao: isso e T007/T008.
    /// ponytail: idempotencia = a cena e recriada do zero a cada execucao (nada a "atualizar", nada duplica).
    /// Nao edite a cena a mao; edite este script.</summary>
    public static class BootstrapSceneBuilder
    {
        public const string ScenePath = "Assets/_COE/Scenes/Bootstrap.unity";
        const string MatDir = "Assets/_COE/Materials";
        const string PresetPath = ProjectSetup.SettingsDir + "/ControlPreset_Destro.asset";

        [MenuItem("COE/Gerar cena Bootstrap")]
        public static void Build()
        {
            Directory.CreateDirectory(MatDir);
            Directory.CreateDirectory(ProjectSetup.SettingsDir);
            Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));

            ControlPreset preset = AssetDatabase.LoadAssetAtPath<ControlPreset>(PresetPath);
            if (preset == null)
            {
                preset = ControlPreset.Default(HandPreset.Destro);
                AssetDatabase.CreateAsset(preset, PresetPath);
            }

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            Populate(preset, Mat);

            EditorSceneManager.SaveScene(scene, ScenePath);
            // Acrescenta sem apagar as outras cenas: regerar o Bootstrap nao pode derrubar Auren da lista.
            var lista = new System.Collections.Generic.List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            if (!lista.Exists(s => s.path == ScenePath)) lista.Insert(0, new EditorBuildSettingsScene(ScenePath, true));
            EditorBuildSettings.scenes = lista.ToArray();
            AssetDatabase.SaveAssets();
            Debug.Log("BootstrapSceneBuilder: cena salva em " + ScenePath);
        }

        /// <summary>Monta a cena na cena ATIVA, sem gravar nada em disco (um teste de Editor pode chamar direto).
        /// preset/mat nulos = instancias em memoria.</summary>
        public static void Populate(ControlPreset preset = null, Func<string, Color, Material> mat = null)
        {
            // ponytail: preset/materiais em memoria nao sao destruidos (vivem ate o domain reload).
            if (preset == null) preset = ControlPreset.Default(HandPreset.Destro);
            if (mat == null) mat = NewMat;

            Material floorMat = mat("COE_Floor", new Color(0.45f, 0.45f, 0.45f));
            Material playerMat = mat("COE_Player", new Color(0.3f, 0.5f, 0.9f));
            Material propMat = mat("COE_Prop", new Color(0.72f, 0.55f, 0.3f));

            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.45f, 0.45f, 0.5f);

            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(4f, 1f, 4f); // Plane = 10 m -> 40x40
            ground.GetComponent<Renderer>().sharedMaterial = floorMat;

            var light = new GameObject("Directional Light").AddComponent<Light>();
            light.type = LightType.Directional;
            light.shadows = LightShadows.Soft;
            light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            var input = new GameObject("Input").AddComponent<PlayerInputReader>();
            input.Preset = preset; // setter publico; via SerializedObject a referencia ao asset vinha nula em batch mode
            EditorUtility.SetDirty(input);

            new GameObject("Save").AddComponent<SaveBootstrap>(); // -200: carrega save.json antes de tudo

            // Raiz na altura dos pes (y=0) + capsula visual filha centrada em y=1. Camera, Hitbox e CharacterController
            // assumem pivot no chao (pivotOffset 1.5, esfera do golpe a 0.9 m).
            var player = new GameObject("Player");
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "Body";
            body.transform.SetParent(player.transform, false);
            body.transform.localPosition = Vector3.up;
            body.GetComponent<Renderer>().sharedMaterial = playerMat;
            Object.DestroyImmediate(body.GetComponent<Collider>());
            CharacterController cc = player.AddComponent<CharacterController>();
            cc.center = Vector3.up;
            cc.height = 2f;
            cc.radius = 0.5f;
            player.AddComponent<Health>();
            player.AddComponent<Hitbox>();
            player.AddComponent<HitFlash>();
            player.AddComponent<Faction>().side = Side.Player;
            player.AddComponent<CharacterAnimator>();
            CharacterMotor motor = player.AddComponent<CharacterMotor>();
            PlayerInteractor interactor = player.AddComponent<PlayerInteractor>();
            Set(interactor, "input", input);
            PlayerCombat combate = player.AddComponent<PlayerCombat>();   // T011: ataque leve/forte, bloqueio, esquiva, magia
            Set(combate, "input", input);
            HumanoidSetup.AttachTo(player); // modelo humanoide como filho, se ja existir; sem prefab segue a capsula

            // Parceiro de treino (T011): determinístico, telegrafa, nao persegue. Sem ele o combate nao tem com quem acontecer.
            var treino = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            treino.name = "ParceiroDeTreino";
            treino.transform.position = new Vector3(4f, 1f, 6f);
            treino.GetComponent<Renderer>().sharedMaterial = propMat;
            treino.AddComponent<Health>();
            treino.AddComponent<Hitbox>();
            treino.AddComponent<HitFlash>();
            treino.AddComponent<Faction>().side = Side.Hostile;
            treino.AddComponent<CharacterAnimator>();
            treino.AddComponent<TrainingDummy>();

            // Longe o bastante do spawn (3,5 m) para so virarem alvo depois de o jogador andar ate eles.
            Prop("Poste", new Vector3(2.5f, 0f, 2.5f), new Vector3(0.3f, 3f, 0.3f), "Examinar o poste", propMat);
            Prop("Caixa", new Vector3(-2f, 0f, 3f), new Vector3(0.8f, 0.8f, 0.8f), "Abrir a caixa", propMat);

            var camGo = new GameObject("Main Camera");
            camGo.tag = "MainCamera";
            Camera cam = camGo.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.16f, 0.18f, 0.22f);
            camGo.AddComponent<AudioListener>();
            camGo.AddComponent<UniversalAdditionalCameraData>();
            var tpc = camGo.AddComponent<ThirdPersonCamera>();
            camGo.transform.position = new Vector3(0f, 3f, -5f);
            Set(tpc, "target", player.transform);
            Set(tpc, "input", input);
            Set(motor, "input", input);
            Set(motor, "cam", tpc); // a camera nasce depois do Player: o yaw so pode ser ligado aqui

            new GameObject("Perf").AddComponent<PerfHud>(); // FPS na tela + CSV em persistentDataPath
        }

        /// <summary>Cubo com SimpleInteractable, apoiado no chao. Prova de interacao do T002 - nao e cenario de Auren.</summary>
        static void Prop(string nome, Vector3 pos, Vector3 escala, string prompt, Material material)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = nome;
            go.transform.localScale = escala;
            go.transform.position = pos + Vector3.up * escala.y * 0.5f;
            go.GetComponent<Renderer>().sharedMaterial = material;
            go.AddComponent<SimpleInteractable>().prompt = prompt;
        }

        static Material Mat(string name, Color color)
        {
            string path = MatDir + "/" + name + ".mat";
            Material m = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (m != null) return m;
            m = NewMat(name, color);
            AssetDatabase.CreateAsset(m, path);
            return m;
        }

        static Material NewMat(string name, Color color)
        {
            Shader lit = Shader.Find("Universal Render Pipeline/Lit");
            if (lit == null) throw new Exception("Shader URP/Lit nao encontrado; URP instalada?");
            var m = new Material(lit) { name = name }; // CreateAsset renomeia pelo arquivo (mesmo nome)
            m.SetColor("_BaseColor", color);
            return m;
        }

        // Campo [SerializeField] privado: falha alto se o nome do campo mudou no script.
        static void Set(Object target, string field, Object value)
        {
            var so = new SerializedObject(target);
            SerializedProperty p = so.FindProperty(field);
            if (p == null) throw new Exception(target.GetType().Name + " nao tem o campo serializado '" + field + "'.");
            p.objectReferenceValue = value;
            so.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
