﻿using Bussiness;
using Bussiness.Service;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Api
{
    [Route("api/realworkhours")]
    [ApiController]
    public class RealWorkHoursApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        private readonly LogService _logService;

        public RealWorkHoursApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
            _logService = new LogService(SecurityDbContext);
        }

        // POST: api/RealWorkHours/Datatable
        [HttpPost("Datatable")]
        public async Task<ActionResult<RealWorkHour>> Datatable([FromBody] Datatable datatable)
        {
            var results = (await new DataTableService(datatable, _baseDataWork, HttpContext)
                .ConvertData<RealWorkHour>())
                .CompleteResponse<RealWorkHour>();
            return Ok(results);
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
                await _baseDataWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return NoContent();
        }

        // POST: api/realworkhours
        [HttpPost]
        public async Task<ActionResult<RealWorkHour>> PostRealWorkHour(RealWorkHour realWorkHour)
        {

            _context.RealWorkHours.Add(realWorkHour);
            await _baseDataWork.SaveChangesAsync();
            _logService.OnCreateEntity("RealWorkHour", realWorkHour);
            return CreatedAtAction("GetRealWorkHour", new { id = realWorkHour.Id }, realWorkHour);
        }

        // DELETE: api/realworkhours/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteViewModel>> Delete(int id)
        {
            var response = new DeleteViewModel();
            var realWorkHour = await _context.RealWorkHours.FindAsync(id);

            if (realWorkHour == null)
                return NotFound();

            _baseDataWork.RealWorkHours.Remove(realWorkHour);
            _logService.OnDeleteEntity("RealWorkHour", realWorkHour);

            var status = await _baseDataWork.SaveChangesAsync();

            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Η βάρδια " +
                    realWorkHour.StartOn.ToString() +
                    " - " +
                    realWorkHour.EndOn.ToString() +
                    " διαγράφηκε με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Η βάρδια " +
                    realWorkHour.StartOn.ToString() +
                    " - " +
                    realWorkHour.EndOn.ToString() +
                    " ΔΕΝ διαγράφηκε!";
            }

            response.ResponseTitle = "Διαγραφή βάρδιας";
            response.Entity = realWorkHour;
            return response;
        }


        //// POST: api/realworkhours
        //[HttpPost("ClockIn")]
        //public async Task<ActionResult<RealWorkHour>> ClockIn(
        //    [FromBody] RealWorkHourTimeClock viewModel)
        //{
        //    var user = _securityDatawork.ApplicationUsers.Get(viewModel.UserId);
        //    if (user.IsEmployee)
        //    {
        //        viewModel.EmployeeId = user.EmployeeId;
        //        _context.RealWorkHours.Add(RealWorkHourTimeClock.ClockIn(viewModel));

        //        await _baseDataWork.SaveChangesAsync();
        //    }

        //    return Ok(new { });
        //}

        //// POST: api/realworkhours
        //[HttpPost("ClockOut")]
        //public async Task<ActionResult<RealWorkHour>> ClockOut(
        //    [FromBody] RealWorkHourTimeClock viewModel)
        //{
        //    var user = _securityDatawork.ApplicationUsers.Get(viewModel.UserId);
        //    if (user.IsEmployee)
        //    {
        //        var realWorkHour = await _context.RealWorkHours.
        //            FirstOrDefaultAsync(x =>
        //                x.IsInProgress == true &&
        //                x.EmployeeId == user.EmployeeId);

        //        realWorkHour.EndOn = viewModel.CurrentDate;
        //        realWorkHour.IsInProgress = false;
        //        if (viewModel.Comments.Length > 1)
        //        {
        //            if (realWorkHour.Comments.Length > 1)
        //                realWorkHour.Comments =
        //                    "ClockIn:'" + realWorkHour.Comments + "'" +
        //                    "ClockOut:'" + viewModel.Comments + "'";
        //            else
        //                realWorkHour.Comments = viewModel.Comments;
        //        }

        //        await _baseDataWork.SaveChangesAsync();
        //    }

        //    return Ok(new { });
        //}






        // POST: api/realworkhours/getforcell
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
                    EndOn = group.Key.EndOn.Value,
                    //IsDayOff = group.Select(x => x.IsDayOff).FirstOrDefault(),
                    Comments = group.Select(x => x.Comments).FirstOrDefault(),
                    EmployeeIds = group.Select(x => x.EmployeeId).ToList()
                });

            return Ok(response);
        }


        // POST: api/realworkhours/deletebatch
        [HttpPost("deletebatch")]
        public async Task<ActionResult<RealWorkHour>> DeleteBatch([FromBody] WorkHourApiViewModel workHourViewModel)
        {
            var filter = PredicateBuilder.New<RealWorkHour>();
            if (workHourViewModel.EmployeeIds.Count > 0)
            {

                foreach (var employeeId in workHourViewModel.EmployeeIds)
                    filter = filter.Or(x => x.EmployeeId == employeeId);

                filter = filter.And(x =>
                    x.StartOn.Date == workHourViewModel.StartOn.Date &&
                    x.StartOn.Hour == workHourViewModel.StartOn.Hour &&
                    x.StartOn.Minute == workHourViewModel.StartOn.Minute &&
                    x.EndOn.Value.Date == workHourViewModel.EndOn.Date &&
                    x.EndOn.Value.Hour == workHourViewModel.EndOn.Hour &&
                    x.EndOn.Value.Minute == workHourViewModel.EndOn.Minute);

                var realWorkHours = await _baseDataWork.RealWorkHours.GetFiltered(filter);
                if (realWorkHours.Count > 0)
                {
                    _baseDataWork.RealWorkHours.RemoveRange(realWorkHours);
                    _logService.OnDeleteEntity("RealWorkHour", realWorkHours as object);
                    await _baseDataWork.SaveChangesAsync();

                }
            }

            return Ok(new { Value = "  __  " });
        }
        // POST: api/realworkhours/deleteEmployeeWorkhours
        [HttpPost("deleteEmployeeWorkhours")]
        public async Task<ActionResult<RealWorkHour>> DeleteEmployeeWorkhours([FromBody] List<WorkHourDelete> workHours)
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
                _logService.OnDeleteEntity("RealWorkHour", workHourToDelete);
            }

            await _baseDataWork.SaveChangesAsync();

            return Ok("success my dudes");
        }

        // POST: api/realworkhours/editEmployeeRealWorkhour
        [HttpPost("editEmployeeRealWorkhour")]
        public async Task<ActionResult<WorkHour>> EditEmployeeRealWorkhour([FromBody] RealWorkHourEdit realWorkHour)
        {
            var expandoService = new ExpandoService();
            object logOriginalEntity;
            object logEditedEntity;
            var workHourToModify = await _baseDataWork.RealWorkHours
                .FirstOrDefaultAsync(x => x.Id == realWorkHour.RealworkHourId);

            //if workhour exists for employee, edit it
            if (workHourToModify != null)
            {
                logOriginalEntity = expandoService.GetCopyFrom(workHourToModify);
                workHourToModify.StartOn = realWorkHour.StartOn;
                workHourToModify.EndOn = realWorkHour.EndOn;
                workHourToModify.Comments = realWorkHour.Comments;
                logEditedEntity = expandoService.GetCopyFrom(workHourToModify);

                _baseDataWork.Update(workHourToModify);
                await _baseDataWork.SaveChangesAsync();
                _logService.OnEditEntity("RealWorkHour", logOriginalEntity, logEditedEntity);

            }

            return Ok();
        }

        // POST: api/realworkhours/editEmployeeWorkhours
        [HttpPost("editEmployeeWorkhours")]
        public async Task<ActionResult<WorkHour>> EditEmployeeWorkhours([FromBody] List<WorkHourEdit> workHours)
        {
            var realWorkHoursToSaveRange = new List<RealWorkHour>();

            foreach (var realWorkHour in workHours)
            {
                var expandoService = new ExpandoService();
                object logOriginalEntity;
                object logEditedEntity;


                var workHourToModify = await _baseDataWork.RealWorkHours
                    .FirstOrDefaultAsync(x =>
                        x.StartOn.Date == realWorkHour.StartOn.Date &&
                        x.StartOn.Hour == realWorkHour.StartOn.Hour &&
                        x.StartOn.Minute == realWorkHour.StartOn.Minute &&
                        x.EndOn.Value.Date == realWorkHour.EndOn.Date &&
                        x.EndOn.Value.Hour == realWorkHour.EndOn.Hour &&
                        x.EndOn.Value.Minute == realWorkHour.EndOn.Minute &&
                        x.EmployeeId == realWorkHour.EmployeeId
                    );

                //if workhour exists for employee, edit it
                if (workHourToModify != null)
                {
                    logOriginalEntity = expandoService.GetCopyFrom(workHourToModify);
                    workHourToModify.StartOn = realWorkHour.NewStartOn;
                    workHourToModify.EndOn = realWorkHour.NewEndOn;
                    workHourToModify.Comments = realWorkHour.Comments;
                    logEditedEntity = expandoService.GetCopyFrom(workHourToModify);

                    _baseDataWork.Update(workHourToModify);


                    _logService.OnEditEntity("RealWorkHour", logOriginalEntity, logEditedEntity);
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
                        Comments = realWorkHour.Comments
                    });
                }
            }

            _baseDataWork.RealWorkHours.AddRange(realWorkHoursToSaveRange);
            if (realWorkHoursToSaveRange.Count > 0)
                _logService.OnCreateEntity("RealWorkHour", realWorkHoursToSaveRange as object);


            await _baseDataWork.SaveChangesAsync();

            return Ok(realWorkHoursToSaveRange);
        }


        // POST: api/realworkhours/addEmployeeWorkhours
        [HttpPost("addEmployeeWorkhours")]
        public async Task<ActionResult<WorkHour>> AddEmployeeWorkhours([FromBody] List<WorkHourApiCreate> workHours)
        {
            var realWorkHoursToSaveRange = new List<RealWorkHour>();
            var workHoursToSaveRange = new List<WorkHour>();

            foreach (var realWorkHour in workHours)
            {
                realWorkHoursToSaveRange.Add(new RealWorkHour()
                {
                    StartOn = realWorkHour.StartOn,
                    EndOn = realWorkHour.EndOn,
                    TimeShiftId = realWorkHour.TimeShiftId,
                    EmployeeId = realWorkHour.EmployeeId,
                    Comments = realWorkHour.Comments,
                    CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                    CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                    CreatedOn = DateTime.Now
                });
            }
            _baseDataWork.RealWorkHours.AddRange(realWorkHoursToSaveRange);
            _logService.OnCreateEntity("RealWorkHour", realWorkHoursToSaveRange as object);


            await _baseDataWork.SaveChangesAsync();

            return Ok(new { realWorkHoursToSaveRange, workHoursToSaveRange });
        }

        // POST: api/realworkhours/hasoverlap
        [HttpPost("hasoverlap")]
        public ActionResult<RealWorkHour> HasOverlap([FromBody] ApiRealWorkHourHasOverlap apiOverlap)
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
                        ErrorType = "error",
                        ErrorValue = "<br>Ο χρήστης αυτός έχει ήδη δηλωθεί για " +
                                        "αυτές τις ώρες ως άδεια",
                    });
            };

            return Ok(dataToReturn);
        }

        // POST: api/realworkhours/hasoverlap
        [HttpPost("HasOverlapRange")]
        public ActionResult<WorkHour> HasOverlapRange([FromBody] List<ApiRealWorkHoursHasOverlapRange> workHours)
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

        // POST: api/realworkhours/hasovertime
        [HttpPost("HasOvertime")]
        public ActionResult<WorkHour> HasOvertime([FromBody] List<ApiRealWorkHoursHasOvertimeRange> workHours)
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
    }
}
