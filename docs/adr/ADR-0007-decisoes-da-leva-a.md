# ADR-0007 — Decisões de produto da leva A (conteúdo da T012)

- **ID:** ADR-0007
- **Data:** 2026-09-30
- **Estado:** APROVADO (decisão delegada pelo Vinicius ao coordenador em 2026-09-30, "siga com tudo que é necessário", sobre as recomendações da leitura de estado; todas reversíveis)
- **Depende de:** ADR-0004, ADR-0005, ADR-0006
- **Documentos afetados:** `docs/PROJETO.md` §6, `docs/design/SLICE_A_PRIMEIRA_EXISTENCIA.md`, `content/quests/**`, `client/Assets/_COE/Resources/strings.pt-BR.json`, `client/Assets/_COE/Scripts/{Core,Quest,NPC,Dialogue,LifeSystem,World}`

## Contexto

A leitura de estado de 2026-09-30 mostrou que a fiação da T012 existe, mas o conteúdo não pode ser escrito sem seis decisões de produto que estavam pendentes no `PROJETO.md` §6. Este ADR as registra, com os ids que o código e o conteúdo passam a compartilhar.

## Decisões

**1. Como o dia passa.** O período (manhã → tarde → noite) avança **um passo** em dois casos, e só neles: (a) quando uma missão é concluída; (b) quando o jogador escolhe **Descansar** em casa (`casa_familia`). Não existe relógio de tempo real (mantém o que `TimeOfDayCycle` já declara). Rejeitado: avançar por objetivo (o dia giraria rápido demais) e timer (o `SLICE` diz "sem timer").

**2. Rótulo do terceiro destino.** O rótulo exibido passa a ser **"Vida Árdua"**. O id `dificil` é publicado e não muda. Motivo: o B02 e o ADR-0004 proíbem adjetivo de dificuldade no nome. Os documentos-fonte (GDD, dossiê, prompt-mestre) não são editados; vale este ADR.

**3. Ausência de Nilo.** O evento de vida **`evento.nilo_desapareceu`** é gravado junto da conclusão da Q-04. Enquanto ele valer, Nilo não aparece em Auren (a rotina dele aponta para a âncora-sentinela `ausente`, que a cena não desenha). A Q-03 ("O Cesto Perdido"), que pede Nilo na trilha, **é encerrada** nesse momento se ainda estiver aberta, do mesmo jeito que o salto encerra as opcionais. O retorno de Nilo depois do salto (B14) é conteúdo da leva B.

**4. Q-04 na reputação.** Aprovado como implementado: promessa cumprida leva a confiança de Sera e Nilo a +20; quebrada, a −20.

**5. Limiar e aparência.** O slice terá um **Limiar mínimo** (uma cena, a fala de Aethron, o símbolo) — entra na leva B. A **aparência** na personalização sai do slice (FUTURO); o B04 fica só com o nome.

**6. "Acordar" (Q-01).** O objetivo `acordar` se cumpre sozinho quando a Q-01 começa. O primeiro ato do jogador é falar com a família.

**7. ADR-0004 e save editado.** O ADR-0004 deixa de prometer que um save editado não troca o destino. O jogo **detecta id inválido e registra**; não promete anti-cheat em save local.

**8. Gamepad.** É conveniência de desenvolvimento, não suporte oficial (responde a pergunta aberta do ADR-0006). A conversa não precisa ser navegável por gamepad no slice.

**9. Padrões de trabalho mantidos** (a confirmar pelo idealizador antes de publicar): id `br.com.vstack.coe`, API mínima 26, Unity 6000.3.23f1, `StringsLoader` atual, correr pela borda do joystick.

## Fora deste ADR (continuam com o idealizador)

Aparelho mínimo de referência; destino do acervo de concept e plano do Tripo3D; estilo visual; Tripo Bridge; público-alvo e conta do Play.

## Consequências

- Concluir missão muda onde os NPCs estão: todo objetivo com NPC precisa continuar alcançável em qualquer período (a rotina sempre dá uma âncora; `ausente` só vale para Nilo desaparecido).
- O teste `NpcCatalogTests.Rotina_SoCondicionaEmEventoQueONpcTestemunha`, que se ignorava por falta de rotina condicional, passa a rodar.
- `content/quests/q04_uma_promessa.json` e `QuestCatalog` ganham o evento novo nos dois lados (paridade do ADR-0005).
