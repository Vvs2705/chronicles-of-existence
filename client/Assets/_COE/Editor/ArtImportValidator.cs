using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using B = UnityEngine.HumanBodyBones;
using Object = UnityEngine.Object;

namespace COE.EditorTools
{
    /// <summary>Medidas comuns do PIPELINE.md secao 11, no espaco do GameObject raiz do modelo instanciado.</summary>
    public sealed class ArtMedidas
    {
        public Vector3 Min = Vector3.positiveInfinity, Max = Vector3.negativeInfinity; // caixa dos vertices do LOD0 (sem _col)
        public int Vertices, Tris, OssosSkin;
        public bool Humano;                                                                 // Animator com Avatar humano
        public readonly Dictionary<HumanBodyBones, Vector3> Ossos = new Dictionary<HumanBodyBones, Vector3>();
        public readonly List<Vector3> PeEsquerdo = new List<Vector3>();                    // vertices com peso >= 0,5 em LeftFoot/LeftToes
        public float H { get { return Max.y - Min.y; } }
    }

    /// <summary>Validador de import de arte: regras V01-V27 do docs/arte/PIPELINE.md secao 11 em cada
    /// Assets/_COE/Art/&lt;Categoria&gt;/&lt;id&gt;/, Anim/&lt;base&gt;/ (so V05, V18-V20) e o placeholder Art/Humanoid como
    /// avatar_crianca5 (so as regras [P]). Numeros e regras puras: ArtRules (ArtImportValidatorRules.cs).
    /// Menu COE / Validar arte. Batch: Unity -batchmode -projectPath client -executeMethod
    /// COE.EditorTools.ArtImportValidator.RunBatch -logFile - (sai com 1 se houver FAIL). V27 e git, fica UNKNOWN aqui.</summary>
    public static class ArtImportValidator
    {
        public const string ArtDir = "Assets/_COE/Art";
        public const string PlaceholderId = "avatar_crianca5";

        [MenuItem("COE/Validar arte")]
        public static void ValidarMenu() { Log(ValidarTudo()); }

        public static void RunBatch()
        {
            int codigo = 1;
            try
            {
                List<ArtCheck> r = ValidarTudo();
                Log(r);
                codigo = r.Any(c => c.Status == ArtStatus.Fail) ? 1 : 0;
            }
            catch (Exception e) { Debug.LogException(e); }
            if (Application.isBatchMode) EditorApplication.Exit(codigo);
        }

        public static List<ArtCheck> ValidarTudo()
        {
            var r = new List<ArtCheck>();
            if (Directory.Exists(HumanoidSetup.Dir)) r.AddRange(ValidarPasta(HumanoidSetup.Dir, "Avatar", PlaceholderId, soP: true));
            foreach (string cat in ArtRules.Categorias.Concat(new[] { "Anim" }))
            {
                string raiz = ArtDir + "/" + cat;
                if (!Directory.Exists(raiz)) continue;
                foreach (string d in Directory.GetDirectories(raiz).OrderBy(x => x, StringComparer.Ordinal))
                {
                    string pasta = d.Replace('\\', '/');
                    r.AddRange(ValidarPasta(pasta, cat, Path.GetFileName(pasta)));
                }
            }
            ArtCheck proibidos = ArtRules.V03Proibidos(Arquivos(ArtDir));
            proibidos.Pasta = ArtDir;
            r.Add(proibidos);
            return r;
        }

        public static string Relatorio(List<ArtCheck> r)
        {
            var sb = new StringBuilder("ArtImportValidator (docs/arte/PIPELINE.md secao 11):");
            foreach (ArtStatus s in Enum.GetValues(typeof(ArtStatus))) sb.Append(' ').Append(s.ToString().ToUpperInvariant()).Append('=').Append(r.Count(c => c.Status == s));
            foreach (ArtCheck c in r) sb.Append('\n').Append(c);
            return sb.ToString();
        }

