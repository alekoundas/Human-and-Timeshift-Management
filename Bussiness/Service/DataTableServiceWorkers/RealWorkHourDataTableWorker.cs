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
    public class RealWorkHourDataTableWorker : DataTableService
    {
        private BaseDatawork _baseDatawork { get; }
        private new Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<RealWorkHour> _filter { get; set; } = PredicateBuilder.New<RealWorkHour>();


        public RealWorkHourDataTableWorker(Datatable datatable, BaseDatawork baseDatawork, HttpContext httpContext)
            : base(datatable, baseDatawork, httpContext)
        {
            _datatable = datatable;
            _httpContext = httpContext;
            _baseDatawork = baseDatawork;

            //Filters for everyone
            _filter = _filter.And(GetSearchFilter());

        }

        private Func<IQueryable<RealWorkHour>, IOrderedQueryable<RealWorkHour>> SetOrderBy()
        {
            if (_columnName == "EmployeeFirstName")
                return x => x.OrderBy("Employee.FirstName" + " " + _orderDirection.ToUpper());
            if (_columnName == "EmployeeLastName")
                return x => x.OrderBy("Employee.LastName" + " " + _orderDirection.ToUpper());
            if (_columnName == "EmployeeVatNumber")
                return x => x.OrderBy("Employee.VatNumber" + " " + _orderDirection.ToUpper());
            if (_columnName == "WorkPlaceTitle")
                return x => x.OrderBy("TimeShift.WorkPlace.Title" + " " + _orderDirection.ToUpper());

            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<RealWorkHour, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<RealWorkHour>();
            if (_datatable.Search.Value != null)
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "EmployeeFirstName")
                        filter = filter.Or(x => x.Employee.FirstName.Contains(_datatable.Search.Value));
                    if (column.Data == "EmployeeVatNumber")
                        filter = filter.Or(x => x.Employee.VatNumber.Contains(_datatable.Search.Value));
                    if (column.Data == "EmployeeLastName")
                        filter = filter.Or(x => x.Employee.LastName.Contains(_datatable.Search.Value));
                    if (column.Data == "WorkPlaceTitle")
                        filter = filter.Or(x => x.TimeShift.WorkPlace.Title.Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }


        public async Task<RealWorkHourDataTableWorker> ProjectionRealWorkHourRestrictions()
        {
            var entities = new List<RealWorkHour>();
            var includes = new List<Func<IQueryable<RealWorkHour>, IIncludableQueryable<RealWorkHour, object>>>();
            includes.Add(x => x.Include(y => y.Employee));
            includes.Add(x => x.Include(y => y.TimeShift).ThenInclude(y => y.WorkPlace));

            if (_datatable.FilterByMonth != 0 && _datatable.FilterByYear != 0 && _datatable.FilterByWorkPlace != 0)
            {
                var hourRestrictions = await _baseDatawork.HourRestrictions
                    .Where(x =>
                        x.WorkPlaceHourRestriction.Month == _datatable.FilterByMonth &&
                        x.WorkPlaceHourRestriction.Year == _datatable.FilterByYear &&
                        x.WorkPlaceHourRestriction.WorkPlaceId == _datatable.FilterByWorkPlace)
                    .ToDynamicListAsync<HourRestriction>();

                foreach (var hourRestriction in hourRestrictions)
                {

                    _filter = _filter.And(x => x.StartOn.Year == _datatable.FilterByYear);
                    _filter = _filter.And(x => x.StartOn.Month == _datatable.FilterByMonth);
                    _filter = _filter.And(x => x.StartOn.Day == hourRestriction.Day);

                    var zzzz = await _baseDatawork.RealWorkHours
                        .Where(_filter)
                        .ToDynamicListAsync();
                    //.GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);
                }
            }

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Company>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<RealWorkHour>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("EmployeeFirstName", result.Employee.FirstName);
                dictionary.Add("EmployeeLastName", result.Employee.LastName);
                dictionary.Add("EmployeeVatNumber", result.Employee.VatNumber);
                dictionary.Add("WorkPlaceTitle", result.TimeShift.WorkPlace.Title);
                dictionary.Add("RealWorkHour_Start", result.StartOn);
                dictionary.Add("RealWorkHour_End", result.EndOn);

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.RealWorkHours.CountAllAsyncFiltered(_filter);

            return this;
        }

    }
}
