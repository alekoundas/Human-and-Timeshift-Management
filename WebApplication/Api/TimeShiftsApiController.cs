﻿using Bussiness;
using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Datatable;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Http;
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
    [Route("api/timeshifts")]
    [ApiController]
    public class TimeShiftsApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public TimeShiftsApiController(BaseDbContext BaseDbContext,
            SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: api/TimeShiftsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeShift>>> GetTimeShifts()
        {
            return await _context.TimeShifts.ToListAsync();
        }

        // GET: api/timeshifts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeShift>> GetTimeShift(int id)
        {
            var timeShift = await _context.TimeShifts.FindAsync(id);

            if (timeShift == null)
                return NotFound();

            return timeShift;
        }

        // PUT: api/timeshifts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimeShift(int id, TimeShift timeShift)
        {
            if (id != timeShift.Id)
                return BadRequest();

            _context.Entry(timeShift).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeShiftExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/timeshifts
        [HttpPost]
        public async Task<ActionResult<TimeShift>> PostTimeShift(TimeShift timeShift)
        {
            _context.TimeShifts.Add(timeShift);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTimeShift", new { id = timeShift.Id }, timeShift);
        }

        // DELETE: api/timeshifts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteViewModel>> DeleteTimeShift(int id)
        {
            var response = new DeleteViewModel();
            var timeShift = await _context.TimeShifts.Include(x => x.WorkHours).FirstOrDefaultAsync(x => x.Id == id);
            if (timeShift == null)
                return NotFound();

            _context.WorkHours.RemoveRange(timeShift.WorkHours);
            _context.TimeShifts.Remove(timeShift);
            var status = await _context.SaveChangesAsync();

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
            filter = filter.And(await GetUserRoleFiltersAsync());
            var includes = new List<Func<IQueryable<TimeShift>, IIncludableQueryable<TimeShift, object>>>();

            includes.Add(x => x.Include(y => y.WorkPlace));
            filter = filter.And(x => true);



            if (predicate == "RealWorkHourCreate")
            {
                filter = filter.And(x => x.Month == DateTime.Now.Month);
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
                filter = filter.And(x => x.Title.Contains(search));
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
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var orderDirection = datatable.Order[0].Dir;

            var filter = PredicateBuilder.New<TimeShift>();
            filter = filter.And(GetSearchFilter(datatable));

            var includes = new List<Func<IQueryable<TimeShift>, IIncludableQueryable<TimeShift, object>>>();
            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            var timeShifts = new List<TimeShift>();


            if (datatable.Predicate == "TimeShiftIndex")
            {
                filter = filter.And(x => x.WorkPlaceId == datatable.GenericId);
                timeShifts = await _baseDataWork.TimeShifts
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            }

            var mapedData = MapResults(timeShifts, datatable);

            var total = await _baseDataWork.TimeShifts.CountAllAsyncFiltered(filter);
            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        protected IEnumerable<ExpandoObject> MapResults(IEnumerable<TimeShift> results, Datatable datatable)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<TimeShift>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var timeShift in results)
            {
                var expandoObj = expandoObject.GetCopyFrom<TimeShift>(timeShift);
                var dictionary = (IDictionary<string, object>)expandoObj;
                if (datatable.Predicate == "TimeShiftIndex")
                {
                    dictionary.Add("Buttons", dataTableHelper.GetButtons("TimeShift",
                        "TimeShifts", timeShift.Id.ToString()));
                }
                returnObjects.Add(expandoObj);
            }

            return returnObjects;
        }
        private bool TimeShiftExists(int id)
        {
            return _context.TimeShifts.Any(e => e.Id == id);
        }

        private Func<IQueryable<TimeShift>, IOrderedQueryable<TimeShift>> SetOrderBy(string columnName, string orderDirection)
        {
            if (columnName != "")
                return x => x.OrderBy(columnName + " " + orderDirection.ToUpper());
            else
                return null;
        }
        private Expression<Func<TimeShift, bool>> GetSearchFilter(Datatable datatable)
        {
            var filter = PredicateBuilder.New<TimeShift>();
            if (datatable.Search.Value != null)
            {
                foreach (var column in datatable.Columns)
                {
                    if (column.Data == "Title")
                        filter = filter.Or(x => x.Title.Contains(datatable.Search.Value));
                    if (column.Data == "Month")
                        filter = filter.Or(x => x.Month.ToString().Contains(datatable.Search.Value));
                    if (column.Data == "Year")
                        filter = filter.Or(x => x.Year.ToString().Contains(datatable.Search.Value));
                }

            }
            else
                filter = filter.And(x => true);

            return filter;
        }

        private async Task<Expression<Func<TimeShift, bool>>> GetUserRoleFiltersAsync()
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
