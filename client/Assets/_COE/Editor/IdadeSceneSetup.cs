using UnityEditor;
using UnityEngine;

namespace COE.EditorTools
{
    /// <summary>T012 — Corpo por idade (5 ou 8 anos: capsula, camera, altura do golpe) e a tela do salto (B12/B13)
    /// no Player. Chamado no fim do BootstrapSceneBuilder.Populate, entao vale na Bootstrap e em Auren.
    /// Contrato do coordenador: assinatura fixa; o corpo e da raia dona deste arquivo.
    /// Tudo por campo serializado (nada de Find em runtime).</summary>
    public static class IdadeSceneSetup
    {
        public const string NomeHud = "SaltoHud";

        public static void Montar(GameObject player, ThirdPersonCamera camera)
        {
            // Visuais = filhos diretos do Player neste ponto do Populate: a capsula "Body" e, se ja existir, o humanoide
            // (HumanoidSetup.AttachTo roda antes). Todos montados na altura de 5 anos; o BodyByAge escala aos 8.
            var visuais = new Transform[player.transform.childCount];
            for (int i = 0; i < visuais.Length; i++) visuais[i] = player.transform.GetChild(i);

            var so = new SerializedObject(player.AddComponent<BodyByAge>());
            so.FindProperty("cam").objectReferenceValue = camera;
            Lista(so.FindProperty("visuais"), visuais);
            so.ApplyModifiedPropertiesWithoutUndo();

            // Tela do salto: raiz propria, como Perf e DamagePopup. Trava motor, combate e interacao enquanto aberta.
            so = new SerializedObject(new GameObject(NomeHud).AddComponent<SaltoHud>());
            Lista(so.FindProperty("travar"), new Object[]
            {
                player.GetComponent<CharacterMotor>(), player.GetComponent<PlayerCombat>(), player.GetComponent<PlayerInteractor>(),
            });
            so.ApplyModifiedPropertiesWithoutUndo();

            so = new SerializedObject(new GameObject(NomeTreino).AddComponent<TreinoHud>());   // B15/R7: o progresso do treino
            so.FindProperty("combate").objectReferenceValue = player.GetComponent<PlayerCombat>();
            so.ApplyModifiedPropertiesWithoutUndo();

            so = new SerializedObject(new GameObject(NomeBarras).AddComponent<BarrasHud>());   // GDD cap. 05: Vida, Vigor e Mana
            so.FindProperty("combate").objectReferenceValue = player.GetComponent<PlayerCombat>();
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        public const string NomeTreino = "TreinoHud";
        public const string NomeBarras = "BarrasHud";
        public const string NomeGancho = "GanchoHud";

        /// <summary>B16: a tela do gancho (fim do slice), so em Auren. Trava o mesmo que o salto trava.</summary>
        public static void MontarGancho(GameObject player)
        {
            var so = new SerializedObject(new GameObject(NomeGancho).AddComponent<GanchoHud>());
            Lista(so.FindProperty("travar"), new Object[]
            {
                player.GetComponent<CharacterMotor>(), player.GetComponent<PlayerCombat>(), player.GetComponent<PlayerInteractor>(),
            });
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static void Lista(SerializedProperty p, Object[] itens)
        {
            p.arraySize = itens.Length;
            for (int i = 0; i < itens.Length; i++) p.GetArrayElementAtIndex(i).objectReferenceValue = itens[i];
        }
    }
}
