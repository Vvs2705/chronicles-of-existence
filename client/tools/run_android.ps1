# Instala o APK de desenvolvimento num aparelho Android ligado por USB, abre o jogo, tira capturas da tela do
# aparelho e mostra do logcat so os erros da Unity.
# Uso: powershell -ExecutionPolicy Bypass -File client\tools\run_android.ps1 -Seconds 25 -Shots 5 [-Scene Auren]
# Antes: Opcoes do desenvolvedor > Depuracao USB ativada, aviso "Permitir depuracao USB" aceito, tela desbloqueada.
# Saida: 0 ok; 1 falha de instalacao/abertura, jogo morreu durante a rodada ou nenhuma captura saiu;
# 2 sem aparelho/APK/adb ou parametro invalido.
param(
    [int]$Seconds = 25,
    [int]$Shots = 5,
    [string]$Serial = "",   # varios aparelhos: o serial do `adb devices`; vazio = o primeiro
    [string]$Package = "br.com.vstack.coe",   # = ProjectSetup.AppId
    [string]$Scene = "",    # abre direto nesta cena (DevSceneArg), ex.: Auren; vazio = a primeira do Build Settings
    [switch]$KeepOpen
)
# Continue, nao Stop: no PowerShell 5.1 o stderr do adb ("daemon started") vira erro fatal com Stop. Os codigos
# de saida do adb sao checados a mao.
$ErrorActionPreference = "Continue"
$Proj = Split-Path -Parent $PSScriptRoot
$Apk = Join-Path $Proj "Builds\android\COE.apk"
$ShotDir = Join-Path $Proj "Builds\android\shots"
$Adb = Join-Path $env:LOCALAPPDATA "Android\Sdk\platform-tools\adb.exe"
if (-not (Test-Path $Adb)) { $Adb = "C:\Program Files\Unity\Hub\Editor\6000.3.23f1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools\adb.exe" }
if (-not (Test-Path $Adb)) { "adb nao encontrado (nem no SDK do Android nem no da Unity)."; exit 2 }
if (-not (Test-Path $Apk)) { "APK ausente: $Apk. Rode client\tools\build_android.ps1 antes."; exit 2 }
# o nome vai para o shell do aparelho: so letras, numeros e _ (nome de cena do Build Settings)
if ($Scene -and $Scene -notmatch '^[A-Za-z0-9_]+$') { "Cena invalida: '$Scene' (so letras, numeros e _)."; exit 2 }

$lista = @(& $Adb devices | Select-Object -Skip 1 | Where-Object { $_ -match "\S+\s+\S+" })
$prontos = @($lista | Where-Object { $_ -match "^(\S+)\s+device$" } | ForEach-Object { ($_ -split "\s+")[0] })
if ($prontos.Count -eq 0) {
    if ($lista | Where-Object { $_ -match "\sunauthorized$" }) {
        "Aparelho conectado mas NAO autorizado: aceite 'Permitir depuracao USB' na tela do celular e rode de novo."
    } else {
        "Nenhum aparelho Android conectado. Ligue o celular por USB com Opcoes do desenvolvedor > Depuracao USB ativada."
    }
    exit 2
}
if (-not $Serial) { $Serial = $prontos[0] }
if ($prontos -notcontains $Serial) { "Aparelho $Serial nao esta pronto. Prontos: $($prontos -join ', ')"; exit 2 }
if ($prontos.Count -gt 1) { "Varios aparelhos ($($prontos -join ', ')); usando $Serial. Escolha com -Serial." }
$modelo = (& $Adb -s $Serial shell getprop ro.product.model | Out-String).Trim()
$api = (& $Adb -s $Serial shell getprop ro.build.version.sdk | Out-String).Trim()
"Aparelho: $Serial ($modelo, API $api)"

