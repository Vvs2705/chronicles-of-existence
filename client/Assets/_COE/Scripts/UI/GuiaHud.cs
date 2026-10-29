using UnityEngine;
using UnityEngine.UI;

namespace COE
{
    /// <summary>Tutorial guiado do episodio 1 na tela (regra em GuiaRegras; padrao do Tour.cs do Dungeon Courier, pedido do
    /// idealizador: congela a tela, so a area sinalizada fica acesa e o cartao diz qual botao apertar, por que e para que).
    ///  - explicacao: Time.timeScale 0, o leitor suspenso (PlayerInputReader.Suspenso), veu escuro em volta do alvo com
    ///    moldura dourada pulsando, cartao com O que / Quando / Por que, PULAR TUTORIAL e ENTENDI;
    ///  - acao: o jogo segue, o alvo fica aceso e o passo termina quando o jogador faz (andar, girar a camera, usar);
    ///  - contexto: cartao de historia no meio (no lugar de cutscene), com ENTENDI.
    /// Cada passo aparece uma vez (Configuracoes: coe.cfg.v1.guia, fora do save); "Rever tutorial" no menu zera.
    /// So com o jogador no controle e sem modal: conversa, menu, salto e gancho tem prioridade, e um passo de acao cujo alvo
    /// sumiu (o prompt de USAR, por exemplo) cancela sem contar como visto. Desligado no -roteiro (robo de teste), salvo
    /// com -tutorial. Nao marca UiFundo.HaModal: as HUDs de tras (toque, missao, seta) precisam aparecer no buraco do veu.
    /// Dependencias por campo, ligadas pelo GuiaSetup (Editor). ponytail: o veu sao quatro retangulos (sem mascara/shader);
    /// buraco redondo e seta animada se a arte de interface pedir.</summary>
    public class GuiaHud : MonoBehaviour
    {
        [SerializeField] PlayerInputReader leitor;
        [Tooltip("Menu de pausa: as Configuracoes dele guardam os passos vistos (e o Rever tutorial zera la).")]
        [SerializeField] MenuDePausa menu;
        [Tooltip("Ligados = jogador no controle (motor, combate, interacao, camera; a mesma lista que o menu trava).")]
        [SerializeField] Behaviour[] controle = new Behaviour[0];
        [SerializeField] PlayerInteractor interacao;
        [SerializeField] PlayerCombat combate;
        [Tooltip("So em Auren (MissaoSceneSetup liga): o cartao de missao e a seta de objetivo que o guia acende.")]
        [SerializeField] MissaoHud missao;
        [SerializeField] IndicadorDeObjetivo indicador;
        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo gerador (PartidaSetup); vazio = a do SaveState.")]
        [SerializeField] Partida partida;

        /// <summary>Segundos de controle antes do primeiro passo da cena e entre dois passos (tempo real).</summary>
        public const float Atraso = 1.0f, EntrePassos = 0.5f;
        /// <summary>Quanto andar (s com o joystick fora da zona morta) e quanto girar (graus somados) fecham os passos de acao.</summary>
        public const float AndarParaFeito = 0.6f, GirarParaFeito = 45f;

        GuiaPasso passo;
        float espera, andou, girou;
        bool congelado;
        float escalaAntes = 1f;
        bool? ligado;

        /// <summary>O passo na tela (null = nenhum). Teste e robo leem.</summary>
        public GuiaPasso Passo { get { return passo; } }

        bool Ligado
        {
            get
            {
                if (!ligado.HasValue) ligado = !Roteiro.Ligado || DevSceneArg.Tem("-tutorial");
                return ligado.Value && menu != null && menu.Config != null;
            }
        }

        bool Visto(string id) { return menu.Config.GuiaVisto(id); }

        void Update()
        {
            if (!Ligado) { if (passo != null) Fechar(); return; }
            GuiaEstado e = Ler();
            if (passo != null)
            {
                if (UiFundo.HaModal) { Fechar(); return; }   // modal por cima: volta quando a condicao valer de novo
                if (passo.Acao)
                {
                    if (Feito()) { Concluir("feito"); return; }
                    if (!GuiaRegras.Vale(passo, e, Visto)) Fechar();   // o alvo sumiu (prompt de USAR): nao conta como visto
                }
                return;
            }
            if (!e.NoControle) { espera = Mathf.Min(espera, 0f); return; }
            espera += Time.unscaledDeltaTime;
            if (espera < Atraso) return;
            GuiaPasso p = GuiaRegras.Proximo(e, Visto);
            if (p != null) Mostrar(p);
        }

