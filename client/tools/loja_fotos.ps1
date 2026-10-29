# Fotos das capturas da Play (docs/LOJA.md sec. 5) por idioma, com o robo -roteiro na build de Windows de desenvolvimento.
# Molde: client/tools/loja_fotos.sh do Dungeon Courier. Cada idioma e uma rodada inteira do -roteiro (~5 min) em 1920x1080
# sem borda; as fotos NN_<etapa>.png vao para client\Builds\loja_fotos\<lang>\. Depois: python -I client\tools\loja_assets.py
# Uso: powershell -ExecutionPolicy Bypass -File client\tools\loja_fotos.ps1 [-Langs pt-BR,en,es,id] [-Width 1920 -Height 1080]
# Precisa de client\Builds\win\COE.exe (client\tools\build_windows.ps1; o -roteiro so roda em build de desenvolvimento).
# ponytail: as fotos saem da rota completa do robo (a mesma do verify). Foto com arte PROTOTIPO nao vai para a loja
# (CONTRATO_V1 sec. 3): rodar de novo quando a arte final estiver no lugar.
param(
    [string[]]$Langs = @("pt-BR", "en", "es", "id"),
    [int]$Width = 1920,
    [int]$Height = 1080
)
$ErrorActionPreference = "Stop"
$Proj = Split-Path -Parent $PSScriptRoot
$Exe = Join-Path $Proj "Builds\win\COE.exe"
$Saida = Join-Path $Proj "Builds\loja_fotos"
$Pasta = Join-Path $env:USERPROFILE "AppData\LocalLow\V-STACK\Chronicles of Existence\roteiro"
if (-not (Test-Path $Exe)) { "EXE ausente: $Exe. Rode client\tools\build_windows.ps1 antes."; exit 2 }

$falhas = 0
foreach ($L in $Langs) {
    if (Test-Path $Pasta) { Remove-Item (Join-Path $Pasta "*.png") -Force -ErrorAction SilentlyContinue }
    $a = @("-screen-width", "$Width", "-screen-height", "$Height", "-screen-fullscreen", "0", "-popupwindow", "-toque", "-roteiro", "-lang", $L)
    $p = Start-Process -FilePath $Exe -ArgumentList $a -PassThru
    if (-not $p.WaitForExit(720000)) { $p.Kill(); "FALHOU $L (passou de 12 min)"; $falhas++; continue }
    $txt = Join-Path $Pasta "roteiro.txt"
    $ultima = if (Test-Path $txt) { (Get-Content $txt | Select-Object -Last 1) } else { "" }
    $dest = Join-Path $Saida $L
    if (Test-Path $dest) { Remove-Item $dest -Recurse -Force }
    New-Item -ItemType Directory -Force $dest | Out-Null
    Copy-Item (Join-Path $Pasta "*.png") $dest -ErrorAction SilentlyContinue
    $n = @(Get-ChildItem $dest -Filter *.png).Count
    if ($p.ExitCode -eq 0 -and $ultima -match "ROTEIRO OK") { "OK $L ($n fotos)" } else { "FALHOU $L (saida $($p.ExitCode): $ultima; $n fotos)"; $falhas++ }
}
"Fotos em $Saida. Agora: python -I client\tools\loja_assets.py"
exit $falhas