        static void Log(List<ArtCheck> r)
        {
            string rel = Relatorio(r);
            if (r.Any(c => c.Status == ArtStatus.Fail)) Debug.LogError(rel);
            else if (r.Any(c => c.Status != ArtStatus.Pass)) Debug.LogWarning(rel);
            else Debug.Log(rel);
        }

        /// <summary>Roda as regras numa pasta. soP = so as regras [P] (placeholder). categoria "Anim" = pasta de clips.</summary>
        public static List<ArtCheck> ValidarPasta(string pasta, string categoria, string id, bool soP = false)
        {
            var r = new List<ArtCheck>();
            string[] arquivos = Arquivos(pasta);
            string[] fbx = arquivos.Where(f => f.EndsWith(".fbx", StringComparison.OrdinalIgnoreCase)).ToArray();

            if (categoria == "Anim")
            {
                r.Add(ArtRules.V05(id));
                string modeloBase = Modelo(Arquivos(ArtDir + "/Avatar/" + id).Where(f => f.EndsWith(".fbx", StringComparison.OrdinalIgnoreCase)).ToArray(), "Avatar");
                ComInstancia(modeloBase, go => Clips(r, fbx, go));
                return Fechar(r, pasta);
            }

            if (!soP)
            {
                r.Add(ArtRules.V01(pasta));
                r.Add(ArtRules.V02(categoria, id));
                r.Add(ArtRules.V03(id, arquivos));
                r.Add(ArtRules.V04(LerProveniencia(), id));
                if (ArtRules.EhHumano(categoria)) r.Add(ArtRules.V05(id));
            }
            r.Add(V06(fbx));

            string modelo = Modelo(fbx, categoria);
            ComInstancia(modelo, go =>
            {
                if (go == null)
                    foreach (string v in RegrasQueUsamModelo(categoria, soP))
                        r.Add(ArtRules.Desconhecido(v, "sem modelo: .fbx com 'model' no nome" + (ArtRules.EhHumano(categoria) ? "" : " ou unico .fbx da pasta")));
                else RegrasDoModelo(r, pasta, categoria, id, soP, modelo, arquivos, go);
                Clips(r, fbx, go);
            });

            if (!soP)
            {
                // ponytail: V12 fica UNKNOWN ate o AurenSceneBuilder publicar os footprints como tabela (PIPELINE secao 12, item 4).
                if (categoria == "Estrutura" && ArtRules.EhGreybox(id))
                    r.Add(ArtRules.Desconhecido("V12", "footprint do greybox ainda nao e publico (argumentos de CasaAcessivel/EstruturaPublica)"));
                // ponytail: V27 e do git, nao do Unity; vira hook de pre-commit/CI (PIPELINE secao 12).
                r.Add(ArtRules.Desconhecido("V27", "fora do Unity: git check-attr filter -- <arquivo> deve dar lfs para .fbx/.png/.tga"));
            }
            return Fechar(r, pasta);
        }

        static List<ArtCheck> Fechar(List<ArtCheck> r, string pasta)
        {
            foreach (ArtCheck c in r) c.Pasta = pasta;
            return r.OrderBy(c => c.Regra, StringComparer.Ordinal).ToList();
        }

        /// <summary>Regras que dependem do modelo; lista usada so para marcar UNKNOWN quando ele falta.</summary>
        static IEnumerable<string> RegrasQueUsamModelo(string categoria, bool soP)
        {
            var v = new List<string> { "V07", "V08" };
            if (categoria != "Vfx") v.AddRange(new[] { "V09", "V10" });
            if (ArtRules.EhHumano(categoria)) v.AddRange(new[] { "V11", "V13", "V14", "V15", "V16", "V17" });
            v.Add("V21");
            if (!soP) v.AddRange(new[] { "V23", "V24", "V25", "V26" });
            return v;
        }

