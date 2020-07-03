using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity.WorkTimeShift;
using DataAccess.Models.Datatable;
using Bussiness;
using System.Dynamic;
using Bussiness.Service;
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
        // POST: api/timeshifts/getdatatable
        [HttpPost("getdatatable")]
        public async Task<ActionResult<TimeShift>> getdatatable([FromBody] Datatable datatable)
        {
            var total = await _baseDataWork.TimeShifts.CountAllAsync();
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var isDescending = datatable.Order[0].Dir == "desc";

            //TODO: order by
            var timeShifts = await _baseDataWork.TimeShifts.GetWithPagging(null,
                pageSize, pageIndex);

            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            var mapedData = MapResults(timeShifts);

            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        protected IEnumerable<ExpandoObject> MapResults(IEnumerable<TimeShift> results)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<TimeShift>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var result in results)
            {
                var expandoObj = expandoObject.GetCopyFrom<TimeShift>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;
                dictionary.Add("Buttons", dataTableHelper.GetButtons("TimeShift",
                    "TimeShifts", result.Id.ToString()));
                returnObjects.Add(expandoObj);
            }

            return returnObjects;
        }
        private bool TimeShiftExists(int id)
        {
            return _context.TimeShifts.Any(e => e.Id == id);
        }
    }
}
