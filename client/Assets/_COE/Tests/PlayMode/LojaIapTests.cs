using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace COE.PlayModeTests
{
    /// <summary>Caminho do dinheiro na loja falsa do Unity IAP (molde: LojaIapTests do Dungeon Courier): comprar o
    /// episodio 2 vira pendente -> ConcederCompra grava no save -> so entao a loja confirma; comprar de novo nao grava,
    /// e restaurar devolve o direito a um save que nao o tem. Sessao em memoria: nenhum arquivo do jogador e tocado.</summary>
    public class LojaIapTests
    {
        [UnityTest]
        public IEnumerator LojaFalsa_CompraConcedeGravaEConfirma_RestaurarDevolve()
        {
            int gravacoes = 0;
            var g = new GameSession(new SaveData(), () => gravacoes++);
            var loja = new LojaIap(() => g, true);
            for (int i = 0; i < 600 && loja.Estado == LojaIap.Situacao.Conectando; i++) yield return null;
            Assert.AreEqual(LojaIap.Situacao.Pronta, loja.Estado, "a loja falsa conectou e trouxe o catalogo");
            Assert.IsNotNull(loja.Preco(Compras.Episodio2), "o produto tem preco");

            bool? ok = null;
            loja.Comprar(Compras.Episodio2, r => ok = r);
            for (int i = 0; i < 600 && ok == null; i++) yield return null;
            Assert.AreEqual(true, ok, "a compra terminou bem");
            Assert.IsTrue(Compras.Tem(g.Save, Compras.Episodio2), "o direito foi concedido");
            Assert.AreEqual(1, gravacoes, "e gravado uma vez");

            // Reinstalou / outro aparelho: save sem o direito, a loja ainda tem a compra.
            var outro = new GameSession(new SaveData(), null);
            g = outro;
            bool? restaurou = null;
            loja.Restaurar(r => restaurou = r);
            for (int i = 0; i < 600 && (restaurou == null || !Compras.Tem(outro.Save, Compras.Episodio2)); i++) yield return null;
            Assert.AreEqual(true, restaurou);
            Assert.IsTrue(Compras.Tem(outro.Save, Compras.Episodio2), "restaurar devolve o episodio 2");
        }
    }
}
