*GAME DESIGN DOCUMENT*

# CHRONICLES OF EXISTENCE

*GDD Mestre • Consolidação de conceito e especificações de produção*

*Versão 1.1 | 28 de setembro de 2026*

Um RPG de fantasia em terceira pessoa sobre nascer, crescer, escolher e transformar a própria existência.

> DOCUMENTO DE TRABALHO — Não representa jogo implementado, orçamento validado nem avaliação jurídica. Nomes e especificações técnicas são provisórios até aprovação e testes.

Escopo-base: PC • single-player • expansão cooperativa futura • Unity / C# • Blender / Tripo3D / Claude Code.

## Como ler este documento

Este GDD reúne as decisões tomadas na concepção e diferencia regras de base, propostas preliminares e conteúdo planejado para versões futuras. A expressão “aprovado” significa alinhamento de design registrado na conversa, não implementação ou validação com jogadores.

| **Status** | **Significado** |
|---|---|
| BASE | Regra de produto a preservar no primeiro protótipo. |
| PROPOSTA | Parâmetro provisório a validar com prototipagem, playtest ou orçamento. |
| FUTURO | Intenção para expansão; não faz parte do primeiro vertical slice. |
| PENDENTE | Exige decisão adicional, pesquisa comercial ou definição técnica. |

### Mapa de capítulos

01 — Visão do produto e pilares

02 — Destino, nascimento e origem

03 — Vida, envelhecimento e progressão

04 — Ascensão da Existência

05 — Combate, magia e habilidades

06 — Reputação, relações e NPCs

07 — Missões, economia e exploits

08 — Mundo, mitologia e campanha

09 — Arte, áudio e pipeline 3D

10 — Arquitetura técnica e dados

11 — Vertical slice de Auren

12 — Backlog e testes de aceitação

13 — Riscos, pendências e próximo marco

## 01 — Visão do produto e pilares

Premissa: antes do nascimento, a alma encontra o Guardião do Limiar e escolhe uma entre quatro condições iniciais de existência. O personagem nasce em Eryndor — não vem de outro universo —, começa jogável aos cinco anos e evolui por relações, aprendizado, decisões e acontecimentos extraordinários.

| **Característica** | **Definição atual** |
|---|---|
| Gênero | Action RPG narrativo de fantasia medieval em terceira pessoa. |
| Lançamento-alvo | PC/Windows; formato single-player premium como hipótese comercial. |
| Motor | Unity 6 LTS (revisão exata fixada ao iniciar repositório), C#. |
| Direção visual | Fantasia estilizada própria, influência anime e cenários medievais detalhados. |
| Futuro online | Expedições cooperativas independentes da campanha pessoal, não MMORPG no lançamento. |
| Diferencial | Uma história de vida que altera oportunidades, vínculos, reputação e progressão. |

### Pilares

- Viver e crescer: infância com atividades significativas e efeitos posteriores.

- Escolher e transformar: decisões registradas e reconhecidas por pessoas, comunidades e mundo.

- Superar a própria existência: poder conquistado por aprendizado e diferentes caminhos de ascensão.

### Guardrails comerciais e criativos

Hell Mode e Fable permanecem referências de gênero e sensação de progressão/escolhas. O projeto deve possuir mitologia, personagens, mecânicas concretas, símbolos, arte, interface, narrativa e identidade próprias. Trocar apenas nomes não elimina risco de propriedade intelectual; revisar nomes e obra com profissional antes do lançamento ou negociação.

## 02 — Destino, nascimento e origem

### Fluxo do prólogo — BASE

Limiar da Existência → explicação do Guardião Aethron → escolha de Destino de Nascimento → apresentação de três origens compatíveis → personalização → início aos cinco anos em Auren. As 4 × 3 escolhas são 12 configurações modulares, não doze campanhas independentes.