        GuiaEstado Ler()
        {
            GameSession s = Partida.De(partida);
            var e = new GuiaEstado
            {
                NoControle = NoControle(),
                Toque = leitor != null && leitor.MostraToque,
                Idade = s != null ? s.Save.ageYears : 5,
                CartaoMissao = missao != null && missao.Visivel,
                Seta = indicador != null && indicador.Seta != null,
                AlvoDeUso = interacao != null && interacao.Alvo != null,
                Golpes = combate != null && combate.PodeTreinar,
            };
            if (s != null)
            {
                e.TemItem = Inventario.Visiveis(s.Save.inventario).Count > 0;
                e.Promessa = s.Missoes.Estado(QuestCatalog.MissaoDaPromessa) == QuestStatus.EmAndamento;
                e.Concluidas = s.Missoes.Concluidas();
            }
            return e;
        }

        bool NoControle()
        {
            if (leitor == null || !leitor.isActiveAndEnabled || UiFundo.HaModal) return false;
            foreach (Behaviour b in controle) if (b != null && !b.enabled) return false;
            return true;
        }

        bool Feito()
        {
            if (leitor == null) return false;
            switch (passo.Id)
            {
                case GuiaRegras.Mover:
                    if (leitor.Move.sqrMagnitude > 0.09f) andou += Time.unscaledDeltaTime;
                    return andou >= AndarParaFeito;
                case GuiaRegras.Olhar:
                    girou += Mathf.Abs(leitor.Look.x) + Mathf.Abs(leitor.Look.y);
                    return girou >= GirarParaFeito;
                case GuiaRegras.Usar:
                    return leitor.InteractPressed;
            }
            return false;
        }

        // ---------- mostrar e fechar ----------

        void Mostrar(GuiaPasso p)
        {
            passo = p;
            andou = girou = 0f;
            if (canvas == null) Montar();
            canvas.enabled = true;
            titulo.text = Strings.Get(p.TituloKey);
            corpo.text = Corpo(p);
            bool explicacao = !p.Acao;
            botaoOk.gameObject.SetActive(explicacao);
            agora.gameObject.SetActive(!explicacao);
            tampa.gameObject.SetActive(explicacao);   // explicacao: o alvo aparece, mas o toque nele nao passa
            foreach (Image v in veu) { v.raycastTarget = explicacao; v.color = explicacao ? CorVeu : CorVeuAcao; }
            if (explicacao) Congelar();
            Debug.Log("Guia: " + p.Id);
            Desenhar();
        }

        string Corpo(GuiaPasso p)
        {
            if (p.Contexto)
            {
                if (p.Id != GuiaRegras.Despertar) return Strings.Get(p.TextoKey);
                BirthChoice b = Partida.De(partida) != null ? Partida.De(partida).Save.birth : null;
                OriginDef o = b != null ? DestinyCatalog.Origem(b.originId) : null;
                return Strings.Format(p.TextoKey, b != null ? b.characterName : EntryFlow.NomePadrao, o != null ? Strings.Get(o.OficioKey) : "");
            }
            return Linha("guia.rotulo.oque", p.OQueKey) + "\n" + Linha("guia.rotulo.quando", p.QuandoKey) + "\n"
                + Linha("guia.rotulo.porque", p.PorQueKey);
        }

        static string Linha(string rotulo, string texto)
        {
            return "<b><color=#FFD15C>" + Strings.Get(rotulo) + "</color></b>  " + Strings.Get(texto);
        }

        void Congelar()
        {
            if (congelado) return;
            congelado = true;
            escalaAntes = Time.timeScale;
            Time.timeScale = 0f;
            if (leitor != null) leitor.Suspenso = true;
        }

        void Concluir(string como)
        {
            Debug.Log("Guia: " + passo.Id + " " + como);
            menu.Config.MarcarGuia(passo.Id);
            Fechar();
            espera = Atraso - EntrePassos;   // o proximo espera um pouco: um cartao atras do outro cansa
        }

        void Entendi() { if (passo != null) Concluir("ok"); }

        void Pular()
        {
            if (passo != null) Debug.Log("Guia: " + passo.Id + " pular");
            menu.Config.PularGuia();
            Fechar();
        }

        /// <summary>Fecha sem marcar (modal por cima, alvo sumiu, componente desligado). Devolve o tempo e o controle.</summary>
        void Fechar()
        {
            passo = null;
            espera = 0f;
            if (congelado)
            {
                congelado = false;
                if (Time.timeScale == 0f) Time.timeScale = escalaAntes;   // so devolve o que este guia parou
                if (leitor != null) leitor.Suspenso = false;
            }
            if (canvas != null) canvas.enabled = false;
        }

        void OnDisable() { Fechar(); }

        // ---------- desenho (uGUI, Bloco D) ----------
        // Veu de quatro retangulos em volta do buraco, moldura dourada pulsando no buraco, cartao no lado oposto ao alvo.
        // O buraco e recalculado por quadro (a seta de objetivo anda); textos so mudam em Mostrar.

