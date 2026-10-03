using UnityEngine;

namespace COE
{
    /// <summary>B16 — o gancho (fim do slice). Quando o treino do B15 termina (GameSession.GanchoPendente), abre uma tela
    /// que deixa as perguntas abertas: o simbolo, o desaparecimento e algo que se rompeu. Nao responde nenhuma e nao promete
    /// continuacao (aceite do SLICE B16). "Continuar em Auren" grava o marco (uma vez) e devolve o jogador a vila.
    /// Nenhuma mecanica nova: so le a sessao e chama VerGancho. Enquanto aberta, "travar" (motor, combate, interacao)
    /// fica desligado. So em Auren (o gerador poe aqui; a Bootstrap e area de treino de desenvolvimento).</summary>
    public class GanchoHud : MonoBehaviour
    {
        [Tooltip("Desligados enquanto a tela esta aberta: o personagem nao anda nem ataca. Ligados pelo gerador.")]
        [SerializeField] Behaviour[] travar;

        bool aberto;
        string titulo, texto, continuar;
        GUIStyle estiloTitulo, estiloTexto, estiloBotao;

        public bool Aberto { get { return aberto; } }

        void Awake() { useGUILayout = false; }

        void Start()
        {
            StringsLoader.EnsureLoaded();
            titulo = Strings.Get("gancho.titulo");
            texto = Strings.Get("gancho.texto");
            continuar = Strings.Get("gancho.continuar");
        }

        void Update()
        {
            if (aberto || SaveState.Sessao == null || !SaveState.Sessao.GanchoPendente()) return;
            aberto = true;
            Travar(true);
        }

        /// <summary>"Continuar em Auren": grava o marco (primeira vez) e devolve o controle.</summary>
        public void Fechar()
        {
            if (SaveState.Sessao != null) SaveState.Sessao.VerGancho();
            aberto = false;
            Travar(false);
        }

        void Travar(bool travado)
        {
            if (travar == null) return;
            foreach (Behaviour b in travar) if (b != null) b.enabled = !travado;
        }

        // ponytail: prototipo IMGUI, mesmo padrao do SaltoHud (modal no centro, botao >= 48 dp). A UI de verdade e a T013.
        void OnGUI()
        {
            if (!aberto) return;
            if (estiloTexto == null) Estilos();
            GUI.depth = -50;
            float alvo = Mathf.Max(ControlPreset.DpToPx(ControlPreset.MinTargetDp, Screen.dpi), Screen.height / 10f);
            float m = alvo * 0.25f;

            Rect painel = new Rect(Screen.width * 0.2f, Screen.height * 0.08f, Screen.width * 0.6f, Screen.height * 0.84f);
            UiFundo.Modal(painel);
            GUI.Label(new Rect(painel.x + m, painel.y + m, painel.width - 2f * m, alvo), titulo, estiloTitulo);
            GUI.Label(new Rect(painel.x + m, painel.y + m + alvo, painel.width - 2f * m, painel.height - 2f * alvo - 3f * m), texto, estiloTexto);
            if (GUI.Button(new Rect(painel.xMax - m - painel.width * 0.4f, painel.yMax - m - alvo, painel.width * 0.4f, alvo), continuar, estiloBotao))
                Fechar();
            if (Event.current.isMouse) Event.current.Use();   // modal: o toque nao passa para a HUD de tras
        }

        void Estilos()
        {
            int fonte = Mathf.RoundToInt(Mathf.Max(ControlPreset.DpToPx(14f, Screen.dpi), Screen.height / 36f));
            estiloTitulo = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(fonte * 1.4f), fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter };
            estiloTitulo.normal.textColor = UiEstilo.Ouro;
            estiloTexto = UiEstilo.EstiloTexto(fonte, TextAnchor.UpperLeft);
            estiloBotao = UiEstilo.EstiloBotao(fonte, true);
        }
    }
}
