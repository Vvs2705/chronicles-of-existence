using System.Collections;
using System.Collections.Generic;
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

        [SetUp]
        public void SetUp()
        {
            idadeAntes = TrainingProgress.IdadeAnos;
            TrainingProgress.IdadeAnos = delegate { return 8; }; // depois do salto: treino liberado
        }

        [TearDown]
        public void TearDown()
        {
            TrainingProgress.IdadeAnos = idadeAntes;
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
            System.Action<string, float> antes = TrainingProgress.Sink;
            TrainingProgress.Sink = delegate (string afinidade, float q) { praticas.Add(q); };
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
    }
}
