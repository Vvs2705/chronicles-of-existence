namespace COE
{
    /// <summary>ESTE E O UNICO PONTO DE ACOPLAMENTO DA T011 COM A T009 (Life System). Nenhum outro arquivo de
    /// combate conhece progressao, atributo, afinidade ou save.
    ///
    /// DIVISAO DE TRABALHO COMBINADA: a T011 EMITE "houve uma pratica significativa" (atividade + qualidade
    /// 0..1); quem decide QUANTO isso vira de atributo/afinidade, e qual e o teto por etapa da vida, e a T009
    /// (Mastery.Praticar -> LifeState.pratica). O combate nao escreve no save: quem escreve e o Mastery.
    ///
    /// LIGACAO (Bloco C, 2026-10-04): a sessao da partida chega por PARAMETRO (Registrar) e quem escreve e a transicao
    /// GameSession.Praticar, que grava. Antes eram delegates static (Sink, IdadeAnos) lendo o SaveState.Current: o treino
    /// escrevia atributo e afinidade sem sessao e sem gravar.
    ///
    /// ATIVIDADES: um id por verbo do treino, cada um em UMA trilha so (ids estaveis, viram chave no save).
    /// Por verbo, e nao "treino_marcial" inteiro, porque o B15 pede o ganho estacionando "apos N repeticoes do
    /// mesmo golpe": cada verbo satura no proprio teto por etapa (Mastery.TetoAtividadePorFase) e a trilha
    /// inteira no teto dela. A espada de Borin (B15) nao entra aqui: objeto e fala diferentes, mesma atividade.
    ///
    /// IDADE: o treino supervisionado so existe depois do salto para ~8 anos (dossie §F e §L). A idade vem de quem
    /// chama (o save da sessao, SaveData.ageYears) e quem traduz idade->fase e LifePhases (T009). PodeTreinar nunca
    /// fixa "8" no codigo de combate.</summary>
    public static class TrainingProgress
    {
        /// <summary>Desafio (1..5) do treino supervisionado com espada de madeira. HIPOTESE v0: acima de 2 na
        /// trilha, o treino vira trivial para o Mastery (teto menor), que e o que se espera de treino de crianca.</summary>
        public const int DesafioTreinoV0 = 2;

        public static readonly AtividadeDef AtividadeLeve =
            new AtividadeDef("treino_ataque_leve", CombatMoves.Leve.Afinidade, DesafioTreinoV0);
        public static readonly AtividadeDef AtividadeForte =
            new AtividadeDef("treino_ataque_forte", CombatMoves.Forte.Afinidade, DesafioTreinoV0);
        public static readonly AtividadeDef AtividadeBloqueio =
            new AtividadeDef("treino_bloqueio", CombatMoves.AfinidadeMarcial, DesafioTreinoV0);
        public static readonly AtividadeDef AtividadeEsquiva =
            new AtividadeDef("treino_esquiva", CombatMoves.Esquiva.Afinidade, DesafioTreinoV0);
        public static readonly AtividadeDef AtividadeMagia =
            new AtividadeDef("treino_fagulha_inicial", CombatMoves.Magia.Afinidade, DesafioTreinoV0);

        /// <summary>Todas as atividades do treino (teste de contrato de ids).</summary>
        public static readonly AtividadeDef[] Atividades =
        {
            AtividadeLeve, AtividadeForte, AtividadeBloqueio, AtividadeEsquiva, AtividadeMagia,
        };

        /// <summary>Fase minima para treinar com espada de madeira e oponente supervisionado.</summary>
        public const LifePhase FaseMinima = LifePhase.DespertarDosTalentos; // 8-11 anos

        /// <summary>false na primeira infancia: aos cinco anos nao ha ataque, bloqueio nem magia
        /// (dossie §F: "evitar combate adulto completo na primeira infancia").</summary>
        public static bool PodeTreinar(int idadeAnos)
        {
            return LifePhases.De(idadeAnos) >= FaseMinima;
        }

        /// <summary>B15 concluido: cada um dos quatro verbos do treino praticado ao menos uma vez (ataque leve, ataque forte,
        /// defesa OU esquiva, magia), lido da pratica que o Mastery grava no save. Repetir um verbo so nao completa.</summary>
        public static bool TreinoSupervisionadoFeito(SaveData save)
        {
            return Praticou(save, AtividadeLeve) && Praticou(save, AtividadeForte)
                && (Praticou(save, AtividadeBloqueio) || Praticou(save, AtividadeEsquiva)) && Praticou(save, AtividadeMagia);
        }

        static bool Praticou(SaveData save, AtividadeDef atividade)
        {
            if (save == null || save.life == null || save.life.pratica == null) return false;
            foreach (PracticeEntry p in save.life.pratica)
                if (p.activityId == atividade.Id && p.vezes > 0) return true;
            return false;
        }

        /// <summary>Emite a pratica pela sessao, que escreve no save e grava (GameSession.Praticar). Qualidade &lt;= 0 nao
        /// emite nada — a decisao de valer zero e do TrainingLedger (anti-farm), nao da T009.
        /// ponytail: a qualidade nao escala o ganho. O Mastery conta EXECUCAO significativa (retorno decrescente + teto por
        /// etapa); peso por qualidade, se o playtest pedir, entra aqui sem mudar o Mastery.</summary>
        public static GanhoResultado Registrar(GameSession sessao, AtividadeDef atividade, float qualidade)
        {
            if (sessao == null || atividade == null || qualidade <= 0f) return default(GanhoResultado);
            return sessao.Praticar(atividade);
        }
    }
}
