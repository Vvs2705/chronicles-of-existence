namespace COE
{
    /// <summary>As missoes do episodio 2, "Despertar dos Talentos" (docs/design/EPISODIO2_DESPERTAR_DOS_TALENTOS.md, PROPOSTA).
    /// C# PURO, como QuestCatalog. Espelho de content/quests/episodio2/*.json (paridade: QuestCatalogEp2Tests).
    ///
    /// POR QUE TABELA SEPARADA e nao mais linhas em QuestCatalog.Missoes: o episodio 1 e gratis e fecha sozinho, e o
    /// -roteiro dele nao pode mudar. QuestSystem, MissaoMundo e o salto so leem QuestCatalog, entao nada daqui aparece
    /// no jogo ate alguem ligar esta tabela de proposito.
    /// ponytail: hoje define o episodio inteiro (q09 a q16) e o portao (Liberado). O teto e "dado + regra, sem jogo": a q09 ainda nao
    /// entra no QuestSystem nem na cena (falta a ancora bosque_trilha e o encontro enc.trilha_abafadores_borda). Upgrade:
    /// o QuestSystem passa a consultar Missao() daqui quando Liberado for true, com teste de que o -roteiro do ep. 1
    /// continua igual sem a compra.
    ///
    /// INVARIANTE: nenhum id daqui (missao, transacao, evento, flag) repete um do QuestCatalog, senao uma recompensa do
    /// ep. 1 pagaria (ou deixaria de pagar) no ep. 2. QuestCatalogEp2Tests prova por varredura.</summary>
    public static class QuestCatalogEp2
    {
        /// <summary>Produto da loja que libera o episodio (Compras.Catalogo). E o mesmo "direito_de_loja" dos JSON.</summary>
        public const string Direito = Compras.Episodio2;

        const string Q08 = "q08_ecos_do_limiar";
        const string Q09 = "q09_a_linha_do_posto";
        const string Q10 = "q10_a_lousa_e_o_livro";
        const string Q11 = "q11_o_aro_de_provas";
        const string Q12 = "q12_as_ervas_que_curam";
        const string Q13 = "q13_o_fio_do_nilo";
        const string Q14 = "q14_a_festa_da_colheita";
        const string Q15 = "q15_a_anomalia_no_bosque";
        const string Q16 = "q16_o_enredado";

        /// <summary>E2-B13: o alarme no fim da festa, gravado junto da conclusao da q14 para TODO destino. Id novo, diferente de
        /// evento.anomalia_no_bosque (o acontecimento so da Ruptura, E2-B07).</summary>
        public const string EventoAnomaliaDespertou = "evento.anomalia_despertou";

        /// <summary>Os dois desfechos da q13 (E2-B11): contar a Maelis o que Nilo lembra, ou guardar com ele.</summary>
        public const string EventoContouAMaelis = "evento.q13_contou_a_maelis";
        public const string EventoGuardouComNilo = "evento.q13_guardou_com_nilo";

        /// <summary>Os dois desfechos da q11 (E2-B05): guardar o segredo de Borin ou conta-lo a Maelis. Exatamente um, cobrado
        /// em decidir_o_segredo, como a assinatura da Q-07. Nenhum e "o certo": mudam fala e objeto aos 11, nunca combate.</summary>
        public const string EventoSegredoGuardado = "evento.q11_segredo_guardado";
        public const string EventoSegredoContado = "evento.q11_segredo_contado";

        public static readonly QuestDef[] Missoes =
        {
            // E2-B03 "A linha do posto" — central e PRIMEIRA do ep. 2: aprendiz de ronda de Tovin, primeira luta na borda.
            // Pre-missao Q08 + evento dela: o mesmo encadeamento do ep. 1. O resto do portao (compra e gancho) e Liberado.
            new QuestDef(Q09, "missao.q09.titulo", QuestTipo.Cotidiana, true,
                new[] { Q08 }, new[] { "evento.q08_concluida" }, true,
                new[]
                {
                    new ObjetivoDef("ouvir_o_toque", "missao.q09.obj.ouvir_o_toque"),
                    new ObjetivoDef("ronda_ate_o_vao", "missao.q09.obj.ronda_ate_o_vao"),
                    new ObjetivoDef("ler_o_rastro", "missao.q09.obj.ler_o_rastro"),
                    new ObjetivoDef("primeira_luta_na_borda", "missao.q09.obj.primeira_luta_na_borda"), // encontro, sem NPC
                    new ObjetivoDef("guiar_a_volta", "missao.q09.obj.guiar_a_volta"),
                },
                new[]
                {
                    new RecompensaDef("rec." + Q09 + ".marco_primeira_ronda", QuestCatalog.TipoMarco, "marco.primeira_ronda", 1),
                    new RecompensaDef("rec." + Q09 + ".estacas", QuestCatalog.TipoItem, "item.estaca_de_freixo", 3), // HIPOTESE v0
                },
                "evento.q09_concluida"),

            // E2-B04 "A lousa e o livro" — central: le e escreve com Eira, le o livro de Maelis (a pagina da q07) e toca o
            // simbolo com o fio de luz. Libera o salto para os 11 (flag salto_onze_liberado). O_sinal_na_clareira nao tem NPC:
            // gatilho na ancora, como a Q-08.
            new QuestDef(Q10, "missao.q10.titulo", QuestTipo.Narrativa, true,
                new[] { Q09 }, new[] { "evento.q09_concluida" }, true,
                new[]
                {
                    new ObjetivoDef("aula_na_praca", "missao.q10.obj.aula_na_praca"),
                    new ObjetivoDef("responder_a_lousa", "missao.q10.obj.responder_a_lousa"),
                    new ObjetivoDef("ler_o_mural_a_noite", "missao.q10.obj.ler_o_mural_a_noite"),
                    new ObjetivoDef("a_pagina_assinada", "missao.q10.obj.a_pagina_assinada"),
                    new ObjetivoDef("o_sinal_na_clareira", "missao.q10.obj.o_sinal_na_clareira"),
                },
                new[]
                {
                    new RecompensaDef("rec." + Q10 + ".marco_primeira_leitura", QuestCatalog.TipoMarco, "marco.primeira_leitura", 1),
                    new RecompensaDef("rec." + Q10 + ".marco_fio_de_luz", QuestCatalog.TipoMarco, "marco.fio_de_luz", 1),
                },
                "evento.q10_concluida"),

            // E2-B05 "O aro de provas" — OPCIONAL: o segredo de Borin aos 8 (ADR-0010 §2), com desfecho em decidir_o_segredo.
            new QuestDef(Q11, "missao.q11.titulo", QuestTipo.Cotidiana, false,
                new[] { Q09 }, null, true,
                new[]
                {
                    new ObjetivoDef("voltar_a_ferraria", "missao.q11.obj.voltar_a_ferraria"),
                    new ObjetivoDef("ser_os_olhos", "missao.q11.obj.ser_os_olhos"),
                    new ObjetivoDef("forjar_os_cravos", "missao.q11.obj.forjar_os_cravos"),
                    new ObjetivoDef("decidir_o_segredo", "missao.q11.obj.decidir_o_segredo"),
                    new ObjetivoDef("entregar_a_tovin", "missao.q11.obj.entregar_a_tovin"),
                },
                new[]
                {
                    new RecompensaDef("rec." + Q11 + ".cravos", QuestCatalog.TipoItem, "item.cravo_de_marcar", 4), // HIPOTESE v0
                    new RecompensaDef("rec." + Q11 + ".moedas", QuestCatalog.TipoMoedas, "", 6),                   // HIPOTESE v0
                },
                "evento.q11_concluida",
                null,
                new[] { EventoSegredoGuardado, EventoSegredoContado },
                "decidir_o_segredo"),

            // E2-B06 "As ervas que curam" — OPCIONAL. So um marco: os unguentos da q15 chegam a todos (opcional nao vira
            // vantagem numerica, regra do B15).
            new QuestDef(Q12, "missao.q12.titulo", QuestTipo.Social, false,
                new[] { Q09 }, null, true,
                new[]
                {
                    new ObjetivoDef("colher_com_lysa", "missao.q12.obj.colher_com_lysa"),
                    new ObjetivoDef("a_tabuinha_de_sera", "missao.q12.obj.a_tabuinha_de_sera"),
                    new ObjetivoDef("preparar_o_unguento", "missao.q12.obj.preparar_o_unguento"),
                    new ObjetivoDef("usar_numa_ferida", "missao.q12.obj.usar_numa_ferida"),
                },
                new[] { new RecompensaDef("rec." + Q12 + ".marco_unguento", QuestCatalog.TipoMarco, "marco.unguento_de_lysa", 1) },
                "evento.q12_concluida"),

            // ---- Ato II, aos 11 (depois do salto que a q10 libera) ----

            // E2-B11 "O fio do Nilo" — central: Nilo de volta, o fio no espinhal (encontro enc.espinhal_feixes) e o segredo dele.
            new QuestDef(Q13, "missao.q13.titulo", QuestTipo.Narrativa, true,
                new[] { Q10 }, new[] { "evento.q10_concluida" }, true,
                new[]
                {
                    new ObjetivoDef("nilo_no_posto", "missao.q13.obj.nilo_no_posto"),
                    new ObjetivoDef("o_buraco_no_espinheiro", "missao.q13.obj.o_buraco_no_espinheiro"),
                    new ObjetivoDef("seguir_o_fio", "missao.q13.obj.seguir_o_fio"),               // encontro, sem NPC
                    new ObjetivoDef("o_que_nilo_lembra", "missao.q13.obj.o_que_nilo_lembra"),
                    new ObjetivoDef("decidir_o_segredo_de_nilo", "missao.q13.obj.decidir_o_segredo_de_nilo"),
                },
                new[] { new RecompensaDef("rec." + Q13 + ".marco_fio_do_nilo", QuestCatalog.TipoMarco, "marco.fio_do_nilo", 1) },
                "evento.q13_concluida",
                null,
                new[] { EventoContouAMaelis, EventoGuardouComNilo },
                "decidir_o_segredo_de_nilo"),

            // E2-B12/B13 "A festa da colheita" — central: tres bancas, a danca da corda e o chifre na noite. O alarme
            // (EventoAnomaliaDespertou) e gravado junto da conclusao. tres_bancas conta 3 de 6 festa.banca_* (cada uma com id).
            new QuestDef(Q14, "missao.q14.titulo", QuestTipo.Social, true,
                new[] { Q13 }, new[] { "evento.q13_concluida" }, true,
                new[]
                {
                    new ObjetivoDef("abrir_a_festa", "missao.q14.obj.abrir_a_festa"),
                    new ObjetivoDef("tres_bancas", "missao.q14.obj.tres_bancas"),
                    new ObjetivoDef("a_danca_da_corda", "missao.q14.obj.a_danca_da_corda"),
                    new ObjetivoDef("o_chifre_na_noite", "missao.q14.obj.o_chifre_na_noite"),
                },
                new[]
                {
                    new RecompensaDef("rec." + Q14 + ".fita", QuestCatalog.TipoItem, "item.fita_da_colheita", 1),
                    new RecompensaDef("rec." + Q14 + ".moedas", QuestCatalog.TipoMoedas, "", 5),   // HIPOTESE v0
                },
                "evento.q14_concluida",
                new[] { EventoAnomaliaDespertou }),

            // E2-B14 "A descida" — central: a trilha, o lajedo e o coracao do bosque. Os unguentos chegam a todos.
            new QuestDef(Q15, "missao.q15.titulo", QuestTipo.Exploratoria, true,
                new[] { Q14 }, new[] { "evento.q14_concluida" }, true,
                new[]
                {
                    new ObjetivoDef("reunir_na_linha", "missao.q15.obj.reunir_na_linha"),
                    new ObjetivoDef("marcar_o_caminho", "missao.q15.obj.marcar_o_caminho"),
                    new ObjetivoDef("atravessar_a_trilha", "missao.q15.obj.atravessar_a_trilha"), // enc.trilha_misto
                    new ObjetivoDef("o_lajedo", "missao.q15.obj.o_lajedo"),                       // enc.lajedo_lajeiro
                    new ObjetivoDef("chegar_ao_coracao", "missao.q15.obj.chegar_ao_coracao"),
                },
                new[]
                {
                    new RecompensaDef("rec." + Q15 + ".unguentos", QuestCatalog.TipoItem, "item.unguento", 2),   // HIPOTESE v0
                    new RecompensaDef("rec." + Q15 + ".marco_noite_no_bosque", QuestCatalog.TipoMarco, "marco.noite_no_bosque", 1),
                },
                "evento.q15_concluida"),

            // E2-B15/B16 "O Enredado" — central e ULTIMA: o encontro maior (enc.coracao_enredado) e os fios ate a clareira.
            new QuestDef(Q16, "missao.q16.titulo", QuestTipo.Narrativa, true,
                new[] { Q15 }, new[] { "evento.q15_concluida" }, true,
                new[]
                {
                    new ObjetivoDef("ver_o_enredado", "missao.q16.obj.ver_o_enredado"),
                    new ObjetivoDef("cortar_os_tres_nos", "missao.q16.obj.cortar_os_tres_nos"),
                    new ObjetivoDef("seguir_os_fios", "missao.q16.obj.seguir_os_fios"),
                },
                new[] { new RecompensaDef("rec." + Q16 + ".marco_enredado_livre", QuestCatalog.TipoMarco, "marco.enredado_livre", 1) },
                "evento.q16_concluida"),
        };

        /// <summary>Par {flag, evento}, como QuestCatalog.Flags.</summary>
        public static readonly string[][] Flags =
        {
            new[] { "primeira_ronda_feita", "evento.q09_concluida" },
            new[] { "sabe_ler", "evento.q10_concluida" },
            new[] { "fio_de_luz_aprendido", "evento.q10_concluida" },
            new[] { "salto_onze_liberado", "evento.q10_concluida" },
            new[] { "aprendiz_de_borin", "evento.q11_concluida" },
            new[] { "preparou_unguento", "evento.q12_concluida" },
            new[] { "fio_achado", "evento.q13_concluida" },
            new[] { "festa_vivida", "evento.q14_concluida" },
            new[] { "alarme_tocado", EventoAnomaliaDespertou },
            new[] { "desceu_ao_coracao", "evento.q15_concluida" },
            new[] { "enredado_libertado", "evento.q16_concluida" },
            new[] { "segundo_vao_visto", "evento.q16_concluida" },
        };

        /// <summary>null se o id nao e do episodio 2.</summary>
        public static QuestDef Missao(string id)
        {
            for (int i = 0; i < Missoes.Length; i++) if (Missoes[i].Id == id) return Missoes[i];
            return null;
        }

        /// <summary>E2-B00: o episodio 2 esta aberto neste save? So com as duas coisas: o direito da loja E o gancho do
        /// episodio 1 visto (GameSession.MarcoGancho, B16). Sem a compra, o jogador segue em Auren aos 8 e nada aparece;
        /// comprar depois abre sem reiniciar o save. Save null le como fechado, nao lanca.</summary>
        public static bool Liberado(SaveData save)
        {
            if (save == null || !Compras.Tem(save, Direito)) return false;
            return new LifeEventHistory(save).Ja(GameSession.MarcoGancho);
        }
    }
}