| **Destino** | **Experiência e regra** |
|---|---|
| Vida Serena | Proteção, apoio, recursos e preparação mais acessíveis. |
| Vida Normal | Condições intermediárias e investigação por caminhos convencionais. |
| Vida Difícil | Restrições materiais e sociais; maior necessidade de preparação. |
| Vida da Ruptura | Acontecimentos anômalos e oportunidades específicas, sem poder automaticamente superior. |

> REGRA BASE: Destino de Nascimento é imutável. Ascensão e assistências de jogabilidade são sistemas separados.

### Arquétipos familiares — BASE

| **Origem** | **Campos de variação** |
|---|---|
| Agricultores | Trabalho rural, conhecimento natural; condição financeira muda com o destino. |
| Artesãos/comerciantes | Oficina, materiais, negociação; acesso a recursos varia. |
| Guardiões regionais | Disciplina, observação, defesa e tradição; prestígio também varia. |

A família utiliza dois papéis principais adaptáveis (Mara e Daren, nomes provisórios); profissão, recursos, diálogos e objetos da casa mudam conforme origem/destino. Não pressupor que um arquétipo seja rico, virtuoso ou melhor que os demais.

### Restrições de implementação

- Persistir IDs estáveis de destino e origem na criação do personagem.

- Após confirmar nascimento, bloquear mudanças retroativas de origem e destino.

- Não conceder vantagens numéricas irreversíveis que tornem uma origem escolha obrigatória.

- Testar as 12 combinações no sistema de criação e nas missões essenciais.

## 03 — Vida, envelhecimento e progressão

### Etapas narrativas

| **Idades** | **Fase** | **Foco** |
|---|---|---|
| 5–7 | Primeiras Descobertas | Família, vínculos, exploração, pequenas responsabilidades. |
| 8–11 | Despertar dos Talentos | Treino supervisionado, estudos e afinidades. |
| 12–15 | Formação | Técnicas e responsabilidades mais complexas. |
| 16–18 | Independência | Viagens e especializações iniciais. |
| 19+ | Construção do Legado | Exploração ampla, conflitos e ascensões. |

Tempo híbrido: relógio cotidiano sustenta rotinas; crescimento de idade depende de marco narrativo explicitamente confirmado. Antes do salto, o jogo mostra missões que expiram e preserva o estado das escolhas. Uma missão opcional não deve bloquear permanentemente envelhecimento.

### Seis atributos fundamentais — BASE

| **Atributo** | **Aplicação** |
|---|---|
| Força | Potência e requisitos de equipamentos. |
| Agilidade | Movimentação, precisão e coordenação. |
| Vigor | Resistência e recuperação. |
| Intelecto | Aprendizagem e técnicas complexas. |
| Percepção | Investigação, observação e perigos. |
| Vontade | Concentração e controle de habilidades. |

Afinidades: Marcial, Arcana, Natural, Artesanal, Social e Exploratória. Origem e eventos podem favorecer descoberta, mas não bloquear caminhos completos. A evolução exige tarefas progressivamente mais desafiadoras; atividades triviais têm rendimento limitado por domínio.

### Nível de Vida vs. Nível de Grau

Nível de Vida documenta marcos globais de desenvolvimento e não multiplica diretamente todos os atributos. Nível de Grau mede domínio da classificação atual e é reiniciado após uma ascensão. Conhecimentos, profissões, história, inventário e vínculos não são apagados.

BASE de protótipo: fase infantil aos cinco anos, um salto temporal para cerca de oito anos, treino supervisionado e reconhecimento de eventos passados. Não criar corpos separados para cada ano.

## 04 — Ascensão da Existência

A classificação evolutiva é independente do destino de nascimento. Um personagem de Vida Serena pode ascender, e alguém da Vida da Ruptura não recebe automaticamente grau superior. Ascensão não é troca livre de dificuldade.

