using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace COE.EditorTools
{
    /// <summary>A vila reage: pecas de greybox ligadas por evento do historico (<see cref="PecaPorEvento"/>), perto das
    /// ancoras. Chamado pelo AurenSceneBuilder depois das ancoras.
    /// Fontes: ELENCO.md (Arbitragem 2, item 1: marcos do sumico de Nilo, de evento.nilo_desapareceu ate marco_idade_8;
    /// a folha da Maelis passa do salto) e SLICE §4.3 / §6 pendencia 4 (mudanca visivel de Auren depois do salto, que a
    /// ficha borin C9 propoe ser a bancada na porta), e ADR-0010 adendo 10 (o chapeu da Lysa, ligado por ESTADO da q05,
    /// nao por evento). Medidas das fichas (docs/arte/fichas); o que a ficha nao mede esta marcado HIPOTESE.
    ///
    /// Monta "PecasDeEvento/&lt;peca&gt;" (nomes unicos entre irmaos, CenaEstavel) com os filhos visuais SEM colisor: nada
    /// aqui barra percurso (T008) nem conta como chao ocupado na vaga do NpcActor. A cena sai no estado de projeto
    /// (5 anos, nada aconteceu); no jogo cada peca se acerta pelo historico no primeiro Update.</summary>
    public static class PecasDeEventoSetup
    {
        public const string Raiz = "PecasDeEvento";

        const string Sumiu = QuestCatalog.EventoNiloDesapareceu;   // "evento.nilo_desapareceu" (conclusao da Q-04)
        const string Salto = AgeAdvanceCatalog.SaltoInfancia;      // "marco_idade_8"
        static readonly string[] Nada = new string[0];

        // Offsets a partir da ancora (XZ; y = 0 no chao). Os que encostam em geometria repetem numeros do
        // AurenSceneBuilder (privados la): mexeu na barreira do bosque, no mural, na casa ou na ferraria, confira aqui.
        static readonly Vector3 NaClareira   = new Vector3(1.5f, 0f, -4f);     // nilo C4: z ~66, x ~+1,5 (clareira em z=70)
        static readonly Vector3 TiraCrianca  = new Vector3(3.25f, 0f, 0.985f);  // face sul da barreira_leste (z=61), rente ao vao
        static readonly Vector3 TiraNilo     = new Vector3(3.55f, 0f, 0.985f);
        static readonly Vector3 Prateleira   = new Vector3(2.5f, 0f, -9.575f);  // parede do fundo de casa_familia, por dentro (z=-52,7)
        static readonly Vector3 NoMural      = new Vector3(-0.6f, 0f, -0.815f); // face da tabua do mural (z=-8,825)
        static readonly Vector3 FundoFerraria = new Vector3(9.8f, 0f, 0f);      // fundo do bloco da ferraria (x=24,5)
        static readonly Vector3 PortaFerraria = new Vector3(0.9f, 0f, -2.4f);   // fachada (x=15,5), ao sul da porta; bigorna fica ao norte
        // ~1,2 m da vaga da Lysa (NpcActor: indice 3, 108 graus, ~(1,43; -0,46)), longe do percurso (x=0) e da barreira (z=61)
        static readonly Vector3 AoLadoDaLysa = new Vector3(2.4f, 0f, 0.2f);

        const string Q05 = "q05_o_animal_ferido";

        const float AlturaPrateleira = 1.5f;   // HIPOTESE: "prateleira de cima" (mara C4) que a crianca de 1,28 m alcanca e a de 1,10 nao

        public static void Montar(Transform ancoras, Func<string, Color, Material> mat = null)
        {
            if (ancoras == null) throw new ArgumentNullException("ancoras");
            if (mat == null) mat = LookSetup.NovoMaterial;
            Material madeira   = mat("COE_Peca_Madeira",   Cor(0x9C, 0x7A, 0x52));
            Material terracota = mat("COE_Peca_Terracota", Cor(0xA8, 0x6D, 0x52)); // GDD cap. 09 "Auren"; fichas mara, borin
            Material marfim    = mat("COE_Peca_Marfim",    Cor(0xE9, 0xDE, 0xC6)); // GDD "textos e pergaminhos"; ficha maelis

            Transform raiz = new GameObject(Raiz).transform;

            // nilo C4: a forquilha cravada onde a escolta do Tovin termina, do sumico ao salto. Galho de 1,00 m (cabo 0,80 e
            // bracos de 0,20 abertos em ~60 graus), diametro 0,035, enterrado 0,15: cabo de 0,65 de fora e o Y no alto (~0,82 m).
            Transform forquilha = Peca("forquilha_nilo", raiz, Pos(ancoras, "bosque_clareira", NaClareira), new[] { Sumiu }, new[] { Salto });
            Bloco(PrimitiveType.Cylinder, "cabo", forquilha, new Vector3(0f, 0.325f, 0f), new Vector3(0.035f, 0.325f, 0.035f), madeira, 0f);
            Bloco(PrimitiveType.Cylinder, "braco_esq", forquilha, new Vector3(-0.05f, 0.737f, 0f), new Vector3(0.035f, 0.1f, 0.035f), madeira, 30f);
            Bloco(PrimitiveType.Cylinder, "braco_dir", forquilha, new Vector3(0.05f, 0.737f, 0f), new Vector3(0.035f, 0.1f, 0.035f), madeira, -30f);

            // mara C4: tiras de 0,60 x 0,08 amarradas a 1,0 m no lado leste do vao de entrada_bosque. A da crianca desde o
            // comeco; a do Nilo do sumico. As duas saem no salto e aparecem dobradas na prateleira de cima de casa_familia.
            // ponytail: presas na face da barreira_leste; o tronco do lado leste do vao entra com a arte do bosque (T013).
            Tira(Peca("tira_crianca_vao", raiz, Pos(ancoras, "entrada_bosque", TiraCrianca), Nada, new[] { Salto }), terracota);
            Tira(Peca("tira_nilo_vao", raiz, Pos(ancoras, "entrada_bosque", TiraNilo), new[] { Sumiu }, new[] { Salto }), terracota);

            Vector3 prateleira = Pos(ancoras, "casa_familia", Prateleira);
            Bloco(PrimitiveType.Cube, "prateleira_casa_familia", raiz, prateleira + Vector3.up * AlturaPrateleira,
                  new Vector3(0.9f, 0.03f, 0.25f), madeira, 0f);   // HIPOTESE de tamanho; sempre ali, nao e peca de evento
            Dobrada(Peca("tira_crianca_prateleira", raiz, prateleira + new Vector3(-0.15f, 0f, 0f), new[] { Salto }, Nada), terracota);
            Dobrada(Peca("tira_nilo_prateleira", raiz, prateleira + new Vector3(0.15f, 0f, 0f), new[] { Salto, Sumiu }, Nada), terracota);

            // maelis C4: a folha do sumico (0,15 x 0,20, marfim) fica de pe do sumico em diante e passa do salto ("nao fecho").
            // ponytail: a ficha poe a folha na prancha da Maelis (prop rigido no Chest); sem malha, ela fica no mural, onde a
            // Maelis passa as tardes (Arbitragem 2.2). Quando a malha entrar, esta mesma PecaPorEvento vai para o prop.
            Transform folha = Peca("folha_maelis", raiz, Pos(ancoras, "mural_avisos", NoMural), new[] { Sumiu }, Nada);
            Bloco(PrimitiveType.Cube, "folha", folha, new Vector3(0f, 2.1f, 0f), new Vector3(0.15f, 0.2f, 0.01f), marfim, 0f);

            // borin C9: antes do salto a bancada fica no fundo da ferraria, de costas para a porta; depois, na porta, a luz do
            // dia (a "mudanca visivel de Auren" do SLICE §4.3). HIPOTESE de tamanho: 1,6 x 0,8 x 0,7 m (a ficha nao mede).
            // ponytail: a ferraria do greybox e um bloco macico, entao a bancada do fundo so aparece quando houver interior (T013).
            Bancada(Peca("bancada_borin_fundo", raiz, Pos(ancoras, "ferraria", FundoFerraria), Nada, new[] { Salto }), madeira);
            Bancada(Peca("bancada_borin_porta", raiz, Pos(ancoras, "ferraria", PortaFerraria), new[] { Salto }, Nada), madeira);

            Chapeu(Peca("chapeu_lysa", raiz, Pos(ancoras, "entrada_bosque", AoLadoDaLysa), Nada, Nada, Q05, "buscar_ajuda"), marfim);

            // Estado de projeto na cena salva: 5 anos, historico vazio.
            foreach (PecaPorEvento p in raiz.GetComponentsInChildren<PecaPorEvento>()) p.Atualizar(null);
        }

        static void Tira(Transform peca, Material m)   // pendurada do no (1,0 m) para baixo
        {
            Bloco(PrimitiveType.Cube, "tira", peca, new Vector3(0f, 0.7f, 0f), new Vector3(0.08f, 0.6f, 0.01f), m, 0f);
        }

        static void Dobrada(Transform peca, Material m)   // 0,60 dobrada em tres, deitada na prateleira
        {
            Bloco(PrimitiveType.Cube, "tira_dobrada", peca, new Vector3(0f, AlturaPrateleira + 0.03f, 0f), new Vector3(0.2f, 0.03f, 0.08f), m, 0f);
        }

        static void Bancada(Transform peca, Material m)   // comprida ao longo da fachada (z): tampo sobre dois cavaletes
        {
            Bloco(PrimitiveType.Cube, "tampo", peca, new Vector3(0f, 0.76f, 0f), new Vector3(0.7f, 0.08f, 1.6f), m, 0f);
            Bloco(PrimitiveType.Cube, "cavalete_norte", peca, new Vector3(0f, 0.36f, 0.62f), new Vector3(0.6f, 0.72f, 0.08f), m, 0f);
            Bloco(PrimitiveType.Cube, "cavalete_sul", peca, new Vector3(0f, 0.36f, -0.62f), new Vector3(0.6f, 0.72f, 0.08f), m, 0f);
        }

        /// <summary>lysa C4/C5 (ADR-0010 adendo 10): com a q05 em andamento e buscar_ajuda cumprido, o chapeu fica emborcado
        /// sobre o bicho na entrada do bosque; concluida ou encerrada pelo salto, some. Aba de 0,72 m e copa de 0,10 m (ficha);
        /// a largura da copa e HIPOTESE. Aba 2,5 cm acima do chao: a rua_norte passa por baixo (calcada a 2 cm).
        /// O BichoNoChapeu mede o Player e o DialogueHud le o bicho calmo: os dois ligados aqui, por campo. Sem Player ou
        /// sem conversa em cena (teste de mini-cena), fica sem ligar.
        /// ponytail: marfim le no gramado do celular; a palha e a cor da arte (T013). A Lysa do greybox nao tem chapeu na
        /// cabeca: quando tiver, e uma peca irma com a regra inversa.</summary>
        static void Chapeu(Transform peca, Material m)
        {
            Bloco(PrimitiveType.Cylinder, "aba", peca, new Vector3(0f, 0.035f, 0f), new Vector3(0.72f, 0.01f, 0.72f), m, 0f);
            Bloco(PrimitiveType.Cylinder, "copa", peca, new Vector3(0f, 0.095f, 0f), new Vector3(0.34f, 0.05f, 0.34f), m, 0f);

            BichoNoChapeu bicho = peca.gameObject.AddComponent<BichoNoChapeu>();
            var so = new SerializedObject(bicho);
            Campo(so, "peca").objectReferenceValue = peca.GetComponent<PecaPorEvento>();
            PlayerInteractor player = Object.FindFirstObjectByType<PlayerInteractor>();
            Campo(so, "player").objectReferenceValue = player != null ? player.transform : null;
            so.ApplyModifiedPropertiesWithoutUndo();

            DialogueHud conversa = Object.FindFirstObjectByType<DialogueHud>();
            if (conversa == null) return;
            so = new SerializedObject(conversa);
            Campo(so, "bicho").objectReferenceValue = bicho;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static SerializedProperty Campo(SerializedObject so, string campo)
        {
            SerializedProperty p = so.FindProperty(campo);
            if (p == null) throw new Exception(so.targetObject.GetType().Name + " nao tem o campo serializado '" + campo + "'.");
            return p;
        }

        static Vector3 Pos(Transform ancoras, string id, Vector3 offset)
        {
            Transform a = ancoras.Find(id);
            if (a == null) throw new Exception("PecasDeEventoSetup: falta a ancora '" + id + "'.");
            return a.position + offset;
        }

        static Transform Peca(string nome, Transform raiz, Vector3 pos, string[] exige, string[] some,
                              string missao = "", string comObjetivo = "")
        {
            var go = new GameObject(nome);
            go.transform.SetParent(raiz, false);
            go.transform.position = pos;
            var so = new SerializedObject(go.AddComponent<PecaPorEvento>());
            Ids(so, "exige", exige);
            Ids(so, "some", some);
            Campo(so, "missao").stringValue = missao;
            Campo(so, "comObjetivo").stringValue = comObjetivo;
            so.ApplyModifiedPropertiesWithoutUndo();
            return go.transform;
        }

        static void Ids(SerializedObject so, string campo, string[] ids)
        {
            SerializedProperty p = Campo(so, campo);
            p.arraySize = ids.Length;
            for (int i = 0; i < ids.Length; i++) p.GetArrayElementAtIndex(i).stringValue = ids[i];
        }

        /// <summary>Primitiva sem colisor. giroZ inclina em torno de Z (bracos da forquilha).</summary>
        static void Bloco(PrimitiveType tipo, string nome, Transform pai, Vector3 pos, Vector3 escala, Material m, float giroZ)
        {
            GameObject go = GameObject.CreatePrimitive(tipo);
            go.name = nome;
            go.transform.SetParent(pai, false);
            go.transform.localPosition = pos;
            go.transform.localRotation = Quaternion.Euler(0f, 0f, giroZ);
            go.transform.localScale = escala;
            go.GetComponent<Renderer>().sharedMaterial = m;
            Object.DestroyImmediate(go.GetComponent<Collider>());
        }

        static Color Cor(int r, int g, int b) { return new Color(r / 255f, g / 255f, b / 255f); }
    }
}
