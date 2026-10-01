# ADR-0006 — Plataforma de lançamento: mobile, Android primeiro

- **ID:** ADR-0006
- **Data:** 2026-09-29
- **Estado:** APROVADO pelo idealizador em 2026-09-29: "esse jogo é mobile, não quero lançar ele para computador agora".
- **Substitui:** a plataforma do dossiê §B e do GDD v1.2 ("PC/Windows") e a linha do `CLAUDE.md` que punha "Android/mobile, HUD de toque" fora de escopo. O resto do dossiê §B continua valendo: action RPG narrativo, terceira pessoa, single-player.

## Decisão

1. **Alvo de lançamento: mobile.** Android primeiro. iOS é FUTURO: exige Mac para build, e a máquina de produção é Windows.
2. **Orientação: paisagem.** Ação em terceira pessoa com câmera orbital pede tela larga. HIPÓTESE A VALIDAR no primeiro teste em aparelho.
3. **Controle: toque** (joystick virtual e botões de ação), com gamepad Bluetooth como opção. O teclado fica só para desenvolvimento.
4. **PC/Windows deixa de ser alvo de lançamento.** A build Windows continua como ferramenta de desenvolvimento: teste rápido, captura de tela, CI local. Nada de produto é decidido por ela.

## Consequências técnicas

- O caminho de toque que a T002 removeu volta (a versão anterior está no commit `889f2fd`), agora ligado às ações de combate da T011.
- `ProjectSetup` passa a configurar o Player de Android: paisagem, IL2CPP, ARM64, API mínima e gráficos. Entra uma build Android por script, ao lado da de Windows.
- A dívida técnica que mandava tirar o "jeito de celular" se inverte: `URP_Base` (afinado para GPU de celular) e as colunas de bateria e temperatura do `PerfHud` ficam.
- `docs/arte/PIPELINE.md` §4 passa a ter orçamentos de celular (triângulos, texturas, materiais). Os números continuam HIPÓTESE até existir um aparelho mínimo definido.
- UI, texto e alvo de toque precisam ser legíveis em tela pequena. O HUD em `OnGUI` continua sendo de protótipo (T013).

## Pendências (decisão do idealizador)

- Aparelho mínimo de referência (modelo ou faixa de GPU e RAM). Sem ele, orçamento e metas de FPS são hipótese.
- Loja e formato: APK para teste interno; AAB para a Play Store, quando houver lançamento.
- Se o gamepad é suporte oficial ou só conveniência. *(Nota de 2026-09-30: respondida pelo [ADR-0007](ADR-0007-decisoes-da-leva-a.md) §8. É conveniência de desenvolvimento, não suporte oficial.)*
