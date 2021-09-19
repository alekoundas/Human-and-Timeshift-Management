using Bussiness;
using DataAccess;
using DataAccess.Models.Security;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Api.Security
{
    [Route("api/userrole")]
    [AllowAnonymous]
    [ApiController]
    public class UserRoleApiController : ControllerBase
    {
        private SecurityDbContext _securityDbContext;
        private BaseDatawork _baseDataWork;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SecurityDataWork _securityDatawork;
        public UserRoleApiController(
            SecurityDbContext securityDbContext,
            BaseDbContext baseDbContext,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _securityDbContext = securityDbContext;
            _securityDatawork = new SecurityDataWork(securityDbContext);
            _baseDataWork = new BaseDatawork(baseDbContext);

        }
        // POST api/userrole/delete/userId/roleId
        [HttpPost("delete/{userId}/{roleId}")]
        public async Task<ActionResult> Delete(string userId, string roleId)
        {
            var userRoleToDelete = await _securityDbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId);
            if (userRoleToDelete == null)
                return BadRequest();
            var resault = _securityDbContext.UserRoles.Remove(userRoleToDelete);
            await _securityDbContext.SaveChangesAsync();
            return Ok();
        }

        // POST api/userrole/delete/userId/roleId
        [HttpPost("add/{userId}/{roleId}")]
        public async Task<ActionResult> Add(string userId, string roleId)
        {
            var userRoleToAdd = await _securityDbContext.UserRoles
                .FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId);

            if (userRoleToAdd != null)
                return BadRequest();

            _securityDbContext.UserRoles.Add(new ApplicationUserRole()
            {
                UserId = userId,
                RoleId = roleId,

            });
            await _securityDbContext.SaveChangesAsync();

            return Ok(new { userid = userId });
        }

        // POST api/userrole/updateworkplaceroles
        [HttpPost("updateworkplaceroles")]
        public async Task<ActionResult> UpdateWorkPlaceRoles([FromBody] ApiUpdateWorkPlaceRoles viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.UserId);

            //Delete other roles from user
            var roles = (await _userManager.GetRolesAsync(user))
                .Where(x => x.Contains("Specific_WorkPlace_"))
                .ToList();
            await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
            await _userManager.UpdateAsync(user);


            foreach (var workPlaceId in viewModel.WorkPlaceIds)
            {
                //If role doesnt exists in db, create it
                if (!await _roleManager.RoleExistsAsync("Specific_WorkPlace_" + workPlaceId))
                {
                    var workPlace = _baseDataWork.WorkPlaces
                        .Query
                        .First(x => x.Id == workPlaceId);

                    await _roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = "Specific_WorkPlace_" + workPlaceId.ToString(),
                        WorkPlaceId = workPlaceId.ToString(),
                        WorkPlaceName = workPlace.Title
                    });
                }
                //Assign it
                await _userManager.AddToRoleAsync(user, "Specific_WorkPlace_" + workPlaceId);
            }

            await _userManager.UpdateAsync(user);



            roles = (await _userManager.GetRolesAsync(user))
                .Where(x => x.Contains("Specific_WorkPlace_"))
                .Select(x => x.Split('_')[2])
                .ToList();

            return Ok(roles);
        }
    }
}
