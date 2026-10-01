using System.Collections.Generic;
using System.Linq;
using COE.EditorTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace COE.EditorTests
{
    /// <summary>Prova do validador de arte (docs/arte/PIPELINE.md secao 11). Positiva: o placeholder infantil de Art/Humanoid
    /// validado como avatar_crianca5 passa em todas as regras [P]. Negativa: o MESMO placeholder validado como avatar_adulto
    /// reprova V09 e V13; e, para V11, as mesmas medidas giradas 180 graus em Y (ou com o pe para tras) reprovam.
    /// Os testes de fixture abrem a propria cena vazia (Single, como AurenSceneTests) e destroem o que instanciam;
    /// os demais testam as regras puras sem asset.</summary>
    public class ArtImportValidatorTests
    {
        static readonly string[] RegrasP = { "V06", "V07", "V08", "V09", "V10", "V11", "V13", "V14", "V15", "V16", "V17", "V18", "V19", "V20", "V21" };
        const string ModeloPlaceholder = HumanoidSetup.Dir + "/Model.fbx";

        static void CenaVazia() { EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single); }

        static ArtCheck Regra(List<ArtCheck> r, string v)
        {
            ArtCheck c = r.SingleOrDefault(x => x.Regra == v);
            Assert.IsNotNull(c, v + " nao rodou");
            return c;
        }

        static void Espera(ArtStatus esperado, ArtCheck c) { Assert.AreEqual(esperado, c.Status, c.ToString()); }

        // ---------- fixtures (placeholder em Art/Humanoid) ----------

        [Test]
        public void FixturePositiva_PlaceholderInfantil_PassaEmTodasAsRegrasP()
        {
            CenaVazia();
            List<ArtCheck> r = ArtImportValidator.ValidarPasta(HumanoidSetup.Dir, "Avatar", "avatar_crianca5", soP: true);
            // todas as [P] de uma vez: a mensagem lista cada regra que nao deu PASS, nao so a primeira
            string[] naoPassou = RegrasP.Select(v => Regra(r, v)).Where(c => c.Status != ArtStatus.Pass).Select(c => c.ToString()).ToArray();
            Assert.IsEmpty(naoPassou, string.Join("\n", naoPassou));
            Assert.IsEmpty(r.Where(c => !RegrasP.Contains(c.Regra)).Select(c => c.ToString()).ToList(), "placeholder roda so as regras [P]");
        }

        [Test]
        public void FixtureNegativa_MesmoPlaceholderComoAdulto_ReprovaAlturaEProporcao()
        {
            CenaVazia();
            List<ArtCheck> r = ArtImportValidator.ValidarPasta(HumanoidSetup.Dir, "Avatar", "avatar_adulto", soP: true);
            Espera(ArtStatus.Fail, Regra(r, "V09")); // 1,10 m fora de [1,698; 1,838]
            Espera(ArtStatus.Fail, Regra(r, "V13")); // r_perna 0,473 e r_cabeca 0,218 fora das faixas adultas
        }

        [Test]
        public void FixtureNegativa_FrenteInvertida_ReprovaV11()
        {
            CenaVazia();
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(ModeloPlaceholder);
            Assert.IsNotNull(asset, "fixture ausente: " + ModeloPlaceholder);
            GameObject go = Object.Instantiate(asset);
            try
            {
                ArtMedidas m = ArtImportValidator.Medir(go);
                Vector3 ce = m.Ossos[HumanBodyBones.LeftUpperLeg], cd = m.Ossos[HumanBodyBones.RightUpperLeg], pe = m.Ossos[HumanBodyBones.LeftFoot];
                Espera(ArtStatus.Pass, ArtRules.V11(ce, cd, pe, m.PeEsquerdo)); // controle: a mesma medida, sem mexer, passa

                Quaternion meiaVolta = Quaternion.Euler(0f, 180f, 0f); // modelo de costas
                Espera(ArtStatus.Fail, ArtRules.V11(meiaVolta * ce, meiaVolta * cd, meiaVolta * pe, m.PeEsquerdo.Select(p => meiaVolta * p).ToList()));

                // pe virado para tras com a esquerda no lugar (espelho em Z): reprova pela segunda metade de V11
                Espera(ArtStatus.Fail, ArtRules.V11(EspelhoZ(ce), EspelhoZ(cd), EspelhoZ(pe), m.PeEsquerdo.Select(EspelhoZ).ToList()));
            }
            finally { Object.DestroyImmediate(go); }
        }

        static Vector3 EspelhoZ(Vector3 v) { return new Vector3(v.x, v.y, -v.z); }

        // ---------- regras puras ----------

        [Test]
        public void V01_CaminhoEId()
        {
            Espera(ArtStatus.Pass, ArtRules.V01("Assets/_COE/Art/Npc/borin"));
            Espera(ArtStatus.Pass, ArtRules.V01("Assets/_COE/Art/Estrutura/modulo_parede_taipa_3m/"));
            Espera(ArtStatus.Fail, ArtRules.V01("Assets/_COE/Art/Npc/Borin"));
            Espera(ArtStatus.Fail, ArtRules.V01("Assets/_COE/Art/Personagem/borin"));
            Espera(ArtStatus.Fail, ArtRules.V01("Assets/_COE/Art/Prop/barril__velho"));
            Espera(ArtStatus.Fail, ArtRules.V01("Assets/_COE/Art/Prop/" + new string('a', 41)));
        }

        [Test]
        public void V02_V05_IdsDosCatalogos()
        {
            foreach (NpcDef n in NpcCatalog.Npcs)
            {
                Espera(ArtStatus.Pass, ArtRules.V02("Npc", n.Id));
                Espera(ArtStatus.Pass, ArtRules.V05(n.Id)); // PIPELINE secao 6: os 10 ids do NpcCatalog passam
            }
            foreach (string a in new[] { "avatar_crianca5", "avatar_crianca8", "avatar_adolescente", "avatar_adulto" })
            {
                Espera(ArtStatus.Pass, ArtRules.V02("Avatar", a));
                Espera(ArtStatus.Pass, ArtRules.V05(a));
            }
            Espera(ArtStatus.Fail, ArtRules.V02("Npc", "zeca"));
            Espera(ArtStatus.Fail, ArtRules.V02("Avatar", "avatar_idoso"));
            Espera(ArtStatus.Pass, ArtRules.V02("Estrutura", "ferraria"));
            Espera(ArtStatus.Pass, ArtRules.V02("Estrutura", "casa_nilo"));
            Espera(ArtStatus.Pass, ArtRules.V02("Estrutura", "modulo_parede_taipa_3m"));
            Espera(ArtStatus.Fail, ArtRules.V02("Estrutura", "castelo"));
            Espera(ArtStatus.Pass, ArtRules.V02("Prop", "barril"));
            Espera(ArtStatus.Fail, ArtRules.V05("guarda_idle"));
            Espera(ArtStatus.Fail, ArtRules.V05("borin_model"));
        }

        [Test]
        public void V03_NomesEExtensoes()
        {
            Espera(ArtStatus.Pass, ArtRules.V03("borin", new[]
            {
                "Assets/_COE/Art/Npc/borin/borin_model.fbx", "Assets/_COE/Art/Npc/borin/borin_corpo_basecolor.png",
                "Assets/_COE/Art/Npc/borin/borin.prefab", "Assets/_COE/Art/Npc/borin/borin_corpo.mat",
            }));
            Espera(ArtStatus.Fail, ArtRules.V03("borin", new[] { "borin_model.fbx", "lysa_corpo.mat" }));
            Espera(ArtStatus.Fail, ArtRules.V03("borin", new[] { "borin_corpo.jpg" }));
            Espera(ArtStatus.Fail, ArtRules.V03("borin", new[] { "borin_Model.fbx" }));
            Espera(ArtStatus.Fail, ArtRules.V03("borin", new[] { "borinho_model.fbx" }));
            Espera(ArtStatus.Fail, ArtRules.V03Proibidos(new[] { "Assets/_COE/Art/Shared/pedra.PSD" }));
            Espera(ArtStatus.Pass, ArtRules.V03Proibidos(new[] { "Assets/_COE/Art/Shared/pedra_basecolor.png" }));

            // ADR-0008: o Tripo exporta a textura em JPEG; protótipo não passa pelo portão, então o JPEG fica onde está.
            // Fora de Art/Prototipo/ o JPEG segue proibido, e master (.blend, .psd...) segue proibido em qualquer lugar.
            Espera(ArtStatus.Pass, ArtRules.V03Proibidos(new[] { "Assets/_COE/Art/Prototipo/Personagens/borin/borin_basecolor.JPEG" }));
            Espera(ArtStatus.Fail, ArtRules.V03Proibidos(new[] { "Assets/_COE/Art/Npc/borin/borin_basecolor.jpeg" }));
            Espera(ArtStatus.Fail, ArtRules.V03Proibidos(new[] { "Assets/_COE/Art/Prototipo/Pecas/poco/poco.blend" }));
            Espera(ArtStatus.Fail, ArtRules.V03Proibidos(new[] { "Assets/_COE/Art/PrototipoFalso/x/x.jpg" }));
        }

        [Test]
        public void V04_LeOBlocoDaProveniencia()
        {
            string doc = "## 4. Registro\n\n### borin\n- licenca: tripo_pago\n- g3: 2026-10-10\n\n"
                + "### lysa\n- licenca: tripo_free\n- g3: 2026-10-10\n\n"
                + "### eira\n- licenca: cc_by_4_0\n- g3: 2026-10-10\n\n"
                + "### oren\n- licenca: `outra`\n- parecer: juridico 2026-10-01\n- g3: 2026-10-10\n\n"
                + "### tovin\r\n- licenca: propria\r\n- g3: n/a\r\n\n## 5. Outra secao\n- licenca: propria\n";
            Espera(ArtStatus.Pass, ArtRules.V04(doc, "borin"));
            Espera(ArtStatus.Fail, ArtRules.V04(doc, "lysa"));   // plano gratuito
            Espera(ArtStatus.Fail, ArtRules.V04(doc, "eira"));   // cc_by_4_0 sem atribuicao
            Espera(ArtStatus.Pass, ArtRules.V04(doc, "oren"));   // outra + parecer (crase removida)
            Espera(ArtStatus.Fail, ArtRules.V04(doc, "tovin"));  // g3 n/a
            Espera(ArtStatus.Fail, ArtRules.V04(doc, "mara"));   // sem bloco
            Espera(ArtStatus.Fail, ArtRules.V04(null, "borin")); // arquivo ausente
            Assert.AreEqual(2, ArtRules.BlocoProveniencia(doc, "tovin").Count, "bloco termina no proximo titulo");
        }

        [Test]
        public void V07_ToleranciaDeRotacaoAbaixoDoQuaternionAngle()
        {
            Espera(ArtStatus.Pass, ArtRules.V07(Vector3.zero, Quaternion.identity, Vector3.one));
            Espera(ArtStatus.Pass, ArtRules.V07(new Vector3(0.0005f, 0f, 0f), Quaternion.Euler(0.005f, 0f, 0f), Vector3.one));
            Espera(ArtStatus.Fail, ArtRules.V07(Vector3.zero, Quaternion.Euler(0.05f, 0f, 0f), Vector3.one)); // Quaternion.Angle diria 0
            Espera(ArtStatus.Fail, ArtRules.V07(Vector3.zero, Quaternion.Euler(-90f, 0f, 0f), Vector3.one));  // FBX do Blender sem transform aplicado
            Espera(ArtStatus.Fail, ArtRules.V07(Vector3.zero, Quaternion.identity, Vector3.one * 100f));
        }

        [Test]
        public void V09_V13_FaixasDaSecao3_NaoEscalonar()
        {
            float min, max;
            Assert.IsTrue(ArtRules.FaixaAltura("Avatar", "avatar_crianca5", out min, out max));
            Assert.AreEqual(1.067f, min, 0.001f);
            Assert.AreEqual(1.155f, max, 0.001f);
            Assert.IsTrue(ArtRules.FaixaAltura("Avatar", "avatar_adulto", out min, out max));
            Assert.AreEqual(1.698f, min, 0.001f);
            Assert.AreEqual(1.838f, max, 0.001f);

            Espera(ArtStatus.Pass, ArtRules.V09("Avatar", "avatar_crianca5", 1.10f));
            Espera(ArtStatus.Fail, ArtRules.V09("Avatar", "avatar_adulto", 1.10f));
            Espera(ArtStatus.Pass, ArtRules.V09("Npc", "nilo", 1.10f));
            Espera(ArtStatus.Fail, ArtRules.V09("Npc", "borin", 1.10f));
            Espera(ArtStatus.Unknown, ArtRules.V09("Npc", "zeca", 1.75f));
            Espera(ArtStatus.Fail, ArtRules.V09("Prop", "caneca", 0.02f)); // (0,02; 4,0]: aberto embaixo

            // placeholder infantil (quadril 0,52, pescoco 0,86, h 1,10): crianca passa, adulto reprova, NPC adulto so avisa
            Espera(ArtStatus.Pass, ArtRules.V13("Avatar", "avatar_crianca5", 0f, 1.10f, 0.52f, 0.86f));
            Espera(ArtStatus.Fail, ArtRules.V13("Avatar", "avatar_adulto", 0f, 1.10f, 0.52f, 0.86f));
            Espera(ArtStatus.Warn, ArtRules.V13("Npc", "borin", 0f, 1.10f, 0.52f, 0.86f));
            // adulto encolhido para 1,10 m (razoes 0,543 / 0,175) reprova como crianca
            Espera(ArtStatus.Fail, ArtRules.V13("Avatar", "avatar_crianca5", 0f, 1.10f, 0.543f * 1.10f, 1.10f - 0.175f * 1.10f));
        }

        // Os valores da secao 4 vao virar orcamento de celular (ADR-0006): nenhum teste fixa numero da tabela. Aqui so a
        // escolha da linha (pelo Nome) e a logica das regras, com teto lido da tabela ou de um orcamento local.
        [Test]
        public void Orcamento_EscolheALinhaDaSecao4_V21_V22()
        {
            Assert.AreEqual("Prop pequeno", ArtRules.Orcamento("Prop", "caneca", 0.3f).Nome);
            Assert.AreEqual("Prop medio", ArtRules.Orcamento("Prop", "carroca", 2.5f).Nome);
            Assert.AreEqual("Estrutura modulo_", ArtRules.Orcamento("Estrutura", "modulo_parede_taipa_3m", 3f).Nome);
            Assert.AreEqual("Estrutura", ArtRules.Orcamento("Estrutura", "ferraria", 11f).Nome);
            foreach (string cat in ArtRules.Categorias) Assert.IsNotNull(ArtRules.Orcamento(cat, "x", 1f), cat + " sem linha na secao 4");

            ArtOrcamento npc = ArtRules.Orcamento("Npc", "borin", 1.8f);
            Espera(ArtStatus.Pass, ArtRules.V21(npc, npc.Tris));
            Espera(ArtStatus.Fail, ArtRules.V21(npc, npc.Tris + 1));

            var lod = new ArtOrcamento("teste", 20000, new[] { 0.50f, 0.25f }, 3, 2048, 75);
            Espera(ArtStatus.Pass, ArtRules.V22(lod, new[] { 20000, 10000, 5000 }));
            Espera(ArtStatus.Fail, ArtRules.V22(lod, new[] { 20000, 10001, 5000 }));
            Espera(ArtStatus.Fail, ArtRules.V22(lod, new[] { 20000, 10000 }));
            Espera(ArtStatus.Fail, ArtRules.V22(lod, null));
        }

        [Test]
        public void V17_V20_V24_V25_V26_ENomeDeColisao()
        {
            var ombroE = new Vector3(-0.11f, 0.83f, 0f);
            var ombroD = new Vector3(0.11f, 0.83f, 0f);
            Espera(ArtStatus.Pass, ArtRules.V17(ombroE, ombroE + new Vector3(-0.18f, 0f, 0f), ombroD, ombroD + new Vector3(0.18f, 0f, 0f)));
            Vector3 caido = Quaternion.Euler(0f, 0f, 30f) * new Vector3(0.18f, 0f, 0f);
            Espera(ArtStatus.Warn, ArtRules.V17(ombroE, ombroE + new Vector3(-0.18f, 0f, 0f), ombroD, ombroD + caido));

            var duracoes = new Dictionary<HumanoidClip, float> { { HumanoidClip.Idle, 74f / 30f }, { HumanoidClip.Hit, 8f / 30f } };
            Espera(ArtStatus.Pass, ArtRules.V20(duracoes));
            duracoes[HumanoidClip.Idle] = 1.0f;
            Espera(ArtStatus.Warn, ArtRules.V20(duracoes));

            var prop = new ArtOrcamento("teste", 1500, null, 1, 512, 0); // local: 1 material, textura 512
            string dir = "Assets/_COE/Art/Prop/caneca";
            Espera(ArtStatus.Pass, ArtRules.V24(dir, "Prop", prop, new List<(string, string)> { (dir + "/caneca_corpo.mat", "Universal Render Pipeline/Lit") }));
            Espera(ArtStatus.Fail, ArtRules.V24(dir, "Prop", prop, new List<(string, string)> { (dir + "/caneca_model.fbx", "Universal Render Pipeline/Lit") })); // embutido
            Espera(ArtStatus.Fail, ArtRules.V24(dir, "Prop", prop, new List<(string, string)> { (dir + "/caneca_corpo.mat", "Standard") }));
            Espera(ArtStatus.Fail, ArtRules.V24(dir, "Prop", prop, new List<(string, string)>
                { (dir + "/caneca_corpo.mat", "Universal Render Pipeline/Lit"), (ArtRules.SharedDir + "trim.mat", "Universal Render Pipeline/Lit") })); // 2 > 1

            var ok = new ArtTextura { Nome = "caneca_corpo_basecolor", Largura = 512, Altura = 512, MaxSize = 512, Mipmap = true, Srgb = true };
            Espera(ArtStatus.Pass, ArtRules.V25(prop, new[] { ok }));
            Espera(ArtStatus.Pass, ArtRules.V26(new[] { ok }));
            var grande = ok; grande.Largura = grande.Altura = 1024;
            Espera(ArtStatus.Fail, ArtRules.V25(prop, new[] { grande }));
            var torta = ok; torta.Largura = 500;
            Espera(ArtStatus.Fail, ArtRules.V25(prop, new[] { torta }));
            var normal = ok; normal.Nome = "caneca_corpo_normal"; normal.Srgb = false;
            Espera(ArtStatus.Fail, ArtRules.V26(new[] { normal })); // sem textureType NormalMap
            normal.NormalMap = true;
            Espera(ArtStatus.Pass, ArtRules.V26(new[] { normal }));
            var semSufixo = ok; semSufixo.Nome = "caneca_corpo_rough";
            Espera(ArtStatus.Fail, ArtRules.V26(new[] { semSufixo }));

            Assert.IsTrue(ArtRules.EhColisao("ferraria_col") && ArtRules.EhColisao("ferraria_col_2"));
            Assert.IsFalse(ArtRules.EhColisao("borin_collar"));
        }
    }
}
