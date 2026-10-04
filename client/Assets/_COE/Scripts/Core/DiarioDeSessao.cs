using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace COE
{
    /// <summary>Diario de sessao para playtest (docs/PROJETO.md §2.3: quanto tempo cada passo leva, onde o jogador trava).
    /// Um arquivo de texto LOCAL por sessao, sessao_yyyyMMdd_HHmmss.txt (hora local do inicio), so os 10 mais novos.
    /// O testador envia o arquivo se quiser: nada de rede.
    ///
    /// UMA LINHA POR ACONTECIMENTO: "mm:ss&lt;TAB&gt;tipo&lt;TAB&gt;detalhe" (tempo de relogio desde o inicio, nao escalado: o
    /// menu pausa com timeScale 0 e o relogio segue; minutos passam de 59, ex. 75:03). Detalhe vazio = linha termina
    /// no TAB, entao sempre ha 3 colunas. Tipos:
    ///   inicio    versao=&lt;Application.version&gt; cena=&lt;cena&gt; save=novo|continuado idade=&lt;n&gt; periodo=&lt;id&gt;
    ///   objetivo  &lt;questId&gt;/&lt;objetivoId&gt;      missao  &lt;questId&gt; &lt;status&gt; (Status abaixo)
    ///   evento    &lt;id novo no historico de vida&gt;   periodo &lt;manha|tarde|noite&gt;   idade &lt;n&gt;
    ///   pausa / volta (app em segundo plano e de volta)      fim &lt;mm:ss total&gt;
    ///
    /// PRIVACIDADE: so ids do jogo, numeros e tempos. Nunca le birth.characterName, characterId nem nada digitado;
    /// nada de aparelho, conta ou localizacao.
    /// NUNCA LANCA: pasta que nao abre ou disco cheio = o diario se cala; o jogo e o save seguem.
    ///
    /// COMO SE LIGA: SaveState.Commit (toda transicao que deu certo grava por ali) chama Observar, que compara o save
    /// com o ultimo retrato e escreve so o que mudou. Pausa e saida vem do SaveBootstrap. C# puro: relogio e pasta
    /// vem de fora (SaveState no jogo; pasta temporaria e relogio fixo nos testes).</summary>
    public sealed class DiarioDeSessao
    {
        public const int ArquivosGuardados = 10;
        public const string Prefixo = "sessao_";

        /// <summary>Status de missao no diario, pelo numero congelado de QuestStatus.</summary>
        static readonly string[] Status = { "indisponivel", "disponivel", "em_andamento", "concluida", "falhada" };

        /// <summary>O que o diario ja viu do save: so ids e numeros. Idade -1 e periodo "" = nada visto ainda.</summary>
        public sealed class Retrato
        {
            public readonly HashSet<string> Objetivos = new HashSet<string>(StringComparer.Ordinal);   // "questId/objetivoId"
            public readonly Dictionary<string, int> Missoes = new Dictionary<string, int>(StringComparer.Ordinal);
            public readonly HashSet<string> Eventos = new HashSet<string>(StringComparer.Ordinal);
            public string Periodo = "";
            public int Idade = -1;
        }

        readonly Func<DateTime> agora;
        readonly DateTime inicio;
        readonly string arquivo;
        SaveData visto;
        Retrato retrato = new Retrato();
        bool fora;

        /// <summary>Arquivo desta sessao, ou null se a pasta nao abriu.</summary>
        public string Arquivo { get { return arquivo; } }

        /// <summary>Abre a sessao: escreve "inicio" e apaga os arquivos de sessao mais velhos que os 10 mais novos (so
        /// sessao_*.txt de `pasta`). agora: no jogo, DateTime.UtcNow (relogio de parede: conta tambem o aparelho
        /// dormindo em segundo plano); o nome do arquivo usa agora().ToLocalTime(). save: o carregado, base do retrato.</summary>
        public DiarioDeSessao(string pasta, Func<DateTime> agora, SaveData save, string versao, string cena)
        {
            this.agora = agora ?? (() => DateTime.UtcNow);
            inicio = this.agora();
            save = save ?? new SaveData();
            try
            {
                Directory.CreateDirectory(pasta);
                arquivo = Path.Combine(pasta, NomeDoArquivo(inicio.ToLocalTime()));
            }
            catch (Exception) { arquivo = null; }

            visto = save;
            Mudancas(retrato, save);   // base: o que o save ja trazia nao e acontecimento desta sessao
            bool nasceu = save.birth != null && !string.IsNullOrEmpty(save.birth.destinyId);
            Escrever("inicio\tversao=" + versao + " cena=" + cena + " save=" + (nasceu ? "continuado" : "novo")
                + " idade=" + save.ageYears + " periodo=" + retrato.Periodo);
            if (arquivo != null) Rotacionar(pasta, ArquivosGuardados);
        }

        /// <summary>Depois de cada gravacao. Save trocado (Nova vida: SaveState.Current = new SaveData) = retrato vazio:
        /// os ids da vida nova contam de novo e a troca aparece como "periodo" e "idade" da vida nova.</summary>
        public void Observar(SaveData save)
        {
            try
            {
                if (save == null) return;
                if (save != visto) { visto = save; retrato = new Retrato(); }
                foreach (string l in Mudancas(retrato, save)) Escrever(l);
            }
            catch (Exception) { }   // diario nunca derruba a gravacao
        }

        /// <summary>App em segundo plano (true) ou de volta (false). OnApplicationPause e OnApplicationFocus chegam em
        /// par: so a primeira de cada lado vira linha.</summary>
        public void SegundoPlano(bool sim)
        {
            if (sim == fora) return;
            fora = sim;
            Escrever(sim ? "pausa\t" : "volta\t");
        }

        public void Fim() { Escrever("fim\t" + Tempo((agora() - inicio).TotalSeconds)); }

        /// <summary>Compara o save com o retrato e ATUALIZA o retrato. Devolve "tipo\tdetalhe" na ordem do save
        /// (objetivos e status de cada missao, eventos, periodo, idade). Nada mudou = lista vazia. Nunca lanca com bloco
        /// null. ponytail: varre o save inteiro a cada gravacao (dezenas de ids, gravacao rara); o mesmo save so ganha ids,
        /// entao somar ao retrato basta. Indice por bloco se um dia o historico for grande.</summary>
        public static List<string> Mudancas(Retrato retrato, SaveData save)
        {
            var linhas = new List<string>();
            if (retrato == null || save == null) return linhas;

            if (save.quests != null && save.quests.missoes != null)
                foreach (QuestState q in save.quests.missoes)
                {
                    if (q == null || string.IsNullOrEmpty(q.questId)) continue;
                    if (q.objetivosFeitos != null)
                        foreach (string o in q.objetivosFeitos)
                            if (retrato.Objetivos.Add(q.questId + "/" + o)) linhas.Add("objetivo\t" + q.questId + "/" + o);
                    int antes;
                    if (retrato.Missoes.TryGetValue(q.questId, out antes) && antes == q.status) continue;
                    retrato.Missoes[q.questId] = q.status;
                    linhas.Add("missao\t" + q.questId + " " + (q.status >= 0 && q.status < Status.Length ? Status[q.status] : q.status.ToString(CultureInfo.InvariantCulture)));
                }

            if (save.lifeHistory != null && save.lifeHistory.eventos != null)
                foreach (LifeEvent e in save.lifeHistory.eventos)
                    if (e != null && !string.IsNullOrEmpty(e.eventId) && retrato.Eventos.Add(e.eventId)) linhas.Add("evento\t" + e.eventId);

            string periodo = TimeOfDayCycle.Id(TimeOfDayCycle.Atual(save.life));
            if (periodo != retrato.Periodo) { retrato.Periodo = periodo; linhas.Add("periodo\t" + periodo); }
            if (save.ageYears != retrato.Idade) { retrato.Idade = save.ageYears; linhas.Add("idade\t" + save.ageYears.ToString(CultureInfo.InvariantCulture)); }
            return linhas;
        }

        /// <summary>mm:ss; os minutos nao viram hora (75:03). Negativo (relogio mudado para tras) = 00:00.</summary>
        public static string Tempo(double segundos)
        {
            long s = segundos > 0 ? (long)Math.Floor(segundos) : 0;
            return (s / 60).ToString("00", CultureInfo.InvariantCulture) + ":" + (s % 60).ToString("00", CultureInfo.InvariantCulture);
        }

        public static string NomeDoArquivo(DateTime inicioLocal)
        {
            return Prefixo + inicioLocal.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture) + ".txt";
        }

        /// <summary>Apaga os sessao_*.txt de `pasta` (sem subpastas) alem dos `manter` mais novos. Nao toca outro arquivo.
        /// ponytail: "mais novo" pelo nome (hora local); a hora que se repete no fim do horario de verao pode trocar a
        /// ordem de duas sessoes. Data de escrita do arquivo se isso um dia importar.</summary>
        public static void Rotacionar(string pasta, int manter)
        {
            try
            {
                var sessoes = new List<string>();
                foreach (string f in Directory.GetFiles(pasta, Prefixo + "*.txt"))
                    if (f.EndsWith(".txt", StringComparison.Ordinal)) sessoes.Add(f);   // o Windows casa "*.txt" com ".txtx"
                sessoes.Sort(StringComparer.Ordinal);
                for (int i = 0; i < sessoes.Count - manter; i++) File.Delete(sessoes[i]);
            }
            catch (Exception) { }
        }

        void Escrever(string tipoEDetalhe)
        {
            if (arquivo == null) return;
            try { File.AppendAllText(arquivo, Tempo((agora() - inicio).TotalSeconds) + "\t" + tipoEDetalhe + "\n"); }
            catch (Exception) { }   // disco cheio, sem permissao: o diario se cala
        }
    }
}
