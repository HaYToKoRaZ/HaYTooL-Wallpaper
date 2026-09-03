param (
    [Parameter(Mandatory=$false)]
    [string]$Message = "chore(website): update HaYTooL Wallpaper website showcase"
)

Write-Host "=============================================" -ForegroundColor Cyan
Write-Host "  HaYTooL-Wallpaper Websites Push Script     " -ForegroundColor Cyan
Write-Host "=============================================" -ForegroundColor Cyan

# 1. 'websites' dizininin varligini dogrula
$WebsitesDir = Join-Path $PSScriptRoot "websites"
if (-not (Test-Path $WebsitesDir)) {
    Write-Error "Hata: 'websites' klasoru bulunamadi ($WebsitesDir)!"
    exit 1
}

Write-Host "[*] Websites klasoru kontrol edildi: $WebsitesDir" -ForegroundColor Yellow

# 2. Ana dizindeki olasi degisiklikleri status ile goster
Write-Host "[*] Git durumlari inceleniyor..." -ForegroundColor Yellow

# 3. websites klasorunu git'e ekle
git add websites 2>&1 | Write-Host

# 4. websites klasorunde degisiklik var mi kontrol et
$diff = git diff --cached -- websites
if ([string]::IsNullOrWhiteSpace($diff)) {
    # Onceden commit edilmis olabilir, yine de subtree push denenebilir veya uyari verilebilir
    Write-Host "[!] websites klasorunde yeni sahnelenmis (staged) degisiklik yok." -ForegroundColor Yellow
} else {
    Write-Host "[*] Commit olusturuluyor: '$Message'..." -ForegroundColor Yellow
    git commit -m "$Message" websites 2>&1 | Write-Host
}

# 5. git subtree ile 'websites' dalina izole push et
Write-Host "[*] 'websites' dalina subtree push yapiliyor (origin websites)..." -ForegroundColor Yellow

# Oncelikle yerel websites dali yoksa subtree split ile olustur veya dogrudan origin'e gonder
try {
    $pushOutput = git subtree push --prefix=websites origin websites 2>&1
    Write-Host $pushOutput
    Write-Host "`n[+] 'websites' dali basariyla GitHub'a gonderildi!" -ForegroundColor Green
} catch {
    Write-Warning "Subtree push sirasinda bir uyari veya hata olustu. Detay:"
    Write-Host $_
}

Write-Host "=============================================" -ForegroundColor Cyan
