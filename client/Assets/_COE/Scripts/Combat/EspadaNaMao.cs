using UnityEngine;

namespace COE
{
    /// <summary>B15 / ADR-0010 §3: a espada do treino na mao direita do jogador, quando ela esta no inventario
    /// (Inventario.EspadaNaMao). So mostra: a estatistica e a mesma nas duas (CombatMoves.LaminaEspadaDeMadeira) e quem
    /// entrega e o MissaoMundo. A marcada leva a plaqueta de cobre no punho (ADR-0010 §4, terracota #A86D52).
    ///
    /// ponytail: espada de primitivas (lamina, guarda, cabo), material copiado do proprio corpo (o shader do jogo ja esta
    /// no build) e reavaliada 4x/s. Vira modelo do Tripo com a arte de objetos separados (CONTRATO_V1 §6 passo 2);
    /// `rotacao` e `posicao` sao o ajuste fino da pega, por osso de rig diferente.</summary>
    public class EspadaNaMao : MonoBehaviour
    {
        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo PartidaSetup; vazio = a do SaveState.")]
        [SerializeField] Partida partida;
        [Tooltip("Pega: rotacao local da espada no osso da mao (graus).")]
        [SerializeField] Vector3 rotacao = new Vector3(0f, 0f, -90f);
        [Tooltip("Pega: posicao local no osso da mao (m), o cabo dentro da mao.")]
        [SerializeField] Vector3 posicao = new Vector3(0f, 0.08f, 0.02f);

        static readonly Color Madeira = new Color(0.58f, 0.40f, 0.24f);
        static readonly Color MadeiraEscura = new Color(0.36f, 0.24f, 0.14f);
        static readonly Color Cobre = new Color(0xA8 / 255f, 0x6D / 255f, 0x52 / 255f);

        string mostrada;
        GameObject espada;
        float proxima;

        /// <summary>O que esta na mao agora (item id), ou null. O teste le.</summary>
        public string Mostrada { get { return mostrada; } }

        void LateUpdate()
        {
            if (Time.unscaledTime < proxima) return;
            proxima = Time.unscaledTime + 0.25f;
            GameSession s = Partida.De(partida);
            string devida = s == null || s.Save == null ? null : Inventario.EspadaNaMao(s.Save.inventario);
            if (devida == mostrada && (devida == null) == (espada == null)) return;
            if (espada != null) Destroy(espada);
            espada = devida == null ? null : Montar(devida == Inventario.EspadaMarcada);
            mostrada = devida;
        }

        GameObject Montar(bool marcada)
        {
            Animator a = GetComponentInChildren<Animator>();
            Transform mao = a != null && a.isHuman ? a.GetBoneTransform(HumanBodyBones.RightHand) : null;
            Renderer corpo = GetComponentInChildren<SkinnedMeshRenderer>();
            if (corpo == null) corpo = GetComponentInChildren<Renderer>();

            var raiz = new GameObject("EspadaDoTreino");
            raiz.transform.SetParent(mao != null ? mao : transform, false);
            raiz.transform.localPosition = mao != null ? posicao : new Vector3(0.3f, 0.7f, 0.2f);
            raiz.transform.localRotation = Quaternion.Euler(rotacao);
            // O osso pode vir escalado do FBX: a espada mede em metros do mundo.
            Vector3 l = raiz.transform.lossyScale;
            raiz.transform.localScale = new Vector3(1f / Mathf.Max(1e-4f, l.x), 1f / Mathf.Max(1e-4f, l.y), 1f / Mathf.Max(1e-4f, l.z));

            float lamina = CombatMoves.LaminaEspadaDeMadeira;
            Peca(raiz.transform, "Cabo", new Vector3(0f, 0f, 0f), new Vector3(0.03f, 0.14f, 0.03f), MadeiraEscura, corpo);
            Peca(raiz.transform, "Guarda", new Vector3(0f, 0.08f, 0f), new Vector3(0.14f, 0.025f, 0.04f), MadeiraEscura, corpo);
            Peca(raiz.transform, "Lamina", new Vector3(0f, 0.09f + lamina * 0.5f, 0f), new Vector3(0.05f, lamina, 0.018f), Madeira, corpo);
            if (marcada) Peca(raiz.transform, "PlaquetaDeCobre", new Vector3(0f, 0.01f, 0.017f), new Vector3(0.035f, 0.05f, 0.006f), Cobre, corpo);
            return raiz;
        }

        static void Peca(Transform pai, string nome, Vector3 pos, Vector3 tamanho, Color cor, Renderer modelo)
        {
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Cube);
            p.name = nome;
            Destroy(p.GetComponent<Collider>());   // so visual: o golpe e do PlayerCombat
            p.transform.SetParent(pai, false);
            p.transform.localPosition = pos;
            p.transform.localScale = tamanho;
            var r = p.GetComponent<MeshRenderer>();
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            if (modelo != null && modelo.sharedMaterial != null)
            {
                var m = new Material(modelo.sharedMaterial) { name = nome };
                if (m.HasProperty("_BaseMap")) m.SetTexture("_BaseMap", null);
                if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", cor);
                if (m.HasProperty("_Color")) m.SetColor("_Color", cor);
                r.sharedMaterial = m;
            }
        }
    }
}
