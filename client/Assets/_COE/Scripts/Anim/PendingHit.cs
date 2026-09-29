using System;

namespace COE
{
    /// <summary>Golpe aguardando o AnimationEvent OnHitFrame. Pura (sem UnityEngine): tempo proprio via Tick(dt).
    /// Se o evento nao chegar (clip sem evento, ataque interrompido por Hit/Dodge), o golpe sai no timeout: dano nunca se perde em silencio.</summary>
    public class PendingHit
    {
        public const float DefaultTimeout = 0.6f;

        Action action;
        float left;

        public bool Pending { get { return action != null; } }

        /// <summary>Agenda o golpe. Se ja havia um pendente, ele dispara antes (nao perde dano). ponytail: um golpe pendente por vez.</summary>
        public void Schedule(Action onHit, float timeout = DefaultTimeout)
        {
            Fire();
            action = onHit;
            left = timeout;
        }

        /// <summary>OnHitFrame (ou timeout): entrega o golpe uma unica vez. Sem pendente = no-op.</summary>
        public void Fire()
        {
            Action a = action;
            action = null; // antes de chamar: o callback pode agendar outro
            if (a != null) a();
        }

        /// <summary>Descarta sem entregar (ex.: morreu com o golpe no ar).</summary>
        public void Clear() { action = null; }

        public void Tick(float dt)
        {
            if (action == null) return;
            left -= dt;
            if (left <= 0f) Fire();
        }
    }
}
