namespace COE
{
    /// <summary>Os tres tipos de pratica que o treino reconhece. Nao existe "pratica de errar o golpe":
    /// balançar a espada no ar nao ensina nada.</summary>
    public enum PraticaTipo
    {
        Golpe = 0,
        Bloqueio = 1,
        Esquiva = 2,
    }

    /// <summary>ANTI-FARM DO TREINO (exploit nº 7 do backlog; dossie §F "nao dar XP infinito por atacar alvos
    /// indefesos"; GDD v1.2 cap. 05 "nao dar experiencia por contagem bruta de golpes").
    /// C# PURO: o tempo entra por parametro, entao o teste negativo roda em EditMode sem cena.
    ///
    /// A REGRA, EM UMA FRASE: so conta o que exigiu do jogador o que o oponente estava cobrando dele.
    ///   1. Golpe em alvo INDEFESO vale 0, sempre. Nao e "vale menos": e zero. Bater num boneco parado mil
    ///      vezes soma exatamente nada. Este e o teste negativo obrigatorio.
    ///   2. Golpe em alvo com guarda ativa (telegrafando ou golpeando) vale 1: o jogador entrou na janela em
    ///      que podia levar o golpe de volta.
    ///   3. Bloqueio vale pela REACAO, nao por segurar: guarda levantada na hora vale 1, tardia vale 0,6,
    ///      guarda erguida desde sempre (turtle) vale 0. Sem isso, segurar o botao viraria renda passiva.
    ///   4. Esquiva so conta quando os i-frames ENGOLIRAM um golpe (Health.Evaded), nunca por rolar sozinho.
    ///   5. Cada tipo tem tempo de recarga: o mesmo tipo de acerto nao rende duas vezes em sequencia rapida.
    ///      Isso limita a TAXA; o TETO por etapa da vida e da T009 (LifeState.pratica), nao daqui.
    ///
    /// ponytail: qualidade discreta (0 / 0,6 / 1). Se o playtest pedir nota continua (distancia do timing
    /// perfeito, variedade de golpes), o unico lugar a mudar e este arquivo — quem chama so recebe 0..1.</summary>
    public class TrainingLedger
    {
        /// <summary>Segundos entre duas praticas do MESMO tipo. Escolhido acima do ciclo de golpe do boneco
        /// (TrainingDummyBrain: ~3,6 s por ciclo) nao ser o gargalo: quem gargala e o oponente, nao o relogio.</summary>
        public const float RecargaPadrao = 1.5f;

        /// <summary>Reacao de bloqueio: ate aqui vale 1 (levantou a guarda vendo o telegrafico).</summary>
        public const float ReacaoBoa = 0.6f;
        /// <summary>Entre ReacaoBoa e aqui vale 0,6. Acima: guarda parada, vale 0.</summary>
        public const float ReacaoTardia = 1.5f;

        readonly float recarga;
        readonly float[] ultimo = new float[3];
        bool[] usado = new bool[3];

        /// <summary>Quantas praticas significativas ja foram creditadas nesta sessao (telemetria/playtest).</summary>
        public int Creditadas { get; private set; }
        /// <summary>Golpes que nao renderam nada por serem em alvo indefeso. E o numero que o playtest quer ver
        /// crescer quando alguem tenta farmar.</summary>
        public int GolpesIgnorados { get; private set; }

        public TrainingLedger(float recarga = RecargaPadrao) { this.recarga = recarga; }

        /// <summary>Acertou o oponente. `alvoDefendido` = ele estava com guarda ativa no instante do impacto.
        /// Devolve a qualidade 0..1; 0 significa "nao reporte nada".</summary>
        public float RegistrarGolpe(bool alvoDefendido, float agora)
        {
            if (!alvoDefendido) { GolpesIgnorados++; return 0f; }
            return Creditar(PraticaTipo.Golpe, 1f, agora);
        }

        /// <summary>Bloqueou um golpe. `segurandoHa` = ha quantos segundos a guarda estava levantada quando o
        /// golpe chegou.</summary>
        public float RegistrarBloqueio(float segurandoHa, float agora)
        {
            float q = segurandoHa <= ReacaoBoa ? 1f : (segurandoHa <= ReacaoTardia ? 0.6f : 0f);
            if (q <= 0f) return 0f; // turtle: guarda erguida desde antes do telegrafico nao e leitura, e espera
            return Creditar(PraticaTipo.Bloqueio, q, agora);
        }

        /// <summary>Os i-frames da esquiva engoliram um golpe (Health.Evaded). Rolar no vazio nao chama isto.</summary>
        public float RegistrarEsquiva(float agora)
        {
            return Creditar(PraticaTipo.Esquiva, 1f, agora);
        }

        float Creditar(PraticaTipo tipo, float qualidade, float agora)
        {
            int i = (int)tipo;
            if (usado[i] && agora - ultimo[i] < recarga) return 0f;
            usado[i] = true;
            ultimo[i] = agora;
            Creditadas++;
            return qualidade;
        }
    }
}
