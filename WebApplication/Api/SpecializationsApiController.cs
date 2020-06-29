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
using DataAccess.Models.Datatable;
using WebApplication.Utilities;
using System.Dynamic;
using Bussiness.Service;
using DataAccess.Models.Select2;

namespace WebApplication.Api
{
    [Route("api/specializations")]
    [ApiController]
    public class SpecializationsApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public SpecializationsApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: api/specializations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Specialization>>> GetSpecializations()
        {
            return await _context.Specializations.ToListAsync();
        }

        // GET: api/specializations/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Specialization>> GetSpecialization(int id)
        {
            var specialization = await _context.Specializations.FindAsync(id);

            if (specialization == null)
            {
                return NotFound();
            }

            return specialization;
        }

        // PUT: api/specializations/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpecialization(int id, Specialization specialization)
        {
            if (id != specialization.Id)
            {
                return BadRequest();
            }

            _context.Entry(specialization).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecializationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/specializations
        [HttpPost]
        public async Task<ActionResult<Specialization>> PostSpecialization(Specialization specialization)
        {
            _context.Specializations.Add(specialization);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpecialization", new { id = specialization.Id }, specialization);
        }

        // DELETE: api/specializations/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Specialization>> DeleteSpecialization(int id)
        {
            var specialization = await _context.Specializations.FindAsync(id);
            if (specialization == null)
            {
                return NotFound();
            }

            _context.Specializations.Remove(specialization);
            await _context.SaveChangesAsync();

            return specialization;
        }

        // GET: api/specializations
        [HttpGet("select2")]
        public async Task<ActionResult<Specialization>> select2(string search, int page)
        {
            var specializations = new List<Specialization>();
            var select2Helper = new Select2Helper();
            if (string.IsNullOrWhiteSpace(search))
            {
                specializations = (List<Specialization>) await _baseDataWork
                    .Specializations
                    .GetPaggingWithFilter(null, null, null,10, page);

                return Ok(select2Helper.CreateSpecializationResponse(specializations));
            }

            specializations = (List<Specialization>)await _baseDataWork
                .Specializations
                .GetPaggingWithFilter(null, x => x.Name.Contains(search), null,10, page);

            return Ok(select2Helper.CreateSpecializationResponse(specializations));
        }

        // POST: api/specializations
        [HttpPost("getdatatable")]
        public async Task<ActionResult<Specialization>> getdatatable([FromBody] Datatable datatable)
        {
            var total = await _baseDataWork.Employees.CountAllAsync();
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var isDescending = datatable.Order[0].Dir == "desc";

            //TODO: order by
            var specializations = await _baseDataWork.Specializations.GetWithPagging(null, pageSize, pageIndex);

            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            var mapedData = MapResults(specializations);

            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        protected IEnumerable<ExpandoObject> MapResults(IEnumerable<Specialization> results)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<Specialization>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var result in results)
            {
                var expandoObj = expandoObject.GetCopyFrom<Specialization>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;
                dictionary.Add("Buttons", dataTableHelper.GetButtons("Specialization", "Specializations", result.Id.ToString()));
                returnObjects.Add(expandoObj);
            }

            return returnObjects;
        }

        private bool SpecializationExists(int id)
        {
            return _context.Specializations.Any(e => e.Id == id);
        }
    }
}
