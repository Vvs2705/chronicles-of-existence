using System.Collections.Generic;
using COE.EditorTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace COE.EditorTests
{
    /// <summary>T012 (raia de missoes): AurenSceneBuilder.Populate monta, via MissaoSceneSetup, um QuestTrigger por
    /// objetivo sem NPC (MissaoMundo.Gatilhos) na ancora certa, desligado ate o MissaoHud ligar, sem collider, e o HUD
    /// com a lista de gatilhos ligada por campo (sem Find em runtime). Cena nova em memoria por teste.</summary>
    public class MissaoSceneTests
    {
        [SetUp]
        public void Abrir()
        {
            Scene s = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SceneManager.SetActiveScene(s);
            AurenSceneBuilder.Populate();
        }

        [TearDown]
        public void Fechar() { LogAssert.NoUnexpectedReceived(); }

        static QuestTrigger[] TodosOsGatilhos()
        {
            return AurenSceneBuilder.Achar(MissaoSceneSetup.Raiz).GetComponentsInChildren<QuestTrigger>(true);
        }

        [Test]
        public void UmGatilhoPorObjetivoSemNpc_NaAncoraCerta_DesligadoESemCollider()
        {
            QuestTrigger[] cena = TodosOsGatilhos();
            Assert.AreEqual(MissaoMundo.Gatilhos.Length, cena.Length, "gatilho a mais ou a menos na cena");

            foreach (string[] linha in MissaoMundo.Gatilhos)
            {
                QuestTrigger t = System.Array.Find(cena, x => x.QuestId == linha[0] && x.ObjetivoId == linha[1]);
                Assert.IsNotNull(t, "falta o gatilho " + linha[0] + "." + linha[1]);
                Vector3 d = t.transform.position - AurenSceneBuilder.PosicaoDaAncora(linha[2]);
                d.y = 0f;
                Assert.Less(d.magnitude, 0.01f, t.name + " fora da ancora " + linha[2]);
                Assert.IsFalse(t.gameObject.activeSelf, t.name + ": nasce desligado; o MissaoHud liga so o pendente");
                Assert.IsEmpty(t.GetComponentsInChildren<Collider>(true), t.name + ": collider barraria o percurso");
                Assert.IsNotNull(t.GetComponentInChildren<Renderer>(true), t.name + ": sem marcador visivel");
            }
        }

        [Test]
        public void Q01Acordar_NaoTemGatilhoNaCena()
        {
            // ADR-0007 §6: "acordar" se cumpre sozinho; um gatilho sobrando pediria um toque que nao existe mais.
            foreach (QuestTrigger t in TodosOsGatilhos())
                Assert.AreNotEqual("acordar", t.ObjetivoId, t.name);
        }

        [Test]
        public void Descanso_UmSo_DentroDeCasa_SemCollider_ELongeDaConversaComAFamilia()
        {
            Descanso[] camas = Object.FindObjectsByType<Descanso>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            Assert.AreEqual(1, camas.Length, "ADR-0007 §1: um lugar de descanso, em casa_familia");
            Descanso cama = camas[0];
            Assert.IsTrue(cama.isActiveAndEnabled, "descansar vale sempre: nao depende de missao");
            Assert.IsNotEmpty(cama.Prompt);
            Assert.IsEmpty(cama.GetComponentsInChildren<Collider>(true), "collider barraria o percurso dentro de casa");
            Assert.IsNotNull(cama.GetComponentInChildren<Renderer>(true), "sem nada visivel o jogador nao acha a cama");

            Vector3 d = cama.transform.position - AurenSceneBuilder.PosicaoDaAncora(Descanso.AncoraId);
            d.y = 0f;
            Assert.Less(d.magnitude, 10f, "a cama e da casa da familia");
            // Mara e Daren ficam a RaioDaVaga da ancora; o PlayerInteractor alcanca 2,5 m e escolhe o mais proximo.
            // Mais longe que isso, a cama nunca rouba o alvo de quem esta falando com a familia na porta.
            Assert.Greater(d.magnitude, NpcActor.RaioDaVaga + 2.5f + 2.5f, "a cama disputaria o toque em USAR com a familia");

            // Dentro das paredes de casa_familia (10 x 9 m, centro 5,5 m atras da ancora da porta).
            Transform casa = AurenSceneBuilder.Achar(AurenSceneBuilder.RaizMundo).transform.Find("Construcoes/casa_familia");
            Assert.IsNotNull(casa);
            Vector3 local = casa.InverseTransformPoint(cama.transform.position);
            Assert.Less(Mathf.Abs(local.x), 4.5f, "cama fora da casa (x)");
            Assert.Less(Mathf.Abs(local.z), 4f, "cama fora da casa (z)");
        }

        /// <summary>[PROPOSTA] Para onde ir: a seta segue a historia principal na ordem (q01 com a familia, a porta, q02
        /// com Daren); a opcional q03 (Oren, Nilo) nao rouba a seta.</summary>
        [Test]
        public void Indicador_LigadoNaCameraENoPlayer_EApontaAHistoriaPrincipal()
        {
            GameObject raiz = AurenSceneBuilder.Achar(MissaoSceneSetup.Raiz);
            IndicadorDeObjetivo ind = raiz.GetComponent<IndicadorDeObjetivo>();
            Assert.IsNotNull(ind, "sem indicador de objetivo");
            var so = new SerializedObject(ind);
            Assert.AreSame(AurenSceneBuilder.Achar("Main Camera").GetComponent<Camera>(), so.FindProperty("cam").objectReferenceValue);
            Assert.AreSame(AurenSceneBuilder.Achar("Player").transform, so.FindProperty("player").objectReferenceValue);

            SaveData salvo = SaveState.Current;
            try
            {
                SaveState.Current = new SaveData();
                GameSession s = SaveState.Sessao;
                MissaoHud hud = raiz.GetComponent<MissaoHud>();
                hud.Atualizar();   // q01 comeca sozinha (B06) e "acordar" se cumpre
                Assert.AreEqual("mara", Npc(Rumo(s)), "q01: falar com a familia");
                Assert.IsTrue(s.Missao(m => m.CumprirObjetivo("q01_um_novo_amanhecer", "falar_com_familia")).Ok);
                hud.Atualizar();
                Assert.IsNotNull(Rumo(s).GetComponent<QuestTrigger>(), "q01: sair de casa e o gatilho na porta");
                Assert.IsTrue(s.Missao(m => m.CumprirObjetivo("q01_um_novo_amanhecer", "sair_de_casa")).Ok);
                hud.Atualizar();   // conclui a q01: q02 (central) e q03/q05 (opcionais) ficam disponiveis
                Assert.AreEqual("daren", Npc(Rumo(s)), "a seta segue a q02; opcional nao rouba");
            }
            finally { SaveState.Current = salvo; }
        }

        static Transform Rumo(GameSession s)
        {
            string motivo;
            var cena = new List<Interactable>(Object.FindObjectsByType<Interactable>(FindObjectsSortMode.None));
            Transform t = RumoDaMissao.Alvo(s, true, cena, out motivo);
            Assert.IsNotNull(t, "sem rumo: " + motivo);
            return t;
        }

        static string Npc(Transform t)
        {
            NpcActor n = t.GetComponent<NpcActor>();
            Assert.IsNotNull(n, "o rumo nao e um NPC: " + t.name);
            return n.NpcId;
        }

        /// <summary>O som chega a quem faz barulho: passo e magia do Player, golpe nos dois corpos, clique na conversa e
        /// a caixinha de missao concluida. Sem isso o jogo e mudo (era, ate 2026-10-02).</summary>
        [Test]
        public void Som_UmSo_LigadoEmTodoMundoQueFazBarulho()
        {
            SomDoJogo som = AurenSceneBuilder.Achar(BootstrapSceneBuilder.NomeSom).GetComponent<SomDoJogo>();
            Assert.IsNotNull(som);
            Assert.AreEqual(1, Object.FindObjectsByType<SomDoJogo>(FindObjectsSortMode.None).Length, "dois sons tocam a musica em dobro");
            GameObject player = AurenSceneBuilder.Achar("Player");
            var ligados = new Object[]
            {
                player.GetComponent<CharacterMotor>(), player.GetComponent<PlayerCombat>(), player.GetComponent<HitFlash>(),
                AurenSceneBuilder.Achar("ParceiroDeTreino").GetComponent<HitFlash>(),
                Object.FindAnyObjectByType<DialogueHud>(), AurenSceneBuilder.Achar(MissaoSceneSetup.Raiz).GetComponent<MissaoHud>(),
            };
            foreach (Object o in ligados)
                Assert.AreSame(som, new SerializedObject(o).FindProperty("som").objectReferenceValue, o.GetType().Name + " mudo");
        }

        /// <summary>q05 (ADR-0010 adendo 10): o chapeu mede o Player e a conversa le o bicho calmo, os dois por campo (sem
        /// Find em runtime). Sem isso a opcao tratar_o_animal nunca apareceria.</summary>
        [Test]
        public void ChapeuDaLysa_LigadoNoPlayerENaConversa()
        {
            BichoNoChapeu[] chapeus = Object.FindObjectsByType<BichoNoChapeu>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            Assert.AreEqual(1, chapeus.Length, "um chapeu, o da q05");
            var so = new SerializedObject(chapeus[0]);
            Assert.AreSame(AurenSceneBuilder.Achar("Player").transform, so.FindProperty("player").objectReferenceValue);
            Assert.AreSame(chapeus[0].GetComponent<PecaPorEvento>(), so.FindProperty("peca").objectReferenceValue);
            Assert.AreSame(chapeus[0], new SerializedObject(Object.FindAnyObjectByType<DialogueHud>()).FindProperty("bicho").objectReferenceValue,
                "a conversa nao sabe do bicho calmo");
        }

        [Test]
        public void Hud_TemTodosOsGatilhosLigadosPorCampo()
        {
            MissaoHud hud = AurenSceneBuilder.Achar(MissaoSceneSetup.Raiz).GetComponent<MissaoHud>();
            Assert.IsNotNull(hud, "raiz Missoes sem MissaoHud");

            SerializedProperty lista = new SerializedObject(hud).FindProperty("gatilhos");
            var ligados = new List<Object>();
            for (int i = 0; i < lista.arraySize; i++) ligados.Add(lista.GetArrayElementAtIndex(i).objectReferenceValue);

            CollectionAssert.AreEquivalent(TodosOsGatilhos(), ligados, "o HUD liga/desliga exatamente os gatilhos da cena");
            CollectionAssert.AllItemsAreNotNull(ligados);
        }
    }
}
