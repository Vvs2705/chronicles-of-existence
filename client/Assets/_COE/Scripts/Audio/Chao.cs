using UnityEngine;

namespace COE
{
    /// <summary>Que passo toca em cada chao: pedra da praca, madeira do piso de casa, e terra/grama no resto. Le o material
    /// do colisor embaixo do pe (os greybox de Auren tem um material por superficie: COE_Auren_Pedra, COE_Auren_Madeira).
    /// ponytail: pelo nome do material, porque o chao e greybox; chao com malha propria pede uma marca de superficie no
    /// proprio colisor (componente ou PhysicsMaterial).</summary>
    public static class Chao
    {
        /// <summary>REGRA PURA: o passo do material de nome 'material' (nulo = terra).</summary>
        public static Som PassoEm(string material)
        {
            if (string.IsNullOrEmpty(material)) return Som.Passo;
            if (material.Contains("Pedra")) return Som.PassoPedra;
            if (material.Contains("Madeira")) return Som.PassoMadeira;
            return Som.Passo;
        }

        /// <summary>O passo do chao embaixo de 'pe' (raio curto para baixo, gatilho de fora).</summary>
        public static Som PassoSob(Vector3 pe)
        {
            RaycastHit hit;
            if (!Physics.Raycast(pe + Vector3.up * 0.3f, Vector3.down, out hit, 0.8f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                return Som.Passo;
            Renderer r = hit.collider.GetComponent<Renderer>();
            return PassoEm(r != null && r.sharedMaterial != null ? r.sharedMaterial.name : null);
        }
    }
}
