using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess;
using DataAccess.Models.Entity;
using WebApplication.Utilities;
using System.Dynamic;
using DataAccess.Models.Datatable;
using Bussiness.Service;
using System.Linq.Dynamic.Core;
using Bussiness;
using Microsoft.EntityFrameworkCore.Query;
using LinqKit;

namespace WebApplication.Api
{
    [Route("api/leavetypes")]
    [ApiController]
    public class LeaveTypeApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public LeaveTypeApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: api/LeaveTypeApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaveType>>> GetLeaveTypes()
        {
            return await _context.LeaveTypes.ToListAsync();
        }

        // GET: api/LeaveTypeApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LeaveType>> GetLeaveType(int id)
        {
            var leaveType = await _context.LeaveTypes.FindAsync(id);

            if (leaveType == null)
                return NotFound();

            return leaveType;
        }

        // PUT: api/LeaveTypeApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLeaveType(int id, LeaveType leaveType)
        {
            if (id != leaveType.Id)
                return BadRequest();

            _context.Entry(leaveType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveTypeExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/LeaveTypeApi
        [HttpPost]
        public async Task<ActionResult<LeaveType>> PostLeaveType(LeaveType leaveType)
        {
            _context.LeaveTypes.Add(leaveType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLeaveType", new { id = leaveType.Id }, leaveType);
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


        // GET: api/Select2
        [HttpGet("select2")]
        public async Task<ActionResult<LeaveType>> Select2(string search, int page)
        {
            var specializations = new List<LeaveType>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<LeaveType>();
            filter = filter.And(x => true);

            if (string.IsNullOrWhiteSpace(search))
                specializations = await _baseDataWork.LeaveTypes
                    .GetPaggingWithFilter(null, null, null, 10, page);
            else
            {
                filter = filter.And(x => x.Name.Contains(search));

                specializations = await _baseDataWork.LeaveTypes
                    .GetPaggingWithFilter(null, filter, null, 10, page);
            }
            var total = await _baseDataWork.LeaveTypes.CountAllAsyncFiltered(filter);
            var hasMore = (page * 10) < total;

            return Ok(select2Helper.CreateLeaveTypeResponse(specializations, hasMore));
        }

        // POST: api/datatable
        [HttpPost("datatable")]
        public async Task<ActionResult<LeaveType>> Datatable([FromBody] Datatable datatable)
        {

            var total = await _baseDataWork.Specializations.CountAllAsync();
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var orderDirection = datatable.Order[0].Dir;

            var includes = new List<Func<IQueryable<LeaveType>, IIncludableQueryable<LeaveType, object>>>();
            var leaveTypes = new List<LeaveType>();

            if (datatable.Predicate == "LeaveTypeIndex")
            {
                leaveTypes = await _baseDataWork.LeaveTypes
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), null, includes, pageSize, pageIndex);
            }

            var mapedData = MapResults(leaveTypes, datatable);

            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        protected IEnumerable<ExpandoObject> MapResults(IEnumerable<LeaveType> results, Datatable datatable)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<LeaveType>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var leaveTypes in results)
            {
                var expandoObj = expandoObject.GetCopyFrom<LeaveType>(leaveTypes);
                var dictionary = (IDictionary<string, object>)expandoObj;

                if (datatable.Predicate == "LeaveTypeIndex")
                {
                    dictionary.Add("Buttons", dataTableHelper.GetButtons("LeaveType", "LeaveTypes", leaveTypes.Id.ToString()));
                    returnObjects.Add(expandoObj);
                }
            }

            return returnObjects;
        }


        private Func<IQueryable<LeaveType>, IOrderedQueryable<LeaveType>> SetOrderBy(string columnName, string orderDirection)
        {
            if (columnName != "")
                return x => x.OrderBy(columnName + " " + orderDirection.ToUpper());
            else
                return null;
        }


        private bool LeaveTypeExists(int id)
        {
            return _context.LeaveTypes.Any(e => e.Id == id);
        }
    }
}
