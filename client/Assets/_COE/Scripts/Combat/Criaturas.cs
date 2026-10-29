using System;

namespace COE
{
    /// <summary>Uma criatura do Bosque dos Sussurros (EPISODIO2 §3.4). DEFINICAO, nao estado: o estado da luta vive no
    /// CriaturaBrain. Ritmo fixo e deterministico como o TrainingDummyBrain: o jogador aprende o aviso de cada uma.
    /// Derrotar = dispersar (ADR-0009): a criatura foge, nao morre, e nao larga item, moeda nem XP.</summary>
    public sealed class CriaturaDef
    {
        public readonly string Id;
        /// <summary>Ataque forte quebra a postura se peso &lt;= Forca + 1 (§3.3). Abafador 1, feixe 2, lajeiro 3, Enredado 4.</summary>
        public readonly int Peso;
        public readonly float Vida;
        public readonly float Espreita, Aviso, Bote, Janela;   // segundos de cada fase do ciclo
        public readonly float Dano;
        /// <summary>Feixe: fechado, e mata (golpe nao entra fora da janela).</summary>
        public readonly bool SoNaJanela;
        /// <summary>Lajeiro: de frente as lajes seguram o golpe; so o flanco ou as costas entram.</summary>
        public readonly bool SoPeloFlanco;
        /// <summary>O som do aviso (enum Som, thread de audio).</summary>
        public readonly Som SomDoAviso;

        public CriaturaDef(string id, int peso, float vida, float espreita, float aviso, float bote, float janela, float dano,
                           bool soNaJanela, bool soPeloFlanco, Som somDoAviso)
        {
            Id = id; Peso = peso; Vida = vida; Espreita = espreita; Aviso = aviso; Bote = bote; Janela = janela; Dano = dano;
            SoNaJanela = soNaJanela; SoPeloFlanco = soPeloFlanco; SomDoAviso = somDoAviso;
        }
    }

    /// <summary>Um encontro: quem aparece, onde e que objetivo de missao ele cumpre ao ser vencido.</summary>
    public sealed class EncontroDef
    {
        public readonly string Id;
        public readonly string QuestId, ObjetivoId, Ancora;
        /// <summary>Ids de criatura, um por individuo (ordem = ordem de entrada).</summary>
        public readonly string[] Criaturas;

        public EncontroDef(string id, string questId, string objetivoId, string ancora, params string[] criaturas)
        {
            Id = id; QuestId = questId; ObjetivoId = objetivoId; Ancora = ancora; Criaturas = criaturas;
        }
    }

    /// <summary>Catalogo das criaturas e encontros do ep. 2 (EPISODIO2 §3.4 e B08/B12/B14/B15, PROPOSTA). C# PURO.
    /// Numeros sao hipotese v0 (mesmo regime do TrainingDummyBrain): aviso longo o bastante para uma crianca ler.
    /// ponytail: o encontro enc.espinhal_feixes_noite da q15 nao tem objetivo proprio no JSON (o trecho do espinhal
    /// fica entre atravessar_a_trilha e o_lajedo); entra quando a q15 ganhar o objetivo. Malha so depois do G1/G2/G3.</summary>
    public static class Criaturas
    {
        public const string Abafador = "criatura.abafador";
        public const string Feixe = "criatura.feixe_de_espinho";
        public const string Lajeiro = "criatura.lajeiro";
        public const string Enredado = "criatura.enredado";

        public static readonly CriaturaDef[] Todas =
        {
            //               id         peso vida esp.  aviso bote janela dano  soJanela soFlanco som
            new CriaturaDef(Abafador,  1,   12f, 2.4f, 0.6f, 0.3f, 1.0f,  4f, false,   false,   Som.AvisoAbafador),
            new CriaturaDef(Feixe,     2,   18f, 2.0f, 0.8f, 0.2f, 1.5f,  6f, true,    false,   Som.AvisoFeixe),
            new CriaturaDef(Lajeiro,   3,   30f, 2.8f, 1.0f, 0.6f, 1.5f,  8f, false,   true,    Som.AvisoLajeiro),
            // O Enredado nao perde vida: perde nos (EnredadoBrain). Vida aqui so para o Health da casca nao zerar.
            new CriaturaDef(Enredado,  4,  999f, 2.6f, 1.2f, 0.8f, 2.0f,  8f, true,    false,   Som.AvisoLajeiro),
        };