"Instalando $Apk ..."
$inst = (& $Adb -s $Serial install -r $Apk | Out-String).Trim()
$inst
if ($LASTEXITCODE -ne 0 -or $inst -notmatch "Success") {
    "Xiaomi/POCO com INSTALL_FAILED_USER_RESTRICTED: ative Opcoes do desenvolvedor > Instalar via USB e toque em Instalar no aviso do celular (tela desbloqueada)."
    "FALHA na instalacao. Se for INSTALL_FAILED_UPDATE_INCOMPATIBLE (assinatura diferente), desinstale a versao antiga"
    "a mao com: `"$Adb`" -s $Serial uninstall $Package  (apaga o save do jogo no aparelho)."
    exit 1
}

& $Adb -s $Serial logcat -c   # logcat limpo: so entra o que esta rodada gerar
& $Adb -s $Serial shell input keyevent KEYCODE_WAKEUP | Out-Null   # tela apagada = captura preta
if ($Scene) {
    # A cena vai no extra de intent "unity", que a activity da Unity repassa como linha de comando (DevSceneArg le o
    # extra). O monkey nao passa extra, entao e am start com a activity de LAUNCHER resolvida no proprio aparelho:
    # Unity 6000.3 com androidApplicationEntry: 2 (ProjectSettings) = GameActivity ->
    # com.unity3d.player.UnityPlayerGameActivity; se o projeto voltar para Activity, vira UnityPlayerActivity sozinho.
    # ponytail: A VALIDAR NO APARELHO (resolve-activity existe desde a API 24; o minimo do COE e 26).
    $res = @(& $Adb -s $Serial shell cmd package resolve-activity --brief -c android.intent.category.LAUNCHER $Package | Where-Object { $_ -match "/" })
    $comp = if ($res.Count -gt 0) { $res[-1].Trim() } else { "$Package/com.unity3d.player.UnityPlayerGameActivity" }
    # uma string so: o adb junta os argumentos e o shell do aparelho re-separa; as aspas simples mantem "-scene X" junto
    $abrir = (& $Adb -s $Serial shell "am start -S -n $comp -e unity '-scene $Scene'" | Out-String)
    if ($LASTEXITCODE -ne 0 -or $abrir -match "Error") { "FALHA ao abrir $comp na cena $Scene :"; $abrir; exit 1 }
    "Aberto: $comp (cena $Scene)"
} else {
    $abrir = (& $Adb -s $Serial shell monkey -p $Package -c android.intent.category.LAUNCHER 1 | Out-String)
    if ($abrir -notmatch "Events injected: 1") { "FALHA ao abrir $Package :"; $abrir; exit 1 }
    "Aberto: $Package"
}

New-Item -ItemType Directory -Force $ShotDir | Out-Null
$stamp = Get-Date -Format "yyyy-MM-dd_HHmm"
$every = [Math]::Max(1, [int]($Seconds / [Math]::Max(1, $Shots)))
$taken = @()
for ($i = 1; $i -le $Shots; $i++) {
    Start-Sleep -Seconds $every
    $out = Join-Path $ShotDir ("shot_" + $stamp + "_" + $i + ".png")
    # PNG e binario: o ">" do PowerShell 5.1 regrava como texto UTF-16 e corrompe. Copia o stdout cru do adb.
    $psi = New-Object System.Diagnostics.ProcessStartInfo
    $psi.FileName = $Adb
    $psi.Arguments = "-s $Serial exec-out screencap -p"
    $psi.UseShellExecute = $false
    $psi.RedirectStandardOutput = $true
    $proc = [System.Diagnostics.Process]::Start($psi)
    $fs = [IO.File]::Create($out)
    $proc.StandardOutput.BaseStream.CopyTo($fs)
    $fs.Close(); $proc.WaitForExit()
    if ((Get-Item $out).Length -gt 0) { $taken += $out; "foto $i/$Shots -> $out" } else { "foto $i/$Shots falhou (arquivo vazio)" }
}

$pid2 = (& $Adb -s $Serial shell pidof $Package | Out-String).Trim()
$morreu = -not $pid2
if ($morreu) { "o jogo NAO esta mais rodando (fechou ou crashou durante a rodada)." }
if (-not $KeepOpen) { & $Adb -s $Serial shell am force-stop $Package; "jogo fechado." }

# so as tags da Unity (e CRASH, onde cai o crash nativo do IL2CPP), filtradas por erro. Formato brief ("E/Unity (pid): ...")
# para a prioridade E/F aparecer no comeco da linha: Debug.LogError nem sempre tem "Error" no texto.
$errs = @(& $Adb -s $Serial logcat -d -v brief -s Unity CRASH | Select-String -Pattern "^[EF]/|Exception|Error" | Select-Object -Last 20)
if ($errs.Count -gt 0) { "---- erros da Unity no logcat ----"; $errs | ForEach-Object { $_.Line } } else { "logcat da Unity sem erro." }
"fotos: $($taken.Count) em $ShotDir"
if ($morreu) { exit 1 }
if ($Shots -gt 0 -and $taken.Count -eq 0) { "nenhuma captura saiu: a rodada nao prova que o jogo abriu."; exit 1 }
exit 0
