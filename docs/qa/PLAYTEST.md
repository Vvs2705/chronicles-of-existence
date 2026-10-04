# Playtest do slice: protocolo da primeira rodada

> **Estado (2026-10-03): [PROPOSTA]. Nenhuma sessão foi rodada.** Escrito para uma pessoa conduzir sozinha, no PC, em modo celular.
> **Fontes:** `docs/PROJETO.md` §2.3 e §6 · `docs/design/SLICE_A_PRIMEIRA_EXISTENCIA.md` (B01–B16) · `docs/qa/T014_REGRESSAO.md` · ADR-0004, ADR-0006, ADR-0007, ADR-0008, ADR-0009 · contrato do diário de sessão (2026-10-03) · fichas 46 (playtest) e 48 (privacidade) do estúdio.
> Não é parecer jurídico. Lei citada tem fonte e data de consulta; o resto está marcado "a verificar".

**Onde roda agora:** a build de Windows em modo celular. Pelo ADR-0006, o PC é ferramenta, não alvo, e não sai APK antes da versão final.
**No celular, quando chegar a hora:** o protocolo é o mesmo. Mudam três coisas: a entrada (toque real; aí entram as hipóteses de toque, paisagem e 30 FPS, com o HUD de desempenho ligado e a contagem de toques errados por botão), a gravação (tela e mãos, nunca rosto) e a coleta do diário (`adb pull` de `Android/data/br.com.vstack.coe/files/diario/`, caminho a verificar no aparelho).

---

## 1. Para quê

Cada sessão mede as hipóteses do PROJETO §2.3 que só se resolvem jogando. A evidência vem de três fontes:
- **O**: observação do condutor, anotada na ficha (§7);
- **D**: o diário que o jogo grava (§6);
- **P**: as perguntas do fim (§5).

**Regras de leitura** (da ficha 46):
- Em rodada pequena, o resultado vai em contagem ("4 de 6"), nunca em porcentagem. Duração vai em mediana e faixa, nunca só em média.
- Um achado só vira padrão quando aparece em **2 sessões independentes**. Antes disso, é sinal fraco.
- Crianças, adolescentes e adultos são populações diferentes: cada número sai separado por faixa. Sessão com ajuda nível A3 (§2.4) fica fora da conta de duração.
- Uma rodada só gera evidência. Quem muda o estado de uma hipótese no §2.3 é o idealizador, com as fichas na mão.
- A arte é protótipo (ADR-0008). "Divertido" medido agora vale para este visual, não para o jogo final.

