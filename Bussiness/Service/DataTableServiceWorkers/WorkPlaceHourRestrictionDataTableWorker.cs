using Bussiness.Helpers;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Models.Entity;
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
    public class WorkPlaceHourRestrictionDataTableWorker : DataTableService
    {
        private BaseDatawork _baseDatawork { get; }
        private new Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<WorkPlaceHourRestriction> _filter { get; set; }
            = PredicateBuilder.New<WorkPlaceHourRestriction>();


        public WorkPlaceHourRestrictionDataTableWorker(Datatable datatable, BaseDatawork baseDatawork, HttpContext httpContext)
            : base(datatable, baseDatawork, httpContext)
        {
            _datatable = datatable;
            _httpContext = httpContext;
            _baseDatawork = baseDatawork;

            //Filters for everyone
            _filter = _filter.And(GetSearchFilter());
        }

        private Func<IQueryable<WorkPlaceHourRestriction>, IOrderedQueryable<WorkPlaceHourRestriction>> SetOrderBy()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<WorkPlaceHourRestriction, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<WorkPlaceHourRestriction>();
            if (_datatable.Search.Value != null)
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "Title")
                        filter = filter.Or(x => x.WorkPlace.Title.Contains(_datatable.Search.Value));
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


        public async Task<WorkPlaceHourRestrictionDataTableWorker> WorkPlaceHourRestrictionIndex()
        {
            var includes = new List<Func<IQueryable<WorkPlaceHourRestriction>, IIncludableQueryable<WorkPlaceHourRestriction, object>>>();
            includes.Add(x => x.Include(y => y.WorkPlace));

            var entities = await _baseDatawork.WorkPlaceHourRestrictions
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlaceHourRestriction>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<WorkPlaceHourRestriction>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("WorkPlaceName", result.WorkPlace.Title);
                dictionary.Add("Buttons", dataTableHelper
                    .GetButtons("WorkPlaceHourRestriction", "WorkPlaceHourRestrictions", result.Id.ToString()));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.WorkPlaceHourRestrictions.CountAllAsyncFiltered(_filter);

            return this;
        }
    }
}
