using System.Collections.Generic;
using UnityEngine;

namespace COE
{
    /// <summary>Golpe corpo a corpo: OverlapSphere na frente do dono, aplica dano (+postura) em cada Health.</summary>
    public class Hitbox : MonoBehaviour
    {
        public float damage = 10f;
        public float posture = 0f;    // 0 = Health usa dano x 0,5
        public float range = 1.5f;    // distancia do centro da esfera a frente do dono
        public float radius = 1f;
        /// <summary>Altura do centro da esfera acima do pivo do dono (m). 0,9 = legado de adulto; o gerador de cena
        /// seta pela escala do corpo (BodyScale, ~0,55 da altura = meio do tronco).</summary>
        public float altura = 0.9f;
        public LayerMask mask = ~0;

        /// <summary>Alvos distintos atingidos no ultimo Swing.</summary>
        public readonly List<Health> LastHits = new List<Health>();

        readonly Collider[] buffer = new Collider[16]; // sem alocacao por golpe

        /// <summary>Executa o golpe com os campos do componente e devolve quantos alvos distintos foram atingidos.</summary>
        public int Swing() { return Swing(damage, posture, range, radius); }

        /// <summary>Golpe parametrizado (habilidades): esfera a `rng` m a frente com raio `rad`.</summary>
        public int Swing(float dmg, float post, float rng, float rad)
        {
            LastHits.Clear();
            Side meuLado = Faction.Of(gameObject); // fogo amigo: sem isso o mesmo lado se mata sozinho
            Vector3 center = transform.position + transform.forward * rng + Vector3.up * altura;
            int n = Physics.OverlapSphereNonAlloc(center, rad, buffer, mask, QueryTriggerInteraction.Ignore);
            for (int i = 0; i < n; i++)
            {
                Health h = buffer[i].GetComponentInParent<Health>();
                if (h == null || h.transform.root == transform.root || LastHits.Contains(h)) continue; // varios colliders do mesmo alvo
                if (!Faction.CanHit(meuLado, Faction.Of(h.gameObject))) continue;
                if (h.TakeDamage(dmg, post, transform)) LastHits.Add(h);
            }
            return LastHits.Count;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + transform.forward * range + Vector3.up * altura, radius);
        }
    }
}
