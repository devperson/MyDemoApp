using BestApp.Abstraction.Main.Infasructures;
using BestApp.Abstraction.Main.PlatformServices;
using Microsoft.Maui.Storage;
using System;

namespace BestApp.Impl.PlatformServices
{
    internal class DirectoryService : IDirectoryService
    {                
        public string GetAppDataDir()
        {
            return FileSystem.AppDataDirectory;
        }

        public string GetCacheDir()
        {
            return FileSystem.CacheDirectory;
        }
    }
}
