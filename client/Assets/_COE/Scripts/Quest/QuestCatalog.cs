using System;

namespace COE
{
    /// <summary>Os quatro tipos de missao do dossie §H. Numero congelado: vai para log e telemetria.</summary>
    public enum QuestTipo
    {
        Cotidiana = 0,
        Social = 1,
        Exploratoria = 2,
        Narrativa = 3,
    }

    /// <summary>Um objetivo de missao. DEFINICAO: o que foi cumprido mora no save (QuestState), nunca aqui.
    /// TextoKey aponta para Strings (Loc); o texto acentuado nunca entra no id nem no save.</summary>
    public sealed class ObjetivoDef
    {
        public readonly string Id;        // snake_case, unico DENTRO da missao
        public readonly string TextoKey;

        public ObjetivoDef(string id, string textoKey) { Id = id; TextoKey = textoKey; }
    }

    /// <summary>Uma recompensa com ID PROPRIO. O id e a chave de idempotencia: a concessao pergunta ao
    /// historico de vida se este id ja foi concedido antes de conceder (dossie §H: "recompensa unica por
    /// ID", exploit nº 3 do backlog). Dois ids iguais em missoes diferentes = a segunda nunca paga, e
    /// QuestCatalogTests.RecompensaTemIdUnicoNoCatalogo quebra.
    ///
    /// Quem APLICA a recompensa nao e este modulo: Conceder devolve os ids aprovados e T012/Inventory
    /// aplica. Missao nao mexe em inventario direto (GDD cap. 10: "Quest ... nao deve alterar UI ou
    /// inventario sem validacao").</summary>
    public sealed class RecompensaDef
    {
        public readonly string Id;      // "rec.<questId>.<slug>" — congelado; mudar exige migracao de save
        public readonly string Tipo;    // "moedas" | "item" | "marco"  (camada cotidiana + marco narrativo)
        public readonly string Alvo;    // id do item ou do marco; "" para moedas
        public readonly int Quantidade; // HIPOTESE v0 — o GDD v1.2 nao publica numero de recompensa

        public RecompensaDef(string id, string tipo, string alvo, int quantidade)
        {
            Id = id; Tipo = tipo; Alvo = alvo; Quantidade = quantidade;
        }
    }

    /// <summary>DEFINICAO de uma missao. Imutavel e compartilhada; nenhum campo guarda progresso de
    /// partida (CLAUDE.md: "ScriptableObject e definicao, nao estado" — vale igual para tabela em codigo).</summary>
    public sealed class QuestDef
    {
        public readonly string Id;               // snake_case ASCII, congelado
        public readonly string TituloKey;        // Strings: missao.<id>.titulo
        public readonly QuestTipo Tipo;
        public readonly bool Central;            // false = opcional; opcional NUNCA e pre-requisito de central
        public readonly string[] PreMissoes;     // ids de missao que precisam estar Concluida
        public readonly string[] PreEventos;     // ids de evento de vida que precisam estar no historico (T005)
        public readonly bool ObjetivosEmOrdem;   // true = so o proximo pendente conta; false = paralelos
        public readonly ObjetivoDef[] Objetivos;
        public readonly RecompensaDef[] Recompensas;
        public readonly string EventoDeConclusao; // id do evento de vida gravado ao concluir (T005)

        public QuestDef(string id, string tituloKey, QuestTipo tipo, bool central,
            string[] preMissoes, string[] preEventos, bool objetivosEmOrdem,
            ObjetivoDef[] objetivos, RecompensaDef[] recompensas, string eventoDeConclusao)
        {
            Id = id; TituloKey = tituloKey; Tipo = tipo; Central = central;
            PreMissoes = preMissoes ?? new string[0];
            PreEventos = preEventos ?? new string[0];
            ObjetivosEmOrdem = objetivosEmOrdem;
            Objetivos = objetivos ?? new ObjetivoDef[0];
            Recompensas = recompensas ?? new RecompensaDef[0];
            EventoDeConclusao = eventoDeConclusao;
        }

        public ObjetivoDef Objetivo(string objetivoId)
        {
            for (int i = 0; i < Objetivos.Length; i++) if (Objetivos[i].Id == objetivoId) return Objetivos[i];
            return null;
        }
    }