| Hipótese (§2.3) | Evidência | Conta como confirmado [PROPOSTA] | Conta como refutado [PROPOSTA] |
|---|---|---|---|
| **H1.** O slice dura 45–75 min | D: do `inicio` ao `marco.fim_da_primeira_existencia`, menos pausas. O: onde o tempo foi gasto | mediana entre 45 e 75 min nas sessões sem A3, e a maioria chega ao B16 | mediana fora de 45–75 min, ou menos da metade chega ao B16 |
| **H2.** A infância é divertida | D: opcionais começadas e concluídas, descansos. O: exploração fora do objetivo (E), minuto e beat de abandono, etiquetas P e F. P1 | a maioria faz ao menos uma opcional sem ninguém mandar, ninguém abandona antes do B12, e a maioria diz "sim" na P1 | abandono por tédio em 2+ sessões antes do B12, ou a maioria ignora as três opcionais e diz "não" na P1 |
| **H3.** As escolhas são perceptíveis | D: `evento.q04_promessa_*` e `evento.q07_assinou_*`. P2, P3 e P8 comparadas com o diário. O: reação às falas de Sera e Nilo | a maioria cita a promessa sem ser perguntada sobre ela (P2 ou P3) e diz uma consequência que bate com o desfecho do diário | a maioria não cita a promessa, ou cita uma consequência que o diário desmente |
| **H4.** O salto temporal é significativo | D: tempo entre `marco.eco_do_limiar` e `idade`, e opcionais concluídas nesse intervalo (voltou para terminar). O: leu o aviso, hesitou, comentou que cresceu. P5 | a maioria lê o aviso do B12 e, na P5, diz algo que mudou ou ficou para trás | a maioria confirma sem ler e não sabe dizer o que mudou |
| **H5.** As 12 configurações dão variação, não reskin | O: destino, origem e o porquê da escolha (o diário não grava isso). P4 e P8 | jogadores com destinos diferentes descrevem a família e a vila de jeitos diferentes, com as palavras deles | descrições iguais para destinos diferentes. **A rodada 1 só junta sinal:** cada jogador vê uma das 12 |
| Extra: destino é circunstância, não dificuldade (ADR-0004) | O: frase literal na tela do B02. P4 | ninguém trata o destino como "fácil" ou "difícil" | 2+ sessões tratam o destino como dificuldade |
| Extra: o indicador basta para achar o caminho (PROPOSTA de 2026-10-02) | D: lacunas sem progresso (§6). O: notou o "▼" e a seta da borda? | nenhuma lacuna acima de 3 min na história principal que a ficha ligue a "não achou" | o mesmo ponto trava em 2+ sessões |
| Escala do corpo (1,10 m aos 5 anos, 1,28 m aos 8) | O: comentário espontâneo sobre tamanho no B06 e no B14 | sem critério: só sinal | — |
| Paisagem, toque ≥ 48 dp, 30 FPS em faixa média | — | não se mede no PC: fica para o celular | — |
| Versão do Unity | — | não se mede jogando | — |

O que cai entre as colunas "confirmado" e "refutado" é **inconclusivo** e volta na rodada 2.

---

## 2. Antes

### 2.1 Quem joga [PROPOSTA]

- **Rodada 1:** 6 pessoas, 2 por faixa: 8–11 anos, 12–17 e 18+. Em cada faixa, uma pessoa que joga RPG ou ação (celular ou console) e uma que não joga.
- **Menores de 8 anos ficam fora:** o jogo é todo em texto, sem voz, e ler bem é condição para medir compreensão.
- **Ninguém joga duas vezes.** Quem já viu o slice vira outra população. A rodada 2 é com gente nova, depois das correções.
- Anote na ficha se a pessoa conhece o desenvolvedor: quem gosta de quem fez o jogo tende a elogiar.

### 2.2 Máquina e build

