using UnityEngine;

namespace COE
{
    /// <summary>Treino de combate do jogador (T011): ataque leve/forte, bloqueio, esquiva e a primeira magia.
    /// Orquestra o que ja existe — le o input (T002), pergunta o custo ao CombatMoves, gasta em CombatResources,
    /// pede a animacao ao CharacterAnimator (o dano dos golpes sai no AnimationEvent OnHitFrame; o da magia, na
    /// fase de manifestacao do SpellCast) e bate pelo Hitbox.
    /// Regra de verdade nenhuma mora aqui: tudo o que da para testar sem cena esta em CombatMoves, BlockRule,
    /// CombatResources, SpellCast e TrainingLedger.
    ///
    /// CONTROLES (hipotese v0; este arquivo so le ACOES do PlayerInputReader, nunca dispositivo. O toque do
    /// ADR-0006 alimenta as mesmas acoes la; aqui nada muda):
    ///   Ataque leve  — botao esquerdo do mouse / gamepad Sul   (PlayerInputReader.AttackPressed)
    ///   Ataque forte — Q / gatilho direito                     (PlayerInputReader.HeavyPressed)
    ///   Bloqueio     — C segurado / gatilho esquerdo segurado  (PlayerInputReader.BlockHeld)
    ///   Esquiva      — Espaco / gamepad Leste                  (PlayerInputReader.DodgePressed)
    ///   Magia        — R / ombro esquerdo                      (PlayerInputReader.CastPressed)
    ///
    /// IDADE MANDA NO ACESSO: nada disso existe antes do salto para ~8 anos (dossie §F/§L). Quem responde e
    /// TrainingProgress.PodeTreinar(idade da sessao) — o numero 8 nao aparece neste arquivo. A esquiva e a excecao consciente:
    /// e rolar, nao combate, e continua disponivel na primeira infancia.</summary>
    [DefaultExecutionOrder(-40)] // depois do CharacterMotor (-50): o bloqueio ve a rotacao ja aplicada do frame
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] PlayerInputReader input;
        [SerializeField] Hitbox hitbox;
        [SerializeField] CharacterAnimator anim;
        [SerializeField] Health health;
        [SerializeField] SomDoJogo som;   // opcional: sem ele, mudo

        [Header("Recursos (hipotese v0 — CombatMoves)")]
        [SerializeField] float vigorMax = CombatMoves.VigorMaxV0;
        [SerializeField] float manaMax = CombatMoves.ManaMaxV0;

        /// <summary>A partida em que o treino rende (idade e pratica). Dependencia explicita: teste e quem monta a cena
        /// atribuem; sem atribuicao, a sessao do SaveState (bootstrap do processo).</summary>
        public GameSession Sessao { get { return sessao ?? SaveState.Sessao; } set { sessao = value; } }
        GameSession sessao;

        /// <summary>Treino liberado pela idade da partida (TrainingDummy e camera perguntam aqui).</summary>
        public bool PodeTreinar { get { return TrainingProgress.PodeTreinar(Sessao.Save.ageYears); } }

        /// <summary>A ultima pratica que chegou ao Mastery e o que ela rendeu: o que o TreinoHud mostra (B15). Registros
        /// sobe a cada pratica aceita; a tela compara para saber que ha novidade. Nao vai para o save.</summary>
        public AtividadeDef UltimaAtividade { get; private set; }
        public GanhoResultado UltimoGanho { get; private set; }
        public int Registros { get; private set; }

        public CombatResources Recursos { get; private set; }
        public TrainingLedger Pratica { get; private set; }
        /// <summary>Fases e recarga da primeira magia (HUD/VFX leem Magia.Fase).</summary>
        public SpellCast Magia { get; private set; }
        /// <summary>Guarda levantada NESTE frame (HUD e TrainingDummy podem ler).</summary>
        public bool Bloqueando { get; private set; }
        /// <summary>Travado na recuperacao de uma acao, ou com a magia em curso (preparacao, manifestacao ou
        /// consequencia): nenhum input de combate e aceito.</summary>
        public bool EmRecuperacao { get { return Time.time < recuperacaoAte || Magia.Fase != SpellFase.Pronta; } }

        float recuperacaoAte;
        float bloqueioDesde = -1f;
        float ultimoGolpe = -99f;
        readonly ComboCounter combo = new ComboCounter();

        void Awake()
        {
            Recursos = new CombatResources(vigorMax, manaMax);
            Pratica = new TrainingLedger();
            Magia = new SpellCast();
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
            if (Magia.Tick(Time.deltaTime)) Acertar(CombatMoves.Magia, TrainingProgress.AtividadeMagia); // manifestacao

            AtualizarBloqueio();
            if (EmRecuperacao) return;

            if (EsquivaPressionada()) { Esquivar(); return; } // esquivar sai do bloqueio: nao fica preso na guarda
            if (Bloqueando) return;                           // guarda levantada nao ataca
            if (!PodeTreinar) return; // primeira infancia: sem ataque, magia nem bloqueio

            if (MagiaPressionada()) { LancarMagia(); return; }
            if (FortePressionado()) { AtacarForte(); return; }
            if (LevePressionado()) { AtacarLeve(); }
        }

        // ---------------- acoes (API publica: o input e so UM dos chamadores; missao e teste chamam direto) ----------------

        /// <summary>Ataque leve. false = nao saiu (idade, recuperacao, guarda ou vigor).</summary>
        public bool AtacarLeve() { return Executar(CombatMoves.Leve, TrainingProgress.AtividadeLeve); }

        /// <summary>Ataque forte: custa mais Vigor, bate mais, recupera mais devagar.</summary>
        public bool AtacarForte() { return Executar(CombatMoves.Forte, TrainingProgress.AtividadeForte); }

        /// <summary>A primeira manifestacao magica: gasta Mana e comeca a PREPARACAO; o efeito sai na manifestacao
        /// (Update -> Magia.Tick). false = idade, recuperacao, recarga ou sem Mana, e nesses casos nada e cobrado.</summary>
        public bool LancarMagia()
        {
            if (EmRecuperacao || !PodeTreinar) return false;
            if (health != null && health.Dead) return false;
            if (!Magia.Pronta) return false;                                   // recarga: checada antes de cobrar
            if (!Recursos.Mana.TryGastar(CombatMoves.Magia.Mana)) return false; // magia sem mana nao existe
            Magia.Iniciar();
            if (som != null) som.Tocar(Som.Magia);
            if (anim != null) anim.Skill(0, null); // so o gatilho visual: o instante do efeito e do SpellCast
            return true;
        }

        bool Executar(MoveSpec spec, AtividadeDef atividade)
        {
            if (EmRecuperacao || !PodeTreinar) return false;
            if (health != null && health.Dead) return false;
            if (!Recursos.Vigor.TryGastar(spec.Vigor)) return false; // sem recurso, a acao NAO sai

            recuperacaoAte = Time.time + spec.Recuperacao;
            int indice = combo.Next(Time.time - ultimoGolpe);
            ultimoGolpe = Time.time;
            if (anim != null) anim.Attack(indice, delegate { Acertar(spec, atividade); });
            else Acertar(spec, atividade);
            return true;
        }

        /// <summary>Momento do impacto (OnHitFrame do clip, na hora sem Animator, ou manifestacao da magia).</summary>
        void Acertar(MoveSpec spec, AtividadeDef atividade)
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
                Praticar(atividade, q);
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
            Praticar(TrainingProgress.AtividadeEsquiva, q);
        }

        void AtualizarBloqueio()
        {
            bool quer = BloqueioSegurado() && PodeTreinar && !EmRecuperacao
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
            Praticar(TrainingProgress.AtividadeBloqueio, q); // defesa tambem e afinidade marcial
            return saida;
        }

        /// <summary>Pratica que valeu (qualidade do TrainingLedger) vira dominio pela sessao, que grava.</summary>
        void Praticar(AtividadeDef atividade, float qualidade)
        {
            GanhoResultado g = TrainingProgress.Registrar(Sessao, atividade, qualidade);
            if (!g.Aceito) return;
            UltimaAtividade = atividade;
            UltimoGanho = g;
            Registros++;
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
        bool FortePressionado() { return input != null && input.HeavyPressed; }
        bool MagiaPressionada() { return input != null && input.CastPressed; }
        bool BloqueioSegurado() { return input != null && input.BlockHeld; }
    }
}
