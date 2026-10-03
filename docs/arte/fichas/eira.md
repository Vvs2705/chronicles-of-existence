# Ficha G1 — `eira`

> Ficha do portão G1 ([ADR-0002](../../adr/ADR-0002-riscos-de-originalidade.md), modelo em [`_MODELO_G1.md`](_MODELO_G1.md)).
> Fato com fonte é cânone. **PROPOSTA** = ideia desta ficha, não está nos docs e não vale até ser aprovada.
> Não contradiz o cânone de trabalho do Borin (ADR-0010).

## 1. Identificação

- **id:** `eira` (`NpcCatalog.cs`; GDD cap. 06, NPC-06)
- **categoria:** Npc, base adulta (`PIPELINE.md` §3.1: 1,55–1,95 m)
- **onde aparece no slice:** B07 (cotidiano: dá aula na `praca_centro` de manhã e à tarde; Nilo e Sera estão "na aula da Eira" de manhã, rotina `aula_com_eira`); B09, central `q07_o_desaparecimento`, objetivo `perguntar_na_vila` (âncora `mural_avisos`, com Maelis e Oren), em que a pista parcial dela é o que Nilo perguntou na véspera (nó `lugar_vazio`, `DialogueCatalog.cs`); B14, fala `aos_oito`. Âncoras: `praca_centro` (manhã e tarde, `dar_aula`) e `mural_avisos` (noite, `ler_os_avisos`) (`NpcCatalog.cs`; os horários são HIPÓTESE v0 do próprio código). As vidas Serena e Normal trazem `oportunidade.aulas_com_eira`; Árdua e Ruptura não (`DestinyCatalog.cs`).
- **cânone de partida:**
  - educadora curiosa; história e conhecimento (dossiê §G; GDD cap. 06)
  - `traco.curiosa`; vínculo com Maelis, `aliada`, e o recíproco dela; Sera → Eira `aluna` (`NpcCatalog.cs`)
  - sabe `topico.historia_de_eldoria`, `topico.vila_auren` e `topico.primeira_fratura`; é "a única que conhece história antiga" e não sabe `topico.limiar` (`NpcCatalog.cs`). Nenhum nó de diálogo usa `primeira_fratura`, e o slice não nomeia a Fratura (`SLICE` B11 e B16)
  - "Auren não tem âncora de escola nem de mercado, então a aula de Eira e a feira acontecem na praça até a T008 publicar uma" (comentário em `NpcCatalog.cs`: fato de cena, não de lore)
  - o mural é uma tábua de 2,4 × 1,4 m com centro a 2,0 m do chão e interagível "Ler o mural de avisos" (`AurenSceneBuilder.cs`); Maelis registra as decisões ali à noite, e "a tinta ainda está fresca" (`NpcCatalog.cs`, `dialogo.maelis.no_mural`)
  - falas já escritas (`strings.pt-BR.json`): "Chegou bem na pergunta do dia: por que o rio nunca volta para trás? Pensa e me responde amanhã." · "De noite eu leio os avisos do mural. Papel velho também conta história, sabia?" · "…Tem ruína por aí que ninguém sabe quem levantou. Nem eu, ainda." · q07: "O Nilo não veio à aula. Ontem ele me perguntou o que tem depois da clareira, e eu disse que não sabia. Devia ter perguntado por que ele queria saber." · aos 8: "…Você e o Nilo fazem perguntas que os meus livros não respondem."
  - Eira ensina a ler: "A Eira disse que eu leio melhor que o Nilo" (`dialogo.sera.na_praca`); dos 8 aos 11 a fase inclui "estudos" (GDD cap. 03)
  - memória: Eira só testemunha `marco_idade_8` (`NpcMemory.cs`); a fala da q07 sai do estado da missão, não de lembrança
- **autor da ficha / data:** Concept Art Lead (agente), 2026-10-03
- **estado:** aprovado G1 por delegação (ADR-0010, 2026-10-03)

## 2. Critérios C1–C10

**C1 — Gancho.** *(PROPOSTA; que Nilo lhe perguntou está na fala `lugar_vazio`)* Eira, a pessoa mais alta de Auren, dá aula na praça com uma lousa pendurada no flanco, na altura do rosto das crianças, que escrevem ali o que ela promete responder amanhã. Ela pergunta "por quê" a todo mundo. A vez que não perguntou foi com Nilo, na véspera de ele sumir: ele quis saber o que tem depois da clareira, ela disse que não sabia e não perguntou por que ele queria saber.

