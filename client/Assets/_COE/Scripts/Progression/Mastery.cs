using System.Reflection;

namespace COE
{
    /// <summary>DEFINICAO de uma atividade praticavel. Imutavel, compartilhada, nunca guarda estado de
    /// jogador. Quem escreve o catalogo e o conteudo (T006/T012); a T009 so entrega a regra.</summary>
    public sealed class AtividadeDef
    {
        public readonly string Id;         // snake_case ASCII, estavel: vira chave no save
        public readonly string TrilhaId;   // um dos 12 ids do CONTRATO_T003_T004 secao 1
        public readonly int Desafio;       // 1..5, HIPOTESE v0 (o GDD nao publica escala de dificuldade)

        public AtividadeDef(string id, string trilhaId, int desafio)
        {
            Id = id; TrilhaId = trilhaId; Desafio = desafio;
        }
    }

    /// <summary>Descobrir, aprender e dominar sao FATOS DIFERENTES (dossie secao D). O resultado de cada
    /// pratica diz qual dos tres aconteceu -- e "dominar" nunca sai de repeticao trivial.</summary>
    public enum MarcoDeDominio
    {
        Nenhum = 0,      // a atividade nao rendeu nada desta vez
        Descoberta = 1,  // primeira vez nesta etapa da vida: "isso existe"
        Aprendizado = 2, // a pratica ainda esta rendendo
        Dominio = 3,     // a atividade deu tudo o que tinha para dar NESTA etapa, e nao era trivial
    }

    public struct GanhoResultado
    {
        public bool Aceito;            // false = entrada invalida; nada foi alterado
        public string TrilhaId;
        public MarcoDeDominio Marco;
        public bool Trivial;           // o desafio ficou abaixo do que o personagem ja e
        public bool Saturada;          // a atividade bateu o teto dela nesta etapa
        public int ProgressoGanho;
        public int PontosGanhos;       // quanto o atributo/afinidade subiu AGORA (quase sempre 0)
        public int ValorDepois;
        public int ProgressoDaAtividade;  // acumulado desta atividade NESTA etapa (o que a tela mostra contra o Teto)
        public int Teto;                  // teto desta atividade nesta etapa (menor se trivial)
    }

    /// <summary>Progressao de atributo e afinidade por atividade significativa (T009). C# PURO.
    ///
    /// O PROBLEMA: "repeticao trivial nao gera dominio infinito" (dossie secao D, GDD cap. 07 "Farming de
    /// cesta ou alvo indefeso", teste obrigatorio 7 do backlog). Carregar o mesmo cesto 400 vezes nao pode
    /// virar Forca 400.
    ///
    /// O MECANISMO, E POR QUE ESTES TRES JUNTOS E NAO UM SO:
    ///   (a) RETORNO DECRESCENTE  ganho = GanhoBase / (1 + vezes). Faz a curva parecer aprendizado de
    ///       verdade: a segunda vez ensina metade da primeira. Sozinho nao basta -- com divisao inteira ele
    ///       chega a zero, mas o jogador nao TEM COMO SABER onde, e a sensacao vira "grindar mais um pouco".
    ///   (b) TETO POR (ATIVIDADE, ETAPA)  o dossie pede "limite/cap por etapa". Da um fim anunciavel:
    ///       a interface pode dizer "esta tarefa ja te ensinou tudo por agora". Sozinho seria um penhasco --
    ///       rendimento cheio ate bater e cair a zero de uma vez.
    ///   (c) LIMIAR DE DIFICULDADE  atividade com Desafio ABAIXO do valor ja alcancado na trilha e TRIVIAL e
    ///       usa um teto muito menor. E o que separa "treinar" de "farmar": o GDD cap. 03 exige "tarefas
    ///       progressivamente mais desafiadoras". Sozinho seria burlavel com 50 atividades faceis distintas,
    ///       por isso existe tambem o teto de trilha por etapa.
    ///
    /// O QUE CONTINUA RENDENDO: atividade NOVA (linha de ledger nova, teto proprio) e atividade DESAFIADORA
    /// (Desafio >= valor atual). Crescer tambem devolve espaco: cada etapa da vida reabre todos os tetos.
    /// Ou seja, o anti-farm nao pune quem joga -- ele so recusa pagar pela mesma licao duas vezes.
    ///
    /// NUMEROS v0 SAO HIPOTESE. O GDD v1.2 nao publica nenhum valor de XP, progresso ou custo de atributo
    /// (cap. 03 e cap. 12 falam so em regra); o dossie secao N lista "numeros exatos de XP/atributos/danos"
    /// como pendencia. Estes existem para o comportamento ser testavel, e devem cair em playtest.</summary>
    public static class Mastery
    {
        /// <summary>Progresso que vale um ponto de atributo ou afinidade. HIPOTESE v0.
        /// Dimensionado para que um ponto exija DUAS atividades significativas levadas ao teto (2 x 30),
        /// nunca uma so repetida.</summary>
        public const int ProgressoPorPonto = 60;

