using DataAccess;
using DataAccess.Models.Entity;
using DataAccess.ViewModels.Validation;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApplication.Api
{
    [Route("api/validations")]
    [ApiController]
    public class ValidationsApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public ValidationsApiController(BaseDbContext BaseDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }


        // POST: api/validations/HasOverlapRangeRealWorkHours
        [HttpPost("HasOverlapRangeRealWorkHours")]
        public ActionResult<ApiValidationResult> HasOverlapRangeRealWorkHours([FromBody] List<ApiValidateHasOverlapRange> workHours)
        {
            List<ApiValidationResult> response = new List<ApiValidationResult>();
            foreach (var workHour in workHours)
            {
                workHour.EmployeeIds.ForEach(id =>
                {
                    var filter = PredicateBuilder.New<RealWorkHour>();
                    var filterLeaves = PredicateBuilder.New<Leave>();

                    filter = filter.And(x => workHour.StartOn <= x.EndOn && x.StartOn <= workHour.EndOn);
                    filter = filter.And(x => x.EmployeeId == id);

                    filterLeaves = filterLeaves.And(x => workHour.StartOn <= x.EndOn && x.StartOn <= workHour.EndOn);
                    filterLeaves = filterLeaves.And(x => x.EmployeeId == id);

                    if (workHour.IsEdit)
                        filter = filter.And(x => x.StartOn != workHour.ExcludeStartOn && x.EndOn != workHour.ExcludeEndOn);


                    if (_baseDataWork.RealWorkHours.Any(filter))
                        response.Add(new ApiValidationResult
                        {
                            EmployeeId = id,
                            ResponseType = "error",
                            ResponseValue = "Ο χρήστης αυτός έχει ήδη δηλωθεί για αυτές τις ώρες"
                        });
                    else if (_baseDataWork.Leaves.Any(filterLeaves))
                        response.Add(new ApiValidationResult
                        {
                            EmployeeId = id,
                            ResponseType = "error",
                            ResponseValue = "<br>Ο χρήστης αυτός έχει ήδη δηλωθεί για " +
                                            "αυτές τις ώρες ως άδεια",
                        });
                    else
                        response.Add(new ApiValidationResult
                        {
                            EmployeeId = id,
                            ResponseType = "success",
                            ResponseValue = ""

                        });
                });
            }
            return Ok(response);
        }

        // POST: api/validations/HasOverlapRangeWorkHours
        [HttpPost("HasOverlapRangeWorkHours")]
        public ActionResult<ApiValidationResult> HasOverlapRangeWorkHours([FromBody] List<ApiValidateHasOverlapRange> workHours)
        {
            List<ApiValidationResult> response = new List<ApiValidationResult>();
            foreach (var workHour in workHours)
            {
                workHour.EmployeeIds.ForEach(id =>
                {
                    var filter = PredicateBuilder.New<WorkHour>();
                    var filterLeaves = PredicateBuilder.New<Leave>();

                    filter = filter.And(x => workHour.StartOn <= x.EndOn && x.StartOn <= workHour.EndOn);
                    filter = filter.And(x => x.EmployeeId == id);

                    filterLeaves = filterLeaves.And(x => workHour.StartOn <= x.EndOn && x.StartOn <= workHour.EndOn);
                    filterLeaves = filterLeaves.And(x => x.EmployeeId == id);

                    if (workHour.IsEdit)
                        filter = filter.And(x => x.StartOn != workHour.ExcludeStartOn && x.EndOn != workHour.ExcludeEndOn);


                    if (_baseDataWork.WorkHours.Any(filter))
                        response.Add(new ApiValidationResult
                        {
                            EmployeeId = id,
                            ResponseType = "error",
                            ResponseValue = "Ο χρήστης αυτός έχει ήδη δηλωθεί για αυτές τις ώρες"
                        });
                    else if (_baseDataWork.Leaves.Any(filterLeaves))
                        response.Add(new ApiValidationResult
                        {
                            EmployeeId = id,
                            ResponseType = "error",
                            ResponseValue = "<br>Ο χρήστης αυτός έχει ήδη δηλωθεί για " +
                                            "αυτές τις ώρες ως άδεια",
                        });
                    else
                        response.Add(new ApiValidationResult
                        {
                            EmployeeId = id,
                            ResponseType = "success",
                            ResponseValue = ""

                        });
                });
            }
            return Ok(response);
        }

        // POST: api/validations/HasOverTimeRangeRealWorkHours
        [HttpPost("HasOverTimeRangeRealWorkHours")]
        public ActionResult<ApiValidationResult> HasOverTimeRangeRealWorkHours([FromBody] List<ApiValidateHasOverlapRange> workHours)
        {
            List<ApiValidationResult> response = new List<ApiValidationResult>();
            foreach (var workHour in workHours)
            {
                workHour.EmployeeIds.ForEach(id =>
                {
                    var filter = PredicateBuilder.New<RealWorkHour>();

                    workHour.StartOn = workHour.StartOn.AddHours(-11);
                    workHour.EndOn = workHour.StartOn.AddHours(11);

                    filter = filter.And(x => workHour.StartOn <= x.EndOn && x.StartOn <= workHour.EndOn);
                    filter = filter.And(x => x.EmployeeId == id);

                    if (workHour.IsEdit)
                        filter = filter.And(x => x.StartOn != workHour.ExcludeStartOn && x.EndOn != workHour.ExcludeEndOn);

                    if (_baseDataWork.RealWorkHours.Any(filter))
                        response.Add(new ApiValidationResult
                        {
                            EmployeeId = id,
                            ResponseType = "warning",
                            ResponseValue = "Ο υπάλληλος έχει ήδη μια βάρδια με λιγότερο απο 11 ώρες διαφορά"
                        });
                    else
                        response.Add(new ApiValidationResult
                        {
                            EmployeeId = id,
                            ResponseType = "success",
                            ResponseValue = ""
                        });
                });
            }
            return Ok(response);
        }

        // POST: api/validations/HasOverTimeRangeRealWorkHours
        [HttpPost("HasOverTimeRangeWorkHours")]
        public ActionResult<ApiValidationResult> HasOverTimeRangeWorkHours([FromBody] List<ApiValidateHasOverlapRange> workHours)
        {
            List<ApiValidationResult> response = new List<ApiValidationResult>();
            foreach (var workHour in workHours)
            {
                workHour.EmployeeIds.ForEach(id =>
                {
                    var filter = PredicateBuilder.New<WorkHour>();

                    workHour.StartOn = workHour.StartOn.AddHours(-11);
                    workHour.EndOn = workHour.StartOn.AddHours(11);

                    filter = filter.And(x => workHour.StartOn <= x.EndOn && x.StartOn <= workHour.EndOn);
                    filter = filter.And(x => x.EmployeeId == id);

                    if (workHour.IsEdit)
                        filter = filter.And(x => x.StartOn != workHour.ExcludeStartOn && x.EndOn != workHour.ExcludeEndOn);

                    if (_baseDataWork.WorkHours.Any(filter))
                        response.Add(new ApiValidationResult
                        {
                            EmployeeId = id,
                            ResponseType = "warning",
                            ResponseValue = "Ο υπάλληλος έχει ήδη μια βάρδια με λιγότερο απο 11 ώρες διαφορά"
                        });
                    else
                        response.Add(new ApiValidationResult
                        {
                            EmployeeId = id,
                            ResponseType = "success",
                            ResponseValue = ""
                        });
                });
            }
            return Ok(response);
        }


    }
}
