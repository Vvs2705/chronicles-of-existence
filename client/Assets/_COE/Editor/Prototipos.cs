using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace COE.EditorTools
{
    /// <summary>ADR-0008 — troca de placeholder por prototipo do Tripo3D nos geradores de cena, SEMPRE com fallback: se o
    /// FBX do id existe em Art/Prototipo/, entra o modelo; se nao, fica o greybox/placeholder de hoje.
    ///
    /// CONTRATO dos arquivos: Art/Prototipo/Personagens/&lt;id&gt;/&lt;id&gt;.fbx e Art/Prototipo/Pecas/&lt;id&gt;/&lt;id&gt;.fbx
    /// (import em <see cref="PrototipoImport"/>). Escala e eixo do Tripo sao arbitrarios: <see cref="Instanciar"/> mede
    /// a malha, escala para a altura-alvo da tabela e apoia a base no chao (pivo na base), num objeto "prototipo_&lt;id&gt;".
    /// Colisao nunca vem do modelo: fica a do greybox (renderer desligado), e as pecas soltas ganham uma caixa.
    /// Tudo por tabela (id -> altura, id -> onde): editar aqui e regerar a cena.</summary>
    public static class Prototipos
    {
        public const string Raiz = "Assets/_COE/Art/Prototipo";
        public const string Prefixo = "prototipo_";

        /// <summary>Altura-alvo (m), topo a sola. Criancas e Mara saem de BodyScale; Borin e direcao de arte.</summary>
        static readonly (string Id, float Altura)[] personagens =
        {
            ("protagonista", BodyScale.Crianca5), // o BodyByAge escala aos 8 (Crianca8/Crianca5)
            ("nilo",         BodyScale.Crianca5), // idem, pela capsula do NpcActor
            ("sera",         BodyScale.Crianca5),
            ("mara",         BodyScale.Adulto),
            ("borin",        1.82f),
        };

        static readonly (string Id, float Altura)[] pecas =
        {
            ("casa_familia", 6f),   ("ferraria", 6f),       ("poco", 2.6f),   ("arvore", 7f),
            ("barril", 1f),         ("caixote", 0.6f),      ("cesto", 0.35f), ("lanterna", 0.5f),
            ("arbusto", 1f),        ("simbolo_limiar", 1.5f), ("bigorna", 0.7f), ("banco", 0.5f),
        };

        /// <summary>Peca que TOMA O LUGAR de um objeto do greybox (caminho sob "Auren"). Footprint (largura X, fundo Z, no
        /// referencial do objeto) limita a escala: a arte nao passa da planta que os percursos varrem (PIPELINE §3.2).
        /// Os numeros de casa e ferraria sao os do AurenSceneBuilder.Construcoes.</summary>
        static readonly (string Caminho, string Id, Vector2 Footprint)[] trocas =
        {
            ("Construcoes/casa_familia", "casa_familia", new Vector2(10f, 9f)),
            ("Construcoes/ferraria",     "ferraria",     new Vector2(11f, 9f)),
            ("Cenario/poco",             "poco",         new Vector2(3.2f, 3.2f)),
            ("Cenario/barril_1",         "barril",       new Vector2(0.9f, 0.9f)),
            ("Cenario/barril_2",         "barril",       new Vector2(0.9f, 0.9f)),
            (AurenSceneBuilder.NomeSimbolo, "simbolo_limiar", Vector2.zero), // o SaltoGatilho continua no objeto do greybox
        };

        /// <summary>Peca solta em "Auren/Cenario" (posicao local = mundo). Colisor = caixa do tamanho medido.
        /// Toda posicao fica a >= 1,5 m de todo percurso do T008 (PrototipoTests confere pela tabela).</summary>
        public struct Pose
        {
            public readonly string Id;
            public readonly Vector3 Pos;
            public readonly float Yaw;
            public readonly bool Colisor;
            public Pose(string id, float x, float y, float z, float yaw, bool colisor) { Id = id; Pos = new Vector3(x, y, z); Yaw = yaw; Colisor = colisor; }
        }

        public static readonly Pose[] Soltas =
        {
            new Pose("caixote",  -20.5f, 0f,    19.2f,   15f, true),  // atras da ervanaria, junto da pilha de lenha
            new Pose("caixote",  -21.3f, 0f,    20.5f,  -20f, true),
            new Pose("caixote",   33f,   0f,   -45f,     30f, true),  // ao lado de casa_sera
            new Pose("lanterna", -12.5f, 0.02f, -12.5f,   0f, false), // quatro cantos da praca (piso a 2 cm)
            new Pose("lanterna",  12.5f, 0.02f, -12.5f,   0f, false),
            new Pose("lanterna", -12.5f, 0.02f,  12.5f,   0f, false),
            new Pose("lanterna",  12.5f, 0.02f,  12.5f,   0f, false),
            new Pose("lanterna", -24.2f, 0f,   -43.7f,    0f, false), // porta de casa_familia (lado sem folha)
            new Pose("banco",     -9f,   0.02f,  -2f,    90f, true),  // bancos da praca, virados para o poco
            new Pose("banco",      8f,   0.02f, -10f,   -45f, true),
            new Pose("bigorna",   14.3f, 0f,    13.8f,   20f, true),  // na porta da ferraria, fora da vaga de Borin
            new Pose("cesto",    -24.5f, 0.1f, -57.5f,   30f, false), // no canteiro da horta (q03)
            new Pose("arbusto",   -5f,   0f,    59f,      0f, false), // flancos da entrada do bosque
            new Pose("arbusto",    5f,   0f,    59f,     70f, false),
            new Pose("arbusto",   16f,   0f,   -45f,    140f, false),
            new Pose("arbusto",  -28f,   0f,   -44f,    210f, false),
        };

        /// <summary>Cor chapada do placeholder de cada NPC sem prototipo: leitura de quem e quem de longe.</summary>
        static readonly (string Id, Color Cor)[] coresNpc =
        {
            ("mara", Hex(0xE0, 0x7A, 0x5F)), ("daren", Hex(0x3D, 0x5A, 0x80)), ("borin", Hex(0x8D, 0x5A, 0x3B)),
            ("lysa", Hex(0x81, 0xB2, 0x9A)), ("tovin", Hex(0x5C, 0x6B, 0x73)), ("eira", Hex(0xF2, 0xCC, 0x8F)),
            ("nilo", Hex(0x4E, 0xA8, 0xDE)), ("sera", Hex(0xE5, 0x6B, 0x9F)), ("oren", Hex(0x9A, 0x8C, 0x98)),
            ("maelis", Hex(0xB5, 0x83, 0x8D)),
        };

        /// <summary>Teto de triangulos das arvores-prototipo somadas (celular: ~150 000 tris visiveis por quadro,
        /// PIPELINE §4). ponytail: corta a quantidade, nao a malha; o resto do bosque fica greybox. Subir so com LOD
        /// ou arvore decimada no Blender, medindo no aparelho.</summary>
        public const int OrcamentoTrisArvores = 60000;

        /// <summary>Carrega o modelo do id (null = sem prototipo). Troca so em teste.</summary>
        public static Func<string, GameObject> Carregar = CarregarDoDisco;

        public static GameObject CarregarDoDisco(string id) { return AssetDatabase.LoadAssetAtPath<GameObject>(Caminho(id)); }

        public static bool EhPersonagem(string id) { return Array.FindIndex(personagens, p => p.Id == id) >= 0; }

        public static IEnumerable<string> Ids()
        {
            foreach (var p in personagens) yield return p.Id;
            foreach (var p in pecas) yield return p.Id;
        }

        public static string Caminho(string id)
        {
            return Raiz + (EhPersonagem(id) ? "/Personagens/" : "/Pecas/") + id + "/" + id + ".fbx";
        }

        public static float Altura(string id)
        {
            foreach (var p in personagens) if (p.Id == id) return p.Altura;
            foreach (var p in pecas) if (p.Id == id) return p.Altura;
            throw new Exception("Prototipos: id sem altura-alvo '" + id + "'.");
        }

        // ---------------------------------------------------------------- normalizacao (C# puro)

        /// <summary>Escala uniforme que leva a altura medida a altura-alvo, limitada pelo footprint (0 = sem limite).
        /// Malha degenerada (altura ~0) ou alvo invalido: 1.</summary>
        public static float Escala(Vector3 tamanhoMedido, float alturaAlvo, Vector2 footprintMax)
        {
            if (tamanhoMedido.y < 1e-4f || alturaAlvo <= 0f) return 1f;
            float s = alturaAlvo / tamanhoMedido.y;
            if (footprintMax.x > 0f && tamanhoMedido.x > 1e-4f) s = Mathf.Min(s, footprintMax.x / tamanhoMedido.x);
            if (footprintMax.y > 0f && tamanhoMedido.z > 1e-4f) s = Mathf.Min(s, footprintMax.y / tamanhoMedido.z);
            return s;
        }

        /// <summary>Deslocamento do modelo (ja escalado por 'escala' em torno do proprio pivo) para o centro da base da
        /// caixa medida cair na origem: pivo na base, centrado em XZ.</summary>
        public static Vector3 ApoioNaBase(Bounds medido, float escala)
        {
            return new Vector3(-medido.center.x, -medido.min.y, -medido.center.z) * escala;
        }

        public static int QuantasArvores(int trisPorArvore)
        {
            return trisPorArvore <= 0 ? int.MaxValue : Mathf.Max(4, OrcamentoTrisArvores / trisPorArvore);
        }

        // ---------------------------------------------------------------- instancia

        /// <summary>Cria "prototipo_&lt;id&gt;" sob 'pai' (pose local zero; quem chama posiciona) com o modelo dentro,
        /// escalado para a altura-alvo e com a base na origem. A rotacao de raiz do FBX e preservada.</summary>
        public static Transform Instanciar(GameObject fonte, string id, Transform pai, float alturaAlvo, Vector2 footprint)
        {
            var w = new GameObject(Prefixo + id).transform;
            w.SetParent(pai, false);

            GameObject m = PrefabUtility.IsPartOfPrefabAsset(fonte)
                ? (GameObject)PrefabUtility.InstantiatePrefab(fonte, w)
                : Object.Instantiate(fonte, w);
            m.name = id;
            m.transform.localPosition = Vector3.zero;
            Vector3 escala0 = m.transform.localScale;

            Bounds b = Medir(m.transform, w);
            float s = Escala(b.size, alturaAlvo, footprint);
            m.transform.localScale = escala0 * s;
            m.transform.localPosition = ApoioNaBase(b, s);
            return w;
        }

        /// <summary>Caixa de todas as malhas sob 'raiz' no espaco local de 'espaco' (pose de bind para malha com pele).</summary>
        public static Bounds Medir(Transform raiz, Transform espaco)
        {
            bool tem = false;
            var b = new Bounds();
            Matrix4x4 paraEspaco = espaco.worldToLocalMatrix;
            foreach (Renderer r in raiz.GetComponentsInChildren<Renderer>(true))
            {
                Mesh mesh = Malha(r);
                if (mesh == null) continue;
                Matrix4x4 m = paraEspaco * r.transform.localToWorldMatrix;
                Bounds mb = mesh.bounds;
                for (int i = 0; i < 8; i++)
                {
                    var canto = new Vector3((i & 1) == 0 ? -1f : 1f, (i & 2) == 0 ? -1f : 1f, (i & 4) == 0 ? -1f : 1f);
                    Vector3 p = m.MultiplyPoint3x4(mb.center + Vector3.Scale(mb.extents, canto));
                    if (!tem) { b = new Bounds(p, Vector3.zero); tem = true; }
                    else b.Encapsulate(p);
                }
            }
            return b;
        }

        static Mesh Malha(Renderer r)
        {
            var skin = r as SkinnedMeshRenderer;
            if (skin != null) return skin.sharedMesh;
            MeshFilter mf = r.GetComponent<MeshFilter>();
            return mf != null ? mf.sharedMesh : null;
        }

        static int Tris(GameObject fonte)
        {
            long t = 0;
            foreach (Renderer r in fonte.GetComponentsInChildren<Renderer>(true))
            {
                Mesh mesh = Malha(r);
                if (mesh == null) continue;
                for (int i = 0; i < mesh.subMeshCount; i++) t += mesh.GetIndexCount(i) / 3;
            }
            return (int)Math.Min(t, int.MaxValue);
        }

        // ---------------------------------------------------------------- cena

        /// <summary>Player (Bootstrap e Auren): modelo do protagonista como filho direto (o BodyByAge o escala aos 8),
        /// com o Player.controller do placeholder se o avatar for Humanoid. Null = sem prototipo (segue o placeholder).</summary>
        public static GameObject AnexarProtagonista(GameObject player)
        {
            GameObject fonte = Carregar("protagonista");
            if (fonte == null) return null;

            HumanoidSetup.EsconderCapsula(player);
            Transform w = Instanciar(fonte, "protagonista", player.transform, Altura("protagonista"), Vector2.zero);
            bool anima = Animar(w, false);
            CharacterAnimator ca = player.GetComponent<CharacterAnimator>();
            if (ca != null)
            {
                ca.immediate = !anima;   // com clip, o dano sai no OnHitFrame; modelo parado (Generic) golpeia na hora
                EditorUtility.SetDirty(ca);
            }
            return w.gameObject;
        }

        /// <summary>Auren: estruturas, poco, barris e simbolo no lugar do greybox; arvores do bosque; pecas soltas; NPCs.
        /// Chamado no fim do AurenSceneBuilder.Populate. 'mat' e o mesmo do gerador (asset no Build, memoria no teste).</summary>
        public static void AplicarEmAuren(Func<string, Color, Material> mat)
        {
            Transform mundo = AurenSceneBuilder.Achar(AurenSceneBuilder.RaizMundo).transform;
            foreach (var t in trocas) Trocar(mundo.Find(t.Caminho), t.Id, t.Footprint);
            Arvores(mundo.Find("Bosque"));
            PecasSoltas(mundo.Find("Cenario"));
            Npcs(mat);
        }

        static void Trocar(Transform greybox, string id, Vector2 footprint)
        {
            if (greybox == null) return;
            GameObject fonte = Carregar(id);
            if (fonte == null) return;   // fallback: fica o greybox

            Transform w = Instanciar(fonte, id, greybox.parent, Altura(id), footprint);
            Vector3 p = greybox.position;
            w.SetPositionAndRotation(new Vector3(p.x, 0f, p.z), Quaternion.Euler(0f, greybox.eulerAngles.y, 0f));
            Esconder(greybox);
        }

        /// <summary>As arvores mais perto da entrada do bosque (as que o jogador ve de perto) viram prototipo, ate o
        /// orcamento; giro e escala variam pelo indice (deterministico: a cena sai igual toda vez).</summary>
        static void Arvores(Transform bosque)
        {
            if (bosque == null) return;
            GameObject fonte = Carregar("arvore");
            if (fonte == null) return;

            var arvores = new List<Transform>();
            foreach (Transform t in bosque) if (t.name == "arvore") arvores.Add(t);
            Vector3 entrada = AurenSceneBuilder.PosicaoDaAncora("entrada_bosque");
            arvores.Sort((a, b) => (a.position - entrada).sqrMagnitude.CompareTo((b.position - entrada).sqrMagnitude));

            int n = Mathf.Min(arvores.Count, QuantasArvores(Tris(fonte)));
            for (int i = 0; i < n; i++)
            {
                float variacao = 0.85f + 0.3f * Mathf.Repeat(i * 0.618034f, 1f);
                Transform w = Instanciar(fonte, "arvore", bosque, Altura("arvore") * variacao, Vector2.zero);
                w.SetPositionAndRotation(arvores[i].position, Quaternion.Euler(0f, Mathf.Repeat(i * 137.5f, 360f), 0f));
                Esconder(arvores[i]);   // o colisor do tronco fica
            }
        }

        static void PecasSoltas(Transform cenario)
        {
            if (cenario == null) return;
            foreach (Pose p in Soltas)
            {
                GameObject fonte = Carregar(p.Id);
                if (fonte == null) continue;
                Transform w = Instanciar(fonte, p.Id, cenario, Altura(p.Id), Vector2.zero);
                w.localPosition = p.Pos;
                w.localRotation = Quaternion.Euler(0f, p.Yaw, 0f);
                if (!p.Colisor) continue;
                Bounds b = Medir(w, w);
                BoxCollider caixa = w.gameObject.AddComponent<BoxCollider>();
                caixa.center = b.center;
                caixa.size = b.size;
            }
        }

        /// <summary>NPC com prototipo: modelo sob a capsula "Corpo" (desligada), entao crescer aos 8 e sumir (Nilo
        /// ausente) continuam do NpcActor. Sem prototipo: capsula com cor chapada propria.</summary>
        static void Npcs(Func<string, Color, Material> mat)
        {
            Transform raiz = AurenSceneBuilder.Achar(NpcSceneSetup.RaizNpcs).transform;
            foreach (Transform npc in raiz)
            {
                Transform corpo = npc.Find("Corpo");
                if (corpo == null) continue;
                Renderer capsula = corpo.GetComponent<Renderer>();
                GameObject fonte = EhPersonagem(npc.name) ? Carregar(npc.name) : null;
                if (fonte == null)
                {
                    if (capsula != null) capsula.sharedMaterial = mat("COE_Npc_" + npc.name, CorDoNpc(npc.name));
                    continue;
                }

                if (capsula != null) capsula.enabled = false;
                Transform w = Instanciar(fonte, npc.name, corpo, Altura(npc.name), Vector2.zero);
                // Corpo = capsula primitiva (2 m, pivo no centro) escalada por altura/2: o wrapper desfaz a escala da
                // idade de projeto e desce aos pes. Aos 8 o NpcActor escala o Corpo e o modelo cresce junto.
                w.localPosition = Vector3.down;
                w.localScale = Vector3.one / corpo.localScale.y;
                Animar(w, true);
            }
        }

        /// <summary>Liga o Player.controller (clips Humanoid do placeholder retargetam para qualquer avatar Humanoid).
        /// Avatar Generic (rig do Tripo que nao mapeou) ou controller ausente: o modelo fica parado.</summary>
        static bool Animar(Transform w, bool npc)
        {
            Animator a = w.GetComponentInChildren<Animator>();
            if (a == null || a.avatar == null || !a.avatar.isHuman) return false;
            var controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(HumanoidSetup.ControllerPath);
            if (controller == null) return false;
            a.runtimeAnimatorController = controller;
            a.applyRootMotion = false;   // quem move e o codigo
            if (npc)
            {
                a.cullingMode = AnimatorCullingMode.CullCompletely;   // fora da tela, NPC parado nao anima
                // O blend de locomocao tem Run (OnFootstep): sem receptor o Unity acusa erro a cada passo.
                if (a.GetComponent<AnimEventRelay>() == null) a.gameObject.AddComponent<AnimEventRelay>();
            }
            return true;
        }

        static void Esconder(Transform greybox)
        {
            foreach (Renderer r in greybox.GetComponentsInChildren<Renderer>(true)) r.enabled = false;
        }

        public static Color CorDoNpc(string id)
        {
            foreach (var c in coresNpc) if (c.Id == id) return c.Cor;
            return new Color(0.6f, 0.6f, 0.6f);
        }

        static Color Hex(int r, int g, int b) { return new Color(r / 255f, g / 255f, b / 255f); }
    }
}
