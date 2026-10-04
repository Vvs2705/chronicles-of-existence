using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace COE.PlayModeTests
{
    /// <summary>Bloco D: a entrada (titulo e nascimento B02-B05) em uGUI, pelo toque nos botoes de verdade. Cada tela: alvos
    /// >= 48 dp na area segura e sem sobreposicao; quatro destinos, tres origens; nome invalido segura na tela com o erro;
    /// "Cancelar" no "tem certeza" volta ao destino SEM gravar (B05). Nao confirma o nascimento: Commit no Play gravaria
    /// no save.json do PC.</summary>
    public class EntradaTelaTests
    {
        SaveData antes;
        GameObject go;

        [SetUp]
        public void SetUp()
        {
            antes = SaveState.Current;
            SaveState.Current = new SaveData();   // sem destino: a entrada vai para o nascimento
        }

        [TearDown]
        public void TearDown()
        {
            if (go != null) Object.Destroy(go);
            SaveState.Current = antes;
        }

        [UnityTest]
        public IEnumerator Nascimento_PeloToque_TelasUsaveis_ECancelarNaoGrava()
        {
            go = new GameObject("Entrada");
            EntryFlow e = go.AddComponent<EntryFlow>();
            yield return null;
            yield return null;
            Assert.IsTrue(e.Aberta, "sem destino a entrada abre");
            Assert.IsNotNull(e.Vista);
            Assert.IsTrue(e.Vista.enabled);
            UiChecagem.BotoesUsaveis(e.Vista, "titulo");

            yield return Tocar(e, "BotaoPrincipal");   // Comecar (sem palco do Limiar no teste: vai direto ao destino)
            Assert.AreEqual(DestinyCatalog.Destinos.Length, UiChecagem.BotoesAtivos(e.Vista).FindAll(b => b.name.StartsWith("Cartao")).Count);
            UiChecagem.BotoesUsaveis(e.Vista, "destino");

            yield return Tocar(e, "Cartao1");
            Assert.AreEqual(3, UiChecagem.BotoesAtivos(e.Vista).FindAll(b => b.name.StartsWith("Cartao")).Count, "tres origens");
            UiChecagem.BotoesUsaveis(e.Vista, "origem");

            yield return Tocar(e, "Cartao0");
            InputField campo = e.Vista.GetComponentInChildren<InputField>(false);
            Assert.IsNotNull(campo, "tela do nome sem campo");
            UiChecagem.BotoesUsaveis(e.Vista, "nome");
            campo.text = "A";
            yield return Tocar(e, "BotaoDireita");
            Assert.IsNotNull(e.Vista.GetComponentInChildren<InputField>(false), "nome curto segura na tela do nome");
            StringAssert.Contains(DestinySystem.NomeMinimo.ToString(), Texto(e, "Dica"), "o erro do nome aparece");

            campo.text = "Íris";
            yield return Tocar(e, "BotaoDireita");
            StringAssert.Contains("Íris", Texto(e, "Corpo"), "o tem certeza mostra o nome com acento");
            UiChecagem.BotoesUsaveis(e.Vista, "certeza");

            yield return Tocar(e, "BotaoEsquerda");   // Cancelar (B05)
            Assert.AreEqual(DestinyCatalog.Destinos.Length, UiChecagem.BotoesAtivos(e.Vista).FindAll(b => b.name.StartsWith("Cartao")).Count,
                "cancelar volta para a escolha de destino");
            Assert.IsTrue(string.IsNullOrEmpty(SaveState.Current.birth.destinyId), "cancelar nao confirma nada");
        }

        static IEnumerator Tocar(EntryFlow e, string botao)
        {
            UiChecagem.Botao(e.Vista, botao).onClick.Invoke();
            yield return null;
            yield return null;
        }

        static string Texto(EntryFlow e, string nome)
        {
            foreach (Text t in e.Vista.GetComponentsInChildren<Text>(false)) if (t.name == nome) return t.text;
            Assert.Fail("texto '" + nome + "' nao esta na tela");
            return null;
        }
    }
}
