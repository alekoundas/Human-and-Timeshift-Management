using Bussiness;
using Bussiness.Helpers;
using Bussiness.Service;
using DataAccess;
using DataAccess.Models.Datatable;
using DataAccess.Models.Entity;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Utilities;

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

        // GET: api/customers/deactivate/5
        [HttpGet("deactivate/{id}")]
        public async Task<ActionResult<DeactivateViewModel>> Deactivate(int id)
        {
            var response = new DeactivateViewModel();
            var company = await _baseDataWork.Companies
                .FirstOrDefaultAsync(x => x.Id == id);

            company.IsActive = !company.IsActive;
            _baseDataWork.Update(company);

            var status = await _context.SaveChangesAsync();
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

            var companyEmployees = _baseDataWork.Employees
                .Where(x => x.CompanyId == id).ToList();

            var companyCustomers = _baseDataWork.Customers
               .Where(x => x.CompanyId == id).ToList();

            companyEmployees.ForEach(x => x.CompanyId = null);
            companyCustomers.ForEach(x => x.CompanyId = null);

            _baseDataWork.UpdateRange(companyEmployees);
            _baseDataWork.UpdateRange(companyCustomers);
            _baseDataWork.Companies.Remove(company);

            var status = await _context.SaveChangesAsync();

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

            //var pageSize = datatable.Length;
            //var pageIndex = (int)Math.Ceiling((decimal)(datatable.Start / datatable.Length) + 1);
            //var columnName = datatable.Columns[datatable.Order[0].Column].Data;
            //var orderDirection = datatable.Order[0].Dir;
            //var filter = PredicateBuilder.New<Company>();
            //filter = filter.And(GetSearchFilter(datatable));
            //var includes = new List<Func<IQueryable<Company>, IIncludableQueryable<Company, object>>>();

            //var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<Company>(HttpContext);

            //if (!canShowDeactivated)
            //    filter = filter.And(x => x.IsActive == true);

            //var companies = new List<Company>();
            //if (datatable.Predicate == "CompanyIndex")
            //{
            //    companies = await _baseDataWork.Companies.GetPaggingWithFilter(
            //        SetOrderBy(columnName, orderDirection), filter, includes,
            //            pageSize, pageIndex);
            //}

            //var dataTableHelper = new DataTableHelper<ExpandoObject>();
            //var total = await _baseDataWork.Companies.CountAllAsyncFiltered(filter);
            //var mapedData = await MapResults(companies, datatable);

            //return Ok(dataTableHelper.CreateResponse(datatable, mapedData, total));
        }

        //protected async Task<IEnumerable<ExpandoObject>> MapResults(IEnumerable<Company> results, Datatable datatable)
        //{
        //    var expandoObject = new ExpandoService();
        //    var dataTableHelper = new DataTableHelper<Company>();
        //    List<ExpandoObject> returnObjects = new List<ExpandoObject>();
        //    foreach (var result in results)
        //    {
        //        var expandoObj = expandoObject.GetCopyFrom<Company>(result);
        //        var dictionary = (IDictionary<string, object>)expandoObj;

        //        if (datatable.Predicate == "CompanyIndex")
        //        {
        //            dictionary.Add("Buttons", dataTableHelper.GetButtons("Company", "Companies", result.Id.ToString()));

        //        }

        //        returnObjects.Add(expandoObj);
        //    }

        //    return returnObjects;
        //}
        //private Func<IQueryable<Company>, IOrderedQueryable<Company>> SetOrderBy(string columnName, string orderDirection)
        //{
        //    if (columnName != "")
        //        return x => x.OrderBy(columnName + " " + orderDirection.ToUpper());
        //    else
        //        return null;
        //}

        //private Expression<Func<Company, bool>> GetSearchFilterOrTrue(string searchString)
        //{
        //    var filter = PredicateBuilder.New<Company>();
        //    if (searchString != null)
        //    {
        //        filter = filter.Or(x => x.Title.Contains(searchString));
        //        filter = filter.Or(x => x.Afm.Contains(searchString));
        //    }
        //    else
        //        filter = filter.And(x => true);

        //    return filter;
        //}

        //private Expression<Func<Company, bool>> GetSearchFilter(Datatable datatable)
        //{
        //    var filter = PredicateBuilder.New<Company>();
        //    if (datatable.Search.Value != null)
        //    {
        //        foreach (var column in datatable.Columns)
        //        {
        //            if (column.Data == "Title")
        //                filter = filter.Or(x => x.Title.Contains(datatable.Search.Value));
        //            if (column.Data == "Afm")
        //                filter = filter.Or(x => x.Afm.Contains(datatable.Search.Value));
        //        }

        //    }
        //    else
        //        filter = filter.And(x => true);

        //    return filter;
        //}

        //private bool CompanyExists(int id)
        //{
        //    return _context.Companies.Any(e => e.Id == id);
        //}
    }
}
