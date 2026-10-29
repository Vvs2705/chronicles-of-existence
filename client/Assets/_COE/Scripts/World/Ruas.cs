using System;
using System.Collections.Generic;
using UnityEngine;

namespace COE
{
    /// <summary>Uma rua de Auren: do spawn_player ate a ancora, pelos pontos de via (o percurso autorado do T008).</summary>
    [Serializable]
    public class Rua
    {
        public string ancora = "";
        public Vector3[] pontos = new Vector3[0];
    }

    /// <summary>As ruas de Auren em runtime: o NPC com nome caminha de uma ancora a outra por elas em vez de teleportar.
    /// Fica na raiz "Ancoras"; o gerador copia para ca os percursos do AurenSceneBuilder, que o teste de navegacao ja varre
    /// com a capsula do Player. Andar so por trechos desses percursos = andar so por chao livre.
    /// ponytail: a rede e uma arvore com raiz no spawn (todo percurso sai dele); o caminho entre duas ancoras volta ate o
    /// ultimo ponto comum dos dois percursos e sai pelo outro. Atalho entre ramos (praca -> horta sem passar pela rua das
    /// casas) pede grafo de verdade ou NavMesh.</summary>
    public class Ruas : MonoBehaviour
    {
        const float Mesmo = 0.05f;

        [SerializeField] Rua[] ruas = new Rua[0];

        public int Quantas { get { return ruas.Length; } }

        /// <summary>O percurso do spawn ate a ancora (inclusive os dois); null se a ancora nao tem rua.</summary>
        public Vector3[] Ida(string ancora)
        {
            for (int i = 0; i < ruas.Length; i++) if (ruas[i].ancora == ancora) return ruas[i].pontos;
            return null;
        }

        /// <summary>Caminho de uma ancora ate outra; null se alguma nao tem rua.</summary>
        public Vector3[] Caminho(string de, string para)
        {
            Vector3[] a = Ida(de), b = Ida(para);
            return a == null || b == null ? null : Entre(a, b);
        }

        /// <summary>REGRA PURA. 'de' e 'para' sao percursos que saem do mesmo ponto (o spawn): o caminho vai do fim de 'de'
        /// ate o fim de 'para' voltando por 'de' ate o ultimo ponto em comum e seguindo por 'para'. Sem prefixo comum
        /// (raizes diferentes) liga os dois inicios. Comeca no fim de 'de' e termina no fim de 'para'.</summary>
        public static Vector3[] Entre(Vector3[] de, Vector3[] para)
        {
            var r = new List<Vector3>();
            if (de == null || para == null || de.Length == 0 || para.Length == 0) return r.ToArray();
            int comum = 0;
            while (comum < de.Length && comum < para.Length && Igual(de[comum], para[comum])) comum++;

            if (comum == 0)
            {
                for (int i = de.Length - 1; i >= 0; i--) r.Add(de[i]);
                for (int i = 0; i < para.Length; i++) r.Add(para[i]);
            }
            else
            {
                // de[comum-1] e o ultimo ponto em comum: volta ate ele e sai por 'para'.
                for (int i = de.Length - 1; i >= comum - 1; i--) r.Add(de[i]);
                for (int i = comum; i < para.Length; i++) r.Add(para[i]);
            }

            // Sem pontos repetidos em sequencia (mesma ancora, ou um percurso prefixo do outro).
            for (int i = r.Count - 1; i > 0; i--) if (Igual(r[i], r[i - 1])) r.RemoveAt(i);
            return r.ToArray();
        }

        static bool Igual(Vector3 a, Vector3 b)
        {
            a.y = 0f; b.y = 0f;
            return (a - b).sqrMagnitude < Mesmo * Mesmo;
        }
    }
}