1. **A mesma build na rodada toda.** Anote o commit na ficha (`git rev-parse --short HEAD`). Build nova no meio da rodada abre uma rodada nova.
2. Antes da rodada, confirme que a build vai do Limiar ao gancho: `COE.exe -roteiro` e `COE.exe -roteiro quebrada` com ROTEIRO OK (`T014_REGRESSAO.md`). O robô usa um save próprio e grava o diário em `roteiro_diario\`: não toca o seu save nem a pasta `diario\` dos jogadores.
3. **Abra e feche o jogo uma vez depois do build.** A primeira abertura de um `COE.exe` novo mostra o alerta do Firewall (pode cancelar: o jogo não usa rede) e trava ~300 ms na primeira esquiva (PROJETO §6).
4. **Despareie o gamepad Bluetooth.** Ele também move o personagem (PROJETO §4). Na rodada inteira, a entrada é o mouse no modo celular, igual para todos.
5. **Confira as configurações antes de cada sessão** (Menu → Configurações: mão, sensibilidade, FPS, qualidade, HUD de desempenho desligado). Elas ficam em `PlayerPrefs`, fora do save: o que um jogador muda, o próximo herda. Volte ao que estava na sessão 1.
6. **Abra o jogo com `client\tools\run_windows.ps1 -Celular -KeepOpen -Shots 0`, sem `-Scene`.** Com `-Scene`, o jogo pula a tela de título e o nascimento. Abra só com o jogador sentado: o diário começa a contar quando o jogo abre.

### 2.3 Save novo sem perder o seu

O save fica em `%USERPROFILE%\AppData\LocalLow\V-STACK\Chronicles of Existence\save.json`, com o `save.json.bak` ao lado (`LocalSave`). O diário fica na subpasta `diario\`.

- **Só "Nova vida" não protege o seu save.** Ela grava um save em branco e manda o anterior para o `.bak`. Cada gravação seguinte também manda a anterior para o `.bak`. Por isso, na confirmação do nascimento (minutos depois), o save em branco vai para o `.bak` e o seu sai de lá (`EntryFlow.RecomecarVida`, `LocalSave`).
- **Uma vez, antes da rodada, com o jogo fechado:** copie `save.json` e `save.json.bak` para uma pasta sua fora dali (ex.: `Documentos\COE_meu_save\`).
- **Antes de cada sessão, com o jogo fechado:** apague `save.json`, `save.json.bak` e qualquer `save.json.*` dessa pasta. São da sessão anterior, e o que importa dela já está no diário. Assim, o jogo abre na tela de título com "Começar". Entre jogadores, não use "Nova vida": a tela dela mostra o nome que o jogador anterior digitou.
- **Depois da rodada:** apague o save de teste e copie o seu de volta.

### 2.4 O que o condutor diz e não diz

**Diz antes de começar, sempre com as mesmas palavras:**
"Este jogo ainda está sendo feito. Eu quero ver o jogo, não você: não tem resposta certa nem errada. Se alguma coisa for confusa, a culpa é do jogo. Pode falar o que estiver pensando enquanto joga. Não vou poder ajudar muito, porque quero ver como o jogo se vira sozinho. Você pode parar quando quiser."
Se o jogador sabe que você fez o jogo, acrescente: "O que der errado me ajuda mais do que elogio."

**Não diz:**
- como se joga, o que é o destino, para onde ir, quem procurar, o que um botão faz;
- nada que aponte o objetivo ("olha ali", "tenta falar com..."), nem com o dedo ou o olhar na tela;
- nada que julgue uma escolha (nem "hmm", nem riso) na promessa (B08), na assinatura (B09) ou no salto (B13);
- que existem missões opcionais, que o dia passa ou que o personagem vai crescer.

**Pode dizer só estas frases neutras:** "O que você acha que dá pra fazer agora?", "O que você está procurando?", "O que você esperava que acontecesse?" e "Me conta mais."

**Escada de ajuda [PROPOSTA].** A ajuda também é dado: anote o nível e o minuto. Ela só entra depois de 3 min sem progresso, **e** se o jogador pedir ou mostrar frustração.
- **A1:** "O que o jogo pediu pra você fazer?"
- **A2:** "Onde você acha que isso fica?"
- **A3:** dizer o que fazer. A sessão continua, mas sai da conta da H1.
- Bug que deixa o jogador sem saída: ajude direto, marque B e siga.

**Imprevistos:**
- **O jogo fechou sozinho:** anote o minuto e abra de novo. O save continua, e o diário abre um arquivo novo: guarde os dois.
- **Pausa** (água, banheiro): pelo botão Menu. Anote na ficha o início e o fim. O diário conta o tempo de relógio, inclusive com o jogo parado; se ele tiver `pausa`/`volta` no mesmo intervalo, vale o diário.

**Quando parar a sessão:**
- o jogador pede: não pergunte por quê, só anote o minuto e o beat (é o dado de abandono);
- o responsável pede;
- a sessão chega a 90 min;
- o jogador mostra desconforto: pause pelo Menu e pergunte se ele quer seguir.

**Falhas conhecidas** (marque B e não explique): o NPC muda de lugar sem andar até lá (sem NavMesh, PROJETO §6); o HUD e os textos são de protótipo.

---

## 3. Crianças e adolescentes

**O que fazemos com todo menor de 18 anos** (regra do estúdio, ficha 46):
- **Termo assinado** por um dos pais ou pelo responsável legal antes de ligar o jogo (modelo abaixo), mais o "sim" da própria criança, perguntado em voz alta.
- **O responsável fica na sala a sessão toda.** Pedimos a ele que não ajude nem aponte. Se a criança pedir ajuda, ele devolve com "o que você acha?".
- **Sem vídeo, sem foto e sem gravação de voz.** A rodada 1 inteira vai sem vídeo, adultos também: o diário é a linha do tempo.
- **Sem nome.** A ficha usa um código (`P01`, `P02`...) que não aparece no termo, e nenhuma lista liga código a pessoa. Não anotamos data de nascimento, escola, cidade nem contato. A idade entra só como faixa.
- **O nome do personagem.** No B04, o jogador digita o que quiser, e pode ser o próprio nome. Esse nome fica só no save deste PC, apagado antes da sessão seguinte (§2.3). Pelo contrato, o diário não grava esse nome; mesmo assim, confira o arquivo antes de levar ao repositório (§5.2). Se a criança perguntar "posso pôr meu nome?", responda: "pode pôr o nome que quiser".
- **O termo em papel** fica guardado pelo idealizador, separado das fichas. Não é digitalizado e não entra no repositório.

**Atenção ao conteúdo.** Na q04, a escolha é entre guardar o segredo de um amigo que vai sozinho ao bosque ("Cumprir a promessa") e contar a um adulto ("Quebrar a promessa"). Depois disso, o amigo some por um tempo (`dialogo.opcao.evento.q04_*` em `strings.pt-BR.json`). O termo avisa sobre isso. Na sessão, anote a frase literal da criança e não comente.

### O que a lei pede (leitura do estúdio, não parecer jurídico)

| Norma | O que o texto diz | Para este teste presencial |
|---|---|---|
| LGPD (Lei 13.709/2018), art. 5º, I, e art. 12 | Dado pessoal é "informação relacionada a pessoa natural identificada ou identificável". Dado anonimizado não é dado pessoal, a não ser que a anonimização possa ser revertida "com esforços razoáveis". | Ficha e diário sem nome, sem rosto, sem voz e com a idade em faixa não identificam a criança. **A verificar:** se faixa e data da sessão, num grupo pequeno de conhecidos, ainda passam no "esforço razoável" do art. 12. |
| LGPD, art. 14, §1º | Dado pessoal de criança só pode ser tratado "com o consentimento específico e em destaque dado por pelo menos um dos pais ou pelo responsável legal". | O único dado pessoal possível é o nome que a criança digita no jogo. Ele fica no save do PC até ser apagado, e é por isso que o termo cita esse nome. |
| LGPD, art. 14, §4º | Não se pode condicionar a participação da criança "em jogos, aplicações de internet ou outras atividades" a dados além dos "estritamente necessários". | O teste não pede nenhum dado da criança, e o nome do personagem pode ser inventado. |
| LGPD, art. 14, §6º | A informação deve ser "simples, clara e acessível" e "adequada ao entendimento da criança". | O termo é curto e tem uma frase para a criança. |
| LGPD, art. 4º, I | A lei não se aplica ao tratamento feito "por pessoa natural para fins exclusivamente particulares e não econômicos". | Um jogo que vai para a loja tem fim econômico: não contar com essa exceção. |
| ECA (Lei 8.069/1990), arts. 2º e 17 | Criança tem até 12 anos incompletos; adolescente, de 12 a 18. O respeito abrange "a preservação da imagem, da identidade". | É a base para não filmar nem fotografar. A faixa 12–17 também precisa de termo. |
| ECA Digital (Lei 15.211/2025), arts. 1º e 2º, I. Em vigor desde 17/03/2026 (art. 41-A, redação da MP 1.319/2025) | Vale para "produto ou serviço de tecnologia da informação" direcionado a crianças e adolescentes ou de acesso provável por eles. O art. 2º, I, define esse produto como "fornecido a distância, por meio eletrônico e provido em virtude de requisição individual" e cita "jogos eletrônicos ou similares conectados à internet ou a outra rede de comunicações". | A sessão é presencial, no PC do estúdio, com um jogo sem rede: não há fornecimento a distância nem jogo conectado. Leitura do estúdio: a lei não alcança a sessão. **A verificar** com advogado. Na loja, a pergunta é outra (ADR-0009 §3). Nota: o ADR-0009 cita o art. 22 para loot box, mas o texto consultado traz a vedação no art. 20. |
| Res. CNS 510/2016 (ética em pesquisa em ciências humanas e sociais) | **A verificar** (texto não lido na íntegra): o art. 1º, parágrafo único, I, dispensaria do sistema CEP/Conep a "pesquisa de opinião pública com participantes não identificados". | É teste de produto de estúdio, não pesquisa acadêmica. Se os dados um dia forem para artigo ou TCC, rever antes. |

**Fontes, consultadas em 2026-10-03.** O planalto.gov.br não respondeu nessa data, e os textos vieram de compilações. Antes de citar em documento externo, confira no planalto.
- LGPD: https://www.vademecumprevidenciario.com.br/legislacao/htm/lei_00137092018
- ECA: https://www.vademecumprevidenciario.com.br/legislacao/htm/lei_00080691990
- ECA Digital: https://www.vademecumprevidenciario.com.br/legislacao/htm/lei_00152112025 (arts. 1º, 2º, I, e 41-A) e https://www.lex.com.br/lei-no-15-211-de-17-de-setembro-de-2025/ (art. 20)
- Res. CNS 510/2016: só o resumo de uma busca. O PDF (https://www.furb.br/web/upl/arquivos/201702061757160.CNS_5102016.pdf) não abriu.

### Termo modelo

```
AUTORIZAÇÃO PARA TESTE DE JOGO

