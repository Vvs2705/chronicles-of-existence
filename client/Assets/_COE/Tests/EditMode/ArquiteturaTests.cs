using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Bloco C (2026-10-04): o localizador estatico SaveState nao cresce (DIVIDA_TECNICA.md). Quem le
    /// SaveState.Current/Sessao no runtime esta na lista abaixo: bootstrap e telas ainda nao migradas. Arquivo novo recebe
    /// a sessao por campo ou parametro. Migrou um? Apague a linha dele. So o proprio SaveState troca o Current.
    /// Le o codigo-fonte em Assets/_COE/Scripts (so roda no Editor), sem comentario.</summary>
    public class ArquiteturaTests
    {
        // Desde 2026-10-04 (passo 6): telas, NPCs, gatilhos e corpo recebem a sessao pela Partida da cena. Ficam o proprio
        // Partida (bootstrap), a entrada (troca o save: nascimento e Nova vida) e o robo de desenvolvimento.
        static readonly string[] ConsumidoresDoSaveState =
        {
            "Core/EntryFlow.cs", "Core/Partida.cs", "Core/Roteiro.cs",
        };

        /// <summary>Bloco D: IMGUI (OnGUI) so para ferramenta de desenvolvimento. Telas de jogador ainda nao migradas para
        /// uGUI ficam nesta lista, que so diminui; o PerfHud (diagnostico) fica.</summary>
        static readonly string[] AindaEmImgui =
        {
            "Perf/PerfHud.cs",
        };

        static readonly string Raiz = Path.Combine(UnityEngine.Application.dataPath, "_COE", "Scripts");

        static IEnumerable<KeyValuePair<string, string>> CodigoDoRuntime()
        {
            foreach (string f in Directory.GetFiles(Raiz, "*.cs", SearchOption.AllDirectories))
            {
                string rel = f.Substring(Raiz.Length + 1).Replace('\\', '/');
                string semComentario = Regex.Replace(File.ReadAllText(f), "//.*", "");
                yield return new KeyValuePair<string, string>(rel, semComentario);
            }
        }

        [Test]
        public void SaveState_NaoGanhaConsumidorNovo()
        {
            List<string> novos = CodigoDoRuntime()
                .Where(c => Regex.IsMatch(c.Value, @"SaveState\.(Current|Sessao)\b") && !ConsumidoresDoSaveState.Contains(c.Key))
                .Select(c => c.Key).ToList();
            CollectionAssert.IsEmpty(novos, "consumidor novo do SaveState: receba a sessao por campo ou parametro");
        }

        /// <summary>Partida.De(null) seria o SaveState com outro nome: quem pede a sessao declara o campo que o gerador liga e
        /// passa ele mesmo (revisao do passo 6).</summary>
        [Test]
        public void QuemPedeAPartida_UsaOCampoLigado()
        {
            List<string> soltos = CodigoDoRuntime()
                .Where(c => c.Key != "Core/Partida.cs" && Regex.IsMatch(c.Value, @"Partida\.De\(")
                            && (Regex.IsMatch(c.Value, @"Partida\.De\((?!partida\))") || !Regex.IsMatch(c.Value, @"\bPartida\s+partida\s*;")))
                .Select(c => c.Key).ToList();
            CollectionAssert.IsEmpty(soltos, "Partida.De so com o campo `[SerializeField] Partida partida;` do proprio componente");
        }

        [Test]
        public void SoOSaveStateTrocaOCurrent()
        {
            List<string> trocam = CodigoDoRuntime()
                .Where(c => Regex.IsMatch(c.Value, @"SaveState\.Current\s*=[^=]")).Select(c => c.Key).ToList();
            CollectionAssert.IsEmpty(trocam, "trocar o save da partida e do SaveState (Load, NovaVida)");
        }

        [Test]
        public void Imgui_NaoGanhaTelaNova_EOLeitorDeInputNaoDesenha()
        {
            var comOnGui = CodigoDoRuntime().Where(c => Regex.IsMatch(c.Value, @"void\s+OnGUI\s*\(")).Select(c => c.Key).ToList();
            CollectionAssert.DoesNotContain(comOnGui, "Input/PlayerInputReader.cs", "o leitor de input nao desenha UI");
            CollectionAssert.IsEmpty(comOnGui.Where(f => !AindaEmImgui.Contains(f)).ToList(), "tela nova vai em uGUI (Tela.cs)");
            CollectionAssert.IsEmpty(AindaEmImgui.Where(f => !comOnGui.Contains(f)).ToList(),
                "tela da lista ja saiu do IMGUI: tire a linha (a lista so diminui)");
        }

        [Test]
        public void ListaDoSaveState_NaoTemArquivoQueJaSaiu()
        {
            var atuais = new HashSet<string>(CodigoDoRuntime()
                .Where(c => Regex.IsMatch(c.Value, @"SaveState\.(Current|Sessao)\b")).Select(c => c.Key));
            CollectionAssert.IsEmpty(ConsumidoresDoSaveState.Where(f => !atuais.Contains(f)).ToList(),
                "arquivo da lista nao usa mais o SaveState: tire a linha (a lista so diminui)");
        }
    }
}
