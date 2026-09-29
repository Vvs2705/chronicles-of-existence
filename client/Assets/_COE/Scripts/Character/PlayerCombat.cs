using UnityEngine;
using UnityEngine.InputSystem;

namespace COE
{
    /// <summary>Treino de combate do jogador (T011): ataque leve/forte, bloqueio, esquiva e a primeira magia.
    /// Orquestra o que ja existe — le o input (T002), pergunta o custo ao CombatMoves, gasta em CombatResources,
    /// pede a animacao ao CharacterAnimator (o dano sai no AnimationEvent OnHitFrame) e bate pelo Hitbox.
    /// Regra de verdade nenhuma mora aqui: tudo o que da para testar sem cena esta em CombatMoves, BlockRule,
    /// CombatResources e TrainingLedger.
    ///
    /// CONTROLES (hipotese v0; rebind e assunto do Input, nao deste arquivo):
    ///   Ataque leve  — botao esquerdo do mouse / gamepad Sul   (PlayerInputReader.AttackPressed)
    ///   Ataque forte — Q / gatilho direito (slot 3)
    ///   Bloqueio     — C segurado / gatilho esquerdo segurado (slot 2)
    ///   Esquiva      — Espaco / gamepad Leste                  (PlayerInputReader.DodgePressed)
    ///   Magia        — R / ombro esquerdo (slot 0)
    /// ponytail: Q/C/R sao lidos direto do Keyboard porque o PlayerInputReader ainda nao expoe estes tres.
    /// Quando ele expuser (HeavyPressed/BlockHeld/CastPressed), apagar os tres metodos *Direto daqui.
    ///
    /// IDADE MANDA NO ACESSO: nada disso existe antes do salto para ~8 anos (dossie §F/§L). Quem responde e
    /// TrainingProgress.PodeTreinar() — o numero 8 nao aparece neste arquivo. A esquiva e a excecao consciente:
    /// e rolar, nao combate, e continua disponivel na primeira infancia.</summary>
    [DefaultExecutionOrder(-40)] // depois do CharacterMotor (-50): o bloqueio ve a rotacao ja aplicada do frame
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] PlayerInputReader input;
        [SerializeField] Hitbox hitbox;
        [SerializeField] CharacterAnimator anim;
        [SerializeField] Health health;

        [Header("Recursos (hipotese v0 — CombatMoves)")]
        [SerializeField] float vigorMax = CombatMoves.VigorMaxV0;
        [SerializeField] float manaMax = CombatMoves.ManaMaxV0;

        public CombatResources Recursos { get; private set; }
        public TrainingLedger Pratica { get; private set; }
        /// <summary>Guarda levantada NESTE frame (HUD e TrainingDummy podem ler).</summary>
        public bool Bloqueando { get; private set; }
        /// <summary>Travado na recuperacao de uma acao: nenhum input de combate e aceito.</summary>
        public bool EmRecuperacao { get { return Time.time < recuperacaoAte; } }

        float recuperacaoAte;
        float bloqueioDesde = -1f;
        float ultimoGolpe = -99f;
        readonly ComboCounter combo = new ComboCounter();

        void Awake()
        {
            Recursos = new CombatResources(vigorMax, manaMax);
            Pratica = new TrainingLedger();
            if (hitbox == null) hitbox = GetComponent<Hitbox>();
            if (anim == null) anim = GetComponent<CharacterAnimator>();
            if (health == null) health = GetComponent<Health>();
            if (input == null) input = GetComponent<PlayerInputReader>();
            if (health != null)
            {
                health.DamageFilter = FiltrarDano;   // bloqueio
                health.Evaded += EsquivouGolpe;      // i-frames engoliram um golpe
            }
        }

        void OnDestroy()
        {
            if (health == null) return;
            health.DamageFilter = null;
            health.Evaded -= EsquivouGolpe;
        }

        void Update()
        {
            Recursos.Tick(Time.deltaTime);
            if (health != null && health.Dead) { Bloqueando = false; return; }

            AtualizarBloqueio();
            if (EmRecuperacao) return;

            if (EsquivaPressionada()) { Esquivar(); return; } // esquivar sai do bloqueio: nao fica preso na guarda
            if (Bloqueando) return;                           // guarda levantada nao ataca
            if (!TrainingProgress.PodeTreinar()) return; // primeira infancia: sem ataque, magia nem bloqueio

            if (MagiaPressionada()) { LancarMagia(); return; }
            if (FortePressionado()) { AtacarForte(); return; }
            if (LevePressionado()) { AtacarLeve(); }
        }

        // ---------------- acoes (API publica: o input e so UM dos chamadores; missao e teste chamam direto) ----------------

        /// <summary>Ataque leve. false = nao saiu (idade, recuperacao, guarda ou vigor).</summary>
        public bool AtacarLeve() { return Executar(CombatMoves.Leve, false); }

        /// <summary>Ataque forte: custa mais Vigor, bate mais, recupera mais devagar.</summary>
        public bool AtacarForte() { return Executar(CombatMoves.Forte, false); }

        /// <summary>A primeira manifestacao magica. Sem Mana nao sai.</summary>
        public bool LancarMagia() { return Executar(CombatMoves.Magia, true); }

        bool Executar(MoveSpec spec, bool magia)
        {
            if (EmRecuperacao || !TrainingProgress.PodeTreinar()) return false;
            if (health != null && health.Dead) return false;
            ResourcePool poco = magia ? Recursos.Mana : Recursos.Vigor;
            float custo = magia ? spec.Mana : spec.Vigor;
            if (!poco.TryGastar(custo)) return false; // sem recurso, a acao NAO sai (magia sem mana nao existe)

            recuperacaoAte = Time.time + spec.Recuperacao;
            MoveSpec s = spec; // captura por valor para o callback do AnimationEvent
            if (magia)
            {
                if (anim != null) anim.Skill(0, delegate { Acertar(s); });
                else Acertar(s);
                return true;
            }
            int indice = combo.Next(Time.time - ultimoGolpe);
            ultimoGolpe = Time.time;
            if (anim != null) anim.Attack(indice, delegate { Acertar(s); });
            else Acertar(s);
            return true;
        }

        /// <summary>Momento do impacto (chega pelo OnHitFrame do clip, ou na hora quando nao ha Animator).</summary>
        void Acertar(MoveSpec spec)
        {
            if (hitbox == null) return;
            hitbox.Swing(spec.Dano, spec.Postura, spec.Alcance, spec.Raio);
            for (int i = 0; i < hitbox.LastHits.Count; i++)
            {
                Health alvo = hitbox.LastHits[i];
                if (alvo == null) continue;
                // ponytail: so o parceiro de treino sabe dizer se estava defendido. Alvo sem TrainingDummy conta
                // como indefeso -> zero pratica. Quando existir inimigo de verdade, ele expoe o mesmo "Guardando"
                // e este e o unico lugar a mudar.
                TrainingDummy d = alvo.GetComponentInParent<TrainingDummy>();
                float q = Pratica.RegistrarGolpe(d != null && d.Guardando, Time.time);
                TrainingProgress.Registrar(spec.Afinidade, q);
            }
        }

        /// <summary>Esquiva: i-frames a custo de Vigor. Liberada tambem na primeira infancia — e rolar, nao combate.</summary>
        public bool Esquivar()
        {
            MoveSpec spec = CombatMoves.Esquiva;
            if (EmRecuperacao) return false;
            if (health != null && health.Dead) return false;
            if (!Recursos.Vigor.TryGastar(spec.Vigor)) return false;
            recuperacaoAte = Time.time + spec.Recuperacao;
            if (health != null) health.SetInvulnerable(CombatMoves.IFramesEsquiva);
            if (anim != null) anim.Dodge();
            return true;
        }

        /// <summary>Health.Evaded: os i-frames engoliram um golpe de verdade. Rolar no vazio nao passa por aqui —
        /// e por isso que esquiva nao e farmavel.</summary>
        void EsquivouGolpe()
        {
            float q = Pratica.RegistrarEsquiva(Time.time);
            TrainingProgress.Registrar(CombatMoves.AfinidadeMarcial, q);
        }

        void AtualizarBloqueio()
        {
            bool quer = BloqueioSegurado() && TrainingProgress.PodeTreinar() && !EmRecuperacao
                        && Recursos.Vigor.Tem(CombatMoves.VigorPorBloqueio);
            if (quer && !Bloqueando) bloqueioDesde = Time.time;
            if (!quer) bloqueioDesde = -1f;
            Bloqueando = quer;
        }

        /// <summary>Health.DamageFilter: o dano bruto passa por aqui antes da defesa. Frontal e com vigor,
        /// a guarda absorve e drena Vigor; pelas costas ou sem vigor, passa inteiro (BlockRule).</summary>
        float FiltrarDano(float raw, Transform origem)
        {
            if (!Bloqueando || origem == null) return raw; // origem desconhecida nao ganha desconto
            float ang = Angulo(origem.position);
            bool temVigor = Recursos.Vigor.Tem(CombatMoves.VigorPorBloqueio);
            float saida = BlockRule.Reduzir(raw, ang, temVigor);
            if (saida >= raw) return raw; // nao bloqueou (costas ou guarda quebrada)

            Recursos.Vigor.Drenar(CombatMoves.VigorPorBloqueio);
            float segurandoHa = bloqueioDesde >= 0f ? Time.time - bloqueioDesde : 999f;
            float q = Pratica.RegistrarBloqueio(segurandoHa, Time.time);
            TrainingProgress.Registrar(CombatMoves.AfinidadeMarcial, q); // defesa tambem e afinidade marcial
            return saida;
        }

        /// <summary>Angulo planar entre a frente do personagem e a direcao de quem atacou (0 = de frente).</summary>
        float Angulo(Vector3 posOrigem)
        {
            Vector3 d = posOrigem - transform.position;
            d.y = 0f;
            if (d.sqrMagnitude < 1e-6f) return 0f;
            Vector3 f = transform.forward;
            f.y = 0f;
            return Vector3.Angle(f, d);
        }

        // ---------------- input ----------------

        bool LevePressionado() { return input != null && input.AttackPressed; }
        bool EsquivaPressionada() { return input != null && input.DodgePressed; }
        bool FortePressionado() { return (input != null && input.SkillPressed(3)) || TeclaDireto(Key.Q); }
        bool MagiaPressionada() { return (input != null && input.SkillPressed(0)) || TeclaDireto(Key.R); }
        bool BloqueioSegurado() { return (input != null && input.SkillHeld(2)) || TeclaSeguraDireto(Key.C); }

        static bool TeclaDireto(Key k)
        {
            Keyboard kb = Keyboard.current;
            return kb != null && kb[k].wasPressedThisFrame;
        }

        static bool TeclaSeguraDireto(Key k)
        {
            Keyboard kb = Keyboard.current;
            return kb != null && kb[k].isPressed;
        }
    }
}
