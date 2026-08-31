using System;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;

namespace Shared
{
    public class IniHelper
    {
        private string path;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        public IniHelper(string iniPath)
        {
            path = Path.GetFullPath(iniPath);
        }

        public string Read(string key, string section, string defaultValue = "")
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section, key, defaultValue, retVal, 255, path);
            return retVal.ToString();
        }

        public void Write(string key, string value, string section)
        {
            WritePrivateProfileString(section, key, value, path);
        }
    }
}
