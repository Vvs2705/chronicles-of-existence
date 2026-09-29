using System.Collections.Generic;
using UnityEngine;

namespace COE
{
    /// <summary>Poe o Player na ancora gravada no save (SaveData.anchorId) quando a cena carrega. A T004 grava o id,
    /// a T009 zera no salto (B14) e este componente e o unico que o le.
    ///
    /// DEPENDENCIA EXPLICITA: a raiz "Ancoras" chega por campo serializado, ligado pelo gerador de cena
    /// (BootstrapSceneBuilder.LigarAncoras). Sem ela (cena Bootstrap, que nao tem ancoras) o componente nao faz nada
    /// e o Player fica onde o gerador o pos. Nada de GameObject.Find.
    /// ponytail: so ancora, sceneId e ignorado. Ainda nao existe loader de cena; quando existir, ele abre save.sceneId
    /// e este componente continua igual (ancora de outra cena cai em spawn_player pela regra abaixo).</summary>
    public class AnchorSpawn : MonoBehaviour
    {
        /// <summary>Ancora de entrada padrao (slice secao 1: B06 e B14). Mesmo id da tabela do AurenSceneBuilder.</summary>
        public const string Padrao = "spawn_player";

        [SerializeField] Transform ancoras;   // raiz "Ancoras" da cena; null = cena sem ancoras

        // Awake e nao Start: SaveBootstrap (-200) ja carregou o save, e a camera le o yaw do alvo no Start dela.
        void Awake() { Aplicar(SaveState.Current == null ? null : SaveState.Current.anchorId); }

        /// <summary>Move o Player para a ancora efetiva. Publico porque Awake nao roda em teste de Editor.</summary>
        public void Aplicar(string anchorIdSalvo)
        {
            if (ancoras == null) return;

            var ids = new List<string>(ancoras.childCount);
            for (int i = 0; i < ancoras.childCount; i++) ids.Add(ancoras.GetChild(i).name);
            Transform alvo = ancoras.Find(AncoraEfetiva(anchorIdSalvo, ids));
            if (alvo == null) return;   // cena sem spawn_player: fica onde o gerador pos

            // CharacterController guarda a posicao dele: sem desligar, o proximo Move desfaz o teleporte.
            var cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            transform.SetPositionAndRotation(alvo.position, alvo.rotation);
            if (cc != null) cc.enabled = true;
        }

        /// <summary>Id da ancora onde o Player entra. C# PURO, nunca lanca. Ancora gravada que a cena tem -> ela;
        /// vazia, nula ou que a cena nao tem (id antigo, ancora de outra cena, save editado) -> spawn_player.</summary>
        public static string AncoraEfetiva(string salva, IList<string> existentes)
        {
            if (!string.IsNullOrEmpty(salva) && existentes != null && existentes.Contains(salva)) return salva;
            return Padrao;
        }
    }
}