**C2 — Necessidade do mundo.** Cânone: Auren decide por escrito (Maelis registra as decisões no mural toda noite), não tem prédio de escola (fato de cena acima), Eira ensina a ler e lê o mural de noite; Eldoria é "mais velho que qualquer avó daqui" e tem ruína que ninguém sabe quem levantou. **PROPOSTA:** em Auren o escrito chegou antes de quem lê. A vila se governa por aviso pregado num poste, e pouca gente lê de corrido, então o mural só funciona porque alguém ensina a ler e lê em voz alta: Eira existe por isso. Sem escola não há parede: a lousa vai pendurada nela, na altura de quem aprende, e a aula vai aonde ela for. A história que ela sabe é a que ficou escrita (o "papel velho" do mural) e a que ninguém escreveu (as ruínas), e por isso o "ainda" dela é honesto. Dependência: se a T008 publicar uma âncora de escola, metade desta necessidade cai; esta ficha propõe que Auren fique sem escola no slice.

**C3 — Silhueta em 3 formas.** *(PROPOSTA; reescrita em 2026-10-03 pelas arbitragens 1 e 2 do [`ELENCO.md`](ELENCO.md): nada acima da cabeça, nos ombros, nos braços, na frente da cintura nem fora da coxa)* Medidas a 30% pela régua do `client/tools/silhueta.py` (120 px/m). Câmera do jogo: pivô a 0,94 m, 2,75 m atrás da criança, 15° na partida (`BodyByAge.cs`, `ThirdPersonCamera.cs`), olho a ~1,65 m.
1. **Lousa no flanco:** a lousa (C4) fica em retrato, **com a face virada para a frente**, no mesmo plano do peito dela. Vai presa por uma alça cruzada e uma correia ao lado direito do corpo, de 0,85 m a 1,45 m do chão, e sai 0,20 m (24 px) para fora do flanco.
   - Em preto, de frente e em T-pose: um retângulo de 54 × 72 px colado ao lado direito do tronco, 12 cm abaixo da linha dos braços (1,57 m). De perfil, a lousa é uma tábua de 2 cm e some: a vista que conta é a de frente (de costas, aparece o verso).
   - Vizinhos, não donos da mesma zona: a prancha da Maelis (horizontal, na frente da cintura, dos dois lados), as caixas do Daren (penduradas longe do corpo, dos dois lados, de 0,90 a 1,15 m) e o aro do Borin (fora da coxa esquerda). O G2 confere (arbitragem 2, item 3).
   - **T-pose:** sobrevive (rígida no `Hips` e no `Spine`).
   - **Câmera:** quando ela fica de frente para a criança, a face da lousa fica de frente para a câmera, com a parte das crianças na altura do rosto delas (~1,0 m). No idle, o braço direito cai na frente da borda de dentro da lousa, e a metade de fora fica livre.
2. **Cintura de cinto:** um cinto de couro duro, de 0,14 m de altura, aperta a túnica na cintura (~1,13 m) em 0,26 m de largura. Acima dele a túnica franze até 0,38 m nas costelas; abaixo, abre até 0,40 m no quadril e termina ali.
   - Em preto: um recorte em X no tronco, de 6 cm (7 px) de cada lado entre costela e cintura. É a única cintura marcada do elenco: Daren, Sera, Aethron e Maelis declaram tronco sem cintura nas fichas deles.
   - Lê inteira do lado esquerdo; do direito, a lousa cobre a cintura e a borda de fora dela faz o contorno.
   - O cinto segura a correia da lousa e o saco de giz.
   - **T-pose:** sobrevive (malha). **Câmera:** fica na altura do olho da criança de 8 (~1,15 m).
   - É a forma mais fraca: se não ler a 30%, o cinto aperta mais (0,22 m) antes de qualquer outra mudança.
3. **Lanterna sob o braço esquerdo:** a lanterna-caixa de óleo, de 0,14 × 0,14 × 0,20 m (17 × 24 px), pende de um gancho curto na alça, encostada nas costelas esquerdas, de 1,28 a 1,48 m, e sai 0,12 m (14 px) do corpo. Fica do lado oposto ao da lousa, para o óleo e a fuligem não sujarem o giz.
   - Em preto: uma caixa pequena sob o braço esquerdo, que faz par torto com a placa grande do lado direito. Acesa só à noite (C4).
   - **T-pose:** sobrevive (rígida no `Chest`, 9 cm abaixo do braço). **Câmera:** de noite, é a luz que atravessa a praça.
   - Vizinha da vara da Maelis (uma reta à esquerda, até 1,30 m e 0,13 m fora do corpo) e das caixas do Daren: caixa contra reta, e colada ao corpo contra pendurada longe dele.
   - No idle, o braço esquerdo cai na frente dela; se raspar no clip Walking, encurta-se o balanço para trás desse braço, sem animação nova.

Apoio, que não conta como forma (arbitragem 2, item 6): a estatura de 1,92 m, a mais alta do elenco (arbitragem 2, item 5); o pescoço à vista; o cabelo curto e sem volume.