        static void RegrasDoModelo(List<ArtCheck> r, string pasta, string categoria, string id, bool soP, string modelo, string[] arquivos, GameObject go)
        {
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(modelo);
            var mi = AssetImporter.GetAtPath(modelo) as ModelImporter;
            Transform raiz = asset.transform;
            r.Add(ArtRules.V07(raiz.localPosition, raiz.localRotation, raiz.localScale));

            ArtMedidas m = Medir(go);
            bool temV = m.Vertices > 0;
            const string semVertice = "nenhum vertice no LOD0";
            r.Add(temV ? ArtRules.V08(m.Min.y) : ArtRules.Desconhecido("V08", semVertice));
            if (categoria != "Vfx")
            {
                r.Add(temV ? ArtRules.V09(categoria, id, m.H) : ArtRules.Desconhecido("V09", semVertice));
                r.Add(temV ? ArtRules.V10(categoria, m.Min, m.Max) : ArtRules.Desconhecido("V10", semVertice));
            }
            Vector3 d = temV ? m.Max - m.Min : Vector3.zero;
            ArtOrcamento orc = ArtRules.Orcamento(categoria, id, Mathf.Max(d.x, d.y, d.z));

            if (ArtRules.EhHumano(categoria))
            {
                Vector3[] p = Pontos(m, B.LeftUpperLeg, B.RightUpperLeg, B.LeftFoot);
                r.Add(p != null ? ArtRules.V11(p[0], p[1], p[2], m.PeEsquerdo) : ArtRules.Desconhecido("V11", "sem LeftUpperLeg/RightUpperLeg/LeftFoot no Animator"));
                p = Pontos(m, B.LeftUpperLeg, B.Neck);
                r.Add(p != null && temV ? ArtRules.V13(categoria, id, m.Min.y, m.Max.y, p[0].y, p[1].y) : ArtRules.Desconhecido("V13", "sem LeftUpperLeg/Neck ou sem vertice"));
                r.Add(V14(mi, modelo));
                string[] faltam = ArtRules.Ossos19.Where(b => !m.Ossos.ContainsKey(b)).Select(b => b.ToString()).ToArray();
                r.Add(!m.Humano ? ArtRules.Desconhecido("V15", "sem Animator com Avatar humano (ver V14)")
                    : new ArtCheck("V15", faltam.Length == 0 ? ArtStatus.Pass : ArtStatus.Fail, faltam.Length == 0 ? "19 ossos mapeados" : "GetBoneTransform nulo: " + string.Join(", ", faltam)));
                r.Add(mi != null ? ArtRules.V16(orc, m.OssosSkin, mi.maxBonesPerVertex) : ArtRules.Desconhecido("V16", "sem ModelImporter"));
                p = Pontos(m, B.LeftUpperArm, B.LeftLowerArm, B.RightUpperArm, B.RightLowerArm);
                r.Add(p != null ? ArtRules.V17(p[0], p[1], p[2], p[3]) : ArtRules.Desconhecido("V17", "sem ossos do braco no Animator"));
            }
            r.Add(ArtRules.V21(orc, m.Tris));
            if (soP) return;

            if (orc.LodRazoes != null) r.Add(ArtRules.V22(orc, TrisPorLod(go)));
            string prefab = arquivos.FirstOrDefault(f => f.EndsWith(".prefab", StringComparison.Ordinal));
            GameObject alvoCol = prefab != null ? AssetDatabase.LoadAssetAtPath<GameObject>(prefab) : null;
            r.Add(V23(categoria, alvoCol != null ? alvoCol : asset)); // V23 le o prefab da pasta; sem prefab, o FBX

            Material[] mats = asset.GetComponentsInChildren<Renderer>(true).Where(x => !ArtRules.EhColisao(x.name))
                .SelectMany(x => x.sharedMaterials).Distinct().ToArray();
            r.Add(ArtRules.V24(pasta, categoria, orc, mats.Select(x => x == null ? ((string)null, (string)null)
                : (AssetDatabase.GetAssetPath(x), x.shader != null ? x.shader.name : null)).ToList()));
            List<ArtTextura> tex = mats.Where(x => x != null).SelectMany(x => x.GetTexturePropertyNames().Select(n => x.GetTexture(n)))
                .Where(t => t != null).Distinct().Select(Textura).ToList();
            r.Add(ArtRules.V25(orc, tex));
            r.Add(ArtRules.V26(tex));
        }

