using Bussiness.Helpers;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Models.Security;
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
    public class LogTypeDataTableWorker : DataTableService
    {
        private Datatable _datatable { get; }
        private HttpContext _httpContext { get; }
        private readonly SecurityDataWork _securityDatawork;

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<LogType> _filter { get; set; } = PredicateBuilder.New<LogType>();


        public LogTypeDataTableWorker(
            Datatable datatable,
            BaseDatawork baseDatawork,
            HttpContext httpContext,
            SecurityDataWork securityDatawork)
            : base(datatable, baseDatawork, httpContext, securityDatawork)
        {
            _datatable = datatable;
            _httpContext = httpContext;
            _securityDatawork = securityDatawork;

            //Filters for everyone
            _filter = _filter.And(GetSearchFilter());

            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<LogType>(_httpContext);
            if (!canShowDeactivated)
                _filter = _filter.And(x => x.IsActive == true);
        }

        private Func<IQueryable<LogType>, IOrderedQueryable<LogType>> SetOrderBy()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<LogType, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<LogType>();
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


        public async Task<LogTypeDataTableWorker> LogTypeIndex()
        {
            var includes = new List<Func<IQueryable<LogType>, IIncludableQueryable<LogType, object>>>();

            var entities = await _securityDatawork.LogTypes
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<LogType>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<LogType>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("Buttons", dataTableHelper
                    .GetButtons("Contract", "Contracts", result.Id.ToString()));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _securityDatawork.LogTypes.CountAllAsyncFiltered(_filter);

            return this;
        }
    }
}
