# CHRONICLES OF EXISTENCE
## BÍBLIA-MESTRE DE PROMPTS PARA CONCEPT ART — V1.0

**Tipo:** Documento de direção artística e comandos executáveis para IA geradora de imagens  
**Projeto:** Chronicles of Existence (COE)  
**Uso:** ChatGPT com geração de imagens, Astra 6 se disponível na interface do usuário, e outros geradores que aceitem instruções em linguagem natural.  
**Escopo:** 341 fichas individuais de geração, padrões de continuidade, refinamentos, auditoria e organização do acervo.  
**Situação:** Guia de PRODUÇÃO CONCEITUAL. Não constitui aprovação final de modelos, licenças, história ou assets 3D.

> **Como iniciar:** cole a seção “PROMPT DE INICIALIZAÇÃO DO AGENTE ARTÍSTICO” no chat que gerará as artes. Depois envie UMA ficha `COE-...` por vez. Para máxima continuidade, anexe a arte-mestre aprovada de Auren, o modelo-base de personagem, a paleta e a referência anterior do MESMO personagem quando disponíveis. Cada ficha individual abaixo também é autossuficiente em um chat novo.

---

# 1. VERDADES DE PROJETO E CONTROLE DE CANON

### Premissas consolidadas da conversa
- RPG original de fantasia medieval em terceira pessoa, single-player, com cooperativo por expedições previsto como expansão futura.
- Personagem começa jogável aos **cinco anos** e cresce por marcos narrativos; visual infantil, adolescente e adulto precisa manter identidade.
- Após encontro antes do nascimento com **Aethron no Limiar**, escolhe uma entre quatro condições iniciais: **Vida Serena, Vida Normal, Vida Difícil, Vida da Ruptura**. O destino natal fica registrado e não pode ser trocado livremente.
- Três arquétipos familiares iniciais: **agricultor, artesão/comerciante, guardião regional**. Compartilham uma vila e conteúdo modular.
- **Ascensão da Existência** é separada de dificuldade de combate e de origem: caminhos divino, arcano, superação e ruptura. Grau evolui; origem natal não muda.
- Mundo provisoriamente chamado **Eryndor**; continente **Valtheris**; primeira região **Eldoria**; primeira vila jogável **Auren**; bosque **Bosque dos Sussurros**.
- NPCs principais de Auren: **Mara, Daren, Borin, Lysa, Tovin, Eira, Nilo, Sera, Oren e Maelis**.
- Entidades narrativas propostas: **Aethron, Elyra, Vaelor, Nythera**. Mistério da **Trama**, da **Primeira Fratura** e de antigas ruínas **Elnor/Nharos**.
- Estilo: fantasia anime estilizada original, ambientes medievais mais detalhados, personagens expressivos, luz cinematográfica, prioridade de modelagem em **Tripo3D + Blender + Unity URP**.

**Não canônico por mera geração:** posição precisa de cidades no mapa, aparência final de deuses, raças novas, novos personagens, regras numéricas de ascensão, profissões finais de Nilo/Sera, armamentos de regiões futuras. A IA deve marcar essas sugestões como **PROPOSTA VISUAL** até aprovação humana.

**Referências externas:** Hell Mode e Fable servem apenas de inspiração abstrata mencionada no planejamento. NÃO solicitar imagem “no estilo exato de”, semelhança com personagem específico, roupas, emblemas, interface ou cena reconhecível dessas franquias. Não usar logos de terceiros.

# 2. IDENTIDADE VISUAL BLOQUEADA (STYLE LOCK)

**Direção:** premium stylized medieval fantasy for a third-person narrative action RPG; anime-influenced eyes, facial appeal and hair masses combined with believable materials, grounded construction, readable silhouettes and atmospheric, restrained magic. Não é hiper-realismo, cartoon infantil, fantasia grimdark uniforme ou cel-shading chapado em todo o cenário.

**Paleta-base:**
- Azul profundo `#253850`: mistério, noite, menus.
- Dourado celestial `#D6B36A`: Limiar, entidades, reconhecimento.
- Verde natural `#648B67`: campo e bosque.
- Terracota `#A86D52`: vila, arquitetura, comércio.
- Turquesa etéreo `#86C8C9`: Trama.
- Violeta arcano `#9777B8`: anomalias/possibilidades; **não é sinônimo de maldade**.
- Marfim `#E9DEC6`: pergaminhos e interface.

**Materiais:** madeira com veio estilizado, pedra com desgaste plausível, linho, lã, couro, ferro, aço e metal com realce controlado; bordados e emblemas como geometria da Trama com fios entrelaçados, círculos incompletos e pontos nodais. Evitar ornamento minúsculo por toda parte. Observar escala humana, animação, colisão, variantes reutilizáveis e viabilidade de produção 3D.

**Crianças:** anatomia e roupas adequadas à faixa etária, atividades lúdicas e supervisionadas. Nunca sexualizar. Armas iniciais de treino são de madeira e sem fio.

**Ambientação:** Auren é pequena, acolhedora e funcional, não uma capital. A diferença entre destinos aparece principalmente nas condições familiares, objetos, diálogos e alguns detalhes ambientais, não em quatro vilas distintas.

# 3. PROMPT DE INICIALIZAÇÃO DO AGENTE ARTÍSTICO

```text
Você é o DIRETOR DE CONCEPT ART e ART BIBLE KEEPER do jogo original Chronicles of Existence.
Sua tarefa é produzir um acervo de referência utilizável por concept artists, modeladores no Blender/Tripo3D e desenvolvedores da Unity, não imagens bonitas desconectadas.

Mantenha o STYLE LOCK: fantasia medieval estilizada de alta qualidade, rostos e cabelos com influência anime original, arquitetura e materiais funcionais, silhuetas de leitura clara, iluminação cinematográfica controlada, símbolos próprios da Trama, paleta #253850 #D6B36A #648B67 #A86D52 #86C8C9 #9777B8 #E9DEC6. Não imite visual identificável de animes/jogos existentes.

Respeite o CANON: Eryndor/Valtheris/Eldoria/Auren/Bosque dos Sussurros; origem natal condicionada ao destino; início aos 5 anos; personagens fixos Mara, Daren, Borin, Lysa, Tovin, Eira, Nilo, Sera, Oren, Maelis. Aethron, Elyra, Vaelor e Nythera são conceitos narrativos; não invente verdades novas como se estivessem aprovadas.

Quando o pedido for CHARACTER DESIGN, priorize identidade estável (mesmo rosto, proporções, cabelo e roupa entre vistas); quando for PROP/WEAPON, mostre forma completa, frente/perfil/costas quando pertinente, encaixes, materiais e escala; quando for BUILDING, modularidade, entradas, interior/exterior coerentes e circulação; quando for ENVIRONMENT, legibilidade de percurso e composição jogável. Em folhas técnicas, adote fundo neutro e luz de estúdio. Em cenas narrativas, iluminação cinematográfica sem esconder o assunto.

Gere somente o asset ID solicitado no momento. Não produza automaticamente as centenas de imagens restantes em um único mosaico. Apresente uma arte principal por solicitação; se a mesma ficha exigir vistas incompatíveis, indique o desdobramento A: hero view, B: turnaround, C: detalhe, preservando o design aprovado. Evite palavras pequenas dentro da imagem; metadados podem vir no texto da resposta.

Não afirme que duas imagens são idênticas se houver inconsistência. Peça que a referência anterior seja anexada ao gerar vistas futuras quando não estiver disponível no contexto. Mostre defeitos potenciais e proponha refinamento. Não considere nada aprovado sem indicação explícita do diretor humano. Não invente licenças nem diga que um concept art gerado é modelo 3D pronto.
```

# 4. COMO EXECUTAR AS 341 FICHAS

1. Produza primeiro **3 imagens-piloto**: `COE-PRO-001` (criança), `COE-NPC-003` (Borin), `COE-ARC-016` (Praça de Auren). Selecione as melhores e use-as como âncoras visuais.
2. Para cada ficha, gere **imagem principal**. Se houver resultado aprovado, crie **turnaround**, depois **material/detail sheet**, em pedidos separados sempre que qualidade cair ao combinar tudo.
3. Salve imagem, ficha e revisão. Nome: `<ID>_<slug>_concept_v01.png`; versões posteriores `v02`, `v03`; referência congelada `APPROVED`. Não sobrescreva originals.
4. Para um personagem recorrente, anexe a imagem aprovada **daquele personagem** para todas as vistas e idades. A IA não deve mudar traços gratuitamente.
5. Os prompts listados já incluem contexto e instruções, mas se o gerador permitir **imagem de referência**, use a referência aprovada antes de aumentar detalhes.
6. **Status:** `TODO > GENERATED > REVIEW > REVISION > APPROVED > 3D_READY`. `GENERATED` não é `APPROVED`. `3D_READY` só após auditoria por artista.
7. Ordem produtiva: PILOTO > Auren/prótagonista/NPCs > modularidade/props > treino/VFX/UI > expansão de mundo. Arte de regiões futuras não entra automaticamente no escopo do vertical slice.

### Tipo de enquadramento
- **Hero concept:** uma composição de design 3/4, objeto/personagem inteiro, fundo neutro.
- **Turnaround:** frente/perfil/costas e, se couber, 3/4; preservar roupa e escala. Se o gerador não mantiver coerência, produzir cada vista separada usando a hero aprovada como referência.
- **Detail sheet:** close do rosto, cabelo, fechamento de roupa, costura, material, empunhadura, traseira, interior ou juntas, conforme asset.
- **Environment key art:** paisagem 16:9 com circulação e pontos de interesse; sem inventar texto em placas.
- **Props sheet:** objetos distintos em grade limpa, sem se sobrepor, com escala comparativa.
- **VFX sheet:** três estados — antecipação, execução, consequência/dissipação; referência visual, não implementação de shader.

# 5. CONTROLE DE QUALIDADE E REFINAMENTO

**Auditoria obrigatória antes de aprovar:** identidade, silhueta, estilo, proporção/escala, geometria plausível, uso prático, legibilidade sem efeitos, materiais, detalhe para 3D, fidelidade à ficha, ausência de semelhança reconhecível com IP alheia, direitos/origem registrados. Em turnarounds confira números de dedos, ângulo de braços, correspondência frente/verso e fixação real dos acessórios.

**Comandos de refinamento reutilizáveis:**

- **R01 — Consistência:** “Refaça apenas as inconsistências entre os ângulos. Mantenha o rosto, penteado, vestimenta, largura dos ombros, número e posição de bolsas e esquema de cores exatamente como na referência aprovada.”
- **R02 — Viabilidade 3D:** “Simplifique microornamentos e geometrias flutuantes; converta em massas claras, costuras e peças modeláveis. Mostre fechos, espessuras, encaixes e articulações sem alterar a silhueta.”
- **R03 — Rosto:** “Corrija olhos, nariz, mandíbula e cabelo para corresponder ao retrato aprovado. Preserve faixa etária e expressão neutra; não invente traços novos.”
- **R04 — Mãos/anatomia:** “Corrija dedos, mãos, cotovelos, joelhos, tornozelos e proporções. A pose precisa ser anatomia funcional e adequada à idade.”
- **R05 — Cenário:** “Mantenha o mapa geral e as construções existentes; melhore só percurso, escala de porta, entradas, circulação, lógica estrutural e materialidade. Não adicione casas novas.”
- **R06 — Material:** “Separe visualmente madeira, couro, tecido, metal e pedra com valores e rugosidades coerentes; reduza emissivo para não esconder a superfície.”
- **R07 — Marca própria:** “Remova qualquer elemento que lembre diretamente personagem, vestuário, brasão, arma, arquitetura ou UI de franquia conhecida; mantenha conceito funcional original.”
- **R08 — Luz:** “Substitua contraste extremo por luz neutra de estúdio para model sheet; preserve sombras suaves que revelam volumes sem color cast.”
- **R09 — Folha técnica:** “Produza vista frontal ortográfica aproximada do asset APROVADO, corpo/objeto completo, sem pose dinâmica, mesma escala e fundo neutro.”
- **R10 — Revisão da cena:** “Preserve conteúdo aprovado e reequilibre hierarquia focal, profundidade, caminho jogável e escala do protagonista. Sem texto ilegível.”
- **R11 — Criança:** “Garanta anatomia infantil, expressão não adultizada, roupa apropriada e situação sem sexualização nem equipamento perigoso de adulto.”
- **R12 — VFX:** “Reduza bloom, partículas e ocupação de tela para o jogador reconhecer arma, corpo, direção do golpe e alvo.”
- **R13 — Limpeza:** “Remova dedos extras, duplicação de acessórios, ornamentos sem suporte, texturas com artefatos, símbolos acidentais e letras sem sentido.”
- **R14 — Visual de game:** “Converta a ilustração excessivamente pictórica em design de asset 3D: forma, materiais, oclusões, uso em câmera de terceira pessoa e detalhes de fabricação.”
- **R15 — Harmonização:** “Use a arte-piloto aprovada como referência de estilo. Ajuste proporções, paleta, espessura do contorno e tratamento de material sem alterar o conceito deste asset.”

### Não pedir à IA de imagens
- “Faça 341 imagens imediatamente” em uma única composição.
- “Copie exatamente personagem X de anime Y” ou “Hell Mode com nomes trocados”.
- Texto pequeno ou medidas numéricas dentro da arte como única fonte da especificação (completar fora da imagem).
- Garantia de ortografia perfeita do turnaround ou asset 3D otimizado: validar manualmente.
- Suposição de que imagens de um chat distante estão disponíveis no chat atual: anexar as referências novamente quando necessário.

# 6. INDEXAÇÃO E PRIORIDADE

**P0 (Style lock / primeiras entregas):** `COE-PRO-001`, `COE-PRO-002`, `COE-NPC-003`, `COE-NPC-004`, `COE-NPC-007`, `COE-NPC-011`, `COE-ARC-016`, `COE-ARC-001`, `COE-ARC-008`, `COE-NAT-001`, `COE-MAG-001`, `COE-SCN-001`, `COE-SCN-005`.

**P1 (vertical slice):** PRO infantil e turnaround, dez NPCs de Auren, as três residências, ferraria, praça, bosque, missão do cesto, treinamento, a primeira manifestação mágica, props cotidianos e HUD básico.

**P2 (bible/escala):** evolução adolescente/adulta, bibliotecas de rosto/cabelo, equipamentos e criaturas, interiores adicionais, VFX mais amplos.

**P3 (futuro):** regiões além de Eldoria, divindades em cena aprofundada, ascensões avançadas, bestiário extenso, armaduras e equipamentos de fim de jogo.

---

# 7. FICHAS INDIVIDUAIS — COPIAR UMA POR GERAÇÃO

As fichas abaixo incluem instrução de estilo, identidade do objeto, entregáveis e refinamento específico. Gere o **asset indicado**, não elementos novos. Os títulos e IDs fora dos blocos de código são metadados; o texto dentro de cada bloco está pronto para colar no chat de imagem.

## A. Protagonista: vida e identidade

### COE-PRO-001 — Criança base A, cinco anos

**Arquivo sugerido:** `COE-PRO-001_crianca_base_a_cinco_anos_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, perfil, costas e close facial.  
**Revisão específica:** Mãos/pés íntegros, rig possível e silhueta infantil.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-001: Criança base A, cinco anos. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Base infantil original de roupa neutra, curiosidade e proporção crível; sem classe ou poder atribuído. ENTREGA VISUAL: Frente, perfil, costas e close facial. REFINAMENTO CRÍTICO: Mãos/pés íntegros, rig possível e silhueta infantil. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-002 — Criança base B, cinco anos

**Arquivo sugerido:** `COE-PRO-002_crianca_base_b_cinco_anos_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Três vistas e rosto em 3/4.  
**Revisão específica:** Não miniaturizar corpo adulto nem usar figurino sexualizado.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-002: Criança base B, cinco anos. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Segunda base infantil personalizável, mesma escala e linguagem estética, traje rural neutro. ENTREGA VISUAL: Três vistas e rosto em 3/4. REFINAMENTO CRÍTICO: Não miniaturizar corpo adulto nem usar figurino sexualizado. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-003 — Adolescente base A

**Arquivo sugerido:** `COE-PRO-003_adolescente_base_a_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, perfil, costas e comparativo etário.  
**Revisão específica:** Preservar identidade e articulações para rig.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-003: Adolescente base A. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Crescimento do rosto infantil A, roupas de aprendiz e postura de descoberta. ENTREGA VISUAL: Frente, perfil, costas e comparativo etário. REFINAMENTO CRÍTICO: Preservar identidade e articulações para rig. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-004 — Adolescente base B

**Arquivo sugerido:** `COE-PRO-004_adolescente_base_b_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Três vistas e close facial.  
**Revisão específica:** Manter escala entre vistas.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-004: Adolescente base B. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Evolução da base B com vestimenta neutra e anatomia jovem. ENTREGA VISUAL: Três vistas e close facial. REFINAMENTO CRÍTICO: Manter escala entre vistas. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-005 — Adulto base A

**Arquivo sugerido:** `COE-PRO-005_adulto_base_a_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, lateral e costas.  
**Revisão específica:** Espaço para armaduras e acessórios modulares.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-005: Adulto base A. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Corpo e rosto adultos configuráveis, silhueta aventuroso-cotidiana sem classe fixa. ENTREGA VISUAL: Frente, lateral e costas. REFINAMENTO CRÍTICO: Espaço para armaduras e acessórios modulares. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-006 — Adulto base B

**Arquivo sugerido:** `COE-PRO-006_adulto_base_b_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, lateral, costas e cabeça.  
**Revisão específica:** Evitar padrões corporais únicos idealizados.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-006: Adulto base B. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Segunda base adulta diversa e coerente com criança B. ENTREGA VISUAL: Frente, lateral, costas e cabeça. REFINAMENTO CRÍTICO: Evitar padrões corporais únicos idealizados. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-007 — Criança agricultora

**Arquivo sugerido:** `COE-PRO-007_crianca_agricultora_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente e costas; detalhe das amarras.  
**Revisão específica:** Origem rural não equivale necessariamente à pobreza.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-007: Criança agricultora. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Traje de família rural, pequeno avental, botas e reparos discretos. ENTREGA VISUAL: Frente e costas; detalhe das amarras. REFINAMENTO CRÍTICO: Origem rural não equivale necessariamente à pobreza. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-008 — Criança artesã

**Arquivo sugerido:** `COE-PRO-008_crianca_artesa_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, costas e acessórios isolados.  
**Revisão específica:** Sem ferramentas perigosas no corpo.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-008: Criança artesã. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Traje de oficina seguro, mangas presas e bolsinha simples. ENTREGA VISUAL: Frente, costas e acessórios isolados. REFINAMENTO CRÍTICO: Sem ferramentas perigosas no corpo. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-009 — Criança de guardiões

**Arquivo sugerido:** `COE-PRO-009_crianca_de_guardioes_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, lado e costas.  
**Revisão específica:** Não equipar armadura adulta.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-009: Criança de guardiões. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Roupa infantil civil com distintivo regional discreto e calçado de campo. ENTREGA VISUAL: Frente, lado e costas. REFINAMENTO CRÍTICO: Não equipar armadura adulta. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-010 — Adolescente cotidiano

**Arquivo sugerido:** `COE-PRO-010_adolescente_cotidiano_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente e costas, costuras.  
**Revisão específica:** Reutilizável para três origens.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-010: Adolescente cotidiano. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Camadas adaptadas de peças antigas, autonomia emergente. ENTREGA VISUAL: Frente e costas, costuras. REFINAMENTO CRÍTICO: Reutilizável para três origens. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-011 — Adolescente de treino

**Arquivo sugerido:** `COE-PRO-011_adolescente_de_treino_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, costas e articulações.  
**Revisão específica:** Proteções sem restringir movimento.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-011: Adolescente de treino. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Roupas flexíveis para treino supervisionado e espada de madeira. ENTREGA VISUAL: Frente, costas e articulações. REFINAMENTO CRÍTICO: Proteções sem restringir movimento. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-012 — Adulto aventureiro inicial

**Arquivo sugerido:** `COE-PRO-012_adulto_aventureiro_inicial_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, costas e sem capa.  
**Revisão específica:** Carga fisicamente plausível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-012: Adulto aventureiro inicial. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Manto curto, mochila leve, botas de viagem e itens bem distribuídos. ENTREGA VISUAL: Frente, costas e sem capa. REFINAMENTO CRÍTICO: Carga fisicamente plausível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-013 — Adulto aventureiro experiente

**Arquivo sugerido:** `COE-PRO-013_adulto_aventureiro_experiente_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Três vistas e close dos materiais.  
**Revisão específica:** Evitar excesso de minipeças.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-013: Adulto aventureiro experiente. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Vestimenta reparada, troféus discretos e equipamento funcional após muitos anos. ENTREGA VISUAL: Três vistas e close dos materiais. REFINAMENTO CRÍTICO: Evitar excesso de minipeças. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-014 — Especialização marcial

**Arquivo sugerido:** `COE-PRO-014_especializacao_marcial_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, costas e ombreiras.  
**Revisão específica:** Não copiar armaduras conhecidas.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-014: Especialização marcial. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Proteções de mobilidade, silhueta de combatente e materiais de Eldoria. ENTREGA VISUAL: Frente, costas e ombreiras. REFINAMENTO CRÍTICO: Não copiar armaduras conhecidas. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-015 — Especialização arcana

**Arquivo sugerido:** `COE-PRO-015_especializacao_arcana_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, costas e bordados.  
**Revisão específica:** Efeito mágico não deve ocultar roupa.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-015: Especialização arcana. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Roupa de estudioso viajante, bordados geométricos da Trama e bolsas de reagentes. ENTREGA VISUAL: Frente, costas e bordados. REFINAMENTO CRÍTICO: Efeito mágico não deve ocultar roupa. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-016 — Especialização adaptativa

**Arquivo sugerido:** `COE-PRO-016_especializacao_adaptativa_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, costas e carga isolada.  
**Revisão específica:** Sem componentes sem função.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-016: Especialização adaptativa. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Traje híbrido de patrulha com bainha e foco arcano de campo. ENTREGA VISUAL: Frente, costas e carga isolada. REFINAMENTO CRÍTICO: Sem componentes sem função. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-017 — Ascensão divina, protagonista

