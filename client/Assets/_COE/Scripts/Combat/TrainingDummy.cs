using UnityEngine;

namespace COE
{
    /// <summary>Oponente de treino supervisionado (dossie §F, GDD v1.2 cap. 05). Casca de Unity em volta do
    /// TrainingDummyBrain: o cerebro decide (C# puro, deterministico), este componente APLICA — vira de frente,
    /// dispara o telegrafico na animacao e na cor do corpo (HitFlash.Hold) e usa o Hitbox que ja existe para bater.
    ///
    /// O QUE ELE NAO FAZ, DE PROPOSITO: nao persegue (nunca ha Move/NavMesh aqui), nao sorteia golpe, nao
    /// procura ninguem (o alvo chega pelo gerador de cena) e so pergunta de fora a idade (PlayerCombat.PodeTreinar do alvo).
    /// Fora da fase Ociosa ele nem gira: quem circular durante o telegrafico consegue
    /// mesmo chegar nas costas dele — e e isso que torna a posicao uma decisao.
    ///
    /// ANTI-FARM: `Guardando` e o que o PlayerCombat pergunta antes de creditar pratica. Parado na fase Ociosa
    /// ele e um alvo indefeso e acertar nao ensina nada (TrainingLedger).</summary>
    [RequireComponent(typeof(Health))]
    public class TrainingDummy : MonoBehaviour
    {
        [Tooltip("Golpe do boneco. Sem Hitbox no objeto, ele telegrafa e nao causa dano (util para greybox).")]
        [SerializeField] Hitbox hitbox;
        [SerializeField] CharacterAnimator anim;    // opcional: sem Animator o aviso fica so na cor (HitFlash)
        [Tooltip("Cor mantida no corpo durante a guarda (telegrafico + golpe): o aviso que se ve sem Animator. Hipotese v0.")]
        [SerializeField] Color corDoAviso = new Color(1f, 0.55f, 0.1f);
        [SerializeField] Transform alvo;            // para quem ele vira (ligado pelo gerador de cena); nulo = nao vira

        [Header("Ritmo do ciclo (s) — hipotese v0")]
        [SerializeField] float ocioso = TrainingDummyBrain.OciosoPadrao;
        [SerializeField] float telegrafico = TrainingDummyBrain.TelegraficoPadrao;
        [SerializeField] float golpe = TrainingDummyBrain.GolpePadrao;
        [SerializeField] float recuperacao = TrainingDummyBrain.RecuperacaoPadrao;

        [Header("Golpe — hipotese v0")]
        [Tooltip("Dano do bastao de treino. Baixo de proposito: o treino ensina, nao mata.")]
        [SerializeField] float dano = 4f;
        [SerializeField] float alcance = 1.6f;
        [SerializeField] float raio = 1f;
        [Tooltip("Fracao da vida em que o parceiro cede e pede pausa (nao morre).")]
        [SerializeField] float fracaoParaCeder = 0.2f;
        [Tooltip("Segundos de pausa antes de recomecar o treino. 0 = fica rendido.")]
        [SerializeField] float pausaAoCeder = 3f;

        TrainingDummyBrain brain; // criado no Awake com o ritmo do Inspector
        Health health;
        HitFlash flash;           // opcional: sem ele nao ha aviso por cor
        DummyFase faseAnterior;
        float rendidoDesde = -1f;

        /// <summary>Cedeu e esta em pausa: nao ataca, nao defende e ignora dano.</summary>
        public bool Rendido { get { return rendidoDesde >= 0f; } }
        /// <summary>Guarda ativa neste instante (o PlayerCombat pergunta isto para avaliar a pratica).</summary>
        public bool Guardando { get { return brain.Guardando && !Rendido; } }
        public DummyFase Fase { get { return brain.Fase; } }
        public int Ciclo { get { return brain.Ciclo; } }

        void Awake()
        {
            brain = new TrainingDummyBrain(ocioso, telegrafico, golpe, recuperacao);
            health = GetComponent<Health>();
            if (hitbox == null) hitbox = GetComponent<Hitbox>();
            if (anim == null) anim = GetComponent<CharacterAnimator>();
            flash = GetComponent<HitFlash>();
            health.DamageFilter = Ceder; // parceiro de treino nao morre; ver Ceder()
        }

        void OnDestroy() { if (health != null) health.DamageFilter = null; }

