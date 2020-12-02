using Bussiness;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // DELETE: api/workhours/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<WorkHour>> DeleteWorkHour(int id)
        {
            var workHour = await _context.WorkHours.FindAsync(id);
            if (workHour == null)
                return NotFound();

            _context.WorkHours.Remove(workHour);
            await _baseDataWork.SaveChangesAsync();

            return workHour;
        }

        // POST: api/workhours/deletebatch
        [HttpPost("deletebatch")]
        public async Task<ActionResult<WorkHour>> DeleteBatch([FromBody] WorkHourApiViewModel workHourViewModel)
        {
            var filter = PredicateBuilder.New<WorkHour>();

            foreach (var employeeId in workHourViewModel.EmployeeIds)
                filter = filter.Or(x => x.EmployeeId == employeeId);

            filter = filter.And(x => x.StartOn == workHourViewModel.StartOn &&
                x.EndOn == workHourViewModel.EndOn);

            var workHours = await _baseDataWork.WorkHours.GetFiltered(filter);
            if (workHours.Count > 0)
            {
                _baseDataWork.WorkHours.RemoveRange(workHours);
                await _baseDataWork.SaveChangesAsync();

            }

            return Ok(new { Value = "Its  me Mario" });
        }


        // POST: api/workhours/employeebelongstoworkhour
        [HttpPost("employeebelongstoworkhour")]
        public ActionResult<WorkHour> EmployeeBelongsToWorkHour([FromBody] WorkHourApiViewModel workHour)
        {
            var filter = PredicateBuilder.New<WorkHour>();
            filter = filter.And(x => x.StartOn == workHour.StartOn);
            filter = filter.And(x => x.EndOn == workHour.EndOn);
            filter = filter.And(x => x.TimeShiftId == workHour.TimeShiftId);

            if (workHour.EmployeeIds != null)
            {
                List<object> response = new List<object>();
                workHour.EmployeeIds.ForEach(id =>
                {

                    if (_baseDataWork.WorkHours.Where(filter).Any(x => x.EmployeeId == id))
                        response.Add(new
                        {
                            employeeId = id,
                            isSuccessful = 1,
                            errorType = "warning",
                            errorValue = "Η ώρα δήλωσης αντιστοιχεί στις βάρδιες του υπαλλήλου"
                        });
                    else
                        response.Add(new
                        {
                            employeeId = id,
                            isSuccessful = 0,
                            errorType = "warning",
                            errorValue = "Δεν βρέθηκε η ώρα δήλωσης στις βάρδιες του υπαλλήλου"

                        });

                });
                return Ok(response);
            }


            return Ok(new { value = "Δεν δόθηκαν υπάλληλοι" });
        }

        //RealWorkHour-Create view
        // POST: api/workhours/hasoverlap
        [HttpPost("hasoverlap")]
        public ActionResult<WorkHour> HasOverlap([FromBody] WorkHourApiViewModel workHour)
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
        public ActionResult<WorkHour> HasOverlapRange([FromBody] List<ApiRealWorkHoursHasOverlapRange> workHours)
        {
            List<object> response = new List<object>();
            foreach (var workHour in workHours)
            {

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
            }
            return Ok(response);

        }

        // POST: api/workhours/hasovertime
        [HttpPost("HasOvertime")]
        public ActionResult<WorkHour> HasOvertime([FromBody] List<WorkHourHasOvertimeRange> workHours)
        {
            List<object> response = new List<object>();
            foreach (var workHour in workHours)
            {
                workHour.EmployeeIds.ForEach(id =>
                {
                    if (_baseDataWork.WorkHours.IsDateOvertime(workHour, id))
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


        // POST: api/workhours/addEmployeeWorkhours
        [HttpPost("addEmployeeWorkhours")]
        public async Task<ActionResult<WorkHour>> AddEmployeeWorkhours([FromBody] List<WorkHourApiCreate> workHours)
        {
            var dataToSaveRange = new List<WorkHour>();

            foreach (var workHour in workHours)
            {
                //if (_baseDataWork.WorkHours.IsDateOverlaping(workHour))
                //    return NotFound();

                dataToSaveRange.Add(new WorkHour()
                {
                    StartOn = workHour.StartOn,
                    EndOn = workHour.EndOn,
                    TimeShiftId = workHour.TimeShiftId,
                    EmployeeId = workHour.EmployeeId,
                    IsDayOff = workHour.IsDayOff,
                    Comments = workHour.Comments,
                    CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                    CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id
                });
            }
            _baseDataWork.WorkHours.AddRange(dataToSaveRange);
            await _baseDataWork.SaveChangesAsync();

            return Ok(dataToSaveRange);
        }
        // POST: api/workhours/editEmployeeWorkhours
        [HttpPost("editEmployeeWorkhours")]
        public async Task<ActionResult<WorkHour>> EditEmployeeWorkhours([FromBody] List<WorkHourEdit> workHours)
        {
            var dataToSaveRange = new List<WorkHour>();

            foreach (var workHour in workHours)
            {
                var workHourToModify = await _baseDataWork.WorkHours
                    .FirstOrDefaultAsync(x =>
                        x.StartOn == workHour.StartOn &&
                        x.EndOn == workHour.EndOn &&
                        x.EmployeeId == workHour.EmployeeId
                 );
                //if workhour exists for employee, edit it
                if (workHourToModify != null)
                {
                    workHourToModify.StartOn = workHour.NewStartOn;
                    workHourToModify.EndOn = workHour.NewEndOn;
                    workHourToModify.IsDayOff = workHour.IsDayOff;
                    workHourToModify.Comments = workHour.Comments;
                    _baseDataWork.Update(workHourToModify);
                    await _baseDataWork.SaveChangesAsync();

                }
                //if workhour does NOT exists for employee, create it
                else
                {
                    dataToSaveRange.Add(new WorkHour()
                    {
                        StartOn = workHour.NewStartOn,
                        EndOn = workHour.NewEndOn,
                        TimeShiftId = workHour.TimeShiftId,
                        EmployeeId = workHour.EmployeeId,
                        CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                        CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id
                    });
                }

            }

            _baseDataWork.WorkHours.AddRange(dataToSaveRange);
            await _baseDataWork.SaveChangesAsync();

            return Ok(dataToSaveRange);
        }
        // POST: api/workhours/deleteEmployeeWorkhours
        [HttpPost("deleteEmployeeWorkhours")]
        public async Task<ActionResult<WorkHour>> DeleteEmployeeWorkhours([FromBody] List<WorkHourDelete> workHours)
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
        public async Task<ActionResult<WorkHour>> GetForEditCell([FromBody] GetForEditCellWorkHoursApiViewModel workHour)
        {
            var response = new List<GetForEditCellWorkHoursApiViewModel>();

            var workHours = await _baseDataWork.WorkHours
                .GetCurrentAssignedOnCellFilterByEmployeeIds(workHour);

            var groupedWorkHours = workHours
                .GroupBy(x => new { x.StartOn, x.EndOn });

            foreach (var group in groupedWorkHours)
                response.Add(new GetForEditCellWorkHoursApiViewModel
                {
                    WorkHourId = group.Select(x => x.Id).FirstOrDefault(),
                    StartOn = group.Key.StartOn,
                    EndOn = group.Key.EndOn,
                    IsDayOff = group.Select(x => x.IsDayOff).FirstOrDefault(),
                    Comments = group.Select(x => x.Comments).FirstOrDefault(),
                    EmployeeIds = group.Select(x => x.EmployeeId).ToList()
                });

            return Ok(response);
        }

    }
}