**Arquivo sugerido:** `COE-PRO-017_ascensao_divina_protagonista_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Antes/depois e símbolo.  
**Revisão específica:** Não dar poder visual gratuito excessivo.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-017: Ascensão divina, protagonista. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Identidade original preservada e marcas douradas da Trama nas vestes. ENTREGA VISUAL: Antes/depois e símbolo. REFINAMENTO CRÍTICO: Não dar poder visual gratuito excessivo. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-018 — Ascensão arcana, protagonista

**Arquivo sugerido:** `COE-PRO-018_ascensao_arcana_protagonista_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Antes/depois e close do foco.  
**Revisão específica:** Manter materialidade e praticidade.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-018: Ascensão arcana, protagonista. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Transformação por foco e padrões geométricos, componentes de estudo estabilizados. ENTREGA VISUAL: Antes/depois e close do foco. REFINAMENTO CRÍTICO: Manter materialidade e praticidade. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-019 — Ascensão por superação

**Arquivo sugerido:** `COE-PRO-019_ascensao_por_superacao_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Comparativo e detalhes.  
**Revisão específica:** Não depender de cicatriz como sinal de mérito.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-019: Ascensão por superação. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Evolução por técnica dominada e equipamento reparado, postura firme. ENTREGA VISUAL: Comparativo e detalhes. REFINAMENTO CRÍTICO: Não depender de cicatriz como sinal de mérito. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-020 — Ascensão pela Ruptura

**Arquivo sugerido:** `COE-PRO-020_ascensao_pela_ruptura_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, costas e close anômalo.  
**Revisão específica:** Não copiar transformação famosa.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-020: Ascensão pela Ruptura. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Assimetrias controladas, turquesa/violeta e símbolos interrompidos. ENTREGA VISUAL: Frente, costas e close anômalo. REFINAMENTO CRÍTICO: Não copiar transformação famosa. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-021 — Turnaround infantil final

**Arquivo sugerido:** `COE-PRO-021_turnaround_infantil_final_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, lado, costas e 3/4.  
**Revisão específica:** Ortografia aproximada e escala constante.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-021: Turnaround infantil final. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Design infantil selecionado em pose neutra e mesma roupa em todos os quadros. ENTREGA VISUAL: Frente, lado, costas e 3/4. REFINAMENTO CRÍTICO: Ortografia aproximada e escala constante. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-022 — Turnaround adolescente final

**Arquivo sugerido:** `COE-PRO-022_turnaround_adolescente_final_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, perfil, costas e 3/4.  
**Revisão específica:** Sem mudança de acessórios.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-022: Turnaround adolescente final. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Design aprovado para modelagem e rig. ENTREGA VISUAL: Frente, perfil, costas e 3/4. REFINAMENTO CRÍTICO: Sem mudança de acessórios. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-023 — Turnaround adulto final

**Arquivo sugerido:** `COE-PRO-023_turnaround_adulto_final_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, lateral, costas e 3/4.  
**Revisão específica:** Perspectiva mínima.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-023: Turnaround adulto final. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Design adulto congelado para referência de produção. ENTREGA VISUAL: Frente, lateral, costas e 3/4. REFINAMENTO CRÍTICO: Perspectiva mínima. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-024 — Expressões da criança

**Arquivo sugerido:** `COE-PRO-024_expressoes_da_crianca_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Seis closes faciais.  
**Revisão específica:** Sem trocar identidade entre quadros.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-024: Expressões da criança. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Mesmo rosto: alegria, dúvida, receio, foco, tristeza e coragem. ENTREGA VISUAL: Seis closes faciais. REFINAMENTO CRÍTICO: Sem trocar identidade entre quadros. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-025 — Expressões adolescente

**Arquivo sugerido:** `COE-PRO-025_expressoes_adolescente_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Seis closes.  
**Revisão específica:** Mesmo cabelo e iluminação.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-025: Expressões adolescente. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Mesmo rosto: orgulho, hesitação, indignação, alegria, susto e serenidade. ENTREGA VISUAL: Seis closes. REFINAMENTO CRÍTICO: Mesmo cabelo e iluminação. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-026 — Expressões adulto

**Arquivo sugerido:** `COE-PRO-026_expressoes_adulto_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Seis closes.  
**Revisão específica:** Preservar estrutura facial.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-026: Expressões adulto. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Mesmo rosto: empatia, suspeita, esforço, surpresa, satisfação e melancolia. ENTREGA VISUAL: Seis closes. REFINAMENTO CRÍTICO: Preservar estrutura facial. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-027 — Protagonista cabelos infantis

**Arquivo sugerido:** `COE-PRO-027_protagonista_cabelos_infantis_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente, lateral e costas.  
**Revisão específica:** Evitar interseção com gola.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-027: Protagonista cabelos infantis. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Seis penteados de Auren, volumes modeláveis e adequados à idade. ENTREGA VISUAL: Frente, lateral e costas. REFINAMENTO CRÍTICO: Evitar interseção com gola. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-028 — Protagonista cabelos adultos

**Arquivo sugerido:** `COE-PRO-028_protagonista_cabelos_adultos_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Frente e costas.  
**Revisão específica:** Mechas em massas coerentes.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-028: Protagonista cabelos adultos. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Seis penteados para viagem e capacete, incluindo versões presas. ENTREGA VISUAL: Frente e costas. REFINAMENTO CRÍTICO: Mechas em massas coerentes. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-029 — Protagonista rosto modular

**Arquivo sugerido:** `COE-PRO-029_protagonista_rosto_modular_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Grade neutra frontal e 3/4.  
**Revisão específica:** Mesma escala para troca de partes.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-029: Protagonista rosto modular. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Biblioteca de formatos de olhos, nariz, boca, mandíbula e sobrancelha. ENTREGA VISUAL: Grade neutra frontal e 3/4. REFINAMENTO CRÍTICO: Mesma escala para troca de partes. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRO-030 — Mãos, botas e cintos por idade

**Arquivo sugerido:** `COE-PRO-030_maos_botas_e_cintos_por_idade_concept_v01.png`  
**Categoria:** personagem protagonista; identidade etária e personalização  
**Saída exigida:** Close e peças soltas.  
**Revisão específica:** Geometria e encaixes aptos ao rig.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRO-030: Mãos, botas e cintos por idade. Categoria: personagem protagonista; identidade etária e personalização. DESCRIÇÃO DIRECIONADA: Três conjuntos de acessórios proporcionais a criança, adolescente e adulto. ENTREGA VISUAL: Close e peças soltas. REFINAMENTO CRÍTICO: Geometria e encaixes aptos ao rig. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```


## B. Habitantes de Auren e divindades

### COE-NPC-001 — Mara

**Arquivo sugerido:** `COE-NPC-001_mara_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Corpo inteiro, três vistas e acessórios por origem.  
**Revisão específica:** Não fixar renda ou profissão antes da escolha.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-001: Mara. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Responsável familiar protetora e observadora, base modular para agricultora, artesã ou guardiã. ENTREGA VISUAL: Corpo inteiro, três vistas e acessórios por origem. REFINAMENTO CRÍTICO: Não fixar renda ou profissão antes da escolha. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-002 — Daren

**Arquivo sugerido:** `COE-NPC-002_daren_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Três vistas e rosto.  
**Revisão específica:** Roupa funcional, expressão afetuosa possível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-002: Daren. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Responsável disciplinado, adulto trabalhador de Auren; três variantes profissionais. ENTREGA VISUAL: Três vistas e rosto. REFINAMENTO CRÍTICO: Roupa funcional, expressão afetuosa possível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-003 — Borin

**Arquivo sugerido:** `COE-NPC-003_borin_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Três vistas, rosto, luvas e avental.  
**Revisão específica:** Punhos livres para animação de forja.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-003: Borin. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Ferreiro robusto, avental de couro, calçados de segurança e ferramentas. ENTREGA VISUAL: Três vistas, rosto, luvas e avental. REFINAMENTO CRÍTICO: Punhos livres para animação de forja. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-004 — Lysa

**Arquivo sugerido:** `COE-NPC-004_lysa_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Três vistas e kit separado.  
**Revisão específica:** Evitar excesso de flores decorativas.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-004: Lysa. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Herbalista paciente, roupas de coleta e bolsas de amostras botânicas. ENTREGA VISUAL: Três vistas e kit separado. REFINAMENTO CRÍTICO: Evitar excesso de flores decorativas. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-005 — Tovin

**Arquivo sugerido:** `COE-NPC-005_tovin_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Frente, costas e perfil com arco.  
**Revisão específica:** Mobilidade e suporte de carga plausíveis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-005: Tovin. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Guarda-caçador, equipamento leve de patrulha e distintivo local discreto. ENTREGA VISUAL: Frente, costas e perfil com arco. REFINAMENTO CRÍTICO: Mobilidade e suporte de carga plausíveis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-006 — Eira

**Arquivo sugerido:** `COE-NPC-006_eira_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Três vistas e close de rosto.  
**Revisão específica:** Peças de cenário de escola coerentes.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-006: Eira. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Educadora inteligente, roupas de ensino e bolsa de livros, sem aparência de maga de combate. ENTREGA VISUAL: Três vistas e close de rosto. REFINAMENTO CRÍTICO: Peças de cenário de escola coerentes. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-007 — Nilo criança

**Arquivo sugerido:** `COE-NPC-007_nilo_crianca_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Três vistas e expressões.  
**Revisão específica:** Preservar traços para envelhecimento.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-007: Nilo criança. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Amigo curioso de cinco anos, vestimenta de exploração segura, acessório pessoal reconhecível. ENTREGA VISUAL: Três vistas e expressões. REFINAMENTO CRÍTICO: Preservar traços para envelhecimento. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-008 — Nilo de oito anos

**Arquivo sugerido:** `COE-NPC-008_nilo_de_oito_anos_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Frente, costas e comparação infantil.  
**Revisão específica:** Sem troca gratuita de identidade.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-008: Nilo de oito anos. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Menino com interesse crescente em exploração e acessório pessoal já conhecido. ENTREGA VISUAL: Frente, costas e comparação infantil. REFINAMENTO CRÍTICO: Sem troca gratuita de identidade. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-009 — Nilo adolescente

**Arquivo sugerido:** `COE-NPC-009_nilo_adolescente_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Turnaround e gesto.  
**Revisão específica:** Relembrar silhueta infantil.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-009: Nilo adolescente. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Aprendiz explorador treinando com Tovin, botas e mochila utilitária. ENTREGA VISUAL: Turnaround e gesto. REFINAMENTO CRÍTICO: Relembrar silhueta infantil. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-010 — Nilo adulto

**Arquivo sugerido:** `COE-NPC-010_nilo_adulto_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Corpo inteiro e comparação de idades.  
**Revisão específica:** Não definir destino narrativo ainda.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-010: Nilo adulto. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Companheiro explorador possível, sinais de percurso e rosto reconhecível. ENTREGA VISUAL: Corpo inteiro e comparação de idades. REFINAMENTO CRÍTICO: Não definir destino narrativo ainda. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-011 — Sera criança

**Arquivo sugerido:** `COE-NPC-011_sera_crianca_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Três vistas e expressões.  
**Revisão específica:** Sem adultização.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-011: Sera criança. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Amiga/rival competitiva de cinco anos, roupas práticas e identidade própria. ENTREGA VISUAL: Três vistas e expressões. REFINAMENTO CRÍTICO: Sem adultização. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-012 — Sera de oito anos

**Arquivo sugerido:** `COE-NPC-012_sera_de_oito_anos_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Frente, costas e item pessoal.  
**Revisão específica:** Não associar rivalidade a vilania.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-012: Sera de oito anos. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Bolsa escolar, detalhes pessoais e postura assertiva. ENTREGA VISUAL: Frente, costas e item pessoal. REFINAMENTO CRÍTICO: Não associar rivalidade a vilania. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-013 — Sera adolescente

**Arquivo sugerido:** `COE-NPC-013_sera_adolescente_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Turnaround e expressões.  
**Revisão específica:** Não fixar carreira canon.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-013: Sera adolescente. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Jovem independente, traje de formação reutilizável para caminhos futuros. ENTREGA VISUAL: Turnaround e expressões. REFINAMENTO CRÍTICO: Não fixar carreira canon. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-014 — Sera adulta

**Arquivo sugerido:** `COE-NPC-014_sera_adulta_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Frente, costas e idades lado a lado.  
**Revisão específica:** Manter liberdade narrativa.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-014: Sera adulta. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Variante futura de Sera com roupas modulares e personalidade reconhecível. ENTREGA VISUAL: Frente, costas e idades lado a lado. REFINAMENTO CRÍTICO: Manter liberdade narrativa. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-015 — Oren

**Arquivo sugerido:** `COE-NPC-015_oren_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Três vistas e bolsa separada.  
**Revisão específica:** Evitar caricatura de trapaceiro.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-015: Oren. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Comerciante pragmático, caderno de contas, bolsos e capa curta. ENTREGA VISUAL: Três vistas e bolsa separada. REFINAMENTO CRÍTICO: Evitar caricatura de trapaceiro. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-016 — Maelis

**Arquivo sugerido:** `COE-NPC-016_maelis_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Três vistas e selo.  
**Revisão específica:** Não parecer monarca.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-016: Maelis. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Administradora diplomática, roupas de autoridade regional e insígnia de Eldoria. ENTREGA VISUAL: Três vistas e selo. REFINAMENTO CRÍTICO: Não parecer monarca. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-017 — Aethron

**Arquivo sugerido:** `COE-NPC-017_aethron_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Corpo inteiro, close de mãos e rosto velado.  
**Revisão específica:** Não imitar entidade de obra existente.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-017: Aethron. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Guardião enigmático do Limiar, vestes claras e douradas, geometria original da Trama. ENTREGA VISUAL: Corpo inteiro, close de mãos e rosto velado. REFINAMENTO CRÍTICO: Não imitar entidade de obra existente. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-018 — Elyra

**Arquivo sugerido:** `COE-NPC-018_elyra_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Frente, 3/4 e emblema.  
**Revisão específica:** Não reduzir a figura a estereótipo de fada.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-018: Elyra. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Divindade dos ciclos, verde/dourado e padrões botânicos estruturais. ENTREGA VISUAL: Frente, 3/4 e emblema. REFINAMENTO CRÍTICO: Não reduzir a figura a estereótipo de fada. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-019 — Vaelor

**Arquivo sugerido:** `COE-NPC-019_vaelor_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Corpo inteiro e objeto focal.  
**Revisão específica:** Diferenciar de mago comum.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-019: Vaelor. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Observador do conhecimento, azul/prata, instrumentos arcanos e postura investigativa. ENTREGA VISUAL: Corpo inteiro e objeto focal. REFINAMENTO CRÍTICO: Diferenciar de mago comum. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-020 — Nythera

**Arquivo sugerido:** `COE-NPC-020_nythera_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Frente, costas e silhueta.  
**Revisão específica:** Não caracterizar automaticamente como vilã.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-020: Nythera. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Guardiã das possibilidades, assimetria controlada, violeta/turquesa não moralizados. ENTREGA VISUAL: Frente, costas e silhueta. REFINAMENTO CRÍTICO: Não caracterizar automaticamente como vilã. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-021 — Aldeão agricultor A

**Arquivo sugerido:** `COE-NPC-021_aldeao_agricultor_a_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Frente, costas e props.  
**Revisão específica:** Individualidade, não aparência genérica repetida.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-021: Aldeão agricultor A. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Adulto rural com mãos de trabalho, traje reparado e ferramenta de ofício. ENTREGA VISUAL: Frente, costas e props. REFINAMENTO CRÍTICO: Individualidade, não aparência genérica repetida. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-022 — Aldeã agricultora B

**Arquivo sugerido:** `COE-NPC-022_aldea_agricultora_b_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Frente e costas.  
**Revisão específica:** Não recolorir aldeão A simplesmente.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-022: Aldeã agricultora B. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Silhueta rural distinta, avental de colheita e roupa de estação. ENTREGA VISUAL: Frente e costas. REFINAMENTO CRÍTICO: Não recolorir aldeão A simplesmente. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-023 — Artesão secundário

**Arquivo sugerido:** `COE-NPC-023_artesao_secundario_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Corpo inteiro e close de cinto.  
**Revisão específica:** Diferente visualmente do ferreiro.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-023: Artesão secundário. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Operário de Auren com utensílios de carpintaria e roupa segura. ENTREGA VISUAL: Corpo inteiro e close de cinto. REFINAMENTO CRÍTICO: Diferente visualmente do ferreiro. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-024 — Guarda regional comum

**Arquivo sugerido:** `COE-NPC-024_guarda_regional_comum_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Frente, costas e sem capa.  
**Revisão específica:** Não armadura palaciana.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-024: Guarda regional comum. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Uniforme modular discreto de patrulha, capa curta e proteção funcional. ENTREGA VISUAL: Frente, costas e sem capa. REFINAMENTO CRÍTICO: Não armadura palaciana. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-025 — Mercador viajante

**Arquivo sugerido:** `COE-NPC-025_mercador_viajante_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Três vistas e mochila isolada.  
**Revisão específica:** Volume transportável.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-025: Mercador viajante. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Adulto com capa resistente, carga transportável e reparos nos tecidos. ENTREGA VISUAL: Três vistas e mochila isolada. REFINAMENTO CRÍTICO: Volume transportável. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-026 — Crianças secundárias

**Arquivo sugerido:** `COE-NPC-026_criancas_secundarias_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Grupo e silhuetas individuais.  
**Revisão específica:** Mesma faixa etária e escala.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-026: Crianças secundárias. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Quatro crianças da vila com variedade de rostos e roupas coerentes. ENTREGA VISUAL: Grupo e silhuetas individuais. REFINAMENTO CRÍTICO: Mesma faixa etária e escala. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-027 — Idosos da vila

**Arquivo sugerido:** `COE-NPC-027_idosos_da_vila_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Corpo inteiro e rostos.  
**Revisão específica:** Idade não significa fragilidade.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-027: Idosos da vila. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Dois idosos de trajetórias distintas, acessórios de ofício e individualidade. ENTREGA VISUAL: Corpo inteiro e rostos. REFINAMENTO CRÍTICO: Idade não significa fragilidade. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-028 — Sacerdote local

**Arquivo sugerido:** `COE-NPC-028_sacerdote_local_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Frente, costas e emblema.  
**Revisão específica:** Não antecipar divindade superior como única.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-028: Sacerdote local. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Figurino modesto de tradição regional, símbolo original sem culto exclusivo. ENTREGA VISUAL: Frente, costas e emblema. REFINAMENTO CRÍTICO: Não antecipar divindade superior como única. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-029 — Aprendiz de ferreiro

**Arquivo sugerido:** `COE-NPC-029_aprendiz_de_ferreiro_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Três vistas e luvas.  
**Revisão específica:** Seguro e distinto de Borin.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-029: Aprendiz de ferreiro. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Adolescente com avental curto, proteção de oficina e ferramentas leves. ENTREGA VISUAL: Três vistas e luvas. REFINAMENTO CRÍTICO: Seguro e distinto de Borin. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NPC-030 — Professora visitante de Lythar

**Arquivo sugerido:** `COE-NPC-030_professora_visitante_de_lythar_concept_v01.png`  
**Categoria:** personagem de Auren ou mitologia; personalidade e função social  
**Saída exigida:** Frente e costas, livros.  
**Revisão específica:** Referência futura, não obrigatória no protótipo.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NPC-030: Professora visitante de Lythar. Categoria: personagem de Auren ou mitologia; personalidade e função social. DESCRIÇÃO DIRECIONADA: Personagem secundária de academia externa, traje de viagem e registros. ENTREGA VISUAL: Frente e costas, livros. REFINAMENTO CRÍTICO: Referência futura, não obrigatória no protótipo. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```


## C. Biblioteca de rosto, cabelo e proporções

### COE-MOR-001 — Rostos infantis

**Arquivo sugerido:** `COE-MOR-001_rostos_infantis_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Frente e 3/4 em grade.  
**Revisão específica:** Sem miniadultos nem clones.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-001: Rostos infantis. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Oito rostos originais com formas e tons variados, mesmo acabamento. ENTREGA VISUAL: Frente e 3/4 em grade. REFINAMENTO CRÍTICO: Sem miniadultos nem clones. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-002 — Rostos adolescentes

**Arquivo sugerido:** `COE-MOR-002_rostos_adolescentes_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Frente e 3/4.  
**Revisão específica:** Escala facial constante.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-002: Rostos adolescentes. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Oito jovens de Auren com traços coerentes e diferenciação. ENTREGA VISUAL: Frente e 3/4. REFINAMENTO CRÍTICO: Escala facial constante. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-003 — Rostos adultos

**Arquivo sugerido:** `COE-MOR-003_rostos_adultos_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Frente e perfil leve.  
**Revisão específica:** Evitar pele fotográfica.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-003: Rostos adultos. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Oito rostos para criação e NPCs, expressões neutras. ENTREGA VISUAL: Frente e perfil leve. REFINAMENTO CRÍTICO: Evitar pele fotográfica. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-004 — Rostos idosos

**Arquivo sugerido:** `COE-MOR-004_rostos_idosos_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Frente e perfil.  
**Revisão específica:** Não caricaturar envelhecimento.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-004: Rostos idosos. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Seis identidades com marcas estilizadas de idade. ENTREGA VISUAL: Frente e perfil. REFINAMENTO CRÍTICO: Não caricaturar envelhecimento. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-005 — Olhos

**Arquivo sugerido:** `COE-MOR-005_olhos_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Detalhes frontais e 3/4.  
**Revisão específica:** Sem codificar moralidade pelo olho.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-005: Olhos. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Doze formatos de olhos e pálpebras na mesma cabeça-base. ENTREGA VISUAL: Detalhes frontais e 3/4. REFINAMENTO CRÍTICO: Sem codificar moralidade pelo olho. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-006 — Sobrancelhas

**Arquivo sugerido:** `COE-MOR-006_sobrancelhas_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Grade em mesmo rosto.  
**Revisão específica:** Posição anatômica uniforme.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-006: Sobrancelhas. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Doze formas modulares, curvas e densidades distintas. ENTREGA VISUAL: Grade em mesmo rosto. REFINAMENTO CRÍTICO: Posição anatômica uniforme. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-007 — Narizes

