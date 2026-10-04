using UnityEngine;
using UnityEngine.UI;

namespace COE
{
    /// <summary>Numeros de dano flutuantes em uGUI (0,6 s, sobem e somem). Um objeto na cena, criado pelo gerador
    /// (BootstrapSceneBuilder), que liga a camera aqui e este componente no HitFlash de cada ator: nada de
    /// Camera.main nem singleton. max. 16 simultaneos (ring buffer).
    /// ponytail: texto de tela que segue o ponto do mundo; UI de mundo (TextMesh) se a arte pedir.</summary>
    public class DamagePopup : MonoBehaviour
    {
        struct Entry { public Vector3 pos; public float amount; public float born; public string texto; }

        const int Max = 16;
        const float Life = 0.6f;
        const float RisePx = 60f;

        /// <summary>Reducao de movimento: o numero fica parado e so esmaece. ponytail: a HUD de opcoes do COE ainda nao
        /// existe; quando existir, ela escreve aqui.</summary>
        public static bool ReduceMotion;

        [SerializeField] Camera cam; // ligada pelo gerador de cena; nula = nao desenha
        readonly Entry[] items = new Entry[Max];
        int next;

        public void Show(Vector3 worldPos, float amount)
        {
            items[next] = new Entry { pos = worldPos, amount = amount, born = Time.time, texto = Mathf.RoundToInt(amount).ToString() };
            next = (next + 1) % Max;
        }

        // uGUI (Bloco D): 16 textos criados uma vez e reaproveitados; o numero vira texto no Show, nao por quadro.
        Canvas canvas;
        readonly Text[] textos = new Text[Max];

        void LateUpdate()
        {
            if (cam == null || UiFundo.HaModal)   // numero de dano nao vaza por cima de painel (gancho, menu)
            {
                if (canvas != null && canvas.enabled) canvas.enabled = false;
                return;
            }
            float now = Time.time;
            bool algum = false;
            for (int i = 0; i < Max; i++) algum |= items[i].born > 0f && now - items[i].born <= Life;
            if (canvas == null)
            {
                if (!algum) return;
                canvas = Tela.NovoCanvas(transform, "DanoCanvas", Tela.CamadaHud);
                for (int i = 0; i < Max; i++)
                {
                    textos[i] = Tela.Texto(canvas.transform, "Dano" + i, 18, TextAnchor.MiddleCenter, Color.white);
                    textos[i].fontStyle = FontStyle.Bold;
                    textos[i].horizontalOverflow = HorizontalWrapMode.Overflow;
                    textos[i].gameObject.AddComponent<Outline>().effectColor = new Color(0f, 0f, 0f, 0.6f);
                }
            }
            if (canvas.enabled != algum) canvas.enabled = algum;
            if (!algum) return;
            int fonte = HudLayout.FontePrompt(Screen.height);
            for (int i = 0; i < Max; i++)
            {
                Text t = textos[i];
                float age = now - items[i].born;
                Vector3 sp = items[i].born > 0f && age <= Life ? cam.WorldToScreenPoint(items[i].pos) : Vector3.back;
                bool vivo = sp.z >= 0f;
                if (t.gameObject.activeSelf != vivo) t.gameObject.SetActive(vivo);
                if (!vivo) continue;
                float k = age / Life;
                float rise = ReduceMotion ? 0f : RisePx * k;
                if (t.text != items[i].texto) t.text = items[i].texto;
                t.fontSize = fonte;
                t.color = new Color(1f, 1f, 1f, 1f - k * k);
                Tela.Colocar(t.rectTransform, new Rect(sp.x - 60f, sp.y - 24f + rise, 120f, 48f));
            }
        }
    }
}
