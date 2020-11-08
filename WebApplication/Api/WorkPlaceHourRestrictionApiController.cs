using Bussiness;
using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Datatable;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication.Utilities;

namespace WebApplication.Api
{
    [Route("api/WorkPlaceHourRestrictions")]
    [ApiController]
    public class WorkPlaceHourRestrictionApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public WorkPlaceHourRestrictionApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // DELETE: api/LeaveTypeApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LeaveType>> DeleteLeaveType(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);
            if (leaveType == null)
                return NotFound();

            _context.LeaveTypes.Remove(leaveType);
            await _context.SaveChangesAsync();

            return leaveType;
        }


        // POST: api/datatable
        [HttpPost("datatable")]
        public async Task<ActionResult<LeaveType>> Datatable([FromBody] Datatable datatable)
        {
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var orderDirection = datatable.Order[0].Dir;
            var filter = PredicateBuilder.New<WorkPlaceHourRestriction>();
            filter = filter.And(GetSearchFilter(datatable));

            var includes = new List<Func<IQueryable<WorkPlaceHourRestriction>, IIncludableQueryable<WorkPlaceHourRestriction, object>>>();
            var workPlaceHourRestrictions = new List<WorkPlaceHourRestriction>();

            if (datatable.Predicate == "WorkPlaceHourRestrictionIndex")
            {
                includes.Add(x => x.Include(y => y.WorkPlace));
                workPlaceHourRestrictions = await _baseDataWork.WorkPlaceHourRestrictions
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            }

            var mapedData = MapResults(workPlaceHourRestrictions, datatable);

            var total = await _baseDataWork.WorkPlaceHourRestrictions.CountAllAsyncFiltered(filter);
            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        protected IEnumerable<ExpandoObject> MapResults(IEnumerable<WorkPlaceHourRestriction> results, Datatable datatable)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<WorkPlaceHourRestriction>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var workPlaceHourRestriction in results)
            {
                var expandoObj = expandoObject.GetCopyFrom<WorkPlaceHourRestriction>(workPlaceHourRestriction);
                var dictionary = (IDictionary<string, object>)expandoObj;

                if (datatable.Predicate == "WorkPlaceHourRestrictionIndex")
                {
                    dictionary.Add("Buttons", dataTableHelper.GetButtons("WorkPlaceHourRestriction", "WorkPlaceHourRestrictions", workPlaceHourRestriction.Id.ToString()));
                    returnObjects.Add(expandoObj);
                }
            }

            return returnObjects;
        }



        // POST: api/datatable
        [HttpPost("ValidateHoursToWorkPlaceHourRestriction")]
        public async Task<ActionResult<RealWorkHour>> ValidateHoursToWorkPlaceHourRestriction([FromBody] ApiHourRestrictionValidate validateVM)
        {
            var dataToReturn = new ApiHourRestrictionValidateResponse();
            var day = validateVM.StartOn.Day;
            var month = validateVM.StartOn.Month;
            var year = validateVM.StartOn.Year;
            var secondsToSubmit = validateVM.EmployeeIds.Count() *
                Math.Abs(validateVM.StartOn.Subtract(validateVM.EndOn).TotalSeconds);

            if (!_baseDataWork.WorkPlaceHourRestrictions.ValidateMaxHours(
                validateVM.TimeShiftId, year, month, day, secondsToSubmit))
                dataToReturn = new ApiHourRestrictionValidateResponse
                {
                    ErrorType = "warning",
                    ErrorValue = "<br>Μέγιστο επιτρεπτό όριο εισαγωγής:" +
                        _baseDataWork.WorkPlaceHourRestrictions.GetDayMaxTime(
                            validateVM.TimeShiftId, year, month, day) +
                        "<br>Υπάρχων ώρες μέρας:" +
                        _baseDataWork.WorkPlaceHourRestrictions.GetDayRealWorkHoursTime(
                            validateVM.TimeShiftId, year, month, day) +
                        "<br>Πρόκειται να εισαχθούν:" +
                        GetTime(secondsToSubmit)
                };

            return Ok(dataToReturn);
        }

        private Func<IQueryable<WorkPlaceHourRestriction>, IOrderedQueryable<WorkPlaceHourRestriction>> SetOrderBy(string columnName, string orderDirection)
        {
            if (columnName == "WorkPlaceName")
                return x => x.OrderBy(y => y.WorkPlace.Title);
            else if (columnName != "")
                return x => x.OrderBy(columnName + " " + orderDirection.ToUpper());
            else
                return null;
        }

        private Expression<Func<WorkPlaceHourRestriction, bool>> GetSearchFilter(Datatable datatable)
        {
            var filter = PredicateBuilder.New<WorkPlaceHourRestriction>();
            if (datatable.Search.Value != null)
            {
                foreach (var column in datatable.Columns)
                {
                    if (column.Data == "WorkPlaceName")
                        filter = filter.Or(x => x.WorkPlace.Title.Contains(datatable.Search.Value));
                    if (column.Data == "Month")
                        filter = filter.Or(x => x.Month.ToString().Contains(datatable.Search.Value));
                    if (column.Data == "Year")
                        filter = filter.Or(x => x.Year.ToString().Contains(datatable.Search.Value));
                }

            }
            else
                filter = filter.And(x => true);

            return filter;
        }
        private static string GetTime(double seconds)
        {
            var hours = (seconds / 3600).ToString();
            var minutes = (seconds % 3600).ToString();

            if (hours.Length == 1)
                hours = "0" + hours;
            if (minutes.Length == 1)
                minutes = "0" + minutes;

            return hours + ":" + minutes;

        }
        private bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.Id == id);
        }
    }
}
