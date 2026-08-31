Set-Location $PSScriptRoot

Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "  HaYTooL-Wallpaper Derleme ve Test Scripti  " -ForegroundColor Cyan
Write-Host "=============================================" -ForegroundColor Cyan
Write-Host ""

$OutDir = "App_Yayin"

# Eger programlar aciksa kapat (Yoksa dosya kilidi hatasi verir)
Write-Host "[*] Calisan uygulamalar kapatiliyor (Dosya kilidi hatasini onlemek icin)..." -ForegroundColor Yellow
Stop-Process -Name "Setting" -Force -ErrorAction SilentlyContinue
Stop-Process -Name "HaYTooL-Wallpaper" -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 1

# Eski derlemeleri temizle
if (Test-Path $OutDir) {
    Write-Host "[*] Eski derleme kalintilari temizleniyor..." -ForegroundColor Yellow
    Remove-Item -Path "$OutDir\*" -Recurse -Force -ErrorAction SilentlyContinue
} else {
    New-Item -ItemType Directory -Path $OutDir | Out-Null
}

Write-Host "`n[*] 1. Arayuz (Setting.exe) derleniyor (Tek Dosya)..." -ForegroundColor Green
dotnet publish Setting/Setting.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o $OutDir

Write-Host "`n[*] 2. Arka Plan (HaYTooL-Wallpaper.exe) derleniyor (Tek Dosya)..." -ForegroundColor Green
dotnet publish HaYTooL-Wallpaper/HaYTooL-Wallpaper.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o $OutDir

Write-Host "`n=============================================" -ForegroundColor Cyan
Write-Host "   DERLEME BASARIYLA TAMAMLANDI! (Tebrikler) " -ForegroundColor Green
Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "`nDosyalariniz '$OutDir' klasorunde test edilmeye hazir." -ForegroundColor Yellow
Write-Host "Test etmek icin App_Yayin\Setting.exe'yi calistirin." -ForegroundColor White

Write-Host "`nCikmak icin ENTER'a basin..." -ForegroundColor Gray
Read-Host
