using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.Models.Datatable;
using Bussiness;
using System.Dynamic;
using Bussiness.Service;
using WebApplication.Utilities;
using DataAccess.ViewModels;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

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
            {
                return NotFound();
            }

            return timeShift;
        }

        // PUT: api/timeshifts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimeShift(int id, TimeShift timeShift)
        {
            if (id != timeShift.Id)
            {
                return BadRequest();
            }

            _context.Entry(timeShift).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeShiftExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
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
        public async Task<ActionResult<TimeShift>> DeleteTimeShift(int id)
        {
            var timeShift = await _context.TimeShifts.FindAsync(id);
            if (timeShift == null)
            {
                return NotFound();
            }

            _context.TimeShifts.Remove(timeShift);
            await _context.SaveChangesAsync();

            return timeShift;
        }




        // GET: api/timeshifts/select2
        [HttpGet("select2")]
        public async Task<ActionResult<TimeShift>> Select2(string search, int page)
        {
            var timeShifts = new List<TimeShift>();
            var select2Helper = new Select2Helper();

            if (string.IsNullOrWhiteSpace(search))
            {
                timeShifts = (List<TimeShift>)await _baseDataWork.TimeShifts
                    .GetPaggingWithFilter(null, null, null, 10, page);

                return Ok(select2Helper.CreateTimeShiftsResponse(timeShifts));
            }

            timeShifts = (List<TimeShift>)await _baseDataWork.TimeShifts
               .GetPaggingWithFilter(null, x => x.Title.Contains(search), null, 10, page);

            return Ok(select2Helper.CreateTimeShiftsResponse(timeShifts));
        }

        // POST: api/timeshifts/datatable
        [HttpPost("datatable")]
        public async Task<ActionResult<TimeShift>> Datatable([FromBody] Datatable datatable)
        {
            var total = await _baseDataWork.TimeShifts.CountAllAsync();
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var orderDirection = datatable.Order[0].Dir;

            var includes = new List<Func<IQueryable<TimeShift>, IIncludableQueryable<TimeShift, object>>>();
            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            Expression<Func<TimeShift, bool>> filter = x => true;

            var timeShifts = new List<TimeShift>();


            if (datatable.Predicate == "TimeShiftIndex")
            {
                filter = x => x.WorkPlaceId == datatable.GenericId;
                timeShifts = await _baseDataWork.TimeShifts
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            }

            var mapedData = MapResults(timeShifts,datatable);

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

    }
}
