using UnityEditor;
using UnityEngine;

namespace COE.EditorTools
{
    /// <summary>Menu de pausa com as configuracoes do jogador (mao destra/canhota, sensibilidade da camera, FPS alvo,
    /// HUD de desempenho), guardadas fora do save de progresso. Chamado no fim do BootstrapSceneBuilder.Populate,
    /// entao vale na Bootstrap e em Auren. Contrato do coordenador: assinatura fixa; o corpo e da raia dona.
    /// Tudo por campo serializado (nada de Find em runtime).</summary>
    public static class ConfiguracoesSceneSetup
    {
        public const string NomeMenu = "MenuDePausa";

        public static void Montar(PlayerInputReader input, ThirdPersonCamera camera, PerfHud desempenho)
        {
            // O Player e o alvo da camera (o Populate acabou de ligar): motor, combate e interacao dele travam com o menu
            // aberto, e a camera tambem (arrastar no menu nao pode girar a vista na volta).
            var alvo = new SerializedObject(camera).FindProperty("target").objectReferenceValue as Transform;
            if (alvo == null) throw new System.Exception("ConfiguracoesSceneSetup.Montar: camera sem alvo; chame no fim do Populate.");
            GameObject player = alvo.gameObject;

            var so = new SerializedObject(new GameObject(NomeMenu).AddComponent<MenuDePausa>());
            so.FindProperty("input").objectReferenceValue = input;
            so.FindProperty("desempenho").objectReferenceValue = desempenho;
            Object[] travar =
            {
                player.GetComponent<CharacterMotor>(), player.GetComponent<PlayerCombat>(), player.GetComponent<PlayerInteractor>(), camera,
            };
            SerializedProperty p = so.FindProperty("travar");
            p.arraySize = travar.Length;
            for (int i = 0; i < travar.Length; i++) p.GetArrayElementAtIndex(i).objectReferenceValue = travar[i];
            so.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