Jogo: Chronicles of Existence (nome provisório), em desenvolvimento pela V-STACK.
Quem conduz: ______________________   Contato: ______________________

O que é: a criança (ou o adolescente) vai jogar no computador um jogo que ainda
está sendo feito, por até 1h30, e responder algumas perguntas curtas no fim.
Você fica na sala o tempo todo.

O jogo: o personagem é uma criança numa vila de fantasia. Faz tarefas, conversa
com os vizinhos, decide se guarda ou conta o segredo de um amigo, e esse amigo
some por um tempo. Mais velho, o personagem treina luta com espada de madeira
e magia, contra um parceiro de treino.

O que anotamos: o que acontece no jogo (aonde vai, quanto tempo leva, o que
escolhe) e as respostas às perguntas. O jogo grava um arquivo com esses
acontecimentos e o tempo de jogo. Da idade, anotamos só a faixa.

O que NÃO fazemos: não filmamos, não fotografamos e não gravamos voz. Não
anotamos nome, data de nascimento, escola, endereço nem contato da criança.
Nada vai para a internet. Se a criança puser o próprio nome no personagem,
o nome fica só neste computador e é apagado antes do próximo jogador.

Para onde vai: as anotações, sem nome, ficam guardadas no projeto do jogo
para melhorá-lo. Este papel fica guardado à parte e não é digitalizado.

