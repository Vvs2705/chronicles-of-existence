using System.Globalization;
using UnityEngine;

namespace COE
{
    /// <summary>Menu de pausa com as configuracoes do jogador (regra e persistencia em Configuracoes, fora do save).
    ///
    /// APLICA ao iniciar e a cada mudanca: mao -> preset de toque do PlayerInputReader (numa COPIA de runtime: o asset
    /// e definicao e nunca muda; quem espelha e o ControlPreset/TouchControls), sensibilidade -> LookMultiplier do
    /// leitor (toque, gamepad e mouse), FPS -> Application.targetFrameRate, desempenho -> PerfHud.Mostrar (CSV segue),
    /// qualidade -> Qualidade.Aplicar (ADR-0009; `-qualidade baixa|media|alta` na linha de comando forca a faixa so
    /// nesta sessao, para medir cada uma no PC/aparelho sem gravar nada).
    ///
    /// TELA: engrenagem no TOPO, na faixa livre entre o botao do salto (35%-65%) e a coluna do HUD de missao (80%+);
    /// o PerfHud fica no alto a esquerda, joystick e os 6 botoes embaixo. So aparece com o jogador no controle (input e
    /// "travar" ligados): some na tela de nascimento, na conversa e no aviso do salto. Aberto: Time.timeScale 0,
    /// "travar" desligados (motor, combate, interacao, camera) e o toque fora dos controles nao chega nas HUDs de tras.
    /// Fechar devolve a escala de tempo anterior e religa so o que ESTE menu desligou.
    /// ponytail: prototipo IMGUI (padrao de SaltoHud/DialogueHud); UI de verdade (Canvas, gamepad, voltar do Android,
    /// pausar sozinho quando o app vai para segundo plano) e a T013.</summary>
    public class MenuDePausa : MonoBehaviour
    {
        [SerializeField] PlayerInputReader input;
        [Tooltip("Opcional: o HUD de desempenho que a opcao liga/desliga (o CSV grava sempre).")]
        [SerializeField] PerfHud desempenho;
        [Tooltip("Desligados com o menu aberto: motor, combate, interacao e camera. Ligados pelo gerador.")]
        [SerializeField] Behaviour[] travar = new Behaviour[0];
        [Tooltip("Volume global de pos-processamento da cena (Auren): desligado na faixa Baixa. Nulo = cena sem pos.")]
        [SerializeField] Behaviour posProcessamento;

        /// <summary>null ate Iniciar (Start).</summary>
        public Configuracoes Config { get; private set; }
        public bool Aberto { get; private set; }

        ControlPreset preset;   // copia de runtime do preset da cena
        bool[] desligados = new bool[0];
        float escalaAntes = 1f;
        string abrir, titulo, mao, destra, canhota, sensibilidade, fps, desempenhoTexto, ligado, desligado, voltar, valorSensibilidade;
        string qualidadeTexto, qualidadeAuto;
        string[] faixas;
        FaixaQualidade? forcada, aplicada;
        GUIStyle estiloTexto, estiloValor, estiloTitulo, estiloBotao, estiloOpcao;

        void Start()
        {
            useGUILayout = false;   // so GUI.* posicionado: pula o passe de Layout do OnGUI
            if (Config == null) Iniciar(new ConfigPlayerPrefs());
        }

        /// <summary>Carrega do armazenamento e aplica. O Start usa PlayerPrefs; teste chama antes com ConfigEmMemoria.</summary>
        public void Iniciar(IConfigArmazenamento armazenamento)
        {
            StringsLoader.EnsureLoaded();   // textos montados aqui: o OnGUI roda 2x+ por quadro
            abrir = Strings.Get("config.abrir");
            titulo = Strings.Get("config.titulo");
            mao = Strings.Get("config.mao");
            destra = Strings.Get("config.mao.destra");
            canhota = Strings.Get("config.mao.canhota");
            sensibilidade = Strings.Get("config.sensibilidade");
            fps = Strings.Get("config.fps");
            desempenhoTexto = Strings.Get("config.desempenho");
            ligado = Strings.Get("config.ligado");
            desligado = Strings.Get("config.desligado");
            voltar = Strings.Get("config.voltar");
            qualidadeTexto = Strings.Get("config.qualidade");
            faixas = new[] { Strings.Get("config.qualidade.baixa"), Strings.Get("config.qualidade.media"), Strings.Get("config.qualidade.alta") };
            FaixaQualidade f;
            forcada = Qualidade.TryParse(DevSceneArg.Valor("-qualidade"), out f) ? f : (FaixaQualidade?)null;
            Config = new Configuracoes(armazenamento, Debug.isDebugBuild);
            Aplicar();
        }

