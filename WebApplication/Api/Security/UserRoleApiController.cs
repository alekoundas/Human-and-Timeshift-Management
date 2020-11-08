using Bussiness;
using DataAccess;
using DataAccess.Models.Identity;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        private readonly SecurityDataWork _securityDatawork;
        public UserRoleApiController(SecurityDbContext securityDbContext, BaseDbContext baseDbContext)
        {
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
            var userRolesToDelete = await _securityDatawork.ApplicationUserRoles
                 .GetUserRolesToDelete(viewModel.WorkPlaceIdsToDelete, viewModel.UserId);

            _securityDatawork.ApplicationUserRoles.RemoveRange(userRolesToDelete);
            await _securityDatawork.SaveChangesAsync();
            foreach (var workPlaceValue in viewModel.WorkPlacesValues)
            {

                //If role exists in db
                if (_securityDatawork.ApplicationRoles
                    .IsWorkPlaceIdAnExistingRole(workPlaceValue.NewWorkPlaceId.ToString()))
                {
                    var newRole = await _securityDatawork.ApplicationRoles
                        .FirstOrDefaultAsync(x => x.WorkPlaceId == workPlaceValue.NewWorkPlaceId.ToString());

                    //If user has edited an WorkPlace Role
                    if (workPlaceValue.ExistingWorkPlaceId != workPlaceValue.NewWorkPlaceId.ToString() && workPlaceValue.ExistingWorkPlaceId != "")
                    {
                        var existingRole = await _securityDatawork.ApplicationUsers
                            .GetRoleByWorkPlaceAndUser(workPlaceValue.ExistingWorkPlaceId, viewModel.UserId);
                        var existingUserRole = await _securityDatawork.ApplicationUserRoles
                            .FirstOrDefaultAsync(x => x.RoleId == existingRole.Id && x.UserId == viewModel.UserId);
                        var newUserRole = new ApplicationUserRole();

                        _securityDatawork.ApplicationUserRoles.Remove(existingUserRole);
                        await _securityDatawork.SaveChangesAsync();

                        newUserRole.RoleId = newRole.Id;
                        newUserRole.UserId = viewModel.UserId;
                        _securityDatawork.ApplicationUserRoles.Add(newUserRole);

                        await _securityDatawork.SaveChangesAsync();

                    }
                    else
                    {
                        _securityDatawork.ApplicationUserRoles.Add(new ApplicationUserRole
                        {
                            UserId = viewModel.UserId,
                            RoleId = newRole.Id
                        });
                        await _securityDatawork.SaveChangesAsync();
                    }

                    //if (await _securityDatawork.ApplicationUsers
                    //    .HasWorkPlaceRole(workPlaceValue.NewWorkPlaceId.ToString(), viewModel.UserId))

                }
                else
                {
                    //Create Role then
                    var workPlace = await _baseDataWork.WorkPlaces
                    .FirstOrDefaultAsync(x => x.Id == workPlaceValue.NewWorkPlaceId);

                    var role = new ApplicationRole()
                    {
                        Name = "Specific_WorkPlace_" + workPlaceValue.NewWorkPlaceId.ToString(),
                        WorkPlaceId = workPlaceValue.NewWorkPlaceId.ToString(),
                        WorkPlaceName = workPlace.Title
                    };

                    _securityDatawork.ApplicationRoles.Add(role);
                    await _securityDatawork.SaveChangesAsync();

                    _securityDatawork.ApplicationUserRoles.Add(new ApplicationUserRole
                    {
                        UserId = viewModel.UserId,
                        RoleId = role.Id
                    });
                    await _securityDatawork.SaveChangesAsync();

                }
            }


            await _securityDbContext.SaveChangesAsync();

            var applicationWorkPlaceRoles = await _securityDatawork.ApplicationRoles
              .GetWorkPlaceRolesByUserId(viewModel.UserId);

            return Ok(applicationWorkPlaceRoles
                .Select(x => new WorkPlaceRoleValues
                {
                    WorkPlaceId = x.WorkPlaceId,
                    Name = x.WorkPlaceName
                }).ToList());

        }
    }
}
