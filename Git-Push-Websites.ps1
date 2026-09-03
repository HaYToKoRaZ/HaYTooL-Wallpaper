param (
    [Parameter(Mandatory=$false)]
    [string]$Message = "chore(website): update HaYTooL Wallpaper website showcase"
)

Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "  HaYTooL-Wallpaper Websites Push Script     " -ForegroundColor Cyan
Write-Host "=============================================" -ForegroundColor Cyan

$WebsitesDir = Join-Path $PSScriptRoot "websites"
if (-not (Test-Path $WebsitesDir)) {
    Write-Error "Hata: 'websites' klasoru bulunamadi ($WebsitesDir)!"
    exit 1
}

Write-Host "[*] 'websites' bagimsiz calisma agacina (worktree) geciliyor..." -ForegroundColor Yellow

Push-Location $WebsitesDir
try {
    Write-Host "[*] Git durumunu kontrol ediyorum..." -ForegroundColor Yellow
    $status = git status --porcelain
    if ([string]::IsNullOrWhiteSpace($status)) {
        Write-Host "[!] 'websites' klasorunde yeni degisiklik yok. Push islemine gerek yok." -ForegroundColor Green
    } else {
        Write-Host "[*] Degisiklikler ekleniyor (git add .)..." -ForegroundColor Yellow
        git add . 2>&1 | Write-Host

        Write-Host "[*] Commit olusturuluyor: '$Message'..." -ForegroundColor Yellow
        git commit -m "$Message" 2>&1 | Write-Host

        Write-Host "[*] Bagimsiz 'websites' dalina push ediliyor (origin websites)..." -ForegroundColor Yellow
        git push origin websites 2>&1 | Write-Host

        Write-Host "[+] 'websites' dali basariyla GitHub'a gonderildi!" -ForegroundColor Green
    }
}
finally {
    Pop-Location
}

Write-Host "=============================================" -ForegroundColor Cyan
