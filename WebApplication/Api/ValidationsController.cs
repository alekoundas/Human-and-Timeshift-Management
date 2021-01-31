using DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Api
{
    [Route("api/validations")]
    [ApiController]
    public class ValidationsController : ControllerBase
    {
        private BaseDbContext _context;
        private BaseDatawork _baseDataWork;
        public ValidationsController(BaseDbContext BaseDbContext)
        {
            _context = BaseDbContext;
            _baseDataWork = new BaseDatawork(BaseDbContext);
        }







    }
}
