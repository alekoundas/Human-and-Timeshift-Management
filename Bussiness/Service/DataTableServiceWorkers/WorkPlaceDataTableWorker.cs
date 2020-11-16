using Bussiness.Helpers;
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
    public class WorkPlaceDataTableWorker : DataTableService
    {
        private BaseDatawork _baseDatawork { get; }
        private Datatable _datatable { get; }
        private HttpContext _httpContext { get; }

        public List<ExpandoObject> EntitiesMapped { get; set; }
        public int EntitiesTotal { get; set; }
        private ExpressionStarter<WorkPlace> _filter { get; set; }
            = PredicateBuilder.New<WorkPlace>();


        public WorkPlaceDataTableWorker(Datatable datatable, BaseDatawork baseDatawork, HttpContext httpContext)
            : base(datatable, baseDatawork, httpContext)
        {
            _datatable = datatable;
            _httpContext = httpContext;
            _baseDatawork = baseDatawork;

            //Filters for everyone
            _filter = _filter.And(GetSearchFilter());
            _filter = _filter.And(GetUserRoleFiltersAsync());
            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<WorkPlace>(_httpContext);

            if (!canShowDeactivated)
                _filter = _filter.And(x => x.IsActive == true);

        }

        private Func<IQueryable<WorkPlace>, IOrderedQueryable<WorkPlace>> SetOrderBy()
        {
            if (_columnName != "")
                return x => x.OrderBy(_columnName + " " + _orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<WorkPlace, bool>> GetSearchFilter()
        {
            var filter = PredicateBuilder.New<WorkPlace>();
            if (_datatable.Search.Value != null)
            {
                foreach (var column in _datatable.Columns)
                {
                    if (column.Data == "Title")
                        filter = filter.Or(x => x.Title.Contains(_datatable.Search.Value));
                    if (column.Data == "Description")
                        filter = filter.Or(x => x.Description.Contains(_datatable.Search.Value));
                    if (column.Data == "ΙdentifyingΝame")
                        filter = filter.Or(x => x.Customer.ΙdentifyingΝame.Contains(_datatable.Search.Value));
                    if (column.Data == "Customer.company.title")
                        filter = filter.Or(x => x.Customer.Company.Title.Contains(_datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);
            return filter;
        }

        private Expression<Func<WorkPlace, bool>> GetUserRoleFiltersAsync()
        {
            //Get WorkPlaceId from user roles
            var workPlaceIds = _httpContext.User.Claims
                .Where(x => x.Value.Contains("Specific_WorkPlace"))
                .Select(y => Int32.Parse(y.Value.Split("_")[2]));

            var filter = PredicateBuilder.New<WorkPlace>();
            foreach (var workPlaceId in workPlaceIds)
                filter = filter.Or(x => x.Id == workPlaceId);

            if (workPlaceIds.Count() == 0)
                filter = filter.And(x => true);

            return filter;
        }

        public async Task<WorkPlaceDataTableWorker> WorkPlaceIndex()
        {
            var includes = new List<Func<IQueryable<WorkPlace>, IIncludableQueryable<WorkPlace, object>>>();
            includes.Add(x => x.Include(y => y.Customer));

            var entities = await _baseDatawork.WorkPlaces
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlace>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<WorkPlace>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("Buttons", dataTableHelper
                    .GetButtons("WorkPlace", "WorkPlaces", result.Id.ToString()));
                dictionary.Add("ΙdentifyingΝame", result.Customer?.ΙdentifyingΝame);

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.WorkPlaces.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<WorkPlaceDataTableWorker> CustomerDetails()
        {
            var includes = new List<Func<IQueryable<WorkPlace>, IIncludableQueryable<WorkPlace, object>>>();
            includes.Add(x => x.Include(y => y.Customer));

            var entities = await _baseDatawork.WorkPlaces
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlace>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<WorkPlace>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                var apiUrl = UrlHelper.CustomerWorkPlace(_datatable.GenericId, result.Id);

                if (result.Customer?.Id == _datatable.GenericId)
                    dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                        "WorkPlace", apiUrl, "checked", true));
                else
                    dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                        "WorkPlace", apiUrl, "", true));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.WorkPlaces.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<WorkPlaceDataTableWorker> CustomerEdit()
        {
            var includes = new List<Func<IQueryable<WorkPlace>, IIncludableQueryable<WorkPlace, object>>>();
            includes.Add(x => x.Include(y => y.Customer));

            var entities = await _baseDatawork.WorkPlaces
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlace>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<WorkPlace>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                var apiUrl = UrlHelper.CustomerWorkPlace(_datatable.GenericId, result.Id);

                if (result.Customer?.Id == _datatable.GenericId)
                    dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                        "WorkPlace", apiUrl, "checked"));
                else
                    dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                        "WorkPlace", apiUrl, ""));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.WorkPlaces.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<WorkPlaceDataTableWorker> EmployeeEdit()
        {
            var includes = new List<Func<IQueryable<WorkPlace>, IIncludableQueryable<WorkPlace, object>>>();
            includes.Add(x => x.Include(y => y.Customer));
            includes.Add(x => x.Include(y => y.EmployeeWorkPlaces).ThenInclude(z => z.Employee));

            _filter = _filter.And(x => x.Customer.Company != null);

            var entities = await _baseDatawork.WorkPlaces
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            entities = entities.Select(X => new WorkPlace
            {
                CustomerId = X.CustomerId,
                Title = X.Title,
                EmployeeWorkPlaces = X.EmployeeWorkPlaces.Select(y => new EmployeeWorkPlace
                {
                    Id = y.Id,
                    WorkPlaceId = y.WorkPlaceId,
                    EmployeeId = y.EmployeeId
                }).ToList()
            }).ToList();

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlace>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<WorkPlace>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                var apiUrl = UrlHelper.EmployeeWorkPlace(_datatable.GenericId, result.Id);

                dictionary.Add("ΙdentifyingΝame", result.Customer?.ΙdentifyingΝame);

                if (result.EmployeeWorkPlaces.Any(x => x.EmployeeId == _datatable.GenericId))
                    dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                        "WorkPlace", apiUrl, "checked"));
                else
                    dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                        "WorkPlace", apiUrl, ""));
                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.WorkPlaces.CountAllAsyncFiltered(_filter);

            return this;
        }

        public async Task<WorkPlaceDataTableWorker> EmployeeDetails()
        {
            var includes = new List<Func<IQueryable<WorkPlace>, IIncludableQueryable<WorkPlace, object>>>();
            includes.Add(x => x.Include(y => y.Customer));
            includes.Add(x => x.Include(y => y.EmployeeWorkPlaces).ThenInclude(z => z.Employee));

            _filter = _filter.And(x => x.Customer.Company != null);

            var entities = await _baseDatawork.WorkPlaces
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlace>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<WorkPlace>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                var apiUrl = UrlHelper.EmployeeWorkPlace(_datatable.GenericId, result.Id);

                dictionary.Add("ΙdentifyingΝame", result.Customer?.ΙdentifyingΝame);

                if (result.EmployeeWorkPlaces.Any(x => x.EmployeeId == _datatable.GenericId))
                    dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                        "WorkPlace", apiUrl, "checked", true));
                else
                    dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                        "WorkPlace", apiUrl, "", true));

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.WorkPlaces.CountAllAsyncFiltered(_filter);

            return this;
        }


        public async Task<WorkPlaceDataTableWorker> TimeShiftIndex()
        {
            var includes = new List<Func<IQueryable<WorkPlace>, IIncludableQueryable<WorkPlace, object>>>();
            includes.Add(x => x.Include(y => y.Customer).ThenInclude(z => z.Company));

            var entities = await _baseDatawork.WorkPlaces
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlace>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<WorkPlace>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("ΙdentifyingΝame", result.Customer?.ΙdentifyingΝame);

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.WorkPlaces.CountAllAsyncFiltered(_filter);

            return this;
        }


        public async Task<WorkPlaceDataTableWorker> ProjectionEmployeeRealHoursSum()
        {
            var includes = new List<Func<IQueryable<WorkPlace>, IIncludableQueryable<WorkPlace, object>>>();
            includes.Add(x => x.Include(y => y.Customer).ThenInclude(z => z.Company));


            if (_datatable.GenericId != 0)
                _filter = _filter.And(x => x.EmployeeWorkPlaces.Any(y => y.EmployeeId == _datatable.GenericId));

            var entities = await _baseDatawork.WorkPlaces
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlace>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<WorkPlace>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                var totalSeconds = await _baseDatawork.RealWorkHours
                       .GetEmployeeTotalSecondsFromRange(_datatable.GenericId, _datatable.StartOn, _datatable.EndOn, result.Id);

                var totalSecondsDay = await _baseDatawork.RealWorkHours
                       .GetEmployeeTotalSecondsDayFromRange(_datatable.GenericId, _datatable.StartOn, _datatable.EndOn, result.Id);

                var totalSecondsNight = await _baseDatawork.RealWorkHours
                        .GetEmployeeTotalSecondsNightFromRange(_datatable.GenericId, _datatable.StartOn, _datatable.EndOn, result.Id);
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
            EntitiesTotal = await _baseDatawork.WorkPlaces.CountAllAsyncFiltered(_filter);

            return this;
        }
    }
}
