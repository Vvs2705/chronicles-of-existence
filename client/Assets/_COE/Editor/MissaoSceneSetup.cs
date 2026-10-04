using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace COE.EditorTools
{
    /// <summary>T012 — Gatilhos de objetivo de missao nas ancoras e o HUD da missao ativa. Chamado pelo
    /// AurenSceneBuilder depois das ancoras.
    /// Contrato do coordenador: assinatura fixa; o corpo e da raia dona deste arquivo.
    ///
    /// Monta a raiz "Missoes" (MissaoHud) com um filho por linha de MissaoMundo.Gatilhos, na posicao da ancora, cada um
    /// com QuestTrigger e um marcador visivel SEM collider. Os gatilhos nascem desligados: quem liga e o MissaoHud,
    /// so o objetivo pendente. Raiz propria, e nao filho da ancora: "Ancoras" continua so com ancoras (contrato T008).
    /// Sem Find em runtime: o HUD recebe a lista de gatilhos por campo, ligada aqui.</summary>
    public static class MissaoSceneSetup
    {
        /// <summary>Raiz dos gatilhos e do HUD. Gatilho: "Missoes/&lt;questId&gt;.&lt;objetivoId&gt;".</summary>
        public const string Raiz = "Missoes";

        static readonly Color CorMarcador = new Color(1f, 0.78f, 0.3f);   // ambar: destaca da grama e da terra

        public static void Montar(Transform ancoras, GameObject player)
        {
            if (ancoras == null) throw new ArgumentNullException("ancoras");
            var raiz = new GameObject(Raiz);
            MissaoHud hud = raiz.AddComponent<MissaoHud>();
            hud.Leitor = UnityEngine.Object.FindFirstObjectByType<PlayerInputReader>();   // edicao: o cartao evita o toque (HudLayout)
            Material mat = Marcador();

            string[][] tabela = MissaoMundo.Gatilhos;
            var gatilhos = new QuestTrigger[tabela.Length];
            for (int i = 0; i < tabela.Length; i++)
            {
                string questId = tabela[i][0], objetivoId = tabela[i][1], ancoraId = tabela[i][2];
                Transform ancora = ancoras.Find(ancoraId);
                if (ancora == null)
                    throw new Exception("MissaoSceneSetup: ancora '" + ancoraId + "' ausente (gatilho " + questId + "." + objetivoId + ").");

                var go = new GameObject(questId + "." + objetivoId);
                go.transform.SetParent(raiz.transform, false);
                go.transform.position = ancora.position;
                gatilhos[i] = go.AddComponent<QuestTrigger>();
                Texto(gatilhos[i], "questId", questId);
                Texto(gatilhos[i], "objetivoId", objetivoId);

                // Coluna fina de 2 m: da para ver de longe onde ir. So decora; o PlayerInteractor nao usa collider.
                GameObject coluna = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                coluna.name = "marcador";
                coluna.transform.SetParent(go.transform, false);
                coluna.transform.localPosition = new Vector3(0f, 1f, 0f);
                coluna.transform.localScale = new Vector3(0.3f, 1f, 0.3f);
                Object.DestroyImmediate(coluna.GetComponent<Collider>());
                if (mat != null) coluna.GetComponent<Renderer>().sharedMaterial = mat;

                go.SetActive(false);
            }

            var so = new SerializedObject(hud);
            GameObject som = AurenSceneBuilder.Achar(BootstrapSceneBuilder.NomeSom);
            so.FindProperty("som").objectReferenceValue = som != null ? som.GetComponent<SomDoJogo>() : null;
            SerializedProperty lista = so.FindProperty("gatilhos");
            lista.arraySize = gatilhos.Length;
            for (int i = 0; i < gatilhos.Length; i++) lista.GetArrayElementAtIndex(i).objectReferenceValue = gatilhos[i];
            so.ApplyModifiedPropertiesWithoutUndo();

            // [PROPOSTA] Para onde ir: seta/"▼" no alvo da historia principal (RumoDaMissao).
            var indicador = new SerializedObject(raiz.AddComponent<IndicadorDeObjetivo>());
            indicador.FindProperty("cam").objectReferenceValue = AurenSceneBuilder.Achar("Main Camera").GetComponent<Camera>();
            indicador.FindProperty("player").objectReferenceValue = player != null ? player.transform : null;
            indicador.ApplyModifiedPropertiesWithoutUndo();
        }

        static void Texto(Object alvo, string campo, string valor)
        {
            var so = new SerializedObject(alvo);
            SerializedProperty p = so.FindProperty(campo);
            if (p == null) throw new Exception(alvo.GetType().Name + " nao tem o campo serializado '" + campo + "'.");
            p.stringValue = valor;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        /// <summary>Material em memoria (vai dentro da cena ao salvar): nao escreve asset, entao o teste de Editor que
        /// chama Populate nao suja Assets/. null (sem URP) = material padrao da primitiva.
        /// ponytail: cor chapada; brilho/particula do "objetivo aqui" e da arte (T013).</summary>
        static Material Marcador()
        {
            Shader lit = Shader.Find("Universal Render Pipeline/Lit");
            if (lit == null) return null;
            var m = new Material(lit) { name = "COE_Missao_Marcador" };
            m.SetColor("_BaseColor", CorMarcador);
            return m;
        }
    }
}
