using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>Os dialogos de Auren, como DADO deterministico. C# PURO, sem UnityEngine.
    ///
    /// ESCOPO v0: tres conversas de exemplo, uma por forma de condicao que o sistema precisa provar
    /// (periodo, memoria, conhecimento limitado, estado de missao + pedido). As falas das oito missoes do
    /// GDD cap. 07 e dos dez NPCs sao CONTEUDO: [a escrever], junto da T012.
    ///
    /// Os ids de missao sao os da T006 (QuestCatalog), e so o NPC da missao (content/quests npcs) emite
    /// pedido para ela. Os ids de evento citados em Lembra() sao do historico de vida da T005 e so valem se
    /// o NPC os testemunha (NpcMemory.Testemunhos); "evento.q06_concluida" e o EventoDeConclusao de Q-06.
    /// Quem oferece q03 (Oren) e q05 (Lysa) e conteudo da T012: nenhum dos dois tem pedido aqui ainda.
    ///
    /// ponytail: tabela em codigo pelo mesmo motivo do NpcCatalog. Vira Resources/dialogos.json
    /// no dia em que um redator precisar editar sem compilar; DialogueGraph.Validar() ja e o validador
    /// desse arquivo quando ele existir.</summary>
    public static class DialogueCatalog
    {
        const string MissaoSegredoDoFerreiro = "q06_o_segredo_do_ferreiro";
        const string EventoAjudouNaForja = "evento.q06_concluida";

        static DialogueOption Op(string textoKey, Condicao cond, string proximo, PedidoDeMissao pedido)
        {
            return new DialogueOption(textoKey, cond, proximo, pedido);
        }

        /// <summary>Borin, o ferreiro. Mostra: entrada por periodo (de noite ele nao esta na ferraria),
        /// entrada por memoria (ele lembra de quem ja ajudou) e uma opcao que EMITE pedido de missao.</summary>
        static DialogueGraph Borin()
        {
            return new DialogueGraph("borin_ferraria", "borin", new[]
            {
                // Noite: o mais especifico vem primeiro. A rotina diz que ele nao esta na forja.
                new DialogueNode("noite", "dialogo.borin.noite", Condicao.Periodo(TimeOfDay.Noite), new[]
                {
                    Op("dialogo.opcao.boa_noite", Condicao.Sempre, null, null),
                }),

                // Ja ajudou antes: ele lembra. A memoria e do NPC, o evento e do historico (T005).
                new DialogueNode("reencontro", "dialogo.borin.reencontro", Condicao.Lembra(EventoAjudouNaForja), new[]
                {
                    Op("dialogo.opcao.perguntar_metais", Condicao.Sabe("topico.metais"), "sobre_metais", null),
                    Op("dialogo.opcao.despedir", Condicao.Sempre, null, null),
                }),

                // Fallback offline: sempre existe, e a ultima entrada possivel.
                new DialogueNode("primeira_vez", "dialogo.borin.primeira_vez", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.oferecer_ajuda",
                       Condicao.Missao(MissaoSegredoDoFerreiro, EstadoMissao.Disponivel),
                       "aceitou",
                       PedidoDeMissao.Iniciar(MissaoSegredoDoFerreiro)),
                    Op("dialogo.opcao.perguntar_metais", Condicao.Sabe("topico.metais"), "sobre_metais", null),
                    Op("dialogo.opcao.despedir", Condicao.Sempre, null, null),
                }),

                new DialogueNode("sobre_metais", "dialogo.borin.sobre_metais", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.despedir", Condicao.Sempre, null, null),
                }),

                // Terminal: o NPC responde ao pedido e a conversa acaba. Se a missao recusar o pedido,
                // quem orquestra mostra o motivo -- o dialogo nao promete recompensa nenhuma.
                new DialogueNode("aceitou", "dialogo.borin.aceitou", Condicao.Sempre, new DialogueOption[0]),
            });
        }

        /// <summary>Lysa, a herbalista. Mostra conhecimento limitado: ela fala do bosque porque SABE do
        /// bosque; sobre o simbolo do Limiar ela nao tem o que dizer, e a opcao nem aparece.</summary>
        static DialogueGraph Lysa()
        {
            return new DialogueGraph("lysa_ervas", "lysa", new[]
            {
                new DialogueNode("no_bosque", "dialogo.lysa.no_bosque", Condicao.Periodo(TimeOfDay.Tarde), new[]
                {
                    Op("dialogo.opcao.perguntar_bosque", Condicao.Sabe("topico.bosque"), "sobre_bosque", null),
                    Op("dialogo.opcao.despedir", Condicao.Sempre, null, null),
                }),

                new DialogueNode("em_casa", "dialogo.lysa.em_casa", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.perguntar_ervas", Condicao.Sabe("topico.ervas"), "sobre_ervas", null),
                    Op("dialogo.opcao.perguntar_limiar", Condicao.Sabe("topico.limiar"), "sobre_limiar", null),
                    Op("dialogo.opcao.despedir", Condicao.Sempre, null, null),
                }),

                new DialogueNode("sobre_ervas", "dialogo.lysa.sobre_ervas", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.despedir", Condicao.Sempre, null, null),
                }),

                new DialogueNode("sobre_bosque", "dialogo.lysa.sobre_bosque", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.despedir", Condicao.Sempre, null, null),
                }),

                // Existe no dado, mas nenhum NPC de Auren sabe "topico.limiar" hoje: o no fica inalcancavel
                // de proposito ate o conteudo do Limiar existir. Validar() nao reclama de no inalcancavel
                // porque o alcance depende de estado de jogo, nao do dado.
                new DialogueNode("sobre_limiar", "dialogo.lysa.sobre_limiar", Condicao.Sempre, new DialogueOption[0]),
            });
        }

        /// <summary>Nilo, amigo de infancia. Conversa minima, sem missao: prova que um grafo sem pedido
        /// nenhum tambem e valido.</summary>
        static DialogueGraph Nilo()
        {
            return new DialogueGraph("nilo_brincar", "nilo", new[]
            {
                new DialogueNode("na_escola", "dialogo.nilo.na_escola", Condicao.Periodo(TimeOfDay.Manha), new[]
                {
                    Op("dialogo.opcao.depois_a_gente_vai", Condicao.Sempre, null, null),
                }),

                new DialogueNode("chamando", "dialogo.nilo.chamando", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.vamos", Condicao.Sempre, "combinado", null),
                    Op("dialogo.opcao.agora_nao", Condicao.Sempre, null, null),
                }),

                new DialogueNode("combinado", "dialogo.nilo.combinado", Condicao.Sempre, new DialogueOption[0]),
            });
        }

        public static readonly DialogueGraph[] Grafos = { Borin(), Lysa(), Nilo() };

        /// <summary>O grafo deste NPC, ou null se ele ainda nao tem fala escrita ([a escrever] para
        /// sete dos dez). null nao e erro: a UI mostra o NPC sem conversa em vez de travar.</summary>
        public static DialogueGraph Do(string npcId)
        {
            for (int i = 0; i < Grafos.Length; i++) if (Grafos[i].NpcId == npcId) return Grafos[i];
            return null;
        }

        /// <summary>Todos os problemas de dado do catalogo. Vazio = catalogo utilizavel.</summary>
        public static string[] ValidarTodos()
        {
            List<string> erros = new List<string>();
            foreach (DialogueGraph g in Grafos) erros.AddRange(g.Validar());
            return erros.ToArray();
        }
    }
}
