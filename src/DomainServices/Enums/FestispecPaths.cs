using System;
using System.IO;

namespace Festispec.DomainServices.Enums
{
    public static class FestispecPaths
    {
        // base %AppData% path.
        public static string FestispecApplicationDataPath =>
            $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\Festispec";

        // base offline data path.
        public static string FestispecOfflineStoragePath => $"{FestispecApplicationDataPath}\\OfflineData";

        public static void Setup()
        {
            // Add a check for all of your custom directories here.
            // example: CheckAndCreateDirectory("C:\MyDirectory");
            CheckAndCreateDirectory(FestispecApplicationDataPath);
            CheckAndCreateDirectory(FestispecOfflineStoragePath);
        }

        private static void CheckAndCreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}