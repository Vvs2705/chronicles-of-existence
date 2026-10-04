using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    /// Desenho em uGUI (Bloco D, 2026-10-04): so o desenho mudou; Decidir, ValidarEscolha e DestinySystem ficam.</summary>
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
        /// sceneId -> cena pelo CenaCatalogo (id snake_case, ou o nome antigo que o jogo gravava); id fora da tabela ainda casa
        /// com uma cena de mesmo nome sem caixa. Fora do Build Settings = CenaInicial.</summary>
        public static Rota Decidir(bool temSceneArg, BirthChoice birth, string sceneId, IList<string> cenasNoBuild, out string cena)
        {
            cena = null;
            if (temSceneArg) return Rota.Nenhuma;
            if (!DestinySystem.EstaConfirmada(birth)) return Rota.Nascimento;
            cena = CenaInicial;
            string alvo = CenaCatalogo.Nome(sceneId) ?? sceneId;
            if (!string.IsNullOrEmpty(alvo) && cenasNoBuild != null)
                foreach (string c in cenasNoBuild)
                    if (string.Equals(c, alvo, StringComparison.OrdinalIgnoreCase)) cena = c;
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

        enum Passo { Nenhuma, Aviso, Titulo, NovaVida, Limiar, Destino, Origem, Nome, Certeza }

        Passo tela;
        Rota rota;
        string cena, destino, origem, erro = "";
        string nome = NomePadrao;
        int fala;   // indice em LimiarRoteiro.Falas

        void Start()
        {
            // SaveBootstrap (-200) ja carregou o save no Awake; DevSceneArg (AfterSceneLoad) ja pediu a cena do -scene.
            rota = Decidir(DevSceneArg.Tem("-scene"), SaveState.Current.birth, SaveState.Current.sceneId, CenasNoBuild(), out cena);
            if (rota == Rota.Nenhuma) { enabled = false; return; }
            StringsLoader.EnsureLoaded();
            if (input != null) input.enabled = false;   // a Bootstrap some no LoadScene; nao precisa religar
            if (SaveMaisNovo(LocalSave.DefaultPath)) tela = Passo.Aviso;
            else Titulo();
        }

        /// <summary>Tela de titulo (2026-10-02): o jogo abre no nome dele, com o simbolo do Limiar ao fundo, e o jogador
        /// escolhe Comecar/Continuar ou Nova vida. Antes abria direto no Limiar ou em Auren, sem como recomecar.</summary>
        void Titulo()
        {
            if (limiar != null) limiar.SetActive(true);
            tela = Passo.Titulo;
        }

        bool Nasceu { get { return !string.IsNullOrEmpty(SaveState.Current.birth.destinyId); } }

        /// <summary>Alguma tela da entrada (aviso, titulo, nascimento) esta aberta.</summary>
        public bool Aberta { get { return tela != Passo.Nenhuma; } }

        /// <summary>A tela atual tem botao "Voltar"/"Cancelar"? false = raiz (aviso, titulo, primeira fala do Limiar,
        /// destino): o voltar do Android pede confirmacao para sair (VoltarHud).</summary>
        public bool PodeVoltar
        {
            get { return tela == Passo.NovaVida || tela == Passo.Origem || tela == Passo.Nome || tela == Passo.Certeza || (tela == Passo.Limiar && fala > 0); }
        }

        /// <summary>O botao "Voltar"/"Cancelar" da tela; o voltar do Android chama o mesmo. Na raiz nao faz nada.</summary>
        public void Voltar()
        {
            switch (tela)
            {
                case Passo.NovaVida: tela = Passo.Titulo; break;
                case Passo.Limiar: if (fala > 0) fala = LimiarRoteiro.Voltar(fala); break;
                case Passo.Origem: destino = null; tela = Passo.Destino; break;
                case Passo.Nome: tela = Passo.Origem; break;
                case Passo.Certeza: destino = null; origem = null; tela = Passo.Destino; break;   // B05: volta ao destino sem gravar
            }
        }

        /// <summary>Nova vida confirmada: save em branco gravado por cima (o anterior fica no .bak do LocalSave) e o
        /// nascimento recomeca pelo Limiar.</summary>
        void RecomecarVida()
        {
            SaveState.NovaVida();
            destino = null; origem = null; nome = NomePadrao; erro = "";
            rota = Rota.Nascimento;
            Seguir();
        }

        void Seguir()
        {
            if (rota == Rota.Nascimento)
            {
                if (limiar == null) { tela = Passo.Destino; return; }
                limiar.SetActive(true);
                fala = 0;
                tela = Passo.Limiar;
                return;
            }
            tela = Passo.Nenhuma;
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
            // B05: nascimento e inventario inicial numa gravacao so, pela sessao; o save existe em disco antes de Auren abrir
            // (save mais novo: LocalSave recusa e loga). Recusa nao muda nem grava.
            BirthResult r = SaveState.Sessao.Nascer(destino, origem, nome);
            if (!r.Ok) { erro = Erro(r.Erro); tela = Passo.Nome; return; }
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

        // ---------- desenho (uGUI, Bloco D; paisagem, toque) ----------
        // A vista e montada uma vez e so muda quando a tela, a area segura ou o tamanho mudam. Mostrar() le o estado
        // (tela, destino, origem, fala, erro) e liga/desliga/escreve; nenhum texto e montado por quadro.

        Canvas canvas;
        Image fundo, painelFala;
        RectTransform area;
        Text titulo, sub, tituloGrande, subtitulo, corpo, textoFala, dica;
        Button botaoEsq, botaoDir, botaoPrincipal, botaoNovaVida;
        Button[] cartoes;   // tantos quanto o maior catalogo (4 destinos; 3 origens por destino)
        InputField campoNome;
        Passo telaMostrada = (Passo)(-1);
        int falaMostrada = -1;
        Rect safeDisposto;
        int alturaDisposta;
        float alvo;   // altura de botao em px: >= 56 dp e proporcional a tela

        /// <summary>A tela da entrada esta desenhada (teste e robo leem).</summary>
        public Canvas Vista { get { return canvas; } }

        void Update()
        {
            if (tela == Passo.Nenhuma)
            {
                if (canvas != null && canvas.enabled) canvas.enabled = false;
                return;
            }
            UiFundo.MarcarModal();   // HUD de toque e diagnostico somem enquanto a entrada esta na tela
            if (canvas == null) Montar();
            if (!canvas.enabled) canvas.enabled = true;
            if (Screen.safeArea != safeDisposto || Screen.height != alturaDisposta) { Dispor(); telaMostrada = (Passo)(-1); }
            if (tela != telaMostrada || fala != falaMostrada) Mostrar();   // reler a fala do Limiar muda so a fala
        }

        void Montar()
        {
            canvas = Tela.NovoCanvas(transform, "EntradaCanvas", Tela.CamadaEntrada);
            fundo = Tela.Imagem(canvas.transform, "Fundo", null, new Color(0.07f, 0.08f, 0.11f));
            fundo.raycastTarget = true;   // modal: toque fora dos botoes nao chega a nada atras
            Tela.Esticar(fundo.rectTransform, 0f);
            area = Tela.Filho(canvas.transform, "Area");
            titulo = Tela.Texto(area, "Titulo", 20, TextAnchor.UpperLeft, UiEstilo.Ouro);
            titulo.fontStyle = FontStyle.Bold;
            sub = Tela.Texto(area, "Sub", 16, TextAnchor.UpperLeft, UiEstilo.Tinta);
            tituloGrande = ComSombra(Tela.Texto(area, "TituloGrande", 40, TextAnchor.MiddleCenter, UiEstilo.Ouro));
            tituloGrande.fontStyle = FontStyle.Bold;
            tituloGrande.horizontalOverflow = HorizontalWrapMode.Overflow;
            subtitulo = ComSombra(Tela.Texto(area, "Subtitulo", 20, TextAnchor.MiddleCenter, UiEstilo.Tinta));
            subtitulo.fontStyle = FontStyle.BoldAndItalic;
            corpo = Tela.Texto(area, "Corpo", 16, TextAnchor.UpperLeft, UiEstilo.Tinta);
            corpo.supportRichText = true;
            painelFala = Tela.Imagem(area, "PainelFala", Tela.SpritePainel, Color.white);
            textoFala = Tela.Texto(painelFala.transform, "Fala", 16, TextAnchor.UpperLeft, UiEstilo.Tinta);
            textoFala.supportRichText = true;
            int maximo = DestinyCatalog.Destinos.Length;
            foreach (DestinyDef d in DestinyCatalog.Destinos) maximo = Mathf.Max(maximo, DestinySystem.OrigensDisponiveis(d.Id).Length);
            cartoes = new Button[maximo];
            for (int i = 0; i < cartoes.Length; i++)
            {
                int indice = i;
                cartoes[i] = Tela.Botao(area, "Cartao" + i, 16, delegate { Escolher(indice); });
                Text t = Tela.Rotulo(cartoes[i]);
                t.alignment = TextAnchor.UpperLeft;
                t.fontStyle = FontStyle.Normal;
                t.supportRichText = true;
                t.resizeTextForBestFit = true;   // descricao longa cabe no cartao em tela pequena
            }
            campoNome = Campo(area);
            dica = Tela.Texto(area, "Dica", 16, TextAnchor.UpperCenter, UiEstilo.Tinta);
            botaoEsq = Tela.Botao(area, "BotaoEsquerda", 16, Voltar);
            botaoDir = Tela.Botao(area, "BotaoDireita", 16, Seguinte);
            botaoPrincipal = Tela.Botao(area, "BotaoPrincipal", 16, Principal);
            botaoNovaVida = Tela.Botao(area, "BotaoNovaVida", 16, delegate { tela = Passo.NovaVida; });
            Dispor();
        }

        /// <summary>Geometria pela area segura (mesmas proporcoes do prototipo): titulo em cima, corpo no meio, a linha de
        /// botoes embaixo (esquerda = voltar/cancelar, direita = seguir), alvo >= 56 dp.</summary>
        void Dispor()
        {
            safeDisposto = Screen.safeArea;
            alturaDisposta = Screen.height;
            alvo = Mathf.Max(ControlPreset.DpToPx(ControlPreset.MinTargetDp + 8f, Tela.Dpi), Screen.height * 0.12f);
            float m = alvo * 0.25f;
            Rect a = new Rect(safeDisposto.x + m, safeDisposto.y + m, safeDisposto.width - 2f * m, safeDisposto.height - 2f * m);
            Tela.Colocar(area, a);
            a.position = Vector2.zero;   // daqui para baixo, relativo a area (origem embaixo)
            int fonte = Mathf.RoundToInt(alvo * 0.28f);

            Tela.Colocar(titulo.rectTransform, new Rect(0f, a.height - alvo, a.width, alvo));
            titulo.fontSize = Mathf.RoundToInt(fonte * 1.3f);
            Tela.Colocar(sub.rectTransform, new Rect(0f, a.height - alvo * 1.9f, a.width, alvo * 0.9f));
            sub.fontSize = fonte;
            Rect corpoR = new Rect(0f, alvo + m, a.width, a.height - alvo * 3f - m);
            Tela.Colocar(corpo.rectTransform, corpoR);
            corpo.fontSize = fonte;

            float yGrande = a.height * 0.96f - alvo * 1.3f;
            Tela.Colocar(tituloGrande.rectTransform, new Rect(0f, yGrande, a.width, alvo * 1.3f));
            tituloGrande.fontSize = Mathf.RoundToInt(fonte * 2.6f);
            Tela.Colocar(subtitulo.rectTransform, new Rect(0f, yGrande - alvo * 0.6f, a.width, alvo * 0.7f));
            subtitulo.fontSize = Mathf.RoundToInt(fonte * 1.2f);

            // Limiar: painel de fala nos 40% de baixo; o simbolo fica a vista em cima.
            Tela.Colocar(painelFala.rectTransform, new Rect(-m, 0f, a.width + 2f * m, a.height * 0.4f + m));
            Tela.Colocar(textoFala.rectTransform, new Rect(m * 2f, alvo + m, a.width - 2f * m, a.height * 0.4f - alvo - m));
            textoFala.fontSize = fonte;

            float gap = alvo * 0.2f;
            for (int i = 0; i < cartoes.Length; i++) Tela.Rotulo(cartoes[i]).resizeTextMaxSize = Mathf.RoundToInt(fonte * 0.8f);
            DisporCartoes(corpoR, cartoes.Length, gap, fonte);

            Rect c = new Rect(a.width * 0.2f, corpoR.yMax - alvo * 1.5f, a.width * 0.6f, alvo);
            Tela.Colocar((RectTransform)campoNome.transform, c);
            campoNome.textComponent.fontSize = fonte;
            Tela.Colocar(dica.rectTransform, new Rect(c.x, c.y - m - alvo * 2f, c.width, alvo * 2f));
            dica.fontSize = fonte;

            float w = Mathf.Max(alvo * 3.5f, a.width * 0.3f);
            Tela.Colocar(botaoEsq.GetComponent<RectTransform>(), new Rect(0f, 0f, w, alvo));
            Tela.Colocar(botaoDir.GetComponent<RectTransform>(), new Rect(a.width - w, 0f, w, alvo));
            float bw = Mathf.Max(alvo * 5f, a.width * 0.34f);
            Tela.Colocar(botaoNovaVida.GetComponent<RectTransform>(), new Rect(a.width * 0.5f - bw * 0.5f, alvo * 0.1f, bw, alvo));
            foreach (Button b in new[] { botaoEsq, botaoDir, botaoPrincipal, botaoNovaVida }) Tela.Rotulo(b).fontSize = fonte;
        }

        /// <summary>n cartoes lado a lado (paisagem), cada um um botao inteiro; os de sobra somem.</summary>
        void DisporCartoes(Rect area, int n, float gap, int fonte)
        {
            float w = (area.width - gap * (n - 1)) / n;
            for (int i = 0; i < cartoes.Length; i++)
            {
                Tela.Colocar(cartoes[i].GetComponent<RectTransform>(), new Rect(area.x + i * (w + gap), area.y, w, area.height));
                Text t = Tela.Rotulo(cartoes[i]);
                Tela.Esticar(t.rectTransform, fonte * 0.6f);
                t.resizeTextMinSize = Mathf.Max(10, Mathf.RoundToInt(fonte * 0.5f));
            }
        }

        /// <summary>Liga, desliga e escreve o que a tela atual mostra. Chamado quando a tela muda, nunca por quadro.</summary>
        void Mostrar()
        {
            telaMostrada = tela;
            falaMostrada = fala;
            bool palco = tela == Passo.Limiar || tela == Passo.Titulo;   // no Limiar e no titulo o fundo e o palco, com o simbolo
            fundo.color = palco ? Color.clear : new Color(0.07f, 0.08f, 0.11f);
            foreach (Component c in new Component[] { titulo, sub, tituloGrande, subtitulo, corpo, painelFala, campoNome, dica,
                                                      botaoEsq, botaoDir, botaoPrincipal, botaoNovaVida })
                c.gameObject.SetActive(false);
            foreach (Button b in cartoes) b.gameObject.SetActive(false);
            string permanente = T("nascimento.permanente",
                "Destino e origem são permanentes: depois de confirmados, nunca mais podem ser trocados.");

            switch (tela)
            {
                case Passo.Aviso:
                    Escrever(titulo, T("aviso.save_mais_novo.titulo", "Este save é de uma versão mais nova do jogo"));
                    Escrever(corpo, T("aviso.save_mais_novo.texto", "O arquivo fica intacto, mas NADA do que acontecer nesta "
                        + "sessão será gravado. Atualize o jogo para continuar de onde parou."));
                    Rotular(botaoDir, T("ui.continuar", "Continuar"));
                    break;

                case Passo.Titulo:
                    Escrever(tituloGrande, T("titulo.nome", "Chronicles of Existence"));
                    Escrever(subtitulo, T("titulo.sub", "A Primeira Existência"));
                    Rotular(botaoPrincipal, Nasceu ? Strings.Format("titulo.continuar", SaveState.Current.birth.characterName) : T("titulo.comecar", "Começar"));
                    float bw = Mathf.Max(alvo * 5f, area.rect.width * 0.34f);
                    Tela.Colocar(botaoPrincipal.GetComponent<RectTransform>(),
                        new Rect(area.rect.width * 0.5f - bw * 0.5f, alvo * (Nasceu ? 1.3f : 0.1f), bw, alvo));
                    if (Nasceu) Rotular(botaoNovaVida, T("titulo.nova_vida", "Nova vida"));
                    break;

                case Passo.NovaVida:
                    Escrever(titulo, T("titulo.nova_vida.titulo", "Começar uma nova vida?"));
                    Escrever(corpo, Strings.Format("titulo.nova_vida.texto", SaveState.Current.birth.characterName));
                    Rotular(botaoEsq, T("ui.cancelar", "Cancelar"));
                    Rotular(botaoDir, T("titulo.nova_vida.confirmar", "Apagar e começar"));
                    break;

                case Passo.Limiar:
                    painelFala.gameObject.SetActive(true);
                    var f = LimiarRoteiro.Falas[fala];
                    textoFala.text = "<b>" + Strings.Get(LimiarRoteiro.FalanteKey) + "</b>\n" + Strings.Get(f.FalaKey);
                    if (fala > 0) Rotular(botaoEsq, T("ui.voltar", "Voltar"));
                    Rotular(botaoDir, Strings.Get(f.RespostaKey));
                    break;

                case Passo.Destino:
                    Escrever(titulo, T("nascimento.destino.titulo", "Antes de nascer: em que vida você chega?"));
                    Escrever(sub, permanente);
                    for (int i = 0; i < DestinyCatalog.Destinos.Length && i < cartoes.Length; i++)
                    {
                        DestinyDef x = DestinyCatalog.Destinos[i];
                        Rotular(cartoes[i], Cartao(x.RotuloKey, x.DescricaoKey, x.FamiliaKey, x.ContextoSocialKey));
                    }
                    DisporCartoes(CorpoRect(), DestinyCatalog.Destinos.Length,
                        alvo * 0.2f, Mathf.RoundToInt(alvo * 0.28f));
                    break;

                case Passo.Origem:
                    OriginDef[] origens = DestinySystem.OrigensDisponiveis(destino);
                    Escrever(titulo, Rotulo(destino) + " — " + T("nascimento.origem.titulo", "em que família?"));
                    Escrever(sub, permanente);
                    for (int i = 0; i < origens.Length && i < cartoes.Length; i++)
                        Rotular(cartoes[i], Cartao(origens[i].RotuloKey, origens[i].DescricaoKey, origens[i].OficioKey));
                    DisporCartoes(CorpoRect(), origens.Length, alvo * 0.2f, Mathf.RoundToInt(alvo * 0.28f));
                    Rotular(botaoEsq, T("ui.voltar", "Voltar"));
                    break;

                case Passo.Nome:
                    Escrever(titulo, T("nascimento.nome.titulo", "Como você vai se chamar?"));
                    campoNome.gameObject.SetActive(true);
                    campoNome.SetTextWithoutNotify(nome ?? "");
                    Escrever(dica, erro.Length > 0 ? erro : TouchScreenKeyboard.isSupported ? T("nascimento.nome.dica", "Toque no nome para editar.") : "");
                    Rotular(botaoEsq, T("ui.voltar", "Voltar"));
                    Rotular(botaoDir, T("ui.continuar", "Continuar"));
                    break;

                case Passo.Certeza:
                    Escrever(titulo, T("nascimento.certeza.titulo", "Tem certeza?"));
                    Escrever(corpo, "<b>" + nome + "</b>\n" + Rotulo(destino) + " · " + RotuloOrigem(origem) + "\n\n"
                        + T("nascimento.certeza.texto", "Este é o ponto sem volta: depois de confirmar, nenhuma tela do jogo "
                            + "vai oferecer trocar destino ou origem."));
                    // B05: cancelar volta para a escolha de destino (B02) sem gravar nada.
                    Rotular(botaoEsq, T("ui.cancelar", "Cancelar"));
                    Rotular(botaoDir, T("nascimento.confirmar", "Confirmar nascimento"));
                    break;
            }
        }

        Rect CorpoRect()
        {
            RectTransform c = corpo.rectTransform;
            return new Rect(c.anchoredPosition, c.sizeDelta);
        }

        /// <summary>Botao da direita: o "seguir" de cada tela.</summary>
        void Seguinte()
        {
            switch (tela)
            {
                case Passo.Aviso: Titulo(); break;
                case Passo.NovaVida: RecomecarVida(); break;
                case Passo.Limiar:
                    fala = LimiarRoteiro.Seguir(fala);
                    if (fala >= LimiarRoteiro.Falas.Length) { if (limiar != null) limiar.SetActive(false); tela = Passo.Destino; }
                    break;
                case Passo.Nome:
                    BirthError e = ValidarEscolha(destino, origem, nome);
                    erro = e == BirthError.Nenhum ? "" : Erro(e);
                    if (e == BirthError.Nenhum) tela = Passo.Certeza;
                    else telaMostrada = (Passo)(-1);   // mesma tela, com o erro
                    break;
                case Passo.Certeza: Nascer(); break;
            }
        }

        /// <summary>Botao principal do titulo: Comecar (sem destino) ou Continuar como &lt;nome&gt;.</summary>
        void Principal()
        {
            if (Nasceu && limiar != null) limiar.SetActive(false);
            Seguir();
        }

        void Escolher(int i)
        {
            if (tela == Passo.Destino && i < DestinyCatalog.Destinos.Length)
            {
                destino = DestinyCatalog.Destinos[i].Id; origem = null; tela = Passo.Origem;
            }
            else if (tela == Passo.Origem)
            {
                OriginDef[] origens = DestinySystem.OrigensDisponiveis(destino);
                if (i < origens.Length) { origem = origens[i].Id; erro = ""; tela = Passo.Nome; }
            }
        }

        static void Escrever(Text t, string s) { t.text = s; t.gameObject.SetActive(true); }

        static void Rotular(Button b, string s) { Tela.Rotulo(b).text = s; b.gameObject.SetActive(true); }

        /// <summary>Sombra escura deslocada: legivel sobre o simbolo e o ceu do palco.</summary>
        static Text ComSombra(Text t)
        {
            var s = t.gameObject.AddComponent<Shadow>();
            s.effectColor = new Color(0f, 0f, 0f, 0.7f);
            s.effectDistance = new Vector2(3f, -3f);
            return t;
        }

        /// <summary>Campo do nome. No celular o InputField abre o teclado do sistema sozinho; limite = NomeMaximo.</summary>
        InputField Campo(Transform pai)
        {
            Image fundoCampo = Tela.Imagem(pai, "CampoNome", Tela.SpriteBotao, Color.white);
            fundoCampo.raycastTarget = true;
            var campo = fundoCampo.gameObject.AddComponent<InputField>();
            Text t = Tela.Texto(fundoCampo.transform, "Texto", 16, TextAnchor.MiddleCenter, UiEstilo.Tinta);
            t.supportRichText = false;
            Tela.Esticar(t.rectTransform, 8f);
            campo.textComponent = t;
            campo.targetGraphic = fundoCampo;
            campo.characterLimit = DestinySystem.NomeMaximo;
            campo.lineType = InputField.LineType.SingleLine;
            // Fechar o teclado cancelando (voltar do Android) faz o InputField voltar ao texto original: o nome digitado fica
            // (como no IMGUI de antes) e o campo volta a mostra-lo.
            campo.onValueChanged.AddListener(delegate (string v) { if (!campo.wasCanceled) nome = v; });
            campo.onEndEdit.AddListener(delegate { if (campo.wasCanceled) campo.SetTextWithoutNotify(nome ?? ""); });
            return campo;
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
