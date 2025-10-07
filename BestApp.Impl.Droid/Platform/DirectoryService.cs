using BestApp.Abstraction.General.Infasructures;
using BestApp.Abstraction.General.Platform;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Droid.Platform
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
