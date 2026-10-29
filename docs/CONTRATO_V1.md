# Contrato da v1 de loja — PROPOSTA (2026-10-29)

**Estado:** PROPOSTA. O tamanho da v1 é decisão do idealizador (cartão aberto em 2026-10-29). Este texto segue a opção recomendada, "episódio 1 grátis + desbloqueio único", e muda se ele escolher outra. Nada aqui altera o GDD (CLAUDE.md: mudança de produto vira proposta em `docs/`).

**Fontes:** [`GDD.md`](GDD.md) §0 (placar: 106 de 158 requisitos feitos, 67,1%), [`COMPETITIVO.md`](COMPETITIVO.md) (diferenciais C1–C5, hipóteses H1–H9, modelo de negócio), [`design/SLICE_A_PRIMEIRA_EXISTENCIA.md`](design/SLICE_A_PRIMEIRA_EXISTENCIA.md), GDD cap. 03 (etapas de vida), ADR-0004, ADR-0006 e ADR-0009.

## 1. O que é a v1

| | Episódio 1 — "A Primeira Existência" | Episódio 2 — "Despertar dos Talentos" |
|---|---|---|
| Idade | 5 → 8 anos (o slice de hoje, B01–B16) | 8 → 11 anos (GDD cap. 03) |
| Lugar | Auren + borda do Bosque dos Sussurros | Auren + Bosque dos Sussurros (mais nada: CLAUDE.md) |
| Preço | grátis e completo, termina no gancho (B16) | desbloqueio único, conteúdo fixo e listado na loja |
| Duração-alvo | 45–75 min (H1, medir) | 60–90 min (HIPÓTESE) |
| Estado | jogável por robô; falta o que se vê e se ouve (§3) | só existe no GDD; precisa de roteiro beat a beat |

**Fora da v1:** ascensão completa, regiões além de Auren, cooperativo, IA generativa em runtime, aparência no slice (ADR-0007 §5), anúncio de qualquer tipo, iOS.

## 2. Modelo de negócio (guardrails)

- Um produto não consumível (`coe.episodio2`), comprado uma vez, com restauração de compra e IAP em dois passos (concede, depois confirma). Loja falsa no PC para teste.
- Nada aleatório pago, nenhuma moeda premium, nenhuma energia. Sem anúncio na v1, o que tira a carga do SDK de Famílias e do perfilamento (público misto com crianças, ADR-0009).
- A compra aparece **só** no fim do episódio 1 e no menu, nunca no meio de uma cena.
- Preço: abaixo do Grimvalor em dólar, com preço regional no Brasil; valor fecha depois do tamanho do episódio 2 (H7).

## 3. Critério de pronto da v1

1. Toda linha do `GDD.md` §0 que pertence aos episódios 1 e 2 está **feito** ou **pendente (externo)**.
2. Diferenciais C1, C2, C3 e C5 visíveis nos primeiros 3 minutos (C3 com a fala de reconhecimento da origem no despertar, proposta do `COMPETITIVO.md`).
3. Arte final no lugar de toda peça `PROTOTIPO` (ADR-0008 §2): nada marcado PROTOTIPO entra no AAB.
4. Lacunas visíveis do slice fechadas: espada de madeira como item e objeto na mão (B15), itens em tela, reputação lida por pelo menos uma fala por NPC central, áudio de ambiente de Auren e motivo do Limiar.
5. Localização em pt-BR, EN, ES e ID (hoje só `strings.pt-BR.json`).
6. `verify.ps1 -Modo completo` verde, `-roteiro` nas duas rotas e nos dois episódios, fotos de cada tela conferidas em 20:9 e 16:9.
7. Medição no Android de entrada (ADR-0009, H8) sem queda abaixo de 25 FPS na faixa Baixa.
8. AAB assinado com a chave de upload, política de privacidade publicada nos 4 idiomas, ficha da loja, ícone e capturas, e backup completo.

## 4. Conteúdo

### Episódio 1 (existe; polir)

- 10 NPCs de Auren, avatar (5 e 8 anos), Guardião Aethron e o símbolo do Limiar.
- 8 missões (`content/quests/q01`–`q08`): 5 centrais e 3 opcionais.
- 6 estruturas do greybox (`casa_familia`, `casa_nilo`, `casa_sera`, `ferraria`, `ervanaria`, `posto_guarda`), a praça e a borda do bosque.
- Treino supervisionado: mover, atacar, defender e primeira magia; espada de madeira.

### Episódio 2 (a desenhar; PROPOSTA de escopo)

- Seis a oito missões novas em Auren e no bosque: treino com Tovin, estudo com Eira e Maelis, ofício com Borin, e o fio do desaparecimento do Nilo e da Trama.
- Afinidades do GDD cap. 03 (Marcial, Arcana, Natural, Artesanal, Social, Exploratória) com efeito real; os seis atributos passam a ser usados por regra (hoje crescem sem uso, `GDD.md` §0).
- Primeiro combate de verdade no bosque: 3 tipos de criatura e 1 encontro maior no fim, com assistências de combate decididas (ADR-0004).
- Variantes por origem que se vejam (casa, família, uma fala), para a H5.
- Gancho do episódio 3 sem prometer data.

Antes de produzir: roteiro beat a beat do episódio 2 em `docs/design/`, no mesmo formato do SLICE, aprovado pelo idealizador.

## 5. Números iniciais

| Item | Valor | Fonte |
|---|---|---|
| FPS alvo | 30 sustentados, `fps_min_1s` ≥ 25 | PIPELINE §4.1 |
| Tris visíveis por quadro | ≤ 150 000 | PIPELINE §4 |
| Draw calls | ≤ 200 | PIPELINE §4.1 |
| AAB | ≤ 150 MB sem asset pack (hoje 50,5 MB) | limite da Play |
| API | alvo 36, mínima 26, ARM64, 16 KB | PROJETO §0 |
| Conversão do desbloqueio | ≥ 2% de quem termina o ep. 1 | COMPETITIVO H7 |

## 6. Ordem de produção proposta

1. Fechar as lacunas visíveis do episódio 1 (§3 item 4) e a fala de reconhecimento da origem (C3).
2. G3 dos 4 personagens aprovados com objetos separados, depois G2 dos restantes.
3. Localização e fontes OFL (ART_BIBLE §5).
4. Roteiro do episódio 2 → aprovação → conteúdo.
5. IAP do desbloqueio, política de privacidade, ficha da loja.
6. Medição no Android, playtest com gente, AAB assinado.