        static readonly Color CorVeu = new Color(0f, 0f, 0f, 0.72f), CorVeuAcao = new Color(0f, 0f, 0f, 0.45f);

        Canvas canvas;
        readonly Image[] veu = new Image[4];
        Image tampa, moldura, cartao;
        Text titulo, corpo, agora;
        Button botaoOk, botaoPular;

        /// <summary>A tela do guia (teste le).</summary>
        public Canvas Vista { get { return canvas; } }

        void Montar()
        {
            canvas = Tela.NovoCanvas(transform, "GuiaCanvas", Tela.CamadaGuia);
            for (int i = 0; i < veu.Length; i++) veu[i] = Tela.Imagem(canvas.transform, "Veu" + i, null, CorVeu);
            tampa = Tela.Imagem(canvas.transform, "Tampa", null, Color.clear);
            tampa.raycastTarget = true;
            moldura = Tela.Imagem(canvas.transform, "Moldura", Tela.SpriteContorno, Color.white);
            cartao = Tela.Imagem(canvas.transform, "Cartao", Tela.SpritePainel, Color.white);
            cartao.raycastTarget = true;   // tocar no cartao nao vaza para o veu
            titulo = Tela.Titulo(cartao.transform, "Titulo", 20, TextAnchor.MiddleLeft, UiEstilo.Ouro);
            titulo.fontStyle = FontStyle.Bold;
            corpo = Tela.Texto(cartao.transform, "Corpo", 16, TextAnchor.UpperLeft, UiEstilo.Tinta);
            corpo.supportRichText = true;
            corpo.resizeTextForBestFit = true;   // ES/ID mais longos encolhem ate o minimo de leitura, nao cortam
            agora = Tela.Texto(cartao.transform, "Agora", 16, TextAnchor.MiddleRight, UiEstilo.Ouro);
            agora.fontStyle = FontStyle.BoldAndItalic;
            agora.text = Strings.Get("guia.agora");
            botaoPular = Tela.Botao(cartao.transform, "Pular", 16, Pular);
            Tela.Rotulo(botaoPular).text = Strings.Get("guia.pular");
            botaoOk = Tela.Botao(cartao.transform, "Entendi", 16, Entendi);
            Tela.Rotulo(botaoOk).text = Strings.Get("guia.entendi");
            canvas.enabled = false;
        }

        void LateUpdate()
        {
            if (passo != null && canvas != null && canvas.enabled) Desenhar();
        }

        void Desenhar()
        {
            Rect safe = Screen.safeArea;
            float alvo = Tela.Alvo, m = alvo * 0.25f, gap = alvo * 0.15f;
            Rect? buraco = Alvo(passo.Alvo, safe);
            Rect tela = new Rect(0f, 0f, Screen.width, Screen.height);
            Rect b = buraco.HasValue ? Expandir(buraco.Value, alvo * 0.15f) : new Rect(tela.center, Vector2.zero);
            b = Recortar(b, tela);
            // Veu: baixo, cima, esquerda e direita do buraco (sem buraco, o primeiro cobre tudo).
            Tela.Colocar(veu[0].rectTransform, new Rect(0f, 0f, tela.width, b.yMin));
            Tela.Colocar(veu[1].rectTransform, new Rect(0f, b.yMax, tela.width, tela.height - b.yMax));
            Tela.Colocar(veu[2].rectTransform, new Rect(0f, b.yMin, b.xMin, b.height));
            Tela.Colocar(veu[3].rectTransform, new Rect(b.xMax, b.yMin, tela.width - b.xMax, b.height));
            if (!buraco.HasValue) Tela.Colocar(veu[0].rectTransform, tela);
            Tela.Colocar(tampa.rectTransform, b);
            moldura.gameObject.SetActive(buraco.HasValue);
            if (buraco.HasValue)
            {
                float pulso = 1f + 0.06f * Mathf.Sin(Time.unscaledTime * 6f);   // a moldura respira: o olho acha o alvo
                Rect r = Expandir(b, alvo * 0.08f * pulso);
                Tela.Colocar(moldura.rectTransform, r);
            }

            int fonte = Tela.Fonte(14f, 1f / 36f);
            bool contexto = passo.Contexto || !buraco.HasValue;
            float w = contexto ? safe.width * 0.6f : safe.width * 0.44f, h = contexto ? safe.height * 0.55f : safe.height * 0.5f;
            float x = safe.center.x - w * 0.5f, y = safe.center.y - h * 0.5f;
            if (!contexto)
            {
                // Lado oposto ao alvo; na vertical, centrado no alvo e preso na area segura.
                x = b.center.x > safe.center.x ? safe.x + m : safe.xMax - m - w;
                y = Mathf.Clamp(b.center.y - h * 0.5f, safe.y + m, safe.yMax - m - h);
            }
            Tela.Colocar(cartao.rectTransform, new Rect(x, y, w, h));
            float pad = m, tituloH = fonte * 1.6f;
            titulo.fontSize = Mathf.RoundToInt(fonte * 1.25f);
            Tela.Colocar(titulo.rectTransform, new Rect(pad, h - pad - tituloH, w - 2f * pad, tituloH));
            float bw = (w - 2f * pad - gap) * 0.5f;
            Tela.Colocar(botaoPular.GetComponent<RectTransform>(), new Rect(pad, pad, bw, alvo));
            Tela.Colocar(botaoOk.GetComponent<RectTransform>(), new Rect(pad + bw + gap, pad, bw, alvo));
            Tela.Colocar(agora.rectTransform, new Rect(pad + bw + gap, pad, bw, alvo));
            float corpoY = pad + alvo + gap;
            Tela.Colocar(corpo.rectTransform, new Rect(pad, corpoY, w - 2f * pad, Mathf.Max(0f, h - pad - tituloH - gap - corpoY)));
            corpo.fontSize = fonte;
            corpo.resizeTextMaxSize = fonte;
            corpo.resizeTextMinSize = Mathf.RoundToInt(HudLayout.Fonte(HudLayout.MinTextoDp, 0f, Screen.height, Tela.Dpi));
            agora.fontSize = fonte;
            foreach (Button bt in new[] { botaoPular, botaoOk })
            {
                Text r = Tela.Rotulo(bt);
                r.fontSize = fonte;
                r.resizeTextForBestFit = true;
                r.resizeTextMaxSize = fonte;
                r.resizeTextMinSize = Mathf.Max(10, fonte / 2);
            }
        }

