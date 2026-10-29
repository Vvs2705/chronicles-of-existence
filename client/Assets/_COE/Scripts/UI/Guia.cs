using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>Onde o passo do guia acende (GuiaHud converte em px de tela). Nenhum = cartao no meio, sem buraco.</summary>
    public enum GuiaAlvo { Nenhum, Joystick, Olhar, CartaoMissao, Indicador, Usar, Menu, Itens, Ataque }

    /// <summary>Um passo do tutorial guiado do episodio 1 (definicao, nao estado: o que ja foi visto mora nas Configuracoes).
    /// Explicacao: o jogo congela e o cartao diz o que e, quando aparece e por que importa, com ENTENDI. Acao: o alvo fica
    /// aberto e o passo termina quando o jogador faz (andar, girar a camera, usar). Contexto: cartao de historia no lugar de
    /// cutscene, com um texto so (Strings.Format com o nome e o oficio da casa).
    /// Textos por id: guia.&lt;id&gt;.titulo e guia.&lt;id&gt;.oque/.quando/.porque, ou guia.&lt;id&gt;.texto no contexto.</summary>
    public sealed class GuiaPasso
    {
        public readonly string Id;          // snake_case, estavel: vai para a lista de vistos (coe.cfg.v1.guia)
        public readonly GuiaAlvo Alvo;
        public readonly bool Acao;          // termina quando o jogador faz; false = ENTENDI
        public readonly bool Contexto;      // cartao de historia (texto unico), sem alvo

        public GuiaPasso(string id, GuiaAlvo alvo, bool acao, bool contexto = false)
        {
            Id = id;
            Alvo = alvo;
            Acao = acao;
            Contexto = contexto;
        }

        public string TituloKey { get { return "guia." + Id + ".titulo"; } }
        public string TextoKey { get { return "guia." + Id + ".texto"; } }
        public string OQueKey { get { return "guia." + Id + ".oque"; } }
        public string QuandoKey { get { return "guia." + Id + ".quando"; } }
        public string PorQueKey { get { return "guia." + Id + ".porque"; } }

        /// <summary>Todas as chaves de texto do passo (StringsCobertura confere que existem).</summary>
        public string[] Chaves
        {
            get { return Contexto ? new[] { TituloKey, TextoKey } : new[] { TituloKey, OQueKey, QuandoKey, PorQueKey }; }
        }
    }

    /// <summary>O que o guia le do jogo neste quadro (GuiaHud monta; o teste monta na mao).</summary>
    public struct GuiaEstado
    {
        public bool NoControle;       // jogador anda e nenhum modal esta aberto
        public bool Toque;            // ha controles de toque na tela (aparelho ou -toque no PC)
        public int Idade;
        public bool CartaoMissao;     // o cartao de missao esta visivel
        public bool Seta;             // o indicador de objetivo esta visivel
        public bool AlvoDeUso;        // ha algo para USAR na frente (o prompt aparece)
        public bool TemItem;          // o inventario mostra pelo menos um item
        public bool Golpes;           // treino liberado (aos 8): os botoes de golpe existem
        public bool Promessa;         // a q04 (a promessa de Sera) esta em andamento
        public int Concluidas;        // missoes concluidas
    }

    /// <summary>Regra do tutorial guiado (FTUE do episodio 1, padrao Tour.cs do Dungeon Courier: congela, acende o alvo,
    /// cartao o que/quando/por que, Pular e Rever). C# puro: a ordem e as condicoes, sem tela.
    /// Cada passo aparece uma vez. O primeiro da lista cuja condicao vale e que ainda nao foi visto e o proximo; um passo
    /// cuja condicao nao vale espera (nao bloqueia os outros). Os cartoes de contexto levam os diferenciais aos primeiros
    /// minutos (docs/COMPETITIVO.md (d)): C1/C2 no despertar (a origem escolhida e lida de volta e o salto aos 8 e
    /// prometido), C3 na promessa (a vila lembra) e C4 no treino (aprende fazendo, sem XP por abate).
    /// ponytail: passos fixos do episodio 1. Episodio novo acrescenta passo aqui (id novo nunca reaproveita um antigo).</summary>
    public static class GuiaRegras
    {
        public const string Despertar = "despertar", Mover = "mover", Olhar = "olhar", Missao = "missao", Rumo = "rumo",
                            Usar = "usar", Promessa = "promessa", Menu = "menu", Itens = "itens", Golpes = "golpes";

        public static readonly GuiaPasso[] Passos =
        {
            new GuiaPasso(Despertar, GuiaAlvo.Nenhum, false, true),
            new GuiaPasso(Mover, GuiaAlvo.Joystick, true),
            new GuiaPasso(Olhar, GuiaAlvo.Olhar, true),
            new GuiaPasso(Missao, GuiaAlvo.CartaoMissao, false),
            new GuiaPasso(Rumo, GuiaAlvo.Indicador, false),
            new GuiaPasso(Usar, GuiaAlvo.Usar, true),
            new GuiaPasso(Promessa, GuiaAlvo.Nenhum, false, true),
            new GuiaPasso(Menu, GuiaAlvo.Menu, false),
            new GuiaPasso(Itens, GuiaAlvo.Itens, false),
            new GuiaPasso(Golpes, GuiaAlvo.Ataque, false),
        };

        public static GuiaPasso Passo(string id)
        {
            foreach (GuiaPasso p in Passos) if (p.Id == id) return p;
            return null;
        }

        /// <summary>O passo vale agora? So com o jogador no controle; os de toque so com toque na tela.</summary>
        public static bool Vale(GuiaPasso p, GuiaEstado e, Func<string, bool> visto)
        {
            if (p == null || !e.NoControle) return false;
            switch (p.Id)
            {
                case Despertar: return !TrainingProgress.PodeTreinar(e.Idade);   // so na primeira infancia
                case Mover: return e.Toque;
                case Olhar: return e.Toque && visto(Mover);
                case Missao: return e.CartaoMissao;
                case Rumo: return e.Seta && visto(Missao);
                case Usar: return e.Toque && e.AlvoDeUso;
                case Promessa: return e.Promessa;
                case Menu: return e.Concluidas >= 1;
                case Itens: return e.TemItem && e.Concluidas >= 1;
                case Golpes: return e.Toque && e.Golpes;
            }
            return false;
        }

        /// <summary>O proximo passo a mostrar, ou null.</summary>
        public static GuiaPasso Proximo(GuiaEstado e, Func<string, bool> visto)
        {
            foreach (GuiaPasso p in Passos)
                if (!visto(p.Id) && Vale(p, e, visto)) return p;
            return null;
        }

        /// <summary>Lista de vistos gravada ("a,b,c") para conjunto. Id fora do catalogo fica (passo de versao futura).</summary>
        public static HashSet<string> Ler(string gravado)
        {
            var r = new HashSet<string>();
            if (string.IsNullOrEmpty(gravado)) return r;
            foreach (string id in gravado.Split(','))
            {
                string t = id.Trim();
                if (t.Length > 0) r.Add(t);
            }
            return r;
        }

        /// <summary>Conjunto para a forma gravada, em ordem estavel (a do catalogo; desconhecidos no fim, em ordem alfabetica).</summary>
        public static string Gravar(ICollection<string> vistos)
        {
            var lista = new List<string>();
            foreach (GuiaPasso p in Passos) if (vistos.Contains(p.Id)) lista.Add(p.Id);
            var resto = new List<string>();
            foreach (string id in vistos) if (Passo(id) == null) resto.Add(id);
            resto.Sort(StringComparer.Ordinal);
            lista.AddRange(resto);
            return string.Join(",", lista.ToArray());
        }
    }
}
