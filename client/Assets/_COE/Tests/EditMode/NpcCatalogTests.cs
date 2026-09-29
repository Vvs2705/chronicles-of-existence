using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace COE.Tests
{
    /// <summary>Elenco e rotina de Auren (T007). Puro: nao monta cena, nao le save, nao olha relogio.</summary>
    public class NpcCatalogTests
    {
        static readonly string[] Elenco =
            { "mara", "daren", "borin", "lysa", "tovin", "eira", "nilo", "sera", "oren", "maelis" };

        [Test]
        public void Catalogo_TemOsDezNpcsDeAuren()
        {
            Assert.AreEqual(10, NpcCatalog.Npcs.Length, "dossie secao G / GDD cap. 06: dez NPCs relevantes");
            foreach (string id in Elenco)
                Assert.IsNotNull(NpcCatalog.Npc(id), "faltou o NPC " + id);
        }

        [Test]
        public void Ids_SaoSnakeCaseAsciiEUnicos()
        {
            HashSet<string> vistos = new HashSet<string>();
            foreach (NpcDef n in NpcCatalog.Npcs)
            {
                Assert.IsTrue(vistos.Add(n.Id), "id repetido: " + n.Id);
                foreach (char c in n.Id)
                    Assert.IsTrue((c >= 'a' && c <= 'z') || c == '_',
                        n.Id + " nao e snake_case ASCII (id vai para o save)");
            }
        }

        [Test]
        public void NpcDesconhecido_DevolveNullSemLancar()
        {
            Assert.IsNull(NpcCatalog.Npc("aethron"));
            Assert.IsNull(NpcCatalog.Npc(null));
            Assert.IsNull(NpcCatalog.Onde("aethron", TimeOfDay.Manha));
        }

        // --- rotina ---

        [Test]
        public void Rotina_CobreOsTresPeriodos_ParaTodoMundo()
        {
            foreach (NpcDef n in NpcCatalog.Npcs)
                foreach (TimeOfDay p in Enum.GetValues(typeof(TimeOfDay)))
                {
                    RotinaEntrada e = NpcCatalog.Onde(n.Id, p);
                    Assert.IsNotNull(e, n.Id + " nao tem rotina no periodo " + p);
                    Assert.IsFalse(string.IsNullOrEmpty(e.AncoraId), n.Id + "/" + p + " sem ancora");
                    Assert.IsFalse(string.IsNullOrEmpty(e.AtividadeKey), n.Id + "/" + p + " sem atividade");
                }
        }

        [Test]
        public void Rotina_DevolveOLugarCertoPorPeriodo()
        {
            // Ids de ancora publicados pela T008 (AurenSceneBuilder.Ancoras).
            // O ferreiro forja de dia e nao dorme na forja.
            Assert.AreEqual("ferraria", NpcCatalog.Onde("borin", TimeOfDay.Manha).AncoraId);
            Assert.AreEqual("ferraria", NpcCatalog.Onde("borin", TimeOfDay.Tarde).AncoraId);
            Assert.AreNotEqual("ferraria", NpcCatalog.Onde("borin", TimeOfDay.Noite).AncoraId);

            // A herbalista so vai ao bosque a tarde.
            Assert.AreEqual("entrada_bosque", NpcCatalog.Onde("lysa", TimeOfDay.Tarde).AncoraId);
            Assert.AreEqual("ervanaria", NpcCatalog.Onde("lysa", TimeOfDay.Manha).AncoraId);
        }

        [Test]
        public void Rotina_EDeterministica()
        {
            RotinaEntrada a = NpcCatalog.Onde("tovin", TimeOfDay.Noite);
            RotinaEntrada b = NpcCatalog.Onde("tovin", TimeOfDay.Noite);
            Assert.AreEqual(a.AncoraId, b.AncoraId);
            Assert.AreEqual(a.AtividadeKey, b.AtividadeKey);
        }

        /// <summary>Copia dos ids publicados por AurenSceneBuilder.Ancoras (T008, assembly COE.Editor —
        /// COE.Tests nao enxerga o editor, entao a lista e duplicada aqui de proposito). Se a T008 mudar
        /// um id, este teste cai e a rotina e consertada junto.</summary>
        static readonly string[] AncorasDaT008 =
        {
            "spawn_player", "portao_sul", "casa_familia", "casa_nilo", "casa_sera", "praca_centro",
            "mural_avisos", "ferraria", "ervanaria", "posto_guarda", "entrada_bosque", "bosque_clareira",
        };

        [Test]
        public void Ancoras_SaoIdsDeString_OrdenadosESemRepeticao()
        {
            string[] ancoras = NpcCatalog.AncorasReferenciadas();
            Assert.Greater(ancoras.Length, 0);
            for (int i = 1; i < ancoras.Length; i++)
                Assert.Less(string.CompareOrdinal(ancoras[i - 1], ancoras[i]), 0, "ancora repetida ou fora de ordem");
        }

        [Test]
        public void Ancoras_DaRotina_ExistemNoGrayboxDeAuren()
        {
            foreach (string a in NpcCatalog.AncorasReferenciadas())
                CollectionAssert.Contains(AncorasDaT008, a, "a rotina cita uma ancora que a cena de Auren nao tem");
        }

        // --- conhecimento limitado ---

        [Test]
        public void Conhecimento_ELimitado_NpcNaoEOnisciente()
        {
            Assert.IsTrue(NpcCatalog.Sabe("borin", "topico.metais"), "o ferreiro sabe de metal");
            Assert.IsFalse(NpcCatalog.Sabe("borin", "topico.ervas"), "o ferreiro nao e herbalista");
            Assert.IsTrue(NpcCatalog.Sabe("eira", "topico.historia_de_eldoria"));
            Assert.IsFalse(NpcCatalog.Sabe("nilo", "topico.historia_de_eldoria"), "crianca nao e enciclopedia");
        }

        [Test]
        public void Limiar_NinguemEmAurenSabe()
        {
            foreach (NpcDef n in NpcCatalog.Npcs)
                Assert.IsFalse(NpcCatalog.Sabe(n.Id, "topico.limiar"),
                    n.Id + " nao pode entregar o misterio da campanha na primeira conversa (dossie secao I)");
        }

        [Test]
        public void Vinculos_ApontamParaNpcsQueExistem()
        {
            foreach (NpcDef n in NpcCatalog.Npcs)
                foreach (Vinculo v in n.Vinculos)
                {
                    Assert.IsNotNull(NpcCatalog.Npc(v.OutroNpcId), n.Id + " tem vinculo com " + v.OutroNpcId + ", que nao existe");
                    Assert.AreNotEqual(n.Id, v.OutroNpcId, n.Id + " tem vinculo consigo mesmo");
                }
        }

        [Test]
        public void Definicao_NaoGuardaEstado()
        {
            // ScriptableObject/tabela e definicao, nao estado (CLAUDE.md). Memoria, confianca e progresso
            // moram no save; se alguem colar um campo desses aqui, este teste cai.
            foreach (System.Reflection.FieldInfo f in typeof(NpcDef).GetFields())
            {
                string nome = f.Name.ToLowerInvariant();
                Assert.IsFalse(nome.Contains("memoria") || nome.Contains("memory") || nome.Contains("confianca")
                    || nome.Contains("reputacao") || nome.Contains("estado"),
                    "NpcDef e definicao compartilhada: " + f.Name + " e estado e pertence ao save");
                Assert.IsTrue(f.IsInitOnly, "NpcDef." + f.Name + " tem de ser readonly");
            }
        }
    }
}
