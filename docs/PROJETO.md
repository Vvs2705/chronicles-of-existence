# CHRONICLES OF EXISTENCE — memória do projeto

Única memória viva do projeto. Vive no repositório e é atualizada ao fim de cada fase. **O estado atual é o §0.** O §7 é histórico: registra o que foi feito ou conferido em cada data e não deve ser lido como estado de hoje.

**Última atualização:** 2026-10-04 (baseline do Prompt Mestre de hardening técnico: suítes, roteiros e build rodados; documentação reconciliada).
**Estado de maturidade:** nível 2+ — os sistemas do slice estão implementados, ligados em runtime e jogáveis do Limiar ao gancho por um robô (`-roteiro`, as duas rotas). **Ainda não é o slice apresentável:** UI de protótipo (IMGUI), arte de protótipo (ADR-0008) com o G3 em andamento fora da `main`, nenhum playtest com gente e nenhuma medição no aparelho-alvo.

Ordem de leitura para quem chega: prompt-mestre → dossiê → GDD → backlog → este documento.

---

## 0. Snapshot atual (2026-10-04)

Medido no worktree `coe-baseline-validation-b75470`, branch `claude/coe-baseline-validation-b75470`, sobre a `main` em `36064b0` (merge do PR #18). Unity em batch, com o worktree mapeado para `W:` (ver §4: sem isso o caminho longo gera 6 erros falsos de import do URP).

| Área | Estado real | Evidência |
|---|---|---|
| Editor e pacotes | Unity 6000.3.23f1; URP 17.3.0, Input System 1.14.0, uGUI 2.0.0, Test Framework 1.5.1, 32 módulos do template; Tripo Bridge 1.0.14 embutido (só Editor) | `ProjectVersion.txt`, `Packages/manifest.json` |
| Código | 92 `.cs` de runtime, 22 de editor, 74 de teste | contagem em `client/Assets/_COE/` |
| Compilação | 0 erro, 0 aviso de C#; 0 erro de import | `editmode.log` do baseline |
| EditMode | **579/579** (`COE.Tests` 496, `COE.EditorTests` 83) | `-runTests -testPlatform EditMode` |
| PlayMode | **27/27** | `-runTests -testPlatform PlayMode` |
| Dados | `validate_quests.py` 11/11 (8 missões, 23 objetivos, 5/5 centrais alcançáveis); `perf_report.py --autoteste` ok; `silhueta.py --teste` ok | Python 3.14, só biblioteca padrão |
| Simulação da partida | `-roteiro` **ROTEIRO OK** em 174 s (promessa cumprida, todas as opcionais, 10 NPCs aos 8, gancho); `-roteiro quebrada` **ROTEIRO OK** em 123 s | `roteiro.txt` de cada rota |
| Build Windows (dev) | `BuildSummary(win) result=Succeeded`, 179,8 MB, 0 erro; cenas regeradas iguais byte a byte (`git status` limpo depois do build) | `build_windows.ps1` |
| Build Android | não rodado neste baseline: nenhum aparelho no `adb` e o idealizador pediu nenhum APK novo até a versão final. Toolchain presente (módulo Android, build-tools 36.0.0) | `adb devices` vazio — `BLOCKED_HARDWARE` para tudo que pede aparelho |
| Aparelho | só o POCO F4 (Android 14, acima do alvo), 2026-09-29, graybox, **sem CSV arquivado**; faixas Baixa/Média/Alta (ADR-0009) nunca medidas em celular | `docs/medicoes/` só tem PC em modo celular |
| CI e verificação | não há CI (`.github/` ausente) nem comando único de verificação; não há `.claude/agents` nem skills do projeto | — |
| Playtest com gente | protocolo pronto ([`docs/qa/PLAYTEST.md`](qa/PLAYTEST.md)) e diário de sessão gravando; **nenhuma sessão rodada** | — |
| Trabalho fora da `main` | G3 lote 1 (malhas do Tripo de Borin, Maelis, Tovin, Daren) no commit `c22b204` da branch `claude/orquestrador-game-dev-aa015f`, não mergeado; lote 2 e corpos sem objeto **sem commit** no worktree `coe-disk-cleanup-b9dd70`. Não tocado por este baseline | `git log main..c22b204`, `git status` daquele worktree |

### Tarefas do backlog (reconciliadas em 2026-10-04)

"Aceita" = regra implementada com teste verde no Unity; não quer dizer jogável.

| Tarefa | Estado | Evidência |
|---|---|---|
| T001–T011 | aceitas | testes `Obrigatorio1..8_*`, `T005_*`, `T010_*`, `T011_*` verdes (579/579) |
| T012 integração | **feita** — fiação e conteúdo. Nascimento → Auren → 8 missões → salto → treino → gancho, com fala dos 10 NPCs, diálogo por destino/origem/item, passagem do dia (ADR-0007), a vila reagindo a eventos (`PecaPorEvento`) | `SliceInteiroTests` (4 destinos × 2 desfechos), os dois `-roteiro` |
| T013 arte e UI | **em curso.** Protótipo de estética no jogo (ADR-0008, marcado PROTOTIPO). G1: 13 de 13 fichas aprovadas. G2: Borin, Maelis, Tovin e Daren aprovados (folha 1); Mara voltou ao autor. G3: lote 1 fora da `main` (acima). UI ainda é IMGUI | `docs/arte/g2/folha1.md`, `docs/arte/PROVENIENCIA.md` §6–§7 |
| T014 regressão | **em curso.** Matriz R1–R18 ligada a testes; partida inteira simulada nas duas rotas. Aberto: os passos que pedem gente (R5 com o processo morto de verdade, R7 tentando farmar) | [`docs/qa/T014_REGRESSAO.md`](qa/T014_REGRESSAO.md) |

### Dívida e riscos que valem hoje

Dívida conferida linha a linha: [`docs/tech/DIVIDA_TECNICA.md`](tech/DIVIDA_TECNICA.md). As duas P0 de save do baseline foram pagas nos Blocos B e C (tabela acima). Riscos de produto no §5.

### Hardening de 2026-10-04 (Prompt Mestre), bloco a bloco

| Bloco | O que mudou | Verificação |
|---|---|---|
| A | baseline e documentação reconciliada | este §0 |
| B | save v2: política única de versão (cabeçalho do `SaveData.cs`), passo v1→v2, fixture congelada `Tests/EditMode/Fixtures/save_v1_completo.json`, testes de v1 mínimo/completo, formato congelado e recompensa depois da migração | EditMode 582/582, PlayMode 27/27; mutação no `SaveData` derruba `FormatoGravado_Congelado` |
| E | movimento e combate por idade: velocidade proporcional à altura (`Corpo`), espada de madeira no corpo (`CombatMoves.NoCorpo`), clip `Skill` opcional e montagem por pasta no pipeline do humanoide; mira suave já restrita a hostil + ataque | EditMode 594/594, PlayMode 30/30, build sem erro, os dois `-roteiro` OK com o treino fechando os quatro verbos |
| D | UI de jogador em uGUI (`Scripts/UI/Tela.cs`, Canvas por tela em pixel 1:1, geometria pura): entrada e nascimento, conversa, missão, indicador, salto, gancho, sair, menu (grade de 2 colunas), treino, prompt, dano e os controles de toque (`ToqueHud`; o leitor de input não desenha). `HudLayout` + `HudLayoutTests`: 16:9, 19.5:9, 20:9 e 360 dp × notch × mão. Só o `PerfHud` (diagnóstico) em IMGUI | EditMode 591/591, PlayMode 30/30, build Windows sem erro, `-roteiro` OK (173 s) e `quebrada` OK (123 s) no modo celular; fotos de cada tela conferidas |
| C | estado: treino e nascimento escrevem só pela sessão (`GameSession.Praticar`, `Nascer`; `SaveState.NovaVida`); `TrainingProgress` sem delegates estáticos; cerca do `SaveState` (`ArquiteturaTests`); revisão independente do Bloco B aplicada (fixture com `resumos`, migração campo a campo, v2 corrompido com `.bak` v1) | EditMode 589/589, PlayMode 27/27, build Windows sem erro, `-roteiro` OK (173 s) e `quebrada` OK (123 s); o save do robô sai em v2 com a prática gravada |

### Próximos passos (ordem do Prompt Mestre, 2026-10-04)

1. **Bloco F — verificação e build:** `client/tools/verify.ps1`, caminho de release AAB (sem publicar), CI do que não pede licença Unity.
2. **Blocos G–J:** manifest, guarda do Tripo Bridge, arte só pelos portões, regressão integral e handoff.

Bloqueado por ambiente: medição em aparelho (faixas, aquecimento, engasgo frio da primeira esquiva) e o caminho do voltar no Android 16 — `BLOCKED_HARDWARE`; assinatura de release — `BLOCKED_CREDENTIAL` até existir keystore local.

## 1. O que é

Action RPG narrativo em terceira pessoa, **mobile — Android primeiro, paisagem, toque** ([ADR-0006](adr/ADR-0006-plataforma-mobile.md)), single-player. Fantasia medieval estilizada com influência de anime. Cooperativo é possibilidade arquitetural futura (expedições da vida adulta), não compromisso de produção; MMORPG está explicitamente fora.

Não é isekai. A alma ainda não nasceu, encontra o Guardião Aethron no Limiar da Existência, escolhe um **Destino de Nascimento permanente** entre quatro (Vida Serena, Vida Normal, Vida Difícil, Vida da Ruptura) e depois uma entre **três origens familiares compatíveis** (agricultores, artesãos/comerciantes, guardiões regionais). Quatro destinos × três origens = até doze configurações iniciais, **não doze campanhas**. A vida começa jogável aos cinco anos, na vila de Auren, reino de Eldoria, continente Valtheris, mundo Eryndor.

**Pilares:** viver e crescer; escolher e transformar; superar a própria existência.

**Vertical slice ("A Primeira Existência"):** prólogo no Limiar → escolha de destino e origem → personalização → despertar aos cinco anos em Auren → atividades e vínculos → decisão com consequência → desaparecimento perto do bosque → símbolo do Limiar → salto temporal confirmado para ~8 anos → treino supervisionado de movimento, ataque, defesa e primeira magia → gancho narrativo. Uma vila e um bosque, dez NPCs relevantes, oito missões planejadas (cinco centrais, três opcionais).

O nome "Chronicles of Existence" é **provisório**. Disponibilidade de marca e domínio não foi pesquisada.

## 2. Estado das decisões

Classificação do prompt-mestre: **APROVADO / PROPOSTA / HIPÓTESE A VALIDAR / FUTURO / DESCARTADO**.

**APROVADO** (decidido pelo idealizador, registrado no dossiê)
- Action RPG narrativo, terceira pessoa, single-player primeiro (seção B). **Plataforma: mobile, Android primeiro** ([ADR-0006](adr/ADR-0006-plataforma-mobile.md), 2026-09-29), no lugar do PC do dossiê.
- [ADR-0002](adr/ADR-0002-riscos-de-originalidade.md): teste positivo de originalidade como portão obrigatório (G1 ficha → G2 concept → G3 licença) antes de qualquer malha. Linha no `CLAUDE.md`.
- Quatro Destinos de Nascimento; destino **permanente**, sem troca (seção C).
- Três origens familiares compatíveis por destino, de arquétipos modulares (seção C).
- Separação em três sistemas distintos: destino permanente, Grau de Existência evolutivo, assistências de jogabilidade (seção C).
- Vida jogável começa aos cinco anos; envelhecimento por marcos narrativos anunciados e confirmados (seção D).
- Seis atributos: Força, Agilidade, Vigor, Intelecto, Percepção, Vontade (seção D).
- Nível de Vida persiste; Nível de Grau reinicia na ascensão (seção D).
- Quatro caminhos de ascensão: Divina, Arcana, Superação, Ruptura; ascensão pela Ruptura não é exclusiva da Vida da Ruptura (seção E).
- Reputação contextual por NPC e comunidade, não medidor binário universal (seção G).
- Vertical slice limitado a Auren e bosque (seção L).

**PROPOSTA** (escrito, aguardando decisão)
- Consequência da Q-04 na reputação: promessa cumprida leva a confiança de Sera e Nilo a +20; quebrada, a −20 (`ReputationSystem.Consequencias`, T010).
- Cinco dimensões de reputação (Honra, Compaixão, Renome, Temor, Confiança) — o dossiê manda introduzir gradualmente, não construir os cinco (seção G).
- Cinco graus de existência (I Comum a V Primordial) como escala narrativa, sem promessa de implementação (seção E).
- Seleção de cinco missões centrais e três opcionais entre as oito concebidas (seção H).
- Indicador de objetivo (2026-10-02): "▼" sobre quem procurar e seta na borda quando fora da tela, só para a história principal; opcional se descobre conversando (`IndicadorDeObjetivo`, `RumoDaMissao`).
- Luz por período (2026-10-02): manhã fresca, a tarde aprovada do protótipo e noite de luar escura mas legível (`LuzDoDia`).
- Botões de missão na voz da criança ("O Daren mandou um recado pra você.") em vez do texto do objetivo (`dialogo.fala.*`).

**HIPÓTESE A VALIDAR** (só se resolve jogando ou medindo)
- Duração de 45–75 minutos do slice.
- Que a infância seja divertida de jogar.
- Que as escolhas sejam perceptíveis para o jogador.
- Que o salto temporal seja emocionalmente significativo.
- Que doze configurações de destino × origem produzam variação percebida e não doze reskins.
- Que a versão 6000.3.23f1 seja a versão certa para fixar (o dossiê fala em Unity 6 LTS e menciona 6.3 LTS).
- Paisagem, layout de toque (botões ≥ 48 dp no canto do polegar), 30 FPS em Android de faixa média, orçamentos da `PIPELINE.md` §4 e API mínima 26: tudo a validar no primeiro aparelho.
- Escala do corpo: 1,10 m aos 5 anos e 1,28 m aos 8 (`BodyScale`, mediana OMS).

**FUTURO** (fora do escopo agora, arquitetura preservada)
- Cooperativo, como expedições da vida adulta.
- Ascensão completa e seu balanceamento.
- Regiões além de Auren: Karvorn, Sylvara, Ashkar, Nharos.
- IA generativa para fala de NPC, sempre com fallback determinístico e sem autoridade sobre estado.

**DESCARTADO**
- Troca de dificuldade após o nascimento — identificado como exploit: começar no fácil, acumular poder e mudar para colher a recompensa da extrema (seção C).
- Herança genética complexa, múltiplas raças, nascimento por todo o continente e descendência na primeira versão (seção C).
- MMORPG.

## 3. Dívida técnica da camada genérica

O que ainda precisa mudar ou sair do código atual, arquivo por arquivo e com a tarefa do backlog correspondente, está em [docs/tech/DIVIDA_TECNICA.md](tech/DIVIDA_TECNICA.md).

## 4. Armadilhas conhecidas do ambiente

**Unity**
- Instalado em `C:\Program Files\Unity\Hub\Editor\6000.3.23f1`. A versão está fixada em `client/ProjectSettings/ProjectVersion.txt`; abrir com outra versão migra o projeto sem perguntar de forma reversível.
- **Um projeto Unity = uma instância por vez.** Se o Editor estiver aberto com `client/`, um build em batch mode falha por lock — e vice-versa. Fechar o Editor antes de rodar `build_windows.ps1`.
- Batch mode: `-batchmode -nographics -projectPath ... -executeMethod ... -logFile ... -quit`. O código de saída não conta a história toda: ler o log, que sai em `client/Builds/build_win.log`.
- `Active Input Handling` não tem API pública em `PlayerSettings`. Os scripts escrevem direto no `ProjectSettings.asset`, e **o valor só vale na próxima abertura do editor**.
- Um script de Editor que gera cena por código é versionável em diff; uma cena `.unity` editada à mão não é. O COE usa o primeiro padrão.
- **Caminho longo em worktree.** Dentro de `.claude\worktrees\<nome>\client`, os shadergraphs de exemplo do URP no `Library\PackageCache` passam de 260 caracteres. O import deles falha (`DirectoryNotFoundException`), e o URP conta 6 erros no `BuildSummary`, mesmo com `LongPathsEnabled=1` no Windows. A build sai assim mesmo. No checkout principal o caminho é 49 caracteres mais curto e o erro não aparece. **Atalho (2026-10-04):** mapear o worktree numa letra com `subst W: "<caminho do worktree>"` e usar `-projectPath W:\client`; o baseline rodou assim com 0 erro de import (`subst W: /d` desfaz). Worktree novo também precisa de `git lfs pull --include="client/**"` antes do primeiro import.
- A build é Development, e a primeira execução de cada `COE.exe` novo abre o alerta do Firewall do Windows (porta do profiler). Pode cancelar: o jogo não usa rede.
- **Android:** módulo, SDK (API 34–36), NDK e OpenJDK instalados junto do Unity; `adb` em `%LOCALAPPDATA%\Android\Sdk\platform-tools`. A primeira build Android reimporta tudo para a plataforma, e alternar entre Android e Windows reimporta de novo. `run_android.ps1` precisa de aparelho com depuração USB (sai com 2 sem aparelho).
- **Simular o celular no PC:** `client/tools/run_windows.ps1 -Celular -Scene Auren -KeepOpen` abre uma janela 20:9 (metade do POCO F4) com o toque feito pelo mouse, no tamanho relativo do aparelho. Um gamepad pareado no PC (um "Wireless Controller" Bluetooth estava pareado em 2026-09-29) também move o personagem: se ele andar sozinho, é o gamepad.
- Mudou um FBX em `Art/Humanoid/`? Rode `COE / Montar humanoide` (`HumanoidSetup.Run`) e depois `COE / Validar arte`. O avatar é mapeado por nome quando o esqueleto usa os nomes humanos do Unity.
- Depois de uma build, as cenas são regeradas iguais byte a byte (`CenaEstavel`) e os YAML do Unity estão em LF (`.gitattributes`): `git status` limpo é o esperado (conferido em 2026-10-04). Diff em `.unity`, `.meta` ou `ProjectSettings/` depois de um build é mudança real e precisa de explicação.

**Blender**
- Instalado em `C:\Program Files\Blender Foundation\Blender 5.2`. Roda headless: `blender.exe --background --python <script> -- <args>`.
- O gerador de placeholder (`client/tools/placeholder_humanoid.py`) já usa esse modo e produz exatamente o que `HumanoidSetup.cs` espera.

**Repositório**
- Remoto no GitHub: `chronicles-of-existence` (`origin`, branch `main`).
- O trabalho acontece em git worktrees sob `.claude/worktrees/`. A pilha de `git stash` é compartilhada entre todos eles: nunca usar `git stash` sem nome.
- Binário de arte e áudio vai para o Git LFS pelo `.gitattributes` da raiz. Os FBX entraram no LFS na T001 sem reescrever o histórico: o commit inicial ainda guarda os binários direto no git.

**Windows**
- Caminhos com espaço e acento (`MEUS PROJETOS`) quebram script que não cita o caminho. Citar sempre.
- PowerShell 5.1: sem `&&`, sem `??`, sem ternário. Encadear com `;` e `if ($?)`.
- Arquivos do repositório são UTF-8 sem BOM, com CRLF.
- `client/Library/`, `Temp/`, `Logs/`, `UserSettings/` e `Assets/StreamingAssets/content/` estão no `.gitignore`.

## 5. Riscos em aberto (2026-10-04)

| Risco | Gravidade | Estado |
|---|---|---|
| Build mais velha apagar dado de save que não conhece (mesma `saveVersion`) | média | mitigado (Bloco B): save v2 e política única; build v1 recusa gravar por cima de v2. Resta disciplina: o `FormatoGravado_Congelado` avisa quando o formato muda |
| Desempenho em celular de faixa média | alta | só o POCO F4 (acima do alvo), sem CSV; faixas do ADR-0009 nunca medidas em aparelho; tooling pronto (`PerfHud`, `perf_report.py`). `BLOCKED_HARDWARE` |
| UI de protótipo na mão de quem joga | média | Bloco D: telas em uGUI, na área segura, alvos >= 48 dp, layout testado em 4 proporções; o visual ainda é o do protótipo (sem fonte própria nem arte de UI, T013) |
| Escolhas imperceptíveis / infância sem graça (hipóteses do §2) | alta | só se resolve jogando; protocolo pronto, nenhuma sessão |
| Erro de originalidade por subtração (personagem genérico) | alta | portões do ADR-0002 em uso: G1 13/13, G2 4 aprovados e 1 devolvido |
| Licença e proveniência de asset gerado por IA | alta | registrada desde o primeiro gerado (`PROVENIENCIA.md` §6–§8); protótipos marcados PROTOTIPO não vão para a loja |
| Combinatória 12 configurações × 8 missões | alta | `SliceInteiroTests` cobre 4 destinos × 2 desfechos; variação percebida não medida |
| Exploits de progressão | média | obrigatórios 1–8 verdes, R1–R18 mapeados; faltam os passos com gente |
| Trabalho de arte não commitado num worktree só | média | G3 lote 2 vive só no disco (`coe-disk-cleanup-b9dd70`); perda do disco perde o lote |
| GDD e dossiê ainda dizem "PC" | média | ADR-0006 prevalece; atualizar na próxima revisão de produto |
| Prazos da loja (target API 36 até 01/11/2026, 16 KB até 01/02/2027) | média | caminho de release inexistente; Bloco F |
| Nome comercial não pesquisado | média | aberto |
| Versão do Unity a fixar em definitivo | baixa | 6000.3.23f1 |

## 6. CONTINUAR DAQUI

O que fazer agora está no §0 ("Próximos passos"). Aqui ficam o que depende do idealizador e os prazos externos.

### Decisões do idealizador

A recomendação vem depois da seta. Estado em 2026-10-04.

1. ~~Como o dia passa~~ → **decidido (ADR-0007 §1):** avança ao concluir missão e ao descansar.
2. ~~Rótulo "Vida Difícil"~~ → **decidido (ADR-0007):** "Vida Árdua"; o id `dificil` fica.
3. ~~Ausência de Nilo e a q03~~ → **decidido (ADR-0007 §3):** Nilo some ao concluir a Q-04 e a q03 encerra.
4. ~~Regra da Q-04 na reputação~~ → **decidido (ADR-0007):** ±20 para Sera e Nilo.
5. ~~Limiar e aparência no slice~~ → **decidido (ADR-0007):** Limiar mínimo (feito, B01); aparência adiada.
6. ~~"Acordar" (q01)~~ → **decidido (ADR-0007):** automático.
7. ~~Aparelho mínimo~~ → **decidido (ADR-0009):** do celular simples ao avançado, três faixas com detecção automática. Falta medir num aparelho da classe Android 8 / 2–3 GB / GLES 3.0.
8. ~~Acervo de concept e plano do Tripo~~ → **decidido:** plano Max; acervo no LFS em `arte/referencias/acervo/` (ADR-0010). Os originais na raiz do checkout principal ficam até o idealizador apagar.
9. ~~Estilo~~ → **decidido (ADR-0008):** anime estilizado, toon no URP.
10. ~~Tripo Bridge~~ → **decidido (ADR-0010):** fica, só no Editor; DLL no git comum.
11. ~~ADR-0004 e save editado~~ → **decidido (ADR-0007 §7):** detecta id inválido e registra, sem prometer anti-cheat local (`LocalSave.Auditar`, teste R9); ADR-0004 já reescrito.
12. **Público-alvo e conta do Play** → direção decidida (ADR-0009): crianças e adultos. Pendente: política de Famílias do Play, LGPD art. 14 e ECA Digital antes de declarar o público; regra de 12 testadores por 14 dias.
13. ~~Gamepad~~ → **decidido (ADR-0007 §8):** conveniência; conversa não precisa ser navegável por gamepad no slice.
14. Padrões em uso que só pedem confirmação: id `br.com.vstack.coe` (não muda depois de publicado), API mínima 26, Unity 6000.3.23f1, leitor de textos atual, correr pela borda do joystick.

### Loja (datas conferidas em 2026-09-30; revalidar no envio)

- O manifesto gerado tem target 36, mínimo 26, ARM64, IL2CPP, categoria `game`. O target é "Auto": depende do SDK instalado na máquina.
- Falta o caminho de release: AAB, chave de upload, build não-Development, `versionCode`, ícone, declaração de público-alvo (Bloco F).
- Prazos: verificação de desenvolvedor no Brasil desde 30/09/2026 (o `adb` continua valendo; APK solto para testador pode não instalar); fim da extensão do target API 36 em 01/11/2026; páginas de 16 KB em 01/02/2027 (o gate `zipalign -c -P 16` nunca foi rodado).

---

## Handoff (modelo da seção P do dossiê)

**Versão / data:** 2026-10-04, baseline do Prompt Mestre de hardening.
**Última decisão aprovada:** [ADR-0010](adr/ADR-0010-arte-por-delegacao.md) e adendo (q05, q07).
**Sistemas existentes e testados:** EditMode 579/579, PlayMode 27/27, 0 erro de compilação, `-roteiro` e `-roteiro quebrada` OK, build Windows sem erro (§0).
**Propostas ainda abertas:** reputação em cinco dimensões; cinco graus de existência; seleção das oito missões; indicador de objetivo; luz por período; botões de missão na voz da criança (§2).
**Riscos e bloqueios:** §5. Sem aparelho Android ligado (`BLOCKED_HARDWARE`); sem keystore de release (`BLOCKED_CREDENTIAL`).
**Próxima tarefa recomendada:** Bloco B (save) do §0.
**Mudanças necessárias no GDD / backlog:** trocar "PC" por mobile no GDD e no dossiê §B (ADR-0006); registrar o portão do ADR-0002 no capítulo de arte do GDD.

---

## 7. HISTÓRICO

**Tudo abaixo é registro do que foi feito ou conferido na data indicada. Não é o estado atual (§0).** Contagens de teste, "falta" e "não existe" aqui valiam naquela data.

### HISTÓRICO — Onde estávamos em 2026-09-30 (antigo §2.1 e §2.2)

#### HISTÓRICO — 2.1 O que é fato verificado

| Fato | Evidência |
|---|---|
| Os quatro documentos-fonte estão versionados no repositório | `docs/direcao/`, `docs/backlog/`, `docs/gdd/` |
| O projeto Unity existe, com editor 6000.3.23f1 fixado | `client/ProjectSettings/ProjectVersion.txt` |
| A camada técnica está em `_COE` | `client/Assets/_COE/`, namespace `COE`: 76 `.cs` de runtime, 14 de editor, 57 de teste (contados em 2026-09-30) |
| Cenas Bootstrap e Auren, geradas por script, nessa ordem no Build Settings | `Editor/BootstrapSceneBuilder.cs`, `Editor/AurenSceneBuilder.cs`, `ProjectSettings/EditorBuildSettings.asset` |
| **T002–T011 aceitas** (2026-09-29, Unity 6000.3.23f1 em batch mode) | 0 erro e 0 aviso de compilação; EditMode **367 testes, 366 passam, 0 falham, 1 ignorado de propósito (rotina condicional de NPC, aguarda conteúdo da T012)**; PlayMode **20/20**; os 8 testes obrigatórios do backlog têm teste nomeado `Obrigatorio<n>_*` e passam (§6) |
| Pipeline de arte técnico pronto | `docs/arte/PIPELINE.md` (orçamento de celular), `docs/arte/PROVENIENCIA.md`, validador `COE / Validar arte` (25 de 27 regras), placeholder infantil de 1,10 m com avatar Humanoid de 19 ossos |
| **Rodou num Android real** (2026-09-29) | POCO F4 (Android 14, Adreno 650, 1080x2400): APK instalado, Auren aberta em paisagem com o toque, logcat da Unity sem erro; CSV do `PerfHud`: 30,3 FPS estável (travado na meta de 30), 33 ms por quadro, 95 MB alocados, sensor de 48–52 °C sem subir. É um aparelho acima do alvo de faixa média |
| Build Android por script | `client/tools/build_android.ps1` → `client/Builds/android/COE.apk` (BuildSummary Succeeded, APK de desenvolvimento com 41,2 MB, 2026-09-29) |
| **T001 concluída** (2026-09-29, Unity 6000.3.23f1 em batch mode) | 0 erro e 0 aviso de compilação; EditMode **275/275** (264 em `COE.Tests`, 11 em `COE.EditorTests`); PlayMode **15/15**; `build_windows.ps1` → `BuildSummary result=Succeeded`, `COE.exe` com 155,8 MB; `run_windows.ps1` abre a janela na Bootstrap e o `Player.log` sai sem erro |

#### HISTÓRICO — 2.2 O que NÃO é fato

- **Ainda não é o slice jogável.** A fiação existe (T012: nascimento → Auren com NPCs, missões, inventário e salto), mas faltam textos, falas, a passagem do dia e a arte (§6).
- **Só um aparelho, e acima do alvo.** O POCO F4 segura 30 FPS no graybox; aparelho de faixa média, arte real e sessão longa (aquecimento) ainda não foram medidos.
- **Nenhuma arte do COE foi produzida.** O humanoide em `Art/Humanoid/` é placeholder gerado por script (`client/tools/placeholder_humanoid.py`). As ~200 referências de concept vão para `arte/referencias/`.
- **Nada foi jogado.** A hipótese de 45–75 minutos do slice não foi medida e não pode ser, porque não há slice.

#### HISTÓRICO — Estado das tarefas (2026-09-29)

"Aceita" = regra implementada e testes verdes no Unity. Não quer dizer jogável: a fiação é a T012.

| Tarefa | Estado | Evidência |
|---|---|---|
| T001 baseline | concluída | tag `t001-baseline` |
| T002 controller e câmera | aceita | escala de criança (`BodyScale`), toque de volta (ADR-0006), mira só em hostil, sem `Find` global; `BootstrapSceneTests`, `TouchControlsTests` |
| T003 destino e origem | aceita | `Obrigatorio1_*`, `Obrigatorio2_*`; confirmação por `destinyId` (carimbo zerado no save não reabre) |
| T004 save v1 | aceita | `sceneId`/`anchorId`, migração v0→v1 com cópia, versão futura intacta (principal e `.bak`), `Obrigatorio4_*` |
| T005 histórico de vida | aceita | `T005_*`; idempotente entre instâncias |
| T006 missões | aceita | `Obrigatorio3_*`, `Obrigatorio6_*`; desfecho único da Q-04; `validate_quests.py` 11/11 |
| T007 NPC e diálogo | aceita | rotina interrompível e por memória (`NpcAgendaTests`); `Obrigatorio8_*` |
| T008 Auren graybox | aceita | percursos varridos com a cápsula da criança; `horta_familia` |
| T009 life system | aceita | `Obrigatorio5_*`, `Obrigatorio7_*`; o salto exige a Q-08 concluída; spawner de âncora |
| T010 reputação | aceita | `T010_*`; ids com `.` aceitos; Sera e Nilo pela Q-04 (proposta) |
| T011 treino de combate | aceita | `T011_*`; magia em três fases; o parceiro não mata a criança |
| T012 integração | fiação feita | sessão, entrada/nascimento, NPC e diálogo, missões no mundo, inventário, idade e salto (abaixo); falta conteúdo |
| T013 arte | G2 da primeira folha feito (2026-10-03): **Borin, Maelis, Tovin e Daren aprovados**; Mara reprovada na descrição (ficha volta ao autor) | concepts em `arte/referencias/concepts_g2/`, laudo em `docs/arte/g2/folha1.md`; próximo: G3 (malha no Tripo a partir dos 4 concepts, com as condições do laudo) e as folhas 2 e 3 |
| T014 regressão | em curso (2026-10-01) | matriz R1–R18 em [`docs/qa/T014_REGRESSAO.md`](qa/T014_REGRESSAO.md): cada passo ligado ao teste automático que o cobre; `SliceInteiroTests` joga o slice inteiro (nascimento → gancho) em 4 destinos × 2 desfechos e trava o R9. Painel do treino (R7) feito em `TreinoHud`. Aberto: a rodada de tela numa build |

#### HISTÓRICO — T012 — fiação em runtime: feita (2026-09-29)

O que liga os sistemas em jogo (contrato em `Scripts/Core/GameSession.cs`, acessado por `SaveState.Sessao`):
- **Sessão sem God Manager:** abre histórico, missões e reputação sobre o save; toda transição Ok sincroniza memória de NPC, reputação e inventário e grava UMA vez; recusa não grava. O save é lido do disco uma vez por processo (trocar de cena não relê).
- **Entrada (ícone → jogo):** a Bootstrap decide: sem `birth.destinyId` → tela de nascimento (destino, origem, nome, "tem certeza?"; moedas e itens iniciais entram no inventário na mesma gravação); com destino → cena salva (Auren). `-scene` pula a entrada (desenvolvimento). Save de versão mais nova → aviso.
- **Auren:** 10 NPCs na âncora da rotina do período, conversa de toque que pausa a agenda e trava o jogador, objetivos com NPC cumpridos conversando (o diálogo pede, a missão decide), escolha da promessa da Q-04; q01 e q08 começam sozinhas; 6 gatilhos de objetivo nas âncoras; HUD de missão e moedas; inventário mínimo no save.
- **Idade:** corpo, câmera e golpe por idade (`Corpo.DaIdade`); o salto é oferecido no símbolo do Limiar da clareira (§4.1), com o aviso "o que se encerra", encerra as opcionais e recarrega Auren aos 8 anos em `spawn_player`.
- **Textos:** `client/Assets/_COE/Resources/strings.pt-BR.json` (lido por `Resources.Load`, funciona no APK) com rótulos de sistema e nomes já aprovados nos docs (destinos, origens, NPCs, títulos de missão, fases); `StringsCoberturaTests` quebra se uma tela usar chave de sistema ausente. Descrições, objetivos e falas continuam `[chave]` (conteúdo).
- **Configurações** (menu de pausa, botão "Menu" no topo): mão destra/canhota (espelha o toque), sensibilidade da câmera 0,5–2,0, 30 ou 60 FPS, HUD de desempenho; em `PlayerPrefs` (`coe.cfg.v1.*`), fora do save de progresso.
- **Onde o jogador está:** gatilho de objetivo e conversa gravam a âncora (`GameSession.Posicao`) na mesma gravação da transição.
- **Verificado:** EditMode 437 (436 ok, 1 ignorado), PlayMode 24/24; no PC em modo celular: nascimento → Auren com a q01 iniciada e 40 moedas, textos reais no HUD, joystick de toque move a criança, menu abre, pausa, troca para canhoto e volta.

#### HISTÓRICO — Feito em 2026-09-30, depois da leitura (commits `3d0730d`, `67fa031`, `aefc358`)

- **Leva A da T012 ([ADR-0007](adr/ADR-0007-decisoes-da-leva-a.md)).** Decisões 1 a 6 aprovadas por delegação: o dia passa ao concluir missão e ao descansar; "Vida Árdua"; Nilo some ao concluir a Q-04 e a Q-03 encerra; Q-04 ±20; Limiar mínimo (ainda sem código); "acordar" automático. Textos de 74 para 262 chaves e fala escrita para os 10 NPCs.
- **Protótipo de estética ([ADR-0008](adr/ADR-0008-prototipo-de-estetica.md)).** Anime toon; 5 personagens e 12 peças gerados no Tripo3D e 6 animações do Mixamo, todos marcados PROTOTIPO em `docs/arte/PROVENIENCIA.md` §6. Não vão para a loja sem o portão do ADR-0002.
- **Verificado:** EditMode 483/483, PlayMode 26/26; build de Windows sem erro; Auren no modo celular a 30 FPS com a protagonista animada.
- **Jogar no PC:** `client/tools/build_windows.ps1` e depois `client/tools/run_windows.ps1 -Celular -Scene Auren -KeepOpen`.
- **Depois (commits `4f95124`, `0d3e793`, `2599682` e o dos NPCs, 2026-10-01):** locomoção sem deslizar (blend de três pontos, passada medida, velocidades de criança 1,6/3,8 m/s); esquiva, reação a golpe e queda do Mixamo; spawn de frente para a casa; GUID estável do `Prototipo.controller`; Daren, Lysa, Tovin, Eira, Oren e Maelis gerados no Tripo com rig Mixamo: nenhum NPC de Auren é mais cápsula. Depois: estrada de terra neutra (saía laranja, colada no Dourado do Limiar) e esquiva vista no jogo, com o corpo agora sobre o Player (o recuo do Mixamo estava assado na pose e levava o corpo ~1 m para longe).
- **Pendências visuais:**
  - reação a golpe e queda só verificadas em teste (`ClipsNoLugarTests`): Auren aos 5 anos não tem quem bata na criança;
  - primeira abertura do `COE.exe` logo após um build trava um quadro de ~300 ms na primeira esquiva (3 de 3 vezes; 0 de 3 nas aberturas seguintes): cache frio no PC. Medir no celular; se repetir, aquecer shaders/clips no load.
  - o visual não foi medido no aparelho.

#### HISTÓRICO — Feito em 2026-10-01 (PRs #1 e #2 na `main`, mais o Limiar)

- **Na `main`:** a leva A, a estética e tudo acima entraram pelo PR #1 (`97d8fa6`); o PR #2 (`5d49279`) pôs os YAML do Unity em LF no checkout.
- **Git sem diff falso:** `Prototipo.controller` estável ao remontar; ProjectSettings com os valores que o URP impõe; cenas geradas iguais byte a byte (`CenaEstavel`); `.gitattributes` com `eol=lf` para os YAML do Unity. Diff em `.unity`, `.meta` ou ProjectSettings agora é mudança real.
- **V03:** aceita o JPEG do Tripo só em `Art/Prototipo/`.
- **B01 — Limiar mínimo (leva B):** seis falas de Aethron antes da escolha de destino, com o símbolo da Trama em cena e "Voltar" para reler (SLICE B01). Testes: textos escritos sem "escolhido" nem dificuldade; palco desligado, câmera por cima e símbolo enquadrado acima do painel.
- **Verificado:** EditMode 490/490, PlayMode 27/27; no PC, sem save: Limiar → reler → destinos.
- **B16 — o gancho (leva B):** a tela final abre quando o treino do B15 termina e grava `marco.fim_da_primeira_existencia` uma vez; o texto deixa as perguntas abertas, sem nomear a Primeira Fratura e sem prometer continuação (teste de texto cobra os dois).
- **Base de medição (leva B):** `client/tools/perf_report.py` (só biblioteca padrão, `--autoteste`) resume o CSV do `PerfHud` com FPS, engasgos por quadro, memória, temperatura e bateria; `docs/medicoes/` guarda CSV e relatório lado a lado. Primeira medição arquivada: PC em modo celular, Auren, 57 s, 30 FPS sem engasgo, 98 MB estáveis — **não é o aparelho-alvo**.
- **`CenaEstavel` por nome:** a chave do objeto é o nome e a ocorrência entre irmãos, não a posição. Objeto novo no gerador muda só os ids dele (a tela do gancho acrescentou 49 linhas a Auren e não mexeu em nenhuma).
- **Verificado (leva B fechada):** EditMode 496/496, PlayMode 27/27; no PC: tela do B16 com save de teste aos 8 anos, save do PC restaurado com o mesmo md5.

#### HISTÓRICO — Feito em 2026-10-01/02 (depois do PR #6)

- **Simulação da partida inteira no PC:** `COE.exe -roteiro` joga do Limiar ao gancho com gamepad virtual e fotografa cada etapa; `-roteiro quebrada` faz a rota da promessa quebrada sem opcionais. As duas: ROTEIRO OK (~1,5 min). Detalhes em `docs/qa/T014_REGRESSAO.md`.
- **Achados da simulação, corrigidos:** missão depois de "Encerrar conversa"; fala longa do Tovin cortada; "Noite" com sol a pino (agora `LuzDoDia`); sombra mais clara que o chão à noite (`COE_Toon`); Borin mandando voltar de dia enquanto oferecia a missão; painel de treino dizendo "prática 12/30" para pontos de aprendizado.
- **Parceiro de treino:** pronto para o modelo do Tripo (`Prototipos.AnexarParceiro`); o modelo foi gerado e falta o rig humanoide (créditos da conta, clique do idealizador) e a exportação FBX.
- **Testes de editor não gravam mais no `save.json` do PC** (`SaveState.Commit` só no Play).
- **Verificado:** EditMode 517/517, PlayMode 27/27, simulação OK.

#### HISTÓRICO — Feito em 2026-10-03 (leva C1, orquestrador: 4 raias + Art Director separado)

- **Integração:** as 13 fichas G1 e os `PROMPTS_G2` (estavam só no branch `coe-arte-borin`) e o WIP de UI menor do dia 2 (cartões do nascimento, botão Menu, cartão de missão) entraram e foram verificados na simulação.
- **Diálogo por nascimento:** condições `Destino`, `Origem` e `TemItem`; os 10 NPCs abrem a conversa de um jeito diferente em cada destino (e vários pela origem ou pelo item), tirado do C8 das fichas. A fala de destino nunca esconde missão nem memória (`DialogueNascimentoTests`).
- **Voltar do Android / Esc:** fecha uma tela por vez e, na raiz, pede confirmação para sair (`VoltarHud`). HUD de desempenho não cobre mais o USAR no canhoto.
- **A vila reage (`PecaPorEvento`):** peças ligadas por evento do histórico. Do sumiço do Nilo até o salto: a forquilha dele na clareira e as tiras no vão; a folha da Maelis no mural continua depois do salto; aos 8, a tira vai para a prateleira de casa e a bancada do Borin vai para a porta da ferraria (B14, ficha C9).
- **Achado da simulação, corrigido:** de noite, com o `ajudar_borin` pendente, o Borin só dizia "boa noite" e o jogador ficava sem saída (o `-roteiro` travava aqui). Agora ele manda voltar de manhã; o robô descansa quando um passo não anda.
- **Fichas:** pendências da conferência G1 corrigidas por uma raia e reconferidas por um Art Director que não as escreveu (ADR-0010 §6): 13 de 13 aprovadas no G1, sem pendência. G2 em imagem quadrada; primeira folha recomendada: Borin, Maelis, Tovin, Mara, Daren (`fichas/README.md`).
- **Leva C2 (q05 e q07, decididas no adendo do ADR-0010):** na q07, a criança assina o que contou à Maelis com um círculo ou com um risco (dois desfechos, exatamente um; aos 8 a Maelis lembra qual). Na q05, depois de buscar ajuda, o chapéu da Lysa emborcado marca o bicho; parar ~3 s ao lado dele acalma o bicho, e só então dá para tratá-lo. Save antigo com a q07 adiantada conclui sem desfecho. Verificado: EditMode 567/567, PlayMode 27/27, `-roteiro` OK (173 s, "parado ao lado do chapeu, o bicho acalmou") e `-roteiro quebrada` OK (123 s, assinou com um risco).
- **Leva C3 (playtest com gente):** diário de sessão local (`Scripts/Core/DiarioDeSessao.cs`): cada partida grava em `persistentDataPath/diario/sessao_<data>.txt` (10 mais novos) uma linha por objetivo, missão, evento, período, idade, pausa e fim, com o tempo desde o início; sem nome, sem aparelho, sem rede. A simulação grava em `roteiro_diario/`. Protocolo do primeiro playtest em [`docs/qa/PLAYTEST.md`](qa/PLAYTEST.md): hipóteses → evidência, roteiro de observação B01–B16, crianças (termo do responsável, sem vídeo, sem nome), 8 perguntas, como ler o diário. Verificado: EditMode 579/579, PlayMode 27/27, `-roteiro` OK com o diário completo (2:51 no robô).
- **Verificado:** EditMode 549/549, PlayMode 27/27; build de Windows sem erro novo; `-roteiro` OK do Limiar ao gancho (164 s, visita aos 8 dos 10 NPCs) e `-roteiro quebrada` OK (124 s).

#### HISTÓRICO — Estado conferido em 2026-09-30 (leitura completa, sem alteração de código)

EditMode 437 (436 ok, 1 ignorado), PlayMode 24/24, 0 erro e 0 aviso de compilação, rodados em batch no checkout principal. 76 `.cs` de runtime, 14 de editor, 57 de teste. Os números de aparelho (30 FPS no POCO F4) são **declarados**: não há CSV nem logcat arquivado; `client/Builds/` só existe na worktree em que o build rodou.

#### HISTÓRICO — T012 — o que falta para o slice correr do ícone ao gancho

Conteúdo (criação): 23 objetivos de missão (`"[a escrever]"`), 18 textos de destino e origem, 10 papéis de NPC, falas dos 10 NPCs (**0 escritas**: Borin, Lysa e Nilo têm só grafo de exemplo; os outros 7 caem em `dialogo.sem_fala`), 13 opções de diálogo, 2 erros de nome (`nascimento.erro.nome_curto`, `nome_longo`). O `StringsCoberturaTests` só guarda chaves de sistema: a falta dessas passa sem alarme.

Sistema (sem código hoje):
1. O dia não passa: `TimeOfDayCycle.Avancar` só é chamado em testes. Depende da decisão 1.
2. Evento de início do desaparecimento de Nilo e a rotina condicional dele (Nilo segue na praça durante a q07). A q03 exige Nilo na trilha e precisa de regra (decisão 3).
3. Limiar com Aethron (B01), gancho final (B16) e Auren depois do salto (B14). Aparência na personalização (B04) também não existe.

Técnico (conferido em 2026-10-01):
1. NPCs teleportados entre vagas (sem NavMesh). Aberto. (Colisor: pago, corpo sólido desde 2026-10-01.) (O NPC colado na câmera do spawn foi resolvido em `2599682`.)
2. Conversa não navegável por gamepad: fora do slice (ADR-0007, decisão 8).
3. ~~No modo canhoto, o texto do HUD de desempenho passa por cima do botão USAR~~: confirmado pelas coordenadas e corrigido (2026-10-03, `PerfHud.XDoTexto`, teste `PerfHudCanhotoTests`).
4. Rótulos dos botões de toque: já vêm de `toque.*` no arquivo de textos. Pago.
5. ~~O "voltar" do Android não é tratado~~: feito (2026-10-03, `VoltarHud`); falta conferir num Android 16 qual caminho de input dispara.
6. `PerfHud` grava CSV só em build de debug e `client/tools/perf_report.py` existe (2026-10-01). Pago.
7. Todos os módulos têm README. Pago.

#### HISTÓRICO — Arte (T013): portão fechado

- **Pronto:** `docs/arte/PIPELINE.md` (orçamento de celular, 27 regras, 25 ativas), `docs/arte/PROVENIENCIA.md`, `COE / Validar arte`, placeholder infantil. Teste cego de silhueta do G2: `python client/tools/silhueta.py --saida <pasta> rotulo=imagem@altura_m ...` gera a folha embaralhada e o gabarito separado (`--teste` confere a própria máscara).
- **Portão do ADR-0002:** modelo de ficha em `docs/arte/fichas/_MODELO_G1.md`; ficha-piloto do Borin (`docs/arte/fichas/borin.md`) com parecer do Art Director, aguardando a nota do idealizador; os outros nove NPCs e o avatar sem ficha; 0 concepts em G2; 0 licenças de arte em G3.
- **Acervo de concept:** 235 PNGs gerados no ChatGPT ("GPT astra 6", informado pelo idealizador em 2026-09-30) em 28–29/09/2026 (manifesto C2PA); o mapa `docs/arte/ACERVO.csv` diz quem é quem, com o SHA-256 de cada arquivo, mais `catalogo_341_fichas.json` (fichas de **prompt**, não G1). Estão na raiz do checkout principal, **não rastreados** (458 MB; o zip de 481 MB era cópia idêntica e foi apagado em 2026-10-03), sem registro de ferramenta, plano ou termos, e só existem neste disco. São referência de direção, não entrada do Tripo. 199 imagens não têm id de ficha e 1 PNG está truncado.
- **Tripo:** o plano gratuito é **uso não comercial** (termos consultados em 2026-09-29). Em 2026-09-30 o idealizador assinou o plano **Max** (25 200 créditos): cada geração registra na PROVENIENCIA `plano: max` e o nº da fatura, com o modelo marcado privado antes de gerar. O Tripo Bridge (`client/Packages/com.tripo3d.unitybridge`, commit `3787ea1`) é só Editor, não entra no player, não tem LICENSE no pacote e traz `websocket-sharp.dll` fora do LFS.
- **Pendente técnico:** `HumanoidSetup` monta só `Art/Humanoid/` (precisa aceitar pasta por parâmetro), clip `Skill` da magia, footprints das estruturas para a regra V12.
- **Áudio e fonte:** não há nenhum arquivo de áudio nem de fonte no repositório (a UI é IMGUI); nenhuma licença decidida.

#### HISTÓRICO — Próxima leva recomendada

Leva A: feita (ADR-0007, PR #1). Leva B: feita (B01 Limiar, B16 gancho, base de medição; 2026-10-01). T014 iniciada (matriz e viagem inteira automatizada). Três faixas gráficas (ADR-0009) feitas e vistas no PC. **Próxima, leva C:** arte mínima pelo portão do ADR-0002, T014 (regressão do slice inteiro) e caminho de release; antes disso, a primeira medição num Android de faixa média (decisão 7) arquivada em `docs/medicoes/`. Decisões 1–6, 11 e 13 foram tomadas no ADR-0007; continuam com o idealizador as 7–10, 12 e 14.

#### HISTÓRICO — Handoff de 2026-09-30

**Versão / data:** 2026-09-30, leitura completa do estado (os artefatos abaixo são de 2026-09-29).
**Última decisão aprovada:** [ADR-0006](adr/ADR-0006-plataforma-mobile.md) — mobile, Android primeiro.
**Artefatos produzidos hoje:** T002–T011 aceitas; toque religado; build Android por script; `docs/arte/PIPELINE.md`, `docs/arte/PROVENIENCIA.md` e validador de arte; placeholder infantil; ADR-0006; `CLAUDE.md`, README e `docs/tech/DIVIDA_TECNICA.md` atualizados; revisão cruzada com os achados altos corrigidos.
**Sistemas existentes e testados:** EditMode 437 testes, 436 passam, 0 falham, 1 ignorado de propósito (rotina condicional de NPC, aguarda conteúdo da T012), PlayMode 24/24; 0 erro de compilação (2026-09-30).
**Propostas ainda abertas:** regra da Q-04 na reputação; reputação em cinco dimensões; cinco graus de existência; seleção das oito missões.
**Riscos e bloqueios:** só um aparelho medido (POCO F4, acima do alvo) e sem CSV arquivado; arte bloqueada até a ficha G1; conteúdo da T012 em zero falas; acervo de concept sem backup nem proveniência.
**Próxima tarefa recomendada:** leva A do §6: decisões 1 a 6, conteúdo da T012, evento de Nilo, passagem do dia e a ficha G1 piloto.
**Mudanças necessárias no GDD / backlog:** trocar "PC" por mobile no GDD e no dossiê §B (ADR-0006); registrar o portão do ADR-0002 no capítulo de arte do GDD.
