using UnityEngine;

namespace COE
{
    /// <summary>Dimensoes do corpo numa idade, pes em y=0. C# PURO e FONTE UNICA das proporcoes: o gerador de cena
    /// (BootstrapSceneBuilder, crianca de 5), a camera (ThirdPersonCamera) e o runtime (BodyByAge) leem daqui.
    /// Tudo sai da altura (BodyScale); as fracoes sao as da T002 (hipotese v0, calibrar no playtest).</summary>
    public struct Corpo
    {
        public readonly float Altura;

        public Corpo(float altura) { Altura = altura; }

        /// <summary>Corpo da idade do save. Abaixo de 8 = crianca de 5 (Crianca5); 8 ou mais = Crianca8.
        /// ponytail: dois degraus (BodyScale tem duas alturas de crianca). Curva por idade quando houver marco alem
        /// dos 8 (AgeAdvanceCatalog so tem o salto 5 -> 8).</summary>
        public static Corpo DaIdade(int idadeAnos)
        {
            return new Corpo(idadeAnos >= 8 ? BodyScale.Crianca8 : BodyScale.Crianca5);
        }

        /// <summary>Raio da capsula: mesma proporcao da capsula visual (~0,28 m aos 5).</summary>
        public float Raio { get { return Altura * 0.25f; } }
        /// <summary>Centro da capsula: base nos pes.</summary>
        public float CentroY { get { return Altura * 0.5f; } }
        /// <summary>ponytail: ~0,2 m aos 5 = degrau de escada infantil (o 0,3 m padrao do Unity e joelho de crianca:
        /// ela "subiria" em caixote). Calibrar no playtest junto com as alturas de piso de Auren.</summary>
        public float Degrau { get { return Altura * 0.18f; } }
        /// <summary>Pivo da camera a 85% da altura (linha dos olhos): a camera fica abaixo do adulto e a vila "cresce".</summary>
        public float PivoCamera { get { return Altura * 0.85f; } }
        /// <summary>2,5x a altura: com o FOV padrao a crianca ocupa ~1/3 da altura da tela (enquadramento da T002).</summary>
        public float DistanciaCamera { get { return Altura * 2.5f; } }
        /// <summary>Centro do golpe (Hitbox.altura): meio do tronco, ~0,55 da altura.</summary>
        public float AlturaDoGolpe { get { return Altura * 0.55f; } }
        /// <summary>Alcance do braco a partir do centro do corpo (~0,4 da altura): base do alcance da espada (CombatMoves.NoCorpo).</summary>
        public float Braco { get { return Altura * 0.4f; } }

        /// <summary>Velocidades da idade (Bloco E): as dos 5 anos (MotionSolver, passada medida) vezes a altura sobre a dos 5.
        /// O modelo dos 8 e o dos 5 ESCALADO (BodyByAge) e toca os mesmos clips na mesma cadencia: a passada cresce na mesma
        /// proporcao, e so assim o pe apoiado continua sem escorregar. ponytail: proporcao linear; medir a passada da arte
        /// propria de 8 anos (T013) e trocar por tabela se ela nao for o modelo escalado.</summary>
        public float VelocidadeCaminhada { get { return MotionSolver.VelocidadeCaminhadaPadrao * Altura / BodyScale.Crianca5; } }
        public float VelocidadeCorrida { get { return MotionSolver.VelocidadeCorridaPadrao * Altura / BodyScale.Crianca5; } }
    }

    /// <summary>Aplica no Player o corpo da idade do save (T012): capsula do CharacterController, capsula visual/modelo,
    /// enquadramento da camera, altura do golpe e velocidades de andar e correr. O gerador monta sempre a crianca de 5 anos; aos 8 (depois do salto,
    /// que recarrega a cena) este componente troca tudo no Awake.
    /// Awake roda depois do SaveBootstrap (-200): a sessao ja esta sobre o save do disco. Camera e visuais chegam por
    /// campo (IdadeSceneSetup); CharacterController e Hitbox sao do proprio Player.</summary>
    public class BodyByAge : MonoBehaviour
    {
        [Tooltip("A sessao da partida (objeto Save da cena). Ligado pelo gerador (PartidaSetup); vazio = a do SaveState.")]
        [SerializeField] Partida partida;
        [SerializeField] ThirdPersonCamera cam;
        [Tooltip("Capsula e modelo, montados pelo gerador na altura de 5 anos (filhos diretos do Player).")]
        [SerializeField] Transform[] visuais;

        float alturaVisual = BodyScale.Crianca5;   // altura em que os visuais estao AGORA (o gerador monta aos 5)

        void Awake() { Aplicar(Partida.De(partida).Save.ageYears); }

        /// <summary>Poe o corpo da idade. Idempotente (aplicar 8 duas vezes nao cresce de novo) e reversivel.
        /// Publico porque Awake nao roda em teste de Editor.</summary>
        public Corpo Aplicar(int idadeAnos)
        {
            Corpo c = Corpo.DaIdade(idadeAnos);

            CharacterController cc = GetComponent<CharacterController>();
            if (cc != null)
            {
                cc.height = c.Altura;
                cc.radius = c.Raio;
                cc.center = Vector3.up * c.CentroY;
                cc.stepOffset = c.Degrau;
            }
            Hitbox golpe = GetComponent<Hitbox>();
            if (golpe != null) golpe.altura = c.AlturaDoGolpe;
            CharacterMotor motor = GetComponent<CharacterMotor>();
            if (motor != null) motor.DefinirVelocidades(c.VelocidadeCaminhada, c.VelocidadeCorrida);
            if (cam != null) cam.Enquadrar(c);

            // ponytail: o placeholder e a crianca de 5 anos (1,10 m); aos 8 ele e ESCALADO por Crianca8/Crianca5, pes no
            // chao (posicao escala junto: a capsula tem pivo a meia altura, o modelo nos pes). A arte de 8 anos e a T013:
            // quando existir, troca-se o modelo aqui em vez de escalar.
            float k = c.Altura / alturaVisual;
            if (visuais != null)
                foreach (Transform v in visuais)
                {
                    if (v == null) continue;
                    v.localPosition *= k;
                    v.localScale *= k;
                }
            alturaVisual = c.Altura;
            return c;
        }
    }
}
