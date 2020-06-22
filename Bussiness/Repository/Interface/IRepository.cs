using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Bussiness.Repository.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);

        IEnumerable<TEntity> GetAll(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo = null,
            int pageSize = 10,
            int pageIndex = 1);

        IEnumerable<TEntity> Search(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo = null,
            int pageSize = 10,
            int pageIndex = 1);


        int CountAll();

        #region Generic Delegates With Filter And Includes
        IEnumerable<TEntity> GetAllWithFilterAndRelated(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
            Expression<Func<TEntity, bool>> filter = null,
            List<Expression<Func<TEntity, object>>> paths = null,
            int pageSize = 10,
            int pageIndex = 1,
            int userDepartment = 0);

        IEnumerable<TEntity> SearchAllWithFilterAndRelated(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo = null,
            Expression<Func<TEntity, bool>> filter = null,
            List<Expression<Func<TEntity, object>>> paths = null,
            int pageSize = 10,
            int pageIndex = 1,
            int userDepartment = 0);

        int CountAllWithFilter(Expression<Func<TEntity, bool>> filter = null, int userDepartment = 0);
        #endregion

        int Count(Expression<Func<TEntity, bool>> predicate);

        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        bool Any(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        //void Update(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Select(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetAllWithRelated(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderingInfo,
            Expression<Func<TEntity, bool>> filter = null,
            List<Expression<Func<TEntity, object>>> paths = null);

        IEnumerable<TEntity> GetAllWithoutPaging();
    }
}
