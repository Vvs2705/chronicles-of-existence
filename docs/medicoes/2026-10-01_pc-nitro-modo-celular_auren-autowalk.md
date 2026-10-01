# Medição: 2026-10-01_pc-nitro-modo-celular_auren-autowalk.csv

**Contexto (o CSV não carrega):**
- **Build:** desenvolvimento, Windows, código de `cc698ae` + B16 e base de medição (mesma branch, antes do commit); arte de protótipo (ADR-0008).
- **Como:** `client/tools/run_windows.ps1 -Celular -Scene Auren -Seconds 65 -AutoWalk` — janela 1200×540 (metade do POCO F4), toque simulado, criança andando sozinha em quadrado, save aos 5 anos; 30 FPS é a meta do jogador (Configurações).
- **Não é o aparelho-alvo:** PC com RTX 3050. Serve de linha de base do pipeline e de regressão grosseira; o número que vale é o de Android de faixa média (decisão 7, pendente).

- **Aparelho:** Nitro ANV15-51 (Acer) · NVIDIA GeForce RTX 3050 6GB Laptop GPU
- **Amostras:** 58 úteis em 57 s (período 1.00 s); 5 de aquecimento descartadas

| Métrica | Valor |
|---|---|
| FPS mediana / p5 / mínimo (suavizado) | 29.9 / 29.9 / 29.8 |
| Amostras na meta (≥ 95% de 30 FPS) | 100% |
| Engasgos (pior quadro < 15 FPS) / pior quadro | 0 / 29.1 FPS |
| Tempo de quadro mediana / p95 | 33.4 / 33.5 ms |
| Memória alocada pico / crescimento | 98 / +0 MB |
| Temperatura início / máx / fim | n/d / n/d / n/d °C |
| Bateria início / fim | 1.00 / 1.00 |
