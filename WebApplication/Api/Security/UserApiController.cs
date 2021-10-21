using Bussiness;
using Bussiness.Helpers;
using Bussiness.Service;
using DataAccess;
using DataAccess.Libraries.Datatable;
using DataAccess.Libraries.Select2;
using DataAccess.Models.Security;
using DataAccess.ViewModels;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApplication.Api.Security
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly BaseDatawork _baseDataWork;
        private readonly SecurityDataWork _securityDatawork;
        public UserApiController(
            UserManager<ApplicationUser> userManager,
            BaseDbContext BaseDbContext,
            SecurityDbContext securityDbContext)
        {
            _securityDatawork = new SecurityDataWork(securityDbContext);
            _baseDataWork = new BaseDatawork(BaseDbContext);
            _userManager = userManager;
        }

        [HttpPost("resetpassword/{userId}")]
        public async Task<ActionResult<ApplicationUser>> ResetPassword(string userId)
        {
            var user = await _securityDatawork.ApplicationUsers
                   .FirstOrDefaultAsync(x => x.Id == userId);


            user.HasToChangePassword = true;

            var status = await _userManager.UpdateAsync(user);
            //if (!status.Succeeded)
            //{
            //}

            return Ok(new { });
        }


        [HttpPost("datatable")]
        public async Task<ActionResult<ApplicationUser>> Datatable([FromBody] Datatable datatable)
        {
            var results = (await new DataTableService(datatable, _baseDataWork, HttpContext, _securityDatawork)
                .ConvertData<ApplicationUser>())
                .CompleteResponse<ApplicationUser>();

            return Ok(results);
        }



        // DELETE: api/specializations/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteViewModel>> Delete(string id)
        {
            var response = new DeleteViewModel();
            var user = await _securityDatawork.ApplicationUsers
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound();

            _securityDatawork.ApplicationUsers.Remove(user);
            var status = await _securityDatawork.SaveChangesAsync();

            if (status >= 1)
                response.ResponseBody = "Ο χρήστης" +
                    user.FirstName + " " +
                    user.LastName +
                    " διαγράφηκε με επιτυχία";
            else
                response.ResponseBody = "Ωχ! Ο χρήστης" +
                    user.FirstName + " " +
                    user.LastName +
                    " ΔΕΝ διαγράφηκε!";


            response.ResponseTitle = "Διαγραφή χρήστη";
            response.Entity = user;
            return response;
        }

        // POST: api/users/select2
        [HttpPost("select2")]
        public async Task<ActionResult<ApplicationUser>> Select2([FromBody] Select2 select2)
        {
            var select2Helper = new Select2Helper();
            var filter = PredicateBuilder.New<ApplicationUser>();
            filter = filter.And(x => true);

            if (select2.FromEntityIdString?.Length > 0)
                filter = filter.And(x => x.Id == select2.FromEntityIdString);

            if (select2.ExistingIdsString?.Count > 0)
                foreach (var id in select2.ExistingIdsString)
                    filter = filter.And(x => x.Id != id);

            if (!string.IsNullOrWhiteSpace(select2.Search))
                filter = filter.And(x => x.FirstName.Contains(select2.Search) || x.LastName.Contains(select2.Search));

            var entities = await _securityDatawork.ApplicationUsers
                .GetPaggingWithFilter(null, filter, null, 10, select2.Page);

            var total = await _securityDatawork.ApplicationUsers.CountAllAsyncFiltered(filter);
            var hasMore = (select2.Page * 10) < total;

            return Ok(select2Helper.CreateUsersResponse(entities, hasMore));
        }

    }
}
