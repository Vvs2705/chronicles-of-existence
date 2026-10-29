using UnityEngine;

namespace COE
{
    /// <summary>Casca de cena de um encontro do ep. 2 (Criaturas.Encontros): mora no QuestTrigger do objetivo de luta.
    /// Tocar o gatilho chama Comecar; o EncontroState (C# puro) decide, este componente APLICA: poe os corpos em volta da
    /// ancora, vira-os para o jogador, pinta o aviso, toca o som do aviso, bate no bote, some quem fugiu e, vencido,
    /// cumpre o objetivo pela sessao (MissaoMundo.Cumprir, uma gravacao).
    ///
    /// Derrota nao mata ninguem (§3.5): se o bote levaria a crianca abaixo de 20% da vida, Tovin a puxa para tras, a vida
    /// enche e o encontro recomeca do inicio. Nada vai para o save durante a luta: sair no meio = recomeca depois.
    /// ponytail: corpos de primitiva (greybox) criados ao comecar, ate a malha passar pelo G1/G2/G3 (ADR-0002). Sem
    /// perseguicao: cada criatura fica no seu lugar do anel e bate em linha a 1,6 m, entao o jogador vai ate ela. Upgrade
    /// com a malha: NavMesh curto e o bote como deslocamento.</summary>
    public class EncontroNoMundo : MonoBehaviour
    {
        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo gerador (PartidaSetup); vazio = a do SaveState.")]
        [SerializeField] Partida partida;
        [SerializeField] string encontroId = "";
        [Tooltip("O jogador: alvo do bote. Ligado pelo gerador (MissaoSceneSetup).")]
        [SerializeField] Transform jogador;
        [Tooltip("Ligado pelo gerador (SomSceneSetup); vazio = mudo.")]
        [SerializeField] SomDoJogo som;
        [Tooltip("Raio do anel em que as criaturas aparecem em volta da ancora (m).")]
        [SerializeField] float raio = 4f;
        [Tooltip("Fracao da vida da crianca abaixo da qual o bote vira derrota (Tovin puxa para tras).")]
        [SerializeField] float fracaoDaDerrota = 0.2f;
        [SerializeField] Color corDoAviso = new Color(1f, 0.55f, 0.1f);

        const float Alcance = 1.6f, RaioDoBote = 1f;

        EncontroState estado;
        Corpo[] corpos;
        string questId, objetivoId;

        public string EncontroId { get { return encontroId; } }
        public bool EmLuta { get { return estado != null && !estado.Vencido; } }
        public EncontroState Estado { get { return estado; } }

        sealed class Corpo
        {
            public CriaturaBrain Brain;
            public GameObject Go;
            public Health Health;
            public HitFlash Flash;
            public Hitbox Hitbox;
            public Vector3 Casa;
            public CriaturaFase Antes;
        }

        /// <summary>Comeca a luta (idempotente: chamar de novo no meio nao reinicia). false = sem encontro com esse id.</summary>
        public bool Comecar(string questId, string objetivoId)
        {
            if (EmLuta) return true;
            EncontroDef def = Criaturas.Encontro(encontroId);
            if (def == null) return false;
            this.questId = questId;
            this.objetivoId = objetivoId;
            estado = new EncontroState(def);
            Limpar();
            corpos = new Corpo[estado.Bando.Length];
            for (int i = 0; i < corpos.Length; i++) corpos[i] = Montar(estado.Bando[i], i, corpos.Length);
            return true;
        }

