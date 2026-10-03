using UnityEditor;
using UnityEngine;

namespace COE.EditorTools
{
    /// <summary>O voltar do Android (e Esc no PC) na cena ATIVA: raiz "Voltar" com o VoltarHud ligado, por campo
    /// serializado, a conversa, menu de pausa, salto, gancho e entrada que EXISTIREM na cena (cada um e opcional:
    /// Bootstrap tem entrada e nao tem conversa; Auren tem conversa e gancho e nao tem entrada). Busca aqui no gerador,
    /// nunca em runtime. Chame por ULTIMO no gerador, depois de tudo que ele monta.
    /// O "travar" da confirmacao de saida e copiado do menu de pausa: a lista mora no ConfiguracoesSceneSetup.</summary>
    public static class VoltarSetup
    {
        public const string Nome = "Voltar";

        public static void Montar()
        {
            MenuDePausa menu = Object.FindFirstObjectByType<MenuDePausa>();
            var so = new SerializedObject(new GameObject(Nome).AddComponent<VoltarHud>());
            Prop(so, "conversa").objectReferenceValue = Object.FindFirstObjectByType<DialogueHud>();
            Prop(so, "menu").objectReferenceValue = menu;
            Prop(so, "salto").objectReferenceValue = Object.FindFirstObjectByType<SaltoHud>();
            Prop(so, "gancho").objectReferenceValue = Object.FindFirstObjectByType<GanchoHud>();
            Prop(so, "entrada").objectReferenceValue = Object.FindFirstObjectByType<EntryFlow>();

            SerializedProperty travar = Prop(so, "travar");
            SerializedProperty doMenu = menu != null ? Prop(new SerializedObject(menu), "travar") : null;
            travar.arraySize = doMenu != null ? doMenu.arraySize : 0;
            for (int i = 0; i < travar.arraySize; i++)
                travar.GetArrayElementAtIndex(i).objectReferenceValue = doMenu.GetArrayElementAtIndex(i).objectReferenceValue;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static SerializedProperty Prop(SerializedObject so, string campo)
        {
            SerializedProperty p = so.FindProperty(campo);
            if (p == null) throw new System.Exception(so.targetObject.GetType().Name + " nao tem o campo serializado '" + campo + "'.");
            return p;
        }
    }
}
