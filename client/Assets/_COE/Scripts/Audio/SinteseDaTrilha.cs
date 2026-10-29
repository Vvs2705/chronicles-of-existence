using System;

namespace COE
{
    /// <summary>Qual musica toca. Valor congelado: e indice de clip, nova entra no fim.</summary>
    public enum TrilhaMusical { VilaDia = 0, VilaNoite = 1, Bosque = 2, Combate = 3, Menu = 4, Limiar = 5 }

    /// <summary>A trilha por momento (GDD 09.9 "identidade musical intimista", 09.10 "motivo do Limiar"): seis lacos curtos,
    /// cada um com o seu andamento, tom e instrumentos, todos com o mesmo DNA (caixinha, harpa, flauta, pad) e o MOTIVO DO
    /// LIMIAR (mi-si-la-fa#: sobe uma quinta e desce sem resolver) costurando menu, bosque e o proprio Limiar.
    /// Mesmas regras do resto da Sintese: C# PURO, deterministico, mono a <see cref="Taxa"/>, sem arquivo nem licenca; toda
    /// nota e "circular" (a cauda que passa do fim volta no comeco), entao cada trilha emenda sem estalo.
    /// ponytail: instrumentos de seno com harmonicos e envelope, sem reverb de verdade (um eco circular no bosque e no
    /// Limiar). Trilha gravada ou composta por gente entra pela PROVENIENCIA trocando so o clip de cada TrilhaMusical.</summary>
    public static partial class Sintese
    {
        /// <summary>O motivo do Limiar em mi (MIDI): E5 B5 A5 F#5.</summary>
        public static readonly int[] MotivoDoLimiar = { 76, 83, 81, 78 };

        struct Forma { public float Bpm; public int Compassos; public Forma(float bpm, int compassos) { Bpm = bpm; Compassos = compassos; } }

        static Forma FormaDe(TrilhaMusical t)
        {
            switch (t)
            {
                case TrilhaMusical.VilaNoite: return new Forma(80f, 8);
                case TrilhaMusical.Bosque:    return new Forma(70f, 6);
                case TrilhaMusical.Combate:   return new Forma(132f, 8);
                case TrilhaMusical.Menu:      return new Forma(84f, 8);
                case TrilhaMusical.Limiar:    return new Forma(60f, 5);
                default:                      return new Forma(Bpm, 8);   // a caixinha de sempre
            }
        }

        /// <summary>Duracao de uma volta (compassos de 4 tempos).</summary>
        public static float SegundosDaTrilha(TrilhaMusical t)
        {
            Forma f = FormaDe(t);
            return f.Compassos * 4 * 60f / f.Bpm;
        }

        public static float[] Trilha(TrilhaMusical t)
        {
            Forma f = FormaDe(t);
            float c = 60f / f.Bpm / 2f;   // colcheia
            var buf = new float[(int)(SegundosDaTrilha(t) * Taxa)];
            switch (t)
            {
                case TrilhaMusical.VilaNoite: VilaNoite(buf, c); break;
                case TrilhaMusical.Bosque:    Bosque(buf, c); break;
                case TrilhaMusical.Combate:   Combate(buf, c); break;
                case TrilhaMusical.Menu:      Menu(buf, c); break;
                case TrilhaMusical.Limiar:    Limiar(buf, c); break;
                default:                      VilaDia(buf, c); break;
            }
            return Normalizar(buf, 0.8f);
        }

        // ---------------------------------------------------------------- as trilhas

        /// <summary>Dia na vila (96 bpm, do maior): a caixinha de sempre por cima de harpa em contratempo e um pad macio.</summary>
        static void VilaDia(float[] buf, float c)
        {
            for (int i = 0; i < Melodia.Length; i++)
                if (Melodia[i] >= 0) Nota(buf, i * c, Frequencia(Melodia[i]), 1.4f, 3f, 0.5f, true);
            int[][] acordes = { C, Am, F, G, C, Am, F, G };
            for (int b = 0; b < acordes.Length; b++)
            {
                Nota(buf, b * 8 * c, Frequencia(Baixo[b]), 1.2f, 1.8f, 0.3f, true);
                Nota(buf, (b * 8 + 4) * c, Frequencia(Baixo[b]), 1.2f, 1.8f, 0.3f, true);
                Pad(buf, b * 8 * c, 8 * c, acordes[b], 0.07f);
                for (int k = 1; k < 8; k += 2) Harpa(buf, (b * 8 + k) * c, acordes[b][(k / 2) % 3], 0.12f);
            }
        }