        Corpo Montar(CriaturaBrain b, int i, int n)
        {
            float ang = (360f / n) * i;
            Vector3 casa = transform.position + Quaternion.Euler(0f, ang, 0f) * (Vector3.forward * raio);
            Vector3 escala = Escala(b.Def.Id);
            GameObject go = GameObject.CreatePrimitive(b.Def.Id == Criaturas.Abafador ? PrimitiveType.Sphere : PrimitiveType.Cube);
            go.name = b.Def.Id;
            go.transform.SetParent(transform, true);
            go.transform.position = casa + Vector3.up * escala.y * 0.5f;
            go.transform.localScale = escala;
            Renderer r = go.GetComponent<Renderer>();
            Material m = NovoMaterial(b.Def.Id);
            if (m != null) r.sharedMaterial = m;

            go.AddComponent<Faction>().side = Side.Hostile;
            Health h = go.AddComponent<Health>();
            // A vida de verdade e a do CriaturaBrain. O Health so precisa nunca zerar: morto, ele recusa golpe (e o Heal
            // nao ressuscita), e a derrota recomeca o encontro com os mesmos corpos.
            h.max = 9999f;
            h.Heal(h.max);   // o Awake ja rodou com o max padrao
            Hitbox hb = go.AddComponent<Hitbox>();
            hb.altura = 0f;   // o pivo da primitiva ja esta no meio do corpo
            HitFlash flash = go.AddComponent<HitFlash>();
            var c = new Corpo { Brain = b, Go = go, Health = h, Flash = flash, Hitbox = hb, Casa = casa, Antes = b.Fase };
            h.DamageFilter = (raw, origem) => Golpeado(c, raw, origem);
            return c;
        }

        static Vector3 Escala(string id)
        {
            if (id == Criaturas.Abafador) return new Vector3(0.45f, 0.45f, 0.45f);
            if (id == Criaturas.Feixe) return new Vector3(0.5f, 1.3f, 0.5f);
            if (id == Criaturas.Lajeiro) return new Vector3(1.0f, 1.0f, 1.8f);
            return new Vector3(2.4f, 2.0f, 3.5f);   // Enredado
        }

        // Paleta do EPISODIO2 §3.4: musgo, terracota dessaturado, ardosia, violeta da anomalia.
        static Color Cor(string id)
        {
            if (id == Criaturas.Abafador) return new Color(0.32f, 0.45f, 0.33f);
            if (id == Criaturas.Feixe) return new Color(0.55f, 0.45f, 0.40f);
            if (id == Criaturas.Lajeiro) return new Color(0.38f, 0.40f, 0.43f);
            return new Color(0.59f, 0.47f, 0.72f);
        }

        static Material NovoMaterial(string id)
        {
            Shader lit = Shader.Find("Universal Render Pipeline/Lit");
            if (lit == null) return null;
            var m = new Material(lit) { name = "COE_Criatura_" + id };
            m.SetColor("_BaseColor", Cor(id));
            return m;
        }

        /// <summary>Filtro de dano do corpo: quem decide se entrou e o EncontroState. O Enredado nao perde vida: golpe com
        /// ele aberto corta o proximo no.</summary>
        float Golpeado(Corpo c, float raw, Transform origem)
        {
            if (estado == null || estado.Vencido) return 0f;
            if (c.Brain.Def.Id == Criaturas.Enredado)
            {
                for (int i = 0; i < 3; i++)
                    if (!estado.NoCortado((NoDoEnredado)i) && estado.Cortar((NoDoEnredado)i))
                    {
                        if (som != null) { som.Tocar(Som.NoCortado); som.MarcarCombate(); }
                        break;
                    }
                return 0f;
            }
            bool pelaFrente = false;
            if (origem != null)
            {
                Vector3 d = origem.position - c.Go.transform.position;
                d.y = 0f;
                pelaFrente = d.sqrMagnitude > 1e-4f && Vector3.Dot(c.Go.transform.forward, d.normalized) > 0.5f;
            }
            return estado.Golpear(c.Brain, raw, pelaFrente);
        }

