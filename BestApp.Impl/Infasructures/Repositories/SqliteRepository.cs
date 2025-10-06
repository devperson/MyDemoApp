using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.General.Infasructures;
using BestApp.Impl.Cross.Infasructures.Repositories.Tables;
using Logging.Aspects;
using MapsterMapper;
using SQLite;
using System.Data;

namespace BestApp.Impl.Cross.Infasructures.Repositories
{
    [LogMethods]
    internal class SqliteRepository<TEntity, Table> : IRepository<TEntity>
        where TEntity : Entity
        where Table : ITable, new()
    {
        
        private SQLiteAsyncConnection database;
        private static SemaphoreSlim semaphor { get; set; } = new SemaphoreSlim(1, 1);                
        private readonly Lazy<IMapper> mapper;
        private readonly Lazy<ILocalDbInitilizer> dbConnectionInitilizer;
        private bool isInited;

        public SqliteRepository(Lazy<IMapper> mapper, Lazy<ILocalDbInitilizer> dbConnectionInitilizer)
        {   
            this.mapper = mapper;
            this.dbConnectionInitilizer = dbConnectionInitilizer;
        }

        
        public async Task<List<TEntity>> GetList(int count=-1, int skip = 0)
        {
            EnsureInitalized();

            List<Table> list = null;
            if(count == -1)
            {
                list = await database.Table<Table>().ToListAsync();
            }
            else
            {
                list = await database.Table<Table>().Skip(skip).Take(count).ToListAsync();
            }            
            var entities = list.Select(s => mapper.Value.Map<TEntity>(s)).ToList();
            return entities;    
        }

        public async Task<TEntity> FindById(int id)
        {
            EnsureInitalized();

            var row = await database.Table<Table>().FirstOrDefaultAsync(x => x.Id == id);
            var entity = mapper.Value.Map<TEntity>(row);

            return entity;
        }

       
        public async Task Add(TEntity entity)
        {
            await semaphor.WaitAsync();
            try
            {                
                EnsureInitalized();
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
                EnsureInitalized();

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
                EnsureInitalized();

                await database.UpdateAsync(item);
            }
            finally
            {
                semaphor.Release();
            }
        }

        private void EnsureInitalized()
        {
            //get singleton database connection
            if (database == null)
            {
                database = dbConnectionInitilizer.Value.GetDbConnection() as SQLiteAsyncConnection;
                if (database == null)
                {
                    throw new InvalidOperationException("database is null, it seems it doesn't initilized");
                }
            }
        }

        public async Task Clear(string reason)
        {
            await semaphor.WaitAsync();
            try
            {
                //logging events
                EnsureInitalized();
                //get all ids for tracking
                var tableName = typeof(Table).Name; 
                var ids = await database.QueryScalarsAsync<int>($"SELECT Id FROM {tableName}");
                //delete all records
                await database.DeleteAllAsync<Table>();
                //track event
                var customValues = string.Join(",", ids);
                var deleteEvent = EventsTb.Create(tableName, "DELETE", reason, customValues);
                await database.InsertAsync(deleteEvent);
            }
            finally
            {
                semaphor.Release();
            }
        }

        public async Task AddAll(List<TEntity> entities)
        {
            await semaphor.WaitAsync();
            try
            {
                EnsureInitalized();
                var records = entities.Select(s=> mapper.Value.Map<Table>(s)).ToList();
                await database.InsertAllAsync(records);

                //set id
                for (int i = 0; i < records.Count; i++)
                {
                    entities[i].Id = records[i].Id;
                }
            }
            finally
            {
                semaphor.Release();
            }
        }
    }
}
