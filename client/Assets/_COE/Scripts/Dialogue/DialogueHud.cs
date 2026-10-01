using System;
using System.Collections.Generic;
using UnityEngine;

namespace COE
{
    /// <summary>A conversa com um NPC na tela (T012). ORQUESTRA, nao decide: a fala e as opcoes vem do DialogueRunner,
    /// as opcoes de missao do MissaoNaConversa, e todo pedido de missao vai por SaveState.Sessao.Missao (a missao
    /// decide; sincroniza memoria/reputacao e grava UMA vez -- obrigatorio 8).
    ///
    /// ABRIR: agenda.Interromper(Conversa); se o NPC ja esta ocupado (evento da vila), nada abre e o aviso diz isso.
    /// Aberta: motor, combate e interacao do Player ficam desligados (o personagem nao anda nem ataca).
    /// FECHAR: agenda.Retomar(Conversa.Id) e religa o que desligou. O NPC volta a rotina do periodo ATUAL.
    ///
    /// Opcoes, na ordem: as da fala (grafo), as de missao deste NPC, e "despedir" quando o no nao tem saida propria
    /// (no terminal ou NPC sem fala escrita, 7 de 10 hoje). Texto so por Strings: sem fala escrita aparece "[chave]".
    /// ponytail: IMGUI de prototipo (mesmo padrao do PlayerInteractor/PerfHud), so toque/mouse; gamepad e a UI de
    /// verdade (Canvas) sao a T013. Estado so muda em Abrir/Escolher/Fechar; o OnGUI so desenha strings prontas.</summary>
    public class DialogueHud : MonoBehaviour
    {
        [Tooltip("Desligados enquanto a conversa esta aberta: motor, combate e interacao do Player (ligados pelo gerador).")]
        [SerializeField] Behaviour[] travarNaConversa = new Behaviour[0];
        [Tooltip("Opcional: zera a velocidade do Animator ao abrir (motor desligado congelaria o passo do ultimo quadro).")]
        [SerializeField] CharacterAnimator anim;

        const float SegundosDeAviso = 2.5f;

        /// <summary>O NPC da conversa aberta; null = fechada.</summary>
        public NpcActor Npc { get; private set; }
        public bool Aberta { get { return Npc != null; } }
        /// <summary>No atual; null = NPC sem fala escrita.</summary>
        public DialogueNode No { get; private set; }
        public string Fala { get; private set; }
        /// <summary>Rotulos dos botoes, na ordem que Escolher recebe.</summary>
        public string[] Rotulos { get; private set; }
        /// <summary>"NPC ocupado" ou o motivo da recusa da missao; null = nada a avisar.</summary>
        public string Aviso { get; private set; }
        /// <summary>Indice (em Rotulos) do primeiro botao de missao; -1 = nenhum.</summary>
        public int PrimeiraDeMissao { get { return deMissao.Length == 0 ? -1 : Array.IndexOf(ordem, autorais.Length); } }
        public int QuantasDeMissao { get { return deMissao.Length; } }
        /// <summary>Falas do grafo que seguem a conversa (vem primeiro em Rotulos).</summary>
        public int QuantasFalasQueSeguem { get; private set; }

        /// <summary>O que o botao pede a missao (opcao de missao, ou fala do grafo com pedido); null = so conversa.</summary>
        public PedidoDeMissao[] PedidosDoBotao(int botao)
        {
            if (botao < 0 || botao >= ordem.Length) return null;
            int i = ordem[botao];
            if (i < autorais.Length) return autorais[i].Pedido != null ? new[] { autorais[i].Pedido } : null;
            i -= autorais.Length;
            return i < deMissao.Length ? deMissao[i].Pedidos : null;
        }

        DialogueGraph grafo;
        DialogueContext ctx;
        DialogueOption[] autorais = new DialogueOption[0];
        OpcaoDeMissao[] deMissao = new OpcaoDeMissao[0];
        bool[] desligados = new bool[0];
        int[] ordem = new int[0];   // botao na tela -> indice logico (autorais, depois missao, depois despedir)
        string nome;
        float avisoAte;
        GUIStyle estiloNome, estiloFala, estiloBotao, estiloAviso;

        // Awake: antes do primeiro OnGUI do PlayerInteractor, que cacheia o nome do NPC (NpcActor.Prompt).
        void Awake()
        {
            useGUILayout = false;
            StringsLoader.EnsureLoaded();
        }

