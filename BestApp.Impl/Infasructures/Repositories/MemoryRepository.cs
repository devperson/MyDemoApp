using BestApp.Abstraction.Domain.Entities;
using BestApp.Abstraction.General.Infasructures;
using BestApp.Impl.Cross.Infasructures.Repositories.Tables;
using MapsterMapper;

namespace BestApp.Impl.Cross.Infasructures.Repositories
{
    public class MemoryRepository<TEntity, Table> : IRepository<TEntity>
        where TEntity : Entity
        where Table : ITable
    {
        protected static List<ITable> records = new List<ITable>();
        private readonly IMapper mapper;

        public MemoryRepository(IMapper mapper) 
        {
            this.mapper = mapper;
        }

        public virtual TEntity FindById(int id)
        {
            var row = records.Where(x => x.Id == id).FirstOrDefault();
            var entity = mapper.Map<TEntity>(row);

            return entity;
        }

        public virtual List<TEntity> Take(int count, int skip)
        {
            var rows = records.Skip(skip).Take(count).ToList();
            var entities = rows.Select(mapper.Map<TEntity>).ToList();

            return entities;
        }

        public virtual void Add(TEntity entity)
        {
            entity.Id = entity.Id + 1;
            var table = mapper.Map<Table>(entity);
            
            records.Add(table);
        }

        public virtual void Remove(TEntity entity)
        {
            var table = mapper.Map<Table>(entity);
            records.Remove(table);
        }

        public virtual void Update(TEntity entity)
        {
            var table = mapper.Map<Table>(entity);

            records.Remove(table);
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
