param (
    [Parameter(Mandatory=$false)]
    [string]$Message = "chore: auto-push from Git-Push script"
)

Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "  HaYTooL-Wallpaper Git Push Script" -ForegroundColor Cyan
Write-Host "=============================================" -ForegroundColor Cyan

Write-Host "[*] Git durumunu kontrol ediyorum..." -ForegroundColor Yellow
$status = git status --porcelain
if ([string]::IsNullOrWhiteSpace($status)) {
    Write-Host "[!] Degisiklik yok. Push islemine gerek yok." -ForegroundColor Green
    exit 0
}

Write-Host "[*] Degisiklikler ekleniyor (git add .)..." -ForegroundColor Yellow
git add . 2>&1 | Write-Host

Write-Host "[*] Commit olusturuluyor: '$Message'..." -ForegroundColor Yellow
git commit -m "$Message" 2>&1 | Write-Host

Write-Host "[*] Degisiklikler GitHub'a gonderiliyor (git push origin master)..." -ForegroundColor Yellow
git push origin master 2>&1 | Write-Host

Write-Host "[+] Islem basariyla tamamlandi!" -ForegroundColor Green
