namespace COE
{
    /// <summary>Altura de corpo do COE, em metros, pes em y=0. Fonte unica para capsula e camera (T002),
    /// cena (T008), placeholder (client/tools/placeholder_humanoid.py) e pipeline de arte (docs/arte/PIPELINE.md).
    /// O slice comeca aos 5 anos e salta para os 8 (dossie §D); adulto e a escala dos NPCs.
    /// ponytail: tres alturas fixas (mediana OMS aos 5 e aos 8 anos; adulto 1,75 m) - hipotese v0, calibrar no
    /// playtest. Curva por idade so quando houver salto alem dos 8 anos.</summary>
    public static class BodyScale
    {
        public const float Crianca5 = 1.10f;
        public const float Crianca8 = 1.28f;
        public const float Adulto = 1.75f;
    }
}
