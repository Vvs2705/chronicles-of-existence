# Build de RELEASE Android (Chronicles of Existence): AAB, IL2CPP, ARM64, nao-Development, versionName/versionCode
# explicitos, API minima 26 e alvo 36 (COE.EditorTools.BuildAndroid.BuildRelease). NAO publica nada.
#
# Chave de upload SO por variavel de ambiente, nunca no repositorio:
#   $env:COE_KEYSTORE = "C:\caminho\fora\do\repo\coe-upload.jks"; $env:COE_KEYSTORE_PASS = "..."
#   $env:COE_KEY_ALIAS = "..."; $env:COE_KEY_PASS = "..."
# Sem elas: sai com 3 (BLOCKED_CREDENTIAL) antes de abrir o Unity.
# -AssinaturaDeDebug: valida o caminho inteiro com a chave de debug da Unity (sai *_debugsign.aab, NAO publicavel).
#
# Uso: powershell -ExecutionPolicy Bypass -File client\tools\build_android_release.ps1 [-Versao 0.1.0] [-Codigo N] [-AssinaturaDeDebug]
#   -Codigo 0 (padrao) = numero de commits do HEAD (sobe sozinho a cada commit; reproduzivel no mesmo commit).
# Depois do build: confere o AAB (escrito nesta rodada, so arm64-v8a, com libil2cpp) e o alinhamento de 16 KB (check_16kb.py).
# Vale so o build atual: AAB anterior em Builds\android e apagado antes.
param([string]$Versao = "0.1.0", [int]$Codigo = 0, [switch]$AssinaturaDeDebug)
$ErrorActionPreference = "Stop"
$Unity = "C:\Program Files\Unity\Hub\Editor\6000.3.23f1\Editor\Unity.exe"
$Proj = Split-Path -Parent $PSScriptRoot
$Builds = Join-Path $Proj "Builds\android"
$Log = Join-Path $Proj "Builds\build_android_release.log"

$temChave = $env:COE_KEYSTORE -and (Test-Path $env:COE_KEYSTORE) -and $env:COE_KEYSTORE_PASS -and $env:COE_KEY_ALIAS -and $env:COE_KEY_PASS
if (-not $temChave -and -not $AssinaturaDeDebug) {
    "BLOCKED_CREDENTIAL: chave de upload ausente (COE_KEYSTORE, COE_KEYSTORE_PASS, COE_KEY_ALIAS, COE_KEY_PASS)."
    "Para validar o caminho sem publicar: -AssinaturaDeDebug."
    exit 3
}
$Modulo = Join-Path (Split-Path -Parent $Unity) "Data\PlaybackEngines\AndroidPlayer"
if (-not (Test-Path $Modulo)) { "Modulo Android do Unity ausente em $Modulo (Android Build Support pelo Hub)."; exit 2 }
if ($Codigo -le 0) {
    $Codigo = [int](git -C $Proj rev-list --count HEAD)
    if ($Codigo -le 0) { "versionCode: git rev-list falhou; passe -Codigo."; exit 2 }
}
if ($Versao -notmatch '^\d+\.\d+\.\d+$') { "versionName '$Versao' fora do formato N.N.N."; exit 2 }

New-Item -ItemType Directory -Force $Builds | Out-Null
Get-ChildItem $Builds -Filter *.aab | Remove-Item -Force

# Active Input Handling = Both (2): sem API publica (igual ao build_android.ps1).
$Ps = Join-Path $Proj "ProjectSettings\ProjectSettings.asset"
$Text = [IO.File]::ReadAllText($Ps)
if ($Text -match "activeInputHandler: [01]") { [IO.File]::WriteAllText($Ps, ($Text -replace "activeInputHandler: [01]", "activeInputHandler: 2")) }

# O ProjectSettings.asset volta byte a byte depois do build: nada desta rodada (versao, alvo, caminho da chave) fica no git.
$PsCopia = Join-Path $env:TEMP ("coe_ProjectSettings_" + [guid]::NewGuid().ToString("N") + ".asset")
Copy-Item $Ps $PsCopia

$Inicio = Get-Date
$UArgs = @("-batchmode", "-nographics", "-projectPath", "`"$Proj`"", "-buildTarget", "Android",
           "-executeMethod", "COE.EditorTools.BuildAndroid.BuildRelease", "-coeVersao", $Versao, "-coeCodigo", $Codigo,
           "-logFile", "`"$Log`"", "-quit")
if ($AssinaturaDeDebug -and -not $temChave) { $UArgs += "-coeDebugSign" }
$P = Start-Process -FilePath $Unity -ArgumentList $UArgs -Wait -PassThru -NoNewWindow
Copy-Item $PsCopia $Ps -Force
Remove-Item $PsCopia -Force
Get-ChildItem $Builds -Directory -Filter *_BurstDebugInformation_DoNotShip | Remove-Item -Recurse -Force
if ($P.ExitCode -eq 3) { "BLOCKED_CREDENTIAL (o Unity recusou: ver $Log)."; exit 3 }
Select-String -Path $Log -Pattern "BuildSummary\(android-release\)" | Select-Object -Last 1 | ForEach-Object { "---- " + $_.Line }

$Aab = Get-ChildItem $Builds -Filter *.aab | Where-Object { $_.LastWriteTime -ge $Inicio } | Select-Object -First 1
if (-not $Aab) { "AAB NAO GERADO nesta rodada (Unity exit code $($P.ExitCode)). Leia $Log."; if ($P.ExitCode -eq 0) { exit 1 }; exit $P.ExitCode }

# Validacao do artefato: so arm64-v8a, com o IL2CPP; depois o alinhamento de 16 KB das bibliotecas.
Add-Type -AssemblyName System.IO.Compression.FileSystem
$Zip = [IO.Compression.ZipFile]::OpenRead($Aab.FullName)
try { $Nomes = @($Zip.Entries | ForEach-Object { $_.FullName }) } finally { $Zip.Dispose() }
$Il2cpp = $Nomes | Where-Object { $_ -like "base/lib/arm64-v8a/libil2cpp.so" }
$Outras = $Nomes | Where-Object { $_ -like "base/lib/*" -and $_ -notlike "base/lib/arm64-v8a/*" }
"AAB: $($Aab.FullName) ($([Math]::Round($Aab.Length / 1MB, 1)) MB, versao $Versao, codigo $Codigo)"
if (-not $Il2cpp) { "FALHOU: AAB sem base/lib/arm64-v8a/libil2cpp.so (IL2CPP ARM64)."; exit 1 }
if ($Outras) { "FALHOU: ABI alem de arm64-v8a no AAB: $($Outras -join ', ')"; exit 1 }
python (Join-Path $PSScriptRoot "check_16kb.py") $Aab.FullName
if ($LASTEXITCODE -ne 0) { "FALHOU: alinhamento de 16 KB (check_16kb.py)."; exit 1 }
if (-not $temChave) { "ATENCAO: assinado com a chave de DEBUG da Unity. Valida o caminho; nao serve para a loja." }
exit 0
