using BestApp.Abstraction.General.Infasructures;
using BestApp.Abstraction.General.Platform;
using BestApp.Impl.Cross.Infasructures.Repositories.Tables;
using Common.Abstrtactions;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross.Infasructures.Repositories
{
    internal class SqliteDbInitilizer : ILocalDbInitilizer
    {
        public SqliteDbInitilizer(Lazy<IDirectoryService> directoryService, Lazy<ILoggingService> loggingService)
        {
            this.directoryService = directoryService;
            this.loggingService = loggingService;
        }
        private SQLiteAsyncConnection database;
        private bool isInited;
        private readonly Lazy<IDirectoryService> directoryService;
        private readonly Lazy<ILoggingService> loggingService;

        public async Task Init()
        {
            if (!isInited)
            {
                isInited = true;
                if (database != null)
                {
                    await database.CloseAsync();
                }

                var path = directoryService.Value.GetDbPath();
                database = new SQLiteAsyncConnection(path);

                await database.CreateTableAsync<EventsTb>();
                await database.CreateTableAsync<ProductTable>();
                await database.CreateTableAsync<MoviewTb>();

                loggingService.Value.Log($"SqliteDbInitilizer is inited! path: {path}");
            }
            else
            {
                loggingService.Value.LogWarning($"SqliteDbInitilizer skip Init() because isInited:True");
            }
        }

        public async Task Release(bool closeConnection = false)
        {
            isInited = false;

            //closeConnection is false by default because after closing database  can not be used again it will require app restart.
            if (closeConnection)
            {
                await database.CloseAsync();
                database = null;
            }
        }

        public object GetDbConnection()
        {
            return database;
        }
    }
}