        /// <summary>Noite (80 bpm, la menor): flauta de ninar sobre harpa grave em seminimas, pad baixo e baixo longo.</summary>
        static void VilaNoite(float[] buf, float c)
        {
            int[][] acordes = { Am, F, C, G, Am, F, Dm, E };
            int[] raiz = { 45, 41, 48, 43, 45, 41, 38, 40 };
            for (int b = 0; b < acordes.Length; b++)
            {
                Pad(buf, b * 8 * c, 8 * c, Oitava(acordes[b], -12), 0.08f);
                Tom(buf, b * 8 * c, Frequencia(raiz[b]), 8 * c, 0.02f, 0.5f, 0.25f, Grave, 0f);
                int[] arpejo = { raiz[b] + 12, acordes[b][1], acordes[b][2], acordes[b][1] };
                for (int k = 0; k < 4; k++) Harpa(buf, (b * 8 + 2 * k) * c, arpejo[k], 0.14f);
            }
            // (colcheia de inicio, nota, duracao em colcheias)
            int[,] flauta =
            {
                { 0, 76, 3 }, { 4, 74, 2 }, { 6, 72, 2 },
                { 8, 72, 4 }, { 12, 69, 4 },
                { 16, 67, 2 }, { 18, 72, 2 }, { 20, 76, 4 },
                { 24, 74, 6 }, { 30, 71, 2 },
                { 32, 76, 3 }, { 36, 79, 2 }, { 38, 77, 2 },
                { 40, 76, 4 }, { 44, 72, 4 },
                { 48, 74, 2 }, { 50, 72, 2 }, { 52, 69, 4 },
                { 56, 68, 6 },
            };
            Frase(buf, c, flauta, 0.3f);
        }

        /// <summary>Bosque dos Sussurros (70 bpm, re dorico): pad que respira, harpa esparsa com eco e o motivo do Limiar
        /// passando de longe na flauta (o bosque lembra o que o jogador viu antes de nascer).</summary>
        static void Bosque(float[] buf, float c)
        {
            int[][] acordes = { new[] { 50, 57, 60, 65 }, new[] { 52, 59, 62, 67 }, new[] { 48, 55, 59, 64 },
                                new[] { 50, 57, 60, 65 }, new[] { 53, 57, 60, 64 }, new[] { 48, 55, 60, 64 } };
            for (int b = 0; b < acordes.Length; b++)
            {
                Pad(buf, b * 8 * c, 8 * c, acordes[b], 0.08f);
                Tom(buf, b * 8 * c, Frequencia(38), 8 * c, 0.4f, 0.1f, 0.2f, Grave, 0f);   // re grave de chao
            }
            var eco = new float[buf.Length];
            var r = new Random(97);
            int[] escala = { 62, 64, 67, 69, 72, 74, 76 };   // re dorico sem as notas que pesam
            for (int k = 0; k < acordes.Length * 8; k++)
                if (r.NextDouble() < 0.35) Harpa(eco, k * c, escala[r.Next(escala.Length)], 0.12f + 0.12f * (float)r.NextDouble());
            int[,] motivo = { { 24, 69, 2 }, { 26, 76, 3 }, { 29, 74, 1 }, { 30, 71, 6 } };   // A E D B: o motivo em re
            Frase(eco, c, motivo, 0.16f);
            Eco(eco, c, 0.45f);
            for (int i = 0; i < buf.Length; i++) buf[i] += eco[i];
        }

