using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace COE
{
    /// <summary>Onde as configuracoes do jogador moram. FORA do save de progresso (SaveData): trocar de mao nao e
    /// progresso, nao entra no versionamento do save e nao some com um save novo. Ler devolve null se a chave falta.</summary>
    public interface IConfigArmazenamento
    {
        string Ler(string chave);
        void Gravar(string chave, string valor);
    }

    /// <summary>O do jogo: PlayerPrefs (Android: SharedPreferences do app). Save() a cada mudanca: o Android mata o app
    /// sem OnApplicationQuit, e a mudanca so acontece num toque do menu.</summary>
    public sealed class ConfigPlayerPrefs : IConfigArmazenamento
    {
        public string Ler(string chave) { return PlayerPrefs.HasKey(chave) ? PlayerPrefs.GetString(chave) : null; }
        public void Gravar(string chave, string valor) { PlayerPrefs.SetString(chave, valor); PlayerPrefs.Save(); }
    }

    /// <summary>Em memoria: testes (nao sujam o PlayerPrefs da maquina).</summary>
    public sealed class ConfigEmMemoria : IConfigArmazenamento
    {
        public readonly Dictionary<string, string> Valores = new Dictionary<string, string>();
        public string Ler(string chave) { string v; return Valores.TryGetValue(chave, out v) ? v : null; }
        public void Gravar(string chave, string valor) { Valores[chave] = valor; }
    }

    /// <summary>Configuracoes do jogador (menu de pausa): mao do layout de toque, sensibilidade do olhar, FPS alvo e
    /// HUD de desempenho. C# puro: le e valida no construtor, cada Definir grava so a propria chave.
    /// CHAVES estaveis e versionadas (prefixo coe.cfg.v1.): chave publicada nao muda; formato novo = prefixo v2 com
    /// migracao. Valor ausente, ilegivel ou fora da faixa cai no padrao (nunca lanca): PlayerPrefs e editavel a mao.
    /// FPS: 30 e a meta de celular do docs/arte/PIPELINE.md §4.1 (HIPOTESE v0; aparelho minimo pendente no ADR-0006).
    /// ponytail: 60 e opcao do jogador, sem checar se o aparelho sustenta (fps_min_1s do PerfHud mede); travar a opcao
    /// por aparelho quando existir a lista de aparelhos suportados.</summary>
    public sealed class Configuracoes
    {
        public const string Prefixo = "coe.cfg.v1.";
        public const string ChaveMao = Prefixo + "mao";                       // "destro" | "canhoto"
        public const string ChaveSensibilidade = Prefixo + "sensibilidade";   // "0.5".."2.0", ponto decimal
        public const string ChaveFps = Prefixo + "fps";                       // "30" | "60"
        public const string ChaveDesempenho = Prefixo + "desempenho";         // "1" | "0"
        public const string ChaveQualidade = Prefixo + "qualidade";           // "auto" | "baixa" | "media" | "alta"
        public const string ChaveSom = Prefixo + "som";                       // "1" | "0" (ausente = ligado)

        /// <summary>Multiplicador do olhar sobre a calibragem do preset/dispositivo (1 = sem mudanca).</summary>
        public const float SensibilidadeMin = 0.5f, SensibilidadeMax = 2f, SensibilidadePasso = 0.1f, SensibilidadePadrao = 1f;
        public const int FpsPadrao = 30, FpsAlto = 60;

        readonly IConfigArmazenamento armazenamento;

        public HandPreset Mao { get; private set; }
        public float Sensibilidade { get; private set; }
        public int Fps { get; private set; }
        public bool MostrarDesempenho { get; private set; }
        /// <summary>Musica e efeitos (SomDoJogo). Ligado por padrao; so "0" desliga.</summary>
        public bool Som { get; private set; }
        /// <summary>Faixa grafica escolhida pelo jogador; null = automatica (pela RAM do aparelho, ADR-0009).</summary>
        public FaixaQualidade? QualidadeEscolhida { get; private set; }

        /// <param name="desenvolvimento">Padrao do HUD de desempenho: ligado em build de desenvolvimento
        /// (Debug.isDebugBuild), desligado fora.</param>
        public Configuracoes(IConfigArmazenamento armazenamento, bool desenvolvimento)
        {
            this.armazenamento = armazenamento;
            Mao = armazenamento.Ler(ChaveMao) == "canhoto" ? HandPreset.Canhoto : HandPreset.Destro;

            float s;
            bool legivel = float.TryParse(armazenamento.Ler(ChaveSensibilidade), NumberStyles.Float, CultureInfo.InvariantCulture, out s);
            Sensibilidade = legivel && s >= SensibilidadeMin && s <= SensibilidadeMax ? NoPasso(s) : SensibilidadePadrao; // NaN falha as duas comparacoes

            Fps = armazenamento.Ler(ChaveFps) == "60" ? FpsAlto : FpsPadrao;

            string d = armazenamento.Ler(ChaveDesempenho);
            MostrarDesempenho = d == "1" || (d != "0" && desenvolvimento);

            Som = armazenamento.Ler(ChaveSom) != "0";

            FaixaQualidade q;
            QualidadeEscolhida = Qualidade.TryParse(armazenamento.Ler(ChaveQualidade), out q) ? q : (FaixaQualidade?)null;
        }

        /// <summary>A faixa que vale: a escolhida, ou a detectada pela RAM (MB) quando automatica.</summary>
        public FaixaQualidade QualidadeEfetiva(int ramMb)
        {
            return QualidadeEscolhida ?? Qualidade.Detectar(ramMb);
        }

        /// <summary>null = automatica.</summary>
        public void DefinirQualidade(FaixaQualidade? faixa)
        {
            QualidadeEscolhida = faixa;
            armazenamento.Gravar(ChaveQualidade, faixa.HasValue ? Qualidade.Id(faixa.Value) : "auto");
        }

        public void DefinirMao(HandPreset mao)
        {
            Mao = mao;
            armazenamento.Gravar(ChaveMao, mao == HandPreset.Canhoto ? "canhoto" : "destro");
        }

        /// <summary>Anda "passos" de SensibilidadePasso (negativo diminui), presa na faixa.</summary>
        public void MudarSensibilidade(int passos)
        {
            Sensibilidade = Mathf.Clamp(NoPasso(Sensibilidade + passos * SensibilidadePasso), SensibilidadeMin, SensibilidadeMax);
            armazenamento.Gravar(ChaveSensibilidade, Sensibilidade.ToString("0.0", CultureInfo.InvariantCulture));
        }

        /// <summary>So 30 ou 60; qualquer outro valor vira 30.</summary>
        public void DefinirFps(int fps)
        {
            Fps = fps == FpsAlto ? FpsAlto : FpsPadrao;
            armazenamento.Gravar(ChaveFps, Fps == FpsAlto ? "60" : "30");
        }

        public void DefinirSom(bool ligado)
        {
            Som = ligado;
            armazenamento.Gravar(ChaveSom, ligado ? "1" : "0");
        }

        public void DefinirMostrarDesempenho(bool mostrar)
        {
            MostrarDesempenho = mostrar;
            armazenamento.Gravar(ChaveDesempenho, mostrar ? "1" : "0");
        }

        // Prende no passo: somar 0,1 repetidas vezes em float derivaria para fora da faixa.
        static float NoPasso(float s) { return Mathf.Round(s / SensibilidadePasso) * SensibilidadePasso; }
    }
}
