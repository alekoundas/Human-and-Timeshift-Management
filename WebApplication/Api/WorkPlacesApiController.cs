using Bussiness;
using Bussiness.Helpers;
using Bussiness.Service;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Libraries.Select2;
using DataAccess.Models.Entity;
using DataAccess.Models.Identity;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

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

            var status = await _baseDataWork.SaveChangesAsync();
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
        public async Task<ActionResult<DeleteViewModel>> Delete(int id)
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

            _context.WorkPlaces.Remove(workPlace);
            var status = await _baseDataWork.SaveChangesAsync();

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

            //if (toggleState == "true" && _baseDataWork.EmployeeWorkPlaces
            //    .Any(x => x.EmployeeId == employeeId && x.WorkPlaceId == workPlaceId))
            //    return NotFound();

            if (toggleState == "true")
                _baseDataWork.EmployeeWorkPlaces.Add(new EmployeeWorkPlace()
                {
                    EmployeeId = employeeId,
                    WorkPlaceId = workPlaceId,
                    CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                    CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id
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


            return new { skata = "Polla Skata" };
        }

        // POST: api/companies/datatable
        [HttpPost("datatable")]
        public async Task<ActionResult<WorkPlace>> Datatable([FromBody] Datatable datatable)
        {
            var results = (await new DataTableService(datatable, _baseDataWork, HttpContext)
              .ConvertData<WorkPlace>())
              .CompleteResponse<WorkPlace>();

            return Ok(results);
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
    }
}
