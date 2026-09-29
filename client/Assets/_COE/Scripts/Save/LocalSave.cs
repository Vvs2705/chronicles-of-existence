using System;
using System.IO;
using UnityEngine;

namespace COE
{
    /// <summary>Camada de disco do save. Tres arquivos ao lado do save.json:
    ///   save.json            — o save corrente
    ///   save.json.bak        — a gravacao anterior, feita automaticamente a cada Save (dossie §K: backup)
    ///   save.json.rejeitado  — arquivo que nao deu para ler; guardado, nunca apagado em silencio
    ///
    /// Leitura: tenta o principal; se ele nao servir, tenta o .bak; se nenhum servir, devolve save padrao.
    /// Nunca lanca para quem chama e nunca "conserta" conteudo: ou o save e valido inteiro, ou e descartado inteiro.
    ///
    /// VERSAO ANTERIOR / DESCONHECIDA = DESCARTE COM COPIA, nao migracao.
    /// Motivo (1 linha): save sem saveVersion nao traz destino, origem nem nome do COE; migrar seria inventar o que
    /// o jogador nunca escolheu.
    /// Quando existir um v1 -> v2 de verdade (campo renomeado ou com outro significado), a migracao entra aqui,
    /// entre FromJson e o retorno de ReadOrQuarantine, e o descarte fica so para versao realmente desconhecida.
    ///
    /// Gravacao atomica: escreve save.json.tmp e troca por cima do save.json, jogando o anterior no .bak.</summary>
    public static class LocalSave
    {
        public const string FileName = "save.json";

        public static string DefaultPath { get { return Path.Combine(Application.persistentDataPath, FileName); } }

        public static string BackupPath(string path) { return path + ".bak"; }
        public static string RejectedPath(string path) { return path + ".rejeitado"; }

        public static SaveData Load() { return Load(DefaultPath); }

        public static SaveData Load(string path)
        {
            SaveData d = ReadOrQuarantine(path);
            if (d != null) return Auditar(d, path);

            d = ReadOrQuarantine(BackupPath(path));
            if (d != null)
            {
                Debug.LogWarning("LocalSave: " + path + " nao servia; recuperado de " + BackupPath(path) + ".");
                return Auditar(d, path);
            }
            return new SaveData();
        }

        /// <summary>Save local e texto puro: o arquivo pode ser editado a mao. A permanencia do nascimento e regra de
        /// dominio (DestinySystem), nao do arquivo — aqui so DETECTAMOS e registramos. Nao prometemos anti-cheat local
        /// (ADR-0004). ponytail: so loga; se um dia o jogo precisar reagir, o ponto de decisao e este.</summary>
        static SaveData Auditar(SaveData d, string path)
        {
            if (d == null || d.birth == null) return d;
            BirthError erro = DestinySystem.Validar(d.birth);
            if (erro != BirthError.Nenhum)
                Debug.LogWarning("LocalSave: nascimento invalido no save (" + erro + ") em " + path
                    + "; destino='" + d.birth.destinyId + "' origem='" + d.birth.originId + "'.");
            return d;
        }

        /// <summary>SaveData valido, ou null. Se o arquivo existe e nao serve, ele e movido para .rejeitado com log.</summary>
        static SaveData ReadOrQuarantine(string path)
        {
            if (!File.Exists(path)) return null;

            string json;
            try { json = File.ReadAllText(path); }
            catch (Exception e)
            {
                Debug.LogWarning("LocalSave: falha ao ler " + path + " (" + e.Message + ").");
                return null;
            }

            SaveData d = FromJson(json);
            if (d != null) return d;

            int v = VersionOf(json);
            Quarantine(path, v == 0 ? "ilegivel ou save de outro jogo" : "saveVersion " + v + " != " + SaveData.SchemaVersion);
            return null;
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

        /// <summary>So o cabecalho: 0 = nao e save do COE (JSON invalido ou sem saveVersion).
        /// Le em um tipo separado de proposito — SaveData tem saveVersion com valor padrao, e um save velho
        /// desserializado direto em SaveData sairia se passando por v1.</summary>
        public static int VersionOf(string json)
        {
            if (string.IsNullOrEmpty(json)) return 0;
            try
            {
                SaveHeader h = JsonUtility.FromJson<SaveHeader>(json);
                return h != null ? h.saveVersion : 0;
            }
            catch (Exception) { return 0; }
        }

        /// <summary>SaveData da versao atual, ou null (JSON invalido, outra versao, save de outro jogo). Nao lanca.</summary>
        public static SaveData FromJson(string json)
        {
            if (VersionOf(json) != SaveData.SchemaVersion) return null;
            SaveData d;
            try { d = JsonUtility.FromJson<SaveData>(json); }
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

        [Serializable]
        class SaveHeader { public int saveVersion = 0; }  // 0 = arquivo sem o campo
    }
}
