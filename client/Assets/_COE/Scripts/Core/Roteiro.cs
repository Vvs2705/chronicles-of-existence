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
    /// segue sem mudar aqui. Botoes de tela (uGUI) sao apertados pelo mesmo metodo que o botao chama.
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

        /// <summary>`-roteiro quebrada`: quebra a promessa da Q-04, assina a Q-07 com um risco (o ULTIMO desfecho de cada
        /// missao) e ignora as opcionais (a rota mais estreita da campanha). Sem valor (ou outro): cumpre a promessa, assina
        /// com o circulo (o PRIMEIRO desfecho) e faz tudo.</summary>
        static bool Quebrada { get { return DevSceneArg.Valor("-roteiro") == "quebrada"; } }

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

            Application.runInBackground = true;   // perder o foco nao pausa a simulacao...
            // ...nem desliga o gamepad virtual: o padrao do Input System desliga dispositivo sem foco, e janela aberta
            // sem foco (alguem usando o PC) criava o pad ja desligado. Antes do AddDevice, e religado a cada toque.
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            pad = InputSystem.AddDevice<Gamepad>("RoteiroPad");
            Ligar();
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

                string motivo;
                Transform rumo = RumoDaMissao.Alvo(s, Quebrada, out motivo);
                if (rumo != null && rumo.GetComponent<SaltoGatilho>() != null) { yield return Saltar(); continue; }
                if (rumo != null && rumo.GetComponent<TrainingDummy>() != null) { yield return Treinar(); continue; }
                Interactable alvo = rumo != null ? rumo.GetComponent<Interactable>() : null;
                if (alvo != null)
                {
                    // q05 (ADR-0010 adendo 10): sem o bicho calmo a conversa nao oferece tratar_o_animal. Como o jogador faria:
                    // vai ate o chapeu e fica parado ao lado dele antes de procurar Lysa ou Tovin.
                    if (motivo == MissaoNaConversa.SoComBichoCalmo)
                    {
                        BichoNoChapeu bicho = FindAnyObjectByType<BichoNoChapeu>();
                        if (bicho != null && !bicho.Calmo) { yield return Acalmar(bicho); continue; }
                    }
                    repetido = motivo == ultimo ? repetido + 1 : 0;
                    ultimo = motivo;
                    if (repetido >= 4) { Falhar(motivo + ": o mesmo passo 5 vezes, nada andou"); break; }
                    // Como o jogador faria: o passo nao andou (ex.: de noite o Borin manda voltar de manha), descansa uma vez.
                    if (repetido == 1) { yield return Descansar(s, motivo + " (nao andou)"); continue; }
                    yield return Interagir(alvo, motivo);
                    continue;
                }

                yield return Descansar(s, motivo);
            }

            yield return Foto("fim");
            Anotar(falhou ? "ROTEIRO FALHOU" : "ROTEIRO OK");
            Salvar();
            Application.Quit(falhou ? 1 : 0);   // verify.ps1 le o codigo de saida (Bloco F)
        }

        IEnumerator Descansar(GameSession s, string motivo)
        {
            Descanso casa = RumoDaMissao.Achar<Descanso>();
            if (casa == null) { Falhar("ninguem a procurar e sem Descanso em cena"); yield break; }
            yield return Interagir(casa, "descansar (" + TimeOfDayCycle.Atual(s.Save.life) + "): " + motivo);
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
            yield return Foto("titulo");
            string destino = DestinyCatalog.Destinos[0].Id;
            string origem = DestinySystem.OrigensDisponiveis(destino)[0].Id;
            // As telas do nascimento, uma foto cada (B01-B05). Reflexao so aqui: o robo e ferramenta de desenvolvimento,
            // e o fluxo de toque dessas telas ja e coberto pelos testes de EntryFlow.
            var f = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
            var campoTela = typeof(EntryFlow).GetField("tela", f);
            typeof(EntryFlow).GetField("destino", f).SetValue(entrada, destino);
            typeof(EntryFlow).GetField("origem", f).SetValue(entrada, origem);
            foreach (string t in new[] { "Limiar", "Destino", "Origem", "Nome", "Certeza" })
            {
                campoTela.SetValue(entrada, System.Enum.Parse(campoTela.FieldType, t));
                yield return Esperar(0.3f);
                yield return Foto("nascimento_" + t.ToLowerInvariant());
            }
            entrada.Nascer(destino, origem, "Robô");
            Anotar("nasceu: " + destino + " / " + origem);
            for (float t = 0f; t < 15f && SceneManager.GetActiveScene().name != EntryFlow.CenaInicial; t += 0.25f)
                yield return Esperar(0.25f);
            yield return Esperar(1.5f);
            yield return Foto("auren_inicio");
            MenuDePausa menu = FindAnyObjectByType<MenuDePausa>();
            if (menu != null)
            {
                menu.Abrir();
                yield return Esperar(0.3f);
                yield return Foto("menu");
                menu.Fechar();
            }
        }

        // ---------------------------------------------------------------- acoes

        /// <summary>Chega perto do alvo, olha para ele e aperta USAR no gamepad. O PlayerInteractor tem de escolher ESTE
        /// alvo (raio e cone de verdade); se escolher outro, e falha de jogo e fica no log. Conversa aberta: fala ate a
        /// missao andar.</summary>
        IEnumerator Interagir(Interactable alvo, string motivo, bool soConversar = false)
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
            yield return CameraAtras(quem.transform);
            if (quem.Alvo != alvo)
            {
                Falhar(motivo + ": o Player nao mira " + nome + " (mira " + (quem.Alvo != null ? quem.Alvo.name : "nada") + ")");
                yield break;
            }
            Anotar(motivo + " -> " + nome);
            DialogueHud hud = FindAnyObjectByType<DialogueHud>();
            PlayerInputReader leitor = FindAnyObjectByType<PlayerInputReader>();
            int antes = interacoes;
            alvo.Interacted += Contar;
            yield return Apertar(GamepadButton.West);
            yield return Esperar(0.2f);
            if (interacoes == antes)
            {
                // Diagnostico: o USAR do gamepad nao chegou ao PlayerInteractor. Aciona pelo mesmo metodo do botao de
                // toque e anota, para separar "input da simulacao" de "regra do jogo".
                Anotar("  USAR do gamepad nao chegou (leitor " + (leitor != null && leitor.isActiveAndEnabled ? "ligado" : "DESLIGADO")
                       + ", interactor " + (quem.isActiveAndEnabled ? "ligado" : "DESLIGADO") + ", pad " + (Gamepad.current == pad ? "atual" : "NAO atual") + (pad.enabled ? "" : " DESLIGADO")
                       + ", foco " + Application.isFocused + ")");
                quem.Interagir();
                yield return Esperar(0.2f);
            }
            alvo.Interacted -= Contar;
            if (hud != null && !hud.Aberta && !string.IsNullOrEmpty(hud.Aviso)) Anotar("  aviso: " + hud.Aviso);
            if (hud == null || !hud.Aberta) yield break;
            for (float t = 0f; t < 6f && hud.Escrevendo; t += 0.1f) yield return Esperar(0.1f);   // fala inteira na foto
            yield return Esperar(0.3f);   // camera assentou no NPC
            yield return Foto((soConversar ? "aos8_" : "fala_") + nome);
            if (soConversar) { hud.Fechar(); yield break; }
            for (int i = 0; i < 12 && hud.Aberta; i++)
            {
                hud.Escolher(Escolha(hud));
                if (!string.IsNullOrEmpty(hud.Aviso)) Anotar("  aviso: " + hud.Aviso);
                yield return Esperar(0.3f);
            }
            if (hud.Aberta) { Anotar("  conversa em laco com " + nome + ": fechada a forca"); hud.Fechar(); }
        }

        /// <summary>Botao da conversa: o pedido de missao da rota (desfecho da rota; opcional so na rota completa); sem
        /// pedido, a primeira fala que segue; senao o ultimo botao (encerra).
        /// Desfecho da rota: o primeiro da missao na rota completa, o ultimo na `quebrada` (Q-04: cumprida/quebrada; Q-07:
        /// circulo/risco). Pela ordem do catalogo, nao pelo nome do evento: missao nova com desfecho nao trava o robo.</summary>
        static int Escolha(DialogueHud hud)
        {
            int missao = -1;
            for (int k = 0; k < hud.Rotulos.Length; k++)
            {
                PedidoDeMissao[] p = hud.PedidosDoBotao(k);
                if (p == null || p.Length == 0) continue;
                QuestDef d = QuestCatalog.Missao(p[0].QuestId);
                if (Quebrada && d != null && !d.Central) continue;
                if (p[0].Intencao.Acao == QuestAcao.EscolherDesfecho)
                {
                    if (d != null && d.Desfechos.Length > 0
                        && p[0].Intencao.ObjetivoId == d.Desfechos[Quebrada ? d.Desfechos.Length - 1 : 0]) return k;
                    continue;
                }
                if (missao < 0) missao = k;
            }
            if (missao >= 0) return missao;
            return hud.QuantasFalasQueSeguem > 0 ? 0 : hud.Rotulos.Length - 1;
        }

        /// <summary>q05, "chegar devagar": chega ao lado do chapeu (dentro do raio de espera) e fica parado, sem botao, ate o
        /// bicho acalmar. O teleporte do Chegar conta como corrida (o chapeu treme) e zera a espera, como o jogo manda.</summary>
        IEnumerator Acalmar(BichoNoChapeu bicho)
        {
            PlayerInteractor quem = FindAnyObjectByType<PlayerInteractor>();
            if (quem == null) { Falhar("sem Player"); yield break; }
            Chegar(quem.transform, bicho.transform.position, ChegarDevagar.RaioDeEspera * 0.6f, 0f);
            yield return CameraAtras(quem.transform);
            float inicio = Time.realtimeSinceStartup;
            while (!bicho.Calmo && Time.realtimeSinceStartup - inicio < ChegarDevagar.SegundosParado + 4f) yield return null;
            yield return Foto("chapeu_bicho_calmo");
            if (bicho.Calmo) Anotar("q05: parado ao lado do chapeu " + (Time.realtimeSinceStartup - inicio).ToString("0.0") + " s, o bicho acalmou");
            else Falhar("q05: parado ao lado do chapeu e o bicho nao acalmou");
        }

        /// <summary>Depois do salto: uma conversa com cada NPC presente, so para ver a fala dos 8 anos (B14).</summary>
        IEnumerator Visitar()
        {
            var npcs = new System.Collections.Generic.List<NpcActor>();
            foreach (Interactable i in Interactable.Ativos) { var n = i as NpcActor; if (n != null && n.Acionavel) npcs.Add(n); }
            foreach (NpcActor n in npcs) yield return Interagir(n, "visita aos 8", true);
            Anotar("visitados aos 8: " + npcs.Count + " NPCs");
        }

        int interacoes;
        void Contar(GameObject quem) { interacoes++; }

        IEnumerator Saltar()
        {
            SaltoGatilho simbolo = RumoDaMissao.Achar<SaltoGatilho>();
            if (simbolo == null) { Falhar("salto liberado e o simbolo nao aparece"); yield break; }
            yield return Interagir(simbolo, "salto");
            SaltoHud hud = FindAnyObjectByType<SaltoHud>();
            if (hud == null || !hud.Aberto) { Falhar("o simbolo nao abriu o aviso do salto"); yield break; }
            yield return Foto("salto_aviso");
            hud.Confirmar();
            yield return Esperar(1.5f);
            yield return Foto("tres_anos_depois");
            Anotar("salto: " + SaveState.Current.ageYears + " anos");
            yield return Visitar();
        }

        /// <summary>B15 contra o parceiro: a cada ciclo dele, um verbo, no tempo que o treino cobra (golpe e magia com a
        /// guarda dele ativa; defesa levantada ao ver o telegrafico).</summary>
        IEnumerator Treinar()
        {
            TrainingDummy parceiro = FindAnyObjectByType<TrainingDummy>();
            PlayerInteractor quem = FindAnyObjectByType<PlayerInteractor>();
            PlayerCombat combate = quem != null ? quem.GetComponent<PlayerCombat>() : null;
            if (parceiro == null || quem == null || combate == null) { Falhar("sem parceiro de treino em cena"); yield break; }
            Chegar(quem.transform, parceiro.transform.position, 1.2f, 0f);
            yield return CameraAtras(quem.transform);
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
                if (b == GamepadButton.LeftTrigger)
                {
                    // Defesa: segura ate o golpe passar (com modelo o impacto sai no quadro do clip, depois do Golpe).
                    Ligar();
                    InputSystem.QueueStateEvent(pad, Com(b));
                    while (parceiro.Fase != DummyFase.Recuperacao && Time.time < limite) yield return null;
                    yield return Esperar(0.5f);
                    InputSystem.QueueStateEvent(pad, new GamepadState());
                    yield return null;
                }
                else yield return Apertar(b);
                while (parceiro.Fase != DummyFase.Recuperacao && Time.time < limite) yield return null;
                if (!fotoDoPainel && combate.Registros > 0) { yield return Foto("treino_painel"); fotoDoPainel = true; }
            }
            Anotar("treino: " + (TrainingProgress.TreinoSupervisionadoFeito(SaveState.Current) ? "os quatro verbos" : "INCOMPLETO")
                   + " (" + combate.Registros + " praticas: " + Praticas() + ")");
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

        void Ligar()
        {
            if (!pad.enabled) InputSystem.EnableDevice(pad);
            if (Gamepad.current != pad) pad.MakeCurrent();
        }

        IEnumerator Apertar(GamepadButton b)
        {
            Ligar();
            InputSystem.QueueStateEvent(pad, Com(b));
            yield return null;
            yield return null;
            InputSystem.QueueStateEvent(pad, new GamepadState());
            yield return null;
        }

        IEnumerator Segurar(GamepadButton b, float segundos)
        {
            Ligar();
            InputSystem.QueueStateEvent(pad, Com(b));
            yield return Esperar(segundos);
            InputSystem.QueueStateEvent(pad, new GamepadState());
            yield return null;
        }

        /// <summary>Gira a camera para tras do Player com o analogico direito (180 graus/s no stick cheio), como o jogador
        /// faria para olhar o NPC por cima do ombro. So muda a foto: nada do jogo depende da camera.</summary>
        IEnumerator CameraAtras(Transform player)
        {
            ThirdPersonCamera cam = FindAnyObjectByType<ThirdPersonCamera>();
            if (cam == null) yield break;
            float delta = Mathf.DeltaAngle(cam.Yaw, player.eulerAngles.y);
            if (Mathf.Abs(delta) < 10f) yield break;
            Ligar();
            InputSystem.QueueStateEvent(pad, new GamepadState { rightStick = new Vector2(Mathf.Sign(delta), 0f) });
            yield return new WaitForSeconds(Mathf.Abs(delta) / 180f);
            InputSystem.QueueStateEvent(pad, new GamepadState());
            yield return Esperar(0.4f);   // o yaw da camera e suavizado
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
