namespace COE
{
    /// <summary>Em que tela a entrada da partida esta (EntryFlow): fora dela, Nenhuma (o jogo esta no mundo).</summary>
    public enum TelaDeEntrada { Nenhuma = 0, Menu = 1, Limiar = 2 }

    /// <summary>Regra pura de qual musica toca agora e de quanto tempo leva a troca. Prioridade: a entrada (titulo e
    /// nascimento = Menu; falas de Aethron = Limiar) manda sobre tudo; no mundo, combate manda sobre lugar; o bosque manda
    /// sobre a hora; senao, dia ou noite na vila.</summary>
    public static class TrilhaDoMomento
    {
        /// <summary>Quanto tempo depois do ultimo golpe (dado, levado ou bloqueado) a musica de combate ainda segura.</summary>
        public const float SegundosDeCombate = 6f;
        /// <summary>Entrar em combate e rapido; o resto cruza devagar.</summary>
        public const float SegundosDeTrocaRapida = 0.8f;
        public const float SegundosDeTroca = 3f;

        public static TrilhaMusical Escolher(TelaDeEntrada entrada, bool emCombate, bool noBosque, bool noite)
        {
            if (entrada == TelaDeEntrada.Limiar) return TrilhaMusical.Limiar;
            if (entrada == TelaDeEntrada.Menu) return TrilhaMusical.Menu;
            if (emCombate) return TrilhaMusical.Combate;
            if (noBosque) return TrilhaMusical.Bosque;
            return noite ? TrilhaMusical.VilaNoite : TrilhaMusical.VilaDia;
        }

        /// <summary>ultimoGolpe &lt; 0 = nunca houve golpe nesta cena.</summary>
        public static bool EmCombate(float agora, float ultimoGolpe)
        {
            return ultimoGolpe >= 0f && agora - ultimoGolpe < SegundosDeCombate;
        }

        /// <summary>Segundos para cruzar de uma trilha para a outra.</summary>
        public static float Troca(TrilhaMusical para)
        {
            return para == TrilhaMusical.Combate ? SegundosDeTrocaRapida : SegundosDeTroca;
        }
    }
}
