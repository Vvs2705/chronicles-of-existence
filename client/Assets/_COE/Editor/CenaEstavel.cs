using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace COE.EditorTools
{
    /// <summary>Cena gerada por script sai igual byte a byte quando o conteudo e igual. O Unity da a cada GameObject (e a
    /// cada PrefabInstance) um fileID-base ALEATORIO ao salvar, e aos componentes base+1, base+2...; regerar a cena trocava
    /// todos os ids e reordenava o arquivo inteiro (24 mil linhas de diff sem mudanca real). Aqui a base vira o hash do
    /// caminho do objeto na hierarquia (o gerador cria sempre na mesma ordem), os componentes seguem o m_Component, e os
    /// blocos voltam a ordem crescente de id, que e a do Unity. Referencia externa ({fileID, guid}) nao muda.
    /// C# puro sobre o texto (<see cref="Normalizar"/>); <see cref="Aplicar"/> grava e reabre a cena.</summary>
    public static class CenaEstavel
    {
        const string ClsGameObject = "1", ClsTransform = "4", ClsRectTransform = "224", ClsPrefabInstance = "1001", ClsSceneRoots = "1660057539";

        static readonly Regex cabecalho = new Regex(@"^--- !u!(\d+) &(-?\d+)( stripped)?$", RegexOptions.Multiline);
        static readonly Regex refLocal = new Regex(@"\{fileID: (-?\d+)\}");

        class Bloco
        {
            public string Cls; public long Id; public bool Stripped; public string Corpo;
            public long Novo;
            public long Ref(string campo) { Match m = Regex.Match(Corpo, "^  " + campo + @": \{fileID: (-?\d+)", RegexOptions.Multiline); return m.Success ? long.Parse(m.Groups[1].Value) : 0; }
            public string Texto(string campo) { Match m = Regex.Match(Corpo, "^  " + campo + @": (.*)$", RegexOptions.Multiline); return m.Success ? m.Groups[1].Value.TrimEnd('\r') : ""; }
            public List<long> Lista(string campo)
            {
                var r = new List<long>();
                Match m = Regex.Match(Corpo, "^  " + campo + @":\s*\n((?:  - .*\n)*)", RegexOptions.Multiline);
                if (m.Success) foreach (Match x in refLocal.Matches(m.Groups[1].Value)) r.Add(long.Parse(x.Groups[1].Value));
                return r;
            }
        }

        public static void Aplicar(string scenePath)
        {
            string texto = File.ReadAllText(scenePath);
            string novo = Normalizar(texto);
            if (novo == texto) return;
            File.WriteAllText(scenePath, novo);
            AssetDatabase.ImportAsset(scenePath, ImportAssetOptions.ForceUpdate);
            EditorSceneManager.OpenScene(scenePath);   // a cena aberta tinha os ids antigos
        }

        public static string Normalizar(string yaml)
        {
            yaml = yaml.Replace("\r\n", "\n");
            MatchCollection cabs = cabecalho.Matches(yaml);
            if (cabs.Count == 0) return yaml;
            string preambulo = yaml.Substring(0, cabs[0].Index);
            var blocos = new List<Bloco>();
            for (int i = 0; i < cabs.Count; i++)
            {
                int ini = cabs[i].Index + cabs[i].Length + 1;
                int fim = i + 1 < cabs.Count ? cabs[i + 1].Index : yaml.Length;
                blocos.Add(new Bloco { Cls = cabs[i].Groups[1].Value, Id = long.Parse(cabs[i].Groups[2].Value), Stripped = cabs[i].Groups[3].Success, Corpo = yaml.Substring(ini, fim - ini) });
            }
            var porId = blocos.ToDictionary(b => b.Id);

            // Caminho de cada transform a partir das raizes (ordem do SceneRoots e do m_Children = ordem de criacao).
            var filhosDeStripped = blocos.Where(b => !b.Stripped && (b.Cls == ClsTransform || b.Cls == ClsRectTransform))
                .GroupBy(b => b.Ref("m_Father")).ToDictionary(g => g.Key, g => g.ToList());
            var chaveGo = new Dictionary<long, string>();
            var chavePi = new Dictionary<long, string>();
            Action<long, string> visitar = null;
            visitar = (tid, caminho) =>
            {
                Bloco t;
                if (!porId.TryGetValue(tid, out t)) return;
                if (t.Stripped)
                {
                    long pi = t.Ref("m_PrefabInstance");
                    if (!chavePi.ContainsKey(pi)) chavePi[pi] = caminho;
                    // filho acrescentado sob a instancia nao aparece em m_Children: vem pelo m_Father, ordenado pelo nome
                    List<Bloco> extras;
                    if (filhosDeStripped.TryGetValue(tid, out extras))
                    {
                        var ordenados = extras.OrderBy(x => NomeDoGo(porId, x), StringComparer.Ordinal).ToList();
                        for (int i = 0; i < ordenados.Count; i++) visitar(ordenados[i].Id, caminho + "/+" + i);
                    }
                    return;
                }
                long go = t.Ref("m_GameObject");
                if (!chaveGo.ContainsKey(go)) chaveGo[go] = caminho + "|" + NomeDoGo(porId, t);
                List<long> filhos = t.Lista("m_Children");
                for (int i = 0; i < filhos.Count; i++) visitar(filhos[i], caminho + "/" + i);
            };
            Bloco raizes = blocos.FirstOrDefault(b => b.Cls == ClsSceneRoots);
            if (raizes != null)
            {
                List<long> r = raizes.Lista("m_Roots");
                for (int i = 0; i < r.Count; i++) visitar(r[i], i.ToString());
            }

            var usados = new HashSet<long>(blocos.Where(Fixo).Select(b => b.Id));
            foreach (Bloco b in blocos.Where(Fixo)) b.Novo = b.Id;

            foreach (var kv in chaveGo.OrderBy(k => k.Value, StringComparer.Ordinal))
            {
                Bloco go;
                if (!porId.TryGetValue(kv.Key, out go) || go.Novo != 0) continue;
                var grupo = new List<Bloco> { go };
                foreach (long c in go.Lista("m_Component")) { Bloco cb; if (porId.TryGetValue(c, out cb) && cb.Novo == 0 && !grupo.Contains(cb)) grupo.Add(cb); }
                Reservar(grupo, "go:" + kv.Value, usados);
            }
            foreach (var kv in chavePi.OrderBy(k => k.Value, StringComparer.Ordinal))
            {
                Bloco pi;
                if (!porId.TryGetValue(kv.Key, out pi) || pi.Novo != 0) continue;
                var grupo = new List<Bloco> { pi };
                grupo.AddRange(blocos.Where(b => b.Stripped && b.Ref("m_PrefabInstance") == pi.Id)
                    .OrderBy(b => b.Cls, StringComparer.Ordinal).ThenBy(b => b.Ref("m_CorrespondingSourceObject")));
                Reservar(grupo, "pi:" + kv.Value, usados);
                // componente acrescentado num objeto da instancia: chave = objeto de origem + tipo + conteudo
                foreach (Bloco s in grupo.Where(b => b.Stripped && b.Cls == ClsGameObject).ToList())
                {
                    var add = blocos.Where(b => b.Novo == 0 && !b.Stripped && b.Ref("m_GameObject") == s.Id)
                        .OrderBy(b => b.Cls, StringComparer.Ordinal).ThenBy(b => SemIds(b.Corpo), StringComparer.Ordinal).ToList();
                    for (int i = 0; i < add.Count; i++)
                        Reservar(new List<Bloco> { add[i] }, "add:" + kv.Value + ":" + s.Ref("m_CorrespondingSourceObject") + ":" + add[i].Cls + ":" + i, usados);
                }
            }
            // Sobra (ex.: material em memoria guardado na cena): tipo + nome + conteudo.
            var soltos = blocos.Where(b => b.Novo == 0).OrderBy(b => b.Cls, StringComparer.Ordinal)
                .ThenBy(b => b.Texto("m_Name"), StringComparer.Ordinal).ThenBy(b => SemIds(b.Corpo), StringComparer.Ordinal).ToList();
            for (int i = 0; i < soltos.Count; i++)
                Reservar(new List<Bloco> { soltos[i] }, "solto:" + soltos[i].Cls + ":" + soltos[i].Texto("m_Name") + ":" + i, usados);

            var mapa = blocos.ToDictionary(b => b.Id, b => b.Novo);
            var sb = new StringBuilder(preambulo);
            foreach (Bloco b in blocos.OrderBy(x => x.Novo))
            {
                sb.Append("--- !u!").Append(b.Cls).Append(" &").Append(b.Novo).Append(b.Stripped ? " stripped" : "").Append('\n');
                sb.Append(refLocal.Replace(b.Corpo, m =>
                {
                    long velho = long.Parse(m.Groups[1].Value), novo;
                    return mapa.TryGetValue(velho, out novo) ? "{fileID: " + novo + "}" : m.Value;
                }));
            }
            return sb.ToString();
        }

        /// <summary>Configuracoes da cena (ids 1..4) e SceneRoots ja tem id fixo.</summary>
        static bool Fixo(Bloco b) { return b.Cls == ClsSceneRoots || (b.Id >= 1 && b.Id <= 4 && b.Cls != ClsGameObject); }

        static string NomeDoGo(Dictionary<long, Bloco> porId, Bloco transform)
        {
            Bloco go;
            return porId.TryGetValue(transform.Ref("m_GameObject"), out go) ? go.Texto("m_Name") : "";
        }

        static string SemIds(string corpo) { return refLocal.Replace(corpo, "{fileID: ?}"); }

        /// <summary>Base = hash da chave; o grupo ocupa base..base+n-1, como o Unity faz. Colisao: novo hash com sal.</summary>
        static void Reservar(List<Bloco> grupo, string chave, HashSet<long> usados)
        {
            for (int sal = 0; ; sal++)
            {
                long b = Hash(sal == 0 ? chave : chave + "#" + sal);
                bool livre = true;
                for (int i = 0; i < grupo.Count && livre; i++) livre = !usados.Contains(b + i);
                if (!livre) continue;
                for (int i = 0; i < grupo.Count; i++) { grupo[i].Novo = b + i; usados.Add(b + i); }
                return;
            }
        }

        /// <summary>FNV-1a 64 em [1e6, 2^62]: positivo, longe dos ids fixos e com folga para base+n.</summary>
        static long Hash(string s)
        {
            ulong h = 14695981039346656037UL;
            foreach (byte c in Encoding.UTF8.GetBytes(s)) { h ^= c; h *= 1099511628211UL; }
            return (long)(h >> 2) + 1000000L;
        }
    }
}
