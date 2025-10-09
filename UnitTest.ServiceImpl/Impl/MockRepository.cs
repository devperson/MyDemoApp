using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.Main.Infasructures;
using BestApp.Impl.Cross.Infasructures.Repositories.Tables;
using Logging.Aspects;
using MapsterMapper;

namespace BestApp.Impl.Cross.Infasructures.Repositories
{
    [LogMethods]
    internal class MockRepository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        protected static List<TEntity> records = new List<TEntity>();
        private readonly IMapper mapper;

        public MockRepository(IMapper mapper) 
        {
            this.mapper = mapper;
        }

        public virtual Task<TEntity> FindById(int id)
        {
            var row = records.Where(x => x.Id == id).FirstOrDefault();            

            return Task.FromResult(row);
        }

        public virtual Task<List<TEntity>> GetList(int count = -1, int skip = 0)
        {
            var rows = records.Skip(skip).Take(count).ToList();            

            return Task.FromResult(rows);
        }

        public virtual Task Add(TEntity entity)
        {
            var lastRecord = records.LastOrDefault();
            if (lastRecord != null)
                entity.Id = lastRecord.Id + 1;
            else
                entity.Id = 1;

            records.Add(entity);

            return Task.CompletedTask;
        }

        public virtual Task Remove(TEntity entity)
        {            
            records.Remove(entity);

            return Task.CompletedTask;
        }

        public virtual async Task Update(TEntity entity)
        {
            var old = await FindById(entity.Id);
            records.Remove(old);
            records.Add(entity);
        }

        public Task AddAll(List<TEntity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Id = i + 1;
            }
            records.Clear();
            records.AddRange(entities);

            return Task.CompletedTask;
        }

        public Task ClearAsync(string reason)
        {
            records.Clear();

            return Task.CompletedTask;
        }

        //public TEntity FindOne(ISpecification<TEntity> spec)
        //{
        //    return entities.Where(spec.IsSatisfiedBy).FirstOrDefault();
        //}

        //public IEnumerable<TEntity> Find(ISpecification<TEntity> spec)
        //{
        //    return entities.Where(spec.IsSatisfiedBy);
        //}
    }
}
