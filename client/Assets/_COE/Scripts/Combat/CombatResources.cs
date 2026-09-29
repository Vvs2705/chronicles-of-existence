namespace COE
{
    /// <summary>Poco de recurso (Vigor ou Mana): gasta, espera, regenera. C# PURO (sem UnityEngine): o tempo
    /// chega por Tick(dt), entao a regra e testavel em EditMode sem cena.
    ///
    /// POR QUE O ATRASO EXISTE: sem ele a regeneracao paga o proximo golpe no mesmo segundo e o custo vira
    /// enfeite. O atraso reinicia a CADA gasto — quem martela o botao nunca regenera.
    /// Vida NAO mora aqui: ela ja e do Health (MonoBehaviour, i-frames e postura dependem de cena).</summary>
    public class ResourcePool
    {
        public readonly float Max;
        public readonly float RegenPorSegundo;
        public readonly float AtrasoAposGasto;

        public float Atual { get; private set; }
        public float Fracao { get { return Max > 0f ? Atual / Max : 0f; } } // 0..1 para a barra do HUD

        float espera;

        public ResourcePool(float max, float regenPorSegundo, float atrasoAposGasto)
        {
            Max = max > 0f ? max : 0f;
            RegenPorSegundo = regenPorSegundo > 0f ? regenPorSegundo : 0f;
            AtrasoAposGasto = atrasoAposGasto > 0f ? atrasoAposGasto : 0f;
            Atual = Max;
        }

        public bool Tem(float custo) { return custo <= 0f || Atual >= custo; }

        /// <summary>Gasta tudo ou nada: sem saldo, a acao nao sai (e por isso devolve bool).</summary>
        public bool TryGastar(float custo)
        {
            if (custo <= 0f) { Marcar(); return true; }
            if (Atual < custo) return false;
            Atual -= custo;
            Marcar();
            return true;
        }

        /// <summary>Drena o que houver (bloqueio absorvendo golpe). Devolve quanto saiu de fato.</summary>
        public float Drenar(float quanto)
        {
            if (quanto <= 0f) return 0f;
            float saiu = quanto < Atual ? quanto : Atual;
            Atual -= saiu;
            Marcar();
            return saiu;
        }

        public void Tick(float dt)
        {
            if (dt <= 0f) return;
            if (espera > 0f)
            {
                espera -= dt;
                if (espera > 0f) return;
                dt = -espera;    // sobra do frame ja conta como regeneracao
                espera = 0f;
            }
            if (Atual >= Max) return;
            Atual += RegenPorSegundo * dt;
            if (Atual > Max) Atual = Max;
        }

        public void Encher() { Atual = Max; espera = 0f; }

        void Marcar() { espera = AtrasoAposGasto; }
    }

    /// <summary>Os dois recursos gastaveis do treino. Concentracao fica de FORA de proposito
    /// (dossie §F e GDD v1.2 cap. 05: "Concentracao apenas em mecanicas futuras que justifiquem uma barra
    /// adicional"). Numeros em CombatMoves; esta classe so junta os dois pocos e o Tick.</summary>
    public class CombatResources
    {
        public readonly ResourcePool Vigor;
        public readonly ResourcePool Mana;

        public CombatResources()
            : this(CombatMoves.VigorMaxV0, CombatMoves.ManaMaxV0) { }

        public CombatResources(float vigorMax, float manaMax)
        {
            Vigor = new ResourcePool(vigorMax, CombatMoves.VigorRegenV0, CombatMoves.VigorAtrasoV0);
            Mana = new ResourcePool(manaMax, CombatMoves.ManaRegenV0, CombatMoves.ManaAtrasoV0);
        }

        public void Tick(float dt) { Vigor.Tick(dt); Mana.Tick(dt); }

        public void Encher() { Vigor.Encher(); Mana.Encher(); }
    }
}
