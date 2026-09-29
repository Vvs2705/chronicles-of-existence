using UnityEngine;

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

        GUIStyle estilo;

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
                if (it == null) continue;
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

        // ponytail: IMGUI provisorio, so para o prompt ser visivel na build de teste do T002. Sai quando o HUD de
        // verdade existir (mesma troca que o PerfHud vai precisar).
        // So o texto do alvo, sem "[E] ": no toque quem aciona e o botao USAR do HUD (ADR-0006), e sem concatenar
        // o OnGUI nao aloca string. Fonte proporcional a tela, como o DamagePopup (18 px some no celular).
        void OnGUI()
        {
            if (Alvo == null) return;
            if (estilo == null)
            {
                estilo = new GUIStyle(GUI.skin.label);
                estilo.alignment = TextAnchor.MiddleCenter;
                estilo.fontSize = Mathf.Max(18, Screen.height / 30);
            }
            float h = estilo.fontSize * 1.6f;
            // Faixa central estreita (35%-65%): fora do cluster de botoes da direita e do joystick da esquerda.
            GUI.Label(new Rect(Screen.width * 0.35f, Screen.height - h * 3f, Screen.width * 0.3f, h), Alvo.Prompt, estilo);
        }
    }
}
