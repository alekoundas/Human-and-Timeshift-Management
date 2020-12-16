using Bussiness;
using Bussiness.Service;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace WebApplication.Api
{
    [Route("api/leaves")]
    [ApiController]
    public class LeavesApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public LeavesApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: api/leaves
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkHour>>> GetLeaves()
        {
            //return await _context.Leaves.ToListAsync();
            return await _baseDataWork.WorkHours.Where(x =>
        x.TimeShiftId == 13 &&
        x.StartOn.Year == 2020 &&
        x.StartOn.Month == 10).Select(x => new WorkHour
        {
            StartOn = x.StartOn,
            EndOn = x.EndOn,
            IsDayOff = x.IsDayOff,
            EmployeeId = x.EmployeeId,
            Comments = x.Comments,
            TimeShiftId = x.TimeShiftId
        }).ToDynamicListAsync<WorkHour>();
        }

        // GET: api/leaves/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Leave>> GetLeave(int id)
        {
            var leave = await _context.Leaves.FindAsync(id);

            if (leave == null)
                return NotFound();

            return leave;
        }

        // PUT: api/leaves/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeave(int id, Leave leave)
        {
            if (id != leave.Id)
                return BadRequest();

            _context.Entry(leave).State = EntityState.Modified;

            try
            {
                await _baseDataWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/leaves/postleave
        //[HttpPost("postleave")]
        //public async Task<ActionResult<Leave>> PostLeave(LeaveCreate apiLeave)
        //{
        //    if (apiLeave.EmployeeIds.Count() > 0)
        //        apiLeave.EmployeeIds.ForEach(id => _baseDataWork.Leaves.Add(
        //            new Leave
        //            {
        //                StartOn = apiLeave.StartOn,
        //                EndOn = apiLeave.EndOn,
        //                ApprovedBy = apiLeave.ApprovedBy,
        //                Description = apiLeave.Description,
        //                EmployeeId = id,
        //                LeaveTypeId = apiLeave.LeaveTypeId
        //            }));
        //    await _baseDataWork.SaveChangesAsync();

        //    return Ok("Success my dudes");
        //}

        // DELETE: api/leaves/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteViewModel>> Delete(int id)
        {
            var response = new DeleteViewModel();
            var leave = await _context.Leaves
                .Include(x => x.Employee)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (leave == null)
                return NotFound();

            _context.Leaves.Remove(leave);
            var status = await _baseDataWork.SaveChangesAsync();

            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Η άδεια του" +
                    leave.Employee.FullName +
                    " διαγράφηκε με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Η άδεια του" +
                     leave.Employee.FullName +
                    " ΔΕΝ διαγράφηκε!";
            }

            response.ResponseTitle = "Διαγραφή άδειας";
            response.Entity = leave;
            return response;
        }


        //TODO: Fix async in repo
        // POST: api/leaves/hasoverlap
        [HttpPost("hasoverlap")]
        public async Task<ActionResult<Leave>> HasOverlap(ApiLeavesHasOverlap apiLeave)
        {
            var dataToReturn = new List<ApiLeavesHasOverlapResponse>();

            if (apiLeave.EmployeeIds.Count() > 0)
                foreach (var id in apiLeave.EmployeeIds)
                {
                    dataToReturn.AddRange(await _baseDataWork.DateHasOverlap(
                        apiLeave.StartOn, apiLeave.EndOn, id) ??
                        new List<ApiLeavesHasOverlapResponse>());
                }

            return Ok(dataToReturn);
        }
        // POST: api/datatable
        [HttpPost("datatable")]
        public async Task<ActionResult<Leave>> Datatable([FromBody] Datatable datatable)
        {
            var results = (await new DataTableService(datatable, _baseDataWork, HttpContext)
                .ConvertData<Leave>())
                .CompleteResponse<Leave>();

            return Ok(results);

            //var pageSize = datatable.Length;
            //var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            //var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            //var orderDirection = datatable.Order[0].Dir;

            //var filter = PredicateBuilder.New<Leave>();
            //filter = filter.And(GetSearchFilter(datatable));

            //var includes = new List<Func<IQueryable<Leave>, IIncludableQueryable<Leave, object>>>();
            //var leaves = new List<Leave>();

            //if (datatable.Predicate == "LeaveIndex")
            //{
            //    includes.Add(x => x.Include(y => y.LeaveType));
            //    includes.Add(x => x.Include(y => y.Employee));
            //    leaves = await _baseDataWork.Leaves
            //        .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            //}

            //var mapedData = MapResults(leaves, datatable);

            //var total = await _baseDataWork.Leaves.CountAllAsyncFiltered(filter);
            //var dataTableHelper = new DataTableHelper<ExpandoObject>();
            //return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        //protected IEnumerable<ExpandoObject> MapResults(IEnumerable<Leave> results, Datatable datatable)
        //{
        //    var expandoObject = new ExpandoService();
        //    var dataTableHelper = new DataTableHelper<Leave>();
        //    List<ExpandoObject> returnObjects = new List<ExpandoObject>();
        //    foreach (var leaves in results)
        //    {
        //        var expandoObj = expandoObject.GetCopyFrom<Leave>(leaves);
        //        var dictionary = (IDictionary<string, object>)expandoObj;

        //        if (datatable.Predicate == "LeaveIndex")
        //        {
        //            dictionary.Add("EmployeeFullName", leaves.Employee.FullName);
        //            dictionary.Add("LeaveTypeName", leaves.LeaveType.Name);
        //            dictionary.Add("Buttons", dataTableHelper.GetButtons("Leave", "Leaves", leaves.Id.ToString()));
        //            returnObjects.Add(expandoObj);
        //        }
        //    }

        //    return returnObjects;
        //}


        //private Func<IQueryable<Leave>, IOrderedQueryable<Leave>> SetOrderBy(string columnName, string orderDirection)
        //{
        //    if (columnName != "")
        //        return x => x.OrderBy(columnName + " " + orderDirection.ToUpper());
        //    else
        //        return null;
        //}
        //private Expression<Func<Leave, bool>> GetSearchFilter(Datatable datatable)
        //{
        //    var filter = PredicateBuilder.New<Leave>();
        //    if (datatable.Search.Value != null)
        //    {
        //        foreach (var column in datatable.Columns)
        //        {
        //            if (column.Data == "StartOn")
        //                filter = filter.Or(x => x.StartOn.ToString().Contains(datatable.Search.Value));
        //            if (column.Data == "EndOn")
        //                filter = filter.Or(x => x.EndOn.ToString().Contains(datatable.Search.Value));
        //            if (column.Data == "EmployeeFullName")
        //                filter = filter.Or(x => x.Employee.FirstName.Contains(datatable.Search.Value) || x.Employee.LastName.Contains(datatable.Search.Value));
        //            if (column.Data == "LeaveTypeName")
        //                filter = filter.Or(x => x.LeaveType.Name.Contains(datatable.Search.Value));
        //            if (column.Data == "ApprovedBy")
        //                filter = filter.Or(x => x.ApprovedBy.Contains(datatable.Search.Value));
        //        }

        //    }
        //    else
        //        filter = filter.And(x => true);

        //    return filter;
        //}
        private bool LeaveExists(int id)
        {
            return _context.Leaves.Any(e => e.Id == id);
        }
    }
}
