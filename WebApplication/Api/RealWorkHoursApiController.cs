﻿using System;
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
using DataAccess.ViewModels.WorkHours;
using LinqKit;

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
                return NotFound();

            return realWorkHour;
        }

        // PUT: api/realworkhours/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRealWorkHour(int id, RealWorkHour realWorkHour)
        {
            if (id != realWorkHour.Id)
                return BadRequest();

            _context.Entry(realWorkHour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RealWorkHourExists(id))
                    return NotFound();
                else
                    throw;
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
                return NotFound();

            _context.RealWorkHours.Remove(realWorkHour);
            await _context.SaveChangesAsync();

            return realWorkHour;
        }








        // POST: api/workhours/getforcell
        [HttpPost("getForEditCell")]
        public async Task<ActionResult<RealWorkHour>> GetForEditCell([FromBody] GetForEditCellWorkHoursApiViewModel realworkHour)
        {
            var response = new List<GetForEditCellWorkHoursApiViewModel>();

            var realWorkHours = await _baseDataWork.RealWorkHours
                .GetCurrentAssignedOnCellFilterByEmployeeIds(realworkHour);

            var groupedRealWorkHours = realWorkHours
                .GroupBy(x => new { x.StartOn, x.EndOn });

            foreach (var group in groupedRealWorkHours)
                response.Add(new GetForEditCellWorkHoursApiViewModel
                {
                    WorkHourId = group.Select(x => x.Id).FirstOrDefault(),
                    StartOn = group.Key.StartOn,
                    EndOn = group.Key.EndOn,
                    //IsDayOff = group.Select(x => x.IsDayOff).FirstOrDefault(),
                    Comments = group.Select(x => x.Comments).FirstOrDefault(),
                    EmployeeIds = group.Select(x => x.EmployeeId).ToList()
                });

            return Ok(response);
        }

        // POST: api/workhours/deletebatch
        [HttpPost("deletebatch")]
        public async Task<ActionResult<RealWorkHour>> DeleteBatch([FromBody] WorkHoursApiViewModel workHourViewModel)
        {
            var filter = PredicateBuilder.New<RealWorkHour>();

            foreach (var employeeId in workHourViewModel.EmployeeIds)
                filter = filter.Or(x => x.EmployeeId == employeeId);

            filter = filter.And(x => x.StartOn == workHourViewModel.StartOn &&
                x.EndOn == workHourViewModel.EndOn);

            var realWorkHours = await _baseDataWork.RealWorkHours.GetFiltered(filter);
            if (realWorkHours.Count > 0)
            {
                _baseDataWork.RealWorkHours.RemoveRange(realWorkHours);
                await _baseDataWork.SaveChangesAsync();

            }

            return Ok(new { Value = "Its  me Mario" });
        }
        // POST: api/workhours/deleteEmployeeWorkhours
        [HttpPost("deleteEmployeeWorkhours")]
        public async Task<ActionResult<RealWorkHour>> DeleteEmployeeWorkhours([FromBody] List<DeleteWorkHoursApiViewModel> workHours)
        {
            foreach (var workHour in workHours)
            {
                var workHourToDelete = await _baseDataWork.RealWorkHours
                    .FirstOrDefaultAsync(x =>
                        x.StartOn == workHour.StartOn &&
                        x.EndOn == workHour.EndOn &&
                        x.EmployeeId == workHour.EmployeeId
                 );

                _baseDataWork.RealWorkHours.Remove(workHourToDelete);
            }

            await _baseDataWork.SaveChangesAsync();

            return Ok("success my dudes");
        }
        // POST: api/workhours/editEmployeeWorkhours
        [HttpPost("editEmployeeWorkhours")]
        public async Task<ActionResult<WorkHour>> EditEmployeeWorkhours([FromBody] List<EditWorkHoursApiViewModel> workHours)
        {
            var realWorkHoursToSaveRange = new List<RealWorkHour>();

            foreach (var realWorkHour in workHours)
            {
                var workHourToModify = await _baseDataWork.RealWorkHours
                    .FirstOrDefaultAsync(x =>
                        x.StartOn == realWorkHour.StartOn &&
                        x.EndOn == realWorkHour.EndOn &&
                        x.EmployeeId == realWorkHour.EmployeeId
                 );


                if (realWorkHour.IsDayOff)
                {

                    //if workhour exists for employee, delete it and create workHour
                    if (workHourToModify != null)
                    {
                        workHourToModify.Comments = realWorkHour.Comments;
                        _baseDataWork.RealWorkHours.Remove(workHourToModify);

                    }
                    _baseDataWork.WorkHours.Add(new WorkHour
                    {
                        StartOn = realWorkHour.StartOn,
                        EndOn = realWorkHour.EndOn,
                        Comments = realWorkHour.Comments,
                        TimeShiftId = realWorkHour.TimeShiftId,
                        EmployeeId = realWorkHour.EmployeeId,
                        CreatedOn = DateTime.Now
                    });

                }
                else
                {
                    //if workhour exists for employee, edit it
                    if (workHourToModify != null)
                    {
                        workHourToModify.StartOn = realWorkHour.NewStartOn;
                        workHourToModify.EndOn = realWorkHour.NewEndOn;
                        workHourToModify.Comments = realWorkHour.Comments;

                        _baseDataWork.Update(workHourToModify);
                    }
                    //if workhour does NOT exists for employee, create it
                    else
                    {
                        realWorkHoursToSaveRange.Add(new RealWorkHour()
                        {
                            StartOn = realWorkHour.NewStartOn,
                            EndOn = realWorkHour.NewEndOn,
                            TimeShiftId = realWorkHour.TimeShiftId,
                            EmployeeId = realWorkHour.EmployeeId,
                            Comments = realWorkHour.Comments,
                            CreatedOn = DateTime.Now
                        });
                    }
                }
            }

            _baseDataWork.RealWorkHours.AddRange(realWorkHoursToSaveRange);
            await _baseDataWork.SaveChangesAsync();

            return Ok(realWorkHoursToSaveRange);
        }
        // POST: api/workhours/addEmployeeWorkhours
        [HttpPost("addEmployeeWorkhours")]
        public async Task<ActionResult<WorkHour>> AddEmployeeWorkhours([FromBody] List<CreateWorkHoursApiViewModel> workHours)
        {
            var realWorkHoursToSaveRange = new List<RealWorkHour>();
            var workHoursToSaveRange = new List<WorkHour>();

            foreach (var realWorkHour in workHours)
            {
                //if (_baseDataWork.WorkHours.IsDateOverlaping(workHour))
                //    return NotFound();
                if (realWorkHour.IsDayOff)
                    workHoursToSaveRange.Add(new WorkHour()
                    {
                        StartOn = realWorkHour.StartOn,
                        EndOn = realWorkHour.EndOn,
                        TimeShiftId = realWorkHour.TimeShiftId,
                        EmployeeId = realWorkHour.EmployeeId,
                        IsDayOff = realWorkHour.IsDayOff,
                        Comments = realWorkHour.Comments,
                        CreatedOn = DateTime.Now
                    });
                else
                    realWorkHoursToSaveRange.Add(new RealWorkHour()
                    {
                        StartOn = realWorkHour.StartOn,
                        EndOn = realWorkHour.EndOn,
                        TimeShiftId = realWorkHour.TimeShiftId,
                        EmployeeId = realWorkHour.EmployeeId,
                        Comments = realWorkHour.Comments,
                        CreatedOn = DateTime.Now
                    });
            }
            _baseDataWork.WorkHours.AddRange(workHoursToSaveRange);
            _baseDataWork.RealWorkHours.AddRange(realWorkHoursToSaveRange);
            await _baseDataWork.SaveChangesAsync();

            return Ok(new { realWorkHoursToSaveRange, workHoursToSaveRange });
        }
        // POST: api/realworkhours/hasoverlap
        [HttpPost("hasoverlap")]
        public async Task<ActionResult<RealWorkHour>> HasOverlap([FromBody] ApiRealWorkHourHasOverlap apiOverlap)
        {
            var dataToReturn = new List<ApiRealWorkHourHasOverlapResponse>();
            foreach (var id in apiOverlap.EmployeeIds)
            {
                if (_baseDataWork.RealWorkHours.AreDatesOverlaping(
                    apiOverlap.StartOn, apiOverlap.EndOn, id))

                    dataToReturn.Add(new ApiRealWorkHourHasOverlapResponse
                    {
                        EmployeeId = id,
                        ErrorType = "error",
                        ErrorValue = "Ο χρήστης αυτός έχει ήδη δηλωθεί για αυτές τις ώρες",
                    });

                if (_baseDataWork.RealWorkHours.AreDatesOverlapingLeaves(
                    apiOverlap.StartOn, apiOverlap.EndOn, id))

                    dataToReturn.Add(new ApiRealWorkHourHasOverlapResponse
                    {
                        EmployeeId = id,
                        ErrorType = "warning",
                        ErrorValue = "<br>Ο χρήστης αυτός έχει ήδη δηλωθεί για " +
                                        "αυτές τις ώρες ως άδεια",
                    });

                if (_baseDataWork.RealWorkHours.AreDatesOverlapingDayOff(
                   apiOverlap.StartOn, apiOverlap.EndOn, false, id))

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
        // POST: api/workhours/hasoverlap
        [HttpPost("HasOverlapRange")]
        public async Task<ActionResult<WorkHour>> HasOverlapRange([FromBody] List<ApiRealWorkHoursHasOverlapRange> workHours)
        {
            List<object> response = new List<object>();
            foreach (var workHour in workHours)
            {
                workHour.EmployeeIds.ForEach(id =>
                {
                    if (_baseDataWork.RealWorkHours.IsDateOverlaping(workHour, id))
                        response.Add(new
                        {
                            employeeId = id,
                            isSuccessful = 0,
                            value = "Ο χρήστης αυτός έχει ήδη δηλωθεί για αυτές τις ώρες"
                        });
                    else if (_baseDataWork.RealWorkHours.AreDatesOverlapingLeaves(
                        workHour.StartOn, workHour.EndOn, id))

                        response.Add(new
                        {
                            employeeId = id,
                            isSuccessful = 0,
                            value = "<br>Ο χρήστης αυτός έχει ήδη δηλωθεί για " +
                                            "αυτές τις ώρες ως άδεια",
                        });
                    else if (_baseDataWork.RealWorkHours.AreDatesOverlapingDayOff(
                        workHour.StartOn, workHour.EndOn, workHour.IsDayOff, id))

                        response.Add(new
                        {
                            employeeId = id,
                            isSuccessful = 0,
                            value = "<br>Ο χρήστης αυτός έχει ήδη δηλωθεί για " +
                                            "αυτές τις ώρες ως ρεπό",
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

        // POST: api/workhours/hasoverlap
        [HttpPost("HasOvertime")]
        public async Task<ActionResult<WorkHour>> HasOvertime([FromBody] List<ApiRealWorkHoursHasOvertimeRange> workHours)
        {
            List<object> response = new List<object>();
            foreach (var workHour in workHours)
            {
                workHour.EmployeeIds.ForEach(id =>
                {
                    if (_baseDataWork.RealWorkHours.IsDateOvertime(workHour, id))
                        response.Add(new
                        {
                            employeeId = id,
                            isSuccessful = 0,
                            value = "Ο υπάλληλος έχει ήδη μια βάρδια με λιγότερο απο 11 ώρες διαφορά"
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


        private bool RealWorkHourExists(int id)
        {
            return _context.RealWorkHours.Any(e => e.Id == id);
        }
    }
}
