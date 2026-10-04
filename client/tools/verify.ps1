# Verificacao de um comando (Chronicles of Existence). Orquestra as ferramentas que ja existem; nao duplica nenhuma.
#
# Uso: powershell -ExecutionPolicy Bypass -File client\tools\verify.ps1 [-Modo rapido|completo|android] [-Subst] [-ValidarSemChave]
#   rapido   : validadores Python + EditMode (COE.Tests e COE.EditorTests)                          ~3 min
#   completo : rapido + PlayMode + build Windows (dev) + -roteiro + -roteiro quebrada                ~12 min
#   android  : rapido + build de release Android (AAB) + 16 KB (build_android_release.ps1)
#              sem chave de upload = BLOCKED_CREDENTIAL; -ValidarSemChave valida com a chave de debug (nao publicavel)
#   -Subst   : mapeia o repositorio numa letra livre durante a verificacao (worktree em .claude\worktrees passa de 260
#              caracteres no PackageCache do URP e da 6 erros falsos de import). Desfaz no fim.
#
# Resultado: uma linha por passo (PASS, FAIL, SKIP ou BLOCKED) e codigo de saida 1 se algum passo falhou (0 se nao).
# BLOCKED = o ambiente impede (Unity aberto no projeto, sem chave); nao conta como falha, mas aparece no resumo.
# Logs e XML em client\Builds\verify\. O -roteiro abre uma janela do jogo e usa save proprio (nao toca o do jogador).
param(
    [ValidateSet("rapido", "completo", "android")][string]$Modo = "rapido",
    [switch]$Subst,
    [switch]$ValidarSemChave
)
$ErrorActionPreference = "Stop"
$Unity = "C:\Program Files\Unity\Hub\Editor\6000.3.23f1\Editor\Unity.exe"
$Repo = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
$Resultados = New-Object System.Collections.ArrayList
$Letra = $null

function Anotar([string]$Passo, [string]$Estado, [string]$Detalhe) {
    [void]$Resultados.Add([pscustomobject]@{ Passo = $Passo; Estado = $Estado; Detalhe = $Detalhe })
    "{0,-8} {1,-26} {2}" -f $Estado, $Passo, $Detalhe
}

function Validador([string]$Passo, [string[]]$ArgsPy) {
    $saida = & python @ArgsPy 2>&1 | Out-String
    if ($LASTEXITCODE -eq 0) { Anotar $Passo "PASS" (($saida.Trim() -split "`n")[-1].Trim()) }
    else { Anotar $Passo "FAIL" (($saida.Trim() -split "`n")[-1].Trim()) }
}

# Testes do Unity em batch: le o XML (nao so o codigo de saida) e o log (erro de compilacao).
function Testes([string]$Plataforma) {
    $xml = Join-Path $Saida "$Plataforma.xml"; $log = Join-Path $Saida "$Plataforma.log"
    Remove-Item $xml -ErrorAction SilentlyContinue
    $p = Start-Process -FilePath $Unity -Wait -PassThru -NoNewWindow -ArgumentList @(
        "-batchmode", "-nographics", "-projectPath", "`"$Proj`"", "-runTests", "-testPlatform", $Plataforma,
        "-testResults", "`"$xml`"", "-logFile", "`"$log`"")
    $compilacao = @(Select-String -Path $log -Pattern "error CS\d+" -ErrorAction SilentlyContinue)
    if ($compilacao.Count -gt 0) { Anotar $Plataforma "FAIL" ("erro de compilacao: " + $compilacao[0].Line.Trim()); return }
    if (-not (Test-Path $xml)) { Anotar $Plataforma "FAIL" "sem resultado (Unity saiu com $($p.ExitCode)); ver $log"; return }
    $r = ([xml](Get-Content $xml -Raw)).'test-run'
    $texto = "$($r.passed)/$($r.total) passaram, $($r.failed) falharam, $($r.skipped) pulados"
    if ($r.result -like "Passed*" -and [int]$r.failed -eq 0) { Anotar $Plataforma "PASS" $texto } else { Anotar $Plataforma "FAIL" $texto }
}

function Roteiro([string]$Rota) {
    $nome = if ($Rota) { "roteiro $Rota" } else { "roteiro completo" }
    $exe = Join-Path $Proj "Builds\win\COE.exe"
    if (-not (Test-Path $exe)) { Anotar $nome "SKIP" "sem COE.exe (build falhou)"; return }
    $pasta = Join-Path $env:USERPROFILE "AppData\LocalLow\V-STACK\Chronicles of Existence\roteiro"
    $a = @("-screen-width", "1200", "-screen-height", "540", "-screen-fullscreen", "0", "-toque", "-roteiro")
    if ($Rota) { $a += $Rota }
    $t0 = Get-Date
    $p = Start-Process -FilePath $exe -ArgumentList $a -PassThru
    if (-not $p.WaitForExit(720000)) { $p.Kill(); Anotar $nome "FAIL" "passou de 12 min"; return }
    $txt = Join-Path $pasta "roteiro.txt"
    $ultima = if (Test-Path $txt) { (Get-Content $txt | Select-Object -Last 1) } else { "" }
    Copy-Item $txt (Join-Path $Saida ("roteiro_" + ($(if ($Rota) { $Rota } else { "completo" })) + ".txt")) -ErrorAction SilentlyContinue
    $seg = [int]((Get-Date) - $t0).TotalSeconds
    if ($p.ExitCode -eq 0 -and $ultima -match "ROTEIRO OK") { Anotar $nome "PASS" "$ultima (${seg} s)" }
    else { Anotar $nome "FAIL" "saida $($p.ExitCode): $ultima" }
}

