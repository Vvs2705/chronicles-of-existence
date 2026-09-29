using System;
using System.Collections.Generic;

namespace COE
{
    /// <summary>Uma partida aberta sobre UM SaveData: historico de vida, missoes e reputacao, e as duas regras de
    /// fiacao do SaveState (contrato T005). (1) Save trocou = sessao nova (SaveState.Load recria). (2) Toda transicao
    /// que deu certo sincroniza memoria de NPC e reputacao e grava UMA vez; transicao recusada nao muda nem grava.
    ///
    /// NAO e God Manager: nao tem Update, nao conhece cena, UI nem NPC em cena. Quem tem cena (NPC, gatilho de missao,
    /// tela do salto, tela de nascimento) recebe a sessao e chama uma transicao.
    /// C# puro: o "gravar" vem de fora (SaveState.Commit no jogo; contador nos testes).</summary>
    public sealed class GameSession
    {
        public readonly SaveData Save;
        public readonly LifeEventHistory Historia;
        public readonly QuestSystem Missoes;
        public readonly ReputationSystem Reputacao;
        readonly Action gravar;

        public GameSession(SaveData save, Action gravar)
        {
            if (save == null) throw new ArgumentNullException("save");
            Save = save;
            this.gravar = gravar ?? delegate { };
            Historia = new LifeEventHistory(save);
            Missoes = new QuestSystem(save.quests, new HistoricoDeVidaLedger(save, Historia));
            Reputacao = new ReputationSystem(save.reputation, new ReputationLedger(Historia, save));
            Sincronizar();   // save velho ou gravado no meio: memoria e reputacao alcancam o historico ao abrir
        }

        /// <summary>Memoria de NPC, reputacao e inventario alcancam o historico de vida. Idempotente; devolve quantas
        /// mudancas houve. Nao grava: quem chama decide (as transicoes abaixo gravam).</summary>
        public int Sincronizar()
        {
            return NpcMemory.Sincronizar(Save.npcs, Historia) + Reputacao.Sincronizar()
                 + Inventario.Sincronizar(Save.inventario, Historia);
        }

        /// <summary>Uma transicao de missao (Iniciar, CumprirObjetivo, Concluir, EscolherDesfecho, TentarAvancar...).
        /// Ok = sincroniza e grava uma vez. Recusada = nada gravado.</summary>
        public QuestResultado Missao(Func<QuestSystem, QuestResultado> transicao)
        {
            if (transicao == null) throw new ArgumentNullException("transicao");
            QuestResultado r = transicao(Missoes);
            if (r.Ok) Gravar();
            return r;
        }

        /// <summary>Onde o jogador esta. Nao grava sozinho: vai junto na proxima gravacao (transicao ou pausa do app).
        /// "" = cena inicial / spawn_player. O salto zera a ancora (B14); quem chama depois dele nao deve repor a antiga.</summary>
        public void Posicao(string sceneId, string anchorId)
        {
            Save.sceneId = sceneId ?? "";
            Save.anchorId = anchorId ?? "";
        }

        /// <summary>Missoes opcionais ainda abertas (nao concluidas nem falhadas): o que o salto vai encerrar.</summary>
        public string[] OpcionaisAbertas()
        {
            var abertas = new List<string>();
            foreach (QuestDef d in QuestCatalog.Missoes)
            {
                if (d.Central) continue;
                QuestStatus s = Missoes.Estado(d.Id);
                if (s != QuestStatus.Concluida && s != QuestStatus.Falhada) abertas.Add(d.Id);
            }
            return abertas.ToArray();
        }

        /// <summary>B12: o aviso "o que se encerra". Nao muda nada.</summary>
        public SaltoPreparado PrepararSalto()
        {
            return AgeAdvance.PrepararSalto(Save, AgeAdvanceCatalog.SaltoInfancia, Historia, OpcionaisAbertas());
        }

        /// <summary>B13: confirma o salto. Aplicado = envelhece, encerra as opcionais anunciadas no preparo,
        /// sincroniza e grava UMA vez. Recusado (sem Q-08, repetido, preparo velho) = nada muda nem grava.</summary>
        public SaltoResultado ConfirmarSalto(SaltoPreparado preparado)
        {
            SaltoResultado r = AgeAdvance.ConfirmarSalto(Save, preparado, Historia);
            if (!r.Aplicado) return r;
            foreach (string questId in r.OportunidadesEncerradas) Missoes.Encerrar(questId);
            Gravar();
            return r;
        }

        void Gravar()
        {
            Sincronizar();
            gravar();
        }
    }
}
