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
using WebApplication.Utilities;
using DataAccess.Models.Datatable;
using System.Dynamic;
using Bussiness.Service;

namespace WebApplication.Api
{
    [Route("api/workplaces")]
    [ApiController]
    public class WorkPlacesApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;

        public WorkPlacesApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: api/workplaces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkPlace>>> GetWorkPlaces()
        {
            return await _context.WorkPlaces.ToListAsync();
        }

        // GET: api/workplaces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkPlace>> GetWorkPlace(int id)
        {
            var workPlace = await _context.WorkPlaces.FindAsync(id);

            if (workPlace == null)
                return NotFound();

            return workPlace;
        }

        // PUT: api/workplaces/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkPlace(int id, WorkPlace workPlace)
        {
            if (id != workPlace.Id)
                return BadRequest();

            _context.Entry(workPlace).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkPlaceExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/workplaces
        [HttpPost]
        public async Task<ActionResult<WorkPlace>> PostWorkPlace(WorkPlace workPlace)
        {
            _context.WorkPlaces.Add(workPlace);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkPlace", new { id = workPlace.Id }, workPlace);
        }

        // DELETE: api/workplaces/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<WorkPlace>> DeleteWorkPlace(int id)
        {
            var workPlace = await _context.WorkPlaces.FindAsync(id);
            if (workPlace == null)
            {
                return NotFound();
            }

            _context.WorkPlaces.Remove(workPlace);
            await _context.SaveChangesAsync();

            return workPlace;
        }


        // GET: api/workplaces/select2
        [HttpGet("select2")]
        public async Task<ActionResult<WorkPlace>> select2(string search, int page)
        {
            var workPlaces = new List<WorkPlace>();
            var select2Helper = new Select2Helper();
            if (string.IsNullOrWhiteSpace(search))
            {
               workPlaces = (List<WorkPlace>)await _baseDataWork
                    .WorkPlaces
                    .GetPaggingWithFilter(null, null, null, 10, page);

                return Ok(select2Helper.CreateWorkplacesResponse(workPlaces));
            }

            workPlaces = await _baseDataWork
                .WorkPlaces
                .GetPaggingWithFilter(null, x => x.Title.Contains(search), null, 10, page);

            return Ok(select2Helper.CreateWorkplacesResponse(workPlaces));
        }

        [HttpGet("manageworkplaceemployee/{employeeId}/{workPlaceId}/{toggleState}")]
        public async Task<ActionResult<WorkPlace>> ManageWorkPlaceEmployee(int employeeId, int workPlaceId, string toggleState)
        {
            var employee = await _baseDataWork.Employees.FindAsync(employeeId);
            var workPlace = await _baseDataWork.WorkPlaces.FindAsync(workPlaceId);

            if (employee == null || workPlace == null)
                return NotFound();

            if(toggleState == "true" && _baseDataWork.EmployeeWorkPlaces
                .Any(x => x.EmployeeId == employeeId && x.WorkPlaceId == workPlaceId))
                    return NotFound();

            if (toggleState == "true")
                _baseDataWork.EmployeeWorkPlaces.Add(new EmployeeWorkPlace() 
                {
                    EmployeeId=employeeId,
                    WorkPlaceId= workPlaceId,
                    CreatedOn=DateTime.Now
                });

            else
                _baseDataWork.EmployeeWorkPlaces.Remove(
                     _baseDataWork.EmployeeWorkPlaces
                        .Where(x=>x.EmployeeId==employeeId&&x.WorkPlaceId==workPlaceId)
                        .FirstOrDefault());

            try
            {

            await _baseDataWork.SaveChangesAsync();
            }
            catch (Exception /*ex*/)
            {

                throw;
            }

            return workPlace;
        }

        // POST: api/companies/getdatatable
        [HttpPost("getdatatable")]
        public async Task<ActionResult<WorkPlace>> getdatatable([FromBody] Datatable datatable)
        {
            var total = await _baseDataWork.Companies.CountAllAsync();
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var isDescending = datatable.Order[0].Dir == "desc";

            //TODO: order by
            var workPlaces = await _baseDataWork.WorkPlaces.GetWithPagging(null, pageSize, pageIndex);

            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            var mapedData = MapResults(workPlaces);

            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        protected IEnumerable<ExpandoObject> MapResults(IEnumerable<WorkPlace> results)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<WorkPlace>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var result in results)
            {
                var expandoObj = expandoObject.GetCopyFrom<WorkPlace>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;
                dictionary.Add("Buttons", dataTableHelper.GetButtons("WorkPlace", "WorkPlaces", result.Id.ToString()));
                returnObjects.Add(expandoObj);
            }

            return returnObjects;
        }

        private bool WorkPlaceExists(int id)
        {
            return _context.WorkPlaces.Any(e => e.Id == id);
        }
    }
}
