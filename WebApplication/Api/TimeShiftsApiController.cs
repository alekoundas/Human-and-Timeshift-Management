using Bussiness;
using Bussiness.Helpers;
using Bussiness.Service;
using DataAccess;
using DataAccess.Libraries.Datatable;
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

        //RealWorkHourCreate
        // GET: api/TimeShiftsApi
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeShift>> GetTimeShift(int id)
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
                filter = filter.And(x => x.Month == DateTime.Now.Month);
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

            //var pageSize = datatable.Length;
            //var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            //var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            //var orderDirection = datatable.Order[0].Dir;
            //var filter = PredicateBuilder.New<TimeShift>();
            //filter = filter.And(GetSearchFilter(datatable));

            //var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<TimeShift>(HttpContext);

            //if (!canShowDeactivated)
            //    filter = filter.And(x => x.IsActive == true);


            //var includes = new List<Func<IQueryable<TimeShift>, IIncludableQueryable<TimeShift, object>>>();
            //var dataTableHelper = new DataTableHelper<ExpandoObject>();
            //var timeShifts = new List<TimeShift>();


            //if (datatable.Predicate == "TimeShiftIndex")
            //{
            //    filter = filter.And(x => x.WorkPlaceId == datatable.GenericId);
            //    timeShifts = await _baseDataWork.TimeShifts
            //        .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            //}

            //var mapedData = MapResults(timeShifts, datatable);

            //var total = await _baseDataWork.TimeShifts.CountAllAsyncFiltered(filter);
            //return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        //protected IEnumerable<ExpandoObject> MapResults(IEnumerable<TimeShift> results, Datatable datatable)
        //{
        //    var expandoObject = new ExpandoService();
        //    var dataTableHelper = new DataTableHelper<TimeShift>();
        //    List<ExpandoObject> returnObjects = new List<ExpandoObject>();
        //    foreach (var timeShift in results)
        //    {
        //        var expandoObj = expandoObject.GetCopyFrom<TimeShift>(timeShift);
        //        var dictionary = (IDictionary<string, object>)expandoObj;
        //        if (datatable.Predicate == "TimeShiftIndex")
        //        {
        //            dictionary.Add("Buttons", dataTableHelper.GetButtons("TimeShift",
        //                "TimeShifts", timeShift.Id.ToString()));
        //        }
        //        returnObjects.Add(expandoObj);
        //    }

        //    return returnObjects;
        //}
        //private bool TimeShiftExists(int id)
        //{
        //    return _context.TimeShifts.Any(e => e.Id == id);
        //}

        //private Func<IQueryable<TimeShift>, IOrderedQueryable<TimeShift>> SetOrderBy(string columnName, string orderDirection)
        //{
        //    if (columnName != "")
        //        return x => x.OrderBy(columnName + " " + orderDirection.ToUpper());
        //    else
        //        return null;
        //}
        //private Expression<Func<TimeShift, bool>> GetSearchFilter(Datatable datatable)
        //{
        //    var filter = PredicateBuilder.New<TimeShift>();
        //    if (datatable.Search.Value != null)
        //    {
        //        foreach (var column in datatable.Columns)
        //        {
        //            if (column.Data == "Title")
        //                filter = filter.Or(x => x.Title.Contains(datatable.Search.Value));
        //            if (column.Data == "Month")
        //                filter = filter.Or(x => x.Month.ToString().Contains(datatable.Search.Value));
        //            if (column.Data == "Year")
        //                filter = filter.Or(x => x.Year.ToString().Contains(datatable.Search.Value));
        //        }

        //    }
        //    else
        //        filter = filter.And(x => true);

        //    return filter;
        //}

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
