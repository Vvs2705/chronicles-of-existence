using System;
using System.Collections.Generic;
using System.Linq;

namespace COE.EditorTools
{
    /// <summary>Os 8 clips base do humanoide e o Skill (primeira magia), opcional: sem ele o estado Skill reusa o Attack3.
    /// O nome do enum vira o nome do clip importado.</summary>
    public enum HumanoidClip { Idle, Run, Attack1, Attack2, Attack3, Dodge, Hit, Death, Skill }

    /// <summary>Resultado da classificacao dos FBX da pasta de entrada.</summary>
    public sealed class HumanoidFiles
    {
        public string Model;                                                       // arquivo com a malha (nome contem "model")
        public readonly Dictionary<HumanoidClip, string> Clips = new Dictionary<HumanoidClip, string>();
        public readonly List<string> Unmapped = new List<string>();               // arquivos que nao viraram clip
        public IEnumerable<HumanoidClip> Missing
        {
            get { return Enum.GetValues(typeof(HumanoidClip)).Cast<HumanoidClip>().Where(c => !Clips.ContainsKey(c) && !HumanoidMapping.Opcional(c)); }
        }
    }

    /// <summary>Parte pura do HumanoidSetup (sem UnityEditor/UnityEngine; so AnimParams): nome de arquivo -> clip e tabela de eventos.</summary>
    public static class HumanoidMapping
    {
        public const string ModelKey = "model";

        // Substring case-insensitive; a PRIMEIRA linha que casa vence (especificas antes das genericas).
        // Attack1 aqui = "qualquer ataque": os arquivos de ataque sao ordenados por nome e viram Attack1/2/3 (AttackIndex 0/1/2).
        // Nomes padrao do Mixamo: "Idle", "Breathing Idle", "Running", "Sword And Shield Slash", "Stand To Roll",
        // "Sword And Shield Impact", "Dying"... Nome proprio tambem vale ("Attack1.fbx", "Dodge.fbx").
        public static readonly (HumanoidClip Clip, string[] Keys)[] Keys =
        {
            (HumanoidClip.Skill, new[] { "skill", "cast", "spell", "magic" }),   // antes de "attack": "Magic Attack" e magia
            (HumanoidClip.Death, new[] { "death", "dying", "die" }),
            (HumanoidClip.Dodge, new[] { "dodge", "roll", "evade", "dive" }),
            (HumanoidClip.Hit, new[] { "hit", "impact", "reaction" }),
            (HumanoidClip.Attack1, new[] { "attack", "slash", "punch", "kick", "strike", "combo" }),
            (HumanoidClip.Run, new[] { "run", "sprint", "jog" }),
            (HumanoidClip.Idle, new[] { "idle" }),
        };

        // Tempos NORMALIZADOS (0..1) do pipeline secao 3; Mixamo nao traz evento nenhum. Ajustar por playtest.
        static readonly Dictionary<HumanoidClip, (string Method, float Time)[]> events = new Dictionary<HumanoidClip, (string, float)[]>
        {
            // ponytail: contatos do pe a 25%/75% do ciclo = chute; conferir no clip real (OnFootstep e so som por enquanto)
            { HumanoidClip.Run, new[] { (AnimParams.EventFootstep, 0.25f), (AnimParams.EventFootstep, 0.75f) } },
            { HumanoidClip.Attack1, new[] { (AnimParams.EventHitFrame, 0.40f) } },
            { HumanoidClip.Attack2, new[] { (AnimParams.EventHitFrame, 0.40f) } },
            { HumanoidClip.Attack3, new[] { (AnimParams.EventHitFrame, 0.45f) } },
            // OnDodgeEnd a 0.95 (nao 1.0): a transicao Dodge -> Locomotion sai em 0.95 e o evento nao pode cair fora dela
            { HumanoidClip.Dodge, new[] { (AnimParams.EventFootstep, 0.75f), (AnimParams.EventDodgeEnd, 0.95f) } },
        };

        public static (string Method, float Time)[] Events(HumanoidClip clip)
        {
            (string, float)[] e;
            return events.TryGetValue(clip, out e) ? e : new (string, float)[0];
        }

        public static bool Loops(HumanoidClip clip) { return clip == HumanoidClip.Idle || clip == HumanoidClip.Run; }

        /// <summary>Clip que a montagem aceita faltar (Skill cai no Attack3). O dano da magia sai do SpellCast, nao de evento.</summary>
        public static bool Opcional(HumanoidClip clip) { return clip == HumanoidClip.Skill; }

        public static int AttackIndex(HumanoidClip clip) { return clip - HumanoidClip.Attack1; } // Attack1..3 -> 0..2

        /// <summary>Nome de arquivo sem extensao -> clip (Attack1 = "algum ataque"), ou null.</summary>
        public static HumanoidClip? Match(string fileName)
        {
            foreach (var row in Keys)
                foreach (string k in row.Keys)
                    if (fileName.IndexOf(k, StringComparison.OrdinalIgnoreCase) >= 0) return row.Clip;
            return null;
        }

        public static HumanoidFiles Classify(IEnumerable<string> fileNames)
        {
            var r = new HumanoidFiles();
            var attacks = new List<string>();
            foreach (string n in fileNames.OrderBy(x => x, StringComparer.OrdinalIgnoreCase))
            {
                if (r.Model == null && n.IndexOf(ModelKey, StringComparison.OrdinalIgnoreCase) >= 0) { r.Model = n; continue; }
                HumanoidClip? c = Match(n);
                if (c == HumanoidClip.Attack1) attacks.Add(n);
                else if (c != null && !r.Clips.ContainsKey(c.Value)) r.Clips[c.Value] = n;
                else r.Unmapped.Add(n); // desconhecido ou repetido (o 1o em ordem alfabetica fica)
            }
            for (int i = 0; i < attacks.Count; i++)
                if (i < 3) r.Clips[HumanoidClip.Attack1 + i] = attacks[i]; else r.Unmapped.Add(attacks[i]);
            return r;
        }
    }
}