        /// <summary>Mede o modelo instanciado (secao 11, "Medidas comuns"). Fora do Play Mode o Animator nao avalia clip:
        /// a pose e a de repouso do FBX, que e a de bind.</summary>
        public static ArtMedidas Medir(GameObject go)
        {
            var m = new ArtMedidas();
            Matrix4x4 paraRaiz = go.transform.worldToLocalMatrix;
            Animator anim = go.GetComponent<Animator>();
            Transform pe = null, dedos = null;
            m.Humano = anim != null && anim.avatar != null && anim.avatar.isHuman;
            if (m.Humano)
            {
                anim.Rebind(); // amarra avatar -> Transforms fora do Play Mode (GetBoneTransform); nao toca na pose
                foreach (HumanBodyBones b in ArtRules.Ossos19)
                {
                    Transform t = anim.GetBoneTransform(b);
                    if (t != null) m.Ossos[b] = paraRaiz.MultiplyPoint3x4(t.position);
                }
                pe = anim.GetBoneTransform(B.LeftFoot);
                dedos = anim.GetBoneTransform(B.LeftToes);
            }

            var ossosSkin = new HashSet<Transform>();
            foreach (Renderer rd in RenderersLod0(go))
            {
                Mesh fonte = MalhaDe(rd);
                if (fonte == null) continue;
                var smr = rd as SkinnedMeshRenderer;
                Mesh malha = fonte;
                if (smr != null) { malha = new Mesh(); smr.BakeMesh(malha, true); } // useScale: vertices no espaco local COM escala
                Matrix4x4 paraRaizDoRenderer = paraRaiz * rd.transform.localToWorldMatrix;
                Vector3[] v = malha.vertices;
                BoneWeight[] w = smr != null ? fonte.boneWeights : new BoneWeight[0];
                Transform[] bones = smr != null ? smr.bones : new Transform[0];
                for (int i = 0; i < v.Length; i++)
                {
                    Vector3 p = paraRaizDoRenderer.MultiplyPoint3x4(v[i]);
                    m.Min = Vector3.Min(m.Min, p);
                    m.Max = Vector3.Max(m.Max, p);
                    if (pe != null && i < w.Length && PesoEm(w[i], bones, pe, dedos) >= ArtRules.PesoPeMin) m.PeEsquerdo.Add(p);
                }
                m.Vertices += v.Length;
                m.Tris += Tris(fonte);
                foreach (Transform b in bones) if (b != null) ossosSkin.Add(b);
                if (smr != null) Object.DestroyImmediate(malha);
            }
            m.OssosSkin = ossosSkin.Count;
            return m;
        }

        static float PesoEm(BoneWeight w, Transform[] bones, Transform a, Transform b)
        {
            return (Eh(bones, w.boneIndex0, a, b) ? w.weight0 : 0f) + (Eh(bones, w.boneIndex1, a, b) ? w.weight1 : 0f)
                 + (Eh(bones, w.boneIndex2, a, b) ? w.weight2 : 0f) + (Eh(bones, w.boneIndex3, a, b) ? w.weight3 : 0f);
        }

        static bool Eh(Transform[] bones, int i, Transform a, Transform b)
        {
            return i >= 0 && i < bones.Length && bones[i] != null && (bones[i] == a || (b != null && bones[i] == b));
        }

        static Vector3[] Pontos(ArtMedidas m, params HumanBodyBones[] ossos)
        {
            var p = new Vector3[ossos.Length];
            for (int i = 0; i < ossos.Length; i++) if (!m.Ossos.TryGetValue(ossos[i], out p[i])) return null;
            return p;
        }

        // ponytail: LOD0 = LODs[0] do LODGroup da raiz (o importador cria a partir de _LODn); sem LODGroup, todos os Renderers.
        static IEnumerable<Renderer> RenderersLod0(GameObject go)
        {
            LODGroup g = go.GetComponent<LODGroup>();
            IEnumerable<Renderer> rs = g != null && g.lodCount > 0 ? g.GetLODs()[0].renderers : go.GetComponentsInChildren<Renderer>();
            return rs.Where(x => x != null && !ArtRules.EhColisao(x.name));
        }

