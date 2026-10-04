using UnityEngine;
using UnityEngine.UI;

namespace COE
{
    /// <summary>B15 / R7 — painel de progresso do treino: a cada pratica que chega ao Mastery (PlayerCombat.Registros),
    /// uma linha no alto da tela diz quanto aquele golpe rendeu contra o teto da etapa e, parado no teto, POR QUE parou e
    /// o que ainda rende (outro golpe, crescer). Some depois de alguns segundos. So le; nao muda nada.
    /// ponytail: uma linha so (uGUI). Barra por verbo e a passagem de arte (T013).</summary>
    public class TreinoHud : MonoBehaviour
    {
        [SerializeField] float segundosNaTela = 3f;
        [SerializeField] PlayerCombat combate;   // de quem e o treino (ligado pelo gerador de cena); nulo = painel apagado

        int visto;
        string texto;
        float ate;

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

        void Start()
        {
            StringsLoader.EnsureLoaded();
            if (combate != null) visto = combate.Registros;   // pratica de antes desta cena nao reaparece
        }

        void Update()
        {
            if (combate == null || combate.Registros == visto) return;
            visto = combate.Registros;
            texto = Texto(combate.UltimaAtividade, combate.UltimoGanho);
            if (texto != null) ate = Time.unscaledTime + segundosNaTela;
        }

        // uGUI (Bloco D): alto e ao centro, abaixo da faixa de aviso do DialogueHud e do botao do salto, fora do joystick
        // e dos botoes. Refeito so quando chega pratica nova ou a tela muda.
        Canvas canvas;
        Image caixa;
        Text linha;
        int alturaDisposta;
        Rect safeDisposto;

        void LateUpdate()
        {
            bool mostrar = texto != null && Time.unscaledTime <= ate;
            if (canvas == null)
            {
                if (!mostrar) return;
                canvas = Tela.NovoCanvas(transform, "TreinoCanvas", Tela.CamadaHud);
                caixa = Tela.Imagem(canvas.transform, "Caixa", Tela.SpritePainel, Color.white);
                linha = Tela.Texto(caixa.transform, "Texto", 16, TextAnchor.MiddleCenter, UiEstilo.Tinta);
                linha.resizeTextForBestFit = true;
            }
            if (canvas.enabled != mostrar) canvas.enabled = mostrar;
            if (!mostrar) return;
            if (linha.text != texto) linha.text = texto;
            if (Screen.height == alturaDisposta && Screen.safeArea == safeDisposto) return;
            alturaDisposta = Screen.height;
            safeDisposto = Screen.safeArea;
            int fonte = Tela.Fonte(14f, 1f / 36f);
            Tela.Colocar(caixa.rectTransform, HudLayout.LinhaTreino(Screen.safeArea));
            Tela.Esticar(linha.rectTransform, fonte * 0.5f);
            linha.resizeTextMaxSize = fonte;
            linha.resizeTextMinSize = Mathf.Max(10, fonte / 2);
        }
    }
}
