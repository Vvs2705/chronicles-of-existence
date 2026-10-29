using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>BLOCO NOVO do save v3: o que o jogador comprou, por id de produto. Padrao neutro (lista vazia) = o que um
    /// save v2 significa: nada comprado. E da conta, nao da vida: SaveState.NovaVida leva o bloco para a vida nova.</summary>
    [Serializable]
    public class CompraData
    {
        public List<string> direitos = new List<string>();
    }

    /// <summary>Desbloqueio do episodio 2 (CONTRATO_V1 §2; Prompt Mestre §7). C# PURO.
    ///
    /// UM produto, nao consumivel, preco fixo e listado: nada aleatorio, nada de anuncio, nada de moeda premium.
    /// COMPRA EM DOIS PASSOS (LojaIap): a loja diz "pendente" -> Conceder grava o direito no save -> so entao a loja confirma.
    /// Conceder e idempotente: pendente repetido, restaurar e reinstalar nao duplicam nada.
    ///
    /// OFERTA (onde aparece): so no menu de pausa e logo depois do gancho do episodio 1 (B16), nunca no meio de cena.
    /// ponytail: o episodio 2 ainda nao existe, entao a oferta diz "em breve" e fica atras de OfertaLigada = false. Liga-se
    /// quando houver conteudo para vender e produto cadastrado na Play Console (com validacao de recibo antes de vender).</summary>
    public static class Compras
    {
        /// <summary>Id do produto na loja. NUNCA mudar depois de cadastrado na Play Console.</summary>
        public const string Episodio2 = "coe.episodio2";
        public static readonly string[] Catalogo = { Episodio2 };

        /// <summary>A oferta do episodio 2 aparece no jogo? Desligada ate existir o episodio (ver cabecalho).</summary>
        public const bool OfertaLigada = false;

        public static bool Conhecido(string produto) { return Array.IndexOf(Catalogo, produto) >= 0; }

        public static bool Tem(SaveData s, string produto)
        {
            return s != null && s.compras != null && s.compras.direitos != null && s.compras.direitos.Contains(produto);
        }

        /// <summary>Passo 1 da compra: grava o direito (uma vez). true = o jogador tem o produto agora, e SO entao a loja
        /// pode confirmar. Produto fora do catalogo: false, nada muda (a loja nao confirma e devolve o dinheiro).</summary>
        public static bool Conceder(SaveData s, string produto)
        {
            if (s == null || !Conhecido(produto)) return false;
            if (s.compras == null) s.compras = new CompraData();
            if (s.compras.direitos == null) s.compras.direitos = new List<string>();
            if (!s.compras.direitos.Contains(produto)) s.compras.direitos.Add(produto);
            return true;
        }

        /// <summary>Onde a oferta pode aparecer. `ligada` e OfertaLigada (ou o -oferta-ep2 de desenvolvimento).
        /// No menu: sempre que ligada. Fora do menu: so depois do gancho (B16), que e o fim do episodio 1.</summary>
        public static bool MostrarOferta(bool ligada, bool noMenu, LifeEventHistory historia)
        {
            if (!ligada) return false;
            return noMenu || (historia != null && historia.Ja(GameSession.MarcoGancho));
        }
    }
}
