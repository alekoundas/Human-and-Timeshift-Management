using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bussiness;
using DataAccess;
using DataAccess.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication.Api.Security
{
    [Route("api/userrole")]
    [AllowAnonymous]
    [ApiController]
    public class UserRoleDataController : ControllerBase
    {
        private SecurityDbContext _securityDbContext;
        public UserRoleDataController(SecurityDbContext dbContext)
        {
            _securityDbContext = dbContext;
        }
        // POST api/userrole/delete/userId/roleId
        [HttpPost("delete/{userId}/{roleId}")]
        public async Task<ActionResult> Delete(string userId, string roleId)
        {
            var userRoleToDelete = await _securityDbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId);
            if (userRoleToDelete ==null)
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

            _securityDbContext.UserRoles.Add(new IdentityUserRole<string>() { 
                UserId = userId,
                RoleId=roleId});
            await _securityDbContext.SaveChangesAsync();

            return Ok(new { userid = userId });
        }
    }
}
