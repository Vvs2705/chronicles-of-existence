using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Iap = UnityEngine.Purchasing;

namespace COE
{
    /// <summary>Ponte com o Unity IAP 5 (o mesmo pacote e versao do Dungeon Courier, com.unity.purchasing 5.4.3; molde no
    /// LojaIap.cs dele). So orquestra: quem decide e grava e Compras / GameSession.ConcederCompra.
    ///
    /// COMPRA EM DOIS PASSOS: PurchaseProduct -> OnPurchasePending -> ConcederCompra grava o direito no save -> SO entao
    /// ConfirmPurchase (se nao concedeu, a loja nao confirma e devolve o dinheiro sozinha). Pendente de outra sessao,
    /// restaurar e reinstalar caem no mesmo caminho; ao conectar, as compras que a loja ja tem voltam (FetchPurchases).
    /// LOJA FALSA (FakeAppStore, "fake") fora do Android: PC e Editor compram de mentira, sem dinheiro. No Android, Google Play.
    /// ponytail: sem validacao de recibo no servidor; obrigatoria antes de ligar Compras.OfertaLigada para vender de verdade.</summary>
    public sealed class LojaIap
    {
        public enum Situacao { Conectando, Pronta, SemLoja }

        readonly Iap.StoreController store;
        readonly Func<GameSession> sessao;
        Action<bool> fim;
        string pedido;

        public bool Falsa { get; private set; }
        public Situacao Estado { get; private set; }

        /// <summary>Loja falsa fora do Android (PC/Editor de desenvolvimento).</summary>
        public static bool UsarLojaFalsa { get { return Application.platform != RuntimePlatform.Android; } }

        public LojaIap(Func<GameSession> sessao, bool falsa)
        {
            this.sessao = sessao;
            Falsa = falsa;
            store = Iap.UnityIAPServices.StoreController(falsa ? "fake" : null);   // "fake" = FakeAppStore.Name (internal no pacote)
            // todos os eventos antes do Connect: pendente de outra sessao pode chegar logo
            store.OnStoreConnected += Conectou;
            store.OnStoreDisconnected += f => { Estado = Situacao.SemLoja; Debug.Log("COE IAP desconectada: " + f.Message); };
            store.OnProductsFetched += Produtos;
            store.OnProductsFetchFailed += f => { Estado = Situacao.SemLoja; Debug.Log("COE IAP produtos falharam: " + f.FailureReason); };
            store.OnPurchasesFetched += Recebidas;
            store.OnPurchasesFetchFailed += f => Debug.Log("COE IAP compras falharam: " + f.Message);
            store.OnPurchasePending += Pendente;
            store.OnPurchaseConfirmed += o => { if (o is Iap.FailedOrder f) Debug.Log("COE IAP confirmacao falhou: " + f.FailureReason); };
            store.OnPurchaseFailed += Falhou;
            store.OnPurchaseDeferred += _ => Fim(false);   // aprovacao pendente (familia): chega depois como pendente
            Conecta();
        }

        async void Conecta()
        {
            try { await Task.Yield(); await store.Connect(); }
            catch (Exception e) { Estado = Situacao.SemLoja; Debug.Log("COE IAP sem conexao: " + e.Message); }
        }

        void Conectou()
        {
            var defs = new List<Iap.ProductDefinition>();
            foreach (string id in Compras.Catalogo) defs.Add(new Iap.ProductDefinition(id, Iap.ProductType.NonConsumable));
            store.FetchProducts(defs);
        }

        void Produtos(List<Iap.Product> lista)
        {
            Estado = Situacao.Pronta;
            Debug.Log("COE IAP: " + lista.Count + " produto(s) (" + (Falsa ? "loja falsa" : "Google Play") + ")");
            store.FetchPurchases();
        }

        void Recebidas(Iap.Orders os)
        {
            GameSession s = sessao();
            foreach (Iap.ConfirmedOrder o in os.ConfirmedOrders)
            {
                string id = IdDo(o);
                if (id != null && s != null) s.ConcederCompra(id);   // ja tem = nada (Compras e idempotente)
            }
        }

        /// <summary>Preco localizado da loja, ou null enquanto conecta / sem a loja / produto indisponivel.</summary>
        public string Preco(string produto)
        {
            Iap.Product p = Estado == Situacao.Pronta ? store.GetProductById(produto) : null;
            return p != null && p.availableToPurchase ? p.metadata.localizedPriceString : null;
        }

        public void Comprar(string produto, Action<bool> aoTerminar)
        {
            if (fim != null) return;   // ja ha compra em andamento
            Iap.Product p = Estado == Situacao.Pronta ? store.GetProductById(produto) : null;
            if (p == null || !p.availableToPurchase) { aoTerminar(false); return; }
            pedido = produto;
            fim = aoTerminar;
            store.PurchaseProduct(p);
        }

        public void Restaurar(Action<bool> aoTerminar)
        {
            if (Estado != Situacao.Pronta) { aoTerminar(false); return; }
            // a loja falsa nao tem RestoreTransactions: buscar as compras faz o mesmo
            if (Falsa) { store.FetchPurchases(); aoTerminar(true); return; }
            store.RestoreTransactions((ok, erro) =>
            {
                if (!ok) Debug.Log("COE IAP restaurar: " + erro);
                aoTerminar(ok);
            });
        }

        /// <summary>Passo 1 confirmado pela loja: concede e grava; SO entao confirma (passo 2).</summary>
        void Pendente(Iap.PendingOrder o)
        {
            string id = IdDo(o);
            GameSession s = sessao();
            bool concedeu = id != null && s != null && s.ConcederCompra(id);
            if (concedeu) store.ConfirmPurchase(o);
            else Debug.Log("COE IAP: " + (id ?? "pedido fora do catalogo") + " nao concedido; sem confirmar");
            if (id == pedido) Fim(concedeu);
        }

        void Falhou(Iap.FailedOrder f)
        {
            Debug.Log("COE IAP compra falhou: " + f.FailureReason + " " + f.Details);
            Fim(false);
        }

        void Fim(bool ok)
        {
            Action<bool> f = fim;
            fim = null;
            pedido = null;
            if (f != null) f(ok);
        }

        static string IdDo(Iap.Order o)
        {
            string id = o.CartOrdered?.Items().FirstOrDefault()?.Product?.definition?.id;
            return Compras.Conhecido(id) ? id : null;
        }
    }
}
