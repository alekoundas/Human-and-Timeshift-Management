using Bussiness.Helpers;
using DataAccess;
using DataAccess.Models.Datatable;
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
    public class EmployeeDataTableWorker : DataTableService
    {
        private BaseDatawork _baseDatawork { get; }
        private new Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<Employee> _filter { get; set; }
            = PredicateBuilder.New<Employee>();


        public EmployeeDataTableWorker(Datatable datatable, BaseDatawork baseDatawork, HttpContext httpContext)
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
            if (_columnName == "WorkHourDate")
                return x => x.OrderBy(y => y.WorkHours.OrderBy(z => z.StartOn));
            else if (_columnName == "RealWorkHourDate")
                return x => x.OrderBy(y => y.RealWorkHours.OrderBy(z => z.StartOn));
            else if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<Employee, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<Employee>();
            if (_datatable.Search.Value != null)
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "FirstName")
                        filter = filter.Or(x => x.FirstName.Contains(_datatable.Search.Value));
                    if (column.Data == "LastName")
                        filter = filter.Or(x => x.LastName.Contains(_datatable.Search.Value));
                    if (column.Data == "DateOfBirth")
                        filter = filter.Or(x => x.DateOfBirth.ToString().Contains(_datatable.Search.Value));
                    if (column.Data == "Afm")
                        filter = filter.Or(x => x.Afm.Contains(_datatable.Search.Value));
                    if (column.Data == "SocialSecurityNumber")
                        filter = filter.Or(x => x.SocialSecurityNumber.Contains(_datatable.Search.Value));
                    if (column.Data == "ErpCode")
                        filter = filter.Or(x => x.ErpCode.Contains(_datatable.Search.Value));
                    if (column.Data == "Address")
                        filter = filter.Or(x => x.Address.Contains(_datatable.Search.Value));
                    if (column.Data == "Email")
                        filter = filter.Or(x => x.Email.Contains(_datatable.Search.Value));
                    if (column.Data == "SpecializationName")
                        filter = filter.Or(x => x.Specialization.Name.Contains(_datatable.Search.Value));
                    if (column.Data == "CompanyTitle")
                        filter = filter.Or(x => x.Company.Title.Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }



        public async Task<EmployeeDataTableWorker> EmployeeIndex()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.Specialization));
            includes.Add(x => x.Include(y => y.Company));

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

                dictionary.Add("SpecializationName", result.Specialization?.Name);

                if (result.Company != null)
                    dictionary.Add("CompanyTitle", result.Company.Title);

                dictionary.Add("Buttons", dataTableHelper
                    .GetButtons("Employee", "Employees", result.Id.ToString()));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }


        public async Task<EmployeeDataTableWorker> CompanyEdit()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.Specialization));
            includes.Add(x => x.Include(y => y.Company));

            _filter = _filter.And(x =>
                x.CompanyId == _datatable.GenericId || x.CompanyId == null);

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

                var apiUrl = UrlHelper.EmployeeCompany(result.Id, _datatable.GenericId);

                if (result.Company != null)
                {
                    dictionary.Add("CompanyTitle", result.Company.Title);
                    dictionary.Add("IsInCompany", dataTableHelper.GetToggle(
                        "Employee", apiUrl, "checked"));
                }
                else
                    dictionary.Add("IsInCompany", dataTableHelper.GetToggle(
                        "Employee", apiUrl, ""));

                dictionary.Add("SpecializationName", result.Specialization?.Name);

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<EmployeeDataTableWorker> CompanyDetail()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.Specialization));
            includes.Add(x => x.Include(y => y.Company));

            _filter = _filter.And(x =>
                x.CompanyId == _datatable.GenericId || x.CompanyId == null);

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

                var apiUrl = UrlHelper.EmployeeCompany(result.Id, _datatable.GenericId);

                if (result.Company != null)
                {
                    dictionary.Add("CompanyTitle", result.Company.Title);
                    dictionary.Add("IsInCompany", dataTableHelper.GetToggle(
                        "Employee", apiUrl, "checked", true));
                }
                else
                    dictionary.Add("IsInCompany", dataTableHelper.GetToggle(
                        "Employee", apiUrl, "", true));

                dictionary.Add("SpecializationName", result.Specialization?.Name);

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<EmployeeDataTableWorker> WorkPlaceEdit()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.Specialization));
            includes.Add(x => x.Include(y => y.Company));

            _filter = _filter.And(x => x.Company.Customers
                .Any(y => y.WorkPlaces
                .Any(z => z.Id == _datatable.GenericId)));

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

                var apiUrl = UrlHelper.EmployeeWorkPlace(result.Id, _datatable.GenericId);

                if (result.Company != null)
                    dictionary.Add("CompanyTitle", result.Company.Title);

                if (_baseDatawork.EmployeeWorkPlaces
                    .Any(x => x.EmployeeId == result.Id &&
                    x.WorkPlaceId == _datatable.GenericId))
                {
                    dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                        "Employee", apiUrl, "checked"));
                }
                else
                    dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                        "Employee", apiUrl, ""));

                dictionary.Add("SpecializationName", result.Specialization?.Name);

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<EmployeeDataTableWorker> WorkPlaceDetail()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.Specialization));
            includes.Add(x => x.Include(y => y.Company));

            _filter = _filter.And(x => x.Company.Customers
                .Any(y => y.WorkPlaces
                .Any(z => z.Id == _datatable.GenericId)));

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

                var apiUrl = UrlHelper.EmployeeWorkPlace(result.Id, _datatable.GenericId);

                if (result.Company != null)
                    dictionary.Add("CompanyTitle", result.Company.Title);

                if (_baseDatawork.EmployeeWorkPlaces
                    .Any(x => x.EmployeeId == result.Id &&
                    x.WorkPlaceId == _datatable.GenericId))
                {
                    dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                        "Employee", apiUrl, "checked", true));
                }
                else
                    dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                        "Employee", apiUrl, "", true));

                dictionary.Add("SpecializationName", result.Specialization?.Name);

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<EmployeeDataTableWorker> TimeShiftDetail()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();


            _filter = _filter.And(x => x.EmployeeWorkPlaces
                .Any(y => y.WorkPlace.TimeShifts.Any(z => z.Id == _datatable.GenericId)));

            var entities = await _baseDatawork.Employees
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);
            //Extra needed data

            var workHours = await _baseDatawork.WorkHours.Where(x =>
                     x.TimeShiftId == _datatable.GenericId &&
                     x.StartOn.Year == _datatable.TimeShiftYear &&
                     x.StartOn.Month == _datatable.TimeShiftMonth)
                 .ToDynamicListAsync<WorkHour>();

            var leaves = await _baseDatawork.Leaves.Where(x =>
                   x.StartOn.Year == _datatable.TimeShiftYear &&
                   x.StartOn.Month == _datatable.TimeShiftMonth)
               .ToDynamicListAsync<Leave>();

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Employee>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                for (int i = 1; i <= DateTime.DaysInMonth(_datatable.TimeShiftYear, _datatable.TimeShiftMonth); i++)
                    dictionary.Add("Day" + i, dataTableHelper.GetTimeShiftEditCellBodyWorkHours
                        (workHours, leaves, i, _datatable, result.Id));

                dictionary.Add("ToggleSlider", dataTableHelper
                    .GetEmployeeCheckbox(_datatable, result.Id));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }
        public async Task<EmployeeDataTableWorker> TimeShiftEdit()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();

            _filter = _filter.And(x => x.EmployeeWorkPlaces
                .Any(y => y.WorkPlace.TimeShifts.Any(z => z.Id == _datatable.GenericId)));

            var entities = await _baseDatawork.Employees
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);
            //Extra needed data

            var workHours = await _baseDatawork.WorkHours.Where(x =>
                     x.TimeShiftId == _datatable.GenericId &&
                     x.StartOn.Year == _datatable.TimeShiftYear &&
                     x.StartOn.Month == _datatable.TimeShiftMonth)
                 .ToDynamicListAsync<WorkHour>();

            var leaves = await _baseDatawork.Leaves.Where(x =>
                   x.StartOn.Year == _datatable.TimeShiftYear &&
                   x.StartOn.Month == _datatable.TimeShiftMonth)
               .ToDynamicListAsync<Leave>();

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Employee>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                for (int i = 1; i <= DateTime.DaysInMonth(_datatable.TimeShiftYear, _datatable.TimeShiftMonth); i++)
                    dictionary.Add("Day" + i, dataTableHelper.GetTimeShiftEditCellBodyWorkHours
                        (workHours, leaves, i, _datatable, result.Id));

                dictionary.Add("ToggleSlider", dataTableHelper
                    .GetEmployeeCheckbox(_datatable, result.Id));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<EmployeeDataTableWorker> RealWorkHourIndex()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();

            if (_datatable.GenericId != 0)
                _filter = _filter.And(x => x.EmployeeWorkPlaces
                    .Any(y => y.WorkPlace.TimeShifts.Any(z => z.Id == _datatable.GenericId)));

            var entities = await _baseDatawork.Employees
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Extra needed data
            var timeshifts = await _baseDatawork.TimeShifts
                .Where(x => x.Id == _datatable.GenericId)
                .ToDynamicListAsync<TimeShift>();

            var compareMonth = 0;
            var compareYear = 0;
            if (_datatable.SelectedMonth == null || _datatable.SelectedYear == null)
            {
                compareMonth = timeshifts[0].Month;
                compareYear = timeshifts[0].Year;
            }
            else
            {
                compareMonth = (int)_datatable.SelectedMonth;
                compareYear = (int)_datatable.SelectedYear;
            }

            var workHours = await _baseDatawork.WorkHours.Where(x =>
                     x.TimeShiftId == _datatable.GenericId &&
                     x.StartOn.Year == compareYear &&
                     x.StartOn.Month == compareMonth)
                 .ToDynamicListAsync<WorkHour>();

            var leaves = await _baseDatawork.Leaves.Where(x =>
                     x.StartOn.Year == compareYear &&
                     x.StartOn.Month == compareMonth)
                .ToDynamicListAsync<Leave>();

            var realWorkHours = await _baseDatawork.RealWorkHours.Where(x =>
                    x.StartOn.Year == compareYear &&
                    x.TimeShiftId == _datatable.GenericId &&
                    x.StartOn.Month == compareMonth)
            .ToDynamicListAsync<RealWorkHour>();

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Employee>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                for (int i = 1; i <= DateTime.DaysInMonth(compareYear, compareMonth); i++)
                    dictionary.Add("Day" + i,
                         dataTableHelper.GetTimeShiftEditCellBodyRealWorkHours(
                             realWorkHours, workHours, leaves, compareMonth, compareYear, i, _datatable, result.Id));

                dictionary.Add("ToggleSlider", dataTableHelper
                    .GetEmployeeCheckbox(_datatable, result.Id));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<EmployeeDataTableWorker> RealWorkHourCurrentDay()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.RealWorkHours));


            var today = DateTime.Now;
            _filter = _filter.And(x => x.RealWorkHours
                .Any(y => y.StartOn.Date == today.Date));

            var entities = await _baseDatawork.Employees
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            entities = entities
                .Select(x => new Employee
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    RealWorkHours = x.RealWorkHours.Where(y => y.StartOn.Day == DateTime.Now.Day).ToList()
                })
                .ToList();

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Employee>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("StartOn", String.Join<string>("</br>",
                    result.RealWorkHours.Select(x => x.StartOn.ToString())));

                dictionary.Add("EndOn", String.Join<string>("</br>",
                    result.RealWorkHours.Select(x => x.EndOn.ToString())));

                dictionary.Add("ToggleSlider", dataTableHelper
                    .GetEmployeeCheckbox(_datatable, result.Id));

                dictionary.Add("Buttons", dataTableHelper
                    .GetCurrentDayButtons(result));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<EmployeeDataTableWorker> ProjectionDifference()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();

            var entities = await _baseDatawork.Employees
                .ProjectionDifference(SetOrderBy(), _datatable, _filter, _pageSize, _pageIndex);

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

        public async Task<EmployeeDataTableWorker> ProjectionConcentric()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();

            if (_datatable.GenericId != 0)
                _filter = _filter.And(x => x.EmployeeWorkPlaces
                    .Any(y => y.WorkPlaceId == _datatable.GenericId));

            var entities = await _baseDatawork.Employees
                .ProjectionDifference(SetOrderBy(), _datatable, _filter, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Employee>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                var totalSeconds = await _baseDatawork.RealWorkHours
                        .GetEmployeeTotalSecondsFromRange(result.Id, _datatable.StartOn, _datatable.EndOn);

                var totalSecondsDay = await _baseDatawork.RealWorkHours
                       .GetEmployeeTotalSecondsDayFromRange(result.Id, _datatable.StartOn, _datatable.EndOn);

                var totalSecondsNight = await _baseDatawork.RealWorkHours
                        .GetEmployeeTotalSecondsNightFromRange(result.Id, _datatable.StartOn, _datatable.EndOn);

                var totalSecondsSaturdayDay = await _baseDatawork.RealWorkHours
                        .GetEmployeeTotalSecondsSaturdayDayFromRange(result.Id, _datatable.StartOn, _datatable.EndOn);

                var totalSecondsSaturdayNight = await _baseDatawork.RealWorkHours
                        .GetEmployeeTotalSecondsSaturdayNightFromRange(result.Id, _datatable.StartOn, _datatable.EndOn);

                var totalSecondsSundayDay = await _baseDatawork.RealWorkHours
                        .GetEmployeeTotalSecondsSundayDayFromRange(result.Id, _datatable.StartOn, _datatable.EndOn);

                var totalSecondsSundayNight = await _baseDatawork.RealWorkHours
                        .GetEmployeeTotalSecondsSundayNightFromRange(result.Id, _datatable.StartOn, _datatable.EndOn);

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


        public async Task<EmployeeDataTableWorker> ProjectionRealWorkHoursAnalytically()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();

            if (_datatable.GenericId != 0)
                _filter = _filter.And(x => x.EmployeeWorkPlaces
                    .Any(y => y.WorkPlaceId == _datatable.GenericId));

            var entities = await _baseDatawork.Employees
                .ProjectionDifference(SetOrderBy(), _datatable, _filter, _pageSize, _pageIndex);

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
                    dictionary.Add("Day_0", await dataTableHelper
                           .GetProjectionRealWorkHoursAnalyticallyCellBodyAsync(
                           _baseDatawork, _datatable.StartOn, _datatable, result.Id));
                }
                else
                    for (int i = 0; i <= (_datatable.EndOn.Date - _datatable.StartOn.Date).TotalDays; i++)
                    {
                        var compareDate = new DateTime(_datatable.StartOn.AddDays(i).Ticks);
                        dictionary.Add("Day_" + i, await dataTableHelper
                            .GetProjectionRealWorkHoursAnalyticallyCellBodyAsync(
                            _baseDatawork, compareDate, _datatable, result.Id));
                    }
                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }


        public async Task<EmployeeDataTableWorker> ProjectionRealWorkHoursAnalyticallySum()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();

            if (_datatable.GenericId != 0)
                _filter = _filter.And(x => x.EmployeeWorkPlaces
                    .Any(y => y.WorkPlaceId == _datatable.GenericId));

            var entities = await _baseDatawork.Employees
                .ProjectionDifference(SetOrderBy(), _datatable, _filter, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Employee>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                var totalSeconds = await _baseDatawork.RealWorkHours
                    .GetEmployeeTotalSecondsFromRange(result.Id, _datatable.StartOn, _datatable.EndOn);


                if ((_datatable.EndOn.Date - _datatable.StartOn.Date).TotalDays == 0.0)
                {
                    var daySeconds = await _baseDatawork.RealWorkHours
                             .GetEmployeeTotalSecondsForDay(result.Id, _datatable.StartOn);

                    var nightSeconds = await _baseDatawork.RealWorkHours
                        .GetEmployeeTotalSecondsForNight(result.Id, _datatable.StartOn);

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
                        var daySeconds = await _baseDatawork.RealWorkHours
                               .GetEmployeeTotalSecondsForDay(result.Id, compareDate);

                        var nightSeconds = await _baseDatawork.RealWorkHours
                            .GetEmployeeTotalSecondsForNight(result.Id, compareDate);
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

        public async Task<EmployeeDataTableWorker> ProjectionPresenceDaily()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();

            if (_datatable.GenericId != 0)
                _filter = _filter.And(x => x.EmployeeWorkPlaces
                    .Any(y => y.WorkPlaceId == _datatable.GenericId));

            var entities = await _baseDatawork.Employees
                .ProjectionDifference(SetOrderBy(), _datatable, _filter, _pageSize, _pageIndex);
            //Extra needed data

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<Employee>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                var realWorkHours = await _baseDatawork.RealWorkHours
                               .GetCurrentAssignedOnCell(DateTime.Now, result.Id);
                var todayCell = "";
                foreach (var realWorkHour in realWorkHours)
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


        public async Task<EmployeeDataTableWorker> ProjectionRealWorkHoursSpecific()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();

            if (_datatable.GenericId != 0)
                _filter = _filter.And(x => x.EmployeeWorkPlaces
                    .Any(y => y.WorkPlaceId == _datatable.GenericId));

            var entities = await _baseDatawork.Employees
                .ProjectionDifference(SetOrderBy(), _datatable, _filter, _pageSize, _pageIndex);

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
                    var realWorkHours = await _baseDatawork.RealWorkHours
                      .GetCurrentAssignedOnCell(_datatable.SpecificDates[i], result.Id);

                    var hasLeave = _baseDatawork.Leaves
                        .Where(x => x.EmployeeId == result.Id)
                        .Any(x => _datatable.SpecificDates[i].Date >= x.StartOn.Date && _datatable.SpecificDates[i].Date <= x.EndOn.Date);


                    var dayOffs = await _baseDatawork.WorkHours
                      .GetCurrentDayOffAssignedOnCell(_datatable.SpecificDates[i], result.Id);

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
    }
}
