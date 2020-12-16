using Bussiness.Service;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApplication.Api
{
    [Route("api/Projections")]
    [ApiController]
    public class ProjectionsApiController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public ProjectionsApiController(BaseDbContext BaseDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }

        // POST: api/companies/Datatable
        [HttpPost("Datatable")]
        public async Task<ActionResult<Employee>> Datatable([FromBody] Datatable datatable)
        {
            var results = (await new DataTableService(datatable, _baseDataWork, HttpContext)
                .ConvertData<object>("Projection"))
                .CompleteResponse<Employee>();
            return Ok(results);
        }
    }
}
