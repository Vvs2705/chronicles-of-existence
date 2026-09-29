using System;
using System.IO;
using UnityEngine;

namespace COE
{
    /// <summary>Camada de disco do save. Arquivos ao lado do save.json:
    ///   save.json            — o save corrente
    ///   save.json.bak        — a gravacao anterior, feita automaticamente a cada Save (dossie §K: backup)
    ///   save.json.rejeitado  — arquivo que nao deu para ler; guardado, nunca apagado em silencio
    ///   save.json.v&lt;N&gt;       — copia do original de versao N, feita antes de migrar (PreMigrationPath)
    ///
    /// Leitura: tenta o principal; se ele nao servir, tenta o .bak; se nenhum servir, devolve save padrao.
    /// Nunca lanca para quem chama e nunca "conserta" conteudo: ou o save e valido inteiro, ou e descartado inteiro.
    ///
    /// VERSAO, pelo cabecalho (VersionOf):
    /// - ilegivel (-1)          -> .rejeitado e tenta o .bak.
    /// - anterior (0..atual-1)  -> MIGRA pela cadeia v -> v+1 (Migracoes) e guarda copia do original. Nao grava
    ///                             nada no Load: o arquivo so vira versao atual na proxima gravacao.
    /// - atual                  -> le direto.
    /// - MAIS NOVA que a build  -> jogo velho lendo save do jogo novo. O arquivo fica intacto no lugar (nao e
    ///                             defeito, e o futuro), o .bak nao e usado (progresso velho que tambem nao poderia
    ///                             ser gravado), Load devolve save padrao em memoria e Save RECUSA gravar por cima.
    ///
    /// Gravacao atomica: escreve save.json.tmp e troca por cima do save.json, jogando o anterior no .bak.</summary>
    public static class LocalSave
    {
        public const string FileName = "save.json";

        public static string DefaultPath { get { return Path.Combine(Application.persistentDataPath, FileName); } }

        public static string BackupPath(string path) { return path + ".bak"; }
        public static string RejectedPath(string path) { return path + ".rejeitado"; }
        public static string PreMigrationPath(string path, int versao) { return path + ".v" + versao; }

        /// <summary>-1 em VersionOf: nao e objeto JSON (vazio, truncado, lixo) ou tem saveVersion negativo.</summary>
        public const int Ilegivel = -1;

        /// <summary>Cadeia de migracao: Migracoes[v] recebe o JSON gravado na versao v e devolve o JSON da versao v+1.
        /// FromJson encadeia do que achou ate SchemaVersion (um v1 lido pela build v3 passa por [1] e [2]).
        /// REGRA para o proximo passo: leia a versao de origem com um DTO CONGELADO dela (classe privada aqui, como
        /// SaveHeader), nunca com SaveData — SaveData muda depois, o passo antigo nao pode mudar junto.
        /// Subir SchemaVersion sem acrescentar o passo deixa SaveDataTests.Migracao_TodaVersaoAnteriorTemPasso vermelho.</summary>
        static readonly Func<string, string>[] Migracoes =
        {
            // v0 -> v1. v0 = arquivo do prototipo anterior a T004 ({"versao":1,"volume":0.4}), sem saveVersion. Nao
            // tem destino, origem, nome nem nada do personagem; levar campo por coincidencia de nome seria inventar o
            // que o jogador nunca escolheu. Sai o v1 neutro: birth vazio reabre a tela de nascimento, e o original
            // fica copiado em PreMigrationPath(path, 0).
            v0 => "{\"saveVersion\":1}",
        };

        public static SaveData Load() { return Load(DefaultPath); }

        public static SaveData Load(string path)
        {
            bool maisNovo;
            SaveData d = ReadOrQuarantine(path, out maisNovo);
            if (d != null) return Auditar(d, path);
            if (maisNovo) return new SaveData();   // ver cabecalho: nem o .bak, nem gravar por cima (Save)

            d = ReadOrQuarantine(BackupPath(path), out maisNovo);
            if (d != null)
            {
                Debug.LogWarning("LocalSave: " + path + " nao servia; recuperado de " + BackupPath(path) + ".");
                return Auditar(d, path);
            }
            return new SaveData();
        }

        /// <summary>Save local e texto puro: o arquivo pode ser editado a mao. A permanencia do nascimento e regra de
        /// dominio (DestinySystem), nao do arquivo — aqui so DETECTAMOS e registramos. Nao prometemos anti-cheat local
        /// (ADR-0004). ponytail: so loga; se um dia o jogo precisar reagir, o ponto de decisao e este.
        /// destinyId vazio = ainda nao nasceu (save novo, v0 migrado): nada a auditar, a tela de nascimento resolve.</summary>
        static SaveData Auditar(SaveData d, string path)
        {
            if (d == null || d.birth == null || string.IsNullOrEmpty(d.birth.destinyId)) return d;
            BirthError erro = DestinySystem.Validar(d.birth);
            if (erro != BirthError.Nenhum)
                Debug.LogWarning("LocalSave: nascimento invalido no save (" + erro + ") em " + path
                    + "; destino='" + d.birth.destinyId + "' origem='" + d.birth.originId + "'.");
            return d;
        }

