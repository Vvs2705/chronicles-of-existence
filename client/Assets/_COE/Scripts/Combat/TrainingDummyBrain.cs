namespace COE
{
    public enum DummyFase
    {
        Ocioso = 0,        // parado, guarda BAIXA: acertar aqui nao e pratica
        Telegrafico = 1,   // anuncia o golpe; guarda ativa
        Golpe = 2,         // o golpe sai na ENTRADA desta fase; guarda ativa
        Recuperacao = 3,   // abertura: da para revidar, mas a guarda esta baixa
    }

    /// <summary>Cerebro do oponente de treino. C# PURO e DETERMINISTICO: mesmo dt, mesma sequencia, sempre.
    /// Zero aleatoriedade e zero LLM na decisao de golpe (dossie §F: "IA de inimigo deterministica; nao LLM
    /// para escolher cada golpe"; principio operacional "deterministic runtime behavior for time-sensitive combat").
    ///
    /// CICLO FIXO: Ocioso -> Telegrafico -> Golpe -> Recuperacao -> Ocioso. As duas variantes de golpe se
    /// alternam por paridade do ciclo (alto, lateral, alto, lateral...): o aluno APRENDE o padrao, que e o
    /// ponto de um treino supervisionado. Nao ha "golpe surpresa" nem dificuldade escondida.
    ///
    /// ponytail: uma maquina de estados de quatro fases com duracoes fixas. Teto conhecido: nao reage ao que o
    /// jogador faz (nao pune turtle, nao persegue). Caminho de upgrade quando houver inimigo de verdade:
    /// trocar Avancar() por uma arvore de comportamento — o resto do arquivo (fases, Guardando, Ciclo) continua valendo.</summary>
    public class TrainingDummyBrain
    {
        // Hipotese v0. Ciclo inteiro = 3,6 s: lento o bastante para uma crianca ler o telegrafico.
        public const float OciosoPadrao = 1.6f;
        public const float TelegraficoPadrao = 0.8f;
        public const float GolpePadrao = 0.2f;
        public const float RecuperacaoPadrao = 1f;

        readonly float ocioso, telegrafico, golpe, recuperacao;
        float t;

        public DummyFase Fase { get; private set; }
        /// <summary>Ciclos completos desde o inicio. Nunca diminui.</summary>
        public int Ciclo { get; private set; }
        /// <summary>0 = golpe alto, 1 = golpe lateral. Alterna por ciclo, sem sorteio.</summary>
        public int Variante { get { return Ciclo % 2; } }
        /// <summary>Guarda ativa: acertar o oponente AGORA e pratica significativa (TrainingLedger).</summary>
        public bool Guardando { get { return Fase == DummyFase.Telegrafico || Fase == DummyFase.Golpe; } }

        public TrainingDummyBrain(float ocioso = OciosoPadrao, float telegrafico = TelegraficoPadrao,
                                  float golpe = GolpePadrao, float recuperacao = RecuperacaoPadrao)
        {
            this.ocioso = ocioso; this.telegrafico = telegrafico;
            this.golpe = golpe; this.recuperacao = recuperacao;
        }

        /// <summary>Avanca o tempo. Devolve true no passo em que o golpe SAI (entrada da fase Golpe),
        /// uma vez por ciclo. dt grande nao pula golpe: o laco consome fase a fase.</summary>
        public bool Tick(float dt)
        {
            bool saiu = false;
            if (dt <= 0f) return false;
            t += dt;
            int guarda = 0;
            while (t >= Duracao(Fase) && guarda++ < 16)
            {
                t -= Duracao(Fase);
                if (Avancar()) saiu = true;
            }
            return saiu;
        }

        public void Reiniciar() { Fase = DummyFase.Ocioso; t = 0f; Ciclo = 0; }

        /// <summary>true se esta transicao e a que solta o golpe.</summary>
        bool Avancar()
        {
            switch (Fase)
            {
                case DummyFase.Ocioso: Fase = DummyFase.Telegrafico; return false;
                case DummyFase.Telegrafico: Fase = DummyFase.Golpe; return true;
                case DummyFase.Golpe: Fase = DummyFase.Recuperacao; return false;
                default: Fase = DummyFase.Ocioso; Ciclo++; return false;
            }
        }

        float Duracao(DummyFase f)
        {
            switch (f)
            {
                case DummyFase.Telegrafico: return telegrafico;
                case DummyFase.Golpe: return golpe;
                case DummyFase.Recuperacao: return recuperacao;
                default: return ocioso;
            }
        }
    }
}
