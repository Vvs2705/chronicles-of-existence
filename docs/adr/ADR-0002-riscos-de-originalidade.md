# ADR-0002 — Teste positivo de originalidade como portão obrigatório de arte e personagem

- **ID:** ADR-0002
- **Data:** 2026-09-28
- **Estado:** APROVADO em 2026-09-28 (o Vinicius delegou a decisão: "pode seguir, a melhor opção está com você"). Os pontos de corte numéricos continuam HIPÓTESE A VALIDAR e serão recalibrados na primeira ficha-piloto.
- **Autor do registro:** raia D1 (documentação canônica)
- **Documentos afetados:** GDD Mestre (capítulo de arte), `docs/PROJETO.md`, `CLAUDE.md`, futuros briefings de arte e de NPC

## Contexto

O COE declara inspiração em duas obras: *Hell Mode* (dificuldade como condição de existência) e *Fable* (repercussão moral e reputação). O prompt-mestre (§4) e o dossiê (seção B) já mandam não copiar nomes, personagens, designs, interface, diálogos, cenas, classes e curvas de progressão dessas obras.

**Isso, sozinho, não basta.**

Aplicada como subtração, a regra tira tudo o que lembra a obra de referência e não põe nada no lugar. Subtrair a referência produz vácuo, e vácuo é preenchido pelo default do gerador — que é o aventureiro genérico: um elenco inteiro saído do mesmo template, sem objeto-assinatura e sem linguagem visual própria. Um teste positivo que existe no papel e não roda antes de encomendar arte não evita isso.

O COE está hoje nessa posição: tem regra de subtração, tem paleta declarada (azul profundo, dourado, verde, terracota, turquesa, violeta, marfim), tem signos da Trama (fios entrelaçados, círculos incompletos, pontos conectados), tem dez NPCs de Auren descritos por função e profissão — e não tem, para nenhum deles, resposta à pergunta "o que só este jogo faz assim". Se nada mudar, o COE cai nesse default com custo maior, porque a arte é anime estilizada, categoria em que o default do gerador é ainda mais forte.

## Decisão

Instituir o **teste positivo de originalidade** como portão obrigatório: nenhum personagem, criatura, local, item-assinatura, VFX de magia ou sistema visível do COE entra em produção de arte (concept, Tripo3D, Blender, animação) antes de ser aprovado no teste.

O portão é **positivo**: não pergunta "isto se parece com a referência?", pergunta "o que isto tem que só existe aqui?". A pergunta negativa continua valendo, mas passa a ser insuficiente sozinha.

### As duas perguntas obrigatórias

Para cada personagem e cada sistema visível:

1. **Que necessidade do mundo o originou?** A resposta precisa ser uma condição concreta de Eryndor/Valtheris/Eldoria/Auren — clima, escassez, ofício, medo comunitário, consequência da Primeira Fratura, relação com o Limiar ou com a Trama. "É o ferreiro da vila" não é resposta; é cargo. "Auren perde ferramentas para a umidade do Bosque, e Borin trabalha ligas que não enferrujam, por isso mede tudo duas vezes e desconfia de quem tem pressa" é resposta.
2. **Qual silhueta ou regra só este jogo tem?** Uma forma reconhecível em preto, a 30% de escala, **ou** uma regra de jogo que só esta peça produz. Precisa ser verificável: ou se vê, ou se joga. "Estilo anime estilizado" não é silhueta; é categoria.

### As três perguntas de amarração ao COE

Amarram a peça aos pilares do COE:

3. **Como o Destino de Nascimento, o Grau de Existência ou a Trama o afetam?** O que muda nesta peça entre a Vida Serena e a Vida da Ruptura, ou entre o Grau I e o II.
4. **Qual decisão de jogo ele cria?** Que escolha o jogador toma por causa dele que não tomaria sem ele.
5. **Qual pilar ele reforça?** Viver e crescer, escolher e transformar, ou superar a própria existência — um, nomeado, não os três por educação.

