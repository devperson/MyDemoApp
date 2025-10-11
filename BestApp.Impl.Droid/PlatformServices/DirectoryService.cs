using Base.Abstractions.Platform;
using Microsoft.Maui.Storage;

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