        /// <summary>Abre a conversa. false = ja ha uma aberta, ou o NPC esta ocupado (Aviso explica).</summary>
        public bool Abrir(NpcActor npc)
        {
            if (npc == null || Aberta) return false;
            if (!npc.Agenda.Interromper(Interrupcao.Conversa))
            {
                Avisar(Strings.Get("dialogo.npc_ocupado"));
                return false;
            }

            Npc = npc;
            nome = npc.Prompt;
            GameSession s = SaveState.Sessao;
            ctx = new DialogueContext
            {
                NpcId = npc.NpcId,
                Periodo = TimeOfDayCycle.Atual(s.Save.life),
                Memoria = s.Save.npcs,
                EstadoDaMissao = QuestIntentAdapter.Leitor(s.Missoes),
                Confianca = s.Reputacao.ConfiancaNo,
            };
            grafo = DialogueCatalog.Do(npc.NpcId);
            Travar();
            IrPara(DialogueRunner.Entrada(grafo, ctx));
            return true;
        }

        /// <summary>O jogador tocou o botao i (indice em Rotulos). Fala do grafo: pede a missao se a fala emitiu pedido
        /// (recusa = fica no no e avisa o motivo) e segue o grafo. Opcao de missao: pede e fica no no, com as opcoes
        /// refeitas. Ultimo botao sem saida propria: fecha.</summary>
        public void Escolher(int i)
        {
            if (!Aberta || Rotulos == null || i < 0 || i >= Rotulos.Length) return;
            i = ordem[i];

            if (i < autorais.Length)
            {
                DialogueStep passo = DialogueRunner.Escolher(grafo, No, i, ctx);
                if (!passo.Ok) return;
                if (passo.Pedido != null && !Pedir(new[] { passo.Pedido })) return;
                if (passo.Encerrou) Fechar();
                else IrPara(passo.Proximo);
                return;
            }

            i -= autorais.Length;
            if (i < deMissao.Length)
            {
                Pedir(deMissao[i].Pedidos);
                IrPara(No);
                return;
            }
            Fechar();
        }

        /// <summary>Fecha a conversa: a agenda retoma a rotina e o Player volta a andar. Idempotente.</summary>
        public void Fechar()
        {
            if (!Aberta) return;
            Npc.Agenda.Retomar(Interrupcao.Conversa.Id);
            for (int i = 0; i < travarNaConversa.Length && i < desligados.Length; i++)
                if (desligados[i] && travarNaConversa[i] != null) travarNaConversa[i].enabled = true;
            Npc = null;
            No = null;
            grafo = null;
            ctx = null;
            Fala = null;
            Rotulos = null;
        }

        void Travar()
        {
            if (desligados.Length != travarNaConversa.Length) desligados = new bool[travarNaConversa.Length];
            for (int i = 0; i < travarNaConversa.Length; i++)
            {
                Behaviour b = travarNaConversa[i];
                desligados[i] = b != null && b.enabled;   // so religa o que ESTA conversa desligou
                if (desligados[i]) b.enabled = false;
            }
            if (anim != null) anim.SetSpeed(0f);
        }

        void IrPara(DialogueNode no)
        {
            No = no;
            Fala = no == null ? Strings.Get("dialogo.sem_fala") : DialogueRunner.Fala(no, ctx, null);
            autorais = DialogueRunner.Opcoes(grafo, no, ctx);
            deMissao = MissaoNaConversa.Opcoes(Npc.NpcId, SaveState.Sessao.Missoes, autorais);

            bool fecharExtra = autorais.Length == 0;   // no com opcoes ja tem saida incondicional (DialogueGraph.Validar)
            int n = autorais.Length + deMissao.Length + (fecharExtra ? 1 : 0);
            // Na tela: falas que seguem, missoes, e por ultimo o que encerra (fala sem proximo no ou o despedir). Sem
            // isso a missao ficava DEPOIS de "Encerrar conversa" (visto na simulacao -roteiro, 2026-10-01).
            var o = new List<int>(n);
            for (int i = 0; i < autorais.Length; i++) if (!Encerra(autorais[i])) o.Add(i);
            QuantasFalasQueSeguem = o.Count;
            for (int i = 0; i < deMissao.Length; i++) o.Add(autorais.Length + i);
            for (int i = 0; i < autorais.Length; i++) if (Encerra(autorais[i])) o.Add(i);
            if (fecharExtra) o.Add(n - 1);
            ordem = o.ToArray();

            var r = new string[n];
            for (int k = 0; k < n; k++)
            {
                int i = ordem[k];
                r[k] = i < autorais.Length ? Strings.Get(autorais[i].TextoKey)
                     : i - autorais.Length < deMissao.Length ? Strings.Get(deMissao[i - autorais.Length].TextoKey)
                     : Strings.Get("dialogo.opcao.despedir");
            }
            Rotulos = r;
        }

        static bool Encerra(DialogueOption o) { return string.IsNullOrEmpty(o.ProximoNoId) && o.Pedido == null; }

