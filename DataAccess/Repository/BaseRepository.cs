﻿using DataAccess.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly BaseDbContext Context;
        protected readonly DbSet<TEntity> _set;

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

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        public async Task<List<TResult>> SelectAllAsync<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return (List<TResult>)await _set.Select(selector).ToListAsync();
        }


        public async Task<int> CountAllAsync()
        {
            return await _set.CountAsync();
        }
        public async Task<int> CountAllAsyncFiltered(Expression<Func<TEntity, bool>> selector)
        {
            return await _set.Where(selector).CountAsync();
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

            if (pageSize != -1)
                qry = qry.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return await qry.ToListAsync();
        }
        public async Task<List<TEntity>> GetPaggingWithFilter<TSource>(
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
           List<string> selectFields,
           Expression<Func<TEntity, bool>> filter,
           List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> includes = null,
           int pageSize = 10,
           int pageIndex = 1)
        {

            var qry = (IQueryable<TEntity>)_set;

            if (includes != null)
                foreach (var include in includes)
                    qry = include(qry);

            //if (selectFields != null)
            //    qry = qry.Select(x=> SelectObject(selectFields,x));

            if (filter != null)
                qry = qry.Where(filter);

            if (orderingInfo != null)
                qry = orderingInfo(qry);

            if (pageSize != -1)
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

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _set.FirstOrDefaultAsync(predicate);
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
        public void Select(Func<TEntity, TEntity> predicate)
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
        public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> filter,
            List<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> includes = null)
        {
            var qry = (IQueryable<TEntity>)_set;

            if (includes != null)
                foreach (var include in includes)
                    qry = include(qry);

            return await qry.FirstOrDefaultAsync(filter);

        }

        public async Task<List<TEntity>> GetFiltered(Expression<Func<TEntity, bool>> filter)
        {
            return await _set.Where(filter).ToListAsync();
        }

        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return _set.Where(expression);
        }



    }
}