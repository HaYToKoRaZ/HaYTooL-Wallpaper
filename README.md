# HaYTooL Wallpaper Çevrimdışı Destekli Duvar Kağıdı Yöneticisi

HaYTooL Wallpaper, Windows masaüstü arka planınızı belirlediğiniz kaynaklardan yüksek çözünürlüklü görsellerle dinamik olarak değiştiren, **hiçbir komut penceresi göstermeden tamamen sessiz çalışan** hafif ve modern bir C# / .NET uygulamasıdır.

## 🌟 Özellikler

- **Çift Dil Desteği:** Türkçe ve İngilizce arayüz (Settings üzerinden).
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