## Critérios de aceite objetivos

O portão é uma ficha de 10 critérios, pontuada 0 / 1 / 2.

| # | Critério | 0 | 1 | 2 |
|---|---|---|---|---|
| C1 | **Gancho em uma frase** com detalhe concreto e tensão | não existe | descreve função ou cargo | detalhe concreto + conflito |
| C2 | **Necessidade do mundo** que o originou (pergunta 1) | nenhuma | genérica, serviria em qualquer RPG | específica de Eryndor/Auren |
| C3 | **Silhueta própria**: reconhecível em preto a 30% de escala | nada | lista de peças de roupa | 3 formas dominantes nomeadas |
| C4 | **Objeto-assinatura com regra**: um objeto que só ele tem e que faz algo no jogo | nenhum | objeto decorativo | objeto + regra de jogo escrita |
| C5 | **Regra ou habilidade exclusiva**: algo que só ele faz e o jogador vê | nada | kit genérico do catálogo | técnica exclusiva especificada |
| C6 | **Voz**: 3 falas que só ele diria (ritmo, vocabulário, tique) | nenhuma | tom descrito em adjetivos | 3 falas escritas |
| C7 | **Contradição visível em ação recorrente** | nenhuma | abstrata na ficha | observável no comportamento |
| C8 | **Relação concreta com o avatar** que muda por destino/origem | nenhuma | uma linha genérica | concreta e ramificada pelos 4 destinos |
| C9 | **Mudança no tempo**: como ele muda no salto temporal dos 5 para os 8 anos | nenhuma | "vai mudar" | 2 batidas marcadas, antes e depois |
| C10 | **Momento de cartaz**: a cena que o vende em 5 segundos | nenhuma | cena implícita | cena escrita e encenável |

### Portões de aprovação

| Portão | Quando | Critério para passar |
|---|---|---|
| **G1 — Ficha** | antes de qualquer concept art | nota ≥ 14/20, **sem nenhum zero** em C2, C3, C4 e C5 |
| **G2 — Concept** | antes de gerar malha 3D (Tripo3D/Blender) | teste cego de silhueta e teste cego de descrição, ambos aprovados |
| **G3 — Malha** | antes de rig, animação e import no Unity | G2 aprovado + licença e proveniência registradas |

### Testes cegos de G2 (procedimento)

1. **Teste de silhueta.** Renderize as silhuetas em preto, a 30% de escala, embaralhadas com pelo menos 3 outras do mesmo elenco. Uma pessoa que leu as fichas precisa identificar corretamente ≥ 4 de 5. Reprovou = o elenco ainda é o mesmo boneco com roupas diferentes.
2. **Teste de descrição.** Descreva a peça em 2 frases, sem nome próprio, a alguém que conhece o gênero e pergunte "de que jogo é isto?". Se a resposta for "poderia ser de qualquer RPG", reprovou.
3. **Teste do troco.** Troque o personagem por outro do mesmo elenco na cena em que ele aparece. Se a cena continuar funcionando sem reescrita, os dois são a mesma pessoa e pelo menos um reprovou.

### Regra de encomenda

Nenhum briefing de arte sai sem a ficha aprovada em G1 anexada. Um briefing que só diz o que **não** fazer ("evitar uniformes", "não copiar a referência", "sem coroas") é um briefing reprovado por construção: é subtração. Toda regra negativa no briefing precisa vir acompanhada da regra positiva correspondente.

### Alvos do portão no vertical slice

Os dez NPCs de Auren (Mara, Daren, Borin, Lysa, Tovin, Eira, Nilo, Sera, Oren, Maelis), o avatar nas três bases corporais, Aethron, o símbolo do Limiar, as três casas acessíveis e três estruturas públicas de Auren, e o VFX das magias iniciais.

### Nota específica sobre o avatar

