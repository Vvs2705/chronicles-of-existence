using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

namespace COE
{
    /// <summary>Simulacao de partida inteira no PC (so build de desenvolvimento/editor): `COE.exe -roteiro`.
    /// Um robo joga do nascimento ao gancho (B01 -> B16) e fotografa cada etapa em persistentDataPath/roteiro/.
    ///
    /// O QUE E DE VERDADE: interagir, atacar, defender e magia saem de um GAMEPAD VIRTUAL do Input System, entao passam
    /// pelo PlayerInputReader, PlayerInteractor (raio e cone) e PlayerCombat como o dedo do jogador. Quem escolhe o
    /// proximo passo e o proprio dado do jogo (QuestCatalog, MissaoNaConversa, MissaoMundo): missao nova no jogo, o robo
    /// segue sem mudar aqui. Botoes de tela (IMGUI) sao apertados pelo mesmo metodo que o botao chama.
    /// O QUE E ATALHO: o deslocamento. O robo se teleporta para perto do alvo (em lugar livre) em vez de andar; andar
    /// esta provado pelo -autowalk e pelos percursos do AurenSceneTests.
    /// Save proprio (roteiro_save.json, LocalSave.DefaultPath): nunca toca a partida de quem joga no PC.
    /// Saida: roteiro.txt (um passo por linha, "FALHOU" quando trava) e NN_etapa.png. Fecha o jogo ao terminar.
    /// ponytail: politica gulosa (primeira missao que da para andar, na ordem do catalogo; descansa se ninguem estiver
    /// por perto). Escolha de fala = primeira opcao de missao, senao a primeira fala. Variar escolhas (promessa
    /// quebrada, opcionais ignoradas) e um parametro quando a gente quiser cobrir as outras rotas.</summary>
    public class Roteiro : MonoBehaviour
    {
        static bool? ligado;

        /// <summary>-roteiro na linha de comando, em build de desenvolvimento ou no editor.</summary>
        public static bool Ligado
        {
            get
            {
                if (!ligado.HasValue) ligado = Debug.isDebugBuild && DevSceneArg.Tem("-roteiro");
                return ligado.Value;
            }
        }

        const float TempoMaximo = 600f;   // s: travou = falha, nao laco infinito
        const int PassosMaximos = 80;

        Gamepad pad;
        string pasta;
        readonly StringBuilder log = new StringBuilder();
        int foto;
        bool falhou;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Iniciar()
        {
            if (!Ligado) return;
            // Partida nova a cada rodada, ANTES do SaveBootstrap (Awake da primeira cena) ler o save.
            string save = LocalSave.DefaultPath;
            foreach (string f in new[] { save, LocalSave.BackupPath(save) }) if (File.Exists(f)) File.Delete(f);
            var go = new GameObject("Roteiro");
            DontDestroyOnLoad(go);
            go.AddComponent<Roteiro>();
        }

        void Start()
        {
            pasta = Path.Combine(Application.persistentDataPath, "roteiro");
            Directory.CreateDirectory(pasta);
            foreach (string f in Directory.GetFiles(pasta)) File.Delete(f);

            pad = InputSystem.AddDevice<Gamepad>("RoteiroPad");
            pad.MakeCurrent();
            Application.runInBackground = true;   // perder o foco nao pausa a simulacao
            // ...nem desliga o gamepad virtual (o padrao do Input System zera dispositivo sem foco).
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            StartCoroutine(Jogar());
        }

        void OnDestroy() { if (pad != null) InputSystem.RemoveDevice(pad); }

        // ---------------------------------------------------------------- roteiro