    /// <summary>As oito missoes de Auren (GDD v1.2 cap. 07, tabela Q-01..Q-08; dossie §H). C# PURO, sem
    /// UnityEngine, como DestinyCatalog: roda e e testavel sem abrir o editor.
    ///
    /// POR QUE TABELA EM CODIGO e nao ScriptableObject nem JSON (1 linha, como o contrato pede):
    /// os ids sao contrato entre T006, T007 e T012 e o codigo precisa conhece-los para validar — dado que
    /// o codigo tem de saber de cor nao e dado externo, e asset/arquivo tornaria o catalogo intestavel
    /// fora do Unity (SO) ou quebravel por arquivo ausente (JSON).
    /// ponytail: quando o roteirista precisar editar isto sem programador, exportar para
    /// StreamingAssets/content/missoes.json seguindo o par Strings/StringsLoader que ja existe.
    ///
    /// O QUE AQUI E FATO DO GDD: os oito ids/titulos, o foco de cada uma e os quatro tipos.
    /// O QUE AQUI E HIPOTESE v0 (marcada [a escrever], decide T012): quais cinco sao centrais — o dossie
    /// §H diz "selecionar cinco centrais e tres opcionais conforme encadeamento final" e nao fixa quais;
    /// a divisao abaixo segue a sequencia do slice (§L: desaparecimento -> simbolo -> salto temporal),
    /// que obriga Q-07 e Q-08 a serem centrais. Tambem sao hipotese: a lista exata de objetivos, os
    /// numeros de recompensa e todo texto (nenhuma fala entra aqui — dialogo e T007).
    ///
    /// INVARIANTE ANTI-SOFTLOCK (backlog, teste obrigatorio nº 6): nenhuma missao CENTRAL tem missao
    /// OPCIONAL em PreMissoes. QuestCatalogTests.CentralNuncaDependeDeOpcional prova por varredura, e a
    /// campanha inteira fecha ignorando as tres opcionais.</summary>
    public static class QuestCatalog
    {
        public const string TipoMoedas = "moedas";
        public const string TipoItem = "item";
        public const string TipoMarco = "marco";

        const string Q01 = "q01_um_novo_amanhecer";
        const string Q02 = "q02_uma_pequena_responsabilidade";
        const string Q03 = "q03_o_cesto_perdido";
        const string Q04 = "q04_uma_promessa";
        const string Q05 = "q05_o_animal_ferido";
        const string Q06 = "q06_o_segredo_do_ferreiro";
        const string Q07 = "q07_o_desaparecimento";
        const string Q08 = "q08_ecos_do_limiar";