**Arquivo sugerido:** `COE-MOR-007_narizes_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Frontal e lateral.  
**Revisão específica:** Sem realismo discrepante.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-007: Narizes. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Oito narizes adequados ao estilo anime-fantasia. ENTREGA VISUAL: Frontal e lateral. REFINAMENTO CRÍTICO: Sem realismo discrepante. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-008 — Bocas e mandíbulas

**Arquivo sugerido:** `COE-MOR-008_bocas_e_mandibulas_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Frente, perfil e sorriso.  
**Revisão específica:** Preparar deformação facial.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-008: Bocas e mandíbulas. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Oito variantes em rostos padronizados. ENTREGA VISUAL: Frente, perfil e sorriso. REFINAMENTO CRÍTICO: Preparar deformação facial. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-009 — Cabelos infantis curtos

**Arquivo sugerido:** `COE-MOR-009_cabelos_infantis_curtos_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Três vistas por corte.  
**Revisão específica:** Volumes limpos.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-009: Cabelos infantis curtos. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Seis cortes modeláveis e apropriados a cinco anos. ENTREGA VISUAL: Três vistas por corte. REFINAMENTO CRÍTICO: Volumes limpos. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-010 — Cabelos infantis médios

**Arquivo sugerido:** `COE-MOR-010_cabelos_infantis_medios_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Frente e costas.  
**Revisão específica:** Não invadir olhos.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-010: Cabelos infantis médios. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Seis opções com tranças ou presilhas simples. ENTREGA VISUAL: Frente e costas. REFINAMENTO CRÍTICO: Não invadir olhos. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-011 — Cabelos adolescentes

**Arquivo sugerido:** `COE-MOR-011_cabelos_adolescentes_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Frente, lado e costas.  
**Revisão específica:** Movimento viável.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-011: Cabelos adolescentes. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Oito estilos para treino e cotidiano. ENTREGA VISUAL: Frente, lado e costas. REFINAMENTO CRÍTICO: Movimento viável. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-012 — Cabelos adultos curtos

**Arquivo sugerido:** `COE-MOR-012_cabelos_adultos_curtos_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Três vistas.  
**Revisão específica:** Coerência de mechas.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-012: Cabelos adultos curtos. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Oito opções ligadas a ofícios e viagem. ENTREGA VISUAL: Três vistas. REFINAMENTO CRÍTICO: Coerência de mechas. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-013 — Cabelos adultos longos

**Arquivo sugerido:** `COE-MOR-013_cabelos_adultos_longos_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Frente e costas.  
**Revisão específica:** Sem centenas de fios isolados.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-013: Cabelos adultos longos. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Oito soluções com pontos de amarração. ENTREGA VISUAL: Frente e costas. REFINAMENTO CRÍTICO: Sem centenas de fios isolados. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-014 — Tranças e coques

**Arquivo sugerido:** `COE-MOR-014_trancas_e_coques_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Traseira em close e frente.  
**Revisão específica:** Fechos claros.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-014: Tranças e coques. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Oito arranjos de trabalho, estudo e patrulha. ENTREGA VISUAL: Traseira em close e frente. REFINAMENTO CRÍTICO: Fechos claros. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-015 — Cabelos de guerreiro

**Arquivo sugerido:** `COE-MOR-015_cabelos_de_guerreiro_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Com/sem capacete.  
**Revisão específica:** Evitar clipping.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-015: Cabelos de guerreiro. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Seis cortes compatíveis com capacetes e capuzes. ENTREGA VISUAL: Com/sem capacete. REFINAMENTO CRÍTICO: Evitar clipping. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-016 — Cabelos arcanistas

**Arquivo sugerido:** `COE-MOR-016_cabelos_arcanistas_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Frente e costas.  
**Revisão específica:** Sem acessórios flutuantes.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-016: Cabelos arcanistas. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Seis opções com adereços de estudo contidos. ENTREGA VISUAL: Frente e costas. REFINAMENTO CRÍTICO: Sem acessórios flutuantes. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-017 — Barbas e bigodes

**Arquivo sugerido:** `COE-MOR-017_barbas_e_bigodes_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Frente e perfil.  
**Revisão específica:** Não usar textura foto-real.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-017: Barbas e bigodes. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Doze tipos com silhueta estilizada. ENTREGA VISUAL: Frente e perfil. REFINAMENTO CRÍTICO: Não usar textura foto-real. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-018 — Cores de cabelo

**Arquivo sugerido:** `COE-MOR-018_cores_de_cabelo_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Swatches e mechas sob luz neutra.  
**Revisão específica:** Manter materialidade.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-018: Cores de cabelo. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Amostras de cores naturais e variações mágicas justificadas. ENTREGA VISUAL: Swatches e mechas sob luz neutra. REFINAMENTO CRÍTICO: Manter materialidade. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-019 — Corpo infantil 5 anos

**Arquivo sugerido:** `COE-MOR-019_corpo_infantil_5_anos_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Frente, lado e silhueta.  
**Revisão específica:** Nunca escalar adulto uniformemente.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-019: Corpo infantil 5 anos. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Proporções estilizadas, posturas e roupas neutras de modelagem. ENTREGA VISUAL: Frente, lado e silhueta. REFINAMENTO CRÍTICO: Nunca escalar adulto uniformemente. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-020 — Corpo infantil 8 anos

**Arquivo sugerido:** `COE-MOR-020_corpo_infantil_8_anos_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Frente e comparação de idade.  
**Revisão específica:** Diferenças de desenvolvimento verossímeis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-020: Corpo infantil 8 anos. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Crescimento anatômico intermediário com mesmo personagem. ENTREGA VISUAL: Frente e comparação de idade. REFINAMENTO CRÍTICO: Diferenças de desenvolvimento verossímeis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-021 — Corpo adolescente

**Arquivo sugerido:** `COE-MOR-021_corpo_adolescente_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Frente, lado, costas.  
**Revisão específica:** Sem sexualização.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-021: Corpo adolescente. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Bases proporcionais para animação e vestuário. ENTREGA VISUAL: Frente, lado, costas. REFINAMENTO CRÍTICO: Sem sexualização. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-022 — Corpo adulto

**Arquivo sugerido:** `COE-MOR-022_corpo_adulto_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Frente, lado e costas.  
**Revisão específica:** Não idealizar um único padrão.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-022: Corpo adulto. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Bases corpóreas variadas para personalização. ENTREGA VISUAL: Frente, lado e costas. REFINAMENTO CRÍTICO: Não idealizar um único padrão. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-023 — Silhuetas de profissões

**Arquivo sugerido:** `COE-MOR-023_silhuetas_de_profissoes_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Folha monocromática.  
**Revisão específica:** Leitura em escala pequena.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-023: Silhuetas de profissões. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Dez personagens reconhecíveis sem cor pela ferramenta e postura. ENTREGA VISUAL: Folha monocromática. REFINAMENTO CRÍTICO: Leitura em escala pequena. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-024 — Mãos por idade

**Arquivo sugerido:** `COE-MOR-024_maos_por_idade_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Close aberto e segurando objeto.  
**Revisão específica:** Dedos e articulações consistentes.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-024: Mãos por idade. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Estudos de mãos infantil, adolescente, adulto e empunhadura. ENTREGA VISUAL: Close aberto e segurando objeto. REFINAMENTO CRÍTICO: Dedos e articulações consistentes. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MOR-025 — Pés, sapatos e botas

**Arquivo sugerido:** `COE-MOR-025_pes_sapatos_e_botas_concept_v01.png`  
**Categoria:** folha técnica de criação modular e anatomia estilizada  
**Saída exigida:** Perfil, frente e sola.  
**Revisão específica:** Material e articulação coerentes.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MOR-025: Pés, sapatos e botas. Categoria: folha técnica de criação modular e anatomia estilizada. DESCRIÇÃO DIRECIONADA: Base de pés e calçados proporcionais por etapa de vida. ENTREGA VISUAL: Perfil, frente e sola. REFINAMENTO CRÍTICO: Material e articulação coerentes. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```


## D. Roupas civis e ornamentos

### COE-CIV-001 — Roupa infantil simples

**Arquivo sugerido:** `COE-CIV-001_roupa_infantil_simples_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Frente, costas e camadas.  
**Revisão específica:** Sem miniarmadura.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-001: Roupa infantil simples. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Linho e lã leve, faixa e fechos seguros para cotidiano. ENTREGA VISUAL: Frente, costas e camadas. REFINAMENTO CRÍTICO: Sem miniarmadura. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-002 — Roupa criança agricultora

**Arquivo sugerido:** `COE-CIV-002_roupa_crianca_agricultora_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Frente e costas.  
**Revisão específica:** Ruralidade não iguala miséria.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-002: Roupa criança agricultora. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Avental, calçado de campo, bolsinha e tecidos simples. ENTREGA VISUAL: Frente e costas. REFINAMENTO CRÍTICO: Ruralidade não iguala miséria. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-003 — Roupa criança artesã

**Arquivo sugerido:** `COE-CIV-003_roupa_crianca_artesa_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Frente, costas e detalhe.  
**Revisão específica:** Nenhum instrumento perigoso.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-003: Roupa criança artesã. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Camadas de oficina com mangas presas e pequenos bolsos. ENTREGA VISUAL: Frente, costas e detalhe. REFINAMENTO CRÍTICO: Nenhum instrumento perigoso. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-004 — Roupa criança guardiã

**Arquivo sugerido:** `COE-CIV-004_roupa_crianca_guardia_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Frente e costas.  
**Revisão específica:** Sem traje militar adulto.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-004: Roupa criança guardiã. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Civil com distintivo de família e luvas leves de treino. ENTREGA VISUAL: Frente e costas. REFINAMENTO CRÍTICO: Sem traje militar adulto. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-005 — Adolescente cotidiano

**Arquivo sugerido:** `COE-CIV-005_adolescente_cotidiano_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Frente e costas.  
**Revisão específica:** Útil nos três arquétipos.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-005: Adolescente cotidiano. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Peças adaptadas e camadas com marcas de crescimento. ENTREGA VISUAL: Frente e costas. REFINAMENTO CRÍTICO: Útil nos três arquétipos. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-006 — Traje rural de verão

**Arquivo sugerido:** `COE-CIV-006_traje_rural_de_verao_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Conjunto desmontado e vestido.  
**Revisão específica:** Respirável, funcional.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-006: Traje rural de verão. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Chapéu, linho leve, bolsa de campo e botas baixas. ENTREGA VISUAL: Conjunto desmontado e vestido. REFINAMENTO CRÍTICO: Respirável, funcional. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-007 — Traje rural de inverno

**Arquivo sugerido:** `COE-CIV-007_traje_rural_de_inverno_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Frente, costas e sem capa.  
**Revisão específica:** Materiais regionais críveis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-007: Traje rural de inverno. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Lã, capa, luvas, botas e fechamentos térmicos. ENTREGA VISUAL: Frente, costas e sem capa. REFINAMENTO CRÍTICO: Materiais regionais críveis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-008 — Uniforme do ferreiro

**Arquivo sugerido:** `COE-CIV-008_uniforme_do_ferreiro_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Frente e peças isoladas.  
**Revisão específica:** Segurança de forja.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-008: Uniforme do ferreiro. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Avental de couro, proteção nos pés e mangas controladas. ENTREGA VISUAL: Frente e peças isoladas. REFINAMENTO CRÍTICO: Segurança de forja. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-009 — Roupa de herbalista

**Arquivo sugerido:** `COE-CIV-009_roupa_de_herbalista_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Três vistas e close kit.  
**Revisão específica:** Não adornar com plantas soltas.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-009: Roupa de herbalista. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Túnica de coleta e bolsa compartimentada. ENTREGA VISUAL: Três vistas e close kit. REFINAMENTO CRÍTICO: Não adornar com plantas soltas. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-010 — Roupa da educadora

**Arquivo sugerido:** `COE-CIV-010_roupa_da_educadora_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Frente e costas.  
**Revisão específica:** Não fantasia de feiticeira.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-010: Roupa da educadora. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Vestido/túnica profissional, caderno e bolsa, aparência sóbria. ENTREGA VISUAL: Frente e costas. REFINAMENTO CRÍTICO: Não fantasia de feiticeira. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-011 — Traje mercantil

**Arquivo sugerido:** `COE-CIV-011_traje_mercantil_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Três vistas.  
**Revisão específica:** Status de vila pequena.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-011: Traje mercantil. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Colete, capa curta e livro de contas. ENTREGA VISUAL: Três vistas. REFINAMENTO CRÍTICO: Status de vila pequena. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-012 — Traje autoridade local

**Arquivo sugerido:** `COE-CIV-012_traje_autoridade_local_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Frente, costas e emblema.  
**Revisão específica:** Não roupagem régia.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-012: Traje autoridade local. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Distintivo discreto, tecido de qualidade e mobilidade. ENTREGA VISUAL: Frente, costas e emblema. REFINAMENTO CRÍTICO: Não roupagem régia. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-013 — Traje viajante

**Arquivo sugerido:** `COE-CIV-013_traje_viajante_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Três vistas e partes.  
**Revisão específica:** Peso realista.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-013: Traje viajante. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Manto impermeável, mochila e bolsas de acesso rápido. ENTREGA VISUAL: Três vistas e partes. REFINAMENTO CRÍTICO: Peso realista. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-014 — Roupa de caça

**Arquivo sugerido:** `COE-CIV-014_roupa_de_caca_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Frente, costas e capuz.  
**Revisão específica:** Sem camuflagem moderna.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-014: Roupa de caça. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Tons naturais e bolsos silenciosos. ENTREGA VISUAL: Frente, costas e capuz. REFINAMENTO CRÍTICO: Sem camuflagem moderna. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-015 — Roupa de celebração

**Arquivo sugerido:** `COE-CIV-015_roupa_de_celebracao_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Corpo inteiro e padrão têxtil.  
**Revisão específica:** Reutilizável entre NPCs.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-015: Roupa de celebração. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Bordados típicos de Auren, duas variações para adultos. ENTREGA VISUAL: Corpo inteiro e padrão têxtil. REFINAMENTO CRÍTICO: Reutilizável entre NPCs. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-016 — Roupa cerimonial de luto

**Arquivo sugerido:** `COE-CIV-016_roupa_cerimonial_de_luto_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Frente e detalhe.  
**Revisão específica:** Não fixar religião.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-016: Roupa cerimonial de luto. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Peças sóbrias e ornamento de memória familiar. ENTREGA VISUAL: Frente e detalhe. REFINAMENTO CRÍTICO: Não fixar religião. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-017 — Roupa residencial

**Arquivo sugerido:** `COE-CIV-017_roupa_residencial_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Frente e costas.  
**Revisão específica:** Distinta da roupa de rua.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-017: Roupa residencial. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Camadas leves, avental removível e chinelos. ENTREGA VISUAL: Frente e costas. REFINAMENTO CRÍTICO: Distinta da roupa de rua. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-018 — Vestuário de estudo

**Arquivo sugerido:** `COE-CIV-018_vestuario_de_estudo_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Três soluções.  
**Revisão específica:** Não uniforme escolar moderno.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-018: Vestuário de estudo. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Roupa de aluno de vila e itens de escrita. ENTREGA VISUAL: Três soluções. REFINAMENTO CRÍTICO: Não uniforme escolar moderno. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-019 — Roupa aprendiz oficina

**Arquivo sugerido:** `COE-CIV-019_roupa_aprendiz_oficina_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Frente, costas e mãos.  
**Revisão específica:** Ergonomia para atividades seguras.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-019: Roupa aprendiz oficina. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Avental curto, proteção suficiente e presilhas. ENTREGA VISUAL: Frente, costas e mãos. REFINAMENTO CRÍTICO: Ergonomia para atividades seguras. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-020 — Traje agricultor experiente

**Arquivo sugerido:** `COE-CIV-020_traje_agricultor_experiente_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Três vistas.  
**Revisão específica:** Individualidade.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-020: Traje agricultor experiente. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Chapéu, roupa resistente e peças reparadas. ENTREGA VISUAL: Três vistas. REFINAMENTO CRÍTICO: Individualidade. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-021 — Vestuário de idoso

**Arquivo sugerido:** `COE-CIV-021_vestuario_de_idoso_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Duas variantes.  
**Revisão específica:** Não reduzir idade a fragilidade.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-021: Vestuário de idoso. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Camadas funcionais e acessórios de ofício. ENTREGA VISUAL: Duas variantes. REFINAMENTO CRÍTICO: Não reduzir idade a fragilidade. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-022 — Capas e mantos civis

**Arquivo sugerido:** `COE-CIV-022_capas_e_mantos_civis_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Frente, costas e padrão de tecido.  
**Revisão específica:** Não clipping em animação.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-022: Capas e mantos civis. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Seis formatos com fechos e golas distintas. ENTREGA VISUAL: Frente, costas e padrão de tecido. REFINAMENTO CRÍTICO: Não clipping em animação. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-023 — Cintos e bolsas

**Arquivo sugerido:** `COE-CIV-023_cintos_e_bolsas_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Isolados e aplicados.  
**Revisão específica:** Pontos de fixação visíveis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-023: Cintos e bolsas. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Biblioteca modular de fechos, pequenas bolsas, cordões e porta-livros. ENTREGA VISUAL: Isolados e aplicados. REFINAMENTO CRÍTICO: Pontos de fixação visíveis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-024 — Botas e sapatos

**Arquivo sugerido:** `COE-CIV-024_botas_e_sapatos_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Perfil, frente e sola.  
**Revisão específica:** Uso plausível em campo.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-024: Botas e sapatos. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Oito pares por idade e profissão. ENTREGA VISUAL: Perfil, frente e sola. REFINAMENTO CRÍTICO: Uso plausível em campo. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CIV-025 — Luvas de trabalho

**Arquivo sugerido:** `COE-CIV-025_luvas_de_trabalho_concept_v01.png`  
**Categoria:** figurino cotidiano com costuras, peso e sobreposição plausíveis  
**Saída exigida:** Close e sobre mãos.  
**Revisão específica:** Dedos íntegros.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CIV-025: Luvas de trabalho. Categoria: figurino cotidiano com costuras, peso e sobreposição plausíveis. DESCRIÇÃO DIRECIONADA: Coleção para forja, coleta, lavoura e viagem. ENTREGA VISUAL: Close e sobre mãos. REFINAMENTO CRÍTICO: Dedos íntegros. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```


## E. Equipamento aventureiro e armaduras

### COE-ADV-001 — Traje marcial inicial

**Arquivo sugerido:** `COE-ADV-001_traje_marcial_inicial_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Frente, costas e camada interna.  
**Revisão específica:** Mobilidade de ombros/quadril.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-001: Traje marcial inicial. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Couro reforçado, roupa leve de treino e bolsos práticos para primeiro adulto aventureiro. ENTREGA VISUAL: Frente, costas e camada interna. REFINAMENTO CRÍTICO: Mobilidade de ombros/quadril. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-002 — Traje arcano inicial

**Arquivo sugerido:** `COE-ADV-002_traje_arcano_inicial_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Frente e costas, foco isolado.  
**Revisão específica:** Sem robe que arraste no chão.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-002: Traje arcano inicial. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Túnica curta, foco simples e kit de notas, magia discretamente ornamentada. ENTREGA VISUAL: Frente e costas, foco isolado. REFINAMENTO CRÍTICO: Sem robe que arraste no chão. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-003 — Traje adaptativo inicial

**Arquivo sugerido:** `COE-ADV-003_traje_adaptativo_inicial_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Três vistas e acessórios soltos.  
**Revisão específica:** Peso distribuído.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-003: Traje adaptativo inicial. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Camadas de patrulha e suporte para instrumento arcano e arma leve. ENTREGA VISUAL: Três vistas e acessórios soltos. REFINAMENTO CRÍTICO: Peso distribuído. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-004 — Marcial intermediário

**Arquivo sugerido:** `COE-ADV-004_marcial_intermediario_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Frente, costas e articulações.  
**Revisão específica:** Definição clara de proteção.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-004: Marcial intermediário. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Armadura segmentada leve/média, sinais de uso e distintivos conquistados. ENTREGA VISUAL: Frente, costas e articulações. REFINAMENTO CRÍTICO: Definição clara de proteção. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-005 — Arcano intermediário

**Arquivo sugerido:** `COE-ADV-005_arcano_intermediario_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Frente, costas e tecido.  
**Revisão específica:** Sem excesso de VFX.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-005: Arcano intermediário. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Manto de campo e padrões da Trama aplicados em costuras. ENTREGA VISUAL: Frente, costas e tecido. REFINAMENTO CRÍTICO: Sem excesso de VFX. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-006 — Adaptativo intermediário

**Arquivo sugerido:** `COE-ADV-006_adaptativo_intermediario_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Frente e costas.  
**Revisão específica:** Evitar dez armas acopladas.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-006: Adaptativo intermediário. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Kit de exploração e reforços adequados a técnica híbrida. ENTREGA VISUAL: Frente e costas. REFINAMENTO CRÍTICO: Evitar dez armas acopladas. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-007 — Marcial avançado

**Arquivo sugerido:** `COE-ADV-007_marcial_avancado_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Turnaround e peças desmontadas.  
**Revisão específica:** Não parecer traje de divindade.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-007: Marcial avançado. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Visual de maestria, proteções funcionais e herança visual de Eldoria. ENTREGA VISUAL: Turnaround e peças desmontadas. REFINAMENTO CRÍTICO: Não parecer traje de divindade. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-008 — Arcano avançado

**Arquivo sugerido:** `COE-ADV-008_arcano_avancado_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Turnaround e bordados.  
**Revisão específica:** Simplicidade na leitura.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-008: Arcano avançado. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Vestes de pesquisador experiente e instrumento arcano estabilizado. ENTREGA VISUAL: Turnaround e bordados. REFINAMENTO CRÍTICO: Simplicidade na leitura. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-009 — Adaptativo avançado

