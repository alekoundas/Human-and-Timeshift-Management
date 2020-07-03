using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity;
using Bussiness;
using DataAccess.ViewModels.View;

namespace WebApplication.Api
{
    [Route("api/employeeworkhours")]
    [ApiController]
    public class EmployeeWorkHoursApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public EmployeeWorkHoursApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: api/employeeworkhours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeWorkHour>>> GetEmployeeWorkHours()
        {
            return await _context.EmployeeWorkHours.ToListAsync();
        }

        // GET: api/employeeworkhours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeWorkHour>> GetEmployeeWorkHour(int id)
        {
            var workHour = await _context.EmployeeWorkHours.FindAsync(id);

            if (workHour == null)
                return NotFound();

            return workHour;
        }

        // PUT: api/employeeworkhours/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeWorkHour(int id, EmployeeWorkHour workHour)
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
                if (!EmployeeExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/employeeworkhours/addEmployeeWorkhours
        [HttpPost("addEmployeeWorkhours")]
        public async Task<ActionResult<EmployeeWorkHour>> addEmployeeWorkhours([FromBody] List<WorkHoursApiViewModel> workHours)
        {
            //(StartDate1 <= EndDate2) and (EndDate1 >= StartDate2)
            foreach (var workHour in workHours)
            {
                if (await _baseDataWork.EmployeeWorkHours.IsDateOverlaps(workHour))
                    return NotFound();

                _baseDataWork.WorkHours.Add(new WorkHour()
                {
                    StartOn = workHour.StartOn,
                    EndOn = workHour.EndOn,
                    TimeShiftId = workHour.TimeShiftId,
                    CreatedOn = DateTime.Now
                });
                    await _baseDataWork.CompleteAsync();


                var createdWorkHour = await _baseDataWork.WorkHours.FirstOrDefaultAsync(x =>
                x.StartOn == workHour.StartOn &&
                x.EndOn == workHour.EndOn &&
                x.TimeShiftId == workHour.TimeShiftId);

                _baseDataWork.EmployeeWorkHours.Add(new EmployeeWorkHour()
                {
                    EmployeeId = workHour.EmployeeId,
                    WorkHourId = createdWorkHour.Id,
                    CreatedOn = DateTime.Now
                });

                await _baseDataWork.CompleteAsync();
            }

            return Ok();
        }

        // DELETE: api/employeeworkhours/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeWorkHour>> DeleteEmployee(int id)
        {
            var workHour = await _context.EmployeeWorkHours.FindAsync(id);
            if (workHour == null)
                return NotFound();

            _context.EmployeeWorkHours.Remove(workHour);
            await _context.SaveChangesAsync();

            return workHour;
        }




        //[HttpGet("manageemployeeworkhour/{employeeId}/{workPlaceId}/{toggleState}")]
        //public async Task<ActionResult<WorkPlace>> ManageWorkPlaceEmployee(int employeeId, int workPlaceId, string toggleState)
        //{
        //    var employee = await _baseDataWork.Employees.FindAsync(employeeId);
        //    var workPlace = await _baseDataWork.WorkPlaces.FindAsync(workPlaceId);

        //    if (employee == null || workPlace == null)
        //        return NotFound();

        //    if (toggleState == "true" && _baseDataWork.EmployeeWorkPlaces
        //        .Any(x => x.EmployeeId == employeeId && x.WorkPlaceId == workPlaceId))
        //        return NotFound();

        //    if (toggleState == "true")
        //        _baseDataWork.EmployeeWorkPlaces.Add(new EmployeeWorkPlace()
        //        {
        //            EmployeeId = employeeId,
        //            WorkPlaceId = workPlaceId,
        //            CreatedOn = DateTime.Now
        //        });

        //    else
        //        _baseDataWork.EmployeeWorkPlaces.Remove(
        //             _baseDataWork.EmployeeWorkPlaces
        //                .Where(x => x.EmployeeId == employeeId && x.WorkPlaceId == workPlaceId)
        //                .FirstOrDefault());

        //    try
        //    {

        //        await _baseDataWork.CompleteAsync();
        //    }
        //    catch (Exception /*ex*/)
        //    {

        //        throw;
        //    }

        //    return workPlace;
        //}


        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
