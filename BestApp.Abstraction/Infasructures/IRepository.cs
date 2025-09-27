using BestApp.Abstraction.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestApp.Abstraction.General.Infasructures
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        //TEntity FindById(Guid id);
        TEntity FindById(int id);
        List<TEntity> Take(int count, int skip);
        //TEntity FindOne(ISpecification<TEntity> spec);
        //IEnumerable<TEntity> Find(ISpecification<TEntity> spec);
        void Add(TEntity entity);
        void Remove(TEntity entity);
    }
}
