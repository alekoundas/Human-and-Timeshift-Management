using Bussiness;
using Bussiness.Helpers;
using Bussiness.Service;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Libraries.Select2;
using DataAccess.Models.Security;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication.Api.Security
{
    [Route("api/LogEntities")]
    [ApiController]
    public class LogEntitiesApiController : ControllerBase
    {
        private SecurityDbContext _context;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public LogEntitiesApiController(
            SecurityDbContext securityDbContext, 
            BaseDbContext baseDbContext)
        {
            _context = securityDbContext;
            _securityDatawork = new SecurityDataWork(securityDbContext);
            _baseDataWork = new BaseDatawork(baseDbContext);

        }

        // GET: api/LogEntities/deactivate/5
        [HttpGet("deactivate/{id}")]
        public async Task<ActionResult<DeactivateViewModel>> Deactivate(int id)
        {
            var response = new DeactivateViewModel();
            var specialization = await _securityDatawork.LogEntities
                .FirstOrDefaultAsync(x => x.Id == id);

            specialization.IsActive = !specialization.IsActive;
            _securityDatawork.Update(specialization);

            var status = await _securityDatawork.SaveChangesAsync();
            if (status >= 1)
            {
                response.IsSuccessful = true;
                response.ResponseBody = "Η Log οντότητα" +
                    specialization.Title +
                    (specialization.IsActive ? " ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : " ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία.";
            }
            else
            {
                response.IsSuccessful = false;
                response.ResponseBody = "Ωχ! Η Log οντότητα" +
                     specialization.Title +
                    " ΔΕΝ " +
                    (specialization.IsActive ? "ΕΝΕΡΓΟΠΟΙΗΘΗΚΕ " : "ΑΠΕΝΕΡΓΟΠΟΙΗΘΗΚΕ ") +
                    "με επιτυχία";
            }

            response.ResponseTitle = "Αλλαγή κατάστασης Log οντότητας";
            response.Entity = specialization;

            return Ok(response);
        }

        // DELETE: api/LogTypes/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteViewModel>> Delete(int id)
        {
            var response = new DeleteViewModel();
            var specialization = await _context.LogEntities.FindAsync(id);
            if (specialization == null)
                return NotFound();

            _context.LogEntities.Remove(specialization);
            var status = await _securityDatawork.SaveChangesAsync();

            if (status >= 1)
                response.ResponseBody = "Η Log οντότητα" +
                    specialization.Title +
                    " διαγράφηκε με επιτυχία";
            else
                response.ResponseBody = "Ωχ! Η Log οντότητα" +
                    specialization.Title +
                    " ΔΕΝ διαγράφηκε!";


            response.ResponseTitle = "Διαγραφή ειδος Log";
            response.Entity = specialization;
            return response;
        }

        // POST: api/workplaces/select2
        [HttpPost("select2")]
        public async Task<ActionResult<LogEntity>> Select2([FromBody] Select2 select2)
        {
            var workPlaces = new List<LogEntity>();
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<LogEntity>();
            filter = filter.And(x => true);
            var canShowDeactivated = DeactivateService.CanShowDeactivatedFromUser<LogEntity>(HttpContext);

            if (!canShowDeactivated)
                filter = filter.And(x => x.IsActive == true);

            if (select2.FromEntityId != 0)
                filter = filter.And(x => x.Id == select2.FromEntityId);

            if (select2.ExistingIds?.Count > 0)
                foreach (var workPlaceId in select2.ExistingIds)
                    filter = filter.And(x => x.Id != workPlaceId);

            if (!string.IsNullOrWhiteSpace(select2.Search))
                filter = filter.And(x => x.Title.Contains(select2.Search));

            workPlaces = await _securityDatawork.LogEntities
                .GetPaggingWithFilter(null, filter, null, 10, select2.Page);

            var total = await _securityDatawork.LogEntities.CountAllAsyncFiltered(filter);
            var hasMore = (select2.Page * 10) < total;

            return Ok(select2Helper.CreateWorkplacesResponse(workPlaces, hasMore));
        }

        // POST: api/datatable
        [HttpPost("datatable")]
        public async Task<ActionResult<LogEntity>> Datatable([FromBody] Datatable datatable)
        {
            var results = (await new DataTableService(datatable, _baseDataWork, HttpContext, _securityDatawork)
                .ConvertData<LogEntity>())
                .CompleteResponse<LogEntity>();

            return Ok(results);
        }
    }
}
