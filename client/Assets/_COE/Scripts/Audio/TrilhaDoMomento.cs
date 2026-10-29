namespace COE
{
    /// <summary>Em que tela a entrada da partida esta (EntryFlow): fora dela, Nenhuma (o jogo esta no mundo).</summary>
    public enum TelaDeEntrada { Nenhuma = 0, Menu = 1, Limiar = 2 }

    /// <summary>O trecho do episodio 2 que tem musica propria, lido das missoes (DoEpisodio).</summary>
    public enum MomentoDoEpisodio { Nenhum = 0, Festa = 1, Descida = 2, Enredado = 3 }

    /// <summary>Regra pura de qual musica toca agora e de quanto tempo leva a troca. Prioridade: a entrada (titulo e
    /// nascimento = Menu; falas de Aethron = Limiar) manda sobre tudo; no bosque, o Enredado (q16) manda ate sobre o combate
    /// (a luta dele e a musica dele); depois combate; no bosque, a descida (q15) ou o bosque de sempre; na vila, a festa
    /// (q14) ou dia e noite.</summary>
    public static class TrilhaDoMomento
    {
        /// <summary>Quanto tempo depois do ultimo golpe (dado, levado ou bloqueado) a musica de combate ainda segura.</summary>
        public const float SegundosDeCombate = 6f;
        /// <summary>Entrar em combate e rapido; o resto cruza devagar.</summary>
        public const float SegundosDeTrocaRapida = 0.8f;
        public const float SegundosDeTroca = 3f;

        public static TrilhaMusical Escolher(TelaDeEntrada entrada, bool emCombate, bool noBosque, bool noite,
                                             MomentoDoEpisodio episodio = MomentoDoEpisodio.Nenhum)
        {
            if (entrada == TelaDeEntrada.Limiar) return TrilhaMusical.Limiar;
            if (entrada == TelaDeEntrada.Menu) return TrilhaMusical.Menu;
            if (noBosque && episodio == MomentoDoEpisodio.Enredado) return TrilhaMusical.Enredado;
            if (emCombate) return TrilhaMusical.Combate;
            if (noBosque) return episodio == MomentoDoEpisodio.Descida ? TrilhaMusical.Descida : TrilhaMusical.Bosque;
            if (episodio == MomentoDoEpisodio.Festa) return TrilhaMusical.Festa;
            return noite ? TrilhaMusical.VilaNoite : TrilhaMusical.VilaDia;
        }

        public const string MissaoDaFesta = "q14_a_festa_da_colheita", FimDaFesta = "a_danca_da_corda";
        public const string MissaoDaDescida = "q15_a_anomalia_no_bosque", MissaoDoEnredado = "q16_o_enredado";

        /// <summary>O trecho do episodio 2 pelas missoes (ids do QuestCatalogEp2): q16 em andamento = Enredado; q15 em
        /// andamento = Descida; q14 em andamento ate a danca da corda = Festa (depois dela vem a noite do alarme, sem festa).
        /// Sem o ep. 2 liberado nenhuma delas anda, entao e Nenhum.
        /// ponytail: o Enredado toca no bosque inteiro durante a q16 (as ancoras bosque_trilha..bosque_coracao ainda nao
        /// estao na cena); com elas, medir a distancia ao bosque_coracao.</summary>
        public static MomentoDoEpisodio DoEpisodio(QuestSystem missoes)
        {
            if (missoes == null) return MomentoDoEpisodio.Nenhum;
            if (missoes.Estado(MissaoDoEnredado) == QuestStatus.EmAndamento) return MomentoDoEpisodio.Enredado;
            if (missoes.Estado(MissaoDaDescida) == QuestStatus.EmAndamento) return MomentoDoEpisodio.Descida;
            if (missoes.Estado(MissaoDaFesta) == QuestStatus.EmAndamento && !missoes.Feito(MissaoDaFesta, FimDaFesta)) return MomentoDoEpisodio.Festa;
            return MomentoDoEpisodio.Nenhum;
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
