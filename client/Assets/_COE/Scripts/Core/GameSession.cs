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

        /// <summary>Memoria de NPC, reputacao, inventario e a opcional que dependia de Nilo alcancam o historico de
        /// vida. Idempotente; devolve quantas mudancas houve. Nao grava: quem chama decide (as transicoes abaixo gravam).</summary>
        public int Sincronizar()
        {
            return EncerrarOQueDependiaDeNilo() + NpcMemory.Sincronizar(Save.npcs, Historia) + Reputacao.Sincronizar()
                 + Inventario.Sincronizar(Save.inventario, Historia);
        }

        /// <summary>ADR-0007 §3: Nilo sumiu (evento gravado na conclusao da Q-04) e a q03 ainda esta aberta = encerrada,
        /// pelo mesmo Encerrar do salto. Roda dentro de Sincronizar, entao sai na MESMA gravacao da conclusao e tambem
        /// conserta save gravado no meio. q03 concluida ou ja encerrada fica como esta (nada muda, devolve 0).
        /// ponytail: uma consequencia, um if. Tabela {evento -> missoes encerradas} quando houver a segunda.</summary>
        int EncerrarOQueDependiaDeNilo()
        {
            if (!Historia.Ja(QuestCatalog.EventoNiloDesapareceu)) return 0;
            QuestStatus s = Missoes.Estado(QuestCatalog.MissaoQuePedeNilo);
            if (s == QuestStatus.Concluida || s == QuestStatus.Falhada) return 0;
            return Missoes.Encerrar(QuestCatalog.MissaoQuePedeNilo).Ok ? 1 : 0;
        }

        /// <summary>Uma transicao de missao (Iniciar, CumprirObjetivo, Concluir, EscolherDesfecho, TentarAvancar...).
        /// Ok = sincroniza e grava uma vez. Recusada = nada gravado.
        ///
        /// ADR-0007 §1: se a transicao CONCLUIU missao, o dia anda UM periodo, na mesma gravacao. Um passo por
        /// transicao, nao por missao: o MissaoMundo.Avancar que conclui a q07 (e inicia a q08), ou que conclui duas
        /// de uma vez, anda um periodo so — senao o jogador perderia a tarde sem ter visto ela passar.</summary>
        public QuestResultado Missao(Func<QuestSystem, QuestResultado> transicao)
        {
            if (transicao == null) throw new ArgumentNullException("transicao");
            int concluidas = Missoes.Concluidas();
            QuestResultado r = transicao(Missoes);
            if (!r.Ok) return r;
            if (Missoes.Concluidas() > concluidas) TimeOfDayCycle.Avancar(Save.life);
            Gravar();
            return r;
        }

        /// <summary>ADR-0007 §1: o jogador escolheu Descansar em casa. O dia anda UM periodo (noite vira a manha do
        /// dia seguinte) e grava uma vez. Recusado (save sem o bloco de vida) = nada muda nem grava.
        /// Nao envelhece ninguem: idade so sobe no salto (AgeAdvance).</summary>
        public bool Descansar()
        {
            if (Save.life == null) return false;
            TimeOfDayCycle.Avancar(Save.life);
            Gravar();
            return true;
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
