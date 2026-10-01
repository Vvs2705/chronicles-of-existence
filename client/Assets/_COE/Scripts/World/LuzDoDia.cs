using UnityEngine;

namespace COE
{
    /// <summary>Uma paleta de luz: sol, ambiente trilight, fog e ceu pintado (COE/Ceu). Espaco de cor do projeto: Gamma.</summary>
    public struct PaletaDeLuz
    {
        public Color Sol, AmbienteCeu, AmbienteMeio, AmbienteChao, CeuTopo, Horizonte, CeuBase, BrilhoDoSol;
        public float Intensidade;
        public Vector2 Rotacao;   // (pitch, yaw) do sol, graus

        public static PaletaDeLuz Lerp(PaletaDeLuz a, PaletaDeLuz b, float t)
        {
            return new PaletaDeLuz
            {
                Sol = Color.Lerp(a.Sol, b.Sol, t),
                AmbienteCeu = Color.Lerp(a.AmbienteCeu, b.AmbienteCeu, t),
                AmbienteMeio = Color.Lerp(a.AmbienteMeio, b.AmbienteMeio, t),
                AmbienteChao = Color.Lerp(a.AmbienteChao, b.AmbienteChao, t),
                CeuTopo = Color.Lerp(a.CeuTopo, b.CeuTopo, t),
                Horizonte = Color.Lerp(a.Horizonte, b.Horizonte, t),
                CeuBase = Color.Lerp(a.CeuBase, b.CeuBase, t),
                BrilhoDoSol = Color.Lerp(a.BrilhoDoSol, b.BrilhoDoSol, t),
                Intensidade = Mathf.Lerp(a.Intensidade, b.Intensidade, t),
                Rotacao = new Vector2(Mathf.LerpAngle(a.Rotacao.x, b.Rotacao.x, t), Mathf.LerpAngle(a.Rotacao.y, b.Rotacao.y, t)),
            };
        }
    }

    /// <summary>ADR-0007 §1: o dia anda em tres periodos (missao concluida ou Descansar). A HUD dizia "Noite" com sol a
    /// pino (visto na simulacao -roteiro, 2026-10-01): agora a luz acompanha. Le o periodo do save; nao muda nada nele.
    /// Ao abrir a cena entra direto na paleta do periodo; quando o periodo vira, desliza em <see cref="SegundosDeTroca"/>.
    /// A TARDE e o look aprovado do prototipo (ADR-0008): LookSetup monta a cena com ela. Noite de luar, azul e legivel
    /// (o publico inclui criancas, ADR-0009): escura o bastante para ler "noite", nunca a ponto de esconder caminho.
    /// ponytail: tres paletas e um lerp; sem ciclo continuo, estrelas nem lanterna acesa. Lanterna com luz e custo de
    /// celular (luz adicional): entra medindo no aparelho.</summary>
    public class LuzDoDia : MonoBehaviour
    {
        public const float SegundosDeTroca = 2.5f;

        [SerializeField] Light sol;
        [SerializeField] Material ceu;   // o do RenderSettings; em runtime vira copia (nao suja o asset no editor)

        static Color Hex(int r, int g, int b) { return new Color(r / 255f, g / 255f, b / 255f); }

        public static PaletaDeLuz Paleta(TimeOfDay periodo)
        {
            switch (periodo)
            {
                case TimeOfDay.Manha:
                    return new PaletaDeLuz
                    {
                        Sol = Hex(0xFF, 0xF4, 0xE2), Intensidade = 1.05f, Rotacao = new Vector2(48f, 25f),
                        AmbienteCeu = Hex(0xA8, 0xC6, 0xE6), AmbienteMeio = Hex(0xD3, 0xD6, 0xCC), AmbienteChao = Hex(0x78, 0x6E, 0x5C),
                        CeuTopo = Hex(0x7C, 0xB6, 0xE8), Horizonte = Hex(0xE4, 0xEC, 0xE6), CeuBase = Hex(0xB2, 0xB6, 0xA4),
                        BrilhoDoSol = Hex(0xFF, 0xF6, 0xDE),
                    };
                case TimeOfDay.Noite:
                    return new PaletaDeLuz
                    {
                        Sol = Hex(0xA8, 0xBC, 0xEE), Intensidade = 0.45f, Rotacao = new Vector2(55f, 150f),
                        AmbienteCeu = Hex(0x4C, 0x60, 0x94), AmbienteMeio = Hex(0x52, 0x5A, 0x7C), AmbienteChao = Hex(0x2E, 0x30, 0x42),
                        CeuTopo = Hex(0x16, 0x20, 0x40), Horizonte = Hex(0x3C, 0x4C, 0x72), CeuBase = Hex(0x2A, 0x2E, 0x42),
                        BrilhoDoSol = Hex(0xC8, 0xD4, 0xF4),
                    };
                default:   // Tarde: o look de fim de tarde do prototipo
                    return new PaletaDeLuz
                    {
                        Sol = Hex(0xFF, 0xEC, 0xD6), Intensidade = 1.0f, Rotacao = new Vector2(38f, -35f),
                        AmbienteCeu = Hex(0x9F, 0xB8, 0xD9), AmbienteMeio = Hex(0xCF, 0xC9, 0xBD), AmbienteChao = Hex(0x7A, 0x66, 0x53),
                        CeuTopo = Hex(0x70, 0xA3, 0xD8), Horizonte = Hex(0xF4, 0xD2, 0xA6), CeuBase = Hex(0xBA, 0xA3, 0x8F),
                        BrilhoDoSol = Hex(0xFF, 0xEC, 0xD6),
                    };
            }
        }

        /// <summary>Aplica a paleta no sol, no ambiente, na fog e no ceu. Publico: o gerador de cena usa a mesma funcao.</summary>
        public static void Aplicar(PaletaDeLuz p, Light sol, Material ceu)
        {
            if (sol != null)
            {
                sol.color = p.Sol;
                sol.intensity = p.Intensidade;
                sol.transform.rotation = Quaternion.Euler(p.Rotacao.x, p.Rotacao.y, 0f);
            }
            RenderSettings.ambientSkyColor = p.AmbienteCeu;
            RenderSettings.ambientEquatorColor = p.AmbienteMeio;
            RenderSettings.ambientGroundColor = p.AmbienteChao;
            RenderSettings.fogColor = p.Horizonte;   // a vila some na tinta do ceu
            if (ceu == null) return;
            ceu.SetColor("_CorTopo", p.CeuTopo);
            ceu.SetColor("_CorHorizonte", p.Horizonte);
            ceu.SetColor("_CorBase", p.CeuBase);
            ceu.SetColor("_CorSol", p.BrilhoDoSol);
        }

        TimeOfDay alvo;
        PaletaDeLuz de, atual;
        float t = 1f;
        bool iniciada;

        void Start()
        {
            if (ceu != null)
            {
                ceu = new Material(ceu);
                RenderSettings.skybox = ceu;
            }
        }

        void Update()
        {
            TimeOfDay periodo = TimeOfDayCycle.Atual(SaveState.Current.life);
            if (!iniciada)
            {
                iniciada = true;
                alvo = periodo;
                atual = Paleta(periodo);
                Aplicar(atual, sol, ceu);
                return;
            }
            if (periodo != alvo)
            {
                alvo = periodo;
                de = atual;
                t = 0f;
            }
            if (t >= 1f) return;
            t = Mathf.Min(1f, t + Time.unscaledDeltaTime / SegundosDeTroca);
            atual = PaletaDeLuz.Lerp(de, Paleta(alvo), Mathf.SmoothStep(0f, 1f, t));
            Aplicar(atual, sol, ceu);
        }
    }
}
