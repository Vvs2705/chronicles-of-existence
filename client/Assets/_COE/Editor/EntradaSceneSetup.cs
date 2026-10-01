using UnityEditor;
using UnityEngine;

namespace COE.EditorTools
{
    /// <summary>T012 — Entrada da partida na Bootstrap (so nela; Auren nao chama): tela de nascimento quando
    /// SaveState.Current.birth.destinyId esta vazio, senao rota para a cena salva; aviso de save de versao mais nova.
    /// Contrato do coordenador: assinatura fixa; o corpo e da raia dona deste arquivo.</summary>
    public static class EntradaSceneSetup
    {
        /// <summary>Poe o EntryFlow na cena ATIVA e liga o PlayerInputReader que o Populate criou (a tela o desliga
        /// enquanto esta aberta). Busca aqui no gerador, nunca em runtime. Estoura sem o input: Montar antes do Populate
        /// e erro de gerador, nao caso a tolerar.</summary>
        public static void Montar()
        {
            PlayerInputReader input = Object.FindFirstObjectByType<PlayerInputReader>();
            if (input == null) throw new System.Exception("EntradaSceneSetup.Montar sem PlayerInputReader: chame depois do BootstrapSceneBuilder.Populate.");

            EntryFlow entrada = new GameObject("Entrada").AddComponent<EntryFlow>();
            var so = new SerializedObject(entrada);
            SerializedProperty p = so.FindProperty("input");
            if (p == null) throw new System.Exception("EntryFlow nao tem o campo serializado 'input'.");
            p.objectReferenceValue = input;
            SerializedProperty l = so.FindProperty("limiar");
            if (l == null) throw new System.Exception("EntryFlow nao tem o campo serializado 'limiar'.");
            l.objectReferenceValue = PalcoDoLimiar();
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        public const string NomeSimbolo = "Simbolo";
        static readonly Color AzulProfundo = new Color(0x25 / 255f, 0x38 / 255f, 0x50 / 255f);   // GDD: menus e elementos arcanos
        static readonly Color Dourado = new Color(0xD6 / 255f, 0xB3 / 255f, 0x6A / 255f);        // GDD: Limiar e ascensao

        /// <summary>B01: o Limiar como palco DESLIGADO na propria Bootstrap, 300 m abaixo da area de treino: camera propria
        /// (por cima da do jogo, fundo Azul profundo) e o simbolo da Trama enquadrado acima do painel de fala. O EntryFlow
        /// liga o palco so na tela do Limiar. ponytail: sem cena TheLiminalRealm nem modelo de Aethron; cena propria quando o
        /// Limiar crescer (Aethron em cena, efeitos), com a mesma tela de fala.</summary>
        static GameObject PalcoDoLimiar()
        {
            var palco = new GameObject("Limiar");
            palco.transform.position = new Vector3(0f, -300f, 0f);

            Transform simbolo = new GameObject(NomeSimbolo).transform;
            simbolo.SetParent(palco.transform, false);
            GameObject fonte = Prototipos.Carregar("simbolo_limiar");
            if (fonte != null) Prototipos.Instanciar(fonte, "simbolo_limiar", simbolo, Prototipos.Altura("simbolo_limiar"), Vector2.zero);
            else
            {
                // greybox: disco dourado de pe, de frente para a camera
                GameObject disco = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                disco.name = "disco";
                Object.DestroyImmediate(disco.GetComponent<Collider>());
                disco.transform.SetParent(simbolo, false);
                disco.transform.localPosition = new Vector3(0f, 0.75f, 0f);
                disco.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                disco.transform.localScale = new Vector3(1.2f, 0.05f, 1.2f);
                disco.GetComponent<Renderer>().sharedMaterial = LookSetup.NovoMaterial("COE_Limiar_Simbolo", Dourado);
            }

            var cam = new GameObject("CameraLimiar").AddComponent<Camera>();
            cam.transform.SetParent(palco.transform, false);
            // FOV 45 a 3,4 m: altura visivel 2,8 m; olhando reto a 0,25 m, o simbolo (0 a 1,5 m) ocupa de 42% a 95% da
            // tela, acima do painel de fala (40% de baixo).
            cam.transform.localPosition = new Vector3(0f, 0.25f, -3.4f);
            cam.transform.LookAt(simbolo.position + Vector3.up * 0.25f);
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = AzulProfundo;
            cam.fieldOfView = 45f;
            Camera principal = Camera.main;
            cam.depth = (principal != null ? principal.depth : 0f) + 10f;

            palco.SetActive(false);
            return palco;
        }
    }
}
