namespace COE
{
    /// <summary>Os tres periodos do dia cotidiano (dossie §D: "tempo cotidiano possui manha/tarde/noite
    /// e rotinas dos habitantes"). Ordem congelada: e ela que define o ciclo.</summary>
    public enum TimeOfDay
    {
        Manha = 0,
        Tarde = 1,
        Noite = 2,
    }

    /// <summary>Relogio do dia. C# PURO, sem UnityEngine e SEM tempo real.
    ///
    /// POR QUE NAO E RELOGIO DE VERDADE: o jogo avanca por ACAO (praticar, viajar, cumprir missao),
    /// DESCANSO ou MARCO — nunca por segundos passando. Se o periodo andasse sozinho, ficar parado no menu
    /// consumiria a manha do jogador, e a rotina de NPC da T007 dependeria de framerate. Nao existe Update,
    /// nao existe DateTime.Now aqui: quem chama Avancar decide quando o tempo passa.
    ///
    /// QUEM CONSOME: rotina de NPC (T007), disponibilidade de atividade e de missao (T006). Todos leem
    /// Atual(life); ninguem alem deste arquivo escreve life.timeOfDay.</summary>
    public static class TimeOfDayCycle
    {
        public const string IdManha = "manha";
        public const string IdTarde = "tarde";
        public const string IdNoite = "noite";

        /// <summary>Id estavel do periodo, como vai para o JSON do save. Salvamos o ID e nao o numero do
        /// enum de proposito: reordenar o enum um dia nao pode transformar noite em manha em save antigo.</summary>
        public static string Id(TimeOfDay periodo)
        {
            if (periodo == TimeOfDay.Tarde) return IdTarde;
            if (periodo == TimeOfDay.Noite) return IdNoite;
            return IdManha;
        }

        /// <summary>Periodo a partir do id. Id desconhecido ou nulo (save velho, arquivo editado a mao)
        /// cai em Manha: nunca lanca, nunca trava o carregamento.</summary>
        public static TimeOfDay De(string id)
        {
            if (id == IdTarde) return TimeOfDay.Tarde;
            if (id == IdNoite) return TimeOfDay.Noite;
            return TimeOfDay.Manha;
        }

        public static TimeOfDay Proximo(TimeOfDay periodo)
        {
            if (periodo == TimeOfDay.Manha) return TimeOfDay.Tarde;
            if (periodo == TimeOfDay.Tarde) return TimeOfDay.Noite;
            return TimeOfDay.Manha;
        }

        public static TimeOfDay Atual(LifeState life)
        {
            return life == null ? TimeOfDay.Manha : De(life.timeOfDay);
        }

        /// <summary>Avanca UM periodo. Devolve true quando a noite virou manha, ou seja, quando o dia mudou
        /// — e o gancho de "dormiu, acordou" para quem quiser reagir (rotina, autosave, missao diaria).
        /// life nulo e ignorado em silencio (devolve false); este metodo nunca lanca.
        ///
        /// A IDADE NAO MORA AQUI. Andar o dia nao envelhece ninguem: idade so sobe em AgeAdvance.ConfirmarSalto
        /// (dossie §D: "a idade avanca em marcos narrativos anunciados e confirmados, nao acelerando ao
        /// caminhar ou repetir tarefas").</summary>
        public static bool Avancar(LifeState life)
        {
            if (life == null) return false;

            TimeOfDay proximo = Proximo(Atual(life));
            life.timeOfDay = Id(proximo);

            if (proximo != TimeOfDay.Manha) return false;
            life.day++;
            return true;
        }
    }
}