| **Caminho** | **Gatilho conceitual** | **Riscos ou contrapartidas** |
|---|---|---|
| Divino | Pacto, provação ou reconhecimento de uma entidade. | Compromissos e reações religiosas. |
| Arcano | Conhecimento e ritual que modificam a relação com a Trama. | Instabilidades e interesse de pesquisadores. |
| Superação | Feito que excede limites conhecidos. | Prova de domínio e adaptação. |
| Ruptura | Contato com anomalia ou regra incomum da Trama. | Consequências imprevisíveis e atenção de entidades. |

### Requisitos de validação

- Domínio demonstrado e registrado.

- Marco narrativo único concluído.

- Provação específica do grau pretendido.

- Estabilização da nova condição.

- Checagem do histórico contra recompensa duplicada.

| **Grau** | **Design provisório** | **Status** |
|---|---|---|
| I — Comum | Desenvolvimento natural | Protótipo |
| II — Desperta | Primeira transformação | Estrutura/tutoria narrativa |
| III — Elevada | Técnicas extraordinárias | FUTURO |
| IV — Transcendente | Superação de limites naturais | FUTURO |
| V — Primordial | Relação excepcional com a Trama | FUTURO |

A classificação abre novas árvores e requisitos; não aplicar multiplicadores exponenciais de atributos por grau. Primeira ascensão funcional fica fora do vertical slice, embora sua arquitetura e prenúncio narrativo possam existir.

## 05 — Combate, magia e habilidades

Ação em terceira pessoa combinando arma física, magia e técnica especial. Recursos iniciais: Vida, Vigor, Mana; Concentração apenas em mecânicas futuras que justifiquem uma barra adicional.

### Especializações flexíveis

O personagem começa sem classe rígida. Caminhos Marcial, Arcano e Adaptativo são desenvolvidos por afinidade, treino e eventos. O jogador pode cruzar conhecimentos, mas especializações avançadas exigem compromissos de progressão e não podem ser maximizadas todas sem custo.

| **Categoria** | **Exemplo** | **Acesso** |
|---|---|---|
| Armas | Espada, machado, lança, arco | Domínio e requisitos de equipamento. |
| Magia | Elemental, proteção, restauração, arcana | Descoberta, aprendizado e domínio distintos. |
| Híbrida | Lâmina Incandescente | Requisitos explícitos de espada, fogo e encantamento. |
| Rara / Transcendente | Técnicas ligadas à trama | Acontecimentos e grau apropriado. |

Na infância: exploração e prática supervisionada, não combate adulto completo. No vertical slice: espada de madeira, ataque leve/forte, bloqueio/esquiva, uma manifestação mágica, um adversário de treino e barras de Vida/Vigor/Mana.

### Integridade

Não dar experiência por contagem bruta de golpes; limitar farming trivial; habilidades híbridas são combinações aprovadas em dados; separar input, regra de dano e apresentação visual para a futura camada cooperativa.

## 06 — Reputação, relações e NPCs

Reputação contextual: Honra, Compaixão, Renome, Temor e Confiança são dimensões de projeto, mas a primeira implementação deverá usar um subconjunto pequeno verificável (por exemplo, Confiança por NPC e reputação local), sem cinco barras globais complexas.

A mesma escolha pode elevar renome e temor em grupos diferentes. O sistema consulta registros de acontecimentos, não somente uma pontuação de “bem vs. mal”.

### Dez NPCs relevantes de Auren

| **ID** | **NPC** | **Função inicial** |
|---|---|---|
| NPC-01 | Mara | Responsável familiar adaptável à origem. |
| NPC-02 | Daren | Responsável familiar e conhecimento profissional. |
| NPC-03 | Borin | Ferreiro e aprendizado artesanal. |
| NPC-04 | Lysa | Herbalista, natureza e cuidado. |
| NPC-05 | Tovin | Guarda, caça e observação. |
| NPC-06 | Eira | Educadora, história e conhecimentos. |
| NPC-07 | Nilo | Amigo de infância com trajetória própria. |
| NPC-08 | Sera | Amiga/rival com consequências de relacionamento. |
| NPC-09 | Oren | Comerciante e economia. |
| NPC-10 | Maelis | Administração regional e decisões comunitárias. |