        static int[] TrisPorLod(GameObject go)
        {
            LODGroup g = go.GetComponent<LODGroup>();
            if (g == null) return null;
            return g.GetLODs().Select(l => l.renderers.Where(x => x != null && !ArtRules.EhColisao(x.name))
                .Select(MalhaDe).Where(x => x != null).Sum(x => Tris(x))).ToArray();
        }

        static Mesh MalhaDe(Renderer rd)
        {
            var smr = rd as SkinnedMeshRenderer;
            if (smr != null) return smr.sharedMesh;
            MeshFilter mf = rd.GetComponent<MeshFilter>();
            return mf != null ? mf.sharedMesh : null;
        }

        static int Tris(Mesh mesh)
        {
            int t = 0;
            for (int i = 0; i < mesh.subMeshCount; i++)
                if (mesh.GetTopology(i) == MeshTopology.Triangles) t += (int)(mesh.GetIndexCount(i) / 3);
            return t;
        }

        static ArtCheck V06(string[] fbx)
        {
            if (fbx.Length == 0) return ArtRules.Desconhecido("V06", "nenhum .fbx na pasta");
            var ruins = new List<string>();
            foreach (string f in fbx)
            {
                var mi = AssetImporter.GetAtPath(f) as ModelImporter;
                if (mi == null) ruins.Add(Path.GetFileName(f) + " sem ModelImporter (nao importado)");
                else if (!Mathf.Approximately(mi.globalScale, 1f) || !mi.useFileScale)
                    ruins.Add(Path.GetFileName(f) + " globalScale=" + ArtRules.F(mi.globalScale) + " useFileScale=" + mi.useFileScale);
            }
            return new ArtCheck("V06", ruins.Count == 0 ? ArtStatus.Pass : ArtStatus.Fail,
                ruins.Count == 0 ? fbx.Length + " FBX com globalScale 1 e useFileScale" : string.Join("; ", ruins));
        }

        static ArtCheck V14(ModelImporter mi, string modelo)
        {
            Avatar av = AssetDatabase.LoadAllAssetsAtPath(modelo).OfType<Avatar>().FirstOrDefault();
            bool ok = mi != null && mi.animationType == ModelImporterAnimationType.Human && av != null && av.isValid && av.isHuman;
            return new ArtCheck("V14", ok ? ArtStatus.Pass : ArtStatus.Fail, "animationType=" + (mi != null ? mi.animationType.ToString() : "?")
                + ", avatar " + (av == null ? "ausente" : "isValid=" + av.isValid + " isHuman=" + av.isHuman));
        }

        static ArtCheck V23(string categoria, GameObject alvo)
        {
            Transform[] cols = alvo.GetComponentsInChildren<Transform>(true).Where(t => ArtRules.EhColisao(t.name)).ToArray();
            if (ArtRules.EhHumano(categoria))
                return new ArtCheck("V23", cols.Length == 0 ? ArtStatus.Pass : ArtStatus.Fail, cols.Length == 0 ? "sem malha _col (capsula vem do codigo)"
                    : "personagem nao leva malha _col: " + string.Join(", ", cols.Select(t => t.name)));
            var ruins = new List<string>();
            foreach (Transform t in cols)
            {
                Renderer rd = t.GetComponent<Renderer>();
                if (rd != null && rd.enabled) ruins.Add(t.name + " com Renderer habilitado");
                MeshCollider mc = t.GetComponent<MeshCollider>();
                MeshFilter mf = t.GetComponent<MeshFilter>();
                Mesh malha = mc != null && mc.sharedMesh != null ? mc.sharedMesh : mf != null ? mf.sharedMesh : null;
                int teto = mc != null && mc.convex ? ArtRules.ColConvexoMax : ArtRules.ColMax;
                if (malha != null && Tris(malha) > teto) ruins.Add(t.name + " " + Tris(malha) + " tris (max " + teto + ")");
            }
            return new ArtCheck("V23", ruins.Count == 0 ? ArtStatus.Pass : ArtStatus.Fail, ruins.Count == 0 ? cols.Length + " malhas _col ok" : string.Join("; ", ruins));
        }

