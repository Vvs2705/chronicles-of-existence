using System;

namespace COE
{
    /// <summary>Escolha de nascimento do jogador: ESTADO, nao definicao. E exatamente o DTO do
    /// CONTRATO_T003_T004 secao 2 -- T004 grava este bloco no save sem interpretar, T003 e quem valida.
    /// Nenhum campo a mais: DestinySystemTests.BirthChoice_TemExatamenteOsQuatroCamposDoContrato quebra
    /// se alguem acrescentar um.
    ///
    /// confirmedAtUtc > 0 significa escolha CONFIRMADA E PERMANENTE (ADR-0004, invariante 1). A partir
    /// dai DestinySystem.Confirmar recusa qualquer troca. Os campos sao publicos e mutaveis porque
    /// JsonUtility exige; a imutabilidade e de API, nao de campo. Save editado a mao: id inexistente e
    /// detectado por DestinySystem.Validar no carregamento; troca entre ids validos nao (ver Validar).</summary>
    [Serializable]
    public class BirthChoice
    {
        public string destinyId;      // um dos 4 ids de DestinyCatalog.Destinos
        public string originId;       // um dos 3 ids de DestinyCatalog.Origens
        public string characterName;  // nome do avatar, ja normalizado por DestinySystem
        public long confirmedAtUtc;   // ticks UTC da confirmacao; 0 = ainda nao confirmado
    }
}
