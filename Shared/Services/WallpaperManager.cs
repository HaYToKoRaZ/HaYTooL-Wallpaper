using System;
using System.IO;
using System.Threading.Tasks;
using Shared.Core;
using Shared.Data;

namespace Shared.Services
{
    public static class WallpaperManager
    {
        public static async Task ExecuteAsync()
        {
            var settings = new SettingsRepository();
            string source = settings.Source;
            string category = settings.Category;

            CacheService.EnsureCacheFolder();
            CacheService.ManageCacheSize();

            string downloadedImagePath = null;
            try
            {
                downloadedImagePath = await WallpaperDownloader.DownloadAsync(source, category);
            }
            catch
            {
                // Ağ / indirme hataları sessizce yutulur
            }

            string pathToSet = downloadedImagePath;

            // Offline Fallback
            if (string.IsNullOrEmpty(pathToSet) || !File.Exists(pathToSet))
            {
                pathToSet = CacheService.GetRandomImageFromCache();
            }

            if (!string.IsNullOrEmpty(pathToSet) && File.Exists(pathToSet))
            {
                string style = (source == "Cats" || source == "Dogs") ? "6" : "10"; // 6: Fit, 10: Fill
                NativeMethods.ApplyWallpaper(pathToSet, style);
            }

            // Hızlı nabız gönderimi
            try
            {
                await PulseClient.SendPulseAsync();
            }
            catch { }
        }
    }
}
