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
    public class LeaveTypeDataTableWorker : DataTableService
    {
        private BaseDatawork _baseDatawork { get; }
        private new Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<LeaveType> _filter { get; set; } = PredicateBuilder.New<LeaveType>();


        public LeaveTypeDataTableWorker(Datatable datatable, BaseDatawork baseDatawork, HttpContext httpContext)
            : base(datatable, baseDatawork, httpContext)
        {
            _datatable = datatable;
            _httpContext = httpContext;
            _baseDatawork = baseDatawork;

            //Filters for everyone
            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<LeaveType>(_httpContext);

            if (!canShowDeactivated)
                _filter = _filter.And(x => x.IsActive == true);
            _filter = _filter.And(GetSearchFilter());
        }

        private Func<IQueryable<LeaveType>, IOrderedQueryable<LeaveType>> SetOrderBy()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<LeaveType, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<LeaveType>();
            if (_datatable.Search.Value != null)
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "Name")
                        filter = filter.Or(x => x.Name.Contains(_datatable.Search.Value));
                    if (column.Data == "Description")
                        filter = filter.Or(x => x.Description.Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }


        public async Task<LeaveTypeDataTableWorker> LeaveTypeIndex()
        {
            var includes = new List<Func<IQueryable<LeaveType>, IIncludableQueryable<LeaveType, object>>>();

            var entities = await _baseDatawork.LeaveTypes
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<LeaveType>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<LeaveType>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("Buttons", dataTableHelper
                    .GetButtons("LeaveType", "LeaveTypes", result.Id.ToString()));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.LeaveTypes.CountAllAsyncFiltered(_filter);

            return this;
        }
    }
}
