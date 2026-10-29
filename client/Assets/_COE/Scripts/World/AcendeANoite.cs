using UnityEngine;

namespace COE
{
    /// <summary>Janela e lanterna que acendem a noite: troca o material de todo renderer filho entre o do dia (vidro escuro,
    /// lanterna apagada) e o aceso (sem luz, cor quente chapada) quando o periodo entra ou sai de <see cref="periodos"/>.
    /// So troca na virada; nada por quadro alem de comparar o periodo.
    /// ponytail: o COE/Toon nao tem emissao nem luz adicional, entao "acesa" e um material Unlit e nao ilumina o chao em
    /// volta. Luz de verdade (por vertice) entra com a cena da noite medida no aparelho.</summary>
    public class AcendeANoite : MonoBehaviour
    {
        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo gerador (PartidaSetup); vazio = a do SaveState.")]
        [SerializeField] Partida partida;
        [SerializeField] Material apagada;
        [SerializeField] Material acesa;
        [Tooltip("Periodos aceso: bit 0 manha, 1 tarde, 2 noite.")]
        [SerializeField] int periodos = 4;

        bool? ligada;

        public bool Acesa { get { return ligada == true; } }

        void Update()
        {
            SaveData save = Partida.De(partida).Save;
            Atualizar(TimeOfDayCycle.Atual(save == null ? null : save.life));
        }

        /// <summary>Publico porque Update nao roda em teste de Editor.</summary>
        public void Atualizar(TimeOfDay agora)
        {
            bool l = Morador.NoPeriodo(periodos, agora);
            if (ligada == l) return;
            ligada = l;
            Material m = l ? acesa : apagada;
            if (m == null) return;
            foreach (Renderer r in GetComponentsInChildren<Renderer>(true)) r.sharedMaterial = m;
        }
    }
}
