# HaYTooL Wallpaper (v1.0.7) 🌍

<p align="center">
  <img src="logo.png" width="128">
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Status-Active-success?style=for-the-badge" alt="Status" />
  <img src="https://img.shields.io/badge/Platform-Windows-blue?style=for-the-badge&logo=windows" alt="Platform" />
  <img src="https://img.shields.io/badge/Language-C%23%20%7C%20.NET%208.0-512BD4?style=for-the-badge&logo=c-sharp" alt="Language" />
  <img src="https://img.shields.io/badge/Database-INI%20(Settings)-red?style=for-the-badge" alt="Database" />
  <br>
  <img src="https://img.shields.io/badge/UI-Windows%20Forms-1572B6?style=for-the-badge" alt="UI" />
  <img src="https://img.shields.io/badge/Downloader-Native%20HTTP-4af626?style=for-the-badge" alt="Downloader" />
  <img src="https://img.shields.io/badge/Version-v1.0.7-purple?style=for-the-badge&logo=git" alt="Version" />
  <img src="https://img.shields.io/badge/License-MIT-green?style=for-the-badge" alt="License" />
  <img src="https://img.shields.io/github/downloads/HaYToKoRaZ/HaYTooL-Wallpaper/latest/total?style=for-the-badge&color=blueviolet" alt="GitHub Downloads (latest release)" />
</p>

*[For Turkish documentation, please scroll down. / Türkçe açıklama için aşağı kaydırın.]*

HaYTooL Wallpaper is a lightweight and modern C# / .NET application that dynamically changes your Windows desktop background using high-resolution images from selected sources. **It runs completely silently in the background without showing any command prompt windows.**

## 🌟 Features (English)

- **Dual Language Support:** Turkish and English UI via the Settings app.
- **Rich Source Options:** Choose from 6 different sources:
  - **Wallhaven:** Professional wallpapers with categories (Nature, Space, Cars, Cyberpunk, etc.).
  - **Bing Image of the Day:** Microsoft Bing's daily beautiful landscape photos.
  - **Picsum:** Completely random art images.
  - **Anime:** High-quality anime drawings powered by Nekos.Life.
  - **Cats:** Cute cat photos.
  - **Dogs:** Cute dog photos.
- **Smart Offline Support (Fallback):** Downloaded wallpapers are cached in `C:\0-wallpaper`. If your internet drops or a server error occurs, the application never crashes; it automatically picks a random old image from the cache and sets it as the wallpaper.
- **Quota Management:** When the cache folder exceeds 100MB, it smartly manages space by deleting only the oldest files.
- **System Startup:** Easily add to the registry to change the background automatically when the computer boots.

## 📁 Architecture

The project consists of 2 main executables (`.exe`):

1. **`Setting.exe` (UI):** A Visual Interface where you can configure settings, choose categories, and set the application to run at system startup. Saves your preferences to a `settings.ini` file.
2. **`HaYTooL-Wallpaper.exe` (Core):** The core engine that runs **completely invisibly** (silent) in the background, reads the `settings.ini` file, and instantly changes the wallpaper.

## 🚀 How to Use?

1. Download or compile the project (you can use the outputs in the `Release` folder).
2. Run the `Setting.exe` file.
3. Choose your Language, Source, and Category (if supported).
4. Check the "Run on Windows startup" box if you want it to run on boot.
5. As soon as you click the **Save & Apply** button, your wallpaper will change automatically based on the new settings.
6. Whenever you want to manually trigger a change, just double-click `HaYTooL-Wallpaper.exe`!

---

# HaYTooL Wallpaper (v1.0.7) 🇹🇷

<p align="center">
  <img src="logo.png" width="128">
</p>

