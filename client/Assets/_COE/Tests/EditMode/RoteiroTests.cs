using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Device lab: o robo nasce com o destino e a origem pedidos (-destino/-origem), e pedido que o catalogo
    /// recusa cai no robo de sempre (primeiro destino, primeira origem dele) em vez de travar o nascimento.</summary>
    public class RoteiroTests
    {
        [Test]
        public void EscolherNascimento_AsDozeCombinacoes_SaemComoPedidas()
        {
            int n = 0;
            foreach (DestinyDef d in DestinyCatalog.Destinos)
                foreach (OriginDef o in DestinySystem.OrigensDisponiveis(d.Id))
                {
                    string destino, origem;
                    Roteiro.EscolherNascimento(d.Id, o.Id, out destino, out origem);
                    Assert.AreEqual(d.Id, destino);
                    Assert.AreEqual(o.Id, origem);
                    n++;
                }
            Assert.AreEqual(12, n, "4 destinos x 3 origens");
        }

        [Test]
        public void EscolherNascimento_PedidoInvalidoOuAusente_CaiNoPrimeiro()
        {
            string destino, origem;
            string primeiro = DestinyCatalog.Destinos[0].Id;
            Roteiro.EscolherNascimento(null, null, out destino, out origem);
            Assert.AreEqual(primeiro, destino);
            Assert.AreEqual(DestinySystem.OrigensDisponiveis(primeiro)[0].Id, origem);

            Roteiro.EscolherNascimento("nao_existe", "agricultores", out destino, out origem);
            Assert.AreEqual(primeiro, destino, "destino fora do catalogo");

            string ultimo = DestinyCatalog.Destinos[DestinyCatalog.Destinos.Length - 1].Id;
            Roteiro.EscolherNascimento(ultimo, "nao_existe", out destino, out origem);
            Assert.AreEqual(ultimo, destino, "destino valido fica");
            Assert.AreEqual(DestinySystem.OrigensDisponiveis(ultimo)[0].Id, origem, "origem invalida = a primeira dele");
        }
    }
}