Rotinas diárias determinísticas, prioridades para interrupção por eventos (ex.: incêndio), memórias estruturadas e diálogos condicionais. IA generativa opcional só expressa informações autorizadas: não escreve diretamente em inventário, missões, dinheiro ou reputação. Missões principais precisam funcionar offline.

### Memória social

Cada evento persistido usa EventID, NPCID, idade/etapa, tipo, flags de consequência e status de processamento. Persistir somente eventos relevantes; não gravar cada fala repetida.

## 07 — Missões, economia e prevenção de exploits

| **ID** | **Missão** | **Foco** |
|---|---|---|
| Q-01 | Um Novo Amanhecer | Família, interação, deslocamento. |
| Q-02 | Uma Pequena Responsabilidade | Atividade e Experiência de Vida. |
| Q-03 | O Cesto Perdido | Exploração com variantes modulares. |
| Q-04 | Uma Promessa | Escolha social e confiança. |
| Q-05 | O Animal Ferido | Conhecimento e compaixão. |
| Q-06 | O Segredo do Ferreiro | Profissão e domínio. |
| Q-07 | O Desaparecimento | Investigação e consequências. |
| Q-08 | Ecos do Limiar | Mistério principal e passagem temporal. |

Prioridade para o vertical slice: cinco missões centrais e três opcionais. Variações são majoritariamente condições, diálogos e objetos; apenas marcos importantes recebem ramificações maiores. Recompensas narrativas únicas são idempotentes.

### Economia

Três camadas planejadas: cotidiana (moedas e mercadorias), recursos especializados (materiais e conhecimento) e conquistas existenciais (não comercializáveis). Protótipo somente da camada cotidiana. Dinheiro não substitui requisito pessoal de ascensão.

### Catálogo de abuso e contramedidas

| **Risco** | **Proteção de projeto** |
|---|---|
| Começar no fácil e trocar para extremo | Destino natal permanente. |
| Farming de cesta ou alvo indefeso | Domínio/relevância e rendimento limitado. |
| Duplicar recompensa em save/load | Concessão idempotente com histórico e gravação consistente. |
| Ascender por equipamento caro | Requisitos pessoais de domínio e provação. |
| Pular missão ou fase sem aviso | Confirmação de salto temporal e aviso de expiração. |
| IA inventar recompensa | Ações estruturadas validadas pelo domínio. |

## 08 — Mundo, mitologia e campanha

Eryndor é o mundo; Valtheris, o continente inicial; Eldoria, o reino de Auren. A Trama liga possibilidades e consequências. O Limiar é a passagem anterior ao nascimento, normalmente esquecida. Aethron apresenta destinos e não controla necessariamente toda a Trama.

| **Entidade** | **Associação narrativa** |
|---|---|
| Aethron | Guardião do Limiar e mistério de suas informações. |
| Elyra | Ciclos, crescimento e renovação. |
| Vaelor | Investigação e conhecimento arcano. |
| Nythera | Possibilidades e acontecimentos improváveis. |

| **Região de Valtheris** | **Papel conceitual** |
|---|---|
| Eldoria | Reino inicial: Auren, estradas, academia e centro político. |
| Karvorn | Montanhas, mineração e metalurgia. |
| Sylvara | Florestas, natureza e culturas isoladas. |
| Ashkar | Terras áridas e investigação arcana. |
| Nharos | Território esquecido e anomalias da Trama. |

O mistério “Primeira Fratura” liga ruínas e experiências históricas de manipulação da Trama. Campanha planejada: Prólogo/Limiar; Ecos de Auren; Caminhos da Existência; Além das Fronteiras; A Primeira Fratura; O Peso das Escolhas. O protagonista não é isekai nem precisa ser único escolhido.