        public static readonly EncontroDef[] Encontros =
        {
            new EncontroDef("enc.trilha_abafadores_borda", "q09_a_linha_do_posto", "primeira_luta_na_borda", "bosque_trilha",
                            Abafador, Abafador, Abafador, Abafador),
            new EncontroDef("enc.espinhal_feixes", "q13_o_fio_do_nilo", "seguir_o_fio", "bosque_espinhal",
                            Feixe, Feixe),
            new EncontroDef("enc.trilha_misto", "q15_a_anomalia_no_bosque", "atravessar_a_trilha", "bosque_trilha",
                            Abafador, Abafador, Abafador, Feixe),
            new EncontroDef("enc.lajedo_lajeiro", "q15_a_anomalia_no_bosque", "o_lajedo", "bosque_lajedo",
                            Lajeiro),
            new EncontroDef("enc.coracao_enredado", "q16_o_enredado", "cortar_os_tres_nos", "bosque_coracao",
                            Enredado, Abafador, Abafador, Abafador),
        };

        public static CriaturaDef Def(string id)
        {
            foreach (CriaturaDef d in Todas) if (d.Id == id) return d;
            return null;
        }

        public static EncontroDef Encontro(string id)
        {
            foreach (EncontroDef e in Encontros) if (e.Id == id) return e;
            return null;
        }

        /// <summary>O encontro que cumpre este objetivo, ou null (objetivo sem luta).</summary>
        public static EncontroDef DoObjetivo(string questId, string objetivoId)
        {
            foreach (EncontroDef e in Encontros) if (e.QuestId == questId && e.ObjetivoId == objetivoId) return e;
            return null;
        }

        /// <summary>§3.3: ataque forte quebra a postura (atordoa) se o peso cabe na Forca.</summary>
        public static bool QuebraPostura(CriaturaDef d, int forca) { return d != null && d.Peso <= forca + 1; }
    }

    public enum CriaturaFase
    {
        Espreita = 0,   // parada (feixe fechado = mata)
        Aviso = 1,      // o leque abre / o estalo / a pata raspa: o som e a cor que o jogador aprende
        Bote = 2,       // o golpe sai na ENTRADA desta fase
        Janela = 3,     // aberta depois do bote: e aqui que se bate
        Atordoada = 4,  // postura quebrada ou presa no tronco: tambem e janela
        Fugiu = 5,      // dispersou: sai de cena e nao volta neste encontro
    }

    /// <summary>Cerebro de UMA criatura. C# PURO e DETERMINISTICO (mesmo dt, mesma sequencia), como o TrainingDummyBrain:
    /// Espreita -> Aviso -> Bote -> Janela -> Espreita. Nenhum sorteio; quem decide quando o bando ataca e o EncontroState
    /// (um de cada vez).</summary>
    public sealed class CriaturaBrain
    {
        public readonly CriaturaDef Def;
        public CriaturaFase Fase { get; private set; }
        public float Vida { get; private set; }
        float t, atordoadaPor;

        public CriaturaBrain(CriaturaDef def)
        {
            if (def == null) throw new ArgumentNullException("def");
            Def = def;
            Reiniciar();
        }

        public bool Fora { get { return Fase == CriaturaFase.Fugiu; } }
        /// <summary>Golpe do jogador entra agora (antes do angulo, que so a casca sabe).</summary>
        public bool Aberta
        {
            get
            {
                if (Fora) return false;
                if (Fase == CriaturaFase.Janela || Fase == CriaturaFase.Atordoada) return true;
                return !Def.SoNaJanela;
            }
        }

        public void Reiniciar() { Fase = CriaturaFase.Espreita; Vida = Def.Vida; t = 0f; atordoadaPor = 0f; }

        /// <summary>Comeca o aviso agora (o EncontroState escolhe quem). So vale em Espreita. true = comecou.</summary>
        public bool Atacar()
        {
            if (Fase != CriaturaFase.Espreita) return false;
            Fase = CriaturaFase.Aviso;
            t = 0f;
            return true;
        }

