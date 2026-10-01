# ADR-0004 — Destino de Nascimento, Grau de Existência e dificuldade são três coisas separadas

- **ID:** ADR-0004
- **Data:** 2026-09-28
- **Estado:** APROVADO (decisão delegada pelo Vinicius ao coordenador)
- **Depende de:** ADR-0003
- **Documentos afetados:** persona do agente diretor, `docs/direcao/PROMPT_MESTRE_AGENTE_v1_0.md` (§5.4), futuros textos de interface, `CLAUDE.md`

## Contexto

O dossiê §C é explícito: a proposta de "trocar de dificuldade" foi **descartada como exploit** — começar na vida fácil, acumular poder e trocar para a extrema para colher a recompensa maior. A correção registrada lá é separar **três sistemas**:

1. **Destino de Nascimento** — permanente, muda família, oportunidades, acontecimentos, diálogos e circunstâncias;
2. **Grau de Existência** — evolutivo, conquistado por domínio + marco + provação + estabilização;
3. **Assistências / dificuldade de combate** — ajustáveis a qualquer momento, sem reescrever origem.

Porém dois documentos operacionais escrevem **"Vida Serena (fácil)"** e **"Vida da Ruptura (extrema)"**: o `PROMPT_MESTRE_AGENTE_v1_0.md` §5.4 e a persona `AGENTE AI GAME DEVELOPMENT DIRECTOR.md` (bloco "Nascimento"). Isso equipara destino a dificuldade exatamente onde o agente vai buscar a regra na hora de especificar código — ou seja, o exploit já descartado voltaria pela porta da implementação.

## Decisão

**Destino não é dificuldade, e nenhum texto do projeto pode rotular destino com nome de dificuldade.**

- A persona `AGENTE AI GAME DEVELOPMENT DIRECTOR.md` foi corrigida no bloco "Nascimento": os parênteses "(fácil)" e "(extrema)" saíram e entrou a separação dos três sistemas, com referência a este ADR.
- A cópia do prompt-mestre em `docs/direcao/` **fica intacta** (é fonte entregue pelo Vinicius); este ADR é a errata que prevalece sobre ela, pela ordem de precedência do ADR-0003.
- Nomes de interface para os quatro destinos ficam **em aberto**, mas com uma restrição: não podem ser adjetivos de dificuldade. Devem descrever a **circunstância de vida**, não o desafio mecânico. *(Nota de 2026-09-30: o rótulo do destino `dificil` ficou "Vida Árdua"; o id não muda. [ADR-0007](ADR-0007-decisoes-da-leva-a.md) §2.)*

## Invariantes que a implementação precisa respeitar (portão de teste)

1. `DestinySystem` grava o destino uma única vez; não existe caminho de código, menu ou item que o troque depois. Teste negativo obrigatório. Save editado à mão: o jogo **detecta id inválido e registra**; não promete anti-cheat em save local. *(Nota de 2026-09-30: reescrito pelo [ADR-0007](ADR-0007-decisoes-da-leva-a.md) §7; a redação original incluía "save editado" entre os caminhos que não trocam o destino.)*
2. Recompensa vinculada a destino/ascensão é **idempotente por ID de evento**: reload ou repetição não concede de novo.
3. Mudar assistência de combate **nunca** altera destino, origem, Grau, recompensa concedida ou histórico narrativo. Teste que muda a assistência no meio da sessão e confere que o save não mudou nesses campos.
4. A Vida da Ruptura não pode ser a única rota ao maior poder, nem o "final verdadeiro": trajetórias diferentes entregam especializações diferentes (prompt-mestre §8).

## Alternativas rejeitadas

1. **Manter os rótulos e "confiar que ninguém implementa assim".** Rejeitada: regra que mora só em prosa não sobrevive ao código.
2. **Editar a cópia do prompt-mestre.** Rejeitada pela mesma razão do ADR-0003: fonte do Vinicius não se reescreve em silêncio; erra-se por errata, não por apagamento.
3. **Renomear os destinos agora.** Rejeitada: nome de interface é decisão do Vinicius e não bloqueia nada hoje.

## Consequências

- O agente diretor deixa de propagar a equiparação em cada nova especificação.
- Os quatro invariantes acima entram como critério de aceite da tarefa **T003 (Destiny + Origin)** do backlog.
- Risco residual: qualquer documento novo copiado de um chat antigo pode trazer os rótulos de volta. Quem revisar deve procurar por "(fácil)" e "(extrema)" antes de aceitar.
