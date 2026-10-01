using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace COE.EditorTools
{
    /// <summary>T012 — NPCs de Auren em cena (rotina por periodo, interacao, dialogo). Chamado pelo AurenSceneBuilder
    /// depois das ancoras.
    /// Contrato do coordenador: assinatura fixa; o corpo e da raia dona deste arquivo.
    ///
    /// Monta "NPCs/&lt;npcId&gt;" (um por NpcCatalog.Npcs, capsula "Corpo" sem colisor) e "Dialogo" (DialogueHud), e liga
    /// tudo por campo serializado: ancoras e HUD em cada NpcActor; motor, combate, interacao e animador do Player no HUD.
    /// Posicao de projeto = manha, 5 anos, sem memoria; no jogo o NpcActor se repoe pelo save a cada quadro.</summary>
    public static class NpcSceneSetup
    {
        public const string RaizNpcs = "NPCs";
        public const string NomeHud = "Dialogo";

        /// <summary>Base corporal de crianca (docs/arte/PIPELINE.md §3.1: amigos de infancia). O resto e adulto.</summary>
        public static readonly string[] Criancas = { "nilo", "sera" };

        /// <summary>Raio local do colisor do NPC (a capsula primitiva tem 0,5).</summary>
        public const float RaioDoCorpo = 0.3f;

        public static void Montar(Transform ancoras, GameObject player)
        {
            DialogueHud hud = new GameObject(NomeHud).AddComponent<DialogueHud>();
            var so = new SerializedObject(hud);
            SerializedProperty travar = Prop(so, "travarNaConversa");
            Behaviour[] player3 = { player.GetComponent<CharacterMotor>(), player.GetComponent<PlayerCombat>(), player.GetComponent<PlayerInteractor>() };
            travar.arraySize = player3.Length;
            for (int i = 0; i < player3.Length; i++) travar.GetArrayElementAtIndex(i).objectReferenceValue = player3[i];
            Prop(so, "anim").objectReferenceValue = player.GetComponent<CharacterAnimator>();
            so.ApplyModifiedPropertiesWithoutUndo();

            Transform raiz = new GameObject(RaizNpcs).transform;
            var projeto = new SaveData();
            for (int i = 0; i < NpcCatalog.Npcs.Length; i++)
            {
                string id = NpcCatalog.Npcs[i].Id;
                var go = new GameObject(id);
                go.transform.SetParent(raiz, false);

                GameObject corpo = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                corpo.name = "Corpo";
                corpo.transform.SetParent(go.transform, false);
                // NPC e solido: o Player esbarra nele como em gente (pedido do idealizador, 2026-10-01). Raio local 0,3 na
                // capsula escalada por altura/2 = ~26 cm no adulto, ~17 cm na crianca (ombro, nao o cilindro do primitivo).
                // Cresce junto no salto (AjustarCorpo) e fica ativo quando o prototipo esconde o renderer da capsula.
                corpo.GetComponent<CapsuleCollider>().radius = RaioDoCorpo;

                NpcActor npc = go.AddComponent<NpcActor>();
                so = new SerializedObject(npc);
                Prop(so, "npcId").stringValue = id;
                Prop(so, "vaga").intValue = i;
                Prop(so, "crianca").boolValue = Array.IndexOf(Criancas, id) >= 0;
                Prop(so, "ancoras").objectReferenceValue = ancoras;
                Prop(so, "corpo").objectReferenceValue = corpo.transform;
                Prop(so, "dialogo").objectReferenceValue = hud;
                so.ApplyModifiedPropertiesWithoutUndo();

                npc.AjustarCorpo(projeto.ageYears);
                npc.Posicionar(projeto);
            }
        }

        static SerializedProperty Prop(SerializedObject so, string campo)
        {
            SerializedProperty p = so.FindProperty(campo);
            if (p == null) throw new Exception(so.targetObject.GetType().Name + " nao tem o campo serializado '" + campo + "'.");
            return p;
        }
    }
}
