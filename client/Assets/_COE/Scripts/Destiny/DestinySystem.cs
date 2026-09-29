using System;
using System.Text;

namespace COE
{
    /// <summary>Por que a escolha falhou. Nenhum = sucesso.</summary>
    public enum BirthError
    {
        Nenhum = 0,
        DestinoDesconhecido,
        OrigemDesconhecida,
        NomeCurto,
        NomeLongo,
        NomeCaractereInvalido,
        JaConfirmado,          // ADR-0004 invariante 1: destino de nascimento e permanente
    }

    /// <summary>Resultado de Confirmar. Ok == false sempre traz um Erro e Escolha == null.</summary>
    public struct BirthResult
    {
        public bool Ok;
        public BirthError Erro;
        public BirthChoice Escolha;
    }

    /// <summary>Regra de nascimento (T003): escolher destino, origem e nome do avatar. C# PURO, sem
    /// UnityEngine, como MotionSolver -- testavel sem cena. Nao le nem grava arquivo: o formato do save
    /// e de T004 (CONTRATO_T003_T004 secao 4).
    ///
    /// CONTRATO. Entrada: o BirthChoice ATUAL (null ou confirmedAtUtc == 0 quando ainda nao ha
    /// escolha), os ids pretendidos e o nome cru digitado. Saida: BirthResult com um BirthChoice NOVO;
    /// o objeto recebido nunca e alterado.
    ///
    /// POR QUE RESULTADO TIPADO E NAO EXCECAO: nome curto e destino ja confirmado sao respostas
    /// esperadas a uma tela de criacao de personagem, que precisa mostrar o motivo; excecao para
    /// entrada de usuario vira try/catch de controle de fluxo. A falha nunca e silenciosa: Ok == false
    /// e Escolha == null.
    ///
    /// O QUE NAO ENTRA AQUI: dificuldade, assistencia de jogabilidade, Grau de Existencia. ADR-0004
    /// separa os tres sistemas; se alguem tentar passar dificuldade para ca, o desenho esta errado --
    /// nao existe parametro, e DestinySystemTests.ApiDeNascimento_NaoAceitaDificuldadeNemAssistencia prova
    /// por reflexao.</summary>
    public static class DestinySystem
    {
        public const int NomeMinimo = 2;   // HIPOTESE v0: o GDD nao publica limite de nome
        public const int NomeMaximo = 24;  // HIPOTESE v0: cabe em balao de dialogo e em lista de save

        static readonly OriginDef[] Nenhuma = new OriginDef[0];

        /// <summary>As tres origens compativeis com o destino. Array vazio (nunca null, nunca excecao)
        /// se o destino nao existe. FONTE UNICA da compatibilidade: Confirmar e Validar passam por aqui.
        /// ponytail: hoje os tres arquetipos servem aos quatro destinos (dossie secao C), entao a resposta e
        /// sempre a mesma lista; restringir uma origem a um destino e mudar so esta funcao.</summary>
        public static OriginDef[] OrigensDisponiveis(string destinyId)
        {
            if (DestinyCatalog.Destino(destinyId) == null) return Nenhuma;
            return (OriginDef[])DestinyCatalog.Origens.Clone(); // copia: ninguem edita o catalogo por fora
        }

        /// <summary>Confirmada = tem destino gravado. So Confirmar escreve destinyId, entao isto nao depende do carimbo:
        /// zerar confirmedAtUtc a mao no save nao reabre o nascimento, e destino vazio com carimbo nao trava a criacao.</summary>
        public static bool EstaConfirmada(BirthChoice escolha)
        {
            return escolha != null && !string.IsNullOrEmpty(escolha.destinyId);
        }

        /// <summary>Confirma o nascimento. Este e o UNICO caminho que escreve destino/origem.
        ///
        /// GUARDA PERMANENTE: se <paramref name="atual"/> ja estiver confirmado, devolve
        /// BirthError.JaConfirmado ANTES de olhar qualquer outro parametro -- nao existe combinacao de
        /// ids, nome ou ordem de chamada que troque destino depois do nascimento (ADR-0004 invariante 1,
        /// GDD cap. 02 "bloquear mudancas retroativas"). Passar exatamente os mesmos ids tambem falha:
        /// nao ha reconfirmacao.</summary>
        public static BirthResult Confirmar(BirthChoice atual, string destinyId, string originId, string nome)
        {
            if (EstaConfirmada(atual)) return Falha(BirthError.JaConfirmado);
            if (DestinyCatalog.Destino(destinyId) == null) return Falha(BirthError.DestinoDesconhecido);
            if (!OrigemDisponivel(destinyId, originId)) return Falha(BirthError.OrigemDesconhecida);

            string nomeOk;
            BirthError erroNome = ValidarNome(nome, out nomeOk);
            if (erroNome != BirthError.Nenhum) return Falha(erroNome);

            BirthChoice novo = new BirthChoice();
            novo.destinyId = destinyId;
            novo.originId = originId;
            novo.characterName = nomeOk;
            novo.confirmedAtUtc = DateTime.UtcNow.Ticks;

            BirthResult r = default(BirthResult);
            r.Ok = true;
            r.Erro = BirthError.Nenhum;
            r.Escolha = novo;
            return r;
        }