        IEnumerator Jogar()
        {
            float inicio = Time.realtimeSinceStartup;
            yield return Nascer();
            string ultimo = null;
            int repetido = 0;

            for (int passo = 0; passo < PassosMaximos && !falhou; passo++)
            {
                if (Time.realtimeSinceStartup - inicio > TempoMaximo) { Falhar("tempo esgotado"); break; }
                yield return Esperar(0.6f);   // MissaoHud conclui 5x/s; NPC troca de lugar no quadro seguinte
                GameSession s = SaveState.Sessao;

                GanchoHud gancho = FindAnyObjectByType<GanchoHud>();
                if (gancho != null && gancho.Aberto)
                {
                    yield return Foto("gancho");
                    gancho.Fechar();
                    Anotar("gancho visto: fim do slice (marco " + s.Historia.Ja(GameSession.MarcoGancho) + ")");
                    break;
                }

                Interactable alvo;
                string motivo = ProximoAlvo(s, out alvo);
                if (alvo != null)
                {
                    repetido = motivo == ultimo ? repetido + 1 : 0;
                    ultimo = motivo;
                    if (repetido >= 4) { Falhar(motivo + ": o mesmo passo 5 vezes, nada andou"); break; }
                    yield return Interagir(alvo, motivo);
                    continue;
                }

                if (SaltoHud.Disponivel(s)) { yield return Saltar(); continue; }
                if (s.Save.ageYears >= 8 && !TrainingProgress.TreinoSupervisionadoFeito(s.Save)) { yield return Treinar(); continue; }

                Descanso casa = Achar<Descanso>();
                if (casa == null) { Falhar("ninguem a procurar e sem Descanso em cena"); break; }
                yield return Interagir(casa, "descansar (" + TimeOfDayCycle.Atual(s.Save.life) + "): " + motivo);
            }

            yield return Foto("fim");
            Anotar(falhou ? "ROTEIRO FALHOU" : "ROTEIRO OK");
            Salvar();
            Application.Quit();
        }

        IEnumerator Nascer()
        {
            EntryFlow entrada = null;
            for (float t = 0f; t < 10f && entrada == null; t += 0.25f)
            {
                entrada = FindAnyObjectByType<EntryFlow>();
                yield return Esperar(0.25f);
            }
            if (entrada == null) { Anotar("sem EntryFlow: segue na cena aberta (" + SceneManager.GetActiveScene().name + ")"); yield break; }

            yield return Esperar(1f);
            yield return Foto("limiar");
            string destino = DestinyCatalog.Destinos[0].Id;
            string origem = DestinySystem.OrigensDisponiveis(destino)[0].Id;
            entrada.Nascer(destino, origem, "Robô");
            Anotar("nasceu: " + destino + " / " + origem);
            for (float t = 0f; t < 15f && SceneManager.GetActiveScene().name != EntryFlow.CenaInicial; t += 0.25f)
                yield return Esperar(0.25f);
            yield return Esperar(1.5f);
            yield return Foto("auren_inicio");
        }

        /// <summary>Primeiro alvo que faz uma missao andar, na ordem do catalogo: NPC que inicia ou cumpre objetivo
        /// (MissaoNaConversa.Participantes), ou o gatilho do objetivo na ancora. Nenhum presente = null e o motivo.</summary>
        static string ProximoAlvo(GameSession s, out Interactable alvo)
        {
            alvo = null;
            var faltando = new StringBuilder();
            foreach (QuestDef d in QuestCatalog.Missoes)
            {
                QuestStatus st = s.Missoes.Estado(d.Id);
                if (st == QuestStatus.Disponivel)
                {
                    alvo = Npc(Npcs(d.Id));
                    if (alvo != null) return "iniciar " + d.Id;
                    faltando.Append(d.Id).Append(" sem quem a ofereca; ");
                    continue;
                }
                if (st != QuestStatus.EmAndamento) continue;

                string[] feitos = s.Missoes.ObjetivosFeitos(d.Id);
                foreach (ObjetivoDef o in d.Objetivos)
                {
                    if (Array.IndexOf(feitos, o.Id) >= 0) continue;
                    alvo = Npc(Npcs(d.Id + "/" + o.Id));
                    if (alvo == null) alvo = Gatilho(d.Id, o.Id);
                    if (alvo != null) return d.Id + "/" + o.Id;
                    faltando.Append(d.Id).Append('/').Append(o.Id).Append(" sem alvo em cena; ");
                    if (d.ObjetivosEmOrdem) break;
                }
            }
            return faltando.Length > 0 ? faltando.ToString() : "nenhuma missao aberta";
        }

        static string[] Npcs(string chave)
        {
            foreach (var p in MissaoNaConversa.Participantes) if (p.Chave == chave) return p.Npcs;
            return new string[0];
        }

        static Interactable Npc(string[] ids)
        {
            foreach (string id in ids)
                foreach (Interactable i in Interactable.Ativos)
                {
                    var n = i as NpcActor;
                    if (n != null && n.NpcId == id && n.Acionavel) return n;
                }
            return null;
        }

        static Interactable Gatilho(string questId, string objetivoId)
        {
            foreach (Interactable i in Interactable.Ativos)
            {
                var g = i as QuestTrigger;
                if (g != null && g.QuestId == questId && g.ObjetivoId == objetivoId) return g;
            }
            return null;
        }

        static T Achar<T>() where T : Interactable
        {
            foreach (Interactable i in Interactable.Ativos) if (i is T && i.Acionavel) return (T)i;
            return null;
        }

