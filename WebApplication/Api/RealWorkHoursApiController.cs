using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.RealWorkHours;
using Bussiness;

namespace WebApplication.Api
{
    [Route("api/realworkhours")]
    [ApiController]
    public class RealWorkHoursApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;

        public RealWorkHoursApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: api/realworkhours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RealWorkHour>>> GetRealWorkHours()
        {
            return await _context.RealWorkHours.ToListAsync();
        }

        // GET: api/realworkhours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RealWorkHour>> GetRealWorkHour(int id)
        {
            var realWorkHour = await _context.RealWorkHours.FindAsync(id);

            if (realWorkHour == null)
            {
                return NotFound();
            }

            return realWorkHour;
        }

        // PUT: api/realworkhours/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRealWorkHour(int id, RealWorkHour realWorkHour)
        {
            if (id != realWorkHour.Id)
            {
                return BadRequest();
            }

            _context.Entry(realWorkHour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RealWorkHourExists(id))
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

        // POST: api/realworkhours
        [HttpPost]
        public async Task<ActionResult<RealWorkHour>> PostRealWorkHour(RealWorkHour realWorkHour)
        {
            _context.RealWorkHours.Add(realWorkHour);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRealWorkHour", new { id = realWorkHour.Id }, realWorkHour);
        }

        // DELETE: api/realworkhours/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RealWorkHour>> DeleteRealWorkHour(int id)
        {
            var realWorkHour = await _context.RealWorkHours.FindAsync(id);
            if (realWorkHour == null)
            {
                return NotFound();
            }

            _context.RealWorkHours.Remove(realWorkHour);
            await _context.SaveChangesAsync();

            return realWorkHour;
        }





        // POST: api/realworkhours/hasoverlap
        [HttpPost("hasoverlap")]
        public async Task<ActionResult<RealWorkHour>> HasOverlap([FromBody] ApiRealWorkHourHasOverlap apiOverlap)
        {
            var dataToReturn = new List<ApiRealWorkHourHasOverlapResponse>();
            foreach (var id in apiOverlap.EmployeeIds)
            {
                if (_baseDataWork.RealWorkHours.AreDatesOverlaping(apiOverlap, id))
                    dataToReturn.Add(new ApiRealWorkHourHasOverlapResponse
                    {
                        EmployeeId = id,
                        ErrorType = "error",
                        ErrorValue = "Ο χρήστης αυτός έχει ήδη δηλωθεί για αυτές τις ώρες",
                    });

                if (_baseDataWork.RealWorkHours.AreDatesOverlapingLeaves(
                    apiOverlap, id))

                    dataToReturn.Add(new ApiRealWorkHourHasOverlapResponse
                    {
                        EmployeeId = id,
                        ErrorType = "warning",
                        ErrorValue = "<br>Ο χρήστης αυτός έχει ήδη δηλωθεί για " +
                                        "αυτές τις ώρες ως άδεια",
                    });

                if (_baseDataWork.RealWorkHours.AreDatesOverlapingDayOff(
                   apiOverlap, id))

                    dataToReturn.Add(new ApiRealWorkHourHasOverlapResponse
                    {
                        EmployeeId = id,
                        ErrorType = "warning",
                        ErrorValue = "<br>Ο χρήστης αυτός έχει ήδη δηλωθεί για " +
                                        "αυτές τις ώρες ως ρεπό",
                    });
            };


            return Ok(dataToReturn);
        }






        private bool RealWorkHourExists(int id)
        {
            return _context.RealWorkHours.Any(e => e.Id == id);
        }
    }
}
