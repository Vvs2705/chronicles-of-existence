using System;
using UnityEngine;

namespace COE
{
    /// <summary>Ponte opcional para o Animator do personagem (parametros em AnimParams). Sem Animator nos filhos = no-op.
    /// Golpes: `Attack/Skill(index, onHit)` disparam o trigger e entregam o dano no AnimationEvent OnHitFrame;
    /// com `immediate` (padrao da PoC sem animacao) ou sem Animator, o dano e aplicado na hora.
    /// `immediate` e por instancia: jogador e cada inimigo tem o proprio CharacterAnimator.</summary>
    public class CharacterAnimator : MonoBehaviour
    {
        [Tooltip("true: o golpe aplica dano imediatamente, sem esperar OnHitFrame do clip.")]
        public bool immediate = true;

        [Tooltip("Se o OnHitFrame nao chegar (clip sem evento, ataque interrompido), o golpe sai depois deste tempo (s).")]
        [SerializeField] float hitFallbackSeconds = PendingHit.DefaultTimeout;

        Animator anim;
        readonly PendingHit pending = new PendingHit();

        public bool HasAnimator { get { return anim != null; } }

        void Awake()
        {
            anim = GetComponentInChildren<Animator>();
            // AnimationEvent so chega a scripts no GameObject do Animator: modelo filho ganha o relay sozinho.
            if (anim != null && anim.gameObject != gameObject && anim.GetComponent<AnimEventRelay>() == null)
                anim.gameObject.AddComponent<AnimEventRelay>().target = this;
        }

        void Update() { pending.Tick(Time.deltaTime); }

        public void SetSpeed(float speed01) { if (anim != null) anim.SetFloat(AnimParams.Speed, Mathf.Clamp01(speed01)); }
        public void Attack(int index, Action onHit) { Trigger(AnimParams.Attack, AnimParams.AttackIndex, index); Schedule(onHit); }
        public void Skill(int index, Action onHit) { Trigger(AnimParams.Skill, AnimParams.SkillIndex, index); Schedule(onHit); }
        public void Dodge() { if (anim != null) anim.SetTrigger(AnimParams.Dodge); }
        public void Hit() { if (anim != null) anim.SetTrigger(AnimParams.Hit); }
        public void Telegraph() { if (anim != null) anim.SetTrigger(AnimParams.Telegraph); }
        public void Dead(bool value) { if (value) pending.Clear(); if (anim != null) anim.SetBool(AnimParams.Dead, value); } // morto nao entrega golpe no ar
        public void Stagger(bool value) { if (anim != null) anim.SetBool(AnimParams.Stagger, value); }

        /// <summary>AnimationEvent do clip (AnimParams.EventHitFrame): repassa o impacto a quem atacou.</summary>
        public void OnHitFrame() { pending.Fire(); }

        // ponytail: no-op. A esquiva nao desloca (so i-frames de CombatMoves.IFramesEsquiva; o clip roda no lugar) e nada espera
        // o fim do clip: um OnDodgeEnd atrasado da esquiva anterior cortaria a seguinte. Ligar quando a esquiva ganhar
        // deslocamento ou estado proprio (docs/tech/DIVIDA_TECNICA.md).
        public void OnDodgeEnd() { }
        public void OnFootstep() { }  // AnimationEvent; som depois

        void Trigger(string trigger, string indexParam, int index)
        {
            if (anim == null) return;
            anim.SetInteger(indexParam, index);
            anim.SetTrigger(trigger);
        }

        void Schedule(Action onHit)
        {
            if (onHit == null) return;
            if (immediate || anim == null) onHit(); else pending.Schedule(onHit, hitFallbackSeconds);
        }
    }
}
