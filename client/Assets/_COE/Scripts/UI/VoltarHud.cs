using UnityEngine;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace COE
{
    public enum AcaoDoVoltar { Nada, FecharSaida, VoltarEntrada, FecharConversa, FecharMenu, FecharSalto, PedirSaida }

    /// <summary>O "voltar" do Android (gesto ou botao) e o Esc no PC: fecha o que estiver aberto, uma coisa por toque, e
    /// na raiz pede confirmacao para sair (DIVIDA_TECNICA, T012).
    ///
    /// COMO CHEGA: com targetSdk 36 o Android 16 nao despacha mais KEYCODE_BACK nem chama onBackPressed; o Unity usa
    /// OnBackInvokedCallback quando "Predictive Back Gesture Support" esta ligado (ProjectSetup liga) e entrega o voltar
    /// ao script como Esc. Input.backButtonLeavesApp fica false (padrao): o app nunca fecha sozinho, quem decide e aqui.
    ///
    /// REGRA (Decidir, pura): confirmacao de saida aberta -> fecha; tela da entrada -> o "Voltar"/"Cancelar" dela, ou
    /// pede saida na raiz; gancho (B16) -> nada (a tela e de leitura e tem o proprio botao); conversa -> fecha; menu ->
    /// fecha; aviso do salto -> "ainda nao" (nada grava); nada aberto -> pede saida.
    /// CONFIRMACAO: modal no padrao do menu (Tela.FundoModal, UiEstilo, botoes >= 48 dp), Time.timeScale 0 e "travar"
    /// desligado (a mesma lista do menu de pausa: o toque fora dos botoes nao anda, nao ataca, nao interage).
    /// "Sair" = Application.Quit (o SaveBootstrap grava no OnApplicationQuit; toda transicao ja gravou).
    /// Desenho em uGUI (Bloco D), na camada de sistema (na frente de tudo).</summary>
    public class VoltarHud : MonoBehaviour
    {
        /// <summary>O que esta aberto quando o voltar chega. Cada campo vale so se a cena tem aquela tela.</summary>
        public struct Estado
        {
            public bool saida, entrada, entradaPodeVoltar, gancho, conversa, menu, salto;
        }

        /// <summary>Uma acao por voltar, da tela mais de cima para a mais de baixo. Nunca fecha o que nao esta aberto.</summary>
        public static AcaoDoVoltar Decidir(Estado e)
        {
            if (e.saida) return AcaoDoVoltar.FecharSaida;
            // A entrada cobre a tela inteira (Tela.CamadaEntrada) e o gancho e o modal de cima em Auren: o que estiver por
            // baixo deles nao recebe o voltar.
            if (e.entrada) return e.entradaPodeVoltar ? AcaoDoVoltar.VoltarEntrada : AcaoDoVoltar.PedirSaida;
            if (e.gancho) return AcaoDoVoltar.Nada;
            if (e.conversa) return AcaoDoVoltar.FecharConversa;
            if (e.menu) return AcaoDoVoltar.FecharMenu;
            if (e.salto) return AcaoDoVoltar.FecharSalto;
            return AcaoDoVoltar.PedirSaida;
        }

        [Tooltip("Telas que o voltar fecha. Cada uma e opcional (a cena pode nao ter). Ligadas pelo VoltarSetup.")]
        [SerializeField] DialogueHud conversa;
        [SerializeField] MenuDePausa menu;
        [SerializeField] SaltoHud salto;
        [SerializeField] GanchoHud gancho;
        [SerializeField] EntryFlow entrada;
        [Tooltip("Desligados com a confirmacao aberta: a mesma lista do menu de pausa (motor, combate, interacao, camera).")]
        [SerializeField] Behaviour[] travar = new Behaviour[0];

        public bool SaidaAberta { get; private set; }

        bool[] desligados = new bool[0];
        float escalaAntes = 1f;
        int quadroDoUltimo = -100;
        string titulo, texto, sair, continuar;

        void Start()
        {
            StringsLoader.EnsureLoaded();   // textos montados uma vez
            titulo = Strings.Get("saida.titulo");
            sair = Strings.Get("saida.sair");
            continuar = Strings.Get("saida.continuar");
            // Save de versao mais nova: o LocalSave recusa toda gravacao desta sessao, entao nao promete "salvo".
            texto = EntryFlow.SaveMaisNovo(LocalSave.DefaultPath) ? "" : Strings.Get("saida.texto");
        }

        void Update()
        {
            if (SaidaAberta) UiFundo.MarcarModal();
            if (!ApertouVoltar()) return;
            Aplicar(Decidir(new Estado
            {
                saida = SaidaAberta,
                entrada = entrada != null && entrada.Aberta,
                entradaPodeVoltar = entrada != null && entrada.PodeVoltar,
                gancho = gancho != null && gancho.Aberto,
                conversa = conversa != null && conversa.Aberta,
                menu = menu != null && menu.Aberto,
                salto = salto != null && salto.Aberto,
            }));
        }

        // Le pelo Input System e pelo legado (activeInputHandler = Both): ha relato de cada caminho falhar no Android com
        // o voltar preditivo. Os dois no mesmo aperto contam uma vez; a janela de 3 quadros cobre um chegar um quadro
        // depois do outro (ninguem aperta voltar duas vezes em 50 ms).
        // ponytail: so Esc. Gamepad (botao Leste) e o callback nativo direto ficam para a UI de verdade (T013).
        bool ApertouVoltar()
        {
            bool v = false;
#if ENABLE_INPUT_SYSTEM
            Keyboard kb = Keyboard.current;
            v = kb != null && kb.escapeKey.wasPressedThisFrame;
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
            v |= UnityEngine.Input.GetKeyDown(KeyCode.Escape);
#endif
            if (!v || Time.frameCount - quadroDoUltimo <= 3) return false;
            quadroDoUltimo = Time.frameCount;
            return true;
        }

        void Aplicar(AcaoDoVoltar a)
        {
            switch (a)
            {
                case AcaoDoVoltar.FecharSaida: FecharSaida(); break;
                case AcaoDoVoltar.VoltarEntrada: entrada.Voltar(); break;
                case AcaoDoVoltar.FecharConversa: conversa.Fechar(); break;
                case AcaoDoVoltar.FecharMenu: menu.Fechar(); break;
                case AcaoDoVoltar.FecharSalto: salto.Fechar(); break;
                case AcaoDoVoltar.PedirSaida: AbrirSaida(); break;
            }
        }

        public void AbrirSaida()
        {
            if (SaidaAberta) return;
            SaidaAberta = true;
            escalaAntes = Time.timeScale;
            Time.timeScale = 0f;
            if (desligados.Length != travar.Length) desligados = new bool[travar.Length];
            for (int i = 0; i < travar.Length; i++)
            {
                Behaviour b = travar[i];
                desligados[i] = b != null && b.enabled;   // so religa o que ESTA confirmacao desligou
                if (desligados[i]) b.enabled = false;
            }
        }

        /// <summary>"Continuar jogando". Idempotente.</summary>
        public void FecharSaida()
        {
            if (!SaidaAberta) return;
            SaidaAberta = false;
            Time.timeScale = escalaAntes;
            for (int i = 0; i < travar.Length && i < desligados.Length; i++)
                if (desligados[i] && travar[i] != null) travar[i].enabled = true;
        }

        // Troca de cena (ou objeto destruido) com a confirmacao aberta nao pode deixar o jogo congelado.
        void OnDisable() { FecharSaida(); }

        // ---- tela (uGUI, Bloco D) ----
        // Painel no centro (25%-75% da area segura) e acima do meio: fora do joystick e dos botoes de toque. Camada de
        // sistema: na frente de tudo, ate da entrada.
        Canvas canvas;
        Image painel;
        Text textoTitulo, textoTexto;
        Button botaoContinuar, botaoSair;
        int alturaDisposta;
        Rect safeDisposto;

        void LateUpdate()
        {
            if (canvas == null)
            {
                if (!SaidaAberta) return;
                Montar();
            }
            if (canvas.enabled != SaidaAberta) canvas.enabled = SaidaAberta;
            if (SaidaAberta && (Screen.height != alturaDisposta || Screen.safeArea != safeDisposto)) Dispor();
        }

        void Montar()
        {
            canvas = Tela.NovoCanvas(transform, "SaidaCanvas", Tela.CamadaSistema);
            Image fundo = Tela.FundoModal(canvas.transform);
            painel = Tela.Imagem(fundo.transform, "Painel", Tela.SpritePainel, Color.white);
            textoTitulo = Tela.Texto(painel.transform, "Titulo", 20, TextAnchor.MiddleCenter, UiEstilo.Ouro);
            textoTitulo.fontStyle = FontStyle.Bold;
            textoTitulo.text = titulo;
            textoTexto = Tela.Texto(painel.transform, "Texto", 16, TextAnchor.MiddleCenter, UiEstilo.Tinta);
            textoTexto.text = texto;
            // Esquerda = ficar (cancelar), direita = seguir: a convencao das telas da entrada.
            botaoContinuar = Tela.Botao(painel.transform, "Continuar", 16, FecharSaida);
            Tela.Rotulo(botaoContinuar).text = continuar;
            botaoSair = Tela.Botao(painel.transform, "Sair", 16, Application.Quit);
            Tela.Rotulo(botaoSair).text = sair;
            Dispor();
        }

        void Dispor()
        {
            alturaDisposta = Screen.height;
            safeDisposto = Screen.safeArea;
            Rect s = safeDisposto;
            float alvo = Tela.Alvo, m = alvo * 0.25f, gap = alvo * 0.3f;
            int fonte = Tela.Fonte(14f, 1f / 40f);
            float tituloH = fonte * 1.3f * 1.6f;
            float textoH = texto.Length > 0 ? fonte * 2.8f : 0f;
            float painelH = 2f * m + tituloH + textoH + gap + alvo;
            Rect p = new Rect(s.x + s.width * 0.25f, s.y + (s.height - painelH) * 0.6f, s.width * 0.5f, painelH);
            Tela.Colocar(painel.rectTransform, p);
            float largura = p.width - 2f * m;
            textoTitulo.fontSize = Mathf.RoundToInt(fonte * 1.3f);
            Tela.Colocar(textoTitulo.rectTransform, new Rect(m, p.height - m - tituloH, largura, tituloH));
            textoTexto.fontSize = fonte;
            textoTexto.gameObject.SetActive(textoH > 0f);
            Tela.Colocar(textoTexto.rectTransform, new Rect(m, m + alvo + gap, largura, textoH));
            float bw = (largura - gap) * 0.5f;
            Tela.Colocar(botaoContinuar.GetComponent<RectTransform>(), new Rect(m, m, bw, alvo));
            Tela.Colocar(botaoSair.GetComponent<RectTransform>(), new Rect(m + bw + gap, m, bw, alvo));
            Tela.Rotulo(botaoContinuar).fontSize = fonte;
            Tela.Rotulo(botaoSair).fontSize = fonte;
        }
    }
}