        /// <summary>Treino e luta (132 bpm, re menor): bumbo, caixa e chimbal, baixo em ostinato e uma harpa seca no tema.
        /// Urgente sem ser epico: e treino de crianca com espada de madeira.</summary>
        static void Combate(float[] buf, float c)
        {
            var r = new Random(131);
            int[][] acordes = { Dm, Dm, Bb, Bb, C, C, A, A };
            int[] raiz = { 38, 38, 34, 34, 36, 36, 33, 33 };
            int[] ostinato = { 0, 0, 12, 0, 7, 0, 10, 12 };
            for (int b = 0; b < acordes.Length; b++)
            {
                float t0 = b * 8 * c;
                if (b % 2 == 0) Pad(buf, t0, 16 * c, acordes[b], 0.06f);
                for (int k = 0; k < 8; k++)
                {
                    float t = t0 + k * c;
                    Tom(buf, t, Frequencia(raiz[b] + ostinato[k]), c * 0.9f, 0.005f, 7f, 0.3f, Grave, 0f);
                    if (k == 0 || k == 3 || k == 5) Bumbo(buf, t, 0.7f);
                    if (k == 2 || k == 6) Caixa(buf, t, 0.35f, r);
                    Chimbal(buf, t, k % 2 == 0 ? 0.07f : 0.045f, r);
                }
            }
            int[,] tema =
            {
                { 0, 74, 2 }, { 2, 77, 1 }, { 3, 76, 1 }, { 4, 74, 2 }, { 6, 72, 1 }, { 7, 74, 1 }, { 8, 69, 4 }, { 12, 72, 2 }, { 14, 74, 2 },
                { 16, 74, 2 }, { 18, 77, 1 }, { 19, 79, 1 }, { 20, 77, 2 }, { 22, 74, 2 }, { 24, 70, 4 }, { 28, 72, 2 }, { 30, 74, 2 },
                { 32, 76, 2 }, { 34, 79, 1 }, { 35, 81, 1 }, { 36, 79, 2 }, { 38, 76, 2 }, { 40, 72, 4 }, { 44, 76, 2 }, { 46, 79, 2 },
                { 48, 81, 2 }, { 50, 79, 1 }, { 51, 77, 1 }, { 52, 76, 2 }, { 54, 73, 2 }, { 56, 76, 6 },
            };
            for (int i = 0; i < tema.GetLength(0); i++)
                Tom(buf, tema[i, 0] * c, Frequencia(tema[i, 1]), tema[i, 2] * c, 0.004f, 6f, 0.28f, Brilho, 0f);
        }

        /// <summary>Titulo e nascimento (84 bpm, mi menor): a flauta abre com o motivo do Limiar e responde; harpa sobe e
        /// desce no acorde; o si do ultimo compasso puxa de volta para o mi do comeco.</summary>
        static void Menu(float[] buf, float c)
        {
            int[][] acordes = { Em, C, G, D, Em, C, Am, B };
            int[] raiz = { 40, 36, 43, 38, 40, 36, 33, 35 };
            for (int b = 0; b < acordes.Length; b++)
            {
                Pad(buf, b * 8 * c, 8 * c, acordes[b], 0.08f);
                Tom(buf, b * 8 * c, Frequencia(raiz[b]), 4 * c, 0.01f, 1f, 0.22f, Grave, 0f);
                Tom(buf, (b * 8 + 4) * c, Frequencia(raiz[b]), 4 * c, 0.01f, 1f, 0.18f, Grave, 0f);
                int[] sobe = { acordes[b][0] + 12, acordes[b][1] + 12, acordes[b][2] + 12, acordes[b][1] + 12 };
                for (int k = 0; k < 8; k++) Harpa(buf, (b * 8 + k) * c, sobe[k % 4], 0.08f);
            }
            int[,] flauta =
            {
                { 0, 76, 2 }, { 2, 83, 4 }, { 6, 81, 2 }, { 8, 78, 8 },
                { 16, 79, 2 }, { 18, 78, 2 }, { 20, 76, 4 }, { 24, 74, 4 }, { 28, 78, 4 },
                { 32, 76, 2 }, { 34, 83, 4 }, { 38, 84, 2 }, { 40, 83, 4 }, { 44, 81, 4 },
                { 48, 79, 2 }, { 50, 78, 2 }, { 52, 76, 4 }, { 56, 75, 8 },
            };
            Frase(buf, c, flauta, 0.3f);
        }