        // ---------------------------------------------------------------- acoes

        /// <summary>Chega perto do alvo, olha para ele e aperta USAR no gamepad. O PlayerInteractor tem de escolher ESTE
        /// alvo (raio e cone de verdade); se escolher outro, e falha de jogo e fica no log. Conversa aberta: fala ate a
        /// missao andar.</summary>
        IEnumerator Interagir(Interactable alvo, string motivo)
        {
            PlayerInteractor quem = FindAnyObjectByType<PlayerInteractor>();
            if (quem == null) { Falhar("sem Player"); yield break; }
            // Como o jogador faria: se outro NPC esta mais perto e "rouba" o alvo, da a volta e tenta de outro lado.
            for (int lado = 0; lado < 8; lado++)
            {
                Chegar(quem.transform, alvo.transform.position, 1.1f, lado * 45f);
                yield return Esperar(0.3f);
                if (quem.Alvo == alvo) break;
            }
            string nome = alvo is NpcActor ? ((NpcActor)alvo).NpcId : alvo.GetType().Name;
            if (quem.Alvo != alvo)
            {
                Falhar(motivo + ": o Player nao mira " + nome + " (mira " + (quem.Alvo != null ? quem.Alvo.name : "nada") + ")");
                yield break;
            }
            Anotar(motivo + " -> " + nome);
            yield return Apertar(GamepadButton.West);

            DialogueHud hud = FindAnyObjectByType<DialogueHud>();
            if (hud == null || !hud.Aberta) yield break;
            yield return Foto("fala_" + nome);
            for (int i = 0; i < 12 && hud.Aberta; i++)
            {
                int escolha = hud.QuantasDeMissao > 0 ? hud.PrimeiraDeMissao
                            : hud.QuantasFalasQueSeguem > 0 ? 0 : hud.Rotulos.Length - 1;
                hud.Escolher(escolha);
                if (!string.IsNullOrEmpty(hud.Aviso)) Anotar("  aviso: " + hud.Aviso);
                yield return Esperar(0.3f);
            }
            if (hud.Aberta) { Anotar("  conversa em laco com " + nome + ": fechada a forca"); hud.Fechar(); }
        }

        IEnumerator Saltar()
        {
            SaltoGatilho simbolo = Achar<SaltoGatilho>();
            if (simbolo == null) { Falhar("salto liberado e o simbolo nao aparece"); yield break; }
            yield return Interagir(simbolo, "salto");
            SaltoHud hud = FindAnyObjectByType<SaltoHud>();
            if (hud == null || !hud.Aberto) { Falhar("o simbolo nao abriu o aviso do salto"); yield break; }
            yield return Foto("salto_aviso");
            hud.Confirmar();
            yield return Esperar(1.5f);
            yield return Foto("tres_anos_depois");
            Anotar("salto: " + SaveState.Current.ageYears + " anos");
        }

        /// <summary>B15 contra o parceiro: a cada ciclo dele, um verbo, no tempo que o treino cobra (golpe e magia com a
        /// guarda dele ativa; defesa levantada ao ver o telegrafico).</summary>
        IEnumerator Treinar()
        {
            TrainingDummy parceiro = FindAnyObjectByType<TrainingDummy>();
            PlayerInteractor quem = FindAnyObjectByType<PlayerInteractor>();
            if (parceiro == null || quem == null) { Falhar("sem parceiro de treino em cena"); yield break; }
            Chegar(quem.transform, parceiro.transform.position, 1.2f, 0f);
            Anotar("treino no posto_guarda");
            yield return Esperar(1f);

            GamepadButton[] verbos = { GamepadButton.South, GamepadButton.RightTrigger, GamepadButton.LeftTrigger, GamepadButton.LeftShoulder };
            bool fotoDoPainel = false;
            for (int ciclo = 0; ciclo < 24 && !TrainingProgress.TreinoSupervisionadoFeito(SaveState.Current); ciclo++)
            {
                float limite = Time.time + 6f;
                while (parceiro.Fase != DummyFase.Telegrafico && Time.time < limite) yield return null;
                Olhar(quem.transform, parceiro.transform.position);
                GamepadButton b = verbos[ciclo % verbos.Length];
                if (b == GamepadButton.LeftTrigger) yield return Segurar(b, 0.9f);   // defesa: segura ate o golpe passar
                else yield return Apertar(b);
                while (parceiro.Fase != DummyFase.Recuperacao && Time.time < limite) yield return null;
                if (!fotoDoPainel && TrainingProgress.Registros > 0) { yield return Foto("treino_painel"); fotoDoPainel = true; }
            }
            Anotar("treino: " + (TrainingProgress.TreinoSupervisionadoFeito(SaveState.Current) ? "os quatro verbos" : "INCOMPLETO")
                   + " (" + TrainingProgress.Registros + " praticas: " + Praticas() + ")");
            if (!TrainingProgress.TreinoSupervisionadoFeito(SaveState.Current)) Falhar("treino nao fechou em 24 ciclos");
        }

