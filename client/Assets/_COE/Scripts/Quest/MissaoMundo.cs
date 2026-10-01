using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>T012: o lado "mundo" das missoes, em C# PURO — o que comeca sozinho, qual gatilho de ancora vale agora,
    /// o que o HUD mostra e a conclusao quando tudo foi feito. Quem tem cena (MissaoHud, QuestTrigger) so chama isto.
    ///
    /// Toda mudanca vai por GameSession.Missao: sincroniza e grava UMA vez, ou nada (contrato do coordenador).
    /// O inventario alcanca o historico DENTRO da mesma transicao (Inventario.Sincronizar), entao moeda/item e o rec.*
    /// que os concedeu saem na mesma gravacao.
    ///
    /// FRONTEIRA COM O DIALOGO (raia L18): objetivo com NPC, inicio de missao por conversa e desfecho da Q-04 sao do
    /// dialogo. Aqui so: inicio automatico, objetivo SEM NPC (gatilho na ancora, ou automatico) e concluir quando nao
    /// falta nada. Concluir faz o dia andar um periodo (ADR-0007 §1): quem aplica e a GameSession.Missao.</summary>
    public static class MissaoMundo
    {
        /// <summary>Missoes que comecam sozinhas ao ficar disponiveis (content/quests "inicio_automatico": true).
        /// q01: o despertar abre o jogo (slice B06). q08: nao tem NPC nenhum, ninguem a oferece (B11); comeca quando
        /// a q07 conclui. As outras comecam falando com o NPC do primeiro objetivo (dialogo).
        /// QuestDataParityTests confere com os JSON.</summary>
        public static readonly string[] Automaticas = { "q01_um_novo_amanhecer", "q08_ecos_do_limiar" };

        /// <summary>{missao, objetivo}: objetivo que se cumpre SOZINHO assim que e o proximo pendente (content/quests:
        /// "automatico": true no objetivo). Nao tem gatilho de ancora nem NPC.
        /// q01.acordar (ADR-0007 §6): o jogo abre com a crianca ja acordada; o primeiro ato do jogador e falar com a
        /// familia. O id continua no catalogo (publicado, vai para o save); so deixou de pedir um toque.
        /// Save antigo com a q01 EmAndamento e "acordar" pendente se resolve no proximo Avancar, sem migracao.</summary>
        public static readonly string[][] ObjetivosAutomaticos =
        {
            new[] { "q01_um_novo_amanhecer", "acordar" },
        };

        /// <summary>{missao, objetivo, ancora}: todo objetivo de content/quests com ancora, SEM npcs e nao automatico.
        /// Cada um vira um QuestTrigger na ancora. QuestDataParityTests confere com os JSON nos dois sentidos.</summary>
        public static readonly string[][] Gatilhos =
        {
            new[] { "q01_um_novo_amanhecer", "sair_de_casa", "spawn_player" },
            new[] { "q03_o_cesto_perdido", "procurar_na_horta", "horta_familia" },
            new[] { "q05_o_animal_ferido", "encontrar_o_animal", "entrada_bosque" },
            new[] { "q08_ecos_do_limiar", "achar_o_simbolo", "bosque_clareira" },
            new[] { "q08_ecos_do_limiar", "tocar_o_simbolo", "bosque_clareira" },
        };

        // --- consulta (nao muda nada) ---

        /// <summary>Ancora do gatilho deste objetivo (tabela Gatilhos), ou "" se o objetivo nao tem gatilho.
        /// E onde o jogador esta ao cumpri-lo: vai para o save junto da transicao (GameSession.Posicao).</summary>
        public static string AncoraDo(string questId, string objetivoId)
        {
            foreach (string[] g in Gatilhos)
                if (g[0] == questId && g[1] == objetivoId) return g[2];
            return "";
        }

        /// <summary>O jogador pode cumprir este objetivo agora? Missao EmAndamento, objetivo ainda pendente e, em
        /// missao ordenada, o proximo da fila. E o que decide se o gatilho aparece.</summary>
        public static bool Ativo(QuestSystem m, string questId, string objetivoId)
        {
            QuestDef d = m.Def(questId);
            if (d == null || d.Objetivo(objetivoId) == null || m.Estado(questId) != QuestStatus.EmAndamento) return false;
            string[] feitos = m.ObjetivosFeitos(questId);
            if (Array.IndexOf(feitos, objetivoId) >= 0) return false;
            return !d.ObjetivosEmOrdem || Proximo(d, feitos) == objetivoId;
        }

        /// <summary>O objetivo que o HUD mostra: o proximo pendente (em missao paralela, o primeiro pendente da lista).
        /// null = nada pendente (a missao espera concluir, ou o desfecho da Q-04).</summary>
        public static ObjetivoDef Atual(QuestSystem m, string questId)
        {
            QuestDef d = m.Def(questId);
            return d == null ? null : d.Objetivo(Proximo(d, m.ObjetivosFeitos(questId)));
        }

        /// <summary>Missoes EmAndamento, centrais antes das opcionais (ordem do catalogo dentro de cada grupo).</summary>
        public static List<QuestDef> EmAndamento(QuestSystem m)
        {
            var lista = new List<QuestDef>();
            for (int passo = 0; passo < 2; passo++)
                foreach (QuestDef d in QuestCatalog.Missoes)
                    if (d.Central == (passo == 0) && m.Estado(d.Id) == QuestStatus.EmAndamento) lista.Add(d);
            return lista;
        }

        // --- transicoes (gravam pela sessao) ---

        /// <summary>O gatilho da ancora foi acionado. Ok = objetivo cumprido e, se era o ultimo, missao concluida e
        /// recompensa no inventario — tudo numa gravacao. Recusado (fora de ordem, missao parada) nao grava.</summary>
        public static QuestResultado Cumprir(GameSession s, string questId, string objetivoId)
        {
            return s.Missao(m =>
            {
                QuestResultado r = m.CumprirObjetivo(questId, objetivoId);
                if (r.Ok) Avancar(m, s);
                return r;
            });
        }

        /// <summary>O que anda sem o jogador pedir: conclui missao EmAndamento sem objetivo pendente (a Q-04 sem
        /// desfecho e recusada com DesfechoPendente e fica como esta), inicia as Automaticas disponiveis, cumpre os
        /// ObjetivosAutomaticos que viraram o proximo pendente e alcanca o inventario. Idempotente. Grava UMA vez se algo mudou; nada mudou = nada gravado.
        /// E chamado ao abrir a cena e periodicamente (MissaoHud): pega tambem o que o dialogo cumpriu.</summary>
        public static QuestResultado Avancar(GameSession s)
        {
            return s.Missao(m =>
            {
                QuestResultado r = default(QuestResultado);
                r.Ok = Avancar(m, s);   // Ok=false aqui nao e recusa: so "nada a gravar" (Erro fica Nenhum)
                r.Status = QuestStatus.Indisponivel;   // nao se aplica: o passo toca varias missoes
                r.Recompensas = QuestResultado.Nada;   // a recompensa ja foi para o inventario
                return r;
            });
        }

        static bool Avancar(QuestSystem m, GameSession s)
        {
            bool mudou = false;
            // Concluir antes de iniciar: concluir a q07 libera a q08 no mesmo passo.
            foreach (QuestDef d in QuestCatalog.Missoes)
                if (m.Estado(d.Id) == QuestStatus.EmAndamento && m.Concluir(d.Id).Ok) mudou = true;
            foreach (string id in Automaticas)
                if (m.Iniciar(id).Ok) mudou = true;
            // Depois de iniciar: a q01 comeca e "acordar" se cumpre no mesmo passo (uma gravacao). Ativo() antes de
            // cumprir porque CumprirObjetivo repetido devolve Ok, e "mudou" a cada leitura gravaria 5x por segundo.
            foreach (string[] o in ObjetivosAutomaticos)
                if (Ativo(m, o[0], o[1]) && m.CumprirObjetivo(o[0], o[1]).Ok) mudou = true;

            if (s.Save.inventario == null) s.Save.inventario = new InventarioData();   // bloco gravado como null
            if (Inventario.Sincronizar(s.Save.inventario, s.Historia) > 0) mudou = true;
            return mudou;
        }

        static string Proximo(QuestDef d, string[] feitos)
        {
            foreach (ObjetivoDef o in d.Objetivos) if (Array.IndexOf(feitos, o.Id) < 0) return o.Id;
            return null;
        }
    }
}