        /// <summary>O Limiar (60 bpm, mi maior): pad alto e parado, re-de-chao grave, e o motivo em sinos lentos que ecoam.
        /// Nao resolve: e a pergunta antes de nascer.</summary>
        static void Limiar(float[] buf, float c)
        {
            int[][] acordes = { new[] { 64, 68, 71, 78 }, new[] { 61, 64, 68, 73 }, new[] { 57, 61, 64, 71 },
                                new[] { 59, 63, 66, 71 }, new[] { 64, 68, 71, 78 } };
            for (int b = 0; b < acordes.Length; b++)
            {
                Pad(buf, b * 8 * c, 8 * c, acordes[b], 0.07f);
                Tom(buf, b * 8 * c, Frequencia(40), 8 * c, 0.6f, 0.05f, 0.18f, Grave, 0f);
            }
            var sinos = new float[buf.Length];
            for (int i = 0; i < MotivoDoLimiar.Length; i++)
            {
                Nota(sinos, i * 2 * c, Frequencia(MotivoDoLimiar[i] + 12), 4f, 1.2f, 0.3f, true);
                Nota(sinos, (20 + i * 3) * c, Frequencia(MotivoDoLimiar[i]), 4f, 1.2f, 0.25f, true);
            }
            Eco(sinos, 1.5f * c, 0.5f);
            for (int i = 0; i < buf.Length; i++) buf[i] += sinos[i];
        }

        // ---------------------------------------------------------------- acordes (MIDI) e timbres

        static readonly int[] C = { 60, 64, 67 }, Am = { 57, 60, 64 }, F = { 57, 60, 65 }, G = { 55, 59, 62 }, Dm = { 57, 62, 65 },
            E = { 56, 59, 64 }, Bb = { 58, 62, 65 }, A = { 57, 61, 64 }, Em = { 55, 59, 64 }, D = { 54, 57, 62 }, B = { 54, 59, 63 };

        static readonly float[] Grave = { 1f, 0.35f, 0.1f };              // baixo
        static readonly float[] Corda = { 1f, 0.5f, 0.25f, 0.12f };       // harpa
        static readonly float[] Brilho = { 1f, 0.6f, 0.4f, 0.25f, 0.12f }; // harpa seca do combate
        static readonly float[] Sopro = { 1f, 0.12f, 0.04f };             // flauta
        static readonly float[] Macio = { 1f, 0.2f };                     // pad

        static int[] Oitava(int[] acorde, int d)
        {
            var o = new int[acorde.Length];
            for (int i = 0; i < o.Length; i++) o[i] = acorde[i] + d;
            return o;
        }

        static void Harpa(float[] buf, float inicio, int midi, float ganho)
        {
            Tom(buf, inicio, Frequencia(midi), 1.6f, 0.003f, 2.6f, ganho, Corda, 0f);
        }

        /// <summary>Pad: cada nota em duas vozes levemente desafinadas (o "coro"), ataque e soltura lentos.</summary>
        static void Pad(float[] buf, float inicio, float dur, int[] acorde, float ganho)
        {
            foreach (int m in acorde)
            {
                float f = Frequencia(m);
                Tom(buf, inicio, f * 0.9985f, dur, 0.5f, 0.15f, ganho, Macio, 0f);
                Tom(buf, inicio, f * 1.0015f, dur, 0.5f, 0.15f, ganho, Macio, 0f);
            }
        }

        /// <summary>Linha de flauta: (colcheia, nota, colcheias) por linha; vibrato que entra devagar.</summary>
        static void Frase(float[] buf, float c, int[,] notas, float ganho)
        {
            for (int i = 0; i < notas.GetLength(0); i++)
                Tom(buf, notas[i, 0] * c, Frequencia(notas[i, 1]), notas[i, 2] * c, 0.06f, 0.35f, ganho, Sopro, 0.005f);
        }

