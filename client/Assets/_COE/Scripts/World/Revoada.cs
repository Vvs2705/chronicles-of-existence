using UnityEngine;

namespace COE
{
    /// <summary>Pose de um bicho de bando que voa em roda (passaros sobre a vila, borboletas na horta). C# PURO e funcao
    /// do tempo: cada membro gira em volta do centro com o proprio atraso, o raio "respira" e a altura ondula, entao o
    /// bando nao parece um disco rigido. A posicao e relativa ao centro do bando.</summary>
    public static class Revoada
    {
        /// <summary>i-esimo de n membros no instante t. raio e altura em metros; volta = segundos de uma volta inteira.</summary>
        public static Vector3 Posicao(int i, int n, float t, float raio, float altura, float volta)
        {
            float w = 2f * Mathf.PI / Mathf.Max(0.1f, volta);
            float atraso = n <= 0 ? 0f : i * 0.9f / n;   // o bando ocupa ~1/7 da volta, em fila solta
            float a = w * t - atraso * 2f * Mathf.PI * 0.15f + i * 0.37f;
            float r = raio * (1f + 0.18f * Mathf.Sin(t * 0.7f + i * 1.3f));
            float y = altura * 0.12f * Mathf.Sin(t * 1.3f + i * 2.1f);
            return new Vector3(Mathf.Cos(a) * r, y, Mathf.Sin(a) * r);
        }

        /// <summary>Para onde o membro olha: a derivada numerica da posicao, no plano (nunca zero).</summary>
        public static Vector3 Direcao(int i, int n, float t, float raio, float altura, float volta)
        {
            Vector3 d = Posicao(i, n, t + 0.05f, raio, altura, volta) - Posicao(i, n, t, raio, altura, volta);
            d.y = 0f;
            return d.sqrMagnitude < 1e-8f ? Vector3.forward : d.normalized;
        }

        /// <summary>Angulo da asa (graus) para o bater: 0 = aberta na horizontal.</summary>
        public static float Asa(int i, float t, float batidasPorSegundo)
        {
            return 45f * Mathf.Sin(2f * Mathf.PI * batidasPorSegundo * t + i * 1.7f);
        }
    }
}
