using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace COE.EditorTools
{
    /// <summary>A vila viva (pedido do idealizador, 2026-10-29: "poucas pessoas, poucas casas, animais, sons"). Chamado pelo
    /// AurenSceneBuilder em dois tempos:
    /// - <see cref="Cenario"/>, logo depois das construcoes do T008: casas de fundo (sem interior, porta so de enfeite), as
    ///   arvores da vila, adornos na porta de cada casa, galinheiro, feno e a carroca do portao. Tudo greybox com colisao; o
    ///   Prototipos troca o visual pelo modelo do Tripo que JA EXISTE (casa_nilo/casa_sera espelhadas e tingidas, arvore,
    ///   barril, caixote, lanterna, arbusto): nenhuma malha nova, nenhum credito.
    /// - <see cref="Vida"/>, depois dos NPCs: moradores andando pela rua (Morador), bichos soltos (Animal), revoadas (Bando),
    ///   o fundo sonoro do periodo (SomAmbiente) e os sons de lugar (FonteDeSom).
    ///
    /// CONTRATO com o T008: nada aqui barra percurso. Construcao e adorno solido ficam a >= <see cref="FolgaDoPercurso"/> de
    /// todo trecho spawn -> ancora (VidaSceneTests confere contra a cena); morador e bicho nao tem colisor.
    /// Posicoes conferidas na planta do AurenSceneBuilder (medidas no fim daquele arquivo). Tudo por tabela: editar aqui e
    /// regerar a cena.</summary>
    public static class VidaSceneSetup
    {
        public const string RaizVila = "Vila";          // sob "Auren": Casas, Adornos, Arvoredo, Terreiro
        public const string RaizVida = "Vida";          // raiz da cena: Moradores, Bichos, Sons
        public const float FolgaDoPercurso = 1.5f;      // a mesma das pecas soltas do Prototipos

        // ---------------------------------------------------------------- tabelas

        /// <summary>Casa de fundo: so silhueta e porta virada para a rua (+Z local). Modelo = prototipo que entra no lugar.</summary>
        public struct Casa
        {
            public readonly string Id, Modelo;
            public readonly Vector3 Pos;
            public readonly float Yaw, Largura, Fundo;
            public readonly bool Espelhar;
            public readonly Color Tinta;
            public Casa(string id, float x, float z, float yaw, float largura, float fundo, string modelo, bool espelhar, Color tinta)
            {
                Id = id; Pos = new Vector3(x, 0f, z); Yaw = yaw; Largura = largura; Fundo = fundo; Modelo = modelo;
                Espelhar = espelhar; Tinta = tinta;
            }
        }

        static readonly Color Branco = Color.white;
        static readonly Color Quente = new Color(1f, 0.93f, 0.86f);
        static readonly Color Frio = new Color(0.9f, 0.94f, 1f);
        static readonly Color Palha = new Color(1f, 0.98f, 0.88f);
        static readonly Color Terra = new Color(0.93f, 0.86f, 0.8f);

        /// <summary>16 casas: fila norte da rua das casas (porta para o sul), a fila sul ganha uma, as voltas da praca, a
        /// estrada norte ate o bosque e a chegada pelo portao. Planta conferida: nenhuma encosta em construcao do T008.</summary>
        public static readonly Casa[] Casas =
        {
            new Casa("casa_fundo_01",  -9.5f, -29.5f, 180f, 8f, 7f, "casa_nilo", false, Quente),
            new Casa("casa_fundo_02",   9f,   -29.5f, 180f, 8f, 7f, "casa_sera", false, Branco),
            new Casa("casa_fundo_03",  19.5f, -29.5f, 180f, 8f, 7f, "casa_nilo", true,  Frio),
            new Casa("casa_fundo_04",  29.5f, -29.5f, 180f, 7f, 7f, "casa_sera", true,  Palha),
            new Casa("casa_fundo_05", -28.5f, -28.5f, 180f, 7f, 7f, "casa_sera", false, Terra),
            new Casa("casa_fundo_06",  -9.5f, -49f,     0f, 7f, 8f, "casa_nilo", true,  Branco),
            new Casa("casa_fundo_07",  22f,    -7f,   -90f, 9f, 8f, "casa_sera", false, Quente),
            new Casa("casa_fundo_08", -22f,    -5f,    90f, 9f, 8f, "casa_nilo", false, Palha),
            new Casa("casa_fundo_09",  22f,    24f,   -90f, 9f, 8f, "casa_nilo", true,  Terra),
            new Casa("casa_fundo_10", -20f,    29f,    90f, 9f, 8f, "casa_sera", true,  Frio),
            new Casa("casa_fundo_11", -13f,    44f,    90f, 9f, 8f, "casa_sera", false, Branco),
            new Casa("casa_fundo_12",  14f,    47f,   -90f, 9f, 8f, "casa_nilo", false, Quente),
            new Casa("casa_fundo_13", -12f,   -63f,    90f, 8f, 7f, "casa_nilo", false, Frio),
            new Casa("casa_fundo_14",  12f,   -63f,   -90f, 8f, 7f, "casa_sera", true,  Palha),
            new Casa("casa_fundo_15",  31f,     8f,   -90f, 8f, 7f, "casa_sera", false, Terra),
            new Casa("casa_fundo_16", -34.5f,  -5f,    90f, 8f, 7f, "casa_nilo", true,  Quente),
        };

        /// <summary>Arvores da vila (fora do Bosque): quintais, beira da cerca, a volta do portao. Todas a > 5 m de percurso.</summary>
        public static readonly Vector3[] Arvores =
        {
            P(-40f, -30f), P(-42f, -50f), P(-45f, -10f), P(-38f, 20f), P(-44f, 40f), P(-35f, 50f), P(44f, -35f),
            P(45f, -10f),  P(42f, 20f),   P(40f, 40f),   P(30f, 55f),  P(-28f, 52f), P(24f, -62f), P(-20f, -78f),
            P(34f, -62f),  P(-6f, -80f),  P(6f, -78f),   P(-15f, -22f), P(15f, -20f), P(28f, -18f), P(-28f, -18f),
            P(-9f, 24f),   P(9f, 24f),    P(5f, -57f),   P(-46f, -70f), P(46f, -70f), P(50f, 0f),  P(-50f, 5f),
        };

        /// <summary>Galinheiro: cerca de 8 x 8 m atras da horta, a galinha cisca dentro do circulo de 2,6 m.</summary>
        public static readonly Vector3 Galinheiro = P(-29f, -66f);
        public const float LadoGalinheiro = 8f;
        public const float RaioDoGalinheiro = 2.6f;

        /// <summary>Chiqueiro: cerca de 6 x 6 m a oeste do galinheiro, alem do feno; os porcos fucam no circulo de 1,8 m.</summary>
        public static readonly Vector3 Chiqueiro = P(-42f, -61f);
        public const float LadoChiqueiro = 6f;
        public const float RaioDoChiqueiro = 1.8f;

        /// <summary>Rotas dos moradores, nas ruas de terra e na praca (fora de toda construcao, poco, banco e mural).</summary>
        static readonly Vector3[] RuaDasCasas = { P(-30f, -39.2f), P(30f, -39.2f) };
        static readonly Vector3[] RuaSul      = { P(1.8f, -68f), P(1.8f, -16f) };
        static readonly Vector3[] VoltaDaPraca = { P(-11.5f, -11.5f), P(11.5f, -11.5f), P(11.5f, 11.5f), P(-11.5f, 11.5f) };
        static readonly Vector3[] RuaNorte    = { P(1.8f, 16f), P(1.8f, 56f) };
        static readonly Vector3[] AteErvanaria = { P(-4f, 2f), P(-12.5f, 9.5f) };
        static readonly Vector3[] AteFerraria = { P(4f, 2f), P(12.5f, 7.5f) };
        static readonly Vector3[] NaHorta     = { P(-20f, -57.5f), P(-24f, -57.5f) };
        static readonly Vector3[] AtePortao   = { P(-1.8f, -20f), P(-1.8f, -66f) };
        static readonly Vector3[] EmCasa      = new Vector3[0];

        /// <summary>O que o morador leva: cada um com uma silhueta propria, mesmo com o corpo emprestado do elenco. Tudo
        /// preso em osso que fica de pe no andar (cabeca, peito, quadril): na mao, a pose do FBX (T) e a do clip nao batem.</summary>
        public enum Carga { Nada, Chapeu, CestaNaCabeca, SacoNasCostas, EnxadaNasCostas, LenhaNasCostas, LanternaNaCintura }

        /// <summary>Morador: corpo emprestado de um prototipo do elenco (tom e altura proprios), o que ele leva e uma rota por
        /// periodo.</summary>
        public struct Figura
        {
            public readonly string Id, Modelo;
            public readonly Color Tinta;
            public readonly float Escala, Fase;
            public readonly Vector3[] Manha, Tarde, Noite;
            public readonly Carga Leva;
            public Figura(string id, string modelo, Color tinta, float escala, float fase, Vector3[] manha, Vector3[] tarde, Vector3[] noite,
                          Carga leva = Carga.Nada)
            {
                Id = id; Modelo = modelo; Tinta = tinta; Escala = escala; Fase = fase; Manha = manha; Tarde = tarde; Noite = noite;
                Leva = leva;
            }
        }

        /// <summary>Doze moradores: de manha a vila enche, a tarde muda de lugar, a noite quase todo mundo recolhe e ficam o
        /// vigia (lanterna na cintura) na estrada norte e alguem dando a volta na praca.</summary>
        public static readonly Figura[] Moradores =
        {
            new Figura("morador_01", "parceiro_treino", Frio,  1.00f,  0f, RuaDasCasas,  VoltaDaPraca, EmCasa, Carga.SacoNasCostas),
            new Figura("morador_02", "oren",   Quente, 0.96f,  7f, RuaSul,       RuaSul,       EmCasa, Carga.CestaNaCabeca),
            new Figura("morador_03", "eira",   Palha,  0.94f, 20f, VoltaDaPraca, AteErvanaria, EmCasa, Carga.Chapeu),
            new Figura("morador_04", "daren",  Terra,  1.03f,  3f, AteFerraria,  RuaDasCasas,  EmCasa, Carga.LenhaNasCostas),
            new Figura("morador_05", "mara",   Frio,   0.97f,  0f, NaHorta,      NaHorta,      EmCasa, Carga.EnxadaNasCostas),
            new Figura("morador_06", "lysa",   Quente, 0.95f, 11f, AteErvanaria, VoltaDaPraca, VoltaDaPraca, Carga.CestaNaCabeca),
            new Figura("morador_07", "tovin",  new Color(0.8f, 0.85f, 0.95f), 1.02f, 0f, EmCasa, RuaNorte, RuaNorte, Carga.LanternaNaCintura),
            new Figura("morador_08", "borin",  Palha,  0.98f,  9f, AtePortao,    AteFerraria,  EmCasa, Carga.SacoNasCostas),
            new Figura("morador_09", "maelis", Terra,  0.96f, 30f, VoltaDaPraca, AtePortao,    EmCasa, Carga.Chapeu),
            new Figura("morador_10", "oren",   Frio,   1.01f, 15f, RuaDasCasas,  AtePortao,    EmCasa, Carga.Chapeu),
            new Figura("morador_11", "daren",  Quente, 0.97f, 40f, VoltaDaPraca, RuaSul,       EmCasa, Carga.Nada),
            new Figura("morador_12", "mara",   Palha,  0.93f, 25f, AtePortao,    RuaDasCasas,  EmCasa, Carga.CestaNaCabeca),
        };

        /// <summary>Velocidade de passeio de adulto (m/s). O clip de andar foi medido na crianca (1,6 m/s a 1,10 m): o Animator
        /// do adulto toca mais devagar para o pe nao escorregar (<see cref="RitmoDoClip"/>).</summary>
        public const float VelocidadeDoMorador = 1.7f;

        public static float RitmoDoClip(float altura)
        {
            return VelocidadeDoMorador * BodyScale.Crianca5 / (MotionSolver.VelocidadeCaminhadaPadrao * Mathf.Max(0.5f, altura));
        }

        static Vector3 P(float x, float z) { return new Vector3(x, 0f, z); }

        // ---------------------------------------------------------------- cenario

        public static void Cenario(Transform mundo, Func<string, Color, Material> mat)
        {
            Material parede  = mat("COE_Auren_Terra",    Cor(0x9C, 0x96, 0x84));   // mesmos materiais do AurenSceneBuilder
            Material madeira = mat("COE_Auren_Madeira",  Cor(0x9C, 0x7A, 0x52));
            Material telhado = mat("COE_Auren_Telhado",  Cor(0xA8, 0x6D, 0x52));
            Material folhagem = mat("COE_Auren_Folhagem", Cor(0x3E, 0x5E, 0x42));
            Material tronco  = mat("COE_Auren_Tronco",   Cor(0x5B, 0x45, 0x30));
            Material feno    = mat("COE_Vila_Feno",      Cor(0xD9, 0xB8, 0x6A));
            Material ferro   = mat("COE_Vila_Ferro",     Cor(0x4A, 0x4A, 0x50));

            Transform vila     = Vazio(RaizVila, mundo).transform;
            Transform casas    = Vazio("Casas", vila).transform;
            Transform adornos  = Vazio("Adornos", vila).transform;
            Transform arvoredo = Vazio("Arvoredo", vila).transform;
            Transform terreiro = Vazio("Terreiro", vila).transform;
            Transform chamines = Vazio(RaizChamines, vila).transform;
            Material pedra = mat("COE_Auren_Pedra", Cor(0xAE, 0xAA, 0x9E));
            Material fumaca = mat("COE_Vila_Fumaca", Cor(0x8E, 0x8A, 0x86));
            Transform janelas = Vazio(RaizJanelas, vila).transform;
            Material vidro = mat("COE_Vila_Janela", Cor(0x3A, 0x42, 0x50));
            Material luz = Acesa("COE_Vila_Acesa", Cor(0xFF, 0xC8, 0x6A));

            for (int i = 0; i < Casas.Length; i++)
            {
                Casa c = Casas[i];
                CasaDeFundo(c, casas, parede, telhado, madeira);
                Adornos(c, i, adornos, madeira, folhagem, ferro, luz);
                Chamine(c, i, chamines, pedra, fumaca);
                Janelas(c, i, janelas, vidro, luz);
            }
            foreach (Vector3 p in Arvores) Arvore(arvoredo, p, tronco, folhagem);
            GalinheiroEFeno(terreiro, madeira, telhado, feno);
            ChiqueiroECocho(terreiro, madeira);
            Carroca(terreiro, madeira, ferro);
        }

        /// <summary>Bloco solido (colisor so no corpo) com telhado de duas aguas e moldura de porta.</summary>
        static void CasaDeFundo(Casa c, Transform pai, Material parede, Material telhado, Material madeira)
        {
            const float Altura = AlturaDaCasa, Inclinacao = InclinacaoDoTelhado;
            Transform raiz = Vazio(c.Id, pai, c.Pos).transform;
            raiz.localRotation = Quaternion.Euler(0f, c.Yaw, 0f);
            Caixa("corpo", raiz, new Vector3(0f, Altura * 0.5f, 0f), new Vector3(c.Largura, Altura, c.Fundo), parede);

            // Cumeeira ao longo de X: cada agua cobre meio fundo, inclinada para fora (+X roda o +Z para baixo).
            float meio = c.Fundo * 0.5f;
            float aba = meio / Mathf.Cos(Inclinacao * Mathf.Deg2Rad) + 0.5f;
            float subida = meio * 0.5f * Mathf.Tan(Inclinacao * Mathf.Deg2Rad);
            for (int lado = -1; lado <= 1; lado += 2)
            {
                GameObject agua = Caixa(lado > 0 ? "telhado_frente" : "telhado_fundo", raiz,
                                        new Vector3(0f, Altura + subida, lado * meio * 0.5f), new Vector3(c.Largura + 0.6f, 0.22f, aba), telhado);
                agua.transform.localRotation = Quaternion.Euler(lado * Inclinacao, 0f, 0f);
                Object.DestroyImmediate(agua.GetComponent<Collider>());
            }
            GameObject porta = Caixa("porta", raiz, new Vector3(0f, 1.05f, c.Fundo * 0.5f + 0.06f), new Vector3(1.3f, 2.1f, 0.12f), madeira);
            Object.DestroyImmediate(porta.GetComponent<Collider>());
        }

        public const string RaizChamines = "Chamines";
        public const string RaizJanelas = "Janelas";

        /// <summary>Duas janelas na fachada (dos lados da porta), sem colisor; tres de cada quatro casas acendem a noite (a
        /// quarta ja dorme). Raiz propria fora do greybox: o Prototipos traz a fachada ate a do modelo.</summary>
        static void Janelas(Casa c, int i, Transform pai, Material vidro, Material luz)
        {
            Transform raiz = Vazio(c.Id, pai, c.Pos).transform;
            raiz.localRotation = Quaternion.Euler(0f, c.Yaw, 0f);
            foreach (float lado in new[] { -1f, 1f })
                Enfeite(PrimitiveType.Cube, "janela", raiz, new Vector3(lado * c.Largura * 0.3f, 1.7f, c.Fundo * 0.5f + 0.05f),
                        new Vector3(0.8f, 0.7f, 0.04f), vidro);
            if (i % 4 != 3) Acender(raiz.gameObject, vidro, luz);
        }

        static void Acender(GameObject go, Material apagada, Material acesa)
        {
            var so = new SerializedObject(go.AddComponent<AcendeANoite>());
            so.FindProperty("apagada").objectReferenceValue = apagada;
            so.FindProperty("acesa").objectReferenceValue = acesa;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        /// <summary>Luz chapada quente (URP Unlit) em memoria, como o marcador de missao: vai dentro da cena, nao suja
        /// Assets/. Sem URP (teste sem pipeline) cai no material toon da paleta.</summary>
        static Material Acesa(string nome, Color cor)
        {
            Shader unlit = Shader.Find("Universal Render Pipeline/Unlit");
            if (unlit == null) return LookSetup.NovoMaterial(nome, cor);
            var m = new Material(unlit) { name = nome };
            m.SetColor("_BaseColor", cor);
            return m;
        }
        public const float AlturaDaCasa = 3.2f, InclinacaoDoTelhado = 28f;

        /// <summary>Topo da chamine da casa de fundo, acima do meio da agua de tras. Fora do greybox da casa (raiz propria):
        /// o Prototipos esconde o greybox e sobe a chamine ate o telhado do modelo.</summary>
        public static Vector3 TopoDaChamine(Casa c, float alturaDoTelhado)
        {
            Quaternion giro = Quaternion.Euler(0f, c.Yaw, 0f);
            return c.Pos + giro * new Vector3(c.Largura * 0.28f, 0f, -c.Fundo * 0.25f) + Vector3.up * alturaDoTelhado;
        }

        /// <summary>Altura do meio da agua de tras do greybox, mais o que a chamine passa dele.</summary>
        public static float AlturaDaChamine(Casa c)
        {
            return AlturaDaCasa + c.Fundo * 0.25f * Mathf.Tan(InclinacaoDoTelhado * Mathf.Deg2Rad) + 1.1f;
        }

        /// <summary>Chamine de pedra sem colisor e, em duas de cada tres casas, a fumaca (manha e noite nas pares, so a noite
        /// nas impares).</summary>
        static void Chamine(Casa c, int i, Transform pai, Material pedra, Material fumaca)
        {
            Transform raiz = Vazio(c.Id, pai, TopoDaChamine(c, AlturaDaChamine(c))).transform;
            raiz.localRotation = Quaternion.Euler(0f, c.Yaw, 0f);
            Enfeite(PrimitiveType.Cube, "chamine", raiz, new Vector3(0f, -0.8f, 0f), new Vector3(0.6f, 1.6f, 0.6f), pedra);
            if (i % 3 == 2) return;
            GameObject f = Vazio("fumaca", raiz, Vector3.up * 0.1f);
            for (int k = 0; k < Fumaca.Baforadas; k++) Enfeite(PrimitiveType.Sphere, "baforada", f.transform, Vector3.zero, Vector3.one * 0.5f, fumaca);
            Fumaca comp = f.AddComponent<Fumaca>();
            var so = new SerializedObject(comp);
            so.FindProperty("periodos").intValue = i % 2 == 0 ? 5 : 4;
            so.FindProperty("fase").floatValue = i * 1.37f;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        /// <summary>Na porta de cada casa: lanterna do lado (sem colisor), barril ou caixote do outro (solido); arbustos nas
        /// laterais. Os nomes sao ids de peca: o Prototipos troca pelo modelo quando o FBX existe.</summary>
        static void Adornos(Casa c, int i, Transform pai, Material madeira, Material folhagem, Material ferro, Material luz)
        {
            Quaternion giro = Quaternion.Euler(0f, c.Yaw, 0f);
            Vector3 Em(float x, float z) { return c.Pos + giro * new Vector3(x, 0f, z); }
            float frente = c.Fundo * 0.5f;

            Transform lanterna = Vazio("lanterna", pai, Em(-1.6f, frente + 0.4f)).transform;
            GameObject poste = Primitiva(PrimitiveType.Cylinder, "poste", lanterna, new Vector3(0f, 0.8f, 0f), new Vector3(0.08f, 0.8f, 0.08f), ferro);
            Object.DestroyImmediate(poste.GetComponent<Collider>());
            GameObject caixa = Caixa("caixa", lanterna, new Vector3(0f, 1.7f, 0f), new Vector3(0.25f, 0.3f, 0.25f), madeira);
            Object.DestroyImmediate(caixa.GetComponent<Collider>());
            Acender(caixa, madeira, luz);   // a lanterna da porta acende a noite

            bool barril = i % 2 == 0;
            Vector3 p = Em(c.Largura * 0.5f - 0.8f, frente + 0.7f);
            if (barril) Primitiva(PrimitiveType.Cylinder, "barril", pai, p + new Vector3(0f, 0.45f, 0f), new Vector3(0.9f, 0.45f, 0.9f), madeira);
            else Caixa("caixote", pai, p + new Vector3(0f, 0.3f, 0f), new Vector3(0.6f, 0.6f, 0.6f), madeira).transform.localRotation = giro;

            foreach (Vector3 q in new[] { Em(c.Largura * 0.5f + 0.8f, -c.Fundo * 0.25f), Em(-(c.Largura * 0.5f + 0.8f), c.Fundo * 0.25f) })
            {
                GameObject arbusto = Primitiva(PrimitiveType.Sphere, "arbusto", pai, q + new Vector3(0f, 0.45f, 0f), new Vector3(1.3f, 0.9f, 1.3f), folhagem);
                Object.DestroyImmediate(arbusto.GetComponent<Collider>());
            }
        }

        /// <summary>A mesma arvore do Bosque (tronco solido, copa so decora), altura variando pela posicao.</summary>
        static void Arvore(Transform pai, Vector3 pos, Material tronco, Material folhagem)
        {
            float v = Mathf.Repeat(pos.x * 0.37f + pos.z * 0.61f, 1f);
            float altura = 5f + 2.5f * v, raio = 2f + 0.8f * v;
            Transform raiz = Vazio("arvore", pai, pos).transform;
            Primitiva(PrimitiveType.Cylinder, "tronco", raiz, new Vector3(0f, altura * 0.5f, 0f), new Vector3(0.5f, altura * 0.5f, 0.5f), tronco);
            GameObject copa = Primitiva(PrimitiveType.Sphere, "copa", raiz, new Vector3(0f, altura + raio * 0.4f, 0f),
                                        new Vector3(raio * 2f, raio * 1.7f, raio * 2f), folhagem);
            Object.DestroyImmediate(copa.GetComponent<Collider>());
        }

        /// <summary>Galinheiro de 8 x 8 m (cerca baixa solida, casinha no canto) e dois fardos de feno ao lado.</summary>
        static void GalinheiroEFeno(Transform pai, Material madeira, Material telhado, Material feno)
        {
            Transform g = Vazio("galinheiro", pai, Galinheiro).transform;
            float m = LadoGalinheiro * 0.5f;
            Caixa("cerca_n", g, new Vector3(0f, 0.4f,  m), new Vector3(LadoGalinheiro, 0.8f, 0.15f), madeira);
            Caixa("cerca_s", g, new Vector3(0f, 0.4f, -m), new Vector3(LadoGalinheiro, 0.8f, 0.15f), madeira);
            Caixa("cerca_l", g, new Vector3( m, 0.4f, 0f), new Vector3(0.15f, 0.8f, LadoGalinheiro), madeira);
            Caixa("cerca_o", g, new Vector3(-m, 0.4f, 0f), new Vector3(0.15f, 0.8f, LadoGalinheiro), madeira);
            // Casinha no canto, fora do circulo das galinhas (RaioDoGalinheiro + folga): elas nao entram na parede.
            Caixa("casinha", g, new Vector3(-m + 0.9f, 0.7f, -m + 0.8f), new Vector3(1.6f, 1.4f, 1.4f), madeira);
            GameObject teto = Caixa("casinha_teto", g, new Vector3(-m + 0.9f, 1.5f, -m + 0.8f), new Vector3(2f, 0.2f, 1.8f), telhado);
            Object.DestroyImmediate(teto.GetComponent<Collider>());

            foreach (float z in new[] { -63f, -66.5f })
                Primitiva(PrimitiveType.Cylinder, "feno", pai, new Vector3(-36.5f, 0.6f, z), new Vector3(1.4f, 0.6f, 1.4f), feno)
                    .transform.localRotation = Quaternion.Euler(0f, 0f, 90f);   // fardo deitado
        }

        /// <summary>Chiqueiro: cerca baixa solida e o cocho no canto, fora do circulo dos porcos.</summary>
        static void ChiqueiroECocho(Transform pai, Material madeira)
        {
            Transform g = Vazio("chiqueiro", pai, Chiqueiro).transform;
            float m = LadoChiqueiro * 0.5f;
            Caixa("cerca_n", g, new Vector3(0f, 0.45f,  m), new Vector3(LadoChiqueiro, 0.9f, 0.15f), madeira);
            Caixa("cerca_s", g, new Vector3(0f, 0.45f, -m), new Vector3(LadoChiqueiro, 0.9f, 0.15f), madeira);
            Caixa("cerca_l", g, new Vector3( m, 0.45f, 0f), new Vector3(0.15f, 0.9f, LadoChiqueiro), madeira);
            Caixa("cerca_o", g, new Vector3(-m, 0.45f, 0f), new Vector3(0.15f, 0.9f, LadoChiqueiro), madeira);
            Caixa("cocho", g, new Vector3(m - 0.5f, 0.2f, -m + 1.4f), new Vector3(0.6f, 0.4f, 1.6f), madeira);
        }

        /// <summary>Carroca parada na chegada pelo portao, fora da estrada (x = -6,5).</summary>
        static void Carroca(Transform pai, Material madeira, Material ferro)
        {
            Transform c = Vazio("carroca", pai, P(-6.5f, -64f)).transform;
            c.localRotation = Quaternion.Euler(0f, 12f, 0f);
            Caixa("caixa", c, new Vector3(0f, 0.85f, 0f), new Vector3(1.4f, 0.6f, 2.6f), madeira);
            Caixa("varal", c, new Vector3(0f, 0.6f, 1.9f), new Vector3(0.1f, 0.1f, 1.4f), madeira);
            foreach (float x in new[] { -0.8f, 0.8f })
            {
                GameObject roda = Primitiva(PrimitiveType.Cylinder, "roda", c, new Vector3(x, 0.5f, -0.3f), new Vector3(1f, 0.05f, 1f), ferro);
                roda.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                Object.DestroyImmediate(roda.GetComponent<Collider>());
            }
        }

        // ---------------------------------------------------------------- vida

        public static void Vida(Transform ancoras, GameObject player, Func<string, Color, Material> mat)
        {
            Transform vida = Vazio(RaizVida, null).transform;
            Transform moradores = Vazio("Moradores", vida).transform;
            Transform bichos = Vazio("Bichos", vida).transform;
            Transform sons = Vazio("Sons", vida).transform;
            Transform jogador = player.transform;

            for (int i = 0; i < Moradores.Length; i++) Morador(Moradores[i], i, moradores, jogador, mat);

            Material branco = mat("COE_Bicho_Branco", Cor(0xF2, 0xEE, 0xE4));
            Material ruivo  = mat("COE_Bicho_Ruivo",  Cor(0xB0, 0x6A, 0x3C));
            Material bico   = mat("COE_Bicho_Bico",   Cor(0xE8, 0xA8, 0x3A));
            Material crista = mat("COE_Bicho_Crista", Cor(0xC8, 0x3C, 0x34));
            Material caramelo = mat("COE_Bicho_Caramelo", Cor(0xA8, 0x7A, 0x4E));
            Material escuro = mat("COE_Bicho_Escuro", Cor(0x4A, 0x3C, 0x34));
            Material cinza  = mat("COE_Bicho_Cinza",  Cor(0x8C, 0x8A, 0x88));
            Material passaro = mat("COE_Bicho_Passaro", Cor(0x3A, 0x3A, 0x44));
            Material asaAmarela = mat("COE_Bicho_Borboleta", Cor(0xF2, 0xCC, 0x5A));
            Material asaBranca = mat("COE_Bicho_BorboletaBranca", Cor(0xF4, 0xF0, 0xE8));

            // Galinhas: cinco no galinheiro, tres soltas no terreiro entre as casas do norte. Recolhem a noite.
            const int Dia = 3;   // manha + tarde
            for (int i = 0; i < 5; i++)
                Galinha(bichos, "galinha_" + (i + 1), Galinheiro, RaioDoGalinheiro, 100 + i, i % 2 == 0 ? branco : ruivo, bico, crista, jogador, Dia);
            Vector3 terreiro = P(14f, -36f);
            for (int i = 0; i < 3; i++)
                Galinha(bichos, "galinha_" + (i + 6), terreiro, 2.2f, 200 + i, i == 1 ? ruivo : branco, bico, crista, jogador, Dia);
            Fonte(sons, "cacarejo_galinheiro", Galinheiro + Vector3.up * 0.5f, SomDoMundo.Galinha, false, 3f, 9f, 0.5f, 16f, Dia);
            Fonte(sons, "cacarejo_terreiro", terreiro + Vector3.up * 0.5f, SomDoMundo.Galinha, false, 6f, 14f, 0.45f, 14f, Dia);

            // Porcos: dois no chiqueiro, de dia e de noite, roncando de vez em quando.
            Material rosa = mat("COE_Bicho_Porco", Cor(0xD9, 0x9A, 0x8C));
            Material focinho = mat("COE_Bicho_Focinho", Cor(0xB8, 0x6E, 0x66));
            for (int i = 0; i < 2; i++) Porco(bichos, "porco_" + (i + 1), Chiqueiro, RaioDoChiqueiro, 500 + i, rosa, focinho, jogador);
            Fonte(sons, "ronco_chiqueiro", Chiqueiro + Vector3.up * 0.4f, SomDoMundo.Porco, false, 7f, 16f, 0.45f, 14f, 7);

            // Cachorro: passeia na rua sul perto da praca e acompanha quem anda devagar perto dele. Late de vez em quando.
            GameObject cao = Cachorro(bichos, P(-3f, -19f), caramelo, escuro, jogador);
            Fonte(cao.transform, "latido", Vector3.up * 0.5f, SomDoMundo.Latido, false, 10f, 28f, 0.55f, 20f, 7, local: true);

            // Gato: cochila na porta da ervanaria e mia raramente, inclusive a noite.
            GameObject gato = Gato(bichos, P(-12.5f, 18.5f), cinza, escuro, jogador);
            Fonte(gato.transform, "miado", Vector3.up * 0.3f, SomDoMundo.Miado, false, 15f, 40f, 0.4f, 12f, 7, local: true);

            // Revoadas sobre os telhados (de dia) e borboletas na horta e na entrada do bosque.
            BandoDe(bichos, "passaros_vila", new Vector3(-18f, 14f, -12f), 6, 9f, 2f, 16f, 6f, 0.7f, passaro, passaro, Dia);
            BandoDe(bichos, "passaros_norte", new Vector3(14f, 16f, 26f), 5, 11f, 2.5f, 19f, 5f, 0.7f, passaro, passaro, Dia);
            BandoDe(bichos, "passaros_portao", new Vector3(6f, 13f, -52f), 5, 8f, 2f, 14f, 6f, 0.7f, passaro, passaro, Dia);
            BandoDe(bichos, "borboletas_horta", new Vector3(-22f, 0.9f, -58f), 3, 1.4f, 0.6f, 6f, 9f, 0.25f, escuro, asaAmarela, Dia);
            BandoDe(bichos, "borboletas_bosque", new Vector3(4.5f, 1f, 57f), 3, 1.6f, 0.6f, 7f, 9f, 0.25f, escuro, asaBranca, Dia);

            // Sons de lugar e o fundo do periodo.
            Fonte(sons, "bigorna", PosicaoDaAncora(ancoras, "ferraria") + Vector3.up * 1.2f, SomDoMundo.Martelo, false, 2.5f, 6f, 0.5f, 24f, Dia);
            Fonte(sons, "poco", Vector3.up * 0.8f, SomDoMundo.Agua, false, 9f, 22f, 0.55f, 14f, 7);
            Fonte(sons, "sussurro_bosque", PosicaoDaAncora(ancoras, "entrada_bosque") + Vector3.up * 1.5f, SomDoMundo.Sussurro, true, 0f, 0f, 0.6f, 22f, 7);
            Vazio("ambiente", sons).AddComponent<SomAmbiente>();
        }

        static Vector3 PosicaoDaAncora(Transform ancoras, string id) { return ancoras.Find(id).position; }

        /// <summary>Morador com o corpo de um prototipo do elenco (tingido) ou, sem FBX, uma capsula sem colisor.</summary>
        static void Morador(Figura f, int i, Transform pai, Transform jogador, Func<string, Color, Material> mat)
        {
            Vector3 inicio = f.Manha.Length > 0 ? f.Manha[0] : f.Tarde.Length > 0 ? f.Tarde[0] : f.Noite[0];
            GameObject go = Vazio(f.Id, pai, inicio);
            Transform corpo = Vazio("Corpo", go.transform).transform;

            float altura = BodyScale.Adulto * f.Escala;
            GameObject fonte = Prototipos.Carregar(f.Modelo);
            if (fonte != null)
            {
                Transform w = Prototipos.Instanciar(fonte, f.Modelo, corpo, Prototipos.Altura(f.Modelo) * f.Escala, Vector2.zero);
                altura = Prototipos.Altura(f.Modelo) * f.Escala;
                Prototipos.Animar(w, true);
                foreach (Collider c in w.GetComponentsInChildren<Collider>(true)) Object.DestroyImmediate(c);
                // O tom vai so na pele do modelo: a carga presa nos ossos guarda a cor propria.
                if (f.Tinta != Color.white)
                {
                    SkinnedMeshRenderer[] peles = w.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                    if (peles.Length == 0) Set(w.gameObject.AddComponent<Tingir>(), "cor", f.Tinta);
                    foreach (SkinnedMeshRenderer pele in peles) Set(pele.gameObject.AddComponent<Tingir>(), "cor", f.Tinta);
                }
                Levar(f.Leva, corpo, w.GetComponentInChildren<Animator>(), altura, mat);
            }
            else
            {
                GameObject capsula = Primitiva(PrimitiveType.Capsule, "capsula", corpo, new Vector3(0f, altura * 0.5f, 0f),
                                               Vector3.one * (altura * 0.5f), mat("COE_Morador_" + (i + 1), Prototipos.CorDoNpc(f.Modelo) * f.Tinta));
                Object.DestroyImmediate(capsula.GetComponent<Collider>());
                Levar(f.Leva, corpo, null, altura, mat);
            }

            Morador m = go.AddComponent<Morador>();
            var so = new SerializedObject(m);
            so.FindProperty("jogador").objectReferenceValue = jogador;
            so.FindProperty("corpo").objectReferenceValue = corpo;
            Rota(so.FindProperty("rotaManha"), f.Manha);
            Rota(so.FindProperty("rotaTarde"), f.Tarde);
            Rota(so.FindProperty("rotaNoite"), f.Noite);
            so.FindProperty("velocidade").floatValue = VelocidadeDoMorador;
            so.FindProperty("fase").floatValue = f.Fase;
            so.FindProperty("ritmoDoClip").floatValue = RitmoDoClip(altura);
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        /// <summary>Monta a carga em volta do corpo de altura 'altura' (pes na origem de 'corpo', olhando +Z) e prende cada peca
        /// no osso dela (cabeca, peito, quadril) quando o modelo e Humanoid; na capsula, no proprio corpo. Sem colisor.</summary>
        static void Levar(Carga c, Transform corpo, Animator a, float altura, Func<string, Color, Material> mat)
        {
            if (c == Carga.Nada) return;
            bool ossos = a != null && a.avatar != null && a.avatar.isHuman;
            Transform Osso(HumanBodyBones b) { Transform t = ossos ? a.GetBoneTransform(b) : null; return t != null ? t : corpo; }
            Vector3 frente = corpo.forward, direita = corpo.right, pe = corpo.position;
            Vector3 topo = pe + Vector3.up * altura, costas = pe + Vector3.up * (altura * 0.68f) - frente * (altura * 0.17f);
            Transform cabeca = Osso(HumanBodyBones.Head), peito = Osso(HumanBodyBones.Chest), quadril = Osso(HumanBodyBones.Hips);

            Material palha = mat("COE_Vila_Palha", Cor(0xC9, 0xA8, 0x62));
            Material saco = mat("COE_Vila_Saco", Cor(0x9A, 0x86, 0x66));
            Material madeira = mat("COE_Auren_Madeira", Cor(0x9C, 0x7A, 0x52));
            Material ferro = mat("COE_Vila_Ferro", Cor(0x4A, 0x4A, 0x50));
            GameObject Peca(PrimitiveType t, string nome, Transform osso, Vector3 onde, Vector3 escala, Material m, Quaternion giro)
            {
                GameObject g = Enfeite(t, "carga_" + nome, corpo, Vector3.zero, escala, m);
                g.transform.SetPositionAndRotation(onde, giro);
                g.transform.SetParent(osso, true);
                return g;
            }
            Quaternion reto = corpo.rotation;
            float k = altura / BodyScale.Adulto;   // a carga acompanha o tamanho de quem leva
            switch (c)
            {
                case Carga.Chapeu:
                    Peca(PrimitiveType.Cylinder, "aba", cabeca, topo - Vector3.up * 0.06f * k, new Vector3(0.5f, 0.012f, 0.5f) * k, palha, reto);
                    Peca(PrimitiveType.Cylinder, "copa", cabeca, topo + Vector3.up * 0.02f * k, new Vector3(0.26f, 0.07f, 0.26f) * k, palha, reto);
                    break;
                case Carga.CestaNaCabeca:
                    Peca(PrimitiveType.Cylinder, "cesta", cabeca, topo + Vector3.up * 0.1f * k, new Vector3(0.48f, 0.1f, 0.48f) * k, palha, reto);
                    Peca(PrimitiveType.Sphere, "frutas", cabeca, topo + Vector3.up * 0.2f * k, new Vector3(0.36f, 0.12f, 0.36f) * k,
                         mat("COE_Vila_Fruta", Cor(0xB8, 0x4A, 0x3A)), reto);
                    break;
                case Carga.SacoNasCostas:
                    Peca(PrimitiveType.Sphere, "saco", peito, costas - frente * 0.08f * k, new Vector3(0.42f, 0.55f, 0.3f) * k, saco, reto);
                    break;
                case Carga.EnxadaNasCostas:
                    Peca(PrimitiveType.Cube, "cabo", peito, costas - frente * 0.04f * k, new Vector3(0.05f, 1.3f, 0.05f) * k, madeira,
                         reto * Quaternion.Euler(0f, 0f, 35f));
                    Peca(PrimitiveType.Cube, "lamina", peito, costas - frente * 0.04f * k + (Quaternion.AngleAxis(35f, frente) * Vector3.up) * 0.62f * k,
                         new Vector3(0.24f, 0.08f, 0.03f) * k, ferro, reto * Quaternion.Euler(0f, 0f, 35f));
                    break;
                case Carga.LenhaNasCostas:
                    for (int i = -1; i <= 1; i++)
                        Peca(PrimitiveType.Cylinder, "lenha", peito, costas - frente * 0.1f * k + direita * (0.09f * i * k) + Vector3.up * (0.02f * i * i * k),
                             new Vector3(0.09f, 0.32f, 0.09f) * k, madeira, reto * Quaternion.Euler(0f, 0f, 90f + 8f * i));
                    break;
                case Carga.LanternaNaCintura:
                    Vector3 cinto = pe + Vector3.up * (altura * 0.48f) + direita * (altura * 0.17f) + frente * 0.04f * k;
                    Peca(PrimitiveType.Cube, "lanterna", quadril, cinto, new Vector3(0.14f, 0.2f, 0.14f) * k, mat("COE_Vila_Luz", Cor(0xF5, 0xD0, 0x7A)), reto);
                    Peca(PrimitiveType.Cube, "alca", quadril, cinto + Vector3.up * 0.13f * k, new Vector3(0.16f, 0.03f, 0.03f) * k, ferro, reto);
                    break;
            }
        }

        static void Rota(SerializedProperty p, Vector3[] pontos)
        {
            p.arraySize = pontos.Length;
            for (int i = 0; i < pontos.Length; i++) p.GetArrayElementAtIndex(i).vector3Value = pontos[i];
        }

        // ---------------------------------------------------------------- bichos (greybox de primitivas, sem colisor)

        static void Galinha(Transform pai, string nome, Vector3 centro, float raio, int semente, Material pena, Material bico,
                            Material crista, Transform jogador, int periodos)
        {
            // Cada galinha comeca num ponto proprio do circulo.
            Vector3 pos = centro + Quaternion.Euler(0f, semente * 77f, 0f) * Vector3.forward * (raio * 0.5f);
            GameObject go = Vazio(nome, pai, pos);
            Transform corpo = Vazio("Corpo", go.transform).transform;
            Enfeite(PrimitiveType.Sphere, "tronco", corpo, new Vector3(0f, 0.28f, 0f), new Vector3(0.32f, 0.3f, 0.4f), pena);
            Transform cabeca = Vazio("cabeca", corpo, new Vector3(0f, 0.42f, 0.16f)).transform;
            Enfeite(PrimitiveType.Sphere, "cabeca", cabeca, new Vector3(0f, 0.04f, 0.03f), Vector3.one * 0.16f, pena);
            Enfeite(PrimitiveType.Cube, "bico", cabeca, new Vector3(0f, 0.03f, 0.12f), new Vector3(0.05f, 0.04f, 0.08f), bico);
            Enfeite(PrimitiveType.Cube, "crista", cabeca, new Vector3(0f, 0.13f, 0.03f), new Vector3(0.03f, 0.07f, 0.08f), crista);
            Transform rabo = Vazio("rabo", corpo, new Vector3(0f, 0.34f, -0.18f)).transform;
            Enfeite(PrimitiveType.Cube, "rabo", rabo, new Vector3(0f, 0.04f, -0.04f), new Vector3(0.06f, 0.16f, 0.12f), pena)
                .transform.localRotation = Quaternion.Euler(-20f, 0f, 0f);
            var pernas = new List<Transform>();
            foreach (float x in new[] { -0.06f, 0.06f })
                pernas.Add(Perna(PrimitiveType.Cylinder, corpo, new Vector3(x, 0.14f, 0f), new Vector3(0.04f, 0.07f, 0.04f), bico));
            AnimalEm(go, Especie.Galinha, corpo, cabeca, rabo, raio, 0f, semente, jogador, periodos, centro, pernas);
        }

        static GameObject Cachorro(Transform pai, Vector3 centro, Material pelo, Material escuro, Transform jogador)
        {
            GameObject go = Vazio("cachorro", pai, centro);
            Transform corpo = Vazio("Corpo", go.transform).transform;
            Enfeite(PrimitiveType.Cube, "tronco", corpo, new Vector3(0f, 0.42f, 0f), new Vector3(0.28f, 0.28f, 0.62f), pelo);
            var pernas = new List<Transform>();
            foreach (float x in new[] { -0.09f, 0.09f })
                foreach (float z in new[] { -0.22f, 0.22f })
                    pernas.Add(Perna(PrimitiveType.Cube, corpo, new Vector3(x, 0.3f, z), new Vector3(0.08f, 0.3f, 0.08f), pelo));
            Transform cabeca = Vazio("cabeca", corpo, new Vector3(0f, 0.58f, 0.32f)).transform;
            Enfeite(PrimitiveType.Cube, "cabeca", cabeca, new Vector3(0f, 0f, 0.06f), new Vector3(0.22f, 0.2f, 0.24f), pelo);
            Enfeite(PrimitiveType.Cube, "focinho", cabeca, new Vector3(0f, -0.04f, 0.2f), new Vector3(0.12f, 0.1f, 0.14f), escuro);
            foreach (float x in new[] { -0.08f, 0.08f })
                Enfeite(PrimitiveType.Cube, "orelha", cabeca, new Vector3(x, 0.12f, 0.02f), new Vector3(0.05f, 0.1f, 0.06f), escuro);
            Transform rabo = Vazio("rabo", corpo, new Vector3(0f, 0.5f, -0.31f)).transform;
            Enfeite(PrimitiveType.Cube, "rabo", rabo, new Vector3(0f, 0.05f, -0.12f), new Vector3(0.05f, 0.05f, 0.25f), pelo)
                .transform.localRotation = Quaternion.Euler(-30f, 0f, 0f);
            AnimalEm(go, Especie.Cachorro, corpo, cabeca, rabo, 5f, 7f, 300, jogador, 7, centro, pernas);
            return go;
        }

        static GameObject Gato(Transform pai, Vector3 centro, Material pelo, Material escuro, Transform jogador)
        {
            GameObject go = Vazio("gato", pai, centro);
            Transform corpo = Vazio("Corpo", go.transform).transform;
            Enfeite(PrimitiveType.Cube, "tronco", corpo, new Vector3(0f, 0.24f, 0f), new Vector3(0.18f, 0.18f, 0.42f), pelo);
            var pernas = new List<Transform>();
            foreach (float x in new[] { -0.06f, 0.06f })
                foreach (float z in new[] { -0.15f, 0.15f })
                    pernas.Add(Perna(PrimitiveType.Cube, corpo, new Vector3(x, 0.16f, z), new Vector3(0.05f, 0.16f, 0.05f), pelo));
            Transform cabeca = Vazio("cabeca", corpo, new Vector3(0f, 0.36f, 0.22f)).transform;
            Enfeite(PrimitiveType.Sphere, "cabeca", cabeca, Vector3.zero, Vector3.one * 0.17f, pelo);
            foreach (float x in new[] { -0.05f, 0.05f })
                Enfeite(PrimitiveType.Cube, "orelha", cabeca, new Vector3(x, 0.09f, 0f), new Vector3(0.04f, 0.06f, 0.02f), escuro)
                    .transform.localRotation = Quaternion.Euler(0f, 0f, 45f);
            Transform rabo = Vazio("rabo", corpo, new Vector3(0f, 0.3f, -0.21f)).transform;
            Enfeite(PrimitiveType.Cube, "rabo", rabo, new Vector3(0f, 0.08f, -0.12f), new Vector3(0.03f, 0.03f, 0.3f), pelo)
                .transform.localRotation = Quaternion.Euler(-50f, 0f, 0f);
            AnimalEm(go, Especie.Gato, corpo, cabeca, rabo, 2f, 0f, 400, jogador, 7, centro, pernas);
            return go;
        }

        static void Porco(Transform pai, string nome, Vector3 centro, float raio, int semente, Material pele, Material focinho,
                          Transform jogador)
        {
            Vector3 pos = centro + Quaternion.Euler(0f, semente * 131f, 0f) * Vector3.forward * (raio * 0.4f);
            GameObject go = Vazio(nome, pai, pos);
            Transform corpo = Vazio("Corpo", go.transform).transform;
            Enfeite(PrimitiveType.Sphere, "tronco", corpo, new Vector3(0f, 0.36f, 0f), new Vector3(0.46f, 0.4f, 0.78f), pele);
            var pernas = new List<Transform>();
            foreach (float x in new[] { -0.13f, 0.13f })
                foreach (float z in new[] { -0.24f, 0.24f })
                    pernas.Add(Perna(PrimitiveType.Cube, corpo, new Vector3(x, 0.2f, z), new Vector3(0.09f, 0.2f, 0.09f), pele));
            Transform cabeca = Vazio("cabeca", corpo, new Vector3(0f, 0.4f, 0.38f)).transform;
            Enfeite(PrimitiveType.Sphere, "cabeca", cabeca, new Vector3(0f, 0f, 0.06f), new Vector3(0.3f, 0.28f, 0.28f), pele);
            Enfeite(PrimitiveType.Cylinder, "focinho", cabeca, new Vector3(0f, -0.03f, 0.21f), new Vector3(0.12f, 0.03f, 0.12f), focinho)
                .transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            foreach (float x in new[] { -0.09f, 0.09f })
                Enfeite(PrimitiveType.Cube, "orelha", cabeca, new Vector3(x, 0.13f, 0.04f), new Vector3(0.08f, 0.08f, 0.03f), pele)
                    .transform.localRotation = Quaternion.Euler(-25f, 0f, x * 200f);
            Transform rabo = Vazio("rabo", corpo, new Vector3(0f, 0.42f, -0.39f)).transform;
            Enfeite(PrimitiveType.Sphere, "rabo", rabo, new Vector3(0f, 0.02f, -0.03f), new Vector3(0.05f, 0.05f, 0.08f), focinho);
            AnimalEm(go, Especie.Porco, corpo, cabeca, rabo, raio, 0f, semente, jogador, 7, centro, pernas);
        }

        /// <summary>Pivo da perna no quadril e o osso pendurado nele: o Animal gira o pivo no passo.</summary>
        static Transform Perna(PrimitiveType tipo, Transform corpo, Vector3 quadril, Vector3 tamanho, Material m)
        {
            Transform pivo = Vazio("perna", corpo, quadril).transform;
            Enfeite(tipo, "osso", pivo, new Vector3(0f, -tamanho.y, 0f) * (tipo == PrimitiveType.Cube ? 0.5f : 1f), tamanho, m);
            return pivo;
        }

        /// <summary>O Animal anda no circulo de 'centro'; nasce onde o objeto esta (a galinha, num ponto proprio dele).</summary>
        static void AnimalEm(GameObject go, Especie e, Transform corpo, Transform cabeca, Transform rabo, float raio,
                             float raioDeSeguir, int semente, Transform jogador, int periodos, Vector3 centro, List<Transform> pernas)
        {
            Animal a = go.AddComponent<Animal>();
            var so = new SerializedObject(a);
            so.FindProperty("especie").enumValueIndex = (int)e;
            so.FindProperty("jogador").objectReferenceValue = jogador;
            so.FindProperty("corpo").objectReferenceValue = corpo;
            so.FindProperty("cabeca").objectReferenceValue = cabeca;
            so.FindProperty("rabo").objectReferenceValue = rabo;
            so.FindProperty("centro").vector3Value = centro;
            so.FindProperty("raio").floatValue = raio;
            so.FindProperty("raioDeSeguir").floatValue = raioDeSeguir;
            so.FindProperty("semente").intValue = semente;
            so.FindProperty("periodos").intValue = periodos;
            SerializedProperty ps = so.FindProperty("pernas");
            ps.arraySize = pernas.Count;
            for (int i = 0; i < pernas.Count; i++) ps.GetArrayElementAtIndex(i).objectReferenceValue = pernas[i];
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        /// <summary>Bando de n membros (corpo + duas asas em pivo no centro) voando em roda em volta de 'centro'.</summary>
        static void BandoDe(Transform pai, string nome, Vector3 centro, int n, float raio, float altura, float volta, float batidas,
                            float tamanho, Material corpo, Material asa, int periodos)
        {
            GameObject go = Vazio(nome, pai, centro);
            for (int i = 0; i < n; i++)
            {
                Transform m = Vazio("membro_" + (i + 1), go.transform).transform;
                m.localScale = Vector3.one * tamanho;
                Enfeite(PrimitiveType.Sphere, "corpo", m, Vector3.zero, new Vector3(0.25f, 0.2f, 0.45f), corpo);
                foreach (int lado in new[] { -1, 1 })
                {
                    Transform pivo = Vazio(lado < 0 ? "asa_e" : "asa_d", m).transform;
                    Enfeite(PrimitiveType.Cube, "asa", pivo, new Vector3(lado * 0.3f, 0f, 0f), new Vector3(0.5f, 0.03f, 0.25f), asa);
                }
            }
            Bando b = go.AddComponent<Bando>();
            var so = new SerializedObject(b);
            so.FindProperty("raio").floatValue = raio;
            so.FindProperty("altura").floatValue = altura;
            so.FindProperty("volta").floatValue = volta;
            so.FindProperty("batidas").floatValue = batidas;
            so.FindProperty("periodos").intValue = periodos;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static void Fonte(Transform pai, string nome, Vector3 pos, SomDoMundo som, bool laco, float min, float max, float volume,
                          float alcance, int periodos, bool local = false)
        {
            GameObject go = Vazio(nome, pai);
            if (local) go.transform.localPosition = pos; else go.transform.position = pos;
            FonteDeSom f = go.AddComponent<FonteDeSom>();
            var so = new SerializedObject(f);
            so.FindProperty("som").enumValueIndex = (int)som;
            so.FindProperty("laco").boolValue = laco;
            so.FindProperty("intervaloMin").floatValue = min;
            so.FindProperty("intervaloMax").floatValue = max;
            so.FindProperty("volume").floatValue = volume;
            so.FindProperty("alcance").floatValue = alcance;
            so.FindProperty("periodos").intValue = periodos;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        // ---------------------------------------------------------------- utilidades

        static GameObject Enfeite(PrimitiveType tipo, string nome, Transform pai, Vector3 pos, Vector3 escala, Material m)
        {
            GameObject go = Primitiva(tipo, nome, pai, pos, escala, m);
            Object.DestroyImmediate(go.GetComponent<Collider>());
            return go;
        }

        static void Set(Object alvo, string campo, Color cor)
        {
            var so = new SerializedObject(alvo);
            so.FindProperty(campo).colorValue = cor;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static GameObject Vazio(string nome, Transform pai, Vector3 pos = default(Vector3))
        {
            var go = new GameObject(nome);
            if (pai != null) go.transform.SetParent(pai, false);
            go.transform.localPosition = pos;
            return go;
        }

        static GameObject Primitiva(PrimitiveType tipo, string nome, Transform pai, Vector3 pos, Vector3 escala, Material m)
        {
            GameObject go = GameObject.CreatePrimitive(tipo);
            go.name = nome;
            if (pai != null) go.transform.SetParent(pai, false);
            go.transform.localPosition = pos;
            go.transform.localScale = escala;
            go.GetComponent<Renderer>().sharedMaterial = m;
            return go;
        }

        static GameObject Caixa(string nome, Transform pai, Vector3 pos, Vector3 tamanho, Material m)
        {
            return Primitiva(PrimitiveType.Cube, nome, pai, pos, tamanho, m);
        }

        static Color Cor(int r, int g, int b) { return new Color(r / 255f, g / 255f, b / 255f); }
    }
}
