using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace COE.EditorTools
{
    /// <summary>Custo de desenho de Auren depois da vila viva (16 casas, 28 arvores, adornos em cada porta): o que nao se
    /// mexe vira lote estatico (o Unity junta as malhas do mesmo material no build: menos draw calls) e adorno pequeno nao
    /// projeta sombra (o mapa de sombra desenhava cada arbusto, barril e borboleta de novo). Chamado no fim do
    /// AurenSceneBuilder.Populate, depois dos prototipos.
    /// ponytail: a regra de "se mexe" e a lista <see cref="Movem"/>; componente novo que move o proprio transform entra
    /// nela (o teste do VidaSceneTests pega os de hoje). Corte por distancia (LOD, layerCullDistances) entra se o
    /// aparelho ainda pedir depois disto.</summary>
    public static class LoteSetup
    {
        /// <summary>Quem move o proprio transform (ou o de um filho) em runtime: nada abaixo dele vira lote.</summary>
        public static readonly Type[] Movem =
        {
            typeof(Animator), typeof(CharacterController), typeof(Camera),
            typeof(Morador), typeof(Animal), typeof(Bando), typeof(NpcActor), typeof(BichoNoChapeu),
            typeof(TrainingDummy), typeof(CharacterMotor), typeof(Fumaca), typeof(Balanco),
        };

        /// <summary>Prefixos de nome de adorno pequeno (ele ou um pai): sem sombra projetada.</summary>
        public static readonly string[] SemSombra =
        {
            "arbusto", "lanterna", "barril", "caixote", "feno", "varal", "galinheiro", "chiqueiro", "roda", "membro_",
            "passaros_", "borboletas_", "carga_", "fumaca", "chamine", "janela", "fruta", "pano", "cesto", "poste",
        };

        public static void Marcar()
        {
            foreach (GameObject raiz in SceneManager.GetActiveScene().GetRootGameObjects())
                foreach (Renderer r in raiz.GetComponentsInChildren<Renderer>(true))
                {
                    if (Pequeno(r.transform)) r.shadowCastingMode = ShadowCastingMode.Off;
                    if (r is MeshRenderer && !SeMexe(r.transform))
                        GameObjectUtility.SetStaticEditorFlags(r.gameObject, StaticEditorFlags.BatchingStatic);
                }
        }

        public static bool SeMexe(Transform t)
        {
            for (; t != null; t = t.parent)
                foreach (Type tipo in Movem)
                    if (t.GetComponent(tipo) != null) return true;
            return false;
        }

        public static bool Pequeno(Transform t)
        {
            for (; t != null; t = t.parent)
                foreach (string p in SemSombra)
                    if (t.name.StartsWith(p, StringComparison.Ordinal)) return true;
            return false;
        }
    }
}
