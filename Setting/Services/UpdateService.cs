using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Setting.Services
{
    public static class UpdateService
    {
        public const string CurrentVersion = "v1.2";
        private const string RepoUrl = "https://api.github.com/repos/HaYToKoRaZ/HaYTooL-Wallpaper/releases/latest";

        public class UpdateCheckResult
        {
            public bool IsSuccess { get; set; }
            public bool HasNewerVersion { get; set; }
            public string LatestVersion { get; set; }
        }

        public static async Task<UpdateCheckResult> CheckForUpdatesAsync()
        {
            try
            {
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "HaYTooL-Wallpaper-Updater");
                client.Timeout = TimeSpan.FromSeconds(5);

                string json = await client.GetStringAsync(RepoUrl);
                using JsonDocument doc = JsonDocument.Parse(json);
                string latestVersionStr = doc.RootElement.GetProperty("tag_name").GetString() ?? "";

                string cleanLatest = latestVersionStr.TrimStart('v', 'V');
                string cleanCurrent = CurrentVersion.TrimStart('v', 'V');

                bool hasNewer = false;
                if (Version.TryParse(cleanLatest, out Version latestVer) && Version.TryParse(cleanCurrent, out Version curVer))
                {
                    hasNewer = latestVer > curVer;
                }
                else if (!string.IsNullOrEmpty(latestVersionStr) && latestVersionStr != CurrentVersion)
                {
                    hasNewer = string.Compare(cleanLatest, cleanCurrent, StringComparison.OrdinalIgnoreCase) > 0;
                }

                return new UpdateCheckResult
                {
                    IsSuccess = true,
                    HasNewerVersion = hasNewer,
                    LatestVersion = latestVersionStr
                };
            }
            catch
            {
                return new UpdateCheckResult { IsSuccess = false };
            }
        }
    }
}