        /// <summary>SaveData valido (ja migrado), ou null. Se o arquivo existe e nao serve, ele e movido para .rejeitado
        /// com log. maisNovo = true quando o arquivo e de uma build mais nova: ai ele NAO e tocado.</summary>
        static SaveData ReadOrQuarantine(string path, out bool maisNovo)
        {
            maisNovo = false;
            if (!File.Exists(path)) return null;

            string json;
            try { json = File.ReadAllText(path); }
            catch (Exception e)
            {
                Debug.LogWarning("LocalSave: falha ao ler " + path + " (" + e.Message + ").");
                return null;
            }

            int v = VersionOf(json);
            if (v > SaveData.SchemaVersion)
            {
                maisNovo = true;
                Debug.LogWarning("LocalSave: " + path + " tem saveVersion " + v + ", mais nova que esta build ("
                    + SaveData.SchemaVersion + "). Fica intacto e nada sera gravado por cima; seguindo com save padrao.");
                return null;
            }

            SaveData d = FromJson(json);
            if (d == null)
            {
                Quarantine(path, v == Ilegivel ? "ilegivel" : "saveVersion " + v + " com conteudo invalido");
                return null;
            }
            if (v < SaveData.SchemaVersion) GuardarOriginal(path, v);
            return d;
        }

        /// <summary>Copia o arquivo de versao antiga antes que a proxima gravacao o substitua. Primeira copia vence:
        /// nunca sobrescreve uma copia que ja existe.</summary>
        static void GuardarOriginal(string path, int versao)
        {
            string copia = PreMigrationPath(path, versao);
            try
            {
                if (!File.Exists(copia)) File.Copy(path, copia);
                Debug.Log("LocalSave: " + path + " migrado de v" + versao + " para v" + SaveData.SchemaVersion
                    + "; original em " + copia + ".");
            }
            catch (Exception e)
            {
                Debug.LogWarning("LocalSave: " + path + " migrado de v" + versao + ", sem copia do original (" + e.Message + ").");
            }
        }

        static void Quarantine(string path, string motivo)
        {
            string dest = RejectedPath(path);
            try
            {
                if (File.Exists(dest)) File.Delete(dest);
                File.Move(path, dest);
                Debug.LogWarning("LocalSave: " + path + " descartado (" + motivo + "); copia intacta em " + dest + ".");
            }
            catch (Exception e)
            {
                Debug.LogWarning("LocalSave: " + path + " descartado (" + motivo + "), sem copia (" + e.Message + ").");
            }
        }

        /// <summary>So o cabecalho: Ilegivel (-1) = nao e objeto JSON; 0 = objeto sem saveVersion (v0, prototipo
        /// anterior a T004); N = versao gravada. Le em um tipo separado de proposito — SaveData tem saveVersion com
        /// valor padrao, e um save velho desserializado direto em SaveData sairia se passando pela versao atual.</summary>
        public static int VersionOf(string json)
        {
            if (string.IsNullOrEmpty(json)) return Ilegivel;
            try
            {
                SaveHeader h = JsonUtility.FromJson<SaveHeader>(json);
                return h != null && h.saveVersion >= 0 ? h.saveVersion : Ilegivel;
            }
            catch (Exception) { return Ilegivel; }
        }

        /// <summary>SaveData da versao atual — migrado pela cadeia se o JSON for de versao anterior — ou null
        /// (ilegivel, versao mais nova que a build, bloco obrigatorio null). Nao lanca e nao toca em disco.</summary>
        public static SaveData FromJson(string json)
        {
            int v = VersionOf(json);
            if (v == Ilegivel || v > SaveData.SchemaVersion) return null;
            SaveData d;
            try
            {
                for (; v < SaveData.SchemaVersion; v++) json = Migracoes[v](json);
                d = JsonUtility.FromJson<SaveData>(json);
            }
            catch (Exception) { return null; }
            // bloco explicitamente null no arquivo viraria NullReferenceException em quem le o save:
            // isso e save corrompido, nao save a consertar — cai no .bak como qualquer outro defeito.
            if (d == null || d.birth == null || d.identity == null || d.attributes == null || d.affinities == null) return null;
            return d;
        }

        public static string ToJson(SaveData data) { return JsonUtility.ToJson(data, true); }

        public static void Save(SaveData data) { Save(data, DefaultPath); }

        public static void Save(SaveData data, string path)
        {
            if (data == null) return;
            // O .bak conta tambem: com o principal ilegivel, a segunda gravacao faria File.Replace por cima dele.
            if (MaisNovoQueEstaBuild(path) || MaisNovoQueEstaBuild(BackupPath(path)))
            {
                Debug.LogError("LocalSave: " + path + " (ou o .bak) e de uma versao mais nova do jogo (saveVersion > "
                    + SaveData.SchemaVersion + "); nada foi gravado por cima.");
                return;
            }

            string now = DateTime.UtcNow.ToString("o");
            data.saveVersion = SaveData.SchemaVersion;
            if (string.IsNullOrEmpty(data.characterId)) data.characterId = Guid.NewGuid().ToString("N");
            if (string.IsNullOrEmpty(data.createdAtUtc)) data.createdAtUtc = now;
            data.updatedAtUtc = now;

            string tmp = path + ".tmp";
            try
            {
                string dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(tmp, ToJson(data));
                // ponytail: rename e atomico no mesmo volume; suficiente para 1 arquivo local.
                // File.Replace ainda leva o save anterior para o .bak de graca.
                if (File.Exists(path)) File.Replace(tmp, path, BackupPath(path));
                else File.Move(tmp, path);
            }
            catch (Exception e)
            {
                Debug.LogError("LocalSave: falha ao gravar " + path + " (" + e.Message + ").");
                try { if (File.Exists(tmp)) File.Delete(tmp); } catch (Exception) { }
            }
        }

        /// <summary>ponytail: le o cabecalho do arquivo a cada gravacao; save e pequeno e raro. Nao deu para ler =
        /// false, e a gravacao normal decide (e loga se falhar).</summary>
        static bool MaisNovoQueEstaBuild(string path)
        {
            try { return File.Exists(path) && VersionOf(File.ReadAllText(path)) > SaveData.SchemaVersion; }
            catch (Exception) { return false; }
        }

        [Serializable]
        class SaveHeader { public int saveVersion = 0; }  // 0 = arquivo sem o campo
    }
}
