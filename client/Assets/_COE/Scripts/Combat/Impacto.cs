using UnityEngine;

namespace COE
{
    /// <summary>Peso do golpe (juice): uma parada curta do tempo no acerto (hit-stop) e um tranco da camera. So o que o
    /// jogador sente; nenhuma regra de dano passa por aqui.
    ///
    /// PARADA: no acerto o tempo cai para Escala por Duracao (tempo real). Quem desce e quem sobe e so esta parada: se
    /// outro dono (menu, guia, salto) mexeu no timeScale no meio, ela NAO devolve por cima dele; o menu que pausou devolve a
    /// Escala ao fechar e o proximo Tick solta. Escala acima de zero de
    /// proposito: o Update do combate continua vendo o quadro e solta a parada sozinho.
    /// TRANCO: amplitude que cai ao quadrado ate zero em DuracaoTranco; a camera soma o deslocamento depois de se posicionar.
    /// ponytail: valores fixos (HIPOTESE v0) iguais para leve, forte e magia; por golpe se o playtest pedir.</summary>
    public sealed class ParadaDeImpacto
    {
        public const float Escala = 0.08f, Duracao = 0.06f;

        float ate = -1f;

        public bool Parado { get { return ate >= 0f; } }

        /// <summary>Comeca uma parada se o tempo esta normal (1). Devolve se parou.</summary>
        public bool Parar(float agoraReal)
        {
            if (!Application.isPlaying || Parado || !Mathf.Approximately(Time.timeScale, 1f)) return false;   // EditMode: o tempo do editor nao muda
            Time.timeScale = Escala;
            ate = agoraReal + Duracao;
            return true;
        }

        /// <summary>Chamado por quadro (tempo real). Solta a parada quando passa a duracao.</summary>
        public void Tick(float agoraReal)
        {
            if (Parado && agoraReal >= ate) Soltar();
        }

        /// <summary>Devolve o tempo so se ainda e o desta parada. Idempotente.</summary>
        public void Soltar()
        {
            if (!Parado) return;
            ate = -1f;
            if (Mathf.Approximately(Time.timeScale, Escala)) Time.timeScale = 1f;
        }
    }

    public static class Tranco
    {
        /// <summary>Metros de deslocamento maximo da camera e duracao (tempo real) do tranco.</summary>
        public const float Acerto = 0.04f, Dano = 0.09f, DuracaoTranco = 0.18f;

        /// <summary>Amplitude depois de `decorrido` segundos: forca no inicio, cai ao quadrado, zero no fim (e fora dele).</summary>
        public static float Amplitude(float forca, float decorrido, float duracao)
        {
            if (duracao <= 0f || decorrido < 0f || decorrido >= duracao) return 0f;
            float t = 1f - decorrido / duracao;
            return forca * t * t;
        }
    }
}
