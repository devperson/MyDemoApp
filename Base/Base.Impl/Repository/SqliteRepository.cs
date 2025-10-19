using Base.Abstractions.Domain;
using Base.Abstractions.Repository;
using Base.Aspect;
using Base.Infrastructures.Abstractions.Repository;
using MapsterMapper;
using SQLite;
using System.Data;

namespace BestApp.Impl.Cross.Infasructures.Repositories
{
    [LogMethods]
    public class SqliteRepository<TEntity, Tb> : IRepository<TEntity>
        where TEntity : IEntity
        where Tb : ITable, new()
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
            List<Tb> list = null;
            if(count == -1)
            {
                list = await database.Table<Tb>().ToListAsync();
            }
            else
            {
                list = await database.Table<Tb>().Skip(skip).Take(count).ToListAsync();
            }            
            var entities = list.Select(s => mapper.Value.Map<TEntity>(s)).ToList();
            return entities;    
        }

        public async Task<TEntity> FindById(int id)
        {
            EnsureInitalized();

            var row = await database.Table<Tb>().FirstOrDefaultAsync(x => x.Id == id);
            var entity = mapper.Value.Map<TEntity>(row);

            return entity;
        }

       
        public async Task<int> AddAsync(TEntity entity)
        {
            await semaphor.WaitAsync();
            try
            {                
                EnsureInitalized();
                var record = mapper.Value.Map<Tb>(entity);
                //var dbTable = database.Table<Table>();
                var res = await database.InsertAsync(record);
                entity.Id = record.Id;

                return res;
            }
            finally
            {
                semaphor.Release();
            }
        }

        public async Task<int> RemoveAsync(TEntity entity)
        {
            await semaphor.WaitAsync();
            try
            {  
                EnsureInitalized();

                var record = mapper.Value.Map<Tb>(entity);                
                var res = await database.DeleteAsync(record);
                return res;
            }
            finally
            {
                semaphor.Release();
            }
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            await semaphor.WaitAsync();

            try
            {                
                EnsureInitalized();

                var record = mapper.Value.Map<Tb>(entity);
                var res = await database.UpdateAsync(record);
                return res;
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

        public async Task<int> ClearAsync(string reason)
        {
            await semaphor.WaitAsync();
            try
            {
                //logging events
                EnsureInitalized();
                //get all ids for tracking
                var tableName = typeof(Tb).Name; 
                var ids = await database.QueryScalarsAsync<int>($"SELECT Id FROM {tableName}");
                //delete all records
                var res = await database.DeleteAllAsync<Tb>();
                ////track event
                //var customValues = string.Join(",", ids);
                //var deleteEvent = EventsTb.Create(tableName, "DELETE", reason, customValues);
                //await database.InsertAsync(deleteEvent);

                return res;
            }
            finally
            {
                semaphor.Release();
            }
        }

        public async Task<int> AddAllAsync(List<TEntity> entities)
        {
            await semaphor.WaitAsync();
            try
            {
                EnsureInitalized();
                var records = entities.Select(s=> mapper.Value.Map<Tb>(s)).ToList();
                var res = await database.InsertAllAsync(records);

                //set id
                for (int i = 0; i < records.Count; i++)
                {
                    entities[i].Id = records[i].Id;
                }

                return res;
            }
            finally
            {
                semaphor.Release();
            }
        }
    }
}
