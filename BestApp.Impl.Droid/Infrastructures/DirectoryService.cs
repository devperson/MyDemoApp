using BestApp.Abstraction.General.Infasructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Droid.Infrastructures
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
            var dbPath = Path.Combine(myDir, "Database");
            if (!Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }

            return dbPath;
        }
    }
}
