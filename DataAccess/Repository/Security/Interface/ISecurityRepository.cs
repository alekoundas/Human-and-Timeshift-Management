using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repository.Security.Interface
{
    public interface ISecurityRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);

        Task<List<TEntity>> GetWithPagging(
         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
         Expression<Func<TEntity, bool>> filter,
         int pageSize = 10,
         int pageIndex = 1);


        Task<List<TEntity>> GetAllAsync();
        int CountAll();

        Task<int> CountAllAsync();

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
