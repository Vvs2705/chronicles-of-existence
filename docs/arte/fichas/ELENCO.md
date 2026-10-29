# Elenco do slice — mapa de silhuetas e arbitragem

- **Para quê:** o ADR-0002 reprova o elenco que é "o mesmo boneco com roupas diferentes". As fichas foram escritas em paralelo (2026-10-03) e várias disputaram a mesma zona do corpo. Este arquivo diz quem fica com cada zona disputada, para o Art Director pontuar todas com a mesma régua e para a reescrita saber o que mudar.
- **Quem arbitrou:** o coordenador, por delegação ([ADR-0010](../../adr/ADR-0010-arte-por-delegacao.md)). O idealizador pode reverter.
- **Regra de desempate:** fica com a zona quem a usa no **objeto-assinatura (C4)**; empate entre C4, fica quem tem a necessidade de mundo (C2) mais ligada à forma.

## Mapa depois da arbitragem (2026-10-03)

A primeira versão das fichas colidia nas zonas da §"Arbitragem"; a tabela abaixo é a de depois da reescrita e das correções da W3 (2026-10-03).

| id | altura (m) | forma 1 | forma 2 | forma 3 | objeto (C4) |
|---|---|---|---|---|---|
| borin | 1,82 | braço direito em manga grossa (assimetria) | aro fechado de chapa de 40 mm, 0,10 m fora da coxa esquerda, preso por suporte de 50 mm de face | crânio raspado, pescoço projetado | aro de provas |
| mara | 1,80 | colar de manta: rolo nos ombros, sem pescoço | mangas de manta em sino até o cotovelo | barra arrancada: saia em diagonal, da canela direita (dela, Arbitragem 3) à coxa esquerda | manta da casa e as tiras no vão do bosque |
| daren | 1,70 | vara de carga de 1,90 m × Ø 0,05 m nos ombros, além das mãos | estojo do ofício: I além da mão esquerda, preso à vara por tira de 6 cm de face | caixa da carga: bloco deitado além da mão direita, preso por tira de 6 cm | vara de carga e cordão do recado |
| lysa | 1,58 | aba-varal: chapéu de aba 0,72 m, borda de 5 cm, feixes pendurados | base-balão: calça cheia na coxa, presa na canela | punhos em funil: luvas de cano largo (reserva: manopla) | chapéu de secar |
| eira | 1,92 | lousa no flanco direito, de face para a frente | cintura de cinto (recorte em X) | lanterna sob o braço esquerdo | lousa de perguntas |
| tovin | 1,76 (caixa ~1,91) | boca do chifre acima do ombro direito | estojo de estacas por fora da canela esquerda (Arbitragem 3) | canos em funil que abrem no joelho | chifre de recolher |
| oren | 1,60 (caixa 2,17; 2,10 sem o cestinho) | torre de cestos acima da cabeça | pernas em parêntese (vão oval) | bandeirola da carga, fora da torre, à esquerda | torre de cestos |
| maelis | 1,68 | nuvem grisalha: cabelo em volume redondo | vara de ofício vertical à esquerda | prancha na cintura | prancha-registro |
| nilo | 1,04 → 1,24 (caixa 1,28 aos 5) | forquilha em Y na diagonal das costas, sobre o ombro esquerdo, ≥ 4,5 cm da cabeça | redemoinho: tufo em cunha para a direita | calça enrolada em rolos nas canelas | forquilha |
| sera | 1,10 → 1,28 (1,19 → 1,37 com o coque) | coque alto centrado | bolota: colete acolchoado em ovo | coluna: calça em bloco, sem vão | tabuinha de cera |
| avatar | 1,10 → 1,28 | trouxa: rolo da manta na horizontal, nas omoplatas | sino da manta até a dobra do joelho | tamancos de sola alta | manta de nascimento |
| aethron | 1,90 (caixa 2,55 com umbral e soleira) | umbral em Π (canga, ombreiras e verga sobre a cabeça) | folha: sobreveste rígida em bloco | soleira: laje sob os pés | soleira |
| simbolo_limiar | 1,50 | anel trançado de 1,20 m | vão de 40° às 13 h | fio solto até o chão | nós da borda do vão (turquesa → dourado) |

## Arbitragem das zonas disputadas

