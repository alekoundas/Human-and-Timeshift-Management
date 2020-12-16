using Bussiness.Helpers;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Models.Entity;
using DataAccess.Service;
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
    public class ProjectionDataTableWorker : DataTableService
    {
        private BaseDatawork _baseDatawork { get; }
        private new Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<Employee> _filter { get; set; } = PredicateBuilder.New<Employee>();


        public ProjectionDataTableWorker(Datatable datatable, BaseDatawork baseDatawork, HttpContext httpContext)
            : base(datatable, baseDatawork, httpContext)
        {
            _datatable = datatable;
            _httpContext = httpContext;
            _baseDatawork = baseDatawork;

            //Filters for everyone
            _filter = _filter.And(GetSearchFilter());

            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<Employee>(_httpContext);
            if (!canShowDeactivated)
                _filter = _filter.And(x => x.IsActive == true);
        }

        private Func<IQueryable<Employee>, IOrderedQueryable<Employee>> SetOrderBy()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Func<IQueryable<WorkPlace>, IOrderedQueryable<WorkPlace>> SetOrderByWorkPlace()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<Employee, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<Employee>();
            if (_datatable.Search.Value != "")
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "VatNumber")
                        filter = filter.Or(x => x.VatNumber.Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }






        public async Task<ProjectionDataTableWorker> ProjectionConcentric()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.RealWorkHours).ThenInclude(z => z.TimeShift));

            _filter = _filter.And(x => x.RealWorkHours.Any(y =>
                _datatable.StartOn.Date <= y.StartOn.Date && y.EndOn.Date <= _datatable.EndOn.Date));

            if (_datatable.FilterByWorkPlaceId != 0)
                _filter = _filter.And(x => x.RealWorkHours.Any(y =>
                    y.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId));

            var entities = await _baseDatawork.Employees
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //break loop reference
            entities.ForEach(x => x.RealWorkHours.ToList().ForEach(x => x.TimeShift.RealWorkHours = null));

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Employee>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;
                var realWorkHoursFilter = PredicateBuilder.New<RealWorkHour>();

                realWorkHoursFilter = realWorkHoursFilter.And(x => x.EmployeeId == result.Id);

                if (_datatable.FilterByWorkPlaceId != 0)
                    realWorkHoursFilter = realWorkHoursFilter
                        .And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

                realWorkHoursFilter = realWorkHoursFilter.And(x =>
                    _datatable.StartOn.Date <= x.StartOn.Date && x.EndOn.Date <= _datatable.EndOn.Date);

                var totalSeconds = result.RealWorkHours
                    .Where(realWorkHoursFilter)
                    .Select(x => Math.Abs((x.EndOn - x.StartOn).TotalSeconds))
                    .Sum();
                var totalSecondsDay = result.RealWorkHours
                    .Where(realWorkHoursFilter)
                    .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToDayWork().TotalSeconds)
                    .Sum();
                var totalSecondsNight = result.RealWorkHours
                    .Where(realWorkHoursFilter)
                    .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToNightWork().TotalSeconds)
                    .Sum();
                var totalSecondsSaturdayDay = result.RealWorkHours
                    .Where(realWorkHoursFilter)
                    .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToSaturdayDayWork().TotalSeconds)
                    .Sum();
                var totalSecondsSaturdayNight = result.RealWorkHours
                    .Where(realWorkHoursFilter)
                    .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToSaturdayNightWork().TotalSeconds)
                    .Sum();
                var totalSecondsSundayDay = result.RealWorkHours
                    .Where(realWorkHoursFilter)
                    .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToSundayDayWork().TotalSeconds)
                    .Sum();
                var totalSecondsSundayNight = result.RealWorkHours
                    .Where(realWorkHoursFilter)
                    .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToSundayNightWork().TotalSeconds)
                    .Sum();

                if (_datatable.ShowHoursInPercentage)
                {
                    dictionary.Add("TotalHours", totalSeconds / 60 / 60);
                    dictionary.Add("TotalHoursDay", totalSecondsDay / 60 / 60);
                    dictionary.Add("TotalHoursNight", totalSecondsNight / 60 / 60);
                    dictionary.Add("TotalHoursSaturdayDay", totalSecondsSaturdayDay / 60 / 60);
                    dictionary.Add("TotalHoursSaturdayNight", totalSecondsSaturdayNight / 60 / 60);
                    dictionary.Add("TotalHoursSundayDay", totalSecondsSundayDay / 60 / 60);
                    dictionary.Add("TotalHoursSundayNight", totalSecondsSundayNight / 60 / 60);
                }
                else
                {
                    dictionary.Add("TotalHours", ((int)totalSeconds / 60 / 60).ToString() + ":" + ((int)totalSeconds / 60 % 60).ToString());
                    dictionary.Add("TotalHoursDay", ((int)totalSecondsDay / 60 / 60).ToString() + ":" + ((int)totalSecondsDay / 60 % 60).ToString());
                    dictionary.Add("TotalHoursNight", ((int)totalSecondsNight / 60 / 60).ToString() + ":" + ((int)totalSecondsNight / 60 % 60).ToString());
                    dictionary.Add("TotalHoursSaturdayDay", ((int)totalSecondsSaturdayDay / 60 / 60).ToString() + ":" + ((int)totalSecondsSaturdayDay / 60 % 60).ToString());
                    dictionary.Add("TotalHoursSaturdayNight", ((int)totalSecondsSaturdayNight / 60 / 60).ToString() + ":" + ((int)totalSecondsSaturdayNight / 60 % 60).ToString());
                    dictionary.Add("TotalHoursSundayDay", ((int)totalSecondsSundayDay / 60 / 60).ToString() + ":" + ((int)totalSecondsSundayDay / 60 % 60).ToString());
                    dictionary.Add("TotalHoursSundayNight", ((int)totalSecondsSundayNight / 60 / 60).ToString() + ":" + ((int)totalSecondsSundayNight / 60 % 60).ToString());
                }

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }


        public async Task<ProjectionDataTableWorker> ProjectionDifference()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            var entities = await _baseDatawork.Employees
                .ProjectionDifference(SetOrderBy(), _datatable, _filter, _pageSize, _pageIndex);

            entities.ForEach(x => x.RealWorkHours.ToList().ForEach(x => x.TimeShift.RealWorkHours = null));
            entities.ForEach(x => x.RealWorkHours.ToList().ForEach(x => x.TimeShift.WorkPlace.TimeShifts = null));

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Employee>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                //Βy RealWorkHour
                if (_datatable.FilterByRealWorkHour == true)

                    if (result.RealWorkHours.Count() > 0)
                        foreach (var realWorkHour in result.RealWorkHours)
                        {
                            expandoObj = expandoService.GetCopyFrom<Employee>(result);
                            dictionary = (IDictionary<string, object>)expandoObj;
                            dictionary.Add("WorkPlaceTitle", realWorkHour.TimeShift.WorkPlace.Title);
                            dictionary.Add("RealWorkHourDate",
                                dataTableHelper.GetProjectionDifferenceRealWorkHourLink(
                                    1,
                                    realWorkHour.StartOn + " - " + realWorkHour.EndOn));

                            returnObjects.Add(expandoObj);

                        }

                //Βy WorkHour
                if (_datatable.FilterByWorkHour == true)
                    if (result.WorkHours.Count() > 0)
                        foreach (var workHour in result.WorkHours)
                        {
                            expandoObj = expandoService.GetCopyFrom<Employee>(result);
                            dictionary = (IDictionary<string, object>)expandoObj;
                            dictionary.Add("WorkPlaceTitle", workHour.TimeShift.WorkPlace.Title);
                            dictionary.Add("WorkHourDate", dataTableHelper.
                                GetProjectionDifferenceWorkHourLink(
                                workHour.TimeShiftId,
                                workHour.StartOn + " - " + workHour.EndOn));

                            returnObjects.Add(expandoObj);
                        }

            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<ProjectionDataTableWorker> ProjectionEmployeeRealHoursSum()
        {
            var includes = new List<Func<IQueryable<WorkPlace>, IIncludableQueryable<WorkPlace, object>>>();
            includes.Add(x => x.Include(y => y.Customer).ThenInclude(z => z.Company));
            includes.Add(x => x.Include(y => y.TimeShifts).ThenInclude(z => z.RealWorkHours));

            var filter = PredicateBuilder.New<WorkPlace>();
            filter = filter.And(x => true);
            if (_datatable.GenericId != 0)
                filter = filter.And(x => x.EmployeeWorkPlaces.Any(y => y.EmployeeId == _datatable.GenericId));

            var entities = await _baseDatawork.WorkPlaces
                .GetPaggingWithFilter(SetOrderByWorkPlace(), filter, includes, _pageSize, _pageIndex);

            //entities.ForEach(x => x.TimeShifts.ToList().ForEach(y => y.RealWorkHours.ToList().ForEach(z => z.TimeShift.RealWorkHours = null)));
            //entities.ForEach(x => x.TimeShifts.ToList().ForEach(y => y.WorkPlace = null));

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlace>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<WorkPlace>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;
                var realWorkHoursFilter = PredicateBuilder.New<RealWorkHour>();

                if (_datatable.GenericId != 0)
                    realWorkHoursFilter = realWorkHoursFilter.And(x => x.EmployeeId == _datatable.GenericId);

                realWorkHoursFilter = realWorkHoursFilter.And(x =>
                    _datatable.StartOn.Date <= x.StartOn.Date && x.EndOn.Date <= _datatable.EndOn.Date);

                var totalSeconds = result.TimeShifts
                    .Where(x => x.WorkPlaceId == result.Id)
                    .Select(x => x.RealWorkHours
                        .Where(realWorkHoursFilter)
                        .Select(x => Math.Abs((x.EndOn - x.StartOn).TotalSeconds))
                        .Sum())
                    .Sum();
                var totalSecondsDay = result.TimeShifts
                     .Where(x => x.WorkPlaceId == result.Id)
                     .Select(x => x.RealWorkHours
                        .Where(realWorkHoursFilter)
                        .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToDayWork().TotalSeconds)
                        .Sum())
                    .Sum();
                var totalSecondsNight = result.TimeShifts
                     .Where(x => x.WorkPlaceId == result.Id)
                     .Select(x => x.RealWorkHours
                        .Where(realWorkHoursFilter)
                        .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToNightWork().TotalSeconds)
                        .Sum())
                    .Sum();

                if (_datatable.ShowHoursInPercentage)
                {
                    dictionary.Add("TotalHours", totalSeconds / 60 / 60);
                    dictionary.Add("TotalHoursDay", totalSecondsDay / 60 / 60);
                    dictionary.Add("TotalHoursNight", totalSecondsNight / 60 / 60);
                }
                else
                {
                    dictionary.Add("TotalHours", ((int)totalSeconds / 60 / 60).ToString() + ":" + ((int)totalSeconds / 60 % 60).ToString());
                    dictionary.Add("TotalHoursDay", ((int)totalSecondsDay / 60 / 60).ToString() + ":" + ((int)totalSecondsDay / 60 % 60).ToString());
                    dictionary.Add("TotalHoursNight", ((int)totalSecondsNight / 60 / 60).ToString() + ":" + ((int)totalSecondsNight / 60 % 60).ToString());
                }

                returnObjects.Add(expandoObj);
            }

            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.WorkPlaces.CountAllAsyncFiltered(filter);

            return this;
        }

        public async Task<ProjectionDataTableWorker> ProjectionPresenceDaily()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.RealWorkHours));

            //Get employees that have a realworkhour today
            _filter = _filter.And(x => x.RealWorkHours.Any(y => y.StartOn.Date == DateTime.Now.Date));

            if (_datatable.FilterByWorkPlaceId != 0)
                _filter = _filter.And(x => x.EmployeeWorkPlaces
                    .Any(y => y.WorkPlaceId == _datatable.FilterByWorkPlaceId));


            var entities = await _baseDatawork.Employees
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);


            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Employee>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                //remove unessecery RealWorkHours
                result.RealWorkHours = result.RealWorkHours
                    .Where(x => x.StartOn.Date == DateTime.Now.Date)
                    .ToList();

                var todayCell = "";
                foreach (var realWorkHour in result.RealWorkHours)
                    todayCell += "<p style='white-space:nowrap;'>" +
                        realWorkHour.StartOn.ToShortTimeString() +
                        " - " +
                        realWorkHour.EndOn.ToShortTimeString() +
                        "</p></br>";
                dictionary.Add("Today", todayCell);

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<ProjectionDataTableWorker> ProjectionRealWorkHoursAnalytically()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.RealWorkHours));

            if (_datatable.FilterByWorkPlaceId != 0)
                _filter = _filter.And(x => x.RealWorkHours
                    .Any(y => y.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId));

            var entities = await _baseDatawork.Employees
               .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Employee>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                if ((_datatable.EndOn.Date - _datatable.StartOn.Date).TotalDays == 0.0)
                {
                    var realWorkHoursToday = result.RealWorkHours
                                    .Where(x => x.StartOn.Date == _datatable.StartOn.Date)
                                    .ToList();
                    dictionary.Add("Day_0", dataTableHelper
                           .GetProjectionRealWorkHoursAnalyticallyCellBody(realWorkHoursToday));
                }
                else
                    for (int i = 0; i <= (_datatable.EndOn.Date - _datatable.StartOn.Date).TotalDays; i++)
                    {
                        var compareDate = new DateTime(_datatable.StartOn.AddDays(i).Ticks);
                        var realWorkHoursToday = result.RealWorkHours
                            .Where(x => x.StartOn.Date == compareDate)
                            .ToList();

                        dictionary.Add("Day_" + i, dataTableHelper
                            .GetProjectionRealWorkHoursAnalyticallyCellBody(realWorkHoursToday));
                    }
                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<ProjectionDataTableWorker> ProjectionRealWorkHoursAnalyticallySum()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.RealWorkHours).ThenInclude(y => y.TimeShift));

            if (_datatable.FilterByWorkPlaceId != 0)
                _filter = _filter.And(x => x.RealWorkHours
                    .Any(y => y.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId));

            var entities = await _baseDatawork.Employees
               .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            entities.ForEach(x => x.RealWorkHours.ToList().ForEach(y => y.Employee = null));
            entities.ForEach(x => x.RealWorkHours.ToList().ForEach(y => y.TimeShift.RealWorkHours = null));

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Employee>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;
                var realWorkHoursFilter = PredicateBuilder.New<RealWorkHour>();
                realWorkHoursFilter = realWorkHoursFilter.And(x => x.EmployeeId == result.Id);

                if (_datatable.FilterByWorkPlaceId != 0)
                    realWorkHoursFilter = realWorkHoursFilter
                        .And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);


                if ((_datatable.EndOn.Date - _datatable.StartOn.Date).TotalDays == 0.0)
                {
                    realWorkHoursFilter = realWorkHoursFilter.And(x =>
                        _datatable.StartOn.Date <= x.StartOn.Date && x.EndOn.Date <= _datatable.EndOn.Date);

                    var daySeconds = result.RealWorkHours
                        .Where(realWorkHoursFilter)
                        .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToDayWork().TotalSeconds)
                        .Sum();
                    var nightSeconds = result.RealWorkHours
                        .Where(realWorkHoursFilter)
                        .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToNightWork().TotalSeconds)
                        .Sum();

                    var dayTotalSeconds = daySeconds + nightSeconds;

                    if (_datatable.ShowHoursInPercentage)
                        dictionary.Add("Day_0", dayTotalSeconds / 60 / 60);
                    else
                        dictionary.Add("Day_0", ((int)dayTotalSeconds / 60 / 60).ToString() + ":" + ((int)dayTotalSeconds / 60 % 60).ToString());
                }
                else
                    for (int i = 0; i <= (_datatable.EndOn.Date - _datatable.StartOn.Date).TotalDays; i++)
                    {
                        var compareDate = new DateTime(_datatable.StartOn.AddDays(i).Ticks);
                        realWorkHoursFilter = realWorkHoursFilter.And(x =>
                            compareDate.Date <= x.StartOn.Date && x.EndOn.Date <= compareDate.Date);

                        var daySeconds = result.RealWorkHours
                            .Where(realWorkHoursFilter)
                            .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToDayWork().TotalSeconds)
                            .Sum();
                        var nightSeconds = result.RealWorkHours
                            .Where(realWorkHoursFilter)
                            .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToNightWork().TotalSeconds)
                            .Sum();
                        var dayTotalSeconds = daySeconds + nightSeconds;

                        if (_datatable.ShowHoursInPercentage)
                            dictionary.Add("Day_" + i, dayTotalSeconds / 60 / 60);
                        else
                            dictionary.Add("Day_" + i, ((int)dayTotalSeconds / 60 / 60).ToString() + ":" + ((int)dayTotalSeconds / 60 % 60).ToString());
                    }

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<ProjectionDataTableWorker> ProjectionRealWorkHoursSpecificDates()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.WorkHours));
            includes.Add(x => x.Include(y => y.RealWorkHours));
            includes.Add(x => x.Include(y => y.Leaves));

            if (_datatable.FilterByWorkPlaceId != 0)
                _filter = _filter.And(x => x.RealWorkHours
                    .Any(y => y.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId));

            var entities = await _baseDatawork.Employees
               .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            entities.ForEach(x => x.RealWorkHours.ToList().ForEach(y => y.Employee = null));
            entities.ForEach(x => x.WorkHours.ToList().ForEach(y => y.Employee = null));
            entities.ForEach(x => x.Leaves.ToList().ForEach(y => y.Employee = null));

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Employee>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                for (int i = 0; i < _datatable.SpecificDates?.Count(); i++)
                {
                    var realWorkHours = result.RealWorkHours
                        .Where(x => x.StartOn.Date == _datatable.SpecificDates[i].Date)
                        .ToList();

                    var hasLeave = result.Leaves
                        .Any(x =>
                            _datatable.SpecificDates[i].Date >= x.StartOn.Date &&
                            _datatable.SpecificDates[i].Date <= x.EndOn.Date);


                    var dayOffs = result.WorkHours
                        .Where(x => x.StartOn.Date == _datatable.SpecificDates[i].Date)
                        .ToList();

                    var dayCell = "";
                    foreach (var realWorkHour in realWorkHours)
                        dayCell += "<p style='white-space:nowrap;'>" +
                            realWorkHour.StartOn.ToShortTimeString() +
                            " - " +
                            realWorkHour.EndOn.ToShortTimeString() +
                            "</p></br>";
                    if (dayOffs.Count > 0)
                    {
                        dayCell += "<p style='white-space:nowrap;'>" +
                            "Ρεπό" +
                            "</p></br>";
                    }
                    if (hasLeave)
                    {
                        dayCell += "<p style='white-space:nowrap;'>" +
                            "Άδεια" +
                            "</p></br>";
                    }
                    dictionary.Add("Day_" + i, dayCell);

                }

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<ProjectionDataTableWorker> ProjectionTimeShiftSuggestions()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.RealWorkHours));

            if (_datatable.FilterByWorkPlaceId != 0)
                _filter = _filter.And(x => x.RealWorkHours
                    .Any(y => y.TimeShiftId == _datatable.FilterByTimeShift));

            var entities = await _baseDatawork.Employees
               .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            entities.ForEach(x => x.RealWorkHours.ToList().ForEach(y => y.Employee = null));

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Employee>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("Day_0", _datatable.StartOn);

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

    }
}