**Contra Lysa, Mara, Borin, Maelis e Daren em preto:**
- Lysa tem faixa larga sobre a cabeça e base larga.
- Mara tem rolo nos ombros, sem pescoço, e mangas em sino no cotovelo.
- No Borin, o braço direito inteiro é grosso. Em Eira, o braço direito é fino e a placa fica abaixo dele (o G2 confere o idle, parecer item 2).
- Maelis tem prancha na frente da cintura, dos dois lados, vara à esquerda e cabelo redondo.
- Daren tem uma canga com duas caixas iguais, longe do corpo, e tronco sem cintura.
- Eira tem uma placa de um lado e uma caixa pequena do outro, as duas coladas ao corpo, e a cintura apertada. Não tem vestido, coque alto, trança nem avental (§4).

**C4 — Objeto-assinatura com regra.** *(PROPOSTA)* **A lousa de perguntas.** Tábua de 0,45 × 0,60 × 0,02 m enegrecida com breu, moldura clara de 3 cm e giz de cal. Uma alça de couro de 3 cm vai do ombro esquerdo ao quadril direito (detalhe, não forma, e não é objeto rígido na diagonal das costas), e uma correia curta prende a lousa ao cinto para ela não balançar. A lanterna vai do outro lado (C3, forma 3). A parte de baixo da lousa (de 0,85 a 1,25 m do chão, toda ao alcance de uma criança de 5, sem ponta de pé) é das crianças, que escrevem ali o que ela promete responder "amanhã". A faixa de cima é da pergunta do dia, que ela escreve de manhã com a lousa no colo. Duas regras, as duas sobre o que o código já lê:
- **Período (rotina):** de manhã, a lousa só tem a pergunta do dia, no alto; à tarde, a parte de baixo está cheia de giz das crianças; à noite, no mural, ela lê o que carregou e apaga, e a lousa fica limpa. A lanterna só acende à noite. Uma luz atravessando a praça de noite é Eira.
- **Memória (`marco_idade_8`, que ela testemunha):** depois do salto, a pergunta do dia é "Eldoria antes de Auren" (a fala `aos_oito`).

A pergunta de Nilo não fica na lousa: o marco do sumiço é a folha da Maelis (arbitragem 2, item 1). Eira só a cita na fala `lugar_vazio`. São quatro estados de textura num atlas (manhã, tarde, noite, manhã depois do salto). Nada é concedido, nenhum número muda.

**C5 — Regra exclusiva.** *(PROPOSTA)* **Ler em voz alta.** Eira é a única pessoa de Auren que lê para o jogador:
1. Aos 5–7, interagir com o mural (o interagível "Ler o mural de avisos" já existe) sem Eira mostra os papéis como imagem, sem texto, e a criança diz "Ainda não sei ler isso."
2. De noite, quando a rotina põe Eira no `mural_avisos`, a mesma interação abre a leitura: ela ergue a lanterna até o papel e lê um aviso por vez, em voz alta, fechando cada um com uma pergunta (C6, fala 2). Durante a q07, um dos avisos é o do sumiço de Nilo (`SLICE` B09: "o mural mostra o aviso").
3. Depois de `marco_idade_8`, o mural abre como texto, sem ela. Se ela estiver lá: "Agora lê você. Eu fico ouvindo."

Nada depende disso: o mural não é passo de missão, e não há item, número nem objetivo atrás dele. A regra só lê período, rotina e idade do save. Custo: condição de presença e idade no interagível do mural, 3 falas e os textos dos avisos (nenhum está escrito).

**C6 — Voz.** *(PROPOSTA; texto final é do redator)* Diz "ainda", deixa a resposta para amanhã e fecha a frase com pergunta, como nas falas já escritas ("sabia?", "Pensa e me responde amanhã", "Nem eu, ainda"):
1. "Não sei. Ainda. Escreve aí na lousa, que amanhã a gente pensa junto."
2. "Aqui diz: 'Ninguém tira água do poço depois do sol.' Por que será que a Maelis escreveu 'depois do sol', e não 'de noite'?" (C5, passo 2)
3. "Pergunta de novo. …Não, espera. Antes, me diz: por que você quer saber?" (aos 8, com `marco_idade_8`: ela passou a perguntar de volta, C9)

**C7 — Contradição visível.** *(PROPOSTA)* Pergunta "por quê" a todo mundo e recebe as perguntas sem olhar. A mais alta de Auren não se abaixa para a criança escrever: a lousa é que fica na altura dela. Observável na aula (manhã e tarde, `praca_centro`, com Nilo e Sera na roda de manhã): quando uma criança vai à lousa, Eira fica parada de frente para ela, com o corpo e a face da lousa virados para a criança, vira o rosto para o lado, para a praça, e espera, sem olhar quem escreve. Só lê o que carregou no fim do dia, à luz da lanterna, no mural, e apaga. Custo: uma pose parada com o rosto virado, que o projeto ainda não tem.

