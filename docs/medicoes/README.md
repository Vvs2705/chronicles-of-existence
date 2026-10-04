# Medições de desempenho

Todo número de desempenho citado em `docs/` (FPS, memória, temperatura) aponta para um arquivo daqui. Número sem arquivo é **declarado**, não medido (`docs/PROJETO.md` §2.2).

## Como medir

1. Build de **desenvolvimento** (só ela grava CSV; release não escreve arquivo no aparelho de quem joga).
2. Rodar a cena. O `PerfHud` grava `perf_<aparelho>_<data>.csv` a cada 1 s em `persistentDataPath`:
   - PC: `%USERPROFILE%\AppData\LocalLow\V-STACK\Chronicles of Existence\`
   - Android: `/storage/emulated/0/Android/data/br.com.vstack.coe/files/` (`adb pull`)
3. Gerar o relatório: `python client/tools/perf_report.py <csv> --md <mesmo-nome>.md`. Os primeiros 5 s (carga da cena) ficam fora do resumo (`--aquecimento`).
4. Copiar o CSV para cá com o nome `AAAA-MM-DD_<aparelho>_<cena-e-roteiro>.csv`, o `.md` ao lado, e escrever no topo do `.md` o contexto que o CSV não carrega: commit, tipo de build, cena, roteiro, ajuste de FPS e se é ou não o aparelho-alvo. A faixa gráfica (ADR-0009) já vem no CSV, na coluna `qualidade`; para medir uma faixa específica, abra com `-qualidade baixa|media|alta` (`run_windows.ps1 -Qualidade`; no Android, no extra `unity` da intent).

## O que o relatório mostra

- **FPS suavizado** (mediana, p5, mínimo) e **% de amostras na meta** (≥ 95% de `--meta-fps`, padrão 30).
- **Engasgos:** amostras cujo pior quadro do segundo (`fps_min_1s`) ficou abaixo de metade da meta. O FPS suavizado esconde um quadro de 300 ms; esta coluna não.
- **Pior quadro na carga:** os primeiros segundos ficam fora do resumo, mas o pior quadro deles sai numa linha à parte. O salto recarrega a cena no meio da partida, e o engasgo dele cai inteiro no aquecimento do CSV novo (2026-10-04).
- **Memória** alocada (pico e crescimento do início ao fim: crescimento contínuo é suspeita de vazamento), **temperatura** e **bateria** (no PC, a temperatura vem `n/d`).

`python client/tools/perf_report.py --autoteste` confere o próprio script.
