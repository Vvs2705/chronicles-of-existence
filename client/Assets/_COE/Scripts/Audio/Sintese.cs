using System;

namespace COE
{
    public enum Som { Clique = 0, Passo = 1, Golpe = 2, Magia = 3, Missao = 4 }

    /// <summary>Gera as amostras (mono, -1..1) do som do prototipo. C# PURO e deterministico (ruido com semente fixa):
    /// mesma entrada, mesmos numeros; testavel sem Unity. Nada de arquivo de audio: zero licenca e zero peso no APK.
    /// Musica: caixinha de musica em do maior pentatonico, 8 compassos a 96 bpm, emendando sem clique (as caudas das
    /// notas do fim "dao a volta" para o comeco do buffer).
    /// ponytail: um timbre (seno + 2 harmonicos com decaimento) e uma melodia. Trilha composta e da T013.</summary>
    public static class Sintese
    {
        public const int Taxa = 22050;
        public const float Bpm = 96f;
        const float Colcheia = 60f / Bpm / 2f;

        // MIDI por colcheia; -1 = pausa. Acordes por compasso: C, Am, F, G, C, Am, F, G->C.
        static readonly int[] Melodia =
        {
            76, -1, 79, -1, 81, 79, 76, -1,
            72, -1, 76, -1, 74, 72, 69, -1,
            69, -1, 72, -1, 74, -1, 72, 69,
            67, -1, 69, -1, 72, -1, 74, -1,
            76, 79, 81, -1, 79, 76, 74, -1,
            72, -1, 74, 76, -1, 72, 69, -1,
            69, 72, 74, -1, 72, -1, 69, 67,
            67, -1, -1, -1, 72, -1, -1, -1,
        };
        static readonly int[] Baixo = { 48, 45, 41, 43, 48, 45, 41, 43 };   // um por compasso, na cabeca e no meio

        public static float SegundosDaMusica { get { return Melodia.Length * Colcheia; } }

        public static float Frequencia(int midi) { return 440f * (float)Math.Pow(2.0, (midi - 69) / 12.0); }

        public static float[] Musica()
        {
            int n = (int)(SegundosDaMusica * Taxa);
            var buf = new float[n];
            for (int i = 0; i < Melodia.Length; i++)
                if (Melodia[i] >= 0) Nota(buf, i * Colcheia, Frequencia(Melodia[i]), 1.4f, 3f, 0.5f, true);
            for (int c = 0; c < Baixo.Length; c++)
                for (int meio = 0; meio < 2; meio++)
                    Nota(buf, (c * 8 + meio * 4) * Colcheia, Frequencia(Baixo[c]), 1.2f, 1.8f, 0.3f, true);
            return Normalizar(buf, 0.8f);
        }

        public static float[] Efeito(Som som)
        {
            switch (som)
            {
                case Som.Clique:
                {
                    var b = new float[(int)(0.05f * Taxa)];
                    Nota(b, 0f, 1320f, 0.05f, 60f, 1f, false);
                    return Normalizar(b, 0.5f);
                }
                case Som.Passo:
                {
                    var b = new float[(int)(0.08f * Taxa)];
                    var r = new Random(7);
                    float suave = 0f;
                    for (int i = 0; i < b.Length; i++)
                    {
                        float t = i / (float)Taxa;
                        suave += ((float)r.NextDouble() * 2f - 1f - suave) * 0.15f;   // passa-baixa: terra, nao chiado
                        b[i] = suave * (float)Math.Exp(-t * 45f);
                    }
                    return Normalizar(b, 0.35f);
                }
                case Som.Golpe:
                {
                    var b = new float[(int)(0.18f * Taxa)];
                    var r = new Random(11);
                    double fase = 0;
                    for (int i = 0; i < b.Length; i++)
                    {
                        float t = i / (float)Taxa;
                        float f = 170f - 110f * (t / 0.18f);   // madeira: cai de 170 para 60 Hz
                        fase += 2 * Math.PI * f / Taxa;
                        float env = (float)Math.Exp(-t * 22f);
                        b[i] = env * ((float)Math.Sin(fase) + 0.25f * ((float)r.NextDouble() * 2f - 1f) * (float)Math.Exp(-t * 80f));
                    }
                    return Normalizar(b, 0.7f);
                }
                case Som.Magia:
                {
                    var b = new float[(int)(0.8f * Taxa)];
                    int[] notas = { 81, 86, 91, 96 };   // A5 D6 G6 C7: brilho subindo
                    for (int i = 0; i < notas.Length; i++) Nota(b, i * 0.07f, Frequencia(notas[i]), 0.6f, 6f, 0.6f, false);
                    return Normalizar(b, 0.55f);
                }
                default:   // Missao: do-mi-sol de caixinha
                {
                    var b = new float[(int)(1.4f * Taxa)];
                    int[] notas = { 72, 76, 79, 84 };
                    for (int i = 0; i < notas.Length; i++) Nota(b, i * 0.12f, Frequencia(notas[i]), 1f, 3.5f, 0.7f, false);
                    return Normalizar(b, 0.7f);
                }
            }
        }

        /// <summary>Soma uma nota de caixinha (seno + 2o e 3o harmonicos, ataque de 4 ms, decaimento exponencial).
        /// circular = a cauda que passa do fim volta no comeco (loop sem emenda).</summary>
        static void Nota(float[] buf, float inicio, float freq, float duracao, float decaimento, float ganho, bool circular)
        {
            int i0 = (int)(inicio * Taxa), n = (int)(duracao * Taxa);
            for (int k = 0; k < n; k++)
            {
                int i = i0 + k;
                if (i >= buf.Length) { if (!circular) break; i -= buf.Length; }
                float t = k / (float)Taxa;
                float env = Math.Min(1f, t / 0.004f) * (float)Math.Exp(-t * decaimento);
                double w = 2 * Math.PI * freq * t;
                buf[i] += ganho * env * (float)(Math.Sin(w) + 0.3 * Math.Sin(2 * w) + 0.1 * Math.Sin(3 * w));
            }
        }

        static float[] Normalizar(float[] buf, float pico)
        {
            float max = 0f;
            foreach (float x in buf) max = Math.Max(max, Math.Abs(x));
            if (max < 1e-6f) return buf;
            float k = pico / max;
            for (int i = 0; i < buf.Length; i++) buf[i] *= k;
            return buf;
        }
    }
}
