using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.View;
using Bussiness;
using DataAccess.ViewModels.WorkHours;

namespace WebApplication.Api
{
    [Route("api/workhours")]
    [ApiController]
    public class WorkHoursApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;

        public WorkHoursApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: api/workhours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkHour>>> GetWorkHours()
        {
            return await _context.WorkHours.ToListAsync();
        }

        // GET: api/workhours/5
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

        // PUT: api/workhours/5
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
                    return NotFound();
                else
                    throw;
            }

            return Ok();
        }

        // POST: api/workhours
        [HttpPost]
        public async Task<ActionResult<WorkHour>> PostWorkHour(WorkHour workHour)
        {
            _context.WorkHours.Add(workHour);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkHour", new { id = workHour.Id }, workHour);
        }

        // DELETE: api/workhours/5
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


        // POST: api/workhours/hasoverlap
        [HttpPost("hasoverlap")]
        public async Task<ActionResult<WorkHour>> HasOverlap([FromBody] WorkHoursApiViewModel workHour)
        {
            if (workHour.EmployeeIds != null)
            {

                List<object> response = new List<object>();
                workHour.EmployeeIds.ForEach(id =>
                {
                    if (_baseDataWork.WorkHours.IsDateOverlaping(workHour, id))
                        response.Add(new
                        {
                            employeeId = id,
                            isSuccessful = 0,
                            value = "Ο χρήστης αυτός έχει ήδη δηλωθεί για αυτές τις ώρες"
                        });
                    else
                        response.Add(new
                        {
                            employeeId = id,
                            isSuccessful = 1,
                            value = ""

                        });

                });
                return Ok(response);
            }

            //(StartDate1 <= EndDate2) and (EndDate1 >= StartDate2)
            if (_baseDataWork.WorkHours.HasExactDate(workHour))
                return Ok(new { value = "Η βάρδια αντιστοιχεί στο χρονόγραμμα ", type = "success" });

            return Ok(new { value = "Δεν βρέθηκε η συγγεκριμενη βάρδια στο χρονόγραμμα ", type = "error" });
        }


        // POST: api/workhours/addEmployeeWorkhours
        [HttpPost("addEmployeeWorkhours")]
        public async Task<ActionResult<WorkHour>> AddEmployeeWorkhours([FromBody] List<WorkHoursApiViewModel> workHours)
        {
            //(StartDate1 <= EndDate2) and (EndDate1 >= StartDate2)
            foreach (var workHour in workHours)
            {
                //if (_baseDataWork.WorkHours.IsDateOverlaping(workHour))
                //    return NotFound();

                _baseDataWork.WorkHours.Add(new WorkHour()
                {
                    StartOn = workHour.StartOn,
                    EndOn = workHour.EndOn,
                    TimeShiftId = workHour.TimeShiftId,
                    EmployeeId = workHour.EmployeeId,
                    CreatedOn = DateTime.Now
                });
                await _baseDataWork.CompleteAsync();

            }

            return Ok("success my dudes");
        }


        // POST: api/workhours/getforcell
        [HttpPost("getforcell")]
        public async Task<ActionResult<WorkHour>> GetForCell([FromBody] WorkHoursApiViewModel workHour)
        {
            var varadata = _baseDataWork.WorkHours.GetCurrentAssignedOnCell(
                workHour.TimeShiftId,
                workHour.StartOn.Year,
                workHour.StartOn.Month,
                workHour.StartOn.Day,
                workHour.EmployeeId);

            return Ok(varadata);
        }





        private bool WorkHourExists(int id)
        {
            return _context.WorkHours.Any(e => e.Id == id);
        }
    }
}
