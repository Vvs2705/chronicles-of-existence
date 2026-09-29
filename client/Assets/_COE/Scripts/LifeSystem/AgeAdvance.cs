using System.Collections.Generic;

namespace COE
{
    /// <summary>DEFINICAO de um marco de idade. Imutavel, compartilhada, nunca guarda estado de jogador
    /// (CLAUDE.md: "ScriptableObject e definicao, nao estado" -- vale igual para tabela em codigo).
    /// O Id e ao mesmo tempo o id do marco e o eventId no LifeEventHistory (T005): e ele que torna o salto
    /// idempotente. Segue a convencao que a T005 documenta ("marco_idade_8"): id do FATO, nunca da vez em
    /// que ele aconteceu.</summary>
    public sealed class MarcoDeIdade
    {
        public readonly string Id;          // snake_case ASCII, congelado quando entrar em save
        public readonly int IdadeMinima;    // so vale a partir desta idade
        public readonly int IdadeAlvo;      // idade DEPOIS do salto

        public MarcoDeIdade(string id, int idadeMinima, int idadeAlvo)
        {
            Id = id; IdadeMinima = idadeMinima; IdadeAlvo = idadeAlvo;
        }
    }

    /// <summary>Catalogo v0 dos marcos que envelhecem o personagem.
    /// Hoje ha UM: o salto do slice, de 5 para cerca de 8 anos (dossie secao L e GDD v1.2 cap. 03,
    /// "BASE de prototipo: fase infantil aos cinco anos, um salto temporal para cerca de oito anos").
    /// Os marcos das fases seguintes (12, 16, 19) entram quando houver narrativa que os justifique --
    /// inventa-los agora seria prometer conteudo que nao existe.</summary>
    public static class AgeAdvanceCatalog
    {
        public const string SaltoInfancia = "marco_idade_8";

        public static readonly MarcoDeIdade[] Marcos =
        {
            new MarcoDeIdade(SaltoInfancia, 5, 8),
        };

