using System.IO;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Save v1 (T004): schema, cena/ancora, cadeia de migracao, versao futura, escrita atomica com backup e
    /// recuperacao, e o teste obrigatorio 4 do backlog (historico + NPCs pelo disco).
    /// Nenhum teste depende de cena nem de Application.persistentDataPath: todos gravam numa pasta temporaria.</summary>
    public class SaveDataTests
    {
        string dir;
        string Path_ { get { return System.IO.Path.Combine(dir, LocalSave.FileName); } }

        [SetUp]
        public void SetUp()
        {
            dir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "coe_save_tests_" + System.Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
        }

        [TearDown]
        public void TearDown()
        {
            try { Directory.Delete(dir, true); } catch (System.Exception) { }
        }

        // Save v0: tem "versao", nao tem "saveVersion".
        const string SaveV0 = "{\"versao\":1,\"volume\":0.4}";

        // LocalSave.Save usa Debug.LogError em falha real; o Test Runner reprova log de erro nao esperado.
        static void EsperaErro(string trecho)
        {
            UnityEngine.TestTools.LogAssert.Expect(UnityEngine.LogType.Error, new System.Text.RegularExpressions.Regex(trecho));
        }

        [Test]
        public void Novo_Padroes()
        {
            SaveData d = new SaveData();
            Assert.AreEqual(1, SaveData.SchemaVersion);
            Assert.AreEqual(SaveData.SchemaVersion, d.saveVersion);
            Assert.AreEqual(5, d.ageYears, "a vida jogavel comeca aos 5");
            Assert.AreEqual(1, d.lifeLevel);
            Assert.AreEqual("", d.characterId, "id so e carimbado na primeira gravacao");
            Assert.IsNotNull(d.birth);
            Assert.AreEqual(0L, d.birth.confirmedAtUtc, "nascimento ainda nao confirmado");
            Assert.AreEqual(1, d.attributes.forca);
            Assert.AreEqual(1, d.attributes.vontade);
            Assert.AreEqual(0, d.affinities.marcial);
            Assert.AreEqual(0, d.affinities.exploratoria);
            Assert.AreEqual("", d.sceneId, "cena padrao");
            Assert.AreEqual("", d.anchorId, "ancora padrao = spawn_player");
        }

        [Test]
        public void RoundTrip_ComAcentoNoNome()
        {
            SaveData d = new SaveData();
            d.birth.destinyId = "ruptura";
            d.birth.originId = "guardioes";
            d.birth.characterName = "Aurélio Çãézinho";
            d.birth.confirmedAtUtc = 638000000000000000L;
            d.identity.appearanceId = "ap_02";
            d.ageYears = 8;
            d.lifeLevel = 3;
            d.attributes.percepcao = 7;
            d.affinities.arcana = 2;
            d.sceneId = "auren";
            d.anchorId = "ferraria";

            LocalSave.Save(d, Path_);
            SaveData back = LocalSave.Load(Path_);

            Assert.AreEqual("ruptura", back.birth.destinyId);
            Assert.AreEqual("guardioes", back.birth.originId);
            Assert.AreEqual("Aurélio Çãézinho", back.birth.characterName, "acento sobrevive ao disco");
            Assert.AreEqual(638000000000000000L, back.birth.confirmedAtUtc);
            Assert.AreEqual("ap_02", back.identity.appearanceId);
            Assert.AreEqual(8, back.ageYears);
            Assert.AreEqual(3, back.lifeLevel);
            Assert.AreEqual(7, back.attributes.percepcao);
            Assert.AreEqual(1, back.attributes.forca, "campo nao mexido mantem o padrao");
            Assert.AreEqual(2, back.affinities.arcana);
            Assert.AreEqual("auren", back.sceneId);
            Assert.AreEqual("ferraria", back.anchorId, "carregar volta para a ancora gravada");
            Assert.AreEqual(SaveData.SchemaVersion, back.saveVersion);
        }

        [Test]
        public void Load_ArquivoAusente_Padrao()
        {
            SaveData d = LocalSave.Load(Path_);
            Assert.AreEqual(SaveData.SchemaVersion, d.saveVersion);
            Assert.AreEqual(5, d.ageYears);
            Assert.IsTrue(string.IsNullOrEmpty(d.birth.destinyId));
            Assert.IsFalse(File.Exists(LocalSave.RejectedPath(Path_)), "nao existir nao e motivo de descarte");
        }

        [Test]
        public void Load_PrincipalCorrompido_RecuperaDoBackup()
        {
            SaveData d = new SaveData();
            d.birth.destinyId = "serena";
            d.birth.characterName = "Íris";
            LocalSave.Save(d, Path_);          // 1a gravacao: so o principal
            d.ageYears = 6;
            LocalSave.Save(d, Path_);          // 2a gravacao: o anterior vira .bak
            Assert.IsTrue(File.Exists(LocalSave.BackupPath(Path_)), "Save gera backup do save anterior");

            File.WriteAllText(Path_, "{ isso nao e json");
            Assert.AreEqual(LocalSave.Ilegivel, LocalSave.VersionOf("{ isso nao e json"), "ilegivel nao se passa por v0");
            SaveData back = LocalSave.Load(Path_);

            Assert.AreEqual("serena", back.birth.destinyId, "carregou do backup, nao do padrao");
            Assert.AreEqual("Íris", back.birth.characterName);
            Assert.IsTrue(File.Exists(LocalSave.RejectedPath(Path_)), "o arquivo corrompido foi guardado, nao apagado");
        }

        [Test]
        public void Load_CorrompidoSemBackup_Padrao()
        {
            File.WriteAllText(Path_, "{ isso nao e json");
            SaveData back = LocalSave.Load(Path_);
            Assert.AreEqual(SaveData.SchemaVersion, back.saveVersion);
            Assert.IsTrue(string.IsNullOrEmpty(back.birth.destinyId));
            Assert.AreEqual(5, back.ageYears);
            Assert.IsTrue(File.Exists(LocalSave.RejectedPath(Path_)));
        }

        // Migracao v0 -> v1 pela cadeia de LocalSave, a partir de um arquivo real de v0 (fixture acima).
        [Test]
        public void Migracao_V0ParaV1_SaveNeutro_ComCopiaDoOriginal()
        {
            File.WriteAllText(Path_, SaveV0);
            Assert.AreEqual(0, LocalSave.VersionOf(SaveV0), "objeto sem saveVersion = v0");

            SaveData back = LocalSave.Load(Path_);

            Assert.AreEqual(SaveData.SchemaVersion, back.saveVersion, "migrado ate a versao atual");
            Assert.IsTrue(string.IsNullOrEmpty(back.birth.destinyId), "nada do v0 vira destino: a tela de nascimento reabre");
            Assert.AreEqual(0L, back.birth.confirmedAtUtc);
            Assert.AreEqual(5, back.ageYears);
            Assert.AreEqual("", back.anchorId);
            Assert.IsFalse(File.Exists(LocalSave.RejectedPath(Path_)), "v0 e migrado, nao descartado");
            Assert.AreEqual(SaveV0, File.ReadAllText(Path_), "Load nao grava: o arquivo so muda na proxima gravacao");
            Assert.AreEqual(SaveV0, File.ReadAllText(LocalSave.PreMigrationPath(Path_, 0)), "original copiado, byte a byte");

            LocalSave.Save(back, Path_);
            Assert.AreEqual(SaveData.SchemaVersion, LocalSave.VersionOf(File.ReadAllText(Path_)), "regravado ja na versao atual");
            Assert.AreEqual(SaveV0, File.ReadAllText(LocalSave.PreMigrationPath(Path_, 0)), "a copia sobrevive a gravacao");
        }

        // Guarda da cadeia: subir SchemaVersion sem acrescentar o passo em LocalSave.Migracoes quebra aqui.
        [Test]
        public void Migracao_TodaVersaoAnteriorTemPasso()
        {
            for (int v = 0; v <= SaveData.SchemaVersion; v++)
            {
                SaveData d = LocalSave.FromJson("{\"saveVersion\":" + v + "}");
                Assert.IsNotNull(d, "saveVersion " + v + " nao chega ate " + SaveData.SchemaVersion);
                Assert.AreEqual(SaveData.SchemaVersion, d.saveVersion);
                Assert.IsNotNull(d.birth);
                Assert.IsNotNull(d.lifeHistory);
                Assert.IsNotNull(d.npcs);
                Assert.AreEqual("", d.sceneId, "chave ausente = cena padrao");
                Assert.AreEqual("", d.anchorId, "chave ausente = spawn_player");
            }
        }

        // Jogo velho lendo save do jogo novo: nada entra pela metade e NADA e gravado por cima.
        [Test]
        public void VersaoFutura_FicaIntacta_ENadaGravaPorCima()
        {
            string futuro = "{\"saveVersion\":999,\"ageYears\":40,\"lifeLevel\":77,\"campoQueAindaNaoExiste\":1}";
            File.WriteAllText(Path_, futuro);
            SaveData velho = new SaveData();
            velho.ageYears = 9;
            string bak = LocalSave.ToJson(velho);
            File.WriteAllText(LocalSave.BackupPath(Path_), bak);
            Assert.AreEqual(999, LocalSave.VersionOf(futuro));
            Assert.IsNull(LocalSave.FromJson(futuro), "esta build nao sabe ler o futuro");

            SaveData back = LocalSave.Load(Path_);

            Assert.AreEqual(SaveData.SchemaVersion, back.saveVersion);
            Assert.AreEqual(5, back.ageYears, "nem o futuro pela metade, nem o .bak velho: save padrao em memoria");
            Assert.AreEqual(1, back.lifeLevel);
            Assert.AreEqual(futuro, File.ReadAllText(Path_), "o save mais novo fica no lugar, byte a byte");
            Assert.IsFalse(File.Exists(LocalSave.RejectedPath(Path_)), "nao e defeito: nao vai para quarentena");

            EsperaErro("versao mais nova");
            back.ageYears = 6;
            LocalSave.Save(back, Path_);

            Assert.AreEqual(futuro, File.ReadAllText(Path_), "gravar nesta build nao sobrescreve o save mais novo");
            Assert.AreEqual(bak, File.ReadAllText(LocalSave.BackupPath(Path_)), "nem o .bak");
            Assert.IsFalse(File.Exists(Path_ + ".tmp"));
        }

        [Test]
        public void BakDeVersaoFutura_ComPrincipalIlegivel_NaoESobrescrito()
        {
            string futuro = "{\"saveVersion\":999,\"ageYears\":40}";
            File.WriteAllText(Path_, "{ isto nao e json");
            File.WriteAllText(LocalSave.BackupPath(Path_), futuro);

            SaveData back = LocalSave.Load(Path_);
            EsperaErro("versao mais nova");
            LocalSave.Save(back, Path_);
            EsperaErro("versao mais nova");
            LocalSave.Save(back, Path_);   // a segunda gravacao e a que faria File.Replace por cima do .bak

            Assert.AreEqual(futuro, File.ReadAllText(LocalSave.BackupPath(Path_)), "o .bak mais novo fica intacto");
        }

        [Test]
        public void Load_BlocoNulo_NaoDerruba()
        {
            File.WriteAllText(Path_, "{\"saveVersion\":1,\"birth\":null,\"attributes\":null}");
            SaveData back = LocalSave.Load(Path_);
            Assert.IsNotNull(back.birth, "quem le o save nunca recebe bloco null");
            Assert.IsNotNull(back.attributes);
            Assert.AreEqual(1, back.attributes.forca);
            Assert.IsNotNull(back.reputation, "bloco acrescentado depois tambem nasce com padrao neutro");
            Assert.IsNotNull(back.quests);
            Assert.IsNotNull(back.npcs);
            // Integracao 2026-09-28: o guard de bloco null de LocalSave.FromJson NAO dispara aqui, e isso esta certo.
            // O JsonUtility do Unity IGNORA "campo": null em objeto aninhado e mantem a instancia padrao — diferente
            // do shim de System.Text.Json usado fora do editor, onde null virava null e o save ia para .rejeitado.
            // A invariante que vale e a de cima (ninguem recebe bloco null); a quarentena continua coberta pelos
            // testes de arquivo corrompido e de versao diferente.
        }

        [Test]
        public void Save_Atomico_NaoDeixaTmp()
        {
            SaveData d = new SaveData();
            LocalSave.Save(d, Path_);
            Assert.IsTrue(File.Exists(Path_));
            Assert.IsFalse(File.Exists(Path_ + ".tmp"), "tmp renomeado por cima do destino");

            LocalSave.Save(d, Path_);
            Assert.IsFalse(File.Exists(Path_ + ".tmp"), "segunda gravacao tambem nao deixa tmp");
            Assert.IsTrue(File.Exists(LocalSave.BackupPath(Path_)));
        }

        // Contrato T003xT004 invariante 6: gravacao que falha no meio nao deixa save meio escrito.
        [Test]
        public void Save_FalhaNoMeio_SaveAnteriorSobrevive()
        {
            SaveData d = new SaveData();
            d.birth.destinyId = "dificil";
            d.ageYears = 6;
            LocalSave.Save(d, Path_);
            string antes = File.ReadAllText(Path_);

            // Falha de escrita de verdade: uma pasta ocupa o nome do .tmp, entao escrever nele lanca.
            Directory.CreateDirectory(Path_ + ".tmp");
            EsperaErro("falha ao gravar");
            d.ageYears = 7;
            LocalSave.Save(d, Path_);

            Assert.AreEqual(antes, File.ReadAllText(Path_), "gravacao que falhou nao toca no save corrente");
            SaveData back = LocalSave.Load(Path_);
            Assert.AreEqual(6, back.ageYears);
            Assert.AreEqual("dificil", back.birth.destinyId);
        }

        // Processo morreu depois de escrever o .tmp e antes da troca: o .tmp e lixo, nunca e lido como save.
        [Test]
        public void Load_TmpSobrandoDeCrash_Ignorado_EProximaGravacaoLimpa()
        {
            SaveData d = new SaveData();
            d.birth.destinyId = "serena";
            d.ageYears = 6;
            LocalSave.Save(d, Path_);
            File.WriteAllText(Path_ + ".tmp", "{\"saveVersion\":1,\"ageYea");

            SaveData back = LocalSave.Load(Path_);
            Assert.AreEqual(6, back.ageYears, "carregou o save inteiro, nao o .tmp");
            Assert.AreEqual("serena", back.birth.destinyId);
            Assert.IsFalse(File.Exists(LocalSave.RejectedPath(Path_)), "o save corrente nao foi afetado");

            back.ageYears = 7;
            LocalSave.Save(back, Path_);
            Assert.IsFalse(File.Exists(Path_ + ".tmp"), "a gravacao seguinte consome o .tmp velho");
            Assert.AreEqual(7, LocalSave.Load(Path_).ageYears);
        }

        // Backlog, teste obrigatorio 4: "Save/load restaura historico e NPCs". Ida e volta pelo DISCO (LocalSave.Save
        // e Load: o mesmo JSON do jogo), com o aceite do B14 — todo fato de NPC aponta para um evento do historico.
        [Test]
        public void Obrigatorio4_SaveLoadRestauraHistoricoENpcs()
        {
            SaveData d = new SaveData();
            d.birth.destinyId = "normal";
            d.birth.originId = "artesaos";
            d.birth.characterName = "Íris";
            d.birth.confirmedAtUtc = 638000000000000000L;

            LifeEventHistory h = new LifeEventHistory(d);
            h.Registrar(new LifeEvent { eventId = "nascimento", categoria = LifeEventCategoria.Nascimento, idade = 5, emUtc = 638000000000000001L });
            h.Registrar(new LifeEvent { eventId = "q04_promessa_nilo", categoria = LifeEventCategoria.Escolha, idade = 6,
                                        emUtc = 638000000000000002L, escopo = "npc_nilo", detalhe = "praça" });
            NpcMemory.Registrar(d.npcs, "nilo", "q04_promessa_nilo", Importancia.Marcante, 638000000000000003L);
            NpcMemory.Registrar(d.npcs, "mara", "nascimento", Importancia.Notavel, 638000000000000004L);
            for (int i = 0; i < NpcMemory.LimiteTrivialPorNpc + 2; i++)
            {
                string id = "recado_borin_" + i;
                h.Registrar(new LifeEvent { eventId = id, categoria = LifeEventCategoria.Relacao, idade = 6, emUtc = 638000000000000010L + i, escopo = "npc_borin" });
                NpcMemory.Registrar(d.npcs, "borin", id, Importancia.Trivial, 638000000000000010L + i);
            }
            h.Registrar(new LifeEvent { eventId = "marco_idade_8", categoria = LifeEventCategoria.Marco, idade = 8, emUtc = 638000000000000020L });
            d.sceneId = "auren";   // anchorId fica "" = B14: reentra em spawn_player depois do salto

            LocalSave.Save(d, Path_);
            SaveData back = LocalSave.Load(Path_);

            // historico: mesma lista, mesma ordem, campo a campo
            Assert.AreEqual(d.lifeHistory.eventos.Count, back.lifeHistory.eventos.Count);
            for (int i = 0; i < d.lifeHistory.eventos.Count; i++)
            {
                LifeEvent a = d.lifeHistory.eventos[i], b = back.lifeHistory.eventos[i];
                Assert.AreEqual(a.eventId, b.eventId, "ordem do historico");
                Assert.AreEqual(a.categoria, b.categoria, a.eventId);
                Assert.AreEqual(a.idade, b.idade, a.eventId);
                Assert.AreEqual(a.emUtc, b.emUtc, a.eventId);
                Assert.AreEqual(a.escopo, b.escopo, a.eventId);
                Assert.AreEqual(a.detalhe, b.detalhe, a.eventId + ": acento no detalhe sobrevive ao disco");
                Assert.AreEqual(a.resumido, b.resumido, a.eventId);
            }

            // NPCs: mesmos fatos e resumos, campo a campo
            Assert.AreEqual(d.npcs.fatos.Count, back.npcs.fatos.Count);
            for (int i = 0; i < d.npcs.fatos.Count; i++)
            {
                NpcMemoryFact a = d.npcs.fatos[i], b = back.npcs.fatos[i];
                Assert.AreEqual(a.npcId, b.npcId);
                Assert.AreEqual(a.eventId, b.eventId);
                Assert.AreEqual(a.importancia, b.importancia, a.npcId + "/" + a.eventId);
                Assert.AreEqual(a.registradoEmUtc, b.registradoEmUtc, a.npcId + "/" + a.eventId);
            }
            Assert.AreEqual(2, NpcMemory.Esquecidos(back.npcs, "borin"), "o resumo do que decaiu tambem volta");
            Assert.AreEqual(d.npcs.resumos.Count, back.npcs.resumos.Count);

            // B14 / dossie §M "NPC recupera evento correto": nenhum NPC lembra de fato que nao esta neste historico
            LifeEventHistory hb = new LifeEventHistory(back);
            for (int i = 0; i < back.npcs.fatos.Count; i++)
                Assert.IsTrue(hb.Ja(back.npcs.fatos[i].eventId), "NPC cita evento fora do historico: " + back.npcs.fatos[i].eventId);
            Assert.AreEqual("q04_promessa_nilo", NpcMemory.Fatos(back.npcs, "nilo")[0].eventId);
            Assert.AreEqual("praça", hb.PorEscopo("npc_nilo")[0].detalhe);
            Assert.AreEqual("auren", back.sceneId);
            Assert.AreEqual("", back.anchorId);

            // e recarregar nao reabre nada (idempotencia depois do load)
            Assert.IsFalse(hb.Registrar("q04_promessa_nilo", LifeEventCategoria.Escolha, 8));
            Assert.IsFalse(hb.Registrar("marco_idade_8", LifeEventCategoria.Marco, 8), "salto nao se repete depois do load");
            Assert.IsFalse(NpcMemory.Registrar(back.npcs, "nilo", "q04_promessa_nilo", Importancia.Marcante, 1L));
            Assert.AreEqual(d.lifeHistory.eventos.Count, back.lifeHistory.eventos.Count);
        }

        [Test]
        public void CharacterId_ECreatedAt_GeradosUmaVezSo()
        {
            SaveData d = new SaveData();
            LocalSave.Save(d, Path_);
            string id = d.characterId;
            string criado = d.createdAtUtc;
            Assert.IsFalse(string.IsNullOrEmpty(id));
            Assert.IsFalse(string.IsNullOrEmpty(criado));

            d.ageYears = 9;
            LocalSave.Save(d, Path_);
            Assert.AreEqual(id, d.characterId, "regravar nao troca a identidade do personagem");
            Assert.AreEqual(criado, d.createdAtUtc);

            SaveData back = LocalSave.Load(Path_);
            Assert.AreEqual(id, back.characterId, "recarregar tambem nao troca");
            Assert.AreEqual(criado, back.createdAtUtc);
            Assert.IsFalse(string.IsNullOrEmpty(back.updatedAtUtc));
        }

        // NEGATIVO: a permanencia do nascimento e regra da T003, mas a persistencia nao pode furar por baixo.
        // Carregar nao normaliza, nao valida e nao "conserta" BirthChoice — devolve exatamente o que esta no arquivo.
        [Test]
        public void Load_NaoConsertaBirth()
        {
            SaveData d = new SaveData();
            d.birth.destinyId = "destino_que_a_t003_nao_conhece";
            d.birth.originId = "origem_estranha";
            d.birth.characterName = "Nôa";
            d.birth.confirmedAtUtc = 42L;
            LocalSave.Save(d, Path_);

            SaveData back = LocalSave.Load(Path_);

            Assert.AreEqual(42L, back.birth.confirmedAtUtc, "confirmedAtUtc nunca volta a 0 no carregamento");
            Assert.AreEqual("destino_que_a_t003_nao_conhece", back.birth.destinyId, "T004 nao troca destino por 'padrao'");
            Assert.AreEqual("origem_estranha", back.birth.originId);
            Assert.AreEqual("Nôa", back.birth.characterName);

            // e nem depois de regravar o que foi carregado
            LocalSave.Save(back, Path_);
            SaveData again = LocalSave.Load(Path_);
            Assert.AreEqual(42L, again.birth.confirmedAtUtc);
            Assert.AreEqual("destino_que_a_t003_nao_conhece", again.birth.destinyId);
        }
    }
}
