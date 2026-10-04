---
name: release-check
description: Confere se o COE esta pronto para um release Android (AAB, IL2CPP, ARM64, versao, API alvo/minima, 16 KB, chave de upload, prazos da Play) SEM publicar nada. Use quando falarem em release, loja, Play Store, AAB ou versao de lancamento.
---

# Prontidao de release Android (sem publicar)

Nunca publica, nunca cria conta, nunca aceita termos da Play. A chave de upload e do idealizador e mora fora do repositorio.

1. Chave: as variaveis `COE_KEYSTORE`, `COE_KEYSTORE_PASS`, `COE_KEY_ALIAS`, `COE_KEY_PASS` existem e o arquivo existe? Sem elas, o release real e BLOCKED_CREDENTIAL (nao invente chave). Confirme que nenhum `*.keystore`, `*.jks` ou senha esta no git (`git ls-files`, `.gitignore`).
2. Build:

```powershell
powershell -ExecutionPolicy Bypass -File client\tools\build_android_release.ps1 -Versao 0.1.0
```

   Sem chave, para validar so o caminho: `-AssinaturaDeDebug` (AAB `*_debugsign`, nao publicavel). O script confere AAB desta rodada, so `arm64-v8a`, `libil2cpp.so` presente e alinhamento de 16 KB (`client/tools/check_16kb.py`), e devolve o `ProjectSettings.asset` como estava.
3. Confira no log `client\Builds\build_android_release.log` a linha `BuildSummary(android-release)`: Succeeded, versionName, versionCode (padrao = numero de commits), assinatura.
4. Pendencias de loja que o build nao resolve (docs/PROJETO.md §6 "Loja"): icone, declaracao de publico (Familias/LGPD/ECA, decisao 12), regra de testadores, verificacao de desenvolvedor, prazos de target API. Liste o que falta; nao decida produto.
5. Relate: o que passou, o que esta BLOCKED e por que, e o caminho do AAB.
