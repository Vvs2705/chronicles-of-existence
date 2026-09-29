using UnityEngine;

namespace COE
{
    public enum Side { Player, Hostile, Neutral }

    /// <summary>Lado do ator. O Hitbox consulta para nao acertar quem esta do mesmo lado (fogo amigo).
    /// Sem o componente o ator e Neutral: golpe de Neutral acerta qualquer um e qualquer um acerta Neutral.
    /// ponytail: o lado e declarado neste componente, nao deduzido pelo tipo do controller.
    /// Nao e sistema de faccao/reputacao: reputacao por NPC/comunidade e T010 e vive em outro lugar.</summary>
    public class Faction : MonoBehaviour
    {
        public Side side = Side.Neutral;

        /// <summary>Lado do ator que contem este objeto (procura no pai). Sem componente = Neutral.</summary>
        public static Side Of(GameObject go)
        {
            Faction f = go.GetComponentInParent<Faction>();
            return f != null ? f.side : Side.Neutral;
        }

        /// <summary>true se um golpe de `attacker` pode acertar `target`. Mesmo lado (nao-Neutral) = nao.</summary>
        public static bool CanHit(Side attacker, Side target)
        {
            return attacker == Side.Neutral || target == Side.Neutral || attacker != target;
        }
    }
}
