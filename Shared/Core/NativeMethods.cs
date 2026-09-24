using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Shared.Core
{
    public static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);

        public const int SPI_SETDESKWALLPAPER = 0x0014;
        public const int SPIF_UPDATEINIFILE = 0x01;
        public const int SPIF_SENDWININICHANGE = 0x02;

        public static void SetWallpaperStyle(string style)
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(@"Control Panel\Desktop", true);
                if (key != null)
                {
                    key.SetValue(@"WallpaperStyle", style);
                    key.SetValue(@"TileWallpaper", "0");
                }
            }
            catch { }
        }

        public static void ApplyWallpaper(string imagePath, string style = "10")
        {
            SetWallpaperStyle(style);
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, imagePath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}
