using System;
using System.Threading.Tasks;
using Shared.Services;

namespace HaYTooL_Wallpaper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await WallpaperManager.ExecuteAsync();
        }
    }
}
