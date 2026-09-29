// CONTRATO de animacao: nomes dos parametros do Animator que o codigo usa e que o
// Animator Controller do personagem humanoide (fase de design) DEVE expor.
namespace COE
{
    public static class AnimParams
    {
        public const string Speed = "Speed";           // float 0..1 (0 parado, 1 corrida)
        public const string Attack = "Attack";         // trigger: ataque basico
        public const string AttackIndex = "AttackIndex"; // int 0..2: qual golpe do combo
        public const string Skill = "Skill";           // trigger: habilidade
        public const string SkillIndex = "SkillIndex"; // int 0..3 (slot de habilidade)
        public const string Dodge = "Dodge";           // trigger
        public const string Hit = "Hit";               // trigger: levou dano
        public const string Dead = "Dead";             // bool
        public const string Stagger = "Stagger";       // bool: postura quebrada
        public const string Telegraph = "Telegraph";   // trigger (inimigos): inicio do telegrafico

        // Eventos de animacao que o clip DEVE disparar (AnimationEvent -> metodo com este nome):
        public const string EventHitFrame = "OnHitFrame";   // momento do impacto do golpe
        public const string EventDodgeEnd = "OnDodgeEnd";
        public const string EventFootstep = "OnFootstep";
    }
}
