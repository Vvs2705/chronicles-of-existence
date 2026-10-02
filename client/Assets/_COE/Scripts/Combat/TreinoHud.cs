using UnityEngine;

namespace COE
{
    /// <summary>B15 / R7 — painel de progresso do treino: a cada pratica que chega ao Mastery (TrainingProgress.Anotar),
    /// uma linha no alto da tela diz quanto aquele golpe rendeu contra o teto da etapa e, parado no teto, POR QUE parou e
    /// o que ainda rende (outro golpe, crescer). Some depois de alguns segundos. So le; nao muda nada.
    /// ponytail: prototipo IMGUI, uma linha so. A UI de verdade (barra por verbo) e a T013.</summary>
    public class TreinoHud : MonoBehaviour
    {
        [SerializeField] float segundosNaTela = 3f;

        int visto;
        string texto;
        float ate;
        GUIStyle estilo;

        /// <summary>A linha da tela para uma pratica. null = pratica recusada (nada a mostrar).</summary>
        public static string Texto(AtividadeDef atividade, GanhoResultado r)
        {
            if (!r.Aceito || atividade == null) return null;
            string verbo = Strings.Get("treino.atividade." + atividade.Id);
            if (r.Saturada && r.ProgressoGanho == 0) return Strings.Format("treino.saturado", verbo);
            if (r.Marco == MarcoDeDominio.Dominio) return Strings.Format("treino.dominio", verbo);
            if (r.Trivial) return Strings.Format("treino.trivial", verbo, r.ProgressoDaAtividade, r.Teto);
            return Strings.Format("treino.progresso", verbo, r.ProgressoDaAtividade, r.Teto);
        }

        void Awake() { useGUILayout = false; }

        void Start()
        {
            StringsLoader.EnsureLoaded();
            visto = TrainingProgress.Registros;   // pratica de antes desta cena nao reaparece
        }

        void Update()
        {
            if (TrainingProgress.Registros == visto) return;
            visto = TrainingProgress.Registros;
            texto = Texto(TrainingProgress.UltimaAtividade, TrainingProgress.UltimoGanho);
            if (texto != null) ate = Time.unscaledTime + segundosNaTela;
        }

        void OnGUI()
        {
            if (texto == null || Time.unscaledTime > ate) return;
            if (estilo == null)
            {
                int fonte = Mathf.RoundToInt(Mathf.Max(ControlPreset.DpToPx(14f, Screen.dpi), Screen.height / 36f));
                estilo = UiEstilo.EstiloCaixa(fonte);
            }
            // alto e ao centro, abaixo da faixa de aviso do DialogueHud e do botao do salto: fora do joystick e dos botoes
            GUI.Label(new Rect(Screen.width * 0.25f, Screen.height * 0.2f, Screen.width * 0.5f, Screen.height * 0.12f), texto, estilo);
        }
    }
}
