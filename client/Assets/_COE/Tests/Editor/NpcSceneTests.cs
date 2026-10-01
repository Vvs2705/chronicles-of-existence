using System;
using COE.EditorTools;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace COE.EditorTests
{
    /// <summary>T012: os dez NPCs de Auren em cena pelo NpcSceneSetup (via AurenSceneBuilder.Populate): na vaga da ancora
    /// da rotina do periodo do save, na escala certa, sem colisor, ligados a ancoras e ao DialogueHud; o HUD ligado ao
    /// Player; e abrir/fechar a conversa interrompe e retoma a agenda e trava/destrava o Player.
    /// Awake/Update nao rodam em teste de Editor: Posicionar/AjustarCorpo sao chamados direto, como o AnchorSpawn.Aplicar.
    /// Nenhum teste aqui pede missao (isso grava o save de verdade): as transicoes estao em DialogueMissaoTests.</summary>
    public class NpcSceneTests
    {
        SaveData salvo;

        [SetUp]
        public void Abrir()
        {
            // Single: em batch mode a cena inicial e "untitled" e NewScene aditiva lanca (ver AurenSceneTests).
            Scene s = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SceneManager.SetActiveScene(s);
            salvo = SaveState.Current;
            SaveState.Current = new SaveData();
        }

        [TearDown]
        public void Fechar()
        {
            SaveState.Current = salvo;
            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void Populate_DezNpcs_NaAncoraDaRotinaDoPeriodoDoSave()
        {
            AurenSceneBuilder.Populate();
            Transform raiz = AurenSceneBuilder.Achar(NpcSceneSetup.RaizNpcs).transform;
            Assert.AreEqual(NpcCatalog.Npcs.Length, raiz.childCount, "um objeto por NPC do catalogo");
            Assert.AreEqual(10, raiz.childCount, "dossie secao G: dez NPCs relevantes");

            foreach (NpcDef def in NpcCatalog.Npcs)
            {
                NpcActor npc = Npc(def.Id);
                NaAncora(npc, NpcCatalog.Onde(def.Id, TimeOfDay.Manha).AncoraId, "posicao de projeto (manha)");

                foreach (TimeOfDay p in new[] { TimeOfDay.Tarde, TimeOfDay.Noite, TimeOfDay.Manha })
                {
                    SaveState.Current.life.timeOfDay = TimeOfDayCycle.Id(p);
                    npc.Posicionar(SaveState.Current);
                    NaAncora(npc, NpcCatalog.Onde(def.Id, p, SaveState.Current.npcs).AncoraId, p.ToString());
                }
            }
        }

        [Test]
        public void Populate_EscalaCerta_SemColisor_ELigacoesFeitas()
        {
            AurenSceneBuilder.Populate();
            Transform ancoras = AurenSceneBuilder.Achar(AurenSceneBuilder.RaizAncoras).transform;
            DialogueHud hud = Hud();

            foreach (NpcDef def in NpcCatalog.Npcs)
            {
                NpcActor npc = Npc(def.Id);
                bool crianca = def.Id == "nilo" || def.Id == "sera";   // docs/arte/PIPELINE.md §3.1
                Transform corpo = (Transform)Ref(npc, "corpo");
                Assert.IsNotNull(corpo, def.Id + ": sem corpo ligado");
                Assert.AreEqual(crianca ? BodyScale.Crianca5 : BodyScale.Adulto, corpo.lossyScale.y * 2f, 0.01f, def.Id + ": altura");
                Assert.AreEqual(npc.transform.position.y, corpo.position.y - corpo.lossyScale.y, 0.01f, def.Id + ": pes no chao");
                // Solido como gente: uma capsula no corpo, que o CharacterController do Player nao atravessa.
                Collider[] colisores = npc.GetComponentsInChildren<Collider>();
                Assert.AreEqual(1, colisores.Length, def.Id + ": o NPC tem de ser solido (um colisor no corpo)");
                Assert.IsFalse(colisores[0].isTrigger, def.Id + ": colisor gatilho nao barra o Player");
                Assert.AreSame(corpo, colisores[0].transform, def.Id + ": colisor fora do corpo nao cresce no salto");
                Assert.AreEqual(NpcSceneSetup.RaioDoCorpo, ((CapsuleCollider)colisores[0]).radius, 1e-4f, def.Id);

                Assert.AreSame(ancoras, Ref(npc, "ancoras"), def.Id + ": ancoras nao ligadas");
                Assert.AreSame(hud, Ref(npc, "dialogo"), def.Id + ": HUD de dialogo nao ligado");
                Assert.AreEqual(def.Id, npc.NpcId);
                Assert.AreSame(def, npc.Agenda.Npc, def.Id + ": agenda sem definicao do catalogo");
                Assert.IsNotEmpty(npc.Prompt);
            }

            // Salto (dossie §G: "nao congelar amigos"): aos 8 a crianca cresce, o adulto nao.
            NpcActor nilo = Npc("nilo");
            nilo.AjustarCorpo(8);
            Assert.AreEqual(BodyScale.Crianca8, ((Transform)Ref(nilo, "corpo")).lossyScale.y * 2f, 0.01f);
            NpcActor borin = Npc("borin");
            borin.AjustarCorpo(8);
            Assert.AreEqual(BodyScale.Adulto, ((Transform)Ref(borin, "corpo")).lossyScale.y * 2f, 0.01f);
        }

        [Test]
        public void Populate_HudLigadoAoPlayer()
        {
            AurenSceneBuilder.Populate();
            GameObject player = AurenSceneBuilder.Achar("Player");
            DialogueHud hud = Hud();

            SerializedProperty travar = new SerializedObject(hud).FindProperty("travarNaConversa");
            var ligados = new Object[travar.arraySize];
            for (int i = 0; i < ligados.Length; i++) ligados[i] = travar.GetArrayElementAtIndex(i).objectReferenceValue;
            CollectionAssert.Contains(ligados, player.GetComponent<CharacterMotor>(), "o Player andaria durante a conversa");
            CollectionAssert.Contains(ligados, player.GetComponent<PlayerCombat>(), "o Player atacaria durante a conversa");
            CollectionAssert.Contains(ligados, player.GetComponent<PlayerInteractor>(), "a conversa reabriria a cada toque em USAR");
            Assert.AreSame(player.GetComponent<CharacterAnimator>(), Ref(hud, "anim"));
        }

        [Test]
        public void InteragirAbre_FecharRetoma_AgendaEPlayer()
        {
            AurenSceneBuilder.Populate();
            GameObject player = AurenSceneBuilder.Achar("Player");
            CharacterMotor motor = player.GetComponent<CharacterMotor>();
            PlayerCombat combate = player.GetComponent<PlayerCombat>();
            DialogueHud hud = Hud();
            NpcActor borin = Npc("borin"), lysa = Npc("lysa");
            Vector3 naForja = borin.transform.position;

            borin.Interact(player);   // o que o PlayerInteractor chama no toque em USAR

            Assert.IsTrue(hud.Aberta, "interagir com o NPC abre a conversa");
            Assert.AreSame(borin, hud.Npc);
            Assert.AreSame(Interrupcao.Conversa, borin.Agenda.Atual, "abrir interrompe a rotina");
            Assert.IsNotNull(hud.No, "Borin tem fala: entra num no do grafo");
            Assert.IsNotEmpty(hud.Fala);
            Assert.IsFalse(motor.enabled, "em conversa o personagem nao anda");
            Assert.IsFalse(combate.enabled, "em conversa o personagem nao ataca");

            Assert.IsFalse(hud.Abrir(lysa), "uma conversa por vez");
            Assert.IsNull(lysa.Agenda.Atual, "a conversa recusada nao interrompe ninguem");

            // Retomar nao restaura foto: se o periodo virou durante a conversa, ele vai para a rotina NOVA.
            SaveState.Current.life.timeOfDay = TimeOfDayCycle.IdNoite;
            borin.Posicionar(SaveState.Current);
            Assert.AreEqual(naForja, borin.transform.position, "em conversa o NPC fica onde esta");

            int despedir = Array.IndexOf(hud.Rotulos, Strings.Get("dialogo.opcao.despedir"));
            Assert.GreaterOrEqual(despedir, 0, "todo no tem saida (fallback do grafo)");
            hud.Escolher(despedir);

            Assert.IsFalse(hud.Aberta, "despedir fecha");
            Assert.IsNull(borin.Agenda.Atual, "fechar retoma a rotina");
            Assert.IsTrue(motor.enabled && combate.enabled, "fechar devolve o controle");
            borin.Posicionar(SaveState.Current);
            NaAncora(borin, NpcCatalog.Onde("borin", TimeOfDay.Noite).AncoraId, "rotina da noite depois da conversa");
        }

        [Test]
        public void NiloDesaparecido_SemCorpoESemConversa_EVoltaQuandoARotinaMuda()
        {
            AurenSceneBuilder.Populate();
            GameObject player = AurenSceneBuilder.Achar("Player");
            NpcActor nilo = Npc("nilo"), sera = Npc("sera");
            Transform corpo = (Transform)Ref(nilo, "corpo");
            Assert.IsFalse(nilo.Ausente);
            Assert.IsTrue(nilo.Acionavel && corpo.gameObject.activeSelf, "pressuposto: Nilo comeca em Auren");

            // ADR-0007 §3: a memoria do sumico (o que GameSession.Sincronizar poe no save ao concluir a Q-04).
            NpcMemory.Registrar(SaveState.Current.npcs, "nilo", QuestCatalog.EventoNiloDesapareceu, Importancia.Marcante, 1);
            foreach (TimeOfDay p in new[] { TimeOfDay.Manha, TimeOfDay.Tarde, TimeOfDay.Noite })
            {
                SaveState.Current.life.timeOfDay = TimeOfDayCycle.Id(p);
                nilo.Posicionar(SaveState.Current);
                sera.Posicionar(SaveState.Current);
                Assert.IsTrue(nilo.Ausente, "Nilo aparece de " + p);
                Assert.IsFalse(corpo.gameObject.activeSelf, "ausente nao tem corpo em cena (" + p + ")");
                Assert.IsFalse(nilo.Acionavel, "ausente nao vira alvo do PlayerInteractor (" + p + ")");
                Assert.IsTrue(nilo.isActiveAndEnabled, "o componente segue ligado: e ele que traz Nilo de volta");
                Assert.IsTrue(sera.Acionavel && !sera.Ausente, "so Nilo some");
            }

            nilo.Interact(player);
            Assert.IsFalse(Hud().Aberta, "ninguem conversa com quem nao esta");
            Assert.IsNull(nilo.Agenda.Atual);

            // Rotina que volta a apontar para Auren (na leva B, a do pos-salto): corpo e conversa de volta.
            SaveState.Current.npcs = new NpcBook();
            nilo.Posicionar(SaveState.Current);
            Assert.IsFalse(nilo.Ausente);
            Assert.IsTrue(corpo.gameObject.activeSelf && nilo.Acionavel);
            NaAncora(nilo, NpcCatalog.Onde("nilo", TimeOfDay.Noite).AncoraId, "de volta a rotina");
        }

        /// <summary>Visto na simulacao -roteiro: o botao da missao ficava DEPOIS de "Encerrar conversa". Na tela: falas
        /// que seguem, missoes, e o que encerra por ultimo; e cada botao faz o que o rotulo diz.</summary>
        [Test]
        public void Conversa_MissaoVemAntesDoEncerrar_ECadaBotaoFazOQueDiz()
        {
            AurenSceneBuilder.Populate();
            QuestSystem m = SaveState.Sessao.Missoes;
            const string q01 = "q01_um_novo_amanhecer", q02 = "q02_uma_pequena_responsabilidade";
            Assert.IsTrue(m.Iniciar(q01).Ok);
            foreach (ObjetivoDef o in QuestCatalog.Missao(q01).Objetivos) Assert.IsTrue(m.CumprirObjetivo(q01, o.Id).Ok, o.Id);
            Assert.IsTrue(m.Concluir(q01).Ok);
            Assert.AreEqual(QuestStatus.Disponivel, m.Estado(q02), "pressuposto: Daren oferece a q02");

            DialogueHud hud = Hud();
            Assert.IsTrue(hud.Abrir(Npc("daren")));
            string missao = Strings.Get(QuestCatalog.Missao(q02).TituloKey);
            int iMissao = System.Array.IndexOf(hud.Rotulos, missao);
            Assert.AreEqual(hud.PrimeiraDeMissao, iMissao, "botao da missao: " + string.Join(" | ", hud.Rotulos));
            Assert.Less(iMissao, hud.Rotulos.Length - 1, "a missao nao pode ser o ultimo botao (o ultimo encerra): "
                        + string.Join(" | ", hud.Rotulos));

            hud.Escolher(iMissao);
            Assert.AreEqual(QuestStatus.EmAndamento, m.Estado(q02), "o botao com o titulo da missao inicia a missao");
            Assert.IsTrue(hud.Aberta, "pedir missao nao fecha a conversa");
            hud.Escolher(hud.Rotulos.Length - 1);
            Assert.IsFalse(hud.Aberta, "o ultimo botao encerra");
        }

        [Test]
        public void Conversa_SempreTemSaida_ENpcOcupado_NaoAbre()
        {
            AurenSceneBuilder.Populate();
            GameObject player = AurenSceneBuilder.Achar("Player");
            DialogueHud hud = Hud();
            NpcActor tovin = Npc("tovin");

            // Desde a leva A todo NPC tem fala escrita; o ultimo botao de cada no leva para fora da conversa.
            Assert.IsTrue(hud.Abrir(tovin));
            Assert.IsNotNull(hud.No, "Tovin tem fala escrita");
            for (int i = 0; i < 8 && hud.Aberta; i++) hud.Escolher(hud.Rotulos.Length - 1);
            Assert.IsFalse(hud.Aberta, "sempre ha como sair");
            Assert.IsNull(tovin.Agenda.Atual);

            var incendio = new Interrupcao("evento.incendio", Interrupcao.PrioridadeEvento, null, "atividade.apagar_fogo");
            Assert.IsTrue(tovin.Agenda.Interromper(incendio));
            Assert.IsFalse(hud.Abrir(tovin), "evento da vila vence a conversa");
            Assert.IsFalse(hud.Aberta);
            Assert.IsNotNull(hud.Aviso, "o jogador ve que o NPC esta ocupado");
            Assert.AreSame(incendio, tovin.Agenda.Atual, "a conversa recusada nao derruba o evento");
            Assert.IsTrue(player.GetComponent<CharacterMotor>().enabled, "recusa nao trava o Player");
        }

        // ---------------------------------------------------------------- utilidades

        static NpcActor Npc(string id)
        {
            Transform t = AurenSceneBuilder.Achar(NpcSceneSetup.RaizNpcs).transform.Find(id);
            Assert.IsNotNull(t, "falta o NPC '" + id + "'");
            NpcActor npc = t.GetComponent<NpcActor>();
            Assert.IsNotNull(npc, id + ": sem NpcActor");
            return npc;
        }

        static DialogueHud Hud()
        {
            DialogueHud hud = AurenSceneBuilder.Achar(NpcSceneSetup.NomeHud).GetComponent<DialogueHud>();
            Assert.IsNotNull(hud, "sem DialogueHud");
            return hud;
        }

        /// <summary>Na vaga do NPC em volta da ancora (NpcActor.RaioDaVaga), no chao da ancora.</summary>
        static void NaAncora(NpcActor npc, string ancoraId, string quando)
        {
            Vector3 a = AurenSceneBuilder.PosicaoDaAncora(ancoraId);
            Vector3 d = npc.transform.position - a;
            Assert.AreEqual(0f, d.y, 0.01f, npc.NpcId + " (" + quando + "): fora do chao da ancora " + ancoraId);
            d.y = 0f;
            Assert.AreEqual(NpcActor.RaioDaVaga, d.magnitude, 0.01f, npc.NpcId + " (" + quando + "): fora da vaga em " + ancoraId);
        }

        static Object Ref(Object alvo, string campo)
        {
            SerializedProperty p = new SerializedObject(alvo).FindProperty(campo);
            Assert.IsNotNull(p, alvo.GetType().Name + " nao tem o campo '" + campo + "'");
            return p.objectReferenceValue;
        }
    }
}