BASE de conteúdo produzido: Auren, bosque adjacente, três casas acessíveis, três estruturas públicas, dez NPCs. Todo o restante é lore ou expansão, ainda não mundo 3D comprometido.

## 09 — Arte, áudio e produção 3D

Fantasia medieval estilizada com influência anime e cenários ricos, sem reproduzir designs específicos de outras obras. Personagens expressivos, materiais coerentes e símbolo próprio da Trama (fios, círculos incompletos, pontos conectados).

| **Paleta conceitual** | **Uso** |
|---|---|
| Azul profundo #253850 | Menus e elementos arcanos. |
| Dourado #D6B36A | Limiar e ascensão. |
| Verde #648B67 | Áreas naturais. |
| Terracota #A86D52 | Auren e materiais urbanos. |
| Turquesa #86C8C9 | Trama e sinais mágicos. |
| Violeta #9777B8 | Anomalias. |
| Marfim #E9DEC6 | Textos e pergaminhos. |

Bases corporais planejadas: infantil, adolescente e adulto. O vertical slice usa base infantil com variação entre 5 e 8 anos. Não criar modelos por ano de idade. NPCs compartilham roupas, rig e animações quando adequado; figuras centrais preservam reconhecimento entre etapas.

### Pipeline

Conceito visual autoral → Tripo3D ou modelagem manual → Blender (malha, UV, rig, materiais) → exportação → Unity URP → revisão de desempenho e licenças. Registrar prompt, origem, versão e autorização comercial de cada asset. Gerar com IA não dispensa revisão de propriedade intelectual.

Áudio inicial: ambiente de Auren, passos/atividades, uma identidade musical intimista e motivo do Limiar retomado em momentos-chave. HUD simples, legível, escalável, com mensagens que não dependam exclusivamente da cor.

## 10 — Arquitetura técnica e contratos de dados

Camadas: Presentation (HUD/input), Application (comandos e orquestração), Domain (regras e validação), Infrastructure (persistência, serviços e carregamento). Sistemas se comunicam por interfaces e eventos significativos. Não construir um gerenciador global monolítico.

### Módulos iniciais

| **Módulo** | **Responsabilidade** | **Não deve fazer** |
|---|---|---|
| Destiny | Registrar destino natal e origem | Modificar grau de ascensão. |
| Life | Idade, marcos, afinidades | Premiar missões diretamente. |
| Quest | Estados e objetivos | Alterar UI ou inventário sem validação. |
| Event History | Registro idempotente | Armazenar cada frame/diálogo trivial. |
| Reputation | Consequências por grupo/NPC | Reinterpretar moral como pontuação universal. |
| NPC | Rotinas e memória estruturada | Conceder itens via texto de IA. |
| Save | Serializar estado versionado | Serializar referências diretas de cena. |
| Combat | Resolver ações e dano | Ler input diretamente no domínio. |
| Ascension | Validar requisitos e conceder transição | Reescrever destino de nascimento. |

### Contratos essenciais

| **Contrato** | **Campos mínimos** |
|---|---|
| CharacterIdentity | characterId, name, appearanceId, birthDestinyId, originId. |
| LifeState | ageYears, lifeStageId, milestoneIds, lifeLevel. |
| ProgressionState | gradeId, gradeLevel, attributes, affinities, masteryFlags. |
| LifeEvent | eventId, scopeId, age, eventType, consequences, processed. |
| QuestState | questId, status, objectiveFlags, rewardClaimed. |
| NPCState | npcId, scheduleState, relationshipFlags, memoryEventIds. |
| SaveMetadata | schemaVersion, slotId, createdAt, updatedAt, contentVersion. |

ScriptableObjects descrevem definições compartilhadas; runtime state permanece por instância e é serializado por DTOs versionados. A gravação deve usar arquivo temporário, verificação e backup válido. Recompensas únicas são transações lógicas idempotentes.

