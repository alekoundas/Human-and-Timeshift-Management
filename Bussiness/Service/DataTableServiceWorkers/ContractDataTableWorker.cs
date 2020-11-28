using Bussiness.Helpers;
using DataAccess;
using DataAccess.Libraries.Datatable;
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
    public class ContractDataTableWorker : DataTableService
    {
        private BaseDatawork _baseDatawork { get; }
        private new Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<Contract> _filter { get; set; } = PredicateBuilder.New<Contract>();


        public ContractDataTableWorker(Datatable datatable, BaseDatawork baseDatawork, HttpContext httpContext)
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

        private Func<IQueryable<Contract>, IOrderedQueryable<Contract>> SetOrderBy()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<Contract, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<Contract>();
            if (_datatable.Search.Value != null)
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "Title")
                        filter = filter.Or(x => x.Title.Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }


        public async Task<ContractDataTableWorker> ContractIndex()
        {
            var includes = new List<Func<IQueryable<Contract>, IIncludableQueryable<Contract, object>>>();

            var entities = await _baseDatawork.Contracts
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Contract>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Contract>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("Buttons", dataTableHelper
                    .GetButtons("Contract", "Contracts", result.Id.ToString()));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Contracts.CountAllAsyncFiltered(_filter);

            return this;
        }
    }
}
