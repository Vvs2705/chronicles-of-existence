using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace COE.EditorTools
{
    /// <summary>Gera Assets/_COE/Scenes/Auren.unity: a vila inicial em GREYBOX (so primitivas), com terreno
    /// delimitado, praca central com poco, tres casas acessiveis (interior simples), tres estruturas publicas
    /// (ferraria de Borin, ervanaria de Lysa, posto de Tovin - dossie secao J e GDD 08), ruas, cercas e a borda
    /// do Bosque dos Sussurros como limite navegavel com uma entrada marcada.
    ///
    /// CONTRATO com T006/T007/T012: os ANCORAS (GameObjects vazios em "Ancoras/&lt;id&gt;", ids estaveis em
    /// <see cref="Ancoras"/>) sao onde missao, NPC e gatilho se penduram. ID publicado nao muda sem migracao.
    /// Aqui NAO existe NPC, dialogo nem quest: so espaco navegavel e interagiveis de prova.
    ///
    /// Player, camera, input, save e luz vem de <see cref="BootstrapSceneBuilder.Populate"/> - o chassi do T002
    /// mora em um lugar so. ponytail: idempotencia = a cena e recriada do zero a cada execucao.
    /// Nao edite a cena a mao; edite este script.</summary>
    public static class AurenSceneBuilder
    {
        public const string ScenePath = "Assets/_COE/Scenes/Auren.unity";
        const string MatDir = "Assets/_COE/Materials";
        const string PresetPath = ProjectSetup.SettingsDir + "/ControlPreset_Destro.asset";

        /// <summary>Raiz dos ancoras na cena. Caminho de um ancora: "Ancoras/&lt;id&gt;".</summary>
        public const string RaizAncoras = "Ancoras";
        /// <summary>Raiz do cenario. Predios ficam em "Auren/Construcoes/&lt;id&gt;".</summary>
        public const string RaizMundo = "Auren";

        // --- medidas em metros (ver o resumo no fim do arquivo) ---
        const float MundoX = 120f;            // largura do terreno (X de -60 a 60)
        const float MundoZ = 180f;            // comprimento do terreno (Z de -90 a 90)
        const float LarguraRua = 7f;
        const float LadoPraca = 28f;
        const float AlturaParede = 3f;
        const float EspessuraParede = 0.3f;
        const float VaoPorta = 1.8f;          // > 1 m do raio do CharacterController: da para entrar andando
        const float AlturaPorta = 2.2f;
        const float TopoPiso = 0.02f;         // calcada 2 cm acima do terreno: sem z-fighting e abaixo do stepOffset
        const float ZLinhaBosque = 62f;       // linha de arvores = limite navegavel ao norte
        const float MeiaAberturaBosque = 3f;  // vao de 6 m na linha de arvores
        const float ZFundoClareira = 78f;
        const float MeiaLarguraClareira = 21f;

        // Ancoras: id estavel -> posicao. A tabela e o contrato; a posicao pode ser ajustada, o id nao.
        static readonly (string Id, Vector3 Pos)[] ancoras =
        {
            ("spawn_player",    new Vector3(-20f, 0f, -41f)), // nasce na rua das casas, de frente para a vila
            ("portao_sul",      new Vector3(  0f, 0f, -70f)), // fim da estrada: chegada/partida de Auren
            ("casa_familia",    new Vector3(-22f, 0f, -43f)), // porta: Mara e Daren (casa acessivel 1)
            ("casa_nilo",       new Vector3( 10f, 0f, -43f)), // porta: Nilo (casa acessivel 2)
            ("casa_sera",       new Vector3( 26f, 0f, -43f)), // porta: Sera (casa acessivel 3)
            ("praca_centro",    new Vector3(  0f, 0f,  -4f)), // 4 m ao sul do poco, livre de colisor
            ("mural_avisos",    new Vector3( -6f, 0f,  -8f)), // mural de Maelis: gancho de missoes de quadro
            ("ferraria",        new Vector3( 14f, 0f,  10f)), // porta da ferraria de Borin
            ("ervanaria",       new Vector3(-15f, 0f,  12f)), // porta da ervanaria de Lysa
            ("posto_guarda",    new Vector3(  7f, 0f,  34f)), // porta do posto de Tovin, na estrada norte
            ("entrada_bosque",  new Vector3(  0f, 0f,  60f)), // vao na linha de arvores
            ("bosque_clareira", new Vector3(  0f, 0f,  70f)), // clareira logo depois da entrada
        };

        /// <summary>Ids estaveis dos ancoras, na ordem em que a cena os cria.</summary>
        public static readonly IList<string> Ancoras = Array.AsReadOnly(Array.ConvertAll(ancoras, a => a.Id));

        /// <summary>Ids das tres casas com interior (dossie secao J).</summary>
        public static readonly IList<string> CasasAcessiveis =
            Array.AsReadOnly(new[] { "casa_familia", "casa_nilo", "casa_sera" });

        /// <summary>Ids das tres estruturas publicas (dossie secao J; NPCs da secao G).</summary>
        public static readonly IList<string> EstruturasPublicas =
            Array.AsReadOnly(new[] { "ferraria", "ervanaria", "posto_guarda" });

        [MenuItem("COE/Gerar cena Auren")]
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
            RegistrarNoBuildSettings();
            AssetDatabase.SaveAssets();
            Debug.Log("AurenSceneBuilder: cena salva em " + ScenePath);
        }

        /// <summary>Monta Auren na cena ATIVA, sem gravar nada em disco (um teste de Editor chama direto).
        /// preset/mat nulos = instancias em memoria.</summary>
        public static void Populate(ControlPreset preset = null, Func<string, Color, Material> mat = null)
        {
            if (mat == null) mat = NewMat;

            // Chassi do T002 (Player+CharacterController, camera, input, save, luz, ambiente). Copiar essa
            // montagem aqui seria uma segunda verdade sobre pivot, camera e wiring de campos serializados.
            // ponytail: o acoplamento e por NOME dos objetos de prova do Bootstrap. Remover() estoura alto se
            // eles forem renomeados, e o teste do T008 pega isso antes de virar cena silenciosamente errada.
            BootstrapSceneBuilder.Populate(preset, mat);
            Remover("Ground"); // chao 40x40 de teste: Auren tem terreno proprio
            Remover("Poste");  // interagiveis de prova do T002: Auren tem os seus
            Remover("Caixa");

            Material grama    = mat("COE_Auren_Grama",    Cor(0x64, 0x8B, 0x67)); // GDD: verde #648B67, areas naturais
            Material terra    = mat("COE_Auren_Terra",    Cor(0xB0, 0x92, 0x6A));
            Material pedra    = mat("COE_Auren_Pedra",    Cor(0xAE, 0xAA, 0x9E));
            Material madeira  = mat("COE_Auren_Madeira",  Cor(0x9C, 0x7A, 0x52));
            Material telhado  = mat("COE_Auren_Telhado",  Cor(0xA8, 0x6D, 0x52)); // GDD: terracota #A86D52, Auren
            Material folhagem = mat("COE_Auren_Folhagem", Cor(0x3E, 0x5E, 0x42));
            Material tronco   = mat("COE_Auren_Tronco",   Cor(0x5B, 0x45, 0x30));

            Transform mundo       = Vazio(RaizMundo, null).transform;
            Transform terreno     = Vazio("Terreno", mundo).transform;
            Transform construcoes = Vazio("Construcoes", mundo).transform;
            Transform cenario     = Vazio("Cenario", mundo).transform;
            Transform bosque      = Vazio("Bosque", mundo).transform;

            Terreno(terreno, grama, terra, pedra, madeira);
            Construcoes(construcoes, madeira, terra, pedra, telhado);
            Cenario(cenario, pedra, madeira);
            Bosque(bosque, folhagem, tronco);

            Transform raizAncoras = Vazio(RaizAncoras, null).transform;
            for (int i = 0; i < ancoras.Length; i++) Vazio(ancoras[i].Id, raizAncoras, ancoras[i].Pos);

            // Spawn: em pe na rua das casas, olhando para o norte (praca ao fundo).
            Achar("Player").transform.SetPositionAndRotation(PosicaoDaAncora("spawn_player"), Quaternion.identity);
        }

        /// <summary>Posicao de projeto de um ancora, sem depender de a cena estar aberta (T006/T012).</summary>
        public static Vector3 PosicaoDaAncora(string id)
        {
            for (int i = 0; i < ancoras.Length; i++) if (ancoras[i].Id == id) return ancoras[i].Pos;
            throw new Exception("AurenSceneBuilder: ancora desconhecida '" + id + "'.");
        }

        /// <summary>Objeto de raiz da cena ATIVA pelo nome (ex.: "Auren", "Ancoras", "Player"). Estoura se faltar.</summary>
        public static GameObject Achar(string nome)
        {
            GameObject[] raizes = SceneManager.GetActiveScene().GetRootGameObjects();
            for (int i = 0; i < raizes.Length; i++) if (raizes[i].name == nome) return raizes[i];
            throw new Exception("AurenSceneBuilder: a cena ativa nao tem o objeto de raiz '" + nome + "'.");
        }

        // ---------------------------------------------------------------- terreno, ruas, limites

        static void Terreno(Transform pai, Material grama, Material terra, Material pedra, Material madeira)
        {
            Caixa("terreno", pai, new Vector3(0f, -0.25f, 0f), new Vector3(MundoX, 0.5f, MundoZ), grama); // topo em y=0

            Caixa("praca", pai, new Vector3(0f, TopoPiso - 0.05f, 0f), new Vector3(LadoPraca, 0.1f, LadoPraca), pedra);

            // Eixo principal: portao sul (z=-72) -> praca -> entrada do bosque (z=62). 134 m de rua.
            Rua("rua_sul",   pai, new Vector3(0f, 0f, -43f), new Vector2(LarguraRua, 58f), terra);
            Rua("rua_norte", pai, new Vector3(0f, 0f,  38f), new Vector2(LarguraRua, 48f), terra);
            Rua("rua_casas", pai, new Vector3(0f, 0f, -40f), new Vector2(64f, 6f), terra);
            Rua("ramal_posto", pai, new Vector3(6f, 0f, 34f), new Vector2(9f, 5f), terra);

            // ponytail: cercas sao um bloco baixo em vez de mourao + travessa. Silhueta de verdade e T013.
            Caixa("cerca_oeste", pai, new Vector3(-34f, 0.55f, -40f), new Vector3(0.3f, 1.1f, 40f), madeira);
            Caixa("cerca_leste", pai, new Vector3( 38f, 0.55f, -40f), new Vector3(0.3f, 1.1f, 40f), madeira);
            Caixa("cerca_horta", pai, new Vector3(-30f, 0.55f,  12f), new Vector3(0.3f, 1.1f, 18f), madeira);

            // Limite do mundo: o jogador nao sai da cena por nenhum lado.
            Caixa("limite_oeste", pai, new Vector3(-59.5f, 1.5f, 0f), new Vector3(1f, 3f, MundoZ), pedra);
            Caixa("limite_leste", pai, new Vector3( 59.5f, 1.5f, 0f), new Vector3(1f, 3f, MundoZ), pedra);
            Caixa("limite_sul",   pai, new Vector3(0f, 1.5f, -89.5f), new Vector3(MundoX, 3f, 1f), pedra);
            Caixa("limite_norte", pai, new Vector3(0f, 1.5f,  89.5f), new Vector3(MundoX, 3f, 1f), pedra);
        }

        static void Rua(string nome, Transform pai, Vector3 centro, Vector2 tamanho, Material m)
        {
            Caixa(nome, pai, new Vector3(centro.x, TopoPiso - 0.05f, centro.z), new Vector3(tamanho.x, 0.1f, tamanho.y), m);
        }

        // ---------------------------------------------------------------- casas e estruturas publicas

        static void Construcoes(Transform pai, Material madeira, Material terra, Material pedra, Material telhado)
        {
            // Tres casas acessiveis, na fila sul, porta (+Z local) virada para a rua das casas.
            // Parede de taipa (terra) e piso/porta de madeira: a porta precisa destacar da parede na foto.
            CasaAcessivel("casa_familia", new Vector3(-22f, 0f, -48.5f), 0f, 10f, 9f,
                          "Entrar na casa da familia", pai, terra, madeira);
            CasaAcessivel("casa_nilo", new Vector3(10f, 0f, -48.5f), 0f, 9f, 9f,
                          "Entrar na casa de Nilo", pai, terra, madeira);
            CasaAcessivel("casa_sera", new Vector3(26f, 0f, -48.5f), 0f, 9f, 9f,
                          "Entrar na casa de Sera", pai, terra, madeira);

            // Tres estruturas publicas em volta da praca, porta virada para ela / para a estrada.
            EstruturaPublica("ferraria", new Vector3(20f, 0f, 10f), -90f, 11f, 9f, 5f, pai, pedra, telhado, madeira);
            EstruturaPublica("ervanaria", new Vector3(-20f, 0f, 12f), 90f, 9f, 8f, 4f, pai, madeira, telhado, pedra);
            EstruturaPublica("posto_guarda", new Vector3(12f, 0f, 34f), -90f, 8f, 7f, 4.5f, pai, pedra, telhado, madeira);
        }

        /// <summary>Casa com interior: piso, quatro paredes com vao de porta na fachada (+Z local), verga e a folha
        /// da porta aberta, encostada na fachada, com SimpleInteractable. O marcador de entrada e o ancora de
        /// mesmo id, em "Ancoras/&lt;id&gt;".
        /// ponytail: sem telhado. A camera em terceira pessoa nao cabe dentro de uma caixa fechada; telhado e
        /// camera de interior entram juntos no T013.</summary>
        static void CasaAcessivel(string id, Vector3 pos, float yaw, float largura, float profundidade,
                                  string promptPorta, Transform pai, Material parede, Material piso)
        {
            Transform raiz = Vazio(id, pai).transform;
            raiz.SetPositionAndRotation(pos, Quaternion.Euler(0f, yaw, 0f));

            float xParede = (largura - EspessuraParede) * 0.5f;
            float zParede = (profundidade - EspessuraParede) * 0.5f;
            float meiaAltura = AlturaParede * 0.5f;

            Caixa("piso", raiz, new Vector3(0f, TopoPiso - 0.1f, 0f), new Vector3(largura, 0.2f, profundidade), piso);
            Caixa("parede_fundo", raiz, new Vector3(0f, meiaAltura, -zParede),
                  new Vector3(largura, AlturaParede, EspessuraParede), parede);
            Caixa("parede_esq", raiz, new Vector3(-xParede, meiaAltura, 0f),
                  new Vector3(EspessuraParede, AlturaParede, profundidade), parede);
            Caixa("parede_dir", raiz, new Vector3(xParede, meiaAltura, 0f),
                  new Vector3(EspessuraParede, AlturaParede, profundidade), parede);

            float trecho = (largura - VaoPorta) * 0.5f;          // fachada = dois trechos + vao no meio
            float xTrecho = (VaoPorta + trecho) * 0.5f;
            Caixa("parede_frente_esq", raiz, new Vector3(-xTrecho, meiaAltura, zParede),
                  new Vector3(trecho, AlturaParede, EspessuraParede), parede);
            Caixa("parede_frente_dir", raiz, new Vector3(xTrecho, meiaAltura, zParede),
                  new Vector3(trecho, AlturaParede, EspessuraParede), parede);
            Caixa("verga", raiz, new Vector3(0f, (AlturaPorta + AlturaParede) * 0.5f, zParede),
                  new Vector3(VaoPorta, AlturaParede - AlturaPorta, EspessuraParede), parede);

            GameObject porta = Caixa("porta", raiz,
                                     new Vector3(VaoPorta * 0.5f + 0.9f, 1.05f, zParede + 0.25f),
                                     new Vector3(1.7f, 2.1f, 0.12f), piso);
            porta.AddComponent<SimpleInteractable>().prompt = promptPorta;
        }

        /// <summary>Estrutura publica: bloco macico com telhado e moldura de porta virada para a praca.
        /// ponytail: sem interior. O requisito de interior do dossie (secao J) esta nas tres casas acessiveis;
        /// ferraria, ervanaria e posto so precisam de silhueta e de uma porta para ancorar NPC e missao.</summary>
        static void EstruturaPublica(string id, Vector3 pos, float yaw, float largura, float profundidade,
                                     float altura, Transform pai, Material parede, Material telhado, Material madeira)
        {
            Transform raiz = Vazio(id, pai).transform;
            raiz.SetPositionAndRotation(pos, Quaternion.Euler(0f, yaw, 0f));

            Caixa("corpo", raiz, new Vector3(0f, altura * 0.5f, 0f), new Vector3(largura, altura, profundidade), parede);
            Caixa("telhado", raiz, new Vector3(0f, altura + 0.2f, 0f),
                  new Vector3(largura + 1f, 0.4f, profundidade + 1f), telhado);
            Caixa("moldura_porta", raiz, new Vector3(0f, AlturaPorta * 0.5f, profundidade * 0.5f + 0.06f),
                  new Vector3(VaoPorta + 0.4f, AlturaPorta, 0.12f), madeira);
        }

        // ---------------------------------------------------------------- poco, mural e adornos

        static void Cenario(Transform pai, Material pedra, Material madeira)
        {
            // Interagivel de prova 1: poco no centro da praca.
            GameObject poco = Primitiva(PrimitiveType.Cylinder, "poco", pai, new Vector3(0f, 0.5f, 0f),
                                        new Vector3(3.2f, 0.5f, 3.2f), pedra);
            poco.AddComponent<SimpleInteractable>().prompt = "Tirar agua do poco";

            // Interagivel de prova 2: mural de avisos, virado para o ancora mural_avisos.
            Transform mural = Vazio("mural_avisos", pai, new Vector3(-6f, 0f, -9f)).transform;
            Caixa("poste", mural, new Vector3(0f, 1.1f, 0f), new Vector3(0.25f, 2.2f, 0.25f), madeira);
            GameObject tabua = Caixa("tabua", mural, new Vector3(0f, 2f, 0.1f), new Vector3(2.4f, 1.4f, 0.15f), madeira);
            tabua.AddComponent<SimpleInteractable>().prompt = "Ler o mural de avisos";

            // Adornos fora da area das construcoes (ferraria ocupa x 15,5..24,5 / z 4,5..15,5; ervanaria x -24..-16 / z 7,5..16,5).
            Primitiva(PrimitiveType.Cylinder, "barril_1", pai, new Vector3(16.5f, 0.45f, 3.2f),
                      new Vector3(0.9f, 0.45f, 0.9f), madeira);
            Primitiva(PrimitiveType.Cylinder, "barril_2", pai, new Vector3(17.6f, 0.45f, 2.6f),
                      new Vector3(0.9f, 0.45f, 0.9f), madeira);
            Caixa("pilha_lenha", pai, new Vector3(-17f, 0.5f, 19f), new Vector3(2.5f, 1f, 1.2f), madeira);
        }

        // ---------------------------------------------------------------- bosque

        /// <summary>Borda do Bosque dos Sussurros: duas barreiras solidas na linha z=62 com um vao de 6 m no meio
        /// (o ancora entrada_bosque), e uma clareira fechada logo atras. As arvores decoram; quem barra o jogador
        /// sao as barreiras - assim o limite nao depende do espacamento das copas.</summary>
        static void Bosque(Transform pai, Material folhagem, Material tronco)
        {
            const float MeioX = 59f;
            float meioTrecho = (MeioX - MeiaAberturaBosque) * 0.5f;
            float xTrecho = MeiaAberturaBosque + meioTrecho;
            Caixa("barreira_oeste", pai, new Vector3(-xTrecho, 1.5f, ZLinhaBosque), new Vector3(meioTrecho * 2f, 3f, 2f), folhagem);
            Caixa("barreira_leste", pai, new Vector3( xTrecho, 1.5f, ZLinhaBosque), new Vector3(meioTrecho * 2f, 3f, 2f), folhagem);

            float meioZClareira = (ZLinhaBosque + ZFundoClareira) * 0.5f;
            float fundoZ = ZFundoClareira - ZLinhaBosque;
            Caixa("clareira_oeste", pai, new Vector3(-MeiaLarguraClareira, 1.5f, meioZClareira), new Vector3(2f, 3f, fundoZ), folhagem);
            Caixa("clareira_leste", pai, new Vector3( MeiaLarguraClareira, 1.5f, meioZClareira), new Vector3(2f, 3f, fundoZ), folhagem);
            Caixa("clareira_fundo", pai, new Vector3(0f, 1.5f, ZFundoClareira), new Vector3(MeiaLarguraClareira * 2f, 3f, 2f), folhagem);

            // ponytail: dispersao pseudoaleatoria com semente fixa - a cena sai igual toda vez, sem arquivo de layout.
            var rng = new System.Random(20260928);
            for (float x = -MeioX; x <= MeioX; x += 5f)
            {
                if (Mathf.Abs(x) < MeiaAberturaBosque + 1.5f) continue; // deixa o vao da entrada livre
                Arvore(pai, new Vector3(x + Entre(rng, -1f, 1f), 0f, ZLinhaBosque + Entre(rng, -1f, 2f)),
                       Entre(rng, 5f, 8f), Entre(rng, 2f, 3f), tronco, folhagem);
            }
            for (int i = 0; i < 30; i++)
            {
                float x = Entre(rng, -MeioX, MeioX);
                float z = Entre(rng, ZLinhaBosque + 6f, 86f);
                if (Mathf.Abs(x) < MeiaLarguraClareira + 2f && z < ZFundoClareira + 2f) continue; // clareira fica limpa
                Arvore(pai, new Vector3(x, 0f, z), Entre(rng, 5f, 9f), Entre(rng, 2f, 3.2f), tronco, folhagem);
            }
        }

        static void Arvore(Transform pai, Vector3 pos, float altura, float raio, Material tronco, Material folhagem)
        {
            Transform raiz = Vazio("arvore", pai, pos).transform;
            Primitiva(PrimitiveType.Cylinder, "tronco", raiz, new Vector3(0f, altura * 0.5f, 0f),
                      new Vector3(0.5f, altura * 0.5f, 0.5f), tronco);
            GameObject copa = Primitiva(PrimitiveType.Sphere, "copa", raiz, new Vector3(0f, altura + raio * 0.4f, 0f),
                                        new Vector3(raio * 2f, raio * 1.7f, raio * 2f), folhagem);
            Object.DestroyImmediate(copa.GetComponent<Collider>()); // ponytail: copa so decora; o tronco ja basta
        }

        static float Entre(System.Random rng, float a, float b) { return a + (float)rng.NextDouble() * (b - a); }

        // ---------------------------------------------------------------- utilidades

        static GameObject Vazio(string nome, Transform pai, Vector3 pos = default(Vector3))
        {
            var go = new GameObject(nome);
            if (pai != null) go.transform.SetParent(pai, false);
            go.transform.localPosition = pos;
            return go;
        }

        static GameObject Primitiva(PrimitiveType tipo, string nome, Transform pai, Vector3 pos, Vector3 escala, Material m)
        {
            GameObject go = GameObject.CreatePrimitive(tipo);
            go.name = nome;
            if (pai != null) go.transform.SetParent(pai, false);
            go.transform.localPosition = pos;
            go.transform.localScale = escala;
            go.GetComponent<Renderer>().sharedMaterial = m;
            return go;
        }

        static GameObject Caixa(string nome, Transform pai, Vector3 pos, Vector3 tamanho, Material m)
        {
            return Primitiva(PrimitiveType.Cube, nome, pai, pos, tamanho, m);
        }

        static void Remover(string nome) { Object.DestroyImmediate(Achar(nome)); }

        static Color Cor(int r, int g, int b) { return new Color(r / 255f, g / 255f, b / 255f); }

        static void RegistrarNoBuildSettings()
        {
            var lista = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            if (lista.Exists(s => s.path == ScenePath)) return;
            lista.Add(new EditorBuildSettingsScene(ScenePath, true));
            EditorBuildSettings.scenes = lista.ToArray();
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

        // MEDIDAS DE PROJETO (a caminhada de ponta a ponta que o dossie secao L pede)
        // Terreno 120 x 180 m; limite navegavel em X=+-59,5 e Z=+-89,5. Rua principal: z=-72 ate z=62 = 134 m.
        // Velocidades do T002: MotionSolver.VelocidadeCaminhadaPadrao 2,2 m/s e VelocidadeCorridaPadrao 4,8 m/s.
        // 1) Linha reta portao_sul (0,-70) -> entrada_bosque (0,60): 130 m => 130/2,2 = 59 s andando (27 s correndo).
        // 2) Rota real com as paradas: portao_sul -> casa_familia (-22,-43) = 35 m; -> praca_centro (0,-4) = 45 m;
        //    -> ferraria (14,10) = 20 m; -> entrada_bosque (0,60) = 52 m. Total 152 m => 152/2,2 = 69 s.
        // 3) Volta completa pelas tres casas e pelas tres estruturas: ~245 m => ~111 s.
        // Ou seja, ~60-70 s de ponta a ponta, dentro da faixa pedida, com a vila densa o bastante para nao
        // virar corredor vazio. Sem NavMesh por enquanto: nada aqui navega sozinho. Quando T007 puser rotina de
        // NPC, um NavMeshSurface assado sobre este greybox passa a fazer falta - e sai barato, porque o chao e
        // plano e toda construcao e caixa.
    }
}
