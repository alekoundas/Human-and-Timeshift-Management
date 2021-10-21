using Bussiness;
using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using DataAccess.ViewModels.Entities.Amendment;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace WebApplication.Api
{
    [Route("api/amendments")]
    [ApiController]
    public class AmendmentsApiController : ControllerBase
    {
        private BaseDbContext _context { get; set; }
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public AmendmentsApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }



        // POST: api/amendments/GetForDayCell
        [HttpPost("GetForCell")]
        public async Task<ActionResult<Amendment>> GetForCell([FromBody] ApiAmendmentGetForCell viewModel)
        {
            var response = new List<object>();
            //var filter = PredicateBuilder.New<TimeShift>();

            var entities = await _baseDataWork.TimeShifts
                    .Query
                    .Include(x => x.RealWorkHours).ThenInclude(x => x.Amendment)
                    .Include(x => x.Amendments)
                    .Where(x => x.Id == viewModel.TimeShiftId)
                    .Select(x => new
                    {
                        RealWorkHours = x.RealWorkHours
                            .Where(y => y.EmployeeId == viewModel.EmployeeId)
                            .Where(y => y.StartOn.Day == viewModel.Day)
                            .ToList(),
                        Amendments = x.Amendments
                            .Where(y => y.EmployeeId == viewModel.EmployeeId)
                            .Where(y => y.RealWorkHourId == null)
                            .Where(y => y.NewStartOn.Day == viewModel.Day)
                            .ToList()
                    })
                    .ToListAsync();

            foreach (var entity in entities)
            {
                foreach (var amentment in entity.Amendments)
                {
                    //_context.Entry(amentment).Reload();
                    //_context.Entry(amentment).State = EntityState.Detached;
                    if (_context.Entry(amentment).State != EntityState.Unchanged)
                    {
                        // order has changed ...
                    }
                    response.Add(new
                    {
                        AmendmentId = amentment.Id,
                        EmployeeId = amentment.EmployeeId,
                        NewStartOn = amentment.NewStartOn,
                        NewEndOn = amentment.NewEndOn,
                        Comments = amentment.Comments,
                        CreatedOn = amentment.CreatedOn,
                        CreatedBy_FullName = amentment.CreatedBy_FullName
                    });
                }
                foreach (var realWorkHour in entity.RealWorkHours)
                {
                    //_context.Entry(realWorkHour).Reload();
                    _context.Entry(realWorkHour).State = EntityState.Detached;

                    response.Add(new
                    {
                        RealWorkHourId = realWorkHour.Id,
                        StartOn = realWorkHour.StartOn,
                        EndOn = realWorkHour.EndOn.Value,
                        Comments = realWorkHour.Comments,
                        EmployeeId = realWorkHour.EmployeeId,
                        CreatedOn = realWorkHour.CreatedOn,
                        CreatedBy_FullName = realWorkHour.CreatedBy_FullName,
                        NewStartOn = realWorkHour.Amendment?.NewStartOn,
                        NewEndOn = realWorkHour.Amendment?.NewEndOn,
                        //AmendmentId = realWorkHour.Amendment?.Id,
                        AmendmentComments = realWorkHour.Amendment?.Comments,
                        AmendmentCreatedOn = realWorkHour.Amendment?.CreatedOn,
                        AmendmentCreatedBy_FullName = realWorkHour.Amendment?.CreatedBy_FullName
                    });
                }
            }

            return Ok(response);
        }


        // POST: api/amendments/AmendmentDelete
        [HttpPost("AmendmentDelete")]
        public ActionResult<Amendment> AmendmentDelete([FromBody] ApiAmendmentDelete viewModel)
        {
            if (viewModel.AmendmentId > 0)
            {
                var amendment = _baseDataWork.Amendments
                    .Query
                    .FirstOrDefault(x => x.Id == viewModel.AmendmentId);

                _baseDataWork.Amendments.Remove(amendment);
            }
            else if (viewModel.RealWorkHourId > 0)
            {
                var amendment = _baseDataWork.RealWorkHours
                    .Query
                    .Include(x => x.Amendment)
                    .FirstOrDefault(x => x.Id == viewModel.RealWorkHourId)
                    .Amendment;

                _baseDataWork.Amendments.Remove(amendment);
            }

            _baseDataWork.SaveChanges();
            return Ok(new { ok = "okkk" });
        }


        // POST: api/amendments/AmendmentEdit
        [HttpPost("AmendmentEdit")]
        public ActionResult<Amendment> AmendmentEdit([FromBody] ApiAmendmentEdit viewModel)
        {
            var timeshift = _baseDataWork.TimeShifts
                .Query
                .FirstOrDefault(x => x.Id == viewModel.TimeShiftId);

            if (viewModel.newStartOn.Month != timeshift.Month ||
                viewModel.newStartOn.Year != timeshift.Year)
                return BadRequest();

            if (viewModel.newEndOn.Month != timeshift.Month &
                viewModel.newEndOn.Year != timeshift.Year)
                return BadRequest();


            //If realworkour exists
            if (viewModel.RealWorkHourId != null)
            {
                var realWorkHour = _baseDataWork.RealWorkHours
                    .Query
                    .Include(x => x.Amendment)
                    .FirstOrDefault(x => x.Id == viewModel.RealWorkHourId);

                //Add amendment
                if (realWorkHour.Amendment == null)
                {
                    realWorkHour.Amendment = new Amendment
                    {
                        NewStartOn = viewModel.newStartOn,
                        NewEndOn = viewModel.newEndOn,
                        EmployeeId = viewModel.EmployeeId,
                        Comments = viewModel.Comments,
                        TimeShiftId = viewModel.TimeShiftId,
                        RealWorkHourId = viewModel.RealWorkHourId,
                        CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                        CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                        CreatedOn = DateTime.Now
                    };
                }
                //Edit aamendment
                else
                {
                    realWorkHour.Amendment.NewEndOn = viewModel.newEndOn;
                    realWorkHour.Amendment.NewStartOn = viewModel.newStartOn;
                    realWorkHour.Amendment.Comments = viewModel.Comments;
                }
            }
            //If amendment exists, edit it
            else if (viewModel.AmendmentId != null)
            {

                var amendment = _baseDataWork.Amendments
                    .Query
                    .FirstOrDefault(x => x.Id == viewModel.AmendmentId);

                amendment.NewEndOn = viewModel.newEndOn;
                amendment.NewStartOn = viewModel.newStartOn;
                amendment.Comments = viewModel.Comments;
            }
            //Else add new amendment
            else
            {
                _baseDataWork.Amendments.Add(new Amendment
                {
                    NewStartOn = viewModel.newStartOn,
                    NewEndOn = viewModel.newEndOn,
                    EmployeeId = viewModel.EmployeeId,
                    Comments = viewModel.Comments,
                    TimeShiftId = viewModel.TimeShiftId,
                    CreatedBy_FullName = HttpAccessorService.GetLoggeInUser_FullName,
                    CreatedBy_Id = HttpAccessorService.GetLoggeInUser_Id,
                    CreatedOn = DateTime.Now
                });
            }

         


            var status = _baseDataWork.SaveChanges();
            return Ok(new { status = status });
        }
    }
}
