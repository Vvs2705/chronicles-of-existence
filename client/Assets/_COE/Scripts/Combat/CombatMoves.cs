using UnityEngine;

namespace COE
{
    /// <summary>Uma acao de combate do treino: custo, dano e recuperacao. Struct de DADO, sem comportamento.
    /// Ponte com a animacao ja existente: o dano dos golpes sai no AnimationEvent OnHitFrame (CharacterAnimator.Attack),
    /// nao no frame do input; o da magia sai na manifestacao do SpellCast.</summary>
    public struct MoveSpec
    {
        public string Id;            // id estavel, snake_case (CLAUDE.md)
        public float Vigor;          // custo
        public float Mana;           // custo
        public float Dano;
        public float Postura;        // 0 = Health usa dano x 0,5
        public float Alcance;        // metros a frente (Hitbox.Swing)
        public float Raio;
        public float Recuperacao;    // segundos travado depois da acao
        public string Afinidade;     // id do CONTRATO_T003_T004 §1: "marcial" | "arcana"
        /// <summary>Golpe de arma: alcance = (braco + lamina) x este fator, no corpo de quem bate (CombatMoves.NoCorpo).
        /// 0 = Alcance e Raio absolutos (magia: o alcance e da fagulha, nao do braco).</summary>
        public float FatorDeAlcance;
    }

    /// <summary>Catalogo v0 do TREINO (T011), nao do combate adulto. GDD v1.2 cap. 05: "espada de madeira,
    /// ataque leve/forte, bloqueio/esquiva, uma manifestacao magica, um adversario de treino e barras de
    /// Vida/Vigor/Mana". Sem combo de adulto: o ComboCounter que ja existe so troca o CLIP (Attack1..3), o dano
    /// e igual em qualquer indice.
    ///
    /// TODOS OS NUMEROS AQUI SAO HIPOTESE v0 (dossie §N: "numeros exatos de XP/atributos/danos" seguem em
    /// aberto). Existem para o treino ser jogavel e medivel, nao para balancear o jogo.</summary>
    public static class CombatMoves
    {
        // --- Recursos (hipotese v0: crianca de ~8 anos com espada de madeira) ---
        public const float VigorMaxV0 = 50f;
        public const float VigorRegenV0 = 12f;   // por segundo
        public const float VigorAtrasoV0 = 1f;   // segundos sem gastar antes de voltar a subir
        public const float ManaMaxV0 = 30f;
        public const float ManaRegenV0 = 3f;
        public const float ManaAtrasoV0 = 2f;    // magia se recupera mais devagar que folego

        /// <summary>Ids de afinidade do CONTRATO_T003_T004 §1. Sao os mesmos campos de SaveData.Affinities.</summary>
        public const string AfinidadeMarcial = "marcial";
        public const string AfinidadeArcana = "arcana";

        // --- Defesa ---
        public const float IFramesEsquiva = 0.35f;        // Health.SetInvulnerable
        public const float ReducaoBloqueioFrontal = 0.7f; // 70% do dano frontal absorvido
        public const float VigorPorBloqueio = 8f;         // drenado por golpe efetivamente bloqueado
        /// <summary>Meia-abertura do cone de bloqueio, em graus: 0 = de frente, 100 = quase de lado.
        /// Acima disso o golpe entra pelas costas e o bloqueio NAO vale.</summary>
        public const float ConeBloqueioGraus = 100f;

        // --- Arma (Bloco E): a UNICA do slice e a espada de madeira do treino (B15, GDD cap. 05) ---
        /// <summary>Comprimento da lamina da espada de madeira, em metros (espada de treino de crianca). HIPOTESE v0.</summary>
        public const float LaminaEspadaDeMadeira = 0.6f;

        /// <summary>Golpes de espada: Alcance/Raio saem do corpo de quem bate (NoCorpo), nao de numero solto. Os valores aqui
        /// sao so o padrao de quem nao tem corpo (teste sem idade): a crianca de 8 anos, 1,28 m.</summary>
        public static readonly MoveSpec Leve = new MoveSpec
        {
            Id = "ataque_leve", Vigor = 4f, Mana = 0f, Dano = 6f, Postura = 0f,
            Recuperacao = 0.45f, Afinidade = AfinidadeMarcial, FatorDeAlcance = 1f,
        };

