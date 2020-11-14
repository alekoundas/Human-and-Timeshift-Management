using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Bussiness.Service
{
    public class ExpandoService
    {
        public ExpandoObject GetCopyFrom<TEntity>(TEntity model)
        {
            var expando = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)expando;

            foreach (var property in model.GetType().GetProperties())
                dictionary.Add(property.Name, property.GetValue(model));

            return expando;
        }
    }
}
