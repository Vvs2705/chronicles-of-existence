using System;

namespace COE
{
    /// <summary>Som que sai de um lugar do mundo (FonteDeSom, 3D): a bigorna do Borin, o balde do poco, o sussurro do bosque,
    /// os bichos. Valor congelado: e indice de clip, novo entra no fim.</summary>
    public enum SomDoMundo { Martelo = 0, Galinha = 1, Latido = 2, Miado = 3, Agua = 4, Sussurro = 5, Porco = 6 }

    /// <summary>A vila soando (GDD 09.8/09.10: "ambiente de Auren"): o fundo de dia (vento e passarinho), o de noite (grilo,
    /// vento baixo e uma coruja) e os sons de lugar. Mesmas regras do resto da Sintese: C# PURO, deterministico (semente
    /// fixa por som), mono a <see cref="Taxa"/>, sem arquivo nem licenca.
    /// ponytail: passaro, grilo e bicho sao seno, ruido filtrado e envelope; o "ambiente de verdade" (gravacao ou banco
    /// licenciado) entra pela PROVENIENCIA quando houver. O laco do fundo e uma volta de <see cref="SegundosDoAmbiente"/>:
    /// curta o bastante para o APK nao pesar, longa o bastante para o ouvido nao pegar a repeticao de cara.</summary>
    public static partial class Sintese
    {
        public const float SegundosDoAmbiente = 12f;
        public const float SegundosDoSussurro = 6f;

        // ---------------------------------------------------------------- fundo do periodo

        /// <summary>Laco do fundo: de dia vento + passarinhos; de noite vento baixo + dois grilos + uma coruja.</summary>
        public static float[] Ambiente(bool noite)
        {
            int n = (int)(SegundosDoAmbiente * Taxa);
            var r = new Random(noite ? 4231 : 1729);
            float[] buf = Vento(n, noite ? 160f : 300f, noite ? 0.22f : 0.3f, r);

            if (!noite)
            {
                for (int k = 0; k < 16; k++)   // 16 cantos em 12 s, cada um de 2 a 5 pios, todos dentro da volta
                {
                    float inicio = (float)r.NextDouble() * (SegundosDoAmbiente - 0.7f);
                    float f0 = 2400f + (float)r.NextDouble() * 1600f;
                    int pios = 2 + r.Next(4);
                    float ganho = 0.12f + (float)r.NextDouble() * 0.18f;   // perto e longe
                    for (int p = 0; p < pios; p++)
                        Pio(buf, inicio + p * 0.11f, f0, f0 * (1.15f + 0.25f * (float)r.NextDouble()), 0.07f, ganho);
                }
            }
            else
            {
                Grilo(buf, 4300f, 0.55f, 0.00f, 0.10f);
                Grilo(buf, 3900f, 0.70f, 0.23f, 0.07f);
                float coruja = 3f + (float)r.NextDouble() * 5f;
                Pio(buf, coruja, 400f, 370f, 0.28f, 0.22f);
                Pio(buf, coruja + 0.45f, 410f, 340f, 0.55f, 0.22f);
            }
            return Normalizar(buf, 0.7f);
        }

        // ---------------------------------------------------------------- sons de lugar

        public static float[] DoMundo(SomDoMundo som)
        {
            switch (som)
            {
                case SomDoMundo.Martelo: return Martelo();
                case SomDoMundo.Galinha: return Galinha();
                case SomDoMundo.Latido:  return Latido();
                case SomDoMundo.Miado:   return Miado();
                case SomDoMundo.Agua:    return Agua();
                case SomDoMundo.Porco:   return Porco();
                default:                 return Sussurro();
            }
        }

        /// <summary>Bigorna: parciais inarmonicas de metal (1; 2,76; 5,40; 8,93) caindo em tempos diferentes, e o baque.</summary>
        static float[] Martelo()
        {
            var b = new float[(int)(0.9f * Taxa)];
            float[] razao = { 1f, 2.76f, 5.40f, 8.93f };
            float[] queda = { 5f, 8f, 13f, 20f };
            float[] ganho = { 1f, 0.55f, 0.35f, 0.18f };
            const float f = 820f;
            for (int i = 0; i < b.Length; i++)
            {
                float t = i / (float)Taxa;
                float s = 0f;
                for (int p = 0; p < razao.Length; p++)
                    s += ganho[p] * (float)Math.Exp(-t * queda[p]) * (float)Math.Sin(2 * Math.PI * f * razao[p] * t);
                b[i] = s * Math.Min(1f, t / 0.001f);
            }
            var r = new Random(53);
            float lp = 0f;
            for (int i = 0; i < (int)(0.03f * Taxa); i++)   // o baque do martelo no ferro
            {
                lp += (Ruido(r) - lp) * 0.3f;
                b[i] += 1.2f * lp * (float)Math.Exp(-i / (float)Taxa * 120f);
            }
            return Normalizar(b, 0.6f);
        }

