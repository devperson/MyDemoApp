using Base.Abstractions.PlatformServices;

namespace UnitTest.InfrasImpl.Impl
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
            var dbDir = Path.Combine(myDir, "InfraTestDatabaseDir");
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
            var cacheDir = Path.Combine(myDir, "InfraTestDirCache");
            if (!Directory.Exists(cacheDir))
            {
                Directory.CreateDirectory(cacheDir);
            }

            return cacheDir;
        }

        public string GetAppDataDir()
        {
            var myDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var dataDir = Path.Combine(myDir, "InfraTestDataDir");
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            return dataDir;
        }
    }
}
