using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace COE
{
    /// <summary>T012: o lado de cena das missoes em Auren. A cada intervalo: MissaoMundo.Avancar (inicio automatico,
    /// conclusao, inventario — grava so se algo mudou), liga so os gatilhos que valem agora e monta o texto do HUD
    /// (periodo do dia, missoes em andamento com o objetivo atual, moedas).
    /// O desenho (uGUI) so mostra a string pronta, e so quando ela muda.
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
        readonly StringBuilder sb = new StringBuilder();

        void Start()
        {
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
        // uGUI (Bloco D): o cartao so e refeito quando o texto ou a tela mudam.
        Canvas canvas;
        Image cartao;
        Text rotulo;
        string mostrado;
        int alturaDisposta;
        Rect safeDisposto;

        /// <summary>O cartao de missao (teste le).</summary>
        public RectTransform Cartao { get { return cartao != null ? cartao.rectTransform : null; } }

        void LateUpdate()
        {
            bool mostrar = texto.Length > 0 && !UiFundo.HaModal;
            if (canvas == null)
            {
                if (!mostrar) return;
                canvas = Tela.NovoCanvas(transform, "MissaoCanvas", Tela.CamadaHud);
                cartao = Tela.Imagem(canvas.transform, "Cartao", Tela.SpriteCartao, Color.white);
                rotulo = Tela.Texto(cartao.transform, "Texto", 14, TextAnchor.UpperLeft, UiEstilo.Tinta);
                rotulo.fontStyle = FontStyle.Bold;
            }
            if (canvas.enabled != mostrar) canvas.enabled = mostrar;
            if (!mostrar) return;
            if (texto == mostrado && Screen.height == alturaDisposta && Screen.safeArea == safeDisposto) return;
            mostrado = texto;
            alturaDisposta = Screen.height;
            safeDisposto = Screen.safeArea;

            int fonte = Mathf.Max(14, Screen.height / 40);
            Rect safe = safeDisposto;
            float margem = fonte * 0.6f, padX = fonte * 2 / 3, padY = fonte / 2;
            float x = Mathf.Max(safe.x, Screen.width * 0.8f), largura = safe.xMax - margem - x;
            rotulo.fontSize = fonte;
            rotulo.text = texto;
            TextGenerationSettings medida = rotulo.GetGenerationSettings(new Vector2(largura - 2f * padX, 0f));
            float altura = rotulo.cachedTextGeneratorForLayout.GetPreferredHeight(texto, medida) / rotulo.pixelsPerUnit + 2f * padY;
            Tela.Colocar(cartao.rectTransform, new Rect(x, safe.yMax - margem - altura, largura, altura));
            Tela.Colocar(rotulo.rectTransform, new Rect(padX, padY, largura - 2f * padX, altura - 2f * padY));
        }
    }
}
