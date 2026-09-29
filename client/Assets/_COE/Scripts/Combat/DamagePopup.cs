using UnityEngine;

namespace COE
{
    /// <summary>Numeros de dano flutuantes via OnGUI (0,6 s, sobem e somem). Um unico objeto criado sob demanda;
    /// max. 16 simultaneos (ring buffer). ponytail: OnGUI/IMGUI; trocar por TextMesh quando houver UI de mundo.</summary>
    public class DamagePopup : MonoBehaviour
    {
        struct Entry { public Vector3 pos; public float amount; public float born; }

        const int Max = 16;
        const float Life = 0.6f;
        const float RisePx = 60f;

        /// <summary>Reducao de movimento: o numero fica parado e so esmaece. ponytail: a HUD de opcoes do COE ainda nao
        /// existe; quando existir, ela escreve aqui.</summary>
        public static bool ReduceMotion;

        static DamagePopup instance;
        readonly Entry[] items = new Entry[Max];
        int next;
        Camera cam;
        GUIStyle style;

        public static void Show(Vector3 worldPos, float amount)
        {
            if (instance == null) instance = new GameObject("DamagePopup").AddComponent<DamagePopup>();
            instance.items[instance.next] = new Entry { pos = worldPos, amount = amount, born = Time.time };
            instance.next = (instance.next + 1) % Max;
        }

        void OnGUI()
        {
            if (cam == null) cam = Camera.main;
            if (cam == null) return;
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