        /// <summary>Avanca o tempo. true no passo em que o bote SAI. A Espreita nao termina sozinha: espera Atacar().</summary>
        public bool Tick(float dt, float escalaDoAviso = 1f)
        {
            if (dt <= 0f || Fora) return false;
            if (Fase == CriaturaFase.Atordoada)
            {
                atordoadaPor -= dt;
                if (atordoadaPor <= 0f) { Fase = CriaturaFase.Espreita; t = 0f; }
                return false;
            }
            if (Fase == CriaturaFase.Espreita) return false;
            t += dt;
            bool saiu = false;
            int guarda = 0;
            while (Fase != CriaturaFase.Espreita && t >= Duracao(escalaDoAviso) && guarda++ < 8)
            {
                t -= Duracao(escalaDoAviso);
                switch (Fase)
                {
                    case CriaturaFase.Aviso: Fase = CriaturaFase.Bote; saiu = true; break;
                    case CriaturaFase.Bote: Fase = CriaturaFase.Janela; break;
                    default: Fase = CriaturaFase.Espreita; t = 0f; break;
                }
            }
            return saiu;
        }

        float Duracao(float escalaDoAviso)
        {
            switch (Fase)
            {
                case CriaturaFase.Aviso: return Def.Aviso * Math.Max(1f, escalaDoAviso);   // assist.aviso_longo (§3.5)
                case CriaturaFase.Bote: return Def.Bote;
                default: return Def.Janela;
            }
        }

        /// <summary>Golpe do jogador. pelaFrente: a casca diz se o golpe veio de frente (lajeiro segura). forte + forca:
        /// quebra de postura. Devolve o dano que entrou (0 = nao entrou). Vida a zero = Fugiu.</summary>
        public float Golpear(float dano, bool pelaFrente = false, bool forte = false, int forca = 0)
        {
            if (!Aberta || dano <= 0f) return 0f;
            bool atordoada = Fase == CriaturaFase.Atordoada;
            if (Def.SoPeloFlanco && pelaFrente && !atordoada) return 0f;
            Vida = Math.Max(0f, Vida - dano);
            if (Vida <= 0f) { Fase = CriaturaFase.Fugiu; return dano; }
            if (forte && Criaturas.QuebraPostura(Def, forca)) Atordoar(1.5f);
            return dano;
        }

        /// <summary>Postura quebrada (1,5 s) ou lajeiro preso no tronco (2 s).</summary>
        public void Atordoar(float segundos)
        {
            if (Fora || segundos <= 0f) return;
            Fase = CriaturaFase.Atordoada;
            atordoadaPor = Math.Max(atordoadaPor, segundos);
        }

        /// <summary>Foge sem luta (ultimo do bando, Natural, ou o Enredado libertado).</summary>
        public void Dispersar() { Fase = CriaturaFase.Fugiu; }
    }

    /// <summary>Os tres nos do Enredado (EPISODIO2 §3.4): cada um so se corta enquanto ele esta aberto (janela ou
    /// atordoado). Tres nos = libertado. Ordem livre.
    /// ponytail: so o caminho-base (golpe na janela). Os caminhos de afinidade (contra, cravo, fio de luz, chamar um
    /// companheiro) entram como outras chamadas de Cortar quando o Mastery do ep. 2 existir.</summary>
    public enum NoDoEnredado { Frente = 0, Flanco = 1, Cima = 2 }

    /// <summary>A luta inteira: um de cada vez ataca, o ultimo abafador foge sozinho, derrota recomeca do zero sem perder
    /// nada (§3.5). C# PURO; a casca (cena) le Fase de cada criatura e chama Golpear/Cortar.</summary>
    public sealed class EncontroState
    {
        public readonly EncontroDef Def;
        public readonly CriaturaBrain[] Bando;
        readonly bool[] nos = new bool[3];
        int vez;           // indice de quem ataca a seguir (rodizio fixo)
        float pausa;       // segundos ate o proximo aviso
        public const float PausaEntreAtaques = 0.8f;

        public EncontroState(EncontroDef def)
        {
            if (def == null) throw new ArgumentNullException("def");
            Def = def;
            Bando = new CriaturaBrain[def.Criaturas.Length];
            for (int i = 0; i < Bando.Length; i++)
            {
                CriaturaDef c = Criaturas.Def(def.Criaturas[i]);
                if (c == null) throw new ArgumentException(def.Id + ": criatura desconhecida " + def.Criaturas[i]);
                Bando[i] = new CriaturaBrain(c);
            }
            pausa = PausaEntreAtaques;
        }

