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
            if (_columnName == "StartOn_string")
                return x => x.OrderBy("StartOn" + " " + _orderDirection.ToUpper());
            if (_columnName == "EndOn_string")
                return x => x.OrderBy("EndOn" + " " + _orderDirection.ToUpper());

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
                    //if (column.Data == "StartOn_string")
                    //    filter = filter.Or(x => x.StartOn.ToString("dd-mm-yyyy").Contains(_datatable.Search.Value));
                    //if (column.Data == "EndOn_string")
                    //    filter = filter.Or(x => x.EndOn.ToString("dd-mm-yyyy").Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }

        public async Task<RealWorkHourDataTableWorker> RealWorkHourCurrentDay()
        {
            var includes = new List<Func<IQueryable<RealWorkHour>, IIncludableQueryable<RealWorkHour, object>>>();
            includes.Add(x => x.Include(y => y.Employee));
            includes.Add(x => x.Include(y => y.TimeShift).ThenInclude(y => y.WorkPlace));


            //Get todays realWorkHours
            _filter = _filter.And(x => x.StartOn.Date == DateTime.Now.Date);

            var entities = await _baseDatawork.RealWorkHours
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);


            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<RealWorkHour>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<RealWorkHour>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("ToggleSlider", dataTableHelper
                    .GetEmployeeCheckbox(_datatable, result.Id));

                dictionary.Add("EmployeeFirstName", result.Employee.FirstName);
                dictionary.Add("EmployeeLastName", result.Employee.LastName);
                dictionary.Add("WorkPlaceTitle", result.TimeShift.WorkPlace.Title);
                dictionary.Add("StartOn_string", result.StartOn.ToString("dd-mm-yyyy hh:mm"));
                dictionary.Add("EndOn_string", result.EndOn.ToString("dd-mm-yyyy hh:mm"));

                dictionary.Add("Buttons", dataTableHelper
                    .GetCurrentDayButtons(result));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.RealWorkHours.CountAllAsyncFiltered(_filter);

            return this;
        }

        //public async Task<RealWorkHourDataTableWorker> ProjectionRealWorkHourRestrictions()
        //{
        //    var entities = new List<RealWorkHour>();
        //    var includes = new List<Func<IQueryable<RealWorkHour>, IIncludableQueryable<RealWorkHour, object>>>();
        //    includes.Add(x => x.Include(y => y.Employee));
        //    includes.Add(x => x.Include(y => y.TimeShift).ThenInclude(y => y.WorkPlace));

        //    if (_datatable.FilterByMonth != 0)
        //    {
        //        _filter = _filter.And(x => x.StartOn.Year == _datatable.FilterByYear);
        //        _filter = _filter.And(x => x.StartOn.Month == _datatable.FilterByMonth);
        //    }
        //    else
        //    {
        //        _filter = _filter.And(x => x.StartOn.Year == DateTime.Now.Year);
        //        _filter = _filter.And(x => x.StartOn.Month == DateTime.Now.Month);
        //    }
        //    if (_datatable.FilterByWorkPlaceId != 0)
        //        _filter = _filter.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

        //    var realWorkHours = await _baseDatawork.RealWorkHours.GetAllAsync(null, _filter, includes);

        //    realWorkHours.ForEach(x => x.TimeShift.RealWorkHours = null);
        //    realWorkHours.ForEach(x => x.Employee.RealWorkHours = null);

        //    var hourRestrictionFilter = PredicateBuilder.New<HourRestriction>();
        //    hourRestrictionFilter = hourRestrictionFilter.And(x => x.WorkPlaceHourRestriction.Month == _datatable.FilterByMonth);
        //    hourRestrictionFilter = hourRestrictionFilter.And(x => x.WorkPlaceHourRestriction.Year == _datatable.FilterByYear);
        //    if (_datatable.FilterByWorkPlaceId != 0)
        //        hourRestrictionFilter = hourRestrictionFilter.And(x => x.WorkPlaceHourRestriction.WorkPlaceId == _datatable.FilterByWorkPlaceId);

        //    var hourRestrictions = new List<HourRestriction>();
        //    hourRestrictions = await _baseDatawork.HourRestrictions
        //        .Where(hourRestrictionFilter)
        //        .ToDynamicListAsync<HourRestriction>();

        //    foreach (var hourRestriction in hourRestrictions)
        //    {
        //        var totalSecondsToday = realWorkHours
        //            .Where(x => x.StartOn.Day == hourRestriction.Day)
        //            .Select(x => Math.Abs((x.StartOn - x.EndOn).TotalSeconds))
        //            .Sum();

        //        var remainingSeconds = 0.0;

        //        var todayFilter = PredicateBuilder.New<RealWorkHour>();
        //        todayFilter = todayFilter.And(x => x.StartOn.Day == hourRestriction.Day);

        //        if (hourRestriction.MaxTicks != 0)
        //            if (totalSecondsToday >= hourRestriction.MaxTicks)
        //            {
        //                remainingSeconds = totalSecondsToday - hourRestriction.MaxTicks;
        //                do
        //                {
        //                    //create filters to remove appended id from query

        //                    var realWorkHour = realWorkHours
        //                        .Where(todayFilter)
        //                        .OrderByDescending(x => x.CreatedOn)
        //                        .FirstOrDefault();

        //                    remainingSeconds = remainingSeconds - Math.Abs((realWorkHour.StartOn - realWorkHour.EndOn).TotalSeconds);
        //                    entities.Add(realWorkHour);
        //                    todayFilter = todayFilter.And(y => y.Id != realWorkHour.Id);

        //                } while (remainingSeconds > 0);
        //            }
        //    }

        //    //Mapping
        //    var expandoService = new ExpandoService();
        //    var dataTableHelper = new DataTableHelper<Company>();
        //    var returnObjects = new List<ExpandoObject>();

        //    foreach (var result in entities)
        //    {
        //        var expandoObj = expandoService.GetCopyFrom<RealWorkHour>(result);
        //        var dictionary = (IDictionary<string, object>)expandoObj;

        //        var totalSecondsToday = realWorkHours
        //            .Where(x => x.StartOn.Day == result.StartOn.Day)
        //            .Select(x => Math.Abs((x.StartOn - x.EndOn).TotalSeconds))
        //            .Sum();

        //        dictionary.Add("EmployeeFirstName", result.Employee.FirstName);
        //        dictionary.Add("EmployeeLastName", result.Employee.LastName);
        //        dictionary.Add("EmployeeVatNumber", result.Employee.VatNumber);
        //        dictionary.Add("WorkPlaceTitle", result.TimeShift.WorkPlace.Title);
        //        dictionary.Add("RealWorkHour_Start", result.StartOn);
        //        dictionary.Add("RealWorkHour_End", result.EndOn);
        //        dictionary.Add("WorkHourRestriction_Max", GetTime(hourRestrictions.First(x => x.Day == result.StartOn.Day).MaxTicks));
        //        dictionary.Add("WorkHourRestriction_Sum", GetTime(totalSecondsToday));

        //        returnObjects.Add(expandoObj);
        //    }
        //    EntitiesMapped = returnObjects;
        //    EntitiesTotal = await _baseDatawork.RealWorkHours.CountAllAsyncFiltered(_filter);

        //    return this;
        //}

        private static string GetTime(double seconds)
        {
            var hours = (seconds / 3600).ToString();
            seconds %= 3600;
            var minutes = (seconds / 60).ToString();

            if (hours.Contains(","))
                hours = hours.Split(',')[0];

            if (hours.Length == 1)
                hours = "0" + hours;

            if (minutes.Length == 1)
                minutes = "0" + minutes;

            return hours + ":" + minutes;
        }

    }
}
