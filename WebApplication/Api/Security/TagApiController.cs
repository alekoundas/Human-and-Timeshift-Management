using Bussiness;
using DataAccess;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Api.Security
{
    [Route("api/tags")]
    [ApiController]
    public class TagApiController : ControllerBase
    {
        private SecurityDbContext _securityDbContext;
        private BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public TagApiController(SecurityDbContext securityDbContext, BaseDbContext baseDbContext)
        {
            _securityDbContext = securityDbContext;
            _securityDatawork = new SecurityDataWork(securityDbContext);
            _baseDataWork = new BaseDatawork(baseDbContext);

        }

        //POST api/tags/selectize
        [HttpPost("selectize")]
        public async Task<ActionResult> UpdateUserTags(ApiSelectizeGet viewModel)
        {
            //Delete old user tags - so i can add new

            var aaaa = await _securityDbContext
                .ApplicationTags
                .Where(x => x.Title.Contains(viewModel.Search))
                .ToListAsync();
            var response = new List<object>();
            aaaa.ForEach(x => response.Add(new { value = x.Title }));
            return Ok(response);
        }
    }
}
