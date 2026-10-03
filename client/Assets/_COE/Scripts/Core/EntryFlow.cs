using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace COE
{
    /// <summary>T012 — porta de entrada da partida. Mora SO na Bootstrap (EntradaSceneSetup; Populate e Auren nao a poem).
    /// Abrir pelo icone: save de versao mais nova -> aviso (a sessao roda sem gravar) e segue; sem destino -> tela de
    /// nascimento (B02-B05); com destino -> a cena salva. Com -scene na linha de comando (desenvolvimento, DevSceneArg)
    /// nao faz nada: -scene Bootstrap continua sendo a area de treino.
    /// O nascimento abre por birth.destinyId vazio, NUNCA por confirmedAtUtc (regra do SaveState). A tela nao tem regra
    /// propria: valida e confirma por DestinySystem, grava em SaveState.Current.birth e pede UM Commit antes de Auren (B05).
    /// B01: quem vai nascer passa antes pelo Limiar (falas de Aethron, LimiarRoteiro) num palco desligado da propria
    /// Bootstrap (camera + simbolo, EntradaSceneSetup); a aparencia (B04) saiu do slice (ADR-0007).
    /// ponytail: prototipo em OnGUI. A UI de verdade (T013) troca so o desenho; Decidir, ValidarEscolha e DestinySystem ficam.</summary>
    public class EntryFlow : MonoBehaviour
    {
        public enum Rota { Nenhuma, Nascimento, Cena }

        /// <summary>Cena de quem ja nasceu e nao tem cena salva (ou tem uma que esta build nao conhece).</summary>
        public const string CenaInicial = "Auren";

        /// <summary>Nome ja preenchido e editavel (B04). Com acento de proposito: o slice cobra "Iris" com acento intacto.</summary>
        public const string NomePadrao = "Íris";

        [Tooltip("Desligado enquanto a tela esta aberta: tocar a tela nao anda, nao gira a camera nem ataca. Ligado pelo gerador.")]
        [SerializeField] PlayerInputReader input;

        [Tooltip("Palco do Limiar (B01): camera e simbolo, desligado. Ligado so na tela do Limiar. Ligado pelo gerador.")]
        [SerializeField] GameObject limiar;

        // ---------- regra (pura, EditMode) ----------

        /// <summary>A rota da entrada. temSceneArg = -scene na linha de comando: fica onde o dev pediu.
        /// cenasNoBuild = cenas do Build Settings SEM a propria entrada (senao sceneId "bootstrap" voltaria para ca em laco).
        /// sceneId e snake_case ("auren") e a cena se chama "Auren": compara sem caixa. Fora da lista = CenaInicial.
        /// ponytail: id -> cena so pela caixa; cena de nome composto (ex.: "BosqueDosSussurros") pede tabela id -> cena.</summary>
        public static Rota Decidir(bool temSceneArg, BirthChoice birth, string sceneId, IList<string> cenasNoBuild, out string cena)
        {
            cena = null;
            if (temSceneArg) return Rota.Nenhuma;
            if (!DestinySystem.EstaConfirmada(birth)) return Rota.Nascimento;
            cena = CenaInicial;
            if (!string.IsNullOrEmpty(sceneId) && cenasNoBuild != null)
                foreach (string c in cenasNoBuild)
                    if (string.Equals(c, sceneId, StringComparison.OrdinalIgnoreCase)) cena = c;
            return Rota.Cena;
        }

        /// <summary>true = LocalSave vai recusar toda gravacao desta sessao: o save.json OU o .bak e de versao mais nova
        /// que a build (a mesma regra de LocalSave.Save). Ausente ou ilegivel = false (isso o LocalSave resolve sozinho).</summary>
        public static bool SaveMaisNovo(string path) { return MaisNovo(path) || MaisNovo(LocalSave.BackupPath(path)); }

        static bool MaisNovo(string path)
        {
            try { return File.Exists(path) && LocalSave.VersionOf(File.ReadAllText(path)) > SaveData.SchemaVersion; }
            catch (Exception) { return false; }
        }

        /// <summary>O que impede ir para o "tem certeza" (Nenhum = pode). E a regra de DestinySystem.Confirmar, sem gravar
        /// nem mudar nada: a tela nao tem validacao propria que possa discordar da confirmacao.</summary>
        public static BirthError ValidarEscolha(string destinyId, string originId, string nome)
        {
            return DestinySystem.Confirmar(null, destinyId, originId, nome).Erro;
        }

        // ---------- runtime ----------

        enum Tela { Nenhuma, Aviso, Titulo, NovaVida, Limiar, Destino, Origem, Nome, Certeza }

        Tela tela;
        Rota rota;
        string cena, destino, origem, erro = "";
        string nome = NomePadrao;
        int fala;   // indice em LimiarRoteiro.Falas
        TouchScreenKeyboard teclado;
        GUIStyle titulo, texto, botao, cartao, campo, tituloGrande, subtitulo;
        float alvo;   // altura de botao em px: >= 48 dp e proporcional a tela

        void Start()
        {
            // SaveBootstrap (-200) ja carregou o save no Awake; DevSceneArg (AfterSceneLoad) ja pediu a cena do -scene.
            rota = Decidir(DevSceneArg.Tem("-scene"), SaveState.Current.birth, SaveState.Current.sceneId, CenasNoBuild(), out cena);
            if (rota == Rota.Nenhuma) { enabled = false; return; }
            StringsLoader.EnsureLoaded();
            if (input != null) input.enabled = false;   // a Bootstrap some no LoadScene; nao precisa religar
            if (SaveMaisNovo(LocalSave.DefaultPath)) tela = Tela.Aviso;
            else Titulo();
        }

        /// <summary>Tela de titulo (2026-10-02): o jogo abre no nome dele, com o simbolo do Limiar ao fundo, e o jogador
        /// escolhe Comecar/Continuar ou Nova vida. Antes abria direto no Limiar ou em Auren, sem como recomecar.</summary>
        void Titulo()
        {
            if (limiar != null) limiar.SetActive(true);
            tela = Tela.Titulo;
        }

        bool Nasceu { get { return !string.IsNullOrEmpty(SaveState.Current.birth.destinyId); } }

        /// <summary>Nova vida confirmada: save em branco gravado por cima (o anterior fica no .bak do LocalSave) e o
        /// nascimento recomeca pelo Limiar.</summary>
        void RecomecarVida()
        {
            SaveState.Current = new SaveData();
            SaveState.Commit();
            destino = null; origem = null; nome = NomePadrao; erro = "";
            rota = Rota.Nascimento;
            Seguir();
        }

        void Seguir()
        {
            if (rota == Rota.Nascimento)
            {
                if (limiar == null) { tela = Tela.Destino; return; }
                limiar.SetActive(true);
                fala = 0;
                tela = Tela.Limiar;
                return;
            }
            tela = Tela.Nenhuma;
            SceneManager.LoadScene(cena);
        }

        /// <summary>Nasce com esta escolha pelo mesmo caminho do botao da tela (validacao, inventario, Commit, Auren).
        /// Quem chama de fora e o Roteiro (simulacao de desenvolvimento).</summary>
        public void Nascer(string destinoId, string origemId, string nomeEscolhido)
        {
            destino = destinoId; origem = origemId; nome = nomeEscolhido;
            Nascer();
        }

        void Nascer()
        {
            BirthResult r = DestinySystem.Confirmar(SaveState.Current.birth, destino, origem, nome);
            if (!r.Ok) { erro = Erro(r.Erro); tela = Tela.Nome; return; }
            SaveState.Current.birth = r.Escolha;
            Inventario.Nascer(SaveState.Current.inventario, DestinySystem.CircunstanciaDe(r.Escolha));   // mesma gravacao
            SaveState.Commit();   // B05: o save existe em disco antes de Auren abrir (save mais novo: LocalSave recusa e loga)
            rota = Decidir(false, SaveState.Current.birth, SaveState.Current.sceneId, CenasNoBuild(), out cena);
            Seguir();
        }

        static List<string> CenasNoBuild()
        {
            var r = new List<string>();
            string entrada = SceneManager.GetActiveScene().name;
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string n = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                if (n != entrada) r.Add(n);
            }
            return r;
        }

        void Update()
        {
            if (teclado == null) return;
            if (teclado.status != TouchScreenKeyboard.Status.Canceled) nome = teclado.text;
            if (teclado.status != TouchScreenKeyboard.Status.Visible) teclado = null;
        }

        // ---------- desenho (prototipo OnGUI, paisagem, toque) ----------

        void OnGUI()
        {
            if (tela == Tela.Nenhuma) return;
            GUI.depth = -100;   // por cima do PerfHud e de qualquer HUD da cena; recebe o toque primeiro
            UiFundo.MarcarModal();   // HUD de toque e diagnostico somem enquanto a entrada esta na tela
            Estilos();
            if (tela != Tela.Limiar && tela != Tela.Titulo)   // no Limiar e no titulo o fundo e o palco, com o simbolo
            {
                GUI.color = new Color(0.07f, 0.08f, 0.11f);
                GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
                GUI.color = Color.white;
            }

            Rect s = Screen.safeArea;   // origem embaixo; o GUI conta de cima
            float m = alvo * 0.25f;
            Rect a = new Rect(s.x + m, Screen.height - s.yMax + m, s.width - 2f * m, s.height - 2f * m);
            Rect corpo = new Rect(a.x, a.y + alvo * 2f, a.width, a.height - alvo * 3f - m);
            string permanente = T("nascimento.permanente",
                "Destino e origem são permanentes: depois de confirmados, nunca mais podem ser trocados.");

            switch (tela)
            {
                case Tela.Aviso:
                    Titulo(a, T("aviso.save_mais_novo.titulo", "Este save é de uma versão mais nova do jogo"));
                    GUI.Label(corpo, T("aviso.save_mais_novo.texto",
                        "O arquivo fica intacto, mas NADA do que acontecer nesta sessão será gravado. "
                        + "Atualize o jogo para continuar de onde parou."), texto);
                    if (Botao(a, true, T("ui.continuar", "Continuar"))) Titulo();
                    break;

                case Tela.Titulo:
                {
                    float h = a.height;
                    ComSombra(new Rect(a.x, a.y + h * 0.04f, a.width, alvo * 1.3f), T("titulo.nome", "Chronicles of Existence"), tituloGrande);
                    ComSombra(new Rect(a.x, a.y + h * 0.04f + alvo * 1.2f, a.width, alvo * 0.7f), T("titulo.sub", "A Primeira Existência"), subtitulo);
                    float bw = Mathf.Max(alvo * 5f, a.width * 0.34f), bx = a.center.x - bw * 0.5f, by = a.yMax - alvo * (Nasceu ? 2.3f : 1.1f);
                    string principal = Nasceu ? Strings.Format("titulo.continuar", SaveState.Current.birth.characterName) : T("titulo.comecar", "Começar");
                    if (GUI.Button(new Rect(bx, by, bw, alvo), principal, botao))
                    {
                        if (Nasceu && limiar != null) limiar.SetActive(false);
                        Seguir();
                    }
                    if (Nasceu && GUI.Button(new Rect(bx, by + alvo * 1.2f, bw, alvo), T("titulo.nova_vida", "Nova vida"), botao))
                        tela = Tela.NovaVida;
                    break;
                }

                case Tela.NovaVida:
                    Titulo(a, T("titulo.nova_vida.titulo", "Começar uma nova vida?"));
                    GUI.Label(corpo, Strings.Format("titulo.nova_vida.texto", SaveState.Current.birth.characterName), texto);
                    if (Botao(a, false, T("ui.cancelar", "Cancelar"))) tela = Tela.Titulo;
                    if (Botao(a, true, T("titulo.nova_vida.confirmar", "Apagar e começar"))) RecomecarVida();
                    break;

                case Tela.Limiar:
                    // painel de fala nos 40% de baixo; o simbolo fica a vista em cima. Nada avanca sozinho (B01).
                    Rect painel = new Rect(a.x, a.y + a.height * 0.6f, a.width, a.height * 0.4f);
                    GUI.Box(new Rect(painel.x - m, painel.y - m, painel.width + 2f * m, painel.height + m), GUIContent.none, UiEstilo.PainelCache);
                    var f = LimiarRoteiro.Falas[fala];
                    GUI.Label(new Rect(painel.x, painel.y, painel.width, painel.height - alvo - m),
                        "<b>" + Strings.Get(LimiarRoteiro.FalanteKey) + "</b>\n" + Strings.Get(f.FalaKey), texto);
                    if (fala > 0 && Botao(a, false, T("ui.voltar", "Voltar"))) fala = LimiarRoteiro.Voltar(fala);
                    else if (Botao(a, true, Strings.Get(f.RespostaKey)))
                    {
                        fala = LimiarRoteiro.Seguir(fala);
                        if (fala >= LimiarRoteiro.Falas.Length) { limiar.SetActive(false); tela = Tela.Destino; }
                    }
                    break;

                case Tela.Destino:
                    Titulo(a, T("nascimento.destino.titulo", "Antes de nascer: em que vida você chega?"), permanente);
                    int d = Cartoes(corpo, Array.ConvertAll(DestinyCatalog.Destinos, x =>
                        Cartao(x.RotuloKey, x.DescricaoKey, x.FamiliaKey, x.ContextoSocialKey)));
                    if (d >= 0) { destino = DestinyCatalog.Destinos[d].Id; origem = null; tela = Tela.Origem; }
                    break;

                case Tela.Origem:
                    OriginDef[] origens = DestinySystem.OrigensDisponiveis(destino);
                    Titulo(a, Rotulo(destino) + " — " + T("nascimento.origem.titulo", "em que família?"), permanente);
                    int o = Cartoes(corpo, Array.ConvertAll(origens, x => Cartao(x.RotuloKey, x.DescricaoKey, x.OficioKey)));
                    if (o >= 0) { origem = origens[o].Id; erro = ""; tela = Tela.Nome; }
                    if (Botao(a, false, T("ui.voltar", "Voltar"))) { destino = null; tela = Tela.Destino; }
                    break;

                case Tela.Nome:
                    Titulo(a, T("nascimento.nome.titulo", "Como você vai se chamar?"));
                    Rect c = new Rect(a.center.x - a.width * 0.3f, corpo.y + alvo * 0.5f, a.width * 0.6f, alvo);
                    if (!TouchScreenKeyboard.isSupported) nome = GUI.TextField(c, nome ?? "", DestinySystem.NomeMaximo, campo);
                    else if (GUI.Button(c, nome, campo))
                        teclado = TouchScreenKeyboard.Open(nome, TouchScreenKeyboardType.Default, false, false, false, false,
                            "", DestinySystem.NomeMaximo);
                    GUI.Label(new Rect(c.x, c.yMax + m, c.width, alvo * 2f), erro.Length > 0 ? erro
                        : TouchScreenKeyboard.isSupported ? T("nascimento.nome.dica", "Toque no nome para editar.") : "", texto);
                    if (Botao(a, false, T("ui.voltar", "Voltar"))) tela = Tela.Origem;
                    if (Botao(a, true, T("ui.continuar", "Continuar")))
                    {
                        BirthError e = ValidarEscolha(destino, origem, nome);
                        erro = e == BirthError.Nenhum ? "" : Erro(e);
                        if (e == BirthError.Nenhum) tela = Tela.Certeza;
                    }
                    break;

                case Tela.Certeza:
                    Titulo(a, T("nascimento.certeza.titulo", "Tem certeza?"));
                    GUI.Label(corpo, "<b>" + nome + "</b>\n" + Rotulo(destino) + " · " + RotuloOrigem(origem) + "\n\n"
                        + T("nascimento.certeza.texto", "Este é o ponto sem volta: depois de confirmar, nenhuma tela do jogo "
                            + "vai oferecer trocar destino ou origem."), texto);
                    // B05: cancelar volta para a escolha de destino (B02) sem gravar nada.
                    if (Botao(a, false, T("ui.cancelar", "Cancelar"))) { destino = null; origem = null; tela = Tela.Destino; }
                    if (Botao(a, true, T("nascimento.confirmar", "Confirmar nascimento"))) Nascer();
                    break;
            }

            if (Event.current.isMouse) Event.current.Use();   // modal: toque fora dos botoes nao chega a HUD de tras
        }

        void Estilos()
        {
            float novo = Mathf.Max(ControlPreset.DpToPx(ControlPreset.MinTargetDp + 8f, Screen.dpi), Screen.height * 0.12f);
            if (titulo != null && Mathf.Approximately(novo, alvo)) return;   // recalcula so se a tela mudou
            alvo = novo;
            int fonte = Mathf.RoundToInt(alvo * 0.28f);
            int p = Mathf.RoundToInt(alvo * 0.2f);
            titulo = new GUIStyle(GUI.skin.label) { fontSize = Mathf.RoundToInt(fonte * 1.3f), fontStyle = FontStyle.Bold, wordWrap = true };
            titulo.normal.textColor = UiEstilo.Ouro;
            texto = new GUIStyle(GUI.skin.label) { fontSize = fonte, wordWrap = true, richText = true };
            texto.normal.textColor = UiEstilo.Tinta;
            botao = UiEstilo.EstiloBotao(fonte, true);
            cartao = UiEstilo.EstiloBotao(Mathf.RoundToInt(fonte * 0.8f));   // texto dos cartoes cabe inteiro
            cartao.richText = true;
            cartao.alignment = TextAnchor.UpperLeft;
            cartao.padding = new RectOffset(p, p, p, p);
            campo = new GUIStyle(GUI.skin.textField) { fontSize = fonte, alignment = TextAnchor.MiddleCenter };
            tituloGrande = new GUIStyle(titulo) { fontSize = Mathf.RoundToInt(fonte * 2.6f), alignment = TextAnchor.MiddleCenter, wordWrap = false };
            subtitulo = new GUIStyle(texto) { fontSize = Mathf.RoundToInt(fonte * 1.2f), alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.BoldAndItalic };
        }

        /// <summary>Texto com sombra escura deslocada: legivel sobre o simbolo e o ceu do palco.</summary>
        static void ComSombra(Rect r, string t, GUIStyle estilo)
        {
            Color cor = estilo.normal.textColor;
            estilo.normal.textColor = new Color(0f, 0f, 0f, 0.7f);
            float d = Mathf.Max(2f, estilo.fontSize * 0.06f);
            GUI.Label(new Rect(r.x + d, r.y + d, r.width, r.height), t, estilo);
            estilo.normal.textColor = cor;
            GUI.Label(r, t, estilo);
        }

        void Titulo(Rect a, string t, string sub = null)
        {
            GUI.Label(new Rect(a.x, a.y, a.width, alvo), t, titulo);
            if (sub != null) GUI.Label(new Rect(a.x, a.y + alvo, a.width, alvo * 0.9f), sub, texto);
        }

        /// <summary>Botao da linha de baixo: esquerda = voltar/cancelar, direita = seguir. Altura = alvo (>= 48 dp).</summary>
        bool Botao(Rect a, bool direita, string rotulo)
        {
            float w = Mathf.Max(alvo * 3.5f, a.width * 0.3f);
            return GUI.Button(new Rect(direita ? a.xMax - w : a.x, a.yMax - alvo, w, alvo), rotulo, botao);
        }

        /// <summary>Cartoes lado a lado (paisagem), cada um um botao inteiro. Devolve o tocado, ou -1.</summary>
        int Cartoes(Rect area, string[] textos)
        {
            float gap = alvo * 0.2f, w = (area.width - gap * (textos.Length - 1)) / textos.Length;
            int tocado = -1;
            for (int i = 0; i < textos.Length; i++)
                if (GUI.Button(new Rect(area.x + i * (w + gap), area.y, w, area.height), textos[i], cartao)) tocado = i;
            return tocado;
        }

        /// <summary>Rotulo em negrito e o resto em paragrafos. Circunstancia de vida (B02), nunca nivel de desafio (ADR-0004).</summary>
        static string Cartao(string rotuloKey, params string[] chaves)
        {
            string s = "<b>" + Strings.Get(rotuloKey) + "</b>";
            foreach (string k in chaves) s += "\n\n" + Strings.Get(k);
            return s;
        }

        static string Rotulo(string destinyId)
        {
            DestinyDef d = DestinyCatalog.Destino(destinyId);
            return d != null ? Strings.Get(d.RotuloKey) : "";
        }

        static string RotuloOrigem(string originId)
        {
            OriginDef o = DestinyCatalog.Origem(originId);
            return o != null ? Strings.Get(o.RotuloKey) : "";
        }

        static string Erro(BirthError e)
        {
            switch (e)
            {
                case BirthError.NomeCurto:
                    return T("nascimento.erro.nome_curto", "O nome precisa de pelo menos " + DestinySystem.NomeMinimo + " letras.");
                case BirthError.NomeLongo:
                    return T("nascimento.erro.nome_longo", "O nome pode ter no máximo " + DestinySystem.NomeMaximo + " letras.");
                case BirthError.NomeCaractereInvalido:
                    return T("nascimento.erro.nome_caractere", "Use só letras; espaço, hífen e apóstrofo só entre duas letras.");
                default:
                    return T("nascimento.erro.escolha", "Não deu para confirmar esta escolha.") + " (" + e + ")";
            }
        }

        /// <summary>Texto da tela por chave. Sem strings.pt-BR.json (conteudo da T012) cai no padrao pt-BR em vez de
        /// "[chave]": o aviso de save e o "e permanente" (B02/B05) precisam ser lidos. Rotulos do catalogo usam
        /// Strings.Get direto e aparecem como [chave] ate o arquivo existir.
        /// ponytail: padrao no codigo; sai quando o arquivo de textos tiver estas chaves.</summary>
        static string T(string chave, string padrao)
        {
            string s = Strings.Get(chave);
            return s == "[" + chave + "]" ? padrao : s;
        }
    }
}