        bool Pedir(PedidoDeMissao[] pedidos)
        {
            // Conversa aconteceu na ancora da rotina do NPC: e ali que o jogador esta, na mesma gravacao do pedido.
            if (Npc != null && Npc.Rotina != null) SaveState.Sessao.Posicao(gameObject.scene.name, Npc.Rotina.AncoraId);
            QuestResultado res = SaveState.Sessao.Missao(m => MissaoNaConversa.Aplicar(m, pedidos));
            if (!res.Ok) Avisar(Strings.Get("dialogo.pedido_recusado") + " (" + res.Erro + ")");
            return res.Ok;
        }

        void Avisar(string texto)
        {
            Aviso = texto;
            avisoAte = Time.unscaledTime + SegundosDeAviso;
        }

        // ---- desenho (so le o estado pronto) ----

        void OnGUI()
        {
            if (Aviso != null && Time.unscaledTime > avisoAte) Aviso = null;
            if (!Aberta && Aviso == null) return;
            GUI.depth = -10;   // por cima da HUD de toque do PlayerInputReader (menor = na frente, e recebe o toque antes)
            Estilos();

            float w = Screen.width, h = Screen.height, fonte = estiloFala.fontSize;
            if (Aviso != null) GUI.Label(new Rect(w * 0.2f, h * 0.01f, w * 0.6f, fonte * 1.8f), Aviso, estiloAviso);
            if (!Aberta) return;

            // Coluna central (20%-80%): longe do joystick (esquerda) e do cluster de botoes (direita) em paisagem.
            float x = w * 0.2f, largura = w * 0.6f, pad = fonte * 0.5f;
            float y = h * 0.08f;
            float nomeH = fonte * 1.6f, falaH = AlturaDaFala(largura, fonte);
            // Alvo de toque >= 48 dp (ControlPreset.MinTargetDp); no PC (dpi ~96) vale o piso pela fonte.
            float botaoH = Mathf.Max(ControlPreset.DpToPx(ControlPreset.MinTargetDp, Screen.dpi), fonte * 2f);
            float gap = botaoH * 0.15f;
            int n = Rotulos.Length;
            int cols = n > 3 ? 2 : 1;   // ponytail: mais de 3 opcoes vira grade de 2 colunas; rolagem so se passar de ~8
            int linhas = (n + cols - 1) / cols;

            UiFundo.MarcarModal();
            UiFundo.Pintar(new Rect(x - pad, y - pad, largura + 2f * pad, nomeH + falaH + linhas * (botaoH + gap) + 2f * pad), UiFundo.Painel);
            GUI.Label(new Rect(x, y, largura, nomeH), nome, estiloNome);
            GUI.Label(new Rect(x, y + nomeH, largura, falaH), Fala, estiloFala);

            float y0 = y + nomeH + falaH, bw = (largura - gap * (cols - 1)) / cols;
            for (int i = 0; i < n; i++)
            {
                Rect r = new Rect(x + (i % cols) * (bw + gap), y0 + (i / cols) * (botaoH + gap), bw, botaoH);
                if (GUI.Button(r, Rotulos[i], estiloBotao)) { Escolher(i); return; }   // Escolher troca Rotulos
            }
        }

        // A caixa cresce com a fala: 4,2 linhas fixas cortavam a fala do Tovin aos 8 (visto na simulacao -roteiro).
        // Medida so quando a fala ou a largura mudam (o OnGUI roda 2x+ por quadro).
        string falaMedida;
        float larguraMedida, alturaMedida;

        float AlturaDaFala(float largura, float fonte)
        {
            if (Fala != falaMedida || largura != larguraMedida)
            {
                falaMedida = Fala;
                larguraMedida = largura;
                alturaMedida = estiloFala.CalcHeight(new GUIContent(Fala), largura) + fonte * 0.4f;
            }
            return Mathf.Max(alturaMedida, fonte * 2f);
        }

        void Estilos()
        {
            int fonte = Mathf.Max(18, Screen.height / 30);   // proporcional a tela, como PlayerInteractor e DamagePopup
            if (estiloFala != null && estiloFala.fontSize == fonte) return;
            estiloFala = new GUIStyle(GUI.skin.label) { fontSize = fonte, wordWrap = true, alignment = TextAnchor.UpperLeft };
            estiloNome = new GUIStyle(estiloFala) { fontStyle = FontStyle.Bold, wordWrap = false };
            estiloBotao = new GUIStyle(GUI.skin.button) { fontSize = fonte, wordWrap = true };
            estiloAviso = new GUIStyle(estiloNome) { alignment = TextAnchor.MiddleCenter, normal = { textColor = Color.yellow } };
        }
    }
}
