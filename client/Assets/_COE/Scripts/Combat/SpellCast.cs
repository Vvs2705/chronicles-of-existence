namespace COE
{
    public enum SpellFase
    {
        Pronta = 0,        // nada em curso (pode estar em recarga: ver SpellCast.Pronta)
        Preparacao = 1,    // concentrando: o efeito AINDA nao existe; o lancador fica comprometido
        Manifestacao = 2,  // o efeito sai na ENTRADA desta fase
        Consequencia = 3,  // depois do efeito: exposto, recuperando
    }

    /// <summary>A primeira manifestacao magica em tres fases legiveis (dossie §J: "preparacao, manifestacao,
    /// consequencia") mais a recarga. C# PURO e deterministico: o tempo chega por Tick(dt), como no
    /// TrainingDummyBrain, entao a regra e testavel em EditMode sem cena.
    ///
    /// POR QUE O RITMO E REGRA E NAO CLIP: o placeholder nao tem clip Skill (HumanoidSetup reusa Attack3). Com o
    /// OnHitFrame mandando, a fagulha sairia no tempo de um golpe de espada, sem preparacao nenhuma. Quem vier
    /// com clip/VFX (T013) ilustra as fases que `Fase` expoe; nao decide quando o efeito acontece.
    ///
    /// ponytail: duracoes fixas e sem interrupcao (levar dano na preparacao nao cancela). Quando o design pedir
    /// magia interrompivel, entra um Interromper() que volta a Pronta mantendo a recarga.</summary>
    public class SpellCast
    {
        readonly float preparacao, manifestacao, consequencia, recarga;
        float t;
        float recargaRestante;

        public SpellFase Fase { get; private set; }
        public float RecargaRestante { get { return recargaRestante; } }
        /// <summary>Pode lancar agora: nenhuma fase em curso e recarga zerada.</summary>
        public bool Pronta { get { return Fase == SpellFase.Pronta && recargaRestante <= 0f; } }

        /// <summary>Numeros v0 da fagulha inicial (CombatMoves).</summary>
        public SpellCast()
            : this(CombatMoves.MagiaPreparacaoV0, CombatMoves.MagiaManifestacaoV0,
                   CombatMoves.Magia.Recuperacao, CombatMoves.MagiaRecargaV0) { }

        public SpellCast(float preparacao, float manifestacao, float consequencia, float recarga)
        {
            this.preparacao = preparacao; this.manifestacao = manifestacao;
            this.consequencia = consequencia; this.recarga = recarga;
        }

        /// <summary>Comeca a preparacao e a recarga. false = em curso ou em recarga (nada muda).
        /// Custo de Mana e de quem chama (PlayerCombat): esta classe so conhece tempo.</summary>
        public bool Iniciar()
        {
            if (!Pronta) return false;
            Fase = SpellFase.Preparacao;
            t = 0f;
            recargaRestante = recarga;
            return true;
        }

        /// <summary>Avanca o tempo. Devolve true no passo em que o efeito se MANIFESTA, uma vez por lancamento.
        /// dt grande nao pula a manifestacao: o laco consome fase a fase.</summary>
        public bool Tick(float dt)
        {
            if (dt <= 0f) return false;
            recargaRestante = recargaRestante > dt ? recargaRestante - dt : 0f;
            if (Fase == SpellFase.Pronta) return false;

            t += dt;
            bool manifestou = false;
            while (Fase != SpellFase.Pronta && t >= Duracao(Fase))
            {
                t -= Duracao(Fase);
                Fase = Fase == SpellFase.Consequencia ? SpellFase.Pronta : (SpellFase)((int)Fase + 1);
                if (Fase == SpellFase.Manifestacao) manifestou = true;
            }
            if (Fase == SpellFase.Pronta) t = 0f;
            return manifestou;
        }

        float Duracao(SpellFase f)
        {
            switch (f)
            {
                case SpellFase.Preparacao: return preparacao;
                case SpellFase.Manifestacao: return manifestacao;
                default: return consequencia;
            }
        }
    }
}
