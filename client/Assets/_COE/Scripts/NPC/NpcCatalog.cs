using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>Onde o NPC esta e o que faz num periodo. O periodo e o TimeOfDay da T009
    /// (TimeOfDayCycle): a rotina LE o relogio do cotidiano, nao inventa um proprio.
    /// AncoraId e id de ANCORA DE MUNDO em string (T008), nunca Transform nem nome de GameObject:
    /// a rotina responde sem a cena estar carregada.
    ///
    /// SeLembra != null torna a entrada CONDICIONAL POR MEMORIA: ela so vale se o NPC lembra daquele evento
    /// (NpcMemory), e ai vence a entrada incondicional do mesmo periodo. E assim que evento e salto temporal
    /// mudam comportamento (dossie secao G: "nao congelar amigos em uma rotina eterna").
    /// AncoraId pode ser NpcCatalog.AncoraAusente: o NPC nao esta em Auren (ADR-0007 §3).</summary>
    public sealed class RotinaEntrada
    {
        public readonly TimeOfDay Periodo;
        public readonly string AncoraId;
        public readonly string AtividadeKey;   // chave de Strings; o texto acentuado vive la
        public readonly string SeLembra;       // eventId (NpcMemory.Testemunhos); null = vale sempre

        public RotinaEntrada(TimeOfDay periodo, string ancoraId, string atividadeKey, string seLembra = null)
        {
            Periodo = periodo; AncoraId = ancoraId; AtividadeKey = atividadeKey; SeLembra = seLembra;
        }
    }

    /// <summary>Vinculo com outro NPC. Sem forca/afinidade numerica: relacionamento medido e T010.</summary>
    public sealed class Vinculo
    {
        public readonly string OutroNpcId;
        public readonly string RelacaoKey;

        public Vinculo(string outroNpcId, string relacaoKey) { OutroNpcId = outroNpcId; RelacaoKey = relacaoKey; }
    }

    /// <summary>DEFINICAO de um NPC. Imutavel e compartilhada: NUNCA guarda memoria, confianca nem
    /// progresso (CLAUDE.md: "ScriptableObject e definicao, nao estado" -- vale igual para tabela em
    /// codigo). O estado vive em NpcBook, dentro do save.</summary>
    public sealed class NpcDef
    {
        public readonly string Id;            // snake_case ASCII, congelado: vai para o save por memoria/reputacao
        public readonly string NomeKey;
        public readonly string PapelKey;      // profissao/funcao inicial (GDD cap. 06, tabela NPC-01..NPC-10)
        public readonly string[] TracosKeys;  // personalidade em 1-2 palavras, como o dossie secao G descreve
        public readonly RotinaEntrada[] Rotina;
        public readonly Vinculo[] Vinculos;
        public readonly string[] Sabe;        // topicos que ESTE NPC conhece. Conhecimento e limitado: o que
                                              // nao esta aqui, ele nao sabe -- e o dialogo tem de respeitar isso.

        public NpcDef(string id, string nomeKey, string papelKey, string[] tracosKeys,
            RotinaEntrada[] rotina, Vinculo[] vinculos, string[] sabe)
        {
            Id = id; NomeKey = nomeKey; PapelKey = papelKey; TracosKeys = tracosKeys;
            Rotina = rotina; Vinculos = vinculos; Sabe = sabe;
        }

        /// <summary>A entrada de rotina deste periodo. Entrada condicional cujo evento o NPC lembra vence a
        /// incondicional, esteja onde estiver na lista; ENTRE condicionais lembradas vence a PRIMEIRA declarada
        /// (a ordem e a prioridade). Sem condicional valendo, a primeira incondicional. memoria null = nao lembra
        /// de nada. Deterministico e sem efeito colateral.</summary>
        public RotinaEntrada Onde(TimeOfDay periodo, NpcBook memoria)
        {
            RotinaEntrada padrao = null;
            for (int i = 0; i < Rotina.Length; i++)
            {
                RotinaEntrada e = Rotina[i];
                if (e.Periodo != periodo) continue;
                if (e.SeLembra == null) { if (padrao == null) padrao = e; }
                else if (NpcMemory.Lembra(memoria, Id, e.SeLembra)) return e;
            }
            return padrao;
        }
    }

    /// <summary>Os dez NPCs relevantes de Auren (dossie secao G, GDD cap. 06). C# PURO, sem UnityEngine.
    ///
    /// POR QUE TABELA EM CODIGO e nao ScriptableObject nem JSON: mesmo motivo do DestinyCatalog -- os ids
    /// sao contrato de save, o codigo precisa conhecer o elenco para validar dialogo e rotina, e asset
    /// (SO) torna o catalogo intestavel fora do Unity.
    /// ponytail: exportar para Resources/npcs.json seguindo o par Strings/StringsLoader no dia
    /// em que um redator nao-programador precisar editar falas e rotinas.
    ///
    /// O QUE E HIPOTESE v0 E PRECISA DE DECISAO DE PRODUTO ([a escrever]):
    /// - HORARIOS. Os documentos dizem que NPC relevante TEM rotina; nao publicam nenhuma. A rotina abaixo
    ///   e derivada da profissao de cada um.
    /// - ANCORAS. Os ids sao os PUBLICADOS pela T008 (AurenSceneBuilder.Ancoras, em COE.Editor):
    ///   spawn_player, portao_sul, casa_familia, casa_nilo, casa_sera, praca_centro, mural_avisos,
    ///   ferraria, ervanaria, posto_guarda, entrada_bosque, bosque_clareira, horta_familia. Auren nao tem
    ///   ancora de escola nem de mercado, entao a aula de Eira e a feira acontecem na praca ate a T008
    ///   publicar uma. AncorasReferenciadas() existe para a T008 conferir o que o elenco usa.
    /// - ROTINA CONDICIONAL (RotinaEntrada.SeLembra). Publicada: Nilo desaparecido (ADR-0007 §3) fica na
    ///   ancora-sentinela AncoraAusente nos tres periodos. A rotina pos-salto de Nilo/Sera (B14) e da leva B.
    /// - MARA e DAREN. O dossie diz que a profissao deles VARIA COM A ORIGEM; a rotina deles aqui e a da
    ///   casa, generica. Rotina por origem e trabalho de conteudo, nao de sistema.
    /// - SALTO TEMPORAL. Dossie secao G: "nao congelar amigos em uma rotina eterna". Nilo e Sera terao outra
    ///   rotina depois do salto; este catalogo so descreve a infancia (5-7).</summary>
    public static class NpcCatalog
    {
        /// <summary>Ancora-SENTINELA (ADR-0007 §3): o NPC nao esta em Auren. Nao e ancora de cena — o gerador nao a
        /// cria, AncorasReferenciadas() nao a lista — e o NpcActor posto nela fica sem corpo e sem conversa.
        /// Hoje so Nilo desaparecido aponta para ca. Objetivo de missao nunca pode depender de NPC ausente
        /// (PassagemDoDiaTests varre isso).</summary>
        public const string AncoraAusente = "ausente";

        /// <summary>Os tres periodos na ancora-sentinela enquanto o NPC lembrar de `seLembra`.</summary>
        static RotinaEntrada[] Ausente(string seLembra)
        {
            return new[]
            {
                new RotinaEntrada(TimeOfDay.Manha, AncoraAusente, "atividade.ausente", seLembra),
                new RotinaEntrada(TimeOfDay.Tarde, AncoraAusente, "atividade.ausente", seLembra),
                new RotinaEntrada(TimeOfDay.Noite, AncoraAusente, "atividade.ausente", seLembra),
            };
        }

        static RotinaEntrada[] Junta(params RotinaEntrada[][] partes)
        {
            var r = new List<RotinaEntrada>();
            foreach (RotinaEntrada[] parte in partes) r.AddRange(parte);
            return r.ToArray();
        }

        static RotinaEntrada[] Rot(string ancoraManha, string atvManha, string ancoraTarde, string atvTarde,
            string ancoraNoite, string atvNoite)
        {
            return new[]
            {
                new RotinaEntrada(TimeOfDay.Manha, ancoraManha, atvManha),
                new RotinaEntrada(TimeOfDay.Tarde, ancoraTarde, atvTarde),
                new RotinaEntrada(TimeOfDay.Noite, ancoraNoite, atvNoite),
            };
        }

        public static readonly NpcDef[] Npcs =
        {
            new NpcDef("mara", "npc.mara.nome", "npc.mara.papel",
                new[] { "traco.protetora" },
                Rot("casa_familia", "atividade.cuidar_da_casa",
                    "praca_centro", "atividade.comprar_o_dia",
                    "casa_familia", "atividade.fim_do_dia_em_casa"),
                new[] { new Vinculo("daren", "relacao.companheiro"), new Vinculo("oren", "relacao.freguesa") },
                new[] { "topico.familia", "topico.vila_auren", "topico.oficio_da_familia" }),

            new NpcDef("daren", "npc.daren.nome", "npc.daren.papel",
                new[] { "traco.disciplinado" },
                Rot("casa_familia", "atividade.preparar_o_oficio",
                    "portao_sul", "atividade.levar_o_que_produziu",
                    "casa_familia", "atividade.fim_do_dia_em_casa"),
                new[] { new Vinculo("mara", "relacao.companheiro"), new Vinculo("borin", "relacao.conhecido_de_oficio") },
                new[] { "topico.familia", "topico.oficio_da_familia", "topico.vila_auren" }),

            new NpcDef("borin", "npc.borin.nome", "npc.borin.papel",
                new[] { "traco.exigente", "traco.leal" },
                Rot("ferraria", "atividade.forjar",
                    "ferraria", "atividade.forjar",
                    "praca_centro", "atividade.descansar_na_praca"),
                new[] { new Vinculo("oren", "relacao.fornecedor"), new Vinculo("daren", "relacao.conhecido_de_oficio") },
                new[] { "topico.metais", "topico.ferraria", "topico.vila_auren" }),

            new NpcDef("lysa", "npc.lysa.nome", "npc.lysa.papel",
                new[] { "traco.paciente" },
                Rot("ervanaria", "atividade.preparar_ervas",
                    "entrada_bosque", "atividade.colher_no_bosque",
                    "ervanaria", "atividade.secar_ervas"),
                new[] { new Vinculo("tovin", "relacao.parceira_de_bosque") },
                new[] { "topico.ervas", "topico.bosque", "topico.vila_auren" }),

            new NpcDef("tovin", "npc.tovin.nome", "npc.tovin.papel",
                new[] { "traco.vigilante" },
                Rot("posto_guarda", "atividade.guardar_o_posto",
                    "entrada_bosque", "atividade.cacar",
                    "posto_guarda", "atividade.ronda_da_noite"),
                new[] { new Vinculo("lysa", "relacao.parceiro_de_bosque"), new Vinculo("maelis", "relacao.subordinado") },
                new[] { "topico.caca", "topico.seguranca_da_vila", "topico.bosque" }),

            new NpcDef("eira", "npc.eira.nome", "npc.eira.papel",
                new[] { "traco.curiosa" },
                Rot("praca_centro", "atividade.dar_aula",
                    "praca_centro", "atividade.dar_aula",
                    "mural_avisos", "atividade.ler_os_avisos"),
                new[] { new Vinculo("maelis", "relacao.aliada") },
                // A unica que conhece historia antiga. Nem ela conhece "topico.limiar": o Limiar e o
                // misterio da campanha (dossie secao I), nao um verbete que um NPC solta na primeira conversa.
                new[] { "topico.historia_de_eldoria", "topico.vila_auren", "topico.primeira_fratura" }),

            new NpcDef("nilo", "npc.nilo.nome", "npc.nilo.papel",
                new[] { "traco.impulsivo" },
                // A ORDEM DAS CONDICIONAIS E A PRIORIDADE (NpcDef.Onde: vence a primeira que ele lembra).
                // Leva B: a rotina pos-salto de Nilo (B14, a volta) entra como PRIMEIRO argumento de Junta, antes de
                // Ausente(...), e passa a vencer o sumico sem apagar nada daqui.
                Junta(
                    Ausente(QuestCatalog.EventoNiloDesapareceu),   // ADR-0007 §3: depois da Q-04, em nenhum periodo
                    Rot("praca_centro", "atividade.aula_com_eira",
                        "entrada_bosque", "atividade.brincar_perto_do_bosque",
                        "casa_nilo", "atividade.voltar_para_casa")),
                new[] { new Vinculo("sera", "relacao.amigo_de_infancia") },
                new[] { "topico.vila_auren", "topico.bosque" }),

            new NpcDef("sera", "npc.sera.nome", "npc.sera.papel",
                new[] { "traco.inteligente", "traco.competitiva" },
                Rot("praca_centro", "atividade.aula_com_eira",
                    "praca_centro", "atividade.ajudar_na_feira",
                    "casa_sera", "atividade.estudar_sozinha"),
                new[] { new Vinculo("nilo", "relacao.amiga_de_infancia"), new Vinculo("eira", "relacao.aluna") },
                new[] { "topico.vila_auren", "topico.historia_de_eldoria" }),

            new NpcDef("oren", "npc.oren.nome", "npc.oren.papel",
                new[] { "traco.pragmatico" },
                Rot("portao_sul", "atividade.receber_a_carga",
                    "praca_centro", "atividade.negociar",
                    "praca_centro", "atividade.fechar_as_contas"),
                new[] { new Vinculo("borin", "relacao.cliente"), new Vinculo("maelis", "relacao.contribuinte") },
                new[] { "topico.comercio", "topico.vila_auren", "topico.estradas" }),

            new NpcDef("maelis", "npc.maelis.nome", "npc.maelis.papel",
                new[] { "traco.cautelosa" },
                Rot("mural_avisos", "atividade.ouvir_queixas",
                    "praca_centro", "atividade.percorrer_a_vila",
                    "mural_avisos", "atividade.registrar_decisoes"),
                new[] { new Vinculo("tovin", "relacao.superiora"), new Vinculo("eira", "relacao.aliada") },
                new[] { "topico.administracao", "topico.seguranca_da_vila", "topico.vila_auren" }),
        };

        /// <summary>null se o id nao existe. Id vindo de save ou de dado de dialogo e entrada suspeita,
        /// nao erro de programacao: quem chama decide o que fazer.</summary>
        public static NpcDef Npc(string npcId)
        {
            for (int i = 0; i < Npcs.Length; i++) if (Npcs[i].Id == npcId) return Npcs[i];
            return null;
        }

        /// <summary>Onde o NPC esta e o que faz NESTE periodo, dado o que ele lembra (NpcDef.Onde). null se
        /// o NPC nao existe. Deterministico: a mesma pergunta devolve a mesma resposta, sem relogio e sem cena.
        /// Interrupcao (conversa, evento da vila) nao mora aqui: e NpcAgenda.</summary>
        public static RotinaEntrada Onde(string npcId, TimeOfDay periodo, NpcBook memoria = null)
        {
            NpcDef n = Npc(npcId);
            return n == null ? null : n.Onde(periodo, memoria);
        }

        /// <summary>Conhecimento limitado: false quando o NPC simplesmente nao sabe. NPC nao e onisciente
        /// (dossie secao G) -- o dialogo consulta isto antes de deixar alguem explicar o que nao viu.</summary>
        public static bool Sabe(string npcId, string topicoId)
        {
            NpcDef n = Npc(npcId);
            if (n == null || topicoId == null) return false;
            for (int i = 0; i < n.Sabe.Length; i++) if (n.Sabe[i] == topicoId) return true;
            return false;
        }

        /// <summary>Todas as ancoras de mundo que as rotinas citam, ordenadas. A T008 usa isto para
        /// conferir que o graybox de Auren tem todos os pontos que o elenco precisa. A sentinela AncoraAusente
        /// fica de fora: nao e lugar da cena.</summary>
        public static string[] AncorasReferenciadas()
        {
            List<string> r = new List<string>();
            foreach (NpcDef n in Npcs)
                foreach (RotinaEntrada e in n.Rotina)
                    if (e.AncoraId != AncoraAusente && !r.Contains(e.AncoraId)) r.Add(e.AncoraId);
            r.Sort(StringComparer.Ordinal);
            return r.ToArray();
        }
    }
}
