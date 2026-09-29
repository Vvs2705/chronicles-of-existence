namespace COE
{
    /// <summary>ESTE E O UNICO PONTO DE ACOPLAMENTO DA T011 COM A T009 (Life System). Nenhum outro arquivo de
    /// combate conhece progressao, atributo, afinidade ou save.
    ///
    /// DIVISAO DE TRABALHO COMBINADA: a T011 EMITE "houve uma pratica significativa" (afinidade + qualidade
    /// 0..1); quem decide QUANTO isso vira de atributo/afinidade, e qual e o teto por etapa da vida, e a T009
    /// (LifeState.pratica / Mastery). O combate nao escreve no save.
    ///
    /// COMO A T009 LIGA (uma linha, no bootstrap dela):
    ///     TrainingProgress.Sink = mastery.RegistrarPratica;   // void RegistrarPratica(string afinidade, float qualidade)
    /// Enquanto ela nao liga, Sink e nulo e Registrar() e no-op: o treino roda inteiro, so nao rende dominio.
    ///
    /// IDADE: o treino supervisionado so existe depois do salto para ~8 anos (dossie §F e §L). Quem responde a
    /// idade e o save (SaveData.ageYears, T004) e quem traduz idade->fase e LifePhases (T009). Este arquivo so
    /// pergunta; PodeTreinar nunca fixa "8" no codigo de combate.</summary>
    public static class TrainingProgress
    {
        /// <summary>(afinidade, qualidade 0..1). A T009 assina; nulo = ninguem ouvindo.</summary>
        public static System.Action<string, float> Sink;

        /// <summary>De onde sai a idade. Padrao: o save da partida. Injetavel em teste.</summary>
        public static System.Func<int> IdadeAnos = delegate { return SaveState.Current.ageYears; };

        /// <summary>Fase minima para treinar com espada de madeira e oponente supervisionado.</summary>
        public const LifePhase FaseMinima = LifePhase.DespertarDosTalentos; // 8-11 anos

        /// <summary>false na primeira infancia: aos cinco anos nao ha ataque, bloqueio nem magia
        /// (dossie §F: "evitar combate adulto completo na primeira infancia").</summary>
        public static bool PodeTreinar()
        {
            return LifePhases.De(IdadeAnos()) >= FaseMinima;
        }

        /// <summary>Emite a pratica. Qualidade &lt;= 0 nao emite nada — a decisao de valer zero e do
        /// TrainingLedger (anti-farm), nao da T009.</summary>
        public static void Registrar(string afinidade, float qualidade)
        {
            if (qualidade <= 0f || string.IsNullOrEmpty(afinidade)) return;
            if (Sink != null) Sink(afinidade, qualidade);
        }
    }
}
