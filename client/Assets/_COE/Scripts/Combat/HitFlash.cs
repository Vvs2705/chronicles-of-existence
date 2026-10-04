using UnityEngine;

namespace COE
{
    /// <summary>Feedback de acerto generico: ao Health.Damaged pinta o Renderer de branco por `seconds` via
    /// MaterialPropertyBlock (_BaseColor, URP/Lit) e mostra o numero (DamagePopup). Quem telegrafa usa `Hold`:
    /// a cor mantida tem prioridade e e restaurada no fim do flash em vez da original.</summary>
    public class HitFlash : MonoBehaviour
    {
        [SerializeField] Renderer body;              // vazio = primeiro Renderer nos filhos
        [SerializeField] Color flashColor = Color.white;
        [SerializeField] float seconds = 0.08f;
        [SerializeField] DamagePopup numeros;        // ligado pelo gerador de cena; vazio = sem numero flutuante
        [SerializeField] SomDoJogo som;              // ligado pelo gerador; vazio = golpe mudo

        static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        Health health;
        MaterialPropertyBlock block;
        Color? hold;
        float until;

        /// <summary>Cor com prioridade sobre o flash (telegrafo). null = cor original do material.</summary>
        public Color? Hold
        {
            get { return hold; }
            set { hold = value; if (until <= 0f) Restore(); }
        }

        void Awake()
        {
            health = GetComponent<Health>();
            block = new MaterialPropertyBlock();
            if (body == null) body = GetComponentInChildren<Renderer>();
            if (health != null) health.Damaged += OnDamaged;
        }

        void OnDestroy() { if (health != null) health.Damaged -= OnDamaged; }

        void OnDamaged(float amount)
        {
            until = Time.time + seconds;
            Set(flashColor);
            if (som != null) som.Tocar(Som.Golpe);
            if (numeros != null) numeros.Show(Topo(), amount);
        }

        // Numero logo acima da cabeca visivel: o pivo varia (parceiro no centro da capsula, jogador nos pes), entao
        // 1,8 m fixo do pivo jogava o numero do adulto para fora do quadro da camera de crianca.
        // ponytail: sem ponto exato do acerto; 1,8 m do pivo so como reserva sem Renderer ligado.
        Vector3 Topo()
        {
            if (body == null || !body.enabled) return transform.position + Vector3.up * 1.8f;
            Bounds b = body.bounds;
            return new Vector3(b.center.x, b.max.y + 0.2f, b.center.z);
        }

        void Update()
        {
            if (until > 0f && Time.time >= until) { until = 0f; Restore(); }
        }

        /// <summary>Sem cor mantida o bloco sai: com MaterialPropertyBlock o Renderer deixa o SRP Batcher, e o corpo do jogador
        /// e o do parceiro ficavam fora do lote para sempre depois do primeiro golpe (auditoria §27).</summary>
        void Restore()
        {
            if (hold.HasValue) Set(hold.Value);
            else if (body != null) body.SetPropertyBlock(null);
        }

        void Set(Color c)
        {
            if (body == null) return;
            block.SetColor(BaseColorId, c);
            body.SetPropertyBlock(block);
        }
    }
}
