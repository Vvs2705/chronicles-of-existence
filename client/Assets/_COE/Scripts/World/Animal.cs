using UnityEngine;

namespace COE
{
    public enum Especie { Galinha = 0, Cachorro = 1, Gato = 2, Porco = 3 }

    /// <summary>Bicho solto de Auren (galinha, cachorro, gato, porco). So orquestra: a regra e o Ciscar; aqui ele mede o
    /// jogador, anda o corpo e anima as partes por codigo (cabeca bica ou balanca, rabo abana, perna vai e volta no passo,
    /// gato deita quando para). Sem colisor e fora do save, como o
    /// Morador. Some nos periodos fora de <see cref="periodos"/> (galinha recolhe ao anoitecer).
    /// ponytail: corpo de primitivas montado pelo VidaSceneSetup; bicho com malha entra pelos portoes do ADR-0002
    /// (criatura: ficha G1, concept G2, licenca G3).</summary>
    public class Animal : MonoBehaviour
    {
        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo gerador (PartidaSetup); vazio = a do SaveState.")]
        [SerializeField] Partida partida;
        [SerializeField] Especie especie;
        [SerializeField] Transform jogador;
        [SerializeField] Transform corpo;
        [SerializeField] Transform cabeca;
        [SerializeField] Transform rabo;
        [Tooltip("Pivos das pernas, no quadril. Quatro patas: frente/tras alternando em diagonal (0 e 3 juntas).")]
        [SerializeField] Transform[] pernas = new Transform[0];
        [Tooltip("Centro do circulo onde o bicho anda (o terreiro, o galinheiro); ele nasce onde o objeto esta.")]
        [SerializeField] Vector3 centro;
        [SerializeField] float raio = 3f;
        [Tooltip("So quem segue (cachorro): ate onde vai atras do jogador, a partir do centro.")]
        [SerializeField] float raioDeSeguir;
        [SerializeField] int semente = 1;
        [Tooltip("Periodos em que o bicho aparece: bit 0 manha, 1 tarde, 2 noite.")]
        [SerializeField] int periodos = 7;

        Ciscar regra;
        Vector3 ultimoJogador;
        Quaternion cabecaRepouso, raboRepouso;
        float tempo;

        public Vector3 Centro { get { return centro; } }
        public float Raio { get { return raio; } }
        public Ciscar Regra { get { if (regra == null) Criar(); return regra; } }
        public Especie Especie { get { return especie; } }

        /// <summary>Velocidade de passeio de cada especie (m/s). HIPOTESE: leitura de bicho de vila, calibrar no aparelho.</summary>
        public static float Velocidade(Especie e)
        {
            return e == Especie.Galinha ? 0.6f : e == Especie.Cachorro ? 1.3f : e == Especie.Porco ? 0.45f : 0.7f;
        }

        /// <summary>Giro da perna i no passo (graus em X): diagonal 0 e 3 juntas contra 1 e 2; duas pernas alternam.
        /// Parado = 0. Publico para o teste.</summary>
        public static float Passada(int i, float tempo, bool anda, Especie e)
        {
            if (!anda) return 0f;
            float lado = i == 0 || i == 3 ? 1f : -1f;
            float ritmo = e == Especie.Galinha ? 14f : e == Especie.Porco ? 8f : 11f;
            return lado * 28f * Mathf.Sin(tempo * ritmo);
        }

        void Criar()
        {
            regra = new Ciscar(centro, raio, Velocidade(especie), semente, especie == Especie.Cachorro, raioDeSeguir);
            regra.Por(transform.position);
            if (cabeca != null) cabecaRepouso = cabeca.localRotation;
            if (rabo != null) raboRepouso = rabo.localRotation;
            if (jogador != null) ultimoJogador = jogador.position;
        }

        void Start() { Criar(); }

        void Update()
        {
            SaveData save = Partida.De(partida).Save;
            Atualizar(TimeOfDayCycle.Atual(save == null ? null : save.life), Time.deltaTime);
        }

        /// <summary>Um passo do bicho. Publico porque Update nao roda em teste de Editor.</summary>
        public void Atualizar(TimeOfDay agora, float dt)
        {
            if (regra == null) Criar();
            bool visivel = Morador.NoPeriodo(periodos, agora);
            if (corpo != null && corpo.gameObject.activeSelf != visivel) corpo.gameObject.SetActive(visivel);
            if (!visivel || dt <= 0f) return;

            Vector3 j = jogador != null ? jogador.position : transform.position + Vector3.one * 100f;
            float vj = jogador != null ? Vector3.Distance(Plano(j), Plano(ultimoJogador)) / dt : 0f;
            ultimoJogador = j;
            regra.Passo(dt, j, vj);
            tempo += dt;

            transform.position = new Vector3(regra.Posicao.x, transform.position.y, regra.Posicao.z);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(regra.Direcao), 400f * dt);
            Animar(regra.Estado);
        }

        void Animar(EstadoDoBicho e)
        {
            bool anda = e == EstadoDoBicho.Andando || e == EstadoDoBicho.Fugindo || e == EstadoDoBicho.Seguindo;
            if (cabeca != null)
            {
                float pitch = e == EstadoDoBicho.Bicando ? 35f + 30f * Mathf.Sin(tempo * 14f)        // bica o chao
                            : anda ? (especie == Especie.Galinha ? 12f * Mathf.Sin(tempo * 12f) : 4f * Mathf.Sin(tempo * 9f))
                            : 6f * Mathf.Sin(tempo * 1.5f);                                            // respira, olha
                float yaw = anda ? 0f : 25f * Mathf.Sin(tempo * 0.6f + semente);
                cabeca.localRotation = cabecaRepouso * Quaternion.Euler(pitch, yaw, 0f);
            }
            if (rabo != null)
            {
                float abana = especie == Especie.Cachorro
                    ? (e == EstadoDoBicho.Seguindo ? 40f * Mathf.Sin(tempo * 18f) : 15f * Mathf.Sin(tempo * 6f))
                    : 10f * Mathf.Sin(tempo * 1.2f);
                rabo.localRotation = raboRepouso * Quaternion.Euler(0f, abana, 0f);
            }
            for (int i = 0; i < pernas.Length; i++)
                if (pernas[i] != null) pernas[i].localRotation = Quaternion.Euler(Passada(i, tempo, anda, especie), 0f, 0f);
            if (corpo != null)   // passo: o corpo sobe e desce um pouco; gato parado deita
            {
                float y = anda ? 0.03f * Mathf.Abs(Mathf.Sin(tempo * 10f))
                        : especie == Especie.Gato && e == EstadoDoBicho.Parado ? -0.09f : 0f;
                corpo.localPosition = new Vector3(0f, y, 0f);
            }
        }

        static Vector3 Plano(Vector3 v) { v.y = 0f; return v; }
    }
}