        /// <summary>Reconfere um BirthChoice que veio de fora (save carregado, save editado a mao).
        /// BirthError.Nenhum = coerente com o catalogo (destino existe, origem compativel, nome valido).
        /// LocalSave.Auditar chama no Load e so registra: jogo local detecta, nao impede.
        /// LIMITE: pega id inexistente ("modo_facil" escrito no bloco de notas); NAO pega troca entre ids
        /// validos ("serena" reescrito como "ruptura") nem confirmedAtUtc zerado a mao -- sem assinatura no
        /// arquivo isso e indistinguivel de um save legitimo.
        /// ponytail: se um dia precisar, hash do bloco birth gravado por T004 AO LADO dele no SaveData
        /// (nunca dentro do BirthChoice, que o contrato secao 2 fixa em quatro campos).</summary>
        public static BirthError Validar(BirthChoice escolha)
        {
            if (escolha == null) return BirthError.DestinoDesconhecido;
            if (DestinyCatalog.Destino(escolha.destinyId) == null) return BirthError.DestinoDesconhecido;
            if (!OrigemDisponivel(escolha.destinyId, escolha.originId)) return BirthError.OrigemDesconhecida;
            string ignorado;
            return ValidarNome(escolha.characterName, out ignorado);
        }

        /// <summary>A circunstancia (familia, recursos, contexto, oportunidades, acontecimentos) de uma
        /// escolha ja confirmada. Derivada do catalogo a cada chamada: nao vai para o save, entao mudar
        /// o catalogo depois nao exige migracao de save.</summary>
        public static Circunstancia CircunstanciaDe(BirthChoice escolha)
        {
            if (escolha == null) throw new ArgumentNullException("escolha");
            return DestinyCatalog.Compor(escolha.destinyId, escolha.originId);
        }

        /// <summary>Normaliza e valida o nome do avatar. Devolve BirthError.Nenhum e o nome ja pronto
        /// para gravar (NFC + trim). Aceita letra de qualquer alfabeto -- acento SIM -- mais espaco,
        /// hifen e apostrofo LIGANDO duas letras. Recusa vazio, so espaco, espaco duplo, digito,
        /// pontuacao, caractere de controle e emoji (par substituto nao e letra).</summary>
        public static BirthError ValidarNome(string bruto, out string nome)
        {
            nome = null;
            if (bruto == null) return BirthError.NomeCurto;

            // NFC antes do Trim: "Jose" digitado decomposto (e + acento combinante) vira uma letra so,
            // senao o acento sozinho cairia como caractere invalido e o save teria dois bytes por letra.
            string s = bruto.Normalize(NormalizationForm.FormC).Trim();

            if (s.Length < NomeMinimo) return BirthError.NomeCurto;
            if (s.Length > NomeMaximo) return BirthError.NomeLongo;

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (char.IsLetter(c)) continue;
                bool ligacao = c == ' ' || c == '-' || c == '\'';
                // ligacao so vale entre letras: pega espaco duplo, hifen no fim, apostrofo solto.
                if (!ligacao || i == 0 || i == s.Length - 1 || !char.IsLetter(s[i - 1]))
                    return BirthError.NomeCaractereInvalido;
            }

            nome = s;
            return BirthError.Nenhum;
        }

        /// <summary>Origem existe E e oferecida para este destino. Incompativel cai em OrigemDesconhecida:
        /// desconhecida PARA ESTE destino.</summary>
        static bool OrigemDisponivel(string destinyId, string originId)
        {
            foreach (OriginDef o in OrigensDisponiveis(destinyId)) if (o.Id == originId) return true;
            return false;
        }

        static BirthResult Falha(BirthError erro)
        {
            BirthResult r = default(BirthResult);
            r.Ok = false;
            r.Erro = erro;
            r.Escolha = null;
            return r;
        }
    }
}