        static ArtTextura Textura(Texture t)
        {
            string caminho = AssetDatabase.GetAssetPath(t);
            var i = new ArtTextura { Nome = string.IsNullOrEmpty(caminho) ? t.name : Path.GetFileNameWithoutExtension(caminho) };
            var ti = AssetImporter.GetAtPath(caminho) as TextureImporter;
            if (ti == null) return i; // Largura 0: V25 reprova
            ti.GetSourceTextureWidthAndHeight(out i.Largura, out i.Altura);
            i.MaxSize = ti.maxTextureSize;
            i.Mipmap = ti.mipmapEnabled;
            i.Srgb = ti.sRGBTexture;
            i.NormalMap = ti.textureType == TextureImporterType.NormalMap;
            return i;
        }

        // ---- clips (V18-V20) ----

        static void Clips(List<ArtCheck> r, string[] fbx, GameObject modelo)
        {
            string[] nomes = fbx.Select(x => Path.GetFileNameWithoutExtension(x)).ToArray();
            HumanoidFiles f = HumanoidMapping.Classify(nomes);
            if (f.Clips.Count == 0 && f.Unmapped.Count == 0) return; // so o modelo: pasta sem clip, V18-V20 nao se aplicam
            r.Add(ArtRules.V18(f));
            var clips = new Dictionary<HumanoidClip, AnimationClip>();
            foreach (KeyValuePair<HumanoidClip, string> kv in f.Clips)
            {
                AnimationClip c = AssetDatabase.LoadAllAssetsAtPath(fbx[Array.IndexOf(nomes, kv.Value)])
                    .OfType<AnimationClip>().FirstOrDefault(a => !a.name.StartsWith("__preview__", StringComparison.Ordinal));
                if (c != null) clips[kv.Key] = c;
            }
            r.Add(V19(clips, modelo != null ? modelo.GetComponent<Animator>() : null));
            r.Add(ArtRules.V20(clips.ToDictionary(kv => kv.Key, kv => kv.Value.length)));
        }

        static ArtCheck V19(Dictionary<HumanoidClip, AnimationClip> clips, Animator anim)
        {
            if (clips.Count == 0) return ArtRules.Desconhecido("V19", "nenhum AnimationClip importado");
            var falhas = new List<string>();
            var avisos = new List<string>();
            var semMedida = new List<string>();
            foreach (KeyValuePair<HumanoidClip, AnimationClip> kv in clips)
            {
                if (Mathf.Abs(kv.Value.frameRate - ArtRules.Fps) > 0.01f) avisos.Add(kv.Key + " a " + ArtRules.F(kv.Value.frameRate) + " FPS");
                if (!HumanoidMapping.Loops(kv.Key)) continue;
                if (!AnimationUtility.GetAnimationClipSettings(kv.Value).loopTime) falhas.Add(kv.Key + " sem loopTime");
                float? d = DiferencaDoLoop(kv.Value, anim);
                if (d == null) semMedida.Add(kv.Key.ToString());
                else if (d.Value > ArtRules.LoopTolGraus) falhas.Add(kv.Key + " fecha o loop com " + ArtRules.F(d.Value) + " graus (max " + ArtRules.F(ArtRules.LoopTolGraus) + ")");
            }
            if (falhas.Count > 0) return new ArtCheck("V19", ArtStatus.Fail, string.Join("; ", falhas.Concat(avisos)));
            if (semMedida.Count > 0) return ArtRules.Desconhecido("V19", "loop nao medido em " + string.Join(", ", semMedida)
                + " (sem modelo humano, algum dos 19 ossos sem mapa - ver V15 - ou a amostragem nao moveu osso)" + (avisos.Count > 0 ? "; " + string.Join("; ", avisos) : ""));
            if (avisos.Count > 0) return new ArtCheck("V19", ArtStatus.Warn, string.Join("; ", avisos));
            return new ArtCheck("V19", ArtStatus.Pass, clips.Count + " clips a 30 FPS; Idle/Run com loopTime e loop fechado");
        }

