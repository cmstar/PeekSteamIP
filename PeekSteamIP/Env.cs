using System;
using System.IO;

namespace PeekSteamIP
{
    public static class Env
    {
        public const int ConnectionLimit = 20;
        public const string SteamStoreHost = "store.steampowered.com";
        public const string ConfigSectionDnsList = "dnslist";
        public const string ConfigSectionIpList = "iplist";

        public static string ConfigFilePath
        {
            get
            {
                var appPath = AppDomain.CurrentDomain.BaseDirectory;
                var filePath = Path.Combine(appPath, "PeekSteamIP.ini");
                return filePath;
            }
        }
    }
}
