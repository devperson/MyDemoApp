using BestApp.Abstraction.General.Infasructures;
using BestApp.Abstraction.General.Platform;
using Logging.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross.Infasructures.Repositories
{
    [LogMethods]
    internal class DatabaseInfo : IDatabaseInfo
    {
        private readonly IDirectoryService directoryService;

        public string DbsFolderName { get; }
        public string DbExtenstion { get; }
        public string DbName { get; }

        public DatabaseInfo(IDirectoryService directoryService)
        {
            this.directoryService = directoryService;

            DbsFolderName = "Databases";
            DbExtenstion = ".db3";
            DbName = $"AppDb{DbExtenstion}";            
        }

        public string GetDbPath()
        {            
            var dbFolder = Path.Combine(directoryService.GetAppDataDir(), DbsFolderName);
            if (!Directory.Exists(dbFolder))
            {
                Directory.CreateDirectory(dbFolder);
            }

            var dbPath = Path.Combine(dbFolder, DbName);

            return dbPath;
        }
    }
}
