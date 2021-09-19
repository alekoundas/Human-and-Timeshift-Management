using Bussiness.Helpers;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Models.Security;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public class LogDataTableWorker : DataTableService
    {
        private readonly SecurityDataWork _securityDatawork;
        private Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<Log> _filter { get; set; } = PredicateBuilder.New<Log>();


        public LogDataTableWorker(
            Datatable datatable,
            BaseDatawork baseDatawork,
            HttpContext httpContext,
            SecurityDataWork securityDatawork)
            : base(datatable, baseDatawork, httpContext, securityDatawork)
        {
            _datatable = datatable;
            _httpContext = httpContext;
            _securityDatawork = securityDatawork;

            _filter = _filter.And(x => true);
            //Filters for everyone
            _filter = _filter.And(GetSearchFilter());
        }

        private Func<IQueryable<Log>, IOrderedQueryable<Log>> SetOrderBy()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<Log, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<Log>();
            if (_datatable.Search.Value != null)
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "CreatedOn")
                        filter = filter.Or(x => x.CreatedOn.ToString().Contains(_datatable.Search.Value));
                    else if (column.Data == "OriginalJSON")
                        filter = filter.Or(x => x.OriginalJSON.Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }


        public async Task<LogDataTableWorker> LogIndex()
        {
            var includes = new List<Func<IQueryable<Log>, IIncludableQueryable<Log, object>>>();
            includes.Add(x => x.Include(y => y.LogType));
            includes.Add(x => x.Include(y => y.LogEntity));

            if (_datatable.FilterByLogEntityId != 0)
                _filter = _filter.And(x => x.LogEntityId == _datatable.FilterByLogEntityId);

            if (_datatable.FilterByLogTypeId != 0)
                _filter = _filter.And(x => x.LogTypeId == _datatable.FilterByLogTypeId);


            _filter = _filter.And(x => _datatable.StartOn <= x.CreatedOn && x.CreatedOn <= _datatable.EndOn);

            var entities = await _securityDatawork.Logs
                .GetWithFilterQueryable(SetOrderBy(), _filter, includes, _pageSize, _pageIndex)
                .Select(x => new
                {
                    x.ApplicationUser,
                    x.CreatedBy_FullName,
                    CreatedOn = x.CreatedOn.ToString(),
                    LogTypeTitle = x.LogType.Title,
                    LogEntityTitle = x.LogEntity.Title,
                    x.OriginalJSON,
                    x.EditedJSON
                })
                .ToListAsync();

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Log>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                //dictionary.Add("Buttons", dataTableHelper
                //.GetButtons("Log", "Logs", result.Id.ToString()));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _securityDatawork.Logs.CountAllAsyncFiltered(_filter);

            return this;
        }
    }
}
