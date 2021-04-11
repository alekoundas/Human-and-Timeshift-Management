using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Bussiness.Helpers
{
    public static class ExpressionTreeHelper
    {
        public static Expression<Func<TEntity, bool>> WherePropertyEquals<TEntity>(string propertyName, string propertyValue)
        {

            //x =>
            var xx = Expression.Parameter(typeof(TEntity), "x");
            var member = Expression.Property(xx, propertyName);

            //var member1 = Expression.Property(xx, propertyName);
            var propertyType = ((PropertyInfo)member.Member).PropertyType;
            var converter = TypeDescriptor.GetConverter(propertyType);
            if (!converter.CanConvertFrom(typeof(string)))
                throw new NotSupportedException();
            var propertyValuee = converter.ConvertFromInvariantString(propertyValue);
            var constant = Expression.Constant(propertyValuee);
            var whatisthis = Expression.Convert(constant, propertyType);



            //x.Id
            //x => x.Id
            var xExpr = whatisthis;
            //x.Id == 253
            Expression body = Expression.Equal(member, xExpr);

            var final = Expression.Lambda<Func<TEntity, bool>>(body: body, parameters: xx);
            //var compiled_shit = final.Compile();

            return final;
        }

        public static List<Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>> IncludeEntities<TEntity>(params string[] propertyNames)
        {
            var includes = new List<Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>>();

            foreach (var propertyName in propertyNames)
            {
                //x =>
                var xx = Expression.Parameter(typeof(TEntity), "x");
                //x.Id
                var member = Expression.Property(xx, propertyName);
                Expression body = member;

                //x => x.Id
                includes.Add(Expression
                    .Lambda<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>(
                        body: body,
                        parameters: xx));
            }

            return includes;
        }

        public static Expression<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>> IncludeEntity<TEntity>(string propertyName)
        {
            //x =>
            var xx = Expression.Parameter(typeof(TEntity), "x");
            //x.Id
            var member = Expression.Property(xx, propertyName);
            Expression body = member;

            //x => x.Id
            return Expression
                .Lambda<Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>>(
                    body: body,
                    parameters: xx);
        }
    }
}
