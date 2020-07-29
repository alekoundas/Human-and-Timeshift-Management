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
using LinqKit;

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
                return BadRequest();

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
                return NotFound();

            _context.WorkHours.Remove(workHour);
            await _context.SaveChangesAsync();

            return workHour;
        }

        // POST: api/workhours/deletebatch
        [HttpPost("deletebatch")]
        public async Task<ActionResult<WorkHour>> DeleteBatch([FromBody] WorkHoursApiViewModel workHourViewModel)
        {
            var filter = PredicateBuilder.New<WorkHour>();

            foreach (var employeeId in workHourViewModel.EmployeeIds)
                filter = filter.Or(x => x.EmployeeId == employeeId);

            var workHours = await _baseDataWork.WorkHours.GetFiltered(filter);
            if (workHours.Count > 0)
            {
                _baseDataWork.WorkHours.RemoveRange(workHours);
                await _baseDataWork.SaveChangesAsync();

            }

            return Ok(new { Value = "Its  me Mario" });
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

            if (_baseDataWork.WorkHours.HasExactDate(workHour))
                return Ok(new { value = "Η βάρδια αντιστοιχεί στο χρονόγραμμα </br>", type = "success" });

            return Ok(new { value = "Δεν βρέθηκε η συγγεκριμενη βάρδια στο χρονόγραμμα </br>", type = "error" });
        }


        // POST: api/workhours/hasoverlap
        [HttpPost("HasOverlapRange")]
        public async Task<ActionResult<WorkHour>> HasOverlapRange([FromBody] List<HasOverlapRangeWorkHoursApiViewModel> workHours)
        {
            List<object> response = new List<object>();
            foreach (var workHour in workHours)
            {

                workHour.EmployeeIds.ForEach(id =>
                {
                    if (_baseDataWork.WorkHours
                    .IsDateOverlaping(workHour, id))
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
            }
            return Ok(response);

        }


        // POST: api/workhours/addEmployeeWorkhours
        [HttpPost("addEmployeeWorkhours")]
        public async Task<ActionResult<WorkHour>> AddEmployeeWorkhours([FromBody] List<WorkHoursApiViewModel> workHours)
        {
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
                await _baseDataWork.SaveChangesAsync();
            }

            return Ok("success my dudes");
        }
        // POST: api/workhours/editEmployeeWorkhours
        [HttpPost("editEmployeeWorkhours")]
        public async Task<ActionResult<WorkHour>> EditEmployeeWorkhours([FromBody] List<EditWorkHoursApiViewModel> workHours)
        {
            foreach (var workHour in workHours)
            {
                var workHourToModify = await _baseDataWork.WorkHours
                    .FirstOrDefaultAsync(x =>
                        x.StartOn==workHour.StartOn &&
                        x.EndOn==workHour.EndOn &&
                        x.EmployeeId == workHour.EmployeeId
                 );
                //if workhour exists for employee, edit it
                if (workHourToModify != null)
                {
                    workHourToModify.StartOn = workHour.NewStartOn;
                    workHourToModify.EndOn = workHour.NewEndOn;
                    _baseDataWork.Update(workHourToModify);
                    await _baseDataWork.SaveChangesAsync();

                }
                //if workhour does NOT exists for employee, create it
                else
                {
                    _baseDataWork.WorkHours.Add(new WorkHour()
                    {
                        StartOn = workHour.NewStartOn,
                        EndOn = workHour.NewEndOn,
                        TimeShiftId = workHour.TimeShiftId,
                        EmployeeId = workHour.EmployeeId,
                        CreatedOn = DateTime.Now
                    });
                    await _baseDataWork.SaveChangesAsync();
                }

            }

            await _baseDataWork.SaveChangesAsync();

            return Ok("success my dudes");
        }
        // POST: api/workhours/deleteEmployeeWorkhours
        [HttpPost("deleteEmployeeWorkhours")]
        public async Task<ActionResult<WorkHour>> DeleteEmployeeWorkhours([FromBody] List<DeleteWorkHoursApiViewModel> workHours)
        {
            foreach (var workHour in workHours)
            {
                var workHourToDelete = await _baseDataWork.WorkHours
                    .FirstOrDefaultAsync(x =>
                        x.StartOn == workHour.StartOn &&
                        x.EndOn == workHour.EndOn &&
                        x.EmployeeId == workHour.EmployeeId
                 );

                _baseDataWork.WorkHours.Remove(workHourToDelete);
            }

            await _baseDataWork.SaveChangesAsync();

            return Ok("success my dudes");
        }


        // POST: api/workhours/getforcell
        [HttpPost("getForEditCell")]
        public async Task<ActionResult<WorkHour>> GetForEditCell([FromBody] WorkHoursApiViewModel workHour)
        {
            var response = new List<WorkHoursApiViewModel>();

            var workHours = await _baseDataWork.WorkHours
                .GetCurrentAssignedOnCellFilterByEmployeeIds(workHour);

            var groupedWorkHours = workHours
                .GroupBy(x => new { x.StartOn, x.EndOn });

            foreach (var group in groupedWorkHours)
                response.Add(new WorkHoursApiViewModel
                {
                    WorkHourId = group.Select(x => x.Id).FirstOrDefault(),
                    StartOn = group.Key.StartOn,
                    EndOn = group.Key.EndOn,
                    EmployeeIds = group.Select(x => x.EmployeeId).ToList()
                });

            return Ok(response);
        }


        private bool WorkHourExists(int id)
        {
            return _context.WorkHours.Any(e => e.Id == id);
        }
    }
}