1. **Massa centrada acima da cabeça, nas costas** — Oren, Daren e Eira. **Fica com o Oren** (a torre de cestos é o C4 dele, e ele foi o que saiu igual ao Borin no teste de silhueta com os concepts antigos). **Daren** troca o cavalete por outra forma fora do contorno, sem passar da cabeça. **Eira** tira a lousa de cima da cabeça. Tovin (chifre fora do centro, sobre o ombro direito) e Nilo (Y na diagonal, escala de criança) ficam: não são massa centrada.
2. **Faixa larga horizontal sobre a cabeça** — Lysa (aba de 0,72 m), Oren (toldo de 0,70 m) e Mara (rodilha). **Fica com a Lysa** (o chapéu é o C4 dela). **Oren** tira o toldo; a terceira forma dele passa a ser a estatura (o homem mais baixo). **Mara** troca a rodilha por uma forma que não seja disco nem massa sobre a cabeça.
3. **Ombros sem pescoço** — Mara (rolo de manta) e Tovin (gola em sino). **Fica com a Mara** (a manta é o C4 dela). **Tovin** troca a gola por outra forma.
4. **Prancha na frente da cintura** — Maelis. **Fica com a Maelis** (C4). A lousa da Eira não pode ir para a frente da cintura; a tabuinha da Sera é pequena demais para ser forma e não conta.
5. **Altura:** dois homens baixos (Daren 1,62, Oren 1,60). **Oren fica com o "mais baixo"**; Daren passa para 1,70 m.

## Arbitragem 2 — depois das notas (2026-10-03)

As 12 fichas passaram no G1 por delegação, todas com condições. Estas decisões cruzam fichas; cada autor cumpre as da sua antes do concept.

1. **Sumiço do Nilo.** Ficam quatro marcos, todos com o **mesmo gatilho**: do `evento.nilo_desapareceu` até o salto (`marco_idade_8`).
   - a forquilha do Nilo, a pista;
   - o chifre calado do Tovin, o único pelo som;
   - a tira da Mara, a espera no caminho da q07, que sai do vão no salto e vai para a prateleira;
   - a folha da Maelis, o único marco que **continua depois do salto** (fala canônica "não fecho").
   **Sai** o estado da q07 na lousa da Eira, que repetia a folha da Maelis.
2. **A linha do bosque tem gente demais.**
   - O alcance por cima da linha é do Nilo, com a forquilha.
   - A Lysa troca a pose de C1 e C7 e não alcança o outro lado.
   - A Maelis sai da `entrada_bosque`: o lugar dela é o mural e a praça.
   - As pedras da Lysa aos 8 são cortadas.
3. **Lado de fora do quadril e da coxa.** A dona é a do **Borin** (o aro é o C4 dele).
   - O podão do Tovin sai dessa faixa.
   - A vara da Maelis (um I) fica, como vizinha; o G2 confere.
   - A lousa da Eira (no flanco, acima da coxa) e as caixas do Daren (penduradas da canga) ficam e também passam pela conferência do G2.
4. **Braços.** O braço inteiro e grosso é do **Borin**.
   - As mangas em sino até o cotovelo, nos dois lados, são da **Mara**.
   - As luvas de cano largo no pulso são da **Lysa**.
   - As mangas-bolso da Eira saem; ela precisa de outra segunda forma.
5. **"A mais alta" é a Eira** (1,92 m). A Mara (1,80 m) deixa de usar a altura como forma e corrige na ficha dela as medidas antigas da Eira.
6. **Estatura conta pouco.** O `silhueta.py` põe na folha a caixa inteira da figura, e a torre do Oren o faz a silhueta mais alta. Estatura como forma vale no máximo como apoio. Quem tem C3 = 1 por causa disso troca por uma forma de contorno. No teste, `@altura_m` é a altura da **caixa** (Oren com a torre: 2,17 m).
7. **Detalhe fino some.** A limpeza do `silhueta.py` apaga traço com menos de ~25 mm num adulto (2–3 px a 30%), e a 30% o olho também não vê. O aro do Borin passa de barra de 20 mm para **chapa de ferro de 40 mm de largura, de face para a frente**. A aba da Lysa tem de ter borda de pelo menos 4 cm vista de frente.
8. **Amuleto da Ruptura.** Só **Borin** (não mede) e **Oren** (não rastreia) reagem a ele com "não sei o que é". As outras fichas tiram essa reação.
9. **Ler o que a criança carrega** é exclusivo do **Oren**. A Mara troca o C5 dela.

## Arbitragem 3 — correções da W3 (2026-10-03)

Decidida pela raia W3 por delegação ([ADR-0010](../../adr/ADR-0010-arte-por-delegacao.md)), com a regra de desempate deste arquivo. O coordenador e o idealizador podem reverter.