        public static readonly QuestDef[] Missoes =
        {
            // Q-01 "Familia, interacao, deslocamento" — abre o jogo; sem pre-requisito, senao nada comeca.
            new QuestDef(Q01, "missao.q01.titulo", QuestTipo.Cotidiana, true,
                null, null, true,
                new[]
                {
                    new ObjetivoDef("acordar", "missao.q01.obj.acordar"),                 // [a escrever]
                    new ObjetivoDef("falar_com_familia", "missao.q01.obj.falar_com_familia"), // [a escrever]
                    new ObjetivoDef("sair_de_casa", "missao.q01.obj.sair_de_casa"),       // [a escrever]
                },
                new[] { new RecompensaDef("rec." + Q01 + ".marco_primeiro_dia", TipoMarco, "marco.primeiro_dia", 1) },
                "evento.q01_concluida"),

            // Q-02 "Atividade e Experiencia de Vida" — a cotidiana que ensina o loop de tarefa.
            new QuestDef(Q02, "missao.q02.titulo", QuestTipo.Cotidiana, true,
                new[] { Q01 }, null, true,
                new[]
                {
                    new ObjetivoDef("receber_tarefa", "missao.q02.obj.receber_tarefa"),   // [a escrever]
                    new ObjetivoDef("cumprir_tarefa", "missao.q02.obj.cumprir_tarefa"),   // [a escrever]
                    new ObjetivoDef("prestar_contas", "missao.q02.obj.prestar_contas"),   // [a escrever]
                },
                new[]
                {
                    new RecompensaDef("rec." + Q02 + ".moedas", TipoMoedas, "", 5),       // HIPOTESE v0
                },
                "evento.q02_concluida"),

            // Q-03 "Exploracao com variantes modulares" — OPCIONAL: e a missao que muda mais por origem.
            new QuestDef(Q03, "missao.q03.titulo", QuestTipo.Exploratoria, false,
                new[] { Q01 }, null, false,   // objetivos PARALELOS: procurar em qualquer ordem
                new[]
                {
                    new ObjetivoDef("procurar_na_praca", "missao.q03.obj.procurar_na_praca"),   // [a escrever]
                    new ObjetivoDef("procurar_na_horta", "missao.q03.obj.procurar_na_horta"),   // [a escrever]
                    new ObjetivoDef("procurar_na_trilha", "missao.q03.obj.procurar_na_trilha"), // [a escrever]
                },
                new[] { new RecompensaDef("rec." + Q03 + ".item_cesto", TipoItem, "item.cesto_de_vime", 1) },
                "evento.q03_concluida"),

            // Q-04 "Escolha social e confianca" — central: a consequencia visivel que o slice quer provar.
            new QuestDef(Q04, "missao.q04.titulo", QuestTipo.Social, true,
                new[] { Q02 }, null, true,
                new[]
                {
                    new ObjetivoDef("ouvir_o_pedido", "missao.q04.obj.ouvir_o_pedido"),   // [a escrever]
                    new ObjetivoDef("decidir", "missao.q04.obj.decidir"),                 // [a escrever]
                    new ObjetivoDef("sustentar_a_escolha", "missao.q04.obj.sustentar_a_escolha"), // [a escrever]
                },
                new[] { new RecompensaDef("rec." + Q04 + ".marco_promessa", TipoMarco, "marco.promessa_feita", 1) },
                "evento.q04_concluida"),

            // Q-05 "Conhecimento e compaixao" — OPCIONAL.
            new QuestDef(Q05, "missao.q05.titulo", QuestTipo.Social, false,
                new[] { Q01 }, null, true,
                new[]
                {
                    new ObjetivoDef("encontrar_o_animal", "missao.q05.obj.encontrar_o_animal"), // [a escrever]
                    new ObjetivoDef("buscar_ajuda", "missao.q05.obj.buscar_ajuda"),             // [a escrever]
                    new ObjetivoDef("tratar_o_animal", "missao.q05.obj.tratar_o_animal"),       // [a escrever]
                },
                new[] { new RecompensaDef("rec." + Q05 + ".item_ervas", TipoItem, "item.ervas_de_lysa", 2) },
                "evento.q05_concluida"),

            // Q-06 "Profissao e dominio" — OPCIONAL. Dominio vem de provacao, nao de repeticao (dossie §H:
            // "dinheiro nao substitui dominio pessoal"); o ganho real de afinidade e T009, nao daqui.
            new QuestDef(Q06, "missao.q06.titulo", QuestTipo.Cotidiana, false,
                new[] { Q02 }, null, true,
                new[]
                {
                    new ObjetivoDef("entrar_na_ferraria", "missao.q06.obj.entrar_na_ferraria"), // [a escrever]
                    new ObjetivoDef("ajudar_borin", "missao.q06.obj.ajudar_borin"),             // [a escrever]
                    new ObjetivoDef("guardar_o_segredo", "missao.q06.obj.guardar_o_segredo"),   // [a escrever]
                },
                new[] { new RecompensaDef("rec." + Q06 + ".moedas", TipoMoedas, "", 8) },       // HIPOTESE v0
                "evento.q06_concluida"),

            // Q-07 "Investigacao e consequencias" — central. A partir daqui a campanha e narrativa.
            new QuestDef(Q07, "missao.q07.titulo", QuestTipo.Narrativa, true,
                new[] { Q04 }, null, true,
                new[]
                {
                    new ObjetivoDef("notar_a_ausencia", "missao.q07.obj.notar_a_ausencia"),   // [a escrever]
                    new ObjetivoDef("perguntar_na_vila", "missao.q07.obj.perguntar_na_vila"), // [a escrever]
                    new ObjetivoDef("seguir_ate_o_bosque", "missao.q07.obj.seguir_ate_o_bosque"), // [a escrever]
                },
                new[] { new RecompensaDef("rec." + Q07 + ".marco_desaparecimento", TipoMarco, "marco.desaparecimento", 1) },
                "evento.q07_concluida"),

            // Q-08 "Misterio principal e passagem temporal" — central e ULTIMA. O salto temporal em si e
            // T009 (exige confirmacao do jogador); esta missao so entrega o gancho e o marco.
            new QuestDef(Q08, "missao.q08.titulo", QuestTipo.Narrativa, true,
                new[] { Q07 }, null, true,
                new[]
                {
                    new ObjetivoDef("achar_o_simbolo", "missao.q08.obj.achar_o_simbolo"),   // [a escrever]
                    new ObjetivoDef("tocar_o_simbolo", "missao.q08.obj.tocar_o_simbolo"),   // [a escrever]
                },
                new[] { new RecompensaDef("rec." + Q08 + ".marco_eco_do_limiar", TipoMarco, "marco.eco_do_limiar", 1) },
                "evento.q08_concluida"),
        };

        /// <summary>null se o id nao existe. Id vindo de save, de dialogo ou de UI e entrada suspeita,
        /// nao erro de programacao — por isso null e nao excecao.</summary>
        public static QuestDef Missao(string id)
        {
            for (int i = 0; i < Missoes.Length; i++) if (Missoes[i].Id == id) return Missoes[i];
            return null;
        }
    }
}
