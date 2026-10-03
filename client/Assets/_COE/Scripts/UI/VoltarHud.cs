using UnityEngine;
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
    /// CONFIRMACAO: modal no padrao do menu (UiFundo.Modal, UiEstilo, botoes >= 48 dp), Time.timeScale 0 e "travar"
    /// desligado (a mesma lista do menu de pausa: o toque fora dos botoes nao anda, nao ataca, nao interage).
    /// "Sair" = Application.Quit (o SaveBootstrap grava no OnApplicationQuit; toda transicao ja gravou).
    /// ponytail: prototipo IMGUI; a UI de verdade (Canvas) e a T013.</summary>
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
            // A entrada cobre a tela inteira (GUI.depth -100) e o gancho e o modal de cima em Auren: o que estiver por
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
        GUIStyle estiloTitulo, estiloTexto, estiloBotao;

        void Start()
        {
            StringsLoader.EnsureLoaded();   // textos montados aqui: o OnGUI roda 2x+ por quadro
            titulo = Strings.Get("saida.titulo");
            sair = Strings.Get("saida.sair");
            continuar = Strings.Get("saida.continuar");
            // Save de versao mais nova: o LocalSave recusa toda gravacao desta sessao, entao nao promete "salvo".
            texto = EntryFlow.SaveMaisNovo(LocalSave.DefaultPath) ? "" : Strings.Get("saida.texto");
        }

        void Update()
        {
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

        // ---- tela ----
        // Painel no centro (25%-75%) e acima do meio: fora do joystick e dos botoes de toque (canto de baixo).

        void OnGUI()
        {
            if (!SaidaAberta) return;
            // Na frente de tudo, ate do EntryFlow (-100). useGUILayout fica ligado: sem o passe de Layout o depth nao vale.
            GUI.depth = -200;
            Estilos();
            float w = Screen.width, h = Screen.height;
            float alvo = Mathf.Max(ControlPreset.DpToPx(ControlPreset.MinTargetDp, Screen.dpi), h / 10f);
            float m = alvo * 0.25f, gap = alvo * 0.3f;
            float tituloH = estiloTitulo.fontSize * 1.6f;
            float textoH = texto.Length > 0 ? estiloTexto.fontSize * 2.8f : 0f;
            float painelH = 2f * m + tituloH + textoH + gap + alvo;
            Rect painel = new Rect(w * 0.25f, (h - painelH) * 0.4f, w * 0.5f, painelH);
            UiFundo.Modal(painel);

            float x = painel.x + m, largura = painel.width - 2f * m, y = painel.y + m;
            GUI.Label(new Rect(x, y, largura, tituloH), titulo, estiloTitulo);
            y += tituloH;
            if (textoH > 0f) GUI.Label(new Rect(x, y, largura, textoH), texto, estiloTexto);
            y += textoH + gap;

            // Esquerda = ficar (cancelar), direita = seguir: a convencao das telas do EntryFlow.
            float bw = (largura - gap) * 0.5f;
            if (GUI.Button(new Rect(x, y, bw, alvo), continuar, estiloBotao)) FecharSaida();
            else if (GUI.Button(new Rect(x + bw + gap, y, bw, alvo), sair, estiloBotao)) Application.Quit();

            Event e = Event.current;   // modal: o toque que sobrou nao chega nas HUDs de tras
            if (e.type != EventType.Repaint && e.type != EventType.Layout) e.Use();
        }

        void Estilos()
        {
            int fonte = Mathf.RoundToInt(Mathf.Max(ControlPreset.DpToPx(14f, Screen.dpi), Screen.height / 40f));
            if (estiloTexto != null && estiloTexto.fontSize == fonte) return;   // refaz so se a tela mudou
            estiloTexto = UiEstilo.EstiloTexto(fonte, TextAnchor.MiddleCenter);
            estiloTitulo = new GUIStyle(estiloTexto) { fontSize = Mathf.RoundToInt(fonte * 1.3f), fontStyle = FontStyle.Bold };
            estiloTitulo.normal.textColor = UiEstilo.Ouro;
            estiloBotao = UiEstilo.EstiloBotao(fonte, true);
        }
    }
}
