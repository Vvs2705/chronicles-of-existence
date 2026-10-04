using UnityEngine;
using UnityEngine.UI;

namespace COE
{
    /// <summary>GDD v1.2 cap. 05 (vertical slice): as barras de Vida, Vigor e Mana do treino. So aparecem com o treino liberado
    /// (PlayerCombat.PodeTreinar, aos 8) e somem com modal aberto. Cada barra leva o nome escrito: a leitura nao depende so
    /// da cor (Prompt Mestre §9). So le o PlayerCombat (Recursos) e o Health do jogador; nao muda nada.
    /// Desenho refeito so quando a tela muda; o preenchimento, so quando o valor anda.
    /// ponytail: retangulo de cor sobre trilho escuro; a arte de UI (T013) troca o desenho, nao a leitura.</summary>
    public class BarrasHud : MonoBehaviour
    {
        [SerializeField] PlayerCombat combate;   // ligado pelo gerador de cena; nulo = sem barras

        static readonly Color[] Cores = { new Color(0.86f, 0.33f, 0.30f), new Color(0.47f, 0.78f, 0.36f), new Color(0.36f, 0.58f, 0.95f) };

        Health vida;
        Canvas canvas;
        readonly Text[] nomes = new Text[3];
        readonly Image[] trilhos = new Image[3], cheios = new Image[3];
        readonly float[] mostrada = { -1f, -1f, -1f };
        int alturaDisposta;
        Rect safeDisposto;

        /// <summary>Barras na tela agora (teste le).</summary>
        public bool Visivel { get { return canvas != null && canvas.enabled; } }

        /// <summary>Fracao 0..1 desenhada na barra i (0 Vida, 1 Vigor, 2 Mana); -1 antes da primeira (teste le).</summary>
        public float Mostrada(int i) { return mostrada[i]; }

        void Start()
        {
            StringsLoader.EnsureLoaded();
            if (combate != null) vida = combate.GetComponent<Health>();
        }

        void LateUpdate()
        {
            bool mostrar = combate != null && combate.Recursos != null && combate.PodeTreinar && !UiFundo.HaModal;
            if (canvas == null)
            {
                if (!mostrar) return;
                Montar();
            }
            if (canvas.enabled != mostrar) canvas.enabled = mostrar;
            if (!mostrar) return;
            if (Screen.height != alturaDisposta || Screen.safeArea != safeDisposto) Dispor();
            Encher(0, vida != null && vida.max > 0f ? Mathf.Clamp01(vida.Current / vida.max) : 0f);
            Encher(1, Mathf.Clamp01(combate.Recursos.Vigor.Fracao));
            Encher(2, Mathf.Clamp01(combate.Recursos.Mana.Fracao));
        }

        void Montar()
        {
            canvas = Tela.NovoCanvas(transform, "BarrasCanvas", Tela.CamadaHud);
            string[] textos = { Strings.Get("hud.vida"), Strings.Get("hud.vigor"), Strings.Get("hud.mana") };   // literais: a cobertura de textos le
            for (int i = 0; i < 3; i++)
            {
                nomes[i] = Tela.Texto(canvas.transform, "Nome" + i, 14, TextAnchor.MiddleRight, UiEstilo.Tinta);
                nomes[i].text = textos[i];
                nomes[i].resizeTextForBestFit = true;
                trilhos[i] = Tela.Imagem(canvas.transform, "Trilho" + i, null, new Color(0f, 0f, 0f, 0.55f));
                cheios[i] = Tela.Imagem(trilhos[i].transform, "Cheio" + i, null, Cores[i]);
            }
        }

        void Dispor()
        {
            alturaDisposta = Screen.height;
            safeDisposto = Screen.safeArea;
            int fonte = HudLayout.FonteCartao(Screen.height, Tela.Dpi);
            Rect faixa = HudLayout.Barras(safeDisposto, Screen.height, Tela.Alvo, fonte, HudLayout.FontePrompt(Screen.height, Tela.Dpi));
            float largura = faixa.width / 3f, rotulo = largura * 0.38f, vao = fonte * 0.4f;
            for (int i = 0; i < 3; i++)
            {
                float x = faixa.x + i * largura;
                nomes[i].fontSize = fonte;
                nomes[i].resizeTextMaxSize = fonte;
                nomes[i].resizeTextMinSize = Mathf.Max(10, fonte / 2);
                Tela.Colocar(nomes[i].rectTransform, new Rect(x, faixa.y, rotulo - vao * 0.5f, faixa.height));
                Tela.Colocar(trilhos[i].rectTransform, new Rect(x + rotulo, faixa.y + faixa.height * 0.3f, largura - rotulo - vao, faixa.height * 0.4f));
                mostrada[i] = -1f;   // tamanho novo: reenche
            }
        }

        void Encher(int i, float f)
        {
            if (Mathf.Abs(f - mostrada[i]) < 0.002f) return;
            mostrada[i] = f;
            Vector2 trilho = trilhos[i].rectTransform.sizeDelta;
            Tela.Colocar(cheios[i].rectTransform, new Rect(0f, 0f, trilho.x * f, trilho.y));
        }
    }
}