        /// <summary>Maior diferenca de rotacao local (graus) dos 19 ossos entre t = 0 e o fim do clip, amostrado no modelo.
        /// ponytail: AnimationClip.SampleAnimation fora do Play Mode. Se a amostragem nao mover osso nenhum (API sem efeito
        /// nesta versao), devolve null = UNKNOWN, nunca PASS. Caminho de upgrade: AnimationMode.SampleAnimationClip.</summary>
        static float? DiferencaDoLoop(AnimationClip clip, Animator anim)
        {
            if (anim == null || anim.avatar == null || !anim.avatar.isHuman) return null;
            Transform[] ossos = ArtRules.Ossos19.Select(b => anim.GetBoneTransform(b)).ToArray();
            if (ossos.Any(t => t == null)) return null;
            Quaternion[] antes = ossos.Select(t => t.localRotation).ToArray();
            Quaternion[] inicio = Amostra(clip, anim.gameObject, 0f, ossos);
            Quaternion[] meio = Amostra(clip, anim.gameObject, clip.length * 0.5f, ossos);
            // um pouco antes de length: em t = length um clip com loopTime pode dar a volta e devolver t = 0 (PASS falso)
            Quaternion[] fim = Amostra(clip, anim.gameObject, Mathf.Max(0f, clip.length - 0.001f), ossos);
            if (Mathf.Max(MaiorAngulo(antes, inicio), MaiorAngulo(inicio, meio)) < 0.5f) return null;
            return MaiorAngulo(inicio, fim);
        }

        static Quaternion[] Amostra(AnimationClip clip, GameObject go, float t, Transform[] ossos)
        {
            clip.SampleAnimation(go, t);
            return ossos.Select(o => o.localRotation).ToArray();
        }

        static float MaiorAngulo(Quaternion[] a, Quaternion[] b) { return a.Select((q, i) => Quaternion.Angle(q, b[i])).Max(); }

        // ---- arquivos ----

        static string[] Arquivos(string pasta)
        {
            if (!Directory.Exists(pasta)) return new string[0];
            return Directory.GetFiles(pasta, "*", SearchOption.AllDirectories).Where(f => !f.EndsWith(".meta", StringComparison.OrdinalIgnoreCase))
                .Select(f => f.Replace('\\', '/')).OrderBy(f => f, StringComparer.Ordinal).ToArray();
        }

        /// <summary>Mesma regra do HumanoidSetup: 1o .fbx (ordem alfabetica) com "model" no nome. Prop/Estrutura/Vfx
        /// tambem aceitam o unico .fbx da pasta. null = sem modelo.</summary>
        static string Modelo(string[] fbx, string categoria)
        {
            HumanoidFiles f = HumanoidMapping.Classify(fbx.Select(x => Path.GetFileNameWithoutExtension(x)));
            if (f.Model != null) return fbx.First(x => Path.GetFileNameWithoutExtension(x) == f.Model);
            return !ArtRules.EhHumano(categoria) && fbx.Length == 1 ? fbx[0] : null;
        }

        /// <summary>Instancia o modelo na cena ativa, roda o corpo e destroi. go = null se nao ha modelo.</summary>
        static void ComInstancia(string modelo, Action<GameObject> corpo)
        {
            GameObject asset = modelo != null ? AssetDatabase.LoadAssetAtPath<GameObject>(modelo) : null;
            GameObject go = asset != null ? Object.Instantiate(asset) : null;
            try { corpo(go); }
            finally { if (go != null) Object.DestroyImmediate(go); }
        }

        static string LerProveniencia()
        {
            string p = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "..", "docs", "arte", "PROVENIENCIA.md"));
            return File.Exists(p) ? File.ReadAllText(p) : null;
        }
    }
}