1. **Canela direita** — a barra arrancada da Mara desce até 0,35 m do chão na perna direita, e o estojo de estacas do Tovin ocupava o lado de fora da mesma canela, de 0,08 a 0,40 m. As duas fichas davam a faixa como livre (conferência final das duas). **Fica com a Mara:** a barra é de onde saem as tiras, o C4 dela; o estojo não é C4 do Tovin (o C4 dele é o chifre). **O Tovin** passa o estojo para a canela **esquerda**, com a mesma altura, forma e medidas. Do lado esquerdo, abaixo de 0,40 m, não há forma de ninguém: o aro do Borin começa em 0,81 m, a vara da Maelis em 0,40 m (fora do corpo), e a perna esquerda da Mara aparece sem nada abaixo de 0,70 m. O G2 confere Tovin, Mara e Maelis na mesma folha.

## Decisões de enredo que cruzam fichas

- **O traço do avatar no símbolo (B11–B13):** a criança traça **de pé**, com o braço erguido até o vão; **não atravessa** o vão. O símbolo fica com 1,50 m (é o que o código e o enquadramento do B01 já usam) e o salto segue como está. A ficha do avatar ajusta C5 e C9.
- **Vínculo Borin/Oren:** pela convenção do `NpcCatalog.cs` (o rótulo descreve quem é dono da entrada, como "freguesa", "superiora", "aluna"), os rótulos estavam invertidos. Corrigido no código em 2026-10-03: Borin é `cliente` de Oren, Oren é `fornecedor` de Borin.
- **O sumiço de Nilo mexe em cinco objetos** (tira da Mara, folha da Maelis, lousa da Eira, chifre calado do Tovin, forquilha do Nilo). O Art Director diz se isso é a vila reagindo, o que é bom, ou o mesmo truque cinco vezes, e recomenda cortes.

## Pendências de código que as fichas criam (não são arte)

Pagas em 2026-10-03 (leva C1/C2, `PROJETO.md` §6):

- ~~**Peça ligada por evento**~~: `Scripts/World/PecaPorEvento.cs` (evento do histórico ou estado de missão). Em cena hoje: forquilha do Nilo, tiras da Mara, folha da Maelis, bancada do Borin e o chapéu da Lysa (`Editor/PecasDeEventoSetup.cs`). Ainda sem peça (pedem malha ou adereço de corpo): prova no aro do Borin, tabuinha da Sera, cestinho do Oren, manta do avatar, página assinada da Maelis.
- ~~**Condição de diálogo por destino e por item**~~: `Condicao.Destino/Origem/TemItem`; os 10 NPCs têm fala de entrada por destino (`DialogueNascimentoTests`).
- ~~**q07, a assinatura como desfecho**~~: dois desfechos (`evento.q07_assinou_com_o_circulo`, `evento.q07_assinou_com_um_risco`) cobrados ao cumprir `perguntar_na_vila` (`QuestDef.ObjetivoDoDesfecho`), botões só com a Maelis, Maelis testemunha e lembra aos 8; save antigo conclui sem desfecho. Falta: o gesto de traço do avatar (C5) no lugar do botão do círculo.
- ~~**q05, o chapéu emborcado e "chegar devagar"**~~: `Scripts/World/BichoNoChapeu.cs` (regra pura `ChegarDevagar`); a opção de tratar o animal só aparece com o bicho calmo, com Lysa ou Tovin. Falta: o chapéu na cabeça da Lysa (o protótipo não tem) e o bicho visível.

Abertas:

- **Crianças por proporção, não por escala:** o `NpcActor` cresce as crianças com escala uniforme, o que vai contra o "não escalonar" da `PIPELINE.md`. Vale quando a malha real entrar.

## Episódio 2 — criaturas (2026-10-29, PROPOSTA)

- **Estado:** PROPOSTA. As quatro fichas vêm do roteiro do episódio 2 ([`EPISODIO2_DESPERTAR_DOS_TALENTOS.md`](../../design/EPISODIO2_DESPERTAR_DOS_TALENTOS.md) §3.4), que ainda não foi aprovado. Cada ficha traz uma autoavaliação do autor e espera a nota de um Art Director separado ([ADR-0010](../../adr/ADR-0010-arte-por-delegacao.md) §6). As linhas e arbitragens acima não mudam.
- **Régua:** criatura não tem T-pose nem roupa; as três formas são estados ou partes do corpo, e a altura é a da caixa no estado mais alto. O teste de silhueta do G2 é adaptado em cada ficha (§6), como o do `simbolo_limiar`.

