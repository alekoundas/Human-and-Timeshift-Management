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

    [Route("api/contracts")]
    [ApiController]
    public class ContractApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public ContractApiController(BaseDbContext BaseDbContext, SecurityDbContext SecurityDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _securityDatawork = new SecurityDataWork(SecurityDbContext);
        }

        // GET: api/Contracts/deactivate/5
        [HttpGet("deactivate/{id}")]
        public async Task<ActionResult<DeactivateViewModel>> Deactivate(int id)
        {
            var response = new DeactivateViewModel();
            var contract = await _baseDataWork.Contracts
                .FirstOrDefaultAsync(x => x.Id == id);

            contract.IsActive = !contract.IsActive;
            _baseDataWork.Update(contract);

            var status = await _baseDataWork.SaveChangesAsync();
            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Η σύμβαση " +
                    contract.Title +
                    (contract.IsActive ? " ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : " ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Η σύμβαση " +
                     contract.Title +
                    " ΔΕΝ " +
                    (contract.IsActive ? "ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : "ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία";
            }

            response.ResponseTitle = "Αλλαγή κατάστασης σύμβασης";
            response.Entity = contract;

            return Ok(response);
        }

        // DELETE: api/Contracts/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteViewModel>> Delete(int id)
        {
            var response = new DeleteViewModel();
            var contract = await _context.Contracts.FindAsync(id);

            if (contract == null)
                return NotFound();


            _baseDataWork.Contracts.Remove(contract);

            var status = await _baseDataWork.SaveChangesAsync();

            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Η σύμβαση " +
                    contract.Title +
                    " διαγράφηκε με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Η σύμβαση " +
                    contract.Title +
                    " ΔΕΝ διαγράφηκε!";
            }

            response.ResponseTitle = "Διαγραφή σύμβασης";
            response.Entity = contract;
            return response;
        }

        // GET: api/Contracts/select2
        [HttpGet("select2")]
        public async Task<ActionResult<Contract>> select2(string search, int page)
        {
            var contracts = new List<Contract>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<Contract>();
            filter = filter.And(x => true);
            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<Contract>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);

            if (string.IsNullOrWhiteSpace(search))
                contracts = await _baseDataWork.Contracts
                    .GetPaggingWithFilter(null, null, null, 10, page);
            else
            {
                filter = filter.And(x => x.Title.Contains(search));

                contracts = await _baseDataWork.Contracts
                    .GetPaggingWithFilter(null, filter, null, 10, page);
            }
            var total = await _baseDataWork.Contracts.CountAllAsyncFiltered(filter);
            var hasMore = (page * 10) < total;

            return Ok(select2Helper.CreateContractsResponse(contracts, hasMore));

        }

        // POST: api/Contracts/Datatable
        [HttpPost("Datatable")]
        public async Task<ActionResult<Company>> Datatable([FromBody] Datatable datatable)
        {
            var results = (await new DataTableService(datatable, _baseDataWork, HttpContext)
                .ConvertData<Contract>())
                .CompleteResponse<Contract>();
            return Ok(results);
        }
    }
}
