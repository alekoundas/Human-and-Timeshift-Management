using DataAccess.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace WebApplication.Utilities
{
    public class SelectLambdaBuilder<TEntity>
    {
        private ParameterExpression _xParameter = null;
        private readonly Type _typeOfBaseClass = typeof(TEntity);

        private static Dictionary<Type, PropertyInfo[]> _typePropertyInfoMappings =
            new Dictionary<Type, PropertyInfo[]>();

        private List<MemberAssignment> _selectedProperies =
            new List<MemberAssignment>();

        public SelectLambdaBuilder<TEntity> CreateSelect()
        {
            this._xParameter = Expression.Parameter(_typeOfBaseClass, "x");
            return this;
        }

        public SelectLambdaBuilder<TEntity> WithProperty(string field)
        {
            Expression xParam = Expression.PropertyOrField(this._xParameter, field);
            PropertyInfo mi = _typeOfBaseClass.GetProperty(((MemberExpression)xParam).Member.Name);
            this._selectedProperies.Add(Expression.Bind(mi, xParam));

            return this;
        }

        public SelectLambdaBuilder<TEntity> WithProperty2<TProperty>(Expression<Func<TEntity, TProperty>> selector)
        {
            //if(selector.Body.)

            var sssssss = selector.Body;
            var dotEntries = selector.ToString().Split('.');
            var aaa = dotEntries.ElementAt(1).GetType();
            var bbb = dotEntries.ElementAt(2).GetType();
            //Expression xParam = Expression.PropertyOrField(this._xParameter, field);
            //PropertyInfo mi = _typeOfBaseClass.GetProperty(((MemberExpression)xParam).Member.Name);
            //this._selectedProperies.Add(Expression.Bind(mi, xParam));

            return this;
        }

        public SelectLambdaBuilder<TEntity> WithClass<TSelectClass, TProperty>(string field)
        {




            //PropertyInfo[] propertyInfos;
            //if (!_typePropertyInfoMappings.TryGetValue(_typeOfBaseClass, out propertyInfos))
            //{
            //    var properties = _typeOfBaseClass.GetProperties();
            //    propertyInfos = properties;
            //    _typePropertyInfoMappings.Add(_typeOfBaseClass, properties);
            //}



            //var propertyType = propertyInfos
            //      .FirstOrDefault(p => p.Name.Equals(className))
            //      .PropertyType;

            Type classType = typeof(Employee);

            PropertyInfo objClassPropInfo = _typeOfBaseClass.GetProperty("WorkHours");
            MemberExpression objNestedMemberExpression = Expression.Property(this._xParameter, objClassPropInfo);
            NewExpression innerObjNew = Expression.New(typeof(TSelectClass));
            PropertyInfo nestedObjPropInfo = classType.GetProperty("Id");
            MemberExpression nestedOrigin2 = Expression.Property(objNestedMemberExpression, nestedObjPropInfo);
            var binding2 = Expression.Bind(nestedObjPropInfo, nestedOrigin2);


            MemberInitExpression nestedInit = Expression.MemberInit(innerObjNew, binding2);
            this._selectedProperies.Add(Expression.Bind(objClassPropInfo, nestedInit));




            return this;
        }


        public Func<TEntity, TEntity> CompleteSelect()
        {
            NewExpression xNew = Expression.New(_typeOfBaseClass);
            var xInit = Expression.MemberInit(xNew, this._selectedProperies);
            var lambda = Expression.Lambda<Func<TEntity, TEntity>>(xInit, this._xParameter);

            return lambda.Compile();
        }


        private Dictionary<string, List<string>> GetFieldMapping(string fields)
        {
            if (string.IsNullOrEmpty(fields))
                return null;

            var selectedFieldsMap = new Dictionary<string, List<string>>();

            foreach (var s in fields.Split(','))
            {
                var nestedFields = s.Split('.').Select(f => f.Trim()).ToArray();
                var nestedValue = nestedFields.Length > 1 ? nestedFields[1] : null;

                if (selectedFieldsMap.Keys.Any(key => key == nestedFields[0]))
                    selectedFieldsMap[nestedFields[0]].Add(nestedValue);
                else
                    selectedFieldsMap.Add(nestedFields[0], new List<string> { nestedValue });
            }

            return selectedFieldsMap;
        }

        public Func<TEntity, TEntity> CreateNewStatement(string fields)
        {
            var selectFields = GetFieldMapping(fields);
            if (selectFields == null)
                return x => x;

            ParameterExpression xParameter = Expression.Parameter(_typeOfBaseClass, "x");
            NewExpression xNew = Expression.New(_typeOfBaseClass);

            var shpNestedPropertyBindings = new List<MemberAssignment>();
            foreach (var keyValuePair in selectFields)
            {
                PropertyInfo[] propertyInfos;
                if (!_typePropertyInfoMappings.TryGetValue(_typeOfBaseClass, out propertyInfos))
                {
                    var properties = _typeOfBaseClass.GetProperties();
                    propertyInfos = properties;
                    _typePropertyInfoMappings.Add(_typeOfBaseClass, properties);
                }

                var propertyType = propertyInfos
                    .FirstOrDefault(p => p.Name.ToLowerInvariant().Equals(keyValuePair.Key.ToLowerInvariant()))
                    .PropertyType;

                if (propertyType.IsClass)
                {
                    PropertyInfo objClassPropInfo = _typeOfBaseClass.GetProperty(keyValuePair.Key);
                    MemberExpression objNestedMemberExpression = Expression.Property(xParameter, objClassPropInfo);

                    NewExpression innerObjNew = Expression.New(propertyType);

                    var nestedBindings = keyValuePair.Value.Select(v =>
                    {
                        PropertyInfo nestedObjPropInfo = propertyType.GetProperty(v);

                        MemberExpression nestedOrigin2 = Expression.Property(objNestedMemberExpression, nestedObjPropInfo);
                        var binding2 = Expression.Bind(nestedObjPropInfo, nestedOrigin2);

                        return binding2;
                    });

                    MemberInitExpression nestedInit = Expression.MemberInit(innerObjNew, nestedBindings);
                    shpNestedPropertyBindings.Add(Expression.Bind(objClassPropInfo, nestedInit));
                }
                //else if (propertyType.IsAbstract)
                //{

                //}
                else
                {
                    Expression mbr = xParameter;
                    mbr = Expression.PropertyOrField(mbr, keyValuePair.Key);

                    PropertyInfo mi = _typeOfBaseClass.GetProperty(((MemberExpression)mbr).Member.Name);

                    var xOriginal = Expression.Property(xParameter, mi);

                    shpNestedPropertyBindings.Add(Expression.Bind(mi, xOriginal));
                }
            }

            var xInit = Expression.MemberInit(xNew, shpNestedPropertyBindings);
            var lambda = Expression.Lambda<Func<TEntity, TEntity>>(xInit, xParameter);

            return lambda.Compile();
        }
    }
}