        /// <summary>Tres "co" curtos e um "coo" comprido: tom grave e aspero (seno com dobra) caindo.</summary>
        static float[] Galinha()
        {
            var b = new float[(int)(1.0f * Taxa)];
            var r = new Random(61);
            float[] inicio = { 0f, 0.16f, 0.30f, 0.50f };
            float[] dur = { 0.08f, 0.08f, 0.09f, 0.32f };
            for (int c = 0; c < inicio.Length; c++)
            {
                bool longo = c == inicio.Length - 1;
                int i0 = (int)(inicio[c] * Taxa), m = (int)(dur[c] * Taxa);
                double fase = 0;
                for (int k = 0; k < m && i0 + k < b.Length; k++)
                {
                    float u = k / (float)m;
                    float f = longo ? 560f + 160f * (float)Math.Sin(Math.PI * u) - 120f * u : 520f - 140f * u;
                    fase += 2 * Math.PI * f / Taxa;
                    float s = (float)Math.Sin(fase);
                    s = Math.Sign(s) * (float)Math.Pow(Math.Abs(s), 0.4);   // dobra: o aspero do cacarejo
                    float env = (float)Math.Sin(Math.PI * u);
                    b[i0 + k] += env * (0.8f * s + 0.25f * Ruido(r));
                }
            }
            return Normalizar(PassaBaixa(b, 2600f), 0.5f);
        }

        /// <summary>Dois latidos: harmonicos de um tom que cai de 420 para 250 Hz, ataque seco, sopro de ruido.</summary>
        static float[] Latido()
        {
            var b = new float[(int)(0.75f * Taxa)];
            var r = new Random(67);
            foreach (float inicio in new[] { 0f, 0.3f })
            {
                int i0 = (int)(inicio * Taxa), m = (int)(0.2f * Taxa);
                double fase = 0;
                for (int k = 0; k < m && i0 + k < b.Length; k++)
                {
                    float t = k / (float)Taxa, u = k / (float)m;
                    float f = 420f - 170f * u;
                    fase += 2 * Math.PI * f / Taxa;
                    float s = 0f;
                    for (int h = 1; h <= 6; h++) s += (float)Math.Sin(h * fase) / h;
                    float env = Math.Min(1f, t / 0.006f) * (float)Math.Exp(-t * 14f);
                    b[i0 + k] += env * (s + 0.4f * Ruido(r));
                }
            }
            return Normalizar(PassaBaixa(b, 1800f), 0.6f);
        }

        /// <summary>Ronco de porco: tres grunhidos curtos e graves (130 -> 95 Hz) em trem de pulsos asperos, nasal pela
        /// banda em 700 Hz.</summary>
        static float[] Porco()
        {
            var b = new float[(int)(0.9f * Taxa)];
            var r = new Random(73);
            var banda = new Banda(700f, 0.5f);
            foreach (float inicio in new[] { 0f, 0.26f, 0.5f })
            {
                int i0 = (int)(inicio * Taxa), m = (int)(0.18f * Taxa);
                double fase = 0;
                for (int k = 0; k < m && i0 + k < b.Length; k++)
                {
                    float u = k / (float)m;
                    float f = 130f - 35f * u;
                    fase += 2 * Math.PI * f / Taxa;
                    float pulso = (float)Math.Pow(0.5 + 0.5 * Math.Sin(fase), 6) - 0.23f;   // glote: pulso estreito (sem a media)
                    float env = (float)Math.Sin(Math.PI * u);
                    b[i0 + k] += env * (pulso + 0.15f * Ruido(r));
                }
            }
            for (int i = 0; i < b.Length; i++) b[i] = 0.4f * b[i] + banda.Passo(b[i]);
            return Normalizar(PassaBaixa(b, 2200f), 0.5f);
        }

