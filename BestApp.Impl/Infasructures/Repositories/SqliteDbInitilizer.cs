using BestApp.Abstraction.Main.Infasructures;
using BestApp.Abstraction.Main.Platform;
using BestApp.Impl.Cross.Infasructures.Repositories.Tables;
using Common.Abstrtactions;
using Logging.Aspects;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Impl.Cross.Infasructures.Repositories
{
    [LogMethods]
    internal class SqliteDbInitilizer : ILocalDbInitilizer
    {
        public SqliteDbInitilizer(Lazy<IDatabaseInfo> databaseInfo, Lazy<ILoggingService> loggingService)
        {
            this.databaseInfo = databaseInfo;
            this.loggingService = loggingService;
        }
        private SQLiteAsyncConnection database;
        private bool isInited;
        private readonly Lazy<IDatabaseInfo> databaseInfo;
        private readonly Lazy<ILoggingService> loggingService;

        public async Task Init()
        {
            try
            {
                if (!isInited)
                {
                    isInited = true;
                    if (database != null)
                    {
                        await database.CloseAsync();
                    }

                    var path = databaseInfo.Value.GetDbPath();
                    database = new SQLiteAsyncConnection(path);

                    await database.CreateTableAsync<EventsTb>();
                    await database.CreateTableAsync<ProductTb>();
                    await database.CreateTableAsync<MovieTb>();

                    loggingService.Value.Log($"SqliteDbInitilizer is inited! path: {path}");
                }
                else
                {
                    loggingService.Value.LogWarning($"SqliteDbInitilizer skip Init() because isInited:True");
                }
            }
            catch (Exception ex)
            {                
                loggingService.Value.TrackError(ex);
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