        public int Derrotas { get; private set; }

        public CriaturaBrain Enredado
        {
            get { foreach (CriaturaBrain b in Bando) if (b.Def.Id == Criaturas.Enredado) return b; return null; }
        }

        public bool NoCortado(NoDoEnredado no) { return nos[(int)no]; }

        public int NosCortados { get { int n = 0; foreach (bool c in nos) if (c) n++; return n; } }

        /// <summary>Vencido: com Enredado, os tres nos; sem ele, todo o bando fugiu.</summary>
        public bool Vencido
        {
            get
            {
                if (Enredado != null) return NosCortados == nos.Length;
                foreach (CriaturaBrain b in Bando) if (!b.Fora) return false;
                return true;
            }
        }

        /// <summary>Avanca a luta. Devolve as criaturas cujo bote saiu neste passo (a casca aplica o dano), ou vazio.</summary>
        public CriaturaBrain[] Tick(float dt, float escalaDoAviso = 1f)
        {
            if (dt <= 0f || Vencido) return Nenhuma;
            int saiu = 0;
            CriaturaBrain[] botes = null;
            bool alguemAtacando = false;
            foreach (CriaturaBrain b in Bando)
            {
                if (b.Tick(dt, escalaDoAviso))
                {
                    if (botes == null) botes = new CriaturaBrain[Bando.Length];
                    botes[saiu++] = b;
                }
                if (b.Fase == CriaturaFase.Aviso || b.Fase == CriaturaFase.Bote) alguemAtacando = true;
            }
            if (!alguemAtacando)
            {
                pausa -= dt;
                if (pausa <= 0f && ProximoAtacar()) pausa = PausaEntreAtaques;
            }
            if (botes == null) return Nenhuma;
            Array.Resize(ref botes, saiu);
            return botes;
        }

        static readonly CriaturaBrain[] Nenhuma = new CriaturaBrain[0];

        bool ProximoAtacar()
        {
            for (int i = 0; i < Bando.Length; i++)
            {
                CriaturaBrain b = Bando[(vez + i) % Bando.Length];
                if (b.Atacar()) { vez = (vez + i + 1) % Bando.Length; return true; }
            }
            return false;
        }

        /// <summary>Golpe do jogador numa criatura. Quando sobra um abafador (e so abafadores), ele foge (§3.4).</summary>
        public float Golpear(CriaturaBrain alvo, float dano, bool pelaFrente = false, bool forte = false, int forca = 0)
        {
            if (alvo == null || Array.IndexOf(Bando, alvo) < 0) return 0f;
            if (alvo.Def.Id == Criaturas.Enredado) return 0f;   // o Enredado nao perde vida: perde nos (Cortar)
            float entrou = alvo.Golpear(dano, pelaFrente, forte, forca);
            if (entrou > 0f) UltimoAbafadorFoge();
            return entrou;
        }

        /// <summary>Corta um no do Enredado: so com ele aberto. true = cortou agora. Tres nos: o anel se desfaz e todo o
        /// bando dispersa.</summary>
        public bool Cortar(NoDoEnredado no)
        {
            CriaturaBrain e = Enredado;
            if (e == null || nos[(int)no] || !e.Aberta) return false;
            nos[(int)no] = true;
            if (NosCortados == nos.Length) foreach (CriaturaBrain b in Bando) b.Dispersar();
            return true;
        }

        void UltimoAbafadorFoge()
        {
            CriaturaBrain sobra = null;
            int vivos = 0;
            foreach (CriaturaBrain b in Bando)
            {
                if (b.Fora) continue;
                if (b.Def.Id != Criaturas.Abafador) return;   // tem outra criatura: o bando nao se desfaz sozinho
                vivos++;
                sobra = b;
            }
            if (vivos == 1 && Bando.Length > 1) sobra.Dispersar();
        }

        /// <summary>A crianca caiu: Tovin puxa para tras, o encontro recomeca do inicio, nada perdido (§3.5).
        /// Nos ja cortados tambem voltam: a luta e uma so.</summary>
        public void Derrota()
        {
            Derrotas++;
            foreach (CriaturaBrain b in Bando) b.Reiniciar();
            for (int i = 0; i < nos.Length; i++) nos[i] = false;
            vez = 0;
            pausa = PausaEntreAtaques;
        }
    }
}