        /// <summary>Filtro de dano do parceiro. Rendido, ignora o golpe (devolver &lt;= 0 cancela o golpe, contrato
        /// do Health). Abaixo da fracao de rendicao, ele CEDE em vez de morrer: um adulto supervisiona o treino,
        /// ninguem mata o instrutor com espada de madeira. Sem isto o Health chegaria a zero e Heal() nao
        /// ressuscita — e o treino acabaria no primeiro combo bem dado.</summary>
        float Ceder(float raw, Transform origem)
        {
            if (Rendido) return 0f;
            if (health.Current - raw > health.max * fracaoParaCeder) return raw;
            Pausar();
            return 0f;
        }

        void Pausar() // cede: guarda baixa, sem golpe, ignora dano ate Recompor
        {
            rendidoDesde = Time.time;
            Avisar(false);
            if (anim != null) anim.Stagger(true);
        }

        void Update()
        {
            // Treino supervisionado so depois do salto (dossie §F): com a crianca de 5 anos o instrutor fica parado,
            // de guarda baixa. Sem isto um adulto bateria de bastao na crianca que passa pelo posto.
            if (!AlunoPodeTreinar()) return;
            if (Rendido) { Recompor(); return; }

            if (brain.Fase == DummyFase.Ocioso) Virar();

            bool bateuAgora = brain.Tick(Time.deltaTime);

            if (brain.Fase != faseAnterior)
            {
                if (brain.Fase == DummyFase.Telegrafico && anim != null) anim.Telegraph();
                // Cor na janela de guarda inteira (telegrafico -> fim do golpe). Pela guarda, e nao por "entrou em
                // Telegrafico / entrou em Recuperacao": um dt grande que pule fase nao deixa a cor presa.
                Avisar(brain.Guardando);
                faseAnterior = brain.Fase;
            }

            if (bateuAgora) Bater();
        }

        void Bater()
        {
            if (anim != null) anim.Attack(brain.Variante, Golpear); // dano no OnHitFrame do clip
            else Golpear();
        }

        void Golpear()
        {
            if (hitbox == null || Rendido) return;
            // O treino ensina, nao mata (B15): se o bastao levaria o aluno abaixo da mesma fracao em que o parceiro
            // cede, ele cede EM VEZ de bater e, ao recompor, cura o aluno. Sem isto a crianca chegava a vida 0 e o
            // PlayerCombat travava de vez (Health nao ressuscita).
            // ponytail: previsao pelo dano BRUTO (defesa e bloqueio so reduzem). Teto: so protege o `alvo` ligado
            // pelo gerador, e ignora damageTakenMult > 1 no aluno; se isso existir, multiplicar aqui.
            Health aluno = Aluno();
            if (aluno != null && aluno.Current - dano <= aluno.max * fracaoParaCeder) { Pausar(); return; }
            hitbox.Swing(dano, 0f, alcance, raio);
        }

        Health Aluno() { return alvo != null ? alvo.GetComponentInParent<Health>() : null; }

        /// <summary>A idade e a da partida do aluno (PlayerCombat do alvo). Sem alvo ligado, ninguem para supervisionar:
        /// fica parado, de guarda baixa.</summary>
        bool AlunoPodeTreinar()
        {
            if (aluno == null && alvo != null) aluno = alvo.GetComponentInParent<PlayerCombat>();
            return aluno != null && aluno.PodeTreinar;
        }
        PlayerCombat aluno;

        void Avisar(bool ligado)
        {
            if (flash != null) flash.Hold = ligado ? corDoAviso : (Color?)null;
        }

        void Virar()
        {
            if (alvo == null) return;
            Vector3 d = alvo.position - transform.position;
            d.y = 0f;
            if (d.sqrMagnitude < 1e-4f) return;
            // Giro lento e so na fase Ociosa: ninguem e "grudado" pelo boneco durante o golpe.
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, Quaternion.LookRotation(d), 120f * Time.deltaTime);
        }

        // ponytail: pausa por tempo fixo. Faze-lo cede -> recompoe nao abre exploit: durante a pausa a guarda esta
        // baixa, e acertar alvo indefeso rende zero pratica (TrainingLedger). Teto: nao ha fala nem reacao do
        // instrutor; quando a T007 (NPC/dialogo) existir, a pausa e o gancho para ele comentar o treino.
        void Recompor()
        {
            if (pausaAoCeder <= 0f) return;
            if (Time.time - rendidoDesde < pausaAoCeder) return;
            health.Heal(health.max);
            Health aluno = Aluno();
            if (aluno != null) aluno.Heal(aluno.max); // o adulto recompoe os dois antes de recomecar
            brain.Reiniciar();
            faseAnterior = DummyFase.Ocioso;
            rendidoDesde = -1f;
            if (anim != null) anim.Stagger(false);
        }
    }
}
