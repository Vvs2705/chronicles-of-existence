using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace COE.PlayModeTests
{
    /// <summary>Treino de combate pelo CAMINHO REAL: PlayerCombat -> CharacterAnimator -> Hitbox (fisica) ->
    /// Health do parceiro -> TrainingLedger. O que os EditMode de regra pura nao veem: overlap de verdade,
    /// i-frames com Time.time e o filtro de dano do bloqueio.</summary>
    public class CombatTrainingTests
    {
        readonly List<GameObject> spawned = new List<GameObject>();
        System.Func<int> idadeAntes;
        System.Action<AtividadeDef, float> sinkAntes;

        [SetUp]
        public void SetUp()
        {
            idadeAntes = TrainingProgress.IdadeAnos;
            TrainingProgress.IdadeAnos = delegate { return 8; }; // depois do salto: treino liberado
            sinkAntes = TrainingProgress.Sink;
            TrainingProgress.Sink = null; // o padrao escreve no SaveState.Current; teste nao suja o save estatico
        }

        [TearDown]
        public void TearDown()
        {
            TrainingProgress.IdadeAnos = idadeAntes;
            TrainingProgress.Sink = sinkAntes;
            foreach (GameObject go in spawned) if (go != null) Object.Destroy(go);
            spawned.Clear();
        }

        GameObject Novo(string nome, Vector3 pos)
        {
            var go = new GameObject(nome);
            go.transform.position = pos;
            spawned.Add(go);
            return go;
        }

        PlayerCombat Jogador(Vector3 pos)
        {
            GameObject go = Novo("Jogador", pos);
            go.AddComponent<BoxCollider>().size = Vector3.one;   // o golpe do parceiro precisa de algo para achar
            go.AddComponent<Faction>().side = Side.Player;
            go.AddComponent<Health>();
            go.AddComponent<Hitbox>();
            go.AddComponent<CharacterAnimator>();  // immediate = true: o golpe sai na hora, sem Animator
            return go.AddComponent<PlayerCombat>();
        }

        TrainingDummy Parceiro(Vector3 pos)
        {
            GameObject go = Novo("Parceiro", pos);
            go.transform.rotation = Quaternion.LookRotation(-pos.normalized); // ja de frente para o jogador na origem
            go.AddComponent<BoxCollider>().size = Vector3.one * 1.5f;
            go.AddComponent<Faction>().side = Side.Hostile;
            go.AddComponent<Health>();
            go.AddComponent<Hitbox>();
            go.AddComponent<HitFlash>(); // como na cena: antes do TrainingDummy, que o pega no Awake (aviso por cor)
            return go.AddComponent<TrainingDummy>();
        }

        [UnityTest]
        public IEnumerator AtaqueLeve_AcertaOParceiroPeloCaminhoReal()
        {
            PlayerCombat p = Jogador(Vector3.zero);
            TrainingDummy d = Parceiro(new Vector3(0f, 0f, 1.2f)); // a frente (forward = +Z)
            Health hd = d.GetComponent<Health>();
            Physics.SyncTransforms();
            yield return null;

            float antes = hd.Current;
            Assert.IsTrue(p.AtacarLeve(), "com idade, vigor e sem recuperacao, o golpe sai");
            Assert.Less(hd.Current, antes, "o golpe tem de chegar no Health do parceiro pelo OverlapSphere");
            Assert.Less(p.Recursos.Vigor.Atual, CombatMoves.VigorMaxV0, "o golpe custou Vigor");
        }

        [UnityTest]
        public IEnumerator Recuperacao_ImpedeSpamNoMesmoFrame()
        {
            PlayerCombat p = Jogador(Vector3.zero);
            Parceiro(new Vector3(0f, 0f, 1.2f));
            Physics.SyncTransforms();
            yield return null;

            Assert.IsTrue(p.AtacarLeve());
            Assert.IsFalse(p.AtacarLeve(), "durante a recuperacao nenhum golpe sai");
            yield return new WaitForSeconds(CombatMoves.Leve.Recuperacao + 0.1f);
            Assert.IsTrue(p.AtacarLeve(), "passada a recuperacao, volta a atacar");
        }

        [UnityTest]
        public IEnumerator AlvoIndefeso_NaoRendePraticaNemComMuitosGolpes()
        {
            PlayerCombat p = Jogador(Vector3.zero);
            TrainingDummy d = Parceiro(new Vector3(0f, 0f, 1.2f));
            Physics.SyncTransforms();
            yield return null;

            var praticas = new List<float>();
            System.Action<AtividadeDef, float> antes = TrainingProgress.Sink;
            TrainingProgress.Sink = delegate (AtividadeDef atividade, float q) { praticas.Add(q); };
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    p.Recursos.Vigor.Encher();
                    if (d.Fase == DummyFase.Ocioso || d.Fase == DummyFase.Recuperacao) p.AtacarLeve();
                    yield return new WaitForSeconds(CombatMoves.Leve.Recuperacao + 0.05f);
                }
            }
            finally { TrainingProgress.Sink = antes; }

            Assert.AreEqual(0, praticas.Count, "golpe em alvo de guarda baixa nao pode emitir pratica");
            Assert.Greater(p.Pratica.GolpesIgnorados, 0, "os golpes aconteceram — so nao valeram dominio");
        }

        [UnityTest]
        public IEnumerator Esquiva_DaIFramesQueEngolemOGolpe()
        {
            PlayerCombat p = Jogador(Vector3.zero);
            Health hp = p.GetComponent<Health>();
            Physics.SyncTransforms();
            yield return null;

            Assert.IsTrue(p.Esquivar());
            Assert.IsTrue(hp.Invulnerable, "a esquiva tem de conceder i-frames");
            float vida = hp.Current;
            Assert.IsFalse(hp.TakeDamage(20f, 0f, p.transform), "durante os i-frames o golpe e ignorado");
            Assert.AreEqual(vida, hp.Current, 1e-3f);

            yield return new WaitForSeconds(CombatMoves.IFramesEsquiva + 0.1f);
            Assert.IsFalse(hp.Invulnerable, "os i-frames acabam");
        }

        [UnityTest]
        public IEnumerator SemVigor_EsquivaNaoSai()
        {
            PlayerCombat p = Jogador(Vector3.zero);
            yield return null;
            while (p.Recursos.Vigor.TryGastar(CombatMoves.Esquiva.Vigor)) { } // esvazia
            Assert.IsFalse(p.Esquivar(), "sem Vigor nao ha esquiva de graca");
        }

        [UnityTest]
        public IEnumerator PrimeiraInfancia_NaoTemAtaqueNemMagia_MasTemEsquiva()
        {
            TrainingProgress.IdadeAnos = delegate { return 5; };
            PlayerCombat p = Jogador(Vector3.zero);
            Parceiro(new Vector3(0f, 0f, 1.2f));
            Physics.SyncTransforms();
            yield return null;

            Assert.IsFalse(p.AtacarLeve(), "aos 5 anos nao ha combate (dossie §F)");
            Assert.IsFalse(p.AtacarForte());
            Assert.IsFalse(p.LancarMagia());
            Assert.AreEqual(CombatMoves.VigorMaxV0, p.Recursos.Vigor.Atual, 1e-3f, "acao recusada nao pode cobrar recurso");
            Assert.IsTrue(p.Esquivar(), "esquivar e rolar, nao combate: continua valendo");
        }

        [UnityTest]
        public IEnumerator MagiaSemMana_NaoSai()
        {
            PlayerCombat p = Jogador(Vector3.zero);
            Parceiro(new Vector3(0f, 0f, 1.2f));
            Physics.SyncTransforms();
            yield return null;

            while (p.Recursos.Mana.TryGastar(CombatMoves.Magia.Mana)) { }
            Assert.IsFalse(p.LancarMagia(), "sem Mana a magia nao existe");
        }

        [UnityTest]
        public IEnumerator Parceiro_TelegrafaAntesDeBaterENaoPersegue()
        {
            PlayerCombat p = Jogador(Vector3.zero);
            TrainingDummy d = Parceiro(new Vector3(0f, 0f, 1.2f));
            Vector3 posInicial = d.transform.position;
            Health hp = p.GetComponent<Health>();
            Physics.SyncTransforms();
            yield return null;

            float vida = hp.Current;
            bool viuTelegrafico = false;
            float t = 0f;
            while (t < 4f && hp.Current >= vida)
            {
                if (d.Fase == DummyFase.Telegrafico) viuTelegrafico = true;
                t += Time.deltaTime;
                yield return null;
            }

            Assert.IsTrue(viuTelegrafico, "o parceiro anuncia antes de bater");
            Assert.Less(hp.Current, vida, "e o golpe chega de verdade no jogador");
            Assert.AreEqual(posInicial, d.transform.position, "o parceiro de treino nao persegue pelo mapa");
        }

        // ---------------- T011: lacunas fechadas na auditoria ----------------

        [UnityTest]
        public IEnumerator T011_Magia_PreparaManifestaEConsequencia_DepoisRecarga()
        {
            PlayerCombat p = Jogador(Vector3.zero);
            TrainingDummy d = Parceiro(new Vector3(0f, 0f, 1.2f));
            Health hd = d.GetComponent<Health>();
            Physics.SyncTransforms();
            yield return null;

            float vida = hd.Current;
            Assert.IsTrue(p.LancarMagia(), "com idade e mana, a magia comeca");
            Assert.AreEqual(SpellFase.Preparacao, p.Magia.Fase);
            Assert.AreEqual(CombatMoves.ManaMaxV0 - CombatMoves.Magia.Mana, p.Recursos.Mana.Atual, 1e-3f, "o custo sai no inicio");
            Assert.AreEqual(vida, hd.Current, 1e-3f, "na preparacao o efeito ainda nao existe");
            Assert.IsFalse(p.AtacarLeve(), "preparando, o lancador esta comprometido");

            yield return new WaitForSeconds(CombatMoves.MagiaPreparacaoV0 + 0.1f);
            Assert.Less(hd.Current, vida, "na manifestacao a fagulha chega no parceiro pelo Hitbox");

            yield return new WaitForSeconds(CombatMoves.MagiaManifestacaoV0 + CombatMoves.Magia.Recuperacao + 0.1f);
            Assert.AreEqual(SpellFase.Pronta, p.Magia.Fase, "a consequencia termina");
            float mana = p.Recursos.Mana.Atual;
            Assert.IsFalse(p.LancarMagia(), "recarga: a segunda fagulha ainda nao sai");
            Assert.AreEqual(mana, p.Recursos.Mana.Atual, 1e-4f, "recusada na recarga, nao cobra mana");

            yield return new WaitForSeconds(CombatMoves.MagiaRecargaV0);
            Assert.IsTrue(p.LancarMagia(), "passada a recarga, lanca de novo");
        }

        [UnityTest]
        public IEnumerator T011_PrimeiraInfancia_ParceiroNaoBateNaCrianca()
        {
            TrainingProgress.IdadeAnos = delegate { return 5; };
            PlayerCombat p = Jogador(Vector3.zero);
            TrainingDummy d = Parceiro(new Vector3(0f, 0f, 1.2f));
            Health hp = p.GetComponent<Health>();
            Physics.SyncTransforms();
            yield return null;

            float vida = hp.Current;
            yield return new WaitForSeconds(TrainingDummyBrain.OciosoPadrao + TrainingDummyBrain.TelegraficoPadrao +
                                            TrainingDummyBrain.GolpePadrao + 0.3f); // aos 8, o golpe ja teria saido
            Assert.AreEqual(vida, hp.Current, 1e-3f, "aos 5 anos o instrutor nao bate na crianca (dossie §F)");
            Assert.AreEqual(DummyFase.Ocioso, d.Fase, "fica parado, de guarda baixa");
        }

        [UnityTest]
        public IEnumerator T011_Hitbox_AlturaDecideOndeOGolpeAcerta()
        {
            Hitbox hb = Novo("Atacante", Vector3.zero).AddComponent<Hitbox>(); // de frente para +Z
            GameObject alvo = Novo("Alvo", new Vector3(0f, 0.6f, 1f));          // tronco de crianca, 1 m a frente
            alvo.AddComponent<BoxCollider>().size = Vector3.one * 0.3f;
            alvo.AddComponent<Health>();
            Physics.SyncTransforms();
            yield return null;

            hb.altura = 1.6f; // mira de adulto
            Assert.AreEqual(0, hb.Swing(1f, 0f, 1f, 0.3f), "golpe alto passa por cima do alvo baixo");
            hb.altura = 0.6f;
            Assert.AreEqual(1, hb.Swing(1f, 0f, 1f, 0.3f), "na altura do tronco, acerta");
        }

        // ---------------- revisao cruzada L17 ----------------

        [UnityTest]
        public IEnumerator Parceiro_AvisoDoGolpeSeVeSemAnimator()
        {
            Jogador(Vector3.zero);
            TrainingDummy d = Parceiro(new Vector3(0f, 0f, 1.2f));
            HitFlash cor = d.GetComponent<HitFlash>();
            Physics.SyncTransforms();
            yield return null;

            Assert.AreEqual(DummyFase.Ocioso, d.Fase);
            Assert.IsFalse(cor.Hold.HasValue, "ocioso, de guarda baixa: sem cor de aviso");
            float t = 0f;
            while (t < 4f && d.Fase != DummyFase.Telegrafico) { t += Time.deltaTime; yield return null; }
            Assert.AreEqual(DummyFase.Telegrafico, d.Fase);
            Assert.IsTrue(cor.Hold.HasValue, "o parceiro da cena e capsula sem Animator: o aviso tem de aparecer na cor");
            while (t < 8f && d.Fase != DummyFase.Recuperacao) { t += Time.deltaTime; yield return null; }
            Assert.AreEqual(DummyFase.Recuperacao, d.Fase);
            Assert.IsFalse(cor.Hold.HasValue, "na recuperacao (abertura) a cor de aviso sai");
        }

        [UnityTest]
        public IEnumerator Parceiro_NaoZeraAVidaDaCrianca_CedeECuraAoRecompor()
        {
            PlayerCombat p = Jogador(Vector3.zero);
            TrainingDummy d = Parceiro(new Vector3(0f, 0f, 1.2f));
            // O gerador de cena liga `alvo` (BootstrapSceneTests confere); aqui vai por reflexao, sem API so para teste.
            FieldInfo alvo = typeof(TrainingDummy).GetField("alvo", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(alvo, "TrainingDummy sem o campo 'alvo'");
            alvo.SetValue(d, p.transform);
            Health hp = p.GetComponent<Health>();
            Physics.SyncTransforms();
            yield return null;

            hp.TakeDamage(hp.max - 3f); // vida baixa: um golpe do bastao (4) zeraria
            Assert.AreEqual(3f, hp.Current, 1e-3f);

            bool cedeu = false;
            float t = 0f;
            while (t < 10f && !(cedeu && !d.Rendido))
            {
                Assert.Greater(hp.Current, 0f, "o treino ensina, nao mata: a vida da crianca nunca zera");
                if (d.Rendido) cedeu = true;
                t += Time.deltaTime;
                yield return null;
            }
            Assert.IsTrue(cedeu, "com o aluno no limite, o parceiro cede em vez de bater");
            Assert.IsFalse(d.Rendido, "passada a pausa, o parceiro se recompoe");
            Assert.AreEqual(hp.max, hp.Current, 1e-3f, "ao recompor, o adulto cura o aluno");
        }
    }
}