        /// <summary>O forte estica o braco: 10% mais longe que o leve. HIPOTESE v0.</summary>
        public static readonly MoveSpec Forte = new MoveSpec
        {
            Id = "ataque_forte", Vigor = 12f, Mana = 0f, Dano = 14f, Postura = 10f,
            Recuperacao = 0.9f, Afinidade = AfinidadeMarcial, FatorDeAlcance = 1.1f,
        };

        /// <summary>O golpe no corpo de quem bate: a esfera do Hitbox vai da frente do corpo (Corpo.Raio) ate o braco + a
        /// lamina, vezes o fator do golpe. Aos 8 anos, leve de ~0,32 a ~1,11 m (era uma esfera de 1,4 m com raio 0,9: alcancava
        /// 2,3 m, golpe de adulto). Golpe sem fator (magia) volta igual.</summary>
        public static MoveSpec NoCorpo(MoveSpec golpe, Corpo corpo, float lamina)
        {
            if (golpe.FatorDeAlcance <= 0f) return golpe;
            float perto = corpo.Raio, longe = (corpo.Braco + lamina) * golpe.FatorDeAlcance;
            golpe.Raio = Mathf.Max(0.05f, (longe - perto) * 0.5f);
            golpe.Alcance = perto + golpe.Raio;
            return golpe;
        }

        /// <summary>A UNICA magia do slice: uma fagulha de curto alcance. Escola completa e futuro
        /// (dossie §F: elemental/protecao/restauradora/arcana sao conceito, nao escopo do slice).
        /// O telegrafico proprio e a fase de preparacao (SpellCast), nao o clip: a magia nao e um golpe leve barato.
        /// Recuperacao = duracao da fase de consequencia.</summary>
        public static readonly MoveSpec Magia = new MoveSpec
        {
            Id = "fagulha_inicial", Vigor = 0f, Mana = 10f, Dano = 8f, Postura = 0f,
            Alcance = 2.4f, Raio = 1.1f, Recuperacao = 1f, Afinidade = AfinidadeArcana,
        };

        // --- Fases da magia (SpellCast; dossie §J "preparacao, manifestacao, consequencia") ---
        public const float MagiaPreparacaoV0 = 0.5f;   // concentrando: legivel para quem esta de frente
        public const float MagiaManifestacaoV0 = 0.2f; // o efeito sai na ENTRADA desta fase
        /// <summary>Segundos entre dois lancamentos, contados do inicio. Maior que as tres fases somadas (1,7 s):
        /// quem gasta a mana toda nao encadeia fagulhas.</summary>
        public const float MagiaRecargaV0 = 3f;

        /// <summary>Esquiva: so custo e recuperacao (dano/alcance nao se aplicam). O deslocamento continua
        /// sendo do CharacterMotor — a T011 nao mexe em movimento.</summary>
        public static readonly MoveSpec Esquiva = new MoveSpec
        {
            Id = "esquiva", Vigor = 10f, Mana = 0f, Dano = 0f, Postura = 0f,
            Alcance = 0f, Raio = 0f, Recuperacao = 0.4f, Afinidade = AfinidadeMarcial,
        };
    }

    /// <summary>Regra do bloqueio. C# PURO: recebe angulo em graus em vez de vetor, entao testa sem cena.</summary>
    public static class BlockRule
    {
        /// <summary>Dano que passa pela guarda. `anguloGraus` = angulo entre a frente do defensor e a direcao
        /// do atacante (0 = de frente, 180 = pelas costas). Fora do cone ou sem vigor, o dano passa inteiro:
        /// segurar a guarda nao e invulnerabilidade e guarda quebrada nao protege.</summary>
        public static float Reduzir(float danoBruto, float anguloGraus, bool temVigor)
        {
            if (danoBruto <= 0f) return danoBruto;
            if (anguloGraus > CombatMoves.ConeBloqueioGraus) return danoBruto; // pelas costas
            if (!temVigor) return danoBruto;                                   // guarda quebrada
            return danoBruto * (1f - CombatMoves.ReducaoBloqueioFrontal);
        }
    }
}
