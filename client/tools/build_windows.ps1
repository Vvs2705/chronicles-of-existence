# Build Windows (Chronicles of Existence) em batch mode, para jogar no PC.
# Uso: powershell -ExecutionPolicy Bypass -File client\tools\build_windows.ps1
$ErrorActionPreference = "Stop"
$Unity = "C:\Program Files\Unity\Hub\Editor\6000.3.23f1\Editor\Unity.exe"
$Proj = Split-Path -Parent $PSScriptRoot
$Builds = Join-Path $Proj "Builds\win"
$Log = Join-Path $Proj "Builds\build_win.log"
$Exe = Join-Path $Builds "COE.exe"
New-Item -ItemType Directory -Force $Builds | Out-Null

# Active Input Handling = Both (2): sem API publica, entao troca direto no ProjectSettings.asset.
$Ps = Join-Path $Proj "ProjectSettings\ProjectSettings.asset"
$Text = [IO.File]::ReadAllText($Ps)
if ($Text -match "activeInputHandler: [01]") {
    [IO.File]::WriteAllText($Ps, ($Text -replace "activeInputHandler: [01]", "activeInputHandler: 2"))
}

$Args = @("-batchmode", "-nographics", "-projectPath", "`"$Proj`"", "-buildTarget", "Win64",
          "-executeMethod", "COE.EditorTools.BuildWindows.Build", "-logFile", "`"$Log`"", "-quit")
$P = Start-Process -FilePath $Unity -ArgumentList $Args -PassThru -NoNewWindow
$null = $P.Handle; $P.WaitForExit()  # so o Unity: -Wait esperaria tambem o VBCSCompiler que ele deixa vivo (verify parado 15 min em 2026-10-04); o Handle guarda o ExitCode no PS 5.1
# O build ja sobrescreve Builds\win no lugar (sem copia); so os simbolos Burst "DoNotShip" sobrariam.
Get-ChildItem $Builds -Directory -Filter *_BurstDebugInformation_DoNotShip | Remove-Item -Recurse -Force
"---- build_win.log (ultimas 20 linhas) ----"
Get-Content $Log -Tail 20
# O -logFile e reescrito a cada rodada: o BuildSummary dele e desta build. Nao da para olhar a data do COE.exe:
# em build incremental o Unity nao recopia o launcher (ele e igual entre builds).
$Ok = Select-String -Path $Log -Pattern "BuildSummary\(win\): result=Succeeded" -Quiet
if ($Ok -and (Test-Path $Exe)) {
    "EXE: $Exe"
    "Jogue com: powershell -File client\tools\run_windows.ps1"
} else {
    "EXE NAO GERADO nesta rodada (Unity exit code $($P.ExitCode)). Leia $Log."
    if ($P.ExitCode -eq 0) { exit 1 }
}
exit $P.ExitCode
