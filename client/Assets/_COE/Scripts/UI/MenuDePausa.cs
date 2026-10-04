using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

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
    /// O voltar do Android / Esc fecha o menu (VoltarHud chama Fechar).
    /// Desenho em uGUI (Bloco D, 2026-10-04). ponytail: sem navegacao por gamepad nem pausa automatica em segundo plano.</summary>
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
        string abrir, titulo, mao, destra, canhota, sensibilidade, fps, desempenhoTexto, ligado, desligado, voltar, valorSensibilidade, somTexto;
        string qualidadeTexto, qualidadeAuto;
        string[] faixas;
        FaixaQualidade? forcada, aplicada;

        void Start()
        {
            if (Config == null) Iniciar(new ConfigPlayerPrefs());
        }

        /// <summary>Carrega do armazenamento e aplica. O Start usa PlayerPrefs; teste chama antes com ConfigEmMemoria.</summary>
        public void Iniciar(IConfigArmazenamento armazenamento)
        {
            StringsLoader.EnsureLoaded();   // textos montados uma vez
            abrir = Strings.Get("config.abrir");
            titulo = Strings.Get("config.titulo");
            mao = Strings.Get("config.mao");
            destra = Strings.Get("config.mao.destra");
            canhota = Strings.Get("config.mao.canhota");
            sensibilidade = Strings.Get("config.sensibilidade");
            fps = Strings.Get("config.fps");
            desempenhoTexto = Strings.Get("config.desempenho");
            somTexto = Strings.Get("config.som");
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
            AudioListener.volume = Config.Som ? 1f : 0f;   // musica e efeitos de uma vez
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

        // ---- tela (uGUI, Bloco D) ----
        // Alvos >= 48 dp com piso de 1/10 da altura, fonte proporcional (como SaltoHud). Painel no centro, em grade de 2
        // colunas (HudLayout.PainelDoMenu: cabe ate em 360 dp; 7 linhas de 48 dp nao cabiam). Montado uma vez; Atualizar()
        // so troca o que esta marcado e o valor da sensibilidade quando uma configuracao muda.

        Canvas canvas;
        Button botaoAbrir, botaoVoltar, botaoMenos, botaoMais;
        Image fundo, painel;
        Text textoTitulo, textoValor;
        Text[] rotulosLinha;
        Button[] maoB, somB, fpsB, desempenhoB, qualidadeB;
        int alturaDisposta;
        Rect safeDisposto;

        /// <summary>A tela do menu (teste le).</summary>
        public Canvas Vista { get { return canvas; } }

        void Update() { if (Aberto) UiFundo.MarcarModal(); }

        void LateUpdate()
        {
            bool mostrar = Config != null && (Aberto || NoControle());
            if (canvas == null)
            {
                if (!mostrar) return;
                Montar();
            }
            if (canvas.enabled != mostrar) canvas.enabled = mostrar;
            if (!mostrar) return;
            if (Screen.height != alturaDisposta || Screen.safeArea != safeDisposto) Dispor();
            if (botaoAbrir.gameObject.activeSelf == Aberto) botaoAbrir.gameObject.SetActive(!Aberto);
            if (fundo.gameObject.activeSelf != Aberto) { fundo.gameObject.SetActive(Aberto); if (Aberto) Atualizar(); }
        }

        void Montar()
        {
            canvas = Tela.NovoCanvas(transform, "MenuCanvas", Tela.CamadaModal);
            botaoAbrir = Tela.Botao(canvas.transform, "Abrir", 16, Abrir);
            Tela.Rotulo(botaoAbrir).text = abrir;
            fundo = Tela.FundoModal(canvas.transform);   // opaco: a HUD de toque nao aparece atraves do painel
            painel = Tela.Imagem(fundo.transform, "Painel", Tela.SpritePainel, Color.white);
            textoTitulo = Tela.Texto(painel.transform, "Titulo", 20, TextAnchor.MiddleCenter, UiEstilo.Tinta);
            textoTitulo.fontStyle = FontStyle.Bold;
            textoTitulo.text = titulo;
            string[] rotulos = { mao, somTexto, sensibilidade, fps, desempenhoTexto, qualidadeTexto };
            rotulosLinha = new Text[rotulos.Length];
            for (int i = 0; i < rotulos.Length; i++)
            {
                rotulosLinha[i] = Tela.Texto(painel.transform, "Rotulo" + i, 16, TextAnchor.MiddleLeft, UiEstilo.Tinta);
                rotulosLinha[i].text = rotulos[i];
            }
            maoB = Opcoes("Mao", delegate (int i) { Config.DefinirMao(i == 1 ? HandPreset.Canhoto : HandPreset.Destro); }, destra, canhota);
            somB = Opcoes("Som", delegate (int i) { Config.DefinirSom(i == 0); }, ligado, desligado);
            botaoMenos = Tela.Botao(painel.transform, "Menos", 16, delegate { Config.MudarSensibilidade(-1); Mudou(); });
            Tela.Rotulo(botaoMenos).text = "-";
            textoValor = Tela.Texto(painel.transform, "Valor", 16, TextAnchor.MiddleCenter, UiEstilo.Tinta);
            textoValor.fontStyle = FontStyle.Bold;
            botaoMais = Tela.Botao(painel.transform, "Mais", 16, delegate { Config.MudarSensibilidade(1); Mudou(); });
            Tela.Rotulo(botaoMais).text = "+";
            fpsB = Opcoes("Fps", delegate (int i) { Config.DefinirFps(i == 1 ? Configuracoes.FpsAlto : Configuracoes.FpsPadrao); }, "30", "60");
            desempenhoB = Opcoes("Desempenho", delegate (int i) { Config.DefinirMostrarDesempenho(i == 0); }, ligado, desligado);
            qualidadeB = Opcoes("Qualidade", delegate (int q) { Config.DefinirQualidade(q == 0 ? (FaixaQualidade?)null : (FaixaQualidade)(q - 1)); },
                qualidadeAuto, faixas[0], faixas[1], faixas[2]);
            botaoVoltar = Tela.Botao(painel.transform, "Voltar", 16, Fechar);
            Tela.Rotulo(botaoVoltar).text = voltar;
            fundo.gameObject.SetActive(false);
            Dispor();
        }

        /// <summary>Uma linha de opcoes de alternancia: tocar a de indice i chama `definir(i)`, aplica e remarca.</summary>
        Button[] Opcoes(string nome, System.Action<int> definir, params string[] textos)
        {
            var bs = new Button[textos.Length];
            for (int i = 0; i < textos.Length; i++)
            {
                int indice = i;
                bs[i] = Tela.Botao(painel.transform, nome + i, 16, delegate { definir(indice); Mudou(); });
                Tela.Rotulo(bs[i]).text = textos[i];
            }
            return bs;
        }

        void Mudou() { Aplicar(); Atualizar(); }

        /// <summary>Remarca as opcoes e o valor pela configuracao atual.</summary>
        void Atualizar()
        {
            if (canvas == null) return;
            Marcar(maoB, Config.Mao == HandPreset.Canhoto ? 1 : 0);
            Marcar(somB, Config.Som ? 0 : 1);
            Marcar(fpsB, Config.Fps == Configuracoes.FpsAlto ? 1 : 0);
            Marcar(desempenhoB, Config.MostrarDesempenho ? 0 : 1);
            Marcar(qualidadeB, Config.QualidadeEscolhida.HasValue ? 1 + (int)Config.QualidadeEscolhida.Value : 0);
            Tela.Rotulo(qualidadeB[0]).text = qualidadeAuto;
            textoValor.text = valorSensibilidade;
        }

        static void Marcar(Button[] bs, int atual) { for (int i = 0; i < bs.Length; i++) Tela.Marcar(bs[i], i == atual); }

        void Dispor()
        {
            alturaDisposta = Screen.height;
            safeDisposto = Screen.safeArea;
            Rect s = safeDisposto;
            float alvo = Tela.Alvo, m = alvo * 0.25f, gap = alvo * 0.15f;
            int fonte = Tela.Fonte(14f, 1f / 40f);
            // Engrenagem no topo, logo a esquerda da coluna do HUD de missao (80%+).
            Tela.Colocar(botaoAbrir.GetComponent<RectTransform>(), HudLayout.BotaoMenu(s, alvo));

            float tituloH = fonte * 1.3f * 1.6f;
            Rect p = HudLayout.PainelDoMenu(s, alvo, fonte);
            Tela.Colocar(painel.rectTransform, p);
            // Grade de 2 colunas: mao | som, sensibilidade | fps, desempenho; qualidade e voltar na largura toda.
            float largura = p.width - 2f * m, colW = (largura - 2f * gap) * 0.5f, rotuloW = colW * 0.4f;
            float topo = p.height - m - tituloH;   // de cima para baixo, em coordenadas do painel (origem embaixo)
            textoTitulo.fontSize = Mathf.RoundToInt(fonte * 1.3f);
            Tela.Colocar(textoTitulo.rectTransform, new Rect(m, topo, largura, tituloH));
            topo -= gap;

            Button[][] linhas = { maoB, somB, null, fpsB, desempenhoB, qualidadeB };
            for (int l = 0; l < linhas.Length; l++)
            {
                bool larga = l == linhas.Length - 1;   // qualidade: quatro opcoes, a linha toda
                int linha = larga ? 3 : l / 2, coluna = larga ? 0 : l % 2;
                float x = m + coluna * (colW + 2f * gap), y = topo - alvo - linha * (alvo + gap);
                float cx = x + rotuloW + gap, cw = (larga ? largura : colW) - rotuloW - gap;
                rotulosLinha[l].fontSize = fonte;
                rotulosLinha[l].resizeTextForBestFit = true;   // "Sensibilidade da camera" numa coluna estreita encolhe
                rotulosLinha[l].resizeTextMaxSize = fonte;
                rotulosLinha[l].resizeTextMinSize = Mathf.Max(10, fonte / 2);
                Tela.Colocar(rotulosLinha[l].rectTransform, new Rect(x, y, rotuloW, alvo));
                if (linhas[l] == null)   // sensibilidade: [-] valor [+]
                {
                    float bw = Mathf.Min(alvo * 1.2f, cw / 3f);
                    Tela.Colocar(botaoMenos.GetComponent<RectTransform>(), new Rect(cx, y, bw, alvo));
                    Tela.Colocar(textoValor.rectTransform, new Rect(cx + bw, y, cw - 2f * bw, alvo));
                    Tela.Colocar(botaoMais.GetComponent<RectTransform>(), new Rect(cx + cw - bw, y, bw, alvo));
                    textoValor.fontSize = fonte;
                    continue;
                }
                int n = linhas[l].Length;
                float ow = (cw - (n - 1) * gap) / n;
                for (int i = 0; i < n; i++)
                {
                    Tela.Colocar(linhas[l][i].GetComponent<RectTransform>(), new Rect(cx + i * (ow + gap), y, ow, alvo));
                    Text r = Tela.Rotulo(linhas[l][i]);
                    r.fontSize = fonte;
                    r.resizeTextForBestFit = true;   // "Auto (Media)" numa opcao estreita encolhe, nao corta
                    r.resizeTextMaxSize = fonte;
                    r.resizeTextMinSize = Mathf.Max(10, fonte / 2);
                }
            }
            Tela.Colocar(botaoVoltar.GetComponent<RectTransform>(), new Rect(m, m, largura, alvo));
            foreach (Button b in new[] { botaoAbrir, botaoVoltar, botaoMenos, botaoMais }) Tela.Rotulo(b).fontSize = fonte;
        }
    }
}
