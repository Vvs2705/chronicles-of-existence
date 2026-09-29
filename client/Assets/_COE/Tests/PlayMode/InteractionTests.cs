using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace COE.PlayModeTests
{
    /// <summary>Selecao de alvo e disparo da interacao (T002), em mini-cena montada em codigo.
    /// PlayMode porque o PlayerInteractor decide no Update: precisa de pelo menos um frame.</summary>
    public class InteractionTests
    {
        readonly List<GameObject> spawned = new List<GameObject>();

        GameObject Spawn(string name, Vector3 pos)
        {
            var go = new GameObject(name);
            go.transform.position = pos;
            spawned.Add(go);
            return go;
        }

        [TearDown]
        public void TearDown()
        {
            // DestroyImmediate: o OnDisable tem de sair do registro estatico de Interactable AGORA, senao o
            // sobrevivente de um teste vira alvo do proximo.
            foreach (GameObject go in spawned) if (go != null) Object.DestroyImmediate(go);
            spawned.Clear();
        }

        /// <summary>Jogador na origem, olhando para +Z (rotacao identidade).</summary>
        PlayerInteractor Jogador()
        {
            return Spawn("Player", Vector3.zero).AddComponent<PlayerInteractor>();
        }

        SimpleInteractable Alvo(string nome, Vector3 pos, string prompt)
        {
            var it = Spawn(nome, pos).AddComponent<SimpleInteractable>();
            it.prompt = prompt;
            return it;
        }

        [UnityTest]
        public IEnumerator EscolheOMaisProximoDentroDoRaioEAFrente()
        {
            PlayerInteractor p = Jogador();
            Alvo("Poste", new Vector3(0f, 0f, 2f), "Examinar o poste");
            SimpleInteractable caixa = Alvo("Caixa", new Vector3(0.5f, 0f, 1f), "Abrir a caixa");

            yield return null;

            Assert.AreSame(caixa, p.Alvo, "entre dois validos vence o mais proximo");
            Assert.AreEqual("Abrir a caixa", p.Prompt);
        }

        [UnityTest]
        public IEnumerator DeCostasOuForaDoRaio_NaoViraAlvo()
        {
            PlayerInteractor p = Jogador();
            Alvo("Atras", new Vector3(0f, 0f, -1.5f), "nao devia aparecer");   // dentro do raio, fora da visao
            Alvo("Longe", new Vector3(0f, 0f, 20f), "nao devia aparecer");     // na visao, fora do raio

            yield return null;

            Assert.IsNull(p.Alvo);
            Assert.AreEqual(string.Empty, p.Prompt);
        }

        [UnityTest]
        public IEnumerator AlturaNaoContaParaOAlcance()
        {
            PlayerInteractor p = Jogador();
            SimpleInteractable alto = Alvo("Poste", new Vector3(0f, 1.5f, 1f), "Examinar o poste");

            yield return null;

            Assert.AreSame(alto, p.Alvo, "distancia e angulo sao planares: o topo do poste nao pode sair do alcance");
        }

        [UnityTest]
        public IEnumerator Interagir_DisparaOEventoUmaVez()
        {
            PlayerInteractor p = Jogador();
            SimpleInteractable caixa = Alvo("Caixa", new Vector3(0f, 0f, 1f), "Abrir a caixa");
            int eventos = 0;
            GameObject quem = null;
            caixa.Interacted += delegate (GameObject g) { eventos++; quem = g; };

            yield return null;
            p.Interagir();

            Assert.AreEqual(1, eventos, "uma chamada = um evento");
            Assert.AreEqual(1, caixa.Contagem);
            Assert.AreSame(p.gameObject, quem, "o evento diz quem interagiu");
        }

        [UnityTest]
        public IEnumerator SemAlvo_InteragirNaoFazNada()
        {
            PlayerInteractor p = Jogador();
            SimpleInteractable longe = Alvo("Caixa", new Vector3(0f, 0f, 20f), "Abrir a caixa");
            int eventos = 0;
            longe.Interacted += delegate { eventos++; };

            yield return null;
            p.Interagir();

            Assert.AreEqual(0, eventos);
            Assert.AreEqual(0, longe.Contagem);
        }
    }
}
