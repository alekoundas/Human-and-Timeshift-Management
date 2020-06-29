using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Bussiness.Repository.Interface;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Bussiness.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly BaseDbContext Context;
        protected readonly DbSet<TEntity> _set;

        // Here we assign the set of entities in the _Set private field in order to avoid
        // extra noise that would be created when in each method we would have to 
        // call things like: 
        // return Context.Set<TEntity>().ToList(); 
        // or 
        // return Context.Set<TEntity>().SingleOrDefault() etc;
        public BaseRepository(BaseDbContext context)
        {
            Context = context;
            _set = Context.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetWithPagging(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
            int pageSize = 10,
            int pageIndex = 1)
        {

            if (orderingInfo == null)
                return await _set.Take(pageSize).ToListAsync();

            var qry = (IQueryable<TEntity>)orderingInfo(_set);
            qry = qry.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return await qry.ToListAsync();
        }

        public IEnumerable<TEntity> GetAllWithoutPaging()
        {
            return _set.ToList();
        }

        public async Task<int> CountAllAsync()
        {
            return await _set.CountAsync();
        }

        public async Task<List<TEntity>> GetPaggingWithFilter(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
            Expression<Func<TEntity, bool>> filter,
            List<Expression<Func<TEntity, object>>> includes = null,
            int pageSize = 10,
            int pageIndex = 1)
        {
            var qry = (IQueryable<TEntity>)_set;

            if (includes != null)
                foreach (var include in includes)
                    qry = qry.Include(include);

            if (filter != null)
                qry = qry.Where(filter);

            if (orderingInfo != null)
                qry = orderingInfo(qry);

            qry = qry.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return await qry.ToListAsync();
        }


        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.Count(predicate);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.SingleOrDefault(predicate);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.FirstOrDefault(predicate);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.Any(predicate);
        }

        public void Add(TEntity entity)
        {
            _set.Add(entity);
        }

        public void Select(Expression<Func<TEntity, bool>> predicate)
        {
            _set.Select(predicate);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _set.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            _set.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _set.RemoveRange(entities);
        }

        public async Task<TEntity> FindAsync(int id)
        {
            return await _set.FindAsync(id);
        }

        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return _set.Where(expression);
        }

    }
}

