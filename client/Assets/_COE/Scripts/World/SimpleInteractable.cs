using UnityEngine;

namespace COE
{
    /// <summary>Interagivel de cenario (portas, poco, mural de Auren) e de prova (T002). Com `chave`, o prompt e a linha que
    /// aparece ao usar vem do arquivo de textos: `chave` (prompt), `chave.fala` (aviso no alto da tela, DialogueHud) e
    /// `chave.depois` quando o `eventoDepois` esta no historico (o mural depois do sumico de Nilo, B09). Sem chave, o prompt
    /// e o texto cru de `prompt` (testes) ou o generico interacao.examinar. Nao e NPC, dialogo nem missao; nao muda nada.</summary>
    public class SimpleInteractable : Interactable
    {
        public string prompt = "";

        [Tooltip("Base das chaves de Strings: <chave> prompt, <chave>.fala ao usar, <chave>.depois com o eventoDepois. Vazio = prompt cru.")]
        public string chave = "";
        [Tooltip("Com este evento no historico da partida, a fala e <chave>.depois.")]
        public string eventoDepois = "";
        [Tooltip("Onde a fala aparece (faixa de aviso da conversa). Ligado pelo gerador; vazio = so conta.")]
        [SerializeField] DialogueHud aviso;
        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo gerador (PartidaSetup); vazio = a do SaveState.")]
        [SerializeField] Partida partida;

        /// <summary>Quantas vezes este objeto foi acionado nesta sessao.</summary>
        public int Contagem { get; private set; }

        public override string Prompt
        {
            get
            {
                if (!string.IsNullOrEmpty(chave)) return Strings.Get(chave);
                return string.IsNullOrEmpty(prompt) ? Strings.Get("interacao.examinar") : prompt;
            }
        }

        /// <summary>A chave da linha que aparece ao usar, com o historico desta partida. Puro: o teste confere sem tela.</summary>
        public string ChaveDaFala(GameSession sessao)
        {
            if (string.IsNullOrEmpty(chave)) return null;
            bool depois = !string.IsNullOrEmpty(eventoDepois) && sessao != null && sessao.Historia.Ja(eventoDepois);
            return chave + (depois ? ".depois" : ".fala");
        }

        protected override void OnInteract(GameObject quem)
        {
            Contagem++;
            string fala = ChaveDaFala(Partida.De(partida));
            if (aviso != null && fala != null) aviso.Avisar(Strings.Get(fala));
        }
    }
}
