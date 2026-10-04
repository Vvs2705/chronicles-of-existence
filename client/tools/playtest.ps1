# Uma sessao do playtest no PC (docs/qa/PLAYTEST.md): guarda o seu save, abre o jogo do zero em modo celular,
# junta o diario e o relatorio em docs\qa\playtests\<data>_<n>\ e devolve o seu save no fim.
# Uso: powershell -ExecutionPolicy Bypass -File client\tools\playtest.ps1 -Jogador 1
#      powershell -ExecutionPolicy Bypass -File client\tools\playtest.ps1 -Restaurar   (so devolve o seu save)
# Sem rede. Nao le o save: so copia/apaga save.json e save.json.bak e copia os sessao_*.txt.
param(
    [int]$Jogador = 0,
    [switch]$Restaurar
)
$ErrorActionPreference = "Stop"
$Client = Split-Path -Parent $PSScriptRoot
$Raiz = Split-Path -Parent $Client
$Exe = Join-Path $Client "Builds\win\COE.exe"
$Dados = Join-Path $env:USERPROFILE "AppData\LocalLow\V-STACK\Chronicles of Existence"
$Diario = Join-Path $Dados "diario"
$Saves = @("save.json", "save.json.bak")   # so estes; o resto da pasta nunca e tocado
$Hoje = Get-Date -Format "yyyy-MM-dd"
$Backup = Join-Path $Dados ("playtest_backup_" + $Hoje)

function Jogo-Aberto { return [bool](Get-Process -Name "COE" -ErrorAction SilentlyContinue) }

function Hash-De([string]$p) {
    if (Test-Path -LiteralPath $p) { return (Get-FileHash -LiteralPath $p).Hash }
    return ""
}

function Apagar-Save {
    foreach ($f in $Saves) {
        $o = Join-Path $Dados $f
        if (Test-Path -LiteralPath $o) { Remove-Item -LiteralPath $o -Force }
    }
}

function Guardar-Save {
    New-Item -ItemType Directory -Force $Backup | Out-Null
    foreach ($f in $Saves) {
        $d = Join-Path $Backup $f
        if (Test-Path -LiteralPath $d) { Remove-Item -LiteralPath $d -Force }
        $o = Join-Path $Dados $f
        if (Test-Path -LiteralPath $o) { Copy-Item -LiteralPath $o -Destination $d }
    }
    "Seu save guardado em $Backup"
}

function Devolver-Save {
    $b = Get-ChildItem -LiteralPath $Dados -Directory -Filter "playtest_backup_*" -ErrorAction SilentlyContinue | Sort-Object Name | Select-Object -Last 1
    if (-not $b) { "Nenhuma copia playtest_backup_* em $Dados. Nada foi mexido."; return }
    Apagar-Save
    foreach ($f in $Saves) {
        $o = Join-Path $b.FullName $f
        if (Test-Path -LiteralPath $o) { Copy-Item -LiteralPath $o -Destination (Join-Path $Dados $f) }
    }
    "Seu save voltou (de $($b.Name))."
}

if (Jogo-Aberto) { "Feche o jogo (COE.exe) antes."; exit 1 }
if ($Restaurar) { Devolver-Save; exit 0 }
if ($Jogador -lt 1) { "Diga o numero do jogador: playtest.ps1 -Jogador 1"; exit 1 }
if (-not (Test-Path -LiteralPath $Exe)) {
    "Jogo nao encontrado: $Exe"
    "Gere com: powershell -ExecutionPolicy Bypass -File client\tools\build_windows.ps1"
    exit 2
}
$Pasta = Join-Path $Raiz ("docs\qa\playtests\" + $Hoje + "_" + $Jogador)
if (Test-Path -LiteralPath $Pasta) { "Ja existe $Pasta. Use outro -Jogador."; exit 1 }

# (a) copia do seu save: uma por dia de rodada
if (-not (Test-Path -LiteralPath $Backup)) {
    Guardar-Save
} elseif ((Test-Path -LiteralPath (Join-Path $Dados "save.json")) -and
          (Hash-De (Join-Path $Dados "save.json")) -ne (Hash-De (Join-Path $Backup "save.json"))) {
    $r = Read-Host "Seu save mudou desde a copia de hoje. E progresso SEU (guardar por cima da copia)? s/N"
    if ($r -match '^[sS]') { Guardar-Save } else { "Copia de hoje mantida; o save atual vai ser apagado." }
} else {
    "Seu save ja esta guardado em $Backup"
}

try {
    # (b) save do zero: so save.json e .bak
    Apagar-Save
    "Save apagado: o jogo abre na tela de titulo."

    # (c) modo celular, igual a run_windows.ps1 -Celular, sem -scene (com -scene pula titulo e nascimento)
    $ArgsJogo = @("-screen-width", "1200", "-screen-height", "540", "-screen-fullscreen", "0", "-popupwindow", "-toque")
    [void](Read-Host "Jogador $Jogador sentado? Enter abre o jogo (o diario comeca a contar)")
    $Inicio = Get-Date
    do {
        "Jogo aberto. No fim, feche pelo proprio jogo (Esc ate a saida)."
        $p = Start-Process -FilePath $Exe -ArgumentList $ArgsJogo -PassThru
        $p.WaitForExit()
        $ult = Get-ChildItem -LiteralPath $Diario -Filter "sessao_*.txt" -ErrorAction SilentlyContinue | Sort-Object Name | Select-Object -Last 1
        if ($ult -and -not (Select-String -LiteralPath $ult.FullName -Pattern "^\d+:\d\d`tfim`t" -Quiet)) {
            "O diario nao tem 'fim': o jogo pode ter caido."
        }
        $r = Read-Host "O jogo fechou. Abrir de novo para o MESMO jogador (caiu no meio)? s/N"
    } while ($r -match '^[sS]')

    # (d) diario(s) desta sessao + relatorio ao lado
    $Novos = @(Get-ChildItem -LiteralPath $Diario -Filter "sessao_*.txt" -ErrorAction SilentlyContinue |
        Where-Object { $_.LastWriteTime -ge $Inicio } | Sort-Object Name)
    if ($Novos.Count -eq 0) {
        "Nenhum diario novo em $Diario."
    } else {
        New-Item -ItemType Directory -Force $Pasta | Out-Null
        $Py = Get-Command py -ErrorAction SilentlyContinue
        if (-not $Py) { $Py = Get-Command python -ErrorAction SilentlyContinue }
        foreach ($f in $Novos) {
            Copy-Item -LiteralPath $f.FullName -Destination $Pasta
            $md = Join-Path $Pasta ($f.BaseName + "_relatorio.md")
            if ($Py) {
                & $Py.Source (Join-Path $PSScriptRoot "diario_report.py") $f.FullName --md $md
                if ($LASTEXITCODE -ne 0) { "Relatorio falhou para $($f.Name) (codigo $LASTEXITCODE)." }
            } else {
                "Sem Python: relatorio nao gerado. Depois: python client\tools\diario_report.py `"$($f.FullName)`" --md `"$md`""
            }
        }
        "Pronto: $Pasta"
        "Falta: abrir o diario e conferir save=novo e nenhum nome; salvar a ficha.md la."
    }
} finally {
    # (e) seu save de volta, mesmo com erro ou Ctrl+C; com o jogo aberto nao (ele gravaria por cima ao fechar)
    if (Jogo-Aberto) { "O jogo ainda esta aberto: feche e rode playtest.ps1 -Restaurar." } else { Devolver-Save }
}