**C8 — Relação com o avatar por destino e origem.** *(PROPOSTA; só fala e objeto, nunca poder, dossiê §H; itens e oportunidades conferem em `DestinyCatalog.cs`)*
- **serena:** aluna (`oportunidade.aulas_com_eira`) com a tarde livre (`oportunidade.tarde_livre`): é a única para quem "pensa e me responde amanhã" vira tarefa de verdade. Eira lhe dá a pergunta do dia para levar para casa, sem cobrar resposta (cobrar pediria estado novo no save).
- **normal:** aluna que também leva recado (`oportunidade.recado_da_vila`). Eira faz do recado uma lição: "Quando levar o recado, repara se tem alguma coisa escrita no caminho e me traz uma letra."
- **dificil** (rótulo "Vida Árdua", ADR-0007 §2): não é aluna; trabalha cedo (`oportunidade.trabalho_cedo`). Eira só a encontra de noite, no mural, e a aula dela é a da lanterna (C5): um aviso por noite. Nunca cobra a falta: "Quem trabalha de dia aprende de noite. A lanterna é a mesma."
- **ruptura:** não é aluna. Eira é dos que olham a criança "com cuidado" (`destino.ruptura.contexto_social`): convida a criança para a roda da manhã, que a família não pediu, e a deixa escrever primeiro na lousa. É convite de fala: a oportunidade não muda e nada é concedido. Não pergunta do sinal, do sonho nem do amuleto (não sabe `topico.limiar`; arbitragem 2, item 8).
- **origem:** sem variante. Se um dia for preciso, muda só o exemplo da pergunta do dia (horta, oficina, portão).
- Custo: 4 falas de destino; o `DialogueGraph` ainda não tem condição de destino (hoje: período, memória, tópico, missão, confiança), a mesma dependência do C8 do Borin e da Lysa.

**C9 — Mudança dos 5 para os 8.**
- **Antes (5–7):** a roda da manhã tem Nilo e Sera (rotina `aula_com_eira`, `NpcCatalog.cs`); a lousa enche de giz à tarde e é apagada à noite, e a pergunta do dia é a do rio (fala `na_aula`). Ela responde e não pergunta de volta.
- **Depois (8):** nenhum dos dois está mais na aula da manhã: Sera vai para a ervanaria e Nilo para o posto da guarda (rotinas pós-salto já implementadas, marcadas [PROPOSTA] no `SLICE` B09), e a roda que o jogador conhecia ficou vazia. A pergunta do dia é "Eldoria antes de Auren" (fala `aos_oito`). A criança lê o mural sozinha, e Eira fica ouvindo (C5, passo 3). E agora ela pergunta de volta (C6, fala 3).
- **Pede variante de malha?** Não. Muda textura da lousa, cena e fala.

**C10 — Momento de cartaz.** *(PROPOSTA)* Manhã de sol na praça, câmera do jogo atrás da criança. De frente para a câmera, a mulher mais alta de Auren está parada, com o rosto virado para o lado, olhando a praça. No flanco direito dela pende uma lousa de moldura clara, com a face para a câmera, bem na altura do rosto de uma criança, que escreve nela com giz. Sem olhar para baixo, a mulher pergunta: "Por que o rio nunca volta para trás?"

## 3. Amarração

- **Destino, Grau ou Trama:** o destino decide se a criança é aluna de dia (Serena e Normal), aluna de lanterna (Árdua) ou alvo da curiosidade dela (Ruptura) (C8). Grau não se aplica no slice (dossiê §E). Trama: ela é a adulta que sabe que existe história que ninguém consegue ler, e diz "ainda". **PROPOSTA de leitura do `topico.primeira_fratura`:** ela sabe que os papéis velhos têm um nome para algo que se quebrou e não sabe o quê; no slice não diz esse nome (B11, B16). A decidir pelo dono da narrativa.
- **Decisão de jogo que ele cria:** gastar uma noite indo ao mural ouvir Eira em vez de descansar (o período só anda concluindo missão ou descansando, ADR-0007 §1).
- **Pilar:** viver e crescer.

## 4. O que NÃO é → o que é

