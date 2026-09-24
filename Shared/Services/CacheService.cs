using System;
using System.IO;
using System.Linq;

namespace Shared.Services
{
    public class CacheService
    {
        public const string CacheFolder = @"C:\0-wallpaper";
        public const long MaxCacheSize = 100 * 1024 * 1024; // 100MB

        public static void EnsureCacheFolder()
        {
            if (!Directory.Exists(CacheFolder))
            {
                Directory.CreateDirectory(CacheFolder);
            }
        }

        public static void ManageCacheSize()
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

        public static string GetRandomImageFromCache()
        {
            try
            {
                EnsureCacheFolder();
                var files = Directory.GetFiles(CacheFolder, "*.*")
                    .Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) || 
                                s.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                if (files.Length > 0)
                {
                    var random = new Random();
                    return files[random.Next(files.Length)];
                }
            }
            catch { }
            return null;
        }

        public static string GenerateNewFilePath()
        {
            EnsureCacheFolder();
            string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            return Path.Combine(CacheFolder, $"wallpaper-{timestamp}.jpg");
        }
    }
}
