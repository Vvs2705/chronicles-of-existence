using System;
using UnityEngine;

namespace COE
{
    /// <summary>Vibracao curta nas acoes principais (juice): usar, acertar, levar golpe, concluir missao. Liga e desliga pelo
    /// menu (Configuracoes.Vibrar, aplicado pelo MenuDePausa como o AudioListener.volume do som).
    /// Android: Vibrator + VibrationEffect.createOneShot (API 26+, a minima do jogo), com intensidade. Fora do Android nao
    /// faz nada. A permissao VIBRATE entra no manifesto porque o fallback chama Handheld.Vibrate (o Unity a adiciona quando
    /// o codigo usa essa chamada).
    /// Regra pura (EditMode): pulsos colados (menos de IntervaloMinimo) viram um so, para o combo nao virar zumbido.
    /// ponytail: um pulso por vez, sem padrao (waveform). Padrao e intensidade por aparelho se o playtest pedir.</summary>
    public static class Tremor
    {
        /// <summary>Duracao (ms) e intensidade (1..255) de cada acao. HIPOTESE v0: calibrar no aparelho.</summary>
        public const int UsarMs = 12, UsarForca = 60;
        public const int AcertoMs = 22, AcertoForca = 140;
        public const int DanoMs = 45, DanoForca = 200;
        public const int MissaoMs = 70, MissaoForca = 120;

        /// <summary>Segundos entre dois pulsos; o de dentro da janela e descartado.</summary>
        public const float IntervaloMinimo = 0.06f;

        /// <summary>Configuracao do jogador (MenuDePausa.Aplicar). Ligado por padrao.</summary>
        public static bool Ligado = true;

        static float ultimo = float.NegativeInfinity;

        /// <summary>O pulso sai agora? Regra pura: desligado nao; colado no anterior nao.</summary>
        public static bool Pode(bool ligado, float agora, float ultimoPulso, int ms)
        {
            return ligado && ms > 0 && agora - ultimoPulso >= IntervaloMinimo;
        }

        public static void Usar() { Pulso(UsarMs, UsarForca); }
        public static void Acerto() { Pulso(AcertoMs, AcertoForca); }
        public static void Dano() { Pulso(DanoMs, DanoForca); }
        public static void Missao() { Pulso(MissaoMs, MissaoForca); }

        /// <summary>Um pulso de `ms` milissegundos com intensidade 1..255.</summary>
        public static void Pulso(int ms, int forca)
        {
            float agora = Time.unscaledTime;
            if (!Pode(Ligado, agora, ultimo, ms)) return;
            ultimo = agora;
            Vibrar(ms, Mathf.Clamp(forca, 1, 255));
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        static AndroidJavaObject vibrador;
        static AndroidJavaClass efeito;
        static bool semVibrador;

        static void Vibrar(int ms, int forca)
        {
            if (semVibrador) return;
            try
            {
                if (vibrador == null)
                {
                    using (var player = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    using (AndroidJavaObject atividade = player.GetStatic<AndroidJavaObject>("currentActivity"))
                        vibrador = atividade.Call<AndroidJavaObject>("getSystemService", "vibrator");
                    efeito = new AndroidJavaClass("android.os.VibrationEffect");
                    if (vibrador == null || !vibrador.Call<bool>("hasVibrator")) { semVibrador = true; return; }
                }
                using (AndroidJavaObject e = efeito.CallStatic<AndroidJavaObject>("createOneShot", (long)ms, forca))
                    vibrador.Call("vibrate", e);
            }
            catch (Exception)
            {
                semVibrador = true;   // aparelho sem a API: o pulso padrao do Unity, uma vez, e para de tentar
                Handheld.Vibrate();
            }
        }
#else
        static void Vibrar(int ms, int forca) { }   // editor e PC: sem vibracao
#endif
    }
}
