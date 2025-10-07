using BestApp.Abstraction.General.Infasructures;
using BestApp.Abstraction.General.Platform;
using Microsoft.Maui.Storage;
using System;

namespace BestApp.Impl.Platform
{
    internal class DirectoryService : IDirectoryService
    {
        private readonly Lazy<IDatabaseInfo> databaseInfo;

        public DirectoryService(Lazy<IDatabaseInfo> databaseInfo)
        {
            this.databaseInfo = databaseInfo;
        }

        public string GetDbPath()
        {
            return databaseInfo.Value.GetDbPath();
        }

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
