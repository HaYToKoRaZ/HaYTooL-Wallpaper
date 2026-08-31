using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Win32;
using Shared;

namespace HaYTooL_Wallpaper
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        private const int SPI_SETDESKWALLPAPER = 0x0014;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDWININICHANGE = 0x02;

        private const string CacheFolder = @"C:\0-wallpaper";
        private const long MaxCacheSize = 100 * 1024 * 1024; // 100MB

        static async Task Main(string[] args)
        {
            string exePath = AppDomain.CurrentDomain.BaseDirectory;
            string iniPath = Path.Combine(exePath, "settings.ini");

            IniHelper ini = new IniHelper(iniPath);
            string source = ini.Read("Source", "Settings", "Picsum");
            string category = ini.Read("Category", "Settings", "");

            EnsureCacheFolder();
            ManageCacheSize();

            string downloadedImagePath = null;
            try
            {
                downloadedImagePath = await DownloadWallpaperAsync(source, category);
            }
            catch (Exception)
            {
                // Silently ignore network/download errors
            }

            string pathToSet = downloadedImagePath;

            // Offline Fallback
            if (string.IsNullOrEmpty(pathToSet) || !File.Exists(pathToSet))
            {
                pathToSet = GetRandomImageFromCache();
            }

            if (!string.IsNullOrEmpty(pathToSet) && File.Exists(pathToSet))
            {
                SetWallpaperStyle(source == "Cats" || source == "Dogs" ? "6" : "10"); // 6: Fit, 10: Fill
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, pathToSet, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
            }
        }

        static void SetWallpaperStyle(string style)
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true))
                {
                    if (key != null)
                    {
                        key.SetValue(@"WallpaperStyle", style);
                        key.SetValue(@"TileWallpaper", "0");
                    }
                }
            }
            catch { }
        }

        static void EnsureCacheFolder()
        {
            if (!Directory.Exists(CacheFolder))
            {
                Directory.CreateDirectory(CacheFolder);
            }
        }

        static void ManageCacheSize()
        {
            try
            {
                var dirInfo = new DirectoryInfo(CacheFolder);
                var files = dirInfo.GetFiles().OrderBy(f => f.CreationTime).ToList();
                long totalSize = files.Sum(f => f.Length);

                while (totalSize > MaxCacheSize && files.Count > 1)
                {
                    var oldest = files.First();
                    totalSize -= oldest.Length;
                    oldest.Delete();
                    files.RemoveAt(0);
                }
            }
            catch { }
        }

        static string GetRandomImageFromCache()
        {
            try
            {
                var files = Directory.GetFiles(CacheFolder, "*.*").Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || s.EndsWith(".png", StringComparison.OrdinalIgnoreCase)).ToArray();
                if (files.Length > 0)
                {
                    var random = new Random();
                    return files[random.Next(files.Length)];
                }
            }
            catch { }
            return null;
        }

        static async Task<string> DownloadWallpaperAsync(string source, string category)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            string savePath = Path.Combine(CacheFolder, $"wallpaper-{timestamp}.jpg");
            string imageUrl = null;

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "HaYTooL-Wallpaper");

            if (source == "Wallhaven")
            {
                string query = string.IsNullOrEmpty(category) ? "" : $"q={category}&";
                string apiUrl = $"https://wallhaven.cc/api/v1/search?{query}sorting=random&resolutions=1920x1080,2560x1440,3840x2160";
                
                string jsonResponse = await client.GetStringAsync(apiUrl);
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                var data = doc.RootElement.GetProperty("data");
                if (data.GetArrayLength() > 0)
                {
                    imageUrl = data[0].GetProperty("path").GetString();
                }
            }
            else if (source == "Bing günün manzarası")
            {
                string apiUrl = "https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US";
                string jsonResponse = await client.GetStringAsync(apiUrl);
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                var images = doc.RootElement.GetProperty("images");
                if (images.GetArrayLength() > 0)
                {
                    string urlBase = images[0].GetProperty("url").GetString();
                    imageUrl = "https://www.bing.com" + urlBase;
                }
            }
            else if (source == "Anime")
            {
                string apiUrl = "https://nekos.life/api/v2/img/wallpaper";
                string jsonResponse = await client.GetStringAsync(apiUrl);
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                imageUrl = doc.RootElement.GetProperty("url").GetString();
            }
            else if (source == "Cats")
            {
                string apiUrl = "https://api.thecatapi.com/v1/images/search?mime_types=jpg,png";
                string jsonResponse = await client.GetStringAsync(apiUrl);
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                if (doc.RootElement.GetArrayLength() > 0)
                {
                    imageUrl = doc.RootElement[0].GetProperty("url").GetString();
                }
            }
            else if (source == "Dogs")
            {
                string apiUrl = "https://dog.ceo/api/breeds/image/random";
                string jsonResponse = await client.GetStringAsync(apiUrl);
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                imageUrl = doc.RootElement.GetProperty("message").GetString();
            }
            else // Default to Picsum
            {
                imageUrl = "https://picsum.photos/3840/2160";
            }

            if (!string.IsNullOrEmpty(imageUrl))
            {
                // Download file
                var response = await client.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();
                using var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None);
                await response.Content.CopyToAsync(fs);
                return savePath;
            }

            return null;
        }
    }
}
