using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity;

namespace WebApplication.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkHoursApiController : ControllerBase
    {
        private readonly BaseDbContext _context;

        public WorkHoursApiController(BaseDbContext context)
        {
            _context = context;
        }

        // GET: api/WorkHoursApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkHour>>> GetWorkHours()
        {
            return await _context.WorkHours.ToListAsync();
        }

        // GET: api/WorkHoursApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkHour>> GetWorkHour(int id)
        {
            var workHour = await _context.WorkHours.FindAsync(id);

            if (workHour == null)
            {
                return NotFound();
            }

            return workHour;
        }

        // PUT: api/WorkHoursApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkHour(int id, WorkHour workHour)
        {
            if (id != workHour.Id)
            {
                return BadRequest();
            }

            _context.Entry(workHour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkHourExists(id))
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

        // POST: api/WorkHoursApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<WorkHour>> PostWorkHour(WorkHour workHour)
        {
            _context.WorkHours.Add(workHour);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkHour", new { id = workHour.Id }, workHour);
        }

        // DELETE: api/WorkHoursApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<WorkHour>> DeleteWorkHour(int id)
        {
            var workHour = await _context.WorkHours.FindAsync(id);
            if (workHour == null)
            {
                return NotFound();
            }

            _context.WorkHours.Remove(workHour);
            await _context.SaveChangesAsync();

            return workHour;
        }

        private bool WorkHourExists(int id)
        {
            return _context.WorkHours.Any(e => e.Id == id);
        }
    }
}
