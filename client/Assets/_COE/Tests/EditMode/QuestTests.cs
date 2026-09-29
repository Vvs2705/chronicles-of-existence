using System.Collections.Generic;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Framework de missoes (T006). PURO: nao monta cena, nao toca disco, nao usa UnityEngine —
    /// por isso este arquivo compila e roda fora do editor.
    ///
    /// No centro estao os dois testes obrigatorios do backlog (BACKLOG_v1_1 "Testes obrigatorios"):
    ///   nº 3 "Conclusao e recompensa de missao sao idempotentes" -> Obrigatorio3_*
    ///   nº 6 "Missao opcional nao bloqueia campanha"             -> Obrigatorio6_*
    ///
    /// COMO O "SAVE/LOAD" E SIMULADO AQUI: os dois estados de partida (save.quests e save.lifeHistory)
    /// sao recriados a partir dos MESMOS dados, que e o que o carregamento faz depois que o JSON virou
    /// objeto. O round-trip do JSON em si e da T004 e mora em QuestSaveTests.cs, que precisa do Unity.</summary>
    public class QuestTests
    {
        SaveData save;
        QuestSystem quests;

        [SetUp]
        public void SetUp()
        {
            save = new SaveData();
            quests = Abrir(save);
        }

        /// <summary>Abre o sistema em cima de um save — e tambem o "carregar" dos testes de reload.</summary>
        static QuestSystem Abrir(SaveData s)
        {
            return new QuestSystem(s.quests, new HistoricoDeVidaLedger(s));
        }

        static void CumprirTudo(QuestSystem q, string questId)
        {
            QuestDef d = QuestCatalog.Missao(questId);
            for (int i = 0; i < d.Objetivos.Length; i++)
            {
                QuestResultado r = q.CumprirObjetivo(questId, d.Objetivos[i].Id);
                Assert.IsTrue(r.Ok, questId + "/" + d.Objetivos[i].Id + ": " + r.Erro);
            }
        }

        /// <summary>Inicia, cumpre tudo, escolhe o desfecho de indice `desfecho` (se a missao tiver) e conclui.
        /// Devolve o resultado do Concluir.</summary>
        static QuestResultado Completar(QuestSystem q, string questId, int desfecho = 0)
        {
            Assert.IsTrue(q.Iniciar(questId).Ok, "montagem do teste falhou ao iniciar " + questId);
            CumprirTudo(q, questId);
            QuestDef d = QuestCatalog.Missao(questId);
            if (d.Desfechos.Length > 0)
                Assert.IsTrue(q.EscolherDesfecho(questId, d.Desfechos[desfecho]).Ok, "montagem: desfecho de " + questId);
            return q.Concluir(questId);
        }

        /// <summary>Tudo que a missao GARANTE gravar ao concluir (nao inclui desfecho: so um dos dois sai).</summary>
        static List<string> EventosGarantidos(QuestDef d)
        {
            List<string> ids = new List<string>();
            if (!string.IsNullOrEmpty(d.EventoDeConclusao)) ids.Add(d.EventoDeConclusao);
            ids.AddRange(d.EventosAoConcluir);
            foreach (RecompensaDef r in d.Recompensas)
            {
                ids.Add(r.Id);
                if (r.Tipo == QuestCatalog.TipoMarco) ids.Add(r.Alvo);
            }
            return ids;
        }

        const string Q01 = "q01_um_novo_amanhecer";
        const string Q02 = "q02_uma_pequena_responsabilidade";
        const string Q03 = "q03_o_cesto_perdido";
        const string Q04 = "q04_uma_promessa";
        const string Q07 = "q07_o_desaparecimento";
        const string Q08 = "q08_ecos_do_limiar";

        const string PromessaCumprida = "evento.q04_promessa_cumprida";
        const string PromessaQuebrada = "evento.q04_promessa_quebrada";

        static readonly string[] Centrais = { Q01, Q02, Q04, Q07, Q08 };

        /// <summary>Chega a Q-04 EmAndamento com os tres objetivos cumpridos e nenhum desfecho.</summary>
        void Q04ProntaParaDecidir()
        {
            Assert.IsTrue(Completar(quests, Q01).Ok);
            Assert.IsTrue(Completar(quests, Q02).Ok);
            Assert.IsTrue(quests.Iniciar(Q04).Ok);
            CumprirTudo(quests, Q04);
        }

        // ---------------------------------------------------------------- catalogo (dados, GDD cap. 07)

        [Test]
        public void Catalogo_TemAsOitoMissoesDeAuren()
        {
            Assert.AreEqual(8, QuestCatalog.Missoes.Length, "GDD cap. 07: Q-01 a Q-08");
            foreach (string id in new[] { Q01, Q02, Q03, Q04, "q05_o_animal_ferido",
                                          "q06_o_segredo_do_ferreiro", Q07, Q08 })
                Assert.IsNotNull(QuestCatalog.Missao(id), "falta a missao " + id);
        }

        [Test]
        public void Catalogo_CincoCentraisETresOpcionais()
        {
            int centrais = 0;
            foreach (QuestDef d in QuestCatalog.Missoes) if (d.Central) centrais++;
            Assert.AreEqual(5, centrais, "dossie §H: cinco centrais e tres opcionais");
            Assert.AreEqual(3, QuestCatalog.Missoes.Length - centrais);
        }

        [Test]
        public void Catalogo_OsQuatroTiposAparecem()
        {
            HashSet<QuestTipo> tipos = new HashSet<QuestTipo>();
            foreach (QuestDef d in QuestCatalog.Missoes) tipos.Add(d.Tipo);
            Assert.AreEqual(4, tipos.Count, "dossie §H: cotidiana, social, exploratoria e narrativa");
        }

        [Test]
        public void Catalogo_IdsQueAMissaoGravaSaoUnicos()
        {
            // Todo id que o QuestSystem grava no historico E chave de idempotencia: repetido entre missoes
            // (ou com o marco de idade da T009), o segundo dono nunca paga e o escopo mente.
            HashSet<string> vistos = new HashSet<string>();
            foreach (MarcoDeIdade m in AgeAdvanceCatalog.Marcos) vistos.Add(m.Id);
            foreach (QuestDef d in QuestCatalog.Missoes)
            {
                List<string> ids = EventosGarantidos(d);
                ids.AddRange(d.Desfechos);
                foreach (string id in ids)
                    Assert.IsTrue(vistos.Add(id), d.Id + ": id gravado no historico repetido: " + id);
            }
        }

        [Test]
        public void Catalogo_TodoPreEventoEGarantidoPorOutraMissao()
        {
            // Pre-evento que nenhuma missao garante = missao que nunca abre (softlock). Desfecho nao conta:
            // so um dos dois e gravado, entao depender dele trancaria o outro ramo.
            HashSet<string> garantidos = new HashSet<string>();
            foreach (QuestDef d in QuestCatalog.Missoes) garantidos.UnionWith(EventosGarantidos(d));

            foreach (QuestDef d in QuestCatalog.Missoes)
                foreach (string pre in d.PreEventos)
                    Assert.IsTrue(garantidos.Contains(pre), d.Id + " exige '" + pre + "', que nenhuma missao garante");
        }

        [Test]
        public void Catalogo_FlagENomeDeEventoQueAMissaoGrava()
        {
            // Flag nao e estado paralelo: levantada <=> o evento dela esta no historico (SaveData.cs, ADR-0005).
            HashSet<string> gravaveis = new HashSet<string>();
            foreach (QuestDef d in QuestCatalog.Missoes) { gravaveis.UnionWith(EventosGarantidos(d)); gravaveis.UnionWith(d.Desfechos); }

            HashSet<string> nomes = new HashSet<string>();
            foreach (string[] par in QuestCatalog.Flags)
            {
                Assert.IsTrue(nomes.Add(par[0]), "flag repetida: " + par[0]);
                Assert.IsTrue(gravaveis.Contains(par[1]), "flag " + par[0] + " aponta para evento que nada grava: " + par[1]);
            }
            Assert.AreEqual(QuestCatalog.Missao(Q08).EventoDeConclusao, QuestCatalog.EventoDaFlag("salto_temporal_liberado"));
            Assert.IsNull(QuestCatalog.EventoDaFlag("flag_que_nao_existe"));
            Assert.IsNull(QuestCatalog.EventoDaFlag(null));
        }

        [Test]
        public void Estados_NumerosCongeladosDoSave()
        {
            // QuestState.status vai para o save como int: reordenar o enum reinterpreta save gravado.
            Assert.AreEqual(0, (int)QuestStatus.Indisponivel);
            Assert.AreEqual(1, (int)QuestStatus.Disponivel);
            Assert.AreEqual(2, (int)QuestStatus.EmAndamento);
            Assert.AreEqual(3, (int)QuestStatus.Concluida);
            Assert.AreEqual(4, (int)QuestStatus.Falhada);
        }

        [Test]
        public void Catalogo_IdsDeObjetivoSaoUnicosDentroDaMissao()
        {
            foreach (QuestDef d in QuestCatalog.Missoes)
            {
                HashSet<string> vistos = new HashSet<string>();
                foreach (ObjetivoDef o in d.Objetivos)
                    Assert.IsTrue(vistos.Add(o.Id), d.Id + ": objetivo repetido " + o.Id);
                Assert.Greater(d.Objetivos.Length, 0, d.Id + " sem objetivo nao da para concluir");
            }
        }

        [Test]
        public void Catalogo_PreRequisitoApontaParaMissaoQueExiste()
        {
            foreach (QuestDef d in QuestCatalog.Missoes)
                foreach (string pre in d.PreMissoes)
                    Assert.IsNotNull(QuestCatalog.Missao(pre), d.Id + " depende de " + pre + ", que nao existe");
        }

        [Test]
        public void Catalogo_NenhumaMissaoFicaInalcancavel()
        {
            // Sem ciclo e sem dependencia impossivel: varrer repetidamente ate nada mais liberar.
            HashSet<string> alcancaveis = new HashSet<string>();
            bool mudou = true;
            while (mudou)
            {
                mudou = false;
                foreach (QuestDef d in QuestCatalog.Missoes)
                {
                    if (alcancaveis.Contains(d.Id)) continue;
                    bool pronto = true;
                    foreach (string pre in d.PreMissoes) if (!alcancaveis.Contains(pre)) pronto = false;
                    if (pronto) { alcancaveis.Add(d.Id); mudou = true; }
                }
            }
            Assert.AreEqual(QuestCatalog.Missoes.Length, alcancaveis.Count,
                "missao presa em ciclo ou em pre-requisito impossivel = softlock");
        }

        // ------------------------------------------------------------------------------- fluxo feliz

        [Test]
        public void FluxoFeliz_DisponivelAndamentoConcluida_EPagaUmaVez()
        {
            Assert.AreEqual(QuestStatus.Disponivel, quests.Estado(Q01), "Q-01 abre o jogo, sem pre-requisito");

            Assert.IsTrue(quests.Iniciar(Q01).Ok);
            Assert.AreEqual(QuestStatus.EmAndamento, quests.Estado(Q01));

            CumprirTudo(quests, Q01);
            QuestResultado fim = quests.Concluir(Q01);

            Assert.IsTrue(fim.Ok, fim.Erro.ToString());
            Assert.AreEqual(QuestStatus.Concluida, fim.Status);
            Assert.AreEqual(QuestCatalog.Missao(Q01).Recompensas.Length, fim.Recompensas.Length,
                "a primeira conclusao paga todas as recompensas da missao");
        }

        [Test]
        public void ConcluirComObjetivoPendente_NaoConcluiENaoPaga()
        {
            quests.Iniciar(Q01);
            QuestResultado r = quests.Concluir(Q01);

            Assert.IsFalse(r.Ok);
            Assert.AreEqual(QuestErro.ObjetivosPendentes, r.Erro);
            Assert.AreEqual(0, r.Recompensas.Length);
            Assert.AreEqual(QuestStatus.EmAndamento, quests.Estado(Q01));
        }

        [Test]
        public void MissaoDesconhecida_NaoLanca()
        {
            Assert.AreEqual(QuestStatus.Indisponivel, quests.Estado("q99_missao_que_nao_existe"));
            Assert.AreEqual(QuestErro.MissaoDesconhecida, quests.Iniciar("q99_missao_que_nao_existe").Erro);
            Assert.AreEqual(QuestErro.MissaoDesconhecida, quests.Concluir(null).Erro);
        }

        // ---------------------------------------------------------------------------- pre-condicoes

        [Test]
        public void PrecondicaoNaoAtendida_NaoInicia()
        {
            Assert.AreEqual(QuestStatus.Indisponivel, quests.Estado(Q02), "Q-02 depende de Q-01");

            QuestResultado r = quests.Iniciar(Q02);

            Assert.IsFalse(r.Ok, "missao com pre-requisito pendente nao pode iniciar");
            Assert.AreEqual(QuestErro.PreRequisitoFaltando, r.Erro, "a falha tem de dizer o motivo");
            Assert.AreEqual(QuestStatus.Indisponivel, quests.Estado(Q02), "e nao pode ter mudado de estado");
        }

        [Test]
        public void PrecondicaoAtendida_LiberaASeguinte()
        {
            Assert.IsTrue(Completar(quests, Q01).Ok);
            Assert.AreEqual(QuestStatus.Disponivel, quests.Estado(Q02));
            Assert.IsTrue(quests.Iniciar(Q02).Ok);
        }

        [Test]
        public void PreEvento_NaoRegistrado_SeguraAMissao()
        {
            // Pre-condicao por evento de vida (T005), usada quando o gatilho nao e outra missao.
            QuestDef exige = new QuestDef("teste_exige_evento", "k", QuestTipo.Narrativa, true,
                null, new[] { "evento.simbolo_visto" }, true,
                new[] { new ObjetivoDef("olhar", "k") }, new RecompensaDef[0], null);
            HistoricoDeVidaLedger ledger = new HistoricoDeVidaLedger(save);
            QuestSystem q = new QuestSystem(save.quests, ledger, new[] { exige });

            Assert.AreEqual(QuestErro.PreRequisitoFaltando, q.Iniciar("teste_exige_evento").Erro);

            ledger.RegistrarSePrimeiro("evento.simbolo_visto", "teste");
            Assert.IsTrue(q.Iniciar("teste_exige_evento").Ok);
        }

        // ------------------------------------------------------------------------------- objetivos

        [Test]
        public void ObjetivoRepetido_NaoAvancaDuasVezes()
        {
            quests.Iniciar(Q01);
            string primeiro = QuestCatalog.Missao(Q01).Objetivos[0].Id;

            Assert.IsTrue(quests.CumprirObjetivo(Q01, primeiro).Ok);
            Assert.IsTrue(quests.CumprirObjetivo(Q01, primeiro).Ok, "repetir e seguro, nao e erro");

            Assert.AreEqual(1, quests.ObjetivosFeitos(Q01).Length,
                "o mesmo objetivo nao pode contar duas vezes");
            Assert.AreEqual(QuestErro.ObjetivosPendentes, quests.Concluir(Q01).Erro,
                "repetir o objetivo 1 nao pode empurrar a missao para o fim");
        }

        [Test]
        public void ObjetivoForaDeOrdem_NaoConta_EmMissaoOrdenada()
        {
            QuestDef d = QuestCatalog.Missao(Q01);
            Assert.IsTrue(d.ObjetivosEmOrdem, "premissa deste teste");
            quests.Iniciar(Q01);

            QuestResultado r = quests.CumprirObjetivo(Q01, d.Objetivos[2].Id);

            Assert.IsFalse(r.Ok);
            Assert.AreEqual(QuestErro.ObjetivoForaDeOrdem, r.Erro);
            Assert.AreEqual(0, quests.ObjetivosFeitos(Q01).Length);
        }

        [Test]
        public void ObjetivosParalelos_AceitamQualquerOrdem()
        {
            QuestDef d = QuestCatalog.Missao(Q03);
            Assert.IsFalse(d.ObjetivosEmOrdem, "Q-03 e a exploratoria de objetivos paralelos");
            Assert.IsTrue(Completar(quests, Q01).Ok);
            quests.Iniciar(Q03);

            Assert.IsTrue(quests.CumprirObjetivo(Q03, d.Objetivos[2].Id).Ok);
            Assert.IsTrue(quests.CumprirObjetivo(Q03, d.Objetivos[0].Id).Ok);
            Assert.IsTrue(quests.CumprirObjetivo(Q03, d.Objetivos[1].Id).Ok);
            Assert.IsTrue(quests.Concluir(Q03).Ok);
        }

        [Test]
        public void ObjetivoDesconhecido_NaoEntraNoSave()
        {
            quests.Iniciar(Q01);
            Assert.AreEqual(QuestErro.ObjetivoDesconhecido, quests.CumprirObjetivo(Q01, "objetivo_inventado").Erro);
            Assert.AreEqual(0, quests.ObjetivosFeitos(Q01).Length);
        }

        [Test]
        public void ObjetivoCumprido_NaoRegride()
        {
            // Nenhuma chamada recusada desfaz objetivo ja cumprido — nem depois de recarregar.
            QuestDef d = QuestCatalog.Missao(Q01);
            quests.Iniciar(Q01);
            quests.CumprirObjetivo(Q01, d.Objetivos[0].Id);

            quests.CumprirObjetivo(Q01, d.Objetivos[2].Id);        // fora de ordem
            quests.CumprirObjetivo(Q01, "objetivo_inventado");     // desconhecido
            quests.Falhar(Q01);                                    // central nao falha
            quests.Concluir(Q01);                                  // pendente

            CollectionAssert.AreEqual(new[] { d.Objetivos[0].Id }, Abrir(save).ObjetivosFeitos(Q01));
        }

        // ---------------------------------- obrigatorio nº 3: conclusao e recompensa sao idempotentes

        [Test]
        public void Obrigatorio3_ConcluirDuasVezes_NaoConcedeNemRegistraDuasVezes()
        {
            QuestResultado primeira = Completar(quests, Q01);
            Assert.Greater(primeira.Recompensas.Length, 0, "montagem: a primeira tem de pagar");
            int fatos = new LifeEventHistory(save).Total;

            QuestResultado segunda = quests.Concluir(Q01);

            Assert.IsFalse(segunda.Ok, "missao concluida nao reabre");
            Assert.AreEqual(QuestErro.NaoEstaEmAndamento, segunda.Erro);
            Assert.AreEqual(0, segunda.Recompensas.Length, "e nao paga de novo");
            Assert.AreEqual(QuestStatus.Concluida, quests.Estado(Q01));
            Assert.AreEqual(fatos, new LifeEventHistory(save).Total, "concluir de novo nao grava fato novo");
        }

        [Test]
        public void Obrigatorio3_SalvarECarregarNoMeio_PreservaProgressoENaoDuplica()
        {
            quests.Iniciar(Q01);
            string primeiro = QuestCatalog.Missao(Q01).Objetivos[0].Id;
            quests.CumprirObjetivo(Q01, primeiro);

            // "salvar e carregar": mesmo SaveData, sistema e historico reabertos do zero.
            QuestSystem depois = Abrir(save);

            Assert.AreEqual(QuestStatus.EmAndamento, depois.Estado(Q01), "o progresso tem de sobreviver");
            Assert.AreEqual(1, depois.ObjetivosFeitos(Q01).Length);

            CumprirTudo(depois, Q01);
            QuestResultado pagou = depois.Concluir(Q01);
            Assert.IsTrue(pagou.Ok);
            Assert.Greater(pagou.Recompensas.Length, 0);

            // recarrega OUTRA vez, ja concluida
            QuestSystem terceira = Abrir(save);
            Assert.AreEqual(QuestStatus.Concluida, terceira.Estado(Q01));
            Assert.AreEqual(0, terceira.Concluir(Q01).Recompensas.Length, "recarregar nao paga de novo");
        }

        [Test]
        public void Obrigatorio3_StatusRevertidoAMao_AindaAssimNaoPagaDeNovo()
        {
            // Segunda linha de defesa: mesmo com o QuestLog devolvido a EmAndamento (save editado, ou
            // gravacao que pegou o historico atualizado e o status atrasado), o id de recompensa ja esta
            // no historico de vida e a concessao nao repete.
            Completar(quests, Q01);
            int fatos = new LifeEventHistory(save).Total;
            foreach (QuestState s in save.quests.missoes)
                if (s.questId == Q01) s.status = (int)QuestStatus.EmAndamento;

            QuestSystem depois = Abrir(save);
            QuestResultado r = depois.Concluir(Q01);

            Assert.IsTrue(r.Ok, "a missao ate conclui de novo (o status foi adulterado)");
            Assert.AreEqual(0, r.Recompensas.Length, "mas nao concede recompensa nenhuma");
            Assert.AreEqual(fatos, new LifeEventHistory(save).Total, "nem grava marco ou evento de novo");
        }

        [Test]
        public void Obrigatorio3_RecompensaEMarcoFicamRegistradosNoHistoricoDeVida()
        {
            Completar(quests, Q01);
            LifeEventHistory historia = new LifeEventHistory(save);
            List<string> garantidos = EventosGarantidos(QuestCatalog.Missao(Q01));

            foreach (string id in garantidos)
                Assert.IsTrue(historia.Ja(id), id + " tem de virar fato do historico");
            Assert.IsTrue(historia.Ja("marco.primeiro_dia"), "slice B06: concluir grava marco.primeiro_dia");
            Assert.IsTrue(historia.Ja("evento.q01_familia_apresentada"), "registra_no_historico da q01");
            Assert.AreEqual(garantidos.Count, historia.PorEscopo(QuestSystem.Escopo(Q01)).Count,
                "T007/T010 encontram tudo pelo escopo da missao, e nada alem disso");
        }

        [Test]
        public void Obrigatorio3_DesfechoDaPromessa_ExatamenteUmEIdempotente()
        {
            Q04ProntaParaDecidir();

            Assert.IsTrue(quests.EscolherDesfecho(Q04, PromessaQuebrada).Ok);
            Assert.IsTrue(quests.EscolherDesfecho(Q04, PromessaQuebrada).Ok, "repetir o mesmo e seguro");
            QuestResultado outro = quests.EscolherDesfecho(Q04, PromessaCumprida);
            Assert.IsFalse(outro.Ok, "slice B08: nunca os dois");
            Assert.AreEqual(QuestErro.DesfechoJaDecidido, outro.Erro);

            // recarregar no meio e mudar de ideia tambem nao grava o segundo
            QuestSystem depois = Abrir(save);
            Assert.AreEqual(QuestErro.DesfechoJaDecidido, depois.EscolherDesfecho(Q04, PromessaCumprida).Erro);
            Assert.IsTrue(depois.Concluir(Q04).Ok);
            Assert.AreEqual(QuestErro.NaoEstaEmAndamento, depois.EscolherDesfecho(Q04, PromessaCumprida).Erro,
                "concluida, a escolha esta selada");

            LifeEventHistory historia = new LifeEventHistory(save);
            Assert.IsTrue(historia.Ja(PromessaQuebrada));
            Assert.IsFalse(historia.Ja(PromessaCumprida));
            int daPromessa = 0;
            foreach (LifeEvent e in historia.PorEscopo(QuestSystem.Escopo(Q04)))
                if (e.eventId == PromessaQuebrada || e.eventId == PromessaCumprida) daPromessa++;
            Assert.AreEqual(1, daPromessa, "slice R4: o historico tem UM registro da promessa");
        }

        [Test]
        public void Obrigatorio3_ConcluirSemDesfecho_NaoConcluiNemPaga()
        {
            Q04ProntaParaDecidir();

            QuestResultado r = quests.Concluir(Q04);

            Assert.IsFalse(r.Ok, "Q-07 le o par cumprida/quebrada: concluir sem nenhum deixaria o par vazio");
            Assert.AreEqual(QuestErro.DesfechoPendente, r.Erro);
            Assert.AreEqual(0, r.Recompensas.Length);
            Assert.AreEqual(QuestStatus.EmAndamento, quests.Estado(Q04));
            Assert.IsFalse(new LifeEventHistory(save).Ja("marco.promessa_feita"));
        }

        [Test]
        public void DesfechoInvalido_NaoGravaNada()
        {
            Q04ProntaParaDecidir();
            int fatos = new LifeEventHistory(save).Total;

            Assert.AreEqual(QuestErro.DesfechoInvalido, quests.EscolherDesfecho(Q04, "evento.q01_concluida").Erro);
            Assert.AreEqual(QuestErro.DesfechoInvalido, quests.EscolherDesfecho(Q04, null).Erro);
            Assert.AreEqual(QuestErro.NaoEstaEmAndamento, quests.EscolherDesfecho(Q01, PromessaCumprida).Erro);
            Assert.AreEqual(QuestErro.MissaoDesconhecida, quests.EscolherDesfecho("q99_nada", PromessaCumprida).Erro);
            Assert.AreEqual(fatos, new LifeEventHistory(save).Total);
        }

        // ------------------------------------- obrigatorio nº 6: opcional nao bloqueia campanha

        [Test]
        public void Obrigatorio6_NenhumaCentralDependeDeOpcional()
        {
            // Por missao E por evento: um PreEvento gravado so por opcional prenderia a central do mesmo jeito.
            HashSet<string> daOpcional = new HashSet<string>();
            foreach (QuestDef d in QuestCatalog.Missoes)
                if (!d.Central) { daOpcional.UnionWith(EventosGarantidos(d)); daOpcional.UnionWith(d.Desfechos); }

            foreach (QuestDef d in QuestCatalog.Missoes)
            {
                if (!d.Central) continue;
                foreach (string pre in d.PreMissoes)
                    Assert.IsTrue(QuestCatalog.Missao(pre).Central,
                        d.Id + " (central) depende da opcional " + pre + " = campanha refem de conteudo opcional");
                foreach (string pre in d.PreEventos)
                    Assert.IsFalse(daOpcional.Contains(pre),
                        d.Id + " (central) exige o evento " + pre + ", que so uma opcional grava");
            }
        }

        [Test]
        public void Obrigatorio6_CampanhaFechaIgnorandoAsTresOpcionais_EmQualquerDesfecho()
        {
            for (int desfecho = 0; desfecho < QuestCatalog.Missao(Q04).Desfechos.Length; desfecho++)
            {
                SaveData s = new SaveData();
                QuestSystem q = Abrir(s);
                foreach (string id in Centrais)
                {
                    Assert.AreEqual(QuestStatus.Disponivel, q.Estado(id),
                        id + " nao liberou sem nenhuma missao opcional (desfecho " + desfecho + ")");
                    Assert.IsTrue(Completar(q, id, desfecho).Ok, id + " nao concluiu");
                }
                Assert.AreEqual(QuestStatus.Concluida, q.Estado(Q08), "a campanha tem de chegar ao fim");

                foreach (QuestDef d in QuestCatalog.Missoes)
                    if (!d.Central)
                        Assert.AreNotEqual(QuestStatus.Concluida, q.Estado(d.Id), "nenhuma opcional foi tocada");

                // Slice B11 e §4.3: fim da campanha levanta o salto; flag de opcional nao feita NAO aparece.
                LifeEventHistory h = new LifeEventHistory(s);
                Assert.IsTrue(h.Ja(QuestCatalog.EventoDaFlag("salto_temporal_liberado")));
                Assert.IsTrue(h.Ja("marco.eco_do_limiar"));
                foreach (string flag in new[] { "ajudou_oren", "cuidou_do_animal", "confianca_de_borin" })
                    Assert.IsFalse(h.Ja(QuestCatalog.EventoDaFlag(flag)), "NPC lembraria missao nunca feita: " + flag);
            }
        }

        [Test]
        public void Obrigatorio6_OpcionalAbandonadaEmAndamento_NaoTrancaACampanha()
        {
            Completar(quests, Q01);
            Assert.IsTrue(quests.Iniciar(Q03).Ok);   // comeca e larga no meio

            foreach (string id in Centrais)
                if (quests.Estado(id) != QuestStatus.Concluida)
                    Assert.IsTrue(Completar(quests, id).Ok, id + " travou por causa de uma opcional em andamento");
            Assert.AreEqual(QuestStatus.Concluida, quests.Estado(Q08));
            Assert.AreEqual(QuestStatus.EmAndamento, quests.Estado(Q03), "a opcional fica como estava");
        }

        [Test]
        public void Obrigatorio6_OpcionalFalhada_NaoTrancaACampanha()
        {
            Completar(quests, Q01);
            quests.Iniciar(Q03);
            Assert.IsTrue(quests.Falhar(Q03).Ok);
            Assert.AreEqual(QuestStatus.Falhada, quests.Estado(Q03));

            foreach (string id in Centrais)
                if (quests.Estado(id) != QuestStatus.Concluida)
                    Assert.IsTrue(Completar(quests, id).Ok, id + " travou por causa de uma opcional falhada");
            Assert.AreEqual(QuestStatus.Concluida, quests.Estado(Q08));
        }

        [Test]
        public void MissaoCentral_NaoPodeFalhar()
        {
            quests.Iniciar(Q01);
            QuestResultado r = quests.Falhar(Q01);

            Assert.IsFalse(r.Ok, "central em Falhada travaria a campanha para sempre");
            Assert.AreEqual(QuestErro.CentralNaoFalha, r.Erro);
            Assert.AreEqual(QuestStatus.EmAndamento, quests.Estado(Q01));
        }

        [Test]
        public void Falhada_ETerminal_NaoReabreENaoPaga()
        {
            Completar(quests, Q01);
            quests.Iniciar(Q03);
            quests.Falhar(Q03);

            Assert.AreEqual(QuestErro.NaoEstaDisponivel, quests.Iniciar(Q03).Erro);
            Assert.AreEqual(0, quests.Concluir(Q03).Recompensas.Length);
            Assert.AreEqual(QuestStatus.Falhada, quests.Estado(Q03));
        }

        // ------------------------------------------------------------------------- persistencia

        [Test]
        public void SaveAntigoSemOBloco_CarregaLimpo()
        {
            // Save gravado antes da T006: a chave "quests" nao existe, entao o bloco chega null.
            SaveData antigo = new SaveData();
            antigo.quests = null;

            QuestSystem q = new QuestSystem(null, new HistoricoDeVidaLedger(antigo));

            Assert.AreEqual(QuestStatus.Disponivel, q.Estado(Q01), "campanha comeca do zero, sem excecao");
            Assert.AreEqual(QuestStatus.Indisponivel, q.Estado(Q02));
            Assert.IsTrue(q.Iniciar(Q01).Ok);
        }

        [Test]
        public void SaveSoGuardaMissaoTocada()
        {
            Assert.AreEqual(0, save.quests.missoes.Count, "save novo nao lista as oito missoes");
            quests.Iniciar(Q01);
            Assert.AreEqual(1, save.quests.missoes.Count, "so a missao tocada ocupa linha");
            Assert.AreEqual(Q01, save.quests.missoes[0].questId);
        }

        [Test]
        public void ProgressoSobreviveAoReload_ComObjetivosEStatus()
        {
            Completar(quests, Q01);
            quests.Iniciar(Q02);
            quests.CumprirObjetivo(Q02, QuestCatalog.Missao(Q02).Objetivos[0].Id);

            QuestSystem depois = Abrir(save);

            Assert.AreEqual(QuestStatus.Concluida, depois.Estado(Q01));
            Assert.AreEqual(QuestStatus.EmAndamento, depois.Estado(Q02));
            Assert.AreEqual(1, depois.ObjetivosFeitos(Q02).Length);
            Assert.AreEqual(QuestStatus.Indisponivel, depois.Estado(Q04), "Q-04 ainda espera Q-02");
        }

        // --------------------------------------------------- fronteira com T007 (dialogo -> missao)

        [Test]
        public void TentarAvancar_PercorreAMissaoInteira()
        {
            Assert.IsTrue(quests.TentarAvancar(Q01, new QuestIntent(QuestAcao.Iniciar)).Ok);
            foreach (ObjetivoDef o in QuestCatalog.Missao(Q01).Objetivos)
                Assert.IsTrue(quests.TentarAvancar(Q01, new QuestIntent(QuestAcao.CumprirObjetivo, o.Id)).Ok);

            QuestResultado fim = quests.TentarAvancar(Q01, new QuestIntent(QuestAcao.Concluir));
            Assert.IsTrue(fim.Ok);
            Assert.Greater(fim.Recompensas.Length, 0);
        }

        [Test]
        public void TentarAvancar_RespeitaAsMesmasRegras()
        {
            // O dialogo nao tem atalho: pedir para concluir sem iniciar falha igual.
            QuestResultado r = quests.TentarAvancar(Q01, new QuestIntent(QuestAcao.Concluir));
            Assert.IsFalse(r.Ok);
            Assert.AreEqual(QuestErro.NaoEstaEmAndamento, r.Erro);
            Assert.AreEqual(0, r.Recompensas.Length);
        }

        [Test]
        public void IntencaoDeTextoForaDaAllowlist_NaoViraAcao()
        {
            QuestIntent i;
            Assert.IsFalse(QuestIntent.TryParse("conceder_recompensa", null, out i),
                "nao existe acao de conceder: recompensa e consequencia de concluir");
            Assert.IsFalse(QuestIntent.TryParse("Concluir", null, out i), "allowlist e exata");
            Assert.IsFalse(QuestIntent.TryParse(null, null, out i));

            Assert.IsTrue(QuestIntent.TryParse("iniciar", null, out i));
            Assert.AreEqual(QuestAcao.Iniciar, i.Acao);
            Assert.IsTrue(QuestIntent.TryParse("objetivo", "acordar", out i));
            Assert.AreEqual("acordar", i.ObjetivoId);
        }

        [Test]
        public void TentarAvancar_DesfechoPelaAllowlist_PassaPelaMesmaValidacao()
        {
            // A escolha de B08 vem de uma fala (T007): entra pela allowlist e e validada como qualquer pedido.
            Q04ProntaParaDecidir();
            QuestIntent i;
            Assert.IsTrue(QuestIntent.TryParse("desfecho", PromessaCumprida, out i));
            Assert.AreEqual(QuestAcao.EscolherDesfecho, i.Acao);

            Assert.IsTrue(quests.TentarAvancar(Q04, i).Ok);
            Assert.AreEqual(QuestErro.DesfechoJaDecidido,
                quests.TentarAvancar(Q04, new QuestIntent(QuestAcao.EscolherDesfecho, PromessaQuebrada)).Erro);
            Assert.AreEqual(0, quests.TentarAvancar(Q04, i).Recompensas.Length, "escolher nao concede nada");
            Assert.IsTrue(new LifeEventHistory(save).Ja(PromessaCumprida));
        }
    }
}