        /// <summary>Progresso da PRIMEIRA execucao de uma atividade. HIPOTESE v0.</summary>
        public const int GanhoBase = 12;

        /// <summary>Teto de uma atividade significativa dentro de uma etapa da vida. HIPOTESE v0.
        /// Com o decrescimo acima (12, 6, 4, 3, 2, 2, 1) a setima execucao bate exatamente aqui: a curva e o
        /// teto terminam juntos, entao nenhum dos dois e decorativo.</summary>
        public const int TetoAtividadePorFase = 30;

        /// <summary>Teto de uma atividade TRIVIAL na etapa. HIPOTESE v0. Meia primeira execucao e acabou:
        /// o jogo reconhece "ja sei fazer isso" uma vez e nunca mais paga.</summary>
        public const int TetoTrivialPorFase = 6;

        /// <summary>Teto de UMA TRILHA (um atributo ou uma afinidade) dentro de uma etapa. HIPOTESE v0:
        /// 240 = 4 pontos por etapa. E a garantia dura -- mesmo que o conteudo publique 500 atividades
        /// distintas, uma etapa da vida nao rende mais do que isso numa trilha so.</summary>
        public const int TetoTrilhaPorFase = 240;

        /// <summary>Os 6 atributos do CONTRATO_T003_T004 secao 1. O nome do campo em Attributes E o id.</summary>
        public static readonly string[] Atributos =
        {
            "forca", "agilidade", "vigor", "intelecto", "percepcao", "vontade"
        };

        /// <summary>As 6 afinidades do CONTRATO_T003_T004 secao 1. O nome do campo em Affinities E o id.</summary>
        public static readonly string[] Afinidades =
        {
            "marcial", "arcana", "natural", "artesanal", "social", "exploratoria"
        };

        public static bool TrilhaExiste(string trilhaId)
        {
            return CampoDe(trilhaId) != null;
        }

        /// <summary>Valor atual de um atributo ou afinidade. -1 quando a trilha nao existe.</summary>
        public static int Valor(SaveData save, string trilhaId)
        {
            FieldInfo f = CampoDe(trilhaId);
            if (save == null || f == null) return -1;
            object alvo = EhAtributo(trilhaId) ? (object)save.attributes : save.affinities;
            return alvo == null ? -1 : (int)f.GetValue(alvo);
        }

        /// <summary>Progresso acumulado numa trilha em TODAS as etapas. E o que converte em pontos.</summary>
        public static int ProgressoTotal(SaveData save, string trilhaId)
        {
            if (save == null || save.life == null || save.life.pratica == null) return 0;
            int soma = 0;
            for (int i = 0; i < save.life.pratica.Count; i++)
                if (save.life.pratica[i].trackId == trilhaId) soma += save.life.pratica[i].progresso;
            return soma;
        }

