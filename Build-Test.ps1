[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
Set-Location $PSScriptRoot

Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "  HaYTooL-Wallpaper Derleme ve Test Scripti  " -ForegroundColor Cyan
Write-Host "=============================================" -ForegroundColor Cyan
Write-Host ""

$OutDir = "App_Yayin"

# Eğer programlar açıksa kapat (Yoksa dosya kilidi hatası verir)
Write-Host "[*] Çalışan uygulamalar kapatılıyor (Dosya kilidi hatasını önlemek için)..." -ForegroundColor Yellow
Stop-Process -Name "Setting" -Force -ErrorAction SilentlyContinue
Stop-Process -Name "HaYTooL-Wallpaper" -Force -ErrorAction SilentlyContinue
Start-Sleep -Seconds 1

# Eski derlemeleri temizle
if (Test-Path $OutDir) {
    Write-Host "[*] Eski derleme kalıntıları temizleniyor..." -ForegroundColor Yellow
    Remove-Item -Path "$OutDir\*" -Recurse -Force -ErrorAction SilentlyContinue
} else {
    New-Item -ItemType Directory -Path $OutDir | Out-Null
}

Write-Host "`n[*] 1. Arayüz (Setting.exe) derleniyor (Tek Dosya)..." -ForegroundColor Green
dotnet publish Setting/Setting.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o $OutDir

Write-Host "`n[*] 2. Arka Plan (HaYTooL-Wallpaper.exe) derleniyor (Tek Dosya)..." -ForegroundColor Green
dotnet publish HaYTooL-Wallpaper/HaYTooL-Wallpaper.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o $OutDir

Write-Host "`n=============================================" -ForegroundColor Cyan
Write-Host "   DERLEME BAŞARIYLA TAMAMLANDI! (Tebrikler) " -ForegroundColor Green
Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "`nDosyalarınız '$OutDir' klasöründe test edilmeye hazır." -ForegroundColor Yellow
Write-Host "Test etmek için App_Yayin\Setting.exe'yi çalıştırın." -ForegroundColor White

Write-Host "`nÇıkmak için ENTER'a basın..." -ForegroundColor Gray
Read-Host
