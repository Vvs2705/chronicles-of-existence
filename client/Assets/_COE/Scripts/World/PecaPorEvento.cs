using UnityEngine;

namespace COE
{
    /// <summary>Peca do mundo ligada por evento do historico de vida (docs/arte/fichas/ELENCO.md, "Pendencias de codigo",
    /// item 1): a vila reage ao que aconteceu nesta vida. Os FILHOS ficam ligados se e so se todos os eventos de 'exige'
    /// estao no historico e nenhum de 'some' esta (<see cref="Visivel"/>). Liga e desliga, nunca destroi; o proprio
    /// objeto fica ligado para continuar acompanhando o historico. So exibe estado: nao concede nem grava nada.
    ///
    /// CUSTO: o historico muda em transicao de missao (sem recarregar a cena) e no salto (que recarrega Auren). O Update
    /// so reavalia quando o historico trocou (save novo = sessao nova) ou cresceu (Total); fora isso e uma comparacao.
    /// ponytail: Total serve de versao porque o historico so cresce (Registrar nunca apaga id; Podar resume e mantem o id).
    /// Se um dia algo remover evento, a versao vira um contador [NonSerialized] no LifeHistoryData.
    ///
    /// ponytail: o Update le SaveState.Sessao (localizador estatico, docs/tech/DIVIDA_TECNICA.md); o resto recebe o
    /// historico por parametro. Quando a sessao chegar por injecao (T012), muda so a linha do Update.</summary>
    public class PecaPorEvento : MonoBehaviour
    {
        [Tooltip("Ids de evento que TEM de estar no historico (todos). Vazio = sem exigencia.")]
        [SerializeField] string[] exige = new string[0];
        [Tooltip("Ids de evento que NAO podem estar no historico (nenhum). Vazio = nunca some.")]
        [SerializeField] string[] some = new string[0];

        LifeEventHistory vista;
        int totalVisto = -1;   // -1: a primeira chamada sempre avalia

        /// <summary>Estado da ultima avaliacao (os filhos seguem isto).</summary>
        public bool Ligada { get; private set; }

        void Update() { Atualizar(SaveState.Sessao.Historia); }

        /// <summary>Reavalia se o historico trocou ou cresceu desde a ultima chamada e liga/desliga os filhos.
        /// Publico porque Update nao roda em teste de Editor (e o gerador poe a cena no estado de projeto).</summary>
        public void Atualizar(LifeEventHistory historia)
        {
            int total = historia == null ? 0 : historia.Total;
            if (historia == vista && total == totalVisto) return;
            vista = historia;
            totalVisto = total;
            Ligada = Visivel(exige, some, historia);
            foreach (Transform filho in transform) filho.gameObject.SetActive(Ligada);
        }

        /// <summary>A regra, pura: todos de 'exige' no historico e nenhum de 'some'. Historico nulo vale como vazio
        /// (nada aconteceu): visivel so se nada for exigido. Lista nula vale como vazia.</summary>
        public static bool Visivel(string[] exige, string[] some, LifeEventHistory historia)
        {
            if (exige != null)
                for (int i = 0; i < exige.Length; i++)
                    if (historia == null || !historia.Ja(exige[i])) return false;
            if (some != null && historia != null)
                for (int i = 0; i < some.Length; i++)
                    if (historia.Ja(some[i])) return false;
            return true;
        }
    }
}
