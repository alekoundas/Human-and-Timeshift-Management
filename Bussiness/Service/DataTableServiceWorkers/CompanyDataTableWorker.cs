using Bussiness.Helpers;
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
    public class CompanyDataTableWorker : DataTableService
    {
        private BaseDatawork _baseDatawork { get; }
        private Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<Company> _filter { get; set; } = PredicateBuilder.New<Company>();


        public CompanyDataTableWorker(Datatable datatable, BaseDatawork baseDatawork, HttpContext httpContext)
            : base(datatable, baseDatawork, httpContext)
        {
            _datatable = datatable;
            _httpContext = httpContext;
            _baseDatawork = baseDatawork;

            //Filters for everyone
            _filter = _filter.And(GetSearchFilter());

            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<Company>(_httpContext);
            if (!canShowDeactivated)
                _filter = _filter.And(x => x.IsActive == true);
        }

        private Func<IQueryable<Company>, IOrderedQueryable<Company>> SetOrderBy()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<Company, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<Company>();
            if (_datatable.Search.Value != null)
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "Title")
                        filter = filter.Or(x => x.Title.Contains(_datatable.Search.Value));
                    if (column.Data == "Afm")
                        filter = filter.Or(x => x.Afm.Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }




        public async Task<CompanyDataTableWorker> CompanyIndex()
        {
            var includes = new List<Func<IQueryable<Company>, IIncludableQueryable<Company, object>>>();

            var entities = await _baseDatawork.Companies
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Company>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Company>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("Buttons", dataTableHelper
                    .GetButtons("Company", "Companies", result.Id.ToString()));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Companies.CountAllAsyncFiltered(_filter);

            return this;
        }

        //public CompanyDataTableWorker CompanyIndexDataMap()
        //{
        //    var expandoService = new ExpandoService();
        //    var dataTableHelper = new DataTableHelper<Company>();
        //    List<ExpandoObject> returnObjects = new List<ExpandoObject>();

        //    foreach (var result in _entities)
        //    {
        //        var expandoObj = expandoService.GetCopyFrom<Company>(result);
        //        var dictionary = (IDictionary<string, object>)expandoObj;

        //        dictionary.Add("Buttons", dataTableHelper.GetButtons("Company", "Companies", result.Id.ToString()));

        //        returnObjects.Add(expandoObj);
        //    }

        //    EntitiesMapped = returnObjects;
        //    return this;
        //}
    }
}
