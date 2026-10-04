using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace COE.EditorTools
{
    /// <summary>UNKNOWN = sem evidencia para medir; nunca vira PASS (PIPELINE.md secao 11).</summary>
    public enum ArtStatus { Pass, Warn, Fail, Unknown }

    /// <summary>Resultado de uma regra Vnn do docs/arte/PIPELINE.md secao 11 numa pasta de arte.</summary>
    public sealed class ArtCheck
    {
        public string Pasta;
        public readonly string Regra;
        public readonly ArtStatus Status;
        public readonly string Detalhe;

        public ArtCheck(string regra, ArtStatus status, string detalhe) { Regra = regra; Status = status; Detalhe = detalhe; }

        public override string ToString() { return "[" + Status.ToString().ToUpperInvariant() + "] " + Regra + " " + Pasta + ": " + Detalhe; }
    }

    /// <summary>Teto de uma categoria (PIPELINE.md secao 4). Ossos 0 = sem regra; LodRazoes null = LOD nao exigido,
    /// LodRazoes[i] = teto de tris(LOD i+1) / tris(LOD0).</summary>
    public sealed class ArtOrcamento
    {
        public readonly string Nome;
        public readonly int Tris, Materiais, Textura, Ossos;
        public readonly float[] LodRazoes;

        public ArtOrcamento(string nome, int tris, float[] lodRazoes, int materiais, int textura, int ossos)
        {
            Nome = nome; Tris = tris; LodRazoes = lodRazoes; Materiais = materiais; Textura = textura; Ossos = ossos;
        }
    }

    /// <summary>O que V25/V26 leem de uma textura. Largura 0 = sem TextureImporter.</summary>
    public struct ArtTextura
    {
        public string Nome;             // nome do arquivo sem extensao
        public int Largura, Altura, MaxSize;
        public bool Mipmap, Srgb, NormalMap;
    }

    /// <summary>Parte pura do ArtImportValidator: a tabela de numeros do PIPELINE.md e as regras que so comparam numero,
    /// nome e texto. Nao le asset; testavel direto (Tests/Editor/ArtImportValidatorTests.cs).</summary>
    public static class ArtRules
    {
        public static readonly string[] Categorias = { "Avatar", "Npc", "Prop", "Estrutura", "Vfx" };

        // ================= TABELA UNICA (docs/arte/PIPELINE.md). Mudou la, muda aqui. =================

        // secao 3: base -> altura alvo (m) e faixas de proporcao (V13). Alturas do BodyScale; faixas HIPOTESE v0.
        static readonly Dictionary<string, (float Alvo, float PernaMin, float PernaMax, float CabecaMin, float CabecaMax)> bases =
            new Dictionary<string, (float, float, float, float, float)>
            {
                { "avatar_crianca5", (BodyScale.Crianca5, 0.40f, 0.49f, 0.19f, 0.28f) },
                { "avatar_crianca8", (BodyScale.Crianca8, 0.43f, 0.51f, 0.17f, 0.25f) },
                { "avatar_adolescente", (1.60f, 0.46f, 0.54f, 0.14f, 0.21f) }, // 1,60 HIPOTESE v0: nao existe em BodyScale (secao 12)
                { "avatar_adulto", (BodyScale.Adulto, 0.50f, 0.58f, 0.12f, 0.18f) },
            };
        const float AlturaFatorMin = 0.97f, AlturaFatorMax = 1.05f; // secao 3: faixa V09 = alvo x [0,97; 1,05]

        // secao 3.1: NPC -> base (adulto usa as faixas de avatar_adulto; crianca, as de avatar_crianca5) e altura
        static readonly string[] npcAdultos = { "mara", "daren", "borin", "lysa", "tovin", "eira", "oren", "maelis" };
        static readonly string[] npcCriancas = { "nilo", "sera" };
        const float NpcAdultoMin = 1.55f, NpcAdultoMax = 1.95f, NpcCriancaMin = 1.00f, NpcCriancaMax = 1.20f;

        // secao 11 V09: Prop em (0,02; 4,0] m (aberto embaixo), Estrutura em [2,5; 15] m
        const float PropMin = 0.02f, PropMax = 4.0f, EstruturaMin = 2.5f, EstruturaMax = 15f;

        // ---- ORCAMENTO: a UNICA tabela de tetos do validador = PIPELINE.md secao 4, linha a linha (HIPOTESE v0). ----
        // ponytail: Android de faixa media a 30 FPS (ADR-0006), HIPOTESE v0 ate o idealizador definir o aparelho minimo.
        // Quando a secao 4 mudar, troca-se SO este bloco (V16, V21, V22, V24 e V25 leem daqui; os testes nao fixam valor).
        //                                                           Tris LOD0  LOD1.. / LOD0            Mat  Textura  Ossos(0=sem regra)
        static readonly ArtOrcamento orcAvatar      = new ArtOrcamento("Avatar",            15000, null,                     2, 1024, 75);
        static readonly ArtOrcamento orcNpc         = new ArtOrcamento("Npc",                8000, new[] { 0.50f, 0.25f },   2, 1024, 55);
        static readonly ArtOrcamento orcPropPequeno = new ArtOrcamento("Prop pequeno",        500, null,                     1,  256,  0);
        static readonly ArtOrcamento orcPropMedio   = new ArtOrcamento("Prop medio",         2000, new[] { 0.50f },          1,  512,  0);
        static readonly ArtOrcamento orcEstrutura   = new ArtOrcamento("Estrutura",         10000, new[] { 0.50f, 0.25f },   3, 1024,  0);
        static readonly ArtOrcamento orcModulo      = new ArtOrcamento("Estrutura modulo_",  1000, null,                     1, 1024,  0);
        static readonly ArtOrcamento orcVfx         = new ArtOrcamento("Vfx",                 300, null,                     1,  512,  0);
        const float PropPequenoAte = 1.0f;       // secao 4: Prop pequeno = maior dimensao < 1 m; senao medio
        public const int MaxOssosPorVertice = 4; // secao 4: influencias por vertice

        // secao 7.2: duracao de referencia em quadros a 30 FPS (V20 +-30%)
        static readonly Dictionary<HumanoidClip, int> quadrosReferencia = new Dictionary<HumanoidClip, int>
        {
            { HumanoidClip.Idle, 75 }, { HumanoidClip.Run, 21 }, { HumanoidClip.Attack1, 14 }, { HumanoidClip.Attack2, 14 },
            { HumanoidClip.Attack3, 18 }, { HumanoidClip.Dodge, 12 }, { HumanoidClip.Hit, 9 }, { HumanoidClip.Death, 42 },
        };
        public const float Fps = 30f;
        const float DuracaoTol = 0.30f;

        // secao 11: tolerancias de V07, V08, V10, V11, V17, V19; secao 8: tetos de colisao (V23)
        const float PosTol = 0.001f, RotTolGraus = 0.01f, EscalaTol = 0.0001f, ChaoTol = 0.02f;
        const float CentroHumanoMax = 0.10f, CentroFracaoMax = 0.05f, TPoseTolGraus = 10f;
        public const float PesoPeMin = 0.5f, LoopTolGraus = 2f;
        public const int ColConvexoMax = 255, ColMax = 2000;

        // secao 7.1: os 19 ossos obrigatorios
        public static readonly HumanBodyBones[] Ossos19 =
        {
            HumanBodyBones.Hips, HumanBodyBones.Spine, HumanBodyBones.Chest, HumanBodyBones.Neck, HumanBodyBones.Head,
            HumanBodyBones.LeftShoulder, HumanBodyBones.LeftUpperArm, HumanBodyBones.LeftLowerArm, HumanBodyBones.LeftHand,
            HumanBodyBones.RightShoulder, HumanBodyBones.RightUpperArm, HumanBodyBones.RightLowerArm, HumanBodyBones.RightHand,
            HumanBodyBones.LeftUpperLeg, HumanBodyBones.LeftLowerLeg, HumanBodyBones.LeftFoot,
            HumanBodyBones.RightUpperLeg, HumanBodyBones.RightLowerLeg, HumanBodyBones.RightFoot,
        };

        // secoes 5, 6 e 11 (V01, V03, V04, V24, V26)
        static readonly Regex pastaRe = new Regex(@"^Assets/_COE/Art/(Avatar|Npc|Prop|Estrutura|Vfx)/([a-z][a-z0-9]*(_[a-z0-9]+)*)/$");
        public const int IdMax = 40;
        static readonly string[] extPermitidas = { ".fbx", ".png", ".tga", ".mat", ".prefab", ".controller", ".anim", ".asset" };
        static readonly string[] extProibidas = { ".blend", ".glb", ".gltf", ".obj", ".psd", ".jpg", ".jpeg", ".webp" };
        static readonly string[] licencasAceitas = { "propria", "tripo_pago", "cc0", "cc_by_4_0", "outra" };
        public const string SharedDir = "Assets/_COE/Art/Shared/";
        static readonly string[] shadersLit = { "Universal Render Pipeline/Lit" };
        static readonly string[] shadersVfx = { "Universal Render Pipeline/Lit", "Universal Render Pipeline/Particles/Unlit", "Universal Render Pipeline/Particles/Lit" };
        static readonly string[] sufixosMapa = { "_basecolor", "_normal", "_metallicsmoothness", "_occlusion", "_emission" };
        static readonly Regex colisaoRe = new Regex("_col(_[0-9]+)?$");

        // ================= auxiliares =================

        public static string F(float v) { return v.ToString("0.000", CultureInfo.InvariantCulture); }
        static ArtCheck Passa(string r, string d) { return new ArtCheck(r, ArtStatus.Pass, d); }
        static ArtCheck Falha(string r, string d) { return new ArtCheck(r, ArtStatus.Fail, d); }
        public static ArtCheck Desconhecido(string r, string d) { return new ArtCheck(r, ArtStatus.Unknown, d); }
        static ArtCheck Lista(string r, List<string> ruins, ArtStatus seRuim, string seOk)
        {
            return ruins.Count == 0 ? Passa(r, seOk) : new ArtCheck(r, seRuim, string.Join("; ", ruins));
        }

        public static bool EhHumano(string categoria) { return categoria == "Avatar" || categoria == "Npc"; }
        public static bool EhColisao(string nome) { return colisaoRe.IsMatch(nome); } // <id>_col, <id>_col_1 (nao pega "_collar")
        public static bool EhGreybox(string id) { return AurenSceneBuilder.CasasAcessiveis.Contains(id) || AurenSceneBuilder.EstruturasPublicas.Contains(id); }

        public static ArtOrcamento Orcamento(string categoria, string id, float maiorDimensao)
        {
            switch (categoria)
            {
                case "Avatar": return orcAvatar;
                case "Npc": return orcNpc;
                case "Prop": return maiorDimensao < PropPequenoAte ? orcPropPequeno : orcPropMedio;
                case "Estrutura": return id.StartsWith("modulo_", StringComparison.Ordinal) ? orcModulo : orcEstrutura;
                case "Vfx": return orcVfx;
                default: return null;
            }
        }

        public static bool FaixaAltura(string categoria, string id, out float min, out float max)
        {
            min = max = 0f;
            switch (categoria)
            {
                case "Avatar":
                    if (!bases.TryGetValue(id, out var b)) return false;
                    min = b.Alvo * AlturaFatorMin; max = b.Alvo * AlturaFatorMax; return true;
                case "Npc":
                    if (npcAdultos.Contains(id)) { min = NpcAdultoMin; max = NpcAdultoMax; return true; }
                    if (npcCriancas.Contains(id)) { min = NpcCriancaMin; max = NpcCriancaMax; return true; }
                    return false;
                case "Prop": min = PropMin; max = PropMax; return true;
                case "Estrutura": min = EstruturaMin; max = EstruturaMax; return true;
                default: return false;
            }
        }

        /// <summary>Base de proporcao de V13: Avatar = o proprio id; Npc pela secao 3.1. null = sem base.</summary>
        public static string BaseDeProporcao(string categoria, string id)
        {
            if (categoria == "Avatar") return bases.ContainsKey(id) ? id : null;
            if (categoria == "Npc") return npcAdultos.Contains(id) ? "avatar_adulto" : npcCriancas.Contains(id) ? "avatar_crianca5" : null;
            return null;
        }

        // ================= regras =================

        public static ArtCheck V01(string pasta)
        {
            string p = pasta.Replace('\\', '/').TrimEnd('/') + "/";
            Match m = pastaRe.Match(p);
            if (!m.Success) return Falha("V01", "caminho fora de Assets/_COE/Art/<Categoria>/<id snake_case>/: " + p);
            string id = m.Groups[2].Value;
            return id.Length <= IdMax ? Passa("V01", "id '" + id + "'") : Falha("V01", "id com " + id.Length + " caracteres (max " + IdMax + ")");
        }

        public static ArtCheck V02(string categoria, string id)
        {
            bool ok;
            switch (categoria)
            {
                case "Avatar": ok = bases.ContainsKey(id); break;
                case "Npc": ok = NpcCatalog.Npc(id) != null; break;
                case "Estrutura": ok = EhGreybox(id) || id.StartsWith("modulo_", StringComparison.Ordinal); break;
                default: return Passa("V02", categoria + " nao tem catalogo: basta V01");
            }
            return ok ? Passa("V02", "id conhecido em " + categoria) : Falha("V02", "id '" + id + "' fora do catalogo de " + categoria);
        }

        /// <summary>arquivos = caminhos ou nomes, sem .meta.</summary>
        public static ArtCheck V03(string id, IEnumerable<string> arquivos)
        {
            var nome = new Regex("^" + Regex.Escape(id) + "(_[a-z0-9_]+)?$");
            string[] ruins = arquivos.Select(a => Path.GetFileName(a))
                .Where(a => !nome.IsMatch(Path.GetFileNameWithoutExtension(a)) || Array.IndexOf(extPermitidas, Path.GetExtension(a)) < 0)
                .ToArray();
            return ruins.Length == 0 ? Passa("V03", "nomes <id>[_parte] e extensoes permitidas")
                : Falha("V03", "fora de <id>[_parte].{fbx,png,tga,mat,prefab,controller,anim,asset}: " + string.Join(", ", ruins));
        }

        /// <summary>Protótipos do ADR-0008: fora do portão, não vão para a loja.</summary>
        public const string PastaPrototipo = "Assets/_COE/Art/Prototipo/";

        /// <summary>Segunda metade de V03: nenhum master/formato proibido em Assets/_COE/Art/**. Exceção única: o JPEG que
        /// o Tripo exporta, dentro de <see cref="PastaPrototipo"/> (ADR-0008). Master segue proibido ali também.</summary>
        public static ArtCheck V03Proibidos(IEnumerable<string> arquivos)
        {
            string[] ruins = arquivos.Where(a =>
            {
                string ext = Path.GetExtension(a).ToLowerInvariant();
                if (Array.IndexOf(extProibidas, ext) < 0) return false;
                bool jpegDePrototipo = (ext == ".jpg" || ext == ".jpeg") && a.Replace('\\', '/').StartsWith(PastaPrototipo, StringComparison.Ordinal);
                return !jpegDePrototipo;
            }).ToArray();
            return ruins.Length == 0 ? Passa("V03", "nenhum .blend/.glb/.gltf/.obj/.psd/.jpg/.jpeg/.webp em Art/** (JPEG do Tripo so em Art/Prototipo/)")
                : Falha("V03", "master ou formato proibido em Art/** (vai para arte/fonte/): " + string.Join(", ", ruins));
        }

        public static ArtCheck V04(string proveniencia, string id)
        {
            if (proveniencia == null) return Falha("V04", "docs/arte/PROVENIENCIA.md nao encontrado");
            Dictionary<string, string> c = BlocoProveniencia(proveniencia, id);
            if (c == null) return Falha("V04", "PROVENIENCIA.md sem bloco '### " + id + "' (portao G3)");
            string g3 = Campo(c, "g3"), lic = Campo(c, "licenca");
            if (!DateTime.TryParseExact(g3, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                return Falha("V04", "g3 '" + g3 + "' nao e data AAAA-MM-DD");
            if (Array.IndexOf(licencasAceitas, lic) < 0) return Falha("V04", "licenca '" + lic + "' reprova (tripo_free, desconhecida ou vazia)");
            if (lic == "cc_by_4_0" && Campo(c, "atribuicao").Length == 0) return Falha("V04", "cc_by_4_0 sem atribuicao");
            if (lic == "outra" && Campo(c, "parecer").Length == 0) return Falha("V04", "licenca outra sem parecer");
            return Passa("V04", "g3 " + g3 + ", licenca " + lic);
        }

        /// <summary>Campos '- chave: valor' do bloco '### id' ate o proximo titulo; null se nao ha bloco.</summary>
        public static Dictionary<string, string> BlocoProveniencia(string texto, string id)
        {
            Dictionary<string, string> campos = null;
            foreach (string bruta in texto.Split('\n'))
            {
                string l = bruta.Trim();
                if (l.StartsWith("#", StringComparison.Ordinal))
                {
                    if (campos != null) break;
                    if (l == "### " + id) campos = new Dictionary<string, string>();
                    continue;
                }
                int dp = l.IndexOf(':');
                if (campos == null || !l.StartsWith("- ", StringComparison.Ordinal) || dp < 0) continue;
                campos[l.Substring(2, dp - 2).Trim()] = l.Substring(dp + 1).Trim().Trim('`').Trim();
            }
            return campos;
        }

        static string Campo(Dictionary<string, string> c, string chave) { return c.TryGetValue(chave, out var v) ? v : ""; }

        public static ArtCheck V05(string id)
        {
            string[] achadas = new[] { HumanoidMapping.ModelKey }.Concat(HumanoidMapping.Keys.SelectMany(k => k.Keys))
                .Where(k => id.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0).ToArray();
            return achadas.Length == 0 ? Passa("V05", "id sem chave do HumanoidMapping")
                : Falha("V05", "id contem '" + string.Join("', '", achadas) + "': HumanoidMapping.Classify confundiria modelo e clip");
        }

        public static ArtCheck V07(Vector3 pos, Quaternion rot, Vector3 escala)
        {
            // angulo pela parte vetorial: Quaternion.Angle devolve 0 abaixo de ~0,16 grau e a tolerancia aqui e 0,01
            float ang = 2f * Mathf.Asin(Mathf.Min(1f, new Vector3(rot.x, rot.y, rot.z).magnitude)) * Mathf.Rad2Deg;
            Vector3 de = escala - Vector3.one;
            bool ok = MaxAbs(pos) <= PosTol && ang <= RotTolGraus && MaxAbs(de) <= EscalaTol;
            return new ArtCheck("V07", ok ? ArtStatus.Pass : ArtStatus.Fail,
                "raiz: posicao " + Vec(pos) + ", rotacao " + F(ang) + " graus, escala " + Vec(escala));
        }

        public static ArtCheck V08(float minY)
        {
            return new ArtCheck("V08", Mathf.Abs(minY) <= ChaoTol ? ArtStatus.Pass : ArtStatus.Fail, "minY=" + F(minY) + " m (tolerancia +-" + F(ChaoTol) + ")");
        }

        public static ArtCheck V09(string categoria, string id, float h)
        {
            if (!FaixaAltura(categoria, id, out float min, out float max)) return Desconhecido("V09", "sem faixa de altura para " + categoria + "/" + id);
            bool ok = h <= max && (categoria == "Prop" ? h > min : h >= min);
            return new ArtCheck("V09", ok ? ArtStatus.Pass : ArtStatus.Fail, "h=" + F(h) + " m, faixa [" + F(min) + "; " + F(max) + "] (" + categoria + "/" + id + ")");
        }

        public static ArtCheck V10(string categoria, Vector3 min, Vector3 max)
        {
            Vector3 c = (min + max) * 0.5f;
            float lim = EhHumano(categoria) ? CentroHumanoMax : CentroFracaoMax * Mathf.Max(max.x - min.x, max.z - min.z);
            bool ok = Mathf.Abs(c.x) <= lim && Mathf.Abs(c.z) <= lim;
            return new ArtCheck("V10", ok ? ArtStatus.Pass : ArtStatus.Fail, "centro XZ (" + F(c.x) + ", " + F(c.z) + "), limite " + F(lim) + " m");
        }

        /// <summary>Frente +Z, esquerda em -X. pontosPe = vertices com peso >= 0,5 em LeftFoot/LeftToes.</summary>
        public static ArtCheck V11(Vector3 coxaEsq, Vector3 coxaDir, Vector3 peEsq, IList<Vector3> pontosPe)
        {
            if (pontosPe.Count == 0) return Desconhecido("V11", "nenhum vertice com peso >= 0,5 no pe esquerdo");
            if (!(coxaEsq.x < coxaDir.x))
                return Falha("V11", "LeftUpperLeg.x=" + F(coxaEsq.x) + " >= RightUpperLeg.x=" + F(coxaDir.x) + ": espelhado ou de costas (esquerda deve ficar em -X)");
            Vector3 ponta = pontosPe.OrderByDescending(p => (p - peEsq).sqrMagnitude).First();
            return ponta.z > peEsq.z ? Passa("V11", "esquerda em -X; ponta do pe z=" + F(ponta.z) + " > LeftFoot z=" + F(peEsq.z))
                : Falha("V11", "ponta do pe z=" + F(ponta.z) + " <= LeftFoot z=" + F(peEsq.z) + ": pe virado para tras (frente deve ser +Z)");
        }

        public static ArtCheck V13(string categoria, string id, float minY, float maxY, float yCoxaEsq, float yPescoco)
        {
            string b = BaseDeProporcao(categoria, id);
            float h = maxY - minY;
            if (b == null || h <= 0f) return Desconhecido("V13", "sem base de proporcao para " + categoria + "/" + id + " ou h <= 0");
            var f = bases[b];
            float perna = (yCoxaEsq - minY) / h, cabeca = (maxY - yPescoco) / h;
            bool ok = perna >= f.PernaMin && perna <= f.PernaMax && cabeca >= f.CabecaMin && cabeca <= f.CabecaMax;
            ArtStatus ruim = categoria == "Npc" ? ArtStatus.Warn : ArtStatus.Fail;
            return new ArtCheck("V13", ok ? ArtStatus.Pass : ruim, "base " + b + ": r_perna=" + F(perna) + " [" + F(f.PernaMin) + "; " + F(f.PernaMax)
                + "], r_cabeca=" + F(cabeca) + " [" + F(f.CabecaMin) + "; " + F(f.CabecaMax) + "]");
        }

        public static ArtCheck V16(ArtOrcamento o, int ossos, int maxPorVertice)
        {
            bool ok = ossos <= o.Ossos && maxPorVertice <= MaxOssosPorVertice;
            return new ArtCheck("V16", ok ? ArtStatus.Pass : ArtStatus.Fail, ossos + " ossos de skin (max " + o.Ossos + "), maxBonesPerVertex=" + maxPorVertice + " (max " + MaxOssosPorVertice + ")");
        }

        public static ArtCheck V17(Vector3 bracoEsq, Vector3 cotoveloEsq, Vector3 bracoDir, Vector3 cotoveloDir)
        {
            float e = AnguloComXZ(cotoveloEsq - bracoEsq), d = AnguloComXZ(cotoveloDir - bracoDir);
            bool ok = e <= TPoseTolGraus && d <= TPoseTolGraus;
            return new ArtCheck("V17", ok ? ArtStatus.Pass : ArtStatus.Warn, "braco esq " + F(e) + ", dir " + F(d) + " graus do plano XZ (max " + F(TPoseTolGraus) + ")");
        }

        public static float AnguloComXZ(Vector3 v)
        {
            return v.sqrMagnitude < 1e-12f ? 90f : Mathf.Asin(Mathf.Min(1f, Mathf.Abs(v.y) / v.magnitude)) * Mathf.Rad2Deg;
        }

        public static ArtCheck V18(HumanoidFiles f)
        {
            string[] falta = f.Missing.Select(c => c.ToString()).ToArray();
            bool ok = falta.Length == 0 && f.Unmapped.Count == 0;
            return new ArtCheck("V18", ok ? ArtStatus.Pass : ArtStatus.Fail,
                ok ? "8 clips mapeados" : "faltam [" + string.Join(", ", falta) + "]; sem clip [" + string.Join(", ", f.Unmapped) + "]");
        }

        public static ArtCheck V20(IDictionary<HumanoidClip, float> duracoes)
        {
            if (duracoes.Count == 0) return Desconhecido("V20", "nenhum AnimationClip importado");
            var fora = new List<string>();
            foreach (KeyValuePair<HumanoidClip, float> d in duracoes)
            {
                int quadros;
                if (!quadrosReferencia.TryGetValue(d.Key, out quadros)) continue;   // Skill (opcional) sem referencia na secao 7.2
                float referencia = quadros / Fps;
                if (Mathf.Abs(d.Value - referencia) > DuracaoTol * referencia) fora.Add(d.Key + " " + F(d.Value) + " s (ref " + F(referencia) + ")");
            }
            return Lista("V20", fora, ArtStatus.Warn, "duracoes dentro de +-30% da secao 7.2");
        }

        public static ArtCheck V21(ArtOrcamento o, int tris)
        {
            return new ArtCheck("V21", tris <= o.Tris ? ArtStatus.Pass : ArtStatus.Fail, tris + " tris no LOD0 (max " + o.Tris + ", " + o.Nome + ")");
        }

        /// <summary>trisPorLod null = sem LODGroup na raiz. So chamar quando o.LodRazoes != null.</summary>
        public static ArtCheck V22(ArtOrcamento o, int[] trisPorLod)
        {
            int niveis = o.LodRazoes.Length + 1;
            if (trisPorLod == null) return Falha("V22", o.Nome + " exige LODGroup na raiz com " + niveis + " niveis");
            if (trisPorLod.Length != niveis) return Falha("V22", "LODGroup com " + trisPorLod.Length + " niveis; secao 4 pede " + niveis);
            var ruins = new List<string>();
            for (int i = 1; i < niveis; i++)
                if (trisPorLod[i] > o.LodRazoes[i - 1] * trisPorLod[0])
                    ruins.Add("LOD" + i + " " + trisPorLod[i] + " tris > " + F(o.LodRazoes[i - 1]) + " x LOD0 (" + trisPorLod[0] + ")");
            return Lista("V22", ruins, ArtStatus.Fail, niveis + " niveis dentro das razoes da secao 4");
        }

        /// <summary>materiais = distintos; Caminho null = slot vazio.</summary>
        public static ArtCheck V24(string pasta, string categoria, ArtOrcamento o, IList<(string Caminho, string Shader)> materiais)
        {
            string[] aceitos = categoria == "Vfx" ? shadersVfx : shadersLit;
            string dir = pasta.TrimEnd('/') + "/";
            var ruins = new List<string>();
            foreach (var m in materiais)
            {
                if (m.Caminho == null) { ruins.Add("slot de material vazio"); continue; }
                bool daPasta = m.Caminho.StartsWith(dir, StringComparison.Ordinal) || m.Caminho.StartsWith(SharedDir, StringComparison.Ordinal);
                if (!m.Caminho.EndsWith(".mat", StringComparison.Ordinal) || !daPasta) ruins.Add(m.Caminho + " nao e .mat da pasta nem de Art/Shared (embutido no FBX nao vale)");
                if (Array.IndexOf(aceitos, m.Shader) < 0) ruins.Add(m.Caminho + " com shader '" + m.Shader + "'");
            }
            int n = materiais.Count(m => m.Caminho != null);
            if (n > o.Materiais) ruins.Add(n + " materiais (max " + o.Materiais + ", " + o.Nome + ")");
            return Lista("V24", ruins, ArtStatus.Fail, n + " materiais .mat URP (max " + o.Materiais + ")");
        }

        public static ArtCheck V25(ArtOrcamento o, IList<ArtTextura> texturas)
        {
            var ruins = new List<string>();
            foreach (ArtTextura t in texturas)
            {
                if (t.Largura <= 0) { ruins.Add(t.Nome + " sem TextureImporter"); continue; }
                if (!Mathf.IsPowerOfTwo(t.Largura) || !Mathf.IsPowerOfTwo(t.Altura)) ruins.Add(t.Nome + " " + t.Largura + "x" + t.Altura + " nao e potencia de 2");
                if (Mathf.Max(t.Largura, t.Altura) > o.Textura) ruins.Add(t.Nome + " " + t.Largura + "x" + t.Altura + " acima de " + o.Textura);
                if (t.MaxSize > o.Textura) ruins.Add(t.Nome + " maxTextureSize " + t.MaxSize + " > " + o.Textura);
                if (!t.Mipmap) ruins.Add(t.Nome + " sem mipmap");
            }
            return Lista("V25", ruins, ArtStatus.Fail, texturas.Count + " texturas potencia de 2, <= " + o.Textura + ", com mipmap");
        }

        public static ArtCheck V26(IList<ArtTextura> texturas)
        {
            var ruins = new List<string>();
            foreach (ArtTextura t in texturas)
            {
                string s = sufixosMapa.FirstOrDefault(x => t.Nome.EndsWith(x, StringComparison.Ordinal));
                if (s == null) ruins.Add(t.Nome + " sem sufixo de mapa (_basecolor, _normal, _metallicsmoothness, _occlusion, _emission)");
                else if ((s == "_basecolor" || s == "_emission") && !t.Srgb) ruins.Add(t.Nome + " precisa sRGB");
                else if (s == "_normal" && !t.NormalMap) ruins.Add(t.Nome + " precisa textureType NormalMap");
                else if ((s == "_metallicsmoothness" || s == "_occlusion") && t.Srgb) ruins.Add(t.Nome + " precisa linear (sRGB desligado)");
            }
            return Lista("V26", ruins, ArtStatus.Fail, texturas.Count + " texturas com sufixo e espaco de cor certos");
        }

        static float MaxAbs(Vector3 v) { return Mathf.Max(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z)); }
        static string Vec(Vector3 v) { return "(" + F(v.x) + ", " + F(v.y) + ", " + F(v.z) + ")"; }
    }
}
