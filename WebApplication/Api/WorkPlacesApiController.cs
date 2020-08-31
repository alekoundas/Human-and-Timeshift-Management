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
using LinqKit;
using DataAccess.Models.Select2;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

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
        public async Task<ActionResult<WorkPlace>> Select2(string search, int page)
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

        // POST: api/workplaces/select2filtered
        [HttpPost("select2filtered")]
        public async Task<ActionResult<WorkPlace>> Select2Filtered([FromBody] Select2FilteredGet select2)
        {
            var workPlaces = new List<WorkPlace>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<WorkPlace>();
            filter = filter.And(x => true);

            if (select2.ExistingIds?.Count > 0)
                foreach (var workPlaceId in select2.ExistingIds)
                    filter = filter.And(x => x.Id != workPlaceId);

            if (!string.IsNullOrWhiteSpace(select2.Search))
                filter = filter.And(x => x.Title.Contains(select2.Search));

            workPlaces = await _baseDataWork
             .WorkPlaces
             .GetPaggingWithFilter(null, filter, null, 10, select2.Page);

            return Ok(select2Helper.CreateWorkplacesResponse(workPlaces));
        }



        [HttpGet("manageworkplaceemployee/{employeeId}/{workPlaceId}/{toggleState}")]
        public async Task<ActionResult<WorkPlace>> ManageWorkPlaceEmployee(int employeeId, int workPlaceId, string toggleState)
        {
            var employee = await _baseDataWork.Employees.FindAsync(employeeId);
            var workPlace = await _baseDataWork.WorkPlaces.FindAsync(workPlaceId);

            if (employee == null || workPlace == null)
                return NotFound();

            if (toggleState == "true" && _baseDataWork.EmployeeWorkPlaces
                .Any(x => x.EmployeeId == employeeId && x.WorkPlaceId == workPlaceId))
                return NotFound();

            if (toggleState == "true")
                _baseDataWork.EmployeeWorkPlaces.Add(new EmployeeWorkPlace()
                {
                    EmployeeId = employeeId,
                    WorkPlaceId = workPlaceId,
                    CreatedOn = DateTime.Now
                });

            else
                _baseDataWork.EmployeeWorkPlaces.Remove(
                     _baseDataWork.EmployeeWorkPlaces
                        .Where(x => x.EmployeeId == employeeId && x.WorkPlaceId == workPlaceId)
                        .FirstOrDefault());


            await _baseDataWork.SaveChangesAsync();

            return workPlace;
        }

        [HttpGet("manageworkplacecustomer/{customerId}/{workPlaceId}/{toggleState}")]
        public async Task<ActionResult<Object>> ManageWorkPlaceCustomer(int customerId, int workPlaceId, string toggleState)
        {
            var workPlace = await _baseDataWork.WorkPlaces
                .FirstOrDefaultAsync(x => x.Id == workPlaceId);

            if (toggleState == "true")
                workPlace.Customer = await _baseDataWork.Customers
                    .FirstOrDefaultAsync(x => x.Id == customerId);
            else
                workPlace.Customer = null;

            _baseDataWork.Update(workPlace);
            await _baseDataWork.SaveChangesAsync();

            //TempData["StatusMessage"] = "αμ μπλου ντα μπου ντι ντα μπουνται";
            return new { skata = "Polla Skata" };
        }

        // POST: api/companies/datatable
        [HttpPost("datatable")]
        public async Task<ActionResult<WorkPlace>> Datatable([FromBody] Datatable datatable)
        {
            var total = await _baseDataWork.WorkPlaces.CountAllAsync();
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var orderDirection = datatable.Order[0].Dir;

            var includes = new List<Func<IQueryable<WorkPlace>, IIncludableQueryable<WorkPlace, object>>>();
            //includes.Add(x => x.Include(y => y.Specialization));

            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            var workPlaces = new List<WorkPlace>();


            if (datatable.Predicate == "WorkPlaceIndex")
            {
                includes.Add(x => x.Include(y => y.Customer));
                Expression<Func<WorkPlace, bool>> filter = x => true;

                workPlaces = await _baseDataWork.WorkPlaces
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            }
            if (datatable.Predicate == "CustomerEdit")
            {
                includes.Add(x => x.Include(y => y.Customer));
                Expression<Func<WorkPlace, bool>> filter = x => true;

                workPlaces = await _baseDataWork.WorkPlaces
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            }  
            if (datatable.Predicate == "EmployeeEdit")
            {
                includes.Add(x => x.Include(y => y.Customer));
                includes.Add(x => x.Include(y => y.EmployeeWorkPlaces).ThenInclude(z=>z.Employee));
                Expression<Func<WorkPlace, bool>> filter = x => x.Customer.Company!=null;


                workPlaces = await _baseDataWork.WorkPlaces
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            } 
            if (datatable.Predicate == "EmployeeDetails")
            {
                includes.Add(x => x.Include(y => y.Customer));
                includes.Add(x => x.Include(y => y.EmployeeWorkPlaces).ThenInclude(z=>z.Employee));
                Expression<Func<WorkPlace, bool>> filter = x => 
                x.EmployeeWorkPlaces.Any(y=>y.EmployeeId==datatable.GenericId);

                workPlaces = await _baseDataWork.WorkPlaces
                    .GetPaggingWithFilter(SetOrderBy(columnName, orderDirection), filter, includes, pageSize, pageIndex);
            }

            var mapedData = MapResults(workPlaces, datatable);

            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        protected IEnumerable<ExpandoObject> MapResults(IEnumerable<WorkPlace> results, Datatable datatable)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<WorkPlace>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var workplace in results)
            {
                var expandoObj = expandoObject.GetCopyFrom<WorkPlace>(workplace);
                var dictionary = (IDictionary<string, object>)expandoObj;
                 if (datatable.Predicate == "WorkPlaceIndex")
                {
                    dictionary.Add("Buttons", dataTableHelper.GetButtons(
                        "WorkPlace", "WorkPlaces", workplace.Id.ToString()));
                    dictionary.Add("CustomerFullName", workplace.Customer?.FullName);
                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "CustomerEdit")
                {
                    var apiUrl = UrlHelper.CustomerWorkPlace(datatable.GenericId,
                        workplace.Id);

                    if (workplace.Customer != null)
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "WorkPlace", apiUrl, "checked"));
                    else
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "WorkPlace", apiUrl, ""));

                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "EmployeeEdit")
                {
                    var apiUrl = UrlHelper.EmployeeWorkPlace(datatable.GenericId,
                        workplace.Id);

                    dictionary.Add("CustomerFullName", workplace.Customer?.FullName);

                    if (workplace.EmployeeWorkPlaces.Any(x=>x.EmployeeId== datatable.GenericId))
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "WorkPlace", apiUrl, "checked"));
                    else
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "WorkPlace", apiUrl, ""));

                    returnObjects.Add(expandoObj);
                }
                else if (datatable.Predicate == "EmployeeDetails")
                {
                    var apiUrl = UrlHelper.EmployeeWorkPlace(datatable.GenericId,
                        workplace.Id);

                    dictionary.Add("CustomerFullName", workplace.Customer?.FullName);

                    if (workplace.EmployeeWorkPlaces.Any(x=>x.EmployeeId== datatable.GenericId))
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "WorkPlace", apiUrl, "checked",true));
                    else
                        dictionary.Add("IsInWorkPlace", dataTableHelper.GetToggle(
                            "WorkPlace", apiUrl, "",true));

                    returnObjects.Add(expandoObj);
                }

            }

            return returnObjects;
        }

        private Func<IQueryable<WorkPlace>, IOrderedQueryable<WorkPlace>> SetOrderBy(string columnName, string orderDirection)
        {
            if (columnName != "")
                return x => x.OrderBy(columnName + " " + orderDirection.ToUpper());
            else
                return null;
        }

        private bool WorkPlaceExists(int id)
        {
            return _context.WorkPlaces.Any(e => e.Id == id);
        }
    }
}