<p align="center">
  <img src="https://img.shields.io/badge/Status-Aktif-success?style=for-the-badge" alt="Status" />
  <img src="https://img.shields.io/badge/Platform-Windows-blue?style=for-the-badge&logo=windows" alt="Platform" />
  <img src="https://img.shields.io/badge/Language-C%23%20%7C%20.NET%208.0-512BD4?style=for-the-badge&logo=c-sharp" alt="Language" />
  <img src="https://img.shields.io/badge/Database-INI%20(Ayarlar)-red?style=for-the-badge" alt="Database" />
  <br>
  <img src="https://img.shields.io/badge/UI-Windows%20Forms-1572B6?style=for-the-badge" alt="UI" />
  <img src="https://img.shields.io/badge/Downloader-Yerel%20HTTP-4af626?style=for-the-badge" alt="Downloader" />
  <img src="https://img.shields.io/badge/Version-v1.0.7-purple?style=for-the-badge&logo=git" alt="Version" />
  <img src="https://img.shields.io/badge/License-MIT-green?style=for-the-badge" alt="License" />
  <img src="https://img.shields.io/github/downloads/HaYToKoRaZ/HaYTooL-Wallpaper/latest/total?style=for-the-badge&color=blueviolet" alt="GitHub İndirmeleri (en son sürüm)" />
</p>

HaYTooL Wallpaper, Windows masaüstü arka planınızı belirlediğiniz kaynaklardan yüksek çözünürlüklü görsellerle dinamik olarak değiştiren, **hiçbir komut penceresi göstermeden tamamen sessiz çalışan** hafif ve modern bir C# / .NET uygulamasıdır.

## 🌟 Özellikler (Türkçe)

- **Çift Dil Desteği:** Türkçe ve İngilizce arayüz (Ayarlar üzerinden).
- **Zengin Kaynak Seçenekleri:** 6 farklı kaynaktan dilediğinizi seçin:
  - **Wallhaven:** Kategorili (Doğa, Uzay, Arabalar, Siberpunk vb.) profesyonel duvar kağıtları.
  - **Bing Günün Manzarası:** Microsoft Bing'in günlük harika fotoğrafları.
  - **Picsum:** Tamamen rastgele sanat kareleri.
  - **Anime:** Nekos.Life destekli yüksek kalite anime çizimleri.
  - **Cats:** Sevimli kedi fotoğrafları.
  - **Dogs:** Sevimli köpek fotoğrafları.
- **Akıllı Çevrimdışı (Offline) Destek:** İndirilen duvar kağıtları `C:\0-wallpaper` önbelleğine (cache) kaydedilir. İnternetiniz koptuğunda veya sunucu hatası yaşandığında uygulama asla çökmez; otomatik olarak önbellekteki eski resimlerden birini rastgele seçerek masaüstünüzü değiştirir.
- **Kota Yönetimi:** Önbellek klasörü 100MB'ı geçtiğinde, sadece en eski dosyalar silinerek akıllıca bir alan yönetimi yapılır.
- **Sistem Başlangıcı:** Bilgisayar açıldığında otomatik olarak arka planı değiştirmesi için kayıt defterine kolayca eklenebilir.

## 📁 Mimari

Proje 2 ana çalıştırılabilir dosyadan (`.exe`) oluşur:

1. **`Setting.exe` (Arayüz):** Ayarları yapabileceğiniz, kategorileri seçebileceğiniz ve uygulamanın sistem başlangıcında çalışmasını ayarlayabileceğiniz Görsel Arayüz programı. Yaptığınız seçimleri `settings.ini` dosyasına kaydeder.
2. **`HaYTooL-Wallpaper.exe` (Çekirdek):** Arka planda **tamamen görünmez** (silent) çalışan ve `settings.ini` dosyasını okuyarak duvar kağıdını anında değiştiren çekirdek yapı.

## 🚀 Nasıl Kullanılır?

1. Projeyi bilgisayarınıza indirin veya derleyin (`Release` klasöründeki çıktıları kullanabilirsiniz).
2. `Setting.exe` dosyasını çalıştırın.
3. Dil, Kaynak ve Kategori (Destekleniyorsa) seçimlerinizi yapın.
4. "Sistem açılışında çalıştır" kutucuğu ile isterseniz Windows başlangıcına ekleyin.
5. **Kaydet ve Uygula** butonuna bastığınız anda duvar kağıdınız otomatik olarak yeni ayarlara göre değişecektir.
6. Manuel tetiklemek istediğinizde dilediğiniz zaman `HaYTooL-Wallpaper.exe`'ye çift tıklayabilirsiniz!
