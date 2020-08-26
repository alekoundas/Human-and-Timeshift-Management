using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Models.Identity;
using DataAccess;
using Microsoft.AspNetCore.Identity;
using Bussiness.Repository.Security.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bussiness.Repository
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

        public TEntity Get(int id)
        {
            return _set.Find(id);
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

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _set.FirstOrDefault(predicate);
        }
        public async Task<TEntity >FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
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

    }
}
