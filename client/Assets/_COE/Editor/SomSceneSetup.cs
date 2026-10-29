using UnityEditor;
using UnityEngine;

namespace COE.EditorTools
{
    /// <summary>Liga o SomDoJogo da cena em todo componente com o campo serializado `som` vazio, como o
    /// PartidaSetup faz com a Partida (e e chamado por ele: todo gerador ja termina nele). Tambem afina o que e de cena:
    /// o golpe levado pelo jogador soa como baque no corpo (HitFlash.somDoAcerto), tocar o simbolo da clareira (B11) soa o
    /// motivo do Limiar e, em Auren, o SomDoJogo sabe de quem e de onde medir o bosque. Busca so no Editor, na geracao; campo ja ligado fica como esta.</summary>
    public static class SomSceneSetup
    {
        public const string Campo = "som";
        const string TipoDoCampo = "PPtr<$SomDoJogo>";
        /// <summary>O bosque comeca a soar alguns metros antes da ancora entrada_bosque (o vao na linha das arvores).</summary>
        public const float AntesDoBosque = 4f;
        /// <summary>O objetivo da q08 que e tocar o simbolo da clareira (MissaoMundo.Gatilhos).</summary>
        public const string ObjetivoDoSimbolo = "tocar_o_simbolo";
        /// <summary>Onde o Enredado (q16) toca: perto desta ancora.</summary>
        public const string AncoraDoCoracao = "bosque_coracao";

        public static void Ligar()
        {
            SomDoJogo som = Object.FindFirstObjectByType<SomDoJogo>();   // um por cena (MissaoSceneTests confere)
            if (som == null) return;   // cena sem som (teste que monta so um pedaco): nada a ligar
            foreach (MonoBehaviour m in Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
            {
                if (m == null || m == som) continue;
                var so = new SerializedObject(m);
                SerializedProperty p = so.FindProperty(Campo);
                if (p == null || p.propertyType != SerializedPropertyType.ObjectReference || p.type != TipoDoCampo || p.objectReferenceValue != null) continue;
                p.objectReferenceValue = som;
                so.ApplyModifiedPropertiesWithoutUndo();
            }

            GameObject player = GameObject.Find("Player");
            var s = new SerializedObject(som);
            if (player != null)
            {
                s.FindProperty("jogador").objectReferenceValue = player.transform;
                HitFlash flash = player.GetComponent<HitFlash>();
                if (flash != null)
                {
                    var f = new SerializedObject(flash);
                    f.FindProperty("somDoAcerto").intValue = (int)Som.DanoRecebido;
                    f.ApplyModifiedPropertiesWithoutUndo();
                }
            }
            foreach (QuestTrigger q in Object.FindObjectsByType<QuestTrigger>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID))
            {
                if (q.ObjetivoId != ObjetivoDoSimbolo) continue;   // B11: o simbolo responde com o motivo
                var t = new SerializedObject(q);
                t.FindProperty("somAoCumprir").intValue = (int)Som.EcoDoLimiar;
                t.ApplyModifiedPropertiesWithoutUndo();
            }

            GameObject ancoras = GameObject.Find(AurenSceneBuilder.RaizAncoras);
            Transform entrada = ancoras != null ? ancoras.transform.Find("entrada_bosque") : null;
            if (entrada != null) s.FindProperty("zDoBosque").floatValue = entrada.position.z - AntesDoBosque;
            Transform coracao = ancoras != null ? ancoras.transform.Find(AncoraDoCoracao) : null;
            if (coracao != null) s.FindProperty("coracao").objectReferenceValue = coracao;
            s.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
