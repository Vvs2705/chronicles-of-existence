# Medição: picos de quadro nas rodadas do `-roteiro`, antes e depois da passada §27

**Contexto (os CSVs não carregam):**
- **Aparelho:** Nitro ANV15-51 (Acer), RTX 3050 6 GB Laptop. **Não é o aparelho-alvo** (Android de faixa média: `BLOCKED_HARDWARE`).
- **Build:** desenvolvimento, Windows, gerada pelo `verify.ps1 -Modo completo` antes de cada par de rodadas; arte de protótipo (ADR-0008); 30 FPS.
- **Como:** o próprio verify roda `COE.exe -roteiro` (rota completa, **primeira abertura** depois do build) e `COE.exe -roteiro quebrada` (**segunda abertura**). O `PerfHud` grava um CSV por cena carregada: Bootstrap, Auren aos 5 anos e Auren de novo depois do salto (o salto recarrega a cena).
- **Antes:** três verify em `e6d078a`/árvore do passo 6, sem a passada §27 (10:45, 10:54 e 11:02). **Depois:** dois verify com a passada §27 (`817d189`: som sintetizado uma vez por processo, `HitFlash` sem bloco de propriedades fora do flash, número de dano sem refazer malha, sem `Debug.Log` por interação; 11:37 e 11:48). Nenhuma outra mudança de runtime entre os dois grupos.
- **Linha do tempo do robô** (`roteiro.txt`): rota quebrada salta em 58–60 s e treina em 109–122 s; rota completa salta por volta de 108 s e treina perto do fim (~145–170 s).
- **Coluna olhada:** `fps_min_1s` (pior quadro de cada segundo), **incluindo** os 5 s iniciais de cada CSV. O `perf_report.py` descarta esses 5 s como aquecimento, e é justamente aí que cai a recarga do salto: nenhum relatório anterior mostrava este pico.

## Pior quadro por cena (FPS instantâneo; entre parênteses, o segundo do processo)

| Rodada | Auren aos 5 (carga) | Auren depois do salto, segundo da recarga | Auren depois do salto, resto (inclui o treino) |
|---|---|---|---|
| antes, completo, 10:45 | 4,8 (7 s) ≈ 210 ms | **6,1 (110 s) ≈ 165 ms** | 13,5 (162 s) ≈ 75 ms |
| antes, quebrada, 10:48 | 3,9 (7 s) | **8,4 (60 s)** | 25,0 (66 s) |
| antes, completo, 10:54 | 4,9 (7 s) | **4,5 (110 s) ≈ 220 ms** | 13,3 (165 s) |
| antes, quebrada, 10:57 | 4,7 (8 s) | **8,5 (61 s)** | 25,4 (67 s) |
| antes, completo, 11:02 | 4,5 (7 s) | **4,8 (110 s)** | 12,1 (146 s) |
| antes, quebrada, 11:05 | 3,6 (8 s) | **4,5 (61 s)** | 12,7 (81 s) |
| depois, completo, 11:37 | 8,6 (7 s) ≈ 116 ms | 29,6 (110 s), sem pico | 25,8 (116 s) ≈ 39 ms |
| depois, quebrada, 11:40 | 8,8 (7 s) | 29,3 (59 s) | 25,7 (66 s) |
| depois, completo, 11:48 | 8,6 (7 s) | 27,3 (108 s) | 25,4 (115 s) |
| depois, quebrada, 11:51 | 8,6 (7 s) | 28,1 (59 s) | 25,3 (66 s) |

CSVs arquivados (Auren depois do salto): `2026-10-04_pc-nitro_roteiro-pos-salto_{antes,depois}_{completo,quebrada}.csv` (10:45/10:48 e 11:48/11:51). Os outros ficaram em `persistentDataPath`.

## Leitura

1. **O engasgo do salto era a música sendo sintetizada de novo.** O pico de 165–220 ms caía no segundo em que Auren recarrega, nas duas rotas e nas duas aberturas. O `SomDoJogo` gerava ~20 s de música (441 mil amostras) no `Awake` de cada cena. Com os clips guardados por processo, o segundo da recarga ficou sem pico (27–30 FPS) e o pior quadro de toda a cena depois do salto ficou em ~40 ms. A carga inicial de Auren também caiu de ~210 para ~116 ms: a síntese passou para a Bootstrap, que já tem a tela da entrada parada.
2. **Os picos de 75–80 ms perto do treino apareciam nas três rodadas de primeira abertura depois do build** (rota completa, 146–165 s); na segunda abertura (quebrada), um de 79 ms aos 81 s, fora do treino, numa das três. Depois da passada §27, nenhum quadro abaixo de 25 FPS nessa cena. O padrão bate com o relato do engasgo frio (`DIVIDA_TECNICA.md`), mas esta comparação não isola qual mudança o tirou: as candidatas são o `HitFlash` (o corpo saía do SRP Batcher no primeiro golpe e ficava fora dele) e o texto do dano refeito a cada quadro. **Hipótese, não conclusão.**
3. **O que sobra:** ~116 ms na carga de Auren (cena, NPCs, céu). É carga de cena, atrás da tela de entrada. Medir no Android antes de mexer.
4. O pico de 0,3 FPS aos 3 s em todo CSV da Bootstrap é a abertura do processo, antes do primeiro quadro útil.

## Limites

- PC não é o alvo: os números absolutos não valem para o celular, o padrão (onde o pico cai) vale.
- Cinco verify, dez rodadas; o "depois" tem duas de cada rota.
