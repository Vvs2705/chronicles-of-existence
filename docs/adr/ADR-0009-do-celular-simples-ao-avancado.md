# ADR-0009 — Do celular simples ao avançado, para crianças e adultos

- **ID:** ADR-0009
- **Data:** 2026-10-01
- **Estado:** direção APROVADA pelo idealizador (Vinicius, 2026-10-01: "quero que este jogo funcione em celulares simples aos avançados; ele é um RPG cartunizado, então a ideia é que funcione para crianças e adultos"). Números e faixas: **[PROPOSTA]** do coordenador, a medir no aparelho.
- **Responde:** decisão 7 (aparelho mínimo) e, em parte, decisão 12 (público-alvo) do `docs/PROJETO.md` §6.
- **Depende de:** ADR-0006 (mobile, Android primeiro), ADR-0008 (estética cartunizada)
- **Documentos afetados:** `docs/arte/PIPELINE.md` §4, `client/Assets/_COE/Editor/ProjectSetup.cs`, `Scripts/Perf/Qualidade.cs`, `Scripts/UI/MenuDePausa.cs`

## Contexto

O orçamento da `PIPELINE.md` §4 supunha um Android de **faixa média** (Adreno 610 / Mali-G57, 4 GB) e o único aparelho medido é um POCO F4, acima disso. O idealizador quer o mesmo jogo do celular **simples** ao **avançado**, e o público inclui **crianças**.

## Decisão

### 1. Um jogo, três faixas gráficas

O mesmo conteúdo e os mesmos assets em todo aparelho; muda só o custo de desenhar. Cada faixa é um nível do `QualitySettings` com o seu asset do URP (criados por `ProjectSetup.Apply`, tabela `ProjectSetup.Faixas`):

| Faixa | Render | Sombra | Pós (cor, vinheta) | Contorno toon | Texturas | Pele (ossos/vértice) | LOD bias |
|---|---|---|---|---|---|---|---|
| **Baixa** | 70% | nenhuma | desligado | desligado | metade | 2 | 0,7 |
| **Média** | 85% | 30 m, mapa 1024 | ligado | ligado | cheias | 4 | 1 |
| **Alta** | 100% | 50 m, mapa 2048 | ligado | ligado | cheias | 4 | 1,5 |

- **Escolha automática** na primeira abertura, pela RAM (`Qualidade.Detectar`): até ~3 GB = Baixa; até ~5 GB = Média; 6 GB ou mais = Alta. **[PROPOSTA]**
- O jogador troca em **Configurações → Qualidade** (Auto, Baixa, Média, Alta), gravado em `coe.cfg.v1.qualidade`, fora do save.
- **30 FPS é a meta em todas as faixas**; 60 FPS continua opção do jogador.
- Para medir: `COE.exe -qualidade baixa|media|alta` força a faixa só naquela sessão (`run_windows.ps1 -Qualidade`); o CSV do `PerfHud` tem a coluna `qualidade`.

### 2. Aparelho mínimo de referência **[PROPOSTA]**

Android 8.0 (API 26, o mínimo já configurado), 2–3 GB de RAM, GPU de entrada com OpenGL ES 3.0 (classe Mali-G52 / Adreno 506–610), tela 720p. Exemplos da classe, para comprar ou emprestar um: Galaxy A0x, Moto E/G de entrada. A meta só vale **medida nesse aparelho** (`docs/medicoes/`); o PC trava em 30 FPS em qualquer faixa e não prova nada.

### 3. Crianças e adultos: o que isto obriga a verificar (não decidido aqui)

Público que inclui crianças muda regras de loja e de lei. **Nada abaixo foi verificado nas fontes nesta data**; cada item precisa de consulta com data antes do envio à loja:

- **Google Play — política de Famílias:** app cujo público-alvo inclui menores de 13 anos tem de cumprir a política de Famílias (SDKs de anúncio certificados, limites de coleta de dados, conteúdo adequado). A recomendação anterior da decisão 12 ("declarar 13+") **não combina** com a direção nova e fica substituída por esta verificação.
- **LGPD, art. 14:** dado pessoal de criança só com consentimento específico de um dos pais ou responsável.
- **Lei brasileira de proteção de crianças em ambientes digitais (o "ECA Digital", de 2025):** confirmar número, vigência e obrigações para jogos (verificação de idade, monetização, publicidade).
- **Classificação indicativa** (IARC no Play / ClassInd no Brasil): o combate de treino com espada de madeira e a magia precisam caber na faixa que o idealizador quiser.

O que o jogo **já** faz e ajuda: não tem anúncio, chat, conta nem rede; o CSV de desempenho só existe em build de desenvolvimento; o save é local.

## Consequências

- **Feito (2026-10-01):** as três faixas, a detecção, a opção em Configurações, o `-qualidade` de medição e a coluna no CSV. Testes: `QualidadeTests` (detecção e persistência), `EsteticaTests.Faixas_*` (níveis, assets, volume de pós ligado ao menu). No PC, as três rodam em Auren a 30 FPS (o PC não diferencia).
- **ponytail — contorno:** na Baixa o casco do contorno sai da tela no vértice, mas a chamada de desenho continua. Se a CPU do aparelho simples pedir, o caminho é um renderer sem o passe `SRPDefaultUnlit` só na faixa Baixa.
- **Pendente:** medir as três faixas no aparelho de referência; LODs reais nos assets (o `lodBias` só age quando houver `LODGroup`); decidir a declaração de público na Play Store depois da verificação acima.
