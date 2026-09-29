namespace COE
{
    /// <summary>Formula de dano. C# puro: testavel sem Unity.</summary>
    public static class Damage
    {
        /// <summary>dano = base * 100 / (100 + defesa), minimo 1.</summary>
        public static float Compute(float baseDamage, float defense)
        {
            return System.Math.Max(1f, baseDamage * 100f / (100f + defense));
        }
    }
}
