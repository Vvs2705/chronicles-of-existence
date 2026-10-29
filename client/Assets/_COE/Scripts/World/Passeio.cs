using UnityEngine;

namespace COE
{
    /// <summary>Onde um morador esta num instante do passeio: posicao, para onde olha e se esta andando.</summary>
    public struct PoseDePasseio
    {
        public Vector3 Posicao;
        public Vector3 Direcao;   // no plano, normalizada; nunca zero
        public bool Andando;
    }

    /// <summary>O passeio de um morador de Auren (vida da vila, nao NPC de historia): um circuito fechado de pontos na rua,
    /// percorrido a velocidade constante, com uma pausa em cada ponto ("olha a vitrine, cumprimenta alguem"). Dois pontos =
    /// vai e volta. C# PURO e sem estado: a pose e funcao do tempo, entao a cena sai igual toda vez e o teste nao precisa
    /// de frames.
    ///
    /// ponytail: linha reta entre pontos autorados (as ruas de Auren sao retas e o chao e plano), sem NavMesh nem desvio.
    /// Quem garante que o trecho nao atravessa parede e o teste da tabela (VidaSceneTests). NavMesh entra quando um
    /// morador precisar sair da rua.</summary>
    public sealed class Passeio
    {
        readonly Vector3[] pontos;
        readonly float velocidade, pausa;
        readonly float[] inicio;   // instante em que comeca a pausa do ponto i
        readonly float[] comprimento;

        /// <summary>Segundos de uma volta inteira (pausas incluidas).</summary>
        public readonly float Duracao;

        /// <summary>null = sem passeio (rota vazia: o morador esta em casa neste periodo).</summary>
        public static Passeio De(Vector3[] pontos, float velocidade, float pausa)
        {
            if (pontos == null || pontos.Length == 0) return null;
            return new Passeio(pontos, velocidade, pausa);
        }

        Passeio(Vector3[] pontos, float velocidade, float pausa)
        {
            this.pontos = (Vector3[])pontos.Clone();
            this.velocidade = Mathf.Max(0.05f, velocidade);
            this.pausa = Mathf.Max(0f, pausa);
            int n = this.pontos.Length;
            inicio = new float[n];
            comprimento = new float[n];
            float t = 0f;
            for (int i = 0; i < n; i++)
            {
                inicio[i] = t;
                comprimento[i] = n == 1 ? 0f : Plano(this.pontos[(i + 1) % n] - this.pontos[i]).magnitude;
                t += this.pausa + comprimento[i] / this.velocidade;
            }
            Duracao = Mathf.Max(t, 0.01f);
        }

        public PoseDePasseio Em(float tempo)
        {
            float t = Mathf.Repeat(tempo, Duracao);
            int n = pontos.Length;
            int i = n - 1;
            while (i > 0 && inicio[i] > t) i--;

            Vector3 a = pontos[i], b = pontos[(i + 1) % n];
            Vector3 dir = Plano(b - a);
            dir = dir.sqrMagnitude < 1e-6f ? Vector3.forward : dir.normalized;
            float local = t - inicio[i];
            if (local < pausa || comprimento[i] < 1e-4f)
                return new PoseDePasseio { Posicao = a, Direcao = dir, Andando = false };
            float u = Mathf.Clamp01((local - pausa) * velocidade / comprimento[i]);
            return new PoseDePasseio { Posicao = Vector3.Lerp(a, b, u), Direcao = dir, Andando = true };
        }

        /// <summary>Raio em que o morador para e da passagem a quem esta na frente dele (o jogador).</summary>
        public const float RaioDePassagem = 1.4f;

        /// <summary>Morador e figurante sem colisor (nao barra percurso nenhum); em vez de atravessar o jogador, para quando
        /// ele esta a menos de <see cref="RaioDePassagem"/> NA FRENTE. Atras ou do lado, segue.</summary>
        public static bool DaPassagem(Vector3 eu, Vector3 frente, Vector3 outro)
        {
            Vector3 d = Plano(outro - eu);
            return d.sqrMagnitude < RaioDePassagem * RaioDePassagem && Vector3.Dot(Plano(frente), d) > 0f;
        }

        static Vector3 Plano(Vector3 v) { v.y = 0f; return v; }
    }
}
