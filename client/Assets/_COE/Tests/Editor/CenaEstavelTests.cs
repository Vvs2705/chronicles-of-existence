using System.Text.RegularExpressions;
using COE.EditorTools;
using NUnit.Framework;

namespace COE.EditorTests
{
    /// <summary>Cena regerada sem mudanca de conteudo sai igual byte a byte (CenaEstavel). Mini-cena em YAML: raiz com um
    /// filho, um prefab instanciado sob a raiz e um material solto; as duas versoes so diferem nos ids aleatorios do Unity.</summary>
    public class CenaEstavelTests
    {
        static string Cena(long raiz, long filho, long pi, long mat)
        {
            return "%YAML 1.1\n%TAG !u! tag:unity3d.com,2011:\n" +
                "--- !u!29 &1\nOcclusionCullingSettings:\n  m_ObjectHideFlags: 0\n" +
                $"--- !u!1 &{raiz}\nGameObject:\n  m_Component:\n  - component: {{fileID: {raiz + 1}}}\n  m_Name: Auren\n" +
                $"--- !u!4 &{raiz + 1}\nTransform:\n  m_GameObject: {{fileID: {raiz}}}\n  m_Children:\n  - {{fileID: {filho + 1}}}\n  - {{fileID: {pi + 1}}}\n  m_Father: {{fileID: 0}}\n" +
                $"--- !u!1 &{filho}\nGameObject:\n  m_Component:\n  - component: {{fileID: {filho + 1}}}\n  - component: {{fileID: {filho + 2}}}\n  m_Name: poco\n" +
                $"--- !u!4 &{filho + 1}\nTransform:\n  m_GameObject: {{fileID: {filho}}}\n  m_Children: []\n  m_Father: {{fileID: {raiz + 1}}}\n" +
                $"--- !u!23 &{filho + 2}\nMeshRenderer:\n  m_GameObject: {{fileID: {filho}}}\n  m_Materials:\n  - {{fileID: {mat}}}\n  - {{fileID: 2100000, guid: abc, type: 2}}\n" +
                $"--- !u!1001 &{pi}\nPrefabInstance:\n  m_Modification:\n    m_TransformParent: {{fileID: {raiz + 1}}}\n    m_Modifications:\n    - target: {{fileID: -867, guid: def, type: 3}}\n      objectReference: {{fileID: 0}}\n" +
                $"--- !u!4 &{pi + 1} stripped\nTransform:\n  m_CorrespondingSourceObject: {{fileID: -867, guid: def, type: 3}}\n  m_PrefabInstance: {{fileID: {pi}}}\n" +
                $"--- !u!21 &{mat}\nMaterial:\n  m_Name: COE_Marcador\n" +
                $"--- !u!1660057539 &9223372036854775807\nSceneRoots:\n  m_Roots:\n  - {{fileID: {raiz + 1}}}\n";
        }

        [Test]
        public void MesmaCenaComIdsAleatoriosDiferentes_SaiIgual()
        {
            string a = CenaEstavel.Normalizar(Cena(2583158, 5760326, 61750460, 545608388));
            string b = CenaEstavel.Normalizar(Cena(4886904, 14253138, 97869147, 123456789));
            Assert.AreEqual(a, b, "a cena regerada mudou sem mudanca de conteudo");
            Assert.AreEqual(a, CenaEstavel.Normalizar(a), "normalizar de novo tem de ser no-op");
        }

        /// <summary>Objeto novo no gerador muda so os ids dele: a chave e o NOME (e a ocorrencia entre irmaos de mesmo nome),
        /// nao a posicao. Por posicao, inserir uma raiz renumerava todas as raizes seguintes (diff de 50 linhas em Auren).</summary>
        [Test]
        public void RaizNovaAntesDeOutra_NaoMudaOsIdsDaOutra()
        {
            string sem = CenaEstavel.Normalizar(Cena(2583158, 5760326, 61750460, 545608388));
            string novaRaiz = "--- !u!1 &777\nGameObject:\n  m_Component:\n  - component: {fileID: 778}\n  m_Name: GanchoHud\n" +
                              "--- !u!4 &778\nTransform:\n  m_GameObject: {fileID: 777}\n  m_Children: []\n  m_Father: {fileID: 0}\n";
            string com = Cena(2583158, 5760326, 61750460, 545608388)
                .Replace("--- !u!1660057539", novaRaiz + "--- !u!1660057539")
                .Replace("  m_Roots:\n", "  m_Roots:\n  - {fileID: 778}\n");   // a raiz nova entra ANTES de Auren
            com = CenaEstavel.Normalizar(com);
            foreach (Match m in Regex.Matches(sem, @"^--- !u!\d+ &(\d+)", RegexOptions.Multiline))
                StringAssert.Contains("&" + m.Groups[1].Value, com, "id da cena antiga mudou com a raiz nova");
        }

        [Test]
        public void ReferenciasContinuamApontandoParaOMesmoObjeto()
        {
            string n = CenaEstavel.Normalizar(Cena(2583158, 5760326, 61750460, 545608388));
            long Id(string cls, string depois) => long.Parse(Regex.Match(n, @"--- !u!" + cls + @" &(\d+)[^\n]*\n" + depois).Groups[1].Value);
            long goPoco = Id("1", @"GameObject:\n  m_Component:\n  - component: \{fileID: \d+\}\n  - component");
            long tPoco = Id("4", @"Transform:\n  m_GameObject: \{fileID: " + goPoco + @"\}");
            long mat = Id("21", "Material:");
            long pi = Id("1001", "PrefabInstance:");
            StringAssert.Contains("- {fileID: " + tPoco + "}", n, "m_Children da raiz aponta para o transform do poco");
            StringAssert.Contains("- {fileID: " + mat + "}", n, "renderer aponta para o material solto");
            StringAssert.Contains("m_PrefabInstance: {fileID: " + pi + "}", n, "stripped aponta para a PrefabInstance");
            Assert.AreEqual(pi + 1, Id("4", @"Transform:\n  m_CorrespondingSourceObject"), "stripped = base + 1, como o Unity faz");
            StringAssert.Contains("{fileID: 2100000, guid: abc, type: 2}", n, "referencia externa nao muda");
            StringAssert.Contains("target: {fileID: -867, guid: def, type: 3}", n, "alvo de modificacao do prefab nao muda");
            StringAssert.Contains("--- !u!29 &1\n", n, "configuracoes da cena mantem id fixo");
            StringAssert.EndsWith("--- !u!1660057539 &9223372036854775807\nSceneRoots:\n  m_Roots:\n  - {fileID: " + Id("4", @"Transform:\n  m_GameObject: \{fileID: \d+\}\n  m_Children:\n  - ") + "}\n", n);
        }
    }
}
