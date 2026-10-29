using System;

namespace COE
{
    /// <summary>Os efeitos de interface, combate e eventos (Som 9 em diante). Mesmas regras da Sintese: C# puro,
    /// deterministico, mono a <see cref="Taxa"/>. Os de interface e de evento sao da familia da caixinha e da harpa (soam como
    /// a trilha); os de combate sao madeira e corpo, nada de metal (a espada e de treino).</summary>
    public static partial class Sintese
    {
        // ---------------------------------------------------------------- interface

        /// <summary>Abrir painel: duas notas de caixinha subindo (sol-do).</summary>
        static float[] Abrir()
        {
            var b = new float[(int)(0.35f * Taxa)];
            Nota(b, 0f, Frequencia(79), 0.3f, 14f, 0.6f, false);
            Nota(b, 0.06f, Frequencia(84), 0.29f, 12f, 0.7f, false);
            return Normalizar(b, 0.45f);
        }

        /// <summary>Fechar painel: as mesmas duas notas descendo.</summary>
        static float[] Fechar()
        {
            var b = new float[(int)(0.35f * Taxa)];
            Nota(b, 0f, Frequencia(84), 0.3f, 14f, 0.6f, false);
            Nota(b, 0.06f, Frequencia(79), 0.29f, 12f, 0.7f, false);
            return Normalizar(b, 0.4f);
        }

        /// <summary>Confirmar uma escolha que pesa (nascer): acorde de harpa arpejado, do-mi-sol-do.</summary>
        static float[] Confirmar()
        {
            var b = new float[(int)(1.2f * Taxa)];
            int[] notas = { 72, 76, 79, 84 };
            for (int i = 0; i < notas.Length; i++) Tom(b, i * 0.05f, Frequencia(notas[i]), 1f, 0.003f, 3.5f, 0.6f, Corda, 0f);
            return Normalizar(b, 0.6f);
        }

        // ---------------------------------------------------------------- combate

        /// <summary>Espada de madeira cortando o ar: sopro curto em banda que desce de 1800 para 500 Hz.</summary>
        static float[] GolpeNoAr()
        {
            int m = (int)(0.16f * Taxa);
            var b = new float[m];
            var r = new Random(101);
            var banda = new Banda(1800f, 0.6f);
            for (int k = 0; k < m; k++)
            {
                float u = k / (float)m;
                banda.Frequencia(1800f - 1300f * u);
                b[k] = banda.Passo(Ruido(r)) * (float)Math.Sin(Math.PI * Math.Pow(u, 0.6));
            }
            return Normalizar(b, 0.4f);
        }

        /// <summary>Levar um golpe: baque grave no corpo (seno de 95 caindo para 50 Hz) com o abafado do ruido.</summary>
        static float[] DanoRecebido()
        {
            var b = new float[(int)(0.28f * Taxa)];
            var r = new Random(103);
            double fase = 0;
            float lp = 0f;
            for (int i = 0; i < b.Length; i++)
            {
                float t = i / (float)Taxa;
                fase += 2 * Math.PI * (50f + 45f * (float)Math.Exp(-t * 18f)) / Taxa;
                lp += (Ruido(r) - lp) * 0.12f;
                b[i] = Math.Min(1f, t / 0.002f) * ((float)Math.Sin(fase) * (float)Math.Exp(-t * 14f) + 1.5f * lp * (float)Math.Exp(-t * 35f));
            }
            return Normalizar(b, 0.75f);
        }

        /// <summary>Bloqueio: madeira contra madeira, duas parciais secas (620 e 1130 Hz) e o estalo.</summary>
        static float[] Bloqueio()
        {
            var b = new float[(int)(0.15f * Taxa)];
            var r = new Random(107);
            for (int i = 0; i < b.Length; i++)
            {
                float t = i / (float)Taxa;
                float corpo = (float)(Math.Sin(2 * Math.PI * 620.0 * t) * Math.Exp(-t * 40.0) + 0.6 * Math.Sin(2 * Math.PI * 1130.0 * t) * Math.Exp(-t * 60.0));
                b[i] = Math.Min(1f, t / 0.0005f) * (corpo + 0.5f * Ruido(r) * (float)Math.Exp(-t * 150f));
            }
            return Normalizar(b, 0.6f);
        }

        /// <summary>O parceiro de treino cede: duas notas de harpa que descem (sol-do), um "chega por hoje".</summary>
        static float[] Ceder()
        {
            var b = new float[(int)(1f * Taxa)];
            Tom(b, 0f, Frequencia(67), 0.8f, 0.004f, 3f, 0.6f, Corda, 0f);
            Tom(b, 0.18f, Frequencia(60), 0.8f, 0.004f, 3f, 0.6f, Corda, 0f);
            return Normalizar(b, 0.5f);
        }

        // ---------------------------------------------------------------- eventos

        /// <summary>Marco de dominio no treino: arpejo de harpa subindo duas oitavas (do-mi-sol-do-mi).</summary>
        static float[] Maestria()
        {
            var b = new float[(int)(1.3f * Taxa)];
            int[] notas = { 72, 76, 79, 84, 88 };
            for (int i = 0; i < notas.Length; i++) Tom(b, i * 0.07f, Frequencia(notas[i]), 1f, 0.003f, 3f, 0.5f, Corda, 0f);
            return Normalizar(b, 0.6f);
        }

        /// <summary>O dia anda (descansar): dois sinos longos e mansos, la e mi, um depois do outro.</summary>
        static float[] DiaPassa()
        {
            var b = new float[(int)(2f * Taxa)];
            Nota(b, 0f, Frequencia(69), 1.8f, 1.8f, 0.6f, false);
            Nota(b, 0.25f, Frequencia(76), 1.7f, 1.8f, 0.5f, false);
            return Normalizar(b, 0.5f);
        }

        /// <summary>O simbolo do Limiar responde: o motivo em sinos, com eco.</summary>
        static float[] EcoDoLimiar()
        {
            var b = new float[(int)(2.4f * Taxa)];
            for (int i = 0; i < MotivoDoLimiar.Length; i++) Nota(b, i * 0.18f, Frequencia(MotivoDoLimiar[i] + 12), 1.2f, 2.5f, 0.5f, false);
            var seco = (float[])b.Clone();
            int d = (int)(0.27f * Taxa);
            for (int i = d; i < b.Length; i++) b[i] = seco[i] + 0.4f * b[i - d];   // eco que nao volta no comeco: e efeito, nao laco
            return Normalizar(b, 0.55f);
        }

        /// <summary>O salto de idade: pad de mi maior que cresce e o motivo do Limiar por cima, inteiro.</summary>
        static float[] Salto()
        {
            var b = new float[(int)(3.2f * Taxa)];
            foreach (int m in new[] { 52, 59, 64, 68, 71 })
            {
                Tom(b, 0f, Frequencia(m) * 0.9985f, 3f, 1.2f, 0.3f, 0.12f, Macio, 0f);
                Tom(b, 0f, Frequencia(m) * 1.0015f, 3f, 1.2f, 0.3f, 0.12f, Macio, 0f);
            }
            for (int i = 0; i < MotivoDoLimiar.Length; i++) Nota(b, 0.6f + i * 0.35f, Frequencia(MotivoDoLimiar[i]), 1.4f, 2f, 0.4f, false);
            return Normalizar(b, 0.65f);
        }
    }
}
