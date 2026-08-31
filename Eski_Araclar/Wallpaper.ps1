# ===============================
# Sessiz Rastgele Wallpaper Betiği
# ===============================

# Kayıt klasörü
$FolderPath = "C:\0-wallpaper"

# Klasör yoksa oluştur
if (!(Test-Path $FolderPath)) {
    New-Item -ItemType Directory -Path $FolderPath | Out-Null
}

# Klasör boyutunu kontrol et
try {
    $FolderSize = Get-ChildItem -Path $FolderPath -Recurse -File | Measure-Object -Property Length -Sum
    if ($FolderSize.Sum -gt 100MB) {
        # Klasör 100MB'tan büyükse içeriği sil
        Get-ChildItem -Path $FolderPath -Recurse | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
    }
}
catch {
    # Klasör boyutunu kontrol ederken bir hata oluşursa, sessizce devam et
    # Bu, örneğin klasöre erişim izni yoksa yaşanabilir
}

# Dosya adı (tarih + saat + saniye)
$TimeStamp = Get-Date -Format "yyyyMMdd-HHmmss"
$WallpaperPath = "$FolderPath\wallpaper-$TimeStamp.jpg"

# Rastgele resim URL
# İsteğe bağlı olarak çözünürlüğü buradan değiştirebilirsiniz
$RandomUrl = "https://picsum.photos/3840/2160"

# Resmi indir (hiçbir progress/pencere olmadan)
$wc = New-Object System.Net.WebClient
try {
    $wc.DownloadFile($RandomUrl, $WallpaperPath)
}
catch {
    # İndirme başarısız olursa betikten sessizce çık
    exit
}
finally {
    $wc.Dispose()
}

# Windows API ile arka planı değiştir
Add-Type @"
using System.Runtime.InteropServices;
public class Wallpaper {
  [DllImport("user32.dll", SetLastError = true)]
  public static extern bool SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
}
"@ > $null

$SPI_SETDESKWALLPAPER = 0x0014
$SPIF_UPDATEINIFILE   = 0x01
$SPIF_SENDCHANGE      = 0x02

[Wallpaper]::SystemParametersInfo($SPI_SETDESKWALLPAPER, 0, $WallpaperPath, $SPIF_UPDATEINIFILE -bor $SPIF_SENDCHANGE) | Out-Null