        /// <summary>Executa uma atividade e devolve o que ela rendeu. UNICO caminho que escreve atributo e
        /// afinidade. NAO toca em save.ageYears: praticar nunca envelheceu ninguem (dossie secao D).</summary>
        public static GanhoResultado Praticar(SaveData save, AtividadeDef atividade)
        {
            GanhoResultado r = default(GanhoResultado);
            if (save == null || atividade == null || string.IsNullOrEmpty(atividade.Id)) return r;

            FieldInfo campo = CampoDe(atividade.TrilhaId);
            if (campo == null) return r;                      // trilha fora do contrato: recusa, nao inventa

            if (save.life == null) save.life = new LifeState();

            string faseId = LifePhases.Id(LifePhases.De(save.ageYears));
            int valorAntes = Valor(save, atividade.TrilhaId);

            // (c) trivial = o desafio da tarefa ficou abaixo do que o personagem JA e naquela trilha.
            bool trivial = atividade.Desafio < valorAntes;
            int tetoAtividade = trivial ? TetoTrivialPorFase : TetoAtividadePorFase;

            PracticeEntry e = Linha(save.life, atividade, faseId);

            int bruto = GanhoBase / (1 + e.vezes);                          // (a) retorno decrescente
            int espacoAtividade = tetoAtividade - e.progresso;              // (b) teto por atividade e etapa
            int espacoTrilha = TetoTrilhaPorFase - ProgressoDaFase(save.life, atividade.TrilhaId, faseId);

            int ganho = bruto;
            if (ganho > espacoAtividade) ganho = espacoAtividade;
            if (ganho > espacoTrilha) ganho = espacoTrilha;
            if (ganho < 0) ganho = 0;

            int totalAntes = ProgressoTotal(save, atividade.TrilhaId);

            e.vezes++;
            e.progresso += ganho;

            // Ponto so nasce ao cruzar um multiplo de ProgressoPorPonto: o resto fica no ledger, nao some.
            int pontos = (totalAntes + ganho) / ProgressoPorPonto - totalAntes / ProgressoPorPonto;
            if (pontos > 0) Escrever(save, atividade.TrilhaId, campo, valorAntes + pontos);

            r.Aceito = true;
            r.TrilhaId = atividade.TrilhaId;
            r.Trivial = trivial;
            r.Saturada = e.progresso >= tetoAtividade;
            r.ProgressoGanho = ganho;
            r.PontosGanhos = pontos;
            r.ValorDepois = valorAntes + pontos;
            r.ProgressoDaAtividade = e.progresso;
            r.Teto = tetoAtividade;

            // Descoberta e Dominio sao TRANSICOES, reportadas uma vez so. Se Dominio saisse em toda pratica
            // depois de saturar, um chamador que concedesse titulo ou marco em cima dele duplicaria o premio
            // a cada repeticao -- o exploit de recompensa repetida (GDD cap. 07) entrando pela porta do
            // anti-farm. Depois de saturada, a atividade devolve Nenhum, que e a verdade: nao rendeu nada.
            if (e.vezes == 1) r.Marco = MarcoDeDominio.Descoberta;
            else if (ganho > 0 && r.Saturada && !trivial) r.Marco = MarcoDeDominio.Dominio;
            else if (ganho > 0) r.Marco = MarcoDeDominio.Aprendizado;
            else r.Marco = MarcoDeDominio.Nenhum;

            return r;
        }

        static PracticeEntry Linha(LifeState life, AtividadeDef atividade, string faseId)
        {
            for (int i = 0; i < life.pratica.Count; i++)
            {
                PracticeEntry p = life.pratica[i];
                if (p.activityId == atividade.Id && p.phaseId == faseId) return p;
            }
            PracticeEntry nova = new PracticeEntry();
            nova.activityId = atividade.Id;
            nova.phaseId = faseId;
            nova.trackId = atividade.TrilhaId;
            life.pratica.Add(nova);
            return nova;
        }

        static int ProgressoDaFase(LifeState life, string trilhaId, string faseId)
        {
            int soma = 0;
            for (int i = 0; i < life.pratica.Count; i++)
            {
                PracticeEntry p = life.pratica[i];
                if (p.trackId == trilhaId && p.phaseId == faseId) soma += p.progresso;
            }
            return soma;
        }

        static bool EhAtributo(string trilhaId)
        {
            for (int i = 0; i < Atributos.Length; i++)
                if (Atributos[i] == trilhaId) return true;
            return false;
        }

        /// <summary>Campo de Attributes ou Affinities cujo NOME e o id da trilha. Vale porque SaveData.cs
        /// declara isso no cabecalho dos dois blocos ("o nome do campo E o id estavel no JSON") e
        /// ProgressionMasteryTests.Os12IdsDoContratoResolvem quebra no dia em que deixar de valer.
        /// ponytail: reflexao em vez de um switch de 12 casos -- vale enquanto Praticar for chamado por
        /// acao do jogador, nao por frame. Se virar caminho quente, cachear FieldInfo por id.</summary>
        static FieldInfo CampoDe(string trilhaId)
        {
            if (string.IsNullOrEmpty(trilhaId)) return null;
            if (EhAtributo(trilhaId)) return typeof(Attributes).GetField(trilhaId);
            for (int i = 0; i < Afinidades.Length; i++)
                if (Afinidades[i] == trilhaId) return typeof(Affinities).GetField(trilhaId);
            return null;
        }

        static void Escrever(SaveData save, string trilhaId, FieldInfo campo, int valor)
        {
            object alvo = EhAtributo(trilhaId) ? (object)save.attributes : save.affinities;
            if (alvo != null) campo.SetValue(alvo, valor);
        }
    }
}
