using System;
using System.IO;

namespace Shared.Data
{
    public class SettingsRepository
    {
        private readonly IniHelper _ini;
        private const string Section = "Settings";

        public SettingsRepository(string iniPath = null)
        {
            if (string.IsNullOrEmpty(iniPath))
            {
                iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini");
            }
            _ini = new IniHelper(iniPath);
        }

        public string Language
        {
            get => _ini.Read("Language", Section, "TR");
            set => _ini.Write("Language", value, Section);
        }

        public string Source
        {
            get => _ini.Read("Source", Section, "Wallhaven");
            set => _ini.Write("Source", value, Section);
        }

        public string Category
        {
            get => _ini.Read("Category", Section, "Nature");
            set => _ini.Write("Category", value, Section);
        }
    }
}
