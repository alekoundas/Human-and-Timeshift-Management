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
using System.Globalization;
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


        private Expression<Func<RealWorkHour, bool>> GetUserRoleFiltersAsync()
        {
            //Get WorkPlaceId from user roles
            var workPlaceIds = _httpContext.User.Claims
                .Where(x => x.Value.Contains("Specific_WorkPlace"))
                .Select(y => Int32.Parse(y.Value.Split("_")[2]));

            var filter = PredicateBuilder.New<RealWorkHour>();
            foreach (var workPlaceId in workPlaceIds)
                filter = filter.Or(x => x.TimeShift.WorkPlaceId == workPlaceId);

            if (workPlaceIds.Count() == 0)
                filter = filter.And(x => true);

            return filter;
        }


        public async Task<ProjectionDataTableWorker> ProjectionCurrentDay()
        {
            var includes = new List<Func<IQueryable<RealWorkHour>, IIncludableQueryable<RealWorkHour, object>>>();
            includes.Add(x => x.Include(y => y.Employee));
            includes.Add(x => x.Include(y => y.TimeShift).ThenInclude(y => y.WorkPlace));

            var filter = PredicateBuilder.New<RealWorkHour>();

            //Get todays realWorkHours
            filter = filter.And(x => x.StartOn.Date == DateTime.Now.Date);

            //Filter workplaces form user roles(if any)
            filter = filter.And(GetUserRoleFiltersAsync());

            var entities = await _baseDatawork.RealWorkHours
                .GetPaggingWithFilter(x => x.OrderBy(y => y.StartOn), filter, includes, _pageSize, _pageIndex);


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
                dictionary.Add("StartOn_string", result.StartOn.ToString());
                dictionary.Add("EndOn_string", result.EndOn.ToString());

                dictionary.Add("Buttons", "");
                //dictionary.Add("Buttons", dataTableHelper
                //   .GetCurrentDayButtons(result));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.RealWorkHours.CountAllAsyncFiltered(filter);

            return this;
        }


        public async Task<ProjectionDataTableWorker> ProjectionConcentric()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.RealWorkHours).ThenInclude(z => z.TimeShift));

            _filter = _filter.And(x => x.RealWorkHours.Any(y =>
            _datatable.StartOn <= y.StartOn && y.StartOn <= _datatable.EndOn));

            if (_datatable.FilterByWorkPlaceId != 0)
                _filter = _filter.And(x => x.RealWorkHours.Any(y =>
                    y.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId));

            var entities = await _baseDatawork.Employees
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);


            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();
            var filterRealWorkHours = PredicateBuilder.New<RealWorkHour>();

            filterRealWorkHours = filterRealWorkHours.And(x => _datatable.StartOn <= x.StartOn && x.StartOn <= _datatable.EndOn);

            if (_datatable.FilterByWorkPlaceId != 0)
                filterRealWorkHours = filterRealWorkHours.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

            var totalFooter = entities
                .Select(x => x.RealWorkHours
                    .Where(filterRealWorkHours)
                    .Select(x => Math.Abs((x.EndOn - x.StartOn).TotalSeconds))
                .Sum())
            .Sum();

            var totalFooterDay = entities
                .Select(x => x.RealWorkHours
                    .Where(filterRealWorkHours)
                    .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToDayWork().TotalSeconds)
                .Sum())
            .Sum();

            var totalFooterNight = entities
                .Select(x => x.RealWorkHours
                    .Where(filterRealWorkHours)
                    .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToNightWork().TotalSeconds)
                .Sum())
            .Sum();

            var totalFooterSaturdayDay = entities
                .Select(x => x.RealWorkHours
                    .Where(filterRealWorkHours)
                    .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToSaturdayDayWork().TotalSeconds)
                .Sum())
            .Sum();

            var totalFooterSaturdayNight = entities
                .Select(x => x.RealWorkHours
                    .Where(filterRealWorkHours)
                    .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToSaturdayNightWork().TotalSeconds)
                .Sum())
            .Sum();

            var totalFooterSundayDay = entities
                .Select(x => x.RealWorkHours
                    .Where(filterRealWorkHours)
                    .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToSundayDayWork().TotalSeconds)
                .Sum())
            .Sum();

            var totalFooterSundayNight = entities
                .Select(x => x.RealWorkHours
                    .Where(filterRealWorkHours)
                    .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToSundayNightWork().TotalSeconds)
                .Sum())
            .Sum();


            foreach (var result in entities)
            {

                var resultRealWorkHours = result.RealWorkHours;
                result.RealWorkHours = null;
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;
                filterRealWorkHours = PredicateBuilder.New<RealWorkHour>();

                dictionary.Add("FirstName", result.FirstName);
                dictionary.Add("LastName", result.LastName);
                dictionary.Add("ErpCode", result.ErpCode);
                filterRealWorkHours = filterRealWorkHours.And(x => x.EmployeeId == result.Id);

                if (_datatable.FilterByWorkPlaceId != 0)
                    filterRealWorkHours = filterRealWorkHours
                        .And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

                filterRealWorkHours = filterRealWorkHours.And(x =>
                               _datatable.StartOn <= x.StartOn && x.StartOn <= _datatable.EndOn);



                var totalSeconds = resultRealWorkHours
                    .Where(filterRealWorkHours)
                    .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToTotalWork().TotalSeconds)
                    .Sum();
                var totalSecondsDay = resultRealWorkHours
                    .Where(filterRealWorkHours)
                    .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToDayWork().TotalSeconds)
                    .Sum();
                var totalSecondsNight = resultRealWorkHours
                    .Where(filterRealWorkHours)
                    .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToNightWork().TotalSeconds)
                    .Sum();
                var totalSecondsSaturdayDay = resultRealWorkHours
                    .Where(filterRealWorkHours)
                    .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToSaturdayDayWork().TotalSeconds)
                    .Sum();
                var totalSecondsSaturdayNight = resultRealWorkHours
                    .Where(filterRealWorkHours)
                    .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToSaturdayNightWork().TotalSeconds)
                    .Sum();
                var totalSecondsSundayDay = resultRealWorkHours
                    .Where(filterRealWorkHours)
                    .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToSundayDayWork().TotalSeconds)
                    .Sum();
                var totalSecondsSundayNight = resultRealWorkHours
                    .Where(filterRealWorkHours)
                    .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToSundayNightWork().TotalSeconds)
                    .Sum();

                if (_datatable.ShowHoursInPercentage)
                {
                    dictionary.Add("TotalHours", totalSeconds / 60 / 480);
                    dictionary.Add("TotalHoursDay", totalSecondsDay / 60 / 480);
                    dictionary.Add("TotalHoursNight", totalSecondsNight / 60 / 480);
                    dictionary.Add("TotalHoursSaturdayDay", totalSecondsSaturdayDay / 60 / 480);
                    dictionary.Add("TotalHoursSaturdayNight", totalSecondsSaturdayNight / 60 / 480);
                    dictionary.Add("TotalHoursSundayDay", totalSecondsSundayDay / 60 / 480);
                    dictionary.Add("TotalHoursSundayNight", totalSecondsSundayNight / 60 / 480);

                    dictionary.Add("TotalFooterHours", totalFooter / 60 / 480);
                    dictionary.Add("TotalFooterHoursDay", totalFooterDay / 60 / 480);
                    dictionary.Add("TotalFooterHoursNight", totalFooterNight / 60 / 480);
                    dictionary.Add("TotalFooterSaturdayDay", totalFooterSaturdayDay / 60 / 480);
                    dictionary.Add("TotalFooterSaturdayNight", totalFooterSaturdayNight / 60 / 480);
                    dictionary.Add("TotalFooterSundayDay", totalFooterSundayDay / 60 / 480);
                    dictionary.Add("TotalFooterSundayNight", totalFooterSundayNight / 60 / 480);
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

                    dictionary.Add("TotalFooterHours", ((int)totalFooter / 60 / 60).ToString() + ":" + ((int)totalFooter / 60 % 60).ToString());
                    dictionary.Add("TotalFooterHoursDay", ((int)totalFooterDay / 60 / 60).ToString() + ":" + ((int)totalFooterDay / 60 % 60).ToString());
                    dictionary.Add("TotalFooterHoursNight", ((int)totalFooterNight / 60 / 60).ToString() + ":" + ((int)totalFooterNight / 60 % 60).ToString());
                    dictionary.Add("TotalFooterSaturdayDay", ((int)totalFooterSaturdayDay / 60 / 60).ToString() + ":" + ((int)totalFooterSaturdayDay / 60 % 60).ToString());
                    dictionary.Add("TotalFooterSaturdayNight", ((int)totalFooterSaturdayNight / 60 / 60).ToString() + ":" + ((int)totalFooterSaturdayNight / 60 % 60).ToString());
                    dictionary.Add("TotalFooterSundayDay", ((int)totalFooterSundayDay / 60 / 60).ToString() + ":" + ((int)totalFooterSundayDay / 60 % 60).ToString());
                    dictionary.Add("TotalFooterSundayNight", ((int)totalFooterSundayNight / 60 / 60).ToString() + ":" + ((int)totalFooterSundayNight / 60 % 60).ToString());
                }

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }


        public async Task<ProjectionDataTableWorker> ProjectionConcentricSpecificDates()
        {
            var filter = PredicateBuilder.New<RealWorkHour>();
            var includes = new List<Func<IQueryable<RealWorkHour>, IIncludableQueryable<RealWorkHour, object>>>();
            includes.Add(x => x.Include(y => y.Employee));

            foreach (var specificDate in _datatable.SpecificDates)
                filter = filter.Or(x => x.StartOn.Date == specificDate);

            filter = filter.And(filter);

            if (_datatable.FilterByWorkPlaceId != 0)
                filter = filter.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

            var entitiesasd = (await _baseDatawork.RealWorkHours
.GetPaggingWithFilter(x => x.OrderBy(y => y.StartOn), filter, includes, _pageSize, _pageIndex))
.GroupBy(x => new
{
    x.StartOn.Date,
    x.Employee
}).ToList();
            var entities = (await _baseDatawork.RealWorkHours
            .GetPaggingWithFilter(x => x.OrderBy(y => y.StartOn), filter, includes, _pageSize, _pageIndex))
            .GroupBy(x => new
            {
                x.StartOn.Date,
                x.Employee
            })
            .Select(x => new
            {
                x.Key.Date,
                x.Key.Employee,
                TotalSeconds = x.ToList()
                                .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToTotalWork().TotalSeconds)
                                .Sum(),
                TotalDaySeconds = x.ToList()
                                .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToDayWork().TotalSeconds)
                                .Sum(),
                TotalNightSeconds = x.ToList()
                                .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToNightWork().TotalSeconds)
                                .Sum(),
                TotalSaturdayDaySeconds = x.ToList()
                                .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToSaturdayDayWork().TotalSeconds)
                                .Sum(),
                TotalSaturdayNightSeconds = x.ToList()
                                .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToSaturdayNightWork().TotalSeconds)
                                .Sum(),
                TotalSundayDaySeconds = x.ToList()
                                .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToSundayDayWork().TotalSeconds)
                                .Sum(),
                TotalSundayNightSeconds = x.ToList()
                                .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToSundayNightWork().TotalSeconds)
                                .Sum(),
            })
            .ToList();


            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            var totalFooter = 0.0;
            var totalFooterDay = 0.0;
            var totalFooterNight = 0.0;
            var totalFooterSaturdayDay = 0.0;
            var totalFooterSaturdayNight = 0.0;
            var totalFooterSundayDay = 0.0;
            var totalFooterSundayNight = 0.0;
            foreach (var specificDate in _datatable.SpecificDates)
            {
                totalFooter = entities.Where(x => x.Date == specificDate)
                    .Select(x => x.TotalSeconds)
                    .Sum();

                totalFooterDay = entities.Where(x => x.Date == specificDate)
                    .Select(x => x.TotalSeconds)
                    .Sum();

                totalFooterNight = entities.Where(x => x.Date == specificDate)
                    .Select(x => x.TotalSeconds)
                    .Sum();

                totalFooterSaturdayDay = entities.Where(x => x.Date == specificDate)
                    .Select(x => x.TotalSeconds)
                    .Sum();

                totalFooterSaturdayNight = entities.Where(x => x.Date == specificDate)
                    .Select(x => x.TotalSeconds)
                    .Sum();

                totalFooterSundayDay = entities.Where(x => x.Date == specificDate)
                    .Select(x => x.TotalSeconds)
                    .Sum();

                totalFooterSundayNight = entities.Where(x => x.Date == specificDate)
                    .Select(x => x.TotalSeconds)
                    .Sum();
            }

            foreach (var result in entities)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("FirstName", result.Employee.FirstName);
                dictionary.Add("LastName", result.Employee.LastName);
                dictionary.Add("ErpCode", result.Employee.ErpCode);
                dictionary.Add("VatNumber", result.Employee.VatNumber);

                var countDay = 0;
                foreach (var specificDate in _datatable.SpecificDates)
                {
                    var totalSeconds = entities
                        .Where(x => x.Date == specificDate)
                        .Where(x => x.Employee.Id == result.Employee.Id)
                        .Select(x => x.TotalSeconds)
                        .Sum();

                    var totalSecondsDay = entities
                        .Where(x => x.Date == specificDate)
                        .Where(x => x.Employee.Id == result.Employee.Id)
                        .Select(x => x.TotalDaySeconds)
                        .Sum();

                    var totalSecondsNight = entities
                        .Where(x => x.Date == specificDate)
                        .Where(x => x.Employee.Id == result.Employee.Id)
                        .Select(x => x.TotalNightSeconds)
                        .Sum();

                    var totalSecondsSaturdayDay = entities
                        .Where(x => x.Date == specificDate)
                        .Where(x => x.Employee.Id == result.Employee.Id)
                        .Select(x => x.TotalSaturdayDaySeconds)
                        .Sum();

                    var totalSecondsSaturdayNight = entities
                        .Where(x => x.Date == specificDate)
                        .Where(x => x.Employee.Id == result.Employee.Id)
                        .Select(x => x.TotalSaturdayNightSeconds)
                        .Sum();

                    var totalSecondsSundayDay = entities
                        .Where(x => x.Date == specificDate)
                        .Where(x => x.Employee.Id == result.Employee.Id)
                        .Select(x => x.TotalSundayDaySeconds)
                        .Sum();

                    var totalSecondsSundayNight = entities
                        .Where(x => x.Date == specificDate)
                        .Where(x => x.Employee.Id == result.Employee.Id)
                        .Select(x => x.TotalSundayNightSeconds)
                        .Sum();

                    if (_datatable.ShowHoursInPercentage)
                    {
                        dictionary.Add("TotalHours_" + countDay, totalSeconds / 60 / 480);
                        dictionary.Add("TotalHoursDay_" + countDay, totalSecondsDay / 60 / 480);
                        dictionary.Add("TotalHoursNight_" + countDay, totalSecondsNight / 60 / 480);
                        dictionary.Add("TotalHoursSaturdayDay_" + countDay, totalSecondsSaturdayDay / 60 / 480);
                        dictionary.Add("TotalHoursSaturdayNight_" + countDay, totalSecondsSaturdayNight / 60 / 480);
                        dictionary.Add("TotalHoursSundayDay_" + countDay, totalSecondsSundayDay / 60 / 480);
                        dictionary.Add("TotalHoursSundayNight_" + countDay, totalSecondsSundayNight / 60 / 480);

                        //dictionary.Add("TotalFooterHours", totalFooter / 60 / 480);
                        //dictionary.Add("TotalFooterHoursDay", totalFooterDay / 60 / 480);
                        //dictionary.Add("TotalFooterHoursNight", totalFooterNight / 60 / 480);
                        //dictionary.Add("TotalFooterSaturdayDay", totalFooterSaturdayDay / 60 / 480);
                        //dictionary.Add("TotalFooterSaturdayNight", totalFooterSaturdayNight / 60 / 480);
                        //dictionary.Add("TotalFooterSundayDay", totalFooterSundayDay / 60 / 480);
                        //dictionary.Add("TotalFooterSundayNight", totalFooterSundayNight / 60 / 480);
                    }
                    else
                    {
                        dictionary.Add("TotalHours_" + countDay, ((int)totalSeconds / 60 / 60).ToString() + ":" + ((int)totalSeconds / 60 % 60).ToString());
                        dictionary.Add("TotalHoursDay_" + countDay, ((int)totalSecondsDay / 60 / 60).ToString() + ":" + ((int)totalSecondsDay / 60 % 60).ToString());
                        dictionary.Add("TotalHoursNight_" + countDay, ((int)totalSecondsNight / 60 / 60).ToString() + ":" + ((int)totalSecondsNight / 60 % 60).ToString());
                        dictionary.Add("TotalHoursSaturdayDay_" + countDay, ((int)totalSecondsSaturdayDay / 60 / 60).ToString() + ":" + ((int)totalSecondsSaturdayDay / 60 % 60).ToString());
                        dictionary.Add("TotalHoursSaturdayNight_" + countDay, ((int)totalSecondsSaturdayNight / 60 / 60).ToString() + ":" + ((int)totalSecondsSaturdayNight / 60 % 60).ToString());
                        dictionary.Add("TotalHoursSundayDay_" + countDay, ((int)totalSecondsSundayDay / 60 / 60).ToString() + ":" + ((int)totalSecondsSundayDay / 60 % 60).ToString());
                        dictionary.Add("TotalHoursSundayNight_" + countDay, ((int)totalSecondsSundayNight / 60 / 60).ToString() + ":" + ((int)totalSecondsSundayNight / 60 % 60).ToString());

                        //dictionary.Add("TotalFooterHours", ((int)totalFooter / 60 / 60).ToString() + ":" + ((int)totalFooter / 60 % 60).ToString());
                        //dictionary.Add("TotalFooterHoursDay", ((int)totalFooterDay / 60 / 60).ToString() + ":" + ((int)totalFooterDay / 60 % 60).ToString());
                        //dictionary.Add("TotalFooterHoursNight", ((int)totalFooterNight / 60 / 60).ToString() + ":" + ((int)totalFooterNight / 60 % 60).ToString());
                        //dictionary.Add("TotalFooterSaturdayDay", ((int)totalFooterSaturdayDay / 60 / 60).ToString() + ":" + ((int)totalFooterSaturdayDay / 60 % 60).ToString());
                        //dictionary.Add("TotalFooterSaturdayNight", ((int)totalFooterSaturdayNight / 60 / 60).ToString() + ":" + ((int)totalFooterSaturdayNight / 60 % 60).ToString());
                        //dictionary.Add("TotalFooterSundayDay", ((int)totalFooterSundayDay / 60 / 60).ToString() + ":" + ((int)totalFooterSundayDay / 60 % 60).ToString());
                        //dictionary.Add("TotalFooterSundayNight", ((int)totalFooterSundayNight / 60 / 60).ToString() + ":" + ((int)totalFooterSundayNight / 60 % 60).ToString());
                    }
                    countDay++;
                }
                returnObjects.Add(expandoObj);
            }

            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }


        public async Task<ProjectionDataTableWorker> ProjectionDifference()
        {
            var DbF = EF.Functions;

            EntitiesMapped = new List<ExpandoObject>();

            var includesWorkHour = new List<Func<IQueryable<WorkHour>, IIncludableQueryable<WorkHour, object>>>();
            includesWorkHour.Add(x => x.Include(y => y.Employee));
            includesWorkHour.Add(x => x.Include(y => y.TimeShift.WorkPlace));

            var filterWorkHour = PredicateBuilder.New<WorkHour>();
            //filterWorkHour = filterWorkHour.And(x => x.EndOn >= _datatable.StartOn && x.StartOn <= _datatable.EndOn);
            filterWorkHour = filterWorkHour.And(x => (_datatable.StartOn <= x.StartOn && x.EndOn <= _datatable.EndOn) || (_datatable.StartOn <= x.StartOn && _datatable.EndOn <= x.EndOn));
            filterWorkHour = filterWorkHour.And(x => !x.Employee.RealWorkHours.Any(y => x.StartOn <= y.StartOn && y.EndOn <= x.EndOn));

            if (_datatable.FilterByWorkPlaceId != 0)
                filterWorkHour = filterWorkHour.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

            var entitiesWorkHour = await _baseDatawork.WorkHours
                .GetPaggingWithFilter(x => x.OrderBy(y => y.StartOn), filterWorkHour, includesWorkHour, _pageSize, _pageIndex);

            var totalFooterWorkHours = entitiesWorkHour
                .Select(x => Math.Abs((x.EndOn - x.StartOn).TotalSeconds))
                .Sum();



            var includesRealWorkHour = new List<Func<IQueryable<RealWorkHour>, IIncludableQueryable<RealWorkHour, object>>>();
            includesRealWorkHour.Add(x => x.Include(y => y.Employee));
            includesRealWorkHour.Add(x => x.Include(y => y.TimeShift.WorkPlace));

            var filterRealWorkHour = PredicateBuilder.New<RealWorkHour>();
            filterRealWorkHour = filterRealWorkHour.And(x => (_datatable.StartOn <= x.StartOn && x.EndOn <= _datatable.EndOn) || (_datatable.StartOn <= x.StartOn && _datatable.EndOn <= x.EndOn));
            filterRealWorkHour = filterRealWorkHour.And(x => !x.Employee.WorkHours.Any(y => y.StartOn <= x.StartOn && x.EndOn <= y.EndOn));

            if (_datatable.FilterByWorkPlaceId != 0)
                filterRealWorkHour = filterRealWorkHour.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

            var entitiesRealWorkHour = await _baseDatawork.RealWorkHours
                .GetPaggingWithFilter(x => x.OrderBy(y => y.StartOn), filterRealWorkHour, includesRealWorkHour, _pageSize, _pageIndex);

            var totalFooterRealWorkHours = entitiesRealWorkHour
                    .Select(x => Math.Abs((x.EndOn - x.StartOn).TotalSeconds))
                    .Sum();

            var realWorkHours = entitiesRealWorkHour
                .GroupBy(x => x.EmployeeId)
                .Select(x => new { RealWorkHours = x.ToList(), x.Key })
                .ToList();

            var workHours = entitiesWorkHour
                .GroupBy(x => x.EmployeeId)
                .Select(x => new { WorkHours = x.ToList(), x.Key })
                .ToList();


            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkHour>();
            var returnObjects = new List<ExpandoObject>();
            foreach (var group in realWorkHours)
            {
                foreach (var result in group.RealWorkHours)
                {
                    if (workHours.Any(x => x.Key == group.Key))
                    {
                        foreach (var resultWorkHour in workHours.First(x => x.Key == group.Key).WorkHours)
                        {
                            if (_datatable.FilterByWorkHour)
                            {
                                var expandoObj = new ExpandoObject();
                                var dictionary = (IDictionary<string, object>)expandoObj;

                                dictionary.Add("FirstName", resultWorkHour.Employee.FirstName);
                                dictionary.Add("LastName", resultWorkHour.Employee.LastName);
                                dictionary.Add("VatNumber", resultWorkHour.Employee.VatNumber);
                                dictionary.Add("WorkPlaceTitle", resultWorkHour.TimeShift.WorkPlace.Title);
                                dictionary.Add("WorkHourDate", resultWorkHour.StartOn + " - " + resultWorkHour.EndOn);
                                dictionary.Add("RealWorkHourDate", "");

                                dictionary.Add("TotalFooterWorkHours", ((int)totalFooterWorkHours / 60 / 60).ToString() + ":" + ((int)totalFooterWorkHours / 60 % 60).ToString());

                                returnObjects.Add(expandoObj);
                            }
                        }
                        workHours.Remove(workHours.First(x => x.Key == group.Key));
                    }
                    if (_datatable.FilterByRealWorkHour)
                    {
                        var expandoObj = new ExpandoObject();
                        var dictionary = (IDictionary<string, object>)expandoObj;

                        dictionary.Add("FirstName", result.Employee.FirstName);
                        dictionary.Add("LastName", result.Employee.LastName);
                        dictionary.Add("VatNumber", result.Employee.VatNumber);
                        dictionary.Add("WorkPlaceTitle", result.TimeShift.WorkPlace.Title);
                        dictionary.Add("RealWorkHourDate", result.StartOn + " - " + result.EndOn);
                        dictionary.Add("WorkHourDate", "");

                        dictionary.Add("TotalFooterWorkHours", ((int)totalFooterRealWorkHours / 60 / 60).ToString() + ":" + ((int)totalFooterRealWorkHours / 60 % 60).ToString());

                        returnObjects.Add(expandoObj);
                    }

                }
            }

            //Print workhours if left any
            foreach (var resultWorkHour in workHours)
            {
                foreach (var result in resultWorkHour.WorkHours)
                {

                    if (_datatable.FilterByWorkHour)
                    {
                        var expandoObj = new ExpandoObject();
                        var dictionary = (IDictionary<string, object>)expandoObj;

                        dictionary.Add("FirstName", result.Employee.FirstName);
                        dictionary.Add("LastName", result.Employee.LastName);
                        dictionary.Add("VatNumber", result.Employee.VatNumber);
                        dictionary.Add("WorkPlaceTitle", result.TimeShift.WorkPlace.Title);
                        dictionary.Add("WorkHourDate", result.StartOn + " - " + result.EndOn);
                        dictionary.Add("RealWorkHourDate", "");

                        dictionary.Add("TotalFooterWorkHours", ((int)totalFooterWorkHours / 60 / 60).ToString() + ":" + ((int)totalFooterWorkHours / 60 % 60).ToString());

                        returnObjects.Add(expandoObj);
                    }
                }
            }
            returnObjects.ForEach(x => EntitiesMapped.Add(x));
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

            if (_datatable.FilterByWorkPlaceId != 0)
                filter = filter.And(x => x.Id == _datatable.FilterByWorkPlaceId);

            var entities = await _baseDatawork.WorkPlaces
                .GetPaggingWithFilter(SetOrderByWorkPlace(), filter, includes, _pageSize, _pageIndex);


            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlace>();
            var returnObjects = new List<ExpandoObject>();

            var realWorkHoursFilter = PredicateBuilder.New<RealWorkHour>();

            if (_datatable.FilterByEmployeeId != 0)
                realWorkHoursFilter = realWorkHoursFilter.And(x => x.EmployeeId == _datatable.FilterByEmployeeId);

            realWorkHoursFilter = realWorkHoursFilter.And(x =>
                _datatable.StartOn.Date <= x.StartOn.Date && x.EndOn.Date <= _datatable.EndOn.Date);


            var totalFooter = entities.Select(x => x.TimeShifts
                .Select(x => x.RealWorkHours
                    .Where(realWorkHoursFilter)
                    .Select(x => Math.Abs((x.EndOn - x.StartOn).TotalSeconds))
                    .Sum())
                .Sum())
                .Sum();

            var totalFooterDay = entities.Select(x => x.TimeShifts
              .Select(x => x.RealWorkHours
                  .Where(realWorkHoursFilter)
                  .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToDayWork().TotalSeconds)
                    .Sum())
                  .Sum())
                .Sum();

            var totalFooterNight = entities.Select(x => x.TimeShifts
              .Select(x => x.RealWorkHours
                  .Where(realWorkHoursFilter)
                  .Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToNightWork().TotalSeconds)
                    .Sum())
                  .Sum())
               .Sum();

            foreach (var result in entities)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("Title", result.Title);
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
                    dictionary.Add("TotalHours", totalSeconds / 60 / 480);
                    dictionary.Add("TotalHoursDay", totalSecondsDay / 60 / 480);
                    dictionary.Add("TotalHoursNight", totalSecondsNight / 60 / 480);

                    dictionary.Add("TotalFooterHours", totalFooter / 60 / 480);
                    dictionary.Add("TotalFooterHoursDay", totalFooterDay / 60 / 480);
                    dictionary.Add("TotalFooterHoursNight", totalFooterNight / 60 / 480);
                }
                else
                {
                    dictionary.Add("TotalHours", ((int)totalSeconds / 60 / 60).ToString() + ":" + ((int)totalSeconds / 60 % 60).ToString());
                    dictionary.Add("TotalHoursDay", ((int)totalSecondsDay / 60 / 60).ToString() + ":" + ((int)totalSecondsDay / 60 % 60).ToString());
                    dictionary.Add("TotalHoursNight", ((int)totalSecondsNight / 60 / 60).ToString() + ":" + ((int)totalSecondsNight / 60 % 60).ToString());

                    dictionary.Add("TotalFooterHours", ((int)totalFooter / 60 / 60).ToString() + ":" + ((int)totalFooter / 60 % 60).ToString());
                    dictionary.Add("TotalFooterHoursDay", ((int)totalFooterDay / 60 / 60).ToString() + ":" + ((int)totalFooterDay / 60 % 60).ToString());
                    dictionary.Add("TotalFooterHoursNight", ((int)totalFooterNight / 60 / 60).ToString() + ":" + ((int)totalFooterNight / 60 % 60).ToString());
                }

                if (totalSeconds != 0 || totalSecondsDay != 0 || totalSecondsNight != 0)
                    returnObjects.Add(expandoObj);
            }

            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.WorkPlaces.CountAllAsyncFiltered(filter);

            return this;
        }

        public async Task<ProjectionDataTableWorker> ProjectionPresenceDaily()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.RealWorkHours).ThenInclude(y => y.TimeShift).ThenInclude(y => y.WorkPlace));

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

            var totalFooterSeconds = entities.Select(x => x.RealWorkHours
                    .Where(x => x.StartOn.Date == DateTime.Now.Date)
                    .Select(x => Math.Abs((x.EndOn - x.StartOn).TotalSeconds))
                    .Sum())
                .Sum();
            foreach (var result in entities)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                var todayCell = "";
                var workPlaceName = "";
                var realWorkHours = result.RealWorkHours
                    .Where(x => x.StartOn.Date == DateTime.Now.Date)
                    .ToList();

                foreach (var realWorkHour in realWorkHours)
                {
                    workPlaceName = realWorkHour.TimeShift.WorkPlace.Title;
                    todayCell += "<p style='white-space:nowrap;'>" +
                        realWorkHour.StartOn.ToShortTimeString() +
                        " - " +
                        realWorkHour.EndOn.ToShortTimeString() +
                        "</p></br>";
                }
                dictionary.Add("FirstName", result.FirstName);
                dictionary.Add("LastName", result.LastName);
                dictionary.Add("ErpCode", result.ErpCode);
                dictionary.Add("VatNumber", result.VatNumber);
                dictionary.Add("WorkPlace_RealWorkHour", workPlaceName);
                dictionary.Add("Today", todayCell);
                dictionary.Add("Today_FooterTotal", ((int)totalFooterSeconds / 60 / 60).ToString() + ":" + ((int)totalFooterSeconds / 60 % 60).ToString());


                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<ProjectionDataTableWorker> ProjectionRealWorkHoursAnalytically()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.RealWorkHours).ThenInclude(y => y.TimeShift));

            if (_datatable.FilterByWorkPlaceId != 0)
                _filter = _filter.And(x => x.RealWorkHours
                    .Any(y => y.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId));

            var entities = await _baseDatawork.Employees
               .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            var totalRealWorkHours = entities.SelectMany(x => x.RealWorkHours);

            foreach (var result in entities)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("FirstName", result.FirstName);
                dictionary.Add("LastName", result.LastName);
                dictionary.Add("ErpCode", result.ErpCode);

                for (int i = 0; i <= (_datatable.EndOn.Date - _datatable.StartOn.Date).TotalDays; i++)
                {
                    var compareDate = new DateTime(_datatable.StartOn.AddDays(i).Ticks);
                    var filterRealWorkHours = PredicateBuilder.New<RealWorkHour>();

                    filterRealWorkHours = filterRealWorkHours.And(x => x.StartOn.Date == compareDate);
                    if (_datatable.FilterByWorkPlaceId != 0)
                        filterRealWorkHours = filterRealWorkHours
                            .And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

                    var totalFooterSeconds = totalRealWorkHours
                            .Where(filterRealWorkHours)
                            .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToTotalWork().TotalSeconds)
                        .Sum();

                    var realWorkHoursToday = result.RealWorkHours
                        .Where(filterRealWorkHours)
                        .ToList();

                    dictionary.Add("Day_" + i, dataTableHelper
                        .GetProjectionRealWorkHoursAnalyticallyCellBody(realWorkHoursToday));

                    dictionary.Add("Day_" + i + "_FooterTotal", ((int)totalFooterSeconds / 60 / 60).ToString() + ":" + ((int)totalFooterSeconds / 60 % 60).ToString());

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

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();
            var totalRealWorkHours = entities.SelectMany(x => x.RealWorkHours);

            foreach (var result in entities)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("FirstName", result.FirstName);
                dictionary.Add("LastName", result.LastName);
                dictionary.Add("ErpCode", result.ErpCode);

                var totalDays = Math.Round((_datatable.EndOn - _datatable.StartOn).TotalDays);

                for (int i = 0; i <= totalDays; i++)
                {
                    var compareDate = new DateTime(_datatable.StartOn.AddDays(i).Ticks);
                    var realWorkHoursFilter = PredicateBuilder.New<RealWorkHour>();

                    if (_datatable.FilterByWorkPlaceId != 0)
                        realWorkHoursFilter = realWorkHoursFilter
                            .And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

                    realWorkHoursFilter = realWorkHoursFilter.And(x => x.StartOn.Date == compareDate);

                    var totalFooterSeconds = totalRealWorkHours
                            .Where(realWorkHoursFilter)
                            .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToTotalWork().TotalSeconds)
                        .Sum();

                    var totalSeconds = result.RealWorkHours
                        .Where(realWorkHoursFilter)
                        .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToTotalWork().TotalSeconds)
                        .Sum();


                    if (_datatable.ShowHoursInPercentage)
                    {
                        dictionary.Add("Day_" + i, totalSeconds / 60 / 480);
                        dictionary.Add("Day_" + i + "_FooterTotal", totalFooterSeconds / 60 / 480);
                    }
                    else
                    {
                        dictionary.Add("Day_" + i, ((int)totalSeconds / 60 / 60).ToString() + ":" + ((int)totalSeconds / 60 % 60).ToString());
                        dictionary.Add("Day_" + i + "_FooterTotal", ((int)totalFooterSeconds / 60 / 60).ToString() + ":" + ((int)totalFooterSeconds / 60 % 60).ToString());
                    }
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


            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("FirstName", result.FirstName);
                dictionary.Add("LastName", result.LastName);
                dictionary.Add("ErpCode", result.ErpCode);

                for (int i = 0; i < _datatable.SpecificDates?.Count(); i++)
                {
                    var totalFooterSeconds = entities.Select(x => x.RealWorkHours
                            .Where(x => x.StartOn.Date == _datatable.SpecificDates[i])
                            .Select(x => new DateRangeService(x.StartOn, x.EndOn).ConvertToTotalWork().TotalSeconds)
                            .Sum())
                        .Sum();

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
                    dictionary.Add("Day_" + i + "_FooterTotal", ((int)totalFooterSeconds / 60 / 60).ToString() + ":" + ((int)totalFooterSeconds / 60 % 60).ToString());

                }

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<ProjectionDataTableWorker> ProjectionTimeShiftSuggestions()
        {
            var sum = 0.0;
            var includes = new List<Func<IQueryable<WorkHour>, IIncludableQueryable<WorkHour, object>>>();
            includes.Add(x => x.Include(y => y.Employee).ThenInclude(y => y.RealWorkHours));
            includes.Add(x => x.Include(y => y.Employee).ThenInclude(y => y.Contract));

            var filter = PredicateBuilder.New<WorkHour>();
            filter = filter.And(x => x.EndOn >= _datatable.StartOn && x.StartOn <= _datatable.EndOn);

            if (_datatable.FilterByWorkPlaceId != 0)
                filter = filter.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

            var entities = await _baseDatawork.WorkHours
               .GetPaggingWithFilter(x => x.OrderBy(y => y.StartOn), filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();


            //OverTime
            if (_datatable.FilterByValidateOvertime)
            {
                filter = PredicateBuilder.New<WorkHour>();
                filter = filter.And(x => x.Employee.WorkHours
                     .Where(y => y.Id != x.Id)
                     .Any(y => (x.StartOn - y.EndOn).TotalHours < 11));
                var overTimeEntities = entities.Where(filter).ToList();
                foreach (var result in overTimeEntities)
                {
                    var expandoObj = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expandoObj;

                    dictionary.Add("Id", result.Employee.Id);
                    dictionary.Add("FirstName", result.Employee.FirstName);
                    dictionary.Add("LastName", result.Employee.LastName);
                    dictionary.Add("ErpCode", result.Employee.ErpCode);
                    dictionary.Add("WorkHour", dataTableHelper.GetProjectionTimeShiftSuggestionCellBody(result));
                    dictionary.Add("Error", $"Βάρδια με λιγότερο απο 11 ώρες διαφορά");

                    returnObjects.Add(expandoObj);
                }
            }

            //Contract DayOff per week
            if (_datatable.FilterByValidateDayOfDaysPerWeek)
            {
                filter = PredicateBuilder.New<WorkHour>();
                filter = filter.And(x => true);

                var dayOfDaysPerWeekEntities = entities.Where(filter)
                    .GroupBy(x => new
                    {
                        Week = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetWeekOfYear(x.StartOn, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                        x.EmployeeId,
                        x.Employee?.Contract?.DayOfDaysPerWeek
                    })
                    .Where(x => x.Key.DayOfDaysPerWeek != null)
                    .Where(x => x.Key.DayOfDaysPerWeek < x.Count())
                    .Select(x => new
                    {
                        WorkHours = x.ToList(),
                        Error = $"Βάρδια ξεπερνάει το όριο 'Ρεπό ανα εβδομάδα' με όριο '{x.Key.DayOfDaysPerWeek}' στην ημέρα '{x.Key.Week}'"
                    })
                    .ToList();
                foreach (var group in dayOfDaysPerWeekEntities)
                {
                    foreach (var result in group.WorkHours)
                    {
                        var expandoObj = new ExpandoObject();
                        var dictionary = (IDictionary<string, object>)expandoObj;

                        dictionary.Add("Id", result.Employee.Id);
                        dictionary.Add("FirstName", result.Employee.FirstName);
                        dictionary.Add("LastName", result.Employee.LastName);
                        dictionary.Add("ErpCode", result.Employee.ErpCode);
                        dictionary.Add("WorkHour", dataTableHelper.GetProjectionTimeShiftSuggestionCellBody(result));
                        dictionary.Add("Error", group.Error);

                        returnObjects.Add(expandoObj);
                    }
                }
            }

            //Contract WorkDays per week 
            if (_datatable.FilterByValidateWorkDaysPerWeek)
            {
                filter = PredicateBuilder.New<WorkHour>();
                filter = filter.And(x => true);

                var workingDaysPerWeekEntities = entities.Where(filter)
                    .GroupBy(x => new
                    {
                        Week = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetWeekOfYear(x.StartOn, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                        x.EmployeeId,
                        x.Employee?.Contract?.WorkingDaysPerWeek
                    })
                    .Where(x => x.Key.WorkingDaysPerWeek != null)
                    .Where(x => x.Key.WorkingDaysPerWeek < x.Count())
                    .Select(x => new
                    {
                        WorkHours = x.ToList(),
                        Error = $"Βάρδια ξεπερνάει το όριο 'Εργάσιμες ημέρες ανα εβδομάδα' με όριο '{x.Key.WorkingDaysPerWeek}' στην ημέρα '{x.Key.Week}'"
                    })
                    .ToList();
                foreach (var group in workingDaysPerWeekEntities)
                {
                    foreach (var result in group.WorkHours)
                    {
                        var expandoObj = new ExpandoObject();
                        var dictionary = (IDictionary<string, object>)expandoObj;

                        dictionary.Add("Id", result.Employee.Id);
                        dictionary.Add("FirstName", result.Employee.FirstName);
                        dictionary.Add("LastName", result.Employee.LastName);
                        dictionary.Add("ErpCode", result.Employee.ErpCode);
                        dictionary.Add("WorkHour", dataTableHelper.GetProjectionTimeShiftSuggestionCellBody(result));
                        dictionary.Add("Error", group.Error);

                        returnObjects.Add(expandoObj);
                    }
                }
            }

            //Contract Hours per week
            if (_datatable.FilterByValidateHoursPerWeek)
            {
                filter = PredicateBuilder.New<WorkHour>();
                filter = filter.And(x => true);
                var hoursPerWeekEntities = entities.Where(filter)
                    .GroupBy(x => new
                    {
                        Week = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetWeekOfYear(x.StartOn, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                        x.EmployeeId,
                        x.Employee?.Contract?.HoursPerWeek
                    })
                    .Where(x => x.Key.HoursPerWeek != null)
                    .Select(x =>
                {
                    sum = 0.0;
                    return new
                    {
                        WorkHours = x.TakeWhile(y =>
                    {
                        sum += new DateRangeService(y.StartOn, y.EndOn).ConvertToTotalWork().TotalHours;
                        return sum < x.Key.HoursPerWeek;
                    }).ToList(),
                        Error = $"Βάρδια ξεπερνάει το όριο 'Εργάσιμες ώρες ανα εβδομάδα' με όριο '{x.Key.HoursPerWeek}' στην ημέρα '{x.Key.Week}'"

                    };
                })
                    .ToList();
                foreach (var group in hoursPerWeekEntities)
                {
                    foreach (var result in group.WorkHours)
                    {
                        var expandoObj = new ExpandoObject();
                        var dictionary = (IDictionary<string, object>)expandoObj;

                        dictionary.Add("Id", result.Employee.Id);
                        dictionary.Add("FirstName", result.Employee.FirstName);
                        dictionary.Add("LastName", result.Employee.LastName);
                        dictionary.Add("ErpCode", result.Employee.ErpCode);
                        dictionary.Add("WorkHour", dataTableHelper.GetProjectionTimeShiftSuggestionCellBody(result));
                        dictionary.Add("Error", group.Error);

                        returnObjects.Add(expandoObj);
                    }
                }
            }

            //Contract Hours per day
            if (_datatable.FilterByValidateWorkingHoursPerDay)
            {
                filter = PredicateBuilder.New<WorkHour>();
                filter = filter.And(x => true);
                var hoursPerDayEntities = entities.Where(filter)
                    .GroupBy(x => new
                    {
                        Day = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetDayOfYear(x.StartOn),
                        x.EmployeeId,
                        x.Employee?.Contract?.HoursPerDay
                    })
                    .Where(x => x.Key.HoursPerDay != null)
                    .Select(x =>
                    {
                        sum = 0.0;
                        return new
                        {
                            WorkHours = x.TakeWhile(y =>
                            {
                                sum += new DateRangeService(y.StartOn, y.EndOn).ConvertToTotalWork().TotalHours;
                                return sum < x.Key.HoursPerDay;
                            }).ToList(),
                            Error = $"Βάρδια ξεπερνάει το όριο 'Εργάσιμες ώρες ανα εβδομάδα' με όριο '{x.Key.HoursPerDay}' στην ημέρα '{x.Key.Day}'"
                        };
                    })
                    .ToList();
                foreach (var group in hoursPerDayEntities)
                {
                    foreach (var result in group.WorkHours)
                    {
                        var expandoObj = new ExpandoObject();
                        var dictionary = (IDictionary<string, object>)expandoObj;

                        dictionary.Add("Id", result.Employee.Id);
                        dictionary.Add("FirstName", result.Employee.FirstName);
                        dictionary.Add("LastName", result.Employee.LastName);
                        dictionary.Add("ErpCode", result.Employee.ErpCode);
                        dictionary.Add("WorkHour", dataTableHelper.GetProjectionTimeShiftSuggestionCellBody(result));
                        dictionary.Add("Error", group.Error);

                        returnObjects.Add(expandoObj);
                    }
                }
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<ProjectionDataTableWorker> ProjectionHoursWithComments()
        {
            if (_datatable.FilterByRealWorkHour)
            {
                var realWorkHourIncludes = new List<Func<IQueryable<RealWorkHour>, IIncludableQueryable<RealWorkHour, object>>>();

                realWorkHourIncludes.Add(x => x.Include(y => y.TimeShift).ThenInclude(y => y.WorkPlace));
                realWorkHourIncludes.Add(x => x.Include(y => y.Employee));

                var realWorkHourfilter = PredicateBuilder.New<RealWorkHour>();

                realWorkHourfilter = realWorkHourfilter.And(x => x.Comments.Length > 1);

                if (_datatable.FilterByWorkPlaceId != 0)
                    realWorkHourfilter = realWorkHourfilter.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

                if (_datatable.FilterByEmployeeId != 0)
                    realWorkHourfilter = realWorkHourfilter.And(x => x.EmployeeId == _datatable.FilterByEmployeeId);

                if (_datatable.StartOn != null && _datatable.EndOn != null)
                    realWorkHourfilter = realWorkHourfilter.And(x => x.EndOn >= _datatable.StartOn && x.StartOn <= _datatable.EndOn);


                var entities = await _baseDatawork.RealWorkHours
                   .GetPaggingWithFilter(x => x.OrderBy(x => x.StartOn), realWorkHourfilter, realWorkHourIncludes, _pageSize, _pageIndex);

                //Mapping
                var expandoService = new ExpandoService();
                var dataTableHelper = new DataTableHelper<Employee>();
                var returnObjects = new List<ExpandoObject>();

                foreach (var result in entities)
                {
                    var expandoObj = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expandoObj;

                    dictionary.Add("LastName", result.Employee.LastName);
                    dictionary.Add("FirstName", result.Employee.FirstName);
                    dictionary.Add("ErpCode", result.Employee.ErpCode);
                    dictionary.Add("Hour", result.StartOn.ToString() + " - " + result.EndOn.ToString());
                    dictionary.Add("Hour_Type", "Πραγματική");
                    dictionary.Add("WorkPlace_Title", result.TimeShift.WorkPlace.Title);
                    dictionary.Add("Comment", result.Comments);

                    returnObjects.Add(expandoObj);
                }
                EntitiesMapped = returnObjects;
                EntitiesTotal = await _baseDatawork.RealWorkHours.CountAllAsyncFiltered(realWorkHourfilter);

            }
            else
            {
                var workHourIncludes = new List<Func<IQueryable<WorkHour>, IIncludableQueryable<WorkHour, object>>>();

                workHourIncludes.Add(x => x.Include(y => y.TimeShift).ThenInclude(y => y.WorkPlace));
                workHourIncludes.Add(x => x.Include(y => y.Employee));

                var workHourfilter = PredicateBuilder.New<WorkHour>();

                workHourfilter = workHourfilter.And(x => x.Comments.Length > 1);
                if (_datatable.FilterByWorkPlaceId != 0)
                    workHourfilter = workHourfilter.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

                if (_datatable.FilterByEmployeeId != 0)
                    workHourfilter = workHourfilter.And(x => x.EmployeeId == _datatable.FilterByEmployeeId);

                if (_datatable.StartOn != null && _datatable.EndOn != null)
                    workHourfilter = workHourfilter.And(x => x.EndOn >= _datatable.StartOn && x.StartOn <= _datatable.EndOn);

                var entities = await _baseDatawork.WorkHours
                   .GetPaggingWithFilter(x => x.OrderBy(x => x.StartOn), workHourfilter, workHourIncludes, _pageSize, _pageIndex);

                //Mapping
                var expandoService = new ExpandoService();
                var dataTableHelper = new DataTableHelper<Employee>();
                var returnObjects = new List<ExpandoObject>();

                foreach (var result in entities)
                {
                    var expandoObj = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expandoObj;

                    dictionary.Add("LastName", result.Employee.LastName);
                    dictionary.Add("FirstName", result.Employee.FirstName);
                    dictionary.Add("ErpCode", result.Employee.ErpCode);
                    dictionary.Add("Hour", result.StartOn.ToString() + " - " + result.EndOn.ToString());
                    dictionary.Add("Hour_Type", "Χρονοδιαγράμματος");
                    dictionary.Add("WorkPlace_Title", result.TimeShift.WorkPlace.Title);
                    dictionary.Add("Comment", result.Comments);

                    returnObjects.Add(expandoObj);
                }
                EntitiesMapped = returnObjects;
                EntitiesTotal = await _baseDatawork.WorkHours.CountAllAsyncFiltered(workHourfilter);

            }
            return this;
        }

        public async Task<ProjectionDataTableWorker> ProjectionRealWorkHourRestrictions()
        {
            var filter = PredicateBuilder.New<RealWorkHour>();
            var includes = new List<Func<IQueryable<RealWorkHour>, IIncludableQueryable<RealWorkHour, object>>>();
            includes.Add(x => x.Include(y => y.Employee));
            includes.Add(x => x.Include(y => y.TimeShift)
                .ThenInclude(y => y.WorkPlace)
                .ThenInclude(y => y.WorkPlaceHourRestrictions)
                .ThenInclude(y => y.HourRestrictions));

            filter = filter.And(x => _datatable.StartOn <= x.StartOn && x.StartOn <= _datatable.EndOn);

            if (_datatable.FilterByWorkPlaceId != 0)
                filter = filter.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

            var entities = (await _baseDatawork.RealWorkHours
                .GetPaggingWithFilter(x => x.OrderBy(y => y.StartOn), filter, includes))
                .GroupBy(x => new
                {
                    x.StartOn.Date,
                    x.TimeShift.WorkPlace
                })
                .Select(x => new
                {
                    x.Key.Date,
                    x.Key.WorkPlace,
                    MaxPermitedSeconds = x.Key.WorkPlace.WorkPlaceHourRestrictions
                        .FirstOrDefault(y => y.Month == x.Key.Date.Month && y.Year == x.Key.Date.Year)
                        .HourRestrictions.FirstOrDefault(y => y.Day == x.Key.Date.Day)
                        .MaxTicks,
                    TotalDaySeconds = x.Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToTotalWork().TotalSeconds)
                                     .Sum()
                })
                .ToList();

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Company>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
                if (result.MaxPermitedSeconds != 0 && result.MaxPermitedSeconds < result.TotalDaySeconds)
                {
                    var expandoObj = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expandoObj;

                    dictionary.Add("WorkPlaceTitle", result.WorkPlace.Title);
                    dictionary.Add("DateError", result.Date.ToShortDateString());
                    dictionary.Add("Error", $"Μέγιστο όριο ημέρας: {GetTime(result.MaxPermitedSeconds) } Εισήχθησαν: {GetTime(result.TotalDaySeconds) }");

                    returnObjects.Add(expandoObj);
                }

            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.RealWorkHours.CountAllAsyncFiltered(filter);

            return this;
        }

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
