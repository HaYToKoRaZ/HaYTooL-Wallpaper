---
name: HaYTooL Wallpaper Anayasası
description: Projenin geliştirme ve derleme kuralları
---

# HaYTooL Wallpaper - Proje Anayasası (Constitution)

Bu belge, HaYTooL Wallpaper projesinin bütünlüğünü korumak için, projeye müdahale eden tüm AI ajanları (Antigravity vb.) ve geliştiriciler tarafından okunması zorunlu olan kesin kuralları içerir.

## 1. Mimari Prensipler
- **İki Parçalı Yapı:** Proje daima 2 çalıştırılabilir dosyadan oluşmalıdır:
  1. `Setting.exe` (Ayarlar Arayüzü)
  2. `HaYTooL-Wallpaper.exe` (Arka Planda Çalışan Sessiz Tetikleyici)
- **Veri Paylaşımı:** İki uygulama arasındaki tüm iletişim `settings.ini` dosyası üzerinden yapılmalıdır.
- **Sessizlik (Silent Execution):** `HaYTooL-Wallpaper.exe` kesinlikle ekrana siyah bir terminal (console) penceresi fırlatmamalıdır. `WinExe` olarak yapılandırılmalıdır.

## 2. Derleme (Build) Kuralları
- Uygulama, kullanıcının sisteminde DLL kirliliği yaratmamak için **DAİMA "Tek Dosya" (Single-File)** formatında derlenmelidir.
- Manuel derleme yapılacaksa şu komutlar kullanılmalıdır:
  - `dotnet publish Setting/Setting.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o App_Yayin/`
  - `dotnet publish HaYTooL-Wallpaper/HaYTooL-Wallpaper.csproj -c Release -r win-x64 --self-contained false -p:PublishSingleFile=true -o App_Yayin/`
- Derleme çıktıları mutlaka `App_Yayin` klasöründe ve **aynı dizin içinde** toplanmalıdır ki `Setting.exe` diğer exe'yi bulabilsin.

## 3. Güncelleme (Update) ve Sürüm Yönetimi
- Projeye yeni bir özellik eklendiğinde `Setting/Form1.cs` içindeki `CheckForUpdates()` metodu kontrol edilmeli ve arayüz başlığındaki versiyon numarası (Örn: `v1.0.0`) manuel artırılmalıdır.
- Sürüm numarası ayrıca `README.md` dosyasında da güncellenmelidir.
- Her geliştirme sonrasında Github deposuna commit atıldığında, GitHub Actions `.github/workflows/build.yml` sayesinde otomatik olarak yeni bir release artifact'i oluşturur.

## 4. Kullanıcı Deneyimi (UX)
- Arayüz her zaman Çift Dilli (Türkçe ve İngilizce) olmalıdır. Eklenen yeni metinler her iki dilde de yazılmalıdır.
- İnternet bağlantısı kesilirse uygulama ÇÖKMEMELİDİR. Hata yönetimi (Try-Catch) `HaYTooL-Wallpaper/Program.cs` içinde sıkı tutulmalı ve her zaman `C:\0-wallpaper` içindeki eski görseller geri çağrılmalıdır (Offline Fallback).

> Geliştirici Ajan (Antigravity) Notu: Bu anayasayı okudun. Tüm değişikliklerini bu kurallar çerçevesinde uygula ve kullanıcıya "Anayasaya uygun hareket ettim" mesajını ver.