        static string Praticas()
        {
            var sb = new StringBuilder();
            if (SaveState.Current.life != null && SaveState.Current.life.pratica != null)
                foreach (PracticeEntry p in SaveState.Current.life.pratica) sb.Append(p.activityId).Append('x').Append(p.vezes).Append(' ');
            return sb.ToString().Trim();
        }

        // ---------------------------------------------------------------- corpo e gamepad

        /// <summary>Teleporta a 'distancia' do alvo, no primeiro lado livre (capsula da crianca), olhando para ele.</summary>
        static void Chegar(Transform player, Vector3 alvo, float distancia, float giro)
        {
            var cc = player.GetComponent<CharacterController>();
            Vector3 de = player.position - alvo;
            de.y = 0f;
            float base0 = (de.sqrMagnitude > 0.01f ? Mathf.Atan2(de.x, de.z) * Mathf.Rad2Deg : 0f) + giro;
            Vector3 destino = alvo;
            for (int i = 0; i < 12; i++)
            {
                float a = base0 + (i % 2 == 0 ? 1 : -1) * ((i + 1) / 2) * 30f;
                Vector3 p = alvo + Quaternion.Euler(0f, a, 0f) * Vector3.forward * distancia;
                p.y = alvo.y;
                float r = cc != null ? cc.radius : 0.3f;
                if (!Physics.CheckCapsule(p + Vector3.up * (r + 0.25f), p + Vector3.up * 1.2f, r, Physics.AllLayers, QueryTriggerInteraction.Ignore))
                { destino = p; break; }
            }
            if (cc != null) cc.enabled = false;
            player.position = new Vector3(destino.x, alvo.y + 0.05f, destino.z);
            if (cc != null) cc.enabled = true;
            Olhar(player, alvo);
        }

        static void Olhar(Transform player, Vector3 alvo)
        {
            Vector3 d = alvo - player.position;
            d.y = 0f;
            if (d.sqrMagnitude > 0.001f) player.rotation = Quaternion.LookRotation(d);
        }

        /// <summary>Estado do gamepad com o botao apertado. RT/LT sao EIXOS no GamepadState (WithButton os recusa).</summary>
        static GamepadState Com(GamepadButton b)
        {
            if (b == GamepadButton.RightTrigger) return new GamepadState { rightTrigger = 1f };
            if (b == GamepadButton.LeftTrigger) return new GamepadState { leftTrigger = 1f };
            return new GamepadState().WithButton(b);
        }

        IEnumerator Apertar(GamepadButton b)
        {
            InputSystem.QueueStateEvent(pad, Com(b));
            yield return null;
            yield return null;
            InputSystem.QueueStateEvent(pad, new GamepadState());
            yield return null;
        }

        IEnumerator Segurar(GamepadButton b, float segundos)
        {
            InputSystem.QueueStateEvent(pad, Com(b));
            yield return Esperar(segundos);
            InputSystem.QueueStateEvent(pad, new GamepadState());
            yield return null;
        }

        static IEnumerator Esperar(float s) { yield return new WaitForSecondsRealtime(s); }

        // ---------------------------------------------------------------- saida

        IEnumerator Foto(string etapa)
        {
            yield return new WaitForEndOfFrame();
            string arq = Path.Combine(pasta, (++foto).ToString("00") + "_" + etapa + ".png");
            Texture2D t = ScreenCapture.CaptureScreenshotAsTexture();
            File.WriteAllBytes(arq, t.EncodeToPNG());
            Destroy(t);
        }

        void Anotar(string linha)
        {
            string l = Time.realtimeSinceStartup.ToString("000.0") + "s " + linha;
            Debug.Log("ROTEIRO " + l);
            log.AppendLine(l);
            Salvar();
        }

        void Falhar(string linha) { falhou = true; Anotar("FALHOU: " + linha); }

        void Salvar() { File.WriteAllText(Path.Combine(pasta, "roteiro.txt"), log.ToString()); }
    }
}
