using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>Os dialogos de Auren, como DADO deterministico. C# PURO, sem UnityEngine.
    ///
    /// ESCOPO (leva A, ADR-0007): os dez NPCs do NpcCatalog tem conversa de INFANCIA (5-7). Fala pos-salto e
    /// conteudo da leva B. O texto mora em Resources/strings.pt-BR.json (chave dialogo.&lt;npc&gt;.&lt;no&gt;);
    /// StringsCoberturaTests quebra se um no ou opcao daqui ficar sem texto.
    ///
    /// COMO A MISSAO ENTRA NA CONVERSA: objetivo com NPC e oferecido sozinho por MissaoNaConversa, em qualquer
    /// no, com o texto do proprio objetivo. Por isso quase nenhum grafo emite PedidoDeMissao: o no so da a
    /// FALA certa para o estado (Condicao.Missao) e o botao vem do dado de missao. A unica opcao autoral com
    /// pedido e a de Borin (q06), que MissaoNaConversa reconhece e nao duplica.
    ///
    /// Os ids de missao sao os do QuestCatalog. Os ids de evento citados em Lembra() so valem se o NPC os
    /// testemunha (NpcMemory.Testemunhos; DialogueGraphTests.Lembra_SoCitaEventoQueONpcTestemunha).
    /// O desfecho da q04 entra no historico na hora da escolha, entao Sera e Nilo ja falam diferente ANTES de a
    /// missao concluir -- e Nilo some ao concluir (evento.nilo_desapareceu, ADR-0007 decisao 3).
    ///
    /// O LIMIAR: ninguem em Auren o explica (e o misterio). Nenhum NPC sabe "topico.limiar".
    ///
    /// ponytail: tabela em codigo pelo mesmo motivo do NpcCatalog. Vira Resources/dialogos.json
    /// no dia em que um redator precisar editar sem compilar; DialogueGraph.Validar() ja e o validador
    /// desse arquivo quando ele existir.</summary>
    public static class DialogueCatalog
    {
        const string Q01 = "q01_um_novo_amanhecer";
        const string Q02 = "q02_uma_pequena_responsabilidade";
        const string Q04 = "q04_uma_promessa";
        const string Q07 = "q07_o_desaparecimento";
        const string MissaoSegredoDoFerreiro = "q06_o_segredo_do_ferreiro";

        const string EventoAjudouNaForja = "evento.q06_concluida";
        const string PromessaCumprida = "evento.q04_promessa_cumprida";
        const string PromessaQuebrada = "evento.q04_promessa_quebrada";

        const string Despedir = "dialogo.opcao.despedir";

        static DialogueOption Op(string textoKey, Condicao cond, string proximo, PedidoDeMissao pedido)
        {
            return new DialogueOption(textoKey, cond, proximo, pedido);
        }

        /// <summary>A saida incondicional de todo no com opcoes.</summary>
        static DialogueOption Sair(string textoKey = Despedir)
        {
            return new DialogueOption(textoKey, Condicao.Sempre, null, null);
        }

        static DialogueOption[] SoSair() { return new[] { Sair() }; }

        /// <summary>Mara, a mae. Sustenta a Q-01 (o primeiro dia) e reage ao sumico de Nilo como quem protege.
        /// Falas neutras quanto a origem: o oficio da familia muda com ela, a casa nao.</summary>
        static DialogueGraph Mara()
        {
            return new DialogueGraph("mara_casa", "mara", new[]
            {
                // B14: depois do salto (aos 8) estas vencem as falas da infancia.
                new DialogueNode("aos_oito", "dialogo.mara.aos_oito", Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), SoSair()),

                new DialogueNode("primeiro_dia", "dialogo.mara.primeiro_dia", Condicao.Missao(Q01, EstadoMissao.EmAndamento), new[]
                {
                    Op("dialogo.opcao.perguntar_familia", Condicao.Sabe("topico.familia"), "sobre_familia", null),
                    Sair(),
                }),

                new DialogueNode("nilo_sumiu", "dialogo.mara.nilo_sumiu", Condicao.Missao(Q07, EstadoMissao.EmAndamento), SoSair()),

                new DialogueNode("noite", "dialogo.mara.noite", Condicao.Periodo(TimeOfDay.Noite), new[]
                {
                    Sair("dialogo.opcao.boa_noite"),
                }),

                new DialogueNode("em_casa", "dialogo.mara.em_casa", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.perguntar_familia", Condicao.Sabe("topico.familia"), "sobre_familia", null),
                    Sair(),
                }),

                new DialogueNode("sobre_familia", "dialogo.mara.sobre_familia", Condicao.Sempre, SoSair()),
            });
        }

        /// <summary>Daren, o pai. Sustenta a Q-01 e e dono da Q-02: cobra a tarefa enquanto ela anda e lembra
        /// dela depois (memoria). A lembranca vem antes da noite porque concluir a q02 vira o periodo.</summary>
        static DialogueGraph Daren()
        {
            return new DialogueGraph("daren_oficio", "daren", new[]
            {
                // B14: depois do salto (aos 8) estas vencem as falas da infancia.
                new DialogueNode("aos_oito", "dialogo.daren.aos_oito", Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), SoSair()),

                new DialogueNode("primeiro_dia", "dialogo.daren.primeiro_dia", Condicao.Missao(Q01, EstadoMissao.EmAndamento), SoSair()),

                new DialogueNode("tarefa_pendente", "dialogo.daren.tarefa_pendente", Condicao.Missao(Q02, EstadoMissao.EmAndamento), SoSair()),

                new DialogueNode("tarefa_cumprida", "dialogo.daren.tarefa_cumprida", Condicao.Lembra("evento.q02_concluida"), new[]
                {
                    Op("dialogo.opcao.perguntar_oficio", Condicao.Sabe("topico.oficio_da_familia"), "sobre_oficio", null),
                    Sair(),
                }),

                new DialogueNode("noite", "dialogo.daren.noite", Condicao.Periodo(TimeOfDay.Noite), new[]
                {
                    Sair("dialogo.opcao.boa_noite"),
                }),

                new DialogueNode("em_casa", "dialogo.daren.em_casa", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.perguntar_oficio", Condicao.Sabe("topico.oficio_da_familia"), "sobre_oficio", null),
                    Sair(),
                }),

                new DialogueNode("sobre_oficio", "dialogo.daren.sobre_oficio", Condicao.Sempre, SoSair()),
            });
        }

        /// <summary>Borin, o ferreiro. Mostra: entrada por periodo (de noite ele nao esta na ferraria),
        /// entrada por memoria (ele lembra de quem ja ajudou) e uma opcao que EMITE pedido de missao.</summary>
        static DialogueGraph Borin()
        {
            return new DialogueGraph("borin_ferraria", "borin", new[]
            {
                // B14: depois do salto (aos 8) estas vencem as falas da infancia.
                new DialogueNode("aos_oito_espada", "dialogo.borin.aos_oito_espada", Condicao.E(Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), Condicao.Lembra("evento.q06_concluida")), SoSair()),

                new DialogueNode("aos_oito", "dialogo.borin.aos_oito", Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), SoSair()),

                // Noite: o mais especifico vem primeiro. A rotina diz que ele nao esta na forja.
                new DialogueNode("noite", "dialogo.borin.noite", Condicao.Periodo(TimeOfDay.Noite), new[]
                {
                    Sair("dialogo.opcao.boa_noite"),
                }),

                // Ja ajudou antes: ele lembra. A memoria e do NPC, o evento e do historico (T005).
                new DialogueNode("reencontro", "dialogo.borin.reencontro", Condicao.Lembra(EventoAjudouNaForja), new[]
                {
                    Op("dialogo.opcao.perguntar_metais", Condicao.Sabe("topico.metais"), "sobre_metais", null),
                    Sair(),
                }),

                // Fallback offline: sempre existe, e a ultima entrada possivel.
                new DialogueNode("primeira_vez", "dialogo.borin.primeira_vez", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.oferecer_ajuda",
                       Condicao.Missao(MissaoSegredoDoFerreiro, EstadoMissao.Disponivel),
                       "aceitou",
                       PedidoDeMissao.Iniciar(MissaoSegredoDoFerreiro)),
                    Op("dialogo.opcao.perguntar_metais", Condicao.Sabe("topico.metais"), "sobre_metais", null),
                    Sair(),
                }),

                new DialogueNode("sobre_metais", "dialogo.borin.sobre_metais", Condicao.Sempre, SoSair()),

                // Terminal: o NPC responde ao pedido e a conversa acaba. Se a missao recusar o pedido,
                // quem orquestra mostra o motivo -- o dialogo nao promete recompensa nenhuma.
                new DialogueNode("aceitou", "dialogo.borin.aceitou", Condicao.Sempre, new DialogueOption[0]),
            });
        }

        /// <summary>Lysa, a herbalista. Mostra conhecimento limitado: ela fala do bosque porque SABE do
        /// bosque; sobre o Limiar ela nao tem o que dizer, e a opcao nem aparece.</summary>
        static DialogueGraph Lysa()
        {
            return new DialogueGraph("lysa_ervas", "lysa", new[]
            {
                // B14: depois do salto (aos 8) estas vencem as falas da infancia.
                new DialogueNode("aos_oito_animal", "dialogo.lysa.aos_oito_animal", Condicao.E(Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), Condicao.Lembra("evento.q05_concluida")), SoSair()),

                new DialogueNode("aos_oito", "dialogo.lysa.aos_oito", Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), SoSair()),

                new DialogueNode("no_bosque", "dialogo.lysa.no_bosque", Condicao.Periodo(TimeOfDay.Tarde), new[]
                {
                    Op("dialogo.opcao.perguntar_bosque", Condicao.Sabe("topico.bosque"), "sobre_bosque", null),
                    Sair(),
                }),

                // Quem cuidou do animal com ela (q05) nao e recebido como estranho.
                new DialogueNode("depois_do_animal", "dialogo.lysa.depois_do_animal", Condicao.Lembra("evento.q05_concluida"), new[]
                {
                    Op("dialogo.opcao.perguntar_ervas", Condicao.Sabe("topico.ervas"), "sobre_ervas", null),
                    Sair(),
                }),

                new DialogueNode("em_casa", "dialogo.lysa.em_casa", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.perguntar_ervas", Condicao.Sabe("topico.ervas"), "sobre_ervas", null),
                    Op("dialogo.opcao.perguntar_limiar", Condicao.Sabe("topico.limiar"), "sobre_limiar", null),
                    Sair(),
                }),

                new DialogueNode("sobre_ervas", "dialogo.lysa.sobre_ervas", Condicao.Sempre, SoSair()),

                new DialogueNode("sobre_bosque", "dialogo.lysa.sobre_bosque", Condicao.Sempre, SoSair()),

                // Existe no dado, mas nenhum NPC de Auren sabe "topico.limiar": o no fica inalcancavel de
                // proposito (ADR-0007: ninguem em Auren explica o Limiar), e a fala dele tambem nao explica.
                // Validar() nao reclama de no inalcancavel porque o alcance depende de estado de jogo.
                new DialogueNode("sobre_limiar", "dialogo.lysa.sobre_limiar", Condicao.Sempre, new DialogueOption[0]),
            });
        }

        /// <summary>Tovin, guarda e cacador. Na Q-07 e ele quem aponta o bosque (B09); depois dela, lembra de
        /// quem foi junto.</summary>
        static DialogueGraph Tovin()
        {
            return new DialogueGraph("tovin_posto", "tovin", new[]
            {
                // B14: depois do salto (aos 8) estas vencem as falas da infancia.
                new DialogueNode("aos_oito", "dialogo.tovin.aos_oito", Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), SoSair()),

                new DialogueNode("rastro_no_bosque", "dialogo.tovin.rastro_no_bosque", Condicao.Missao(Q07, EstadoMissao.EmAndamento), SoSair()),

                new DialogueNode("depois_da_busca", "dialogo.tovin.depois_da_busca", Condicao.Lembra("evento.q07_concluida"), new[]
                {
                    Op("dialogo.opcao.perguntar_bosque", Condicao.Sabe("topico.bosque"), "sobre_bosque", null),
                    Sair(),
                }),

                new DialogueNode("noite", "dialogo.tovin.noite", Condicao.Periodo(TimeOfDay.Noite), new[]
                {
                    Sair("dialogo.opcao.boa_noite"),
                }),

                new DialogueNode("no_posto", "dialogo.tovin.no_posto", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.perguntar_bosque", Condicao.Sabe("topico.bosque"), "sobre_bosque", null),
                    Sair(),
                }),

                new DialogueNode("sobre_bosque", "dialogo.tovin.sobre_bosque", Condicao.Sempre, SoSair()),
            });
        }

        /// <summary>Eira, a educadora. Pista PARCIAL da Q-07: o que Nilo perguntou na vespera. Conhece historia
        /// antiga, mas nao o Limiar.</summary>
        static DialogueGraph Eira()
        {
            return new DialogueGraph("eira_aula", "eira", new[]
            {
                // B14: depois do salto (aos 8) estas vencem as falas da infancia.
                new DialogueNode("aos_oito", "dialogo.eira.aos_oito", Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), SoSair()),

                new DialogueNode("lugar_vazio", "dialogo.eira.lugar_vazio", Condicao.Missao(Q07, EstadoMissao.EmAndamento), SoSair()),

                new DialogueNode("noite", "dialogo.eira.noite", Condicao.Periodo(TimeOfDay.Noite), new[]
                {
                    Op("dialogo.opcao.perguntar_historia", Condicao.Sabe("topico.historia_de_eldoria"), "sobre_historia", null),
                    Sair("dialogo.opcao.boa_noite"),
                }),

                new DialogueNode("na_aula", "dialogo.eira.na_aula", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.perguntar_historia", Condicao.Sabe("topico.historia_de_eldoria"), "sobre_historia", null),
                    Sair(),
                }),

                new DialogueNode("sobre_historia", "dialogo.eira.sobre_historia", Condicao.Sempre, SoSair()),
            });
        }

        /// <summary>Nilo, amigo de infancia. Depois da escolha da Q-04 ele fala DIFERENTE conforme o desfecho
        /// (B08); e nesse no que o objetivo "sustentar_a_escolha" aparece. Ao concluir a q04 ele some.</summary>
        static DialogueGraph Nilo()
        {
            return new DialogueGraph("nilo_brincar", "nilo", new[]
            {
                // B14: depois do salto (aos 8) estas vencem as falas da infancia.
                new DialogueNode("aos_oito_cumprida", "dialogo.nilo.aos_oito_cumprida", Condicao.E(Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), Condicao.Lembra("evento.q04_promessa_cumprida")), SoSair()),

                new DialogueNode("aos_oito_quebrada", "dialogo.nilo.aos_oito_quebrada", Condicao.E(Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), Condicao.Lembra("evento.q04_promessa_quebrada")), SoSair()),

                new DialogueNode("aos_oito", "dialogo.nilo.aos_oito", Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), SoSair()),

                new DialogueNode("promessa_cumprida", "dialogo.nilo.promessa_cumprida", Condicao.Lembra(PromessaCumprida), SoSair()),

                new DialogueNode("promessa_quebrada", "dialogo.nilo.promessa_quebrada", Condicao.Lembra(PromessaQuebrada), SoSair()),

                new DialogueNode("na_escola", "dialogo.nilo.na_escola", Condicao.Periodo(TimeOfDay.Manha), new[]
                {
                    Sair("dialogo.opcao.depois_a_gente_vai"),
                }),

                new DialogueNode("chamando", "dialogo.nilo.chamando", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.vamos", Condicao.Sempre, "combinado", null),
                    Sair("dialogo.opcao.agora_nao"),
                }),

                new DialogueNode("combinado", "dialogo.nilo.combinado", Condicao.Sempre, new DialogueOption[0]),
            });
        }

        /// <summary>Sera, amiga e rival. Faz o pedido da Q-04 e, depois da escolha, fala DIFERENTE conforme o
        /// desfecho (B08). Quando Nilo some (q07 em andamento), a mesma escolha volta como custo: cada desfecho
        /// tem a sua resposta, e nenhuma das duas diz que o jogador acertou.</summary>
        static DialogueGraph Sera()
        {
            return new DialogueGraph("sera_promessa", "sera", new[]
            {
                // B14: depois do salto (aos 8) estas vencem as falas da infancia.
                new DialogueNode("aos_oito_cumprida", "dialogo.sera.aos_oito_cumprida", Condicao.E(Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), Condicao.Lembra("evento.q04_promessa_cumprida")), SoSair()),

                new DialogueNode("aos_oito_quebrada", "dialogo.sera.aos_oito_quebrada", Condicao.E(Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), Condicao.Lembra("evento.q04_promessa_quebrada")), SoSair()),

                new DialogueNode("aos_oito", "dialogo.sera.aos_oito", Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), SoSair()),

                new DialogueNode("promessa_cumprida", "dialogo.sera.promessa_cumprida", Condicao.Lembra(PromessaCumprida), new[]
                {
                    Op("dialogo.opcao.perguntar_nilo", Condicao.Missao(Q07, EstadoMissao.EmAndamento), "nilo_sumiu_cumprida", null),
                    Sair(),
                }),

                new DialogueNode("promessa_quebrada", "dialogo.sera.promessa_quebrada", Condicao.Lembra(PromessaQuebrada), new[]
                {
                    Op("dialogo.opcao.perguntar_nilo", Condicao.Missao(Q07, EstadoMissao.EmAndamento), "nilo_sumiu_quebrada", null),
                    Sair(),
                }),

                new DialogueNode("o_pedido", "dialogo.sera.o_pedido", Condicao.Missao(Q04, EstadoMissao.EmAndamento), SoSair()),

                new DialogueNode("na_praca", "dialogo.sera.na_praca", Condicao.Sempre, SoSair()),

                new DialogueNode("nilo_sumiu_cumprida", "dialogo.sera.nilo_sumiu_cumprida", Condicao.Sempre, SoSair()),

                new DialogueNode("nilo_sumiu_quebrada", "dialogo.sera.nilo_sumiu_quebrada", Condicao.Sempre, SoSair()),
            });
        }

        /// <summary>Oren, o comerciante. Pista PARCIAL da Q-07: o que ele viu Nilo carregar. Lembra de quem lhe
        /// fez favor (q03) e de quem cumpriu o recado (q02).</summary>
        static DialogueGraph Oren()
        {
            return new DialogueGraph("oren_banca", "oren", new[]
            {
                // B14: depois do salto (aos 8) estas vencem as falas da infancia.
                new DialogueNode("aos_oito_cesto", "dialogo.oren.aos_oito_cesto", Condicao.E(Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), Condicao.Lembra("evento.q03_concluida")), SoSair()),

                new DialogueNode("aos_oito", "dialogo.oren.aos_oito", Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), SoSair()),

                new DialogueNode("viu_nilo", "dialogo.oren.viu_nilo", Condicao.Missao(Q07, EstadoMissao.EmAndamento), SoSair()),

                new DialogueNode("cesto_achado", "dialogo.oren.cesto_achado", Condicao.Lembra("evento.q03_concluida"), new[]
                {
                    Op("dialogo.opcao.perguntar_comercio", Condicao.Sabe("topico.comercio"), "sobre_comercio", null),
                    Sair(),
                }),

                new DialogueNode("recado_feito", "dialogo.oren.recado_feito", Condicao.Lembra("evento.q02_concluida"), new[]
                {
                    Op("dialogo.opcao.perguntar_comercio", Condicao.Sabe("topico.comercio"), "sobre_comercio", null),
                    Sair(),
                }),

                new DialogueNode("noite", "dialogo.oren.noite", Condicao.Periodo(TimeOfDay.Noite), new[]
                {
                    Sair("dialogo.opcao.boa_noite"),
                }),

                new DialogueNode("na_banca", "dialogo.oren.na_banca", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.perguntar_comercio", Condicao.Sabe("topico.comercio"), "sobre_comercio", null),
                    Sair(),
                }),

                new DialogueNode("sobre_comercio", "dialogo.oren.sobre_comercio", Condicao.Sempre, SoSair()),
            });
        }

        /// <summary>Maelis, a administradora. Abre a Q-07 com o que a vila sabe (pista PARCIAL: desde quando) e,
        /// depois, lembra de quem investigou.</summary>
        static DialogueGraph Maelis()
        {
            return new DialogueGraph("maelis_mural", "maelis", new[]
            {
                // B14: depois do salto (aos 8) estas vencem as falas da infancia.
                new DialogueNode("aos_oito_registro", "dialogo.maelis.aos_oito_registro", Condicao.E(Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), Condicao.Lembra("evento.q07_concluida")), SoSair()),

                new DialogueNode("aos_oito", "dialogo.maelis.aos_oito", Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), SoSair()),

                new DialogueNode("ausencia", "dialogo.maelis.ausencia", Condicao.Missao(Q07, EstadoMissao.EmAndamento), SoSair()),

                new DialogueNode("depois_da_busca", "dialogo.maelis.depois_da_busca", Condicao.Lembra("evento.q07_concluida"), SoSair()),

                new DialogueNode("noite", "dialogo.maelis.noite", Condicao.Periodo(TimeOfDay.Noite), new[]
                {
                    Sair("dialogo.opcao.boa_noite"),
                }),

                new DialogueNode("no_mural", "dialogo.maelis.no_mural", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.perguntar_vila", Condicao.Sabe("topico.vila_auren"), "sobre_vila", null),
                    Sair(),
                }),

                new DialogueNode("sobre_vila", "dialogo.maelis.sobre_vila", Condicao.Sempre, SoSair()),
            });
        }

        /// <summary>Um grafo por NPC do NpcCatalog, na mesma ordem.</summary>
        public static readonly DialogueGraph[] Grafos =
        {
            Mara(), Daren(), Borin(), Lysa(), Tovin(), Eira(), Nilo(), Sera(), Oren(), Maelis(),
        };

        /// <summary>O grafo deste NPC, ou null se o id nao e de um NPC com fala. null nao e erro: a UI mostra
        /// o NPC sem conversa em vez de travar.</summary>
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
