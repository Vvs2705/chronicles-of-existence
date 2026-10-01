using UnityEngine;

namespace COE
{
    /// <summary>Faixa grafica (ADR-0009): o MESMO jogo do celular simples ao avancado. O valor e o indice do nivel no
    /// QualitySettings (ProjectSetup cria os tres, cada um com o seu asset do URP: escala de render e sombra).</summary>
    public enum FaixaQualidade { Baixa = 0, Media = 1, Alta = 2 }

    /// <summary>Regra (C# puro) e aplicacao da faixa. Detecta pela RAM na primeira abertura; o jogador troca em
    /// Configuracoes. Baixa: render a 70%, sem sombra, sem pos-processamento, sem contorno toon, pele com 2 ossos.
    /// Media: 85%, sombra curta. Alta: o URP_Base. Numeros [PROPOSTA] do ADR-0009, a medir no aparelho.</summary>
    public static class Qualidade
    {
        /// <summary>Ate ~3 GB de RAM (o sistema reporta um pouco menos que o nominal: 3 GB = ~2,8 GB).</summary>
        public const int RamBaixaAteMb = 3500;
        /// <summary>Ate ~5 GB; 6 GB ou mais (reporta ~5,6 GB) = Alta.</summary>
        public const int RamMediaAteMb = 5400;

        /// <summary>Variavel global do COE/Toon: 1 = passe de contorno jogado para fora da tela.</summary>
        public const string SemContornoGlobal = "_COE_SemContorno";

        static readonly string[] ids = { "baixa", "media", "alta" };

        /// <summary>Faixa pela RAM do aparelho (SystemInfo.systemMemorySize, MB). Leitura invalida (0) = Media.</summary>
        public static FaixaQualidade Detectar(int ramMb)
        {
            if (ramMb <= 0) return FaixaQualidade.Media;
            if (ramMb <= RamBaixaAteMb) return FaixaQualidade.Baixa;
            if (ramMb <= RamMediaAteMb) return FaixaQualidade.Media;
            return FaixaQualidade.Alta;
        }

        public static string Id(FaixaQualidade f) { return ids[(int)f]; }

        public static bool TryParse(string id, out FaixaQualidade f)
        {
            int i = System.Array.IndexOf(ids, id);
            f = i < 0 ? FaixaQualidade.Media : (FaixaQualidade)i;
            return i >= 0;
        }

        /// <summary>Contorno e pos-processamento em qualquer lugar; o nivel do QualitySettings (escala de render, sombra,
        /// pele) so no jogo compilado: no editor, trocar o nivel gravaria o QualitySettings.asset e sujaria o git.
        /// posProcessamento = o Volume global da cena (pode ser nulo: a Bootstrap nao tem).</summary>
        public static void Aplicar(FaixaQualidade f, Behaviour posProcessamento)
        {
            if (!Application.isEditor && QualitySettings.names.Length > (int)f) QualitySettings.SetQualityLevel((int)f, true);
            Shader.SetGlobalFloat(SemContornoGlobal, f == FaixaQualidade.Baixa ? 1f : 0f);
            if (posProcessamento != null) posProcessamento.enabled = f != FaixaQualidade.Baixa;
        }
    }
}