        void Aplicar()
        {
            if (input != null)
            {
                if (preset == null && input.Preset != null) { preset = Instantiate(input.Preset); input.Preset = preset; }
                if (preset != null) preset.hand = Config.Mao;
                input.LookMultiplier = Config.Sensibilidade;
            }
            Application.targetFrameRate = Config.Fps;
            if (desempenho != null) desempenho.Mostrar = Config.MostrarDesempenho;
            valorSensibilidade = Config.Sensibilidade.ToString("0.0", CultureInfo.InvariantCulture) + "x";

            FaixaQualidade faixa = forcada ?? Config.QualidadeEfetiva(SystemInfo.systemMemorySize);
            if (aplicada != faixa) { Qualidade.Aplicar(faixa, posProcessamento); aplicada = faixa; }   // troca de nivel so quando muda
            qualidadeAuto = Strings.Format("config.qualidade.auto", faixas[(int)Qualidade.Detectar(SystemInfo.systemMemorySize)]);
        }

        /// <summary>A faixa que esta valendo (forcada pela linha de comando, escolhida ou detectada).</summary>
        public FaixaQualidade QualidadeAplicada { get { return aplicada ?? FaixaQualidade.Media; } }

        public void Abrir()
        {
            if (Aberto) return;
            Aberto = true;
            escalaAntes = Time.timeScale;
            Time.timeScale = 0f;
            if (desligados.Length != travar.Length) desligados = new bool[travar.Length];
            for (int i = 0; i < travar.Length; i++)
            {
                Behaviour b = travar[i];
                desligados[i] = b != null && b.enabled;   // so religa o que ESTE menu desligou
                if (desligados[i]) b.enabled = false;
            }
        }

        /// <summary>"Voltar ao jogo". Idempotente.</summary>
        public void Fechar()
        {
            if (!Aberto) return;
            Aberto = false;
            Time.timeScale = escalaAntes;
            for (int i = 0; i < travar.Length && i < desligados.Length; i++)
                if (desligados[i] && travar[i] != null) travar[i].enabled = true;
        }

        // Troca de cena (ou objeto destruido) com o menu aberto nao pode deixar o jogo congelado.
        void OnDisable() { Fechar(); }

        void OnDestroy() { if (preset != null) Destroy(preset); }

        bool NoControle()
        {
            if (input != null && !input.enabled) return false;
            foreach (Behaviour b in travar) if (b != null && !b.enabled) return false;
            return true;
        }

        // ---- tela ----
        // Alvos >= 48 dp (ControlPreset.MinTargetDp) com piso de 1/10 da altura, fonte proporcional (como SaltoHud).
        // Painel no centro (25%-75%): fora do joystick (esquerda) e dos botoes (direita).

