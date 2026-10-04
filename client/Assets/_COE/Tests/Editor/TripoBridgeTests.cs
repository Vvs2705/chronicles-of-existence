using System.IO;
using NUnit.Framework;
using UnityEngine;

namespace COE.EditorTests
{
    /// <summary>Bloco G (ADR-0010): o Tripo Bridge e ferramenta de Editor. Nao pode entrar no player (Android) nem ser
    /// referenciado pelo runtime do jogo, e nao trabalha em batch (build, testes, verify).</summary>
    public class TripoBridgeTests
    {
        static readonly string Pacote = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Packages", "com.tripo3d.unitybridge"));

        [Test]
        public void Bridge_SoNoEditor_ForaDoRuntime_ESemLimpezaEmBatch()
        {
            string asmdef = File.ReadAllText(Path.Combine(Pacote, "Editor", "com.tripo3d.unitybridge.editor.asmdef"));
            StringAssert.IsMatch(@"""includePlatforms""\s*:\s*\[\s*""Editor""\s*\]", asmdef, "o Bridge so compila no Editor");

            string runtime = File.ReadAllText(Path.Combine(Application.dataPath, "_COE", "Scripts", "COE.asmdef"));
            StringAssert.DoesNotContain("Tripo3D", runtime, "o runtime do jogo nao referencia o Bridge");

            string limpeza = File.ReadAllText(Path.Combine(Pacote, "Editor", "StartupCleanup.cs"));
            StringAssert.Contains("Application.isBatchMode", limpeza, "a limpeza do [InitializeOnLoad] nao roda em batch");
        }
    }
}
