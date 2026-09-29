namespace COE
{
    /// <summary>Indice do golpe do combo do basico (AnimParams.AttackIndex): 0 -> 1 -> 2 -> 0. Pura (sem UnityEngine).
    /// So escolhe a animacao (Attack1/2/3); dano NAO muda por indice.</summary>
    public class ComboCounter
    {
        public const int Length = 3;
        public const float DefaultResetWindow = 1.2f;

        readonly float resetWindow;
        int next;

        public ComboCounter(float resetWindow = DefaultResetWindow) { this.resetWindow = resetWindow; }

        /// <summary>Indice do golpe de agora. `sinceLast` = segundos desde o golpe anterior; passou da janela = volta ao 0.</summary>
        public int Next(float sinceLast)
        {
            if (sinceLast > resetWindow) next = 0;
            int index = next;
            next = (next + 1) % Length;
            return index;
        }
    }
}
