using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    /// Desenho em uGUI (Bloco D, 2026-10-04), so toque/mouse (conversa por gamepad fora do slice, ADR-0007 §8). Estado so
    /// muda em Abrir/Escolher/Fechar; o desenho so le strings prontas.</summary>
    public class DialogueHud : MonoBehaviour
    {
        [Tooltip("Desligados enquanto a conversa esta aberta: motor, combate e interacao do Player (ligados pelo gerador).")]
        [SerializeField] Behaviour[] travarNaConversa = new Behaviour[0];
        [Tooltip("Opcional: zera a velocidade do Animator ao abrir (motor desligado congelaria o passo do ultimo quadro).")]
        [SerializeField] CharacterAnimator anim;
        [SerializeField] SomDoJogo som;   // clique ao abrir e a cada escolha; vazio = mudo
        [Tooltip("Enquadra o NPC por cima do ombro enquanto a conversa esta aberta. Vazio = camera livre.")]
        [SerializeField] ThirdPersonCamera cam;
        [Tooltip("q05: o chapeu da Lysa (ligado pelo PecasDeEventoSetup). So com o bicho calmo a conversa oferece tratar_o_animal. Vazio = nunca.")]
        [SerializeField] BichoNoChapeu bicho;

        /// <summary>Letras por segundo da fala que se escreve (toque no painel completa na hora).</summary>
        public const float LetrasPorSegundo = 45f;
        float inicioFala;
        bool falaCompleta;
        string[] rotulosTela = new string[0];   // seta + rotulo, montado em IrPara (o desenho nao concatena)

        /// <summary>A fala ainda esta aparecendo letra a letra (os botoes esperam).</summary>
        public bool Escrevendo { get { return Aberta && Fala != null && !falaCompleta && Visiveis() < Fala.Length; } }

        int Visiveis()
        {
            if (Fala == null) return 0;
            if (falaCompleta) return Fala.Length;
            return Mathf.Min(Fala.Length, (int)((Time.unscaledTime - inicioFala) * LetrasPorSegundo));
        }

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

        // Awake: antes do primeiro desenho do PlayerInteractor, que cacheia o nome do NPC (NpcActor.Prompt).
        void Awake() { StringsLoader.EnsureLoaded(); }

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
                ObjetivoProximo = (q, o) => MissaoNaConversa.EhOProximo(s.Missoes, q, o),
                Destino = s.Save.birth == null ? "" : s.Save.birth.destinyId ?? "",
                Origem = s.Save.birth == null ? "" : s.Save.birth.originId ?? "",
                QuantidadeDoItem = item => Inventario.Quantidade(s.Save.inventario, item),
            };
            grafo = DialogueCatalog.Do(npc.NpcId);
            Travar();
            if (som != null) som.Tocar(Som.Clique);
            // O NPC se vira para a crianca, e a camera os enquadra por cima do ombro.
            if (anim != null)
            {
                Vector3 d = anim.transform.position - npc.transform.position;
                d.y = 0f;
                if (d.sqrMagnitude > 0.01f) npc.transform.rotation = Quaternion.LookRotation(d);
            }
            if (cam != null) cam.Focar(npc.transform);
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
            if (som != null) som.Tocar(Som.Clique);

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
            if (cam != null) cam.Focar(null);
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
            deMissao = MissaoNaConversa.Opcoes(Npc.NpcId, SaveState.Sessao.Missoes, autorais, bicho != null && bicho.Calmo);

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
                     : i - autorais.Length < deMissao.Length ? FalaDaMissao(deMissao[i - autorais.Length].TextoKey)
                     : Strings.Get("dialogo.opcao.despedir");
            }
            Rotulos = r;
            rotulosTela = new string[n];
            for (int k = 0; k < n; k++) rotulosTela[k] = "\u25B8  " + r[k];
            inicioFala = Time.unscaledTime;
            falaCompleta = false;
        }

        static string FalaDaMissao(string chave) { return Strings.GetOu(MissaoNaConversa.ChaveDaFala(chave), chave); }

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

        // ---- desenho (uGUI, Bloco D; so le o estado pronto) ----
        // Montado sob demanda no Play (teste de editor usa Abrir/Escolher sem desenho). Painel embaixo (caixa de dialogo de
        // RPG): os rostos ficam a vista em cima; com a conversa aberta a HUD de toque some (UiFundo.HaModal).

        Canvas canvas;
        Image painel, etiqueta;
        Text textoNome, textoFala, textoAviso;
        Button completar;   // toque no painel enquanto a fala se escreve: crianca que ja leu nao espera a maquina
        readonly List<Button> botoes = new List<Button>();
        string falaDisposta, nomeDisposto, avisoMostrado;
        string[] rotulosDispostos;
        int visMostrado = -1, alturaDisposta;
        Rect safeDisposto;

        /// <summary>O painel da conversa (teste le).</summary>
        public Canvas Vista { get { return canvas; } }

        void LateUpdate()
        {
            if (Aviso != null && Time.unscaledTime > avisoAte) Aviso = null;
            if (!Aberta && Aviso == null)
            {
                if (canvas != null && canvas.enabled) canvas.enabled = false;
                return;
            }
            if (canvas == null) Montar();
            if (!canvas.enabled) canvas.enabled = true;
            if (Screen.height != alturaDisposta || Screen.safeArea != safeDisposto) { falaDisposta = null; avisoMostrado = null; }
            if (Aviso != avisoMostrado)
            {
                avisoMostrado = Aviso;
                textoAviso.gameObject.SetActive(Aviso != null);
                if (Aviso != null) { textoAviso.text = Aviso; DisporAviso(); }
            }
            painel.gameObject.SetActive(Aberta);
            etiqueta.gameObject.SetActive(Aberta);
            if (!Aberta) return;
            UiFundo.MarcarModal();
            if (Fala != falaDisposta || Rotulos != rotulosDispostos || nome != nomeDisposto) Dispor();

            int vis = Visiveis();
            if (vis != visMostrado)
            {
                visMostrado = vis;
                // O resto da fala vai transparente: a quebra de linha ja e a final e nenhuma palavra pula de linha.
                textoFala.text = vis >= Fala.Length ? Fala : Fala.Substring(0, vis) + "<color=#00000000>" + Fala.Substring(vis) + "</color>";
                bool escrevendo = vis < Fala.Length;
                completar.gameObject.SetActive(escrevendo);
                for (int i = 0; i < botoes.Count; i++) botoes[i].gameObject.SetActive(!escrevendo && i < Rotulos.Length);
            }
        }

        void Montar()
        {
            canvas = Tela.NovoCanvas(transform, "ConversaCanvas", Tela.CamadaConversa);
            painel = Tela.Imagem(canvas.transform, "Painel", Tela.SpritePainel, Color.white);
            textoFala = Tela.Texto(painel.transform, "Fala", 18, TextAnchor.UpperLeft, UiEstilo.Tinta);
            textoFala.supportRichText = true;   // so para a parte que ainda nao apareceu (as falas nao tem marcacao)
            completar = Tela.Botao(painel.transform, "CompletarFala", 18, delegate { falaCompleta = true; });
            completar.GetComponent<Image>().color = Color.clear;
            Tela.Rotulo(completar).text = "";
            Tela.Esticar(completar.GetComponent<RectTransform>(), 0f);
            etiqueta = Tela.Imagem(canvas.transform, "Nome", Fatiado(UiEstilo.Etiqueta), Color.white);
            textoNome = Tela.Texto(etiqueta.transform, "Texto", 18, TextAnchor.MiddleCenter, new Color(0.12f, 0.10f, 0.16f));
            textoNome.fontStyle = FontStyle.Bold;
            textoNome.horizontalOverflow = HorizontalWrapMode.Overflow;
            Tela.Esticar(textoNome.rectTransform, 0f);
            textoAviso = Tela.Texto(canvas.transform, "Aviso", 18, TextAnchor.MiddleCenter, UiEstilo.Ouro);
            textoAviso.fontStyle = FontStyle.Bold;
            textoAviso.resizeTextForBestFit = true;
        }

        static Sprite etiquetaSprite;
        static Sprite Fatiado(Texture2D t)
        {
            if (etiquetaSprite != null) return etiquetaSprite;
            float b = UiEstilo.Borda.left;
            return etiquetaSprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0.5f, 0.5f), 100f, 0,
                SpriteMeshType.FullRect, new Vector4(b, b, b, b));
        }

        int Fonte { get { return Mathf.Max(18, Screen.height / 30); } }   // proporcional a tela, como as outras HUDs

        void DisporAviso()
        {
            Rect s = Screen.safeArea;
            int fonte = Fonte;
            textoAviso.fontSize = fonte;
            textoAviso.resizeTextMaxSize = fonte;
            textoAviso.resizeTextMinSize = Mathf.Max(10, fonte / 2);
            Tela.Colocar(textoAviso.rectTransform, new Rect(s.x + s.width * 0.2f, s.yMax - Screen.height * 0.01f - fonte * 1.8f, s.width * 0.6f, fonte * 1.8f));
        }

        /// <summary>Painel centrado na area segura (66% da largura), crescendo com a fala; botoes em 1 coluna, ou 2 com
        /// mais de 3 opcoes. Alvo >= 48 dp. Refeito so quando a fala, as opcoes, o nome ou a tela mudam.</summary>
        void Dispor()
        {
            falaDisposta = Fala;
            rotulosDispostos = Rotulos;
            nomeDisposto = nome;
            alturaDisposta = Screen.height;
            safeDisposto = Screen.safeArea;
            visMostrado = -1;

            Rect s = safeDisposto;
            int fonte = Fonte;
            float largura = s.width * 0.66f, x = s.x + (s.width - largura) * 0.5f, pad = fonte * 0.9f, interno = largura - 2f * pad;
            textoFala.fontSize = fonte;
            // A caixa cresce com a fala: 4,2 linhas fixas cortavam a fala do Tovin aos 8 (visto na simulacao -roteiro).
            TextGenerationSettings medida = textoFala.GetGenerationSettings(new Vector2(interno, 0f));
            float falaH = Mathf.Max(textoFala.cachedTextGeneratorForLayout.GetPreferredHeight(Fala, medida) / textoFala.pixelsPerUnit
                                    + fonte * 0.4f, fonte * 2f);
            float botaoH = Mathf.Max(ControlPreset.DpToPx(ControlPreset.MinTargetDp, Tela.Dpi), fonte * 2.1f);
            float gap = fonte * 0.45f;
            int n = Rotulos.Length;
            int cols = n > 3 ? 2 : 1;   // ponytail: mais de 3 opcoes vira grade de 2 colunas; rolagem so se passar de ~8
            int linhas = (n + cols - 1) / cols;
            float alturaPainel = pad + falaH + gap + linhas * (botaoH + gap) + pad * 0.5f;   // botoes reservados: nada pula
            float y = s.y + Screen.height * 0.03f;
            Tela.Colocar(painel.rectTransform, new Rect(x, y, largura, alturaPainel));
            float topo = alturaPainel;   // daqui em diante relativo ao painel (origem embaixo)
            Tela.Colocar(textoFala.rectTransform, new Rect(pad, topo - pad - falaH, interno, falaH));

            float nomeH = fonte * 1.7f;
            textoNome.fontSize = fonte;
            textoNome.text = nome;
            float nomeW = textoNome.preferredWidth + fonte * 2f;
            Tela.Colocar(etiqueta.rectTransform, new Rect(x + pad, y + alturaPainel - nomeH * 0.4f, nomeW, nomeH));

            while (botoes.Count < n)
            {
                int indice = botoes.Count;
                Button b = Tela.Botao(painel.transform, "Opcao" + indice, fonte, delegate { Escolher(indice); });
                Tela.Rotulo(b).alignment = TextAnchor.MiddleLeft;
                Tela.Rotulo(b).fontStyle = FontStyle.Normal;
                Tela.Rotulo(b).resizeTextForBestFit = true;   // opcao longa encolhe em vez de perder a linha que nao cabe
                botoes.Add(b);
            }
            float bw = (interno - gap * (cols - 1)) / cols, y0 = topo - pad - falaH - gap;
            for (int i = 0; i < botoes.Count; i++)
            {
                if (i >= n) { botoes[i].gameObject.SetActive(false); continue; }
                Tela.Colocar(botoes[i].GetComponent<RectTransform>(),
                    new Rect(pad + (i % cols) * (bw + gap), y0 - (i / cols) * (botaoH + gap) - botaoH, bw, botaoH));
                Text r = Tela.Rotulo(botoes[i]);
                r.fontSize = fonte;
                r.resizeTextMaxSize = fonte;
                r.resizeTextMinSize = Mathf.Max(10, fonte / 2);
                r.text = rotulosTela[i];
            }
            completar.transform.SetAsLastSibling();   // por cima das opcoes (que esperam a fala terminar)
        }
    }
}