**Arquivo sugerido:** `COE-ADV-009_adaptativo_avancado_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Três vistas e mapa de carga.  
**Revisão específica:** Sem acessórios decorativos inúteis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-009: Adaptativo avançado. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Equipamento de sobrevivente versátil, múltiplos usos reais. ENTREGA VISUAL: Três vistas e mapa de carga. REFINAMENTO CRÍTICO: Sem acessórios decorativos inúteis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-010 — Traje espada e magia

**Arquivo sugerido:** `COE-ADV-010_traje_espada_e_magia_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Frente, costas e pose de uso.  
**Revisão específica:** Testar volumes durante animação.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-010: Traje espada e magia. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Uma mão livre para conjuração, bainha e foco arcano sem interseção. ENTREGA VISUAL: Frente, costas e pose de uso. REFINAMENTO CRÍTICO: Testar volumes durante animação. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-011 — Traje de exploração leve

**Arquivo sugerido:** `COE-ADV-011_traje_de_exploracao_leve_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Frente e costas.  
**Revisão específica:** Não militar moderno.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-011: Traje de exploração leve. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Botas, capa curta, mochila e proteção de terreno. ENTREGA VISUAL: Frente e costas. REFINAMENTO CRÍTICO: Não militar moderno. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-012 — Traje guarda-florestal

**Arquivo sugerido:** `COE-ADV-012_traje_guarda_florestal_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Corpo inteiro e capa separada.  
**Revisão específica:** Material plausível para Auren.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-012: Traje guarda-florestal. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Cores locais, bracelete de patrulha e proteção de caminhada. ENTREGA VISUAL: Corpo inteiro e capa separada. REFINAMENTO CRÍTICO: Material plausível para Auren. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-013 — Armadura leve Auren

**Arquivo sugerido:** `COE-ADV-013_armadura_leve_auren_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Três vistas.  
**Revisão específica:** Leitura de função e conforto.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-013: Armadura leve Auren. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Jaqueta reforçada, luvas e placas locais discretas. ENTREGA VISUAL: Três vistas. REFINAMENTO CRÍTICO: Leitura de função e conforto. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-014 — Armadura média Eldoria

**Arquivo sugerido:** `COE-ADV-014_armadura_media_eldoria_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Frente, costas e vista explodida.  
**Revisão específica:** Cobertura coerente, sem pontas exageradas.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-014: Armadura média Eldoria. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Proteções segmentadas para tronco, coxas e braços, heráldica original. ENTREGA VISUAL: Frente, costas e vista explodida. REFINAMENTO CRÍTICO: Cobertura coerente, sem pontas exageradas. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-015 — Armadura guarda regional

**Arquivo sugerido:** `COE-ADV-015_armadura_guarda_regional_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Frente e costas.  
**Revisão específica:** Visual reconhecível sem grandiosidade.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-015: Armadura guarda regional. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Peitoral discreto, capa curta e suporte de arma. ENTREGA VISUAL: Frente e costas. REFINAMENTO CRÍTICO: Visual reconhecível sem grandiosidade. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-016 — Armadura cerimonial

**Arquivo sugerido:** `COE-ADV-016_armadura_cerimonial_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Frente e detalhe do símbolo.  
**Revisão específica:** Não confundir com equipamento de batalha.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-016: Armadura cerimonial. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Traje não obrigatório para combate, detalhes de autoridade e tecido trabalhado. ENTREGA VISUAL: Frente e detalhe do símbolo. REFINAMENTO CRÍTICO: Não confundir com equipamento de batalha. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-017 — Acessórios de couro

**Arquivo sugerido:** `COE-ADV-017_acessorios_de_couro_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Peças soltas e montadas.  
**Revisão específica:** Costura e espessura corretas.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-017: Acessórios de couro. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Braceletes, faixas, bainhas, fechos e joelheiras. ENTREGA VISUAL: Peças soltas e montadas. REFINAMENTO CRÍTICO: Costura e espessura corretas. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-018 — Capas aventureiras

**Arquivo sugerido:** `COE-ADV-018_capas_aventureiras_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Frente e costas em mesmo corpo.  
**Revisão específica:** Animação e clipping.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-018: Capas aventureiras. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Quatro capas por clima e especialidade. ENTREGA VISUAL: Frente e costas em mesmo corpo. REFINAMENTO CRÍTICO: Animação e clipping. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-019 — Mantos de magia

**Arquivo sugerido:** `COE-ADV-019_mantos_de_magia_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Três vistas e bordado.  
**Revisão específica:** Diferenciar de trajes religiosos.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-019: Mantos de magia. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Quatro mantos com linguagem geométrica da Trama, sem relevo exagerado. ENTREGA VISUAL: Três vistas e bordado. REFINAMENTO CRÍTICO: Diferenciar de trajes religiosos. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-020 — Cintos e bolsas de aventura

**Arquivo sugerido:** `COE-ADV-020_cintos_e_bolsas_de_aventura_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Desenho montado e isolados.  
**Revisão específica:** Acesso pelas mãos.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-020: Cintos e bolsas de aventura. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Conjunto modular de poções, bolso de mapas, bainhas e fixadores. ENTREGA VISUAL: Desenho montado e isolados. REFINAMENTO CRÍTICO: Acesso pelas mãos. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-021 — Botas aventureiras

**Arquivo sugerido:** `COE-ADV-021_botas_aventureiras_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Três ângulos e materiais.  
**Revisão específica:** Não adotar sola contemporânea.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-021: Botas aventureiras. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Solas para floresta, rocha e estrada, versões leve e média. ENTREGA VISUAL: Três ângulos e materiais. REFINAMENTO CRÍTICO: Não adotar sola contemporânea. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ADV-022 — Luvas de combate e magia

**Arquivo sugerido:** `COE-ADV-022_luvas_de_combate_e_magia_concept_v01.png`  
**Categoria:** equipamento e roupa de aventura preparado para animação  
**Saída exigida:** Close das mãos e peças.  
**Revisão específica:** Sem dedos extras.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ADV-022: Luvas de combate e magia. Categoria: equipamento e roupa de aventura preparado para animação. DESCRIÇÃO DIRECIONADA: Dedos livres ou cobertos, transição de tecido/couro e canalizadores discretos. ENTREGA VISUAL: Close das mãos e peças. REFINAMENTO CRÍTICO: Sem dedos extras. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```


## F. Armas e folhas técnicas

### COE-WEA-001 — Espada infantil de madeira

**Arquivo sugerido:** `COE-WEA-001_espada_infantil_de_madeira_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Frente, lateral e escala na mão infantil.  
**Revisão específica:** Não reproduzir lâmina cortante.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-001: Espada infantil de madeira. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Instrumento arredondado de treino supervisionado com cabo curto e sem fio. ENTREGA VISUAL: Frente, lateral e escala na mão infantil. REFINAMENTO CRÍTICO: Não reproduzir lâmina cortante. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-002 — Espada curta inicial

**Arquivo sugerido:** `COE-WEA-002_espada_curta_inicial_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Perfil completo e close de empunhadura.  
**Revisão específica:** Centro de massa plausível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-002: Espada curta inicial. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Arma civil de aventureiro, guarda original, aço e couro simples. ENTREGA VISUAL: Perfil completo e close de empunhadura. REFINAMENTO CRÍTICO: Centro de massa plausível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-003 — Espada longa de Eldoria

**Arquivo sugerido:** `COE-WEA-003_espada_longa_de_eldoria_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Frente, lado e bainha.  
**Revisão específica:** Sem lâmina desproporcional.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-003: Espada longa de Eldoria. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Lâmina de viagem com linguagem regional e bainha coerente. ENTREGA VISUAL: Frente, lado e bainha. REFINAMENTO CRÍTICO: Sem lâmina desproporcional. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-004 — Espada de duas mãos

**Arquivo sugerido:** `COE-WEA-004_espada_de_duas_maos_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Frente e pose de empunhadura.  
**Revisão específica:** Não impossibilitar animação.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-004: Espada de duas mãos. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Arma de treinamento avançado, cabo estendido e proporções moderadas. ENTREGA VISUAL: Frente e pose de empunhadura. REFINAMENTO CRÍTICO: Não impossibilitar animação. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-005 — Machado utilitário

**Arquivo sugerido:** `COE-WEA-005_machado_utilitario_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Lado, 3/4 e cabeça isolada.  
**Revisão específica:** Construção funcional.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-005: Machado utilitário. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Ferramenta de madeira e ferro de agricultor, uso não militar primário. ENTREGA VISUAL: Lado, 3/4 e cabeça isolada. REFINAMENTO CRÍTICO: Construção funcional. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-006 — Machado de combate

**Arquivo sugerido:** `COE-WEA-006_machado_de_combate_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Frente, lateral e empunhadura.  
**Revisão específica:** Sem peso extravagante.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-006: Machado de combate. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Geometria marcante e cabo compatível com luta, ornamentação mínima. ENTREGA VISUAL: Frente, lateral e empunhadura. REFINAMENTO CRÍTICO: Sem peso extravagante. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-007 — Lança simples

**Arquivo sugerido:** `COE-WEA-007_lanca_simples_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Vista integral e ponteira.  
**Revisão específica:** Comprimento coerente.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-007: Lança simples. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Haste de madeira tratada e ponta de ferro, emprego de patrulha. ENTREGA VISUAL: Vista integral e ponteira. REFINAMENTO CRÍTICO: Comprimento coerente. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-008 — Lança de guarda

**Arquivo sugerido:** `COE-WEA-008_lanca_de_guarda_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Frente, perfil e detalhe.  
**Revisão específica:** Diferenciar de lança simples por função.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-008: Lança de guarda. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Variante com insígnia local e ferragens discretas. ENTREGA VISUAL: Frente, perfil e detalhe. REFINAMENTO CRÍTICO: Diferenciar de lança simples por função. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-009 — Arco de caça

**Arquivo sugerido:** `COE-WEA-009_arco_de_caca_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Vista frontal, lateral e armado.  
**Revisão específica:** Tensão e empunhadura plausíveis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-009: Arco de caça. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Arco de Auren com materiais vegetais e corda visível. ENTREGA VISUAL: Vista frontal, lateral e armado. REFINAMENTO CRÍTICO: Tensão e empunhadura plausíveis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-010 — Arco de aventureiro

**Arquivo sugerido:** `COE-WEA-010_arco_de_aventureiro_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Arco isolado, detalhe de empunhadura.  
**Revisão específica:** Sem tecnologia moderna.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-010: Arco de aventureiro. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Design reforçado para viagens, aljava compatível. ENTREGA VISUAL: Arco isolado, detalhe de empunhadura. REFINAMENTO CRÍTICO: Sem tecnologia moderna. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-011 — Adaga utilitária

**Arquivo sugerido:** `COE-WEA-011_adaga_utilitaria_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Frente, lado e bainha.  
**Revisão específica:** Não excessivamente ornamental.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-011: Adaga utilitária. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Lâmina curta de uso cotidiano com bainha segura. ENTREGA VISUAL: Frente, lado e bainha. REFINAMENTO CRÍTICO: Não excessivamente ornamental. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-012 — Faca de sobrevivência

**Arquivo sugerido:** `COE-WEA-012_faca_de_sobrevivencia_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Três vistas.  
**Revisão específica:** Identidade própria, forma crível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-012: Faca de sobrevivência. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Ferramenta de trilha com cabo resistente e estojo. ENTREGA VISUAL: Três vistas. REFINAMENTO CRÍTICO: Identidade própria, forma crível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-013 — Cajado arcano inicial

**Arquivo sugerido:** `COE-WEA-013_cajado_arcano_inicial_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Vista integral e foco em close.  
**Revisão específica:** Sem partículas escondendo forma.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-013: Cajado arcano inicial. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Madeira esculpida, foco mineral pequeno e marcas geométricas da Trama. ENTREGA VISUAL: Vista integral e foco em close. REFINAMENTO CRÍTICO: Sem partículas escondendo forma. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-014 — Cajado arcano avançado

**Arquivo sugerido:** `COE-WEA-014_cajado_arcano_avancado_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Três vistas e conexão do foco.  
**Revisão específica:** Estrutura 3D producível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-014: Cajado arcano avançado. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Instrumento estabilizado, construção em materiais acessíveis e símbolo original. ENTREGA VISUAL: Três vistas e conexão do foco. REFINAMENTO CRÍTICO: Estrutura 3D producível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-015 — Grimório de aprendiz

**Arquivo sugerido:** `COE-WEA-015_grimorio_de_aprendiz_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Fechado, aberto e lateral.  
**Revisão específica:** Páginas e lombada plausíveis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-015: Grimório de aprendiz. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Livro de estudos, couro, papel e fecho, marcas sutis da Trama. ENTREGA VISUAL: Fechado, aberto e lateral. REFINAMENTO CRÍTICO: Páginas e lombada plausíveis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-016 — Grimório experiente

**Arquivo sugerido:** `COE-WEA-016_grimorio_experiente_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Aberto, fechado e detalhes.  
**Revisão específica:** Não copiar grimório de franquia.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-016: Grimório experiente. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Livro usado de campo, divisórias e desenhos arcanos sem texto legível complexo. ENTREGA VISUAL: Aberto, fechado e detalhes. REFINAMENTO CRÍTICO: Não copiar grimório de franquia. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-017 — Martelo de treino

**Arquivo sugerido:** `COE-WEA-017_martelo_de_treino_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Frente e mão em escala.  
**Revisão específica:** Peso compatível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-017: Martelo de treino. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Instrumento sem quinas perigosas de exercícios juvenis. ENTREGA VISUAL: Frente e mão em escala. REFINAMENTO CRÍTICO: Peso compatível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-018 — Escudo leve

**Arquivo sugerido:** `COE-WEA-018_escudo_leve_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Frente, lado e verso.  
**Revisão específica:** Pegada e suporte coerentes.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-018: Escudo leve. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Madeira laminada, borda metálica e correias internas. ENTREGA VISUAL: Frente, lado e verso. REFINAMENTO CRÍTICO: Pegada e suporte coerentes. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-019 — Escudo regional

**Arquivo sugerido:** `COE-WEA-019_escudo_regional_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Frente, verso e close de heráldica.  
**Revisão específica:** Não copiar brasão histórico específico.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-019: Escudo regional. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Símbolo de Eldoria original, desgaste e função de guarda. ENTREGA VISUAL: Frente, verso e close de heráldica. REFINAMENTO CRÍTICO: Não copiar brasão histórico específico. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-020 — Bainhas e suportes

**Arquivo sugerido:** `COE-WEA-020_bainhas_e_suportes_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Vista explodida e uso no corpo.  
**Revisão específica:** Ancoragem sem clipping.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-020: Bainhas e suportes. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Sistemas de transporte de espadas, adagas e ferramentas em cinto. ENTREGA VISUAL: Vista explodida e uso no corpo. REFINAMENTO CRÍTICO: Ancoragem sem clipping. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-021 — Pontas e guardas de espada

**Arquivo sugerido:** `COE-WEA-021_pontas_e_guardas_de_espada_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Close isolado e montagem.  
**Revisão específica:** Evitar peças frágeis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-021: Pontas e guardas de espada. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Biblioteca de seis soluções originais de guarda, pomo e ponta. ENTREGA VISUAL: Close isolado e montagem. REFINAMENTO CRÍTICO: Evitar peças frágeis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-022 — Cabos e empunhaduras

**Arquivo sugerido:** `COE-WEA-022_cabos_e_empunhaduras_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Close e corte esquemático sem texto.  
**Revisão específica:** Espessura proporcional.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-022: Cabos e empunhaduras. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Madeira, couro e fio em seis soluções ergonômicas. ENTREGA VISUAL: Close e corte esquemático sem texto. REFINAMENTO CRÍTICO: Espessura proporcional. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-023 — Metal comum e raro

**Arquivo sugerido:** `COE-WEA-023_metal_comum_e_raro_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Swatches sobre mesma lâmina.  
**Revisão específica:** Materiais distinguíveis sem emissivo excessivo.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-023: Metal comum e raro. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Prancha comparativa de aço, ferro, bronze e liga arcana ficcional. ENTREGA VISUAL: Swatches sobre mesma lâmina. REFINAMENTO CRÍTICO: Materiais distinguíveis sem emissivo excessivo. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-024 — Runas para armas

**Arquivo sugerido:** `COE-WEA-024_runas_para_armas_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Close em superfície plana e curva.  
**Revisão específica:** Não usar alfabeto real como escrita mística sem justificativa.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-024: Runas para armas. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Sistema geométrico original aplicado discretamente em metal e madeira. ENTREGA VISUAL: Close em superfície plana e curva. REFINAMENTO CRÍTICO: Não usar alfabeto real como escrita mística sem justificativa. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-025 — Prancha família de espadas

**Arquivo sugerido:** `COE-WEA-025_prancha_familia_de_espadas_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Comparativo lado a lado.  
**Revisão específica:** Nomes curtos opcionais, sem texto elaborado.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-025: Prancha família de espadas. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Curta, longa, duas mãos e treino em escala coerente. ENTREGA VISUAL: Comparativo lado a lado. REFINAMENTO CRÍTICO: Nomes curtos opcionais, sem texto elaborado. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-026 — Prancha família de lanças

**Arquivo sugerido:** `COE-WEA-026_prancha_familia_de_lancas_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Vista integral e detalhes.  
**Revisão específica:** Escala consistente.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-026: Prancha família de lanças. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Três ponteiras e duas hastes de uso marcial/regional. ENTREGA VISUAL: Vista integral e detalhes. REFINAMENTO CRÍTICO: Escala consistente. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-027 — Prancha família de arcos

**Arquivo sugerido:** `COE-WEA-027_prancha_familia_de_arcos_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Vista frontal e perfil.  
**Revisão específica:** Cordas e flechas realistas.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-027: Prancha família de arcos. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Caça, aventura e guarda com aljavas. ENTREGA VISUAL: Vista frontal e perfil. REFINAMENTO CRÍTICO: Cordas e flechas realistas. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-WEA-028 — Prancha escudos

**Arquivo sugerido:** `COE-WEA-028_prancha_escudos_concept_v01.png`  
**Categoria:** arma ou folha de arma como referência 3D, proporções funcionais  
**Saída exigida:** Três pares de vistas.  
**Revisão específica:** Diferenciar defesa real e ornamento.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-WEA-028: Prancha escudos. Categoria: arma ou folha de arma como referência 3D, proporções funcionais. DESCRIÇÃO DIRECIONADA: Leve, regional e cerimonial, frente e verso. ENTREGA VISUAL: Três pares de vistas. REFINAMENTO CRÍTICO: Diferenciar defesa real e ornamento. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```


## G. Ferramentas, objetos, itens e economia

### COE-PRP-001 — Ferramentas de ferreiro

**Arquivo sugerido:** `COE-PRP-001_ferramentas_de_ferreiro_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Peças isoladas e oficina em mini layout.  
**Revisão específica:** Escala e pegada plausíveis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-001: Ferramentas de ferreiro. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Tenazes, martelos, bigorna pequena e suporte, metal queimado e madeira. ENTREGA VISUAL: Peças isoladas e oficina em mini layout. REFINAMENTO CRÍTICO: Escala e pegada plausíveis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-002 — Ferramentas de carpinteiro

**Arquivo sugerido:** `COE-PRP-002_ferramentas_de_carpinteiro_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Folha de objetos e detalhes.  
**Revisão específica:** Sem ferramentas industriais.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-002: Ferramentas de carpinteiro. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Serra manual, formões, esquadro e malhete em padrão local. ENTREGA VISUAL: Folha de objetos e detalhes. REFINAMENTO CRÍTICO: Sem ferramentas industriais. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-003 — Ferramentas de agricultor

**Arquivo sugerido:** `COE-PRP-003_ferramentas_de_agricultor_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Objetos separados e escala humana.  
**Revisão específica:** Materiais e desgaste de uso.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-003: Ferramentas de agricultor. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Enxada, ancinho, pá, foice de trabalho e cesto. ENTREGA VISUAL: Objetos separados e escala humana. REFINAMENTO CRÍTICO: Materiais e desgaste de uso. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-004 — Kit de herbalista

**Arquivo sugerido:** `COE-PRP-004_kit_de_herbalista_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Aberto, fechado e materiais.  
**Revisão específica:** Itens de trabalho identificáveis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-004: Kit de herbalista. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Estojo de amostras, facas de coleta, saquinhos e pinças. ENTREGA VISUAL: Aberto, fechado e materiais. REFINAMENTO CRÍTICO: Itens de trabalho identificáveis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-005 — Kit viajante

**Arquivo sugerido:** `COE-PRP-005_kit_viajante_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Vista explodida e equipado.  
**Revisão específica:** Peso carregável.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-005: Kit viajante. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Mochila, rolo de manta, cantil, corda e estojo de mapas. ENTREGA VISUAL: Vista explodida e equipado. REFINAMENTO CRÍTICO: Peso carregável. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-006 — Kit exploração infantil

**Arquivo sugerido:** `COE-PRP-006_kit_exploracao_infantil_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Itens isolados, escala infantil.  
**Revisão específica:** Sem equipamento adulto perigoso.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-006: Kit exploração infantil. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Lupa simples opcional, caderno, bolsa, lanterna protegida e barbante. ENTREGA VISUAL: Itens isolados, escala infantil. REFINAMENTO CRÍTICO: Sem equipamento adulto perigoso. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-007 — Cestas e baldes

**Arquivo sugerido:** `COE-PRP-007_cestas_e_baldes_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Três quartos e vistas de cima.  
**Revisão específica:** Alças funcionais.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-007: Cestas e baldes. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Seis variações de vime, madeira e corda para missões cotidianas. ENTREGA VISUAL: Três quartos e vistas de cima. REFINAMENTO CRÍTICO: Alças funcionais. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-008 — Caixas e barris

**Arquivo sugerido:** `COE-PRP-008_caixas_e_barris_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Grade em escala coerente.  
**Revisão específica:** Produção reutilizável.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-008: Caixas e barris. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Conjunto modular de transporte e armazenamento, madeira e ferro. ENTREGA VISUAL: Grade em escala coerente. REFINAMENTO CRÍTICO: Produção reutilizável. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-009 — Sacos e cordas

