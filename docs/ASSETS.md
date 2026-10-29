# Lista de assets da v1 (2026-10-29)

Lista fechada do que a v1 precisa, com prioridade e custo estimado no Tripo. Base: [`CONTRATO_V1.md`](CONTRATO_V1.md) (PROPOSTA), elenco em [`arte/fichas/ELENCO.md`](arte/fichas/ELENCO.md), protótipos em `client/Assets/_COE/Art/Prototipo/` e direção em [`ART_BIBLE.md`](ART_BIBLE.md).

- **Prioridade:** 1 = sem ele não há jogo; 2 = o jogo fica pobre sem ele; 3 = troca por variante de cor ou por kit.
- **Portão:** nenhuma malha final é gerada sem ficha G1 aprovada, concept G2 aprovado e registro na PROVENIENCIA (ADR-0002, CLAUDE.md). O Prompt Mestre do estúdio libera crédito do Tripo sem pedir peça por peça; no COE isso vale **dentro** dos portões, por lote autorizado (`arte/PROMPTS_G2.md`, "Quem gera").
- **Custo (estimativa do Dungeon Courier):** modelo HD ≈ 45 créditos, Auto Rig ≈ 20, imagem de concept grátis. O saldo do plano Max não está registrado: conferir antes do primeiro lote e anotar na PROVENIENCIA.
- **Objeto separado do corpo:** cada objeto-assinatura vira peça própria presa a osso (coluna "objeto").

## 1. Personagens

| id | ep. | prio | G1 | G2 | objeto separado | custo (HD + rig + objeto) |
|---|---|---|---|---|---|---|
| avatar (5 e 8 anos, um rig) | 1 | 1 | ok | falta | manta de nascimento, espada de madeira | 45 + 20 + 2×45 = 155 |
| aethron | 1 | 1 | ok | falta | soleira, umbral | 45 + 20 + 45 = 110 |
| mara | 1 | 1 | ok | devolvida à ficha | manta da casa | 45 + 20 + 45 = 110 |
| daren | 1 | 1 | ok | **aprovado** | vara de carga, estojo, caixa | 45 + 20 + 45 = 110 |
| nilo (5 e 8) | 1 | 1 | ok | falta | forquilha | 45 + 20 + 45 = 110 |
| sera (5 e 8) | 1 | 1 | ok | falta | tabuinha de cera | 45 + 20 = 65 (tabuinha por kit) |
| borin | 1 | 1 | ok | **aprovado** | aro de provas | 45 + 20 + 45 = 110 |
| tovin | 1 | 1 | ok | **aprovado** | chifre de recolher, estojo de estacas | 45 + 20 + 45 = 110 |
| maelis | 1 | 2 | ok | **aprovado** | vara de ofício, prancha | 45 + 20 + 45 = 110 |
| eira | 1 | 2 | ok | falta | lousa, lanterna | 45 + 20 + 45 = 110 |
| lysa | 1 | 2 | ok | falta | chapéu de secar | 45 + 20 + 45 = 110 |
| oren | 1 | 2 | ok | falta | torre de cestos | 45 + 20 + 45 = 110 |
| parceiro_treino | 1 | 3 | falta | falta | — | variante de cor de NPC adulto (0) |
| criatura do bosque × 3 | 2 | 1 | falta | falta | — | 3 × (45 + 20) = 195 |
| criatura maior (fim do ep. 2) | 2 | 1 | falta | falta | — | 45 + 20 = 65 |

Subtotal dos personagens: ≈ 1 700 créditos.

## 2. Símbolo, estruturas e props

| id | ep. | prio | custo | observação |
|---|---|---|---|---|
| simbolo_limiar | 1 | 1 | 45 | ficha G1 ok; VFX de fios em turquesa por shader |
| casa_familia, casa_nilo, casa_sera | 1 | 1 | 3 × 45 | variantes por origem (H5) por troca de material, sem gerar de novo |
| ferraria, ervanaria, posto_guarda | 1 | 1 | 3 × 45 | footprint do greybox é teto (PIPELINE §3.2) |
| kit modular `modulo_*` (parede de taipa, telhado, cerca) | 1 | 2 | 4 × 45 | reaproveitado por todas as estruturas |
| props: poço, bigorna, barril, caixote, cesto, banco, lanterna, mural de avisos | 1 | 2 | 8 × 45 | prop pequeno pode sair por textura grátis + Blender |
| vegetação: árvore × 3, arbusto × 2 | 1 | 2 | 5 × 45 | variação por escala e cor |
| bosque: tronco caído, rocha, raiz, pedra do Limiar | 2 | 2 | 4 × 45 | — |

Subtotal: ≈ 1 300 créditos.

**Total estimado da v1: ≈ 3 000 créditos.** Se o saldo não cobrir, corta-se da prioridade 3 para a 2 e entram variantes por cor.

## 3. Animação (sem crédito do Tripo)

- Reaproveitar os clipes Mixamo já no projeto (`Art/Prototipo/Animacoes/`: idle, andar, correr, soco, esquiva, reação, morte, falar, acenar) e os `arte/mixamo/` dos outros jogos do estúdio, com SHA conferido e "reuso <projeto>" na PROVENIENCIA.
- Baixar do Mixamo só o que falta: ataque com espada (3 golpes), defesa, conjuração da primeira magia, carregar cesto, ajoelhar, criança correndo.
- Criatura sem forma humana: rig genérico e animação por script no Blender.

## 4. Interface, ícone e áudio (sem crédito do Tripo)

| Peça | prio | Como |
|---|---|---|
| Fontes Cinzel + Nunito (OFL) | 1 | download com licença junto (ART_BIBLE §5) |
| Molduras e botões 9 fatias | 1 | gerados por código |
| Ícone do app (adaptativo) | 1 | pintado a partir do concept do avatar aprovado no G2; hoje as texturas do ícone estão vazias (`GDD.md`, "Para chegar à loja") |
| Retratos dos 10 NPCs para a conversa | 2 | pintados a partir do concept G2 de cada um |
| Ícones de item e de atributo | 2 | pintados, um estilo só |
| Ambiente de Auren, motivo do Limiar, chifre do Tovin, passos | 1 | sintetizado por script ou com licença registrada |
| Feature graphic 1024×500 e capturas | 1 | do jogo real, depois da arte final |

## 5. Ordem dos lotes

1. **Lote A (agora, prioridade 1 com G2 aprovado):** Borin, Daren, Tovin e os objetos de cada um. Maelis no mesmo lote, se houver saldo.
2. **Lote B:** G2 da segunda folha (Oren, Lysa, Eira, Aethron) e das crianças (Nilo, Sera, avatar); depois as malhas.
3. **Lote C:** estruturas, kit modular e símbolo.
4. **Lote D (ep. 2):** fichas G1 das criaturas → G2 → malhas.
