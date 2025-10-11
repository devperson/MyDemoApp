using Base.Abstractions.Domain;

namespace Base.Infrastructures.Abstractions.Repository
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        //TEntity FindById(Guid id);
        Task<TEntity> FindById(int id);
        Task<List<TEntity>> GetList(int count = -1, int skip = 0);
        //TEntity FindOne(ISpecification<TEntity> spec);
        //IEnumerable<TEntity> Find(ISpecification<TEntity> spec);
        Task<int> AddAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        Task<int> AddAllAsync(List<TEntity> entities);
        Task<int> RemoveAsync(TEntity entity);
        Task<int> ClearAsync(string reason);
    }

    public interface ILocalDbInitilizer
    {
        object GetDbConnection();
        Task Init();
        Task Release(bool closeConnection = false);
    }
}
