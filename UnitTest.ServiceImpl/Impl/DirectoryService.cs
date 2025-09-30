using BestApp.Abstraction.General.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.ServiceImpl.Impl
{
    internal class DirectoryService : IDirectoryService
    {
        public string DbsFolderName { get; }
        public string DbExtenstion { get; }
        public string DbName_Suffix { get; }

        public DirectoryService()
        {
            DbsFolderName = "Databases";
            DbExtenstion = ".db3";
            DbName_Suffix = $"db{DbExtenstion}";
        }

        public string GetDbPath()
        {
            var myDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var dbDir = Path.Combine(myDir, "testDatabaseDir");
            if (!Directory.Exists(dbDir))
            {
                Directory.CreateDirectory(dbDir);
            }

            var dbPath = Path.Combine(dbDir, DbName_Suffix);

            return dbPath;
        }

        public string GetCacheDir()
        {
            var myDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var cacheDir = Path.Combine(myDir, "testDirCache");
            if (!Directory.Exists(cacheDir))
            {
                Directory.CreateDirectory(cacheDir);
            }

            return cacheDir;
        }

        public string GetAppDataDir()
        {
            var myDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var dataDir = Path.Combine(myDir, "testDataDir");
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            return dataDir;
        }
    }
}