**Arquivo sugerido:** `COE-PRP-009_sacos_e_cordas_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Close de textura e conjunto.  
**Revisão específica:** Sem microdetalhes caros.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-009: Sacos e cordas. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Grãos, sementes, lã, nós, laços e suportes. ENTREGA VISUAL: Close de textura e conjunto. REFINAMENTO CRÍTICO: Sem microdetalhes caros. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-010 — Poções e frascos

**Arquivo sugerido:** `COE-PRP-010_pocoes_e_frascos_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Frente e 3/4 de seis frascos.  
**Revisão específica:** Evitar ícones de marcas conhecidas.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-010: Poções e frascos. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Kit de vidro/cerâmica com selos, líquido legível sem interface. ENTREGA VISUAL: Frente e 3/4 de seis frascos. REFINAMENTO CRÍTICO: Evitar ícones de marcas conhecidas. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-011 — Ervas medicinais

**Arquivo sugerido:** `COE-PRP-011_ervas_medicinais_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Planta completa e partes.  
**Revisão específica:** Coerência botânica estilizada.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-011: Ervas medicinais. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Seis plantas fictícias com folhas, flores e raízes diferenciáveis. ENTREGA VISUAL: Planta completa e partes. REFINAMENTO CRÍTICO: Coerência botânica estilizada. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-012 — Ingredientes alquímicos

**Arquivo sugerido:** `COE-PRP-012_ingredientes_alquimicos_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Dez pequenos objetos isolados.  
**Revisão específica:** Materiais distinguíveis por forma.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-012: Ingredientes alquímicos. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Resinas, minerais e extratos de Eryndor em recipientes funcionais. ENTREGA VISUAL: Dez pequenos objetos isolados. REFINAMENTO CRÍTICO: Materiais distinguíveis por forma. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-013 — Alimentos de Auren

**Arquivo sugerido:** `COE-PRP-013_alimentos_de_auren_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Mesa e itens isolados.  
**Revisão específica:** Sem produtos anacrônicos.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-013: Alimentos de Auren. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Pães, vegetais, frutas, sopa, queijo e conserva locais. ENTREGA VISUAL: Mesa e itens isolados. REFINAMENTO CRÍTICO: Sem produtos anacrônicos. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-014 — Utensílios de cozinha

**Arquivo sugerido:** `COE-PRP-014_utensilios_de_cozinha_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Grupo e peças separadas.  
**Revisão específica:** Consistência do acabamento.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-014: Utensílios de cozinha. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Panelas, pratos, tigelas, talheres e fogareiro de casa rural. ENTREGA VISUAL: Grupo e peças separadas. REFINAMENTO CRÍTICO: Consistência do acabamento. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-015 — Livros e cadernos

**Arquivo sugerido:** `COE-PRP-015_livros_e_cadernos_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Fechados e abertos.  
**Revisão específica:** Evitar texto pequeno ilegível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-015: Livros e cadernos. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Material escolar simples e registro comercial com capas distintas. ENTREGA VISUAL: Fechados e abertos. REFINAMENTO CRÍTICO: Evitar texto pequeno ilegível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-016 — Pergaminhos e mapas

**Arquivo sugerido:** `COE-PRP-016_pergaminhos_e_mapas_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Aberto, enrolado e detalhe.  
**Revisão específica:** Símbolos originais e legibilidade.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-016: Pergaminhos e mapas. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Documentos de Auren e antigos vestígios da Trama, selos próprios. ENTREGA VISUAL: Aberto, enrolado e detalhe. REFINAMENTO CRÍTICO: Símbolos originais e legibilidade. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-017 — Lanternas e velas

**Arquivo sugerido:** `COE-PRP-017_lanternas_e_velas_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Isoladas e em ambiente escuro.  
**Revisão específica:** Fonte de luz tecnicamente viável.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-017: Lanternas e velas. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Quatro luminárias de casa, viagem, oficina e templo. ENTREGA VISUAL: Isoladas e em ambiente escuro. REFINAMENTO CRÍTICO: Fonte de luz tecnicamente viável. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-018 — Mobília rural

**Arquivo sugerido:** `COE-PRP-018_mobilia_rural_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Isolados e planta de disposição.  
**Revisão específica:** Escalas adultas/infantis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-018: Mobília rural. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Cama simples, baú, armário, mesa e cadeiras modulares. ENTREGA VISUAL: Isolados e planta de disposição. REFINAMENTO CRÍTICO: Escalas adultas/infantis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-019 — Mobília de ferraria

**Arquivo sugerido:** `COE-PRP-019_mobilia_de_ferraria_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Isolados e aplicados.  
**Revisão específica:** Fluxo de trabalho viável.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-019: Mobília de ferraria. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Bancada, prateleiras e suporte de ferramentas. ENTREGA VISUAL: Isolados e aplicados. REFINAMENTO CRÍTICO: Fluxo de trabalho viável. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-020 — Mobília escolar

**Arquivo sugerido:** `COE-PRP-020_mobilia_escolar_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Isolados e conjunto.  
**Revisão específica:** Não escola moderna.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-020: Mobília escolar. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Mesas, banco, quadro simples e estantes. ENTREGA VISUAL: Isolados e conjunto. REFINAMENTO CRÍTICO: Não escola moderna. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-021 — Taverna: props

**Arquivo sugerido:** `COE-PRP-021_taverna_props_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Folha de props.  
**Revisão específica:** Reutilização em outras vilas.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-021: Taverna: props. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Canecas, barris, mesa, balcão e placas originais. ENTREGA VISUAL: Folha de props. REFINAMENTO CRÍTICO: Reutilização em outras vilas. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-022 — Brinquedos infantis

**Arquivo sugerido:** `COE-PRP-022_brinquedos_infantis_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Seis itens isolados.  
**Revisão específica:** Materiais seguros e idade adequada.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-022: Brinquedos infantis. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Bonecos de pano, piões, cordas e peças de madeira. ENTREGA VISUAL: Seis itens isolados. REFINAMENTO CRÍTICO: Materiais seguros e idade adequada. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-023 — Joias e amuletos

**Arquivo sugerido:** `COE-PRP-023_joias_e_amuletos_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Close e escala em mão.  
**Revisão específica:** Não confundir valor monetário com ascensão.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-023: Joias e amuletos. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Broches, anéis e pingentes civis, diferentes estratos sociais. ENTREGA VISUAL: Close e escala em mão. REFINAMENTO CRÍTICO: Não confundir valor monetário com ascensão. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-024 — Talismãs da Trama

**Arquivo sugerido:** `COE-PRP-024_talismas_da_trama_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Close, verso e conexão.  
**Revisão específica:** Sem similaridade com símbolos de franquia.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-024: Talismãs da Trama. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Quatro pequenos focos com geometrias de fios e nós, efeitos discretos. ENTREGA VISUAL: Close, verso e conexão. REFINAMENTO CRÍTICO: Sem similaridade com símbolos de franquia. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-025 — Relíquias de Elnor

**Arquivo sugerido:** `COE-PRP-025_reliquias_de_elnor_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Frente e detalhe de fratura.  
**Revisão específica:** Mistério sem narrar origem como fato visual.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-025: Relíquias de Elnor. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Três objetos antigos ligados à Primeira Fratura, material desgastado e padrões incompletos. ENTREGA VISUAL: Frente e detalhe de fratura. REFINAMENTO CRÍTICO: Mistério sem narrar origem como fato visual. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-026 — Moedas de Eldoria

**Arquivo sugerido:** `COE-PRP-026_moedas_de_eldoria_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Face, verso e espessura.  
**Revisão específica:** Sem moedas reais copiadas.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-026: Moedas de Eldoria. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Sistema de moedas de cobre, prata e ouro com heráldica original. ENTREGA VISUAL: Face, verso e espessura. REFINAMENTO CRÍTICO: Sem moedas reais copiadas. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-027 — Livro de contas de Oren

**Arquivo sugerido:** `COE-PRP-027_livro_de_contas_de_oren_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Fechado, aberto e detalhes.  
**Revisão específica:** Evitar números/texto inventados legíveis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-027: Livro de contas de Oren. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Registro comercial com lombada, fecho e compartimento de recibos. ENTREGA VISUAL: Fechado, aberto e detalhes. REFINAMENTO CRÍTICO: Evitar números/texto inventados legíveis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-028 — Símbolo familiar adaptável

**Arquivo sugerido:** `COE-PRP-028_simbolo_familiar_adaptavel_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Conjunto agrícola, artesão e guardião.  
**Revisão específica:** Permitir variação modular barata.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-028: Símbolo familiar adaptável. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Três pequenos objetos domésticos por origem, mesmo espaço de residência. ENTREGA VISUAL: Conjunto agrícola, artesão e guardião. REFINAMENTO CRÍTICO: Permitir variação modular barata. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-PRP-029 — Objeto da missão Cesto Perdido

**Arquivo sugerido:** `COE-PRP-029_objeto_da_missao_cesto_perdido_concept_v01.png`  
**Categoria:** prop de cenário/inventário reutilizável e modelável  
**Saída exigida:** Frente, interior, perfil e no chão.  
**Revisão específica:** Silhueta fácil para busca na floresta.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-PRP-029: Objeto da missão Cesto Perdido. Categoria: prop de cenário/inventário reutilizável e modelável. DESCRIÇÃO DIRECIONADA: Cesto único reconhecível com trama de vime e tecido identificador. ENTREGA VISUAL: Frente, interior, perfil e no chão. REFINAMENTO CRÍTICO: Silhueta fácil para busca na floresta. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```


## H. Arquitetura, edifícios e modularidade

### COE-ARC-001 — Casa família agricultora exterior

**Arquivo sugerido:** `COE-ARC-001_casa_familia_agricultora_exterior_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Fachada, laterais e 3/4.  
**Revisão específica:** Não tornar origem sempre miserável.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-001: Casa família agricultora exterior. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Pequena residência rural de pedra, madeira e reboco, horta e reparos discretos. ENTREGA VISUAL: Fachada, laterais e 3/4. REFINAMENTO CRÍTICO: Não tornar origem sempre miserável. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-002 — Casa família artesã exterior

**Arquivo sugerido:** `COE-ARC-002_casa_familia_artesa_exterior_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Fachada, costas e planta simplificada.  
**Revisão específica:** Reutilizar kit residencial.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-002: Casa família artesã exterior. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Mesma base arquitetônica modular com anexo de trabalho e vitrine simples. ENTREGA VISUAL: Fachada, costas e planta simplificada. REFINAMENTO CRÍTICO: Reutilizar kit residencial. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-003 — Casa família guardiã exterior

**Arquivo sugerido:** `COE-ARC-003_casa_familia_guardia_exterior_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Fachada, 3/4 e quintal.  
**Revisão específica:** Evitar fortaleza doméstica.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-003: Casa família guardiã exterior. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Base modular com espaço de treino e depósito seguro. ENTREGA VISUAL: Fachada, 3/4 e quintal. REFINAMENTO CRÍTICO: Evitar fortaleza doméstica. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-004 — Casa agricultora interior

**Arquivo sugerido:** `COE-ARC-004_casa_agricultora_interior_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Perspectiva ampla e planta de mobiliário.  
**Revisão específica:** Circulação da câmera e criança.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-004: Casa agricultora interior. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Cozinha, dormitório, lareira e objetos de lavoura em espaço compacto. ENTREGA VISUAL: Perspectiva ampla e planta de mobiliário. REFINAMENTO CRÍTICO: Circulação da câmera e criança. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-005 — Casa artesã interior

**Arquivo sugerido:** `COE-ARC-005_casa_artesa_interior_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Duas perspectivas e objetos.  
**Revisão específica:** Não criar fábrica.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-005: Casa artesã interior. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Ambiente compartilhado com ferramentas guardadas e área de família. ENTREGA VISUAL: Duas perspectivas e objetos. REFINAMENTO CRÍTICO: Não criar fábrica. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-006 — Casa guardiã interior

**Arquivo sugerido:** `COE-ARC-006_casa_guardia_interior_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Perspectiva e planta.  
**Revisão específica:** Lar antes de quartel.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-006: Casa guardiã interior. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Objetos de patrulha, mapas locais e ambiente acolhedor. ENTREGA VISUAL: Perspectiva e planta. REFINAMENTO CRÍTICO: Lar antes de quartel. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-007 — Casa de Borin

**Arquivo sugerido:** `COE-ARC-007_casa_de_borin_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Fachada, lateral e 3/4.  
**Revisão específica:** Construção em escala de vila.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-007: Casa de Borin. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Fachada conectada à oficina e sinais de longa carreira. ENTREGA VISUAL: Fachada, lateral e 3/4. REFINAMENTO CRÍTICO: Construção em escala de vila. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-008 — Ferraria exterior

**Arquivo sugerido:** `COE-ARC-008_ferraria_exterior_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Três quartos, frente e planta simples.  
**Revisão específica:** Fluxo e ventilação viáveis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-008: Ferraria exterior. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Forja coberta, chaminé, área de água e entrada de material. ENTREGA VISUAL: Três quartos, frente e planta simples. REFINAMENTO CRÍTICO: Fluxo e ventilação viáveis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-009 — Ferraria interior

**Arquivo sugerido:** `COE-ARC-009_ferraria_interior_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Grande angular controlada e props isolados.  
**Revisão específica:** Nenhuma geometria bloquear mãos.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-009: Ferraria interior. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Forno, bigorna, armazenamento e bancada com espaços para animação. ENTREGA VISUAL: Grande angular controlada e props isolados. REFINAMENTO CRÍTICO: Nenhuma geometria bloquear mãos. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-010 — Casa de Lysa

**Arquivo sugerido:** `COE-ARC-010_casa_de_lysa_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Fachada, quintal e três quartos.  
**Revisão específica:** Botanismo sem fantasia exagerada.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-010: Casa de Lysa. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Residência com jardim medicinal, secagem de plantas e madeira natural. ENTREGA VISUAL: Fachada, quintal e três quartos. REFINAMENTO CRÍTICO: Botanismo sem fantasia exagerada. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-011 — Laboratório herbal

**Arquivo sugerido:** `COE-ARC-011_laboratorio_herbal_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Perspectiva, planta e objetos.  
**Revisão específica:** Espaço para interações.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-011: Laboratório herbal. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Bancada, armários de amostras, luz natural e secagem pendurada. ENTREGA VISUAL: Perspectiva, planta e objetos. REFINAMENTO CRÍTICO: Espaço para interações. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-012 — Casa de Eira

**Arquivo sugerido:** `COE-ARC-012_casa_de_eira_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Frente e 3/4.  
**Revisão específica:** Visual residencial modesto.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-012: Casa de Eira. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Fachada de professora com estante visível e pequeno alpendre. ENTREGA VISUAL: Frente e 3/4. REFINAMENTO CRÍTICO: Visual residencial modesto. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-013 — Sala de estudo

**Arquivo sugerido:** `COE-ARC-013_sala_de_estudo_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Perspectiva e planta de acesso.  
**Revisão específica:** Evitar biblioteca monumental.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-013: Sala de estudo. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Pequena sala comunitária com livros, banco e materiais de escrita. ENTREGA VISUAL: Perspectiva e planta de acesso. REFINAMENTO CRÍTICO: Evitar biblioteca monumental. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-014 — Casa de Oren

**Arquivo sugerido:** `COE-ARC-014_casa_de_oren_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Fachada, traseira e planta.  
**Revisão específica:** Escala local.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-014: Casa de Oren. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Comércio integrado à residência, área de depósito e balcão simples. ENTREGA VISUAL: Fachada, traseira e planta. REFINAMENTO CRÍTICO: Escala local. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-015 — Sede de Maelis

**Arquivo sugerido:** `COE-ARC-015_sede_de_maelis_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Fachada e interior.  
**Revisão específica:** Não castelo.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-015: Sede de Maelis. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Pequeno edifício administrativo com aviso público e sala de reunião. ENTREGA VISUAL: Fachada e interior. REFINAMENTO CRÍTICO: Não castelo. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-016 — Praça de Auren

**Arquivo sugerido:** `COE-ARC-016_praca_de_auren_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Vista elevada 3/4 e nível de personagem.  
**Revisão específica:** Jogabilidade e navegação claras.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-016: Praça de Auren. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Praça com fonte/poço, comércio sazonal, conexões de ruas e circulação de NPCs. ENTREGA VISUAL: Vista elevada 3/4 e nível de personagem. REFINAMENTO CRÍTICO: Jogabilidade e navegação claras. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-017 — Poço central

**Arquivo sugerido:** `COE-ARC-017_poco_central_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Frente, lateral e close de guincho.  
**Revisão específica:** Proporção e uso seguros.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-017: Poço central. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Poço de pedra com cobertura de madeira e balde operável. ENTREGA VISUAL: Frente, lateral e close de guincho. REFINAMENTO CRÍTICO: Proporção e uso seguros. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-018 — Mercado pequeno

**Arquivo sugerido:** `COE-ARC-018_mercado_pequeno_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Cena e kit isolado.  
**Revisão específica:** Não feira gigante.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-018: Mercado pequeno. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Bancas desmontáveis, toldos, produtos e corredores de circulação. ENTREGA VISUAL: Cena e kit isolado. REFINAMENTO CRÍTICO: Não feira gigante. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-019 — Celeiro

**Arquivo sugerido:** `COE-ARC-019_celeiro_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Exterior, interior e vistas.  
**Revisão específica:** Escala de produção da vila.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-019: Celeiro. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Estrutura de armazenamento com acesso plausível e materiais de lavoura. ENTREGA VISUAL: Exterior, interior e vistas. REFINAMENTO CRÍTICO: Escala de produção da vila. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-020 — Estábulo

**Arquivo sugerido:** `COE-ARC-020_estabulo_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Exterior/interior.  
**Revisão específica:** Compatível com cavalos e animais.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-020: Estábulo. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Baias, bebedouro, palheiro e entrada de carroça pequena. ENTREGA VISUAL: Exterior/interior. REFINAMENTO CRÍTICO: Compatível com cavalos e animais. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-021 — Cercas e portões

**Arquivo sugerido:** `COE-ARC-021_cercas_e_portoes_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Peças isoladas e trecho montado.  
**Revisão específica:** Snap modular e colisões simples.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-021: Cercas e portões. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Kit de madeira rural com portão, canto, peça reta e desgastes. ENTREGA VISUAL: Peças isoladas e trecho montado. REFINAMENTO CRÍTICO: Snap modular e colisões simples. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-022 — Caminhos de terra e pedra

**Arquivo sugerido:** `COE-ARC-022_caminhos_de_terra_e_pedra_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Tiles modulares e exemplo montado.  
**Revisão específica:** Evitar repetição de textura.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-022: Caminhos de terra e pedra. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Pavimentos de Auren, bordas naturais, cruzamentos e variação de desgaste. ENTREGA VISUAL: Tiles modulares e exemplo montado. REFINAMENTO CRÍTICO: Evitar repetição de textura. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-023 — Ponte pequena

**Arquivo sugerido:** `COE-ARC-023_ponte_pequena_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Vista lateral, superior e 3/4.  
**Revisão específica:** Navegação de personagens e câmera.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-023: Ponte pequena. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Ponte de madeira/pedra sobre riacho, guardas discretas. ENTREGA VISUAL: Vista lateral, superior e 3/4. REFINAMENTO CRÍTICO: Navegação de personagens e câmera. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-024 — Placas e sinalização

**Arquivo sugerido:** `COE-ARC-024_placas_e_sinalizacao_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Peças isoladas e colocadas.  
**Revisão específica:** Sem texto ilegível como única informação.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-024: Placas e sinalização. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Avisos da praça, placa da ferraria e orientação de trilhas com iconografia original. ENTREGA VISUAL: Peças isoladas e colocadas. REFINAMENTO CRÍTICO: Sem texto ilegível como única informação. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-025 — Posto de vigilância

**Arquivo sugerido:** `COE-ARC-025_posto_de_vigilancia_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Frente, perfil e vista de cima.  
**Revisão específica:** Não fortificação de capital.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-025: Posto de vigilância. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Torre baixa com plataforma e abrigo simples em estrada regional. ENTREGA VISUAL: Frente, perfil e vista de cima. REFINAMENTO CRÍTICO: Não fortificação de capital. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-026 — Kit parede de pedra e reboco

**Arquivo sugerido:** `COE-ARC-026_kit_parede_de_pedra_e_reboco_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Folha modular com encaixes visuais.  
**Revisão específica:** Mesma unidade de escala.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-026: Kit parede de pedra e reboco. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Módulos reto, canto, janela, porta, desgaste e versões internas. ENTREGA VISUAL: Folha modular com encaixes visuais. REFINAMENTO CRÍTICO: Mesma unidade de escala. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-027 — Kit parede de madeira

**Arquivo sugerido:** `COE-ARC-027_kit_parede_de_madeira_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Módulos e construção exemplo.  
**Revisão específica:** Encaixes coerentes.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-027: Kit parede de madeira. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Estruturas leves e vigas para casas e oficinas. ENTREGA VISUAL: Módulos e construção exemplo. REFINAMENTO CRÍTICO: Encaixes coerentes. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-028 — Kit telhados

**Arquivo sugerido:** `COE-ARC-028_kit_telhados_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Visão superior e inclinação.  
**Revisão específica:** Não coberturas impossíveis de repetir.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-028: Kit telhados. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Peças reta, cumeeira, canto, borda e claraboia simples. ENTREGA VISUAL: Visão superior e inclinação. REFINAMENTO CRÍTICO: Não coberturas impossíveis de repetir. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-029 — Kit portas

**Arquivo sugerido:** `COE-ARC-029_kit_portas_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Frente, verso e lateral.  
**Revisão específica:** Tamanho infantil e adulto.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-029: Kit portas. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Portas rurais, oficina e instituição, dobradiças e puxadores. ENTREGA VISUAL: Frente, verso e lateral. REFINAMENTO CRÍTICO: Tamanho infantil e adulto. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-030 — Kit janelas

**Arquivo sugerido:** `COE-ARC-030_kit_janelas_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Frente, abertura e seção.  
**Revisão específica:** Consistência material.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-030: Kit janelas. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Variações com veneziana, vidro simples e estrutura de madeira. ENTREGA VISUAL: Frente, abertura e seção. REFINAMENTO CRÍTICO: Consistência material. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-031 — Kit vigas e pilares

**Arquivo sugerido:** `COE-ARC-031_kit_vigas_e_pilares_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Explodido e aplicação.  
**Revisão específica:** Lógica estrutural.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-031: Kit vigas e pilares. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Suportes retos, diagonais e cantos de casas de Auren. ENTREGA VISUAL: Explodido e aplicação. REFINAMENTO CRÍTICO: Lógica estrutural. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-032 — Kit chaminés

