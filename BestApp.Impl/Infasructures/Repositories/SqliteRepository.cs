using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.General.Infasructures;
using BestApp.Abstraction.General.Platform;
using BestApp.Impl.Cross.Infasructures.Repositories.Tables;
using Common.Abstrtactions;
using Logging.Aspects;
using MapsterMapper;
using SQLite;

namespace BestApp.Impl.Cross.Infasructures.Repositories
{
    [LogMethods]
    internal class SqliteRepository<TEntity, Table> : IRepository<TEntity>
        where TEntity : Entity
        where Table : ITable, new()
    {
        
        private static SQLiteAsyncConnection database;
        private static SemaphoreSlim semaphor { get; set; } = new SemaphoreSlim(1, 1);
        private readonly Lazy<IDirectoryService> directoryService;
        private readonly Lazy<ILoggingService> loggingService;
        private readonly Lazy<IMapper> mapper;
        private bool isInited;

        public SqliteRepository(Lazy<IDirectoryService> directoryService, Lazy<ILoggingService> loggingService, Lazy<IMapper> mapper)
        {
            this.directoryService = directoryService;
            this.loggingService = loggingService;
            this.mapper = mapper;
        }

        
        public async Task EnsureInitalized()
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

                await database.CreateTableAsync<ProductTable>();

                loggingService.Value.Log($"SqliteRepository is inited! path: {path}");
            }            
        }

        
        public async Task<List<TEntity>> Take(int count, int skip)
        {
            await EnsureInitalized();

            var list = await database.Table<Table>().Skip(skip).Take(count).ToListAsync();
            var entities = list.Select(s => mapper.Value.Map<TEntity>(s)).ToList();
            return entities;    
        }

        public async Task<TEntity> FindById(int id)
        {
            await EnsureInitalized();

            var row = await database.Table<Table>().FirstOrDefaultAsync(x => x.Id == id);
            var entity = mapper.Value.Map<TEntity>(row);

            return entity;
        }

        
        public async Task Add(TEntity entity)
        {
            await semaphor.WaitAsync();
            try
            {                
                await EnsureInitalized();
                var record = mapper.Value.Map<Table>(entity);
                //var dbTable = database.Table<Table>();
                await database.InsertAsync(record);

                entity.Id = record.Id;
            }
            finally
            {
                semaphor.Release();
            }
        }

        public async Task Remove(TEntity entity)
        {
            await semaphor.WaitAsync();
            try
            {  
                await EnsureInitalized();

                var record = mapper.Value.Map<Table>(entity);
                await database.DeleteAsync(record);
            }
            finally
            {
                semaphor.Release();
            }
        }

        public async Task UpdateItem<T>(T item)
        {   
            try
            {
                await semaphor.WaitAsync();
                await EnsureInitalized();

                await database.UpdateAsync(item);
            }
            finally
            {
                semaphor.Release();
            }
        }

        public async ValueTask Release(bool closeConnection = false)
        {
            isInited = false;

            //closeConnection is false by default because after closing database  can not be used again it will require app restart.
            if (closeConnection)
            {
                await database.CloseAsync();
                database = null;
            }
        }

    }
}
