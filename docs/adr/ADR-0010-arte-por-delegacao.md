# ADR-0010 — Decisões de arte por delegação (portão do ADR-0002 em andamento)

- **ID:** ADR-0010
- **Data:** 2026-10-03
- **Estado:** APROVADO por delegação. Em 2026-10-03 o Vinicius escreveu "sempre faça o mais recomendado em cada situação, pode seguir" e avisou que ficaria horas fora ("preciso que apenas faça sem mim"). Todas as decisões abaixo são reversíveis e ele pode revogar qualquer uma ao voltar.
- **Depende de:** ADR-0002, ADR-0007, ADR-0008
- **Documentos afetados:** `docs/arte/fichas/**`, `docs/arte/PROVENIENCIA.md`, `docs/design/SLICE_A_PRIMEIRA_EXISTENCIA.md` (§1.1 e B15), `content/quests/q06_o_segredo_do_ferreiro.json`, `docs/PROJETO.md` §6

## Contexto

A ficha-piloto do Borin tinha parecer do Art Director (19/20, aprovado com duas condições) e esperava só a aprovação final do idealizador. O resto do elenco do slice não tinha ficha. Sem G1 não há concept, sem concept não há G2, e a arte final (ADR-0002) não anda. O ADR-0002 já previa o problema de "quem pontua" numa equipe de uma pessoa.

## Decisões

1. **G1 do Borin aprovado**, com a nota do parecer (19/20) e a correção de C3 que o próprio parecer pediu: o aro sai do contorno do corpo (preso por um suporte rígido ao lado da coxa, barra de 20 mm) e a terceira forma passa a ser "crânio raspado sobre pescoço projetado"; a pala fica como detalhe.
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
