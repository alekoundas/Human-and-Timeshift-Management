using Bussiness.Helpers;
using DataAccess;
using DataAccess.Models.Datatable;
using DataAccess.Models.Entity;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bussiness.Service.DataTableServiceWorkers
{
    public class CustomerDataTableWorker : DataTableService
    {
        private BaseDatawork _baseDatawork { get; }
        private new Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<Customer> _filter { get; set; } = PredicateBuilder.New<Customer>();


        public CustomerDataTableWorker(Datatable datatable, BaseDatawork baseDatawork, HttpContext httpContext)
            : base(datatable, baseDatawork, httpContext)
        {
            _datatable = datatable;
            _httpContext = httpContext;
            _baseDatawork = baseDatawork;

            //Filters for everyone
            _filter = _filter.And(GetSearchFilter());

            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<Customer>(_httpContext);
            if (!canShowDeactivated)
                _filter = _filter.And(x => x.IsActive == true);
        }

        private Func<IQueryable<Customer>, IOrderedQueryable<Customer>> SetOrderBy()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<Customer, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<Customer>();
            if (_datatable.Search.Value != null)
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "ΙdentifyingΝame")
                        filter = filter.Or(x => x.ΙdentifyingΝame.Contains(_datatable.Search.Value));
                    if (column.Data == "Profession")
                        filter = filter.Or(x => x.Profession.Contains(_datatable.Search.Value));
                    if (column.Data == "Address")
                        filter = filter.Or(x => x.Address.Contains(_datatable.Search.Value));
                    if (column.Data == "PostalCode")
                        filter = filter.Or(x => x.PostalCode.Contains(_datatable.Search.Value));
                    if (column.Data == "DOY")
                        filter = filter.Or(x => x.DOY.Contains(_datatable.Search.Value));
                    if (column.Data == "AFM")
                        filter = filter.Or(x => x.AFM.Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }


        public async Task<CustomerDataTableWorker> CustomerIndex()
        {
            var includes = new List<Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>>>();

            var entities = await _baseDatawork.Customers
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Customer>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Customer>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("Buttons", dataTableHelper
                    .GetButtons("Customer", "Customers", result.Id.ToString()));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Customers.CountAllAsyncFiltered(_filter);

            return this;
        }
    }
}
