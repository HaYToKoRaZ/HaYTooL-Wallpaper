using System;
using System.IO;
using Microsoft.Win32;

namespace Setting.Services
{
    public static class RegistryService
    {
        private const string AppName = "HaYTooL-Wallpaper";
        private const string RunRegistryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private const string ShellKeyPath = @"SOFTWARE\Classes\Directory\Background\shell\HaYTooLWallpaper";

        public static bool IsStartupEnabled()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(RunRegistryKey, false);
                return key?.GetValue(AppName) != null;
            }
            catch
            {
                return false;
            }
        }

        public static void SetStartup(bool enable)
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(RunRegistryKey, true);
                if (key != null)
                {
                    if (enable)
                    {
                        string targetExe = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HaYTooL-Wallpaper.exe");
                        key.SetValue(AppName, $"\"{targetExe}\"");
                    }
                    else
                    {
                        key.DeleteValue(AppName, false);
                    }
                }
            }
            catch { }
        }

        public static bool IsContextMenuEnabled()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(ShellKeyPath, false);
                return key != null;
            }
            catch
            {
                return false;
            }
        }

        public static void SetContextMenu(bool enable, string language)
        {
            try
            {
                if (enable)
                {
                    string targetExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HaYTooL-Wallpaper.exe");
                    using var key = Registry.CurrentUser.CreateSubKey(ShellKeyPath);
                    if (key != null)
                    {
                        key.SetValue("", language == "EN" ? "Change Wallpaper" : "Wallpaper değiştir");
                        key.SetValue("Icon", $"\"{targetExePath}\"");
                        using var cmdKey = key.CreateSubKey("command");
                        cmdKey?.SetValue("", $"\"{targetExePath}\"");
                    }
                }
                else
                {
                    Registry.CurrentUser.DeleteSubKeyTree(ShellKeyPath, false);
                }
            }
            catch { }
        }
    }
}
