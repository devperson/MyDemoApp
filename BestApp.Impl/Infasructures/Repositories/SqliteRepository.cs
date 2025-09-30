using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.General.Infasructures;
using BestApp.Impl.Cross.Infasructures.Repositories.Tables;
using Common.Abstrtactions;
using MapsterMapper;
using SQLite;

namespace BestApp.Impl.Cross.Infasructures.Repositories
{
    internal class SqliteRepository<TEntity, Table> : IRepository<TEntity>
        where TEntity : Entity
        where Table : ITable, new()
    {
        private SQLiteAsyncConnection database;
        private readonly IDirectoryService directoryService;
        private readonly ILoggingService loggingService;
        private readonly IMapper mapper;

        public SqliteRepository(IDirectoryService directoryService, ILoggingService loggingService, IMapper mapper, SQLiteAsyncConnection database)
        {           
            this.directoryService = directoryService;
            this.loggingService = loggingService;
            this.mapper = mapper;
            this.database = database;
        }        

        private readonly SemaphoreSlim dbLock = new SemaphoreSlim(1, 1);       
        public async Task Add(TEntity entity)
        {
            await dbLock.WaitAsync();
            try
            {
                var record = mapper.Map<Table>(entity);
                //var dbTable = database.Table<Table>();
                await database.InsertAsync(record);                
            }
            finally
            {
                dbLock.Release();
            }
        }

        public async Task<List<TEntity>> Take(int count, int skip)
        {
            var list = await database.Table<Table>().Skip(skip).Take(count).ToListAsync();
            var entities = list.Select(s => mapper.Map<TEntity>(s)).ToList();
            return entities;    
        }

        public async Task<TEntity> FindById(int id)
        {
            var row = await database.Table<Table>().FirstOrDefaultAsync(x => x.Id == id);
            var entity = mapper.Map<TEntity>(row);

            return entity;
        }

        public async Task Remove(TEntity entity)
        {
            var record = mapper.Map<Table>(entity);
            await database.DeleteAsync(record);
        }

        public async Task UpdateItem<T>(T item)
        {
            await dbLock.WaitAsync();
            try
            {
                await database.UpdateAsync(item);
            }
            finally
            {
                dbLock.Release();
            }
        }

    }
}
