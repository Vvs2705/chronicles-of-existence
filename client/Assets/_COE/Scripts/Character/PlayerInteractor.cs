using UnityEngine;
using UnityEngine.UI;

namespace COE
{
    /// <summary>Escolhe, a cada frame, o Interactable mais proximo que esteja DENTRO do raio e DENTRO do cone
    /// de visao a frente do personagem, e aciona na tecla de interacao do PlayerInputReader (T002).
    ///
    /// CONTRATO. Alvo: null quando nao ha nada valido. Prompt: texto do alvo ou vazio. Interagir(): aciona o
    /// alvo atual uma vez; sem alvo nao faz nada. Distancia e angulo sao PLANARES (ignoram altura).</summary>
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] PlayerInputReader input;
        [Tooltip("Alcance planar da interacao, em metros.")]
        [SerializeField] float raio = 2.5f;
        [Tooltip("Abertura TOTAL do cone a frente, em graus.")]
        [SerializeField] float anguloVisaoGraus = 120f;

        public Interactable Alvo { get; private set; }
        public string Prompt { get { return Alvo != null ? Alvo.Prompt : string.Empty; } }

        void Update()
        {
            Alvo = Escolher();
            if (input != null && input.InteractPressed) Interagir();
        }

        public void Interagir()
        {
            if (Alvo != null) Alvo.Interact(gameObject);
        }

        Interactable Escolher()
        {
            Vector3 frente = transform.forward;
            frente.y = 0f;
            if (frente.sqrMagnitude < 1e-6f) return null; // olhando reto para cima/baixo: sem frente planar
            frente.Normalize();

            float meioCone = anguloVisaoGraus * 0.5f;
            float melhorSq = raio * raio;
            Interactable escolhido = null;

            System.Collections.Generic.IList<Interactable> lista = Interactable.Ativos;
            for (int i = 0; i < lista.Count; i++)
            {
                Interactable it = lista[i];
                if (it == null || !it.Acionavel) continue;
                Vector3 d = it.transform.position - transform.position;
                d.y = 0f;
                float sq = d.sqrMagnitude;
                if (sq > melhorSq) continue;
                if (sq > 1e-6f && Vector3.Angle(frente, d) > meioCone) continue; // de costas / fora do campo de visao
                melhorSq = sq;
                escolhido = it;
            }
            return escolhido;
        }

        // uGUI (Bloco D). So o texto do alvo, sem "[E] ": no toque quem aciona e o botao USAR do HUD (ADR-0006). Faixa
        // central estreita (35%-65%), embaixo: fora do cluster de botoes da direita e do joystick da esquerda. Com
        // contorno escuro para ler sobre chao claro. Desligado (conversa, menu, salto) = some junto.
        Canvas canvas;
        Text prompt;
        Interactable mostrado;
        int alturaDisposta;
        HandPreset maoDisposta;

        void LateUpdate()
        {
            bool mostrar = Alvo != null && !UiFundo.HaModal;
            if (canvas == null)
            {
                if (!mostrar) return;
                canvas = Tela.NovoCanvas(transform, "PromptCanvas", Tela.CamadaHud);
                prompt = Tela.Texto(canvas.transform, "Prompt", 18, TextAnchor.MiddleCenter, Color.white);
                prompt.fontStyle = FontStyle.Bold;
                prompt.resizeTextForBestFit = true;   // nome longo no vao estreito (tela baixa) encolhe, nao invade os botoes
                prompt.resizeTextMinSize = 10;
                Outline o = prompt.gameObject.AddComponent<Outline>();
                o.effectColor = new Color(0f, 0f, 0f, 0.75f);
            }
            if (canvas.enabled != mostrar) canvas.enabled = mostrar;
            if (!mostrar) return;
            if (Alvo != mostrado) { mostrado = Alvo; prompt.text = Alvo.Prompt; }
            ControlPreset p = input != null ? input.Preset : null;
            if (Screen.height == alturaDisposta && (p == null || p.hand == maoDisposta)) return;
            alturaDisposta = Screen.height;
            int fonte = HudLayout.FontePrompt(Screen.height);   // proporcional a tela (18 px some no celular)
            prompt.fontSize = fonte;
            prompt.resizeTextMaxSize = fonte;
            if (p != null)
            {
                maoDisposta = p.hand;
                Tela.Colocar(prompt.rectTransform, HudLayout.Prompt(Screen.safeArea, Tela.Dpi, p, fonte));   // vao entre joystick e botoes
            }
            else Tela.Colocar(prompt.rectTransform, new Rect(Screen.width * 0.35f, fonte * 3.2f, Screen.width * 0.3f, fonte * 1.6f));
        }

        void OnDisable() { if (canvas != null) canvas.enabled = false; }
    }
}
