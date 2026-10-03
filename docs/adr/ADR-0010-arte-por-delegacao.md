# ADR-0010 — Decisões de arte por delegação (portão do ADR-0002 em andamento)

- **ID:** ADR-0010
- **Data:** 2026-10-03
- **Estado:** APROVADO por delegação. Em 2026-10-03 o Vinicius escreveu "sempre faça o mais recomendado em cada situação, pode seguir" e avisou que ficaria horas fora ("preciso que apenas faça sem mim"). Todas as decisões abaixo são reversíveis e ele pode revogar qualquer uma ao voltar.
- **Depende de:** ADR-0002, ADR-0007, ADR-0008
- **Documentos afetados:** `docs/arte/fichas/**`, `docs/arte/PROVENIENCIA.md`, `docs/design/SLICE_A_PRIMEIRA_EXISTENCIA.md` (§1.1 e B15), `content/quests/q06_o_segredo_do_ferreiro.json`, `docs/PROJETO.md` §6

## Contexto

A ficha-piloto do Borin tinha parecer do Art Director (19/20, aprovado com duas condições) e esperava só a aprovação final do idealizador. O resto do elenco do slice não tinha ficha. Sem G1 não há concept, sem concept não há G2, e a arte final (ADR-0002) não anda. O ADR-0002 já previa o problema de "quem pontua" numa equipe de uma pessoa.

## Decisões