O risco do avatar é o título aparecer na logline e sumir na jogabilidade: um conceito de protagonista sem nenhuma mecânica que faça o jogador *senti-lo*. No COE esse risco é maior, porque o avatar é deliberadamente pouco caracterizado — a vida dele é escrita pelas escolhas do jogador. Para o avatar, C6 (voz) e C7 (contradição) não se aplicam; em compensação **C4, C5 e C8 são obrigatórios em nota 2**, e a pergunta a responder é: qual gesto, objeto ou regra o jogador opera que só existe porque este personagem escolheu o próprio nascimento no Limiar. Sem essa resposta, o avatar volta a ser o aventureiro genérico.

## Alternativas rejeitadas

| Alternativa | Por que foi rejeitada |
|---|---|
| Manter só a regra de não copiar (status quo) | Subtração sem substituição produz o default do gerador: aventureiro de RPG genérico. |
| Gerar arte primeiro e avaliar depois | O custo de reprovar depois é o custo de gerar tudo de novo. |
| Contratar direção de arte humana e delegar o problema | Não resolve: o briefing que o artista recebe continua sendo subtração. O teste positivo é o que transforma o briefing em algo executável. Não impede contratar depois. |
| Portão só para os personagens principais | O defeito típico é o elenco inteiro sair do mesmo template. Um portão parcial produz três personagens fortes num elenco genérico, o que é pior de olhar do que um elenco uniformemente modesto. |
| Rubrica maior, com mais critérios | Dez critérios já são a maior ficha que alguém preenche de verdade para 10 NPCs. Rubrica que não é preenchida não é portão. |

## Consequências

**Positivas**
- O briefing de arte passa a ser executável: o gerador (Tripo3D, Meshy, IA de imagem) recebe formas, objetos e regras, não proibições.
- A reprovação acontece na ficha, que custa uma hora de escrita, e não no lote de imagens.
- C4 e C5 forçam a arte a nascer amarrada à mecânica, o que reduz o risco de um elenco bonito e mecanicamente inerte.
- O teste do troco expõe cedo o template compartilhado, que é o defeito estrutural de um elenco genérico.

**Negativas e custos assumidos**
- Atraso real: dez NPCs × ficha de 10 critérios é trabalho de escrita antes de qualquer imagem. É o custo de não refazer o lote.
- Risco de burocracia: se a ficha virar formulário preenchido para passar, o portão morre. A defesa são os testes cegos de G2, que não se fraudam no papel.
- Subjetividade residual: C1, C6 e C10 dependem de julgamento. Mitigação: nota dada por quem não escreveu a ficha.
- O portão não é garantia jurídica. Passar no teste positivo não é parecer de propriedade intelectual; o dossiê e o prompt-mestre continuam exigindo revisão especializada antes de uso comercial.

## O que precisa acontecer para isto sair de PROPOSTA

1. Aprovação do idealizador de que o portão é obrigatório e de que os pontos de corte (≥ 14/20, sem zero em C2–C5) valem.
2. Registro do portão no GDD Mestre (capítulo de arte) e uma linha em `CLAUDE.md` para que nenhum agente encomende arte sem a ficha.
3. Uma ficha-piloto preenchida para um NPC — a recomendação é **Borin**, que já tem ofício e é o mais fácil de amarrar a uma necessidade concreta de Auren — para calibrar se a rubrica é aplicável antes de exigir dez.

## Pendências

- A rubrica não foi testada em nenhum personagem do COE. **Não há evidência de que ela seja aplicável nesta forma**; a ficha-piloto existe para descobrir isso.
- Os pontos de corte (14/20, ≥ 4 de 5 no teste de silhueta) são hipóteses, não medições.
- Falta definir quem pontua. Com equipe de uma pessoa, "quem não escreveu a ficha" é um problema aberto — uma opção é pontuar em sessão separada, com no mínimo um dia de intervalo.
