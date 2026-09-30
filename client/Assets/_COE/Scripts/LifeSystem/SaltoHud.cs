using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        string texto, seguir, aindaNao, confirmar, tresAnosDepois;   // montados fora do OnGUI: ele roda 2x+ por quadro
        GUIStyle estiloTexto, estiloBotao, estiloDepois;

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

        void Awake() { useGUILayout = false; }   // so GUI.* posicionado: pula o passe de Layout do OnGUI

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
        void Confirmar()
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

        // ---- tela ----
        // ponytail: prototipo IMGUI (mesmo padrao de PlayerInteractor/PerfHud). Modal no centro (25%-75% da largura):
        // fora do joystick (esquerda) e dos botoes (direita). Botoes >= 48 dp (ControlPreset.MinTargetDp), fonte
        // proporcional a tela. Texto longo demais transborda: a UI de verdade (Canvas, rolagem) e a T013.

        void OnGUI()
        {
            if (estiloTexto == null) Estilos();
            float alvo = Mathf.Max(ControlPreset.DpToPx(ControlPreset.MinTargetDp, Screen.dpi), Screen.height / 10f);
            float m = alvo * 0.25f;

            if (Time.unscaledTime < depoisAte)
                GUI.Label(new Rect(0f, Screen.height * 0.35f, Screen.width, Screen.height * 0.3f), tresAnosDepois, estiloDepois);

            if (aberto == null)
            {
                // Topo ao centro, abaixo da faixa de aviso do DialogueHud (~7% da altura) e longe do HUD de missao (direita).
                if (disponivel && gatilho == null && GUI.Button(new Rect(Screen.width * 0.35f, Screen.height * 0.1f, Screen.width * 0.3f, alvo),
                                             seguir, estiloBotao))
                    Abrir();
                return;
            }

            Rect painel = new Rect(Screen.width * 0.25f, Screen.height * 0.06f, Screen.width * 0.5f, Screen.height * 0.88f);
            UiFundo.Modal(painel);   // opaco: escurece o mundo e o painel nao deixa a HUD de toque aparecer
            GUI.Label(new Rect(painel.x + m, painel.y + m, painel.width - 2f * m, painel.height - alvo - 3f * m), texto, estiloTexto);

            float largura = (painel.width - 3f * m) * 0.5f;
            float y = painel.yMax - m - alvo;
            if (GUI.Button(new Rect(painel.x + m, y, largura, alvo), aindaNao, estiloBotao))
                Fechar();
            else if (GUI.Button(new Rect(painel.x + 2f * m + largura, y, largura, alvo), confirmar, estiloBotao))
                Confirmar();
        }

        void Estilos()
        {
            int fonte = Mathf.RoundToInt(Mathf.Max(ControlPreset.DpToPx(14f, Screen.dpi), Screen.height / 40f));
            estiloTexto = new GUIStyle(GUI.skin.label) { fontSize = fonte, wordWrap = true, alignment = TextAnchor.UpperLeft };
            estiloBotao = new GUIStyle(GUI.skin.button) { fontSize = fonte, fontStyle = FontStyle.Bold, wordWrap = true };
            estiloDepois = new GUIStyle(GUI.skin.label) { fontSize = fonte * 2, alignment = TextAnchor.MiddleCenter };
        }
    }
}
