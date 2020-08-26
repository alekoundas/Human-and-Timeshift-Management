using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Repository.Security.Interface
{
    public interface ISecurityRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);

         Task<List<TEntity>> GetWithPagging(
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
          Expression<Func<TEntity, bool>> filter,
          int pageSize = 10,
          int pageIndex = 1);

      

        int CountAll();

        int Count(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        bool Any(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        void Update(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

    }
}
