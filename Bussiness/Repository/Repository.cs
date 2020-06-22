using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Bussiness.Repository.Interface;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Bussiness.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly BaseDbContext Context;
        protected readonly DbSet<TEntity> _set;

        // Here we assign the set of entities in the _Set private field in order to avoid
        // extra noise that would be created when in each method we would have to 
        // call things like: 
        // return Context.Set<TEntity>().ToList(); 
        // or 
        // return Context.Set<TEntity>().SingleOrDefault() etc;
        public Repository(BaseDbContext context)
        {
            Context = context;
            _set = Context.Set<TEntity>();
        }


        public IEnumerable<TEntity> GetAll(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
            int pageSize = 10,
            int pageIndex = 1)
        {

            if (orderingInfo == null)
                return _set.Take(pageSize).ToList();


            var qry = (IQueryable<TEntity>)orderingInfo(_set);

            qry = qry.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return qry.ToList();
        }

        public IEnumerable<TEntity> GetAllWithoutPaging()
        {
            return _set.ToList();
        }

        public IEnumerable<TEntity> Search(
            Expression<Func<TEntity, bool>> predicate,
            //Expression<Func<TEntity, object>>[] includes=null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo = null,
            int pageSize = 10,
            int pageIndex = 1)
        {

            //IQueryable<TEntity> query; 
            //foreach (var include in includes)
            //{
            //    query = _set.Include(include);
            //}

            if (orderingInfo == null)
                return _set.Where(predicate).ToList();


            var qry = _set.Where(predicate);
            qry = orderingInfo(qry).AsQueryable();
            qry = qry.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return qry.ToList();
        }

        public int CountAll()
        {
            return _set.Count();
        }

        #region Generic Delegates With Filter And Includes
        public IEnumerable<TEntity> GetAllWithFilterAndRelated(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
            Expression<Func<TEntity, bool>> filter = null,
            List<Expression<Func<TEntity, object>>> paths = null,
            int pageSize = 10,
            int pageIndex = 1,
            int userDepartment = 0)
        {
            if (userDepartment != 1 && filter == null)
                return new List<TEntity>();

            var qry = (IQueryable<TEntity>)_set;

            if (paths != null)
                foreach (var path in paths)
                    qry = qry.Include(path);

            if (filter != null)
                qry = qry.Where(filter);

            if (orderingInfo != null)
                qry = orderingInfo(qry);

            qry = qry.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return qry.ToList();
        }

        public IEnumerable<TEntity> GetAllWithRelated(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
            Expression<Func<TEntity, bool>> filter = null,
            List<Expression<Func<TEntity, object>>> paths = null)
        {
            var qry = (IQueryable<TEntity>)_set;

            if (paths != null)
                foreach (var path in paths)
                    qry = qry.Include(path);

            if (filter != null)
                qry = qry.Where(filter);

            if (orderingInfo != null)
                qry = orderingInfo(qry);

            return qry.ToList();
        }

        public IEnumerable<TEntity> SearchAllWithFilterAndRelated(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo = null,
            Expression<Func<TEntity, bool>> filter = null,
            List<Expression<Func<TEntity, object>>> paths = null,
            int pageSize = 10,
            int pageIndex = 1,
            int userDepartment = 0)
        {
            if (userDepartment != 1 && filter == null)
                return new List<TEntity>();

            if (orderingInfo == null)
            {
                if (filter == null)
                    return _set.Where(predicate).ToList();
                else
                    return _set.Where(predicate).Where(filter).ToList();
            }

            var qry = (IQueryable<TEntity>)_set;

            if (paths != null)
                foreach (var path in paths)
                    qry = qry.Include(path);

            if (filter != null)
                qry = qry.Where(filter);

            qry = qry.Where(predicate);

            if (orderingInfo != null)
                qry = orderingInfo(qry).AsQueryable();

            qry = qry.Skip((pageIndex - 1) * pageSize).Take(pageSize);

            return qry.ToList();
        }

        public int CountAllWithFilter(Expression<Func<TEntity, bool>> filter = null, int userDepartment = 0)
        {
            if (userDepartment != 1 && filter == null)
                return 0;
            //if (filter == null)
            //    return _set.Count();

            if (filter != null)
                return _set.Where(filter).Count();
            return _set.Count();
        }
        #endregion

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

        //public void Update(TEntity entity)
        //{
        //    var entry = Context.Entry(entity);
        //    foreach (var dbEntityEntry in Context.ChangeTracker.Entries<IEntityState>())
        //    {
        //        IEntityState entityStateInfo = dbEntityEntry.Entity;
        //        dbEntityEntry.State = EntityState.Modified;
        //    }
        //    entry.State = EntityState.Modified;
        //}

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

        public TEntity Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}