        void OnGUI()
        {
            if (Config == null || (!Aberto && !NoControle())) return;
            GUI.depth = -20;   // na frente do DialogueHud (-10) e da HUD de toque; atras do EntryFlow (-100)
            Estilos();
            float w = Screen.width, h = Screen.height;
            float alvo = Mathf.Max(ControlPreset.DpToPx(ControlPreset.MinTargetDp, Screen.dpi), h / 10f);
            float m = alvo * 0.25f, gap = alvo * 0.15f;

            if (!Aberto)
            {
                if (GUI.Button(new Rect(w * 0.8f - m - alvo, h - Screen.safeArea.yMax + m, alvo, alvo), abrir, estiloBotao)) Abrir();
                return;
            }

            float tituloH = estiloTitulo.fontSize * 1.6f;
            float painelH = 2f * m + tituloH + 6f * (alvo + gap);
            Rect painel = new Rect(w * 0.25f, (h - painelH) * 0.5f, w * 0.5f, painelH);
            UiFundo.Modal(painel);   // opaco: a HUD de toque nao aparece atraves do painel

            float x = painel.x + m, largura = painel.width - 2f * m, y = painel.y + m;
            float rotuloW = largura * 0.45f, cx = x + rotuloW + gap, cw = largura - rotuloW - gap;
            GUI.Label(new Rect(x, y, largura, tituloH), titulo, estiloTitulo);
            y += tituloH + gap;

            int i = Escolha(mao, Config.Mao == HandPreset.Canhoto ? 1 : 0, destra, canhota);
            if (i >= 0) { Config.DefinirMao(i == 1 ? HandPreset.Canhoto : HandPreset.Destro); Aplicar(); }

            GUI.Label(new Rect(x, y, rotuloW, alvo), sensibilidade, estiloTexto);
            float bw = alvo * 1.2f;
            if (GUI.Button(new Rect(cx, y, bw, alvo), "-", estiloBotao)) { Config.MudarSensibilidade(-1); Aplicar(); }
            GUI.Label(new Rect(cx + bw, y, cw - 2f * bw, alvo), valorSensibilidade, estiloValor);
            if (GUI.Button(new Rect(cx + cw - bw, y, bw, alvo), "+", estiloBotao)) { Config.MudarSensibilidade(1); Aplicar(); }
            y += alvo + gap;

            i = Escolha(fps, Config.Fps == Configuracoes.FpsAlto ? 1 : 0, "30", "60");
            if (i >= 0) { Config.DefinirFps(i == 1 ? Configuracoes.FpsAlto : Configuracoes.FpsPadrao); Aplicar(); }

            i = Escolha(desempenhoTexto, Config.MostrarDesempenho ? 0 : 1, ligado, desligado);
            if (i >= 0) { Config.DefinirMostrarDesempenho(i == 0); Aplicar(); }

            // Qualidade: Auto (mostra a faixa detectada) | Baixa | Media | Alta
            GUI.Label(new Rect(x, y, rotuloW, alvo), qualidadeTexto, estiloTexto);
            int atualQ = Config.QualidadeEscolhida.HasValue ? 1 + (int)Config.QualidadeEscolhida.Value : 0;
            string[] opcoesQ = { qualidadeAuto, faixas[0], faixas[1], faixas[2] };
            float qw = (cw - 3f * gap) / 4f;
            for (int q = 0; q < 4; q++)
                if (GUI.Toggle(new Rect(cx + q * (qw + gap), y, qw, alvo), atualQ == q, opcoesQ[q], estiloOpcao) && atualQ != q)
                { Config.DefinirQualidade(q == 0 ? (FaixaQualidade?)null : (FaixaQualidade)(q - 1)); Aplicar(); }
            y += alvo + gap;

            if (GUI.Button(new Rect(x, y, largura, alvo), voltar, estiloBotao)) Fechar();

            Event e = Event.current;   // modal: o toque que sobrou (fora dos controles) nao chega nas HUDs de tras
            if (e.type != EventType.Repaint && e.type != EventType.Layout) e.Use();

            // Linha "rotulo | opcao 0 | opcao 1": a atual aparece pressionada. Devolve a outra se tocada, senao -1.
            int Escolha(string rotulo, int atual, string a, string b)
            {
                GUI.Label(new Rect(x, y, rotuloW, alvo), rotulo, estiloTexto);
                float ow = (cw - gap) * 0.5f;
                bool tocouA = GUI.Toggle(new Rect(cx, y, ow, alvo), atual == 0, a, estiloOpcao) && atual != 0;
                bool tocouB = GUI.Toggle(new Rect(cx + ow + gap, y, ow, alvo), atual == 1, b, estiloOpcao) && atual != 1;
                y += alvo + gap;
                return tocouA ? 0 : tocouB ? 1 : -1;
            }
        }

        void Estilos()
        {
            int fonte = Mathf.RoundToInt(Mathf.Max(ControlPreset.DpToPx(14f, Screen.dpi), Screen.height / 40f));
            if (estiloTexto != null && estiloTexto.fontSize == fonte) return;   // refaz so se a tela mudou
            estiloTexto = UiEstilo.EstiloTexto(fonte, TextAnchor.MiddleLeft);
            estiloValor = new GUIStyle(estiloTexto) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold };
            estiloTitulo = new GUIStyle(estiloValor) { fontSize = Mathf.RoundToInt(fonte * 1.3f) };
            estiloBotao = UiEstilo.EstiloBotao(fonte, true);
            estiloOpcao = UiEstilo.EstiloOpcao(fonte);   // opcao escolhida: ouro cheio
        }
    }
}
