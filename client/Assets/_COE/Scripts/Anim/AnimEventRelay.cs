using UnityEngine;

namespace COE
{
    /// <summary>O Unity so entrega AnimationEvents a scripts no MESMO GameObject do Animator; o CharacterAnimator fica na raiz.
    /// Este relay repassa ao CharacterAnimator do pai. Adicionado sozinho por CharacterAnimator.Awake: o artista nao poe script no modelo.</summary>
    public class AnimEventRelay : MonoBehaviour
    {
        [HideInInspector] public CharacterAnimator target; // CharacterAnimator.Awake preenche; posto a mao no prefab = acha no pai

        void Awake() { if (target == null) target = GetComponentInParent<CharacterAnimator>(); }

        public void OnHitFrame() { if (target != null) target.OnHitFrame(); }
        public void OnDodgeEnd() { if (target != null) target.OnDodgeEnd(); }
        public void OnFootstep() { if (target != null) target.OnFootstep(); }
    }
}
