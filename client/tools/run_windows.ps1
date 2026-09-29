# Roda a build de Windows em janela e tira fotos SO da janela do jogo (nao da area de trabalho).
# Uso: powershell -ExecutionPolicy Bypass -File client\tools\run_windows.ps1 -Seconds 25 -Shots 5
# Modo celular (simula o POCO F4 no PC ate a arte chegar): run_windows.ps1 -Celular -Scene Auren -KeepOpen
param(
    [int]$Seconds = 25,
    [int]$Shots = 5,
    [int]$Width = 1280,
    [int]$Height = 720,
    [switch]$KeepOpen,
    [string]$Scene = "",   # abre a build direto nessa cena (ex.: Auren); vazio = primeira do Build Settings
    [switch]$AutoWalk,   # passa -autowalk para a build: o personagem anda em quadrado sozinho (captura)
    [switch]$Celular,   # janela 20:9 paisagem (metade do POCO F4, 2400x1080) + toque simulado com o mouse (-toque)
    [string]$Drive = ""   # teclas seguradas na captura, ex.: "W" ou "W,SHIFT" (SendInput real; SendKeys nao chega no Input System)
)
$ErrorActionPreference = "Stop"
$Proj = Split-Path -Parent $PSScriptRoot
$Exe = Join-Path $Proj "Builds\win\COE.exe"
$ShotDir = Join-Path $Proj "Builds\win\shots"
if (-not (Test-Path $Exe)) { "EXE ausente: $Exe. Rode client\tools\build_windows.ps1 antes."; exit 2 }
New-Item -ItemType Directory -Force $ShotDir | Out-Null

Add-Type -AssemblyName System.Drawing
Add-Type @"
using System;
using System.Runtime.InteropServices;
public class Win {
  [DllImport("user32.dll")] public static extern void keybd_event(byte vk, byte scan, uint flags, UIntPtr extra);
  public const uint KEYUP = 0x0002;
  [DllImport("user32.dll")] public static extern uint MapVirtualKey(uint code, uint type);
  // scan code de verdade: o Input System do Unity le a tecla pelo scan code; com 0 a tecla nao chega
  public static void Key(byte vk, bool down) { keybd_event(vk, (byte)MapVirtualKey(vk, 0), down ? 0u : KEYUP, UIntPtr.Zero); }
  [DllImport("user32.dll")] public static extern bool SetProcessDPIAware();
  [DllImport("user32.dll")] public static extern bool GetWindowRect(IntPtr h, out RECT r);
  [DllImport("user32.dll")] public static extern bool SetForegroundWindow(IntPtr h);
  [DllImport("user32.dll")] public static extern IntPtr GetForegroundWindow();
  [DllImport("user32.dll")] public static extern bool SetCursorPos(int x, int y);
  [DllImport("user32.dll")] public static extern void mouse_event(uint flags, int dx, int dy, uint data, UIntPtr extra);
  public static void Click(int x, int y) { SetCursorPos(x, y); mouse_event(0x0002, 0, 0, 0, UIntPtr.Zero); mouse_event(0x0004, 0, 0, 0, UIntPtr.Zero); }
  [StructLayout(LayoutKind.Sequential)] public struct RECT { public int Left, Top, Right, Bottom; }
}
"@

$args = @("-screen-width", $Width, "-screen-height", $Height, "-screen-fullscreen", "0", "-popupwindow")
if ($Celular)  { $args = @("-screen-width", 1200, "-screen-height", 540, "-screen-fullscreen", "0", "-popupwindow", "-toque") }
if ($AutoWalk) { $args += "-autowalk" }
if ($Scene)   { $args += @("-scene", $Scene) }
# pixels fisicos: sem isto, com escala de tela de 125% o retangulo da janela vem em pixels logicos e a foto corta
[void][Win]::SetProcessDPIAware()
$p = Start-Process -FilePath $Exe -ArgumentList $args -PassThru
"PID $($p.Id): esperando a janela..."
$deadline = (Get-Date).AddSeconds(30)
while (-not $p.MainWindowHandle -or $p.MainWindowHandle -eq 0) {
    if ((Get-Date) -gt $deadline) { "janela nao apareceu em 30 s"; $p.Kill(); exit 3 }
    Start-Sleep -Milliseconds 500
    $p.Refresh()
}
[void][Win]::SetForegroundWindow($p.MainWindowHandle)
$stamp = Get-Date -Format "yyyy-MM-dd_HHmm"
$every = [Math]::Max(1, [int]($Seconds / [Math]::Max(1, $Shots)))
$taken = @()
# teclas seguradas: o jogo precisa de input de verdade para o personagem andar na foto
$vks = @{ "W"=0x57; "A"=0x41; "S"=0x53; "D"=0x44; "SHIFT"=0x10; "SPACE"=0x20; "E"=0x45 }
$held = @()
if ($Drive) {
    [void][Win]::SetForegroundWindow($p.MainWindowHandle)
    Start-Sleep -Milliseconds 400
    # foreground nao e o mesmo que foco de teclado: um clique dentro da janela ativa de verdade
    $r0 = New-Object Win+RECT
    [void][Win]::GetWindowRect($p.MainWindowHandle, [ref]$r0)
    [Win]::Click([int](($r0.Left + $r0.Right) / 2), [int](($r0.Top + $r0.Bottom) / 2))
    Start-Sleep -Milliseconds 700
    if ([Win]::GetForegroundWindow() -ne $p.MainWindowHandle) {
        "ABORTADO: a janela do jogo nao esta em primeiro plano; nao vou digitar as teclas em outra janela."
        $Drive = ""
    }
}
foreach ($k in ($Drive -split "," | Where-Object { $_ })) {
    $name = $k.Trim().ToUpper()
    if ($vks.ContainsKey($name)) { $held += [byte]$vks[$name]; [Win]::Key([byte]$vks[$name], $true); "segurando $name" }
    else { "tecla desconhecida: $name" }
}

for ($i = 1; $i -le $Shots; $i++) {
    Start-Sleep -Seconds $every
    if ($p.HasExited) { "o jogo fechou sozinho (exit $($p.ExitCode))"; break }
    $r = New-Object Win+RECT
    [void][Win]::GetWindowRect($p.MainWindowHandle, [ref]$r)
    $w = $r.Right - $r.Left; $h = $r.Bottom - $r.Top
    if ($w -le 0 -or $h -le 0) { continue }
    $bmp = New-Object System.Drawing.Bitmap $w, $h
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    # so o retangulo da janela do jogo entra na imagem
    $g.CopyFromScreen($r.Left, $r.Top, 0, 0, (New-Object System.Drawing.Size $w, $h))
    $out = Join-Path $ShotDir ("shot_" + $stamp + "_" + $i + ".png")
    $bmp.Save($out, [System.Drawing.Imaging.ImageFormat]::Png)
    $g.Dispose(); $bmp.Dispose()
    $taken += $out
    "foto $i/$Shots -> $out"
}
$logPlayer = Join-Path $env:USERPROFILE "AppData\LocalLow\V-STACK\Chronicles of Existence\Player.log"
foreach ($vk in $held) { [Win]::Key($vk, $false) }   # solta as teclas antes de sair
if (-not $KeepOpen -and -not $p.HasExited) { $p.Kill(); "jogo fechado." }
if (Test-Path $logPlayer) {
    $errs = Select-String -Path $logPlayer -Pattern "Exception|NullReference|error" | Select-Object -Last 10
    if ($errs) { "---- erros no Player.log ----"; $errs | ForEach-Object { $_.Line } } else { "Player.log sem erro." }
}
"fotos: $($taken.Count) em $ShotDir"