**Arquivo sugerido:** `COE-ARC-032_kit_chamines_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Frente e encaixe no teto.  
**Revisão específica:** Funcionamento visual.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-032: Kit chaminés. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Pedra e tijolos com variantes de altura e saída de fumaça. ENTREGA VISUAL: Frente e encaixe no teto. REFINAMENTO CRÍTICO: Funcionamento visual. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-033 — Kit fachada

**Arquivo sugerido:** `COE-ARC-033_kit_fachada_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Seis fachadas lado a lado.  
**Revisão específica:** Provar reutilização.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-033: Kit fachada. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Combinações de cores/decoração das mesmas peças de casa. ENTREGA VISUAL: Seis fachadas lado a lado. REFINAMENTO CRÍTICO: Provar reutilização. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-034 — Kit quintal

**Arquivo sugerido:** `COE-ARC-034_kit_quintal_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Grupo e componentes.  
**Revisão específica:** Set dressing escalável.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-034: Kit quintal. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Horta, tanque, bancos, corda de roupa e depósito pequeno. ENTREGA VISUAL: Grupo e componentes. REFINAMENTO CRÍTICO: Set dressing escalável. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-035 — Kit ruína de Elnor

**Arquivo sugerido:** `COE-ARC-035_kit_ruina_de_elnor_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Peças isoladas e montagem.  
**Revisão específica:** Antigo sem se parecer com ruína famosa.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-035: Kit ruína de Elnor. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Módulos de parede antiga, arco, piso quebrado e padrão da Trama. ENTREGA VISUAL: Peças isoladas e montagem. REFINAMENTO CRÍTICO: Antigo sem se parecer com ruína famosa. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-ARC-036 — Sala de treinamento

**Arquivo sugerido:** `COE-ARC-036_sala_de_treinamento_concept_v01.png`  
**Categoria:** arquitetura/kit modular com escala humana e lógica construtiva  
**Saída exigida:** Perspectiva e planta.  
**Revisão específica:** Adequado a treinamento supervisionado.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-ARC-036: Sala de treinamento. Categoria: arquitetura/kit modular com escala humana e lógica construtiva. DESCRIÇÃO DIRECIONADA: Espaço modesto com boneco de treino, cerca e suportes de espada de madeira. ENTREGA VISUAL: Perspectiva e planta. REFINAMENTO CRÍTICO: Adequado a treinamento supervisionado. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```


## I. Terreno, árvores, vegetação e biomas

### COE-NAT-001 — Árvore comum Auren A

**Arquivo sugerido:** `COE-NAT-001_arvore_comum_auren_a_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Inteira, folhas, tronco e silhueta.  
**Revisão específica:** Variação para LOD.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-001: Árvore comum Auren A. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Árvore caducifólia estilizada de copa legível e tronco modelável. ENTREGA VISUAL: Inteira, folhas, tronco e silhueta. REFINAMENTO CRÍTICO: Variação para LOD. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-002 — Árvore comum Auren B

**Arquivo sugerido:** `COE-NAT-002_arvore_comum_auren_b_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Inteira, perto e sombra.  
**Revisão específica:** Não apenas recolorir A.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-002: Árvore comum Auren B. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Outra espécie de vila com casca e copa claramente diferentes. ENTREGA VISUAL: Inteira, perto e sombra. REFINAMENTO CRÍTICO: Não apenas recolorir A. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-003 — Árvore antiga do Bosque

**Arquivo sugerido:** `COE-NAT-003_arvore_antiga_do_bosque_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Frente, lateral e detalhes de raiz.  
**Revisão específica:** Mistério sutil sem rosto humano.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-003: Árvore antiga do Bosque. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Tronco retorcido, raízes largas e musgo de longa idade. ENTREGA VISUAL: Frente, lateral e detalhes de raiz. REFINAMENTO CRÍTICO: Mistério sutil sem rosto humano. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-004 — Árvore especial da Trama

**Arquivo sugerido:** `COE-NAT-004_arvore_especial_da_trama_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Dia/noite e raízes.  
**Revisão específica:** Não copiar árvore sagrada conhecida.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-004: Árvore especial da Trama. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Árvore de aparência natural com filamentos geométricos luminosos discretos. ENTREGA VISUAL: Dia/noite e raízes. REFINAMENTO CRÍTICO: Não copiar árvore sagrada conhecida. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-005 — Arbustos baixos

**Arquivo sugerido:** `COE-NAT-005_arbustos_baixos_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Isolados e amostra de agrupamento.  
**Revisão específica:** LOD e silhueta.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-005: Arbustos baixos. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Seis volumes vegetais para caminhos e quintais. ENTREGA VISUAL: Isolados e amostra de agrupamento. REFINAMENTO CRÍTICO: LOD e silhueta. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-006 — Vegetação de chão

**Arquivo sugerido:** `COE-NAT-006_vegetacao_de_chao_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Folha modular e trecho.  
**Revisão específica:** Não saturar cenário.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-006: Vegetação de chão. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Grama, trevos, folhas e brotos de borda de trilha. ENTREGA VISUAL: Folha modular e trecho. REFINAMENTO CRÍTICO: Não saturar cenário. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-007 — Flores silvestres

**Arquivo sugerido:** `COE-NAT-007_flores_silvestres_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Planta isolada e conjunto.  
**Revisão específica:** Botânica coerente.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-007: Flores silvestres. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Seis espécies locais com formas e paletas distintas. ENTREGA VISUAL: Planta isolada e conjunto. REFINAMENTO CRÍTICO: Botânica coerente. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-008 — Ervas coletáveis

**Arquivo sugerido:** `COE-NAT-008_ervas_coletaveis_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Frente, perfil e escala de mão.  
**Revisão específica:** Identificáveis sem HUD.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-008: Ervas coletáveis. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Plantas de missão com formato distinto da vegetação decorativa. ENTREGA VISUAL: Frente, perfil e escala de mão. REFINAMENTO CRÍTICO: Identificáveis sem HUD. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-009 — Cogumelos

**Arquivo sugerido:** `COE-NAT-009_cogumelos_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Grupos e detalhes.  
**Revisão específica:** Evitar semelhança literal com jogo famoso.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-009: Cogumelos. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Quatro formas comestíveis/ficcionais sem alegações reais de segurança. ENTREGA VISUAL: Grupos e detalhes. REFINAMENTO CRÍTICO: Evitar semelhança literal com jogo famoso. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-010 — Campo de trigo

**Arquivo sugerido:** `COE-NAT-010_campo_de_trigo_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Planta, fileira e panorama.  
**Revisão específica:** Movimento e densidade otimizáveis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-010: Campo de trigo. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Plantas em várias fases de cultivo e trecho de campo. ENTREGA VISUAL: Planta, fileira e panorama. REFINAMENTO CRÍTICO: Movimento e densidade otimizáveis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-011 — Horta de legumes

**Arquivo sugerido:** `COE-NAT-011_horta_de_legumes_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Vista superior e em perspectiva.  
**Revisão específica:** Culturas na mesma escala.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-011: Horta de legumes. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Canteiros com raízes, folhas e suporte de madeira. ENTREGA VISUAL: Vista superior e em perspectiva. REFINAMENTO CRÍTICO: Culturas na mesma escala. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-012 — Pomar de Auren

**Arquivo sugerido:** `COE-NAT-012_pomar_de_auren_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Conjunto e árvore isolada.  
**Revisão específica:** Reutilizar materiais.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-012: Pomar de Auren. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Árvores frutíferas pequenas com cestaria e caminho. ENTREGA VISUAL: Conjunto e árvore isolada. REFINAMENTO CRÍTICO: Reutilizar materiais. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-013 — Vegetação de beira de estrada

**Arquivo sugerido:** `COE-NAT-013_vegetacao_de_beira_de_estrada_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Faixa modular e peças.  
**Revisão específica:** Não bloquear leitura de caminhos.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-013: Vegetação de beira de estrada. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Samambaias, gramíneas e pedras de trilha. ENTREGA VISUAL: Faixa modular e peças. REFINAMENTO CRÍTICO: Não bloquear leitura de caminhos. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-014 — Rochas de granito

**Arquivo sugerido:** `COE-NAT-014_rochas_de_granito_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Vários ângulos e composição.  
**Revisão específica:** Colisão simples e LOD.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-014: Rochas de granito. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Seis blocos angulares com materiais regionais. ENTREGA VISUAL: Vários ângulos e composição. REFINAMENTO CRÍTICO: Colisão simples e LOD. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-015 — Pedras de riacho

**Arquivo sugerido:** `COE-NAT-015_pedras_de_riacho_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Kit e cena pequena.  
**Revisão específica:** Materiais distinguíveis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-015: Pedras de riacho. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Seixos úmidos, margem e musgo. ENTREGA VISUAL: Kit e cena pequena. REFINAMENTO CRÍTICO: Materiais distinguíveis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-016 — Troncos caídos

**Arquivo sugerido:** `COE-NAT-016_troncos_caidos_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Frente, lado e material.  
**Revisão específica:** Escala compatível com personagem.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-016: Troncos caídos. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Três troncos com fungos, cortes e galhos. ENTREGA VISUAL: Frente, lado e material. REFINAMENTO CRÍTICO: Escala compatível com personagem. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-017 — Raízes de floresta

**Arquivo sugerido:** `COE-NAT-017_raizes_de_floresta_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Peças separadas e montadas.  
**Revisão específica:** Evitar clipping com terreno.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-017: Raízes de floresta. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Módulos curvos e de superfície para bases de árvores. ENTREGA VISUAL: Peças separadas e montadas. REFINAMENTO CRÍTICO: Evitar clipping com terreno. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-018 — Clareira do bosque

**Arquivo sugerido:** `COE-NAT-018_clareira_do_bosque_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Vista elevada e nível do personagem.  
**Revisão específica:** Jogabilidade de investigação.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-018: Clareira do bosque. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Espaço legível com pedras, luz filtrada e rota de saída. ENTREGA VISUAL: Vista elevada e nível do personagem. REFINAMENTO CRÍTICO: Jogabilidade de investigação. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-019 — Entrada do Bosque dos Sussurros

**Arquivo sugerido:** `COE-NAT-019_entrada_do_bosque_dos_sussurros_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Plano geral e POV do personagem.  
**Revisão específica:** Não transformar em terror explícito.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-019: Entrada do Bosque dos Sussurros. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Transição acolhedora-misteriosa de trilha, sinalização e copa fechada. ENTREGA VISUAL: Plano geral e POV do personagem. REFINAMENTO CRÍTICO: Não transformar em terror explícito. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-020 — Riacho de Auren

**Arquivo sugerido:** `COE-NAT-020_riacho_de_auren_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Plano geral e close de materiais.  
**Revisão específica:** Espaço de travessia seguro.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-020: Riacho de Auren. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Água rasa, pedras, margem e pequena ponte opcional. ENTREGA VISUAL: Plano geral e close de materiais. REFINAMENTO CRÍTICO: Espaço de travessia seguro. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-021 — Colinas distantes Eldoria

**Arquivo sugerido:** `COE-NAT-021_colinas_distantes_eldoria_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Panorama e camadas de profundidade.  
**Revisão específica:** Não sugerir tudo explorável no slice.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-021: Colinas distantes Eldoria. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Background modular com campos, floresta e pequenas estradas. ENTREGA VISUAL: Panorama e camadas de profundidade. REFINAMENTO CRÍTICO: Não sugerir tudo explorável no slice. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-NAT-022 — Bosque anômalo noturno

**Arquivo sugerido:** `COE-NAT-022_bosque_anomalo_noturno_concept_v01.png`  
**Categoria:** natureza e vegetação com silhueta e densidade otimizáveis  
**Saída exigida:** Antes/depois pareado.  
**Revisão específica:** Geometria base reutilizada.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-NAT-022: Bosque anômalo noturno. Categoria: natureza e vegetação com silhueta e densidade otimizáveis. DESCRIÇÃO DIRECIONADA: Mesmo bosque conhecido com iluminação e marcas discretas de Ruptura. ENTREGA VISUAL: Antes/depois pareado. REFINAMENTO CRÍTICO: Geometria base reutilizada. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```


## J. Fauna, criaturas e bestiário

### COE-CRE-001 — Galinha de vila

**Arquivo sugerido:** `COE-CRE-001_galinha_de_vila_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Frente, lateral e 3/4.  
**Revisão específica:** Adequada a animação simples.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-001: Galinha de vila. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Galinha estilizada de silhueta clara, penas modeláveis e atitude cotidiana. ENTREGA VISUAL: Frente, lateral e 3/4. REFINAMENTO CRÍTICO: Adequada a animação simples. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-002 — Cabra de Auren

**Arquivo sugerido:** `COE-CRE-002_cabra_de_auren_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Três vistas.  
**Revisão específica:** Proporção animal coerente.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-002: Cabra de Auren. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Caprino doméstico de pequena vila, pelagem e chifres funcionais. ENTREGA VISUAL: Três vistas. REFINAMENTO CRÍTICO: Proporção animal coerente. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-003 — Vaca de Auren

**Arquivo sugerido:** `COE-CRE-003_vaca_de_auren_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Frente, perfil e costas.  
**Revisão específica:** Sem humanização excessiva.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-003: Vaca de Auren. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Animal de criação regional, manchas próprias e corpo volumoso estilizado. ENTREGA VISUAL: Frente, perfil e costas. REFINAMENTO CRÍTICO: Sem humanização excessiva. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-004 — Cavalo de estrada

**Arquivo sugerido:** `COE-CRE-004_cavalo_de_estrada_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Perfil e 3/4, arreio separado.  
**Revisão específica:** Animação viável.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-004: Cavalo de estrada. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Cavalo de transporte com arreio simples, anatomia consistente. ENTREGA VISUAL: Perfil e 3/4, arreio separado. REFINAMENTO CRÍTICO: Animação viável. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-005 — Cão da vila

**Arquivo sugerido:** `COE-CRE-005_cao_da_vila_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Três vistas e duas poses.  
**Revisão específica:** Sem acessórios mágicos aleatórios.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-005: Cão da vila. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Companheiro doméstico com várias emoções legíveis. ENTREGA VISUAL: Três vistas e duas poses. REFINAMENTO CRÍTICO: Sem acessórios mágicos aleatórios. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-006 — Cervo regional

**Arquivo sugerido:** `COE-CRE-006_cervo_regional_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Perfil e frontal.  
**Revisão específica:** Ecologia plausível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-006: Cervo regional. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Fauna de floresta com galhada sazonal e pelagem natural. ENTREGA VISUAL: Perfil e frontal. REFINAMENTO CRÍTICO: Ecologia plausível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-007 — Coelho do bosque

**Arquivo sugerido:** `COE-CRE-007_coelho_do_bosque_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Frente, perfil e pose.  
**Revisão específica:** Articulações de salto.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-007: Coelho do bosque. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Animal pequeno e ágil de volume estilizado. ENTREGA VISUAL: Frente, perfil e pose. REFINAMENTO CRÍTICO: Articulações de salto. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-008 — Lobo de Eldoria

**Arquivo sugerido:** `COE-CRE-008_lobo_de_eldoria_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Três vistas e silhueta.  
**Revisão específica:** Não transformar em demônio.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-008: Lobo de Eldoria. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Predador regional natural, olhar atento e material de pelo simplificado. ENTREGA VISUAL: Três vistas e silhueta. REFINAMENTO CRÍTICO: Não transformar em demônio. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-009 — Pássaros de Auren

**Arquivo sugerido:** `COE-CRE-009_passaros_de_auren_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Folha de corpo/perfil.  
**Revisão específica:** Modelos reutilizáveis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-009: Pássaros de Auren. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Quatro espécies pequenas e comuns, cores comedidas. ENTREGA VISUAL: Folha de corpo/perfil. REFINAMENTO CRÍTICO: Modelos reutilizáveis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-010 — Peixe do riacho

**Arquivo sugerido:** `COE-CRE-010_peixe_do_riacho_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Perfil e 3/4.  
**Revisão específica:** Animável com poucos ossos.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-010: Peixe do riacho. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Espécie pequena de água doce para ambiente. ENTREGA VISUAL: Perfil e 3/4. REFINAMENTO CRÍTICO: Animável com poucos ossos. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-011 — Insetos de bosque

**Arquivo sugerido:** `COE-CRE-011_insetos_de_bosque_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Asas abertas/fechadas.  
**Revisão específica:** Geometria simples.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-011: Insetos de bosque. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Borboleta, besouro e vaga-lume fictícios para vida ambiente. ENTREGA VISUAL: Asas abertas/fechadas. REFINAMENTO CRÍTICO: Geometria simples. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-012 — Criatura arcana menor

**Arquivo sugerido:** `COE-CRE-012_criatura_arcana_menor_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Turnaround e proporção humana.  
**Revisão específica:** Não criatura-mascote copiada.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-012: Criatura arcana menor. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Pequeno ser original, anatomia coerente e acentos da Trama sutis. ENTREGA VISUAL: Turnaround e proporção humana. REFINAMENTO CRÍTICO: Não criatura-mascote copiada. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-013 — Guardião vegetal pequeno

**Arquivo sugerido:** `COE-CRE-013_guardiao_vegetal_pequeno_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Frente, perfil e costas.  
**Revisão específica:** Ação previsível e rig viável.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-013: Guardião vegetal pequeno. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Ser não humano associado ao bosque, raízes e musgos com forma legível. ENTREGA VISUAL: Frente, perfil e costas. REFINAMENTO CRÍTICO: Ação previsível e rig viável. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-014 — Besta hostil iniciante

**Arquivo sugerido:** `COE-CRE-014_besta_hostil_iniciante_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Três vistas e pose alerta.  
**Revisão específica:** Não usar excesso de espinhos.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-014: Besta hostil iniciante. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Ameaça moderada para área posterior à infância, proporção distinta de lobo comum. ENTREGA VISUAL: Três vistas e pose alerta. REFINAMENTO CRÍTICO: Não usar excesso de espinhos. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-015 — Sentinela de ruínas

**Arquivo sugerido:** `COE-CRE-015_sentinela_de_ruinas_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Frente, lado e conexões de juntas.  
**Revisão específica:** Ataques legíveis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-015: Sentinela de ruínas. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Construção antiga animável de pedra e padrões da Fratura. ENTREGA VISUAL: Frente, lado e conexões de juntas. REFINAMENTO CRÍTICO: Ataques legíveis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-016 — Entidade da anomalia

**Arquivo sugerido:** `COE-CRE-016_entidade_da_anomalia_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Frente, costas e detalhes.  
**Revisão específica:** Sem forma humanoide copiada.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-016: Entidade da anomalia. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Silhueta original com fragmentos da Trama e ruptura espacial visualmente controlada. ENTREGA VISUAL: Frente, costas e detalhes. REFINAMENTO CRÍTICO: Sem forma humanoide copiada. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-017 — Prancha de silhuetas de criaturas

**Arquivo sugerido:** `COE-CRE-017_prancha_de_silhuetas_de_criaturas_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Silhuetas monocromáticas.  
**Revisão específica:** Cada categoria distinguível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-017: Prancha de silhuetas de criaturas. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Comparativo fauna, arcano, hostil e extraordinário em escala humana. ENTREGA VISUAL: Silhuetas monocromáticas. REFINAMENTO CRÍTICO: Cada categoria distinguível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-018 — Patas e articulações

**Arquivo sugerido:** `COE-CRE-018_patas_e_articulacoes_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Close esquelético estilizado.  
**Revisão específica:** Rig funcional.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-018: Patas e articulações. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Estudos para quadrúpedes domésticos, selvagens e arcano pequeno. ENTREGA VISUAL: Close esquelético estilizado. REFINAMENTO CRÍTICO: Rig funcional. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-019 — Pelagens e materiais

**Arquivo sugerido:** `COE-CRE-019_pelagens_e_materiais_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Swatches e animal exemplo.  
**Revisão específica:** Consistência URP.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-019: Pelagens e materiais. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Biblioteca de pelo curto, longo, pena, casca e pedra viva. ENTREGA VISUAL: Swatches e animal exemplo. REFINAMENTO CRÍTICO: Consistência URP. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-CRE-020 — Expressões de criatura arcana

**Arquivo sugerido:** `COE-CRE-020_expressoes_de_criatura_arcana_concept_v01.png`  
**Categoria:** criatura original com anatomia e articulações animáveis  
**Saída exigida:** Close de cabeça.  
**Revisão específica:** Identidade estável.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-CRE-020: Expressões de criatura arcana. Categoria: criatura original com anatomia e articulações animáveis. DESCRIÇÃO DIRECIONADA: Seis estados para criatura menor, curiosidade e alerta sem antropomorfismo excessivo. ENTREGA VISUAL: Close de cabeça. REFINAMENTO CRÍTICO: Identidade estável. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```


## K. Trama, magia, ascensão e VFX

### COE-MAG-001 — Emblema primário da Trama

**Arquivo sugerido:** `COE-MAG-001_emblema_primario_da_trama_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Vetor conceitual monocromático e variante metal.  
**Revisão específica:** Não copiar símbolos conhecidos.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-001: Emblema primário da Trama. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Fios entrelaçados, círculos incompletos, quatro pontos e nó central, original. ENTREGA VISUAL: Vetor conceitual monocromático e variante metal. REFINAMENTO CRÍTICO: Não copiar símbolos conhecidos. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-002 — Glifos secundários

**Arquivo sugerido:** `COE-MAG-002_glifos_secundarios_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Grade de traços limpos.  
**Revisão específica:** Evitar escrita falsa ilegível como informação.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-002: Glifos secundários. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Doze variações coerentes com família geométrica da Trama. ENTREGA VISUAL: Grade de traços limpos. REFINAMENTO CRÍTICO: Evitar escrita falsa ilegível como informação. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-003 — Trama em pedra antiga

**Arquivo sugerido:** `COE-MAG-003_trama_em_pedra_antiga_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Close e aplicação em arco.  
**Revisão específica:** Material pedra dominante.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-003: Trama em pedra antiga. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Motivo entalhado desgastado em ruína de Elnor. ENTREGA VISUAL: Close e aplicação em arco. REFINAMENTO CRÍTICO: Material pedra dominante. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-004 — Trama em tecido

