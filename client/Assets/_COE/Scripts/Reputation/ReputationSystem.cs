using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>O bloco de reputacao dentro do SaveData. Bloco NOVO pela regra de crescimento da T004:
    /// nasce com padrao neutro (lista vazia) e NAO sobe saveVersion — save antigo sem esta chave carrega
    /// com reputacao neutra em tudo, e save novo lido por codigo velho so tem uma chave a mais.
    ///
    /// Lista e nao dicionario porque JsonUtility nao serializa Dictionary; o indice em memoria e
    /// reconstruido no construtor de ReputationSystem, do mesmo jeito que LifeEventHistory faz.</summary>
    [Serializable]
    public class ReputationData
    {
        public List<ReputationEntry> leituras = new List<ReputationEntry>();
    }

    /// <summary>Uma leitura: o que UM alvo sente do avatar em UMA dimensao. A chave e o par (alvo, dimensao) —
    /// e por isso que "heroi em Auren, temido no bosque" e dado, e nao um `if`: sao duas linhas desta lista.</summary>
    [Serializable]
    public class ReputationEntry
    {
        public string alvo = "";       // id estavel snake_case: "npc_borin", "com_auren", "com_bosque_dos_sussurros"
        public string dimensao = "";   // um dos ids de ReputationDimensao
        public int valor;              // -100..100, 0 = neutro/desconhecido
        public int valorMarco;         // valor no ultimo marco fixado; alimenta MudancasDesdeMarco (T012)
    }

    /// <summary>As dimensoes implementadas no prototipo. Ids estaveis gravados no save: nao renomear sem migracao.
    ///
    /// POR QUE SO DUAS (dossie §G: "no prototipo testar consequencias de confianca e relacionamentos sem construir
    /// cinco subsistemas completos"): Confianca cobre a relacao com o NPC individual e Renome cobre como a
    /// comunidade te ve — juntas ja produzem o caso que o dossie usa para definir o pilar (heroi numa aldeia,
    /// temido em outra) sem que exista uma barra de bem/mal.
    ///
    /// Honra, Compaixao e Temor encaixam aqui como mais uma const + uma linha na tabela de faixa de
    /// ReputationFaixa. Nenhum outro arquivo muda: quem aplica e quem consulta recebem a dimensao como string.
    /// Nao implementa-las agora e decisao de escopo, nao esquecimento.</summary>
    public static class ReputationDimensao
    {
        public const string Confianca = "confianca";  // relacao pessoal com UM NPC
        public const string Renome = "renome";        // como uma comunidade/faccao te ve

        public static bool Conhecida(string dimensao)
        {
            return dimensao == Confianca || dimensao == Renome;
        }
    }

    /// <summary>Faixa qualitativa — o que a UI, o dialogo (T007) e a missao (T012) leem. Ninguem deve ramificar
    /// por numero cru: o numero e hipotese v0 e vai mudar no balanceamento; a faixa e o contrato.
    ///
    /// HIPOTESE v0 (nao medida, sem playtest): -100..-60 hostil · -59..-20 frio · -19..19 neutro ·
    /// 20..59 cordial · 60..100 leal. Faixa neutra larga de proposito: um gesto isolado nao deve mudar
    /// como alguem te trata, senao a reputacao vira um contador de cliques.</summary>
    public static class ReputationFaixa
    {
        public const string Hostil = "hostil";
        public const string Frio = "frio";
        public const string Neutro = "neutro";
        public const string Cordial = "cordial";
        public const string Leal = "leal";

        public const int MinimoHostil = -60;   // <= isto e hostil
        public const int MinimoFrio = -20;     // <= isto e frio
        public const int MinimoCordial = 20;   // >= isto e cordial
        public const int MinimoLeal = 60;      // >= isto e leal

        public static string De(int valor)
        {
            if (valor <= MinimoHostil) return Hostil;
            if (valor <= MinimoFrio) return Frio;
            if (valor >= MinimoLeal) return Leal;
            if (valor >= MinimoCordial) return Cordial;
            return Neutro;
        }
    }

    /// <summary>Como um NPC ou uma comunidade vira ALVO de reputacao. Um espaco de ids so, com prefixo, porque
    /// reputacao de NPC e de comunidade moram na mesma lista: sem prefixo, uma comunidade chamada "borin" e o
    /// ferreiro Borin seriam a mesma leitura.
    ///
    /// COSTURA COM A T007: NpcCatalog usa id NU ("borin", "lysa"); o alvo de reputacao e "npc_borin". A conversao
    /// acontece aqui e em ReputationSystem.ConfiancaNo — nenhum chamador deve concatenar prefixo na mao.</summary>
    public static class ReputationAlvo
    {
        public const string PrefixoNpc = "npc_";
        public const string PrefixoComunidade = "com_";

        public static string Npc(string npcId) { return string.IsNullOrEmpty(npcId) ? "" : PrefixoNpc + npcId; }
        public static string Comunidade(string comunidadeId) { return string.IsNullOrEmpty(comunidadeId) ? "" : PrefixoComunidade + comunidadeId; }
    }

    /// <summary>Um efeito de um ato sobre UM par (alvo, dimensao).</summary>
    public class ReputationEfeito
    {
        public readonly string Alvo;
        public readonly string Dimensao;
        public readonly int Delta;

        public ReputationEfeito(string alvo, string dimensao, int delta)
        {
            Alvo = alvo;
            Dimensao = dimensao;
            Delta = delta;
        }
    }

    /// <summary>O ato e o DADO que resolve a exigencia central da T010: o MESMO ato sobe numa comunidade e
    /// desce em outra porque carrega varios efeitos, nao porque alguem escreveu um `if` por comunidade.
    ///
    ///     new ReputationAto("denunciou_o_cacador",
    ///         new ReputationEfeito("com_auren", ReputationDimensao.Renome, +12),
    ///         new ReputationEfeito("npc_tovin", ReputationDimensao.Confianca, -20));
    ///
    /// Este tipo tambem E a INTENCAO do prompt-mestre §9: uma fala de dialogo (T007) nao muda estado, ela
    /// devolve um ReputationAto + o id do evento canonico que a originou. Quem valida e aplica e
    /// ReputationSystem.Aplicar, que pode recusar (ver ReputationMotivo). Uma fala gerada por IA nunca
    /// escreve em reputacao: ela no maximo pede um ato ja catalogado pelo conteudo do jogo.
    ///
    /// ponytail: nao existe catalogo/ScriptableObject de atos aqui. Quem tem conteudo (T012) monta o ato ou o
    /// guarda como quiser; um catalogo com um unico consumidor seria abstracao vazia. Se tres sistemas passarem
    /// a montar o mesmo ato, o proximo passo e um catalogo em Scripts/Reputation, nao um `if` espalhado.</summary>
    public class ReputationAto
    {
        public readonly string AtoId;
        public readonly IList<ReputationEfeito> Efeitos;

        public ReputationAto(string atoId, params ReputationEfeito[] efeitos)
        {
            AtoId = atoId ?? "";
            Efeitos = new List<ReputationEfeito>(efeitos ?? new ReputationEfeito[0]);
        }
    }

    /// <summary>O que mudou de fato. `De`/`Para` sao os valores; `FaixaDe`/`FaixaPara` sao o que o jogador percebe.</summary>
    public class ReputationMudanca
    {
        public string Alvo;
        public string Dimensao;
        public int De;
        public int Para;
        public string FaixaDe;
        public string FaixaPara;
        /// <summary>true quando a faixa qualitativa mudou — o unico caso em que vale avisar o jogador.</summary>
        public bool MudouDeFaixa { get { return FaixaDe != FaixaPara; } }
    }

    /// <summary>Por que um pedido nao foi aplicado. String e nao enum porque vai para log e para o reporte
    /// de um adaptador de IA sem tabela de traducao.</summary>
    public static class ReputationMotivo
    {
        public const string Ok = "";
        public const string JaAplicado = "ja_aplicado";                   // idempotencia: aquele evento ja contou
        public const string FonteInvalida = "fonte_invalida";             // id de evento vazio ou fora do snake_case
        public const string AtoVazio = "ato_vazio";                       // ato nulo ou sem efeito
        public const string AlvoInvalido = "alvo_invalido";               // id de alvo vazio ou fora do snake_case
        public const string DimensaoDesconhecida = "dimensao_desconhecida";
        public const string DeltaForaDoLimite = "delta_fora_do_limite";   // 0 ou acima do teto por aplicacao
        public const string Saturado = "saturado";                        // ato trivial batendo no teto anti-farm
        public const string SemLedger = "sem_ledger";                     // sistema montado sem ledger: recusa em vez de aplicar sem idempotencia
    }

    public class ReputationResultado
    {
        public bool Aplicado;
        public string Motivo = ReputationMotivo.Ok;
        public IList<ReputationMudanca> Mudancas = new List<ReputationMudanca>();

        internal static ReputationResultado Recusado(string motivo)
        {
            ReputationResultado r = new ReputationResultado();
            r.Aplicado = false;
            r.Motivo = motivo;
            return r;
        }
    }

    /// <summary>Reputacao contextual do COE (T010). C# puro: nada de UnityEngine, nada de disco.
    ///
    /// O QUE ESTE SISTEMA E (dossie §G, GDD v1.2 §06): uma reputacao POR ALVO — um NPC ou uma comunidade —
    /// e por dimensao. Nao existe medidor universal de bem e mal, e nao existe consulta "quao bom o jogador e":
    /// a pergunta so pode ser feita com um alvo junto. Salvar o animal ferido sobe Confianca em Lysa e pode
    /// nao significar nada para Oren; entregar o cacador sobe Renome em Auren e derruba Confianca em Tovin.
    ///
    /// O QUE ELE NAO FAZ: nao decide o que um ato vale (isso e conteudo, T012), nao fala com a UI, nao grava
    /// em disco (T004 grava o SaveData inteiro) e nao registra o acontecimento no historico de vida — quem
    /// conversa com a T005 e ReputationLedger, o unico arquivo desta pasta que a conhece.
    ///
    /// DECAIMENTO: NAO EXISTE. A reputacao so muda por evento; nao ha esquecimento por tempo. Motivo (dossie §D):
    /// no COE o tempo nao corre sozinho — a idade avanca por marco narrativo confirmado — entao nao existe
    /// relogio contra o qual decair, e um decaimento por marco faria a consequencia da escolha evaporar
    /// exatamente onde o pilar "consequencias persistentes" precisa dela. O unico esquecimento do sistema e o
    /// TETO TRIVIAL (ver AplicarTrivial), que e anti-farm e nao passagem de tempo. Se o playtest mostrar que a
    /// reputacao so sobe e nunca desce, o proximo passo e reconciliacao POR MARCO (puxar para o neutro no salto
    /// temporal), nao um decaimento continuo — e isso e uma linha em FixarMarco, nao um redesenho.
    ///
    /// TODOS OS NUMEROS DESTE ARQUIVO SAO HIPOTESE v0: faixa -100..100, cortes de faixa, DeltaMaximo,
    /// TetoTrivial e DeltaTrivialMaximo. Nenhum foi medido em playtest.</summary>
    public class ReputationSystem
    {
        /// <summary>Limite duro do valor de uma leitura.</summary>
        public const int Minimo = -100;
        public const int Maximo = 100;

        /// <summary>Maior mudanca que UM evento canonico pode causar em UM par (alvo, dimensao). HIPOTESE v0.
        /// Existe para que uma intencao vinda do dialogo (ou de um adaptador de IA) nao consiga pedir +100 numa
        /// fala: quem quiser um salto maior precisa de mais de um evento canonico, cada um com seu id.</summary>
        public const int DeltaMaximo = 25;

        /// <summary>Anti-farm do dossie §D/§F ("repeticao trivial tem limite/cap por etapa"): ato trivial —
        /// aquele que o jogador pode repetir a vontade, sem evento canonico — so move o valor dentro de
        /// [-TetoTrivial, +TetoTrivial]. Em numero v0: cumprimentar Borin mil vezes chega a CORDIAL e para;
        /// LEAL exige acontecimento, nao contagem de cliques. Ja fora do teto (por eventos canonicos), o ato
        /// trivial so consegue mover o valor DE VOLTA na direcao do neutro, nunca aumentar o extremo.</summary>
        public const int TetoTrivial = 30;

        /// <summary>Maior delta de um unico ato trivial. HIPOTESE v0.</summary>
        public const int DeltaTrivialMaximo = 5;

        readonly ReputationData dados;
        readonly ReputationLedger fatos;
        readonly Dictionary<string, ReputationEntry> indice = new Dictionary<string, ReputationEntry>(StringComparer.Ordinal);

        /// <summary>`fatos` pode ser null: um sistema sem ledger responde consultas e atos triviais, mas recusa
        /// qualquer Aplicar com evento canonico — melhor recusar do que aplicar sem idempotencia.</summary>
        public ReputationSystem(ReputationData dados, ReputationLedger fatos)
        {
            this.dados = dados ?? new ReputationData();
            if (this.dados.leituras == null) this.dados.leituras = new List<ReputationEntry>();
            this.fatos = fatos;

            // Save adulterado a mao pode trazer linha sem alvo, dimensao desconhecida ou par repetido. Limpar uma
            // vez na abertura e o que garante que Valor() e Faixa() digam a verdade depois.
            List<ReputationEntry> limpas = new List<ReputationEntry>(this.dados.leituras.Count);
            for (int i = 0; i < this.dados.leituras.Count; i++)
            {
                ReputationEntry e = this.dados.leituras[i];
                if (e == null || !IdValido(e.alvo) || !ReputationDimensao.Conhecida(e.dimensao)) continue;
                string k = Chave(e.alvo, e.dimensao);
                if (indice.ContainsKey(k)) continue;
                e.valor = Limitar(e.valor);
                e.valorMarco = Limitar(e.valorMarco);
                indice[k] = e;
                limpas.Add(e);
            }
            this.dados.leituras = limpas;
        }

        /// <summary>O bloco persistido, para quem for gravar (T004) ou inspecionar.</summary>
        public ReputationData Dados { get { return dados; } }

        // ---------- CONSULTA (o que T007 e T012 usam) ----------

        /// <summary>Valor atual. Alvo desconhecido devolve 0 (neutro) e NAO lanca: perguntar por um NPC que o
        /// jogador nunca encontrou e o caso normal, nao erro.</summary>
        public int Valor(string alvo, string dimensao)
        {
            ReputationEntry e;
            return indice.TryGetValue(Chave(alvo, dimensao), out e) ? e.valor : 0;
        }

        /// <summary>Faixa qualitativa atual — o que o dialogo e a UI devem ler. Desconhecido = "neutro".</summary>
        public string Faixa(string alvo, string dimensao)
        {
            return ReputationFaixa.De(Valor(alvo, dimensao));
        }

        /// <summary>Confianca daquele NPC pelo id NU do NpcCatalog (T007). E exatamente a assinatura de
        /// DialogueContext.Confianca (Func&lt;string,int&gt;), entao a ligacao do dialogo e uma linha:
        ///
        ///     ctx.Confianca = rep.ConfiancaNo;
        ///
        /// Atencao a faixa: T007 documentou "0..100" no delegate, mas esta escala vai de -100 a 100 — e o valor
        /// negativo e o ponto do sistema (dossie §G: rejeitado numa aldeia). Condicao.Confianca(minimo) continua
        /// funcionando: um minimo positivo simplesmente nao passa para quem esta negativo.</summary>
        public int ConfiancaNo(string npcId)
        {
            return Valor(ReputationAlvo.Npc(npcId), ReputationDimensao.Confianca);
        }

        /// <summary>Renome naquela comunidade pelo id nu ("auren", "bosque_dos_sussurros").</summary>
        public int RenomeEm(string comunidadeId)
        {
            return Valor(ReputationAlvo.Comunidade(comunidadeId), ReputationDimensao.Renome);
        }

        /// <summary>O que mudou desde o ultimo marco fixado. E a materia-prima do resumo do salto temporal
        /// (T012): "em tres anos, Auren passou a te ver como cordial". Ordem = ordem de primeira aparicao.</summary>
        public List<ReputationMudanca> MudancasDesdeMarco()
        {
            List<ReputationMudanca> r = new List<ReputationMudanca>();
            for (int i = 0; i < dados.leituras.Count; i++)
            {
                ReputationEntry e = dados.leituras[i];
                if (e.valor == e.valorMarco) continue;
                r.Add(Mudanca(e.alvo, e.dimensao, e.valorMarco, e.valor));
            }
            return r;
        }

        /// <summary>Fecha o periodo: o valor de agora vira a referencia do proximo MudancasDesdeMarco.
        /// Chamar depois de mostrar o resumo do marco, nao antes.</summary>
        public void FixarMarco()
        {
            for (int i = 0; i < dados.leituras.Count; i++) dados.leituras[i].valorMarco = dados.leituras[i].valor;
        }

        // ---------- APLICACAO ----------

        /// <summary>Mudanca de UM par (alvo, dimensao) causada por um evento canonico.</summary>
        public ReputationResultado Aplicar(string fonteEventoId, string alvo, string dimensao, int delta)
        {
            return Aplicar(fonteEventoId, new ReputationAto(fonteEventoId, new ReputationEfeito(alvo, dimensao, delta)));
        }

        /// <summary>Aplica um ato inteiro sob UM id de evento canonico — tudo ou nada.
        ///
        /// IDEMPOTENCIA (a regra que a T005 impoe e esta tarefa obedece): o ledger marca o evento no historico de
        /// vida na PRIMEIRA vez; recarregar o save e repetir o gatilho devolve Aplicado=false com
        /// Motivo=ja_aplicado e nao mexe em nenhum valor. Validacao vem ANTES do ledger de proposito: um pedido
        /// malformado nao pode queimar o id do evento e impedir a aplicacao correta depois.</summary>
        public ReputationResultado Aplicar(string fonteEventoId, ReputationAto ato)
        {
            if (!IdValido(fonteEventoId)) return ReputationResultado.Recusado(ReputationMotivo.FonteInvalida);
            if (ato == null || ato.Efeitos == null || ato.Efeitos.Count == 0)
                return ReputationResultado.Recusado(ReputationMotivo.AtoVazio);

            for (int i = 0; i < ato.Efeitos.Count; i++)
            {
                ReputationEfeito ef = ato.Efeitos[i];
                if (ef == null || !IdValido(ef.Alvo)) return ReputationResultado.Recusado(ReputationMotivo.AlvoInvalido);
                if (!ReputationDimensao.Conhecida(ef.Dimensao)) return ReputationResultado.Recusado(ReputationMotivo.DimensaoDesconhecida);
                if (ef.Delta == 0 || ef.Delta > DeltaMaximo || ef.Delta < -DeltaMaximo)
                    return ReputationResultado.Recusado(ReputationMotivo.DeltaForaDoLimite);
            }

            if (fatos == null) return ReputationResultado.Recusado(ReputationMotivo.SemLedger);
            if (!fatos.PrimeiraVez(fonteEventoId, ato)) return ReputationResultado.Recusado(ReputationMotivo.JaAplicado);

            ReputationResultado res = new ReputationResultado();
            res.Aplicado = true;
            for (int i = 0; i < ato.Efeitos.Count; i++)
            {
                ReputationEfeito ef = ato.Efeitos[i];
                ReputationEntry e = Leitura(ef.Alvo, ef.Dimensao);
                int de = e.valor;
                e.valor = Limitar(de + ef.Delta);
                if (e.valor != de) res.Mudancas.Add(Mudanca(ef.Alvo, ef.Dimensao, de, e.valor));
            }
            return res;
        }

        /// <summary>Ato TRIVIAL: repetivel, sem evento canonico, e por isso limitado ao TetoTrivial.
        /// Cumprimentar, ajudar com a mesma tarefa cotidiana pela decima vez, entregar mais um feixe de lenha.
        /// Nao passa pelo historico de vida — registrar cada gentileza seria o "log de cada fala" que o
        /// dossie §G e o GDD §10 proibem.</summary>
        public ReputationResultado AplicarTrivial(string alvo, string dimensao, int delta)
        {
            if (!IdValido(alvo)) return ReputationResultado.Recusado(ReputationMotivo.AlvoInvalido);
            if (!ReputationDimensao.Conhecida(dimensao)) return ReputationResultado.Recusado(ReputationMotivo.DimensaoDesconhecida);
            if (delta == 0 || delta > DeltaTrivialMaximo || delta < -DeltaTrivialMaximo)
                return ReputationResultado.Recusado(ReputationMotivo.DeltaForaDoLimite);

            ReputationEntry e = Leitura(alvo, dimensao);
            int de = e.valor;
            int para = Limitar(de + delta);
            // O teto so freia o afastamento do neutro: quem ja e leal por acontecimentos nao fica mais leal por
            // repeticao, mas ainda pode perder pontos triviais.
            if (delta > 0) para = Math.Min(para, Math.Max(de, TetoTrivial));
            else para = Math.Max(para, Math.Min(de, -TetoTrivial));

            if (para == de) return ReputationResultado.Recusado(ReputationMotivo.Saturado);

            e.valor = para;
            ReputationResultado res = new ReputationResultado();
            res.Aplicado = true;
            res.Mudancas.Add(Mudanca(alvo, dimensao, de, para));
            return res;
        }

        // ---------- interno ----------

        ReputationEntry Leitura(string alvo, string dimensao)
        {
            string k = Chave(alvo, dimensao);
            ReputationEntry e;
            if (indice.TryGetValue(k, out e)) return e;
            e = new ReputationEntry();
            e.alvo = alvo;
            e.dimensao = dimensao;
            indice[k] = e;
            dados.leituras.Add(e);
            return e;
        }

        static ReputationMudanca Mudanca(string alvo, string dimensao, int de, int para)
        {
            ReputationMudanca m = new ReputationMudanca();
            m.Alvo = alvo;
            m.Dimensao = dimensao;
            m.De = de;
            m.Para = para;
            m.FaixaDe = ReputationFaixa.De(de);
            m.FaixaPara = ReputationFaixa.De(para);
            return m;
        }

        static string Chave(string alvo, string dimensao) { return alvo + "|" + dimensao; }

        static int Limitar(int v) { return v < Minimo ? Minimo : (v > Maximo ? Maximo : v); }

        /// <summary>Id estavel do CLAUDE.md: minusculo ASCII, digito ou `_`, nao vazio. Vale para alvo e para
        /// id de evento. Rejeitar aqui e o que impede um alvo digitado por um adaptador de IA ("Aldeia Auren")
        /// de virar uma leitura fantasma que nunca mais ninguem consulta.</summary>
        public static bool IdValido(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            for (int i = 0; i < id.Length; i++)
            {
                char c = id[i];
                bool ok = (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_';
                if (!ok) return false;
            }
            return true;
        }
    }
}
