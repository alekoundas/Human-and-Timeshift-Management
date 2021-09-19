using DataAccess.Repository.Security.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repository
{

    public class SecurityRepository<TEntity> : ISecurityRepository<TEntity> where TEntity : class
    {
        protected readonly SecurityDbContext Context;
        protected readonly DbSet<TEntity> _set;


        public SecurityRepository(SecurityDbContext dbContext)
        {
            Context = dbContext;
            _set = Context.Set<TEntity>();
        }
        public IQueryable<TEntity> Query => Context.Set<TEntity>();

        public async Task<int> CountAllAsyncFiltered(Expression<Func<TEntity, bool>> selector)
        {
            return await _set.Where(selector).CountAsync();
        }

        public TEntity Get(int id)
        {
            return _set.Find(id);
        }

        public IQueryable<TEntity> GetWithFilterQueryable(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
            Expression<Func<TEntity, bool>> filter,
            List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> includes = null,
            int pageSize = 10,
            int pageIndex = 1)
        {
            var qry = (IQueryable<TEntity>)_set;

            if (includes != null)
                foreach (var include in includes)
                    qry = include(qry);

            if (filter != null)
                qry = qry.Where(filter);

            if (orderingInfo != null)
                qry = orderingInfo(qry);

            return qry;
        }

        public async Task<List<TEntity>> GetPaggingWithFilter(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
            Expression<Func<TEntity, bool>> filter,
            List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> includes = null,
            int pageSize = 10,
            int pageIndex = 1)
        {
            var qry = (IQueryable<TEntity>)_set;
            //qry = qry.AsExpandable();

            if (includes != null)
                foreach (var include in includes)
                    qry = include(qry);

            if (filter != null)
                qry = qry.Where(filter);

            if (orderingInfo != null)
                qry = orderingInfo(qry);

            if (pageSize != -1 && pageSize != 0)
                qry = qry.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return await qry.ToListAsync();
        }
        public async Task<List<TEntity>> GetWithFilter(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
            Expression<Func<TEntity, bool>> filter,
            List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> includes = null)
        {
            var qry = (IQueryable<TEntity>)_set;
            //qry = qry.AsExpandable();

            if (includes != null)
                foreach (var include in includes)
                    qry = include(qry);

            if (filter != null)
                qry = qry.Where(filter);

            if (orderingInfo != null)
                qry = orderingInfo(qry);

            return await qry.ToListAsync();
        }

        public async Task<List<TEntity>> GetWithPagging(
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
            Expression<Func<TEntity, bool>> filter,
           int pageSize = 10,
           int pageIndex = 1)
        {
            var qry = (IQueryable<TEntity>)_set;

            if (orderingInfo != null)
                qry = (IQueryable<TEntity>)orderingInfo(_set);

            if (filter != null)
                qry = qry.Where(filter);

            if (pageSize != -1)
                qry = qry.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return await qry.ToListAsync();

        }

        public int CountAll()
        {
            return _set.Count();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.Count(predicate);
        }

        public async Task<int> CountAllAsync()
        {
            return await _set.CountAsync();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.FirstOrDefault(predicate);
        }
        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _set.FirstOrDefaultAsync(predicate);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.SingleOrDefault(predicate);
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.Any(predicate);
        }

        public void Add(TEntity entity)
        {
            _set.Add(entity);
        }

        public IEnumerable<TEntity> GetAllWithoutPaging()
        {
            return _set.ToList();
        }

        public void Update(TEntity entity)
        {
            Context.Attach(entity);
            var entry = Context.Entry(entity);
            entry.State = EntityState.Modified;
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

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

    }
}