try {
    $Proj = Join-Path $Repo "client"
    if ($Subst) {
        foreach ($l in [char[]]"WVUTSRQPONM") { if (-not (Test-Path "${l}:\")) { $Letra = "${l}:"; break } }
        if (-not $Letra) { "nenhuma letra livre para -Subst"; exit 2 }
        subst $Letra "$Repo" | Out-Null
        $Proj = "$Letra\client"
        "subst $Letra -> $Repo"
    } elseif ($Proj.Length -gt 80) {
        "AVISO: caminho longo ($($Proj.Length) caracteres): o URP pode contar erros falsos de import. Use -Subst."
    }
    $Saida = Join-Path $Proj "Builds\verify"
    New-Item -ItemType Directory -Force $Saida | Out-Null
    $Tools = Join-Path $Proj "tools"

    # 1. Validadores Python (nao pedem Unity; os mesmos do CI)
    Validador "missoes (dados)" @((Join-Path $Repo "content\quests\validate_quests.py"))
    Validador "json (sintaxe e chaves)" @((Join-Path $Tools "check_json.py"))
    Validador "perf_report --autoteste" @((Join-Path $Tools "perf_report.py"), "--autoteste")
    Validador "check_16kb --autoteste" @((Join-Path $Tools "check_16kb.py"), "--autoteste")
    Validador "diario_report --autoteste" @((Join-Path $Tools "diario_report.py"), "--autoteste")
    & python -c "import PIL" 2>$null
    if ($LASTEXITCODE -eq 0) { Validador "silhueta --teste" @((Join-Path $Tools "silhueta.py"), "--teste") }
    else { Anotar "silhueta --teste" "SKIP" "Pillow ausente (pip install pillow)" }

    # 2. Unity: um projeto = uma instancia. Editor aberto no projeto = BLOCKED, nao falha.
    $trava = Join-Path $Proj "Temp\UnityLockfile"
    $aberto = $false
    if (Test-Path $trava) { try { [IO.File]::Open($trava, "Open", "ReadWrite", "None").Close() } catch { $aberto = $true } }
    if (-not (Test-Path $Unity)) { Anotar "Unity" "BLOCKED" "Unity 6000.3.23f1 nao instalado em $Unity" }
    elseif ($aberto) { Anotar "Unity" "BLOCKED" "o projeto esta aberto em outro Unity (feche o Editor)" }
    else {
        Testes "EditMode"
        if ($Modo -eq "completo") {
            Testes "PlayMode"
            & powershell -ExecutionPolicy Bypass -File (Join-Path $Tools "build_windows.ps1") *> (Join-Path $Saida "build_windows.out")
            $ok = Select-String -Path (Join-Path $Proj "Builds\build_win.log") -Pattern "BuildSummary\(win\): result=Succeeded" -Quiet
            if ($ok) { Anotar "build Windows (dev)" "PASS" ((Select-String -Path (Join-Path $Proj "Builds\build_win.log") -Pattern "BuildSummary\(win\)" | Select-Object -Last 1).Line.Trim()) }
            else { Anotar "build Windows (dev)" "FAIL" "ver Builds\build_win.log" }
            if ($ok) { Roteiro ""; Roteiro "quebrada" } else { Anotar "roteiro" "SKIP" "sem build" }
        }
        if ($Modo -eq "android") {
            $a = @("-ExecutionPolicy", "Bypass", "-File", (Join-Path $Tools "build_android_release.ps1"))
            if ($ValidarSemChave) { $a += "-AssinaturaDeDebug" }
            & powershell @a *> (Join-Path $Saida "build_android_release.out")
            $c = $LASTEXITCODE
            $fim = (Get-Content (Join-Path $Saida "build_android_release.out") | Where-Object { $_.Trim() } | Select-Object -Last 1)
            if ($c -eq 0) { Anotar "release Android (AAB)" "PASS" $fim }
            elseif ($c -eq 3) { Anotar "release Android (AAB)" "BLOCKED" "BLOCKED_CREDENTIAL: sem chave de upload (ou -ValidarSemChave)" }
            else { Anotar "release Android (AAB)" "FAIL" "saida ${c}: $fim" }
        }
    }
} finally {
    if ($Letra) { subst $Letra /d | Out-Null; "subst $Letra desfeito" }
}

""
$falhas = @($Resultados | Where-Object { $_.Estado -eq "FAIL" }).Count
$bloq = @($Resultados | Where-Object { $_.Estado -eq "BLOCKED" }).Count
"verify ($Modo): $(@($Resultados | Where-Object { $_.Estado -eq 'PASS' }).Count) PASS, $falhas FAIL, $(@($Resultados | Where-Object { $_.Estado -eq 'SKIP' }).Count) SKIP, $bloq BLOCKED"
if ($falhas -gt 0) { exit 1 }
exit 0
