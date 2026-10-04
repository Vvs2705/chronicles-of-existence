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
    /// pedido sao as de Borin (q06): iniciar a missao e o "ler o risco" do objetivo ajudar_borin, que so a fala cumpre
    /// (MissaoNaConversa.SoPelaFala; ficha G1 aprovada, ADR-0010).
    ///
    /// Os ids de missao sao os do QuestCatalog. Os ids de evento citados em Lembra() so valem se o NPC os
    /// testemunha (NpcMemory.Testemunhos; DialogueGraphTests.Lembra_SoCitaEventoQueONpcTestemunha).
    /// O desfecho da q04 entra no historico na hora da escolha, entao Sera e Nilo ja falam diferente ANTES de a
    /// missao concluir -- e Nilo some ao concluir (evento.nilo_desapareceu, ADR-0007 decisao 3).
    ///
    /// O LIMIAR: ninguem em Auren o explica (e o misterio). Nenhum NPC sabe "topico.limiar".
    ///
    /// NASCIMENTO (C8 das fichas G1, PROPOSTA aprovada por delegacao, ADR-0010): os nos destino_* e origem_* mudam a
    /// fala pelo destino, pela origem ou pelo item de nascimento (ids do DestinyCatalog). Fala que cita o objeto le o
    /// item (Condicao.TemItem); o resto le o destino ou a origem. ORDEM: entram DEPOIS de salto, missao, memoria e
    /// noite, e logo antes do fallback, cujas opcoes repetem -- por isso nao escondem fala de missao nem lembranca, e
    /// o pedido de missao do fallback (Borin, q06) continua na tela. Origem com mais de uma variante e o destino ja
    /// ocupando a entrada (Daren, Borin, Oren) entra por opcao: "olha o que eu trouxe" leva ao no origem_*.
    /// So fala: nenhuma opcao destes nos pede missao, item ou poder (dossie secao H). Sem nascimento (teste, -scene),
    /// nenhum deles passa e a conversa e a de antes.
    ///
    /// ponytail: tabela em codigo pelo mesmo motivo do NpcCatalog. Vira Resources/dialogos.json
    /// no dia em que um redator precisar editar sem compilar; DialogueGraph.Validar() ja e o validador
    /// desse arquivo quando ele existir.</summary>
    public static class DialogueCatalog
    {
        const string Q01 = "q01_um_novo_amanhecer";
        const string Q02 = "q02_uma_pequena_responsabilidade";
        const string Q04 = "q04_uma_promessa";
        const string Q05 = "q05_o_animal_ferido";
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

        /// <summary><paramref name="antes"/> + a crianca mostra a ferramenta da origem (o item que so aquela origem da),
        /// que leva ao origem_* do NPC + a saida. Das tres, so uma fica visivel por vez.</summary>
        static DialogueOption[] ComAFerramenta(string textoKey, params DialogueOption[] antes)
        {
            List<DialogueOption> r = new List<DialogueOption>(antes);
            r.Add(Op(textoKey, Condicao.TemItem("item.foice_pequena"), "origem_agricultores", null));
            r.Add(Op(textoKey, Condicao.TemItem("item.martelo_leve"), "origem_artesaos", null));
            r.Add(Op(textoKey, Condicao.TemItem("item.espada_de_madeira"), "origem_guardioes", null));
            r.Add(Sair());
            return r.ToArray();
        }

        /// <summary>Mara, a mae. Sustenta a Q-01 (o primeiro dia) e reage ao sumico de Nilo como quem protege.
        /// Falas neutras quanto a origem: o oficio da familia muda com ela, a casa nao. O destino (C8) muda o jeito de
        /// pedir o mesmo limite: toda variante repete "so ate a entrada do bosque", que e o papel dela no slice.</summary>
        static DialogueGraph Mara()
        {
            DialogueOption[] emCasa =
            {
                Op("dialogo.opcao.perguntar_familia", Condicao.Sabe("topico.familia"), "sobre_familia", null),
                Sair(),
            };
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

                new DialogueNode("destino_serena", "dialogo.mara.destino_serena", Condicao.Destino("serena"), emCasa),
                new DialogueNode("destino_normal", "dialogo.mara.destino_normal", Condicao.Destino("normal"), emCasa),
                new DialogueNode("destino_dificil", "dialogo.mara.destino_dificil", Condicao.Destino("dificil"), emCasa),
                new DialogueNode("destino_ruptura", "dialogo.mara.destino_ruptura", Condicao.Destino("ruptura"), emCasa),

                new DialogueNode("em_casa", "dialogo.mara.em_casa", Condicao.Sempre, emCasa),

                new DialogueNode("sobre_familia", "dialogo.mara.sobre_familia", Condicao.Sempre, SoSair()),
            });
        }

        /// <summary>Daren, o pai. Sustenta a Q-01 e e dono da Q-02: cobra a tarefa enquanto ela anda e lembra
        /// dela depois (memoria). A lembranca vem antes da noite porque concluir a q02 vira o periodo.
        /// Nascimento (C8): o destino vira um ditado dele antes da q02; a ferramenta pequena da origem foi ele quem fez,
        /// a olho, e o Borin conferiu ("O olho faz, a mao confere", C6).</summary>
        static DialogueGraph Daren()
        {
            DialogueOption[] emCasa = ComAFerramenta("dialogo.opcao.perguntar_ferramenta",
                Op("dialogo.opcao.perguntar_oficio", Condicao.Sabe("topico.oficio_da_familia"), "sobre_oficio", null));
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

                new DialogueNode("destino_serena", "dialogo.daren.destino_serena", Condicao.Destino("serena"), emCasa),
                new DialogueNode("destino_normal", "dialogo.daren.destino_normal", Condicao.Destino("normal"), emCasa),
                new DialogueNode("destino_dificil", "dialogo.daren.destino_dificil", Condicao.Destino("dificil"), emCasa),
                new DialogueNode("destino_ruptura", "dialogo.daren.destino_ruptura", Condicao.Destino("ruptura"), emCasa),

                new DialogueNode("em_casa", "dialogo.daren.em_casa", Condicao.Sempre, emCasa),

                new DialogueNode("sobre_oficio", "dialogo.daren.sobre_oficio", Condicao.Sempre, SoSair()),

                // Respostas a "quem fez a minha ferramenta?". Depois do fallback: nunca sao entrada.
                new DialogueNode("origem_agricultores", "dialogo.daren.origem_agricultores", Condicao.Sempre, SoSair()),
                new DialogueNode("origem_artesaos", "dialogo.daren.origem_artesaos", Condicao.Sempre, SoSair()),
                new DialogueNode("origem_guardioes", "dialogo.daren.origem_guardioes", Condicao.Sempre, SoSair()),
            });
        }

        /// <summary>Borin, o ferreiro. Mostra: entrada por periodo (de noite ele nao esta na ferraria),
        /// entrada por memoria (ele lembra de quem ja ajudou) e opcoes que EMITEM pedido de missao.
        /// LER O RISCO (ficha G1, docs/arte/fichas/borin.md C5): no ajudar_borin ele devolve a peca ("De novo"), depois
        /// estende o gabarito e o jogador le o entalhe. Errar so repete; acertar cumpre o objetivo e revela o segredo
        /// (a vista dele esta falhando), que o guardar_o_segredo pede para guardar.
        /// Nascimento (C8): ele mede o que a crianca traz na mao. O amuleto da Ruptura e o unico metal que ele nao mede
        /// ("nao sei o que e" so com Borin e Oren, ELENCO Arbitragem 2.8). As variantes repetem as opcoes do fallback,
        /// entao o pedido da q06 continua na tela.</summary>
        static DialogueGraph Borin()
        {
            DialogueOption[] primeiraVez = ComAFerramenta("dialogo.opcao.mostrar_o_de_casa",
                Op("dialogo.opcao.oferecer_ajuda",
                   Condicao.Missao(MissaoSegredoDoFerreiro, EstadoMissao.Disponivel),
                   "aceitou",
                   PedidoDeMissao.Iniciar(MissaoSegredoDoFerreiro)),
                // Entrou na ferraria nesta mesma conversa: segue para a forja sem precisar sair e voltar.
                Op("dialogo.opcao.ajudar_na_forja", Condicao.Objetivo(MissaoSegredoDoFerreiro, "ajudar_borin"), "na_forja", null),
                Op("dialogo.opcao.perguntar_metais", Condicao.Sabe("topico.metais"), "sobre_metais", null));
            return new DialogueGraph("borin_ferraria", "borin", new[]
            {
                // B14: depois do salto (aos 8) estas vencem as falas da infancia.
                new DialogueNode("aos_oito_espada", "dialogo.borin.aos_oito_espada", Condicao.E(Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), Condicao.Lembra("evento.q06_concluida")), SoSair()),

                new DialogueNode("aos_oito", "dialogo.borin.aos_oito", Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), SoSair()),

                // Noite: o mais especifico vem primeiro. A rotina diz que ele nao esta na forja.
                // Com o ajudar_borin pendente, ele diz quando voltar: sem isto o jogador fica sem saida (descansar
                // passa o periodo, ADR-0007 §1), e o -roteiro travava aqui.
                new DialogueNode("noite_forja", "dialogo.borin.noite_forja",
                    Condicao.E(Condicao.Periodo(TimeOfDay.Noite), Condicao.Objetivo(MissaoSegredoDoFerreiro, "ajudar_borin")), new[]
                {
                    Sair("dialogo.opcao.ate_amanha"),
                }),
                new DialogueNode("noite","dialogo.borin.noite", Condicao.Periodo(TimeOfDay.Noite), new[]
                {
                    Sair("dialogo.opcao.boa_noite"),
                }),

                // q06, objetivo ajudar_borin: a forja. Vem depois da noite (de noite ele nao esta la).
                new DialogueNode("na_forja", "dialogo.borin.na_forja", Condicao.Objetivo(MissaoSegredoDoFerreiro, "ajudar_borin"), new[]
                {
                    Op("dialogo.opcao.entregar_peca", Condicao.Sempre, "de_novo", null),
                    Sair(),
                }),

                // Ja ajudou antes: ele lembra. A memoria e do NPC, o evento e do historico (T005).
                new DialogueNode("reencontro", "dialogo.borin.reencontro", Condicao.Lembra(EventoAjudouNaForja), new[]
                {
                    Op("dialogo.opcao.perguntar_metais", Condicao.Sabe("topico.metais"), "sobre_metais", null),
                    Sair(),
                }),

                // O gancho e o objeto que so aquele destino da (a normal nao tem objeto: e o recado).
                new DialogueNode("destino_serena", "dialogo.borin.destino_serena", Condicao.TemItem("item.brinquedo_entalhado"), primeiraVez),
                new DialogueNode("destino_normal", "dialogo.borin.destino_normal", Condicao.Destino("normal"), primeiraVez),
                new DialogueNode("destino_dificil", "dialogo.borin.destino_dificil", Condicao.TemItem("item.faca_gasta"), primeiraVez),
                new DialogueNode("destino_ruptura", "dialogo.borin.destino_ruptura", Condicao.TemItem("item.amuleto_rachado"), primeiraVez),

                // Fallback offline: sempre existe, e a ultima entrada possivel.
                new DialogueNode("primeira_vez", "dialogo.borin.primeira_vez", Condicao.Sempre, primeiraVez),

                new DialogueNode("sobre_metais", "dialogo.borin.sobre_metais", Condicao.Sempre, SoSair()),

                // Respostas a "olha o que eu trouxe de casa". Depois do fallback: nunca sao entrada.
                new DialogueNode("origem_agricultores", "dialogo.borin.origem_agricultores", Condicao.Sempre, SoSair()),
                new DialogueNode("origem_artesaos", "dialogo.borin.origem_artesaos", Condicao.Sempre, SoSair()),
                new DialogueNode("origem_guardioes", "dialogo.borin.origem_guardioes", Condicao.Sempre, SoSair()),

                // Ler o risco. Depois do fallback: nunca sao entrada.
                new DialogueNode("de_novo", "dialogo.borin.de_novo", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.entregar_de_novo", Condicao.Sempre, "gabarito", null),
                    Sair(),
                }),
                new DialogueNode("gabarito", "dialogo.borin.gabarito", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.entalhe_segundo", Condicao.Sempre, "errou", null),
                    Op("dialogo.opcao.entalhe_terceiro", Condicao.Sempre, "leu_certo",
                       PedidoDeMissao.Objetivo(MissaoSegredoDoFerreiro, "ajudar_borin")),
                    Op("dialogo.opcao.entalhe_quarto", Condicao.Sempre, "errou", null),
                    Sair(),
                }),
                new DialogueNode("errou", "dialogo.borin.errou", Condicao.Sempre, new[]
                {
                    Op("dialogo.opcao.ler_de_novo", Condicao.Sempre, "gabarito", null),
                    Sair(),
                }),
                // Terminal: o guardar_o_segredo aparece aqui pela MissaoNaConversa.
                new DialogueNode("leu_certo", "dialogo.borin.leu_certo", Condicao.Sempre, new DialogueOption[0]),

                // Terminal: o NPC responde ao pedido e a conversa acaba. Se a missao recusar o pedido,
                // quem orquestra mostra o motivo -- o dialogo nao promete recompensa nenhuma.
                new DialogueNode("aceitou", "dialogo.borin.aceitou", Condicao.Sempre, new DialogueOption[0]),
            });
        }

        /// <summary>Lysa, a herbalista. Mostra conhecimento limitado: ela fala do bosque porque SABE do
        /// bosque; sobre o Limiar ela nao tem o que dizer, e a opcao nem aparece.
        /// Nascimento (C8): o destino muda o que ela ensina (na ervanaria); a origem muda o que ela diz na beira, de
        /// tarde (artesaos: sem variante). Na Ruptura ela mostra o nono feixe e nao sabe o nome; nao reage ao amuleto.
        /// q05 (ADR-0010 adendo 10, ficha lysa C5): com tratar_o_animal pendente ela ensina a chegar devagar, em qualquer
        /// periodo; o botao do objetivo so aparece com o bicho calmo (MissaoNaConversa.SoComBichoCalmo). O cantil (C8) fica
        /// de fora (PROPOSTA da ficha).</summary>
        static DialogueGraph Lysa()
        {
            DialogueOption[] noBosque =
            {
                Op("dialogo.opcao.perguntar_bosque", Condicao.Sabe("topico.bosque"), "sobre_bosque", null),
                Sair(),
            };
            DialogueOption[] emCasa =
            {
                Op("dialogo.opcao.perguntar_ervas", Condicao.Sabe("topico.ervas"), "sobre_ervas", null),
                Op("dialogo.opcao.perguntar_limiar", Condicao.Sabe("topico.limiar"), "sobre_limiar", null),
                Sair(),
            };
            return new DialogueGraph("lysa_ervas", "lysa", new[]
            {
                // B14: depois do salto (aos 8) estas vencem as falas da infancia.
                new DialogueNode("aos_oito_animal", "dialogo.lysa.aos_oito_animal", Condicao.E(Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), Condicao.Lembra("evento.q05_concluida")), SoSair()),

                new DialogueNode("aos_oito", "dialogo.lysa.aos_oito", Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), SoSair()),

                new DialogueNode("chegar_devagar", "dialogo.lysa.chegar_devagar", Condicao.Objetivo(Q05, "tratar_o_animal"), SoSair()),

                new DialogueNode("origem_agricultores", "dialogo.lysa.origem_agricultores", Condicao.E(Condicao.Periodo(TimeOfDay.Tarde), Condicao.Origem("agricultores")), noBosque),
                new DialogueNode("origem_guardioes", "dialogo.lysa.origem_guardioes", Condicao.E(Condicao.Periodo(TimeOfDay.Tarde), Condicao.Origem("guardioes")), noBosque),

                new DialogueNode("no_bosque", "dialogo.lysa.no_bosque", Condicao.Periodo(TimeOfDay.Tarde), noBosque),

                // Quem cuidou do animal com ela (q05) nao e recebido como estranho.
                new DialogueNode("depois_do_animal", "dialogo.lysa.depois_do_animal", Condicao.Lembra("evento.q05_concluida"), new[]
                {
                    Op("dialogo.opcao.perguntar_ervas", Condicao.Sabe("topico.ervas"), "sobre_ervas", null),
                    Sair(),
                }),

                new DialogueNode("destino_serena", "dialogo.lysa.destino_serena", Condicao.Destino("serena"), emCasa),
                new DialogueNode("destino_normal", "dialogo.lysa.destino_normal", Condicao.Destino("normal"), emCasa),
                new DialogueNode("destino_dificil", "dialogo.lysa.destino_dificil", Condicao.Destino("dificil"), emCasa),
                new DialogueNode("destino_ruptura", "dialogo.lysa.destino_ruptura", Condicao.Destino("ruptura"), emCasa),

                new DialogueNode("em_casa", "dialogo.lysa.em_casa", Condicao.Sempre, emCasa),

                new DialogueNode("sobre_ervas", "dialogo.lysa.sobre_ervas", Condicao.Sempre, SoSair()),

                new DialogueNode("sobre_bosque", "dialogo.lysa.sobre_bosque", Condicao.Sempre, SoSair()),

                // Existe no dado, mas nenhum NPC de Auren sabe "topico.limiar": o no fica inalcancavel de
                // proposito (ADR-0007: ninguem em Auren explica o Limiar), e a fala dele tambem nao explica.
                // Validar() nao reclama de no inalcancavel porque o alcance depende de estado de jogo.
                new DialogueNode("sobre_limiar", "dialogo.lysa.sobre_limiar", Condicao.Sempre, new DialogueOption[0]),
            });
        }

        /// <summary>Tovin, guarda e cacador. Na Q-07 e ele quem aponta o bosque (B09); depois dela, lembra de
        /// quem foi junto.
        /// Nascimento (C8): cada destino ganha um uso na conta dele (olho de ca, contagem, o no da corda puida, "pra
        /// casa"). A origem guardioes da conteudo a oportunidade.ronda_com_tovin: de manha, a ronda e ir com ele ao posto.</summary>
        static DialogueGraph Tovin()
        {
            DialogueOption[] noPosto =
            {
                Op("dialogo.opcao.perguntar_bosque", Condicao.Sabe("topico.bosque"), "sobre_bosque", null),
                Sair(),
            };
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

                // So de manha: de tarde a crianca de guardioes ouve a fala do destino, como as outras.
                new DialogueNode("origem_guardioes", "dialogo.tovin.origem_guardioes", Condicao.E(Condicao.Periodo(TimeOfDay.Manha), Condicao.Origem("guardioes")), noPosto),

                new DialogueNode("destino_serena", "dialogo.tovin.destino_serena", Condicao.Destino("serena"), noPosto),
                new DialogueNode("destino_normal", "dialogo.tovin.destino_normal", Condicao.Destino("normal"), noPosto),
                new DialogueNode("destino_dificil", "dialogo.tovin.destino_dificil", Condicao.TemItem("item.corda_puida"), noPosto),
                new DialogueNode("destino_ruptura", "dialogo.tovin.destino_ruptura", Condicao.Destino("ruptura"), noPosto),

                new DialogueNode("no_posto", "dialogo.tovin.no_posto", Condicao.Sempre, noPosto),

                new DialogueNode("sobre_bosque", "dialogo.tovin.sobre_bosque", Condicao.Sempre, SoSair()),
            });
        }

        /// <summary>Eira, a educadora. Pista PARCIAL da Q-07: o que Nilo perguntou na vespera. Conhece historia
        /// antiga, mas nao o Limiar.
        /// Nascimento (C8): aluna de dia (serena, normal), aluna de lanterna (Vida Ardua, que trabalha cedo) ou convidada
        /// da roda (ruptura). E convite de fala: nenhuma oportunidade muda. Sem variante de origem.</summary>
        static DialogueGraph Eira()
        {
            DialogueOption[] naAula =
            {
                Op("dialogo.opcao.perguntar_historia", Condicao.Sabe("topico.historia_de_eldoria"), "sobre_historia", null),
                Sair(),
            };
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

                new DialogueNode("destino_serena", "dialogo.eira.destino_serena", Condicao.Destino("serena"), naAula),
                new DialogueNode("destino_normal", "dialogo.eira.destino_normal", Condicao.Destino("normal"), naAula),
                new DialogueNode("destino_dificil", "dialogo.eira.destino_dificil", Condicao.Destino("dificil"), naAula),
                new DialogueNode("destino_ruptura", "dialogo.eira.destino_ruptura", Condicao.Destino("ruptura"), naAula),

                new DialogueNode("na_aula", "dialogo.eira.na_aula", Condicao.Sempre, naAula),

                new DialogueNode("sobre_historia", "dialogo.eira.sobre_historia", Condicao.Sempre, SoSair()),
            });
        }

        /// <summary>Nilo, amigo de infancia. Depois da escolha da Q-04 ele fala DIFERENTE conforme o desfecho
        /// (B08); e nesse no que o objetivo "sustentar_a_escolha" aparece. Ao concluir a q04 ele some.
        /// Nascimento (C8): o convite da tarde muda pelo destino (mesmas respostas do chamando). De manha, a crianca de
        /// guardioes anda com o Tovin, que e quem ele copia, e ele disfarca.</summary>
        static DialogueGraph Nilo()
        {
            DialogueOption[] naEscola = { Sair("dialogo.opcao.depois_a_gente_vai") };
            DialogueOption[] chamando =
            {
                Op("dialogo.opcao.vamos", Condicao.Sempre, "combinado", null),
                Sair("dialogo.opcao.agora_nao"),
            };
            return new DialogueGraph("nilo_brincar", "nilo", new[]
            {
                // B14: depois do salto (aos 8) estas vencem as falas da infancia.
                new DialogueNode("aos_oito_cumprida", "dialogo.nilo.aos_oito_cumprida", Condicao.E(Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), Condicao.Lembra("evento.q04_promessa_cumprida")), SoSair()),

                new DialogueNode("aos_oito_quebrada", "dialogo.nilo.aos_oito_quebrada", Condicao.E(Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), Condicao.Lembra("evento.q04_promessa_quebrada")), SoSair()),

                new DialogueNode("aos_oito", "dialogo.nilo.aos_oito", Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), SoSair()),

                new DialogueNode("promessa_cumprida", "dialogo.nilo.promessa_cumprida", Condicao.Lembra(PromessaCumprida), SoSair()),

                new DialogueNode("promessa_quebrada", "dialogo.nilo.promessa_quebrada", Condicao.Lembra(PromessaQuebrada), SoSair()),

                new DialogueNode("origem_guardioes", "dialogo.nilo.origem_guardioes", Condicao.E(Condicao.Periodo(TimeOfDay.Manha), Condicao.Origem("guardioes")), naEscola),

                new DialogueNode("na_escola", "dialogo.nilo.na_escola", Condicao.Periodo(TimeOfDay.Manha), naEscola),

                new DialogueNode("destino_serena", "dialogo.nilo.destino_serena", Condicao.Destino("serena"), chamando),
                new DialogueNode("destino_normal", "dialogo.nilo.destino_normal", Condicao.Destino("normal"), chamando),
                new DialogueNode("destino_dificil", "dialogo.nilo.destino_dificil", Condicao.Destino("dificil"), chamando),
                new DialogueNode("destino_ruptura", "dialogo.nilo.destino_ruptura", Condicao.Destino("ruptura"), chamando),

                new DialogueNode("chamando", "dialogo.nilo.chamando", Condicao.Sempre, chamando),

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

                // Nascimento (C8): rival de leitura (serena), de tudo (normal), escriba da palavra de quem nao vai a aula
                // (Vida Ardua) e cronista do que nao esta em historia nenhuma (ruptura). Sem variante de origem.
                new DialogueNode("destino_serena", "dialogo.sera.destino_serena", Condicao.Destino("serena"), SoSair()),
                new DialogueNode("destino_normal", "dialogo.sera.destino_normal", Condicao.Destino("normal"), SoSair()),
                new DialogueNode("destino_dificil", "dialogo.sera.destino_dificil", Condicao.Destino("dificil"), SoSair()),
                new DialogueNode("destino_ruptura", "dialogo.sera.destino_ruptura", Condicao.Destino("ruptura"), SoSair()),

                new DialogueNode("na_praca", "dialogo.sera.na_praca", Condicao.Sempre, SoSair()),

                new DialogueNode("nilo_sumiu_cumprida", "dialogo.sera.nilo_sumiu_cumprida", Condicao.Sempre, SoSair()),

                new DialogueNode("nilo_sumiu_quebrada", "dialogo.sera.nilo_sumiu_quebrada", Condicao.Sempre, SoSair()),
            });
        }

        /// <summary>Oren, o comerciante. Pista PARCIAL da Q-07: o que ele viu Nilo carregar. Lembra de quem lhe
        /// fez favor (q03) e de quem cumpriu o recado (q02).
        /// Nascimento (C8): "ler o que a crianca carrega" e so dele (ELENCO Arbitragem 2.9). O item do destino abre a
        /// conversa; a ferramenta da origem, a crianca mostra. O amuleto e a unica coisa que nao passou pela mao dele.</summary>
        static DialogueGraph Oren()
        {
            DialogueOption[] naBanca = ComAFerramenta("dialogo.opcao.mostrar_o_de_casa",
                Op("dialogo.opcao.perguntar_comercio", Condicao.Sabe("topico.comercio"), "sobre_comercio", null));
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

                new DialogueNode("destino_serena", "dialogo.oren.destino_serena", Condicao.TemItem("item.brinquedo_entalhado"), naBanca),
                new DialogueNode("destino_normal", "dialogo.oren.destino_normal", Condicao.TemItem("item.cantil"), naBanca),
                new DialogueNode("destino_dificil", "dialogo.oren.destino_dificil", Condicao.TemItem("item.faca_gasta"), naBanca),
                new DialogueNode("destino_ruptura", "dialogo.oren.destino_ruptura", Condicao.TemItem("item.amuleto_rachado"), naBanca),

                new DialogueNode("na_banca", "dialogo.oren.na_banca", Condicao.Sempre, naBanca),

                new DialogueNode("sobre_comercio", "dialogo.oren.sobre_comercio", Condicao.Sempre, SoSair()),

                // Respostas a "olha o que eu trouxe de casa". Depois do fallback: nunca sao entrada.
                new DialogueNode("origem_agricultores", "dialogo.oren.origem_agricultores", Condicao.Sempre, SoSair()),
                new DialogueNode("origem_artesaos", "dialogo.oren.origem_artesaos", Condicao.Sempre, SoSair()),
                new DialogueNode("origem_guardioes", "dialogo.oren.origem_guardioes", Condicao.Sempre, SoSair()),
            });
        }

        /// <summary>Maelis, a administradora. Abre a Q-07 com o que a vila sabe (pista PARCIAL: desde quando) e,
        /// depois, lembra de quem investigou.
        /// ASSINATURA (ADR-0010 adendo 11, ficha maelis C5): em perguntar_na_vila ela dita as tres pistas e pede o sinal; os
        /// dois botoes de desfecho vem do MissaoNaConversa (Decisoes). Aos 8 a fala lembra qual sinal ficou no livro; save
        /// sem assinatura (anterior a regra) cai no aos_oito_registro de sempre. So fala: nada e concedido.
        /// Nascimento (C8): o destino muda o que ela anota sobre a casa da crianca, no mural. A reacao da Ruptura ao
        /// circulo e a segunda folha ficam de fora (PROPOSTA da ficha). Sem variante de origem.</summary>
        static DialogueGraph Maelis()
        {
            DialogueOption[] noMural =
            {
                Op("dialogo.opcao.perguntar_vila", Condicao.Sabe("topico.vila_auren"), "sobre_vila", null),
                Sair(),
            };
            return new DialogueGraph("maelis_mural", "maelis", new[]
            {
                // B14: depois do salto (aos 8) estas vencem as falas da infancia.
                new DialogueNode("aos_oito_registro_circulo", "dialogo.maelis.aos_oito_registro_circulo", Condicao.E(Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), Condicao.Lembra(QuestCatalog.EventoAssinouComOCirculo)), SoSair()),
                new DialogueNode("aos_oito_registro_risco", "dialogo.maelis.aos_oito_registro_risco", Condicao.E(Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), Condicao.Lembra(QuestCatalog.EventoAssinouComUmRisco)), SoSair()),

                new DialogueNode("aos_oito_registro", "dialogo.maelis.aos_oito_registro", Condicao.E(Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), Condicao.Lembra("evento.q07_concluida")), SoSair()),

                new DialogueNode("aos_oito", "dialogo.maelis.aos_oito", Condicao.Lembra(AgeAdvanceCatalog.SaltoInfancia), SoSair()),

                // Antes da ausencia: com perguntar_na_vila pendente ela pede a assinatura. "Quem conta, assina."
                new DialogueNode("assinar", "dialogo.maelis.assinar", Condicao.Objetivo(Q07, "perguntar_na_vila"), SoSair()),

                new DialogueNode("ausencia", "dialogo.maelis.ausencia", Condicao.Missao(Q07, EstadoMissao.EmAndamento), SoSair()),

                new DialogueNode("depois_da_busca", "dialogo.maelis.depois_da_busca", Condicao.Lembra("evento.q07_concluida"), SoSair()),

                new DialogueNode("noite", "dialogo.maelis.noite", Condicao.Periodo(TimeOfDay.Noite), new[]
                {
                    Sair("dialogo.opcao.boa_noite"),
                }),

                new DialogueNode("destino_serena", "dialogo.maelis.destino_serena", Condicao.Destino("serena"), noMural),
                new DialogueNode("destino_normal", "dialogo.maelis.destino_normal", Condicao.Destino("normal"), noMural),
                new DialogueNode("destino_dificil", "dialogo.maelis.destino_dificil", Condicao.Destino("dificil"), noMural),
                new DialogueNode("destino_ruptura", "dialogo.maelis.destino_ruptura", Condicao.Destino("ruptura"), noMural),

                new DialogueNode("no_mural", "dialogo.maelis.no_mural", Condicao.Sempre, noMural),

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
