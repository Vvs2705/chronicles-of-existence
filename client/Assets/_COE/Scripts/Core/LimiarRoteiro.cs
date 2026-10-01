namespace COE
{
    /// <summary>B01 — o Limiar: falas de Aethron em ordem, cada uma com a resposta do jogador, que e o proprio botao de
    /// seguir. O jogador so escuta e responde; nao ha ramo. Textos em Resources/strings.pt-BR.json (limiar.*).
    /// Guarda-corpo (dossie §I, SLICE B01): Aethron nao e onisciente nem benigno por decreto e nunca diz que o jogador
    /// e o escolhido; o simbolo da Trama e plantado aqui sem explicacao.</summary>
    public static class LimiarRoteiro
    {
        public const string FalanteKey = "limiar.aethron";

        public static readonly (string FalaKey, string RespostaKey)[] Falas =
        {
            ("limiar.fala.1", "limiar.resposta.1"),
            ("limiar.fala.2", "limiar.resposta.2"),
            ("limiar.fala.3", "limiar.resposta.3"),
            ("limiar.fala.4", "limiar.resposta.4"),
            ("limiar.fala.5", "limiar.resposta.5"),
            ("limiar.fala.6", "limiar.resposta.6"),
        };

        /// <summary>Indice depois de responder; Falas.Length = acabou (segue para a escolha de destino, B02).</summary>
        public static int Seguir(int i) { return i < 0 ? 0 : i >= Falas.Length ? Falas.Length : i + 1; }

        /// <summary>Reler a fala anterior (aceite do B01: nada avanca sozinho e a leitura nao se perde). Para na primeira.</summary>
        public static int Voltar(int i) { return i <= 0 ? 0 : i - 1; }
    }
}
