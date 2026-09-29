using UnityEditor;
using UnityEngine;

namespace COE.EditorTools
{
    /// <summary>T012 — Entrada da partida na Bootstrap (so nela; Auren nao chama): tela de nascimento quando
    /// SaveState.Current.birth.destinyId esta vazio, senao rota para a cena salva; aviso de save de versao mais nova.
    /// Contrato do coordenador: assinatura fixa; o corpo e da raia dona deste arquivo.</summary>
    public static class EntradaSceneSetup
    {
        /// <summary>Poe o EntryFlow na cena ATIVA e liga o PlayerInputReader que o Populate criou (a tela o desliga
        /// enquanto esta aberta). Busca aqui no gerador, nunca em runtime. Estoura sem o input: Montar antes do Populate
        /// e erro de gerador, nao caso a tolerar.</summary>
        public static void Montar()
        {
            PlayerInputReader input = Object.FindFirstObjectByType<PlayerInputReader>();
            if (input == null) throw new System.Exception("EntradaSceneSetup.Montar sem PlayerInputReader: chame depois do BootstrapSceneBuilder.Populate.");

            EntryFlow entrada = new GameObject("Entrada").AddComponent<EntryFlow>();
            var so = new SerializedObject(entrada);
            SerializedProperty p = so.FindProperty("input");
            if (p == null) throw new System.Exception("EntryFlow nao tem o campo serializado 'input'.");
            p.objectReferenceValue = input;
            so.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
