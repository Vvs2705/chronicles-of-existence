using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace COE.PlayModeTests
{
    /// <summary>Passo 6 (2026-10-04): com a sessao injetada pela Partida, as telas que GRAVAM rodam em PlayMode sem tocar no
    /// save.json do PC (o "gravar" e um contador). Cartao de missao, aviso do salto e o gancho, pelos botoes de verdade:
    /// alvos >= 48 dp na area segura, sem sobreposicao, e a transicao certa (ou nenhuma) na sessao.</summary>
    public class TelasDaPartidaTests
    {
        readonly List<GameObject> criados = new List<GameObject>();
        int gravacoes;

        [TearDown]
        public void Limpar()
        {
            foreach (GameObject g in criados) if (g != null) Object.Destroy(g);
            criados.Clear();
        }

        GameSession Sessao(SaveData s) { gravacoes = 0; return new GameSession(s, () => gravacoes++); }

        /// <summary>Monta o componente desligado, liga a Partida (campo privado `partida`, como o gerador faz) e acorda.</summary>
        T Montar<T>(GameSession sessao) where T : MonoBehaviour
        {
            var save = new GameObject("Save");
            criados.Add(save);
            Partida partida = save.AddComponent<Partida>();
            partida.Usar(sessao);
            return Montar<T>(partida, typeof(T).Name);
        }

        T Montar<T>(Partida partida, string nome) where T : MonoBehaviour
        {
            var go = new GameObject(nome);
            criados.Add(go);
            go.SetActive(false);
            T c = go.AddComponent<T>();
            FieldInfo f = typeof(T).GetField("partida", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(f, typeof(T).Name + " sem o campo 'partida'");
            f.SetValue(c, partida);
            return c;
        }

        static T Acordar<T>(T c) where T : MonoBehaviour { c.gameObject.SetActive(true); return c; }

        static void ConcluirCentrais(GameSession g)
        {
            List<string> centrais = QuestCatalog.Missoes.Where(d => d.Central).Select(d => d.Id).ToList();
            for (int volta = 0; volta < centrais.Count; volta++)
                foreach (string id in centrais)
                {
                    if (g.Missoes.Estado(id) != QuestStatus.Disponivel) continue;
                    QuestDef d = QuestCatalog.Missao(id);
                    g.Missao(m => m.Iniciar(id));
                    if (d.Desfechos.Length > 0) g.Missao(m => m.EscolherDesfecho(id, d.Desfechos[0]));
                    foreach (ObjetivoDef o in d.Objetivos)
                        if (!g.Missoes.ObjetivosFeitos(id).Contains(o.Id)) g.Missao(m => m.CumprirObjetivo(id, o.Id));
                    g.Missao(m => m.Concluir(id));
                }
        }

        [UnityTest]
        public IEnumerator CartaoDeMissao_MostraAMissaoQueComecou_NaAreaSegura()
        {
            GameSession g = Sessao(new SaveData());
            MissaoHud hud = Acordar(Montar<MissaoHud>(g));
            yield return null;
            yield return null;

            const string q01 = "q01_um_novo_amanhecer";
            Assert.AreEqual(QuestStatus.EmAndamento, g.Missoes.Estado(q01), "a q01 comeca sozinha ao abrir Auren (B06)");
            Assert.Greater(gravacoes, 0, "e grava pela sessao do teste, nao no save do PC");
            Assert.IsNotNull(hud.Cartao, "sem cartao de missao");
            StringAssert.Contains(Strings.Get(QuestCatalog.Missao(q01).TituloKey), hud.Cartao.GetComponentInChildren<Text>().text);
            Rect r = UiChecagem.Retangulo(hud.Cartao), safe = Screen.safeArea;
            Assert.IsTrue(r.xMin >= safe.xMin - 0.5f && r.xMax <= safe.xMax + 0.5f && r.yMax <= safe.yMax + 0.5f, "cartao fora da area segura " + r);
        }

        [UnityTest]
        public IEnumerator AvisoDoSalto_AbrePeloBotao_AindaNaoFechaSemMudarNada()
        {
            var s = new SaveData();
            GameSession g = Sessao(s);
            ConcluirCentrais(g);
            Assert.IsTrue(SaltoHud.Disponivel(g), "com a Q-08 concluida aos 5, o salto esta liberado");
            int antes = gravacoes;
            SaltoHud hud = Acordar(Montar<SaltoHud>(g));
            yield return null;
            yield return null;
            Assert.IsFalse(UiFundo.HaModal, "so o botao, sem modal");

            UiChecagem.BotoesUsaveis(hud.Vista, "botao seguir adiante");
            UiChecagem.Botao(hud.Vista, "Seguir").onClick.Invoke();
            yield return null;
            Assert.IsTrue(hud.Aberto, "o aviso do B12 abriu");
            Assert.IsTrue(UiFundo.HaModal, "aviso aberto marca o modal: a HUD de toque some (ToqueHudTests)");
            UiChecagem.BotoesUsaveis(hud.Vista, "aviso do salto");

            // B13: o confirmar do aviso so pede a certeza (segundo passo); "Voltar" volta ao aviso. Ninguem cresce.
            UiChecagem.Botao(hud.Vista, "Confirmar").onClick.Invoke();
            yield return null;
            Assert.IsTrue(hud.NaCerteza, "o primeiro toque para na certeza");
            Assert.AreEqual(5, s.ageYears, "a certeza ainda nao salta");
            UiChecagem.BotoesUsaveis(hud.Vista, "certeza do salto");
            UiChecagem.Botao(hud.Vista, "AindaNao").onClick.Invoke();
            yield return null;
            Assert.IsTrue(hud.Aberto && !hud.NaCerteza, "voltar da certeza volta ao aviso, aberto");

            UiChecagem.Botao(hud.Vista, "AindaNao").onClick.Invoke();
            yield return null;
            Assert.IsFalse(hud.Aberto);
            yield return null;
            yield return null;
            Assert.IsFalse(UiFundo.HaModal, "fechado, os controles voltam");
            Assert.AreEqual(5, s.ageYears, "ainda nao: ninguem cresceu (R18)");
            Assert.AreEqual(antes, gravacoes, "abrir e fechar o aviso nao grava nada");
        }

        [UnityTest]
        public IEnumerator Gancho_AbreQuandoOTreinoTermina_ContinuarGravaOMarcoUmaVez()
        {
            var s = new SaveData();
            GameSession g = Sessao(s);
            ConcluirCentrais(g);
            Assert.IsTrue(g.ConfirmarSalto(g.PrepararSalto()).Aplicado);
            foreach (AtividadeDef a in new[] { TrainingProgress.AtividadeLeve, TrainingProgress.AtividadeForte,
                                               TrainingProgress.AtividadeEsquiva, TrainingProgress.AtividadeMagia })
                g.Praticar(a);
            Assert.IsTrue(g.GanchoPendente());
            int antes = gravacoes;
            GanchoHud hud = Acordar(Montar<GanchoHud>(g));
            yield return null;
            yield return null;

            Assert.IsTrue(hud.Aberto, "o gancho (B16) abre sozinho quando o treino termina");
            Assert.IsTrue(UiFundo.HaModal, "gancho aberto marca o modal");
            Canvas tela = hud.GetComponentInChildren<Canvas>();
            UiChecagem.BotoesUsaveis(tela, "gancho");
            UiChecagem.Botao(tela, "Continuar").onClick.Invoke();
            yield return null;
            yield return null;

            Assert.IsFalse(hud.Aberto);
            Assert.IsFalse(UiFundo.HaModal, "fechado, os controles voltam");
            Assert.IsTrue(g.Historia.Ja(GameSession.MarcoGancho), "o marco do fim do slice esta no historico");
            Assert.AreEqual(antes + 1, gravacoes, "uma gravacao, pela sessao do teste");
            Assert.IsFalse(g.GanchoPendente(), "o gancho nao volta");
        }

        [UnityTest]
        public IEnumerator Conversa_MarcaOModalDesdeOQuadroEmQueAbre()
        {
            GameSession g = Sessao(new SaveData());
            var save = new GameObject("Save");
            criados.Add(save);
            Partida partida = save.AddComponent<Partida>();
            partida.Usar(g);
            DialogueHud hud = Acordar(Montar<DialogueHud>(partida, "Conversa"));
            NpcActor borin = Montar<NpcActor>(partida, "borin");
            typeof(NpcActor).GetField("npcId", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(borin, "borin");
            Acordar(borin);
            yield return null;
            yield return null;
            Assert.IsFalse(UiFundo.HaModal);

            Assert.IsTrue(hud.Abrir(borin), "conversa com o Borin abre");
            Assert.IsTrue(UiFundo.HaModal, "ja no quadro da abertura: a HUD de toque e o prompt nao piscam por cima do painel");
            yield return null;
            yield return null;
            Assert.IsTrue(UiFundo.HaModal, "e a cada quadro com ela aberta");
            hud.Fechar();
            yield return null;
            yield return null;
            Assert.IsFalse(UiFundo.HaModal, "fechada, os controles voltam");
        }
    }
}
