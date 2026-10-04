using UnityEngine;

namespace COE
{
    /// <summary>"Chegar devagar" (ADR-0010 adendo 10; ficha lysa C5), a regra pura. C# PURO, testavel sem cena: recebe a
    /// distancia do avatar ao chapeu e a velocidade dele no plano, passo a passo.
    /// - Parado (ate <see cref="VelocidadeParado"/>) a ate <see cref="RaioDeEspera"/> conta; qualquer outra coisa (longe,
    ///   andando, saiu do raio) zera a contagem. <see cref="SegundosParado"/> seguidos = bicho calmo. Sem botao: ficar
    ///   parado e o gesto (ajuste 1).
    /// - Correr (acima de <see cref="VelocidadeDeSusto"/>) a menos de <see cref="RaioDeSusto"/> assusta: o chapeu treme.
    /// - Calmo fica calmo: o jogador ainda tem de achar Lysa ou Tovin (em outro periodo, longe dali). E estado de cena,
    ///   fora do save (ajuste 2): recarregar so pede repetir a espera.</summary>
    public sealed class ChegarDevagar
    {
        public const float RaioDeEspera = 1.5f;    // ADR-0010 adendo 10
        public const float SegundosParado = 3f;    // "tres respiracoes"
        public const float RaioDeSusto = 4f;       // ADR-0010 adendo 10
        // HIPOTESE (ELENCO.md, pendencias): entre andar 1,6 e correr 3,8 (MotionSolver). Calibrar no aparelho.
        public const float VelocidadeDeSusto = 2.5f;
        // HIPOTESE: abaixo disto e "parado" (o CharacterController assentando no chao, stick na zona morta).
        public const float VelocidadeParado = 0.2f;

        float espera;

        public bool Calmo { get; private set; }
        /// <summary>Neste passo o avatar correu perto do bicho ainda assustado (o chapeu treme).</summary>
        public bool Assustou { get; private set; }
        /// <summary>Segundos parados ate aqui (zera ao mexer ou sair do raio).</summary>
        public float Espera { get { return espera; } }

        public void Passo(float distancia, float velocidade, float dt)
        {
            Assustou = !Calmo && distancia < RaioDeSusto && velocidade > VelocidadeDeSusto;
            if (Calmo) return;
            espera = distancia <= RaioDeEspera && velocidade <= VelocidadeParado ? espera + dt : 0f;
            if (espera >= SegundosParado) Calmo = true;
        }
    }

    /// <summary>O chapeu da Lysa emborcado sobre o bicho (q05). So orquestra: mede o avatar a cada quadro e passa para
    /// <see cref="ChegarDevagar"/>. Conta so com o chapeu no chao (a <see cref="PecaPorEvento"/> irma ligada: q05 em
    /// andamento com buscar_ajuda cumprido). O DialogueHud le <see cref="Calmo"/> para oferecer tratar_o_animal.
    /// Dependencias por campo, ligadas pelo PecasDeEventoSetup: a peca e o Player.
    /// ponytail: tremer e girar a raiz da peca por meio segundo; o bicho encolhendo e arte (criatura sem G1).</summary>
    public class BichoNoChapeu : MonoBehaviour
    {
        [SerializeField] PecaPorEvento peca;
        [SerializeField] Transform player;

        const float SegundosTremendo = 0.6f;

        readonly ChegarDevagar regra = new ChegarDevagar();
        Vector3 ultima;
        bool temUltima;
        float tremerAte;

        /// <summary>Chapeu no chao e bicho calmo. Fora do save.</summary>
        public bool Calmo { get { return peca != null && peca.Ligada && regra.Calmo; } }

        void Update()
        {
            if (peca == null || player == null || !peca.Ligada || Time.deltaTime <= 0f) { temUltima = false; return; }

            Vector3 p = player.position;
            Vector3 d = p - ultima;
            d.y = 0f;   // so o plano: cair/assentar no chao nao e andar
            float velocidade = temUltima ? d.magnitude / Time.deltaTime : 0f;
            ultima = p;
            temUltima = true;

            Vector3 ate = transform.position - p;
            ate.y = 0f;
            regra.Passo(ate.magnitude, velocidade, Time.deltaTime);

            if (regra.Assustou) tremerAte = Time.time + SegundosTremendo;
            transform.localRotation = Time.time < tremerAte ? Quaternion.Euler(Mathf.Sin(Time.time * 45f) * 6f, 0f, 0f) : Quaternion.identity;
        }
    }
}
