﻿using Bussiness;
using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Datatable;
using DataAccess.Models.Entity;
using DataAccess.Models.Identity;
using DataAccess.Models.Select2;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication.Utilities;

namespace WebApplication.Api
{
    [Route("api/workplaces")]
    [ApiController]
    public class WorkPlacesApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;

        public WorkPlacesApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext, UserManager<ApplicationUser> userManager)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }


        // GET: api/workplaces/deactivate/5
        [HttpGet("deactivate/{id}")]
        public async Task<ActionResult<DeactivateViewModel>> Deactivate(int id)
        {
            var response = new DeactivateViewModel();
            var workPlace = await _baseDataWork.WorkPlaces
                .FirstOrDefaultAsync(x => x.Id == id);

            workPlace.IsActive = !workPlace.IsActive;
            _baseDataWork.Update(workPlace);

            var status = await _context.SaveChangesAsync();
            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Το πόστο " +
                    workPlace.Title +
                    (workPlace.IsActive ? " ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : " ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Το πόστο " +
                     workPlace.Title +
                    " ΔΕΝ " +
                    (workPlace.IsActive ? "ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : "ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία";
            }

            response.ResponseTitle = "Αλλαγή κατάστασης πόστου";
            response.Entity = workPlace;

            return Ok(response);
        }


        // DELETE: api/workplaces/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteViewModel>> DeleteWorkPlace(int id)
        {
            var response = new DeleteViewModel();
            var workPlace = await _context.WorkPlaces
                .Include(x => x.WorkPlaceHourRestrictions).ThenInclude(x => x.HourRestrictions)
                .Include(x => x.TimeShifts).ThenInclude(x => x.RealWorkHours)
                .Include(x => x.TimeShifts).ThenInclude(x => x.WorkHours)
                .Include(x => x.WorkPlaceHourRestrictions)

                .FirstOrDefaultAsync(x => x.Id == id);

            if (workPlace == null)
                return NotFound();

            var workPlaceHourRestrictions = workPlace.WorkPlaceHourRestrictions;
            var workPlaceHourRestrictionsHourrestrictions = workPlace.WorkPlaceHourRestrictions.SelectMany(x => x.HourRestrictions);
            var workPlaceTimeShifts = workPlace.TimeShifts;
            var workPlaceTimeShiftsRealWorkHours = workPlace.TimeShifts.SelectMany(x => x.RealWorkHours);
            var workPlaceTimeShiftsWorkHours = workPlace.TimeShifts.SelectMany(x => x.WorkHours);

            _context.WorkHours.RemoveRange(workPlaceTimeShiftsWorkHours);
            _context.RealWorkHours.RemoveRange(workPlaceTimeShiftsRealWorkHours);
            _context.TimeShifts.RemoveRange(workPlaceTimeShifts);
            _context.HourRestrictions.RemoveRange(workPlaceHourRestrictionsHourrestrictions);
            _context.WorkPlaceHourRestrictions.RemoveRange(workPlaceHourRestrictions);
            _context.WorkPlaces.Remove(workPlace);
            var status = await _context.SaveChangesAsync();

            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Το πόστο " +
                    workPlace.Title +
                    " διαγράφηκε με επιτυχία." +
                    "Επίσης διαγράφηκαν για αυτο το πόστο: " +
                    " Περιορισμοί πόστων:" + workPlaceHourRestrictions.Count().ToString() +
                    " Χρονοδιαγράμματα:" + workPlaceTimeShifts.Count().ToString() +
                    " Βάρδιες:" + workPlaceTimeShiftsWorkHours.Count().ToString() +
                    " Πραγματικές Βάρδιες:" + workPlaceTimeShiftsRealWorkHours.Count().ToString();
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Το πόστο " +
                     workPlace.Title +
                    " ΔΕΝ διαγράφηκε!";
            }

            response.ResponseTitle = "Διαγραφή πόστου";
            response.Entity = workPlace;
            return response;
        }


        // GET: api/workplaces/select2
        [HttpPost("select2")]
        public async Task<ActionResult<WorkPlace>> Select2([FromBody] Select2Get select2)
        {
            var workPlaces = new List<WorkPlace>();
            var select2Helper = new Select2Helper();
            var total = await _baseDataWork.WorkPlaces.CountAllAsync();
            var hasMore = (select2.Page * 10) < total;
            var filter = PredicateBuilder.New<WorkPlace>();
            filter = filter.And(GetUserRoleFiltersAsync());

            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<WorkPlace>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);

            if (string.IsNullOrWhiteSpace(select2.Search))
                workPlaces = (List<WorkPlace>)await _baseDataWork
                     .WorkPlaces
                     .GetPaggingWithFilter(null, filter, null, 10, select2.Page);


            filter = filter.And(x => x.Title.Contains(select2.Search));
            workPlaces = await _baseDataWork
                .WorkPlaces
                .GetPaggingWithFilter(null, filter, null, 10, select2.Page);

            return Ok(select2Helper.CreateWorkplacesResponse(workPlaces, hasMore));
        }

        // GET: api/workplaces/select2
        [HttpGet("select2")]
        public async Task<ActionResult<WorkPlace>> Select2(string search, int page)
        {
            var workPlaces = new List<WorkPlace>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<WorkPlace>();
            filter = filter.And(GetUserRoleFiltersAsync());
            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<WorkPlace>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);

            if (string.IsNullOrWhiteSpace(search))
                workPlaces = (List<WorkPlace>)await _baseDataWork
                     .WorkPlaces
                     .GetPaggingWithFilter(null, filter, null, 10, page);
            else
            {

                filter = filter.And(x => x.Title.Contains(search));
                workPlaces = await _baseDataWork
                    .WorkPlaces
                    .GetPaggingWithFilter(null, filter, null, 10, page);

            }
            var total = await _baseDataWork.WorkPlaces.CountAllAsyncFiltered(filter);
            var hasMore = (page * 10) < total;

            return Ok(select2Helper.CreateWorkplacesResponse(workPlaces, hasMore));
        }

        // POST: api/workplaces/select2filtered
        [HttpPost("select2filtered")]
        public async Task<ActionResult<WorkPlace>> Select2Filtered([FromBody] Select2FilteredGet select2)
        {
            var workPlaces = new List<WorkPlace>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<WorkPlace>();
            filter = filter.And(GetUserRoleFiltersAsync());
            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<WorkPlace>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);
            if (select2.ExistingIds?.Count > 0)
                foreach (var workPlaceId in select2.ExistingIds)
                    filter = filter.And(x => x.Id != workPlaceId);

            if (!string.IsNullOrWhiteSpace(select2.Search))
                filter = filter.And(x => x.Title.Contains(select2.Search));

            workPlaces = await _baseDataWork
             .WorkPlaces
             .GetPaggingWithFilter(null, filter, null, 10, select2.Page);
            var total = await _baseDataWork.WorkPlaces.CountAllAsyncFiltered(filter);
            var hasMore = (select2.Page * 10) < total;

            return Ok(select2Helper.CreateWorkplacesResponse(workPlaces, hasMore));
        }



        [HttpGet("manageworkplaceemployee/{employeeId}/{workPlaceId}/{toggleState}")]
        public async Task<ActionResult<WorkPlace>> ManageWorkPlaceEmployee(int employeeId, int workPlaceId, string toggleState)
        {
            var employee = await _baseDataWork.Employees.FindAsync(employeeId);
            var workPlace = await _baseDataWork.WorkPlaces.FindAsync(workPlaceId);

            if (employee == null || workPlace == null)
                return NotFound();

            if (toggleState == "true" && _baseDataWork.EmployeeWorkPlaces
                .Any(x => x.EmployeeId == employeeId && x.WorkPlaceId == workPlaceId))
                return NotFound();

            if (toggleState == "true")
                _baseDataWork.EmployeeWorkPlaces.Add(new EmployeeWorkPlace()
                {
                    EmployeeId = employeeId,
                    WorkPlaceId = workPlaceId,
                    CreatedOn = DateTime.Now
                });

            else
                _baseDataWork.EmployeeWorkPlaces.Remove(
                     _baseDataWork.EmployeeWorkPlaces
                        .Where(x => x.EmployeeId == employeeId && x.WorkPlaceId == workPlaceId)
                        .FirstOrDefault());


            await _baseDataWork.SaveChangesAsync();

            return workPlace;
        }

        [HttpGet("manageworkplacecustomer/{customerId}/{workPlaceId}/{toggleState}")]
        public async Task<ActionResult<Object>> ManageWorkPlaceCustomer(int customerId, int workPlaceId, string toggleState)
        {
            var workPlace = await _baseDataWork.WorkPlaces
                .FirstOrDefaultAsync(x => x.Id == workPlaceId);

            if (toggleState == "true")
                workPlace.Customer = await _baseDataWork.Customers
                    .FirstOrDefaultAsync(x => x.Id == customerId);
            else
                workPlace.Customer = null;

            _baseDataWork.Update(workPlace);
            await _baseDataWork.SaveChangesAsync();

            //TempData["StatusMessage"] = "αμ μπλου ντα μπου ντι ντα μπουνται";
            return new { skata = "Polla Skata" };
        }

        // POST: api/companies/datatable
        [HttpPost("datatable")]
        public async Task<ActionResult<WorkPlace>> Datatable([FromBody] Datatable datatable)
        {
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var orderDirection = datatable.Order[0].Dir;

            var includes = new List<Func<IQueryable<WorkPlace>, IIncludableQueryable<WorkPlace, object>>>();
            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            var filter = PredicateBuilder.New<WorkPlace>();
            filter = filter.And(GetUserRoleFiltersAsync());
            filter = filter.And(GetSearchFilter(datatable));

            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<WorkPlace>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);

            var workPlaces = new List<WorkPlace>();


            if (datatable.Predicate == "WorkPlaceIndex")
            {
                includes.Add(x => x.Include(y => y.Customer));

                workPlaces = await _baseDataWork.WorkPlaces
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "CustomerDetails")
            {
                includes.Add(x => x.Include(y => y.Customer));

                workPlaces = await _baseDataWork.WorkPlaces
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "CustomerEdit")
            {
                includes.Add(x => x.Include(y => y.Customer));

                workPlaces = await _baseDataWork.WorkPlaces
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "EmployeeEdit")
            {
                includes.Add(x => x.Include(y => y.Customer));
                includes.Add(x => x.Include(y => y.EmployeeWorkPlaces).ThenInclude(z => z.Employee));
                filter = filter.And(x => x.Customer.Company != null);


                workPlaces = await _baseDataWork.WorkPlaces
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);

                workPlaces = workPlaces.Select(X => new WorkPlace
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
            }
            if (datatable.Predicate == "EmployeeDetails")
            {
                includes.Add(x => x.Include(y => y.Customer));
                includes.Add(x => x.Include(y => y.EmployeeWorkPlaces).ThenInclude(z => z.Employee));
                filter = filter.And(x => x.Customer.Company != null);

                workPlaces = await _baseDataWork.WorkPlaces
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "TimeShiftIndex")
            {
                includes.Add(x => x.Include(y => y.Customer).ThenInclude(z => z.Company));

                workPlaces = await _baseDataWork.WorkPlaces
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "ProjectionEmployeeRealHoursSum")
            {
                if (datatable.GenericId != 0)
                    filter = filter.And(x => x.EmployeeWorkPlaces.Any(y => y.EmployeeId == datatable.GenericId));

                workPlaces = await _baseDataWork.WorkPlaces
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            }

            var mapedData = await MapResults(workPlaces, datatable);

            var total = _baseDataWork.WorkPlaces.Where(filter).Count();
            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        protected async Task<IEnumerable<ExpandoObject>> MapResults(IEnumerable<WorkPlace> results, Datatable datatable)
        {
            var expandoObject = new ExpandoService();
            var dataTableHelper = new DataTableHelper<WorkPlace>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var workplace in results)
            {
                var expandoObj = expandoObject.GetCopyFrom<WorkPlace>(workplace);
                var dictionary = (IDictionary<string, object>)expandoObj;
                if (datatable.Predicate == "WorkPlaceIndex")
                {
                    dictionary.Add("Buttons", dataTableHelper.GetButtons(
                        "WorkPlace", "WorkPlaces", workplace.Id.ToString()));
                    dictionary.Add("ΙdentifyingΝame", workplace.Customer?.ΙdentifyingΝame);
                    returnObjects.Add(expandoObj);
                }
                if (datatable.Predicate == "CustomerDetails")
                {
                    var apiUrl = UrlHelper.CustomerWorkPlace(datatable.GenericId,
                         workplace.Id);

                    if (workplace.Customer?.Id == datatable.GenericId)
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "WorkPlace", apiUrl, "checked", true));
                    else
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "WorkPlace", apiUrl, "", true));

                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "CustomerEdit")
                {
                    var apiUrl = UrlHelper.CustomerWorkPlace(datatable.GenericId,
                        workplace.Id);

                    if (workplace.Customer?.Id == datatable.GenericId)
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "WorkPlace", apiUrl, "checked"));
                    else
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "WorkPlace", apiUrl, ""));

                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "EmployeeEdit")
                {
                    var apiUrl = UrlHelper.EmployeeWorkPlace(datatable.GenericId,
                        workplace.Id);

                    dictionary.Add("ΙdentifyingΝame", workplace.Customer?.ΙdentifyingΝame);

                    if (workplace.EmployeeWorkPlaces.Any(x => x.EmployeeId == datatable.GenericId))
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "WorkPlace", apiUrl, "checked"));
                    else
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "WorkPlace", apiUrl, ""));

                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "EmployeeDetails")
                {
                    var apiUrl = UrlHelper.EmployeeWorkPlace(datatable.GenericId,
                        workplace.Id);

                    dictionary.Add("ΙdentifyingΝame", workplace.Customer?.ΙdentifyingΝame);

                    if (workplace.EmployeeWorkPlaces.Any(x => x.EmployeeId == datatable.GenericId))
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "WorkPlace", apiUrl, "checked", true));
                    else
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "WorkPlace", apiUrl, "", true));

                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "TimeShiftIndex")
                {
                    dictionary.Add("ΙdentifyingΝame", workplace.Customer?.ΙdentifyingΝame);
                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "ProjectionEmployeeRealHoursSum")
                {
                    var totalSeconds = await _baseDataWork.RealWorkHours
                           .GetEmployeeTotalSecondsFromRange(datatable.GenericId, datatable.StartOn, datatable.EndOn, workplace.Id);

                    var totalSecondsDay = await _baseDataWork.RealWorkHours
                           .GetEmployeeTotalSecondsDayFromRange(datatable.GenericId, datatable.StartOn, datatable.EndOn, workplace.Id);

                    var totalSecondsNight = await _baseDataWork.RealWorkHours
                            .GetEmployeeTotalSecondsNightFromRange(datatable.GenericId, datatable.StartOn, datatable.EndOn, workplace.Id);
                    if (datatable.ShowHoursInPercentage)
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

            }

            return returnObjects;
        }

        private Expression<Func<WorkPlace, bool>> GetUserRoleFiltersAsync()
        {
            //Get WorkPlaceId from user roles
            var workPlaceIds = HttpContext.User.Claims
                .Where(x => x.Value.Contains("Specific_WorkPlace"))
                .Select(y => Int32.Parse(y.Value.Split("_")[2]));

            var filter = PredicateBuilder.New<WorkPlace>();
            foreach (var workPlaceId in workPlaceIds)
                filter = filter.Or(x => x.Id == workPlaceId);

            if (workPlaceIds.Count() == 0)
                filter = filter.And(x => true);

            return filter;
        }

        private Func<IQueryable<WorkPlace>, IOrderedQueryable<WorkPlace>> SetOrderBy(string columnName, string orderDirection)
        {
            if (columnName != "")
                return x => x.OrderBy(columnName + " " + orderDirection.ToUpper());
            //else if(columnName == "IsInWorkPlace")
            //return x => x.OrderBy(x=>x. + " " + orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<WorkPlace, bool>> GetSearchFilter(Datatable datatable)
        {
            var filter = PredicateBuilder.New<WorkPlace>();
            if (datatable.Search.Value != null)
            {
                foreach (var column in datatable.Columns)
                {
                    if (column.Data == "Title")
                        filter = filter.Or(x => x.Title.Contains(datatable.Search.Value));
                    if (column.Data == "Description")
                        filter = filter.Or(x => x.Description.Contains(datatable.Search.Value));
                    if (column.Data == "ΙdentifyingΝame")
                        filter = filter.Or(x => x.Customer.ΙdentifyingΝame.Contains(datatable.Search.Value));
                    if (column.Data == "Customer.company.title")
                        filter = filter.Or(x => x.Customer.Company.Title.Contains(datatable.Search.Value));
                }
            }
            else
                filter = filter.And(x => true);

            return filter;
        }

    }
}
