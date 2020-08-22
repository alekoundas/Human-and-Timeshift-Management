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
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Dynamic.Core;


namespace WebApplication.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public CompaniesApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: api/companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await _context.Companies.ToListAsync();
        }

        // GET: api/companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // PUT: api/companies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
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

        // POST: api/companies
        [HttpPost]
        public async Task<ActionResult<Company>> PostCompany(Company company)
        {
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompany", new { id = company.Id }, company);
        }

        // DELETE: api/companies/id
        [HttpDelete("{id}")]
        [Authorize(Roles = "Company_Delete")]
        public async Task<ActionResult<Company>> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return company;
        }

        // GET: api/companies/select2
        [HttpGet("select2")]
        public async Task<ActionResult<Company>> select2(string search, int page)
        {
            var companies = new List<Company>();
            var select2Helper = new Select2Helper();

            if (string.IsNullOrWhiteSpace(search))
            {
                companies = (List<Company>)await _baseDataWork.Companies
                    .GetPaggingWithFilter(null, null, null, 10, page);

                return Ok(select2Helper.CreateCompaniesResponse(companies));
            }

            companies = (List<Company>)await _baseDataWork.Companies
               .GetPaggingWithFilter(null, x => x.Title.Contains(search), null, 10, page);

            return Ok(select2Helper.CreateCompaniesResponse(companies));
        }

        [HttpGet("managecompanyemployees/{employeeId}/{companyId}/{toggleState}")]
        public async Task<ActionResult<Company>> ManageCompanyEmployees(int employeeId, int companyId, string toggleState)
        {
            var employee = await _baseDataWork.Employees.FindAsync(employeeId);
            var company = await _baseDataWork.Companies.FindAsync(companyId);

            if (employee == null || company == null)
                return NotFound();

            if (toggleState == "true")
                employee.CompanyId = companyId;
            else
                employee.CompanyId = null;

            _baseDataWork.Update(employee);
            await _baseDataWork.SaveChangesAsync();

            return company;
        }

        // POST: api/companies/Datatable
        [HttpPost("Datatable")]
        public async Task<ActionResult<Company>> Datatable([FromBody] Datatable datatable)
        {
            var total = await _baseDataWork.Companies.CountAllAsync();
            var pageSize = datatable.Length;
            var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            var isDescending = datatable.Order[0].Dir == "desc";


            //TODO: order by
            var companies = await _baseDataWork.Companies.GetWithPagging(SetOrderBy(columnName,isDescending), pageSize, pageIndex);

            var dataTableHelper = new DataTableHelper<ExpandoObject>(_securityDatawork);
            var mapedData = MapResults(companies);

            return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        private IEnumerable<ExpandoObject> MapResults(IEnumerable<Company> results)
        {
            var expandoObject = new ExpandoCopier();
            var dataTableHelper = new DataTableHelper<Company>(_securityDatawork);
            List<ExpandoObject> returnObjects = new List<ExpandoObject>();
            foreach (var result in results)
            {
                var expandoObj = expandoObject.GetCopyFrom<Company>(result);
                var dictionary = (IDictionary<string, object>)expandoObj;
                dictionary.Add("Buttons", dataTableHelper.GetButtons("Company", "Companies", result.Id.ToString()));
                returnObjects.Add(expandoObj);
            }

            return returnObjects;
        }
        private Func<IQueryable<Company>, IOrderedQueryable<Company>> SetOrderBy(string collumnName, bool isDiscending)
        {
            if (isDiscending)
                return x => x.OrderBy(collumnName+" DESC");
            else
                return x => x.OrderBy(collumnName+" ASC");
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
