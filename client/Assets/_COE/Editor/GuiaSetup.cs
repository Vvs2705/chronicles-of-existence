using UnityEditor;
using UnityEngine;

namespace COE.EditorTools
{
    /// <summary>Tutorial guiado do episodio 1 (GuiaHud). Montado pelo ConfiguracoesSceneSetup, logo depois do menu de pausa
    /// (as Configuracoes dele guardam os passos vistos), entao vale na Bootstrap e em Auren. O cartao de missao e a seta de
    /// objetivo so existem em Auren: o MissaoSceneSetup chama LigarMissao depois de criar os dois. Tudo por campo serializado.</summary>
    public static class GuiaSetup
    {
        public const string Nome = "Guia";

        public static GuiaHud Montar(PlayerInputReader input, MenuDePausa menu, GameObject player, Object[] controle)
        {
            var so = new SerializedObject(new GameObject(Nome).AddComponent<GuiaHud>());
            so.FindProperty("leitor").objectReferenceValue = input;
            so.FindProperty("menu").objectReferenceValue = menu;
            so.FindProperty("interacao").objectReferenceValue = player.GetComponent<PlayerInteractor>();
            so.FindProperty("combate").objectReferenceValue = player.GetComponent<PlayerCombat>();
            SerializedProperty p = so.FindProperty("controle");
            p.arraySize = controle.Length;
            for (int i = 0; i < controle.Length; i++) p.GetArrayElementAtIndex(i).objectReferenceValue = controle[i];
            so.ApplyModifiedPropertiesWithoutUndo();
            return (GuiaHud)so.targetObject;
        }

        /// <summary>Auren: o guia acende o cartao de missao e a seta. Sem guia na cena (gerador antigo) nao faz nada.</summary>
        public static void LigarMissao(MissaoHud missao, IndicadorDeObjetivo indicador)
        {
            GuiaHud guia = Object.FindFirstObjectByType<GuiaHud>();
            if (guia == null) return;
            var so = new SerializedObject(guia);
            so.FindProperty("missao").objectReferenceValue = missao;
            so.FindProperty("indicador").objectReferenceValue = indicador;
            so.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