        /// <summary>Nota generica e circular: soma de harmonicos com ataque, decaimento exponencial, soltura de ate 0,25 s no
        /// fim da duracao (sem clique) e vibrato opcional (5 Hz, entrando em 0,3 s).</summary>
        static void Tom(float[] buf, float inicio, float freq, float dur, float ataque, float decaimento, float ganho,
                        float[] harmonicos, float vibrato)
        {
            int i0 = (int)(inicio * Taxa), n = (int)(dur * Taxa);
            float soltura = Math.Min(0.25f, dur * 0.3f);
            double fase = 0;
            for (int k = 0; k < n; k++)
            {
                float t = k / (float)Taxa;
                float env = Math.Min(1f, t / ataque) * (float)Math.Exp(-t * decaimento) * Math.Min(1f, (dur - t) / soltura);
                float f = vibrato > 0f ? freq * (1f + vibrato * Math.Min(1f, t / 0.3f) * (float)Math.Sin(2 * Math.PI * 5.0 * t)) : freq;
                fase += 2 * Math.PI * f / Taxa;
                // sen((h+1)x) pela recorrencia de Chebyshev: um seno e um cosseno por amostra, nao um seno por harmonico
                double s1 = Math.Sin(fase), dois = 2 * Math.Cos(fase), antes = 0, agora = s1, s = 0;
                for (int h = 0; h < harmonicos.Length; h++)
                {
                    s += harmonicos[h] * agora;
                    double prox = dois * agora - antes;
                    antes = agora;
                    agora = prox;
                }
                buf[(i0 + k) % buf.Length] += ganho * env * (float)s;
            }
        }

        /// <summary>Bumbo: seno que cai de 110 para 45 Hz e um estalo no ataque.</summary>
        static void Bumbo(float[] buf, float inicio, float ganho)
        {
            int i0 = (int)(inicio * Taxa), n = (int)(0.3f * Taxa);
            double fase = 0;
            for (int k = 0; k < n; k++)
            {
                float t = k / (float)Taxa;
                fase += 2 * Math.PI * (45f + 65f * (float)Math.Exp(-t * 25f)) / Taxa;
                float env = Math.Min(1f, t / 0.002f) * (float)Math.Exp(-t * 11f);
                buf[(i0 + k) % buf.Length] += ganho * env * (float)Math.Sin(fase);
            }
        }

        /// <summary>Caixa: corpo de 190 Hz e ruido que some rapido.</summary>
        static void Caixa(float[] buf, float inicio, float ganho, Random r)
        {
            int i0 = (int)(inicio * Taxa), n = (int)(0.18f * Taxa);
            for (int k = 0; k < n; k++)
            {
                float t = k / (float)Taxa;
                float corpo = (float)Math.Sin(2 * Math.PI * 190.0 * t) * (float)Math.Exp(-t * 30f);
                float ruido = Ruido(r) * (float)Math.Exp(-t * 22f);
                buf[(i0 + k) % buf.Length] += ganho * Math.Min(1f, t / 0.001f) * (0.5f * corpo + 0.7f * ruido);
            }
        }

        /// <summary>Chimbal: ruido sem graves (ruido menos a media dele), curtinho.</summary>
        static void Chimbal(float[] buf, float inicio, float ganho, Random r)
        {
            int i0 = (int)(inicio * Taxa), n = (int)(0.05f * Taxa);
            float lp = 0f;
            for (int k = 0; k < n; k++)
            {
                float t = k / (float)Taxa, x = Ruido(r);
                lp += (x - lp) * 0.5f;
                buf[(i0 + k) % buf.Length] += ganho * (x - lp) * (float)Math.Exp(-t * 60f);
            }
        }

        /// <summary>Eco circular (atraso em segundos, realimentacao): duas voltas pelo buffer chegam no regime do laco, entao a
        /// emenda continua sem estalo.</summary>
        static void Eco(float[] buf, float atraso, float realimentacao)
        {
            int n = buf.Length, d = (int)(atraso * Taxa);
            var seco = (float[])buf.Clone();
            for (int volta = 0; volta < 2; volta++)
                for (int i = 0; i < n; i++)
                    buf[i] = seco[i] + realimentacao * buf[(i - d + n) % n];
        }
    }
}