        void Update()
        {
            if (estado == null || corpos == null) return;
            if (estado.Vencido) { Vencer(); return; }

            CriaturaBrain[] botes = estado.Tick(Time.deltaTime);
            foreach (Corpo c in corpos)
            {
                if (c.Go == null) continue;
                if (c.Brain.Fora) { Fugir(c); continue; }
                if (c.Brain.Fase == CriaturaFase.Espreita) Virar(c);
                if (c.Brain.Fase != c.Antes)
                {
                    if (c.Brain.Fase == CriaturaFase.Aviso && som != null) som.Tocar(c.Brain.Def.SomDoAviso);
                    c.Flash.Hold = c.Brain.Fase == CriaturaFase.Aviso ? corDoAviso : (Color?)null;
                    c.Antes = c.Brain.Fase;
                }
            }
            foreach (CriaturaBrain b in botes) Bater(b);
            if (estado.Vencido) Vencer();
        }

        void Bater(CriaturaBrain b)
        {
            Corpo c = Do(b);
            if (c == null || c.Go == null) return;
            Health crianca = jogador != null ? jogador.GetComponentInParent<Health>() : null;
            if (crianca != null && crianca.Current - b.Def.Dano <= crianca.max * fracaoDaDerrota) { Derrota(crianca); return; }
            c.Hitbox.Swing(b.Def.Dano, 0f, Alcance, RaioDoBote);
            if (som != null) som.MarcarCombate();
        }

        /// <summary>Tovin puxa a crianca para tras: vida cheia, ela volta para a ancora, o encontro recomeca do inicio.</summary>
        void Derrota(Health crianca)
        {
            crianca.Heal(crianca.max);
            if (jogador != null)
            {
                CharacterController cc = jogador.GetComponentInParent<CharacterController>();
                if (cc != null) cc.enabled = false;   // senao o CharacterController desfaz o teleporte
                jogador.position = transform.position - transform.forward * (raio + 2f);
                if (cc != null) cc.enabled = true;
            }
            estado.Derrota();
            foreach (Corpo c in corpos)
            {
                if (c.Go == null) continue;
                c.Go.SetActive(true);
                c.Health.Heal(c.Health.max);
                c.Go.transform.position = c.Casa + Vector3.up * c.Go.transform.localScale.y * 0.5f;
                c.Flash.Hold = null;
                c.Antes = c.Brain.Fase;
            }
        }

        void Fugir(Corpo c)
        {
            if (!c.Go.activeSelf) return;
            if (som != null) som.Tocar(Som.CriaturaFoge);
            c.Go.SetActive(false);
        }

        void Vencer()
        {
            EncontroState fim = estado;
            estado = null;
            Limpar();
            if (fim == null) return;
            GameSession s = Partida.De(partida);
            s.Posicao(CenaCatalogo.Id(gameObject.scene.name), MissaoMundo.AncoraDo(questId, objetivoId));
            MissaoMundo.Cumprir(s, questId, objetivoId);
            if (som != null) som.Tocar(Som.Missao);
        }

        void Virar(Corpo c)
        {
            if (jogador == null) return;
            Vector3 d = jogador.position - c.Go.transform.position;
            d.y = 0f;
            if (d.sqrMagnitude < 1e-4f) return;
            c.Go.transform.rotation = Quaternion.RotateTowards(c.Go.transform.rotation, Quaternion.LookRotation(d), 180f * Time.deltaTime);
        }

        Corpo Do(CriaturaBrain b)
        {
            if (corpos != null) foreach (Corpo c in corpos) if (c.Brain == b) return c;
            return null;
        }

        void Limpar()
        {
            if (corpos == null) return;
            foreach (Corpo c in corpos)
                if (c != null && c.Go != null)
                {
                    c.Health.DamageFilter = null;
                    Renderer r = c.Go.GetComponent<Renderer>();
                    if (r != null && r.sharedMaterial != null && r.sharedMaterial.name.StartsWith("COE_Criatura_")) Destroy(r.sharedMaterial);
                    Destroy(c.Go);
                }
            corpos = null;
        }

        void OnDisable()
        {
            // O MissaoHud desliga o gatilho quando o objetivo deixa de ser o pendente: luta pela metade nao fica na cena.
            estado = null;
            Limpar();
        }
    }
}
