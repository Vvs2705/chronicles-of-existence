# Build Android de desenvolvimento (Chronicles of Existence) em batch mode -> client\Builds\android\COE.apk
# (IL2CPP, ARM64, assinado com a chave de debug). O alvo do jogo e Android (ADR-0006); Windows e so ferramenta de dev.
# Uso: powershell -ExecutionPolicy Bypass -File client\tools\build_android.ps1
# A primeira rodada reimporta o projeto inteiro para Android: demora bem mais que as seguintes.
# -Testadores: APK sem modo de desenvolvimento para amigos testarem -> client\Builds\android\COE_teste.apk
param([switch]$Testadores)
$ErrorActionPreference = "Stop"
$Unity = "C:\Program Files\Unity\Hub\Editor\6000.3.23f1\Editor\Unity.exe"
$Proj = Split-Path -Parent $PSScriptRoot
$Builds = Join-Path $Proj "Builds\android"
$Log = Join-Path $Proj "Builds\build_android.log"
$Apk = Join-Path $Builds $(if ($Testadores) { "COE_teste.apk" } else { "COE.apk" })
$Metodo = if ($Testadores) { "COE.EditorTools.BuildAndroid.BuildTestadores" } else { "COE.EditorTools.BuildAndroid.Build" }
$Modulo = Join-Path (Split-Path -Parent $Unity) "Data\PlaybackEngines\AndroidPlayer"
if (-not (Test-Path $Modulo)) {
    "Modulo Android do Unity ausente em $Modulo. Instale pelo Unity Hub (Android Build Support, SDK/NDK, OpenJDK)."
    exit 2
}
New-Item -ItemType Directory -Force $Builds | Out-Null
# Vale so o build atual: o APK desta rodada substitui qualquer APK anterior (COE.apk ou COE_teste.apk).
Get-ChildItem $Builds -Filter *.apk | Remove-Item -Force

# Active Input Handling = Both (2): sem API publica, entao troca direto no ProjectSettings.asset (igual ao build_windows.ps1).
$Ps = Join-Path $Proj "ProjectSettings\ProjectSettings.asset"
$Text = [IO.File]::ReadAllText($Ps)
if ($Text -match "activeInputHandler: [01]") {
    [IO.File]::WriteAllText($Ps, ($Text -replace "activeInputHandler: [01]", "activeInputHandler: 2"))
}

# Start-Process no PowerShell 5.1 junta os argumentos sem aspas: caminho com espaco (MEUS PROJETOS) vai entre aspas a mao.
$Inicio = Get-Date
$UArgs = @("-batchmode", "-nographics", "-projectPath", "`"$Proj`"", "-buildTarget", "Android",
           "-executeMethod", $Metodo, "-logFile", "`"$Log`"", "-quit")
$P = Start-Process -FilePath $Unity -ArgumentList $UArgs -PassThru -NoNewWindow
$null = $P.Handle; $P.WaitForExit()  # so o Unity: -Wait esperaria tambem o VBCSCompiler que ele deixa vivo (verify parado 15 min em 2026-10-04); o Handle guarda o ExitCode no PS 5.1
# Simbolos Burst "DoNotShip": o Unity recria a cada build e ninguem usa; nao deixa acumular.
Get-ChildItem $Builds -Directory -Filter *_BurstDebugInformation_DoNotShip | Remove-Item -Recurse -Force
if (Test-Path $Log) {
    "---- build_android.log (ultimas 20 linhas) ----"
    Get-Content $Log -Tail 20
    Select-String -Path $Log -Pattern "BuildSummary\(android\)" | Select-Object -Last 1 | ForEach-Object { "---- " + $_.Line }
}
# so vale o APK escrito nesta rodada: um COE.apk velho no disco nao conta como sucesso
if ((Test-Path $Apk) -and ((Get-Item $Apk).LastWriteTime -ge $Inicio)) {
    "APK: $Apk"
    "Instale e rode num aparelho USB com: powershell -ExecutionPolicy Bypass -File client\tools\run_android.ps1"
} else {
    "APK NAO GERADO nesta rodada (Unity exit code $($P.ExitCode)). Leia $Log."
    if ($P.ExitCode -eq 0) { exit 1 }
}
exit $P.ExitCode
