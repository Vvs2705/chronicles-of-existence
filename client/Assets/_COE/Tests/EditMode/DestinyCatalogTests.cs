using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Catalogo de destinos e origens (T003). Puro: nao monta cena, nao le arquivo.
    /// Cobre o CONTRATO_T003_T004 secao 1 (ids congelados) e os invariantes do ADR-0004 que sao de DADO.</summary>
    public class DestinyCatalogTests
    {
        // Os ids do contrato secao 1. Se este teste quebrar, ou o contrato mudou ou o save antigo quebrou.
        static readonly string[] IdsDestino = { "serena", "normal", "dificil", "ruptura" };
        static readonly string[] IdsOrigem = { "agricultores", "artesaos", "guardioes" };

        [Test]
        public void QuatroDestinos_ComOsIdsDoContrato()
        {
            Assert.AreEqual(4, DestinyCatalog.Destinos.Length);
            foreach (string id in IdsDestino)
                Assert.IsNotNull(DestinyCatalog.Destino(id), "destino sumiu do catalogo: " + id);
        }

        [Test]
        public void TresOrigens_ComOsIdsDoContrato()
        {
            Assert.AreEqual(3, DestinyCatalog.Origens.Length);
            foreach (string id in IdsOrigem)
                Assert.IsNotNull(DestinyCatalog.Origem(id), "origem sumiu do catalogo: " + id);
        }

        [Test]
        public void IdDesconhecido_DevolveNullEmVezDeLancar()
        {
            Assert.IsNull(DestinyCatalog.Destino("facil"));
            Assert.IsNull(DestinyCatalog.Origem(""));
            Assert.IsNull(DestinyCatalog.Destino(null));
        }

        [Test]
        public void Ids_SaoSnakeCaseAsciiSemAcento()
        {
            List<string> ids = new List<string>();
            foreach (DestinyDef d in DestinyCatalog.Destinos) ids.Add(d.Id);
            foreach (OriginDef o in DestinyCatalog.Origens) ids.Add(o.Id);
            foreach (string a in DestinyCatalog.Afinidades) ids.Add(a);

            foreach (string id in ids)
            {
                Assert.IsFalse(string.IsNullOrEmpty(id));
                foreach (char c in id)
                    Assert.IsTrue((c >= 'a' && c <= 'z') || c == '_',
                        "id fora de snake_case ASCII (contrato secao 1): " + id);
            }
        }

        [Test]
        public void DozeCombinacoes_ProduzemCircunstanciaValida()
        {
            int combinacoes = 0;
            foreach (DestinyDef d in DestinyCatalog.Destinos)
                foreach (OriginDef o in DestinyCatalog.Origens)
                {
                    Circunstancia c = DestinyCatalog.Compor(d.Id, o.Id);
                    combinacoes++;

                    Assert.AreEqual(d.Id, c.DestinyId);
                    Assert.AreEqual(o.Id, c.OriginId);
                    string onde = d.Id + "/" + o.Id;
                    Assert.IsNotEmpty(c.FamiliaKey, onde);
                    Assert.IsNotEmpty(c.OficioKey, onde);
                    Assert.IsNotEmpty(c.ContextoSocialKey, onde);
                    Assert.Greater(c.MoedasIniciais, 0, onde);
                    SemBuraco(c.ItensIniciais, onde + " itens");
                    SemBuraco(c.Oportunidades, onde + " oportunidades");
                    SemBuraco(c.Acontecimentos, onde + " acontecimentos");
                    SemBuraco(c.AfinidadesFavorecidas, onde + " afinidades");
                }
            Assert.AreEqual(12, combinacoes, "4 destinos x 3 origens = 12 configuracoes (dossie secao C)");
        }

        static void SemBuraco(string[] lista, string onde)
        {
            Assert.IsNotNull(lista, onde);
            Assert.Greater(lista.Length, 0, onde + ": lista vazia deixa a configuracao sem dados");
            List<string> vistos = new List<string>();
            foreach (string s in lista)
            {
                Assert.IsNotEmpty(s, onde + ": id vazio");
                Assert.IsFalse(vistos.Contains(s), onde + ": id repetido " + s);
                vistos.Add(s);
            }
        }

        [Test]
        public void AfinidadesDeOrigem_EstaoNaListaDoContrato()
        {
            foreach (OriginDef o in DestinyCatalog.Origens)
                foreach (string a in o.AfinidadesFavorecidas)
                    Assert.Contains(a, DestinyCatalog.Afinidades,
                        o.Id + " favorece afinidade que nao existe no contrato secao 1: " + a);
        }

        [Test]
        public void Compor_ComIdDesconhecido_Lanca()
        {
            Assert.Throws<ArgumentException>(delegate { DestinyCatalog.Compor("facil", "agricultores"); });
            Assert.Throws<ArgumentException>(delegate { DestinyCatalog.Compor("serena", "nobres"); });
        }

        [Test]
        public void Compor_DevolveCopia_NinguemEditaOCatalogo()
        {
            Circunstancia c = DestinyCatalog.Compor("serena", "agricultores");
            int antes = DestinyCatalog.Destino("serena").ItensIniciais.Length;
            c.ItensIniciais[0] = "item.hackeado";
            Assert.AreEqual(antes, DestinyCatalog.Destino("serena").ItensIniciais.Length);
            Assert.AreNotEqual("item.hackeado", DestinyCatalog.Destino("serena").ItensIniciais[0],
                "definicao compartilhada nao pode ser alterada por quem consome a circunstancia");
        }

        // --- ADR-0004: destino nao e dificuldade ---

        static readonly string[] Proibidos =
            { "dificuldade", "difficulty", "assist", "multiplicador", "multiplier", "facil", "extrema" };

        [Test]
        public void CatalogoNaoTemCampoDeDificuldade()
        {
            Type[] tipos = { typeof(DestinyDef), typeof(OriginDef), typeof(Circunstancia), typeof(BirthChoice) };
            foreach (Type t in tipos)
            {
                foreach (FieldInfo f in t.GetFields(BindingFlags.Public | BindingFlags.Instance))
                    SemPalavraProibida(f.Name, t.Name + "." + f.Name);
                foreach (PropertyInfo p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    SemPalavraProibida(p.Name, t.Name + "." + p.Name);
            }
        }

        [Test]
        public void ChavesDeTexto_NaoRotulamDestinoComoDificuldade()
        {
            // ADR-0004: nome de destino descreve CIRCUNSTANCIA DE VIDA, nunca desafio mecanico.
            foreach (DestinyDef d in DestinyCatalog.Destinos)
            {
                SemPalavraProibida(d.RotuloKey, d.Id + ".rotulo");
                SemPalavraProibida(d.DescricaoKey, d.Id + ".descricao");
            }
        }

        static void SemPalavraProibida(string texto, string onde)
        {
            string s = texto == null ? "" : texto.ToLowerInvariant();
            foreach (string p in Proibidos)
                Assert.IsFalse(s.Contains(p),
                    onde + " usa \"" + p + "\": ADR-0004 separa destino, Grau e assistencia de combate");
        }

        [Test]
        public void CadaDestino_EOMaiorEmAlgumEixo()
        {
            // ADR-0004 invariante 4: nenhuma trajetoria e a rota obrigatoria; cada uma entrega algo que
            // as outras nao entregam. Quebra se alguem inflar um destino ate dominar tudo.
            foreach (DestinyDef d in DestinyCatalog.Destinos)
            {
                bool exclusivo =
                    UnicoMaior(d, delegate (DestinyDef x) { return x.MoedasIniciais; }) ||
                    UnicoMaior(d, delegate (DestinyDef x) { return x.ItensIniciais.Length; }) ||
                    UnicoMaior(d, delegate (DestinyDef x) { return x.Oportunidades.Length; }) ||
                    UnicoMaior(d, delegate (DestinyDef x) { return x.Acontecimentos.Length; });
                Assert.IsTrue(exclusivo, d.Id + " nao e o maior em nenhum eixo: vira escolha dominada");
            }
        }

        [Test]
        public void NenhumDestino_EOMaiorEmTodosOsEixos()
        {
            foreach (DestinyDef d in DestinyCatalog.Destinos)
            {
                bool domina =
                    UnicoMaior(d, delegate (DestinyDef x) { return x.MoedasIniciais; }) &&
                    UnicoMaior(d, delegate (DestinyDef x) { return x.ItensIniciais.Length; }) &&
                    UnicoMaior(d, delegate (DestinyDef x) { return x.Oportunidades.Length; }) &&
                    UnicoMaior(d, delegate (DestinyDef x) { return x.Acontecimentos.Length; });
                Assert.IsFalse(domina, d.Id + " domina todos os eixos: vira a escolha obrigatoria");
            }
        }

        static bool UnicoMaior(DestinyDef d, Func<DestinyDef, int> eixo)
        {
            foreach (DestinyDef outro in DestinyCatalog.Destinos)
                if (outro.Id != d.Id && eixo(outro) >= eixo(d)) return false;
            return true;
        }
    }
}
