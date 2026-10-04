using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace COE
{
    /// <summary>Para onde ir agora: o primeiro alvo em cena que faz a historia andar. Le o PROPRIO dado do jogo (quem
    /// oferece e quem cumpre: MissaoNaConversa.Participantes; gatilho de ancora: QuestTrigger ligado pelo MissaoHud),
    /// entao missao nova no catalogo aparece aqui sem codigo. Depois da Q-08: o simbolo do salto; aos 8 anos, o
    /// parceiro de treino. Usado pelo IndicadorDeObjetivo (a seta do jogador) e pelo Roteiro (a simulacao).
    /// Ordem do catalogo (= ordem da historia: cada central pede a anterior). A seta do jogador passa ignorarOpcionais:
    /// aponta so a historia principal; opcional se descobre conversando. Nao muda nada.</summary>
    public static class RumoDaMissao
    {
        /// <summary>O alvo, ou null (motivo diz o que faltou: NPC fora de cena neste periodo, nada aberto...).</summary>
        public static Transform Alvo(GameSession s, bool ignorarOpcionais, out string motivo)
        {
            return Alvo(s, ignorarOpcionais, Interactable.Ativos, out motivo);
        }

        /// <summary>'candidatos' = Interactable.Ativos no jogo; o teste de editor passa os da cena (sem OnEnable).</summary>
        public static Transform Alvo(GameSession s, bool ignorarOpcionais, IList<Interactable> candidatos, out string motivo)
        {
            var faltando = new StringBuilder();
            foreach (QuestDef d in QuestCatalog.Missoes)
            {
                if (ignorarOpcionais && !d.Central) continue;
                QuestStatus st = s.Missoes.Estado(d.Id);
                if (st == QuestStatus.Disponivel)
                {
                    Interactable npc = Npc(Npcs(d.Id), candidatos);
                    if (npc != null) { motivo = "iniciar " + d.Id; return npc.transform; }
                    faltando.Append(d.Id).Append(" sem quem a ofereca; ");
                    continue;
                }
                if (st != QuestStatus.EmAndamento) continue;

                string[] feitos = s.Missoes.ObjetivosFeitos(d.Id);
                foreach (ObjetivoDef o in d.Objetivos)
                {
                    if (Array.IndexOf(feitos, o.Id) >= 0) continue;
                    Interactable alvo = Npc(Npcs(d.Id + "/" + o.Id), candidatos) ?? Gatilho(d.Id, o.Id, candidatos);
                    if (alvo != null) { motivo = d.Id + "/" + o.Id; return alvo.transform; }
                    faltando.Append(d.Id).Append('/').Append(o.Id).Append(" sem alvo em cena; ");
                    if (d.ObjetivosEmOrdem) break;
                }
            }

            if (SaltoHud.Disponivel(s))
            {
                SaltoGatilho simbolo = Achar<SaltoGatilho>(candidatos);
                motivo = "salto";
                if (simbolo != null) return simbolo.transform;
            }
            else if (TrainingProgress.PodeTreinar(s.Save.ageYears) && !TrainingProgress.TreinoSupervisionadoFeito(s.Save))
            {
                TrainingDummy parceiro = UnityEngine.Object.FindAnyObjectByType<TrainingDummy>();
                motivo = "treino";
                if (parceiro != null) return parceiro.transform;
            }
            motivo = faltando.Length > 0 ? faltando.ToString() : "nenhuma missao aberta";
            return null;
        }

        static string[] Npcs(string chave)
        {
            foreach (var p in MissaoNaConversa.Participantes) if (p.Chave == chave) return p.Npcs;
            return new string[0];
        }

        static Interactable Npc(string[] ids, IList<Interactable> candidatos)
        {
            foreach (string id in ids)
                foreach (Interactable i in candidatos)
                {
                    var n = i as NpcActor;
                    if (n != null && n.NpcId == id && n.Acionavel) return n;
                }
            return null;
        }

        static Interactable Gatilho(string questId, string objetivoId, IList<Interactable> candidatos)
        {
            foreach (Interactable i in candidatos)
            {
                var g = i as QuestTrigger;
                if (g != null && g.QuestId == questId && g.ObjetivoId == objetivoId) return g;
            }
            return null;
        }

        public static T Achar<T>() where T : Interactable { return Achar<T>(Interactable.Ativos); }

        static T Achar<T>(IList<Interactable> candidatos) where T : Interactable
        {
            foreach (Interactable i in candidatos) if (i is T && i.Acionavel && i.isActiveAndEnabled) return (T)i;
            return null;
        }
    }
}
