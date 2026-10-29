# Chronicles of Existence — Art Bible (v0.1, 2026-10-29)

Uma página com a direção de arte em vigor. Não decide nada novo de personagem: consolida o que já está aprovado e aponta para o detalhe. Onde houver conflito, vale o documento citado.

- **Aprovado:** estilo anime estilizado, toon no URP ([ADR-0008](adr/ADR-0008-prototipo-de-estetica.md) §4); portões G1 → G2 → G3 antes de qualquer malha final ([ADR-0002](adr/ADR-0002-riscos-de-originalidade.md)); paleta do GDD cap. 09; arte por delegação ([ADR-0010](adr/ADR-0010-arte-por-delegacao.md)); três faixas gráficas sobre os mesmos assets ([ADR-0009](adr/ADR-0009-do-celular-simples-ao-avancado.md)).
- **Detalhe técnico:** [`arte/PIPELINE.md`](arte/PIPELINE.md) (eixos, escala, orçamento, rig, clips, validador). Origem e licença: [`arte/PROVENIENCIA.md`](arte/PROVENIENCIA.md). Fichas: [`arte/fichas/`](arte/fichas/).
- **Proposta nesta versão (reversível, marcada ⚑):** as fontes da interface e as regras de leitura no celular. Ficam até o idealizador trocar.

## 1. Pilares visuais

1. **A criança é pequena num mundo adulto.** O mundo é feito na escala adulta (porta de 2,2 m, parede de 3 m) e a câmera mostra isso. Crescer se vê no corpo (1,10 m aos 5, 1,28 m aos 8), não num número.
2. **Auren é quente; o Limiar e a Trama são frios.** Vila em terracota, verde e marfim; magia, Limiar e anomalia em turquesa, dourado e violeta. O jogador sabe que algo é extraordinário pela cor antes do texto.
3. **Específico, nunca genérico.** Cada personagem tem um objeto ou gesto que o define e que passa no teste cego de silhueta e de descrição do G2. Personagem que lembra outra obra volta para a ficha.
4. **Legível numa tela de 6,5" em paisagem.** Cor chapada com dois tons e contorno leve. Micro-detalhe e PBR espelhado somem em ASTC e não são usados.

## 2. Câmera e projeção

- **3D em tempo real**, não sprites: o gênero (action RPG em terceira pessoa, exploração da vila e do bosque) pede câmera que acompanha o personagem. O caminho de sprites do Dungeon Courier não serve aqui.
- Câmera em terceira pessoa atrás e acima do personagem, com mira suave só em alvo hostil (`Scripts/Camera/`). Os marcadores de objetivo são desenhados depois da câmera.
- Paisagem, 30 FPS sustentados como meta (PIPELINE §4.1). O orçamento vale para a faixa Média, e a faixa Baixa tem de caber nele.

## 3. Paleta (GDD cap. 09)

| Cor | Hex | Uso |
|---|---|---|
| Azul profundo | `#253850` | menus e elementos arcanos |
| Dourado | `#D6B36A` | Limiar e ascensão |
| Verde | `#648B67` | áreas naturais, bosque |
| Terracota | `#A86D52` | Auren e materiais urbanos |
| Turquesa | `#86C8C9` | Trama e sinais mágicos |
| Violeta | `#9777B8` | anomalias |
| Marfim | `#E9DEC6` | textos e pergaminhos |

Regras: violeta só em anomalia; turquesa só em magia e Trama. Nenhuma informação depende só da cor (GDD cap. 09): estado e alerta sempre têm forma ou ícone junto.

## 4. Personagens

- Bases: infantil (uma topologia e um rig, presets de 5 e 8 anos), adolescente e adulto (PIPELINE §3). Não se cria modelo por ano de idade.
- **Objeto separado do corpo, sempre** (regra que reabriu o G3, `PROJETO.md` §0; a seção §7.1a citada lá ainda não está no PIPELINE da `main`): espada, chifre, ferramenta e cajado são gerados à parte e presos a um osso. Gerado junto, o item funde na mão e o rig arrasta errado.
- Elenco do slice: 10 NPCs de Auren, o avatar e o Guardião Aethron. Estado dos portões em [`arte/fichas/ELENCO.md`](arte/fichas/ELENCO.md) e [`arte/g2/folha1.md`](arte/g2/folha1.md): G1 13/13; G2 aprovado para Borin, Maelis, Tovin e Daren; Mara devolvida para a ficha.
- Peça marcada `PROTOTIPO` (ADR-0008) não entra em build de loja.

## 5. Interface

- uGUI montado por código (`Scripts/UI/Tela.cs`), dentro da área segura, alvo de toque de no mínimo 48 dp e texto de no mínimo 12 dp (layout testado em 16:9, 19,5:9 e 20:9).
- ⚑ **Fontes OFL:** títulos e nomes em **Cinzel** (serifada de inscrição, combina com fantasia medieval e com o Limiar); texto corrido e HUD em **Nunito** (a mesma do Dungeon Courier, lê bem em tamanho pequeno). A licença OFL vai junto do arquivo da fonte e entra na PROVENIENCIA. Isso resolve o `BLOCKED_PRODUCT_DECISION` da fonte própria até o idealizador escolher outra.
- Molduras e botões em 9 fatias gerados por código, em azul profundo com filete dourado. Ícones e retratos são **pintados** a partir do concept aprovado no G2, nunca de referência de terceiros.
- Critério de pronto da tela: nenhuma faixa lisa em 20:9, nenhum texto sobreposto, joystick sem cobrir o personagem e cada tela mudada conferida em foto no tamanho do aparelho.

## 6. Ambiente, luz e efeitos

- Luz por período (manhã fresca, tarde quente, noite de luar escura mas legível; `LuzDoDia`). Clima se faz com cor, nunca tirando luz.
- Céu e névoa pintados (ADR-0008). Sombra só da luz principal, uma cascata (PIPELINE §4.1).
- Símbolo da Trama: fios, círculos incompletos e pontos conectados, em turquesa. É o único motivo gráfico recorrente da magia.
- Efeito transparente só em VFX, no máximo três camadas no mesmo pixel.

## 7. O que esta página não cobre

Concept de personagem novo (vai pela ficha G1), áudio (GDD cap. 09: ambiente de Auren, identidade intimista, motivo do Limiar) e lista de peças com custo do Tripo (`ASSETS.md`).
