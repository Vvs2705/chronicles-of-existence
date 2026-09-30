using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>DEFINICAO de um destino de nascimento. Imutavel, compartilhada, nunca guarda escolha de
    /// jogador (CLAUDE.md: "ScriptableObject e definicao, nao estado" -- vale igual para tabela em codigo).
    ///
    /// Modificadores de CIRCUNSTANCIA (dossie secao C): familia, recursos iniciais, contexto social,
    /// oportunidades e acontecimentos. NAO existe campo de dificuldade, assistencia, multiplicador de
    /// dano ou HP de inimigo aqui -- ADR-0004 separa destino, Grau de Existencia e assistencia de
    /// combate em tres sistemas, e DestinyCatalogTests.CatalogoNaoTemCampoDeDificuldade prova isso por
    /// reflexao.</summary>
    public sealed class DestinyDef
    {
        public readonly string Id;                 // snake_case ASCII, congelado pelo contrato secao 1
        public readonly string RotuloKey;          // chave de Strings (Loc); o texto acentuado vive la
        public readonly string DescricaoKey;
        public readonly string FamiliaKey;         // condicao da familia NESTE destino
        public readonly string ContextoSocialKey;
        public readonly int MoedasIniciais;        // HIPOTESE v0: o GDD v1.2 nao publica numero de recurso inicial
        public readonly string[] ItensIniciais;    // HIPOTESE v0: ids de item, ainda sem catalogo de Inventory
        public readonly string[] Oportunidades;    // ids de oportunidade (T006/T007 os consomem)
        public readonly string[] Acontecimentos;   // ids de evento de vida (T005 os consome)

        public DestinyDef(string id, string rotuloKey, string descricaoKey, string familiaKey,
            string contextoSocialKey, int moedasIniciais, string[] itensIniciais,
            string[] oportunidades, string[] acontecimentos)
        {
            Id = id; RotuloKey = rotuloKey; DescricaoKey = descricaoKey; FamiliaKey = familiaKey;
            ContextoSocialKey = contextoSocialKey; MoedasIniciais = moedasIniciais;
            ItensIniciais = itensIniciais; Oportunidades = oportunidades; Acontecimentos = acontecimentos;
        }
    }

    /// <summary>DEFINICAO de uma origem familiar (arquetipo). Os mesmos tres arquetipos servem aos quatro
    /// destinos: dossie secao C fala em 12 CONFIGURACOES, nao 12 campanhas.</summary>
    public sealed class OriginDef
    {
        public readonly string Id;
        public readonly string RotuloKey;
        public readonly string DescricaoKey;
        public readonly string OficioKey;              // oficio da familia; muda dialogo e objetos da casa
        public readonly string[] ItensIniciais;        // HIPOTESE v0
        public readonly string[] Oportunidades;
        public readonly string[] AfinidadesFavorecidas; // ids de AffinityId do contrato secao 1; favorece, nao tranca

        public OriginDef(string id, string rotuloKey, string descricaoKey, string oficioKey,
            string[] itensIniciais, string[] oportunidades, string[] afinidadesFavorecidas)
        {
            Id = id; RotuloKey = rotuloKey; DescricaoKey = descricaoKey; OficioKey = oficioKey;
            ItensIniciais = itensIniciais; Oportunidades = oportunidades;
            AfinidadesFavorecidas = afinidadesFavorecidas;
        }
    }

    /// <summary>Uma das 12 configuracoes: destino x origem ja compostos. Continua sendo DEFINICAO
    /// derivada -- e recalculavel a partir do BirthChoice, entao nao vai para o save.</summary>
    public struct Circunstancia
    {
        public string DestinyId;
        public string OriginId;
        public string FamiliaKey;          // do destino: em que condicao essa familia vive
        public string OficioKey;           // da origem: do que essa familia vive
        public string ContextoSocialKey;
        public int MoedasIniciais;
        public string[] ItensIniciais;     // destino + origem
        public string[] Oportunidades;     // destino + origem
        public string[] Acontecimentos;    // do destino
        public string[] AfinidadesFavorecidas;
    }

    /// <summary>Catalogo dos 4 destinos e das 3 origens. C# PURO, sem UnityEngine, como MotionSolver:
    /// roda e e testavel sem abrir o editor.
    ///
    /// POR QUE TABELA EM CODIGO e nao ScriptableObject nem JSON: os ids sao congelados pelo contrato secao 1
    /// e o codigo precisa conhecer os quatro exaustivamente para validar -- dado que o codigo tem de
    /// saber de cor nao e dado externo, e asset/arquivo ainda torna o catalogo intestavel fora do Unity
    /// (SO) ou quebravel por arquivo ausente (JSON).
    /// ponytail: quando alguem que nao programa precisar editar isto, exportar para
    /// Resources/destinos.json seguindo o par Strings/StringsLoader que ja existe.</summary>
    public static class DestinyCatalog
    {
        /// <summary>AffinityId do contrato secao 1. Toda afinidade citada por uma origem precisa estar aqui.</summary>
        public static readonly string[] Afinidades =
            { "marcial", "arcana", "natural", "artesanal", "social", "exploratoria" };

        /// <summary>Os quatro destinos. Rotulos provisorios em Strings: destino.&lt;id&gt;.rotulo.
        /// Os numeros sao HIPOTESE v0 -- o GDD v1.2 (cap. 02) descreve as condicoes em prosa e nao
        /// publica recurso inicial. Regra de desenho que os testes cobram: cada destino e o maior em
        /// ALGUM eixo e nenhum e o maior em todos (ADR-0004, invariante 4: a Ruptura nao pode ser a
        /// rota obrigatoria ao poder).</summary>
        public static readonly DestinyDef[] Destinos =
        {
            new DestinyDef("serena", "destino.serena.rotulo", "destino.serena.descricao",
                "destino.serena.familia", "destino.serena.contexto_social",
                40,
                new[] { "item.manta_boa", "item.brinquedo_entalhado" },
                new[] { "oportunidade.aulas_com_eira", "oportunidade.tarde_livre" },
                new[] { "evento.festa_da_colheita" }),

            new DestinyDef("normal", "destino.normal.rotulo", "destino.normal.descricao",
                "destino.normal.familia", "destino.normal.contexto_social",
                25,
                new[] { "item.manta_simples", "item.cantil" },
                new[] { "oportunidade.aulas_com_eira", "oportunidade.recado_da_vila", "oportunidade.feira_de_auren" },
                new[] { "evento.visita_do_comerciante" }),

            new DestinyDef("dificil", "destino.dificil.rotulo", "destino.dificil.descricao",
                "destino.dificil.familia", "destino.dificil.contexto_social",
                8,
                new[] { "item.manta_remendada", "item.faca_gasta", "item.corda_puida" },
                new[] { "oportunidade.trabalho_cedo", "oportunidade.favor_do_vizinho" },
                new[] { "evento.ano_de_escassez" }),

            new DestinyDef("ruptura", "destino.ruptura.rotulo", "destino.ruptura.descricao",
                "destino.ruptura.familia", "destino.ruptura.contexto_social",
                18,
                new[] { "item.manta_simples", "item.amuleto_rachado" },
                new[] { "oportunidade.marca_do_limiar", "oportunidade.recado_da_vila" },
                new[] { "evento.anomalia_no_bosque", "evento.sonho_recorrente" }),
        };

        /// <summary>Os tres arquetipos familiares (GDD cap. 02). Nenhum e rico, virtuoso ou melhor:
        /// a condicao material vem do destino, o oficio vem daqui.</summary>
        public static readonly OriginDef[] Origens =
        {
            new OriginDef("agricultores", "origem.agricultores.rotulo", "origem.agricultores.descricao",
                "origem.agricultores.oficio",
                new[] { "item.foice_pequena" },
                new[] { "oportunidade.horta_da_familia" },
                new[] { "natural", "exploratoria" }),

            new OriginDef("artesaos", "origem.artesaos.rotulo", "origem.artesaos.descricao",
                "origem.artesaos.oficio",
                new[] { "item.martelo_leve" },
                new[] { "oportunidade.oficina_de_borin" },
                new[] { "artesanal", "social" }),

            new OriginDef("guardioes", "origem.guardioes.rotulo", "origem.guardioes.descricao",
                "origem.guardioes.oficio",
                new[] { "item.espada_de_madeira" },
                new[] { "oportunidade.ronda_com_tovin" },
                new[] { "marcial", "exploratoria" }),
        };

        /// <summary>null se o id nao existe (id vindo de save ou de UI e entrada suspeita, nao excecao).</summary>
        public static DestinyDef Destino(string id)
        {
            for (int i = 0; i < Destinos.Length; i++) if (Destinos[i].Id == id) return Destinos[i];
            return null;
        }

        public static OriginDef Origem(string id)
        {
            for (int i = 0; i < Origens.Length; i++) if (Origens[i].Id == id) return Origens[i];
            return null;
        }

        /// <summary>Compoe a configuracao de uma das 12 combinacoes. Lanca ArgumentException em id
        /// desconhecido: aqui o chamador ja passou por DestinySystem, entao id invalido e erro de
        /// programacao, nao escolha de jogador.</summary>
        public static Circunstancia Compor(string destinyId, string originId)
        {
            DestinyDef d = Destino(destinyId);
            OriginDef o = Origem(originId);
            if (d == null) throw new ArgumentException("destino desconhecido: " + destinyId, "destinyId");
            if (o == null) throw new ArgumentException("origem desconhecida: " + originId, "originId");

            Circunstancia c = default(Circunstancia);
            c.DestinyId = d.Id;
            c.OriginId = o.Id;
            c.FamiliaKey = d.FamiliaKey;
            c.OficioKey = o.OficioKey;
            c.ContextoSocialKey = d.ContextoSocialKey;
            c.MoedasIniciais = d.MoedasIniciais;
            c.ItensIniciais = Juntar(d.ItensIniciais, o.ItensIniciais);
            c.Oportunidades = Juntar(d.Oportunidades, o.Oportunidades);
            c.Acontecimentos = Juntar(d.Acontecimentos, null);
            c.AfinidadesFavorecidas = Juntar(o.AfinidadesFavorecidas, null);
            return c;
        }

        /// <summary>Copia defensiva: o chamador nunca recebe o array da definicao compartilhada, senao
        /// um sistema de runtime editaria o catalogo de todo mundo. Repetido nao entra duas vezes.</summary>
        static string[] Juntar(string[] a, string[] b)
        {
            List<string> r = new List<string>();
            if (a != null) foreach (string s in a) if (!r.Contains(s)) r.Add(s);
            if (b != null) foreach (string s in b) if (!r.Contains(s)) r.Add(s);
            return r.ToArray();
        }
    }
}