### Organização das cenas

Bootstrap; MainMenu; CharacterCreation; TheLiminalRealm; Auren_Village; Auren_Interiors; Auren_Forest; Auren_Training; cenas de teste separadas. Streaming continental não faz parte da primeira versão.

### Cooperativo futuro — não implementar ainda

Começar com expedições instanciadas na vida adulta. Mundo pessoal e crescimento permanecem individuais. Combate e recompensas de uma sessão cooperativa serão validados por autoridade da sessão; não presumir que adicionar um pacote de rede converterá o single-player automaticamente.

## 11 — Vertical slice: A Primeira Existência

Hipótese de duração: 45–75 minutos, a verificar em playtests. Sequência: Limiar → destino/origem → criação → despertar em Auren aos 5 anos → tarefas e vínculos → decisão com consequência → desaparecimento → símbolo da Trama → salto para aproximadamente 8 anos → treino marcial/magia.

| **Incluir** | **Excluir nesta fase** |
|---|---|
| Uma vila e um bosque próximo | Continente 3D completo. |
| 10 NPCs relevantes; 4 com memória mais profunda | Todos os NPCs com IA generativa. |
| 8 missões (5 centrais, 3 opcionais) | Sistema infinito/procedural de missões. |
| Um salto temporal confirmado | Vida adulta completa. |
| Combate de treinamento e uma magia | Todos os armamentos/escolas. |
| Save/load de histórico e vínculos | Multiplayer e MMORPG. |
| Prenúncio narrativo de ascensão | Quatro árvores completas de ascensão. |

### Critérios de aceitação do slice

- As 12 combinações de destino/origem entram no mundo sem erro nem história duplicada.

- Ao menos uma decisão altera diálogo futuro e permanece após salvar/carregar.

- O salto temporal é avisado, confirmado e aplicado apenas uma vez.

- O NPC lembra evento relevante sem inventar inventário ou missão.

- Ataque, defesa e magia básica funcionam sem conceder progressão ilimitada por repetição.

- O jogo é compreensível sem conhecimento de Hell Mode; arte e narrativa são independentes.

## 12 — Backlog técnico inicial e testes

Cada tarefa deve produzir código, passos claros de integração no Editor, testes e uma lista de arquivos criados ou alterados. Não delegar “o RPG inteiro” de uma vez ao Claude Code.

| **ID / Prior.** | **Entrega** | **Critério verificável** |
|---|---|---|
| T001 / P0 | Repositório e Unity baseline | Projeto abre em versão fixada; cena Bootstrap carrega. |
| T002 / P0 | Controller + câmera | Movimenta/interage sem referências globais rígidas. |
| T003 / P0 | Destiny + Origin | 4 destinos, 3 origens, destino imutável após confirmação. |
| T004 / P0 | Save v1 | Salvar/carregar identidade, cena e flags; migração de versão. |
| T005 / P0 | LifeEventHistory | EventIDs idempotentes e consulta por NPC/quest. |
| T006 / P0 | Quest framework | Estados, objetivos, recompensa única e persistência. |
| T007 / P0 | NPC e diálogo | Rotina interrompível e condicional por memória. |
| T008 / P0 | Auren graybox | Casa, praça, ferraria, bosque, trajetos navegáveis. |
| T009 / P1 | Life System | Atributos, afinidade e salto temporal confirmado. |
| T010 / P1 | Reputação mínima | Confiança NPC e reputação local persistentes. |
| T011 / P1 | Treino de combate | Ataque leve/forte, defesa e magia inicial. |
| T012 / P1 | Integração de conteúdo | 8 missões e transição narrativa do slice. |
| T013 / P1 | Passagem de arte | Modelos refinados, HUD, animação, áudio, profiling. |
| T014 / P0 | Regressão final | Matriz 12 casos, saves, duplicação e consequências. |

### Dependências críticas

