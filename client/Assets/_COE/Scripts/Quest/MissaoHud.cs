using System.Text;
using UnityEngine;

namespace COE
{
    /// <summary>T012: o lado de cena das missoes em Auren. A cada intervalo: MissaoMundo.Avancar (inicio automatico,
    /// conclusao, inventario — grava so se algo mudou), liga so os gatilhos que valem agora e monta o texto do HUD
    /// (periodo do dia, missoes em andamento com o objetivo atual, moedas).
    /// O OnGUI so desenha a string pronta (padrao do PerfHud: nada de concatenar por quadro).
    ///
    /// Dependencias por campo, ligadas por MissaoSceneSetup; a sessao e SaveState.Sessao (contrato do coordenador).
    /// Nao e God Manager: nao conhece NPC, dialogo nem salto — o que o dialogo cumprir aparece aqui na proxima leitura.
    /// ponytail: reavalia por tempo (5x/s), nao por evento. Teto: 8 missoes e 5 gatilhos, algumas listas pequenas por
    /// leitura. Evento de "save mudou" so se o catalogo crescer a ponto de pesar no perfil do celular.</summary>
    public class MissaoHud : MonoBehaviour
    {
        [SerializeField] QuestTrigger[] gatilhos = new QuestTrigger[0];
        [SerializeField] SomDoJogo som;   // a caixinha toca quando uma missao conclui; vazio = mudo
        int concluidas = -1;              // -1 = ainda nao leu: abrir a cena com missoes feitas nao toca nada
        [Tooltip("Segundos entre reavaliacoes das missoes (tempo real, nao para com timeScale 0).")]
        [SerializeField] float intervalo = 0.2f;

        float proxima;
        string texto = string.Empty;
        GUIStyle estilo;
        readonly StringBuilder sb = new StringBuilder();

        void Start()
        {
            useGUILayout = false;
            StringsLoader.EnsureLoaded();
            Atualizar();   // ao abrir a cena: a q01 comeca aqui (B06) e o gatilho certo ja aparece
        }

        void Update()
        {
            if (Time.unscaledTime >= proxima) Atualizar();
        }

        /// <summary>Uma reavaliacao completa. Publico para teste chamar sem esperar quadro.</summary>
        public void Atualizar()
        {
            proxima = Time.unscaledTime + intervalo;
            GameSession s = SaveState.Sessao;
            MissaoMundo.Avancar(s);
            int agora = s.Missoes.Concluidas();
            if (concluidas >= 0 && agora > concluidas && som != null) som.Tocar(Som.Missao);
            concluidas = agora;

            for (int i = 0; i < gatilhos.Length; i++)
            {
                QuestTrigger g = gatilhos[i];
                if (g == null) continue;
                bool ativo = MissaoMundo.Ativo(s.Missoes, g.QuestId, g.ObjetivoId);
                if (g.gameObject.activeSelf != ativo) g.gameObject.SetActive(ativo);
            }

            sb.Length = 0;
            // ADR-0007 §1: o periodo do dia e a primeira linha (anda ao concluir missao e ao descansar em casa).
            sb.Append(Strings.Get(TimeOfDayCycle.ChaveHud(TimeOfDayCycle.Atual(s.Save.life))));
            foreach (QuestDef d in MissaoMundo.EmAndamento(s.Missoes))
            {
                if (sb.Length > 0) sb.Append('\n');
                sb.Append(Strings.Get(d.TituloKey));
                ObjetivoDef o = MissaoMundo.Atual(s.Missoes, d.Id);
                if (o != null) sb.Append("\n- ").Append(Strings.Get(o.TextoKey));
            }
            int moedas = s.Save.inventario == null ? 0 : s.Save.inventario.moedas;
            if (moedas > 0) sb.Append(sb.Length > 0 ? "\n" : "").Append(Strings.Get("hud.moedas")).Append(' ').Append(moedas);
            texto = sb.ToString();
        }

        // Coluna da DIREITA (80%-100%), metade de cima da area segura: embaixo ficam o joystick (esquerda) e os botoes
        // (direita); o centro (20%-80%) e da conversa e dos avisos (DialogueHud); no alto a esquerda fica o PerfHud.
        // Fonte proporcional a tela, como PlayerInteractor/DamagePopup.
        string medido;
        float alturaMedida, larguraMedida;

        void OnGUI()
        {
            if (texto.Length == 0 || Event.current.type != EventType.Repaint || UiFundo.HaModal) return;
            int fonte = Mathf.Max(14, Screen.height / 40);
            if (estilo == null || estilo.fontSize != fonte) { estilo = UiEstilo.EstiloCartao(fonte); medido = null; }

            // Cartao no alto a direita (80%-100% da largura; o botao Menu fica logo a esquerda): embaixo ficam o joystick e os botoes; o centro e da conversa.
            Rect safe = Screen.safeArea;
            float margem = fonte * 0.6f;
            float x = Mathf.Max(safe.x, Screen.width * 0.8f), largura = safe.xMax - margem - x;
            if (texto != medido || largura != larguraMedida)
            {
                medido = texto;
                larguraMedida = largura;
                alturaMedida = estilo.CalcHeight(new GUIContent(texto), largura);
            }
            GUI.Box(new Rect(x, Screen.height - safe.yMax + margem, largura, alturaMedida), texto, estilo);
        }
    }
}
