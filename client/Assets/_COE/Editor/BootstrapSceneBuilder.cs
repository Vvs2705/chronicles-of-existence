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
        /// <summary>Raiz do SomDoJogo (Bootstrap e Auren).</summary>
        public const string NomeSom = "Som";

        public const string ScenePath = "Assets/_COE/Scenes/Bootstrap.unity";
        const string MatDir = "Assets/_COE/Materials";
        public const string PresetPath = "Assets/_COE/Settings/ControlPreset_Destro.asset";

        // Crianca de 5 anos (o slice comeca aqui). Capsula e degrau saem de Corpo (fonte unica com BodyByAge e a
        // camera); pes em y=0. Depois do salto o BodyByAge troca tudo pelo corpo de 8 no Awake do Player.
        static readonly Corpo Crianca = Corpo.DaIdade(5);

        /// <summary>Altura do centro do golpe da crianca de 8 anos (Corpo.AlturaDoGolpe). O golpe so existe depois do
        /// salto (TrainingProgress.PodeTreinar): e o valor da cena salva e a mira do bastao do instrutor. Em runtime o
        /// BodyByAge poe o Hitbox.altura do Player pela idade do save.</summary>
        public static readonly float AlturaDoGolpe = Corpo.DaIdade(8).AlturaDoGolpe;

        [MenuItem("COE/Gerar cena Bootstrap")]
        public static void Build()
        {
            Directory.CreateDirectory(MatDir);
            Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            Populate(Mat);
            EntradaSceneSetup.Montar();   // T012: so a Bootstrap e porta de entrada (nascimento, rota para a cena salva)
            VoltarSetup.Montar();         // voltar do Android / Esc: entrada, menu e salto
            PartidaSetup.Ligar();         // de novo no fim: o que os dois setups acima montarem tambem recebe a Partida

            EditorSceneManager.SaveScene(scene, ScenePath);
            CenaEstavel.Aplicar(ScenePath);   // ids estaveis: regerar sem mudanca de conteudo nao muda o arquivo
            // Acrescenta sem apagar as outras cenas: regerar o Bootstrap nao pode derrubar Auren da lista.
            var lista = new System.Collections.Generic.List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            if (!lista.Exists(s => s.path == ScenePath)) lista.Insert(0, new EditorBuildSettingsScene(ScenePath, true));
            EditorBuildSettings.scenes = lista.ToArray();
            AssetDatabase.SaveAssets();
            Debug.Log("BootstrapSceneBuilder: cena salva em " + ScenePath);
        }

        /// <summary>Monta a cena na cena ATIVA, sem gravar nada em disco (um teste de Editor pode chamar direto).
        /// mat nulo = materiais em memoria.</summary>
        public static void Populate(Func<string, Color, Material> mat = null)
        {
            // ponytail: materiais em memoria nao sao destruidos (vivem ate o domain reload).
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
            // Layout de toque (ADR-0006) por campo serializado. Sem o asset, o padrao destro em memoria (a cena salva o
            // embute). Setter publico: via SerializedObject a referencia vinha nula em batch mode (T002, 889f2fd).
            ControlPreset preset = AssetDatabase.LoadAssetAtPath<ControlPreset>(PresetPath);
            input.Preset = preset != null ? preset : ControlPreset.Default(HandPreset.Destro);
            EditorUtility.SetDirty(input);
            // Desenho dos controles de toque (uGUI), separado da leitura (Bloco D): so le o estado do leitor.
            var toqueHud = new GameObject("ToqueHud").AddComponent<ToqueHud>();
            toqueHud.Leitor = input;
            EditorUtility.SetDirty(toqueHud);

            GameObject save = new GameObject("Save");
            save.AddComponent<SaveBootstrap>(); // -200: carrega save.json antes de tudo
            save.AddComponent<Partida>();       // a sessao da cena, ligada por campo em quem usa (PartidaSetup no fim do Populate)

            // Raiz na altura dos pes (y=0), escala 1: o humanoide (Art/Humanoid) ja vem com a altura da crianca e
            // entra como filho SEM escala. So a capsula-placeholder e escalada. Camera e CharacterController assumem
            // pivot no chao. A esfera do golpe (Hitbox.altura) sai de AlturaDoGolpe.
            var player = new GameObject("Player");
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            body.name = "Body";
            body.transform.SetParent(player.transform, false);
            body.transform.localPosition = Vector3.up * Crianca.CentroY;
            body.transform.localScale = Vector3.one * (Crianca.Altura / 2f); // capsula primitiva = 2 m x raio 0,5
            body.GetComponent<Renderer>().sharedMaterial = playerMat;
            Object.DestroyImmediate(body.GetComponent<Collider>());
            CharacterController cc = player.AddComponent<CharacterController>();
            cc.height = Crianca.Altura;
            cc.radius = Crianca.Raio;                    // ~0,28 m: mesma proporcao da capsula visual
            cc.center = Vector3.up * Crianca.CentroY;    // base da capsula nos pes
            cc.stepOffset = Crianca.Degrau;              // ~0,2 m, degrau de escada infantil (ponytail em Corpo)
            player.AddComponent<Health>();
            player.AddComponent<Hitbox>().altura = AlturaDoGolpe;
            HitFlash flashPlayer = player.AddComponent<HitFlash>();
            player.AddComponent<Faction>().side = Side.Player;
            player.AddComponent<CharacterAnimator>();
            CharacterMotor motor = player.AddComponent<CharacterMotor>();
            PlayerInteractor interactor = player.AddComponent<PlayerInteractor>();
            Set(interactor, "input", input);
            PlayerCombat combate = player.AddComponent<PlayerCombat>();   // T011: ataque leve/forte, bloqueio, esquiva, magia
            Set(combate, "input", input);
            // T004/T009: entra na ancora do save. Aqui fica inerte (Bootstrap nao tem ancoras); quem cria "Ancoras"
            // liga com LigarAncoras.
            player.AddComponent<AnchorSpawn>();
            // ADR-0008: prototipo do Tripo (Art/Prototipo/Personagens/protagonista) se existir; senao o humanoide
            // placeholder, se ja existir; sem os dois segue a capsula.
            GameObject modeloPlayer = Prototipos.AnexarProtagonista(player) ?? HumanoidSetup.AttachTo(player);

            // Parceiro de treino (T011): determinístico, telegrafa, nao persegue. Sem ele o combate nao tem com quem acontecer.
            // E o INSTRUTOR ADULTO (BodyScale.Adulto), nao outra crianca. A capsula primitiva tem 2 m e pivo no centro:
            // escala para a altura de adulto e raiz a meia altura (pes em y=0). Auren o leva ao posto_guarda (B15).
            const float meioAdulto = BodyScale.Adulto * 0.5f;
            var treino = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            treino.name = "ParceiroDeTreino";
            treino.transform.localScale = Vector3.one * (BodyScale.Adulto / 2f);
            treino.transform.position = new Vector3(4f, meioAdulto, 6f);
            treino.GetComponent<Renderer>().sharedMaterial = propMat;
            treino.AddComponent<Health>();
            // O bastao mira o tronco da crianca (a mesma AlturaDoGolpe), medido a partir do pivo no centro da capsula.
            treino.AddComponent<Hitbox>().altura = AlturaDoGolpe - meioAdulto;
            HitFlash flashTreino = treino.AddComponent<HitFlash>();
            treino.AddComponent<Faction>().side = Side.Hostile;
            treino.AddComponent<CharacterAnimator>();
            Set(treino.AddComponent<TrainingDummy>(), "alvo", player.transform); // para quem ele vira, sem busca global
            // ADR-0008: modelo do Tripo (Personagens/parceiro_treino) se existir; senao a capsula. Colisor e Hitbox ficam.
            GameObject modeloTreino = Prototipos.AnexarParceiro(treino);

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

            // Numero de dano flutuante (T011): camera e alvos ligados aqui, sem Camera.main nem singleton.
            DamagePopup numeros = new GameObject("DamagePopup").AddComponent<DamagePopup>();
            Set(numeros, "cam", cam);
            Set(flashPlayer, "numeros", numeros);
            Set(flashTreino, "numeros", numeros);
            // Com modelo, a capsula fica com o renderer desligado e o HitFlash ("primeiro Renderer nos filhos") pegaria
            // ela: o flash e o aviso do golpe nao apareceriam. Liga direto no renderer do modelo.
            if (modeloPlayer != null) Set(flashPlayer, "body", modeloPlayer.GetComponentInChildren<Renderer>());
            if (modeloTreino != null) Set(flashTreino, "body", modeloTreino.GetComponentInChildren<Renderer>());

            // Som do prototipo (Sintese): musica de caixinha e efeitos; quem faz barulho recebe por campo.
            SomDoJogo som = new GameObject(NomeSom).AddComponent<SomDoJogo>();
            Set(motor, "som", som);
            Set(combate, "som", som);
            Set(flashPlayer, "som", som);
            Set(flashTreino, "som", som);

            PerfHud perf = new GameObject("Perf").AddComponent<PerfHud>(); // FPS na tela + CSV em persistentDataPath
            Set(perf, "input", input);   // diagnostico na tela sem FindAnyObjectByType por quadro
            Set(perf, "motor", motor);

            // T012: cada raia monta o seu pedaco em arquivo proprio (contrato do coordenador; zero colisao aqui).
            IdadeSceneSetup.Montar(player, tpc);   // corpo aos 8 anos e tela do salto (B12/B13)
            ConfiguracoesSceneSetup.Montar(input, tpc, perf);   // menu de pausa: mao, sensibilidade, FPS, desempenho
            PartidaSetup.Ligar();   // todo campo `partida` da cena aponta para a Partida do objeto Save
        }

        /// <summary>Liga a raiz "Ancoras" no AnchorSpawn do Player (dependencia explicita, sem Find em runtime).
        /// O gerador que cria as ancoras chama isto logo depois de cria-las (Auren). Estoura se o Player nao tiver
        /// AnchorSpawn: cena montada sem Populate e erro de gerador, nao caso a tolerar.</summary>
        public static void LigarAncoras(GameObject player, Transform raizAncoras)
        {
            AnchorSpawn spawn = player.GetComponent<AnchorSpawn>();
            if (spawn == null) throw new Exception("Player sem AnchorSpawn: a cena nao passou por BootstrapSceneBuilder.Populate.");
            Set(spawn, "ancoras", raizAncoras);
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

        // ADR-0008: todo material de cena e toon (LookSetup); o asset que ja existia troca de shader na regeracao.
        static Material Mat(string name, Color color) { return LookSetup.MaterialAsset(name, color); }

        static Material NewMat(string name, Color color) { return LookSetup.NovoMaterial(name, color); }

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
