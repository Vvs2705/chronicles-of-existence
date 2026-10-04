using System;

namespace COE
{
    /// <summary>sceneId (estavel, snake_case, vai no save) -> nome da cena no Build Settings (Prompt Mestre §14). Uma linha
    /// por cena do slice. Ler aceita o valor antigo: ate 2026-10-04 o jogo gravava o NOME da cena ("Auren") no sceneId.
    /// Gravar normaliza para o id (GameSession.Posicao recebe CenaCatalogo.Id). A ancora continua sendo o contrato de onde
    /// o jogador entra (AnchorSpawn). ponytail: tabela fixa; cena nova do slice entra com uma linha (e o teste de editor
    /// confere que ela esta no Build Settings).</summary>
    public static class CenaCatalogo
    {
        /// <summary>{ id, nome da cena }.</summary>
        public static readonly string[][] Cenas =
        {
            new[] { "auren", "Auren" },
        };

        /// <summary>Nome da cena do id (ou do nome antigo), sem diferenciar caixa; null se nao esta na tabela.</summary>
        public static string Nome(string sceneId)
        {
            if (string.IsNullOrEmpty(sceneId)) return null;
            foreach (string[] c in Cenas)
                if (string.Equals(c[0], sceneId, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(c[1], sceneId, StringComparison.OrdinalIgnoreCase)) return c[1];
            return null;
        }

        /// <summary>Id para gravar a partir do nome da cena; cena fora da tabela (area de treino de desenvolvimento) = "".</summary>
        public static string Id(string nomeDaCena)
        {
            foreach (string[] c in Cenas)
                if (string.Equals(c[1], nomeDaCena, StringComparison.Ordinal)) return c[0];
            return "";
        }
    }
}
