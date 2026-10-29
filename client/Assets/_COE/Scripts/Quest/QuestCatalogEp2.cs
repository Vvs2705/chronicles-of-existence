namespace COE
{
    /// <summary>As missoes do episodio 2, "Despertar dos Talentos" (docs/design/EPISODIO2_DESPERTAR_DOS_TALENTOS.md, PROPOSTA).
    /// C# PURO, como QuestCatalog. Espelho de content/quests/episodio2/*.json (paridade: QuestCatalogEp2Tests).
    ///
    /// POR QUE TABELA SEPARADA e nao mais linhas em QuestCatalog.Missoes: o episodio 1 e gratis e fecha sozinho, e o
    /// -roteiro dele nao pode mudar. QuestSystem, MissaoMundo e o salto so leem QuestCatalog, entao nada daqui aparece
    /// no jogo ate alguem ligar esta tabela de proposito.
    /// ponytail: hoje so define a q09 e o portao dela (Liberado). O teto e "dado + regra, sem jogo": a q09 ainda nao
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
        };

        /// <summary>Par {flag, evento}, como QuestCatalog.Flags.</summary>
        public static readonly string[][] Flags =
        {
            new[] { "primeira_ronda_feita", "evento.q09_concluida" },
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
