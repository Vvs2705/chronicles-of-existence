using UnityEngine;

namespace COE
{
    /// <summary>O que um bicho da vila esta fazendo (galinha ciscando, cachorro passeando, gato cochilando).</summary>
    public enum EstadoDoBicho { Parado = 0, Andando = 1, Bicando = 2, Fugindo = 3, Seguindo = 4 }

    /// <summary>A regra de um bicho solto em Auren. C# PURO e deterministico (semente fixa): passo a passo, dentro de um
    /// circulo (o terreiro, o galinheiro, a praca), alterna parar, andar ate um ponto sorteado e bicar o chao.
    /// - Correr perto (acima de <see cref="ChegarDevagar.VelocidadeDeSusto"/> a menos de <see cref="RaioDeSusto"/>), ou
    ///   encostar (menos de <see cref="RaioDeEncosto"/>), assusta: ele foge para longe do jogador, mais rapido, sem sair do
    ///   circulo. E a mesma leitura de "chegar devagar" do bicho da q05, em escala de bicho comum.
    /// - Quem segue (o cachorro) vem para perto do jogador que anda devagar a ate <see cref="RaioDeAmizade"/>, para a
    ///   <see cref="DistanciaDeCompanhia"/> dele e volta a passear quando o jogador vai embora. Segue dentro de
    ///   <see cref="raioDeSeguir"/> do centro: nao entra no bosque nem atravessa a vila atras de ninguem.
    /// Estado de cena, fora do save: recarregar so poe o bicho de novo no comeco.
    /// ponytail: sem obstaculo (o circulo e escolhido longe de parede na tabela do VidaSceneSetup) e sem bando; a galinha
    /// ignora a outra. Desvio entra se um circulo precisar encostar numa construcao.</summary>
    public sealed class Ciscar
    {
        public const float RaioDeSusto = 2.5f;
        public const float RaioDeEncosto = 0.7f;
        public const float RaioDeAmizade = 6f;
        public const float DistanciaDeCompanhia = 1.6f;
        const float FatorDeFuga = 2.6f;

        readonly Vector3 centro;
        readonly float raio, raioDeSeguir, velocidade;
        readonly bool segue;
        readonly System.Random rng;
        Vector3 alvo;
        float restante;

        public Vector3 Posicao { get; private set; }
        /// <summary>No plano, normalizada; nunca zero.</summary>
        public Vector3 Direcao { get; private set; }
        public EstadoDoBicho Estado { get; private set; }

        public Ciscar(Vector3 centro, float raio, float velocidade, int semente, bool segue = false, float raioDeSeguir = 0f)
        {
            this.centro = Plano(centro);
            this.raio = Mathf.Max(0.1f, raio);
            this.velocidade = Mathf.Max(0.05f, velocidade);
            this.segue = segue;
            this.raioDeSeguir = Mathf.Max(this.raio, raioDeSeguir);
            rng = new System.Random(semente);
            Posicao = this.centro;
            Direcao = Quaternion.Euler(0f, (float)(rng.NextDouble() * 360.0), 0f) * Vector3.forward;
            Estado = EstadoDoBicho.Parado;
            restante = (float)rng.NextDouble() * 2f;
        }

        /// <summary>Poe o bicho num ponto (preso ao circulo): cada galinha do galinheiro comeca num lugar proprio.</summary>
        public void Por(Vector3 p) { Posicao = Dentro(p, raio); }

        public void Passo(float dt, Vector3 jogador, float velocidadeDoJogador)
        {
            Vector3 longe = Posicao - Plano(jogador);
            float d = longe.magnitude;

            if (d < RaioDeEncosto || (d < RaioDeSusto && velocidadeDoJogador > ChegarDevagar.VelocidadeDeSusto))
            {
                Vector3 fuga = d < 1e-3f ? -Direcao : longe / d;
                alvo = Dentro(Posicao + fuga * 3f, raio);
                // Encurralado na borda do circulo: foge de lado, rente a ela, em vez de tremer parado.
                if ((alvo - Posicao).magnitude < 0.5f) alvo = Dentro(Posicao + Quaternion.Euler(0f, 90f, 0f) * fuga * 3f, raio);
                Estado = EstadoDoBicho.Fugindo;
                restante = 1.2f;
            }
            else if (segue && Estado != EstadoDoBicho.Fugindo && d < RaioDeAmizade
                     && velocidadeDoJogador <= ChegarDevagar.VelocidadeDeSusto
                     && (Plano(jogador) - centro).magnitude < raioDeSeguir)
            {
                Estado = EstadoDoBicho.Seguindo;
                alvo = d > DistanciaDeCompanhia ? Dentro(Plano(jogador) + longe / d * DistanciaDeCompanhia, raioDeSeguir) : Posicao;
                restante = 0.5f;
                if (d > 1e-3f) Direcao = -longe / d;   // olha para o jogador
            }
            else if (Estado == EstadoDoBicho.Seguindo)
            {
                Escolher();   // o jogador foi embora ou correu: volta a passear
            }

            restante -= dt;
            switch (Estado)
            {
                case EstadoDoBicho.Andando:
                case EstadoDoBicho.Fugindo:
                case EstadoDoBicho.Seguindo:
                    float v = velocidade * (Estado == EstadoDoBicho.Fugindo ? FatorDeFuga : Estado == EstadoDoBicho.Seguindo ? 1.5f : 1f);
                    Vector3 falta = alvo - Posicao;
                    float passo = v * dt;
                    if (falta.magnitude > 1e-3f) Direcao = falta.normalized;
                    if (falta.magnitude <= passo)
                    {
                        Posicao = alvo;
                        if (Estado != EstadoDoBicho.Seguindo) Escolher();
                    }
                    else Posicao += falta.normalized * passo;
                    if (Estado != EstadoDoBicho.Seguindo && restante <= 0f) Escolher();
                    break;
                default:
                    if (restante <= 0f) Escolher();
                    break;
            }
        }

        void Escolher()
        {
            double r = rng.NextDouble();
            if (r < 0.45)
            {
                float ang = (float)(rng.NextDouble() * 360.0);
                float dist = raio * Mathf.Sqrt((float)rng.NextDouble());
                alvo = centro + Quaternion.Euler(0f, ang, 0f) * Vector3.forward * dist;
                Estado = EstadoDoBicho.Andando;
                restante = (alvo - Posicao).magnitude / velocidade + 0.5f;
            }
            else if (r < 0.75)
            {
                Estado = EstadoDoBicho.Bicando;
                restante = 1f + (float)rng.NextDouble() * 1.5f;
            }
            else
            {
                Estado = EstadoDoBicho.Parado;
                restante = 1f + (float)rng.NextDouble() * 2f;
            }
        }

        Vector3 Dentro(Vector3 p, float limite)
        {
            Vector3 d = Plano(p) - centro;
            return d.magnitude <= limite ? centro + d : centro + d.normalized * limite;
        }

        static Vector3 Plano(Vector3 v) { v.y = 0f; return v; }
    }
}