        /// <summary>O retangulo do alvo em px de tela, ou null quando ele nao esta desenhado agora.</summary>
        Rect? Alvo(GuiaAlvo a, Rect safe)
        {
            float dpi = Tela.Dpi;
            ControlPreset p = leitor != null ? leitor.Preset : null;
            switch (a)
            {
                case GuiaAlvo.Joystick:
                    if (p == null) return null;
                    return Quadrado(p.JoystickRestPx(safe, dpi), ControlPreset.DpToPx(p.joystickRadiusDp, dpi));
                case GuiaAlvo.Olhar:
                    // O meio livre do lado dos botoes: fora da zona do joystick e acima dos botoes.
                    bool destro = p == null || p.hand == HandPreset.Destro;
                    float x0 = destro ? 0.52f : 0.22f;
                    return new Rect(safe.x + safe.width * x0, safe.y + safe.height * 0.45f, safe.width * 0.26f, safe.height * 0.4f);
                case GuiaAlvo.CartaoMissao: return missao != null && missao.Visivel ? NaTela(missao.Cartao) : null;
                case GuiaAlvo.Indicador: return indicador != null ? NaTela(indicador.Seta) : null;
                case GuiaAlvo.Usar: return Botao(p, TouchAction.Interagir, safe, dpi);
                case GuiaAlvo.Ataque: return Botao(p, TouchAction.Ataque, safe, dpi);
                case GuiaAlvo.Menu: return HudLayout.BotaoMenu(safe, Tela.Alvo);
                case GuiaAlvo.Itens: return HudLayout.BotaoItens(safe, Tela.Alvo);
            }
            return null;
        }

        static Rect? Botao(ControlPreset p, TouchAction acao, Rect safe, float dpi)
        {
            if (p == null) return null;
            for (int i = 0; i < p.buttons.Length; i++)
                if (p.buttons[i].action == acao) return Quadrado(p.ButtonCenterPx(i, safe, dpi), p.ButtonRadiusPx(i, dpi));
            return null;
        }

        static readonly Vector3[] cantos = new Vector3[4];

        /// <summary>Canvas overlay em pixel 1:1: o mundo do RectTransform ja e px de tela.</summary>
        static Rect? NaTela(RectTransform rt)
        {
            if (rt == null || !rt.gameObject.activeInHierarchy) return null;
            rt.GetWorldCorners(cantos);
            return Rect.MinMaxRect(cantos[0].x, cantos[0].y, cantos[2].x, cantos[2].y);
        }

        static Rect Quadrado(Vector2 c, float r) { return new Rect(c.x - r, c.y - r, 2f * r, 2f * r); }
        static Rect Expandir(Rect r, float d) { return new Rect(r.x - d, r.y - d, r.width + 2f * d, r.height + 2f * d); }

        static Rect Recortar(Rect r, Rect limite)
        {
            return Rect.MinMaxRect(Mathf.Clamp(r.xMin, limite.xMin, limite.xMax), Mathf.Clamp(r.yMin, limite.yMin, limite.yMax),
                                   Mathf.Clamp(r.xMax, limite.xMin, limite.xMax), Mathf.Clamp(r.yMax, limite.yMin, limite.yMax));
        }
    }
}