1. **G1 do Borin aprovado**, com a nota do parecer (19/20) e a correção de C3 que o próprio parecer pediu: o aro sai do contorno do corpo (preso por um suporte rígido ao lado da coxa; chapa de ferro de 40 mm de largura, de face para a frente, porque a barra de 20 mm sumia a 30% — `docs/arte/fichas/ELENCO.md`, arbitragem 2) e a terceira forma passa a ser "crânio raspado sobre pescoço projetado"; a pala fica como detalhe.
2. **Conteúdo da q06 aprovado** (condição (a) do parecer): o segredo é que a vista de Borin está falhando e ele confere com o polegar; o passo "ler o risco" de `ajudar_borin` vale como desenho da missão. A implementação (texto e interação) é trabalho da T012/T014, não deste ADR.
3. **Espada do B15:** com `confianca_de_borin`, Borin entrega a espada marcada na `ferraria`; sem a flag, a espada comum vem do próprio treino, no `posto_guarda` (Tovin). Estatística idêntica nos dois casos. O SLICE §1.1 e o B15 deixam de ter a leitura "entrega sem condição".
4. **A marca do jogador** (pendência 4 do parecer, para o G2): a prova do jogador no aro é a única de **cobre** (terracota #A86D52); as outras são de ferro escuro. A espada marcada leva uma plaqueta de cobre rebitada no punho. Cor e não letra: texto de 4 cm não se lê no celular.
5. **Marfim em roupa comum:** linho cru em marfim #E9DEC6 fica liberado em roupa de NPC (extensão da paleta do GDD cap. 09, que reservava o marfim a textos e pergaminhos).
6. **Como o G1 anda enquanto a delegação vale:**
   - quem escreve a ficha não pontua; a nota é de um agente Art Director **separado**, que não escreveu nenhuma das fichas que pontua;
   - ficha com ≥ 14/20 e nenhum zero em C2–C5 fica **aprovada por delegação** e marcada assim no próprio arquivo; abaixo disso, volta para reescrita;
   - o idealizador pode revogar qualquer aprovação; uma ficha revogada derruba o G2 e o G3 que dependiam dela.
7. **G2 por delegação:** o teste cego de silhueta (`client/tools/silhueta.py`) e o de descrição são aplicados por um agente que leu as fichas e recebe só a folha embaralhada; o gabarito fica com o coordenador. O teste do troco é feito por escrito pelo mesmo agente.
8. **Tripo Bridge** (decisão 10 do `PROJETO.md` §6): fica no projeto, só no Editor (assembly `Editor`, não entra no player). O repositório citado no `package.json` (`github.com/tripo3d/Tripo3D-Unity-Bridge`) não é público em 2026-10-03 (HTTP 404) e o pacote não traz LICENSE: o uso fica coberto pelos termos do Tripo já registrados na `PROVENIENCIA.md` §2. A `websocket-sharp.dll` (250 KB) fica no git comum: o LFS custa cota de banda a cada clone e a regra V27 só cobre arte.

9. **Acervo de concept no git** (decisão 8 do `PROJETO.md` §6): copiado para `arte/referencias/acervo/` e versionado pelo LFS, com os 235 SHA-256 conferidos contra o `ACERVO.csv`. Motivo: era o único exemplar (o zip duplicado foi apagado na limpeza de disco de 2026-10-03). Custo: 0,46 GB dos 10 GiB de armazenamento LFS do GitHub Free (docs.github.com, "Git LFS", consultado em 2026-10-03). Continua sendo referência de direção, não entrada de gerador fora do ADR-0008.

## Fica com o idealizador

- Revogar ou confirmar as aprovações por delegação deste ADR.
- Apagar os originais do acervo na raiz do checkout principal (`imagens/`, `documentos/`, `INVENTARIO.csv`, `LEIA-ME.md`, `INDEX.html`), agora duplicados no repositório. Apagar é com ele.

## Adendo — 2026-10-03 (W3): condição (b) das fichas `lysa` e `maelis`

- **Estado:** APROVADO por delegação, com a mesma autorização do topo deste ADR ("sempre faça o mais recomendado em cada situação, pode seguir"). Reversível: o idealizador pode revogar qualquer item.
- **Quem decidiu:** a raia W3, como dono do conteúdo de missão (persona Quest Content Designer), porque as duas fichas esperavam esse dono (condição (b) da conferência final de cada uma). A reconferência das fichas é do Art Director, na próxima leva (§6).
- **Código:** nada foi codificado aqui. O que estas decisões pedem está em `docs/arte/fichas/ELENCO.md`, "Pendências de código que as fichas criam".

10. **q05 — chapéu emborcado e "chegar devagar" (ficha `lysa`, C4 e C5): aprovados, com dois ajustes.**
    - Chapéu: com a q05 em andamento e `buscar_ajuda` cumprido, o chapéu fica emborcado sobre o bicho na `entrada_bosque`, sem colisor, e a Lysa anda sem ele. Quando a q05 sai de "em andamento" (concluída, ou encerrada pelo salto), o chapéu volta. A regra lê só o estado da missão, que o save já guarda. O HUD continua dizendo o objetivo por texto.
    - "Chegar devagar": correr a menos de 4 m do chapéu zera a espera; ficar parado a até 1,5 m dele por ~3 s acalma o bicho, e só então a opção de `tratar_o_animal` aparece na conversa com Lysa ou Tovin. Ajuste 1: a espera é ficar parado, sem segurar botão. Ajuste 2: "bicho calmo" é estado de cena, fora do save.
    - Por quê: a q05 é "conhecimento e compaixão" (GDD cap. 07), e esta é a única regra do slice que mede o jeito de chegar. O chapéu no chão marca o objetivo no mundo sem seta, o que serve o celular em paisagem. Não há estado novo no save nem recompensa nova; falhar só repete, a q05 continua opcional e alcançável em qualquer período (ADR-0007, consequências). Ficar parado é mais simples no toque do que segurar um botão.
11. **q07 — a assinatura no livro de Maelis (ficha `maelis`, C5): aprovada, como desfecho da missão.**
    - `perguntar_na_vila` passa a ser o objetivo de decisão da q07, como o `decidir` da q04. Só na conversa com Maelis, dois botões: traçar o sinal ou fazer um risco. Cada um grava um desfecho e cumpre o objetivo; Eira e Oren continuam dando as pistas (B09), mas não fecham mais o objetivo.
    - Ids: `evento.q07_assinou_com_o_circulo` (o da ficha, aprovado) e `evento.q07_assinou_com_um_risco` (o par, novo). Seguem a convenção do `CLAUDE.md` (`evento.` + `snake_case`, como `evento.q04_promessa_cumprida`). Exatamente um, nunca os dois; Maelis testemunha os dois.
    - Por quê: reaproveita o mecanismo que já existe (`QuestDef.Desfechos`, `MissaoNaConversa.Decisoes`, erros `DesfechoJaDecidido` e `DesfechoPendente`) em vez de inventar um tipo de regra. O par é necessário porque esse mecanismo grava um evento por botão. A escolha não concede nada (`marco.desaparecimento` é o mesmo nos dois) e dá ao B09 uma decisão do jogador que volta aos 8. Save antigo com `perguntar_na_vila` já cumprido conclui a q07 sem desfecho: missão central não trava.

Também nesta leva, sem ser decisão de missão: a faixa da canela direita ficou com a Mara pela regra de desempate, e o estojo do Tovin foi para a canela esquerda (`ELENCO.md`, Arbitragem 3).