**Arquivo sugerido:** `COE-MAG-004_trama_em_tecido_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Amostra e aplicação em gola.  
**Revisão específica:** Costura plausível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-004: Trama em tecido. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Padrão de bordado sutil para vestes de Aethron e estudiosos. ENTREGA VISUAL: Amostra e aplicação em gola. REFINAMENTO CRÍTICO: Costura plausível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-005 — Trama em metal

**Arquivo sugerido:** `COE-MAG-005_trama_em_metal_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Macro de superfícies.  
**Revisão específica:** Evitar emissivo intenso.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-005: Trama em metal. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Inscrição discreta em arma, broche e talismã. ENTREGA VISUAL: Macro de superfícies. REFINAMENTO CRÍTICO: Evitar emissivo intenso. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-006 — Manifestação arcana leve

**Arquivo sugerido:** `COE-MAG-006_manifestacao_arcana_leve_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Três frames conceituais.  
**Revisão específica:** Não ocultar mãos/personagem.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-006: Manifestação arcana leve. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Fios de luz turquesa, início pequeno e leitura clara de conjuração. ENTREGA VISUAL: Três frames conceituais. REFINAMENTO CRÍTICO: Não ocultar mãos/personagem. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-007 — Manifestação arcana forte

**Arquivo sugerido:** `COE-MAG-007_manifestacao_arcana_forte_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Preparação, execução e dissipação.  
**Revisão específica:** Não efeito de tela inteira.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-007: Manifestação arcana forte. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Expansão geométrica controlada com círculos incompletos e partículas. ENTREGA VISUAL: Preparação, execução e dissipação. REFINAMENTO CRÍTICO: Não efeito de tela inteira. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-008 — Magia fogo

**Arquivo sugerido:** `COE-MAG-008_magia_fogo_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Sequência de três quadros.  
**Revisão específica:** Sem fumaça que esconda o alvo.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-008: Magia fogo. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Ataque em forma legível, núcleo quente, ignição e curta dissipação. ENTREGA VISUAL: Sequência de três quadros. REFINAMENTO CRÍTICO: Sem fumaça que esconda o alvo. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-009 — Magia água

**Arquivo sugerido:** `COE-MAG-009_magia_agua_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Três frames e material.  
**Revisão específica:** Evitar esfera genérica azul.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-009: Magia água. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Fluxo, gotas e efeito de contenção defensivo/ofensivo. ENTREGA VISUAL: Três frames e material. REFINAMENTO CRÍTICO: Evitar esfera genérica azul. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-010 — Magia vento

**Arquivo sugerido:** `COE-MAG-010_magia_vento_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Três frames e silhueta.  
**Revisão específica:** Alvo visível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-010: Magia vento. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Linhas de pressão e partículas de ambiente, efeito transparente. ENTREGA VISUAL: Três frames e silhueta. REFINAMENTO CRÍTICO: Alvo visível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-011 — Magia terra

**Arquivo sugerido:** `COE-MAG-011_magia_terra_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Preparação e impacto.  
**Revisão específica:** Física visual coerente.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-011: Magia terra. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Fragmentos angulares, pó e bloco defensivo com origem no solo. ENTREGA VISUAL: Preparação e impacto. REFINAMENTO CRÍTICO: Física visual coerente. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-012 — Magia protetiva

**Arquivo sugerido:** `COE-MAG-012_magia_protetiva_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Antes/durante/absorção.  
**Revisão específica:** Não cobrir HUD nem aliado.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-012: Magia protetiva. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Campo estável de linhas circulares e superfície translúcida. ENTREGA VISUAL: Antes/durante/absorção. REFINAMENTO CRÍTICO: Não cobrir HUD nem aliado. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-013 — Magia restauradora

**Arquivo sugerido:** `COE-MAG-013_magia_restauradora_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Três frames.  
**Revisão específica:** Não prometer cura de condição não definida.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-013: Magia restauradora. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Movimento delicado de fios luminosos e padrões de recomposição. ENTREGA VISUAL: Três frames. REFINAMENTO CRÍTICO: Não prometer cura de condição não definida. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-014 — Encantamento de espada

**Arquivo sugerido:** `COE-MAG-014_encantamento_de_espada_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Arma sem/ativa e detalhes.  
**Revisão específica:** Lâmina legível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-014: Encantamento de espada. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Geometria da Trama seguindo canal da lâmina, núcleo contido. ENTREGA VISUAL: Arma sem/ativa e detalhes. REFINAMENTO CRÍTICO: Lâmina legível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-015 — Ascensão divina

**Arquivo sugerido:** `COE-MAG-015_ascensao_divina_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Storyboard três frames.  
**Revisão específica:** Não retratar grau como vitória gratuita.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-015: Ascensão divina. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Luz dourada e turquesa, símbolo de reconhecimento, sem troca automática de traje. ENTREGA VISUAL: Storyboard três frames. REFINAMENTO CRÍTICO: Não retratar grau como vitória gratuita. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-016 — Ascensão arcana

**Arquivo sugerido:** `COE-MAG-016_ascensao_arcana_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Três frames e close de glifos.  
**Revisão específica:** Continuidade do personagem.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-016: Ascensão arcana. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Padrões reorganizados ao redor de foco pesquisado e estabilizado. ENTREGA VISUAL: Três frames e close de glifos. REFINAMENTO CRÍTICO: Continuidade do personagem. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-017 — Ascensão superação

**Arquivo sugerido:** `COE-MAG-017_ascensao_superacao_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Antes, momento e depois.  
**Revisão específica:** Não efeito cósmico obrigatório.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-017: Ascensão superação. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Expressão de domínio técnico, foco em postura/energia e equipamento pessoal. ENTREGA VISUAL: Antes, momento e depois. REFINAMENTO CRÍTICO: Não efeito cósmico obrigatório. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-018 — Ascensão ruptura

**Arquivo sugerido:** `COE-MAG-018_ascensao_ruptura_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Três frames com fundo neutro.  
**Revisão específica:** Anomalia deve ser legível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-018: Ascensão ruptura. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Linhas desconectadas, violeta contido e reestruturação de fios. ENTREGA VISUAL: Três frames com fundo neutro. REFINAMENTO CRÍTICO: Anomalia deve ser legível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-019 — Fratura primordial

**Arquivo sugerido:** `COE-MAG-019_fratura_primordial_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Plano geral e detalhes.  
**Revisão específica:** Preservar pontos de navegação.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-019: Fratura primordial. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Fenômeno ambiental de padrões interrompidos e espaço distorcido, sem destruir indiscriminadamente mundo. ENTREGA VISUAL: Plano geral e detalhes. REFINAMENTO CRÍTICO: Preservar pontos de navegação. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-MAG-020 — Altar de ascensão

**Arquivo sugerido:** `COE-MAG-020_altar_de_ascensao_concept_v01.png`  
**Categoria:** linguagem mágica original com preparação, manifestação e consequência  
**Saída exigida:** Frente, três quartos e símbolos removíveis.  
**Revisão específica:** Não impor panteão único.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-MAG-020: Altar de ascensão. Categoria: linguagem mágica original com preparação, manifestação e consequência. DESCRIÇÃO DIRECIONADA: Dispositivo arquitetônico neutro que admite quatro caminhos em cenas distintas. ENTREGA VISUAL: Frente, três quartos e símbolos removíveis. REFINAMENTO CRÍTICO: Não impor panteão único. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```


## L. Cenas narrativas, keyframes e iluminação

### COE-SCN-001 — Limiar, plano de abertura

**Arquivo sugerido:** `COE-SCN-001_limiar_plano_de_abertura_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Composição 16:9 e versões sem personagens.  
**Revisão específica:** Centro de atenção claro, sem interface inventada.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-001: Limiar, plano de abertura. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Santuário celestial com Aethron distante, espaço de escolha e fios da Trama. ENTREGA VISUAL: Composição 16:9 e versões sem personagens. REFINAMENTO CRÍTICO: Centro de atenção claro, sem interface inventada. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-002 — Limiar, encontro com guardião

**Arquivo sugerido:** `COE-SCN-002_limiar_encontro_com_guardiao_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Plano aberto e plano médio.  
**Revisão específica:** Não imitar cena de isekai reconhecível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-002: Limiar, encontro com guardião. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Alma de novo personagem frente à divindade, escala e mistério. ENTREGA VISUAL: Plano aberto e plano médio. REFINAMENTO CRÍTICO: Não imitar cena de isekai reconhecível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-003 — Limiar, escolha dos destinos

**Arquivo sugerido:** `COE-SCN-003_limiar_escolha_dos_destinos_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Keyframe 16:9 com quatro portais discretos.  
**Revisão específica:** Legibilidade e neutralidade.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-003: Limiar, escolha dos destinos. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Quatro possibilidades como ambientes e símbolos, sem ranking visual de superioridade. ENTREGA VISUAL: Keyframe 16:9 com quatro portais discretos. REFINAMENTO CRÍTICO: Legibilidade e neutralidade. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-004 — Nascimento, transição

**Arquivo sugerido:** `COE-SCN-004_nascimento_transicao_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Storyboard três quadros.  
**Revisão específica:** Poética, adequada à faixa etária.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-004: Nascimento, transição. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Passagem simbólica de luz do Limiar à residência de Auren, sem representação gráfica de parto. ENTREGA VISUAL: Storyboard três quadros. REFINAMENTO CRÍTICO: Poética, adequada à faixa etária. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-005 — Auren, visão geral

**Arquivo sugerido:** `COE-SCN-005_auren_visao_geral_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Aérea 3/4 e eye-level.  
**Revisão específica:** Escala viável para vertical slice.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-005: Auren, visão geral. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Vila compacta com praça, moradias, ferraria, bosque e estrada. ENTREGA VISUAL: Aérea 3/4 e eye-level. REFINAMENTO CRÍTICO: Escala viável para vertical slice. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-006 — Auren, amanhecer

**Arquivo sugerido:** `COE-SCN-006_auren_amanhecer_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Landscape 16:9.  
**Revisão específica:** Não mudar arquitetura.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-006: Auren, amanhecer. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Mesmo layout de Auren com luz dourada, aldeões começando rotina. ENTREGA VISUAL: Landscape 16:9. REFINAMENTO CRÍTICO: Não mudar arquitetura. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-007 — Auren, entardecer

**Arquivo sugerido:** `COE-SCN-007_auren_entardecer_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Landscape 16:9.  
**Revisão específica:** Continuidade espacial.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-007: Auren, entardecer. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Mesmo local com mercado encerrando e fumaça de lareiras. ENTREGA VISUAL: Landscape 16:9. REFINAMENTO CRÍTICO: Continuidade espacial. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-008 — Auren, noite

**Arquivo sugerido:** `COE-SCN-008_auren_noite_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Landscape 16:9.  
**Revisão específica:** Não virar vila de terror.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-008: Auren, noite. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Lanternas e janelas iluminadas, circulação reduzida, atmosfera segura. ENTREGA VISUAL: Landscape 16:9. REFINAMENTO CRÍTICO: Não virar vila de terror. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-009 — Casa, primeiro despertar

**Arquivo sugerido:** `COE-SCN-009_casa_primeiro_despertar_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** POV e plano aberto.  
**Revisão específica:** Mesmo layout adaptado aos três arquétipos.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-009: Casa, primeiro despertar. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Quarto infantil modesto com objetos da origem selecionada. ENTREGA VISUAL: POV e plano aberto. REFINAMENTO CRÍTICO: Mesmo layout adaptado aos três arquétipos. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-010 — Cena familiar agricultora

**Arquivo sugerido:** `COE-SCN-010_cena_familiar_agricultora_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Narrativa 16:9 e detalhes de objetos.  
**Revisão específica:** Não equiparar pobreza a dificuldade automaticamente.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-010: Cena familiar agricultora. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Responsáveis e criança em atividade cotidiana de quintal. ENTREGA VISUAL: Narrativa 16:9 e detalhes de objetos. REFINAMENTO CRÍTICO: Não equiparar pobreza a dificuldade automaticamente. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-011 — Cena familiar artesã

**Arquivo sugerido:** `COE-SCN-011_cena_familiar_artesa_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Narrativa 16:9.  
**Revisão específica:** Ferramentas perigosas fora do alcance infantil.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-011: Cena familiar artesã. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Criança observando atividade segura em oficina doméstica. ENTREGA VISUAL: Narrativa 16:9. REFINAMENTO CRÍTICO: Ferramentas perigosas fora do alcance infantil. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-012 — Cena familiar guardiã

**Arquivo sugerido:** `COE-SCN-012_cena_familiar_guardia_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Narrativa 16:9.  
**Revisão específica:** Não combate letal.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-012: Cena familiar guardiã. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Família ensinando disciplina por atividade lúdica no quintal. ENTREGA VISUAL: Narrativa 16:9. REFINAMENTO CRÍTICO: Não combate letal. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-013 — Praça, encontro Nilo e Sera

**Arquivo sugerido:** `COE-SCN-013_praca_encontro_nilo_e_sera_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Plano geral e reação facial.  
**Revisão específica:** Preservar identidades canônicas.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-013: Praça, encontro Nilo e Sera. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: As duas crianças com protagonista em conflito leve ou brincadeira. ENTREGA VISUAL: Plano geral e reação facial. REFINAMENTO CRÍTICO: Preservar identidades canônicas. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-014 — Missão O Cesto Perdido

**Arquivo sugerido:** `COE-SCN-014_missao_o_cesto_perdido_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Cena jogável em perspectiva de câmera 3ª pessoa.  
**Revisão específica:** Localizável sem brilho arbitrário.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-014: Missão O Cesto Perdido. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Cesto característico próximo à trilha, pistas visuais naturais. ENTREGA VISUAL: Cena jogável em perspectiva de câmera 3ª pessoa. REFINAMENTO CRÍTICO: Localizável sem brilho arbitrário. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-015 — Missão Animal Ferido

**Arquivo sugerido:** `COE-SCN-015_missao_animal_ferido_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Keyframe com composição clara.  
**Revisão específica:** Sem sofrimento explícito.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-015: Missão Animal Ferido. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Lysa orienta criança em cuidado básico a um animal da vila. ENTREGA VISUAL: Keyframe com composição clara. REFINAMENTO CRÍTICO: Sem sofrimento explícito. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-016 — Borin, aprendizado

**Arquivo sugerido:** `COE-SCN-016_borin_aprendizado_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Plano geral e close das mãos do mentor.  
**Revisão específica:** Ferraria coerente com arquitetura aprovada.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-016: Borin, aprendizado. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Criança observando forja a distância segura; mentor demonstra técnica. ENTREGA VISUAL: Plano geral e close das mãos do mentor. REFINAMENTO CRÍTICO: Ferraria coerente com arquitetura aprovada. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-017 — Desaparecimento no bosque

**Arquivo sugerido:** `COE-SCN-017_desaparecimento_no_bosque_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Plano de jogo e keyframe cinematográfico.  
**Revisão específica:** Não bloquear orientação.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-017: Desaparecimento no bosque. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Entrada da floresta, trilha de pistas e tensão sutil sem violência. ENTREGA VISUAL: Plano de jogo e keyframe cinematográfico. REFINAMENTO CRÍTICO: Não bloquear orientação. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-018 — Ecos do Limiar

**Arquivo sugerido:** `COE-SCN-018_ecos_do_limiar_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Close do símbolo e plano amplo.  
**Revisão específica:** Símbolo exatamente igual ao guia.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-018: Ecos do Limiar. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Símbolo antigo em ruína pequena desperta memória do protagonista. ENTREGA VISUAL: Close do símbolo e plano amplo. REFINAMENTO CRÍTICO: Símbolo exatamente igual ao guia. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-019 — Primeiro salto temporal

**Arquivo sugerido:** `COE-SCN-019_primeiro_salto_temporal_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Díptico correspondente.  
**Revisão específica:** Não redesenhar todo o mapa.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-019: Primeiro salto temporal. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Mesmo local da vila antes/depois com pequenas mudanças e crescimento da criança. ENTREGA VISUAL: Díptico correspondente. REFINAMENTO CRÍTICO: Não redesenhar todo o mapa. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-020 — Treinamento aos oito anos

**Arquivo sugerido:** `COE-SCN-020_treinamento_aos_oito_anos_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Câmera 3ª pessoa e composição lateral.  
**Revisão específica:** Atividades seguras.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-020: Treinamento aos oito anos. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Tovin com espada de madeira em espaço supervisionado. ENTREGA VISUAL: Câmera 3ª pessoa e composição lateral. REFINAMENTO CRÍTICO: Atividades seguras. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-021 — Primeira magia

**Arquivo sugerido:** `COE-SCN-021_primeira_magia_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Keyframe e close de mão.  
**Revisão específica:** Poder limitado.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-021: Primeira magia. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Manifestação arcana pequena e imprevista, emoções humanas ao redor. ENTREGA VISUAL: Keyframe e close de mão. REFINAMENTO CRÍTICO: Poder limitado. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-022 — Bosque dos Sussurros, dia

**Arquivo sugerido:** `COE-SCN-022_bosque_dos_sussurros_dia_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Ambiente 16:9 e mapa visual simples.  
**Revisão específica:** Otimizável.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-022: Bosque dos Sussurros, dia. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Floresta legível com clareira, riacho e trilha de missão. ENTREGA VISUAL: Ambiente 16:9 e mapa visual simples. REFINAMENTO CRÍTICO: Otimizável. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-023 — Bosque dos Sussurros, noite

**Arquivo sugerido:** `COE-SCN-023_bosque_dos_sussurros_noite_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Comparativo dia/noite.  
**Revisão específica:** Navegação continua legível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-023: Bosque dos Sussurros, noite. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Mesma geometria, iluminação atmosférica e indício de Trama. ENTREGA VISUAL: Comparativo dia/noite. REFINAMENTO CRÍTICO: Navegação continua legível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-024 — Ruínas Elnor, antecipação

**Arquivo sugerido:** `COE-SCN-024_ruinas_elnor_antecipacao_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Landscape 16:9.  
**Revisão específica:** Conteúdo futuro, não jogável no slice.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-024: Ruínas Elnor, antecipação. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Estruturas antigas vistas à distância, associação à Primeira Fratura. ENTREGA VISUAL: Landscape 16:9. REFINAMENTO CRÍTICO: Conteúdo futuro, não jogável no slice. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-SCN-025 — Cartaz-chave do jogo

**Arquivo sugerido:** `COE-SCN-025_cartaz_chave_do_jogo_concept_v01.png`  
**Categoria:** composição narrativa e ambiental jogável em terceira pessoa  
**Saída exigida:** Key art horizontal e vertical sem lettering complexo.  
**Revisão específica:** Original, protagonismo do viver e não só combate.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-SCN-025: Cartaz-chave do jogo. Categoria: composição narrativa e ambiental jogável em terceira pessoa. DESCRIÇÃO DIRECIONADA: Criança e sombra evolutiva adulta diante de Auren, fio geométrico conecta fases da vida. ENTREGA VISUAL: Key art horizontal e vertical sem lettering complexo. REFINAMENTO CRÍTICO: Original, protagonismo do viver e não só combate. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```


## M. Interface, iconografia e material de pitch

### COE-UI-001 — HUD exploração

**Arquivo sugerido:** `COE-UI-001_hud_exploracao_concept_v01.png`  
**Categoria:** mockup de interface legível, NÃO texto final de produto  
**Saída exigida:** Mockup 16:9 com cenário simples.  
**Revisão específica:** Não usar texto gerado como especificação final.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-UI-001: HUD exploração. Categoria: mockup de interface legível, NÃO texto final de produto. DESCRIÇÃO DIRECIONADA: Vida/vigor/mana discretos, objetivo, localização e interação clara. ENTREGA VISUAL: Mockup 16:9 com cenário simples. REFINAMENTO CRÍTICO: Não usar texto gerado como especificação final. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-UI-002 — HUD combate

**Arquivo sugerido:** `COE-UI-002_hud_combate_concept_v01.png`  
**Categoria:** mockup de interface legível, NÃO texto final de produto  
**Saída exigida:** Mockup 16:9 e blocos isolados.  
**Revisão específica:** Nenhum excesso de efeitos.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-UI-002: HUD combate. Categoria: mockup de interface legível, NÃO texto final de produto. DESCRIÇÃO DIRECIONADA: Barras, quatro slots de habilidade e leitura limpa de alvo. ENTREGA VISUAL: Mockup 16:9 e blocos isolados. REFINAMENTO CRÍTICO: Nenhum excesso de efeitos. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-UI-003 — Tela criação do personagem

**Arquivo sugerido:** `COE-UI-003_tela_criacao_do_personagem_concept_v01.png`  
**Categoria:** mockup de interface legível, NÃO texto final de produto  
**Saída exigida:** Layout desktop 16:9.  
**Revisão específica:** Legível e sem sexualização.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-UI-003: Tela criação do personagem. Categoria: mockup de interface legível, NÃO texto final de produto. DESCRIÇÃO DIRECIONADA: Controles de aparência modular em volta de modelo infantil digno. ENTREGA VISUAL: Layout desktop 16:9. REFINAMENTO CRÍTICO: Legível e sem sexualização. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-UI-004 — Tela escolha de destino

**Arquivo sugerido:** `COE-UI-004_tela_escolha_de_destino_concept_v01.png`  
**Categoria:** mockup de interface legível, NÃO texto final de produto  
**Saída exigida:** Mockup e iconografia isolada.  
**Revisão específica:** Destino distinto de ascensão.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-UI-004: Tela escolha de destino. Categoria: mockup de interface legível, NÃO texto final de produto. DESCRIÇÃO DIRECIONADA: Quatro opções com ícones e descrição, sem indicação automática de melhor caminho. ENTREGA VISUAL: Mockup e iconografia isolada. REFINAMENTO CRÍTICO: Destino distinto de ascensão. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-UI-005 — Tela escolha de origem

**Arquivo sugerido:** `COE-UI-005_tela_escolha_de_origem_concept_v01.png`  
**Categoria:** mockup de interface legível, NÃO texto final de produto  
**Saída exigida:** Mockup 16:9.  
**Revisão específica:** Não mostrar números não aprovados.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-UI-005: Tela escolha de origem. Categoria: mockup de interface legível, NÃO texto final de produto. DESCRIÇÃO DIRECIONADA: Três cartas de família condicionadas ao destino, ambientes e características. ENTREGA VISUAL: Mockup 16:9. REFINAMENTO CRÍTICO: Não mostrar números não aprovados. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-UI-006 — Painel atributos

