using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>Uma opcao de missao que a conversa oferece a partir do DADO de missao, sem fala escrita. TextoKey e a
    /// chave do titulo da missao, do objetivo ou do desfecho; Pedidos sao aplicados em ordem numa transicao so.</summary>
    public sealed class OpcaoDeMissao
    {
        public readonly string TextoKey;
        public readonly PedidoDeMissao[] Pedidos;

        public OpcaoDeMissao(string textoKey, params PedidoDeMissao[] pedidos) { TextoKey = textoKey; Pedidos = pedidos; }
    }

    /// <summary>T012: objetivo de missao que envolve NPC se cumpre CONVERSANDO com ele. C# PURO.
    ///
    /// DE ONDE VEM: content/quests/*.json diz quem participa (npcs da missao = quem a oferece; npcs de cada objetivo =
    /// com quem ele se cumpre). O QuestCatalog nao carrega isso; a copia em C# fica aqui, guardada por
    /// DialogueMissaoTests.Participantes_BatemComOJson (mesmo regime do ADR-0005 e do NpcMemory.Testemunhos).
    ///
    /// A REGRA DO DIALOGO CONTINUA: a opcao so PEDE (PedidoDeMissao pelo QuestIntentAdapter); quem decide e o
    /// QuestSystem. Nenhuma fala e inventada: o rotulo e a chave do proprio dado de missao.
    ///
    /// QUEM INICIA E QUEM CONCLUI: a conversa inicia as missoes do NPC (as automaticas, q01 e q08, comecam sozinhas no
    /// MissaoMundo). Concluir e do MissaoMundo.Avancar (MissaoHud, 5x/s), que pega tambem o que a conversa cumpriu e
    /// leva a recompensa ao inventario: um dono so para conclusao e recompensa.
    /// ponytail: tabela em codigo; vai para JSON junto com o QuestCatalog no dia do editor de conteudo.</summary>
    public static class MissaoNaConversa
    {
        static string[] N(params string[] npcs) { return npcs; }

        /// <summary>"questId" = npcs da missao; "questId/objetivoId" = npcs do objetivo. So entra quem tem npcs.</summary>
        public static readonly (string Chave, string[] Npcs)[] Participantes =
        {
            ("q01_um_novo_amanhecer", N("mara", "daren")),
            ("q01_um_novo_amanhecer/falar_com_familia", N("mara", "daren")),
            ("q02_uma_pequena_responsabilidade", N("daren", "oren")),
            ("q02_uma_pequena_responsabilidade/receber_tarefa", N("daren")),
            ("q02_uma_pequena_responsabilidade/cumprir_tarefa", N("oren")),
            ("q02_uma_pequena_responsabilidade/prestar_contas", N("daren")),
            ("q03_o_cesto_perdido", N("oren", "nilo")),
            ("q03_o_cesto_perdido/procurar_na_praca", N("oren")),
            ("q03_o_cesto_perdido/procurar_na_trilha", N("nilo")),
            ("q04_uma_promessa", N("sera", "nilo")),
            ("q04_uma_promessa/ouvir_o_pedido", N("sera")),
            ("q04_uma_promessa/decidir", N("sera", "nilo")),
            ("q04_uma_promessa/sustentar_a_escolha", N("nilo")),
            ("q05_o_animal_ferido", N("lysa", "tovin")),
            ("q05_o_animal_ferido/buscar_ajuda", N("lysa")),
            ("q05_o_animal_ferido/tratar_o_animal", N("lysa", "tovin")),
            ("q06_o_segredo_do_ferreiro", N("borin")),
            ("q06_o_segredo_do_ferreiro/entrar_na_ferraria", N("borin")),
            ("q06_o_segredo_do_ferreiro/ajudar_borin", N("borin")),
            ("q06_o_segredo_do_ferreiro/guardar_o_segredo", N("borin")),
            ("q07_o_desaparecimento", N("maelis", "eira", "oren", "tovin")),
            ("q07_o_desaparecimento/notar_a_ausencia", N("maelis")),
            // ADR-0010 adendo 11: so a assinatura no livro de Maelis fecha o objetivo. Eira e Oren dao a pista na fala deles.
            ("q07_o_desaparecimento/perguntar_na_vila", N("maelis")),
            ("q07_o_desaparecimento/seguir_ate_o_bosque", N("tovin")),
        };

        /// <summary>Objetivo em que o desfecho da missao e escolhido: em vez de "cumprir", a conversa oferece um botao
        /// por desfecho, e cada um grava o desfecho E cumpre o objetivo. HIPOTESE v0: o "decidir" da Q-04 e a escolha
        /// da promessa (slice B08). Q-07 (ADR-0010 adendo 11): a assinatura no livro de Maelis em perguntar_na_vila; o
        /// QuestCatalog a amarra (QuestDef.ObjetivoDoDesfecho) e DialogueMissaoTests confere que esta aqui tambem.</summary>
        public static readonly (string Missao, string Objetivo)[] Decisoes =
        {
            ("q04_uma_promessa", "decidir"),
            ("q07_o_desaparecimento", "perguntar_na_vila"),
        };

        /// <summary>ADR-0010 adendo 10 ("chegar devagar", ficha lysa C5): este objetivo so aparece na conversa com o bicho
        /// calmo (BichoNoChapeu, estado de cena). Com Lysa ou Tovin, em qualquer ancora.
        /// ponytail: um portao, um bool em Opcoes. Tabela {objetivo -> condicao de cena} quando houver o segundo.</summary>
        public const string SoComBichoCalmo = "q05_o_animal_ferido/tratar_o_animal";

        /// <summary>Objetivo que so a fala escrita cumpre: a conversa NAO oferece o atalho de dado, mesmo sem opcao
        /// autoral visivel. "ajudar_borin" e o "ler o risco" da ficha G1 do Borin (ADR-0010): ele devolve a peca, pede
        /// de novo e o jogador le o entalhe do gabarito (DialogueCatalog.Borin).</summary>
        public static readonly string[] SoPelaFala = { "q06_o_segredo_do_ferreiro/ajudar_borin" };

        /// <summary>Missao em andamento e <paramref name="objetivoId"/> e o proximo pendente (em missao sem ordem,
        /// qualquer pendente). E o que a Condicao.Objetivo do dialogo le.</summary>
        public static bool EhOProximo(QuestSystem missoes, string questId, string objetivoId)
        {
            QuestDef d = QuestCatalog.Missao(questId);
            if (missoes == null || d == null || missoes.Estado(questId) != QuestStatus.EmAndamento) return false;
            string[] feitos = missoes.ObjetivosFeitos(questId);
            foreach (ObjetivoDef o in d.Objetivos)
            {
                if (Array.IndexOf(feitos, o.Id) >= 0) continue;
                if (o.Id == objetivoId) return true;
                if (d.ObjetivosEmOrdem) return false;
            }
            return false;
        }

        /// <summary>Chave do botao de um desfecho, ex.: "dialogo.opcao.evento.q04_promessa_cumprida" ([a escrever]).</summary>
        public static string ChaveDoDesfecho(string eventoId) { return "dialogo.opcao." + eventoId; }

        /// <summary>O que a CRIANCA diz no botao de missao (ex.: "dialogo.fala.missao.q02.obj.cumprir_tarefa"). Sem ela o
        /// botao mostra o titulo/objetivo, que le como lista de tarefas e nao como conversa (simulacao -roteiro).</summary>
        public static string ChaveDaFala(string textoKey) { return "dialogo.fala." + textoKey; }

        public static bool Participa(string chave, string npcId)
        {
            for (int i = 0; i < Participantes.Length; i++)
                if (Participantes[i].Chave == chave) return Array.IndexOf(Participantes[i].Npcs, npcId) >= 0;
            return false;
        }

        static bool Decide(string questId, string objetivoId)
        {
            for (int i = 0; i < Decisoes.Length; i++)
                if (Decisoes[i].Missao == questId && Decisoes[i].Objetivo == objetivoId) return true;
            return false;
        }

        /// <summary>O que este NPC pode fazer andar AGORA: iniciar missao Disponivel dele; cumprir o objetivo pendente
        /// dele (em missao ordenada, so se for o proximo). Pula o que uma opcao autoral visivel ja pede (a Borin do
        /// DialogueCatalog ja oferece a q06) e o <see cref="SoComBichoCalmo"/> sem <paramref name="bichoCalmo"/>.
        /// Sem efeito colateral; lista vazia, nunca null.</summary>
        public static OpcaoDeMissao[] Opcoes(string npcId, QuestSystem missoes, DialogueOption[] autorais = null, bool bichoCalmo = false)
        {
            var r = new List<OpcaoDeMissao>();
            if (missoes == null || string.IsNullOrEmpty(npcId)) return r.ToArray();

            foreach (QuestDef d in QuestCatalog.Missoes)
            {
                QuestStatus st = missoes.Estado(d.Id);
                if (st == QuestStatus.Disponivel)
                {
                    if (Participa(d.Id, npcId)) Somar(r, autorais, new OpcaoDeMissao(d.TituloKey, PedidoDeMissao.Iniciar(d.Id)));
                    continue;
                }
                if (st != QuestStatus.EmAndamento) continue;

                string[] feitos = missoes.ObjetivosFeitos(d.Id);
                foreach (ObjetivoDef o in d.Objetivos)
                {
                    if (Array.IndexOf(feitos, o.Id) >= 0) continue;
                    string chave = d.Id + "/" + o.Id;
                    if (Participa(chave, npcId) && Array.IndexOf(SoPelaFala, chave) < 0 && (bichoCalmo || chave != SoComBichoCalmo))
                    {
                        PedidoDeMissao cumprir = PedidoDeMissao.Objetivo(d.Id, o.Id);
                        if (!Decide(d.Id, o.Id)) Somar(r, autorais, new OpcaoDeMissao(o.TextoKey, cumprir));
                        else
                            foreach (string ev in d.Desfechos)
                                Somar(r, autorais, new OpcaoDeMissao(ChaveDoDesfecho(ev),
                                    new PedidoDeMissao(d.Id, new QuestIntent(QuestAcao.EscolherDesfecho, ev)), cumprir));
                    }
                    if (d.ObjetivosEmOrdem) break;   // so o proximo pendente conta
                }
            }
            return r.ToArray();
        }

        static void Somar(List<OpcaoDeMissao> r, DialogueOption[] autorais, OpcaoDeMissao op)
        {
            if (autorais != null)
                foreach (DialogueOption a in autorais)
                    if (a != null && a.Pedido != null && Igual(a.Pedido, op.Pedidos[0])) return;
            r.Add(op);
        }

        static bool Igual(PedidoDeMissao a, PedidoDeMissao b)
        {
            return a.QuestId == b.QuestId && a.Intencao.Acao == b.Intencao.Acao && a.Intencao.ObjetivoId == b.Intencao.ObjetivoId;
        }

        /// <summary>Aplica os pedidos em ordem pelo QuestIntentAdapter; para no primeiro recusado e devolve a recusa.
        /// C# puro: quem grava e o GameSession.Missao que embrulha esta chamada.</summary>
        public static QuestResultado Aplicar(QuestSystem missoes, PedidoDeMissao[] pedidos)
        {
            if (pedidos == null || pedidos.Length == 0) return QuestIntentAdapter.Despachar(missoes, null);   // recusa, nada muda
            QuestResultado r = default(QuestResultado);
            foreach (PedidoDeMissao p in pedidos)
            {
                r = QuestIntentAdapter.Despachar(missoes, p);
                if (!r.Ok) return r;
            }
            return r;
        }
    }
}