| Não é | É |
|---|---|
| maga: manto longo, capuz, cajado, varinha, livro aberto que brilha, azul profundo (#253850 é "menus e elementos arcanos", GDD cap. 09) | nada na mão: aponta com o dedo e com o giz; o que ela carrega é uma tábua no flanco; a única luz é uma lanterna de óleo de noite, chama laranja; nada de azul profundo; ela não ensina a faísca do B15 (o treino é no posto, `SLICE` B15) |
| manga de mago: sino aberto no pulso, tecido caindo do braço inteiro | manga reta de linho marfim, arregaçada até o cotovelo e suja de giz; os braços dela não têm forma (são do Borin, da Mara e da Lysa, arbitragem 2, item 4) |
| mentora sábia que explica o mundo: idosa, coque grisalho, óculos, pergaminho (o protótipo é coque grisalho e óculos, `PROVENIENCIA.md` §6; o acervo dá mecha grisalha, `ACERVO.csv` #125) | perto dos 35 anos (PROPOSTA), cabelo escuro curto, sem volume (o volume redondo de cabelo é da Maelis, o coque alto é da Sera), sem óculos como assinatura; responde "não sei, ainda" e devolve a pergunta (falas já escritas) |
| professora de livros: bolsa de livros, livro debaixo do braço (acervo #129, #154) | nenhum livro na silhueta: ela carrega as perguntas dos outros |
| túnica lilás (acervo #127); blusa mel e saia mostarda (protótipo; placeholder #F2CC8F em `Prototipos.cs`) | lilás cola no violeta #9777B8 das anomalias, e mel ou mostarda no dourado #D6B36A do Limiar. Nela: carvão e giz na lousa, moldura de madeira clara, camisa em marfim #E9DEC6 (liberado pelo ADR-0010 §5, e temático: "textos e pergaminhos"), túnica em terracota #A86D52 (Auren e materiais urbanos: ela é praça e mural) num tom mais fechado que o couro e o cobre do Borin |
| saia até o joelho ou vestido (protótipo) | túnica até o quadril, apertada pelo cinto, com calça e bota de trabalho. A forma de baixo dela é a cintura; as pernas não são forma, porque o elenco já usa todas, e o trapézio da cintura ao joelho é o sino da manta do avatar |
| vara de apontar, que lê como varinha | o dedo, o giz e a própria lousa |

## 5. Avaliação — parecer do Art Director: 19/20, aprovado (ADR-0010)

| C1 | C2 | C3 | C4 | C5 | C6 | C7 | C8 | C9 | C10 | Total /20 |
|---|---|---|---|---|---|---|---|---|---|---|
| 2 | 2 | 1 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | **19** |

- **Corte:** ≥ 14/20 e nenhum zero em C2, C3, C4, C5 — **atingido** (HIPÓTESE do ADR, a recalibrar)
- **Pontuador / data:** Art Director (agente) / 2026-10-03. Não escreveu a ficha.
- **Veredito:** **aprovado por delegação (ADR-0010)**, com três condições antes do concept:
  - **(a) Decidir para que lado a lousa está virada e alinhar C3, C7 e C10.** C3 quer a lousa de frente ("placa vertical, em retrato", saindo 0,20 m). C7 e C10 põem Eira "de lado", com a criança escrevendo nela, e isso só funciona com a face da lousa virada para o lado. Nesse caso, de frente a lousa vira uma tábua de 2 cm que o `silhueta.py` apaga. Uma placa lê em um eixo só: "lê de frente, de lado e de costas" não vale.
  - **(b) C1: tirar "sem ser lida".** O cânone (`dialogo.eira.lugar_vazio`) diz que ela ouviu a pergunta e respondeu que não sabia. O arrependimento é não ter perguntado de volta, que é a fala 3 da própria ficha. E pela C7 ela lê a lousa toda noite: teria lido a pergunta na véspera do sumiço.
  - **(c) "A mais alta" está disputada.** O `ELENCO.md` dá à Mara "a adulta mais alta de Auren depois do Borin" e à Eira "a mais alta do elenco", e a ficha `mara` ainda diz que Eira tem 1,72 m e "pernas de compasso". Recomendo que fique com a Eira (C1, C7 e C10 dependem da altura). Quem decide é o coordenador.
- **C3 sobe para 2** quando (a) e (c) estiverem resolvidas e as mangas passarem no teste de descrição.
- **Recomendação (repetição no elenco):** a pergunta de Nilo fica na lousa só durante a q07, onde é pista. Aos 8 ela sai, e a fala 3 muda. "O que não se fecha" é cânone da Maelis ("Esse registro eu não fecho"), e a ficha `maelis` conta com "Eira apaga giz" para o teste do troco.
- **Dependências:**
  - o dono da q07 aprovar a pergunta na lousa;
  - o dono do `SLICE` aceitar a C5: o B09 aprovado diz "o mural mostra o aviso", e a C5 tira o texto do mural de dia, aos 5–7;
  - Auren continuar sem âncora de escola (C2).

**Nota por critério (1 linha cada):**
- **C1 = 2.** Tem detalhe concreto (a lousa no quadril, na altura das crianças, e ela sem olhar quem escreve) e conflito. Mas o fato em que o conflito se apoia contradiz o cânone: condição (b).
- **C2 = 2.** Uma vila que se governa por aviso escrito, sem escola, com uma pessoa que ensina a ler e lê em voz alta. Está tudo amarrado a fatos que conferem: o mural, a Maelis, a aula na praça, a Sera lendo. Depende de Auren continuar sem escola.
- **C3 = 1.** Três formas nomeadas, e só a lousa é forte, com a geometria contraditória de (a). As mangas-bolso trazem um risco de "manga de maga" que a própria ficha declara e ficam no braço, zona sem arbitragem, já ocupada pela Mara (cotovelo), pela Lysa (pulso) e pelo braço do Borin. A estatura dá 10 cm sobre o Borin (12 px em 230) e está disputada com a Mara (c). Além disso, na folha a figura mais alta é a torre do Oren (2,17 m), porque o `silhueta.py` escala pela caixa da figura.
- **C4 = 2.** Lousa com medidas e três regras com ids. A da q07 tem função: o jogador vê a pista antes de falar com ela. Só não é "o mesmo estado do `lugar_vazio`": o nó lê `EmAndamento`, e a regra da ficha vale até o salto.
- **C5 = 2.** É a única que lê para o jogador, em três passos, sem nada a ganhar. Mexe num beat aprovado (B09) e cria uma condição de presença e idade no mural.
- **C6 = 2.** Três falas no tique das falas canônicas ("ainda", "amanhã", a pergunta no fim). A 2 é a melhor: ela ensina lendo. A 3 repete a Maelis (ver a recomendação).
- **C7 = 2.** Ação recorrente na aula, observável. A geometria da cena depende de (a). Custo: uma pose.
- **C8 = 2.** Quatro destinos, divididos em aluna de dia, aluna de lanterna e alvo da curiosidade dela, o que sai do `DestinyCatalog.cs` (as aulas só existem na Serena e na Normal). A Normal pede "o que está escrito nos sacos" do Oren, mas a ficha `oren` não tem sacos nem escrita (tem cestos e caixote).
- **C9 = 2.** A roda da manhã que fica vazia (Sera e Nilo com rotina nova) é a melhor batida de salto do elenco que li. Muda tudo sem mexer na malha.
- **C10 = 2.** Um quadro, fala canônica, 5 s. A encenação depende de (a).

**Fatos dados como cânone que não conferiram:**
1. C1, "sem ser lida": contradiz `dialogo.eira.lugar_vazio` e a C7 da própria ficha.
2. C4, "o mesmo [estado] que o nó `lugar_vazio` já lê": o nó lê `Missao(Q07, EmAndamento)`, e a regra vale até o salto.
3. C3, "É o único volume do elenco no flanco": o podão do Tovin é um J fora da coxa **direita**, entre o quadril e o meio da coxa (ficha `tovin`, C3), na faixa de altura da borda de baixo da lousa (0,85 m). As caixas do Daren pendem dos dois lados, de 0,90 a 1,15 m (ficha `daren`). É vizinhança, não colisão: um J pequeno contra uma placa.
4. C3, "lê de frente, de lado e de costas": uma placa de 0,02 m lê em um eixo só.
5. C3, "a mais alta do elenco (Borin 1,82; Mara 1,80)": confere com a tabela do `ELENCO.md`, mas a mesma tabela dá à Mara "a adulta mais alta depois do Borin", e a ficha `mara` diz que Eira tem 1,72 m.

Conferiram: id, traço, rotina (praça de manhã e à tarde, mural à noite; `aula_com_eira` de Nilo e Sera), vínculos e tópicos, inclusive `primeira_fratura` sem nenhum nó (`NpcCatalog.cs`, `DialogueCatalog.cs`); Auren sem âncora de escola (comentário do `NpcCatalog.cs`); mural de 2,4 × 1,4 m com centro a 2,0 m e interagível (`AurenSceneBuilder.cs`); todas as falas citadas (`strings.pt-BR.json`); testemunho só de `marco_idade_8` (`NpcMemory.cs`); `aulas_com_eira` só na Serena e na Normal (`DestinyCatalog.cs`); rotinas pós-salto (`NpcCatalog.cs`); GDD cap. 03 ("estudos") e cap. 09 (hex); acervo #125, #127, #129 e #154, protótipo (`PROVENIENCIA.md` §6) e #F2CC8F (`Prototipos.cs`); `PIPELINE.md` §3.1, §3.2 e §4; câmera (`BodyByAge.cs`, `ThirdPersonCamera.cs`); ADR-0007 §1; `SLICE` B09 e B15.

**Para o G2 — o que o concept precisa provar:**
1. **A lousa** de frente, em retrato, saindo 0,20 m do flanco, com a face decidida em (a), e a cena da aula desenhada com essa mesma face.
2. **Eira e Borin em idle,** na câmera do jogo e em preto: o braço direito caído sobre a lousa não pode ler como o braço-clava. Os dois são altos e estreitos, e de frente o pescoço projetado do Borin não aparece.
3. **As mangas-bolso:** teste de descrição sem "maga". E, na mesma folha com a Mara e a Lysa, os três braços se separam.
4. **A estatura** só conta depois de fechada a (c). Ela só separa entre corpos sem nada acima da cabeça. Se a folha não separar Eira de Mara e de Borin pela altura, a forma 3 cai.
5. **O desenho de Nilo** (trilha + "?") legível a 2,75 m, no celular em paisagem (já pedido pela ficha).
6. **Troco com a Maelis,** depois da recomendação: Eira lê o que os outros escreveram e apaga; Maelis escreve e não apaga.

**Condições cumpridas em 2026-10-03** (pelo autor da ficha; a nota acima não mudou):
- **(a) Face da lousa:** virada para a frente, no plano do peito dela. C3 mede a face de frente e diz que de perfil a lousa some. Em C7 e C10, Eira fica de frente para a criança, e a face da lousa para a criança e para a câmera; o que se vira é só o rosto dela.
- **(b) C1:** saiu "sem ser lida". O conflito agora é o do cânone (`lugar_vazio`): ela respondeu que não sabia e não perguntou por que ele queria saber.
- **(c) Estatura:** a mais alta é Eira, 1,92 m (coordenador, arbitragem 2, item 5). Pela arbitragem 2, item 6, a estatura passou a apoio e não conta como forma.
- **Recomendação da repetição e arbitragem 2, item 1:** o estado da q07 saiu da lousa. A pergunta de Nilo não fica na lousa nem antes nem depois do salto, a regra de C4 virou o ciclo do dia (limpa, cheia, apagada) e a fala 3 foi trocada ("por que você quer saber?", aos 8). O troco com a Maelis fica: Eira apaga, Maelis não.
- **Arbitragem 2, item 4:** as mangas-bolso saíram. A forma 2 é a cintura de cinto (única cintura marcada do elenco), e a forma 3 é a lanterna sob o braço esquerdo, que deixou de ser degrau na borda da lousa.
- **Fatos:** 1 resolvido em (b). 2 caiu junto com a regra da q07. 3: "único volume do flanco" saiu, e C3 lista as vizinhanças (Maelis, Daren, Borin) para o G2. 4: "lê de frente, de lado e de costas" corrigido. 5 resolvido em (c).
- **C8:** "os sacos do Oren" saíram (o recado agora pede "alguma coisa escrita no caminho"), e a reação ao amuleto saiu da Ruptura (arbitragem 2, item 8).
- **Dependências:** saiu a da q07 (a pergunta não está mais na lousa). Ficam o dono do `SLICE` aceitar a C5 e Auren sem âncora de escola.

**Conferência final (Art Director, 2026-10-03):** condições atendidas. (a), (b), (c), a recomendação da repetição e a Arbitragem 2, itens 1, 3, 4, 5, 6 e 8, estão no corpo. Ressalvas, sem pendência: o C3 e o §7 ainda descrevem o Daren antigo ("duas caixas iguais, dos dois lados, de 0,90 a 1,15 m"); hoje ele tem um estojo em I à esquerda (0,70–1,30 m) e uma caixa à direita (0,95–1,20 m), além das mãos. "Única cintura marcada do elenco" não vale para o avatar, que afina a cintura entre a trouxa e o sino, em escala de criança. As dependências do dono do `SLICE` (C5) e de Auren sem escola continuam, mas não eram condição.

## 6. Encaminhamento

**Para o G2 (concept), só depois de aprovado:**
- vistas: frente, perfil, costas e 3/4 verdadeiros em T-pose, fundo neutro, linha de chão, 1,92 m marcados; a lousa nos quatro estados de textura (manhã, tarde, noite, manhã depois do salto)
- silhueta: lousa no flanco, cintura de cinto e lanterna sob o braço esquerdo, em preto a 30% (`client/tools/silhueta.py`, 120 px/m), embaralhada com pelo menos Lysa, Mara, Borin, Maelis e Daren; render na câmera do jogo de frente (como a criança a vê na aula, com a face da lousa para a câmera) e de costas
- o concept precisa provar:
  - a lousa de face não lê como escudo nem como bolsa (retrato, moldura clara, giz à vista);
  - a cintura lê a 30% do lado esquerdo;
  - a lanterna não se confunde com as caixas do Daren nem com a vara da Maelis;
  - o braço direito caído na frente da borda da lousa não lê como o braço-clava do Borin (parecer, G2 item 2);
  - os dois braços passam na frente da lousa e da lanterna no idle e no clip Walking sem atravessá-las (se raspar no balanço para trás, encurta-se o balanço do braço no clip, sem animação nova)
- paleta: carvão, giz, madeira clara, marfim, terracota fechada, ferro escuro na lanterna; reservados e proibidos nela: turquesa #86C8C9, dourado #D6B36A (inclusive na chama e no vidro), violeta #9777B8
- objeto à parte: a lousa com medidas, a alça, a correia, o gancho e a lanterna
- orçamento (`PIPELINE.md` §4, HIPÓTESE v0): 8 000 tris, 2 materiais, textura 1024, 55 ossos sem osso secundário → lousa rígida no `Hips` e no `Spine`, lanterna rígida no `Chest`, as duas sem balanço; material 1 corpo, material 2 lousa e lanterna (atlas com os 4 estados e o vidro emissivo); lanterna acesa só por emissão, sem luz em tempo real por NPC
- altura: 1,92 m passa nas portas de 2,2 m (`PIPELINE.md` §3.2) e fica a 3 cm do teto da faixa de Npc adulto no V09
- **custo que esta ficha pede:** pose parada de lado com o rosto virado (C7); condição de presença e idade no interagível do mural, os textos dos avisos e 3 falas (C5); 3 falas (C6) e 4 de destino com a condição de destino no diálogo (C8); 1 componente que troca a textura da lousa por período e memória
- dependências: manter Auren sem âncora de escola (C2); o dono do `SLICE` aceitar a C5 (o B09 diz "o mural mostra o aviso")
- testes a registrar: silhueta (≥ 4/5), descrição, troco (com Maelis, a outra adulta do mural, e com Lysa: a cena da aula e a do mural têm de quebrar com qualquer uma das duas no lugar dela)

**Para o G3:** bloco `### eira` em `PROVENIENCIA.md` com `g1:` (data, nota, pontuador), `g2:` (data, silhueta n/5, descrição, troco), `entrada:` (concept em `arte/referencias/eira/` + SHA-256), `ferramenta`, `plano`, `licenca`, `termos_url`, `g3:`. `referencias_terceiros`: nenhuma usada nesta ficha.

## 7. Ligações com outras fichas

**PROPOSTAS desta ficha que envolvem outro personagem:**
- **Maelis:** "Maelis escreve, Eira lê". De noite as duas estão no mural (rotinas do `NpcCatalog.cs`), e Eira lê em voz alta o que Maelis registrou (C5). Maelis escreve e não apaga; Eira lê e apaga (C4, noite). A pergunta de Nilo fica com a folha da Maelis, não com a lousa (arbitragem 2, item 1). As duas carregam onde escrever: a prancha de Maelis é horizontal, na frente da cintura e dos dois lados; a lousa de Eira fica de face, num lado só, no flanco direito. O G2 confere que não se confundem.
- **Tovin:** a lanterna de Eira saiu de cima do ombro direito (zona da boca do chifre dele) e foi para baixo do braço esquerdo. O podão dele sai do flanco direito (arbitragem 2, item 3).
- **Daren:** as caixas dele pendem sob os braços, iguais, longe do corpo e dos dois lados; a lousa e a lanterna de Eira são diferentes entre si e coladas ao corpo. O G2 confere.
- **Nilo:** a pergunta dele não fica na lousa. Eira só a cita na fala `lugar_vazio`.
- **Sera:** aluna (`NpcCatalog.cs`) e lê melhor que Nilo (`dialogo.sera.na_praca`); sai da aula aos 8 (rotina pós-salto, [PROPOSTA] no `SLICE` B09). Esta ficha não lhe dá forma.
- **Lysa (par desta ficha):** Lysa lê a beira, sinal sem escrita; Eira lê o escrito. As duas dizem "não sei": Lysa só diante da coisa nova, Eira como tique. O redator precisa guardar essa diferença. A cintura marcada, que seria a reserva da Lysa, ficou aqui.
- **Mara:** o pescoço de Eira fica à vista (o rolo nos ombros é da Mara), e os braços de Eira não têm forma (as mangas em sino são da Mara). Mara deixa de usar a altura como forma e corrige na ficha dela as medidas antigas de Eira, 1,72 m e "pernas de compasso" (arbitragem 2, item 5).
- **Borin:** Eira é a pessoa mais alta do elenco, 10 cm acima dele (apoio, não forma). O G2 confere que o braço direito dela, caído na frente da lousa, não lê como o braço-clava dele.
- **Padrão do elenco, a conferir:** o objeto que muda com o estado do jogo (aro do Borin, chapéu de Lysa, lousa de Eira). Na lousa, a regra é o ciclo do dia (limpa de manhã, cheia à tarde, apagada à noite), não uma marca do jogador nem o sumiço de Nilo.

**Zonas de silhueta que esta ficha ocupa:**
- flanco direito, abaixo da linha dos braços e acima da coxa (de 0,85 a 1,45 m): placa em retrato de face para a frente, saindo 0,20 m do corpo (lousa);
- cintura marcada: recorte em X no tronco, o único do elenco (cinto);
- costelas esquerdas, sob o braço: caixa pequena colada ao corpo, saindo 0,12 m (lanterna).

Apoio, não forma: a estatura (1,92 m, a mais alta do elenco) e o pescoço à vista. Livres de propósito: a cabeça e o espaço acima dela, os ombros, os braços, a frente da cintura, o lado de fora das coxas, a diagonal nas costas e as pernas.
