using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Shared
{
    public static class PulseClient
    {
        private const string Endpoint = "https://hayto-telemetry.korazhayto.workers.dev/api/ping";
        private const string AppId = "pc_wallpaper";
        private static readonly HttpClient HttpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
        private static string _sessionId;
        private static bool _isNewSession = true;
        private static System.Threading.Timer _timer;

        public static void Start()
        {
            try
            {
                if (_sessionId == null)
                {
                    _sessionId = "pc_" + Guid.NewGuid().ToString("N").Substring(0, 10);
                }

                // İlk ping'i arka planda tetikle
                Task.Run(() => SendPulseAsync());

                // 2 dakikada bir düzenli nabız (heartbeat)
                _timer = new System.Threading.Timer(async _ => await SendPulseAsync(), null, TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(2));
            }
            catch
            {
                // Sessizce yutulur
            }
        }

        public static async Task SendPulseAsync()
        {
            try
            {
                var payload = new
                {
                    app = AppId,
                    session_id = _sessionId,
                    is_new_session = _isNewSession
                };

                string json = JsonSerializer.Serialize(payload);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                using var response = await HttpClient.PostAsync(Endpoint, content);
                
                if (response.IsSuccessStatusCode)
                {
                    _isNewSession = false;
                }
            }
            catch
            {
                // Ağ yoksa veya hata alınırsa sessizce devam edilir
            }
        }
    }
}