T001 precede todos. T003 e T004 devem existir antes da criação de personagem completa. T005 precede consequências duradouras de T006, T007 e T010. T009 precede o salto temporal da sequência. Arte final T013 não bloqueia validação de mecânica por graybox.

### Testes de comportamento obrigatórios

- Não é possível alterar birthDestinyId depois do nascimento.

- Mesmo evento não concede recompensa duas vezes após salvar/carregar.

- Salto de idade não pode ocorrer sem confirmação nem duplicar ao recuperar arquivo.

- Missão opcional não impede progressão principal.

- Atributos não crescem ilimitadamente com ação trivial repetida.

- Variações de origem modificam contexto sem quebrar a mesma missão base.

- NPC não concede recurso sem validação por operação de domínio.

- Carga de save antigo trata schemaVersion explicitamente.

## 13 — Riscos, pendências e próximos passos

| **Tema** | **Situação / ação** |
|---|---|
| Equilíbrio dos destinos | Playtest A/B; verificar que extremo não se torna escolha obrigatória. |
| Sensação de infância | Testar se atividades cotidianas são divertidas, não burocráticas. |
| Reputação | Definir limiares e grupos após protótipo simples. |
| Números de progressão | Não fixar curvas/XP antes de instrumentação e testes. |
| Viabilidade do coop | Avaliar depois de vertical slice; sem promessa de MMORPG. |
| Licenças/PI | Revisar nomes, arte, assets e termos comerciais das ferramentas. |
| Mercado e orçamento | Pesquisa e estimativa de equipe antes de pitch financeiro. |
| Versão exata da Unity | Fixar após verificar compatibilidade de pacotes. |
| Idiomas e acessibilidade | Definir escopo do lançamento e testes específicos. |

> PORTÃO DE PRODUÇÃO: não expandir para outra cidade antes de concluir e testar a infância jogável em Auren.

### Pacote documental seguinte

Este GDD é a fonte de referência. Derivados recomendados: Technical Design Document por módulo; Bíblia do Universo; Bíblia de Arte; planilha de backlog e custos; pitch comercial. Alterações serão registradas por versão e não substituídas silenciosamente.

## Apêndice A — Brief inicial para Claude Code

Contexto: implementar apenas o vertical slice de Auren, respeitando GDD v1.1. Unity/C#, arquitetura modular, regras testáveis, sem conexão obrigatória de IA ou rede.

### Primeira solicitação sugerida

“Inspecione o projeto Unity existente e proponha somente a estrutura de pastas e assemblies para T001, sem implementar gameplay. Explique dependências, arquivos que criará e critérios de teste. Não modifique pacotes sem justificar; preserve cenas e metadados. Depois de aprovado, implemente apenas T001 e informe como validar no Editor.”

### Definition of Done

Código compila; setup documentado; teste automatizado quando for regra de domínio; execução manual demonstrada; ausência de dependência indevida; alterações anotadas; salvamento compatível; escopo da tarefa respeitado.

### Glossário de produção

**GDD:** Documento central das regras, experiência e conteúdo do jogo.

**Vertical slice:** Trecho pequeno, integrado e representativo da qualidade pretendida.

**Graybox:** Cenário funcional com formas provisórias, antes da arte final.

**Marco narrativo:** Evento que altera fase da vida ou estado importante do mundo.

**Idempotência:** Repetir uma solicitação não duplica seus efeitos ou recompensas.

**Afinidade:** Predisposição evolutiva descoberta e desenvolvida por experiências.

**Destino de Nascimento:** Condição original e permanente, distinta do grau evolutivo.

**Grau de Existência:** Classificação conquistável por ascensão e seus requisitos.

**Fonte de verdade:** Documento de referência cujo controle de alterações é rastreado.

### Registro da versão

**v1.1 —** Consolidação dos capítulos conceituais 0.1–1.0; inclusão de contratos mínimos, backlog T001–T014, testes de regressão e correções sobre origem, nível e envelhecimento.
