using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace Bussiness.Repository.Interface
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> FindAsync(int id);

        Task<List<TEntity>> GetWithPagging(
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo = null,
             int pageSize = 10,
             int pageIndex = 1);



        Task<int> CountAllAsync();
        Task<int> CountAllAsyncFiltered(Expression<Func<TEntity, bool>> filter);
        Task<List<TResult>> SelectAllAsync<TResult>(Expression<Func<TEntity, TResult>> selector);

        Task<List<TEntity>> GetPaggingWithFilter(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
            Expression<Func<TEntity, bool>> filter = null,
            List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> includes = null,
            int pageSize = 10,
            int pageIndex = 1);

        Task<List<TEntity>> GetPaggingWithFilter<TSource>(
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
             List<string> selectFields,
             Expression<Func<TEntity, bool>> filter,
             List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> includes = null,
             int pageSize = 10,
             int pageIndex = 1);


        Task<List<TEntity>> GetFiltered(Expression<Func<TEntity, bool>> filter);
        Task<List<TEntity>> GetAllAsync();
        int Count(Expression<Func<TEntity, bool>> predicate);

        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        bool Any(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        //void Update(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> filter, List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> includes = null);
        void Select(Func<TEntity, TEntity> predicate);
        //void Select(Expression<Func<TEntity, bool>> predicate);


    }
}
