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
    public class LogEntityDataTableWorker : DataTableService
    {
        private readonly SecurityDataWork _securityDatawork;
        private Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<LogEntity> _filter { get; set; } = PredicateBuilder.New<LogEntity>();


        public LogEntityDataTableWorker(
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

            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<LogEntity>(_httpContext);
            if (!canShowDeactivated)
                _filter = _filter.And(x => x.IsActive == true);
        }

        private Func<IQueryable<LogEntity>, IOrderedQueryable<LogEntity>> SetOrderBy()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<LogEntity, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<LogEntity>();
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


        public async Task<LogEntityDataTableWorker> LogEntityIndex()
        {
            var includes = new List<Func<IQueryable<LogEntity>, IIncludableQueryable<LogEntity, object>>>();

            var entities = await _securityDatawork.LogEntities
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<LogEntity>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<LogEntity>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("Buttons", dataTableHelper
                    .GetButtons("Contract", "Contracts", result.Id.ToString()));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _securityDatawork.LogEntities.CountAllAsyncFiltered(_filter);

            return this;
        }
    }
}
