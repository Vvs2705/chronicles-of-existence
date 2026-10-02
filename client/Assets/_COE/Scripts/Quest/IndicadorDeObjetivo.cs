using UnityEngine;

namespace COE
{
    /// <summary>[PROPOSTA] Para onde ir (simulacao -roteiro, 2026-10-02: a HUD dizia "Levar o recado de Daren a Oren" e
    /// nada mostrava onde o Oren estava; para uma crianca de 5 anos jogando, a vila inteira vira procura).
    /// Alvo na tela: um "▼" que balanca sobre a cabeca. Fora da tela ou atras: uma seta na borda apontando para ele.
    /// Perto (o PlayerInteractor ja mostra o nome) ou com painel aberto: some. So a historia principal
    /// (RumoDaMissao com ignorarOpcionais): opcional se descobre conversando.
    /// ponytail: IMGUI (padrao das HUDs do prototipo), alvo reavaliado 4x/s. A UI de verdade e a T013.</summary>
    public class IndicadorDeObjetivo : MonoBehaviour
    {
        [SerializeField] Camera cam;
        [SerializeField] Transform player;
        [Tooltip("Mais perto que isto o indicador some: o nome do NPC ja aparece.")]
        [SerializeField] float perto = 3f;

        const float Intervalo = 0.25f;
        static readonly Color Cor = new Color(1f, 0.82f, 0.3f);   // ambar dos marcadores de missao

        Transform alvo;
        float proxima;
        GUIStyle estilo;

        /// <summary>O que a seta aponta agora (null = nada). Publico para teste.</summary>
        public Transform Alvo { get { return alvo; } }

        public void Atualizar()
        {
            proxima = Time.unscaledTime + Intervalo;
            string motivo;
            alvo = SaveState.Sessao != null ? RumoDaMissao.Alvo(SaveState.Sessao, true, out motivo) : null;
        }

        void Update()
        {
            if (Time.unscaledTime >= proxima) Atualizar();
        }

        void OnGUI()
        {
            if (Event.current.type != EventType.Repaint || alvo == null || cam == null || UiFundo.HaModal) return;
            if (player != null)
            {
                Vector3 d = alvo.position - player.position;
                d.y = 0f;
                if (d.sqrMagnitude < perto * perto) return;
            }
            if (estilo == null)
                estilo = new GUIStyle(GUI.skin.label)
                {
                    fontSize = Mathf.RoundToInt(Screen.height / 14f),
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = Cor },
                };

            Vector3 topo = alvo.position + Vector3.up * 2.0f;   // logo acima da cabeca de adulto (1,75 m)
            Vector3 v = cam.WorldToScreenPoint(topo);
            float tam = estilo.fontSize * 1.4f, margem = tam;
            bool naTela = v.z > 0f && v.x > margem && v.x < Screen.width - margem && v.y > margem && v.y < Screen.height - margem;
            if (naTela)
            {
                float balanco = Mathf.Sin(Time.unscaledTime * 4f) * tam * 0.12f;
                GUI.Label(new Rect(v.x - tam * 0.5f, Screen.height - v.y - tam + balanco, tam, tam), "▼", estilo);
                return;
            }

            // Fora da tela: direcao do alvo em relacao ao centro, presa na borda. Atras da camera o ponto vem espelhado.
            Vector2 centro = new Vector2(Screen.width, Screen.height) * 0.5f;
            Vector2 dir = new Vector2(v.x, v.y) - centro;
            if (v.z < 0f) dir = -dir;
            if (dir.sqrMagnitude < 1f) dir = Vector2.down;
            dir.Normalize();
            float k = Mathf.Min((centro.x - margem) / Mathf.Max(Mathf.Abs(dir.x), 1e-4f),
                                (centro.y - margem) / Mathf.Max(Mathf.Abs(dir.y), 1e-4f));
            Vector2 p = centro + dir * k;
            Vector2 gui = new Vector2(p.x, Screen.height - p.y);
            float graus = Mathf.Atan2(-dir.y, dir.x) * Mathf.Rad2Deg + 90f;   // "▲" aponta para cima no GUI
            Matrix4x4 antes = GUI.matrix;
            GUIUtility.RotateAroundPivot(graus, gui);
            GUI.Label(new Rect(gui.x - tam * 0.5f, gui.y - tam * 0.5f, tam, tam), "▲", estilo);
            GUI.matrix = antes;
        }
    }
}