| id | altura / caixa (m) | forma 1 | forma 2 | forma 3 | objeto (C4) | autoavaliação |
|---|---|---|---|---|---|---|
| abafador | 0,30 fechado; 0,80 com o leque | monte de folha de 0,45 × 0,30 m, sem rosto | leque: semicírculo cheio de 0,70 × 0,50 m atrás do corpo, 7 nervuras | pulo raso: o corpo todo sai do chão, até 0,25 m | o leque com os dois olhos marfim, que só aparecem no aviso | 18/20 |
| feixe_de_espinho | 1,30 fechado; 1,80 aberto | feixe fechado: fuso de galhos de 1,30 × Ø 0,40 m, igual ao espinheiro | tripé em Λ, base de 1,40 m, miolo pendurado a 0,75–1,05 m | coroa eriçada: ~20 espinhos de 0,25 m no terço de cima, Ø 0,90 m | o miolo (bola de raiz Ø 0,30 m), único ponto de dano | 17/20 |
| lajeiro | 1,00 × 1,80 de comprimento | mesa baixa: retângulo deitado sobre quatro pés | escada de cinco lajes em telha, a viseira em pá inclinada a 0,15 m do chão | pilões: patas retas Ø 0,25 m, vão de 0,35 m sob a barriga | as lajes do dorso: a frente só empurra, a junta aberta derruba laje | 17/20 |
| enredado | 2,00 de corpo × 3,50; caixa 4,60 com o anel | massa de lajes (lajeiro velho, o dobro) | anel de três fios trançados, Ø 2,40 m, vão de 40° às 13 h, de 2,20 a 4,60 m | três fios: à viseira, ao chão 3 m à esquerda, e do topo à cauda | a trança que perde um fio a cada nó cortado | 18/20 |

**Conferência com o elenco humano (zonas que poderiam confundir a leitura):**

1. **Nenhuma criatura disputa zona do corpo de um NPC**: criatura não veste nada, e as formas delas são a figura inteira. O risco é de rima entre a figura de uma e a peça de outro, na mesma folha ou na mesma cena:
   - **Tripé do feixe (Λ) × forquilha do Nilo (Y):** o Y é de criança, nas costas, ao lado da cabeça, e não passa de ~1,30 m; o Λ é a figura inteira, de 1,80 m, com o miolo no meio. Os dois estão juntos no espinhal (q13). O G2 põe os dois na mesma folha.
   - **Feixe de varas × estojo de estacas do Tovin (canela esquerda, Arbitragem 3):** mesmo motivo, escala e cor diferentes (0,08–0,40 m, freixo claro e reto, preso a uma pessoa, contra 1,30 m de galho torto e escuro). Sem conflito.
   - **Lajes do lajeiro × soleira do Aethron:** a laje sob os pés é do Aethron (é o C4 dele). As lajes do lajeiro são do dorso, deitadas em escada, sem a cava dourada. O G2 confere os dois na mesma folha.
   - **Anel do Enredado × `simbolo_limiar`:** a rima é **de propósito** (o Enredado tem de ler como o símbolo). O que separa é a altura (o anel do Enredado fica de 2,20 a 4,60 m; o do símbolo, de 0 a 1,50 m), o Violeta (só no Enredado) e os fios (dois esticados até o chão contra um solto e ondulado).
   - **Leque do abafador × aba da Lysa:** a aba é horizontal, na cabeça de uma pessoa, a ~1,5 m; o leque é vertical, cheio, abaixo de 0,80 m. Sem conflito; é o mesmo bicho da q05 dela (ficha `abafador`, §8).
2. **Altura:** o anel do Enredado (4,60 m) passa a ser a caixa mais alta do jogo; a torre do Oren (2,17 m) continua sendo a mais alta entre as pessoas (Arbitragem 2, item 6, não muda).
3. **Paleta:** o `EP2` dava ao feixe uma casca de "Terracota dessaturada". A ficha `feixe_de_espinho` propõe tirar a casca da família da Terracota, para o cobre continuar sendo só a marca do jogador (ADR-0010, item 4). Violeta só no Enredado (anomalia) e no fio preso no espinhal; Turquesa só como alma dos fios (Trama). Nenhuma criatura usa Dourado.
4. **Pendência do `EP2` §6.2, item 7 (lajes da praça e Oren):** resolvida na ficha `lajeiro`, C2, **contra** a ligação com o Oren (a ficha `oren` diz que tudo entra pelo portão sul, e a Arbitragem 2, item 8, que ele não rastreia). A ficha `oren` não muda.
5. **Ainda sem ficha:** o fio de luz preso no espinhal (q13) é peça de cena da Trama, coberto pelas regras de cor do Enredado; se for modelado à parte, entra como Vfx.