**Arquivo sugerido:** `COE-UI-006_painel_atributos_concept_v01.png`  
**Categoria:** mockup de interface legível, NÃO texto final de produto  
**Saída exigida:** Mockup mais ícones.  
**Revisão específica:** Sem falsa promessa de fórmulas finais.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-UI-006: Painel atributos. Categoria: mockup de interface legível, NÃO texto final de produto. DESCRIÇÃO DIRECIONADA: Força, Agilidade, Vigor, Intelecto, Percepção, Vontade com hierarquia clara. ENTREGA VISUAL: Mockup mais ícones. REFINAMENTO CRÍTICO: Sem falsa promessa de fórmulas finais. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-UI-007 — Painel afinidades

**Arquivo sugerido:** `COE-UI-007_painel_afinidades_concept_v01.png`  
**Categoria:** mockup de interface legível, NÃO texto final de produto  
**Saída exigida:** Mockup e símbolo isolado.  
**Revisão específica:** Sem classes rígidas.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-UI-007: Painel afinidades. Categoria: mockup de interface legível, NÃO texto final de produto. DESCRIÇÃO DIRECIONADA: Marcial, Arcana, Natural, Artesanal, Social, Exploratória em visual não linear. ENTREGA VISUAL: Mockup e símbolo isolado. REFINAMENTO CRÍTICO: Sem classes rígidas. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-UI-008 — Painel reputação

**Arquivo sugerido:** `COE-UI-008_painel_reputacao_concept_v01.png`  
**Categoria:** mockup de interface legível, NÃO texto final de produto  
**Saída exigida:** Mockup e ícones.  
**Revisão específica:** Não barra única bom/mau.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-UI-008: Painel reputação. Categoria: mockup de interface legível, NÃO texto final de produto. DESCRIÇÃO DIRECIONADA: Relações contextuais por vila/facção e dimensões legíveis. ENTREGA VISUAL: Mockup e ícones. REFINAMENTO CRÍTICO: Não barra única bom/mau. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-UI-009 — Diário de vida

**Arquivo sugerido:** `COE-UI-009_diario_de_vida_concept_v01.png`  
**Categoria:** mockup de interface legível, NÃO texto final de produto  
**Saída exigida:** Mockup e cartão de evento.  
**Revisão específica:** Datas/nomes placeholders claros.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-UI-009: Diário de vida. Categoria: mockup de interface legível, NÃO texto final de produto. DESCRIÇÃO DIRECIONADA: Linha temporal com eventos, idade e memórias pessoais. ENTREGA VISUAL: Mockup e cartão de evento. REFINAMENTO CRÍTICO: Datas/nomes placeholders claros. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-UI-010 — Diário de missões

**Arquivo sugerido:** `COE-UI-010_diario_de_missoes_concept_v01.png`  
**Categoria:** mockup de interface legível, NÃO texto final de produto  
**Saída exigida:** Mockup e cartões.  
**Revisão específica:** Informação de requisitos clara.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-UI-010: Diário de missões. Categoria: mockup de interface legível, NÃO texto final de produto. DESCRIÇÃO DIRECIONADA: Quatro categorias de missão e status de conclusão. ENTREGA VISUAL: Mockup e cartões. REFINAMENTO CRÍTICO: Informação de requisitos clara. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-UI-011 — Tela de ascensão

**Arquivo sugerido:** `COE-UI-011_tela_de_ascensao_concept_v01.png`  
**Categoria:** mockup de interface legível, NÃO texto final de produto  
**Saída exigida:** Mockup e ícones.  
**Revisão específica:** Separar Nível de Vida e Nível de Grau.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-UI-011: Tela de ascensão. Categoria: mockup de interface legível, NÃO texto final de produto. DESCRIÇÃO DIRECIONADA: Grau atual, caminho, quatro requisitos e histórico; visual não promete poder automático. ENTREGA VISUAL: Mockup e ícones. REFINAMENTO CRÍTICO: Separar Nível de Vida e Nível de Grau. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-UI-012 — Inventário

**Arquivo sugerido:** `COE-UI-012_inventario_concept_v01.png`  
**Categoria:** mockup de interface legível, NÃO texto final de produto  
**Saída exigida:** Mockup 16:9.  
**Revisão específica:** Não canonizar limites numéricos.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-UI-012: Inventário. Categoria: mockup de interface legível, NÃO texto final de produto. DESCRIÇÃO DIRECIONADA: Mochila organizada, itens por tipo, equipamento e peso como possibilidade futura. ENTREGA VISUAL: Mockup 16:9. REFINAMENTO CRÍTICO: Não canonizar limites numéricos. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-UI-013 — Conjunto de ícones

**Arquivo sugerido:** `COE-UI-013_conjunto_de_icones_concept_v01.png`  
**Categoria:** mockup de interface legível, NÃO texto final de produto  
**Saída exigida:** Grade monocromática e em cor.  
**Revisão específica:** Consistentes e compreensíveis sem cor.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-UI-013: Conjunto de ícones. Categoria: mockup de interface legível, NÃO texto final de produto. DESCRIÇÃO DIRECIONADA: 24 ícones originais para vida, mana, vigor, origem, graus e ofícios. ENTREGA VISUAL: Grade monocromática e em cor. REFINAMENTO CRÍTICO: Consistentes e compreensíveis sem cor. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-UI-014 — Mapa de Auren

**Arquivo sugerido:** `COE-UI-014_mapa_de_auren_concept_v01.png`  
**Categoria:** mockup de interface legível, NÃO texto final de produto  
**Saída exigida:** Vista superior, legendas substituíveis.  
**Revisão específica:** Layout reflete cenário real, não inventa novas áreas.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-UI-014: Mapa de Auren. Categoria: mockup de interface legível, NÃO texto final de produto. DESCRIÇÃO DIRECIONADA: Mapa de jogo estilizado com praça, casas, bosque e estrada. ENTREGA VISUAL: Vista superior, legendas substituíveis. REFINAMENTO CRÍTICO: Layout reflete cenário real, não inventa novas áreas. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-UI-015 — Prancha de apresentação

**Arquivo sugerido:** `COE-UI-015_prancha_de_apresentacao_concept_v01.png`  
**Categoria:** mockup de interface legível, NÃO texto final de produto  
**Saída exigida:** Layout de pitch 16:9 sem texto miúdo.  
**Revisão específica:** Apenas designs aprovados.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-UI-015: Prancha de apresentação. Categoria: mockup de interface legível, NÃO texto final de produto. DESCRIÇÃO DIRECIONADA: Art board com protagonista, Auren, NPCs, quatro destinos e Trama em formato comercial. ENTREGA VISUAL: Layout de pitch 16:9 sem texto miúdo. REFINAMENTO CRÍTICO: Apenas designs aprovados. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```


## N. Regiões futuras e universo ampliado

### COE-REG-001 — Eldoria, panorama

**Arquivo sugerido:** `COE-REG-001_eldoria_panorama_concept_v01.png`  
**Categoria:** exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice  
**Saída exigida:** Environment key art 16:9.  
**Revisão específica:** Auren pertence à mesma arquitetura.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-REG-001: Eldoria, panorama. Categoria: exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice. DESCRIÇÃO DIRECIONADA: Campos, vilas, fortalezas moderadas e estrada entre comunidades. ENTREGA VISUAL: Environment key art 16:9. REFINAMENTO CRÍTICO: Auren pertence à mesma arquitetura. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-REG-002 — Valecross

**Arquivo sugerido:** `COE-REG-002_valecross_concept_v01.png`  
**Categoria:** exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice  
**Saída exigida:** Vista elevada e rua.  
**Revisão específica:** Conteúdo de expansão.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-REG-002: Valecross. Categoria: exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice. DESCRIÇÃO DIRECIONADA: Cidade comercial regional com mercado ampliado e vias de caravanas. ENTREGA VISUAL: Vista elevada e rua. REFINAMENTO CRÍTICO: Conteúdo de expansão. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-REG-003 — Academia de Lythar

**Arquivo sugerido:** `COE-REG-003_academia_de_lythar_concept_v01.png`  
**Categoria:** exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice  
**Saída exigida:** Fachada, pátio e interior.  
**Revisão específica:** Não escola moderna ou castelo de Hogwarts.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-REG-003: Academia de Lythar. Categoria: exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice. DESCRIÇÃO DIRECIONADA: Centro de estudo com arquitetura própria e elementos da Trama discretos. ENTREGA VISUAL: Fachada, pátio e interior. REFINAMENTO CRÍTICO: Não escola moderna ou castelo de Hogwarts. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-REG-004 — Castelo de Eldoria

**Arquivo sugerido:** `COE-REG-004_castelo_de_eldoria_concept_v01.png`  
**Categoria:** exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice  
**Saída exigida:** Visão externa e silhueta.  
**Revisão específica:** Heráldica própria.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-REG-004: Castelo de Eldoria. Categoria: exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice. DESCRIÇÃO DIRECIONADA: Sede monárquica de escala plausível, pedra, torres e espaço cívico. ENTREGA VISUAL: Visão externa e silhueta. REFINAMENTO CRÍTICO: Heráldica própria. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-REG-005 — Montanhas Karvorn

**Arquivo sugerido:** `COE-REG-005_montanhas_karvorn_concept_v01.png`  
**Categoria:** exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice  
**Saída exigida:** Panorama e rua de povoado.  
**Revisão específica:** Não copiar arquitetura anã famosa.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-REG-005: Montanhas Karvorn. Categoria: exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice. DESCRIÇÃO DIRECIONADA: Rochedos frios, mineração e pequenas fortalezas integradas. ENTREGA VISUAL: Panorama e rua de povoado. REFINAMENTO CRÍTICO: Não copiar arquitetura anã famosa. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-REG-006 — Mina de Karvorn

**Arquivo sugerido:** `COE-REG-006_mina_de_karvorn_concept_v01.png`  
**Categoria:** exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice  
**Saída exigida:** Corte visual e detalhe.  
**Revisão específica:** Engenharia plausível.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-REG-006: Mina de Karvorn. Categoria: exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice. DESCRIÇÃO DIRECIONADA: Galerias de mineração, sistemas de suporte e materiais raros. ENTREGA VISUAL: Corte visual e detalhe. REFINAMENTO CRÍTICO: Engenharia plausível. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-REG-007 — Florestas Sylvara

**Arquivo sugerido:** `COE-REG-007_florestas_sylvara_concept_v01.png`  
**Categoria:** exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice  
**Saída exigida:** Panorama e nível do personagem.  
**Revisão específica:** Não copiar cidade élfica famosa.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-REG-007: Florestas Sylvara. Categoria: exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice. DESCRIÇÃO DIRECIONADA: Mata ancestral, aldeias discretas e concentrações de energia natural. ENTREGA VISUAL: Panorama e nível do personagem. REFINAMENTO CRÍTICO: Não copiar cidade élfica famosa. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-REG-008 — Comunidade de Sylvara

**Arquivo sugerido:** `COE-REG-008_comunidade_de_sylvara_concept_v01.png`  
**Categoria:** exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice  
**Saída exigida:** Fachada e detalhe.  
**Revisão específica:** Recursos locais verossímeis.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-REG-008: Comunidade de Sylvara. Categoria: exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice. DESCRIÇÃO DIRECIONADA: Moradias integradas com floresta sem desafiar gravidade gratuitamente. ENTREGA VISUAL: Fachada e detalhe. REFINAMENTO CRÍTICO: Recursos locais verossímeis. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-REG-009 — Terras Ashkar

**Arquivo sugerido:** `COE-REG-009_terras_ashkar_concept_v01.png`  
**Categoria:** exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice  
**Saída exigida:** Panorama 16:9.  
**Revisão específica:** Cultura original sem exotificação rasa.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-REG-009: Terras Ashkar. Categoria: exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice. DESCRIÇÃO DIRECIONADA: Clima árido, centros de estudo e ruínas associadas à pesquisa arcana. ENTREGA VISUAL: Panorama 16:9. REFINAMENTO CRÍTICO: Cultura original sem exotificação rasa. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-REG-010 — Biblioteca de Ashkar

**Arquivo sugerido:** `COE-REG-010_biblioteca_de_ashkar_concept_v01.png`  
**Categoria:** exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice  
**Saída exigida:** Grande sala e canto modular.  
**Revisão específica:** Não visual de biblioteca contemporânea.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-REG-010: Biblioteca de Ashkar. Categoria: exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice. DESCRIÇÃO DIRECIONADA: Ambiente de manuscritos e instrumentos de pesquisa. ENTREGA VISUAL: Grande sala e canto modular. REFINAMENTO CRÍTICO: Não visual de biblioteca contemporânea. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-REG-011 — Nharos, território esquecido

**Arquivo sugerido:** `COE-REG-011_nharos_territorio_esquecido_concept_v01.png`  
**Categoria:** exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice  
**Saída exigida:** Ambiente diurno e noturno.  
**Revisão específica:** Misterioso, não todo sombrio.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-REG-011: Nharos, território esquecido. Categoria: exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice. DESCRIÇÃO DIRECIONADA: Paisagem de ruínas e fenômenos discretos da Fratura. ENTREGA VISUAL: Ambiente diurno e noturno. REFINAMENTO CRÍTICO: Misterioso, não todo sombrio. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-REG-012 — Ruínas centrais Nharos

**Arquivo sugerido:** `COE-REG-012_ruinas_centrais_nharos_concept_v01.png`  
**Categoria:** exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice  
**Saída exigida:** Vista 3/4 e detalhe.  
**Revisão específica:** Conexão cultural visual.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-REG-012: Ruínas centrais Nharos. Categoria: exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice. DESCRIÇÃO DIRECIONADA: Monumento antigo, arquitetura coerente com Elnor e símbolo incompleto. ENTREGA VISUAL: Vista 3/4 e detalhe. REFINAMENTO CRÍTICO: Conexão cultural visual. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-REG-013 — Mapa conceitual Valtheris

**Arquivo sugerido:** `COE-REG-013_mapa_conceitual_valtheris_concept_v01.png`  
**Categoria:** exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice  
**Saída exigida:** Mapa top-down estilizado.  
**Revisão específica:** Etiquetas podem ser adicionadas na pós-produção.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-REG-013: Mapa conceitual Valtheris. Categoria: exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice. DESCRIÇÃO DIRECIONADA: Cinco regiões e localização aproximada de Eldoria/Auren, sem precisão cartográfica falsa. ENTREGA VISUAL: Mapa top-down estilizado. REFINAMENTO CRÍTICO: Etiquetas podem ser adicionadas na pós-produção. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```

### COE-REG-014 — Famílias de heráldica regional

**Arquivo sugerido:** `COE-REG-014_familias_de_heraldica_regional_concept_v01.png`  
**Categoria:** exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice  
**Saída exigida:** Prancha com cores e P&B.  
**Revisão específica:** Não copiar símbolos nacionais reais.

```text
Crie uma CONCEPT ART ORIGINAL para Chronicles of Existence, RPG narrativo de fantasia medieval em terceira pessoa. STYLE LOCK: personagens estilizados e expressivos com influência anime ORIGINAL; cenários medievais detalhados; formas modeláveis em Blender/Tripo3D, leitura clara em Unity URP; madeira, pedra, linho, couro e metais verossímeis; paleta natural com acentos #253850, #D6B36A, #648B67, #A86D52, #86C8C9 e #9777B8. Trama = fios entrelaçados e círculos incompletos, NUNCA símbolo de franquia existente. Não copiar animes ou jogos.

ASSET COE-REG-014: Famílias de heráldica regional. Categoria: exploração conceitual de conteúdo FUTURO, sem comprometer o vertical slice. DESCRIÇÃO DIRECIONADA: Símbolos originais para Eldoria e quatro regiões, diferenciados por forma e material. ENTREGA VISUAL: Prancha com cores e P&B. REFINAMENTO CRÍTICO: Não copiar símbolos nacionais reais. Fundo neutro e luz de estúdio para assets e model sheets; cenas narrativas em 16:9 com luz coerente. Não adicionar logotipos, caracteres falsos como informação crucial, marca d'água, dedos extras, partes duplicadas ou detalhes que ocultem silhueta. Se um modelo/referência do MESMO asset estiver anexado, PRESERVE rigorosamente identidade, paleta e geometria. Se a ficha requer múltiplos painéis e houver perda de consistência, produza primeiro a imagem principal e proponha as vistas restantes como gerações sequenciais, não como designs alternativos.
```


---

# 8. PROMPTS DE DESDOBRAMENTO APÓS APROVAÇÃO

## P-TURN — Turnaround de personagem aprovado
```text
Usando a arte ANEXADA e APROVADA como referência absoluta, produza uma model sheet do MESMO personagem: frente, perfil direito, costas e 3/4 neutro, mesma altura aparente, mesma roupa, acessórios, penteado e estrutura facial. Fundo cinza-marfinado liso, luz uniforme e nenhuma pose dramática. Não redesenhe identidade nem adicione poderes. Se não conseguir manter coerência em quatro vistas, gere somente frente agora; faremos as outras vistas separadamente com esta mesma imagem de referência.
```

## P-FACE — Close de identidade
```text
Use a imagem aprovada do MESMO personagem como referência. Faça uma folha facial com retrato neutro frontal, lateral e 3/4, e seis expressões discretas. Mesmos olhos, nariz, mandíbula, pele e cabelo. Preserve idade e identidade. Sem maquiagem ou textura fotográfica não previstas.
```

## P-MAT — Materiais e acabamentos
```text
Desdobre o asset APROVADO em close de materiais, junções, costuras, fechos, rugosidade, interior/verso onde relevante. Não crie uma nova versão visual; mostre como a peça existente seria construída e texturizada para um RPG estilizado 3D.
```

## P-LOD — Simplificação para produção
```text
Mantenha o design aprovado e proponha 3 níveis de detalhe visual (perto, médio, distante) sem mudar cores, silhueta principal nem narrativa material. Identifique quais detalhes viram textura, malha simples ou podem desaparecer a distância. Não insira números de polígonos não verificados na imagem.
```

## P-ENV — Vistas de arquitetura coerentes
```text
A partir da fachada APROVADA, mostre lado, costas, planta esquemática sem números e três peças modulares reaproveitáveis. Preserve posição de portas, janelas, telhado, chaminé e espaço interno. Não crie um prédio diferente.
```

## P-WEAR — Variantes de condição social
```text
Use a MESMA malha-base e a MESMA casa/personagem de referência; altere apenas acabamento, reparos, inventário e organização do cenário para Vida Serena, Normal, Difícil e Ruptura. Não gere quatro arquiteturas ou corpos diferentes. Ruptura pode acrescentar pista narrativa discreta, nunca bônus de poder automático.
```

## P-AGE — Continuidade etária
```text
Use as referências aprovadas de idade anterior do MESMO personagem. Mostre evolução crível para a faixa etária indicada, preservando estrutura dos olhos, nariz, formato do rosto, traço de cabelo e acessório pessoal quando adequado. Não apenas aumente a escala do corpo infantil. Roupas e interesses mudam conforme eventos documentados, sem inventar carreira definitiva.
```

## P-COLOR — Exploração controlada de cor
```text
A partir do design APROVADO, apresente três alternativas cromáticas dentro da paleta de Eryndor, preservando silhueta, tecido, origem regional e leitura de material. Não altere classe, identidade, equipamento ou personalidade.
```

## P-SCENE — Conversão de conceito para cena de gameplay
```text
Transforme a key art APROVADA em imagem de gameplay realista para um RPG estilizado de câmera em terceira pessoa: distância de câmera legível, rotas de navegação, pontos de interesse, iluminação que preserve orientação. Reaproveite exatamente os prédios, props e personagens aprovados. Não desenhe UI falsa sobre a cena.
```

# 9. PROMPT DE AUDITORIA DE CADA LOTE

```text
Você recebeu as imagens geradas e suas fichas COE. Audite cada imagem separadamente: (1) correspondência com a ficha; (2) fidelidade ao STYLE LOCK; (3) consistência com artes aprovadas; (4) escala e modelabilidade 3D; (5) silhueta e legibilidade; (6) defeitos anatômicos ou de geometria; (7) risco de semelhança com franquias; (8) peças/ângulos ainda ausentes. Produza para cada ID status GENERATED, REVIEW, REVISION ou APPROVED (somente se houver aprovação humana explícita), nota descritiva SEM nota numérica autoritária, lista de correções e UM prompt de refinamento exato. Não trate saída da IA como licença comercial ou asset 3D final.
```

# 10. FICHA DE REGISTRO PARA O ACERVO

```yaml
asset_id: COE-PRO-001
titulo: "Criança base A, cinco anos"
categoria: "personagem"
prioridade: P0
arquivo_conceito: "COE-PRO-001_crianca_base_a_cinco_anos_concept_v01.png"
referencias_anexadas: []
status: GENERATED
aprovado_por: null
data_aprovacao: null
prompt_versao: "COE Concept Art Bible v1.0"
modelo_gerador: "registrar nome real usado; não pressupor Astra 6"
origem_e_licenca: "pendente de verificação comercial"
problemas: []
proxima_acao: "revisão de anatomia e identidade"
destino_3d: "Tripo3D / Blender / Unity"
```

# 11. COMO ENTREGAR A UMA OUTRA IA OU MEMBRO DA EQUIPE

Entregue este `.md`, o GDD Mestre vigente e 3 referências-piloto aprovadas. Na primeira interação, diga:

```text
Leia a Bíblia-Mestre de Prompts para Concept Art v1.0 e o GDD vigente. Atue como operador da biblioteca COE. Nunca substitua decisões aprovadas por invenções. Comece pelos IDs P0 e gere UMA imagem por chamada. Ao concluir uma imagem, informe ID, nome de arquivo, fidelidade à referência, inconsistências detectadas e próximo refinamento. Não considere o lote aprovado sem aceite humano. Ao esgotar os P0, siga a ordem P1/P2/P3, sem deixar que arte de expansão atrase Auren.
```

**Fim do documento.**
