---
name: verify
description: Verificacao de um comando do COE (validadores, testes do Unity, build e simulacao da partida) com resumo PASS/FAIL/SKIP/BLOCKED. Use antes de commit, ao fechar um bloco e quando alguem perguntar "passa tudo?".
---

# Verificar o COE

1. Feche o Editor do Unity se ele estiver com `client/` aberto (um projeto = uma instancia; aberto = BLOCKED).
2. Escolha o modo:
   - `rapido` (~3 min): validadores Python + EditMode. Depois de qualquer mudanca de codigo.
   - `completo` (~12 min): rapido + PlayMode + build Windows (dev) + `-roteiro` + `-roteiro quebrada`. Antes de PR, merge ou fim de bloco. Abre uma janela do jogo por uns minutos (save proprio do robo; nao toca o do jogador).
   - `android`: rapido + release AAB + checagem de 16 KB. Sem chave de upload = BLOCKED_CREDENTIAL; `-ValidarSemChave` valida com a chave de debug (nao publicavel). Troca a plataforma do Library para Android (o proximo build de PC reimporta).
3. Rode (worktree em `.claude\worktrees\...`: acrescente `-Subst` para nao contar erro falso de import do URP):

```powershell
powershell -ExecutionPolicy Bypass -File client\tools\verify.ps1 -Modo rapido
```

4. Leia o resumo, nao so o codigo de saida: `exit 1` = algum FAIL. Logs e XML em `client\Builds\verify\`. FAIL de teste: abra o XML do passo (`EditMode.xml`, `PlayMode.xml`) e procure `result="Failed"`.
5. Relate o resultado com os numeros reais (ex.: "EditMode 594/594, PlayMode 31/31, roteiro OK 173 s"). Nunca declare verde sem ter rodado.

Depois de build, `git status` limpo e o esperado (cenas regeradas iguais byte a byte). Diff em `.unity`/`ProjectSettings` sem mudanca no gerador = investigar.
