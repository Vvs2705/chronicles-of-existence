using UnityEngine;
using UnityEngine.UI;

namespace COE
{
    /// <summary>[PROPOSTA] Para onde ir (simulacao -roteiro, 2026-10-02: a HUD dizia "Levar o recado de Daren a Oren" e
    /// nada mostrava onde o Oren estava; para uma crianca de 5 anos jogando, a vila inteira vira procura).
    /// Alvo na tela: um "▼" que balanca sobre a cabeca. Fora da tela ou atras: uma seta na borda apontando para ele.
    /// Perto (o PlayerInteractor ja mostra o nome) ou com painel aberto: some. So a historia principal
    /// (RumoDaMissao com ignorarOpcionais): opcional se descobre conversando.
    /// uGUI (Bloco D); ponytail: alvo reavaliado 4x/s.</summary>
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

        // uGUI (Bloco D): um texto so ("▼" sobre o alvo, ou "▲" girado na borda), movido por quadro; sem alocacao.
        Canvas canvas;
        Text seta;

        void LateUpdate()
        {
            bool mostrar = alvo != null && cam != null && !UiFundo.HaModal && !Perto();
            if (canvas == null)
            {
                if (!mostrar) return;
                canvas = Tela.NovoCanvas(transform, "IndicadorCanvas", Tela.CamadaHud);
                seta = Tela.Texto(canvas.transform, "Seta", 20, TextAnchor.MiddleCenter, Cor);
                seta.horizontalOverflow = HorizontalWrapMode.Overflow;
                seta.verticalOverflow = VerticalWrapMode.Overflow;
            }
            if (canvas.enabled != mostrar) canvas.enabled = mostrar;
            if (!mostrar) return;

            int fonte = Mathf.RoundToInt(Screen.height / 14f);
            if (seta.fontSize != fonte) seta.fontSize = fonte;
            Vector3 topo = alvo.position + Vector3.up * 2.0f;   // logo acima da cabeca de adulto (1,75 m)
            Vector3 v = cam.WorldToScreenPoint(topo);
            float tam = fonte * 1.4f, margem = tam;
            RectTransform rt = seta.rectTransform;
            bool naTela = v.z > 0f && v.x > margem && v.x < Screen.width - margem && v.y > margem && v.y < Screen.height - margem;
            if (naTela)
            {
                float balanco = Mathf.Sin(Time.unscaledTime * 4f) * tam * 0.12f;
                if (seta.text != "▼") seta.text = "▼";
                rt.localRotation = Quaternion.identity;
                Tela.Colocar(rt, new Rect(v.x - tam * 0.5f, v.y - balanco, tam, tam));
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
            if (seta.text != "▲") seta.text = "▲";
            Tela.Colocar(rt, new Rect(p.x - tam * 0.5f, p.y - tam * 0.5f, tam, tam));
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = p;
            rt.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f);   // "▲" aponta para cima
        }

        bool Perto()
        {
            if (player == null || alvo == null) return false;
            Vector3 d = alvo.position - player.position;
            d.y = 0f;
            return d.sqrMagnitude < perto * perto;
        }
    }
}
