using UnityEngine;

namespace COE
{
    /// <summary>Numeros de dano flutuantes via OnGUI (0,6 s, sobem e somem). Um objeto na cena, criado pelo gerador
    /// (BootstrapSceneBuilder), que liga a camera aqui e este componente no HitFlash de cada ator: nada de
    /// Camera.main nem singleton. max. 16 simultaneos (ring buffer).
    /// ponytail: OnGUI/IMGUI; trocar por TextMesh quando houver UI de mundo.</summary>
    public class DamagePopup : MonoBehaviour
    {
        struct Entry { public Vector3 pos; public float amount; public float born; }

        const int Max = 16;
        const float Life = 0.6f;
        const float RisePx = 60f;

        /// <summary>Reducao de movimento: o numero fica parado e so esmaece. ponytail: a HUD de opcoes do COE ainda nao
        /// existe; quando existir, ela escreve aqui.</summary>
        public static bool ReduceMotion;

        [SerializeField] Camera cam; // ligada pelo gerador de cena; nula = nao desenha
        readonly Entry[] items = new Entry[Max];
        int next;
        GUIStyle style;

        public void Show(Vector3 worldPos, float amount)
        {
            items[next] = new Entry { pos = worldPos, amount = amount, born = Time.time };
            next = (next + 1) % Max;
        }

        void OnGUI()
        {
            if (cam == null || UiFundo.HaModal) return;   // numero de dano nao vaza por cima de painel (gancho, menu)
            if (style == null)
            {
                style = new GUIStyle(GUI.skin.label);
                style.fontSize = Mathf.Max(18, Screen.height / 30);
                style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.MiddleCenter;
            }
            float now = Time.time;
            for (int i = 0; i < Max; i++)
            {
                float age = now - items[i].born;
                if (items[i].born <= 0f || age > Life) continue;
                Vector3 sp = cam.WorldToScreenPoint(items[i].pos);
                if (sp.z < 0f) continue;
                float t = age / Life;
                float rise = ReduceMotion ? 0f : RisePx * t;
                style.normal.textColor = new Color(1f, 1f, 1f, 1f - t * t);
                GUI.Label(new Rect(sp.x - 60f, Screen.height - sp.y - 24f - rise, 120f, 48f), Mathf.RoundToInt(items[i].amount).ToString(), style);
            }
        }
    }
}