        /// <summary>Miau: sobe e desce (500 -> 900 -> 600 Hz) com vibrato, harmonicos decrescentes.</summary>
        static float[] Miado()
        {
            int m = (int)(0.7f * Taxa);
            var b = new float[m];
            double fase = 0;
            for (int k = 0; k < m; k++)
            {
                float t = k / (float)Taxa, u = k / (float)m;
                float f = 500f + 400f * (float)Math.Sin(Math.PI * Math.Min(1f, u * 1.4f)) - 100f * u + 12f * (float)Math.Sin(2 * Math.PI * 6f * t);
                fase += 2 * Math.PI * f / Taxa;
                float s = (float)(Math.Sin(fase) + 0.5 * Math.Sin(2 * fase) + 0.25 * Math.Sin(3 * fase) + 0.12 * Math.Sin(4 * fase));
                b[k] = s * (float)Math.Pow(Math.Sin(Math.PI * u), 0.6);
            }
            return Normalizar(b, 0.45f);
        }

        /// <summary>Balde no poco: um estalo de agua (ruido em banda caindo) e bolhas subindo de tom.</summary>
        static float[] Agua()
        {
            var b = new float[(int)(0.8f * Taxa)];
            var r = new Random(71);
            var banda = new Banda(1400f, 0.6f);
            for (int i = 0; i < (int)(0.35f * Taxa); i++)
            {
                float t = i / (float)Taxa;
                b[i] += banda.Passo(Ruido(r)) * (float)Math.Exp(-t * 11f) * Math.Min(1f, t / 0.003f);
            }
            for (int k = 0; k < 6; k++)
            {
                float inicio = 0.05f + (float)r.NextDouble() * 0.5f;
                float f0 = 500f + (float)r.NextDouble() * 500f;
                Pio(b, inicio, f0, f0 * 2.2f, 0.035f, 0.35f);
            }
            return Normalizar(b, 0.5f);
        }

        /// <summary>Bosque dos Sussurros: sopro em banda de voz (~1,8 kHz), ondas lentas de intensidade. Laco de
        /// <see cref="SegundosDoSussurro"/>; toca em volta da entrada do bosque.</summary>
        static float[] Sussurro()
        {
            int n = (int)(SegundosDoSussurro * Taxa), fade = Taxa / 2;
            var longo = new float[n + fade];
            var r = new Random(73);
            var banda = new Banda(1800f, 0.9f);
            var grave = new Banda(700f, 1.2f);
            for (int i = 0; i < longo.Length; i++)
            {
                float t = i / (float)Taxa;
                // ondas com numero inteiro de ciclos na volta: o laco emenda tambem na intensidade
                double w = 2 * Math.PI * t / SegundosDoSussurro;
                float onda = 0.35f + 0.65f * (float)Math.Pow(0.5 + 0.5 * Math.Sin(3 * w) * Math.Sin(2 * w + 1.0), 2.0);
                float x = Ruido(r);
                longo[i] = onda * (banda.Passo(x) + 0.5f * grave.Passo(x));
            }
            return Normalizar(Laco(longo, n, fade), 0.5f);
        }

        // ---------------------------------------------------------------- efeitos de acao (Som)

        /// <summary>Esquiva: sopro que varre de 600 a 2400 Hz.</summary>
        static float[] Esquiva()
        {
            int m = (int)(0.3f * Taxa);
            var b = new float[m];
            var r = new Random(79);
            var banda = new Banda(600f, 0.7f);
            for (int k = 0; k < m; k++)
            {
                float u = k / (float)m;
                banda.Frequencia(600f + 1800f * u);
                b[k] = banda.Passo(Ruido(r)) * (float)Math.Sin(Math.PI * u);
            }
            return Normalizar(b, 0.5f);
        }

        /// <summary>Porta de madeira: duas batidas surdas (toc-toc) e o rangido curto da folha.</summary>
        static float[] Porta()
        {
            var b = new float[(int)(0.6f * Taxa)];
            var r = new Random(83);
            foreach (float inicio in new[] { 0f, 0.14f })
            {
                int i0 = (int)(inicio * Taxa);
                for (int k = 0; k < (int)(0.09f * Taxa); k++)
                {
                    float t = k / (float)Taxa;
                    b[i0 + k] += (float)Math.Exp(-t * 45f) * ((float)Math.Sin(2 * Math.PI * 150f * t) + 0.3f * Ruido(r) * (float)Math.Exp(-t * 200f));
                }
            }
            int c0 = (int)(0.3f * Taxa), cm = (int)(0.25f * Taxa);
            double fase = 0;
            for (int k = 0; k < cm; k++)
            {
                float u = k / (float)cm;
                fase += 2 * Math.PI * (110f + 60f * u + 25f * Ruido(r)) / Taxa;
                float dente = (float)(fase / (2 * Math.PI) % 1.0) * 2f - 1f;   // dente de serra: o atrito da dobradica
                b[c0 + k] += 0.25f * dente * (float)Math.Sin(Math.PI * u);
            }
            return Normalizar(PassaBaixa(b, 2200f), 0.55f);
        }

