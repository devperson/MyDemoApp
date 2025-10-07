using BestApp.Abstraction.General.Infasructures;
using BestApp.Abstraction.General.Platform;
using Microsoft.Maui.Storage;
using System;

namespace BestApp.Impl.Platform
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
