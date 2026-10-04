using UnityEditor;
using UnityEngine;

namespace COE.EditorTools
{
    /// <summary>Liga a Partida da cena (objeto "Save") em todo componente que tem o campo serializado `partida`. Os geradores
    /// chamam no fim do Populate: quem ganhar o campo depois ja sai ligado, sem um Set por raia. Busca so no Editor, na geracao
    /// da cena (nada de Find em runtime). Campo ja ligado fica como esta.</summary>
    public static class PartidaSetup
    {
        public const string Campo = "partida";

        public static void Ligar()
        {
            Partida partida = Object.FindFirstObjectByType<Partida>();
            if (partida == null) throw new System.Exception("PartidaSetup: cena sem Partida (o Populate do Bootstrap poe no objeto Save).");
            foreach (MonoBehaviour m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
            {
                if (m == null || m is Partida) continue;
                var so = new SerializedObject(m);
                SerializedProperty p = so.FindProperty(Campo);
                if (p == null || p.propertyType != SerializedPropertyType.ObjectReference || p.objectReferenceValue != null) continue;
                p.objectReferenceValue = partida;
                so.ApplyModifiedPropertiesWithoutUndo();
            }
        }
    }
}
