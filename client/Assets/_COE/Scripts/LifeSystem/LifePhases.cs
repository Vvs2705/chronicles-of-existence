namespace COE
{
    /// <summary>As cinco etapas narrativas da vida. GDD v1.2 cap. 03 "Etapas narrativas" e dossie §D.
    /// NAO sao cinco campanhas independentes: sao a mesma vida em faixas de idade.</summary>
    public enum LifePhase
    {
        PrimeirasDescobertas = 0,  // 5-7   familia, vinculos, exploracao, pequenas responsabilidades
        DespertarDosTalentos = 1,  // 8-11  treino supervisionado, estudos e afinidades
        Formacao = 2,              // 12-15 tecnicas e responsabilidades mais complexas
        Independencia = 3,         // 16-18 viagens e especializacoes iniciais
        Legado = 4,                // 19+   exploracao ampla, conflitos e ascensoes
    }

    /// <summary>Faixa de idade -> fase. C# PURO. Os numeros vem do GDD v1.2 cap. 03 (tabela "Etapas
    /// narrativas"), NAO sao hipotese minha.
    ///
    /// PARA QUE SERVE ALEM DE NARRATIVA: a fase e a CHAVE DO TETO ANTI-FARM (Mastery). "Repeticao trivial
    /// tem limite/cap por etapa" (dossie §D) so significa alguma coisa se existir um nome de etapa estavel
    /// para guardar no save — e Id(fase).</summary>
    public static class LifePhases
    {
        /// <summary>Fase da idade em anos. Abaixo de 5 devolve PrimeirasDescobertas: nao existe vida
        /// jogavel antes disso (a partida nasce com ageYears = 5), entao nao ha fase "bebe" a inventar.</summary>
        public static LifePhase De(int idadeAnos)
        {
            if (idadeAnos >= 19) return LifePhase.Legado;
            if (idadeAnos >= 16) return LifePhase.Independencia;
            if (idadeAnos >= 12) return LifePhase.Formacao;
            if (idadeAnos >= 8) return LifePhase.DespertarDosTalentos;
            return LifePhase.PrimeirasDescobertas;
        }

        /// <summary>Id estavel da fase, do jeito que vai para o save (PracticeEntry.phaseId).
        /// snake_case ASCII, como manda o CLAUDE.md. Nao muda depois de gravado.</summary>
        public static string Id(LifePhase fase)
        {
            switch (fase)
            {
                case LifePhase.DespertarDosTalentos: return "talentos";
                case LifePhase.Formacao: return "formacao";
                case LifePhase.Independencia: return "independencia";
                case LifePhase.Legado: return "legado";
                default: return "descobertas";
            }
        }

        /// <summary>Primeira idade da fase. Usado para saber se um salto muda de etapa (a UI avisa isso).</summary>
        public static int IdadeMinima(LifePhase fase)
        {
            switch (fase)
            {
                case LifePhase.DespertarDosTalentos: return 8;
                case LifePhase.Formacao: return 12;
                case LifePhase.Independencia: return 16;
                case LifePhase.Legado: return 19;
                default: return 5;
            }
        }
    }
}
