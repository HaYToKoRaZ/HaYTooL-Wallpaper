using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class WallpaperDownloader
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        static WallpaperDownloader()
        {
            HttpClient.DefaultRequestHeaders.Add("User-Agent", "HaYTooL-Wallpaper");
            HttpClient.Timeout = TimeSpan.FromSeconds(15);
        }

        public static async Task<string> DownloadAsync(string source, string category)
        {
            string savePath = CacheService.GenerateNewFilePath();
            string imageUrl = null;

            if (source == "Wallhaven")
            {
                string query = string.IsNullOrEmpty(category) ? "" : $"q={category}&";
                string apiUrl = $"https://wallhaven.cc/api/v1/search?{query}sorting=random&resolutions=1920x1080,2560x1440,3840x2160";

                string jsonResponse = await HttpClient.GetStringAsync(apiUrl);
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                var data = doc.RootElement.GetProperty("data");
                if (data.GetArrayLength() > 0)
                {
                    imageUrl = data[0].GetProperty("path").GetString();
                }
            }
            else if (source == "Bing" || source == "Bing günün manzarası" || source == "Bing Günün Manzarası")
            {
                string apiUrl = "https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1&mkt=en-US";
                string jsonResponse = await HttpClient.GetStringAsync(apiUrl);
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
                string jsonResponse = await HttpClient.GetStringAsync(apiUrl);
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                imageUrl = doc.RootElement.GetProperty("url").GetString();
            }
            else if (source == "Cats" || source == "Sevimli Kediler 🐱" || source == "Sevimli Kediler")
            {
                string apiUrl = "https://api.thecatapi.com/v1/images/search?mime_types=jpg,png";
                string jsonResponse = await HttpClient.GetStringAsync(apiUrl);
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                if (doc.RootElement.GetArrayLength() > 0)
                {
                    imageUrl = doc.RootElement[0].GetProperty("url").GetString();
                }
            }
            else if (source == "Dogs" || source == "Sadık Köpekler 🐶" || source == "Sadık Köpekler")
            {
                string apiUrl = "https://dog.ceo/api/breeds/image/random";
                string jsonResponse = await HttpClient.GetStringAsync(apiUrl);
                using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                imageUrl = doc.RootElement.GetProperty("message").GetString();
            }
            else if (source == "Foxes" || source == "Kurnaz Tilkiler 🦊" || source == "Kurnaz Tilkiler")
            {
                // RandomFox API (Ekstra Canlı Doğal Hayvan Kaynağı)
                try
                {
                    string apiUrl = "https://randomfox.ca/floof/";
                    string jsonResponse = await HttpClient.GetStringAsync(apiUrl);
                    using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                    imageUrl = doc.RootElement.GetProperty("image").GetString();
                }
                catch { }
            }
            else if (source == "NASA APOD" || source == "NASA APOD 🚀")
            {
                // NASA Astronomy Picture of the Day
                try
                {
                    string apiUrl = "https://api.nasa.gov/planetary/apod?api_key=DEMO_KEY";
                    string jsonResponse = await HttpClient.GetStringAsync(apiUrl);
                    using JsonDocument doc = JsonDocument.Parse(jsonResponse);
                    var root = doc.RootElement;
                    if (root.TryGetProperty("media_type", out var mediaType) && mediaType.GetString() == "image")
                    {
                        if (root.TryGetProperty("hdurl", out var hdurl) && !string.IsNullOrEmpty(hdurl.GetString()))
                            imageUrl = hdurl.GetString();
                        else if (root.TryGetProperty("url", out var url))
                            imageUrl = url.GetString();
                    }
                }
                catch { }

                // Eger APOD video ise veya erisilemezse yedek uzay gorseli
                if (string.IsNullOrEmpty(imageUrl))
                {
                    imageUrl = "https://picsum.photos/3840/2160?space";
                }
            }
            else if (source == "Unsplash Doğa" || source == "Unsplash Nature")
            {
                imageUrl = "https://picsum.photos/3840/2160?nature";
            }
            else // Varsayilan Picsum
            {
                imageUrl = "https://picsum.photos/3840/2160";
            }

            if (!string.IsNullOrEmpty(imageUrl))
            {
                using var response = await HttpClient.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();
                using var fs = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None);
                await response.Content.CopyToAsync(fs);
                return savePath;
            }

            return null;
        }
    }
}