        // ---------------------------------------------------------------- blocos

        static float Ruido(Random r) { return (float)r.NextDouble() * 2f - 1f; }

        /// <summary>Vento: ruido passa-baixa duas vezes, com um sopro lento de 2 ciclos por volta (emenda no laco).</summary>
        static float[] Vento(int n, float corte, float pico, Random r)
        {
            int fade = Taxa / 2;
            var longo = new float[n + fade];
            float a = Alfa(corte), lp = 0f, lp2 = 0f;
            float volta = n / (float)Taxa;
            for (int i = 0; i < longo.Length; i++)
            {
                lp += (Ruido(r) - lp) * a;
                lp2 += (lp - lp2) * a;
                float t = i / (float)Taxa;
                float sopro = 0.55f + 0.45f * (float)Math.Sin(2 * Math.PI * 2f * t / volta);
                longo[i] = lp2 * sopro;
            }
            return Normalizar(Laco(longo, n, fade), pico);
        }

        /// <summary>Um pio: seno varrendo f0 -> f1 em 'dur', envelope seno ao quadrado, somado em volta (o que passa do fim
        /// cai no comeco, para o laco nao cortar um canto ao meio).</summary>
        static void Pio(float[] buf, float inicio, float f0, float f1, float dur, float ganho)
        {
            int i0 = (int)(inicio * Taxa), m = (int)(dur * Taxa);
            double fase = 0;
            for (int k = 0; k < m; k++)
            {
                float u = k / (float)m;
                fase += 2 * Math.PI * (f0 + (f1 - f0) * u) / Taxa;
                float env = (float)Math.Sin(Math.PI * u);
                int i = (i0 + k) % buf.Length;
                buf[i] += ganho * env * env * (float)Math.Sin(fase);
            }
        }

        /// <summary>Grilo: trens de 3 pulsos de 18 ms (12 ms de folga) a cada 'periodo' s, a partir de 'atraso'. Pulso que
        /// passaria do fim fica de fora: na emenda do laco so ha vento.</summary>
        static void Grilo(float[] buf, float freq, float periodo, float atraso, float ganho)
        {
            float volta = buf.Length / (float)Taxa;
            for (float t0 = atraso; t0 < volta; t0 += periodo)
                for (int p = 0; p < 3; p++)
                {
                    int i0 = (int)((t0 + p * 0.03f) * Taxa), m = (int)(0.018f * Taxa);
                    if (i0 + m >= buf.Length) continue;
                    for (int k = 0; k < m; k++)
                    {
                        float u = k / (float)m;
                        buf[i0 + k] += ganho * (float)Math.Sin(Math.PI * u) * (float)Math.Sin(2 * Math.PI * freq * (i0 + k) / Taxa);
                    }
                }
        }

        /// <summary>Fecha o laco: as 'fade' amostras depois do fim sao misturadas no comeco, que passa a continuar o fim.</summary>
        static float[] Laco(float[] longo, int n, int fade)
        {
            var b = new float[n];
            Array.Copy(longo, b, n);
            for (int i = 0; i < fade && n + i < longo.Length; i++)
            {
                float k = i / (float)fade;
                b[i] = longo[i] * k + longo[n + i] * (1f - k);
            }
            return b;
        }

        static float Alfa(float corte) { return 1f - (float)Math.Exp(-2 * Math.PI * corte / Taxa); }

        static float[] PassaBaixa(float[] b, float corte)
        {
            float a = Alfa(corte), lp = 0f;
            for (int i = 0; i < b.Length; i++) { lp += (b[i] - lp) * a; b[i] = lp; }
            return b;
        }

        /// <summary>Passa-banda de variavel de estado (Chamberlin). Estavel abaixo de ~Taxa/6 (3,6 kHz), onde tudo aqui mora.</summary>
        sealed class Banda
        {
            float f, low, band;
            readonly float q;
            public Banda(float freq, float q) { this.q = q; Frequencia(freq); }
            public void Frequencia(float freq) { f = 2f * (float)Math.Sin(Math.PI * Math.Min(freq, Taxa / 6f) / Taxa); }
            public float Passo(float x)
            {
                low += f * band;
                float high = x - low - q * band;
                band += f * high;
                return band;
            }
        }
    }
}
