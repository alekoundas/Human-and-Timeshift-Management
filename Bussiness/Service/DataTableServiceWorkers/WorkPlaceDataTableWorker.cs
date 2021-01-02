﻿using Bussiness.Helpers;
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
    public class WorkPlaceDataTableWorker : DataTableService
    {
        private BaseDatawork _baseDatawork { get; }
        private new Datatable _datatable { get; }
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
            if (_columnName == "IdentifyingName")
                return x => x.OrderBy(y => y.Customer.IdentifyingName + " " + _orderDirection.ToUpper());
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
                    if (column.Data == "IdentifyingName")
                        filter = filter.Or(x => x.Customer.IdentifyingName.Contains(_datatable.Search.Value));
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
                dictionary.Add("IdentifyingName", result.Customer?.IdentifyingName);

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

            if (!_datatable.FilterByIncludedCustomer)
                _filter = _filter.And(x => x.CustomerId == _datatable.GenericId);

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

            if (!_datatable.FilterByIncludedCustomer)
                _filter = _filter.And(x => x.CustomerId == _datatable.GenericId);

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

            if (!_datatable.FilterByIncludedWorkPlaces)
                _filter = _filter.And(x => x.EmployeeWorkPlaces.Any(y => y.WorkPlaceId == _datatable.GenericId));

            _filter = _filter.And(x => x.Customer.Company != null);

            var entities = await _baseDatawork.WorkPlaces
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            entities.ForEach(x => x.Customer.WorkPlaces = null);
            entities.ForEach(x => x.Customer.Company = null);
            entities.ForEach(x => x.EmployeeWorkPlaces.ToList().ForEach(y => y.Employee.EmployeeWorkPlaces = null));
            entities.ForEach(x => x.EmployeeWorkPlaces.ToList().ForEach(y => y.Employee.Company = null));
            entities.ForEach(x => x.EmployeeWorkPlaces.ToList().ForEach(y => y.Employee.RealWorkHours = null));
            entities.ForEach(x => x.EmployeeWorkPlaces.ToList().ForEach(y => y.Employee.WorkHours = null));

            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlace>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<WorkPlace>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                var apiUrl = UrlHelper.EmployeeWorkPlace(_datatable.GenericId, result.Id);

                dictionary.Add("CustomerName", result.Customer?.IdentifyingName);

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

            if (!_datatable.FilterByIncludedWorkPlaces)
                _filter = _filter.And(x => x.EmployeeWorkPlaces.Any(y => y.WorkPlaceId == _datatable.GenericId));

            _filter = _filter.And(x => x.Customer.Company != null);

            var entities = await _baseDatawork.WorkPlaces
                .GetPaggingWithFilter(SetOrderBy(), _filter, includes, _pageSize, _pageIndex);

            entities.ForEach(x => x.Customer.WorkPlaces = null);
            entities.ForEach(x => x.Customer.Company = null);
            entities.ForEach(x => x.EmployeeWorkPlaces.ToList().ForEach(y => y.Employee.EmployeeWorkPlaces = null));
            entities.ForEach(x => x.EmployeeWorkPlaces.ToList().ForEach(y => y.Employee.Company = null));
            entities.ForEach(x => x.EmployeeWorkPlaces.ToList().ForEach(y => y.Employee.RealWorkHours = null));
            entities.ForEach(x => x.EmployeeWorkPlaces.ToList().ForEach(y => y.Employee.WorkHours = null));
            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlace>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<WorkPlace>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                var apiUrl = UrlHelper.EmployeeWorkPlace(_datatable.GenericId, result.Id);

                dictionary.Add("CustomerName", result.Customer?.IdentifyingName);

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

            //entities.ForEach(x => x.Customer.Company.Customers = null);
            //Mapping
            var expandoService = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlace>();
            var returnObjects = new List<ExpandoObject>();

            foreach (var result in entities)
            {
                var expandoObj = expandoService.GetCopyFrom<WorkPlace>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;

                dictionary.Add("CompanyTitle", result.Customer?.Company?.Title);
                dictionary.Add("IdentifyingName", result.Customer?.IdentifyingName);

                returnObjects.Add(expandoObj);
            }
            EntitiesMapped = returnObjects;
            EntitiesTotal = await _baseDatawork.WorkPlaces.CountAllAsyncFiltered(_filter);

            return this;
        }


    }
}