Parar: a criança ou você podem parar a qualquer momento, sem explicar.
Como as anotações não têm nome, depois de hoje não dá para saber quais são
dela. Se quiser que sejam descartadas, avise hoje, antes de ir embora.

Eu, responsável pela criança ou pelo adolescente, autorizo a participação
neste teste.
Nome do responsável: ____________________  Assinatura: ____________  Data: __/__/____

Para a criança, em voz alta: "Você quer jogar um jogo que ainda está sendo
feito? Você pode parar quando quiser, e não tem resposta certa nem errada."
[ ] A criança disse que quer.
```

---

## 4. Durante

**Tempo.** Use um cronômetro (mm:ss), disparado quando a janela do jogo abre. O diário também conta tempo de relógio desde a abertura. Anote o cronômetro na primeira entrada em Auren: esse momento corresponde à primeira linha da `q01_um_novo_amanhecer` no diário, e a diferença entre os dois tempos é o **ajuste** da sessão.

**Etiquetas:**
- **C** compreendeu
- **D** dúvida ou não entendeu
- **F** frustração
- **P** prazer
- **E** explorou fora do objetivo
- **B** bug
- **T** clique errado
- **A1–A3** ajuda dada

Escreva a frase do jogador entre aspas, inteira (ficha 46: o relato já costuma trazer o diagnóstico).

| Beat | O que olhar | O que aparece no diário |
|---|---|---|
| B01 Limiar | Lê as seis falas ou passa direto? Usa "Voltar"? Olha o símbolo? | só o `inicio` (`save=novo`, `idade=5`) |
| B02 Destinos | Quanto tempo fica na tela? Lê os quatro? Troca? **O que acha que é** (frase literal: aparece "fácil" ou "difícil"?) | nada: anote o destino na ficha |
| B03 Origem | Lê? Troca? | nada: anote a origem na ficha |
| B04 Nome | Hesita? (não anote o nome) | nada |
| B05 Confirmação | Lê o "tem certeza"? Volta? | nada (o save nasce aqui) |
| B06 q01 | Acha a família? Sabe sair de casa? Nota o "▼"? | `objetivo q01_um_novo_amanhecer/falar_com_familia`, `.../sair_de_casa`, `evento.q01_concluida` |
| B07 q02 e opcionais | Acha o Daren e a tarefa? Começa q03, q05 ou q06 sozinho? Anda sem objetivo (E)? Usa "Descansar"? | linhas `missao` da q02 a q06, `periodo` |
| B08 q04 (a promessa) | Quanto tempo leva para escolher? Frase literal na hora da escolha. Reação às falas de Sera e Nilo | `evento.q04_promessa_cumprida` ou `evento.q04_promessa_quebrada`, `evento.nilo_desapareceu` |
| B09 q07 (o sumiço) | Nota que o Nilo sumiu? Liga o sumiço à promessa (frase)? Como assina com a Maelis? | `objetivo q07_o_desaparecimento/...`, `evento.q07_assinou_com_o_circulo` ou `evento.q07_assinou_com_um_risco` |
| B10 Bosque | Acha a entrada? Volta para a vila? | lacuna entre o fim da q07 e a q08 |
| B11 Símbolo | Reconhece o símbolo do Limiar (frase espontânea)? | `objetivo q08_ecos_do_limiar/achar_o_simbolo`, `.../tocar_o_simbolo`, `marco.eco_do_limiar` |
| B12 Aviso | Lê? Quanto tempo fica nele? Volta para a vila para terminar opcionais? | opcionais concluídas entre o `eco` e a `idade` |
| B13 Salto | Hesita? Cancela alguma vez? Frase literal | `idade`, `marco_idade_8` |
| B14 Aos 8 | Reação ao crescer (comenta o tamanho?). Quem procura? Percebe que lembram da promessa? | nada: conversa não grava, anote quem ele procurou |
| B15 Treino | Entende os quatro golpes? Onde trava com o mouse? | nada: os golpes não gravam |
| B16 Gancho | Lê? Reação e frase literal | `marco.fim_da_primeira_existencia` |

---

## 5. Depois

### 5.1 As perguntas

Faça as perguntas nesta ordem, com estas palavras. Para seguir, use só "Me conta mais" ou "Por quê?". Anote a frase inteira. Compare as respostas com o diário só depois que o jogador for embora: não corrija nada na hora.

1. "Se desse, você queria continuar jogando agora?"
2. "Me conta a sua história no jogo, do começo até o fim."
3. "Teve alguma hora em que você escolheu o que fazer? O que aconteceu depois?"
4. "No começo apareceram quatro vidas pra escolher. O que você acha que era aquilo?"
5. "Teve uma hora em que o tempo passou. Como foi isso?"
6. "Teve alguma hora em que você não sabia o que fazer? Onde?"
7. "Qual foi a parte mais legal? E a mais chata?"
8. "Se você jogasse de novo, faria alguma coisa diferente?"

A P2 vem antes da P3 para a lembrança livre aparecer antes de a pergunta falar em escolha. A P4 vem depois das duas para não plantar o destino na resposta.

### 5.2 Pegar o diário e guardar

1. **Feche o jogo pelo próprio jogo:** Esc ou Voltar até a raiz, depois confirme a saída. Assim o diário fecha com `fim`. Matar o processo deixa o arquivo sem `fim` (o leitor aceita, mas marca).
2. **Ache o arquivo** na pasta `%USERPROFILE%\AppData\LocalLow\V-STACK\Chronicles of Existence\diario\`. É o `sessao_<yyyyMMdd_HHmmss>.txt` com a hora de início da sessão, o mais novo. Se o jogo caiu e reabriu, são dois. O jogo guarda só os 10 mais novos: copie no mesmo dia.
3. **Confira com os olhos:**
   - a primeira linha tem `save=novo`. Se tiver `save=continuado`, o save da sessão anterior não foi apagado, e a sessão não serve para a H1;
   - nenhuma linha tem o nome digitado nem outro dado do jogador.
4. **Guarde em `docs/qa/playtests/<aaaa-mm-dd>_<n>/` [PROPOSTA]**, em que `<n>` é o número da sessão no dia. Vão o `sessao_*.txt` com o nome original e a `ficha.md` (cópia do §7, preenchida). Mais nada: sem termo, sem foto, sem save.
5. **Síntese da rodada [PROPOSTA]:** em `docs/qa/playtests/rodada_<n>.md`, a tabela do §1 com a contagem por sessão, e a sessão e o minuto de cada achado.

---

## 6. Ler o diário

**Formato (contrato fixado pela raia do código em 2026-10-03):**
- Uma linha por acontecimento, sempre com 3 colunas: `mm:ss<TAB>tipo<TAB>detalhe`. Em `pausa` e `volta`, o detalhe é vazio e a linha termina no TAB.
- O tempo é de relógio, desde o início da sessão, e conta também o tempo com o jogo em segundo plano.
- `inicio`: `00:00<TAB>inicio<TAB>versao=<v> cena=<Bootstrap|Auren> save=novo|continuado idade=<5|8> periodo=<manha|tarde|noite>`. `save=novo` quer dizer save sem nascimento confirmado.
- `objetivo`: `<questId>/<objetivoId>`.
- `missao`: `<questId> <status>`, com o status em `indisponivel`, `disponivel`, `em_andamento`, `concluida` ou `falhada`.
- `evento`: o id do histórico, como `evento.q04_promessa_quebrada` ou `marco_idade_8`.
- `periodo`, `idade`: o valor novo.
- `fim`: traz a duração total (`mm:ss`) no detalhe.
- O que sai de uma mesma gravação vem nesta ordem: objetivos e status, depois eventos, depois período, por fim idade. Linhas com o mesmo `mm:ss` são um passo só e não formam lacuna.
- **"Nova vida" no meio da sessão** continua no mesmo arquivo e aparece como `periodo manha` seguido de `idade 5`. Daí em diante é outra partida: separe as contas.
- **A confirmar com a raia do diário:** se `mm` passa de 59 numa sessão longa (o leitor assume minutos totais, como em `75:12`).

**À mão, em planilha ou papel:**
- **Duração total:** o detalhe do `fim`. Sem `fim` (processo morto; no Android, app fechado pela lista de recentes), vale o tempo da última linha, que costuma ser uma `pausa`.
- **Duração ativa:** a total menos a soma dos intervalos entre cada `pausa` e a `volta` seguinte.
- **Duração do slice (H1):** do `inicio` ao `marco.fim_da_primeira_existencia`, menos as pausas.
- **Prólogo (B01–B05):** do `inicio` até a primeira linha da `q01`.
- **Tempo por missão:** da primeira linha `objetivo` ou `missao` dela até `missao <id> concluida`. Uma q03 `falhada` no sumiço do Nilo, ou uma opcional `falhada` no salto, é regra do jogo (ADR-0007 §3, B12), não fracasso do jogador.
- **Lacunas:** toda lacuna de mais de 1 min entre duas linhas de progresso (`objetivo`, `missao`, `evento`, `idade`), fora de pausa, entra na lista. **Acima de 3 min, é candidata a travamento [PROPOSTA].**
  - O diário não tem posição: lacuna quer dizer "sem progresso", não "parado".
  - Nomeie a lacuna pelo que vem antes e depois. Exemplo: "entre `q02/receber_tarefa` e `q02/cumprir_tarefa`, 4:10".
  - Ela só vira travamento se a ficha tiver, no mesmo minuto (com o ajuste), uma nota D, uma nota F ou uma ajuda.
- **Também conte:** os desfechos da q04 e da q07; as opcionais começadas e concluídas; os descansos, que são as linhas `periodo` sem missão concluída junto.

**Script (escrito em 2026-10-04):** `python client/tools/diario_report.py <sessao_*.txt | pasta>`, no molde do `perf_report.py` (só biblioteca padrão, `--autoteste` no CI e no `verify.ps1`). Faz o que está abaixo, menos somar as duas partes de uma sessão em que o jogo caiu (cada arquivo sai sozinho, com o aviso de "sem fim"). Conferido nos diários do robô: rota completa com 3 opcionais concluídas, rota quebrada com 0.
- **Entrada:** um ou mais `sessao_*.txt`, ou a pasta de uma rodada.
- **Saída por sessão:** as durações acima, o tempo por missão, os desfechos, as opcionais e a lista de lacunas acima de 1 min, com as acima de 3 min marcadas.
- **Saída por rodada:** a mediana e a faixa de cada número.
- **Tolerância:**
  - tipo desconhecido é contado e ignorado;
  - arquivo sem `fim` sai marcado e termina na última linha;
  - as duas partes de uma sessão em que o jogo caiu são somadas;
  - `periodo manha` seguido de `idade 5` depois do `inicio` abre uma partida nova ("Nova vida");
  - `inicio` com `save=continuado` sai marcado.
- **Autoteste:** o `--autoteste` roda num diário de exemplo escrito no próprio script e quebra se alguma conta mudar.

---

## 7. Ficha de observação (copiar como `ficha.md` em cada sessão)

| Campo | Valor |
|---|---|
| Sessão (código) | P__ |
| Data e nº da sessão no dia | |
| Faixa | 8–11 · 12–17 · 18+ |
| Joga RPG ou ação? | sim · não |
| Conhece o desenvolvedor? | sim · não |
| Menor: termo assinado e "sim" da criança | |
| Menor: responsável na sala | |
| Build (commit) | |
| Entrada e mão no mouse | mouse, modo celular · direita · esquerda |
| Configurações (mão, sensibilidade, FPS, qualidade) | |
| Ajuste (cronômetro na entrada em Auren − tempo da 1ª linha da q01) | |
| Destino e origem escolhidos | |
| Arquivo(s) do diário | `sessao_________________.txt` |

| Beat | Tempo (mm:ss) | O que aconteceu / frase literal | Etiqueta | Ajuda |
|---|---|---|---|---|
| B01 Limiar | | | | |
| B02 Destinos | | | | |
| B03 Origem | | | | |
| B04 Nome | | | | |
| B05 Confirmação | | | | |
| B06 q01 | | | | |
| B07 q02 e opcionais | | | | |
| B08 q04 (a promessa) | | | | |
| B09 q07 (o sumiço) | | | | |
| B10 Bosque | | | | |
| B11 Símbolo | | | | |
| B12 Aviso | | | | |
| B13 Salto | | | | |
| B14 Aos 8 | | | | |
| B15 Treino | | | | |
| B16 Gancho | | | | |
| (outro) | | | | |
| (outro) | | | | |

| Pergunta | Resposta (frase literal) |
|---|---|
| P1 Continuar? | |
| P2 A história | |
| P3 Escolhas | |
| P4 As quatro vidas | |
| P5 O tempo passou | |
| P6 Sem saber o que fazer | |
| P7 Mais legal / mais chata | |
| P8 Jogaria diferente? | |

| Fechamento | |
|---|---|
| Parou antes do fim? Beat, minuto e o que disse | |
| Resumo do condutor (3 linhas) | |
