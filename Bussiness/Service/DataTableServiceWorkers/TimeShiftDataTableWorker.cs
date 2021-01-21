
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
    public class TimeShiftDataTableWorker : DataTableService
    {
        private BaseDatawork _baseDatawork { get; }
        private new Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<TimeShift> _filter { get; set; } = PredicateBuilder.New<TimeShift>();


        public TimeShiftDataTableWorker(Datatable datatable, BaseDatawork baseDatawork, HttpContext httpContext)
            : base(datatable, baseDatawork, httpContext)
        {
            _datatable = datatable;
            _httpContext = httpContext;
            _baseDatawork = baseDatawork;

            //Filters for everyone
            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<TimeShift>(_httpContext);

            if (!canShowDeactivated)
                _filter = _filter.And(x => x.IsActive == true);
            _filter = _filter.And(GetSearchFilter());
        }

        private Func<IQueryable<TimeShift>, IOrderedQueryable<TimeShift>> SetOrderBy()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<TimeShift, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<TimeShift>();
            if (_datatable.Search.Value != null)
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "Title")
                        filter = filter.Or(x => x.Title.Contains(_datatable.Search.Value));
                    if (column.Data == "Month")
                        filter = filter.Or(x => x.Month.ToString().Contains(_datatable.Search.Value));
                    if (column.Data == "Year")
                        filter = filter.Or(x => x.Year.ToString().Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }


        public async Task<TimeShiftDataTableWorker> TimeShiftIndex()
        {
            var includes = new List<Func<IQueryable<TimeShift>, IIncludableQueryable<TimeShift, object>>>();
            _filter = _filter.And(x => x.WorkPlaceId == _datatable.GenericId);
            var entities = await _baseDatawork.TimeShifts
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<TimeShift>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("Title", result.Title);
                dictionary.Add("Month", result.Month);
                dictionary.Add("Year", result.Year);
                dictionary.Add("Buttons", dataTableHelper
                    .GetButtons("TimeShift", "TimeShifts", result.Id.ToString()));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.TimeShifts.CountAllAsyncFiltered(_filter);

            return this;
        }
    }
}