        /// <summary>O marco pelo id, ou null. Nunca lanca.</summary>
        public static MarcoDeIdade Marco(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Marcos.Length; i++)
                if (Marcos[i].Id == id) return Marcos[i];
            return null;
        }
    }

    /// <summary>Por que o salto nao aconteceu. Nenhum = aconteceu.</summary>
    public enum SaltoBloqueio
    {
        Nenhum = 0,
        MarcoDesconhecido,
        SemSave,
        IdadeInsuficiente,   // ainda nao chegou na IdadeMinima do marco
        NaoAvanca,           // IdadeAlvo <= idade atual: salto nao anda para tras nem fica parado
        JaAplicado,          // ESTE e o exploit n. 5: recarregar ou clicar duas vezes nao envelhece de novo
        PreparoInvalido,     // ConfirmarSalto sem PrepararSalto, ou com preparo de outro marco
    }

    /// <summary>O que o jogador ve ANTES de confirmar. E a tela de aviso do dossie secoes D e L
    /// ("avisar oportunidades que serao encerradas pelo salto") em forma de dado.</summary>
    public sealed class SaltoPreparado
    {
        public bool Possivel;
        public SaltoBloqueio Motivo;
        public string MarcoId;
        public int IdadeAtual;
        public int IdadeDepois;
        public LifePhase FaseAtual;
        public LifePhase FaseDepois;
        public bool MudaDeFase;
        /// <summary>Ids que o salto encerra. Nunca null; vazio quando nao ha nada aberto.</summary>
        public string[] OportunidadesEncerradas = new string[0];
    }

    public struct SaltoResultado
    {
        public bool Aplicado;
        public SaltoBloqueio Motivo;
        public int IdadeAntes;
        public int IdadeDepois;
        public int NivelDeVidaDepois;
        /// <summary>O que o chamador TEM de encerrar agora (missao, oportunidade, dialogo de fase).
        /// A T009 nao mexe em estado de missao: isso e da T006.</summary>
        public string[] OportunidadesEncerradas;
    }

    /// <summary>Envelhecimento por marco narrativo (T009). C# PURO, sem UnityEngine, sem disco.
    ///
    /// REGRA CENTRAL (dossie secao D, GDD v1.2 cap. 03): a idade NAO sobe por caminhar, praticar, avancar o
    /// dia ou repetir tarefa. Sobe em marco narrativo ANUNCIADO e CONFIRMADO. Por isso o unico caminho que
    /// escreve save.ageYears e ConfirmarSalto, e ele exige um SaltoPreparado -- o "anuncio" nao e detalhe de
    /// interface, e passo obrigatorio da API. AgeAdvanceTests.ApiSoTemOsTresMetodos trava isso por reflexao:
    /// quem quiser um atalho de uma chamada so vai quebrar o teste.
    ///
    /// IDEMPOTENCIA (exploit n. 5 do backlog, dossie secao M): a T009 NAO reimplementa idempotencia. Quem
    /// responde "ja aconteceu?" e o LifeEventHistory da T005, pelo eventId do marco -- este arquivo e o unico
    /// ponto da T009 que fala com ela. PrepararSalto le (Ja) para nao anunciar um salto que ja ocorreu;
    /// ConfirmarSalto usa Registrar, que testa-e-marca num passo so e devolve true apenas na primeira vez.
    /// Clique duplo, gatilho repetido e SaltoPreparado guardado antes de um reload caem todos em JaAplicado.
    ///
    /// ATOMICIDADE: idade, Nivel de Vida e registro do marco mudam no MESMO objeto SaveData. Quem grava e o
    /// LocalSave (tmp + replace), entao ou os tres entram no arquivo, ou nenhum entra.</summary>
    public static class AgeAdvance
    {
        static readonly string[] Nada = new string[0];

        /// <summary>Da para envelhecer por este marco agora? Consulta pura: nao altera nada.
        /// false quando o marco nao existe, a idade ainda nao chegou, o alvo nao avanca, ou o marco JA
        /// aconteceu.</summary>
        public static bool PodeAvancarIdade(SaveData save, string marcoId, LifeEventHistory historico)
        {
            return Bloqueio(save, AgeAdvanceCatalog.Marco(marcoId), historico) == SaltoBloqueio.Nenhum;
        }

        /// <summary>Monta o aviso: quanto o personagem vai envelhecer, se muda de fase da vida e O QUE SERA
        /// ENCERRADO. Nao altera nada -- esta e a metade "anuncio" do par anuncio/confirmacao.
        ///
        /// oportunidadesAbertas vem de fora (missoes e oportunidades ainda abertas, que sao estado da
        /// T006/T007). A T009 nao adivinha o que esta aberto e nao fecha nada: ela lista, a interface avisa,
        /// e ConfirmarSalto devolve a mesma lista para quem e dono fechar.
        /// Entrada null vira lista vazia; a lista devolvida e sempre uma COPIA, nunca o array do chamador.</summary>
        public static SaltoPreparado PrepararSalto(SaveData save, string marcoId, LifeEventHistory historico,
            IList<string> oportunidadesAbertas)
        {
            MarcoDeIdade marco = AgeAdvanceCatalog.Marco(marcoId);
            SaltoPreparado p = new SaltoPreparado();
            p.MarcoId = marcoId;
            p.Motivo = Bloqueio(save, marco, historico);
            p.Possivel = p.Motivo == SaltoBloqueio.Nenhum;

            p.IdadeAtual = save == null ? 0 : save.ageYears;
            p.IdadeDepois = marco == null ? p.IdadeAtual : marco.IdadeAlvo;
            p.FaseAtual = LifePhases.De(p.IdadeAtual);
            p.FaseDepois = LifePhases.De(p.IdadeDepois);
            p.MudaDeFase = p.FaseAtual != p.FaseDepois;

            // So lista encerramento quando o salto pode mesmo acontecer: avisar "voce vai perder X" num
            // salto ja aplicado (reload) seria assustar o jogador com uma perda que nao vai ocorrer.
            p.OportunidadesEncerradas = p.Possivel ? Copia(oportunidadesAbertas) : Nada;
            return p;
        }

        /// <summary>Aplica o salto. Este e o UNICO metodo de todo o COE que escreve save.ageYears.
        ///
        /// Recusa (sem alterar nada) quando: preparo null, preparo de outro marco, preparo marcado como
        /// impossivel, ou o marco ja consta no historico AGORA -- a reconsulta cobre o caso de preparar,
        /// salvar, recarregar e confirmar com um preparo velho na mao.
        ///
        /// Efeitos, todos no mesmo SaveData: idade vai para IdadeAlvo; Nivel de Vida sobe UM (LifeLevel, que
        /// explica por que isso nao multiplica poder); o dia recomeca de manha; o marco entra no historico.
        /// O ledger de pratica NAO e limpo: ele e por (atividade, fase), entao mudar de fase ja reabre o teto
        /// sozinho, e apagar apagaria a historia do que o personagem aprendeu na infancia.</summary>
        public static SaltoResultado ConfirmarSalto(SaveData save, SaltoPreparado preparado, LifeEventHistory historico)
        {
            SaltoResultado r = default(SaltoResultado);
            r.OportunidadesEncerradas = Nada;

            if (preparado == null || !preparado.Possivel)
                return Recusa(r, SaltoBloqueio.PreparoInvalido, save);

            MarcoDeIdade marco = AgeAdvanceCatalog.Marco(preparado.MarcoId);
            SaltoBloqueio agora = Bloqueio(save, marco, historico);
            if (agora != SaltoBloqueio.Nenhum)
                return Recusa(r, agora, save);   // JaAplicado cai aqui: clique duplo, reload, gatilho repetido

            // REGISTRAR PRIMEIRO, ENVELHECER DEPOIS -- e o padrao que a propria T005 documenta. Registrar
            // devolve true so na primeira vez; se dois gatilhos entrarem juntos, o segundo ve false e para
            // aqui, ANTES de escrever idade. A checagem acima ja teria pego, mas ela e leitura: so este
            // retorno e testa-e-marca num passo.
            if (!historico.Registrar(marco.Id, LifeEventCategoria.Marco, marco.IdadeAlvo))
                return Recusa(r, SaltoBloqueio.JaAplicado, save);

            r.IdadeAntes = save.ageYears;
            save.ageYears = marco.IdadeAlvo;
            save.lifeLevel = LifeLevel.AposMarco(save.lifeLevel);

            if (save.life != null)
            {
                save.life.day++;
                save.life.timeOfDay = TimeOfDayCycle.IdManha;   // depois do salto, amanhece
            }

            r.Aplicado = true;
            r.Motivo = SaltoBloqueio.Nenhum;
            r.IdadeDepois = save.ageYears;
            r.NivelDeVidaDepois = save.lifeLevel;
            r.OportunidadesEncerradas = preparado.OportunidadesEncerradas ?? Nada;
            return r;
        }

        static SaltoBloqueio Bloqueio(SaveData save, MarcoDeIdade marco, LifeEventHistory historico)
        {
            if (save == null) return SaltoBloqueio.SemSave;
            if (marco == null) return SaltoBloqueio.MarcoDesconhecido;
            if (historico == null) return SaltoBloqueio.SemSave;   // sem historico nao ha idempotencia a garantir
            if (historico.Ja(marco.Id)) return SaltoBloqueio.JaAplicado;
            if (save.ageYears < marco.IdadeMinima) return SaltoBloqueio.IdadeInsuficiente;
            if (marco.IdadeAlvo <= save.ageYears) return SaltoBloqueio.NaoAvanca;
            return SaltoBloqueio.Nenhum;
        }

        static SaltoResultado Recusa(SaltoResultado r, SaltoBloqueio motivo, SaveData save)
        {
            r.Aplicado = false;
            r.Motivo = motivo;
            r.IdadeAntes = save == null ? 0 : save.ageYears;
            r.IdadeDepois = r.IdadeAntes;                      // nada mudou, e o resultado diz isso
            r.NivelDeVidaDepois = save == null ? 0 : save.lifeLevel;
            return r;
        }

        static string[] Copia(IList<string> origem)
        {
            if (origem == null || origem.Count == 0) return Nada;
            string[] copia = new string[origem.Count];
            for (int i = 0; i < origem.Count; i++) copia[i] = origem[i];
            return copia;
        }
    }
}
