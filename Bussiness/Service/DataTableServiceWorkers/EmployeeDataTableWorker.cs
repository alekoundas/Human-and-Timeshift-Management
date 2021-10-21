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
    public class EmployeeDataTableWorker : DataTableService
    {
        private BaseDatawork _baseDatawork { get; }
        private SecurityDataWork _securityDataWork { get; }
        private new Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<Employee> _filter { get; set; }
            = PredicateBuilder.New<Employee>();


        public EmployeeDataTableWorker(
            Datatable datatable,
            BaseDatawork baseDatawork,
            SecurityDataWork securityDataWork,
            HttpContext httpContext)
            : base(datatable, baseDatawork, httpContext, securityDataWork)
        {
            _datatable = datatable;
            _httpContext = httpContext;
            _baseDatawork = baseDatawork;
            _securityDataWork = securityDataWork;

            //Filters for everyone
            _filter = _filter.And(GetSearchFilter());
            _filter = _filter.And(GetDataTableFilter());

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
            else if (_columnName == "WorkPlaceTitle")
                return x => x.OrderBy(y => y.EmployeeWorkPlaces.OrderBy(z => z.WorkPlace.Title));

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
                    if (column.Data == "VatNumber")
                        filter = filter.Or(x => x.VatNumber.Contains(_datatable.Search.Value));
                    if (column.Data == "SocialSecurityNumber")
                        filter = filter.Or(x => x.SocialSecurityNumber.Contains(_datatable.Search.Value));
                    if (column.Data == "ErpCode")
                        filter = filter.Or(x => x.ErpCode.Contains(_datatable.Search.Value));
                    if (column.Data == "Address")
                        filter = filter.Or(x => x.Address.Contains(_datatable.Search.Value));
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

        private Expression<Func<Employee, bool>> GetDataTableFilter()
        {
            var filter = PredicateBuilder.New<Employee>();
            if (_datatable.FilterByTimeShiftId != 0)
            {
                filter = filter.And(x => x.EmployeeWorkPlaces
                 .Any(y => y.WorkPlace.TimeShifts
                     .Any(z => z.Id == _datatable.FilterByTimeShiftId)) ||
                     x.WorkHours.Any(y => y.TimeShiftId == _datatable.FilterByTimeShiftId));
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

            //var entities = await _baseDatawork.Employees
            //  .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);
            var entities = await _baseDatawork.Employees
                .GetWithFilterQueryable(SetOrderBy(), _filter, includes, _pageSize, _pageIndex)
                .Select(x => new
                {
                    x.Id,
                    x.FirstName,
                    x.LastName,
                    x.ErpCode,
                    x.VatNumber,
                    x.IsActive,
                    SpecializationName = x.Specialization.Name,
                    CompanyTitle = x.Company.Title
                })
                .ToListAsync();

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {

                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("LastName", result.LastName);
                dictionary.Add("FirstName", result.FirstName);
                dictionary.Add("ErpCode", result.ErpCode);
                dictionary.Add("VatNumber", result.VatNumber);
                dictionary.Add("IsActive", result.IsActive);
                dictionary.Add("SpecializationName", result.SpecializationName);

                //if (result.Company != null)
                //{
                //    result.Company.Employees = null;
                //    dictionary.Add("CompanyTitle", result.CompanyTitle);
                //}

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
                    result.Company.Employees = null;
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
                    result.Company.Employees = null;
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
            includes.Add(x => x.Include(y => y.EmployeeWorkPlaces));

            if (!_datatable.FilterByIncludedEmployees)
                _filter = _filter.And(x => x.EmployeeWorkPlaces.Any(y => y.WorkPlaceId == _datatable.GenericId));

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
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                var apiUrl = UrlHelper.EmployeeWorkPlace(result.Id, _datatable.GenericId);

                dictionary.Add("Id", result.Id);
                dictionary.Add("FirstName", result.FirstName);
                dictionary.Add("LastName", result.LastName);
                dictionary.Add("ErpCode", result.ErpCode);
                dictionary.Add("VatNumber", result.VatNumber);
                dictionary.Add("IsActive", result.IsActive);
                dictionary.Add("CompanyTitle", result?.Company.Title);

                if (result.EmployeeWorkPlaces.Any(x => x.EmployeeId == result.Id &&
                    x.WorkPlaceId == _datatable.GenericId))
                {
                    dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                        "Employee", apiUrl, "checked"));
                }
                else
                    dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                        "Employee", apiUrl, ""));

                dictionary.Add("SpecializationName", result?.Specialization?.Name);

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
            includes.Add(x => x.Include(y => y.EmployeeWorkPlaces));

            if (!_datatable.FilterByIncludedEmployees)
                _filter = _filter.And(x => x.EmployeeWorkPlaces.Any(y => y.WorkPlaceId == _datatable.GenericId));

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
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                var apiUrl = UrlHelper.EmployeeWorkPlace(result.Id, _datatable.GenericId);

                dictionary.Add("Id", result.Id);
                dictionary.Add("FirstName", result.FirstName);
                dictionary.Add("LastName", result.LastName);
                dictionary.Add("ErpCode", result.ErpCode);
                dictionary.Add("VatNumber", result.VatNumber);
                dictionary.Add("IsActive", result.IsActive);
                dictionary.Add("CompanyTitle", result?.Company.Title);


                if (result.EmployeeWorkPlaces.Any(x => x.EmployeeId == result.Id &&
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
            includes.Add(x => x.Include(y => y.WorkHours));
            includes.Add(x => x.Include(y => y.Leaves));

            var entities = await _baseDatawork.Employees
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();
            var autoIncrementNumber = 0;

            foreach (var result in entities)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("FirstName", result.FirstName);
                dictionary.Add("LastName", result.LastName);
                dictionary.Add("ErpCode", result.ErpCode);
                dictionary.Add("IsActive", result.IsActive);

                if (_datatable.MakePrintable)
                    dictionary.Add("AutoIncrementNumber", ++autoIncrementNumber);

                for (int i = 0; i < DateTime.DaysInMonth(_datatable.TimeShiftYear, _datatable.TimeShiftMonth); i++)
                    dictionary.Add("Day_" + i, dataTableHelper
                        .GetTimeShiftEditCellBodyWorkHours(i + 1, _datatable, result));

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
            includes.Add(x => x.Include(y => y.WorkHours));
            includes.Add(x => x.Include(y => y.Leaves));

            var entities = await _baseDatawork.Employees
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();
            var autoIncrementNumber = 0;
            foreach (var result in entities)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("FirstName", result.FirstName);
                dictionary.Add("LastName", result.LastName);
                dictionary.Add("ErpCode", result.ErpCode);
                dictionary.Add("IsActive", result.IsActive);

                if (_datatable.MakePrintable)
                    dictionary.Add("AutoIncrementNumber", ++autoIncrementNumber);

                for (int i = 0; i < DateTime.DaysInMonth(_datatable.TimeShiftYear, _datatable.TimeShiftMonth); i++)
                    dictionary.Add("Day_" + i, dataTableHelper.GetTimeShiftEditCellBodyWorkHours
                        (i + 1, _datatable, result));

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
            includes.Add(x => x.Include(y => y.RealWorkHours));
            includes.Add(x => x.Include(y => y.Leaves));

            //if (_datatable.GenericId != 0)
            //    _filter = _filter.And(x => (x.EmployeeWorkPlaces
            //        .Any(y => y.WorkPlace.TimeShifts.Any(z => z.Id == _datatable.GenericId))));
            if (_datatable.GenericId != 0)
                _filter = _filter.And(x => x.RealWorkHours.Any(y => y.TimeShiftId == _datatable.GenericId) || x.EmployeeWorkPlaces.Any(y => y.WorkPlace.TimeShifts.Any(z => z.Id == _datatable.GenericId)));

            var entities = await _baseDatawork.Employees
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);


            var compareMonth = 0;
            var compareYear = 0;
            if (_datatable.SelectedMonth == null || _datatable.SelectedYear == null)
            {
                //Extra needed data
                var timeshift = await _baseDatawork.TimeShifts
                    .FirstOrDefaultAsync(x => x.Id == _datatable.GenericId);

                compareMonth = timeshift.Month;
                compareYear = timeshift.Year;
            }
            else
            {
                compareMonth = (int)_datatable.SelectedMonth;
                compareYear = (int)_datatable.SelectedYear;
            }

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;
                var daysinmonth = DateTime.DaysInMonth(compareYear, compareMonth);

                dictionary.Add("FirstName", result.FirstName);
                dictionary.Add("LastName", result.LastName);
                dictionary.Add("ErpCode", result.ErpCode);
                dictionary.Add("IsActive", result.IsActive);

                for (int i = 0; i <= DateTime.DaysInMonth(compareYear, compareMonth); i++)
                    dictionary.Add("Day_" + i,
                         dataTableHelper.GetTimeShiftEditCellBodyRealWorkHours(
                             compareMonth, compareYear, i + 1, _datatable, result));

                dictionary.Add("ToggleSlider", dataTableHelper
                    .GetEmployeeCheckbox(_datatable, result.Id));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<EmployeeDataTableWorker> TimeShiftAmendment()
        {
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.RealWorkHours).ThenInclude(y=>y.Amendment));
            includes.Add(x => x.Include(y => y.RealWorkHours).ThenInclude(y=>y.TimeShift));


            var entities = await _baseDatawork.Employees
                .GetWithFilterQueryable(SetOrderBy(), null, includes, _pageSize, _pageIndex)
                .Select(x => new Employee
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    ErpCode = x.ErpCode,
                    IsActive = x.IsActive,
                    RealWorkHours = x.RealWorkHours
                        //.Where(y => y.TimeShiftId == _datatable.FilterByTimeShiftId)
                        .Select(y => new RealWorkHour
                        {
                            Id = y.Id,
                            StartOn = y.StartOn,
                            EndOn = y.EndOn,
                            TimeShift = new TimeShift
                            {
                                Month = y.TimeShift.Month,
                                Year = y.TimeShift.Year

                            },
                            AmendmentId = y.AmendmentId,
                            Amendment = y.Amendment
                        }).ToList(),
                    Amendments = x.Amendments
                        .Where(y=>y.RealWorkHourId==null)
                        .ToList()
                }).ToListAsync();

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();
            var autoIncrementNumber = 0;
            foreach (var result in entities)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;
                var timeshift = result.RealWorkHours.Select(x => x.TimeShift).FirstOrDefault();
                if (timeshift != null)
                {


                    dictionary.Add("FirstName", result.FirstName);
                    dictionary.Add("LastName", result.LastName);
                    dictionary.Add("ErpCode", result.ErpCode);
                    dictionary.Add("IsActive", result.IsActive);



                    for (int i = 0; i < DateTime.DaysInMonth(timeshift.Year, timeshift.Month); i++)
                        dictionary.Add("Day_" + i, dataTableHelper
                            .GetAmendmentCellBodyWorkHours(i + 1, result));

                    dictionary.Add("ToggleSlider", dataTableHelper
                        .GetEmployeeCheckbox(_datatable, result.Id));

                    returnObjects.Add(expandoObj);
                }
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<EmployeeDataTableWorker> RealWorkHourClockIn()
        {
            var clockInState = "none";
            var includes = new List<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>();
            includes.Add(x => x.Include(y => y.RealWorkHours));
            includes.Add(x => x.Include(y => y.Leaves));

            //_filter = _filter.Or(x => x.RealWorkHours.Any(y => y.IsInProgress == true));

            var loggedInUserId = HttpAccessorService.GetLoggeInUser_Id;
            if (loggedInUserId == null)
            {
                _filter = _filter.And(x => false);
            }
            else
            {
                var user = _securityDataWork.ApplicationUsers.Get(loggedInUserId);
                if (user.IsEmployee)
                {

                    _filter = _filter.And(x => x.Id == user.EmployeeId);
                    var userIsInProgres = _baseDatawork.Employees
                        .Any(x => x.Id == user.EmployeeId && x.RealWorkHours.Any(y => y.IsInProgress));
                    if (userIsInProgres)
                        clockInState = "true";
                    else
                        clockInState = "false";
                }
            }

            if (_datatable.GenericId != 0)
                _filter = _filter.And(x => x.EmployeeWorkPlaces
                    .Any(y => y.WorkPlace.TimeShifts.Any(z => z.Id == _datatable.GenericId)));

            var entities = await _baseDatawork.Employees
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);



            var compareMonth = 0;
            var compareYear = 0;
            if (_datatable.SelectedMonth == null || _datatable.SelectedYear == null)
            {
                //Extra needed data
                var timeshift = await _baseDatawork.TimeShifts
                    .FirstOrDefaultAsync(x => x.Id == _datatable.GenericId);

                compareMonth = timeshift.Month;
                compareYear = timeshift.Year;
            }
            else
            {
                compareMonth = (int)_datatable.SelectedMonth;
                compareYear = (int)_datatable.SelectedYear;
            }

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<Employee>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = new ExpandoObject();
                var dictionary = (IDictionary<string, object>)expandoObj;
                var daysinmonth = DateTime.DaysInMonth(compareYear, compareMonth);

                dictionary.Add("FirstName", result.FirstName);
                dictionary.Add("LastName", result.LastName);
                dictionary.Add("IsActive", result.IsActive);
                dictionary.Add("ClockInState", clockInState);

                dictionary.Add("Day",
                     dataTableHelper.GetTimeShiftEditCellBodyRealWorkHours(
                         compareMonth, compareYear, DateTime.Now.Day, _datatable, result));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.Employees.CountAllAsyncFiltered(_filter);

            return this;
        }



    }
}
