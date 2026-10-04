using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace COE
{
    /// <summary>Tela do salto temporal (T012; slice B12, B13 e §4): aviso "o que se encerra" -> confirmacao -> recarrega.
    ///
    /// FLUXO: com o salto liberado (Q-08 concluida e ainda 5 anos) aparece no topo o botao "seguir adiante". Toca-lo
    /// abre o aviso B12 (idade, fase, missoes opcionais que se encerram pelo titulo, vinculos que mudam e o que
    /// sobrevive). Dali: "ainda nao" fecha sem mudar nada (R18: termina as opcionais e volta) ou "deixar a infancia
    /// para tras" (B13) confirma pela sessao -- ela envelhece, encerra as opcionais, sincroniza e grava UMA vez; recusa
    /// nao grava. Aplicado, a cena recarrega: o BodyByAge poe o corpo de 8 anos e o AnchorSpawn entra em spawn_player
    /// (o salto zerou anchorId). A cena nova mostra "tres anos depois" (B14).
    /// Enquanto o aviso esta aberto os componentes em "travar" (motor, combate, interacao) ficam desligados.
    /// Nada aqui grava: so SaveState.Sessao.
    ///
    /// ONDE: em Auren o salto e oferecido so no simbolo do Limiar da clareira (§4.1): o gerador liga o SaltoGatilho em
    /// "gatilho", e este HUD o habilita so quando o salto esta liberado. Sem gatilho (Bootstrap, area de treino de
    /// desenvolvimento) aparece o botao "seguir adiante" no topo.</summary>
    public class SaltoHud : MonoBehaviour
    {
        [Tooltip("Desligados enquanto o aviso esta aberto: o personagem nao anda nem ataca. Ligados pelo gerador.")]
        [SerializeField] Behaviour[] travar;
        [SerializeField] float segundosTresAnosDepois = 3f;
        [Tooltip("Simbolo na clareira que abre o aviso. Nulo = botao no topo (cena sem clareira).")]
        [SerializeField] Interactable gatilho;

        // Sobrevive ao LoadScene (estatico): a cena recarregada mostra "tres anos depois". Nao vai para o save.
        static bool acabouDeSaltar;

        SaltoPreparado aberto;   // null = aviso fechado
        bool disponivel;
        float depoisAte;
        string texto, seguir, aindaNao, confirmar, tresAnosDepois;   // montados uma vez (Start, Abrir)

        public bool Aberto { get { return aberto != null; } }

        /// <summary>O botao "seguir adiante" aparece? Consulta pura (nao muda nem grava): salto liberado pela Q-08,
        /// ainda nao aplicado e a idade ainda antes do alvo.</summary>
        public static bool Disponivel(GameSession sessao)
        {
            return sessao != null && AgeAdvance.PodeAvancarIdade(sessao.Save, AgeAdvanceCatalog.SaltoInfancia, sessao.Historia);
        }

        /// <summary>Texto do aviso B12 (§4.1), rotulos por Strings. As opcionais aparecem pelo TITULO (nunca o id);
        /// sem nenhuma aberta a secao inteira some (B12: nada de lista vazia com moldura).</summary>
        public static string Aviso(SaltoPreparado p)
        {
            var sb = new StringBuilder();
            sb.AppendLine(Strings.Get("salto.titulo"));
            sb.AppendLine(Strings.Format("salto.idade", p.IdadeAtual, p.IdadeDepois));
            if (p.MudaDeFase)
                sb.AppendLine(Strings.Format("salto.fase", Strings.Get("fase." + LifePhases.Id(p.FaseAtual)),
                                             Strings.Get("fase." + LifePhases.Id(p.FaseDepois))));
            if (p.OportunidadesEncerradas.Length > 0)
            {
                sb.AppendLine().AppendLine(Strings.Get("salto.encerra"));
                foreach (string id in p.OportunidadesEncerradas)
                {
                    QuestDef d = QuestCatalog.Missao(id);
                    sb.Append("  - ").AppendLine(Strings.Get(d != null ? d.TituloKey : id));
                }
            }
            sb.AppendLine().AppendLine(Strings.Get("salto.vinculos"));
            sb.AppendLine().Append(Strings.Get("salto.sobrevive"));
            return sb.ToString();
        }

        void Start()
        {
            StringsLoader.EnsureLoaded();
            seguir = Strings.Get("salto.seguir");
            aindaNao = Strings.Get("salto.ainda_nao");
            confirmar = Strings.Get("salto.confirmar");            // §4.2: diz o que acontece, nao "OK"
            tresAnosDepois = Strings.Get("salto.tres_anos_depois");
            if (!acabouDeSaltar) return;
            acabouDeSaltar = false;
            depoisAte = Time.unscaledTime + segundosTresAnosDepois;
        }

        void Update()
        {
            if (aberto != null) UiFundo.MarcarModal();
            disponivel = aberto == null && Disponivel(SaveState.Sessao);
            if (gatilho != null && gatilho.enabled != disponivel) gatilho.enabled = disponivel;
        }

        /// <summary>Abre o aviso (B12) com o preparo da sessao. Nao muda nada. Publico: a clareira pode chamar.</summary>
        public void Abrir()
        {
            SaltoPreparado p = SaveState.Sessao.PrepararSalto();
            if (!p.Possivel) return;
            aberto = p;
            texto = Aviso(p);
            Travar(true);
        }

        /// <summary>"Ainda nao": fecha o aviso e devolve o controle. Nada muda nem grava.</summary>
        public void Fechar()
        {
            aberto = null;
            Travar(false);
        }

        /// <summary>B13: confirma com o MESMO preparo que o jogador leu. Aplicado = recarrega a cena (corpo de 8 anos,
        /// spawn_player). Recusado (preparo velho, ja aplicado) = so fecha.</summary>
        public void Confirmar()
        {
            SaltoResultado r = SaveState.Sessao.ConfirmarSalto(aberto);
            Fechar();
            if (!r.Aplicado) return;
            acabouDeSaltar = true;
            SceneManager.LoadScene(gameObject.scene.name);   // Auren: o salto e nela (sceneId fica)
        }

        void Travar(bool travado)
        {
            if (travar == null) return;
            foreach (Behaviour b in travar) if (b != null) b.enabled = !travado;
        }

        // ---- tela (uGUI, Bloco D) ----
        // Modal no centro (25%-75% da area segura): fora do joystick (esquerda) e dos botoes (direita). Botoes >= 48 dp,
        // fonte proporcional a tela; texto longo encolhe para caber (ajuste de fonte) em vez de transbordar.

        Canvas canvas;
        Button botaoSeguir, botaoAindaNao, botaoConfirmar;
        Text textoDepois, textoAviso;
        Image fundo, painel;
        int alturaDisposta;
        Rect safeDisposto;

        /// <summary>A tela do salto (teste le).</summary>
        public Canvas Vista { get { return canvas; } }

        void LateUpdate()
        {
            bool depois = Time.unscaledTime < depoisAte;
            bool seguirVisivel = aberto == null && disponivel && gatilho == null;
            bool algo = depois || seguirVisivel || aberto != null;
            if (canvas == null)
            {
                if (!algo) return;
                Montar();
            }
            if (canvas.enabled != algo) canvas.enabled = algo;
            if (!algo) return;
            if (Screen.height != alturaDisposta || Screen.safeArea != safeDisposto) Dispor();
            Ligar(textoDepois.gameObject, depois);
            Ligar(botaoSeguir.gameObject, seguirVisivel);
            Ligar(fundo.gameObject, aberto != null);
            if (aberto != null && textoAviso.text != texto) textoAviso.text = texto;
        }

        static void Ligar(GameObject go, bool sim) { if (go.activeSelf != sim) go.SetActive(sim); }

        void Montar()
        {
            canvas = Tela.NovoCanvas(transform, "SaltoCanvas", Tela.CamadaModal);
            textoDepois = Tela.Texto(canvas.transform, "TresAnosDepois", 28, TextAnchor.MiddleCenter, UiEstilo.Ouro);
            textoDepois.fontStyle = FontStyle.Bold;
            textoDepois.text = tresAnosDepois;
            botaoSeguir = Tela.Botao(canvas.transform, "Seguir", 16, Abrir);
            Tela.Rotulo(botaoSeguir).text = seguir;
            fundo = Tela.FundoModal(canvas.transform);   // opaco: escurece o mundo; o painel e filho dele
            painel = Tela.Imagem(fundo.transform, "Painel", Tela.SpritePainel, Color.white);
            textoAviso = Tela.Texto(painel.transform, "Aviso", 16, TextAnchor.UpperLeft, UiEstilo.Tinta);
            textoAviso.resizeTextForBestFit = true;
            botaoAindaNao = Tela.Botao(painel.transform, "AindaNao", 16, Fechar);
            Tela.Rotulo(botaoAindaNao).text = aindaNao;
            botaoConfirmar = Tela.Botao(painel.transform, "Confirmar", 16, Confirmar);
            Tela.Rotulo(botaoConfirmar).text = confirmar;
            Dispor();
        }

        void Dispor()
        {
            alturaDisposta = Screen.height;
            safeDisposto = Screen.safeArea;
            Rect s = safeDisposto;
            float alvo = Tela.Alvo, m = alvo * 0.25f;
            int fonte = Tela.Fonte(14f, 1f / 40f);

            textoDepois.fontSize = fonte * 2;
            Tela.Colocar(textoDepois.rectTransform, new Rect(0f, Screen.height * 0.35f, Screen.width, Screen.height * 0.3f));
            // Topo ao centro, abaixo da faixa de aviso do DialogueHud (~7% da altura) e longe do HUD de missao (direita).
            Tela.Colocar(botaoSeguir.GetComponent<RectTransform>(), HudLayout.BotaoSeguir(new Vector2(Screen.width, Screen.height), s, alvo));

            Rect p = new Rect(s.x + s.width * 0.25f, s.y + s.height * 0.06f, s.width * 0.5f, s.height * 0.88f);
            Tela.Colocar(painel.rectTransform, p);
            Tela.Colocar(textoAviso.rectTransform, new Rect(m, alvo + 2f * m, p.width - 2f * m, p.height - alvo - 3f * m));
            textoAviso.resizeTextMaxSize = fonte;
            textoAviso.resizeTextMinSize = Mathf.Max(10, Mathf.RoundToInt(fonte * 0.6f));
            float largura = (p.width - 3f * m) * 0.5f;
            Tela.Colocar(botaoAindaNao.GetComponent<RectTransform>(), new Rect(m, m, largura, alvo));
            Tela.Colocar(botaoConfirmar.GetComponent<RectTransform>(), new Rect(2f * m + largura, m, largura, alvo));
            foreach (Button b in new[] { botaoSeguir, botaoAindaNao, botaoConfirmar }) Tela.Rotulo(b).fontSize = fonte;
        }
    }
}
