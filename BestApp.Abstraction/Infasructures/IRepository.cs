using BestApp.Abstraction.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.Main.Infasructures
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        //TEntity FindById(Guid id);
        Task<TEntity> FindById(int id);
        Task<List<TEntity>> GetList(int count = -1, int skip = 0);
        //TEntity FindOne(ISpecification<TEntity> spec);
        //IEnumerable<TEntity> Find(ISpecification<TEntity> spec);
        Task Add(TEntity entity);
        Task AddAll(List<TEntity> entities);
        Task Remove(TEntity entity);
        Task Clear(string reason);
    }

    public interface ILocalDbInitilizer
    {
        object GetDbConnection();
        Task Init();
        Task Release(bool closeConnection = false);
    }
}
