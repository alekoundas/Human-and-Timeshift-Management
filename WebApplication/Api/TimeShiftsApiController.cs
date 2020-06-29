using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity.WorkTimeShift;

namespace WebApplication.Api
{
    [Route("api/timeshifts")]
    [ApiController]
    public class TimeShiftsApiController : ControllerBase
    {
        private readonly BaseDbContext _context;

        public TimeShiftsApiController(BaseDbContext context)
        {
            _context = context;
        }

        // GET: api/TimeShiftsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeShift>>> GetTimeShifts()
        {
            return await _context.TimeShifts.ToListAsync();
        }

        // GET: api/TimeShiftsApi/5
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

        // PUT: api/TimeShiftsApi/5
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

        // POST: api/TimeShiftsApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TimeShift>> PostTimeShift(TimeShift timeShift)
        {
            _context.TimeShifts.Add(timeShift);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTimeShift", new { id = timeShift.Id }, timeShift);
        }

        // DELETE: api/TimeShiftsApi/5
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

        private bool TimeShiftExists(int id)
        {
            return _context.TimeShifts.Any(e => e.Id == id);
        }
    }
}
