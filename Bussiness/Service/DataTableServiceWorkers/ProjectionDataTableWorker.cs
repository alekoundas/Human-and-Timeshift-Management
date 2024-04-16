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

            if (_columnName == "Title")
                return null;
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Func<IQueryable<RealWorkHour>, IOrderedQueryable<RealWorkHour>> SetOrderByRealWorkHour()
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
                    else if (column.Data == "FirstName")
                        filter = filter.Or(x => x.FirstName.Contains(_datatable.Search.Value));
                    else if (column.Data == "LastName")
                        filter = filter.Or(x => x.LastName.Contains(_datatable.Search.Value));
                    else if (column.Data == "ErpCode")
                        filter = filter.Or(x => x.ErpCode.Contains(_datatable.Search.Value));
                    else if (column.Data == "WorkPlaceTitle")
                        filter = filter.Or(x => x.EmployeeWorkPlaces.Any(y => y.WorkPlace.Title.Contains(_datatable.Search.Value)));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }

        private Expression<Func<RealWorkHour, bool>> GetSearchFilterRealWorkHour()
        {
            var filter = PredicateBuilder.New<RealWorkHour>();
            if (_datatable.Search.Value != "")
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "VatNumber")
                        filter = filter.Or(x => x.Employee.VatNumber.Contains(_datatable.Search.Value));
                    else if (column.Data == "FirstName")
                        filter = filter.Or(x => x.Employee.FirstName.Contains(_datatable.Search.Value));
                    else if (column.Data == "ErpCode")
                        filter = filter.Or(x => x.Employee.ErpCode.Contains(_datatable.Search.Value));
                    else if (column.Data == "LastName")
                        filter = filter.Or(x => x.Employee.LastName.Contains(_datatable.Search.Value));
                    else if (column.Data == "WorkPlaceTitle")
                        filter = filter.Or(x => x.TimeShift.WorkPlace.Title.Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);

            return filter;
        }

        private Expression<Func<WorkHour, bool>> GetSearchFilterWorkHour()
        {
            var filter = PredicateBuilder.New<WorkHour>();

            if (_datatable.Search.Value != "")
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "VatNumber")
                        filter = filter.Or(x => x.Employee.VatNumber.Contains(_datatable.Search.Value));
                    else if (column.Data == "FirstName")
                        filter = filter.Or(x => x.Employee.FirstName.Contains(_datatable.Search.Value));
                    else if (column.Data == "ErpCode")
                        filter = filter.Or(x => x.Employee.ErpCode.Contains(_datatable.Search.Value));
                    else if (column.Data == "LastName")
                        filter = filter.Or(x => x.Employee.LastName.Contains(_datatable.Search.Value));
                    else if (column.Data == "WorkPlaceTitle")
                        filter = filter.Or(x => x.TimeShift.WorkPlace.Title.Contains(_datatable.Search.Value));
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
                .GetWithFilterQueryable(x => x.OrderBy(y => y.StartOn), filter, includes, _pageSize, _pageIndex)
                .Select(x => new
                {
                    x.Id,
                    x.Employee.FirstName,
                    x.Employee.LastName,
                    x.TimeShift.WorkPlace.Title,
                    x.StartOn,
                    x.EndOn
                }).ToListAsync();


            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<RealWorkHour>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("ToggleSlider", dataTableHelper
                    .GetEmployeeCheckbox(_datatable, result.Id));

                dictionary.Add("EmployeeFirstName", result.FirstName);
                dictionary.Add("EmployeeLastName", result.LastName);
                dictionary.Add("WorkPlaceTitle", result.Title);
                dictionary.Add("StartOn_string", result.StartOn.ToString());
                dictionary.Add("EndOn_string", result.EndOn.ToString());

                dictionary.Add("Buttons", dataTableHelper
                   .GetCurrentDayButtons(result.Id));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.RealWorkHours.CountAllAsyncFiltered(filter);

            return this;
        }


        public async Task<ProjectionDataTableWorker> ProjectionConcentric()
        {
            var includes = new List<Func<IQueryable<RealWorkHour>, IIncludableQueryable<RealWorkHour, object>>>();
            includes.Add(x => x.Include(y => y.Employee));
            var filter = PredicateBuilder.New<RealWorkHour>();
            filter = filter.And(x => true);
            filter = filter.And(GetSearchFilterRealWorkHour());
            filter = filter.And(x => _datatable.StartOn <= x.StartOn && x.StartOn <= _datatable.EndOn);

            if (_datatable.FilterByWorkPlaceId != 0)
                filter = filter.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);


            var entities = (await _baseDatawork.RealWorkHours
                .GetPaggingWithFilter(null, filter, includes, _pageSize, _pageIndex))
                .GroupBy(x => x.Employee)
                .Select(x => new
                {
                    Employee = x.Key,
                    TotalSeconds = x.Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToTotalWork().TotalSeconds).Sum(),
                    TotalSecondsDay = x.Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToDayWork().TotalSeconds).Sum(),
                    TotalSecondsNight = x.Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToNightWork().TotalSeconds).Sum(),
                    TotalSecondsSaturdayDay = x.Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToSaturdayDayWork().TotalSeconds).Sum(),
                    TotalSecondsSaturdayNight = x.Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToSaturdayNightWork().TotalSeconds).Sum(),
                    TotalSecondsSundayDay = x.Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToSundayDayWork().TotalSeconds).Sum(),
                    TotalSecondsSundayNight = x.Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToSundayNightWork().TotalSeconds).Sum()
                });


            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            var totalFooter = entities.Select(x => x.TotalSeconds).Sum();
            var totalFooterDay = entities.Select(x => x.TotalSecondsDay).Sum();
            var totalFooterNight = entities.Select(x => x.TotalSecondsNight).Sum();
            var totalFooterSaturdayDay = entities.Select(x => x.TotalSecondsSaturdayDay).Sum();
            var totalFooterSaturdayNight = entities.Select(x => x.TotalSecondsSaturdayNight).Sum();
            var totalFooterSundayDay = entities.Select(x => x.TotalSecondsSundayDay).Sum();
            var totalFooterSundayNight = entities.Select(x => x.TotalSecondsSundayNight).Sum();

            foreach (var result in entities)
            {

                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("FirstName", result.Employee.FirstName);
                dictionary.Add("LastName", result.Employee.LastName);
                dictionary.Add("ErpCode", result.Employee.ErpCode);

                if (_datatable.ShowHoursInPercentage)
                {
                    dictionary.Add("TotalHours", result.TotalSeconds / 60 / 480);
                    dictionary.Add("TotalHoursDay", result.TotalSecondsDay / 60 / 480);
                    dictionary.Add("TotalHoursNight", result.TotalSecondsNight / 60 / 480);
                    dictionary.Add("TotalHoursSaturdayDay", result.TotalSecondsSaturdayDay / 60 / 480);
                    dictionary.Add("TotalHoursSaturdayNight", result.TotalSecondsSaturdayNight / 60 / 480);
                    dictionary.Add("TotalHoursSundayDay", result.TotalSecondsSundayDay / 60 / 480);
                    dictionary.Add("TotalHoursSundayNight", result.TotalSecondsSundayNight / 60 / 480);

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
                    dictionary.Add("TotalHours", ((int)result.TotalSeconds / 60 / 60).ToString() + ":" + ((int)result.TotalSeconds / 60 % 60).ToString());
                    dictionary.Add("TotalHoursDay", ((int)result.TotalSecondsDay / 60 / 60).ToString() + ":" + ((int)result.TotalSecondsDay / 60 % 60).ToString());
                    dictionary.Add("TotalHoursNight", ((int)result.TotalSecondsNight / 60 / 60).ToString() + ":" + ((int)result.TotalSecondsNight / 60 % 60).ToString());
                    dictionary.Add("TotalHoursSaturdayDay", ((int)result.TotalSecondsSaturdayDay / 60 / 60).ToString() + ":" + ((int)result.TotalSecondsSaturdayDay / 60 % 60).ToString());
                    dictionary.Add("TotalHoursSaturdayNight", ((int)result.TotalSecondsSaturdayNight / 60 / 60).ToString() + ":" + ((int)result.TotalSecondsSaturdayNight / 60 % 60).ToString());
                    dictionary.Add("TotalHoursSundayDay", ((int)result.TotalSecondsSundayDay / 60 / 60).ToString() + ":" + ((int)result.TotalSecondsSundayDay / 60 % 60).ToString());
                    dictionary.Add("TotalHoursSundayNight", ((int)result.TotalSecondsSundayNight / 60 / 60).ToString() + ":" + ((int)result.TotalSecondsSundayNight / 60 % 60).ToString());

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
                                .Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToTotalWork().TotalSeconds)
                                .Sum(),
                TotalDaySeconds = x.ToList()
                                .Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToDayWork().TotalSeconds)
                                .Sum(),
                TotalNightSeconds = x.ToList()
                                .Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToNightWork().TotalSeconds)
                                .Sum(),
                TotalSaturdayDaySeconds = x.ToList()
                                .Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToSaturdayDayWork().TotalSeconds)
                                .Sum(),
                TotalSaturdayNightSeconds = x.ToList()
                                .Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToSaturdayNightWork().TotalSeconds)
                                .Sum(),
                TotalSundayDaySeconds = x.ToList()
                                .Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToSundayDayWork().TotalSeconds)
                                .Sum(),
                TotalSundayNightSeconds = x.ToList()
                                .Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToSundayNightWork().TotalSeconds)
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
            filterWorkHour = filterWorkHour.And(GetSearchFilterWorkHour());
            filterWorkHour = filterWorkHour.And(x => _datatable.StartOn <= x.StartOn && x.StartOn <= _datatable.EndOn);
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
            filterRealWorkHour = filterRealWorkHour.And(GetSearchFilterRealWorkHour());
            filterRealWorkHour = filterRealWorkHour.And(x => _datatable.StartOn <= x.StartOn && x.StartOn <= _datatable.EndOn);
            filterRealWorkHour = filterRealWorkHour.And(x => !x.Employee.WorkHours.Any(y => y.StartOn <= x.StartOn && x.EndOn <= y.EndOn));

            if (_datatable.FilterByWorkPlaceId != 0)
                filterRealWorkHour = filterRealWorkHour.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

            var entitiesRealWorkHour = await _baseDatawork.RealWorkHours
                .GetPaggingWithFilter(x => x.OrderBy(y => y.StartOn), filterRealWorkHour, includesRealWorkHour, _pageSize, _pageIndex);

            var totalFooterRealWorkHours = entitiesRealWorkHour
                    .Select(x => Math.Abs((x.EndOn.Value - x.StartOn).TotalSeconds))
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
            var includes = new List<Func<IQueryable<RealWorkHour>, IIncludableQueryable<RealWorkHour, object>>>();
            includes.Add(x => x.Include(y => y.TimeShift).ThenInclude(z => z.WorkPlace));

            var filter = PredicateBuilder.New<RealWorkHour>();
            filter = filter.And(x => true);
            filter = filter.And(GetSearchFilterRealWorkHour());
            filter = filter.And(x => _datatable.StartOn <= x.StartOn && x.StartOn <= _datatable.EndOn);


            if (_datatable.FilterByEmployeeId != 0)
                filter = filter.And(x => x.EmployeeId == _datatable.FilterByEmployeeId);

            if (_datatable.FilterByWorkPlaceId != 0)
                filter = filter.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

            var entities = (await _baseDatawork.RealWorkHours
                .GetPaggingWithFilter(null, filter, includes, _pageSize, _pageIndex))
                .GroupBy(x => x.TimeShift.WorkPlace)
                .Select(x => new
                {
                    WorkPlace = x.Key,
                    TotalHours = x.Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToTotalWork().TotalSeconds).Sum(),
                    TotalHoursDay = x.Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToDayWork().TotalSeconds).Sum(),
                    TotalHoursNight = x.Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToNightWork().TotalSeconds).Sum()
                });


            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlace>();
            var returnObjects = new List<ExpandoObject>();

            var totalFooter = entities.Select(x => x.TotalHours).Sum();
            var totalFooterDay = entities.Select(x => x.TotalHoursDay).Sum();
            var totalFooterNight = entities.Select(x => x.TotalHoursNight).Sum();

            foreach (var result in entities)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("WorkPlaceTitle", result.WorkPlace.Title);
                var totalSeconds = result.TotalHours;
                var totalSecondsDay = result.TotalHoursDay;
                var totalSecondsNight = result.TotalHoursNight;

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
            EntitiesTotal = 0;

            return this;
        }


        public async Task<ProjectionDataTableWorker> ProjectionPresenceDaily()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.RealWorkHours).ThenInclude(y => y.TimeShift).ThenInclude(y => y.WorkPlace));

            //Get employees that have a realworkhour today
            if (_datatable.FilterByWorkPlaceId != 0)
                _filter = _filter.And(x => x.EmployeeWorkPlaces
                .Any(y => y.WorkPlaceId == _datatable.FilterByWorkPlaceId));

            _filter = _filter.And(x => x.RealWorkHours.Any(y => y.StartOn.Date == DateTime.Now.AddHours(3).Date));

            var entities = await _baseDatawork.Employees
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            var totalFooterSeconds = entities.Select(x => x.RealWorkHours
                    .Where(x => x.StartOn.Date == DateTime.Now.Date)
                    .Select(x => Math.Abs((x.EndOn.Value - x.StartOn).TotalSeconds))
                    .Sum())
                .Sum();

            foreach (var result in entities)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                var todayCell = "";
                var workPlaceTitle = "";
                var realWorkHours = result.RealWorkHours
                    .Where(x => x.StartOn.Date == DateTime.Now.Date)
                    .ToList();

                //Add multiple work hours on the same cell 
                foreach (var realWorkHour in realWorkHours)
                {
                    workPlaceTitle = realWorkHour.TimeShift.WorkPlace.Title;
                    todayCell += "<p style='white-space:nowrap;'>" +
                        realWorkHour.StartOn.ToShortTimeString() +
                        " - " +
                        realWorkHour.EndOn.Value.ToShortTimeString() +
                        "</p></br>";
                }

                dictionary.Add("FirstName", result.FirstName);
                dictionary.Add("LastName", result.LastName);
                dictionary.Add("ErpCode", result.ErpCode);
                dictionary.Add("VatNumber", result.VatNumber);
                dictionary.Add("WorkPlaceTitle", workPlaceTitle);
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

            //var entities = await _baseDatawork.Employees
            //   .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);
            var entities = await _baseDatawork.Employees
            .GetWithFilterQueryable(SetOrderBy(), _filter, includes)
            .Select(x => new
            {
                x.FirstName,
                x.LastName,
                x.ErpCode,
                x.VatNumber,
                RealWorkHours = x.RealWorkHours
                    .Where(y => _datatable.StartOn.Date <= y.StartOn.Date && y.StartOn.Date <= _datatable.EndOn.Date)
                    .Select(y => new RealWorkHour
                    {
                        StartOn = y.StartOn,
                        EndOn = y.EndOn,
                        TimeShift = new TimeShift { WorkPlaceId = y.TimeShift.WorkPlaceId }
                    }).ToList(),
            }).ToListAsync();



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
                            .Select(x => new DateRangeService(x.StartOn, x.EndOn.Value).ConvertToTotalWork().TotalSeconds)
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

            //var entities = await _baseDatawork.Employees
            //   .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);


            var entities = await _baseDatawork.Employees
            .GetWithFilterQueryable(SetOrderBy(), _filter, includes)
            .Select(x => new
            {
                x.FirstName,
                x.LastName,
                x.ErpCode,
                x.VatNumber,
                RealWorkHours = x.RealWorkHours
                    .Where(y => _datatable.StartOn.Date <= y.StartOn.Date && y.StartOn.Date <= _datatable.EndOn.Date)
                    .Select(y => new RealWorkHour
                    {
                        StartOn = y.StartOn,
                        EndOn = y.EndOn,
                        TimeShift = new TimeShift { WorkPlaceId = y.TimeShift.WorkPlaceId }
                    }).ToList(),
            }).ToListAsync();


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
                            .Select(x => new DateRangeService(x.StartOn, x.EndOn.Value).ConvertToTotalWork().TotalSeconds)
                        .Sum();

                    var totalSeconds = result.RealWorkHours
                        .Where(realWorkHoursFilter)
                        .Select(x => new DateRangeService(x.StartOn, x.EndOn.Value).ConvertToTotalWork().TotalSeconds)
                        .Sum();


                    if (_datatable.ShowHoursInPercentage)
                    {
                        dictionary.Add("Day_" + i, totalSeconds / 60 / 480);
                        dictionary.Add("Day_" + i + "_FooterTotal", totalFooterSeconds / 60 / 480);
                    }
                    else
                    {
                        var minutes = ((int)totalSeconds / 60 % 60).ToString();
                        if (minutes.Length == 1)
                            minutes = "0" + minutes;

                        var footerMinutes = ((int)totalFooterSeconds / 60 % 60).ToString();
                        if (footerMinutes.Length == 1)
                            footerMinutes = "0" + footerMinutes;

                        dictionary.Add("Day_" + i, ((int)totalSeconds / 60 / 60).ToString() + ":" + minutes);
                        dictionary.Add("Day_" + i + "_FooterTotal", ((int)totalFooterSeconds / 60 / 60).ToString() + ":" + footerMinutes);
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
                .GetWithFilterQueryable(SetOrderBy(), _filter, includes)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.ErpCode,
                    x.VatNumber,
                    RealWorkHours = x.RealWorkHours.Where(y => _datatable.SpecificDates.Contains(y.StartOn.Date)).Select(y => new { y.StartOn, y.EndOn }),
                    Leaves = x.Leaves.Where(y => _datatable.SpecificDates.Contains(y.StartOn.Date)).Select(y => new { y.StartOn, y.EndOn }),
                    WorkHours = x.WorkHours.Where(y => _datatable.SpecificDates.Contains(y.StartOn.Date)).Select(y => new { y.StartOn, y.EndOn })
                }).ToListAsync();





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
                            .Select(x => new DateRangeService(x.StartOn, x.EndOn.Value).ConvertToTotalWork().TotalSeconds)
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
                            realWorkHour.EndOn.Value.ToShortTimeString() +
                            "</p></br>";

                    if (dayOffs.Count == 0)
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

        public async Task<ProjectionDataTableWorker> ProjectionEmployeeConsecutiveDayOff()
        {

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlace>();
            var returnObjects = new List<ExpandoObject>();
            var entities = new List<Employee>();

            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x =>
                x.Include(y => y.RealWorkHours)
                .ThenInclude(y => y.TimeShift)
                .ThenInclude(y => y.WorkPlace));


            if (_datatable.FilterByTimeShiftId > 0)
                entities = await _baseDatawork.Employees
                   .GetWithFilterQueryable(x => x.OrderBy(y => y.LastName), _filter, includes)
                   .Select(x => new Employee
                   {
                       Id = x.Id,
                       FirstName = x.FirstName,
                       LastName = x.LastName,
                       VatNumber = x.VatNumber,
                       EmployeeWorkPlaces = x.EmployeeWorkPlaces.Select(y => new EmployeeWorkPlace
                       {
                           WorkPlace = new WorkPlace
                           {
                               Title = y.WorkPlace.Title,
                               TimeShifts = y.WorkPlace.TimeShifts
                                    .Where(y => _datatable.StartOn.Month <= y.Month && y.Month <= _datatable.EndOn.Month)
                                    .Where(y => _datatable.StartOn.Year <= y.Year && y.Year <= _datatable.EndOn.Year)
                                    .Select(z =>
                                        new TimeShift
                                        {
                                            Id = z.Id,
                                            Title = z.Title
                                        })
                                    .ToList()
                           }
                       }).ToList(),
                       RealWorkHours = x.RealWorkHours
                           .Where(y => _datatable.StartOn.Date <= y.StartOn.Date && y.StartOn.Date <= _datatable.EndOn.Date)
                           .Where(y => y.TimeShiftId == _datatable.FilterByTimeShiftId)
                           .Select(y => new RealWorkHour
                           {
                               StartOn = y.StartOn,
                               EndOn = y.EndOn,
                               TimeShiftId = y.TimeShiftId
                           }).ToList()
                   }).ToListAsync();
            else
                entities = await _baseDatawork.Employees
                    .GetWithFilterQueryable(x => x.OrderBy(y => y.LastName), _filter, includes)
                    .Select(x => new Employee
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        VatNumber = x.VatNumber,
                        EmployeeWorkPlaces = x.EmployeeWorkPlaces.Select(y => new EmployeeWorkPlace
                        {
                            WorkPlace = new WorkPlace
                            {
                                Title = y.WorkPlace.Title,
                                TimeShifts = y.WorkPlace.TimeShifts
                                .Where(y => _datatable.StartOn.Month <= y.Month && y.Month <= _datatable.EndOn.Month)
                                .Where(y => _datatable.StartOn.Year <= y.Year && y.Year <= _datatable.EndOn.Year)
                                .Select(z =>
                                    new TimeShift
                                    {
                                        Id = z.Id,
                                        Title = z.Title,
                                        WorkPlace = new WorkPlace { Title = z.WorkPlace.Title }
                                    })
                                .ToList()
                            }
                        }).ToList(),
                        RealWorkHours = x.RealWorkHours
                            .Where(y => _datatable.StartOn.Date <= y.StartOn.Date && y.StartOn.Date <= _datatable.EndOn.Date)
                            .Select(y => new RealWorkHour
                            {
                                StartOn = y.StartOn,
                                EndOn = y.EndOn,
                                TimeShiftId = y.TimeShiftId
                            }).ToList()
                    }).ToListAsync();



            //Create a list of consecutive dates from date range filter
            var startOnFilter = _datatable.StartOn.Date;
            var endOnFilter = _datatable.EndOn.Date;
            var consecutiveDays = new List<DateTime>();

            while (startOnFilter != endOnFilter)
            {
                consecutiveDays.Add(startOnFilter);
                startOnFilter = startOnFilter.AddDays(1);
            }
            consecutiveDays.Add(startOnFilter);



            foreach (var employee in entities)
            {

                var workhours = employee.RealWorkHours
                    .GroupBy(x => x.TimeShiftId);

                if (workhours.Any())
                {
                    var asdasdf = "sdfsdsdsdsd";
                }

                foreach (var timeShift in employee.EmployeeWorkPlaces.SelectMany(x => x.WorkPlace.TimeShifts))
                {
                    if (workhours.Any(y => y.Key == timeShift.Id))
                        foreach (var timeshift_workhours in workhours)
                        {
                            var dayOffDays = new List<DateTime>();
                            dayOffDays.AddRange(consecutiveDays);
                            dayOffDays.RemoveAll(x => timeshift_workhours.Any(y => y.StartOn.Date == x));

                            var consecuriveCounter = 1;
                            if (dayOffDays.Count > 1)
                                for (int i = 1; i < dayOffDays.Count; i++)
                                    if (dayOffDays[i] == dayOffDays[i - 1].AddDays(1))
                                        consecuriveCounter++;
                                    else
                                    {
                                        if (consecuriveCounter <= _datatable.FilterByConsecutiveDayOff_Max && consecuriveCounter >= _datatable.FilterByConsecutiveDayOff_Min)
                                        {
                                            var expandoObj = new ExpandoObject();
                                            var dictionary = (IDictionary<string, object>)expandoObj;

                                            dictionary.Add("FirstName", employee.FirstName);
                                            dictionary.Add("LastName", employee.LastName);
                                            dictionary.Add("VatNumber", employee.VatNumber);
                                            dictionary.Add("WorkPlaceTitle", timeShift.WorkPlace.Title);
                                            dictionary.Add("TimeShiftTitle", timeShift.Title);
                                            dictionary.Add("ConsecutiveDays", consecuriveCounter);
                                            returnObjects.Add(expandoObj);
                                        }
                                        consecuriveCounter = 1;
                                    }

                            if (consecuriveCounter > 0 && consecuriveCounter <= _datatable.FilterByConsecutiveDayOff_Max && consecuriveCounter >= _datatable.FilterByConsecutiveDayOff_Min)
                            {
                                var expandoObj = new ExpandoObject();
                                var dictionary = (IDictionary<string, object>)expandoObj;

                                dictionary.Add("FirstName", employee.FirstName);
                                dictionary.Add("LastName", employee.LastName);
                                dictionary.Add("VatNumber", employee.VatNumber);
                                dictionary.Add("WorkPlaceTitle", timeShift.WorkPlace.Title);
                                dictionary.Add("TimeShiftTitle", timeShift.Title);
                                dictionary.Add("ConsecutiveDays", consecuriveCounter);
                                returnObjects.Add(expandoObj);

                            }
                        }
                    else
                    {
                        if (consecutiveDays.Count <= _datatable.FilterByConsecutiveDayOff_Max && consecutiveDays.Count >= _datatable.FilterByConsecutiveDayOff_Min)
                        {
                            var expandoObj = new ExpandoObject();
                            var dictionary = (IDictionary<string, object>)expandoObj;

                            dictionary.Add("FirstName", employee.FirstName);
                            dictionary.Add("LastName", employee.LastName);
                            dictionary.Add("VatNumber", employee.VatNumber);
                            dictionary.Add("WorkPlaceTitle", timeShift.WorkPlace.Title);
                            dictionary.Add("TimeShiftTitle", timeShift.Title);
                            dictionary.Add("ConsecutiveDays", consecutiveDays.Count);
                            returnObjects.Add(expandoObj);
                        }
                    }
                }
            }

            EntitiesMapped = returnObjects;
            EntitiesTotal = 0;

            return this;
        }


        //public async Task<ProjectionDataTableWorker> ProjectionEmployeeConsecutiveDayOff()
        //{

        //    //Mapping
        //    var expandoService = new ExpandoService();
        //    var dataTableHelper = new DataTableHelper<WorkPlace>();
        //    var returnObjects = new List<ExpandoObject>();

        //    var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
        //    includes.Add(x => x.Include(y => y.RealWorkHours).ThenInclude(y => y.TimeShift).ThenInclude(y => y.WorkPlace));


        //    if (_datatable.FilterByTimeShiftId > 0)
        //        _filter = _filter.And(x =>
        //            x.EmployeeWorkPlaces.Any(y =>
        //                y.WorkPlace.TimeShifts.Any(z =>
        //                    z.Id == _datatable.FilterByTimeShiftId)));

        //    _filter = _filter.And(x =>
        //        x.RealWorkHours.Any(y => y.StartOn.Month == _datatable.StartOn.Month && y.StartOn.Year == _datatable.StartOn.Year) ||
        //        x.RealWorkHours.Any(y => y.StartOn.Month == _datatable.StartOn.AddMonths(-1).Month && y.StartOn.Year == _datatable.StartOn.AddMonths(-1).Year)
        //    );

        //    if (_datatable.FilterByTimeShiftId > 0)
        //    {

        //        var entities = await _baseDatawork.Employees
        //            .GetWithFilterQueryable(x => x.OrderBy(y => y.LastName), _filter, includes)
        //            .Select(x => new
        //            {
        //                Id = x.Id,
        //                FirstName = x.FirstName,
        //                LastName = x.LastName,
        //                VatNumber = x.VatNumber,
        //                TimeShifts = x.EmployeeWorkPlaces
        //                    .SelectMany(y => y.WorkPlace.TimeShifts.Where(z => z.Id == _datatable.FilterByTimeShiftId))
        //                    .Select(y => new
        //                    {
        //                        y.Id,
        //                        y.Title,
        //                        y.Month,
        //                        y.Year,
        //                        y.DaysInMonth,
        //                        WorkPlaceTitle = y.WorkPlace.Title
        //                    }),
        //                RealWorkHours = x.RealWorkHours
        //                    .Where(y => _datatable.StartOn.Date <= y.StartOn.Date && y.StartOn.Date <= _datatable.EndOn.Date)
        //                    .Where(y => y.TimeShiftId == _datatable.FilterByTimeShiftId)
        //                    //.Where(y => x.HireDate.Date <= y.StartOn.Date)
        //                    .Select(y => new
        //                    {
        //                        Day = y.StartOn.Day,
        //                        TimeShiftId = y.TimeShiftId
        //                    })
        //            }).ToListAsync();

        //        foreach (var employee in entities)
        //        {
        //            foreach (var timeShift in employee.TimeShifts)
        //            {

        //                var daysInMonth = timeShift.DaysInMonth;
        //                var workingDays = employee.RealWorkHours
        //                    .Where(x => x.TimeShiftId == timeShift.Id)
        //                    .Select(x => x.Day)
        //                    .ToList()
        //                    .Distinct();

        //                //Dont count days in future
        //                if (timeShift.Year == DateTime.Now.Year && timeShift.Month == DateTime.Now.Month)
        //                    daysInMonth = Math.Abs(DateTime.Now.Day - timeShift.DaysInMonth);


        //                var dayOffs =
        //                   Enumerable.Range(1, daysInMonth).ToList()
        //                   .Except(workingDays)
        //                   .OrderBy(x => x)
        //                   .ToList();

        //                var consecuriveCounter = 1;
        //                if (dayOffs.Count > 1)
        //                    for (int i = 1; i < dayOffs.Count; i++)
        //                    {
        //                        //If consecutive
        //                        if (dayOffs[i] == dayOffs[i - 1] + 1)
        //                            consecuriveCounter++;
        //                        else
        //                        {
        //                            if (consecuriveCounter >= _datatable.FilterByConsecutiveDayOff_Min && consecuriveCounter <= _datatable.FilterByConsecutiveDayOff_Max)
        //                            {
        //                                if (dayOffs[i - consecuriveCounter] - 1 > 0)
        //                                {
        //                                    var expandoObj = new ExpandoObject();
        //                                    var dictionary = (IDictionary<string, object>)expandoObj;

        //                                    dictionary.Add("FirstName", employee.FirstName);
        //                                    dictionary.Add("LastName", employee.LastName);
        //                                    dictionary.Add("VatNumber", employee.VatNumber);
        //                                    dictionary.Add("WorkPlaceTitle", timeShift.WorkPlaceTitle);
        //                                    dictionary.Add("TimeShiftTitle", timeShift.Title);
        //                                    dictionary.Add("LastConsecutiveDay", dayOffs[i - consecuriveCounter] - 1 + "/" + timeShift.Month + "/" + timeShift.Year);
        //                                    dictionary.Add("ConsecutiveDays", consecuriveCounter);
        //                                    returnObjects.Add(expandoObj);
        //                                }
        //                                else
        //                                {
        //                                    var expandoObj = new ExpandoObject();
        //                                    var dictionary = (IDictionary<string, object>)expandoObj;

        //                                    dictionary.Add("FirstName", employee.FirstName);
        //                                    dictionary.Add("LastName", employee.LastName);
        //                                    dictionary.Add("VatNumber", employee.VatNumber);
        //                                    dictionary.Add("WorkPlaceTitle", timeShift.WorkPlaceTitle);
        //                                    dictionary.Add("TimeShiftTitle", timeShift.Title);
        //                                    dictionary.Add("LastConsecutiveDay", "Ξεκινάει το χρονοδιάγραμμα με ρεπό");
        //                                    dictionary.Add("ConsecutiveDays", consecuriveCounter);
        //                                    returnObjects.Add(expandoObj);
        //                                }
        //                            }
        //                            consecuriveCounter = 1;
        //                        }
        //                    }

        //                if (consecuriveCounter >= _datatable.FilterByConsecutiveDayOff_Min && consecuriveCounter <= _datatable.FilterByConsecutiveDayOff_Max)
        //                {
        //                    if (dayOffs[dayOffs.Count - consecuriveCounter] - 1 > 0)
        //                    {

        //                        var expandoObj = new ExpandoObject();
        //                        var dictionary = (IDictionary<string, object>)expandoObj;

        //                        dictionary.Add("FirstName", employee.FirstName);
        //                        dictionary.Add("LastName", employee.LastName);
        //                        dictionary.Add("VatNumber", employee.VatNumber);
        //                        dictionary.Add("WorkPlaceTitle", timeShift.WorkPlaceTitle);
        //                        dictionary.Add("TimeShiftTitle", timeShift.Title);
        //                        dictionary.Add("LastConsecutiveDay", dayOffs[dayOffs.Count - consecuriveCounter] - 1 + "/" + timeShift.Month + "/" + timeShift.Year);
        //                        dictionary.Add("ConsecutiveDays", consecuriveCounter);
        //                        returnObjects.Add(expandoObj);
        //                    }
        //                    else
        //                    {
        //                        var expandoObj = new ExpandoObject();
        //                        var dictionary = (IDictionary<string, object>)expandoObj;

        //                        dictionary.Add("FirstName", employee.FirstName);
        //                        dictionary.Add("LastName", employee.LastName);
        //                        dictionary.Add("VatNumber", employee.VatNumber);
        //                        dictionary.Add("WorkPlaceTitle", timeShift.WorkPlaceTitle);
        //                        dictionary.Add("TimeShiftTitle", timeShift.Title);
        //                        dictionary.Add("LastConsecutiveDay", "Δεν έχει εργαστεί στο χρονοδιάγραμμα");
        //                        dictionary.Add("ConsecutiveDays", consecuriveCounter);
        //                        returnObjects.Add(expandoObj);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var entities = await _baseDatawork.Employees
        //            .GetWithFilterQueryable(x => x.OrderBy(y => y.LastName), _filter, includes)
        //            .Select(x => new
        //            {
        //                Id = x.Id,
        //                FirstName = x.FirstName,
        //                LastName = x.LastName,
        //                VatNumber = x.VatNumber,
        //                TimeShifts = x.EmployeeWorkPlaces
        //                    .SelectMany(y => y.WorkPlace.TimeShifts)
        //                    .Select(y => new
        //                    {
        //                        y.Id,
        //                        y.Title,
        //                        y.Month,
        //                        y.Year,
        //                        y.DaysInMonth,
        //                        WorkPlaceTitle = y.WorkPlace.Title
        //                    }),
        //                RealWorkHours = x.RealWorkHours
        //                    .Where(y => _datatable.StartOn.Date <= y.StartOn.Date && y.StartOn.Date <= _datatable.EndOn.Date)
        //                    //.Where(y => x.HireDate.Date <= y.StartOn.Date)
        //                    .Select(y => new
        //                    {
        //                        Day = y.StartOn.Day,
        //                        TimeShiftId = y.TimeShiftId
        //                    })
        //            }).ToListAsync();

        //        foreach (var employee in entities)
        //        {
        //            foreach (var timeShift in employee.TimeShifts)
        //            {

        //                var workingDays = employee.RealWorkHours
        //                    .Where(x => x.TimeShiftId == timeShift.Id)
        //                    .Select(x => x.Day)
        //                    .ToList()
        //                    .Distinct();

        //                var dayOffs =
        //                   Enumerable.Range(1, timeShift.DaysInMonth).ToList()
        //                   .Except(workingDays)
        //                   .OrderBy(x => x)
        //                   .ToList();

        //                var consecuriveCounter = 1;
        //                if (dayOffs.Count > 1)
        //                    for (int i = 1; i < dayOffs.Count; i++)
        //                    {
        //                        if (dayOffs[i] == dayOffs[i - 1] + 1)
        //                            consecuriveCounter++;
        //                        else
        //                        {
        //                            if (consecuriveCounter >= _datatable.FilterByConsecutiveDayOff_Min && consecuriveCounter <= _datatable.FilterByConsecutiveDayOff_Max)
        //                            {
        //                                if (dayOffs[i - consecuriveCounter] - 1 > 0)
        //                                {
        //                                    var expandoObj = new ExpandoObject();
        //                                    var dictionary = (IDictionary<string, object>)expandoObj;

        //                                    dictionary.Add("FirstName", employee.FirstName);
        //                                    dictionary.Add("LastName", employee.LastName);
        //                                    dictionary.Add("VatNumber", employee.VatNumber);
        //                                    dictionary.Add("WorkPlaceTitle", timeShift.WorkPlaceTitle);
        //                                    dictionary.Add("TimeShiftTitle", timeShift.Title);
        //                                    dictionary.Add("LastConsecutiveDay", dayOffs[i - consecuriveCounter] - 1 + "/" + timeShift.Month + "/" + timeShift.Year);
        //                                    dictionary.Add("ConsecutiveDays", consecuriveCounter);
        //                                    returnObjects.Add(expandoObj);
        //                                }
        //                                else
        //                                {
        //                                    var expandoObj = new ExpandoObject();
        //                                    var dictionary = (IDictionary<string, object>)expandoObj;

        //                                    dictionary.Add("FirstName", employee.FirstName);
        //                                    dictionary.Add("LastName", employee.LastName);
        //                                    dictionary.Add("VatNumber", employee.VatNumber);
        //                                    dictionary.Add("WorkPlaceTitle", timeShift.WorkPlaceTitle);
        //                                    dictionary.Add("TimeShiftTitle", timeShift.Title);
        //                                    dictionary.Add("LastConsecutiveDay", "-");
        //                                    dictionary.Add("ConsecutiveDays", consecuriveCounter);
        //                                    returnObjects.Add(expandoObj);
        //                                }
        //                            }
        //                            consecuriveCounter = 1;
        //                        }
        //                    }



        //                if (consecuriveCounter >= _datatable.FilterByConsecutiveDayOff_Min && consecuriveCounter <= _datatable.FilterByConsecutiveDayOff_Max)
        //                {
        //                    if (dayOffs[dayOffs.Count - consecuriveCounter] - 1 > 0)
        //                    {

        //                        var expandoObj = new ExpandoObject();
        //                        var dictionary = (IDictionary<string, object>)expandoObj;

        //                        dictionary.Add("FirstName", employee.FirstName);
        //                        dictionary.Add("LastName", employee.LastName);
        //                        dictionary.Add("VatNumber", employee.VatNumber);
        //                        dictionary.Add("WorkPlaceTitle", timeShift.WorkPlaceTitle);
        //                        dictionary.Add("TimeShiftTitle", timeShift.Title);
        //                        dictionary.Add("LastConsecutiveDay", dayOffs[dayOffs.Count - consecuriveCounter] - 1 + "/" + timeShift.Month + "/" + timeShift.Year);
        //                        dictionary.Add("ConsecutiveDays", consecuriveCounter);
        //                        returnObjects.Add(expandoObj);
        //                    }
        //                    else
        //                    {
        //                        var expandoObj = new ExpandoObject();
        //                        var dictionary = (IDictionary<string, object>)expandoObj;

        //                        dictionary.Add("FirstName", employee.FirstName);
        //                        dictionary.Add("LastName", employee.LastName);
        //                        dictionary.Add("VatNumber", employee.VatNumber);
        //                        dictionary.Add("WorkPlaceTitle", timeShift.WorkPlaceTitle);
        //                        dictionary.Add("TimeShiftTitle", timeShift.Title);
        //                        dictionary.Add("LastConsecutiveDay", "Δεν έχει εργαστεί στο χρονοδιάγραμμα");
        //                        dictionary.Add("ConsecutiveDays", consecuriveCounter);
        //                        returnObjects.Add(expandoObj);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    EntitiesMapped = returnObjects;
        //    EntitiesTotal = 0;

        //    return this;
        //}




        public async Task<ProjectionDataTableWorker> ProjectionErganiSuggestions()
        {

            //set search dates of start to earliest Monday and set end to latest of Sunday
            var originalStartOn = _datatable.StartOn.Date;
            var originalEndOn = _datatable.EndOn.Date;
            //If Sunday (DayOfWeek=0)
            if (_datatable.StartOn.DayOfWeek.GetHashCode() == 0)
                _datatable.StartOn = _datatable.StartOn.AddDays(-1);
            else
                _datatable.StartOn = _datatable.StartOn.AddDays(1 - _datatable.StartOn.DayOfWeek.GetHashCode());

            _datatable.EndOn = _datatable.EndOn.AddDays(7 - _datatable.EndOn.DayOfWeek.GetHashCode());

            var sum = 0.0;
            var includes = new List<Func<IQueryable<WorkHour>, IIncludableQueryable<WorkHour, object>>>();
            includes.Add(x => x.Include(y => y.Employee).ThenInclude(y => y.RealWorkHours).ThenInclude(y => y.TimeShift).ThenInclude(y => y.WorkPlace));
            includes.Add(x => x.Include(y => y.Employee).ThenInclude(y => y.Contract));

            var filter = PredicateBuilder.New<WorkHour>();
            filter = filter.And(GetSearchFilterWorkHour());
            filter = filter.And(x => _datatable.StartOn.Date <= x.StartOn.Date && x.StartOn.Date <= _datatable.EndOn.Date);

            if (_datatable.FilterByWorkPlaceId != 0)
                filter = filter.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

            var entities = await _baseDatawork.WorkHours
               .GetWithFilterQueryable(x => x.OrderBy(y => y.StartOn), filter, includes, _pageSize, _pageIndex)
               .Select(x => new WorkHour
               {
                   Id = x.Id,
                   StartOn = x.StartOn,
                   EndOn = x.EndOn,
                   Employee = new Employee
                   {
                       Id = x.EmployeeId,
                       FirstName = x.Employee.FirstName,
                       LastName = x.Employee.LastName,
                       VatNumber = x.Employee.VatNumber,
                       Contract = new Contract
                       {
                           DayOfDaysPerWeek = x.Employee.Contract != null ? x.Employee.Contract.DayOfDaysPerWeek : 0,
                           WorkingDaysPerWeek = x.Employee.Contract != null ? x.Employee.Contract.WorkingDaysPerWeek : 0,
                           HoursPerWeek = x.Employee.Contract != null ? x.Employee.Contract.HoursPerWeek : 0,
                           HoursPerDay = x.Employee.Contract != null ? x.Employee.Contract.HoursPerDay : 0
                       }
                   },
                   TimeShift = new TimeShift
                   {
                       WorkPlace = new WorkPlace
                       {
                           Title = x.TimeShift.WorkPlace.Title
                       }
                   }
               })
               .ToListAsync();

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();


            //OverTime
            if (_datatable.FilterByValidateOvertime)
            {
                var entitiesFiltered = entities
                    .Where(x => originalStartOn <= x.StartOn.Date && x.StartOn.Date <= originalEndOn)
                    .ToList();

                var overTimeEntities = entitiesFiltered
                    .Where(x => entitiesFiltered
                        .Where(y => y.Id != x.Id)
                        .Where(y => y.Employee.Id == x.Employee.Id)
                        .Any(y => x.EndOn.AddHours(11) >= y.StartOn && x.StartOn.AddHours(-11) <= y.EndOn)).ToList();

                foreach (var result in overTimeEntities)
                {
                    var expandoObj = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expandoObj;

                    dictionary.Add("Id", result.Employee.Id);
                    dictionary.Add("FirstName", result.Employee.FirstName);
                    dictionary.Add("LastName", result.Employee.LastName);
                    dictionary.Add("VatNumber", result.Employee.VatNumber);
                    dictionary.Add("WorkHour", dataTableHelper.GetProjectionTimeShiftSuggestionCellBody(result));
                    dictionary.Add("Title", result.TimeShift.WorkPlace.Title);
                    dictionary.Add("Error", $"Βάρδια με λιγότερο απο 11 ώρες διαφορά {result.StartOn.ToShortDateString()}");

                    returnObjects.Add(expandoObj);
                }
            }

            //Contract DayOff per week
            if (_datatable.FilterByValidateDayOfDaysPerWeek)
            {

                var dayOfDaysPerWeekEntities = entities
                    .GroupBy(x => new
                    {
                        Week = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetWeekOfYear(x.StartOn, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                        Year = x.StartOn.Year,
                        x.EmployeeId,
                        x.Employee.FirstName,
                        x.Employee.LastName,
                        x.Employee.VatNumber,
                        x.TimeShift.WorkPlace.Title,
                        x.Employee.Contract.DayOfDaysPerWeek
                    })
                    .Select(x => new
                    {
                        x.Key.EmployeeId,
                        x.Key.FirstName,
                        x.Key.LastName,
                        x.Key.VatNumber,
                        x.Key.Week,
                        x.Key.Title,
                        x.Key.DayOfDaysPerWeek,
                        WorkingDays = x.Select(y => y.StartOn.Day).Distinct().Count()
                    })
                    //.Where(x => x.DayOfDaysPerWeek != null)
                    .Where(x => (7 - x.WorkingDays) != x.DayOfDaysPerWeek)
                    .Select(x => new
                    {
                        x.EmployeeId,
                        x.FirstName,
                        x.LastName,
                        x.Title,
                        x.VatNumber,
                        Error = $"Η εβδομάδα {x.Week} ξεπερνάει το όριο 'Ρεπό ανα εβδομάδα' με όριο '{x.DayOfDaysPerWeek}" +
                        $"' με ρεπό'{7 - x.WorkingDays }'"
                    })
                    .ToList();

                foreach (var result in dayOfDaysPerWeekEntities)
                {
                    var expandoObj = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expandoObj;

                    dictionary.Add("Id", result.EmployeeId);
                    dictionary.Add("FirstName", result.FirstName);
                    dictionary.Add("LastName", result.LastName);
                    dictionary.Add("VatNumber", result.VatNumber);
                    dictionary.Add("WorkHour", "-");
                    dictionary.Add("Title", result.Title);
                    dictionary.Add("Error", result.Error);

                    returnObjects.Add(expandoObj);
                }
            }

            //Contract WorkDays per week 
            if (_datatable.FilterByValidateWorkDaysPerWeek)
            {

                var workingDaysPerWeekEntities = entities
                    .GroupBy(x => new
                    {
                        Week = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetWeekOfYear(x.StartOn, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                        Year = x.StartOn.Year,
                        x.EmployeeId,
                        x.Employee.FirstName,
                        x.Employee.LastName,
                        x.TimeShift.WorkPlace.Title,
                        x.Employee.VatNumber,
                        x.Employee.Contract.WorkingDaysPerWeek
                    })
                    .Select(x => new
                    {
                        x.Key.EmployeeId,
                        x.Key.FirstName,
                        x.Key.LastName,
                        x.Key.VatNumber,
                        x.Key.Week,
                        x.Key.Title,
                        x.Key.WorkingDaysPerWeek,
                        WorkingDays = x.Select(y => y.StartOn.Day).Distinct().Count(),
                    })
                    .Where(x => x.WorkingDays != x.WorkingDaysPerWeek)
                    .Select(x => new
                    {
                        x.EmployeeId,
                        x.FirstName,
                        x.LastName,
                        x.VatNumber,
                        x.Title,
                        Error = $"Η εβδομάδα {x.Week} ξεπερνάει το όριο 'Εργ. ημ. ανα εβδομάδα' με όριο '{x.WorkingDaysPerWeek}" +
                            $"' με ημ. εργασίας'{x.WorkingDays }'"
                    })
                    .ToList();

                foreach (var result in workingDaysPerWeekEntities)
                {
                    var expandoObj = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expandoObj;

                    dictionary.Add("Id", result.EmployeeId);
                    dictionary.Add("FirstName", result.FirstName);
                    dictionary.Add("LastName", result.LastName);
                    dictionary.Add("VatNumber", result.VatNumber);
                    dictionary.Add("WorkHour", "-");
                    dictionary.Add("Title", result.Title);
                    dictionary.Add("Error", result.Error);

                    returnObjects.Add(expandoObj);
                }
            }

            //Contract Hours per week
            if (_datatable.FilterByValidateHoursPerWeek)
            {
                var hoursPerWeekEntities = entities
                    .GroupBy(x => new
                    {
                        Week = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetWeekOfYear(x.StartOn, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                        Year = x.StartOn.Year,
                        x.EmployeeId,
                        x.Employee.FirstName,
                        x.Employee.LastName,
                        x.TimeShift.WorkPlace.Title,
                        x.Employee.VatNumber,
                        x.Employee.Contract?.HoursPerWeek
                    })
                    .Select(x => new
                    {
                        x.Key.EmployeeId,
                        x.Key.FirstName,
                        x.Key.LastName,
                        x.Key.VatNumber,
                        x.Key.Week,
                        x.Key.Title,
                        x.Key.HoursPerWeek,
                        WorkingHours = x.Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToTotalWork().TotalHours).Sum(),
                    })
                    .Where(x => (decimal)x.WorkingHours != x.HoursPerWeek)
                    .Select(x => new
                    {
                        x.EmployeeId,
                        x.FirstName,
                        x.LastName,
                        x.Title,
                        x.VatNumber,
                        Error = $"Η εβδομάδα {x.Week} ξεπερνάει το όριο 'Ώρες ανα εβδομάδα' με όριο '{x.HoursPerWeek}" +
                        $"' με ώρες εργασίας'{x.WorkingHours }'"
                    })
                    .ToList();

                foreach (var result in hoursPerWeekEntities)
                {
                    var expandoObj = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expandoObj;

                    dictionary.Add("Id", result.EmployeeId);
                    dictionary.Add("FirstName", result.FirstName);
                    dictionary.Add("LastName", result.LastName);
                    dictionary.Add("VatNumber", result.VatNumber);
                    dictionary.Add("WorkHour", "-");
                    dictionary.Add("Title", result.Title);
                    dictionary.Add("Error", result.Error);

                    returnObjects.Add(expandoObj);
                }
            }

            //Contract Hours per day
            if (_datatable.FilterByValidateWorkingHoursPerDay)
            {
                var hoursPerDayEntities = entities
                    .Where(x => originalStartOn <= x.StartOn.Date && x.StartOn.Date <= originalEndOn)
                    .GroupBy(x => new
                    {
                        Day = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetDayOfYear(x.StartOn),
                        x.EmployeeId,
                        x.Employee.FirstName,
                        x.Employee.LastName,
                        x.TimeShift.WorkPlace.Title,
                        x.Employee.VatNumber,
                        x.Employee.Contract?.HoursPerDay
                    })
                    .Select(x => new
                    {
                        x.Key.Day,
                        x.Key.EmployeeId,
                        x.Key.FirstName,
                        x.Key.LastName,
                        x.Key.VatNumber,
                        x.Key.Title,
                        x.Key.HoursPerDay,
                        WorkingHours = (decimal)x.Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToTotalWork().TotalHours).Sum(),
                    })
                    .Where(x => x.WorkingHours != x.HoursPerDay)
                    .Select(x => new
                    {
                        x.Day,
                        x.EmployeeId,
                        x.FirstName,
                        x.LastName,
                        x.VatNumber,
                        x.Title,
                        x.WorkingHours,
                        Error = $"Η ημέρα {new DateTime(_datatable.StartOn.Year, 1, 1).AddDays(x.Day).ToShortDateString() } ξεπερνάει το όριο 'Ώρες ανα ημέρα' με όριο '{x.HoursPerDay}" +
                        $"' με ώρες εργασίας'{x.WorkingHours.ToString() }'"
                    })
                    .ToList();

                var employees = new List<Employee>();
                if (_datatable.FilterByWorkPlaceId == 0)
                    employees.AddRange(_baseDatawork.Employees.Query
                        .Include(x => x.Contract)
                        .Where(x => x.ContractId != null)
                        .Select(x => new Employee
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            VatNumber = x.VatNumber,
                            Contract = new Contract
                            {
                                HoursPerDay = x.Contract.HoursPerDay
                            }
                        })
                        .ToList());
                else
                    employees.AddRange(_baseDatawork.Employees.Query
                        .Include(x => x.Contract)
                        .Where(x => x.EmployeeWorkPlaces.Any(y => y.WorkPlaceId == _datatable.FilterByWorkPlaceId))
                        .Where(x => x.ContractId != null)
                        .Select(x => new Employee
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            VatNumber = x.VatNumber,
                            Contract = new Contract
                            {
                                HoursPerDay = x.Contract.HoursPerDay
                            }
                        })
                        .ToList());

                foreach (var result in hoursPerDayEntities)
                {
                    var expandoObj = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expandoObj;

                    dictionary.Add("Id", result.EmployeeId);
                    dictionary.Add("FirstName", result.FirstName);
                    dictionary.Add("LastName", result.LastName);
                    dictionary.Add("VatNumber", result.VatNumber);
                    dictionary.Add("WorkHour", "-");
                    dictionary.Add("Title", result.Title);
                    dictionary.Add("Error", result.Error);

                    returnObjects.Add(expandoObj);
                }
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }


        public async Task<ProjectionDataTableWorker> ProjectionTimeShiftSuggestions()
        {

            //set search dates of start to earliest Monday and set end to latest of Sunday
            var originalStartOn = _datatable.StartOn.Date;
            var originalEndOn = _datatable.EndOn.Date;
            //If Sunday (DayOfWeek=0)
            if (_datatable.StartOn.DayOfWeek.GetHashCode() == 0)
                _datatable.StartOn = _datatable.StartOn.AddDays(-1);
            else
                _datatable.StartOn = _datatable.StartOn.AddDays(1 - _datatable.StartOn.DayOfWeek.GetHashCode());

            _datatable.EndOn = _datatable.EndOn.AddDays(7 - _datatable.EndOn.DayOfWeek.GetHashCode());

            var sum = 0.0;
            var includes = new List<Func<IQueryable<WorkHour>, IIncludableQueryable<WorkHour, object>>>();
            includes.Add(x => x.Include(y => y.Employee).ThenInclude(y => y.RealWorkHours).ThenInclude(y => y.TimeShift).ThenInclude(y => y.WorkPlace));
            includes.Add(x => x.Include(y => y.Employee).ThenInclude(y => y.Contract));

            var filter = PredicateBuilder.New<WorkHour>();
            filter = filter.And(GetSearchFilterWorkHour());
            filter = filter.And(x => _datatable.StartOn.Date <= x.StartOn.Date && x.StartOn.Date <= _datatable.EndOn.Date);

            if (_datatable.FilterByWorkPlaceId != 0)
                filter = filter.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

            var entities = await _baseDatawork.WorkHours
               .GetWithFilterQueryable(x => x.OrderBy(y => y.StartOn), filter, includes, _pageSize, _pageIndex)
               .Select(x => new WorkHour
               {
                   Id = x.Id,
                   StartOn = x.StartOn,
                   EndOn = x.EndOn,
                   Employee = new Employee
                   {
                       Id = x.EmployeeId,
                       FirstName = x.Employee.FirstName,
                       LastName = x.Employee.LastName,
                       VatNumber = x.Employee.VatNumber,
                       Contract = new Contract
                       {
                           DayOfDaysPerWeek = x.Employee.Contract != null ? x.Employee.Contract.DayOfDaysPerWeek : 0,
                           WorkingDaysPerWeek = x.Employee.Contract != null ? x.Employee.Contract.WorkingDaysPerWeek : 0,
                           HoursPerWeek = x.Employee.Contract != null ? x.Employee.Contract.HoursPerWeek : 0,
                           HoursPerDay = x.Employee.Contract != null ? x.Employee.Contract.HoursPerDay : 0
                       }
                   },
                   TimeShift = new TimeShift
                   {
                       WorkPlace = new WorkPlace
                       {
                           Title = x.TimeShift.WorkPlace.Title
                       }
                   }
               })
               .ToListAsync();

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();


            //OverTime
            if (_datatable.FilterByValidateOvertime)
            {
                var entitiesFiltered = entities
                    .Where(x => originalStartOn <= x.StartOn.Date && x.StartOn.Date <= originalEndOn)
                    .ToList();

                var overTimeEntities = entitiesFiltered
                    .Where(x => entitiesFiltered
                        .Where(y => y.Id != x.Id)
                        .Where(y => y.Employee.Id == x.Employee.Id)
                        .Any(y => x.EndOn.AddHours(11) >= y.StartOn && x.StartOn.AddHours(-11) <= y.EndOn)).ToList();

                foreach (var result in overTimeEntities)
                {
                    var expandoObj = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expandoObj;

                    dictionary.Add("Id", result.Employee.Id);
                    dictionary.Add("FirstName", result.Employee.FirstName);
                    dictionary.Add("LastName", result.Employee.LastName);
                    dictionary.Add("VatNumber", result.Employee.VatNumber);
                    dictionary.Add("WorkHour", dataTableHelper.GetProjectionTimeShiftSuggestionCellBody(result));
                    dictionary.Add("Title", result.TimeShift.WorkPlace.Title);
                    dictionary.Add("Error", $"Βάρδια με λιγότερο απο 11 ώρες διαφορά {result.StartOn.ToShortDateString()}");

                    returnObjects.Add(expandoObj);
                }
            }

            //Contract DayOff per week
            if (_datatable.FilterByValidateDayOfDaysPerWeek)
            {

                var dayOfDaysPerWeekEntities = entities
                    .GroupBy(x => new
                    {
                        Week = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetWeekOfYear(x.StartOn, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                        Year = x.StartOn.Year,
                        x.EmployeeId,
                        x.Employee.FirstName,
                        x.Employee.LastName,
                        x.Employee.VatNumber,
                        x.TimeShift.WorkPlace.Title,
                        x.Employee.Contract.DayOfDaysPerWeek
                    })
                    .Select(x => new
                    {
                        x.Key.EmployeeId,
                        x.Key.FirstName,
                        x.Key.LastName,
                        x.Key.VatNumber,
                        x.Key.Week,
                        x.Key.Title,
                        x.Key.DayOfDaysPerWeek,
                        WorkingDays = x.Select(y => y.StartOn.Day).Distinct().Count()
                    })
                    //.Where(x => x.DayOfDaysPerWeek != null)
                    .Where(x => (7 - x.WorkingDays) < x.DayOfDaysPerWeek)
                    .Select(x => new
                    {
                        x.EmployeeId,
                        x.FirstName,
                        x.LastName,
                        x.Title,
                        x.VatNumber,
                        Error = $"Η εβδομάδα {x.Week} ξεπερνάει το όριο 'Ρεπό ανα εβδομάδα' με όριο '{x.DayOfDaysPerWeek}" +
                        $"' με ρεπό'{7 - x.WorkingDays }'"
                    })
                    .ToList();

                foreach (var result in dayOfDaysPerWeekEntities)
                {
                    var expandoObj = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expandoObj;

                    dictionary.Add("Id", result.EmployeeId);
                    dictionary.Add("FirstName", result.FirstName);
                    dictionary.Add("LastName", result.LastName);
                    dictionary.Add("VatNumber", result.VatNumber);
                    dictionary.Add("WorkHour", "-");
                    dictionary.Add("Title", result.Title);
                    dictionary.Add("Error", result.Error);

                    returnObjects.Add(expandoObj);
                }
            }

            //Contract WorkDays per week 
            if (_datatable.FilterByValidateWorkDaysPerWeek)
            {

                var workingDaysPerWeekEntities = entities
                    .GroupBy(x => new
                    {
                        Week = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetWeekOfYear(x.StartOn, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                        Year = x.StartOn.Year,
                        x.EmployeeId,
                        x.Employee.FirstName,
                        x.Employee.LastName,
                        x.TimeShift.WorkPlace.Title,
                        x.Employee.VatNumber,
                        x.Employee.Contract.WorkingDaysPerWeek
                    })
                    .Select(x => new
                    {
                        x.Key.EmployeeId,
                        x.Key.FirstName,
                        x.Key.LastName,
                        x.Key.VatNumber,
                        x.Key.Week,
                        x.Key.Title,
                        x.Key.WorkingDaysPerWeek,
                        WorkingDays = x.Select(y => y.StartOn.Day).Distinct().Count(),
                    })
                    .Where(x => x.WorkingDays > x.WorkingDaysPerWeek)
                    .Select(x => new
                    {
                        x.EmployeeId,
                        x.FirstName,
                        x.LastName,
                        x.VatNumber,
                        x.Title,
                        Error = $"Η εβδομάδα {x.Week} ξεπερνάει το όριο 'Εργ. ημ. ανα εβδομάδα' με όριο '{x.WorkingDaysPerWeek}" +
                            $"' με ημ. εργασίας'{x.WorkingDays }'"
                    })
                    .ToList();

                foreach (var result in workingDaysPerWeekEntities)
                {
                    var expandoObj = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expandoObj;

                    dictionary.Add("Id", result.EmployeeId);
                    dictionary.Add("FirstName", result.FirstName);
                    dictionary.Add("LastName", result.LastName);
                    dictionary.Add("VatNumber", result.VatNumber);
                    dictionary.Add("WorkHour", "-");
                    dictionary.Add("Title", result.Title);
                    dictionary.Add("Error", result.Error);

                    returnObjects.Add(expandoObj);
                }
            }

            //Contract Hours per week
            if (_datatable.FilterByValidateHoursPerWeek)
            {
                var hoursPerWeekEntities = entities
                    .GroupBy(x => new
                    {
                        Week = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetWeekOfYear(x.StartOn, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday),
                        Year = x.StartOn.Year,
                        x.EmployeeId,
                        x.Employee.FirstName,
                        x.Employee.LastName,
                        x.TimeShift.WorkPlace.Title,
                        x.Employee.VatNumber,
                        x.Employee.Contract?.HoursPerWeek
                    })
                    .Select(x => new
                    {
                        x.Key.EmployeeId,
                        x.Key.FirstName,
                        x.Key.LastName,
                        x.Key.VatNumber,
                        x.Key.Week,
                        x.Key.Title,
                        x.Key.HoursPerWeek,
                        WorkingHours = x.Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToTotalWork().TotalHours).Sum(),
                    })
                    .Where(x => (decimal)x.WorkingHours > x.HoursPerWeek)
                    .Select(x => new
                    {
                        x.EmployeeId,
                        x.FirstName,
                        x.LastName,
                        x.Title,
                        x.VatNumber,
                        Error = $"Η εβδομάδα {x.Week} ξεπερνάει το όριο 'Ώρες ανα εβδομάδα' με όριο '{x.HoursPerWeek}" +
                        $"' με ώρες εργασίας'{x.WorkingHours }'"
                    })
                    .ToList();

                foreach (var result in hoursPerWeekEntities)
                {
                    var expandoObj = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expandoObj;

                    dictionary.Add("Id", result.EmployeeId);
                    dictionary.Add("FirstName", result.FirstName);
                    dictionary.Add("LastName", result.LastName);
                    dictionary.Add("VatNumber", result.VatNumber);
                    dictionary.Add("WorkHour", "-");
                    dictionary.Add("Title", result.Title);
                    dictionary.Add("Error", result.Error);

                    returnObjects.Add(expandoObj);
                }
            }

            //Contract Hours per day
            if (_datatable.FilterByValidateWorkingHoursPerDay)
            {
                var hoursPerDayEntities = entities
                    .Where(x => originalStartOn <= x.StartOn.Date && x.StartOn.Date <= originalEndOn)
                    .GroupBy(x => new
                    {
                        Day = CultureInfo.CurrentCulture.DateTimeFormat.Calendar.GetDayOfYear(x.StartOn),
                        x.EmployeeId,
                        x.Employee.FirstName,
                        x.Employee.LastName,
                        x.TimeShift.WorkPlace.Title,
                        x.Employee.VatNumber,
                        x.Employee.Contract?.HoursPerDay
                    })
                    .Select(x => new
                    {
                        x.Key.Day,
                        x.Key.EmployeeId,
                        x.Key.FirstName,
                        x.Key.LastName,
                        x.Key.VatNumber,
                        x.Key.Title,
                        x.Key.HoursPerDay,
                        WorkingHours = (decimal)x.Select(y => new DateRangeService(y.StartOn, y.EndOn).ConvertToTotalWork().TotalHours).Sum(),
                    })
                    .Where(x => x.WorkingHours > x.HoursPerDay)
                    .Select(x => new
                    {
                        x.Day,
                        x.EmployeeId,
                        x.FirstName,
                        x.LastName,
                        x.VatNumber,
                        x.Title,
                        x.WorkingHours,
                        Error = $"Η ημέρα {new DateTime(_datatable.StartOn.Year, 1, 1).AddDays(x.Day).ToShortDateString() } ξεπερνάει το όριο 'Ώρες ανα ημέρα' με όριο '{x.HoursPerDay}" +
                        $"' με ώρες εργασίας'{x.WorkingHours.ToString() }'"
                    })
                    .ToList();

                var employees = new List<Employee>();
                if (_datatable.FilterByWorkPlaceId == 0)
                    employees.AddRange(_baseDatawork.Employees.Query
                        .Include(x => x.Contract)
                        .Where(x => x.ContractId != null)
                        .Select(x => new Employee
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            VatNumber = x.VatNumber,
                            Contract = new Contract
                            {
                                HoursPerDay = x.Contract.HoursPerDay
                            }
                        })
                        .ToList());
                else
                    employees.AddRange(_baseDatawork.Employees.Query
                        .Include(x => x.Contract)
                        .Where(x => x.EmployeeWorkPlaces.Any(y => y.WorkPlaceId == _datatable.FilterByWorkPlaceId))
                        .Where(x => x.ContractId != null)
                        .Select(x => new Employee
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            VatNumber = x.VatNumber,
                            Contract = new Contract
                            {
                                HoursPerDay = x.Contract.HoursPerDay
                            }
                        })
                        .ToList());

                //if (_datatable.FilterByOffsetMin == 0 || _datatable.FilterByOffsetMin == null)
                //{

                //    var searchDaysDiff = originalEndOn.Date.Subtract(originalStartOn.Date).Days;
                //    foreach (var employee in employees)
                //    {
                //        for (int i = 0; i <= searchDaysDiff; i++)
                //        {
                //            var currentSearchDate = originalStartOn.Date.AddDays(i).Date;
                //            var currentSearchDateDay = CultureInfo.CurrentCulture.DateTimeFormat
                //                .Calendar.GetDayOfYear(currentSearchDate);

                //            var dateHasWorkHour = hoursPerDayEntities
                //                .Any(x => x.Day == currentSearchDateDay
                //                        && x.EmployeeId == employee.Id);

                //            if (!dateHasWorkHour)
                //                hoursPerDayEntities.Add(new
                //                {
                //                    Day = currentSearchDateDay,
                //                    EmployeeId = employee.Id,
                //                    FirstName = employee.FirstName,
                //                    LastName = employee.LastName,
                //                    VatNumber = employee.VatNumber,
                //                    HoursPerDay_Max = (_datatable.FilterByOffsetMax == null) ? decimal.MaxValue: (decimal)_datatable.FilterByOffsetMax,
                //                    HoursPerDay_Min = (_datatable.FilterByOffsetMin == null) ? 0.0m : (decimal)_datatable.FilterByOffsetMin,
                //                    WorkingHours = 0.0M,
                //                    Error = $"Η ημέρα {currentSearchDateDay} ξεπερνάει το όριο 'Ώρες ανα ημέρα' με όριο '{employee.Contract.HoursPerDay.ToString()}' με ώρες εργασίας'{0}'"
                //                });

                //        }
                //    }
                //}

                foreach (var result in hoursPerDayEntities)
                {
                    var expandoObj = new ExpandoObject();
                    var dictionary = (IDictionary<string, object>)expandoObj;

                    dictionary.Add("Id", result.EmployeeId);
                    dictionary.Add("FirstName", result.FirstName);
                    dictionary.Add("LastName", result.LastName);
                    dictionary.Add("VatNumber", result.VatNumber);
                    dictionary.Add("WorkHour", "-");
                    dictionary.Add("Title", result.Title);
                    dictionary.Add("Error", result.Error);

                    returnObjects.Add(expandoObj);
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

                var filter = PredicateBuilder.New<RealWorkHour>();
                filter = filter.And(GetSearchFilterRealWorkHour());


                filter = filter.And(x => x.Comments.Length > 1);

                if (_datatable.FilterByWorkPlaceId != 0)
                    filter = filter.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

                if (_datatable.FilterByEmployeeId != 0)
                    filter = filter.And(x => x.EmployeeId == _datatable.FilterByEmployeeId);

                if (_datatable.StartOn != null && _datatable.EndOn != null)
                    filter = filter.And(x => x.EndOn >= _datatable.StartOn && x.StartOn <= _datatable.EndOn);


                var entities = await _baseDatawork.RealWorkHours
                   .GetPaggingWithFilter(x => x.OrderBy(x => x.StartOn), filter, realWorkHourIncludes, _pageSize, _pageIndex);

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
                EntitiesTotal = await _baseDatawork.RealWorkHours.CountAllAsyncFiltered(filter);

            }
            else
            {
                var workHourIncludes = new List<Func<IQueryable<WorkHour>, IIncludableQueryable<WorkHour, object>>>();

                workHourIncludes.Add(x => x.Include(y => y.TimeShift).ThenInclude(y => y.WorkPlace));
                workHourIncludes.Add(x => x.Include(y => y.Employee));

                var filter = PredicateBuilder.New<WorkHour>();

                filter = filter.And(GetSearchFilterWorkHour());
                filter = filter.And(x => x.Comments.Length > 1);
                if (_datatable.FilterByWorkPlaceId != 0)
                    filter = filter.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

                if (_datatable.FilterByEmployeeId != 0)
                    filter = filter.And(x => x.EmployeeId == _datatable.FilterByEmployeeId);

                if (_datatable.StartOn != null && _datatable.EndOn != null)
                    filter = filter.And(x => x.EndOn >= _datatable.StartOn && x.StartOn <= _datatable.EndOn);

                var entities = await _baseDatawork.WorkHours
                   .GetPaggingWithFilter(x => x.OrderBy(x => x.StartOn), filter, workHourIncludes, _pageSize, _pageIndex);

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
                EntitiesTotal = await _baseDatawork.WorkHours.CountAllAsyncFiltered(filter);

            }
            return this;
        }

        public async Task<ProjectionDataTableWorker> ProjectionRealWorkHourRestrictions()
        {
            var filter = PredicateBuilder.New<RealWorkHour>();
            var includes = new List<Func<IQueryable<RealWorkHour>, IIncludableQueryable<RealWorkHour, object>>>();

            filter = filter.And(GetSearchFilterRealWorkHour());
            filter = filter.And(x => _datatable.StartOn <= x.StartOn && x.StartOn <= _datatable.EndOn);
            filter = filter.And(x => x.TimeShift.WorkPlace.WorkPlaceHourRestrictions.Where(y => y.Month == x.StartOn.Month && y.Year == x.StartOn.Year).Any());

            if (_datatable.FilterByWorkPlaceId != 0)
                filter = filter.And(x => x.TimeShift.WorkPlaceId == _datatable.FilterByWorkPlaceId);

            var entities = (await _baseDatawork.RealWorkHours
                .GetWithFilterQueryable(x => x.OrderBy(y => y.StartOn), filter, includes)
                .Select(x => new
                {
                    x.StartOn,
                    x.EndOn,
                    x.TimeShift.WorkPlace.Title,
                    x.TimeShift.WorkPlace.WorkPlaceHourRestrictions
                        .FirstOrDefault(y => y.Month == x.StartOn.Month && y.Year == x.StartOn.Year)
                        .HourRestrictions.FirstOrDefault(y => y.Day == x.StartOn.Day)
                        .MaxTicks,
                }).ToListAsync())
                .GroupBy(x => new
                {
                    x.StartOn.Date,
                    x.Title,
                    x.MaxTicks
                })
                .Select(x => new
                {
                    x.Key.Date,
                    x.Key.Title,
                    MaxPermitedSeconds = x.Key.MaxTicks,
                    TotalDaySeconds = x.Select(y => new DateRangeService(y.StartOn, y.EndOn.Value).ConvertToTotalWork().TotalSeconds)
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

                    dictionary.Add("WorkPlaceTitle", result.Title);
                    dictionary.Add("DateError", result.Date.ToShortDateString());
                    dictionary.Add("Error", $"Μέγιστο όριο ημέρας: {GetTime(result.MaxPermitedSeconds) } Εισήχθησαν: {GetTime(result.TotalDaySeconds) }");

                    returnObjects.Add(expandoObj);
                }

            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.RealWorkHours.CountAllAsyncFiltered(filter);

            return this;
        }

        private static string GetTime(double? seconds)
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
