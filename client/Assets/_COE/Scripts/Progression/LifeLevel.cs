namespace COE
{
    /// <summary>Nivel de Vida: MARCADOR HISTORICO, nao fonte de poder. C# PURO.
    ///
    /// O QUE ELE E (dossie secao D, GDD v1.2 cap. 03 "Nivel de Vida vs. Nivel de Grau"): "marcador geral e
    /// historico", conta quantos marcos de vida o personagem atravessou, e NAO reinicia numa ascensao.
    /// O Nivel de Grau -- esse sim mede dominio dentro do grau atual e reinicia -- e outro sistema, fora do
    /// primeiro slice (dossie secao E).
    ///
    /// POR QUE ELE NAO E MULTIPLICADOR, escrito aqui para nao virar um de novo por descuido:
    ///   1. O dossie proibe: "nao multiplica-los automaticamente em progressao exponencial"; o GDD repete
    ///      "nao multiplica diretamente todos os atributos".
    ///   2. Se multiplicasse, o salto temporal viraria a fonte de poder mais barata do jogo: pular idade e
    ///      uma confirmacao de dialogo, enquanto ganhar um atributo custa varias atividades desafiadoras.
    ///      O jogador otimizaria pulando a infancia -- exatamente o conteudo que o slice existe para provar.
    ///   3. Multiplicador composto com 5 fases torna qualquer numero de dano impossivel de balancear: cada
    ///      ajuste de curva de atributo teria de ser refeito para cada nivel.
    ///
    /// COMO ISSO E MANTIDO VERDADE: esta classe nao tem nenhum membro que devolva fator, escala ou
    /// porcentagem, e nenhum calculo do COE recebe lifeLevel como entrada. ProgressionMasteryTests prova as
    /// duas coisas -- por reflexao (nenhum float/double aqui) e por comportamento (praticar com lifeLevel 1
    /// e com lifeLevel 99 rende exatamente o mesmo).
    ///
    /// Para que serve, entao: gatilho narrativo (NPC reconhece quem o personagem virou), requisito de marco
    /// e de ascensao futura, e texto de ficha. Consequencia, nao estatistica.</summary>
    public static class LifeLevel
    {
        public const int Inicial = 1;

        /// <summary>Um marco de vida atravessado = um nivel. Soma, nunca produto; +1 fixo, nunca uma curva.
        /// ponytail: linear de proposito. Se um dia um marco tiver de valer mais que outro, o peso vai no
        /// MarcoDeIdade (definicao) e este metodo passa a receber esse peso -- nunca em tabela por nivel.</summary>
        public static int AposMarco(int atual)
        {
            return (atual < Inicial ? Inicial : atual) + 1;
        }
    }
}
