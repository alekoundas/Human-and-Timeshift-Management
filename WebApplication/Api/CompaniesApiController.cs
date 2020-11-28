using Bussiness;
using Bussiness.Helpers;
using Bussiness.Service;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication.Api
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

        // GET: api/customers/deactivate/5
        [HttpGet("deactivate/{id}")]
        public async Task<ActionResult<DeactivateViewModel>> Deactivate(int id)
        {
            var response = new DeactivateViewModel();
            var company = await _baseDataWork.Companies
                .FirstOrDefaultAsync(x => x.Id == id);

            company.IsActive = !company.IsActive;
            _baseDataWork.Update(company);

            var status = await _baseDataWork.SaveChangesAsync();
            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Η εταιρία " +
                    company.Title +
                    (company.IsActive ? " ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : " ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Η εταιρία " +
                     company.Title +
                    " ΔΕΝ " +
                    (company.IsActive ? "ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : "ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία";
            }

            response.ResponseTitle = "Αλλαγή κατάστασης εταιρίας";
            response.Entity = company;

            return Ok(response);
        }

        // DELETE: api/companies/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteViewModel>> DeleteCompany(int id)
        {
            var response = new DeleteViewModel();
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
                return NotFound();

            _baseDataWork.Companies.Remove(company);

            var status = await _baseDataWork.SaveChangesAsync();

            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Η εταιρία " +
                    company.Title +
                    " διαγράφηκε με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Η εταιρία " +
                    company.Title +
                    " ΔΕΝ διαγράφηκε!";
            }

            response.ResponseTitle = "Διαγραφή εταιρίας";
            response.Entity = company;
            return response;
        }

        // GET: api/companies/select2
        [HttpGet("select2")]
        public async Task<ActionResult<Company>> select2(string search, int page)
        {
            var companies = new List<Company>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<Company>();
            filter = filter.And(x => true);
            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<Company>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);

            if (string.IsNullOrWhiteSpace(search))
                companies = await _baseDataWork.Companies
                    .GetPaggingWithFilter(null, null, null, 10, page);
            else
            {
                filter = filter.And(x => x.Title.Contains(search));

                companies = await _baseDataWork.Companies
                    .GetPaggingWithFilter(null, filter, null, 10, page);
            }
            var total = await _baseDataWork.Companies.CountAllAsyncFiltered(filter);
            var hasMore = (page * 10) < total;

            return Ok(select2Helper.CreateCompaniesResponse(companies, hasMore));

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
            var results = (await new DataTableService(datatable, _baseDataWork, HttpContext)
                .ConvertData<Company>())
                .CompleteResponse<Company>();
            return Ok(results);
        }
    }
}
