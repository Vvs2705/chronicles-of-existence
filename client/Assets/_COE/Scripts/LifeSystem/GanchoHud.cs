using UnityEngine;
using UnityEngine.UI;

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

        public bool Aberto { get { return aberto; } }

        void Start()
        {
            StringsLoader.EnsureLoaded();
            titulo = Strings.Get("gancho.titulo");
            texto = Strings.Get("gancho.texto");
            continuar = Strings.Get("gancho.continuar");
        }

        void Update()
        {
            if (aberto) UiFundo.MarcarModal();
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

        // Modal no centro (20%-80% da area segura), botao >= 48 dp, texto que encolhe para caber. uGUI (Bloco D).
        Canvas canvas;
        Image painel;
        Text textoTitulo, textoTexto;
        Button botao;
        int alturaDisposta;
        Rect safeDisposto;

        void LateUpdate()
        {
            if (canvas == null)
            {
                if (!aberto) return;
                Montar();
            }
            if (canvas.enabled != aberto) canvas.enabled = aberto;
            if (aberto && (Screen.height != alturaDisposta || Screen.safeArea != safeDisposto)) Dispor();
        }

        void Montar()
        {
            canvas = Tela.NovoCanvas(transform, "GanchoCanvas", Tela.CamadaModal);
            Image fundo = Tela.FundoModal(canvas.transform);
            painel = Tela.Imagem(fundo.transform, "Painel", Tela.SpritePainel, Color.white);
            textoTitulo = Tela.Texto(painel.transform, "Titulo", 20, TextAnchor.MiddleCenter, UiEstilo.Ouro);
            textoTitulo.fontStyle = FontStyle.Bold;
            textoTitulo.text = titulo;
            textoTexto = Tela.Texto(painel.transform, "Texto", 16, TextAnchor.UpperLeft, UiEstilo.Tinta);
            textoTexto.resizeTextForBestFit = true;
            textoTexto.text = texto;
            botao = Tela.Botao(painel.transform, "Continuar", 16, Fechar);
            Tela.Rotulo(botao).text = continuar;
            Dispor();
        }

        void Dispor()
        {
            alturaDisposta = Screen.height;
            safeDisposto = Screen.safeArea;
            Rect s = safeDisposto;
            float alvo = Tela.Alvo, m = alvo * 0.25f;
            int fonte = Tela.Fonte(14f, 1f / 36f);
            Rect p = new Rect(s.x + s.width * 0.2f, s.y + s.height * 0.08f, s.width * 0.6f, s.height * 0.84f);
            Tela.Colocar(painel.rectTransform, p);
            textoTitulo.fontSize = Mathf.RoundToInt(fonte * 1.4f);
            Tela.Colocar(textoTitulo.rectTransform, new Rect(m, p.height - m - alvo, p.width - 2f * m, alvo));
            Tela.Colocar(textoTexto.rectTransform, new Rect(m, alvo + 2f * m, p.width - 2f * m, p.height - 2f * alvo - 3f * m));
            textoTexto.resizeTextMaxSize = fonte;
            textoTexto.resizeTextMinSize = Mathf.Max(10, Mathf.RoundToInt(fonte * 0.6f));
            Tela.Colocar(botao.GetComponent<RectTransform>(), new Rect(p.width - m - p.width * 0.4f, m, p.width * 0.4f, alvo));
            Tela.Rotulo(botao).fontSize = fonte;
        }
    }
}
