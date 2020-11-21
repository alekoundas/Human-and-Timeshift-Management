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

    [Route("api/contractMemberships")]
    [ApiController]
    public class ContractMembershipApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public ContractMembershipApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: api/ContractMembership/deactivate/5
        [HttpGet("deactivate/{id}")]
        public async Task<ActionResult<DeactivateViewModel>> Deactivate(int id)
        {
            var response = new DeactivateViewModel();
            var ContractType = await _baseDataWork.ContractMemberships
                .FirstOrDefaultAsync(x => x.Id == id);

            ContractType.IsActive = !ContractType.IsActive;
            _baseDataWork.Update(ContractType);

            var status = await _context.SaveChangesAsync();
            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Η ιδιότητα σύμβασης " +
                    ContractType.Name +
                    (ContractType.IsActive ? " ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : " ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Η ιδιότητα σύμβασης " +
                     ContractType.Name +
                    " ΔΕΝ " +
                    (ContractType.IsActive ? "ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : "ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία";
            }

            response.ResponseTitle = "Αλλαγή κατάστασης σύμβασης";
            response.Entity = ContractType;

            return Ok(response);
        }

        // DELETE: api/ContractMembership/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteViewModel>> Delete(int id)
        {
            var response = new DeleteViewModel();
            var contractMembership = await _context.ContractMemberships.FindAsync(id);

            if (contractMembership == null)
                return NotFound();


            _baseDataWork.ContractMemberships.Remove(contractMembership);

            var status = await _context.SaveChangesAsync();

            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Η  κατάστασης σύμβασης " +
                    contractMembership.Name +
                    " διαγράφηκε με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Η κατάστασης σύμβασης " +
                    contractMembership.Name +
                    " ΔΕΝ διαγράφηκε!";
            }

            response.ResponseTitle = "Διαγραφή κατάστασης σύμβασης";
            response.Entity = contractMembership;
            return response;
        }

        // GET: api/ContractMembership/select2
        [HttpGet("select2")]
        public async Task<ActionResult<ContractMembership>> select2(string search, int page)
        {
            var ContractMembership = new List<ContractMembership>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<ContractMembership>();
            filter = filter.And(x => true);
            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<ContractMembership>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);

            if (string.IsNullOrWhiteSpace(search))
                ContractMembership = await _baseDataWork.ContractMemberships
                    .GetPaggingWithFilter(null, null, null, 10, page);
            else
            {
                filter = filter.And(x => x.Name.Contains(search));

                ContractMembership = await _baseDataWork.ContractMemberships
                    .GetPaggingWithFilter(null, filter, null, 10, page);
            }
            var total = await _baseDataWork.ContractMemberships.CountAllAsyncFiltered(filter);
            var hasMore = (page * 10) < total;

            return Ok(select2Helper.CreateContractMembershipResponse(ContractMembership, hasMore));

        }

        // POST: api/ContractMembership/Datatable
        [HttpPost("Datatable")]
        public async Task<ActionResult<ContractMembership>> Datatable([FromBody] Datatable datatable)
        {
            var results = (await new DataTableService(datatable, _baseDataWork, HttpContext)
                .ConvertData<ContractMembership>())
                .CompleteResponse<ContractMembership>();
            return Ok(results);
        }
    }
}
