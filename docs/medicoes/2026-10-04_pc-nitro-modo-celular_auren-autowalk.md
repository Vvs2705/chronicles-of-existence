# Medição: 2026-10-04_pc-nitro-modo-celular_auren-autowalk.csv

**Contexto (o CSV não carrega):**
- **Build:** desenvolvimento, Windows, branch `claude/coe-baseline-validation-b75470` em `796ea93` (hardening do Prompt Mestre: UI de jogador em uGUI, manifest enxuto); arte de protótipo (ADR-0008).
- **Como:** o mesmo roteiro da medição de 2026-10-01: `client/tools/run_windows.ps1 -Celular -Scene Auren -Seconds 65 -AutoWalk` — janela 1200×540, toque simulado (os controles agora desenhados em uGUI pelo `ToqueHud`), criança andando sozinha em quadrado, save do PC (restaurado com o mesmo md5 depois).
- **Comparação com 2026-10-01 (antes da UI em uGUI):** FPS e engasgos iguais (29,9 / 0); memória alocada 103 MB contra 98 MB (+5 MB, os Canvas e texturas das telas uGUI). Sem regressão de quadro.
- **Não é o aparelho-alvo:** PC com RTX 3050. O número que vale é o de Android de faixa média (`BLOCKED_HARDWARE`: nenhum aparelho ligado nesta sessão).

- **Aparelho:** Nitro ANV15-51 (Acer) · NVIDIA GeForce RTX 3050 6GB Laptop GPU
- **Amostras:** 58 úteis em 57 s (período 1.00 s); 5 de aquecimento descartadas

| Métrica | Valor |
|---|---|
| FPS mediana / p5 / mínimo (suavizado) | 29.9 / 29.8 / 29.8 |
| Amostras na meta (≥ 95% de 30 FPS) | 100% |
| Engasgos (pior quadro < 15 FPS) / pior quadro | 0 / 29.2 FPS |
| Tempo de quadro mediana / p95 | 33.4 / 33.5 ms |
| Memória alocada pico / crescimento | 103 / +1 MB |
| Temperatura início / máx / fim | n/d / n/d / n/d °C |
| Bateria início / fim | 1.00 / 1.00 |

Colunas extras (contadores de jogo): qualidade
