using System;
using UnityEngine;

namespace COE
{
    public class Health : MonoBehaviour
    {
        public float max = 100f;
        public float defense = 0f;

        /// <summary>Multiplicador de dano recebido vindo do dono. 1 = neutro.</summary>
        [NonSerialized] public float damageTakenMult = 1f;
        /// <summary>Consumido no proximo TakeDamage. 1 = neutro.</summary>
        [NonSerialized] public float nextHitMult = 1f;

        public float Current { get; private set; }
        /// <summary>Dano bruto (pre-defesa) do ultimo golpe.</summary>
        public float LastRaw { get; private set; }
        public bool Dead { get { return Current <= 0f; } }
        public bool Invulnerable { get { return Time.time < invulnerableUntil; } }

        /// <summary>Callbacks por instancia. Nao e event bus: quem quer ouvir, assina direto.</summary>
        public Action<float> Damaged;          // dano efetivo aplicado
        public Action<float> PostureDamaged;   // dano de postura
        public Action Evaded;                  // golpe ignorado por i-frames (esquiva perfeita)
        /// <summary>(dano bruto, origem) -> dano bruto ajustado; &lt;= 0 ignora o golpe (bloqueio perfeito). Dono do ator assina.</summary>
        public Func<float, Transform, float> DamageFilter;

        float invulnerableUntil;

        void Awake() { Current = max; }

        public void SetInvulnerable(float seconds) { invulnerableUntil = Time.time + seconds; }

        public void Heal(float amount)
        {
            if (Dead || amount <= 0f) return;
            Current = Mathf.Min(max, Current + amount);
        }

        /// <summary>Dano efetivo = Damage.Compute(raw * multiplicadores, defesa), minimo 1. Retorna false se ignorado
        /// (i-frames ou morto). posture &lt;= 0 usa a regra provisoria dano_final x 0,5.
        /// ponytail: o COE ainda nao tem sistema de status.
        /// Quando houver, entra aqui como mais um fator de damageTakenMult, nao como caso especial.</summary>
        public bool TakeDamage(float rawDamage, float posture = 0f, Transform source = null, bool ignoreDefense = false)
        {
            if (Dead) return false;
            if (Invulnerable) { if (Evaded != null) Evaded(); return false; }
            if (DamageFilter != null) { rawDamage = DamageFilter(rawDamage, source); if (rawDamage <= 0f) return false; }
            float mult = damageTakenMult * nextHitMult;
            nextHitMult = 1f;
            LastRaw = rawDamage;
            float dmg = Damage.Compute(rawDamage * mult, ignoreDefense ? 0f : defense);
            Current = Mathf.Max(0f, Current - dmg);
            if (Damaged != null) Damaged(dmg);
            TakePosture(posture > 0f ? posture : dmg * 0.5f);
            return true;
        }

        public void TakePosture(float posture)
        {
            if (posture <= 0f || PostureDamaged == null) return;
            PostureDamaged(posture);
        }
    }
}
