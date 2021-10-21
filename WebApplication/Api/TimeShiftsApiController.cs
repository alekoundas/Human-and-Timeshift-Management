using Bussiness;
using Bussiness.Helpers;
using Bussiness.Service;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Libraries.Select2;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApplication.Api
{
    [Route("api/timeshifts")]
    [ApiController]
    public class TimeShiftsApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public TimeShiftsApiController(BaseDbContext BaseDbContext,
            SecurityDbContext securityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(securityDbContext);
        }


        // GET: api/TimeShifts/id
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeShift>> Get(int id)
        {
            var timeShift = await _context.TimeShifts.FindAsync(id);

            if (timeShift == null)
                return NotFound();

            return timeShift;
        }

        // GET: api/timeshifts/deactivate/5
        [HttpGet("deactivate/{id}")]
        public async Task<ActionResult<DeactivateViewModel>> Deactivate(int id)
        {
            var response = new DeactivateViewModel();
            var timeshift = await _baseDataWork.TimeShifts
                .FirstOrDefaultAsync(x => x.Id == id);

            timeshift.IsActive = !timeshift.IsActive;
            _baseDataWork.Update(timeshift);

            var status = await _baseDataWork.SaveChangesAsync();
            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Το χρονοδιάγραμμα " +
                    timeshift.Title +
                    (timeshift.IsActive ? " ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : " ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Το χρονοδιάγραμμα " +
                     timeshift.Title +
                    " ΔΕΝ " +
                    (timeshift.IsActive ? "ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : "ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία";
            }

            response.ResponseTitle = "Αλλαγή κατάστασης χρονοδιαγράμματος";
            response.Entity = timeshift;

            return Ok(response);
        }

        // DELETE: api/timeshifts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteViewModel>> DeleteTimeShift(int id)
        {
            var response = new DeleteViewModel();
            var timeShift = await _context.TimeShifts
                .Include(x => x.WorkHours)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (timeShift == null)
                return NotFound();

            _context.WorkHours.RemoveRange(timeShift.WorkHours);
            _context.TimeShifts.Remove(timeShift);
            var status = await _baseDataWork.SaveChangesAsync();

            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Το χρονοδιάγραμμα " +
                    timeShift.Title +
                    " διαγράφηκε με επιτυχία." +
                    " Επίσης διαγράφηκαν για αυτο το χρονοδιάγραμμα:" +
                    "Βάρδιες:" + timeShift.WorkHours.Count;
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Το χρονοδιάγραμμα" +
                     timeShift.Title +
                    " ΔΕΝ διαγράφηκε!";
            }

            response.ResponseTitle = "Διαγραφή χρονοδιάγραμματος";
            response.Entity = timeShift;
            return response;
        }


        // POST: api/timeshifts/select2
        [HttpPost("select2")]
        public async Task<ActionResult<TimeShift>> Select2(Select2 select2)
        {
            var entities = new List<TimeShift>();
            var select2Helper = new Select2Helper();
            var includes = new List<Func<IQueryable<TimeShift>, IIncludableQueryable<TimeShift, object>>>();
            includes.Add(x => x.Include(y => y.WorkPlace));
            var filter = PredicateBuilder.New<TimeShift>();
            filter = filter.And(x => true);
            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<TimeShift>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);

            if (select2.FromEntityId != 0)
                filter = filter.And(x => x.Id == select2.FromEntityId);

            if (select2.ExistingIds?.Count > 0)
                foreach (var workPlaceId in select2.ExistingIds)
                    filter = filter.And(x => x.Id != workPlaceId);

            if (!string.IsNullOrWhiteSpace(select2.Search))
                filter = filter.And(x => x.Title.Contains(select2.Search));

            entities = await _baseDataWork.TimeShifts
                .GetPaggingWithFilter(null, filter, includes, 10, select2.Page);

            var total = await _baseDataWork.TimeShifts.CountAllAsyncFiltered(filter);
            var hasMore = (select2.Page * 10) < total;

            return Ok(select2Helper.CreateTimeShiftsResponse(entities, hasMore));
        }


        // GET: api/timeshifts/select2
        [HttpGet("select2")]
        public async Task<ActionResult<TimeShift>> Select2(string search, int page, string predicate = "", int workPlaceId = 0)
        {
            var timeShifts = new List<TimeShift>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<TimeShift>();
            filter = filter.And(GetUserRoleFilters());
            var includes = new List<Func<IQueryable<TimeShift>, IIncludableQueryable<TimeShift, object>>>();

            includes.Add(x => x.Include(y => y.WorkPlace));
            filter = filter.And(x => true);
            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<TimeShift>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);

            if (predicate == "RealWorkHourCreate")
            {
                filter = filter.And(x => x.Month == DateTime.Now.AddHours(3).Month);
            }
            else if (predicate == "RealWorkHourClockIn")
            {
                filter = filter.And(x => x.Month == DateTime.Now.Month);
                var loggedInUserId = HttpAccessorService.GetLoggeInUser_Id;
                if (loggedInUserId != null)
                {
                    var user = _securityDatawork.ApplicationUsers.Get(loggedInUserId);
                    if (user.IsEmployee)
                        filter = filter.And(x =>
                            x.WorkPlace.EmployeeWorkPlaces
                                .Any(y => y.EmployeeId == user.EmployeeId));
                }
            }
            else
            {
                if (workPlaceId != 0)
                    filter = filter.And(x => x.WorkPlaceId == workPlaceId);
            }

            if (string.IsNullOrWhiteSpace(search))
            {
                timeShifts = (List<TimeShift>)await _baseDataWork.TimeShifts
                    .GetPaggingWithFilter(null, filter, includes, 10, page);
            }
            else
            {
                filter = filter.And(x => x.Title.Contains(search) || x.WorkPlace.Title.Contains(search));
                timeShifts = (List<TimeShift>)await _baseDataWork.TimeShifts
                   .GetPaggingWithFilter(null, filter, includes, 10, page);
            }
            var total = await _baseDataWork.TimeShifts.CountAllAsyncFiltered(filter);
            var hasMore = (page * 10) < total;
            return Ok(select2Helper.CreateTimeShiftsResponse(timeShifts, hasMore));
        }

        // POST: api/timeshifts/datatable
        [HttpPost("datatable")]
        public async Task<ActionResult<TimeShift>> Datatable([FromBody] Datatable datatable)
        {
            var results = (await new DataTableService(datatable, _baseDataWork, HttpContext)
              .ConvertData<TimeShift>())
              .CompleteResponse<TimeShift>();

            return Ok(results);

        }

       

        private Expression<Func<TimeShift, bool>> GetUserRoleFilters()
        {
            //Get WorkPlaceId from user roles
            var workPlaceIds = HttpContext.User.Claims
                .Where(x => x.Value.Contains("Specific_WorkPlace"))
                .Select(y => Int32.Parse(y.Value.Split("_")[2]));

            var filter = PredicateBuilder.New<TimeShift>();
            foreach (var workPlaceId in workPlaceIds)
                filter = filter.Or(x => x.WorkPlaceId == workPlaceId);

            if (workPlaceIds.Count() == 0)
                filter = filter.And(x => true);

            return filter;
        }
    }
}
