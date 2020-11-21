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
using System.Threading.Tasks;

namespace WebApplication.Api
{

    [Route("api/contracttypes")]
    [ApiController]
    public class ContractTypeApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public ContractTypeApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: api/ContractTypes/deactivate/5
        [HttpGet("deactivate/{id}")]
        public async Task<ActionResult<DeactivateViewModel>> Deactivate(int id)
        {
            var response = new DeactivateViewModel();
            var ContractType = await _baseDataWork.ContractTypes
                .FirstOrDefaultAsync(x => x.Id == id);

            ContractType.IsActive = !ContractType.IsActive;
            _baseDataWork.Update(ContractType);

            var status = await _context.SaveChangesAsync();
            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Η σύμβαση " +
                    ContractType.Name +
                    (ContractType.IsActive ? " ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : " ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Η σύμβαση " +
                     ContractType.Name +
                    " ΔΕΝ " +
                    (ContractType.IsActive ? "ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : "ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία";
            }

            response.ResponseTitle = "Αλλαγή κατάστασης σύμβασης";
            response.Entity = ContractType;

            return Ok(response);
        }

        // DELETE: api/ContractTypes/id
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<DeleteViewModel>> Delete(int id)
        //{
        //    var response = new DeleteViewModel();
        //    var ContractType = await _context.ContractTypes.FindAsync(id);

        //    if (ContractType == null)
        //        return NotFound();

        //    var companyEmployees = _baseDataWork.Employees
        //        .Where(x => x.CompanyId == id).ToList();

        //    var companyCustomers = _baseDataWork.Customers
        //       .Where(x => x.CompanyId == id).ToList();

        //    companyEmployees.ForEach(x => x.CompanyId = null);
        //    companyCustomers.ForEach(x => x.CompanyId = null);

        //    _baseDataWork.UpdateRange(companyEmployees);
        //    _baseDataWork.UpdateRange(companyCustomers);
        //    _baseDataWork.Companies.Remove(ContractType);

        //    var status = await _context.SaveChangesAsync();

        //    if (status >= 1)
        //    {
        //        response.IsSuccessful = true;
        //        response.ResponseBody = "Η εταιρία " +
        //            company.Title +
        //            " διαγράφηκε με επιτυχία.";
        //    }
        //    else
        //    {
        //        response.IsSuccessful = false;
        //        response.ResponseBody = "Ωχ! Η εταιρία " +
        //            company.Title +
        //            " ΔΕΝ διαγράφηκε!";
        //    }

        //    response.ResponseTitle = "Διαγραφή εταιρίας";
        //    response.Entity = company;
        //    return response;
        //}

        // GET: api/ContractTypes/select2
        [HttpGet("select2")]
        public async Task<ActionResult<ContractType>> select2(string search, int page)
        {
            var ContractTypes = new List<ContractType>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<ContractType>();
            filter = filter.And(x => true);
            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<ContractType>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);

            if (string.IsNullOrWhiteSpace(search))
                ContractTypes = await _baseDataWork.ContractTypes
                    .GetPaggingWithFilter(null, null, null, 10, page);
            else
            {
                filter = filter.And(x => x.Name.Contains(search));

                ContractTypes = await _baseDataWork.ContractTypes
                    .GetPaggingWithFilter(null, filter, null, 10, page);
            }
            var total = await _baseDataWork.ContractTypes.CountAllAsyncFiltered(filter);
            var hasMore = (page * 10) < total;

            return Ok(select2Helper.CreateContractTypesResponse(ContractTypes, hasMore));

        }

        // POST: api/ContractTypes/Datatable
        [HttpPost("Datatable")]
        public async Task<ActionResult<Company>> Datatable([FromBody] Datatable datatable)
        {
            var results = (await new DataTableService(datatable, _baseDataWork, HttpContext)
                .ConvertData<ContractType>())
                .CompleteResponse<ContractType>();
            return Ok(results);
        }
    }
}
